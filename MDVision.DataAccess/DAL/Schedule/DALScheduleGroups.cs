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

namespace MDVision.DataAccess.DAL.Schedule
{
   public class DALScheduleGroups
    {
       #region Variable
      // public SharedVariable ;
       #endregion

       #region Constructors
       public DALScheduleGroups()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
          //   = Obj;
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
       private const string PROC_SCHEDULE_GROUPS_LOOKUP = "Provider.sp_MultipleScheduleGroupsLookup";
       private const string PROC_SCHEDULE_GROUPS_DELETE = "Provider.sp_MultipleScheduleGroupsDelete";
       private const string PROC_SCHEDULE_GROUPS_INSERT = "Provider.sp_MultipleScheduleGroupsInsert";
       private const string PROC_SCHEDULE_GROUPS_UPDATE = "Provider.sp_MultipleScheduleGroupsUpdate";
       private const string PROC_SCHEDULE_GROUPS_SELECT = "Provider.sp_MultipleScheduleGroupsSelect";

       private const string PROC_SCH_RESOURCE_SELECT = "Provider.sp_SchResourcesSelect";
       private const string PROC_SCH_PROVIDER_SELECT = "Provider.sp_SchProviderSelect";

       private const string PROC_SCHGROUP_PRO_RES_SELECT = "Provider.sp_SchGroupProvResSelect";

       private const string PROC_GROUP_PROVIDER_SELECT = "Provider.sp_GroupProviderSelect";
       private const string PROC_GROUP_RESOURCE_SELECT = "Provider.sp_GroupResourceSelect";


       private const string PROC_GROUP_PROVIDER_DELETE = "Provider.sp_GroupProviderDelete";
       private const string PROC_GROUP_RESOURCE_DELETE = "Provider.sp_GroupResourceDelete";

       private const string PROC_GROUP_PROVIDER_INSERT = "Provider.sp_GroupProvidersInsert";
       private const string PROC_GROUP_RESOURCE_INSERT = "Provider.sp_GroupResourcesInsert";

       #endregion

       #region "Parameters"
       private const string PARM_MSGROUP_ID = "@MSGroupId";
       private const string PARM_SHORT_NAME = "@ShortName";
       private const string PARM_ENTITY_ID = "@EntityId";
       private const string PARM_DESCRIPTION = "@Description";
       private const string PARM_PROVIDER_ID = "@ProviderId";
       private const string PARM_RESOURCE_ID = "@ResourceId";
       private const string PARM_FACILITY_ID = "@FacilityId";
       private const string PARM_IS_ACTIVE = "@IsActive";
       private const string PARM_CREATED_BY = "@CreatedBy";
       private const string PARM_CREATED_ON = "@CreatedOn";
       private const string PARM_MODIFIED_BY = "@ModifiedBy";
       private const string PARM_MODIFIED_ON = "@ModifiedOn";
       private const string PARM_USER_ID = "@UserId";
       private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

       private const string PARM_GROUP_PROVIDER_ID = "@GrpProvidersId";
       private const string PARM_GROUP_RESOURCE_ID = "@GrpResourcesId";

       private const string PARM_PROVIDER_SCHEDULE_ID = "@ProScheduleId";
       private const string PARM_RESOURCE_SCHEDULE_ID = "@ResScheduleId";

       #endregion

       #region "Support Functions"

       private void CreateParameters(IDBManager dbManager, DSScheduleGroup ds, Boolean IsInsert)
       {
           dbManager.CreateParameters(9);

           if (IsInsert == true)
               dbManager.AddParameters(0, PARM_MSGROUP_ID, ds.MultipleScheduleGroups.MSGroupIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
           else
               dbManager.AddParameters(0, PARM_MSGROUP_ID, ds.MultipleScheduleGroups.MSGroupIdColumn.ColumnName, DbType.Int32);
           dbManager.AddParameters(1, PARM_SHORT_NAME, ds.MultipleScheduleGroups.ShortNameColumn.ColumnName, DbType.String);
           dbManager.AddParameters(2, PARM_ENTITY_ID, ds.MultipleScheduleGroups.EntityIdColumn.ColumnName, DbType.Int64);
           dbManager.AddParameters(3, PARM_DESCRIPTION, ds.MultipleScheduleGroups.DescriptionColumn.ColumnName, DbType.String);
           //dbManager.AddParameters(4, PARM_PROVIDER_ID, ds.MultipleScheduleGroups.ProviderIdColumn.ColumnName, DbType.String);
           //dbManager.AddParameters(5, PARM_RESOURCE_ID, ds.MultipleScheduleGroups.ResourceIdColumn.ColumnName, DbType.String);
           dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.MultipleScheduleGroups.IsActiveColumn.ColumnName, DbType.Byte);
           dbManager.AddParameters(5, PARM_CREATED_BY, ds.MultipleScheduleGroups.CreatedByColumn.ColumnName, DbType.String);
           dbManager.AddParameters(6, PARM_CREATED_ON, ds.MultipleScheduleGroups.CreatedOnColumn.ColumnName, DbType.DateTime);
           dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.MultipleScheduleGroups.ModifiedByColumn.ColumnName, DbType.String);
           dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.MultipleScheduleGroups.ModifiedOnColumn.ColumnName, DbType.DateTime);

       }


       private void CreateSchGroupProviderParameters(IDBManager dbManager, DSScheduleGroup ds, Boolean IsInsert)
       {
           dbManager.CreateParameters(ds.Tables[ds.GroupProviders.TableName].Columns.Count);

           if (IsInsert == true)
               dbManager.AddParameters(0, PARM_GROUP_PROVIDER_ID, ds.GroupProviders.GrpProvidersIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
           else
               dbManager.AddParameters(0, PARM_GROUP_PROVIDER_ID, ds.GroupProviders.GrpProvidersIdColumn.ColumnName, DbType.Int32);
           dbManager.AddParameters(1, PARM_MSGROUP_ID, ds.GroupProviders.MSGroupIdColumn.ColumnName, DbType.Int32);
           dbManager.AddParameters(2, PARM_PROVIDER_ID, ds.GroupProviders.ProviderIdColumn.ColumnName, DbType.Int64);
           dbManager.AddParameters(3, PARM_FACILITY_ID, ds.GroupProviders.FacilityIdColumn.ColumnName, DbType.Int64);
           dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.GroupProviders.IsActiveColumn.ColumnName, DbType.Byte);
           dbManager.AddParameters(5, PARM_CREATED_BY, ds.GroupProviders.CreatedByColumn.ColumnName, DbType.String);
           dbManager.AddParameters(6, PARM_CREATED_ON, ds.GroupProviders.CreatedOnColumn.ColumnName, DbType.DateTime);
           dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.GroupProviders.ModifiedByColumn.ColumnName, DbType.String);
           dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.GroupProviders.ModifiedOnColumn.ColumnName, DbType.DateTime);

           dbManager.AddParameters(9, PARM_PROVIDER_SCHEDULE_ID, ds.GroupProviders.ProScheduleIdColumn.ColumnName, DbType.Int64);
       }

       private void CreateSchGroupResourceParameters(IDBManager dbManager, DSScheduleGroup ds, Boolean IsInsert)
       {
           dbManager.CreateParameters(ds.Tables[ds.GroupResources.TableName].Columns.Count);

           if (IsInsert == true)
               dbManager.AddParameters(0, PARM_GROUP_RESOURCE_ID, ds.GroupResources.GrpResourcesIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
           else
               dbManager.AddParameters(0, PARM_GROUP_RESOURCE_ID, ds.GroupResources.GrpResourcesIdColumn.ColumnName, DbType.Int32);
           dbManager.AddParameters(1, PARM_MSGROUP_ID, ds.GroupResources.MSGroupIdColumn.ColumnName, DbType.Int32);
           dbManager.AddParameters(2, PARM_RESOURCE_ID, ds.GroupResources.ResourceIdColumn.ColumnName, DbType.Int64);
           dbManager.AddParameters(3, PARM_FACILITY_ID, ds.GroupResources.FacilityIdColumn.ColumnName, DbType.Int64);
           dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.GroupResources.IsActiveColumn.ColumnName, DbType.Byte);
           dbManager.AddParameters(5, PARM_CREATED_BY, ds.GroupResources.CreatedByColumn.ColumnName, DbType.String);
           dbManager.AddParameters(6, PARM_CREATED_ON, ds.GroupResources.CreatedOnColumn.ColumnName, DbType.DateTime);
           dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.GroupResources.ModifiedByColumn.ColumnName, DbType.String);
           dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.GroupResources.ModifiedOnColumn.ColumnName, DbType.DateTime);

           dbManager.AddParameters(9, PARM_RESOURCE_SCHEDULE_ID, ds.GroupResources.ResScheduleIdColumn.ColumnName, DbType.Int64);
       }

       #endregion

       #region "Insert, delete, update and get using dataset Functions"

       public DSScheduleGroup InsertScheduleGroups(DSScheduleGroup ds)
       {
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {
               dbManager.Open();
               CreateParameters(dbManager, ds, true);
               ds = (DSScheduleGroup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_GROUPS_INSERT, ds, ds.MultipleScheduleGroups.TableName);
               ds.AcceptChanges();
               return ds;
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::InsertScheduleGroups", PROC_SCHEDULE_GROUPS_INSERT, ex);
               throw ex;
           }
           finally
           {
               dbManager.Dispose();
           }
       }
       public DSScheduleGroup LoadScheduleGroups(int MSGroupId, string ShortName, string Description, string IsActive, string EntityId)
       {
           DSScheduleGroup ds = new DSScheduleGroup();
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {

               if (ShortName == "")
                   ShortName = null;
               if (Description == "")
                   Description = null;
               if (IsActive == "")
                   IsActive = null;
               if (EntityId == "")
                   EntityId = null;

               dbManager.Open();
               dbManager.CreateParameters(6);

               if (MSGroupId == 0)
                   dbManager.AddParameters(0, PARM_MSGROUP_ID, null);
               else
                   dbManager.AddParameters(0, PARM_MSGROUP_ID, MSGroupId);

               dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
               dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);
               if (EntityId == null)
               {
                   if ( MDVSession.Current.IsAdmin)
                       dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                   else
                       dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
               }
               else
                   dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
               dbManager.AddParameters(4, PARM_DESCRIPTION, Description);
               dbManager.AddParameters(5, PARM_IS_ACTIVE, IsActive);

               ds = (DSScheduleGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_GROUPS_SELECT, ds, ds.MultipleScheduleGroups.TableName);
               return ds;
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::LoadScheduleGroups", PROC_SCHEDULE_GROUPS_SELECT, ex);
               throw ex;
           }
           finally
           {
               dbManager.Dispose();
           }
       }
       public string DeleteScheduleGroups(string MSGroupId)
       {
           string returnVal = "";
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {
               dbManager.Open();
               dbManager.CreateParameters(2);
               dbManager.AddParameters(0, PARM_MSGROUP_ID, MSGroupId);
               dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
               returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SCHEDULE_GROUPS_DELETE).ToString();

               if (returnVal != "")
                   throw new Exception(returnVal);

               return "";
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::DeleteScheduleGroups", PROC_SCHEDULE_GROUPS_DELETE, ex);
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
       public DSScheduleGroup UpdateScheduleGroups(DSScheduleGroup ds)
       {
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {
               dbManager.Open();
               this.CreateParameters(dbManager, ds, false);
               ds = (DSScheduleGroup)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_GROUPS_UPDATE, ds, ds.MultipleScheduleGroups.TableName);
               ds.AcceptChanges();
               return ds;
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::UpdateScheduleGroups", PROC_SCHEDULE_GROUPS_UPDATE, ex);
               throw ex;
           }
           finally
           {
               dbManager.Dispose();
           }
       }

       public DSScheduleGroup LoadScheduleResources()
       {
           DSScheduleGroup ds = new DSScheduleGroup();
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {               

               dbManager.Open();
               dbManager.CreateParameters(2);

               if ( MDVSession.Current.IsAdmin)
                   dbManager.AddParameters(0, PARM_ENTITY_ID, null);
               else
                   dbManager.AddParameters(0, PARM_ENTITY_ID,  MDVSession.Current.EntityId);

               dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);

               ds = (DSScheduleGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCH_RESOURCE_SELECT, ds, ds.ScheduleResource.TableName);
               return ds;
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::LoadScheduleResources", PROC_SCH_RESOURCE_SELECT, ex);
               throw ex;
               //Usual code              
           }
           finally
           {
               dbManager.Dispose();
           }
       }



       public DSScheduleGroup LoadScheduleProvider()
       {
           DSScheduleGroup ds = new DSScheduleGroup();
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {
               

               dbManager.Open();
               dbManager.CreateParameters(2);



               if ( MDVSession.Current.IsAdmin)
                   dbManager.AddParameters(0, PARM_ENTITY_ID, null);
               else
                   dbManager.AddParameters(0, PARM_ENTITY_ID,  MDVSession.Current.EntityId);

               dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);

               ds = (DSScheduleGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCH_PROVIDER_SELECT, ds, ds.ScheduleProvider.TableName);
               return ds;
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::LoadScheduleProvider", PROC_SCH_PROVIDER_SELECT, ex);
               throw ex;
           }
           finally
           {
               dbManager.Dispose();
           }
       }



       public DSScheduleGroup LoadSchGroupProvResByID(long MSGroupId)
       {
           DSScheduleGroup ds = new DSScheduleGroup();
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {

               

               dbManager.Open();
               dbManager.CreateParameters(1);

               if (MSGroupId <= 0)
                   dbManager.AddParameters(0, PARM_MSGROUP_ID, null);
               else
                   dbManager.AddParameters(0, PARM_MSGROUP_ID, MSGroupId);



               ds = (DSScheduleGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHGROUP_PRO_RES_SELECT, ds, ds.ScheduleGroupsProRes.TableName);
               return ds;
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::LoadSchGroupProvResByID", PROC_SCHGROUP_PRO_RES_SELECT, ex);
               throw ex;
           }
           finally
           {
               dbManager.Dispose();
           }
       }



       public DSScheduleGroup LoadGroupProvider(long MSGroupId)
       {
           DSScheduleGroup ds = new DSScheduleGroup();
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {



               dbManager.Open();
               dbManager.CreateParameters(1);

               if (MSGroupId <= 0)
                   dbManager.AddParameters(0, PARM_MSGROUP_ID, null);
               else
                   dbManager.AddParameters(0, PARM_MSGROUP_ID, MSGroupId);



               ds = (DSScheduleGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GROUP_PROVIDER_SELECT, ds, ds.GroupProviders.TableName);
               return ds;
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::LoadGroupProvider", PROC_GROUP_PROVIDER_SELECT, ex);
               throw ex;
           }
           finally
           {
               dbManager.Dispose();
           }
       }



       public DSScheduleGroup LoadGroupResource(long MSGroupId)
       {
           DSScheduleGroup ds = new DSScheduleGroup();
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {



               dbManager.Open();
               dbManager.CreateParameters(1);

               if (MSGroupId <= 0)
                   dbManager.AddParameters(0, PARM_MSGROUP_ID, null);
               else
                   dbManager.AddParameters(0, PARM_MSGROUP_ID, MSGroupId);



               ds = (DSScheduleGroup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GROUP_RESOURCE_SELECT, ds, ds.GroupResources.TableName);
               return ds;
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::LoadGroupResource", PROC_GROUP_RESOURCE_SELECT, ex);
               throw ex;
           }
           finally
           {
               dbManager.Dispose();
           }
       }



       public string DeleteGroupProvider(string GrpProvidersId)
       {
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {
               dbManager.Open();
               dbManager.CreateParameters(1);
               dbManager.AddParameters(0, PARM_GROUP_PROVIDER_ID, GrpProvidersId);
               dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_GROUP_PROVIDER_DELETE);
               return "";
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::DeleteGroupProvider", PROC_GROUP_PROVIDER_DELETE, ex);
               return ex.Message;
           }
           finally
           {
               dbManager.Dispose();
           }
       }



       public string DeleteGroupResource(string GrpResourcesId)
       {
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {
               dbManager.Open();
               dbManager.CreateParameters(1);
               dbManager.AddParameters(0, PARM_GROUP_RESOURCE_ID, GrpResourcesId);
               dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_GROUP_RESOURCE_DELETE);
               return "";
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::DeleteGroupResource", PROC_GROUP_RESOURCE_DELETE, ex);
               return ex.Message;
           }
           finally
           {
               dbManager.Dispose();
           }
       }


       public DSScheduleGroup InsertSchGroupProvider(ref DSScheduleGroup ds)
       {
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {
               dbManager.Open();
               CreateSchGroupProviderParameters(dbManager, ds, true);
               ds = (DSScheduleGroup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_GROUP_PROVIDER_INSERT, ds, ds.GroupProviders.TableName);
               ds.AcceptChanges();
               return ds;
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::InsertSchGroupProvider", PROC_GROUP_PROVIDER_INSERT, ex);
               throw ex;
           }
           finally
           {
               dbManager.Dispose();
           }
       }


       public DSScheduleGroup InsertSchGroupResource(ref DSScheduleGroup ds)
       {
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {
               dbManager.Open();
               CreateSchGroupResourceParameters(dbManager, ds, true);
               ds = (DSScheduleGroup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_GROUP_RESOURCE_INSERT, ds, ds.GroupResources.TableName);
               ds.AcceptChanges();
               return ds;
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::InsertSchGroupResource", PROC_GROUP_RESOURCE_INSERT, ex);
               throw ex;
           }
           finally
           {
               dbManager.Dispose();
           }
       }

       #endregion

       #region "Lookups"
       public DSScheduleLookups LookupScheduleGroups()
       {
           DSScheduleLookups ds = new DSScheduleLookups();
           IDBManager dbManager = ClientConfiguration.GetDBManager();
           try
           {
               dbManager.Open();
               dbManager.CreateParameters(2);
               dbManager.AddParameters(0, PARM_USER_ID,  MDVSession.Current.AppUserId);
               if ( MDVSession.Current.IsAdmin)
                   dbManager.AddParameters(1, PARM_ENTITY_ID, null);
               else
                   dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
               ds = (DSScheduleLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SCHEDULE_GROUPS_LOOKUP, ds,ds.MultipleSchedualGroups.TableName);
               return ds;
           }
           catch (Exception ex)
           {
               MDVLogger.DALErrorLog("DALScheduleGroups::LookupScheduleGroups", PROC_SCHEDULE_GROUPS_LOOKUP, ex);
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
