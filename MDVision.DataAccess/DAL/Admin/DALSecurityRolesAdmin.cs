//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using MDVision.Datasets;
//using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
//using System.Data;


//namespace MDVision.DataAccess.DAL
//{
//    public class DALSecurityRolesAdmin
//    {
//        #region "Stored Procedure Names"
//        private const string PROC_PRIVILEGES_SELECT = "System.sp_PrivilegesSelect";
//        private const string PROC_FORM_SELECT = "System.sp_FormsSelect";
//        private const string PROC_MODULE_SELECT = "System.sp_ModulesSelect";
//        private const string PROC_MODULE_FORM_SELECT = "System.sp_ModuleFormsSelect";
       
//        #endregion

//        #region "Parameters"
//        private const string PARM_MODULE_ID = "@ModuleId";
//        private const string PARM_PRIVILEGE_ID = "@PrivilegeSelectionId";
//        private const string PARM_FORM_ID = "@FormId";
//        private const string PARM_FORM_NAME = "@Name";
//        private const string PARM_MODULE_FORM_ID = "@ModuleFormId";
//        private const string PARM_IS_ACTIVE = "@IsActive";
//        private const string PARM_CREATED_BY = "@CreatedBy";
//        private const string PARM_CREATED_ON = "@CreatedOn";
//        private const string PARM_MODIFIED_BY = "@ModifiedBy";
//        private const string PARM_MODIFIED_ON = "@ModifiedOn";
//        #endregion

//        //#region Singleton
//        //private static DALSecurityRolesAdmin _instance = null;
//        ///// <summary>
//        ///// Singleton context
//        ///// </summary>
//        //public static DALSecurityRolesAdmin Instance
//        //{
//        //    get
//        //    {
//        //        if (_instance == null)
//        //            _instance = new DALSecurityRolesAdmin();
//        //        return _instance;
//        //    }
//        //}
//        //#endregion

//        #region "Generic Functions"
//        /// <summary>
//        /// Loads the modules.
//        /// </summary>
//        /// <param name="ModuleID">The module identifier.</param>
//        /// <returns></returns>
//        public DSUsers LoadModules(long ModuleID)
//        {
//            DSUsers ds = new DSUsers();
//            IDBManager dbManager =ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                dbManager.CreateParameters(1);

//                if (ModuleID <= 0)
//                    dbManager.AddParameters(0, PARM_MODULE_ID, null);
//                else
//                    dbManager.AddParameters(0, PARM_MODULE_ID, ModuleID);
//                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODULE_SELECT, ds, ds.Modules.TableName);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DALAdmin::LoadModules", PROC_MODULE_SELECT, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();

//            }
//        }

//        /// <summary>
//        /// Loads the forms.
//        /// </summary>
//        /// <param name="FormID">The form identifier.</param>
//        /// <returns></returns>
//        public DSUsers LoadForms(long FormID, string FormName)
//        {
//            DSUsers ds = new DSUsers();
//            IDBManager dbManager =ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                dbManager.CreateParameters(2);

//                if (FormID <= 0)
//                    dbManager.AddParameters(0, PARM_FORM_ID, null);
//                else
//                    dbManager.AddParameters(0, PARM_FORM_NAME, FormID);
//                dbManager.AddParameters(1, PARM_FORM_ID, FormName);
//                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FORM_SELECT, ds, ds.Forms.TableName);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DALAdmin::LoadForms", PROC_FORM_SELECT, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }
//        }

//        /// <summary>
//        /// Loads the privileges.
//        /// </summary>
//        /// <param name="PrivilegeID">The privilege identifier.</param>
//        /// <returns></returns>
//        public DSUsers LoadPrivileges(long PrivilegeID)
//        {
//            DSUsers ds = new DSUsers();
//            IDBManager dbManager =ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                dbManager.CreateParameters(1);

//                if (PrivilegeID <= 0)
//                    dbManager.AddParameters(0, PARM_PRIVILEGE_ID, null);
//                else
//                    dbManager.AddParameters(0, PARM_PRIVILEGE_ID, PrivilegeID);
//                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRIVILEGES_SELECT, ds, ds.Privileges.TableName);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DALAdmin::LoadPrivileges", PROC_PRIVILEGES_SELECT, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }
//        }

//        /// <summary>
//        /// Loads the module forms.
//        /// </summary>
//        /// <param name="ModuleFormID">The module form identifier.</param>
//        /// <returns></returns>
//        public DSUsers LoadModuleForms(long ModuleID)
//        {
//            DSUsers ds = new DSUsers();
//            IDBManager dbManager =ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                dbManager.CreateParameters(1);

//                if (ModuleID <= 0)
//                    dbManager.AddParameters(0, PARM_MODULE_ID, null);
//                else
//                    dbManager.AddParameters(0, PARM_MODULE_ID, ModuleID);
//                ds = (DSUsers)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_SELECT, ds, ds.ModuleFormsDetail.TableName);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DALAdmin::LoadModuleForms", PROC_MODULE_FORM_SELECT, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }
//        }

//        #endregion
//    }
//}
