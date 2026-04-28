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
    public class DALPatientStatementGroup
    {
        #region Variable

        

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALSpecialty"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALPatientStatementGroup()
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

        private const string PROC_STATEMENT_GROUP_INSERT = "System.sp_PatientStatementGroupInsert";
        private const string PROC_STATEMENT_GROUP_UPDATE = "System.sp_PatientStatementGroupUpdate";
        private const string PROC_STATEMENT_GROUP_SELECT = "System.sp_PatientStatementGroupSelect";
        private const string PROC_STATEMENT_GROUP_DELETE = "System.sp_PatientStatementGroupDelete";
        private const string PROC_STATEMENT_GROUP_LOOKUP = "System.sp_PatientStatementGroupLookup";
        



        #endregion

        #region "Parameters"




        private const string PARM_STATEMENT_GROUP_ID = "@PtStmtGrpId";
        private const string PARM_NAME = "@Name";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_CYCLE_DAYS = "@CycleDays";
        private const string PARM_FIRST_MESSAGE_ID = "@FirstMsgId";
        private const string PARM_SECOND_MESSAGE_ID = "@SecondMsgId";
        private const string PARM_THIRD_MESSAGE_ID = "@ThirdMsgId";
        private const string PARM_FOURTH_MESSAGE_ID = "@FourthMsgId";
        private const string PARM_FIFTH_MESSAGE_ID = "@FifthMsgId";
        private const string PARM_OUTSTANDING_DAYS = "@OutStandingDays";
        private const string PARM_NO_OF_STATEMENTS = "@NoOfStatements";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_LETTER_ID = "@LetterId";
        private const string PARM_STATEMENT_IMAGE = "@StatementImage";
        private const string PARM_IMAGE_Name = "@ImageName";
        private const string PARM_MESSAGE = "@Message";
        private const string PARM_FROM_NAME = "@FromName";
        private const string PARM_FROM_ADDRESS = "@FromAddress";
        private const string PARM_FROM_CITY = "@FromCity";
        private const string PARM_FROM_STATE = "@FromState";
        private const string PARM_FROM_ZIP = "@FromZip";
        private const string PARM_FROM_ZIP_EXT = "@FromZipExt";
        private const string PARM_REMITTO_NAME = "@RemitToName";
        private const string PARM_REMITTO_ADDRESS = "@RemitToAddress";
        private const string PARM_REMITTO_CITY = "@RemitToCity";
        private const string PARM_REMITTO_STATE = "@RemitToState";
        private const string PARM_REMITTO_ZIP = "@RemitToZip";
        private const string PARM_REMITTO_ZIP_EXT = "@RemitToZipExt";
        private const string PARM_OFC_HOURS_FROM = "@OfcHoursFrom";
        private const string PARM_OFC_HOURS_TO = "@OfcHoursTo";
        private const string PARM_OFC_PHONE_NO = "@PhoneNo";
        private const string PARM_OFC_PHONE_EXT = "@PhoneExt";
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
            dbManager.CreateParameters(36);


            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_STATEMENT_GROUP_ID, ds.PatientStatementGroup.PtStmtGrpIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_STATEMENT_GROUP_ID, ds.PatientStatementGroup.PtStmtGrpIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_NAME, ds.PatientStatementGroup.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.PatientStatementGroup.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CYCLE_DAYS, ds.PatientStatementGroup.CycleDaysColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(4, PARM_FIRST_MESSAGE_ID, ds.PatientStatementGroup.FirstMsgIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_SECOND_MESSAGE_ID, ds.PatientStatementGroup.SecondMsgIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_THIRD_MESSAGE_ID, ds.PatientStatementGroup.ThirdMsgIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_FOURTH_MESSAGE_ID, ds.PatientStatementGroup.FourthMsgIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_FIFTH_MESSAGE_ID, ds.PatientStatementGroup.FifthMsgIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_OUTSTANDING_DAYS, ds.PatientStatementGroup.OutStandingDaysColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(10, PARM_NO_OF_STATEMENTS, ds.PatientStatementGroup.NoOfStatementsColumn.ColumnName, DbType.Int32);

            dbManager.AddParameters(11, PARM_IS_ACTIVE, ds.PatientStatementGroup.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_CREATED_BY, ds.PatientStatementGroup.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_CREATED_ON, ds.PatientStatementGroup.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_MODIFIED_BY, ds.PatientStatementGroup.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_MODIFIED_ON, ds.PatientStatementGroup.ModifiedOnColumn.ColumnName, DbType.DateTime);
            
            dbManager.AddParameters(16, PARM_LETTER_ID, null, DbType.Int32);
            dbManager.AddParameters(17, PARM_STATEMENT_IMAGE, ds.PatientStatementGroup.StatementImageColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_IMAGE_Name, ds.PatientStatementGroup.ImageNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_MESSAGE, ds.PatientStatementGroup.MessageColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_FROM_NAME, ds.PatientStatementGroup.FromNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_FROM_ADDRESS, ds.PatientStatementGroup.FromAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_FROM_CITY, ds.PatientStatementGroup.FromCityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_FROM_STATE, ds.PatientStatementGroup.FromStateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_FROM_ZIP, ds.PatientStatementGroup.FromZipColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(25, PARM_FROM_ZIP_EXT, ds.PatientStatementGroup.FromZipExtColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(26, PARM_REMITTO_NAME, ds.PatientStatementGroup.RemitToNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_REMITTO_ADDRESS, ds.PatientStatementGroup.RemitToAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_REMITTO_CITY, ds.PatientStatementGroup.RemitToCityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_REMITTO_STATE, ds.PatientStatementGroup.RemitToStateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_REMITTO_ZIP, ds.PatientStatementGroup.RemitToZipColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(31, PARM_REMITTO_ZIP_EXT, ds.PatientStatementGroup.RemitToZipExtColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(32, PARM_OFC_HOURS_FROM, ds.PatientStatementGroup.OfcHoursFromColumn.ColumnName, DbType.String);
            dbManager.AddParameters(33, PARM_OFC_HOURS_TO, ds.PatientStatementGroup.OfcHoursToColumn.ColumnName, DbType.String);
            dbManager.AddParameters(34, PARM_OFC_PHONE_NO, ds.PatientStatementGroup.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(35, PARM_OFC_PHONE_EXT, ds.PatientStatementGroup.PhoneExtColumn.ColumnName, DbType.Int32);
            //  dbManager.AddParameters(8, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);


        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSPatientStatement InsertStatementGroup(DSPatientStatement ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PatientStatementGroup.GetChanges();
                ds = (DSPatientStatement)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_STATEMENT_GROUP_INSERT, ds, ds.PatientStatementGroup.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientStatementGroup.Rows[0][ds.PatientStatementGroup.PtStmtGrpIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;                
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatementGroup::InsertPatientStatementGroup", PROC_STATEMENT_GROUP_INSERT, ex);
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

        public DSPatientStatement LoadStatementGroup(long statementGroupId, string name, Int32 cycleDays, string isActive, int pageNumber, int RecordPerPage)
        {
            DSPatientStatement ds = new DSPatientStatement();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (statementGroupId <= 0)
                    dbManager.AddParameters(0, PARM_STATEMENT_GROUP_ID, null);
                else
                    dbManager.AddParameters(0, PARM_STATEMENT_GROUP_ID, statementGroupId);

                if (name == "")
                    dbManager.AddParameters(1, PARM_NAME, null);
                else
                    dbManager.AddParameters(1, PARM_NAME, name);

                if (cycleDays <= 0)
                    dbManager.AddParameters(2, PARM_CYCLE_DAYS, null);
                else
                    dbManager.AddParameters(2, PARM_CYCLE_DAYS, cycleDays);


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

                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.PatientStatementGroup.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                //    dbManager.AddParameters(4, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));

                ds = (DSPatientStatement)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_STATEMENT_GROUP_SELECT, ds, ds.PatientStatementGroup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatementGroup::LoadStatementGroup", PROC_STATEMENT_GROUP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatientStatement UpdateStatementGroup(DSPatientStatement ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, false);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PatientStatementGroup.GetChanges();
                ds = (DSPatientStatement)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_STATEMENT_GROUP_UPDATE, ds, ds.PatientStatementGroup.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientStatementGroup.Rows[0][ds.PatientStatementGroup.PtStmtGrpIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatementGroup::UpdateStatementGroup", PROC_STATEMENT_GROUP_UPDATE, ex);
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


        public string DeleteStatementGroup(long StatementGroupId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                //DSPatientStatement ds = LoadStatementGroup(StatementGroupId, "", 0, "", 1, 15);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.PatientStatementGroup;
                dbManager.AddParameters(0, PARM_STATEMENT_GROUP_ID, StatementGroupId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_STATEMENT_GROUP_DELETE).ToString();
                if (returnValue != null && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.PatientStatementGroup.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PatientStatementGroup.Rows[0][ds.PatientStatementGroup.PtStmtGrpIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatementGroup::DeleteStatementGroup", PROC_STATEMENT_GROUP_DELETE, ex);
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
        public DSPatientStatementLookup LookupStatementGroup()
        {
            DSPatientStatementLookup ds = new DSPatientStatementLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPatientStatementLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_STATEMENT_GROUP_LOOKUP, ds, ds.PatientStatementGroupLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientStatementGroup::LookupStatementGroup", PROC_STATEMENT_GROUP_LOOKUP, ex);
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


