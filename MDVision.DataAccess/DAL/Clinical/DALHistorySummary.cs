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
using MDVision.Model.Clinical.History.HistorySummary;

namespace MDVision.DataAccess.DAL.Clinical
{
    /*
     Author: Muhammad Azhar Shahzad
     Date Creation: Dec 16,2015
     Reason: Business Requirements to manipulate data with Database
 */
    public class DALHistorySummary
    {
        #region Variable

        #endregion
        #region "Stored Procedure Names"
        private const string PROC_HISTORY_SUMMARY_FOR_SOAP_SELECT = "Clinical.sp_HistorySummaryForSoapSelect";
        private const string PROC_HXLOG_SELECT = "Clinical.sp_HXLogSelect";

        #endregion

        #region "Parameters"
        private const string PARM_PATIENT_ID = "@PatientId";

        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_HX_ID = "@HxId";
        private const string PARM_HX_TYPE = "@HxType";
        private const string PARM_LOG_TYPE = "@LogType";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        #endregion

        #region Constructors
        public DALHistorySummary()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALHistorySummary(SharedVariable SharedVariable)
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
        #region "HistorySummary Select SOAP Text"
        public DSHistorySummary loadHistorySummaryForSoap_Obsolete(long PatientId, long NotesId)
        {
            DSHistorySummary ds = new DSHistorySummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (NotesId > 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }

                ds = (DSHistorySummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_HISTORY_SUMMARY_FOR_SOAP_SELECT, ds, ds.HistorySummary_Soap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHistorySummary::loadHistorySummaryForSoap", PROC_HISTORY_SUMMARY_FOR_SOAP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<HistorySummary_Soap> loadHistorySummaryForSoap(long PatientId, long NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                if (NotesId > 0)
                {
                    dbManager.AddParameters(PARM_NOTE_ID, NotesId);
                }
                else
                {
                    dbManager.AddParameters(PARM_NOTE_ID, DBNull.Value);
                }

                List<HistorySummary_Soap> historySummary_SoapList = dbManager.ExecuteReaderMapper<HistorySummary_Soap>(PROC_HISTORY_SUMMARY_FOR_SOAP_SELECT);
                return historySummary_SoapList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHistorySummary::loadHistorySummaryForSoap", PROC_HISTORY_SUMMARY_FOR_SOAP_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSHistorySummary loadHxLog(long hxTypeId, string hxType, string status, int pageNumber = 1, int rowsPerPage = 15)
        {
            DSHistorySummary ds = new DSHistorySummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (hxTypeId == 0)
                    dbManager.AddParameters(0, PARM_HX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_HX_ID, hxTypeId);

                if (string.IsNullOrEmpty(hxType))
                    dbManager.AddParameters(1, PARM_HX_TYPE, null);
                else
                    dbManager.AddParameters(1, PARM_HX_TYPE, hxType);

                if (string.IsNullOrEmpty(status))
                    dbManager.AddParameters(2, PARM_LOG_TYPE, null);
                else
                    dbManager.AddParameters(2, PARM_LOG_TYPE, status);

                if (pageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, pageNumber);

                if (rowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, rowsPerPage);

                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.HxLog.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSHistorySummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_HXLOG_SELECT, ds, ds.HxLog.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHistorySummary::loadHxLog", PROC_HXLOG_SELECT, ex);
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
