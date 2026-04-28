using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.ProcedureOrder
{
    public class ProcedureOrderModel
    {
        public string ProcedureOrderId { get; set; }
        public string ProcedureOrderIDs { get; set; }
        public string ProcedureOrderTitle { get; set; }
        public string ProcedureOrderDeveloper { get; set; }
        public string ProcedureOrderFundingSource { get; set; }
        public string ProcedureOrderReferenceURL { get; set; }
        public string ProcedureOrderRelease { get; set; }
        public string ProcedureOrderRevisionDate { get; set; }
        public string ProcedureOrderTriggerLocation { get; set; }
        public string ProcedureOrderUserRole { get; set; }
        public string ProcedureOrderRuleType { get; set; }
        public string ProcedureOrderSex { get; set; }
        public string ProcedureOrderEthnicity { get; set; }
        public string ProcedureOrderRace { get; set; }
        public string ProcedureOrderLanguage { get; set; }
        public string ProcedureOrderReminderLength { get; set; }
        public string ProcedureOrderReminderPeriod { get; set; }
        public bool ProcedureOrderRecursive { get; set; }
        //public string ProcedureOrderProblemList { get; set; }
        public string ProcedureOrderAllergies { get; set; }
        public string ProcedureOrderMedications { get; set; }
        public string ProcedureOrderLabResults { get; set; }
        public string ProcedureOrderVitals { get; set; }
        public string ProcedureOrderAlertNote { get; set; }
        public string ModifiedOn { get; set; }
        public bool ProcedureOrderActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ProcedureOrderQuery { get; set; }
        public string commandType { get; set; }        
        public string RowsPerPage { get; set; }
        public string PageNumber { get; set; }
        public string PatientId { get; set; }
        public string NoteId { get; set; }
        public string ProviderId { get; set; }
        public string AssigneeId { get; set; }
        public string ProcedureOderDate { get; set; }
        public string ProcedureOderTime { get; set; }

        
        public string Procedures { get; set; }
        public List<ProcedureOrderProblemModel> ProcedureOrderProblemList { get; set; }
        public string OrderFromDate { get; set; }
        public string OrderToDate { get; set; }
        public string Status { get; set; }
        public string OrderNo { get; set; }
        public string OrderSetId { get; set; }
    }
}