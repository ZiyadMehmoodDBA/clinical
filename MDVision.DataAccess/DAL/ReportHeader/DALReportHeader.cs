using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Data;

using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.ComponentModel;
using MDVision.Datasets;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.ReportHeader
{


    public class DALReportHeader
    {

        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        //[Clinical].[sp_HeaderTemplatePopulateSpeciality]
        //[Clinical].[sp_HeaderTemplatePopulateProviders] 
        //[Clinical].[sp_HeaderTemplatePopulateFacility]

        private const string PROC_REPORTHEADER_SEARCH = "Clinical.sp_ReportHeaderSearch";
        private const string PROC_REPORTHEADER_SELECT = "Clinical.sp_ReportHeaderSelect";
        private const string PROC_REPORTHEADER_INSERT = "Clinical.sp_ReportHeaderInsert";
        private const string PROC_REPORTHEADER_UPDATE = "Clinical.sp_ReportHeaderUpdate";
        private const string PROC_REPORTHEADER_DELETE = "Clinical.sp_ReportHeaderDelete";
        private const string PROC_REPORTHEADER_ACTIVE_INACTIVE = "Clinical.sp_ReportHeaderActiveInactive";
        private const string PROC_REPORTHEADER_SELECT_Lookup = "Clinical.sp_ReportHeaderLookup";
        private const string PROC_REPORTHEADER_CONFIGURATION_SELECT = "Clinical.sp_RptHeaderConfigurationSelect";
        private const string PROC_REPORTHEADER_SETTINGS_INSERT = "Clinical.sp_RptHeaderSettingsInsert";
        private const string PROC_REPORTHEADER_SETTINGS_SELECT = "Clinical.sp_RptHeaderSettingsSelect";
        private const string PROC_REPORTHEADER_SETTINGS_UPDATE = "Clinical.sp_RptHeaderSettingsUpdate";

        private const string PROC__REPORTHEADER_TAGS_VALUES_SELECT = "Clinical.sp_RptHdrTagsValuesSelect";

        #endregion

        #region "Parameters"

        private const string PARM_SPECIALITY_IDS = "@SpecialtyIds";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_FACILITY_IDS = "@FacilityIds";

        private const string PARM_REPORT_HEADER_ID = "@ReportHeaderId";
        
        private const string PARM_REPORT_HEADER_NAME = "@Name";
        private const string PARM_GENERATED_BY = "@GeneratedBy";
        private const string PARM_HEADER_LOGO = "@HeaderLogo";
        private const string PARM_HEADER_LOGO_NAME = "@HeaderLogoName";
        private const string PARM_HEADER_LOGO_UPLD_DATE = "@HeaderLogoUpldDate";

        private const string PARM_PROVIDER_TAG = "@ProviderTag";
        private const string PARM_PATIENT_TAG = "@PatientTag";
        private const string PARM_PRACTICE_TAG = "@PracticeTag";
        private const string PARM_NOTES_ID = "@NotesId";
        private const string PARM_FORM_NAME = "@FormName";
        private const string PARM_PREVIEW_STYLE = "@PreviewStyle";
        //  private const string PARM_SPECIALTY_ID = "@SpecialtyId";


        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ENTITY_IDS = "@EntityIds";


        private const string PARM_ERROR_ReportHeader = "@ErrorReportHeader";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_ERROR_MESSAGE = "@errormessage";

        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_REPORT_HEADER_SETTINGS_ID = "@RptHeaderSettingsId";
        private const string PARM_REPORT_HEADER_CONFIGURATION_ID = "@RptHeaderConfigurationId";
       
        #endregion

        #region Constructors
        public DALReportHeader()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
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

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSReportHeader ds, Boolean IsInsert)
        {

            if (IsInsert == true)
            {
                dbManager.CreateParameters(18);
                dbManager.AddParameters(0, PARM_REPORT_HEADER_ID, ds.ReportHeader.ReportHeaderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(16);
                dbManager.AddParameters(0, PARM_REPORT_HEADER_ID, ds.ReportHeader.ReportHeaderIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddParameters(1, PARM_REPORT_HEADER_NAME, ds.ReportHeader.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_GENERATED_BY, ds.ReportHeader.GeneratedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_HEADER_LOGO, ds.ReportHeader.HeaderLogoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_HEADER_LOGO_NAME, ds.ReportHeader.HeaderLogoNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_HEADER_LOGO_UPLD_DATE, ds.ReportHeader.HeaderLogoUpldDateColumn.ColumnName, DbType.String);

            dbManager.AddParameters(6, PARM_PRACTICE_TAG, ds.ReportHeader.PracticeTagColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_PATIENT_TAG, ds.ReportHeader.PatientTagColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PROVIDER_TAG, ds.ReportHeader.ProviderTagColumn.ColumnName, DbType.String);
            
            dbManager.AddParameters(9, PARM_PROVIDER_IDS, ds.ReportHeader.ProviderIdsColumn.ColumnName, DbType.String);

            dbManager.AddParameters(10, PARM_FACILITY_IDS, ds.ReportHeader.FacilityIdsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_SPECIALITY_IDS, ds.ReportHeader.SpecialtyIdsColumn.ColumnName, DbType.String);

            dbManager.AddParameters(12, PARM_IS_ACTIVE, ds.ReportHeader.IsActiveColumn.ColumnName, DbType.Byte);
            if (IsInsert == true)
            {
                dbManager.AddParameters(13, PARM_CREATED_BY, ds.ReportHeader.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(14, PARM_CREATED_ON, ds.ReportHeader.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(15, PARM_MODIFIED_BY, ds.ReportHeader.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(16, PARM_MODIFIED_ON, ds.ReportHeader.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(17, PARM_ENTITY_IDS, ds.ReportHeader.EntityIdsColumn.ColumnName, DbType.String);
            }
            else
            {                
                dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.ReportHeader.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.ReportHeader.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(15, PARM_ENTITY_IDS, ds.ReportHeader.EntityIdsColumn.ColumnName, DbType.String);
            }           
        }

        private void CreateSettingsParameters(IDBManager dbManager, DSReportHeader ds)
        {         
            dbManager.CreateParameters(4);
            dbManager.AddParameters(0, PARM_REPORT_HEADER_SETTINGS_ID, ds.RptHeaderSettings.RptHeaderSettingsIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_REPORT_HEADER_ID, ds.RptHeaderSettings.ReportHeaderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.RptHeaderSettings.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(3, PARM_REPORT_HEADER_CONFIGURATION_ID, ds.RptHeaderSettings.RptHeaderConfigurationIdColumn.ColumnName, DbType.Int64);          
        }
        #endregion

        #region "Support Functions For Report Header"

        /// <summary>
        /// Loads the patient.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="FirstName">The first name.</param>
        /// <param name="LastName">The last name.</param>
        /// <param name="AccountNumber">The account number.</param>
        /// <param name="SSN">The SSN.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSReportHeader LoadReportHeader(string ReportHeaderName, string SpecialtyIds, string ProviderIds, string FacilityIds, Int32 PageNumber, Int32 RowsPerPage, string IsActive)
        {
            DSReportHeader ds = new DSReportHeader();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                if (string.IsNullOrEmpty(ReportHeaderName))
                    dbManager.AddParameters(0, PARM_REPORT_HEADER_NAME, null);
                else
                    dbManager.AddParameters(0, PARM_REPORT_HEADER_NAME, string.IsNullOrEmpty(ReportHeaderName) ? null : ReportHeaderName);
                dbManager.AddParameters(1, PARM_SPECIALITY_IDS, string.IsNullOrEmpty(SpecialtyIds) ? null : SpecialtyIds);
                dbManager.AddParameters(2, PARM_PROVIDER_IDS, string.IsNullOrEmpty(ProviderIds) ? null : ProviderIds);
                dbManager.AddParameters(3, PARM_FACILITY_IDS, string.IsNullOrEmpty(FacilityIds) ? null : FacilityIds);
                dbManager.AddParameters(4, PARM_IS_ACTIVE, string.IsNullOrEmpty(IsActive) ? null : IsActive);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.ReportHeaderList.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSReportHeader)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTHEADER_SEARCH, ds, ds.ReportHeaderList.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReportHeader::LoadReportHeader", PROC_REPORTHEADER_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReportHeader FillReportHeader(long ReportHeaderId)
        {
            DSReportHeader ds = new DSReportHeader();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(1);

                if (ReportHeaderId <= 0)
                    dbManager.AddParameters(0, PARM_REPORT_HEADER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REPORT_HEADER_ID, ReportHeaderId);

                ds = (DSReportHeader)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTHEADER_SELECT, ds, ds.ReportHeader.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReportHeader::FillReportHeader", PROC_REPORTHEADER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Updates the ReportHeader.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSReportHeader UpdateReportHeader(DSReportHeader ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSReportHeader)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_REPORTHEADER_UPDATE, ds, ds.ReportHeader.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReportHeader::UpdateReportHeader", PROC_REPORTHEADER_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the ReportHeader.
        /// </summary>
        /// <param name="PatMsgId">The ReportHeader identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string UpdateReportHeaderActiveInactive(Int64 reportHeaderId, bool IsActive)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REPORT_HEADER_ID, reportHeaderId);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);
               dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_REPORTHEADER_ACTIVE_INACTIVE);

               return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReportHeader::UpdateReportHeaderActiveInactive", PROC_REPORTHEADER_ACTIVE_INACTIVE, ex);
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

        /// <summary>
        /// Deletes the ReportHeader.
        /// </summary>
        /// <param name="PatMsgId">The ReportHeader identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteReportHeader(Int64 reportHeaderId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REPORT_HEADER_ID, reportHeaderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REPORTHEADER_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReportHeader::DeleteReportHeader", PROC_REPORTHEADER_DELETE, ex);
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

        /// <summary>
        /// Inserts the ReportHeader.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSReportHeader InsertReportHeader(DSReportHeader ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSReportHeader)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_REPORTHEADER_INSERT, ds, ds.ReportHeader.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReportHeader::InsertReportHeader", PROC_REPORTHEADER_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReportHeader LoadReportHeaderConfiguration()
        {
            DSReportHeader ds = new DSReportHeader();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSReportHeader)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTHEADER_CONFIGURATION_SELECT, ds, ds.RptHeaderConfiguration.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReportHeader::LoadReportHeaderConfiguration", PROC_REPORTHEADER_CONFIGURATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSReportHeader UpdateReportHeaderSettings(DSReportHeader ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {

                dbManager.Open();                
                this.CreateSettingsParameters(dbManager, ds);

                ds = (DSReportHeader)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_REPORTHEADER_SETTINGS_UPDATE, ds, ds.RptHeaderSettings.TableName);

                ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReportHeader::InsertUpdateReportHeaderSettings", PROC_REPORTHEADER_SETTINGS_INSERT + " " + PROC_REPORTHEADER_SETTINGS_UPDATE, ex);
                throw ex;
            }
        }

        //public DSReportHeader InsertUpdateReportHeaderSettings(DSReportHeader ds)
        //{
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
          
        //    try
        //    {

        //        dbManager.Open();
            
        //        this.CreateSettingsParameters(dbManager, ds, true);
        //        this.CreateSettingsParameters(dbManager, ds, false);

        //        ds = (DSReportHeader)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_REPORTHEADER_SETTINGS_INSERT, PROC_REPORTHEADER_SETTINGS_UPDATE, ds, ds.RptHeaderSettings.TableName);
               
        //        ds.AcceptChanges();
              
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALReportHeader::InsertUpdateReportHeaderSettings", PROC_REPORTHEADER_SETTINGS_INSERT + " " + PROC_REPORTHEADER_SETTINGS_UPDATE, ex);
        //        throw ex;
        //    }
        //}

        public DSReportHeader LoadReportHeaderSettings(long reportHeaderId)
        {
            DSReportHeader ds = new DSReportHeader();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_REPORT_HEADER_ID, reportHeaderId);

                ds = (DSReportHeader)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTHEADER_SETTINGS_SELECT, ds, ds.RptHeaderSettings.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReportHeader::LoadReportHeaderSettings", PROC_REPORTHEADER_SETTINGS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        
        #endregion


        #region "Report Header Text"


        public DSReportHeader getReportHeaderTagsValue(long PatientId, long ProviderId, long NotesId,string formName="", string PreviewStyle = null)
        {
            DSReportHeader ds = new DSReportHeader();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                if (ProviderId <= 0) 
                {
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                }
                if (PatientId <= 0) {
                    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                }
                if (NotesId <= 0)
                {
                    dbManager.AddParameters(2, PARM_NOTES_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_NOTES_ID, NotesId);
                }
                if (formName == "")
                {
                    dbManager.AddParameters(3, PARM_FORM_NAME, string.Empty);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_FORM_NAME, formName);
                }

                dbManager.AddParameters(4, PARM_PREVIEW_STYLE, PreviewStyle);

                ds = (DSReportHeader)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC__REPORTHEADER_TAGS_VALUES_SELECT, ds, ds.ReportHeaderTags.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReportHeader::getReportHeaderTagsValue", PROC__REPORTHEADER_TAGS_VALUES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion



        #region  " Lookups "

        public DSReportHeader LookupReportHeader(long providerId)
        {
            DSReportHeader ds = new DSReportHeader();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (providerId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, providerId);

                ds = (DSReportHeader)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REPORTHEADER_SELECT_Lookup, ds, ds.ReportHeader.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::getLabsLookup", PROC_REPORTHEADER_SELECT_Lookup, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //public DSReportHeader LookupSpeciality(string ProviderIds, string FacilityIds)
        //{
        //    DSReportHeader ds = new DSReportHeader();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(2);
               
        //        dbManager.AddParameters(0, PARM_PROVIDER_IDS, ProviderIds);
        //        dbManager.AddParameters(1, PARM_FACILITY_IDS, FacilityIds);

        //        ds = (DSReportHeader)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_POPULATE_SPECIALITY, ds, ds.LookupSpeciality.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DSReportHeader::LookupSpeciality", PROC_POPULATE_SPECIALITY, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}
        //public DSReportHeader LookupProvider(string SpecialityIds, string FacilityIds)
        //{
        //    DSReportHeader ds = new DSReportHeader();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(2);

        //        dbManager.AddParameters(0, PARM_SPECIALITY_IDS, SpecialityIds);
        //        dbManager.AddParameters(1, PARM_FACILITY_IDS, FacilityIds);

        //        ds = (DSReportHeader)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_POPULATE_PROVIDERS, ds, ds.LookupProvider.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DSReportHeader::LookupProvider", PROC_POPULATE_PROVIDERS, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}
        //public DSReportHeader LookupFacility(string SpecialityIds, string ProviderIds)
        //{
        //    DSReportHeader ds = new DSReportHeader();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(2);

        //        dbManager.AddParameters(0, PARM_SPECIALITY_IDS, SpecialityIds);
        //        dbManager.AddParameters(1, PARM_PROVIDER_IDS, ProviderIds);

        //        ds = (DSReportHeader)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_POPULATE_FACILITY, ds, ds.LookupFacility.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DSReportHeader::LookupFacility", PROC_POPULATE_FACILITY, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

       

        #endregion


    }


}
