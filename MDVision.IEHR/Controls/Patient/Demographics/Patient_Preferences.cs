using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Patient.Demographics
{
    public class Patient_Preferences
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_Preferences()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_Preferences _obj = null;
        public static Patient_Preferences Instance()
        {
            if (_obj == null)
                _obj = new Patient_Preferences();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Updates the patient preferences.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns></returns>
        private string UpdatePatientPreferences(string fieldsJSON, Int64 PatientId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                //Int32 Pref1stCommunicationId = 0;
                //Int32 Pref2ndCommunicationId = 0;
                //string SchoolStatus = "";
                //Int64 SchoolId = 0;
                //string PatientStatement = "";
                //string CommunicatewithGuarantor = "";
                //string CommunicationOptout = "";
                //string CauseOfDeath = "";

                // BLObject<DSCharge> obj = BLLPatientObj
                DSPatient dsPatient = null;

                if (PatientId > 0)
                {
                    BLObject<DSPatient> obj = BLLPatientObj.FillPatientById(PatientId, null);
                    dsPatient = obj.Data;


                    foreach (DSPatient.PatientsRow drPatient in dsPatient.Tables[dsPatient.Patients.TableName].Rows)
                    {

                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddl1stPreference"]))
                        {
                            drPatient.PrefCommunicationId = MDVUtility.ToInt32(SearchedfieldsJSON["ddl1stPreference"]);
                        }
                        else
                        {
                            drPatient.PrefCommunicationId = 0;

                        }


                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddl2ndPreference"]))
                        {
                            drPatient.ScndPrefCommunicationId = MDVUtility.ToInt32(SearchedfieldsJSON["ddl2ndPreference"]);
                        }
                        else
                        {
                            drPatient.ScndPrefCommunicationId = 0;
                        }

                        //-------
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSchoolStatus"]))
                        {
                            drPatient.SchoolStatus = SearchedfieldsJSON["ddlSchoolStatus_text"];
                        }
                        else
                        {
                            drPatient.SchoolStatus = "";
                        }

                        //--------
                        //DateTime? DOD = (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDateOfDeath"])) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dtpDateOfDeath"]);

                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDateOfDeath"]))
                        //{
                        //    drPatient.DOD = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDateOfDeath"]);
                        //}



                        //--------
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSchool"]) && SearchedfieldsJSON["hfSchool"] != "-1")
                            drPatient.SchoolId = MDVUtility.ToInt64(SearchedfieldsJSON["hfSchool"]);
                        else
                            drPatient[dsPatient.Patients.SchoolIdColumn] = DBNull.Value;

                        //--------
                        // CauseOfDeathColumn
                        //drPatient.CauseOfDeath = SearchedfieldsJSON["txtCauseOfDeath"];

                        drPatient.PatientStatement = MDVUtility.ToBool(SearchedfieldsJSON["chkPatientStatement"]) == true ? true : false;
                        drPatient.CommunicatewithGuarantor = MDVUtility.ToBool(SearchedfieldsJSON["chkcommnwithgrntr"]) == true ? true : false;
                        drPatient.CommunicationOptout = MDVUtility.ToBool(SearchedfieldsJSON["chkcommnoptout"]) == true ? true : false;
                    }


                    BLObject<DSPatient> objPref = BLLPatientObj.UpdatePatientPreferences(dsPatient);
                    if (objPref.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objPref.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }


                    //DSPatient dsPatient = null;
                    //BLObject<DSPatient> obj = BLLPatientObj.FillPatientById(PatientId, "temp");
                    //dsPatient = obj.Data;
                    //if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    //{
                    //    DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];
                    //    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPreferredComm"]))
                    //        dr[dsPatient.Patients.PrefCommunicationIdColumn.ColumnName] = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPreferredComm"]);
                    //    else
                    //        dr[dsPatient.Patients.PrefCommunicationIdColumn.ColumnName] = DBNull.Value;
                    //    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSchoolStatus"]))
                    //        dr[dsPatient.Patients.SchoolStatusColumn.ColumnName] = SearchedfieldsJSON["ddlSchoolStatus_text"];
                    //    else
                    //        dr[dsPatient.Patients.SchoolStatusColumn.ColumnName] = DBNull.Value;
                    //    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDateOfDeath"]))
                    //        dr[dsPatient.Patients.DODColumn.ColumnName] = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDateOfDeath"]);
                    //    else
                    //        dr[dsPatient.Patients.DODColumn.ColumnName] = DBNull.Value;
                    //    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSchool"]) && SearchedfieldsJSON["hfSchool"] != "-1")
                    //        dr[dsPatient.Patients.SchoolIdColumn.ColumnName] = MDVUtility.ToInt64(SearchedfieldsJSON["hfSchool"]);
                    //    else
                    //        dr[dsPatient.Patients.SchoolIdColumn.ColumnName] = DBNull.Value;
                    //    dr[dsPatient.Patients.CauseOfDeathColumn.ColumnName] = SearchedfieldsJSON["txtCauseOfDeath"];
                    //    dr[dsPatient.Patients.PatientStatementColumn.ColumnName] = MDVUtility.ToStr(SearchedfieldsJSON["chkPatientStatement"]) == "True" ? true : false;

                    //    BLObject<DSPatient> objPref = BLLPatientObj.UpdatePatient(dsPatient);
                    //    if (objPref.Data != null)
                    //    {
                    //        var response = new
                    //        {
                    //            status = true,
                    //            message = Common.AppPrivileges.Save_Message
                    //        };
                    //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    //    }
                    //    else
                    //    {
                    //        var response = new
                    //        {
                    //            status = false,
                    //            Message = objPref.Message
                    //        };
                    //        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    //    }
                    //}
                    //else
                    //{
                    //    var response = new
                    //    {
                    //        status = false,
                    //        Message = obj.Message
                    //    };
                    //    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    //}


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient not found."
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

        /// <summary>
        /// Fills the patient preferences.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns></returns>
        private string FillPatientPreferences(Int64 PatientId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
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
                    DSPatient dsProfile = null;
                    BLObject<DSPatient> obj = BLLPatientObj.FillPatientById(PatientId, null);
                    if (obj.Data != null)
                    {
                        dsProfile = obj.Data;
                        if (dsProfile.Tables[dsProfile.Patients.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsProfile.Tables[dsProfile.Patients.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "ddl1stPreference", MDVUtility.ToStr(dr[dsProfile.Patients.PrefCommunicationIdColumn.ColumnName])},
                            { "ddl2ndPreference", MDVUtility.ToStr(dr[dsProfile.Patients.ScndPrefCommunicationIdColumn.ColumnName])},
                            { "ddlSchoolStatus", MDVUtility.ToStr(dr[dsProfile.Patients.SchoolStatusColumn.ColumnName])},
                            //{ "dtpDateOfDeath", MDVUtility.ToStr(dr[dsProfile.Patients.DODColumn.ColumnName])},
                            { "dtpDateOfDeath", MDVUtility.ToStr(dr[dsProfile.Patients.DODColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsProfile.Patients.DODColumn.ColumnName]).ToShortDateString():""},
                            { "hfSchool", MDVUtility.ToStr(dr[dsProfile.Patients.SchoolIdColumn.ColumnName])},
                            { "txtSchool", MDVUtility.ToStr(dr[dsProfile.Patients.SchoolNameColumn.ColumnName])},
                            { "txtCauseOfDeath", MDVUtility.ToStr(dr[dsProfile.Patients.CauseOfDeathColumn.ColumnName])},
                            { "chkPatientStatement", MDVUtility.ToStr(dr[dsProfile.Patients.PatientStatementColumn.ColumnName])},
                            { "chkcommnwithgrntr", MDVUtility.ToStr(dr[dsProfile.Patients.CommunicatewithGuarantorColumn.ColumnName])},
                            { "chkcommnoptout", MDVUtility.ToStr(dr[dsProfile.Patients.CommunicationOptoutColumn.ColumnName])},
                            { "guarantorNumber", MDVUtility.ToStr(dr[dsProfile.Patients.GuarantorPhoneNumberColumn.ColumnName])},
                            { "guarantorRelationText", MDVUtility.ToStr(dr[dsProfile.Patients.GuarantorRelationTextColumn.ColumnName])},
                            { "patientCellNo", MDVUtility.ToStr(dr[dsProfile.Patients.CellNoColumn.ColumnName])},
                            { "patientHomeNo", MDVUtility.ToStr(dr[dsProfile.Patients.HomePhoneNoColumn.ColumnName])},
                            { "patientWorkNo", MDVUtility.ToStr(dr[dsProfile.Patients.WorkPhoneNoColumn.ColumnName])},
                            { "patientGuarantorId", MDVUtility.ToStr(dr[dsProfile.Patients.GuarantorIdColumn.ColumnName])},
                            { "PatientFName", MDVUtility.ToStr(dr[dsProfile.Patients.FirstNameColumn.ColumnName])},
                            { "PatientLName", MDVUtility.ToStr(dr[dsProfile.Patients.LastNameColumn.ColumnName])},
                            { "EmailAddress", MDVUtility.ToStr(dr[dsProfile.Patients.EmailAddressColumn.ColumnName])},

                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PreferencesFill_JSON = js.Serialize(keyValues)
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Patient Preferences Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "UPDATE_PATIENT_PREFERENCES":
                    {
                        string fieldsJSON = context.Request["PatientPreferencesData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = UpdatePatientPreferences(fieldsJSON, PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_PREFERENCES":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = FillPatientPreferences(PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}