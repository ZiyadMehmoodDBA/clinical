using System;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using Newtonsoft.Json;
using System.Data;
using EDIParser;
using MDVision.IEHR.Model.Billing.ERA;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Billing.ERA
{
    public class Bill_ERA
    {
        private BLLBillingClaim BLLBillingClaimObj = null;
        private BLLERA BLLERAObj = null;
        public Bill_ERA()
        {
            BLLBillingClaimObj = new BLLBillingClaim();
            BLLERAObj = new BLLERA();
        }
        #region Singleton
        private static Bill_ERA _obj = null;
        public static Bill_ERA Instance()
        {
            if (_obj == null)
                _obj = new Bill_ERA();
            return _obj;
        }
        #endregion

        #region Private Functions

        public string LoadERA(ERAModel model)
        {
            try
            {
                DSERA dsERA = new DSERA();
                BLObject<DSERA> obj = new BLObject<DSERA>();

                DateTime? dtfrom = String.IsNullOrEmpty(model.FromEntryDate) ? (DateTime?)null : DateTime.Parse(model.FromEntryDate);
                DateTime? dtto = String.IsNullOrEmpty(model.ToEntryDate) ? (DateTime?)null : DateTime.Parse(model.ToEntryDate);
                obj = BLLERAObj.ERASearch(MDVUtility.ToLong(model.ERAID), MDVUtility.ToLong(model.ClearingHouse), MDVUtility.ToStr(model.CheckNumber), MDVUtility.ToLong(model.FacilityID), MDVUtility.ToLong(model.PracticeID), MDVUtility.ToStr(model.PayerName), MDVUtility.ToInt32(model.PayeeName), MDVUtility.ToStr(model.Status), MDVUtility.ToStr(dtfrom), MDVUtility.ToStr(dtto), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "LoadERA",model.CheckAmount);//


                if (obj.Data != null)
                {
                    dsERA = obj.Data;

                    if (dsERA.Tables[dsERA.ERA.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ERACount = dsERA.Tables[dsERA.ERA.TableName].Rows.Count,
                            iTotalDisplayRecords = dsERA.ERA.Rows[0][dsERA.ERA.RecordCountColumn.ColumnName],
                            ERALoad_JSON = MDVUtility.JSON_DataTable(dsERA.Tables[dsERA.ERA.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ERACount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ERACount = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }


            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string DeleteERA(ERAModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ERAID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLERAObj.DeleteERA(MDVUtility.ToStr(model.ERAID));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
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
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string UpdateERAIsActive(ERAModel model)
        {
            try
            {
                DSERA dsERA = null;
                BLObject<DSERA> obj = BLLERAObj.LoadERA(MDVUtility.ToLong(model.ERAID), 0, "", 0, 0, "",0, "", "", "");
                dsERA = obj.Data;
                if (dsERA.Tables[dsERA.ERA.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsERA.Tables[dsERA.ERA.TableName].Rows[0];
                    dr[dsERA.ERA.IsActiveColumn.ColumnName] = MDVUtility.ToLong(model.IsActive);

                    BLObject<DSERA> objERA = BLLERAObj.UpdateERA(dsERA);
                    string successMsg;
                    if (objERA.Data != null)
                    {
                        if (MDVUtility.ToLong(model.IsActive) == 0)
                            successMsg = Common.AppPrivileges.Inactive_Message;
                        else
                            successMsg = Common.AppPrivileges.Active_Message;
                        var response = new
                        {
                            status = true,
                            message = successMsg
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objERA.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }
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
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }

        }
        public string DownloadERA(ERAModel model)
        {
            try
            {
                BLObject<bool> obj;
                obj = BLLERAObj.DownloadERA(MDVUtility.ToLong(model.EDIReportID));

                if (obj.Data == true)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Download_Success_Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string ElectronicEOB(ERAModel model)
        {
            try
            {
                BLObject<string> obj = BLLBillingClaimObj.ElectronicEOB(MDVUtility.ToLong(model.ChargeID), MDVUtility.ToLong(model.VisitID));

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ElectronicEOB_JSON = obj.Data
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
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion


    }
}