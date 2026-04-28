using System;
using MDVision.WebAPI.Filters;
using MDVision.Common.Utilities;
using System.Web.Script.Serialization;
using MDVision.Common.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Owin.Security;
using System.Net.Http;
using System.Web.Http;
using MDVision.WebAPI.Helpers;
using MDVision.IEHR.EMR.Helpers.Clinical.ClinicalNotes;
using MDVision.Model.Clinical.Notes;

namespace MDVision.WebAPI.Controllers
{
    [RoutePrefix("api/Notes")]
    public class NotesController : ApiController
    {
        private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public NotesController()
        {

            _repo = new AuthRepository();
        }
        #region NotesRegion
        [Route("LoadClinicalNotes")]
        [HttpGet]
        public string LoadNotes(string NoteStatus, Int64 PatientID, Int32 PageNumber, Int32 RowsPerPage)
        {
            return new NotesHelpler().LoadClinicalNotes(NoteStatus, PatientID,PageNumber, RowsPerPage );
        }

        [Route("LoadClinicalNoteComponents")]
        [HttpGet]
        public string LoadClinicalNoteComponents(long NotesId, string ComponentName)
        {
            var NoteModel = new NoteComponentModel();
                NoteModel.NotesId = NotesId;
            return ClinicalNotesHelper.Instance().loadNoteComponents(NoteModel);
        }

        [Route("FillClinicalNotes")]
        [HttpGet]
        public string Fill_Clinical_Notes(Int64 NotesId, Int64 PatientID)
        {
            return new NotesHelpler().fillClinicalNoteById(NotesId, PatientID);
        }

        #endregion
    }
}
