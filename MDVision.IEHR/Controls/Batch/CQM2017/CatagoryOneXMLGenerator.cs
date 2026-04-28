using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using MDVision.Datasets;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Batch.CQM2017
{
    public class CatagoryOneXMLGenerator : CatagoryOneXMLDataExtraction
    {

        private BLLCQM BLLCQMObj = null;
        public CatagoryOneXMLGenerator()
        {
            BLLCQMObj = new BLLCQM();
        }
        private static AmalgamatedClinicalDocument _document;

        static DSCQM _dsPatientDemoGraphic;
        static DSProfile _dsProvider;
        static DSProfile _dsPractice;
        private static DSCQM _dsMeasureSection;

        private static StructuredBody StructuredBody
        {
            get { return _document.component.Item as StructuredBody; }
        }

        private static DSCQM GetMeasureSection_CategoryOne(Int64 providerId, string cqmid, string startDate, string endDate)
        {
            var obj = new BLLCQM().Load_CQM(providerId, null, startDate, endDate, null, 0, cqmid);
            _dsMeasureSection = obj.Data;
            return _dsMeasureSection;
        }


        public static List<KeyValuePair<string, string>> InitializeCategoryOne_DataSets(string cqmid, Int64 patientId, Int64 providerId,
            Int64 practiceId, string startDate, string endDate, List<string> listPatients)
        {
            #region Instantiate Record Set


            _dsPatientDemoGraphic = GetPatient_CategoryOne(cqmid, providerId, startDate, endDate);

            _dsProvider = GetProvider_CategoryOne(providerId);
            _dsPractice = GetPractice_CategoryOne(practiceId);

            #endregion

            var dtCqmPatientsList = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.CQM_PatientsList.TableName];
            var kvListPatients_ = new List<KeyValuePair<string, string>>();

            _dsMeasureSection = GetMeasureSection_CategoryOne(providerId, cqmid, startDate, endDate);

            for (var i = 0; i < listPatients.Count; i++)
            {
                patientId = Int64.Parse(listPatients[i]);
                _dsPatientDemoGraphic = GetPatient_CategoryOne(patientId);

                DataView dvPatientDemoGraphic = null;

                if (!string.IsNullOrEmpty(patientId.ToString()))
                {
                    dvPatientDemoGraphic =
                        new DataView(_dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName])
                        {
                            RowFilter = "PatientID = " + patientId
                        };
                }

                DataTable dtPatientDemoGraphic = null;
                if (dvPatientDemoGraphic != null)
                {
                    dtPatientDemoGraphic = dvPatientDemoGraphic.ToTable();
                }

                if (dtPatientDemoGraphic == null) continue;

                var lastName = dtPatientDemoGraphic.Rows[0][_dsPatientDemoGraphic.CQM_PatientsList.LastNameColumn.ColumnName].ToString();
                var firstName = dtPatientDemoGraphic.Rows[0][_dsPatientDemoGraphic.CQM_PatientsList.FirstNameColumn.ColumnName].ToString();

                var currentFileContent = GenerateCategoryOne_XML(cqmid, patientId, providerId, practiceId, startDate, endDate, _dsMeasureSection);

                kvListPatients_.Add(new KeyValuePair<string, string>(lastName + " ," + firstName, currentFileContent));
            }
            return kvListPatients_;
        }

        private static string GenerateCategoryOne_XML(string cqmid, Int64 patientId, Int64 providerId, Int64 practiceId, string startDate, string endDate, DSCQM _dsMeasureSection)
        {

            _document = new AmalgamatedClinicalDocument()
            {
                component = new Component2
                {
                    Item = new StructuredBody()
                }
            };

            StructuredBody.component = new List<Component3>();

            BuildTemplate(cqmid, patientId, providerId, practiceId, startDate, endDate, _dsMeasureSection);

            string xml;
            var serializer = new XmlSerializer(typeof(AmalgamatedClinicalDocument));

            using (var strWriter = new StringWriter())
            {
                serializer.Serialize(strWriter, _document);
                xml = strWriter.ToString();
            }

            xml = xml.Replace("utf-16", "utf-8");

            //var rand = new Random().Next(10000);

            //string path = @"E:\" + lastName + ", " + firstName + ".xml";
            xml = xml.Replace(
               "<ClinicalDocument xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" classCode=\"DOCCLIN\" xmlns=\"urn:hl7-org:v3\">",
               "<ClinicalDocument xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn:hl7-org:v3\" xmlns:voc=\"urn:hl7-org:v3/voc\" xmlns:sdtc=\"urn:hl7-org:sdtc\">");
            xml = xml.Replace("compression=\"DF\"", "").Replace("frame=\"void\" rules=\"none\"", "")
                //.Replace("d8p1:valueSet=\"2.16.840.1.113883.3.464.1003.101.12.1001\" xmlns:d8p1=\"urn:hl7-org:sdtc\"", "sdtc:valueSet=\"2.16.840.1.113883.3.464.1003.101.12.1001\"")
                .Replace("d8p1:valueSet", "sdtc:valueSet").Replace("xmlns: d8p1 = \"urn:hl7-org:sdtc\"", "")
                .Replace("d11p1:valueSet", "sdtc:valueSet").Replace("xmlns:d11p1=\"urn:hl7-org:sdtc\" ", "")
                .Replace("d13p1:valueSet", "sdtc:valueSet").Replace("xmlns:d13p1=\"urn:hl7-org:sdtc\" ", "");


            //<?xml-stylesheet type="text/xsl" href="cda.xsl"?>

            if (xml.Contains("<ClinicalDocument xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" classCode=\"DOCCLIN\" xmlns=\"urn:hl7-org:v3\">"))
            {
                xml =
                    xml.Replace(
                        "<ClinicalDocument xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" classCode=\"DOCCLIN\" xmlns=\"urn:hl7-org:v3\">",
                        "<ClinicalDocument xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn:hl7-org:v3\" xmlns:voc=\"urn:hl7-org:v3/voc\" xmlns:sdtc=\"urn:hl7-org:sdtc\">");
            }

            // Style Sheet
            if (xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"))
                xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>",
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?> <?xml-stylesheet type=\"text/xsl\" href=\"cda.xsl\"?>");

            var bytes = Encoding.UTF8.GetBytes(xml);
            var base64String = Convert.ToBase64String(bytes);

            return base64String;

            //File.WriteAllText(path, xml);
        }
        private static void BuildTemplate(string cqmid, Int64 patientId, Int64 providerId, Int64 practiceId, string startDate, string endDate, DSCQM dsMeasureSection)
        {
            var catagoryOneXmlDocumentLevelTemplates = new CatagoryOneXMLDocumentLevelTemplates();
            catagoryOneXmlDocumentLevelTemplates.BuildDocumentLevelTemplate(_document, _dsPatientDemoGraphic, _dsProvider, _dsPractice);

            //var catagoryOneXmlSectionLevelTemplates = new CatagoryOneXMLSectionLevelTemplates();
            //catagoryOneXmlSectionLevelTemplates.BuildSectionLevelTemplate(_document, _dsPatientDemoGraphic, _dsProvider, _dsPractice, cqmid, patientId, providerId, practiceId, startDate, endDate, dsMeasureSection);
        }
    }
}