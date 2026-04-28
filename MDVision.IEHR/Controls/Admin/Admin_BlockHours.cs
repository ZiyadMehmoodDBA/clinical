using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_BlockHours
    {
        private BLLSchedule BLLScheduleObj = null;
        public Admin_BlockHours()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Admin_BlockHours _obj = null;
        public static Admin_BlockHours Instance()
        {
            if (_obj == null)
                _obj = new Admin_BlockHours();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Loads the block hours.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="BlockHoursID">The block hours identifier.</param>
        /// <returns>System.String.</returns>
        private string LoadBlockHours(string fieldsJSON, Int64 BlockHoursID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Block Hours", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLScheduleObj.LoadBlockHours(BlockHoursID, null, null, null, null, null, null, null);
                    else
                        obj = BLLScheduleObj.LoadBlockHours(BlockHoursID, SearchedfieldsJSON["hfProvider"], SearchedfieldsJSON["hfResource"], SearchedfieldsJSON["hfFacility"], SearchedfieldsJSON["blockHoursFromDate"], SearchedfieldsJSON["blockHoursToDate"], SearchedfieldsJSON["blockHoursFromTime"], SearchedfieldsJSON["blockHoursToTime"], PageNumber, RowsPerPage);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            BlockHoursCount = dsSchedule.Tables[dsSchedule.SchBlockHours.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsSchedule.SchBlockHours.Rows.Count > 0) ? dsSchedule.SchBlockHours.Rows[0][dsSchedule.SchBlockHours.RecordCountColumn.ColumnName] : 0,
                            BlockHoursLoad_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.SchBlockHours.TableName]),
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            BlockHoursCount = 0,
                            Message = obj.Message
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
        #endregion

        #region Service Command Handler

        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_BLOCK_HOURS":
                    {
                        string fieldsJSON = context.Request["BlockHoursData"];
                        Int64 BlockHoursID = MDVUtility.ToInt64(context.Request["BlockHoursID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadBlockHours(fieldsJSON, BlockHoursID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}