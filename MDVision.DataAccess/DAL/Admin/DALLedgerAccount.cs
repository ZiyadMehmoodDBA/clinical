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
using MDVision.Model.Billing.Payments;
using System.Threading.Tasks;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALLedgerAccount
    {
        #region Variable
        private SharedVariable shardVardiable;
        #endregion

        #region " Stored Procedure Names"
        private const string PROC_LEDGER_ACCOUNT_INSERT = "Provider.sp_LedgerAccountInsert";
        private const string PROC_LEDGER_ACCOUNT_UPDATE = "Provider.sp_LedgerAccountUpdate";
        private const string PROC_LEDGER_ACCOUNT_DELETE = "Provider.sp_LedgerAccountDelete";
        private const string PROC_LEDGER_ACCOUNT_SELECT = "Provider.sp_LedgerAccountSelect";
        private const string PROC_ALL_LEDGER_ACCOUNT_LOOKUP = "Provider.sp_AllLedgerAccountsLookup";


        //private const string PROC_LOOKUP_LEDGER_ACCOUNT = "Provider.sp_LedgerAccountLookup";
        private const string PROC_LOOKUP_LEDGER_TYPE = "Provider.sp_LedgerAccountTypeLookup";
        private const string PROC_LOOKUP_LEDGER_APPLY_TO = "Provider.sp_LedgerApplyToLookup";
        private const string PROC_LOOKUP_LEDGER_SYSTEM_CATEGORY = "Provider.sp_LedgerSystemCategoryLookup";

        private const string PROC_LOOKUP_PAYMENT_TYPE = "Patient.sp_PaymentTypeLookup";

        //private const string PROC_LOOKUP_LEDGER_ACCOUNT_TYPE = "Provider.sp_LedgerAccountTypeLookup";

        private const string PROC_LOOKUP_LEDGER_ACCOUNT = "Provider.sp_LedgerAccountLookup";
        private const string PROC_LOOKUP_LEDGER_ACCOUNT_FOR_PATIENT = "Provider.sp_LedgerAccountLookupForPatientAndCopay";


        #endregion

        #region "Query "

        #endregion

        #region "Parameters"

        private const string PARM_LEDGER_ACCOUNT_ID = "@LedgerAccountId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_TYPE = "@Type";
        private const string PARM_APPLY_TO = "@ApplyTo";
        private const string PARM_SYSTEM_CATEGORY = "@SystemCategory";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";

        private const string PARM_TYPE_ID = "@TypeId";
        private const string PARM_APPLY_TO_ID = "@ApplyToId";
        private const string PARM_IS_SYSTEM = "@IsSystem";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        public struct Parameters
        {
            public int ID;
            public string FNAME;
            public string LNAME;
        }

        #endregion


        #region Constructors

        public DALLedgerAccount()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();


        }
        public DALLedgerAccount(SharedVariable shardVardiable)
        {
            this.shardVardiable = shardVardiable;
            InitializeComponent();
            ClientConfiguration.SetClientObject(shardVardiable);


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
        private void CreateParameters(IDBManager dbManager, DSPaymentSetup ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(13);


            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_LEDGER_ACCOUNT_ID, ds.LedgerAccount.LedgerAccountIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_LEDGER_ACCOUNT_ID, ds.LedgerAccount.LedgerAccountIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.LedgerAccount.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.LedgerAccount.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_TYPE, ds.LedgerAccount.TypeColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_APPLY_TO, ds.LedgerAccount.ApplyToColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_SYSTEM_CATEGORY, ds.LedgerAccount.SystemCategoryColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.LedgerAccount.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.LedgerAccount.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.LedgerAccount.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.LedgerAccount.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.LedgerAccount.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_ENTITY_ID, ds.LedgerAccount.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(12, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public List<LedgerAccountModel> LoadAllLedgerAccounts()
        {
            List<LedgerAccountModel> ledgerAccountList = new List<LedgerAccountModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ENTITY_ID, shardVardiable.EntityId);

                IDataReader reader = dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ALL_LEDGER_ACCOUNT_LOOKUP);

                while (reader.Read())
                {
                    LedgerAccountModel ledgerAccountModel = new LedgerAccountModel();
                    ledgerAccountModel.LedgerAccountId = MDVUtility.ToStr(reader["LedgerAccountId"]);
                    ledgerAccountModel.ApplyTo = MDVUtility.ToStr(reader["ApplyTo"]);
                    ledgerAccountModel.Type = MDVUtility.ToStr(reader["Type"]);
                    ledgerAccountModel.ShortName = MDVUtility.ToStr(reader["ShortName"]);
                    ledgerAccountModel.IsSystem = MDVUtility.ToStr(reader["IsSystem"]);
                    ledgerAccountModel.Description = MDVUtility.ToStr(reader["Description"]);

                    ledgerAccountList.Add(ledgerAccountModel);
                }
                //ledgerAccountList = dbManager.ExecuteReaders<LedgerAccountModel>(PROC_ALL_LEDGER_ACCOUNT_LOOKUP);
                return ledgerAccountList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLedgerAccount::LoadAllLedgerAccounts", PROC_ALL_LEDGER_ACCOUNT_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPaymentSetup LoadLedgerAccount(long LedgerAccountId, string ShortName, string Description, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSPaymentSetup ds = new DSPaymentSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                if (EntityId == "")
                    EntityId = null;

                if (IsActive == "")
                    IsActive = null;
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (LedgerAccountId <= 0)
                    dbManager.AddParameters(0, PARM_LEDGER_ACCOUNT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LEDGER_ACCOUNT_ID, LedgerAccountId);

                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(4, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.LedgerAccount.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                //dbManager.AddParameters(4, PARM_EIN, EIN);
                ds = (DSPaymentSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LEDGER_ACCOUNT_SELECT, ds, ds.LedgerAccount.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLedgerAccount::LoadLedgerAccount", PROC_LEDGER_ACCOUNT_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }

        public DSPaymentSetup UpdateLedgerAccount(ref DSPaymentSetup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.LedgerAccount.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPaymentSetup)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_LEDGER_ACCOUNT_UPDATE, ds, ds.LedgerAccount.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.LedgerAccount.Rows[0][ds.LedgerAccount.LedgerAccountIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLedgerAccount::UpdateLedgerAccount", PROC_LEDGER_ACCOUNT_UPDATE, ex);
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


        public string DeleteLedgerAccount(string LedgerAccountIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(2);
                //DSPaymentSetup ds = LoadLedgerAccount(Convert.ToInt64(LedgerAccountIds), null, null, null, null, 1, 1000);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.LedgerAccount;
                dbManager.AddParameters(0, PARM_LEDGER_ACCOUNT_ID, LedgerAccountIds);

                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_LEDGER_ACCOUNT_DELETE).ToString();
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.LedgerAccount.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.LedgerAccount.Rows[0][ds.LedgerAccount.LedgerAccountIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                    
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRevenueCode::DeleteRevenueCode", PROC_LEDGER_ACCOUNT_DELETE, ex);
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

        public DSPaymentSetup InsertLedgerAccount(ref DSPaymentSetup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.LedgerAccount.GetChanges();
                CreateParameters(dbManager, ds, true);

                ds = (DSPaymentSetup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_LEDGER_ACCOUNT_INSERT, ds, ds.LedgerAccount.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.LedgerAccount.Rows[0][ds.LedgerAccount.LedgerAccountIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLedgerAccount::InsertLedgerAccount", PROC_LEDGER_ACCOUNT_INSERT, ex);
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

        public DSPaymentSetup LookupLedgerType(string Active)
        {
            DSPaymentSetup ds = new DSPaymentSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_IS_ACTIVE, Active);

                //if (POSId <= 0)
                //    dbManager.AddParameters(0, PARM_POS_ID, null);
                //else
                //    dbManager.AddParameters(0, PARM_POS_ID, POSId);

                ds = (DSPaymentSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOKUP_LEDGER_TYPE, ds, ds.LedgerAccountType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLedgerAccount::LookupLedgerType", PROC_LOOKUP_LEDGER_TYPE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPaymentSetup LookupLedgerApplyTo(string Active)
        {
            DSPaymentSetup ds = new DSPaymentSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_IS_ACTIVE, Active);
                //if (POSId <= 0)
                //    dbManager.AddParameters(0, PARM_POS_ID, null);
                //else
                //    dbManager.AddParameters(0, PARM_POS_ID, POSId);

                ds = (DSPaymentSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOKUP_LEDGER_APPLY_TO, ds, ds.LedgerApplyTo.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLedgerAccount::LookupLedgerApplyTo", PROC_LOOKUP_LEDGER_APPLY_TO, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSPaymentSetup LookupLedgerSystemAccount(string Active)
        {
            DSPaymentSetup ds = new DSPaymentSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_IS_ACTIVE, Active);
                //if (POSId <= 0)
                //    dbManager.AddParameters(0, PARM_POS_ID, null);
                //else
                //    dbManager.AddParameters(0, PARM_POS_ID, POSId);

                ds = (DSPaymentSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOKUP_LEDGER_SYSTEM_CATEGORY, ds, ds.LedgerSystemCategory.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLedgerAccount::LookupLedgerSystemcategory", PROC_LOOKUP_LEDGER_SYSTEM_CATEGORY, ex);
                throw ex;
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

        public DSPaymentSetup LookupPaymentType(string Active)
        {
            DSPaymentSetup ds = new DSPaymentSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();

                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_IS_ACTIVE, Active);

                ds = (DSPaymentSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOKUP_PAYMENT_TYPE, ds, ds.PaymentType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLedgerAccount::LookupPaymentType", PROC_LOOKUP_PAYMENT_TYPE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //public DSPaymentSetup LookupLedgerAccountType()
        //{
        //    DSPaymentSetup ds = new DSPaymentSetup();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        ds = (DSPaymentSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOKUP_LEDGER_TYPE, ds, ds.LedgerAccountType.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALLedgerAccount::LookupLedgerAccountType", PROC_LOOKUP_LEDGER_TYPE, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}


        public DSPaymentSetup LookupLedgerAccount(Int64 TypeId, Int64 ApplyToId, Int64 SystemCategory = 0, Int64 IsSystem = -1)
        {
            DSPaymentSetup ds = new DSPaymentSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //if (Active == "")
                //    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(6);
                if (TypeId <= 0)
                {
                    dbManager.AddParameters(0, PARM_TYPE_ID, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_TYPE_ID, TypeId);
                }

                if (ApplyToId <= 0)
                {
                    dbManager.AddParameters(1, PARM_APPLY_TO_ID, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_APPLY_TO_ID, ApplyToId);
                }

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);



                if (SystemCategory <= 0)
                {
                    dbManager.AddParameters(4, PARM_SYSTEM_CATEGORY, null);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_SYSTEM_CATEGORY, SystemCategory);
                }


                if (IsSystem < 0)
                {
                    dbManager.AddParameters(5, PARM_IS_SYSTEM, null);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_IS_SYSTEM, IsSystem);
                }

                //dbManager.AddParameters(6, PARM_IS_ACTIVE, Active);


                ds = (DSPaymentSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOKUP_LEDGER_ACCOUNT, ds, ds.LedgerAccount1.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLedgerAccount::LookupLedgerAccount", PROC_LOOKUP_LEDGER_ACCOUNT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPaymentSetup LookupLedgerAccountForPatientAndCopay(Int64 TypeId, Int64 SystemCategory = 0, Int64 IsSystem = -1)
        {
            DSPaymentSetup ds = new DSPaymentSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //if (Active == "")
                //    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(5);
                if (TypeId <= 0)
                {
                    dbManager.AddParameters(0, PARM_TYPE_ID, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_TYPE_ID, TypeId);
                }

                //if (ApplyToId <= 0)
                //{
                //    dbManager.AddParameters(1, PARM_APPLY_TO_ID, null);
                //}
                //else
                //{
                //    dbManager.AddParameters(1, PARM_APPLY_TO_ID, ApplyToId);
                //}

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);



                if (SystemCategory <= 0)
                {
                    dbManager.AddParameters(3, PARM_SYSTEM_CATEGORY, null);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_SYSTEM_CATEGORY, SystemCategory);
                }


                if (IsSystem < 0)
                {
                    dbManager.AddParameters(4, PARM_IS_SYSTEM, null);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_IS_SYSTEM, IsSystem);
                }

                //dbManager.AddParameters(6, PARM_IS_ACTIVE, Active);


                ds = (DSPaymentSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOKUP_LEDGER_ACCOUNT_FOR_PATIENT, ds, ds.LedgerAccount1.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLedgerAccount::LookupLedgerAccountForPatientAndCopay", PROC_LOOKUP_LEDGER_ACCOUNT_FOR_PATIENT, ex);
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


