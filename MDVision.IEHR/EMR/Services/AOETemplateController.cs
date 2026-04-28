using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.PhysicalExam;
using MDVision.IEHR.EMR.Helpers.Clinical.Templates;
using MDVision.IEHR.EMR.Model.PhysicalExam;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;
using MDVision.Model.Clinical.AOETemplates;

namespace MDVision.IEHR.EMR.Services
{
    public class AOETemplateController : ApiController
    {
        [HttpPost]
        public string AOETemplate(JObject AllData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();

            AOETemplatesModel model = ser.Deserialize<AOETemplatesModel>(MDVUtility.ToStr(AllData["data"]));
            AOETemplateHelper helper = new AOETemplateHelper();
            PhysicalExamHelper helperPhysicalExam1 = new PhysicalExamHelper();
            if (model.commandType == "Save_AOETemplate")
            {
                response = helper.insertUpdateAOETemplate(MDVUtility.ToLong(model.AOETemplateId), model);
            }
            else if (model.commandType == "Load_AOETemplates")
            {
                response = helper.loadAOETemplates(model);
            }
            else if (model.commandType == "AOETemplate_ActiveInactive")
            {
                response = helper.ActiveInactiveAOETemplate(model);
            }
            else if (model.commandType == "Load_AOE_TemplatesFill")
            {
                response = null;
                response = helper.loadAOETemplates(MDVUtility.ToLong(model.AOETemplateId));
            }
            else if (model.commandType == "Fill_AOE_Observations_For_Notes")
            {
                response = null;
                AOETemplatesModel.AOENotesObservationModel modeltemSys = ser.Deserialize<AOETemplatesModel.AOENotesObservationModel>(MDVUtility.ToStr(AllData["data"]));
                response = helper.loadAOESystemObservationForNotes(MDVUtility.ToLong(modeltemSys.AOETemplateSystemId), MDVUtility.ToLong(modeltemSys.NotesId));
            }
            else if (model.commandType == "Fill_AOE_Radiology_Observations_For_Notes")
            {
                response = null;
                AOETemplatesModel.AOENotesRadiologyObservationModel modeltemSys = ser.Deserialize<AOETemplatesModel.AOENotesRadiologyObservationModel>(MDVUtility.ToStr(AllData["data"]));
                response = helper.loadAOESystemRadiologyObservationForNotes(MDVUtility.ToLong(modeltemSys.AOETemplateSystemId), MDVUtility.ToLong(modeltemSys.NotesId));
            }

            else if (model.commandType == "DELETE_AOETemplate")
            {
                response = null;
                //response = helperPhysicalExam.deletePhysicalExamTemplate(MDVUtility.ToLong(model.TemplateId));
                response = helper.deleteAOETemplate(MDVUtility.ToLong(model.AOETemplateId));

            }
            else if (model.commandType == "Update_AOE_Selected_Observations_Desc_For_Notes")
            {
                response = null;
                AOETemplatesModel.AOENotesObservationModel modeltem = ser.Deserialize<AOETemplatesModel.AOENotesObservationModel>(MDVUtility.ToStr(AllData["data"]));
                response = helper.UpdateAOENotesDescription(MDVUtility.ToLong(modeltem.AOENotesObservationId), MDVUtility.ToStr(modeltem.Descr));
            }
            else if (model.commandType == "Update_AOE_Selected_Radiology_Observations_Desc_For_Notes")
            {
                response = null;
                AOETemplatesModel.AOENotesRadiologyObservationModel modeltem = ser.Deserialize<AOETemplatesModel.AOENotesRadiologyObservationModel>(MDVUtility.ToStr(AllData["data"]));
                response = helper.UpdateAOENotesRadiologyDescription(MDVUtility.ToLong(modeltem.AOENotesRadiologyObservationId), MDVUtility.ToStr(modeltem.Descr));
            }
            else if (model.commandType == "Insert_Observation_System_Assosication")
            {
                response = null;
                AOETemplatesModel.AOETemplateSystemObservationsModel modeltem = ser.Deserialize<AOETemplatesModel.AOETemplateSystemObservationsModel>(MDVUtility.ToStr(AllData["data"]));
                response = helper.associateAOEObservationAndSystem(MDVUtility.ToLong(modeltem.PESystemId), MDVUtility.ToLong(modeltem.ObservationId), MDVUtility.ToLong(modeltem.PETemplateSystemId));
            }
            return response;
        }
    }
}
