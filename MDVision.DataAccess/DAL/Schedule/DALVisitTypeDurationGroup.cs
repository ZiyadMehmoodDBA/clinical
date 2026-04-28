using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.DataAccess.DAL.Schedule
{
    public class DALVisitTypeDurationGroup
    {
        #region "Stored Procedure Names"
        private const string PROC_VISIT_TYPE_DURATION_GROUP_SELECT = "Patient.sp_VisitDurationGroupsSelect";
        private const string PROC_VISIT_TYPE_DURATION_GROUP_LOOKUP = "Patient.sp_VisitTypeDurationGroupLookup";
        private const string PROC_VISIT_TYPE_DURATION_GROUP_INSERT = "Patient.sp_VisitDurationGroupsInsert";
        private const string PROC_VISIT_TYPE_DURATION_GROUP_UPDATE = "Patient.sp_VisitDurationGroupsUpdate";
        private const string PROC_VISIT_TYPE_DURATION_GROUP_ACTIVE_INACTIVE = "Patient.sp_VisitDurationGroupActiveInActive";
        private const string PROC_VISIT_TYPE_DURATION_GROUP_DELETE = "Patient.sp_VisitDurationGroupsDelete";
        private const string PROC_VISIT_TYPE_DURATIONS_MAPPING_INSERT = "Patient.sp_VisitDurationGroupMappingInsert";
        private const string PROC_VISIT_TYPE_DURATIONS_MAPPING_SELECT = "Patient.sp_VisitDurationGroupMappingSelect";
        private const string PROC_VISIT_TYPE_DURATIONS_MAPPING_UPDATE = "Patient.sp_VisitDurationGroupMappingUpdate";
        private const string PROC_VISIT_TYPE_DURATION_ON_VISIT_TYPE_SELECT = "Patient.sp_VisitDurationOnVisitTypeSelect";
        
        #endregion

        #region "Parameters"
        private const string PARM_VISIT_TYPE_GROUP_ID = "@Id";
        private const string PARM_VISIT_TYPE_DURATION_GROUP_ID = "@VisitDurationGroupId";
        private const string PARM_PATIENT_TYPE_ID = "@PatientVisitTypeId";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_PATIENT_VISIT_TYPE_ID = "@PatientVisitTypeId";
        private const string PARM_VISIT_TYPE_DURATION_GROUP_NAME = "@Name";
        private const string PARM_VISIT_DURATION_GROUP_NAME = "@Groupname";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_GROUP_NAME = "@Groupname";
        private const string PARM_COLOR = "@Color";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        #endregion

        #region "Constructors"

        public DALVisitTypeDurationGroup()
        {
            ClientConfiguration.SetClientObject();
        }

        #endregion

        #region "Get, Insert, Update, Delete" 
        
        public DSScheduleSetup LoadVisitTypeDurationGroup(long VisitTypeDurationGroupId, string Name, string IsActive, string EntityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSScheduleSetup ds = new DSScheduleSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);
                if (VisitTypeDurationGroupId <= 0)
                    dbManager.AddParameters(0, PARM_VISIT_TYPE_GROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_TYPE_GROUP_ID, VisitTypeDurationGroupId);
                dbManager.AddParameters(1, PARM_VISIT_TYPE_DURATION_GROUP_NAME, Name == "" ? null : Name);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive == ""? null: IsActive);
                if (string.IsNullOrEmpty(EntityId))
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.VisitTypeDurationGroup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
               
                ds = (DSScheduleSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VISIT_TYPE_DURATION_GROUP_SELECT, ds, ds.VisitTypeDurationGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisitTypeDurationGroup::LoadVisitTypeDurationGroup", PROC_VISIT_TYPE_DURATION_GROUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSScheduleSetup LoadVisitTypeDurationGroupForm(long VisitTypeDurationGroupId, string Name, string IsActive, string EntityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSScheduleSetup ds = new DSScheduleSetup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);
                if (VisitTypeDurationGroupId <= 0)
                    dbManager.AddParameters(0, PARM_VISIT_TYPE_GROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_VISIT_TYPE_GROUP_ID, VisitTypeDurationGroupId);
                dbManager.AddParameters(1, PARM_VISIT_TYPE_DURATION_GROUP_NAME, Name == "" ? null : Name);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive == "" ? null : IsActive);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.VisitTypeDurationGroup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSScheduleSetup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VISIT_TYPE_DURATIONS_MAPPING_SELECT, ds, ds.VisitTypeDurationGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisitTypeDurationGroup::LoadVisitTypeDurationGroup", PROC_VISIT_TYPE_DURATIONS_MAPPING_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSScheduleLookups LookupVisitTypeDurationGroup()
        {
            DSScheduleLookups ds = new DSScheduleLookups();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                ds = (DSScheduleLookups)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_VISIT_TYPE_DURATION_GROUP_LOOKUP, ds, ds.VisitTypeDurationGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisitTypeDurationGroup::LookupVisitTypeDurationGroup", PROC_VISIT_TYPE_DURATION_GROUP_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        
        public DSScheduleSetup InsertVisitTypeDurationGroup(DSScheduleSetup ds, Boolean IsInsert)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);
                if (IsInsert)
                    dbManager.AddParameters(0, PARM_VISIT_TYPE_GROUP_ID, ds.VisitTypeDurationGroup.IdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                else
                    dbManager.AddParameters(0, PARM_VISIT_TYPE_GROUP_ID, ds.VisitTypeDurationGroup.IdColumn.ColumnName, DbType.Int64);

                dbManager.AddParameters(1, PARM_VISIT_TYPE_DURATION_GROUP_NAME, ds.VisitTypeDurationGroup.NameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, ds.VisitTypeDurationGroup.IsActiveColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, PARM_ENTITY_ID, ds.VisitTypeDurationGroup.EntityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(4, PARM_CREATED_BY, ds.VisitTypeDurationGroup.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, PARM_CREATED_ON, ds.VisitTypeDurationGroup.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.VisitTypeDurationGroup.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.VisitTypeDurationGroup.ModifiedOnColumn.ColumnName, DbType.DateTime);

                ds = (DSScheduleSetup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_VISIT_TYPE_DURATION_GROUP_INSERT, ds, ds.VisitTypeDurationGroup.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisitTypeDurationGroup::InsertVisitTypeDurationGroup", PROC_VISIT_TYPE_DURATION_GROUP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string InsertVisitTypeDurations(Int64 VisitTypeId, Int64 VisitGroupId, Int64 VisitDurationId, float Duration, bool IsActive, string CreatedBy, DateTime CreatedOn, string ModifiedBy, DateTime ModifiedOn,string color)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                int retVal;
                dbManager.Open();
                dbManager.CreateParameters(10);
                
                dbManager.AddParameters(0, PARM_VISIT_TYPE_GROUP_ID, MDVUtility.ToStr(VisitTypeId));

                dbManager.AddParameters(1, PARM_VISIT_TYPE_DURATION_GROUP_ID, MDVUtility.ToStr(VisitGroupId));
                dbManager.AddParameters(2, PARM_PATIENT_TYPE_ID, MDVUtility.ToStr(VisitDurationId));
                dbManager.AddParameters(3, PARM_DURATION, MDVUtility.ToStr(Duration));
                dbManager.AddParameters(4, PARM_IS_ACTIVE, MDVUtility.ToStr(IsActive));
                dbManager.AddParameters(5, PARM_CREATED_BY, MDVUtility.ToStr(CreatedBy));
                dbManager.AddParameters(6, PARM_CREATED_ON, MDVUtility.ToStr(CreatedOn));
                dbManager.AddParameters(7, PARM_MODIFIED_BY, MDVUtility.ToStr(ModifiedBy));
                dbManager.AddParameters(8, PARM_MODIFIED_ON, MDVUtility.ToStr(ModifiedOn));
                dbManager.AddParameters(9, PARM_COLOR, MDVUtility.ToStr(color));

                retVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_VISIT_TYPE_DURATIONS_MAPPING_INSERT);
                
                return MDVUtility.ToStr(retVal);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisitTypeDurationGroup::InsertVisitTypeDurations", PROC_VISIT_TYPE_DURATIONS_MAPPING_INSERT, ex);
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

        public string UpdateVisitTypeDurationGroup(string GroupName, Int64 VisitTypeId, Int64 VisitGroupId, Int64 VisitDurationId, float Duration, bool IsActive, string CreatedBy, DateTime CreatedOn, string ModifiedBy, DateTime ModifiedOn, string EntityId,string color)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retVal;
                dbManager.Open();
                dbManager.CreateParameters(11);

                dbManager.AddParameters(0, PARM_VISIT_TYPE_GROUP_ID, MDVUtility.ToStr(VisitDurationId));

                dbManager.AddParameters(1, PARM_VISIT_TYPE_DURATION_GROUP_ID, MDVUtility.ToStr(VisitGroupId));
                dbManager.AddParameters(2, PARM_PATIENT_TYPE_ID, MDVUtility.ToStr(VisitTypeId));
                dbManager.AddParameters(3, PARM_DURATION, MDVUtility.ToStr(Duration));
                dbManager.AddParameters(4, PARM_IS_ACTIVE, MDVUtility.ToStr(IsActive));
                dbManager.AddParameters(5, PARM_MODIFIED_BY, MDVUtility.ToStr(ModifiedBy));
                dbManager.AddParameters(6, PARM_MODIFIED_ON, MDVUtility.ToStr(ModifiedOn));
                dbManager.AddParameters(7, PARM_GROUP_NAME, MDVUtility.ToStr(GroupName));
                if (string.IsNullOrEmpty(EntityId))
                    dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                else
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(8, PARM_ENTITY_ID, EntityId);
                    else
                    dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(9, PARM_COLOR, MDVUtility.ToStr(color));
                dbManager.AddParameters(10, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                retVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VISIT_TYPE_DURATIONS_MAPPING_UPDATE).ToString();
                if (retVal != "")
                    throw new Exception(retVal.ToString());


                return MDVUtility.ToStr(retVal);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisitTypeDurationGroup::UpdateVisitTypeDurationGroup", PROC_VISIT_TYPE_DURATION_GROUP_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteVisitTypeDurationGroup(string VisitTypeDurationGroupId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_VISIT_TYPE_GROUP_ID, VisitTypeDurationGroupId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VISIT_TYPE_DURATION_GROUP_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch(Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisitTypeDurationGroup::DeleteVisitTypeDurationGroup", PROC_VISIT_TYPE_DURATION_GROUP_DELETE, ex);
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
        public string UpdateVisitTypeDurationGroupActiveInActive(Int64 VisitTypeDurationGroupId, Int64 IsActive)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_VISIT_TYPE_DURATION_GROUP_ID, VisitTypeDurationGroupId);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);
                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VISIT_TYPE_DURATION_GROUP_ACTIVE_INACTIVE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisitTypeDurationGroup::UpdateVisitTypeDurationGroupActiveInActive", PROC_VISIT_TYPE_DURATION_GROUP_ACTIVE_INACTIVE, ex);
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
        public string LoadDurationOnVisitType(Int64 providerId, Int64 PatientVisitTypeId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_PROVIDER_ID, providerId);
                dbManager.AddParameters(1, PARM_PATIENT_VISIT_TYPE_ID, PatientVisitTypeId);
                dbManager.AddParameters(2, PARM_DURATION, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_VISIT_TYPE_DURATION_ON_VISIT_TYPE_SELECT).ToString();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALVisitTypeDurationGroup::LoadDurationOnVisitType", PROC_VISIT_TYPE_DURATION_ON_VISIT_TYPE_SELECT, ex);
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
    }
}
