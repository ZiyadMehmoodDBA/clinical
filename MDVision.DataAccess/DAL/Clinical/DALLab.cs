using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALLab
    {
         

        #region Constructors

        public DALLab()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
          
        }
        private IContainer components;

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Stored Procedure Names
        private const string PROC_Lab_Select = "Clinical.sp_LabSelect";
        #endregion

        #region Parameters
        private const string PARAM_LabId = "@LabId";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        public DSLab GetLab(long LabId, long ProviderId, long FacilityId)
        {
            DSLab ds = new DSLab();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
          
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARAM_LabId, LabId);
                dbManager.AddParameters(1, "@ProviderId", ProviderId);
                dbManager.AddParameters(2, "@FacilityId", FacilityId);
                dbManager.AddParameters(3, PARM_RECORD_COUNT, ds.Lab.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSLab)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Lab_Select, ds, "Lab");
                return ds;
            }
            catch (Exception e)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALLabOrder::Lab_ORDER_SELECT", PROC_Lab_Select, e);
                throw e;
            }
            finally
            {
                dbManager.Dispose();
            }

            return ds;
        }

    }
}
