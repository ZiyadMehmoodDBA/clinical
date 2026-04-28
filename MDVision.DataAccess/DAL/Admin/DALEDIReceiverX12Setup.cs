using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.ComponentModel;
using System.Data;
using MDVision.Common.Shared;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALEDIReceiverX12Setup
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_RECEIVER_SETUP_X12_INSERT = "Provider.sp_EDIReceiverX12SetupInsert";
        private const string PROC_RECEIVER_SETUP_X12_UPDATE = "Provider.sp_EDIReceiverX12SetupUpdate";
        //private const string PROC_RECEIVER_SETUP_X12_DELETE = "Provider.sp_EDIReceiverX12SetupDelete";
        private const string PROC_RECEIVER_SETUP_X12_SELECT = "Provider.sp_EDIReceiverX12SetupSelect";
        #endregion

        #region "Parameters"
        private const string PARM_RECEIVER_X12_ID = "@EDIReceiverX12SetupId";
        private const string PARM_RECEIVER_SETUP_ID = "@EDIReceiverSetupId";
        private const string PARM_AUTH_INFO_QUAL = "@ISA01";
        private const string PARM_AUTH_INFO = "@ISA02";
        private const string PARM_SECURITY_INFO_QUAL = "@ISA03";
        private const string PARM_SECURTY_INFO = "@Securityinfo";
        private const string PARM_INTER_SENDER_ID_QUAL = "@ISA05";
        private const string PARM_INTER_SENDER_ID = "@InterchangeSenderId";
        private const string PARM_INTER_RECEIVER_ID_QUAL = "@ISA07";
        private const string PARM_INTER_RECEIVER_ID = "@ISA08";
        private const string PARM_REPETITION_SEPARATOR = "@ISA11";
        private const string PARM_INTER_CONTROL_VERSION = "@ISA12";
        private const string PARM_INTERCHANGE_CONTROL_NUMBER = "@ISA13";
        private const string PARM_ACKNOWLEDGEMENT_REQUESTED = "@ISA14";
        private const string PARM_USAGE_INDICATOR = "@ISA15";
        private const string PARM_COMPONENT_ELEMENT_SEPARATOR = "@ISA16";
        private const string PARM_APPLICATION_SENDER_CODE = "@GS02";
        private const string PARM_APPLICATION_RECEIVER_CODE = "@GS03";
        private const string PARM_GROUP_CONTROL_NUMBER = "@GS06";
        private const string PARM_VERSION_RELEASE_ID = "@GS08";
        private const string PARM_TRAN_SETUP_PURPOSE_CODE = "@BHT02";
        private const string PARM_REFERENCE_ID = "@BHT03";
        private const string PARM_TRANSACTION_TYPE_CODE = "@BHT06";
        private const string PARM_TRAN_SETUP_CTRL_NUM = "@ST02Transaction";
        private const string PARM_IMPLEMENT_CONVENTION_REF = "@ST03";
        private const string PARM_RECEIVER_NAME = "@ReceiverName";
        private const string PARM_TNSHDLR_CLASS = "@TNSHDLRClass";
        private const string PARM_FILE_COUNTER = "@FileCounter";
        private const string PARM_RECEIVER_PRIMARY_ID_NUM = "@RX1000BNM109";
        private const string PARM_TO_DAY = "@ToDay";
        private const string PARM_ONE_ISA_ONE_FILE = "@ONEISA";
        private const string PARM_SEGMENT_TERMINATOR = "@SegmentTerminator";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALEDIReceiverX12Setup"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALEDIReceiverX12Setup()
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
        private void CreateParameters(IDBManager dbManager, DSEDI ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(ds.Tables[ds.EDIReceiverX12Setup.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_RECEIVER_X12_ID, ds.EDIReceiverX12Setup.EDIReceiverX12SetupIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_RECEIVER_X12_ID, ds.EDIReceiverX12Setup.EDIReceiverX12SetupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_RECEIVER_SETUP_ID, ds.EDIReceiverX12Setup.EDIReceiverSetupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_AUTH_INFO_QUAL, ds.EDIReceiverX12Setup.ISA01Column.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_AUTH_INFO, ds.EDIReceiverX12Setup.ISA02Column.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SECURITY_INFO_QUAL, ds.EDIReceiverX12Setup.ISA03Column.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_SECURTY_INFO, ds.EDIReceiverX12Setup.SecurityinfoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_INTER_SENDER_ID_QUAL, ds.EDIReceiverX12Setup.ISA05Column.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_INTER_SENDER_ID, ds.EDIReceiverX12Setup.InterchangeSenderIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_INTER_RECEIVER_ID_QUAL, ds.EDIReceiverX12Setup.ISA07Column.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_INTER_RECEIVER_ID, ds.EDIReceiverX12Setup.ISA08Column.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_REPETITION_SEPARATOR, ds.EDIReceiverX12Setup.ISA11Column.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_INTER_CONTROL_VERSION, ds.EDIReceiverX12Setup.ISA12Column.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_INTERCHANGE_CONTROL_NUMBER, ds.EDIReceiverX12Setup.ISA13Column.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_ACKNOWLEDGEMENT_REQUESTED, ds.EDIReceiverX12Setup.ISA14Column.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_USAGE_INDICATOR, ds.EDIReceiverX12Setup.ISA15Column.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_COMPONENT_ELEMENT_SEPARATOR, ds.EDIReceiverX12Setup.ISA16Column.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_APPLICATION_SENDER_CODE, ds.EDIReceiverX12Setup.GS02Column.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_APPLICATION_RECEIVER_CODE, ds.EDIReceiverX12Setup.GS03Column.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_GROUP_CONTROL_NUMBER, ds.EDIReceiverX12Setup.GS06Column.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_VERSION_RELEASE_ID, ds.EDIReceiverX12Setup.GS08Column.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_TRAN_SETUP_PURPOSE_CODE, ds.EDIReceiverX12Setup.BHT02Column.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_REFERENCE_ID, ds.EDIReceiverX12Setup.BHT03Column.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_TRANSACTION_TYPE_CODE, ds.EDIReceiverX12Setup.BHT06Column.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_TRAN_SETUP_CTRL_NUM, ds.EDIReceiverX12Setup.ST02TransactionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_IMPLEMENT_CONVENTION_REF, ds.EDIReceiverX12Setup.ST03Column.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_RECEIVER_NAME, ds.EDIReceiverX12Setup.ReceiverNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_TNSHDLR_CLASS, ds.EDIReceiverX12Setup.TNSHDLRClassColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_FILE_COUNTER, ds.EDIReceiverX12Setup.FileCounterColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_RECEIVER_PRIMARY_ID_NUM, ds.EDIReceiverX12Setup.RX1000BNM109Column.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_TO_DAY, ds.EDIReceiverX12Setup.ToDayColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_ONE_ISA_ONE_FILE, ds.EDIReceiverX12Setup.ONEISAColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(31, PARM_SEGMENT_TERMINATOR, ds.EDIReceiverX12Setup.SegmentTerminatorColumn.ColumnName, DbType.String);
            dbManager.AddParameters(32, PARM_IS_ACTIVE, ds.EDIReceiverX12Setup.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(33, PARM_CREATED_BY, ds.EDIReceiverX12Setup.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(34, PARM_CREATED_ON, ds.EDIReceiverX12Setup.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(35, PARM_MODIFIED_BY, ds.EDIReceiverX12Setup.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(36, PARM_MODIFIED_ON, ds.EDIReceiverX12Setup.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the receiver setup X12.
        /// </summary>
        /// <param name="ReceiverSetupId">The receiver setup identifier.</param>
        /// <returns></returns>
        public DSEDI LoadReceiverSetupX12(long ReceiverSetupId)
        {
            DSEDI ds = new DSEDI();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (ReceiverSetupId <= 0)
                    dbManager.AddParameters(0, PARM_RECEIVER_SETUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_RECEIVER_SETUP_ID, ReceiverSetupId);
                ds = (DSEDI)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RECEIVER_SETUP_X12_SELECT, ds, ds.EDIReceiverX12Setup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReceiverX12Setup::LoadReceiverSetupX12", PROC_RECEIVER_SETUP_X12_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the receiver setup X12.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSEDI UpdateReceiverSetupX12(DSEDI ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDIReceiverX12Setup.GetChanges();
                ds = (DSEDI)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_RECEIVER_SETUP_X12_UPDATE, ds, ds.EDIReceiverX12Setup.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDIReceiverX12Setup.Rows[0][ds.EDIReceiverX12Setup.EDIReceiverX12SetupIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReceiverX12Setup::UpdateReceiverSetupX12", PROC_RECEIVER_SETUP_X12_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //public string DeleteReceiverSetupX12(string ReceiverSetupX12Ids)
        //{
        //    object returnValue;
        //    IDBManager dbManager =ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(2);
        //        dbManager.AddParameters(0, PARM_POS_ID, ReceiverSetupX12Ids);
        //        dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        //        returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_RECEIVER_SETUP_X12_DELETE);
        //        if (returnValue != null)
        //            throw new Exception(returnValue.ToString());

        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALEDIReceiverX12Setup::DeleteReceiverSetupX12", PROC_RECEIVER_SETUP_X12_DELETE, ex);
        //        string[] str = ex.Message.Split('|');
        //        if (str.Length > 1)
        //            return str[1].ToString();
        //        else
        //            return ex.Message;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        /// <summary>
        /// Inserts the receiver setup X12.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSEDI InsertReceiverSetupX12(DSEDI ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.EDIReceiverX12Setup.GetChanges();
                ds = (DSEDI)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_RECEIVER_SETUP_X12_INSERT, ds, ds.EDIReceiverX12Setup.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.EDIReceiverX12Setup.Rows[0][ds.EDIReceiverX12Setup.EDIReceiverX12SetupIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReceiverX12Setup::InsertReceiverSetupX12", PROC_RECEIVER_SETUP_X12_INSERT, ex);
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
