using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Patient.Insurance
{
    public class Patient_Referral
    {
        #region Singleton
        private static Patient_Referral _obj = null;
        private  BLLPatient BLLPatientObj = null;
        public Patient_Referral()
        {

            BLLPatientObj = new BLLPatient();
        }
        public static Patient_Referral Instance()
        {
            if (_obj == null)
                _obj = new Patient_Referral();
            return _obj;
        }
        #endregion


      
        #region Private Functions
        /// <summary>
        /// Loads the patient referral.
        /// </summary>
        /// <param name="PatientReferralID">The patient referral identifier.</param>
        /// <returns></returns>
        public string LoadPatientReferral(Int64 PatientInsuranceID,string IsFromAppointment, int pageNumber, int rowsPerPage)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj;
                obj = BLLPatientObj.searchPatientReferrals(PatientInsuranceID, IsFromAppointment, pageNumber, rowsPerPage);
                dsPatient = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ReferralCount = dsPatient.Tables[dsPatient.PatientReferralsSearch.TableName].Rows.Count > 0 ? dsPatient.Tables[dsPatient.PatientReferralsSearch.TableName].Rows.Count : 0,
                        ReferralLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientReferralsSearch.TableName]),
                        iTotalDisplayRecords = (dsPatient.PatientReferralsSearch.Rows.Count > 0) ? dsPatient.PatientReferralsSearch.Rows[0][dsPatient.PatientReferralsSearch.RecordCountColumn.ColumnName] : 0,
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ReferralCount = 0,
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

        /// <summary>
        /// Saves the patient referral.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PatientInsuranceId">The patient insurance identifier.</param>
        /// <returns>System.String.</returns>
        private string SavePatientReferral(string fieldsJSON, Int32 PatientInsuranceId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatient dsPatient = new DSPatient();
                DSPatient.PatientReferralsRow dr = dsPatient.PatientReferrals.NewPatientReferralsRow();


                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlInsurancePlan"]))
                    dr.InsurancePlanId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlInsurancePlan"]);

                dr.PatientInsuranceId = PatientInsuranceId;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientID"]))
                    dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientID"]);
                if (SearchedfieldsJSON["ddlReferralType"] == "Incoming")
                {
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfIncomingRefProviderFrom"]))
                        dr.ReferringFromId = MDVUtility.ToInt64(SearchedfieldsJSON["hfIncomingRefProviderFrom"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfIncomingfRefProviderTo"]))
                        dr.ReferringToId = MDVUtility.ToInt64(SearchedfieldsJSON["hfIncomingfRefProviderTo"]);
                    else
                        dr[dsPatient.PatientReferrals.ReferringToIdColumn] = DBNull.Value;
                }
                else
                {
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfOutgoingRefProviderFrom"]))
                        dr.ReferringFromId = MDVUtility.ToInt64(SearchedfieldsJSON["hfOutgoingRefProviderFrom"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfOutgoingRefProviderTo"]))
                        dr.ReferringToId = MDVUtility.ToInt64(SearchedfieldsJSON["hfOutgoingRefProviderTo"]);
                    else
                        dr[dsPatient.PatientReferrals.ReferringToIdColumn] = DBNull.Value;
                }
                //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReferringFrom"]))
                //    dr.ReferringFromId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlReferringFrom"]);
                //else
                //    dr.ReferringFromId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlOutgoingReferringFrom"]);
                //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReferringTo"]))
                //    dr.ReferringToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlReferringTo"]);
                //else
                //    dr.ReferringToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlOutgoingReferringTo"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReferralType"]))
                    dr.ReferralType = MDVUtility.ToStr(SearchedfieldsJSON["ddlReferralType"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFromFacility"]))
                    dr.FacilityFromId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFromFacility"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfToFacility"]))
                    dr.FacilityToId = MDVUtility.ToInt64(SearchedfieldsJSON["hfToFacility"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpFromDate"]))
                    dr.FromDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpFromDate"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpToDate"]))
                    dr.ToDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpToDate"]);
                dr.VisitsAllowed = MDVUtility.ToStr(SearchedfieldsJSON["txtVisitsAllowed"]);
                dr.VisitsUsed = MDVUtility.ToStr(SearchedfieldsJSON["txtVisitsUsed"]);
                dr.ReferralAuthNo = MDVUtility.ToStr(SearchedfieldsJSON["txtReferralAuthNo"]);
                dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsPatient.PatientReferrals.AddPatientReferralsRow(dr);
                BLObject<DSPatient> obj = BLLPatientObj.InsertPatientReferral(dsPatient);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        PatientReferralId = dsPatient.Tables[dsPatient.PatientReferrals.TableName].Rows[0][dsPatient.PatientReferrals.PatientReferralIdColumn.ColumnName]
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
        /// Updates the patient referral.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PatientInsuranceId">The patient insurance identifier.</param>
        /// <param name="PatientReferralID">The patient referral identifier.</param>
        /// <returns>System.String.</returns>
        private string UpdatePatientReferral(string fieldsJSON, Int64 PatientReferralID)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (PatientReferralID > 0)
                {

                    DSPatient dsPatient = new DSPatient();
                    //DSPatient.PatientReferralsRow dr = dsPatient.PatientReferrals.NewPatientReferralsRow();
                    BLObject<DSPatient> objLoad = BLLPatientObj.LoadPatientReferral(0, PatientReferralID);
                    dsPatient = objLoad.Data;

                    foreach (DSPatient.PatientReferralsRow dr in dsPatient.Tables[dsPatient.PatientReferrals.TableName].Rows)
                    {
                        //dr.PatientReferralId = PatientReferralID;

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlInsurancePlan"]))
                            dr.PatientInsuranceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlInsurancePlan"]);
                            
                        if (SearchedfieldsJSON["ddlReferralType"] == "Incoming")
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReferringFrom"]))
                                dr.ReferringFromId = MDVUtility.ToInt64(SearchedfieldsJSON["hfIncomingRefProviderFrom"]);
                            else
                                dr[dsPatient.PatientReferrals.ReferringFromIdColumn] = 0;

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReferringTo"]))
                                dr.ReferringToId = MDVUtility.ToInt64(SearchedfieldsJSON["hfIncomingfRefProviderTo"]);
                            else
                                dr[dsPatient.PatientReferrals.ReferringToIdColumn] = 0;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlOutgoingReferringFrom"]))
                            {
                                dr.ReferringFromId = MDVUtility.ToInt64(SearchedfieldsJSON["hfOutgoingRefProviderFrom"]);
                            }
                            else {
                                dr[dsPatient.PatientReferrals.ReferringFromIdColumn] = 0;
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlOutgoingReferringTo"]))
                                dr.ReferringToId = MDVUtility.ToInt64(SearchedfieldsJSON["hfOutgoingRefProviderTo"]);
                            else
                                dr[dsPatient.PatientReferrals.ReferringToIdColumn] = 0;
                        }

                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReferringFrom"]))
                        //    dr.ReferringFromId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlReferringFrom"]);
                        ////if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReferringTo"]))
                        //dr.ReferringToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlReferringTo"]);
                                          

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlReferralType"]))
                            dr.ReferralType = MDVUtility.ToStr(SearchedfieldsJSON["ddlReferralType"]);
                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFacilityFrom"]))
                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFromFacility"])) {
                        //    dr.FacilityFromId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFromFacility"]);
                        //}                                  

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFacilityTo"]))
                        {
                            dr.FacilityToId = MDVUtility.ToInt64(SearchedfieldsJSON["hfToFacility"]);
                        }
                        else
                        {
                            dr[dsPatient.PatientReferrals.FacilityToIdColumn] = 0;
                        }


                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFacilityFrom"]))
                        {
                            dr.FacilityFromId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFromFacility"]);
                        }
                        else {
                            dr[dsPatient.PatientReferrals.FacilityFromIdColumn] = 0;
                        }

                        dr.FromDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpFromDate"]);
                        dr.ToDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpToDate"]);                       
                        dr.VisitsAllowed = MDVUtility.ToStr(SearchedfieldsJSON["txtVisitsAllowed"]);
                        dr.VisitsUsed = MDVUtility.ToStr(SearchedfieldsJSON["txtVisitsUsed"]);
                        dr.ReferralAuthNo = MDVUtility.ToStr(SearchedfieldsJSON["txtReferralAuthNo"]);
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsPatient.PatientReferrals.AddPatientReferralsRow(dr);
                    //dsPatient.PatientReferrals.AcceptChanges();

                    if (dsPatient.Tables[dsPatient.PatientReferrals.TableName].Rows.Count > 0)
                    {
                        //dsPatient.PatientReferrals.Rows[0].SetModified();
                        BLObject<DSPatient> obj = BLLPatientObj.UpdatePatientReferral(dsPatient);
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
                        Message = "Referral not found."
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
        /// Deletes the patient referral.
        /// </summary>
        /// <param name="PatientReferralID">The patient referral identifier.</param>
        /// <returns>System.String.</returns>
        private string DeletePatientReferral(Int64 PatientReferralID)
        {

            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientReferralID)))
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
                    BLObject<string> obj = BLLPatientObj.DeletePatientReferral(MDVUtility.ToStr(PatientReferralID));
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

        /// <summary>
        /// Fills the patient referral.
        /// </summary>
        /// <param name="PatientinsuranceId">The patientinsurance identifier.</param>
        /// <param name="PatientReferralId">The patient referral identifier.</param>
        /// <returns></returns>
        private string FillPatientReferral(Int64 PatientReferralId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientReferralId)))
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
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.LoadPatientReferral(0, PatientReferralId);
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;
                        if (dsPatient.Tables[dsPatient.PatientReferrals.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatient.Tables[dsPatient.PatientReferrals.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {

                            { "ddlReferringFrom", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.ReferringFromNameColumn.ColumnName])},
                            { "hfIncomingRefProviderFrom", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.ReferringFromIdColumn.ColumnName])},

                            { "ddlReferringTo", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.ReferringToNameColumn.ColumnName])},
                            { "hfIncomingfRefProviderTo", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.ReferringToIdColumn.ColumnName])},

                            { "ddlOutgoingReferringFrom", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.ReferringFromNameColumn.ColumnName])},
                            { "hfOutgoingRefProviderFrom", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.ReferringFromIdColumn.ColumnName])},

                            { "ddlOutgoingReferringTo", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.ReferringToNameColumn.ColumnName])},
                            { "hfOutgoingRefProviderTo", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.ReferringToIdColumn.ColumnName])},

                            { "ddlFacilityFrom", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.FacilityFromNameColumn.ColumnName])},
                            { "hfFromFacility", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.FacilityFromIdColumn.ColumnName])},

                            { "ddlFacilityTo",MDVUtility.ToStr(dr[dsPatient.PatientReferrals.FacilityToNameColumn.ColumnName])},
                            { "hfToFacility",MDVUtility.ToStr(dr[dsPatient.PatientReferrals.FacilityToIdColumn.ColumnName])},

                            { "ddlInsurancePlan", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.PatientInsuranceIdColumn.ColumnName])},
                            { "ddlReferralType", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.ReferralTypeColumn.ColumnName])},
                            { "dtpFromDate",string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.PatientReferrals.FromDateColumn.ColumnName]))?"":MDVUtility.ToDateTime(dr[dsPatient.PatientReferrals.FromDateColumn.ColumnName]).ToShortDateString()},
                            { "dtpToDate",string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.PatientReferrals.ToDateColumn.ColumnName]))?"":MDVUtility.ToDateTime(dr[dsPatient.PatientReferrals.ToDateColumn.ColumnName]).ToShortDateString()},
                           // { "dtpFromDate",  MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsPatient.PatientReferrals.FromDateColumn.ColumnName]))},
                         //   { "dtpToDate",  MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsPatient.PatientReferrals.ToDateColumn.ColumnName]))},
                            //{ "dtpToDate", MDVUtility.ToDateTime(dr[dsPatient.PatientReferrals.ToDateColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "txtVisitsAllowed", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.VisitsAllowedColumn.ColumnName])},
                            { "txtVisitsUsed", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.VisitsUsedColumn.ColumnName])},
                            { "txtReferralAuthNo", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.ReferralAuthNoColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsPatient.PatientReferrals.IsActiveColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ReferralFill_JSON = js.Serialize(keyValues)
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

        /// <summary>
        /// Updates the patient referral is active.
        /// </summary>
        /// <param name="ReferralId">The referral identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        private string UpdatePatientReferralIsActive(Int64 ReferralId, Int64 IsActive)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj = BLLPatientObj.LoadPatientReferral(0, ReferralId);
                dsPatient = obj.Data;
                if (dsPatient.Tables[dsPatient.PatientReferrals.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsPatient.Tables[dsPatient.PatientReferrals.TableName].Rows[0];
                    dr[dsPatient.PatientReferrals.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSPatient> objReferral = BLLPatientObj.UpdatePatientReferral(dsPatient);
                    string successMsg;
                    if (objReferral.Data != null)
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
                            Message = objReferral.Message
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
                case "SEARCH_REFERRAL":
                    {
                        Int64 PatientInsuranceID = MDVUtility.ToInt64(context.Request["PatientInsuranceID"]);

                        string IsFromAppointment = "";
                        if (!string.IsNullOrEmpty(context.Request["IsFromAppointment"]))
                        {
                            IsFromAppointment = context.Request["IsFromAppointment"];
                        }

                        //long FacilityToId = 0;
                        //if (!string.IsNullOrEmpty(context.Request["FacilityToId"]))
                        //{
                        //    FacilityToId = MDVUtility.ToInt64(context.Request["FacilityToId"]);
                        //}

                        //long ReferringToId = 0;
                        //if (!string.IsNullOrEmpty(context.Request["ReferringToId"]))
                        //{
                        //    ReferringToId = MDVUtility.ToInt64(context.Request["ReferringToId"]);
                        //}

                        //DateTime? ReferringDate = null;
                        //if (!string.IsNullOrEmpty(context.Request["ReferringDate"]))
                        //{
                        //    ReferringDate = MDVUtility.ToDateTime(context.Request["ReferringDate"]);
                        //}

                        Int32 pageNumber = 0;
                        if (!string.IsNullOrEmpty(context.Request["pageNumber"]))
                        {
                            pageNumber = MDVUtility.ToInt32(context.Request["pageNumber"]);
                        }
                        Int32 rowsPerPage = 0;
                        if (!string.IsNullOrEmpty(context.Request["rowsPerPage"]))
                        {
                            rowsPerPage = MDVUtility.ToInt32(context.Request["rowsPerPage"]);
                        }

                        string strJSONData = LoadPatientReferral(PatientInsuranceID, IsFromAppointment,pageNumber, rowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_REFERRAL":
                    {
                        string fieldsJSON = context.Request["ReferralData"];
                        Int32 PatientInsuranceId = MDVUtility.ToInt32(context.Request["PatientInsuranceId"]);                                              
                        string strJSONData = SavePatientReferral(fieldsJSON, PatientInsuranceId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_REFERRAL":
                    {
                        Int64 PatientReferralID = MDVUtility.ToInt64(context.Request["PatientReferralID"]);
                        string strJSONData = FillPatientReferral(PatientReferralID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_REFERRAL":
                    {
                        Int64 PatientReferralID = MDVUtility.ToInt64(context.Request["ReferralID"]);                        
                        string strJSONData = DeletePatientReferral(PatientReferralID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_REFERRAL":
                    {
                        string fieldsJSON = context.Request["ReferralData"];
                        Int64 PatientReferralID = MDVUtility.ToInt64(context.Request["PatientReferralID"]);                       
                        string strJSONData = UpdatePatientReferral(fieldsJSON, PatientReferralID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_REFERRAL_ACTIVE_INACTIVE":
                    {
                        Int64 PatientReferralID = MDVUtility.ToInt64(context.Request["ReferralID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdatePatientReferralIsActive(PatientReferralID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}