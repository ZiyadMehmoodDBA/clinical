using MDVision.Business.BLL;
using MDVision.Datasets;
//using MDVision.IEHR.EMR.Model.PQRS;
//using MDVision.IEHR.Controls.Batch.CQM;
using MDVision.IEHR.EMR.Model.PQRS;
using MDVision.IEHR.EMR.Model.QRDA;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace MDVision.IEHR.EMR.Helpers.Clinical.PQRSReports.Schema
{
    public class PqrsQrdaOneXMLGenerator : PqrsQrdaOneXMLDataExtraction
    {
        private BLLCQM BLLCQMObj = null;
        public PqrsQrdaOneXMLGenerator()
        {
            BLLCQMObj = new BLLCQM();
        }
        private static ClinicalDocument _document;

        static DSPQRS _dsPatientDemoGraphic;
        static DSProfile _dsProvider;
        static DSProfile _dsPractice;
        private static DSPQRS _dsMeasureSection;

        private static StructuredBody StructuredBody
        {
            get { return _document.component.Item as StructuredBody; }
        }

        private static DSPQRS GetMeasureSection_QrdaOne(Int64 providerId, int measureId, string startDate, string endDate)
        {
            var obj = new BLLPQRS().generateIndividualReportQRDA(measureId, providerId, startDate, endDate);
            _dsMeasureSection = obj.Data;
            return _dsMeasureSection;
        }


        public static List<KeyValuePair<string, string>> InitializeQrdaOne_DataSets(int measureId, Int64 patientId, Int64 providerId,
            Int64 practiceId, string startDate, string endDate)
        {
            #region Instantiate Record Set


            _dsPatientDemoGraphic = GetPatient_QrdaOne(measureId, providerId, startDate, endDate);

            _dsProvider = GetProvider_QrdaOne(providerId);
            _dsPractice = GetPractice_QrdaOne(practiceId);

            #endregion

            var dtPQRS_PatientsList = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PQRS_PatientsList.TableName];
            var kvListPatients_ = new List<KeyValuePair<string, string>>();

            _dsMeasureSection = GetMeasureSection_QrdaOne(providerId, measureId, startDate, endDate);
            string Patients = _dsMeasureSection.PQRSReports.Rows[0][_dsMeasureSection.PQRSReports.PatientsColumn.ColumnName].ToString();
           string MeasureNumber = _dsMeasureSection.PQRSReports.Rows[0][_dsMeasureSection.PQRSReports.MeasureNumberColumn.ColumnName].ToString();
            List<string> listPatients = Patients.Split(',').ToList<string>();
            
            if (listPatients != null)
            {
                for (var i = 0; i < listPatients.Count; i++)
                {
                    patientId = Int64.Parse(listPatients[i]);
                    _dsPatientDemoGraphic = GetPatient_QrdaOne(patientId);

                    DataView dvPatientDemoGraphic = null;

                    if (!string.IsNullOrEmpty(patientId.ToString()))
                    {
                        dvPatientDemoGraphic =
                            new DataView(_dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName])
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

                    var lastName = dtPatientDemoGraphic.Rows[0][_dsPatientDemoGraphic.PQRS_PatientsList.LastNameColumn.ColumnName].ToString();
                    var firstName = dtPatientDemoGraphic.Rows[0][_dsPatientDemoGraphic.PQRS_PatientsList.FirstNameColumn.ColumnName].ToString();

                    var currentFileContent = GenerateQrdaOne_XML(measureId, patientId, providerId, practiceId, startDate, endDate, _dsMeasureSection, MeasureNumber);

                    kvListPatients_.Add(new KeyValuePair<string, string>(lastName + " ," + firstName, currentFileContent));
                }
            }
            return kvListPatients_;
        }

        private static string GenerateQrdaOne_XML(int measureId, Int64 patientId, Int64 providerId, Int64 practiceId, string startDate, string endDate, DSPQRS _dsMeasureSection,string MeasureNumber)
        {

            //  _document.component.Item = new StructuredBody();



            //   StructuredBody.component = new List<Component3>();

            if (_document == null)
            {
                _document = new ClinicalDocument();
                _document = new ClinicalDocument()
                {
                    component = new Component2
                    {
                        Item = new StructuredBody()
                    }
                };
            }

            StructuredBody.component = new List<Component3>();
            BuildTemplate(measureId, patientId, providerId, practiceId, startDate, endDate, _dsMeasureSection, MeasureNumber);

            string xml;
            var serializer = new XmlSerializer(typeof(ClinicalDocument));

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
               "<ClinicalDocument xmlns:sdtc=\"urn:hl7-org:sdtc\" xmlns:voc=\"urn:hl7-org:v3/voc\" xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
           // xml = xml.Replace("compression=\"DF\"", "").Replace("frame=\"void\" rules=\"none\"", "")
                //.Replace("d8p1:valueSet=\"2.16.840.1.113883.3.464.1003.101.12.1001\" xmlns:d8p1=\"urn:hl7-org:sdtc\"", "sdtc:valueSet=\"2.16.840.1.113883.3.464.1003.101.12.1001\"")
              //  .Replace("d8p1:valueSet", "sdtc:valueSet").Replace("xmlns: d8p1 = \"urn:hl7-org:sdtc\"", "")
            //    .Replace("d11p1:valueSet", "sdtc:valueSet").Replace("xmlns:d11p1=\"urn:hl7-org:sdtc\" ", "")
             //   .Replace("d13p1:valueSet", "sdtc:valueSet").Replace("xmlns:d13p1=\"urn:hl7-org:sdtc\" ", "");


            //<?xml-stylesheet type="text/xsl" href="cda.xsl"?>

            //if (xml.Contains("<ClinicalDocument xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" classCode=\"DOCCLIN\" xmlns=\"urn:hl7-org:v3\">"))
            //{
            //    xml =
            //        xml.Replace(
            //            "<ClinicalDocument xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" classCode=\"DOCCLIN\" xmlns=\"urn:hl7-org:v3\">",
            //            "<ClinicalDocument xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn:hl7-org:v3\" xmlns:voc=\"urn:hl7-org:v3/voc\" xmlns:sdtc=\"urn:hl7-org:sdtc\">");
            //}

            // Style Sheet
            if (xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"))
                xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>",
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?> <?xml-stylesheet type=\"text/xsl\" href=\"cda.xsl\"?>");

            var bytes = Encoding.UTF8.GetBytes(xml);
            var base64String = Convert.ToBase64String(bytes);

            return base64String;

            //File.WriteAllText(path, xml);
        }
        private static void BuildTemplate(int measureId, Int64 patientId, Int64 providerId, Int64 practiceId, string startDate, string endDate, DSPQRS dsMeasureSection,string MeasureNumber)
        {
            var catagoryOneXmlDocumentLevelTemplates = new QRDAXMLDocumentLevelTemplates();
            catagoryOneXmlDocumentLevelTemplates.BuildDocumentLevelTemplate(_document, _dsPatientDemoGraphic, _dsProvider, _dsPractice);

            var catagoryOneXmlSectionLevelTemplates = new QRDXMLSectionLevelTemplates();
            catagoryOneXmlSectionLevelTemplates.BuildSectionLevelTemplate(_document, _dsPatientDemoGraphic, _dsProvider, _dsPractice, MeasureNumber, patientId, providerId, practiceId, startDate, endDate, dsMeasureSection);
        }

    }
}