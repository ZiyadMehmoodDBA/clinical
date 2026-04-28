

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;
using MDVision.Model.Admin.Codes;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Admin
{

    public class DALCPTCode
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"
        private const string PROC_CPT_CODE_INSERT = "Provider.sp_CPTCodeInsert";
        private const string PROC_CPT_CODE_UPDATE = "Provider.sp_CPTCodeUpdate";
        private const string PROC_CPT_CODE_DELETE = "Provider.sp_CPTCodeDelete";
        private const string PROC_CPT_CODE_SELECT = "Provider.sp_CPTCodeSelect";
        private const string PROC_HPCS_CODE_SELECT = "Provider.sp_HPCSCCostSelect";
        private const string PROC_GET_CHARGE_CPTCODE = "[Billing].[sp_GetChargeCPTCode]";
        private const string PROC_CPT_CODE_LOOKUP = "Provider.sp_CPTCodeLookup";
        private const string PROC_VALIDATE_CPT_CODE = "system.sp_ValidateCPTCode";

        private const string PROC_CPT_PLAN_INFO_DELETE = "Provider.sp_CPTPlanDelete";
        private const string PROC_CPT_PLAN_INFO_INSERT = "Provider.sp_CPTPlanInsert";
        private const string PROC_CPT_PLAN_INFO_SELECT = "Provider.sp_CPTPlanSelect";
        private const string PROC_CPT_PLAN_INFO_UPDATE = "Provider.sp_CPTPlanUpdate";

        private const string PROC_CPT_FEE_SELECT = "Billing.sp_CalculateCPTFee_New";
        private const string PROC_SCT_PROCEDURES_SELECT = "Clinical.sp_SCTProceduresSelect";
        private const string PROC_SAVE_NDC_INFO = "Provider.sp_CPTNdcInsert";
        private const string PROC_SP_CPTNDC_SELECT = "Provider.sp_CPTNdcSelect";
        private const string PROC_UPDATE_NDC_INFO = "Provider.sp_CPTNdcUpdate";
        private const string PROC_CPT_NDC_DELETE = "Provider.sp_CPTNdcDelete";

        #endregion

        #region "Parameters"
        private const string PARM_CPT_CODE_ID = "@CPTCodeId";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CHARGE_ID = "@ChargeId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_BASIC_UNITS = "@BasicUnits";
        private const string PARM_UNIT_PRICE = "@UnitPrice";
        private const string PARM_DURATION_PER_UNIT = "@DurationPerUnit";
        private const string PARM_TOS_ID = "@TOSId";
        private const string PARM_SPECIALTY_ID = "@SpecialtyId";
        private const string PARM_PROCEDURE_CATEGORY_ID = "@ProcedureCategoryId";
        private const string PARM_POS_CATEGORY = "@POScategory";
        private const string PARM_ELECTRONIC = "@Electronic";
        private const string PARM_CLIA = "@CLIA";
        private const string PARM_GLOBAL_PERIOD_DAYS = "@GlobalPeriodDays";
        private const string PARM_REFERRAL_REQUIRED = "@ReferralRequired";
        private const string PARM_ACTIVELY_USED = "@ActivelyUsed";
        private const string PARM_DISCONTINUED = "@Discontinued";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";
        private const string PARM_NDC = "@NDC";
        private const string PARM_NDC_MEASUREMENT_ID = "@NDCMeasurementId";
        private const string PARM_NDC_UNIT_PRICE = "@NDCUnitPrice";
        private const string PARM_REVENUE_CODE_ID = "@RevenueCodeId";
        private const string PARM_SERVICE_DESCRIPTION = "@ServiceDescription";
        private const string PARM_COMMENTS_FOR_CLAIM = "@CommentsForClaim";
        private const string PARM_PRINT_OPTIONS = "@PrintOptions";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_CPT_PLAN_ID = "@CPTPlanId";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_CPT_PLAN_CODE = "@CPTPlanCode";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_BIT = "@Bit";
        private const string PARM_EXPECTED_FEE = "@ExpectedFee";
        private const string PARM_FEE = "@Fee";

        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PRACTICE_ID = "@PracticeId";

        private const string PARM_LEXICODE = "@LexiCode";

        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";
        private const string PARM_POS_CODE = "@POSCode";
        private const string PARM_MODIFIER_1 = "@Modifier1";
        private const string PARM_MODIFIER_2 = "@Modifier2";
        private const string PARM_MODIFIER_3 = "@Modifier3";
        private const string PARM_MODIFIER_4 = "@Modifier4";
        private const string PARM_CHARGE_DOS = "@ChargeDOS";

        private const string PARM_SCT_SNOMED = "@SNOMED";
        private const string PARM_CPT_NDC_ID = "@CPTNdcId";
        private const string PARM_NDC_CODE = "@NDCCode";
        private const string PARM_UNIT = "@Unit";
        private const string PARM_NDC_DESCRIPTION = "@NDCDescription";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALCPTCode"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALCPTCode()
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
        private void CreateParameters(IDBManager dbManager, DSCodes ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(32);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CPT_CODE_ID, ds.CPTCode.CPTCodeIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CPT_CODE_ID, ds.CPTCode.CPTCodeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CPT_CODE, ds.CPTCode.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_SHORT_NAME, ds.CPTCode.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.CPTCode.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_BASIC_UNITS, ds.CPTCode.BasicUnitsColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(5, PARM_FEE, ds.CPTCode.FeeColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(6, PARM_DURATION_PER_UNIT, ds.CPTCode.DurationPerUnitColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(7, PARM_TOS_ID, ds.CPTCode.TOSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_SPECIALTY_ID, ds.CPTCode.SpecialtyIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_PROCEDURE_CATEGORY_ID, ds.CPTCode.ProcedureCategoryIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_POS_CATEGORY, ds.CPTCode.POScategoryColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_ELECTRONIC, ds.CPTCode.ElectronicColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_CLIA, ds.CPTCode.CLIAColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(13, PARM_GLOBAL_PERIOD_DAYS, ds.CPTCode.GlobalPeriodDaysColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_REFERRAL_REQUIRED, ds.CPTCode.ReferralRequiredColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(15, PARM_ACTIVELY_USED, ds.CPTCode.ActivelyUsedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(16, PARM_DISCONTINUED, ds.CPTCode.DiscontinuedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(17, PARM_START_DATE, ds.CPTCode.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(18, PARM_END_DATE, ds.CPTCode.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, PARM_NDC, ds.CPTCode.NDCColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_NDC_MEASUREMENT_ID, ds.CPTCode.NDCMeasurementIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(21, PARM_NDC_UNIT_PRICE, ds.CPTCode.NDCUnitPriceColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(22, PARM_SERVICE_DESCRIPTION, ds.CPTCode.ServiceDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_PRINT_OPTIONS, ds.CPTCode.PrintOptionsColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(24, PARM_IS_ACTIVE, ds.CPTCode.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(25, PARM_CREATED_BY, ds.CPTCode.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_CREATED_ON, ds.CPTCode.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(27, PARM_MODIFIED_BY, ds.CPTCode.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_MODIFIED_ON, ds.CPTCode.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(29, PARM_ENTITY_ID, ds.CPTCode.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(30, PARM_EXPECTED_FEE, ds.CPTCode.ExpectedFeeColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(31, PARM_LEXICODE, ds.CPTCode.LexiCodeColumn.ColumnName, DbType.String);
        }

        /// <summary>
        /// Creates the parameters_ CPT plan_ information.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters_CPTPlan_Info(IDBManager dbManager, DSCodes ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(5);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CPT_PLAN_ID, ds.CPTPlan.CPTPlanIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CPT_PLAN_ID, ds.CPTPlan.CPTPlanIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CPT_CODE_ID, ds.CPTPlan.CPTCodeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, ds.CPTPlan.InsurancePlanIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CPT_PLAN_CODE, ds.CPTPlan.CPTPlanCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }

        private void createCPTNdcParameters(IDBManager dbManager, CPTNdcModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_CPT_NDC_ID, model.CPTNdcId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_CPT_NDC_ID, model.CPTNdcId, DbType.Int64);
            if (!string.IsNullOrEmpty(model.CPTCodeId))
            {
                dbManager.AddParameters(PARM_CPT_CODE_ID, model.CPTCodeId);
            }
            else
            {
                dbManager.AddParameters(PARM_CPT_CODE_ID, null);
            }
            if (!string.IsNullOrEmpty(model.NDCCode))
            {
                dbManager.AddParameters(PARM_NDC_CODE, model.NDCCode);
            }
            else
            {
                dbManager.AddParameters(PARM_NDC_CODE, null);
            }

            if (!string.IsNullOrEmpty(model.NDCDescription))
            {
                dbManager.AddParameters(PARM_NDC_DESCRIPTION, model.NDCDescription);
            }
            else
            {
                dbManager.AddParameters(PARM_NDC_DESCRIPTION, null);
            }

            if (!string.IsNullOrEmpty(model.Unit))
            {
                dbManager.AddParameters(PARM_UNIT, model.Unit);
            }
            else
            {
                dbManager.AddParameters(PARM_UNIT, null);
            }


            if (!string.IsNullOrEmpty(model.UnitPrice))
            {
                dbManager.AddParameters(PARM_UNIT_PRICE, model.UnitPrice);
            }
            else
            {
                dbManager.AddParameters(PARM_UNIT_PRICE, null);
            }

            if (!string.IsNullOrEmpty(model.NDCMeasurementId))
            {
                dbManager.AddParameters(PARM_NDC_MEASUREMENT_ID, model.NDCMeasurementId);
            }
            else
            {
                dbManager.AddParameters(PARM_NDC_MEASUREMENT_ID, null);
            }


           
            if (IsInsert)
            {
                if (!string.IsNullOrEmpty(model.CreatedBy))
                {
                    dbManager.AddParameters(PARM_CREATED_BY, model.CreatedBy);
                }
                else
                {
                    dbManager.AddParameters(PARM_CREATED_BY, null);
                }

                if (!string.IsNullOrEmpty(model.CreatedOn))
                {
                    dbManager.AddParameters(PARM_CREATED_ON, model.CreatedOn);
                }
                else
                {
                    dbManager.AddParameters(PARM_CREATED_ON, null);
                }

            }
            if (!string.IsNullOrEmpty(model.ModifiedBy))
            {
                dbManager.AddParameters(PARM_MODIFIED_BY, model.ModifiedBy);
            }
            else
            {
                dbManager.AddParameters(PARM_MODIFIED_BY, null);
            }
            if (!string.IsNullOrEmpty(model.ModifiedOn))
            {
                dbManager.AddParameters(PARM_MODIFIED_ON, model.ModifiedOn);
            }
            else
            {
                dbManager.AddParameters(PARM_MODIFIED_ON, null);
            }

        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSCodes LoadCPTCode(long CPTCodeId, string ShortName, string CPT, string Description, string TOSId, string SpecialtyId, string IsActive, string Discontinued, string EntityId, Int64 PageNumber, Int64 RowsPerPage)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (CPT == "")
                    CPT = null;

                if (Description == "")
                    Description = null;

                if (TOSId == "")
                    TOSId = null;

                if (SpecialtyId == "")
                    SpecialtyId = null;

                if (Discontinued == "")
                    Discontinued = null;

                if (EntityId == "")
                    EntityId = null;

                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(13);

                if (CPTCodeId <= 0)
                    dbManager.AddParameters(0, PARM_CPT_CODE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CPT_CODE_ID, CPTCodeId);

                dbManager.AddParameters(1, PARM_CPT_CODE, CPT);
                dbManager.AddParameters(2, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(3, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(4, PARM_TOS_ID, TOSId);
                dbManager.AddParameters(5, PARM_SPECIALTY_ID, SpecialtyId);
                dbManager.AddParameters(6, PARM_DISCONTINUED, Discontinued);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(7, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(7, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(8, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(10, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(10, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, ds.ICD.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(12, PARM_IS_ACTIVE, IsActive);
                //dbManager.AddParameters(5, PARM_DISCONTINUED, Discontinued);
                //dbManager.AddParameters(4, PARM_EIN, EIN);
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CPT_CODE_SELECT, ds, ds.CPTCode.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::LoadCPTCode", PROC_CPT_CODE_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }

        public DSCodes LoadHPCSCode(string HPCSCode)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_CPT_CODE, HPCSCode);




                //dbManager.AddParameters(5, PARM_DISCONTINUED, Discontinued);
                //dbManager.AddParameters(4, PARM_EIN, EIN);
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_HPCS_CODE_SELECT, ds, ds.HPCSCode.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::LoadHPCSCode", PROC_HPCS_CODE_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }

        public DSCodes LoadCPTFee(Int64 VisitId, string CPTCode, Int64 ProviderId, Int64 FacilityId, Int64 PracticeId, DateTime ChargeDOS, Int64 PatientInsuranceId = 0
                                , string POSCode = "", string Modifier1 = "", string Modifier2 = "", string Modifier3 = "", string Modifier4 = "")
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (CPTCode == "")
                    CPTCode = null;

                dbManager.Open();
                dbManager.CreateParameters(12);
                if (VisitId == 0)
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);

                dbManager.AddParameters(1, PARM_CPT_CODE, CPTCode);

                if (ProviderId == 0)
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, ProviderId);

                if (FacilityId == 0)
                    dbManager.AddParameters(3, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_FACILITY_ID, FacilityId);


                if (PracticeId == 0)
                    dbManager.AddParameters(4, PARM_PRACTICE_ID, null);
                else
                    dbManager.AddParameters(4, PARM_PRACTICE_ID, PracticeId);

                if (PatientInsuranceId == 0)
                    dbManager.AddParameters(5, PARM_PATIENT_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PATIENT_INSURANCE_ID, PatientInsuranceId);

                if (POSCode == "")
                    POSCode = null;
                if (Modifier1 == "")
                    Modifier1 = null;
                if (Modifier2 == "")
                    Modifier2 = null;
                if (Modifier3 == "")
                    Modifier3 = null;
                if (Modifier4 == "")
                    Modifier4 = null;

                dbManager.AddParameters(6, PARM_POS_CODE, POSCode);
                dbManager.AddParameters(7, PARM_MODIFIER_1, Modifier1);
                dbManager.AddParameters(8, PARM_MODIFIER_2, Modifier2);
                dbManager.AddParameters(9, PARM_MODIFIER_3, Modifier3);
                dbManager.AddParameters(10, PARM_MODIFIER_4, Modifier4);
                dbManager.AddParameters(11, PARM_CHARGE_DOS, ChargeDOS);



                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CPT_FEE_SELECT, ds, ds.CPTFee.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisits::LoadCPTFee", PROC_CPT_FEE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCodes UpdateCPTCode(ref DSCodes ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.CPTCode.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSCodes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CPT_CODE_UPDATE, ds, ds.CPTCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.CPTCode.Rows[0][ds.CPTCode.CPTCodeIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::UpdateCPTCode", PROC_CPT_CODE_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }


        }


        public string DeleteCPTCode(string CPTCodeIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSCodes ds = LoadCPTCode(Convert.ToInt64(CPTCodeIds), null, null, null, null, null, null, null, null, 1, 15);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.CPTCode;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CPT_CODE_ID, CPTCodeIds);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_USER_DELETE);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CPT_CODE_DELETE).ToString();
                if (returnValue != "" && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.CPTCode.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.CPTCode.Rows[0][ds.CPTCode.CPTCodeIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::UpdateCPTCode", PROC_CPT_CODE_DELETE, ex);
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

        public DSCodes InsertCPTCode(ref DSCodes ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.CPTCode.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSCodes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CPT_CODE_INSERT, ds, ds.CPTCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.CPTCode.Rows[0][ds.CPTCode.CPTCodeIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::LoadCPTCode", PROC_CPT_CODE_INSERT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        //public DSProfileLookup LookupTypeOfService()
        //{
        //    DSProfileLookup ds = new DSProfileLookup();
        //    IDBManager dbManager =ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        //dbManager.CreateParameters(1);

        //        //if (POSId <= 0)
        //        //    dbManager.AddParameters(0, PARM_POS_ID, null);
        //        //else
        //        //    dbManager.AddParameters(0, PARM_POS_ID, POSId);

        //        ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOKUP_TYPE_OF_SERVICE, ds, ds.TypeOfService.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALProfile::LookupFacilityType", PROC_LOOKUP_TYPE_OF_SERVICE, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        #region "use for transaction with dataset"

        #endregion
        #endregion

        #region "Insert, delete, update and get using dataset Functions for CPT Plan Info"
        /// <summary>
        /// Loads the CPT plan information.
        /// </summary>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <returns></returns>
        public DSCodes LoadCPTPlanInfo(long CPTCodeId, long CPTPlanId)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (CPTCodeId <= 0)
                    dbManager.AddParameters(0, PARM_CPT_CODE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CPT_CODE_ID, CPTCodeId);
                if (CPTPlanId <= 0)
                    dbManager.AddParameters(1, PARM_CPT_PLAN_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CPT_PLAN_ID, CPTPlanId);
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CPT_PLAN_INFO_SELECT, ds, ds.CPTPlan.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::LoadCPTPlanInfo", PROC_CPT_PLAN_INFO_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the CPT plan information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes UpdateCPTPlanInfo(DSCodes ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.CPTPlan.GetChanges();
                this.CreateParameters_CPTPlan_Info(dbManager, ds, false);
                ds = (DSCodes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CPT_PLAN_INFO_UPDATE, ds, ds.CPTPlan.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.CPTPlan.Rows[0][ds.CPTPlan.CPTPlanIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::UpdateCPTPlanInfo", PROC_CPT_PLAN_INFO_UPDATE, ex);
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

        /// <summary>
        /// Deletes the CPT plan information.
        /// </summary>
        /// <param name="CPTPlanId">The CPT plan identifier.</param>
        /// <returns></returns>
        public string DeleteCPTPlanInfo(string CPTPlanId)
        {
            object returnValue;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSCodes ds = LoadCPTPlanInfo(0, Convert.ToInt64(CPTPlanId));
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.CPTPlan;
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_CPT_PLAN_ID, CPTPlanId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CPT_PLAN_INFO_DELETE);
                //if (dtTemp != null && ds.CPTPlan.Rows.Count > 0)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.CPTPlan.Rows[0][ds.CPTPlan.CPTPlanIdColumn].ToString(), "", false, false, true);
                //    dsDBAudit.AcceptChanges();
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::DeleteCPTPlanInfo", PROC_CPT_PLAN_INFO_DELETE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the CPT plan information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes InsertCPTPlanInfo(DSCodes ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.CPTPlan.GetChanges();
                CreateParameters_CPTPlan_Info(dbManager, ds, true);
                ds = (DSCodes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CPT_PLAN_INFO_INSERT, ds, ds.CPTPlan.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.CPTPlan.Rows[0][ds.CPTPlan.CPTPlanIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::InsertCPTPlanInfo", PROC_CPT_PLAN_INFO_INSERT, ex);
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

        #region Lookups
        /// <summary>
        /// Lookups the CPT code.
        /// </summary>
        /// <param name="EntityId">The entity identifier.</param>
        /// <param name="CPTCode">The CPT code.</param>
        /// <param name="IsEqule">The is equle.</param>
        /// <returns></returns>
        public DSCodeLookup LookupCPTCode(string EntityId, string CPTCode, int IsEqule)
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (CPTCode == "")
                    CPTCode = null;

                if (EntityId == "")
                    EntityId = null;

                dbManager.Open();
                dbManager.CreateParameters(4);

                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                //While parsing from JSON, undefined becomes undefined value in string
                if (string.IsNullOrWhiteSpace(EntityId) || EntityId == "undefined")
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(2, PARM_CPT_CODE, CPTCode);
                dbManager.AddParameters(3, PARM_BIT, IsEqule);


                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CPT_CODE_LOOKUP, ds, ds.CPTCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::LookupCPTCode", PROC_CPT_CODE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSCodeLookup ValidateCPTCode(string CPTCode)
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (CPTCode == "")
                    CPTCode = null;
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_CPT_CODE, CPTCode);

                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VALIDATE_CPT_CODE, ds, ds.CPTCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::ValidateCPTCode", PROC_CPT_CODE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region SCTProcedures

        public DSCodes LoadSCTProcedures(string SCTProcedures)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (SCTProcedures == "")
                    SCTProcedures = null;
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SCT_SNOMED, SCTProcedures);
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCT_PROCEDURES_SELECT, ds, ds.SCTProcedures.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::LoadSCTProcedures", PROC_SCT_PROCEDURES_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }



        public DSCodes LoadCPTCodeByChargeId(Int64 chargeId)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_CHARGE_ID, chargeId);
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_CHARGE_CPTCODE, ds, ds.ChargeCPT.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::LoadCPTCodeByChargeId", PROC_GET_CHARGE_CPTCODE, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        #endregion

        #region NDC
        public string SaveNDCInfo(CPTNdcModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createCPTNdcParameters(dbManager, model, true);
                var NDCId = dbManager.ExecuteScalar(PROC_SAVE_NDC_INFO);
                return MDVUtility.ToStr(NDCId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::SaveNDCInfo", PROC_SAVE_NDC_INFO, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdateNDCInfo(CPTNdcModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createCPTNdcParameters(dbManager, model, false);
                var NDCId = dbManager.ExecuteScalar(PROC_UPDATE_NDC_INFO);
                return MDVUtility.ToStr(NDCId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::UpdateNDCInfo", PROC_UPDATE_NDC_INFO, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        
        public List<CPTNdcModel> LoadCptNdc(Int64 CptCodeId=0, string CptCode="", Int64 CPTNdcId = 0, string NdcCode="")
        {
            List<CPTNdcModel> NdcList = new List<CPTNdcModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (CptCodeId == 0)
                {
                    dbManager.AddParameters(PARM_CPT_CODE_ID, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_CPT_CODE_ID, CptCodeId);
                }

                if (string.IsNullOrEmpty(CptCode))
                {
                    dbManager.AddParameters(PARM_CPT_CODE, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_CPT_CODE, CptCode);
                }
                if (CPTNdcId == 0)
                {
                    dbManager.AddParameters(PARM_CPT_NDC_ID, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_CPT_NDC_ID, CPTNdcId);
                }
                if (string.IsNullOrEmpty(NdcCode))
                {
                    dbManager.AddParameters(PARM_NDC_CODE, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_NDC_CODE, NdcCode);
                }
                NdcList = dbManager.ExecuteReaders<CPTNdcModel>(PROC_SP_CPTNDC_SELECT);
                return NdcList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCptCode::LoadCptNdc", PROC_SP_CPTNDC_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }
        
        public string DeleteCPTNdcInfo(string CPTNdcId)
        {
            object returnValue;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_CPT_NDC_ID, CPTNdcId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_CPT_NDC_DELETE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCPTCode::DeleteCPTNdcInfo", PROC_CPT_NDC_DELETE, ex);
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
