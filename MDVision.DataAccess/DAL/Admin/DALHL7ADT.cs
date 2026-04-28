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
    public class DALHL7ADT
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_Mirth_Log_ADT_Select = "System.sp_MirthLogADTSelect";

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
        public DALHL7ADT()
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
            dbManager.CreateParameters(10);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_MIRTH_LOG_ID, ds.HL7ADT.MirthLogIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_MIRTH_LOG_ID, ds.HL7ADT.MirthLogIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_MRNUMBER, ds.HL7ADT.MRNumberColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_NAME, ds.HL7ADT.PatientNameColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(3, PARM_FACILITY, ds.HL7ADT.FacilityColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_PROVIDER, ds.HL7ADT.ProviderColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(5, PARM_MESSAGE_STATUS, ds.HL7ADT.MessageStatusColumn.ColumnName, DbType.String);

            dbManager.AddParameters(6, PARM_MESSAGE, ds.HL7ADT.ErrorMessageColumn.ColumnName, DbType.String);

            dbManager.AddParameters(7, PARM_FILENAME, ds.HL7ADT.FileNameColumn.ColumnName, DbType.String);

            dbManager.AddParameters(8, PARM_CREATED_ON, ds.HL7ADT.CreatedOnColumn.ColumnName, DbType.String);

            dbManager.AddParameters(9, PARM_INS_COMPANY_NAME, ds.HL7ADT.InsCompanyNameColumn.ColumnName, DbType.String);


        }

        #endregion

        #region "get using dataset Functions"

        /// <summary>
        /// LoadHL7ADTLog
        /// </summary>
        /// <param name="MirthLogADTId"></param>
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

        public DSHL7 LoadHL7ADTLog(long MirthLogADTId, string MrNumber,string PatientName, string Facility, string Provider, string MessageStatus, string ErrorMessage, string FileName, string CreatedOn, string InsCompanyName, int PageNumber = 1, int RowsPerPage = 1000)
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
                if (MirthLogADTId <= 0)
                    dbManager.AddParameters(0, PARM_MIRTH_LOG_ID, null);
                else
                    dbManager.AddParameters(0, PARM_MIRTH_LOG_ID, MirthLogADTId);

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



                dbManager.AddParameters(12, PARM_RECORD_COUNT, ds.HL7ADT.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSHL7)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Mirth_Log_ADT_Select, ds, ds.HL7ADT.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHL7ADT::LoadHL7ADTLog", PROC_Mirth_Log_ADT_Select, ex);
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
