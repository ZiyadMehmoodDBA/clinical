using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_Appointment_History
    {
        private BLLSchedule BLLScheduleObj = null;
        public Scheduling_Appointment_History()
        {
            
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Scheduling_Appointment_History _obj = null;

        public static Scheduling_Appointment_History Instance()
        {
            if (_obj == null)
            {
                _obj = new Scheduling_Appointment_History();

            }
            return _obj;
        }
        #endregion
        public string LoadAppointmentHistory(long AppointmentId,string CreatedOn)
        {
            try
            {
                DSDBAudit dsDbAudit = null;
                BLObject<DSDBAudit> obj;
                DateTime? createdDate = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                if (CreatedOn != "")
                    createdDate = MDVUtility.ToDateTime(CreatedOn);
                obj = BLLScheduleObj.LoadAppointmentHistory(AppointmentId, createdDate);
                dsDbAudit = obj.Data;
                if (obj.Data != null)
                {
                    if (dsDbAudit.Tables[dsDbAudit.DBAuditAppointment.TableName].Rows.Count > 0)
                    {
                        DataView view = new DataView(dsDbAudit.DBAuditAppointment);
                        //  item.DBAuditAppointmentId + '</td><td>' + item.ProfileName + '</td><td class="ellipses size-max90" title="' + item.UserName + '">' + item.UserName + '</td><td>' + item.CreatedDate + '</td>');

                        view.Sort = "CreatedDate ASC";
                        DataTable UserEntry = view.ToTable(true,  "DBAuditAction", "ProfileName", "UserName", "CreatedDate");
                      
                        DataTable ChangedFields = view.ToTable(true, "DBAuditAppointmentId", "CreatedDate", "DisplayName", "CurrentValue", "OriginalValue", "DBAuditAction");
                      //  view.RowFilter = "DBAuditAction = 'Insert'";
                      
                        DataTable NewEntry = view.ToTable(true, "DBAuditAppointmentId", "ProfileName", "UserName", "CreatedDate", "DBAuditAction");
                        


                        var response = new
                        {
                            status = true,
                            AppointmentLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(NewEntry.AsEnumerable().Take(1).CopyToDataTable())),
                            UserEntry = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(UserEntry)),
                            ChangedFields = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(ChangedFields)),
                            RecordCount = dsDbAudit.Tables[dsDbAudit.DBAuditAppointment.TableName].Rows.Count,
                          //  AppointmentLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsDbAudit.Tables[dsDbAudit.DBAuditAppointment.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            RecordCount = 0,
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
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                
                case "SELECT_APPOINTMENT_HISTORY":
                    {
                        string AppointmentId = context.Request["AppointmentId"];
                        string CreatedOn = context.Request["CreatedOn"];
                       
                        string strJSONData = LoadAppointmentHistory(MDVUtility.ToInt64(AppointmentId),CreatedOn);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                    
            }
        }
    }
}