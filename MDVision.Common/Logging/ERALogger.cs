using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

namespace MDVision.Common.Logging
{

    [Serializable, XmlRoot("ERAActivitySteps"), XmlType("ERAActivitySteps")]
    public class ERAActivitySteps
    {
        public ERAActivitySteps()
        {
        }
        public ERAActivitySteps(string Name, string Description, string Details, long ERAId, long ERADetailsId,long EDIReportId)
        {
            // TODO: Complete member initialization
            this.Name = Name;
            this.Description = Description;
            this.Details = Details;
            this.ERAId = ERAId;
            this.ERADetailsId = ERADetailsId;
            this.StartTime = DateTime.Now;
            this.EDIReportId = EDIReportId;
            this.EndTime = DateTime.Now;
        }
        [XmlIgnore]
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        [XmlIgnore]
        public DateTime StartTime { get; set; }
        [XmlIgnore]
        public DateTime EndTime { get; set; }
        public Int64 ERAActivityId { get; set; }
        public string UserName { get; set; }
        public bool Status { get; set; }
        public Int64 ERAId { get; set; }
        public Int64 ERADetailsId { get; set; }
        public Int64 EDIReportId { get; set; }
        [XmlElement("StartTime")]
        public string StartTimeString
        {
            get { return this.StartTime.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.StartTime = DateTime.Parse(value); }
        }
        [XmlElement("EndTime")]
        public string EndTimeString
        {
            get { return this.EndTime.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.EndTime = DateTime.Parse(value); }
        }
    }

    public static class ERAActivityStepsLogger
    {
        #region StartActivityStep
        #endregion

        #region EndActivityStep
        #endregion


        #region Genrate XML
        public static string MaintainERALogging(string UserName, List<ERAActivitySteps> ActivityStepList, int ERAActivityId)
        {
            try
            {
                foreach (ERAActivitySteps step in ActivityStepList)
                {
                    step.UserName = UserName;
                    step.ERAActivityId = ERAActivityId;//
                }

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ERAActivitySteps>));
                StringWriter textWriter = new StringWriter();

                // var memoryStream = new MemoryStream();
                // TextWriter stringWriter = new StreamWriter(memoryStream, System.Text.Encoding.UTF8);
                // xmlSerializer.Serialize(stringWriter, ActivityStepList);
                xmlSerializer.Serialize(textWriter, ActivityStepList);
                return textWriter.ToString(); //System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        #endregion
    }

}
