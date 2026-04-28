using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using System.Threading.Tasks;
using MDVision.Model.Admin.Provider;
using System.Net;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_Provider_Detail
    {
        private BLLFeeSchedule BLLFeeScheduleObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_Provider_Detail()
        {
            BLLFeeScheduleObj = new BLLFeeSchedule();
            BLLAdminProfileObj = new BLLAdminProfile();
        }

        #region Singleton
        private static Admin_Provider_Detail _obj = null;
        public static Admin_Provider_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_Provider_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the provider.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string SaveProvider(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsProfile = new DSProfile();
                    DSProfile.ProviderRow dr = dsProfile.Provider.NewProviderRow();
                    ProviderModel modelProviderCPTs = new ProviderModel();

                    var ProviderCPTsList = ser.Deserialize<ProviderModel>(MDVUtility.ToStr(fieldsJSON));

                    if (ProviderCPTsList.ListProviderCPTs.Count > 0)
                    {
                        modelProviderCPTs.ProviderCPTsXML = MDVUtility.GetXmlOfObject(typeof(List<ProviderCPTs>), ProviderCPTsList.ListProviderCPTs);
                    }

                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                    dr.MiddleInitial = SearchedfieldsJSON["txtMI"];
                    dr.LastName = SearchedfieldsJSON["txtLastName"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMeaningfulUse"]))
                    {
                        dr.MeaningfulUse = MDVUtility.ToInt32(SearchedfieldsJSON["ddlMeaningfulUse"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSpecialty"]))
                    {
                        dr.SpecialtyId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlSpecialty"]);
                        dr.SpecialtyName = MDVUtility.ToStr(SearchedfieldsJSON["ddlSpecialty_text"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSex"]))
                    {
                        dr.Gender = MDVUtility.ToInt16(SearchedfieldsJSON["ddlSex"]);
                        dr.GenderName = MDVUtility.ToStr(SearchedfieldsJSON["ddlSex_text"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlProfileType"]))
                    {
                        dr.ProfileType = MDVUtility.ToStr(SearchedfieldsJSON["ddlProfileType"]);
                        dr.ProfileTypeName = MDVUtility.ToStr(SearchedfieldsJSON["ddlProfileType_text"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                    {
                        dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                        dr.EntityName = MDVUtility.ToStr(SearchedfieldsJSON["ddlEntity_text"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFeeGroup"]))
                    {
                        dr.FeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFeeGroup"]);
                        dr.FeeGroupName = MDVUtility.ToStr(SearchedfieldsJSON["ddlFeeGroup_text"]);
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBasicFeeGroup"]))
                    {
                        dr.BasicFeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBasicFeeGroup"]);
                        dr.BasicFeeGroupName = MDVUtility.ToStr(SearchedfieldsJSON["ddlBasicFeeGroup_text"]);
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlProviderType"]))
                    {
                        dr.ProviderType = MDVUtility.ToStr(SearchedfieldsJSON["ddlProviderType"]);
                        dr.ProviderTypeName = MDVUtility.ToStr(SearchedfieldsJSON["ddlProviderType_text"]);
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSupervisingProvider"]))
                    {
                        dr.SupervisingProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlSupervisingProvider"]);
                        dr.SupervisingProviderName = MDVUtility.ToStr(SearchedfieldsJSON["ddlSupervisingProvider_text"]);
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlHeaderTemplate"]))
                    {
                        dr.ReportHeaderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlHeaderTemplate"]);
                        dr.HeaderTemplateName = MDVUtility.ToStr(SearchedfieldsJSON["ddlHeaderTemplate_text"]);
                    }
                    else
                    {
                        dr[dsProfile.Provider.ReportHeaderIdColumn.ColumnName] = DBNull.Value;
                    }

                    //add by azhar, for PQRS , started 7/20/2016
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtTIN"])))
                    {
                        dr[dsProfile.Provider.TINColumn.ColumnName] = DBNull.Value;
                    }
                    else
                    {
                        dr.TIN = MDVUtility.ToStr(SearchedfieldsJSON["txtTIN"]);
                    }

                    //end add by azhar on 7/20/2016
                    dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                    dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                    dr.ZIPCodeExt = SearchedfieldsJSON["txtZipExt"];
                    dr.State = SearchedfieldsJSON["txtState"];
                    dr.City = SearchedfieldsJSON["txtCity"];
                    dr.HomeAddress = "";
                    dr.OfficeAddress = SearchedfieldsJSON["txtOfficeAddress"];
                    dr.Fax = SearchedfieldsJSON["txtFax"];
                    dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                    dr.PhoneExt = SearchedfieldsJSON["txtExt"];
                    dr.Comments = SearchedfieldsJSON["txtComments"];
                    dr.TaxonomyCode = SearchedfieldsJSON["txtTaxonomyCode"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.IsSpecialist = MDVUtility.ToStr(SearchedfieldsJSON["chkSpecialist"]) == "True" ? true : false;
                    dr.IsScrubClaim = MDVUtility.ToStr(SearchedfieldsJSON["chkScrubClaim"]) == "True" ? true : false;
                    dr.CLIA = SearchedfieldsJSON["txtCLIA"];
                    dr.NPI = SearchedfieldsJSON["txtNPI"];
                    dr.WebSiteURL = SearchedfieldsJSON["txtWebSite"];
                    dr.DEA = SearchedfieldsJSON["txtDEA"];
                    dr.Alias = SearchedfieldsJSON["txtAlias"];
                    dr.Qualification = SearchedfieldsJSON["txtQualification"];
                    dr.SSN = MDVUtility.GetSimpleNumber(SearchedfieldsJSON["txtSSN"]);
                    dr.CellNo = SearchedfieldsJSON["txtCell"];
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRcopiaUserName"]))
                        dr.RcopiaUserName = MDVUtility.ToStr(SearchedfieldsJSON["txtRcopiaUserName"]);
                    else
                    {
                        dr[dsProfile.Provider.RcopiaUserNameColumn.ColumnName] = DBNull.Value;
                    }
                    // Start  ||  Talha Tanweer || 22 july 2016
                    dr.Is_eSignatured = Convert.ToBoolean(SearchedfieldsJSON["chkeSignature"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["img_eSignature_Admin_Provider_Detail"]))
                    {
                        // Checking whether default image is shown or not
                        if (!SearchedfieldsJSON["img_eSignature_Admin_Provider_Detail"].Contains("default"))
                        {
                            string strBase64 = SearchedfieldsJSON["img_eSignature_Admin_Provider_Detail"].Split(',')[1];
                            strBase64 = strBase64.Replace(' ', '+');
                            byte[] currentFileStream = Convert.FromBase64String(strBase64);
                            dr.eSignature = currentFileStream;
                            // dr.ImageType = SearchedfieldsJSON["imgPatient"].Split(',')[0].Split(':')[1].Split(';')[0];
                        }
                    }
                    // End    ||  Talha Tanweer || 22 july 2016
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlVisitTypeDurationGroup"]))
                        dr.VisitDurationGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlVisitTypeDurationGroup"]);
                    else
                        dr[dsProfile.Provider.VisitDurationGroupIdColumn.ColumnName] = DBNull.Value;

                    dr.IsNoteSignWOCPTCode = MDVUtility.ToBool(SearchedfieldsJSON["chbxNoteSignCPT"]);
                    dr.IsNoteSignWOICDCode = MDVUtility.ToBool(SearchedfieldsJSON["chbxNoteSignICD"]);
                    dr.NewPatientColor = SearchedfieldsJSON["txtNewPatColor"];
                    dr.EstablishedPatientColor = SearchedfieldsJSON["txtEstPatColor"];
                    dr.BulkSignGridShow = MDVUtility.ToStr(SearchedfieldsJSON["ddlBulkSign"]) == "1" ? true : false;

                    if (!string.IsNullOrEmpty(modelProviderCPTs.ProviderCPTsXML))
                    {
                        dr.ProviderCPTsXML = modelProviderCPTs.ProviderCPTsXML;
                    }
                    else
                    {
                        dr[dsProfile.Provider.ProviderCPTsXMLColumn.ColumnName] = DBNull.Value;
                    }
                    DataTable dtFacility = new DataTable();
                    dtFacility = MDVUtility.ConvertCommaSepatedValuesToDataTable(SearchedfieldsJSON.ContainsKey("strFacilityIds") ? SearchedfieldsJSON["strFacilityIds"] : "");

                    // Provider setting for note bulk sign exception
                    DataTable dtBulkSignException = new DataTable();
                    dtBulkSignException = MDVUtility.ConvertCommaSepatedValuesToDataTable(SearchedfieldsJSON.ContainsKey("strBulkSignExceptionIds") ? SearchedfieldsJSON["strBulkSignExceptionIds"] : "");
                    #region Database Insertion
                    dsProfile.Provider.AddProviderRow(dr);
                    BLObject<DSProfile> obj = BLLAdminProfileObj.InsertProvider(ref dsProfile, ref dtFacility, ref dtBulkSignException);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message_Contact_Admin,
                            ProviderId = dsProfile.Tables[dsProfile.Provider.TableName].Rows[0][dsProfile.Provider.ProviderIdColumn.ColumnName]
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

        /// <summary>
        /// Updates the provider.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateProvider(string fieldsJSON, Int64 ProviderId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    ProviderModel modelProviderCPTs = new ProviderModel();
                    var ProviderCPTsList = ser.Deserialize<ProviderModel>(MDVUtility.ToStr(fieldsJSON));

                    if (ProviderCPTsList.ListProviderCPTs.Count > 0)
                    {
                        modelProviderCPTs.ProviderCPTsXML = MDVUtility.GetXmlOfObject(typeof(List<ProviderCPTs>), ProviderCPTsList.ListProviderCPTs);
                    }

                    DSProfile dsProfile = new DSProfile();
                    //DSProfile.ProviderRow dr = dsProfile.Provider.NewProviderRow();

                    BLObject<DSProfile> objLoad = BLLAdminProfileObj.LoadProvider(ProviderId, null, null, null, null, null, null, null);
                    dsProfile = objLoad.Data;

                    if (objLoad.Data != null && dsProfile.Tables[dsProfile.Provider.TableName].Rows.Count > 0)
                    {
                        DSProfile.ProviderRow dr = dsProfile.Tables[dsProfile.Provider.TableName].Rows[0] as DSProfile.ProviderRow;
                        if (dr != null)
                        {
                            //dr.ProviderId = ProviderId;
                            dr.ShortName = SearchedfieldsJSON["txtShortName"];
                            dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                            dr.MiddleInitial = SearchedfieldsJSON["txtMI"];
                            dr.LastName = SearchedfieldsJSON["txtLastName"];
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMeaningfulUse"]))
                            {
                                dr.MeaningfulUse = MDVUtility.ToInt32(SearchedfieldsJSON["ddlMeaningfulUse"]);
                            }
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSpecialty"]))
                            {
                                dr.SpecialtyId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlSpecialty"]);
                                dr.SpecialtyName = MDVUtility.ToStr(SearchedfieldsJSON["ddlSpecialty_text"]);
                            }
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSex"]))
                            {
                                dr.Gender = MDVUtility.ToInt16(SearchedfieldsJSON["ddlSex"]);
                                dr.GenderName = MDVUtility.ToStr(SearchedfieldsJSON["ddlSex_text"]);
                            }
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlProfileType"]))
                            {
                                dr.ProfileType = MDVUtility.ToStr(SearchedfieldsJSON["ddlProfileType"]);
                                dr.ProfileTypeName = MDVUtility.ToStr(SearchedfieldsJSON["ddlProfileType_text"]);
                            }
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEntity"]))
                            {
                                dr.EntityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlEntity"]);
                                dr.EntityName = MDVUtility.ToStr(SearchedfieldsJSON["ddlEntity"]);
                            }
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlFeeGroup"]))
                            {
                                dr.FeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlFeeGroup"]);
                                dr.FeeGroupName = MDVUtility.ToStr(SearchedfieldsJSON["ddlFeeGroup_text"]);
                            }
                            else
                                dr.FeeGroupId = MDVUtility.ToInt64(0);
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlBasicFeeGroup"]))
                            {
                                dr.BasicFeeGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlBasicFeeGroup"]);
                                dr.BasicFeeGroupName = MDVUtility.ToStr(SearchedfieldsJSON["ddlBasicFeeGroup_text"]);
                            }
                            else
                                dr.BasicFeeGroupId = MDVUtility.ToInt64(0);
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlProviderType"]))
                            {
                                dr.ProviderType = MDVUtility.ToStr(SearchedfieldsJSON["ddlProviderType"]);
                                dr.ProviderTypeName = MDVUtility.ToStr(SearchedfieldsJSON["ddlProviderType_text"]);
                            }
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSupervisingProvider"]))
                            {
                                dr.SupervisingProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlSupervisingProvider"]);
                                dr.SupervisingProviderName = MDVUtility.ToStr(SearchedfieldsJSON["ddlSupervisingProvider_text"]);
                            }
                            else
                                dr.SupervisingProviderId = MDVUtility.ToInt64(0);
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlHeaderTemplate"]))
                            {
                                dr.ReportHeaderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlHeaderTemplate"]);
                                dr.HeaderTemplateName = MDVUtility.ToStr(SearchedfieldsJSON["ddlHeaderTemplate_text"]);
                            }
                            else
                            {
                                dr[dsProfile.Provider.ReportHeaderIdColumn.ColumnName] = DBNull.Value;
                            }
                            //add by azhar, for PQRS , started 7/20/2016
                            if (string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtTIN"])))
                            {
                                dr[dsProfile.Provider.TINColumn.ColumnName] = DBNull.Value;
                            }
                            else
                            {
                                dr.TIN = MDVUtility.ToStr(SearchedfieldsJSON["txtTIN"]);
                            }
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRcopiaUserName"]))
                                dr.RcopiaUserName = MDVUtility.ToStr(SearchedfieldsJSON["txtRcopiaUserName"]);
                            else
                            {
                                dr[dsProfile.Provider.RcopiaUserNameColumn.ColumnName] = DBNull.Value;
                            }
                            //end add by azhar on 7/20/2016
                            dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                            dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                            dr.ZIPCodeExt = SearchedfieldsJSON["txtZipExt"];
                            dr.State = SearchedfieldsJSON["txtState"];
                            dr.City = SearchedfieldsJSON["txtCity"];
                            dr.HomeAddress = "";
                            dr.OfficeAddress = SearchedfieldsJSON["txtOfficeAddress"];
                            dr.Fax = SearchedfieldsJSON["txtFax"];
                            dr.PhoneNo = SearchedfieldsJSON["txtTelephone"];
                            dr.PhoneExt = SearchedfieldsJSON["txtExt"];
                            dr.Comments = SearchedfieldsJSON["txtComments"];
                            dr.TaxonomyCode = SearchedfieldsJSON["txtTaxonomyCode"];
                            dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                            dr.IsSpecialist = MDVUtility.ToStr(SearchedfieldsJSON["chkSpecialist"]) == "True" ? true : false;
                            dr.IsScrubClaim = MDVUtility.ToStr(SearchedfieldsJSON["chkScrubClaim"]) == "True" ? true : false;
                            dr.CLIA = SearchedfieldsJSON["txtCLIA"];
                            dr.NPI = SearchedfieldsJSON["txtNPI"];
                            dr.WebSiteURL = SearchedfieldsJSON["txtWebSite"];
                            dr.DEA = SearchedfieldsJSON["txtDEA"];
                            dr.Alias = SearchedfieldsJSON["txtAlias"];
                            dr.Qualification = SearchedfieldsJSON["txtQualification"];
                            dr.SSN = MDVUtility.GetSimpleNumber(SearchedfieldsJSON["txtSSN"]);
                            dr.CellNo = SearchedfieldsJSON["txtCell"];
                            //dr.CreatedBy = "";
                            //dr.CreatedOn = DateTime.Now;
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                            dr.BulkSignGridShow = MDVUtility.ToStr(SearchedfieldsJSON["ddlBulkSign"]) == "1" ? true : false;

                            // Start  ||  Talha Tanweer || 22 july 2016
                            dr.Is_eSignatured = Convert.ToBoolean(SearchedfieldsJSON["chkeSignature"]);
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["img_eSignature_Admin_Provider_Detail"]))
                            {
                                // Checking whether default image is shown or not
                                if (!SearchedfieldsJSON["img_eSignature_Admin_Provider_Detail"].Contains("default"))
                                {
                                    string strBase64 = SearchedfieldsJSON["img_eSignature_Admin_Provider_Detail"].Split(',')[1];
                                    strBase64 = strBase64.Replace(' ', '+');

                                    // ------------------------     strBase64 = _Utility.AddWaterMarkOnImage(strBase64, "©Sovereign Health System");

                                    byte[] currentFileStream = Convert.FromBase64String(strBase64);
                                    dr.eSignature = currentFileStream;
                                    // dr.ImageType = SearchedfieldsJSON["imgPatient"].Split(',')[0].Split(':')[1].Split(';')[0];
                                }
                            }
                            else
                            {
                                dr.eSignature = null;
                            }
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlVisitTypeDurationGroup"]))
                                dr.VisitDurationGroupId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlVisitTypeDurationGroup"]);
                            else
                                dr[dsProfile.Provider.VisitDurationGroupIdColumn.ColumnName] = DBNull.Value;

                            // End    ||  Talha Tanweer || 22 july 2016

                            if (!string.IsNullOrEmpty(modelProviderCPTs.ProviderCPTsXML))
                            {
                                dr.ProviderCPTsXML = modelProviderCPTs.ProviderCPTsXML;
                            }
                            else
                            {
                                dr[dsProfile.Provider.ProviderCPTsXMLColumn.ColumnName] = DBNull.Value;
                            }
                            dr.IsNoteSignWOCPTCode = MDVUtility.ToBool(SearchedfieldsJSON["chbxNoteSignCPT"]);
                            dr.IsNoteSignWOICDCode = MDVUtility.ToBool(SearchedfieldsJSON["chbxNoteSignICD"]);
                            dr.NewPatientColor = SearchedfieldsJSON["txtNewPatColor"];
                            dr.EstablishedPatientColor = SearchedfieldsJSON["txtEstPatColor"];

                            DataTable dtFacility = new DataTable();
                            dtFacility = MDVUtility.ConvertCommaSepatedValuesToDataTable(SearchedfieldsJSON.ContainsKey("strFacilityIds") ? SearchedfieldsJSON["strFacilityIds"] : "");

                            // Provider setting for note bulk sign exception
                            DataTable dtBulkSignException = new DataTable();
                            dtBulkSignException = MDVUtility.ConvertCommaSepatedValuesToDataTable(SearchedfieldsJSON.ContainsKey("strBulkSignExceptionIds") ? SearchedfieldsJSON["strBulkSignExceptionIds"] : "");

                            #region Database Updation
                            if (dsProfile.Tables[dsProfile.Provider.TableName].Rows.Count > 0)
                            {
                                BLObject<DSProfile> obj = BLLAdminProfileObj.UpdateProvider(ref dsProfile, ref dtFacility, ref dtBulkSignException);
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
                                    Message = Common.AppPrivileges.Update_Error_Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.Update_Error_Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objLoad.Message
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

        /// <summary>
        /// Fills the provider.
        /// </summary>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string FillProvider(Int64 ProviderId, bool IsWaterMarkApplied,string NIPNumber)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ProviderId)))
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
                    DSProfile dsProfile = null;
                    BLObject<DSProfile> obj = BLLAdminProfileObj.LoadProvider(ProviderId, null, null, null, null, NIPNumber, null, null);
                    if (obj.Data != null)
                    {
                        dsProfile = obj.Data;

                        if (dsProfile.Tables[dsProfile.Provider.TableName].Rows.Count > 0)
                        {
                            if (dsProfile.Tables[dsProfile.Provider.TableName].Rows.Count < 2)
                            {
                                DataRow dr = dsProfile.Tables[dsProfile.Provider.TableName].Rows[0];

                                string imageBase64 = "";
                                string orignalimage = "";
                                {
                                    byte[] imageByteArr = dr[dsProfile.Provider.eSignatureColumn.ColumnName] as byte[];
                                    if (imageByteArr != null)
                                    {
                                        orignalimage = "data:" + dr[dsProfile.Provider.eSignatureColumn.ColumnName] + ";base64," + Convert.ToBase64String(dr[dsProfile.Provider.eSignatureColumn.ColumnName] as byte[]);
                                        if (IsWaterMarkApplied)
                                        {
                                            var imageString = Convert.ToBase64String(dr[dsProfile.Provider.eSignatureColumn.ColumnName] as byte[]);
                                            imageBase64 = MDVUtility.AddWaterMarkOnImage(imageString, "©Sovereign Health System");
                                            imageBase64 = "data:" + dr[dsProfile.Provider.eSignatureColumn.ColumnName] + ";base64," + imageBase64;
                                        }
                                        else
                                        {
                                            imageBase64 = "data:" + dr[dsProfile.Provider.eSignatureColumn.ColumnName] + ";base64," + Convert.ToBase64String(dr[dsProfile.Provider.eSignatureColumn.ColumnName] as byte[]);
                                        }

                                    }
                                }
                                string strDiagnosticImagingFacilities = dsProfile.Tables[dsProfile.ProviderDiagnosticImagingFacilities.TableName].Rows.Count > 0 ? dsProfile.Tables[dsProfile.ProviderDiagnosticImagingFacilities.TableName].AsEnumerable().Select(row => row["FacilityId"].ToString()).Aggregate((s1, s2) => String.Concat(s1, "," + s2)) : String.Empty;
                                var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsProfile.Provider.ShortNameColumn.ColumnName])},
                            { "txtFirstName", MDVUtility.ToStr(dr[dsProfile.Provider.FirstNameColumn.ColumnName])},
                            { "txtMI", MDVUtility.ToStr(dr[dsProfile.Provider.MiddleInitialColumn.ColumnName])},
                            { "txtLastName", MDVUtility.ToStr(dr[dsProfile.Provider.LastNameColumn.ColumnName])},
                            { "ddlBasicFeeGroup", MDVUtility.ToStr(dr[dsProfile.Provider.BasicFeeGroupIdColumn.ColumnName])},
                            { "txtddlBasicFeeGroup", MDVUtility.ToStr(dr[dsProfile.Provider.BasicFeeGroupColumn.ColumnName])},
                            { "ddlFeeGroup", MDVUtility.ToStr(dr[dsProfile.Provider.FeeGroupIdColumn.ColumnName])},
                            { "ddlSpecialty", MDVUtility.ToStr(dr[dsProfile.Provider.SpecialtyIdColumn.ColumnName])},
                            { "ddlSex", MDVUtility.ToStr(dr[dsProfile.Provider.GenderColumn.ColumnName])},
                            { "ddlProfileType", MDVUtility.ToStr(dr[dsProfile.Provider.ProfileTypeColumn.ColumnName])},
                            { "txtWebSite", MDVUtility.ToStr(dr[dsProfile.Provider.WebSiteURLColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsProfile.Provider.EmailAddressColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsProfile.Provider.ZIPCodeColumn.ColumnName])},
                            { "txtZipExt", MDVUtility.ToStr(dr[dsProfile.Provider.ZIPCodeExtColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsProfile.Provider.StateColumn.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsProfile.Provider.CityColumn.ColumnName])},
                            { "txtOfficeAddress", MDVUtility.ToStr(dr[dsProfile.Provider.OfficeAddressColumn.ColumnName])},
                            { "txtFax", MDVUtility.ToStr(dr[dsProfile.Provider.FaxColumn.ColumnName])},
                            { "txtTelephone", MDVUtility.ToStr(dr[dsProfile.Provider.PhoneNoColumn.ColumnName])},
                            { "txtExt", MDVUtility.ToStr(dr[dsProfile.Provider.PhoneExtColumn.ColumnName])},
                            { "txtCell", MDVUtility.ToStr(dr[dsProfile.Provider.CellNoColumn.ColumnName])},
                            { "txtTaxonomyCode", MDVUtility.ToStr(dr[dsProfile.Provider.TaxonomyCodeColumn.ColumnName])},
                            { "txtNPI", MDVUtility.ToStr(dr[dsProfile.Provider.NPIColumn.ColumnName])},
                            { "ddlProviderType", MDVUtility.ToStr(dr[dsProfile.Provider.ProviderTypeColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsProfile.Provider.CommentsColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsProfile.Provider.IsActiveColumn.ColumnName])},
                            { "chkSpecialist", MDVUtility.ToStr(dr[dsProfile.Provider.IsSpecialistColumn.ColumnName])},
                            { "chkScrubClaim", MDVUtility.ToStr(dr[dsProfile.Provider.IsScrubClaimColumn.ColumnName])},
                            { "ddlEntity", MDVUtility.ToStr(dr[dsProfile.Provider.EntityIdColumn.ColumnName])},
                            { "ddlSupervisingProvider", MDVUtility.ToStr(dr[dsProfile.Provider.SupervisingProviderIdColumn.ColumnName])},
                            { "txtCLIA", MDVUtility.ToStr(dr[dsProfile.Provider.CLIAColumn.ColumnName])},
                            { "txtDEA", MDVUtility.ToStr(dr[dsProfile.Provider.DEAColumn.ColumnName])},
                            { "txtAlias", MDVUtility.ToStr(dr[dsProfile.Provider.AliasColumn.ColumnName])},
                            { "txtQualification", MDVUtility.ToStr(dr[dsProfile.Provider.QualificationColumn.ColumnName])},
                            //add by azhar, for PQRS , started 7/20/2016
                            { "txtTIN", MDVUtility.ToStr(dr[dsProfile.Provider.TINColumn.ColumnName])},
                            { "txtSSN", MDVUtility.GetFormatedNumber(MDVUtility.ToLong(dr[dsProfile.Provider.SSNColumn.ColumnName]),MDVUtility.NumbersType.SSN)},
                           // Start  ||  Talha Tanweer || 22 july 2016
                            { "chkIs_eSignatured", MDVUtility.ToStr(dr[dsProfile.Provider.Is_eSignaturedColumn.ColumnName])},
                            { "imgeSignature",!String.IsNullOrEmpty(imageBase64)? imageBase64:""},
                            { "OrignalimgeSignature",!String.IsNullOrEmpty(orignalimage)? orignalimage:""},
                            { "ddlHeaderTemplate", MDVUtility.ToStr(dr[dsProfile.Provider.ReportHeaderIdColumn.ColumnName])},
                            { "txtRcopiaUserName", MDVUtility.ToStr(dr[dsProfile.Provider.RcopiaUserNameColumn.ColumnName])},
                            { "ddlVisitTypeDurationGroup", MDVUtility.ToStr(dr[dsProfile.Provider.VisitDurationGroupIdColumn.ColumnName])},
                            { "strDiagnosticImagingFacilities", MDVUtility.ToStr(strDiagnosticImagingFacilities) },
                            { "chbxNoteSignCPT", MDVUtility.ToStr(dr[dsProfile.Provider.IsNoteSignWOCPTCodeColumn.ColumnName])},
                            { "chbxNoteSignICD", MDVUtility.ToStr(dr[dsProfile.Provider.IsNoteSignWOICDCodeColumn.ColumnName])},
                            { "strBulkSignException", MDVUtility.ToStr(dr[dsProfile.Provider.BulkSignColumn.ColumnName])},
                            { "ddlMeaningfulUse", MDVUtility.ToStr(dr[dsProfile.Provider.MeaningfulUseColumn.ColumnName])},
                            { "txtNewPatColor", MDVUtility.ToStr(dr[dsProfile.Provider.NewPatientColorColumn.ColumnName])},
                            { "txtEstPatColor", MDVUtility.ToStr(dr[dsProfile.Provider.EstablishedPatientColorColumn.ColumnName])},
                            { "ddlBulkSign", MDVUtility.ToStr(dr[dsProfile.Provider.BulkSignGridShowColumn.ColumnName]) == "True" ? "1" : "0"},
                            // End    ||  Talha Tanweer || 22 july 2016
                        };
                            BLObject<List<ProviderModel>> model = BLLAdminProfileObj.GetProviderCpts(MDVUtility.ToInt64(dr[dsProfile.Provider.ProviderIdColumn.ColumnName]));
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            if (model.Data[0].ListProviderCPTs.Count > 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    ProviderFill_JSON = js.Serialize(keyValues),
                                    ProviderCPTs_JSON = js.Serialize(model.Data[0].ListProviderCPTs)
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    ProviderFill_JSON = js.Serialize(keyValues),
                                    ProviderCPTs_JSON = "[]"
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }

                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "Multiple records Found.",
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

        /// <summary>
        /// Fills the provider.
        /// </summary>
        /// <param name="ProviderIds">The provider identifier.</param>
        /// <returns>Json string containing key value pair or Exception message</returns>
        private string GetProviderInfo(string ProviderIds)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProviderIds)))
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
                    Dictionary<long, string> ProvidersDetail = new Dictionary<long, string>();
                    List<string> list = ProviderIds.Split(',').ToList();
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    Dictionary<long, Task<BLObject<DSProfile>>> tasks = new Dictionary<long, Task<BLObject<DSProfile>>>();
                    List<ProviderResponseModel> Response_list = new List<ProviderResponseModel>();

                    SharedVariable SharedVariable = new SharedVariable();
                    SharedVariable.EntityId = MDVSession.Current.EntityId;
                    SharedVariable.AppUserId = MDVSession.Current.AppUserId;
                    SharedVariable.UserName = MDVSession.Current.AppUserName;
                    SharedVariable.ClientId = MDVSession.Current.ClientId;
                    SharedVariable.AppPassWord = MDVSession.Current.AppPassWord;
                    SharedVariable.WebEntityURL = MDVSession.Current.WebEntityURL;

                    for (int i = 0; i < list.Count; i++)
                    {
                        long ProviderId = MDVUtility.ToLong(list[i]);
                        if (ProviderId > 0)
                        {

                            BLLAdminProfile BLLAdminProfileObj1 = new BLLAdminProfile(SharedVariable);
                            Task<BLObject<DSProfile>> task = new Task<BLObject<DSProfile>>(() => BLLAdminProfileObj1.LoadProvider(SharedVariable, ProviderId, null, null, null, null, null, null, null));
                            tasks.Add(ProviderId, task);
                            task.Start();
                        }
                    }

                    Task.WaitAll(tasks.Values.ToArray());
                    tasks.OrderByDescending(p => p.Key);
                    List<BLObject<DSProfile>> list_ = tasks.Values.ToList<Task<BLObject<DSProfile>>>().Select(p => p.Result).ToList<BLObject<DSProfile>>();
                    foreach (var item in tasks)
                    {
                        DSProfile dsProfile = null;
                        if (item.Value.Result.Data != null)
                        {
                            dsProfile = item.Value.Result.Data;
                            if (dsProfile.Tables[dsProfile.Provider.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsProfile.Tables[dsProfile.Provider.TableName].Rows[0];

                                var keyValues = new Dictionary<string, string>
                                        {
                                            { "txtShortName", MDVUtility.ToStr(dr[dsProfile.Provider.ShortNameColumn.ColumnName])},
                                            { "txtFirstName", MDVUtility.ToStr(dr[dsProfile.Provider.FirstNameColumn.ColumnName])},
                                            { "txtMI", MDVUtility.ToStr(dr[dsProfile.Provider.MiddleInitialColumn.ColumnName])},
                                            { "txtLastName", MDVUtility.ToStr(dr[dsProfile.Provider.LastNameColumn.ColumnName])},
                                            { "txtEmail", MDVUtility.ToStr(dr[dsProfile.Provider.EmailAddressColumn.ColumnName])},
                                            { "txtState", MDVUtility.ToStr(dr[dsProfile.Provider.StateColumn.ColumnName])},
                                            { "txtCity", MDVUtility.ToStr(dr[dsProfile.Provider.CityColumn.ColumnName])},
                                            { "txtOfficeAddress", MDVUtility.ToStr(dr[dsProfile.Provider.OfficeAddressColumn.ColumnName])},
                                            { "txtCell", MDVUtility.ToStr(dr[dsProfile.Provider.CellNoColumn.ColumnName])},
                                            { "txtNPI", MDVUtility.ToStr(dr[dsProfile.Provider.NPIColumn.ColumnName])},
                                            { "txtQualification", MDVUtility.ToStr(dr[dsProfile.Provider.QualificationColumn.ColumnName])},
                                        };

                                ProviderResponseModel model = new ProviderResponseModel();
                                model.status = true;
                                model.ProviderId = item.Key;
                                model.ProviderFill_JSON = keyValues;
                                Response_list.Add(model);
                            }
                            else
                            {
                                ProviderResponseModel model = new ProviderResponseModel();
                                model.status = false;
                                model.ProviderId = item.Key;
                                model.Message = Common.AppPrivileges.No_Record_Message;
                                Response_list.Add(model);
                            }
                        }
                        else
                        {
                            ProviderResponseModel model = new ProviderResponseModel();
                            model.status = false;
                            model.ProviderId = item.Key;
                            model.Message = item.Value.Result.Message;
                            Response_list.Add(model);

                        }
                    }

                    var response = new
                    {
                        status = true,
                        ProviderFill_JSON = Response_list,
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
        /// <summary>
        /// Fills the basic fee group.
        /// </summary>
        /// <param name="EntityID">The entity identifier.</param>
        /// <returns>System.String.</returns>
        private string FillBasicFeeGroup(string EntityID)
        {
            try
            {
                DSFeeSchedule dsBasicFeeGroup = null;
                BLObject<DSFeeSchedule> obj;
                obj = BLLFeeScheduleObj.LoadBasicFeeGroup(0, null, null, EntityID, null);

                dsBasicFeeGroup = obj.Data;
                var response = new
                {
                    status = true,
                    BasicFeeGroupCount = dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName].Rows.Count,
                    BasicFeeGroupLoad_JSON = MDVUtility.JSON_DataTable(dsBasicFeeGroup.Tables[dsBasicFeeGroup.BasicFeeGroup.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        /// Fills the supervising provider.
        /// </summary>
        /// <param name="EntityID">The entity identifier.</param>
        /// <returns>System.String.</returns>
        private string FillSupervisingProvider(string EntityID)
        {
            try
            {
                DSProfile dsProfile = null;
                BLObject<DSProfile> obj = BLLAdminProfileObj.LoadProvider(0, null, null, null, null, null, EntityID, null);
                dsProfile = obj.Data;
                var response = new
                {
                    status = true,
                    BasicFeeGroupCount = dsProfile.Tables[dsProfile.Provider.TableName].Rows.Count,
                    SupervisingProviderLoad_JSON = MDVUtility.JSON_DataTable(dsProfile.Tables[dsProfile.Provider.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        /// Fills the specialty.
        /// </summary>
        /// <param name="EntityID">The entity identifier.</param>
        /// <returns>System.String.</returns>
        private string FillSpecialty(string EntityID)
        {
            try
            {
                DSProfile dsProfile = null;
                //dsProfile = DALSpecialty.Instance.LoadSpecialty(SpecialtyId, null, null);
                BLObject<DSProfile> obj = BLLAdminProfileObj.LoadSpecialty(0, null, null, EntityID, "true");
                dsProfile = obj.Data;
                var response = new
                {
                    status = true,
                    BasicFeeGroupCount = dsProfile.Tables[dsProfile.Specialty.TableName].Rows.Count,
                    SpecialtyLoad_JSON = MDVUtility.JSON_DataTable(dsProfile.Tables[dsProfile.Specialty.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        /// Deletes the provider against provider Id.
        /// </summary>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string DeleteProvider(Int64 ProviderId)
        {

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ProviderId)))
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
                        BLObject<string> obj = BLLAdminProfileObj.DeleteProvider(MDVUtility.ToStr(ProviderId));
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

        /// <summary>
        /// Updates the provider is active.
        /// </summary>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>Json string containing Succes message or Exception message</returns>
        private string UpdateProviderIsActive(Int64 ProviderId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSProfile dsProfile = null;
                    BLObject<DSProfile> obj = BLLAdminProfileObj.LoadProvider(ProviderId, null, null, null, null, null, null, null);
                    dsProfile = obj.Data;
                    if (dsProfile.Tables[dsProfile.Provider.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsProfile.Tables[dsProfile.Provider.TableName].Rows[0];
                        dr[dsProfile.Provider.IsActiveColumn.ColumnName] = IsActive;
                        dr[dsProfile.Provider.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsProfile.Provider.ModifiedOnColumn.ColumnName] = DateTime.Now;
                        //    dr[dsProfile.Provider.IsActiveColumn.ColumnName] = 

                        BLObject<DSProfile> objProvider = BLLAdminProfileObj.UpdateProviderIsActive(ref dsProfile);
                        string successMsg;
                        if (objProvider.Data != null)
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
                                Message = objProvider.Message
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

        private string DeleteAssociatedProcedure(Int64 ProcedureListId)
        {

            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProcedureListId)))
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
                    BLObject<string> obj = BLLAdminProfileObj.DeleteAssociatedProcedure(MDVUtility.ToStr(ProcedureListId));
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Loads data from NPI API.
        /// </summary>
        /// <param name="NPINumber">Nation provider identifier.</param>
        /// <returns>Provider's data return by API against NPI</returns>
        private string LoadproviderApiData(string NPINumber)
        {
            string page = System.Configuration.ConfigurationManager.AppSettings["ProviderNPI_API_URL"] +"?address_purpose=MAILING&number="+ NPINumber;

            WebRequest request = WebRequest.Create(page);

            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            // Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server. 
            System.IO.Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access. 
            System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
            // Read the content. 
            string responseFromServer = reader.ReadToEnd();
            // Display the content. 
            //Console.WriteLine(responseFromServer);
            // Clean up the streams and the response. 
            reader.Close();
            response.Close();
            return responseFromServer;

        }


        /// <summary>
        /// Gets the state of the city.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <returns>
        /// Json string containing key value pair or Exception message
        /// </returns>
        //private string GetCityState(string zipcode, string cityname)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(MDVUtility.ToStr(zipcode)))
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = MDVUtility.ToStr("Please select a record first")
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        else
        //        {
        //            DSCityStateZip dsCityStatzip = null;
        //            BLObject<DSCityStateZip> obj = BLAdminCityStateZip.BusinessObj.GetCityState(zipcode, cityname);
        //            dsCityStatzip = obj.Data;

        //            if (obj.Data != null)
        //            {
        //                if (dsCityStatzip.Tables[dsCityStatzip.CityState.TableName].Rows.Count > 0)
        //                {
        //                    DataRow dr = dsCityStatzip.Tables[dsCityStatzip.CityState.TableName].Rows[0];
        //                    var keyValues = new Dictionary<string, string>
        //                    {
        //                        { "txtCity", MDVUtility.ToStr(dr["City"])},
        //                        { "txtState", MDVUtility.ToStr(dr["State"])}
        //                    };
        //                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        //                    var response = new
        //                    {
        //                        status = true,
        //                        CITYSTATE_JSON = js.Serialize(keyValues)
        //                    };
        //                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //                }
        //            }
        //        }
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}
        #endregion

        #region Provider License Info
        private string SaveProviderLicenseInfo(string fieldsJSON, Int64 ProviderId, string RowId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsProfile = new DSProfile();
                    DSProfile.ProvidersLicenseInfoRow dr = dsProfile.ProvidersLicenseInfo.NewProvidersLicenseInfoRow();

                    dr.State = SearchedfieldsJSON["txtState" + RowId];
                    dr.LicenseNo = SearchedfieldsJSON["txtLicense" + RowId];
                    dr.ProviderId = ProviderId;

                    #region Database Insertion
                    dsProfile.ProvidersLicenseInfo.AddProvidersLicenseInfoRow(dr);
                    BLObject<DSProfile> obj = BLLAdminProfileObj.InsertProviderLicenseInfo(ref dsProfile);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            ProviderLicenseInfoId = dsProfile.Tables[dsProfile.ProvidersLicenseInfo.TableName].Rows[0][dsProfile.ProvidersLicenseInfo.ProviderLicenseIdColumn.ColumnName]
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

        /// <summary>
        /// Updates the provider license information.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <param name="ProvLicenseId">The prov license identifier.</param>
        /// <returns></returns>
        private string UpdateProviderLicenseInfo(string fieldsJSON, Int64 ProvLicenseId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSProfile dsProfile = new DSProfile();
                    BLObject<DSProfile> objProfile;

                    objProfile = BLLAdminProfileObj.LoadProviderLicenseInfo(0, ProvLicenseId);
                    if (objProfile.Data != null)
                    {
                        foreach (DSProfile.ProvidersLicenseInfoRow dr in objProfile.Data.Tables[dsProfile.ProvidersLicenseInfo.TableName].Rows)
                        {
                            dr.State = SearchedfieldsJSON["txtState" + ProvLicenseId];
                            dr.LicenseNo = SearchedfieldsJSON["txtLicense" + ProvLicenseId];
                        }

                        dsProfile = objProfile.Data;

                        #region Database Updation

                        if (dsProfile.Tables[dsProfile.ProvidersLicenseInfo.TableName].Rows.Count > 0)
                        {
                            BLObject<DSProfile> obj = BLLAdminProfileObj.UpdateProviderLicenseInfo(ref dsProfile);
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
                            Message = privilegesMessage
                        };
                        return (JsonConvert.SerializeObject(response));
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Loads the provider license information.
        /// </summary>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <returns></returns>
        private string LoadProviderLicenseInfo(Int64 ProviderId)
        {
            try
            {
                DSProfile dsProfile = null;
                BLObject<DSProfile> obj;
                if (ProviderId > 0)
                {
                    obj = BLLAdminProfileObj.LoadProviderLicenseInfo(ProviderId, 0);

                    dsProfile = obj.Data;
                    var response = new
                    {
                        status = true,
                        ProviderLicenseCount = dsProfile.Tables[dsProfile.ProvidersLicenseInfo.TableName].Rows.Count,
                        ProviderLicense_JSON = MDVUtility.JSON_DataTable(dsProfile.Tables[dsProfile.ProvidersLicenseInfo.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.No_Record_Message
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

        /// <summary>
        /// Fills the provider license information.
        /// </summary>
        /// <param name="ProviderLicenseId">The provider license identifier.</param>
        /// <returns></returns>
        private string FillProviderLicenseInfo(Int64 ProviderLicenseId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProviderLicenseId)))
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
                    DSProfile dsProfile = null;
                    BLObject<DSProfile> obj = BLLAdminProfileObj.LoadProviderLicenseInfo(0, ProviderLicenseId);
                    if (obj.Data != null)
                    {
                        dsProfile = obj.Data;
                        if (dsProfile.Tables[dsProfile.ProvidersLicenseInfo.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsProfile.Tables[dsProfile.ProvidersLicenseInfo.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtState", MDVUtility.ToStr(dr[dsProfile.ProvidersLicenseInfo.StateColumn.ColumnName])},
                            { "txtLicense", MDVUtility.ToStr(dr[dsProfile.ProvidersLicenseInfo.LicenseNoColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ProviderLicenseFill_JSON = js.Serialize(keyValues)
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
        /// Deletes the provider license information.
        /// </summary>
        /// <param name="ProviderLicenseId">The provider license identifier.</param>
        /// <returns></returns>
        private string DeleteProviderLicenseInfo(Int64 ProviderLicenseId)
        {

            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProviderLicenseId)))
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
                    BLObject<string> obj = BLLAdminProfileObj.DeleteProviderLicenseInfo(MDVUtility.ToStr(ProviderLicenseId));
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Provider Detail Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ProviderData"];
                        string strJSONData = SaveProvider(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PROVIDER":
                    {
                        string strProviderId = context.Request["ProviderID"];
                        bool IsWaterMarkApplied = false;
                        if (context.Request["IsWaterMark"] != null)
                        {
                            if (context.Request["IsWaterMark"].ToString() == "0")
                            {
                                IsWaterMarkApplied = false;
                            }
                            else if (context.Request["IsWaterMark"].ToString() == "1")
                            {
                                IsWaterMarkApplied = true;
                            }
                        }
                        string NIPNumber= context.Request["NIPNumber"];
                        if (NIPNumber == "undefined") NIPNumber = null;
                        string strJSONData = FillProvider(MDVUtility.ToInt64(strProviderId), IsWaterMarkApplied, NIPNumber);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "GET_MULTIPLE_PROVIDER_INFO":
                    {
                        string strProviderIds = context.Request["ProviderIDs"];

                        string strJSONData = GetProviderInfo(strProviderIds);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_BASIC_FEE_GROUP":
                    {
                        string strEntityId = context.Request["EntityID"];
                        string strJSONData = FillBasicFeeGroup(strEntityId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_SUPERVISING_PROVIDER":
                    {
                        string strEntityId = context.Request["EntityID"];
                        string strJSONData = FillSupervisingProvider(strEntityId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_SPECIALTY":
                    {
                        string strEntityId = context.Request["EntityID"];
                        string strJSONData = FillSpecialty(strEntityId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PROVIDER":
                    {
                        string strProviderId = context.Request["ProviderID"];
                        string strJSONData = DeleteProvider(MDVUtility.ToInt64(strProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ProviderData"];
                        Int64 ProviderID = MDVUtility.ToInt64(context.Request["ProviderID"]);
                        string strJSONData = UpdateProvider(fieldsJSON, ProviderID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PROVIDER_ACTIVE_INACTIVE":
                    {
                        Int64 ProviderID = MDVUtility.ToInt64(context.Request["ProviderID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateProviderIsActive(ProviderID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                //case "CITYSTATE":
                //    {
                //        string zipcode = context.Request["zipcode"];
                //        string cityname = context.Request["cityname"];

                //        string strJSONData = GetCityState(zipcode, cityname);

                //        context.Response.ContentType = "text/plain";
                //        context.Response.Write(strJSONData);
                //    }
                //    break;
                case "SAVE_LICENSE_INFO":
                    {
                        string fieldsJSON = context.Request["ProviderLicenseData"];
                        Int64 ProviderID = MDVUtility.ToInt64(context.Request["ProviderId"]);
                        string RowId = context.Request["RowId"];
                        string strJSONData = SaveProviderLicenseInfo(fieldsJSON, ProviderID, RowId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_LICENSE_INFO":
                    {
                        string fieldsJSON = context.Request["ProviderLicenseData"];
                        Int64 ProviderLicenseID = MDVUtility.ToInt64(context.Request["ProviderLicenseID"]);
                        string strJSONData = UpdateProviderLicenseInfo(fieldsJSON, ProviderLicenseID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_LICENSE_INFO":
                    {
                        Int64 ProviderID = MDVUtility.ToInt64(context.Request["ProviderId"]);
                        string strJSONData = LoadProviderLicenseInfo(ProviderID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_LICENSE_INFO":
                    {
                        Int64 ProviderLicenseID = MDVUtility.ToInt64(context.Request["ProviderLicenseID"]);
                        string strJSONData = FillProviderLicenseInfo(ProviderLicenseID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_LICENSE_INFO":
                    {
                        Int64 ProviderLicenseID = MDVUtility.ToInt64(context.Request["ProviderLicenseID"]);
                        string strJSONData = DeleteProviderLicenseInfo(ProviderLicenseID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PROCEDURELIST_CPT":
                    {
                        Int64 ProcedureListId = MDVUtility.ToInt64(context.Request["ProcedureListId"]);
                        string strJSONData = DeleteAssociatedProcedure(ProcedureListId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_PROVIDER_NPI_APIDATA":
                    {
                        string strJSONData = LoadproviderApiData(context.Request["NIPNumber"]);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }


        public class ProviderResponseModel
        {
            public bool status { get; set; }
            public long ProviderId { get; set; }
            public Dictionary<string, string> ProviderFill_JSON { get; set; }
            public string Message { get; set; }
        }

        #endregion
    }
}
