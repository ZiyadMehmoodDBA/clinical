using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALSupperBill
    {
        #region Variable

        

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALSpecialty"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALSupperBill()
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

        #region "Stored Procedure Names"

        private const string PROC_SUPPERBILL_INSERT = "Patient.sp_SuperBillsInsert";
        private const string PROC_SUPPERBILL_UPDATE = "Patient.sp_SuperBillsUpdate";
        private const string PROC_SUPPERBILL_SELECT = "Patient.sp_SuperBillsSelect";
        private const string PROC_SUPPERBILL_DELETE = "Patient.sp_SuperBillsDelete";

        private const string PROC_SUPPERBILL_TITLE_INSERT = "Patient.sp_SupBillTitleInsert";
        private const string PROC_SUPPERBILL_TITLE_UPDATE = "Patient.sp_SupBillTitleUpdate";
        private const string PROC_SUPPERBILL_TITLE_SELECT = "Patient.sp_SupBillTitleSelect";
        private const string PROC_SUPPERBILL_TITLE_DELETE = "Patient.sp_SupBillTitleDelete";

        private const string PROC_SUPPERBILL_ICD_INSERT = "Patient.sp_SupBillICDInsert";
        private const string PROC_SUPPERBILL_ICD_UPDATE = "Patient.sp_SupBillICDUpdate";
        private const string PROC_SUPPERBILL_ICD_SELECT = "Patient.sp_SupBillICDSelect";
        private const string PROC_SUPPERBILL_ICD_DELETE = "Patient.sp_SupBillICDDelete";

        private const string PROC_SUPPERBILL_CPT_INSERT = "Patient.sp_SupBillCPTInsert";
        private const string PROC_SUPPERBILL_CPT_UPDATE = "Patient.sp_SupBillCPTUpdate";
        private const string PROC_SUPPERBILL_CPT_SELECT = "Patient.sp_SupBillCPTSelect";
        private const string PROC_SUPPERBILL_CPT_DELETE = "Patient.sp_SupBillCPTDelete";

        private const string PROC_SUPPERBILL_MODIFIER_INSERT = "Patient.sp_SupBillModifierInsert";
        private const string PROC_SUPPERBILL_MODIFIER_UPDATE = "Patient.sp_SupBillModifierUpdate";
        private const string PROC_SUPPERBILL_MODIFIER_SELECT = "Patient.sp_SupBillModifierSelect";
        private const string PROC_SUPPERBILL_MODIFIER_DELETE = "Patient.sp_SupBillModifierDelete";

        private const string PROC_SUPPERBILL_LOOK_UP = "Patient.sp_SuperBillsLookup";

        #endregion

        #region "Parameters"

        private const string PARM_SUPPERBILL_ID = "@SuperBillId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_IS_ACTIVE = "@IsActive";

        private const string PARM_SB_TITLE_ID = "@SBTitleId";
        private const string PARM_TITLE_ID = "@TitleId";

        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_TITLE_DESC = "@TitleDesc";
        private const string PARM_ICD_CODE = "@ICDCode";
        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_MOD_CODE = "@MODCode";
        private const string PARM_ICD_DESC = "@ICDDesc";
        private const string PARM_CPT_DESC = "@CPTDesc";
        private const string PARM_MOD_DESC = "@MODDesc";

        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";

        private const string PARM_SBICD_ID = "@SBICDId";
        private const string PARM_SBCPT_ID = "@SBCPTId";
        private const string PARM_SBMODIFIER_ID = "@SBModifierId";
        private const string PARM_MODIFIER_CODE = "@ModifierCode";
        private const string PARM_SORT_ID = "@SortId";
        private const string PARM_TITLE_TYPE = "@TitleType";

        private const string PARM_ICD_10 = "@ICD10";
        private const string PARM_ICD_10_DESCRIPTION = "@ICD10Description";
        private const string PARM_SNOMED_ID = "@SNOMEDId";
        private const string PARM_SNMOED_DESCRIPTION = "@SNOMEDDescription";
        private const string PARM_LEXI_CODE = "@LexiCode";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Supper Bill

        #region "Support Functions"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSSupperBill ds, Boolean IsInsert)
        {


            if (IsInsert == true)
            {
                dbManager.CreateParameters(18);

                dbManager.AddParameters(0, PARM_SUPPERBILL_ID, ds.SuperBills.SuperBillIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_SB_TITLE_ID, ds.SuperBills.SBTitleIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, PARM_SHORT_NAME, ds.SuperBills.ShortNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, PARM_DESCRIPTION, ds.SuperBills.DescriptionColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, PARM_PRACTICE_ID, ds.SuperBills.PracticeIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(5, PARM_TITLE_DESC, ds.SuperBills.TitleDescColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, PARM_ICD_CODE, ds.SuperBills.ICDCodeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(7, PARM_ICD_DESC, ds.SuperBills.ICDDescColumn.ColumnName, DbType.String);
                dbManager.AddParameters(8, PARM_CPT_CODE, ds.SuperBills.CPTCodeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(9, PARM_CPT_DESC, ds.SuperBills.CPTDescColumn.ColumnName, DbType.String);
                dbManager.AddParameters(10, PARM_MOD_CODE, ds.SuperBills.MODCodeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(11, PARM_MOD_DESC, ds.SuperBills.MODDescColumn.ColumnName, DbType.String);
                dbManager.AddParameters(12, PARM_IS_ACTIVE, ds.SuperBills.IsActiveColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(13, PARM_CREATED_BY, ds.SuperBills.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(14, PARM_CREATED_ON, ds.SuperBills.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(15, PARM_MODIFIED_BY, ds.SuperBills.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(16, PARM_MODIFIED_ON, ds.SuperBills.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(17, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            }

            else
            {
                dbManager.CreateParameters(10);

                dbManager.AddParameters(0, PARM_SUPPERBILL_ID, ds.SuperBills.SuperBillIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ds.SuperBills.ShortNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_DESCRIPTION, ds.SuperBills.DescriptionColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, PARM_PRACTICE_ID, ds.SuperBills.PracticeIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.SuperBills.IsActiveColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(5, PARM_CREATED_BY, ds.SuperBills.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, PARM_CREATED_ON, ds.SuperBills.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.SuperBills.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.SuperBills.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(9, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            }



        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Load the supper bill.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSSupperBill LoadSupperBill(long SupperBillId, long TitleId, long PracticeId, string ShortName, string Description, string isActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSSupperBill ds = new DSSupperBill();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(11);

                if (SupperBillId == 0)
                    dbManager.AddParameters(0, PARM_SUPPERBILL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SUPPERBILL_ID, SupperBillId);

                if (TitleId == 0)
                    dbManager.AddParameters(1, PARM_SB_TITLE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SB_TITLE_ID, TitleId);

                if (PracticeId == 0)
                    dbManager.AddParameters(2, PARM_PRACTICE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PRACTICE_ID, PracticeId);

                dbManager.AddParameters(3, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(4, PARM_DESCRIPTION, Description);


                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(5, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(5, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                //if (SharedObj.IsAdmin)
                //    dbManager.AddParameters(5, PARM_ENTITY_ID, null);
                //else
                //    dbManager.AddParameters(5, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(6, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(7, PARM_IS_ACTIVE, isActive);
                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.SuperBills.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSSupperBill)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_SELECT, ds, ds.SuperBills.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::LoadSupperBill", PROC_SUPPERBILL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the supper table.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSSupperBill UpdateSupperBill(ref DSSupperBill ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.SuperBills.GetChanges();
                ds = (DSSupperBill)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_UPDATE, ds, ds.SuperBills.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.SuperBills.Rows[0][ds.SuperBills.SuperBillIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::UpdateSupperBill", PROC_SUPPERBILL_UPDATE, ex);
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
        /// Inserts the supper table.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSSupperBill InsertSupperBill(ref DSSupperBill ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.SuperBills.GetChanges();
                ds = (DSSupperBill)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_INSERT, ds, ds.SuperBills.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.SuperBills.Rows[0][ds.SuperBills.SuperBillIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;               
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::InsertSupperBill", PROC_SUPPERBILL_INSERT, ex);
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
        /// Deletes the supper bill.
        /// </summary>
        /// <param name="BillIds">The bill ids.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string DeleteSupperBill(long BillIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSSupperBill ds = LoadSupperBill(BillIds, 0, 0, "", "", null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.SuperBills;
                dbManager.AddParameters(0, PARM_SUPPERBILL_ID, BillIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SUPPERBILL_DELETE).ToString();
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.SuperBills.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.SuperBills.Rows[0][ds.SuperBills.SuperBillIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::DeleteSupperBill", PROC_SUPPERBILL_DELETE, ex);
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

        #region Supper Bill Title

        #region "Support Functions"

        /// <summary>
        /// Creates the parameters_ for_ supper bill title.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters_For_SupperBillTitle(IDBManager dbManager, DSSupperBill ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(ds.Tables[ds.SupBillTitle.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SB_TITLE_ID, ds.SupBillTitle.SBTitleIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SB_TITLE_ID, ds.SupBillTitle.SBTitleIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SUPPERBILL_ID, ds.SupBillTitle.SuperBillIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.SupBillTitle.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.SupBillTitle.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.SupBillTitle.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.SupBillTitle.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.SupBillTitle.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.SupBillTitle.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_TITLE_TYPE, ds.SupBillTitle.TitleTypeColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the supper bill title.
        /// </summary>
        /// <param name="TitleId">The title identifier.</param>
        /// <returns></returns>
        public DSSupperBill LoadSupperBillTitle(long TitleId, long SupperBillId)
        {
            DSSupperBill ds = new DSSupperBill();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (TitleId == 0)
                    dbManager.AddParameters(0, PARM_SB_TITLE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SB_TITLE_ID, TitleId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (SupperBillId == 0)
                    dbManager.AddParameters(3, PARM_SUPPERBILL_ID, null);
                else
                    dbManager.AddParameters(3, PARM_SUPPERBILL_ID, SupperBillId);

                ds = (DSSupperBill)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_TITLE_SELECT, ds, ds.SupBillTitle.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::LoadSupperBillTitle", PROC_SUPPERBILL_TITLE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the supper bill title.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSSupperBill UpdateSupperBillTitle(ref DSSupperBill ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters_For_SupperBillTitle(dbManager, ds, false);
                ds = (DSSupperBill)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_TITLE_UPDATE, ds, ds.SupBillTitle.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::UpdateSupperBillTitle", PROC_SUPPERBILL_TITLE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the supper bill title.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSSupperBill InsertSupperBillTitle(ref DSSupperBill ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters_For_SupperBillTitle(dbManager, ds, true);
                ds = (DSSupperBill)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_TITLE_INSERT, ds, ds.SupBillTitle.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::InsertSupperBillTitle", PROC_SUPPERBILL_TITLE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the supper bill title.
        /// </summary>
        /// <param name="TitleId">The title identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string DeleteSupperBillTitle(long TitleId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SB_TITLE_ID, TitleId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SUPPERBILL_TITLE_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::DeleteSupperBillTitle", PROC_SUPPERBILL_TITLE_DELETE, ex);
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

        #endregion

        #region Supper Bill ICD

        #region "Support Functions"

        /// <summary>
        /// Creates the parameters_ for_ supper bill icd.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters_For_SupperBillICD(IDBManager dbManager, DSSupperBill ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(15);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SBICD_ID, ds.SupBillICD.SBICDIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SBICD_ID, ds.SupBillICD.SBICDIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SB_TITLE_ID, ds.SupBillICD.SBTitleIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ICD_CODE, ds.SupBillICD.ICDCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.SupBillICD.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.SupBillICD.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.SupBillICD.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.SupBillICD.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.SupBillICD.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.SupBillICD.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_SORT_ID, ds.SupBillICD.SortIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(10, PARM_ICD_10, ds.SupBillICD.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_ICD_10_DESCRIPTION, ds.SupBillICD.ICD10DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_SNOMED_ID, ds.SupBillICD.SNOMEDIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_SNMOED_DESCRIPTION, ds.SupBillICD.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_LEXI_CODE, ds.SupBillICD.LexiCodeColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the supper bill icd.
        /// </summary>
        /// <param name="ICDId">The icd identifier.</param>
        /// <returns></returns>
        public DSSupperBill LoadSupperBillICD(long ICDId, long SupperBillId, long TitleId)
        {
            DSSupperBill ds = new DSSupperBill();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (ICDId == 0)
                    dbManager.AddParameters(0, PARM_SBICD_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SBICD_ID, ICDId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (SupperBillId == 0)
                    dbManager.AddParameters(3, PARM_SUPPERBILL_ID, null);
                else
                    dbManager.AddParameters(3, PARM_SUPPERBILL_ID, SupperBillId);

                if (TitleId == 0)
                    dbManager.AddParameters(4, PARM_TITLE_ID, null);
                else
                    dbManager.AddParameters(4, PARM_TITLE_ID, TitleId);

                ds = (DSSupperBill)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_ICD_SELECT, ds, ds.SupBillICD.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::LoadSupperBillICD", PROC_SUPPERBILL_ICD_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Updates the supper bill icd.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSSupperBill UpdateSupperBillICD(ref DSSupperBill ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters_For_SupperBillICD(dbManager, ds, false);
                ds = (DSSupperBill)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_ICD_UPDATE, ds, ds.SupBillICD.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::UpdateSupperBillICD", PROC_SUPPERBILL_ICD_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Inserts the supper bill icd.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSSupperBill InsertSupperBillICD(ref DSSupperBill ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters_For_SupperBillICD(dbManager, ds, true);
                ds = (DSSupperBill)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_ICD_INSERT, ds, ds.SupBillICD.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::InsertSupperBillICD", PROC_SUPPERBILL_ICD_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Deletes the supper bill icd.
        /// </summary>
        /// <param name="ICDId">The icd identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string DeleteSupperBillICD(long ICDId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SBICD_ID, ICDId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_SUPPERBILL_ICD_DELETE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::DeleteSupperBillICD", PROC_SUPPERBILL_ICD_DELETE, ex);
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

        #endregion

        #region Supper Bill CPT

        #region "Support Functions"

        /// <summary>
        /// Creates the parameters_ for_ supper bill CPT.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters_For_SupperBillCPT(IDBManager dbManager, DSSupperBill ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(ds.Tables[ds.SupBillCPT.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SBCPT_ID, ds.SupBillCPT.SBCPTIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SBCPT_ID, ds.SupBillCPT.SBCPTIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SB_TITLE_ID, ds.SupBillCPT.SBTitleIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_CPT_CODE, ds.SupBillCPT.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.SupBillCPT.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.SupBillCPT.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.SupBillCPT.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.SupBillCPT.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.SupBillCPT.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.SupBillCPT.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_SORT_ID, ds.SupBillCPT.SortIdColumn.ColumnName, DbType.Int32);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"


        /// <summary>
        /// Loads the supper bill CPT.
        /// </summary>
        /// <param name="CPTId">The CPT identifier.</param>
        /// <returns></returns>
        public DSSupperBill LoadSupperBillCPT(long CPTId, long SupperBillId, long TitleId)
        {
            DSSupperBill ds = new DSSupperBill();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (CPTId == 0)
                    dbManager.AddParameters(0, PARM_SBCPT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SBCPT_ID, CPTId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (SupperBillId == 0)
                    dbManager.AddParameters(3, PARM_SUPPERBILL_ID, null);
                else
                    dbManager.AddParameters(3, PARM_SUPPERBILL_ID, SupperBillId);

                if (TitleId == 0)
                    dbManager.AddParameters(4, PARM_TITLE_ID, null);
                else
                    dbManager.AddParameters(4, PARM_TITLE_ID, TitleId);

                ds = (DSSupperBill)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_CPT_SELECT, ds, ds.SupBillCPT.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::LoadSupperBillCPT", PROC_SUPPERBILL_CPT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Updates the supper bill CPT.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSSupperBill UpdateSupperBillCPT(ref DSSupperBill ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters_For_SupperBillCPT(dbManager, ds, false);
                ds = (DSSupperBill)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_CPT_UPDATE, ds, ds.SupBillCPT.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::UpdateSupperBillCPT", PROC_SUPPERBILL_CPT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Inserts the supper bill CPT.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSSupperBill InsertSupperBillCPT(ref DSSupperBill ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters_For_SupperBillCPT(dbManager, ds, true);
                ds = (DSSupperBill)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_CPT_INSERT, ds, ds.SupBillCPT.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::InsertSupperBillCPT", PROC_SUPPERBILL_CPT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        /// <summary>
        /// Deletes the supper bill CPT.
        /// </summary>
        /// <param name="CPTId">The CPT identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string DeleteSupperBillCPT(long CPTId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SBCPT_ID, CPTId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_SUPPERBILL_CPT_DELETE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::DeleteSupperBillCPT", PROC_SUPPERBILL_CPT_DELETE, ex);
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

        #endregion

        #region Supper Bill Modifier

        #region "Support Functions"

        /// <summary>
        /// Creates the parameters_ for_ supper bill modifier.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters_For_SupperBillModifier(IDBManager dbManager, DSSupperBill ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(ds.Tables[ds.SupBillModifier.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SBMODIFIER_ID, ds.SupBillModifier.SBModifierIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SBMODIFIER_ID, ds.SupBillModifier.SBModifierIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SB_TITLE_ID, ds.SupBillModifier.SBTitleIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_MODIFIER_CODE, ds.SupBillModifier.ModifierCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.SupBillModifier.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.SupBillModifier.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.SupBillModifier.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.SupBillModifier.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.SupBillModifier.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.SupBillModifier.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_SORT_ID, ds.SupBillModifier.SortIdColumn.ColumnName, DbType.Int32);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the supper bill modifier.
        /// </summary>
        /// <param name="ModifierId">The modifier identifier.</param>
        /// <returns></returns>
        public DSSupperBill LoadSupperBillModifier(long ModifierId, long SupperBillId, long TitleId)
        {
            DSSupperBill ds = new DSSupperBill();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (ModifierId == 0)
                    dbManager.AddParameters(0, PARM_SBMODIFIER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SBMODIFIER_ID, ModifierId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (SupperBillId == 0)
                    dbManager.AddParameters(3, PARM_SUPPERBILL_ID, null);
                else
                    dbManager.AddParameters(3, PARM_SUPPERBILL_ID, SupperBillId);

                if (TitleId == 0)
                    dbManager.AddParameters(4, PARM_TITLE_ID, null);
                else
                    dbManager.AddParameters(4, PARM_TITLE_ID, TitleId);

                ds = (DSSupperBill)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_MODIFIER_SELECT, ds, ds.SupBillModifier.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::LoadSupperBillModifier", PROC_SUPPERBILL_MODIFIER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Updates the supper bill modifier.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSSupperBill UpdateSupperBillModifier(ref DSSupperBill ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters_For_SupperBillModifier(dbManager, ds, false);
                ds = (DSSupperBill)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_MODIFIER_UPDATE, ds, ds.SupBillModifier.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::UpdateSupperBillModifier", PROC_SUPPERBILL_MODIFIER_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Inserts the supper bill modifier.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSSupperBill InsertSupperBillModifier(ref DSSupperBill ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters_For_SupperBillModifier(dbManager, ds, true);
                ds = (DSSupperBill)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_MODIFIER_INSERT, ds, ds.SupBillModifier.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::InsertSupperBillModifier", PROC_SUPPERBILL_MODIFIER_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Deletes the supper bill modifier.
        /// </summary>
        /// <param name="ModifierId">The modifier identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string DeleteSupperBillModifier(long ModifierId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SBMODIFIER_ID, ModifierId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_SUPPERBILL_MODIFIER_DELETE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::DeleteSupperBillModifier", PROC_SUPPERBILL_MODIFIER_DELETE, ex);
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

        #endregion

        #region Lookups
        /// <summary>
        /// Lookups the supper bill.
        /// </summary>
        /// <returns></returns>
        public DSSupperBillLookup LookupSupperBill(long PracticeId)
        {
            DSSupperBillLookup ds = new DSSupperBillLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (PracticeId != 0)
                    dbManager.AddParameters(0, PARM_PRACTICE_ID, PracticeId);
                else
                    dbManager.AddParameters(0, PARM_PRACTICE_ID, null);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSSupperBillLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SUPPERBILL_LOOK_UP, ds, ds.SuperBillsLookUp.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSupperBill::LookupSupperBill", PROC_SUPPERBILL_LOOK_UP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #endregion


    }
}


