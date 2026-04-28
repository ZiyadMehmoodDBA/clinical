/* Author: Zia Mehmood
 * Created Date: 10/11/2017
 * OverView: Created to handel Export CCDA
 */

using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Batch.ExportCCDA;
using MDVision.Model.Batch.ExportCCDA;
using Newtonsoft.Json.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class Batch_ExportCCDAController : ApiController
    {
        [HttpPost]
        
        public string ExportCCDA(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ExportCCDAModel modelExportCCDA = ser.Deserialize<ExportCCDAModel>(MDVUtility.ToStr(AllData["data"]));
            string privilegesMessage = string.Empty;
            HelperExportCCDA helperExportCCDA = new HelperExportCCDA();

            if (modelExportCCDA.commandType.ToUpper() == "FILL_PATIENT_LOOKUP")
            {
                response = null;
                response = helperExportCCDA.Fill_Paitent_Lookpup(modelExportCCDA);
            }
            else if (modelExportCCDA.commandType.ToUpper() == "FILL_NOTECOMPONENT_LOOKPUP")
            {
                response = null;
                response = helperExportCCDA.Fill_NoteComponent_Lookpup(modelExportCCDA);
            }
            else if (modelExportCCDA.commandType.ToUpper() == "SAVE_CCDASCHEDULE")
            {
                privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ExportCCDA", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = null;
                    response = helperExportCCDA.Insert_CCDA_Schedule(modelExportCCDA);
                }
            }
            else if (modelExportCCDA.commandType.ToUpper() == "LOAD_CCDA_SCHEDULE")
            {
                privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ExportCCDA", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = null;
                    response = helperExportCCDA.Load_CCDA_Schedule(modelExportCCDA);
                }
            }
            else if (modelExportCCDA.commandType.ToUpper() == "SELECT_CCDA_SCHEDULE")
            {
                response = null;
                response = helperExportCCDA.Select_CCDA_Schedule(modelExportCCDA);
            }
            else if (modelExportCCDA.commandType.ToUpper() == "DELETE_CCDA_SCHEDULE")
            {
                privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ExportCCDA", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = null;
                    response = helperExportCCDA.Delete_CCDA_Schedule(modelExportCCDA.SchedulerId);
                }
            }
            else if (modelExportCCDA.commandType.ToUpper() == "ACTIVE_INACTIVE_CCDA_SCHEDULE")
            {
                privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ExportCCDA", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    response = null;
                    response = helperExportCCDA.ActiveInactive_CCDA_Schedule(modelExportCCDA.SchedulerId, modelExportCCDA.IsActive);
                }
            }
            else if (modelExportCCDA.commandType.ToUpper() == "GET_SCHEDULED_PATIENTVISITS")
            {
                response = null;
                response = helperExportCCDA.Get_Scheduled_PatientVisits(modelExportCCDA);

            }

            if (!string.IsNullOrEmpty(privilegesMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegesMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
                return response;
        }
    }
}