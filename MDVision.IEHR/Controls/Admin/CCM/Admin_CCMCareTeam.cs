using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin.CCM
{
    public class Admin_CCMCareTeam
    {


        private readonly BLLAdmin _bllAdminObj = null;

        public Admin_CCMCareTeam()
        {
            _bllAdminObj = new BLLAdmin();
        }

        #region Singleton
        private static Admin_CCMCareTeam _obj = null;
        public static Admin_CCMCareTeam Instance()
        {
            if (_obj == null)
                _obj = new Admin_CCMCareTeam();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// LoadCCMCareTeam
        /// </summary>
        /// <param name="careTeamData"></param>
        /// <param name="careTeamId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <returns></returns>
        private string LoadCCMCareTeam(string careTeamData, long careTeamId, Int32 pageNumber, Int32 rowsPerPage)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(careTeamData);
            string shortName = "";
            var providerName = "";
            if (!string.IsNullOrEmpty(Convert.ToString(data["ShortName"])))
            {
                shortName = Convert.ToString(data["ShortName"]);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(data["ProviderName"])))
            {
                providerName = Convert.ToString(data["ProviderName"]);
            }
            var isActive = Convert.ToBoolean(data["IsActive"]);
            BLObject<DSCCM> obj = _bllAdminObj.loadCareTeams(careTeamId, shortName, providerName, isActive, pageNumber, rowsPerPage);
            if (obj.Data != null)
            {
                var ds = obj.Data;
                if (ds.Tables[ds.CareTeams.TableName].Rows.Count > 0)
                {
                    var rows = ds.Tables[ds.CareTeams.TableName];
                    var dataRows = MDVUtility.JSON_DataTable(rows);
                    var response = new
                    {
                        status = true,
                        CCMCareTeamFill_JSON = dataRows
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        CCMCareTeamFill_JSON = "No Data Found."
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            else
            {
                var response = new
                {
                    status = false,
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// SaveCCMCareTeam
        /// </summary>
        /// <param name="careTeamData"></param>
        /// <returns></returns>
        private string SaveCCMCareTeam(string careTeamData)
        {
            try
            {


                JavaScriptSerializer ser =
                    new JavaScriptSerializer();

                var data = ser.Deserialize<dynamic>(careTeamData);
                var ds = new DSCCM();
                var dr = ds.CareTeams.NewCareTeamsRow();

                dr.ShortName = Convert.ToString(data["CareTeamName"]);
                dr.Description = Convert.ToString(data["Description"]);
                if (data["PCPId"] != "") dr.PCPId = Convert.ToInt64(data["PCPId"]);
                dr.ProviderId = Convert.ToInt64(data["ProviderId"]);
                dr.IsActive = Convert.ToBoolean(data["IsActive"]);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                ds.CareTeams.AddCareTeamsRow(dr);

                string careManagers = data["CareManagerId"];
                string careCoordinators = data["CareCoordinatorId"];
                string careGivers = data["CareGiverId"];

                var careManagesList = careManagers.Split(',').ToList();
                var careCoordinatorList = careCoordinators.Split(',').ToList();
                var careGiverList = careGivers.Split(',').ToList();

                foreach (var str in careManagesList)
                {
                    var drCareManager = ds.CareManagers.NewCareManagersRow();
                    drCareManager.CareManagerId = Convert.ToInt64(str);
                    ds.CareManagers.AddCareManagersRow(drCareManager);
                }

                foreach (var str in careCoordinatorList)
                {
                    var drCareCoordinator = ds.CareCoordinators.NewCareCoordinatorsRow();
                    drCareCoordinator.CareCoordinatorId = Convert.ToInt64(str);
                    ds.CareCoordinators.AddCareCoordinatorsRow(drCareCoordinator);
                }

                foreach (var str in careGiverList)
                {
                    var drCareGiver = ds.CareGivers.NewCareGiversRow();
                    drCareGiver.CareGiverId = Convert.ToInt64(str);
                    ds.CareGivers.AddCareGiversRow(drCareGiver);
                }

                BLObject<DSCCM> obj = _bllAdminObj.saveCareTeams(ref ds);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = "Settings have been saved!",
                        CareTeamId = ds.Tables[ds.CareTeams.TableName].Rows[0][ds.CareTeams.CareTeamIdColumn.ColumnName]
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                //MDVision.Common.Shared. MDCustomException 
                MDVLogger.BLLErrorLog("Admin_CCMCareTeam::SaveCCMCareTeam", ex);
                var response = new
                {
                    status = false,
                    Message = ex.Message
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        /// <summary>
        /// UpdateCCMCareTeam
        /// </summary>
        /// <param name="careTeamData"></param>
        /// <param name="careTeamId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        private string UpdateCCMCareTeam(string careTeamData, long careTeamId, bool isActive)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(careTeamData);

            DSCCM ds = new DSCCM();
            BLObject<DSCCM> obj = _bllAdminObj.loadCareTeams(careTeamId, "", "", isActive, 1, 10);
            ds = obj.Data;
            if (ds.Tables[ds.CareTeams.TableName].Rows.Count > 0)
            {
                foreach (DSCCM.CareTeamsRow dr in ds.Tables[ds.CareTeams.TableName].Rows)
                {
                    dr.ShortName = Convert.ToString(data["CareTeamName"]);
                    dr.Description = Convert.ToString(data["Description"]);
                    if (data["PCPId"] != "")
                        dr.PCPId = Convert.ToInt64(data["PCPId"]);
                  
                  //  dr.CareManagerId = "";
                 //   dr.CareCoordinatorId = 174;
                 //   dr.CareGiverId = 174;
                    dr.ProviderId = Convert.ToInt64(data["ProviderId"]);
                    dr.IsActive = Convert.ToBoolean(data["IsActive"]);
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    // ccm-335 Faizan Ameen .
                    DSCCM.CareManagersRow drCareManager;
                    DSCCM.CareCoordinatorsRow drCareCoordinator;
                    DSCCM.CareGiversRow drCareGiver;
                    string careManagers = data["CareManagerId"];
                    string careCoordinators = data["CareCoordinatorId"];
                    string careGivers = data["CareGiverId"];


                    List<string> careManagesList = careManagers.Split(',').ToList<string>();
                    List<string> careCoordinatorList = careCoordinators.Split(',').ToList<string>();
                    List<string> careGiverList = careGivers.Split(',').ToList<string>();

                    foreach (string str in careManagesList)
                    {
                        drCareManager = ds.CareManagers.NewCareManagersRow();
                        drCareManager.CareManagerId = Convert.ToInt64(str);
                        ds.CareManagers.AddCareManagersRow(drCareManager);

                    }

                    foreach (string str in careCoordinatorList)
                    {
                        drCareCoordinator = ds.CareCoordinators.NewCareCoordinatorsRow();
                        drCareCoordinator.CareCoordinatorId = Convert.ToInt64(str);
                        ds.CareCoordinators.AddCareCoordinatorsRow(drCareCoordinator);

                    }

                    foreach (string str in careGiverList)
                    {
                        drCareGiver = ds.CareGivers.NewCareGiversRow();
                        drCareGiver.CareGiverId = Convert.ToInt64(str);
                        ds.CareGivers.AddCareGiversRow(drCareGiver);

                    }
                    // ccm-335 Faizan Ameen End of Code.

                    string obj2 = _bllAdminObj.updateCareTeams(ref ds);
                    if (obj2 == "Success")
                    {
                        var rows = ds.Tables[ds.Templates.TableName];
                        var dataRows = MDVUtility.JSON_DataTable(rows);
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var response = new
                        {
                            status = true,

                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = obj2
                        };
                        return (JsonConvert.SerializeObject(response));
                    }

                }
            }
            else
            {
                var response = new
                {
                    status = false,
                };
                return (JsonConvert.SerializeObject(response));

            }
            return "";
        }

        /// <summary>
        /// DeleteCCMCareTeam
        /// </summary>
        /// <param name="careTeamId"></param>
        /// <returns></returns>
        private string DeleteCCMCareTeam(string careTeamId)
        {
            try
            {
                var obj = _bllAdminObj.deleteCareTeams(careTeamId);
                if (obj.Data != "Successfully Deleted")
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Data
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = obj.Data
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    status = false,
                    message = e.Message
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// ActiveInActiveICDGroup
        /// </summary>
        /// <param name="careTeamId"></param>
        /// <param name="isactive"></param>
        /// <returns></returns>
        private string ActiveInActiveCareTeam(string careTeamId, long isactive)
        {
            try
            {
                var obj = _bllAdminObj.ActiveInActiveCareTeam(careTeamId, isactive);
                if (obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Update_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Data
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
                case "LOAD_CCMCARETEAM":
                    {
                        var fieldsJson = context.Request["CCMCareTeamData"];
                        long careTeamId = MDVUtility.ToInt(context.Request["CareTeamID"]);
                        var pageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        var rowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        var strJsonData = LoadCCMCareTeam(fieldsJson, careTeamId, MDVUtility.ToInt32(pageNumber), MDVUtility.ToInt32(rowsPerPage));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "SAVE_CCMCARETEAM":
                    {
                        var fieldsJson = context.Request["CCMCareTeamData"];
                        var strJsonData = SaveCCMCareTeam(fieldsJson);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "DELETE_CCMCARETEAM":
                    {
                        var strJsonData = DeleteCCMCareTeam(context.Request["CareTeamID"]);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "UPDATE_CCMCARETEAM":
                    {
                        var fieldsJson = context.Request["CCMCareTeamData"];
                        var careTeamId = MDVUtility.ToInt(context.Request["CareTeamID"]);
                        var isActive = MDVUtility.ToBool(context.Request["IsActive"]);
                        var strJsonData = UpdateCCMCareTeam(fieldsJson, careTeamId, isActive);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "ACTIVEINACTIVE_CCMCARETEAM":
                    {
                        var careTeamId = MDVUtility.ToStr(context.Request["CareTeamID"]);
                        var isActive = MDVUtility.ToLong(context.Request["isactive"]);
                        var strJsonData = ActiveInActiveCareTeam(careTeamId, isActive);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }

        #endregion
    }
}