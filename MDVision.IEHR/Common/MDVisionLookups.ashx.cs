using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using MDVision.DataAccess.DAL.CCM;
using MDVision.Datasets;
using System.Data;
using System.Reflection;
using MDVision.Business.BLL;
using MDVision.Business.BCommon;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.IEHR.Security;
using MDVision.IEHR.EMR.Model.FavoriteList;
using MDVision.Model.Lookups;
using MDVision.Model.Common;
using Newtonsoft.Json;
using MDVision.Model;
using MDVision.Model.Clinical.Medical.Implantable;
using MDVision.Model.Clinical.Medical.CarePlan;
using MDVision.Model.Clinical.Medication;
using MDVision.IEHR.EMR.Helpers.Clinical.AuditReport;
using MDVision.Model.AuditableEvents;
using MDVision.Model.Admin.Codes;
using MDVision.Model.Patient;
using MDVision.Model.Clinical.Reports;

namespace MDVision.IEHR.Common
{
    /// <summary>
    /// Summary description for MDVisionLookups
    /// </summary>
    public class MDVisionLookups : IHttpHandler, IRequiresSessionState
    {

        #region Variables

        //public string EntityId { get; set; }

        #endregion

        public class NameValuePair
        {
            private string _name;
            private string _value;
            private string _Refvalue;
            private string _Refname;
            private string _IsActive;
            private string _ExName;
            private string _ExValue;
            private string _Title;
            private string _Isreferral;


            public string Name { get { return _name; } set { _name = value; } }
            public string Value { get { return _value; } set { _value = value; } }
            public string RefValue { get { return _Refvalue; } set { _Refvalue = value; } }
            public string RefName { get { return _Refname; } set { _Refname = value; } }
            public string IsActive { get { return _IsActive; } set { _IsActive = value; } }
            public string ExName { get { return _ExName; } set { _ExName = value; } }
            public string ExValue { get { return _ExValue; } set { _ExValue = value; } }
            public string Title { get { return _Title; } set { _Title = value; } }
            public string IsReferral { get { return _Isreferral; } set { _Isreferral = value; } }

            public NameValuePair() { }

            public NameValuePair(string name, string value, string Refvalue = "", string Refname = "", string IsActive = "", string Exvalue = "", string Exname = "", string Title = "",string IsReferral="")
            {
                this.Name = name;
                this.Value = value;
                this.RefValue = Refvalue;
                this.RefName = Refname;
                this.IsActive = IsActive;
                this.ExName = Exname;
                this.ExValue = Exvalue;
                this.Title = Title;
                this.IsReferral = IsReferral;
            }

        }//end class
        public void ProcessRequest(HttpContext context)
        {
            Type thisType = this.GetType();

            if (MDVSession.Current.UserLoggedIn == false && MDVSession.Current.EntityId != "" && MDVSession.Current.AppUserName != "")
            {
                string error = new UserLoginHelper().ReLogIn();
                if (error != "")
                    context.Response.Redirect(MDVSession.Current.WebEntityURL + "MDVisionLogin.aspx?error=" + error);

                //    string user_data = context.Request.QueryString["userData"];
                //    WebEMR.Common.AppUser.ReLogIn(user_data);
            }

            if (MDVSession.Current.UserLoggedIn == true && MDVSession.Current.EntityId != "" && MDVSession.Current.AppUserName != "")
            {

                //    return ;

                //BLLLogin.Instance.Login(ref ds, MDVSession.Current.AppUserName, AppConfig.AppPassWord);

                string result = null;
                Dictionary<string, object> responseResult = new Dictionary<string, object>();
                if (context.Request.QueryString["method"] != "")
                {
                    Int64 ID3 = MDVUtility.ToInt64(context.Request["ID3"]);
                    Int64 ID2 = MDVUtility.ToInt64(context.Request["ID2"]);
                    Int64 ID = MDVUtility.ToInt64(context.Request["ID"]);

                    string StrID3 = MDVUtility.ToStr(context.Request["StrID3"]);
                    string StrID2 = MDVUtility.ToStr(context.Request["StrID2"]);
                    string StrID = MDVUtility.ToStr(context.Request["StrID"]);

                    string EntityID = MDVUtility.ToStr(context.Request["entityID"]);

                    string ProviderIDs = MDVUtility.ToStr(context.Request.QueryString["ProviderIDs"]);

                    string TabId = MDVUtility.ToStr(context.Request["CategoryId"]);
                    string IsActive = "1";
                    if (context.Request["IsActive"] != null)
                    {
                        IsActive = MDVUtility.ToStr(context.Request["IsActive"]);
                    }

                    //EntityId = EntityID;
                    string Text = MDVUtility.ToStr(context.Request["Text"]);
                    object[] parameters = null;
                    Type[] arrTypes = new Type[] { };

                    //if (EntityID != 0 && Text != "")
                    //    parameters = new object[] { EntityID, Text };
                    if (StrID != "" && StrID2 != "" && StrID3 != "" && ID != 0)
                    {
                        parameters = new object[] { IsActive, StrID, StrID2, StrID3, ID };
                        arrTypes = new Type[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(Int64) };
                    }
                    else if (StrID != "" && StrID2 != "")
                    {
                        parameters = new object[] { IsActive, StrID, StrID2 };
                        arrTypes = new Type[] { typeof(string), typeof(string), typeof(string) };
                    }

                    else if (StrID != "")
                    {
                        parameters = new object[] { IsActive, StrID };
                        arrTypes = new Type[] { typeof(string), typeof(string) };
                    }
                    else if (ID != 0 && ID2 != 0 && ID3 != 0)
                    {
                        parameters = new object[] { IsActive, ID, ID2, ID3 };
                        arrTypes = new Type[] { typeof(string), typeof(Int64), typeof(Int64), typeof(Int64) };
                    }

                    else if (ID != 0 && ID2 != 0)
                    {
                        parameters = new object[] { IsActive, ID, ID2 };
                        arrTypes = new Type[] { typeof(string), typeof(Int64), typeof(Int64) };
                    }

                    else if (ID != 0 && ID2 == 0)
                    {
                        parameters = new object[] { IsActive, ID };
                        arrTypes = new Type[] { typeof(string), typeof(Int64) };
                    }

                    else if (Text != "")
                    {
                        parameters = new object[] { IsActive, EntityID, Text };
                        arrTypes = new Type[] { typeof(string), typeof(string), typeof(string) };
                    }

                    else if (EntityID != "")
                    {
                        parameters = new object[] { IsActive, EntityID };
                        arrTypes = new Type[] { typeof(string), typeof(string) };
                    }
                    else if (ProviderIDs != "")
                    {
                        parameters = new object[] { ProviderIDs };
                        arrTypes = new Type[] { typeof(string) };
                    }
                    else if (TabId != "")
                    {
                        parameters = new object[] { IsActive, TabId };
                        arrTypes = new Type[] { typeof(string), typeof(string) };
                    }
                    else
                    {
                        parameters = new object[] { IsActive };
                        arrTypes = new Type[] { typeof(string) };
                    }
                    string queryString = context.Request.QueryString["method"];
                    foreach (var methodname in queryString.Split(','))
                    {
                        try
                        {
                            MethodInfo theMethod = null;
                            if (parameters.Length == 0)
                            {
                                theMethod = thisType.GetMethod(methodname);
                            }
                            else
                                theMethod = thisType.GetMethod(methodname, arrTypes);

                            //ParameterInfo[] arrparameters =theMethod.GetParameters();
                            if (theMethod != null)
                            {
                                if (parameters.Length == 0)
                                    result = (string)theMethod.Invoke(this, null);
                                else
                                    result = (string)theMethod.Invoke(this, parameters);

                                responseResult.Add(methodname, result);
                            }
                            MDVLogger.PresentationLog("LOOKUP", methodname, "", MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), "lookup");
                        }
                        catch (Exception ex)
                        {
                            MDVLogger.PresentationErrorLog(methodname, ex, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());

                            ex.Message.ToString();
                        }
                    }
                }
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;

                context.Response.ContentType = "text/plain";
                context.Response.Write(js.Serialize(responseResult));

            }
            //else
            //       context.Response.Redirect(AppConfig.WebEntityURL + "MDVisionLogin.aspx?error=Your session has expired. Please log in again");


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        //public string GetSupervisingProvider()
        //{
        //    HashSet<NameValuePair> list = new HashSet<NameValuePair>();

        //    //DSProfile ds = DALProvider.Instance.LoadProvider(0, "", "", "", "", "", "");

        //    BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupProvider();
        //    DSProfileLookup ds = obj.Data;

        //    list.Add(new NameValuePair("- Select -", ""));
        //    if (ds != null)
        //    {
        //        if (ds.Tables[ds.Provider.TableName] != null)
        //        {
        //            DataRow[] dRows = ds.Tables[ds.Provider.TableName].Select("1=1", ds.Provider.ShortNameColumn.ColumnName);

        //            foreach (DataRow dr in dRows)
        //            {
        //                list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString()));
        //            }
        //        }
        //    }

        //    return getJSONofList(list);
        //}

        //public string GetCity()
        //{
        //    HashSet<NameValuePair> list = new HashSet<NameValuePair>();


        //    BLObject<DSProfileLookup> objCity = BusinessWrapper.BLAdminCityStateZip.BusinessObj.LookupCity();
        //    DSProfileLookup ds = objCity.Data;
        //    list.Add(new NameValuePair("- Select -", ""));
        //    if (ds != null)
        //    {

        //        if (ds.Tables[ds.CityName.TableName] != null)
        //        {
        //            DataRow[] dRows = ds.Tables[ds.CityName.TableName].Select("1=1", ds.CityName.CITYSTATENAMEColumn.ColumnName);

        //            foreach (DataRow dr in dRows)
        //            {
        //                list.Add(new NameValuePair(dr[ds.CityName.CITYSTATENAMEColumn.ColumnName].ToString(), dr[ds.CityName.CITYSTATENAMEColumn.ColumnName].ToString()));
        //            }
        //        }
        //    }
        //    return getJSONofList(list);
        //}
        //public string GetStates()
        //{
        //    HashSet<NameValuePair> list = new HashSet<NameValuePair>();


        //    BLObject<DSProfileLookup> objState = BusinessWrapper.BLAdminCityStateZip.BusinessObj.LookupState();
        //    DSProfileLookup ds = objState.Data;
        //    list.Add(new NameValuePair("- Select -", ""));
        //    if (ds != null)
        //    {

        //        if (ds.Tables[ds.StateName.TableName] != null)
        //        {
        //            DataRow[] dRows = ds.Tables[ds.StateName.TableName].Select("1=1", ds.StateName.STATEABBREVColumn.ColumnName);

        //            foreach (DataRow dr in dRows)
        //            {
        //                list.Add(new NameValuePair(dr[ds.StateName.STATEABBREVColumn].ToString(), dr[ds.StateName.STATEABBREVColumn].ToString()));
        //            }
        //        }
        //    }
        //    return getJSONofList(list);
        //}
        //public string GetCountry()
        //{
        //    HashSet<NameValuePair> list = new HashSet<NameValuePair>();


        //    BLObject<DSProfileLookup> objCountry = BusinessWrapper.BLAdminCityStateZip.BusinessObj.LookupCity();
        //    DSProfileLookup ds = objCountry.Data;
        //    list.Add(new NameValuePair("- Select -", ""));
        //    if (ds != null)
        //    {

        //        if (ds.Tables[ds.CityName.TableName] != null)
        //        {
        //            DataRow[] dRows = ds.Tables[ds.CountryName.TableName].Select("1=1", ds.CountryName.ShortNameColumn.ColumnName);

        //            foreach (DataRow dr in dRows)
        //            {
        //                list.Add(new NameValuePair(dr[ds.CountryName.ShortNameColumn.ColumnName].ToString(), dr[ds.CountryName.ShortNameColumn.ColumnName].ToString()));
        //            }
        //        }
        //    }
        //    return getJSONofList(list);
        //}
        private static string getJSONofList(HashSet<NameValuePair> list)
        {
            return JsonConvert.SerializeObject(list);

        }

        #region Admin Lookups

        public string GetEntity(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            //DSProfile ds = DALEntity.Instance.LoadEntity(0, "");
            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupEntity();
            DSProfileLookup ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.OrganizationEntity.TableName != null)
                {
                    DSProfileLookup.OrganizationEntityRow[] dRows = (DSProfileLookup.OrganizationEntityRow[])ds.OrganizationEntity.Select("1=1", ds.OrganizationEntity.ShortNameColumn.ColumnName);

                    foreach (DSProfileLookup.OrganizationEntityRow dr in dRows.OrderBy(dr => dr.EntityId))
                    {
                        list.Add(new NameValuePair(dr.ShortName, dr.EntityId.ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetCoWorkersGroups(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSUsers> obj = new BLLAdminProfile().LookupCoWorker();
            DSUsers ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.CoWorkersGroup.TableName != null)
                {
                    DSUsers.CoWorkersGroupRow[] dRows = (DSUsers.CoWorkersGroupRow[])ds.CoWorkersGroup.Select("1=1", ds.CoWorkersGroup.NameColumn.ColumnName);
                    foreach (DSUsers.CoWorkersGroupRow dr in dRows.OrderBy(dr => dr.CoWorkersGroupId))
                    {
                        list.Add(new NameValuePair(dr.Name, dr.CoWorkersGroupId.ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the place of service for dropdown.
        /// </summary>
        /// <returns></returns>
        public string GetPlaceOfService(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSCodeLookup> obj = new BLLAdminCodes().LookupPlaceOfService();
            DSCodeLookup ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (obj.Data != null)
            {

                if (ds.Tables[ds.PlaceOfService.TableName] != null)
                {
                    DSCodeLookup.PlaceOfServiceRow[] dRows = (DSCodeLookup.PlaceOfServiceRow[])ds.PlaceOfService.Select("1=1", ds.PlaceOfService.POSCodeColumn.ColumnName);

                    foreach (DSCodeLookup.PlaceOfServiceRow dr in dRows.OrderBy(dr => dr.POSId))
                    {
                        list.Add(new NameValuePair(dr.POSCode + " - " + dr.Description, dr.POSId.ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the type of service for dropdown.
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfService(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSCodeLookup> obj = new BLLAdminCodes().LookupTypeOfService(IsActive);
            DSCodeLookup ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (obj.Data != null)
            {

                if (ds.TypeOfService != null)
                {
                    DSCodeLookup.TypeOfServiceRow[] dRows = (DSCodeLookup.TypeOfServiceRow[])ds.TypeOfService.Select("1=1", ds.TypeOfService.TypeOfServiceCodeColumn.ColumnName);

                    foreach (DSCodeLookup.TypeOfServiceRow dr in dRows.OrderBy(dr => dr.TOSId))
                    {
                        list.Add(new NameValuePair(dr.TypeOfServiceCode + " - " + dr.Name, dr.TOSId.ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the procedure category for dropdown.
        /// </summary>
        /// <returns></returns>
        public string GetProcedureCategory(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSCodeLookup> obj = new BLLAdminCodes().LookupProcedureCategory(IsActive);
            DSCodeLookup ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (obj.Data != null)
            {

                if (ds.Tables[ds.ProcedureCategory.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ProcedureCategory.TableName].Select("1=1", ds.ProcedureCategory.NameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ProcedureCategory.ProcCategoryIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ProcedureCategory.NameColumn.ColumnName].ToString(), dr[ds.ProcedureCategory.ProcCategoryIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the Improvement Activity for dropdown.
        /// </summary>
        /// <returns></returns>
        public string GetImprovementActivity(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSiTrack> obj = new BLLAdminCodes().LookupImprovementActivity(IsActive);
            DSiTrack ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (obj.Data != null)
            {

                if (ds.Tables[ds.DT_IA_Lookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.DT_IA_Lookup.TableName].Select("1=1", ds.DT_IA_Lookup.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.DT_IA_Lookup.MeasureIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.DT_IA_Lookup.ShortNameColumn.ColumnName].ToString(), dr[ds.DT_IA_Lookup.MeasureIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the NDC Mesurement Code for dropdown.
        /// </summary>
        /// <returns></returns>
        public string GetNDCMeasurementCode(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSCodeLookup> obj = new BLLAdminCodes().LookupNDCMeasurementCode();
            DSCodeLookup ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (obj.Data != null)
            {

                if (ds.Tables[ds.NDCMeasurementCode.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.NDCMeasurementCode.TableName].Select("1=1", ds.NDCMeasurementCode.CodeColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.NDCMeasurementCode.NDCMeasurementIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.NDCMeasurementCode.CodeColumn.ColumnName].ToString(), dr[ds.NDCMeasurementCode.NDCMeasurementIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the practices for dropdown.
        /// </summary>
        /// <returns></returns>
        public string GetPractice(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProfileLookup> objPractice = new BLLAdminProfile().LookupPractice(IsActive);
            // DSProfile ds = DALPractice.Instance.LoadPractice(0, "", "", "", "");
            DSProfileLookup ds = objPractice.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Practice.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Practice.TableName].Select("1=1", ds.Practice.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Practice.ShortNameColumn.ColumnName].ToString(), dr[ds.Practice.PracticeIdColumn.ColumnName].ToString(), dr[ds.Practice.EntityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetAppointmentStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSScheduleLookups> objPractice = new BLLSchedule().LookupAppointmentStatus();
            DSScheduleLookups ds = objPractice.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.AppointmentStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.AppointmentStatus.TableName].Select("1=1", ds.AppointmentStatus.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.AppointmentStatus.ShortNameColumn.ColumnName].ToString(), dr[ds.AppointmentStatus.AppointmentIdColumn.ColumnName].ToString(), dr[ds.AppointmentStatus.ColorColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the gender for dropdown.
        /// </summary>
        /// <returns></returns>
        public string GetGender(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();



            BLObject<DSCodes> objGender = new BLLAdminCodes().LookupGender();
            DSCodes ds = objGender.Data;
            //list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.gender.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.gender.TableName].Select("1=1");

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.gender.idColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.gender.GenderNameColumn.ColumnName].ToString(), dr[ds.gender.idColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetProviderParticipentStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var objParticipentProvider = new BLLAdminCodes().LookupParticipentProvider();
            list.Add(new NameValuePair("- Select -", ""));
            foreach (var item in objParticipentProvider.Data)
            {
                list.Add(new NameValuePair(item.ShortName, item.ProviderParticipentStatusId));
            }
            return getJSONofList(list);
        }
        public string GetCaseAdjuster(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var objParticipentProvider = new BLLAdminCodes().LookupCaseAdjuster();
            list.Add(new NameValuePair("- Select -", ""));
            foreach (var item in objParticipentProvider.Data)
            {
                list.Add(new NameValuePair(item.LastName + ", " + item.FirstName, item.CaseAdjusterId));
            }
            return getJSONofList(list);
        }
        public string GetGenderDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            var model = new BLLAdminCodes().LookupGenderDemographic();


            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.GenderName, item.Id));
                    }
                }

            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Author: Ahmad Raza
        /// Date: 19-08-2016
        /// FunctionName: GetPQRSTreatmentType
        /// Description: Get PQRS Treatment Type values for drop down
        /// </summary>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public string GetPQRSTreatmentType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLAdminCodes().LookupPQRSTreatmentType();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));

            if (ds != null)
            {
                foreach (var item in ds)
                {
                    list.Add(new NameValuePair(item.Description, item.Id));
                }
            }

            return getJSONofList(list);
        }

        /// <summary>
        /// Author: Ahmad Raza
        /// Date: 19-08-2016
        /// FunctionName: GetPQRSReasonType
        /// Description: Get PQRS ReasonType values for drop down
        /// </summary>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public string GetPQRSReasonType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLAdminCodes().LookupPQRSReasonType();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));

            if (ds != null)
            {
                foreach (var item in ds)
                {
                    list.Add(new NameValuePair(item.Description, item.Id));
                }
            }

            return getJSONofList(list);
        }

        //public string GetState()
        //{
        //    HashSet<NameValuePair> list = new HashSet<NameValuePair>();

        //    //DSCodes ds = DALCodes.Instance.LoadPlaceOfService(0);
        //    //list.Add(new NameValuePair("- Select -", ""));
        //    //if (ds.Tables[ds.PlaceOfService.TableName] != null)
        //    //{
        //    //    DataRow[] dRows = ds.Tables[ds.PlaceOfService.TableName].Select("1=1", ds.PlaceOfService.ServiceNameColumn.ColumnName);

        //    //    foreach (DataRow dr in dRows)
        //    //    {
        //    //        list.Add(new NameValuePair(dr[ds.PlaceOfService.ServiceNameColumn.ColumnName].ToString(), dr[ds.PlaceOfService.POSIdColumn.ColumnName].ToString()));
        //    //    }
        //    //}

        //    return getJSONofList(list);
        //}


        public string GetProvider(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> objProvider = new BLLAdminProfile().LookupProvider(IsActive);
            DSProfileLookup ds = objProvider.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Provider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Provider.TableName].Select("1=1", ds.Provider.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Provider.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString(), dr[ds.Provider.IsActiveColumn.ColumnName].ToString(), dr[ds.Provider.SpecialityNameColumn.ColumnName].ToString()));

                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetProviderWithQualification(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> objProvider = new BLLAdminProfile().LookupProviderWithQualification(IsActive);
            DSProfileLookup ds = objProvider.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Provider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Provider.TableName].Select("1=1", ds.Provider.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Provider.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString(), dr[ds.Provider.IsActiveColumn.ColumnName].ToString(), dr[ds.Provider.SpecialityNameColumn.ColumnName].ToString()));

                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetAllProviders(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> objProvider = new BLLAdminProfile().LookupAllProviders(IsActive);
            DSProfileLookup ds = objProvider.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Provider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Provider.TableName].Select("1=1", ds.Provider.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Provider.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString(), dr[ds.Provider.IsActiveColumn.ColumnName].ToString(), dr[ds.Provider.SpecialityNameColumn.ColumnName].ToString()));

                    }
                }
            }
            return getJSONofList(list);
        }


        public string GetAnesthesiologist(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> objProvider = new BLLAdminProfile().LookupAnesthesiologist(IsActive);
            DSProfileLookup ds = objProvider.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Provider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Provider.TableName].Select("1=1", ds.Provider.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Provider.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString(), dr[ds.Provider.IsActiveColumn.ColumnName].ToString(), dr[ds.Provider.SpecialityNameColumn.ColumnName].ToString()));

                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetCRNA(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> objProvider = new BLLAdminProfile().LookupCRNA(IsActive);
            DSProfileLookup ds = objProvider.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Provider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Provider.TableName].Select("1=1", ds.Provider.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Provider.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString(), dr[ds.Provider.IsActiveColumn.ColumnName].ToString(), dr[ds.Provider.SpecialityNameColumn.ColumnName].ToString()));

                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetMemberProvidersWithTIN(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> objProvider = new BLLAdminProfile().LookupProvider(IsActive, true);
            DSProfileLookup ds = objProvider.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Provider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Provider.TableName].Select("1=1 ", ds.Provider.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Provider.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));

                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetProviderEntityBased(string IsActive, string EntityId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> objProvider = new BLLAdminProfile().LookupProviderEntityBased(IsActive, EntityId, false);
            DSProfileLookup ds = objProvider.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null && EntityId != "")
            {

                if (ds.Tables[ds.Provider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Provider.TableName].Select("1=1", ds.Provider.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Provider.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));

                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetEntityProvider(string IsActive, string EntityId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> objProvider = new BLLAdminProfile().LookupProvider(IsActive, EntityId, false);
            DSProfileLookup ds = objProvider.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null && EntityId != "")
            {

                if (ds.Tables[ds.Provider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Provider.TableName].Select("1=1", ds.Provider.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Provider.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString(), null, dr[ds.Provider.SpecialityNameColumn.ColumnName].ToString()));

                    }
                }
            }
            return getJSONofList(list);
        }


        public string GetCustomFormsByProvider(string ProviderIDs)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            List<MDVision.Model.CustomFormModel> ProvidersList = null;
            BLObject<List<MDVision.Model.CustomFormModel>> obj = new BLLClinical().LookupCustomFormsByProvider(ProviderIDs);

            list.Add(new NameValuePair("- Select -", ""));
            ProvidersList = obj.Data;
            if (obj.Data != null)
            {
                if (ProvidersList != null)
                {
                    foreach (MDVision.Model.CustomFormModel item in ProvidersList)
                    {
                        list.Add(new NameValuePair(item.FormName, item.CustomFormId));
                    }
                }
            }
            return getJSONofList(list);

        }


        public string GetNotesProviders(string IsActive, string patientId)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            List<ProfileLookupModel> ProvidersList = null;
            BLObject<List<ProfileLookupModel>> obj = new BLLAdminProfile().LookupNotesProviders(patientId);

            list.Add(new NameValuePair("- Select -", ""));
            ProvidersList = obj.Data;
            if (obj.Data != null)
            {
                if (ProvidersList != null)
                {
                    foreach (ProfileLookupModel item in ProvidersList)
                    {
                        list.Add(new NameValuePair(item.ShortName, item.ProviderId));
                    }
                }
            }
            return getJSONofList(list);












        }
        /// <summary>
        /// Gets the reference providers.
        /// </summary>
        /// <returns></returns>
        public string GetRefProviders(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupReferringProvider(IsActive);
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ReferringProvider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ReferringProvider.TableName].Select("1=1", ds.ReferringProvider.FirstNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ReferringProvider.FirstNameColumn.ColumnName].ToString(), dr[ds.ReferringProvider.ReferringProviderIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetRefProviders(string IsActive, string StrID)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupReferringProvider(IsActive, StrID);
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ReferringProvider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ReferringProvider.TableName].Select("1=1", ds.ReferringProvider.FirstNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ReferringProvider.FirstNameColumn.ColumnName].ToString(), dr[ds.ReferringProvider.ReferringProviderIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetRefProvidersOutgoingReferral(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupReferringProviderOutgoing(IsActive);
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ReferringProvider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ReferringProvider.TableName].Select("1=1", ds.ReferringProvider.FirstNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ReferringProvider.FirstNameColumn.ColumnName].ToString() + " (" + dr[ds.ReferringProvider.EntityNameColumn.ColumnName].ToString() + ")", dr[ds.ReferringProvider.ReferringProviderIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetBillingProviders(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupLoopBillingProvider(IsActive);
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BillingProvider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.BillingProvider.TableName].Select("1=1", ds.BillingProvider.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BillingProvider.ShortNameColumn.ColumnName].ToString(), dr[ds.BillingProvider.BillingProviderIdColumn.ColumnName].ToString(), dr[ds.BillingProvider.EntityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetInsurance(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> objInsurance = new BLLAdminProfile().LookupInsurance(IsActive);
            DSProfileLookup ds = objInsurance.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Insurance.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Insurance.TableName].Select("1=1", ds.Insurance.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Insurance.InsuranceIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Insurance.ShortNameColumn.ColumnName].ToString(), dr[ds.Insurance.InsuranceIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        //public string GetTypeOfService()
        //{
        //    HashSet<NameValuePair> list = new HashSet<NameValuePair>();

        //    BLObject<DSProfileLookup> objTypeOfService = new BLLAdminCodes().LookupTypeOfService();
        //    DSProfileLookup ds = objTypeOfService.Data;
        //    list.Add(new NameValuePair("- Select -", ""));
        //    if (ds != null)
        //    {

        //        if (ds.Tables[ds.TypeOfService.TableName] != null)
        //        {
        //            DataRow[] dRows = ds.Tables[ds.TypeOfService.TableName].Select("1=1", ds.TypeOfService.NameColumn.ColumnName);

        //            foreach (DataRow dr in dRows)
        //            {
        //                list.Add(new NameValuePair(dr[ds.TypeOfService.NameColumn.ColumnName].ToString(), dr[ds.TypeOfService.TypeOfServiceCodeColumn].ToString()));
        //            }
        //        }
        //    }
        //    return getJSONofList(list);
        //}
        public string GetRoles(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            //this will return only regular User Roles
            bool IsEmergencyRole = false;
            BLObject<DSProfileLookup> objRoles = new BLLAdminProfile().LookupRoles(IsActive, IsEmergencyRole);
            DSProfileLookup ds = objRoles.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Roles.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Roles.TableName].Select("1=1", ds.Roles.RoleNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Roles.RoleIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Roles.RoleNameColumn.ColumnName].ToString(), dr[ds.Roles.RoleIdColumn.ColumnName].ToString(), dr[ds.Roles.IsAdminColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string AuditReportRoles(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            //this will return only regular User Roles
            bool IsEmergencyRole = false;
            BLObject<DSProfileLookup> objRoles = new BLLAdminProfile().LookupAuditReportRoles(IsActive, IsEmergencyRole);
            DSProfileLookup ds = objRoles.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Roles.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Roles.TableName].Select("1=1", ds.Roles.RoleNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Roles.RoleIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Roles.RoleNameColumn.ColumnName].ToString(), dr[ds.Roles.RoleIdColumn.ColumnName].ToString(), dr[ds.Roles.IsAdminColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetEmergencyRoles(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            //this will return only Emergency User Roles
            bool IsEmergencyRole = true;
            BLObject<DSProfileLookup> objRoles = new BLLAdminProfile().LookupRoles(IsActive, IsEmergencyRole);
            DSProfileLookup ds = objRoles.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Roles.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Roles.TableName].Select("1=1", ds.Roles.RoleNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Roles.RoleIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Roles.RoleNameColumn.ColumnName].ToString(), dr[ds.Roles.RoleIdColumn.ColumnName].ToString(), dr[ds.Roles.IsAdminColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetUsers(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSUserLookup> objUser = new BLLAdminSecurity().LookupUser(IsActive);
            DSUserLookup ds = objUser.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Users.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Users.TableName].Select("1=1", ds.Users.UserNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Users.UserNameColumn.ColumnName].ToString(), dr[ds.Users.UserIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetUsersFullName(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSUserLookup> objUser = new BLLAdminSecurity().LookupUser(IsActive);
            DSUserLookup ds = objUser.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Users.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Users.TableName].Select("1=1", ds.Users.UserFullNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Users.UserFullNameColumn.ColumnName].ToString(), dr[ds.Users.UserIdColumn.ColumnName].ToString(), dr[ds.Users.UserNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetUsersForCoWorker(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSUserLookup> objUser = new BLLAdminSecurity().LookupUsersForCoWorker();
            DSUserLookup ds = objUser.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Users.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Users.TableName].Select("1=1", ds.Users.UserNameColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Users.UserFullNameColumn.ColumnName].ToString(), dr[ds.Users.UserIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetCoWorkerGroupUsers(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSUserLookup> objUser = new BLLAdminSecurity().LookupCoWorkerGroupUser();
            DSUserLookup ds = objUser.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Users.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Users.TableName].Select("1=1", ds.Users.UserNameColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Users.UserNameColumn.ColumnName].ToString(), dr[ds.Users.UserIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetCoWorkerGroupUsersFullName(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSUserLookup> objUser = new BLLAdminSecurity().LookupCoWorkerGroupUser();
            DSUserLookup ds = objUser.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Users.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Users.TableName].Select("1=1", ds.Users.UserFullNameColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Users.UserFullNameColumn.ColumnName].ToString(), dr[ds.Users.UserIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetUsers(string IsActive, Int64 AllUsers)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSUserLookup> objUser = new BLLAdminSecurity().LookupUser(IsActive, AllUsers.ToString());
            DSUserLookup ds = objUser.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Users.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Users.TableName].Select("1=1", ds.Users.UserNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Users.UserNameColumn.ColumnName].ToString(), dr[ds.Users.UserIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetUsersFullName(string IsActive, Int64 AllUsers)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSUserLookup> objUser = new BLLAdminSecurity().LookupUser(IsActive, AllUsers.ToString());
            DSUserLookup ds = objUser.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Users.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Users.TableName].Select("1=1", ds.Users.UserFullNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Users.UserFullNameColumn.ColumnName].ToString(), dr[ds.Users.UserIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetProviderType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> objProviderType = new BLLAdminProfile().LookupProviderType();
            DSProfileLookup ds = objProviderType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.ProviderType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ProviderType.TableName].Select("1=1", ds.ProviderType.TypeColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ProviderType.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ProviderType.TypeColumn.ColumnName].ToString(), dr[ds.ProviderType.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetProfileType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> objProfileType = new BLLAdminProfile().LookupProfileType();
            DSProfileLookup ds = objProfileType.Data;
            //list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.ProfileType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ProfileType.TableName].Select("1=1");

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ProfileType.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ProfileType.TypeColumn.ColumnName].ToString(), dr[ds.ProfileType.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the specialty.
        /// </summary>
        /// <returns></returns>
        public string GetSpecialty(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();


            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupSpecialty(IsActive);
            DSProfileLookup ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Specialty.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Specialty.TableName].Select("1=1", ds.Specialty.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Specialty.ShortNameColumn.ColumnName].ToString(), dr[ds.Specialty.SpecialtyIdColumn.ColumnName].ToString(), dr[ds.Specialty.EntityIdColumn.ColumnName].ToString(), dr[ds.Specialty.EntityNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetSpecialtyDescription(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();


            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupSpecialty(IsActive);
            DSProfileLookup ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Specialty.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Specialty.TableName].Select("1=1", ds.Specialty.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Specialty.DescriptionColumn.ColumnName].ToString(), dr[ds.Specialty.SpecialtyIdColumn.ColumnName].ToString(), dr[ds.Specialty.EntityIdColumn.ColumnName].ToString(), dr[ds.Specialty.EntityNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetSpecialtiesAllEntities(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();


            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupSpecialtiesAllEntities(IsActive);
            DSProfileLookup ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Specialty.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Specialty.TableName].Select("1=1", ds.Specialty.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Specialty.ShortNameColumn.ColumnName].ToString(), dr[ds.Specialty.SpecialtyIdColumn.ColumnName].ToString(), dr[ds.Specialty.EntityIdColumn.ColumnName].ToString(), dr[ds.Specialty.EntityNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetSpecialtiesAllEntitiesReferals(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();


            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupSpecialtiesAllEntities(IsActive);
            DSProfileLookup ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Specialty.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Specialty.TableName].Select("1=1", ds.Specialty.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Specialty.DescriptionColumn.ColumnName].ToString(), dr[ds.Specialty.SpecialtyIdColumn.ColumnName].ToString(), dr[ds.Specialty.EntityIdColumn.ColumnName].ToString(), dr[ds.Specialty.EntityNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetSpecialty(string IsActive, string EntityId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupSpecialty(IsActive, EntityId);
            DSProfileLookup ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Specialty.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Specialty.TableName].Select("1=1", ds.Specialty.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Specialty.ShortNameColumn.ColumnName].ToString(), dr[ds.Specialty.SpecialtyIdColumn.ColumnName].ToString(), dr[ds.Specialty.EntityIdColumn.ColumnName].ToString(), dr[ds.Specialty.EntityNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetFacilityType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupFacilityType();
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FacilityType.TableName] != null)
                {


                    DataRow[] dRows = ds.Tables[ds.FacilityType.TableName].Select("1=1", ds.FacilityType.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.FacilityType.FacilityTypeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.FacilityType.FacilityCodeColumn.ColumnName].ToString() + " - " + dr[ds.FacilityType.DescriptionColumn.ColumnName].ToString(), dr[ds.FacilityType.FacilityTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetFacilityLocation(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupLocation();
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Location.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Location.TableName].Select("1=1", ds.Location.LocationColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Location.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Location.LocationColumn.ColumnName].ToString(), dr[ds.Location.IdColumn.ColumnName].ToString(), dr[ds.Location.FacilityTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetFacility(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupFacility(IsActive);
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Facility.TableName] != null)
                {
                    foreach (DataRow dr in ds.Facility.Rows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                        {
                            //if (_EntityId == null)
                            list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Facility.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.PracticeIdColumn].ToString(), dr[ds.Facility.PracticeColumn].ToString(), dr[ds.Facility.IsActiveColumn].ToString(), dr[ds.Facility.FacilityDescriptionColumn].ToString(), dr[ds.Facility.OrgIdColumn].ToString(), dr[ds.Facility.ColorColumn].ToString()));
                            //else
                            //    list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Facility.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn].ToString()));
                        }
                        else
                        {
                            //if (_EntityId == null)
                            list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.PracticeIdColumn].ToString(), dr[ds.Facility.PracticeColumn].ToString(), dr[ds.Facility.IsActiveColumn].ToString(), dr[ds.Facility.FacilityDescriptionColumn].ToString(), dr[ds.Facility.OrgIdColumn].ToString(), dr[ds.Facility.ColorColumn].ToString()));
                            //else
                            //    list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn].ToString()));
                        }


                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetFacilitySchedular(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupFacilitySchedular(IsActive);
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Facility.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Facility.TableName].Select("1=1", ds.Facility.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                        {
                            //if (_EntityId == null)
                            list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Facility.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.PracticeIdColumn].ToString(), dr[ds.Facility.PracticeColumn].ToString(), dr[ds.Facility.FacilityDescriptionColumn].ToString()));
                            //else
                            //    list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Facility.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn].ToString()));
                        }
                        else
                        {
                            //if (_EntityId == null)
                            list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.PracticeIdColumn].ToString(), dr[ds.Facility.PracticeColumn].ToString(), dr[ds.Facility.IsActiveColumn].ToString(), dr[ds.Facility.FacilityDescriptionColumn].ToString()));
                            //else
                            //    list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn].ToString()));
                        }


                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetFacilitySchedular(string IsActive, string _EntityId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupFacilitySchedular(IsActive, _EntityId);
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Facility.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Facility.TableName].Select("1=1", ds.Facility.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                        {
                            if (_EntityId == null)
                                list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Facility.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.PracticeIdColumn].ToString(), dr[ds.Facility.PracticeColumn].ToString()));
                            else
                                list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Facility.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn].ToString()));
                        }
                        else
                        {
                            //if (_EntityId == null)
                            list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn.ColumnName].ToString()));
                            //else
                            //    list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn].ToString()));
                        }


                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetAntimicrobials(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSRadiologyOrderLookup> objAntimicrobial = new BLLReports().LookupAntimicrobials();
            DSRadiologyOrderLookup ds = objAntimicrobial.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Antimicrobial.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Antimicrobial.TableName].Select("1=1", ds.Antimicrobial.CodeColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Antimicrobial.CodeColumn.ColumnName].ToString() + "-" + dr[ds.Antimicrobial.DescriptionColumn.ColumnName].ToString(), dr[ds.Antimicrobial.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        //Begin Edit by Fahad Malik 19-Dec-2016, Bug# EMR-2277
        public string GetFacilityOutgoingReferral(string IsActive)
        {

            //DSProfile dsEntity = null;
            //BLObject<DSProfile> objFacility;
            //objFacility = BLLAdminProfileObj.LoadFacilityOutgoingReferral(0, "", "", "", "","1");
            //dsEntity = objFacility.Data;
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupFacilityOutgoingReferral(IsActive);
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Facility.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Facility.TableName].Select("1=1", ds.Facility.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                        {
                            //if (_EntityId == null)
                            list.Add(new NameValuePair(dr[ds.Facility.DescriptionColumn.ColumnName].ToString() + " - " + dr[ds.Facility.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.PracticeIdColumn].ToString(), dr[ds.Facility.PracticeColumn].ToString()));
                            //else
                            //    list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Facility.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn].ToString()));
                        }
                        else
                        {
                            //if (_EntityId == null)
                            list.Add(new NameValuePair(dr[ds.Facility.DescriptionColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.PracticeIdColumn].ToString(), dr[ds.Facility.PracticeColumn].ToString(), dr[ds.Facility.IsActiveColumn].ToString()));
                            //else
                            //    list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn].ToString()));
                        }


                    }
                }
            }
            return getJSONofList(list);
        }
        //End Edit by Fahad Malik 19-Dec-2016, Bug# EMR-2277
        public string GetFacility(string IsActive, string _EntityId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupFacility(IsActive, _EntityId);
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Facility.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Facility.TableName].Select("1=1", ds.Facility.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                        {
                            if (_EntityId == null)
                                list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Facility.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.PracticeIdColumn].ToString(), dr[ds.Facility.PracticeColumn].ToString()));
                            else
                                list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Facility.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn].ToString()));
                        }
                        else
                        {
                            //if (_EntityId == null)
                            list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn.ColumnName].ToString()));
                            //else
                            //    list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn].ToString()));
                        }


                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the Fee Group.
        /// </summary>
        /// <returns></returns>
        public string GetFeeGroup(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFeeScheduleLookup> objFeeGroup = new BLLFeeSchedule().LookupFeeGroup();
            DSFeeScheduleLookup ds = objFeeGroup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FeeGroup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.FeeGroup.TableName].Select("1=1", ds.FeeGroup.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.FeeGroup.EntityNameColumn.ColumnName].ToString() + " - " + dr[ds.FeeGroup.ShortNameColumn.ColumnName].ToString(), dr[ds.FeeGroup.FeeGroupIdColumn.ColumnName].ToString(), dr[ds.FeeGroup.EntityIdColumn].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the Fee Group.
        /// </summary>
        /// <returns></returns>
        public string GetBasicFeeGroup(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFeeScheduleLookup> objBasicFeeGroup = new BLLFeeSchedule().LookupBasicFeeGroup();
            DSFeeScheduleLookup ds = objBasicFeeGroup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BasicFeeGroup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.BasicFeeGroup.TableName].Select("1=1", ds.BasicFeeGroup.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BasicFeeGroup.EntityNameColumn.ColumnName].ToString() + " - " + dr[ds.BasicFeeGroup.ShortNameColumn.ColumnName].ToString(), dr[ds.BasicFeeGroup.BasicFeeGroupIdColumn.ColumnName].ToString(), dr[ds.BasicFeeGroup.EntityIdColumn].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the CPT code.
        /// </summary>
        /// <returns></returns>
        public string GetCPTCode(string IsActive, string EntityID, string CPTCode)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> objCode = new BLLAdminCodes().LookupCPTCode(EntityID, CPTCode);
            DSCodeLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.CPTCode.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.CPTCode.TableName].Select("1=1", ds.CPTCode.CPTCodeColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.CPTCode.CPTCodeColumn.ColumnName].ToString() + " - " + dr[ds.CPTCode.DescriptionColumn.ColumnName].ToString(), dr[ds.CPTCode.CPTCodeIdColumn.ColumnName].ToString(), dr[ds.CPTCode.CPTCodeColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);

        }

        /// <summary>
        /// Gets the ICD code.
        /// </summary>
        /// <returns></returns>
        public string GetICDCode(string IsActive, string EntityID, string ICDCode)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> objCode = new BLLAdminCodes().LookupICDCode(EntityID, ICDCode);
            DSCodeLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ICDCode.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ICDCode.TableName].Select("1=1", ds.ICDCode.ICD9Column.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ICDCode.ICD9Column.ColumnName].ToString() + " - " + dr[ds.ICDCode.DescriptionColumn.ColumnName].ToString(), dr[ds.ICDCode.ICDIdColumn.ColumnName].ToString(), dr[ds.ICDCode.ICD9Column.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);

        }

        /// <summary>
        /// Gets the submitter setup.
        /// </summary>
        /// <returns></returns>
        public string GetSubmitterSetup(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSEDILookup> objEDI = new BLLAdminEDI().LookupSubmitterSetup();
            DSEDILookup ds = objEDI.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SubmitterSetup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SubmitterSetup.TableName].Select("1=1", ds.SubmitterSetup.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SubmitterSetup.ShortNameColumn.ColumnName].ToString(), dr[ds.SubmitterSetup.SubmitterSetupIdColumn.ColumnName].ToString(), dr[ds.SubmitterSetup.EntityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the edi receiver setup.
        /// </summary>
        /// <returns></returns>
        public string GetEDIReceiverSetup(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSEDILookup> objEDI = new BLLAdminEDI().LookupEDIReceiverSetup();
            DSEDILookup ds = objEDI.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.EDIReceiverSetup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.EDIReceiverSetup.TableName].Select("1=1", ds.EDIReceiverSetup.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.EDIReceiverSetup.ShortNameColumn.ColumnName].ToString(), dr[ds.EDIReceiverSetup.EDIReceiverSetupIdColumn.ColumnName].ToString(), dr[ds.EDIReceiverSetup.EnitityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the loop2310 b.
        /// </summary>
        /// <returns></returns>
        public string GetLoop2310B(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProfileLookup> objProfile = new BLLAdminProfile().LookupLoop2310B();
            DSProfileLookup ds = objProfile.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Loop2310B.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Loop2310B.TableName].Select("1=1", ds.Loop2310B.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Loop2310B.DescriptionColumn.ColumnName].ToString(), dr[ds.Loop2310B.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the modifier.
        /// </summary>
        /// <returns></returns>
        public string GetModifier(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> obj = new BLLAdminCodes().LookupModifier("", null);
            DSCodeLookup ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (obj.Data != null)
            {
                if (ds.Tables[ds.Modifier.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Modifier.TableName].Select("1=1", ds.Modifier.ModifierCodeColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Modifier.ModifierCodeColumn.ColumnName].ToString(), dr[ds.Modifier.ModifierIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the modifier.
        /// </summary>
        /// <returns></returns>
        public string GetTestType(string IsActive)
        {
            //ahmar todo
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            List<TestTypeModel> testTypeList = new BLLAdminCodes().LookupTestType(IsActive);

            list.Add(new NameValuePair("- Select -", ""));
            if (testTypeList != null && testTypeList.Count > 0)
            {
                foreach (TestTypeModel obj in testTypeList)
                {
                    list.Add(new NameValuePair(obj.Name, obj.TestTypeId, obj.Code, obj.CodeSystem));
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the revenue code.
        /// </summary>
        /// <returns></returns>
        public string GetRevenueCode(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSCodeLookup> obj = new BLLAdminCodes().LookupRevenueCode();
            DSCodeLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.RevenueCode.TableName] != null)
                {


                    DataRow[] dRows = ds.Tables[ds.RevenueCode.TableName].Select("1=1", ds.RevenueCode.RevenueCodeColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.RevenueCode.RevenueCodeColumn.ColumnName].ToString() + " - " + dr[ds.RevenueCode.DescriptionColumn.ColumnName].ToString(), dr[ds.RevenueCode.RevenueCodeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetBlockReasons(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSScheduleLookups> obj = new BLLSchedule().LookupReasons(IsActive);
            DSScheduleLookups ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ScheduleReasons.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ScheduleReasons.TableName].Select("1=1", ds.ScheduleReasons.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair(dr[ds.ScheduleReasons.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.ScheduleReasons.EntityShortNameColumn.ColumnName].ToString(), dr[ds.ScheduleReasons.ScheduleReasonIdColumn.ColumnName].ToString(), dr[ds.ScheduleReasons.EntityIdColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair(dr[ds.ScheduleReasons.ShortNameColumn.ColumnName].ToString(), dr[ds.ScheduleReasons.ScheduleReasonIdColumn.ColumnName].ToString(), dr[ds.ScheduleReasons.EntityIdColumn.ColumnName].ToString()));

                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetBlockReasons(string IsActive, string EntityId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSScheduleLookups> obj = new BLLSchedule().LookupReasons(IsActive, EntityId);
            DSScheduleLookups ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ScheduleReasons.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ScheduleReasons.TableName].Select("1=1", ds.ScheduleReasons.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair(dr[ds.ScheduleReasons.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.ScheduleReasons.EntityShortNameColumn.ColumnName].ToString(), dr[ds.ScheduleReasons.ScheduleReasonIdColumn.ColumnName].ToString(), dr[ds.ScheduleReasons.EntityIdColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair(dr[ds.ScheduleReasons.ShortNameColumn.ColumnName].ToString(), dr[ds.ScheduleReasons.ScheduleReasonIdColumn.ColumnName].ToString(), dr[ds.ScheduleReasons.EntityIdColumn.ColumnName].ToString()));

                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetResources(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupResources(IsActive);
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Resources.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Resources.TableName].Select("1=1", ds.Resources.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair(dr[ds.Resources.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Resources.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Resources.ResourceIdColumn.ColumnName].ToString(), dr[ds.Resources.EntityIdColumn.ColumnName].ToString(), dr[ds.Resources.ResourceProviderIdColumn.ColumnName].ToString() + "-" + dr[ds.Resources.ResourceProviderNameColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair(dr[ds.Resources.ShortNameColumn.ColumnName].ToString(), dr[ds.Resources.ResourceIdColumn.ColumnName].ToString(), dr[ds.Resources.EntityIdColumn.ColumnName].ToString(), dr[ds.Resources.ResourceProviderIdColumn.ColumnName].ToString() + "-" + dr[ds.Resources.ResourceProviderNameColumn.ColumnName].ToString()));

                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetResourcesWithDescription(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupResources(IsActive);
            DSProfileLookup ds = obj.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Resources.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Resources.TableName].Select("1=1", ds.Resources.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair((dr[ds.Resources.DescriptionColumn.ColumnName].ToString() == "" ? dr[ds.Resources.ShortNameColumn.ColumnName].ToString() : dr[ds.Resources.DescriptionColumn.ColumnName].ToString()) + " - " + dr[ds.Resources.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Resources.ResourceIdColumn.ColumnName].ToString(), dr[ds.Resources.EntityIdColumn.ColumnName].ToString(), dr[ds.Resources.ResourceProviderIdColumn.ColumnName].ToString() + "-" + dr[ds.Resources.ResourceProviderNameColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair((dr[ds.Resources.DescriptionColumn.ColumnName].ToString() == "" ? dr[ds.Resources.ShortNameColumn.ColumnName].ToString() : dr[ds.Resources.DescriptionColumn.ColumnName].ToString()), dr[ds.Resources.ResourceIdColumn.ColumnName].ToString(), dr[ds.Resources.EntityIdColumn.ColumnName].ToString(), dr[ds.Resources.ResourceProviderIdColumn.ColumnName].ToString() + "-" + dr[ds.Resources.ResourceProviderNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Gets the claim flag.
        /// </summary>
        /// <returns></returns>
        public string GetClaimFlag(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> objCode = new BLLAdminInsurance().LookupClaimFlag();
            DSCodeLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ClaimFlag.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ClaimFlag.TableName].Select("1=1", ds.ClaimFlag.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ClaimFlag.ClaimFlagIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ClaimFlag.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.ClaimFlag.DescriptionColumn.ColumnName].ToString(), dr[ds.ClaimFlag.ClaimFlagIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the type of the claim.
        /// </summary>
        /// <returns></returns>
        public string GetClaimType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> objCode = new BLLAdminInsurance().LookupClaimType(IsActive);
            DSCodeLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ClaimType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ClaimType.TableName].Select("1=1", ds.ClaimType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ClaimType.ClaimTypeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ClaimType.ShortNameColumn.ColumnName].ToString(), dr[ds.ClaimType.ClaimTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the type of the plan.
        /// </summary>
        /// <returns></returns>
        public string GetPlanType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> objCode = new BLLAdminInsurance().LookupPlanType(IsActive);
            DSCodeLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PlanType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PlanType.TableName].Select("1=1", ds.PlanType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.PlanType.PlanTypeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.PlanType.ShortNameColumn.ColumnName].ToString(), dr[ds.PlanType.PlanTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the plan fee link.
        /// </summary>
        /// <returns></returns>
        public string GetPlanFeeLink(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> objCode = new BLLAdminInsurance().LookupPlanFeeLink();
            DSCodeLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PlanFeeLink.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PlanFeeLink.TableName].Select("1=1", ds.PlanFeeLink.NameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PlanFeeLink.EntityNameColumn.ColumnName].ToString() + " - " + dr[ds.PlanFeeLink.NameColumn.ColumnName].ToString(), dr[ds.PlanFeeLink.PlanFeeLinkIdColumn.ColumnName].ToString(), dr[ds.PlanFeeLink.EntityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetPlanFeeLink(string IsActive, string EntityId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> objCode = new BLLAdminInsurance().LookupPlanFeeLink();
            DSCodeLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PlanFeeLink.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PlanFeeLink.TableName].Select("1=1", ds.PlanFeeLink.NameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PlanFeeLink.EntityNameColumn.ColumnName].ToString() + " - " + dr[ds.PlanFeeLink.NameColumn.ColumnName].ToString(), dr[ds.PlanFeeLink.PlanFeeLinkIdColumn.ColumnName].ToString(), dr[ds.PlanFeeLink.EntityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Lookups the claim scrubbing profile.
        /// </summary>
        /// <returns></returns>
        public string GetClaimScrubbingProfile(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> objCode = new BLLAdminInsurance().LookupClaimScrubbingProfile();
            DSCodeLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ClaimScrubbingProfile.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ClaimScrubbingProfile.TableName].Select("1=1", ds.ClaimScrubbingProfile.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ClaimScrubbingProfile.ClaimScrubbingProfileIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ClaimScrubbingProfile.ShortNameColumn.ColumnName].ToString(), dr[ds.ClaimScrubbingProfile.ClaimScrubbingProfileIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the plan category.
        /// </summary>
        /// <returns></returns>
        public string GetPlanCategory(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSInsuranceLookup> objInsurance = new BLLAdminInsurance().LookupPlanCategory(IsActive);
            DSInsuranceLookup ds = objInsurance.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PlanCategory.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PlanCategory.TableName].Select("1=1", ds.PlanCategory.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PlanCategory.ShortNameColumn.ColumnName].ToString(), dr[ds.PlanCategory.PlanIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the edi claim status insurance.
        /// </summary>
        /// <returns></returns>
        public string GetEDIClaimStatusInsurance(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSEDILookup> objEDI = new BLLAdminEDI().LookupEDIClaimStatusInsurance();
            DSEDILookup ds = objEDI.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.EDIClaimStatusInsurance.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.EDIClaimStatusInsurance.TableName].Select("1=1", ds.EDIClaimStatusInsurance.EDIClaimStatusIDColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.EDIClaimStatusInsurance.EDIStatusInsuranceColumn.ColumnName].ToString(), dr[ds.EDIClaimStatusInsurance.EDIClaimStatusIDColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the edi eligibility insurance.
        /// </summary>
        /// <returns></returns>
        public string GetEDIEligibilityInsurance(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSEDILookup> objEDI = new BLLAdminEDI().LookupEDIEligibilityInsurance();
            DSEDILookup ds = objEDI.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.EDIEligibilityInsurance.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.EDIEligibilityInsurance.TableName].Select("1=1", ds.EDIEligibilityInsurance.EDIEligibilityIDColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.EDIEligibilityInsurance.EligibilityInsuranceNameColumn.ColumnName].ToString(), dr[ds.EDIEligibilityInsurance.EDIEligibilityIDColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the edi submit insurance.
        /// </summary>
        /// <returns></returns>
        public string GetEDISubmitInsurance(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSEDILookup> objEDI = new BLLAdminEDI().LookupEDISubmitInsurance();
            DSEDILookup ds = objEDI.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.EDISubmitInsurance.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.EDISubmitInsurance.TableName].Select("1=1", ds.EDISubmitInsurance.EDISubmitIDColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.EDISubmitInsurance.SubmitInsuranceNameColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.EDISubmitInsurance.SubmitInsuranceNameColumn.ColumnName].ToString(), dr[ds.EDISubmitInsurance.EDISubmitIDColumn.ColumnName].ToString(), dr[ds.EDISubmitInsurance.PayorIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the insurance plan.
        /// </summary>
        /// <returns></returns>
        public string GetInsurancePlan(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSInsuranceLookup> objInsLookup = new BLLAdminInsurance().LookupInsurancePlan(IsActive);
            DSInsuranceLookup ds = objInsLookup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.InsurancePlan.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.InsurancePlan.TableName].Select("1=1", ds.InsurancePlan.InsurancePlanIdColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.InsurancePlan.ShortNameColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.InsurancePlan.IPDescriptionColumn.ColumnName].ToString(), dr[ds.InsurancePlan.InsurancePlanIdColumn.ColumnName].ToString(), dr[ds.InsurancePlan.SearchPatternColumn.ColumnName].ToString(), dr[ds.InsurancePlan.ShortNameColumn.ColumnName].ToString(),"","","","", dr[ds.InsurancePlan.IsreferralColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetInsurancePlan(string IsActive,string ShortName)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSInsuranceLookup> objInsLookup = new BLLAdminInsurance().LookupInsurancePlan(IsActive,null,ShortName);
            DSInsuranceLookup ds = objInsLookup.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.InsurancePlan.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.InsurancePlan.TableName].Select("1=1", ds.InsurancePlan.InsurancePlanIdColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.InsurancePlan.ShortNameColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.InsurancePlan.IPDescriptionColumn.ColumnName].ToString(), dr[ds.InsurancePlan.InsurancePlanIdColumn.ColumnName].ToString(), dr[ds.InsurancePlan.SearchPatternColumn.ColumnName].ToString(), dr[ds.InsurancePlan.ShortNameColumn.ColumnName].ToString(), "", "", "", "", dr[ds.InsurancePlan.IsreferralColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the insurance plan.
        /// </summary>
        /// <returns></returns>
        public string GetInsurancePlanAddress(string IsActive, Int64 InsurancePlanId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            Int64 IPId = 0;

            //Int64 GuarantorID = MDVUtility.ToInt64(context.Request["ID"]);
            BLObject<DSInsuranceLookup> objInsLookup = new BLLAdminInsurance().LookupInsurancePlanAddress(InsurancePlanId);
            DSInsuranceLookup ds = objInsLookup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.InsurancePlanAddress.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.InsurancePlanAddress.TableName].Select("1=1", ds.InsurancePlanAddress.InsurancePlanAddressIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.InsurancePlanAddress.AddressColumn.ColumnName].ToString(), dr[ds.InsurancePlanAddress.InsurancePlanAddressIdColumn.ColumnName].ToString(), dr[ds.InsurancePlanAddress.phoneNoColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the clearing house.
        /// </summary>
        /// <returns></returns>
        public string GetClearingHouse(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSEDILookup> objEdiLookup = new BLLAdminEDI().LookupClearingHouse();
            DSEDILookup ds = objEdiLookup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ClearingHouse.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ClearingHouse.TableName].Select("1=1", ds.ClearingHouse.ClearingHouseIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ClearingHouse.ShortNameColumn.ColumnName].ToString(), dr[ds.ClearingHouse.ClearingHouseIdColumn.ColumnName].ToString(), dr[ds.ClearingHouse.EntityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the type of the clearing house.
        /// </summary>
        /// <returns></returns>
        public string GetClearingHouseType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSEDILookup> objEdiLookup = new BLLAdminEDI().LookupClearingHouseType();
            DSEDILookup ds = objEdiLookup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ClearingHouse.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ClearingHouseType.TableName].Select("1=1", ds.ClearingHouseType.ClearingHouseTypeIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ClearingHouseType.ShortNameColumn.ColumnName].ToString(), dr[ds.ClearingHouseType.ClearingHouseTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetARType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFollowUp> objARType = new BLLAdminFollowUp().LookupActionReasonType();
            DSFollowUp ds = objARType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FollowupActionReasonTypeLookUp.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.FollowupActionReasonTypeLookUp.TableName].Select("1=1", ds.FollowupActionReasonTypeLookUp.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.FollowupActionReasonTypeLookUp.ShortNameColumn.ColumnName].ToString(), dr[ds.FollowupActionReasonTypeLookUp.ARTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetFollowUpReasons(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFollowUp> objARType = new BLLAdminFollowUp().LookupFollowUpReasons();
            DSFollowUp ds = objARType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FollowupReasonsLookUp.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.FollowupReasonsLookUp.TableName].Select("1=1", ds.FollowupReasonsLookUp.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.FollowupReasonsLookUp.ShortNameColumn.ColumnName].ToString(), dr[ds.FollowupReasonsLookUp.ReasonIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetFollowUpCallStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFollowUp> objCallStatus = new BLLBilling().LookupCallStatus();
            DSFollowUp ds = objCallStatus.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.CallStatusLookUp.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.CallStatusLookUp.TableName].Select("1=1", ds.CallStatusLookUp.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.CallStatusLookUp.ShortNameColumn.ColumnName].ToString(), dr[ds.CallStatusLookUp.CallStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetAutoAction(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFollowUp> objAutoAction = new BLLAdminFollowUp().LookupAutoAction();
            DSFollowUp ds = objAutoAction.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.AutoActionLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.AutoActionLookup.TableName].Select("1=1", ds.AutoActionLookup.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.AutoActionLookup.ShortNameColumn.ColumnName].ToString(), dr[ds.AutoActionLookup.AutoActionIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetFollowUpAction(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFollowUp> objFollowAction = new BLLAdminFollowUp().LookupFollowUpAction();
            DSFollowUp ds = objFollowAction.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FollowUpActionLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.FollowUpActionLookup.TableName].Select("1=1", ds.FollowUpActionLookup.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.FollowUpActionLookup.ShortNameColumn.ColumnName].ToString(), dr[ds.FollowUpActionLookup.FollowUpActionIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetFollowUpARGroup(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFollowUp> objFollowArGroup = new BLLAdminFollowUp().LookupARGroup();
            DSFollowUp ds = objFollowArGroup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FollowupARGroupLookUp.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.FollowupARGroupLookUp.TableName].Select("1=1", ds.FollowupARGroupLookUp.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.FollowupARGroupLookUp.ShortNameColumn.ColumnName].ToString(), dr[ds.FollowupARGroupLookUp.ARGroupIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetClaimStatusCode(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFollowUp> objAutoAction = new BLLAdminFollowUp().LookupClaimStatusCode();
            DSFollowUp ds = objAutoAction.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ClaimStatusCodeLookUp.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ClaimStatusCodeLookUp.TableName].Select("1=1", ds.ClaimStatusCodeLookUp.CodeColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ClaimStatusCodeLookUp.CodeColumn.ColumnName].ToString(), dr[ds.ClaimStatusCodeLookUp.CSCodeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetClaimStatusCategoryCode(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFollowUp> objAutoAction = new BLLAdminFollowUp().LookupClaimStatusCategoryCode();
            DSFollowUp ds = objAutoAction.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ClaimStatusCategoryCodeLookUp.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ClaimStatusCategoryCodeLookUp.TableName].Select("1=1", ds.ClaimStatusCategoryCodeLookUp.CodeColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ClaimStatusCategoryCodeLookUp.CodeColumn.ColumnName].ToString(), dr[ds.ClaimStatusCategoryCodeLookUp.CSCatCodeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetThemes(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSUserLookup> objThemes = new BLLAdminSecurity().LookupThemes(IsActive);
            DSUserLookup ds = objThemes.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Themes.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Themes.TableName].Select("1=1", ds.Themes.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Themes.ShortNameColumn.ColumnName].ToString(), dr[ds.Themes.ThemeIdColumn.ColumnName].ToString(), dr[ds.Themes.ShortNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetRemindersType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRemindersLookup> objThemes = new BLLAdmin().GetRemindersType();
            DSRemindersLookup ds = objThemes.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.TemplateType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.TemplateType.TableName].Select("1=1", ds.TemplateType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.TemplateType.ShortNameColumn.ColumnName].ToString(), dr[ds.TemplateType.RemindersTemplateTypeIdColumn.ColumnName].ToString(), dr[ds.TemplateType.ShortNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetDrugCodeCost(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodes> objThemes = new BLLAdminCodes().GeDrugCodeCost();
            DSCodes ds = objThemes.Data;
            //list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.DrugCodesLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.DrugCodesLookup.TableName].Select("1=1", ds.DrugCodesLookup.CPTCodeColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.DrugCodesLookup.CPTCodeColumn.ColumnName].ToString(), dr[ds.DrugCodesLookup.CostColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetReminderDeliveryDateTime(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRemindersLookup> objThemes = new BLLAdmin().GetReminderDeliveryDateTime();
            DSRemindersLookup ds = objThemes.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.ReminderDeliveryDateTime.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ReminderDeliveryDateTime.TableName].Select("1=1", ds.ReminderDeliveryDateTime.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ReminderDeliveryDateTime.ReminderDeliveryDateTimeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ReminderDeliveryDateTime.DescriptionColumn.ColumnName].ToString(), dr[ds.ReminderDeliveryDateTime.DescriptionColumn.ColumnName].ToString(), dr[ds.ReminderDeliveryDateTime.DescriptionColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the User Type.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetUserType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            var model = new BLLAdminSecurity().LookupUserType();

            var ds = model.Data;
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Name, item.Id));
                    }
                }

            }
            return getJSONofList(list);
        }

        #endregion

        #region Patient Lookups

        /// <summary>
        /// Gets the modifier.
        /// </summary>
        /// <returns></returns>
        public string GetPatientInsurance(string IsActive, long PatientId)
        {
            if (PatientId == -1)
            {
                PatientId = 0;
            }
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupPatientInsurance(PatientId, IsActive);
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PreFix.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PatientInsurance.TableName].Select("1=1", ds.PatientInsurance.PlanPriorityColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PatientInsurance.InsurancePlanNameColumn.ColumnName].ToString(), dr[ds.PatientInsurance.InsuranceIdColumn.ColumnName].ToString(), dr[ds.PatientInsurance.PlanPriorityColumn.ColumnName].ToString(), dr[ds.PatientInsurance.AmtCopayColumn.ColumnName].ToString(), "", dr[ds.PatientInsurance.InsurancePlanIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetPatientVisitInsurance(string IsActive, long visitId)
        {
            if (visitId == -1)
            {
                visitId = 0;
            }

            var list = new BLLPatient().LookupPatientVisitInsurance(visitId, IsActive);

            return JsonConvert.SerializeObject(list.Data);
        }

        /// <summary>
        /// Gets the modifier.
        /// </summary>
        /// <returns></returns>
        public string GetPatientCase(string IsActive, long PatientId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCaseLookup> objPatient = new BLLPatient().LookupCaseManagement(PatientId);
            DSCaseLookup ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.CaseManagement.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.CaseManagement.TableName].Select("1=1", ds.CaseManagement.CaseNumberColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.CaseManagement.CaseMgmtIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.CaseManagement.CaseNumberColumn.ColumnName].ToString(), dr[ds.CaseManagement.CaseMgmtIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetPrefix(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupPrefix();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PreFix.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PreFix.TableName].Select("1=1", ds.PreFix.TitleColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.PreFix.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.PreFix.TitleColumn.ColumnName].ToString(), dr[ds.PreFix.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetPrefixDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLPatient().LookupPrefixDemographic();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Title, item.Id));
                    }
                }

            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the suffix.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetSuffix(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupSuffix();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Suffix.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Suffix.TableName].Select("1=1", ds.Suffix.TitleColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Suffix.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Suffix.TitleColumn.ColumnName].ToString(), dr[ds.Suffix.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetSuffixDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLPatient().LookupSuffixDemographic();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Title, item.Id));
                    }
                }

            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the marital status.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetMaritalStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupMaritalStatus();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MaritialStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.MaritialStatus.TableName].Select("1=1", ds.MaritialStatus.StatusColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.MaritialStatus.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.MaritialStatus.StatusColumn.ColumnName].ToString(), dr[ds.MaritialStatus.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetMaritalStatusDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLPatient().LookupMaritalStatusDemographic();
            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                foreach (var item in ds)
                    list.Add(new NameValuePair(item.Status, item.Id));
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the guarantor.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetGuarantor(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupGuarantor();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Guarantor.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Guarantor.TableName].Select("1=1", ds.Guarantor.FullNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Guarantor.GuarantorIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Guarantor.FullNameColumn.ColumnName].ToString(), dr[ds.Guarantor.GuarantorIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the languages.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetLanguages(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupLanguages();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Languages.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Languages.TableName].Select("1=1", ds.Languages.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Languages.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Languages.DescriptionColumn.ColumnName].ToString(), dr[ds.Languages.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetCountries(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objCountries = new BLLPatient().LookupCountries();
            DSPatientLookups ds = objCountries.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Countries.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Countries.TableName].Select("1=1", ds.Countries.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Countries.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Countries.DescriptionColumn.ColumnName].ToString(), dr[ds.Countries.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetPreferredAddress(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPrefAddress = new BLLPatient().LookupPreferredAddress();
            DSPatientLookups ds = objPrefAddress.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PreferredAddress.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PreferredAddress.TableName].Select("1=1", ds.PreferredAddress.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.PreferredAddress.DescriptionColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.PreferredAddress.DescriptionColumn.ColumnName].ToString(), dr[ds.PreferredAddress.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetPreferredPhone(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPrefPhone = new BLLPatient().LookupPreferredPhone();
            DSPatientLookups ds = objPrefPhone.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PreferredPhone.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PreferredPhone.TableName].Select("1=1", ds.PreferredPhone.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.PreferredPhone.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.PreferredPhone.DescriptionColumn.ColumnName].ToString(), dr[ds.PreferredPhone.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the smokers status.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetSmokersStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupSmokersStatus();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SmokersStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SmokersStatus.TableName].Select("1=1", ds.SmokersStatus.CodeColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SmokersStatus.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SmokersStatus.CodeColumn.ColumnName].ToString(), dr[ds.SmokersStatus.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the school status.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetSchoolStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupSchoolStatus();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SchoolStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SchoolStatus.TableName].Select("1=1", ds.SchoolStatus.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SchoolStatus.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SchoolStatus.DescriptionColumn.ColumnName].ToString(), dr[ds.SchoolStatus.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the school.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetSchool(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupSchool();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.School.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.School.TableName].Select("1=1", ds.School.SchoolNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.School.SchoolIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.School.SchoolNameColumn.ColumnName].ToString(), dr[ds.School.SchoolIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the communication.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetCommunication(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupCommunication();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Communication.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Communication.TableName].Select("1=1", ds.Communication.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Communication.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Communication.DescriptionColumn.ColumnName].ToString(), dr[ds.Communication.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }



        public string GetRaceByDescription(string IsActive)
        {
            return GetRaceByDescription(null,false);
        }


        /// <summary>
        /// Gets the race.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetRace(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupRace();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Race.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Race.TableName].Select("1=1", ds.Race.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Race.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Race.DescriptionColumn.ColumnName].ToString(), dr[ds.Race.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetRaceByDescription(string Description, bool IsSelectRequired = true)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupRaceByDescription(Description);
            DSPatientLookups ds = objPatient.Data;

            if (IsSelectRequired)
                list.Add(new NameValuePair("- Select -", ""));

            if (ds != null)
            {
                if (ds.Tables[ds.Race.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Race.TableName].Select("1=1", ds.Race.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Race.DescriptionColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Race.DescriptionColumn.ColumnName].ToString(), dr[ds.Race.IdColumn.ColumnName].ToString(),dr[ds.Race.RaceParentIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetGenderIdentity(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            List<LookUpModel> GenderIdentityList = null;
            BLObject<List<LookUpModel>> objGenderIdentity = new BLLPatient().LoadGenderIdentityLookUp();
            list.Add(new NameValuePair("- Select -", ""));
            GenderIdentityList = objGenderIdentity.Data;
            if (objGenderIdentity.Data != null && GenderIdentityList != null)
            {
                GenderIdentityList = GenderIdentityList.OrderBy(x => x.Name).ToList();
                foreach (LookUpModel item in GenderIdentityList)
                    list.Add(new NameValuePair(item.Name, item.Id));
            }
            return getJSONofList(list);
        }
        public string GetSexualOrientation(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            List<LookUpModel> SexualOrientationList = null;
            BLObject<List<LookUpModel>> objSexualOrientation = new BLLPatient().LoadSexualOrientationLookUp();
            list.Add(new NameValuePair("- Select -", ""));
            SexualOrientationList = objSexualOrientation.Data;
            if (objSexualOrientation.Data != null && SexualOrientationList != null)
            {
                SexualOrientationList = SexualOrientationList.OrderBy(x => x.Name).ToList();
                foreach (LookUpModel item in SexualOrientationList)
                    list.Add(new NameValuePair(item.Name, item.Id));
            }
            return getJSONofList(list);
        }
        //public string GetRaceForDemographics(string IsActive)
        //{
        //    List<HierarchicalLookUpModel> listTobeReturned = new List<HierarchicalLookUpModel>();
        //    List<HierarchicalLookUpModel> PatientRaceLookUpModelList = null;
        //    BLObject<List<HierarchicalLookUpModel>> obRace = new BLLPatient().LookupPatientRaceDemographic();
        //    PatientRaceLookUpModelList = obRace.Data;
        //    if (PatientRaceLookUpModelList != null)
        //    {
        //        listTobeReturned = PatientRaceLookUpModelList.Where(c => c.ParentId == 0).Select(c => new HierarchicalLookUpModel() { Id = c.Id, Name = c.Name, ParentId = c.ParentId, Children = GetChildren(PatientRaceLookUpModelList, c.Id) }).ToList();
        //    }
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(listTobeReturned);
        //}
        //public string GetEthnicityForDemographics(string IsActive)
        //{
        //    List<HierarchicalLookUpModel> listTobeReturned = new List<HierarchicalLookUpModel>();
        //    List<HierarchicalLookUpModel> PatientEthnicityLookUpModelList = null;
        //    BLObject<List<HierarchicalLookUpModel>> obRace = new BLLPatient().LookupPatientEthnicityDemographic();
        //    PatientEthnicityLookUpModelList = obRace.Data;
        //    if (PatientEthnicityLookUpModelList != null)
        //    {
        //        listTobeReturned = PatientEthnicityLookUpModelList.Where(c => c.ParentId == 0).Select(c => new HierarchicalLookUpModel() { Id = c.Id, Name = c.Name, ParentId = c.ParentId, Children = GetChildren(PatientEthnicityLookUpModelList, c.Id) }).ToList();
        //    }
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(listTobeReturned);
        //}
        public string GetEthnicityForDemographics(string IsActive)
        {
            List<CustomModel> listTobeReturned = new List<CustomModel>();
            List<CustomModel> PatientEthnicityLookUpModelList = null;
            BLObject<List<CustomModel>> obRace = new BLLPatient().LookupPatientEthnicityDemographics();
            PatientEthnicityLookUpModelList = obRace.Data;
            if (PatientEthnicityLookUpModelList != null)
                PatientEthnicityLookUpModelList = PatientEthnicityLookUpModelList.OrderBy(x => x.Name).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(PatientEthnicityLookUpModelList);
        }
        public string GetRaceForDemographics(string IsActive)
        {
            List<CustomModel> listTobeReturned = new List<CustomModel>();
            List<CustomModel> PatientRaceLookUpModelList = null;
            BLObject<List<CustomModel>> obRace = new BLLPatient().LookupPatientRaceDemographics();
            PatientRaceLookUpModelList = obRace.Data;
            return Newtonsoft.Json.JsonConvert.SerializeObject(PatientRaceLookUpModelList);
        }
        public string GetPatientCareGiver(string IsActive, long PatientId)
        {           
            List<CustomModel> CareGiverLookUpList = null;
            BLObject<List<CustomModel>> objCareGiver = new BLLPatient().LookupPatientCareGiver(PatientId);
            CareGiverLookUpList = objCareGiver.Data;
            if (CareGiverLookUpList != null)
                CareGiverLookUpList = CareGiverLookUpList.OrderBy(x => x.Name).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(CareGiverLookUpList);
        }
        public static List<HierarchicalLookUpModel> GetChildren(List<HierarchicalLookUpModel> PatientRaceLookUpModelList, long parentId)
        {
            return PatientRaceLookUpModelList.Where(c => c.ParentId == parentId).Select(c => new HierarchicalLookUpModel { Id = c.Id, Name = c.Name, ParentId = c.ParentId, Children = GetChildren(PatientRaceLookUpModelList, c.Id) }).ToList();
        }
        public string GetRaceDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            var model = new BLLPatient().LookupRaceDemographic();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Description, item.Id));
                    }
                }

            }
            return getJSONofList(list);
        }


        /// <summary>
        /// Gets the ethnicity.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetEthnicity(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupEthnicity();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Ethnicity.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Ethnicity.TableName].Select("1=1", ds.Ethnicity.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Ethnicity.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Ethnicity.DescriptionColumn.ColumnName].ToString(), dr[ds.Ethnicity.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetEthnicityDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLPatient().LookupEthnicityDemographic();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Description, item.Id));
                    }
                }

            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the relationship.
        /// </summary>
        /// <returns></returns>
        public string GetRelationship(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupRelation();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.RelationShip.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.RelationShip.TableName].Select("1=1", ds.RelationShip.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.RelationShip.DescriptionColumn.ColumnName].ToString(), dr[ds.RelationShip.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetRelationshipDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLPatient().LookupRelationDemographic();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Description, item.Id));
                    }
                }

            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the type of the MSP.
        /// </summary>
        /// <returns></returns>
        public string GetMSPType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupMSPType();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MSPType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.MSPType.TableName].Select("1=1", ds.MSPType.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.MSPType.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.MSPType.DescriptionColumn.ColumnName].ToString(), dr[ds.MSPType.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetMSPTypeDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLPatient().LookupMSPTypeDemographic();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Description, item.Id));
                    }
                }

            }
            return getJSONofList(list);
        }

        public string GetIsuranceTypeDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLPatient().LookupInsurancePlanTypeDemographic();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Description, item.Id));
                    }
                }

            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the type of the isurance.
        /// </summary>
        /// <returns></returns>
        public string GetIsuranceType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupInsurancePlanType();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.InsurancePlanType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.InsurancePlanType.TableName].Select("1=1", ds.InsurancePlanType.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.InsurancePlanType.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.InsurancePlanType.DescriptionColumn.ColumnName].ToString(), dr[ds.InsurancePlanType.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the employer.
        /// </summary>
        /// <returns></returns>
        public string GetEmployer(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupEmployer();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Employer.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Employer.TableName].Select("1=1", ds.Employer.EmployerNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Employer.EmployerIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Employer.EmployerNameColumn.ColumnName].ToString(), dr[ds.Employer.EmployerIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetEmployerByName(string Name)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupEmployer(Name);
            DSPatientLookups ds = objPatient.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.Employer.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Employer.TableName].Select("1=1", ds.Employer.EmployerNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Employer.EmployerIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Employer.EmployerNameColumn.ColumnName].ToString(), dr[ds.Employer.EmployerIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the lawyer.
        /// </summary>
        /// <returns></returns>
        public string GetLawyer(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupLawyer();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Lawyer.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Lawyer.TableName].Select("1=1", ds.Lawyer.LawyerNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Lawyer.LawyerIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Lawyer.LawyerNameColumn.ColumnName].ToString(), dr[ds.Lawyer.LawyerIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetLawyerByName(string LawyerName)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupLawyer(LawyerName);
            DSPatientLookups ds = objPatient.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.Lawyer.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Lawyer.TableName].Select("1=1", ds.Lawyer.LawyerNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Lawyer.LawyerIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Lawyer.LawyerNameColumn.ColumnName].ToString(), dr[ds.Lawyer.LawyerIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        public string GetPatientReferral(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupPatientReferral(IsActive);
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PatientReferral.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PatientReferral.TableName].Select("1=1", ds.PatientReferral.IdColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.PatientReferral.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.PatientReferral.IdColumn.ColumnName].ToString(), dr[ds.PatientReferral.RefferalAuthNoColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        public string GetPatientPAN(string IsActive, long PatientID)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatient> obj = new BLLPatient().LoadPatientAuthorization(PatientID, 0, 0, null, null, null);
            DSPatient ds = obj.Data;

            if (ds != null)
            {
                if (ds.Tables[ds.Authorizations.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Authorizations.TableName].Select("1=1", ds.Authorizations.AuthorizeIdColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Authorizations.AuthorizeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Authorizations.PANColumn.ColumnName].ToString(), dr[ds.Authorizations.AuthorizeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetSecurityQuestions(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientPortal> objPatient = new BLLPatient().LookupSecurityQuestions();
            DSPatientPortal ds = objPatient.Data;
            list.Add(new NameValuePair("Select Control", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SecurityQuestionLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SecurityQuestionLookup.TableName].Select("1=1", ds.SecurityQuestionLookup.SecurityQuestionIdColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SecurityQuestionLookup.SecurityQuestionIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SecurityQuestionLookup.QuestionNameColumn.ColumnName].ToString(), dr[ds.SecurityQuestionLookup.SecurityQuestionIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetSecurityEntityGroup(string IsActive)
        {


            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSUsers> objUser = new BLLAdminSecurity().LookupSecurityEntityGroup(IsActive);
            DSUsers ds = objUser.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.SecurityGroup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SecurityGroup.TableName].Select("1=1", ds.SecurityGroup.SecGroupIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SecurityGroup.ShortNameColumn.ColumnName].ToString(), dr[ds.SecurityGroup.SecGroupIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the Document Source.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetDocumentSource(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            var model = new BLLPatient().LookupDocumentSource();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Name, item.Id));
                    }
                }

            }
            return getJSONofList(list);
        }
        public string GetStatusReasons(string StatusId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objListLookup = new BLLPatient().GetStatusReasons(MDVUtility.ToInt64(StatusId));
            DSPatientLookups ds = objListLookup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ReferralStatusReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ReferralStatusReason.TableName].Select("1=1", ds.ReferralStatusReason.IdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ReferralStatusReason.DescriptionColumn.ColumnName].ToString(), dr[ds.ReferralStatusReason.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region Message Lookups

        /// <summary>
        /// Gets the MessageType.
        /// </summary>
        /// <returns></returns>
        public string GetMessageType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMessageLookup> objMessage = new BLLMessage().LookupMessageTypes();
            DSMessageLookup ds = objMessage.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MessageTypes.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.MessageTypes.TableName].Select("1=1", ds.MessageTypes.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.MessageTypes.MgtIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.MessageTypes.ShortNameColumn.ColumnName].ToString(), dr[ds.MessageTypes.MgtIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the MessageStatus.
        /// </summary>
        /// <returns></returns>
        public string GetMessageStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMessageLookup> objMessage = new BLLMessage().LookupMessageStatus();
            DSMessageLookup ds = objMessage.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MessageStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.MessageStatus.TableName].Select("1=1", ds.MessageStatus.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.MessageStatus.MgsIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.MessageStatus.ShortNameColumn.ColumnName].ToString(), dr[ds.MessageStatus.MgsIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the MsgAlertTypes.
        /// </summary>
        /// <returns></returns>
        public string GetMsgAlertTypes(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMessageLookup> objMessage = new BLLMessage().LookupMsgAlertTypes();
            DSMessageLookup ds = objMessage.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MsgAlertTypes.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.MsgAlertTypes.TableName].Select("1=1", ds.MsgAlertTypes.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.MsgAlertTypes.MatIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.MsgAlertTypes.ShortNameColumn.ColumnName].ToString(), dr[ds.MsgAlertTypes.MatIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the MsgPriority.
        /// </summary>
        /// <returns></returns>
        public string GetMsgPriority(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMessageLookup> objMessage = new BLLMessage().LookupMsgPriority();
            DSMessageLookup ds = objMessage.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MsgPriority.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.MsgPriority.TableName].Select("1=1", ds.MsgPriority.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.MsgPriority.PrioIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.MsgPriority.ShortNameColumn.ColumnName].ToString(), dr[ds.MsgPriority.PrioIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Gets the AmendmentSource.
        /// </summary>
        /// <returns></returns>
        public string GetAmendmentSource(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMessageLookup> objMessage = new BLLMessage().LookupAmendmentSource();
            DSMessageLookup ds = objMessage.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.AmendmentSource.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.AmendmentSource.TableName].Select("1=1", ds.AmendmentSource.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.AmendmentSource.AmdtIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.AmendmentSource.ShortNameColumn.ColumnName].ToString(), dr[ds.AmendmentSource.AmdtIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetMessagesPriority(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMessageLookup> objMessage = new BLLMessage().LookupMessagesPriority();
            DSMessageLookup ds = objMessage.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MessagesPriority.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.MessagesPriority.TableName].Select("1=1", ds.MessagesPriority.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.MessagesPriority.PriorityIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.MessagesPriority.ShortNameColumn.ColumnName].ToString(), dr[ds.MessagesPriority.PriorityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        #endregion

        #region Document LookUps
        public string GetDocument(string IsActive)
        {
            string PatientId;
            PatientId = "-1";
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSDocumentLookup> objCode = new BLLDocument().LookupDocument(PatientId);
            DSDocumentLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Documents.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Documents.TableName].Select("1=1", ds.Documents.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Documents.ShortNameColumn.ColumnName].ToString(), dr[ds.Documents.DocIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetDocumentWithCount(string IsActive, string PatientId, string All = "1")
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSDocumentLookup> objCode = new BLLDocument().LookupDocument(PatientId);
            DSDocumentLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Documents.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Documents.TableName].Select("1=1", ds.Documents.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (All == "0" && dr[ds.Documents.ShortNameColumn.ColumnName].ToString().Contains(" (0)"))
                        {

                        }
                        else
                        {
                            list.Add(new NameValuePair(dr[ds.Documents.ShortNameColumn.ColumnName].ToString(), dr[ds.Documents.DocIdColumn.ColumnName].ToString()));
                        }
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetDocumentType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSDocumentLookup> objCode = new BLLDocument().LookupDocumentType();
            DSDocumentLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.DocumentType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.DocumentType.TableName].Select("1=1", ds.DocumentType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.DocumentType.ShortNameColumn.ColumnName].ToString(), dr[ds.DocumentType.DoctypeIdColumn.ColumnName].ToString(), dr[ds.DocumentType.EntityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetFolders(string IsActive)
        {
            string PatientId;
            PatientId = "-1";
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSDocumentLookup> objCode = new BLLDocument().LookupFolders(PatientId);
            DSDocumentLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Documents.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Documents.TableName].Select("1=1", ds.Documents.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Documents.ShortNameColumn.ColumnName].ToString(), dr[ds.Documents.DocIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Gets the Document Provider.
        /// </summary>
        /// <param name="IsActive">IsActive.</param>
        /// <param name="PatientId">Patient Id.</param>
        /// <returns>System.String.</returns>
        public string GetDocumentProvider(string IsActive, long PatientId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            var model = new BLLDocument().LookupDocumentProvider(PatientId);

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Name, item.Id));
                    }
                }

            }
            return getJSONofList(list);
        }

        #endregion

        #region Case LookUps
        public string GetCaseType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCaseLookup> objCode = new BLLPatient().LookupCaseType();
            DSCaseLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.CaseType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.CaseType.TableName].Select("1=1", ds.CaseType.CaseTypeIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.CaseType.ShortNameColumn.ColumnName].ToString(), dr[ds.CaseType.CaseTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetConditionCode(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCaseLookup> objCode = new BLLPatient().LookupConditionCode();
            DSCaseLookup ds = objCode.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ConditionCodes.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ConditionCodes.TableName].Select("1=1", ds.ConditionCodes.ConditionCodeIdColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ConditionCodes.ConditionCodeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ConditionCodes.ShortNameColumn.ColumnName].ToString(), dr[ds.ConditionCodes.ConditionCodeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region Schedule Lookups
        public string GetScheduleGroup(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSScheduleLookups> objschedule = new BLLSchedule().LookupScheduleGroups();
            DSScheduleLookups ds = objschedule.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MultipleSchedualGroups.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.MultipleSchedualGroups.TableName].Select("1=1", ds.MultipleSchedualGroups.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.MultipleSchedualGroups.ShortNameColumn.ColumnName].ToString(), dr[ds.MultipleSchedualGroups.MSGroupIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetAppVisitStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCharge> objPractice = new BLLBilling().LookupAppVisitStatus();
            DSCharge ds = objPractice.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.VisitStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VisitStatus.TableName].Select("1=1", ds.VisitStatus.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.VisitStatus.VStatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.VisitStatus.ShortNameColumn.ColumnName].ToString(), dr[ds.VisitStatus.VStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetPreferredTime(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSAppointment> objPractice = new BLLSchedule().LookupPreferredTime();
            DSAppointment ds = objPractice.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.PreferredTime.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PreferredTime.TableName].Select("1=1", ds.PreferredTime.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.PreferredTime.PrfTimeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.PreferredTime.ShortNameColumn.ColumnName].ToString(), dr[ds.PreferredTime.PrfTimeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetWaitListStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSAppointment> objPractice = new BLLSchedule().LookupWaitListStatus();
            DSAppointment ds = objPractice.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.PreferredTime.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.WaitListStatus.TableName].Select("1=1", ds.WaitListStatus.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.WaitListStatus.wtListStatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.WaitListStatus.ShortNameColumn.ColumnName].ToString(), dr[ds.WaitListStatus.wtListStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetPatientType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSScheduleLookups> objPatientType = new BLLSchedule().LookupPatientType();
            DSScheduleLookups ds = objPatientType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.PatientType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PatientType.TableName].Select("1=1", ds.PatientType.PatientTypeColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PatientType.PatientTypeColumn.ColumnName].ToString(), dr[ds.PatientType.PatientTypeIDColumn.ColumnName].ToString(), dr[ds.PatientType.PatientTypeIDColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetPatientVisitType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSScheduleLookups> objVisitType = new BLLSchedule().LookupPatientVisitType();
            DSScheduleLookups ds = objVisitType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.PatientVisitType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PatientVisitType.TableName].Select("1=1", ds.PatientVisitType.VisitTypeColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PatientVisitType.VisitTypeColumn.ColumnName].ToString(), dr[ds.PatientVisitType.VisitTypeIDColumn.ColumnName].ToString(), dr[ds.PatientVisitType.PatientTypeIDColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetPatientVisitTypeWithoutCancerRegistries(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<List<PatientVisitTypeLookUpModel>> objVisitType = new BLLSchedule().LookupPatientVisitType_WO_CancerRegistries();
            list.Add(new NameValuePair("- Select -", ""));
            var data = objVisitType.Data;
            if (data != null)
                foreach (var row in data)
                    list.Add(new NameValuePair(row.VisitType, row.VisitTypeID, row.PatientTypeID));
            return getJSONofList(list);
        }
        public string GetVisitType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSScheduleLookups> objVisitType = new BLLSchedule().LookupVisitType();
            DSScheduleLookups ds = objVisitType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.VisitType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VisitType.TableName].Select("1=1", ds.VisitType.VisitTypeIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.VisitType.ShortNameColumn.ColumnName].ToString(), dr[ds.VisitType.VisitTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region Visits Lookups

        public string GetPatientVisits(string IsActive, Int64 PatientId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVisitLookup> objvisits = new BLLVisits().LookupPatientVisits(PatientId);
            DSVisitLookup ds = objvisits.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PatientVisits.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PatientVisits.TableName].Select("1=1", ds.PatientVisits.AppointmentDateColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PatientVisits.AppointmentDateColumn.ColumnName].ToString(), dr[ds.PatientVisits.VisitIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetVisitDelayReason(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVisitLookup> objvisits = new BLLVisits().LookupVisitsDelayReason();
            DSVisitLookup ds = objvisits.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.DelayReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.DelayReason.TableName].Select("1=1", ds.DelayReason.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.DelayReason.DRIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.DelayReason.ShortNameColumn.ColumnName].ToString(), dr[ds.DelayReason.DRIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetVisitClaimFrequency(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVisitLookup> objvisits = new BLLVisits().LookupVisitsClaimFrequency();
            DSVisitLookup ds = objvisits.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ClaimFrequency.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ClaimFrequency.TableName].Select("1=1", ds.ClaimFrequency.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ClaimFrequency.CFIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ClaimFrequency.ShortNameColumn.ColumnName].ToString(), dr[ds.ClaimFrequency.CFIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetReportTypeCode(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVisitLookup> objvisits = new BLLVisits().LookupReportTypeCode();
            DSVisitLookup ds = objvisits.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ReportTypeCode.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ReportTypeCode.TableName].Select("1=1", ds.ReportTypeCode.CodeColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ReportTypeCode.RTCIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ReportTypeCode.CodeColumn.ColumnName].ToString(), dr[ds.ReportTypeCode.RTCIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        public string GetTransmissionCode(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVisitLookup> objvisits = new BLLVisits().LookupTransmissionCode();
            DSVisitLookup ds = objvisits.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.TransmissionCode.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.TransmissionCode.TableName].Select("1=1", ds.TransmissionCode.CodeColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.TransmissionCode.TCIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.TransmissionCode.CodeColumn.ColumnName].ToString(), dr[ds.TransmissionCode.TCIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }



        public string GetVisitStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVisitLookup> objvisits = new BLLVisits().LookupVisitStatus();
            DSVisitLookup ds = objvisits.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VisitStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VisitStatus.TableName].Select("1=1", ds.VisitStatus.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.VisitStatus.VStatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.VisitStatus.ShortNameColumn.ColumnName].ToString(), dr[ds.VisitStatus.VStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetSubmitStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVisitLookup> objvisits = new BLLVisits().LookupSubmitStatus(IsActive);
            DSVisitLookup ds = objvisits.Data;
            //list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SubmitStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SubmitStatus.TableName].Select("1=1", ds.SubmitStatus.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SubmitStatus.SubmitStatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SubmitStatus.ShortNameColumn.ColumnName].ToString(), dr[ds.SubmitStatus.SubmitStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetAllSubmitStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVisitLookup> objvisits = new BLLVisits().LookupSubmitStatus(IsActive);
            DSVisitLookup ds = objvisits.Data;
            list.Add(new NameValuePair("All", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SubmitStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SubmitStatus.TableName].Select("1=1", ds.SubmitStatus.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SubmitStatus.SubmitStatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SubmitStatus.ShortNameColumn.ColumnName].ToString(), dr[ds.SubmitStatus.SubmitStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetAnesthesiaType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVisitLookup> objvisits = new BLLVisits().LookupAnesthesiaType();
            DSVisitLookup ds = objvisits.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.AnesthesiaType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.AnesthesiaType.TableName].Select("1=1", ds.AnesthesiaType.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.AnesthesiaType.AnesTypeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.AnesthesiaType.DescriptionColumn.ColumnName].ToString(), dr[ds.AnesthesiaType.AnesTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetAnesServiceType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVisitLookup> objvisits = new BLLVisits().LookupAnesServiceType();
            DSVisitLookup ds = objvisits.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.AnesServiceType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.AnesServiceType.TableName].Select("1=1", ds.AnesServiceType.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.AnesServiceType.ServiceTypeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.AnesServiceType.DescriptionColumn.ColumnName].ToString(), dr[ds.AnesServiceType.ServiceTypeIdColumn.ColumnName].ToString(), dr[ds.AnesServiceType.ModifierColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetAnesthesiaASA(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVisitLookup> objvisits = new BLLVisits().LookupAnesthesiaASA();
            DSVisitLookup ds = objvisits.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ASA.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ASA.TableName].Select("1=1", ds.ASA.PhysicalStatusColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ASA.ASAColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ASA.PhysicalStatusColumn.ColumnName].ToString(), dr[ds.ASA.ASAColumn.ColumnName].ToString(), dr[ds.ASA.RiskUnitsColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetVoidedStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVisitLookup> objvisits = new BLLVisits().LookupVoidedStatus(IsActive);
            DSVisitLookup ds = objvisits.Data;
            //list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.DT_Voided_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.DT_Voided_Status.TableName].Select("1=1", ds.DT_Voided_Status.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.DT_Voided_Status.VoidedStatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.DT_Voided_Status.ShortNameColumn.ColumnName].ToString(), dr[ds.DT_Voided_Status.VoidedStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region Batches Lookups
        public string GetChargeBatches(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSCharge> objCharges = new BLLBilling().LookupBatches(IsActive);
            DSCharge ds = objCharges.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.BactheLookups.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.BactheLookups.TableName].Select("1=1", ds.BactheLookups.BatchNumberColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BactheLookups.BatchNumberColumn.ColumnName].ToString(), dr[ds.BactheLookups.BatchIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion
        #region Billing Lookups

        public string GetServiceType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodes> objServiceType = new BLLAdminCodes().LookupServiceType(IsActive);
            DSCodes ds = objServiceType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ServiceType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ServiceType.TableName].Select("1=1", ds.ServiceType.CodeColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ServiceType.ServiceTypeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ServiceType.CodeColumn.ColumnName].ToString() + "-" + dr[ds.ServiceType.DescriptionColumn.ColumnName].ToString(), dr[ds.ServiceType.CodeColumn].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetLedgerType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPaymentSetup> objPaymentSetup = new BLLBilling().LookupLedgerType(IsActive);
            DSPaymentSetup ds = objPaymentSetup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LedgerAccountType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LedgerAccountType.TableName].Select("1=1", ds.LedgerAccountType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.LedgerAccountType.LedgerAccountTypeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.LedgerAccountType.ShortNameColumn.ColumnName].ToString(), dr[ds.LedgerAccountType.LedgerAccountTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetCreditCardType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPaymentLookup> objPaymentSetup = new BLLBilling().LookupCreditCardType();
            DSPaymentLookup ds = objPaymentSetup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.CreditCardType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.CreditCardType.TableName].Select("1=1", ds.CreditCardType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.CreditCardType.CardIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.CreditCardType.ShortNameColumn.ColumnName].ToString(), dr[ds.CreditCardType.CardIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetCRemittanceCode(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPaymentLookup> objPaymentSetup = new BLLBilling().LookupRemittanceCode();
            DSPaymentLookup ds = objPaymentSetup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.RemittanceCode.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.RemittanceCode.TableName].Select("1=1", ds.RemittanceCode.CodeColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.RemittanceCode.RemittanceIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.RemittanceCode.CodeColumn.ColumnName].ToString(), dr[ds.RemittanceCode.RemittanceIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetPaymentType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPaymentSetup> objPaymentSetup = new BLLBilling().LookupPaymentType(IsActive);
            DSPaymentSetup ds = objPaymentSetup.Data;
            // list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PaymentType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PaymentType.TableName].Select("1=1", ds.PaymentType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.PaymentType.PmtTypeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.PaymentType.ShortNameColumn.ColumnName].ToString(), dr[ds.PaymentType.PmtTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetPaymentBatch(string BatchNumber)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPaymentLookup> objPaymentSetup = new BLLBilling().LookupPaymentBatch(BatchNumber);
            DSPaymentLookup ds = objPaymentSetup.Data;
            //list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PaymentBatch.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PaymentBatch.TableName].Select("1=1", ds.PaymentBatch.PmtBatchNumberColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PaymentBatch.PmtBatchNumberColumn.ColumnName].ToString(), dr[ds.PaymentBatch.PmtBatchIdColumn.ColumnName].ToString(), dr[ds.PaymentBatch.CheckNumberColumn.ColumnName].ToString(), dr[ds.PaymentBatch.CheckDateColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        public string GetLedgerAccount(string IsActive, Int64 TypeId, Int64 ApplyToId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            //Int64 IPId = 0;
            if (TypeId == -1 && ApplyToId == -1)
            {
                TypeId = 0;
                ApplyToId = 0;
            }
            //Int64 GuarantorID = MDVUtility.ToInt64(context.Request["ID"]);
            BLObject<DSPaymentSetup> objInsLookup = new BLLBilling().LookupLedgerAccount(TypeId, ApplyToId);
            DSPaymentSetup ds = objInsLookup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LedgerAccount1.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LedgerAccount1.TableName].Select("1=1", ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        string Description = string.Empty;

                        if (Convert.ToBoolean(dr[ds.LedgerAccount1.IsSystemColumn.ColumnName]) == false)
                        {
                            Description = MDVUtility.ToStr(dr[ds.LedgerAccount1.DescriptionColumn.ColumnName]);
                            if (!string.IsNullOrEmpty(Description))
                                Description = " - " + Description;
                        }

                        // list.Add(new NameValuePair(dr[ds.LedgerAccount1.ShortNameColumn.ColumnName].ToString(), dr[ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName].ToString()));
                        list.Add(new NameValuePair(dr[ds.LedgerAccount1.ShortNameColumn.ColumnName].ToString() + Description, dr[ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName].ToString(), dr[ds.LedgerAccount1.IsSystemColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetLedgerAccountCopay(string IsActive, Int64 TypeId, Int64 ApplyToId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            //Int64 IPId = 0;
            if (TypeId == -1 && ApplyToId == -1)
            {
                TypeId = 0;
                ApplyToId = 0;
            }
            //Int64 GuarantorID = MDVUtility.ToInt64(context.Request["ID"]);
            BLObject<DSPaymentSetup> objInsLookup = new BLLBilling().LookupLedgerAccount(TypeId, ApplyToId);
            DSPaymentSetup ds = objInsLookup.Data;
            //list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LedgerAccount1.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LedgerAccount1.TableName].Select("1=1", ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        string Description = string.Empty;
                        if (MDVUtility.ToStr(dr[ds.LedgerAccount1.ShortNameColumn.ColumnName]) == "MPS - Patient Payment" || MDVUtility.ToStr(dr[ds.LedgerAccount1.ShortNameColumn.ColumnName]) == "Patient Discount" ||
                            MDVUtility.ToStr(dr[ds.LedgerAccount1.ShortNameColumn.ColumnName]) == "Patient Payment" || MDVUtility.ToStr(dr[ds.LedgerAccount1.ShortNameColumn.ColumnName]) == "Copay Payment" ||
                            MDVUtility.ToStr(dr[ds.LedgerAccount1.ShortNameColumn.ColumnName]) == "Copay Discount")
                        {
                            if (Convert.ToBoolean(dr[ds.LedgerAccount1.IsSystemColumn.ColumnName]) == false)
                            {
                                Description = MDVUtility.ToStr(dr[ds.LedgerAccount1.DescriptionColumn.ColumnName]);
                                if (!string.IsNullOrEmpty(Description))
                                    Description = " - " + Description;
                            }

                            // list.Add(new NameValuePair(dr[ds.LedgerAccount1.ShortNameColumn.ColumnName].ToString(), dr[ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName].ToString()));
                            list.Add(new NameValuePair(dr[ds.LedgerAccount1.ShortNameColumn.ColumnName].ToString() + Description, dr[ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName].ToString(), dr[ds.LedgerAccount1.IsSystemColumn.ColumnName].ToString()));
                        }
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetLedgerAccountForPatient(string IsActive, Int64 TypeId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            //Int64 IPId = 0;
            if (TypeId == -1)
            {
                TypeId = 0;
                // ApplyToId = 0;
            }
            //Int64 GuarantorID = MDVUtility.ToInt64(context.Request["ID"]);
            BLObject<DSPaymentSetup> objInsLookup = new BLLBilling().LookupLedgerAccountForPatientAndCopay(TypeId);
            DSPaymentSetup ds = objInsLookup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LedgerAccount1.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LedgerAccount1.TableName].Select("1=1", ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        string Description = string.Empty;

                        if (Convert.ToBoolean(dr[ds.LedgerAccount1.IsSystemColumn.ColumnName]) == false)
                        {
                            Description = MDVUtility.ToStr(dr[ds.LedgerAccount1.DescriptionColumn.ColumnName]);
                            if (!string.IsNullOrEmpty(Description))
                                Description = " - " + Description;
                        }

                        // list.Add(new NameValuePair(dr[ds.LedgerAccount1.ShortNameColumn.ColumnName].ToString(), dr[ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName].ToString()));
                        list.Add(new NameValuePair(dr[ds.LedgerAccount1.ShortNameColumn.ColumnName].ToString() + Description, dr[ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName].ToString(), dr[ds.LedgerAccount1.IsSystemColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetAdvancePaymentLedgerAccount(string IsActive, Int64 TypeId, Int64 ApplyToId, Int64 SystemCategory)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            //Int64 IPId = 0;

            //Int64 GuarantorID = MDVUtility.ToInt64(context.Request["ID"]);
            BLObject<DSPaymentSetup> objInsLookup = new BLLBilling().LookupLedgerAccount(TypeId, ApplyToId, SystemCategory);
            DSPaymentSetup ds = objInsLookup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LedgerAccount1.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LedgerAccount1.TableName].Select("1=1", ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.LedgerAccount1.ShortNameColumn.ColumnName].ToString(), dr[ds.LedgerAccount1.LedgerAccountIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetDocumentPriority(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var objPriorityLookup = new BLLPatient().LoadPatientDocumentPriority().OrderByDescending(x => x.DocumentPriorityId);

            list.Add(new NameValuePair("- Select -", ""));
            foreach (var item in objPriorityLookup)
            {
                list.Add(new NameValuePair(item.Name, item.DocumentPriorityId));
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Gets the Ledger Apply To.
        /// </summary>
        /// <returns></returns>
        public string GetLedgerApplyTo(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPaymentSetup> objPaymentSetup = new BLLBilling().LookupLedgerApplyTo(IsActive);
            DSPaymentSetup ds = objPaymentSetup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LedgerApplyTo.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LedgerApplyTo.TableName].Select("1=1", ds.LedgerApplyTo.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.LedgerApplyTo.LedgerApplyToIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.LedgerApplyTo.ShortNameColumn.ColumnName].ToString(), dr[ds.LedgerApplyTo.LedgerApplyToIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Gets the Ledger System Category.
        /// </summary>
        /// <returns></returns>
        public string GetLedgerSystemCategory(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPaymentSetup> objInsurance = new BLLBilling().LookupLedgerSystemAccount(IsActive);
            DSPaymentSetup ds = objInsurance.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LedgerSystemCategory.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LedgerSystemCategory.TableName].Select("1=1", ds.LedgerSystemCategory.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.LedgerSystemCategory.LedgerSystemCategoryIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.LedgerSystemCategory.ShortNameColumn.ColumnName].ToString(), dr[ds.LedgerSystemCategory.LedgerSystemCategoryIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetSupperBill(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSupperBillLookup> objSupperbill = new BLLBilling().LookupSupperBill(MDVUtility.ToInt64(MDVSession.Current.DefaultPracticeId));
            DSSupperBillLookup ds = objSupperbill.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SuperBillsLookUp.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SuperBillsLookUp.TableName].Select("1=1", ds.SuperBillsLookUp.SuperBillIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SuperBillsLookUp.ShortNameColumn.ColumnName].ToString(), dr[ds.SuperBillsLookUp.SuperBillIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        public string GetStatementMessage(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientStatementLookup> objStatementMessage = new BLLBilling().LookupStatementMessage();
            DSPatientStatementLookup ds = objStatementMessage.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.StatementMessageLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.StatementMessageLookup.TableName].Select("1=1", ds.StatementMessageLookup.StmtMsgIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.StatementMessageLookup.ShortNameColumn.ColumnName].ToString(), dr[ds.StatementMessageLookup.StmtMsgIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        public string GetStatementGroup(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientStatementLookup> objStatementGroup = new BLLBilling().LookupStatementGroup();
            DSPatientStatementLookup ds = objStatementGroup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PatientStatementGroupLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PatientStatementGroupLookup.TableName].Select("1=1", ds.PatientStatementGroupLookup.PtStmtGrpIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PatientStatementGroupLookup.NameColumn.ColumnName].ToString(), dr[ds.PatientStatementGroupLookup.PtStmtGrpIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        #endregion

        #region Clinical

        public string GetQuestionType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSTemplateBuilderLookups> objquestions = new BLLClinical().LookupQuestionType();
            DSTemplateBuilderLookups ds = objquestions.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.QuestionType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.QuestionType.TableName].Select("1=1", ds.QuestionType.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.QuestionType.DescriptionColumn.ColumnName].ToString(), dr[ds.QuestionType.QuestionTypeIDColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }

        public string GetSectionType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSTemplateBuilderLookups> objsection = new BLLClinical().LookupSectionType();
            DSTemplateBuilderLookups ds = objsection.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SectionType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SectionType.TableName].Select("1=1", ds.SectionType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SectionType.ShortNameColumn.ColumnName].ToString(), dr[ds.SectionType.SectionTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        // Labs
        public string GetLabCategory(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalLab> objCategory = new BLLClinical().LookupLabCategory();
            DSClinicalLab ds = objCategory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LabCategory.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LabCategory.TableName].Select("1=1", ds.LabCategory.CategoryNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.LabCategory.CategoryNameColumn.ColumnName].ToString(), dr[ds.LabCategory.CategoryIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        public string GetProcedureTemplateAssociation(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProcedureTemplate> objCategory = new BLLProcedureTemplates().LookupTempAssociation();
            DSProcedureTemplate ds = objCategory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ProcedureTemplateAssociationLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ProcedureTemplateAssociationLookup.TableName].Select("1=1", ds.ProcedureTemplateAssociationLookup.NameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ProcedureTemplateAssociationLookup.NameColumn.ColumnName].ToString(), dr[ds.ProcedureTemplateAssociationLookup.IdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }


        public string GetProcedureTemplateNoteView(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProcedureTemplate> objCategory = new BLLProcedureTemplates().LookupTempNoteView();
            DSProcedureTemplate ds = objCategory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ProcedureTemplateNoteViewLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ProcedureTemplateNoteViewLookup.TableName].Select("1=1", ds.ProcedureTemplateNoteViewLookup.NameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ProcedureTemplateNoteViewLookup.NameColumn.ColumnName].ToString(), dr[ds.ProcedureTemplateNoteViewLookup.IdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }


        public string GetEncounterType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSNotes> objType = new BLLBilling().LookupEncounterType();
            DSNotes ds = objType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.EncounterTypeLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.EncounterTypeLookup.TableName].Select("1=1", ds.EncounterTypeLookup.EncounterTypeNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.EncounterTypeLookup.EncounterTypeNameColumn.ColumnName].ToString(), dr[ds.EncounterTypeLookup.EncounterTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }

        public string GetInformant(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<List<InformantLookup>> listInformants = new BLLClinical().LookupInformant();

            list.Add(new NameValuePair("- Select -", ""));
            var data = listInformants.Data;
            if (data != null)
            {
                foreach (var row in data)
                {
                    list.Add(new NameValuePair(row.Description, row.Id));
                }

            }

            return getJSONofList(list);
        }

        public string GetLaterality(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<List<LateralityLookup>> listInformants = new BLLClinical().LookupLaterality();

            list.Add(new NameValuePair("- Select -", ""));
            var data = listInformants.Data;
            if (data != null)
            {
                foreach (var row in data)
                {
                    list.Add(new NameValuePair(row.Description, row.Id));
                }

            }

            return getJSONofList(list);
        }

        public string GetVisitTypeDurationGroup(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSScheduleLookups> objType = new BLLSchedule().LookupVisitTypeDurationGroup();
            DSScheduleLookups ds = objType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VisitTypeDurationGroup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VisitTypeDurationGroup.TableName].Select("1=1", ds.VisitTypeDurationGroup.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.VisitTypeDurationGroup.ShortNameColumn.ColumnName].ToString(), dr[ds.VisitTypeDurationGroup.IdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }

        public string GetLabRequisitionTemplate(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalLab> objRequisitionTemplate = new BLLClinical().LookupLabRequisitionTemplate();
            DSClinicalLab ds = objRequisitionTemplate.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LabRequisitionTemplate.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LabRequisitionTemplate.TableName].Select("1=1", ds.LabRequisitionTemplate.RequisitionTemplateNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.LabRequisitionTemplate.RequisitionTemplateNameColumn.ColumnName].ToString(), dr[ds.LabRequisitionTemplate.RequisitionTemplateIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        public string GetLabCodeSystem(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalLab> objCodeSystem = new BLLClinical().LookupLabCodeSystem();
            DSClinicalLab ds = objCodeSystem.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LabCodeSystem.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LabCodeSystem.TableName].Select("1=1", ds.LabCodeSystem.CodeSystemNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.LabCodeSystem.CodeSystemNameColumn.ColumnName].ToString(), dr[ds.LabCodeSystem.CodeSystemIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        public string GetLabType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalLab> objType = new BLLClinical().LookupLabType();
            DSClinicalLab ds = objType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LabType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LabType.TableName].Select("1=1", ds.LabType.LabTypeNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.LabType.LabTypeNameColumn.ColumnName].ToString(), dr[ds.LabType.LabTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }


        public string LookupLetterFieldFormat(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSLetterLookup> objFieldFormat = new BLLClinical().LookupLetterFieldFormat();
            DSLetterLookup ds = objFieldFormat.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FieldFromat.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.FieldFromat.TableName].Select("1=1", ds.FieldFromat.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.FieldFromat.ShortNameColumn.ColumnName].ToString(), dr[ds.FieldFromat.FieldFormatIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetLetterfields(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSLetterLookup> objFieldsLookup = new BLLClinical().LookupLetterFields();
            DSLetterLookup ds = objFieldsLookup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Fields.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Fields.TableName].Select("1=1", ds.Fields.FieldIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Fields.DescriptionColumn.ColumnName].ToString(), (dr[ds.Fields.FieldIdColumn.ColumnName].ToString() + "~" + dr[ds.Fields.NameColumn.ColumnName].ToString())));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetBodySystem(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSTemplateBuilderLookups> objquestions = new BLLClinical().LookupBodySystem();
            DSTemplateBuilderLookups ds = objquestions.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BodySystems.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.BodySystems.TableName].Select("1=1", ds.BodySystems.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.BodySystems.BodySystemIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.BodySystems.ShortNameColumn.ColumnName].ToString(), dr[ds.BodySystems.BodySystemIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        public string GetLetters(string IsActive, Int64 CategoryId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            Int64 IPId = 0;
            BLObject<DSLetterLookup> objInsLookup = new BLLClinical().LookupLetters(CategoryId);
            DSLetterLookup ds = objInsLookup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Letter.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Letter.TableName].Select("1=1", ds.Letter.LetterIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Letter.LetterNameColumn.ColumnName].ToString(), dr[ds.Letter.LetterIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetLetters(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            Int64 IPId = 0;
            BLObject<DSLetterLookup> objInsLookup = new BLLClinical().LookupLetters(0);
            DSLetterLookup ds = objInsLookup.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Letter.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Letter.TableName].Select("1=1", ds.Letter.LetterIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Letter.LetterNameColumn.ColumnName].ToString(), dr[ds.Letter.LetterIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetTemplateType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSTemplateBuilderLookups> objTemplateTypes = new BLLClinical().LookupTemplateType();
            DSTemplateBuilderLookups ds = objTemplateTypes.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.QuestionType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.TemplateType.TableName].Select("1=1", ds.TemplateType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.TemplateType.ShortNameColumn.ColumnName].ToString(), dr[ds.TemplateType.TemplateTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }



        public string GetTemplateType(string IsActive, Int64 isNote, Int64 TemplateID)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSTemplateBuilderLookups> objTemplateTypes = new BLLClinical().LookupTemplateType(isNote, TemplateID);
            DSTemplateBuilderLookups ds = objTemplateTypes.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.QuestionType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.TemplateType.TableName].Select("1=1", ds.TemplateType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.TemplateType.ShortNameColumn.ColumnName].ToString(), dr[ds.TemplateType.TemplateTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        public string GetTemplate(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSTemplateBuilderLookups> objTemplate = new BLLClinical().LookupTemplate();
            DSTemplateBuilderLookups ds = objTemplate.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Template.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Template.TableName].Select("1=1", ds.Template.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Template.ShortNameColumn.ColumnName].ToString(), dr[ds.Template.TemplateIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }


        public string GetTemplate(string IsActive, Int64 TemplateTypeID)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSTemplateBuilderLookups> objTemplate = new BLLClinical().LookupTemplate(TemplateTypeID);
            DSTemplateBuilderLookups ds = objTemplate.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Template.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Template.TableName].Select("1=1", ds.Template.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Template.ShortNameColumn.ColumnName].ToString(), dr[ds.Template.TemplateIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        /*Author: Azhar Sial, Date: 03/17/2016*/
        public string GetNoteTemplate(string IsActive, Int64 TemplateTypeID, Int64 ProviderID)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalNoteTemplateLookup> objTemplate = new BLLClinical().LookupNotesTemplate(TemplateTypeID, ProviderID);
            DSClinicalNoteTemplateLookup ds = objTemplate.Data;
            list.Add(new NameValuePair("- Blank -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.NotesTemplateLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.NotesTemplateLookup.TableName].Select("1=1", ds.NotesTemplateLookup.NoteTemplateNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.NotesTemplateLookup.NoteTemplateNameColumn.ColumnName].ToString(), dr[ds.NotesTemplateLookup.NotesTemplateIdColumn.ColumnName].ToString(), dr[ds.NotesTemplateLookup.TemplateTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }

        public string GetNoteTemplateType(string IsActive, Int64 TemplateID)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalNoteTemplateLookup> objTemplateTypes = new BLLClinical().GetNoteTemplateType(string.IsNullOrEmpty(IsActive) ? true : Convert.ToBoolean(MDVUtility.ToInt32(IsActive)), TemplateID);
            DSClinicalNoteTemplateLookup ds = objTemplateTypes.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.NotesTemplateType.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.NotesTemplateType.TableName].Select("1=1", ds.NotesTemplateType.NotesTemplateTypeIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.NotesTemplateType.ShortNameColumn.ColumnName].ToString(), dr[ds.NotesTemplateType.NotesTemplateTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }



        public string GetPhoneEncounterDuration(string IsActive, string PatientId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSResources> objschedule = new BLLClinical().LookupDuration(MDVUtility.ToConvertInt32(PatientId));
            DSResources ds = objschedule.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Duration.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Duration.TableName].Select("1=1", ds.Duration.DurationIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Duration.DurationColumn.ColumnName].ToString(), dr[ds.Duration.CPTCodeColumn.ColumnName].ToString(), dr[ds.Duration.IsPhysicianColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetRooms(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            long FacilityId = string.IsNullOrEmpty(MDVSession.Current.DefaultFacilityId) ? 0 : MDVUtility.ToInt64(MDVSession.Current.DefaultFacilityId);
            BLObject<DSResources> objschedule = new BLLClinical().LookupRooms(FacilityId);
            DSResources ds = objschedule.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Rooms.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Rooms.TableName].Select("1=1", ds.Rooms.RoomNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Rooms.RoomNameColumn.ColumnName].ToString(), dr[ds.Rooms.RoomsIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /*
            Purpose: this function get the rooms ddl based on facility selected
            Author: Muhammad Azhar Shahzad
            Created Date: March 24,2016
        */
        public string GetRooms(string IsActive, Int64 FacilityId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            FacilityId = FacilityId != 0 ? FacilityId : MDVUtility.ToInt64(MDVSession.Current.DefaultFacilityId);
            BLObject<DSResources> objschedule = new BLLClinical().LookupRooms(FacilityId);
            DSResources ds = objschedule.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Rooms.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Rooms.TableName].Select("1=1", ds.Rooms.RoomNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Rooms.RoomNameColumn.ColumnName].ToString(), dr[ds.Rooms.RoomsIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetClinicalAppAndVisitStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSAppointment> objschedule = new BLLSchedule().LookupAppointmentAndVisitStatus();
            DSAppointment ds = objschedule.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.AppVisitStatusLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.AppVisitStatusLookup.TableName].Select("1=1", ds.AppVisitStatusLookup.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.AppVisitStatusLookup.ShortNameColumn.ColumnName].ToString(), dr[ds.AppVisitStatusLookup.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetAllVitalSigns(string IsActive, string DropDownValue)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSVitals> objPatient = new BLLClinical().LookupAllVitalSigns(DropDownValue);
            DSVitals ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VitalSignsLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VitalSignsLookup.TableName].Select("1=1", ds.VitalSignsLookup.ValueColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.VitalSignsLookup.LookUpIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.VitalSignsLookup.ValueColumn.ColumnName].ToString(), dr[ds.VitalSignsLookup.LookUpIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        // Start 27/11/2015 Muhammad Irfan Created to use dropdown values from DB end
        public string GetChronicityLevel(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProblemLists> objschedule = new BLLClinical().LookupChronicityLevel();
            DSProblemLists ds = objschedule.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ChronicityLevel.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ChronicityLevel.TableName].Select("1=1", ds.ChronicityLevel.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ChronicityLevel.ShortNameColumn.ColumnName].ToString(), dr[ds.ChronicityLevel.ShortNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetSeverityType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProblemLists> objschedule = new BLLClinical().LookupSeverityType();
            DSProblemLists ds = objschedule.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SeverityType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SeverityType.TableName].Select("1=1", ds.SeverityType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SeverityType.ShortNameColumn.ColumnName].ToString(), dr[ds.SeverityType.ShortNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        //Start//01/12/2015///Ahmad Raza///Lookup for severity drop down in Allergies add
        public string getSeverity(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSAllergyLookup> objAllergy = new BLLClinical().lookupSeverity();
            DSAllergyLookup ds = objAllergy.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Severity.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Severity.TableName].Select("1=1", ds.Severity.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Severity.ShortNameColumn.ColumnName].ToString(), dr[ds.Severity.ShortNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        //End//01/12/2015///Ahmad Raza///Lookup for severity drop down in Allergies add

        //Start//01/12/2015///Ahmad Raza///Lookup for AllergyType drop down in Allergies add
        public string getAllergyType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSAllergyLookup> objAllergy = new BLLClinical().lookupAllergyType();
            DSAllergyLookup ds = objAllergy.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.AllergyType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.AllergyType.TableName].Select("1=1", ds.AllergyType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.AllergyType.ShortNameColumn.ColumnName].ToString(), dr[ds.AllergyType.ShortNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        //End//01/12/2015///Ahmad Raza///Lookup for AllergyType drop down in Allergies add

        public string GetNoteStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProblemLists> objschedule = new BLLClinical().LookupNoteStatus();
            DSProblemLists ds = objschedule.Data;
            list.Add(new NameValuePair("All", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.NoteStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.NoteStatus.TableName].Select("1=1", ds.NoteStatus.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.NoteStatus.ShortNameColumn.ColumnName].ToString(), dr[ds.NoteStatus.NoteStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetNoteAction(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProblemLists> objschedule = new BLLClinical().LookupNoteAction();
            DSProblemLists ds = objschedule.Data;
            list.Add(new NameValuePair("All", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.NoteAction.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.NoteAction.TableName].Select("1=1", ds.NoteAction.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.NoteAction.ShortNameColumn.ColumnName].ToString(), dr[ds.NoteAction.NoteActionIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /* End 27/11/2015 Muhammad Irfan Created to use dropdown values from DB end */

        /* Start 03/12/2015 Muhammad Irfan Created for SocialHx lookups */
        public string GetSexualHxComplaints(string IsActive, string Gender)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSexualHxComplaints();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_SexualHx_Complaints.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_SexualHx_Complaints.TableName].Select("1=1", ds.SocialHx_SexualHx_Complaints.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_SexualHx_Complaints.ComplaintIdColumn.ColumnName]))
                    {
                        // Start 09/12/2015 Muhammad Irfan To get the dropdown values gender specific
                        if (dr[ds.SocialHx_SexualHx_Complaints.GenderColumn.ColumnName].ToString() == Gender.ToString())
                        {
                            list.Add(new NameValuePair(dr[ds.SocialHx_SexualHx_Complaints.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_Complaints.ComplaintIdColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_Complaints.GenderColumn.ColumnName].ToString()));
                        }
                        // End 09/12/2015 Muhammad Irfan To get the dropdown values gender specific
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetSexualHxComplaintsWithoutGender(string IsActive, string Gender)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSexualHxComplaints();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_SexualHx_Complaints.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_SexualHx_Complaints.TableName].Select("1=1", ds.SocialHx_SexualHx_Complaints.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_SexualHx_Complaints.ComplaintIdColumn.ColumnName]))
                    {
                        // Start 09/12/2015 Muhammad Irfan To get the dropdown values gender specific
                        //if (dr[ds.SocialHx_SexualHx_Complaints.GenderColumn.ColumnName].ToString() == Gender.ToString())
                        //{
                        list.Add(new NameValuePair(dr[ds.SocialHx_SexualHx_Complaints.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_Complaints.ComplaintIdColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_Complaints.GenderColumn.ColumnName].ToString()));
                        //}
                        // End 09/12/2015 Muhammad Irfan To get the dropdown values gender specific
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetSexualHxProtectionMethod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSexualHxProtectionMethod();
            DSSocialHistory ds = obHistory.Data;
            //Start 29/12/2015 Muhammad Irfan for bug # EMR-183
            list.Add(new NameValuePair("- Method -", ""));
            //End 29/12/2015 Muhammad Irfan for bug # EMR-183
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_SexualHx_ProtectionMethod.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_SexualHx_ProtectionMethod.TableName].Select("1=1", ds.SocialHx_SexualHx_ProtectionMethod.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_SexualHx_ProtectionMethod.ProtectionMethodIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_SexualHx_ProtectionMethod.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_ProtectionMethod.ProtectionMethodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetSexualHxProtectionPeriod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSexualHxProtectionPeriod();
            DSSocialHistory ds = obHistory.Data;
            //Start 29/12/2015 Muhammad Irfan for bug # EMR-183
            list.Add(new NameValuePair("- Period -", ""));
            //End 29/12/2015 Muhammad Irfan for bug # EMR-183
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_SexualHx_ProtectionPeriod.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_SexualHx_ProtectionPeriod.TableName].Select("1=1", ds.SocialHx_SexualHx_ProtectionPeriod.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_SexualHx_ProtectionPeriod.ProtectionPeriodIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_SexualHx_ProtectionPeriod.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_ProtectionPeriod.ProtectionPeriodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetSexualHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSexualHxStatus();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_SexualHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_SexualHx_Status.TableName].Select("1=1", ds.SocialHx_SexualHx_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_SexualHx_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_SexualHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetSexualHxSTD(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSexualHxSTD();
            DSSocialHistory ds = obHistory.Data;
            //list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_SexualHx_STD.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_SexualHx_STD.TableName].Select("1=1", ds.SocialHx_SexualHx_STD.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_SexualHx_STD.STDIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_SexualHx_STD.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_STD.STDIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetTobaccoCounsellingTopic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupTobaccoCounsellingTopic();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Tobacco_CounsellingTopic.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Tobacco_CounsellingTopic.TableName].Select("1=1", ds.SocialHx_Tobacco_CounsellingTopic.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_Tobacco_CounsellingTopic.CounsellingTopicIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Tobacco_CounsellingTopic.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Tobacco_CounsellingTopic.CounsellingTopicIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetTobaccoFrequency(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupTobaccoFrequency();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Tobacco_Frequency.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Tobacco_Frequency.TableName].Select("1=1", ds.SocialHx_Tobacco_Frequency.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_Tobacco_Frequency.FrequencyIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Tobacco_Frequency.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Tobacco_Frequency.FrequencyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetTobaccoSmokingStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupTobaccoSmokingStatus();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Tobacco_SmokingStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Tobacco_SmokingStatus.TableName].Select("1=1", ds.SocialHx_Tobacco_SmokingStatus.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Tobacco_SmokingStatus.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Tobacco_SmokingStatus.StatusIdColumn.ColumnName].ToString(), dr[ds.SocialHx_Tobacco_SmokingStatus.SNOMEDCTCodeColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetTobaccoType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupTobaccoType();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Tobacco_Type.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Tobacco_Type.TableName].Select("1=1", ds.SocialHx_Tobacco_Type.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Tobacco_Type.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Tobacco_Type.TypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetSocialHxUsagePeriod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSocialHxUsagePeriod();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_UsagePeriod.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_UsagePeriod.TableName].Select("1=1", ds.SocialHx_UsagePeriod.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_UsagePeriod.UsagePeriodIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_UsagePeriod.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_UsagePeriod.UsagePeriodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetAlcoholCounsellingTopics(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupAlcoholCounsellingTopics();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Alcohol_CounsellingTopics.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Alcohol_CounsellingTopics.TableName].Select("1=1", ds.SocialHx_Alcohol_CounsellingTopics.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_Alcohol_CounsellingTopics.CounsellingTopicIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Alcohol_CounsellingTopics.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Alcohol_CounsellingTopics.CounsellingTopicIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetAlcoholFrequency(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupAlcoholFrequency();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Alcohol_Frequency.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Alcohol_Frequency.TableName].Select("1=1", ds.SocialHx_Alcohol_Frequency.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_Alcohol_Frequency.FrequencyIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Alcohol_Frequency.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Alcohol_Frequency.FrequencyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetAlcoholStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupAlcoholStatus();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Alcohol_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Alcohol_Status.TableName].Select("1=1", ds.SocialHx_Alcohol_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_Alcohol_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Alcohol_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Alcohol_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetAlcoholType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupAlcoholType();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_Alcohol_Type.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_Alcohol_Type.TableName].Select("1=1", ds.SocialHx_Alcohol_Type.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_Alcohol_Type.TypeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_Alcohol_Type.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_Alcohol_Type.TypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetSocialHxCessationPeriod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSocialHxCessationPeriod();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_CessationPeriod.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_CessationPeriod.TableName].Select("1=1", ds.SocialHx_CessationPeriod.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_CessationPeriod.CessationPeriodIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_CessationPeriod.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_CessationPeriod.CessationPeriodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetDrugAbuseDrug(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupDrugAbuseDrug();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_DrugAbuse_Drug.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_DrugAbuse_Drug.TableName].Select("1=1", ds.SocialHx_DrugAbuse_Drug.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_DrugAbuse_Drug.DrugIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_DrugAbuse_Drug.ShortNameColumn.ColumnName].ToString(), dr[ds.SocialHx_DrugAbuse_Drug.DrugIdColumn.ColumnName].ToString(), dr[ds.SocialHx_DrugAbuse_Drug.DescriptionColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetDrugAbuseFrequencyDaily(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupDrugAbuseFrequencyDaily();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_DrugAbuse_FrequencyDaily.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_DrugAbuse_FrequencyDaily.TableName].Select("1=1", ds.SocialHx_DrugAbuse_FrequencyDaily.DescriptionColumn.ColumnName);

                    // Start 28/12/2015 Muhammad Irfan bug # EMR-165
                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_DrugAbuse_FrequencyDaily.FrequencyDailyIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_DrugAbuse_FrequencyDaily.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_DrugAbuse_FrequencyDaily.FrequencyDailyIdColumn.ColumnName].ToString()));
                    }
                    // End 28/12/2015 Muhammad Irfan bug # EMR-165
                }
            }
            return getJSONofList(list);
        }
        public string GetDrugAbuseFrequencyMonthly(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupDrugAbuseFrequencyMonthly();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_DrugAbuse_FrequencyMonthly.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_DrugAbuse_FrequencyMonthly.TableName].Select("1=1", ds.SocialHx_DrugAbuse_FrequencyMonthly.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_DrugAbuse_FrequencyMonthly.FrequencyMonthlyIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_DrugAbuse_FrequencyMonthly.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_DrugAbuse_FrequencyMonthly.FrequencyMonthlyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetDrugAbuseRoute(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupDrugAbuseRoute();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_DrugAbuse_Route.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_DrugAbuse_Route.TableName].Select("1=1", ds.SocialHx_DrugAbuse_Route.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_DrugAbuse_Route.RouteIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_DrugAbuse_Route.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_DrugAbuse_Route.RouteIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetDrugAbuseStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupDrugAbuseStatus();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_DrugAbuse_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_DrugAbuse_Status.TableName].Select("1=1", ds.SocialHx_DrugAbuse_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_DrugAbuse_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_DrugAbuse_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_DrugAbuse_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /* Start 04/12/2015 Muhammad Irfan Lookup for sp_SocialHx_CounsellingPeriodLookup */
        public string GetSocialHxCounsellingPeriod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSocialHxCounsellingPeriod();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_CounsellingPeriod.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_CounsellingPeriod.TableName].Select("1=1", ds.SocialHx_CounsellingPeriod.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_CounsellingPeriod.CounsellingPeriodIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_CounsellingPeriod.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_CounsellingPeriod.CounsellingPeriodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /* End 04/12/2015 Muhammad Irfan Lookup for sp_SocialHx_CounsellingPeriodLookup */

        /* Start 8/12/2015 Muhammad Irfan Lookup for GetSocialHxPreferences */
        public string GetSocialHxPreferences(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistory> obHistory = new BLLClinical().LookupSexualHxPreferences();
            DSSocialHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_SexualHx_Preferences.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_SexualHx_Preferences.TableName].Select("1=1", ds.SocialHx_SexualHx_Preferences.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_SexualHx_Preferences.PreferenceIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_SexualHx_Preferences.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_SexualHx_Preferences.PreferenceIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /* End 8/12/2015 Muhammad Irfan Lookup for GetSocialHxPreferences */

        /* End 03/12/2015 Muhammad Irfan Created for SocialHx lookups  */


        /** Start MedicalHx Lookups **/
        /** Author : Abid Ali **/
        /** Date : 08/01/2016 **/
        public string GetMedicalHxDurationPeriod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMedicalHxLookup> obHistory = new BLLClinical().LookupMedicalHxDurationPeriod();
            DSMedicalHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MedicalHx_DurationPeriod.TableName] != null)
                {
                    //Start 27-01-2016 Muhammad Arshad Bug#	EMR-235	Medical Hx in Clinical Module -> Duration -> Values of Dropdown list
                    DataRow[] dRows = ds.Tables[ds.MedicalHx_DurationPeriod.TableName].Select("1=1", ds.MedicalHx_DurationPeriod.DurationPeriodIdColumn.ColumnName);
                    //End 27-01-2016 Muhammad Arshad Bug#	EMR-235	Medical Hx in Clinical Module -> Duration -> Values of Dropdown list
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.MedicalHx_DurationPeriod.DescriptionColumn.ColumnName].ToString(), dr[ds.MedicalHx_DurationPeriod.DurationPeriodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetMedicalHxPattern(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMedicalHxLookup> obHistory = new BLLClinical().LookupMedicalHxPattern();
            DSMedicalHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MedicalHx_Pattern.TableName] != null)
                {
                    //Start 27-01-2016 Muhammad Arshad Bug#	EMR-235	Medical Hx in Clinical Module -> Duration -> Values of Dropdown list
                    DataRow[] dRows = ds.Tables[ds.MedicalHx_Pattern.TableName].Select("1=1", ds.MedicalHx_Pattern.PatternIdColumn.ColumnName);
                    //End 27-01-2016 Muhammad Arshad Bug#	EMR-235	Medical Hx in Clinical Module -> Duration -> Values of Dropdown list
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.MedicalHx_Pattern.DescriptionColumn.ColumnName].ToString(), dr[ds.MedicalHx_Pattern.PatternIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetMedicalHxSeverity(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMedicalHxLookup> obHistory = new BLLClinical().LookupMedicalHxSeverity();
            DSMedicalHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MedicalHx_Severity.TableName] != null)
                {
                    //Start 27-01-2016 Muhammad Arshad Bug#	EMR-235	Medical Hx in Clinical Module -> Duration -> Values of Dropdown list
                    DataRow[] dRows = ds.Tables[ds.MedicalHx_Severity.TableName].Select("1=1", ds.MedicalHx_Severity.SeverityIdColumn.ColumnName);
                    //End 27-01-2016 Muhammad Arshad Bug#	EMR-235	Medical Hx in Clinical Module -> Duration -> Values of Dropdown list
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.MedicalHx_Severity.DescriptionColumn.ColumnName].ToString(), dr[ds.MedicalHx_Severity.SeverityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetMedicalHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMedicalHxLookup> obHistory = new BLLClinical().LookupMedicalHxStatus();
            DSMedicalHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MedicalHx_Status.TableName] != null)
                {
                    //Start 27-01-2016 Muhammad Arshad Bug#	EMR-236	Medical Hx in Clinical Module -> Status -> Values of Dropdown list
                    DataRow[] dRows = ds.Tables[ds.MedicalHx_Status.TableName].Select("1=1", ds.MedicalHx_Status.StatusIdColumn.ColumnName);
                    //End 27-01-2016 Muhammad Arshad Bug#	EMR-236	Medical Hx in Clinical Module -> Status -> Values of Dropdown list
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.MedicalHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.MedicalHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        // GetPhoneEncounterDuration

        public string GetMedicalHxTestResults(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMedicalHxLookup> obHistory = new BLLClinical().LookupMedicalHxTestResults();
            DSMedicalHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MedicalHx_TestResult.TableName] != null)
                {
                    //Start 27-01-2016 Muhammad Arshad Bug#	EMR-234	Medical Hx in Clinical Module -> Test result -> Values of Dropdown list
                    DataRow[] dRows = ds.Tables[ds.MedicalHx_TestResult.TableName].Select("1=1", ds.MedicalHx_TestResult.TestResultIdColumn.ColumnName);
                    //End 27-01-2016 Muhammad Arshad Bug#	EMR-234	Medical Hx in Clinical Module -> Test result -> Values of Dropdown list
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.MedicalHx_TestResult.DescriptionColumn.ColumnName].ToString(), dr[ds.MedicalHx_TestResult.TestResultIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetMedicalHxAggravatedBy(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSMedicalHxLookup> obHistory = new BLLClinical().LookupMedicalHxAggravatedBy();
            DSMedicalHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MedicalHx_AggravatedBy.TableName] != null)
                {
                    //Start 27-01-2016 Muhammad Arshad Bug#	EMR-238	Medical Hx in Clinical Module -> Aggravated by -> Values of Dropdown list
                    DataRow[] dRows = ds.Tables[ds.MedicalHx_AggravatedBy.TableName].Select("1=1", ds.MedicalHx_AggravatedBy.AggravatedByIdColumn.ColumnName);
                    //End 27-01-2016 Muhammad Arshad Bug#	EMR-238	Medical Hx in Clinical Module -> Aggravated by -> Values of Dropdown list
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.MedicalHx_AggravatedBy.DescriptionColumn.ColumnName].ToString(), dr[ds.MedicalHx_AggravatedBy.AggravatedByIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /** End MedicalHx Lookups **/
        /** Author : Abid Ali **/
        /** Date : 08/01/2016 **/

        /** start SurgicalHx Lookups **/
        /** Author : Syed Zia **/
        /** Date : 01/20/2016 **/
        public string GetSurgicalHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSurgicalHxLookup> obHistory = new BLLClinical().LookupSurgicalHxStatus();
            DSSurgicalHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SurgicalHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SurgicalHx_Status.TableName].Select("1=1", ds.SurgicalHx_Status.StatusIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SurgicalHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SurgicalHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetSurgicalHxLocation(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSurgicalHxLookup> obHistory = new BLLClinical().LookupSurgicalHxStatus();
            DSSurgicalHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SurgicalHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SurgicalHx_Location.TableName].Select("1=1", ds.SurgicalHx_Location.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SurgicalHx_Location.DescriptionColumn.ColumnName].ToString(), dr[ds.SurgicalHx_Location.LocationIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /** End MedicalHx Lookups **/
        /** Author : Syed Zia **/
        /** Date : 01/12/2016 **/

        /** Start HospitalizationHx Lookups **/
        /** Author : Abid Ali **/
        /** Date : 19/01/2016 **/
        public string GetHospitalizationHxHospital(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSHospitalizationHxLookup> obHistory = new BLLClinical().LookupHospitalizationHxHospital();
            DSHospitalizationHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.HospitalizationHx_Hospital.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.HospitalizationHx_Hospital.TableName].Select("1=1", ds.HospitalizationHx_Hospital.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.HospitalizationHx_Hospital.DescriptionColumn.ColumnName].ToString(), dr[ds.HospitalizationHx_Hospital.HospitalIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetHospitalizationHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSHospitalizationHxLookup> obHistory = new BLLClinical().LookupHospitalizationHxStatus();
            DSHospitalizationHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.HospitalizationHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.HospitalizationHx_Status.TableName].Select("1=1", ds.HospitalizationHx_Status.DescriptionColumn.ColumnName);
                    //Start//11-02-2016//Abid Ali//EMR Bug#303 fixed
                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.HospitalizationHx_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.HospitalizationHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.HospitalizationHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                    //End//1-02-2016//Abid Ali//EMR Bug#303 fixed
                }
            }
            return getJSONofList(list);
        }
        public string GetHospitalizationHxStay(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSHospitalizationHxLookup> obHistory = new BLLClinical().LookupHospitalizationHxStay();
            DSHospitalizationHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.HospitalizationHx_Stay.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.HospitalizationHx_Stay.TableName].Select("1=1", ds.HospitalizationHx_Stay.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.HospitalizationHx_Stay.DescriptionColumn.ColumnName].ToString(), dr[ds.HospitalizationHx_Stay.StayIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /** End HospitalizationHx Lookup **/
        /** Author : Abid Ali **/
        /** Date : 08/01/2016 **/


        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for the lookup of Birth Delivery Methods.
        /// Date : 5 january 2016
        /// </summary>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public string getBirthHxDeliveryMethod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBirthHistory> obHistory = new BLLClinical().lookupBirthHxDeliveryMethods();
            DSBirthHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BirthHx_MaternalDelivery_DeliveryMethod.TableName] != null)
                {
                    //emr 433 fix by azhar shahzad on april 12, 2016
                    DataRow[] dRows = ds.Tables[ds.BirthHx_MaternalDelivery_DeliveryMethod.TableName].Select("1=1", ds.BirthHx_MaternalDelivery_DeliveryMethod.DeliveryMethodIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BirthHx_MaternalDelivery_DeliveryMethod.DescriptionColumn.ColumnName].ToString(), dr[ds.BirthHx_MaternalDelivery_DeliveryMethod.DeliveryMethodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for the lookup of Birth Delivery Presentation.
        /// Date : 5 january 2016
        /// </summary>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public string getBirthHxDeliveryPresentation(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBirthHistory> obHistory = new BLLClinical().lookupBirthHxDeliveryPresentation();
            DSBirthHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BirthHx_MaternalDelivery_DeliveryPresentation.TableName] != null)
                {
                    //emr 432 fix by azhar shahzad on april 12, 2016
                    DataRow[] dRows = ds.Tables[ds.BirthHx_MaternalDelivery_DeliveryPresentation.TableName].Select("1=1", ds.BirthHx_MaternalDelivery_DeliveryPresentation.DeliveryPresentationIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BirthHx_MaternalDelivery_DeliveryPresentation.DescriptionColumn.ColumnName].ToString(), dr[ds.BirthHx_MaternalDelivery_DeliveryPresentation.DeliveryPresentationIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for the lookup of Birth Maternal history.
        /// Date : 5 january 2016
        /// </summary>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public string getBirthHxMaternalHistory(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBirthHistory> obHistory = new BLLClinical().lookupBirthHxMaternalHistory();
            DSBirthHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BirthHx_MaternalDelivery_MaternalHistory.TableName] != null)
                {
                    //emr 431 fix by azhar shahzad on april 12, 2016
                    DataRow[] dRows = ds.Tables[ds.BirthHx_MaternalDelivery_MaternalHistory.TableName].Select("1=1", ds.BirthHx_MaternalDelivery_MaternalHistory.MaternalHistoryIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BirthHx_MaternalDelivery_MaternalHistory.DescriptionColumn.ColumnName].ToString(), dr[ds.BirthHx_MaternalDelivery_MaternalHistory.MaternalHistoryIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /* End 05/01/2016 By K.R Lookup for Birth History */

        /*This Lookup is used For new born tab in Birth History,
         Author: Muhammad Azhar Shahzad
         Date: January 05, 2016*/
        public string getBirthHxNewbornPatientBloodType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBirthHistory> obHistory = new BLLClinical().birthHxNewbornPatientBloodTypeLookup();
            DSBirthHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BirthHx_Newborn_PatientBloodType.TableName] != null)
                {
                    //emr 431 fix by azhar shahzad on april 12, 2016
                    DataRow[] dRows = ds.Tables[ds.BirthHx_Newborn_PatientBloodType.TableName].Select("1=1", ds.BirthHx_Newborn_PatientBloodType.PatientBloodTypeIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BirthHx_Newborn_PatientBloodType.DescriptionColumn.ColumnName].ToString(), dr[ds.BirthHx_Newborn_PatientBloodType.PatientBloodTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /*This Lookup is used For Newborn Problems At Birth,
         Author: ZeeshanAK
         Date: January 05, 2016*/
        public string getBirthHxNewbornProblemsAtBirth(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBirthHistory> obHistory = new BLLClinical().lookupBirthHxNewbornProblemsAtBirth();
            DSBirthHistory ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BirthHx_Newborn_ProblemsAtBirth.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.BirthHx_Newborn_ProblemsAtBirth.TableName].Select("1=1", ds.BirthHx_Newborn_ProblemsAtBirth.ProblemsAtBirthIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BirthHx_Newborn_ProblemsAtBirth.DescriptionColumn.ColumnName].ToString(), dr[ds.BirthHx_Newborn_ProblemsAtBirth.ProblemsAtBirthIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /* End 20/01/2016 Muhammad Irfan functions for FamilyHx Lookups */
        public string GetFamilyHxFamilyMember(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFamilyHxLookup> obHistory = new BLLClinical().LookupFamilyHx_FamilyMember();
            DSFamilyHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FamilyHx_FamilyMember.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.FamilyHx_FamilyMember.TableName].Select("1=1", ds.FamilyHx_FamilyMember.DescriptionColumn.ColumnName);
                    //Start//11-02-2016//Ahmad Raza//EMR Bug#291 fixed
                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.FamilyHx_FamilyMember.FamilyMemberIdColumn.ColumnName]))
                    //End//11-02-2016//Ahmad Raza//EMR Bug#291 fixed
                    {
                        list.Add(new NameValuePair(dr[ds.FamilyHx_FamilyMember.DescriptionColumn.ColumnName].ToString(), dr[ds.FamilyHx_FamilyMember.FamilyMemberIdColumn.ColumnName].ToString(), dr[ds.FamilyHx_FamilyMember.SNOMEDIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetFamilyHxHealthStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFamilyHxLookup> obHistory = new BLLClinical().LookupFamilyHx_HealthStatus();
            DSFamilyHxLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FamilyHx_FamilyMember.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.FamilyHx_HealthStatus.TableName].Select("1=1", ds.FamilyHx_HealthStatus.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.FamilyHx_HealthStatus.DescriptionColumn.ColumnName].ToString(), dr[ds.FamilyHx_HealthStatus.HealthStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /* End 20/01/2016 Muhammad Irfan functions for FamilyHx Lookups */

        /* Lookups start by Khaleel Ur Rehman on 27 January 2016 for Review Of System.*/
        /// <summary>
        /// Author : Khaleel ur Rehman.
        /// Purpose : lookupROSCharacteristicsDetailAggravedBy.
        /// Date : 27 January 2016.
        /// </summary>
        /// <returns></returns>
        public string getROSCharacteristicsDetailAggravedBy(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalReviewofSystem> obj = new BLLClinical().lookupROSCharacteristicsDetailAggravedBy();
            DSClinicalReviewofSystem ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ROSCharacteristicsDetailAggravedBy.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ROSCharacteristicsDetailAggravedBy.TableName].Select("1=1", ds.ROSCharacteristicsDetailAggravedBy.ROSCharacteristicsDetailAggravedByIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ROSCharacteristicsDetailAggravedBy.ShortNameColumn.ColumnName].ToString(), dr[ds.ROSCharacteristicsDetailAggravedBy.ROSCharacteristicsDetailAggravedByIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Author : Khaleel ur Rehman.
        /// Purpose : function for lookup ROSCharacteristicsDetailCharacterCSZ.
        /// Date : 27 January 2016.
        /// </summary>
        /// <returns></returns>
        public string getROSCharacteristicsDetailCharacterCSZ(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalReviewofSystem> obj = new BLLClinical().lookupROSCharacteristicsDetailCharacterCSZ();
            DSClinicalReviewofSystem ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ROSCharacteristicsDetailCharacterCSZ.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ROSCharacteristicsDetailCharacterCSZ.TableName].Select("1=1", ds.ROSCharacteristicsDetailCharacterCSZ.ROSCharacteristicsDetailCharacterCSZIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ROSCharacteristicsDetailCharacterCSZ.ShortNameColumn.ColumnName].ToString(), dr[ds.ROSCharacteristicsDetailCharacterCSZ.ROSCharacteristicsDetailCharacterCSZIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /// <summary>
        /// Author : Khaleel ur Rehman.
        /// Purpose : function for lookup ROSCharacteristicsDetailAggravedBy.
        /// Date : 27 January 2016.
        /// </summary>
        /// <returns></returns>
        public string getROSCharacteristicsDetailContext(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalReviewofSystem> obj = new BLLClinical().lookupROSCharacteristicsDetailContext();
            DSClinicalReviewofSystem ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ROSCharacteristicsDetailContext.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ROSCharacteristicsDetailContext.TableName].Select("1=1", ds.ROSCharacteristicsDetailContext.ROSCharacteristicsDetailContextIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ROSCharacteristicsDetailContext.ShortNameColumn.ColumnName].ToString(), dr[ds.ROSCharacteristicsDetailContext.ROSCharacteristicsDetailContextIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Author : Khaleel ur Rehman.
        /// Purpose : function for lookup ROSCharacteristicsDetailFrequency.
        /// Date : 27 January 2016.
        /// </summary>
        /// <returns></returns>
        public string getROSCharacteristicsDetailFrequency(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalReviewofSystem> obj = new BLLClinical().lookupROSCharacteristicsDetailFrequency();
            DSClinicalReviewofSystem ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ROSCharacteristicsDetailFrequency.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ROSCharacteristicsDetailFrequency.TableName].Select("1=1", ds.ROSCharacteristicsDetailFrequency.ROSCharacteristicsDetailFrequencyIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ROSCharacteristicsDetailFrequency.ShortNameColumn.ColumnName].ToString(), dr[ds.ROSCharacteristicsDetailFrequency.ROSCharacteristicsDetailFrequencyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Author : Khaleel ur Rehman.
        /// Purpose : function for lookup ROSCharacteristicsDetailRadiation.
        /// Date : 27 January 2016.
        /// </summary>
        /// <returns></returns>
        public string getROSCharacteristicsDetailRadiation(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalReviewofSystem> obj = new BLLClinical().lookupROSCharacteristicsDetailRadiation();
            DSClinicalReviewofSystem ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ROSCharacteristicsDetailRadiation.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ROSCharacteristicsDetailRadiation.TableName].Select("1=1", ds.ROSCharacteristicsDetailRadiation.ROSCharacteristicsDetailRadiationIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ROSCharacteristicsDetailRadiation.ShortNameColumn.ColumnName].ToString(), dr[ds.ROSCharacteristicsDetailRadiation.ROSCharacteristicsDetailRadiationIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Author : Khaleel ur Rehman.
        /// Purpose : function for lookup ROSCharacteristicsDetailCourse.
        /// Date : 27 January 2016.
        /// </summary>
        /// <returns></returns>
        public string getROSCharacteristicsDetailCourse(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalReviewofSystem> obj = new BLLClinical().lookupROSCharacteristicsDetailCourse();
            DSClinicalReviewofSystem ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ROSCharacteristicsDetailCourse.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ROSCharacteristicsDetailCourse.TableName].Select("1=1", ds.ROSCharacteristicsDetailCourse.ROSCharacteristicsDetailCourseIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ROSCharacteristicsDetailCourse.ShortNameColumn.ColumnName].ToString(), dr[ds.ROSCharacteristicsDetailCourse.ROSCharacteristicsDetailCourseIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Author : Khaleel ur Rehman.
        /// Purpose : function for lookup ROSCharacteristicsDetailSeverity.
        /// Date : 27 January 2016.
        /// </summary>
        /// <returns></returns>
        public string getROSCharacteristicsDetailSeverity(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalReviewofSystem> obj = new BLLClinical().lookupROSCharacteristicsDetailSeverity();
            DSClinicalReviewofSystem ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ROSCharacteristicsDetailSeverity.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ROSCharacteristicsDetailSeverity.TableName].Select("1=1", ds.ROSCharacteristicsDetailSeverity.ROSCharacteristicsDetailSeverityIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ROSCharacteristicsDetailSeverity.ShortNameColumn.ColumnName].ToString(), dr[ds.ROSCharacteristicsDetailSeverity.ROSCharacteristicsDetailSeverityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Author : Khaleel ur Rehman.
        /// Purpose : function for lookup ROSCharacteristicsDetailPattern.
        /// Date : 27 January 2016.
        /// </summary>
        /// <returns></returns>
        public string getROSCharacteristicsDetailPattern(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalReviewofSystem> obj = new BLLClinical().lookupROSCharacteristicsDetailPattern();
            DSClinicalReviewofSystem ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ROSCharacteristicsDetailPattern.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ROSCharacteristicsDetailPattern.TableName].Select("1=1", ds.ROSCharacteristicsDetailPattern.ROSCharacteristicsDetailPatternIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ROSCharacteristicsDetailPattern.ShortNameColumn.ColumnName].ToString(), dr[ds.ROSCharacteristicsDetailPattern.ROSCharacteristicsDetailPatternIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Author : Khaleel ur Rehman.
        /// Purpose : function for lookup ROSCharacteristicsDetailDuration.
        /// Date : 27 January 2016.
        /// </summary>
        /// <returns></returns>
        public string getROSCharacteristicsDetailDuration(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalReviewofSystem> obj = new BLLClinical().lookupROSCharacteristicsDetailDuration();
            DSClinicalReviewofSystem ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ROSCharacteristicsDetailDuration.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ROSCharacteristicsDetailDuration.TableName].Select("1=1", ds.ROSCharacteristicsDetailDuration.ROSCharacteristicsDetailDurationIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ROSCharacteristicsDetailDuration.ShortNameColumn.ColumnName].ToString(), dr[ds.ROSCharacteristicsDetailDuration.ROSCharacteristicsDetailDurationIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Author : Khaleel ur Rehman.
        /// Purpose : function for lookup ROSCharacteristicsDetailStatus.
        /// Date : 27 January 2016.
        /// </summary>
        /// <returns></returns>
        public string getROSCharacteristicsDetailStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalReviewofSystem> obj = new BLLClinical().lookupROSCharacteristicsDetailStatus();
            DSClinicalReviewofSystem ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ROSCharacteristicsDetailStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ROSCharacteristicsDetailStatus.TableName].Select("1=1", ds.ROSCharacteristicsDetailStatus.ROSCharacteristicsDetailStatusIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ROSCharacteristicsDetailStatus.ShortNameColumn.ColumnName].ToString(), dr[ds.ROSCharacteristicsDetailStatus.ROSCharacteristicsDetailStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Author : Khaleel ur Rehman.
        /// Purpose : function for lookup ROSCharacteristicsDetailStatus.
        /// Date : 27 January 2016.
        /// </summary>
        /// <returns></returns>
        public string getROSCharacteristicsDetailRelievedBy(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalReviewofSystem> obj = new BLLClinical().lookupROSCharacteristicsDetailRelievedBy();
            DSClinicalReviewofSystem ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ROSCharacteristicsDetailRelievedBy.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ROSCharacteristicsDetailRelievedBy.TableName].Select("1=1", ds.ROSCharacteristicsDetailRelievedBy.ROSCharacteristicsDetailRelievedByIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ROSCharacteristicsDetailRelievedBy.ShortNameColumn.ColumnName].ToString(), dr[ds.ROSCharacteristicsDetailRelievedBy.ROSCharacteristicsDetailRelievedByIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /* Lookups End by Khaleel Ur Rehman on 27 January 2016 for Review Of System.*/

        public string GetCollectedAt(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRadiologyOrderLookup> obHistory = new BLLClinical().lookupCollectedAt();
            DSRadiologyOrderLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.CollectedAt.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.CollectedAt.TableName].Select("1=1", ds.CollectedAt.CollectedAtIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.CollectedAt.DescriptionColumn.ColumnName].ToString(), dr[ds.CollectedAt.CollectedAtIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetUrgency(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRadiologyOrderLookup> obHistory = new BLLClinical().lookupUrgency();
            DSRadiologyOrderLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Urgency.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Urgency.TableName].Select("1=1", ds.Urgency.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Urgency.DescriptionColumn.ColumnName].ToString(), dr[ds.Urgency.UrgencyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetSpecimen(string IsActive, string TestCode, string SpecimenType, string DropDownId, long LabId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRadiologyOrderLookup> obHistory = new BLLClinical().lookupSpecimen(TestCode, SpecimenType, LabId);
            DSRadiologyOrderLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Specimen.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Specimen.TableName].Select("1=1", ds.Specimen.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Specimen.DescriptionColumn.ColumnName].ToString(), dr[ds.Specimen.SpecimenIdColumn.ColumnName].ToString()));
                    }
                }
            }
            // return getJSONofList(list);
            var response = new
            {
                data = getJSONofList(list),
                DropDownId = DropDownId,
            };
            return (JsonConvert.SerializeObject(response));
        }

        public string GetSpecimenSource(string IsActive, long SpecimenId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRadiologyOrderLookup> obHistory = new BLLClinical().lookupSpecimenSource(SpecimenId);
            DSRadiologyOrderLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SpecimenSource.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SpecimenSource.TableName].Select("1=1", ds.SpecimenSource.IdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SpecimenSource.ConceptIdColumn.ColumnName].ToString() + "-" + dr[ds.SpecimenSource.PreferredTermColumn.ColumnName].ToString(), dr[ds.SpecimenSource.IdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        public string GetOrganism(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRadiologyOrderLookup> obHistory = new BLLClinical().lookupOrganism();
            DSRadiologyOrderLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Organism.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Organism.TableName].Select("1=1", ds.Organism.IdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Organism.AROPathogenCategoryColumn.ColumnName].ToString(), dr[ds.Organism.IdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        public string GetAntimicrobialBySpecimentAndOrganism(string IsActive, long SpecimenId, long OrganismId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRadiologyOrderLookup> obHistory = new BLLClinical().lookupAntimicrobialBySpecimentAndOrganism(SpecimenId, OrganismId);
            DSRadiologyOrderLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Antimicrobial.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Antimicrobial.TableName].Select("1=1", ds.Antimicrobial.IdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Antimicrobial.CodeColumn.ColumnName].ToString() + "-" + dr[ds.Antimicrobial.DescriptionColumn.ColumnName].ToString(), dr[ds.Antimicrobial.IdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }


        //public string GetSpecimen(string IsActive)
        //{
        //    HashSet<NameValuePair> list = new HashSet<NameValuePair>();
        //    BLObject<DSRadiologyOrderLookup> obHistory = new BLLClinical().lookupSpecimen();
        //    DSRadiologyOrderLookup ds = obHistory.Data;
        //    list.Add(new NameValuePair("- Select -", ""));
        //    if (ds != null)
        //    {
        //        if (ds.Tables[ds.Specimen.TableName] != null)
        //        {
        //            DataRow[] dRows = ds.Tables[ds.Specimen.TableName].Select("1=1", ds.Specimen.DescriptionColumn.ColumnName);

        //            foreach (DataRow dr in dRows)
        //            {
        //                list.Add(new NameValuePair(dr[ds.Specimen.DescriptionColumn.ColumnName].ToString(), dr[ds.Specimen.SpecimenIdColumn.ColumnName].ToString()));
        //            }
        //        }
        //    }

        //    return getJSONofList(list);
        //}

        public string GetVolume(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRadiologyOrderLookup> obHistory = new BLLClinical().lookupVolume();
            DSRadiologyOrderLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Specimen.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Volume.TableName].Select("1=1", ds.Volume.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Volume.DescriptionColumn.ColumnName].ToString(), dr[ds.Volume.VolumeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetNoteComponents(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLClinical().LookupNoteComponents();

            var ds = model.Data;
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.NoteComponentName, item.NoteComponentId));
                    }
                }

            }
            return getJSONofList(list);
        }
        public string GetNoteSections(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLClinical().LookupNoteSections();

            var ds = model.Data;
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.SectionName, MDVUtility.ToStr(item.NoteSectionsLookupId), item.SectionMarkup));
                    }
                }

            }
            return getJSONofList(list);
        }

        // Load bulk sign exception lookup
        public string GetBulkSignExceptionLookup(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            List<LookupModel> objListLookupModel = new BLLClinical().LookupByType("BulkSign");
            list.Add(new NameValuePair("- Select -", ""));
            if (objListLookupModel.Any())
            {
                foreach (LookupModel obj in objListLookupModel)
                {
                    list.Add(new NameValuePair(obj.Name, obj.Id.ToString()));
                }
            }
            return getJSONofList(list);
        }


        #endregion

        #region Charge Batch Lookup
        public string GetChargeBatchStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBatchCharge> objPractice = new BLLBilling().LookupChargeBatchStatus();
            DSBatchCharge ds = objPractice.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.ChargeBatchStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ChargeBatchStatus.TableName].Select("1=1", ds.ChargeBatchStatus.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ChargeBatchStatus.ShortNameColumn.ColumnName].ToString(), dr[ds.ChargeBatchStatus.ChargeBtchStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetBatchChargeAction(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBatchCharge> objBatchChargeAction = new BLLBilling().LookupBatchChargeAction();
            DSBatchCharge ds = objBatchChargeAction.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.BatchChargeAction.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.BatchChargeAction.TableName].Select("1=1", ds.BatchChargeAction.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BatchChargeAction.ShortNameColumn.ColumnName].ToString(), dr[ds.BatchChargeAction.ActionIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetBatchChargeReason(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBatchCharge> objBatchChargeReason = new BLLBilling().LookupBatchChargeReason();
            DSBatchCharge ds = objBatchChargeReason.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.BatchChargeReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.BatchChargeReason.TableName].Select("1=1", ds.BatchChargeReason.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.BatchChargeReason.ShortNameColumn.ColumnName].ToString(), dr[ds.BatchChargeReason.ReasonIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region ERA
        public string GetAdjustmentReasonCode(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSERALookup> objquestions = new BLLERA().LookupAdjustmentReasonCode();
            DSERALookup ds = objquestions.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.AdjustmentReasonCode.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.AdjustmentReasonCode.TableName].Select("1=1", ds.AdjustmentReasonCode.AdjustmentReasonNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.AdjustmentReasonCode.AdjustmentReasonNameColumn.ColumnName].ToString() + " " + dr[ds.AdjustmentReasonCode.DescriptionColumn.ColumnName].ToString(), dr[ds.AdjustmentReasonCode.AdjustmentIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }

        public string GetAdjustmentGroupCode(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSERALookup> objquestions = new BLLERA().LookupAdjustmentGroupCode();
            DSERALookup ds = objquestions.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.AdjustmentGroupCode.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.AdjustmentGroupCode.TableName].Select("1=1", ds.AdjustmentGroupCode.AdjustmentGroupNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.AdjustmentGroupCode.AdjustmentGroupNameColumn.ColumnName].ToString() + " " + dr[ds.AdjustmentGroupCode.DescriptionColumn.ColumnName].ToString(), dr[ds.AdjustmentGroupCode.AdjGroupIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        public string GetAdjustmentGroupCodeReport(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSERALookup> objquestions = new BLLERA().LookupAdjustmentGroupCode();
            DSERALookup ds = objquestions.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.AdjustmentGroupCode.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.AdjustmentGroupCode.TableName].Select("1=1", ds.AdjustmentGroupCode.AdjustmentGroupNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.AdjustmentGroupCode.AdjustmentGroupNameColumn.ColumnName].ToString(), dr[ds.AdjustmentGroupCode.AdjustmentGroupNameColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        public string GetAdjustmentReasonCodeReport(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSERALookup> objquestions = new BLLERA().LookupAdjustmentReasonCode();
            DSERALookup ds = objquestions.Data;

            if (ds != null)
            {
                if (ds.Tables[ds.AdjustmentReasonCode.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.AdjustmentReasonCode.TableName].Select("1=1", ds.AdjustmentReasonCode.AdjustmentReasonNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.AdjustmentReasonCode.AdjustmentReasonNameColumn.ColumnName].ToString(), dr[ds.AdjustmentReasonCode.AdjustmentReasonNameColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }

        public string GetERAAction(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSERA> objquestions = new BLLERA().LoadERAAction(0);
            DSERA ds = objquestions.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ERAAction.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ERAAction.TableName].Select("1=1", ds.ERAAction.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ERAAction.ShortNameColumn.ColumnName].ToString(), dr[ds.ERAAction.ERAActionIdColumn.ColumnName].ToString() + "_" + dr[ds.ERAAction.LedgerAccountTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }


        public string GetERAStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSERALookup> objEraStatus = new BLLERA().LookupERAStatus();
            DSERALookup ds = objEraStatus.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.ERAStatus.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ERAStatus.TableName].Select("1=1", ds.ERAStatus.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ERAStatus.ShortNameColumn.ColumnName].ToString(), dr[ds.ERAStatus.ERAStatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetERAPayee(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSERALookup> objEraPayee = new BLLERA().LookupERAPayee();
            DSERALookup ds = objEraPayee.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.ERAPayeeLookUp.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ERAPayeeLookUp.TableName].Select("1=1", ds.ERAPayeeLookUp.PayeeNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ERAPayeeLookUp.PayeeNameColumn.ColumnName].ToString(), dr[ds.ERAPayeeLookUp.PayeeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        #endregion

        #region Miscellaneous

        public string getOccupationStatus(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxOccupationStatus();
            DSSocialHistoryLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_OccupationHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_OccupationHx_Status.TableName].Select("1=1", ds.SocialHx_MiscHx_OccupationHx_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_OccupationHx_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_OccupationHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_OccupationHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string getHousingHxStatus(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxHousingHxStatus();
            DSSocialHistoryLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_HousingHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_HousingHx_Status.TableName].Select("1=1", ds.SocialHx_MiscHx_HousingHx_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_HousingHx_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_HousingHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_HousingHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string getCaffeineIntakeHxStatus(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxCaffeineIntakeHxStatus();
            DSSocialHistoryLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.TableName].Select("1=1", ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.StatusIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_CaffeineIntakeHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string getTravelHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxTravelHxStatus();
            DSSocialHistoryLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_TravelHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_TravelHx_Status.TableName].Select("1=1", ds.SocialHx_MiscHx_TravelHx_Status.NameColumn.ColumnName);
                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_TravelHx_Status.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_TravelHx_Status.NameColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_TravelHx_Status.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string getCaffeineIntakHxFrequency(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxCaffeineIntakeHxFrequency();
            DSSocialHistoryLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.TableName].Select("1=1", ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.FrequencyIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_CaffeineIntakeHx_Frequency.FrequencyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string getExercisesHxDiet(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxExercisesHxDiet();
            DSSocialHistoryLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_ExercisesHx_Diet.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_ExercisesHx_Diet.TableName].Select("1=1", ds.SocialHx_MiscHx_ExercisesHx_Diet.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_ExercisesHx_Diet.DietIdColumn.ColumnName]))
                    {

                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_ExercisesHx_Diet.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_ExercisesHx_Diet.DietIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string getExercisesHxType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxExercisesHxType();
            DSSocialHistoryLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_ExercisesHx_Type.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_ExercisesHx_Type.TableName].Select("1=1", ds.SocialHx_MiscHx_ExercisesHx_Type.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_ExercisesHx_Type.TypeIdColumn.ColumnName]))
                    {

                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_ExercisesHx_Type.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_ExercisesHx_Type.TypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string getExercisesHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxExercisesHxStatus();
            DSSocialHistoryLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_ExercisesHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_ExercisesHx_Status.TableName].Select("1=1", ds.SocialHx_MiscHx_ExercisesHx_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_ExercisesHx_Status.StatusIdColumn.ColumnName]))
                    {

                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_ExercisesHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_ExercisesHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        public string getSleepHxStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSSocialHistoryLookup> obHistory = new BLLClinical().lookupSocialHxMiscHxSleepHxStatus();
            DSSocialHistoryLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SocialHx_MiscHx_SleepHx_Status.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SocialHx_MiscHx_SleepHx_Status.TableName].Select("1=1", ds.SocialHx_MiscHx_SleepHx_Status.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SocialHx_MiscHx_SleepHx_Status.StatusIdColumn.ColumnName]))
                    {

                        list.Add(new NameValuePair(dr[ds.SocialHx_MiscHx_SleepHx_Status.DescriptionColumn.ColumnName].ToString(), dr[ds.SocialHx_MiscHx_SleepHx_Status.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetLookRoles(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            AuditReportHelper helperAuditReport = new AuditReportHelper();
            List<LookupRoles> LstLookupRole = new List<LookupRoles>();
            LstLookupRole = helperAuditReport.GetLookupRoles();
            list.Add(new NameValuePair("- Select -", ""));
            if (LstLookupRole.Count() > 0)
            {
                foreach (var dr in LstLookupRole)
                {
                    list.Add(new NameValuePair(dr.Name, dr.LookupId));
                }

            }
            return getJSONofList(list);
        }
        #endregion

        /*
         Farooq Ahmad 9th Feb 2016 Physical Exam Lookups
         Following Functions are used in Physcial Exam
        */
        #region Physical Exam
        //Author: Muhammad Arshad
        //Date: 02-26-2016
        //This function will handle fill of PhysicalExam System
        public string GetPhysicalExamSystem(string IsActive, Int64 templateId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            DSPhysicalExamLookup ds = null;
            BLObject<DSPhysicalExamLookup> obj = new BLLClinical().lookupPhysicalExamSystem(templateId);
            ds = obj.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamSystem.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamSystem.TableName].Select("1=1", ds.PhysicalExamSystem.SystemIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.PhysicalExamSystem.DescriptionColumn.ColumnName].ToString(), dr[ds.PhysicalExamSystem.SystemIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        //Author: Muhammad Arshad
        //Date: 02-26-2016
        //This function will handle fill of PhysicalExam System
        public string GetPhysicalExamDataTemplateSystem(string IsActive, long templateId, long dataTemplateId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            DSPhysicalExamDataTemplate ds = null;
            BLObject<DSPhysicalExamDataTemplate> obj = new BLLClinical().lookupPhysicalExamDataTemplateSystem(dataTemplateId, templateId);
            ds = obj.Data;
            if (ds != null)
            {
                if (dataTemplateId > 0)
                {
                    if (ds.Tables["dtFoundSystem"] != null)
                    {
                        DataRow[] dRows = ds.Tables["dtFoundSystem"].Select("1=1", "SystemId");

                        foreach (DataRow dr in dRows)
                        {
                            list.Add(new NameValuePair(dr["SystemName"].ToString(), dr["SystemId"].ToString(), dr["IsFound"].ToString(), dr["IsNormal"].ToString()));
                        }
                    }
                }
                else
                {
                    DSPhysicalExamTemplate dsTemplat = new DSPhysicalExamTemplate();
                    if (ds.Tables[dsTemplat.PhysExamTemplateSys.TableName] != null)
                    {
                        DataRow[] dRows = ds.Tables[dsTemplat.PhysExamTemplateSys.TableName].Select("1=1", dsTemplat.PhysExamTemplateSys.TemplateSysIdColumn.ColumnName);

                        foreach (DataRow dr in dRows)
                        {

                            list.Add(new NameValuePair(dr[dsTemplat.PhysExamTemplateSys.SystemNameColumn.ColumnName].ToString(), dr[dsTemplat.PhysExamTemplateSys.SystemIdColumn.ColumnName].ToString(), "0"));
                        }
                    }
                }

            }
            return getJSONofList(list);
        }
        public string GetPhysicalExamDataTemplateSectionBySystemId(string IsActive, long SystemId, long templateId, long dataTemplateId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            DSPhysicalExamDataTemplate ds = null;
            BLObject<DSPhysicalExamDataTemplate> obj = new BLLClinical().LookupPhysicalExamDataTemplateSystemSection(dataTemplateId, templateId, SystemId);
            ds = obj.Data;
            if (ds != null)
            {
                if (dataTemplateId > 0)
                {
                    if (ds.Tables["dtFoundSections"] != null)
                    {
                        DataRow[] dRows = ds.Tables["dtFoundSections"].Select("1=1", ds.PhysExamDataTemplateSection.SectionIdColumn.ColumnName);

                        foreach (DataRow dr in dRows)
                        {

                            list.Add(new NameValuePair(dr["SectionName"].ToString(), dr["SectionId"].ToString(), dr["IsFound"].ToString(), dr["Comments"].ToString(), dr["IsNormal"].ToString()));
                        }
                    }
                }
                else
                {
                    DSPhysicalExamTemplate dsTemplat = new DSPhysicalExamTemplate();
                    if (ds.Tables[dsTemplat.PhysExamTemplateSection.TableName] != null)
                    {
                        DataRow[] dRows = ds.Tables[dsTemplat.PhysExamTemplateSection.TableName].Select("1=1", dsTemplat.PhysExamTemplateSection.TemplateSectionIdColumn.ColumnName);

                        foreach (DataRow dr in dRows)
                        {
                            list.Add(new NameValuePair(dr[dsTemplat.PhysExamTemplateSection.SectionNameColumn.ColumnName].ToString(), dr[dsTemplat.PhysExamTemplateSection.SectionIdColumn.ColumnName].ToString(), "0", "0"));
                        }
                    }
                }

            }
            return getJSONofList(list);
        }
        public string GetPhysicalExamDataTemplateCharBySectionId(string IsActive, long SectionId, long templateId, long dataTemplateId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            DSPhysicalExamDataTemplate ds = null;
            BLObject<DSPhysicalExamDataTemplate> obj = new BLLClinical().LookupPhysicalExamDataTemplateSystemChar(dataTemplateId, templateId, SectionId);
            ds = obj.Data;
            if (ds != null)
            {
                if (dataTemplateId > 0)
                {
                    if (ds.Tables["dtFoundChars"] != null)
                    {
                        DataRow[] dRows = ds.Tables["dtFoundChars"].Select("1=1", ds.PhysExamDataTemplateChar.CharIdColumn.ColumnName);

                        foreach (DataRow dr in dRows)
                        {

                            list.Add(new NameValuePair(dr["CharName"].ToString(), dr["CharId"].ToString(), dr["IsFound"].ToString(), dr["Comments"].ToString(), dr["IsPositive"].ToString() + "," + dr["IsNegative"].ToString(), dr["SectionCharacteristicDetailModel"].ToString()));
                        }
                    }
                }
                else
                {
                    DSPhysicalExamTemplate dsTemplat = new DSPhysicalExamTemplate();
                    if (ds.Tables[dsTemplat.PhysExamTemplateChar.TableName] != null)
                    {
                        DataRow[] dRows = ds.Tables[dsTemplat.PhysExamTemplateChar.TableName].Select("1=1", dsTemplat.PhysExamTemplateChar.TemplateCharIdColumn.ColumnName);

                        foreach (DataRow dr in dRows)
                        {
                            list.Add(new NameValuePair(dr[dsTemplat.PhysExamTemplateChar.CharNameColumn.ColumnName].ToString(), dr[dsTemplat.PhysExamTemplateChar.CharIdColumn.ColumnName].ToString(), "0", dr[dsTemplat.PhysExamTemplateChar.bSubCharacteristicExistColumn.ColumnName].ToString(), "0,0", ""));
                        }
                    }
                }

            }
            return getJSONofList(list);
        }

        public string GetPhysicalExamDataTemplateSubCharByCharId(string IsActive, long CharId, long templateId, long dataTemplateId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            DSPhysicalExamDataTemplate ds = null;
            BLObject<DSPhysicalExamDataTemplate> obj = new BLLClinical().LookupPhysicalExamDataTemplateSystemSubChar(dataTemplateId, templateId, CharId);
            ds = obj.Data;
            if (ds != null)
            {
                if (dataTemplateId > 0)
                {
                    if (ds.Tables["dtFoundSubChars"] != null)
                    {
                        DataRow[] dRows = ds.Tables["dtFoundSubChars"].Select("1=1", ds.PhysExamDataTemplateSubChar.SubCharIdColumn.ColumnName);

                        foreach (DataRow dr in dRows)
                        {

                            list.Add(new NameValuePair(dr["SubCharName"].ToString(), dr["SubCharId"].ToString(), dr["IsFound"].ToString(), dr["Comments"].ToString(), dr["IsPositive"].ToString() + "," + dr["IsNegative"].ToString(), dr["SubCharacteristicDetailModel"].ToString()));
                        }
                    }
                }
                else
                {
                    DSPhysicalExamTemplate dsTemplat = new DSPhysicalExamTemplate();
                    if (ds.Tables[dsTemplat.PhysExamTemplateSubChar.TableName] != null)
                    {
                        DataRow[] dRows = ds.Tables[dsTemplat.PhysExamTemplateSubChar.TableName].Select("1=1", dsTemplat.PhysExamTemplateSubChar.TemplateSubCharIdColumn.ColumnName);

                        foreach (DataRow dr in dRows)
                        {
                            list.Add(new NameValuePair(dr[dsTemplat.PhysExamTemplateSubChar.SubCharNameColumn.ColumnName].ToString(), dr[dsTemplat.PhysExamTemplateSubChar.SubCharIdColumn.ColumnName].ToString(), "0", "", "0,0", ""));
                        }
                    }
                }

            }
            return getJSONofList(list);
        }
        //Author: Farooq Ahmad
        //Date: 02-09-2016
        //This function will handle fill of PhysicalExam Sections By SystemId
        public string GetPhysicalExamSectionBySystemId(string IsActive, long SystemId, long templateId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            DSPhysicalExamLookup ds = null;
            BLObject<DSPhysicalExamLookup> obj = new BLLClinical().lookupPhysicalExamSystemSection(SystemId, templateId);
            ds = obj.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamSystemSection.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamSystemSection.TableName].Select("1=1", ds.PhysicalExamSystemSection.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.PhysicalExamSystemSection.DescriptionColumn.ColumnName].ToString(), dr[ds.PhysicalExamSystemSection.SectionIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        //Author: Farooq Ahmad
        //Date: 02-10-2016
        //This function will handle fill of PhysicalExam CharcteristicBySectionId
        public string GetPhysicalExamCharcteristicBySectionId(string IsActive, long SectionId, long templateId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            DSPhysicalExamLookup ds = null;
            BLObject<DSPhysicalExamLookup> obj = new BLLClinical().lookupPhysicalExamCharcteristic(SectionId, templateId);
            ds = obj.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamSystemSectionCharacteristic.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamSystemSectionCharacteristic.TableName].Select("1=1", ds.PhysicalExamSystemSectionCharacteristic.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.PhysicalExamSystemSectionCharacteristic.DescriptionColumn.ColumnName].ToString(), dr[ds.PhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName].ToString(), "", dr[ds.PhysicalExamSystemSectionCharacteristic.bSubCharacteristicExistColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Module Name: GetPhysicalExamSubCharcteristicByCharacteristicId
        /// Author: Humaira Yousaf
        /// Created Date: 11-02-2016
        /// Description: Gets Physical Exam SubCharcteristic By CharacteristicId
        /// </summary>
        /// <param name="IsActive" type="bool">IsActive</param>
        /// <param name="CharacteristicId" type="long">CharacteristicId</param>
        public string GetPhysicalExamSubCharcteristicByCharacteristicId(string IsActive, long CharacteristicId, long templateId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            DSPhysicalExamLookup ds = null;
            BLObject<DSPhysicalExamLookup> obj = new BLLClinical().lookupPhysicalExamSubCharcteristic(CharacteristicId, templateId);
            ds = obj.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName].Select("1=1", ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.DescriptionColumn.ColumnName].ToString(), dr[ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        //Start//15-02-2016//Ahmad Raza//Methods for Physical Exam Lookups
        public string getPhysicalExamCharacter(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPhysicalExamLookup> objCharacter = new BLLClinical().lookupCharacter();
            DSPhysicalExamLookup ds = objCharacter.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamCharacter.TableName] != null)
                {
                    //Begin 26-02-2016 Edit By Humaira Yousaf Bug# 382
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamCharacter.TableName].Select("1=1", ds.PhysicalExamCharacter.CharacterIdColumn.ColumnName);
                    //End 26-02-2016 Edit By Humaira Yousaf Bug# 382

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PhysicalExamCharacter.ShortNameColumn.ColumnName].ToString(), dr[ds.PhysicalExamCharacter.CharacterIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string getPhysicalExamContext(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPhysicalExamLookup> objCharacter = new BLLClinical().lookupContext();
            DSPhysicalExamLookup ds = objCharacter.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamContext.TableName] != null)
                {
                    //Begin 26-02-2016 Edit By Humaira Yousaf Bug# 381
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamContext.TableName].Select("1=1", ds.PhysicalExamContext.ContextIdColumn.ColumnName);
                    //End 26-02-2016 Edit By Humaira Yousaf Bug# 381

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PhysicalExamContext.ShortNameColumn.ColumnName].ToString(), dr[ds.PhysicalExamContext.ContextIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string getPhysicalExamCourse(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPhysicalExamLookup> objCourse = new BLLClinical().lookupCourse();
            DSPhysicalExamLookup ds = objCourse.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamCourse.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamCourse.TableName].Select("1=1", ds.PhysicalExamCourse.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.PhysicalExamCourse.CourseIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.PhysicalExamCourse.ShortNameColumn.ColumnName].ToString(), dr[ds.PhysicalExamCourse.CourseIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string getPhysicalExamFrequency(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPhysicalExamLookup> objFrequency = new BLLClinical().lookupFrequency();
            DSPhysicalExamLookup ds = objFrequency.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamFrequency.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamFrequency.TableName].Select("1=1", ds.PhysicalExamFrequency.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.PhysicalExamFrequency.FrequencyIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.PhysicalExamFrequency.ShortNameColumn.ColumnName].ToString(), dr[ds.PhysicalExamFrequency.FrequencyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string getPhysicalExamRadiation(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPhysicalExamLookup> objRadiation = new BLLClinical().lookupRadiation();
            DSPhysicalExamLookup ds = objRadiation.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamRadiation.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamRadiation.TableName].Select("1=1", ds.PhysicalExamRadiation.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.PhysicalExamRadiation.RadiationIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.PhysicalExamRadiation.ShortNameColumn.ColumnName].ToString(), dr[ds.PhysicalExamRadiation.RadiationIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string getPhysicalExamRelievedBy(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPhysicalExamLookup> objRelievedBy = new BLLClinical().lookupRelievedBy();
            DSPhysicalExamLookup ds = objRelievedBy.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamRelievedby.TableName] != null)
                {
                    //Begin 26-02-2016 Edit By Humaira Yousaf Bug# 383
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamRelievedby.TableName].Select("1=1", ds.PhysicalExamRelievedby.RelievedbyIdColumn.ColumnName);
                    //End 26-02-2016 Edit By Humaira Yousaf Bug# 383

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PhysicalExamRelievedby.ShortNameColumn.ColumnName].ToString(), dr[ds.PhysicalExamRelievedby.RelievedbyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        //End//15-02-2016//Ahmad Raza//Methods for Physical Exam Lookups
        #endregion

        //Author: Abid Ali
        //Date: 08-30-2016
        //This function will load sections by systemId For template
        public string GetPhysicalExamSectionBySystemIdForTemplate(string IsActive, long SystemId, long templateId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            DSPhysicalExamLookup ds = null;
            BLObject<DSPhysicalExamLookup> obj = new BLLClinical().lookupPhysicalExamSystemSectionForTemplate(SystemId, templateId);
            ds = obj.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamSystemSection.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamSystemSection.TableName].Select("1=1", ds.PhysicalExamSystemSection.SectionIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.PhysicalExamSystemSection.DescriptionColumn.ColumnName].ToString(), dr[ds.PhysicalExamSystemSection.SectionIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetPhysicalExamCharBySectionIdForTemplate(string IsActive, long SectionId, long templateId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            DSPhysicalExamLookup ds = null;
            BLObject<DSPhysicalExamLookup> obj = new BLLClinical().lookupPhysicalExamCharForTemplate(SectionId, templateId);
            ds = obj.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamSystemSectionCharacteristic.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamSystemSectionCharacteristic.TableName].Select("1=1", ds.PhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.PhysicalExamSystemSectionCharacteristic.DescriptionColumn.ColumnName].ToString(), dr[ds.PhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName].ToString(), "", dr[ds.PhysicalExamSystemSectionCharacteristic.bSubCharacteristicExistColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetPhysicalExamSubCharByCharIdForTemplate(string IsActive, long CharacteristicId, long templateId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            DSPhysicalExamLookup ds = null;
            BLObject<DSPhysicalExamLookup> obj = new BLLClinical().lookupPhysicalExamSubCharForTemplate(CharacteristicId, templateId);
            ds = obj.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName].Select("1=1", ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.DescriptionColumn.ColumnName].ToString(), dr[ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        /*
         Abid Ali 20th july 2016 Clinical Billing info Lookups (eSuperBill)
         Following Functions are used in Clinical Billing info (eSuperBill)
        */
        #region eSuperBill
        //Lookups code goes here
        #endregion

        /*
         Muhammad Ahmad Imran 10th Feb 2016 Cheif Complaints Lookups
         Following Functions are used in Clinical Complaints
        */
        #region OrderSet Medication

        public string GetAction(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineRoute = new BLLOrderSet().GetMedicationOrdersetLookUp("Action");
            DSImmunization ds = objVaccineRoute.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineRefusalReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineRefusalReason.TableName].Select("1=1", ds.VaccineRefusalReason.DropDownColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.VaccineRefusalReason.DropDownColumn.ColumnName].ToString(), dr[ds.VaccineRefusalReason.LookUpIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetDose(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineRoute = new BLLOrderSet().GetMedicationOrdersetLookUp("Dose");
            DSImmunization ds = objVaccineRoute.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineRefusalReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineRefusalReason.TableName].Select("1=1", ds.VaccineRefusalReason.DropDownColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.VaccineRefusalReason.DropDownColumn.ColumnName].ToString(), dr[ds.VaccineRefusalReason.LookUpIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetDoseUnit(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineRoute = new BLLOrderSet().GetMedicationOrdersetLookUp("Dose Unit");
            DSImmunization ds = objVaccineRoute.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineRefusalReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineRefusalReason.TableName].Select("1=1", ds.VaccineRefusalReason.DropDownColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.VaccineRefusalReason.DropDownColumn.ColumnName].ToString(), dr[ds.VaccineRefusalReason.LookUpIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetRoute(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineRoute = new BLLOrderSet().GetMedicationOrdersetLookUp("Route");
            DSImmunization ds = objVaccineRoute.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineRefusalReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineRefusalReason.TableName].Select("1=1", ds.VaccineRefusalReason.DropDownColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.VaccineRefusalReason.DropDownColumn.ColumnName].ToString(), dr[ds.VaccineRefusalReason.LookUpIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetDoseTiming(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineRoute = new BLLOrderSet().GetMedicationOrdersetLookUp("Dose Timing");
            DSImmunization ds = objVaccineRoute.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineRefusalReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineRefusalReason.TableName].Select("1=1", ds.VaccineRefusalReason.DropDownColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.VaccineRefusalReason.DropDownColumn.ColumnName].ToString(), dr[ds.VaccineRefusalReason.LookUpIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetDoseOther(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineRoute = new BLLOrderSet().GetMedicationOrdersetLookUp("Dose Other");
            DSImmunization ds = objVaccineRoute.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineRefusalReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineRefusalReason.TableName].Select("1=1", ds.VaccineRefusalReason.DropDownColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.VaccineRefusalReason.DropDownColumn.ColumnName].ToString(), dr[ds.VaccineRefusalReason.LookUpIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetMedicationDuration(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineRoute = new BLLOrderSet().GetMedicationOrdersetLookUp("Duration");
            DSImmunization ds = objVaccineRoute.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineRefusalReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineRefusalReason.TableName].Select("1=1", ds.VaccineRefusalReason.DropDownColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.VaccineRefusalReason.DropDownColumn.ColumnName].ToString(), dr[ds.VaccineRefusalReason.LookUpIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetQuantityUnit(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineRoute = new BLLOrderSet().GetMedicationOrdersetLookUp("Quantity Unit");
            DSImmunization ds = objVaccineRoute.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineRefusalReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineRefusalReason.TableName].Select("1=1", ds.VaccineRefusalReason.DropDownColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.VaccineRefusalReason.DropDownColumn.ColumnName].ToString(), dr[ds.VaccineRefusalReason.LookUpIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region Cheif Complaints
        public string GetCase(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalComplaintLookup> obCase = new BLLClinical().GetCase();
            DSClinicalComplaintLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", "0"));
            if (ds != null)
            {
                if (ds.Tables[ds.Complaint_Case.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Complaint_Case.TableName].Select("1=1", ds.Complaint_Case.CaseIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Complaint_Case.DropDownColumn.ColumnName].ToString(), dr[ds.Complaint_Case.CaseIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetLocation(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalComplaintLookup> obCase = new BLLClinical().GetLocation();
            DSClinicalComplaintLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", "0"));
            if (ds != null)
            {
                if (ds.Tables[ds.Complaint_Location.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Complaint_Location.TableName].Select("1=1", ds.Complaint_Location.LocationIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Complaint_Location.DropDownColumn.ColumnName].ToString(), dr[ds.Complaint_Location.LocationIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetRadiation(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalComplaintLookup> obCase = new BLLClinical().GetRadiation();
            DSClinicalComplaintLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", "0"));
            if (ds != null)
            {
                if (ds.Tables[ds.Complaint_Radiation.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Complaint_Radiation.TableName].Select("1=1", ds.Complaint_Radiation.RadiationIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Complaint_Radiation.DropDownColumn.ColumnName].ToString(), dr[ds.Complaint_Radiation.RadiationIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetQuality(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalComplaintLookup> obCase = new BLLClinical().GetQuality();
            DSClinicalComplaintLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", "0"));
            if (ds != null)
            {
                if (ds.Tables[ds.Complaint_Quality.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Complaint_Quality.TableName].Select("1=1", ds.Complaint_Quality.QualityIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Complaint_Quality.DropDownColumn.ColumnName].ToString(), dr[ds.Complaint_Quality.QualityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetSeverity(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalComplaintLookup> obCase = new BLLClinical().GetSeverity();
            DSClinicalComplaintLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", "0"));
            if (ds != null)
            {
                if (ds.Tables[ds.Complaint_Severity.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Complaint_Severity.TableName].Select("1=1", ds.Complaint_Severity.SeverityIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Complaint_Severity.DropDownColumn.ColumnName].ToString(), dr[ds.Complaint_Severity.SeverityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetDuration(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalComplaintLookup> obCase = new BLLClinical().GetDuration();
            DSClinicalComplaintLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", "0"));
            if (ds != null)
            {
                if (ds.Tables[ds.Complaint_Duration.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Complaint_Duration.TableName].Select("1=1", ds.Complaint_Duration.DurationIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Complaint_Duration.DropDownColumn.ColumnName].ToString(), dr[ds.Complaint_Duration.DurationIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetFrequency(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalComplaintLookup> obCase = new BLLClinical().GetFrequency();
            DSClinicalComplaintLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", "0"));
            if (ds != null)
            {
                if (ds.Tables[ds.Complaint_Frequency.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Complaint_Frequency.TableName].Select("1=1", ds.Complaint_Frequency.FrequencyIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Complaint_Frequency.DropDownColumn.ColumnName].ToString(), dr[ds.Complaint_Frequency.FrequencyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetContext(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalComplaintLookup> obCase = new BLLClinical().GetContext();
            DSClinicalComplaintLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", "0"));
            if (ds != null)
            {
                if (ds.Tables[ds.Complaint_Context.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Complaint_Context.TableName].Select("1=1", ds.Complaint_Context.ContextIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Complaint_Context.DropDownColumn.ColumnName].ToString(), dr[ds.Complaint_Context.ContextIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetCharacter(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalComplaintLookup> obCase = new BLLClinical().GetCharacter();
            DSClinicalComplaintLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", "0"));
            if (ds != null)
            {
                if (ds.Tables[ds.Complaint_Character.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Complaint_Character.TableName].Select("1=1", ds.Complaint_Character.CharacterIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Complaint_Character.DropDownColumn.ColumnName].ToString(), dr[ds.Complaint_Character.CharacterIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetAggravatedBy(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalComplaintLookup> obCase = new BLLClinical().GetAggravatedBy();
            DSClinicalComplaintLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", "0"));
            if (ds != null)
            {
                if (ds.Tables[ds.Complaint_AggravatedBy.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Complaint_AggravatedBy.TableName].Select("1=1", ds.Complaint_AggravatedBy.AggravatedByIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Complaint_AggravatedBy.DropDownColumn.ColumnName].ToString(), dr[ds.Complaint_AggravatedBy.AggravatedByIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetRevieledBy(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalComplaintLookup> obCase = new BLLClinical().GetRevieledBy();
            DSClinicalComplaintLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", "0"));
            if (ds != null)
            {
                if (ds.Tables[ds.Complaint_RelievedBy.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Complaint_RelievedBy.TableName].Select("1=1", ds.Complaint_RelievedBy.RelievedByIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Complaint_RelievedBy.DropDownColumn.ColumnName].ToString(), dr[ds.Complaint_RelievedBy.RelievedByIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion


        #region "Template Letter"
        public string GetLetterCategory(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalLetterTemplateLookup> obCase = new BLLAdmin().GetLetterCategory();
            DSClinicalLetterTemplateLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Template_LetterCategory.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Template_LetterCategory.TableName].Select("1=1", ds.Template_LetterCategory.CategoryIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Template_LetterCategory.DropDownColumn.ColumnName].ToString(), dr[ds.Template_LetterCategory.CategoryIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetLetterTagCategory(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalLetterTemplateLookup> obCase = new BLLAdmin().GetLetterTagCategory();
            DSClinicalLetterTemplateLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Template_Letter_TagCategory.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Template_Letter_TagCategory.TableName].Select("1=1", ds.Template_Letter_TagCategory.TagCategoryIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Template_Letter_TagCategory.DropDownColumn.ColumnName].ToString(), dr[ds.Template_Letter_TagCategory.TagCategoryIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetLetterTagName(string IsActive, Int64 TagCategoryId)
        {
            int TagCategoryID = MDVUtility.ToInt32(TagCategoryId);

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalLetterTemplateLookup> obCase = new BLLAdmin().GetLetterTagName(TagCategoryID);
            DSClinicalLetterTemplateLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Template_Letter_TagCategoryName.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Template_Letter_TagCategoryName.TableName].Select("1=1", ds.Template_Letter_TagCategoryName.TagNameIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Template_Letter_TagCategoryName.DropDownColumn.ColumnName].ToString(), dr[ds.Template_Letter_TagCategoryName.TagNameIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region "Patient Template Letter"

        public string GetLetterTemplatesName(string IsActive, long ProviderId)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalLetterTemplateLookup> obCase = new BLLPatient().GetLetterTemplatesName(ProviderId);
            DSClinicalLetterTemplateLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Template_Letter_Name.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Template_Letter_Name.TableName].Select("1=1", ds.Template_Letter_Name.TemplateLetterIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Template_Letter_Name.DropDownColumn.ColumnName].ToString(), dr[ds.Template_Letter_Name.TemplateLetterIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region CDS
        /// <summary>
        /// Module Name: GetCDSAgeCondition
        /// Author: Humaira Yousaf
        /// Created Date: 02-03-2016
        /// Description: Gets CDS Age Conditions
        /// </summary>
        public string getCDSAgeCondition(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSCDSLookup> objAgeCondition = new BLLClinical().lookupCDSAgeCondition();
            DSCDSLookup ds = objAgeCondition.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.CDS_AgeCondition.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.CDS_AgeCondition.TableName].Select("1=1", ds.CDS_AgeCondition.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.CDS_AgeCondition.AgeConditionIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.CDS_AgeCondition.DescriptionColumn.ColumnName].ToString(), dr[ds.CDS_AgeCondition.AgeConditionIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Module Name: GetCDSReminderPeriod
        /// Author: Humaira Yousaf
        /// Created Date: 02-03-2016
        /// Description: Gets CDS Reminder Period
        /// </summary>
        public string getCDSReminderPeriod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSCDSLookup> objReminderPeriod = new BLLClinical().lookupCDSReminderPeriod();
            DSCDSLookup ds = objReminderPeriod.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.CDS_ReminderPeriod.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.CDS_ReminderPeriod.TableName].Select("1=1", ds.CDS_ReminderPeriod.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.CDS_ReminderPeriod.ReminderPeriodIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.CDS_ReminderPeriod.DescriptionColumn.ColumnName].ToString(), dr[ds.CDS_ReminderPeriod.ReminderPeriodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Module Name: GetCDSRuleType
        /// Author: Humaira Yousaf
        /// Created Date: 02-03-2016
        /// Description: Gets CDS Rule Type
        /// </summary>
        public string getCDSRuleType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSCDSLookup> objRuleType = new BLLClinical().lookupCDSRuleType();
            DSCDSLookup ds = objRuleType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.CDS_RuleType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.CDS_RuleType.TableName].Select("1=1", ds.CDS_RuleType.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.CDS_RuleType.RuleTypeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.CDS_RuleType.DescriptionColumn.ColumnName].ToString(), dr[ds.CDS_RuleType.RuleTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        /// <summary>
        /// Module Name: GetCDSTriggerLocation
        /// Author: Humaira Yousaf
        /// Created Date: 02-03-2016
        /// Description: Gets CDS Trigger Location
        /// </summary>
        public string getCDSTriggerLocation(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSCDSLookup> objTriggerLocation = new BLLClinical().lookupCDSTriggerLocation();
            DSCDSLookup ds = objTriggerLocation.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.CDS_TriggerLocation.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.CDS_TriggerLocation.TableName].Select("1=1", ds.CDS_TriggerLocation.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.CDS_TriggerLocation.TriggerLocationIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.CDS_TriggerLocation.DescriptionColumn.ColumnName].ToString(), dr[ds.CDS_TriggerLocation.TriggerLocationIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string getOrderSets(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSOrderSet> objOrderSet = new BLLClinical().lookupOrderSet();
            DSOrderSet ds = objOrderSet.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.OrderSetLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.OrderSetLookup.TableName].Select("1=1", ds.OrderSetLookup.OrderSetNameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.OrderSetLookup.OrderSetIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.OrderSetLookup.OrderSetNameColumn.ColumnName].ToString(), dr[ds.OrderSetLookup.OrderSetIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string getCDSRecursivePeriod(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSCDSLookup> objRecursivePeriod = new BLLClinical().lookupCDSRecursivePeriod();
            DSCDSLookup ds = objRecursivePeriod.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.CDS_RecursivePeriod.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.CDS_RecursivePeriod.TableName].Select("1=1", ds.CDS_RecursivePeriod.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.CDS_RecursivePeriod.RecursivePeriodIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.CDS_RecursivePeriod.DescriptionColumn.ColumnName].ToString(), dr[ds.CDS_RecursivePeriod.RecursivePeriodIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string getCDSQuestionnaireControlType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSCDSLookup> objRecursivePeriod = new BLLClinical().lookupQuestionnaireControlType();
            DSCDSLookup ds = objRecursivePeriod.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.QuestionnaireControlType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.QuestionnaireControlType.TableName].Select("1=1", ds.QuestionnaireControlType.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.QuestionnaireControlType.QuestionnaireControlTypeIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.QuestionnaireControlType.DescriptionColumn.ColumnName].ToString(), dr[ds.QuestionnaireControlType.QuestionnaireControlTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion
        #region Provider Notes template
        public string GetNoteTemplateType(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalNoteTemplateLookup> obCase = new BLLClinical().GetNoteTemplateType();
            DSClinicalNoteTemplateLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.NotesTemplateType.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.NotesTemplateType.TableName].Select("1=1", ds.NotesTemplateType.NotesTemplateTypeIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.NotesTemplateType.ShortNameColumn.ColumnName].ToString(), dr[ds.NotesTemplateType.NotesTemplateTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetPhoneEncounterTemplateType(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalNoteTemplateLookup> obCase = new BLLClinical().GetPhoneEncounterTemplateType();
            DSClinicalNoteTemplateLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.NotesTemplateType.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.NotesTemplateType.TableName].Select("1=1", ds.NotesTemplateType.NotesTemplateTypeIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.NotesTemplateType.ShortNameColumn.ColumnName].ToString(), dr[ds.NotesTemplateType.NotesTemplateTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetNoteTemplatTagCategory(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalNoteTemplateLookup> obCase = new BLLClinical().GetNoteTemplatTagCategory();
            DSClinicalNoteTemplateLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.NotesTagCategory.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.NotesTagCategory.TableName].Select("1=1", ds.NotesTagCategory.NotesTagCategoryIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.NotesTagCategory.ShortNameColumn.ColumnName].ToString(), dr[ds.NotesTagCategory.NotesTagCategoryIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetROSDataTemplate(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalNoteTemplateLookup> obCase = new BLLClinical().GetROSDataTemplate();
            DSClinicalNoteTemplateLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.NotesTagCategory.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.ROSDataTemplate.TableName].Select("1=1", ds.ROSDataTemplate.ROSDataTemplateIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ROSDataTemplate.NameColumn.ColumnName].ToString(), dr[ds.ROSDataTemplate.ROSDataTemplateIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetPhysExamDataTemplate(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalNoteTemplateLookup> obCase = new BLLClinical().GetPEDataTemplate();
            DSClinicalNoteTemplateLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.NotesTagCategory.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.PEDataTemplate.TableName].Select("1=1", ds.PEDataTemplate.DataTemplateIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PEDataTemplate.DataTemplateNameColumn.ColumnName].ToString(), dr[ds.PEDataTemplate.DataTemplateIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        public string GetNoteTemplateTagName(string IsActive, Int64 NoteTagCategory)
        {
            Int32 NoteTagCategor = MDVUtility.ToInt32(NoteTagCategory);
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalNoteTemplateLookup> obCase = new BLLClinical().GetNoteTemplateTagName(NoteTagCategor);
            DSClinicalNoteTemplateLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.NotesTagName.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.NotesTagName.TableName].Select("1=1", ds.NotesTagName.NotesTagNameIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.NotesTagName.ShortNameColumn.ColumnName].ToString(), dr[ds.NotesTagName.NotesTagNameIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }



        public string GetCategoryAgaintsSchAndSchtype(string IsActive, Int64 ScheduleTypeId, Int64 ScheduleId)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> obCase = new BLLClinical().GetCategoryAgaintsSchAndSchtype(ScheduleTypeId, ScheduleId);
            DSImmunization ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Category.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.Category.TableName].Select("1=1", ds.Category.VaccineGroupIDColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Category.ShortNameColumn.ColumnName].ToString(), dr[ds.Category.VaccineGroupIDColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetReminderTemplateTagName(string IsActive, Int64 NoteTagCategory)
        {
            Int32 NoteTagCategor = MDVUtility.ToInt32(NoteTagCategory);
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalNoteTemplateLookup> obCase = new BLLAdmin().GetReminderTemplateTagName(NoteTagCategor);
            DSClinicalNoteTemplateLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.NotesTagName.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.NotesTagName.TableName].Select("1=1", ds.NotesTagName.NotesTagNameIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.NotesTagName.ShortNameColumn.ColumnName].ToString(), dr[ds.NotesTagName.NotesTagNameIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetRemindersTemplateTagCategory(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalNoteTemplateLookup> obCase = new BLLAdmin().GetRemindersTemplateTagCategory();
            DSClinicalNoteTemplateLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.NotesTagCategory.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.NotesTagCategory.TableName].Select("1=1", ds.NotesTagCategory.NotesTagCategoryIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.NotesTagCategory.ShortNameColumn.ColumnName].ToString(), dr[ds.NotesTagCategory.NotesTagCategoryIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetPETemplate(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPhysicalExamECWLookup> obCase = new BLLPhysicalExamECW().GetPETemplate();
            DSPhysicalExamECWLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.PETemplateLookup.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.PETemplateLookup.TableName].Select("1=1", ds.PETemplateLookup.PETemplateIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.PETemplateLookup.TemplateNameColumn.ColumnName].ToString(), dr[ds.PETemplateLookup.PETemplateIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetHPITemplate(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var objHPITemplate = new BLLHPI().LookupHPITemplate();
            list.Add(new NameValuePair("- Select -", ""));
            foreach (var item in objHPITemplate.Data)
            {
                list.Add(new NameValuePair(item.Name, item.HPITemplateId));
            }
            return getJSONofList(list);
        }

        public string GetOrderSetTemplate(string IsActive, string Providerids)
        {

            DataTable dtProvider = new DataTable();
            DataColumn COLUMN = new DataColumn();
            COLUMN.ColumnName = "Id";
            COLUMN.DataType = typeof(int);
            dtProvider.Columns.Add(COLUMN);
           
            if (!Providerids.Equals("0"))
            {
                string[] strArry = Providerids.Split(',');
                for (int i = 0; i < strArry.Length; i++)
                {
                    DataRow Dr = dtProvider.NewRow();
                    Dr[0] = strArry[i];
                    dtProvider.Rows.Add(Dr);
                }
            }
            else
            {
                DataRow Dr = dtProvider.NewRow();
                Dr[0] = 0;
                dtProvider.Rows.Add(Dr);
            }         

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var objOrderSetTemplate = new BLLOrderSet().LookupOrderSetTemplate( dtProvider, "");
         
            list.Add(new NameValuePair("- Select -", ""));
            foreach (var item in objOrderSetTemplate.Data)
            {
                list.Add(new NameValuePair(item.OrderSetName, item.OrderSetId));
            }
            return getJSONofList(list);
        }

        public string GetOrderSetTemplateByID(string IsActive, string Providerids, string TemplateID)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            if (MDVUtility.ToLong(TemplateID) > 0)
            {
                var objOrderSetTemplate = new BLLOrderSet().LookupOrderSetTemplateByID(TemplateID);
                list.Add(new NameValuePair("- Select -", "0"));
                foreach (var item in objOrderSetTemplate.Data)
                {
                    list.Add(new NameValuePair(item.OrderSetName, item.OrderSetId));
                }
            }
            return getJSONofList(list);
        }
     
        #endregion

        #region Procedures
        public string LookupProblemLists(string IsActive, Int64 patientId)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProblemLists> obCase = new BLLClinical().LookupProblemListForPt(MDVUtility.ToInt32(patientId));
            DSProblemLists ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ProblemListForPt.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.ProblemListForPt.TableName].Select("1=1", ds.ProblemListForPt.ProblemListIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.ProblemListForPt.DescriptionColumn.ColumnName].ToString(), dr[ds.ProblemListForPt.ProblemListIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string LookupProblemListsForOrderSet(string IsActive, Int64 orderSetId)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSOS_ProblemLists> obCase = new BLLOrderSet().LookupProblemListsForOrderSet(MDVUtility.ToInt32(orderSetId));
            DSOS_ProblemLists ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ProblemListForOS.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.ProblemListForOS.TableName].Select("1=1", ds.ProblemListForOS.ProblemListIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.ProblemListForOS.DescriptionColumn.ColumnName].ToString(), dr[ds.ProblemListForOS.ProblemListIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region ReminderTemplates
        public string GetRemindersTemplateType(string IsActive, string type)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRemindersLookup> objTemplateTypes = new BLLAdmin().GetRemindersTemplateType(type);
            DSRemindersLookup ds = objTemplateTypes.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.RemindersTemplateType.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.RemindersTemplateType.TableName].Select("1=1", ds.RemindersTemplateType.RemindersTemplateIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.RemindersTemplateType.RemindersTemplateNameColumn.ColumnName].ToString(), dr[ds.RemindersTemplateType.RemindersTemplateIdColumn.ColumnName].ToString(), dr[ds.RemindersTemplateType.HTMLTemplateColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetWeekDays(string IsActive, Int64 TemplateID)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRemindersLookup> objWeekDays = new BLLAdmin().GetWeekDays();
            DSRemindersLookup ds = objWeekDays.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.WeekDays.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.WeekDays.TableName].Select("1=1", ds.WeekDays.WeekDaysIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.WeekDays.NameColumn.ColumnName].ToString(), dr[ds.WeekDays.WeekDaysIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetReminderConfirmationKey(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRemindersLookup> objWeekDays = new BLLAdmin().GetReminderConfirmationKey();
            DSRemindersLookup ds = objWeekDays.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ReminderConfirmationKey.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.ReminderConfirmationKey.TableName].Select("1=1", ds.ReminderConfirmationKey.ReminderConfirmationKeyIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.ReminderConfirmationKey.ValueColumn.ColumnName].ToString(), dr[ds.ReminderConfirmationKey.ReminderConfirmationKeyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetRemindersTextVoice(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRemindersLookup> RemindersTextVoice = new BLLAdmin().GetRemindersTextVoice();
            DSRemindersLookup ds = RemindersTextVoice.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.RemindersTextVoice.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.RemindersTextVoice.TableName].Select("1=1", ds.RemindersTextVoice.RemindersTextVoiceIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.RemindersTextVoice.NameColumn.ColumnName].ToString(), dr[ds.RemindersTextVoice.RemindersTextVoiceIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region "Favourities"

        public string GetBodyParts(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var obj = new BLLClinical().LoadBodyPartsLookUp();
            list.Add(new NameValuePair("- Select -", ""));
            List<BodyPartModel> bodypartsList = obj.Data;
            if (bodypartsList != null)
            {
                foreach (BodyPartModel item in bodypartsList)
                {
                    list.Add(new NameValuePair(item.BodyPart, item.BodyPartId, item.Position));
                }
            }
            return getJSONofList(list);
        }
        #region "Favorities Complaint"
        public string GetFavComplaint(string IsActive, string StrID = null)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFavoriteListLookup> obCase = new BLLClinical().GetFavListByProvider("Complaints", StrID);
            DSFavoriteListLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FavoriteListName.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.FavoriteListName.TableName].Select("1=1", ds.FavoriteListName.FavoriteListIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.FavoriteListName.DropDownColumn.ColumnName].ToString(), dr[ds.FavoriteListName.FavoriteListIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion
        #region "Favorities Problems"
        // Get Provider Specfic fav problem
        public string GetFavProblems(string IsActive, string StrID = null)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();            
            BLObject<DSFavoriteListLookup> obCase = new BLLClinical().GetFavListByProvider("Problems", StrID);
            DSFavoriteListLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FavoriteListName.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.FavoriteListName.TableName].Select("1=1", ds.FavoriteListName.FavoriteListIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.FavoriteListName.DropDownColumn.ColumnName].ToString(), dr[ds.FavoriteListName.FavoriteListIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        // Get all Favourite problem 
        public string GetFavProblems(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSFavoriteListLookup> obCase = new BLLClinical().GetFavComplaint("Problems");
            DSFavoriteListLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.FavoriteListName.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.FavoriteListName.TableName].Select("1=1", ds.FavoriteListName.FavoriteListIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.FavoriteListName.DropDownColumn.ColumnName].ToString(), dr[ds.FavoriteListName.FavoriteListIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetFavVaccine(string IsActive, string ProviderId, string Tab, string Type, Int64 Category)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            List<FavoriteListImmunizationModel> FavoriteListImmunizationList = null;
            if (Category == -1)
            {
                Category = 0;
            }
            BLObject<List<FavoriteListImmunizationModel>> obCase = new BLLClinical().LoadFavImmunizationLookUp(MDVUtility.ToInt64(ProviderId), Tab, Type, Category);

            list.Add(new NameValuePair("- Select -", ""));
            FavoriteListImmunizationList = obCase.Data;
            if (obCase.Data != null)
            {
                if (FavoriteListImmunizationList != null)
                {
                    foreach (FavoriteListImmunizationModel item in FavoriteListImmunizationList)
                    {
                        list.Add(new NameValuePair(item.FavoriteListName, item.FavoritiesListId));
                    }
                }
            }
            return getJSONofList(list);
        }




        #endregion
        #endregion

        #region "Immunization"
        //public string Getimm(string IsActive,object data)

        //Function Name: GetAdministerVaccine_Category
        //Author Name: Talha Tanweer
        //Created Date: 05-04-2016
        //Description:
        public string GetAdministerVaccine_Category(string IsActive, string Active)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccine_Category = new BLLClinical().LookupVaccineGroupCategory(Active == "1" ? true : false);
            DSImmunization ds = objVaccine_Category.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LookUpVaccineGroup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookUpVaccineGroup.TableName].Select("1=1", ds.LookUpVaccineGroup.ShortNameColumn.ColumnName);
                    string VaccineHxId = ds.LookUpVaccineGroup.VaccineGroupIDColumn.ColumnName;
                    string VaccineGroupCategoryShortName = ds.LookUpVaccineGroup.ShortNameColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineGroupCategoryShortName].ToString(), dr[VaccineHxId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetAdministerVaccine_CategoryOp(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccine_Category = new BLLClinical().LookupVaccineGroupCategory(true);
            DSImmunization ds = objVaccine_Category.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LookUpVaccineGroup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookUpVaccineGroup.TableName].Select("1=1", ds.LookUpVaccineGroup.ShortNameColumn.ColumnName);
                    string VaccineHxId = ds.LookUpVaccineGroup.VaccineGroupIDColumn.ColumnName;
                    string VaccineGroupCategoryShortName = ds.LookUpVaccineGroup.ShortNameColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineGroupCategoryShortName].ToString(), dr[VaccineHxId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        public string GetAdministerVaccine_Category(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccine_Category = new BLLClinical().LookupVaccineGroupCategory(true);
            DSImmunization ds = objVaccine_Category.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LookUpVaccineGroup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookUpVaccineGroup.TableName].Select("1=1", ds.LookUpVaccineGroup.ShortNameColumn.ColumnName);
                    string VaccineHxId = ds.LookUpVaccineGroup.VaccineGroupIDColumn.ColumnName;
                    string VaccineGroupCategoryShortName = ds.LookUpVaccineGroup.ShortNameColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineGroupCategoryShortName].ToString(), dr[VaccineHxId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetImmunizationAlerts(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objImmunizationAlertType = new BLLClinical().LookupImmunizationAlerts();
            DSImmunization ds = objImmunizationAlertType.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ImmunizationAlertType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ImmunizationAlertType.TableName].Select("1=1", ds.ImmunizationAlertType.AlertDescriptionColumn.ColumnName);
                    string AlertId = ds.ImmunizationAlertType.AlertIdColumn.ColumnName;
                    string AlertDescription = ds.ImmunizationAlertType.AlertDescriptionColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[AlertDescription].ToString(), dr[AlertId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }







        //Function Name: GetAdministerVaccine_Vaccine
        //Author Name: Talha Tanweer
        //Created Date: 05-04-2016
        //Description:
        public string GetAdministerVaccine_Vaccine(string IsActive, string VaccineGroupCategoryId, string forModule)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccine_Category = new BLLClinical().LookupVaccine(MDVUtility.ToInt64(VaccineGroupCategoryId), forModule);
            DSImmunization ds = objVaccine_Category.Data;


            if (ds != null)
            {
                if (ds.Tables[ds.LookupVaccine.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupVaccine.TableName].Select("1=1", ds.LookupVaccine.CVXShortDescriptionColumn.ColumnName);
                    string VaccineID = ds.LookupVaccine.VaccineIDColumn.ColumnName;
                    string CVXCode = ds.LookupVaccine.CVXCodeColumn.ColumnName;
                    string CPTCode = ds.LookupVaccine.CPTCodeColumn.ColumnName;
                    string cvxShortDescription = ds.LookupVaccine.CVXShortDescriptionColumn.ColumnName;
                    string priority = ds.LookupVaccine.PriorityColumn.ColumnName;
                    int count = 0;
                    foreach (DataRow dr in dRows)
                    {
                        if (dr[priority].ToString() == "1")
                        {
                            count++;
                            list.Add(new NameValuePair(dr[cvxShortDescription].ToString(), dr[VaccineID].ToString(), dr[CVXCode].ToString(), dr[CPTCode].ToString()));
                        }
                    }
                    if (count == 0)
                    {
                        list.Add(new NameValuePair("- Select -", ""));
                    }
                    foreach (DataRow dr in dRows)
                    {
                        if (dr[priority].ToString() == "0")
                        {
                            list.Add(new NameValuePair(dr[cvxShortDescription].ToString(), dr[VaccineID].ToString(), dr[CVXCode].ToString(), dr[CPTCode].ToString()));
                        }
                    }
                }
            }
            return getJSONofList(list);
        }



        public string GetVaccine(string IsActive, string VaccineGroupCategoryId, string forModule)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccine_Category = new BLLClinical().LookupVaccine(MDVUtility.ToInt64(VaccineGroupCategoryId), forModule);
            DSImmunization ds = objVaccine_Category.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.LookupVaccine.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupVaccine.TableName].Select("1=1", ds.LookupVaccine.CVXShortDescriptionColumn.ColumnName);
                    string VaccineID = ds.LookupVaccine.VaccineIDColumn.ColumnName;
                    string CVXCode = ds.LookupVaccine.CVXCodeColumn.ColumnName;
                    string CPTCode = ds.LookupVaccine.CPTCodeColumn.ColumnName;
                    string cvxShortDescription = ds.LookupVaccine.CVXShortDescriptionColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[cvxShortDescription].ToString(), dr[VaccineID].ToString(), dr[CVXCode].ToString(), dr[CPTCode].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetVaccine(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccine_Category = new BLLClinical().LookupVaccine(0, "");
            DSImmunization ds = objVaccine_Category.Data;

            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.LookupVaccine.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupVaccine.TableName].Select("1=1", ds.LookupVaccine.CVXShortDescriptionColumn.ColumnName);
                    string VaccineID = ds.LookupVaccine.VaccineIDColumn.ColumnName;
                    string CVXCode = ds.LookupVaccine.CVXCodeColumn.ColumnName;
                    string CPTCode = ds.LookupVaccine.CPTCodeColumn.ColumnName;
                    string cvxShortDescription = ds.LookupVaccine.CVXShortDescriptionColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[cvxShortDescription].ToString(), dr[VaccineID].ToString(), dr[CVXCode].ToString(), dr[CPTCode].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        //Function Name: GetVaccineRoute
        //Author Name: Talha Tanweer
        //Created Date: 05-04-2016
        //Description:
        public string GetVaccineRoute(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineRoute = new BLLClinical().LookupVaccineRoute();
            DSImmunization ds = objVaccineRoute.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LookUpVaccineGroup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupVaccineRoute.TableName].Select("1=1", ds.LookupVaccineRoute.DescriptionColumn.ColumnName);
                    string VaccineRouteHL7Code = ds.LookupVaccineRoute.VaccineRouteIDColumn.ColumnName;
                    string VaccineRouteDescription = ds.LookupVaccineRoute.DescriptionColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineRouteDescription].ToString(), dr[VaccineRouteHL7Code].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetVaccineFundingSource(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineFundingSource = new BLLClinical().LookupVaccineFundingSource();
            DSImmunization ds = objVaccineFundingSource.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.LookUpVaccineGroup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupVaccineFundingSource.TableName].Select("1=1", ds.LookupVaccineFundingSource.DescriptionColumn.ColumnName);
                    string VaccineFundingSourceID = ds.LookupVaccineFundingSource.IdColumn.ColumnName;
                    string VaccineRouteDescription = ds.LookupVaccineFundingSource.DescriptionColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineRouteDescription].ToString(), dr[VaccineFundingSourceID].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetVaccineAmount(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineAmount = new BLLClinical().LookupVaccineAmount();
            DSImmunization ds = objVaccineAmount.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineReactionLookups.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineReactionLookups.TableName].Select("1=1", ds.VaccineReactionLookups.DropDownColumn.ColumnName);
                    string Id = ds.VaccineReactionLookups.LookUpIdColumn.ColumnName;
                    string Description = ds.VaccineReactionLookups.DropDownColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[Description].ToString(), dr[Id].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetVaccineAmountForClinicalSide(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineAmount = new BLLClinical().LookupVaccineAmount();
            DSImmunization ds = objVaccineAmount.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineReactionLookups.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineReactionLookups.TableName].Select("1=1", ds.VaccineReactionLookups.DropDownColumn.ColumnName);
                    string Id = ds.VaccineReactionLookups.LookUpIdColumn.ColumnName;
                    string Description = ds.VaccineReactionLookups.DropDownColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[Description].ToString(), dr[Description].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetTherapeuticInjection(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineRoute = new BLLClinical().LookupTherapeuticInjection();
            DSImmunization ds = objVaccineRoute.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LookupTherapeuticInjection.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupTherapeuticInjection.TableName].Select("1=1", ds.LookupTherapeuticInjection.DescriptionColumn.ColumnName);
                    string TherapeuticInjectionID = ds.LookupTherapeuticInjection.TherapeuticInjectionIDColumn.ColumnName;
                    string Description = ds.LookupTherapeuticInjection.DescriptionColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[Description].ToString(), dr[TherapeuticInjectionID].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        public string GetCQMEncounterType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineRoute = new BLLClinical().GetCQMEncounterType();
            DSImmunization ds = objVaccineRoute.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineRefusalReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineRefusalReason.TableName].Select("1=1", ds.VaccineRefusalReason.DropDownColumn.ColumnName);
                    string VaccineRefusalReasonId = ds.VaccineRefusalReason.LookUpIdColumn.ColumnName;
                    string VaccineRefusalReasonDropDown = ds.VaccineRefusalReason.DropDownColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineRefusalReasonDropDown].ToString(), dr[VaccineRefusalReasonId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }



        //Function Name: GetVaccineRefusalReason
        //Author Name: M Ahmad Imran
        //Created Date: 05-04-2016
        //Description:
        public string GetVaccineRefusalReason(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineRoute = new BLLClinical().LookupVaccineRefusalReason();
            DSImmunization ds = objVaccineRoute.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineRefusalReason.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineRefusalReason.TableName].Select("1=1", ds.VaccineRefusalReason.DropDownColumn.ColumnName);
                    string VaccineRefusalReasonId = ds.VaccineRefusalReason.LookUpIdColumn.ColumnName;
                    string VaccineRefusalReasonDropDown = ds.VaccineRefusalReason.DropDownColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineRefusalReasonDropDown].ToString(), dr[VaccineRefusalReasonId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        //Function Name: GetVaccineSite
        //Author Name: Talha Tanweer
        //Created Date: 05-04-2016
        //Description:
        public string GetVaccineSite(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineSite = new BLLClinical().LookupVaccineSite();
            DSImmunization ds = objVaccineSite.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LookupVaccineSite.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupVaccineSite.TableName].Select("1=1", ds.LookupVaccineSite.DescriptionColumn.ColumnName);
                    string VaccineSiteHL7Code = ds.LookupVaccineSite.VaccineSiteIDColumn.ColumnName;
                    string VaccineSiteDescription = ds.LookupVaccineSite.DescriptionColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineSiteDescription].ToString(), dr[VaccineSiteHL7Code].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetAnswers(string IsActive, long QuestionId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineSite = new BLLClinical().GetAnswers(QuestionId);
            DSImmunization ds = objVaccineSite.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineReactionLookups.TableName] != null)
                {
                    foreach (DSImmunization.VaccineReactionLookupsRow dr in ds.VaccineReactionLookups.Rows)
                    {
                        list.Add(new NameValuePair(dr[ds.VaccineReactionLookups.DropDownColumn].ToString() + "~_~" + dr[ds.VaccineReactionLookups.TitleColumn].ToString(), dr[ds.VaccineReactionLookups.LookUpIdColumn].ToString(), "", "", "", "", "", dr[ds.VaccineReactionLookups.TitleColumn].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        //Function Name: LookupVaccinePublicityCode
        //Author Name: M Ahmad Imran
        //Created Date: 22-07-2016
        //Description: for get Publicity Code of Vaccine
        public string GetPublicityCode(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineSite = new BLLClinical().LookupVaccinePublicityCode();
            DSImmunization ds = objVaccineSite.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccinePublicityCodeLookups.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccinePublicityCodeLookups.TableName].Select("1=1", ds.VaccinePublicityCodeLookups.DropDownColumn.ColumnName);
                    string PublicityCodeId = ds.VaccinePublicityCodeLookups.LookUpIdColumn.ColumnName;
                    string PublicityCodeDescription = ds.VaccinePublicityCodeLookups.DropDownColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[PublicityCodeDescription].ToString(), dr[PublicityCodeId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        //Function Name: LookupVaccinePublicityCode
        //Author Name: M Ahmad Imran
        //Created Date: 22-07-2016
        //Description: for get Publicity Code of Vaccine
        public string GetRegistryStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineSite = new BLLClinical().LookupVaccineRegistryStatus();
            DSImmunization ds = objVaccineSite.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineRegistryStatusLookups.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineRegistryStatusLookups.TableName].Select("1=1", ds.VaccineRegistryStatusLookups.DropDownColumn.ColumnName);
                    string RegistryStatusId = ds.VaccineRegistryStatusLookups.LookUpIdColumn.ColumnName;
                    string RegistryStatusDescription = ds.VaccineRegistryStatusLookups.DropDownColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[RegistryStatusDescription].ToString(), dr[RegistryStatusId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }



        //Function Name: GetVaccineReaction
        //Author Name: M Ahmad Imran
        //Created Date: 21-07-2016
        //Description:
        public string GetVaccineReaction(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineSite = new BLLClinical().LookupVaccineReaction();
            DSImmunization ds = objVaccineSite.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineReactionLookups.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineReactionLookups.TableName].Select("1=1", ds.VaccineReactionLookups.DropDownColumn.ColumnName);
                    string Id = ds.VaccineReactionLookups.LookUpIdColumn.ColumnName;
                    string DropDown = ds.VaccineReactionLookups.DropDownColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[DropDown].ToString(), dr[Id].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        //Function Name: GetVaccineSite
        //Author Name: Talha Tanweer
        //Created Date: 08-04-2016
        //Description:
        public string GetVaccineGivenBy(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineGivenBy = new BLLClinical().LookupVaccineGivenBy();
            DSImmunization ds = objVaccineGivenBy.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LookupVaccineGivenBy.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupVaccineGivenBy.TableName].Select("1=1", ds.LookupVaccineGivenBy.GivenByColumn.ColumnName);
                    string VaccineUserId = ds.LookupVaccineGivenBy.UserIdColumn.ColumnName;
                    string VaccineGivenBy = ds.LookupVaccineGivenBy.GivenByColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineGivenBy].ToString(), dr[VaccineUserId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        //Function Name: GetVaccineVFC
        //Author Name: Talha Tanweer
        //Created Date: 05-04-2016
        //Description:
        public string GetVaccineVFC(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineVfc = new BLLClinical().LookupVaccineVfc();
            DSImmunization ds = objVaccineVfc.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LookupVaccineVFC.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupVaccineVFC.TableName].Select("1=1", ds.LookupVaccineVFC.ConceptNameColumn.ColumnName);
                    string VaccineVFCId = ds.LookupVaccineVFC.VaccineVFCIdColumn.ColumnName;
                    string VaccineVFDescription = ds.LookupVaccineVFC.ConceptNameColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineVFDescription].ToString(), dr[VaccineVFCId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        //Function Name: GetVaccineSourceOfHx
        //Author Name: Talha Tanweer
        //Created Date: 05-04-2016
        //Description:
        public string GetVaccineSourceOfHx(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineSourceOfHx = new BLLClinical().LookupVaccineSourceOfHx();
            DSImmunization ds = objVaccineSourceOfHx.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LookupVaccineSourceOfHx.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupVaccineSourceOfHx.TableName].Select("1=1", ds.LookupVaccineSourceOfHx.DescriptionColumn.ColumnName);
                    string VaccineSourceOfHxId = ds.LookupVaccineSourceOfHx.VaccineSourceOfHxIdColumn.ColumnName;
                    string VaccineSourceOfHxDescription = ds.LookupVaccineSourceOfHx.DescriptionColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineSourceOfHxDescription].ToString(), dr[VaccineSourceOfHxId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }


        //Function Name: GetVaccineManufacturer
        //Author Name: Talha Tanweer
        //Created Date: 05-04-2016
        //Description:
        public string GetVaccineManufacturer(string IsActive, string VaccineId, string TherpeuticId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineManufacturer = new BLLClinical().LookupVaccineManufacturer(VaccineId, TherpeuticId);
            DSImmunization ds = objVaccineManufacturer.Data;
            if (ds.LookupVaccineManufacturer.Count == 1)
            {

            }
            else
            {
                list.Add(new NameValuePair("- Select -", ""));
            }

            if (ds != null)
            {
                if (ds.Tables[ds.LookupVaccineManufacturer.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupVaccineManufacturer.TableName].Select("1=1", ds.LookupVaccineManufacturer.ManufacturerNameColumn.ColumnName);
                    string VaccineManufacturerId = ds.LookupVaccineManufacturer.VaccineManufacturerIdColumn.ColumnName;
                    string VaccineManufacturerName = ds.LookupVaccineManufacturer.ManufacturerNameColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineManufacturerName].ToString(), dr[VaccineManufacturerId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetVaccineManufacturer(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineManufacturer = new BLLClinical().LookupVaccineManufacturer();
            DSImmunization ds = objVaccineManufacturer.Data;
            if (ds.LookupVaccineManufacturer.Count == 1)
            {

            }
            else
            {
                list.Add(new NameValuePair("- Select -", ""));
            }

            if (ds != null)
            {
                if (ds.Tables[ds.LookupVaccineManufacturer.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupVaccineManufacturer.TableName].Select("1=1", ds.LookupVaccineManufacturer.ManufacturerNameColumn.ColumnName);
                    string VaccineManufacturerId = ds.LookupVaccineManufacturer.VaccineManufacturerIdColumn.ColumnName;
                    string VaccineManufacturerName = ds.LookupVaccineManufacturer.ManufacturerNameColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineManufacturerName].ToString(), dr[VaccineManufacturerId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetVaccineLotNumber(string IsActive, Int64 ID, Int64 ProviderId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineLotNumber = new BLLClinical().LookupVaccineLotNumbers(ID, ProviderId);
            DSImmunization ds = objVaccineLotNumber.Data;
            if (ds.VaccineLotNumberLookups.Count == 1)
            {
            }
            else
            {
                list.Add(new NameValuePair("- Select -", ""));
            }
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineLotNumberLookups.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineLotNumberLookups.TableName].Select("1=1", ds.VaccineLotNumberLookups.DropDownColumn.ColumnName);
                    string VaccineLotNumberId = ds.VaccineLotNumberLookups.LookUpIdColumn.ColumnName;
                    string VaccineLotNumberName = ds.VaccineLotNumberLookups.DropDownColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineLotNumberName].ToString(), dr[VaccineLotNumberId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetTherapeuticInjectionLotNumber(string IsActive, Int64 ID, Int64 ProviderId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineLotNumber = new BLLClinical().LookupTherapeuticInjectionLotNumber(MDVUtility.ToInt(ID), ProviderId);
            DSImmunization ds = objVaccineLotNumber.Data;
            if (ds.VaccineLotNumberLookups.Count == 1)
            {
            }
            else
            {
                list.Add(new NameValuePair("- Select -", ""));
            }
            if (ds != null)
            {
                if (ds.Tables[ds.VaccineLotNumberLookups.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.VaccineLotNumberLookups.TableName].Select("1=1", ds.VaccineLotNumberLookups.DropDownColumn.ColumnName);
                    string VaccineLotNumberId = ds.VaccineLotNumberLookups.LookUpIdColumn.ColumnName;
                    string VaccineLotNumberName = ds.VaccineLotNumberLookups.DropDownColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[VaccineLotNumberName].ToString(), dr[VaccineLotNumberId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string Getimm(string IsActive, string param1, string param2)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSProblemLists> objschedule = new BLLClinical().LookupSeverityType();
            DSProblemLists ds = objschedule.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.SeverityType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.SeverityType.TableName].Select("1=1", ds.SeverityType.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.SeverityType.ShortNameColumn.ColumnName].ToString(), dr[ds.SeverityType.ShortNameColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetImmunizationSchedule(string IsActive, Int64 ScheduleTypeId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objImmunizationSchedule = new BLLClinical().LookupSchedule(MDVUtility.ToStr(ScheduleTypeId));
            DSImmunization ds = objImmunizationSchedule.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LookupSchedule.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupSchedule.TableName].Select("1=1", ds.LookupSchedule.ScheduleIdColumn.ColumnName);
                    string ScheduleId = ds.LookupSchedule.ScheduleIdColumn.ColumnName;
                    string ScheduleDes = ds.LookupSchedule.DescriptionColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ScheduleDes].ToString(), dr[ScheduleId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetImmunizationScheduleType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objImmunizationSchedule = new BLLClinical().LookupScheduleType();
            DSImmunization ds = objImmunizationSchedule.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LookupScheduleType.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupScheduleType.TableName].Select("1=1", ds.LookupScheduleType.ScheduleTypeIdColumn.ColumnName);
                    string ScheduleId = ds.LookupScheduleType.ScheduleTypeIdColumn.ColumnName;
                    string ScheduleDes = ds.LookupScheduleType.DescriptionColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ScheduleDes].ToString(), dr[ScheduleId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetManufacturer(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSImmunization> objVaccineManufacturer = new BLLClinical().LookupManufacturer();
            DSImmunization ds = objVaccineManufacturer.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LookupManufacturer.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupManufacturer.TableName].Select("1=1", ds.LookupManufacturer.ManufacturerNameColumn.ColumnName);
                    string ManufacturerId = ds.LookupManufacturer.ManufacturerIdColumn.ColumnName;
                    string ManufacturerName = ds.LookupManufacturer.ManufacturerNameColumn.ColumnName;
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ManufacturerName].ToString(), dr[ManufacturerId].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetRegistery(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSImmunization> objRegistery = new BLLClinical().LookupRegistery(IsActive);
            DSImmunization ds = objRegistery.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.LookupRegistery.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupRegistery.TableName].Select("1=1", ds.LookupRegistery.ReceivingApplicationNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.LookupRegistery.ReceivingApplicationNameColumn.ColumnName].ToString(), dr[ds.LookupRegistery.ReceivingApplicationIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetHL7BatchMessageType(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSImmunizationHL7> obj = new BLLBatch().LookupHL7BatchMessageType();
            DSImmunizationHL7 ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.HL7MessageTypeLookUp.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.HL7MessageTypeLookUp.TableName].Select("1=1", ds.HL7MessageTypeLookUp.TypeNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.HL7MessageTypeLookUp.TypeNameColumn.ColumnName].ToString(), dr[ds.HL7MessageTypeLookUp.TypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetHL7BatchStatus(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSImmunizationHL7> obj = new BLLBatch().LookupHL7BatchStatus();
            DSImmunizationHL7 ds = obj.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.HL7StatusLookUp.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.HL7StatusLookUp.TableName].Select("1=1", ds.HL7StatusLookUp.StatusNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.HL7StatusLookUp.StatusNameColumn.ColumnName].ToString(), dr[ds.HL7StatusLookUp.StatusIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetRegistrySubmission(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSImmunization> objRegistery = new BLLClinical().LookupRegistrySubmission(IsActive);
            DSImmunization ds = objRegistery.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.LookupRegistery.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LookupRegistrySubmission.TableName].Select("1=1", ds.LookupRegistrySubmission.RegistrySubmissionNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.LookupRegistrySubmission.RegistrySubmissionNameColumn.ColumnName].ToString(), dr[ds.LookupRegistrySubmission.RegistrySubmissionIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        #endregion

        #region Clincial LabOrder

        /// <summary>
        /// Author : Abid Ali
        /// Date : 07-04-2016
        /// Overview : Diet Lookup for LabOrder
        /// </summary>
        /// <returns></returns>
        public string getClinicalLabOrderDiet(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSRadiologyOrderLookup> obHistory = new BLLClinical().lookupLabOrderDiet();
            DSRadiologyOrderLookup ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Diet.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Diet.TableName].Select("1=1", ds.Diet.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Diet.DietIdColumn.ColumnName]))
                    {

                        list.Add(new NameValuePair(dr[ds.Diet.DescriptionColumn.ColumnName].ToString(), dr[ds.Diet.DietIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetLabs(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSClinicalLab> obHistory = new BLLClinical().getLabsLookup();
            DSClinicalLab ds = obHistory.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.LabLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.LabLookup.TableName].Select("1=1", ds.LabLookup.NameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.LabLookup.LabIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.LabLookup.NameColumn.ColumnName].ToString(), dr[ds.LabLookup.LabIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        #endregion

        #region " Time Zones "
        public string GetTimeZones(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            System.Collections.ObjectModel.ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
            list.Add(new NameValuePair("- Select -", ""));
            if (timeZones != null)
            {
                int count = 1;
                foreach (TimeZoneInfo time in timeZones)
                {
                    string hours = time.BaseUtcOffset.Hours.ToString();
                    if (time.BaseUtcOffset.Hours == 0)
                        hours = "+00";
                    else if (time.BaseUtcOffset.Hours > 0 && time.BaseUtcOffset.Hours <= 9)
                        hours = "+0" + hours;
                    else if (time.BaseUtcOffset.Hours < 0 && time.BaseUtcOffset.Hours >= -9)
                        hours = "-0" + Math.Abs(time.BaseUtcOffset.Hours);

                    string Totalhours = time.BaseUtcOffset.TotalHours.ToString();
                    if (time.BaseUtcOffset.TotalHours == 0)
                        Totalhours = "+00";
                    else if (time.BaseUtcOffset.TotalHours > 0 && time.BaseUtcOffset.TotalHours <= 9)
                        Totalhours = "+0" + Totalhours;
                    else if (time.BaseUtcOffset.TotalHours < 0 && time.BaseUtcOffset.TotalHours >= -9 && time.BaseUtcOffset.TotalHours == Math.Floor(time.BaseUtcOffset.TotalHours))
                        Totalhours = "-0" + Math.Abs(time.BaseUtcOffset.TotalHours);


                    list.Add(new NameValuePair(time.DisplayName, count.ToString(), hours, Totalhours));
                    count++;
                }
            }
            return getJSONofList(list);
        }

        #endregion

        #region Notes Extra Info
        public string GetValueSetting(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSNotes> obCase = new BLLClinical().LookupValueSetting(3);
            DSNotes ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ValueSetting.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.ValueSetting.TableName].Select("1=1", ds.ValueSetting.ValueSettingIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.ValueSetting.DescriptionColumn.ColumnName].ToString(), dr[ds.ValueSetting.ValueSettingIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        public string GetValueSettingTopTwo(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSNotes> obCase = new BLLClinical().LookupValueSetting(2);
            DSNotes ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ValueSetting.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.ValueSetting.TableName].Select("1=1", ds.ValueSetting.ValueSettingIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.ValueSetting.DescriptionColumn.ColumnName].ToString(), dr[ds.ValueSetting.ValueSettingIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region BilllingInformation

        public string GetBillingInfoType(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSBillingInformationLookup> obCase = new BLLClinical().LookupBillingInfoType();
            DSBillingInformationLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.BillingInfoType.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.BillingInfoType.TableName].Select("1=1", ds.BillingInfoType.BillingInfoTypeIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.BillingInfoType.DescriptionColumn.ColumnName].ToString(), dr[ds.BillingInfoType.BillingInfoTypeIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }



        #endregion

        #region PQRS Measure Provider Group Lookup

        public string GetMeasureProviderGroup(string IsActive)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPQRS> obCase = new BLLPQRS().LookupMeasureProviderGroup();
            DSPQRS ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.MeasureGroupLookup.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.MeasureGroupLookup.TableName].Select("1=1", ds.MeasureGroupLookup.MeasureGroupIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.MeasureGroupLookup.ShortNameColumn.ColumnName].ToString(), dr[ds.MeasureGroupLookup.MeasureGroupIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region Out Of Office Visit
        public string GetOutOfOfficeVisitProvider(string IsActive, string PatientId)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSOutOfOfficeVisitLookup> obCase = new BLLClinical().LookupOutOfOfficeVisitProvider(MDVUtility.ToInt64(PatientId));
            DSOutOfOfficeVisitLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.OutOfOfficeVisitProvider.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.OutOfOfficeVisitProvider.TableName].Select("1=1", ds.OutOfOfficeVisitProvider.ProviderIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.OutOfOfficeVisitProvider.DescriptionColumn.ColumnName].ToString(), dr[ds.OutOfOfficeVisitProvider.ProviderIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        public string GetOutOfOfficeVisitFacility(string IsActive, string PatientId)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSOutOfOfficeVisitLookup> obCase = new BLLClinical().LookupOutOfOfficeVisitFacility(MDVUtility.ToInt64(PatientId));
            DSOutOfOfficeVisitLookup ds = obCase.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.OutOfOfficeVisitFacility.TableName] != null)
                {

                    DataRow[] dRows = ds.Tables[ds.OutOfOfficeVisitFacility.TableName].Select("1=1", ds.OutOfOfficeVisitFacility.FacilityIdColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {

                        list.Add(new NameValuePair(dr[ds.OutOfOfficeVisitFacility.DescriptionColumn.ColumnName].ToString(), dr[ds.OutOfOfficeVisitFacility.FacilityIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        #endregion

        #region Report Header
        public string GetReportHeaders(string IsActive, long providerId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSReportHeader> objHeaders = new BLLAdminClinical().LookupReportHeader(providerId);
            DSReportHeader ds = objHeaders.Data;
            list.Add(new NameValuePair("- Blank -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.ReportHeader.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.ReportHeader.TableName].Select("1=1", ds.ReportHeader.NameColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.ReportHeader.ReportHeaderIdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.ReportHeader.NameColumn.ColumnName].ToString(), dr[ds.ReportHeader.ReportHeaderIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region Clinical Reports Lookups
        /// <summary>
        /// Module Name: GetPharmacy
        /// Author: Humaira Yousaf
        /// Created Date: 07-09-2016
        /// Description: Gets Pharmacy
        /// </summary>
        /// <param name="IsActive" type="bool">IsActive</param>
        public string GetPharmacy(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSMedicationLookup> objPharmacy = new BLLReports().getPharmacyLookup();
            DSMedicationLookup ds = objPharmacy.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {

                if (ds.Tables[ds.Pharmacy.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Pharmacy.TableName].Select("1=1", ds.Pharmacy.PharmacyNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Pharmacy.PharmacyNameColumn.ColumnName].ToString(), dr[ds.Pharmacy.PharmacyIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }
        #endregion

        #region CCM Lookups
        public string GetCarePlanTempt(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<List<MDVision.Model.CCM.PatientHub.CarePlanTemptModel>> objPharmacy = new BLLCCM().loadCarePlanTemplates();
            List<MDVision.Model.CCM.PatientHub.CarePlanTemptModel> modellist = objPharmacy.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (modellist != null && modellist.Count > 0)
            {
                foreach (MDVision.Model.CCM.PatientHub.CarePlanTemptModel obj in modellist)
                {
                    list.Add(new NameValuePair(obj.TemplateName, obj.TemplateId.ToString()));
                }
            }
            return getJSONofList(list);
        }
        public string GetCallDetails(string IsActive, Int64 EnrollmentInfoId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            List<MDVision.Model.CCM.CCMHub.CallDetailLookupModel> modellist = new DALCCMProgramUpdate().CallDetailsLookup(EnrollmentInfoId);
            list.Add(new NameValuePair("- Select -", ""));
            if (modellist != null && modellist.Count > 0)
            {
                foreach (MDVision.Model.CCM.CCMHub.CallDetailLookupModel obj in modellist)
                {
                    var callerName = obj.ProviderName + "~_~";
                    var callerTitle = obj.Title;
                    var callerString = callerName + callerTitle;

                    list.Add(new NameValuePair(callerString, obj.CareTeamId));
                }
            }
            return getJSONofList(list);
        }

        //public string GetSpecialtyProvider(string IsActive, Int64 SpecialityId)
        //{
        //    HashSet<NameValuePair> list = new HashSet<NameValuePair>();
        //    BLObject<DSPhysicalExamECW> objHeaders = new BLLPhysicalExamECW().GetSpecialtyProvider(SpecialityId);
        //    DSPhysicalExamECW ds = objHeaders.Data;
        //    list.Add(new NameValuePair("- Select -", ""));

        //    if (ds != null)
        //    {
        //        if (ds.Tables[ds.SpecialityProvider.TableName] != null)
        //        {
        //            DataRow[] dRows = ds.Tables[ds.SpecialityProvider.TableName].Select("1=1", ds.SpecialityProvider.NameColumn.ColumnName);

        //            foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.SpecialityProvider.NameColumn.ColumnName]))
        //            {
        //                list.Add(new NameValuePair(dr[ds.SpecialityProvider.NameColumn.ColumnName].ToString(), dr[ds.SpecialityProvider.ProviderIdColumn.ColumnName].ToString()));
        //            }
        //        }
        //    }
        //    return getJSONofList(list);
        //}
        public string GetPatientCareTeams(string IsActive, Int64 PatientId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            List<MDVision.Model.CCM.CCMHub.CareTeamLookupModel> modellist = new DALCCMProgramUpdate().PatientCareTeamLookup(PatientId);
            list.Add(new NameValuePair("- Select -", ""));
            if (modellist != null && modellist.Count > 0)
            {
                foreach (MDVision.Model.CCM.CCMHub.CareTeamLookupModel obj in modellist)
                {
                    var careTeamName = obj.CareTeamName + "~_~";
                    var careTeamMembers = "Provider: " + obj.ProviderName + ", Primary Care Provider: " + obj.PCPName + ", Care Coordinator: " + obj.CareCoordinatorName + ", Care Giver: " + obj.CareGiverName + ", Care Manager: " + obj.CareManagerName;
                    var careTeamString = careTeamName + careTeamMembers;

                    list.Add(new NameValuePair(careTeamString, obj.CareTeamId));
                }
            }
            return getJSONofList(list);
        }
        public string GetCareTeams(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            DSCCM ds = new DALCCM().CCMCareTeamLookup();
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.CareTeams.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.CareTeams.TableName].Select("1=1", ds.CareTeams.ShortNameColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.CareTeams.ShortNameColumn.ColumnName].ToString(), dr[ds.CareTeams.CareTeamIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        public string HealthRiskAssessmentTemplateLookup(string IsActive, Int64 EnrollmentInfoId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            DSCCM ds = new DALCCM().CCMHealthRiskAssessmentLookup(EnrollmentInfoId);
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.HRATemplateLookup.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.HRATemplateLookup.TableName].Select("1=1", ds.HRATemplateLookup.ShortNameColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        var templateName = dr[ds.HRATemplateLookup.ShortNameColumn.ColumnName].ToString() + "~_~";
                        var templateScore = dr[ds.HRATemplateLookup.RiskScoreColumn.ColumnName].ToString();
                        var callerString = templateName + templateScore;

                        list.Add(new NameValuePair(callerString, dr[ds.HRATemplateLookup.TemplateIdColumn.ColumnName].ToString()));
                    }
                }
            }

            return getJSONofList(list);
        }
        public string GetHRAssessmentTempt(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<List<MDVision.Model.CCM.CCMHub.HRAssessmentTemptModel>> objPharmacy = new BLLCCM().loadHRAssessmentTemplates();
            List<MDVision.Model.CCM.CCMHub.HRAssessmentTemptModel> modellist = objPharmacy.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (modellist != null && modellist.Count > 0)
            {
                foreach (MDVision.Model.CCM.CCMHub.HRAssessmentTemptModel obj in modellist)
                {
                    list.Add(new NameValuePair(obj.TemplateName, obj.TemplateId.ToString()));
                }
            }
            return getJSONofList(list);
        }
        public string GetEnrollmentInfoProgram(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<List<MDVision.Model.CCM.EnrollmentInfoProgram>> model = new BLLCCM().loadEnrollmentInfoPrograms();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));

            if (ds != null)
                foreach (var item in ds)
                    list.Add(new NameValuePair(item.ProgramName, item.ProgramId.ToString()));

            return getJSONofList(list);
        }
        public string TaskReasonLookup(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            DSCCM ds = new DALCCM().TaskReasonLookup();
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Reasons.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Reasons.TableName].Select("1=1", ds.Reasons.ShortNameColumn.ColumnName);
                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Reasons.ShortNameColumn.ColumnName].ToString(), dr[ds.Reasons.ReasonIdColumn.ColumnName].ToString()));
                    }
                }
            }
            return getJSONofList(list);
        }

        #endregion

        #region "Patient Custom Form Lookup"
        public string GetCustomFormName(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLPatient().LookupCustomFormName();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.CustomFormName, item.CustomFormId));
                    }
                }

            }
            return getJSONofList(list);
        }
        #endregion

        #region "Custom Forms and Letters for Fax"
        public string GetCustomFormLetters(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLAdminProfile().GetCustomFormLetters();

            var ds = model.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.CustomFormName, item.CustomFormId, item.Title));
                    }
                }

            }
            return getJSONofList(list);
        }
        #endregion


        #region "Outgoing Referrals LookUp"
        public string GetOutgoingReferrals(string IsActive, Int64 noteId, Int64 patientId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLAdminProfile().GetOutgoingReferrals(noteId, patientId);

            var ds = model.Data;
            //list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.ProviderName, item.RowNumber, item.FaxNumber));
                    }
                }

            }
            return getJSONofList(list);
        }
        public string GetPatientReferrals(string IsActive, Int64 patientId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLAdminProfile().GetPatientReferrals(patientId);

            var ds = model.Data;
            //list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.ProviderName, item.RowNumber, item.FaxNumber));
                    }
                }

            }
            return getJSONofList(list);
        }
        #endregion

        #region Clinical Care Plan
        public string GetCarePlanGoals(string IsActive, long CarePlanId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            List<CarePlanGoalsModel> goals = new BLLClinical().GetGoalsLookup(CarePlanId);
            list.Add(new NameValuePair("- Select -", ""));
            if (goals != null && goals.Count > 0)
            {
                string goalName = "";
                foreach (CarePlanGoalsModel goal in goals)
                {
                    goalName = "";
                    if (!string.IsNullOrEmpty(goal.GoalComments))
                    {
                        var comments = goal.GoalComments.Split().Take(4);
                        string goalComment = string.Join(" ", comments);
                        goalName = goal.GoalCode + " - " + goalComment;
                    }
                    else
                    {
                        goalName = goal.GoalCode + " - " + goal.GoalDescription;
                    }
                    list.Add(new NameValuePair(goalName, goal.GoalId));
                }
            }
            return getJSONofList(list);
        }
        public string GetMedicationsLookup(string IsActive, long PatientId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            List<ClinicalMedicationsModel> medications = new BLLClinical().GetMedicationsLookup(PatientId);
            list.Add(new NameValuePair("- Select -", ""));
            if (medications != null && medications.Count > 0)
            {
                foreach (ClinicalMedicationsModel med in medications)
                {
                    list.Add(new NameValuePair(med.MedicationName, med.MedicationID));
                }
            }
            return getJSONofList(list);
        }
        public string GetCarePlanInterventions(string IsActive, long CarePlanId)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            List<CarePlanInterventionsModel> inteventions = new BLLClinical().GetInterventionLookup(CarePlanId);
            list.Add(new NameValuePair("- Select -", ""));
            if (inteventions != null && inteventions.Count > 0)
            {
                foreach (CarePlanInterventionsModel intervention in inteventions)
                {
                    list.Add(new NameValuePair(intervention.InterventionName, intervention.InterventionId));
                }
            }
            return getJSONofList(list);
        }
        #endregion
    }
}