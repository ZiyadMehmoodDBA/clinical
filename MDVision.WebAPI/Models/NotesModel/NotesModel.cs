using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.NotesModel
{
    public class NotesModel
    {
        public string status { get; set; }
        public string ClinicalNotesCount { get; set; }
        public string iTotalDraftDisplayRecords { get; set; }
        public string iTotalSignedDisplayRecords { get; set; }
        public string NotesLoad_JSON { get; set; }
        
        public string Message { get; set; }
        
    }
}