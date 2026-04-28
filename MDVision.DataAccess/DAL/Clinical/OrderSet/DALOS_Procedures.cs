using MDVision.Common.Logging;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Clinical.OrderSet
{
    public class DALOS_Procedures
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"
        private const string PROC_PROCEDURES_SELECT = "Clinical.sp_OS_ProceduresSelect";
        private const string PROC_PROCEDURE_INSERT = "Clinical.sp_OS_ProceduresInsert";
        private const string PROC_PROCEDURE_UPDATE = "Clinical.sp_OS_ProceduresUpdate";
        private const string PROC_PROCEDURE_DELETE = "Clinical.sp_OS_ProceduresDelete";
        private const string PROC_GET_PROCEDURE_IDS = "Clinical.sp_OS_GetProcedureIdsOfVaccineHxId";

      
        //sp_ProcedureLookUp

        private const string PROC_Procedure_HISTORY_SELECT = "Clinical.sp_OS_ProceduresHistorySelect";

     
        #endregion

        #region "Parameters"

        private const string PARM_PROCEDURE_ID = "@ProcedureId";//kr
        private const string PARM_PROCEDURE_IDS = "@ProcedureIds";
        private const string PARM_START_DATE = "@StartDate";//
        private const string PARM_END_DATE = "@EndDate";//
        private const string PARM_COMMENTS = "@Comments";//
        private const string PARM_ORDERSET_ID = "@OrderSetId";//
       
        private const string PARM_IS_ACTIVE = "@IsActive";//
        private const string PARM_CREATED_BY = "@CreatedBy";//
        private const string PARM_CREATED_ON = "@CreatedOn";//
        private const string PARM_MODIFIED_BY = "@ModifiedBy";//
        private const string PARM_MODIFIED_ON = "@ModifiedOn";//
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_INACTIVE_VALUE = "@InActiveChkBoxValue";//
        private const string PARM_INACTIVE_REASON = "@InActiveReason";//
        //------------------------------
        private const string PARM_ICD9 = "@ICD9";
        private const string PARM_ICD10 = "@ICD10";
        private const string PARM_SNOMEDID = "@SNOMEDID";
        private const string PARM_CPTCODE = "@CPTCode";
        private const string PARM_SNOMED_DESCRIPTION = "@SNOMED_DESCRIPTION";
        private const string PARM_CPT_DESCRIPTION = "@CPT_DESCRIPTION";
        private const string PARM_ICD9_DESCRIPTION = "@ICD9_DESCRIPTION";
        private const string PARM_ICD10_DESCRIPTION = "@ICD10_DESCRIPTION";


        private const string PARM_CPT_ID = "@CPTId";
        private const string PARM_VACCINE_HX_ID = "@VaccineHxId";
        private const string PARM_IMM_THER_INJECTION_ID = "@ImmTherInjectionId";



        //------------------------------
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_IS_HISTORY = "@IsHistory";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_RCOPIAID = "@RcopiaID";
        private const string PARM_UNIT = "@Unit";
        private const string PARM_MODIFIER = "@Modifier";
        private const string PARM_PROBLEMLIST_ID = "@ProblemListId";
        #endregion

        #region Constructors
        public DALOS_Procedures()
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

        #region "Support Functions For Procedures"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">The is insert.</param>
        private void CreateParameters(IDBManager dbManager, DSOS_Procedures ds, Boolean IsInsert)
        {
            int i = 0;

            if (IsInsert == true)
            {
                dbManager.CreateParameters(20);
                dbManager.AddParameters(i++, PARM_PROCEDURE_ID, ds.Procedures.ProcedureIdColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateParameters(17);
                dbManager.AddParameters(i++, PARM_PROCEDURE_ID, ds.Procedures.ProcedureIdColumn.ColumnName, DbType.Int32);
            }

            dbManager.AddParameters(i++, PARM_START_DATE, ds.Procedures.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(i++, PARM_END_DATE, ds.Procedures.EndDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(i++, PARM_COMMENTS, ds.Procedures.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_ORDERSET_ID, ds.Procedures.OrderSetIdColumn.ColumnName, DbType.Int64);
         
            dbManager.AddParameters(i++, PARM_IS_ACTIVE, ds.Procedures.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(i++, PARM_CREATED_BY, ds.Procedures.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_CREATED_ON, ds.Procedures.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, ds.Procedures.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, ds.Procedures.ModifiedOnColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(i++, PARM_CPTCODE, ds.Procedures.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_CPT_DESCRIPTION, ds.Procedures.CPT_DESCRIPTIONColumn.ColumnName, DbType.String);

            dbManager.AddParameters(i++, PARM_UNIT, ds.Procedures.UnitColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_MODIFIER, ds.Procedures.ModifierColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_PROBLEMLIST_ID, ds.Procedures.ProblemListIdColumn.ColumnName, DbType.Int32);
            
                dbManager.AddParameters(i++, PARM_SNOMEDID, ds.Procedures.SNOMEDIDColumn.ColumnName, DbType.String);
                dbManager.AddParameters(i++, PARM_SNOMED_DESCRIPTION, ds.Procedures.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
            
            if (IsInsert == true)
            {
                dbManager.AddParameters(i++, PARM_CPT_ID, ds.Procedures.CPTIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(i++, PARM_VACCINE_HX_ID, ds.Procedures.VaccineHxIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(i++, PARM_IMM_THER_INJECTION_ID, ds.Procedures.ImmTherInjectionIdColumn.ColumnName, DbType.Int64);

            }
        }

        #endregion

        #region Procedures (CRUD)
        public DSOS_Procedures loadProcedures(long procedureId, long orderSetId,  string isHistory = "0", string active = "", int pageNumber = 1, int rowsPerPage = 1000, string isViewProcedure = "", string isPrintAllergy = "")
        {
            DSOS_Procedures ds = new DSOS_Procedures();
            DSOS_Procedures DSOS_Procedures = new DSOS_Procedures();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "")
                    active = null;
                
               dbManager.Open();
                dbManager.CreateParameters(7);

                if (procedureId == 0)
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, procedureId);
                if (orderSetId == 0)
                    dbManager.AddParameters(1, PARM_ORDERSET_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ORDERSET_ID, orderSetId);
               

                dbManager.AddParameters(2, PARM_IS_HISTORY, "0");

                dbManager.AddParameters(3, PARM_IS_ACTIVE, active);
                if (pageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.Procedures.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSOS_Procedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_SELECT, ds, ds.Procedures.TableName);

                if (isHistory == "1")
                {
                    dbManager.AddParameters(2, PARM_IS_HISTORY, isHistory);
                    DSOS_Procedures dsProcedureHistory = (DSOS_Procedures)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURES_SELECT, ds, ds.ProcedureHistory.TableName);
                    if (dsProcedureHistory != null)
                    {
                        ds.Merge(dsProcedureHistory);
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_Procedures::loadProcedures", PROC_PROCEDURES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSOS_Procedures insertProcedure(DSOS_Procedures ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
               
                CreateParameters(dbManager, ds, true);
                ds = (DSOS_Procedures)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_INSERT, ds, ds.Procedures.TableName);
               
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_Procedures::insertProcedure", PROC_PROCEDURE_INSERT, ex);
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
        public DSOS_Procedures updateProcedure(DSOS_Procedures ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                 dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSOS_Procedures)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_UPDATE, ds, ds.Procedures.TableName);
               
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_Procedures::updateProcedure", PROC_PROCEDURE_UPDATE, ex);
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

        public string deleteProcedure(string procedureId, string VaccineHxId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            IDBManager dbManager1 = ClientConfiguration.GetDBManager();
            try
            {
                if (VaccineHxId != "")//for a time no db aduit
                {
                    dbManager1.Open();
                    dbManager1.CreateParameters(2);
                    dbManager1.AddParameters(0, PARM_VACCINE_HX_ID, VaccineHxId);
                    dbManager1.AddParameters(1, PARM_PROCEDURE_IDS, "", DbType.String, ParamDirection.Output, null, 255);
                    var ReturnProcedureIds = dbManager1.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_PROCEDURE_IDS).ToString();
                    var ReturnProcedureIdsList = ReturnProcedureIds.Split(',');
                    DSOS_Procedures.ProceduresDataTable dtTemp = new DSOS_Procedures.ProceduresDataTable();
                    DSOS_Procedures ds = new DSOS_Procedures();
                    if (ReturnProcedureIds == "")
                    {
                        dtTemp = null;
                        returnVal = "";
                    }
                    else
                    {
                        foreach (string a in ReturnProcedureIds.Split(','))
                        {
                            ds = loadProcedures(Convert.ToInt64(a), 0,  "0", "", 1, 1000);
                            dtTemp.Merge(ds.Procedures);
                        }

                       
                        dbManager.CreateParameters(3);
                        dbManager.AddParameters(0, PARM_PROCEDURE_ID, null);
                        if (!string.IsNullOrEmpty(VaccineHxId))
                        {
                            dbManager.AddParameters(1, PARM_VACCINE_HX_ID, VaccineHxId);
                        }
                        else
                        {
                            dbManager.AddParameters(1, PARM_VACCINE_HX_ID, null);
                        }
                        dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                        returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROCEDURE_DELETE).ToString();

                        if (returnVal != "")
                        {
                            throw new Exception(returnVal);
                        }
                    }


                }
                else
                {
                    dbManager.Open();
                    dbManager.CreateParameters(3);
                    dbManager.AddParameters(0, PARM_PROCEDURE_ID, procedureId);
                    if (!string.IsNullOrEmpty(VaccineHxId))
                    {
                        dbManager.AddParameters(1, PARM_VACCINE_HX_ID, VaccineHxId);
                    }
                    else
                    {
                        dbManager.AddParameters(1, PARM_VACCINE_HX_ID, null);
                    }
                    dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                    returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROCEDURE_DELETE).ToString();

                    if (returnVal != "")
                        throw new Exception(returnVal);
                 
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOS_Procedures::deleteProcedure", PROC_PROCEDURE_DELETE, ex);
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

  

      

       
    }
}
