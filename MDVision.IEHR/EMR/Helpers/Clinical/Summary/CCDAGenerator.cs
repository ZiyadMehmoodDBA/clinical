using System;
using System.Collections.Generic;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.IEHR.EMR.Model.Clinical;
using System.Text;
using System.Xml;
using System.Linq;
using System.IO;
using System.Xml.Xsl;
using System.Xml.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Summary
{
    public class StringWriterUtf8 : System.IO.StringWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
    public class CCDAGenrator
    {
        private static CCDAGenrator _instance = null;

        public static CCDAGenrator Instance()
        {
            if (_instance == null)
                _instance = new CCDAGenrator();
            return _instance;
        }

        public enum DocumentTemplateType : int
        {
            ClinicalSummary,
            TransferSummary,
            DataPortability,
            DischargeSummary
        }

        public static Dictionary<string, string> StaticCcdaData = new Dictionary<string, string>();

        public static DocumentTemplateType DocumentTemplate { get; set; }

        public static Dictionary<string, string> GetXmlValues()
        {
            string xmlFilePath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\StaticCCDAData.xml";

            Dictionary<string, string> dictionaryObject = new Dictionary<string, string>();
            try
            {
                if (File.Exists(xmlFilePath))
                {
                    XmlDocument ccdDocument = new XmlDocument();
                    ccdDocument.Load(xmlFilePath);
                    XmlNodeList xmlNodelist = ccdDocument.SelectNodes("section");
                    foreach (XmlElement node in xmlNodelist[0].ChildNodes)
                    {
                        dictionaryObject.Add(node.Attributes[0].Value, node.Attributes[1].Value);
                    }
                }
                return dictionaryObject;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string MakeCcda(CCDADataModel objCCDAModel, Int64 noteId, Int64 providerId, Int64 patientId, string StreamType, CCDAGenrator.DocumentTemplateType documentType)
        {
            DocumentTemplate = (DocumentTemplateType)Enum.Parse(typeof(DocumentTemplateType), "ClinicalSummary");
            StaticCcdaData = GetXmlValues();


            ClinicalDocument ccda = new ClinicalDocument();

            MakeCcdaHeader(ccda, objCCDAModel);

            MakeCcdaBody(ccda, objCCDAModel, documentType);
            string output = "";
            output = GenerateXmlDocument(ccda);

            if (StreamType.ToLower() == "html")
            {
                output = GetHtmlFromXml(noteId, providerId, patientId, output);
            }
            else if (StreamType.ToLower() == "pdf")
            {
                output = GetHtmlFromXml(noteId, providerId, patientId, output);
                output = GetPDFFromHTML(output);
            }

            return output;
        }

        public static string DecryptRawXml(string xmlData, bool dataIsEncrypted, string password)
        {
            AESAlgorithm aes = new AESAlgorithm();
            if (!string.IsNullOrWhiteSpace(xmlData))
            {
                byte[] ct = Convert.FromBase64String(xmlData);
                xmlData = Encoding.UTF8.GetString(ct);
                if (dataIsEncrypted)
                {
                    xmlData = aes.Decrypt(xmlData, password);
                }
            }

            return xmlData;
        }

        public static string GetHtmlFromXml(Int64 noteId, Int64 providerId, Int64 patientId, string xmlContent, bool isDelete = true, string orignalXML = "")
        {
            var folderPath = System.Web.HttpContext.Current.Server.MapPath(MDVision.IEHR.Common.AppConfig.ImagePath);
            var htmlfileName = string.Format("CCDA_{0}_{1}_{2}_{3}.html", noteId, providerId, patientId, MDVision.IEHR.Common.AppConfig.AppUserId);
            var xmlfileName = string.Format("CCDA_{0}_{1}_{2}_{3}.xml", noteId, providerId, patientId, MDVision.IEHR.Common.AppConfig.AppUserId);
            var OrignalfileName = string.Format("Orignal_{0}_{1}_{2}_{3}.xml", noteId, providerId, patientId, MDVision.IEHR.Common.AppConfig.AppUserId);
            var htmlFilePath = string.Format(@"{0}\{1}", folderPath, htmlfileName);
            var xmlFilePath = string.Format(@"{0}\{1}", folderPath, xmlfileName);
            var orignalFilePath = string.Format(@"{0}\{1}", folderPath, OrignalfileName);

            //Delete Xml & Html File
            if (isDelete)
            {
                if (File.Exists(xmlFilePath))
                    File.Delete(xmlFilePath);
                if (File.Exists(htmlFilePath))
                    File.Delete(htmlFilePath);
            }

            //Save XML Content
            File.WriteAllText(xmlFilePath, xmlContent);
            if (orignalXML != "")
                File.WriteAllText(orignalFilePath, orignalXML);

            // Create a resolver with default credentials.
            XmlUrlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;
            // transform Xml file to HTML
            XslCompiledTransform transform = new XslCompiledTransform();
            XsltSettings settings = new XsltSettings() { EnableDocumentFunction = true };
            // load up the stylesheet
            var stylesheetPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\EverestPoC.xsl";
            transform.Load(stylesheetPath, settings, resolver);
            // perform the transformation
            if (orignalXML == "")
                transform.Transform(xmlFilePath, htmlFilePath);
            else
            {
                transform.Transform(orignalFilePath, htmlFilePath);
                File.Delete(OrignalfileName);
            }
            string content = File.ReadAllText(htmlFilePath);
            content = content.Replace("�", " ");

            return content;
        }


        private static string GenerateXmlDocument(ClinicalDocument document)
        {
            string xml = string.Empty;
            XmlSerializer serializer = new XmlSerializer(typeof(ClinicalDocument));

            using (StringWriterUtf8 strWriter = new StringWriterUtf8())
            {
                serializer.Serialize(strWriter, document);
                xml = Convert.ToString(strWriter);
            }
            return xml;
        }

        public static byte[] ReadWholeArray(Stream stream)
        {

            byte[] data = new byte[stream.Length];

            int offset = 0;
            int remaining = data.Length;
            while (remaining > 0)
            {
                int read = stream.Read(data, offset, remaining);
                if (read <= 0)
                    throw new EndOfStreamException
                        (String.Format("End of stream reached with {0} bytes left to read", remaining));
                remaining -= read;
                offset += read;
            }

            return data;
        }

        public static string GetPDFFromHTML(string htmlContent)
        {
            #region Style String
            string style = @"<style type=""text/css"">
                    body {
                        color: #003366;
                        background-color: #FFFFFF;
                        font-family: Verdana, Tahoma, sans-serif;
                        font-size: 5px;
                    }

                    a {
                        color: #003366;
                        background-color: #FFFFFF;
                    }

                    h1 {
                        font-size: 12pt;
                        font-weight: bold;
                    }

                    h2 {
                        font-size: 11pt;
                        font-weight: bold;
                    }

                    h3 {
                        font-size: 10pt;
                        font-weight: bold;
                    }

                    h4 {
                        font-size: 8pt;
                        font-weight: bold;
                    }

                    h1, .h1, h2, .h2, h3, .h3 {
                        margin-top: 20px;
                        margin-bottom: 10px;
                    }

                    table {
                        color:#003366;
                        line-height: 10pt;
                        width: 100%;
                        display: table;
                        border-collapse: separate;
                        border-spacing: 2px;
                        font-size:11px;
                        
                    }

                    th {
                        background-color: #ffd700;
                    }

                    td {
                        padding: 0.1cm 0.2cm;
                        vertical-align: top;
                        background-color: #ffffcc;
                    }

                    .h1center {
                        font-size: 12pt;
                        font-weight: bold;
                        text-align: center;
                        width: 80%;
                    }

                    .header_table {
                        border: 1px solid #00008b;
                    }

                    .td_label {
                        font-weight: bold;
                        color: white;
                    }

                    .td_header_role_name {
                        width: 20%;
                        background-color: #3399ff;
                    }

                    ul{
                          font-size:11px;
                    }

                    .td_header_role_value {
                        width: 80%;
                        background-color: #ccccff;
                    }

                    .Bold {
                        font-weight: bold;
                    }

                    .Italics {
                        font-style: italic;
                    }

                    .Underline {
                        text-decoration: underline;
                    }
                </style>";
            #endregion Style String

            Byte[] bytes;
            htmlContent = htmlContent.Replace("<br>", "<br/>").Replace(@"<hr size=""2"" align=""left"" color=""teal"">", @"<hr align=""left"" color=""teal"" size=""2"" />").Replace(@"<hr align=""left"" color=""teal"" size=""2"">", @"<hr align=""left"" color=""teal"" size=""2"" />").Replace(@"<META http-equiv=""Content-Type"" content=""text/html; charset=iso-8859-1"">", @"<META http-equiv=""Content-Type"" content=""text/html; charset=iso-8859-1"" />");
            htmlContent = htmlContent.Replace("<table class=\"header_table\">\r\n      <tbody></tbody>\r\n    </table>\r\n", "");

            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();

                        using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(style)))
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlContent)))
                            {
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                            }
                        }
                        doc.Close();
                    }
                }
                bytes = ms.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }

        public static void MakeCcdaHeader(ClinicalDocument ccda, CCDADataModel objCCDAModel)
        {
            MakeStaticSection(ccda);

            MakeRecordTargetNode(ccda, objCCDAModel);

            MakeAuthorNode(ccda, objCCDAModel.lstProviderData);

            MakeDataEntererNode(ccda, objCCDAModel);

            MakeCustodianNode(ccda, objCCDAModel);

            MakeInformationRecipientNode(ccda, objCCDAModel);

            MakeLegalAuthenticatorNode(ccda, objCCDAModel);

            MakeInformantNode(ccda, objCCDAModel);

            MakeDocumentationOfNode(ccda, objCCDAModel);

            MakeComponentOfNode(ccda, objCCDAModel);



        }

        private static void MakeComponentOfNode(ClinicalDocument ccda, CCDADataModel objCCDAModel = null)
        {
            Dictionary<string, string> objProvider = objCCDAModel.lstProviderData[0];
            AssignedEntity ae = new AssignedEntity();

            ae.id = new List<II>()
            {  
                new II { root = StaticCcdaData["ProviderOrganizationID"] } 
            };

            //Provider 
            ae.addr = new List<AD>
            {
               HelperMethods.SetAddress("WP", new List<string>() { objProvider["WorkAddress"] }, objProvider["City"], objProvider["State"], objProvider["ZIPCode"], "US")
            };

            ae.telecom = new List<TEL>
            {
                HelperMethods.SetTelephone(objProvider["PhoneNo"], "WP",objProvider["PhoneExt"])
            };

            ae.assignedPerson = new Person();
            ae.assignedPerson.name = new List<PN>();
            ae.assignedPerson.name.Add(HelperMethods.SetName("L", objProvider["LastName"], new List<string> { objProvider["FirstName"], objProvider["MI"] }));

            Component1 componentOf = new Component1();
            EncompassingEncounter encounter = new EncompassingEncounter();
            var High = Convert.ToDateTime(objCCDAModel.VisitDate);

            encounter.responsibleParty = new ResponsibleParty();

            encounter.responsibleParty.assignedEntity = ae;

            Location loc = new Location();
            loc.healthCareFacility = MakeHealthCareFacility(objCCDAModel);
            encounter.location = loc;

            encounter.id = new List<II> { new II { root = StaticCcdaData["UniqueIdentifierID"] } };

            encounter.effectiveTime = new IVL_TS
            {
                ItemsElementName = new ItemsChoiceType2[]
                {
                    ItemsChoiceType2.low,
                    ItemsChoiceType2.high
                },

                Items = new QTY[]
                {
                    new IVXB_TS { value = HelperMethods.SetDateTime(High) },
                    new IVXB_TS { value = HelperMethods.SetDateTime(High) }
                }
            };

            encounter.responsibleParty = new ResponsibleParty();
            encounter.responsibleParty.assignedEntity = new AssignedEntity();
            encounter.responsibleParty.assignedEntity.id = new List<II> { new II { root = StaticCcdaData["UniqueIdentifierID"] } };
            encounter.responsibleParty.assignedEntity.assignedPerson = new Person();

            encounter.responsibleParty.assignedEntity.assignedPerson.name = new List<PN> 
            { 
                HelperMethods.SetName("L", objProvider["LastName"], new List<string> { objProvider["FirstName"], objProvider["MI"] })
            };

            encounter.responsibleParty.assignedEntity.addr = new List<AD>
            {
               HelperMethods.SetAddress("WP", new List<string>() { objProvider["WorkAddress"] }, objProvider["City"], objProvider["State"], objProvider["ZIPCode"], "US")
            };

            encounter.responsibleParty.assignedEntity.telecom = new List<TEL> { HelperMethods.SetTelephone(objProvider["PhoneNo"], "WP", objProvider["PhoneExt"]) };
            encounter.responsibleParty.assignedEntity.representedOrganization = new Organization
            {
                name = new List<ON> { new ON { Text = new List<string> { objCCDAModel.lstPracticeData[0]["ShortName"] } } }
            };

            componentOf.encompassingEncounter = encounter;
            ccda.componentOf = new Component1();
            ccda.componentOf = componentOf;
        }

        private static HealthCareFacility MakeHealthCareFacility(CCDADataModel objCCDAModel = null)
        {
            Dictionary<string, string> objPracticeData = objCCDAModel.lstPracticeData[0];
            HealthCareFacility hcf = new HealthCareFacility();
            hcf.id = new List<II>
            {  
                new II { root = StaticCcdaData["ProviderOrganizationID"] } 
            };

            hcf.location = new Place();

            hcf.location.addr = HelperMethods.SetAddress("WP",
                new List<string> { objPracticeData["Address"] },
                objPracticeData["City"], objPracticeData["State"], objPracticeData["ZIPCode"], "US");

            hcf.serviceProviderOrganization = new Organization();
            hcf.serviceProviderOrganization.id = new List<II>
            {  
                new II { root = StaticCcdaData["ProviderOrganizationID"] } 
            };
            hcf.serviceProviderOrganization.telecom = new List<TEL>
            {
                HelperMethods.SetTelephone(objCCDAModel.lstPracticeData[0]["PhoneNo"], "WP",objCCDAModel.lstPracticeData[0]["PhoneExt"])
            };

            hcf.serviceProviderOrganization.addr = new List<AD>
            {
                HelperMethods.SetAddress("WP",
                new List<string> { objPracticeData["Address"] },
                objPracticeData["City"], objPracticeData["State"], objPracticeData["ZIPCode"], "US")
            };

            return hcf;
        }

        private static void MakeInformantNode(ClinicalDocument ccda, CCDADataModel objCCDAModel = null)
        {
            Dictionary<string, string> objProvider = objCCDAModel.lstProviderData[0];

            AssignedEntity ae = new AssignedEntity();

            ae.id = new List<II>()
            {  
                new II { root = StaticCcdaData["ProviderOrganizationID"] } 
            };

            //Provider
            ae.addr = new List<AD>
            {
               HelperMethods.SetAddress("WP", new List<string>() { objProvider["WorkAddress"] }, objProvider["City"], objProvider["State"], objProvider["ZIPCode"], "US")
            };

            ae.telecom = new List<TEL>
            {
                HelperMethods.SetTelephone(objProvider["PhoneNo"], "WP",objProvider["PhoneExt"])
            };

            ae.assignedPerson = new Person();
            ae.assignedPerson.name = new List<PN>();
            ae.assignedPerson.name.Add(HelperMethods.SetName("L", objProvider["LastName"], new List<string> { objProvider["FirstName"], objProvider["MI"] }));

            Informant12 informant = new Informant12();
            informant.Item = ae;

            ccda.informant = new List<Informant12>();
            ccda.informant.Add(informant);
        }

        public static void MakeDocumentationOfNode(ClinicalDocument ccda, CCDADataModel objCCDAModel = null)
        {
            Dictionary<string, string> objPractice = objCCDAModel.lstPracticeData[0];
            Dictionary<string, string> objPatient = objCCDAModel.lstPatientData[0];
            ServiceEvent se = new ServiceEvent();
            se.classCode = "PCPR";

            se.effectiveTime = new IVL_TS
            {
                ItemsElementName = new ItemsChoiceType2[]
                {
                    ItemsChoiceType2.low,
                    ItemsChoiceType2.high
                },

                Items = new QTY[]
                {
                    new IVXB_TS { value = Convert.ToDateTime( objCCDAModel.VisitDate).ToString("yyyyMMdd") },
                    new IVXB_TS { value = Convert.ToDateTime( objCCDAModel.VisitDate).ToString("yyyyMMdd") }
                }
            };

            Performer1 performer = new Performer1();

            AssignedEntity ae = new AssignedEntity();

            ae.id = new List<II>()
            {  
                new II { root = StaticCcdaData["ProviderOrganizationID"] } 
            };

            //Practice 
            ae.addr = new List<AD>
            {
                HelperMethods.SetAddress("WP", new List<string> { objPractice["Address"]}, objPractice["City"], objPractice["State"], objPractice["ZIPCode"], "US")
            };
            ae.telecom = new List<TEL>
            {
                HelperMethods.SetTelephone(objPractice["PhoneNo"], "WP", objPractice["PhoneExt"])
            };

            string LastName = objPatient["PCPName"], FirstName = string.Empty;

            if (!string.IsNullOrEmpty(LastName) && LastName.IndexOf(" ") > -1)
            {
                string[] names = LastName.Split(' ');
                LastName = names[0];
                FirstName = names[1];
            }

            ae.assignedPerson = new Person();
            ae.assignedPerson.name = new List<PN>();
            ae.assignedPerson.name.Add(HelperMethods.SetName("L", LastName, new List<string>() { FirstName, string.Empty }));

            performer.assignedEntity = ae;
            performer.typeCode = x_ServiceEventPerformer.PPRF;
            performer.functionCode = new CE
            {
                code = "PCP",
                displayName = "Primary Care Physician",
                codeSystemName = "ParticipationFunction",
                codeSystem = "2.16.840.1.113883.5.88"
            };

            se.performer = new List<Performer1>
            {
                performer
            };

            DocumentationOf docOf = new DocumentationOf();
            docOf.serviceEvent = new ServiceEvent();
            docOf.serviceEvent = se;

            ccda.documentationOf = new List<DocumentationOf>();
            ccda.documentationOf.Add(docOf);
        }

        public static void MakeLegalAuthenticatorNode(ClinicalDocument ccda, CCDADataModel objCCDAModel)
        {
            Dictionary<string, string> objProvider = objCCDAModel.lstProviderData[0];
            LegalAuthenticator la = new LegalAuthenticator();
            la.time = new TS
            {
                value = DateTime.Now.ToString("yyyyMMdd")

            };
            la.signatureCode = new CS
            {
                code = "S"
            };

            AssignedEntity ae = new AssignedEntity();

            ae.id = new List<II>
            {  
                new II { root = StaticCcdaData["ProviderOrganizationID"] } 
            };

            //Provider
            ae.addr = new List<AD>
            {
               HelperMethods.SetAddress("WP", new List<string>() { objProvider["WorkAddress"] }, objProvider["City"], objProvider["State"], objProvider["ZIPCode"], "US")
            };

            ae.telecom = new List<TEL>
            {
                HelperMethods.SetTelephone(objProvider["PhoneNo"],  "WP",objProvider["PhoneExt"])
            };


            ae.assignedPerson = new Person();
            ae.assignedPerson.name = new List<PN>();
            ae.assignedPerson.name.Add(HelperMethods.SetName("L", objProvider["LastName"], new List<string> { objProvider["FirstName"], objProvider["MI"] }));

            la.assignedEntity = ae;

            ccda.legalAuthenticator = new LegalAuthenticator();
            ccda.legalAuthenticator = la;
        }

        public static void MakeInformationRecipientNode(ClinicalDocument ccda, CCDADataModel objCCDAModel)
        {

            Dictionary<string, string> refProviderData = objCCDAModel.lstPatientData[0];
            if (refProviderData["ReferringProviderName"] == string.Empty)
                return;

            IntendedRecipient ir = new IntendedRecipient();
            ir.id = new List<II>()
            {  
                new II { root = StaticCcdaData["ProviderOrganizationID"] } 
            };


            ir.informationRecipient = new Person();
            ir.informationRecipient.name = new List<PN>{
                HelperMethods.SetName("L",  refProviderData["ReferringLastName"], new List<string> { refProviderData["ReferringFirstName"], refProviderData["ReferringMI"] })
            };

            var pOrg = new Organization();
            string providerOrganizationName = objCCDAModel.lstPracticeData[0]["ShortName"];
            if (!String.IsNullOrWhiteSpace(providerOrganizationName))
            {
                pOrg.name = new List<ON>
                { 
                    new ON { Text = new List<string>{ providerOrganizationName }}
                };
            }

            //Practice work phone and address
            pOrg.telecom = new List<TEL>
            {
                HelperMethods.SetTelephone(objCCDAModel.lstPracticeData[0]["PhoneNo"],  "WP",objCCDAModel.lstPracticeData[0]["PhoneExt"])
            };
            pOrg.addr = new List<AD>
            {
                HelperMethods.SetAddress( "WP", new List<string> {objCCDAModel.lstPracticeData[0]["Address"]},
                    objCCDAModel.lstPracticeData[0]["City"], objCCDAModel.lstPracticeData[0]["State"],  objCCDAModel.lstPracticeData[0]["ZIPCode"], "US")
            };

            ir.receivedOrganization = pOrg;

            InformationRecipient infor = new InformationRecipient();
            infor.intendedRecipient = ir;

            ccda.informationRecipient = new List<InformationRecipient>();
            ccda.informationRecipient.Add(infor);

        }

        public static void MakeCustodianNode(ClinicalDocument ccda, CCDADataModel objCCDAModel)
        {
            Dictionary<string, string> ProviderData = objCCDAModel.lstProviderData[0];
            var cousdOrg = new CustodianOrganization();

            cousdOrg.id = new List<II>()
            {  
                new II { root = StaticCcdaData["ProviderOrganizationID"] } 
            };

            string providerOrganizationName = objCCDAModel.lstPracticeData[0]["ShortName"];
            if (!String.IsNullOrWhiteSpace(providerOrganizationName))
            {
                cousdOrg.name = new ON
                {
                    Text = new List<string> { providerOrganizationName }
                };
            }

            cousdOrg.telecom = HelperMethods.SetTelephone(ProviderData["PhoneNo"], "WP", ProviderData["PhoneExt"]);
            cousdOrg.addr = HelperMethods.SetAddress("WP", new List<string> { ProviderData["WorkAddress"] }, ProviderData["City"], ProviderData["State"], ProviderData["ZIPCode"], "US");

            AssignedCustodian ac = new AssignedCustodian();
            ac.representedCustodianOrganization = cousdOrg;

            Custodian custodian = new Custodian();
            custodian.assignedCustodian = ac;
            ccda.custodian = custodian;
        }

        public static void MakeDataEntererNode(ClinicalDocument ccda, CCDADataModel objCCDAModel)
        {
            Dictionary<string, string> objProvider = objCCDAModel.lstProviderData[0];


            AssignedEntity ae = new AssignedEntity();
            ae.id = new List<II>()
            {  
                new II { root = StaticCcdaData["ProviderOrganizationID"] } 
            };
            ae.addr = new List<AD>
            {
                HelperMethods.SetAddress("WP", new List<string>() { objProvider["WorkAddress"] }, objProvider["City"], objProvider["State"], objProvider["ZIPCode"], "US")
            };
            ae.telecom = new List<TEL>
            {
                HelperMethods.SetTelephone(objProvider["PhoneNo"],"WP",objProvider["PhoneExt"])
            };
            ae.assignedPerson = new Person();
            ae.assignedPerson.name = new List<PN> { 
                HelperMethods.SetName("L", objProvider["LastName"], new List<string>() { objProvider["FirstName"], objProvider["MI"] }) 
            };

            ccda.dataEnterer = new DataEnterer();
            ccda.dataEnterer.assignedEntity = ae;
        }

        public static void MakeAuthorNode(ClinicalDocument ccda, List<Dictionary<string, string>> lstProviderData)
        {
            if (lstProviderData != null && lstProviderData.Count > 0)
            {
                Dictionary<string, string> ProviderData = lstProviderData[0];
                Author author = new Author();
                author.time = new TS
                {
                    value = DateTime.Now.ToString("yyyyMMdd")
                };
                author.assignedAuthor = new AssignedAuthor();

                author.assignedAuthor.id = new List<II>()
                {  
                    new II { root = StaticCcdaData["ProviderOrganizationID"] } 
                };

                author.assignedAuthor.addr = new List<AD>
                {
                    HelperMethods.SetAddress("WP", new List<string>() { ProviderData["WorkAddress"] }, ProviderData["City"], ProviderData["State"], ProviderData["ZIPCode"], "US")
                };
                author.assignedAuthor.telecom = new List<TEL>
                {
                    HelperMethods.SetTelephone(ProviderData["PhoneNo"], "WP",ProviderData["PhoneExt"])
                };

                var assignedPerson = new Person();
                assignedPerson.name = new List<PN>
                {
                    HelperMethods.SetName("L", ProviderData["LastName"], new List<string> { ProviderData["FirstName"], ProviderData["MI"] })
                };
                author.assignedAuthor.Item = assignedPerson;

                ccda.author = new List<Author>
                {
                   author
                };
            }
        }

        public static void MakeStaticSection(ClinicalDocument ccda)
        {
            ccda.realmCode = new List<CS> 
            { 
                new CS { code = StaticCcdaData["RealmCode"] } 
            };

            ccda.typeId = new InfrastructureRoottypeId
            {
                root = StaticCcdaData["ClinicalDocumentTypeIdRoot"],
                extension = StaticCcdaData["ClinicalDocumentTypeIdExtension"]
            };


            ccda.templateId = new List<II> 
            { 
                new II { root = StaticCcdaData["TemplateID"] } 
            };

            ccda.id = new II
            {
                root = StaticCcdaData["UniqueIdentifierID"]
            };

            ccda.code = new CE()
            {
                codeSystem = StaticCcdaData["ClinicalDocumentCodeCodeSystem"],
                codeSystemName = StaticCcdaData["ClinicalDocumentCodeCodeSystemName"],
                code = StaticCcdaData["ClinicalDocumentCodeCode"]
            };

            // Template Id based on Document Type 
            if (DocumentTemplate == DocumentTemplateType.TransferSummary)
            {
                ccda.templateId.Add(new II { root = StaticCcdaData["TS"] });
                ccda.code.code = "34133-9";
                ccda.code.displayName = "TRANSITION OF CARE REFERRAL SUMMARY";
                ccda.title = new ST { Text = new List<string> { "Transition of Care/Referral Summary" } };
            }
            else if (DocumentTemplate == DocumentTemplateType.ClinicalSummary)
            {
                ccda.templateId.Add(new II { root = StaticCcdaData["CS"] });
                ccda.code.code = "34133-9";
                ccda.code.displayName = "SUMMARIZATION OF EPISODE NOTE";
                ccda.title = new ST { Text = new List<string> { "Clinical Summary" } };
            }

            ccda.effectiveTime = new TS
            {
                value = DateTime.Now.ToString("yyyyMMdd")
            };

            ccda.confidentialityCode = new CE
            {
                code = "R",
                codeSystem = StaticCcdaData["ClinicalDocumentConfidentialityCodeCodeSystem"]
            };

            ccda.languageCode = new CS
            {
                code = StaticCcdaData["ClinicalDocumentLanguageCode"]
            };

            if (DocumentTemplate == DocumentTemplateType.ClinicalSummary)
                ccda.componentOf = new Component1
                {
                    encompassingEncounter = new EncompassingEncounter
                    {
                        id = new List<II> { new II { root = StaticCcdaData["UniqueIdentifierID"] } },
                        effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new ItemsChoiceType2[] 
                            { 
                                ItemsChoiceType2.low, 
                                ItemsChoiceType2.high 
                            },

                            Items = new QTY[] 
                            { 
                                new IVXB_TS { value = DateTime.Now.ToString("yyyyMMdd") },
                                new IVXB_TS { value = DateTime.Now.ToString("yyyyMMdd") }
                            }
                        },
                    }
                };
        }

        public static void MakeRecordTargetNode(ClinicalDocument ccda, CCDADataModel objCCDAModel)
        {
            RecordTarget rt = new RecordTarget();

            //Patient Demographics
            //1) Patient name
            //2) Sex
            //3) Date of birth
            //4) Race 
            //5) Ethnicity 
            //6) Preferred language 

            MakePatientRoleNode(rt, objCCDAModel);
            ccda.recordTarget = new List<RecordTarget>
            {
                rt
            };
        }

        public static void MakePatientRoleNode(RecordTarget rt, CCDADataModel objCCDAModel)
        {
            PatientRole pr = new PatientRole();
            List<Dictionary<string, string>> lstPatientData = objCCDAModel.lstPatientData;
            if (lstPatientData != null && lstPatientData.Count > 0)
            {
                Dictionary<string, string> PatientData = lstPatientData[0];
                pr.id = new List<II> 
            { 
                new II { root =  StaticCcdaData["UniqueIdentifierID"] } 
            };

                pr.addr = new List<AD>
                {
                    HelperMethods.SetAddress("H", new List<string>() { PatientData["PatientStreetAddress"], }, PatientData["PatientCity"], PatientData["PatientState"], PatientData["PatientZIPCode"], "US")
                };

                pr.telecom = new List<TEL>();
                pr.telecom.Add(HelperMethods.SetTelephone(PatientData["PatientHomePhoneNo"], "HP"));
                pr.telecom.Add(HelperMethods.SetTelephone(PatientData["PatientWorkPhoneNo"], "WP"));

                MakePatientNode(pr, PatientData);

                //<!-- Practice  -->
                var pOrg = new Organization();
                pOrg.id = new List<II>()
                {  
                    new II { root = StaticCcdaData["ProviderOrganizationID"] } 
                };

                Dictionary<string, string> PracticeData = objCCDAModel.lstPracticeData[0];

                string providerOrganization = PracticeData["ShortName"];
                if (!String.IsNullOrWhiteSpace(providerOrganization))
                {
                    pOrg.name = new List<ON>
                { 
                    new ON { Text = new List<string>{ providerOrganization }}
                };
                }


                pOrg.telecom = new List<TEL>
                {
                    HelperMethods.SetTelephone(PracticeData["PhoneNo"], "WP")
                };
                pOrg.addr = new List<AD>
                {
                    HelperMethods.SetAddress("WP", new List<string>() { PracticeData["Address"] }, PracticeData["City"], PracticeData["State"], PracticeData["ZIPCode"], "US")
                };
                pr.providerOrganization = pOrg;
            }

            rt.patientRole = pr;
        }

        public static void MakePatientNode(PatientRole pr, Dictionary<string, string> PatientData)
        {
            Patient p = new Patient();

            p.name = new List<PN>
            {
                HelperMethods.SetName("L", PatientData["LastName"], new List<string>() { PatientData["FirstName"], PatientData["MI"] })
            };

            p.administrativeGenderCode = new CE
            {
                code = HelperMethods.GetGenderCode(PatientData["PatientGender"]),
                codeSystem = StaticCcdaData["GenderCodeCodeSystem"],
                codeSystemName = StaticCcdaData["GenderCodeCodeSystemName"],
                displayName = PatientData["PatientGender"]
            };

            DateTime dob = Utility.ToDateTime(PatientData["PatientDOB"]);
            if (dob != null)
            {
                p.birthTime = new TS
                {
                    value = dob.ToString("yyyyMMdd")
                };
            }


            p.maritalStatusCode = new CE
            {
                code = HelperMethods.GetMaritalStatusCode(PatientData["MaritialStatus"]),
                codeSystem = StaticCcdaData["MaritalStatusCodeSystem"],
                codeSystemName = StaticCcdaData["MaritalStatusCodeSystemName"],
                displayName = PatientData["MaritialStatus"]
            };

            string raceCode = PatientData["PatientRaceCode"];
            string race = PatientData["PatientRace"];
            if (!String.IsNullOrWhiteSpace(raceCode) && !String.IsNullOrWhiteSpace(race))
            {
                p.raceCode = new CE
                {
                    code = raceCode,
                    codeSystem = StaticCcdaData["RaceCode"],
                    codeSystemName = "Race and Ethnicity - CDC",
                    displayName = race
                };
            }

            string ethinicityCode = PatientData["PatientEthnicityCode"];
            p.ethnicGroupCode = new CE
            {
                codeSystem = StaticCcdaData["RaceCode"],
                codeSystemName = "Race and Ethnicity - CDC"
            };

            if (ethinicityCode == "2135-2")
            {
                p.ethnicGroupCode.code = ethinicityCode;
                p.ethnicGroupCode.displayName = "Hispanic or Latino";
            }
            else if (ethinicityCode == "2186-5")
            {
                p.ethnicGroupCode.code = "2186-5";
                p.ethnicGroupCode.displayName = "Not Hispanic or Latino";
            }

            string languageCode = PatientData["PatientLanguageCode"];
            if (!String.IsNullOrWhiteSpace(languageCode))
            {
                p.languageCommunication = new List<LanguageCommunication>() 
                { 
                    new LanguageCommunication
                    {
                        languageCode = new CS{ code = languageCode },
                        preferenceInd = new BL { value = true , valueSpecified = true}
                    }
                };
            }

            pr.patient = p;
        }

        private static void MakeCcdaBody(ClinicalDocument ccda, CCDADataModel objCCDAModel, CCDAGenrator.DocumentTemplateType documentType)
        {
            StructuredBody sb = new StructuredBody();
            sb.component = new List<Component3>();
            if (objCCDAModel != null && objCCDAModel.Components != null)
            {
                if (documentType == DocumentTemplateType.ClinicalSummary || documentType == DocumentTemplateType.DataPortability)
                    AddAllergiesComponent(sb, objCCDAModel);

                if (documentType == DocumentTemplateType.ClinicalSummary || documentType == DocumentTemplateType.DataPortability)
                    AddImmunizationComponent(sb, objCCDAModel);

                if (documentType == DocumentTemplateType.ClinicalSummary || documentType == DocumentTemplateType.DataPortability)
                    AddMedicationComponent(sb, objCCDAModel);

                if (documentType == DocumentTemplateType.ClinicalSummary)
                    AddMedicationAdministeredComponent(sb, objCCDAModel);

                if (documentType == DocumentTemplateType.ClinicalSummary)
                    AddPlanOfCareComponent(sb, objCCDAModel);

                if (documentType == DocumentTemplateType.ClinicalSummary)
                    AddChiefComplaintComponent(sb, objCCDAModel);

                if (documentType == DocumentTemplateType.ClinicalSummary || documentType == DocumentTemplateType.DataPortability)
                    AddProcedureComponent(sb, objCCDAModel);

                if (documentType == DocumentTemplateType.ClinicalSummary || documentType == DocumentTemplateType.DataPortability)
                    AddProblemListComponent(sb, objCCDAModel);

                if (documentType == DocumentTemplateType.ClinicalSummary || documentType == DocumentTemplateType.DataPortability)
                    AddResultsSection(sb, objCCDAModel);

                if (documentType == DocumentTemplateType.ClinicalSummary || documentType == DocumentTemplateType.DataPortability)
                    AddSocialHistoryComponent(sb, objCCDAModel);

                if (documentType == DocumentTemplateType.ClinicalSummary || documentType == DocumentTemplateType.DataPortability)
                    AddVitalSignsComponent(sb, objCCDAModel);

            }


            ccda.component = new Component2();
            ccda.component.Item = sb;
        }

        private static void AddResultsSection(StructuredBody sb, CCDADataModel objCCDAModel)
        {
            Component3 resultsComponent = HelperMethods.IntializeComponent3(
               new List<string> { "2.16.840.1.113883.10.20.22.2.3", "2.16.840.1.113883.10.20.22.2.3.1" },
               "30954-2", "2.16.840.1.113883.6.1", "LOINC", "RESULTS", "RESULTS");

            StrucDocText text = resultsComponent.section.text;

            List<Dictionary<string, string>> lstResults = objCCDAModel.lstResults;
            if (lstResults != null && lstResults.Count > 0)
            {
                var table = new StrucDocTable();
                table.width = "100%";
                table.border = "1";

                text.Items.Add(table);

                table.thead = new StrucDocThead();
                table.thead.tr = new List<StrucDocTr>
               {
                   new StrucDocTr
                   {
                       Items = new List<object>
                       {
                           new StrucDocTh
                           { 
                               Text = new List<string>{"LABORATORY INFORMATION"}
                           },
                           new StrucDocTh
                           { 
                               Text = new List<string>{"ACTUAL RESULT"}
                           },
                           new StrucDocTh
                           { 
                               Text = new List<string>{"DATE"}
                           }
                       }
                      
                   }
               };

                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);
                tbody.tr = new List<StrucDocTr>();

                resultsComponent.section.entry = new List<Entry>();


                foreach (var drOrderResult in lstResults)
                {
                    StrucDocTr labBodyTr = new StrucDocTr();
                    labBodyTr.Items = new List<object>();

                    labBodyTr.Items.Add(new StrucDocTd
                    {
                        Items = new List<object>
                       {
                           new StrucDocContent
                           {
                               Text = new List<string> { Convert.ToString(drOrderResult["LabTest"])  }
                           }                            
                       }
                    });

                    labBodyTr.Items.Add(new StrucDocTd
                    {
                        Items = new List<object>
                       {
                           new StrucDocContent
                           {
                               Text = new List<string> { Convert.ToString(drOrderResult["ActualResult"])  }
                           }                            
                       }
                    });

                    labBodyTr.Items.Add(new StrucDocTd
                    {
                        Items = new List<object>
                       {
                           new StrucDocContent
                           {
                               Text = new List<string> { Convert.ToString(drOrderResult["ResultDate"])  }
                           }                            
                       }
                    });
                    tbody.tr.Add(labBodyTr);

                    var resultOrganizerEntry = new Entry();
                    resultsComponent.section.entry.Add(resultOrganizerEntry);

                    var resultOrganizer = new Organizer();
                    resultOrganizerEntry.Item = resultOrganizer;

                    resultOrganizer.classCode = x_ActClassDocumentEntryOrganizer.BATTERY;
                    resultOrganizer.moodCode = "EVN";
                    resultOrganizer.templateId = new List<II>
                   {
                       new II { root = "2.16.840.1.113883.10.20.22.4.1" }
                   };

                    resultOrganizer.id = new List<II>
                   {
                       new II { root = StaticCcdaData["UniqueIdentifierID"] }
                   };

                    if (!String.IsNullOrWhiteSpace(Convert.ToString(drOrderResult["LoincCode"])))
                    {
                        resultOrganizer.code = new CD
                        {
                            code = Convert.ToString(drOrderResult["LoincCode"]),
                            displayName = Convert.ToString(drOrderResult["LabTest"]),
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC"
                        };
                    }

                    resultOrganizer.statusCode = new CS
                    {
                        code = "completed"
                    };
                    resultOrganizer.component = new List<Component4>();

                    var resultObservationComponent = new Component4();
                    resultOrganizer.component.Add(resultObservationComponent);

                    var resultObservation = new Observation();
                    resultObservationComponent.Item = resultObservation;

                    resultObservation.classCode = "OBS";
                    resultObservation.moodCode = x_ActMoodDocumentObservation.EVN;

                    resultObservation.templateId = new List<II>
                   {
                        new II { root = "2.16.840.1.113883.10.20.22.4.2" }
                   };

                    resultObservation.id = new List<II> 
                   {
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                   };

                    resultObservation.code = new CE
                    {
                        code = Convert.ToString(drOrderResult["LoincCode"]),
                        displayName = Convert.ToString(drOrderResult["LabTest"]),
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC"
                    };

                    resultObservation.text = new ED
                    {
                        reference = new TEL { value = "#Result" }
                    };

                    resultObservation.statusCode = new CS
                    {
                        code = "completed"
                    };

                    resultObservation.effectiveTime = new IVL_TS
                    {
                        value = Convert.ToDateTime(drOrderResult["ResultDate"]).ToString("yyyyMMddhhmm")
                    };

                    resultObservation.value = new List<ANY>
                   {
                        new PQ
                        { 
                            value = Convert.ToString(drOrderResult["ResultValue"]),
                            unit =   string.IsNullOrEmpty(Convert.ToString(drOrderResult["Unit"])) ? "UNK" : Convert.ToString(drOrderResult["Unit"])
                        }
                   };
                }
            }
            else
            {
                if (lstResults == null || lstResults.Count == 0)
                {
                    text.Items.Add(new StrucDocContent { ID = "noResults", Text = new List<string> { "Not included in document" } });
                }

                resultsComponent.section.entry = new List<Entry>();

                var resultOrganizerEntry = new Entry();
                resultsComponent.section.entry.Add(resultOrganizerEntry);

                var resultOrganizer = new Organizer();
                resultOrganizerEntry.Item = resultOrganizer;

                resultOrganizer.classCode = x_ActClassDocumentEntryOrganizer.BATTERY;
                resultOrganizer.moodCode = "EVN";
                resultOrganizer.templateId = new List<II>
               {
                    new II { root = "2.16.840.1.113883.10.20.22.4.1" }
               };

                resultOrganizer.id = new List<II>
               {
                    new II { nullFlavor = "UNK" }
               };

                resultOrganizer.code = new CD { nullFlavor = "UNK" };
                resultOrganizer.statusCode = new CS { nullFlavor = "UNK" };
                resultOrganizer.component = new List<Component4>();

                var resultObservationComponent = new Component4();
                resultOrganizer.component.Add(resultObservationComponent);

                var resultObservation = new Observation();
                resultObservationComponent.Item = resultObservation;

                resultObservation.classCode = "OBS";
                resultObservation.moodCode = x_ActMoodDocumentObservation.EVN;

                resultObservation.templateId = new List<II>
               {
                   new II { root = "2.16.840.1.113883.10.20.22.4.2" }
               };

                resultObservation.id = new List<II> 
               {
                   new II { root = "NI" }
               };

                resultObservation.code = new CD
                {
                    code = "completed"
                };

                resultObservation.statusCode = new CS
                {
                    code = "completed"
                };

                resultObservation.effectiveTime = new IVL_TS
                {
                    nullFlavor = "NI"
                };

                resultObservation.value = new List<ANY>
               {
                   new PQ { nullFlavor = "UNK" }
               };
            }

            sb.component.Add(resultsComponent);
        }

        private static void AddPlanOfCareComponent(StructuredBody sb, CCDADataModel objCCDAModel)
        {
            Component3 Component = HelperMethods.IntializeComponent3(
            new List<string> { "2.16.840.1.113883.10.20.22.2.10" },
            "18776-5", "2.16.840.1.113883.6.1", "LOINC", "Plan of Care", "PLAN OF CARE");

            StrucDocText text = Component.section.text;
            List<Dictionary<string, string>> lstscheduledProcedure = objCCDAModel.lstscheduledProcedure;
            if (lstscheduledProcedure == null)
            {
                lstscheduledProcedure = new List<Dictionary<string, string>>();
            }
            if (objCCDAModel.lstFutureAppointment == null)
            {
                objCCDAModel.lstFutureAppointment = new List<Dictionary<string, string>>();
            }
            if (lstscheduledProcedure.Count > 0 || objCCDAModel.lstGoal.Count > 0 || objCCDAModel.lstFutureAppointment.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);

                table.border = "1";
                table.width = "100%";

                table.thead = new StrucDocThead();
                table.thead.tr = new List<StrucDocTr>();
                var headTr = new StrucDocTr();
                table.thead.tr.Add(headTr);

                headTr.Items = new List<object> 
                { 
                    new StrucDocTh { Text = new List<string> { "Name" } },
                    new StrucDocTh { Text = new List<string> { "Instruction" } },
                    new StrucDocTh { Text = new List<string> { "Type" } },
                    new StrucDocTh { Text = new List<string> { "Date" } }
                };

                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);

                tbody.tr = new List<StrucDocTr>();

                //Procedure
                DataTable dtProcedure = new DataTable();
                foreach (Dictionary<string, string> dr in objCCDAModel.lstscheduledProcedure)
                {
                    var bodyTr = new StrucDocTr();
                    bodyTr.Items = new List<object>();
                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                            Convert.ToString(dr["Name"])
                        }
                    });

                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                            Convert.ToString(dr["Instruction"])
                        }
                    });

                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                            Convert.ToString(dr["Type"])
                        }
                    });

                    var visitDate = Convert.ToDateTime(dr["Date"]).ToString("MMM dd, yyyy");
                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                            visitDate
                        }
                    });

                    tbody.tr.Add(bodyTr);
                }

                //Future Appointments
                DataTable dtAppointments = new DataTable();
                foreach (Dictionary<string, string> dr in objCCDAModel.lstFutureAppointment)
                {
                    var bodyTr = new StrucDocTr();
                    bodyTr.Items = new List<object>();
                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                            Convert.ToString(dr["Name"])
                        }
                    });

                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                            Convert.ToString(dr["Instruction"])
                        }
                    });

                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                            Convert.ToString(dr["Type"])
                        }
                    });

                    var visitDate = Convert.ToDateTime(dr["Date"]).ToString("MMM dd, yyyy");
                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                            visitDate
                        }
                    });

                    tbody.tr.Add(bodyTr);
                }

                //Instructions
                DataTable dtInstructions = new DataTable();
                foreach (Dictionary<string, string> dr in objCCDAModel.lstGoal)
                {
                    var bodyTr = new StrucDocTr();
                    bodyTr.Items = new List<object>();
                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                            Convert.ToString(dr["Name"])
                        }
                    });

                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                            Convert.ToString(dr["Instruction"])
                        }
                    });

                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                            Convert.ToString(dr["Type"])
                        }
                    });

                    var visitDate = Convert.ToDateTime(dr["Date"]).ToString("MMM dd, yyyy");
                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                            visitDate
                        }
                    });

                    tbody.tr.Add(bodyTr);
                }

                Component.section.entry = new List<Entry>();
            }
            else
            {
                if (lstscheduledProcedure.Count == 0 || objCCDAModel.lstGoal.Count == 0 || objCCDAModel.lstFutureAppointment.Count == 0)
                {
                    text.Items.Add(new StrucDocContent { ID = "noPlanOfCare", Text = new List<string> { "Not included in document" } });
                }
            }

            sb.component.Add(Component);
        }

        private static void AddChiefComplaintComponent(StructuredBody sb, CCDADataModel objCCDAModel)
        {
            Component3 chiefComplaintsComponent = HelperMethods.IntializeComponent3(
                new List<string> { "2.16.840.1.113883.10.20.22.2.13" },
                "46239-0", "2.16.840.1.113883.6.1", "LOINC", "Chief Complaint and Reason for Visit", "REASON FOR VISIT/CHIEF COMPLAINT");

            StrucDocText text = chiefComplaintsComponent.section.text;

            if (objCCDAModel != null && !string.IsNullOrEmpty(objCCDAModel.ReasonForVisit))
            {
                StrucDocTable table = new StrucDocTable();
                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead
                {
                    tr = new List<StrucDocTr>
                    {
                        new StrucDocTr 
                        {
                            Items = new List<object>
                            {
                                new StrucDocTh{ Text = new List<string>{"REASON FOR VISIT/CHIEF COMPLAINT"}}
                            }
                        }
                    }
                };

                text.Items.Add(table);

                table.tbody = new List<StrucDocTbody>();
                StrucDocTbody tBody = new StrucDocTbody();
                table.tbody.Add(tBody);

                StrucDocTd td = new StrucDocTd();

                tBody.tr = new List<StrucDocTr>
                {
                    new StrucDocTr 
                    {
                        Items = new List<object>
                        {
                            new StrucDocTd { Text = new List<string> { objCCDAModel.ReasonForVisit } }
                        }
                    }
                };
            }
            else
            {
                text.Items.Add(new StrucDocContent { ID = "noChiefComplaints", Text = new List<string> { "Unknown ChiefComplaint" } });
            }

            sb.component.Add(chiefComplaintsComponent);
        }

        private static void AddImmunizationComponent(StructuredBody sb, CCDADataModel objCCDAModel)
        {
            Component3 immunizationsComponent = HelperMethods.IntializeComponent3(
                new List<string> { "2.16.840.1.113883.10.20.22.2.2", "2.16.840.1.113883.10.20.22.2.2.1" },
                "11369-6", "2.16.840.1.113883.6.1", "LOINC", "Immunizations", "IMMUNIZATION");

            StrucDocText text = immunizationsComponent.section.text;

            List<Dictionary<string, string>> lstImmunization = objCCDAModel.lstImmunization;
            if (lstImmunization != null && lstImmunization.Count > 0)
            {

                var content = new StrucDocContent();
                content.ID = "Immunization";
                text.Items.Add(content);

                var table = new StrucDocTable();
                table.border = "1";
                table.width = "100%";
                text.Items.Add(table);

                table.thead = new StrucDocThead();
                table.thead.tr = new List<StrucDocTr>();

                var headTr = new StrucDocTr();
                headTr.Items = new List<object>();
                headTr.Items.Add(new StrucDocTh { Text = new List<string> { "Vaccine Name" } });
                headTr.Items.Add(new StrucDocTh { Text = new List<string> { "Date" } });
                headTr.Items.Add(new StrucDocTh { Text = new List<string> { "Status" } });
                table.thead.tr.Add(headTr);

                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);
                tbody.tr = new List<StrucDocTr>();

                DataTable Immunization = new DataTable();

                int counter = lstImmunization.Count;

                for (int i = 0; i < counter; i++)
                {
                    Dictionary<string, string> dr = lstImmunization[i];

                    var tr = new StrucDocTr();
                    tbody.tr.Add(tr);

                    string status = dr["Status"];//"Administered";

                    tr.Items = new List<object> 
                    { 
                        new StrucDocTd 
                        { 
                            Items = new List<object>
                            {
                                new StrucDocContent 
                                { 
                                    ID = String.Concat("Immune", i + 1)
                                }
                            },

                            Text = new List<string>{ dr["Immunization"] }
                        },

                        new StrucDocTd 
                        { 
                                Text = new List<string>{ Convert.ToDateTime(dr["DateAdministered"]).ToString("dd-MMM-yyyy")}
                        },

                        new StrucDocTd 
                        { 
                                Text = new List<string>{ status }
                        }
                    };
                }

                immunizationsComponent.section.entry = new List<Entry>();

                for (int i = 0; i < counter; i++)
                {
                    Dictionary<string, string> dr = lstImmunization[i];

                    var immunizationActivityEntry = new Entry();
                    immunizationsComponent.section.entry.Add(immunizationActivityEntry);

                    var immunizationActivity = new SubstanceAdministration();
                    immunizationActivityEntry.Item = immunizationActivity;

                    immunizationActivity.classCode = "SBADM";
                    immunizationActivity.moodCode = x_DocumentSubstanceMood.EVN;
                    immunizationActivity.negationInd = false;
                    immunizationActivity.negationIndSpecified = true;
                    immunizationActivity.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.52" }
                    };

                    immunizationActivity.id = new List<II>
                    {
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };

                    immunizationActivity.text = new ED
                    {
                        reference = new TEL { value = String.Concat("#Immune", i + 1) }
                    };

                    immunizationActivity.statusCode = new CS
                    {
                        code = "completed"
                    };

                    string dateAdmin = Convert.ToDateTime(dr["DateAdministered"]).ToString("yyyyMMdd");
                    if (!String.IsNullOrWhiteSpace(dateAdmin))
                    {
                        immunizationActivity.effectiveTime = new List<SXCM_TS>
                        {
                            new SXCM_TS 
                            { 
                                value = dateAdmin
                            }
                        };
                    }
                    else
                    {
                        immunizationActivity.effectiveTime = new List<SXCM_TS>
                        {
                            new SXCM_TS 
                            { 
                                nullFlavor = "UNK"
                            }
                        };
                    }

                    Consumable consumable = new Consumable();

                    ManufacturedProduct manufacturedProduct = new ManufacturedProduct();
                    consumable.manufacturedProduct = manufacturedProduct;

                    manufacturedProduct.classCode = RoleClassManufacturedProduct.MANU;
                    manufacturedProduct.classCodeSpecified = true;
                    manufacturedProduct.templateId = new List<II>
                    { 
                        new II { root = "2.16.840.1.113883.10.20.22.4.54" } 
                    };

                    manufacturedProduct.Item = new Material
                    {
                        code = new CE
                        {
                            code = Convert.ToString(dr["CVX"]),
                            codeSystem = "2.16.840.1.113883.12.292",
                            codeSystemName = "CVX",
                            displayName = Convert.ToString(dr["Immunization"]),
                            originalText = new ED
                            {
                                reference = new TEL { value = String.Concat("#Immune", i + 1) }
                            }
                        }
                    };

                    immunizationActivity.consumable = consumable;
                }
            }
            else
            {
                if (lstImmunization == null || lstImmunization.Count == 0)
                {
                    text.Items.Add(new StrucDocContent { ID = "noImmunization", Text = new List<string> { "Not included in document" } });
                }

                immunizationsComponent.section.entry = new List<Entry>();

                var immunizationActivityEntry = new Entry();
                immunizationsComponent.section.entry.Add(immunizationActivityEntry);

                var immunizationActivity = new SubstanceAdministration();
                immunizationActivityEntry.Item = immunizationActivity;

                immunizationActivity.classCode = "SBADM";
                immunizationActivity.moodCode = x_DocumentSubstanceMood.EVN;
                immunizationActivity.negationInd = true;
                immunizationActivity.negationIndSpecified = true;
                immunizationActivity.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.52" }
                    };

                immunizationActivity.id = new List<II>
                    {
                        new II { nullFlavor = "NA" }
                    };

                immunizationActivity.statusCode = new CS
                {
                    code = "completed"
                };

                immunizationActivity.effectiveTime = new List<SXCM_TS> { new SXCM_TS { nullFlavor = "UNK" } };

                immunizationActivity.consumable = new Consumable();
                var immunizationMedicationInfo = new ManufacturedProduct();
                immunizationActivity.consumable.manufacturedProduct = immunizationMedicationInfo;

                immunizationMedicationInfo.classCode = RoleClassManufacturedProduct.MANU;
                immunizationMedicationInfo.classCodeSpecified = true;
                immunizationMedicationInfo.templateId = new List<II> 
                { 
                    new II { root = "2.16.840.1.113883.10.20.22.4.54" } 
                };

                var manufacturedMaterial = new Material();
                immunizationMedicationInfo.Item = manufacturedMaterial;

                var vaccineAdministeredCode = new CE
                {
                    nullFlavor = "NA"
                };

                manufacturedMaterial.code = vaccineAdministeredCode;
            }

            sb.component.Add(immunizationsComponent);
        }

        private static void AddProcedureComponent(StructuredBody sb, CCDADataModel objCCDAModel)
        {
            Component3 proComponent = HelperMethods.IntializeComponent3(
               new List<string> { "2.16.840.1.113883.10.20.22.2.7", "2.16.840.1.113883.10.20.22.2.7.1" },
               "47519-4", "2.16.840.1.113883.6.1", "LOINC", "History of Procedures", "PROCEDURES");

            StrucDocText text = proComponent.section.text;
            List<Dictionary<string, string>> lstProcedure = objCCDAModel.lstProcedure;
            if (objCCDAModel.lstProcedure != null && objCCDAModel.lstProcedure.Count > 0)
            {
                DataTable dtProcedures = new DataTable(); //Procedure DataTable

                var table = new StrucDocTable();
                text.Items.Add(table);

                table.border = "1";
                table.width = "100%";

                table.thead = new StrucDocThead();
                table.thead.tr = new List<StrucDocTr>();
                var headTr = new StrucDocTr();
                table.thead.tr.Add(headTr);

                headTr.Items = new List<object> 
                { 
                    new StrucDocTh { Text = new List<string> { "Procedure" } },
                    new StrucDocTh { Text = new List<string> { "Date" } }
                };

                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);

                tbody.tr = new List<StrucDocTr>();

                for (int i = 0; i < lstProcedure.Count; i++)
                {
                    var bodyTr = new StrucDocTr();
                    bodyTr.Items = new List<object>();

                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Items = new List<object> { new StrucDocContent { ID = "Procedure" + (i + 1) } },
                        Text = new List<string> { Convert.ToString(lstProcedure[i]["ProcedureName"]) }
                    });

                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> 
                        { 
                           Convert.ToDateTime(lstProcedure[i]["ProcedureDate"]).ToString("yyyy")  //Check for Null
                        }
                    });

                    tbody.tr.Add(bodyTr);
                }

                proComponent.section.entry = new List<Entry>();

                for (int i = 0; i < lstProcedure.Count; i++)
                {
                    Dictionary<string, string> drProcedure = lstProcedure[i];

                    Entry obsevationEntry = new Entry();
                    proComponent.section.entry.Add(obsevationEntry);

                    Observation observation = new Observation();

                    observation.classCode = "OBS";
                    observation.moodCode = x_ActMoodDocumentObservation.EVN;

                    observation.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.13"}
                    };

                    observation.id = new List<II>
                    {
                        new II 
                        { 
                            root = StaticCcdaData["UniqueIdentifierID"]
                        }
                    };

                    if (!String.IsNullOrWhiteSpace(drProcedure["SnomedId"]))
                    {
                        observation.code = new CD
                        {
                            code = drProcedure["SnomedId"],
                            codeSystem = "2.16.840.1.113883.6.96",
                            codeSystemName = "SNOMED CT",
                            displayName = drProcedure["ProcedureName"]
                        };
                    }
                    else if (!String.IsNullOrWhiteSpace(drProcedure["CPT"]))
                    {
                        observation.code = new CD
                        {
                            code = drProcedure["CPT"],
                            codeSystem = "2.16.840.1.113883.6.12",
                            codeSystemName = "CPT-4",
                            displayName = drProcedure["ProcedureName"]
                        };
                    }

                    observation.statusCode = new CS
                    {
                        code = "active"
                    };
                    observation.effectiveTime = new IVL_TS
                    {
                        value = Convert.ToDateTime(drProcedure["ProcedureDate"]).ToString("yyyyMMdd")
                    };
                    observation.value = new List<ANY> 
                    { 
                        new CD
                        {
                            nullFlavor = "UNK"
                        }
                    };

                    obsevationEntry.Item = observation;
                }
            }
            else
            {
                if (objCCDAModel.lstProcedure == null || objCCDAModel.lstProcedure.Count == 0)
                {
                    text.Items.Add(new StrucDocContent { ID = "noProcedure", Text = new List<string> { "Not included in document" } });
                }

                proComponent.section.entry = new List<Entry>();
                Entry obsevationEntry = new Entry();
                proComponent.section.entry.Add(obsevationEntry);

                Observation observation = new Observation();
                obsevationEntry.Item = observation;

                observation.classCode = "OBS";
                observation.moodCode = x_ActMoodDocumentObservation.EVN;


                observation.templateId = new List<II>
                {
                    new II { root = "2.16.840.1.113883.10.20.22.4.13"}
                };

                observation.id = new List<II>
                {
                    new II 
                    { 
                        root= StaticCcdaData["UniqueIdentifierID"]
                    }
                };

                observation.code = new CD
                {
                    nullFlavor = "UNK"
                };

                observation.statusCode = new CS
                {
                    nullFlavor = "NI"
                };

                observation.value = new List<ANY> 
                { 
                    new CD { nullFlavor= "NI"}
                };
            }

            sb.component.Add(proComponent);
        }

        private static void AddMedicationAdministeredComponent(StructuredBody sb, CCDADataModel objCCDAModel)
        {
            Component3 medicationComponent = HelperMethods.IntializeComponent3(
               new List<string> { "2.16.840.1.113883.10.20.22.2.38" },
               "29549-3", "2.16.840.1.113883.6.1", "LOINC", "Medications Administered", "MEDICATIONS ADMINISTERED");

            StrucDocText text = medicationComponent.section.text;
            List<Dictionary<string, string>> lstMedicationAdministered = objCCDAModel.lstMedicationsAdministered;

            if (objCCDAModel.lstMedicationsAdministered != null && objCCDAModel.lstMedicationsAdministered.Count > 0)
            {

                DataTable dtMedications = new DataTable(); // Medication table

                var table = new StrucDocTable();
                text.Items.Add(table);

                table.border = "1";
                table.width = "100%";

                table.thead = new StrucDocThead();
                table.thead.tr = new List<StrucDocTr>();

                var headerTr = new StrucDocTr();
                table.thead.tr.Add(headerTr);
                headerTr.Items = new List<object>();
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Medication" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Directions" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Start Date" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Status" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Indications" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Fill Instructions" } });

                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);

                tbody.tr = new List<StrucDocTr>();

                int id = 0;
                foreach (Dictionary<string, string> medication in lstMedicationAdministered)
                {
                    var bodyTr = new StrucDocTr();
                    bodyTr.Items = new List<object>();


                    var StartDate = medication["StartDate"];
                    if (StartDate != string.Empty)
                    {
                        StartDate = Convert.ToDateTime(StartDate).ToString("yyyyMMdd");
                    }
                    bodyTr.Items.Add(
                        new StrucDocTd
                        {
                            Items = new List<object> 
                        { 
                            new StrucDocContent 
                            { 
                                ID = String.Concat("MedicationAdminst_", ++id), 
                                Text = new List<string> { medication["Medication"] } 
                            } 
                        }
                        }
                        );

                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["Directions"] } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { StartDate } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["Status"] } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["Indication"] } });
                    string substituation = medication["Substitution"];
                    if (substituation.ToLower() == "n")
                    {
                        bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { "Generic Substitution not Allowed" } });
                    }
                    else if (substituation.ToLower() == "y")
                    {
                        bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { "Generic Substitution Allowed" } });
                    }

                    tbody.tr.Add(bodyTr);
                }

                medicationComponent.section.entry = new List<Entry>();

                id = 0;
                for (var i = 0; i < lstMedicationAdministered.Count; i++)
                {
                    var medicationEntry = new Entry();
                    medicationEntry.typeCode = x_ActRelationshipEntry.DRIV;

                    medicationEntry.Item = SetMedicationActivity(lstMedicationAdministered[i], String.Concat("#MedicationAdminst_", ++id));
                    medicationComponent.section.entry.Add(medicationEntry);
                }
            }
            else
            {
                var noMedicationText = new StrucDocContent();
                text.Items.Add(noMedicationText);

                //Section not included
                if (lstMedicationAdministered == null || lstMedicationAdministered.Count == 0)
                {
                    noMedicationText.ID = "nomedAd1";
                    noMedicationText.Text = new List<string> { "Not included in document" };
                }

                medicationComponent.section.entry = new List<Entry>();
                var medicationEntry = new Entry();
                medicationComponent.section.entry.Add(medicationEntry);

                medicationEntry.typeCode = x_ActRelationshipEntry.DRIV;
                var medicationActivity = new SubstanceAdministration();
                medicationEntry.Item = medicationActivity;

                medicationActivity.classCode = "SBADM";
                medicationActivity.moodCode = x_DocumentSubstanceMood.EVN;

                medicationActivity.nullFlavor = "NI";

                medicationActivity.templateId = new List<II>
                { 
                    new II { root = "2.16.840.1.113883.10.20.22.4.16" } 
                };

                medicationActivity.id = new List<II> 
                { 
                    new II { root = StaticCcdaData["UniqueIdentifierID"] } 
                };

                medicationActivity.statusCode = new CS { code = "completed" };
                medicationActivity.effectiveTime = new List<SXCM_TS>
                {
                    new IVL_TS 
                    {
                        ItemsElementName = new ItemsChoiceType2[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[] { new IVXB_TS { nullFlavor = "NI" }, new IVXB_TS { nullFlavor = "NI" } }
                    }
                };

                var consumable = new Consumable();
                medicationActivity.consumable = consumable;

                var manufacturedProduct = new ManufacturedProduct();
                consumable.manufacturedProduct = manufacturedProduct;

                manufacturedProduct.classCode = RoleClassManufacturedProduct.MANU;
                manufacturedProduct.classCodeSpecified = true;
                manufacturedProduct.templateId = new List<II> { new II { root = "2.16.840.1.113883.10.20.22.4.23" } };

                var manufacturedMaterial = new Material();
                manufacturedProduct.Item = manufacturedMaterial;

                manufacturedMaterial.code = new CE
                {
                    nullFlavor = "NI",
                };
            }

            sb.component.Add(medicationComponent);
        }

        private static SubstanceAdministration SetMedicationActivity(DataRow dr, String referenceValue = "")
        {
            var medicationActivity = new SubstanceAdministration();

            medicationActivity.classCode = "SBADM";
            medicationActivity.moodCode = x_DocumentSubstanceMood.EVN;
            medicationActivity.templateId = new List<II>
            { 
                new II { root = "2.16.840.1.113883.10.20.22.4.16" } 
            };

            medicationActivity.id = new List<II> 
            { 
                new II { root = StaticCcdaData["UniqueIdentifierID"] } 
            };

            medicationActivity.text = new ED
            {
                reference = new TEL { value = referenceValue },
                Text = new List<string> { Convert.ToString(dr["Medication"]) }
            };

            medicationActivity.statusCode = new CS { code = Convert.ToString(dr["Status"]) };

            DateTime lowEffectiveTime = Convert.ToDateTime(dr["StartDate"]);
            DateTime highEffectiveTime = Convert.ToDateTime(dr["EndDate"]);

            medicationActivity.effectiveTime = new List<SXCM_TS>();

            var effectiveTime = new IVL_TS();
            effectiveTime.ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high, };
            effectiveTime.Items = new QTY[2];

            if (lowEffectiveTime != DateTime.MinValue)
                effectiveTime.Items[0] = new IVXB_TS { value = lowEffectiveTime.ToString("yyyyMMdd") };
            else
                effectiveTime.Items[0] = new IVXB_TS { nullFlavor = "UNK" };

            if (highEffectiveTime != DateTime.MinValue)
                effectiveTime.Items[1] = new IVXB_TS { value = highEffectiveTime.ToString("yyyyMMdd") };
            else
                effectiveTime.Items[1] = new IVXB_TS { nullFlavor = "UNK" };

            medicationActivity.effectiveTime.Add(effectiveTime);

            if (!String.IsNullOrWhiteSpace(Convert.ToString(dr["NoOfTimes"])) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["FrequencyDescription"])))
            {
                string noOfTime = Convert.ToString(dr["NoOfTimes"]);

                string frequencyDescription = Convert.ToString(dr["FrequencyDescription"]);

                if (frequencyDescription.Contains("day"))
                {
                    frequencyDescription = "d";
                }
                else if (frequencyDescription.Contains("hour"))
                {
                    frequencyDescription = "h";
                }
                else if (frequencyDescription.Contains("week"))
                {
                    frequencyDescription = "w";
                }
                else if (frequencyDescription.Contains("month"))
                {
                    frequencyDescription = "m";
                }

                medicationActivity.effectiveTime.Add(new PIVL_TS()
                {
                    @operator = SetOperator.A,
                    period = new PQ
                    {
                        value = noOfTime,
                        unit = frequencyDescription
                    }
                });
            }

            string refill;
            if (string.IsNullOrWhiteSpace(Convert.ToString(dr["Refill"])) || Convert.ToString(dr["Refill"]) == "0")
            {
                refill = "0";
            }
            else
            {
                refill = Convert.ToString(dr["Refill"]);
            }


            if (!String.IsNullOrEmpty(refill))
            {
                medicationActivity.repeatNumber = new IVL_INT { value = refill };
            }

            if (!String.IsNullOrWhiteSpace(Convert.ToString(dr["RouteCode"]))
                && !String.IsNullOrWhiteSpace(Convert.ToString(dr["RouteDescription"])))
            {
                medicationActivity.routeCode = new CE
                {
                    code = Convert.ToString(dr["RouteCode"]),
                    codeSystem = "2.16.840.1.113883.3.26.1.1",
                    codeSystemName = "NCI Thesaurus",
                    displayName = Convert.ToString(dr["RouteDescription"])
                };
            }

            if (!String.IsNullOrWhiteSpace(Convert.ToString(dr["Dose"])))
            {
                medicationActivity.doseQuantity = new IVL_PQ();
                medicationActivity.doseQuantity.value = Convert.ToString(dr["Dose"]).TrimEnd('0', '.');
            }


            Consumable consumable = new Consumable();
            medicationActivity.consumable = consumable;

            ManufacturedProduct medicationInfo = new ManufacturedProduct();
            consumable.manufacturedProduct = medicationInfo;

            medicationInfo.classCodeSpecified = true;
            medicationInfo.classCode = RoleClassManufacturedProduct.MANU;
            medicationInfo.templateId = new List<II>() { new II { root = "2.16.840.1.113883.10.20.22.4.23" } };

            Material manufacturedMaterial = new Material();
            medicationInfo.Item = manufacturedMaterial;

            manufacturedMaterial.code = new CE
            {
                code = Convert.ToString(dr["RxNormID"]),
                codeSystem = "2.16.840.1.113883.6.88",
                displayName = Convert.ToString(dr["Medication"])
            };


            medicationActivity.performer = new List<Performer2>();
            var performer2 = new Performer2();

            performer2.assignedEntity = new AssignedEntity();
            performer2.assignedEntity.id = new List<II>();
            performer2.assignedEntity.id.Add(new II { nullFlavor = "UNK" });

            performer2.assignedEntity.addr = new List<AD>
            {
                new AD { nullFlavor = "UNK" }
            };

            performer2.assignedEntity.telecom = new List<TEL>() 
            { 
                new TEL { nullFlavor = "UNK" }
            };

            var organization = new Organization();
            organization.id = new List<II>();

            organization.id.Add(new II
            {
                root = StaticCcdaData["UniqueIdentifierID"]
            });

            organization.name = new List<ON>()
            {
                new ON
                {
                    Text = new List<string>{ Convert.ToString(dr["LocationName"]) }
                }
            };

            organization.telecom = new List<TEL>() 
            { 
                new TEL { nullFlavor = "UNK" } 
            };

            organization.addr = new List<AD>
            {
                new AD
                {
                    nullFlavor = "UNK"
                }
            };

            performer2.assignedEntity.representedOrganization = organization;

            medicationActivity.performer.Add(performer2);

            return medicationActivity;
        }

        private static void AddProblemListComponent(StructuredBody sb, CCDADataModel objCCDAModel)
        {
            Component3 problemListComponent = HelperMethods.IntializeComponent3(
                  new List<string> { "2.16.840.1.113883.10.20.22.2.5", "2.16.840.1.113883.10.20.22.2.5.1" },
                  "11450-4", "2.16.840.1.113883.6.1", "LOINC", "Problems", "PROBLEMS");

            StrucDocText text = problemListComponent.section.text;

            List<Dictionary<string, string>> lstProblemList = objCCDAModel.lstProblems;

            if (lstProblemList != null && lstProblemList.Count > 0)
            {

                StrucDocTable table = new StrucDocTable();
                table.width = "100%";
                table.border = "1";
                table.thead = new StrucDocThead
                {
                    tr = new List<StrucDocTr> 
                    {
                        new StrucDocTr
                        {
                            Items = new List<object>
                            {
                                new StrucDocTh { Text = new List<string> { "Problem" } },
                                new StrucDocTh { Text = new List<string> { "Status" } },
                                new StrucDocTh { Text = new List<string> { "Effective Date" } },
                                new StrucDocTh { Text = new List<string> { "Type" } }
                            }
                        }
                    }
                };

                table.tbody = new List<StrucDocTbody>();
                StrucDocTbody tBody = new StrucDocTbody();
                tBody.tr = new List<StrucDocTr>();
                table.tbody.Add(tBody);

                text.Items.Add(table);


                int counter = lstProblemList.Count; //;Problem List Count

                for (int i = 0; i < counter; i++)
                {
                    //DataRow dr = dtProblems.Rows[i];

                    StrucDocTr tr = new StrucDocTr();
                    tr.Items = new List<object>();
                    tBody.tr.Add(tr);

                    tr.Items.Add(new StrucDocTd
                    {
                        ID = string.Concat("problem", i + 1),
                        Text = new List<string> { lstProblemList[i]["ProblemName"] }
                    });

                    string status = lstProblemList[i]["Status"];
                    if (status != "Resolved" && status != "Inactive")
                    {
                        status = "Active";
                    }

                    tr.Items.Add(new StrucDocTd
                    {
                        ID = string.Concat("stat", i + 1),
                        Text = new List<string> { status }
                    });
                    var EffectiveDate = lstProblemList[i]["EffectiveDate"];
                    if (!string.IsNullOrWhiteSpace(EffectiveDate))
                        EffectiveDate = Convert.ToDateTime(EffectiveDate).ToString("MMMM dd, yyyy");

                    tr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> { EffectiveDate }
                    });

                    tr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> { "Problem" }
                    });


                }
                problemListComponent.section.text = text;

                problemListComponent.section.entry = new List<Entry>();


                for (int i = 0; i < lstProblemList.Count; i++)
                {
                    Dictionary<string, string> problem = lstProblemList[i];

                    var problemEntry = new Entry();
                    problemEntry.typeCode = x_ActRelationshipEntry.DRIV;

                    var concernAct = new Act();
                    concernAct.classCode = x_ActClassDocumentEntryAct.ACT;
                    concernAct.moodCode = x_DocumentActMood.EVN;
                    concernAct.templateId = new List<II> 
                    { 
                        new II { root = "2.16.840.1.113883.10.20.22.4.3" } 
                    };

                    concernAct.id = new List<II> 
                    { 
                        new II { root = StaticCcdaData["UniqueIdentifierID"] } 
                    };

                    concernAct.code = new CD
                    {
                        code = "CONC",
                        codeSystem = "2.16.840.1.113883.5.6",
                        displayName = "Concern"
                    };

                    concernAct.statusCode = new CS
                    {
                        code = "completed"
                    };

                    concernAct.effectiveTime = new IVL_TS
                    {
                        ItemsElementName = new ItemsChoiceType2[2],
                        Items = new QTY[2]
                    };
                    var EffectiveDate = problem["EffectiveDate"];
                    if (!string.IsNullOrWhiteSpace(EffectiveDate))
                        EffectiveDate = HelperMethods.SetDateTime(Convert.ToDateTime(EffectiveDate));
                    else
                        EffectiveDate = HelperMethods.SetDateTime(DateTime.Now);
                    string lowTime = EffectiveDate;
                    if (!String.IsNullOrWhiteSpace(lowTime))
                    {
                        concernAct.effectiveTime.ItemsElementName[0] = ItemsChoiceType2.low;
                        concernAct.effectiveTime.Items[0] = new IVXB_TS { value = lowTime };
                    }

                    concernAct.entryRelationship = new List<EntryRelationship>();
                    var problemEntryRelationship = new EntryRelationship();
                    problemEntryRelationship.typeCode = x_ActRelationshipEntryRelationship.SUBJ;

                    var problemObservation = new Observation();
                    problemEntryRelationship.Item = problemObservation;

                    problemObservation.classCode = "OBS";
                    problemObservation.moodCode = x_ActMoodDocumentObservation.EVN;

                    problemObservation.templateId = new List<II> 
                    { 
                        new II { root = "2.16.840.1.113883.10.20.22.4.4" } 
                    };

                    problemObservation.id = new List<II> 
                    { 
                        new II { root = StaticCcdaData["UniqueIdentifierID"] } 
                    };

                    problemObservation.code = new CD
                    {
                        code = "55607006",
                        codeSystem = "2.16.840.1.113883.6.96",
                        displayName = "Problem"
                    };

                    problemObservation.text = new ED
                    {
                        reference = new TEL { value = String.Concat("#problem", i + 1) }
                    };

                    problemObservation.statusCode = new CS
                    {
                        code = "completed"
                    };
                    EffectiveDate = problem["EffectiveDate"];
                    if (!string.IsNullOrWhiteSpace(EffectiveDate))
                        EffectiveDate = HelperMethods.SetDateTime(Convert.ToDateTime(EffectiveDate));
                    else
                        EffectiveDate = HelperMethods.SetDateTime(DateTime.Now);

                    lowTime = EffectiveDate;
                    if (!String.IsNullOrWhiteSpace(lowTime))
                    {
                        problemObservation.effectiveTime = new IVL_TS();
                        problemObservation.effectiveTime.Items = new QTY[1];
                        problemObservation.effectiveTime.ItemsElementName = new ItemsChoiceType2[1];

                        problemObservation.effectiveTime.ItemsElementName[0] = ItemsChoiceType2.low;
                        problemObservation.effectiveTime.Items[0] = new IVXB_TS { value = lowTime };
                    }

                    if (!String.IsNullOrEmpty(Convert.ToString(problem["SNOMEDID"])))
                    {
                        problemObservation.value = new List<ANY>();
                        problemObservation.value.Add(new CD
                        {
                            code = Convert.ToString(problem["SNOMEDID"]),
                            codeSystem = "2.16.840.1.113883.6.96",
                            displayName = Convert.ToString(problem["ProblemName"])
                        });
                    }
                    else
                    {
                        problemObservation.value = new List<ANY>();
                        problemObservation.value.Add(new CD
                        {
                            nullFlavor = "UNK"
                        });
                    }

                    problemObservation.entryRelationship = new List<EntryRelationship>();
                    EntryRelationship entryProblemStatus = new EntryRelationship();
                    problemObservation.entryRelationship.Add(entryProblemStatus);

                    Observation problemStatusObs = new Observation();
                    entryProblemStatus.Item = problemStatusObs;

                    problemStatusObs.classCode = "OBS";
                    problemStatusObs.moodCode = x_ActMoodDocumentObservation.EVN;
                    problemStatusObs.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.6" }
                    };
                    problemStatusObs.code = new CD
                    {
                        code = "33999-4",
                        codeSystem = "2.16.840.1.113883.6.1",
                        displayName = "LOINC"
                    };

                    problemStatusObs.statusCode = new CS { code = "completed" };

                    string status = Convert.ToString(problem["Status"]);
                    string code = string.Empty;

                    if (status == "Resolved")
                    {
                        code = "413322009";
                    }
                    else if (status == "Inactive")
                    {
                        code = "73425007";
                    }
                    else
                    {
                        status = "Active";
                        code = "55561003";
                    }

                    problemStatusObs.value = new List<ANY>
                    {
                        new CD 
                        {
                            code = code,
                            codeSystem = "2.16.840.1.113883.6.96",
                            displayName = status
                        }
                    };

                    concernAct.entryRelationship.Add(problemEntryRelationship);
                    problemEntry.Item = concernAct;
                    problemListComponent.section.entry.Add(problemEntry);
                }
            }
            else
            {
                //Section not included
                if (lstProblemList == null || lstProblemList.Count == 0)
                {
                    text.ID = "noproblem";
                    text.Text = new List<string> { "Not included in document" };
                }

                problemListComponent.section.entry = new List<Entry>();
                var problemEntry = new Entry();
                problemEntry.typeCode = x_ActRelationshipEntry.DRIV;

                var concernAct = new Act();
                concernAct.classCode = x_ActClassDocumentEntryAct.ACT;
                concernAct.moodCode = x_DocumentActMood.EVN;
                concernAct.templateId = new List<II> 
                { 
                    new II { root = "2.16.840.1.113883.10.20.22.4.3" } 
                };

                concernAct.id = new List<II> 
                { 
                    new II { root = StaticCcdaData["UniqueIdentifierID"] } 
                };

                concernAct.code = new CD
                {
                    code = "CONC",
                    codeSystem = "2.16.840.1.113883.5.6",
                    displayName = "Concern",
                };

                concernAct.statusCode = new CS
                {
                    code = "completed"
                };

                concernAct.effectiveTime = new IVL_TS
                {
                    ItemsElementName = new ItemsChoiceType2[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                    Items = new QTY[] { new IVXB_TS { nullFlavor = "NI" }, new IVXB_TS { nullFlavor = "NI" } }
                };

                concernAct.entryRelationship = new List<EntryRelationship>();
                var problemEntryRelationship = new EntryRelationship();
                problemEntryRelationship.typeCode = x_ActRelationshipEntryRelationship.SUBJ;

                var problemObservation = new Observation();
                problemEntryRelationship.Item = problemObservation;

                problemObservation.classCode = "OBS";
                problemObservation.moodCode = x_ActMoodDocumentObservation.EVN;

                problemObservation.nullFlavor = "NI";

                problemObservation.templateId = new List<II> 
                { 
                    new II { root = "2.16.840.1.113883.10.20.22.4.4" } 
                };

                problemObservation.id = new List<II> 
                { 
                    new II { root = StaticCcdaData["UniqueIdentifierID"] } 
                };

                problemObservation.code = new CD
                {
                    nullFlavor = "NI"
                };

                problemObservation.statusCode = new CS
                {
                    code = "completed"
                };

                problemObservation.value = new List<ANY>();
                problemObservation.value.Add(new CD
                {
                    nullFlavor = "NI"
                });

                concernAct.entryRelationship.Add(problemEntryRelationship);
                problemEntry.Item = concernAct;
                problemListComponent.section.entry.Add(problemEntry);
            }

            sb.component.Add(problemListComponent);
        }

        private static void AddVitalSignsComponent(StructuredBody sb, CCDADataModel objCCDAModel)
        {
            Component3 vitalSignsComponent = HelperMethods.IntializeComponent3(
                new List<string> { "2.16.840.1.113883.10.20.22.2.4", "2.16.840.1.113883.10.20.22.2.4.1" },
                "8716-3", "2.16.840.1.113883.6.1", "LOINC", "Vital Signs", "VITAL SIGNS");

            StrucDocText text = vitalSignsComponent.section.text;

            List<Dictionary<string, string>> lstVitalSigns = objCCDAModel.lstVitals;

            if (lstVitalSigns != null && lstVitalSigns.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);

                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead();
                table.thead.tr = new List<StrucDocTr>();

                var headerTr = new StrucDocTr();
                headerTr.Items = new List<object>();
                headerTr.Items.Add(new StrucDocTh
                {
                    Text = new List<string> { "Date / Time" },
                    align = StrucDocThAlign.right,
                    alignSpecified = true
                });
                table.thead.tr.Add(headerTr);

                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);
                tbody.tr = new List<StrucDocTr>();

                var bodyTr = new StrucDocTr();
                bodyTr.Items = new List<object>();
                bodyTr.Items.Add(new StrucDocTh
                {
                    Text = new List<string> { "Height" },
                    align = StrucDocThAlign.left,
                    alignSpecified = true
                });
                tbody.tr.Add(bodyTr);

                bodyTr = new StrucDocTr();
                bodyTr.Items = new List<object>();
                bodyTr.Items.Add(new StrucDocTh
                {
                    Text = new List<string> { "Weight" },
                    align = StrucDocThAlign.left,
                    alignSpecified = true
                });
                tbody.tr.Add(bodyTr);

                bodyTr = new StrucDocTr();
                bodyTr.Items = new List<object>();
                bodyTr.Items.Add(new StrucDocTh
                {
                    Text = new List<string> { "Systolic BP" },
                    align = StrucDocThAlign.left,
                    alignSpecified = true
                });
                tbody.tr.Add(bodyTr);

                bodyTr = new StrucDocTr();
                bodyTr.Items = new List<object>();
                bodyTr.Items.Add(new StrucDocTh
                {
                    Text = new List<string> { "Diastolic BP" },
                    align = StrucDocThAlign.left,
                    alignSpecified = true
                });
                tbody.tr.Add(bodyTr);

                bodyTr = new StrucDocTr();
                bodyTr.Items = new List<object>();
                bodyTr.Items.Add(new StrucDocTh
                {
                    Text = new List<string> { "BMI" },
                    align = StrucDocThAlign.left,
                    alignSpecified = true
                });
                tbody.tr.Add(bodyTr);

                int id = 0;


                DataTable dtVitalSigns = new DataTable(); //Vitals List
                for (int i = 0; i < lstVitalSigns.Count; i++)
                {
                    Dictionary<string, string> dr = lstVitalSigns[i];

                    StrucDocTh th = new StrucDocTh();
                    table.thead.tr[0].Items.Add(th);

                    String date = Convert.ToDateTime(dr["createdOn"]).ToString("MMM dd, yyyy");
                    if (!String.IsNullOrWhiteSpace(date))
                    {
                        th.Text = new List<string> { date };
                    }

                    StrucDocTd td = new StrucDocTd();
                    table.tbody[0].tr[0].Items.Add(td);

                    var height = dr["Height"];
                    if (height == "0" || height == "")
                    {
                        height = string.Empty;
                    }
                    if (!String.IsNullOrWhiteSpace(height))
                    {
                        td.Items = new List<object>
                        {
                            new StrucDocContent
                            { 
                                ID = String.Concat("vit", ++id),
                                Text = new List<string> { height + " cm" }
                            }
                        };
                    }

                    td = new StrucDocTd();
                    table.tbody[0].tr[1].Items.Add(td);

                    var weight = dr["Weight"];
                    if (weight == "0" || weight == "")
                        weight = string.Empty;

                    if (!String.IsNullOrWhiteSpace(weight))
                    {
                        td.Items = new List<object>
                        {
                            new StrucDocContent
                            {
                                ID = String.Concat("vit", ++id),
                                Text = new List<string> { Convert.ToString(Math.Round((Convert.ToDouble(weight) / 2.2), 2), CultureInfo.InvariantCulture) + " kg" }
                            }
                        };
                    }

                    td = new StrucDocTd();
                    table.tbody[0].tr[2].Items.Add(td);

                    var sbp = dr["BPSystolic"];
                    if (!String.IsNullOrWhiteSpace(sbp))
                    {
                        sbp = sbp + " mmHg";
                        td.Items = new List<object>
                        {
                            new StrucDocContent
                            {
                                ID = String.Concat("vit", ++id),
                                Text = new List<string> { sbp }
                            }
                        };
                    }

                    td = new StrucDocTd();
                    table.tbody[0].tr[3].Items.Add(td);

                    var dbp = dr["BPDiastolic"];
                    if (!String.IsNullOrWhiteSpace(dbp))
                    {
                        dbp = dbp + " mmHg";
                        td.Items = new List<object>
                        {
                            new StrucDocContent
                            {
                                ID = String.Concat("vit", ++id),
                                Text = new List<string> { dbp }
                            }
                        };
                    }

                    td = new StrucDocTd();
                    table.tbody[0].tr[4].Items.Add(td);

                    var bmi = dr["BMI"];
                    if (bmi == "0" || bmi == "")
                        bmi = string.Empty;

                    if (!String.IsNullOrWhiteSpace(bmi))
                    {
                        td.Items = new List<object>
                        {
                            new StrucDocContent
                            {
                                ID = String.Concat("vit", ++id),
                                Text = new List<string> { bmi }
                            }
                        };
                    }
                }

                vitalSignsComponent.section.entry = new List<Entry>();
                id = 0;

                for (int i = 0; i < lstVitalSigns.Count; i++)
                {
                    Dictionary<string, string> dr = lstVitalSigns[i];

                    var vitalSignsOrganizerEntry = new Entry();
                    vitalSignsOrganizerEntry.typeCode = x_ActRelationshipEntry.DRIV;

                    var organizer = new Organizer();
                    vitalSignsOrganizerEntry.Item = organizer;
                    organizer.classCode = x_ActClassDocumentEntryOrganizer.CLUSTER;
                    organizer.moodCode = "EVN";

                    organizer.templateId = new List<II>();
                    organizer.templateId.Add(new II
                    {
                        root = "2.16.840.1.113883.10.20.22.4.26"
                    });

                    organizer.id = new List<II>();
                    organizer.id.Add(new II
                    {
                        root = StaticCcdaData["UniqueIdentifierID"]
                    });

                    organizer.code = new CD
                    {
                        code = "46680005",
                        displayName = "Vital Signs",
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED CT"
                    };

                    organizer.statusCode = new CS
                    {
                        code = "completed"
                    };

                    organizer.effectiveTime = new IVL_TS();
                    organizer.effectiveTime.value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd");


                    organizer.component = new List<Component4>();

                    var height = dr["Height"];
                    if (height == "0" || height == "")
                    {
                        height = "UNK";
                    }

                    var vitalSignObservationComponent = new Component4();
                    var vitalSignObservation = new Observation();
                    vitalSignObservationComponent.Item = vitalSignObservation;

                    vitalSignObservation.classCode = "OBS";
                    vitalSignObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    vitalSignObservation.templateId = new List<II> 
                    { 
                        new II { root = "2.16.840.1.113883.10.20.22.4.27" } 
                    };

                    vitalSignObservation.id = new List<II> 
                    { 
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };

                    vitalSignObservation.code = new CD
                    {
                        code = "8302-2",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "Height"
                    };

                    if (height != "UNK")
                    {
                        vitalSignObservation.text = new ED
                        {
                            reference = new TEL
                            {
                                value = String.Concat("#vit", ++id)
                            }
                        };
                    }

                    vitalSignObservation.statusCode = new CS { code = "completed" };
                    vitalSignObservation.effectiveTime = new IVL_TS();
                    vitalSignObservation.effectiveTime.value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd");

                    if (height != "UNK")
                    {
                        vitalSignObservation.value = new List<ANY> 
                        { 
                            new PQ 
                            { 
                                value = height,
                                unit = "cm" 
                            } 
                        };
                    }
                    else
                    {
                        vitalSignObservation.value = new List<ANY> 
                        { 
                            new PQ 
                            { 
                                nullFlavor = "UNK"
                            } 
                        };
                    }
                    organizer.component.Add(vitalSignObservationComponent);

                    var weight = dr["Weight"];
                    if (weight == "0" || weight == "")
                        weight = "UNK";

                    vitalSignObservationComponent = new Component4();
                    vitalSignObservation = new Observation();
                    vitalSignObservationComponent.Item = vitalSignObservation;

                    vitalSignObservation.classCode = "OBS";
                    vitalSignObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    vitalSignObservation.templateId = new List<II> 
                    { 
                        new II { root = "2.16.840.1.113883.10.20.22.4.27" } 
                    };
                    vitalSignObservation.id = new List<II> 
                    { 
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };

                    vitalSignObservation.code = new CD
                    {
                        code = "3141-9",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "Patient Body Weight - Measured"
                    };

                    if (weight != "UNK")
                    {
                        vitalSignObservation.text = new ED
                        {
                            reference = new TEL
                            {
                                value = String.Concat("#vit", ++id)
                            }
                        };
                    }

                    vitalSignObservation.statusCode = new CS { code = "completed" };
                    vitalSignObservation.effectiveTime = new IVL_TS();
                    vitalSignObservation.effectiveTime.value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd");

                    if (weight != "UNK")
                    {
                        vitalSignObservation.value = new List<ANY> 
                        { 
                            new PQ 
                            { 
                                value = Convert.ToString(Math.Round((Convert.ToDouble(dr["Weight"]) / 2.2), 2), CultureInfo.InvariantCulture),
                                unit = "kg" 
                            } 
                        };
                    }
                    else
                    {
                        vitalSignObservation.value = new List<ANY> 
                        { 
                            new PQ 
                            { 
                                nullFlavor = "UNK" 
                            } 
                        };
                    }

                    organizer.component.Add(vitalSignObservationComponent);

                    vitalSignObservationComponent = new Component4();
                    vitalSignObservation = new Observation();
                    vitalSignObservationComponent.Item = vitalSignObservation;

                    vitalSignObservation.classCode = "OBS";
                    vitalSignObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    vitalSignObservation.templateId = new List<II> 
                    { 
                        new II { root = "2.16.840.1.113883.10.20.22.4.27" } 
                    };

                    vitalSignObservation.id = new List<II> 
                    { 
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };

                    vitalSignObservation.code = new CD
                    {
                        code = "8480-6",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "BP Systolic"
                    };

                    var sbp = dr["BPSystolic"];
                    if (sbp == "0" || sbp == "")
                        sbp = "UNK";
                    if (sbp != "UNK")
                    {
                        vitalSignObservation.text = new ED
                        {
                            reference = new TEL
                            {
                                value = String.Concat("#vit", ++id)
                            }
                        };
                    }

                    vitalSignObservation.statusCode = new CS { code = "completed" };
                    vitalSignObservation.effectiveTime = new IVL_TS();
                    vitalSignObservation.effectiveTime.value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd");

                    if (sbp != "UNK")
                    {
                        vitalSignObservation.value = new List<ANY> 
                        { 
                            new PQ 
                            { 
                                value = sbp,
                                unit = "mmHg" 
                            } 
                        };
                    }
                    else
                    {
                        vitalSignObservation.value = new List<ANY> 
                        { 
                            new PQ 
                            { 
                                nullFlavor = "UNK"
                            } 
                        };
                    }

                    organizer.component.Add(vitalSignObservationComponent);


                    vitalSignObservationComponent = new Component4();
                    vitalSignObservation = new Observation();
                    vitalSignObservationComponent.Item = vitalSignObservation;

                    vitalSignObservation.classCode = "OBS";
                    vitalSignObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    vitalSignObservation.templateId = new List<II> 
                    { 
                        new II { root = "2.16.840.1.113883.10.20.22.4.27" } 
                    };

                    vitalSignObservation.id = new List<II> 
                    { 
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };

                    vitalSignObservation.code = new CD
                    {
                        code = "8462-4",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "BP Diastolic"
                    };

                    var dbp = dr["BPDiastolic"];
                    if (dbp == "0" || dbp == "")
                        dbp = "UNK";
                    if (dbp != "UNK")
                    {
                        vitalSignObservation.text = new ED
                        {
                            reference = new TEL
                            {
                                value = String.Concat("#vit", ++id)
                            }
                        };

                        vitalSignObservation.value = new List<ANY> 
                        { 
                            new PQ 
                            { 
                                value = dbp,
                                unit = "mmHg" 
                            } 
                        };
                    }
                    else
                    {
                        vitalSignObservation.value = new List<ANY> 
                        { 
                            new PQ 
                            { 
                                nullFlavor = "UNK"
                            } 
                        };
                    }

                    vitalSignObservation.statusCode = new CS { code = "completed" };
                    vitalSignObservation.effectiveTime = new IVL_TS();
                    vitalSignObservation.effectiveTime.value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd");

                    organizer.component.Add(vitalSignObservationComponent);

                    vitalSignObservationComponent = new Component4();
                    vitalSignObservation = new Observation();
                    vitalSignObservationComponent.Item = vitalSignObservation;

                    vitalSignObservation.classCode = "OBS";
                    vitalSignObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    vitalSignObservation.templateId = new List<II> 
                    { 
                        new II { root = "2.16.840.1.113883.10.20.22.4.27" } 
                    };

                    vitalSignObservation.id = new List<II> 
                    { 
                         new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };

                    vitalSignObservation.code = new CD
                    {
                        code = "39156-5",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "BMI (Body Mass Index)"
                    };

                    var bmi = dr["BMI"];
                    if (bmi == "0" || bmi == "")
                        bmi = "UNK";

                    if (bmi != "UNK")
                    {
                        vitalSignObservation.text = new ED
                        {
                            reference = new TEL
                            {
                                value = String.Concat("#vit", ++id)
                            }
                        };
                    }

                    vitalSignObservation.statusCode = new CS { code = "completed" };
                    vitalSignObservation.effectiveTime = new IVL_TS();
                    vitalSignObservation.effectiveTime.value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd");
                    if (bmi != "UNK")
                    {
                        vitalSignObservation.value = new List<ANY> 
                        { 
                            new PQ 
                            { 
                                value = bmi
                            } 
                        };
                    }
                    else
                    {
                        vitalSignObservation.value = new List<ANY> 
                        { 
                            new PQ 
                            { 
                                nullFlavor = "UNK"
                            } 
                        };
                    }

                    organizer.component.Add(vitalSignObservationComponent);

                    vitalSignsComponent.section.entry.Add(vitalSignsOrganizerEntry);
                }
            }
            else
            {
                text.Items.Add(new StrucDocContent { ID = "noVitalSign", Text = new List<string> { "Not indicated" } });

                vitalSignsComponent.section.entry = new List<Entry>();
                var vitalSignsOrganizerEntry = new Entry();
                vitalSignsComponent.section.entry.Add(vitalSignsOrganizerEntry);
                vitalSignsOrganizerEntry.typeCode = x_ActRelationshipEntry.DRIV;

                var organizer = new Organizer();
                vitalSignsOrganizerEntry.Item = organizer;
                organizer.classCode = x_ActClassDocumentEntryOrganizer.CLUSTER;
                organizer.moodCode = "EVN";

                organizer.templateId = new List<II>()
                {
                    new II{root = "2.16.840.1.113883.10.20.22.4.26"}
                };

                organizer.id = new List<II>()
                {
                    new II { nullFlavor = "UNK" }
                };

                organizer.code = new CD
                {
                    code = "46680005",
                    codeSystem = "2.16.840.1.113883.6.96",
                    codeSystemName = "SNOMED CT"
                };

                organizer.statusCode = new CS
                {
                    code = "completed"
                };

                organizer.effectiveTime = new IVL_TS() { nullFlavor = "UNK" };

                organizer.component = new List<Component4>();
                var vitalSignObservationComponent = new Component4();
                organizer.component.Add(vitalSignObservationComponent);

                var vitalSignObservation = new Observation();
                vitalSignObservationComponent.Item = vitalSignObservation;
                vitalSignObservation.classCode = "OBS";
                vitalSignObservation.moodCode = x_ActMoodDocumentObservation.EVN;

                vitalSignObservationComponent.Item = vitalSignObservation;

                vitalSignObservation.templateId = new List<II> 
                { 
                    new II { root = "2.16.840.1.113883.10.20.22.4.27" } 
                };
                vitalSignObservation.id = new List<II> 
                { 
                    new II { nullFlavor = "UNK" } 
                };
                vitalSignObservation.code = new CD
                {
                    nullFlavor = "UNK"
                };

                vitalSignObservation.statusCode = new CS { code = "completed" };
                vitalSignObservation.effectiveTime = new IVL_TS() { nullFlavor = "UNK" };
                vitalSignObservation.value = new List<ANY> { new PQ { nullFlavor = "UNK" } };
            }

            sb.component.Add(vitalSignsComponent);
        }

        private static void AddAllergiesComponent(StructuredBody sb, CCDADataModel objCCDAModel)
        {
            Component3 medicationAllergiesComponent = HelperMethods.IntializeComponent3
            (new List<string> { "2.16.840.1.113883.10.20.22.2.6", "2.16.840.1.113883.10.20.22.2.6.1" },
               "48765-2", "2.16.840.1.113883.6.1", "LOINC", "Allergies, adverse reactions, alerts", "ALLERGIES");

            StrucDocText text = medicationAllergiesComponent.section.text;

            List<Dictionary<string, string>> lstAllergies = objCCDAModel.lstAllergs;

            if (lstAllergies != null && lstAllergies.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);

                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead();
                table.thead.tr = new List<StrucDocTr>();

                var headerTr = new StrucDocTr();
                table.thead.tr.Add(headerTr);
                headerTr.Items = new List<object>();
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Substance" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Overall Severity" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Reaction" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Reaction Severity" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Status" } });

                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);

                tbody.tr = new List<StrucDocTr>();
                int counter = lstAllergies.Count;

                for (int i = 0; i < counter; i++)
                {
                    var bodyTr = new StrucDocTr();
                    bodyTr.Items = new List<object>();

                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { lstAllergies[i]["Substance"] } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { lstAllergies[i]["Severity"] } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { lstAllergies[i]["Reaction"] } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { lstAllergies[i]["Severity"] } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { lstAllergies[i]["Status"] } });

                    tbody.tr.Add(bodyTr);
                }


                medicationAllergiesComponent.section.entry = new List<Entry>();

                for (int i = 0; i < counter; i++)
                {

                    var allergyProblemActEntry = new Entry();
                    allergyProblemActEntry.typeCode = x_ActRelationshipEntry.DRIV;

                    var allergyProblemAct = new Act();
                    allergyProblemActEntry.Item = allergyProblemAct;

                    allergyProblemAct.classCode = x_ActClassDocumentEntryAct.ACT;
                    allergyProblemAct.moodCode = x_DocumentActMood.EVN;

                    allergyProblemAct.templateId = new List<II> 
                    { 
                        new II { root = "2.16.840.1.113883.10.20.22.4.30" } 
                    };

                    allergyProblemAct.id = new List<II> 
                    { 
                        new II { root = StaticCcdaData["UniqueIdentifierID"] } 
                    };

                    allergyProblemAct.code = new CD
                    {
                        code = "48765-2",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "Allergies, adverse reactions, alerts"
                    };

                    Boolean status = lstAllergies[i]["Status"].ToLower() == "active" ? true : false;

                    if (status)
                    {
                        allergyProblemAct.statusCode = new CS
                        {
                            code = "active"
                        };
                    }
                    else
                    {
                        allergyProblemAct.statusCode = new CS
                        {
                            code = "completed"
                        };
                    }

                    DateTime lowTime = Convert.ToDateTime(lstAllergies[i]["CreatedOn"]);
                    DateTime highTime = Convert.ToDateTime(lstAllergies[i]["ResolvedDate"]);

                    allergyProblemAct.effectiveTime = new IVL_TS();
                    if (status && lowTime != DateTime.MinValue)
                    {
                        allergyProblemAct.effectiveTime.ItemsElementName = new ItemsChoiceType2[] 
                        { 
                            ItemsChoiceType2.low 
                        };

                        allergyProblemAct.effectiveTime.Items = new QTY[]
                        {
                            new IVXB_TS { value = lowTime.ToString("yyyyMMdd") }
                        };
                    }
                    else if (!status && highTime != DateTime.MinValue)
                    {
                        allergyProblemAct.effectiveTime.ItemsElementName = new ItemsChoiceType2[] 
                        { 
                            ItemsChoiceType2.high 
                        };

                        allergyProblemAct.effectiveTime.Items = new QTY[]    
                        {
                            new IVXB_TS { value = highTime.ToString("yyyyMMdd") }
                        };
                    }


                    allergyProblemAct.entryRelationship = new List<EntryRelationship>();

                    var allergyIntoleranceOnservationEntry = new EntryRelationship();

                    allergyIntoleranceOnservationEntry.typeCode = x_ActRelationshipEntryRelationship.SUBJ;
                    var allergyIntoleranceObservation = new Observation();


                    allergyIntoleranceObservation.classCode = "OBS";
                    allergyIntoleranceObservation.moodCode = x_ActMoodDocumentObservation.EVN;

                    allergyIntoleranceObservation.templateId = new List<II> 
                    { 
                        new II { root = "2.16.840.1.113883.10.20.22.4.7" } 
                    };

                    allergyIntoleranceObservation.id = new List<II> 
                    { 
                        new II { root = StaticCcdaData["UniqueIdentifierID"] } 
                    };

                    allergyIntoleranceObservation.code = new CD
                    {
                        code = "ASSERTION",
                        codeSystem = "2.16.840.1.113883.5.4",
                    };

                    allergyIntoleranceObservation.statusCode = new CS
                    {
                        code = "completed",
                    };

                    allergyIntoleranceObservation.effectiveTime = new IVL_TS();
                    allergyIntoleranceObservation.effectiveTime.ItemsElementName = new[] 
                    { 
                        ItemsChoiceType2.low, 
                        ItemsChoiceType2.high 
                    };

                    allergyIntoleranceObservation.effectiveTime.Items = new QTY[2];

                    if (lowTime != DateTime.MinValue)
                    {
                        allergyIntoleranceObservation.effectiveTime.Items[0] = new IVXB_TS
                        {
                            value = lowTime.ToString("yyyyMMdd")
                        };
                    }
                    else
                    {
                        allergyIntoleranceObservation.effectiveTime.Items[0] = new IVXB_TS
                        {
                            nullFlavor = "UNK"
                        };
                    }

                    if (highTime != DateTime.MinValue)
                    {
                        allergyIntoleranceObservation.effectiveTime.Items[1] = new IVXB_TS
                        {
                            value = highTime.ToString("yyyyMMdd")
                        };
                    }

                    allergyIntoleranceObservation.value = new List<ANY>();
                    CD allergyType = new CD
                    {
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED CT",
                        code = "416098002",
                        displayName = "Drug allergy (disorder)"
                    };

                    if (!String.IsNullOrWhiteSpace(lstAllergies[i]["Reaction"]))
                    {
                        allergyType.originalText = new ED
                        {
                            reference = new TEL
                            {
                                value = String.Concat("#reaction", i + 1)
                            }
                        };
                    }

                    allergyIntoleranceObservation.value.Add(allergyType);


                    if (!String.IsNullOrWhiteSpace(lstAllergies[i]["Substance"]))
                    {
                        allergyIntoleranceObservation.participant = new List<Participant2>();

                        var participant = new Participant2()
                        {
                            typeCode = "CSM"
                        };

                        participant.participantRole = new ParticipantRole();
                        participant.participantRole.classCode = "MANU";

                        var playingEntity = new PlayingEntity()
                        {
                            classCode = "MMAT",
                            code = new CE()
                            {
                                code = Convert.ToString(lstAllergies[i]["RxNormID"]), //Not available right now
                                displayName = lstAllergies[i]["Substance"],
                                codeSystem = "2.16.840.1.113883.6.88",
                                codeSystemName = "RxNorm",
                            }
                        };

                        if (!String.IsNullOrWhiteSpace(lstAllergies[i]["Reaction"]))
                        {
                            playingEntity.code.originalText = new ED
                            {
                                reference = new TEL
                                {
                                    value = String.Concat("#reaction", i + 1)
                                }
                            };
                        }

                        participant.participantRole.Item = playingEntity;
                        allergyIntoleranceObservation.participant.Add(participant);
                    }


                    allergyIntoleranceObservation.entryRelationship = new List<EntryRelationship>();

                    var allergyStatusRelationship = new EntryRelationship();
                    allergyStatusRelationship.typeCode = x_ActRelationshipEntryRelationship.SUBJ;
                    allergyStatusRelationship.inversionInd = true;
                    allergyStatusRelationship.inversionIndSpecified = true;

                    var allergyStatusObservation = new Observation();

                    allergyStatusObservation.classCode = "OBS";
                    allergyStatusObservation.moodCode = x_ActMoodDocumentObservation.EVN;

                    allergyStatusObservation.templateId = new List<II>()
                    { 
                        new II{ root = "2.16.840.1.113883.10.20.22.4.28" }
                    };

                    allergyStatusObservation.code = new CD
                    {
                        code = "33999-4",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "Status"
                    };

                    allergyStatusObservation.statusCode = new CS
                    {
                        code = "completed"
                    };

                    allergyStatusObservation.value = new List<ANY>();
                    var allergyStatusValue = new CD();

                    if (status)
                    {
                        allergyStatusValue.code = "55561003";
                        allergyStatusValue.displayName = "Active";
                    }
                    else
                    {
                        allergyStatusValue.code = "73425007";
                        allergyStatusValue.displayName = "Inactive";
                    }

                    allergyStatusValue.codeSystem = "2.16.840.1.113883.6.96";
                    allergyStatusValue.codeSystemName = "SNOMED CT";

                    allergyStatusObservation.value.Add(allergyStatusValue);

                    allergyStatusRelationship.Item = allergyStatusObservation;
                    allergyIntoleranceObservation.entryRelationship.Add(allergyStatusRelationship);


                    if (!String.IsNullOrWhiteSpace(lstAllergies[i]["Reaction"]))
                    {
                        var allergyReactionRelationship = new EntryRelationship();
                        allergyReactionRelationship.typeCode = x_ActRelationshipEntryRelationship.MFST;
                        allergyReactionRelationship.inversionInd = true;
                        allergyReactionRelationship.inversionIndSpecified = true;

                        var allergyReactionObservation = new Observation();
                        allergyReactionRelationship.Item = allergyReactionObservation;

                        allergyReactionObservation.classCode = "OBS";
                        allergyReactionObservation.moodCode = x_ActMoodDocumentObservation.EVN;

                        allergyReactionObservation.templateId = new List<II>()
                        {
                            new II { root = "2.16.840.1.113883.10.20.22.4.9" }
                        };

                        allergyReactionObservation.id = new List<II>() 
                        { 
                            new II { root = StaticCcdaData["UniqueIdentifierID"] }
                        };

                        allergyReactionObservation.code = new CD
                        {
                            nullFlavor = "NA"
                        };

                        allergyReactionObservation.text = new ED
                        {
                            reference = new TEL { value = String.Concat("#reaction", i + 1) }
                        };

                        allergyReactionObservation.statusCode = new CS
                        {
                            code = "completed",
                        };

                        if (!status)
                        {
                            allergyReactionObservation.effectiveTime = new IVL_TS();

                            allergyReactionObservation.effectiveTime.ItemsElementName = new[] 
                            { 
                                ItemsChoiceType2.low, 
                                ItemsChoiceType2.high 
                            };

                            allergyReactionObservation.effectiveTime.Items = new QTY[2];

                            if (lowTime != DateTime.MinValue)
                            {
                                allergyReactionObservation.effectiveTime.Items[0] = new IVXB_TS
                                {
                                    value = lowTime.ToString("yyyyMMdd")
                                };
                            }

                            if (highTime != DateTime.MinValue)
                            {
                                allergyReactionObservation.effectiveTime.Items[1] = new IVXB_TS
                                {
                                    value = highTime.ToString("yyyyMMdd")
                                };
                            }
                        }

                        allergyReactionObservation.value = new List<ANY>()
                        {
                            new CD() 
                            {
                                code = "416098002",
                                displayName =  lstAllergies[i]["Reaction"],
                                codeSystem = "2.16.840.1.113883.6.96",
                                codeSystemName = "SNOMED CT"
                            }
                        };

                        if (!String.IsNullOrWhiteSpace(lstAllergies[i]["Severity"]))
                        {
                            allergyReactionObservation.entryRelationship = new List<EntryRelationship>();

                            var allergyReactionSeverityRelationship = new EntryRelationship();

                            allergyReactionSeverityRelationship.typeCode = x_ActRelationshipEntryRelationship.SUBJ;
                            allergyReactionSeverityRelationship.inversionInd = true;
                            allergyReactionSeverityRelationship.inversionIndSpecified = true;

                            var allergyReactionSeverityObservation = new Observation();
                            allergyReactionSeverityRelationship.Item = allergyReactionSeverityObservation;

                            allergyReactionSeverityObservation.classCode = "OBS";
                            allergyReactionSeverityObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                            allergyReactionSeverityObservation.templateId = new List<II>() 
                            { 
                                new II { root = "2.16.840.1.113883.10.20.22.4.8" }
                            };

                            allergyReactionSeverityObservation.code = new CE
                            {
                                code = "SEV",
                                codeSystem = "2.16.840.1.113883.5.4",
                                codeSystemName = "ActCode",
                                displayName = "Severity Observation"
                            };

                            allergyReactionSeverityObservation.text = new ED()
                            {
                                reference = new TEL { value = "#severity" + (i * 2 + 2) }
                            };

                            allergyReactionSeverityObservation.statusCode = new CS
                            {
                                code = "completed",
                            };

                            allergyReactionSeverityObservation.value = new List<ANY>();
                            CD allergyReaction = new CD();

                            allergyReaction.codeSystem = "2.16.840.1.113883.6.96";
                            allergyReaction.codeSystemName = "SNOMED CT";


                            if (lstAllergies[i]["Severity"].ToLower() == "mild")
                            {
                                allergyReaction.code = "255604002";
                                allergyReaction.displayName = "Mild";
                            }
                            else if (lstAllergies[i]["Severity"].ToLower() == "moderate")
                            {
                                allergyReaction.code = "6736007";
                                allergyReaction.displayName = "Moderate";
                            }
                            else if (lstAllergies[i]["Severity"].ToLower() == "sever")
                            {
                                allergyReaction.code = "24484000 ";
                                allergyReaction.displayName = "Severe";
                            }
                            else
                            {
                                allergyReaction = new CD();
                                allergyReaction.nullFlavor = "UNK";
                            }

                            allergyReactionSeverityObservation.value.Add(allergyReaction);

                            allergyReactionObservation.entryRelationship.Add(allergyReactionSeverityRelationship);
                        }

                        allergyIntoleranceObservation.entryRelationship.Add(allergyReactionRelationship);
                    }

                    allergyIntoleranceOnservationEntry.Item = allergyIntoleranceObservation;
                    allergyProblemAct.entryRelationship.Add(allergyIntoleranceOnservationEntry);
                    medicationAllergiesComponent.section.entry.Add(allergyProblemActEntry);
                }
            }
            else
            {
                text.Items.Add(new StrucDocContent { ID = "noAllergy", Text = new List<string> { "Not included in document" } });

                medicationAllergiesComponent.section.entry = new List<Entry>();

                var allergyProblemActEntry = new Entry();
                allergyProblemActEntry.typeCode = x_ActRelationshipEntry.DRIV;

                var allergyProblemAct = new Act();
                allergyProblemActEntry.Item = allergyProblemAct;

                allergyProblemAct.classCode = x_ActClassDocumentEntryAct.ACT;
                allergyProblemAct.moodCode = x_DocumentActMood.EVN;

                allergyProblemAct.templateId = new List<II> 
                { 
                    new II { root = "2.16.840.1.113883.10.20.22.4.30" } 
                };

                allergyProblemAct.id = new List<II> 
                { 
                    new II { root = StaticCcdaData["UniqueIdentifierID"] } 
                };

                allergyProblemAct.code = new CD
                {
                    code = "48765-2",
                    codeSystem = "2.16.840.1.113883.6.1",
                    codeSystemName = "LOINC",
                    displayName = "Allergies, adverse reactions, alerts"
                };

                allergyProblemAct.statusCode = new CS
                {
                    code = "completed"
                };

                allergyProblemAct.effectiveTime = new IVL_TS()
                {
                    ItemsElementName = new ItemsChoiceType2[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                    Items = new QTY[] { new IVXB_TS { nullFlavor = "NI" }, new IVXB_TS { nullFlavor = "NI" } }
                };

                allergyProblemAct.entryRelationship = new List<EntryRelationship>();

                var allergyIntoleranceOnservationEntry = new EntryRelationship();
                allergyProblemAct.entryRelationship.Add(allergyIntoleranceOnservationEntry);

                allergyIntoleranceOnservationEntry.typeCode = x_ActRelationshipEntryRelationship.SUBJ;

                var allergyIntoleranceObservation = new Observation();
                allergyIntoleranceOnservationEntry.Item = allergyIntoleranceObservation;

                allergyIntoleranceObservation.classCode = "OBS";
                allergyIntoleranceObservation.moodCode = x_ActMoodDocumentObservation.EVN;

                allergyIntoleranceObservation.templateId = new List<II> 
                { 
                    new II { root = "2.16.840.1.113883.10.20.22.4.7" } 
                };

                allergyIntoleranceObservation.id = new List<II> 
                { 
                    new II { root = StaticCcdaData["UniqueIdentifierID"] } 
                };

                allergyIntoleranceObservation.code = new CD
                {
                    code = "ASSERTION",
                    codeSystem = "2.16.840.1.113883.5.4",
                };

                allergyIntoleranceObservation.statusCode = new CS
                {
                    code = "completed"
                };

                allergyIntoleranceObservation.effectiveTime = new IVL_TS()
                {
                    nullFlavor = "NI"
                };

                allergyIntoleranceObservation.value = new List<ANY>() 
                { 
                    new CD
                    {
                        nullFlavor = "NI"
                    }
                };

                medicationAllergiesComponent.section.entry.Add(allergyProblemActEntry);
            }

            sb.component.Add(medicationAllergiesComponent);
        }

        private static void AddSocialHistoryComponent(StructuredBody sb, CCDADataModel objCCDAModel)
        {
            Component3 socialHistoryComponent = HelperMethods.IntializeComponent3(
            new List<string> { "2.16.840.1.113883.10.20.22.2.17" },
            "29762-2", "2.16.840.1.113883.6.1", "LOINC", "Social History", "SOCIAL HISTORY");

            StrucDocText text = socialHistoryComponent.section.text;

            socialHistoryComponent.section.entry = new List<Entry>();
            List<Dictionary<string, string>> lstSocials = objCCDAModel.lstSocials;

            if (lstSocials != null && lstSocials.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);

                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead();
                table.thead.tr = new List<StrucDocTr>();

                var headerTr = new StrucDocTr();
                table.thead.tr.Add(headerTr);
                headerTr.Items = new List<object>();
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Social History Element" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Description" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Effective Dates" } });

                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);

                tbody.tr = new List<StrucDocTr>();

                var bodyTr = new StrucDocTr();
                bodyTr.Items = new List<object>();
                bodyTr.Items.Add(new StrucDocTd { Items = new List<object> { new StrucDocContent { ID = "social" } }, Text = new List<string> { "Smoking Status" } });

                var description = lstSocials[0]["Description"];
                bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { description } });
                var effectiveDate = Convert.ToDateTime(lstSocials[0]["SocialHxDate"]);
                bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { effectiveDate.ToString("MMMM dd, yyyy") } });
                tbody.tr.Add(bodyTr);


                var smokingHistoryEntry = new Entry();
                socialHistoryComponent.section.entry.Add(smokingHistoryEntry);
                var smokingHistoryObservation = new Observation();
                smokingHistoryEntry.Item = smokingHistoryObservation;

                smokingHistoryObservation.classCode = "OBS";
                smokingHistoryObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                smokingHistoryObservation.templateId = new List<II>() 
                {
                    new II { root = "2.16.840.1.113883.10.20.22.4.38" }       
                };

                smokingHistoryObservation.id = new List<II> { new II { root = StaticCcdaData["UniqueIdentifierID"] } };

                smokingHistoryObservation.code = new CD
                {
                    code = "229819007",
                    codeSystem = "2.16.840.1.113883.6.96",
                    displayName = "Tobacco use and exposure",
                    originalText = new ED { reference = new TEL { value = "#social" } },
                };

                smokingHistoryObservation.statusCode = new CS
                {
                    code = "completed"
                };

                smokingHistoryObservation.value = new List<ANY>() 
                { 
                    new ST{ Text = new List<string> {"Not Known"} }
                };

                var smokingStatusEntry = new Entry();
                socialHistoryComponent.section.entry.Add(smokingStatusEntry);

                var smokingStatusObservation = new Observation();
                smokingStatusEntry.Item = smokingStatusObservation;

                smokingStatusObservation.classCode = "OBS";
                smokingStatusObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                smokingStatusObservation.templateId = new List<II>() 
                {
                    new II { root = "2.16.840.1.113883.10.22.4.78" }       
                };

                smokingStatusObservation.code = new CD
                {
                    code = "ASSERTION",
                    codeSystem = "2.16.840.1.113883.5.4"
                };

                smokingStatusObservation.statusCode = new CS
                {
                    code = "completed"
                };

                smokingStatusObservation.effectiveTime = new IVL_TS();
                smokingStatusObservation.effectiveTime.ItemsElementName = new ItemsChoiceType2[] 
                { 
                    ItemsChoiceType2.low,
                    ItemsChoiceType2.high
                };

                smokingStatusObservation.effectiveTime.Items = new QTY[] 
                { 
                    new IVXB_TS { nullFlavor = "UNK" },
                    new IVXB_TS { nullFlavor = "UNK" } 
                };

                smokingStatusObservation.value = new List<ANY>();

                var SNOMEDID = lstSocials[0]["SNOMEDID"];

                smokingStatusObservation.value.Add(new CD
                {
                    code = SNOMEDID,
                    codeSystem = "2.16.840.1.113883.6.96",
                    displayName = description
                });
            }
            else
            {
                var content = new StrucDocContent();
                text.Items.Add(content);
                content.Text = new List<string> { "Not included in document" };

                var smokingStatusEntry = new Entry();
                socialHistoryComponent.section.entry.Add(smokingStatusEntry);

                var smokingStatusObservation = new Observation();
                smokingStatusEntry.Item = smokingStatusObservation;

                smokingStatusObservation.classCode = "OBS";
                smokingStatusObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                smokingStatusObservation.templateId = new List<II>() 
                {
                    new II { root = "2.16.840.1.113883.10.22.4.78" }       
                };

                smokingStatusObservation.code = new CD
                {
                    code = "ASSERTION",
                    codeSystem = "2.16.840.1.113883.5.4"
                };

                smokingStatusObservation.statusCode = new CS
                {
                    code = "completed"
                };

                smokingStatusObservation.effectiveTime = new IVL_TS();
                smokingStatusObservation.effectiveTime.ItemsElementName = new ItemsChoiceType2[] 
                { 
                    ItemsChoiceType2.low
                };

                smokingStatusObservation.effectiveTime.Items = new QTY[] 
                { 
                    new IVXB_TS { nullFlavor = "NI" }
                };

                smokingStatusObservation.value = new List<ANY>()
                {
                    new CD
                    {
                        nullFlavor = "NI"
                    }
                };
            }


            sb.component.Add(socialHistoryComponent);
        }

        private static void AddMedicationComponent(StructuredBody sb, CCDADataModel objCCDAModel)
        {
            Component3 medicationComponent = HelperMethods.IntializeComponent3(
               new List<string> { "2.16.840.1.113883.10.20.22.2.1", "2.16.840.1.113883.10.20.22.2.1.1" },
               "10160-0", "2.16.840.1.113883.6.1", "LOINC", "History of medication use", "MEDICATIONS");

            StrucDocText text = medicationComponent.section.text;

            List<Dictionary<string, string>> lstMedication = objCCDAModel.lstMedication;


            if (lstMedication != null && lstMedication.Count > 0)
            {

                DataTable dtMedications = new DataTable(); //Medication datatable

                var table = new StrucDocTable();
                text.Items.Add(table);

                table.border = "1";
                table.width = "100%";

                table.thead = new StrucDocThead();
                table.thead.tr = new List<StrucDocTr>();

                var headerTr = new StrucDocTr();
                table.thead.tr.Add(headerTr);
                headerTr.Items = new List<object>();
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Medication" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Directions" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Start Date" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Status" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Indications" } });
                headerTr.Items.Add(new StrucDocTh { Text = new List<string> { "Fill Instructions" } });

                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);

                tbody.tr = new List<StrucDocTr>();

                int id = 0;
                foreach (Dictionary<string, string> medication in lstMedication)
                {
                    var bodyTr = new StrucDocTr();
                    bodyTr.Items = new List<object>();

                    var StartDate = medication["StartDate"];
                    if (StartDate != string.Empty)
                    {
                        StartDate = Convert.ToDateTime(StartDate).ToString("yyyyMMdd");
                    }

                    bodyTr.Items.Add(
                    new StrucDocTd
                    {
                        Items = new List<object> 
                        { 
                            new StrucDocContent 
                            { 
                                ID = String.Concat("Medication_", ++id), 
                                Text = new List<string> {medication["Medication"] } 
                            } 
                        }
                    });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["Directions"] } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { StartDate } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["Status"] } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["Indication"] } });
                    string substituation = medication["Substitution"];
                    if (substituation.ToLower() == "n")
                    {
                        bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { "Generic Substitution not Allowed" } });
                    }
                    else if (substituation.ToLower() == "y")
                    {
                        bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { "Generic Substitution Allowed" } });
                    }

                    tbody.tr.Add(bodyTr);
                }


                medicationComponent.section.entry = new List<Entry>();


                id = 0;
                for (var i = 0; i < lstMedication.Count; i++)
                {
                    var medicationEntry = new Entry();
                    medicationEntry.typeCode = x_ActRelationshipEntry.DRIV;

                    medicationEntry.Item = SetMedicationActivity(lstMedication[i], objCCDAModel.lstPracticeData[0]["ShortName"], String.Concat("#Medication_", ++id));
                    medicationComponent.section.entry.Add(medicationEntry);
                }

            }
            else
            {
                var noMedicationText = new StrucDocContent();
                text.Items.Add(noMedicationText);

                //Section not included
                if (lstMedication == null || lstMedication.Count == 0)
                {
                    noMedicationText.ID = "nomeds1";
                    noMedicationText.Text = new List<string> { "Not included in document" };
                }

                medicationComponent.section.entry = new List<Entry>();
                var medicationEntry = new Entry();
                medicationComponent.section.entry.Add(medicationEntry);

                medicationEntry.typeCode = x_ActRelationshipEntry.DRIV;
                var medicationActivity = new SubstanceAdministration();
                medicationEntry.Item = medicationActivity;

                medicationActivity.classCode = "SBADM";
                medicationActivity.moodCode = x_DocumentSubstanceMood.EVN;

                medicationActivity.nullFlavor = "NI";

                medicationActivity.templateId = new List<II>
                { 
                    new II { root = "2.16.840.1.113883.10.20.22.4.16" } 
                };

                medicationActivity.id = new List<II> 
                { 
                    new II { root = StaticCcdaData["UniqueIdentifierID"] } 
                };

                medicationActivity.statusCode = new CS { code = "completed" };
                medicationActivity.effectiveTime = new List<SXCM_TS>
                {
                    new IVL_TS 
                    { 
                        ItemsElementName = new ItemsChoiceType2[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[] { new IVXB_TS { nullFlavor = "NI" }, new IVXB_TS { nullFlavor = "NI" } }
                    }
                };

                var consumable = new Consumable();
                medicationActivity.consumable = consumable;

                var manufacturedProduct = new ManufacturedProduct();
                consumable.manufacturedProduct = manufacturedProduct;

                manufacturedProduct.classCode = RoleClassManufacturedProduct.MANU;
                manufacturedProduct.classCodeSpecified = true;
                manufacturedProduct.templateId = new List<II> { new II { root = "2.16.840.1.113883.10.20.22.4.23" } };

                var manufacturedMaterial = new Material();
                manufacturedProduct.Item = manufacturedMaterial;

                manufacturedMaterial.code = new CE
                {
                    nullFlavor = "NI"
                };
            }

            sb.component.Add(medicationComponent);

        }

        private static SubstanceAdministration SetMedicationActivity(Dictionary<string, string> dr, String Practice, String referenceValue = "")
        {
            var medicationActivity = new SubstanceAdministration();

            medicationActivity.classCode = "SBADM";
            medicationActivity.moodCode = x_DocumentSubstanceMood.EVN;
            medicationActivity.templateId = new List<II>
            { 
                new II { root = "2.16.840.1.113883.10.20.22.4.16" } 
            };

            medicationActivity.id = new List<II> 
            { 
                new II { root = StaticCcdaData["UniqueIdentifierID"] } 
            };

            medicationActivity.text = new ED
            {
                reference = new TEL { value = referenceValue },
                Text = new List<string> { dr["Medication"] }
            };

            if (dr["Status"].ToLower() == "active")
                medicationActivity.statusCode = new CS { code = "active" };
            else
                medicationActivity.statusCode = new CS { code = "suspended" };

            var StartDate = DateTime.MinValue;
            if (dr["StartDate"] != string.Empty)
                StartDate = Convert.ToDateTime(dr["StartDate"]);
            var EndDate = DateTime.MinValue;
            if (dr["EndDate"] != string.Empty)
                EndDate = Convert.ToDateTime(dr["EndDate"]);

            DateTime lowEffectiveTime = Convert.ToDateTime(StartDate);
            DateTime highEffectiveTime = Convert.ToDateTime(EndDate);

            medicationActivity.effectiveTime = new List<SXCM_TS>();

            var effectiveTime = new IVL_TS();
            effectiveTime.ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high, };
            effectiveTime.Items = new QTY[2];

            if (lowEffectiveTime != DateTime.MinValue)
                effectiveTime.Items[0] = new IVXB_TS { value = lowEffectiveTime.ToString("yyyyMMdd") };
            else
                effectiveTime.Items[0] = new IVXB_TS { nullFlavor = "UNK" };

            if (highEffectiveTime != DateTime.MinValue)
                effectiveTime.Items[1] = new IVXB_TS { value = highEffectiveTime.ToString("yyyyMMdd") };
            else
                effectiveTime.Items[1] = new IVXB_TS { nullFlavor = "UNK" };

            medicationActivity.effectiveTime.Add(effectiveTime);

            if (!String.IsNullOrWhiteSpace(dr["NoOfTimes"]) && !string.IsNullOrWhiteSpace(dr["FrequencyDescription"]))
            {
                string noOfTime = dr["NoOfTimes"];

                string frequencyDescription = dr["FrequencyDescription"];

                if (frequencyDescription.Contains("day"))
                {
                    frequencyDescription = "d";
                }
                else if (frequencyDescription.Contains("hour"))
                {
                    frequencyDescription = "h";
                }
                else if (frequencyDescription.Contains("week"))
                {
                    frequencyDescription = "w";
                }
                else if (frequencyDescription.Contains("month"))
                {
                    frequencyDescription = "m";
                }

                medicationActivity.effectiveTime.Add(new PIVL_TS()
                {
                    @operator = SetOperator.A,
                    period = new PQ
                    {
                        value = noOfTime,
                        unit = frequencyDescription
                    }
                });
            }

            string refill;
            if (string.IsNullOrWhiteSpace(dr["Refill"]) || dr["Refill"] == "0")
            {
                refill = "0";
            }
            else
            {
                refill = dr["Refill"];
            }


            if (!String.IsNullOrEmpty(refill))
            {
                medicationActivity.repeatNumber = new IVL_INT { value = refill };
            }

            if (!String.IsNullOrWhiteSpace(dr["RouteCode"])
                && !String.IsNullOrWhiteSpace(dr["RouteDescription"]))
            {
                medicationActivity.routeCode = new CE
                {
                    code = dr["RouteCode"],
                    codeSystem = "2.16.840.1.113883.3.26.1.1",
                    codeSystemName = "NCI Thesaurus",
                    displayName = dr["RouteDescription"]
                };
            }

            if (!String.IsNullOrWhiteSpace(dr["Dose"]))
            {
                medicationActivity.doseQuantity = new IVL_PQ();
                medicationActivity.doseQuantity.value = dr["Dose"].TrimEnd('0', '.');
            }


            Consumable consumable = new Consumable();
            medicationActivity.consumable = consumable;

            ManufacturedProduct medicationInfo = new ManufacturedProduct();
            consumable.manufacturedProduct = medicationInfo;

            medicationInfo.classCodeSpecified = true;
            medicationInfo.classCode = RoleClassManufacturedProduct.MANU;
            medicationInfo.templateId = new List<II>() { new II { root = "2.16.840.1.113883.10.20.22.4.23" } };

            Material manufacturedMaterial = new Material();
            medicationInfo.Item = manufacturedMaterial;

            manufacturedMaterial.code = new CE
            {
                code = dr["RxnormID"],
                codeSystem = "2.16.840.1.113883.6.88",
                displayName = dr["Medication"]
            };


            medicationActivity.performer = new List<Performer2>();
            var performer2 = new Performer2();

            performer2.assignedEntity = new AssignedEntity();
            performer2.assignedEntity.id = new List<II>();
            performer2.assignedEntity.id.Add(new II { nullFlavor = "UNK" });

            performer2.assignedEntity.addr = new List<AD>
            {
                new AD { nullFlavor = "UNK" }
            };

            performer2.assignedEntity.telecom = new List<TEL>() 
            { 
                new TEL { nullFlavor = "UNK" }
            };

            var organization = new Organization();
            organization.id = new List<II>();

            organization.id.Add(new II
            {
                root = StaticCcdaData["UniqueIdentifierID"]
            });

            organization.name = new List<ON>()
            {
                new ON
                {
                    Text = new List<string>{ Practice }
                }
            };

            organization.telecom = new List<TEL>() 
            { 
                new TEL { nullFlavor = "UNK" } 
            };

            organization.addr = new List<AD>
            {
                new AD
                {
                    nullFlavor = "UNK"
                }
            };

            performer2.assignedEntity.representedOrganization = organization;

            medicationActivity.performer.Add(performer2);

            return medicationActivity;
        }
    }

    public class HelperMethods
    {
        public static String SetDateTime(DateTime dt)
        {
            String formattedDateTime = String.Empty;
            if (dt != null && dt != DateTime.MinValue)
            {
                formattedDateTime = dt.ToString("yyyyMMddhhmmsszzzz").Replace(":", "");
            }
            return formattedDateTime;
        }

        public static TEL SetTelephone(string telephone, string use, string extension = "")
        {
            var tel = new TEL();

            if (!String.IsNullOrWhiteSpace(telephone) && telephone.Length == 10)
            {
                tel.use = new List<String> { use };

                String phoneNumber = "tel:";

                phoneNumber = String.Concat(phoneNumber, String.Format("({0}){1}-{2}", telephone.Substring(0, 3), telephone.Substring(3, 3), telephone.Substring(6, 4)));

                if (!String.IsNullOrWhiteSpace(extension))
                {
                    phoneNumber = String.Concat(phoneNumber, "-", extension);
                }

                tel.value = phoneNumber;
            }
            else
            {
                tel.nullFlavor = "NAV"; //Temporarily unavailable

            }
            return tel;
        }

        public static AD SetAddress(String use, List<string> streetAddressLines, String city, String state, String postalCode, String country)
        {

            var address = new AD();

            if (streetAddressLines.Count == 0 || String.IsNullOrWhiteSpace(city) || String.IsNullOrWhiteSpace(state) || String.IsNullOrWhiteSpace(postalCode) || (streetAddressLines.Count == 1 && string.IsNullOrWhiteSpace(streetAddressLines[0])))
                address.nullFlavor = "NI";
            else
            {
                address.use = new List<String> { use };
                address.Items = new List<ADXP>();
                foreach (String add in streetAddressLines)
                {
                    if (!String.IsNullOrWhiteSpace(add))
                    {
                        address.Items.Add(new adxpstreetAddressLine { Text = new List<String> { add } });
                    }
                }
                address.Items.Add(new adxpcity() { Text = new List<String>() { city } });
                address.Items.Add(new adxpstate() { Text = new List<String>() { state } });
                address.Items.Add(new adxppostalCode() { Text = new List<String>() { postalCode } });
                country = !String.IsNullOrEmpty(country) ? country : "US";
                address.Items.Add(new adxpcountry() { Text = new List<String>() { country } });
            }

            return address;
        }

        public static PN SetName(String use, String familyName, List<String> givenNames, String prefix = "", String suffix = "")
        {
            var Name = new PN();

            if (!String.IsNullOrWhiteSpace(use))
            {
                Name.use = new List<String> { use };
            }

            Name.Items = new List<ENXP>();
            if (!String.IsNullOrWhiteSpace(familyName))
            {
                Name.Items.Add(
                    new enfamily { Text = new List<String> { familyName } }
                    );
            }

            if (givenNames != null && givenNames.Count > 0)
            {
                foreach (String name in givenNames)
                {
                    if (!String.IsNullOrWhiteSpace(name))
                    {
                        Name.Items.Add(
                            new engiven { Text = new List<String> { name } });
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(prefix))
            {
                Name.Items.Add(
                    new enprefix { Text = new List<String> { prefix } }
                    );
            }

            if (!String.IsNullOrWhiteSpace(suffix))
            {
                Name.Items.Add(
                    new ensuffix { Text = new List<String> { suffix } }
                    );
            }

            return Name;
        }

        public static string GetGenderCode(string gender)
        {
            if (gender.ToLower() == "male")
                return "M";
            else if (gender.ToLower() == "female")
                return "F";
            else
                return "UN";
        }

        public static string GetMaritalStatusCode(string maritalStatus)
        {
            string returnVal = string.Empty;
            if (maritalStatus.ToLower() == "divorced")
                returnVal = "D";
            else if (maritalStatus.ToLower() == "married")
                returnVal = "M";
            else if (maritalStatus.ToLower() == "partner")
                returnVal = "P";
            else if (maritalStatus.ToLower() == "single")
                returnVal = "S";
            else if (maritalStatus.ToLower() == "unknown")
                returnVal = "S";
            else if (maritalStatus.ToLower() == "widowed")
                returnVal = "W";
            else if (maritalStatus.ToLower() == "legally separated")
                returnVal = "L";
            return returnVal;
        }

        public static Component3 IntializeComponent3(List<string> templatedId, string code, string codeSystem,
            string codeSystemName, string displayName, string title)
        {
            Component3 comp3 = new Component3();

            //Section
            comp3.section = new Section();

            //TemplateID
            if (templatedId.Count > 0)
            {
                comp3.section.templateId = new List<II>();
                foreach (string t in templatedId)
                {
                    if (!String.IsNullOrWhiteSpace(t))
                    {
                        comp3.section.templateId.Add(new II { root = t });
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }

            //Code
            if (!String.IsNullOrWhiteSpace(code) && !String.IsNullOrWhiteSpace(codeSystem) &&
                !String.IsNullOrWhiteSpace(codeSystemName) && !String.IsNullOrWhiteSpace(displayName))
            {
                comp3.section.code = new CE
                {
                    code = code,
                    codeSystem = codeSystem,
                    codeSystemName = codeSystemName,
                    displayName = displayName
                };
            }
            else
            {
                return null;
            }

            //Title
            if (!String.IsNullOrWhiteSpace(title))
            {
                comp3.section.title = new ST { Text = new List<string> { title } };
            }
            else
            {
                return null;
            }

            //Text
            comp3.section.text = new StrucDocText();
            comp3.section.text.Items = new List<object>();

            return comp3;
        }
    }
}