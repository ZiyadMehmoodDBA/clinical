using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MDVision.Common.Logging;
using MDVision.Model.Common;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Admin.FollowUp
{
    public class DALFollowUpRemittanceCode
    {
        #region Variable

        private SharedVariable SharedVariable;

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_REMITTANCE_CODE_INSERT = "Patient.sp_RemittanceCodeInsert";
        private const string PROC_REMITTANCE_CODE_UPDATE = "Patient.sp_RemittanceCodeUpdate";
        private const string PROC_REMITTANCE_CODE_DELETE = "Patient.sp_RemittanceCodeDelete";
        private const string PROC_REMITTANCE_CODE_SELECT = "Patient.sp_RemittanceCodeSelect";
        private const string PROC_REMITTANCE_CODE_LOOKUP = "Patient.sp_RemittanceCodeLookup";
        #endregion

        #region "Parameters"

        private const string PARM_REMITTANCE_ID = "@RemittanceId";
        private const string PARM_CODE = "@Code";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_REJECTION = "@Rejection";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors
        public DALFollowUpRemittanceCode()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        public DALFollowUpRemittanceCode(SharedVariable SharedVariable)
        {
            this.SharedVariable = SharedVariable;
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
            components = new Container();
        }
        #endregion

        #region "Support Functions"

        /// <summary>
        /// Create The Parameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void CreateParameters(IDBManager dbManager, DSFollowUp ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_REMITTANCE_ID, ds.RemittanceCode.RemittanceIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_REMITTANCE_ID, ds.RemittanceCode.RemittanceIdColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(1, PARM_CODE, ds.RemittanceCode.CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.RemittanceCode.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_REJECTION, ds.RemittanceCode.RejectionColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.RemittanceCode.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.RemittanceCode.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.RemittanceCode.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.RemittanceCode.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFIED_ON, ds.RemittanceCode.ModifiedOnColumn.ColumnName, DbType.DateTime);

            //dbManager.AddParameters(9, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"        

        public DSPaymentLookup LookupRemittanceCode()
        {
            DSPaymentLookup ds = new DSPaymentLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPaymentLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMITTANCE_CODE_LOOKUP, ds, ds.RemittanceCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpRemittanceCode::LookupRemittanceCode", PROC_REMITTANCE_CODE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<GenericLookupModel> LookupRemittanceCodes()
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<GenericLookupModel> remitCodes = new List<GenericLookupModel>();
            try
            {
                dbManager.Open();
                var reader = dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_REMITTANCE_CODE_LOOKUP);
                while (reader.Read())
                {
                    GenericLookupModel obj = new GenericLookupModel()
                    {
                        Name = MDVUtility.ToStr(reader["Code"]),
                        Value = MDVUtility.ToStr(reader["RemittanceId"])
                    };
                    remitCodes.Add(obj);
                }
                return remitCodes;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpRemittanceCode::LookupRemittanceCodes", PROC_REMITTANCE_CODE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSFollowUp LoadRemittanceCode(int RemittanceId, string Code, string Description, string Rejection, int PageNumber = 1, int RowsPerPage = 1000, string IsActive = "")
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Code == "")
                    Code = null;
                if (Description == "")
                    Description = null;
                if (Rejection == "")
                    Rejection = null;
                if (IsActive == "")
                    IsActive = null;

                dbManager.Open();
                dbManager.CreateParameters(8);
                if (RemittanceId <= 0)
                    dbManager.AddParameters(0, PARM_REMITTANCE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REMITTANCE_ID, RemittanceId);

                dbManager.AddParameters(1, PARM_CODE, Code);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_REJECTION, Rejection);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.RemittanceCode.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(7, PARM_IS_ACTIVE, IsActive);
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REMITTANCE_CODE_SELECT, ds, ds.RemittanceCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpRemittanceCode::LoadRemittanceCode", PROC_REMITTANCE_CODE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp InsertRemittanceCode(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.RemittanceCode.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_REMITTANCE_CODE_INSERT, ds, ds.RemittanceCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.RemittanceCode.Rows[0][ds.RemittanceCode.RemittanceIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpRemittanceCode::InsertRemittanceCode", PROC_REMITTANCE_CODE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp UpdateRemittanceCode(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.RemittanceCode.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_REMITTANCE_CODE_UPDATE, ds, ds.RemittanceCode.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.RemittanceCode.Rows[0][ds.RemittanceCode.RemittanceIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpRemittanceCode::UpdateRemittanceCode", PROC_REMITTANCE_CODE_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteRemittanceCode(string RemittanceId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSFollowUp ds = LoadRemittanceCode(Convert.ToInt16(RemittanceId), null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.RemittanceCode;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REMITTANCE_ID, RemittanceId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REMITTANCE_CODE_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.RemittanceCode.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.RemittanceCode.Rows[0][ds.RemittanceCode.RemittanceIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpRemittanceCode::DeleteRemittanceCode", PROC_REMITTANCE_CODE_DELETE, ex);
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
