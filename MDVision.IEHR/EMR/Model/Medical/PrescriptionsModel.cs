using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Medical
{
    public class PrescriptionsModel
    {

        public long PrescriptionID { get; set; }
        public string PrescriptionIDForSoap { get; set; }
        public string RcopiaID { get; set; }
        public long PatientID { get; set; }
        public string ProviderID { get; set; }
        public string Preparer_UserID { get; set; }
        public string PharmacyID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime SignedDate { get; set; }
        public DateTime StopDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string SendMethod { get; set; }
        public string Refill { get; set; }
        public string Cancelleddate { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string MedicationName { get; set; }
        public string ProviderName { get; set; }
        public string Status { get; set; }
        public string PharmacyName { get; set; }
        public string commandType { get; set; }
        public string IsDeleted { get; set; }
        public object PageNumber { get; set; }
        public string Voided { get; set; }
        public string Denied { get; set; }
        public object RowsPerPage { get; set; }
        public DrugModel drugModel { get; set; }
        public MedicationModel MedicationModel { get; set; }
        public PharamacyModel PharamacyModel { get; set; }
        public string SubstitutionPermitted { get; set; }
        public string OtherNotes { get; set; }
        public string PatientNotes { get; set; }
        public string Comments { get; set; }
        public string LastUpdateDate { get; set; }

        public bool IsNoteLinked { get; set; }   
        public string DrugRcopiaID { get; set; }

        public string CompletionAction { get; set; }
        public string IntendedUse { get; set; }
        

        public long NotesId { get; set; }

        //start Below columns added by Khaleel Ur Rehman to fetch data for these fields too on 20 JAN 2016
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

        public string NoteId { get; set; }
        //End by Khaleel Ur Rehman

    }
}