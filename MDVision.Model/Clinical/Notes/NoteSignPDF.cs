using MDVision.Model.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Notes
{
    public class NoteSignPDF
    {
        public string PDFData { get; set; }
        public string PatientID { get; set; }
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FolderId { get; set; }
        public string NotesId { get; set; }
        public string DOS { get; set; }
    }

    public class NotesViewModel
    {
        public List<NoteComponentModel> NoteComponentModelList { get; set; }
        public List<NotesModel> NotesModelList { get; set; }
        public List<PatientViewModel> PatientList { get; set; }
        public List<ProviderModel> ProviderList { get; set; }
        public List<PracticeModel> PracticeList { get; set; }
        public List<RptHdrTags> RptHdrTagsList { get; set; }
    }
}
