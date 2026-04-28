using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MDVision.Model.Batch.ExportCCDA
{
    public class ExportCCDAModel
    {
        [XmlIgnore]
        public long PatientId { get; set; }
        [XmlIgnore]
        public string PatientName { get; set; }
        [XmlIgnore]
        public string PatientFullName { get; set; }
        [XmlIgnore]
        public string Gender { get; set; }
        [XmlIgnore]

        public string NoteComponentsLookupId { get; set; }
        [XmlIgnore]
        public string ComponentName { get; set; }
        [XmlIgnore]
        public long RecordCount { get; set; }
        [XmlIgnore]
        public string commandType { get; set; }
        [XmlIgnore]
        public string FilePath { get; set; }
        public string DateTo { get; set; }
        public string DateFrom { get; set; }
        public string RuleType { get; set; }
        //public List<NoteComponent> NoteComponents { get; set; }
        //public List<Patient> Patients { get; set; }
        //public List<ExportTimes> TimeCheckedIds { get; set; }
        //public List<Month> MonthsCheckedIds { get; set; }
        //public List<Week> WeeksCheckedIds { get; set; }
        //public List<Day> DaysCheckedIds { get; set; }

        public string Years { get; set; }
        public string Months { get; set; }
        public string Weeks { get; set; }
        public string Days { get; set; }

        public string MultiTime { get; set; }
        public string NoteComponents { get; set; }
        public string SecPatient { get; set; }
        public string IsError { get; set; }

        public string SchedulerId { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string IsActive { get; set; }
        public string IsPatientsAll { get; set; }
        public string IsComponentsAll { get; set; }
        public string ErrorMessage { get; set; }
        public string DateExport { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string Id { get; set; }
        public string ProviderId { get; set; }
        public string SchedulerHour { get; set; }

    }
    //public class NoteComponent
    //{
    //    public string NoteComponents { get; set; }

    //}
    //public class Patient
    //{
    //    public string PatientId { get; set; }

    //}
    //public class ExportTimes
    //{
    //    public string ExportTime { get; set; }

    //}
    //public class Month
    //{
    //    public string Months { get; set; }

    //}
    //public class Week
    //{
    //    public string Weeks { get; set; }

    //}
    //public class Day
    //{
    //    public string Days { get; set; }

    //}

}