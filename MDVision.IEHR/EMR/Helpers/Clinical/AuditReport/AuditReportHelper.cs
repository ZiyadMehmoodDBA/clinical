// Author:  Muhammad Arshad
// Created Date: 14/04/2016
//OverView: Helper class for FaceSheet
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.EMR.Model.AuditReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Text;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Model.AuditableEvents;
using MDVision.Model.User;
using MDVision.IEHR.Common;
using Newtonsoft.Json;

namespace MDVision.IEHR.EMR.Helpers.Clinical.AuditReport
{
    public class AuditReportHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public AuditReportHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static AuditReportHelper _instance = null;
        public static AuditReportHelper Instance()
        {
            if (_instance == null)
                _instance = new AuditReportHelper();
            return _instance;
        }

        #region fillAuditReport

        // Author:  Muhammad Arshad
        // Created Date: 14/04/2016
        //OverView: This function will handle fill of AuditReport
        public string LoadAuditReport(AuditReportModel model, int PageNumber, int RowsPerPage)
        {
            try
            {
                DSDBAudit dsDBAudit = null;
                BLObject<DSDBAudit> obj;
                DataTable dtAuditUserIds = new DataTable();
                DataTable dtPatientIds = new DataTable();

                dtAuditUserIds = MDVUtility.ConvertCommaSepatedValuesToDataTable(model.AuditUserIds);
                dtPatientIds = MDVUtility.ConvertCommaSepatedValuesToDataTable(model.PatientId);
                obj = BLLClinicalObj.LoadAuditReport("", ref dtAuditUserIds, "", "", "", ref dtPatientIds, "", null, "", "", "",model.AuditAction,model.CreatedDateFrom, model.CreatedDateTo,PageNumber,RowsPerPage, model.UserType);

                dsDBAudit = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,

                        DBAuditCount = dsDBAudit.Tables[dsDBAudit.DBAudit.TableName].Rows.Count,
                        iTotalDisplayRecords = (dsDBAudit.DBAudit.Rows.Count > 0) ? dsDBAudit.DBAudit.Rows[0][dsDBAudit.DBAudit.RecordCountColumn.ColumnName] : 0,
                        DBAuditLoad_JSON = MDVUtility.JSON_DataTable(dsDBAudit.Tables[dsDBAudit.DBAudit.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        DBAuditCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string LoadUserAuditReport(AuditReportModel model, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Clinical_Report_Audit Report", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    List<ActivityLogUser> obj;
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    DataTable dtAuditUserIds = new DataTable();

                    dtAuditUserIds = MDVUtility.ConvertCommaSepatedValuesToDataTable(model.AuditUserIds);
                    obj = BLLClinicalObj.LoadUserAuditReport(model.UserType, ref dtAuditUserIds, model.FromDate, model.ToDate, model.AuditAction, model.PageNumber, model.RowsPerPage);



                    if (obj.Count > 0)
                    {
                        var response = new
                        {
                            status = true,

                            DBAuditCount = obj.Count,
                            iTotalDisplayRecords = (obj.Count > 0) ? int.Parse(obj[0].RecordCount) : 0,
                            DBAuditLoad_JSON = js.Serialize(obj),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            DBAuditCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public List<LookupRoles> GetLookupRoles()
        {
            return BLLClinicalObj.GetLookupRoles();
        }

        #endregion

        #region "PDF View of AuditReport"
        // Author:  Muhammad Arshad
        // Created Date: 14/04/2016
        //Overview: This function will load pdf for Audit Report

        public string previewAuditReport(AuditReportModel model)
        {
            try
            {
               
                
                if (model.PatientId == "0")
                {
                    model.PatientId = "";
                }
                BLObject<byte[]> objLoad = BLLClinicalObj.previewAuditReport("", model.AuditUserName, "", "", "", model.PatientId, "", null, "", "", "", model.AuditAction, model.CreatedDateFrom, model.CreatedDateTo);


                if (objLoad.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        AuditReportHTML = Convert.ToBase64String(objLoad.Data),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        AuditReportCount = 0,
                        Message = objLoad.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion
    }
}