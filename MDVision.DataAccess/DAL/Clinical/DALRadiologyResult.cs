/* Author:  Muhammad Arshad
 * Created Date: 22/04/2016
 * OverView: Created for RadiologyResult in Clinical Module
 */

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
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using System.Data.SqlClient;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALRadiologyResult
    {

        #region Variable

        #endregion

        #region Stored Procedure Names

        //Start//18-04-2016//Abid Ali// Result Store Procedures Names
        private const string PROC_Radiology_ORDER_RESULT_DELETE = "Clinical.sp_RadiologyOrderResultDelete";
        private const string PROC_Radiology_ORDER_RESULT_INSERT = "Clinical.sp_RadiologyOrderResultInsert";
        private const string PROC_Radiology_ORDER_RESULT_UPDATE = "Clinical.sp_RadiologyOrderResultUpdate";
        private const string PROC_Radiology_ORDER_RESULT_SELECT = "Clinical.sp_RadiologyOrderResultSelect";
        private const string PROC_Radiology_ORDER_RESULT_SELECT_AGAINST_NOTE = "Clinical.sp_RadiologyOrderResultSelectAgainstNote";

        //End//18-04-2016//Abid Ali// Result Store Procedures Names

        //Start//18-04-2016//Abid Ali//Result Detail Store Procedures Names
        private const string PROC_Radiology_ORDER_RESULT_DETAIL_DELETE = "Clinical.sp_RadiologyOrderResultDetailDelete";
        private const string PROC_Radiology_ORDER_RESULT_DETAIL_INSERT = "Clinical.sp_RadiologyOrderResultDetailInsert";
        private const string PROC_Radiology_ORDER_RESULT_DETAIL_UPDATE = "Clinical.sp_RadiologyOrderResultDetailUpdate";
        private const string PROC_Radiology_ORDER_RESULT_DETAIL_SELECT = "Clinical.sp_RadiologyOrderResultDetailSelect";

        //End//18-04-2016//Abid Ali//Result Detail Store Procedures Names

        //Start 18-04-2016 Muhammad Arshad Lookup LOINC
        private const string PROC_ORDER_RESULT_LOINC_LOOKUP = "Clinical.sp_OrderResultLOINCLookup";
        //End 18-04-2016 Muhammad Arshad Lookup LOINC


        private const string PROC_RADIOLOGYORDER_RESULT_SELECT_FOR_SOAPTEXT = "Clinical.sp_GetSoapTextForRadiologyOrderResult"; //"Clinical.sp_RadiologyOrderResultSelectForSoapText";
        private const string PROC_DETACH_RADIOLOGY_ORDER_FROM_NOTES = "Clinical.sp_DetachRadiologyOrderResultFromNotes";
        private const string PROC_ATTACH_RADIOLOGYORDER_RESULT_WITH_NOTES = "Clinical.sp_AttachRadiologyOrderResultWithNotes";

        private const string RADIOLOGY_ORDER_RESULT_TEST_DELETE = "Clinical.Sp_RadiologyOrderResultTestDelete";

        private const string PROC_NOTES_RADORDERRESULT_SELECT = "[Clinical].[sp_NotesRadOrderResultSelect]";

        #endregion

        #region Parameters
        //Start//18-04-2016//Abid Ali// Paramenters Names

        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_PATIENT_ID = "@PatientId";

        private const string PARM_Radiology_ORDER_RESULT_ID = "@RadiologyOrderResultId";
        private const string PARM_Radiology_ORDER_ID = "@RadiologyOrderId";
        private const string PARM_TEST = "@Test";
        private const string PARM_ORDER_NO = "@OrderNo";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_ORDER_DATE_FROM = "@OrderDateFrom";
        private const string PARM_ORDER_DATE_TO = "@OrderDateTo";
        private const string PARM_STATUS = "@Status";
        private const string PARM_Radiology_ID = "@RadiologyId";

        private const string PARM_REMARKS = "@Remarks";

        private const string PARM_OBSERVATION_DATE = "@ObservationDate";

        private const string PARM_Radiology_ORDER_RESULT_DETAIL_ID = "@RadiologyOrderResultDetailId";

        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_DESCRIPTION = "@CPTCodeDescription";

        private const string PARM_LOINC = "@LOINC";
        private const string PARM_LOINC_DECSRIPTION = "@LOINCDescription";

        private const string PARM_RESULT = "@Result";
        private const string PARM_UoM = "@UoM";
        private const string PARM_FLAG = "@Flag";
        private const string PARM_RANGE = "@Range";

        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";

        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";

        private const string PARM_MODIFIED_BY = "@ModifiedBy";

        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_IS_SENT_TO_POTRTALPARM_MODIFIED_BY = "ModifiedBy";
        //   private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_SOAP_TEXT = "@SoapText";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_CPT_SNOMEDID = "@CPTSNOMEDID";
        private const string PARM_CPT_SNOMEDDESCRIPTION = "@CPTSNOMEDDescription";

        //End//18-04-2016//Abid Ali// Paramenters Names


        //Start 18-04-2016 Muhammad Arshad Lookup LOINC
        private const string PARM_LOINC_CODE = "@LOINCCode";
        private const string PARM_LOINC_CODE_DESCRIPTION = "@LOINCCodeDescription";
        //End 18-04-2016 Muhammad Arshad Lookup LOINC

        private const string PARM_ASSIGNEE_ID = "@AssigneeId";

        private const string PARM_IS_SENT_TO_POTRTAL = "@IsSentToPortal";

        private const string PARM_IS_AKNOWLEDGED = "@IsAknowledged";

        private const string PARM_LAB_ID = "@LabId";

        private const string RADIOLOGY_ORDER_TEST_ID = "@RadiologyOrderTestId";
        private const string PARM_TYPE = "@Type";

        #endregion

        #region Constructors

        public DALRadiologyResult()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        public DALRadiologyResult(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
        }

        private IContainer components;

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Support Functions for Radiology Result & Radiology Result Detail


        /// <summary>
        /// Radiology Result Insert Update parameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateRadiologyOderResultInsertUpdateParameters(IDBManager dbManager, DSRadiologyResult ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(19);
                dbManager.AddInsertUpdateParameters(0, PARM_Radiology_ORDER_RESULT_ID, ds.RadiologyOrderResult.RadiologyOrderResultIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(19);
                dbManager.AddInsertUpdateParameters(0, PARM_Radiology_ORDER_RESULT_ID, ds.RadiologyOrderResult.RadiologyOrderResultIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_Radiology_ORDER_ID, ds.RadiologyOrderResult.RadiologyOrderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ORDER_NO, ds.RadiologyOrderResult.OrderNoColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_STATUS, ds.RadiologyOrderResult.StatusColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.RadiologyOrderResult.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.RadiologyOrderResult.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.RadiologyOrderResult.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.RadiologyOrderResult.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.RadiologyOrderResult.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.RadiologyOrderResult.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAP_TEXT, ds.RadiologyOrderResult.SoapTextColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(11, PARM_PROVIDER_ID, ds.RadiologyOrderResult.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(12, PARM_PATIENT_ID, ds.RadiologyOrderResult.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(13, PARM_REMARKS, ds.RadiologyOrderResult.RemarksColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_ASSIGNEE_ID, ds.RadiologyOrderResult.AssigneeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(15, PARM_IS_SENT_TO_POTRTAL, ds.RadiologyOrderResult.IsSentToPortalColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(16, PARM_IS_AKNOWLEDGED, ds.RadiologyOrderResult.IsAknowledgedColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(17, PARM_NOTE_ID, ds.RadiologyOrderResult.NoteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(18, PARM_TYPE, ds.RadiologyOrderResult.TypeColumn.ColumnName, DbType.String);


        }


        /// <summary>
        ///Radiology Order Result Detail Insert Update parameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void CreateRadiologyOderResultDetailInsertUpdateParameters(IDBManager dbManager, DSRadiologyResult ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(24);
                dbManager.AddInsertUpdateParameters(0, PARM_Radiology_ORDER_RESULT_DETAIL_ID, ds.RadiologyOrderResultDetail.RadiologyOrderResultDetailIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(24);
                dbManager.AddInsertUpdateParameters(0, PARM_Radiology_ORDER_RESULT_DETAIL_ID, ds.RadiologyOrderResultDetail.RadiologyOrderResultDetailIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_Radiology_ORDER_RESULT_ID, ds.RadiologyOrderResultDetail.RadiologyOrderResultIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CPT_CODE, ds.RadiologyOrderResultDetail.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CPT_DESCRIPTION, ds.RadiologyOrderResultDetail.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_OBSERVATION_DATE, ds.RadiologyOrderResultDetail.ObservationDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_LOINC, ds.RadiologyOrderResultDetail.LOINCColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_LOINC_DECSRIPTION, ds.RadiologyOrderResultDetail.LOINCDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_RESULT, ds.RadiologyOrderResultDetail.ResultColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(8, PARM_UoM, ds.RadiologyOrderResultDetail.UoMColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_FLAG, ds.RadiologyOrderResultDetail.FlagColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_RANGE, ds.RadiologyOrderResultDetail.RangeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_STATUS, ds.RadiologyOrderResultDetail.StatusColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(12, PARM_COMMENTS, ds.RadiologyOrderResultDetail.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_IS_ACTIVE, ds.RadiologyOrderResultDetail.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(14, PARM_CREATED_BY, ds.RadiologyOrderResultDetail.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(15, PARM_CREATED_ON, ds.RadiologyOrderResultDetail.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(16, PARM_MODIFIED_BY, ds.RadiologyOrderResultDetail.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(17, PARM_MODIFIED_ON, ds.RadiologyOrderResultDetail.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(18, PARM_SOAP_TEXT, ds.RadiologyOrderResultDetail.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_PROVIDER_ID, ds.RadiologyOrderResultDetail.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(20, PARM_PATIENT_ID, ds.RadiologyOrderResultDetail.PatientIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(21, PARM_CPT_SNOMEDID, ds.RadiologyOrderResultDetail.CPTSNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(22, PARM_CPT_SNOMEDDESCRIPTION, ds.RadiologyOrderResultDetail.CPTSNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(23, PARM_REMARKS, ds.RadiologyOrderResultDetail.RemarksColumn.ColumnName, DbType.String);

        }

        //End//31-03-2016//Abid Ali// Support functions for Radiology Result

        #endregion

        #region Radiology Result (Insert, Update, Delete, Select)

        // Author: Abid Ali
        // Date: 18/044/2016
        //This function will insert/update Radiology Result
        public DSRadiologyResult InsertUpdateRadiologyResult(DSRadiologyResult ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.RadiologyOrderResult.GetChanges();
                dbManager.BeginTransaction();
                this.CreateRadiologyOderResultInsertUpdateParameters(dbManager, ds, true);
                this.CreateRadiologyOderResultInsertUpdateParameters(dbManager, ds, false);

                ds = (DSRadiologyResult)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_Radiology_ORDER_RESULT_INSERT, PROC_Radiology_ORDER_RESULT_UPDATE, ds, ds.RadiologyOrderResult.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.RadiologyOrderResult.Rows[0][ds.RadiologyOrderResult.RadiologyOrderResultIdColumn].ToString(), null, ds.RadiologyOrderResult.Rows[0][ds.RadiologyOrderResult.RadiologyOrderResultIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALRadiologyResult::PROC_RadiologyResult_INSERT", PROC_Radiology_ORDER_RESULT_INSERT + " " + PROC_Radiology_ORDER_RESULT_UPDATE, ex);
                throw ex;
            }
        }

        // Author: Abid Ali
        // Date: 18/044/2016
        //This function will load Radiology Result
        public DSRadiologyResult LoadRadiologyResult(long RadiologyResultId, long RadiologyOrderId, string pageNumber, string rowsPerPage, string test, string orderNo, long providerId, string orderDateFrom, string orderDateTo, string status, long patientId, string isViewOrder = "", string isPrintOrder = "", long noteId = 0, long labId = 0, int IsSentToPortal = 0, bool AllResults = false, SharedVariable sharedVariable = null)
        {
            DSRadiologyResult ds = new DSRadiologyResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {

                int page;
                int rpp;
                if (string.IsNullOrEmpty(pageNumber))
                {
                    page = 1;
                }
                else
                {
                    page = Convert.ToInt32(pageNumber);
                }

                if (string.IsNullOrEmpty(rowsPerPage))
                {
                    rpp = 2000;
                }
                else
                {
                    rpp = Convert.ToInt32(rowsPerPage);
                }

                dbManager.BeginTransaction();
                dbManager.CreateParameters(15);

                if (RadiologyResultId == 0)
                    dbManager.AddParameters(0, PARM_Radiology_ORDER_RESULT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Radiology_ORDER_RESULT_ID, RadiologyResultId);
                if (RadiologyOrderId <= 0)
                    dbManager.AddParameters(1, PARM_Radiology_ORDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_Radiology_ORDER_ID, RadiologyOrderId);
                if (page <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, page);
                if (rpp <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, rpp);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.RadiologyOrderResult.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                //Start 31-03-2016 Abid Ali 

                if (test == "")
                    dbManager.AddParameters(5, PARM_TEST, null);
                else
                    dbManager.AddParameters(5, PARM_TEST, test);

                if (orderNo == "")
                    dbManager.AddParameters(6, PARM_ORDER_NO, null);
                else
                    dbManager.AddParameters(6, PARM_ORDER_NO, orderNo);

                if (providerId == 0)
                    dbManager.AddParameters(7, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(7, PARM_PROVIDER_ID, providerId);

                if (orderDateFrom == "")
                    dbManager.AddParameters(8, PARM_ORDER_DATE_FROM, null);
                else
                    dbManager.AddParameters(8, PARM_ORDER_DATE_FROM, orderDateFrom);

                if (orderDateTo == "")
                    dbManager.AddParameters(9, PARM_ORDER_DATE_TO, null);
                else
                    dbManager.AddParameters(9, PARM_ORDER_DATE_TO, orderDateTo);

                if (status == "")
                    dbManager.AddParameters(10, PARM_STATUS, null);
                else
                    dbManager.AddParameters(10, PARM_STATUS, status);
                if (patientId == 0)
                    dbManager.AddParameters(11, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(11, PARM_PATIENT_ID, patientId);

                if (noteId == 0)
                    dbManager.AddParameters(12, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(12, PARM_NOTE_ID, noteId);

                if (labId == 0)

                    dbManager.AddParameters(13, PARM_LAB_ID, null);
                else
                    dbManager.AddParameters(13, PARM_LAB_ID, labId);


                dbManager.AddParameters(14, PARM_IS_SENT_TO_POTRTAL, IsSentToPortal);

                if (noteId > 0 && !AllResults)
                {
                    ds = (DSRadiologyResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Radiology_ORDER_RESULT_SELECT_AGAINST_NOTE, ds, ds.RadiologyOrderResult.TableName);
                }
                else
                {
                    ds = (DSRadiologyResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Radiology_ORDER_RESULT_SELECT, ds, ds.RadiologyOrderResult.TableName);
                }

                if (RadiologyOrderId > 0 && ds.RadiologyOrderResult.Count > 0)
                {

                    DataTable dtTemp = ds.RadiologyOrderResult;
                    if (dtTemp != null)
                    {
                        if ((isViewOrder == "1" || isPrintOrder == "1") && noteId == 0)
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            //dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.RadiologyOrderResult.Rows[0][ds.RadiologyOrderResult.RadiologyOrderResultIdColumn].ToString(), null, ds.RadiologyOrderResult.Rows[0][ds.RadiologyOrderResult.RadiologyOrderResultIdColumn].ToString(), isViewAction, isPrintAcion);
                            //dsDBAudit.AcceptChanges();
                            if (sharedVariable != null)
                                new DBActivityAudit(sharedVariable).InsertDBAuditAsync(dtTemp, ds.RadiologyOrderResult.Rows[0][ds.RadiologyOrderResult.RadiologyOrderResultIdColumn].ToString(), null, ds.RadiologyOrderResult.Rows[0][ds.RadiologyOrderResult.RadiologyOrderResultIdColumn].ToString(), isViewAction, isPrintAcion, false, "", "0", sharedVariable);
                            else
                                new DBActivityAudit().InsertDBAuditAsync(dtTemp, ds.RadiologyOrderResult.Rows[0][ds.RadiologyOrderResult.RadiologyOrderResultIdColumn].ToString(), null, ds.RadiologyOrderResult.Rows[0][ds.RadiologyOrderResult.RadiologyOrderResultIdColumn].ToString(), isViewAction, isPrintAcion, false, "", "0");
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALRadiologyResult::Radiology_ORDER_SELECT", PROC_Radiology_ORDER_RESULT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Abid Ali
        // Date: 17/03/2016
        //This function will delete Radiology Result
        public string DeleteRadiologyResult(long RadiologyResultId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                DSRadiologyResult dsCurrentOrder = LoadRadiologyResult(RadiologyResultId, 0, "", "", "", "", 0, "", "", "", 0, "", "");
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_Radiology_ORDER_RESULT_ID, RadiologyResultId);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_Radiology_ORDER_RESULT_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrder.RadiologyOrderResult;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, RadiologyResultId.ToString(), null, RadiologyResultId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALRadiologyResult::DeleteRadiologyResult", PROC_Radiology_ORDER_RESULT_DELETE, ex);
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

        #region RadiologyResult Detail (Insert, Update, Delete, Select)

        /// <summary>
        /// Method Name: insertUpdateRadiologyResultDetails
        /// Author: Abid Ali
        /// Created Date: 18-04-2016
        /// Description: insert/update  Radiology Result Details
        /// </summary> 
        /// <param name="DSRadiologyResult" type="DATASET"></param>
        public DSRadiologyResult insertUpdateRadiologyResultDetail(DSRadiologyResult ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.RadiologyOrderResultDetail.GetChanges();
                dbManager.BeginTransaction();
                CreateRadiologyOderResultDetailInsertUpdateParameters(dbManager, ds, true);
                CreateRadiologyOderResultDetailInsertUpdateParameters(dbManager, ds, false);
                ds = (DSRadiologyResult)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_Radiology_ORDER_RESULT_DETAIL_INSERT, PROC_Radiology_ORDER_RESULT_DETAIL_UPDATE, ds, ds.RadiologyOrderResultDetail.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.RadiologyOrderResultDetail.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.RadiologyOrderResultDetail.Rows[i][ds.RadiologyOrderResultDetail.RadiologyOrderResultDetailIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.RadiologyOrderResultDetail.Rows[0][ds.RadiologyOrderResultDetail.RadiologyOrderResultDetailIdColumn].ToString(), null, ds.RadiologyOrderResultDetail.Rows[0][ds.RadiologyOrderResultDetail.RadiologyOrderResultIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALRadiologyResult::insertUpdateRadiologyResultDetail", PROC_Radiology_ORDER_RESULT_DETAIL_INSERT + " " + PROC_Radiology_ORDER_RESULT_DETAIL_UPDATE, ex);
                throw ex;

            }
            finally
            {
                dbManager.Dispose();
            }
        }



        /// <summary>
        /// Method Name: loadRadiologyResultDetaii
        /// Author: Abid Ali
        /// Created Date: 31-03-2016
        /// Description: loading Radiology Result Detail
        /// </summary> 
        /// <param name="RadiologyResultId" type="long">RadiologyOrderResultId</param>
        /// <param name="patientId" type="long"></param>
        /// <param name="pageNumber" type="int"></param>
        /// <param name="rowsPerPage" type="int"></param>
        public DSRadiologyResult loadRadiologyResultDetail(long RadiologyOrderResultDetailId, long RadiologyOrderResultId, SharedVariable sharedVariable = null)
        {
            DSRadiologyResult ds = new DSRadiologyResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (RadiologyOrderResultDetailId == 0)
                    dbManager.AddParameters(0, PARM_Radiology_ORDER_RESULT_DETAIL_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Radiology_ORDER_RESULT_DETAIL_ID, RadiologyOrderResultDetailId);

                if (RadiologyOrderResultId == 0)
                    dbManager.AddParameters(1, PARM_Radiology_ORDER_RESULT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_Radiology_ORDER_RESULT_ID, RadiologyOrderResultId);

                ds = (DSRadiologyResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Radiology_ORDER_RESULT_DETAIL_SELECT, ds, ds.RadiologyOrderResultDetail.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALRadiologyOrderResultDetail::loadRadiologyOrderResultDetail", PROC_Radiology_ORDER_RESULT_DETAIL_SELECT, ex);
                MDVLogger.SendExcepToDB(ex, "DALRadiologyOrderResultDetail::loadRadiologyOrderResultDetail", PROC_Radiology_ORDER_RESULT_DETAIL_SELECT);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Method Name: deleteRadiologyResultDetail
        /// Author: Abid Ali
        /// Created Date: 18-04-2016
        /// Description: deleting Radiology Result Detail
        /// </summary> 
        /// <param name="RadiologyResultId" type="long">RadiologyResultId to be deleted</param>
        /// 
        public string deleteRadiologyResultDetail(long RadiologyResultDetailId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                DSRadiologyResult dsCurrentOrderDetails = loadRadiologyResultDetail(RadiologyResultDetailId, 0);
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_Radiology_ORDER_RESULT_DETAIL_ID, RadiologyResultDetailId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_Radiology_ORDER_RESULT_DETAIL_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrderDetails.RadiologyOrderResultDetail;
                    if (dtTemp != null)
                    {

                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, dsCurrentOrderDetails.RadiologyOrderResultDetail.Rows[0].ToString(), null, dsCurrentOrderDetails.RadiologyOrderResultDetail.Rows[0][dsCurrentOrderDetails.RadiologyOrderResultDetail.RadiologyOrderResultIdColumn].ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALRadiologyResult::deleteRadiologyResultDetail", PROC_Radiology_ORDER_RESULT_DETAIL_DELETE, ex);
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

        public DSRadiologyResult loadRadiologyResultsForSoap(string resultID, long patientId, long ProviderId)
        {
            DSRadiologyResult ds = new DSRadiologyResult();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);


                if (string.IsNullOrEmpty(resultID))
                    dbManager.AddParameters(0, PARM_Radiology_ORDER_RESULT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_Radiology_ORDER_RESULT_ID, resultID);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                if (ProviderId == 0)
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, DBNull.Value);
                else
                    dbManager.AddParameters(2, PARM_PROVIDER_ID, ProviderId);
                List<string> tableNames = new List<string>
                {
                    ds.RadiologyOrderResult.TableName,
                    ds.RadiologyOrderResultDetail.TableName,
                    ds.RadiologyOrderResultSoapText.TableName
                };

                ds = (DSRadiologyResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIOLOGYORDER_RESULT_SELECT_FOR_SOAPTEXT, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyResult::loadRadiologyResultsForSoap", PROC_RADIOLOGYORDER_RESULT_SELECT_FOR_SOAPTEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string detachRadiologyOrderResultFromNotes(string radiologyResultId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (string.IsNullOrEmpty(radiologyResultId))
                {
                    dbManager.AddParameters(0, PARM_Radiology_ORDER_RESULT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_Radiology_ORDER_RESULT_ID, radiologyResultId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_RADIOLOGY_ORDER_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyResult::detachRadiologyOrderResultFromNotes", PROC_DETACH_RADIOLOGY_ORDER_FROM_NOTES, ex);
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

        public DSRadiologyResult attachRadiologyResultWithNotes(string radiologyResultId, long noteId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSRadiologyResult ds = new DSRadiologyResult();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(radiologyResultId))
                {
                    dbManager.AddParameters(0, PARM_Radiology_ORDER_RESULT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_Radiology_ORDER_RESULT_ID, radiologyResultId);
                }

                if (noteId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, noteId);
                }


                ds = (DSRadiologyResult)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_RADIOLOGYORDER_RESULT_WITH_NOTES, ds, ds.RadiologyOrderResult.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyResult::attachRadiologyResultWithNotes", PROC_ATTACH_RADIOLOGYORDER_RESULT_WITH_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string DeleteRadiologyOrderResultTest(long labTestId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            //DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                //dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, RADIOLOGY_ORDER_TEST_ID, labTestId);
                //dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, RADIOLOGY_ORDER_RESULT_TEST_DELETE).ToString();

                return "";
            }
            catch (Exception ex)
            {
                //dsDBAudit.RejectChanges();
                //dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALLabResult::DeleteRadiologyOrderResultTest", RADIOLOGY_ORDER_RESULT_TEST_DELETE, ex);
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

        #region Legacy Notes

        public List<RadOrderResult> NotesRadOrderResultSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<RadOrderResult> objList_RadOrderResult = new List<RadOrderResult>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_RADORDERRESULT_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        RadOrderResult model = new RadOrderResult();
                        var properties = typeof(RadOrderResult).GetProperties();
                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        objList_RadOrderResult.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALRadiologyResult::NotesRadOrderResultSelect", PROC_NOTES_RADORDERRESULT_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_RadOrderResult;
        }

        #endregion Legacy Notes

    }
}
