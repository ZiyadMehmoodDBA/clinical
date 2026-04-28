using MDVision.Model.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MDVision.Model.Patient
{
    public class PatientInsuranceModel
    {
        public PatientInsuranceModel()
        {
           
            lstChangedColumns = new List<ChangedColumnsNative>();
            DataChangeRequest = new List<DataChangeRequest>();

        }
        public string InsuranceId { get; set; }
        public string ColumnKeyId { get; set; }
        public string SubscriberId { get; set; }
        public string GroupId { get; set; }
        public string AmtCopay { get; set; }
        public string SpecialistCopay { get; set; }
        public string SubscriberDOB { get; set; }
        public string SubscriberFirstName { get; set; }
        public string SubscriberMI { get; set; }
        public string SubscriberLastName { get; set; }
        public string RelationShipId { get; set; }
        public string RelationName { get; set; }
        public string EligibilityDate { get; set; }
        public string PlanPriority { get; set; }
        public string InsurancePlanId { get; set; }
        public string InsurancePlanName { get; set; }

        public string CoverageTo { get; set; }
        //public string PatientId { get; set; }


        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }

        public string PatientInsuranceID { get; set; }
        public string PatientId { get; set; }
        public string DimmyPatientId { get; set; }
        public string InsuraceJson { get; set; }
        public string CammandAction { get; set; }
        public string Gender { get; set; }
        public string Comments { get; set; }
        public string listChangedColumns { get; set; }
        
        public List<ChangedColumnsNative> lstChangedColumns { get; set; }

        public List<DataChangeRequest> DataChangeRequest { get; set; }
    }
}
