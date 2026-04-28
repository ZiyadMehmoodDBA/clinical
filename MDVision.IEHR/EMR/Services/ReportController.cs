using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MDVision.Business.BCommon;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.IEHR.EMR.Model.Medical;
using Newtonsoft.Json.Linq;

using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Model.Reports;
using Newtonsoft.Json;
using MDVision.IEHR.EMR.Helpers.Clinical.MUReports;
using MDVision.IEHR.EMR.Helpers.Clinical.ReportHeader;
using MDVision.IEHR.EMR.Model.ReportHeader;
using MDVision.IEHR.EMR.Helpers.Clinical.Reports;
using MDVision.Model.Clinical.Reports;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.IEHR.Areas.CCM.Helpers;
using MDVision.Model.CCM.Reports;

namespace MDVision.IEHR.EMR.Services
{
    
    public class ReportController : ApiController
    {
        [HttpPost]

        public string MUReport(JObject AllData)
        {
            try
            {
                string response = null;


                JavaScriptSerializer ser = new JavaScriptSerializer();
                MUModel model = JsonConvert.DeserializeObject<MUModel>(MDVUtility.ToStr(AllData["data"]));

                MUHelper helper = new MUHelper();
                if (model.commandType.ToLower() == "search_mustagereport1")
                {
                    response = helper.LoadMUReportData(model);
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public string ReportHeader(JObject AllData)
        {
            string response = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ReportHeader_searchModel model = ser.Deserialize<ReportHeader_searchModel>(MDVUtility.ToStr(AllData["data"]));
            string privilegasMessage = string.Empty;
            if (model.commandType.ToLower() == "search_report_header")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Report_Report Header", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ReportHeaderHelper.Instance().seachReportHeader(model);
                }

            }
            else if (model.commandType.ToLower() == "fill_report_header")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Report_Report Header", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ReportHeaderHelper.Instance().fillReportHeader(model.ReportHeaderId);
                }

            }
            else if (model.commandType.ToLower() == "update_clinical_report_header_active_inactive")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Report_Report Header", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ReportHeaderHelper.Instance().updateClinical_ReportHeaderIsActive(model.ReportHeaderId, model.IsActive);
                }

            }
            else if (model.commandType.ToLower() == "delete_clinical_report_header")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Report_Report Header", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    Int64 ReportHeaderId = MDVUtility.ToInt64(model.ReportHeaderId);
                    response = ReportHeaderHelper.Instance().deleteClinical_ReportHeader(ReportHeaderId);
                }
            }
            else if (model.commandType.ToLower() == "save_report_header")
            {
                  privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Report_Report Header", "ADD")).ToString();
                  if (string.IsNullOrEmpty(privilegasMessage))
                  {
                      ReportHeader_FillModel fillmodel = ser.Deserialize<ReportHeader_FillModel>(MDVUtility.ToStr(AllData["data"]));
                      response = ReportHeaderHelper.Instance().SaveReportHeader(fillmodel);
                  }
            }

            else if (model.commandType.ToLower() == "update_report_header")
            {
                  privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Report_Report Header", "EDIT")).ToString();
                  if (string.IsNullOrEmpty(privilegasMessage))
                  {
                      ReportHeader_FillModel fillmodel = ser.Deserialize<ReportHeader_FillModel>(MDVUtility.ToStr(AllData["data"]));
                      response = ReportHeaderHelper.Instance().updateReportHeader(fillmodel);
                  }
            }

            else if (model.commandType.ToLower() == "get_report_header_tagname")
            {
                    response = ReportHeaderHelper.Instance().GetNoteTemplateTagName();
            }
            else if (model.commandType.ToLower() == "loadheaderconfiuration")
            {
                response = ReportHeaderHelper.Instance().LoadHeaderSettings(model);
            }
            else if (model.commandType.ToLower() == "saveheadersettings")
            {
                privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Report_Report Header", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = ReportHeaderHelper.Instance().UpdateHeaderSettings(model);
                }
            }
            else if (model.commandType.ToLower() == "get_report_header_tags_html")
            {
                response = ReportHeaderHelper.Instance().getReportHeaderTags(model.PatientId, model.ProviderId, -1, model.FormName);
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
        public string ClinicalReports(JObject AllData)
        {
            try
            {
                string response = null;


                JavaScriptSerializer ser = new JavaScriptSerializer();
                dynamic data = JsonConvert.DeserializeObject<dynamic>(MDVUtility.ToStr(AllData["data"]));

                string commandType = data.commandType.Value.ToLower();
                if (commandType == "load_phoneencounter_report")
                {
                    PhoneEncounterSearchModel model = JsonConvert.DeserializeObject<PhoneEncounterSearchModel>(MDVUtility.ToStr(AllData["data"]));
                    response = ClinicalReportsHelper.Instance().LoadPhoneEncounterReport(model);
                }
                else if (commandType == "load_progressnote_report")
                {
                    PhoneEncounterSearchModel model = JsonConvert.DeserializeObject<PhoneEncounterSearchModel>(MDVUtility.ToStr(AllData["data"]));
                    response = ClinicalReportsHelper.Instance().LoadProgressNoteReport(model);
                }
                else if (commandType == "load_allergies_report")
                {
                    CAllergiesModel model = JsonConvert.DeserializeObject<CAllergiesModel>(MDVUtility.ToStr(AllData["data"]));
                    response = ClinicalReportsHelper.Instance().LoadAllergiesReport(model);
                }
                else if (commandType == "load_vitals_report")
                {
                    CVitalsModel model = JsonConvert.DeserializeObject<CVitalsModel>(MDVUtility.ToStr(AllData["data"]));
                    response = ClinicalReportsHelper.Instance().LoadVitalsReport(model);
                }

                else if (commandType == "load_order_report")
                {
                    COrdersModel model = JsonConvert.DeserializeObject<COrdersModel>(MDVUtility.ToStr(AllData["data"]));
                    if (model.OrderType == "Procedure")
                    {
                        response = ClinicalReportsHelper.Instance().LoadProcedureOrdersReport(model);
                    }
                    else
                    {
                        response = ClinicalReportsHelper.Instance().LoadOrdersReport(model);
                    }
                }
                else if (commandType.ToString().ToLower() == "load_problems_report")
                {
                    CProblemsModel model = JsonConvert.DeserializeObject<CProblemsModel>(MDVUtility.ToStr(AllData["data"]));
                    response = ClinicalReportsHelper.Instance().LoadProblemsReport(model);
                }

                else if (commandType == "load_procedures_report")
                {
                    CProceduresModelcs model = JsonConvert.DeserializeObject<CProceduresModelcs>(MDVUtility.ToStr(AllData["data"]));
                    response = ClinicalReportsHelper.Instance().LoadProceduresReport(model);
                }
                else if (commandType == "load_immunization_report")
                {
                    CImmunizationModel model = JsonConvert.DeserializeObject<CImmunizationModel>(MDVUtility.ToStr(AllData["data"]));
                    response = ClinicalReportsHelper.Instance().LoadImmunizationReport(model);
                }
                else if (commandType == "load_medications_report")
                {
                    CMedicationModel model = JsonConvert.DeserializeObject<CMedicationModel>(MDVUtility.ToStr(AllData["data"]));
                    response = ClinicalReportsHelper.Instance().LoadMedicationReport(model);
                }

                else if (commandType == "load_result_report")
                {
                    CResultsModel model = JsonConvert.DeserializeObject<CResultsModel>(MDVUtility.ToStr(AllData["data"]));
                    response = ClinicalReportsHelper.Instance().LoadResultsReport(model);
                }
                // Start || 1 September, 2016 || Talha Tanweer ||
                else if (commandType.ToLower() == "load_consultation_order_report")
                {
                    COrdersModel model = JsonConvert.DeserializeObject<COrdersModel>(MDVUtility.ToStr(AllData["data"]));
                    response = ClinicalReportsHelper.Instance().LoadConsultationOrdersReport(model);
                }
                // End   || 1 September, 2016 || Talha Tanweer ||

                  // Start || 6 September, 2016 || Talha Tanweer ||
                else if (commandType.ToLower() == "get_report_header_footer_clinical_reports")
                {
                  //  COrdersModel model = JsonConvert.DeserializeObject<COrdersModel>(MDVUtility.ToStr(AllData["data"]));
                    response = ClinicalReportsHelper.Instance().GetHeaderFooterForClinicalReports(MDVSession.Current.DefaultProviderId);
                }
                // End   || 6 September, 2016 || Talha Tanweer ||

                else if (commandType == "load_prescriptionorder_report")
                {
                    CPrescriptionOrderModel model = JsonConvert.DeserializeObject<CPrescriptionOrderModel>(MDVUtility.ToStr(AllData["data"]));
                    response = ClinicalReportsHelper.Instance().LoadPrescriptionOrdersReport(model);
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]

        public string CCM_Reports(JObject AllData)
        {
            try
            {
                string response = null;


                JavaScriptSerializer ser = new JavaScriptSerializer();
                dynamic data = JsonConvert.DeserializeObject<dynamic>(MDVUtility.ToStr(AllData["data"]));

                string commandType = data.commandType.Value.ToLower();
                if (commandType == "load_ccm_report")
                {
                    CCM_ReportSearchModel model = JsonConvert.DeserializeObject<CCM_ReportSearchModel>(MDVUtility.ToStr(AllData["data"]));
                    response = CCM_ReportHelper.Instance().Load_CCM_Report(model);
                }
                

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}