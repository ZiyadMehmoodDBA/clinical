using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Reports
{
    public class MUModel
    {
        public string PatientID { get; set; }
        public string ID { get; set; }
        public string MUID { get; set; }
        public string Measure { get; set; }
        public string Denominator { get; set; }
        public string DenominatorExclusion { get; set; }
        public string Numerator { get; set; }
        public string PerfromanceRate1 { get; set; }
        public string PerfromanceRate2 { get; set; }
        public string ReportingRate1 { get; set; }
        public string ReportingRate2 { get; set; }
        public string Optional1 { get; set; }
        public string Optional2 { get; set; }

        public string Provider { get; set; }
        public string IsActiveProvider { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string IsQuarterlyReport { get; set; }
        public string Quarter { get; set; }
        public string Year { get; set; }
        public string commandType { get; set; }

        public string reportType { get; set; }


        public string Gender { get; set; }
        public string RaceId { get; set; }
        public string RaceDescription { get; set; }
        public string ZIPCode { get; set; }
        public string EthnicityId { get; set; }
        public string EthnicityDescription { get; set; }
        public string Medicaid { get; set; }
        public string Medicare { get; set; }

        public string ReportName { get; set; }

    }
}