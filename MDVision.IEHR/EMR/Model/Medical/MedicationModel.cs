/*
 Author : Khaleel Ur Rehman
 Date : 04-01-2016
 Purpose : Model class for Medications in clinical module.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Medical
{
    public class MedicationModel
    {
        public long MedicationID { get; set; }
        public string RouteId { get; set; }
        public string EntityId { get; set; }
        public string MedicationIDs { get; set; }
        public string RcopiaID { get; set; }
        public long PatientID { get; set; }
        public long PrescriptionID { get; set; }
        public long ProviderID { get; set; }
        public string ProviderName { get; set; }
        public long Preparer_UserID { get; set; }
        public long DrugID { get; set; }
        public string Action { get; set; }
        public string Dose { get; set; }
        public string DoseUnit { get; set; }
        public string Routeby { get; set; }
        public string DoseTiming { get; set; }
        public string DoseOther { get; set; }
        public int Duration { get; set; }
        public string Quantity { get; set; }
        public string QuantityUnit { get; set; }
        public string Substitution { get; set; }
        public string OtherNote { get; set; }
        public string PatientNotes { get; set; }
        public string Comments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
        public string StopReason { get; set; }
        public DateTime SigChangedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string IntendedUse { get; set; }
        public int Number { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public DateTime FillDate { get; set; }

        public string RowsPerPage { get; set; }
        public string PageNumber { get; set; }
        public string commandType { get; set; }
        public string MedicationName { get; set; }

        public bool IsNoteLinked { get; set; }
        public bool isCurrent { get; set; }
        // Author:  Muhammad Ahmad Imran
        // Created Date: 14/01/2016
        //OverView: Add new Attribute LastUpdateDate for Update MedicationLastUpdateDate in PatientLastUpdateInfo
        public string LastUpdateDate { get; set; }
        public string IsDeleted { get; set; }
        public string DrugCodingLevel { set; get; }
        public string PrescriptionRcopiaID { set; get; }
        public string PreparerRcopiaID { set; get; }
        public string ProviderRcopiaID { set; get; }
        public DrugModel drugModel { get; set; }
        public string Refill { get; set; }
        

        public string NoteId { get; set; }

        public bool isFromCDS { get; set; }
        public string bMedReconciled { get; set; }
        public string MedReconciledId { get; set; }
        public string UserId { get; set; }
        public string RxNormId { get; set; }
        public string DrugDescription { get; set; }
        public string RouteCode { get; set; }
        public string RepeatNumber { get; set; }
        public string NegationReason { get; set; }
        public bool NegationIndex { get; set; }
        public string DoseValue { get; set; }
        public string RcopiaUserName { get; set; }
        public string ProviderNPI { get; set; }
    }
}