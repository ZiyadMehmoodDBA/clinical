using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using System.Data;
using MDVision.IEHR.Common;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_PasswordConfiguration
    {
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        public Admin_PasswordConfiguration()
        {
            BLLAdminSecurityObj = new BLLAdminSecurity();
        }

        #region Singleton
        private static Admin_PasswordConfiguration _obj = null;
        public static Admin_PasswordConfiguration Instance()
        {
            if (_obj == null)
                _obj = new Admin_PasswordConfiguration();
            return _obj;
        }
        #endregion

        #region Private Functions


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldsJSON"></param>
        /// <param name="PasswordConfigurationID"></param>
        /// <returns></returns>
        private string SaveUpdatePasswordConfiguration(string fieldsJSON)
        {
            try
            {
                int minPasswordLength = 8;
                int minSpecialCharacter = 0;
                int minAlphaCharacter = 0;
                int minNumeriCharacter = 0;
                int minUppercaseeCharacter = 0;

                JavaScriptSerializer ser = new JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSUsers dsUser = new DSUsers();
                DSUsers.PasswordConfigurationRow dr = dsUser.PasswordConfiguration.NewPasswordConfigurationRow();

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["lstEntityId"]))
                {
                    dr.PasswordConfigurationId = MDVUtility.ToInt64(SearchedfieldsJSON["lstEntityId"]);
                }
                else
                {
                    throw new Exception("please select an Entity First");
                }

                minPasswordLength = MDVUtility.ToInt32(SearchedfieldsJSON["txtMinPasswordLength"]);
                minSpecialCharacter = MDVUtility.ToInt32(SearchedfieldsJSON["txtMinAlphaCharacter"]);
                minAlphaCharacter = MDVUtility.ToInt32(SearchedfieldsJSON["txtMinNumericCharacter"]);
                minNumeriCharacter = MDVUtility.ToInt32(SearchedfieldsJSON["txtMinUppercaseCharacter"]);
                minUppercaseeCharacter = MDVUtility.ToInt32(SearchedfieldsJSON["txtMinUppercaseCharacter"]);
              
                dr.MinPasswordLength = minPasswordLength;
                dr.MinSpecialCharacter = minSpecialCharacter;
                dr.MinAlphaCharacter = minAlphaCharacter;
                dr.MinNumericCharacter = minNumeriCharacter;
                dr.MinUppercaseCharacter = minUppercaseeCharacter;
                dr.MaxPasswordAge = MDVUtility.ToInt32(SearchedfieldsJSON["txtMaxPasswordAge"]);
                dr.PasswordHistory = MDVUtility.ToInt32(SearchedfieldsJSON["txtPasswordHistory"]);
                dr.AccountLockoutThreshold = MDVUtility.ToInt32(SearchedfieldsJSON["txtAccountLockoutThreshold"]);
                dr.IdleSessionTimeout = MDVUtility.ToInt32(SearchedfieldsJSON["txtIdleSessionTimeout"]);
                dr.PasswordRegex = MDVUtility.GenerateRegex(minPasswordLength, minSpecialCharacter, minAlphaCharacter, minNumeriCharacter, minUppercaseeCharacter);
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                #region Database Updation
                dsUser.PasswordConfiguration.AddPasswordConfigurationRow(dr);
                //  dsUser.Users.AcceptChanges();

                if (dsUser.PasswordConfiguration.Rows.Count > 0)
                {
                    //    dsUser.PasswordConfiguration.Rows[0].SetModified();
                    BLObject<DSUsers> obj = BLLAdminSecurityObj.SaveUpdatePasswordConfiguration(ref dsUser);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = ""
                    };
                    return JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PasswordConfigurationID"></param>
        /// <returns></returns>
        private string FillPasswordConfiguration(Int64 PasswordConfigurationID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PasswordConfigurationID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSUsers dsUsers = null;
                    BLObject<DSUsers> objPasswordConfiguration = BLLAdminSecurityObj.LoadPasswordConfiguration(PasswordConfigurationID);
                    if (objPasswordConfiguration.Data != null)
                    {
                        dsUsers = objPasswordConfiguration.Data;
                        if (dsUsers.PasswordConfiguration.Rows.Count > 0)
                        {
                            DSUsers.PasswordConfigurationRow dr = (DSUsers.PasswordConfigurationRow)dsUsers.PasswordConfiguration.Rows[0];

                            var keyValues = new Dictionary<string, string>
                        {
                           // { "lstEntityId", MDVUtility.ToStr(dr[dsUsers.PasswordConfiguration.PasswordConfigurationIdColumn.ColumnName])},
                            { "txtMinPasswordLength", MDVUtility.ToStr(dr.MinPasswordLength)},
                            { "txtMinSpecialCharacter", MDVUtility.ToStr(dr.MinSpecialCharacter)},
                            { "txtMinAlphaCharacter", MDVUtility.ToStr(dr.MinAlphaCharacter)},
                            { "txtMinNumericCharacter", MDVUtility.ToStr(dr.MinNumericCharacter)},
                            { "txtMinUppercaseCharacter", MDVUtility.ToStr(dr.MinUppercaseCharacter)},
                            { "txtMaxPasswordAge", MDVUtility.ToStr(dr.MaxPasswordAge)},
                            { "txtPasswordHistory", MDVUtility.ToStr(dr.PasswordHistory)},
                            { "txtAccountLockoutThreshold", MDVUtility.ToStr(dr.AccountLockoutThreshold)},
                            { "txtIdleSessionTimeout", MDVUtility.ToStr(dr.IdleSessionTimeout)},
                            
                        };
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PasswordConfigurationFill_JSON = js.Serialize(keyValues)
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = AppPrivileges.No_Record_Message,
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objPasswordConfiguration.Message
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }


        #endregion

        #region Service Command Handler

        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "FILL_PASSWORD_CONFIGURATION":
                    {
                        string strPasswordConfigurationId = context.Request["PasswordConfigurationID"];
                        string strJSONData = FillPasswordConfiguration(MDVUtility.ToInt64(strPasswordConfigurationId));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_PASSWORD_CONFIGURATION":
                    {
                        string fieldsJSON = context.Request["PasswordConfigurationData"];
                        string strJSONData = SaveUpdatePasswordConfiguration(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}