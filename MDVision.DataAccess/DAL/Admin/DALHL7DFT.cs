using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALHL7DFT
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_Mirth_Log_DFT_Select = "System.sp_MirthLogDFTSelect";

        #endregion

        #region "Parameters"
        private const string PARM_MIRTH_LOG_ID = "@MirthLogID";
        private const string PARM_FILENAME = "@FileName";
        private const string PARM_MESSAGE_STATUS = "@MessageStatus";

        private const string PARM_MRNUMBER = "@MRNumber";
        private const string PARM_PATIENT_NAME = "@PatientName";

        private const string PARM_FACILITY = "@Facility";
        private const string PARM_PROVIDER = "@Provider";

        private const string PARM_REF_PROVIDER = "@RefProvider";
        private const string PARM_PC_PROVIDER = "@PCProvider";
        private const string PARM_GUARANTOR = "@Guarantor";

        private const string PARM_INS_COMPANY_NAME = "@InsCompanyName";
        private const string PARM_INS_SUBSCRIBER_NUMBER = "@SubNumber";

        private const string PARM_INSURED = "@Insured";
        private const string PARM_CREATED_ON = "@CreatedOn";

        private const string PARM_MESSAGE = "@Message";
        private const string PARM_FILE_TEXT = "@FileText";
        private const string PARM_ICD_CODE1 = "@ICDCode1";
        private const string PARM_ICD_CODE2 = "@ICDCode2";
        private const string PARM_ICD_CODE3 = "@ICDCode3";
        private const string PARM_ICD_CODE4 = "@ICDCode4";
        private const string PARM_ICD_DESC_1 = "@ICDCodeDesc1";
        private const string PARM_ICD_DESC_2 = "@ICDCodeDesc2";
        private const string PARM_ICD_DESC_3 = "@ICDCodeDesc3";
        private const string PARM_ICD_DESC_4 = "@ICDCodeDesc4";
        private const string PARM_COPAY = "@FinTranCoPay";
        private const string PARM_PROCEDURE_CODE = "@FinTranProcedureCode";
        private const string PARM_START_DATE = "@FinTranStartDate";
        private const string PARM_END_DATE = "@FinTranEndDate";

        private const string PARM_REJECTED = "@Rejected";
        private const string PARM_ACCEPTED = "@Accepted";
        private const string PARM_ERRORED = "@Errored";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALClearingHouse"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALHL7DFT()
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
        private void CreateParameters(IDBManager dbManager, DSHL7 ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(22);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MIRTH_LOG_ID, ds.HL7DFT.MirthLogIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MIRTH_LOG_ID, ds.HL7DFT.MirthLogIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_MRNUMBER, ds.HL7DFT.MRNumberColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_NAME, ds.HL7DFT.PatientNameColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(3, PARM_FACILITY, ds.HL7DFT.FacilityColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_PROVIDER, ds.HL7DFT.ProviderColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(5, PARM_MESSAGE_STATUS, ds.HL7DFT.MessageStatusColumn.ColumnName, DbType.String);

            dbManager.AddParameters(6, PARM_MESSAGE, ds.HL7DFT.ErrorMessageColumn.ColumnName, DbType.String);

            dbManager.AddParameters(7, PARM_FILENAME, ds.HL7DFT.FileNameColumn.ColumnName, DbType.String);

            dbManager.AddParameters(8, PARM_CREATED_ON, ds.HL7DFT.CreatedOnColumn.ColumnName, DbType.String);

            dbManager.AddParameters(9, PARM_INS_COMPANY_NAME, ds.HL7DFT.InsCompanyNameColumn.ColumnName, DbType.String);

            dbManager.AddParameters(10, PARM_ICD_CODE1, ds.HL7DFT.ICDCode1Column.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_ICD_CODE2, ds.HL7DFT.ICDCode2Column.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ICD_CODE3, ds.HL7DFT.ICDCode3Column.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_ICD_CODE4, ds.HL7DFT.ICDCode4Column.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_ICD_DESC_1, ds.HL7DFT.ICDCodeDesc1Column.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_ICD_DESC_2, ds.HL7DFT.ICDCodeDesc2Column.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_ICD_DESC_3, ds.HL7DFT.ICDCodeDesc3Column.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_ICD_DESC_4, ds.HL7DFT.ICDCodeDesc4Column.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_COPAY, ds.HL7DFT.FinTranCoPayColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_PROCEDURE_CODE, ds.HL7DFT.FinTranProcedureCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_START_DATE, ds.HL7DFT.FinTranStartDateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_END_DATE, ds.HL7DFT.FinTranEndDateColumn.ColumnName, DbType.String);
        


        }

        #endregion

        #region "get using dataset Functions"

        /// <summary>
        /// LoadHL7DFTLog
        /// </summary>
        /// <param name="MirthLogDFTId"></param>
        /// <param name="MrNumber"></param>
        /// <param name="Facility"></param>
        /// <param name="Provider"></param>
        /// <param name="MessageStatus"></param>
        /// <param name="ErrorMessage"></param>
        /// <param name="FileName"></param>
        /// <param name="CreatedOn"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>

        public DSHL7 LoadHL7DFTLog(long MirthLogDFTId, string MrNumber, string PatientName, string Facility, string Provider, string MessageStatus, string ErrorMessage, string FileName, string CreatedOn, string InsCompanyName, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSHL7 ds = new DSHL7();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (MrNumber == "")
                    MrNumber = null;
                if (PatientName == "")
                    PatientName = null;
                if (Facility == "")
                    Facility = null;
                if (Provider == "")
                    Provider = null;
                if (MessageStatus == "")
                    MessageStatus = null;
                if (ErrorMessage == "")
                    ErrorMessage = null;
                if (FileName == "")
                    FileName = null;
                if (CreatedOn == "")
                    CreatedOn = null;
                if (InsCompanyName == "")
                    InsCompanyName = null;

                dbManager.Open();
                dbManager.CreateParameters(13);
                if (MirthLogDFTId <= 0)
                    dbManager.AddParameters(0, PARM_MIRTH_LOG_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MIRTH_LOG_ID, MirthLogDFTId);

                dbManager.AddParameters(1, PARM_FILENAME, FileName);
                dbManager.AddParameters(2, PARM_MESSAGE_STATUS, MessageStatus);
                dbManager.AddParameters(3, PARM_MRNUMBER, MrNumber);
                dbManager.AddParameters(4, PARM_FACILITY, Facility);
                dbManager.AddParameters(5, PARM_PROVIDER, Provider);


                dbManager.AddParameters(6, PARM_MESSAGE, ErrorMessage);
                dbManager.AddParameters(7, PARM_CREATED_ON, CreatedOn);

                dbManager.AddParameters(8, PARM_PATIENT_NAME, PatientName);
                dbManager.AddParameters(9, PARM_INS_COMPANY_NAME, InsCompanyName);


                if (PageNumber <= 0) { dbManager.AddParameters(10, PARM_PAGE_NUMBER, null); }
                else { dbManager.AddParameters(10, PARM_PAGE_NUMBER, PageNumber); }
                if (RowsPerPage <= 0) { dbManager.AddParameters(11, PARM_ROWS_PAGE, null); }
                else { dbManager.AddParameters(11, PARM_ROWS_PAGE, RowsPerPage); }



                dbManager.AddParameters(12, PARM_RECORD_COUNT, ds.HL7DFT.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSHL7)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Mirth_Log_DFT_Select, ds, ds.HL7DFT.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHL7DFT::LoadHL7DFTLog", PROC_Mirth_Log_DFT_Select, ex);
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
