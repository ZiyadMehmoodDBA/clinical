using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using MDVision.IEHR.Common;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Clinical
{
    public class ClinicalMenuSettings
    {
         private BLLClinical BLLClinicalObj = null;
         public ClinicalMenuSettings()
        {
            BLLClinicalObj = new BLLClinical();
        }
        #region Singleton
        private static ClinicalMenuSettings _obj = null;
        public static ClinicalMenuSettings Instance()
        {
            if (_obj == null)
                _obj = new ClinicalMenuSettings();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// SearchClinicalMenuSettings
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string SearchClinicalMenuSettings(Int64 userId)
        {

            //DSModuleForm dsClinicalMenuSettings_ = null;
            //    BLObject<DSModuleForm> obj_;

            //    obj_ = BLLClinicalObj.LoadForms("clinicalMenuMedical");

            try
            {
                DSClinical dsClinicalMenuSettings = null;
                BLObject<DSClinical> obj;

                obj = BLLClinicalObj.LoadClinicalMenuSettings(userId);

                dsClinicalMenuSettings = obj.Data;
                if (obj.Data != null)
                {
                    if (dsClinicalMenuSettings.Tables[dsClinicalMenuSettings.ClinicalMenuSettings.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            html = dsClinicalMenuSettings.Tables[dsClinicalMenuSettings.ClinicalMenuSettings.TableName].Rows[0][dsClinicalMenuSettings.ClinicalMenuSettings.ClinicalMenuHTMLColumn.ColumnName],
                            status = true,
                            MenuSettingsCount = dsClinicalMenuSettings.Tables[dsClinicalMenuSettings.ClinicalMenuSettings.TableName].Rows.Count,
                            ClinicalMenuSettingsLoad_JSON = MDVUtility.JSON_DataTable(dsClinicalMenuSettings.Tables[dsClinicalMenuSettings.ClinicalMenuSettings.TableName])//Utility.JSON_DataTable(dsClinicalMenuSettings.Tables[dsClinicalMenuSettings.ClinicalMenuSettings.TableName],"",false)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            MenuSettingsCount = 0
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
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

        private string UpdateClincialMenuSettings(Int64 userId, string htmlClinicalMenuSettings)
        {
            DSClinical dsClinicalMenuSettings = null;
            try
            {

                BLObject<DSClinical> obj = BLLClinicalObj.LoadClinicalMenuSettings(userId);
                dsClinicalMenuSettings = obj.Data;
                if (dsClinicalMenuSettings.Tables[dsClinicalMenuSettings.ClinicalMenuSettings.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsClinicalMenuSettings.Tables[dsClinicalMenuSettings.ClinicalMenuSettings.TableName].Rows[0];

                    dr[dsClinicalMenuSettings.ClinicalMenuSettings.ClinicalMenuHTMLColumn.ColumnName] = htmlClinicalMenuSettings;
                    dr[dsClinicalMenuSettings.ClinicalMenuSettings.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr[dsClinicalMenuSettings.ClinicalMenuSettings.ModifiedOnColumn.ColumnName] = DateTime.Now;

                    BLObject<DSClinical> objUser = BLLClinicalObj.UpdateClinicalMenuSettings(dsClinicalMenuSettings);
                    if (objUser.Data != null)
                    {
                        var response = new
                        {
                            status = true
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objUser.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    DSClinical dsClinicalMenuSettings_ = new DSClinical();
                    DSClinical.ClinicalMenuSettingsRow dr_ = dsClinicalMenuSettings_.ClinicalMenuSettings.NewClinicalMenuSettingsRow();

                    dr_.ClinicalMenuSettingsId = -1;
                    dr_.UserId = userId;
                    dr_.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr_.CreatedOn = DateTime.Now;
                    dr_.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr_.ModifiedOn = DateTime.Now;
                    dr_.ClinicalMenuHTML = htmlClinicalMenuSettings;

                    dsClinicalMenuSettings_.ClinicalMenuSettings.AddClinicalMenuSettingsRow(dr_);
                    BLObject<DSClinical> obj_ = BLLClinicalObj.InsertClinicalMenuSettings(dsClinicalMenuSettings_);
                    //dsClinicalMenuSettings_ = obj_.Data;

                    if (obj_.Data != null)
                    {
                        var response = new
                        {
                            status = true
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj_.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
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



        private string GetClinicalSubMenu(string FormParentHTMLId)
        {

          

            try
            {
                DSModuleForm dsClinicalMenuSettings = null;
                BLObject<DSModuleForm> obj = BLLClinicalObj.LoadForms(FormParentHTMLId);

                dsClinicalMenuSettings = obj.Data;
                if (obj.Data != null)
                {
                    if (dsClinicalMenuSettings.Tables[dsClinicalMenuSettings.Forms.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MenuSettingsCount = dsClinicalMenuSettings.Tables[dsClinicalMenuSettings.Forms.TableName].Rows.Count,
                            ClinicalMenuSettingsLoad_JSON = MDVUtility.JSON_DataTable(dsClinicalMenuSettings.Tables[dsClinicalMenuSettings.Forms.TableName])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            MenuSettingsCount = 0
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
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
                case "SEARCH_CLINICAL_MENU_SETTINGS":
                    {
                        var userId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                        string strJsonData = SearchClinicalMenuSettings(userId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "UPDATE_CLINICAL_MENU_SETTINGS":
                    {
                        string clinicalMenuHtml = context.Request["clinicalMenuHTML"];
                        var userId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                        string strJsonData = UpdateClincialMenuSettings(userId, clinicalMenuHtml);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "GET_CLINICAL_SUB_MENU":
                    {
                        string FormParentHTMLId = context.Request["FormParentHTMLId"];
                        string strJsonData = GetClinicalSubMenu(FormParentHTMLId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                    
            }
        }
        #endregion
    }
}