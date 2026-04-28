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
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Model.CCDA;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Summary
{
    public class StringWriterUtf8CaseReports : System.IO.StringWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
    public class CaseReportsCCDAGenerator
    {
        private static CaseReportsCCDAGenerator _instance = null;

        public static CaseReportsCCDAGenerator Instance()
        {
            if (_instance == null)
                _instance = new CaseReportsCCDAGenerator();
            return _instance;
        }

        public enum DocumentTemplateType : int
        {
            ClinicalSummary,
            ReferralSummary,
            DataPortability,
            ContinutyofCaredocument,
            ReferralNote
        }
        public static string currentHTML = "";
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

        public static string MakeCcda(CCDACaseReportDataModel objCCDAModel, Int64 noteId, Int64 providerId, Int64 patientId, string StreamType, DocumentTemplateType documentType)
        {
            DocumentTemplate = documentType;
            StaticCcdaData = GetXmlValues();

            ClinicalDocument ccda = new ClinicalDocument();

            MakeCcdaHeader(ccda, objCCDAModel);
            MakeCcdaBody(ccda, objCCDAModel);

            var html = MakeCaseReportHTML(ccda, objCCDAModel, noteId, providerId, patientId, StreamType);
            currentHTML = html;

            var output = GenerateXmlDocument(ccda);

            switch (StreamType.ToLower())
            {
                case "html":
                    output = GetHtmlFromXml(noteId, providerId, patientId, output);
                    break;
                case "pdf":
                    output = GetHtmlFromXml(noteId, providerId, patientId, output);
                    output = GetPDFFromHTML(output);
                    break;
             
            }

            return output;
        }

        public class CASEReportingModel
        {
            public string ComponentName { get; set; }
            public string ComponentHTML { get; set; }
        }
        public static string MakeCaseReportHTML(ClinicalDocument ccda, CCDACaseReportDataModel objCCDAModel, Int64 noteId, Int64 providerId, Int64 patientId, string StreamType)
        {
            List<Component3> cmpnnt = new List<Component3>();
            var cmpnntVal = ccda.component.Item.GetType().GetProperty("component").GetValue(ccda.component.Item, null);
            var components_List = (List<Clinical.Summary.Component3>)(ccda.component.Item.GetType().GetProperty("component").GetValue(ccda.component.Item, null));
            List<CASEReportingModel> list = new List<CASEReportingModel>();
            foreach (var item in components_List)
            {
                CASEReportingModel model = new CASEReportingModel();
                model.ComponentName = item.section.title.Text.FirstOrDefault();
                string xml_ = GenerateXmlDocument(item.section.text);
                if (model.ComponentName == "HISTORY OF PRESENT ILLNESS")
                {
                    xml_ = xml_.Replace("&lt;br /&gt;", "<br />");
                }
                //if (xml_.IndexOf("<table") >= 0)
                //{
                //    string ss = xml_.Remove(0, xml_.IndexOf("<table"));
                //    string dd = ss.Remove(ss.IndexOf("</table>") + 9);
                //    model.ComponentHTML = dd;
                //}
                    model.ComponentHTML = xml_; 
                list.Add(model);
            }
            //cmpnnt = cmpnntVal.ToString();
            //foreach (var item in cmpnnt)
            //{

            //}
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            return js.Serialize(list);
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

        public static string GetHtmlFromXml(Int64 noteId, Int64 providerId, Int64 patientId, string xmlContent, bool isDelete = true, string orignalXML = "", string Style = "ClinicalSummary")
        {
            var folderPath = System.Web.HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);
            var htmlfileName = string.Format("CCDA_{0}_{1}_{2}_{3}.html", noteId, providerId, patientId, MDVSession.Current.AppUserId);
            var xmlfileName = string.Format("CCDA_{0}_{1}_{2}_{3}.xml", noteId, providerId, patientId, MDVSession.Current.AppUserId);
            var orignalfileName = string.Format("Orignal_{0}_{1}_{2}_{3}.xml", noteId, providerId, patientId, MDVSession.Current.AppUserId);
            var htmlFilePath = string.Format(@"{0}\{1}", folderPath, htmlfileName);
            var xmlFilePath = string.Format(@"{0}\{1}", folderPath, xmlfileName);
            var orignalFilePath = string.Format(@"{0}\{1}", folderPath, orignalfileName);

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
            if (!string.IsNullOrEmpty(orignalXML))
                File.WriteAllText(orignalFilePath, orignalXML);

            // Create a resolver with default credentials.
            XmlUrlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;
            // transform Xml file to HTML
            XslCompiledTransform transform = new XslCompiledTransform();
            XsltSettings settings = new XsltSettings() { EnableDocumentFunction = true };
            // load up the stylesheet
            string stylesheetPath;
            if (Style == "ReferralSummary")
                stylesheetPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\CDA.xsl";
            else
                stylesheetPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\CCDA.xsl";

            transform.Load(stylesheetPath, settings, resolver);
            // perform the transformation
            if (string.IsNullOrEmpty(orignalXML))
                transform.Transform(xmlFilePath, htmlFilePath);
            else
            {
                transform.Transform(orignalFilePath, htmlFilePath);
                File.Delete(orignalfileName);
            }
            var content = File.ReadAllText(htmlFilePath);
            content = content.Replace("�", " ");
            return content;
        }

        //private static string GenerateXmlDocument(object document)
        //{
        //    string xml = string.Empty;
        //    XmlSerializer serializer = new XmlSerializer(document.GetType());

        //    using (StringWriterUtf8CaseReports strWriter = new StringWriterUtf8CaseReports())
        //    {
        //        serializer.Serialize(strWriter, document);
        //        xml = MDVUtility.ToStr(strWriter);
        //    }
        //    return xml;
        //}
        private static string GenerateXmlDocument(object document)
        {
            string xml = string.Empty;
            XmlSerializer serializer = new XmlSerializer(document.GetType());

            using (StringWriterUtf8CaseReports strWriter = new StringWriterUtf8CaseReports())
            {
                serializer.Serialize(strWriter, document);
                xml = MDVUtility.ToStr(strWriter);
            }
            xml = xml.Replace("&lt;br /&gt;", "<br />");
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
                        (string.Format("End of stream reached with {0} bytes left to read", remaining));
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

        public static void MakeCcdaHeader(ClinicalDocument ccda, CCDACaseReportDataModel objCcdaModel)
        {
            MakeStaticSection(ccda, objCcdaModel);
            MakeRecordTargetNode(ccda, objCcdaModel);
            MakeAuthorNode(ccda, objCcdaModel.lstProviderData);
            MakeDataEntererNode(ccda, objCcdaModel);
            MakeCustodianNode(ccda, objCcdaModel);
            MakeInformationRecipientNode(ccda, objCcdaModel);
            MakeLegalAuthenticatorNode(ccda, objCcdaModel);
            MakeCaregiverParticipantNode(ccda, objCcdaModel);
            MakeInformantNode(ccda, objCcdaModel);
            MakeDocumentationOfNode(ccda, objCcdaModel);
            MakeComponentOfNode(ccda, objCcdaModel);
        }

        private static void MakeComponentOfNode(ClinicalDocument ccda, CCDACaseReportDataModel objCcdaModel = null)
        {
            if (objCcdaModel == null) return;
            var objProvider = objCcdaModel.lstProviderData[0];
            var ae = new AssignedEntity
            {
                id = new List<II>()
                {
                    new II {root = StaticCcdaData["ProviderOrganizationID"]}
                },
                addr = new List<AD>
                {
                    HelperMethodsCaseReports.SetAddress("WP", new List<string>() {objProvider["WorkAddress"]}, objProvider["City"],
                        objProvider["State"], objProvider["ZIPCode"], "US")
                },
                telecom = new List<TEL>
                {
                    HelperMethodsCaseReports.SetTelephone(objProvider["PhoneNo"], "WP", objProvider["PhoneExt"])
                },
                assignedPerson = new Person
                {
                    name = new List<PN>
                    {
                        HelperMethodsCaseReports.SetName("L", objProvider["LastName"],
                            new List<string> {objProvider["FirstName"], objProvider["MI"]})
                    }
                }
            };
            //Provider 
            var componentOf = new Component1();
            var encounter = new EncompassingEncounter();
            var high = Convert.ToDateTime(objCcdaModel.VisitDate);
            encounter.responsibleParty = new ResponsibleParty { assignedEntity = ae };
            var loc = new Location { healthCareFacility = MakeHealthCareFacility(objCcdaModel) };
            encounter.location = loc;
            encounter.id = new List<II> { new II { root = StaticCcdaData["UniqueIdentifierID"] } };
            encounter.effectiveTime = new IVL_TS
            {
                ItemsElementName = new[]
                {
                    ItemsChoiceType2.low,
                    ItemsChoiceType2.high
                },
                Items = new QTY[]
                {
                    new IVXB_TS { value = HelperMethodsCaseReports.SetDateTime(high) },
                    new IVXB_TS { value = HelperMethodsCaseReports.SetDateTime(high) }
                }
            };
            encounter.responsibleParty = new ResponsibleParty
            {
                assignedEntity = new AssignedEntity
                {
                    id = new List<II> { new II { root = StaticCcdaData["UniqueIdentifierID"] } },
                    assignedPerson = new Person
                    {
                        name = new List<PN>
                        {
                            HelperMethodsCaseReports.SetName("L", objProvider["LastName"],
                                new List<string> {objProvider["FirstName"], objProvider["MI"]})
                        }
                    }
                }
            };
            encounter.responsibleParty.assignedEntity.addr = new List<AD>
            {
                HelperMethodsCaseReports.SetAddress("WP", new List<string>() { objProvider["WorkAddress"] }, objProvider["City"], objProvider["State"], objProvider["ZIPCode"], "US")
            };
            encounter.responsibleParty.assignedEntity.telecom = new List<TEL> { HelperMethodsCaseReports.SetTelephone(objProvider["PhoneNo"], "WP", objProvider["PhoneExt"]) };
            encounter.responsibleParty.assignedEntity.representedOrganization = new Organization
            {
                name = new List<ON> { new ON { Text = new List<string> { objCcdaModel.lstPracticeData[0]["ShortName"] } } }
            };
            componentOf.encompassingEncounter = encounter;
            ccda.componentOf = new Component1();
            ccda.componentOf = componentOf;
        }

        private static HealthCareFacility MakeHealthCareFacility(CCDACaseReportDataModel objCcdaModel = null)
        {
            var hcf = new HealthCareFacility();
            if (objCcdaModel == null) return hcf;
            var objPracticeData = objCcdaModel.lstPracticeData[0];
            hcf.id = new List<II>
            {
                new II { root = StaticCcdaData["ProviderOrganizationID"] }
            };
            hcf.location = new Place();
            var facilityName = objCcdaModel.lstPatientData[0]["FacilityName"];
            if (!string.IsNullOrWhiteSpace(facilityName))
            {
                hcf.location.name = new ON
                {
                    Text = new List<string> { facilityName }
                };
            }
            hcf.location.addr = HelperMethodsCaseReports.SetAddress("WP",
                new List<string> { objPracticeData["Address"] },
                objPracticeData["City"], objPracticeData["State"], objPracticeData["ZIPCode"], "US");
            hcf.serviceProviderOrganization = new Organization
            {
                id = new List<II>
                {
                    new II {root = StaticCcdaData["ProviderOrganizationID"]}
                },
                telecom = new List<TEL>
                {
                    HelperMethodsCaseReports.SetTelephone(objCcdaModel.lstPracticeData[0]["PhoneNo"], "WP",
                        objCcdaModel.lstPracticeData[0]["PhoneExt"])
                },
                addr = new List<AD>
                {
                    HelperMethodsCaseReports.SetAddress("WP",
                        new List<string> {objPracticeData["Address"]},
                        objPracticeData["City"], objPracticeData["State"], objPracticeData["ZIPCode"], "US")
                }
            };
            return hcf;
        }

        private static void MakeInformantNode(ClinicalDocument ccda, CCDACaseReportDataModel objCcdaModel = null)
        {
            if (objCcdaModel == null) return;
            var objProvider = objCcdaModel.lstProviderData[0];
            var ae = new AssignedEntity
            {
                id = new List<II>()
                {
                    new II {root = StaticCcdaData["ProviderOrganizationID"]}
                },
                addr = new List<AD>
                {
                    HelperMethodsCaseReports.SetAddress("WP", new List<string>() {objProvider["WorkAddress"]},
                        objProvider["City"], objProvider["State"], objProvider["ZIPCode"], "US")
                },
                telecom = new List<TEL>
                {
                    HelperMethodsCaseReports.SetTelephone(objProvider["PhoneNo"], "WP", objProvider["PhoneExt"])
                },
                assignedPerson = new Person
                {
                    name = new List<PN>
                    {
                        HelperMethodsCaseReports.SetName("L", objProvider["LastName"],
                            new List<string> {objProvider["FirstName"], objProvider["MI"]})
                    }
                }
            };
            //Provider
            var informant = new Informant12 { Item = ae };
            ccda.informant = new List<Informant12> { informant };
        }

        public static void MakeDocumentationOfNode(ClinicalDocument ccda, CCDACaseReportDataModel objCcdaModel = null)
        {
            if (objCcdaModel == null) return;
            var objPractice = objCcdaModel.lstPracticeData[0];

            var se = new ServiceEvent
            {
                classCode = "PCPR",
                effectiveTime = new IVL_TS
                {
                    ItemsElementName = new[]
                    {
                        ItemsChoiceType2.low,
                        ItemsChoiceType2.high
                    },
                    Items = new QTY[]
                    {
                        new IVXB_TS {value = Convert.ToDateTime(objCcdaModel.VisitDate).ToString("yyyyMMdd")},
                        new IVXB_TS {value = Convert.ToDateTime(objCcdaModel.VisitDate).ToString("yyyyMMdd")}
                    }
                }
            };
            se.performer = new List<Performer1>();
            if (objCcdaModel.lstCareTeamPCP != null && objCcdaModel.lstCareTeamPCP.Count > 0)
            {
                var objPCP = objCcdaModel.lstCareTeamPCP[0];
                #region PCP
                var performerPCP = new Performer1();
                var ae = new AssignedEntity
                {
                    id = new List<II>()
                {
                    new II {root = StaticCcdaData["ProviderOrganizationID"]}
                },
                    addr = new List<AD>
                {
                    HelperMethodsCaseReports.SetAddress("WP", new List<string> {objPractice["Address"]}, objPractice["City"],
                        objPractice["State"], objPractice["ZIPCode"], "US")
                },
                    telecom = new List<TEL>
                {
                    HelperMethodsCaseReports.SetTelephone(objPractice["PhoneNo"], "WP", objPractice["PhoneExt"])
                }
                };
                //Practice
                string lastName = objPCP["Name"], firstName = objPCP["PCPName"];

                if (!string.IsNullOrEmpty(lastName) && lastName.IndexOf(" ", StringComparison.Ordinal) > -1)
                {
                    var names = lastName.Split(' ');
                    lastName = names[0];
                    firstName = names[1];
                }
                ae.assignedPerson = new Person
                {
                    name = new List<PN>
                {
                    HelperMethodsCaseReports.SetName("L", objPCP["LastName"],
                            new List<string> { objPCP["FirstName"], objPCP["MI"]})
                }
                };
                performerPCP.assignedEntity = ae;
                performerPCP.typeCode = x_ServiceEventPerformer.PPRF;
                performerPCP.functionCode = new CE
                {
                    code = "PCP",
                    displayName = "Primary Care Physician",
                    codeSystemName = "ParticipationFunction",
                    codeSystem = "2.16.840.1.113883.5.88"
                };
                #endregion PCP
                se.performer.Add(performerPCP);
            }
            if (objCcdaModel.lstCareTeamProvider.Count > 0)
            {
                var objProvider = objCcdaModel.lstCareTeamProvider[0];
                #region Provider
                var performerPr = new Performer1();
                var aePr = new AssignedEntity
                {
                    id = new List<II>()
                {
                    new II {root = StaticCcdaData["ProviderOrganizationID"]}
                },
                    addr = new List<AD>
                {
                    HelperMethodsCaseReports.SetAddress("WP", new List<string> {objPractice["Address"]}, objPractice["City"],
                        objPractice["State"], objPractice["ZIPCode"], "US")
                },
                    telecom = new List<TEL>
                {
                    HelperMethodsCaseReports.SetTelephone(objPractice["PhoneNo"], "WP", objPractice["PhoneExt"])
                }
                };


                aePr.assignedPerson = new Person
                {
                    name = new List<PN>
                {
                    HelperMethodsCaseReports.SetName("L", objProvider["LastName"],
                            new List<string> { objProvider["FirstName"], objProvider["MI"]})
                }
                };
                performerPr.assignedEntity = aePr;
                performerPr.typeCode = x_ServiceEventPerformer.PPRF;
                performerPr.functionCode = new CE
                {
                    code = "PR",
                    displayName = "Provider",
                    codeSystemName = "ParticipationFunction",
                    codeSystem = "2.16.840.1.113883.5.88"
                };
                #endregion PCP
                se.performer.Add(performerPr);
            }


            var docOf = new DocumentationOf { serviceEvent = new ServiceEvent() };
            docOf.serviceEvent = se;
            ccda.documentationOf = new List<DocumentationOf> { docOf };
        }

        public static void MakeLegalAuthenticatorNode(ClinicalDocument ccda, CCDACaseReportDataModel objCcdaModel)
        {
            var objProvider = objCcdaModel.lstProviderData[0];
            var la = new LegalAuthenticator
            {
                time = new TS
                {
                    value = DateTime.Now.ToString("yyyyMMdd")
                },
                signatureCode = new CS
                {
                    code = "S"
                }
            };
            var ae = new AssignedEntity
            {
                id = new List<II>
                {
                    new II {root = StaticCcdaData["ProviderOrganizationID"]}
                },
                addr = new List<AD>
                {
                    HelperMethodsCaseReports.SetAddress("WP", new List<string>() {objProvider["WorkAddress"]}, objProvider["City"],
                        objProvider["State"], objProvider["ZIPCode"], "US")
                },
                telecom = new List<TEL>
                {
                    HelperMethodsCaseReports.SetTelephone(objProvider["PhoneNo"], "WP", objProvider["PhoneExt"])
                },
                assignedPerson = new Person
                {
                    name = new List<PN>
                    {
                        HelperMethodsCaseReports.SetName("L", objProvider["LastName"],
                            new List<string> {objProvider["FirstName"], objProvider["MI"]})
                    }
                }
            };
            //Provider
            la.assignedEntity = ae;
            ccda.legalAuthenticator = new LegalAuthenticator();
            ccda.legalAuthenticator = la;
        }
        public static void MakeCaregiverParticipantNode(ClinicalDocument ccda, CCDACaseReportDataModel objCcdaModel)
        {
            if (objCcdaModel.lstCaregivers.Count > 0)
                ccda.participant = new List<Participant1>();
            foreach (var dr in objCcdaModel.lstCaregivers)
            {
                var participant = new Participant1
                {
                    typeCode = "IND",
                };

                var associatedEntity = new AssociatedEntity()
                {
                    addr = new List<AD>
                    {
                        new AD {nullFlavor = "NI"}
                    },
                    associatedPerson = new Person
                    {
                        name = new List<PN>
                    {
                        HelperMethodsCaseReports.SetName("L", dr["LastName"], new List<string> {dr["FirstName"]})
                    }
                    },

                };
                associatedEntity.telecom = new List<TEL>
                {
                    HelperMethodsCaseReports.SetTelephone(dr["PhoneNo"],  "WP",dr["PhoneExt"])
                };

                associatedEntity.classCode = "CAREGIVER";
                participant.associatedEntity = associatedEntity;
                ccda.participant.Add(participant);
            }
        }

        public static void MakeInformationRecipientNode(ClinicalDocument ccda, CCDACaseReportDataModel objCcdaModel)
        {
            var refProviderData = objCcdaModel.lstPatientData[0];
            if (refProviderData["ReferringProviderName"] == string.Empty)
                return;
            var ir = new IntendedRecipient
            {
                id = new List<II>()
                {
                    new II {root = StaticCcdaData["ProviderOrganizationID"]}
                },
                informationRecipient = new Person
                {
                    name = new List<PN>
                    {
                        HelperMethodsCaseReports.SetName("L", refProviderData["ReferringLastName"],
                            new List<string> {refProviderData["ReferringFirstName"], refProviderData["ReferringMI"]})
                    }
                }
            };
            var pOrg = new Organization();
            string providerOrganizationName = objCcdaModel.lstPracticeData[0]["ShortName"];
            if (!string.IsNullOrWhiteSpace(providerOrganizationName))
            {
                pOrg.name = new List<ON>
                {
                    new ON { Text = new List<string>{ providerOrganizationName }}
                };
            }
            pOrg.telecom = new List<TEL>
            {
                HelperMethodsCaseReports.SetTelephone(objCcdaModel.lstPracticeData[0]["PhoneNo"],  "WP",objCcdaModel.lstPracticeData[0]["PhoneExt"])
            };
            pOrg.addr = new List<AD>
            {
                HelperMethodsCaseReports.SetAddress( "WP", new List<string> {objCcdaModel.lstPracticeData[0]["Address"]},
                    objCcdaModel.lstPracticeData[0]["City"], objCcdaModel.lstPracticeData[0]["State"],  objCcdaModel.lstPracticeData[0]["ZIPCode"], "US")
            };
            ir.receivedOrganization = pOrg;
            var infor = new InformationRecipient { intendedRecipient = ir };
            ccda.informationRecipient = new List<InformationRecipient> { infor };
        }

        public static void MakeCustodianNode(ClinicalDocument ccda, CCDACaseReportDataModel objCcdaModel)
        {
            var practiceData = objCcdaModel.lstPracticeData[0];
            var cousdOrg = new CustodianOrganization
            {
                id = new List<II>()
                {
                    new II {root = StaticCcdaData["ProviderOrganizationID"]}
                }
            };
            var providerOrganizationName = practiceData["ShortName"];
            if (!string.IsNullOrWhiteSpace(providerOrganizationName))
            {
                cousdOrg.name = new ON
                {
                    Text = new List<string> { providerOrganizationName }
                };
            }
            cousdOrg.telecom = HelperMethodsCaseReports.SetTelephone(practiceData["PhoneNo"], "WP", practiceData["PhoneExt"]);
            cousdOrg.addr = HelperMethodsCaseReports.SetAddress("WP", new List<string> { practiceData["Address"] }, practiceData["City"], practiceData["State"], practiceData["ZIPCode"], "US");

            var ac = new AssignedCustodian { representedCustodianOrganization = cousdOrg };
            var custodian = new Custodian { assignedCustodian = ac };
            ccda.custodian = custodian;
        }

        public static void MakeDataEntererNode(ClinicalDocument ccda, CCDACaseReportDataModel objCcdaModel)
        {
            var objProvider = objCcdaModel.lstProviderData[0];
            var ae = new AssignedEntity
            {
                id = new List<II>()
                {
                    new II {root = StaticCcdaData["ProviderOrganizationID"]}
                },
                addr = new List<AD>
                {
                    HelperMethodsCaseReports.SetAddress("WP", new List<string>() {objProvider["WorkAddress"]}, objProvider["City"],
                        objProvider["State"], objProvider["ZIPCode"], "US")
                },
                telecom = new List<TEL>
                {
                    HelperMethodsCaseReports.SetTelephone(objProvider["PhoneNo"], "WP", objProvider["PhoneExt"])
                },
                assignedPerson = new Person
                {
                    name = new List<PN>
                    {
                        HelperMethodsCaseReports.SetName("L", objProvider["LastName"],
                            new List<string>() {objProvider["FirstName"], objProvider["MI"]})
                    }
                }
            };
            ccda.dataEnterer = new DataEnterer { assignedEntity = ae };
        }

        public static void MakeAuthorNode(ClinicalDocument ccda, List<Dictionary<string, string>> lstProviderData)
        {
            if (lstProviderData == null || lstProviderData.Count <= 0) return;
            var providerData = lstProviderData[0];
            var author = new Author
            {
                time = new TS
                {
                    value = DateTime.Now.ToString("yyyyMMdd")
                },
                assignedAuthor = new AssignedAuthor
                {
                    id = new List<II>()
                    {
                        new II {root = StaticCcdaData["ProviderOrganizationID"]}
                    },
                    addr = new List<AD>
                    {
                        HelperMethodsCaseReports.SetAddress("WP", new List<string>() {providerData["WorkAddress"]},
                            providerData["City"], providerData["State"], providerData["ZIPCode"], "US")
                    },
                    telecom = new List<TEL>
                    {
                        HelperMethodsCaseReports.SetTelephone(providerData["PhoneNo"], "WP", providerData["PhoneExt"])
                    }
                }
            };
            var assignedPerson = new Person
            {
                name = new List<PN>
                {
                    HelperMethodsCaseReports.SetName("L", providerData["LastName"],
                        new List<string> {providerData["FirstName"], providerData["MI"]})
                }
            };
            author.assignedAuthor.Item = assignedPerson;
            ccda.author = new List<Author>
            {
                author
            };
        }

        public static void MakeStaticSection(ClinicalDocument ccda, CCDACaseReportDataModel objCCDAModel)
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
                new II { root = StaticCcdaData["TemplateID"],extension="2015-08-01" },
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
            switch (DocumentTemplate)
            {
                case DocumentTemplateType.DataPortability:
                    ccda.templateId.Add(new II { root = StaticCcdaData["CS"], extension = "2015-08-01" });
                    ccda.templateId.Add(new II { root = StaticCcdaData["CS"] });
                    ccda.code.code = "34133-9";
                    ccda.code.displayName = "Transition of Care/Referral Summary";
                    ccda.title = new ST { Text = new List<string> { "Transition of Care/Referral Summary" } };
                    break;
                case DocumentTemplateType.ClinicalSummary:
                    ccda.templateId.Add(new II { root = StaticCcdaData["CS"], extension = "2015-08-01" });
                    ccda.templateId.Add(new II { root = StaticCcdaData["CS"] });
                    ccda.code.code = "34133-9";
                    ccda.code.displayName = "Summarization Of Episode Note";
                    ccda.title = new ST { Text = new List<string> { "Clinical Summary" } };
                    break;
                case DocumentTemplateType.ReferralSummary:
                    ccda.templateId.Add(new II { root = StaticCcdaData["CS"], extension = "2015-08-01" });
                    ccda.templateId.Add(new II { root = StaticCcdaData["CS"] });
                    ccda.code.code = "34133-9";
                    ccda.code.displayName = "Summarization Of Episode Note";
                    ccda.title = new ST { Text = new List<string> { "Transitions of Care" } };
                    break;
                case DocumentTemplateType.ReferralNote:
                    ccda.templateId.Add(new II { root = StaticCcdaData["CS"], extension = "2015-08-01" });
                    ccda.templateId.Add(new II { root = StaticCcdaData["CS"] });
                    ccda.code.code = "34133-9";
                    ccda.code.displayName = "Referral Note";
                    ccda.title = new ST { Text = new List<string> { "Referral Note" } };
                    break;
                case DocumentTemplateType.ContinutyofCaredocument:
                    ccda.templateId.Add(new II { root = StaticCcdaData["CS"], extension = "2015-08-01" });
                    ccda.templateId.Add(new II { root = StaticCcdaData["CS"] });
                    ccda.code.code = "34133-9";
                    ccda.code.displayName = "Continuty of Care Document";
                    ccda.title = new ST { Text = new List<string> { "Continuty of Care Document" } };
                    break;
            }
            ccda.effectiveTime = new TS
            {
                value = DateTime.Now.ToString("yyyyMMdd")
            };
            if (objCCDAModel.IsConfidential == "true")
            {
                ccda.confidentialityCode = new CE
                {
                    code = "R",
                    codeSystem = StaticCcdaData["ClinicalDocumentConfidentialityCodeCodeSystem"]

                };
            }
            else
            {
                ccda.confidentialityCode = new CE
                {
                    code = "N",
                    codeSystem = StaticCcdaData["ClinicalDocumentConfidentialityCodeCodeSystem"]

                };

            }
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
                            ItemsElementName = new[]
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

        public static void MakeRecordTargetNode(ClinicalDocument ccda, CCDACaseReportDataModel objCcdaModel)
        {
            RecordTarget rt = new RecordTarget();
            MakePatientRoleNode(rt, objCcdaModel);
            ccda.recordTarget = new List<RecordTarget>
            {
                rt
            };
        }

        public static void MakePatientRoleNode(RecordTarget rt, CCDACaseReportDataModel objCcdaModel)
        {
            PatientRole pr = new PatientRole();
            List<Dictionary<string, string>> lstPatientData = objCcdaModel.lstPatientData;
            if (lstPatientData != null && lstPatientData.Count > 0)
            {
                Dictionary<string, string> patientData = lstPatientData[0];
                pr.id = new List<II>
                {
                    new II { root =  StaticCcdaData["UniqueIdentifierID"] }
                };

                pr.addr = new List<AD>
                {
                    HelperMethodsCaseReports.SetAddress("H", new List<string>() { patientData["PatientStreetAddress"], }, patientData["PatientCity"], patientData["PatientState"], patientData["PatientZIPCode"], "US")
                };

                pr.telecom = new List<TEL>
                {
                    HelperMethodsCaseReports.SetTelephone(patientData["PatientHomePhoneNo"], "HP"),
                    HelperMethodsCaseReports.SetTelephone(patientData["PatientWorkPhoneNo"], "WP",
                        patientData["PatientWorkPhoneExt"])
                };

                MakePatientNode(pr, patientData, objCcdaModel.lstRaceCodes, objCcdaModel.lstEthnicityCodes);
                var pOrg = new Organization
                {
                    id = new List<II>
                    {
                        new II {root = StaticCcdaData["ProviderOrganizationID"]}
                    }
                };

                var practiceData = objCcdaModel.lstPracticeData[0];
                var providerOrganization = practiceData["ShortName"];
                if (string.IsNullOrWhiteSpace(providerOrganization))
                {
                }
                else
                {
                    pOrg.name = new List<ON>
                    {
                        new ON {Text = new List<string> {providerOrganization}}
                    };
                }
                pOrg.telecom = new List<TEL>
                {
                    HelperMethodsCaseReports.SetTelephone(practiceData["PhoneNo"], "WP")
                };
                pOrg.addr = new List<AD>
                {
                    HelperMethodsCaseReports.SetAddress("WP", new List<string>() { practiceData["Address"] }, practiceData["City"], practiceData["State"], practiceData["ZIPCode"], "US")
                };
                pr.providerOrganization = pOrg;
            }
            rt.patientRole = pr;
        }

        public static void MakePatientNode(PatientRole pr, Dictionary<string, string> patientData, List<Dictionary<string, string>> lstRaceCodes, List<Dictionary<string, string>> lstEthnicityCodes)
        {
            var p = new Patient
            {
                name = new List<PN>
                {
                    HelperMethodsCaseReports.SetName("L", patientData["LastName"],
                        new List<string> {patientData["FirstName"], patientData["MI"]}),
                    !string.IsNullOrWhiteSpace(patientData["PreviousName"])?  HelperMethodsCaseReports.SetName("L", patientData["LastName"],
                        new List<string> {patientData["PreviousName"], patientData["MI"]}):null
                },

                administrativeGenderCode = new CE
                {
                    code = HelperMethodsCaseReports.GetGenderCode(patientData["PatientGender"]),
                    codeSystem = StaticCcdaData["GenderCodeCodeSystem"],
                    codeSystemName = StaticCcdaData["GenderCodeCodeSystemName"],
                    displayName = patientData["PatientGender"]
                }
            };
            var dob = MDVUtility.ToDateTime(patientData["PatientDOB"]);
            p.birthTime = new TS
            {
                value = dob.ToString("yyyyMMdd")
            };
            p.maritalStatusCode = new CE();
            var maritalStatus = HelperMethodsCaseReports.GetMaritalStatusCode(patientData["MaritialStatus"]);
            if (string.IsNullOrEmpty(maritalStatus))
            {
                p.maritalStatusCode.nullFlavor = "NAV";
            }
            else
            {
                p.maritalStatusCode.code = HelperMethodsCaseReports.GetMaritalStatusCode(patientData["MaritialStatus"]);
                p.maritalStatusCode.codeSystem = StaticCcdaData["MaritalStatusCodeSystem"];
                p.maritalStatusCode.codeSystemName = StaticCcdaData["MaritalStatusCodeSystemName"];
                p.maritalStatusCode.displayName = patientData["MaritialStatus"];
            }
            if (lstRaceCodes != null && lstRaceCodes.Count > 0)
            {
                p.raceCode1 = new List<CE>();
                var firstRace = lstRaceCodes[0];
                p.raceCode = new CE
                {
                    codeSystem = StaticCcdaData["RaceCode"],
                    codeSystemName = "OMB Standards for Race and Ethnicity",
                    code = firstRace["RaceCode"],
                    displayName = firstRace["RaceName"],
                };
                lstRaceCodes = lstRaceCodes.Where(a => a.Keys != firstRace.Keys).ToList();
                foreach (Dictionary<string, string> itemr in lstRaceCodes)
                {
                    if (!string.IsNullOrWhiteSpace(itemr["RaceCode"]))
                    {

                        p.raceCode1.Add(
                            new CE
                            {
                                code = itemr["RaceCode"],
                                codeSystem = StaticCcdaData["RaceCode"],
                                displayName = itemr["RaceName"],

                            });
                    }
                }
            }
            if (lstEthnicityCodes != null && lstEthnicityCodes.Count > 0)
            {

                var firstItem = lstEthnicityCodes[0];
                p.ethnicGroupCode = new CE
                {
                    codeSystem = StaticCcdaData["RaceCode"],
                    codeSystemName = "OMB Standards for Race and Ethnicity",
                    code = firstItem["EthnicityCode"],
                    displayName = firstItem["EthnicityName"],
                };
                lstEthnicityCodes = lstEthnicityCodes.Where(a => a.Keys != firstItem.Keys).ToList();
                p.ethnicGroupCode1 = new List<DE>();
                foreach (Dictionary<string, string> itemr in lstEthnicityCodes)
                {
                    p.ethnicGroupCode1.Add(
                        new DE
                        {
                            codeSystem = StaticCcdaData["RaceCode"],
                            codeSystemName = "OMB Standards for Race and Ethnicity",
                            code = itemr["EthnicityCode"],
                            displayName = itemr["EthnicityName"]
                        });
                }
            }

            string languageCode = patientData["PatientLanguageCode"];
            if (!string.IsNullOrWhiteSpace(languageCode))
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

        private static void MakeCcdaBody(ClinicalDocument ccda, CCDACaseReportDataModel objCcdaModel)
        {
            StructuredBody sb = new StructuredBody { component = new List<Component3>() };
            if (objCcdaModel != null && objCcdaModel.Components != null)
            {
                // MK
                if (!string.IsNullOrWhiteSpace(objCcdaModel.IsConfidential) && objCcdaModel.IsConfidential.ToLower() == "true")
                    AddClinicalPrivacy(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddSocialHistoryComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddProblemListComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddAllergiesComponent(sb, objCcdaModel);

                //if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                //    AddMedicationComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary)
                    AddMedicationAdministeredComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddImmunizationComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary)
                    AddChiefComplaintComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary)
                    SetEncounters(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddVitalSignsComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddProcedureComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddLabOrderTestComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddResultsSection(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddHPIComponent(sb, objCcdaModel);

                //if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                //    AddPlanOfCareComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddImplantedDeviceComponent(sb, objCcdaModel);


                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddReasonForReferralComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddFunctionalStatusComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddClinicalInstructionsDecisionAids(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddMentalStatusComponent(sb, objCcdaModel);

                //if (DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                //    AddFamilyHistoryComponent(sb, objCcdaModel);


                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability)
                    AddCancerComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary)
                    AddGoalsComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ClinicalSummary || DocumentTemplate == DocumentTemplateType.DataPortability || DocumentTemplate == DocumentTemplateType.ReferralSummary || DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddHealthConcernsSectionComponent(sb, objCcdaModel);

                if (DocumentTemplate == DocumentTemplateType.ReferralNote || DocumentTemplate == DocumentTemplateType.ContinutyofCaredocument)
                    AddPayerSectionComponent(sb, objCcdaModel);
            }
            ccda.component = new Component2 { Item = sb };
        }

        private static void AddFunctionalStatusComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var functionalStatusComponent = HelperMethodsCaseReports.IntializeComponent3(
                new List<string> { "2.16.840.1.113883.10.20.22.2.14", "2.16.840.1.113883.10.20.22.2.14" },
                "47420-5", "2.16.840.1.113883.6.1", "LOINC", "Functional Status", "FUNCTIONAL STATUS");
            functionalStatusComponent.section.templateId[0].extension = "2014-06-09";
            var text = functionalStatusComponent.section.text;
            var lstCognitiveFunctionalStatus = objCcdaModel.lstCognitiveFunctionalStatus;
            if (lstCognitiveFunctionalStatus != null && lstCognitiveFunctionalStatus.Count > 0)
            {
                StrucDocTable table = new StrucDocTable
                {
                    width = "100%",
                    border = "1",
                    Items = new List<object>()
                };

                text.Items.Add(table);

                table.thead = new StrucDocThead
                {
                    tr = new List<StrucDocTr>
                    {
                        new StrucDocTr
                        {
                            Items = new List<object>
                            {
                                new StrucDocTh{ Text = new List<string>{ "Name" } },
                                new StrucDocTh{ Text = new List<string>{ "" } },
                                new StrucDocTh{ Text = new List<string>{ "Effective Date" } },
                            }
                        }
                    }
                };
                table.tbody = new List<StrucDocTbody>();
                var tBody = new StrucDocTbody();
                table.tbody.Add(tBody);
                tBody.tr = new List<StrucDocTr>();

                var dtFuncStatus = new DataTable();
                var i = 0;
                foreach (var dr in lstCognitiveFunctionalStatus)
                {
                    i++;
                    var tr = new StrucDocTr { Items = new List<object>() };
                    var name = MDVUtility.ToStr(dr["Name"]);
                    var snomedid = MDVUtility.ToStr(dr["SNOMEDID"]);

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        name = name + ", [SNOMED-CT: " + snomedid + "]";
                        tr.Items.Add(new StrucDocTd { ID = "functionalstatus_" + i, Text = new List<string> { name } });
                    }

                    if (!string.IsNullOrWhiteSpace(snomedid))
                    {
                        snomedid = "[SNOMED-CT: " + snomedid + "]";
                        tr.Items.Add(new StrucDocTd { Text = new List<string> { snomedid } });
                    }
                    var effectiveDate = objCcdaModel.VisitDate;
                    if (!string.IsNullOrWhiteSpace(effectiveDate))
                    {
                        effectiveDate = Convert.ToDateTime(effectiveDate).ToString("MM/dd/yyyy");
                    }
                    tr.Items.Add(new StrucDocTd { Text = new List<string> { effectiveDate } });

                    tBody.tr.Add(tr);
                }

                functionalStatusComponent.section.entry = new List<Entry>();

                i = 0;
                foreach (DataRow dr in dtFuncStatus.Rows)
                {
                    i++;
                    var type = MDVUtility.ToStr(dr["Type"]);
                    var effectiveDate = MDVUtility.ToStr(dr["EffectiveDate"]);
                    var ent = new Entry();
                    functionalStatusComponent.section.entry.Add(ent);
                    var obs = new Observation();
                    ent.Item = obs;
                    obs.classCode = "OBS";
                    obs.moodCode = x_ActMoodDocumentObservation.EVN;

                    switch (type)
                    {
                        case "Cognitive":
                            obs.templateId = new List<II> { new II { root = "2.16.840.1.113883.10.20.22.4.73" } };
                            obs.id = new List<II> { new II { root = Guid.NewGuid().ToString() } };
                            obs.code = new CD
                            {
                                code = "373930000",
                                codeSystem = "2.16.840.1.113883.6.96",
                                displayName = "Cognitive function finding"
                            };
                            break;
                        case "functional":
                            obs.templateId = new List<II> { new II { root = "2.16.840.1.113883.10.20.22.4.68" } };
                            obs.id = new List<II> { new II { root = Guid.NewGuid().ToString() } };
                            obs.code = new CD
                            {
                                code = "248536006",
                                codeSystem = "2.16.840.1.113883.6.96",
                                displayName = "finding of functional performance and activity"
                            };
                            break;
                    }
                    obs.text = new ED
                    {
                        reference = new TEL
                        {
                            value = "#functional_status_" + i
                        }
                    };
                    obs.statusCode = new CS { code = "completed" };
                    if (!string.IsNullOrWhiteSpace(effectiveDate))
                    {
                        obs.effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[] { ItemsChoiceType2.low },
                            Items =
                                new QTY[] { new IVXB_TS { value = Convert.ToDateTime(effectiveDate).ToString("yyyyMMdd") } }
                        };
                    }
                    else
                    {
                        obs.effectiveTime = new IVL_TS { nullFlavor = "UNK" };
                    }
                    obs.value = new List<ANY>
                   {
                       new CD
                       {
                           code = "66557003",
                           codeSystem = "2.16.840.1.113883.6.96",
                           displayName = "No impairment"
                       }
                   };
                }
            }
            else
                text.Text = new List<string> { "Not known" };
            sb.component.Add(functionalStatusComponent);
        }

        private static void AddReasonForReferralComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var reasonReferralComponent = HelperMethodsCaseReports.IntializeComponent3(
            new List<string> { "1.3.6.1.4.1.19376.1.5.3.1.3.1", "1.3.6.1.4.1.19376.1.5.3.1.3.1" },
            "42349-1", "2.16.840.1.113883.6.1", "LOINC", "Reason for Referral", "REASON FOR REFERRAL");
            var text = reasonReferralComponent.section.text;
            reasonReferralComponent.section.templateId[0].extension = "2014-06-09";
            if (objCcdaModel.lstReasonReferral != null && objCcdaModel.lstReasonReferral.Count > 0)
            {
                var table = new StrucDocTable
                {
                    width = "100%",
                    border = "1",
                    Items = new List<object>()
                };
                text.Items.Add(table);
                table.thead = new StrucDocThead
                {
                    tr = new List<StrucDocTr>
                    {
                        new StrucDocTr
                        {
                            Items = new List<object>
                            {
                                new StrucDocTh{ Text = new List<string>{ "Reason for Referral" } }
                            }
                        }
                    }
                };
                table.tbody = new List<StrucDocTbody>();
                var tBody = new StrucDocTbody();
                table.tbody.Add(tBody);
                tBody.tr = new List<StrucDocTr>();
                var i = 0;
                foreach (var dr in objCcdaModel.lstReasonReferral)
                {
                    i++;
                    var tr = new StrucDocTr { Items = new List<object>() };
                    var reason = MDVUtility.ToStr(dr["Reason"]);
                    if (!string.IsNullOrWhiteSpace(reason))
                        tr.Items.Add(new StrucDocTd { ID = "ReferalReason_" + i, Text = new List<string> { reason } });
                    tBody.tr.Add(tr);
                }
            }
            else
                text.Text = new List<string> { "Not known" };
            sb.component.Add(reasonReferralComponent);
        }
        private static void AddHPIComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var reasonReferralComponent = HelperMethodsCaseReports.IntializeComponent3(
            new List<string> { "1.3.6.1.4.1.19376.1.5.3.1.3.4" },
            "10164-2", "2.16.840.1.113883.6.1", "LOINC", "HISTORY OF PRESENT ILLNESS", "HISTORY OF PRESENT ILLNESS");
            var text = reasonReferralComponent.section.text;
            text.mediaType = "text/x-hl7-text+xml";
            reasonReferralComponent.section.templateId[0].extension = "2014-06-09";
            if (objCcdaModel.lstComplaints != null && objCcdaModel.lstComplaints.Count > 0)
            {

                var HPIs = "";
                foreach (var item in objCcdaModel.lstComplaints)
                {
                    if (item["IsReported"].ToLower() == "true" || item["IsReported"].ToLower() == "1")
                    {
                        HPIs += item["ComplaintDescription"] + " REPORTED starting on " + Convert.ToDateTime(item["ReportDate"]).ToString("MM/dd/yyyy") + " <br /> ";
                    }
                    else
                    {
                        HPIs += item["ComplaintDescription"] + " not reported <br /> ";
                    }
                }
                var para = new StrucDocParagraph
                {
                    Items = new List<object> { HPIs }
                };
                text.Items.Add(para);
                // para.Text = "sadas";

            }
            else
                text.Text = new List<string> { "Not known" };
            sb.component.Add(reasonReferralComponent);
        }

        //private static void AddResultsSection(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        //{
        //    var resultsComponent = HelperMethodsCaseReports.IntializeComponent3(
        //       new List<string> { "2.16.840.1.113883.10.20.22.2.3.1", "2.16.840.1.113883.10.20.22.2.3.1" },
        //       "30954-2", "2.16.840.1.113883.6.1", "LOINC", "RESULTS", "RESULTS");
        //    resultsComponent.section.templateId[0].extension = "2015-08-01";
        //    var text = resultsComponent.section.text;
        //    var lstResults = objCcdaModel.lstResults;
        //    if (lstResults != null && lstResults.Count > 0)
        //    {
        //        foreach (var drOrderResult in lstResults)
        //        {
        //            var table = new StrucDocTable
        //        {
        //            width = "100%",
        //            border = "1"
        //        };
        //        text.Items.Add(table);
        //        table.thead = new StrucDocThead
        //        {
        //            tr = new List<StrucDocTr>
        //            {
        //                new StrucDocTr
        //                {
        //                    Items = new List<object>
        //                    {
        //                        new StrucDocTh
        //                        {
        //                            Text = new List<string> {"LABORATORY INFORMATION"}
        //                        },
        //                        new StrucDocTh
        //                        {
        //                            Text = new List<string> {"ACTUAL RESULT"}
        //                        },
        //                        new StrucDocTh
        //                        {
        //                            Text = new List<string> {"STATUS"}
        //                        },
        //                        new StrucDocTh
        //                        {
        //                            Text = new List<string> {"DATE"}
        //                        }
        //                    }
        //                }
        //            }
        //        };

        //        table.tbody = new List<StrucDocTbody>();
        //        var tbody = new StrucDocTbody();
        //        table.tbody.Add(tbody);
        //        tbody.tr = new List<StrucDocTr>();
        //        resultsComponent.section.entry = new List<Entry>();

        //            if (string.IsNullOrWhiteSpace(drOrderResult["LoincCode"])) continue;
        //            var labBodyTr = new StrucDocTr
        //            {
        //                Items = new List<object>
        //                {
        //                    new StrucDocTd
        //                    {
        //                        Items = new List<object>
        //                        {
        //                            new StrucDocContent
        //                            {
        //                                Text = new List<string> {MDVUtility.ToStr(drOrderResult["LabTest"])}
        //                            }
        //                        }
        //                    },
        //                    new StrucDocTd
        //                    {
        //                        Items = new List<object>
        //                        {
        //                            new StrucDocContent
        //                            {
        //                                Text = new List<string> {MDVUtility.ToStr(drOrderResult["ActualResult"])}
        //                            }
        //                        }
        //                    },
        //                    new StrucDocTd
        //                    {
        //                        Items = new List<object>
        //                        {
        //                            new StrucDocContent
        //                            {
        //                                Text = new List<string> {MDVUtility.ToStr(drOrderResult["Status"])}
        //                            }
        //                        }
        //                    }
        //                }
        //            };
        //            var resultDate = drOrderResult["ResultDate"];
        //            if (!string.IsNullOrWhiteSpace(resultDate))
        //            {
        //                resultDate = Convert.ToDateTime(resultDate).ToString("MM/dd/yyyy");
        //            }
        //            labBodyTr.Items.Add(new StrucDocTd
        //            {
        //                Items = new List<object>
        //                {
        //                    new StrucDocContent
        //                    {
        //                        Text = new List<string> {resultDate}
        //                    }
        //                }
        //            });
        //            tbody.tr.Add(labBodyTr);
        //            var resultOrganizerEntry = new Entry();
        //            resultsComponent.section.entry.Add(resultOrganizerEntry);
        //            var resultOrganizer = new Organizer();
        //            resultOrganizerEntry.Item = resultOrganizer;
        //            resultOrganizer.classCode = x_ActClassDocumentEntryOrganizer.BATTERY;
        //            resultOrganizer.moodCode = "EVN";
        //            resultOrganizer.templateId = new List<II>
        //            {
        //                new II {root = "2.16.840.1.113883.10.20.22.4.1"}
        //            };
        //            resultOrganizer.id = new List<II>
        //            {
        //                new II {root = StaticCcdaData["UniqueIdentifierID"]}
        //            };
        //            if (!string.IsNullOrWhiteSpace(MDVUtility.ToStr(drOrderResult["LoincCode"])))
        //            {
        //                resultOrganizer.code = new CD
        //                {
        //                    code = MDVUtility.ToStr(drOrderResult["LoincCode"]),
        //                    displayName = MDVUtility.ToStr(drOrderResult["LabTestName"]),
        //                    codeSystem = "2.16.840.1.113883.6.1",
        //                    codeSystemName = "LOINC"
        //                };
        //            }
        //            resultOrganizer.statusCode = new CS
        //            {
        //                code = "completed"
        //            };
        //            resultOrganizer.component = new List<Component4>();
        //            var resultObservationComponent = new Component4();
        //            resultOrganizer.component.Add(resultObservationComponent);
        //            var resultObservation = new Observation();
        //            resultObservationComponent.Item = resultObservation;
        //            resultObservation.classCode = "OBS";
        //            resultObservation.moodCode = x_ActMoodDocumentObservation.EVN;
        //            resultObservation.templateId = new List<II>
        //            {
        //                new II {root = "2.16.840.1.113883.10.20.22.4.2"}
        //            };
        //            resultObservation.id = new List<II>
        //            {
        //                new II {root = StaticCcdaData["UniqueIdentifierID"]}
        //            };
        //            resultObservation.code = new CE
        //            {
        //                code = Convert.ToString(drOrderResult["LoincCode"]),
        //                displayName = Convert.ToString(drOrderResult["LabTestName"]),
        //                codeSystem = "2.16.840.1.113883.6.1",
        //                codeSystemName = "LOINC"
        //            };
        //            resultObservation.statusCode = new CS
        //            {
        //                code = "completed"
        //            };
        //            resultObservation.effectiveTime = new IVL_TS
        //            {
        //                value = Convert.ToDateTime(drOrderResult["ResultDate"]).ToString("yyyyMMddhhmm")
        //            };
        //            var resultValue = Convert.ToString(drOrderResult["ResultValue"]);
        //            if (string.IsNullOrWhiteSpace(resultValue))
        //            {
        //                resultObservation.value = new List<ANY>
        //                {
        //                    new ST
        //                    {
        //                        Text = new List<string> {"Pending"}
        //                    }
        //                };
        //            }
        //            else
        //            {
        //                resultObservation.value = new List<ANY>
        //                {
        //                    new PQ
        //                    {
        //                        value = Convert.ToString(drOrderResult["ResultValue"]),
        //                        unit =
        //                            string.IsNullOrEmpty(Convert.ToString(drOrderResult["Unit"]))
        //                                ? "UNK"
        //                                : Convert.ToString(drOrderResult["Unit"])
        //                    }
        //                };
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (lstResults == null || lstResults.Count == 0)
        //        {
        //            text.Items.Add(new StrucDocContent
        //            {
        //                ID = "noResults",
        //                Text = new List<string> { "Not included in document" }
        //            });
        //        }
        //        resultsComponent.section.entry = new List<Entry>();
        //        var resultOrganizerEntry = new Entry();
        //        resultsComponent.section.entry.Add(resultOrganizerEntry);
        //        var resultOrganizer = new Organizer();
        //        resultOrganizerEntry.Item = resultOrganizer;
        //        resultOrganizer.classCode = x_ActClassDocumentEntryOrganizer.BATTERY;
        //        resultOrganizer.moodCode = "EVN";
        //        resultOrganizer.templateId = new List<II>
        //        {
        //            new II {root = "2.16.840.1.113883.10.20.22.4.1"}
        //        };
        //        resultOrganizer.id = new List<II>
        //        {
        //            new II {nullFlavor = "UNK"}
        //        };
        //        resultOrganizer.code = new CD { nullFlavor = "UNK" };
        //        resultOrganizer.statusCode = new CS { nullFlavor = "UNK" };
        //        resultOrganizer.component = new List<Component4>();
        //        var resultObservationComponent = new Component4();
        //        resultOrganizer.component.Add(resultObservationComponent);
        //        var resultObservation = new Observation();
        //        resultObservationComponent.Item = resultObservation;
        //        resultObservation.classCode = "OBS";
        //        resultObservation.moodCode = x_ActMoodDocumentObservation.EVN;
        //        resultObservation.templateId = new List<II>
        //        {
        //            new II {root = "2.16.840.1.113883.10.20.22.4.2"}
        //        };
        //        resultObservation.id = new List<II>
        //        {
        //            new II {root = "NI"}
        //        };
        //        resultObservation.code = new CD
        //        {
        //            code = "completed"
        //        };
        //        resultObservation.statusCode = new CS
        //        {
        //            code = "completed"
        //        };
        //        resultObservation.effectiveTime = new IVL_TS
        //        {
        //            nullFlavor = "NI"
        //        };
        //        resultObservation.value = new List<ANY>
        //        {
        //            new PQ {nullFlavor = "UNK"}
        //        };
        //    }
        //    sb.component.Add(resultsComponent);
        //}


        private static void AddResultsSection(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var resultsComponent = HelperMethods.IntializeComponent3(
               new List<string> { "2.16.840.1.113883.10.20.22.2.3.1", "2.16.840.1.113883.10.20.22.2.3.1" },
               "30954-2", "2.16.840.1.113883.6.1", "LOINC", "RESULTS", "RESULTS");
            resultsComponent.section.templateId[0].extension = "2015-08-01";
            var text = resultsComponent.section.text;
            var lstResults = objCcdaModel.lstResults;

            var i = -1;

            List<Dictionary<string, string>> HtmlResults = new List<Dictionary<string, string>>();
            HtmlResults = lstResults.Select(x => new Dictionary<string, string>(x)).ToList();
  

            // Seperate Organisms
            var OrganismResults = new List<Dictionary<string, string>>();
            i = 0;
            while (i < HtmlResults.Count)
            {
                if (HtmlResults[i]["IsOrganism"].ToLower() == "true" || HtmlResults[i]["IsTriggered"].ToLower() == "1")
                {
                    OrganismResults.Add(HtmlResults[i]);
                    HtmlResults.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            // Seperate Triggers
            var TriggeredResults = new List<Dictionary<string, string>>();
            i = 0;
            while (i < HtmlResults.Count)
            {
                if (HtmlResults[i]["IsTriggered"].ToLower() == "true" || HtmlResults[i]["IsTriggered"].ToLower() == "1")
                {
                    TriggeredResults.Add(HtmlResults[i]);
                    HtmlResults.RemoveAt(i);
                }
                else { i++; }
            }

            // Seperate Test Type Results 
            var TestTypedResults = new List<Dictionary<string, string>>();

            TestTypedResults = HtmlResults;






            if (lstResults != null && lstResults.Count > 0)
            {

                #region TestTypedTables
                if (TestTypedResults.Count > 0)
                {

                    String[] breakApart = TestTypedResults[0]["TotalTestTypes"].Split(',');
                    for (i = 0; i < breakApart.Length; i++)
                    {

                        var TestTypesTable = new StrucDocTable
                        {
                            width = "100%",
                            border = "1"
                        };
                        text.Items.Add(TestTypesTable);
                        TestTypesTable.thead = new StrucDocThead
                        {
                            tr = new List<StrucDocTr>
                    {
                        new StrucDocTr
                        {
                            Items = new List<object>
                            {
                                new StrucDocTh
                                {
                                    Text = new List<string> {"Results Panel"},
                                    align = StrucDocThAlign.left
                                },
                                new StrucDocTh
                                {
                                    Text = new List<string> {"Date(s)"},
                                    align = StrucDocThAlign.left
                                },
                            }
                        }
                    }
                        };

                        TestTypesTable.tbody = new List<StrucDocTbody>();
                        var TestTypesTabletbody = new StrucDocTbody();
                        TestTypesTable.tbody.Add(TestTypesTabletbody);
                        TestTypesTabletbody.tr = new List<StrucDocTr>();

                        // int lRowNo = 0;

                        var labBodyTr = new StrucDocTr
                        {
                            Items = new List<object>
                        {
                            new StrucDocTd
                            {
                                Items = new List<object>
                                {
                                    new StrucDocContent
                                    {
                                        Text = new List<string> {MDVUtility.ToStr(breakApart[i])}
                                    }
                                }
                            },
                            new StrucDocTd
                            {
                                Items = new List<object>
                                {
                                    new StrucDocContent
                                    {
                                        Text = new List<string> { TestTypedResults[0]["ResultDate"]}
                                    }
                                }
                            }
                        }
                        };
                        var TypedTestsList = new List<StrucDocTr>();
                        // Prepare Tr for insertion
                        foreach (var item in TestTypedResults)
                        {


                            if (item["TestTypeName"] == breakApart[i].Trim())
                            {
                                var InnerTr = new StrucDocTr
                                {
                                    Items = new List<object>
                                {
                                    new StrucDocTd
                                    {
                                        Text = new List<string> { MDVUtility.ToStr(item["LabTestName"]) }

                                    },
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["ResultValue"]) }



                                    },
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["Flag"]) }

                                    },
                                   new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["ResultDate"]) }

                                    },
                                      new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["Range"]) }

                                    },
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["ReferenceRangeInt"]) }

                                    },
                                     new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["ReferenceRangeDesc"]) }

                                    }
                                }
                                };
                                TypedTestsList.Add(InnerTr);
                            }
                        }

                        var labBodyTrDetail = new StrucDocTr
                        {
                            Items = new List<object>
                        {
                            new StrucDocTd
                            {
                                Items = new List<object>
                                {
                                    new StrucDocList
                                    {
                                        item = new List<StrucDocItem>
                                        {
                                           new StrucDocItem
                                           {
                                               Items = new List<object>
                                               {
                                                    new StrucDocTable
                                                   {
                                                       thead = new StrucDocThead
                                                       {
                                                            tr = new List<StrucDocTr>
                                                            {

                                                               new StrucDocTr
                                                               {
                                                                   Items = new List<object>
                                                                   {
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> {"Test"}
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Outcome" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Interpretation" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Date(s)" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Reference Range" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Reference Range Interpretation" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Reference Range Description" }
                                                                       }
                                                                   }

                                                                }
                                                            }
                                                       },


                                                        tbody = new List<StrucDocTbody>
                                                        {
                                                            new StrucDocTbody
                                                            {
                                                                tr = TypedTestsList
                                                            }
                                                        }
                                                   }
                                               }
                                           }
                                        }
                                    }
                                }
                            }

                        }
                        };

                        TestTypesTabletbody.tr.Add(labBodyTr);
                        TestTypesTabletbody.tr.Add(labBodyTrDetail);
                    }
                }
                    #endregion
                
                #region TriggerredTests

                foreach (var item in TriggeredResults)
                {

                    var TriggeredTestsTable = new StrucDocTable
                    {
                        width = "100%",
                        border = "1"
                    };
                    text.Items.Add(TriggeredTestsTable);
                    TriggeredTestsTable.thead = new StrucDocThead
                    {
                        tr = new List<StrucDocTr>
                    {
                        new StrucDocTr
                        {
                            Items = new List<object>
                            {
                                new StrucDocTh
                                {
                                    Text = new List<string> {"Results Panel"},
                                    align = StrucDocThAlign.left
                                },
                                new StrucDocTh
                                {
                                    Text = new List<string> {"Date(s)"},
                                    align = StrucDocThAlign.left
                                },
                            }
                        }
                    }
                    };

                    TriggeredTestsTable.tbody = new List<StrucDocTbody>();
                    var TriggeredTestsTabletbody = new StrucDocTbody();
                    TriggeredTestsTable.tbody.Add(TriggeredTestsTabletbody);
                    TriggeredTestsTabletbody.tr = new List<StrucDocTr>();

                    // int lRowNo = 0;

                    var labBodyTr = new StrucDocTr
                    {
                        Items = new List<object>
                        {
                            new StrucDocTd
                            {

                                        Text = new List<string> {MDVUtility.ToStr(item["LabTestName"])}

                            },
                            new StrucDocTd
                            {

                                        Text = new List<string> {MDVUtility.ToStr(item["ResultDate"] + " to " + item["ResultDate"])}

                            }
                        }
                    };
                    var TriggeredTestsList = new List<StrucDocTr>();
                    // Prepare Tr for insertion

                    var InnerTr = new StrucDocTr
                    {
                        Items = new List<object>
                                {
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["LabTestName"]) }

                                    },
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["LoincCode"]) }

                                    },
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["CodeSystem"]) }

                                    },
                                   new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr("2.16.840.1.114222.4.11.7508") }

                                    },
                                      new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr("19/05/2016") }

                                    },
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["ActualResult"]) }

                                    },
                                     new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["Flag"]) }

                                    },
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["ResultDate"]) }

                                    },
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["Range"]) }

                                    },
                                   new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["ReferenceRangeInt"]) }

                                    }

                                }
                    };
                    TriggeredTestsList.Add(InnerTr);



                    var labBodyTrDetail = new StrucDocTr
                    {
                        Items = new List<object>
                        {
                            new StrucDocTd
                            {
                                Items = new List<object>
                                {
                                    new StrucDocList
                                    {
                                        item = new List<StrucDocItem>
                                        {
                                           new StrucDocItem
                                           {
                                               Items = new List<object>
                                               {
                                                    new StrucDocTable
                                                   {
                                                       thead = new StrucDocThead
                                                       {
                                                            tr = new List<StrucDocTr>
                                                            {

                                                               new StrucDocTr
                                                               {
                                                                   Items = new List<object>
                                                                   {
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Initial Case Report Trigger Code Result Observation" }
                                                                       },
                                                                                                                                              new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Trigger Code" }
                                                                       },
                                                                        new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Trigger Code codeSystem" }
                                                                       },

                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "RCTC OID" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "RCTC Version" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Outcome" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Interpretation" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Date(s)" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Reference Range" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Reference Range Interpretation" }
                                                                       }
                                                                   }

                                                                }
                                                            }
                                                       },


                                                        tbody = new List<StrucDocTbody>
                                                        {
                                                            new StrucDocTbody
                                                            {
                                                                tr = TriggeredTestsList
                                                            }
                                                        }
                                                   }
                                               }
                                           }
                                        }
                                    }
                                }
                            }

                        }
                    };

                    TriggeredTestsTabletbody.tr.Add(labBodyTr);
                    TriggeredTestsTabletbody.tr.Add(labBodyTrDetail);
                }

                #endregion

                #region TriggerredOrganisms

                foreach (var item in OrganismResults)
                {

                    var TriggeredOrganismsTable = new StrucDocTable
                    {
                        width = "100%",
                        border = "1"
                    };
                    text.Items.Add(TriggeredOrganismsTable);
                    TriggeredOrganismsTable.thead = new StrucDocThead
                    {
                        tr = new List<StrucDocTr>
                    {
                        new StrucDocTr
                        {
                            Items = new List<object>
                            {
                                new StrucDocTh
                                {
                                    Text = new List<string> {"Results Panel"},
                                    align = StrucDocThAlign.left
                                },
                                new StrucDocTh
                                {
                                    Text = new List<string> {"Date(s)"},
                                    align = StrucDocThAlign.left
                                },
                            }
                        }
                    }
                    };

                    TriggeredOrganismsTable.tbody = new List<StrucDocTbody>();
                    var TriggeredOrganismsTabletbody = new StrucDocTbody();
                    TriggeredOrganismsTable.tbody.Add(TriggeredOrganismsTabletbody);
                    TriggeredOrganismsTabletbody.tr = new List<StrucDocTr>();

                    // int lRowNo = 0;

                    var labBodyTr = new StrucDocTr
                    {
                        Items = new List<object>
                        {
                            new StrucDocTd
                            {

                                        Text = new List<string> {MDVUtility.ToStr(item["LabTestName"])}

                            },
                            new StrucDocTd
                            {

                                        Text = new List<string> {MDVUtility.ToStr(item["ResultDate"] + " to " + item["ResultDate"])}

                            }
                        }
                    };
                    var TriggeredOrganismsList = new List<StrucDocTr>();
                    // Prepare Tr for insertion

                    var InnerTr = new StrucDocTr
                    {
                        Items = new List<object>
                                {
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["LabTestName"]) }

                                    },
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["LoincCode"]) }

                                    },
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["CodeSystem"]) }

                                    },
                                   new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr("2.16.840.1.114222.4.11.7508") }

                                    },
                                      new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr("19/05/2016") }

                                    },
                                     new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["OrganismCodeDescription"]) }

                                    },
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["OrganismCode"]) }

                                    },
                                     new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr("SNOMED CT") }

                                    },
                                   new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr("2.16.840.1.114222.4.11.7508") }

                                    },
                                     new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr("19/05/2016") }

                                    },
                                     new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["Flag"]) }

                                    },
                                    new StrucDocTd
                                    {

                                                Text = new List<string> { MDVUtility.ToStr(item["ResultDate"]) }

                                    }


                                }
                    };
                    TriggeredOrganismsList.Add(InnerTr);



                    var labBodyTrDetail = new StrucDocTr
                    {
                        Items = new List<object>
                        {
                            new StrucDocTd
                            {
                                Items = new List<object>
                                {
                                    new StrucDocList
                                    {
                                        item = new List<StrucDocItem>
                                        {
                                           new StrucDocItem
                                           {
                                               Items = new List<object>
                                               {
                                                    new StrucDocTable
                                                   {
                                                       thead = new StrucDocThead
                                                       {
                                                            tr = new List<StrucDocTr>
                                                            {

                                                               new StrucDocTr
                                                               {
                                                                   Items = new List<object>
                                                                   {
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Initial Case Report Trigger Code Result Observation" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Trigger Code" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Trigger Code codeSystem" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "RCTC OID" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "RCTC Version" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Outcome" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Trigger Code" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Trigger Code codeSystem" }
                                                                       },
                                                                      new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "RCTC OID" }
                                                                       },
                                                                        new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "RCTC Version" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Interpretation" }
                                                                       },
                                                                       new StrucDocTh
                                                                       {
                                                                           Text = new List<string> { "Date(s)" }
                                                                       }

                                                                   }

                                                                }
                                                            }
                                                       },


                                                        tbody = new List<StrucDocTbody>
                                                        {
                                                            new StrucDocTbody
                                                            {
                                                                tr = TriggeredOrganismsList
                                                            }
                                                        }
                                                   }
                                               }
                                           }
                                        }
                                    }
                                }
                            }

                        }
                    };

                    TriggeredOrganismsTabletbody.tr.Add(labBodyTr);
                    TriggeredOrganismsTabletbody.tr.Add(labBodyTrDetail);
                }
                #endregion

                var lRowNo = 0;
                Organizer resultOrganizer = null;
                Entry resultOrganizerEntry = null;
                Component4 resultObservationComponent = null;
                resultsComponent.section.entry = new List<Entry>();
                string previsousType = "";
                string codeSystemName = "";
                foreach (var drOrderResult in lstResults)
                {
                    resultOrganizer = new Organizer();
                    if (previsousType != drOrderResult["TestTypeName"] && drOrderResult["TestTypeName"] != "")
                    {
                        resultOrganizerEntry = new Entry();
                        resultsComponent.section.entry.Add(resultOrganizerEntry);

                        resultOrganizerEntry.Item = resultOrganizer;
                        resultOrganizer.classCode = x_ActClassDocumentEntryOrganizer.BATTERY;
                        resultOrganizer.moodCode = "EVN";
                        resultOrganizer.templateId = new List<II>
                        {
                            new II {root = "2.16.840.1.113883.10.20.22.4.1", extension="2015-08-01"},
                            new II {root = "2.16.840.1.113883.10.20.22.4.1"}
                        };
                        resultOrganizer.id = new List<II>
                        {
                            new II {root = StaticCcdaData["UniqueIdentifierID"]}
                        };
                        codeSystemName = drOrderResult["TestTypeCodeSystem"];
                        if (!string.IsNullOrWhiteSpace(codeSystemName) && codeSystemName.ToLower() == "loinc")
                        {
                            resultOrganizer.code = new CE
                            {
                                code = MDVUtility.ToStr(drOrderResult["TestTypeCode"]),
                                displayName = MDVUtility.ToStr(drOrderResult["TestTypeName"]),
                                codeSystem = "2.16.840.1.113883.6.1",
                                codeSystemName = "LOINC"
                            };
                        }
                        else if (!string.IsNullOrWhiteSpace(codeSystemName) && codeSystemName.ToLower() != "loinc")
                        {
                            resultOrganizer.code = new CE
                            {
                                code = MDVUtility.ToStr(drOrderResult["TestTypeCode"]),
                                displayName = MDVUtility.ToStr(drOrderResult["TestTypeName"]),
                                codeSystem = "2.16.840.1.113883.6.96",
                                codeSystemName = "SNOMED CT",
                            };
                        }
                        resultOrganizer.statusCode = new CS
                        {
                            code = "completed"
                        };
                        resultOrganizer.component = new List<Component4>();
                        previsousType = drOrderResult["TestTypeName"];
                    }
                    else
                    {
                        resultOrganizerEntry = new Entry();
                        resultsComponent.section.entry.Add(resultOrganizerEntry);

                        resultOrganizerEntry.Item = resultOrganizer;
                        resultOrganizer.classCode = x_ActClassDocumentEntryOrganizer.BATTERY;
                        resultOrganizer.moodCode = "EVN";
                        resultOrganizer.templateId = new List<II>
                        {
                            new II {root = "2.16.840.1.113883.10.20.22.4.1", extension="2015-08-01"},
                            new II {root = "2.16.840.1.113883.10.20.22.4.1"}
                        };
                        resultOrganizer.id = new List<II>
                        {
                            new II {root = StaticCcdaData["UniqueIdentifierID"]}
                        };
                        codeSystemName = drOrderResult["TestTypeCodeSystem"];

                        resultOrganizer.code = new CE
                        {
                            code = Convert.ToString(drOrderResult["LoincCode"]),
                            displayName = Convert.ToString(drOrderResult["LabTestName"]),
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC"
                        };


                        resultOrganizer.statusCode = new CS
                        {
                            code = "completed"
                        };
                        resultOrganizer.component = new List<Component4>();
                        // previsousType = drOrderResult["TestTypeName"];
                    }

                    resultObservationComponent = new Component4();
                    resultOrganizer.component.Add(resultObservationComponent);
                    var resultObservation = new Observation();
                    resultObservationComponent.Item = resultObservation;
                    resultObservation.classCode = "OBS";
                    resultObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    resultObservation.templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.2", extension="2015-08-01"},
                        new II {root = "2.16.840.1.113883.10.20.22.4.2"}
                    };

                    resultObservation.id = new List<II>
                    {
                        new II {root = StaticCcdaData["UniqueIdentifierID"]}
                    };
                    resultObservation.code = new CE
                    {
                        code = Convert.ToString(drOrderResult["LoincCode"]),
                        displayName = Convert.ToString(drOrderResult["LabTestName"]),
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC"
                    };
                    resultObservation.text = new ED
                    {
                        reference = new TEL { value = string.Concat("#Results", ++lRowNo) }
                    };
                    resultObservation.statusCode = new CS
                    {
                        code = "completed"
                    };
                    resultObservation.effectiveTime = new IVL_TS
                    {
                        value = Convert.ToDateTime(drOrderResult["ResultDate"]).ToString("yyyyMMddhhmm")
                    };
                    var resultValue = Convert.ToString(drOrderResult["ResultValue"]);
                    if (string.IsNullOrWhiteSpace(resultValue))
                    {
                        resultObservation.value = new List<ANY>
                        {
                            new ST
                            {
                                Text = new List<string> {"Pending"}
                            }
                        };
                    }
                    else
                    {
                        resultObservation.value = new List<ANY>
                        {
                            new ST
                            {
                               Text = new List<string> { Convert.ToString(drOrderResult["ResultValue"]) }
                            }
                        };
                        resultObservation.interpretationCode = new List<CE>
                        {
                            new CE
                            {
                                code=  string.IsNullOrEmpty(Convert.ToString(drOrderResult["Unit"]))
                                        ? "UNK"
                                        : Convert.ToString(drOrderResult["Unit"]),
                                codeSystem="2.16.840.1.113883.5.83"
                            }
                        };
                        resultObservation.referenceRange = new List<ReferenceRange> {
                        new ReferenceRange
                        {
                            observationRange=new ObservationRange
                            {
                                value=new ST
                                {
                                    Text = new List<string> { Convert.ToString(drOrderResult["ResultValue"]) }
                                }

                            },
                        }
                        };
                    }
                }
            }
            else
            {
                if (lstResults == null || lstResults.Count == 0)
                {
                    text.Items.Add(new StrucDocContent
                    {
                        ID = "noResults",
                        Text = new List<string> { "Not included in document" }
                    });
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
                    new II {root = "2.16.840.1.113883.10.20.22.4.1", extension="2015-08-01"},
                    new II {root = "2.16.840.1.113883.10.20.22.4.1"}
                };
                resultOrganizer.id = new List<II>
                {
                    new II {nullFlavor = "UNK"}
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
                     new II {root = "2.16.840.1.113883.10.20.22.4.2", extension="2015-08-01"},
                     new II {root = "2.16.840.1.113883.10.20.22.4.2"}
                };
                resultObservation.id = new List<II>
                {
                    new II {root = "NI"}
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
                    new PQ {nullFlavor = "UNK"}
                };
            }
            sb.component.Add(resultsComponent);
        }

        private static void AddPlanOfCareComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var component = HelperMethodsCaseReports.IntializeComponent3(
            new List<string> { "2.16.840.1.113883.10.20.22.2.10", "2.16.840.1.113883.10.20.22.2.10" },
            "18776-5", "2.16.840.1.113883.6.1", "LOINC", "Plan of Care", "PLAN OF CARE");
            component.section.templateId[0].extension = "2014-06-09";
            var text = component.section.text;
            var lstscheduledProcedure = objCcdaModel.lstscheduledProcedure ?? new List<Dictionary<string, string>>();
            if (objCcdaModel.lstFutureAppointment == null)
                objCcdaModel.lstFutureAppointment = new List<Dictionary<string, string>>();
            if (lstscheduledProcedure.Count > 0 || objCcdaModel.lstGoal.Count > 0 ||
                objCcdaModel.lstFutureAppointment.Count > 0 || objCcdaModel.lstRefferalProviderData.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);
                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                var headTr = new StrucDocTr();
                table.thead.tr.Add(headTr);
                headTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Name"}},
                    new StrucDocTh {Text = new List<string> {"Instruction"}},
                    new StrucDocTh {Text = new List<string> {"Type"}},
                    new StrucDocTh {Text = new List<string> {"Date"}}
                };
                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);

                tbody.tr = new List<StrucDocTr>();
                if (objCcdaModel.lstscheduledProcedure != null)
                    foreach (Dictionary<string, string> dr in objCcdaModel.lstscheduledProcedure)
                    {
                        var bodyTr = new StrucDocTr
                        {
                            Items = new List<object>
                            {
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["Name"])
                                    }
                                },
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["Instruction"]) == ""
                                            ? "-"
                                            : MDVUtility.ToStr(dr["Instruction"])
                                    }
                                },
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["Type"])
                                    }
                                }
                            }
                        };



                        var visitDate = dr["Date"];
                        if (!string.IsNullOrWhiteSpace(visitDate))
                        {
                            visitDate = Convert.ToDateTime(visitDate).ToString("MM/dd/yyyy");
                        }
                        bodyTr.Items.Add(new StrucDocTd
                        {
                            Text = new List<string>
                            {
                                visitDate
                            }
                        });

                        tbody.tr.Add(bodyTr);
                    }
                foreach (var dr in objCcdaModel.lstRefferalProviderData)
                {
                    var bodyTr = new StrucDocTr
                    {
                        Items = new List<object>
                        {
                            new StrucDocTd
                            {
                                Text = new List<string>
                                {
                                    Convert.ToString(dr["Name"])
                                }
                            },
                            new StrucDocTd
                            {
                                Text = new List<string>
                                {
                                    MDVUtility.ToStr(dr["Instruction"]) == ""
                                        ? "-"
                                        : MDVUtility.ToStr(dr["Instruction"])
                                }
                            },
                            new StrucDocTd
                            {
                                Text = new List<string>
                                {
                                    MDVUtility.ToStr(dr["Type"])
                                }
                            }
                        }
                    };
                    var visitDate = dr["Date"];
                    if (!string.IsNullOrWhiteSpace(visitDate))
                    {
                        visitDate = Convert.ToDateTime(visitDate).ToString("MM/dd/yyyy");
                    }
                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string>
                        {
                            visitDate
                        }
                    });

                    tbody.tr.Add(bodyTr);
                }
                foreach (Dictionary<string, string> dr in objCcdaModel.lstFutureAppointment)
                {
                    var bodyTr = new StrucDocTr
                    {
                        Items = new List<object>
                        {
                            new StrucDocTd
                            {
                                Text = new List<string>
                                {
                                    MDVUtility.ToStr(dr["Name"])
                                }
                            },
                            new StrucDocTd
                            {
                                Text = new List<string>
                                {
                                    MDVUtility.ToStr(dr["Instruction"]) == ""
                                        ? "-"
                                        : MDVUtility.ToStr(dr["Instruction"])
                                }
                            },
                            new StrucDocTd
                            {
                                Text = new List<string>
                                {
                                    MDVUtility.ToStr(dr["Type"])
                                }
                            }
                        }
                    };
                    var visitDate = dr["Date"];
                    if (!string.IsNullOrWhiteSpace(visitDate))
                    {
                        visitDate = Convert.ToDateTime(visitDate).ToString("MM/dd/yyyy");
                    }
                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string>
                        {
                            visitDate
                        }
                    });

                    tbody.tr.Add(bodyTr);
                }
                foreach (var dr in objCcdaModel.lstGoal)
                {
                    var bodyTr = new StrucDocTr
                    {
                        Items = new List<object>
                        {
                            new StrucDocTd
                            {
                                Text = new List<string>
                                {
                                    MDVUtility.ToStr(dr["Name"])
                                }
                            },
                            new StrucDocTd
                            {
                                Text = new List<string>
                                {
                                    MDVUtility.ToStr(dr["Instruction"]) == ""
                                        ? "-"
                                        : MDVUtility.ToStr(dr["Instruction"])
                                }
                            },
                            new StrucDocTd
                            {
                                Text = new List<string>
                                {
                                    MDVUtility.ToStr(dr["Type"])
                                }
                            }
                        }
                    };
                    var visitDate = objCcdaModel.VisitDate;
                    if (!string.IsNullOrWhiteSpace(visitDate))
                    {
                        visitDate = Convert.ToDateTime(visitDate).ToString("MM/dd/yyyy");
                    }
                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string>
                        {
                            visitDate
                        }
                    });
                    tbody.tr.Add(bodyTr);
                }
                component.section.entry = new List<Entry>();
            }
            else
            {
                if (lstscheduledProcedure.Count == 0 || objCcdaModel.lstGoal.Count == 0 ||
                    objCcdaModel.lstFutureAppointment.Count == 0)
                {
                    text.Items.Add(new StrucDocContent
                    {
                        ID = "noPlanOfCare",
                        Text = new List<string> { "Not included in document" }
                    });
                }
            }

            sb.component.Add(component);
        }

        private static void AddGoalsComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var component = HelperMethodsCaseReports.IntializeComponent3(
            new List<string> { "2.16.840.1.113883.10.20.22.2.60" },
            "61146-7", "2.16.840.1.113883.6.1", "LOINC", "Goals", "Goals Section");
            var text = component.section.text;
            if (objCcdaModel.lstGoal.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);
                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                var headTr = new StrucDocTr();
                table.thead.tr.Add(headTr);
                headTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Name"}},
                    new StrucDocTh {Text = new List<string> {"Instruction"}},
                    new StrucDocTh {Text = new List<string> {"Type"}},
                    new StrucDocTh {Text = new List<string> {"Date"}}
                };
                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);

                tbody.tr = new List<StrucDocTr>();
                foreach (var dr in objCcdaModel.lstGoal)
                {
                    var bodyTr = new StrucDocTr
                    {
                        Items = new List<object>
                        {
                            new StrucDocTd
                            {
                                Text = new List<string>
                                {
                                    MDVUtility.ToStr(dr["Name"])
                                }
                            },
                            new StrucDocTd
                            {
                                Text = new List<string>
                                {
                                    MDVUtility.ToStr(dr["Instruction"]) == ""
                                        ? "-"
                                        : MDVUtility.ToStr(dr["Instruction"])
                                }
                            },
                            new StrucDocTd
                            {
                                Text = new List<string>
                                {
                                    MDVUtility.ToStr(dr["Type"])
                                }
                            }
                        }
                    };
                    var visitDate = objCcdaModel.VisitDate;
                    if (!string.IsNullOrWhiteSpace(visitDate))
                    {
                        visitDate = Convert.ToDateTime(visitDate).ToString("MM/dd/yyyy");
                    }
                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string>
                        {
                            visitDate
                        }
                    });
                    tbody.tr.Add(bodyTr);
                }
                component.section.entry = new List<Entry>();
            }
            else
            {
                if (objCcdaModel.lstGoal.Count == 0)
                {
                    text.Items.Add(new StrucDocContent
                    {
                        ID = "noGoals",
                        Text = new List<string> { "Not included in document" }
                    });
                    component.section.entry = new List<Entry>();
                    var obsevationEntry = new Entry();
                    component.section.entry.Add(obsevationEntry);
                    var observation = new Observation();
                    obsevationEntry.Item = observation;

                    observation.classCode = "OBS";
                    observation.moodCode = x_ActMoodDocumentObservation.GOL;
                    observation.templateId = new List<II>
                    {
                    new II {root = "2.16.840.1.113883.10.20.22.4.121"}
                    };
                    observation.id = new List<II>
                    {
                        new II
                        {
                            nullFlavor ="UNK"
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
                }
            }

            sb.component.Add(component);
        }

        private static void AddChiefComplaintComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var chiefComplaintsComponent = HelperMethodsCaseReports.IntializeComponent3(
                           new List<string> { "2.16.840.1.113883.10.20.22.2.12" },
                           "29299-5", "2.16.840.1.113883.6.1", "LOINC", "Reason for Visit", "Reason for Visit");

            var text = chiefComplaintsComponent.section.text;
            var lstReasonForVisit = objCcdaModel.ReasonForVisit;
            if (lstReasonForVisit.Count > 0)
            {
                var para = new StrucDocParagraph
                {

                    Text = new List<string> { MDVUtility.ToStr(lstReasonForVisit[0]) }
                };
                text.Items.Add(para);
                // para.Text = "sadas";

            }
            else
                text.Text = new List<string> { "Not known" };


            //var chiefComplaintsComponent = HelperMethodsCaseReports.IntializeComponent3(
            //    new List<string> { "2.16.840.1.113883.10.20.22.2.13" },
            //    "46239-0", "2.16.840.1.113883.6.1", "LOINC", "Chief Complaint and Reason for Visit", "REASON FOR VISIT/CHIEF COMPLAINT");

            //var text = chiefComplaintsComponent.section.text;
            //var lstReasonForVisit = objCcdaModel.ReasonForVisit;

            //if (lstReasonForVisit != null && lstReasonForVisit.Count > 0)
            //{
            //    StrucDocTable table = new StrucDocTable
            //    {
            //        border = "1",
            //        width = "100%",
            //        thead = new StrucDocThead
            //        {
            //            tr = new List<StrucDocTr>
            //            {
            //                new StrucDocTr
            //                {
            //                    Items = new List<object>
            //                    {
            //                        new StrucDocTh {Text = new List<string> {"REASON FOR VISIT/CHIEF COMPLAINT"}}
            //                    }
            //                }
            //            }
            //        }
            //    };
            //    text.Items.Add(table);
            //    table.tbody = new List<StrucDocTbody>();
            //    var tBody = new StrucDocTbody();
            //    table.tbody.Add(tBody);
            //    tBody.tr = new List<StrucDocTr>();
            //    foreach (var reason in lstReasonForVisit)
            //    {
            //        var tr = new StrucDocTr();
            //        tBody.tr.Add(tr);

            //        tr.Items = new List<object>
            //        {
            //              new StrucDocTd { Text = new List<string> { reason } }
            //        };
            //    }
            //}
            //else
            //    text.Items.Add(new StrucDocContent { ID = "noChiefComplaints", Text = new List<string> { "Unknown ChiefComplaint" } });
            sb.component.Add(chiefComplaintsComponent);
        }

        private static void AddImmunizationComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var immunizationsComponent = HelperMethodsCaseReports.IntializeComponent3(
                new List<string> { "2.16.840.1.113883.10.20.22.2.2.1", "2.16.840.1.113883.10.20.22.2.2.1" },
                "11369-6", "2.16.840.1.113883.6.1", "LOINC", "Immunizations", "IMMUNIZATION");

            immunizationsComponent.section.templateId[0].extension = "2014-06-09";
            var text = immunizationsComponent.section.text;
            var lstImmunization = objCcdaModel.lstImmunization;
            if (lstImmunization != null && lstImmunization.Count > 0)
            {
                var content = new StrucDocContent { ID = "Immunization" };
                text.Items.Add(content);
                var table = new StrucDocTable
                {
                    border = "1",
                    width = "100%"
                };
                text.Items.Add(table);
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                var headTr = new StrucDocTr
                {
                    Items = new List<object>
                    {
                        new StrucDocTh {Text = new List<string> {"Vaccine Name"}},
                        new StrucDocTh {Text = new List<string> {"Date"}},
                        new StrucDocTh {Text = new List<string> {"Status"}}
                    }
                };
                table.thead.tr.Add(headTr);
                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);
                tbody.tr = new List<StrucDocTr>();
                var counter = lstImmunization.Count;
                for (var i = 0; i < counter; i++)
                {
                    var dr = lstImmunization[i];
                    var tr = new StrucDocTr();
                    tbody.tr.Add(tr);
                    var status = dr["CompletionStatusCode"];
                    status = HelperMethodsCaseReports.GetCompletionStatusOfImmunization(status);
                    var expiryDate = dr["ExpiryDate"];
                    if (!string.IsNullOrWhiteSpace(expiryDate))
                        expiryDate = Convert.ToDateTime(expiryDate).ToString("MM/dd/yyyy");
                    var immunization = MDVUtility.ToStr(dr["Immunization"]);
                    if (!string.IsNullOrWhiteSpace(dr["CVXCode"]))
                        immunization = string.Concat(immunization, ", [CVX: ", dr["CVXCode"], "]");
                    tr.Items = new List<object>
                    {
                        new StrucDocTd
                        {
                            Items = new List<object>
                            {
                                new StrucDocContent
                                {
                                    ID = string.Concat("Immune", i + 1)
                                }
                            },
                            Text = new List<string>{ immunization }
                        },

                        new StrucDocTd
                        {
                                Text = new List<string>{ expiryDate }
                        },
                        new StrucDocTd
                        {
                                Text = new List<string>{ status }
                        }
                    };
                }
                immunizationsComponent.section.entry = new List<Entry>();

                for (var i = 0; i < counter; i++)
                {
                    var dr = lstImmunization[i];
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
                        reference = new TEL { value = string.Concat("#Immune", i + 1) }
                    };
                    immunizationActivity.statusCode = new CS
                    {
                        code = "completed"
                    };
                    var dateAdmin = dr["DateAdministered"];
                    if (!string.IsNullOrWhiteSpace(dateAdmin))
                    {
                        dateAdmin = Convert.ToDateTime(dateAdmin).ToString("yyyyMMdd");
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
                    var consumable = new Consumable();
                    var manufacturedProduct = new ManufacturedProduct();
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
                            code = MDVUtility.ToStr(dr["CVX"]),
                            codeSystem = "2.16.840.1.113883.12.292",
                            codeSystemName = "CVX",
                            displayName = MDVUtility.ToStr(dr["Immunization"]),
                            originalText = new ED
                            {
                                reference = new TEL { value = string.Concat("#Immune", i + 1) }
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
                    new II {root = "2.16.840.1.113883.10.20.22.4.52"}
                };
                immunizationActivity.id = new List<II>
                {
                    new II {nullFlavor = "NA"}
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

        private static void AddProcedureComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var proComponent = HelperMethodsCaseReports.IntializeComponent3(
               new List<string> { "2.16.840.1.113883.10.20.22.2.7.1", "2.16.840.1.113883.10.20.22.2.7.1" },
               "47519-4", "2.16.840.1.113883.6.1", "LOINC", "History of Procedures", "PROCEDURES");

            proComponent.section.templateId[0].extension = "2014-06-09";
            var text = proComponent.section.text;
            var lstProcedure = objCcdaModel.lstProcedure;
            if (objCcdaModel.lstProcedure != null && objCcdaModel.lstProcedure.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);
                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                var headTr = new StrucDocTr();
                table.thead.tr.Add(headTr);
                headTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Procedure"}},
                    new StrucDocTh {Text = new List<string> {"Date"}}
                };
                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);
                tbody.tr = new List<StrucDocTr>();
                for (var i = 0; i < lstProcedure.Count; i++)
                {
                    var bodyTr = new StrucDocTr
                    {
                        Items = new List<object>
                        {
                            new StrucDocTd
                            {
                                Items = new List<object> {new StrucDocContent {ID = "Procedure" + (i + 1)}},
                                Text = new List<string> {MDVUtility.ToStr(lstProcedure[i]["ProcedureName"])}
                            }
                        }
                    };
                    var procedureDate = lstProcedure[i]["ProcedureDate"];
                    if (!string.IsNullOrWhiteSpace(procedureDate))
                    {
                        procedureDate = Convert.ToDateTime(procedureDate).ToString("MM/dd/yyyy");
                    }
                    bodyTr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string>
                        {
                            procedureDate
                        }
                    });
                    tbody.tr.Add(bodyTr);
                }
                proComponent.section.entry = new List<Entry>();

                foreach (var procDictionary in lstProcedure)
                {
                    var drProcedure = procDictionary;

                    var obsevationEntry = new Entry();
                    proComponent.section.entry.Add(obsevationEntry);
                    var procedure = new Procedure
                    {
                        classCode = "PROC",
                        moodCode = x_DocumentProcedureMood.EVN,
                        templateId = new List<II>
                        {
                            new II {root = "2.16.840.1.113883.10.20.22.4.14",extension="2014-06-09"},
                            new II {root = "2.16.840.1.113883.10.20.22.4.14"},
                        },
                        id = new List<II>
                        {
                            new II
                            {
                                root = StaticCcdaData["UniqueIdentifierID"]
                            }
                        }
                    };
                    if (!string.IsNullOrWhiteSpace(drProcedure["SnomedId"]))
                    {
                        procedure.code = new CD
                        {
                            code = drProcedure["SnomedId"],
                            codeSystem = "2.16.840.1.113883.6.96",
                            codeSystemName = "SNOMED CT",
                            displayName = drProcedure["ProcedureName"]
                        };
                    }
                    else if (!string.IsNullOrWhiteSpace(drProcedure["CPT"]))
                    {
                        CPTLookupModel model = HelperMethodsCaseReports.GetCPT(drProcedure["CPT"]);
                        if (model != null)
                        {
                            procedure.code = new CD
                            {
                                code = model.SNOMEDID,
                                codeSystem = "2.16.840.1.113883.6.96",
                                codeSystemName = "SNOMED CT",
                                displayName = model.SNOMEDDescription
                            };

                        }
                        else
                        {
                            procedure.code = new CD
                            {
                                code = drProcedure["CPT"],
                                codeSystem = "2.16.840.1.113883.6.12",
                                codeSystemName = "CPT-4",
                                displayName = drProcedure["ProcedureName"]
                            };
                        }
                    }
                    procedure.statusCode = new CS
                    {
                        code = "active"
                    };
                    var procedureDate = procDictionary["ProcedureDate"];
                    if (!string.IsNullOrWhiteSpace(procedureDate))
                    {
                        procedureDate = Convert.ToDateTime(procedureDate).ToString("yyyyMMdd");
                    }
                    procedure.effectiveTime = new IVL_TS
                    {
                        value = procedureDate
                    };
                    procedure.methodCode = new List<CE>
                    {
                        new CE
                        {
                            nullFlavor = "UNK"
                        }
                    };
                    procedure.targetSiteCode = new List<CD>
                    {
                         new CD
                        {
                            nullFlavor = "UNK"
                        }
                    };
                    obsevationEntry.Item = procedure;
                }
            }
            else
            {
                if (objCcdaModel.lstProcedure == null || objCcdaModel.lstProcedure.Count == 0)
                {
                    text.Items.Add(new StrucDocContent
                    {
                        ID = "noProcedure",
                        Text = new List<string> { "Not included in document" }
                    });
                }
                proComponent.section.entry = new List<Entry>();
                var obsevationEntry = new Entry();
                proComponent.section.entry.Add(obsevationEntry);
                var observation = new Observation();
                obsevationEntry.Item = observation;

                observation.classCode = "OBS";
                observation.moodCode = x_ActMoodDocumentObservation.EVN;
                observation.templateId = new List<II>
                {
                    new II {root = "2.16.840.1.113883.10.20.22.4.14"}
                };
                observation.id = new List<II>
                {
                    new II
                    {
                        root = StaticCcdaData["UniqueIdentifierID"]
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
                    new CD {nullFlavor = "NI"}
                };
            }
            sb.component.Add(proComponent);
        }

        private static void AddMedicationAdministeredComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var medicationComponent = HelperMethodsCaseReports.IntializeComponent3(
               new List<string> { "2.16.840.1.113883.10.20.22.2.38" },
               "29549-3", "2.16.840.1.113883.6.1", "LOINC", "Medications Administered", "MEDICATIONS ADMINISTERED");

            var text = medicationComponent.section.text;
            var lstMedicationAdministered = objCcdaModel.lstMedicationsAdministered;
            if (objCcdaModel.lstMedicationsAdministered != null && objCcdaModel.lstMedicationsAdministered.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);
                table.Items = new List<object>(getCols(9));
                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                var headerTr = new StrucDocTr();
                table.thead.tr.Add(headerTr);
                headerTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Medication"}},
                    new StrucDocTh {Text = new List<string> {"Dose"}},
                    new StrucDocTh {Text = new List<string> {"Duration"}},
                    new StrucDocTh {Text = new List<string> {"Route"}},
                    //new StrucDocTh {Text = new List<string> {"Status"}},
                    //new StrucDocTh {Text = new List<string> {"Indications"}},
                    //new StrucDocTh {Text = new List<string> {"Fill Instructions"}}
                };
                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);
                tbody.tr = new List<StrucDocTr>();
                var id = 0;
                foreach (var medication in lstMedicationAdministered)
                {
                    var bodyTr = new StrucDocTr { Items = new List<object>() };
                    var startDate = medication["StartDate"];
                    if (startDate != string.Empty)
                    {
                        startDate = Convert.ToDateTime(startDate).ToString("MM/dd/yyyy");
                    }
                    bodyTr.Items.Add(
                    new StrucDocTd
                    {
                        Items = new List<object>
                        {
                            new StrucDocContent
                            {
                               // ID = string.Concat("Medication_", ++id),
                                Text = new List<string> {medication["MedicationName"] }
                            }
                        }
                    });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["Dose"] + " " + medication["DoseUnit"] } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { startDate } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["RouteDescription"] } });
                    //bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["Status"] } });
                    //bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["Indication"] } });
                    //string substituation = medication["Substitution"];

                    //bodyTr.Items.Add(substituation.ToLower() == "y"
                    //    ? new StrucDocTd { Text = new List<string> { "Generic Substitution Allowed" } }
                    //    : new StrucDocTd { Text = new List<string> { "Generic Substitution not Allowed" } });
                    tbody.tr.Add(bodyTr);

                }
                medicationComponent.section.entry = new List<Entry>();
                id = 0;
                foreach (var medicationDictionary in lstMedicationAdministered)
                {
                    var medicationEntry = new Entry
                    {
                        typeCode = x_ActRelationshipEntry.DRIV,
                        Item = SetMedicationActivity(medicationDictionary,
                            objCcdaModel.lstPracticeData[0]["ShortName"], string.Concat("#MedicationAdminst_", ++id))
                    };
                    medicationComponent.section.entry.Add(medicationEntry);
                }
            }
            else
            {
                var noMedicationText = new StrucDocContent();
                text.Items.Add(noMedicationText);
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
                    new II {root = "2.16.840.1.113883.10.20.22.4.16",extension= "2014-06-09"},
                    new II {root = "2.16.840.1.113883.10.20.22.4.16"}
                };
                medicationActivity.id = new List<II>
                {
                    new II {root = StaticCcdaData["UniqueIdentifierID"]}
                };
                medicationActivity.statusCode = new CS { code = "completed" };
                medicationActivity.effectiveTime = new List<SXCM_TS>
                {
                    new IVL_TS
                    {
                        ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                        Items = new QTY[] {new IVXB_TS {nullFlavor = "NI"}, new IVXB_TS {nullFlavor = "NI"}}
                    }
                };

                var consumable = new Consumable();
                medicationActivity.consumable = consumable;
                var manufacturedProduct = new ManufacturedProduct();
                consumable.manufacturedProduct = manufacturedProduct;
                manufacturedProduct.classCode = RoleClassManufacturedProduct.MANU;
                manufacturedProduct.classCodeSpecified = true;
                manufacturedProduct.templateId = new List<II> { new II { root = "2.16.840.1.113883.10.20.22.4.23",extension="2014-06-09" } ,
                new II { root = "2.16.840.1.113883.10.20.22.4.23" }};
                var manufacturedMaterial = new Material();
                manufacturedProduct.Item = manufacturedMaterial;
                manufacturedMaterial.code = new CE
                {
                    nullFlavor = "NI",
                };
            }
            sb.component.Add(medicationComponent);
        }

        private static void AddProblemListComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var problemListComponent = HelperMethodsCaseReports.IntializeComponent3(
                  new List<string> { "2.16.840.1.113883.10.20.22.2.5.1", "2.16.840.1.113883.10.20.22.2.5.1" },
                  "11450-4", "2.16.840.1.113883.6.1", "LOINC", "Problems", "PROBLEMS");

            var text = problemListComponent.section.text;
            problemListComponent.section.templateId[0].extension = "2015-08-01";
            var lstProblemList = objCcdaModel.lstProblems;
            if (lstProblemList != null && lstProblemList.Count > 0)
            {
                var tablemain = new StrucDocTable
                {
                    Items = new List<object>(getCols(9)),
                    thead = new StrucDocThead
                    {
                        tr = new List<StrucDocTr>
                        {
                            new StrucDocTr
                            {
                                Items = new List<object>
                                {
                                    new StrucDocTh {Text = new List<string> {"Concern"}},
                                    new StrucDocTh {Text = new List<string> {"Concern Status"}},
                                    new StrucDocTh {Text = new List<string> {"Date(s)"}}

                                }
                            }
                        }
                    },
                    tbody = new List<StrucDocTbody>()
                };

                var tBodymain = new StrucDocTbody { tr = new List<StrucDocTr>() };
                tablemain.tbody.Add(tBodymain);
                text.Items.Add(tablemain);
                var counter = lstProblemList.Count;

                var trmain = new StrucDocTr { Items = new List<object>() };
                tBodymain.tr.Add(trmain);
                trmain.Items.Add(new StrucDocTd
                {
                    Text = new List<string> { "Problem" }
                });
                var status2 = lstProblemList[0]["Status"];
                if (status2 != "Resolved" && status2 != "Inactive")
                {
                    status2 = "Active";
                }
                trmain.Items.Add(new StrucDocTd
                {
                    Text = new List<string> { status2 }
                });
                var effectiveDate2 = lstProblemList[0]["EffectiveDate"];
                if (!string.IsNullOrWhiteSpace(effectiveDate2))
                    effectiveDate2 = Convert.ToDateTime(effectiveDate2).ToString("MM/dd/yyyy");

                trmain.Items.Add(new StrucDocTd
                {
                    Text = new List<string> { effectiveDate2 }
                });


                // Dynamic binding
                var table = new StrucDocTable
                {
                    Items = new List<object>(getCols(9)),
                    thead = new StrucDocThead
                    {
                        tr = new List<StrucDocTr>
                        {
                            new StrucDocTr
                            {
                                Items = new List<object>
                                {
                                    new StrucDocTh {Text = new List<string> {"Problem Type"}},
                                    new StrucDocTh {Text = new List<string> {"Problem"}},
                                    new StrucDocTh {Text = new List<string> {"Date(s)"}}

                                }
                            }
                        }
                    },
                    tbody = new List<StrucDocTbody>()
                };

                var tBody = new StrucDocTbody { tr = new List<StrucDocTr>() };
                table.tbody.Add(tBody);
                text.Items.Add(table);

                for (var i = 0; i < counter; i++)
                {
                    var tr = new StrucDocTr { Items = new List<object>() };
                    tBody.tr.Add(tr);
                    tr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> { lstProblemList[i]["ProblemType"] }
                    });

                    tr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> { lstProblemList[i]["ProblemName"] }
                    });
                    var effectiveDate1 = lstProblemList[i]["EffectiveDate"];
                    if (!string.IsNullOrWhiteSpace(effectiveDate1))
                        effectiveDate1 = Convert.ToDateTime(effectiveDate1).ToString("MM/dd/yyyy");

                    tr.Items.Add(new StrucDocTd
                    {
                        Text = new List<string> { effectiveDate1 }
                    });


                }

                problemListComponent.section.text = text;
                problemListComponent.section.entry = new List<Entry>();
                for (var i = 0; i < lstProblemList.Count; i++)
                {
                    var problem = lstProblemList[i];
                    var problemEntry = new Entry { typeCode = x_ActRelationshipEntry.DRIV };
                    var concernAct = new Act
                    {
                        classCode = x_ActClassDocumentEntryAct.ACT,
                        moodCode = x_DocumentActMood.EVN,
                        templateId = new List<II>
                        {
                            new II {root = "2.16.840.1.113883.10.20.22.4.3",extension="2015-08-01"},
                            new II {root = "2.16.840.1.113883.10.20.22.4.3"}

                        },
                        id = new List<II>
                        {
                            new II {root = StaticCcdaData["UniqueIdentifierID"]}
                        },
                        code = new CD
                        {
                            code = "CONC",
                            codeSystem = "2.16.840.1.113883.5.6",
                            displayName = "Concern"
                        },
                        statusCode = new CS
                        {
                            code = "completed"
                        },
                        effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new ItemsChoiceType2[2],
                            Items = new QTY[2]
                        }
                    };
                    var effectiveDate = problem["EffectiveDate"];
                    effectiveDate = HelperMethods.SetDateTime(!string.IsNullOrWhiteSpace(effectiveDate) ? Convert.ToDateTime(effectiveDate) : DateTime.Now);
                    var lowTime = effectiveDate;
                    if (!string.IsNullOrWhiteSpace(lowTime))
                    {
                        concernAct.effectiveTime.ItemsElementName[0] = ItemsChoiceType2.low;
                        concernAct.effectiveTime.Items[0] = new IVXB_TS { value = lowTime };
                    }

                    concernAct.entryRelationship = new List<EntryRelationship>();
                    var problemEntryRelationship = new EntryRelationship
                    {
                        typeCode = x_ActRelationshipEntryRelationship.SUBJ
                    };
                    var problemObservation = new Observation();
                    problemEntryRelationship.Item = problemObservation;
                    problemObservation.classCode = "OBS";
                    problemObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    problemObservation.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.4",extension="2015-08-01" },
                        new II { root = "2.16.840.1.113883.10.20.22.4.4" }

                    };
                    problemObservation.id = new List<II>
                    {
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };
                    if (problem["ProblemType"].ToLower() == "Symptom")
                    {
                        problemObservation.code = new CD
                        {
                            code = "75325-1",
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC",
                            displayName = "Symptom",
                            translation = new List<CD>
                            {
                                new CD
                                {
                                    code = "418799008",
                                    codeSystem = "2.16.840.1.113883.6.96",
                                    codeSystemName = "SNOMED CT",
                                    displayName = "Symptom",
                                }
                            }
                        };
                    }
                    else if (problem["ProblemType"].ToLower() == "complaint")
                    {
                        problemObservation.code = new CD
                        {
                            code = "75322-8",
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC",
                            displayName = "Complaint",
                            translation = new List<CD>
                            {
                                new CD
                                {
                                    code = "409586006",
                                    codeSystem = "2.16.840.1.113883.6.96",
                                    codeSystemName = "SNOMED CT",
                                    displayName = "Complaint",
                                }
                            }
                        };
                    }
                    else
                    {
                        problemObservation.code = new CD
                        {
                            code = "55607006",
                            codeSystem = "2.16.840.1.113883.6.96",
                            displayName = "Problem",
                            translation = new List<CD>
                            {
                                new CD
                                {
                                    code = "75323-6",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "Condition",
                                }
                            }
                        };
                    }

                    problemObservation.text = new ED
                    {
                        reference = new TEL { value = string.Concat("#problem", i + 1) }
                    };
                    problemObservation.statusCode = new CS
                    {
                        code = "completed"
                    };
                    effectiveDate = problem["EffectiveDate"];
                    effectiveDate = HelperMethods.SetDateTime(!string.IsNullOrWhiteSpace(effectiveDate) ? Convert.ToDateTime(effectiveDate) : DateTime.Now);
                    lowTime = effectiveDate;
                    if (!string.IsNullOrWhiteSpace(lowTime))
                    {
                        problemObservation.effectiveTime = new IVL_TS
                        {
                            Items = new QTY[1],
                            ItemsElementName = new ItemsChoiceType2[1]
                        };

                        problemObservation.effectiveTime.ItemsElementName[0] = ItemsChoiceType2.low;
                        problemObservation.effectiveTime.Items[0] = new IVXB_TS { value = lowTime };
                    }
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(problem["SNOMEDID"])))
                    {
                        problemObservation.value = new List<ANY>
                        {
                            new CD
                            {
                                code = MDVUtility.ToStr(problem["SNOMEDID"]),
                                codeSystem = "2.16.840.1.113883.6.96",
                                displayName = MDVUtility.ToStr(problem["ProblemName"])
                            }
                        };
                    }
                    else
                    {
                        problemObservation.value = new List<ANY>
                        {
                            new CD
                            {
                                nullFlavor = "UNK"
                            }
                        };
                    }
                    problemObservation.entryRelationship = new List<EntryRelationship>();
                    var entryProblemStatus = new EntryRelationship();
                    problemObservation.entryRelationship.Add(entryProblemStatus);
                    var problemStatusObs = new Observation();
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
                    var status = MDVUtility.ToStr(problem["Status"]);
                    string code;

                    switch (status)
                    {
                        case "Resolved":
                            code = "413322009";
                            break;
                        case "Inactive":
                            code = "73425007";
                            break;
                        default:
                            status = "Active";
                            code = "55561003";
                            break;
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
                if (lstProblemList == null || lstProblemList.Count == 0)
                {
                    text.ID = "noproblem";
                    text.Text = new List<string> { "Not included in document" };
                }
                problemListComponent.section.entry = new List<Entry>();
                var problemEntry = new Entry { typeCode = x_ActRelationshipEntry.DRIV };
                var concernAct = new Act
                {
                    classCode = x_ActClassDocumentEntryAct.ACT,
                    moodCode = x_DocumentActMood.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.3",extension="2015-08-01"},
                        new II {root = "2.16.840.1.113883.10.20.22.4.3"}
                    },
                    id = new List<II>
                    {
                        new II {root = StaticCcdaData["UniqueIdentifierID"]}
                    },
                    code = new CD
                    {
                        code = "CONC",
                        codeSystem = "2.16.840.1.113883.5.6",
                        displayName = "Concern",
                    },
                    statusCode = new CS
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[] { new IVXB_TS { nullFlavor = "NI" }, new IVXB_TS { nullFlavor = "NI" } }
                    },
                    entryRelationship = new List<EntryRelationship>()
                };
                var problemEntryRelationship = new EntryRelationship
                {
                    typeCode = x_ActRelationshipEntryRelationship.SUBJ
                };
                var problemObservation = new Observation();
                problemEntryRelationship.Item = problemObservation;

                problemObservation.classCode = "OBS";
                problemObservation.moodCode = x_ActMoodDocumentObservation.EVN;

                problemObservation.nullFlavor = "NI";

                problemObservation.templateId = new List<II>
                {
                    new II { root = "2.16.840.1.113883.10.20.22.4.4",extension="2015-08-01"},
                    new II { root = "2.16.840.1.113883.10.20.22.4.4"}
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
                problemObservation.value = new List<ANY>
                {
                    new CD
                    {
                        nullFlavor = "NI"
                    }
                };
                concernAct.entryRelationship.Add(problemEntryRelationship);
                problemEntry.Item = concernAct;
                problemListComponent.section.entry.Add(problemEntry);
            }
            sb.component.Add(problemListComponent);
        }

        private static void AddVitalSignsComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var vitalSignsComponent = HelperMethodsCaseReports.IntializeComponent3(
                new List<string> { "2.16.840.1.113883.10.20.22.2.4.1", "2.16.840.1.113883.10.20.22.2.4.1" },
                "8716-3", "2.16.840.1.113883.6.1", "LOINC", "Vital Signs", "VITAL SIGNS");

            vitalSignsComponent.section.templateId[0].extension = "2015-08-01";
            var text = vitalSignsComponent.section.text;
            var lstVitalSigns = objCcdaModel.lstVitals;
            if (lstVitalSigns != null && lstVitalSigns.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);
                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                var headerTr = new StrucDocTr
                {
                    Items = new List<object>
                    {
                        new StrucDocTh
                        {
                            Text = new List<string> {"Date / Time"},
                            align = StrucDocThAlign.right,
                            alignSpecified = true
                        }
                    }
                };
                table.thead.tr.Add(headerTr);

                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);
                tbody.tr = new List<StrucDocTr>();

                var bodyTr = new StrucDocTr
                {
                    Items = new List<object>
                    {
                        new StrucDocTh
                        {
                            Text = new List<string> {"Height"},
                            align = StrucDocThAlign.left,
                            alignSpecified = true
                        }
                    }
                };
                tbody.tr.Add(bodyTr);
                bodyTr = new StrucDocTr
                {
                    Items = new List<object>
                    {
                        new StrucDocTh
                        {
                            Text = new List<string> {"Weight"},
                            align = StrucDocThAlign.left,
                            alignSpecified = true
                        }
                    }
                };
                tbody.tr.Add(bodyTr);

                bodyTr = new StrucDocTr
                {
                    Items = new List<object>
                    {
                        new StrucDocTh
                        {
                            Text = new List<string> {"Blood Pressure"},
                            align = StrucDocThAlign.left,
                            alignSpecified = true
                        }
                    }
                };
                tbody.tr.Add(bodyTr);
                bodyTr = new StrucDocTr
                {
                    Items = new List<object>
                    {
                        new StrucDocTh
                        {
                            Text = new List<string> {"Heart Rate"},
                            align = StrucDocThAlign.left,
                            alignSpecified = true
                        }
                    }
                };
                tbody.tr.Add(bodyTr);
                bodyTr = new StrucDocTr
                {
                    Items = new List<object>
                    {
                        new StrucDocTh
                        {
                            Text = new List<string> {"O2 Percentage BldC Oximetry"},
                            align = StrucDocThAlign.left,
                            alignSpecified = true
                        }
                    }
                };
                tbody.tr.Add(bodyTr);
                bodyTr = new StrucDocTr
                {
                    Items = new List<object>
                    {
                        new StrucDocTh
                        {
                            Text = new List<string> {"Body Temperature"},
                            align = StrucDocThAlign.left,
                            alignSpecified = true
                        }
                    }
                };
                tbody.tr.Add(bodyTr);
                bodyTr = new StrucDocTr
                {
                    Items = new List<object>
                    {
                        new StrucDocTh
                        {
                            Text = new List<string> {"Respiratory Rate"},
                            align = StrucDocThAlign.left,
                            alignSpecified = true
                        }
                    }
                };
                tbody.tr.Add(bodyTr);
                var id = 0;
                foreach (var dr in lstVitalSigns)
                {
                    var th = new StrucDocTh();
                    table.thead.tr[0].Items.Add(th);
                    var date = string.Format("{0:g}", Convert.ToDateTime(dr["createdOn"]));
                    if (!string.IsNullOrWhiteSpace(date))
                        th.Text = new List<string> { date };
                    var td = new StrucDocTd();
                    table.tbody[0].tr[0].Items.Add(td);
                    var height = dr["Height"];
                    if (height == "0" || height == "")
                        height = string.Empty;
                    else
                        height = HelperMethodsCaseReports.ConvertFeetToInch(height);
                    td.Items = new List<object>
                    {
                        new StrucDocContent
                        {
                            ID = string.Concat("vit", ++id),
                            Text = new List<string> { height + (height == "" ? "" : " cm") }
                        }
                    };
                    td = new StrucDocTd();
                    table.tbody[0].tr[1].Items.Add(td);
                    var weight = dr["Weight"];
                    if (weight == "0" || weight == "")
                        weight = string.Empty;
                    td.Items = new List<object>
                    {
                        new StrucDocContent
                        {
                            ID = string.Concat("vit", ++id),
                            Text = new List<string> {  weight + (weight == "" ? "" : " kg") }
                        }
                    };
                    td = new StrucDocTd();
                    table.tbody[0].tr[2].Items.Add(td);
                    var sbp = dr["BPSystolic"];
                    var dbp = dr["BPDiastolic"];

                    if (!string.IsNullOrWhiteSpace(sbp) && !string.IsNullOrWhiteSpace(sbp))
                    {
                        var bloodPres = sbp + "/" + dbp + " mm[Hg]";

                        td.Items = new List<object>
                        {
                            new StrucDocContent
                            {
                                ID = string.Concat("vit", ++id),
                                Text = new List<string> { bloodPres }
                            }
                        };
                    }
                    td = new StrucDocTd();
                    table.tbody[0].tr[3].Items.Add(td);
                    var Pulse = dr["Pulse"];
                    if (!string.IsNullOrWhiteSpace(Pulse))
                    {
                        Pulse = Pulse + " beats per minute";


                        td.Items = new List<object>
                        {
                            new StrucDocContent
                            {
                                ID = string.Concat("vit", ++id),
                                Text = new List<string> { Pulse }
                            }
                        };
                    }
                    td = new StrucDocTd();
                    table.tbody[0].tr[4].Items.Add(td);
                    var SPO2 = dr["SPO2"];

                    if (!string.IsNullOrWhiteSpace(SPO2))
                    {
                        SPO2 = SPO2 + " percent";
                        td.Items = new List<object>
                        {
                            new StrucDocContent
                            {
                                ID = string.Concat("vit", ++id),
                                Text = new List<string> { SPO2 }
                            }
                        };
                    }
                    td = new StrucDocTd();
                    table.tbody[0].tr[5].Items.Add(td);
                    var Temprature = dr["Temprature"];

                    if (!string.IsNullOrWhiteSpace(Temprature))
                    {
                        Temprature = HelperMethodsCaseReports.ConvertTempFtoC(Temprature) + " degree celsius";
                        td.Items = new List<object>
                        {
                            new StrucDocContent
                            {
                                ID = string.Concat("vit", ++id),
                                Text = new List<string> { Temprature }
                            }
                        };
                    }
                    td = new StrucDocTd();
                    table.tbody[0].tr[6].Items.Add(td);
                    var RespiratoryRate = dr["RespiratoryRate"];
                    if (!string.IsNullOrWhiteSpace(dbp))
                    {
                        RespiratoryRate = RespiratoryRate + " beats per minute";


                        td.Items = new List<object>
                        {
                            new StrucDocContent
                            {
                                ID = string.Concat("vit", ++id),
                                Text = new List<string> { RespiratoryRate }
                            }
                        };
                    }
                }
                vitalSignsComponent.section.entry = new List<Entry>();
                id = 0;
                foreach (var dr in lstVitalSigns)
                {
                    var vitalSignsOrganizerEntry = new Entry { typeCode = x_ActRelationshipEntry.DRIV };

                    var organizer = new Organizer();
                    vitalSignsOrganizerEntry.Item = organizer;
                    organizer.classCode = x_ActClassDocumentEntryOrganizer.CLUSTER;
                    organizer.moodCode = "EVN";

                    organizer.templateId = new List<II>
                    {
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.22.4.26"
                        }
                    };
                    organizer.id = new List<II>
                    {
                        new II
                        {
                            root = StaticCcdaData["UniqueIdentifierID"]
                        }
                    };
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
                    organizer.effectiveTime = new IVL_TS
                    {
                        value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd")
                    };
                    organizer.component = new List<Component4>();
                    var height = dr["Height"];
                    if (height == "0" || height == "")
                        height = "UNK";
                    var vitalSignObservationComponent = new Component4();
                    var vitalSignObservation = new Observation();
                    vitalSignObservationComponent.Item = vitalSignObservation;
                    vitalSignObservation.classCode = "OBS";
                    vitalSignObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    vitalSignObservation.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.27",extension= "2014-06-09" },
                        new II { root = "2.16.840.1.113883.10.20.22.4.27"}
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
                                value = string.Concat("#vit", ++id)
                            }
                        };
                    }
                    vitalSignObservation.statusCode = new CS { code = "completed" };
                    vitalSignObservation.effectiveTime = new IVL_TS
                    {
                        value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd")
                    };

                    if (height != "UNK")
                    {
                        vitalSignObservation.value = new List<ANY>
                        {
                            new PQ
                            {
                                value = HelperMethodsCaseReports.ConvertFeetToInch(height),
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
                    vitalSignObservationComponent = new Component4();
                    vitalSignObservation = new Observation();
                    vitalSignObservationComponent.Item = vitalSignObservation;
                    vitalSignObservation.classCode = "OBS";
                    vitalSignObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    vitalSignObservation.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.27",extension= "2014-06-09" },
                        new II { root = "2.16.840.1.113883.10.20.22.4.27"}
                    };
                    vitalSignObservation.id = new List<II>
                    {
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };
                    vitalSignObservation.code = new CD
                    {
                        code = "29463-7",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "Patient Body Weight"
                    };
                    var weight = dr["Weight"];
                    if (weight == "0" || weight == "")
                        weight = "UNK";
                    if (weight != "UNK")
                    {
                        vitalSignObservation.text = new ED
                        {
                            reference = new TEL
                            {
                                value = string.Concat("#vit", ++id)
                            }
                        };
                    }
                    vitalSignObservation.statusCode = new CS { code = "completed" };
                    vitalSignObservation.effectiveTime = new IVL_TS
                    {
                        value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd")
                    };
                    if (weight != "UNK")
                    {
                        vitalSignObservation.value = new List<ANY>
                        {
                            new PQ
                            {
                                value =HelperMethodsCaseReports.ConvertlbsToKG(weight),
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
                        new II { root = "2.16.840.1.113883.10.20.22.4.27",extension= "2014-06-09" },
                        new II { root = "2.16.840.1.113883.10.20.22.4.27"}
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
                        displayName = "Intravascular Systolic"
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
                                value = string.Concat("#vit", ++id)
                            }
                        };
                    }
                    vitalSignObservation.statusCode = new CS { code = "completed" };
                    vitalSignObservation.effectiveTime = new IVL_TS
                    {
                        value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd")
                    };
                    if (sbp != "UNK")
                    {
                        vitalSignObservation.value = new List<ANY>
                        {
                            new PQ
                            {
                                value = sbp,
                                unit = "mm[Hg]"
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
                        new II { root = "2.16.840.1.113883.10.20.22.4.27",extension= "2014-06-09" },
                        new II { root = "2.16.840.1.113883.10.20.22.4.27"}
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
                                value = string.Concat("#vit", ++id)
                            }
                        };
                        vitalSignObservation.value = new List<ANY>
                        {
                            new PQ
                            {
                                value = dbp,
                                unit = "mm[Hg]"
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
                    vitalSignObservation.effectiveTime = new IVL_TS
                    {
                        value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd")
                    };
                    organizer.component.Add(vitalSignObservationComponent);

                    #region Heart Rate
                    vitalSignObservationComponent = new Component4();
                    vitalSignObservation = new Observation();
                    vitalSignObservationComponent.Item = vitalSignObservation;

                    vitalSignObservation.classCode = "OBS";
                    vitalSignObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    vitalSignObservation.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.27",extension= "2014-06-09" },
                        new II { root = "2.16.840.1.113883.10.20.22.4.27"}
                    };

                    vitalSignObservation.id = new List<II>
                    {
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };

                    vitalSignObservation.code = new CD
                    {
                        code = "8867-4",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "Heart Rate"
                    };

                    var pluse = dr["Pulse"];
                    if (pluse == "0" || pluse == "")
                        pluse = "UNK";
                    if (pluse != "UNK")
                    {
                        vitalSignObservation.text = new ED
                        {
                            reference = new TEL
                            {
                                value = string.Concat("#vit", ++id)
                            }
                        };
                    }
                    vitalSignObservation.statusCode = new CS { code = "completed" };
                    vitalSignObservation.effectiveTime = new IVL_TS
                    {
                        value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd")
                    };
                    if (pluse != "UNK")
                    {
                        vitalSignObservation.value = new List<ANY>
                        {
                            new PQ
                            {
                                value = pluse,
                                unit = "/min"
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
                    #endregion Hear Rate

                    #region SPO2
                    vitalSignObservationComponent = new Component4();
                    vitalSignObservation = new Observation();
                    vitalSignObservationComponent.Item = vitalSignObservation;

                    vitalSignObservation.classCode = "OBS";
                    vitalSignObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    vitalSignObservation.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.27",extension= "2014-06-09" },
                        new II { root = "2.16.840.1.113883.10.20.22.4.27"}
                    };

                    vitalSignObservation.id = new List<II>
                    {
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };

                    vitalSignObservation.code = new CD
                    {
                        code = "59408-5",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "O2 % BldC Oximetry"
                    };

                    var spO2 = dr["SPO2"];
                    if (spO2 == "0" || spO2 == "")
                        spO2 = "UNK";
                    if (spO2 != "UNK")
                    {
                        vitalSignObservation.text = new ED
                        {
                            reference = new TEL
                            {
                                value = string.Concat("#vit", ++id)
                            }
                        };
                    }
                    vitalSignObservation.statusCode = new CS { code = "completed" };
                    vitalSignObservation.effectiveTime = new IVL_TS
                    {
                        value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd")
                    };
                    if (spO2 != "UNK")
                    {
                        vitalSignObservation.value = new List<ANY>
                        {
                            new PQ
                            {
                                value = spO2,
                                unit = "%"
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
                    #endregion Hear Rate

                    #region Body Temprature
                    vitalSignObservationComponent = new Component4();
                    vitalSignObservation = new Observation();
                    vitalSignObservationComponent.Item = vitalSignObservation;

                    vitalSignObservation.classCode = "OBS";
                    vitalSignObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    vitalSignObservation.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.27",extension= "2014-06-09" },
                        new II { root = "2.16.840.1.113883.10.20.22.4.27"}
                    };

                    vitalSignObservation.id = new List<II>
                    {
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };

                    vitalSignObservation.code = new CD
                    {
                        code = "8310-5",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "Body Temperature"
                    };

                    var temp = dr["Temprature"];
                    if (temp == "0" || temp == "")
                        temp = "UNK";
                    if (temp != "UNK")
                    {
                        vitalSignObservation.text = new ED
                        {
                            reference = new TEL
                            {
                                value = string.Concat("#vit", ++id)
                            }
                        };
                    }
                    vitalSignObservation.statusCode = new CS { code = "completed" };
                    vitalSignObservation.effectiveTime = new IVL_TS
                    {
                        value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd")
                    };
                    if (temp != "UNK")
                    {
                        vitalSignObservation.value = new List<ANY>
                        {
                            new PQ
                            {
                                value =HelperMethodsCaseReports.ConvertTempFtoC( temp),
                                unit = "Cel"
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
                    #endregion Hear Rate

                    #region Respiratory Rate
                    vitalSignObservationComponent = new Component4();
                    vitalSignObservation = new Observation();
                    vitalSignObservationComponent.Item = vitalSignObservation;

                    vitalSignObservation.classCode = "OBS";
                    vitalSignObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    vitalSignObservation.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.27",extension= "2014-06-09" },
                        new II { root = "2.16.840.1.113883.10.20.22.4.27"}
                    };

                    vitalSignObservation.id = new List<II>
                    {
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };

                    vitalSignObservation.code = new CD
                    {
                        code = "9279-1",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "Respiratory Rate"
                    };

                    var respR = dr["RespiratoryRate"];
                    if (respR == "0" || respR == "")
                        respR = "UNK";
                    if (respR != "UNK")
                    {
                        vitalSignObservation.text = new ED
                        {
                            reference = new TEL
                            {
                                value = string.Concat("#vit", ++id)
                            }
                        };
                    }
                    vitalSignObservation.statusCode = new CS { code = "completed" };
                    vitalSignObservation.effectiveTime = new IVL_TS
                    {
                        value = Convert.ToDateTime(dr["createdOn"]).ToString("yyyyMMdd")
                    };
                    if (respR != "UNK")
                    {
                        vitalSignObservation.value = new List<ANY>
                        {
                            new PQ
                            {
                                value = respR,
                                unit = "/min"
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
                    #endregion Hear Rate
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
                    new II { root = "2.16.840.1.113883.10.20.22.4.27",extension= "2014-06-09" },
                    new II { root = "2.16.840.1.113883.10.20.22.4.27"}
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

        private static void AddAllergiesComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            Component3 medicationAllergiesComponent = HelperMethodsCaseReports.IntializeComponent3
            (new List<string> { "2.16.840.1.113883.10.20.22.2.6.1", "2.16.840.1.113883.10.20.22.2.6.1" },
               "48765-2", "2.16.840.1.113883.6.1", "LOINC", "Allergies, adverse reactions, alerts", "ALLERGIES");
            medicationAllergiesComponent.section.templateId[0].extension = "2015-08-01";
            var text = medicationAllergiesComponent.section.text;
            var lstAllergies = objCcdaModel.lstAllergs;
            if (lstAllergies != null && lstAllergies.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);
                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };

                var headerTr = new StrucDocTr();
                table.thead.tr.Add(headerTr);
                headerTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Substance"}},
                    new StrucDocTh {Text = new List<string> {"Reaction"}},
                    new StrucDocTh {Text = new List<string> {"Severity"}},
                    new StrucDocTh {Text = new List<string> {"Status"}}
                };
                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);
                tbody.tr = new List<StrucDocTr>();
                var counter = lstAllergies.Count;
                for (int i = 0; i < counter; i++)
                {
                    var bodyTr = new StrucDocTr
                    {
                        Items = new List<object>
                        {
                            new StrucDocTd {Text = new List<string> {lstAllergies[i]["Substance"]}},
                            new StrucDocTd {Text = new List<string> {lstAllergies[i]["Reaction"]}},
                            new StrucDocTd {Text = new List<string> {lstAllergies[i]["Severity"]}},
                            new StrucDocTd {Text = new List<string> {lstAllergies[i]["Status"]}}
                        }
                    };
                    tbody.tr.Add(bodyTr);
                }
                medicationAllergiesComponent.section.entry = new List<Entry>();
                for (int i = 0; i < counter; i++)
                {
                    var allergyProblemActEntry = new Entry { typeCode = x_ActRelationshipEntry.DRIV };
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
                    var status = lstAllergies[i]["Status"].ToLower() == "active";
                    if (status)
                        allergyProblemAct.statusCode = new CS
                        {
                            code = "active"
                        };
                    else
                        allergyProblemAct.statusCode = new CS
                        {
                            code = "completed"
                        };
                    var lowTime = Convert.ToDateTime(lstAllergies[i]["CreatedOn"]);
                    var highTime = Convert.ToDateTime(lstAllergies[i]["ResolvedDate"]);

                    allergyProblemAct.effectiveTime = new IVL_TS();
                    if (status && lowTime != DateTime.MinValue)
                    {
                        allergyProblemAct.effectiveTime.ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low
                        };

                        allergyProblemAct.effectiveTime.Items = new QTY[]
                        {
                            new IVXB_TS { value = lowTime.ToString("yyyyMMdd") }
                        };
                    }
                    if (!status && highTime != DateTime.MinValue)
                    {
                        allergyProblemAct.effectiveTime.ItemsElementName = new[]
                        {
                            ItemsChoiceType2.high
                        };

                        allergyProblemAct.effectiveTime.Items = new QTY[]
                        {
                            new IVXB_TS { value = highTime.ToString("yyyyMMdd") }
                        };
                    }
                    allergyProblemAct.entryRelationship = new List<EntryRelationship>();
                    var allergyIntoleranceOnservationEntry = new EntryRelationship
                    {
                        typeCode = x_ActRelationshipEntryRelationship.SUBJ
                    };
                    var allergyIntoleranceObservation = new Observation
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>
                        {
                            new II {root = "2.16.840.1.113883.10.20.22.4.7"}
                        },
                        id = new List<II>
                        {
                            new II {root = StaticCcdaData["UniqueIdentifierID"]}
                        },
                        code = new CD
                        {
                            code = "ASSERTION",
                            codeSystem = "2.16.840.1.113883.5.4",
                        },
                        statusCode = new CS
                        {
                            code = "completed",
                        },
                        effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[]
                            {
                                ItemsChoiceType2.low,
                                ItemsChoiceType2.high
                            },
                            Items = new QTY[2]
                        }
                    };
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
                    var allergyType = new CD
                    {
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED CT",
                        code = "416098002",
                        displayName = "Drug allergy (disorder)"
                    };
                    allergyIntoleranceObservation.value.Add(allergyType);
                    if (!string.IsNullOrWhiteSpace(lstAllergies[i]["Allergen"])
                        && !string.IsNullOrWhiteSpace(MDVUtility.ToStr(lstAllergies[i]["RxNormID"])))
                    {
                        allergyIntoleranceObservation.participant = new List<Participant2>();

                        var participant = new Participant2
                        {
                            typeCode = "CSM",
                            participantRole = new ParticipantRole { classCode = "MANU" }
                        };
                        var playingEntity = new PlayingEntity()
                        {
                            classCode = "MMAT",
                            code = new CE()
                            {
                                code = MDVUtility.ToStr(lstAllergies[i]["RxNormID"]),
                                displayName = lstAllergies[i]["Allergen"],
                                codeSystem = "2.16.840.1.113883.6.88",
                                codeSystemName = "RxNorm",
                            }
                        };
                        participant.participantRole.Item = playingEntity;
                        allergyIntoleranceObservation.participant.Add(participant);
                    }
                    allergyIntoleranceObservation.entryRelationship = new List<EntryRelationship>();
                    var allergyStatusRelationship = new EntryRelationship
                    {
                        typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                        inversionInd = true,
                        inversionIndSpecified = true
                    };
                    var allergyStatusObservation = new Observation
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>()
                        {
                            new II {root = "2.16.840.1.113883.10.20.22.4.28"}
                        },
                        code = new CD
                        {
                            code = "33999-4",
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC",
                            displayName = "Status"
                        },
                        statusCode = new CS
                        {
                            code = "completed"
                        },
                        value = new List<ANY>()
                    };
                    var allergyStatusValue = new CE();
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
                    if (!string.IsNullOrWhiteSpace(lstAllergies[i]["Reaction"]))
                    {
                        var allergyReactionRelationship = new EntryRelationship
                        {
                            typeCode = x_ActRelationshipEntryRelationship.MFST,
                            inversionInd = true,
                            inversionIndSpecified = true
                        };
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
                        allergyReactionObservation.statusCode = new CS
                        {
                            code = "completed",
                        };
                        if (!status)
                        {
                            allergyReactionObservation.effectiveTime = new IVL_TS
                            {
                                ItemsElementName = new[]
                                {
                                    ItemsChoiceType2.low,
                                    ItemsChoiceType2.high
                                },
                                Items = new QTY[2]
                            };
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
                                displayName =  "Drug allergy (disorder)",
                                codeSystem = "2.16.840.1.113883.6.96",
                                codeSystemName = "SNOMED CT"
                            }
                        };
                        if (!string.IsNullOrWhiteSpace(lstAllergies[i]["Severity"]))
                        {
                            allergyReactionObservation.entryRelationship = new List<EntryRelationship>();

                            var allergyReactionSeverityRelationship = new EntryRelationship
                            {
                                typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                                inversionInd = true,
                                inversionIndSpecified = true
                            };
                            var allergyReactionSeverityObservation = new Observation();
                            allergyReactionSeverityRelationship.Item = allergyReactionSeverityObservation;
                            allergyReactionSeverityObservation.classCode = "OBS";
                            allergyReactionSeverityObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                            allergyReactionSeverityObservation.templateId = new List<II>()
                            {
                                new II { root = "2.16.840.1.113883.10.20.22.4.8",extension="2014-06-09" },
                                new II { root = "2.16.840.1.113883.10.20.22.4.8"}
                            };
                            allergyReactionSeverityObservation.code = new CD
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
                            var allergyReaction = new CD
                            {
                                codeSystem = "2.16.840.1.113883.6.96",
                                codeSystemName = "SNOMED CT"
                            };
                            switch (lstAllergies[i]["Severity"].ToLower())
                            {
                                case "mild":
                                    allergyReaction.code = "255604002";
                                    allergyReaction.displayName = "Mild";
                                    break;
                                case "moderate":
                                    allergyReaction.code = "6736007";
                                    allergyReaction.displayName = "Moderate";
                                    break;
                                case "sever":
                                    allergyReaction.code = "24484000 ";
                                    allergyReaction.displayName = "Severe";
                                    break;
                                default:
                                    allergyReaction = new CE { nullFlavor = "UNK" };
                                    break;
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
                var allergyProblemActEntry = new Entry { typeCode = x_ActRelationshipEntry.DRIV };
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
                    ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
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
        private static List<StrucDocCol> getCols(int n)
        {
            var list = new List<StrucDocCol>();
            for (int i = 0; i < n; i++)
            {
                list.Add(new StrucDocCol { width = "10%" });
            }
            return list;
        }
        private static void AddSocialHistoryComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var socialHistoryComponent = HelperMethods.IntializeComponent3(
             new List<string> { "2.16.840.1.113883.10.20.22.2.17" },
             "29762-2", "2.16.840.1.113883.6.1", "LOINC", "Social History", "SOCIAL HISTORY");

            socialHistoryComponent.section.templateId[0].extension = "2015-08-01";
            var text = socialHistoryComponent.section.text;
            socialHistoryComponent.section.entry = new List<Entry>();
            var lstSocials = objCcdaModel.lstSocials;
            var lstSocialOccupations = objCcdaModel.lstSocialOccupations;
            var lstSocialTravels = objCcdaModel.lstSocialTravels;
            var lstSocialSexuals = objCcdaModel.lstSocialSexuals;
            var patientData = objCcdaModel.lstPatientData[0];
            if ((lstSocials != null && lstSocials.Count > 0) || (lstSocialOccupations.Count > 0 || lstSocialTravels.Count > 0 || lstSocialSexuals.Count > 0))
            {

                //var description = lstSocials[0]["Description"];
                //bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { description } });

                //var effectiveDate = lstSocials[0]["SocialHxDate"];
                //if (!string.IsNullOrWhiteSpace(effectiveDate))
                //{
                //    effectiveDate = Convert.ToDateTime(effectiveDate).ToString("MM/dd/yyyy");
                //}

                //bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { effectiveDate } });



                //Added by Rizwan
                if (!string.IsNullOrWhiteSpace(patientData["BirthSex"]))
                {
                    var table = new StrucDocTable();
                    table.Items = new List<object>(getCols(9));
                    text.Items.Add(table);
                    table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                    var headerTr = new StrucDocTr();
                    table.thead.tr.Add(headerTr);
                    headerTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Birth Sex"}},
                    new StrucDocTh {Text = new List<string> {"Value"}},
                    new StrucDocTh {Text = new List<string> {"Date"}}
                };

                    table.tbody = new List<StrucDocTbody>();
                    var tbody = new StrucDocTbody();
                    table.tbody.Add(tbody);

                    tbody.tr = new List<StrucDocTr>();


                    var BithsexTr = new StrucDocTr();
                    BithsexTr.Items = new List<object>();
                    var birthSexTD = new StrucDocTd();
                    birthSexTD.Text = new List<string> { "Sex Assigned At Birth" };
                    birthSexTD.ID = "BirthSexInfo";

                    BithsexTr.Items.Add(birthSexTD);
                    BithsexTr.Items.Add(new StrucDocTd { Text = new List<string> { patientData["BirthSex"] } });
                    BithsexTr.Items.Add(new StrucDocTd { Text = new List<string> { patientData["PatientDOB"] } });


                    tbody.tr.Add(BithsexTr);
                }

                if (lstSocialOccupations.Count > 0)
                {


                    var table = new StrucDocTable();
                    text.Items.Add(table);
                    table.Items = new List<object>(getCols(9));
                    table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                    var headerTr = new StrucDocTr();
                    table.thead.tr.Add(headerTr);
                    headerTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Social History Observation Type"}},
                    new StrucDocTh {Text = new List<string> {"Value"}},
                    new StrucDocTh {Text = new List<string> {"Date(s)"}}
                };

                    table.tbody = new List<StrucDocTbody>();
                    var tbody = new StrucDocTbody();
                    table.tbody.Add(tbody);

                    tbody.tr = new List<StrucDocTr>();


                    foreach (var item in lstSocialOccupations)
                    {
                        var OccupTr = new StrucDocTr();
                        OccupTr.Items = new List<object>();
                        var OccupTD = new StrucDocTd();
                        OccupTD.Text = new List<string> { item["Type"] };
                        OccupTD.ID = "OccupationDetails";

                        OccupTr.Items.Add(OccupTD);
                        OccupTr.Items.Add(new StrucDocTd { Text = new List<string> { item["Details"] } });
                        if (item["StartDate"] != "")
                        {
                            OccupTr.Items.Add(new StrucDocTd
                            {
                                Text = new List<string> { Convert.ToDateTime(item["StartDate"]).ToString("MM/dd/yyyy")
                            }});
                        }
                        tbody.tr.Add(OccupTr);
                    }


                }

                if (lstSocialTravels.Count > 0)
                {


                    var table = new StrucDocTable();
                    text.Items.Add(table);
                    table.Items = new List<object>(getCols(9));
                    table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                    var headerTr = new StrucDocTr();
                    table.thead.tr.Add(headerTr);
                    headerTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Travel History: Date(s)"}},
                    new StrucDocTh {Text = new List<string> {"Notes"}},
                    new StrucDocTh {Text = new List<string> {"Location"}}
                };

                    table.tbody = new List<StrucDocTbody>();
                    var tbody = new StrucDocTbody();
                    table.tbody.Add(tbody);

                    tbody.tr = new List<StrucDocTr>();


                    foreach (var item in lstSocialTravels)
                    {
                        var TravelTr = new StrucDocTr();
                        TravelTr.Items = new List<object>();
                        var TravelTD = new StrucDocTd();
                        if (item["StartDate"] != "" && item["EndDate"] != "")
                        {
                            TravelTD.Text = new List<string> { Convert.ToDateTime(item["StartDate"]).ToString("MM/dd/yyyy") + " to " + Convert.ToDateTime(item["EndDate"]).ToString("MM/dd/yyyy") };
                        }
                        else if (item["StartDate"] != "")
                        {
                            TravelTD.Text = new List<string> { Convert.ToDateTime(item["StartDate"]).ToString("MM/dd/yyyy")  };
                        }
                        TravelTD.ID = "TravelationDetails";
                        TravelTr.Items.Add(TravelTD);
                        TravelTr.Items.Add(new StrucDocTd { Text = new List<string> { item["Comments"] } });
                        TravelTr.Items.Add(new StrucDocTd { Text = new List<string> { item["Location"] } });


                        tbody.tr.Add(TravelTr);
                    }


                }

                if (lstSocialSexuals.Count > 0)
                {


                    var table = new StrucDocTable();
                    text.Items.Add(table);
                    table.Items = new List<object>(getCols(9));
                    table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                    var headerTr = new StrucDocTr();
                    table.thead.tr.Add(headerTr);
                    headerTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Pregnancy Status"}},

                    new StrucDocTh {Text = new List<string> {"Date(s)"}}
                };

                    table.tbody = new List<StrucDocTbody>();
                    var tbody = new StrucDocTbody();
                    table.tbody.Add(tbody);

                    tbody.tr = new List<StrucDocTr>();


                    foreach (var item in lstSocialSexuals)
                    {
                        var SexualTr = new StrucDocTr();
                        SexualTr.Items = new List<object>();
                        var SexualTD = new StrucDocTd();
                        SexualTD.Items = new List<object>();
                        if (item["PregnancyStatus"].ToLower() == "true" || item["PregnancyStatus"].ToLower() == "1")
                        {

                            SexualTD.Items.Add(new StrucDocContent
                            {
                                //  styleCode = "Italics Bold",
                                Text = new List<string> { "Pregnant" }
                            });

                        }
                        else
                        {
                            SexualTD.Items.Add(new StrucDocContent
                            {
                                styleCode = "Italics Bold",
                                Text = new List<string> { "Not " }
                            });
                            SexualTD.Text = new List<string> { "Pregnant" };
                        }
                        SexualTD.ID = "PregnancyStatus";

                        SexualTr.Items.Add(SexualTD);

                        SexualTr.Items.Add(new StrucDocTd { Text = new List<string> { item["PregnancyDuration"] } });


                        tbody.tr.Add(SexualTr);
                    }


                }



                // XML Begins here
                if (lstSocials.Count > 0)
                {
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
                    new II { root = "2.16.840.1.113883.10.20.22.4.78" }
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
                    smokingStatusObservation.effectiveTime = new IVL_TS { value = DateTime.Now.ToString("yyyy") };
                    smokingStatusObservation.value = new List<ANY>();
                    var snomedid = lstSocials[0]["SNOMEDID"];
                    if (!string.IsNullOrWhiteSpace(snomedid))
                    {
                        smokingStatusObservation.value.Add(new CD
                        {
                            code = snomedid,
                            codeSystem = "2.16.840.1.113883.6.96",
                            displayName = MDVUtility.ToStr(lstSocials[0]["SmokingStatus"])
                        });
                    }
                    else
                    {
                        smokingStatusObservation.value.Add(new CD
                        {
                            code = "266927001",
                            codeSystem = "2.16.840.1.113883.6.96",
                            displayName = "Unknown if ever smoked"
                        });
                    }

                }

                //Added by Rizwan               
                if (!string.IsNullOrWhiteSpace(patientData["BirthSex"]))
                {
                    var birthSexEntry = new Entry();
                    socialHistoryComponent.section.entry.Add(birthSexEntry);
                    var birthSexObservation = new Observation();
                    birthSexEntry.Item = birthSexObservation;

                    birthSexObservation.classCode = "OBS";
                    birthSexObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    birthSexObservation.templateId = new List<II>()
                {
                    new II { root = "2.16.840.1.113883.10.20.22.4.200", extension = "2016-06-01" }
                };

                    // birthSexObservation.id = new List<II> { new II { root = StaticCcdaData["UniqueIdentifierID"] } };

                    birthSexObservation.code = new CD
                    {
                        code = "76689-9",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "Sex Assigned At Birth",
                        originalText = new ED { reference = new TEL { value = "#BirthSexInfo" } },
                    };

                    birthSexObservation.statusCode = new CS
                    {
                        code = "completed"
                    };
                    birthSexObservation.value = new List<ANY>()
                {
                    new CD{code=HelperMethods.GetGenderCode(patientData["BirthSex"]),codeSystem = "2.16.840.1.113883.5.1",displayName=patientData["BirthSex"]  }
                };
                }

                // Social Occupations

                foreach (var item in lstSocialOccupations)
                {
                    var OccupationsEntry = new Entry();
                    socialHistoryComponent.section.entry.Add(OccupationsEntry);
                    var OccupationsObservation = new Observation();
                    OccupationsEntry.Item = OccupationsObservation;

                    OccupationsObservation.classCode = "OBS";
                    OccupationsObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    OccupationsObservation.templateId = new List<II>()
                {
                    new II { root = "2.16.840.1.113883.10.20.22.4.38" }
                };
                    OccupationsObservation.templateId = new List<II>()
                {
                    new II {  root = "2.16.840.1.113883.10.20.22.4.38", extension = "2015-08-01" }
                };


                    OccupationsObservation.code = new CD
                    {
                        code = "364703007",
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED CT",
                        displayName = "Employment detail",
                        translation = new List<CD> { new CD { code = "11295-3",
                            codeSystem = "2.16.840.1.113883.6.1" , codeSystemName="LOINC",
                            displayName = "Current employment - Reported" } }

                    };

                    OccupationsObservation.statusCode = new CS
                    {
                        code = "completed"
                    };
                    var date = "";
                    if (!string.IsNullOrWhiteSpace(item["StartDate"]))
                    {
                        date = HelperMethods.SetDateTime(Convert.ToDateTime(item["StartDate"]));
                    }

                    if (!string.IsNullOrWhiteSpace(date))
                    {
                        OccupationsObservation.effectiveTime = new IVL_TS
                        {
                            value = date
                        };
                    }
                    else
                    {
                        OccupationsObservation.effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[]
                            {
                                ItemsChoiceType2.low,
                                ItemsChoiceType2.high
                            },
                            Items = new QTY[]
                            {
                                new IVXB_TS { nullFlavor = "UNK" },
                                new IVXB_TS { nullFlavor = "UNK" }
                            }
                        };
                    }
                    OccupationsObservation.value = new List<ANY>()
                {
                    new ST{ Text = new List<string> { item["Details"] } }
                };
                }

                // Travel Histories


                foreach (var item in lstSocialTravels)
                {
                    var TravelsEntry = new Entry();
                    socialHistoryComponent.section.entry.Add(TravelsEntry);
                    var TravelsAct = new Act();
                    TravelsEntry.Item = TravelsAct;

                    TravelsAct.classCode = x_ActClassDocumentEntryAct.ACT;
                    TravelsAct.moodCode = x_DocumentActMood.EVN;

                    TravelsAct.templateId = new List<II>()
                {
                    new II {  root = "2.16.840.1.113883.10.20.15.2.3.1", extension = "2016-12-01" }
                };


                    TravelsAct.code = new CD
                    {
                        code = "420008001",
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED CT",
                        displayName = "Travel"


                    };
                    TravelsAct.text = new ED { Text = new List<string> { item["Comments"] } };
                    TravelsAct.statusCode = new CS
                    {
                        code = "completed"
                    };
                    var date = "";
                    var dateEnd = "";
                    if (item["StartDate"] != "")
                    {
                        date = HelperMethods.SetDateTime(Convert.ToDateTime(item["StartDate"]));
                    }
                    if (item["EndDate"] != "")
                    {
                        dateEnd = HelperMethods.SetDateTime(Convert.ToDateTime(item["EndDate"]));
                    }

                    if (!string.IsNullOrWhiteSpace(date) || !string.IsNullOrWhiteSpace(dateEnd))
                    {
                        TravelsAct.effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[]
                            {
                                ItemsChoiceType2.low,
                                ItemsChoiceType2.high
                            },
                            Items = new QTY[]
                            {
                                new IVXB_TS { value = date },
                                new IVXB_TS { value = dateEnd }
                            }
                        };
                    }
                    else
                    {
                        TravelsAct.effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[]
                            {
                                ItemsChoiceType2.low,
                                ItemsChoiceType2.high
                            },
                            Items = new QTY[]
                            {
                                new IVXB_TS { nullFlavor = "UNK" },
                                new IVXB_TS { nullFlavor = "UNK" }
                            }
                        };
                    }
                    if (item["Location"] != "")
                    {
                        var address = new AD();


                        address.Items = new List<ADXP>();
                        address.Items.Add(new adxpstreetAddressLine { Text = new List<string> { item["Location"] } });



                        TravelsAct.participant = new List<Participant2> {
                        new Participant2
                        {
                            typeCode="LOC",
                            participantRole= new ParticipantRole
                            {
                                classCode="TERR",
                                templateId=new List<II>
                                {
                                    new II
                                    {
                                        root = "2.16.840.1.113883.10.20.22.4.32"
                                    }
                                },

                               addr = new List<AD>
                                {
                                 address
                                }
                            }
                        }
                    };

                    }
                }


                // Pregnancy Status

                foreach (var item in lstSocialSexuals)
                {
                    var SexualEntry = new Entry();
                    socialHistoryComponent.section.entry.Add(SexualEntry);
                    var SexualObservation = new Observation();
                    SexualEntry.Item = SexualObservation;

                    SexualObservation.classCode = "OBS";
                    SexualObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                    if (item["PregnancyStatus"].ToLower() == "false" || item["PregnancyStatus"].ToLower() == "0")
                    {
                        SexualObservation.negationInd = false;
                    }
                    else
                    {
                        SexualObservation.negationInd = true;
                    }
                    SexualObservation.templateId = new List<II>()
                    {
                        new II { root = "2.16.840.1.113883.10.20.15.3.8" }
                    };



                    SexualObservation.code = new CD
                    {
                        code = "ASSERTION",
                        codeSystem = "2.16.840.1.113883.5.4"

                    };

                    SexualObservation.statusCode = new CS
                    {
                        code = "completed"
                    };
                    var date = "";
                    try
                    {
                        HelperMethods.SetDateTime(Convert.ToDateTime(item["StartDate"]));
                    }
                    catch (Exception e)
                    {
                        date = "";
                    }
                    if (!string.IsNullOrWhiteSpace(date))
                    {
                        SexualObservation.effectiveTime = new IVL_TS
                        {
                            value = item["PregnancyDuration"]
                        };
                    }
                    else
                    {
                        SexualObservation.effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[]
                            {
                                ItemsChoiceType2.low,
                                ItemsChoiceType2.high
                            },
                            Items = new QTY[]
                            {
                                new IVXB_TS { nullFlavor = "UNK" },
                                new IVXB_TS { nullFlavor = "UNK" }
                            }
                        };
                    }
                    SexualObservation.value = new List<ANY>()
                {
                    new CD{ code = "77386006", displayName = "Pregnant", codeSystem = "2.16.840.1.113883.6.96" }
                };
                }

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
                    new II { root = "2.16.840.1.113883.10.20.22.4.78" }
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
                smokingStatusObservation.effectiveTime = new IVL_TS { value = DateTime.Now.ToString("yyyy") };
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

        private static void AddMedicationComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var medicationComponent = HelperMethodsCaseReports.IntializeComponent3(
               new List<string> { "2.16.840.1.113883.10.20.22.2.1.1", "2.16.840.1.113883.10.20.22.2.1.1" },
               "10160-0", "2.16.840.1.113883.6.1", "LOINC", "History of medication use", "MEDICATIONS");
            medicationComponent.section.templateId[0].extension = "2014-06-09";
            StrucDocText text = medicationComponent.section.text;
            List<Dictionary<string, string>> lstMedication = objCcdaModel.lstMedication;
            if (lstMedication != null && lstMedication.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);
                table.Items = new List<object>(getCols(9));
                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                var headerTr = new StrucDocTr();
                table.thead.tr.Add(headerTr);
                headerTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Medication"}},
                    new StrucDocTh {Text = new List<string> {"Dose"}},
                    new StrucDocTh {Text = new List<string> {"Duration"}},
                    new StrucDocTh {Text = new List<string> {"Route"}},
                    //new StrucDocTh {Text = new List<string> {"Status"}},
                    //new StrucDocTh {Text = new List<string> {"Indications"}},
                    //new StrucDocTh {Text = new List<string> {"Fill Instructions"}}
                };
                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);
                tbody.tr = new List<StrucDocTr>();
                var id = 0;
                foreach (var medication in lstMedication)
                {
                    var bodyTr = new StrucDocTr { Items = new List<object>() };
                    var startDate = medication["StartDate"];
                    if (startDate != string.Empty)
                    {
                        startDate = Convert.ToDateTime(startDate).ToString("MM/dd/yyyy");
                    }
                    bodyTr.Items.Add(
                    new StrucDocTd
                    {
                        Items = new List<object>
                        {
                            new StrucDocContent
                            {
                               // ID = string.Concat("Medication_", ++id),
                                Text = new List<string> {medication["Medication"] }
                            }
                        }
                    });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["Dose"] } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { startDate } });
                    bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["RouteDescription"] } });
                    //bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["Status"] } });
                    //bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { medication["Indication"] } });
                    //string substituation = medication["Substitution"];

                    //bodyTr.Items.Add(substituation.ToLower() == "y"
                    //    ? new StrucDocTd { Text = new List<string> { "Generic Substitution Allowed" } }
                    //    : new StrucDocTd { Text = new List<string> { "Generic Substitution not Allowed" } });
                    tbody.tr.Add(bodyTr);
                }
                medicationComponent.section.entry = new List<Entry>();

                id = 0;
                foreach (Dictionary<string, string> medicationDictionary in lstMedication)
                {
                    var medicationEntry = new Entry
                    {
                        typeCode = x_ActRelationshipEntry.DRIV,
                        Item =
                            SetMedicationActivity(medicationDictionary, objCcdaModel.lstPracticeData[0]["ShortName"],
                                string.Concat("#Medication_", ++id))
                    };
                    medicationComponent.section.entry.Add(medicationEntry);
                }
            }
            else
            {
                var noMedicationText = new StrucDocContent();
                text.Items.Add(noMedicationText);
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
                    new II { root = "2.16.840.1.113883.10.20.22.4.16",extension= "2014-06-09" },
                    new II { root = "2.16.840.1.113883.10.20.22.4.16"}
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
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[] { new IVXB_TS { nullFlavor = "NI" }, new IVXB_TS { nullFlavor = "NI" } }
                    }
                };
                var consumable = new Consumable();
                medicationActivity.consumable = consumable;
                var manufacturedProduct = new ManufacturedProduct();
                consumable.manufacturedProduct = manufacturedProduct;
                manufacturedProduct.classCode = RoleClassManufacturedProduct.MANU;
                manufacturedProduct.classCodeSpecified = true;
                manufacturedProduct.templateId = new List<II>
                {
                    new II { root = "2.16.840.1.113883.10.20.22.4.23", extension = "2014-06-09" },
                    new II { root = "2.16.840.1.113883.10.20.22.4.23"}
                };
                var manufacturedMaterial = new Material();
                manufacturedProduct.Item = manufacturedMaterial;
                manufacturedMaterial.code = new CE
                {
                    nullFlavor = "NI"
                };
            }
            sb.component.Add(medicationComponent);
        }

        private static void AddClinicalInstructionsDecisionAids(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var instructionsComponent = HelperMethodsCaseReports.IntializeComponent3(
                new List<string> { "2.16.840.1.113883.10.20.22.2.45" },
                "69730-0", "2.16.840.1.113883.6.1", "LOINC", "Instructions", "INSTRUCTIONS");

            var text = instructionsComponent.section.text;
            var lstInstructionsAndDecisionAids = objCcdaModel.lstInstructionsAndDecisionAids;
            if (lstInstructionsAndDecisionAids != null && lstInstructionsAndDecisionAids.Count > 0)
            {
                StrucDocTable table = new StrucDocTable
                {
                    width = "100%",
                    border = "1"
                };
                text.Items.Add(table);
                table.thead = new StrucDocThead
                {
                    tr = new List<StrucDocTr>
                    {
                        new StrucDocTr
                        {
                            Items = new List<object>
                            {
                                new StrucDocTh
                                {
                                    colspan = "3",
                                    Text = new List<string> {"Clinical Instructions/Patient Decision Aids"}
                                }
                            }
                        }
                    }
                };
                table.tbody = new List<StrucDocTbody>();
                StrucDocTbody tBody = new StrucDocTbody();
                table.tbody.Add(tBody);
                tBody.tr = new List<StrucDocTr>();
                var id = 0;
                foreach (var dr in lstInstructionsAndDecisionAids)
                {
                    var tr = new StrucDocTr();
                    var ceatedOn = dr["CreatedOn"];
                    if (!string.IsNullOrWhiteSpace(ceatedOn))
                    {
                        ceatedOn = Convert.ToDateTime(ceatedOn).ToString("MM/dd/yyyy");
                    }

                    tr.Items = new List<object>
                    {
                        new StrucDocTd
                        {
                            ID = string.Concat("instruction", ++id),
                            Text = new List<string>{  MDVUtility.ToStr(dr["Instruction"]) }
                        },
                        new StrucDocTd{ Text = new List<string>{  MDVUtility.ToStr(dr["Type"]) }},
                        new StrucDocTd{ Text = new List<string>{ ceatedOn }}
                    };

                    tBody.tr.Add(tr);
                }

                id = 0;
                instructionsComponent.section.entry = new List<Entry>();

                foreach (var dr in lstInstructionsAndDecisionAids)
                {
                    var instructionsEntry = new Entry();
                    instructionsComponent.section.entry.Add(instructionsEntry);

                    var instructions = new Act();
                    instructionsEntry.Item = instructions;

                    instructions.classCode = x_ActClassDocumentEntryAct.ACT;
                    instructions.moodCode = x_DocumentActMood.INT;
                    instructions.templateId = new List<II>
                   {
                       new II {root = "2.16.840.1.113883.10.20.22.4.20"}
                   };

                    instructions.code = new CD
                    {
                        code = "311401005",
                        codeSystem = "2.16.840.1.113883.11.20.9.34",
                        displayName = "Patient Education"
                    };

                    instructions.text = new ED
                    {
                        reference = new TEL { value = string.Concat("#instruction", ++id) },
                        Text = new List<string> { MDVUtility.ToStr(dr["Instruction"]) }
                    };

                    instructions.statusCode = new CS
                    {
                        code = "completed"
                    };
                }
            }
            else
            {
                text.Items.Add(new StrucDocContent { ID = "noInstructions", Text = new List<string> { "Not included in document" } });
            }

            sb.component.Add(instructionsComponent);
        }

        private static void AddClinicalPrivacy(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var instructionsComponent = HelperMethodsCaseReports.IntializeComponent3(new List<string> { "2.16.840.1.113883.3.3251.1.5" }, "57017-6", "2.16.840.1.113883.6.1", "LOINC", "Privacy policy Organization", "Security and Privacy Prohibitions");
            #region Author

            instructionsComponent.section.text = new StrucDocText
            {
                Text = new List<string>
                {
                    "PROHIBITION ON REDISCLOSURE OF CONFIDENTIAL INFORMATION:", "This notice accompanies a disclosure of information concerning a client in alcohol/drug treatment, made to you with the consent of such client. This information has been disclosed to you from records protected by federal confidentiality rules (42 C.F.R. Part 2). The federal rules prohibit you from making any further disclosure of this information unless further disclosure is expressly permitted by the written consent of the person to whom it pertains or as otherwise permitted by 42 C.F.R. Part 2. A general authorization for the release of medical or other information is NOT sufficient for this purpose. The federal rules restrict any use of the information to criminally investigate or prosecute any alcohol or drug abuse patient."
                }
            };
            string confidentialCode = "N";
            if (objCcdaModel.IsConfidential == "true")
                confidentialCode = "R";
            instructionsComponent.section.confidentialityCode = new CE
            {
                code = confidentialCode,
                codeSystem = "2.16.840.1.113883.5.25",
                codeSystemName = "HL7 Confidentiality"
            };
            var objProvider = objCcdaModel.lstProviderData[0];
            var objPracticeData = objCcdaModel.lstPracticeData[0];
            instructionsComponent.section.author = new List<Author>
            {
                new Author
                {
                    templateId = new List<II>
                    {
                        new II
                        {
                            root = "2.16.840.1.113883.3.3251.1.6",
                            assigningAuthorityName = "HL7 Security"
                        }
                    },
                    time = new TS
                    {
                        value = DateTime.Now.ToString("yyyyMMdd")
                    },
                    assignedAuthor = new AssignedAuthor
                    {
                        templateId = new List<II>
                        {
                            new II
                            {
                                root ="2.16.840.1.113883.3.3251.1.7"
                            }
                        },
                        id = new List<II>
                        {
                            new II
                            {
                                root = Guid.NewGuid().ToString()
                            }
                        },
                        addr = new List<AD>
                        {
                            HelperMethodsCaseReports.SetAddress("WP", new List<string>() { objProvider["WorkAddress"] }, objProvider["City"], objProvider["State"], objProvider["ZIPCode"], "US")
                        },
                        telecom = new List<TEL>
                        {
                            HelperMethodsCaseReports.SetTelephone(objProvider["PhoneNo"], "WP",objProvider["PhoneExt"])
                        },
                        Item = new Person
                        {
                            name = new List<PN>
                            {
                                HelperMethodsCaseReports.SetName("L", objProvider["LastName"], new List<string> { objProvider["FirstName"], objProvider["MI"] })
                            }
                        },
                        representedOrganization = new Organization
                        {
                            name = new List<ON>
                            {
                               new ON
                               {
                                   Text = new List<string>
                                   {
                                       objCcdaModel.lstPracticeData[0]["ShortName"]
                                   }
                               }
                            },
                            telecom = new List<TEL>
                            {
                                HelperMethodsCaseReports.SetTelephone(objPracticeData["PhoneNo"], "WP",objPracticeData["PhoneExt"])
                            },
                            addr = new List<AD>
                            {
                                HelperMethodsCaseReports.SetAddress("WP", new List<string> { objPracticeData["Address"] }, objPracticeData["City"], objPracticeData["State"], objPracticeData["ZIPCode"], "US")
                            }
                        }
                    }
                }
            };

            #endregion

            #region Entry

            instructionsComponent.section.entry = new List<Entry>
            {
                new Entry
                {
                    typeCode = x_ActRelationshipEntry.COMP,
                    templateId = new List<II>
                    {
                        new II
                        {
                            root = "2.16.840.1.113883.3.3251.1.9",
                            assigningAuthorityName = "HL7 Security"
                        }
                    },
                    Item = new Organizer
                    {
                        classCode = x_ActClassDocumentEntryOrganizer.CLUSTER,
                        moodCode = "EVN",
                        templateId = new List<II>
                        {
                            new II
                            {
                                root = "2.16.840.1.113883.3.3251.1.4",
                                assigningAuthorityName = "HL7 Security"
                            }
                        },
                        statusCode = new CS
                        {
                            code = "active"
                        },
                        component = new List<Component4>
                        {
                            #region Component 1

                            new Component4
                            {
                                typeCode = ActRelationshipHasComponent.COMP,
                                Item = new Observation
                                {
                                    classCode = "OBS",
                                    moodCode = x_ActMoodDocumentObservation.EVN,
                                    templateId = new List<II>
                                    {
                                        new II
                                        {
                                            root = "2.16.840.1.113883.3.445.21",
                                            assigningAuthorityName = "HL7 CBCC"
                                        },
                                        new II
                                        {
                                             root = "2.16.840.1.113883.3.445.12",
                                            assigningAuthorityName = "HL7 CBCC"
                                        }
                                    },
                                    code = new CD
                                    {
                                        code = "SECCLASSOBS",
                                        codeSystem = "2.16.840.1.113883.1.11.20457",
                                        displayName = "Security Classification",
                                        codeSystemName = "HL7 SecurityObservationTypeCodeSystem"
                                    },
                                    value = new List<ANY>
                                    {
                                        new CD
                                        {
                                            code = "R",
                                            codeSystem = "2.16.840.1.113883.5.1063",
                                            codeSystemName = "SecurityObservationValueCodeSystem",
                                            displayName = "Restricted",
                                            originalText = new ED
                                            {
                                                Text = new List<string>
                                                {
                                                    "Restricted Confidentiality"
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            
	                        #endregion
                            #region Component 2

                            new Component4
                            {
                                typeCode = ActRelationshipHasComponent.COMP,
                                Item = new Observation
                                {
                                    classCode = "OBS",
                                    moodCode = x_ActMoodDocumentObservation.EVN,
                                    templateId = new List<II>
                                    {
                                        new II
                                        {
                                            root = "2.16.840.1.113883.3.445.21",
                                            assigningAuthorityName = "HL7 CBCC"
                                        },
                                        new II
                                        {
                                             root = "2.16.840.1.113883.3.445.14",
                                            assigningAuthorityName = "HL7 CBCC"
                                        },
                                    },
                                    code = new CD
                                    {
                                        code = "SECCONOBS",
                                        codeSystem = "2.16.840.1.113883.1.11.20457",
                                        displayName = "Security Classification",
                                        codeSystemName = "HL7 SecurityObservationTypeCodeSystem"
                                    },
                                    value = new List<ANY>
                                    {
                                        new CD
                                        {
                                            code = "ENCRYPT",
                                            codeSystem = "2.16.840.1.113883.5.1063",
                                            codeSystemName = "SecurityObservationValueCodeSystem",
                                            displayName = "Encrypt information",
                                            originalText = new ED
                                            {
                                                Text = new List<string>
                                                {
                                                    "Information must be encrypted"
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            
	                        #endregion
                            #region Component 3

                            new Component4
                            {
                                typeCode = ActRelationshipHasComponent.COMP,
                                Item = new Observation
                                {
                                    classCode = "OBS",
                                    moodCode = x_ActMoodDocumentObservation.EVN,
                                    templateId = new List<II>
                                    {
                                        new II
                                        {
                                            root = "2.16.840.1.113883.3.445.21",
                                            assigningAuthorityName = "HL7 CBCC"
                                        },
                                        new II
                                        {
                                             root = "2.16.840.1.113883.3.445.23",
                                            assigningAuthorityName = "HL7 CBCC"
                                        },
                                    },
                                    code = new CD
                                    {
                                        code = "SECCONOBS",
                                        codeSystem = "2.16.840.1.113883.1.11.20457",
                                        displayName = "Security Classification",
                                        codeSystemName = "HL7 SecurityObservationTypeCodeSystem"
                                    },
                                    value = new List<ANY>
                                    {
                                        new CD
                                        {
                                            code = "NORDSLCD",
                                            codeSystem = "2.16.840.1.113883.5.1063",
                                            codeSystemName = "SecurityObservationValueCodeSystem",
                                            displayName = "Prohibition on redisclosure without patient consent directive",
                                            originalText = new ED
                                            {
                                                Text = new List<string>
                                                {
                                                    "Prohibition on redisclosure without patient consent directive"
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            
	                        #endregion
                            #region Component 4

                            new Component4
                            {
                                typeCode = ActRelationshipHasComponent.COMP,
                                Item = new Observation
                                {
                                    classCode = "OBS",
                                    moodCode = x_ActMoodDocumentObservation.EVN,
                                    templateId = new List<II>
                                    {
                                        new II
                                        {
                                            root = "2.16.840.1.113883.3.445.21",
                                            assigningAuthorityName = "HL7 CBCC"
                                        },
                                        new II
                                        {
                                             root = "2.16.840.1.113883.3.445.22",
                                            assigningAuthorityName = "HL7 CBCC"
                                        },
                                    },
                                    code = new CD
                                    {
                                        code = "SECCONOBS",
                                        codeSystem = "2.16.840.1.113883.1.11.20457",
                                        displayName = "Security Classification",
                                        codeSystemName = "HL7 SecurityObservationTypeCodeSystem"
                                    },
                                    value = new List<ANY>
                                    {
                                        new CD
                                        {
                                            code = "TREAT",
                                            codeSystem = "2.16.840.1.113883.5.1063",
                                            codeSystemName = "SecurityObservationValueCodeSystem",
                                            displayName = "Treatment",
                                            originalText = new ED
                                            {
                                                Text = new List<string>
                                                {
                                                    "Information intended for treatment"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            
	                        #endregion
                        }
                    }
                }
            };

            #endregion


            sb.component.Add(instructionsComponent);
        }
        private static void AddCancerComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var instructionsComponent = HelperMethodsCaseReports.IntializeComponent3(new List<string> { "2.16.840.1.113883.10.13.2" }, "72135-7", "2.16.840.1.113883.6.1", "LOINC", "Cancer diagnosis", "Cancer Diagnosis");
            StrucDocTable table = new StrucDocTable
            {
                width = "100%",
                border = "1",
                Items = new List<object>()
            };
            instructionsComponent.section.text.Items.Add(table);
            var lstCancerDiseases = objCcdaModel.lstCognitiveFunctionalStatus;
            if (lstCancerDiseases != null && lstCancerDiseases.Count > 0)
            {
                #region TableHeader

                table.thead = new StrucDocThead
                {
                    tr = new List<StrucDocTr>
                    {
                        new StrucDocTr
                        {
                            Items = new List<object>
                            {
                                new StrucDocTh{ Text = new List<string>{ "Diagnosis Date" } },
                                new StrucDocTh{ Text = new List<string>{ "Primary Site" } },
                                new StrucDocTh{ Text = new List<string>{ "Laterality" } },
                                new StrucDocTh{ Text = new List<string>{ "Histology" } },
                                new StrucDocTh{ Text = new List<string>{ "Behavior" } },
                                new StrucDocTh{ Text = new List<string>{ "Diagnostic Confirmation" } },
                                new StrucDocTh{ Text = new List<string>{ "Grade" } },
                                new StrucDocTh{ Text = new List<string>{ "Stage" } },
                            }
                        }
                    }
                };

                #endregion

                #region TableBody

                table.tbody = new List<StrucDocTbody>();
                StrucDocTbody tBody = new StrucDocTbody();
                table.tbody.Add(tBody);
                tBody.tr = new List<StrucDocTr>();
                var i = 0;
                foreach (Dictionary<string, string> dr in lstCancerDiseases)
                {
                    i++;
                    var tr = new StrucDocTr { Items = new List<object>() };
                    var name = MDVUtility.ToStr(dr["Name"]);
                    var snomedid = MDVUtility.ToStr(dr["SNOMEDID"]);
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        name = name + ", [SNOMED-CT: " + snomedid + "]";
                        tr.Items.Add(new StrucDocTd { ID = "functionalstatus_" + i, Text = new List<string> { name } });
                    }
                    if (!string.IsNullOrWhiteSpace(snomedid))
                    {
                        snomedid = "[SNOMED-CT: " + snomedid + "]";
                        tr.Items.Add(new StrucDocTd { Text = new List<string> { snomedid } });
                    }
                    var effectiveDate = objCcdaModel.VisitDate;
                    if (!string.IsNullOrWhiteSpace(effectiveDate))
                    {
                        effectiveDate = Convert.ToDateTime(effectiveDate).ToString("MM/dd/yyyy");
                    }
                    tr.Items.Add(new StrucDocTd { Text = new List<string> { effectiveDate } });
                    tBody.tr.Add(tr);
                }

                #endregion

                instructionsComponent.section.entry = new List<Entry>
            {
                new Entry
                {
                    typeCode = x_ActRelationshipEntry.DRIV,
                    Item = new Act()
                    {
                        classCode = x_ActClassDocumentEntryAct.ACT,
                        moodCode = x_DocumentActMood.EVN,
                        templateId= new List<II>
                        {
                            new II
                            {
                                root = "2.16.840.1.113883.10.13.3",
                                extension = "2015-02-05"
                            },
                             new II
                            {
                                root = "2.16.840.1.113883.10.13.3",
                            }
                        },
                        id = new List<II>
                        {
                            new II
                            {
                                root = Guid.NewGuid().ToString()
                            }
                        },
                        code = new CD
                        {
                            code = "CONC",
                            codeSystem = "2.16.840.1.113883.5.6",
                            codeSystemName = "HL7ActClass",
                            displayName = "Concern"
                        },
                        statusCode = new CS
                        {
                            code = "active"
                        },
                        effectiveTime = new IVL_TS()
                        {
                            ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                            Items = new QTY[]
                            {
                                new IVXB_TS()
                                {
                                    value = DateTime.Now.ToString("yyyyMMdd") // The low value represents when the problem was first recorded in the patient's chart 
                                },
                                new IVXB_TS()
                                {
                                    nullFlavor = "NA"
                                }
                            }
                        },
                        entryRelationship = new List<EntryRelationship>
                        {
                            #region EntryRelationship
                            new EntryRelationship
                            {
                                typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                                Item = new Observation()
                                {
                                    classCode = "OBS",
                                    moodCode = x_ActMoodDocumentObservation.EVN,
                                    templateId= new List<II>
                                    {
                                        new II
                                        {
                                            root = "2.16.840.1.113883.10.13.4",
                                            extension = "2015-02-05"
                                        },
                                        new II
                                        {
                                            root = "2.16.840.1.113883.10.13.4",

                                        }
                                    },
                                    id = new List<II>
                                    {
                                        new II
                                        {
                                            root = Guid.NewGuid().ToString()
                                        }
                                    },
                                    code = new CD  // Get the Codes and Done with it
                                    {
                                        code = "29308-4",
                                        codeSystem = "2.16.840.1.113883.5.6",
                                        codeSystemName = "HL7ActClass",
                                        displayName = "Concern"
                                    },
                                    text = new ED
                                    {
                                        reference = new TEL
                                        {
                                            value = "#Diagnosis_1"
                                        }
                                    },
                                    statusCode = new CS
                                    {
                                        code = "active"
                                    },
                                    effectiveTime = new IVL_TS()
                                    {
                                        ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                        Items = new QTY[]
                                        {
                                            new IVXB_TS()
                                            {
                                                value = DateTime.Now.ToString("yyyyMMdd") // The low value represents when the problem was first recorded in the patient's chart 
                                            },
                                            new IVXB_TS()
                                            {
                                                nullFlavor = "NA"
                                            }
                                        }
                                    },
                                    value = new List<ANY>
                                    {
                                         new CD
                                         {
                                             code = "8140",
                                             codeSystem = "2.16.840.1.113883.6.43.1",
                                             codeSystemName = "ICD-O-3",
                                             displayName = "Adenocarcinoma, NOS",
                                             originalText = new ED
                                             {
                                                 reference = new TEL
                                                 {
                                                     value = "#Histology_1"
                                                 }
                                             },

                                             #region Qualifier's
                                             qualifier = new List<CR>
                                             {
                                                #region Qualifier1
                                                 new CR
                                                 {
                                                     name = new CV // Get and set the codes here
                                                     {
                                                         code = "31206-6",
                                                         codeSystem = "2.16.840.1.113883.6.1",
                                                         codeSystemName = "LOINC",
                                                         displayName = "Behavior ICD-O-3 Cancer"
                                                     },
                                                     value = new CD
                                                     {

                                                         code = "3",
                                                         codeSystem = "2.16.840.1.113883.3.520.3.14",
                                                         codeSystemName = "NAACCR Behavior Code",
                                                         displayName = "Malignant, Primary",
                                                         valueSet = "2.16.840.1.113883.3.520.4.14",
                                                         originalText =  new ED
                                                         {
                                                             reference = new TEL
                                                             {
                                                                 value = "#Behavior_1"
                                                             }
                                                         }
                                                     }
                                                 },
                                                 #endregion 
                                                #region Qualifier2
                                                 new CR
                                                 {
                                                     name = new CV // Get and set the codes here
                                                     {
                                                         code = "21858-6",
                                                         codeSystem = "2.16.840.1.113883.6.1",
                                                         codeSystemName = "LOINC",
                                                         displayName = "Grade Cancer"
                                                     },
                                                     value = new CD
                                                     {
                                                         code = "II",
                                                         codeSystem = "2.16.840.1.113883.3.520.3.15",
                                                         codeSystemName = "NAACCR Grade",
                                                         displayName = "Grade II",
                                                         valueSet = "2.16.840.1.113883.3.520.4.15",
                                                         originalText =  new ED
                                                         {
                                                             reference = new TEL
                                                             {
                                                                 value = "#Grade_1"
                                                             }
                                                         }
                                                     }
                                                 },
                                                 #endregion 
                                                #region Qualifier3
                                                 new CR
                                                 {
                                                     name = new CV() // Get and set the codes here
                                                     {
                                                         code = "21861-0",
                                                         codeSystem = "2.16.840.1.113883.6.1",
                                                         codeSystemName = "LOINC",
                                                         displayName = "Dx confirmed by Cancer"
                                                     },
                                                     value = new CD
                                                     {
                                                         code = "",
                                                         codeSystem = "2.16.840.1.113883.3.520.3.3",
                                                         codeSystemName = "NAACCR Diagnostic Confirmation",
                                                         displayName = "Positive Histology",
                                                         valueSet = "2.16.840.1.113883.3.520.4.3",
                                                         originalText =  new ED
                                                         {
                                                             reference = new TEL
                                                             {
                                                                 value = "#Confirmation_1"
                                                             }
                                                         }
                                                     }
                                                 }
                                                 #endregion 
                                             }
	                                        #endregion
                                         },
                                    },
                                    #region TargetSiteCode
                                    targetSiteCode = new List<CD>
                                    {
                                        new CD
                                        {
                                            code = "181131000", // Get and set the codes here
                                            codeSystem = "2.16.840.1.113883.6.96",
                                            codeSystemName = "SNOMED CT",
                                            displayName = "Entire breast",
                                            valueSet = "2.16.840.1.113883.3.88.12.3221.8.9",
                                            originalText = new ED
                                            {
                                                reference = new TEL
                                                {
                                                    value = "#PrimarySite_1"
                                                }
                                            },
                                            qualifier = new List<CR>
                                            {
                                                new CR
                                                {
                                                    name = new CV // Get and set the codes here
                                                    {
                                                        code = "20228-3",
                                                        codeSystem = "2.16.840.1.113883.6.1",
                                                        codeSystemName = "LOINC",
                                                        displayName = "Anatomic part Laterality"
                                                    },
                                                    value = new CD
                                                    {

                                                        code = "24028007", // Get and set the codes here
                                                        codeSystem = "2.16.840.1.113883.6.96",
                                                        codeSystemName = "SNOMED-CT",
                                                        displayName = "Right (qualifier value)",
                                                        valueSet = "2.16.840.1.113883.3.520.4.22",
                                                        originalText =  new ED
                                                        {
                                                            reference = new TEL
                                                            {
                                                                value = "#Laterality_1"
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    #endregion
                                    entryRelationship = new List<EntryRelationship>
                                    {
                                        #region EntryRelationship 1.1
                                        new EntryRelationship
                                        {
                                            typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                                            inversionInd = true,
                                            Item = new Observation()
                                            {
                                                classCode = "OBS",
                                                moodCode = x_ActMoodDocumentObservation.EVN,
                                                templateId= new List<II>
                                                {
                                                    new II
                                                    {
                                                        root = "2.16.840.1.113883.10.13.5",
                                                        extension = "2015-02-05"
                                                    },
                                                    new II
                                                    {
                                                        root = "2.16.840.1.113883.10.13.5",

                                                    }
                                                },
                                                id = new List<II>
                                                {
                                                    new II
                                                    {
                                                        root = Guid.NewGuid().ToString()
                                                    }
                                                },
                                                code = new CD  // Get the Codes and Done with it
                                                {
                                                    code = "75620-5",
                                                    codeSystem = "2.16.840.1.113883.6.1",
                                                    codeSystemName = "LOINC",
                                                    displayName = "TNM clinical staging before treatment panel Cancer"
                                                },
                                                statusCode = new CS
                                                {
                                                    code = "completed"
                                                },
                                                effectiveTime = new IVL_TS()
                                                {
                                                    ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                                    Items = new QTY[]
                                                    {
                                                        new IVXB_TS()
                                                        {
                                                            value = DateTime.Now.ToString("yyyyMMddhhmmss") // The low value represents when the problem was first recorded in the patient's chart 
                                                        },
                                                        new IVXB_TS()
                                                        {
                                                            nullFlavor = "NA"
                                                        }
                                                    }
                                                },
                                                entryRelationship = new List<EntryRelationship>
                                                {
                                                    new EntryRelationship
                                                    {
                                                        typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                        Item = new Observation
                                                        {
                                                            classCode = "",
                                                            moodCode = x_ActMoodDocumentObservation.EVN,
                                                            templateId= new List<II>
                                                            {
                                                                new II
                                                                {
                                                                    root = "2.16.840.1.113883.10.13.35",
                                                                    extension = "2015-02-05"
                                                                },
                                                                new II
                                                                {
                                                                    root = "2.16.840.1.113883.10.13.35",

                                                                }
                                                            },
                                                            code = new CD  // Get the Codes and Done with it
                                                            {
                                                                code = "21908-9",
                                                                codeSystem = "2.16.840.1.113883.6.1",
                                                                codeSystemName = "LOINC",
                                                                displayName = "Stage group.clinical"
                                                            },
                                                            statusCode = new CS
                                                            {
                                                                code = "completed"
                                                            },
                                                            value = new List<ANY>
                                                            {
                                                                new CD
                                                                {
                                                                    code = "IIB",
                                                                    codeSystem = "2.16.840.1.113883.15.6",
                                                                    codeSystemName = "TNM 7. Edition",
                                                                    displayName = "",
                                                                    valueSet = "2.16.840.1.113883.3.520.4.9",
                                                                    originalText = new ED
                                                                    {
                                                                        reference = new TEL
                                                                        {
                                                                            value = "#ClinicalStageGroup_1"
                                                                        }
                                                                    },
                                                                    qualifier = new List<CR>
                                                                    {
                                                                        new CR
                                                                        {
                                                                            name = new CV
                                                                            {
                                                                                code = "21909-7",
                                                                                codeSystem = "2.16.840.1.113883.6.1",
                                                                                codeSystemName = "LOINC",
                                                                                displayName = "Descriptor.clinical Cancer Narrative"
                                                                            },
                                                                            value = new CD
                                                                            {
                                                                                code = "0",
                                                                                codeSystem = "2.16.840.1.113883.15.6",
                                                                                codeSystemName = "TNM 7. Edition",
                                                                                displayName = "None",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.10",
                                                                                originalText = new ED
                                                                                {
                                                                                    reference = new TEL
                                                                                    {
                                                                                        value = "#ClinicalNarrative_1"
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            },
                                                            entryRelationship = new List<EntryRelationship>
                                                            {
                                                                #region EntryRelationship 1.4.1
                                                                new EntryRelationship
                                                                {
                                                                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                    Item = new Observation
                                                                    {
                                                                        classCode = "OBS",
                                                                        moodCode = x_ActMoodDocumentObservation.EVN,
                                                                        templateId = new List<II>
                                                                        {
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.36",
                                                                                extension = "2015-02-05"
                                                                            },
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.36",

                                                                            }
                                                                        },
                                                                        code = new CD
                                                                        {
                                                                            code = "21905-5",
                                                                            codeSystem = "2.16.840.1.113883.6.1",
                                                                            codeSystemName = "LOINC",
                                                                            displayName = "Primary tumor.clinical [Class] Cancer"
                                                                        },
                                                                        value = new List<ANY>
                                                                        {
                                                                            new CD
                                                                            {
                                                                                code = "T2",
                                                                                codeSystem = "2.16.840.1.113883.15.6",
                                                                                codeSystemName = "TNM 7. Edition",
                                                                                displayName = "Tumor > 20 mm but <= to 50 mm in greatest dimension",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.6",
                                                                                originalText = new ED
                                                                                {
                                                                                    reference = new TEL
                                                                                    {
                                                                                        value = "#ClinicalPrimaryTumor_1"
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                },
                                                                #endregion
                                                                #region EntryRelationship 1.4.2
                                                                new EntryRelationship
                                                                {
                                                                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                    Item = new Observation
                                                                    {
                                                                        classCode = "OBS",
                                                                        moodCode = x_ActMoodDocumentObservation.EVN,
                                                                        templateId = new List<II>
                                                                        {
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.37",
                                                                                extension = "2015-02-05"
                                                                            },
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.37",
                                                                            }
                                                                        },
                                                                        code = new CD
                                                                        {
                                                                            code = "21906-3",
                                                                            codeSystem = "2.16.840.1.113883.6.1",
                                                                            codeSystemName = "LOINC",
                                                                            displayName = "Regional lymph nodes.clinical [Class] Cancer"
                                                                        },
                                                                        value = new List<ANY>
                                                                        {
                                                                            new CD
                                                                            {
                                                                                code = "N1",
                                                                                codeSystem = "2.16.840.1.113883.15.6",
                                                                                codeSystemName = "TNM 7. Edition",
                                                                                displayName = "Metastases to movable ipsilateral level I, II axillary lymph node(s)",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.7",
                                                                                originalText = new ED
                                                                                {
                                                                                    reference = new TEL
                                                                                    {
                                                                                        value = "#ClinicalNode_1"
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                },
                                                                #endregion
                                                                #region EntryRelationship 1.4.3
                                                                new EntryRelationship
                                                                {
                                                                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                    Item = new Observation
                                                                    {
                                                                        classCode = "OBS",
                                                                        moodCode = x_ActMoodDocumentObservation.EVN,
                                                                        templateId = new List<II>
                                                                        {
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.38",
                                                                                extension = "2015-02-05"
                                                                            },
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.38",
                                                                            }
                                                                        },
                                                                        code = new CD
                                                                        {
                                                                            code = "21907-1",
                                                                            codeSystem = "2.16.840.1.113883.6.1",
                                                                            codeSystemName = "LOINC",
                                                                            displayName = "Distant metastases.clinical [Class] Cancer"
                                                                        },
                                                                        value = new List<ANY>
                                                                        {
                                                                            new CD
                                                                            {
                                                                                code = "M0",
                                                                                codeSystem = "2.16.840.1.113883.15.6",
                                                                                codeSystemName = "TNM 7. Edition",
                                                                                displayName = "No clinical or radiographic evidence of distant metastases",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.8",
                                                                                originalText = new ED
                                                                                {
                                                                                    reference = new TEL
                                                                                    {
                                                                                        value = "#ClinicalMetasases_1"
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                },
                                                                #endregion
                                                                #region EntryRelationship 1.4.4
                                                                new EntryRelationship
                                                                {
                                                                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                    Item = new Observation
                                                                    {
                                                                        classCode = "OBS",
                                                                        moodCode = x_ActMoodDocumentObservation.EVN,
                                                                        templateId = new List<II>
                                                                        {
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.39",
                                                                                extension = "2015-02-05"
                                                                            },
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.39",
                                                                            }
                                                                        },
                                                                        code = new CD
                                                                        {
                                                                            code = "21910-5",
                                                                            codeSystem = "2.16.840.1.113883.6.1",
                                                                            codeSystemName = "LOINC",
                                                                            displayName = "Stager.clinical Cancer"
                                                                        },
                                                                        value = new List<ANY>
                                                                        {
                                                                            new CD
                                                                            {
                                                                                code = "3",
                                                                                codeSystem = "2.16.840.1.113883.3.520.3.4",
                                                                                codeSystemName = "NAACCR TNM Clinical Staged By",
                                                                                displayName = "Pathologist and managing physician",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.4",
                                                                                originalText = new ED
                                                                                {
                                                                                    reference = new TEL
                                                                                    {
                                                                                        value = "#ClinicalStagedBy_1"
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        #endregion
                                        #region EntryRelationship 1.2
                                        new EntryRelationship
                                        {
                                            typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                                            inversionInd = true,
                                            Item = new Observation()
                                            {
                                                classCode = "OBS",
                                                moodCode = x_ActMoodDocumentObservation.EVN,
                                                templateId= new List<II>
                                                {
                                                    new II
                                                    {
                                                        root = "2.16.840.1.113883.10.13.7",
                                                        extension = "2015-02-05"
                                                    },
                                                     new II
                                                    {
                                                        root = "2.16.840.1.113883.10.13.7",
                                                    }
                                                },
                                                id = new List<II>
                                                {
                                                    new II
                                                    {
                                                        root = Guid.NewGuid().ToString()
                                                    }
                                                },
                                                code = new CD  // Get the Codes and Done with it
                                                {
                                                    code = "75621-3",
                                                    codeSystem = "2.16.840.1.113883.6.1",
                                                    codeSystemName = "LOINC",
                                                    displayName = "TNM pathologic staging after surgery panel"
                                                },
                                                statusCode = new CS
                                                {
                                                    code = "completed"
                                                },
                                                effectiveTime = new IVL_TS()
                                                {
                                                    ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                                    Items = new QTY[]
                                                    {
                                                        new IVXB_TS()
                                                        {
                                                            value = DateTime.Now.ToString("yyyyMMddhhmmss") // The low value represents when the problem was first recorded in the patient's chart 
                                                        },
                                                        new IVXB_TS()
                                                        {
                                                            nullFlavor = "NA"
                                                        }
                                                    }
                                                },
                                                entryRelationship = new List<EntryRelationship>
                                                {
                                                    new EntryRelationship
                                                    {
                                                        typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                        Item = new Observation
                                                        {
                                                            classCode = "",
                                                            moodCode = x_ActMoodDocumentObservation.EVN,
                                                            templateId= new List<II>
                                                            {
                                                                new II
                                                                {
                                                                    root = "2.16.840.1.113883.10.13.40",
                                                                    extension = "2015-02-05"
                                                                },
                                                                new II
                                                                {
                                                                    root = "2.16.840.1.113883.10.13.40",
                                                                 }
                                                            },
                                                            code = new CD  // Get the Codes and Done with it
                                                            {
                                                                code = "21902-2",
                                                                codeSystem = "2.16.840.1.113883.6.1",
                                                                codeSystemName = "LOINC",
                                                                displayName = "Stage group.pathology Cancer"
                                                            },
                                                            statusCode = new CS
                                                            {
                                                                code = "completed"
                                                            },
                                                            value = new List<ANY>
                                                            {
                                                                new CD
                                                                {
                                                                    code = "IIB",
                                                                    codeSystem = "2.16.840.1.113883.15.6",
                                                                    codeSystemName = "TNM 7. Edition",
                                                                    displayName = "",
                                                                    valueSet = "2.16.840.1.113883.3.520.4.20",
                                                                    originalText = new ED
                                                                    {
                                                                        reference = new TEL
                                                                        {
                                                                            value = "#PathologicStageGroup_1"
                                                                        }
                                                                    },
                                                                    qualifier = new List<CR>
                                                                    {
                                                                        new CR
                                                                        {
                                                                            name = new CV
                                                                            {
                                                                                code = "21903-0",
                                                                                codeSystem = "2.16.840.1.113883.6.1",
                                                                                codeSystemName = "LOINC",
                                                                                displayName = "Descriptor.pathology Cancer Narrative"
                                                                            },
                                                                            value = new CD
                                                                            {
                                                                                code = "0",
                                                                                codeSystem = "2.16.840.1.113883.15.6",
                                                                                codeSystemName = "TNM 7. Edition",
                                                                                displayName = "None",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.21"
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            },
                                                            #region EntryRelationship 2.16.840.1.113883.10.13.41
                                                            entryRelationship = new List<EntryRelationship>
                                                            {
                                                                new EntryRelationship
                                                                {
                                                                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                    Item = new Observation
                                                                    {
                                                                        classCode = "OBS",
                                                                        moodCode = x_ActMoodDocumentObservation.EVN,
                                                                        templateId = new List<II>
                                                                        {
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.41",
                                                                                extension = "2015-02-05"
                                                                            },
                                                                             new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.41",
                                                                            }
                                                                        },
                                                                        code = new CD
                                                                        {
                                                                            code = "21899-0",
                                                                            codeSystem = "2.16.840.1.113883.6.1",
                                                                            codeSystemName = "LOINC",
                                                                            displayName = "Primary tumor.pathology Cancer"
                                                                        },
                                                                        value = new List<ANY>
                                                                        {
                                                                            new CD
                                                                            {
                                                                                code = "T2",
                                                                                codeSystem = "2.16.840.1.113883.15.6",
                                                                                codeSystemName = "TNM 7. Edition",
                                                                                displayName = "Tumor &gt; 20 mm but &lt;= to 50 mm in greatest dimension",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.17"
                                                                            }
                                                                        }
                                                                    }
                                                                },
                                                            #endregion  
                                                            #region EntryRelationship 2.16.840.1.113883.10.13.42
                                                            new EntryRelationship
                                                            {
                                                                typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                Item = new Observation
                                                                {
                                                                    classCode = "OBS",
                                                                    moodCode = x_ActMoodDocumentObservation.EVN,
                                                                    templateId = new List<II>
                                                                    {
                                                                        new II
                                                                        {
                                                                            root = "2.16.840.1.113883.10.13.37",
                                                                            extension = "2015-02-05"
                                                                        }
                                                                    },
                                                                    code = new CD
                                                                    {
                                                                        code = "21900-6",
                                                                        codeSystem = "2.16.840.1.113883.6.1",
                                                                        codeSystemName = "LOINC",
                                                                        displayName = "Regional lymph nodes.clinical [Class] Cancer"
                                                                    },
                                                                    value = new List<ANY>
                                                                    {
                                                                        new CD
                                                                        {
                                                                            code = "N1",
                                                                            codeSystem = "2.16.840.1.113883.15.6",
                                                                            codeSystemName = "TNM 7. Edition",
                                                                            displayName = "Micrometastases; or metastases in 1 to 3 axillary lymph nodes; and/or in internal mammary nodes with metastases detected by sentinel lymph node biopsy but not clinically detected.",
                                                                            valueSet = "2.16.840.1.113883.3.520.4.18"
                                                                            //originalText = new ED
                                                                            //{
                                                                            //    reference = new TEL
                                                                            //    {
                                                                            //        value = "#ClinicalNode_1"
                                                                            //    }
                                                                            //}
                                                                        }
                                                                    }
                                                                }
                                                            },
                                                            #endregion  
                                                            #region EntryRelationship 2.16.840.1.113883.10.13.43
                                                            new EntryRelationship
                                                            {
                                                                typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                Item = new Observation
                                                                {
                                                                    classCode = "OBS",
                                                                    moodCode = x_ActMoodDocumentObservation.EVN,
                                                                    templateId = new List<II>
                                                                    {
                                                                        new II
                                                                        {
                                                                            root = "2.16.840.1.113883.10.13.43",
                                                                            extension = "2015-02-05"
                                                                        }
                                                                    },
                                                                    code = new CD
                                                                    {
                                                                        code = "21901-4",
                                                                        codeSystem = "2.16.840.1.113883.6.1",
                                                                        codeSystemName = "LOINC",
                                                                        displayName = "Distant metastases.pathology [Class] Cancer"
                                                                    },
                                                                    value = new List<ANY>
                                                                    {
                                                                        new CD
                                                                        {
                                                                            code = "MX",
                                                                            codeSystem = "2.16.840.1.113883.15.6",
                                                                            codeSystemName = "TNM 7. Edition",
                                                                            //displayName = "No clinical or radiographic evidence of distant metastases",
                                                                            valueSet = "2.16.840.1.113883.3.520.4.19"
                                                                            //originalText = new ED
                                                                            //{
                                                                            //    reference = new TEL
                                                                            //    {
                                                                            //        value = "#ClinicalMetasases_1"
                                                                            //    }
                                                                            //}
                                                                        }
                                                                    }
                                                                }
                                                            },
                                                            #endregion  
                                                            #region EntryRelationship 2.16.840.1.113883.10.13.44
                                                            new EntryRelationship
                                                            {
                                                                typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                Item = new Observation
                                                                {
                                                                    classCode = "OBS",
                                                                    moodCode = x_ActMoodDocumentObservation.EVN,
                                                                    templateId = new List<II>
                                                                    {
                                                                        new II
                                                                        {
                                                                            root = "2.16.840.1.113883.10.13.44",
                                                                            extension = "2015-02-05"
                                                                        }
                                                                    },
                                                                    code = new CD
                                                                    {
                                                                        code = "21904-8",
                                                                        codeSystem = "2.16.840.1.113883.6.1",
                                                                        codeSystemName = "LOINC",
                                                                        displayName = "Stager.pathology Cancer"
                                                                    },
                                                                    value = new List<ANY>
                                                                    {
                                                                        new CD
                                                                        {
                                                                            code = "3",
                                                                            codeSystem = "2.16.840.1.113883.3.520.3.17",
                                                                            codeSystemName = "NAACCR TNM Pathologic Staged By",
                                                                            displayName = "Pathologist and managing physician",
                                                                            valueSet = "2.16.840.1.113883.3.520.4.27"
                                                                            //originalText = new ED
                                                                            //{
                                                                            //    reference = new TEL
                                                                            //    {
                                                                            //        value = "#ClinicalStagedBy_1"
                                                                            //    }
                                                                            //}
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            #endregion  
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        #endregion
                                        #region EntryRelationship 1.3
                                        new EntryRelationship
                                        {
                                            typeCode = x_ActRelationshipEntryRelationship.REFR,
                                            Item = new Observation
                                            {
                                                classCode = "OBS",
                                                moodCode = x_ActMoodDocumentObservation.EVN,
                                                id = new List<II>
                                                {
                                                    new II
                                                    {
                                                        root = Guid.NewGuid().ToString()
                                                    }
                                                },
                                                code = new CD
                                                {
                                                    code = "64572001",
                                                    codeSystem = "2.16.840.1.113883.6.96",
                                                    codeSystemName = "SNOMED CT",
                                                    displayName = "Condition",
                                                }
                                            }
                                        }
                                    	#endregion
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            };
                sb.component.Add(instructionsComponent);

            }
            else
            {
            }

        }

        private static void AddHealthConcernsSectionComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            Component3 instructionsComponent = HelperMethodsCaseReports.IntializeComponent3(new List<string> { "2.16.840.1.113883.10.20.22.2.58" }, "75310-3", "2.16.840.1.113883.6.1", "LOINC", "Health Concerns Document", "Health Concerns Section");

            instructionsComponent.section.templateId[0].extension = "2015-08-01";
            StrucDocTable table = new StrucDocTable
            {
                width = "100%",
                border = "1",
                Items = new List<object>()
            };

            instructionsComponent.section.text.Items.Add(table);
            var lstConcerns = objCcdaModel.lstGoal;
            if (lstConcerns != null && lstConcerns.Count > 0)
            {
                #region TableHeader

                table.thead = new StrucDocThead
                {
                    tr = new List<StrucDocTr>
                    {
                        new StrucDocTr
                        {
                            Items = new List<object>
                            {
                                new StrucDocTh{ Text = new List<string>{ "Concern" } },
                                new StrucDocTh{ Text = new List<string>{ "Status" } },
                                new StrucDocTh{ Text = new List<string>{ "Date" } },
                            }
                        }
                    }
                };

                #endregion

                #region TableBody

                table.tbody = new List<StrucDocTbody>();
                var tBody = new StrucDocTbody();
                table.tbody.Add(tBody);
                tBody.tr = new List<StrucDocTr>();
                var i = 0;
                foreach (var dr in lstConcerns)
                {
                    i++;
                    var tr = new StrucDocTr { Items = new List<object>() };
                    var name = MDVUtility.ToStr(dr["Name"]);
                    var snomedid = MDVUtility.ToStr(dr["SNOMEDID"]);

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        name = name + ", [SNOMED-CT: " + snomedid + "]";
                        tr.Items.Add(new StrucDocTd { ID = "functionalstatus_" + i, Text = new List<string> { name } });
                    }
                    if (!string.IsNullOrWhiteSpace(snomedid))
                    {
                        snomedid = "[SNOMED-CT: " + snomedid + "]";
                        tr.Items.Add(new StrucDocTd { Text = new List<string> { snomedid } });
                    }
                    var effectiveDate = objCcdaModel.VisitDate;//dr["EffectiveDate"];
                    if (!string.IsNullOrWhiteSpace(effectiveDate))
                        effectiveDate = Convert.ToDateTime(effectiveDate).ToString("MM/dd/yyyy");
                    tr.Items.Add(new StrucDocTd { Text = new List<string> { effectiveDate } });
                    tBody.tr.Add(tr);
                }

                #endregion

                instructionsComponent.section.entry = new List<Entry>
            {
                new Entry
                {
                    typeCode = x_ActRelationshipEntry.DRIV,
                    Item = new Act()
                    {
                        classCode = x_ActClassDocumentEntryAct.ACT,
                        moodCode = x_DocumentActMood.EVN,
                        templateId= new List<II>
                        {
                            new II
                            {
                                root = "2.16.840.1.113883.10.20.22.4.132",
                                extension = "2015-02-05"
                            }
                        },
                        id = new List<II>
                        {
                            new II
                            {
                                root = Guid.NewGuid().ToString()
                            }
                        },
                        code = new CD
                        {
                            code = "75310-3",
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC",
                            displayName = "Health Concern"
                        },
                        statusCode = new CS
                        {
                            code = "active"
                        },
                        effectiveTime = new IVL_TS()
                        {
                            ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                            Items = new QTY[]
                            {
                                new IVXB_TS()
                                {
                                    value = DateTime.Now.ToString("yyyyMMdd") // The low value represents when the problem was first recorded in the patient's chart 
                                },
                                new IVXB_TS()
                                {
                                    nullFlavor = "NA"
                                }
                            }
                        },
                        entryRelationship = new List<EntryRelationship>
                        {
                            #region EntryRelationship
                            new EntryRelationship
                            {
                                typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                                Item = new Observation()
                                {
                                    classCode = "OBS",
                                    moodCode = x_ActMoodDocumentObservation.EVN,
                                    templateId= new List<II>
                                    {
                                        new II
                                        {
                                            root = "2.16.840.1.113883.10.20.22.4.85",
                                            extension = "2015-02-05"
                                        },
                                        new II
                                        {
                                            root = "2.16.840.1.113883.10.20.22.4.85"
                                        }
                                    },
                                    id = new List<II>
                                    {
                                        new II
                                        {
                                            root = Guid.NewGuid().ToString()
                                        }
                                    },
                                    code = new CD  // Get the Codes and Done with it
                                    {
                                        code = "11367-0",
                                        codeSystem = "2.16.840.1.113883.5.6",
                                        codeSystemName = "HL7ActClass",
                                        displayName = "Concern"
                                    },
                                    text = new ED
                                    {
                                        reference = new TEL
                                        {
                                            value = "#Diagnosis_1"
                                        }
                                    },
                                    statusCode = new CS
                                    {
                                        code = "active"
                                    },
                                    effectiveTime = new IVL_TS()
                                    {
                                        ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                        Items = new QTY[]
                                        {
                                            new IVXB_TS()
                                            {
                                                value = DateTime.Now.ToString("yyyyMMdd") // The low value represents when the problem was first recorded in the patient's chart 
                                            },
                                            new IVXB_TS()
                                            {
                                                nullFlavor = "NA"
                                            }
                                        }
                                    },
                                    value = new List<ANY>
                                    {
                                         new CD
                                         {
                                             code = "8140",
                                             codeSystem = "2.16.840.1.113883.6.43.1",
                                             codeSystemName = "ICD-O-3",
                                             displayName = "Adenocarcinoma, NOS",
                                             originalText = new ED
                                             {
                                                 reference = new TEL
                                                 {
                                                     value = "#Histology_1"
                                                 }
                                             },

                                             #region Qualifier's
                                             qualifier = new List<CR>
                                             {
                                                #region Qualifier1
                                                 new CR
                                                 {
                                                     name = new CV // Get and set the codes here
                                                     {
                                                         code = "31206-6",
                                                         codeSystem = "2.16.840.1.113883.6.1",
                                                         codeSystemName = "LOINC",
                                                         displayName = "Behavior ICD-O-3 Cancer"
                                                     },
                                                     value = new CD
                                                     {

                                                         code = "3",
                                                         codeSystem = "2.16.840.1.113883.3.520.3.14",
                                                         codeSystemName = "NAACCR Behavior Code",
                                                         displayName = "Malignant, Primary",
                                                         valueSet = "2.16.840.1.113883.3.520.4.14",
                                                         originalText =  new ED
                                                         {
                                                             reference = new TEL
                                                             {
                                                                 value = "#Behavior_1"
                                                             }
                                                         }
                                                     }
                                                 },
                                                 #endregion 
                                                #region Qualifier2
                                                 new CR
                                                 {
                                                     name = new CV // Get and set the codes here
                                                     {
                                                         code = "21858-6",
                                                         codeSystem = "2.16.840.1.113883.6.1",
                                                         codeSystemName = "LOINC",
                                                         displayName = "Grade Cancer"
                                                     },
                                                     value = new CD
                                                     {
                                                         code = "II",
                                                         codeSystem = "2.16.840.1.113883.3.520.3.15",
                                                         codeSystemName = "NAACCR Grade",
                                                         displayName = "Grade II",
                                                         valueSet = "2.16.840.1.113883.3.520.4.15",
                                                         originalText =  new ED
                                                         {
                                                             reference = new TEL
                                                             {
                                                                 value = "#Grade_1"
                                                             }
                                                         }
                                                     }
                                                 },
                                                 #endregion 
                                                #region Qualifier3
                                                 new CR
                                                 {
                                                     name = new CV() // Get and set the codes here
                                                     {
                                                         code = "21861-0",
                                                         codeSystem = "2.16.840.1.113883.6.1",
                                                         codeSystemName = "LOINC",
                                                         displayName = "Dx confirmed by Cancer"
                                                     },
                                                     value = new CD
                                                     {
                                                         code = "",
                                                         codeSystem = "2.16.840.1.113883.3.520.3.3",
                                                         codeSystemName = "NAACCR Diagnostic Confirmation",
                                                         displayName = "Positive Histology",
                                                         valueSet = "2.16.840.1.113883.3.520.4.3",
                                                         originalText =  new ED
                                                         {
                                                             reference = new TEL
                                                             {
                                                                 value = "#Confirmation_1"
                                                             }
                                                         }
                                                     }
                                                 }
                                                 #endregion 
                                             }
	                                        #endregion
                                         },
                                    },
                                    #region TargetSiteCode
                                    targetSiteCode = new List<CD>
                                    {
                                        new CD
                                        {
                                            code = "181131000", // Get and set the codes here
                                            codeSystem = "2.16.840.1.113883.6.96",
                                            codeSystemName = "SNOMED CT",
                                            displayName = "Entire breast",
                                            valueSet = "2.16.840.1.113883.3.88.12.3221.8.9",
                                            originalText = new ED
                                            {
                                                reference = new TEL
                                                {
                                                    value = "#PrimarySite_1"
                                                }
                                            },
                                            qualifier = new List<CR>
                                            {
                                                new CR
                                                {
                                                    name = new CV // Get and set the codes here
                                                    {
                                                        code = "20228-3",
                                                        codeSystem = "2.16.840.1.113883.6.1",
                                                        codeSystemName = "LOINC",
                                                        displayName = "Anatomic part Laterality"
                                                    },
                                                    value = new CD
                                                    {

                                                        code = "24028007", // Get and set the codes here
                                                        codeSystem = "2.16.840.1.113883.6.96",
                                                        codeSystemName = "SNOMED-CT",
                                                        displayName = "Right (qualifier value)",
                                                        valueSet = "2.16.840.1.113883.3.520.4.22",
                                                        originalText =  new ED
                                                        {
                                                            reference = new TEL
                                                            {
                                                                value = "#Laterality_1"
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    #endregion
                                    entryRelationship = new List<EntryRelationship>
                                    {
                                        #region EntryRelationship 1.1
                                        new EntryRelationship
                                        {
                                            typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                                            inversionInd = true,
                                            Item = new Observation()
                                            {
                                                classCode = "OBS",
                                                moodCode = x_ActMoodDocumentObservation.EVN,
                                                templateId= new List<II>
                                                {
                                                    new II
                                                    {
                                                        root = "2.16.840.1.113883.10.13.5",
                                                        extension = "2015-02-05"
                                                    }
                                                },
                                                id = new List<II>
                                                {
                                                    new II
                                                    {
                                                        root = Guid.NewGuid().ToString()
                                                    }
                                                },
                                                code = new CD  // Get the Codes and Done with it
                                                {
                                                    code = "75620-5",
                                                    codeSystem = "2.16.840.1.113883.6.1",
                                                    codeSystemName = "LOINC",
                                                    displayName = "TNM clinical staging before treatment panel Cancer"
                                                },
                                                statusCode = new CS
                                                {
                                                    code = "completed"
                                                },
                                                effectiveTime = new IVL_TS()
                                                {
                                                    ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                                    Items = new QTY[]
                                                    {
                                                        new IVXB_TS()
                                                        {
                                                            value = DateTime.Now.ToString("yyyyMMddhhmmss") // The low value represents when the problem was first recorded in the patient's chart 
                                                        },
                                                        new IVXB_TS()
                                                        {
                                                            nullFlavor = "NA"
                                                        }
                                                    }
                                                },
                                                entryRelationship = new List<EntryRelationship>
                                                {
                                                    new EntryRelationship
                                                    {
                                                        typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                        Item = new Observation
                                                        {
                                                            classCode = "",
                                                            moodCode = x_ActMoodDocumentObservation.EVN,
                                                            templateId= new List<II>
                                                            {
                                                                new II
                                                                {
                                                                    root = "2.16.840.1.113883.10.13.35",
                                                                    extension = "2015-02-05"
                                                                }
                                                            },
                                                            code = new CD  // Get the Codes and Done with it
                                                            {
                                                                code = "21908-9",
                                                                codeSystem = "2.16.840.1.113883.6.1",
                                                                codeSystemName = "LOINC",
                                                                displayName = "Stage group.clinical"
                                                            },
                                                            statusCode = new CS
                                                            {
                                                                code = "completed"
                                                            },
                                                            value = new List<ANY>
                                                            {
                                                                new CD
                                                                {
                                                                    code = "IIB",
                                                                    codeSystem = "2.16.840.1.113883.15.6",
                                                                    codeSystemName = "TNM 7. Edition",
                                                                    displayName = "",
                                                                    valueSet = "2.16.840.1.113883.3.520.4.9",
                                                                    originalText = new ED
                                                                    {
                                                                        reference = new TEL
                                                                        {
                                                                            value = "#ClinicalStageGroup_1"
                                                                        }
                                                                    },
                                                                    qualifier = new List<CR>
                                                                    {
                                                                        new CR
                                                                        {
                                                                            name = new CV
                                                                            {
                                                                                code = "21909-7",
                                                                                codeSystem = "2.16.840.1.113883.6.1",
                                                                                codeSystemName = "LOINC",
                                                                                displayName = "Descriptor.clinical Cancer Narrative"
                                                                            },
                                                                            value = new CD
                                                                            {
                                                                                code = "0",
                                                                                codeSystem = "2.16.840.1.113883.15.6",
                                                                                codeSystemName = "TNM 7. Edition",
                                                                                displayName = "None",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.10",
                                                                                originalText = new ED
                                                                                {
                                                                                    reference = new TEL
                                                                                    {
                                                                                        value = "#ClinicalNarrative_1"
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            },
                                                            entryRelationship = new List<EntryRelationship>
                                                            {
                                                                #region EntryRelationship 1.4.1
                                                                new EntryRelationship
                                                                {
                                                                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                    Item = new Observation
                                                                    {
                                                                        classCode = "OBS",
                                                                        moodCode = x_ActMoodDocumentObservation.EVN,
                                                                        templateId = new List<II>
                                                                        {
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.36",
                                                                                extension = "2015-02-05"
                                                                            }
                                                                        },
                                                                        code = new CD
                                                                        {
                                                                            code = "21905-5",
                                                                            codeSystem = "2.16.840.1.113883.6.1",
                                                                            codeSystemName = "LOINC",
                                                                            displayName = "Primary tumor.clinical [Class] Cancer"
                                                                        },
                                                                        value = new List<ANY>
                                                                        {
                                                                            new CD
                                                                            {
                                                                                code = "T2",
                                                                                codeSystem = "2.16.840.1.113883.15.6",
                                                                                codeSystemName = "TNM 7. Edition",
                                                                                displayName = "Tumor > 20 mm but <= to 50 mm in greatest dimension",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.6",
                                                                                originalText = new ED
                                                                                {
                                                                                    reference = new TEL
                                                                                    {
                                                                                        value = "#ClinicalPrimaryTumor_1"
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                },
                                                                #endregion
                                                                #region EntryRelationship 1.4.2
                                                                new EntryRelationship
                                                                {
                                                                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                    Item = new Observation
                                                                    {
                                                                        classCode = "OBS",
                                                                        moodCode = x_ActMoodDocumentObservation.EVN,
                                                                        templateId = new List<II>
                                                                        {
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.37",
                                                                                extension = "2015-02-05"
                                                                            }
                                                                        },
                                                                        code = new CD
                                                                        {
                                                                            code = "21906-3",
                                                                            codeSystem = "2.16.840.1.113883.6.1",
                                                                            codeSystemName = "LOINC",
                                                                            displayName = "Regional lymph nodes.clinical [Class] Cancer"
                                                                        },
                                                                        value = new List<ANY>
                                                                        {
                                                                            new CD
                                                                            {
                                                                                code = "N1",
                                                                                codeSystem = "2.16.840.1.113883.15.6",
                                                                                codeSystemName = "TNM 7. Edition",
                                                                                displayName = "Metastases to movable ipsilateral level I, II axillary lymph node(s)",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.7",
                                                                                originalText = new ED
                                                                                {
                                                                                    reference = new TEL
                                                                                    {
                                                                                        value = "#ClinicalNode_1"
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                },
                                                                #endregion
                                                                #region EntryRelationship 1.4.3
                                                                new EntryRelationship
                                                                {
                                                                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                    Item = new Observation
                                                                    {
                                                                        classCode = "OBS",
                                                                        moodCode = x_ActMoodDocumentObservation.EVN,
                                                                        templateId = new List<II>
                                                                        {
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.38",
                                                                                extension = "2015-02-05"
                                                                            }
                                                                        },
                                                                        code = new CD
                                                                        {
                                                                            code = "21907-1",
                                                                            codeSystem = "2.16.840.1.113883.6.1",
                                                                            codeSystemName = "LOINC",
                                                                            displayName = "Distant metastases.clinical [Class] Cancer"
                                                                        },
                                                                        value = new List<ANY>
                                                                        {
                                                                            new CD
                                                                            {
                                                                                code = "M0",
                                                                                codeSystem = "2.16.840.1.113883.15.6",
                                                                                codeSystemName = "TNM 7. Edition",
                                                                                displayName = "No clinical or radiographic evidence of distant metastases",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.8",
                                                                                originalText = new ED
                                                                                {
                                                                                    reference = new TEL
                                                                                    {
                                                                                        value = "#ClinicalMetasases_1"
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                },
                                                                #endregion
                                                                #region EntryRelationship 1.4.4
                                                                new EntryRelationship
                                                                {
                                                                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                    Item = new Observation
                                                                    {
                                                                        classCode = "OBS",
                                                                        moodCode = x_ActMoodDocumentObservation.EVN,
                                                                        templateId = new List<II>
                                                                        {
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.39",
                                                                                extension = "2015-02-05"
                                                                            }
                                                                        },
                                                                        code = new CD
                                                                        {
                                                                            code = "21910-5",
                                                                            codeSystem = "2.16.840.1.113883.6.1",
                                                                            codeSystemName = "LOINC",
                                                                            displayName = "Stager.clinical Cancer"
                                                                        },
                                                                        value = new List<ANY>
                                                                        {
                                                                            new CD
                                                                            {
                                                                                code = "3",
                                                                                codeSystem = "2.16.840.1.113883.3.520.3.4",
                                                                                codeSystemName = "NAACCR TNM Clinical Staged By",
                                                                                displayName = "Pathologist and managing physician",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.4",
                                                                                originalText = new ED
                                                                                {
                                                                                    reference = new TEL
                                                                                    {
                                                                                        value = "#ClinicalStagedBy_1"
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        #endregion
                                        #region EntryRelationship 1.2
                                        new EntryRelationship
                                        {
                                            typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                                            inversionInd = true,
                                            Item = new Observation()
                                            {
                                                classCode = "OBS",
                                                moodCode = x_ActMoodDocumentObservation.EVN,
                                                templateId= new List<II>
                                                {
                                                    new II
                                                    {
                                                        root = "2.16.840.1.113883.10.13.7",
                                                        extension = "2015-02-05"
                                                    }
                                                },
                                                id = new List<II>
                                                {
                                                    new II
                                                    {
                                                        root = Guid.NewGuid().ToString()
                                                    }
                                                },
                                                code = new CD  // Get the Codes and Done with it
                                                {
                                                    code = "75621-3",
                                                    codeSystem = "2.16.840.1.113883.6.1",
                                                    codeSystemName = "LOINC",
                                                    displayName = "TNM pathologic staging after surgery panel"
                                                },
                                                statusCode = new CS
                                                {
                                                    code = "completed"
                                                },
                                                effectiveTime = new IVL_TS()
                                                {
                                                    ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                                    Items = new QTY[]
                                                    {
                                                        new IVXB_TS()
                                                        {
                                                            value = DateTime.Now.ToString("yyyyMMddhhmmss") // The low value represents when the problem was first recorded in the patient's chart 
                                                        },
                                                        new IVXB_TS()
                                                        {
                                                            nullFlavor = "NA"
                                                        }
                                                    }
                                                },
                                                entryRelationship = new List<EntryRelationship>
                                                {
                                                    new EntryRelationship
                                                    {
                                                        typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                        Item = new Observation
                                                        {
                                                            classCode = "",
                                                            moodCode = x_ActMoodDocumentObservation.EVN,
                                                            templateId= new List<II>
                                                            {
                                                                new II
                                                                {
                                                                    root = "2.16.840.1.113883.10.13.40",
                                                                    extension = "2015-02-05"
                                                                }
                                                            },
                                                            code = new CD  // Get the Codes and Done with it
                                                            {
                                                                code = "21902-2",
                                                                codeSystem = "2.16.840.1.113883.6.1",
                                                                codeSystemName = "LOINC",
                                                                displayName = "Stage group.pathology Cancer"
                                                            },
                                                            statusCode = new CS
                                                            {
                                                                code = "completed"
                                                            },
                                                            value = new List<ANY>
                                                            {
                                                                new CD
                                                                {
                                                                    code = "IIB",
                                                                    codeSystem = "2.16.840.1.113883.15.6",
                                                                    codeSystemName = "TNM 7. Edition",
                                                                    displayName = "",
                                                                    valueSet = "2.16.840.1.113883.3.520.4.20",
                                                                    originalText = new ED
                                                                    {
                                                                        reference = new TEL
                                                                        {
                                                                            value = "#PathologicStageGroup_1"
                                                                        }
                                                                    },
                                                                    qualifier = new List<CR>
                                                                    {
                                                                        new CR
                                                                        {
                                                                            name = new CV
                                                                            {
                                                                                code = "21903-0",
                                                                                codeSystem = "2.16.840.1.113883.6.1",
                                                                                codeSystemName = "LOINC",
                                                                                displayName = "Descriptor.pathology Cancer Narrative"
                                                                            },
                                                                            value = new CD
                                                                            {
                                                                                code = "0",
                                                                                codeSystem = "2.16.840.1.113883.15.6",
                                                                                codeSystemName = "TNM 7. Edition",
                                                                                displayName = "None",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.21"
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            },
                                                            #region EntryRelationship 2.16.840.1.113883.10.13.41
                                                            entryRelationship = new List<EntryRelationship>
                                                            {
                                                                new EntryRelationship
                                                                {
                                                                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                    Item = new Observation
                                                                    {
                                                                        classCode = "OBS",
                                                                        moodCode = x_ActMoodDocumentObservation.EVN,
                                                                        templateId = new List<II>
                                                                        {
                                                                            new II
                                                                            {
                                                                                root = "2.16.840.1.113883.10.13.41",
                                                                                extension = "2015-02-05"
                                                                            }
                                                                        },
                                                                        code = new CD
                                                                        {
                                                                            code = "21899-0",
                                                                            codeSystem = "2.16.840.1.113883.6.1",
                                                                            codeSystemName = "LOINC",
                                                                            displayName = "Primary tumor.pathology Cancer"
                                                                        },
                                                                        value = new List<ANY>
                                                                        {
                                                                            new CD
                                                                            {
                                                                                code = "T2",
                                                                                codeSystem = "2.16.840.1.113883.15.6",
                                                                                codeSystemName = "TNM 7. Edition",
                                                                                displayName = "Tumor &gt; 20 mm but &lt;= to 50 mm in greatest dimension",
                                                                                valueSet = "2.16.840.1.113883.3.520.4.17"
                                                                            }
                                                                        }
                                                                    }
                                                                },
                                                            #endregion  
                                                            #region EntryRelationship 2.16.840.1.113883.10.13.42
                                                            new EntryRelationship
                                                            {
                                                                typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                Item = new Observation
                                                                {
                                                                    classCode = "OBS",
                                                                    moodCode = x_ActMoodDocumentObservation.EVN,
                                                                    templateId = new List<II>
                                                                    {
                                                                        new II
                                                                        {
                                                                            root = "2.16.840.1.113883.10.13.37",
                                                                            extension = "2015-02-05"
                                                                        }
                                                                    },
                                                                    code = new CD
                                                                    {
                                                                        code = "21900-6",
                                                                        codeSystem = "2.16.840.1.113883.6.1",
                                                                        codeSystemName = "LOINC",
                                                                        displayName = "Regional lymph nodes.clinical [Class] Cancer"
                                                                    },
                                                                    value = new List<ANY>
                                                                    {
                                                                        new CD
                                                                        {
                                                                            code = "N1",
                                                                            codeSystem = "2.16.840.1.113883.15.6",
                                                                            codeSystemName = "TNM 7. Edition",
                                                                            displayName = "Micrometastases; or metastases in 1 to 3 axillary lymph nodes; and/or in internal mammary nodes with metastases detected by sentinel lymph node biopsy but not clinically detected.",
                                                                            valueSet = "2.16.840.1.113883.3.520.4.18"
                                                                        }
                                                                    }
                                                                }
                                                            },
                                                            #endregion  
                                                            #region EntryRelationship 2.16.840.1.113883.10.13.43
                                                            new EntryRelationship
                                                            {
                                                                typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                Item = new Observation
                                                                {
                                                                    classCode = "OBS",
                                                                    moodCode = x_ActMoodDocumentObservation.EVN,
                                                                    templateId = new List<II>
                                                                    {
                                                                        new II
                                                                        {
                                                                            root = "2.16.840.1.113883.10.13.43",
                                                                            extension = "2015-02-05"
                                                                        }
                                                                    },
                                                                    code = new CD
                                                                    {
                                                                        code = "21901-4",
                                                                        codeSystem = "2.16.840.1.113883.6.1",
                                                                        codeSystemName = "LOINC",
                                                                        displayName = "Distant metastases.pathology [Class] Cancer"
                                                                    },
                                                                    value = new List<ANY>
                                                                    {
                                                                        new CD
                                                                        {
                                                                            code = "MX",
                                                                            codeSystem = "2.16.840.1.113883.15.6",
                                                                            codeSystemName = "TNM 7. Edition",
                                                                            valueSet = "2.16.840.1.113883.3.520.4.19"
                                                                        }
                                                                    }
                                                                }
                                                            },
                                                            #endregion  
                                                            #region EntryRelationship 2.16.840.1.113883.10.13.44
                                                            new EntryRelationship
                                                            {
                                                                typeCode = x_ActRelationshipEntryRelationship.COMP,
                                                                Item = new Observation
                                                                {
                                                                    classCode = "OBS",
                                                                    moodCode = x_ActMoodDocumentObservation.EVN,
                                                                    templateId = new List<II>
                                                                    {
                                                                        new II
                                                                        {
                                                                            root = "2.16.840.1.113883.10.13.44",
                                                                            extension = "2015-02-05"
                                                                        }
                                                                    },
                                                                    code = new CD
                                                                    {
                                                                        code = "21904-8",
                                                                        codeSystem = "2.16.840.1.113883.6.1",
                                                                        codeSystemName = "LOINC",
                                                                        displayName = "Stager.pathology Cancer"
                                                                    },
                                                                    value = new List<ANY>
                                                                    {
                                                                        new CD
                                                                        {
                                                                            code = "3",
                                                                            codeSystem = "2.16.840.1.113883.3.520.3.17",
                                                                            codeSystemName = "NAACCR TNM Pathologic Staged By",
                                                                            displayName = "Pathologist and managing physician",
                                                                            valueSet = "2.16.840.1.113883.3.520.4.27"
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            #endregion  
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        #endregion
                                        #region EntryRelationship 1.3
                                        new EntryRelationship
                                        {
                                            typeCode = x_ActRelationshipEntryRelationship.REFR,
                                            Item = new Observation
                                            {
                                                classCode = "OBS",
                                                moodCode = x_ActMoodDocumentObservation.EVN,
                                                id = new List<II>
                                                {
                                                    new II
                                                    {
                                                        root = Guid.NewGuid().ToString()
                                                    }
                                                },
                                                code = new CD
                                                {
                                                    code = "64572001",
                                                    codeSystem = "2.16.840.1.113883.6.96",
                                                    codeSystemName = "SNOMED CT",
                                                    displayName = "Condition",
                                                }
                                            }
                                        }
                                    	#endregion
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            };
                sb.component.Add(instructionsComponent);
            }
            else
            {
            }

        }

        private static void SetEncounters(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var encountersComponent = HelperMethods.IntializeComponent3(
                new List<string> { "2.16.840.1.113883.10.20.22.2.22.1", "2.16.840.1.113883.10.20.22.2.22.1" },
                "46240-8", "2.16.840.1.113883.6.1", "LOINC", "Encounters", "ENCOUNTER");
            var objPracticeData = objCcdaModel.lstPracticeData[0];
            var objProviderData = objCcdaModel.lstProviderData[0];
            encountersComponent.section.templateId[0].extension = "2015-08-01";
            var text = encountersComponent.section.text;
            var lstEncounterDiagnosic = objCcdaModel.lstEncounterDiagnosicDatakeyValues;
            if (lstEncounterDiagnosic != null && lstEncounterDiagnosic.Count > 0)
            {

                var id = 0;
                var table = new StrucDocTable
                {
                    border = "1",
                    width = "100%"
                };
                text.Items.Add(table);
                table.Items = new List<object>(getCols(9));
                table.thead = new StrucDocThead()
                {
                    tr = new List<StrucDocTr>
                    {
                        new StrucDocTr
                        {
                            Items = new List<object>
                            {
                                new StrucDocTh { Text = new List<string>{"Encounter"}},
                                new StrucDocTh { Text = new List<string>{"Date(s)"}},
                                new StrucDocTh { Text = new List<string>{"Location"}}
                            }
                        }
                    }
                };

                var tBody = new StrucDocTbody();
                table.tbody = new List<StrucDocTbody> { tBody };
                tBody.tr = new List<StrucDocTr>();

                var tr = new StrucDocTr();
                tBody.tr.Add(tr);
                tr.Items = new List<object>();
                var td = new StrucDocTd();
                tr.Items.Add(td);
                td.Items = new List<object>
                    {
                        new StrucDocContent
                        {
                            ID = string.Concat("Encounter", ++id)

                        }
                    };
                td.Text = new List<string>
                    {
                        //MDVUtility.ToStr(dr["ProblemShortName"])
                        MDVUtility.ToStr("Office outpatient visit 15 minutes")
                    };
                //td = new StrucDocTd();
                //tr.Items.Add(td);
                //td.Text = new List<string>
                //{
                //    objPracticeData["ShortName"]/*Convert.ToBoolean(dr["IsActive"]) ? "Active" : "Inactive"*/ };
                td = new StrucDocTd();
                tr.Items.Add(td);
                td.Text = new List<string> { MDVUtility.ToStr(lstEncounterDiagnosic[0]["VisitDate"]) == ""
                                            ? "-"
                                            : Convert.ToDateTime(lstEncounterDiagnosic[0]["VisitDate"]).ToString("MM/dd/yyyy")};

                td = new StrucDocTd();
                tr.Items.Add(td);
                td.Items = new List<object>();
                td.Items.Add(new StrucDocList
                {
                    item = new List<StrucDocItem> { new StrucDocItem {
                        Items = new List<object> {
                            new StrucDocContent
                            {
                                styleCode="italics",
                                Text = new List<string> { objCcdaModel.lstNoteData.Count > 0 ? objCcdaModel.lstNoteData[0]["EncounterLocation"] : "" }
                            }
                        }
                    } }
                });
                var Triggers = new List<StrucDocTr>();
                // Bind Trigger Rows
                foreach (var item in lstEncounterDiagnosic)
                {

                    if (item["IsTriggered"].ToLower() == "true" || item["IsTriggered"].ToLower() == "1")
                    {
                        var row = new StrucDocTr
                        {
                            Items = new List<object>
                                     {
                                             new StrucDocTd
                                             {
                                               Text = new List<string> {"Diagnosis" }
                                             },new StrucDocTd
                                             {
                                               Text = new List<string> { item["ProblemName"] }
                                             },new StrucDocTd
                                             {
                                               Text = new List<string> { item["SNOMEDID"] }
                                             },new StrucDocTd
                                             {
                                               Text = new List<string> { "SNOMED CT" }
                                             },new StrucDocTd
                                             {
                                               Text = new List<string> { "2.16.840.1.114222.4.11.7508" }
                                             },new StrucDocTd
                                             {
                                               Text = new List<string> { "19/05/2016" }
                                             },new StrucDocTd
                                             {
                                               Text = new List<string> { Convert.ToDateTime(item["StartDate"]).ToString("MM/dd/yyyy")  }
                                             },
                                     }
                        };
                        Triggers.Add(row);
                    }


                }

                // Bind non Trigger Rows
                var nonTriggers = new List<StrucDocTr>();
                foreach (var item in lstEncounterDiagnosic)
                {

                    if (item["IsTriggered"].ToLower() != "true" && item["IsTriggered"].ToLower() != "1")
                    {
                        var row = new StrucDocTr
                        {
                            Items = new List<object>
                                     {
                                             new StrucDocTd
                                             {
                                               Text = new List<string> {"Diagnosis" }
                                             },new StrucDocTd
                                             {
                                               Text = new List<string> { item["ProblemName"] }
                                             },new StrucDocTd
                                             {
                                               Text = new List<string> { Convert.ToDateTime(item["StartDate"]).ToString("MM/dd/yyyy") }
                                             },
                                     }
                        };
                        nonTriggers.Add(row);
                    }


                }
                // Second TR


                var InnerTr = new StrucDocTr
                {
                    Items = new List<object>
                                {
                                    new StrucDocTd
                                    {
                                        colspan = "20",
                                        Items = new List<object>
                                        {
                                           new StrucDocList
                                           {
                                               item = new List<StrucDocItem>
                                               {
                                                   new StrucDocItem
                                                   {
                                                       Items = new List<object>
                                                       {
                                                           new StrucDocTable
                                                           {
                                                               Items = new List<object> ( getCols(9) ),
                                                               thead = new StrucDocThead
                                                               {
                                                                   tr = new List<StrucDocTr>
                                                                   {
                                                                       new StrucDocTr
                                                                       {
                                                                         Items = new List<object>
                                                                         {
                                                                             new StrucDocTh
                                                                             {
                                                                                 Text = new List<string> { "Encounter Diagnosis Type" }
                                                                             }
                                                                         }
                                                                       }
                                                                   }
                                                               },
                                                               tbody = new List<StrucDocTbody>
                                                               {
                                                                   new StrucDocTbody
                                                                   {
                                                                       tr = new List<StrucDocTr>
                                                                       {
                                                                           new StrucDocTr
                                                                           {
                                                                               Items = new List<object>
                                                                               {
                                                                                   new StrucDocTd
                                                                                   {
                                                                                       Text = new List<string> { "Diagnosis" }
                                                                                   }
                                                                               }
                                                                           },
                                                                           new StrucDocTr
                                                                           {
                                                                               Items = new List<object>
                                                                               {
                                                                                   new StrucDocTd
                                                                                   {
                                                                                       colspan = "20",
                                                                                       Items = new List<object>
                                                                                       {
                                                                                           new StrucDocList
                                                                                           {
                                                                                               item = new List<StrucDocItem> {
                                                                                                   new StrucDocItem {
                                                                                                       Items = new List<object>
                                                                                                       {
                                                                                                           new StrucDocTable
                                                                                                           {
                                                                                                               thead = new StrucDocThead {
                                                                                                                   tr = new List<StrucDocTr>
                                                                                                                   {
                                                                                                                       new StrucDocTr
                                                                                                                       {
                                                                                                                            Items = new List<object> {
                                                                                                                                new StrucDocTh
                                                                                                                                {
                                                                                                                                        Text = new List<string> { "Initial Case Report Trigger Code Problem Observation " }
                                                                                                                                },
                                                                                                                                new StrucDocTh
                                                                                                                                {
                                                                                                                                        Text = new List<string> { "Problem" }
                                                                                                                                },new StrucDocTh
                                                                                                                                {
                                                                                                                                        Text = new List<string> { "Trigger Code" }
                                                                                                                                },new StrucDocTh
                                                                                                                                {
                                                                                                                                        Text = new List<string> { "Trigger Code codeSystem" }
                                                                                                                                },new StrucDocTh
                                                                                                                                {
                                                                                                                                        Text = new List<string> { "RCTC OID" }
                                                                                                                                },new StrucDocTh
                                                                                                                                {
                                                                                                                                        Text = new List<string> { "RCTC Version" }
                                                                                                                                },new StrucDocTh
                                                                                                                                {
                                                                                                                                        Text = new List<string> { "Date(s)" }
                                                                                                                                }

                                                                                                                           }
                                                                                                                       }
                                                                                                                   }
                                                                                                               },
                                                                                                               tbody = new List<StrucDocTbody>
                                                                                                               {
                                                                                                                   new StrucDocTbody
                                                                                                                   {
                                                                                                                        tr = Triggers
                                                                                                                   }
                                                                                                               }
                                                                                                           }
                                                                                                           ,
                                                                                                           new StrucDocTable
                                                                                                           {
                                                                                                               Items = new List<object>( getCols(9)),
                                                                                                               thead = new StrucDocThead {
                                                                                                                   tr = new List<StrucDocTr>
                                                                                                                   {
                                                                                                                       new StrucDocTr
                                                                                                                       {
                                                                                                                            Items = new List<object> {
                                                                                                                                new StrucDocTh
                                                                                                                                {
                                                                                                                                        Text = new List<string> { "Encounter Diagnosis Type" }
                                                                                                                                }


                                                                                                                           }
                                                                                                                       }
                                                                                                                   }
                                                                                                               },
                                                                                                               tbody = new List<StrucDocTbody>
                                                                                                               {
                                                                                                                   new StrucDocTbody
                                                                                                                   {
                                                                                                                        tr = new List<StrucDocTr>
                                                                                                                        {
                                                                                                                                new StrucDocTr
                                                                                                                                {
                                                                                                                                        Items = new List<object>
                                                                                                                                        {
                                                                                                                                                new StrucDocTd
                                                                                                                                                {
                                                                                                                                                        Text = new List<string> {"Diagnosis" }
                                                                                                                                                }
                                                                                                                                        }
                                                                                                                                },
                                                                                                                                new StrucDocTr
                                                                                                                                {
                                                                                                                                        Items = new List<object>
                                                                                                                                        {
                                                                                                                                                new StrucDocTd
                                                                                                                                                {
                                                                                                                                                        colspan = "20",
                                                                                                                                                        Items = new List<object>
                                                                                                                                                        {
                                                                                                                                                                new StrucDocList
                                                                                                                                                                {
                                                                                                                                                                        styleCode = "none",
                                                                                                                                                                        item = new List<StrucDocItem>
                                                                                                                                                                        {
                                                                                                                                                                                new StrucDocItem
                                                                                                                                                                                {
                                                                                                                                                                                        Items = new List<object>
                                                                                                                                                                                        {
                                                                                                                                                                                                new StrucDocTable
                                                                                                                                                                                                {
                                                                                                                                                                                                    Items = new List<object> (getCols(9)),
                                                                                                                                                                                                        thead = new StrucDocThead
                                                                                                                                                                                                        {
                                                                                                                                                                                                                tr = new List<StrucDocTr>
                                                                                                                                                                                                                {
                                                                                                                                                                                                                      new StrucDocTr
                                                                                                                                                                                                                      {
                                                                                                                                                                                                                            Items = new List<object>
                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                    new StrucDocTh
                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                            Text = new List<string> { "Problem Type" }
                                                                                                                                                                                                                                    },new StrucDocTh
                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                            Text = new List<string> { "Problem" }
                                                                                                                                                                                                                                    },new StrucDocTh
                                                                                                                                                                                                                                    {
                                                                                                                                                                                                                                            Text = new List<string> { "Date(s)" }
                                                                                                                                                                                                                                    },
                                                                                                                                                                                                                            }
                                                                                                                                                                                                                      }
                                                                                                                                                                                                                }
                                                                                                                                                                                                        },
                                                                                                                                                                                                        tbody = new List<StrucDocTbody>
                                                                                                                                                                                                        {
                                                                                                                                                                                                                new StrucDocTbody
                                                                                                                                                                                                                {
                                                                                                                                                                                                                     tr = nonTriggers
                                                                                                                                                                                                                }
                                                                                                                                                                                                        }
                                                                                                                                                                                                }
                                                                                                                                                                                        }
                                                                                                                                                                                }
                                                                                                                                                                        }
                                                                                                                                                                }
                                                                                                                                                        }
                                                                                                                                                }
                                                                                                                                        }
                                                                                                                                }
                                                                                                                        }
                                                                                                                   }
                                                                                                               }
                                                                                                           }

                                                                                                       }
                                                                                                   }
                                                                                               }
                                                                                           }
                                                                                       }
                                                                                   }
                                                                               }
                                                                           }
                                                                       }

                                                                   }
                                                               }
                                                           }
                                                       }
                                                   }
                                               }
                                           }

                                        }
                                    }
                        }
                };

                tBody.tr.Add(InnerTr);

                encountersComponent.section.entry = new List<Entry>();
                int lRowNo = 0;
                foreach (var dr in lstEncounterDiagnosic)
                {
                    var encounterActivitiesEntry = new Entry();
                    encountersComponent.section.entry.Add(encounterActivitiesEntry);
                    var encounterActivities = new Encounter();
                    encounterActivitiesEntry.Item = encounterActivities;
                    encounterActivities.classCode = "ENC";
                    encounterActivities.moodCode = x_DocumentEncounterMood.EVN;
                    encounterActivities.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.49",extension="2015-08-01" },
                        new II { root = "2.16.840.1.113883.10.20.22.4.49" }
                    };
                    encounterActivities.id = new List<II>
                    {
                        new II
                        {
                            root = StaticCcdaData["UniqueIdentifierID"]
                        }
                    };
                    encounterActivities.code = new CD
                    {
                        code = "99213",
                        codeSystem = "2.16.840.1.113883.6.12",
                        codeSystemName = "CPT-4",
                        displayName = "Office outpatient visit",
                        originalText = new ED
                        {
                            reference = new TEL { value = string.Concat("#Encounter", ++lRowNo) }
                        },
                        translation = new List<CD>
                        {
                            new CD
                            {
                                code = "AMB",
                                codeSystem = "2.16.840.1.113883.5.4",
                                codeSystemName = "HL7 ActEncounterCode",
                                displayName = "Ambulatory",
                            }
                        }
                    };
                    var date = HelperMethods.SetDateTime(Convert.ToDateTime(dr["VisitDate"]));
                    if (!string.IsNullOrWhiteSpace(date))
                    {
                        encounterActivities.effectiveTime = new IVL_TS
                        {
                            value = date
                        };
                    }
                    else
                    {
                        encounterActivities.effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[]
                            {
                                ItemsChoiceType2.low,
                                ItemsChoiceType2.high
                            },
                            Items = new QTY[]
                            {
                                new IVXB_TS { nullFlavor = "UNK" },
                                new IVXB_TS { nullFlavor = "UNK" }
                            }
                        };
                    }
                    encounterActivities.performer = new List<Performer2> {
                            new Performer2
                            {
                                assignedEntity= new AssignedEntity
                                {
                                        id = new List<II>()
                                        {
                                            new II {root = StaticCcdaData["ProviderOrganizationID"],extension="99999999"}
                                        },
                                        code=new CE
                                        {
                                            code = "59058001",
                                            codeSystem = "2.16.840.1.113883.6.96",
                                            codeSystemName = "SNOMED-CT",
                                            displayName = "General Physician",
                                        }

                                    }

                            }
                    };
                    encounterActivities.participant = new List<Participant2> {
                        new Participant2
                        {
                            typeCode="LOC",
                            participantRole= new ParticipantRole
                            {
                                classCode="SDLOC",
                                templateId=new List<II>
                                {
                                    new II
                                    {
                                        root = "2.16.840.1.113883.10.20.22.4.32"
                                    }
                                },
                                code=new CE
                                {
                                    code = "1160-1",
                                    codeSystem = "2.16.840.1.113883.6.259",
                                    codeSystemName = "HL7 HealthcareServiceLocation",
                                    displayName = "Urgent Care Center",

                                },
                               addr = new List<AD>
                                {
                                    HelperMethods.SetAddress("WP", new List<string>() { objProviderData["WorkAddress"] }, objProviderData["City"], objProviderData["State"], objProviderData["ZIPCode"], "US")
                                },
                                telecom = new List<TEL>
                                {
                                    HelperMethods.SetTelephone(objProviderData["PhoneNo"], "WP",objProviderData["PhoneExt"])
                                },
                                Item = new PlayingEntity
                                {
                                    classCode="PLC",
                                     name = new List<PN> { new PN { Text = new List<string> { objPracticeData["ShortName"] } } }
                                }

                            }


                        }
                    };
                    encounterActivities.entryRelationship = new List<EntryRelationship>();
                    var encounterDiagnosisEr = new EntryRelationship { typeCode = x_ActRelationshipEntryRelationship.SUBJ };
                    encounterActivities.entryRelationship.Add(encounterDiagnosisEr);
                    var encounterDiagnosis = new Act();
                    encounterDiagnosisEr.Item = encounterDiagnosis;
                    encounterDiagnosis.classCode = x_ActClassDocumentEntryAct.ACT;
                    encounterDiagnosis.moodCode = x_DocumentActMood.EVN;
                    encounterDiagnosis.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.80" ,extension="2015-08-01"},
                        new II { root = "2.16.840.1.113883.10.20.22.4.19"}

                    };
                    encounterDiagnosis.id = new List<II>
                    {
                     new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };
                    encounterDiagnosis.code = new CD
                    {
                        code = "29308-4",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC",
                        displayName = "Encounter Diagnosis"
                    };
                    var startDate = MDVUtility.ToStr(dr["VisitDate"]);
                    var dtStartDate = DateTime.Now;
                    if (!string.IsNullOrWhiteSpace(startDate))
                        dtStartDate = Convert.ToDateTime(startDate);
                    string lowTime = HelperMethods.SetDateTime(dtStartDate);
                    if (!string.IsNullOrWhiteSpace(lowTime))
                    {
                        encounterDiagnosis.effectiveTime = new IVL_TS
                        {
                            Items = new QTY[1],
                            ItemsElementName = new ItemsChoiceType2[1]
                        };
                        encounterDiagnosis.effectiveTime.ItemsElementName[0] = ItemsChoiceType2.low;
                        encounterDiagnosis.effectiveTime.Items[0] = new IVXB_TS { value = lowTime };
                    }
                    encounterDiagnosis.entryRelationship = new List<EntryRelationship>();
                    var problemObservationEr = new EntryRelationship();
                    encounterDiagnosis.entryRelationship.Add(problemObservationEr);
                    problemObservationEr.typeCode = x_ActRelationshipEntryRelationship.SUBJ;
                    var observation = new Observation();
                    problemObservationEr.Item = observation;
                    observation.classCode = "OBS";
                    observation.moodCode = x_ActMoodDocumentObservation.EVN;
                    observation.templateId = new List<II>
                    {
                        new II { root = "2.16.840.1.113883.10.20.22.4.4",extension="2015-08-01"},
                        new II { root = "2.16.840.1.113883.10.20.22.4.4"}
                    };
                    observation.id = new List<II>
                    {
                        new II { root = StaticCcdaData["UniqueIdentifierID"] }
                    };
                    observation.code = new CD
                    {
                        code = "404684003",
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED CT",
                        displayName = "Finding",
                        translation = new List<CD>
                            {
                                new CD
                                {
                                    code = "75321-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "Clinical finding",
                                }
                            }
                    };
                    observation.statusCode = new CS
                    {
                        code = "completed"
                    };

                    if (!string.IsNullOrWhiteSpace(lowTime))
                    {
                        observation.effectiveTime = new IVL_TS
                        {
                            Items = new QTY[1],
                            ItemsElementName = new ItemsChoiceType2[1]
                        };
                        observation.effectiveTime.ItemsElementName[0] = ItemsChoiceType2.low;
                        observation.effectiveTime.Items[0] = new IVXB_TS { value = lowTime };
                    }
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr["SNOMEDID"])))
                    {
                        observation.value = new List<ANY>
                        {
                            new CD
                            {
                                code = MDVUtility.ToStr(dr["SNOMEDID"]),
                                codeSystem = "2.16.840.1.113883.6.96",
                                displayName = MDVUtility.ToStr(dr["ProblemShortName"])
                            }
                        };
                    }
                    else
                    {
                        observation.value = new List<ANY>
                       {
                           new CD
                           {
                               nullFlavor = "UNK"
                           }
                       };
                    }
                }
            }
            else
            {
                text.Items.Add(new StrucDocContent { ID = "noEncounter", Text = new List<string> { "Not included in document" } });
                encountersComponent.section.entry = new List<Entry>();
                var encounterActivitiesEntry = new Entry();
                encountersComponent.section.entry.Add(encounterActivitiesEntry);
                var encounterActivities = new Encounter();
                encounterActivitiesEntry.Item = encounterActivities;
                encounterActivities.classCode = "ENC";
                encounterActivities.moodCode = x_DocumentEncounterMood.EVN;
                encounterActivities.templateId = new List<II>
                {
                    new II { root = "2.16.840.1.113883.10.20.22.4.49",extension="2015-08-01" },
                    new II { root = "2.16.840.1.113883.10.20.22.4.49" }
                };
                encounterActivities.id = new List<II>
                {
                    new II
                    {
                       root = StaticCcdaData["UniqueIdentifierID"]
                    }
                };
                encounterActivities.nullFlavor = "NI";
                encounterActivities.effectiveTime = new IVL_TS
                {
                    value = "UNK"
                };
                encounterActivities.effectiveTime = new IVL_TS
                {
                    ItemsElementName = new[]
                    {
                        ItemsChoiceType2.low,
                        ItemsChoiceType2.high
                    },
                    Items = new QTY[]
                    {
                        new IVXB_TS { nullFlavor = "UNK" },
                        new IVXB_TS { nullFlavor = "UNK" }
                    }
                };
            }
            sb.component.Add(encountersComponent);
        }

        private static SubstanceAdministration SetMedicationActivity(Dictionary<string, string> dr, string practice, string referenceValue = "")
        {
            var medicationActivity = new SubstanceAdministration
            {
                classCode = "SBADM",
                moodCode = x_DocumentSubstanceMood.EVN,
                templateId = new List<II>
                {
                    new II {root = "2.16.840.1.113883.10.20.22.4.16",extension= "2014-06-09"},
                    new II {root = "2.16.840.1.113883.10.20.22.4.16"}
                },
                id = new List<II>
                {
                    new II {root = StaticCcdaData["UniqueIdentifierID"]}
                },
                text = new ED
                {
                    Text = new List<string> { dr["MedicationName"] }
                },
                statusCode = dr["Status"].ToLower() == "active" ? new CS { code = "completed" } : new CS { code = "suspended" }
            };

            var startDate = DateTime.MinValue;
            if (dr["StartDate"] != string.Empty)
                startDate = Convert.ToDateTime(dr["StartDate"]);
            var endDate = DateTime.MinValue;
            if (dr["EndDate"] != string.Empty)
                endDate = Convert.ToDateTime(dr["EndDate"]);

            var lowEffectiveTime = Convert.ToDateTime(startDate);
            var highEffectiveTime = Convert.ToDateTime(endDate);
            medicationActivity.effectiveTime = new List<SXCM_TS>();
            var effectiveTime = new IVL_TS
            {
                ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high, },
                Items = new QTY[2]
            };
            if (lowEffectiveTime != DateTime.MinValue)
                effectiveTime.Items[0] = new IVXB_TS { value = lowEffectiveTime.ToString("yyyyMMdd") };
            else
                effectiveTime.Items[0] = new IVXB_TS { nullFlavor = "UNK" };
            if (highEffectiveTime != DateTime.MinValue)
                effectiveTime.Items[1] = new IVXB_TS { value = highEffectiveTime.ToString("yyyyMMdd") };
            else
                effectiveTime.Items[1] = new IVXB_TS { nullFlavor = "UNK" };
            medicationActivity.effectiveTime.Add(effectiveTime);
            if (!string.IsNullOrWhiteSpace(dr["NoOfTimes"]) && !string.IsNullOrWhiteSpace(dr["FrequencyDescription"]))
            {
                var noOfTime = dr["NoOfTimes"];
                var frequencyDescription = dr["FrequencyDescription"];
                if (frequencyDescription.Contains("hour"))
                    frequencyDescription = "h";
                else if (frequencyDescription.Contains("week"))
                    frequencyDescription = "w";
                else if (frequencyDescription.Contains("month"))
                    frequencyDescription = "m";
                else
                    frequencyDescription = "d";
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
                refill = "0";
            else
                refill = dr["Refill"];

            if (!string.IsNullOrEmpty(refill))
                medicationActivity.repeatNumber = new IVL_INT { value = refill };

            if (!string.IsNullOrWhiteSpace(dr["RouteCode"])
                && !string.IsNullOrWhiteSpace(dr["RouteDescription"]))
                medicationActivity.routeCode = new CE
                {
                    code = dr["RouteCode"],
                    codeSystem = "2.16.840.1.113883.3.26.1.1",
                    codeSystemName = "NCI Thesaurus",
                    displayName = dr["RouteDescription"]
                };

            if (!string.IsNullOrWhiteSpace(dr["Dose"]))
                medicationActivity.doseQuantity = new IVL_PQ { value = dr["Dose"].TrimEnd('0', '.') , unit = dr["DoseUnit"] };
            var consumable = new Consumable();
            medicationActivity.consumable = consumable;
            var medicationInfo = new ManufacturedProduct();
            consumable.manufacturedProduct = medicationInfo;
            medicationInfo.classCodeSpecified = true;
            medicationInfo.classCode = RoleClassManufacturedProduct.MANU;
            medicationInfo.templateId = new List<II>() { new II { root = "2.16.840.1.113883.10.20.22.4.23", extension = "2014-06-09" }, new II { root = "2.16.840.1.113883.10.20.22.4.23" } };
            var manufacturedMaterial = new Material();
            medicationInfo.Item = manufacturedMaterial;

            if (!string.IsNullOrWhiteSpace(dr["RxnormID"]))
            {
                manufacturedMaterial.code = new CE
                {
                    code = dr["RxnormID"],
                    codeSystem = "2.16.840.1.113883.6.88",
                    displayName = dr["MedicationName"]
                };
            }
            else
            {
                manufacturedMaterial.code = new CE
                {
                    nullFlavor = "NI"
                };
            }

            medicationActivity.performer = new List<Performer2>();
            var performer2 = new Performer2
            {
                assignedEntity = new AssignedEntity
                {
                    id = new List<II> { new II { nullFlavor = "UNK" } },
                    addr = new List<AD>
                    {
                        new AD {nullFlavor = "UNK"}
                    },
                    telecom = new List<TEL>()
                    {
                        new TEL {nullFlavor = "UNK"}
                    }
                }
            };
            var organization = new Organization
            {
                id = new List<II>
                {
                    new II
                    {
                        root = StaticCcdaData["UniqueIdentifierID"]
                    }
                },
                name = new List<ON>()
                {
                    new ON
                    {
                        Text = new List<string> {practice}
                    }
                },
                telecom = new List<TEL>()
                {
                    new TEL {nullFlavor = "UNK"}
                },
                addr = new List<AD>
                {
                    new AD
                    {
                        nullFlavor = "UNK"
                    }
                }
            };
            performer2.assignedEntity.representedOrganization = organization;
            medicationActivity.performer.Add(performer2);
            return medicationActivity;
        }
        private static void AddLabOrderTestComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var component = HelperMethodsCaseReports.IntializeComponent3(
            new List<string> { "2.16.840.1.113883.10.20.22.2.10" },
            "18776-5", "2.16.840.1.113883.6.1", "LOINC", "Plan of Care", "Plan of Treatment");
            var text = component.section.text;
            var lstLabOrderTests = objCcdaModel.lstLabOrderTests ?? new List<Dictionary<string, string>>();
            if (objCcdaModel.lstLabOrderTests != null && lstLabOrderTests.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);
                table.Items = new List<object>(getCols(9));
                table.border = "1";
                table.width = "100%";
                
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                var headTr = new StrucDocTr();
                table.thead.tr.Add(headTr);
                headTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Initial Case Report Trigger Code Lab Test Order"}},
                    new StrucDocTh {Text = new List<string> {"Trigger Code"}},
                    new StrucDocTh {Text = new List<string> {"Trigger Code codeSystem"}},
                    new StrucDocTh {Text = new List<string> {"RCTC OID"}},
                    new StrucDocTh {Text = new List<string> {"RCTC Version"}},
                    new StrucDocTh {Text = new List<string> {"Ordered Date"}},
                };
                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);

                tbody.tr = new List<StrucDocTr>();
                int lRowNo = 1;
                foreach (Dictionary<string, string> dr in objCcdaModel.lstLabOrderTests)
                {
                    var bodyTr = new StrucDocTr
                    {
                        ID = string.Concat("labOrderTest", lRowNo++),
                        Items = new List<object>
                            {
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["Name"])
                                    }
                                },
                                 new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["CPTCode"])
                                    }
                                },
                                                                  new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["CodeSystem"])
                                    }
                                },
                                      new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        "2.16.840.1.114222.4.11.7508"
                                    }
                                },
                                                                            new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        "19/05/2016"
                                    }
                                },
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["TestDate"]) == ""
                                            ? "-"
                                            : Convert.ToDateTime(dr["TestDate"]).ToString("MM/dd/yyyy")
                                    }
                                }

                            }
                    };
                    tbody.tr.Add(bodyTr);
                }
                component.section.entry = new List<Entry>();
                lRowNo = 1;
                foreach (var procDictionary in lstLabOrderTests)
                {
                    var drItem = procDictionary;
                    var obsevationEntry = new Entry();
                    component.section.entry.Add(obsevationEntry);
                    var observation = new Observation
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.INT,
                        templateId = new List<II>
                        {
                            new II {root = "2.16.840.1.113883.10.20.22.4.44"}
                        },
                        id = new List<II>
                        {
                            new II
                            {
                                root = Guid.NewGuid().ToString()//StaticCcdaData["UniqueIdentifierID"]
                            }
                        }
                    };
                    if (!string.IsNullOrWhiteSpace(drItem["CPTCode"]))
                    {
                        observation.code = new CD
                        {
                            code = drItem["CPTCode"],
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC",
                            displayName = drItem["CPTCodeDescription"],
                            originalText = new ED
                            {
                                reference = new TEL { value = string.Concat("#labOrderTest", lRowNo) }
                            },
                            valueSet = "2.16.840.1.114222.4.11.7508"
                            ,
                            valueSetVersion = "19/05/2016"
                        };
                    }
                    observation.text = new ED
                    {
                        reference = new TEL { value = string.Concat("#labOrderTest", lRowNo) }
                    };
                    observation.statusCode = new CS
                    {
                        code = "active"
                    };
                    var procedureDate = procDictionary["TestDate"];
                    if (!string.IsNullOrWhiteSpace(procedureDate))
                    {
                        procedureDate = Convert.ToDateTime(procedureDate).ToString("yyyyMMdd");
                    }
                    observation.effectiveTime = new IVL_TS
                    {
                        value = procedureDate
                    };
                    obsevationEntry.Item = observation;
                    lRowNo++;
                }

            }
            else
            {
                if (lstLabOrderTests.Count == 0)
                {
                    text.Items.Add(new StrucDocContent
                    {
                        ID = "noPlanOfTreatment",
                        Text = new List<string> { "Not included in document" }
                    });
                }
            }

            sb.component.Add(component);
        }
        private static void AddImplantedDeviceComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var component = HelperMethodsCaseReports.IntializeComponent3(
            new List<string> { "2.16.840.1.113883.10.20.22.2.23", "2.16.840.1.113883.10.20.22.2.23" },
            "46264-8", "2.16.840.1.113883.6.1", "LOINC", "Medical Equipment", "Implants");
            component.section.templateId[0].extension = "2014-06-09";
            var text = component.section.text;
            var lstImplantableDevice = objCcdaModel.lstImplantableDevice ?? new List<Dictionary<string, string>>();
            if (objCcdaModel.lstImplantableDevice != null && lstImplantableDevice.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);
                table.border = "1";
                table.width = "100%";
                // table.caption = new StrucDocCaption { Text = new List<string> { "Planned Observation" } };
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                var headTr = new StrucDocTr();
                table.thead.tr.Add(headTr);
                headTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Implanted"}},
                    new StrucDocTh {Text = new List<string> {"Area"}},
                //    new StrucDocTh {Text = new List<string> {"Manufacturer"}},
                    new StrucDocTh {Text = new List<string> {"UDI"}},
                    //new StrucDocTh {Text = new List<string> {"Details"}},
                    //new StrucDocTh {Text = new List<string> {"Model"}},
                    //new StrucDocTh {Text = new List<string> {"Serial"}},
                    //new StrucDocTh {Text = new List<string> {"Lot"}},
                };
                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);

                tbody.tr = new List<StrucDocTr>();
                int lRowNo = 1;
                foreach (Dictionary<string, string> dr in objCcdaModel.lstImplantableDevice)
                {
                    var bodyTr = new StrucDocTr
                    {
                        ID = string.Concat("implant", lRowNo++),
                        Items = new List<object>
                            {
                            new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["Implanted"])
                                    }
                                },
                            new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["Area"])
                                    }
                                },
                            //new StrucDocTd
                            //    {
                            //        Text = new List<string>
                            //        {
                            //            MDVUtility.ToStr(dr["Manufacturer"])
                            //        }
                            //    },
                            new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["UDI"])
                                    }
                                },
                            //new StrucDocTd
                            //    {
                            //        Text = new List<string>
                            //        {
                            //            MDVUtility.ToStr(dr["Details"])
                            //        }
                            //    },
                            //new StrucDocTd
                            //    {
                            //        Text = new List<string>
                            //        {
                            //            MDVUtility.ToStr(dr["Model"])
                            //        }
                            //    },
                            //new StrucDocTd
                            //    {
                            //        Text = new List<string>
                            //        {
                            //            MDVUtility.ToStr(dr["Serial"])
                            //        }
                            //    },
                            //new StrucDocTd
                            //    {
                            //        Text = new List<string>
                            //        {
                            //            MDVUtility.ToStr(dr["Lot"])
                            //        }
                            //    },
                            }
                    };
                    tbody.tr.Add(bodyTr);
                }
                component.section.entry = new List<Entry>();
                lRowNo = 1;
                foreach (var procDictionary in lstImplantableDevice)
                {
                    var drItem = procDictionary;
                    var obsevationEntry = new Entry();
                    component.section.entry.Add(obsevationEntry);
                    var observation = new Procedure
                    {
                        classCode = "PROC",
                        moodCode = x_DocumentProcedureMood.EVN,
                        templateId = new List<II>
                        {
                            new II {root = "2.16.840.1.113883.10.20.22.4.14", extension="2014-06-09"}
                        },
                        id = new List<II>
                        {
                            new II
                            {
                                extension="2744",
                                root = "1.2.840.114350.1.13.5552.1.7.2.737780"
                            }
                        }
                    };
                    observation.code = new CE { nullFlavor = "UNK" };
                    observation.statusCode = new CS { code = "completed" };
                    var lowTime = procDictionary["Manufacturing_Date"];
                    if (!string.IsNullOrWhiteSpace(lowTime))
                    {
                        lowTime = Convert.ToDateTime(lowTime).ToString("yyyyMMdd");
                    }
                    if (!string.IsNullOrWhiteSpace(lowTime))
                    {
                        observation.effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[]
                            {
                            ItemsChoiceType2.low,
                            },
                            Items = new QTY[]
                            {
                            new IVXB_TS { value = lowTime},
                            }
                        };
                    }
                    observation.targetSiteCode = new List<CD>
                    {
                        new CD
                        {
                            code = "72696002", // Get and set the codes here
                            codeSystem = "2.16.840.1.113883.6.96",
                            codeSystemName = "SNOMED CT",
                            displayName = "Structure of left knee",

                         }
                    };
                    observation.participant = new List<Participant2> {
                        new Participant2
                        {
                            typeCode="DEV",
                            participantRole=new ParticipantRole
                            {
                                classCode="MANU",
                                templateId=new List<II>
                                {
                                    new II
                                    {
                                        root = "2.16.840.1.113883.10.20.22.4.37"
                                    }
                                },
                                id=new List<II>
                                {
                                    new II
                                    {
                                        assigningAuthorityName="FDA",
                                        extension=drItem["UDI"],//"(01)00848486001032(11)160330(10)ABC124",
                                        root="2.16.840.1.113883.3.3719"
                                    }

                                },
                                Item= new Device
                                {
                                    code=new CE
                                    {
                                        nullFlavor="UNK",
                                        originalText=new ED
                                        {
                                            reference = new TEL { value = string.Concat("#implant", lRowNo++) }
                                        }
                                    }
                                },
                                scopingEntity=new Entity
                                {
                                    id=new List<II>
                                    {
                                         new II
                                        {
                                            root="2.16.840.1.113883.3.3719",
                                        }
                                    }
                                }

                            }


                        }
                    };
                    obsevationEntry.Item = observation;
                    lRowNo++;
                }

            }
            else
            {
                if (lstImplantableDevice.Count == 0)
                {
                    text.Items.Add(new StrucDocContent
                    {
                        ID = "noImplant",
                        Text = new List<string> { "Not included in document" }
                    });
                }
            }

            sb.component.Add(component);
        }
        private static void AddMentalStatusComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var component = HelperMethodsCaseReports.IntializeComponent3(
            new List<string> { "2.16.840.1.113883.10.20.22.2.56" },
            "10190-7", "2.16.840.1.113883.6.1", "LOINC", "MENTAL STATUS", "MENTAL STATUS");
            var text = component.section.text;

            component.section.templateId[0].extension = "2015-08-01";
            var lstMentalStatus = objCcdaModel.lstMentalStatus ?? new List<Dictionary<string, string>>();
            if (objCcdaModel.lstMentalStatus != null && lstMentalStatus.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);
                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                var headTr = new StrucDocTr();
                table.thead.tr.Add(headTr);
                headTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Status"}},
                    new StrucDocTh {Text = new List<string> {"Date"}},
                };
                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);

                tbody.tr = new List<StrucDocTr>();
                int lRowNo = 1;
                foreach (Dictionary<string, string> dr in objCcdaModel.lstMentalStatus)
                {
                    var bodyTr = new StrucDocTr
                    {
                        Items = new List<object>
                            {
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["SNOMEDDescription"])
                                    }
                                },
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["CreatedOn"]) == ""
                                            ? "-"
                                            : Convert.ToDateTime(dr["CreatedOn"]).ToString("MM/dd/yyyy")
                                    }
                                },
                            }
                    };
                    tbody.tr.Add(bodyTr);
                }
                component.section.entry = new List<Entry>();
                lRowNo = 1;
                foreach (var procDictionary in lstMentalStatus)
                {
                    var drItem = procDictionary;
                    var obsevationEntry = new Entry();
                    component.section.entry.Add(obsevationEntry);
                    var observation = new Observation
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>
                        {
                            new II {root = "2.16.840.1.113883.10.20.22.4.74",extension="2015-08-01"},
                            new II {root = "2.16.840.1.113883.10.20.22.4.74"}
                        },
                        id = new List<II>
                        {
                            new II
                            {
                                root = Guid.NewGuid().ToString()
                            }
                        }
                    };

                    observation.code = new CD
                    {
                        code = "373930000",
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED-CT",
                        displayName = "Cognitive function",
                        translation = new List<CD>
                            {
                                new CD
                                {
                                    code = "75275-8",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "Cognitive function",
                                }
                            }
                    };
                    observation.statusCode = new CS
                    {
                        code = "completed"
                    };
                    var createdDate = procDictionary["CreatedOn"];
                    if (!string.IsNullOrWhiteSpace(createdDate))
                    {
                        createdDate = Convert.ToDateTime(createdDate).ToString("yyyyMMdd");
                        var effectiveTime = new IVL_TS()
                        {
                            ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                            Items = new QTY[]
                           {
                                new IVXB_TS()
                                {
                                    value =createdDate
                                },
                                new IVXB_TS()
                                {
                                    nullFlavor = "UNK"
                                }
                           }
                        };
                        observation.effectiveTime = effectiveTime;
                    }
                    else
                    {
                        var effectiveTime = new IVL_TS()
                        {
                            ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                            Items = new QTY[]
                               {
                                new IVXB_TS()
                                {
                                    value =createdDate
                                },
                                new IVXB_TS()
                                {
                                    nullFlavor = "UNK"
                                }
                               }
                        };
                        observation.effectiveTime = effectiveTime;
                    }

                    if (!string.IsNullOrWhiteSpace(drItem["SNOMEDID"]))
                    {
                        observation.value = new List<ANY>
                        {
                            new CD
                            {
                                    code = drItem["SNOMEDID"],
                                    codeSystem = "2.16.840.1.113883.6.96",
                                    codeSystemName = "SNOMED-CT",
                                    displayName =  drItem["SNOMEDDescription"],
                            }

                        };
                    }
                    else
                    {
                        observation.value = new List<ANY>
                        {
                            new CD
                            {
                                nullFlavor = "UNK"
                            }
                        };
                    }
                    obsevationEntry.Item = observation;
                    lRowNo++;
                }

            }
            else
            {
                if (lstMentalStatus.Count == 0)
                {
                    text.Items.Add(new StrucDocContent
                    {
                        ID = "noMentalStatus",
                        Text = new List<string> { "Not included in document" }
                    });
                }
            }

            sb.component.Add(component);
        }
        private static void AddFamilyHistoryComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            var familyHistoryComponent = HelperMethodsCaseReports.IntializeComponent3(
            new List<string> { "2.16.840.1.113883.10.20.22.2.15" },
            "10157-6", "2.16.840.1.113883.6.1", "LOINC", "Family History", "FAMILY HISTORY");
            var text = familyHistoryComponent.section.text;
            familyHistoryComponent.section.entry = new List<Entry>();
            var lstFamilyHx = objCcdaModel.lstFamilyHx;
            if (lstFamilyHx != null && lstFamilyHx.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);
                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                var headerTr = new StrucDocTr();
                table.thead.tr.Add(headerTr);
                headerTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Family History Element"}},
                    new StrucDocTh {Text = new List<string> {"Description"}},
                    new StrucDocTh {Text = new List<string> {"Effective Dates"}}
                };
                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);
                tbody.tr = new List<StrucDocTr>();
                var bodyTr = new StrucDocTr
                {
                    Items = new List<object>
                    {
                        new StrucDocTd
                        {
                            Items = new List<object> {new StrucDocContent {ID = "family"}},
                            Text = new List<string> {"Smoking Status"}
                        }
                    }
                };
                var description = lstFamilyHx[0]["Description"];
                bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { description } });
                var effectiveDate = lstFamilyHx[0]["FamilyHxDate"];
                if (!string.IsNullOrWhiteSpace(effectiveDate))
                {
                    effectiveDate = Convert.ToDateTime(effectiveDate).ToString("MM/dd/yyyy");
                }

                bodyTr.Items.Add(new StrucDocTd { Text = new List<string> { effectiveDate } });
                tbody.tr.Add(bodyTr);


                var smokingHistoryEntry = new Entry();
                familyHistoryComponent.section.entry.Add(smokingHistoryEntry);
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
                    originalText = new ED { reference = new TEL { value = "#family" } },
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
                familyHistoryComponent.section.entry.Add(smokingStatusEntry);
                var smokingStatusObservation = new Observation();
                smokingStatusEntry.Item = smokingStatusObservation;
                smokingStatusObservation.classCode = "OBS";
                smokingStatusObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                smokingStatusObservation.templateId = new List<II>()
                {
                    new II { root = "2.16.840.1.113883.10.20.22.4.78" }
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
                smokingStatusObservation.effectiveTime = new IVL_TS { value = DateTime.Now.ToString("yyyy") };
                smokingStatusObservation.value = new List<ANY>();
                var snomedid = lstFamilyHx[0]["SNOMEDID"];
                if (!string.IsNullOrWhiteSpace(snomedid))
                {
                    smokingStatusObservation.value.Add(new CD
                    {
                        code = snomedid,
                        codeSystem = "2.16.840.1.113883.6.96",
                        displayName = MDVUtility.ToStr(lstFamilyHx[0]["SmokingStatus"])
                    });
                }
                else
                {
                    smokingStatusObservation.value.Add(new CD
                    {
                        code = "266927001",
                        codeSystem = "2.16.840.1.113883.6.96",
                        displayName = "Unknown if ever smoked"
                    });
                }
            }
            else
            {
                var content = new StrucDocContent();
                text.Items.Add(content);
                content.Text = new List<string> { "Not included in document" };
                var smokingStatusEntry = new Entry();
                familyHistoryComponent.section.entry.Add(smokingStatusEntry);
                var smokingStatusObservation = new Observation();
                smokingStatusEntry.Item = smokingStatusObservation;
                smokingStatusObservation.classCode = "OBS";
                smokingStatusObservation.moodCode = x_ActMoodDocumentObservation.EVN;
                smokingStatusObservation.templateId = new List<II>()
                {
                    new II { root = "2.16.840.1.113883.10.20.22.4.78" }
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
                smokingStatusObservation.effectiveTime = new IVL_TS { value = DateTime.Now.ToString("yyyy") };
                smokingStatusObservation.value = new List<ANY>()
                {
                    new CD
                    {
                        nullFlavor = "NI"
                    }
                };
            }
            sb.component.Add(familyHistoryComponent);
        }
        private static void AddPayerSectionComponent(StructuredBody sb, CCDACaseReportDataModel objCcdaModel)
        {
            //Need to implement table value and entry
            var component = HelperMethodsCaseReports.IntializeComponent3(
            new List<string> { "2.16.840.1.113883.10.20.1.9", "2.16.840.1.113883.10.20.1.10" },
            "48768-6", "2.16.840.1.113883.6.1", "LOINC", "Payers", "Insurance Payers", "CCD");
            var text = component.section.text;
            if (component.section.templateId.Count > 1)
                component.section.templateId[1].assigningAuthorityName = "HL7 Security";

            var lstInsurance = objCcdaModel.lstInsurance ?? new List<Dictionary<string, string>>();
            if (objCcdaModel.lstInsurance != null && lstInsurance.Count > 0)
            {
                var table = new StrucDocTable();
                text.Items.Add(table);
                table.border = "1";
                table.width = "100%";
                table.thead = new StrucDocThead { tr = new List<StrucDocTr>() };
                var headTr = new StrucDocTr();
                table.thead.tr.Add(headTr);
                headTr.Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Payer Name/Contact"}},
                    new StrucDocTh {Text = new List<string> {"Priority"}},
                    new StrucDocTh {Text = new List<string> {"Group Id"}},
                    new StrucDocTh {Text = new List<string> {"Covered Party Id"}},
                    new StrucDocTh {Text = new List<string> {"Covered Party Name/Relationship"}},
                    new StrucDocTh {Text = new List<string> {"Covered Party Date of Birth"}},
                };
                table.tbody = new List<StrucDocTbody>();
                var tbody = new StrucDocTbody();
                table.tbody.Add(tbody);

                tbody.tr = new List<StrucDocTr>();
                foreach (Dictionary<string, string> dr in objCcdaModel.lstInsurance)
                {
                    string planAdress = "";
                    string partyNameRelation = "";
                    if (!string.IsNullOrWhiteSpace(dr["PlanName"]))
                    {
                        planAdress = MDVUtility.ToStr(dr["PlanName"]);
                    }
                    if (!string.IsNullOrWhiteSpace(dr["PlanAddress"]))
                    {
                        planAdress += Environment.NewLine + MDVUtility.ToStr(dr["PlanAddress"]);
                    }
                    if (!string.IsNullOrWhiteSpace(dr["State"]))
                    {
                        planAdress += Environment.NewLine + MDVUtility.ToStr(dr["State"]);
                    }
                    if (!string.IsNullOrWhiteSpace(dr["PhoneNo"]))
                    {
                        planAdress += Environment.NewLine + MDVUtility.ToStr(dr["PhoneNo"]);
                    }
                    if (!string.IsNullOrWhiteSpace(dr["PartyFullName"]))
                    {
                        partyNameRelation += MDVUtility.ToStr(dr["PartyFullName"]);
                    }
                    if (!string.IsNullOrWhiteSpace(dr["RelationShip"]))
                    {
                        partyNameRelation += "/" + MDVUtility.ToStr(dr["RelationShip"]);
                    }
                    var bodyTr = new StrucDocTr
                    {
                        Items = new List<object>
                            {
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                      planAdress
                                    }
                                },
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                       HelperMethodsCaseReports.GetPriority(dr["Priority"])
                                    }
                                },
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["GroupId"])
                                    }
                                },
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["SubscriberId"])
                                    }
                                },
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                       partyNameRelation
                                    }
                                },
                                new StrucDocTd
                                {
                                    Text = new List<string>
                                    {
                                        MDVUtility.ToStr(dr["DOB"])
                                    }
                                }
                            }
                    };
                    tbody.tr.Add(bodyTr);
                }
                #region Author
                string confidentialCode = "N";
                if (objCcdaModel.IsConfidential == "true")
                    confidentialCode = "R";
                component.section.confidentialityCode = new CE
                {
                    code = confidentialCode,
                    codeSystem = "2.16.840.1.113883.5.25",
                    codeSystemName = "HL7 Confidentiality"
                };
                var objProvider = objCcdaModel.lstProviderData[0];
                var objPracticeData = objCcdaModel.lstPracticeData[0];
                component.section.author = new List<Author>
                    {
                        new Author
                        {
                            templateId = new List<II>
                            {
                                new II
                                {
                                    root = "2.16.840.1.113883.3.3251.1.6",
                                    assigningAuthorityName = "HL7 Security"
                                }
                            },
                            time = new TS
                            {
                                value = DateTime.Now.ToString("yyyyMMdd")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                templateId = new List<II>
                                {
                                    new II
                                    {
                                        root ="2.16.840.1.113883.3.3251.1.7"
                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = Guid.NewGuid().ToString()
                                    }
                                },
                                addr = new List<AD>
                                {
                                    HelperMethodsCaseReports.SetAddress("WP", new List<string>() { objProvider["WorkAddress"] }, objProvider["City"], objProvider["State"], objProvider["ZIPCode"], "US")
                                },
                                telecom = new List<TEL>
                                {
                                    HelperMethodsCaseReports.SetTelephone(objProvider["PhoneNo"], "WP",objProvider["PhoneExt"])
                                },
                                Item = new Person
                                {
                                    name = new List<PN>
                                    {
                                        HelperMethodsCaseReports.SetName("L", objProvider["LastName"], new List<string> { objProvider["FirstName"], objProvider["MI"] })
                                    }
                                },
                                representedOrganization = new Organization
                                {
                                    name = new List<ON>
                                    {
                                       new ON
                                       {
                                           Text = new List<string>
                                           {
                                              objCcdaModel.lstPracticeData[0]["ShortName"]
                                           }
                                       }
                                    },
                                    telecom = new List<TEL>
                                    {
                                        HelperMethodsCaseReports.SetTelephone(objPracticeData["PhoneNo"], "WP",objPracticeData["PhoneExt"])
                                    },
                                    addr = new List<AD>
                                    {
                                        HelperMethodsCaseReports.SetAddress("WP", new List<string> { objPracticeData["Address"] }, objPracticeData["City"], objPracticeData["State"], objPracticeData["ZIPCode"], "US")
                                    }
                                }
                            }
                        }
                    };

                #endregion Author
                component.section.entry = new List<Entry>();

                var entry = new Entry();
                var concernAct = new Act
                {
                    classCode = x_ActClassDocumentEntryAct.ACT,
                    moodCode = x_DocumentActMood.EVN,
                    templateId = new List<II>
                        {
                            new II {root = "2.16.840.1.113883.10.20.1.20",assigningAuthorityName="CCD"}
                        },
                    id = new List<II>
                        {
                            new II {root = Guid.NewGuid().ToString()}
                        },
                    code = new CD
                    {
                        code = "LOINC",
                        codeSystem = "2.16.840.1.113883.6.1",
                        displayName = "48768-6"
                    },
                    statusCode = new CS
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS
                    {
                        ItemsElementName = new ItemsChoiceType2[2],
                        Items = new QTY[2]
                    }
                };
                for (int i = 0; i < lstInsurance.Count; i++)
                {
                    var drItem = lstInsurance[i];
                    concernAct.entryRelationship = new List<EntryRelationship>();
                    var problemEntryRelationship = new EntryRelationship
                    {
                        typeCode = x_ActRelationshipEntryRelationship.COMP
                    };
                    var contAct = new Act
                    {
                        classCode = x_ActClassDocumentEntryAct.ACT,
                        moodCode = x_DocumentActMood.EVN,
                        templateId = new List<II>
                        {
                            new II {root = "2.16.840.1.113883.10.20.1.26",assigningAuthorityName="CCD"}
                        },
                        id = new List<II>
                        {
                            new II {root=Guid.NewGuid().ToString(),assigningAuthorityName="GroupIdentifier"}
                        },
                        code = new CD(),
                        statusCode = new CS
                        {
                            code = "completed"
                        },
                        priorityCode = new CE
                        {
                            code = drItem["Priority"],
                            displayName = HelperMethodsCaseReports.GetPriority(drItem["Priority"]),
                            codeSystem = "Example"
                        },
                    };
                    var performer = new Performer2();
                    var ae = new AssignedEntity
                    {
                        id = new List<II>()
                        {
                            new II {root=Guid.NewGuid().ToString()}
                        },
                        code = new CE
                        {
                            code = "PAYOR",
                            codeSystem = "2.16.840.1.113883.6.1",
                            displayName = "Payor",
                            codeSystemName = "HL7 RoleClassRelationshipFormal"
                        },

                        addr = new List<AD>
                        {
                            HelperMethodsCaseReports.SetAddress("WP", new List<string> {drItem["PlanAddress"] }, drItem["City"],
                                drItem["State"], drItem["ZIPCode"], "US")
                        },
                        telecom = new List<TEL>
                        {
                            HelperMethodsCaseReports.SetTelephone(drItem["PhoneNo"], "WP")
                        }
                    };
                    //Patient Information

                    ae.assignedPerson = new Person
                    {
                        name = new List<PN>
                        {
                            HelperMethodsCaseReports.SetName("L", drItem["PartyLastName"],
                            new List<string> { drItem["PartyFirstName"], drItem["PartyMI"] })
                        },
                        //birthTime = new TS
                        //{
                        //    value = drItem["PartyBirthTime"]
                        //}
                    };
                    ae.representedOrganization = new Organization
                    {
                        name = new List<ON>
                            {
                               new ON
                               {
                                   Text = new List<string>
                                   {
                                       drItem["PlanName"]
                                   }
                               }
                            },
                        addr = new List<AD>
                        {
                            HelperMethodsCaseReports.SetAddress("WP", new List<string> {drItem["PlanAddress"] }, drItem["City"],
                                drItem["State"], drItem["ZIPCode"], "US")
                        },
                        telecom = new List<TEL>
                        {
                            HelperMethodsCaseReports.SetTelephone(drItem["PhoneNo"], "WP")
                        }
                    };
                    performer.assignedEntity = ae;
                    performer.typeCode = ParticipationPhysicalPerformer.PPRF;
                    contAct.performer = new List<Performer2>
                    {
                        performer
                    };
                    contAct.participant = new List<Participant2>
                    {
                        new Participant2
                        {
                            typeCode = "COV",
                            participantRole = new ParticipantRole
                            {
                                id = new List<II>
                                {
                                    new II {root=Guid.NewGuid().ToString(),extension=drItem["SubscriberId"]}
                                },
                                code=new CE
                                {
                                    code=MDVUtility.ToStr(drItem["RelationShip"]).ToUpper(),
                                    codeSystem="2.16.840.1.113883.5.111",
                                    displayName=drItem["RelationShip"]
                                }
                            }
                        }
                    };
                    problemEntryRelationship.Item = contAct;
                    concernAct.entryRelationship.Add(problemEntryRelationship);
                    entry.Item = concernAct;
                    component.section.entry.Add(entry);
                }
            }
            else
            {
                if (lstInsurance.Count == 0)
                {
                    text.Items.Add(new StrucDocContent
                    {
                        ID = "noPayerSection",
                        Text = new List<string> { "Not included in document" }
                    });
                }
            }

            sb.component.Add(component);
        }
    }

    public class HelperMethodsCaseReports
    {
        public static string SetDateTime(DateTime dt)
        {
            string formattedDateTime = string.Empty;
            if (dt != DateTime.MinValue)
            {
                formattedDateTime = dt.ToString("yyyyMMddhhmmsszzzz").Replace(":", "");
            }
            return formattedDateTime;
        }

        public static TEL SetTelephone(string telephone, string use, string extension = "")
        {
            var tel = new TEL();
            if (!string.IsNullOrWhiteSpace(telephone) && telephone.Length == 14)
            {
                tel.use = new List<string> { use };
                var phoneNumber = "tel:";
                phoneNumber = string.Concat(phoneNumber, telephone);
                if (!string.IsNullOrWhiteSpace(extension))
                    phoneNumber = string.Concat(phoneNumber, "-", extension);
                tel.value = phoneNumber;
            }
            else
                tel.nullFlavor = "NAV";
            return tel;
        }

        public static AD SetAddress(string use, List<string> streetAddressLines, string city, string state, string postalCode, string country)
        {
            var address = new AD();
            if (streetAddressLines.Count == 0 || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(state) || string.IsNullOrWhiteSpace(postalCode) || (streetAddressLines.Count == 1 && string.IsNullOrWhiteSpace(streetAddressLines[0])))
                address.nullFlavor = "NI";
            else
            {
                address.use = new List<string> { use };
                address.Items = new List<ADXP>();
                foreach (var add in streetAddressLines.Where(add => !string.IsNullOrWhiteSpace(add)))
                {
                    address.Items.Add(new adxpstreetAddressLine { Text = new List<string> { add } });
                }
                address.Items.Add(new adxpcity { Text = new List<string> { city } });
                address.Items.Add(new adxpstate { Text = new List<string> { state } });
                address.Items.Add(new adxppostalCode { Text = new List<string> { postalCode } });
                country = !string.IsNullOrEmpty(country) ? country : "US";
                address.Items.Add(new adxpcountry { Text = new List<string> { country } });
            }
            return address;
        }

        public static PN SetName(string use, string familyName, List<string> givenNames, string prefix = "", string suffix = "", string previousName = "")
        {
            var name = new PN();
            if (!string.IsNullOrWhiteSpace(use))
                name.use = new List<string> { use };
            name.Items = new List<ENXP>();
            if (!string.IsNullOrWhiteSpace(familyName))
                name.Items.Add(new enfamily { Text = new List<string> { familyName } });

            if (givenNames != null && givenNames.Count > 0)
            {
                foreach (var nam in givenNames.Where(nam => !string.IsNullOrWhiteSpace(nam)))
                {
                    name.Items.Add(new engiven { Text = new List<string> { nam } });
                }
            }
            if (!string.IsNullOrWhiteSpace(previousName))
                name.Items.Add(new enfamily { Text = new List<string> { previousName } });

            if (!string.IsNullOrWhiteSpace(prefix))
                name.Items.Add(new enprefix { Text = new List<string> { prefix } });

            if (!string.IsNullOrWhiteSpace(suffix))
                name.Items.Add(new ensuffix { Text = new List<string> { suffix } });
            return name;
        }
        public static string ConvertlbsToKG(string lbs)
        {
            string kg = "";
            kg = MDVUtility.ToStr(MDVUtility.ToInt32(lbs) / 0.453592);
            return kg;
        }

        public static string ConvertFeetToInch(string feet)
        {
            string inch;
            if (feet.IndexOf(".", StringComparison.Ordinal) > -1)
            {
                var feetarr = feet.Split('.');
                inch = feetarr.Count() > 1 ? MDVUtility.ToStr((MDVUtility.ToInt32(feetarr[0]) * 12) + MDVUtility.ToInt32(feetarr[1])) : MDVUtility.ToStr(MDVUtility.ToInt32(feetarr[0]) * 12);
            }
            else
                inch = MDVUtility.ToStr(MDVUtility.ToInt32(feet) * 12);
            string cm = string.IsNullOrWhiteSpace(inch) ? "" : MDVUtility.ToStr(MDVUtility.Tofloat(inch) / 2.54);
            return cm;
        }
        public static string GetGenderCode(string gender)
        {
            switch (gender.ToLower())
            {
                case "male":
                    return "M";
                case "female":
                    return "F";
                default:
                    return "UN";
            }
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

        public static string GetCompletionStatusOfImmunization(string code)
        {
            var returnVal = string.Empty;
            switch (code.ToLower())
            {
                case "cp":
                    returnVal = "Complete";
                    break;
                case "re":
                    returnVal = "Refused";
                    break;
                case "na":
                    returnVal = "Not Administered";
                    break;
                case "pa":
                    returnVal = "Partially Administered";
                    break;
            }
            return returnVal;
        }

        public static Component3 IntializeComponent3(List<string> templatedId, string code, string codeSystem,
            string codeSystemName, string displayName, string title, string assigningAuthorityName = "")
        {
            var comp3 = new Component3 { section = new Section() };
            if (templatedId.Count > 0)
            {
                comp3.section.templateId = new List<II>();

                foreach (string t in templatedId)
                {
                    if (!string.IsNullOrWhiteSpace(t) && string.IsNullOrWhiteSpace(assigningAuthorityName))
                    {
                        comp3.section.templateId.Add(new II { root = t });
                    }
                    else if (!string.IsNullOrWhiteSpace(t) && !string.IsNullOrWhiteSpace(assigningAuthorityName))
                    {
                        comp3.section.templateId.Add(new II { root = t, assigningAuthorityName = assigningAuthorityName });
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
                return null;
            if (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(codeSystem) &&
                !string.IsNullOrWhiteSpace(codeSystemName) && !string.IsNullOrWhiteSpace(displayName))
            {
                comp3.section.code = new CE
                {
                    code = code,
                    codeSystem = codeSystem,
                    codeSystemName = codeSystemName,
                    displayName = displayName,
                };
            }
            else
                return null;
            if (!string.IsNullOrWhiteSpace(title))
                comp3.section.title = new ST { Text = new List<string> { title } };
            else
                return null;
            comp3.section.text = new StrucDocText { Items = new List<object>() };
            return comp3;
        }
        public static string GetPriority(string priority)
        {
            switch (priority)
            {
                case "1":
                    priority = "Primary";
                    break;
                case "2":
                    priority = "Secondary";
                    break;
                case "3":
                    priority = "Tertiary";
                    break;
                case "4":
                    priority = "Quaternary";
                    break;
                case "5":
                    priority = "Quinary";
                    break;
                case "6":
                    priority = "Senary";
                    break;
                case "7":
                    priority = "Septenary";
                    break;
                case "8":
                    priority = "Octonary";
                    break;
                case "9":
                    priority = "Nonary";
                    break;
                case "10":
                    priority = "Denary";
                    break;
                default:
                    break;
            }
            return priority;
        }
        public static CPTLookupModel GetCPT(string cptcCOde)
        {
            CPTLookupModel cptData = new CPTLookupModel();
            string jsonData = new Business.BLL.BLLIMO().SearchCPTCode(cptcCOde);
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonData);
            var itemArray = SearchedfieldsJSON["data"]["items"];
            foreach (var item in itemArray)
            {
                var DatakeyValues = new CPTLookupModel
                {
                    CPTCodeDescription = MDVUtility.ToStr(item["title"]),
                    CPTCode = MDVUtility.ToStr(item["CPT_CODE"]),
                    SNOMEDID = MDVUtility.ToStr(item["SCT_CONCEPT_ID"]),
                    SNOMEDDescription = MDVUtility.ToStr(item["SNOMED_DESCRIPTION"]),

                };
                cptData = DatakeyValues;
                if (!string.IsNullOrWhiteSpace(MDVUtility.ToStr(item["SCT_CONCEPT_ID"]))
                    && !string.IsNullOrWhiteSpace(MDVUtility.ToStr(item["SNOMED_DESCRIPTION"])))
                {
                    break;
                }
            }
            return cptData;
        }

        public static string ConvertTempFtoC(string tempInF)
        {
            string temInC = "";
            if (!string.IsNullOrWhiteSpace(tempInF))
            {
                temInC = MDVUtility.ToStr(MDVUtility.Tofloat(tempInF) / 100.4);
            }
            return temInC;
        }
    }
}
