using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin.PatientStatement
{
    public class DALPatientStatementMessage
    {
        #region Variable

        

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALSpecialty"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALPatientStatementMessage()
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

        #region "Stored Procedure Names"

        private const string PROC_STATEMENT_MESSAGE_INSERT = "System.sp_StatementMessageInsert";
        private const string PROC_STATEMENT_MESSAGE_UPDATE = "System.sp_StatementMessageUpdate";
        private const string PROC_STATEMENT_MESSAGE_SELECT = "System.sp_StatementMessageSelect";
        private const string PROC_STATEMENT_MESSAGE_DELETE = "System.sp_StatementMessageDelete";
        private const string PROC_STATEMENT_MESSAGE_LOOKUP = "System.sp_StatementMessageLookup";
        


        #endregion

        #region "Parameters"




        private const string PARM_STATEMENT_MESSAGE_ID = "@StmtMsgId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_MESSAGE = "@Message";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PAGE_ = "@RowspPage";



        #endregion


        #region "Support Functions"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSPatientStatement ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(8);


            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_STATEMENT_MESSAGE_ID, ds.StatementMessage.StmtMsgIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_STATEMENT_MESSAGE_ID, ds.StatementMessage.StmtMsgIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.StatementMessage.ShortNameColumn.ColumnName, DbType.String);

            dbManager.AddParameters(2, PARM_MESSAGE, ds.StatementMessage.MessageColumn.ColumnName, DbType.String);


            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.StatementMessage.IsActiveColumn.ColumnName, DbType.Byte);

            dbManager.AddParameters(4, PARM_CREATED_BY, ds.StatementMessage.CreatedByColumn.ColumnName, DbType.String);

            dbManager.AddParameters(5, PARM_CREATED_ON, ds.StatementMessage.CreatedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.StatementMessage.ModifiedByColumn.ColumnName, DbType.String);

            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.StatementMessage.ModifiedOnColumn.ColumnName, DbType.DateTime);


          //  dbManager.AddParameters(8, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);


        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSPatientStatement InsertStatementMessage(DSPatientStatement ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.StatementMessage.GetChanges();
                ds = (DSPatientStatement)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_STATEMENT_MESSAGE_INSERT, ds, ds.StatementMessage.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.StatementMessage.Rows[0][ds.StatementMessage.StmtMsgIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatement::InsertPatientStatement", PROC_STATEMENT_MESSAGE_INSERT, ex);
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

        public DSPatientStatement LoadStatementMessage(long statementMessageId, string shortName, string message, string isActive, int pageNumber, int RecordPerPage)
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (statementMessageId <= 0)
                    dbManager.AddParameters(0, PARM_STATEMENT_MESSAGE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_STATEMENT_MESSAGE_ID, statementMessageId);



                if (shortName == "")
                    dbManager.AddParameters(1, PARM_SHORT_NAME, null);
                else
                    dbManager.AddParameters(1, PARM_SHORT_NAME, shortName);

                if (message == "")
                    dbManager.AddParameters(2, PARM_MESSAGE, null);
                else
                    dbManager.AddParameters(2, PARM_MESSAGE, message);


                if (isActive == "")

                    dbManager.AddParameters(3, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(3, PARM_IS_ACTIVE, isActive);


                if (pageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, pageNumber);

                if (RecordPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWS_PAGE_, null);
                else
                    dbManager.AddParameters(5, PARM_ROWS_PAGE_, RecordPerPage);

                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.StatementMessage.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


            //    dbManager.AddParameters(4, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));



                ds = (DSPatientStatement)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_STATEMENT_MESSAGE_SELECT, ds, ds.StatementMessage.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatementMessage::LoadStatementMessage", PROC_STATEMENT_MESSAGE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientStatement UpdateStatementMessage(DSPatientStatement ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.StatementMessage.GetChanges();
                ds = (DSPatientStatement)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_STATEMENT_MESSAGE_UPDATE, ds, ds.StatementMessage.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.StatementMessage.Rows[0][ds.StatementMessage.StmtMsgIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatementMessage::UpdateStatementMenssage", PROC_STATEMENT_MESSAGE_UPDATE, ex);
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


        public string DeleteStatementMessage(long StatementMessageId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                //DSPatientStatement ds = LoadStatementMessage(StatementMessageId, "", "", "", 1, 15);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.StatementMessage;
                dbManager.AddParameters(0, PARM_STATEMENT_MESSAGE_ID, StatementMessageId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_STATEMENT_MESSAGE_DELETE).ToString();
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.StatementMessage.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.StatementMessage.Rows[0][ds.StatementMessage.StmtMsgIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatementMessage::DeleteStatementMessage", PROC_STATEMENT_MESSAGE_DELETE, ex);
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



        #region Lookup
        public DSPatientStatementLookup LookupStatementMessage()
        {
            DSPatientStatementLookup ds = new DSPatientStatementLookup(); 
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientStatementLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_STATEMENT_MESSAGE_LOOKUP, ds, ds.StatementMessageLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatementMessage::LookupStatementMessage", PROC_STATEMENT_MESSAGE_LOOKUP, ex);
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


