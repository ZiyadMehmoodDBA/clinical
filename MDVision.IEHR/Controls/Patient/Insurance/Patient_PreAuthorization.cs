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
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Patient.Insurance
{
    public class Patient_PreAuthorization
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_PreAuthorization()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_PreAuthorization _obj = null;
        public static Patient_PreAuthorization Instance()
        {
            if (_obj == null)
                _obj = new Patient_PreAuthorization();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Saves the PreAuthorization.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string SavePreAuthorization(string fieldsJSON, Int64 PatientId)
        {
            try
            {
                String cptCodeDesc = "";
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (PatientId > 0)
                {
                    DSPatient dsPatient = new DSPatient();
                    DSPatient.AuthorizationsRow dr = dsPatient.Authorizations.NewAuthorizationsRow();

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlan"]))
                        dr.InsurancePlanId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPlan"]);
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCPTCode"]))
                    //    dr.CPTCodeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfcptcode"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCPTCode"]))
                    {
                        cptCodeDesc = Convert.ToString(SearchedfieldsJSON["hfcptcode"]);
                        String[] cptCodeDescSplit = cptCodeDesc.Split(new Char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        if (cptCodeDescSplit.Length > 0)
                            dr.CPTCode = cptCodeDescSplit[0];
                        if (cptCodeDescSplit.Length > 1)
                            dr.Description = cptCodeDescSplit[1];
                    }
                    dr.PatientId = MDVUtility.ToInt64(PatientId);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpFromDate"]))
                        dr.FromDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpFromDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpToDate"]))
                        dr.ToDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpToDate"]);
                    if (SearchedfieldsJSON.ContainsKey("txtVisitsAllowed") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtVisitsAllowed"]))
                        dr.VisitsAllowed = MDVUtility.ToInt32(SearchedfieldsJSON["txtVisitsAllowed"]);
                    else
                        dr[dsPatient.Authorizations.VisitsAllowedColumn] = DBNull.Value;

                    if (SearchedfieldsJSON.ContainsKey("txtVisitsUsed") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtVisitsUsed"]))
                        dr.VisitsUsed = MDVUtility.ToInt32(SearchedfieldsJSON["txtVisitsUsed"]);
                    else
                        dr[dsPatient.Authorizations.VisitsUsedColumn] = DBNull.Value;
                    dr.PAN = SearchedfieldsJSON["txtPAN"];
                    dr.Comments = SearchedfieldsJSON["txtComments"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsPatient.Authorizations.AddAuthorizationsRow(dr);
                    BLObject<DSPatient> obj = BLLPatientObj.InsertPatientAuthorization(dsPatient);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            PatientFamilyId = dsPatient.Tables[dsPatient.Authorizations.TableName].Rows[0][dsPatient.Authorizations.AuthorizeIdColumn.ColumnName]
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
                else
                {
                    throw new Exception("Patient not found.");
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
        /// Updates the PreAuthorization.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="FamilyID">The PreAuthorization identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string UpdatePreAuthorization(string fieldsJSON, Int64 PatientId, Int64 AuthorizationID)
        {
            try
            {
                String cptCodeDesc = "";
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (AuthorizationID > 0)
                {

                    DSPatient dsPatient = new DSPatient();
                    //DSPatient.AuthorizationsRow dr = dsPatient.Authorizations.NewAuthorizationsRow();
                    BLObject<DSPatient> objLoad = BLLPatientObj.LoadPatientAuthorization(PatientId, AuthorizationID);
                    dsPatient = objLoad.Data;

                    foreach (DSPatient.AuthorizationsRow dr in dsPatient.Tables[dsPatient.Authorizations.TableName].Rows)
                    {
                        //dr.AuthorizeId = AuthorizationID;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlan"]))
                            dr.InsurancePlanId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPlan"]);

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCPTCode"]))
                        {
                            cptCodeDesc = Convert.ToString(SearchedfieldsJSON["hfcptcode"]);
                            String[] cptCodeDescSplit = cptCodeDesc.Split(new Char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                            if (cptCodeDescSplit.Length > 0)
                                dr.CPTCode = cptCodeDescSplit[0];
                            if (cptCodeDescSplit.Length > 1)
                                dr.Description = cptCodeDescSplit[1];
                        }
                        dr.PatientId = MDVUtility.ToInt64(PatientId);
                        if (SearchedfieldsJSON["dtpFromDate"] == "")
                            dr[dsPatient.Authorizations.FromDateColumn] = DBNull.Value;
                        else
                            dr.FromDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpFromDate"]);

                        if (SearchedfieldsJSON["dtpToDate"] == "")
                            dr[dsPatient.Authorizations.ToDateColumn] = DBNull.Value;
                        else
                            dr.ToDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpToDate"]);
                        if (SearchedfieldsJSON.ContainsKey("txtVisitsAllowed") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtVisitsAllowed"]))
                            dr.VisitsAllowed = MDVUtility.ToInt32(SearchedfieldsJSON["txtVisitsAllowed"]);
                        else
                            dr[dsPatient.Authorizations.VisitsAllowedColumn] = DBNull.Value;

                        if (SearchedfieldsJSON.ContainsKey("txtVisitsUsed") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtVisitsUsed"]))
                            dr.VisitsUsed = MDVUtility.ToInt32(SearchedfieldsJSON["txtVisitsUsed"]);
                        else
                            dr[dsPatient.Authorizations.VisitsUsedColumn] = DBNull.Value;
                        dr.PAN = SearchedfieldsJSON["txtPAN"];
                        dr.Comments = SearchedfieldsJSON["txtComments"];
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsPatient.Authorizations.AddAuthorizationsRow(dr);
                    //dsPatient.Authorizations.AcceptChanges();

                    if (dsPatient.Tables[dsPatient.Authorizations.TableName].Rows.Count > 0)
                    {
                        //dsPatient.Authorizations.Rows[0].SetModified();
                        BLObject<DSPatient> obj = BLLPatientObj.UpdatePatientAuthorization(dsPatient);
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
                        Message = "PreAuthorization not found."
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
            //return "";
        }

        /// <summary>
        /// Updates the PreAuthorization IsActive.
        /// </summary>
        /// <param name="PatientId">The Patient identifier.</param>
        /// <param name="FamilyID">The PreAuthorization identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string UpdatePreAuthorizationIsActive(Int64 PatientId, Int64 AuthorizationID, Int64 IsActive)
        {
            try
            {
                if (AuthorizationID > 0)
                {

                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.LoadPatientAuthorization(PatientId, AuthorizationID);
                    dsPatient = obj.Data;
                    if (dsPatient.Tables[dsPatient.Authorizations.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPatient.Tables[dsPatient.Authorizations.TableName].Rows[0];
                        dr[dsPatient.Authorizations.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPatient> objAuthorization = BLLPatientObj.UpdatePatientAuthorization(dsPatient);
                        string successMsg;
                        if (objAuthorization.Data != null)
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
                                Message = objAuthorization.Message
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
                        Message = "PreAuthorization not found."
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
        /// Loads the Patient PreAuthorizations.
        /// </summary>
        /// <param name="PatientId">The Patient identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string LoadPatientPreAuthorizations(long PatientID, long patientInsuranceId = 0, string CPTCode = null, DateTime? DOSFrom = null, DateTime? DOSTo = null)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
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
                    BLObject<DSPatient> obj = BLLPatientObj.LoadPatientAuthorization(PatientID, 0, patientInsuranceId, CPTCode, DOSFrom, DOSTo);
                    dsPatient = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            PatientPreAuthorizationCount = dsPatient.Tables[dsPatient.Authorizations.TableName].Rows.Count > 0 ? dsPatient.Tables[dsPatient.Authorizations.TableName].Rows.Count : 0,
                            PatientPreAuthorizationLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Authorizations.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientPreAuthorizationCount = 0,
                            Message = obj.Message
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
        /// Fills the PreAuthorization.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string FillPatientPreAuthorization(long PatientID, long AuthorizationID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AuthorizationID)))
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
                    BLObject<DSPatient> obj = BLLPatientObj.LoadPatientAuthorization(PatientID, AuthorizationID);
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;
                        if (dsPatient.Tables[dsPatient.Authorizations.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatient.Tables[dsPatient.Authorizations.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {

                            { "ddlPlan", MDVUtility.ToStr(dr[dsPatient.Authorizations.InsurancePlanIdColumn.ColumnName])},
                            { "hfcptcode", MDVUtility.ToStr(dr[dsPatient.Authorizations.CPTCodeColumn.ColumnName])},
                            { "txtCPTCode", MDVUtility.ToStr(dr[dsPatient.Authorizations.CPTCodeColumn.ColumnName])},
                            { "txtPAN", MDVUtility.ToStr(dr[dsPatient.Authorizations.PANColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsPatient.Authorizations.IsActiveColumn.ColumnName])},
                            { "dtpFromDate",  MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsPatient.Authorizations.FromDateColumn.ColumnName]))},
                            { "dtpToDate",  MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsPatient.Authorizations.ToDateColumn.ColumnName]))},
                            { "txtVisitsAllowed", MDVUtility.ToStr(dr[dsPatient.Authorizations.VisitsAllowedColumn.ColumnName])},
                            { "txtVisitsUsed", MDVUtility.ToStr(dr[dsPatient.Authorizations.VisitsUsedColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsPatient.Authorizations.CommentsColumn.ColumnName])},
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PatientAuthorizationFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the PreAuthorization.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string DeletePreAuthorization(long AuthorizationID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AuthorizationID)))
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
                    BLObject<string> obj = BLLPatientObj.DeletePatientAuthorization(MDVUtility.ToStr(AuthorizationID));
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

        #endregion

        #region Service Command Handler
        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            string privilegesMessage = string.Empty;
            switch (cammandAction)
            {
                case "SAVE_PREAUTHORIZATION":
                    {
                        string fieldsJSON = context.Request["PreAuthorizationData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = SavePreAuthorization(fieldsJSON, PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_PREAUTHORIZATION":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);

                        Int64 patientInsuranceId = MDVUtility.ToInt64(context.Request["patientInsuranceId"]);
                        string CPTCode = context.Request["CPTCode"];

                        DateTime? DOSFrom = null;
                        DateTime? DOSTo = null;

                        if (context.Request["DOSFrom"] != null)
                            DOSFrom = MDVUtility.ToDateTime(context.Request["DOSFrom"]);

                        if (context.Request["DOSTo"] != null)
                            DOSTo = MDVUtility.ToDateTime(context.Request["DOSTo"]);

                        //DateTime? DOSTo = MDVUtility.ToDateTime(context.Request["DOSTo"]);

                        string strJSONData = "";

                        if (patientInsuranceId > 0 && CPTCode != null && DOSFrom != null && DOSTo != null)
                        {
                            strJSONData = LoadPatientPreAuthorizations(PatientID, patientInsuranceId, CPTCode, DOSFrom, DOSTo);
                        }
                        else
                        {

                            strJSONData = LoadPatientPreAuthorizations(PatientID);
                        }
                        if (strJSONData == "")
                        {
                            var response = new
                            {
                                status = false,
                                Message = ""
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PREAUTHORIZATION":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 PreAuthorizationID = MDVUtility.ToInt64(context.Request["PreAuthorizationID"]);
                        string strJSONData = FillPatientPreAuthorization(PatientID, PreAuthorizationID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PREAUTHORIZATION":
                    {
                        Int64 PreAuthorizationID = MDVUtility.ToInt64(context.Request["PreAuthorizationID"]);
                        string strJSONData = DeletePreAuthorization(PreAuthorizationID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PREAUTHORIZATION":
                    {
                        string fieldsJSON = context.Request["PreAuthorizationData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 PreAuthorizationID = MDVUtility.ToInt64(context.Request["PreAuthorizationID"]);
                        string strJSONData = UpdatePreAuthorization(fieldsJSON, PatientID, PreAuthorizationID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PREAUTHORIZATION_ACTIVE_INACTIVE":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 PreAuthorizationID = MDVUtility.ToInt64(context.Request["PreAuthorizationID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdatePreAuthorizationIsActive(PatientID, PreAuthorizationID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_CPT_PREAUTHORIZATION":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Authorization", "VIEW")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            Int64 patientInsuranceId = MDVUtility.ToInt64(context.Request["patientInsuranceId"]);
                            string CPTCode = context.Request["CPTCode"];
                            DateTime? DOSFrom = null;
                            DateTime? DOSTo = null;

                            if (context.Request["DOSFrom"] != null)
                                DOSFrom = MDVUtility.ToDateTime(context.Request["DOSFrom"]);

                            if (context.Request["DOSTo"] != null)
                                DOSTo = MDVUtility.ToDateTime(context.Request["DOSTo"]);

                            //DateTime? DOSTo = MDVUtility.ToDateTime(context.Request["DOSTo"]);

                            if (patientInsuranceId > 0 && CPTCode != null && DOSFrom != null && DOSTo != null)
                            {
                                strJSONData = LoadPatientPreAuthorizations(PatientID, patientInsuranceId, CPTCode, DOSFrom, DOSTo);
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = ""
                                };
                                strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion
    }
}