using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using MDVision.Datasets;

namespace MDVision.IEHR.Controls.Batch.CQM
{
    public class CatagoryThreeXMLGenerator : CatagoryOneXMLDataExtraction
    {
        private static AmalgamatedClinicalDocument _document;

        static DSProfile _dsProvider;
        static DSProfile _dsPractice;

        private static StructuredBody StructuredBody
        {

            get { return _document.component.Item as StructuredBody; }
        }

        public static string InitializeCategoryThree_DataSets( string cqmid, Int64 providerId, Int64 practiceId, string startDate, string endDate, string fieldsJSON = null )
        {
            _dsProvider = GetProvider_CategoryOne(providerId);
            _dsPractice = GetPractice_CategoryOne(practiceId);
            var base64StringToDownload = GenerateCategoryThree_XML(providerId, startDate, endDate, fieldsJSON);
            return base64StringToDownload;
        }

        private static string GenerateCategoryThree_XML( Int64 providerId, string startDate, string endDate, string fieldsJSON = null)
        {
            _document = new AmalgamatedClinicalDocument(){
                component = new Component2
                {
                    Item = new StructuredBody()
                }
            };

            StructuredBody.component = new List<Component3>();

            BuildTemplate(providerId, startDate, endDate, fieldsJSON);
            
            string xml;
            var serializer = new XmlSerializer(typeof(AmalgamatedClinicalDocument));

            using (var strWriter = new StringWriter()){
                serializer.Serialize(strWriter, _document);
                xml = strWriter.ToString();
            }
            xml = xml.Replace(
                "<ClinicalDocument xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" classCode=\"DOCCLIN\" xmlns=\"urn:hl7-org:v3\">",
                "<ClinicalDocument xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn:hl7-org:v3\" xmlns:voc=\"urn:hl7-org:v3/voc\" xmlns:sdtc=\"urn:hl7-org:sdtc\">").
                Replace("utf-16", "utf-8").Replace("compression=\"DF\"", "").
                Replace("frame=\"void\" rules=\"none\"", "").
                Replace("<table >", "<table border=\"1\" width=\"100%\">").
                Replace("displayable=\"false\"", "").Replace("contextConductionInd=\"true\"", "").Replace("negationInd=\"false\"","").
                Replace("<externalObservation moodCode=\"EVN\">", "<externalObservation moodCode=\"EVN\" classCode=\"OBS\">");

            if (xml.Contains("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\""))
                xml = xml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "xmlns:voc=\"urn:hl7-org:v3/voc\" xmlns:sdtc=\"urn:hl7-org:sdtc\"");

            

            if (xml.Contains("<ClinicalDocument xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" classCode=\"DOCCLIN\" xmlns=\"urn:hl7-org:v3\">"))
                xml = xml.Replace("<ClinicalDocument xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" classCode=\"DOCCLIN\" xmlns=\"urn:hl7-org:v3\">",
                "<ClinicalDocument xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn:hl7-org:v3\" xmlns:voc=\"urn:hl7-org:v3/voc\" xmlns:sdtc=\"urn:hl7-org:sdtc\">");

            // Style Sheet
            if (xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"))
                xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>",
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?> <?xml-stylesheet type=\"text/xsl\" href=\"qrda.xsl\"?>");

            var bytes = Encoding.UTF8.GetBytes(xml);
            var base64String = Convert.ToBase64String(bytes);
            return base64String;
        }
        private static void BuildTemplate( Int64 providerId, string startDate, string endDate, string fieldsJSON = null)
        {
            var catagoryThreeXmlSectionLevelTemplates = new CatagoryThreeXMLSectionLevelTemplates();
            catagoryThreeXmlSectionLevelTemplates.BuildSectionLevelTemplate(_document, providerId, _dsProvider, _dsPractice, startDate, endDate, fieldsJSON);
        }
    }
}