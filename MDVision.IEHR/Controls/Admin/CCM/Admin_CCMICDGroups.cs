using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Web;

namespace MDVision.IEHR.Controls.Admin.CCM
{
    public class Admin_CCMICDGroups
    {
        private BLLAdmin BLLAdminObj = null;
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_CCMICDGroups()
        {
            BLLAdminObj = new BLLAdmin();
            BLLAdminCodesObj = new BLLAdminCodes();
        }

        #region Singleton
        private static Admin_CCMICDGroups _obj = null;
        public static Admin_CCMICDGroups Instance()
        {
            if (_obj == null)
                _obj = new Admin_CCMICDGroups();
            return _obj;
        }
        #endregion

        #region Private Functions

        #region ICD Group

        /// <summary>
        /// LoadCCMICDGroups
        /// </summary>
        /// <param name="ICDGroupsData"></param>
        /// <param name="ICDGroupId"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        private string LoadCCMICDGroups(string ICDGroupsData, long ICDGroupId, Int32 PageNumber, Int32 RowsPerPage)
        {
            DSCCM ds = null;
            var ShortName = "";
            var Description = "";
            bool IsActive = false;
            dynamic data;

            if (!string.IsNullOrEmpty(ICDGroupsData))
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                data = ser.Deserialize<dynamic>(ICDGroupsData);

                if (!string.IsNullOrEmpty(Convert.ToString(data["ShortName"])))
                    ShortName = Convert.ToString(data["ShortName"]);

                if (!string.IsNullOrEmpty(Convert.ToString(data["IsActive"])))
                    IsActive = Convert.ToBoolean(data["IsActive"]);
            }

            BLObject<DSCCM> obj = BLLAdminObj.loadICDGroups(ICDGroupId, ShortName, IsActive, PageNumber, RowsPerPage);
            if (obj.Data != null)
            {
                ds = obj.Data;
                if (ds.Tables[ds.ICDGroups.TableName].Rows.Count > 0)
                {
                    var rows = ds.Tables[ds.ICDGroups.TableName];
                    var dataRows = MDVUtility.JSON_DataTable(rows);
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ICDGroupId = ds.Tables[ds.ICDGroups.TableName].Rows[0][ds.ICDGroups.ICDGroupIdColumn.ColumnName],
                        CCMICDGroupFill_JSON = dataRows
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            else
            {
                var response = new
                {
                    status = false,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// SaveICDGroup
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <returns></returns>
        private string SaveICDGroup(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSCCM dsCodes = new DSCCM();
                DSCCM.ICDGroupsRow dr = dsCodes.ICDGroups.NewICDGroupsRow();

                dr.ShortName = SearchedfieldsJSON["txtICDGroupName"];
                dr.Description = SearchedfieldsJSON["txtICDGroupDescription"];
                dr.IsActive = SearchedfieldsJSON["chkActive"];// == true ? "1" : false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsCodes.ICDGroups.AddICDGroupsRow(dr);
                BLObject<DSCCM> obj = BLLAdminObj.InsertICDGroup(ref dsCodes);
                dsCodes = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        ICDGroupId = dsCodes.Tables[dsCodes.ICDGroups.TableName].Rows[0][dsCodes.ICDGroups.ICDGroupIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
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

        /// <summary>
        /// UpdateICDGroup
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <returns></returns>
        private string UpdateICDGroup(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSCCM dsCodes = new DSCCM();
                DSCCM.ICDGroupsRow dr = dsCodes.ICDGroups.NewICDGroupsRow();

                dr.ICDGroupId = MDVUtility.ToLong(SearchedfieldsJSON["hftxtICDGroupId"]);
                dr.ShortName = SearchedfieldsJSON["txtICDGroupName"];
                dr.Description = SearchedfieldsJSON["txtICDGroupDescription"];
                dr.IsActive = SearchedfieldsJSON["chkActive"];// == true ? "1" : false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsCodes.ICDGroups.AddICDGroupsRow(dr);
                BLObject<DSCCM> obj = BLLAdminObj.UpdateICDGroup(ref dsCodes);
                dsCodes = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Update_Message,
                        ICDGroupId = dsCodes.Tables[dsCodes.ICDGroups.TableName].Rows[0][dsCodes.ICDGroups.ICDGroupIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
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

        /// <summary>
        /// DeleteICDGroup
        /// </summary>
        /// <param name="ICDGroupId"></param>
        /// <returns></returns>
        private string DeleteICDGroup(long ICDGroupId)
        {
            try
            {
                BLObject<string> obj = BLLAdminObj.DeleteICDGroup(MDVUtility.ToStr(ICDGroupId));
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

        /// <summary>
        /// ActiveInActiveICDGroup
        /// </summary>
        /// <param name="ICDGroupId"></param>
        /// <param name="isactive"></param>
        /// <returns></returns>
        private string ActiveInActiveICDGroup(string ICDGroupId, long isactive)
        {
            try
            {
                BLObject<string> obj = BLLAdminObj.ActiveInActiveICDGroup(ICDGroupId, isactive);
                if (obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message
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

        #endregion

        #region ICD Group Details

        /// <summary>
        /// LoadCCMICDGroupsDetail
        /// </summary>
        /// <param name="ICDGroupId"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        private string LoadCCMICDGroupsDetail(long ICDGroupId, Int32 PageNumber, Int32 RowsPerPage)
        {
            DSCCM dsCCM = null;

            BLObject<DSCCM> objCCM = BLLAdminObj.LoadCCMICDGroupsDetail(ICDGroupId, PageNumber, RowsPerPage);
            if (objCCM.Data != null)
            {
                dsCCM = objCCM.Data;
                if (dsCCM.Tables[dsCCM.ICDGroupsDetailSelect.TableName].Rows.Count > 0)
                {
                    var rows = dsCCM.Tables[dsCCM.ICDGroupsDetailSelect.TableName];
                    var dataRows = MDVUtility.JSON_DataTable(rows);
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ICDGroupId = dsCCM.Tables[dsCCM.ICDGroupsDetailSelect.TableName].Rows[0][dsCCM.ICDGroupsDetailSelect.ICDGroupIdColumn.ColumnName],
                        ICDGroupMapId = dsCCM.Tables[dsCCM.ICDGroupsDetailSelect.TableName].Rows[0][dsCCM.ICDGroupsDetailSelect.ICDGroupMapIdColumn.ColumnName],
                        Code = dsCCM.Tables[dsCCM.ICDGroupsDetailSelect.TableName].Rows[0][dsCCM.ICDGroupsDetailSelect.CodeColumn.ColumnName],
                        ICDGroupShortName = dsCCM.Tables[dsCCM.ICDGroupsDetailSelect.TableName].Rows[0][dsCCM.ICDGroupsDetailSelect.ICDGroupShortNameColumn.ColumnName],
                        ICDGroupDescription = dsCCM.Tables[dsCCM.ICDGroupsDetailSelect.TableName].Rows[0][dsCCM.ICDGroupsDetailSelect.ICDGroupDescriptionColumn.ColumnName],
                        ICDGroupisActive = dsCCM.Tables[dsCCM.ICDGroupsDetailSelect.TableName].Rows[0][dsCCM.ICDGroupsDetailSelect.IsActiveColumn.ColumnName],
                        CCMICDGroupDetailFill_JSON = dataRows,
                        ICDGroupCount = dsCCM.Tables[dsCCM.ICDGroupsDetailSelect.TableName].Rows.Count

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            else
            {
                var response = new
                {
                    status = false,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// DeleteICD_ICDGroup
        /// </summary>
        /// <param name="ICDGroupMapId"></param>
        /// <returns></returns>
        private string DeleteICD_ICDGroup(long ICDGroupMapId)
        {
            try
            {
                BLObject<string> obj = BLLAdminObj.DeleteICDGroupMap(MDVUtility.ToStr(ICDGroupMapId));
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

        /// <summary>
        /// SaveICD_ICDGroup
        /// </summary>
        /// <param name="icd9"></param>
        /// <param name="icd9Description"></param>
        /// <param name="icd10"></param>
        /// <param name="icd10Description"></param>
        /// <param name="snomed"></param>
        /// <param name="snomedDescription"></param>
        /// <param name="ICDGroupId"></param>
        /// <returns></returns>
        private string SaveICD_ICDGroup(string icd9, string icd9Description, string icd10, string icd10Description, string snomed, string snomedDescription, long ICDGroupId)
        {
            try
            {
                DSCodeLookup dsCodes = new DSCodeLookup();
                DSCodeLookup.ICDLookupRow dr = dsCodes.ICDLookup.NewICDLookupRow();

                dr.ICD9 = icd9;
                dr.ICD9_Description = icd9Description;
                dr.ICD10 = icd10;
                dr.ICD10_Description = icd10Description;
                dr.SNOMEDId = snomed;
                dr.SNOMED_Description = snomedDescription;

                #region Database Insertion
                dsCodes.ICDLookup.AddICDLookupRow(dr);
                BLObject<DSCodeLookup> obj = BLLAdminCodesObj.InsertICDLookup(ref dsCodes);
                dsCodes = obj.Data;
                if (obj.Data != null)
                {
                    var ICDId = MDVUtility.ToLong(dsCodes.Tables[dsCodes.ICDLookup.TableName].Rows[0][dsCodes.ICDLookup.IdColumn.ColumnName].ToString());
                    var response_ = InsertICDGroupMap(ICDId, ICDGroupId);
                    var message = "";
                    bool isDuplicate = response_.Contains("ICD already exists");

                    if (isDuplicate)
                        message = "ICD already exists";
                    else
                        message = Common.AppPrivileges.Save_Message;

                    var response = new
                    {
                        status = true,
                        message = message,
                        ICDId = dsCodes.Tables[dsCodes.ICDLookup.TableName].Rows[0][dsCodes.ICDLookup.IdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// InsertICDGroupMap
        /// </summary>
        /// <param name="icdCodeId"></param>
        /// <param name="icdGroupId"></param>
        /// <returns></returns>
        private string InsertICDGroupMap(long icdCodeId, long icdGroupId)
        {
            try
            {
                DSCCM dsICDGroupMap = new DSCCM();
                DSCCM.ICDGroupMapRow dr = dsICDGroupMap.ICDGroupMap.NewICDGroupMapRow();

                dr.ICDCodeId = icdCodeId;
                dr.ICDGroupId = icdGroupId;

                #region Database Insertion

                dsICDGroupMap.ICDGroupMap.AddICDGroupMapRow(dr);

                BLObject<DSCCM> obj = BLLAdminObj.InsertICDGroupMap(ref dsICDGroupMap);
                dsICDGroupMap = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        ICDGroupMapId = dsICDGroupMap.Tables[dsICDGroupMap.ICDGroupMap.TableName].Rows[0][dsICDGroupMap.ICDGroupMap.ICDGroupMapIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #endregion

        #region Service Command Handler
        /// <summary>
        /// CommandHandler
        /// </summary>
        /// <param name="context"></param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "LOAD_CCMICDGROUPS":
                    {
                        string fieldsJSON = context.Request["CCMICDGroupsData"];
                        long ICDGroupID = MDVUtility.ToInt(context.Request["ICDGroupID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadCCMICDGroups(fieldsJSON, ICDGroupID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "LOAD_CCMICDGROUPS_DETAIL":
                    {
                        long ICDGroupID = MDVUtility.ToInt(context.Request["CCMICDGroupID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadCCMICDGroupsDetail(ICDGroupID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_CCMICDGROUPS":
                    {
                        string fieldsJSON = context.Request["ICDData"];
                        string strJSONData = SaveICDGroup(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_CCMICDGROUPS":
                    {
                        string fieldsJSON = context.Request["ICDData"];
                        string strJSONData = UpdateICDGroup(fieldsJSON );

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "ACTIVEINACTIVE_CCMICDGROUPS":
                    {
                        string ICDGroupId = MDVUtility.ToStr(context.Request["ICDGroupId"]);
                        long isactive = MDVUtility.ToLong(context.Request["isactive"]);
                        string strJSONData = ActiveInActiveICDGroup(ICDGroupId, isactive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_CCMICDGROUPS":
                    {
                        long ICDGroupId = MDVUtility.ToLong(context.Request["ICDGroupId"]);
                        string strJSONData = DeleteICDGroup(ICDGroupId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_CCMICD_ICDGROUPS":
                    {
                        long ICDGroupMapId = MDVUtility.ToLong(context.Request["ICDGroupMapId"]);
                        string strJSONData = DeleteICD_ICDGroup(ICDGroupMapId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_CCMICD_ICDGROUPS":
                    {
                        string icd9code = MDVUtility.ToStr(context.Request["icd9code"]);
                        string icd9description = MDVUtility.ToStr(context.Request["icd9description"]);
                        string icd10code = MDVUtility.ToStr(context.Request["icd10code"]);
                        string icd10description = MDVUtility.ToStr(context.Request["icd10description"]);
                        string snomedcode = MDVUtility.ToStr(context.Request["snomedcode"]);
                        string snomeddescription = MDVUtility.ToStr(context.Request["snomeddescription"]);
                        long ICDGroupId = MDVUtility.ToLong(context.Request["ICDGroupId"]);

                        string strJSONData = SaveICD_ICDGroup(icd9code, icd9description, icd10code, icd10description, snomedcode, snomeddescription, ICDGroupId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion


    }
}