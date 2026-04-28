using MDVision.Common.Utilities;
using MDVision.IEHR.EMR.Helpers.Clinical.Orthopedic;
using MDVision.IEHR.EMR.Helpers.Patient;
using MDVision.Model.Clinical.Orthopedic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class OrthopedicController : ApiController
    {
        [HttpPost]
        public string Orthopedic(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            OrthopedicModel model = ser.Deserialize<OrthopedicModel>(MDVUtility.ToStr(AllData["data"]));
            OrthopedicHelper helper = new OrthopedicHelper();


            if (model.commandType.ToLower() == "load_bodyparts_by_notes")
            {
                response = helper.loadBodyPartsByNotes(model);
            }
            if (model.commandType.ToLower() == "load_bodyparts")
            {
                response = helper.loadBodyParts();
            }
            else if (model.commandType.ToLower() == "load_orthopedic_complaints")
            {
                response = helper.loadOrthopedicComplaints();
            }
            else if (model.commandType.ToLower() == "save_orthopedic_complaints")
            {
                response = helper.saveOrthopedicComplaints(model.Complaint);
            }
            else if (model.commandType.ToLower() == "save_bodypart_and_complaint")
            {
                response = helper.saveBodyPartAndComplaint(model);
            }
            else if (model.commandType.ToLower() == "delete_bodypart_and_complaint")
            {
                response = helper.deleteBodyPartAndComplaint(model);
            }
            else if (model.commandType.ToLower() == "delete_orthopedic_complaints")
            {
                response = helper.deleteOrthopedicComplaints(model.OrthopedicComplainId);
            }
            else if (model.commandType.ToLower() == "load_notes_bodyparts_complaints")
            {
                response = helper.loadNotesBodyPartsComplaints(model.NotesId);
            }
            return response;
        }
        [HttpPost]
        public string OrthopedicFavoriteList(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            OrthoFavListModel model = ser.Deserialize<OrthoFavListModel>(MDVUtility.ToStr(AllData["data"]));
            OrthopedicHelper helper = new OrthopedicHelper();

            if (model.commandType.ToLower() == "load_favoritelist_orthopedic")
            {
                response = helper.LoadOrthopedicFavoriteList(model);
            }
            return response;
        }
    }
}