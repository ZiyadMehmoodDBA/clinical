using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MDVision.Business.BCommon;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using MDVision.IEHR.EMR.Model.PQRS;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.PQRSAdmin;
using MDVision.Common.Utilities;
using MDVision.IEHR.EMR.Helpers.Clinical.PQRSReports.Schema;
using Newtonsoft.Json;
using MDVision.IEHR.EMR.Model.QRDA;

namespace MDVision.IEHR.EMR.Services
{
    public class PQRSController : ApiController
    {
        [HttpPost]
        public string PQRSAdmin_MeasureGroup(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            PQRS_MeasureGroupSearchModel model = new PQRS_MeasureGroupSearchModel();
            if (!string.IsNullOrEmpty(MDVUtility.ToStr(AllData["data"])))
            {
                try
                {
                    model = ser.Deserialize<PQRS_MeasureGroupSearchModel>(MDVUtility.ToStr(AllData["data"]));
                }
                catch (Exception ex)
                {

                    model = new PQRS_MeasureGroupSearchModel();
                }

            }
            string privilegasMessage = string.Empty;
            if (model.commandType.ToLower() == "search_pqrs_measuregroups")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = PQRSHelper.Instance().searchMeasureGroup(model);
                }
            }
            else if (model.commandType.ToLower() == "save_pqrs_measuregroups")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    PQRS_MeasureGroupFillModel modelfill = ser.Deserialize<PQRS_MeasureGroupFillModel>(MDVUtility.ToStr(AllData["data"]));
                    response = PQRSHelper.Instance().savePQRS_MeasureGroups(modelfill);
                }
            }
            else if (model.commandType.ToLower() == "fill_pqrs_measuregroups")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = PQRSHelper.Instance().fillPQRS_MeasureGroups(model.MeasureGroupId);
                }
            }
            else if (model.commandType.ToLower() == "update_pqrs_measuregroups")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    PQRS_MeasureGroupFillModel modelfill = ser.Deserialize<PQRS_MeasureGroupFillModel>(MDVUtility.ToStr(AllData["data"]));
                    response = PQRSHelper.Instance().updatePQRS_MeasureGroups(modelfill);
                }
            }

            else if (model.commandType.ToLower() == "update_pqrs_measuregroups_active_inactive")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = PQRSHelper.Instance().updatePQRS_MeasureGroupsIsActive(model.MeasureGroupId, model.IsActive);
                }
            }
            else if (model.commandType.ToLower() == "delete_pqrs_measuregroups")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = PQRSHelper.Instance().deletePQRS_MeasureGroups(model.MeasureGroupId);
                }
            }

            if (!string.IsNullOrEmpty(privilegasMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegasMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (string.IsNullOrEmpty(response))
            {
                var responseObj = new
                {
                    status = false,
                    Message = "Please contact IT administrator, this operation is not invoked"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
            {
                return response;
            }
        }

        [HttpPost]
        public string PQRSAdmin_Measure(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            PQRS_MeasureSearchModel model = new PQRS_MeasureSearchModel();
            if (!string.IsNullOrEmpty(MDVUtility.ToStr(AllData["data"])))
            {
                try
                {
                    model = ser.Deserialize<PQRS_MeasureSearchModel>(MDVUtility.ToStr(AllData["data"]));
                }
                catch (Exception ex)
                {

                    model = new PQRS_MeasureSearchModel();
                }

            }
            string privilegasMessage = string.Empty;
            if (model.commandType.ToLower() == "search_pqrs_measures")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = PQRSHelper.Instance().searchMeasures(model);
                }
            }
            else if (model.commandType.ToLower() == "preview_cmsdocument")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = PQRSHelper.Instance().viewPDFMeasures(model.MeasureId);
                }
            }
            else if (model.commandType.ToLower() == "preview_cmsdocumentbymeasurenumber")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = PQRSHelper.Instance().viewPDFMeasuresByMeasureNumber(model.MeasureNumber);
                }
            }
            if (!string.IsNullOrEmpty(privilegasMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegasMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (string.IsNullOrEmpty(response))
            {
                var responseObj = new
                {
                    status = false,
                    Message = "Please contact IT administrator, this operation is not invoked"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
            {
                return response;
            }
        }



        [HttpPost]
        public string PQRSAdmin_IndividualReporting(JObject AllData)
         {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            PQRS_IndividualReportingSearchModel model = new PQRS_IndividualReportingSearchModel();
            if (!string.IsNullOrEmpty(MDVUtility.ToStr(AllData["data"])))
            {
                try
                {
                    model = ser.Deserialize<PQRS_IndividualReportingSearchModel>(MDVUtility.ToStr(AllData["data"]));
                }
                catch (Exception ex)
                {

                    model = new PQRS_IndividualReportingSearchModel();
                }

            }
            string privilegasMessage = string.Empty;
            string FormName = "Clinical_PQRS_Individual Reporting";
            if (model.commandType.ToLower() == "search_pqrs_individualreporting")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity(FormName, "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = PQRSHelper.Instance().searchMeasureIndividual(model);
                }
            }
            else if (model.commandType.ToLower() == "update_pqrs_measureindividual_active_inactive")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity(FormName, "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = PQRSHelper.Instance().updatePQRS_MeasureIndividualIsActive(model.MeasureIndividualId, model.IsActive);
                }
            }

            else if (model.commandType.ToLower() == "update_pqrs_measuregroup_active_inactive")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity(FormName, "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    PQRS_IndividualReportingFillModel modelfill = ser.Deserialize<PQRS_IndividualReportingFillModel>(MDVUtility.ToStr(AllData["data"]));

                    response = PQRSHelper.Instance().updatePQRS_MeasureGroupIsActive(modelfill.MeasureGroupId, modelfill.is_Active);
                }
            }

            else if (model.commandType.ToLower() == "delete_pqrs_measureindividual")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity(FormName, "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = PQRSHelper.Instance().deletePQRS_MeasureIndividual(model.MeasureIndividualId);
                }
            }

            else if (model.commandType.ToLower() == "delete_pqrs_measuregroup")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity(FormName, "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    PQRS_IndividualReportingFillModel modelfill = ser.Deserialize<PQRS_IndividualReportingFillModel>(MDVUtility.ToStr(AllData["data"]));

                    response = PQRSHelper.Instance().deletePQRS_MeasureGroup(modelfill.MeasureGroupId);
                }
            }
            else if (model.commandType.ToLower() == "save_pqrs_measureindividual")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    PQRS_IndividualReportingFillModel modelfill = ser.Deserialize<PQRS_IndividualReportingFillModel>(MDVUtility.ToStr(AllData["data"]));
                    response = PQRSHelper.Instance().savePQRS_MeasureIndividual(modelfill);
                }
            }
            else if (model.commandType.ToLower() == "save_pqrs_measuregroup")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    PQRS_IndividualReportingFillModel modelfill = ser.Deserialize<PQRS_IndividualReportingFillModel>(MDVUtility.ToStr(AllData["data"]));
                    response = PQRSHelper.Instance().savePQRS_MeasureGroup(modelfill);
                }
            }
            else if (model.commandType.ToLower() == "update_pqrs_measuregroup")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    PQRS_IndividualReportingFillModel modelfill = ser.Deserialize<PQRS_IndividualReportingFillModel>(MDVUtility.ToStr(AllData["data"]));
                    response = PQRSHelper.Instance().updatePQRS_MeasureGroup(modelfill);
                }
            }
            else if (model.commandType.ToLower() == "fill_pqrs_measureindividual")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = PQRSHelper.Instance().fillPQRS_MeasureIndividual(model.MeasureIndividualId);
                }
            }
            else if (model.commandType.ToLower() == "fill_pqrs_measuregroupdetails")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    PQRS_IndividualReportingFillModel modelfill = ser.Deserialize<PQRS_IndividualReportingFillModel>(MDVUtility.ToStr(AllData["data"]));
                    response = PQRSHelper.Instance().fillPQRS_MeasureGroupdetails (long.Parse( modelfill.MeasureGroupId));
                }
            }

            else if (model.commandType.ToLower() == "fill_pqrs_measuregroupdata")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    PQRS_IndividualReportingFillModel modelfill = ser.Deserialize<PQRS_IndividualReportingFillModel>(MDVUtility.ToStr(AllData["data"]));
                    response = PQRSHelper.Instance().fillPQRS_MeasureGroupData(modelfill);
                }
            }
           
            else if (model.commandType.ToLower() == "update_pqrs_measureindividual")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    PQRS_IndividualReportingFillModel modelfill = ser.Deserialize<PQRS_IndividualReportingFillModel>(MDVUtility.ToStr(AllData["data"]));
                    response = PQRSHelper.Instance().updatePQRS_MeasureIndividual(modelfill);
                }
            }
            if (!string.IsNullOrEmpty(privilegasMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegasMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (string.IsNullOrEmpty(response))
            {
                var responseObj = new
                {
                    status = false,
                    Message = "Please contact IT administrator, this operation is not invoked"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
            {
                return response;
            }
        }


        [HttpPost]
        public string PQRSReports(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            PQRSReportsModel model = new PQRSReportsModel();
            if (!string.IsNullOrEmpty(MDVUtility.ToStr(AllData["data"])))
            {
                try
                {
                    model = ser.Deserialize<PQRSReportsModel>(MDVUtility.ToStr(AllData["data"]));
                }
                catch (Exception ex)
                {

                    model = new PQRSReportsModel();
                }

            }
            string privilegasMessage = string.Empty;
            if (model.commandType.ToLower() == "generate_individual_report")
            {
                // privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "SEARCH")).ToString();
                //if (string.IsNullOrEmpty(privilegasMessage))
                //{
                response = PQRSHelper.Instance().generateIndividualReport(model);
                // }
            }
            if (model.commandType.ToLower() == "generate_gpro_report")
            {
                //privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "SEARCH")).ToString();
                //if (string.IsNullOrEmpty(privilegasMessage))
                //{
                response = PQRSHelper.Instance().generateGPROReport(model);
                //}
            }
            if (model.commandType.ToLower() == "generate_summary_report")
            {
                // privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "SEARCH")).ToString();
                //if (string.IsNullOrEmpty(privilegasMessage))
                //{
                response = PQRSHelper.Instance().generateSummaryReport(model);
                // }
            }
            else if (model.commandType.ToLower() == "preview_cmsdocument")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_PQRS_GPRO Submission", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    // response = PQRSHelper.Instance().viewPDFMeasures(model.MeasureId);
                }
            }
            if (model.commandType.ToLower() == "pqrs_get_patients_from_visits")
            {
                response = PQRSHelper.Instance().PQRS_GetPatientsFromVisits(model.VisitsID);
            }else if (model.commandType.ToLower() == "pqrs_getmeasurereasons")
            {
                response = PQRSHelper.Instance().PQRS_GetMeasureReasons(model.MeasureId);
            }
            if (!string.IsNullOrEmpty(privilegasMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegasMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (string.IsNullOrEmpty(response))
            {
                var responseObj = new
                {
                    status = false,
                    Message = "Please contact IT administrator, this operation is not invoked"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
            {
                return response;
            }
        }



        [HttpPost]
        public string PQRSPatientList(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            PQRSPatientListModel model = new PQRSPatientListModel();
            if (!string.IsNullOrEmpty(MDVUtility.ToStr(AllData["data"])))
            {
                try
                {
                    model = ser.Deserialize<PQRSPatientListModel>(MDVUtility.ToStr(AllData["data"]));
                    if (model.json != "" && model.json != null)
                    {
                        var js = ser.Deserialize<List<string>>(model.json);
                        model.missingDataList = new List<MissingData>();
                        foreach (var item in js)
                        {
                            MissingData obj = ser.Deserialize<MissingData>(item);
                            model.missingDataList.Add(obj);
                        }
                    }
                }
                catch (Exception ex)
                {

                    model = new PQRSPatientListModel();
                }

            }
            string privilegasMessage = string.Empty;
            if (model.CommandType.ToLower() == "save_patientlist")
            {
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = PQRSHelper.Instance().saveMissingData(model);
                }
            }

            if (!string.IsNullOrEmpty(privilegasMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegasMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (string.IsNullOrEmpty(response))
            {
                var responseObj = new
                {
                    status = false,
                    Message = "Please contact IT administrator, this operation is not invoked"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
            {
                return response;
            }
        }


        [HttpPost]
        public string PQRS_QRDA1(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            PQRSReportsModel model = new PQRSReportsModel();
            if (!string.IsNullOrEmpty(MDVUtility.ToStr(AllData["data"])))
            {
                try
                {
                    model = ser.Deserialize<PQRSReportsModel>(MDVUtility.ToStr(AllData["data"]));
                }
                catch (Exception ex)
                {

                    model = new PQRSReportsModel();
                }

            }
            string privilegasMessage = string.Empty;
            if (model.commandType.ToLower() == "generate_qrda1_report")
            {
               // List<string> listpatient = new List<string>(new string[]{"244710" ,"244711" ,"244712" ,"244713" });
                //response = PQRSHelper.Instance().generateIndividualReport(model);
                Int64 providerId = string.IsNullOrEmpty(model.ProviderId) ? -1 : MDVUtility.ToInt64(model.ProviderId);
                var base64StringToDownload = PqrsQrdaOneXMLGenerator.InitializeQrdaOne_DataSets(model.MeasureId, -1, providerId, 1, model.ReportFromDate, model.ReportToDate);
                response = DownloadQRDA1(base64StringToDownload, "");
                return response;

            }

            if (!string.IsNullOrEmpty(privilegasMessage))
            {
                var responseObj = new
                {
                    status = false,
                    Message = privilegasMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else if (string.IsNullOrEmpty(response))
            {
                var responseObj = new
                {
                    status = false,
                    Message = "Please contact IT administrator, this operation is not invoked"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
            }
            else
            {
                return response;
            }
        }
        private string DownloadQRDA1(List<KeyValuePair<string, string>> base64Strings, string providerName)
        {
            if (base64Strings.Count > 0)
            {
                var response = new
                {
                    status = true,
                    DownloadFile = base64Strings,
                    FileName = DateTime.Now.ToShortDateString() + " - QRDA I",
                    ProviderName = providerName
                };
                return (JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = true,
                    Message = "Nothing To Download."
                };
                return (JsonConvert.SerializeObject(response));
            }
        }




    }
}