using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Helpers.Clinical.Templates;
using MDVision.Model.Clinical.BillingInformation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

using MDVision.IEHR.EMR.Helpers.Clinical.BillingInformation;
namespace MDVision.IEHR.EMR.Services
{
    public class BillingInformationController : ApiController
    {

        public string BillingInformation(JObject AllData)
        {
            var response = new { status = true };


            JavaScriptSerializer ser = new JavaScriptSerializer();


            BillingInformationModel model = JsonConvert.DeserializeObject<BillingInformationModel>(MDVUtility.ToStr(AllData["data"]));
            BillingInformationHelper objHelper = BillingInformationHelper.Instance();
            EMCodeGeneratorHelper objEMCodeHelper = EMCodeGeneratorHelper.Instance();
            if (model.commandType.ToLower() == "get_appointmentidbynoteid")
            {
                model.AppointmentId = objHelper.GetAppointmentIdByNoteId(model.NotesId);
                var responseObj = new
                {
                    status = true,
                    AppointmentId = model.AppointmentId
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (model.commandType.ToLower() == "billing_information_time_load")
            {
                DSBillingInformationLookup ds = objHelper.Get_LookupBillingInfoTime();

                var responseObj = new
                {
                    status = true,
                    BillingInfoTime = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[ds.BillingInfoTime.TableName])
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (model.commandType.ToLower() == "billing_information_save")
            {
                var result = objHelper.BillingInfo_Save(model);
                return result;
            }
            else if (model.commandType.ToLower() == "signed_billinginfo")
            {
                var result = objHelper.Signed_BillingInfo(model);
                return result;
            }
            else if (model.commandType.ToLower() == "delete_cptid")
            {
                BillingInfoCPTModel modelCPT = JsonConvert.DeserializeObject<BillingInfoCPTModel>(MDVUtility.ToStr(AllData["data"]));
                var result = objHelper.BillingInfoCPT_DELETE(modelCPT.BillingInfoId, model.CPTCode, MDVUtility.ToInt64(model.PatientId), model.NotesId);
                return result;
            }
            else if (model.commandType.ToLower() == "billing_information_select")
            {
                var result = objHelper.BillingInfo_SELECT(MDVUtility.ToInt64(model.NotesId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.BillingInfoId));
                return result;
            }
            else if (model.commandType.ToLower() == "is_billing_information_created")
            {
                var result = objHelper.IsBillingInfoCreated(MDVUtility.ToInt64(model.NotesId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.BillingInfoId));
                return result;
            }
            else if (model.commandType.ToLower() == "billing_information_select_by_visitid")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(MDVision.IEHR.Common.AppPrivileges.GetFormSecurity("Notes_Notes", "EDIT")).ToString();

                  if (string.IsNullOrEmpty(privilegasMessage))
                  {
                      var result = objHelper.BillingInfo_SELECT_By_VisitId(MDVUtility.ToInt64(model.VisitId));
                      return result;
                  }
                  else
                  {
                      var responseObj = new
                      {
                          status = false,
                          Message = privilegasMessage
                      };
                      return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                  }
               
            }
            else if (model.commandType.ToLower() == "billing_information_select_customized")
            {
                var result = objHelper.BillingInfo_SELECT_Customized(MDVUtility.ToInt64(model.NotesId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.BillingInfoId));
                return result;
            }
            else if (model.commandType.ToLower() == "billing_information_select_out_of_offive_visit")
            {
               // DateTime? DosFrom = model.ENMCPTDOSFrom == "" ? null : Utility.ToDateTime?(model.ENMCPTDOSFrom);
               // DateTime? DosTo =model.ENMCPTDOSTo == "" ? null : Utility.ToDateTime?(model.ENMCPTDOSTo);
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(MDVision.IEHR.Common.AppPrivileges.GetFormSecurity("Miscellaneous_eSuperbill", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    var result = objHelper.BillingInfo_SELECT_OutOfOfficeVisit(MDVUtility.ToStr(model.ICDCode), MDVUtility.ToStr(model.CPTCode), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.FacilityId), model.Status, model.ENMCPTDOSFrom, model.ENMCPTDOSTo, model.PageNumber, model.RowsPerPage);
                    return result;
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }
                
            }
            else if (model.commandType.ToLower() == "loadattachedproceduresandproblems")
            {
                var result = objHelper.LoadAttachedProceduresAndProblems(MDVUtility.ToInt64(model.NotesId), MDVUtility.ToInt64(model.PatientId));
                return result;
            }
            else if (model.commandType.ToLower() == "loadattachedproceduresandproblemsforsign")
            {
                var result = objHelper.LoadAttachedProceduresAndProblemsForSign(MDVUtility.ToInt64(model.NotesId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.BillingInfoId));
                return result;
            }
            else if (model.commandType.ToLower() == "loadproceduresandproblemsbynoteandpatientid")
            {
                var result = objHelper.LoadProceduresAndProblems(MDVUtility.ToInt64(model.NotesId), MDVUtility.ToInt64(model.PatientId));
                return result;
            }
            
            else if (model.commandType.ToLower() == "insert_missing_cpt_in_billing_info_cpt")
            {
                var result = objHelper.InsertMissingBillingCpt(MDVUtility.ToInt64(model.NotesId), MDVUtility.ToInt64(model.BillingInfoId));
                return result;
            }
            else if (model.commandType.ToLower() == "billing_information_delete")
            {
                var result = objHelper.BillingInfo_DELETE(model.BillingInfoId);
                return result;
            }
            else if (model.commandType.ToLower() == "get_emcodesuggestion")
            {
                if (model.ENMTypeId == "1")
                {
                    var result = objEMCodeHelper.SuggestNewPatientEMCodes(Convert.ToInt64(model.PatientId), model.NotesId);
                    return result;
                }
                else if (model.ENMTypeId == "2")
                {
                    var result = objEMCodeHelper.SuggestEstablishedPatientEMCodes(Convert.ToInt64(model.PatientId), model.NotesId);
                    return result;
                }
            }
            else if (model.commandType.ToLower() == "is_cpt_exsists_in_esupperbill")
            {
                var result = objHelper.IsCptExsistsInEsupperbill(MDVUtility.ToInt64(model.NotesId));
                return result;
            }
            

            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }

    }
}
