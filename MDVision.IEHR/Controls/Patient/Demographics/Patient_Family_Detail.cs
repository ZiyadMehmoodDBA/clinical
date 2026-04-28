using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Patient.Demographics
{
    public class Patient_Family_Detail
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_Family_Detail()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_Family_Detail _obj = null;
        public static Patient_Family_Detail Instance()
        {
            if (_obj == null)
                _obj = new Patient_Family_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Saves the emergency contact.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string SaveFamily(string fieldsJSON, Int64 PatientId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (PatientId > 0)
                {
                    DSPatient dsPatient = new DSPatient();
                    DSPatient.PatientFamilyRow dr = dsPatient.PatientFamily.NewPatientFamilyRow();

                    dr.PatientId = MDVUtility.ToInt64(PatientId);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtLastName"]))
                        dr.LastName = MDVUtility.ToStr(SearchedfieldsJSON["txtLastName"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFirstName"]))
                        dr.FirstName = MDVUtility.ToStr(SearchedfieldsJSON["txtFirstName"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtMiddleInitial"]))
                        dr.MI = MDVUtility.ToStr(SearchedfieldsJSON["txtMiddleInitial"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlRelation"]))
                        dr.RelationShipId = MDVUtility.ToInt(SearchedfieldsJSON["ddlRelation"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOtherRelation"]))
                        dr.OtherRelation = MDVUtility.ToStr(SearchedfieldsJSON["txtOtherRelation"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOB"]))
                        dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfLinkedPatientId"]))
                    {
                        if (MDVUtility.ToBool(SearchedfieldsJSON["chkAddAsPatient"]) == true && MDVUtility.ToInt64(SearchedfieldsJSON["hfLinkedPatientId"]) < 0)
                        {
                            BLObject<DSPatient> objPatient = BLLPatientObj.InsertPatient(SetPatientData(fieldsJSON, true, MDVUtility.ToInt64(SearchedfieldsJSON["hfLinkedPatientId"])));
                            if (objPatient.Data != null)
                                dr.LinkedPatientId = MDVUtility.ToInt64(objPatient.Data.Patients.Rows[0]["PatientId"]);
                            else
                            {
                                throw new Exception(objPatient.Message.ToString());
                            }
                        }
                        else
                            dr.LinkedPatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfLinkedPatientId"]);
                    }
                    dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                    dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                    dr.City = SearchedfieldsJSON["txtCity"];
                    dr.State = SearchedfieldsJSON["txtState"];
                    dr.ZipCode = SearchedfieldsJSON["txtZip"];
                    dr.ZipExt = SearchedfieldsJSON["txtZipExt"];
                    dr.HomePhoneNo = SearchedfieldsJSON["txtHomeTel"];
                    dr.WorkPhoneNo = SearchedfieldsJSON["txtWorkTel"];
                    dr.WorkPhExt = SearchedfieldsJSON["txtExt"];
                    dr.CellNo = SearchedfieldsJSON["txtCell"];
                    dr.FaxNo = SearchedfieldsJSON["txtFax"];
                    dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOD"]))
                        dr.DOD = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOD"]);
                    dr.IsAddAsPatient = MDVUtility.ToBool(SearchedfieldsJSON["chkAddAsPatient"]);
                    dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                    dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                    dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPractice"]);
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsPatient.PatientFamily.AddPatientFamilyRow(dr);
                    
                    BLObject<DSPatient> obj = BLLPatientObj.InsertPatientFamily(dsPatient);
                    if (obj.Data != null)
                    {
                        if (MDVUtility.ToBool(SearchedfieldsJSON["chkAddAsPatient"]) == true && MDVUtility.ToInt64(SearchedfieldsJSON["hfLinkedPatientId"]) > 0)
                        {
                            BLObject<DSPatient> objPatient = BLLPatientObj.UpdatePatient(SetPatientData(fieldsJSON, false, MDVUtility.ToInt64(SearchedfieldsJSON["hfLinkedPatientId"])));
                            if (objPatient.Data != null)
                            {
                                var response = new
                                {
                                    status = true,
                                    message = Common.AppPrivileges.Save_Message,
                                    PatientFamilyId = dsPatient.Tables[dsPatient.PatientFamily.TableName].Rows[0][dsPatient.PatientFamily.FamilyIdColumn.ColumnName]
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = objPatient.Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Save_Message,
                                PatientFamilyId = dsPatient.Tables[dsPatient.PatientFamily.TableName].Rows[0][dsPatient.PatientFamily.FamilyIdColumn.ColumnName]
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private DSPatient SetPatientData(string fieldsJSON, bool IsInsert, long LinkedPatientId)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
            DSPatient dsPatient = new DSPatient();
            if (IsInsert)
            {
                DSPatient.PatientsRow dr = dsPatient.Patients.NewPatientsRow();

                dr.LastName = SearchedfieldsJSON["txtLastName"];
                dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                dr.MI = SearchedfieldsJSON["txtMiddleInitial"];
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpDOB"])))
                    dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpDOD"])))
                    dr.DOD = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOD"]);
                dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                dr.City = SearchedfieldsJSON["txtCity"];
                dr.State = SearchedfieldsJSON["txtState"];
                dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                dr.ZIPCodeExt = SearchedfieldsJSON["txtZipExt"];
                dr.HomePhoneNo = SearchedfieldsJSON["txtHomeTel"];
                dr.WorkPhoneNo = SearchedfieldsJSON["txtWorkTel"];
                dr.WorkPhoneExt = SearchedfieldsJSON["txtExt"];
                dr.CellNo = SearchedfieldsJSON["txtCell"];
                dr.FaxNo = SearchedfieldsJSON["txtFax"];
                dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPractice"]);
                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                dr.IsActive = 1;
                dr.BadAddress = false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.PatientPortalStatus = "0";
                dsPatient.Patients.AddPatientsRow(dr);
            }
            else
            {
                BLObject<DSPatient> objLoad = BLLPatientObj.FillPatient_PictureById(MDVUtility.ToInt64(LinkedPatientId), "Demographics");
                dsPatient = objLoad.Data;
                if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                {
                    foreach (DSPatient.PatientsRow dr in dsPatient.Tables[dsPatient.Patients.TableName].Rows)
                    {
                        dr.LastName = SearchedfieldsJSON["txtLastName"];
                        dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                        dr.MI = SearchedfieldsJSON["txtMiddleInitial"];
                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpDOB"])))
                            dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpDOD"])))
                            dr.DOD = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOD"]);
                        else
                            dr[dsPatient.Patients.DODColumn] = DBNull.Value;
                        dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                        dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                        dr.City = SearchedfieldsJSON["txtCity"];
                        dr.State = SearchedfieldsJSON["txtState"];
                        dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                        dr.ZIPCodeExt = SearchedfieldsJSON["txtZipExt"];
                        dr.HomePhoneNo = SearchedfieldsJSON["txtHomeTel"];
                        dr.WorkPhoneNo = SearchedfieldsJSON["txtWorkTel"];
                        dr.WorkPhoneExt = SearchedfieldsJSON["txtExt"];
                        dr.CellNo = SearchedfieldsJSON["txtCell"];
                        dr.FaxNo = SearchedfieldsJSON["txtFax"];
                        dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                        dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                        dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPractice"]);
                        dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }
                }
            }
            return dsPatient;
        }

        /// <summary>
        /// Updates the emergency contact.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="FamilyID">The emergency contact identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string UpdateFamily(string fieldsJSON, Int64 PatientId, Int64 FamilyID)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (FamilyID > 0)
                {
                    DSPatient dsPatient = new DSPatient();
                    BLObject<DSPatient> objLoad = BLLPatientObj.LoadPatientFamily(PatientId, FamilyID);
                    dsPatient = objLoad.Data;
                    foreach (DSPatient.PatientFamilyRow dr in dsPatient.Tables[dsPatient.PatientFamily.TableName].Rows)
                    {
                        dr.PatientId = MDVUtility.ToInt64(PatientId);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtLastName"]))
                            dr.LastName = MDVUtility.ToStr(SearchedfieldsJSON["txtLastName"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFirstName"]))
                            dr.FirstName = MDVUtility.ToStr(SearchedfieldsJSON["txtFirstName"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtMiddleInitial"]))
                            dr.MI = MDVUtility.ToStr(SearchedfieldsJSON["txtMiddleInitial"]);
                        else
                            dr[dsPatient.PatientFamily.MIColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlRelation_text"]))
                            dr.RelationShipId = MDVUtility.ToInt(SearchedfieldsJSON["ddlRelation"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOtherRelation"]))
                            dr.OtherRelation = MDVUtility.ToStr(SearchedfieldsJSON["txtOtherRelation"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOB"]))
                            dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                        else
                            dr[dsPatient.PatientFamily.DOBColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfLinkedPatientId"]))
                        {
                            if (MDVUtility.ToBool(SearchedfieldsJSON["chkAddAsPatient"]) == true && MDVUtility.ToInt64(SearchedfieldsJSON["hfLinkedPatientId"]) < 0)
                            {
                                BLObject<DSPatient> objPatient = BLLPatientObj.InsertPatient(SetPatientData(fieldsJSON, true, MDVUtility.ToInt64(SearchedfieldsJSON["hfLinkedPatientId"])));
                                if (objPatient.Data != null)
                                    dr.LinkedPatientId = MDVUtility.ToInt64(objPatient.Data.Patients.Rows[0]["PatientId"]);
                                else
                                {
                                    throw new Exception(objPatient.Message.ToString());
                                }
                            }
                            else
                                dr.LinkedPatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfLinkedPatientId"]);
                        }

                        dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                        dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                        dr.City = SearchedfieldsJSON["txtCity"];
                        dr.State = SearchedfieldsJSON["txtState"];
                        dr.ZipCode = SearchedfieldsJSON["txtZip"];
                        dr.ZipExt = SearchedfieldsJSON["txtZipExt"];
                        dr.HomePhoneNo = SearchedfieldsJSON["txtHomeTel"];
                        dr.WorkPhoneNo = SearchedfieldsJSON["txtWorkTel"];
                        dr.WorkPhExt = SearchedfieldsJSON["txtExt"];
                        dr.CellNo = SearchedfieldsJSON["txtCell"];
                        dr.FaxNo = SearchedfieldsJSON["txtFax"];
                        dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOD"]))
                            dr.DOD = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOD"]);
                        else
                            dr[dsPatient.PatientFamily.DODColumn] = DBNull.Value;

                        dr.IsAddAsPatient = MDVUtility.ToBool(SearchedfieldsJSON["chkAddAsPatient"]);
                        dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                        dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPractice"]);

                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["hfIsActive"]) == "True" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }


                    #region Database Updation

                    if (dsPatient.Tables[dsPatient.PatientFamily.TableName].Rows.Count > 0)
                    {
                        BLObject<DSPatient> obj = BLLPatientObj.UpdatePatientFamily(dsPatient);
                        if (obj.Data != null)
                        {
                            if (MDVUtility.ToBool(SearchedfieldsJSON["chkAddAsPatient"]) == true && MDVUtility.ToInt64(SearchedfieldsJSON["hfLinkedPatientId"]) > 0)
                            {
                                BLObject<DSPatient> objPatient = BLLPatientObj.UpdatePatient(SetPatientData(fieldsJSON, false, MDVUtility.ToInt64(SearchedfieldsJSON["hfLinkedPatientId"])));
                                if(objPatient.Data != null)
                                {
                                    var response = new
                                    {
                                        status = true,
                                        message = Common.AppPrivileges.Update_Message,
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                }
                                else
                                {
                                    var response = new
                                    {
                                        status = false,
                                        Message = objPatient.Message
                                    };
                                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    message = Common.AppPrivileges.Update_Message,
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                        Message = "Family not found."
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
            //return "";
        }

        private string UpdateFamilyIsActive(Int64 PatientId, Int64 FamilyID, Int64 IsActive)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj = BLLPatientObj.LoadPatientFamily(PatientId, FamilyID);
                dsPatient = obj.Data;
                if (dsPatient.Tables[dsPatient.PatientFamily.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsPatient.Tables[dsPatient.PatientFamily.TableName].Rows[0];
                    dr[dsPatient.PatientFamily.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSPatient> objFamily = BLLPatientObj.UpdatePatientFamily(dsPatient);
                    string successMsg;
                    if (objFamily.Data != null)
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
                            Message = objFamily.Message
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Fills the emergency contact.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string FillFamily(long PatientID, long FamilyID)
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
                    BLObject<DSPatient> obj = BLLPatientObj.LoadPatientFamily(PatientID, FamilyID);
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;
                        if (dsPatient.Tables[dsPatient.PatientFamily.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatient.Tables[dsPatient.PatientFamily.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtLastName", MDVUtility.ToStr(dr[dsPatient.PatientFamily.LastNameColumn.ColumnName])},
                            { "txtFirstName", MDVUtility.ToStr(dr[dsPatient.PatientFamily.FirstNameColumn.ColumnName])},
                            { "txtOtherRelation", MDVUtility.ToStr(dr[dsPatient.PatientFamily.OtherRelationColumn.ColumnName])},
                            { "txtMiddleInitial", MDVUtility.ToStr(dr[dsPatient.PatientFamily.MIColumn.ColumnName])},
                            { "ddlRelation", MDVUtility.ToStr(dr[dsPatient.PatientFamily.RelationShipIdColumn.ColumnName])},
                            { "dtpDOB", MDVUtility.ToStr(dr[dsPatient.PatientFamily.DOBColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsPatient.PatientFamily.DOBColumn.ColumnName]).ToShortDateString():""},
                            { "dtpDOD", MDVUtility.ToStr(dr[dsPatient.PatientFamily.DODColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsPatient.PatientFamily.DODColumn.ColumnName]).ToShortDateString():""},
                            { "txtAddress1", MDVUtility.ToStr(dr[dsPatient.PatientFamily.Address1Column.ColumnName])},
                            { "txtAddress2", MDVUtility.ToStr(dr[dsPatient.PatientFamily.Address2Column.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsPatient.PatientFamily.CityColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsPatient.PatientFamily.StateColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsPatient.PatientFamily.ZipCodeColumn.ColumnName])},
                            { "txtZipExt", MDVUtility.ToStr(dr[dsPatient.PatientFamily.ZipExtColumn.ColumnName])},
                            { "txtHomeTel", MDVUtility.ToStr(dr[dsPatient.PatientFamily.HomePhoneNoColumn.ColumnName])},
                            { "txtWorkTel", MDVUtility.ToStr(dr[dsPatient.PatientFamily.WorkPhoneNoColumn.ColumnName])},
                            { "txtExt", MDVUtility.ToStr(dr[dsPatient.PatientFamily.WorkPhExtColumn.ColumnName])},
                            { "txtCell", MDVUtility.ToStr(dr[dsPatient.PatientFamily.CellNoColumn.ColumnName])},
                            { "txtFax", MDVUtility.ToStr(dr[dsPatient.PatientFamily.FaxNoColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsPatient.PatientFamily.EmailAddressColumn.ColumnName])},
                            { "hfIsActive", MDVUtility.ToStr(dr[dsPatient.PatientFamily.IsActiveColumn.ColumnName])},
                            { "hfRelationName", MDVUtility.ToStr(dr[dsPatient.PatientFamily.RelationShipNameColumn.ColumnName])},
                            { "hfLinkedPatientId", MDVUtility.ToStr(dr[dsPatient.PatientFamily.LinkedPatientIdColumn.ColumnName])},
                            { "txtAccountNumber", MDVUtility.ToStr(dr[dsPatient.Patients.AccountNumberColumn.ColumnName])},
                            { "chkAddAsPatient", MDVUtility.ToStr(dr[dsPatient.PatientFamily.IsAddAsPatientColumn.ColumnName])},
                            { "txtProvider", MDVUtility.ToStr(dr[dsPatient.PatientFamily.ProviderNameColumn.ColumnName])},
                            { "hfProvider", MDVUtility.ToStr(dr[dsPatient.PatientFamily.ProviderIdColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsPatient.PatientFamily.FacilityNameColumn.ColumnName])},
                            { "hfFacility", MDVUtility.ToStr(dr[dsPatient.PatientFamily.FacilityIdColumn.ColumnName])},
                            { "txtPractice", MDVUtility.ToStr(dr[dsPatient.PatientFamily.PracticeNameColumn.ColumnName])},
                            { "hfPractice", MDVUtility.ToStr(dr[dsPatient.PatientFamily.PracticeIdColumn.ColumnName])},
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PatientFamilyFill_JSON = js.Serialize(keyValues)
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

        /// <summary>
        /// Fills the patient information.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="IsFromSearch">The is from search.</param>
        /// <returns>System.String.</returns>
        private string FillPatientInfo(Int64 PatientId, Int64 IsFromSearch)
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
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.FillPatientById(PatientId, "Family");
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;
                        if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string> { };
                            if (IsFromSearch == 1)
                            {
                                keyValues = new Dictionary<string, string>
                            {
                                { "txtLastName", MDVUtility.ToStr(dr[dsPatient.Patients.LastNameColumn.ColumnName])},
                                { "txtFirstName", MDVUtility.ToStr(dr[dsPatient.Patients.FirstNameColumn.ColumnName])},
                                { "txtMiddleInitial", MDVUtility.ToStr(dr[dsPatient.Patients.MIColumn.ColumnName])},
                                { "dtpDOB",string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.Patients.DOBColumn.ColumnName]))?"":MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]).ToShortDateString()},
                                { "txtAddress1", MDVUtility.ToStr(dr[dsPatient.Patients.Address1Column.ColumnName])},
                                { "txtAddress2", MDVUtility.ToStr(dr[dsPatient.Patients.Address2Column.ColumnName])},
                                { "txtCity", MDVUtility.ToStr(dr[dsPatient.Patients.CityColumn.ColumnName])},
                                { "txtState", MDVUtility.ToStr(dr[dsPatient.Patients.StateColumn.ColumnName])},
                                { "txtZip", MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeColumn.ColumnName])},
                                { "txtZipExt", MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeExtColumn.ColumnName])},
                                { "txtHomeTel", MDVUtility.ToStr(dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName])},
                                { "txtWorkTel", MDVUtility.ToStr(dr[dsPatient.Patients.WorkPhoneNoColumn.ColumnName])},
                                { "txtExt", MDVUtility.ToStr(dr[dsPatient.Patients.WorkPhoneExtColumn.ColumnName])},
                                { "txtCell", MDVUtility.ToStr(dr[dsPatient.Patients.CellNoColumn.ColumnName])},
                                { "txtFax", MDVUtility.ToStr(dr[dsPatient.Patients.FaxNoColumn.ColumnName])},
                                { "txtEmail", MDVUtility.ToStr(dr[dsPatient.Patients.EmailAddressColumn.ColumnName])},
                                { "hfLinkedPatientId", MDVUtility.ToStr(dr[dsPatient.Patients.PatientIdColumn.ColumnName])},
                                { "txtAccountNumber", MDVUtility.ToStr(dr[dsPatient.Patients.AccountNumberColumn.ColumnName])}
                            };
                            }
                            else
                            {
                                keyValues = new Dictionary<string, string>
                            {
                                { "txtAddress1", MDVUtility.ToStr(dr[dsPatient.Patients.Address1Column.ColumnName])},
                                { "txtAddress2", MDVUtility.ToStr(dr[dsPatient.Patients.Address2Column.ColumnName])},
                                { "txtCity", MDVUtility.ToStr(dr[dsPatient.Patients.CityColumn.ColumnName])},
                                { "txtState", MDVUtility.ToStr(dr[dsPatient.Patients.StateColumn.ColumnName])},
                                { "txtZip", MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeColumn.ColumnName])},
                                { "txtZipExt", MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeExtColumn.ColumnName])},
                                { "txtHomeTel", MDVUtility.ToStr(dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName])},
                                //{ "txtWorkTel", MDVUtility.ToStr(dr[dsPatient.Patients.WorkPhoneNoColumn.ColumnName])},
                                //{ "txtExt", MDVUtility.ToStr(dr[dsPatient.Patients.WorkPhoneExtColumn.ColumnName])},
                                //{ "txtCell", MDVUtility.ToStr(dr[dsPatient.Patients.CellNoColumn.ColumnName])},
                                //{ "txtFax", MDVUtility.ToStr(dr[dsPatient.Patients.FaxNoColumn.ColumnName])},
                                //{ "txtEmail", MDVUtility.ToStr(dr[dsPatient.Patients.EmailAddressColumn.ColumnName])}
                            };
                            }

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PatientInfoFill_JSON = js.Serialize(keyValues)
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
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_FAMILY":
                    {
                        string fieldsJSON = context.Request["FamilyData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = SaveFamily(fieldsJSON, PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_FAMILY":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 FamilyID = MDVUtility.ToInt64(context.Request["FamilyID"]);
                        string strJSONData = FillFamily(PatientID, FamilyID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_FAMILY":
                    {
                        string fieldsJSON = context.Request["FamilyData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 FamilyID = MDVUtility.ToInt64(context.Request["FamilyID"]);
                        string strJSONData = UpdateFamily(fieldsJSON, PatientID, FamilyID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_PATIENT_INFO":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 IsFromSearch = MDVUtility.ToInt64(context.Request["IsFromSearch"]);
                        string strJSONData = FillPatientInfo(PatientID, IsFromSearch);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion
    }
}
