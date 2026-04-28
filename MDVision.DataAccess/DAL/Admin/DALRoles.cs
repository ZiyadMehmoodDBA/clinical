using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Shared;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALRoles
    {
        #region Variable

        #endregion
        #region "Stored Procedure Names"
        private const string PROC_ROLE_DELETE = "System.sp_RolesDelete";
        private const string PROC_ROLE_INSERT = "System.sp_RolesInsert";
        private const string PROC_ROLE_SELECT = "System.sp_RolesSelect";
        private const string PROC_ROLE_UPDATE = "System.sp_RolesUpdate";
        private const string PROC_MODULE_FORM_ROLE_UPDATE = "System.sp_ModuleFormRolesUpdate";
        private const string PROC_MODULE_FORM_ROLE_INSERT = "System.sp_ModuleFormRolesInsert";
        private const string PROC_MODULE_FORM_ROLE_SELECT = "System.sp_ModuleFormRolesSelect";
        private const string PROC_MODULE_FORM_ROLE_DELETE = "System.sp_ModuleFormRolesDelete";
        private const string PROC_MODULE_FORM_ROLE_PRIVILEGE_DELETE = "System.sp_ModuleFormRolePrivilegesDelete";
        private const string PROC_MODULE_FORM_ROLE_PRIVILEGE_INSERT = "System.sp_ModuleFormRolePrivilegesInsert";
        private const string PROC_MODULE_FORM_ROLE_PRIVILEGE_SELECT = "System.sp_ModuleFormRolePrivilegesSelect";
        private const string PROC_MODULE_FORM_ROLE_PRIVILEGE_UPDATE = "System.sp_ModuleFormRolePrivilegesUpdate";
        private const string PROC_MODULE_FORMS_DELETE = "system.sp_ModuleFormsDelete";
        private const string PROC_ROLES_LOOKUP = "system.sp_RolesLookup";

        #endregion

        #region "Parameters"
        private const string PARM_ROLE_ID = "@RoleId";
        private const string PARM_ROLE_NAME = "@RoleName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_IS_ADMIN = "@IsAdmin";
        private const string PARM_MODULE_FORM_ROLE_ID = "@MFRId";
        private const string PARM_MODULE_FORM_ID = "@ModuleFormId";
        private const string PARM_MODULE_FORM_ROLE_PRIVILEGE_ID = "@ModuleFormRolePrivilegesId";
        private const string PARM_PRIVILEGE_SELECTEION_ID = "@PrivilegeSelectionid";
        private const string PARM_IS_PRIVILEGED = "@IsPrivileged";
        private const string PARM_IS_DELETED = "@IsDeleted";
        private const string PARM_MODULE_ID = "@ModuleId";
        private const string PARM_USER_ID = "@Userid";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ROLE_TYPE = "@RoleType";

        #endregion

        //#region Singleton
        //private static DALRoles _instance = null;
        ///// <summary>
        ///// Singleton context
        ///// </summary>
        //public static DALRoles Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = new DALRoles();
        //        return _instance;
        //    }
        //}
        //#endregion

        #region Constructors

        public DALRoles()
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
        private void CreateRoleParameters(IDBManager dbManager, DSRoles ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ROLE_ID, ds.Roles.RoleIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ROLE_ID, ds.Roles.RoleIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_ROLE_NAME, ds.Roles.RoleNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.Roles.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.Roles.IsActiveColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_DELETED, ds.Roles.IsDeletedColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.Roles.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.Roles.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.Roles.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.Roles.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_IS_ADMIN, ds.Roles.IsAdminColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_ROLE_TYPE, ds.Roles.RoleTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_ERROR_MESSAGE, ds.Roles.ErrorMessageColumn.ColumnName, DbType.String, ParamDirection.Output, null, 255);
        }

        /// <summary>
        /// Creates the module form role parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateModuleFormRoleParameters(IDBManager dbManager, DSRoles ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(3);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MODULE_FORM_ROLE_ID, ds.ModuleFormRoles.MFRIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MODULE_FORM_ROLE_ID, ds.ModuleFormRoles.MFRIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_MODULE_FORM_ID, ds.ModuleFormRoles.ModuleFormIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ROLE_ID, ds.ModuleFormRoles.RoleIdColumn.ColumnName, DbType.Int64);
        }

        /// <summary>
        /// Creates the module form role privilege parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateModuleFormRolePrivilegeParameters(IDBManager dbManager, DSRoles ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(ds.Tables[ds.ModuleFormRolePrivileges.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MODULE_FORM_ROLE_PRIVILEGE_ID, ds.ModuleFormRolePrivileges.ModuleFormRolePrivilegesIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MODULE_FORM_ROLE_PRIVILEGE_ID, ds.ModuleFormRolePrivileges.ModuleFormRolePrivilegesIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_MODULE_FORM_ROLE_ID, ds.ModuleFormRolePrivileges.ModuleFormRoleIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PRIVILEGE_SELECTEION_ID, ds.ModuleFormRolePrivileges.PrivilegeSelectionidColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_IS_PRIVILEGED, ds.ModuleFormRolePrivileges.IsPrivilegedColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.ModuleFormRolePrivileges.IsActiveColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_DELETED, ds.ModuleFormRolePrivileges.IsDeletedColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.ModuleFormRolePrivileges.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.ModuleFormRolePrivileges.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.ModuleFormRolePrivileges.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.ModuleFormRolePrivileges.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region "Insert, delete, update and get Functions For SECURITY ROLES"
        /// <summary>
        /// Loads the roles.
        /// </summary>
        /// <param name="RoleId">The role identifier.</param>
        /// <param name="RoleName">Name of the role.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public DSRoles LoadRoles(long RoleId, string RoleName, string Description, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSRoles ds = new DSRoles();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (RoleName == "")
                    RoleName = null;

                if (Description == "")
                    Description = null;

                if (IsActive == "")
                    IsActive = null;

                string CreatedBy = null;
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() != ClientConfiguration.DefaultUser.ToUpper())
                {
                    CreatedBy = ClientConfiguration.DefaultUser.ToUpper();
                }

                dbManager.Open();
                dbManager.CreateParameters(8);

                if (RoleId == 0)
                    dbManager.AddParameters(0, PARM_ROLE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ROLE_ID, RoleId);
                dbManager.AddParameters(1, PARM_ROLE_NAME, RoleName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(4, PARM_CREATED_BY, CreatedBy);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.Roles.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSRoles)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROLE_SELECT, ds, ds.Roles.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRoles::LoadRoles", PROC_ROLE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSRoles UpdateRole(ref DSRoles ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Roles.GetChanges();
                dbManager.Open();
                this.CreateRoleParameters(dbManager, ds, false);
                ds = (DSRoles)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ROLE_UPDATE, ds, ds.Roles.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Roles.Rows[0][ds.Roles.RoleIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRoles::UpdateRole", PROC_ROLE_UPDATE, ex);
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
        /// Deletes the role.
        /// </summary>
        /// <param name="RoleIds">The role ids.</param>
        /// <returns></returns>
        public string DeleteRole(string RoleIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSRoles ds = LoadRoles(Convert.ToInt64(RoleIds), null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Roles;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ROLE_ID, RoleIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ROLE_DELETE).ToString();
                if (returnValue != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.Roles.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Roles.Rows[0][ds.Roles.RoleIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";


            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRoles::DeleteRole", PROC_ROLE_DELETE, ex);
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
        /// Inserts the role.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSRoles InsertRole(ref DSRoles ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Roles.GetChanges();
                dbManager.Open();
                CreateRoleParameters(dbManager, ds, true);
                ds = (DSRoles)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ROLE_INSERT, ds, ds.Roles.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Roles.Rows[0][ds.Roles.RoleIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRoles::InsertRole", PROC_ROLE_INSERT, ex);

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

        #region "Insert, delete, update and get Functions For MODULES, FORMS, PRIVILEGES"
        /// <summary>
        /// Inserts the module form role.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSRoles InsertModuleFormRole(ref DSRoles ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateModuleFormRoleParameters(dbManager, ds, true);
                ds = (DSRoles)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_ROLE_INSERT, ds, ds.ModuleFormRoles.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRoles::InsertModuleFormRole", PROC_MODULE_FORM_ROLE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Loads the module form roles.
        /// </summary>
        /// <param name="ModuleFormId">The module form identifier.</param>
        /// <param name="RoleId">The role identifier.</param>
        /// <returns></returns>
        public DSRoles LoadModuleFormRoles(Int64 ModuleFormId, Int64 RoleId)
        {
            DSRoles dsRole = new DSRoles();
            //DataSet ds = new DataSet();
            //DataTable dt = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (ModuleFormId <= 0)
                    dbManager.AddParameters(0, PARM_MODULE_FORM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MODULE_FORM_ID, ModuleFormId);

                if (RoleId == 0)
                    dbManager.AddParameters(1, PARM_ROLE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROLE_ID, RoleId);
                //dt = new DataTable();
                //dt = dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_ROLE_SELECT, dsUser, dsUser.ModuleFormRoles.TableName).Tables[dsUser.ModuleFormRoles.TableName];
                //dsUser.Tables[dsUser.ModuleFormRoles.TableName].Merge(dt);
                dsRole = (DSRoles)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_ROLE_SELECT, dsRole, dsRole.ModuleFormRoles.TableName);
                return dsRole;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRoles::LoadModuleFormRoles", PROC_MODULE_FORM_ROLE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Loads the module form roles privileges.
        /// </summary>
        /// <param name="ModuleFormRolePrivilegesId">The module form role privileges identifier.</param>
        /// <param name="RoleId">The role identifier.</param>
        /// <param name="ModuleFormId">The module form identifier.</param>
        /// <returns></returns>
        public DSRoles LoadModuleFormRolesPrivileges(Int64 ModuleFormRolePrivilegesId, Int64 RoleId, Int64 ModuleFormId)
        {
            DSRoles dsRoles = new DSRoles();
            // DataSet ds = new DataSet();
            // DataTable dt = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();


                dbManager.CreateParameters(3);

                if (ModuleFormRolePrivilegesId == 0)
                    dbManager.AddParameters(0, PARM_MODULE_FORM_ROLE_PRIVILEGE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MODULE_FORM_ROLE_PRIVILEGE_ID, ModuleFormRolePrivilegesId);


                if (RoleId == 0)
                    dbManager.AddParameters(1, PARM_ROLE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROLE_ID, RoleId);

                if (ModuleFormId == 0)
                    dbManager.AddParameters(2, PARM_MODULE_FORM_ID, null);
                else
                    dbManager.AddParameters(2, PARM_MODULE_FORM_ID, ModuleFormId);
                //dt = new DataTable();
                //dt = dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_ROLE_PRIVILEGE_SELECT, dsUser, dsUser.ModuleFormRoles.TableName).Tables[dsUser.ModuleFormRoles.TableName];
                //dsUser.Tables[dsUser.ModuleFormRoles.TableName].Merge(dt);
                dsRoles = (DSRoles)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_ROLE_PRIVILEGE_SELECT, dsRoles, dsRoles.ModuleFormRolePrivileges.TableName);

                return dsRoles;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRoles::LoadModuleFormRolesPrivileges", PROC_MODULE_FORM_ROLE_PRIVILEGE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the module form role privileges.
        /// </summary>
        /// <param name="ds">The DSUsers.</param>
        /// <returns></returns>
        public DSRoles InsertModuleFormRolePrivileges(ref DSRoles ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateModuleFormRolePrivilegeParameters(dbManager, ds, true);
                ds = (DSRoles)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_MODULE_FORM_ROLE_PRIVILEGE_INSERT, ds, ds.ModuleFormRolePrivileges.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRoles::InsertModuleFormRolePrivileges", PROC_MODULE_FORM_ROLE_PRIVILEGE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the modules forms privileges.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="RoleId">The role identifier.</param>
        /// <param name="ModuleId">The module identifier.</param>
        /// <returns></returns>
        public string DeleteModulesFormsPrivileges(long UserId, long RoleId, long ModuleId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (ModuleId == 0)
                    dbManager.AddParameters(0, PARM_MODULE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MODULE_ID, ModuleId);
                if (RoleId == 0)
                    dbManager.AddParameters(1, PARM_ROLE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ROLE_ID, RoleId);
                if (UserId == 0)
                    dbManager.AddParameters(2, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(2, PARM_USER_ID, UserId);

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_MODULE_FORMS_DELETE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser:: DeleteModuleUsers", PROC_MODULE_FORMS_DELETE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the module form role.
        /// </summary>
        /// <param name="ModuleFormRoleId">The module form role identifier.</param>
        /// <returns></returns>
        public string DeleteModuleFormRole(string ModuleFormRoleId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_MODULE_FORM_ROLE_ID, ModuleFormRoleId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_MODULE_FORM_ROLE_DELETE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRoles::DeleteModuleFormRole", PROC_MODULE_FORM_ROLE_DELETE, ex);
                return ex.Message;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the module form role privilege.
        /// </summary>
        /// <param name="ModuleFormRolePrivilegeId">The module form role privilege identifier.</param>
        /// <returns></returns>
        public string DeleteModuleFormRolePrivilege(string ModuleFormRolePrivilegeId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_MODULE_FORM_ROLE_PRIVILEGE_ID, ModuleFormRolePrivilegeId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_MODULE_FORM_ROLE_PRIVILEGE_DELETE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRoles::DeleteModuleFormRolePrivilege", PROC_MODULE_FORM_ROLE_PRIVILEGE_DELETE, ex);
                return ex.Message;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfileLookup LookupRoles(string Active, bool IsEmergencyRole)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                string CreatedBy = null;
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() != ClientConfiguration.DefaultUser.ToUpper())
                {
                    CreatedBy = ClientConfiguration.DefaultUser.ToUpper();
                }

                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_CREATED_BY, CreatedBy);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, Active);
                dbManager.AddParameters(2, PARM_ROLE_TYPE, IsEmergencyRole);
                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROLES_LOOKUP, ds, ds.Roles.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEntity::LookupEntity", PROC_ROLES_LOOKUP, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }

        public DSProfileLookup LookupAuditReportRoles(string Active, bool IsEmergencyRole)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                string CreatedBy = null;


                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_CREATED_BY, CreatedBy);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, Active);
                dbManager.AddParameters(2, PARM_ROLE_TYPE, IsEmergencyRole);
                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROLES_LOOKUP, ds, ds.Roles.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEntity::LookupEntity", PROC_ROLES_LOOKUP, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }
        #endregion
    }
}
