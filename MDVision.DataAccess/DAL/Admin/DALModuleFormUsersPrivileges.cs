//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using MDVision.Datasets;
//using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
//using System.Data;
//using System.ComponentModel;

//namespace MDVision.DataAccess.DAL
//{
//    public class DALModuleFormUsersPrivileges
//    {
//        #region " Stored Procedure Names"

//        private const string PROC_MODULE_FORM_USERS_PRIVILEGES_INSERT = "System.sp_ModuleFormUsersPrivilegesInsert";
//        private const string PROC_MODULE_FORM_USERS_PRIVILEGES_DELETE = "System.sp_ModuleFormUsersPrivilegesDelete";

//        #endregion

//        #region "Query "

//        #endregion

//        #region "Parameters"

//        private const string PARM_MODULE_FORM_USER_PRIVILIGES_ID = "@ModuleFormUserPriviligesId";
//        private const string PARM_MODULE_FORM_USERS_ID = "@ModuleFormUsersId";
//        private const string PARM_PRIVILEGE_SELECTION_ID = "@PrivilegeSelectionId";
//        private const string PARM_IS_PRIVILEGED = "@IsPrivileged";
//        private const string PARM_IS_ACTIVE = "@IsActive";
//        private const string PARM_IS_DELETED = "@IsDeleted";
//        private const string PARM_CREATED_BY = "@CreatedBy";
//        private const string PARM_CREATED_ON = "@CreatedOn";
//        private const string PARM_MODIFIED_BY = "@ModifiedBy";
//        private const string PARM_MODIFIED_ON = "@ModifiedOn";
//        public struct Parameters
//        {
//            public int ID;
//            public string FNAME;
//            public string LNAME;
//        }

//        #endregion

//        //#region Singleton
//        //private static DALModuleFormUsersPrivileges _instance = null;
//        ///// <summary>
//        ///// Singleton context
//        ///// </summary>
//        //public static DALModuleFormUsersPrivileges Instance
//        //{
//        //    get
//        //    {
//        //        if (_instance == null)
//        //            _instance = new DALModuleFormUsersPrivileges();
//        //        return _instance;
//        //    }
//        //}
//        //#endregion
//        #region Constructors
//        //private static DALFacility _instance = null;
//        ///// <summary>
//        ///// Singleton context
//        ///// </summary>
//        //public static DALFacility Instance
//        //{
//        //    get
//        //    {
//        //        if (_instance == null)
//        //            _instance = new DALFacility();
//        //        return _instance;
//        //    }
//        //}
//        public DALModuleFormUsersPrivileges()
//        {
//            InitializeComponent();
//            ClientConfiguration.SetClientObject();

//        }
//        private IContainer components;
//        //NOTE: The following procedure is required by the Web Services Designer
//        //It can be modified using the Web Services Designer.  
//        //Do not modify it using the code editor.
//        [System.Diagnostics.DebuggerStepThrough()]
//        private void InitializeComponent()
//        {
//            components = new System.ComponentModel.Container();
//        }
//        #endregion


//        #region "Support Functions"
//        private void CreateParameters(IDBManager dbManager, DSUsers ds, Boolean IsInsert)
//        {
//            dbManager.CreateParameters(ds.Tables[ds.ModuleFormUsersPrivileges.TableName].Columns.Count);


//            dbManager.AddParameters(0, PARM_MODULE_FORM_USERS_ID, ds.ModuleFormUsersPrivileges.ModuleFormUsersIdColumn.ColumnName, DbType.Int64);
//            dbManager.AddParameters(1, PARM_PRIVILEGE_SELECTION_ID, ds.ModuleFormUsersPrivileges.PrivilegeSelectionIdColumn.ColumnName, DbType.Int64);
//            dbManager.AddParameters(2, PARM_IS_PRIVILEGED, ds.ModuleFormUsersPrivileges.IsPrivilegedColumn.ColumnName, DbType.Byte);
//            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.ModuleFormUsersPrivileges.IsActiveColumn.ColumnName, DbType.Byte);
//            dbManager.AddParameters(4, PARM_IS_DELETED, ds.ModuleFormUsersPrivileges.IsDeletedColumn.ColumnName, DbType.Byte);
//            dbManager.AddParameters(5, PARM_CREATED_BY, ds.ModuleFormUsersPrivileges.CreatedByColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(6, PARM_CREATED_ON, ds.ModuleFormUsersPrivileges.CreatedOnColumn.ColumnName, DbType.DateTime);
//            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.ModuleFormUsersPrivileges.ModifiedByColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.ModuleFormUsersPrivileges.ModifiedOnColumn.ColumnName, DbType.DateTime);
//            if (IsInsert == true)
//                dbManager.AddParameters(9, PARM_MODULE_FORM_USER_PRIVILIGES_ID, ds.ModuleFormUsersPrivileges.ModuleFormUserPriviligesIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
//            else
//                dbManager.AddParameters(9, PARM_MODULE_FORM_USER_PRIVILIGES_ID, ds.ModuleFormUsersPrivileges.ModuleFormUserPriviligesIdColumn.ColumnName, DbType.Int64);
//        }

//        private void CreateInsertUpdateParameters(IDBManager dbManager, DSUsers ds)
//        {

//            dbManager.CreateInsertUpdateDeleteParameters(ds.Tables[ds.ModuleFormUsersPrivileges.TableName].Columns.Count, 1);
//            dbManager.AddInsertUpdateDeleteParameters(0, PARM_MODULE_FORM_USER_PRIVILIGES_ID, ds.ModuleFormUsersPrivileges.ModuleFormUserPriviligesIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
//            //dbManager.AddInsertUpdateDeleteParameters(1, PARM_SHORT_NAME, ds.Practice.ShortNameColumn.ColumnName, DbType.String);


//        }
//        #endregion

//        #region "Insert, delete, update and get using dataset Functions"


//        public DSUsers InsertModuleFormUsersPrivileges(ref DSUsers ds)
//        {
//            IDBManager dbManager =ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                CreateParameters(dbManager, ds, true);
//                ds = (DSUsers)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_USERS_PRIVILEGES_INSERT, ds, ds.ModuleFormUsersPrivileges.TableName);
//                ds.AcceptChanges();
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DALModuleFormUsersPrivileges::InsertModuleFormUsersPrivileges", PROC_MODULE_FORM_USERS_PRIVILEGES_INSERT, ex);
//                throw ex;
//                //Usual code              
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }

//        }
//        public string DeleteModuleFormUsersPrivileges(string ModuleFormUsersPrivilegesIds)
//        {
//            IDBManager dbManager =ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                dbManager.CreateParameters(1);
//                dbManager.AddParameters(0, PARM_MODULE_FORM_USER_PRIVILIGES_ID, ModuleFormUsersPrivilegesIds);
//                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_MODULE_FORM_USERS_PRIVILEGES_DELETE);
//                //dbManager.AddParameters(0, PARM_PRACTICE_ID, ds.Practice.PracticeIdColumn.ColumnName, DbType.Int64);
//                //ds = (DSEntity)dbManager.DeleteDataSet(CommandType.StoredProcedure, PROC_PRACTICE_DELETE, ds, ds.Practice.TableName);
//                //ds.AcceptChanges();
//                //return ds;
//                return "";
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DALModuleFormUsersPrivileges::DeleteModuleFormUsersPrivileges", PROC_MODULE_FORM_USERS_PRIVILEGES_DELETE, ex);
//                return ex.Message;
//                //Usual code              
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }
//        }


//        #region "use for transaction with dataset"

//        #endregion
//        #endregion
//    }
//}
