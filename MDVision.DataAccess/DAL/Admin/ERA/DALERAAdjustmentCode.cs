using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin.ERA
{
    public class DALERAAdjustmentCode
    {
         #region Variable
        
        #endregion

        #region " Stored Procedure Names"
        private const string PROC_ERAADJUSTMENTCODE_INSERT = "Billing.sp_ERAAdjustmentCodeInsert";
        private const string PROC_ERAADJUSTMENTCODE_UPDATE = "Billing.sp_ERAAdjustmentCodeUpdate";
        private const string PROC_ERAADJUSTMENTCODE_DELETE = "Billing.sp_ERAAdjustmentCodeDelete";
        private const string PROC_ERAADJUSTMENTCODE_SELECT = "Billing.sp_ERAAdjustmentCodeSelect";

        private const string PROC_LOOKUP_ADJUSTMENT_REASONCODE = "Billing.sp_AdjustmentReasonCodeLookup ";
        private const string PROC_LOOKUP_ADJUSTMENT_GROUPCODE = "Billing.sp_AdjustmentGroupCodeLookup ";
        //private const string PROC_LOOKUP_LEDGER_SYSTEM_CATEGORY = "Provider.sp_LedgerSystemCategoryLookup";
        //private const string PROC_LOOKUP_PAYMENT_TYPE = "Patient.sp_PaymentTypeLookup";
        //private const string PROC_LOOKUP_ERAADJUSTMENTCODE = "Provider.sp_ERAAdjustmentCodeLookup";


        #endregion

        #region "Query "

        #endregion

        #region "Parameters"

        private const string PARM_ERAADJUSTMENTCODE_ID = "@ERAAdjCodeId";
        private const string PARM_CLAIM_ADJU_GROUPCODE = "@ClaimAdjuGroupId";
        private const string PARM_CLAIM_ADJ_REASONCODE = "@ClaimAdjReasonId";
        private const string PARM_PRACTICEID = "@PracticeId";
        private const string PARM_CLEARINGHOUSEID = "@ClearingHouseId";
        private const string PARM_ERAACTIONID = "@ERAActionId";

        private const string PARM_LEDGER_ACCOUNTID = "@LedgerAccountId";
        private const string PARM_LEDGER_ENTRY_COMMENTS = "@LedgerEntryComments";
     

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ENTITY_ID = "@EntityId";

        //public struct Parameters
        //{
        //    public int ID;
        //    public string FNAME;
        //    public string LNAME;
        //}

        #endregion

     
        #region Constructors 
        
        public DALERAAdjustmentCode()
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
        private void CreateParameters(IDBManager dbManager, DSERA ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(13);


            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ERAADJUSTMENTCODE_ID, ds.ERAAdjustmentCode.ERAAdjCodeIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ERAADJUSTMENTCODE_ID, ds.ERAAdjustmentCode.ERAAdjCodeIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_CLAIM_ADJU_GROUPCODE, ds.ERAAdjustmentCode.ClaimAdjuGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_CLAIM_ADJ_REASONCODE, ds.ERAAdjustmentCode.ClaimAdjReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PRACTICEID, ds.ERAAdjustmentCode.PracticeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_CLEARINGHOUSEID, ds.ERAAdjustmentCode.ClearingHouseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_ERAACTIONID, ds.ERAAdjustmentCode.ERAActionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_LEDGER_ACCOUNTID, ds.ERAAdjustmentCode.LedgerAccountIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_LEDGER_ENTRY_COMMENTS, ds.ERAAdjustmentCode.LedgerEntryCommentsColumn.ColumnName, DbType.String);


            dbManager.AddParameters(8, PARM_IS_ACTIVE, ds.ERAAdjustmentCode.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(9, PARM_CREATED_BY, ds.ERAAdjustmentCode.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CREATED_ON, ds.ERAAdjustmentCode.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_MODIFIED_BY, ds.ERAAdjustmentCode.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_MODIFIED_ON, ds.ERAAdjustmentCode.ModifiedOnColumn.ColumnName, DbType.DateTime);
            
          //  dbManager.AddParameters(13, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSERA LoadERAAdjustmentCode(long ERAAdjustmentCodeId, long ClaimAdjGroupCodeId, long ClaimAdjReasonCodesId, long ClearinghouseId, long ERAActionId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSERA ds = new DSERA();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(10);
                if (ERAAdjustmentCodeId <= 0)
                    dbManager.AddParameters(0, PARM_ERAADJUSTMENTCODE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ERAADJUSTMENTCODE_ID, ERAAdjustmentCodeId);

                if (ClaimAdjGroupCodeId<=0)
                    dbManager.AddParameters(1, PARM_CLAIM_ADJU_GROUPCODE, null);
                else
                {
                    dbManager.AddParameters(1, PARM_CLAIM_ADJU_GROUPCODE, ClaimAdjGroupCodeId);
                }
                if (ClaimAdjReasonCodesId <= 0)
                    dbManager.AddParameters(2, PARM_CLAIM_ADJ_REASONCODE, null);
                else
                {
                    dbManager.AddParameters(2, PARM_CLAIM_ADJ_REASONCODE, ClaimAdjReasonCodesId);
                }
                if (ClearinghouseId <=0)
                    dbManager.AddParameters(3, PARM_CLEARINGHOUSEID, null);
                else
                {
                    dbManager.AddParameters(3, PARM_CLEARINGHOUSEID, ClearinghouseId);
                }
                if (ERAActionId <=0)
                    dbManager.AddParameters(4, PARM_ERAACTIONID, null);
                else
                {
                    dbManager.AddParameters(4, PARM_ERAACTIONID, ERAActionId);
                }
                if (string.IsNullOrEmpty(IsActive))
                {
                    dbManager.AddParameters(5, PARM_IS_ACTIVE, null);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_IS_ACTIVE, IsActive);
                }
                if (PageNumber == 0)
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(6, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(7, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.ERAAdjustmentCode.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ENTITY_ID, MDVSession.Current.EntityId); 

                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ERAADJUSTMENTCODE_SELECT, ds, ds.ERAAdjustmentCode.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERAAdjustmentCode::LoadERAAdjustmentCode", PROC_ERAADJUSTMENTCODE_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }

        public DSERA UpdateERAAdjustmentCode(DSERA ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ERAAdjustmentCode.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSERA)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ERAADJUSTMENTCODE_UPDATE, ds, ds.ERAAdjustmentCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ERAAdjustmentCode.Rows[0][ds.ERAAdjustmentCode.ERAAdjCodeIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERAAdjustmentCode::UpdateERAAdjustmentCode", PROC_ERAADJUSTMENTCODE_UPDATE, ex);
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


        public string DeleteERAAdjustmentCode(string ERAAdjustmentCodeIds)
        {
            string returnValue = "";
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
             

                dbManager.Open();
                dbManager.CreateParameters(2);            
                //DSERA ds = LoadERAAdjustmentCode(Convert.ToInt64(ERAAdjustmentCodeIds),0,0,0,0,null,1,1000);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ERAAdjustmentCode;
                dbManager.AddParameters(0, PARM_ERAADJUSTMENTCODE_ID, ERAAdjustmentCodeIds);
               
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ERAADJUSTMENTCODE_DELETE).ToString();
                if (returnValue != null  && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.ERAAdjustmentCode.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ERAAdjustmentCode.Rows[0][ds.ERAAdjustmentCode.ERAAdjCodeIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                    
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERAAdjustmentCode::DeleteERAAdjustmentCode", PROC_ERAADJUSTMENTCODE_DELETE, ex);
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

        public DSERA InsertERAAdjustmentCode(ref DSERA ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ERAAdjustmentCode.GetChanges();
                CreateParameters(dbManager, ds, true);

                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERAADJUSTMENTCODE_INSERT, ds, ds.ERAAdjustmentCode.TableName);
                ds.AcceptChanges();

                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.ERAAdjustmentCode.Rows[0][ds.ERAAdjustmentCode.ERAAdjCodeIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERAAdjustmentCode::InsertERAAdjustmentCode", PROC_ERAADJUSTMENTCODE_INSERT, ex);
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




        #region "use for transaction with dataset"
       
        #endregion

        #endregion
        #region "Lookups"

        public DSERALookup LookupAdjustmentReasonCode()
        {
            DSERALookup ds = new DSERALookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSERALookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOKUP_ADJUSTMENT_REASONCODE, ds, ds.AdjustmentReasonCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERAAdjustmentCode::LookupAdjustmentReasonCode", PROC_LOOKUP_ADJUSTMENT_REASONCODE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSERALookup LookupAdjustmentGroupCode()
        {
            DSERALookup ds = new DSERALookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSERALookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOKUP_ADJUSTMENT_GROUPCODE, ds, ds.AdjustmentGroupCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERAAdjustmentCode::LookupAdjustmentGroupCode", PROC_LOOKUP_ADJUSTMENT_GROUPCODE, ex);
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
