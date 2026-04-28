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
    public class ProcedureTemplateController : ApiController
    {
        [HttpPost]
        public string ProcedureTemplate(JObject AllData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();

            ProcedureTemplatesModel model = ser.Deserialize<ProcedureTemplatesModel>(MDVUtility.ToStr(AllData["data"]));
            ProcedureTemplateHelper helper = new ProcedureTemplateHelper();
            PhysicalExamHelper helperPhysicalExam1 = new PhysicalExamHelper();
            if (model.commandType == "Save_ProcedureTemplate")
            {
                response = helper.insertUpdateProcedureTemplate(MDVUtility.ToLong(model.ProcedureTemplateId), model);
            }
            else if (model.commandType == "Load_ProcedureTemplates")
            {
                response = helper.loadProcedureTemplates(model);
            }
            else if (model.commandType == "Load_ProcedureTemplateTests")
            {
                response = helper.ProcedureTemplateTests(model);
            }
            else if (model.commandType == "ProcedureTemplate_ActiveInactive")
            {
                response = helper.ActiveInactiveProcedureTemplate(model);
            }
            else if (model.commandType == "Load_Procedure_TemplatesFill")
            {
                response = null;
                response = helper.loadProcedureTemplates(MDVUtility.ToLong(model.ProcedureTemplateId));
            }
            else if (model.commandType == "Fill_Procedure_Observations_For_Notes")
            {
                response = null;
                ProcedureTemplatesModel.ProcedureNotesObservationModel modeltemSys = ser.Deserialize<ProcedureTemplatesModel.ProcedureNotesObservationModel>(MDVUtility.ToStr(AllData["data"]));
                response = helper.loadProcedureSystemObservationForNotes(MDVUtility.ToLong(modeltemSys.ProcedureTemplatesystemId), MDVUtility.ToLong(modeltemSys.NotesId), MDVUtility.ToInt(modeltemSys.ProcedureId));
            }
            else if (model.commandType == "Get_Procedure_Template_SoapText")
            {
                response = null;
                response = helper.GetProcedureTemplateSoapText(MDVUtility.ToInt(model.ProcedureId), MDVUtility.ToInt64(model.NotesId));
            }
            else if (model.commandType == "Fill_ProcedureOrder_Observations_For_Notes")
            {
                response = null;
                ProcedureTemplatesModel.ProcedureOrderNotesObservationModel modeltemSys = ser.Deserialize<ProcedureTemplatesModel.ProcedureOrderNotesObservationModel>(MDVUtility.ToStr(AllData["data"]));
                response = helper.loadProcedureOrderSystemObservationForNotes(MDVUtility.ToLong(modeltemSys.ProcedureTemplatesystemId), MDVUtility.ToLong(modeltemSys.NotesId), MDVUtility.ToLong(modeltemSys.ProcedureOrderId));
            }

            else if (model.commandType == "DELETE_ProcedureTemplate")
            {
                response = null;
                //response = helperPhysicalExam.deletePhysicalExamTemplate(MDVUtility.ToLong(model.TemplateId));
                response = helper.deleteProcedureTemplate(MDVUtility.ToLong(model.ProcedureTemplateId));

            }
            else if (model.commandType == "Update_Procedure_Selected_Observations_Desc_For_Notes")
            {
                response = null;
                ProcedureTemplatesModel.ProcedureNotesObservationModel modeltem = ser.Deserialize<ProcedureTemplatesModel.ProcedureNotesObservationModel>(MDVUtility.ToStr(AllData["data"]));
                response = helper.UpdateProcedureNotesDescription(MDVUtility.ToLong(modeltem.ProcedureNotesObservationId), MDVUtility.ToStr(modeltem.Descr));
            }
            else if (model.commandType == "Update_ProcedureOrder_Selected_Observations_Desc_For_Notes")
            {
                response = null;
                ProcedureTemplatesModel.ProcedureOrderNotesObservationModel modeltem = ser.Deserialize<ProcedureTemplatesModel.ProcedureOrderNotesObservationModel>(MDVUtility.ToStr(AllData["data"]));
                response = helper.UpdateProcedureOrderNotesDescription(MDVUtility.ToLong(modeltem.ProcedureOrderNotesObservationId), MDVUtility.ToStr(modeltem.Descr));
            }
            else if (model.commandType == "Insert_Observation_System_Assosication")
            {
                response = null;
                ProcedureTemplatesModel.ProcedureTemplatesystemObservationsModel modeltem = ser.Deserialize<ProcedureTemplatesModel.ProcedureTemplatesystemObservationsModel>(MDVUtility.ToStr(AllData["data"]));
                response = helper.associateProcedureObservationAndSystem(MDVUtility.ToLong(modeltem.PESystemId), MDVUtility.ToLong(modeltem.ObservationId), MDVUtility.ToLong(modeltem.PETemplateSystemId));
            }
            return response;
        }
    }
}
