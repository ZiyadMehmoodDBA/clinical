//using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
//using MDVision.Datasets;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Linq;
//using System.Text;

//namespace MDVision.DataAccess.DAL.Claim
//{
//    public class DAL835
//    {

//        #region "Variable"
//        
//        #endregion

//        #region "Constructors"
//        public DAL835()
//        {
//            InitializeComponent();
//            ClientConfiguration.SetClientObject();
//           
//        }
//        private IContainer components;
//        //NOTE: The following procedure is required by the Web Services Designer
//        //It can be modified using the Web Services Designer.  
//        //Do not modify it using the code editor.
//        [System.Diagnostics.DebuggerStepThrough()]
//        private void InitializeComponent()
//        {
//            components = new System.ComponentModel.Container();
//        }
//        #endregion

//        #region "Stored Procedure Names"

//        private const string PROC_BILLING_EDI_REPORT_DELETE = "Billing.sp_EDIReportsDelete";
//        private const string PROC_BILLING_EDI_REPORT_UPDATE = "Billing.sp_EDIReportsUpdate";
//        private const string PROC_BILLING_EDI_REPORT_SELECT = "Billing.sp_EDIReportsSelect";
//        private const string PROC_BILLING_EDI_REPORT_INSERT = "Billing.sp_EDIReportsInsert";

//        private const string PROC_BILLING_837_BATCH_REPORT_DELETE = "Billing.sp_837BatchReportDelete";
//        private const string PROC_BILLING_837_BATCH_REPORT_UPDATE = "Billing.sp_837BatchReportUpdate";
//        private const string PROC_BILLING_837_BATCH_REPORT_SELECT = "Billing.sp_837BatchReportSelect";
//        private const string PROC_BILLING_837_BATCH_REPORT_INSERT = "Billing.sp_837BatchReportInsert";

//        private const string PROC_BILLING_837_CLAIM_REPORT_DELETE = "Billing.sp_837ClaimReportDelete";
//        private const string PROC_BILLING_837_CLAIM_REPORT_UPDATE = "Billing.sp_837ClaimReportUpdate";
//        private const string PROC_BILLING_837_CLAIM_REPORT_SELECT = "Billing.sp_837ClaimReportSelect";
//        private const string PROC_BILLING_837_CLAIM_REPORT_INSERT = "Billing.sp_837ClaimReportInsert";

//        #endregion

//        #region "Parameters"
//        private const string PARM_EDI_REPORT_ID = "@EDIReportId";
//        private const string PARM_CLEARING_HOUSE_ID = "@ClearingHouseId";
//        private const string PARM_REPORT_DATE = " @ReportDate";

//        private const string PARM_FILE_NAME = "@FileName";
//        private const string PARM_DOS = "@DOS";
//        private const string PARM_SUBMITTED_BATCH_NO = "@SubmittedBatchNo";
//        private const string PARM_BILLING_PROV_NP1 = "@BillingProvNPI";

//        private const string PARM_CLAIM_NUMBER = "@ClaimNumber";
//        private const string PARM_REVIEW_STATUS = "@ReviewStatus";
//        private const string PARM_EDI_TEXT = "@EDIText";
//        private const string PARM_COMMENTS = "@Comments";
//        private const string PARM_IS_ACTIVE = "@IsActive";
//        private const string PARM_CREATED_BY = "@CreatedBy";
//        private const string PARM_CREATED_ON = "@CreatedOn";
//        private const string PARM_MODIFIED_BY = "@ModifiedBy";
//        private const string PARM_MODIFIED_ON = "@ModifiedOn";
//        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

//        private const string PARM_BATCH_REPORT_ID = "@BatchReportId";
//        private const string PARM_BATCH_ID = "@BatchId";
//        private const string PARM_CLAIM_REPORT_ID = "@ClaimReportId";
//        private const string PARM_VISIT_ID = "@VisitId";
//        private const string PARM_ENTITY_ID = "@EntityId";
//        private const string PARM_CLIENT_ID = "@ClientId";
//        private const string PARM_837_BATCH_ID = "@837BatchId";
//        private const string PARM_REPORT_TITLE = "@ReportTitle";
//        private const string PARM_REPORT_TYPE = "@ReportType";

//        #endregion

//        #region "Support Functions"
//        private void CreateEDIReportParameters(IDBManager dbManager, DSEDIReports ds, Boolean IsInsert)
//        {
//            dbManager.CreateParameters(20);

//            if (IsInsert == true)
//                dbManager.AddParameters(0, PARM_EDI_REPORT_ID, ds.EDIReports.EDIReportIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
//            else
//                dbManager.AddParameters(0, PARM_EDI_REPORT_ID, ds.EDIReports.EDIReportIdColumn.ColumnName, DbType.Int64);

//            dbManager.AddParameters(1, PARM_CLEARING_HOUSE_ID, ds.EDIReports.ClearingHouseIdColumn.ColumnName, DbType.Int64);
//            dbManager.AddParameters(2, PARM_REPORT_DATE, ds.EDIReports.ReportDateColumn.ColumnName, DbType.DateTime);
//            dbManager.AddParameters(3, PARM_FILE_NAME, ds.EDIReports.FileNameColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(4, PARM_DOS, ds.EDIReports.DOSColumn.ColumnName, DbType.DateTime);
//            dbManager.AddParameters(5, PARM_SUBMITTED_BATCH_NO, ds.EDIReports.SubmittedBatchNoColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(6, PARM_BILLING_PROV_NP1, ds.EDIReports.BillingProvNPIColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(7, PARM_CLAIM_NUMBER, ds.EDIReports.ClaimNumberColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(8, PARM_REVIEW_STATUS, ds.EDIReports.ReviewStatusColumn.ColumnName, DbType.Boolean);
//            dbManager.AddParameters(9, PARM_EDI_TEXT, ds.EDIReports.EDITextColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(10, PARM_COMMENTS, ds.EDIReports.CommentsColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(11, PARM_IS_ACTIVE, ds.EDIReports.IsActiveColumn.ColumnName, DbType.Boolean);
//            dbManager.AddParameters(12, PARM_CREATED_BY, ds.EDIReports.CreatedByColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(13, PARM_CREATED_ON, ds.EDIReports.CreatedOnColumn.ColumnName, DbType.DateTime);
//            dbManager.AddParameters(14, PARM_MODIFIED_BY, ds.EDIReports.ModifiedByColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(15, PARM_MODIFIED_ON, ds.EDIReports.ModifiedOnColumn.ColumnName, DbType.DateTime);
//            dbManager.AddParameters(16, PARM_CLIENT_ID, ds.EDIReports.ClientIdColumn.ColumnName, DbType.Int64);
//            dbManager.AddParameters(17, PARM_ENTITY_ID, ds.EDIReports.EntityIdColumn.ColumnName, DbType.Int64);
//            dbManager.AddParameters(18, PARM_REPORT_TITLE, ds.EDIReports.EntityIdColumn.ColumnName, DbType.String);
//            dbManager.AddParameters(19, PARM_REPORT_TYPE, ds.EDIReports.EntityIdColumn.ColumnName, DbType.String);
//            //dbManager.AddParameters(11, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
//        }

//        private void Create837BatchReportsParameters(IDBManager dbManager, DSEDIReports ds, Boolean IsInsert)
//        {
//            dbManager.CreateParameters(3);

//            if (IsInsert == true)
//                dbManager.AddParameters(0, PARM_BATCH_REPORT_ID, ds._837BatchReport.BatchReportIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
//            else
//                dbManager.AddParameters(0, PARM_BATCH_REPORT_ID, ds._837BatchReport.BatchReportIdColumn.ColumnName, DbType.Int64);

//            dbManager.AddParameters(1, PARM_BATCH_ID, ds._837BatchReport.BatchIdColumn.ColumnName, DbType.Int32);
//            dbManager.AddParameters(2, PARM_EDI_REPORT_ID, ds._837BatchReport.EDIReportIdColumn.ColumnName, DbType.Int64);
//            //dbManager.AddParameters(3, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
//        }

//        private void Create837ClaimReportsParameters(IDBManager dbManager, DSEDIReports ds, Boolean IsInsert)
//        {
//            dbManager.CreateParameters(3);

//            if (IsInsert == true)
//                dbManager.AddParameters(0, PARM_CLAIM_REPORT_ID, ds._837ClaimReport.ClaimReportIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
//            else
//                dbManager.AddParameters(0, PARM_CLAIM_REPORT_ID, ds._837ClaimReport.ClaimReportIdColumn.ColumnName, DbType.Int64);

//            dbManager.AddParameters(1, PARM_VISIT_ID, ds._837ClaimReport.VisitIdColumn.ColumnName, DbType.Int64);
//            dbManager.AddParameters(2, PARM_EDI_REPORT_ID, ds._837ClaimReport.EDIReportIdColumn.ColumnName, DbType.Int64);
//            //dbManager.AddParameters(3, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
//        }

//        #endregion

//        #region "Billing.EDIReports"

//        public DSEDIReports LoadEDIReports(long EDIReportId, long _837BatchId)
//        {
//            DSEDIReports ds = new DSEDIReports();
//            IDBManager dbManager = ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                dbManager.CreateParameters(2);
//                if (EDIReportId == 0)
//                    dbManager.AddParameters(0, PARM_EDI_REPORT_ID, null);
//                else
//                    dbManager.AddParameters(0, PARM_EDI_REPORT_ID, EDIReportId);

//                if (_837BatchId == 0)
//                    dbManager.AddParameters(1, PARM_837_BATCH_ID, null);
//                else
//                    dbManager.AddParameters(1, PARM_837_BATCH_ID, _837BatchId);


//                ds = (DSEDIReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_SELECT, ds, ds.EDIReports.TableName);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::LoadEDIReports", PROC_BILLING_EDI_REPORT_SELECT, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }
//        }
//        public DSEDIReports InsertEDIReports(DSEDIReports ds)
//        {
//            IDBManager dbManager = ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                this.CreateEDIReportParameters(dbManager, ds, true);
//                ds = (DSEDIReports)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_INSERT, ds, ds.EDIReports.TableName);
//                ds.AcceptChanges();
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::InsertEDIReports", PROC_BILLING_EDI_REPORT_INSERT, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }

//        }

//        public DSEDIReports InsertEDIReports(DSEDIReports ds, IDBManager dbManager)
//        {
           
//            try
//            {
//                this.CreateEDIReportParameters(dbManager, ds, true);
//                ds = (DSEDIReports)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_INSERT, ds, ds.EDIReports.TableName);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::InsertEDIReports", PROC_BILLING_EDI_REPORT_INSERT, ex);
//                throw ex;
//            }
           

//        }

//        public DSEDIReports UpdateEDIReports(DSEDIReports ds)
//        {
//            IDBManager dbManager = ClientConfiguration.GetDBManager();

//            try
//            {
//                dbManager.Open();
//                this.CreateEDIReportParameters(dbManager, ds, false);
//                ds = (DSEDIReports)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_UPDATE, ds, ds.EDIReports.TableName);
//                ds.AcceptChanges();
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::UpdateEDIReports", PROC_BILLING_EDI_REPORT_UPDATE, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }

//        }
//        public string DeleteEDIReports(string EDIReportId)
//        {
//            string returnValue = "";
//            IDBManager dbManager = ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                dbManager.CreateParameters(2);
//                dbManager.AddParameters(0, PARM_EDI_REPORT_ID, EDIReportId);
//                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
//                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_DELETE).ToString();
//                if (returnValue != "")
//                    throw new Exception(returnValue);

//                return "";
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::DaleteEDIReports", PROC_BILLING_EDI_REPORT_DELETE, ex);
//                string[] str = ex.Message.Split('|');
//                if (str.Length > 1)
//                    return str[1].ToString();
//                else
//                    return ex.Message;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }
//        }

//        #endregion

//        #region "Billing.837BatchReport"

//        public DSEDIReports Load837BatchReports(long BatchId)
//        {
//            DSEDIReports ds = new DSEDIReports();
//            IDBManager dbManager = ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                dbManager.CreateParameters(1);
//                dbManager.AddParameters(0, PARM_BATCH_ID, BatchId);

//                ds = (DSEDIReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_REPORT_SELECT, ds, ds._837BatchReport.TableName);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::Load837BatchReports", PROC_BILLING_837_BATCH_REPORT_SELECT, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }
//        }

//        public DSEDIReports Insert837BatchReports(DSEDIReports ds, IDBManager dbManager)
//        {
//            try
//            {
//                this.Create837BatchReportsParameters(dbManager, ds, true);
//                ds = (DSEDIReports)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_REPORT_INSERT, ds, ds._837BatchReport.TableName);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::Insert837BatchReports", PROC_BILLING_837_BATCH_REPORT_INSERT, ex);
//                throw ex;
//            }
           
//        }
//        public DSEDIReports Insert837BatchReports(DSEDIReports ds)
//        {
//            IDBManager dbManager = ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                this.Create837BatchReportsParameters(dbManager, ds, true);
//                ds = (DSEDIReports)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_REPORT_INSERT, ds, ds._837BatchReport.TableName);
//                ds.AcceptChanges();
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::Insert837BatchReports", PROC_BILLING_837_BATCH_REPORT_INSERT, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }

//        }
//        public DSEDIReports Update837BatchReports(DSEDIReports ds)
//        {
//            IDBManager dbManager = ClientConfiguration.GetDBManager();

//            try
//            {
//                dbManager.Open();
//                this.Create837BatchReportsParameters(dbManager, ds, false);
//                ds = (DSEDIReports)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_REPORT_UPDATE, ds, ds._837BatchReport.TableName);
//                ds.AcceptChanges();
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::Update837BatchReports", PROC_BILLING_837_BATCH_REPORT_UPDATE, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }

//        }
//        public string Delete837BatchReports(long BatchReportId)
//        {
//            string returnValue = "";
//            IDBManager dbManager = ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                dbManager.CreateParameters(1);
//                dbManager.AddParameters(0, PARM_BATCH_REPORT_ID, BatchReportId);
//                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_REPORT_DELETE).ToString();


//                return returnValue;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::Dalete837BatchReports", PROC_BILLING_837_BATCH_REPORT_DELETE, ex);
//                string[] str = ex.Message.Split('|');
//                if (str.Length > 1)
//                    return str[1].ToString();
//                else
//                    return ex.Message;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }
//        }

//        #endregion

//        #region "Billing.837ClaimReport"

//        public DSEDIReports Load837ClaimReports(long ClaimReportId)
//        {
//            DSEDIReports ds = new DSEDIReports();
//            IDBManager dbManager = ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                dbManager.CreateParameters(1);
//                dbManager.AddParameters(0, PARM_CLAIM_REPORT_ID, ClaimReportId);

//                ds = (DSEDIReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_CLAIM_REPORT_SELECT, ds, ds._837ClaimReport.TableName);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::Load837ClaimReports", PROC_BILLING_837_CLAIM_REPORT_SELECT, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }
//        }
//        public DSEDIReports Insert837ClaimReports(DSEDIReports ds)
//        {
//            IDBManager dbManager = ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                this.Create837ClaimReportsParameters(dbManager, ds, true);
//                ds = (DSEDIReports)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_837_CLAIM_REPORT_INSERT, ds, ds._837ClaimReport.TableName);
//                ds.AcceptChanges();
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::Insert837ClaimReports", PROC_BILLING_837_CLAIM_REPORT_INSERT, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }

//        }
//        public DSEDIReports Update837ClaimReports(DSEDIReports ds)
//        {
//            IDBManager dbManager = ClientConfiguration.GetDBManager();

//            try
//            {
//                dbManager.Open();
//                this.Create837ClaimReportsParameters(dbManager, ds, false);
//                ds = (DSEDIReports)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLING_837_CLAIM_REPORT_UPDATE, ds, ds._837ClaimReport.TableName);
//                ds.AcceptChanges();
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::Update837ClaimReports", PROC_BILLING_837_CLAIM_REPORT_UPDATE, ex);
//                throw ex;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }

//        }
//        public string Delete837ClaimReports(long ClaimReportId)
//        {
//            string returnValue = "";
//            IDBManager dbManager = ClientConfiguration.GetDBManager();
//            try
//            {
//                dbManager.Open();
//                dbManager.CreateParameters(1);
//                dbManager.AddParameters(0, PARM_CLAIM_REPORT_ID, ClaimReportId);
//                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_BILLING_837_CLAIM_REPORT_DELETE).ToString();


//                return returnValue;
//            }
//            catch (Exception ex)
//            {
//                MDVLogger.LogErrorMessage("DAL835::Dalete837ClaimReports", PROC_BILLING_837_CLAIM_REPORT_DELETE, ex);
//                string[] str = ex.Message.Split('|');
//                if (str.Length > 1)
//                    return str[1].ToString();
//                else
//                    return ex.Message;
//            }
//            finally
//            {
//                dbManager.Dispose();
//            }
//        }

//        #endregion

//    }
//}
