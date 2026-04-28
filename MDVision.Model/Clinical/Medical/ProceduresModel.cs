using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Medical
{
    public class ProceduresModel
    {
        public string commandType { get; set; }
        public string UserId { get; set; }
        public string EntityId { get; set; }
        /*public string ProcedureId { get; set; }
        public string PatientId { get; set; }
        public string NoteId { get; set; }
        public string IsActive { get; set; }
        public string RowsPerPage { get; set; }
        public string PageNumber { get; set; }*/
        public string ProcedureId { get; set; }
        public long PatientId { get; set; }
        public long NotesId { get; set; }

        public List<ProceduresDetailModel> procedureDetailModel { get; set; }
        public string IsActive { get; set; }
        public string ShowEMCodes { get; set; }
        public string CustomFormId { get; set; }
        public bool ForVBP { get; set; }
        public bool PHQTextNeeded { get; set; }
        public string ProviderId { get; set; }
    }
}
