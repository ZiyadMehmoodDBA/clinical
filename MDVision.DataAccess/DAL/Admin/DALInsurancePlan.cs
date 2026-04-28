using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using System.Data.SqlClient;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALInsurancePlan
    {
        #region Variable

        #endregion

        #region " Stored Procedure Names"
        private const string PROC_INSURANCE_PLAN_INSERT = "Provider.sp_InsurancePlanInsert";
        private const string PROC_INSURANCE_PLAN_UPDATE = "Provider.sp_InsurancePlanUpdate";
        private const string PROC_INSURANCE_PLAN_DELETE = "Provider.sp_InsurancePlanDelete";
        private const string PROC_INSURANCE_PLAN_SELECT = "Provider.sp_InsurancePlanSelect";
        private const string PROC_INSURANCE_PLAN_ADDRESS_INSERT = "Provider.sp_InsurancePlanAddressInsert";
        private const string PROC_INSURANCE_PLAN_ADDRESS_UPDATE = "Provider.sp_InsurancePlanAddressUpdate";
        private const string PROC_INSURANCE_PLAN_ADDRESS_DELETE = "Provider.sp_InsurancePlanAddressDelete";
        private const string PROC_INSURANCE_PLAN_ADDRESS_SELECT = "Provider.sp_InsurancePlanAddressSelect";
        private const string PROC_INSURANCE_PLAN_ADDRESS_SEARCH = "Provider.sp_InsPlanAddressSelect";
        private const string PROC_INSURANCEPLAN_LOOKUP = "Provider.sp_InsurancePlanLookup";

        private const string PROC_CLAIM_FLAG_LOOKUP = "Provider.sp_ClaimFlagLookup";
        private const string PROC_CLAIIM_TYPE_LOOKUP = "Provider.sp_ClaimTypeLookup";
        private const string PROC_CLAIM_SCRUBBING_PROFILE_LOOKUP = "Provider.sp_ClaimScrubbingProfileLookup";
        private const string PROC_PLAN_FEE_LINK_LOOKUP = "Provider.sp_PlanFeeLinkLookup";
        private const string PROC_PLAN_TYPE_LOOKUP = "Provider.sp_PlanTypeLookup";
        private const string PROC_INSURANCE_PLAN_LOOKUP = "Provider.sp_InsurancePlanLookup";
        private const string PROC_INSURANCE_PLAN_ADDRESS_LOOKUP = "Provider.sp_InsurancePlanAddressLookup";
        private const string PROC_INSURANCE_PLAN_MATCHING = "Provider.sp_InsPlanSelect";
        private const string PROC_INSURANCE_PLAN_BY_PATTERN_MATCHING = "Provider.sp_InsurancePlanSelectByPatternMatch";
        #endregion

        #region "Parameters"
        private const string PARM_PLAN_ID = "@PlanId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCCRIPTION = "@Description";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_INSURANCE_ID = "@InsuranceId";
        private const string PARM_CO_PAMENT = "@CoPayment";
        private const string PARM_MODIFIER = "@Modifier";
        private const string PARM_PLAN_FEE_LINK_ID = "@PlanFeeLinkId";
        private const string PARM_OUTSTANDING_DAYS = "@OutstandingDays";
        private const string PARM_MANAGED_CARE = "@ManagedCare";
        private const string PARM_CAPITATED_PLAN = "@CapitatedPlan";
        private const string PARM_CAP_AUTO_WRITEOFF = "@CapAutoWriteOff";
        private const string PARM_ELECTRONIC_SUBMIT = "@ElectronicSubmit";
        private const string PARM_PLAN_TYPE_ID = "@PlanTypeId";
        private const string PARM_CLAIM_FLAG_ID = "@ClaimFlagId";
        private const string PARM_CLAIM_TYPE_ID = "@ClaimTypeId";
        private const string PARM_CLAIM_PAYER_ID = "@ClaimPayerId";
        private const string PARM_EDI_SUBMIT_INSURANCE_ID = "@EDISubmitInsuranceId";
        private const string PARM_EDI_ELIGIBILITY_INSURANCE_ID = "@EDIEligibilityInsuranceId";
        private const string PARM_EDI_CLAIM_STATUS_INSURANCE_ID = "@EDIClaimStatusInsuranceId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_CLAIM_SCRUBBING_TEMPLATE = "@ClaimScrubbingTemplate";
        private const string PARM_INSURANCE_PLAN_ADDRESS_ID = "@InsurancePlanAddressId";
        private const string PARM_PHONE_NO = "@PhoneNo";
        private const string PARM_PHONE_EXT = "@PhoneExt";
        private const string PARM_FAX = "@Fax";
        private const string PARM_ADDRESS = "@Address";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP_CODE = "@ZIPCode";
        private const string PARM_ZIP_CODE_EXT = "@ZIPCodeExt";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_WEB_PORTAL = "@WebPortal";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_PASSWORD = "@Password";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_SEARCH_PATTERN = "@SearchPattern";
        private const string PARM_ISICD10 = "@IsICD10";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_SUBSCRIBER_ID = "@SubscriberId";
        private const string PARM_RegularExpression = "@RegularExpression";
        private const string PARM_INSURANCE_PLAN_IDS = "@InsurancePlanIds";
        private const string PARM_SPLITTED = "@Splitted";
        private const string PARM_SPLITTED_INSURANCE_PLAN_ID = "@SplittedInsurancePlanId";
        private const string PARM_IS_REPORT_NPI = "@IsReportNPI";
        private const string PARM_IS_ANESTHESIA_BY_MINUTES = "@IsAnesthesiaByMinutes";
        private const string PARM_BOX_24_IJ_SHADED = "@Box24IJShaded";
        private const string PARM_BOX_24_B_SHADED = "@Box24BShaded";
        private const string PARM_IS_REFFERAL_REQUIRED = "@IsReferralRequired";

        #endregion

        #region Constructors
        public DALInsurancePlan()
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
        private void CreateParameters(IDBManager dbManager, DSInsurance ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(37);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_INSURANCE_PLAN_ID, ds.InsurancePlan.InsurancePlanIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_INSURANCE_PLAN_ID, ds.InsurancePlan.InsurancePlanIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.InsurancePlan.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCCRIPTION, ds.InsurancePlan.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_INSURANCE_ID, ds.InsurancePlan.InsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_PLAN_ID, ds.InsurancePlan.PlanIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_CO_PAMENT, ds.InsurancePlan.CoPaymentColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_MODIFIER, ds.InsurancePlan.ModifierColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_IS_ACTIVE, ds.InsurancePlan.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_PLAN_FEE_LINK_ID, ds.InsurancePlan.PlanFeeLinkIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_OUTSTANDING_DAYS, ds.InsurancePlan.OutstandingDaysColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MANAGED_CARE, ds.InsurancePlan.ManagedCareColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CAPITATED_PLAN, ds.InsurancePlan.CapitatedPlanColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CAP_AUTO_WRITEOFF, ds.InsurancePlan.CapAutoWriteOffColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_ELECTRONIC_SUBMIT, ds.InsurancePlan.ElectronicSubmitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_PLAN_TYPE_ID, ds.InsurancePlan.PlanTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARM_CLAIM_FLAG_ID, ds.InsurancePlan.ClaimFlagIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(16, PARM_CLAIM_TYPE_ID, ds.InsurancePlan.ClaimTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(17, PARM_CLAIM_PAYER_ID, ds.InsurancePlan.ClaimPayerIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_EDI_SUBMIT_INSURANCE_ID, ds.InsurancePlan.EDISubmitInsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(19, PARM_EDI_ELIGIBILITY_INSURANCE_ID, ds.InsurancePlan.EDIEligibilityInsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(20, PARM_EDI_CLAIM_STATUS_INSURANCE_ID, ds.InsurancePlan.EDIClaimStatusInsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(21, PARM_CREATED_BY, ds.InsurancePlan.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_CREATED_ON, ds.InsurancePlan.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(23, PARM_MODIFIED_BY, ds.InsurancePlan.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_MODIFIED_ON, ds.InsurancePlan.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(25, PARM_CLAIM_SCRUBBING_TEMPLATE, ds.InsurancePlan.ClaimScrubbingTemplateColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(26, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            dbManager.AddParameters(27, PARM_SEARCH_PATTERN, ds.InsurancePlan.SearchPatternColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_ISICD10, ds.InsurancePlan.IsICD10Column.ColumnName, DbType.Byte);
            dbManager.AddParameters(29, PARM_RegularExpression, ds.InsurancePlan.RegularExpressionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_SPLITTED, ds.InsurancePlan.SplittedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(31, PARM_SPLITTED_INSURANCE_PLAN_ID, ds.InsurancePlan.SplittedInsurancePlanIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(32, PARM_IS_REPORT_NPI, ds.InsurancePlan.IsReportNPIColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(33, PARM_IS_ANESTHESIA_BY_MINUTES, ds.InsurancePlan.IsAnesthesiaByMinutesColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(34, PARM_BOX_24_IJ_SHADED, ds.InsurancePlan.Box24IJShadedColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(35, PARM_BOX_24_B_SHADED, ds.InsurancePlan.Box24BShadedColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(36, PARM_IS_REFFERAL_REQUIRED, ds.InsurancePlan.IsReferralRequiredColumn.ColumnName, DbType.Boolean);

        }

        /// <summary>
        /// Creates the parameters insurance plan address information.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParametersInsurancePlanAddressInfo(IDBManager dbManager, DSInsurance ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(19);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_INSURANCE_PLAN_ADDRESS_ID, ds.InsurancePlanAddress.InsurancePlanAddressIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_INSURANCE_PLAN_ADDRESS_ID, ds.InsurancePlanAddress.InsurancePlanAddressIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_INSURANCE_PLAN_ID, ds.InsurancePlanAddress.InsurancePlanIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ADDRESS, ds.InsurancePlanAddress.AddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CITY, ds.InsurancePlanAddress.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_STATE, ds.InsurancePlanAddress.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_ZIP_CODE, ds.InsurancePlanAddress.ZipCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_ZIP_CODE_EXT, ds.InsurancePlanAddress.ZipCodeExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_PHONE_NO, ds.InsurancePlanAddress.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_PHONE_EXT, ds.InsurancePlanAddress.PhoneExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_FAX, ds.InsurancePlanAddress.FaxColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_EMAIL_ADDRESS, ds.InsurancePlanAddress.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_WEB_PORTAL, ds.InsurancePlanAddress.WebPortalColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_USER_NAME, ds.InsurancePlanAddress.UserNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_PASSWORD, ds.InsurancePlanAddress.PasswordColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_IS_ACTIVE, ds.InsurancePlanAddress.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(15, PARM_CREATED_BY, ds.InsurancePlanAddress.CreateByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_CREATED_ON, ds.InsurancePlanAddress.CreateOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_MODIFIED_BY, ds.InsurancePlanAddress.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_MODIFIED_ON, ds.InsurancePlanAddress.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the insurance plan.
        /// </summary>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="InsuranceId">The insurance identifier.</param>
        /// <param name="PlanId">The plan identifier.</param>
        /// <param name="PlanType">Type of the plan.</param>
        /// <param name="ClaimFlag">The claim flag.</param>
        /// <param name="ClaimType">Type of the claim.</param>
        /// <returns></returns>
        public DSInsurance LoadInsurancePlan(long InsurancePlanId, string ShortName, string Description, string InsuranceId, string PlanId, string PlanTypeId, string ClaimFlagId, string ClaimTypeId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000, string SubscriberId = "", string entityId = null, string Address = null)
        {
            DSInsurance ds = new DSInsurance();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                if (InsuranceId == "")
                    InsuranceId = null;

                if (PlanId == "")
                    PlanId = null;

                if (PlanTypeId == "")
                    PlanTypeId = null;

                if (ClaimFlagId == "")
                    ClaimFlagId = null;

                if (ClaimTypeId == "")
                    ClaimTypeId = null;

                if (IsActive == "")
                    IsActive = null;

                if (SubscriberId == "")
                    SubscriberId = null;


                if (Address == "")
                    Address = null;

                dbManager.Open();
                dbManager.CreateParameters(15);
                int i = 0;

                if (InsurancePlanId == 0)
                    dbManager.AddParameters(i++, PARM_INSURANCE_PLAN_ID, null);
                else
                    dbManager.AddParameters(i++, PARM_INSURANCE_PLAN_ID, InsurancePlanId);
                dbManager.AddParameters(i++, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(i++, PARM_DESCCRIPTION, Description);
                dbManager.AddParameters(i++, PARM_INSURANCE_ID, InsuranceId);
                dbManager.AddParameters(i++, PARM_PLAN_ID, PlanId);
                dbManager.AddParameters(i++, PARM_PLAN_TYPE_ID, PlanTypeId);
                dbManager.AddParameters(i++, PARM_CLAIM_FLAG_ID, ClaimFlagId);
                dbManager.AddParameters(i++, PARM_CLAIM_TYPE_ID, ClaimTypeId);
                dbManager.AddParameters(i++, PARM_ENTITY_ID, entityId);
                dbManager.AddParameters(i++, PARM_ADDRESS, Address);
                dbManager.AddParameters(i++, PARM_IS_ACTIVE, IsActive);
                if (PageNumber == 0)
                    dbManager.AddParameters(i++, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(i++, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(i++, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(i++, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(i++, PARM_RECORD_COUNT, ds.InsurancePlan.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(i++, PARM_SUBSCRIBER_ID, SubscriberId);
                ds = (DSInsurance)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_SELECT, ds, ds.InsurancePlan.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::LoadInsurancePlan", PROC_INSURANCE_PLAN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSInsurance LoadInsurancePlanForPatternMatching(bool IsActive)
        {
            DSInsurance ds = new DSInsurance();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);
                ds = (DSInsurance)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_MATCHING, ds, ds.InsurancePlanRegex.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::LoadInsurancePlan", PROC_INSURANCE_PLAN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSInsurance LoadMatchedInsurancePlan(string InsurancePlanIds)
        {
            DSInsurance ds = new DSInsurance();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_INSURANCE_PLAN_IDS, InsurancePlanIds);
                ds = (DSInsurance)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_BY_PATTERN_MATCHING, ds, ds.InsurancePlan.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::LoadInsurancePlan", PROC_INSURANCE_PLAN_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the insurance plan.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSInsurance UpdateInsurancePlan(ref DSInsurance ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.InsurancePlan.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSInsurance)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_UPDATE, ds, ds.InsurancePlan.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.InsurancePlan.Rows[0][ds.InsurancePlan.InsurancePlanIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::UpdateInsurancePlan", PROC_INSURANCE_PLAN_UPDATE, ex);
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
        /// Deletes the insurance plan.
        /// </summary>
        /// <param name="InsurancePlanIds">The insurance plan ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteInsurancePlan(string InsurancePlanIds)
        {
            object returnValue;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSInsurance ds = LoadInsurancePlan(Convert.ToInt64(InsurancePlanIds), null, null, null, null, null, null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.InsurancePlan;

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_INSURANCE_PLAN_ID, InsurancePlanIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_DELETE);
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.InsurancePlan.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.InsurancePlan.Rows[0][ds.InsurancePlan.InsurancePlanIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::DeleteInsurancePlan", PROC_INSURANCE_PLAN_DELETE, ex);
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
        /// Inserts the insurance plan.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSInsurance InsertInsurancePlan(ref DSInsurance ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.InsurancePlan.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSInsurance)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_INSERT, ds, ds.InsurancePlan.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.InsurancePlan.Rows[0][ds.InsurancePlan.InsurancePlanIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::InsertInsurancePlan", PROC_INSURANCE_PLAN_INSERT, ex);
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

        public DSInsuranceLookup LookupInsurancePlan(string Active, string EntityId, string ShortName)
        {
            DSInsuranceLookup ds = new DSInsuranceLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(3);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(1, PARM_IS_ACTIVE, Active);

                if (string.IsNullOrEmpty(ShortName))
                    dbManager.AddParameters(2, PARM_SHORT_NAME, null);
                else
                    dbManager.AddParameters(2, PARM_SHORT_NAME, ShortName);

                ds = (DSInsuranceLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCEPLAN_LOOKUP, ds, ds.InsurancePlan.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupInsurancePlan", PROC_INSURANCEPLAN_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions for Insurance Plan Address Info"
        /// <summary>
        /// Loads the insurance plan address information.
        /// </summary>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <returns></returns>
        public DSInsurance LoadInsurancePlanAddressInfo(long InsurancePlanId, long IndurancePlanAddressId)
        {
            DSInsurance ds = new DSInsurance();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (InsurancePlanId <= 0)
                    dbManager.AddParameters(0, PARM_INSURANCE_PLAN_ID, null);
                else
                    dbManager.AddParameters(0, PARM_INSURANCE_PLAN_ID, InsurancePlanId);
                if (IndurancePlanAddressId <= 0)
                    dbManager.AddParameters(1, PARM_INSURANCE_PLAN_ADDRESS_ID, null);
                else
                    dbManager.AddParameters(1, PARM_INSURANCE_PLAN_ADDRESS_ID, IndurancePlanAddressId);
                ds = (DSInsurance)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_ADDRESS_SELECT, ds, ds.InsurancePlanAddress.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::LoadInsurancePlanAddressInfo", PROC_INSURANCE_PLAN_ADDRESS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSInsurance LoadInsurancePlanAddressSearch(string InsurancePlan, string Description, string Address, string City, string State, string Zip, string Telephone, int PageNumber, int RowsPerPage)
        {
            DSInsurance ds = new DSInsurance();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                //List<SqlParameter> parameters = new List<SqlParameter>();

                //parameters.Add(new SqlParameter(PARM_SHORT_NAME, InsurancePlan));
                //parameters.Add(new SqlParameter(PARM_DESCCRIPTION, Description));
                //parameters.Add(new SqlParameter(PARM_ADDRESS, Address));
                //parameters.Add(new SqlParameter(PARM_CITY, City));
                //parameters.Add(new SqlParameter(PARM_ZIP_CODE, Zip));
                //parameters.Add(new SqlParameter(PARM_PHONE_NO, Telephone));
                //parameters.Add(new SqlParameter(PARM_PAGE_NUMBER, PageNumber));
                //parameters.Add(new SqlParameter(PARM_ROWSP_PAGE, RowsPerPage));
                //SqlParameter customParam = new SqlParameter();

                //customParam.Direction = ParameterDirection.Output;
                //customParam.Value = "yourvalue";
                //customParam.ParameterName = PARM_RECORD_COUNT;

                //parameters.Add(customParam);
                //List<InsurancePlanAddressModel> modelList = dbManager.ExecuteReaders<InsurancePlanAddressModel>(PROC_INSURANCE_PLAN_ADDRESS_SEARCH, parameters);

                //return modelList;
                dbManager.Open();
                dbManager.CreateParameters(11);
                if (InsurancePlan == "")
                    dbManager.AddParameters(0, PARM_SHORT_NAME, null);
                else
                    dbManager.AddParameters(0, PARM_SHORT_NAME, InsurancePlan);
                if (Description == "")
                    dbManager.AddParameters(1, PARM_DESCCRIPTION, null);
                else
                    dbManager.AddParameters(1, PARM_DESCCRIPTION, Description);
                if (Address == "")
                    dbManager.AddParameters(2, PARM_ADDRESS, null);
                else
                    dbManager.AddParameters(2, PARM_ADDRESS, Address);
                if (City == "")
                    dbManager.AddParameters(3, PARM_CITY, null);
                else
                    dbManager.AddParameters(3, PARM_CITY, City);
                if (State == "")
                    dbManager.AddParameters(4, PARM_STATE, null);
                else
                    dbManager.AddParameters(4, PARM_STATE, State);
                if (Zip == "")
                    dbManager.AddParameters(5, PARM_ZIP_CODE, null);
                else
                    dbManager.AddParameters(5, PARM_ZIP_CODE, Zip);
                if (Telephone == "")
                    dbManager.AddParameters(6, PARM_PHONE_NO, null);
                else
                    dbManager.AddParameters(6, PARM_PHONE_NO, Telephone);
                if (PageNumber == 0)
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(8, PARM_ROWSP_PAGE, 15);
                else
                    dbManager.AddParameters(8, PARM_ROWSP_PAGE, RowsPerPage);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.InsurancePlanAddress.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSInsurance)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_ADDRESS_SEARCH, ds, ds.InsurancePlanAddress.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::LoadInsurancePlanAddressInfo", PROC_INSURANCE_PLAN_ADDRESS_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the insurance plan address information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSInsurance UpdateInsurancePlanAddressInfo(DSInsurance ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.InsurancePlanAddress.GetChanges();
                this.CreateParametersInsurancePlanAddressInfo(dbManager, ds, false);
                ds = (DSInsurance)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_ADDRESS_UPDATE, ds, ds.InsurancePlanAddress.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.InsurancePlanAddress.Rows[0][ds.InsurancePlanAddress.InsurancePlanAddressIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::UpdateInsurancePlanAddressInfo", PROC_INSURANCE_PLAN_ADDRESS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the insurance plan address information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSInsurance InsertInsurancePlanAddressInfo(DSInsurance ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.InsurancePlanAddress.GetChanges();
                CreateParametersInsurancePlanAddressInfo(dbManager, ds, true);
                ds = (DSInsurance)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_ADDRESS_INSERT, ds, ds.InsurancePlanAddress.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.InsurancePlanAddress.Rows[0][ds.InsurancePlanAddress.InsurancePlanAddressIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::InsertInsurancePlanAddressInfo", PROC_INSURANCE_PLAN_ADDRESS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the insurance plan address information.
        /// </summary>
        /// <param name="InsurancePlanAddressId">The insurance plan address identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteInsurancePlanAddressInfo(string InsurancePlanAddressId)
        {
            object returnValue;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSInsurance ds = LoadInsurancePlanAddressInfo(0, Convert.ToInt64(InsurancePlanAddressId));
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.InsurancePlanAddress;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_INSURANCE_PLAN_ADDRESS_ID, InsurancePlanAddressId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_ADDRESS_DELETE);
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.InsurancePlanAddress.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.InsurancePlanAddress.Rows[0][ds.InsurancePlanAddress.InsurancePlanAddressIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::DeleteInsurancePlanAddressInfo", PROC_INSURANCE_PLAN_ADDRESS_DELETE, ex);
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

        #region Lookups
        /// <summary>
        /// Lookups the claim flag.
        /// </summary>
        /// <returns></returns>
        public DSCodeLookup LookupClaimFlag()
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_FLAG_LOOKUP, ds, ds.ClaimFlag.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::LookupClaimFlag", PROC_CLAIM_FLAG_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the type of the claim.
        /// </summary>
        /// <returns></returns>
        public DSCodeLookup LookupClaimType(string IsActive)
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);
                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIIM_TYPE_LOOKUP, ds, ds.ClaimType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::LookupClaimType", PROC_CLAIIM_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the claim scrubbing profile.
        /// </summary>
        /// <returns></returns>
        public DSCodeLookup LookupClaimScrubbingProfile()
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_SCRUBBING_PROFILE_LOOKUP, ds, ds.ClaimScrubbingProfile.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::LookupClaimScrubbingProfile", PROC_CLAIM_SCRUBBING_PROFILE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the plan fee link.
        /// </summary>
        /// <returns></returns>
        public DSCodeLookup LookupPlanFeeLink()
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PLAN_FEE_LINK_LOOKUP, ds, ds.PlanFeeLink.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::LookupPlanFeeLink", PROC_PLAN_FEE_LINK_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the type of the plan.
        /// </summary>
        /// <returns></returns>
        public DSCodeLookup LookupPlanType(string Active)
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_IS_ACTIVE, Active);

                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PLAN_TYPE_LOOKUP, ds, ds.PlanType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::LookupPlanType", PROC_PLAN_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the insurance plan.
        /// </summary>
        /// <returns></returns>
        public DSInsuranceLookup LookupInsurancePlan(string active)
        {
            if (active == "")
                active = null;

            DSInsuranceLookup ds = new DSInsuranceLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(1, PARM_IS_ACTIVE, active);

                ds = (DSInsuranceLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_LOOKUP, ds, ds.InsurancePlan.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::LookupInsurancePlan", PROC_INSURANCE_PLAN_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the insurance plan address.
        /// </summary>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <returns></returns>
        public DSInsuranceLookup LookupInsurancePlanAddress(Int64 InsurancePlanId)
        {
            DSInsuranceLookup ds = new DSInsuranceLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_INSURANCE_PLAN_ID, InsurancePlanId);
                ds = (DSInsuranceLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_ADDRESS_LOOKUP, ds, ds.InsurancePlanAddress.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurancePlan::LookupInsurancePlanAddress", PROC_INSURANCE_PLAN_ADDRESS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
    }
}
