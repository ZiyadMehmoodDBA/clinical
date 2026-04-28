using MDVision.Common.Logging;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Clinical.OrderSet
{
   
    public class DALOS_Immunization
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_OS_VACCINEHx_INSERT = "Clinical.sp_OS_VaccineHxInsert";
        private const string PROC_OS_VaccineHx_SELECT = "Clinical.sp_OS_VaccineHxSelectForGrid";
        private const string PROC_GET_AGELIM_SCHE_CATEG_AGAINST_VACCSHEDID = "Clinical.sp_GetAgelimScheCategAgainstVaccshedid";
        private const string PROC_OS_VaccineHx_SELECT_FOR_EDIT = "Clinical.sp_OS_VaccineHxSelect";
        private const string PROC_OS_VaccineHx_UPDATE = "Clinical.sp_OS_VaccineHxUpdate";
        private const string PROC_OS_VACCINEHX_DELETE = "Clinical.sp_OS_VaccineHxDelete";
        private const string PROC_IS_VACCINEHX_IN_VALID_AGE = "Clinical.VaccineHxIsInValidAge";
        private const string PROC_IS_VACCINEHX_LOT_ISSUE = "Clinical.IsVaccineHxLotIssue";

        private const string PROC_OS_Therapeutic_Injection_SELECT = "Clinical.sp_OS_ImmunizationTherapeuticInjectionSelect";
        private const string PROC_OS_THERAPEUTIC_INJECTION_INSERT = "Clinical.sp_OS_ImmunizationTherapeuticInjectionInsert";
        private const string PROC_OS_THERAPEUTIC_INJECTION_UPDATE = "Clinical.sp_OS_ImmunizationTherapeuticInjectionUpdate";
        private const string PROC_OS_THERAPEUTIC_INJECTION_DELETE = "Clinical.sp_OS_ImmunizationTherapeuticInjectionDelete";
        

        #endregion

        #region "Parameters"



        private const string PARAM_VACCINE_HX_ID = "@OS_VaccineHxId";
        private const string PARAM_VACCINE_GROUP_CATEGORY = "@OS_VaccineGroupCategory";
        private const string PARAM_VACCINE = "@OS_Vaccine";
        private const string PARAM_LOT_NUMBER = "@OS_LotNumberId";
        private const string PARAM_ROUTE = "@OS_Route";
        private const string PARAM_VFC = "@OS_VFC";
        private const string PARAM_COMMENTS = "@OS_Comments";
        private const string PARAM_DOSE = "@Dose";
        private const string PARAM_AMOUNT = "@Amount";
        private const string PARAM_SITE = "@Site";
        private const string PARAM_VISDATE = "@VISDate";
        private const string PARAM_GIVEN_BY = "@GivenBy";
        private const string PARAM_MANUFACTURER = "@Manufacturer";
        private const string PARAM_EXPIRY_DATE = "@ExpiryDate";
        private const string PARAM_SOURCE_OF_HX = "@SourceOfHx";
        private const string PARAM_TYPE = "@Type";
        private const string PARAM_IS_ACITVE = "@ISActive";
        private const string PARAM_CREATED_BY = "@CreatedBy";
        private const string PARAM_CREATED_ON = "@CreatedOn";
        private const string PARAM_MODIFIED_ON = "@ModifiedOn";
        private const string PARAM_MODIFIED_BY = "@ModifiedBy";
        private const string PARAM_PROVIDER_ID = "@ProviderId";
        private const string PARAM_REFUSAL_REASON_ID = "@RefusalReasonId";
        private const string PARAM_PUBLICITY_CODE = "@PublicityCode";
        private const string PARAM_PUBLICITY_CODE_EXPIRY_DATE = "@PublicityCodeExpiryDate";
        private const string PARAM_IMMUNIZATION_REGISTRY_STATUS_CODE = "@ImmunizationRegistryStatusCode";
        private const string PARAM_IRS_EFFECTIVE_DATE = "@IRSEffectiveDate";
        private const string PARAM_PROTECTION_INDICATOR = "@ProtectionIndicator";
        private const string PARAM_PI_EFFECTIVE_DATE = "@PIEffectiveDate";
        private const string PARAM_REACTION = "@ReactionId";
        private const string PARAM_VOID_DOES = "@VoidDose";
        private const string PARAM_OVERRIDE_RULE = "@OverrideRule";
        private const string PARAM_ORDERSET_ID = "@OrderSetId";
        private const string PARAM_MAIN_AGEGROUP = "@MainAgeGroup";
        private const string PARAM_MAIN_SCHEDULE = "@MainSchedule";
        private const string PARAM_MAIN_CATEGORY = "@MainCategory";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_VACCINESCHEDULE_ID = "@VaccineScheduleId";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_OS_VACCINE_HX_ID = "@OS_VaccinehxId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARAM_SITE_ID = "@SiteId";
        private const string PARAM_ROUTE_ID = "@RouteId";
        private const string PARAM_VACCINE_MANUFACTURER_ID = "@ManufacturerId";
        private const string PARAM_ADMINISTRATION_DATE = "@AdministrationDate";
        private const string PARAM_THERAPEUTIC_INJECTION_ID = "@TherapeuticInjectionId";
        private const string PARAM_VISIT_DATE = "@VisitDate";
        private const string PARAM_THERAPEUTIC_VFC = "@VFC";
        private const string PARAM_COMMENT = "@Comments";
        private const string PARAM_OS_IMM_THER_INJECTION_ID = "@OSImmTherInjectionId";
        private const string PARAM_LOT_NO = "@LotNumberId";
        private const string PARAM_NOTES_ID = "@NotesId";
        private const string PARAM_IMMUNIZATION_IDS = "@ImmunizationIds";
        
        
        #endregion

        #region Constructors
        public DALOS_Immunization()
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

        #region "Support Functions Problem Lists"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParametersForVaccineHx(IDBManager dbManager, DSOS_Immunization ds, Boolean IsInsert)
        {
            if (IsInsert)
            {
                dbManager.CreateParameters(36);
                dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, ds.OS_VaccineHx.OS_VaccineHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(30);
                dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, ds.OS_VaccineHx.OS_VaccineHxIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddParameters(1, PARAM_VACCINE_GROUP_CATEGORY, ds.OS_VaccineHx.OS_VaccineGroupCategoryColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARAM_VACCINE, ds.OS_VaccineHx.OS_VaccineColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARAM_LOT_NUMBER, ds.OS_VaccineHx.OS_LotNumberIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARAM_ROUTE, ds.OS_VaccineHx.OS_RouteColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARAM_VFC, ds.OS_VaccineHx.OS_VFCColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARAM_COMMENTS, ds.OS_VaccineHx.OS_CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARAM_DOSE, ds.OS_VaccineHx.DoseColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(8, PARAM_AMOUNT, ds.OS_VaccineHx.AmountColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARAM_SITE, ds.OS_VaccineHx.SiteColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARAM_VISDATE, ds.OS_VaccineHx.VISDateColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARAM_GIVEN_BY, ds.OS_VaccineHx.GivenByColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARAM_MANUFACTURER, ds.OS_VaccineHx.ManufacturerColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARAM_EXPIRY_DATE, ds.OS_VaccineHx.ExpiryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARAM_SOURCE_OF_HX, ds.OS_VaccineHx.SourceOfHxColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARAM_TYPE, ds.OS_VaccineHx.TypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARAM_IS_ACITVE, ds.OS_VaccineHx.ISActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(17, PARAM_PROVIDER_ID, ds.OS_VaccineHx.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(18, PARAM_REFUSAL_REASON_ID, ds.OS_VaccineHx.RefusalReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(19, PARAM_PUBLICITY_CODE, ds.OS_VaccineHx.PublicityCodeColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(20, PARAM_PUBLICITY_CODE_EXPIRY_DATE, ds.OS_VaccineHx.PublicityCodeExpiryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(21, PARAM_IMMUNIZATION_REGISTRY_STATUS_CODE, ds.OS_VaccineHx.ImmunizationRegistryStatusCodeColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(22, PARAM_IRS_EFFECTIVE_DATE, ds.OS_VaccineHx.IRSEffectiveDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(23, PARAM_PROTECTION_INDICATOR, ds.OS_VaccineHx.ProtectionIndicatorColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(24, PARAM_PI_EFFECTIVE_DATE, ds.OS_VaccineHx.PIEffectiveDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(25, PARAM_REACTION, ds.OS_VaccineHx.ReactionColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(26, PARAM_VOID_DOES, ds.OS_VaccineHx.VoidDoseColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(27, PARAM_OVERRIDE_RULE, ds.OS_VaccineHx.OverrideRuleColumn.ColumnName, DbType.Byte);
            
            if (IsInsert == true)
            {
                dbManager.AddParameters(28, PARAM_ORDERSET_ID, ds.OS_VaccineHx.OrderSetIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(29, PARAM_MAIN_AGEGROUP, ds.OS_VaccineHx.MainAgeGroupColumn.ColumnName, DbType.Int32);
                dbManager.AddParameters(30, PARAM_MAIN_SCHEDULE, ds.OS_VaccineHx.MainScheduleColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(31, PARAM_MAIN_CATEGORY, ds.OS_VaccineHx.MainCategoryColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(32, PARAM_CREATED_BY, ds.OS_VaccineHx.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(33, PARAM_CREATED_ON, ds.OS_VaccineHx.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(34, PARAM_MODIFIED_BY, ds.OS_VaccineHx.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(35, PARAM_MODIFIED_ON, ds.OS_VaccineHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            }
            else
            {
                dbManager.AddParameters(28, PARAM_MODIFIED_BY, ds.OS_VaccineHx.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(29, PARAM_MODIFIED_ON, ds.OS_VaccineHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            }
        }

        private void CreateParametersForTherapeuticInjection(IDBManager dbManager, DSImmunization ds, Boolean IsInsert)
        {
            if (IsInsert)
            {
                dbManager.CreateParameters(20);
                dbManager.AddParameters(0, PARAM_OS_IMM_THER_INJECTION_ID, ds.TherapeuticInjection.OSImmTherInjectionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(18);
                dbManager.AddParameters(0, PARAM_OS_IMM_THER_INJECTION_ID, ds.TherapeuticInjection.OSImmTherInjectionIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddParameters(1, PARAM_THERAPEUTIC_INJECTION_ID, ds.TherapeuticInjection.TherapeuticInjectionIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(2, PARAM_PROVIDER_ID, ds.TherapeuticInjection.ProviderIdColumn.ColumnName, DbType.Int64);
             dbManager.AddParameters(3, PARAM_DOSE, ds.TherapeuticInjection.DoseColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(4, PARAM_AMOUNT, ds.TherapeuticInjection.AmountColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARAM_LOT_NO, ds.TherapeuticInjection.LotNumberColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARAM_VACCINE_MANUFACTURER_ID, ds.TherapeuticInjection.ManufacturerIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARAM_ROUTE_ID, ds.TherapeuticInjection.RouteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARAM_SITE_ID, ds.TherapeuticInjection.SiteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARAM_EXPIRY_DATE, ds.TherapeuticInjection.ExpiryDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARAM_THERAPEUTIC_VFC, ds.TherapeuticInjection.VFCColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARAM_REACTION, ds.TherapeuticInjection.ReactionIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(12, PARAM_COMMENT, ds.TherapeuticInjection.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARAM_TYPE, ds.TherapeuticInjection.TypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARAM_ORDERSET_ID, ds.TherapeuticInjection.OrderSetIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARAM_SOURCE_OF_HX, ds.TherapeuticInjection.SourceOfHxColumn.ColumnName, DbType.Int64);

            if (IsInsert == true)
            {
                dbManager.AddParameters(16, PARAM_CREATED_BY, ds.TherapeuticInjection.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(17, PARAM_CREATED_ON, ds.TherapeuticInjection.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(18, PARAM_MODIFIED_BY, ds.TherapeuticInjection.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(19, PARAM_MODIFIED_ON, ds.TherapeuticInjection.ModifiedOnColumn.ColumnName, DbType.DateTime);
            }
            else
            {
                dbManager.AddParameters(16, PARAM_MODIFIED_BY, ds.TherapeuticInjection.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(17, PARAM_MODIFIED_ON, ds.TherapeuticInjection.ModifiedOnColumn.ColumnName, DbType.DateTime);
            }
        }
        #endregion


        #region "Immunization"
        public DSOS_Immunization InsertVaccineHx(DSOS_Immunization ds)
        {
            
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.OS_VaccineHx.GetChanges();
                dbManager.Open();
                //dbManager.BeginTransaction();
                CreateParametersForVaccineHx(dbManager, ds, true);
                ds = (DSOS_Immunization)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_OS_VACCINEHx_INSERT, ds, ds.OS_VaccineHx.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VaccineHx.Rows[0][ds.VaccineHx.VaccineHxIdColumn].ToString(), null, ds.VaccineHx.Rows[0][ds.VaccineHx.VaccineHxIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit

                MDVLogger.DALErrorLog("DALOS_Immunization::InsertVaccineHx", PROC_OS_VACCINEHx_INSERT, ex);
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

        public DSOS_Immunization UpdateVaccineHx(DSOS_Immunization ds)
        {
            //Start || 22 April, 2016 || ZeeshanAK || Changes for audit
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.OS_VaccineHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                this.CreateParametersForVaccineHx(dbManager, ds, false);

                ds = (DSOS_Immunization)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_OS_VaccineHx_UPDATE, ds, ds.OS_VaccineHx.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.VaccineHx.Rows[0][ds.VaccineHx.VaccineHxIdColumn].ToString(), null, ds.VaccineHx.Rows[0][ds.VaccineHx.VaccineHxIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit


                MDVLogger.DALErrorLog("DALOS_Immunization::UpdateVaccineHx", PROC_OS_VaccineHx_UPDATE, ex);
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

        public string DeleteOsVaccinehx(string VaccineHxId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSOS_Immunization ds = SearchVacinehxForEdit(Convert.ToInt64(VaccineHxId));
                //DataTable dtTemp = ds.OS_VaccineHx;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, VaccineHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_OS_VACCINEHX_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    //if (dtTemp != null)
                    //{
                    //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ProblemList.Rows[0][ds.ProblemList.ProblemListIdColumn].ToString(), null, "", false, false, true);
                    //    dsDBAudit.AcceptChanges();
                    //}
                }
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOrderSet::DeleteOsVaccinehx", PROC_OS_VACCINEHX_DELETE, ex);
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

        public DSOS_Immunization loadParentChildImmunization(long OrderSetId, long NotesId, int pageNo = 0, int rpp = 0)
        {
            DSOS_Immunization ds = new DSOS_Immunization();
            //For multiple select in same store procedure.
            List<string> tablesList = new List<string>();
            tablesList.Add(ds.ParentOS_VaccineHx.TableName);
            tablesList.Add(ds.ChildOS_VaccineHx.TableName);
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();
            dbManager.CreateParameters(5);
            dbManager.AddParameters(0, PARAM_ORDERSET_ID, OrderSetId);
            

            if (pageNo <= 0)
                dbManager.AddParameters(1, PARM_PAGE_NUMBER, 1);
            else
                dbManager.AddParameters(1, PARM_PAGE_NUMBER, pageNo);
            if (rpp <= 0)
                dbManager.AddParameters(2, PARM_ROWSP_PAGE, 2000);
            else
                dbManager.AddParameters(2, PARM_ROWSP_PAGE, rpp);

            if (NotesId <= 0)
                dbManager.AddParameters(3, PARAM_NOTES_ID, null);
            else
                dbManager.AddParameters(3, PARAM_NOTES_ID, NotesId);

            dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.ParentOS_VaccineHx.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            
            ds = (DSOS_Immunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_OS_VaccineHx_SELECT, ds, tablesList);

            dbManager.Dispose();
            return ds;

        }


        public DSOS_Immunization GetAgeLimScheCategAgainstVaccShedId(long VaccineScheduleId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSOS_Immunization ds = new DSOS_Immunization();

                //For multiple select in same store procedure.


                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_VACCINESCHEDULE_ID, VaccineScheduleId);
                ds = (DSOS_Immunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_AGELIM_SCHE_CATEG_AGAINST_VACCSHEDID, ds, ds.ScheduleDetail.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_Immunization::GetAgeLimScheCategAgainstVaccShedId", PROC_GET_AGELIM_SCHE_CATEG_AGAINST_VACCSHEDID, ex);
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

        public DSOS_Immunization SearchVacinehxForEdit(long VaccineHxId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                DSOS_Immunization ds = new DSOS_Immunization();
                //dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARAM_VACCINE_HX_ID, VaccineHxId);
                ds = (DSOS_Immunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_OS_VaccineHx_SELECT_FOR_EDIT, ds, ds.OS_VaccineHx.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_Immunization::SearchVacinehxForEdit", PROC_OS_VaccineHx_SELECT_FOR_EDIT, ex);
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


        public string IsVaccineHxInValidAge(string OS_VaccineHxId,long PatientId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_OS_VACCINE_HX_ID, OS_VaccineHxId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IS_VACCINEHX_IN_VALID_AGE).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::IsVaccineHxInValidAge", PROC_IS_VACCINEHX_IN_VALID_AGE, ex);
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


        public string IsVaccineHxLotIssue(long OS_VaccineHxId, string Type,string ImmunizationIds)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_OS_VACCINE_HX_ID, OS_VaccineHxId);
                dbManager.AddParameters(1, PARAM_TYPE, Type);
                if (!string.IsNullOrEmpty(ImmunizationIds))
                {
                    dbManager.AddParameters(2, PARAM_IMMUNIZATION_IDS, ImmunizationIds);
                }
                else
                {
                    dbManager.AddParameters(2, PARAM_IMMUNIZATION_IDS, null);
                }
                dbManager.AddParameters(3, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IS_VACCINEHX_LOT_ISSUE).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImmunization::IsVaccineHxLotIssue", PROC_IS_VACCINEHX_LOT_ISSUE, ex);
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

        
        #endregion


        #region therpeutic Injection
        public DSImmunization OS_InsertTherapeuticInjection(DSImmunization ds)
        {
            //Start || 22 April, 2016 || ZeeshanAK || Changes for audit
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForTherapeuticInjection(dbManager, ds, true);
                ds = (DSImmunization)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_OS_THERAPEUTIC_INJECTION_INSERT, ds, ds.TherapeuticInjection.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit

                MDVLogger.DALErrorLog("DALImmunization::OS_InsertTherapeuticInjection", PROC_OS_THERAPEUTIC_INJECTION_INSERT, ex);
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

        public DSImmunization OS_LoadImmunizationTherapeuticInjection(long immTherapeuticInjectionId, long OrderSetId, long NotesId=0, int pageNumber = 1, int rowsPerPage = 1000)
        {
            DSImmunization ds = new DSImmunization();


            IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();

            dbManager.CreateParameters(6);
            if (immTherapeuticInjectionId > 0)
            {
                dbManager.AddParameters(0, PARAM_OS_IMM_THER_INJECTION_ID, immTherapeuticInjectionId);
            }
            else
            {
                dbManager.AddParameters(0, PARAM_OS_IMM_THER_INJECTION_ID, null);
            }
            dbManager.AddParameters(1, PARAM_ORDERSET_ID, OrderSetId);
            if (pageNumber == 0)
                dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
            else
                dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);
            if (rowsPerPage == 0)
                dbManager.AddParameters(3, PARM_ROWSP_PAGE, null);
            else
                dbManager.AddParameters(3, PARM_ROWSP_PAGE, rowsPerPage);
            if (NotesId == 0)
                dbManager.AddParameters(4, PARAM_NOTES_ID, null);
            else
                dbManager.AddParameters(4, PARAM_NOTES_ID, NotesId);

            dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.TherapeuticInjection.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_OS_Therapeutic_Injection_SELECT, ds, ds.TherapeuticInjection.TableName);

            dbManager.Dispose();
            return ds;
        }

        public DSImmunization OS_UpdateTherapeuticInjection(DSImmunization ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForTherapeuticInjection(dbManager, ds, false);
                ds = (DSImmunization)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_OS_THERAPEUTIC_INJECTION_UPDATE, ds, ds.TherapeuticInjection.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALImmunization::OS_UpdateTherapeuticInjection", PROC_OS_THERAPEUTIC_INJECTION_UPDATE, ex);
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



        public string DeleteOsTherapeutichx(string TherapeuticHxId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARAM_OS_IMM_THER_INJECTION_ID, TherapeuticHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_OS_THERAPEUTIC_INJECTION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                }
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOrderSet::DeleteOsTherapeutichx", PROC_OS_THERAPEUTIC_INJECTION_DELETE, ex);
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
        #endregion





    }
}
