using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Shared;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALPrivilegeGroup
    {
        #region Variable
        
        #endregion
        #region "Stored Procedure Names"
        private const string PROC_SECURITY_GROUP_DELETE = "System.sp_SecurityGroupDelete";
        private const string PROC_SECURITY_GROUP_INSERT = "System.sp_SecurityGroupInsert";
        private const string PROC_SECURITY_GROUP_SELECT = "System.sp_SecurityGroupSelect";
        private const string PROC_SECURITY_GROUP_UPDATE = "System.sp_SecurityGroupUpdate";
        private const string PROC_SECURITY_GROUP_PROVIDER_SELECT = "System.sp_SecurityGroupProviderSelect";
        private const string PROC_SECURITY_GROUP_PROVIDER_INSERT = "System.sp_SecurityGroupProviderInsert";
        private const string PROC_SECURITY_GROUP_PROVIDER_DELETE = "System.sp_SecurityGroupProviderDelete";
        private const string PROC_SECURITY_GROUP_RESOURCE_SELECT = "System.sp_SecurityGroupResourceSelect";
        private const string PROC_SECURITY_GROUP_RESOURCE_INSERT = "System.sp_SecurityGroupResourceInsert";
        private const string PROC_SECURITY_GROUP_RESOURCE_DELETE = "System.sp_SecurityGroupResourceDelete";
        private const string PROC_SECURITY_GROUP_FACILITY_SELECT = "System.sp_SecurityGroupFacilitySelect";
        private const string PROC_SECURITY_GROUP_FACILITY_INSERT = "System.sp_SecurityGroupFacilityInsert";
        private const string PROC_SECURITY_GROUP_FACILITY_DELETE = "System.sp_SecurityGroupFacilityDelete";

        private const string PROC_SECURITY_GROUP_ENTITY_SELECT = "System.sp_SecurityGroupEntitySelect";
        private const string PROC_SECURITY_GROUP_PRACTICE_SELECT = "System.sp_SecurityGroupEntitySelect";
        #endregion

        #region "Parameters"
        private const string PARM_SEC_GROUP_PROVIDER_ID = "@SecGroupProviderId";
        private const string PARM_SEC_GROUP_ID = "@SecGroupId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_RESOURCE_ID = "@ResourceId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_NAME = "@Name";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_IS_ADMIN = "@IsAdmin";
        private const string PARM_IS_DELETED = "@IsDeleted";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_SEC_GROUP_ENTITY_ID = "@SecGroupEntityId";
        private const string PARM_SEC_GROUP_PRACTICE_ID = "@SecGroupPracticeId";
        private const string PARM_SEC_GROUP_RESOURCE_ID = "@SecGroupResourceId";
        private const string PARM_SEC_GROUP_FACILITY_ID = "@SecGroupFacilityId";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors
        public DALPrivilegeGroup()
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
        private void CreateParameters(IDBManager dbManager, DSPrivilegeGroup ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(11);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SEC_GROUP_ID, ds.SecurityGroup.SecGroupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SEC_GROUP_ID, ds.SecurityGroup.SecGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_NAME, ds.SecurityGroup.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_SHORT_NAME, ds.SecurityGroup.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.SecurityGroup.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.SecurityGroup.IsActiveColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_ADMIN, ds.SecurityGroup.IsAdminColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_IS_DELETED, ds.SecurityGroup.IsDeletedColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.SecurityGroup.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.SecurityGroup.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.SecurityGroup.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.SecurityGroup.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        /// <summary>
        /// Creates the privilege group provider parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreatePrivGroupProviderParameters(IDBManager dbManager, DSPrivilegeGroup ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(ds.Tables[ds.SecurityGroupProvider.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SEC_GROUP_PROVIDER_ID, ds.SecurityGroupProvider.SecGroupProviderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SEC_GROUP_PROVIDER_ID, ds.SecurityGroupProvider.SecGroupProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SEC_GROUP_ID, ds.SecurityGroupProvider.SecGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PROVIDER_ID, ds.SecurityGroupProvider.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_NAME, ds.SecurityGroupProvider.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SHORT_NAME, ds.SecurityGroupProvider.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DESCRIPTION, ds.SecurityGroupProvider.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.SecurityGroupProvider.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_IS_DELETED, ds.SecurityGroupProvider.IsDeletedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.SecurityGroupProvider.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.SecurityGroupProvider.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.SecurityGroupProvider.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.SecurityGroupProvider.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        /// <summary>
        /// Creates the privilige group resource parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreatePrivGroupResourceParameters(IDBManager dbManager, DSPrivilegeGroup ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(ds.Tables[ds.SecurityGroupResource.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SEC_GROUP_RESOURCE_ID, ds.SecurityGroupResource.SecGroupResourceIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SEC_GROUP_RESOURCE_ID, ds.SecurityGroupResource.SecGroupResourceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SEC_GROUP_ID, ds.SecurityGroupResource.SecGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_RESOURCE_ID, ds.SecurityGroupResource.ResourceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_NAME, ds.SecurityGroupResource.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SHORT_NAME, ds.SecurityGroupResource.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DESCRIPTION, ds.SecurityGroupResource.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.SecurityGroupResource.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_IS_DELETED, ds.SecurityGroupResource.IsDeletedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.SecurityGroupResource.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.SecurityGroupResource.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.SecurityGroupResource.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.SecurityGroupResource.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        /// <summary>
        /// Creates the privilege group facility parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreatePrivGroupFacilityParameters(IDBManager dbManager, DSPrivilegeGroup ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(ds.Tables[ds.SecurityGroupFacility.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_SEC_GROUP_FACILITY_ID, ds.SecurityGroupFacility.SecGroupFacilityIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_SEC_GROUP_FACILITY_ID, ds.SecurityGroupFacility.SecGroupFacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SEC_GROUP_ID, ds.SecurityGroupFacility.SecGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.SecurityGroupFacility.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_NAME, ds.SecurityGroupFacility.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SHORT_NAME, ds.SecurityGroupFacility.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DESCRIPTION, ds.SecurityGroupFacility.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.SecurityGroupFacility.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_IS_DELETED, ds.SecurityGroupFacility.IsDeletedColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_CREATED_BY, ds.SecurityGroupFacility.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CREATED_ON, ds.SecurityGroupFacility.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_MODIFIED_BY, ds.SecurityGroupFacility.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_MODIFIED_ON, ds.SecurityGroupFacility.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_ENTITY_ID, ds.SecurityGroupFacility.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_PRACTICE_ID, ds.SecurityGroupFacility.PracticeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(14, PARM_USER_ID, ds.SecurityGroupFacility.UserIdColumn.ColumnName, DbType.Int64);
        }
        #endregion

        #region "Insert, delete, update and get Functions For Privilege Group"
        public DSPrivilegeGroup LoadPrivilegeGroup(long PrivilegeGroupId, string ShortName, string Description, string IsActive, int PageNumber,int RowsPerPage)
        {
            DSPrivilegeGroup ds = new DSPrivilegeGroup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

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

                if (PrivilegeGroupId == 0)
                    dbManager.AddParameters(0, PARM_SEC_GROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SEC_GROUP_ID, PrivilegeGroupId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
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
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.SecurityGroup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                
                ds = (DSPrivilegeGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SECURITY_GROUP_SELECT, ds, ds.SecurityGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::LoadPrivilegeGroup", PROC_SECURITY_GROUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPrivilegeGroup InsertPrivilegeGroup(ref DSPrivilegeGroup ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSPrivilegeGroup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SECURITY_GROUP_INSERT, ds, ds.SecurityGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::InsertPrivilegeGroup", PROC_SECURITY_GROUP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPrivilegeGroup UpdatePrivilegeGroup(ref DSPrivilegeGroup ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPrivilegeGroup)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SECURITY_GROUP_UPDATE, ds, ds.SecurityGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::UpdatePrivilegeGroup", PROC_SECURITY_GROUP_UPDATE, ex);
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

        public string DeletePrivilegeGroup(string PrivilegeGroupIds)
        {
            string returnValue = "";
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SEC_GROUP_ID, PrivilegeGroupIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SECURITY_GROUP_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::DeletePrivilegeGroup", PROC_SECURITY_GROUP_DELETE, ex);
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

        #region "Insert, delete, update and get Functions For Privilege Group Detail (Facility, Practice, Entity, Provider, Resources)"
        //public DSPrivilegeGroup LoadSecurityGroupEntity(long SecGroupEntityId, long SecGroupId)
        //{
        //    DSPrivilegeGroup ds = new DSPrivilegeGroup();
        //    IDBManager dbManager =ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(2);

        //        if (SecGroupEntityId == 0)
        //            dbManager.AddParameters(0, PARM_SEC_GROUP_ENTITY_ID, null);
        //        else
        //            dbManager.AddParameters(0, PARM_SEC_GROUP_ENTITY_ID, SecGroupEntityId);
        //        if (SecGroupId == 0)
        //            dbManager.AddParameters(0, PARM_SEC_GROUP_ID, null);
        //        else
        //            dbManager.AddParameters(0, PARM_SEC_GROUP_ID, SecGroupId);
        //        ds = (DSPrivilegeGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SECURITY_GROUP_ENTITY_SELECT, ds, ds.SecurityGroupEntity.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALPrivilegeGroup::LoadPrivilegeGroupEntity", PROC_SECURITY_GROUP_ENTITY_SELECT, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        //public DSPrivilegeGroup LoadSecurityGroupPractice(long SecGroupPracticeId, long SecGroupId)
        //{
        //    DSPrivilegeGroup ds = new DSPrivilegeGroup();
        //    IDBManager dbManager =ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(2);

        //        if (SecGroupPracticeId == 0)
        //            dbManager.AddParameters(0, PARM_SEC_GROUP_PRACTICE_ID, null);
        //        else
        //            dbManager.AddParameters(0, PARM_SEC_GROUP_PRACTICE_ID, SecGroupPracticeId);
        //        if (SecGroupId == 0)
        //            dbManager.AddParameters(0, PARM_SEC_GROUP_ID, null);
        //        else
        //            dbManager.AddParameters(0, PARM_SEC_GROUP_ID, SecGroupId);
        //        ds = (DSPrivilegeGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SECURITY_GROUP_PRACTICE_SELECT, ds, ds.SecurityGroupPractice.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALPrivilegeGroup::LoadSecurityGroupPractice", PROC_SECURITY_GROUP_PRACTICE_SELECT, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        public DSPrivilegeGroup LoadSecurityGroupProvider(long SecGroupProviderId, long SecGroupId)
        {
            DSPrivilegeGroup ds = new DSPrivilegeGroup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (SecGroupProviderId == 0)
                    dbManager.AddParameters(0, PARM_SEC_GROUP_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SEC_GROUP_PROVIDER_ID, SecGroupProviderId);
                if (SecGroupId == 0)
                    dbManager.AddParameters(1, PARM_SEC_GROUP_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SEC_GROUP_ID, SecGroupId);
                ds = (DSPrivilegeGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SECURITY_GROUP_PROVIDER_SELECT, ds, ds.SecurityGroupProvider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::LoadSecurityGroupProvider", PROC_SECURITY_GROUP_PROVIDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPrivilegeGroup LoadSecurityGroupResources(long SecGroupResourceId, long SecGroupId)
        {
            DSPrivilegeGroup ds = new DSPrivilegeGroup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (SecGroupResourceId == 0)
                    dbManager.AddParameters(0, PARM_SEC_GROUP_RESOURCE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SEC_GROUP_RESOURCE_ID, SecGroupResourceId);
                if (SecGroupId == 0)
                    dbManager.AddParameters(1, PARM_SEC_GROUP_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SEC_GROUP_ID, SecGroupId);
                ds = (DSPrivilegeGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SECURITY_GROUP_RESOURCE_SELECT, ds, ds.SecurityGroupResource.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::LoadSecurityGroupResources", PROC_SECURITY_GROUP_RESOURCE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPrivilegeGroup LoadSecurityGroupFacility(long SecGroupFacilityId, long SecGroupId)
        {
            DSPrivilegeGroup ds = new DSPrivilegeGroup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (SecGroupFacilityId == 0)
                    dbManager.AddParameters(0, PARM_SEC_GROUP_FACILITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SEC_GROUP_FACILITY_ID, SecGroupFacilityId);
                if (SecGroupId == 0)
                    dbManager.AddParameters(1, PARM_SEC_GROUP_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SEC_GROUP_ID, SecGroupId);
                ds = (DSPrivilegeGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SECURITY_GROUP_FACILITY_SELECT, ds, ds.SecurityGroupFacility.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::LoadSecurityGroupFacility", PROC_SECURITY_GROUP_FACILITY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPrivilegeGroup InsertPrivilegeGroupProvider(ref DSPrivilegeGroup ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreatePrivGroupProviderParameters(dbManager, ds, true);
                ds = (DSPrivilegeGroup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SECURITY_GROUP_PROVIDER_INSERT, ds, ds.SecurityGroupProvider.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::InsertPrivilegeGroupProvider", PROC_SECURITY_GROUP_PROVIDER_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeletePrivilegeGroupProvider(string PrivGroupProviderId)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SEC_GROUP_PROVIDER_ID, PrivGroupProviderId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_SECURITY_GROUP_PROVIDER_DELETE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::DeletePrivilegeGroupProvider", PROC_SECURITY_GROUP_PROVIDER_DELETE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPrivilegeGroup InsertPrivilegeGroupResource(ref DSPrivilegeGroup ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreatePrivGroupResourceParameters(dbManager, ds, true);
                ds = (DSPrivilegeGroup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SECURITY_GROUP_RESOURCE_INSERT, ds, ds.SecurityGroupResource.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::InsertPrivilegeGroupResource", PROC_SECURITY_GROUP_RESOURCE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeletePrivilegeGroupResource(string PrivGroupResourceId)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SEC_GROUP_RESOURCE_ID, PrivGroupResourceId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_SECURITY_GROUP_RESOURCE_DELETE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::DeletePrivilegeGroupResource", PROC_SECURITY_GROUP_RESOURCE_DELETE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPrivilegeGroup InsertPrivilegeGroupFacility(ref DSPrivilegeGroup ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreatePrivGroupFacilityParameters(dbManager, ds, true);
                ds = (DSPrivilegeGroup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SECURITY_GROUP_FACILITY_INSERT, ds, ds.SecurityGroupFacility.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::InsertPrivilegeGroupFacility", PROC_SECURITY_GROUP_FACILITY_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeletePrivilegeGroupFacility(string PrivGroupFacilityId, string PrivGroupId, string EntityId, string PracticeId)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_SEC_GROUP_FACILITY_ID, PrivGroupFacilityId);
                dbManager.AddParameters(1, PARM_SEC_GROUP_ID, PrivGroupId);
                dbManager.AddParameters(2, PARM_PRACTICE_ID, PracticeId);
                dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_SECURITY_GROUP_FACILITY_DELETE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPrivilegeGroup::DeletePrivilegeGroupFacility", PROC_SECURITY_GROUP_FACILITY_DELETE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
    }
}
