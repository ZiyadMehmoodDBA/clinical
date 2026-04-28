using MDVision.Business.BCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MDVision.IEHR.Common;
using MDVision.IEHR.Model.Billing.ERA;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Common.Logging;
using MDVision.Model.Billing.ERA;

namespace MDVision.IEHR.Controls.Billing.ERA
{
    public class Bill_ERA_Detail
    {

        private BLLBilling BLLBillingObj = null;
        private BLLERA BLLERAObj = null;
        private BLLVisits BLLVisitsObj = null;
        public Bill_ERA_Detail()
        {
            BLLBillingObj = new BLLBilling();
            BLLERAObj = new BLLERA();
            BLLVisitsObj = new BLLVisits();
        }
        #region Singleton
        private static Bill_ERA_Detail _obj = null;
        public static Bill_ERA_Detail Instance()
        {
            if (_obj == null)
                _obj = new Bill_ERA_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the ERA Adjustment Codes.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>

        public string SaveERA(ERAModel model)
        {
            try
            {

                DSERA dsERA = new DSERA();
                DSERA.ERARow dr = dsERA.ERA.NewERARow();
                dr.ERAId = -1;
                if (!string.IsNullOrEmpty(model.FacilityID))
                    dr.FacilityId = MDVUtility.ToInt64(model.FacilityID);
                if (!string.IsNullOrEmpty(model.ProviderID))
                    dr.ProviderId = MDVUtility.ToInt64(model.ProviderID);

                if (!string.IsNullOrEmpty(model.ClearingHouse))
                    dr.ClearingHouseId = MDVUtility.ToInt64(model.ClearingHouse);
                if (!string.IsNullOrEmpty(model.ERAStatus))
                    dr.ERAStatusId = MDVUtility.ToInt64(model.ERAStatus);


                // dr.LedgerAccountId = string.IsNullOrWhiteSpace(SearchedfieldsJSON["LedgerAccountId"]);

                dr.CheckNo = model.CheckNumber;
                if (!string.IsNullOrEmpty(model.CheckAmount))
                    dr.CheckAmount = MDVUtility.ToInt64(model.CheckAmount);
                if (!string.IsNullOrEmpty(model.CheckDate))
                    dr.CheckDate = MDVUtility.StringToDate(model.CheckDate);
                if (!string.IsNullOrEmpty(model.CheckDepositDate))
                    dr.CheckDepositDate = MDVUtility.StringToDate(model.CheckDepositDate);
                //dr.AppliedAmount = SearchedfieldsJSON["AppliedAmount"];
                //dr.PostedAmount = SearchedfieldsJSON["PostedAmount"];
                //dr.DateofEntry = SearchedfieldsJSON["DateofEntry"];
                //dr.User = SearchedfieldsJSON["User"];



                dr.PayerName = model.PayerName;
                dr.PayerAddress = model.PayerAddress;
                dr.PayerCity = model.PayerCity;
                dr.PayerState = model.PayerState;
                dr.PayerZipCode = model.PayerZip;


                dr.PayeeName = model.PayeeName;
                dr.PayeeAddress = model.PayeeAddress;
                dr.PayeeCity = model.PayeeCity;
                dr.PayeeState = model.PayeeState;
                dr.PayeeZipCode = model.PayeeZip;


                dr.IsActive = true;// MDVUtility.ToStr(SearchedfieldsJSON["IsActive"]) == "True" ? true : false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsERA.ERA.AddERARow(dr);
                BLObject<DSERA> obj = BLLERAObj.InsertERA(dsERA);
                dsERA = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        ERAId = dsERA.Tables[dsERA.ERA.TableName].Rows[0][dsERA.ERA.ERAIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                #endregion
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
        /// <summary>
        /// Updates the ERA Adjustment Codes.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ERAId">The procedure category identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        public string UpdateERA(ERAModel model)
        {
            try
            {
                DSERA dsCode = null;
                BLObject<DSERA> obj = BLLERAObj.LoadERA(MDVUtility.ToInt64(model.ERAID), 0, null, 0, 0, null, 0, "", null, null);
                dsCode = obj.Data;
                if (dsCode.Tables[dsCode.ERA.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsCode.Tables[dsCode.ERA.TableName].Rows[0];

                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["Clearinghouse"]))
                    //    dr[dsCode.ERA.ClearingHouseIdColumn.ColumnName] = MDVUtility.ToInt64(SearchedfieldsJSON["Clearinghouse"]);
                    //else
                    //{
                    //    dr[dsCode.ERA.ClearingHouseIdColumn.ColumnName] = DBNull.Value;
                    //}
                    if (!string.IsNullOrEmpty(model.ERAStatus))
                        dr[dsCode.ERA.ERAStatusIdColumn.ColumnName] = MDVUtility.ToInt64(model.ERAStatus);
                    else
                    {
                        dr[dsCode.ERA.ERAStatusIdColumn.ColumnName] = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(model.ProviderID))
                        dr[dsCode.ERA.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(model.ProviderID.ToString());
                    else
                    {
                        dr[dsCode.ERA.ProviderIdColumn.ColumnName] = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(model.FacilityID))
                        dr[dsCode.ERA.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(model.FacilityID);
                    else
                    {
                        dr[dsCode.ERA.FacilityIdColumn.ColumnName] = DBNull.Value;
                    }

                    //dr[dsCode.ERA.CheckNoColumn.ColumnName] = SearchedfieldsJSON["CheckNo"];
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["CheckAmount"]))
                    //    dr[dsCode.ERA.CheckAmountColumn.ColumnName] = MDVUtility.ToInt64(SearchedfieldsJSON["CheckAmount"]);
                    //else
                    //{
                    //    dr[dsCode.ERA.CheckAmountColumn.ColumnName] = DBNull.Value;
                    //}

                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["CheckDate"]))
                    //    dr[dsCode.ERA.CheckDateColumn.ColumnName] = MDVUtility.StringToDate(SearchedfieldsJSON["CheckDate"]);
                    //else
                    //{
                    //    dr[dsCode.ERA.CheckDateColumn.ColumnName] = DBNull.Value;
                    //}

                    if (!string.IsNullOrEmpty(model.CheckDepositDate))
                        dr[dsCode.ERA.CheckDepositDateColumn.ColumnName] = MDVUtility.StringToDate(model.CheckDepositDate);
                    else
                    {
                        dr[dsCode.ERA.CheckDepositDateColumn.ColumnName] = DBNull.Value;
                    }

                    //dr[dsCode.ERA.AppliedAmountColumn.ColumnName]      = SearchedfieldsJSON["AppliedAmount"];
                    //dr[dsCode.ERA.PostedAmountColumn.ColumnName]        = SearchedfieldsJSON["PostedAmount"];
                    //dr[dsCode.ERA.DateofEntryColumn.ColumnName]    = SearchedfieldsJSON["DateofEntry"];
                    // dr[dsCode.ERA.UserColumn.ColumnName]       = SearchedfieldsJSON["User"];



                    //dr[dsCode.ERA.PayerNameColumn.ColumnName] = SearchedfieldsJSON["PayerName"];
                    //dr[dsCode.ERA.PayerAddressColumn.ColumnName] = SearchedfieldsJSON["PayerAddress"];
                    //dr[dsCode.ERA.PayerCityColumn.ColumnName] = SearchedfieldsJSON["PayerCity"];
                    //dr[dsCode.ERA.PayerStateColumn.ColumnName] = SearchedfieldsJSON["PayerState"];
                    //dr[dsCode.ERA.PayerZipCodeColumn.ColumnName] = SearchedfieldsJSON["PayerZip"];


                    //dr[dsCode.ERA.PayeeNameColumn.ColumnName] = SearchedfieldsJSON["PayeeName"];
                    //dr[dsCode.ERA.PayeeAddressColumn.ColumnName] = SearchedfieldsJSON["PayeeAddress"];
                    //dr[dsCode.ERA.PayeeCityColumn.ColumnName] = SearchedfieldsJSON["PayeeCity"];
                    //dr[dsCode.ERA.PayeeStateColumn.ColumnName] = SearchedfieldsJSON["PayeeState"];
                    //dr[dsCode.ERA.PayeeZipCodeColumn.ColumnName] = SearchedfieldsJSON["PayeeZip"];

                    dr[dsCode.ERA.IsActiveColumn.ColumnName] = true;// MDVUtility.ToStr(SearchedfieldsJSON["IsActive"]) == "True" ? true : false;

                    dr[dsCode.ERA.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr[dsCode.ERA.ModifiedOnColumn.ColumnName] = DateTime.Now;




                    BLObject<DSERA> objUpdated = BLLERAObj.UpdateERA(dsCode);
                    if (objUpdated.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                        Message = ""
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        /// <summary>
        /// Fills the ERA Adjustment Codes.
        /// </summary>
        /// <param name="ProcedureCategoryId">The procedure category identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        public string FillERA(ERAModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ERAID)))
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
                    DSERA dsERA = null;

                    BLObject<DSERA> obj = BLLERAObj.LoadERA(MDVUtility.ToLong(model.ERAID), 0, "", 0, 0, "", 0,  "", "", "", 1, 15, "FillERA");
                    if (obj.Data != null)
                    {
                        dsERA = obj.Data;
                        if (dsERA.Tables[dsERA.ERA.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsERA.Tables[dsERA.ERA.TableName].Rows[0];

                            model.ERAID = MDVUtility.ToStr(dr[dsERA.ERA.ERAIdColumn.ColumnName]);
                            model.ClearingHouse = MDVUtility.ToStr(dr[dsERA.ERA.ClearingHouseIdColumn.ColumnName]);
                            model.FacilityID = MDVUtility.ToStr(dr[dsERA.ERA.FacilityIdColumn.ColumnName]);
                            model.Facility = MDVUtility.ToStr(dr[dsERA.ERA.FacilityNameColumn.ColumnName]);
                            model.IsActive = MDVUtility.ToStr(dr[dsERA.ERA.IsActiveColumn.ColumnName]);
                            model.Provider = MDVUtility.ToStr(dr[dsERA.ERA.ProviderNameColumn.ColumnName]);
                            model.ERAStatus = MDVUtility.ToStr(dr[dsERA.ERA.ERAStatusIdColumn.ColumnName]);
                            model.CheckNumber = MDVUtility.ToStr(dr[dsERA.ERA.CheckNoColumn.ColumnName]);
                            model.CheckAmount = MDVUtility.ToStr(dr[dsERA.ERA.CheckAmountColumn.ColumnName]);
                            //model.CheckDate = MDVUtility.ToStr(dr[dsERA.ERA.CheckDateColumn.ColumnName]); 
                            model.CheckDate = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsERA.ERA.CheckDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsERA.ERA.CheckDateColumn.ColumnName]).ToShortDateString();
                            model.CheckDepositDate = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsERA.ERA.CheckDepositDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsERA.ERA.CheckDepositDateColumn.ColumnName]).ToShortDateString();
                            model.AppliedAmount = MDVUtility.ToStr(dr[dsERA.ERA.AppliedAmountColumn.ColumnName]);
                            model.PostedAmount = MDVUtility.ToStr(dr[dsERA.ERA.PostedAmountColumn.ColumnName]);
                            model.DateofEntry = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsERA.ERA.CreatedOnColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsERA.ERA.CreatedOnColumn.ColumnName]).ToShortDateString();
                            model.User = MDVUtility.ToStr(dr[dsERA.ERA.CreatedByNameColumn.ColumnName]);
                            model.PayerName = MDVUtility.ToStr(dr[dsERA.ERA.PayerNameColumn.ColumnName]);
                            model.PayerAddress = MDVUtility.ToStr(dr[dsERA.ERA.PayerAddressColumn.ColumnName]);
                            model.PayerCity = MDVUtility.ToStr(dr[dsERA.ERA.PayerCityColumn.ColumnName]);
                            model.PayerState = MDVUtility.ToStr(dr[dsERA.ERA.PayerStateColumn.ColumnName]);
                            model.PayerZip = MDVUtility.ToStr(dr[dsERA.ERA.PayerZipCodeColumn.ColumnName]);
                            model.PayeeName = MDVUtility.ToStr(dr[dsERA.ERA.PayeeNameColumn.ColumnName]);
                            model.PayeeAddress = MDVUtility.ToStr(dr[dsERA.ERA.PayeeAddressColumn.ColumnName]);
                            model.PayeeCity = MDVUtility.ToStr(dr[dsERA.ERA.PayeeCityColumn.ColumnName]);
                            model.PayeeState = MDVUtility.ToStr(dr[dsERA.ERA.PayeeStateColumn.ColumnName]);
                            model.PayeeZip = MDVUtility.ToStr(dr[dsERA.ERA.PayeeZipCodeColumn.ColumnName]);

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ERAFill_JSON = js.Serialize(model)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string FillERALinkedHistory(ERAModel model)
        {
            try
            {
                DSERA dsERA = null;
                BLObject<DSERA> obj = BLLERAObj.FillERALinkedHistory(MDVUtility.ToLong(model.ERADetailID));
                dsERA = obj.Data;
                if (dsERA != null)
                {
                    var response = new
                    {
                        status = true,
                        LinkedHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsERA.Tables[dsERA.ERALinkedHistory.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        public string FillERADetail(ERAModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ERAID)))
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
                    DSERA dsERA = null;
                    BLObject<DSERA> obj = BLLERAObj.LoadERADetail(0, MDVUtility.ToLong(model.ERAID));
                    dsERA = obj.Data;

                    //Load Provider Adjustments
                    BLObject<DSERA> obj1 = BLLERAObj.LoadERAProviderAdjustmentCode(0, MDVUtility.ToLong(model.ERAID));
                    dsERA.Merge(obj1.Data);

                    if (dsERA.Tables[dsERA.ERADetail.TableName].Rows.Count > 0)
                    {
                        IEnumerable<DataRow> LinkedPaidCharges = dsERA.ERADetail.Select(dsERA.ERADetail.ChargeIdColumn.ColumnName + " is not null AND ( " + dsERA.ERADetail.ProcessAsColumn.ColumnName + "<>'Denied' AND " + dsERA.ERADetail.ProcessAsColumn.ColumnName + "<>'Reversal of Previous Payment' AND (" + dsERA.ERADetail.PaidAmountColumn + " > '0' OR " + dsERA.ERADetail.TotalAmountColumn + " > '0'  ) )",dsERA.ERADetail.ClaimNumberColumn.ColumnName +" ASC, " + dsERA.ERADetail.ChargeOrderColumn.ColumnName + " ASC");
                        int LinkedPaidChargessCount = LinkedPaidCharges.Count();
                        IEnumerable<DataRow> LinkedUnpaidCharges = dsERA.ERADetail.Select(dsERA.ERADetail.ChargeIdColumn.ColumnName + " is not null AND " + dsERA.ERADetail.ProcessAsColumn.ColumnName + "<>'Reversal of Previous Payment' AND ( " + dsERA.ERADetail.ProcessAsColumn.ColumnName + "='Denied'" + " OR (" + dsERA.ERADetail.PaidAmountColumn + "='0' AND " + dsERA.ERADetail.TotalAmountColumn + "='0' ) ) ", dsERA.ERADetail.ClaimNumberColumn.ColumnName + " ASC, " + dsERA.ERADetail.ChargeOrderColumn.ColumnName + " ASC");
                        int LinkedUnpaidChargesCount = LinkedUnpaidCharges.Count();
                        IEnumerable<DataRow> NotLinkedCharges = dsERA.ERADetail.Select(dsERA.ERADetail.ChargeIdColumn.ColumnName + " is null", dsERA.ERADetail.ClaimNumberColumn.ColumnName + " ASC, " + dsERA.ERADetail.ChargeOrderColumn.ColumnName + " ASC");
                        int NotLinkedChargesCount = NotLinkedCharges.Count();
                        IEnumerable<DataRow> LinkedRecoupmentCharges = dsERA.ERADetail.Select(dsERA.ERADetail.ChargeIdColumn.ColumnName + " is not null AND ( " + dsERA.ERADetail.ProcessAsColumn.ColumnName + "='Reversal of Previous Payment' ) ", dsERA.ERADetail.ClaimNumberColumn.ColumnName + " ASC, " + dsERA.ERADetail.ChargeOrderColumn.ColumnName + " ASC");
                        int LinkedRecoupmentCount = LinkedRecoupmentCharges.Count();

                        string LinkedPaid = string.Empty;
                        string LinkedUnpaid = string.Empty;
                        string NotLinked = string.Empty;
                        string LinkedRecoupment = string.Empty;

                        if (LinkedPaidChargessCount > 0)
                        {
                            // LinkedPaid = MDVUtility.JSON_DataTable(LinkedPaidCharges.ToList()[0].Table);

                            DataTable dtLinkedPaidCharges = LinkedPaidCharges.CopyToDataTable<DataRow>();
                            LinkedPaid = MDVUtility.JSON_DataTable(dtLinkedPaidCharges);

                        }
                        if (NotLinkedChargesCount > 0)
                        {
                            DataTable dtNotLinkedCharges = NotLinkedCharges.CopyToDataTable<DataRow>();
                            NotLinked = MDVUtility.JSON_DataTable(dtNotLinkedCharges);
                        }
                        if (LinkedUnpaidChargesCount > 0)
                        {
                            DataTable dtLinkedUnpaidCharges = LinkedUnpaidCharges.CopyToDataTable<DataRow>();
                            LinkedUnpaid = MDVUtility.JSON_DataTable(dtLinkedUnpaidCharges);
                        }
                        if (LinkedRecoupmentCount > 0)
                        {
                            DataTable dtLinkedRecoupmentCharges = LinkedRecoupmentCharges.CopyToDataTable<DataRow>();
                            LinkedRecoupment = MDVUtility.JSON_DataTable(dtLinkedRecoupmentCharges);
                        }

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            LinkedPaidCharges = LinkedPaid,
                            LinkedPaidChargessCount = LinkedPaidChargessCount,

                            NotLinkedCharges = NotLinked,
                            NotLinkedChargesCount = NotLinkedChargesCount,

                            LinkedUnpaidCharges = LinkedUnpaid,
                            LinkedUnpaidChargesCount = LinkedUnpaidChargesCount,

                            LinkedRecoupmentCharges = LinkedRecoupment,
                            LinkedRecoupmentChargesCount = LinkedRecoupmentCount,

                            PLBSegment_JSON = MDVUtility.JSON_DataTable(dsERA.Tables[dsERA.ERAProviderAdjustments.TableName]),
                            PLBSegmentCount = dsERA.ERAProviderAdjustments.Rows.Count,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            LinkedPaidCharges = string.Empty,
                            LinkedPaidChargessCount = 0,

                            NotLinkedCharges = string.Empty,
                            NotLinkedChargesCount = 0,

                            LinkedUnpaidCharges = string.Empty,
                            LinkedUnpaidChargesCount = 0,

                            LinkedRecoupmentCharges = string.Empty,
                            LinkedRecoupmentChargesCount = 0,

                            PLBSegment_JSON = string.Empty,
                            PLBSegmentCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string UpdateERADetail(ERAModel model)
        {
            try
            {


                DSERA dsERADetail = new DSERA();
                BLObject<DSERA> objLoad = BLLERAObj.LoadERADetail(MDVUtility.ToLong(model.ERADetailID), 0);
                if (objLoad.Data != null)
                {
                    dsERADetail = objLoad.Data;
                    DSERA.ERADetailRow dr = (DSERA.ERADetailRow)dsERADetail.Tables[dsERADetail.ERADetail.TableName].Rows[0];

                    if (!String.IsNullOrEmpty(model.ChargeID))
                    {
                        dr.ChargeId = MDVUtility.ToLong(model.ChargeID);
                        dr.ClaimNumber = model.ClaimNumber;

                    }

                    dr.IsActive = true;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;



                    #region Database Updation

                    if (dsERADetail.Tables[dsERADetail.ERADetail.TableName].Rows.Count > 0)
                    {
                        //  dsPatient.Patients.Rows[0].SetModified();
                        BLObject<DSERA> obj = BLLERAObj.UpdateERADetail(dsERADetail);

                        //SET ICN 
                        #region " ICN "

                        long VisitId = 0;
                        DSCharge dsCharge = null;
                        BLObject<DSCharge> objDSCharge = BLLBillingObj.LoadPatientCharges(dr.ChargeId, "", "", 0, 0, "", 0, "", "");
                        if (objDSCharge.Data != null)
                        {
                            dsCharge = objDSCharge.Data;
                            DSCharge.PatientChargesRow drTemp = (DSCharge.PatientChargesRow)dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows[0];
                            VisitId = MDVUtility.ToLong(drTemp.VisitId);
                            if (VisitId != 0)
                            {
                                BLObject<DSVisits> objVisit = null;
                                objVisit = BLLVisitsObj.LoadPatientsVisits(VisitId, 0, 0, 0, null, null, "", "", "");
                                if (objVisit.Data != null)
                                {
                                    DSVisits dsVisit = null;
                                    dsVisit = objVisit.Data;
                                    DSVisits.PatientVisitsRow drVisit = (DSVisits.PatientVisitsRow)dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows[0];
                                    drVisit.ICDDCN = dr.ICN;

                                    // Update Patient Visit
                                    objVisit = BLLVisitsObj.UpdatePatientsVisit(dsVisit);

                                }
                            }
                        }

                        #endregion

                        if (obj.Data != null)
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
                            Message = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objLoad.Message,
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

        public string LinkERADetail(ERAModel model)
        {
            try
            {
                BLObject<string> obj = BLLERAObj.LinkERADetail(MDVUtility.ToLong(model.ERADetailID), MDVUtility.ToLong(model.ChargeID), Convert.ToBoolean(model.IsLink), MDVUtility.ToLong(model.PaymentInsuranceID));

                if (string.IsNullOrEmpty(obj.Data))
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
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
       
        public string IsVoidedClaimExist(ERAModel model)
        {
            try
            {
                BLObject<List<ERAVoidedClaims>> objERAVoidedClaims = BLLERAObj.IsVoidedClaimExist(MDVUtility.ToInt64(model.ERAID), MDVUtility.ToInt64(model.VisitID));
                if (objERAVoidedClaims.Data.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        VoidedClaims = objERAVoidedClaims.Data[0].VoidedClaims,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objERAVoidedClaims.Message,
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
        public string PostPayment(ERAModel model)
        {
            try
            {
                BLObject<Dictionary<string, DSERA>> obj = BLLERAObj.PostPayment(MDVUtility.ToLong(model.ERAID), MDVUtility.ToLong(model.ERADetailID), "PostPayment");
                if (obj.Data != null)
                {
                    DSERA UnPostedCharges = new DSERA();
                    DSERA PostedCharges = new DSERA();
                    if (obj.Data.ContainsKey("UnPostedCharges"))
                        UnPostedCharges = obj.Data["UnPostedCharges"];
                    if (obj.Data.ContainsKey("PostedCharges"))
                        PostedCharges = obj.Data["PostedCharges"];

                    string Message = string.Empty;

                    if (UnPostedCharges.ERADetail.Rows.Count == 0)
                        Message = Common.AppPrivileges.Payment_Posted_Success;
                    else
                        Message = Common.AppPrivileges.Payment_Posted_Partially_Success;

                    bool IsMessage = true;
                    bool status = true;
                    int PostedERACount = 0;
                    if (PostedCharges.ERADetail.Rows.Count > 0)
                    {
                        PostedERACount = Convert.ToInt32(PostedCharges.ERADetail.Rows[0][PostedCharges.ERADetail.PostedERADtlCountColumn.ColumnName]);
                    }
                    int totalcharges = MDVUtility.ToInt32(PostedERACount) + UnPostedCharges.ERADetail.Rows.Count;
                    if (totalcharges == 0)
                    {
                        status = false;
                        IsMessage = true;
                        Message = Common.AppPrivileges.No_Charge_Message;
                    }
                    else if (totalcharges == UnPostedCharges.ERADetail.Rows.Count)
                        IsMessage = false;


                    var response = new
                    {
                        status = status,
                        UnPostedChargesCount = UnPostedCharges.ERADetail.Rows.Count,
                        UnPostedCharges_JSON = MDVUtility.JSON_DataTable(UnPostedCharges.Tables[UnPostedCharges.ERADetail.TableName]),
                        //PostedChargesCount = PostedCharges.ERADetail.Rows.Count,
                        //PostedCharges_JSON = MDVUtility.JSON_DataTable(PostedCharges.Tables[PostedCharges.ERADetail.TableName]),
                        Message = Message,
                        IsMessage = IsMessage,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message,
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

        public string PostChargesPayment(ERAModel model)
        {
            try
            {
                BLObject<bool> obj = BLLERAObj.PostChargesPayment(MDVUtility.ToLong(model.ERAID), MDVUtility.ToStr(model.ERADetailIDs), "PostChargesPayment");
                if (obj.Data)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Payment_Posted_Success,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message,
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

        public string DeleteERADetail(ERAModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ERADetailID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLERAObj.DeleteERADetail(MDVUtility.ToStr(model.ERADetailID));
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

        private string UpdateERADetail(long ERADetailId, string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSERA dsERADetail = new DSERA();
                BLObject<DSERA> objLoad = BLLERAObj.LoadERADetail(MDVUtility.ToLong(ERADetailId), 0);
                if (objLoad.Data != null)
                {
                    dsERADetail = objLoad.Data;
                    DSERA.ERADetailRow dr = (DSERA.ERADetailRow)dsERADetail.Tables[dsERADetail.ERADetail.TableName].Rows[0];

                    dr.PaidAmount = MDVUtility.ToDecimal(MDVUtility.ToStr(SearchedfieldsJSON["PaidAmount" + ERADetailId]));
                    dr.WriteOff = MDVUtility.ToDecimal(MDVUtility.ToStr(SearchedfieldsJSON["WriteOff" + ERADetailId]));
                    dr.DeductableAmount = MDVUtility.ToDecimal(MDVUtility.ToStr(SearchedfieldsJSON["Deductable" + ERADetailId]));
                    dr.CoInsuranceAmount = MDVUtility.ToDecimal(MDVUtility.ToStr(SearchedfieldsJSON["CoInsurance" + ERADetailId]));
                    dr.Copayment = MDVUtility.ToDecimal(MDVUtility.ToStr(SearchedfieldsJSON["Copayment" + ERADetailId]));
                    //dr.ChargeCopay = MDVUtility.ToDecimal(MDVUtility.ToStr(SearchedfieldsJSON["ChargeCopay" + ERADetailId]));
                    dr.PatientResponsibility = MDVUtility.ToDecimal(MDVUtility.ToStr(SearchedfieldsJSON["PatientResponsibility" + ERADetailId]));
                    dr.NextResponsibility = MDVUtility.ToStr(SearchedfieldsJSON["ddlNextResponsibility" + ERADetailId]);

                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlNextInsurancePlan" + ERADetailId]))
                    dr.NextResponsibilityId = MDVUtility.ToLong(MDVUtility.ToStr(SearchedfieldsJSON["ddlNextInsurancePlan" + ERADetailId]));




                    dr.IsActive = true;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Updation

                    if (dsERADetail.Tables[dsERADetail.ERADetail.TableName].Rows.Count > 0)
                    {
                        BLObject<DSERA> obj = BLLERAObj.UpdateERADetail(dsERADetail);

                        if (obj.Data != null)
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
                            Message = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objLoad.Message,
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
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "UPDATE_ERA_DETAIL_RECORD":
                    {
                        long ERADetailId = MDVUtility.ToLong(context.Request["ERADetailId"]);
                        string ERADate = MDVUtility.ToStr(context.Request["ERAData"]);

                        string strJSONData = UpdateERADetail(ERADetailId, ERADate);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


            }
        }







        #endregion
    }
}