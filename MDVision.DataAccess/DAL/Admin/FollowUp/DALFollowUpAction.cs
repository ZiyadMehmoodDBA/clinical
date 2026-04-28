using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;
using MDVision.Model.Billing.FollowUp;
using MDVision.Common.Utilities;
using System.Data.SqlClient;

namespace MDVision.DataAccess.DAL.Admin.FollowUp
{
    public class DALFollowUpAction
    {
        #region Variable
        
        #endregion
        #region "Stored Procedure Names"

        private const string PROC_FOLLOW_UP_ACTION_INSERT = "Billing.sp_FollowupActionInsert";
        private const string PROC_FOLLOW_UP_ACTION_UPDATE = "Billing.sp_FollowupActionUpdate";
        private const string PROC_FOLLOW_UP_ACTION_DELETE = "Billing.sp_FollowupActionDelete";
        private const string PROC_FOLLOW_UP_ACTION_SELECT = "Billing.sp_FollowupActionSelect";
        private const string PROC_FOLLOW_AUTO_ACTION_LOOKUP = "Billing.sp_FollowupAutoActionLookup";
        private const string PROC_FOLLOW_ACTION_LOOKUP = "Billing.sp_FollowUpActionLookup";
        private const string PROC_FOLLOW_UP_COMMENTS_INSERT = "Billing.sp_FollowupCommentsInsert";
        private const string PROC_FOLLOW_UP_COMMENTS_UPDATE = "Billing.sp_FollowupCommentsUpdate";
        private const string PROC_FOLLOW_UP_COMMENTS_DELETE = "Billing.sp_FollowupCommentsDelete";
        private const string PROC_FOLLOW_UP_COMMENTS_SELECT = "Billing.sp_FollowupCommentsSelect";
        #endregion
        #region "Parameters"

        private const string PARM_FOLLOW_UP_ACTION_ID = "@FollowupActionId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_AR_TYPE_ID = "@ARTypeId";
        private const string PARM_LETTER_ID = "@LetterId";
        private const string PARM_SUSPENDED_DAYS = "@SuspendedDays";
        private const string PARM_AUTO_ACTION_ID = "@AutoActionid";
        private const string PARM_LEDGER_ACCOUNT_ID = "@LedgerAccountid";
        private const string PARM_NEXT_ACTION_ID = "@NextActionId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PAGE_ = "@RowspPage";
        private const string PARM_FOLLOW_UP_COMMENT_ID = "@Follow_Up_Comment_Id";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_FROM_CLAIM = "@IsFromClaim";
        private const string PARM_IS_DELETED = "@IsDeleted";
        private const string PARAM_FOLLOW_UP_COMMENT_ID = "@FollowUpCommentId";

        #endregion
        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSFollowUp ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(14);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_FOLLOW_UP_ACTION_ID, ds.FollowupAction.FollowupActionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_FOLLOW_UP_ACTION_ID, ds.FollowupAction.FollowupActionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.FollowupAction.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.FollowupAction.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_AR_TYPE_ID, ds.FollowupAction.ARTypeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_LETTER_ID, ds.FollowupAction.LetterIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(5, PARM_SUSPENDED_DAYS, ds.FollowupAction.SuspendedDaysColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_AUTO_ACTION_ID, ds.FollowupAction.AutoActionidColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(7, PARM_LEDGER_ACCOUNT_ID, ds.FollowupAction.LedgerAccountidColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_NEXT_ACTION_ID, ds.FollowupAction.NextActionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(9, PARM_IS_ACTIVE, ds.FollowupAction.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(10, PARM_CREATED_BY, ds.FollowupAction.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_CREATED_ON, ds.FollowupAction.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(12, PARM_MODIFIED_BY, ds.FollowupAction.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_MODIFIED_ON, ds.FollowupAction.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion
        #region Constructors
        public DALFollowUpAction()
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
        #region "Lookups"

        public DSFollowUp LookupAutoAction()
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_AUTO_ACTION_LOOKUP, ds, ds.AutoActionLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpAction::LookupAutoAction", PROC_FOLLOW_AUTO_ACTION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFollowUp LookupFollowUpAction()
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_ACTION_LOOKUP, ds, ds.FollowUpActionLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpAction::LookupFollowUpAction", PROC_FOLLOW_ACTION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        #endregion
        #region "Insert, delete, update and get using dataset Functions"
        public DSFollowUp InsertFollowUpAction(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FollowupAction.GetChanges();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSFollowUp)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_ACTION_INSERT, ds, ds.FollowupAction.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FollowupAction.Rows[0][ds.FollowupAction.FollowupActionIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpAction::InsertFollowUpAction", PROC_FOLLOW_UP_ACTION_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp UpdateFollowUpAction(DSFollowUp ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FollowupAction.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSFollowUp)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_ACTION_UPDATE, ds, ds.FollowupAction.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FollowupAction.Rows[0][ds.FollowupAction.FollowupActionIdColumn].ToString(), "");
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpAction::UpdateFollowUpAction", PROC_FOLLOW_UP_ACTION_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFollowUp LoadFollowUpAction(Int64 FollowupActionId,string shortName , string Description,Int64 ARTypeId , string isActive , int pageNumber , int RecordPerPage)
        {
            DSFollowUp ds = new DSFollowUp();
            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if (shortName == "")
                    shortName = null;
                if (Description == "")
                    Description = null;
                if (isActive == "")
                    isActive = null;
                

                dbManager.Open();
                dbManager.CreateParameters(8);

                if (FollowupActionId == 0)
                    dbManager.AddParameters(0, PARM_FOLLOW_UP_ACTION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FOLLOW_UP_ACTION_ID, FollowupActionId);

                dbManager.AddParameters(1, PARM_SHORT_NAME, shortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);

                if (ARTypeId == 0)
                    dbManager.AddParameters(3, PARM_AR_TYPE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_AR_TYPE_ID, ARTypeId);

                dbManager.AddParameters(4, PARM_IS_ACTIVE, isActive);
                
                if (pageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, pageNumber);

                if (RecordPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWS_PAGE_, null);
                else
                    dbManager.AddParameters(6, PARM_ROWS_PAGE_, RecordPerPage);

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.FollowupAction.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSFollowUp)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FOLLOW_UP_ACTION_SELECT, ds, ds.FollowupAction.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpAction::LoadFollowUpAction", PROC_FOLLOW_UP_ACTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteFollowUpAction(Int64 FollowupActionId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSFollowUp ds = LoadFollowUpAction(Convert.ToInt64(FollowupActionId), null, null, 0,null,1,15);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FollowupAction;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_FOLLOW_UP_ACTION_ID, FollowupActionId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FOLLOW_UP_ACTION_DELETE).ToString();

                if (returnVal != "" && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal);
                }
                //else
                //{
                //    if (dtTemp != null && ds.FollowupAction.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FollowupAction.Rows[0][ds.FollowupAction.FollowupActionIdColumn].ToString(), null, "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DASFollowUpAction::DeleteFollowUpAction", PROC_FOLLOW_UP_ACTION_DELETE, ex);
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
        #region Follow Up Comments
        private void ParamsFollowUpComments(IDBManager dbManager, FollowUpARComments model, Boolean IsInsert)
        {


            if (IsInsert == true)
            {
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, PARM_COMMENTS, model.followUpComments);
                dbManager.AddParameters(1, PARM_VISIT_ID, model.VisitId);
                dbManager.AddParameters(2, PARM_IS_FROM_CLAIM, model.IsFromClaim);
                dbManager.AddParameters(3, PARM_IS_DELETED, model.IsDeleted);
                dbManager.AddParameters(4, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(5, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(6, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(7, PARM_MODIFIED_ON, DateTime.Now);
            }
            else
            {
                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, PARM_FOLLOW_UP_COMMENT_ID, model.Id);
                dbManager.AddParameters(1, PARM_COMMENTS, model.followUpComments);
                dbManager.AddParameters(2, PARM_VISIT_ID, model.VisitId);
                dbManager.AddParameters(3, PARM_IS_FROM_CLAIM, model.IsFromClaim);
                dbManager.AddParameters(4, PARM_IS_DELETED, model.IsDeleted);
                dbManager.AddParameters(5, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(6, PARM_MODIFIED_ON, DateTime.Now);
            }

        }
        public List<FollowUpARComments> GetFollowUpComments(Int64 FollowUpCommentId=0,Int64 VisitId=0)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try

            {
                dbManager.Open();
                List<FollowUpARComments> fcmodel = new List<FollowUpARComments>();
                FollowUpARComments model = null;
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(PARM_FOLLOW_UP_COMMENT_ID, FollowUpCommentId==0?(object)DBNull.Value:FollowUpCommentId));
                parameters.Add(new SqlParameter(PARM_VISIT_ID, VisitId==0?(object)DBNull.Value:VisitId));

                using (var reader = (SqlDataReader)dbManager.ExecuteReader(PROC_FOLLOW_UP_COMMENTS_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        model = new FollowUpARComments();
                        model.Id = MDVUtility.ToLong(reader["Id"]);
                        model.followUpComments = MDVUtility.ToStr(reader["followUpComments"]);
                        model.IsDeleted = MDVUtility.ToBool(reader["IsDeleted"]);
                        model.IsFromClaim = MDVUtility.ToBool(reader["IsFromClaim"]);
                        model.VisitId = MDVUtility.ToLong(reader["VisitId"]);
                        model.CreatedBy = MDVUtility.ToStr(reader["CreatedBy"]);
                        model.ModifiedBy = MDVUtility.ToStr(reader["ModifiedBy"]);
                        model.ModifiedOn= MDVUtility.ToStr(reader["ModifiedOn"]);
                        model.CreatedOn= MDVUtility.ToStr(reader["CreatedOn"]);
                        fcmodel.Add(model);
                    }
                }
                return fcmodel;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("GetFollowUpComments::Billing.GetFollowUpComments", PROC_FOLLOW_UP_COMMENTS_SELECT, ex);
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
        public string InsertNewFollowUpComments(FollowUpARComments model)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try

            {
                
                string retunVal = "";
                dbManager.Open();
                ParamsFollowUpComments(dbManager, model, true);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FOLLOW_UP_COMMENTS_INSERT));
                return retunVal;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFollowUpAction::Billing.InsertNewFollowUpComments", PROC_FOLLOW_UP_COMMENTS_INSERT, ex);
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
        public string UpdateNewFollowUpComments(FollowUpARComments model)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try

            {
               
                string retunVal = "";
                dbManager.Open();
                ParamsFollowUpComments(dbManager, model, false);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FOLLOW_UP_COMMENTS_UPDATE));
                return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("UpdateNewFollowUpComments::Billing.UpdateNewFollowUpComments", PROC_FOLLOW_UP_COMMENTS_UPDATE, ex);
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
        public string DeleteNewFollowUpComments(Int64 FollowUpCommentId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try

            {
              
                string retunVal = "";
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_FOLLOW_UP_COMMENT_ID, FollowUpCommentId);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FOLLOW_UP_COMMENTS_DELETE));
                return retunVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DeleteNewFollowUpComments::Billing.DeleteNewFollowUpComments", PROC_FOLLOW_UP_COMMENTS_DELETE, ex);
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
    }
}
