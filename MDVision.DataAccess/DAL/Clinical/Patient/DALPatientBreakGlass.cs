using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Clinical.Patient
{
    public class DALPatientBreakGlass
    {
        #region Variable
        
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatientBreakGlass"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALPatientBreakGlass()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }
        private IContainer components;
        //PatientBreakGlass: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_PATUSER_BREAKGLASS_INSERT = "Patient.sp_PatUserBreakGlassInsert";
        private const string PROC_PATUSER_BREAKGLASS_DELETE = "Patient.sp_PatUserBreakGlassDelete";
        private const string PROC_PATUSER_BREAKGLASS_SELECT = "Patient.sp_PatUserBreakGlassSelect";
        private const string PROC_PATUSER_BREAKGLASS_UPDATE = "Patient.sp_PatUserBreakGlassUpdate";
        private const string PROC_PATUSR_BRKGLASSREASON_INSERT = "Patient.sp_PatUsrBrkGlassReasonInsert";
        #endregion
        #region Parameters
        private const string PARM_PAT_USER_BREAKGLASS_REASONID = "@PatUsrBrkGlassReasonId";
        private const string PARM_PAT_USER_BREAKGLASS_ID = "@PatUserBreakGlassId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_IS_BREAK_GLASS_ALLOW = "@IsBreakGlassAllow";
        private const string PARM_IS_BREAK_GLASS_ON = "@IsBreakGlassOn";
        private const string PARM_BREAK_REASON = "@BreakReason";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        #endregion
        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void createParameters(IDBManager dbManager, DSPatientBreakGlass ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(8);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PAT_USER_BREAKGLASS_ID, ds.PatUserBreakGlass.PatUserBreakGlassIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PAT_USER_BREAKGLASS_ID, ds.PatUserBreakGlass.PatUserBreakGlassIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_USER_ID, ds.PatUserBreakGlass.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.PatUserBreakGlass.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_IS_BREAK_GLASS_ALLOW, ds.PatUserBreakGlass.IsBreakGlassAllowColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.PatUserBreakGlass.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.PatUserBreakGlass.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.PatUserBreakGlass.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.PatUserBreakGlass.ModifiedOnColumn.ColumnName, DbType.DateTime);


        }
        private void createParametersReason(IDBManager dbManager, DSPatientBreakGlass ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PAT_USER_BREAKGLASS_REASONID, ds.PatUsrBrkGlassReason.PatUsrBrkGlassReasonIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PAT_USER_BREAKGLASS_REASONID, ds.PatUsrBrkGlassReason.PatUsrBrkGlassReasonIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_USER_ID, ds.PatUsrBrkGlassReason.UserIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_ID, ds.PatUsrBrkGlassReason.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_BREAK_REASON, ds.PatUsrBrkGlassReason.BreakReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_BREAK_GLASS_ON, ds.PatUsrBrkGlassReason.IsBreakGlassOnColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.PatUsrBrkGlassReason.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.PatUsrBrkGlassReason.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.PatUsrBrkGlassReason.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.PatUsrBrkGlassReason.ModifiedOnColumn.ColumnName, DbType.DateTime);


        }
        #endregion

        #region "Insert, delete, update and get PatientBreakGlass using dataset Functions"
        /// <summary>
        /// Loads the Clinical.
        /// </summary>
        /// <param name="ClinicalId">The Clinical identifier.</param>
        /// <param name="FirstName">The first name.</param>
        /// <param name="LastName">The last name.</param>
        /// <param name="AccountNumber">The account number.</param>
        /// <param name="SSN">The SSN.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        /// 


        public DSPatientBreakGlass loadPatientBreakGlass(long UserId, long PatientId, bool? IsBreakGlassAllow)
        {
            DSPatientBreakGlass ds = new DSPatientBreakGlass();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (UserId == 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, UserId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                if (IsBreakGlassAllow != null)
                {
                    dbManager.AddParameters(2, PARM_IS_BREAK_GLASS_ALLOW, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_IS_BREAK_GLASS_ALLOW, IsBreakGlassAllow);
                }

                ds = (DSPatientBreakGlass)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATUSER_BREAKGLASS_SELECT, ds, ds.PatUserBreakGlass.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientBreakGlass::loadPatientBreakGlass", PROC_PATUSER_BREAKGLASS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        ///// <summary>
        /// Updates the PatientBreakGlass.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public string updatePatientBreakGlass(long UserId, long PatientId, string ModifiedBy, DateTime ModifiedOn, bool IsBreakGlassAllow = false)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                if (UserId == 0)
                    dbManager.AddParameters(0, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_USER_ID, UserId);
                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                if (IsBreakGlassAllow == false)
                {
                    dbManager.AddParameters(2, PARM_IS_BREAK_GLASS_ALLOW, "0");
                }
                else
                {
                    dbManager.AddParameters(2, PARM_IS_BREAK_GLASS_ALLOW, "1");
                }

                if (ModifiedBy == "")
                    dbManager.AddParameters(3, PARM_MODIFIED_BY, null);
                else
                    dbManager.AddParameters(3, PARM_MODIFIED_BY, ModifiedBy);

                dbManager.AddParameters(4, PARM_MODIFIED_ON, DateTime.Now);
                //this.createParameters(dbManager, ds, false);
                object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATUSER_BREAKGLASS_UPDATE);
                if (returnVal != null)
                    throw new Exception("");

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientBreakGlass::UpdatePatientBreakGlass", PROC_PATUSER_BREAKGLASS_UPDATE, ex);
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
        /// Deletes the PatientBreakGlass.
        /// </summary>
        /// <param name="MsgId">The PatientBreakGlass identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string deletePatientBreakGlass(string PatientBreakGlassId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PAT_USER_BREAKGLASS_ID, PatientBreakGlassId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATUSER_BREAKGLASS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientBreakGlass::DeletePatientBreakGlass", PROC_PATUSER_BREAKGLASS_DELETE, ex);
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
        /// Inserts the PatientBreakGlass.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSPatientBreakGlass insertPatientBreakGlass(DSPatientBreakGlass ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParameters(dbManager, ds, true);
                ds = (DSPatientBreakGlass)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATUSER_BREAKGLASS_INSERT, ds, ds.PatUserBreakGlass.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientBreakGlass::InsertPatientBreakGlass", PROC_PATUSER_BREAKGLASS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSPatientBreakGlass insertPatientBreakGlassReason(DSPatientBreakGlass ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createParametersReason(dbManager, ds, true);
                ds = (DSPatientBreakGlass)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATUSR_BRKGLASSREASON_INSERT, ds, ds.PatUsrBrkGlassReason.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientBreakGlass::insertPatientBreakGlassReason", PROC_PATUSR_BRKGLASSREASON_INSERT, ex);
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
