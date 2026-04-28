using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALICD
    {
        #region Variable
        
        #endregion

        #region " Stored Procedure Names"
        private const string PROC_ICD_INSERT = "Provider.sp_ICDInsert";
        private const string PROC_ICD_UPDATE = "Provider.sp_ICDUpdate";
        private const string PROC_ICD_DELETE = "Provider.sp_ICDDelete";
        private const string PROC_ICD_SELECT = "Provider.sp_ICDSelect";
        private const string PROC_ICD_LOOKUP = "Provider.sp_ICDLookup";

        private const string PROC_ICDLookup_INSERT = "CCM.sp_ICDLookupInsert";
        

        #endregion

        #region "Parameters"
        private const string PARM_ICD_ID = "@ICDId";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_ICD_9 = "@ICD9";
        private const string PARM_ACTIVELY_USED = "@ActivelyUsed";
        private const string PARM_VALID = "@Valid";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_IS_ICD10 = "@IsICD10";
        private const string PARM_ICD_CODE = "@ICDCode";
        private const string PARM_BIT = "@Bit";

        private const string PARM_ICD_10 = "@ICD10";
        private const string PARM_ICD10DESCRIPTION = "@ICD10Description";
        private const string PARM_SNOMED_ID = "@SNOMEDId";
        private const string PARM_SNOMED_DESCRIPTION = "@SNOMEDDescription";
        private const string PARM_LEXICODE = "@LexiCode";


        private const string PARM_ICDLookupId = "@ICDLookupId";
        private const string PARM_SNOMEDID_ = "@SNOMEDID";
        private const string PARM_ICD9DESCRIPTION_ = "@ICD9_DESCRIPTION";
        private const string PARM_ICD10DESCRIPTION_ = "@ICD10_DESCRIPTION";
        private const string PARM_SNOMEDDESCRIPTION_ = "@SNOMED_DESCRIPTION";

        #endregion

        #region Constructors
        public DALICD()
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
            dbManager.CreateParameters(18);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ICD_ID, ds.ICD.ICDIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ICD_ID, ds.ICD.ICDIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.ICD.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ICD_9, ds.ICD.ICD9Column.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.ICD.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ACTIVELY_USED, ds.ICD.ActivelyUsedColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_VALID, ds.ICD.ValidColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.ICD.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.ICD.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.ICD.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.ICD.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.ICD.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_ENTITY_ID, ds.ICD.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_IS_ICD10, ds.ICD.IsICD10Column.ColumnName, DbType.Byte);

            dbManager.AddParameters(13, PARM_ICD_10, ds.ICD.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_ICD10DESCRIPTION, ds.ICD.ICD10DescriptionColumn.ColumnName, DbType.String);

            dbManager.AddParameters(15, PARM_SNOMED_ID, ds.ICD.SNOMEDIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_SNOMED_DESCRIPTION, ds.ICD.SNOMEDDescriptionColumn.ColumnName, DbType.String);

            dbManager.AddParameters(17, PARM_LEXICODE, ds.ICD.LexiCodeColumn.ColumnName, DbType.String);
        }

        private void CreateParameters_ICDLookUp(IDBManager dbManager, DSCodeLookup ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);
            
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ICDLookupId, ds.ICDLookup.IdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ICDLookupId, ds.ICDLookup.IdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ICD_9, ds.ICDLookup.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ICD9DESCRIPTION_, ds.ICDLookup.ICD9_DescriptionColumn.ColumnName, DbType.String);

            dbManager.AddParameters(3, PARM_ICD_10, ds.ICDLookup.ICD10Column.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ICD10DESCRIPTION_, ds.ICDLookup.ICD10_DescriptionColumn.ColumnName, DbType.String);

            dbManager.AddParameters(5, PARM_SNOMEDID_, ds.ICDLookup.SNOMEDIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_SNOMEDDESCRIPTION_, ds.ICDLookup.SNOMED_DescriptionColumn.ColumnName, DbType.String);
            
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the icd.
        /// </summary>
        /// <param name="ICDId">The icd identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="ICD9">The ic d9.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public DSCodes LoadICD(long ICDId, string ShortName, string Description, string ICD9, string ICD10Description, string ICD10, string SNOMEDDescription, string SNOMED, string IsActive, Int64 PageNumber, Int64 RowsPerPage)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                if (ICD9 == "")
                    ICD9 = null;

                if (ICD10Description == "")
                    ICD10Description = null;

                if (ICD10 == "")
                    ICD10 = null;

                if (SNOMEDDescription == "")
                    SNOMEDDescription = null;

                if (SNOMED == "")
                    SNOMED = null;

                if (IsActive == "") 
                    IsActive = null;

                //if (EntityId == "")
                //    EntityId = null;

                dbManager.Open();
                dbManager.CreateParameters(13);

                if (ICDId <= 0)
                    dbManager.AddParameters(0, PARM_ICD_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ICD_ID, ICDId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_ICD_9, ICD9);

                dbManager.AddParameters(4, PARM_ICD10DESCRIPTION, ICD10Description);
                dbManager.AddParameters(5, PARM_ICD_10, ICD10);

                dbManager.AddParameters(6, PARM_SNOMED_DESCRIPTION, SNOMEDDescription);
                dbManager.AddParameters(7, PARM_SNOMED_ID, SNOMED);

                dbManager.AddParameters(8, PARM_IS_ACTIVE, IsActive);

                //if (IsICD10 == "")
                //    IsICD10 = null;
                //dbManager.AddParameters(5, PARM_IS_ICD10, IsICD10);

                //if (EntityId == null)
                //{
                //    if (SharedObj.IsAdmin)
                //        dbManager.AddParameters(6, PARM_ENTITY_ID, EntityId);
                //    else
                //        dbManager.AddParameters(6, PARM_ENTITY_ID, SharedObj.SeletedEntityId);
                //}
                //else
                //    dbManager.AddParameters(6, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(9, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (PageNumber == 0)
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(10, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage == 0)
                    dbManager.AddParameters(11, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(11, PARM_ROWSP_PAGE, RowsPerPage);

                dbManager.AddParameters(12, PARM_RECORD_COUNT, ds.ICD.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ICD_SELECT, ds, ds.ICD.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALICD::LoadICD", PROC_ICD_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the icd.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes UpdateICD(ref DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ICD.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSCodes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ICD_UPDATE, ds, ds.ICD.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ICD.Rows[0][ds.ICD.ICDIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALICD::UpdateICD", PROC_ICD_UPDATE, ex);
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
        /// Deletes the icd.
        /// </summary>
        /// <param name="ICDIds">The icd ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteICD(string ICDIds)
        {
            object returnValue;
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                //DSCodes ds = LoadICD(Convert.ToInt64(ICDIds), null, null, null, null,null,null,null,null,1,15);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ICD;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ICD_ID, ICDIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ICD_DELETE);
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.ICD.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ICD.Rows[0][ds.ICD.ICDIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALICD::DeleteICD", PROC_ICD_DELETE, ex);
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
        /// Inserts the icd.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes InsertICD(ref DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ICD.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSCodes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ICD_INSERT, ds, ds.ICD.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ICD.Rows[0][ds.ICD.ICDIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALICD::InsertICD", PROC_ICD_INSERT, ex);
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
        /// Lookups the ICD code.
        /// </summary>
        /// <returns></returns>
        public DSCodeLookup LookupICDCode(string EntityId, string ICDCode, int IsEqule)
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (ICDCode == "")
                    ICDCode = null;


                if (EntityId == "")
                    EntityId = null;

                dbManager.Open();

                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_ICD_CODE, ICDCode);
                dbManager.AddParameters(1, PARM_BIT, IsEqule);

                if (EntityId == null)
                {
                    if (MDVSession.Current.IsAdmin)
                        dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ICD_LOOKUP, ds, ds.ICDCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALICD::LookupICDCode", PROC_ICD_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        public DSCodeLookup InsertICDLookup(ref DSCodeLookup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters_ICDLookUp(dbManager, ds, true);
                ds = (DSCodeLookup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ICDLookup_INSERT, ds, ds.ICDLookup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALICD::InsertICDLookup", PROC_ICDLookup_INSERT, ex);
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
    }
}
