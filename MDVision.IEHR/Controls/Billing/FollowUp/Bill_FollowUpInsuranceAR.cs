using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Model.Billing.FollowUp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Billing.FollowUp
{
    public class Bill_FollowUpInsuranceAR
    {
        private BLLBilling BLLBillingObj = null;
        public Bill_FollowUpInsuranceAR()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Bill_FollowUpInsuranceAR _obj = null;
        public static Bill_FollowUpInsuranceAR Instance()
        {
            if (_obj == null)
                _obj = new Bill_FollowUpInsuranceAR();
            return _obj;
        }
        #endregion

        #region Private Functions

        public string LoadFollowUpARDetail(FollowUpInsuranceARModel model)
        {
            string PatientAccount = "", ClaimNumber = "", LastName = "", FirstName = "", suspended = "", NameInitialFrom="", NameInitialTo="";
            Int64 facility = 0, provider = 0, groupId = 0, ActionId = 0, ReasonId = 0, InsurancePlanId = 0, Age = 0, ClaimType = 0, PlanCategory=0;
            double InsBalGreater = 0, InsBalLess = 0;
            DateTime? DOSFrom = null, DOSTo = null;
            try
            {
                DOSFrom = String.IsNullOrEmpty(model.DatePaidFrom) ? (DateTime?)null : DateTime.Parse(model.DatePaidFrom);
                DOSTo = String.IsNullOrEmpty(model.DatePaidTo) ? (DateTime?)null : DateTime.Parse(model.DatePaidTo);

                if (DOSFrom == null)
                    DOSFrom = DOSTo;
                if (DOSTo == null)
                    DOSTo = DOSFrom;


                if (!string.IsNullOrEmpty(model.GroupID))
                    groupId = MDVUtility.ToLong(model.GroupID);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.ActionID)))
                    ActionId = MDVUtility.ToLong(model.ActionID);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.Reason)))
                    ReasonId = MDVUtility.ToLong(model.Reason);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.InsurancePlan)))
                    InsurancePlanId = MDVUtility.ToLong(model.InsurancePlan);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.LastName)))
                    LastName = MDVUtility.ToStr(model.LastName);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.FirstName)))
                    FirstName = MDVUtility.ToStr(model.FirstName);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientAccount)))
                    PatientAccount = MDVUtility.ToStr(model.PatientAccount);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.Facility)))
                    facility = MDVUtility.ToInt64(model.FacilityID);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.Provider)))
                    provider = MDVUtility.ToInt64(model.ProviderID);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.ClaimNumber)))
                    ClaimNumber = MDVUtility.ToStr(model.ClaimNumber);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.Suspend)))
                    suspended = MDVUtility.ToStr(model.Suspend);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.ClaimType)))
                    ClaimType = MDVUtility.ToInt64(model.ClaimType);
                  
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.NameInitialFrom)))
                    NameInitialFrom = MDVUtility.ToStr(model.NameInitialFrom);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.NameInitialTo)))
                    NameInitialTo = MDVUtility.ToStr(model.NameInitialTo);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.InsBalGreater)))
                    InsBalGreater = MDVUtility.ToDouble(model.InsBalGreater);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.InsBalLess)))
                    InsBalLess = MDVUtility.ToDouble(model.InsBalLess);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.PlanCategory)))
                    PlanCategory = MDVUtility.ToInt64(model.PlanCategory);

                

                Age = !string.IsNullOrEmpty(model.Age) ? MDVUtility.ToLong(model.Age) : 0;


                DSFollowUp dsFollowUp = null;
                BLObject<DSFollowUp> obj = null;

                obj = BLLBillingObj.LoadFollowUpInsuranceARDetailSelect(0, 0, PatientAccount, provider, facility, ClaimNumber, DOSFrom, DOSTo, groupId, ActionId, ReasonId, InsurancePlanId, LastName, FirstName, suspended, Age, ClaimType, NameInitialTo,NameInitialFrom,InsBalGreater,InsBalLess,PlanCategory, model.ARType, MDVUtility.ToInt(model.PageNumber), MDVUtility.ToInt(model.RowsPerPage), "LoadFollowUpARDetail",model.LastModified,model.LastComment);
                dsFollowUp = obj.Data;

                if (obj.Data != null)
                {
                    if (dsFollowUp.Tables[dsFollowUp.FollowUpARDetail.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ARVisitCount = dsFollowUp.Tables[dsFollowUp.FollowUpARDetail.TableName].Rows.Count,
                            iTotalDisplayRecords = dsFollowUp.FollowUpARDetail.Rows[0][dsFollowUp.FollowUpARDetail.RecordCountColumn.ColumnName],
                            ARVisitLoad_JSON = MDVUtility.JSON_DataTable(dsFollowUp.Tables[dsFollowUp.FollowUpARDetail.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ARVisitCount = 0,
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
                        ARVisitCount = 0,
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
        public string FillFollowUpARDetail(FollowUpInsuranceARModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.FollowUpInsuranceARID)))
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

                    DSFollowUp dsFollowup = null;
                    BLObject<DSFollowUp> obj = null;


                    if (MDVUtility.ToInt64(model.FollowUpInsuranceARID) <= 0)
                        obj = BLLBillingObj.LoadFollowUpInsuranceARDetail(MDVUtility.ToInt64(model.FollowUpInsuranceARID), MDVUtility.ToInt64(model.VisitID), "", 0, 0, "", null, null, 0, 0, 0, 0, "", "", "", 0, 0, "", 1, 15, "LoadFollowUpARDetail");
                    else
                        obj = BLLBillingObj.LoadFollowUpInsuranceARDetail(MDVUtility.ToInt64(model.FollowUpInsuranceARID), MDVUtility.ToInt64(model.VisitID), "", 0, 0, "", null, null, 0, 0, 0, 0, "", "", "", 0, 0, "", 1, 15, "FillFollowUpARDetail");

                    if (obj.Data != null)
                    {
                        dsFollowup = obj.Data;
                        if (dsFollowup.Tables[dsFollowup.FollowUpARDetail.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsFollowup.Tables[dsFollowup.FollowUpARDetail.TableName].Rows[0];

                            model.FollowUpInsuranceARDetailID = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.FolUpARDtlIdColumn.ColumnName]);
                            // model.PatientID = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.PatientIdColumn.ColumnName]);
                            model.GroupID = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.ARGroupIdColumn.ColumnName]);
                            model.ActionID = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.FollowupActionIdColumn.ColumnName]);
                            model.Reason = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.FollowupReasonIdColumn.ColumnName]);
                            model.RemitanceCode = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.RemittanceIdColumn.ColumnName]);
                            model.SuspendedDate = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.SuspendDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsFollowup.FollowUpARDetail.SuspendDateColumn.ColumnName]).ToShortDateString();
                            model.Comments = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.CommentsColumn.ColumnName]);
                            /**PATIENT INFORMATION**/
                            model.PatientAccount = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.AccountNumberColumn.ColumnName]);
                            model.PatientID = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.PatientIdColumn.ColumnName]);
                            model.SSN = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.SSNColumn.ColumnName]);
                            model.LastName = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.LastNameColumn.ColumnName]);
                            model.FirstName = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.FirstNameColumn.ColumnName]);
                            model.MiddleInitial = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.MIColumn.ColumnName]);
                            model.DOB = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.DOBColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsFollowup.FollowUpARDetail.DOBColumn.ColumnName]).ToShortDateString();
                            model.Provider = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.ProviderNameColumn.ColumnName]);
                            model.Facility = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.FacilityNameColumn.ColumnName]);
                            model.FacilityID = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.FacilityIdColumn.ColumnName]);
                            model.ProviderID = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.ProviderIdColumn.ColumnName]);
                            model.Practice = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.PracticeNameColumn.ColumnName]);
                            model.PracticeID = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.PracticeIdColumn.ColumnName]);
                            model.TaxID = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.TaxIdColumn.ColumnName]);
                            model.InsurancePlan = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.InsurancePlanNameColumn.ColumnName]);
                            model.InsuranceTelephone = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.InsuranceTelePhoneColumn.ColumnName]);
                            /**INSURED INFORMATION**/
                            model.InsSSN = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.InsuredSSNColumn.ColumnName]);
                            model.InsLastName = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.InsuredLastNameColumn.ColumnName]);
                            model.InsFirstName = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.InsuredFirstNameColumn.ColumnName]);
                            model.InsMiddleInitial = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.InsuredMIColumn.ColumnName]);
                            model.SubscriberID = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.SubscriberIdColumn.ColumnName]);
                            model.InsGroupID = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.InsuredGroupIdColumn.ColumnName]);
                            model.EligibilityDate = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.EligibilityDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsFollowup.FollowUpARDetail.EligibilityDateColumn.ColumnName]).ToShortDateString();
                            model.EligibilityStatus = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.EligibilityStatusColumn.ColumnName]);
                            /**CLAIM INFORMATION**/
                            model.ClaimNumber = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.ClaimNumberColumn.ColumnName]);
                            model.ICN = MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.ICNDCNColumn.ColumnName]);
                            model.FirstSubmitDate = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.FirstSubmittedDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsFollowup.FollowUpARDetail.FirstSubmittedDateColumn.ColumnName]).ToShortDateString();
                            model.SubmitDate = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsFollowup.FollowUpARDetail.SubmittedDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsFollowup.FollowUpARDetail.SubmittedDateColumn.ColumnName]).ToShortDateString();

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                FollowUpInsuranceARDetailFill_JSON = js.Serialize(model)
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
        private string SaveFollowUpARDetail(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSFollowUp dsFollowUp = new DSFollowUp();
                DSFollowUp.FollowUpARDetailRow dr = dsFollowUp.FollowUpARDetail.NewFollowUpARDetailRow();

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlGroup"])))
                    dr.ARGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlGroup"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlAction"])))
                    dr.FollowupActionId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAction"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlReason"])))
                    dr.FollowupReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlReason"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlAction"])))
                    dr.FollowupActionId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAction"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlRemitanceCode"])))
                    dr.RemittanceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlRemitanceCode"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpSuspendedDate"])))
                    dr.SuspendDate = MDVUtility.ToStr(MDVUtility.ToDateTime(SearchedfieldsJSON["dtpSuspendedDate"]));//txtComments
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtComments"])))
                    dr.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);

                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsFollowUp.FollowUpARDetail.AddFollowUpARDetailRow(dr);
                BLObject<DSFollowUp> obj = BLLBillingObj.InsertFollowUpPatientARDetail(dsFollowUp);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        FollowUpARDetailId = dsFollowUp.Tables[dsFollowUp.FollowUpARDetail.TableName].Rows[0][dsFollowUp.FollowUpARDetail.FolUpARDtlIdColumn.ColumnName],
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
        public string UpdateFollowUpARDetail(FollowUpInsuranceARModel model)
        {
            try
            {

                if (MDVUtility.ToInt64(model.FollowUpInsuranceARID) <= 0)
                {
                    if (!string.IsNullOrEmpty(model.FollowUpInsuranceARDetailID))
                    {
                        model.FollowUpInsuranceARID = model.FollowUpInsuranceARDetailID;
                    }
                    else
                    {

                        throw new Exception("AR FollowUp Doesn't Exists");
                    }
                }

                DSFollowUp dsFollowUp = null;
                BLObject<DSFollowUp> obj1 = BLLBillingObj.LoadFollowUpInsuranceARDetail(MDVUtility.ToInt64(model.FollowUpInsuranceARID), 0, "", 0, 0, "", null, null, 0, 0, 0, 0, "", "", "", 0, 0, "", 1, 15);
                if (obj1.Data != null)
                {
                    dsFollowUp = obj1.Data;
                    if (MDVUtility.ToInt64(model.FollowUpInsuranceARID) > 0)
                    {
                        DSFollowUp.FollowUpARDetailRow dr = (DSFollowUp.FollowUpARDetailRow)dsFollowUp.Tables[dsFollowUp.FollowUpARDetail.TableName].Rows[0];

                        if (!string.IsNullOrEmpty(model.GroupID))
                            dr.ARGroupId = MDVUtility.ToInt64(model.GroupID);
                        else
                            dr[dsFollowUp.FollowUpARDetail.ARGroupIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.GroupName))
                            dr.ARGroupName = model.GroupName;
                        else
                            dr[dsFollowUp.FollowUpARDetail.ARGroupNameColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.ActionID))
                            dr.FollowupActionId = MDVUtility.ToLong(model.ActionID);

                        if (!string.IsNullOrEmpty(model.ActionName))
                            dr.FollowupActionName = model.ActionName;

                        if (!string.IsNullOrEmpty(model.Reason))
                            dr.FollowupReasonId = MDVUtility.ToInt32(model.Reason);
                        else
                            dr[dsFollowUp.FollowUpARDetail.FollowupReasonIdColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.ReasonName))
                            dr.FollowupReasonName = model.ReasonName;
                        else
                            dr[dsFollowUp.FollowUpARDetail.FollowupReasonNameColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.RemitanceCode))
                            dr.RemittanceId = MDVUtility.ToInt32(model.RemitanceCode);
                        else
                            dr[dsFollowUp.FollowUpARDetail.RemittanceIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.RemitanceCodeText))
                            dr.RemittanceCode = model.RemitanceCodeText;
                        else
                            dr[dsFollowUp.FollowUpARDetail.RemittanceCodeColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.SuspendedDate))
                            dr.SuspendDate = MDVUtility.ToDateTime(model.SuspendedDate);

                        if (model.hfIsCommnetsChanged.ToString()=="true")
                        {
                            dr.UpdateCommentsDate = DateTime.Now;
                            dr.Comments= model.Comments.Trim();
                        }
                        if (!string.IsNullOrEmpty(model.Comments))
                        {
                            dr.Comments = model.Comments.Trim();
                        }
                        else
                        {
                            dr[dsFollowUp.FollowUpARDetail.CommentsColumn] = DBNull.Value;
                        }
                      //  dr.Comments = model.Comments.Trim();

                        dr.IsActive = true;

                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        #region Database Updation

                        if (dsFollowUp.Tables[dsFollowUp.FollowUpARDetail.TableName].Rows.Count > 0)
                        {
                            BLObject<DSFollowUp> obj = BLLBillingObj.UpdateFollowUpInsuranceARDetail(dsFollowUp);
                            if (obj.Data != null)
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
                        #endregion
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "followup not found."
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj1.Message
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

        #endregion

    }

    //


}