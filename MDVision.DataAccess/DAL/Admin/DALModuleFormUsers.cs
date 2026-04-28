
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
//    public class DALModuleFormUsers
//    {
//        #region " Stored Procedure Names"

//        private const string PROC_MODULE_FORM_USERS_INSERT = "System.sp_ModuleFormUsersInsert";
//        private const string PROC_MODULE_FORM_USERS_DELETE = "System.sp_ModuleFormUsersDelete";

//        #endregion

//        #region "Query "

//        #endregion

//        #region "Parameters"

//        private const string PARM_MFUID = "@MFUId";
//        private const string PARM_MODULE_FORM_ID = "@ModuleFormId";
//        private const string PARM_USER_ID = "@UserId";


//        public struct Parameters
//        {
//            public int ID;
//            public string FNAME;
//            public string LNAME;
//        }

//        #endregion

//        //#region Singleton
//        //private static DALModuleFormUsers _instance = null;
//        ///// <summary>
//        ///// Singleton context
//        ///// </summary>
//        //public static DALModuleFormUsers Instance
//        //{
//        //    get
//        //    {
//        //        if (_instance == null)
//        //            _instance = new DALModuleFormUsers();
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
//        public DALModuleFormUsers()
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
//            dbManager.CreateParameters(ds.Tables[ds.ModuleFormUsers.TableName].Columns.Count);



//            dbManager.AddParameters(0, PARM_MODULE_FORM_ID, ds.ModuleFormUsers.ModuleFormIdColumn.ColumnName, DbType.Int64);
//            dbManager.AddParameters(1, PARM_USER_ID, ds.ModuleFormUsers.UserIdColumn.ColumnName, DbType.Int64);

//            if (IsInsert == true)
//                dbManager.AddParameters(2, PARM_MFUID, ds.ModuleFormUsers.MFUIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
//            else
//                dbManager.AddParameters(2, PARM_MFUID, ds.ModuleFormUsers.MFUIdColumn.ColumnName, DbType.Int64);
//        }

//        private void CreateInsertUpdateParameters(IDBManager dbManager, DSUsers ds)
//        {

//            dbManager.CreateInsertUpdateDeleteParameters(ds.Tables[ds.ModuleFormUsers.TableName].Columns.Count, 1);
//            dbManager.AddInsertUpdateDeleteParameters(0, PARM_MFUID, ds.ModuleFormUsers.MFUIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
//            //dbManager.AddInsertUpdateDeleteParameters(1, PARM_SHORT_NAME, ds.Practice.ShortNameColumn.ColumnName, DbType.String);


//        }
//        #endregion

//        #region "Insert, delete, update and get using dataset Functions"


//        public DSUsers InsertModuleFormUsers(ref DSUsers ds)
//        {
//            IDBManager dbManager =ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                CreateParameters(dbManager, ds, true);
//                ds = (DSUsers)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_USERS_INSERT, ds, ds.ModuleFormUsers.TableName);
//                ds.AcceptChanges();
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DALModuleFormUsers::InsertModuleFormUsers", PROC_MODULE_FORM_USERS_INSERT, ex);
//                throw ex;
//                //Usual code              
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }

//        }

//        public string DeleteModuleFormUsers(string ModuleFormUsersIds)
//        {
//            IDBManager dbManager =ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                dbManager.CreateParameters(1);
//                dbManager.AddParameters(0, PARM_MFUID, ModuleFormUsersIds);
//                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_MODULE_FORM_USERS_DELETE);
//                //dbManager.AddParameters(0, PARM_PRACTICE_ID, ds.Practice.PracticeIdColumn.ColumnName, DbType.Int64);
//                //ds = (DSEntity)dbManager.DeleteDataSet(CommandType.StoredProcedure, PROC_PRACTICE_DELETE, ds, ds.Practice.TableName);
//                //ds.AcceptChanges();
//                //return ds;
//                return "";
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DALModuleFormUsers::DeleteModuleFormUsers", PROC_MODULE_FORM_USERS_DELETE, ex);
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
