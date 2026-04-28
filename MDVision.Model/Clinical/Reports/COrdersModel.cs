using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Reports
{
    public class COrdersModel
    {
        public string AccountNumber { get; set; }
        public bool IncludeInactivePatient { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public long ProviderId { get; set; }
        public string LabId { get; set; }
        public string CPTCode { get; set; }
        public string OrderNo { get; set; }
        public string OrderStatus { get; set; }
        public string PatientId { get; set; }
        public string Provider { get; set; }
        public string DOB { get; set; }
        public string PatientStatus { get; set; }
        public string Test { get; set; }
        public string LaboratoryIds { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string OrderType { get; set; }
        public long AssigneeProviderId { get; set; }

        // Start || 1 October, 2016 || Talha Tanweer || for consultation tab
        public long AssingneeProviderId { get; set; }
        public string Procedure { get; set; }
        // End   || 1 October, 2016 || Talha Tanweer || for consultation tab
        public bool IsSummaryReport { get; set; }
 
    }

    public class COrdersFillModel
    {
        public string AccountNumber { get; set; }
        public string PatientName { get; set; }
        public string DOB { get; set; }
        public string PatStatus { get; set; }
        public string Test { get; set; }
        public string Laboratory { get; set; }
        public string OrderNo { get; set; }
        public string OrderStatus { get; set; }
        public string Provider { get; set; }
        public string OrderDateTime { get; set; }
        public long PatientId { get; set; }
            

        // Start || 1 October, 2016 || Talha Tanweer || for consultation tab
        public string AssingneeProvider { get; set; }
        public string Procedure { get; set; }
        // End   || 1 October, 2016 || Talha Tanweer || for consultation tab
        public string InsuranceName { get; set; }
            
    }
}
