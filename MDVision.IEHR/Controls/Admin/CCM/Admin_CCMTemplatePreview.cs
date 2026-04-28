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
    public class Admin_CCMTemplatePreview
    {

        private BLLAdmin BLLAdmineObj = null;
        private BLLAdminHL7 BLLAdminHL7Obj = null;
        public Admin_CCMTemplatePreview()
        {
            BLLAdmineObj = new BLLAdmin();
        }

        #region Singleton
        private static Admin_CCMTemplatePreview _obj = null;
        public static Admin_CCMTemplatePreview Instance()
        {
            if (_obj == null)
                _obj = new Admin_CCMTemplatePreview();
            return _obj;
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
            }
        }

        private string LoadCCMTemplate(string TemplateData, int TemplateId, string TempLookupName , Int32 PageNumber, Int32 RowsPerPage )
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(TemplateData);
            DSCCM ds = null;
            var ShortName = "";
            var Description = "";
            if (!string.IsNullOrEmpty(Convert.ToString(data["ShortName"]))) {
                ShortName = Convert.ToString(data["ShortName"]);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(data["Description"]))) {
                Description = Convert.ToString(data["Description"]);
            }

            var IsActive = Convert.ToBoolean(data["IsActive"]);

            BLObject<DSCCM> obj = BLLAdmineObj.loadTemplates(TemplateId, TempLookupName, ShortName, Description, IsActive, PageNumber, RowsPerPage);
            if (obj.Data != null)
            {
                ds = obj.Data;
                if (ds.Tables[ds.Templates.TableName].Rows.Count > 0)
                {
                    var rows = ds.Tables[ds.Templates.TableName];
                    var dataRows = MDVUtility.JSON_DataTable(rows);
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        CCMTemplateFill_JSON = dataRows
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

            #endregion
        }
    }