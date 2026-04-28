using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.ComponentModel;
using MDVision.Datasets;
using MDVision.Common.Logging;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using System.Data.SqlClient;
using MDVision.Model.Clinical.BillingInformation;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALBillingInformation
    {

        #region Variable
        
        #endregion

        #region "Stored Procedure Names"


        private const string PROC_BILLINGINFOTYPE_LOOKUP = "Clinical.sp_BillingInfoTypeLookup";

        private const string PROC_BILLINGINFOTIME_LOOKUP = "Clinical.sp_BillingInfoTimeLookup";

        private const string PROC_BILLINGINFOUPDATE = "Clinical.sp_BillingInfoUpdate";
        private const string PROC_BILLINGINFODELETE = "Clinical.sp_BillingInfoDelete";
        private const string PROC_BILLINGINFOINSERT = "Clinical.sp_BillingInfoInsert";
        private const string PROC_BILLINGINFOSELECT = "Clinical.sp_BillingInfoSelect";

        private const string PROC_BILLINGINFOSELECTBYVISITID = "Clinical.sp_BillingInfoSelectByVisitId";

        private const string PROC_BILLINGINFO_OUTOFOFFICESELECT = "Clinical.sp_BillingInfoOutOfOfficeSelect";

        private const string PROC_OUTOFOFFICE_PROVIDER_LOOKUP = "Clinical.sp_OutOfOfficeVisitProviderLookup";
        private const string PROC_OUTOFOFFICE_FACILITY_LOOKUP = "Clinical.sp_OutOfOfficeVisitFacilityLookup";


        private const string PROC_BILLINGINFOICDDELETE = "Clinical.sp_BillingInfoICDDelete";
        private const string PROC_BILLINGINFOICDINSERT = "Clinical.sp_BillingInfoICDInsert";
        private const string PROC_BILLINGINFOICDSELECT = "Clinical.sp_BillingInfoICDSelect";
        private const string PROC_BILLINGINFOICDUPDATE = "Clinical.sp_BillingInfoICDUpdate";

        private const string PROC_BILLINGINFOCPTDELETE = "Clinical.sp_BillingInfoCPTDelete";
        private const string PROC_BILLINGINFOCPTINSERT = "Clinical.sp_BillingInfoCPTInsert";
        private const string PROC_BILLINGINFOCPTSELECT = "Clinical.sp_BillingInfoCPTSelect";
        private const string PROC_BILLINGINFOCPTUPDATE = "Clinical.sp_BillingInfoCPTUpdate";
        private const string PROC_CHECK_CPT_EXSISTS_IN_ESUPPERBILL = "Clinical.sp_CheckCptExsistsInEsupperbill";

        private const string PROC_NOTES_ESUPERBILL_SELECTL = "[Clinical].[sp_NoteseSuperBillSelect]";
        private const string PROC_BILLINGINFO_PROVIDERCPTS = "[Clinical].[sp_GetProviderCPTsForBillingInfo]";


        #endregion

        #region Constructors
        /// <summary>
        /// A Constructor of the Class
        /// </summary>
        /// <param name="Obj"></param>
        public DALBillingInformation()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }

        public DALBillingInformation(SharedVariable SharedVariable)
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

        #region "Parameters"
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : parameters for BirthHx.
        /// Date : 6 january 2016
        /// </summary>
        private const string PARM_BIRTHHX_ID = "@BirthHxId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_BIRTHHX_GENERAL_ID = "@GeneralId";
        private const string PARM_BIRTHHX_MATERNALDELIVERY_ID = "@MaternalDeliveryId";
        private const string PARM_BIRTHHX_NEWBORN_ID = "@NewbornId";
        private const string PARM_BIRTHHXDATE = "@BirthHxDate";
        private const string PARM_BUNREMARKABLE = "@bUnremarkable";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_SOAPTEXT = "@SoapText";

        private const string PARM_ADMISSION_DATE = "@AdmissionDate";
        private const string PARM_DISCHARGE_DATE = "@DischargeDate";
        private const string PARM_BILLING_INFO_TYPE = "@BillingInfoType";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_RESOURCE_PROVIDER_ID = "@ResourceProviderId";
        private const string PARM_REF_PROVIDER_ID = "@RefProviderId";
        //General
        private const string PARM_HOSPITAL_NAME = "@HospitalName";
        private const string PARM_PATIENTDOB = "@PatientDOB";
        private const string PARM_LENGTH_OF_STAY_AT_HOSPITAL = "@LengthStayatHospital";
        private const string PARM_DATE_ADMITTED = "@DateAdmitted";
        private const string PARM_OBSTETRICIAN = "@ObstetricianName";
        private const string PARM_PEDIATRICIAN = "@PediatricianName";
        private const string PARM_RESPONSIBLE_PHYSICIAN = "@ResponsiblePhysicianId";

        //Maternal Delivery
        private const string PARM_GESTATION = "@Gestation";
        private const string PARM_NUMBEROF_FETUSES = "@NumberOfFetuses";
        private const string PARM_NUMBEROF_LIVINGFETUSES = "@NumberOfLivingFetuses";
        private const string PARM_LABOR_LENGTH = "@LaborLength";
        private const string PARM_DELIVERY_METHOD_ID = "@DeliveryMethodId";
        private const string PARM_MATERNAL_HISTORY_ID = "@MaternalHistoryId";
        private const string PARM_DELIVERY_PRESENTATION_ID = "@DeliveryPresentationId";

        //New Born
        private const string PARM_HEAD_CIRCUM = "@HeadCircumference";
        private const string PARM_CHEST_CIRCUM = "@ChestCircumference";
        private const string PARM_WEIGHT_AT_BIRTH = "@WeightAtBirth";
        private const string PARM_LENGTH_AT_BIRTH = "@LengthAtBirth";
        private const string PARM_APGAR_AT_BIRTH = "@ApgarAtBirth";
        private const string PARM_APGAR_AT_5MIN = "@ApgarAt5Minutes";
        private const string PARM_WEIGHT_RELEASED = "@WeightReleased";
        private const string PARM_BLOODTYPE_ID = "@PatientBloodTypeId";
        private const string PARM_PROBLEMS_AT_BIRTH_ID = "@ProblemsAtBirthId";
        private const string PARM_BFATAL_DISTRESS = "@bFetalDistress";

        /*
           Change Implement BY: Muhammad Azhar Shahzad
           Reason:Paramters For Soap Text and attachement detachment of social history with progress note
           Created Date: JAN 07, 2016
       */
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_IS_CPT_EXSISTS_IN_ESUPPERBILL = "@IsCptExsistsInEsupperbill";
        
        private const string PARM_SOAPTEXT_BY = "@SoapText";
        //end azhar changed







        private const string PARM_BILLINGINFOID = "@BillingInfoId";
        private const string PARM_ENMTYPEID = "@ENMTypeId";
        private const string PARM_ENMTIMEID = "@ENMTimeId";
        private const string PARM_ENMCPTCODE = "@ENMCPTCode";
        private const string PARM_ENMCPTDESCRIPTION = "@ENMCPTDescription";
        private const string PARM_ENMCPTUNIT = "@ENMCPTUnit";
        private const string PARM_ENMCPTDOSFROM = "@ENMCPTDOSFrom";
        private const string PARM_ENMCPTDOSTO = "@ENMCPTDOSTo";
        private const string PARM_PatientId = "@PatientId";
        private const string PARM_NotesId = "@NotesId";
        private const string PARM_VisitId = "@VisitId";
        private const string PARM_ProviderId = "@ProviderId";
        private const string PARM_Status = "@Status";
        private const string PARM_IsActive = "@IsActive";
        private const string PARM_CreatedBy = "@CreatedBy";
        private const string PARM_CreatedOn = "@CreatedOn";
        private const string PARM_ModifiedBy = "@ModifiedBy";
        private const string PARM_ModifiedOn = "@ModifiedOn";
        private const string PARM_SoapText = "@SoapText";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_BILLINGINFOCPTID = "@BillingInfoCPTId";
        private const string PARM_BILLING_TYPE = "@BillingType";
        private const string PARM_BILLING_INFO_TIME_ID = "@BillingInfoTimeId";
        

        private const string PARM_DOSFROM = "@DOSFrom";
        private const string PARM_DOSTO = "@DOSTo";
        private const string PARM_CPTCODE = "@CPTCode";
        private const string PARM_CPTDESCRIPTION = "@CPTDescription";
        private const string PARM_UNITS = "@Units";
        private const string PARM_MODIFIER1 = "@Modifier1";
        private const string PARM_MODIFIER2 = "@Modifier2";
        private const string PARM_MODIFIER3 = "@Modifier3";
        private const string PARM_MODIFIER4 = "@Modifier4";
        private const string PARM_POSCODE = "@POSCode";
        private const string PARM_ICDPOINTER1 = "@ICDPointer1";
        private const string PARM_ICDPOINTER2 = "@ICDPointer2";
        private const string PARM_ICDPOINTER3 = "@ICDPointer3";
        private const string PARM_ICDPOINTER4 = "@ICDPointer4";

        private const string PARM_BILLINGINFOICDID = "@BillingInfoICDId";
        private const string PARM_ICDTYPE = "@ICDType";
        private const string PARM_ICDCODE = "@ICDCode";
        private const string PARM_ICDCODEDESCRIPTION = "@ICDCodeDescription";
        private const string PARM_SNOMEDID = "@SNOMEDID";
        private const string PARM_SNOMEDDESCRIPTION = "@SNOMEDDescription";
        private const string PARM_LEXICODE = "@LexiCode";
        private const string PARM_LEXICODEDESCRIPTION = "@LexiCodeDescription";

        private const string PARM_CPT_SNOMEDID = "@CPTSNOMEDID";
        private const string PARM_CPT_SNOMEDDESCRIPTION = "@CPTSNOMEDDescription";

        #endregion

        #region "Billing Information Lookups"
        /// <summary>
        /// Author : Farooq Ahmad
        /// Purpose : lookup function for Billing Information Type methods.
        /// Date : 22/7/2016
        /// </summary>
        /// <returns></returns>
        public DSBillingInformationLookup LookupBillingInfoType()
        {
            DSBillingInformationLookup ds = new DSBillingInformationLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSBillingInformationLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLINGINFOTYPE_LOOKUP, ds, ds.BillingInfoType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingInformation::lookupBillingInfoType", PROC_BILLINGINFOTYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author : Farooq Ahmad
        /// Purpose : lookup function for Billing Information Time methods.
        /// Date : 22/7/2016
        /// </summary>
        /// <returns></returns>
        public DSBillingInformationLookup LookupBillingInfoTime()
        {
            DSBillingInformationLookup ds = new DSBillingInformationLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSBillingInformationLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLINGINFOTIME_LOOKUP, ds, ds.BillingInfoTime.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingInformation::LookupBillingInfoTime", PROC_BILLINGINFOTIME_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion


        #region Billing Information CRUD

        public DSBillingInformation BillingInfo_Save(IDBManager dbManager,DSBillingInformation ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                //DataTable dtTemp = ds.BillingInfo.GetChanges();
                CreateBillingInfoInsertParameters(dbManager, ds);
                CreateBillingInfoUpdateParameters(dbManager, ds);
                ds = (DSBillingInformation)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_BILLINGINFOINSERT, PROC_BILLINGINFOUPDATE, ds, ds.BillingInfo.TableName);
                //if (dtTemp != null)
                //{
                //    dtTemp.Columns.Add("PrimaryKey");
                //    for (int i = 0; i < ds.BillingInfo.Rows.Count; i++)
                //    {
                //        dtTemp.Rows[i]["PrimaryKey"] = ds.BillingInfo.Rows[i][ds.BillingInfo.BillingInfoIdColumn];
                //    }
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BillingInfo.Rows[0][ds.BillingInfo.BillingInfoIdColumn].ToString(), null, ds.BillingInfo.Rows[0][ds.BillingInfo.BillingInfoIdColumn].ToString(), false, false, false, patientId);
                //    dsDBAudit.AcceptChanges();
                //}
                ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                string Params_Insert = " INSERT PARAMETERS";
                string Params_Update = " UPDATE PARAMETERS";
                for (int i = 0; i < dbManager.InsertParameters.Length; i++)
                {
                    Params_Insert = Params_Insert + "   " + dbManager.InsertParameters[i].SourceColumn + " : " + dbManager.InsertParameters[i].Value + "   ,   ";
                }
                for (int i = 0; i < dbManager.UpdateParameters.Length; i++)
                {
                    Params_Update = Params_Update + "   " + dbManager.InsertParameters[i].SourceColumn + " : " + dbManager.InsertParameters[i].Value + "   ,   ";
                }

                MDVLogger.DALErrorLog("DALBillingInformation::BillingInfo_Save", PROC_BILLINGINFOINSERT + " " + PROC_BILLINGINFOUPDATE + "**********" + Params_Insert + "**********" + Params_Update, ex);
                throw ex;
            }
        }

        public DSBillingInformation BillingInfo_Save(DSBillingInformation ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                //DataTable dtTemp = ds.BillingInfo.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateBillingInfoInsertParameters(dbManager, ds);
                CreateBillingInfoUpdateParameters(dbManager, ds);
                ds = (DSBillingInformation)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_BILLINGINFOINSERT, PROC_BILLINGINFOUPDATE, ds, ds.BillingInfo.TableName);
                //if (dtTemp != null)
                //{
                //    dtTemp.Columns.Add("PrimaryKey");
                //    for (int i = 0; i < ds.BillingInfo.Rows.Count; i++)
                //    {
                //        dtTemp.Rows[i]["PrimaryKey"] = ds.BillingInfo.Rows[i][ds.BillingInfo.BillingInfoIdColumn];
                //    }
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BillingInfo.Rows[0][ds.BillingInfo.BillingInfoIdColumn].ToString(), null, ds.BillingInfo.Rows[0][ds.BillingInfo.BillingInfoIdColumn].ToString(), false, false, false, patientId);
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
                string Params_Insert = " INSERT PARAMETERS";
                string Params_Update = " UPDATE PARAMETERS";
                for (int i = 0; i < dbManager.InsertParameters.Length; i++)
                {
                    Params_Insert = Params_Insert + "   " + dbManager.InsertParameters[i].SourceColumn + " : " + dbManager.InsertParameters[i].Value + "   ,   ";
                }
                for (int i = 0; i < dbManager.UpdateParameters.Length; i++)
                {
                    Params_Update = Params_Update + "   " + dbManager.InsertParameters[i].SourceColumn + " : " + dbManager.InsertParameters[i].Value + "   ,   ";
                }

                MDVLogger.DALErrorLog("DALBillingInformation::BillingInfo_Save", PROC_BILLINGINFOINSERT + " " + PROC_BILLINGINFOUPDATE + "**********" + Params_Insert + "**********" + Params_Update, ex);
                throw ex;
            }
        }

        private void CreateBillingInfoUpdateParameters(IDBManager dbManager, DSBillingInformation ds)
        {
            dbManager.CreateUpdateParameters(25);

            dbManager.AddInsertUpdateParameters(0, PARM_BILLINGINFOID, ds.BillingInfo.BillingInfoIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_ENMTYPEID, ds.BillingInfo.ENMTypeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(2, PARM_ENMTIMEID, ds.BillingInfo.ENMTimeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_ENMCPTCODE, ds.BillingInfo.ENMCPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ENMCPTDESCRIPTION, ds.BillingInfo.ENMCPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_ENMCPTUNIT, ds.BillingInfo.ENMCPTUnitColumn.ColumnName, DbType.Decimal);
            dbManager.AddInsertUpdateParameters(6, PARM_ENMCPTDOSFROM, ds.BillingInfo.ENMCPTDOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_ENMCPTDOSTO, ds.BillingInfo.ENMCPTDOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_PatientId, ds.BillingInfo.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(9, PARM_NotesId, ds.BillingInfo.NotesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(10, PARM_VisitId, ds.BillingInfo.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(11, PARM_ProviderId, ds.BillingInfo.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(12, PARM_Status, ds.BillingInfo.StatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_IsActive, ds.BillingInfo.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(14, PARM_CreatedBy, ds.BillingInfo.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_CreatedOn, ds.BillingInfo.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(16, PARM_ModifiedBy, ds.BillingInfo.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_ModifiedOn, ds.BillingInfo.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(18, PARM_SoapText, ds.BillingInfo.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_BILLING_INFO_TYPE, ds.BillingInfo.BillingInfoTypeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_FACILITY_ID, ds.BillingInfo.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(21, PARM_RESOURCE_PROVIDER_ID, ds.BillingInfo.ResourceProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(22, PARM_REF_PROVIDER_ID, ds.BillingInfo.RefProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(23, PARM_ADMISSION_DATE, ds.BillingInfo.AdmissionDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(24, PARM_DISCHARGE_DATE, ds.BillingInfo.DischargeDateColumn.ColumnName, DbType.DateTime);
        }

        private void CreateBillingInfoInsertParameters(IDBManager dbManager, DSBillingInformation ds)
        {
            dbManager.CreateInsertParameters(25);

            dbManager.AddInsertUpdateParameters(0, PARM_BILLINGINFOID, ds.BillingInfo.BillingInfoIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_ENMTYPEID, ds.BillingInfo.ENMTypeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(2, PARM_ENMTIMEID, ds.BillingInfo.ENMTimeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_ENMCPTCODE, ds.BillingInfo.ENMCPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ENMCPTDESCRIPTION, ds.BillingInfo.ENMCPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_ENMCPTUNIT, ds.BillingInfo.ENMCPTUnitColumn.ColumnName, DbType.Decimal);
            dbManager.AddInsertUpdateParameters(6, PARM_ENMCPTDOSFROM, ds.BillingInfo.ENMCPTDOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_ENMCPTDOSTO, ds.BillingInfo.ENMCPTDOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_PatientId, ds.BillingInfo.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(9, PARM_NotesId, ds.BillingInfo.NotesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(10, PARM_VisitId, ds.BillingInfo.VisitIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(11, PARM_ProviderId, ds.BillingInfo.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(12, PARM_Status, ds.BillingInfo.StatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_IsActive, ds.BillingInfo.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(14, PARM_CreatedBy, ds.BillingInfo.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_CreatedOn, ds.BillingInfo.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(16, PARM_ModifiedBy, ds.BillingInfo.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_ModifiedOn, ds.BillingInfo.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(18, PARM_SoapText, ds.BillingInfo.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_BILLING_INFO_TYPE, ds.BillingInfo.BillingInfoTypeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_FACILITY_ID, ds.BillingInfo.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(21, PARM_RESOURCE_PROVIDER_ID, ds.BillingInfo.ResourceProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(22, PARM_REF_PROVIDER_ID, ds.BillingInfo.RefProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(23, PARM_ADMISSION_DATE, ds.BillingInfo.AdmissionDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(24, PARM_DISCHARGE_DATE, ds.BillingInfo.DischargeDateColumn.ColumnName, DbType.DateTime);
        }
        public string BillingInfo_DELETE(string BillingInfoId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                //DSBillingInformation dsCurrentDSBillingInformation = BillingInfo_SELECT(0, 0, Convert.ToInt64(BillingInfoId));

                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_BILLINGINFOID, Convert.ToInt64(BillingInfoId));

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLINGINFODELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                //else
                //{

                //    DataTable dtTemp = dsCurrentDSBillingInformation.BillingInfo;//.GetChanges();
                //    if (dtTemp != null)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, BillingInfoId.ToString(), null, BillingInfoId.ToString(), false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //    dbManager.CommitTransaction();
                //}
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALFamilyHx::DeleteFamilyHx", PROC_BILLINGINFODELETE, ex);
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

        public DSBillingInformation BillingInfo_SELECT(long NoteId, long PatientId, long BillingInfoId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSBillingInformation ds = new DSBillingInformation();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (BillingInfoId == 0)
                    dbManager.AddParameters(1, PARM_BILLINGINFOID, null);
                else
                    dbManager.AddParameters(1, PARM_BILLINGINFOID, BillingInfoId);

                if (NoteId == 0)
                    dbManager.AddParameters(2, PARM_NotesId, null);
                else
                    dbManager.AddParameters(2, PARM_NotesId, NoteId);

                ds = (DSBillingInformation)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLINGINFOSELECT, ds, ds.BillingInfo.TableName);
                if (ds.BillingInfo.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.BillingInfo.Rows[0]["BillingInfoId"]) > 0)
                    {

                        DataTable dtTemp = ds.BillingInfo;
                        if (dtTemp != null)
                        {
                            //if (true)
                            //{
                            //    bool isViewAction = true;
                            //    bool isPrintAcion = true;
                            //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BillingInfo.Rows[0][ds.BillingInfo.BillingInfoIdColumn].ToString(), null, ds.BillingInfo.Rows[0][ds.BillingInfo.BillingInfoIdColumn].ToString(), isViewAction, isPrintAcion);
                            //    dsDBAudit.AcceptChanges();
                            //}
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALFamilyHx::BillingInfo_SELECT", PROC_BILLINGINFOSELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSBillingInformation BillingInfo_SELECT_By_VisitId(long visitId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSBillingInformation ds = new DSBillingInformation();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_VisitId, visitId);

                ds = (DSBillingInformation)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLINGINFOSELECTBYVISITID, ds, ds.BillingInfo.TableName);
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALBillingInformation::BillingInfo_SELECT_By_VisitId", PROC_BILLINGINFOSELECTBYVISITID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }






        #endregion

        #region Billing Information ICD CRUD
        public DSBillingInformation BillingInfo_ICD_Save(IDBManager dbManager, DSBillingInformation ds, string patientId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                DataTable dtTemp = ds.BillingInfo.GetChanges();
                CreateBillingInfoICDInsertParameters(dbManager, ds);
                CreateBillingInfoICDUpdateParameters(dbManager, ds);
                ds = (DSBillingInformation)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_BILLINGINFOICDINSERT, PROC_BILLINGINFOICDUPDATE, ds, ds.BillingInfoICD.TableName);
                //if (dtTemp != null)
                //{
                //    dtTemp.Columns.Add("PrimaryKey");
                //    for (int i = 0; i < ds.BillingInfoICD.Rows.Count; i++)
                //    {
                //        dtTemp.Rows[i]["PrimaryKey"] = ds.BillingInfo.Rows[i][ds.BillingInfoICD.BillingInfoICDIdColumn];
                //    }
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BillingInfoICD.Rows[0][ds.BillingInfoICD.BillingInfoICDIdColumn].ToString(), null, ds.BillingInfoICD.Rows[0][ds.BillingInfoICD.BillingInfoICDIdColumn].ToString(), false, false, false, patientId);
                //    dsDBAudit.AcceptChanges();
                //}
                ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                string Params_Insert = " INSERT PARAMETERS";
                string Params_Update = " UPDATE PARAMETERS";
                for (int i = 0; i < dbManager.InsertParameters.Length; i++)
                {
                    Params_Insert = Params_Insert + "   " + dbManager.InsertParameters[i].SourceColumn + " : " + dbManager.InsertParameters[i].Value + "   ,   ";
                }
                for (int i = 0; i < dbManager.UpdateParameters.Length; i++)
                {
                    Params_Update = Params_Update + "   " + dbManager.InsertParameters[i].SourceColumn + " : " + dbManager.InsertParameters[i].Value + "   ,   ";
                }
                MDVLogger.DALErrorLog("DALBillingInformation::BillingInfo_ICD_Save", PROC_BILLINGINFOICDINSERT + " " + PROC_BILLINGINFOICDUPDATE + "**********" + Params_Insert + "**********" + Params_Update, ex);
                throw ex;
            }
        }

        private void CreateBillingInfoICDUpdateParameters(IDBManager dbManager, DSBillingInformation ds)
        {
            dbManager.CreateUpdateParameters(9);

            dbManager.AddInsertUpdateParameters(0, PARM_BILLINGINFOICDID, ds.BillingInfoICD.BillingInfoICDIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_BILLINGINFOID, ds.BillingInfoICD.BillingInfoIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICDTYPE, ds.BillingInfoICD.ICDTypeColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_ICDCODE, ds.BillingInfoICD.ICDCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICDCODEDESCRIPTION, ds.BillingInfoICD.ICDCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_SNOMEDID, ds.BillingInfoICD.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOMEDDESCRIPTION, ds.BillingInfoICD.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_LEXICODE, ds.BillingInfoICD.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXICODEDESCRIPTION, ds.BillingInfoICD.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            
        }

        private void CreateBillingInfoICDInsertParameters(IDBManager dbManager, DSBillingInformation ds)
        {
            dbManager.CreateInsertParameters(9);

            //dbManager.AddInsertUpdateParameters(0, PARM_BILLINGINFOID, ds.BillingInfo.BillingInfoIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(0, PARM_BILLINGINFOICDID, ds.BillingInfoICD.BillingInfoICDIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_BILLINGINFOID, ds.BillingInfoICD.BillingInfoIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICDTYPE, ds.BillingInfoICD.ICDTypeColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_ICDCODE, ds.BillingInfoICD.ICDCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_ICDCODEDESCRIPTION, ds.BillingInfoICD.ICDCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_SNOMEDID, ds.BillingInfoICD.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_SNOMEDDESCRIPTION, ds.BillingInfoICD.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_LEXICODE, ds.BillingInfoICD.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_LEXICODEDESCRIPTION, ds.BillingInfoICD.LexiCodeDescriptionColumn.ColumnName, DbType.String);
            
        }
        public string BillingInfoICD_DELETE(IDBManager dbManager,string BillingInfoICDId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            try
            {
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_BILLINGINFOICDID, Convert.ToInt64(BillingInfoICDId));

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLINGINFOICDDELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                //else
                //{

                //    DataTable dtTemp = dsCurrentDSBillingInformation.BillingInfo;//.GetChanges();
                //    if (dtTemp != null)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, BillingInfoICDId.ToString(), null, BillingInfoICDId.ToString(), false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                //dsDBAudit.RejectChanges();

                MDVLogger.DALErrorLog("DALFamilyHx::DeleteFamilyHx", PROC_BILLINGINFODELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
        }

        public DSBillingInformation BillingInfoICD_SELECT(long BillingInfoId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSBillingInformation ds = new DSBillingInformation();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (BillingInfoId == 0)
                    dbManager.AddParameters(0, PARM_BILLINGINFOID, null);
                else
                    dbManager.AddParameters(0, PARM_BILLINGINFOID, BillingInfoId);


                ds = (DSBillingInformation)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLINGINFOICDSELECT, ds, ds.BillingInfoICD.TableName);
                if (ds.BillingInfo.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.BillingInfo.Rows[0]["BillingInfoId"]) > 0)
                    {

                        //DataTable dtTemp = ds.BillingInfo;
                        //if (dtTemp != null)
                        //{
                        //    if (true)
                        //    {
                        //        bool isViewAction = true;
                        //        bool isPrintAcion = true;
                        //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BillingInfoICD.Rows[0][ds.BillingInfoICD.BillingInfoICDIdColumn].ToString(), null, ds.BillingInfoICD.Rows[0][ds.BillingInfoICD.BillingInfoICDIdColumn].ToString(), isViewAction, isPrintAcion);
                        //        dsDBAudit.AcceptChanges();
                        //    }
                        //}
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALFamilyHx::BillingInfoICD_SELECT", PROC_BILLINGINFOSELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }







        #endregion

        #region Billing Information CPT CRUD
        public DSBillingInformation BillingInfo_CPT_Save(IDBManager dbManager,DSBillingInformation ds, string patientId)
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                //DataTable dtTemp = ds.BillingInfo.GetChanges();
                CreateBillingInfoCPTInsertParameters(dbManager, ds);
                CreateBillingInfoCPTUpdateParameters(dbManager, ds);
                ds = (DSBillingInformation)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_BILLINGINFOCPTINSERT, PROC_BILLINGINFOCPTUPDATE, ds, ds.BillingInfoCPT.TableName);
                //if (dtTemp != null)
                //{
                //    dtTemp.Columns.Add("PrimaryKey");
                //    for (int i = 0; i < ds.BillingInfoCPT.Rows.Count; i++)
                //    {
                //        dtTemp.Rows[i]["PrimaryKey"] = ds.BillingInfo.Rows[i][ds.BillingInfoCPT.BillingInfoCPTIdColumn];
                //    }
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BillingInfoCPT.Rows[0][ds.BillingInfoCPT.BillingInfoCPTIdColumn].ToString(), null, ds.BillingInfoCPT.Rows[0][ds.BillingInfoCPT.BillingInfoCPTIdColumn].ToString(), false, false, false, patientId);
                //    dsDBAudit.AcceptChanges();
                //}
                ds.AcceptChanges();

                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                //dsDBAudit.RejectChanges();

                string Params_Insert = " INSERT PARAMETERS";
                string Params_Update = " UPDATE PARAMETERS";
                for (int i = 0; i < dbManager.InsertParameters.Length; i++)
                {
                    Params_Insert = Params_Insert + "   " + dbManager.InsertParameters[i].SourceColumn + " : " + dbManager.InsertParameters[i].Value + "   ,   ";
                }
                for (int i = 0; i < dbManager.UpdateParameters.Length; i++)
                {
                    Params_Update = Params_Update + "   " + dbManager.InsertParameters[i].SourceColumn + " : " + dbManager.InsertParameters[i].Value + "   ,   ";
                }

                MDVLogger.DALErrorLog("DALBillingInformation::BillingInfo_CPT_Save", PROC_BILLINGINFOCPTINSERT + " " + PROC_BILLINGINFOCPTUPDATE + "**********" + Params_Insert + "**********" + Params_Update, ex);
                throw ex;
            }
        }

        private void CreateBillingInfoCPTUpdateParameters(IDBManager dbManager, DSBillingInformation ds)
        {
            dbManager.CreateUpdateParameters(25);

            dbManager.AddInsertUpdateParameters(0, PARM_BILLINGINFOCPTID, ds.BillingInfoCPT.BillingInfoCPTIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_BILLINGINFOID, ds.BillingInfoCPT.BillingInfoIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(2, PARM_DOSFROM, ds.BillingInfoCPT.DOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(3, PARM_DOSTO, ds.BillingInfoCPT.DOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(4, PARM_CPTCODE, ds.BillingInfoCPT.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_CPTDESCRIPTION, ds.BillingInfoCPT.CPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_UNITS, ds.BillingInfoCPT.UnitsColumn.ColumnName, DbType.Decimal);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIER1, ds.BillingInfoCPT.Modifier1Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIER2, ds.BillingInfoCPT.Modifier2Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIER3, ds.BillingInfoCPT.Modifier3Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIER4, ds.BillingInfoCPT.Modifier4Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_POSCODE, ds.BillingInfoCPT.POSCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_IsActive, ds.BillingInfoCPT.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(13, PARM_CreatedBy, ds.BillingInfoCPT.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_CreatedOn, ds.BillingInfoCPT.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(15, PARM_ModifiedBy, ds.BillingInfoCPT.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_ModifiedOn, ds.BillingInfoCPT.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_ICDPOINTER1, ds.BillingInfoCPT.ICDPointer1Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_ICDPOINTER2, ds.BillingInfoCPT.ICDPointer2Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_ICDPOINTER3, ds.BillingInfoCPT.ICDPointer3Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_ICDPOINTER4, ds.BillingInfoCPT.ICDPointer4Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(21, PARM_CPT_SNOMEDID, ds.BillingInfoCPT.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(22, PARM_CPT_SNOMEDDESCRIPTION, ds.BillingInfoCPT.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(23, PARM_BILLING_TYPE, ds.BillingInfoCPT.TypeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(24, PARM_BILLING_INFO_TIME_ID, ds.BillingInfoCPT.BillingInfoTimeIdColumn.ColumnName, DbType.String);
            


        }

        private void CreateBillingInfoCPTInsertParameters(IDBManager dbManager, DSBillingInformation ds)
        {
            dbManager.CreateInsertParameters(25);

            dbManager.AddInsertUpdateParameters(0, PARM_BILLINGINFOCPTID, ds.BillingInfoCPT.BillingInfoCPTIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_BILLINGINFOID, ds.BillingInfoCPT.BillingInfoIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(2, PARM_DOSFROM, ds.BillingInfoCPT.DOSFromColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(3, PARM_DOSTO, ds.BillingInfoCPT.DOSToColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(4, PARM_CPTCODE, ds.BillingInfoCPT.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_CPTDESCRIPTION, ds.BillingInfoCPT.CPTDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_UNITS, ds.BillingInfoCPT.UnitsColumn.ColumnName, DbType.Decimal);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIER1, ds.BillingInfoCPT.Modifier1Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIER2, ds.BillingInfoCPT.Modifier2Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIER3, ds.BillingInfoCPT.Modifier3Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIER4, ds.BillingInfoCPT.Modifier4Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_POSCODE, ds.BillingInfoCPT.POSCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_IsActive, ds.BillingInfoCPT.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(13, PARM_CreatedBy, ds.BillingInfoCPT.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_CreatedOn, ds.BillingInfoCPT.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(15, PARM_ModifiedBy, ds.BillingInfoCPT.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(16, PARM_ModifiedOn, ds.BillingInfoCPT.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(17, PARM_ICDPOINTER1, ds.BillingInfoCPT.ICDPointer1Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(18, PARM_ICDPOINTER2, ds.BillingInfoCPT.ICDPointer2Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_ICDPOINTER3, ds.BillingInfoCPT.ICDPointer3Column.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_ICDPOINTER4, ds.BillingInfoCPT.ICDPointer4Column.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(21, PARM_CPT_SNOMEDID, ds.BillingInfoCPT.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(22, PARM_CPT_SNOMEDDESCRIPTION, ds.BillingInfoCPT.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(23, PARM_BILLING_TYPE, ds.BillingInfoCPT.TypeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(24, PARM_BILLING_INFO_TIME_ID, ds.BillingInfoCPT.BillingInfoTimeIdColumn.ColumnName, DbType.String);
            



        }
        public string BillingInfoCPT_DELETE(string BillingInfoId, string CPTCode)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.BeginTransaction();
                //DSBillingInformation dsCurrentDSBillingInformation = BillingInfoCPT_SELECT(Convert.ToInt64(BillingInfoCPTId));

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BILLINGINFOID, Convert.ToInt64(BillingInfoId));
                dbManager.AddParameters(1, PARM_CPTCODE, CPTCode);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLINGINFOCPTDELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {

                    //DataTable dtTemp = dsCurrentDSBillingInformation.BillingInfo;//.GetChanges();
                    //if (dtTemp != null)
                    //{
                    //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, BillingInfoCPTId.ToString(), null, BillingInfoCPTId.ToString(), false, false, true);
                    //    dsDBAudit.AcceptChanges();
                    //}
                    //dbManager.CommitTransaction();
                }
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                //dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALFamilyHx::DeleteFamilyHx", PROC_BILLINGINFODELETE, ex);
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

        public string BillingInfoCPT_DELETE(IDBManager dbManager,string BillingInfoCPTId)
        {
            //DSDBAudit dsDBAudit = new DSDBAudit();
            string returnVal = "";
            try
            {
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_BILLINGINFOCPTID, Convert.ToInt64(BillingInfoCPTId));

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLINGINFOCPTDELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {

                    //DataTable dtTemp = dsCurrentDSBillingInformation.BillingInfo;//.GetChanges();
                    //if (dtTemp != null)
                    //{
                    //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, BillingInfoCPTId.ToString(), null, BillingInfoCPTId.ToString(), false, false, true);
                    //    dsDBAudit.AcceptChanges();
                    //}
                    //dbManager.CommitTransaction();
                }
                return "";
            }
            catch (Exception ex)
            {
                //dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALFamilyHx::DeleteFamilyHx", PROC_BILLINGINFODELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
        }

        public DSBillingInformation BillingInfoCPT_SELECT(long BillingInfoId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSBillingInformation ds = new DSBillingInformation();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (BillingInfoId == 0)
                    dbManager.AddParameters(0, PARM_BILLINGINFOID, null);
                else
                    dbManager.AddParameters(0, PARM_BILLINGINFOID, BillingInfoId);


                ds = (DSBillingInformation)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLINGINFOCPTSELECT, ds, ds.BillingInfoCPT.TableName);
                if (ds.BillingInfo.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.BillingInfo.Rows[0]["BillingInfoId"]) > 0)
                    {

                        //DataTable dtTemp = ds.BillingInfo;
                        //if (dtTemp != null)
                        //{
                        //    if (true)
                        //    {
                        //        bool isViewAction = true;
                        //        bool isPrintAcion = true;
                        //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BillingInfoCPT.Rows[0][ds.BillingInfoCPT.BillingInfoCPTIdColumn].ToString(), null, ds.BillingInfoCPT.Rows[0][ds.BillingInfoCPT.BillingInfoCPTIdColumn].ToString(), isViewAction, isPrintAcion);
                        //        dsDBAudit.AcceptChanges();
                        //    }
                        //}
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALFamilyHx::BillingInfoCPT_SELECT", PROC_BILLINGINFOCPTSELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string IsCptExsistsInEsupperbill(long NotesId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_NOTE_ID, NotesId);

                dbManager.AddParameters(1, PARM_IS_CPT_EXSISTS_IN_ESUPPERBILL, "", DbType.String, ParamDirection.Output, null,1);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CHECK_CPT_EXSISTS_IN_ESUPPERBILL).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingInfo::IsCptExsistsInEsupperbill", PROC_CHECK_CPT_EXSISTS_IN_ESUPPERBILL, ex);
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

        public List<BillingInfoCPTModel> GetProviderCPTs(long BillingInfoId, long ProviderId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();         
            List<BillingInfoCPTModel> ProviderCPTList = new List<BillingInfoCPTModel>();
            BillingInfoCPTModel providerCPTs = null;           
            SqlDataReader reader = null;           
           
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (BillingInfoId == 0)
                    dbManager.AddParameters(0, PARM_BILLINGINFOID, null);
                else
                    dbManager.AddParameters(0, PARM_BILLINGINFOID, BillingInfoId);

                if (ProviderId == 0)
                    dbManager.AddParameters(1, PARM_ProviderId, null);
                else
                    dbManager.AddParameters(1, PARM_ProviderId, ProviderId);                

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_BILLINGINFO_PROVIDERCPTS);

                while (reader.Read())
                {
                    providerCPTs = new BillingInfoCPTModel();
                    providerCPTs.CPTCode = !String.IsNullOrEmpty(reader["CPTCode"].ToString()) ? reader["CPTCode"].ToString() : "";
                    providerCPTs.CPTDescription = !String.IsNullOrEmpty(reader["CPTDescription"].ToString()) ? reader["CPTDescription"].ToString() : "";
                    providerCPTs.ShowCPTCode = !String.IsNullOrEmpty(reader["ShowCPTCode"].ToString()) ? reader["ShowCPTCode"].ToString() : "";
                    ProviderCPTList.Add(providerCPTs);
                }

                return ProviderCPTList;
            }           
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingInfo::GetProviderCPTs", PROC_BILLINGINFO_PROVIDERCPTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion

        #region Out Of Office Visit
        public DSBillingInformation loadOutOfOfficeVisit(string ICDCode, string CPTCode, long PatientId, long ProviderId, long FacilityId, string Status, string DOSFrom, string DOSTo, string pageNumber="1", string rowsPerPage="15")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSBillingInformation ds = new DSBillingInformation();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(11);
                if (ICDCode == "")
                {
                    ICDCode = null;
                }

                dbManager.AddParameters(0, PARM_ICDCODE, ICDCode);
                if (CPTCode == "")
                {
                    CPTCode = null;
                }

                dbManager.AddParameters(1, PARM_CPTCODE, CPTCode);

                if (PatientId == 0)
                    dbManager.AddParameters(2, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PATIENT_ID, PatientId);

                if (ProviderId == 0)
                    dbManager.AddParameters(3, PARM_ProviderId, null);
                else
                    dbManager.AddParameters(3, PARM_ProviderId, ProviderId);

                if (FacilityId == 0)
                    dbManager.AddParameters(4, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(4, PARM_FACILITY_ID, FacilityId);

                if (Status == "")
                {
                    Status = null;
                }

                dbManager.AddParameters(5, PARM_Status, Status);
                if (DOSFrom == "")
                {
                    DOSFrom = null;
                }
                dbManager.AddParameters(6, PARM_DOSFROM, DOSFrom);
                if (DOSTo == "")
                {
                    DOSTo = null;
                }
                dbManager.AddParameters(7, PARM_DOSTO, DOSTo);
                
                if (pageNumber == "")
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == "")
                    dbManager.AddParameters(9, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(9, PARM_ROWS_PER_PAGE, rowsPerPage);

                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.OutOfOfficeVisit.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                //if (BillingInfoId == 0)
                //    dbManager.AddParameters(1, PARM_BILLINGINFOID, null);
                //else
                //    dbManager.AddParameters(1, PARM_BILLINGINFOID, BillingInfoId);

                //if (NoteId == 0)
                //    dbManager.AddParameters(2, PARM_NotesId, null);
                //else
                //    dbManager.AddParameters(2, PARM_NotesId, NoteId);

                ds = (DSBillingInformation)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLINGINFO_OUTOFOFFICESELECT, ds, ds.OutOfOfficeVisit.TableName);
                if (ds.BillingInfo.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.BillingInfo.Rows[0]["BillingInfoId"]) > 0)
                    {

                        DataTable dtTemp = ds.BillingInfo;
                        if (dtTemp != null)
                        {
                            //if (true)
                            //{
                            //    bool isViewAction = true;
                            //    bool isPrintAcion = true;
                            //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BillingInfo.Rows[0][ds.BillingInfo.BillingInfoIdColumn].ToString(), null, ds.BillingInfo.Rows[0][ds.BillingInfo.BillingInfoIdColumn].ToString(), isViewAction, isPrintAcion);
                            //    dsDBAudit.AcceptChanges();
                            //}
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();

                MDVLogger.DALErrorLog("DALFamilyHx::BillingInfo_SELECT", PROC_BILLINGINFOSELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSOutOfOfficeVisitLookup LookupOutOfOfficeVisitProvider(long PatientId)
        {
            DSOutOfOfficeVisitLookup ds = new DSOutOfOfficeVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                //dbManager.BeginTransaction();
                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);


                dbManager.Open();
                ds = (DSOutOfOfficeVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_OUTOFOFFICE_PROVIDER_LOOKUP, ds, ds.OutOfOfficeVisitProvider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingInformation::LookupOutOfOfficeVisitProvider", PROC_OUTOFOFFICE_PROVIDER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSOutOfOfficeVisitLookup LookupOutOfOfficeVisitFacility(long PatientId)
        {
            DSOutOfOfficeVisitLookup ds = new DSOutOfOfficeVisitLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(1);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                ds = (DSOutOfOfficeVisitLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_OUTOFOFFICE_FACILITY_LOOKUP, ds, ds.OutOfOfficeVisitFacility.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingInformation::LookupOutOfOfficeVisitFacility", PROC_OUTOFOFFICE_FACILITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region Legacy Notes

        public List<eSuperbill> NoteseSuperbillSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<eSuperbill> objList_eSuperbill = new List<eSuperbill>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_ESUPERBILL_SELECTL, parameters))
                {
                    while (reader.Read())
                    {
                        eSuperbill model = new eSuperbill();
                        var properties = typeof(eSuperbill).GetProperties();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }

                        objList_eSuperbill.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingInformation::NoteseSuperbillSelect", PROC_NOTES_ESUPERBILL_SELECTL, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_eSuperbill;
        }

        #endregion Legacy Notes

    }
}
