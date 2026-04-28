using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Model;
using System.Data.SqlClient;
using MDVision.Common.Logging;
using MDVision.Model.Patient;
using MDVision.Model.Common;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Patient
{
    public class DALPatientInsurance
    {

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatientInsurance"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALPatientInsurance()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }

        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALPatientInsurance(SharedVariable SharedVariable)
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
        public DALPatientInsurance(bool isNative)
        {


        }
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_PATIENT_INSURANCE_DELETE = "Patient.sp_PatientInsuranceDelete";
        private const string PROC_PATIENT_INSURANCE_INSERT = "Patient.sp_PatientInsuranceInsert";
        private const string PROC_GET_PATIENT_INSURANCE = "Patient.sp_GetInsurancePlanName";

        private const string PROC_PATIENT_VISIT_INSURANCE_SELECT = "Patient.sp_PatientVisitInsuranceSelect";
        private const string PROC_PATIENT_INSURANCE_SELECT = "Patient.sp_PatientInsuranceSelect";
        private const string PROC_PATIENT_PATIENT_RELATIONSHIP_INFO = "Patient.Sp_GetPatientRelationshipInfo";

        private const string PROC_PATIENT_INSURANCE_UPDATE = "Patient.sp_PatientInsuranceUpdate";
        private const string PROC_PATIENT_INSURANCE_PRIORITY_UPDATE = "Patient.sp_InsurancePriorityUpdate";
        private const string PROC_PATIENT_INSURANCE_PLAN_TYPE_LOOKUP = "Patient.sp_InsurancePlanTypeLookup";
        private const string PROC_PATIENT_RELATION_LOOKUP = "Patient.sp_RelationShipLookup";
        private const string PROC_PATIENT_MSP_TYPE_LOOKUP = "Patient.sp_MSPTypeLookup";
        private const string PROC_PATIENT_INSURANCE_LOOKUP = "Patient.sp_PatientInsuranceLookup";
        private const string PROC_PATIENT_VISIT_INSURANCE_LOOKUP = "Patient.sp_PatientVisitInsuranceLookup";

        private const string PROC_PATIENT_REFERRAL_LOOKUP = "Patient.sp_PatientReferralLookup";
        private const string PROC_INSURANCE_PLAN_LOOKUP = "Provider.sp_InsurancePlanLookup";

        private const string PROC_INSURANCE_PLAN_UPDATE_NATIVE = "Patient.sp_PatientInsuranceUpdateNative";
        private const string PROC_INSURANCE_PLAN_INSERT_NATIVE = "Patient.sp_PatientInsuranceInsertNative";
        private const string PROC_INSURANCE_SYNC = "Patient.PatientInsuranceSync";
        private const string PROC_PATIENT_INSURANCE_SELECT_CCDA = "Patient.sp_PatientInsuranceSelect_CCDA";
        private const string PROC_PATIENT_REFERRAL_ALERT = "Patient.Sp_PatientReferralAlert";
        private const string PROC_INSURANCE_CHECK = "Patient.Sp_InsuranceVisitConflict";
        #endregion

        #region Parameters
        private const string PARM_VISIT_ID = "@Visitid";
        private const string PARM_INSURANCE_ID = "@InsuranceId";
        private const string PARM_PATIENT_ID = "@PatientId";

        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_MI = "@MI";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_SSN = "@SSN";
        private const string PARM_GENDER = "@Gender";
        private const string PARM_DOB = "@DOB";
        private const string PARM_ADDRESS1 = "@Address1";
        private const string PARM_ADDRESS2 = "@Address2";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP_CODE = "@ZIPCode";
        private const string PARM_ZIP_CODE_EXT = "@ZIPCodeExt";
        private const string PARM_HOME_PHONE_NO = "@HomePhoneNo";
        private const string PARM_RELATION_SHIP_ID = "@RelationShipId";
        private const string PARM_INSURANCE_PLAN_ID = "@InsurancePlanId";
        private const string PARM_INSURANCE_PLAN_ADDRESS_ID = "@InsurancePlanAddressId";
        private const string PARM_INUSRANCE_TYPE = "@InsuranceType";
        private const string PARM_PLAN_PRIORITY = "@PlanPriority";
        private const string PARM_SUBSCRIBER_ID = "@SubscriberId";
        private const string PARM_GROUP_ID = "@GroupId";
        private const string PARM_AMT_COPAY = "@AmtCopay";
        private const string PARM_COVERAGE_FROM = "@CoverageFrom";
        private const string PARM_COVERAGE_TO = "@CoverageTo";
        private const string PARM_DATE_SIGNED = "@DateSigned";
        private const string PARM_LAWYER_ID = "@LawyerId";
        private const string PARM_EMPLOYER_ID = "@EmployerId";
        private const string PARM_MS_PTYPE_ID = "@MSPTypeId";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_ELIGIBILITY_DATE = "@EligibilityDate";
        private const string PARM_ELIGIBILITY_STATUS = "@EligibilityStatus";
        private const string PARM_ASSIGN_BENEFITS = "@AssignBenefits";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_IS_FILESTREAM = "@IsFileStream";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_SPECIALIST_COPAY = "@SpecialistCopay";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_FRONT_IMAGE_STREAM = "@FrontImageStream";
        private const string PARM_BACK_IMAGE_STREAM = "@BackImageStream";
        private const string PARM_PAR_STATUS_ID = "@ParStatusId";
        private const string PARM_UNASSIGNED = "@UnAssigned";

        private const string PARM_INSURANC_CARD_IMAGE_PATH = "@InsuranceCardImagePath";
        private const string PARM_FRONT_SIDE_IMAGE_PATH = "@FrontSideImagePath";
        private const string PARM_BACK_SIDE_IMAGE_PATH = "@BackSideImagePath";
        private const string PARM_CHECK_INSURANCE = "@flag";
        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSPatient ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(46);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_INSURANCE_ID, ds.PatientInsurance.InsuranceIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_INSURANCE_ID, ds.PatientInsurance.InsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatientInsurance.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FIRST_NAME, ds.PatientInsurance.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_LAST_NAME, ds.PatientInsurance.LastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SSN, ds.PatientInsurance.SSNColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_GENDER, ds.PatientInsurance.GenderColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_DOB, ds.PatientInsurance.DOBColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_ADDRESS1, ds.PatientInsurance.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ADDRESS2, ds.PatientInsurance.Address2Column.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CITY, ds.PatientInsurance.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_STATE, ds.PatientInsurance.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_ZIP_CODE, ds.PatientInsurance.ZIPCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ZIP_CODE_EXT, ds.PatientInsurance.ZIPCodeExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_HOME_PHONE_NO, ds.PatientInsurance.HomePhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_RELATION_SHIP_ID, ds.PatientInsurance.RelationShipIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(15, PARM_INSURANCE_PLAN_ID, ds.PatientInsurance.InsurancePlanIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(16, PARM_INSURANCE_PLAN_ADDRESS_ID, ds.PatientInsurance.InsurancePlanAddressIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(17, PARM_INUSRANCE_TYPE, ds.PatientInsurance.PlanTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_PLAN_PRIORITY, ds.PatientInsurance.PlanPriorityColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(19, PARM_SUBSCRIBER_ID, ds.PatientInsurance.SubscriberIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_GROUP_ID, ds.PatientInsurance.GroupIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_AMT_COPAY, ds.PatientInsurance.AmtCopayColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(22, PARM_COVERAGE_FROM, ds.PatientInsurance.CoverageFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(23, PARM_COVERAGE_TO, ds.PatientInsurance.CoverageToColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(24, PARM_DATE_SIGNED, ds.PatientInsurance.DateSignedColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(25, PARM_LAWYER_ID, ds.PatientInsurance.LawyerIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(26, PARM_EMPLOYER_ID, ds.PatientInsurance.EmployerIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(27, PARM_MS_PTYPE_ID, ds.PatientInsurance.MSPTypeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(28, PARM_COMMENTS, ds.PatientInsurance.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_ELIGIBILITY_DATE, ds.PatientInsurance.EligibilityDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(30, PARM_ELIGIBILITY_STATUS, ds.PatientInsurance.EligibilityStatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(31, PARM_ASSIGN_BENEFITS, ds.PatientInsurance.AssignBenefitsColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(32, PARM_IS_ACTIVE, ds.PatientInsurance.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(33, PARM_CREATED_BY, ds.PatientInsurance.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(34, PARM_CREATED_ON, ds.PatientInsurance.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(35, PARM_MODIFIED_BY, ds.PatientInsurance.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(36, PARM_MODIFIED_ON, ds.PatientInsurance.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(37, PARM_SPECIALIST_COPAY, ds.PatientInsurance.SpecialistCopayColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(38, PARM_MI, ds.PatientInsurance.MIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(39, PARM_FRONT_IMAGE_STREAM, ds.PatientInsurance.FrontImageStreamColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(40, PARM_BACK_IMAGE_STREAM, ds.PatientInsurance.BackImageStreamColumn.ColumnName, DbType.Binary);
            dbManager.AddParameters(41, PARM_PAR_STATUS_ID, ds.PatientInsurance.PARStatusIdColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(42, PARM_INSURANC_CARD_IMAGE_PATH, ds.PatientInsurance.InsuranceCardImagePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(43, PARM_FRONT_SIDE_IMAGE_PATH, ds.PatientInsurance.FrontSideImagePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(44, PARM_BACK_SIDE_IMAGE_PATH, ds.PatientInsurance.BackSideImagePathColumn.ColumnName, DbType.String);
            dbManager.AddParameters(45, PARM_UNASSIGNED, ds.PatientInsurance.UnAssignedColumn.ColumnName, DbType.Boolean);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the patient insurance.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient LoadPatientInsurance(long PatientInsuranceId, long PatientId, short isFileStream, int PageNumber = 1, int RowspPage = 1000)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(13);
                if (PatientInsuranceId == 0)
                    dbManager.AddParameters(0, PARM_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_INSURANCE_ID, PatientInsuranceId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, null);
                dbManager.AddParameters(3, PARM_SUBSCRIBER_ID, null);
                dbManager.AddParameters(4, PARM_RELATION_SHIP_ID, null);
                dbManager.AddParameters(5, PARM_FIRST_NAME, null);
                dbManager.AddParameters(6, PARM_LAST_NAME, null);
                dbManager.AddParameters(7, PARM_DOB, null);
                dbManager.AddParameters(8, PARM_GENDER, null);
                dbManager.AddParameters(9, PARM_IS_FILESTREAM, isFileStream);
                //if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                //    dbManager.AddParameters(10, PARM_ENTITY_ID, null);
                //else
                dbManager.AddParameters(10, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (PageNumber == 0)
                    dbManager.AddParameters(11, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(11, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(12, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(12, PARM_ROWS_PERPAGE, RowspPage);


                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_SELECT, ds, ds.PatientInsurance.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LoadPatientInsurance", PROC_PATIENT_INSURANCE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient GetPatientRelationshipInfo(long PatientId)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_PATIENT_RELATIONSHIP_INFO, ds, ds.Patients.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::GetPatientRelationshipInfo", PROC_PATIENT_PATIENT_RELATIONSHIP_INFO, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        //if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
        //    dbManager.AddParameters(10, PARM_ENTITY_ID, null);
        //else





        public DSPatient LoadPatientVisitInsurance(long visitId, long patientId, int PageNumber = 1, int RowspPage = 1000)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(12);
                if (visitId == 0)
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, visitId);

                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);

                dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, null);
                dbManager.AddParameters(3, PARM_SUBSCRIBER_ID, null);
                dbManager.AddParameters(4, PARM_RELATION_SHIP_ID, null);
                dbManager.AddParameters(5, PARM_FIRST_NAME, null);
                dbManager.AddParameters(6, PARM_LAST_NAME, null);
                dbManager.AddParameters(7, PARM_DOB, null);
                dbManager.AddParameters(8, PARM_GENDER, null);

                dbManager.AddParameters(9, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (PageNumber == 0)
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(11, PARM_ROWS_PERPAGE, RowspPage);


                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISIT_INSURANCE_SELECT, ds, ds.PatientInsurance.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LoadPatientVisitInsurance", PROC_PATIENT_VISIT_INSURANCE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="PatientInsuranceId"></param>
        /// <param name="PatientId"></param>
        /// <param name="isFileStream"></param>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context</param>
        /// <param name="PageNumber"></param>
        /// <param name="RowspPage"></param>
        /// <returns></returns>
        public DSPatient LoadPatientInsurance(long PatientInsuranceId, long PatientId, short isFileStream, SharedVariable SharedVariable, int PageNumber = 1, int RowspPage = 1000)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(13);
                if (PatientInsuranceId == 0)
                    dbManager.AddParameters(0, PARM_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_INSURANCE_ID, PatientInsuranceId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, null);
                dbManager.AddParameters(3, PARM_SUBSCRIBER_ID, null);
                dbManager.AddParameters(4, PARM_RELATION_SHIP_ID, null);
                dbManager.AddParameters(5, PARM_FIRST_NAME, null);
                dbManager.AddParameters(6, PARM_LAST_NAME, null);
                dbManager.AddParameters(7, PARM_DOB, null);
                dbManager.AddParameters(8, PARM_GENDER, null);
                dbManager.AddParameters(9, PARM_IS_FILESTREAM, isFileStream);
                //if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                //    dbManager.AddParameters(10, PARM_ENTITY_ID, null);
                //else
                dbManager.AddParameters(10, PARM_ENTITY_ID, SharedVariable.EntityId);

                if (PageNumber == 0)
                    dbManager.AddParameters(11, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(11, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(12, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(12, PARM_ROWS_PERPAGE, RowspPage);


                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_SELECT, ds, ds.PatientInsurance.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALPatientInsurance::LoadPatientInsuranceService", PROC_PATIENT_INSURANCE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Updates the patient insurance.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient UpdatePatientInsurance(DSPatient ds, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager(); ;
                try
                {
                    DataTable dtTemp = ds.PatientInsurance.GetChanges();

                    dbManager.Open();
                    this.CreateParameters(dbManager, ds, false);
                    ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_UPDATE, ds, ds.PatientInsurance.TableName);

                    ds.AcceptChanges();
                    PatientInsuranceDBAudit(ds, dtTemp, "UPDATE");
                    return ds;
                }
                catch (Exception ex)
                {
                    DALUsersActivity obj = new DALUsersActivity();
                    obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_PATIENT_INSURANCE, false, "Error While updating Patient Insurance : " + ex.ToString());
                    MDVLogger.DALErrorLog("DALPatientInsurance::UpdatePatientInsurance", PROC_PATIENT_INSURANCE_UPDATE, ex);
                    throw ex;
                }
                finally
                {
                    dbManager.Dispose();
                }
            }
            else
            {
                try
                {
                    this.CreateParameters(dbManager, ds, false);
                    ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_UPDATE, ds, ds.PatientInsurance.TableName);
                    ds.AcceptChanges();
                    return ds;
                }

                catch (Exception ex)
                {
                    DALUsersActivity obj = new DALUsersActivity();
                    obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_PATIENT_INSURANCE, false, "Error While updating Patient Insurance : " + ex.ToString());
                    MDVLogger.DALErrorLog("DALPatientInsurance::UpdatePatientInsurance", PROC_PATIENT_INSURANCE_UPDATE, ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <returns></returns>
        public DSPatient UpdatePatientInsurance(DSPatient ds, SharedVariable SharedVariable)
        {
            DALUsersActivity obj = new DALUsersActivity(SharedVariable);
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                DataTable dtTemp = ds.PatientInsurance.GetChanges();

                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_UPDATE, ds, ds.PatientInsurance.TableName);

                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit(SharedVariable).InsertDBAudit(SharedVariable, dtTemp, dbManager, ds.PatientInsurance.Rows[0][ds.PatientInsurance.InsuranceIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }

                obj.InsertUsersActivityLog(SharedVariable, DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_PATIENT_INSURANCE, true, "Update Patient Insurance", ds.Tables[ds.PatientInsurance.TableName].Rows[0][ds.PatientInsurance.InsuranceIdColumn.ColumnName].ToString());

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(SharedVariable, DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_PATIENT_INSURANCE, false, "Error While updating Patient Insurance : " + ex.ToString());
                MDVLogger.DALErrorLog(SharedVariable, "DALPatientInsurance::UpdatePatientInsuranceService", PROC_PATIENT_INSURANCE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Deletes the patient insurance.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeletePatientInsurance(string PatientId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::DeletePatientInsurance", PROC_PATIENT_INSURANCE_DELETE, ex);
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
        /// Inserts the Patient Insurance.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSPatient InsertPatientInsurance(DSPatient ds, IDBManager dbManager = null)
        {
            if (dbManager == null)
            {
                dbManager = ClientConfiguration.GetDBManager();
                try
                {
                    DataTable dtTemp = ds.PatientInsurance.GetChanges();
                    dbManager.Open();
                    ds = InsertPatientInsuranceConcrete(ds, dbManager);

                    ds.AcceptChanges();

                    PatientInsuranceDBAudit(ds, dtTemp, "INSERT");
                    return ds;
                }
                catch (Exception ex)
                {
                    DALUsersActivity obj = new DALUsersActivity();
                    obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_PATIENT_INSURANCE, false, "Error While inserting Patient Insurance : " + ex.ToString());
                    MDVLogger.DALErrorLog("DALPatientInsurance::InsertPatientInsurance", PROC_PATIENT_INSURANCE_INSERT, ex);
                    throw ex;
                }
                finally
                {
                    dbManager.Dispose();
                }
            }
            else
            {
                try
                {
                    ds = InsertPatientInsuranceConcrete(ds, dbManager);
                    ds.AcceptChanges();
                    return ds;
                }
                catch (Exception ex)
                {
                    DALUsersActivity obj = new DALUsersActivity();
                    obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_PATIENT_INSURANCE, false, "Error While inserting Patient Insurance : " + ex.ToString());
                    MDVLogger.DALErrorLog("DALPatientInsurance::InsertPatientInsurance", PROC_PATIENT_INSURANCE_INSERT, ex);
                    throw ex;
                }

            }
        }


        private DSPatient InsertPatientInsuranceConcrete(DSPatient ds, IDBManager dbManager = null)
        {
            CreateParameters(dbManager, ds, true);
            ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_INSERT, ds, ds.PatientInsurance.TableName);
            return ds;
        }

        public void PatientInsuranceDBAudit(DSPatient ds, DataTable dtTemp, string Mode)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                DALUsersActivity obj = new DALUsersActivity();
                DSDBAudit dsDBAudit = new DSDBAudit();

                // DB Audit flow after Commit Transaction 
                if (Mode == "INSERT")
                {
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientInsurance.Rows[0][ds.PatientInsurance.InsuranceIdColumn].ToString());
                        dsDBAudit.AcceptChanges();
                    }
                    obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_PATIENT_INSURANCE, true, "Insert Patient Insurance", ds.Tables[ds.PatientInsurance.TableName].Rows[0][ds.PatientInsurance.InsuranceIdColumn.ColumnName].ToString());
                }
                else
                {
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientInsurance.Rows[0][ds.PatientInsurance.InsuranceIdColumn].ToString());
                        dsDBAudit.AcceptChanges();
                    }

                    obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_PATIENT_INSURANCE, true, "Update Patient Insurance", ds.Tables[ds.PatientInsurance.TableName].Rows[0][ds.PatientInsurance.InsuranceIdColumn.ColumnName].ToString());
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::PatientInsuranceDBAudit", PROC_PATIENT_INSURANCE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPatient GetInsuranceName(string patientInsuranceId)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_INSURANCE_ID, patientInsuranceId);
                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_PATIENT_INSURANCE, ds, ds.InsurancePlan.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::GetInsuranceName", PROC_GET_PATIENT_INSURANCE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Updates the patient insurances priorities.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="InsuranceIDs">The insurance i ds.</param>
        /// <returns></returns>
        public string UpdatePatientInsurancesPriorities(Int64 PatientId, string InsuranceIDs)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_INSURANCE_ID, InsuranceIDs);
                dbManager.AddParameters(2, PARM_CREATED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                int res = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_PRIORITY_UPDATE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::UpdatePatientInsurancesPriorities", PROC_PATIENT_INSURANCE_PRIORITY_UPDATE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdatePatientVisitInsurance(Int64 PatientId, Int64 VisitId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_VISIT_ID, VisitId);
                dbManager.AddParameters(2, PARM_CREATED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                dbManager.AddParameters(3, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                int res = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_INSURANCE_SYNC);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::UpdatePatientVisitInsurance", PROC_INSURANCE_SYNC, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        public int CheckPatientVisitInsurance(Int64 PatientId, Int64 VisitId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(1, PARM_VISIT_ID, VisitId);
                dbManager.AddParameters(2, PARM_CHECK_INSURANCE, "", DbType.Int32, ParamDirection.Output, null, 255);

                int res = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_INSURANCE_CHECK);
                return int.Parse(dbManager.Parameters[2].Value.ToString());
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::CheckPatientVisitInsurance", PROC_INSURANCE_SYNC, ex);
                return 0;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region "Lookups"


        /// <summary>
        /// Lookups the type of the insurance plan.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupInsurancePlanType()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_PLAN_TYPE_LOOKUP, ds, ds.InsurancePlanType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LookupInsurancePlanType", PROC_PATIENT_INSURANCE_PLAN_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<InsurancePlanTypeModel> LookupInsurancePlanTypeDemographic()
        {
            List<InsurancePlanTypeModel> listAllergies = new List<InsurancePlanTypeModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_PLAN_TYPE_LOOKUP);

                InsurancePlanTypeModel model = null;
                while (reader.Read())
                {
                    model = new InsurancePlanTypeModel();
                    model.Id = reader["Id"].ToString();
                    model.Description = reader["Description"].ToString();

                    listAllergies.Add(model);
                }

                return listAllergies;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LookupInsurancePlanTypeDemographic", PROC_PATIENT_INSURANCE_PLAN_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Lookups the patient relation.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupPatientRelation()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_RELATION_LOOKUP, ds, ds.RelationShip.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LookupPatientRelation", PROC_PATIENT_RELATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Lookups the type of the MSP.
        /// </summary>
        /// <returns>DSPatientLookups.</returns>
        public DSPatientLookups LookupMSPType()
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_MSP_TYPE_LOOKUP, ds, ds.MSPType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LookupMSPType", PROC_PATIENT_MSP_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<MSPTypeModel> LookupMSPTypeDemographic()
        {
            List<MSPTypeModel> listAllergies = new List<MSPTypeModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_MSP_TYPE_LOOKUP);

                MSPTypeModel model = null;
                while (reader.Read())
                {
                    model = new MSPTypeModel();
                    model.Id = reader["Id"].ToString();
                    model.Description = reader["Description"].ToString();

                    listAllergies.Add(model);
                }

                return listAllergies;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LookupMSPTypeDemographic", PROC_PATIENT_MSP_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public DSPatientLookups LookupPatientInsurance(long PatientId, string IsActive)
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (!string.IsNullOrEmpty(IsActive))
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                else
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, null);


                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_LOOKUP, ds, ds.PatientInsurance.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LookupPatientInsurance", PROC_PATIENT_INSURANCE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<GenericLookupModel> LookupPatientVisitInsurance(long VisitId, string IsActive)
        {
            List<GenericLookupModel> list = new List<GenericLookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (VisitId == 0)
                    dbManager.AddParameters(0, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (!string.IsNullOrEmpty(IsActive))
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                else
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, null);


                var reader = dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_VISIT_INSURANCE_LOOKUP);
                list.Add(new GenericLookupModel() { Name = "- Select -", Value = "" });
                while (reader.Read())
                {
                    GenericLookupModel obj = new GenericLookupModel()
                    {
                        Name = MDVUtility.ToStr(reader["InsurancePlanName"]),
                        Value = MDVUtility.ToStr(reader["InsuranceId"]),
                        RefValue = MDVUtility.ToStr(reader["PlanPriority"]),
                        RefName = MDVUtility.ToStr(reader["AmtCopay"]),
                        IsActive = "",
                        ExValue = MDVUtility.ToStr(reader["InsurancePlanId"])
                    };
                    list.Add(obj);
                }

                // list = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_VISIT_INSURANCE_LOOKUP, list, list.PatientInsurance.TableName);
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LookupPatientVisitInsurance", PROC_PATIENT_VISIT_INSURANCE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPatientLookups LookupPatientReferral(string IsActive)
        {
            DSPatientLookups ds = new DSPatientLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (MDVSession.Current.IsAdmin)
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (!string.IsNullOrEmpty(IsActive))
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                else
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, null);


                ds = (DSPatientLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_REFERRAL_LOOKUP, ds, ds.PatientReferral.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LookupPatientReferral", PROC_PATIENT_REFERRAL_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion

        #region "Native Functions"

        public List<InsuranceLookupModel> LookupInsurancePlan(string ShortName, long EntityId, string IsActive)
        {
            List<InsuranceLookupModel> InsuranceList = new List<InsuranceLookupModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                if (IsActive == "")
                    IsActive = null;
                if (ShortName == "")
                    ShortName = null;
                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(1, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_LOOKUP);

                InsuranceLookupModel model = null;
                while (reader.Read())
                {
                    model = new InsuranceLookupModel();
                    model.InsurancePlanId = !String.IsNullOrEmpty(reader["InsurancePlanId"].ToString()) ? reader["InsurancePlanId"].ToString() : "";
                    model.ShortName = !String.IsNullOrEmpty(reader["ShortName"].ToString()) ? reader["ShortName"].ToString() : "";

                    InsuranceList.Add(model);
                }


                return InsuranceList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LookupInsurancePlan", PROC_INSURANCE_PLAN_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<PatientInsuranceModel> LoadPatientInsuranceNative(long InsuranceId, long PatientId, long EntityId)
        {
            List<PatientInsuranceModel> InsuranceList = new List<PatientInsuranceModel>();
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(11);
                if (InsuranceId == 0)
                    dbManager.AddParameters(0, PARM_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_INSURANCE_ID, InsuranceId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, null);
                dbManager.AddParameters(3, PARM_SUBSCRIBER_ID, null);
                dbManager.AddParameters(4, PARM_RELATION_SHIP_ID, null);
                dbManager.AddParameters(5, PARM_FIRST_NAME, null);
                dbManager.AddParameters(6, PARM_LAST_NAME, null);
                dbManager.AddParameters(7, PARM_DOB, null);
                dbManager.AddParameters(8, PARM_GENDER, null);
                dbManager.AddParameters(9, PARM_IS_FILESTREAM, "0");
                //if (EntityId == 0)
                //    dbManager.AddParameters(10, PARM_ENTITY_ID, EntityId);
                //else
                dbManager.AddParameters(10, PARM_ENTITY_ID, null);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_SELECT);

                PatientInsuranceModel model = null;
                while (reader.Read())
                {
                    model = new PatientInsuranceModel();
                    model.InsuranceId = !String.IsNullOrEmpty(reader["InsuranceId"].ToString()) ? reader["InsuranceId"].ToString() : "";
                    model.SubscriberId = !String.IsNullOrEmpty(reader["SubscriberId"].ToString()) ? reader["SubscriberId"].ToString() : "";
                    model.GroupId = !String.IsNullOrEmpty(reader["GroupId"].ToString()) ? reader["GroupId"].ToString() : "";
                    model.SubscriberDOB = !String.IsNullOrEmpty(reader["DOB"].ToString()) ? reader["DOB"].ToString() : "";
                    model.SubscriberFirstName = !String.IsNullOrEmpty(reader["FirstName"].ToString()) ? reader["FirstName"].ToString() : "";
                    model.SubscriberMI = !String.IsNullOrEmpty(reader["MI"].ToString()) ? reader["MI"].ToString() : "";
                    model.SubscriberLastName = !String.IsNullOrEmpty(reader["LastName"].ToString()) ? reader["LastName"].ToString() : "";
                    model.PlanPriority = !String.IsNullOrEmpty(reader["PlanPriority"].ToString()) ? reader["PlanPriority"].ToString() : "";
                    model.InsurancePlanId = !String.IsNullOrEmpty(reader["InsurancePlanId"].ToString()) ? reader["InsurancePlanId"].ToString() : "";
                    model.InsurancePlanName = !String.IsNullOrEmpty(reader["InsurancePlanName"].ToString()) ? reader["InsurancePlanName"].ToString() : "";
                    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                    model.RelationShipId = !String.IsNullOrEmpty(reader["RelationShipId"].ToString()) ? reader["RelationShipId"].ToString() : "";
                    model.RelationName = !String.IsNullOrEmpty(reader["RelationName"].ToString()) ? reader["RelationName"].ToString() : "";
                    model.Gender = !String.IsNullOrEmpty(reader["Gender"].ToString()) ? reader["Gender"].ToString() : "";
                    if (Convert.ToString(reader["IsActive"]) == "True")
                        model.IsActive = true;
                    else
                        model.IsActive = false;
                    InsuranceList.Add(model);
                }


                return InsuranceList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LoadPatientInsuranceNative", PROC_PATIENT_INSURANCE_SELECT, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        #region "Save & Update Functions"

        private void createParametersNative(IDBManager dbManager, PatientInsuranceModel model, Boolean IsInsert)
        {

            if (IsInsert == true)
            {
                dbManager.CreateParameters(17);
                dbManager.AddParameters(0, PARM_INSURANCE_ID, model.InsuranceId);
                dbManager.AddParameters(14, PARM_IS_ACTIVE, 1);
                dbManager.AddParameters(15, PARM_CREATED_BY, model.ModifiedBy);
                dbManager.AddParameters(16, PARM_CREATED_ON, model.ModifiedOn);
            }
            else
            {
                dbManager.CreateParameters(14);
                dbManager.AddParameters(0, PARM_INSURANCE_ID, model.InsuranceId);
            }
            dbManager.AddParameters(1, PARM_PATIENT_ID, model.PatientId);
            dbManager.AddParameters(2, PARM_INSURANCE_PLAN_ID, model.InsurancePlanId);
            dbManager.AddParameters(3, PARM_FIRST_NAME, model.SubscriberFirstName);
            dbManager.AddParameters(4, PARM_MI, model.SubscriberMI);
            dbManager.AddParameters(5, PARM_LAST_NAME, model.SubscriberLastName);
            dbManager.AddParameters(6, PARM_RELATION_SHIP_ID, model.RelationShipId);
            dbManager.AddParameters(7, PARM_DOB, model.SubscriberDOB);
            dbManager.AddParameters(8, PARM_SUBSCRIBER_ID, model.SubscriberId);
            dbManager.AddParameters(9, PARM_GROUP_ID, model.GroupId);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, model.ModifiedBy);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, model.ModifiedOn);
            dbManager.AddParameters(12, PARM_PLAN_PRIORITY, model.PlanPriority);
            dbManager.AddParameters(13, PARM_GENDER, model.Gender);
        }

        public PatientInsuranceModel insertPatientInsuranceNative(IDBManager dbManager, PatientInsuranceModel model)
        {
            SqlDataReader reader = null;
            PatientInsuranceModel patientInsuranceModel = null;
            //string ConnectionString = "";
            //IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            try
            {
                //dbManager.Open();
                this.createParametersNative(dbManager, model, true);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_INSERT_NATIVE);
                while (reader.Read())
                {
                    patientInsuranceModel = new PatientInsuranceModel();
                    patientInsuranceModel.InsuranceId = !String.IsNullOrEmpty(reader["InsuranceId"].ToString()) ? reader["InsuranceId"].ToString() : "";
                    patientInsuranceModel.PlanPriority = !String.IsNullOrEmpty(reader["PlanPriority"].ToString()) ? reader["PlanPriority"].ToString() : "";
                    patientInsuranceModel.InsurancePlanId = !String.IsNullOrEmpty(reader["InsurancePlanId"].ToString()) ? reader["InsurancePlanId"].ToString() : "";
                    patientInsuranceModel.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                }
                return patientInsuranceModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::insertPatientInsuranceNative", "", ex);
                throw ex;
            }
            finally
            {
                reader.Close();
            }
        }
        public PatientInsuranceModel updatePatientInsuranceNative(IDBManager dbManager, PatientInsuranceModel model)
        {
            //string ConnectionString = "";
            //IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            PatientInsuranceModel patientInsuranceModel = null;
            SqlDataReader reader = null;
            try
            {
                //dbManager.Open();
                createParametersNative(dbManager, model, false);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_INSURANCE_PLAN_UPDATE_NATIVE);
                while (reader.Read())
                {
                    patientInsuranceModel = new PatientInsuranceModel();
                    patientInsuranceModel.InsuranceId = !String.IsNullOrEmpty(reader["InsuranceId"].ToString()) ? reader["InsuranceId"].ToString() : "";
                    patientInsuranceModel.PlanPriority = !String.IsNullOrEmpty(reader["PlanPriority"].ToString()) ? reader["PlanPriority"].ToString() : "";
                    patientInsuranceModel.InsurancePlanId = !String.IsNullOrEmpty(reader["InsurancePlanId"].ToString()) ? reader["InsurancePlanId"].ToString() : "";
                    patientInsuranceModel.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                }
                return patientInsuranceModel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::updatePatientInsuranceNative", "", ex);
                throw ex;
            }
            finally
            {
                reader.Close();
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// Loads the patient insurance for ccda.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns>DSPatient.</returns>
        public DSNotes LoadPatientInsuranceCCDA(long PatientId)
        {
            DSNotes ds = new DSNotes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                ds = (DSNotes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_INSURANCE_SELECT_CCDA, ds, ds.PatientInsurance_CCDA.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::LoadPatientInsuranceCCDA", PROC_PATIENT_INSURANCE_SELECT_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public object patientReffralALert(long patientInsuranceId, long PatientId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, "@InsuranceId", patientInsuranceId);
                if (PatientId == 0)
                {
                    dbManager.AddParameters(1, "@PatientId", DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, "@PatientId", PatientId);
                }

                dbManager.AddParameters(2, "@IsAlertDisplay", null, DbType.Int32, ParamDirection.Output);


                object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_REFERRAL_ALERT);
                return returnVal != null ? returnVal.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::patientReffralALert", PROC_PATIENT_REFERRAL_ALERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
    }
}
