using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Model.Lookups;
using System.Data.SqlClient;
using MDVision.Model;
using MDVision.Common.Logging;
using MDVision.Model.Patient;
using MDVision.Common.Utilities;
using MDVision.Model.Native.Patient;
using System.Diagnostics;

namespace MDVision.DataAccess.DAL.Admin
{
  public  class DALLoginMobile
    {
        #region parameters
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_ENTITY_REG_CODE = "@EntityRegCode";
        #endregion parameters

        #region StoredProcedures
        private const string PROC_ENTITY_USER_OPTION_SELECT = "system.sp_EntityUserOptionSelect";
        #endregion StoredProcedures
        public DALLoginMobile()
        {
            InitializeComponent();
          //  ClientConfiguration.SetClientObject();

        }
        public DALLoginMobile(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
        }

        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>

        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #region "Stored Procedure Names"
      
        private const string PROC_USER_LOGIN_INSERT_UPDATE = "System.sp_InsertUpdateDeviceIdExpirationTimeNative";
        private const string PROC_USER_LOGOUT = "System.sp_LogOutMobileUserNative";
        private const string PROC_LOAD_DEVICEID = "System.sp_LoadDevicesNative";

        #endregion
        #region Parameters
        private const string PARM_PATIENT_ID = "@PatientId";
       
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_DEVICE_ID = "@DeviceId";
        private const string PARM_EXPIRATION_TIME = "@ExpirationTime";
        private const string PARM_GRANT_TYPE = "@GrantType";

        #endregion
        public string InsertUpdateMobileLogin(IDBManager dbManager,string DeviceId, DateTimeOffset ExpirationTime, string GrantType)
        {
            string returnVal = "";
            //DSDBAudit dsDBAudit = new DSDBAudit();


            //  IDBManager dbManager = ClientConfiguration.GetDBManager();
            dbManager.Open();
            try
            {
                //DataTable dtTemp = ds.Patients.GetChanges();

                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);


                dbManager.AddParameters(0, PARM_DEVICE_ID, DeviceId);
                dbManager.AddParameters(1, PARM_EXPIRATION_TIME, ExpirationTime);
                dbManager.AddParameters(2, PARM_GRANT_TYPE, GrantType);



                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_USER_LOGIN_INSERT_UPDATE).ToString();


                if (returnVal != "-1")
                {
                    dbManager.RollBackTransaction();
                    throw new Exception(returnVal);

                }
                else
                {
                    dbManager.CommitTransaction();

                    return "";
                }
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Update Patient Pic", ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.PatientIdColumn.ColumnName].ToString());



            }
            catch (Exception ex)
            {


                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPatient::PROC_USER_LOGIN_INSERT_UPDATE", PROC_USER_LOGIN_INSERT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string LogOutMobileLogin(IDBManager dbManager, string DeviceId)
        {
            string returnVal = "";
            //DSDBAudit dsDBAudit = new DSDBAudit();
            

            try
            {
                //DataTable dtTemp = ds.Patients.GetChanges();

                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);


                dbManager.AddParameters(0, PARM_DEVICE_ID, DeviceId);
           



                returnVal = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_USER_LOGOUT).ToString();


                if (returnVal != "-1")
                {
                    dbManager.RollBackTransaction();
                    throw new Exception(returnVal);

                }
                else
                {
                    dbManager.CommitTransaction();

                    return "";
                }
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Update Patient Pic", ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.PatientIdColumn.ColumnName].ToString());



            }
            catch (Exception ex)
            {


                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPatient::PROC_USER_LOGIN_INSERT_UPDATE", PROC_USER_LOGOUT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DataSet LoadDeviceIds(IDBManager dbManager)
        {
            string returnVal = "";
            DataSet ds = new DataSet();
            //DSDBAudit dsDBAudit = new DSDBAudit();
          

            try
            {
                //DataTable dtTemp = ds.Patients.GetChanges();

                dbManager.BeginTransaction();
              




                ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOAD_DEVICEID);


                if (ds.Tables[0].Rows.Count == 0)
                {
                    dbManager.RollBackTransaction();
                    // throw new Exception(returnVal);
                    return ds;

                }
                else
                {
                    dbManager.CommitTransaction();

                    return ds;
                }
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Update Patient Pic", ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.PatientIdColumn.ColumnName].ToString());



            }
            catch (Exception ex)
            {


                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPatient::PROC_LOAD_DEVICEID", PROC_LOAD_DEVICEID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public List<Model.User.EntityUserOptions> LoadEntityUserOptionForService(IDBManager dbManager,string userName, string entityId, string entityRegCode = null)
        {
           
            try
            {

                dbManager.AddParameters(PARM_ENTITY_ID, entityId);
                dbManager.AddParameters(PARM_USER_NAME, userName);
                dbManager.AddParameters(PARM_ENTITY_REG_CODE, entityRegCode);
                return dbManager.ExecuteReaders<Model.User.EntityUserOptions>(PROC_ENTITY_USER_OPTION_SELECT);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DLLUser::LoadEntityUserOption", PROC_ENTITY_USER_OPTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



    }
}
