using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;


namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_FaxSettings
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_FaxSettings()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }

        #region Singleton
        private static Admin_FaxSettings _obj = null;
        public static Admin_FaxSettings Instance()
        {
            if (_obj == null)
                _obj = new Admin_FaxSettings();
            return _obj;
        }

        #endregion
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ProviderFaxData"];
                        string strJSONData = SaveProviderFaxSettings(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);

                        
                    }
                    break;
                case "LOAD_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ProviderData"];
                        string strJSONData = LoadProviderFaxSettings(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ProviderFaxData"];
                        string strJSONData = UpdateProviderFaxSettings(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);

                    }
                    break;
                
                case "DELETE_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ProviderId"];
                        string strJSONData = DeleteProviderFaxSettings(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);


                    }
                    break;
                case "SAVE_PROVIDER_USER":
                    {
                        string fieldsJSON = context.Request["ProviderUserData"];
                        string strJSONData = InsertProviderFaxSettingsUsers(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_PROVIDER_USER":
                    {
                        string fieldsJSON = context.Request["ProviderUserData"];
                        string strJSONData = LoadProviderFaxSettingsUsers(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PROVIDER_USER":
                    {
                        string fieldsJSON = context.Request["ProviderUserData"];
                        string strJSONData = DeleteProviderFaxSettingsUsers(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_FACILITY":
                    {
                        string fieldsJSON = context.Request["FacilityFaxData"];
                        string strJSONData = SaveFacilityFaxSettings(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);


                    }
                    break;
                case "LOAD_FACILITY":
                    {
                        string fieldsJSON = context.Request["FacilityData"];
                        string strJSONData = LoadFacilityFaxSettings(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_FACILITY":
                    {
                        string fieldsJSON = context.Request["FacilityFaxData"];
                        string strJSONData = UpdateFacilityFaxSettings(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);

                    }
                    break;
                case "DELETE_FACILITY":
                    {
                        string fieldsJSON = context.Request["FacilityId"];
                        string strJSONData = DeleteFacilityFaxSettings(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);


                    }
                    break;
                case "SAVE_FACILITY_USER":
                    {
                        string fieldsJSON = context.Request["FacilityUserData"];
                        string strJSONData = InsertFacilityFaxSettingsUsers(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_FACILITY_USER":
                    {
                        string fieldsJSON = context.Request["FacilityUserData"];
                        string strJSONData = LoadFacilityFaxSettingsUsers(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_FACILITY_USER":
                    {
                        string fieldsJSON = context.Request["FacilityUserData"];
                        string strJSONData = DeleteFacilityFaxSettingsUsers(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #region "Provider Fax Settings"

        private string SaveProviderFaxSettings(string providerData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(providerData);

            DSProfile dsProfile = new DSProfile();
            DSProfile.ProviderFaxSettingsRow dr = dsProfile.ProviderFaxSettings.NewProviderFaxSettingsRow();


            
            dr.ProviderId = Convert.ToInt64(data["ProviderId"]);
            dr.DisplayName = Convert.ToString(data["txtDisplayName"]);
            dr.FirstName = Convert.ToString(data["txtFirstName"]);
            dr.MiddleName = Convert.ToString(data["txtMiddleName"]);
            dr.LastName = Convert.ToString(data["txtLastName"]);
            dr.CompanyName = Convert.ToString(data["txtCompanyName"]);
            dr.PhoneNo = Convert.ToString(data["txtPhoneNo"]);
            dr.FaxNo = Convert.ToString(data["txtFaxNo"]);
            dr.TimeZone = Convert.ToString(data["txtTimeZone_text"]);
            dr.HasCoverPage = Convert.ToBoolean(data["IsCustomCoverPage"]);
            
            dr.CoverPage = Convert.ToString(data["CoverPage"]);
            dr.Is_eSignatured = Convert.ToBoolean(data["Is_esignatured"]);

         
            if (!string.IsNullOrEmpty(data["eSignature"]))
            {
                // Checking whether default image is shown or not
                if (!data["eSignature"].Contains("default"))
                {
                    string strBase64 = data["eSignature"].Split(',')[1];
                    strBase64 = strBase64.Replace(' ', '+');
                    byte[] currentFileStream = Convert.FromBase64String(strBase64);
                    dr.eSignature = currentFileStream;
                    // dr.ImageType = SearchedfieldsJSON["imgPatient"].Split(',')[0].Split(':')[1].Split(';')[0];
                }
            }



            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;


            // inserting into database
            dsProfile.ProviderFaxSettings.AddProviderFaxSettingsRow(dr);
            string obj = BLLAdminProfileObj.InsertProviderFaxSetting(ref dsProfile);


            return "";
        }
        private string LoadProviderFaxSettings(string providerData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(providerData);

            DSProfile ds = null;
            long ProviderId = 0;


            // If provider Id is 0, it will return all rows

            // else specific provider
            var id = Convert.ToString(data["ProviderId"]);
            if (string.IsNullOrEmpty(id))
            {
                ProviderId = 0;
            }
            else
            {
                ProviderId = Convert.ToInt64(data["ProviderId"]);
            }
            


            BLObject<DSProfile> obj = BLLAdminProfileObj.LoadProviderFaxSettings(ProviderId, 1, 1000);

            if (obj.Data != null)
            {
                ds = obj.Data;
                if (ds.Tables[ds.ProviderFaxSettings.TableName].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[ds.ProviderFaxSettings.TableName].Rows[0];


                    var rows = ds.Tables[ds.ProviderFaxSettings.TableName];

                    var dataRows = MDVUtility.JSON_DataTable(rows);



                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ProviderFaxFill_JSON = dataRows
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            else
            {
                var response = new
                {
                    status = false,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string DeleteProviderFaxSettings(string providerData)
        {
            long ProviderId = Convert.ToInt64(providerData);
            string x = BLLAdminProfileObj.DeleteProviderFaxSettings(ProviderId);
            return "";
        }
        private string UpdateProviderFaxSettings(string providerData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(providerData);

            DSProfile dsProfile = null;
            long ProviderId = 0;

            ProviderId = Convert.ToInt64(data["ProviderId"]);
            //}
            BLObject<DSProfile> obj = BLLAdminProfileObj.LoadProviderFaxSettings(ProviderId, 1, 1000);
            dsProfile = obj.Data;
            if (dsProfile.Tables[dsProfile.ProviderFaxSettings.TableName].Rows.Count > 0)
            {

                foreach (DSProfile.ProviderFaxSettingsRow dr in dsProfile.Tables[dsProfile.ProviderFaxSettings.TableName].Rows)
                {
                    dr.ProviderId = Convert.ToInt64(data["ProviderId"]);
                    if (!string.IsNullOrEmpty(data["txtDisplayName"]))
                        dr.DisplayName = Convert.ToString(data["txtDisplayName"]);
                    if (!string.IsNullOrEmpty(data["txtFirstName"]))
                        dr.FirstName = Convert.ToString(data["txtFirstName"]);
                    if (!string.IsNullOrEmpty(data["txtMiddleName"]))
                        dr.MiddleName = Convert.ToString(data["txtMiddleName"]);
                    if (!string.IsNullOrEmpty(data["txtLastName"]))
                        dr.LastName = Convert.ToString(data["txtLastName"]);
                    if (!string.IsNullOrEmpty(data["txtCompanyName"]))
                        dr.CompanyName = Convert.ToString(data["txtCompanyName"]);
                    if (!string.IsNullOrEmpty(data["txtPhoneNo"]))
                        dr.PhoneNo = Convert.ToString(data["txtPhoneNo"]);
                    if (!string.IsNullOrEmpty(data["txtFaxNo"]))
                        dr.FaxNo = Convert.ToString(data["txtFaxNo"]);
                    if (!string.IsNullOrEmpty(data["txtTimeZone_text"]))
                    {
                        dr.TimeZone = Convert.ToString(data["txtTimeZone_text"]);
                    }

                    dr.HasCoverPage = Convert.ToBoolean(data["IsCustomCoverPage"]);


                    //      if (!string.IsNullOrEmpty(data[""]))
                    if (!string.IsNullOrEmpty(data["CoverPage"]))
                    {
                        dr.CoverPage = Convert.ToString(data["CoverPage"]);
                    }
                        // if (!string.IsNullOrEmpty(data["Is_esignatured"]))
                    dr.Is_eSignatured = Convert.ToBoolean(data["Is_esignatured"]);
                    // dr.eSignature = ch;

                    if (!string.IsNullOrEmpty(data["eSignature"]))
                    {
                        // Checking whether default image is shown or not
                        if (!data["eSignature"].Contains("default"))
                        {
                            string strBase64 = data["eSignature"].Split(',')[1];
                            strBase64 = strBase64.Replace(' ', '+');
                            byte[] currentFileStream = Convert.FromBase64String(strBase64);
                            dr.eSignature = currentFileStream;
                        }
                    }

                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;


                    string update = BLLAdminProfileObj.UpdateProviderFaxSettings(ref dsProfile);


                }






            }

            return "";
        }

        private string InsertProviderFaxSettingsUsers(string ProviderData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(ProviderData);

            DSProfile dsProfile = new DSProfile();
            DSProfile.ProviderFaxSettingsUsersRow dr = dsProfile.ProviderFaxSettingsUsers.NewProviderFaxSettingsUsersRow();




            if (!string.IsNullOrEmpty(data["ProviderId"]))
                dr.ProviderId = Convert.ToInt64(data["ProviderId"]);
            if (!string.IsNullOrEmpty(data["UserId"]))
                dr.UserId = Convert.ToInt64(data["UserId"]);

            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;


            // inserting into database
            dsProfile.ProviderFaxSettingsUsers.AddProviderFaxSettingsUsersRow(dr);
            string obj = BLLAdminProfileObj.InsertProviderFaxSettingUsers(ref dsProfile);


            return "";
        }
        private string LoadProviderFaxSettingsUsers(string ProviderData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(ProviderData);

            DSProfile ds = null;
            long ProviderId = 0;



            ProviderId = Convert.ToInt64(data["ProviderId"]);




            BLObject<DSProfile> obj = BLLAdminProfileObj.LoadProviderFaxSettingsUsers(ProviderId, 1, 1000);

            if (obj.Data != null)
            {
                ds = obj.Data;
                if (ds.Tables[ds.ProviderFaxSettingsUsers.TableName].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[ds.ProviderFaxSettingsUsers.TableName].Rows[0];


                    var rows = ds.Tables[ds.ProviderFaxSettingsUsers.TableName];

                    var dataRows = MDVUtility.JSON_DataTable(rows);



                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ProviderFaxFill_JSON = dataRows
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                return "";
            }
            return "";
        }
        private string DeleteProviderFaxSettingsUsers(string ProviderData)
        {

            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(ProviderData);


            long UserId = Convert.ToInt64(data["UserId"]);
            string x = BLLAdminProfileObj.DeleteProviderFaxSettingsUsers(UserId);
            return "";
        }

        #endregion

        #region "Facility Fax Settings"

        private string SaveFacilityFaxSettings(string FacilityData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(FacilityData);

            DSProfile dsProfile = new DSProfile();
            DSProfile.FacilityFaxSettingsRow dr = dsProfile.FacilityFaxSettings.NewFacilityFaxSettingsRow();



            dr.FacilityId = Convert.ToInt64(data["FacilityId"]);
            dr.DisplayName = Convert.ToString(data["txtDisplayName"]);
            dr.ShortName = Convert.ToString(data["txtShortName"]);
            dr.PhoneNo = Convert.ToString(data["txtPhoneNo"]);
            dr.FaxNo = Convert.ToString(data["txtFaxNo"]);
            dr.TimeZone = Convert.ToString(data["txtTimeZoneFacility_text"]);
            dr.HasCoverPage = Convert.ToBoolean(data["IsCustomCoverPage"]);

            dr.CoverPage = Convert.ToString(data["CoverPage"]);
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;


            // inserting into database
            dsProfile.FacilityFaxSettings.AddFacilityFaxSettingsRow(dr);
            string obj = BLLAdminProfileObj.InsertFacilityFaxSetting(ref dsProfile);


            return "";
        }
        private string LoadFacilityFaxSettings(string FacilityData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(FacilityData);

            DSProfile ds = null;
            long FacilityId = 0;


            // If Facility Id is 0, it will return all rows

            // else specific Facility
            var id = Convert.ToString(data["FacilityId"]);
            if (string.IsNullOrEmpty(id))
            {
                FacilityId = 0;
            }
            else
            {
                FacilityId = Convert.ToInt64(data["FacilityId"]);
            }



            BLObject<DSProfile> obj = BLLAdminProfileObj.LoadFacilityFaxSettings(FacilityId, 1, 1000);

            if (obj.Data != null)
            {
                ds = obj.Data;
                if (ds.Tables[ds.FacilityFaxSettings.TableName].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[ds.FacilityFaxSettings.TableName].Rows[0];


                    var rows = ds.Tables[ds.FacilityFaxSettings.TableName];

                    var dataRows = MDVUtility.JSON_DataTable(rows);



                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        FacilityFaxFill_JSON = dataRows
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            else
            {
                var response = new
                {
                    status = false,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string DeleteFacilityFaxSettings(string FacilityData)
        {
            long FacilityId = Convert.ToInt64(FacilityData);
            string x = BLLAdminProfileObj.DeleteFacilityFaxSettings(FacilityId);
            return "";
        }
        private string UpdateFacilityFaxSettings(string FacilityData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(FacilityData);

            DSProfile dsProfile = null;
            long FacilityId = 0;


            FacilityId = Convert.ToInt64(data["FacilityId"]);
            //}
            BLObject<DSProfile> obj = BLLAdminProfileObj.LoadFacilityFaxSettings(FacilityId, 1, 1000);
            dsProfile = obj.Data;
            if (dsProfile.Tables[dsProfile.FacilityFaxSettings.TableName].Rows.Count > 0)
            {

                foreach (DSProfile.FacilityFaxSettingsRow dr in dsProfile.Tables[dsProfile.FacilityFaxSettings.TableName].Rows)
                {
                    dr.FacilityId = Convert.ToInt64(data["FacilityId"]);
                    if (!string.IsNullOrEmpty(data["txtDisplayName"]))
                        dr.DisplayName = Convert.ToString(data["txtDisplayName"]);
                    if (!string.IsNullOrEmpty(data["txtShortName"]))
                        dr.ShortName = Convert.ToString(data["txtShortName"]);
                    if (!string.IsNullOrEmpty(data["txtPhoneNo"]))
                        dr.PhoneNo = Convert.ToString(data["txtPhoneNo"]);
                    if (!string.IsNullOrEmpty(data["txtFaxNo"]))
                        dr.FaxNo = Convert.ToString(data["txtFaxNo"]);
                    if (!string.IsNullOrEmpty(data["txtTimeZoneFacility_text"]))
                    {
                        dr.TimeZone = Convert.ToString(data["txtTimeZoneFacility_text"]);
                    }

                    dr.HasCoverPage = Convert.ToBoolean(data["IsCustomCoverPage"]);


                    //      if (!string.IsNullOrEmpty(data[""]))
                    dr.CoverPage = Convert.ToString(data["CoverPage"]);
                    // if (!string.IsNullOrEmpty(data["Is_esignatured"]))
                  
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;


                    string update = BLLAdminProfileObj.UpdateFacilityFaxSettings(ref dsProfile);


                }






            }

            return "";
        }

        private string InsertFacilityFaxSettingsUsers(string FacilityData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(FacilityData);

            DSProfile dsProfile = new DSProfile();
            DSProfile.FacilityFaxSettingsUsersRow dr = dsProfile.FacilityFaxSettingsUsers.NewFacilityFaxSettingsUsersRow();




            if (!string.IsNullOrEmpty(data["FacilityId"]))
                dr.FacilityId = Convert.ToInt64(data["FacilityId"]);
            if (!string.IsNullOrEmpty(data["UserId"]))
                dr.UserId = Convert.ToInt64(data["UserId"]);

            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;


            // inserting into database
            dsProfile.FacilityFaxSettingsUsers.AddFacilityFaxSettingsUsersRow(dr);
            string obj = BLLAdminProfileObj.InsertFacilityFaxSettingUsers(ref dsProfile);


            return "";
        }
        private string LoadFacilityFaxSettingsUsers(string FacilityData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(FacilityData);

            DSProfile ds = null;
            long FacilityId = 0;



            FacilityId = Convert.ToInt64(data["FacilityId"]);




            BLObject<DSProfile> obj = BLLAdminProfileObj.LoadFacilityFaxSettingsUsers(FacilityId, 1, 1000);

            if (obj.Data != null)
            {
                ds = obj.Data;
                if (ds.Tables[ds.FacilityFaxSettingsUsers.TableName].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[ds.FacilityFaxSettingsUsers.TableName].Rows[0];


                    var rows = ds.Tables[ds.FacilityFaxSettingsUsers.TableName];

                    var dataRows = MDVUtility.JSON_DataTable(rows);



                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        FacilityFaxFill_JSON = dataRows
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                return "";
            }
            return "";
        }
        private string DeleteFacilityFaxSettingsUsers(string FacilityData)
        {

            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = ser.Deserialize<dynamic>(FacilityData);


            long UserId = Convert.ToInt64(data["UserId"]);
            string x = BLLAdminProfileObj.DeleteFacilityFaxSettingsUsers(UserId);
            return "";
        }

        #endregion

    }
}