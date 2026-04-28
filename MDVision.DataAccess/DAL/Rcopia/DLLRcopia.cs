using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Rcopia
{
    public class DLLRcopia
    {
        #region Variable
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_INSERT_LASTUPDATEPATIENTINFO = "Clinical.sp_Rcopia_PatientLastUpdateInfoInsert";
        private const string PROC_SELECT_LASTUPDATEPATIENTINFO = "Clinical.sp_Rcopia_PatientLastUpdateInfoSelect";
        private const string PROC_SELECT_LASTUPDATEPATIENTINFO_Op = "Clinical.sp_DrFirst_Rcopia_PatientLastUpdateInfoSelect";
        private const string PROC_SELECT_GETURLS = "Clinical.sp_Rcopia_GetUrlSelect";
        private const string PROC_SELECT_SOFTWARECUSTOMERINFO = "System.sp_RCopiaSoftwareCustomerInfoSelect";
        private const string PROC_UPDATE_GETURLS_FOR_LIMP = "Clinical.sp_Rcopia_GetUrlUpdateForLIMP";
        private const string PROC_RCOPIA_GETURLUPDATE = "Clinical.sp_Rcopia_GetUrlUpdate";
        private const string PROC_IS_PATIENT_REGISTERED_ON_DRFIRST = "Patient.sp_IsPatientRegisteredOnDrFirst";
        private const string PROC_GET_RCOPIA_USER_NAME_OF_USER = "Clinical.sp_GetRcopiaUserNameofUser";
        private const string PROC_SELECT_PATIENTS_FOR_RCOPIA_REGISTERATION = "Clinical.GetPatientsForRcopiaRegisteration";
        private const string PROC_IS_USER_HAVE_RCOPIA_RIGHTS = "Patient.sp_IsCheckUserHaveRcopiaRights";
        private const string PROC_UPDATE_PATIENT_RCOPIA = "Patient.InsertPatientsRcopialID";
        private const string PROC_UPDATE_PROBLEM_RCOPIA = "Clinical.InsertProblemsRcopialID";
        private const string PROC_SELECT_PROBLEM_FOR_RCOPIA_REGISTERATION = "Clinical.GetProblemsForRcopiaRegisteration";
        private const string PROC_INSERT_DRFIRST_SRV_RESPONSE = "Clinical.sp_DrFirstSrvResponseInsert";
        private const string PROC_UPDATE_RCOPIA_GETURL = "Clinical.sp_UpdateRcopiaGetUrl";
        private const string PROC_GET_PROVIDER_RCOPIA_USER_NAME = "Clinical.sp_GetProviderRcopiaUserName";
        
        
        

        

        #endregion

        #region "Parameters"


        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PATIENT_IDS = "@PatientIds";
        private const string PARM_RCOPIA_ID = "@RcopiaID";
        private const string PARM_PROBLEM_ID = "@ProblemListId";

         private const string PARM_UPLOAD_URL = "@UploadUrl";
         private const string PARM_DOWNLOAD_URL = "@DownLoadUrl";
         private const string PARM_WEB_BROWZER_URL = "@WebBrowzerUrl";
        
        
        
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_AllergyLastUpdateDate = "@AllergyLastUpdateDate";
        private const string PARM_MedicationLastUpdateDate = "@MedicationLastUpdateDate";
        private const string PARM_PrescriptionLastUpdateDate = "@PrescriptionLastUpdateDate";
        private const string PARM_R_PLASTUPDATEINFO = "@R_PLastUpdateInfo";
        private const string PARM_MODIFY_ON = "@ModifiedOn";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_CUSTOMER_REGCODE = "@CustomerRegCode";
        private const string PARM_MEDICATION_LAST_UPDATE_FOR_LIMP = "@MedicationLastUpdateDateForLIMP";
        private const string PARM_PRESCRIPTION_LAST_UPDATE_FOR_LIMP = "@PrescriptionLastUpdateDateForLIMP";

        private const string PARM_URLID = "@UrlID";
        private const string PARM_WEBBROWSERURL = "@WebBrowserURL";
        private const string PARM_ENGINERUPLOADURL = "@EngineUploadURL";
        private const string PARM_ENGINEDOWNLOADURL = "@EngineDownloadURL";
        private const string PARM_ISACTIVE = "@IsActive";
        private const string PARM_CREATEDBY = "@CreatedBy";
        private const string PARM_CREATEDON = "@CreatedOn";
        private const string PARM_MODIFIEDBY = "@ModifiedBy";
        private const string PARM_MODIFIEDON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_USERNAME = "@UserName";
        private const string PARM_REQUEST_TYPE = "@RequestType";
        private const string PARM_RESPONSE = "@Response";
        private const string PARM_STATUS = "@Status";
        private const string PARM_WHICH_ATTR_UPDATE = "@WhichAttrUpdate";
        private const string PARM_NOTES_ID = "@NotesId";
        private const string PARM_ONLY_INSERT = "@OnlyInsert";
        
        #endregion

        #region Constructors
        public DLLRcopia()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }

        public DLLRcopia(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);

        }
        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region "Support Functions For Rcopia"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">Is insert ?</param>

        private void CreateParametersForUpdateLastUpdatePatientInfo(IDBManager dbManager, DSRcopia ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_R_PLASTUPDATEINFO, ds.Rcopia_PatientLastUpdateInfo.R_PLastUpdateInfoColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_R_PLASTUPDATEINFO, ds.Rcopia_PatientLastUpdateInfo.R_PLastUpdateInfoColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.Rcopia_PatientLastUpdateInfo.PatientIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_AllergyLastUpdateDate, ds.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.Rcopia_PatientLastUpdateInfo.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_MODIFIED_BY, ds.Rcopia_PatientLastUpdateInfo.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.Rcopia_PatientLastUpdateInfo.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.Rcopia_PatientLastUpdateInfo.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFY_ON, ds.Rcopia_PatientLastUpdateInfo.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MedicationLastUpdateDate, ds.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_PrescriptionLastUpdateDate, ds.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_WHICH_ATTR_UPDATE, ds.Rcopia_PatientLastUpdateInfo.WhichAttrUpdateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_ONLY_INSERT, ds.Rcopia_PatientLastUpdateInfo.OnlyInsertColumn.ColumnName, DbType.Byte);
            

        }


        private void CreateParametersForUpDateGetUrl(IDBManager dbManager, DSRcopia ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_URLID, ds.Rcopia_GetUrl.UrlIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_URLID, ds.Rcopia_GetUrl.UrlIDColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_WEBBROWSERURL, ds.Rcopia_GetUrl.WebBrowserURLColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ENGINERUPLOADURL, ds.Rcopia_GetUrl.EngineUploadURLColumn.ColumnName, DbType.String);

            dbManager.AddParameters(3, PARM_ENGINEDOWNLOADURL, ds.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ISACTIVE, ds.Rcopia_GetUrl.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_MODIFIEDBY, ds.Rcopia_GetUrl.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATEDBY, ds.Rcopia_GetUrl.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATEDON, ds.Rcopia_GetUrl.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIEDON, ds.Rcopia_GetUrl.ModifiedOnColumn.ColumnName, DbType.DateTime);



        }

        private void CreateParametersForSelectLastUpdatePatientInfo(IDBManager dbManager, DSRcopia ds)
        {
            dbManager.CreateParameters(1);
            dbManager.AddParameters(0, PARM_PATIENT_ID, ds.Rcopia_PatientLastUpdateInfo.PatientIdColumn.ColumnName, DbType.Int64);
        }

        private void CreateParametersForUpdateMedicationAndPrescriptionLastUpdateDateInLIMP(IDBManager dbManager, DSRcopia ds)
        {
            dbManager.CreateParameters(2);

            dbManager.AddParameters(0, PARM_MEDICATION_LAST_UPDATE_FOR_LIMP, ds.Rcopia_GetUrl.MedicationLastUpdateDateForLIMPColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(1, PARM_PRESCRIPTION_LAST_UPDATE_FOR_LIMP, ds.Rcopia_GetUrl.PrescriptionLastUpdateDateForLIMPColumn.ColumnName, DbType.DateTime);

        }
        #endregion

        #region Common Function

        public DSRcopia UpdatePtientLastUpdateDate(DSRcopia ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForUpdateLastUpdatePatientInfo(dbManager, ds, true);
                ds = (DSRcopia)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSERT_LASTUPDATEPATIENTINFO, ds, ds.Rcopia_PatientLastUpdateInfo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DLLRcopia::UpdateLastUpdatePatientInfo", PROC_INSERT_LASTUPDATEPATIENTINFO, ex);
                MDVLogger.SendExcepToDB(ex, "DLLRcopia::UpdateLastUpdatePatientInfo", null);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRcopia UpdateMedicationAndPrescriptionLastUpdateDateForLIMP(DSRcopia ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForUpdateMedicationAndPrescriptionLastUpdateDateInLIMP(dbManager, ds);
                ds = (DSRcopia)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_UPDATE_GETURLS_FOR_LIMP, ds, ds.Rcopia_GetUrl.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLRcopia::UpdateMedicationAndPrescriptionLastUpdateDateForLIMP", PROC_UPDATE_GETURLS_FOR_LIMP, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //UpDateGetUrl
        public DSRcopia UpDateGetUrl(DSRcopia ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForUpDateGetUrl(dbManager, ds, false);
                ds = (DSRcopia)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_RCOPIA_GETURLUPDATE, ds, ds.Rcopia_GetUrl.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DLLRcopia::PROC RCOPIA GETURLUPDATE", PROC_RCOPIA_GETURLUPDATE, ex);
                MDVLogger.SendExcepToDB(ex, "DLLRcopia::PROC RCOPIA GETURLUPDATE", null);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRcopia SelectPatientLastUpdateInfo(DSRcopia ds)
        {

            DSRcopia returnDataSet = new DSRcopia();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);


                if (ds.Rcopia_PatientLastUpdateInfo.Rows[0]["PatientId"] == null)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, ds.Rcopia_PatientLastUpdateInfo.Rows[0]["PatientId"]);

                if (ds.Rcopia_PatientLastUpdateInfo.Rows[0]["IsActive"] == null)
                    dbManager.AddParameters(1, PARM_ISACTIVE, null);
                else
                    dbManager.AddParameters(1, PARM_ISACTIVE, ds.Rcopia_PatientLastUpdateInfo.Rows[0]["IsActive"]);

                if (ds.Rcopia_PatientLastUpdateInfo.Rows[0]["ModifiedBy"] == null)
                    dbManager.AddParameters(2, PARM_MODIFIEDBY, null);
                else
                    dbManager.AddParameters(2, PARM_MODIFIEDBY, ds.Rcopia_PatientLastUpdateInfo.Rows[0]["ModifiedBy"]);

                if (ds.Rcopia_PatientLastUpdateInfo.Rows[0]["CreatedBy"] == null)
                    dbManager.AddParameters(3, PARM_CREATEDBY, null);
                else
                    dbManager.AddParameters(3, PARM_CREATEDBY, ds.Rcopia_PatientLastUpdateInfo.Rows[0]["CreatedBy"]);

                if (ds.Rcopia_PatientLastUpdateInfo.Rows[0]["CreatedOn"] == null)
                    dbManager.AddParameters(4, PARM_CREATEDON, null);
                else
                    dbManager.AddParameters(4, PARM_CREATEDON, ds.Rcopia_PatientLastUpdateInfo.Rows[0]["CreatedOn"]);

                if (ds.Rcopia_PatientLastUpdateInfo.Rows[0]["ModifiedOn"] == null)
                    dbManager.AddParameters(5, PARM_MODIFIEDON, null);
                else
                    dbManager.AddParameters(5, PARM_MODIFIEDON, ds.Rcopia_PatientLastUpdateInfo.Rows[0]["ModifiedOn"]);

                returnDataSet = (DSRcopia)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SELECT_LASTUPDATEPATIENTINFO, returnDataSet, returnDataSet.Rcopia_PatientLastUpdateInfo.TableName);
                return returnDataSet;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::GetPatientLastUpdateDates", PROC_SELECT_LASTUPDATEPATIENTINFO, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRcopia SelectPatientLastUpdateInfoOp(long PatientID)
        {

            DSRcopia returnDataSet = new DSRcopia();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientID);
                returnDataSet = (DSRcopia)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SELECT_LASTUPDATEPATIENTINFO_Op, returnDataSet, returnDataSet.Rcopia_PatientLastUpdateInfo.TableName);
                return returnDataSet;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALProblemLists::GetPatientLastUpdateDates", PROC_SELECT_LASTUPDATEPATIENTINFO_Op, ex);
                MDVLogger.SendExcepToDB(ex, "DALProblemLists::GetPatientLastUpdateDates", null);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRcopia SelectGetUrls()
        {
            DSRcopia GetUrlData = new DSRcopia();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                GetUrlData = (DSRcopia)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SELECT_GETURLS, GetUrlData, GetUrlData.Rcopia_GetUrl.TableName);
                GetUrlData.AcceptChanges();
                return GetUrlData;
            }
            catch (Exception ex)
            {
               // MDVLogger.DALErrorLog("DALProblemLists::GetPatientLastUpdateDates", PROC_SELECT_LASTUPDATEPATIENTINFO, ex);
                MDVLogger.SendExcepToDB(ex, "DALProblemLists::GetPatientLastUpdateDates", null);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRcopia SelectGetUrls(SharedVariable SharedVariable)
        {
            DSRcopia GetUrlData = new DSRcopia();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                GetUrlData = (DSRcopia)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SELECT_GETURLS, GetUrlData, GetUrlData.Rcopia_GetUrl.TableName);
                GetUrlData.AcceptChanges();
                return GetUrlData;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog(SharedVariable, "DALProblemLists::GetPatientLastUpdateDates", PROC_SELECT_LASTUPDATEPATIENTINFO, ex);
                MDVLogger.SendExcepToDB(ex, "DALProblemLists::GetPatientLastUpdateDates", null);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function to Select Software Customer Info.
        /// Date : 20 january 2016
        /// </summary>
        /// <returns></returns>
        public DSRcopia SelectSoftwareCustomerInfo(string customerRegCode)
        {
            DSRcopia ds = new DSRcopia();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (string.IsNullOrEmpty(customerRegCode))
                    dbManager.AddParameters(0, PARM_CUSTOMER_REGCODE, null);
                else
                    dbManager.AddParameters(0, PARM_CUSTOMER_REGCODE, customerRegCode);
                ds = (DSRcopia)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SELECT_SOFTWARECUSTOMERINFO, ds, ds.SoftwareCustomersInfo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DLLRcopia::SelectSoftwareCustomerInfo", PROC_SELECT_SOFTWARECUSTOMERINFO, ex);
                MDVLogger.SendExcepToDB(ex, "DLLRcopia::SelectSoftwareCustomerInfo", null);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRcopia SelectSoftwareCustomerInfo(SharedVariable SharedVariable, string customerRegCode)
        {
            DSRcopia ds = new DSRcopia();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                if (string.IsNullOrEmpty(customerRegCode))
                    dbManager.AddParameters(0, PARM_CUSTOMER_REGCODE, null);
                else
                    dbManager.AddParameters(0, PARM_CUSTOMER_REGCODE, customerRegCode);
                ds = (DSRcopia)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SELECT_SOFTWARECUSTOMERINFO, ds, ds.SoftwareCustomersInfo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DLLRcopia::SelectSoftwareCustomerInfo", PROC_SELECT_SOFTWARECUSTOMERINFO, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string IsPatientRegisteredOnDrFirs(Int64 PatientId)
        {
            string returnVal = "yes";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IS_PATIENT_REGISTERED_ON_DRFIRST).ToString();

                if (returnVal == "")
                    throw new Exception(returnVal);
                else
                {

                }
                return returnVal;
            }
            catch (Exception ex)
            {

                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DLLRcopia::IsPatientRegisteredOnDrFirs", PROC_IS_PATIENT_REGISTERED_ON_DRFIRST, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }


        }



        public string GetRcopiaUserName()
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, PARM_USERNAME, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_RCOPIA_USER_NAME_OF_USER).ToString();

                if (returnVal == "")
                    throw new Exception(returnVal);
                else
                {

                }
                return returnVal;
            }
            catch (Exception ex)
            {

                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DLLRcopia::GetRcopiaUserName", PROC_GET_RCOPIA_USER_NAME_OF_USER, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }


        }


        public string GetProviderRcopiaUserName(long NotesId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_NOTES_ID, NotesId);
                dbManager.AddParameters(1, PARM_USERNAME, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_PROVIDER_RCOPIA_USER_NAME).ToString();

                if (returnVal == "")
                    throw new Exception(returnVal);
                else
                {

                }
                return returnVal;
            }
            catch (Exception ex)
            {

                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DLLRcopia::GetProviderRcopiaUserName", PROC_GET_PROVIDER_RCOPIA_USER_NAME, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }


        }
        

        public string CheckUserHaveRcopiaRights(Int64 UserId)
        {
            string returnVal = "yes";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ID, UserId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IS_USER_HAVE_RCOPIA_RIGHTS).ToString();

                if (returnVal == "")
                    throw new Exception(returnVal);
                else
                {

                }
                return returnVal;
            }
            catch (Exception ex)
            {

                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DLLRcopia::CheckUserHaveRcopiaRights", PROC_IS_USER_HAVE_RCOPIA_RIGHTS, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }


        }




        public DSRcopia GetPatientsForDrfirstRegisteration(SharedVariable SharedVariable)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSRcopia ds = new DSRcopia();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {


                DataTable dtTemp = ds.Patients;
                dbManager.Open();
                ds = (DSRcopia)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SELECT_PATIENTS_FOR_RCOPIA_REGISTERATION, ds, ds.Patients.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog(SharedVariable, "DALRcopia::GetPatientsForDrfirstRegisteration", PROC_SELECT_PATIENTS_FOR_RCOPIA_REGISTERATION, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRcopia SaveResponse(SharedVariable SharedVariable, string RequestType, string Response, string status)
        {
            DSRcopia ds = new DSRcopia();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                DSRcopia.DrFirstSrvResponseRow dr = ds.DrFirstSrvResponse.NewDrFirstSrvResponseRow();
                dr.RequestType = RequestType;
                dr.Response = Response;
                dr.status = status;
                ds.DrFirstSrvResponse.AddDrFirstSrvResponseRow(dr);
                dbManager.AddParameters(0, "@Id", ds.DrFirstSrvResponse.IdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_REQUEST_TYPE, ds.DrFirstSrvResponse.RequestTypeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_RESPONSE, ds.DrFirstSrvResponse.ResponseColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, PARM_STATUS, ds.DrFirstSrvResponse.statusColumn.ColumnName, DbType.String);
                ds = (DSRcopia)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSERT_DRFIRST_SRV_RESPONSE, ds, ds.DrFirstSrvResponse.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog(SharedVariable,"DALRcopia::SaveResponse", PROC_INSERT_DRFIRST_SRV_RESPONSE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSRcopia UpdatePatientRcopia(SharedVariable SharedVariable,DSRcopia ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, ds.Patients.PatientIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_RCOPIA_ID, ds.Patients.RcopiaIdColumn.ColumnName, DbType.String);
                ds = (DSRcopia)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_UPDATE_PATIENT_RCOPIA, ds, ds.Patients.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog(SharedVariable, "DALRcopia::UpdatePatientRcopia", PROC_UPDATE_PATIENT_RCOPIA, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRcopia UpdateRcopiaGetUrl(SharedVariable SharedVariable, DSRcopia ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_UPLOAD_URL, ds.Rcopia_GetUrl.EngineUploadURLColumn.ColumnName, DbType.String);
                dbManager.AddParameters(1, PARM_DOWNLOAD_URL, ds.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_WEB_BROWZER_URL, ds.Rcopia_GetUrl.WebBrowserURLColumn.ColumnName, DbType.String);
                ds = (DSRcopia)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_UPDATE_RCOPIA_GETURL, ds, ds.Rcopia_GetUrl.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog(SharedVariable, "DALRcopia::UpdateRcopiaGetUrl", PROC_UPDATE_RCOPIA_GETURL, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        

        public DSRcopia GetProblemsForAddOnDrfirst(SharedVariable SharedVariable,string PatientIds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSRcopia ds = new DSRcopia();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {


                DataTable dtTemp = ds.ProblemList;
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PATIENT_IDS, PatientIds);
                ds = (DSRcopia)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SELECT_PROBLEM_FOR_RCOPIA_REGISTERATION, ds, ds.ProblemList.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog(SharedVariable, "DALRcopia::GetProblemsForAddOnDrfirst", PROC_SELECT_PROBLEM_FOR_RCOPIA_REGISTERATION, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSRcopia UpdateProblemRcopia(SharedVariable SharedVariable,DSRcopia ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROBLEM_ID, ds.ProblemList.ProblemListIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_RCOPIA_ID, ds.ProblemList.RcopiaIdColumn.ColumnName, DbType.String);
                ds = (DSRcopia)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_UPDATE_PROBLEM_RCOPIA, ds, ds.ProblemList.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog(SharedVariable, "DALRcopia::UpdateProblemRcopia", PROC_UPDATE_PROBLEM_RCOPIA, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
    }
}
