using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace MDVision.IEHR.Controls.Admin.CCM
{
    public class Admin_CCMTemplates
    {

        private BLLAdmin BLLAdmineObj = null;
        private BLLAdminHL7 BLLAdminHL7Obj = null;
        public Admin_CCMTemplates()
        {
            BLLAdmineObj = new BLLAdmin();
        }

        #region Singleton
        private static Admin_CCMTemplates _obj = null;
        public static Admin_CCMTemplates Instance()
        {
            if (_obj == null)
                _obj = new Admin_CCMTemplates();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// LoadCCMTemplate
        /// </summary>
        /// <param name="TemplateData"></param>
        /// <param name="TemplateId"></param>
        /// <param name="TempLookupName"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        private string LoadCCMTemplate(string TemplateData, int TemplateId, string TempLookupName, Int32 PageNumber, Int32 RowsPerPage)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(TemplateData);
            DSCCM ds = null;
            var ShortName = "";
            var Description = "";
            if (!string.IsNullOrEmpty(Convert.ToString(data["ShortName"])))
            {
                ShortName = Convert.ToString(data["ShortName"]);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(data["Description"])))
            {
                Description = Convert.ToString(data["Description"]);
            }

            var IsActive = Convert.ToBoolean(data["IsActive"]);

            BLObject<DSCCM> obj = BLLAdmineObj.loadTemplates(TemplateId, TempLookupName, ShortName, Description, IsActive, PageNumber, RowsPerPage);
            if (obj.Data != null)
            {
                ds = obj.Data;
                if (ds.Tables[ds.Templates.TableName].Rows.Count > 0)
                {
                  //  var rows = ds.Tables[ds.Templates.TableName];
                    //var dataRows = MDVUtility.JSON_DataTable(rows);
                  //  System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        CCMTemplateFill_JSON = ds.Tables[ds.Templates.TableName] //dataRows
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
        /// ActiveInActiveTemplate
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        private string ActiveInActiveTemplate(string templateId, long isActive)
        {
            try
            {
                BLObject<string> obj = BLLAdmineObj.ActiveInActiveTemplate(templateId, isActive);
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
                case "LOAD_CCMTEMPLATES":
                    {
                        string fieldsJSON = context.Request["CCMTemplateData"];
                        int tempID = MDVUtility.ToInt(context.Request["TemplateID"]);
                        string tempLookupName = MDVUtility.ToStr(context.Request["TemplateLookupName"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadCCMTemplate(fieldsJSON, tempID, tempLookupName, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "ACTIVEINACTIVE_CCMTEMPLATE":
                    {
                        string templateId = MDVUtility.ToStr(context.Request["TemplateId"]);
                        long isActive = MDVUtility.ToLong(context.Request["isactive"]);
                        string strJSONData = ActiveInActiveTemplate(templateId, isActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion
    }
}