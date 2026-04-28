using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALModulesForms
    {



        #region "Stored Procedure Names"
        private const string PROC_PRIVILEGES_SELECT = "System.sp_PrivilegesSelect";
        private const string PROC_FORM_SELECT = "System.sp_FormsSelect";
        private const string PROC_MODULE_SELECT = "System.sp_ModulesSelect";
        private const string PROC_MODULE_FORM_SELECT = "System.sp_ModuleFormsSelect";
        private const string PROC_MODULE_FORM_SELECT_BY_USER = "System.sp_UsersModulePrivileges";
        private const string PROC_IS_USER_HAVE_NOTE_UNSIGN_RIGHTS = "System.sp_CheckUserHaveNoteUnSignRights";

        #endregion

        #region "Parameters"
        private const string PARM_MODULE_ID = "@ModuleId";
        private const string PARM_PRIVILEGE_ID = "@PrivilegeSelectionId";
        private const string PARM_FORM_ID = "@FormsId";
        private const string PARM_FORM_NAME = "@Name";
        private const string PARM_MODULE_FORM_ID = "@ModuleFormId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_DB_TABLE_NAME = "@DBTableName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_MODULE_NAME = "@ModuleName";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        #endregion

        #region Constructors

        public DALModulesForms()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALModulesForms(SharedVariable SharedVariable)
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
        #region "Generic Functions"
        /// <summary>
        /// Loads the modules.
        /// </summary>
        /// <param name="ModuleID">The module identifier.</param>
        /// <returns></returns>
        public DSModuleForm LoadModules(long ModuleID, string IsActive)
        {
            DSModuleForm ds = new DSModuleForm();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (ModuleID == 0)
                    dbManager.AddParameters(0, PARM_MODULE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MODULE_ID, ModuleID);

                dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);

                ds = (DSModuleForm)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODULE_SELECT, ds, ds.Modules.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdmin::LoadModules", PROC_MODULE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();

            }
        }

        /// <summary>
        /// Loads the forms.
        /// </summary>
        /// <param name="FormID">The form identifier.</param>
        /// <returns></returns>
        public DSModuleForm LoadForms(long FormID, string FormName, string FormDescription, string DBTableName = "")
        {
           // DBTableName =  : DBTableName;
            if (DBTableName == "Allergy")
            {
                DBTableName = "Allergies";
            }

            DSModuleForm ds = new DSModuleForm();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (DBTableName == "")
                    DBTableName = null;

                if (FormName == "")
                    FormName = null;

                if (FormDescription == "")
                    FormDescription = null;

                if (FormID == 0)
                    dbManager.AddParameters(0, PARM_FORM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FORM_ID, FormID);

                dbManager.AddParameters(1, PARM_FORM_NAME, FormName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, FormDescription);
                dbManager.AddParameters(3, PARM_DB_TABLE_NAME, DBTableName);

                ds = (DSModuleForm)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FORM_SELECT, ds, ds.Forms.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdmin::LoadForms", PROC_FORM_SELECT, ex);
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
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="FormID"></param>
        /// <param name="FormName"></param>
        /// <param name="FormDescription"></param>
        /// <param name="DBTableName"></param>
        /// <returns></returns>
        public DSModuleForm LoadForms(SharedVariable SharedVariable, long FormID, string FormName, string FormDescription, string DBTableName = "")
        {
            // DBTableName =  : DBTableName;
            if (DBTableName == "Allergy")
                DBTableName = "Allergies";

            DSModuleForm ds = new DSModuleForm();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (DBTableName == "")
                    DBTableName = null;

                if (FormName == "")
                    FormName = null;

                if (FormDescription == "")
                    FormDescription = null;

                if (FormID == 0)
                    dbManager.AddParameters(0, PARM_FORM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FORM_ID, FormID);

                dbManager.AddParameters(1, PARM_FORM_NAME, FormName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, FormDescription);
                dbManager.AddParameters(3, PARM_DB_TABLE_NAME, DBTableName);

                ds = (DSModuleForm)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FORM_SELECT, ds, ds.Forms.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALAdmin::LoadForms", PROC_FORM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string CheckUserHaveNoteUnSignRights(Int64 UserId)
        {
            string returnVal = "yes";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_USER_ID, UserId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IS_USER_HAVE_NOTE_UNSIGN_RIGHTS).ToString();

                if (returnVal == "")
                    throw new Exception(returnVal);
                else
                {

                }
                return returnVal;
            }
            catch (Exception ex)
            {

                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DLLRcopia::CheckUserHaveNoteUnSignRights", PROC_IS_USER_HAVE_NOTE_UNSIGN_RIGHTS, ex);
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
        /// Loads the privileges.
        /// </summary>
        /// <param name="PrivilegeID">The privilege identifier.</param>
        /// <returns></returns>
        public DSModuleForm LoadPrivileges(long PrivilegeID, string IsActive)
        {
            DSModuleForm ds = new DSModuleForm();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PrivilegeID == 0)
                    dbManager.AddParameters(0, PARM_PRIVILEGE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PRIVILEGE_ID, PrivilegeID);

                dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);
                ds = (DSModuleForm)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRIVILEGES_SELECT, ds, ds.Privileges.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdmin::LoadPrivileges", PROC_PRIVILEGES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Loads the module forms.
        /// </summary>
        /// <param name="ModuleFormID">The module form identifier.</param>
        /// <returns></returns>
        public DSModuleForm LoadModuleForms(long ModuleID, string IsActive, string ModuleName = "")
        {
            DSModuleForm ds = new DSModuleForm();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (ModuleID == 0)
                    dbManager.AddParameters(0, PARM_MODULE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MODULE_ID, ModuleID);


                dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);


                if (ModuleName == "")
                    dbManager.AddParameters(2, PARM_MODULE_NAME, null);
                else
                    dbManager.AddParameters(2, PARM_MODULE_NAME, ModuleName);
                ds = (DSModuleForm)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_SELECT, ds, ds.ModuleForms.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdmin::LoadModuleForms", PROC_MODULE_FORM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSModuleForm LoadModuleFormsByUser(long ModuleID, string UserId)
        {
            DSModuleForm ds = new DSModuleForm();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_MODULE_ID, ModuleID);
                dbManager.AddParameters(1, PARM_USER_ID, UserId);
                ds = (DSModuleForm)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_SELECT_BY_USER, ds, ds.ModuleForms.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdmin::LoadModuleForms", PROC_MODULE_FORM_SELECT_BY_USER, ex);
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
