using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.PQRS
{
    public class PQRSReportsModel
    {
        public string commandType { get; set; }
        public string ProviderId { get; set; }
        public string ReportToDate { get; set; }
        public string ReportFromDate { get; set; }
        public string ProviderGroupId { get; set; }

        public string SummaryProviderGroupId { get; set; }
        public string SummaryProviderId { get; set; }
        public int MeasureId { get; set; }
        public string VisitsID { get; set; }
    }
    public class PQRSReports_FillModel
    {
        public string measureId { get; set; }

        public string measureName { get; set; }

        public string totalPatients { get; set; }

        public string numerator { get; set; }

        public string denuminator { get; set; }

        public string performanceMet { get; set; }

        public string performanceNotMet { get; set; }

        public string exclusion { get; set; }

        public long reportingRate { get; set; }

        public string nonCompliantPatients { get; set; }

        public long performanceRate { get; set; }

        public string Patients { get; set; }
        public string PMPatients { get; set; }
        public string NonCompliantVisitsList { get; set; }
        public string measureNumber { get; internal set; }
    }
}