using System.Web.Http;
using System.Web.Script.Serialization;
using MDVision.Common.Utilities;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.Model.Clinical.Medical.Implantable;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MDVision.IEHR.EMR.Services
{
    public class ImplantableController : ApiController
    {
        [HttpPost]
        public string Implantable(JObject allData)
        {
            string response;
            var ser = new JavaScriptSerializer();
            var model = ser.Deserialize<ImplantableDevices>(MDVUtility.ToStr(allData["data"]));
            var helper = new ImplantableHelper();

            switch (model.commandType.ToLower())
            {
                case "load_implantabledevices":
                    response = helper.LoadImplantableDevice(model);
                    break;
                case "delete_implantabledevice":
                    response = helper.DeleteImplantableDevices(model);
                    break;
                case "activeinactive_implantabledevice":
                    response = helper.ActiveInactiveImplantableDevices(model);
                    break;
                case "get_devices_forsoap":
                    response = helper.GetDevicesForSoap(model.ImplantableDevicesPKId, MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteProviderId));
                    break;
                case "detach_devices_from_notes":
                    response = helper.DetachDevicesFromNotes(model.ImplantableDevicesPKId, MDVUtility.ToInt64(model.NotesId));
                    break;
                case "attach_devices_with_notes":
                    response = helper.AttachDevicesWithNotes(model.ImplantableDevicesPKId, model.NotesId);
                    break;
                case "save_associated_procedures":
                    response = helper.insertAssociatedProcedures(model);
                    break;
                //case "getlatest_implantabledevice_bypatientid":
                //    response = helper.getLatestImplantableDevicesByPatientId(MDVUtility.ToInt64(model.PatientId));
                //    break;
                default:
                    var errorMessage = new
                    {
                        status = false,
                        Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                    };
                    return (JsonConvert.SerializeObject(errorMessage));
            }
            return response;
        }

        public string ImplantableDetail(JObject allData)
        {
            string response;
            var ser = new JavaScriptSerializer();
            var model = ser.Deserialize<ImplantableDevices>(MDVUtility.ToStr(allData["data"]));
            var helper = new ImplantableHelper();

            switch (model.commandType.ToLower())
            {
                case "verify_udi":
                    response = helper.VerifyUdi(model);
                    break;
                case "save_implantabledevices":
                    response = helper.InsertImplantableDevices(model);
                    break;
                case "fill_implantabledevice":
                    response = helper.FillImplantableDevices(model);
                    break;
                case "update_implantabledevices":
                    response = helper.UpdateImplantableDevices(model);
                    break;
                case "targetsite_lookup":
                    response = helper.TargetSiteLookUp(model.TargetSite);
                    break;
                case "delete_implantable_device_procedure":
                    response = helper.DeleteImplantableDeviceProcedure(model.ProcedureId);
                    break;
                default:
                    var errorMessage = new
                    {
                        status = false,
                        Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                    };
                    return (JsonConvert.SerializeObject(errorMessage));
            }
            return response;
        }
    }
}
