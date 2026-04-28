using MDVision.Common.Logging;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALOccupationStatus
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"
        private const string PROC_SOCIALHX_MISCHX_OCCUPATIONHX_STATUSINSERTUPDATE = "Clinical.SocialHx_MiscHx_OccupationHx_Statusinsertupdate";
        private const string PROC_SOCIALHX_MISCHX_OCCUPATIONHX_STATUSSELECT = "[Clinical].[SocialHx_MiscHx_OccupationHx_StatusSelect]";
        private const string PROC_SOCIALHX_MISCHX_OCCUPATIONHX_STATUSDELETE = "[Clinical].[SocialHx_MiscHx_OccupationHx_Statusdelete]";
        #endregion

        #region "Parameters"
        private const string PARM_ID = "@Id";
        private const string PARM_STATUS_ID = "@StatusId";
        private const string PARM_CONCEPT_CODE = "@ConceptCode";
        private const string PARM_IS_OCCUPATION = "@IsOccupation";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors 
        /// <summary>
        /// Initializes a new instance of the <see cref="DALOccupationStatus"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALOccupationStatus()
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
        private void CreateParameters(IDBManager dbManager, DSClinicalSummary ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(5);
            dbManager.AddParameters(0, PARM_STATUS_ID, ds.OccupationStatus.StatusIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(1, PARM_DESCRIPTION, ds.OccupationStatus.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_CONCEPT_CODE, ds.OccupationStatus.ConceptCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_OCCUPATION, ds.OccupationStatus.IsOccupationColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_ID, ds.OccupationStatus.IdColumn.ColumnName, DbType.Int32, ParamDirection.Output);

        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        
        public DSClinicalSummary InsertOccupationStatus(ref DSClinicalSummary ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSClinicalSummary)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_MISCHX_OCCUPATIONHX_STATUSINSERTUPDATE, ds, ds.OccupationStatus.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOccupationStatus::InsertOccupationStatus", PROC_SOCIALHX_MISCHX_OCCUPATIONHX_STATUSINSERTUPDATE, ex);
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
        
        public DSClinicalSummary LoadOccupationStatus(long StatusId, string ConceptCode, string Description, string IsOccupation, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSClinicalSummary ds = new DSClinicalSummary();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ConceptCode == "")
                    ConceptCode = null;

                if (Description == "")
                    Description = null;


                if (IsOccupation == "")
                    IsOccupation = null;

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (StatusId <= 0)
                    dbManager.AddParameters(0, PARM_STATUS_ID, null);
                else
                    dbManager.AddParameters(0, PARM_STATUS_ID, StatusId);

                dbManager.AddParameters(1, PARM_CONCEPT_CODE, ConceptCode);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_IS_OCCUPATION, IsOccupation);

                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.OccupationStatus.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSClinicalSummary)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SOCIALHX_MISCHX_OCCUPATIONHX_STATUSSELECT, ds, ds.OccupationStatus.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOccupationStatus::LoadOccupationStatus", PROC_SOCIALHX_MISCHX_OCCUPATIONHX_STATUSSELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }

        public string DeleteOccupationStatus(string StatusId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_STATUS_ID, StatusId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_SOCIALHX_MISCHX_OCCUPATIONHX_STATUSDELETE);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOccupationStatus::DeleteOccupationStatus", PROC_SOCIALHX_MISCHX_OCCUPATIONHX_STATUSDELETE, ex);
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

        //#region "use for transaction with dataset"

        //#endregion
        #endregion

    }
}
