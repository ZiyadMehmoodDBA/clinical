using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.Model.Batch.HL7ImmunizationBatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Batch
{
    public class Batch_ImportHL7ImmunizationBatch
    {
        private static Batch_ImportHL7ImmunizationBatch _obj = null;
        private BLLBatch BLLBatchObj = null;
        public Batch_ImportHL7ImmunizationBatch()
        {
            BLLBatchObj = new BLLBatch();
        }
        #region Singleton
        public static Batch_ImportHL7ImmunizationBatch Instance()
        {
            if (_obj == null)
                _obj = new Batch_ImportHL7ImmunizationBatch();
            return _obj;
        }
        #endregion

        #region command Handler
        public void CommandHandler(HttpContext context)
        {

            string commandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (commandAction)
            {
                case "LOAD_HL7BATCH":
                    {
                        string data = context.Request["HL7BatchSearchData"];
                        long PageNumber = MDVUtility.ToInt64(context.Request["PageNumber"]);
                        long RowsPerPage = MDVUtility.ToInt64(context.Request["RowsPerPage"]);
                        string HL7BatchData = SearchHL7Batch(data, PageNumber, RowsPerPage);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(HL7BatchData);
                    }
                    break;
                case "LOAD_HL7QUEUE":
                    {
                        string data = context.Request["HL7QueueSearchData"];
                        long PageNumber = MDVUtility.ToInt64(context.Request["PageNumber"]);
                        long RowsPerPage = MDVUtility.ToInt64(context.Request["RowsPerPage"]);
                        string HL7BatchData = SearchHL7Queue(data, PageNumber, RowsPerPage);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(HL7BatchData);
                    }
                    break;
                case "DELETE_HL7BATCH_IMMUNIZATION":
                    {
                        string BatchIds = context.Request["HL7BatchID"];
                        string response = DeleteBatch(BatchIds);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(response);
                    }
                    break;
                case "MARK_COMPLETED_HL7BATCH_IMMUNIZATION":
                    {
                        string BatchIds = context.Request["HL7BatchID"];
                        string response = MarkBatchAsCompleted(BatchIds);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(response);
                    }
                    break;
                case "REPROCESS_HL7BATCH_IMMUNIZATION":
                    {
                        string BatchIds = context.Request["HL7BatchID"];
                        string response = ReProcessBatch(BatchIds);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(response);
                    }
                    break;
                case "LOAD_HL7BATCH_BY_ID":
                    {
                        string BatchIds = context.Request["HL7BatchID"];
                        string response = SearchHL7BatchById(BatchIds);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(response);
                    }
                    break;
                default:
                    break;
            }
        }

        private string MarkBatchAsCompleted(string BatchIds)
        {
            try
            {
                if (string.IsNullOrEmpty(BatchIds))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBatchObj.MarkBatchAsCompleted(BatchIds);
                    if (obj.Data != null && obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = "Successfully Mark as Completed"
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
        private string ReProcessBatch(string BatchIds)
        {
            try
            {
                if (string.IsNullOrEmpty(BatchIds))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBatchObj.ReProcessBatch(BatchIds);
                    if (obj.Data != null && obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = "Successfully Re-Process"
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
        private string DeleteBatch(string BatchIds)
        {
            try
            {
                if (string.IsNullOrEmpty(BatchIds))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBatchObj.DeleteHL7ImmunizationBatch(BatchIds);
                    if (obj.Data != null && obj.Data == "")
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
        private string SearchHL7Queue(string HL7QueueData, long PageNumber = 1, long RowsPerPage = 15)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var data = ser.Deserialize<dynamic>(HL7QueueData);

                HL7ImmunizationBatchModel model = new HL7ImmunizationBatchModel();
                model.PatientId = MDVUtility.ToStr(data["hfPatientId"]);
                model.PatientLastName = MDVUtility.ToStr(data["txtPatientLastName"]);
                model.PatientFirstName = MDVUtility.ToStr(data["txtPatientFirstName"]);
                model.DOSFrom = MDVUtility.ToStr(data["dtpDateFrom"]);
                model.DOSTo = MDVUtility.ToStr(data["dtpDateTo"]);
                model.StatusId = MDVUtility.ToStr(data["ddlStatus"]);
                model.Type = MDVUtility.ToStr(data["ddlType"]);
                model.FacilityId = MDVUtility.ToStr(data["hfFacilityId"]);
                model.GivenByProviderid = MDVUtility.ToStr(data["hfGivenByProviderId"]);
                model.PageNumber = PageNumber;
                model.RowsPerPage = RowsPerPage;


                BLObject<List<HL7ImmunizationBatchModel>> obj = BLLBatchObj.LoadHL7ImmunizationQueue(model);

                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listSearchHL7Queue = obj.Data,
                        HL7QueueCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        listSearchHL7Queue = "[]",
                        HL7QueueCount = 0,
                        iTotalDisplayRecords = 0
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
        private string SearchHL7Batch(string HL7BatchData, long PageNumber = 1, long RowsPerPage = 15)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(HL7BatchData);

            HL7ImmunizationBatchModel model = new HL7ImmunizationBatchModel();
            model.Providerid = MDVUtility.ToStr(data["hfProviderId"]);
            model.DOSFrom = MDVUtility.ToStr(data["dtpDateFrom"]);
            model.DOSTo = MDVUtility.ToStr(data["dtpDateTo"]);
            model.StatusId = MDVUtility.ToStr(data["ddlStatus"]);
            model.PageNumber = PageNumber;
            model.RowsPerPage = RowsPerPage;

            try
            {
                BLObject<List<HL7ImmunizationBatchModel>> obj = BLLBatchObj.LoadHL7ImmunizationBatch(model);

                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listSearchHL7Batch = obj.Data,
                        HL7BatchCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        listSearchHL7Batch = "[]",
                        HL7BatchCount = 0,
                        iTotalDisplayRecords = 0
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
        private string SearchHL7BatchById(string HL7BatchId)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

            HL7ImmunizationBatchModel model = new HL7ImmunizationBatchModel();
            model.HL7BatchId = HL7BatchId;

            try
            {
                BLObject<List<HL7ImmunizationBatchModel>> obj = BLLBatchObj.LoadHL7ImmunizationBatchById(model);

                if (obj.Data != null && obj.Data.Count != 0)
                {
                    var response = new
                    {
                        status = true,
                        listSearchHL7Batch = obj.Data,
                        HL7BatchCount = obj.Data.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        listSearchHL7Batch = "[]",
                        HL7BatchCount = 0,
                        iTotalDisplayRecords = 0
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
    }
}