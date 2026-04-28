using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.IEHR.EMR.Model.Clinical.Immunization;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.Model.Clinical.Orderset;
using MDVision.Model.Clinical.Templates.OrderSets;
using MDVision.Model.Lookups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates.OrderSet
{
    public class OS_MedicationHelper
    {
        private BLLOrderSet BLLOrderSetObj = null;
        public OS_MedicationHelper()
        {
            BLLOrderSetObj = new BLLOrderSet();
            // BLLRcopiaObj = new BLLRcopia();
        }
        private static OS_ImmunizationHelper _instance = null;
        //private static bool isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
        public static OS_ImmunizationHelper Instance()
        {
            if (_instance == null)
            {
                _instance = new OS_ImmunizationHelper();
                //   isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
            }
            return _instance;
        }
        public string SaveOSMedication(OS_MedicationModel model)
        {
            try
            {
                string OS_MedicationId = BLLOrderSetObj.SaveOSMedication(model);
                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        public string LoadMedication(OS_MedicationModel model)
        {
            try
            {
                List<OS_MedicationModel> MedicationList = null;
                BLObject<List<OS_MedicationModel>> obj;

                obj = BLLOrderSetObj.LoadMedication(model.OrdersetId, model.OS_MedicationId, MDVUtility.ToInt(model.pageNumber), MDVUtility.ToInt(model.rowsPerPage));
                MedicationList = obj.Data;
                if (obj.Data != null)
                {
                    if (MedicationList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            iTotalDisplayRecords=MedicationList[0].RecordCount,
                            MedicationCount = MedicationList.Count,
                            Medication_JSON = MedicationList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MedicationCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string UpdateOSMedication(OS_MedicationModel model)
        {
            try
            {
                string MedicationId = BLLOrderSetObj.UpdateOSMedication(model);

                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Update_Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string DeleteOSMedication(OS_MedicationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.OS_MedicationId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {

                    BLObject<string> obj = BLLOrderSetObj.DeleteOsMedication(model.OS_MedicationId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
    }
}