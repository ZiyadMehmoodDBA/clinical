using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_SupperBill
    {
        private BLLBilling BLLBillingObj = null;
        public Admin_SupperBill()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton

        private static Admin_SupperBill _instance = null;
        public static Admin_SupperBill Instance()
        {
            if (_instance == null)
                _instance = new Admin_SupperBill();
            return _instance;
        }

        #endregion

        #region Data Members
        #endregion

        #region Private Functions

        private string LoadSupperBill(string fieldsJSON, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    BLObject<DSSupperBill> objBill;
                    DSSupperBill dsEntity = null;

                    string ShortName = SearchedfieldsJSON["txtShortName"];
                    string Description = SearchedfieldsJSON["txtDescription"];
                    string isActive = null;

                    long Practice = 0;

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPractice"]))
                        Practice = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPractice"]);
                    if (MDVUtility.ToStr(SearchedfieldsJSON["ddlActive"]).ToLower() == "active")
                        isActive = "1";
                    else if (MDVUtility.ToStr(SearchedfieldsJSON["ddlActive"]).ToLower() == "inactive")
                        isActive = "0";

                    objBill = BLLBillingObj.LoadSupperBill(0, 0, Practice, ShortName, Description, isActive, PageNumber, RowsPerPage);

                    if (objBill.Data != null)
                    {
                        dsEntity = objBill.Data;
                        var response = new
                        {
                            status = true,
                            BillCount = dsEntity.Tables[dsEntity.SuperBills.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsEntity.SuperBills.Rows.Count > 0) ? dsEntity.SuperBills.Rows[0][dsEntity.SuperBills.RecordCountColumn.ColumnName] : 0,
                            BillLoad_JSON = MDVUtility.JSON_DataTable(dsEntity.Tables[dsEntity.SuperBills.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objBill.Message,
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

        private string DeleteSupperBill(Int64 BillID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(BillID)))
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        BLObject<string> obj = BLLBillingObj.DeleteSupperBill(MDVUtility.ToLong(BillID));
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

        private string UpdateSupperBillIsActive(Int64 BillId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSSupperBill dsBill = null;
                    BLObject<DSSupperBill> obj = BLLBillingObj.LoadSupperBill(BillId, 0, 0);
                    dsBill = obj.Data;
                    if (dsBill.Tables[dsBill.SuperBills.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsBill.Tables[dsBill.SuperBills.TableName].Rows[0];
                        dr[dsBill.SuperBills.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSSupperBill> objBill = BLLBillingObj.UpdateSupperBill(ref dsBill);
                        string successMsg;
                        if (objBill.Data != null)
                        {
                            if (IsActive == 0)
                                successMsg = Common.AppPrivileges.Inactive_Message;
                            else
                                successMsg = Common.AppPrivileges.Active_Message;
                            var response = new
                            {
                                status = true,
                                message = successMsg
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objBill.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
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

        #region Public Functions

        #endregion

        #region Control Events
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the User Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "SEARCH_SUPPER_BILL":
                    {
                        string fieldsJSON = context.Request["SupperBillData"];
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadSupperBill(fieldsJSON, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SUPPER_BILL_ACTIVE_INACTIVE":
                    {
                        Int64 strBillId = MDVUtility.ToInt64(context.Request["BillId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateSupperBillIsActive(strBillId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_SUPPER_BILL":
                    {
                        string strBillId = context.Request["BillId"];
                        string strJSONData = DeleteSupperBill(MDVUtility.ToInt64(strBillId));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        

       

        #endregion
    }
}