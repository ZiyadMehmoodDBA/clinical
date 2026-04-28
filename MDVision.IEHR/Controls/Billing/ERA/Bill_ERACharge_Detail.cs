using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using System.Data;
using MDVision.IEHR.Model.Billing.ERA;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Billing.ERA
{
    public class Bill_ERACharge_Detail
    {
        private BLLBilling BLLBillingObj = null;
        private BLLERA BLLERAObj = null;
        public Bill_ERACharge_Detail()
        {
            BLLBillingObj = new BLLBilling();
            BLLERAObj = new BLLERA();
        }
        #region Singleton
        private static Bill_ERACharge_Detail _obj = null;
        public static Bill_ERACharge_Detail Instance()
        {
            if (_obj == null)
                _obj = new Bill_ERACharge_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions

        public string FillChargeDetail(ERAModel model)
        {
            // string Format = "{0:0.00}";
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ERADetailID)) && string.IsNullOrEmpty(MDVUtility.ToStr(model.ERAID)))
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
                    BLObject<DSERA> obj = BLLERAObj.LoadERADetail(MDVUtility.ToLong(model.ERADetailID), MDVUtility.ToLong(model.ERAID), "ERAChargeDetail");
                    dsERA = obj.Data;

                    DSERA dsAdjustmentCode = null;
                    BLObject<DSERA> objAdjustmentCode = BLLERAObj.LoadERAClaimAdjustmentCode(0, MDVUtility.ToLong(model.ERADetailID));
                    dsAdjustmentCode = objAdjustmentCode.Data;


                    DSCharge dsCharge = null;


                    if (dsERA.Tables[dsERA.ERADetail.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsERA.Tables[dsERA.ERADetail.TableName].Rows[0];
                        long chargeCapID = MDVUtility.ToLong(dr[dsERA.ERADetail.ChargeIdColumn.ColumnName]);
                        string JSONCharge = null;
                        if (chargeCapID != 0)
                        {
                            BLObject<DSCharge> objDSCharge = BLLBillingObj.LoadPatientCharges(MDVUtility.ToLong(dr[dsERA.ERADetail.ChargeIdColumn.ColumnName]), "", "", 0, 0, "", 0, "", "", null, null, 0, 1);
                            dsCharge = objDSCharge.Data;
                            JSONCharge = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientCharges.TableName]);
                        }

                        model.DOSFrom = string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsERA.ERADetail.DOSFromColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsERA.ERADetail.DOSFromColumn.ColumnName]).ToShortDateString();
                        model.DOSTo = string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsERA.ERADetail.DOSToColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsERA.ERADetail.DOSToColumn.ColumnName]).ToShortDateString();
                        model.ClaimNumber = MDVUtility.ToStr(dr[dsERA.ERADetail.ClaimNumberColumn.ColumnName]);
                        model.FirstName = MDVUtility.ToStr(dr[dsERA.ERADetail.PatFirstNameColumn.ColumnName]);
                        model.LastName = MDVUtility.ToStr(dr[dsERA.ERADetail.PatLastNameColumn.ColumnName]);
                        model.DOB = MDVUtility.ToStr(dr[dsERA.ERADetail.PatDOBColumn.ColumnName]);
                        model.ERAClaimNumber = MDVUtility.ToStr(dr[dsERA.ERADetail.ERAClaimNumberColumn.ColumnName]);
                        model.ChargeNumber = MDVUtility.ToStr(dr[dsERA.ERADetail.ChargeIdColumn.ColumnName]);
                        model.ERAChargeNumber = MDVUtility.ToStr(dr[dsERA.ERADetail.ERAChargeNumberColumn.ColumnName]);
                        model.SubscriberID = MDVUtility.ToStr(dr[dsERA.ERADetail.SubscriberIdColumn.ColumnName]);
                        model.CPT = MDVUtility.ToStr(dr[dsERA.ERADetail.CPTCodeColumn.ColumnName]);
                        model.Modifiers = MDVUtility.ToStr(dr[dsERA.ERADetail.ModifierCodeColumn.ColumnName]);
                        model.Units = MDVUtility.ToStr(dr[dsERA.ERADetail.UnitsBilledColumn.ColumnName]);
                        model.POS = MDVUtility.ToStr(dr[dsERA.ERADetail.POSColumn.ColumnName]);
                        model.Status = MDVUtility.ToStr(dr[dsERA.ERADetail.ProcessAsColumn.ColumnName]);
                        model.ICN = MDVUtility.ToStr(dr[dsERA.ERADetail.ICNColumn.ColumnName]);
                        model.SecondaryInsPerERA = MDVUtility.ToStr(dr[dsERA.ERADetail.SecondaryInsuranceColumn.ColumnName]);
                        model.SecondarySubscriberID = MDVUtility.ToStr(dr[dsERA.ERADetail.SecondarySubscriberIdColumn.ColumnName]);
                        model.CrossedOver = MDVUtility.ToStr(dr[dsERA.ERADetail.IsCrossedOverColumn.ColumnName]).ToLower() == "true" ? "Yes" : "No";
                        model.PaymentStatus = MDVUtility.ToStr(dr[dsERA.ERADetail.PostStatusColumn.ColumnName]);
                        model.ChargedAmount = MDVUtility.ToStr(dr[dsERA.ERADetail.ChargedAmountColumn.ColumnName]);
                        model.AllowedAmount = MDVUtility.ToStr(dr[dsERA.ERADetail.AllowedAmountColumn.ColumnName]);
                        model.PaidAmount = MDVUtility.ToStr(dr[dsERA.ERADetail.PaidAmountColumn.ColumnName]);
                        model.CoInsuranceAmount = MDVUtility.ToStr(dr[dsERA.ERADetail.CoInsuranceAmountColumn.ColumnName]);
                        model.DeductibleAmount = MDVUtility.ToStr(dr[dsERA.ERADetail.DeductableAmountColumn.ColumnName]);
                        model.Copayment = MDVUtility.ToStr(dr[dsERA.ERADetail.CopaymentColumn.ColumnName]);
                        model.LateFilingCharges = MDVUtility.ToStr(dr[dsERA.ERADetail.LateFilingChargesColumn.ColumnName]);
                        model.InterestAmount = MDVUtility.ToStr(dr[dsERA.ERADetail.InterestAmountColumn.ColumnName]);
                        model.LinkedBy = MDVUtility.ToStr(dr[dsERA.ERADetail.CreatedByNameColumn.ColumnName]);
                        model.LinkedDate = MDVUtility.ToStr(dr[dsERA.ERADetail.LinkedDateColumn.ColumnName]);

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ERA_Detail_Fill_JSON = js.Serialize(model),
                            //ERA_Detail_JSON = MDVUtility.JSON_DataTable(dsERA.Tables[dsERA.ERADetail.TableName]),
                            ERA_Detail_AdjCode_JSON = MDVUtility.JSON_DataTable(dsAdjustmentCode.Tables[dsAdjustmentCode.ERAClaimAdjustmentCode.TableName]),
                            ERA_LinkedCharge_Detail_JSON = JSONCharge
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

        #endregion
    }
}