using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.IEHR.EMR.Helpers.Clinical.ClinicalNotes;
using MDVision.WebAPI.Models.NotesModel;
using Newtonsoft.Json;


namespace MDVision.WebAPI.Helpers
{
    public class NotesHelpler
    {

        string response = string.Empty;
       
        public string LoadClinicalNotes(string NoteStatus, Int64 PatientID,Int32 PageNumber, Int32 RowsPerPage)
        {
            

            response = ClinicalNotesHelper.Instance().loadClinical_Notes_Obsolete(PatientID, 0, 0, PageNumber, RowsPerPage, 0, NoteStatus);

            
                return response;
            }


        public string fillClinicalNoteById(Int64 NotesID, Int64 PatientID)
        {


            response = ClinicalNotesHelper.Instance().fillClinical_Note_By_Id(NotesID, PatientID);


            return response;
        }



    }
}