using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.OrdersAndResults.ConsultationOrder
{
    public class ConsultationOrderModel
    {
        public string ConsultationOrderId { get; set; }
        public string ConsultationOrderIDs { get; set; }
        public string ConsultationOrderTitle { get; set; }
        public string ConsultationOrderDeveloper { get; set; }
        public string ConsultationOrderFundingSource { get; set; }
        public string ConsultationOrderReferenceURL { get; set; }
        public string ConsultationOrderRelease { get; set; }
        public string ConsultationOrderRevisionDate { get; set; }
        public string ConsultationOrderTriggerLocation { get; set; }
        public string ConsultationOrderUserRole { get; set; }
        public string ConsultationOrderRuleType { get; set; }
        public string ConsultationOrderSex { get; set; }
        public string ConsultationOrderEthnicity { get; set; }
        public string ConsultationOrderRace { get; set; }
        public string ConsultationOrderLanguage { get; set; }
        public string ConsultationOrderReminderLength { get; set; }
        public string ConsultationOrderReminderPeriod { get; set; }
        public bool ConsultationOrderRecursive { get; set; }
        //public string ConsultationOrderProblemList { get; set; }
        public string ConsultationOrderAllergies { get; set; }
        public string ConsultationOrderMedications { get; set; }
        public string ConsultationOrderLabResults { get; set; }
        public string ConsultationOrderVitals { get; set; }
        public string ConsultationOrderAlertNote { get; set; }
        public string ModifiedOn { get; set; }
        public bool ConsultationOrderActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ConsultationOrderQuery { get; set; }
        public string commandType { get; set; }
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public string AssigneeId { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string OrderTime { get; set; }
        public string IsActive { get; set; }
        public string SoapText { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string NoteId { get; set; }
        public List<ConsultationOrderProblemModel> ConsultationOrderProblemList { get; set; }
        public string ProcedureName { get; set; }
        public string Procedures { get; set; }

        public string CPTCode { get; set; }
        
        public string OrderFromDate { get; set; }
        public string OrderToDate { get; set; }
        public string Status { get; set; }
    }
}
