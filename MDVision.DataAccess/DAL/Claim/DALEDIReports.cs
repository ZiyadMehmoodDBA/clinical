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
using MDVision.Model.Billing.ERA;

namespace MDVision.DataAccess.DAL.Claim
{
    public class DALEDIReports
    {

        #region "Variable"

        #endregion

        #region "Constructors"
        public DALEDIReports()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALEDIReports(SharedVariable SharedVariable)
        {
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
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_BILLING_EDI_REPORT_DELETE = "Billing.sp_EDIReportsDelete";
        private const string PROC_BILLING_EDI_REPORT_UPDATE = "Billing.sp_EDIReportsUpdate";
        private const string PROC_BILLING_EDI_REPORT_SELECT = "Billing.sp_EDIReportsSelect";
        private const string PROC_BILLING_EDI_REPORT_INSERT = "Billing.sp_EDIReportsInsert";

        private const string PROC_BILLING_837_BATCH_REPORT_DELETE = "Billing.sp_837BatchReportDelete";
        private const string PROC_BILLING_837_BATCH_REPORT_UPDATE = "Billing.sp_837BatchReportUpdate";
        private const string PROC_BILLING_837_BATCH_REPORT_SELECT = "Billing.sp_837BatchReportSelect";
        private const string PROC_BILLING_837_BATCH_REPORT_INSERT = "Billing.sp_837BatchReportInsert";

        private const string PROC_BILLING_837_CLAIM_REPORT_DELETE = "Billing.sp_837ClaimReportDelete";
        private const string PROC_BILLING_837_CLAIM_REPORT_UPDATE = "Billing.sp_837ClaimReportUpdate";
        private const string PROC_BILLING_837_CLAIM_REPORT_SELECT = "Billing.sp_837ClaimReportSelect";
        private const string PROC_BILLING_837_CLAIM_REPORT_INSERT = "Billing.sp_837ClaimReportInsert";

        private const string PROC_GET_NOT_PARSED_EDI_REPORTS = "Billing.sp_GetEditReports";
        private const string PROC_EDI_DETAIL_INSERT = "Billing.sp_EdiDetailInsert";


        #endregion

        #region "Parameters"
        private const string PARM_EDI_REPORT_ID = "@EDIReportId";
        private const string PARM_CLEARING_HOUSE_ID = "@ClearingHouseId";
        private const string PARM_REPORT_DATE = "@ReportDate";

        private const string PARM_FILE_NAME = "@FileName";
        //private const string PARM_DOS = "@DOS";
        //private const string PARM_SUBMITTED_BATCH_NO = "@SubmittedBatchNo";
        private const string PARM_BILLING_PROV_NP1 = "@BillingProvNPI";

        //private const string PARM_CLAIM_NUMBER = "@ClaimNumber";
        private const string PARM_REVIEW_STATUS = "@ReviewStatus";
        private const string PARM_IS_REVIEWED = "@IsReviewed";
        private const string PARM_EDI_TEXT = "@EDIText";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_IS_PARSE = "@IsParse";

        private const string PARM_BATCH_REPORT_ID = "@BatchReportId";
        private const string PARM_BATCH_ID = "@BatchId";
        private const string PARM_CLAIM_REPORT_ID = "@ClaimReportId";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_IS_ERA_DELETED = "@IsERADeleted";
        private const string PARM_CLIENT_ID = "@ClientId";
        private const string PARM_837_BATCH_ID = "@837BatchId";
        private const string PARM_REPORT_TITLE = "@ReportTitle";
        private const string PARM_REPORT_TYPE = "@ReportType";
        private const string PARM_BATCH_NUMBER = "@BatchNumber";



        private const string PARM_CLAIM_NUMBER = "@ClaimNumber";
        private const string PARM_STATUS = "@Status";
        private const string PARM_DESCRIPTION = "@Description";

        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_XML = "@XML";
        private const string PARM_TOTAL_ACCEPTED = "@TotalAccepted";
        private const string PARM_TOTAL_REJECTED = "@TotalRejected";
        private const string PARM_EDI_DETAIL_XML = "@EdiDetailXML";
        private const string PARM_EDI_XML = "@EdiXML";
        
        #endregion

        #region "Support Functions"
        private void CreateEDIReportParameters(IDBManager dbManager, DSEDIReports ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(21);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_EDI_REPORT_ID, ds.EDIReports.EDIReportIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_EDI_REPORT_ID, ds.EDIReports.EDIReportIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_CLEARING_HOUSE_ID, ds.EDIReports.ClearingHouseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_REPORT_DATE, ds.EDIReports.ReportDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_FILE_NAME, ds.EDIReports.FileNameColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(4, PARM_DOS, ds.EDIReports.DOSColumn.ColumnName, DbType.DateTime);
            //dbManager.AddParameters(5, PARM_SUBMITTED_BATCH_NO, ds.EDIReports.SubmittedBatchNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_BILLING_PROV_NP1, ds.EDIReports.BillingProvNPIColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(7, PARM_CLAIM_NUMBER, ds.EDIReports.ClaimNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_REVIEWED, ds.EDIReports.IsReviewedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, PARM_EDI_TEXT, ds.EDIReports.EDITextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_COMMENTS, ds.EDIReports.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_IS_ACTIVE, ds.EDIReports.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(9, PARM_CREATED_BY, ds.EDIReports.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_CREATED_ON, ds.EDIReports.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_MODIFIED_BY, ds.EDIReports.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_MODIFIED_ON, ds.EDIReports.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_CLIENT_ID, ds.EDIReports.ClientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(14, PARM_ENTITY_ID, ds.EDIReports.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARM_REPORT_TITLE, ds.EDIReports.ReportTitleColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_REPORT_TYPE, ds.EDIReports.ReportTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_IS_PARSE, ds.EDIReports.IsParseColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_XML, ds.EDIReports.xmlColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_TOTAL_ACCEPTED, ds.EDIReports.TotalAcceptedColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(20, PARM_TOTAL_REJECTED, ds.EDIReports.TotalRejectedColumn.ColumnName, DbType.Int16);
            //dbManager.AddParameters(11, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }

        private void Create837BatchReportsParameters(IDBManager dbManager, DSEDIReports ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(5);


            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_BATCH_REPORT_ID, ds._837BatchReport.BatchReportIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_BATCH_REPORT_ID, ds._837BatchReport.BatchReportIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_BATCH_NUMBER, ds._837BatchReport.BatchNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_STATUS, ds._837BatchReport.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds._837BatchReport.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_EDI_REPORT_ID, ds._837BatchReport.EDIReportIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(3, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }

        private void Create837ClaimReportsParameters(IDBManager dbManager, DSEDIReports ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(5);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CLAIM_REPORT_ID, ds._837ClaimReport.ClaimReportIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CLAIM_REPORT_ID, ds._837ClaimReport.ClaimReportIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_CLAIM_NUMBER, ds._837ClaimReport.ClaimNumberColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_STATUS, ds._837ClaimReport.StatusColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds._837ClaimReport.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_EDI_REPORT_ID, ds._837ClaimReport.EDIReportIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(3, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }

        #endregion

        #region "Billing.EDIReports"

        public DSEDIReports LoadEDIReports(long EDIReportId, long clearingHouseId, string FileName, string ReviewStatus, string ReportType, string EDIText, string IsERADeleted, DateTime? ReportDate = null, string IsParse = null,DateTime? CreatedOn = null, int PageNumber = 1, int RowspPage = 1000)
        {
            DSEDIReports ds = new DSEDIReports();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(14);

                if (EDIReportId == 0)
                    dbManager.AddParameters(0, PARM_EDI_REPORT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EDI_REPORT_ID, EDIReportId);

                if (clearingHouseId == 0)
                    dbManager.AddParameters(1, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CLEARING_HOUSE_ID, clearingHouseId);

                if (FileName == "")
                    FileName = null;

                if (ReviewStatus == "")
                    ReviewStatus = null;

                if (ReportType == "")
                    ReportType = null;

                if (EDIText == "")
                    EDIText = null;

                dbManager.AddParameters(2, PARM_FILE_NAME, FileName);

                if (ReportDate == null)
                    dbManager.AddParameters(3, PARM_REPORT_DATE, null);
                else
                    dbManager.AddParameters(3, PARM_REPORT_DATE, ReportDate);

                dbManager.AddParameters(4, PARM_REVIEW_STATUS, ReviewStatus);
                dbManager.AddParameters(5, PARM_REPORT_TYPE, ReportType);
                dbManager.AddParameters(6, PARM_EDI_TEXT, EDIText);
                dbManager.AddParameters(7, PARM_IS_PARSE, IsParse);

                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(9, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.EDIReports.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(11, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                if (string.IsNullOrEmpty(IsERADeleted))
                    dbManager.AddParameters(12, PARM_IS_ERA_DELETED, null);
                else
                    dbManager.AddParameters(12, PARM_IS_ERA_DELETED, IsERADeleted);

                if (CreatedOn == null)
                    dbManager.AddParameters(13, PARM_CREATED_ON, null);
                else
                    dbManager.AddParameters(13, PARM_CREATED_ON, CreatedOn);
                ds = (DSEDIReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_SELECT, ds, ds.EDIReports.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::LoadEDIReports", PROC_BILLING_EDI_REPORT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="EDIReportId"></param>
        /// <param name="clearingHouseId"></param>
        /// <param name="FileName"></param>
        /// <param name="ReviewStatus"></param>
        /// <param name="ReportType"></param>
        /// <param name="EDIText"></param>
        /// <param name="IsERADeleted"></param>
        /// <param name="ReportDate"></param>
        /// <param name="IsParse"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowspPage"></param>
        /// <returns></returns>
        public DSEDIReports LoadEDIReports(SharedVariable SharedVariable, long EDIReportId, long clearingHouseId, string FileName, string ReviewStatus, string ReportType, string EDIText, string IsERADeleted, DateTime? ReportDate = null, string IsParse = null, DateTime? CreatedOn = null, int PageNumber = 1, int RowspPage = 1000)
        {
            DSEDIReports ds = new DSEDIReports();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(14);

                if (EDIReportId == 0)
                    dbManager.AddParameters(0, PARM_EDI_REPORT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_EDI_REPORT_ID, EDIReportId);

                if (clearingHouseId == 0)
                    dbManager.AddParameters(1, PARM_CLEARING_HOUSE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CLEARING_HOUSE_ID, clearingHouseId);

                if (FileName == "")
                    FileName = null;

                if (ReviewStatus == "")
                    ReviewStatus = null;

                if (ReportType == "")
                    ReportType = null;

                if (EDIText == "")
                    EDIText = null;

                dbManager.AddParameters(2, PARM_FILE_NAME, FileName);

                if (ReportDate == null)
                    dbManager.AddParameters(3, PARM_REPORT_DATE, null);
                else
                    dbManager.AddParameters(3, PARM_REPORT_DATE, ReportDate);

                dbManager.AddParameters(4, PARM_REVIEW_STATUS, ReviewStatus);
                dbManager.AddParameters(5, PARM_REPORT_TYPE, ReportType);
                dbManager.AddParameters(6, PARM_EDI_TEXT, EDIText);
                dbManager.AddParameters(7, PARM_IS_PARSE, IsParse);

                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(9, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWS_PERPAGE, RowspPage);

                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.EDIReports.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(11, PARM_ENTITY_ID, SharedVariable.EntityId);

                if (string.IsNullOrEmpty(IsERADeleted))
                    dbManager.AddParameters(12, PARM_IS_ERA_DELETED, null);
                else
                    dbManager.AddParameters(12, PARM_IS_ERA_DELETED, IsERADeleted);
                if (CreatedOn == null)
                    dbManager.AddParameters(13, PARM_CREATED_ON, null);
                else
                    dbManager.AddParameters(13, PARM_CREATED_ON, CreatedOn);
                ds = (DSEDIReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_SELECT, ds, ds.EDIReports.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALEDIReports::LoadEDIReports", PROC_BILLING_EDI_REPORT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSEDIReports InsertEDIReports(DSEDIReports ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateEDIReportParameters(dbManager, ds, true);
                ds = (DSEDIReports)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_INSERT, ds, ds.EDIReports.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::InsertEDIReports", PROC_BILLING_EDI_REPORT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }


        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSEDIReports InsertEDIReports(SharedVariable SharedVariable,DSEDIReports ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                this.CreateEDIReportParameters(dbManager, ds, true);
                ds = (DSEDIReports)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_INSERT, ds, ds.EDIReports.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALEDIReports::InsertEDIReports", PROC_BILLING_EDI_REPORT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        public DSEDIReports InsertEDIReports(DSEDIReports ds, IDBManager dbManager)
        {

            try
            {
                this.CreateEDIReportParameters(dbManager, ds, true);
                ds = (DSEDIReports)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_INSERT, ds, ds.EDIReports.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::InsertEDIReports", PROC_BILLING_EDI_REPORT_INSERT, ex);
                throw ex;
            }


        }

        public DSEDIReports UpdateEDIReports(DSEDIReports ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                this.CreateEDIReportParameters(dbManager, ds, false);
                ds = (DSEDIReports)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_UPDATE, ds, ds.EDIReports.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::UpdateEDIReports", PROC_BILLING_EDI_REPORT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSEDIReports UpdateEDIReports(SharedVariable SharedVariable, DSEDIReports ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);

            try
            {
                dbManager.Open();
                this.CreateEDIReportParameters(dbManager, ds, false);
                ds = (DSEDIReports)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_UPDATE, ds, ds.EDIReports.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALEDIReports::UpdateEDIReports", PROC_BILLING_EDI_REPORT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        public string DeleteEDIReports(string EDIReportId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_EDI_REPORT_ID, EDIReportId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLING_EDI_REPORT_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::DaleteEDIReports", PROC_BILLING_EDI_REPORT_DELETE, ex);
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

        #region "Billing.837BatchReport"

        public DSEDIReports Load837BatchReports(string BatchNumber, long EDIReportId)
        {
            DSEDIReports ds = new DSEDIReports();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (BatchNumber == "")
                    dbManager.AddParameters(0, PARM_BATCH_NUMBER, null);
                else
                    dbManager.AddParameters(0, PARM_BATCH_NUMBER, BatchNumber);


                if (EDIReportId == 0)
                    dbManager.AddParameters(1, PARM_EDI_REPORT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_EDI_REPORT_ID, EDIReportId);


                ds = (DSEDIReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_REPORT_SELECT, ds, ds._837BatchReport.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::Load837BatchReports", PROC_BILLING_837_BATCH_REPORT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSEDIReports Insert837BatchReports(DSEDIReports ds, IDBManager dbManager)
        {
            try
            {
                this.Create837BatchReportsParameters(dbManager, ds, true);
                ds = (DSEDIReports)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_REPORT_INSERT, ds, ds._837BatchReport.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::Insert837BatchReports", PROC_BILLING_837_BATCH_REPORT_INSERT, ex);
                throw ex;
            }

        }
        public DSEDIReports Insert837BatchReports(DSEDIReports ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.Create837BatchReportsParameters(dbManager, ds, true);
                ds = (DSEDIReports)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_REPORT_INSERT, ds, ds._837BatchReport.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::Insert837BatchReports", PROC_BILLING_837_BATCH_REPORT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        public DSEDIReports Update837BatchReports(DSEDIReports ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                this.Create837BatchReportsParameters(dbManager, ds, false);
                ds = (DSEDIReports)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_REPORT_UPDATE, ds, ds._837BatchReport.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::Update837BatchReports", PROC_BILLING_837_BATCH_REPORT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        public string Delete837BatchReports(long BatchReportId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_BATCH_REPORT_ID, BatchReportId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_BILLING_837_BATCH_REPORT_DELETE).ToString();


                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::Dalete837BatchReports", PROC_BILLING_837_BATCH_REPORT_DELETE, ex);
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

        #region "Billing.837ClaimReport"

        public DSEDIReports Load837ClaimReports(long ClaimReportId)
        {
            DSEDIReports ds = new DSEDIReports();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_CLAIM_REPORT_ID, ClaimReportId);

                ds = (DSEDIReports)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLING_837_CLAIM_REPORT_SELECT, ds, ds._837ClaimReport.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::Load837ClaimReports", PROC_BILLING_837_CLAIM_REPORT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSEDIReports Insert837ClaimReports(DSEDIReports ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.Create837ClaimReportsParameters(dbManager, ds, true);
                ds = (DSEDIReports)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_837_CLAIM_REPORT_INSERT, ds, ds._837ClaimReport.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::Insert837ClaimReports", PROC_BILLING_837_CLAIM_REPORT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSEDIReports Insert837ClaimReports(SharedVariable SharedVariable,DSEDIReports ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                dbManager.Open();
                this.Create837ClaimReportsParameters(dbManager, ds, true);
                ds = (DSEDIReports)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLING_837_CLAIM_REPORT_INSERT, ds, ds._837ClaimReport.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALEDIReports::Insert837ClaimReports", PROC_BILLING_837_CLAIM_REPORT_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        public DSEDIReports Update837ClaimReports(DSEDIReports ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                this.Create837ClaimReportsParameters(dbManager, ds, false);
                ds = (DSEDIReports)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLING_837_CLAIM_REPORT_UPDATE, ds, ds._837ClaimReport.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::Update837ClaimReports", PROC_BILLING_837_CLAIM_REPORT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }
        public string Delete837ClaimReports(long ClaimReportId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_CLAIM_REPORT_ID, ClaimReportId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_BILLING_837_CLAIM_REPORT_DELETE).ToString();


                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::Dalete837ClaimReports", PROC_BILLING_837_CLAIM_REPORT_DELETE, ex);
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


        
        public List<EDIReport> LoadPreEdiReports()
        {
            List<EDIReport> EDIReportList = new List<EDIReport>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                EDIReportList = dbManager.ExecuteReaders<EDIReport>(PROC_GET_NOT_PARSED_EDI_REPORTS);
                return EDIReportList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEDIReports::LoadPreEdiReports", PROC_GET_NOT_PARSED_EDI_REPORTS, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public string SaveEdiReportDetail(string stcXML, string ediReportXML)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.AddParameters(PARM_EDI_DETAIL_XML, stcXML);
                dbManager.AddParameters(PARM_EDI_XML, ediReportXML);
                var result = dbManager.ExecuteScalar(PROC_EDI_DETAIL_INSERT).ToString();
                if (result != "")
                {
                    throw new Exception(result);
                }
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALEDIReports::SaveEdiReportDetail", PROC_EDI_DETAIL_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

    }
}
