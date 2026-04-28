using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using MDVision.IEHR.Model.Billing.Charges;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Billing
{
    public class Bill_ChargeSearch
    {
        private BLLBillingClaim BLLBillingClaimObj = null;
        private BLLBilling BLLBillingObj = null;

        public Bill_ChargeSearch()
        {
            BLLBillingClaimObj = new BLLBillingClaim();
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Bill_ChargeSearch _obj = null;
        public static Bill_ChargeSearch Instance()
        {
            if (_obj == null)
                _obj = new Bill_ChargeSearch();
            return _obj;
        }
        #endregion

        #region Private Functions

        public string DeleteCharge(ChargeModel model)
        {

            try
            {
                if (MDVUtility.ToLong(model.ChargeCapId) == 0)
                {
                    var response = new
                {
                    status = false,
                    Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBillingObj.DeletePatientCharges(MDVUtility.ToLong(model.ChargeCapId));
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


        //Begin Added by Azeem Raza Tayyab on 29-Apr-2016 for the changes related to Bug#:PMS-4685
        private string ChangeVisitStatus(string VisitsIds, string SubmitStatusId)
        {

            try
            {
                BLObject<bool> obj;
                if (!string.IsNullOrEmpty(VisitsIds) && !string.IsNullOrEmpty(SubmitStatusId))
                {
                    obj = BLLBillingClaimObj.UpdateVisitsSubmitStatus(VisitsIds, MDVUtility.ToInt32(SubmitStatusId));
                    if (obj.Data == true)
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
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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
        //End Added by Azeem Raza Tayyab on 29-Apr-2016 for the changes related to Bug#:PMS-4685

        public string SearchCharge(ChargeModel model)
        {
            string ClaimCreatedBy = null;
            string ClaimType = null;
            bool IncludeSecondaryClaim = true;
            bool IncludeVoidedClaims = false;
            
            try
            {
                if (string.IsNullOrEmpty(model.Facility))
                    model.FacilityId = "0";

                if (string.IsNullOrEmpty(model.Provider))
                    model.ProviderId = "0";

                if (string.IsNullOrEmpty(model.ResourceProvider))
                    model.ResourceProviderId = "0";

                if (string.IsNullOrEmpty(model.InsurancePlan))
                    model.InsurancePlanId = "0";


                //  IncludeSecondaryClaim = Convert.ToBoolean(model.IncludeSecondaryClaim);
                IncludeVoidedClaims = Convert.ToBoolean(model.IncludeVoidedClaims);
                
                if (model.Hold_Text == "All")
                {
                    model.Hold = null;
                }

                if (model.CreatedBy_Text == "- Select -")
                    ClaimCreatedBy = null;
                else
                    ClaimCreatedBy = model.CreatedBy;

                if (model.Paid_Text == "All" || model.Paid_Text == "- Select -")
                {
                    model.Paid = null;
                }



                DateTime? dtfrom = String.IsNullOrEmpty(model.DOSFrom) ? (DateTime?)null : DateTime.Parse(model.DOSFrom);
                DateTime? dtto = String.IsNullOrEmpty(model.DOSTo) ? (DateTime?)null : DateTime.Parse(model.DOSTo);

                DateTime? dtSubmittedTo = String.IsNullOrEmpty(model.SubmittedDateTo) ? (DateTime?)null : DateTime.Parse(model.SubmittedDateTo);
                DateTime? dtSubmittedFrom = String.IsNullOrEmpty(model.SubmittedDateFrom) ? (DateTime?)null : DateTime.Parse(model.SubmittedDateFrom);

                DateTime? ImportedDateFrom = null;
                DateTime? ImportedDateTo = null;

                DateTime? ClaimCreatedFrom = String.IsNullOrEmpty(model.DateClaimCreatedFrom) ? (DateTime?)null : DateTime.Parse(model.DateClaimCreatedFrom);
                DateTime? ClaimCreatedTo = String.IsNullOrEmpty(model.DateClaimCreatedTo) ? (DateTime?)null : DateTime.Parse(model.DateClaimCreatedTo);
                DateTime? EncounterSignOffDate = String.IsNullOrEmpty(model.EncounterSignOffDate) ? (DateTime?)null : DateTime.Parse(model.EncounterSignOffDate);


                if (dtto == null)
                    dtto = dtfrom;
                if (dtfrom == null)
                    dtfrom = dtto;

                ClaimType = !string.IsNullOrEmpty(model.ClaimType) ? MDVUtility.ToStr(model.ClaimType) : "";

                ImportedDateFrom = String.IsNullOrEmpty(model.ImportedOnFrom) ? (DateTime?)null : DateTime.Parse(model.ImportedOnFrom);

                ImportedDateTo = String.IsNullOrEmpty(model.ImportedOnTo) ? (DateTime?)null : DateTime.Parse(model.ImportedOnTo);

                DSCharge dsCharge = null;
                BLObject<DSCharge> obj;

                obj = BLLBillingObj.PatientVisitChargesSearch(MDVUtility.ToInt64(model.ChargeCapId), model.LastName, model.FirstName, MDVUtility.ToInt64(model.FacilityId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt64(model.ResourceProviderId), model.ChargeStatus, MDVUtility.ToInt64(model.InsurancePlanId), model.ClaimNumber, model.Paid, model.Hold, dtfrom, dtto, 0, 1, model.PatientAccount, 0, MDVUtility.ToInt(model.PageNumber), MDVUtility.ToInt(model.RowsPerPage), null, ClaimType, model.ClaimErroredId, MDVUtility.ToInt(model.SubmitStatus), ClaimCreatedBy, ClaimCreatedFrom, ClaimCreatedTo, EncounterSignOffDate, ImportedDateFrom, ImportedDateTo, dtSubmittedFrom, dtSubmittedTo, IncludeSecondaryClaim, IncludeVoidedClaims, MDVUtility.ToInt64(model.EncounterTypeId));

                if (obj.Data != null)
                {
                    dsCharge = obj.Data;

                    if (dsCharge.Tables[dsCharge.VisitCharges.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            BillChargeCount = dsCharge.Tables[dsCharge.VisitCharges.TableName].Rows.Count,
                            iTotalDisplayRecords = dsCharge.VisitCharges.Rows[0][dsCharge.VisitCharges.RecordCountColumn.ColumnName],
                            BillChargeLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.VisitCharges.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            BillChargeCount = 0,
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
                        BillChargeCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "CHANGE_VISITS_STATUS":
                    {
                        string VisitsIds = MDVUtility.ToStr(context.Request["VisitsIds"]);
                        string SubmitStatusId = MDVUtility.ToStr(context.Request["SubmitStatusId"]);
                        string strJSONData = ChangeVisitStatus(VisitsIds, SubmitStatusId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }

            }
        }

        #endregion
    }
}