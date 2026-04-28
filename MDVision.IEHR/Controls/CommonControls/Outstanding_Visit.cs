using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.CommonControls
{
    public class Outstanding_Visit
    {
        private BLLMessage BLLMessageObj = null;
        private BLLVisits BLLVisitsObj = null;
        public Outstanding_Visit()
        {
            BLLMessageObj = new BLLMessage();
            BLLVisitsObj = new BLLVisits();
        }

        #region Singleton
        private static Outstanding_Visit _obj = null;


        public static Outstanding_Visit Instance()
        {
            if (_obj == null)
            {
                _obj = new Outstanding_Visit();
            }
            return _obj;
        }
        #endregion



        #region Private Functions

        private string SearchUserTask(Int64 MessageID, Int64 AssignedToId, Int32 PageNumber, Int32 RowsPerPage, string MsgType = "Task", int MsgStatusId = 2)
        {
            try
            {

                DSMessage dsMessage = null;
                // MsgStatusId = 1 for Resolved Status
                // MsgStatusId = 2 for Unresolved Status
                BLObject<DSMessage> obj = BLLMessageObj.LoadPatientMessage(0, MessageID, "", MsgStatusId, null, null, AssignedToId, MsgType, PageNumber, RowsPerPage);

                //if (fieldsJSON == null)
                //    obj = BusinessWrapper.Message.BusinessObj.LoadPatientMessage(PatientID, MessageID, MsgTypeId, MsgStatusId, null, null, 0);
                //else
                //{
                //    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                //    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                //    DateTime? callDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpCalledDate"]) != "" ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCalledDate"]) : null;
                //    DateTime? entryDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpEntryDate"]) != "" ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpEntryDate"]) : null;
                //    obj = BusinessWrapper.Message.BusinessObj.LoadPatientMessage(PatientID, MessageID, SearchedfieldsJSON["ddlType"], MDVUtility.ToInt32(SearchedfieldsJSON["ddlStatus"]), callDate, entryDate, AssignedToId);
                //}

                dsMessage = obj.Data;
                if (obj.Data != null)
                {
                    if (dsMessage.Tables[dsMessage.PatMessages.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            iTotalDisplayRecords = dsMessage.PatMessages.Rows[0][dsMessage.PatMessages.RecordCountColumn.ColumnName],
                            UserTaskCount = dsMessage.Tables[dsMessage.PatMessages.TableName].Rows.Count,
                            UserTaskLoad_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.PatMessages.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            UserTaskCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string SearchVisits(Int64 PatientID, string PatientOutstanding)
        {
            try
            {
                DSVisits dsVisit = null;
                BLObject<DSVisits> obj = null;
                obj = BLLVisitsObj.LoadPatientOutstandingVisits(PatientID, PatientOutstanding);
                if (obj != null)
                {
                    dsVisit = obj.Data;
                }

                if (dsVisit != null)
                {
                    if (dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows.Count,
                            iTotalDisplayRecords = dsVisit.PatientVisits.Rows[0][dsVisit.PatientVisits.RecordCountColumn.ColumnName],
                            //OpenVisitsCount = dsVisit.PatientVisits.Rows[0][dsVisit.PatientVisits.OpenVisitsColumn.ColumnName],
                            //CloseVisitsCount = dsVisitClose.PatientVisits.Rows[0][dsVisitClose.PatientVisits.CloseVisitsColumn.ColumnName],
                            VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisit.Tables[dsVisit.PatientVisits.TableName]),
                            //CloseVisitsLoad_JSON = MDVUtility.JSON_DataTable(dsVisitClose.Tables[dsVisitClose.PatientVisits.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        VisitsCount = 0,
                        Message = "Record not found."
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
        /// Handle the Patient Message Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_OUTSTANDING_VISITS":
                    {
                        string fieldsJSON = context.Request["PatientMessageData"];
                        string page = context.Request["page"];
                        Int64 rpp = 15;//Utility.ToInt64(context.Request["rp"]);
                        Int64 pageNo = 1; //Utility.ToInt64(context.Request["rp"]);
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string PatientOutstanding = context.Request["PatientOutstanding"];
                        string strJSONData = SearchVisits(PatientID, PatientOutstanding);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}