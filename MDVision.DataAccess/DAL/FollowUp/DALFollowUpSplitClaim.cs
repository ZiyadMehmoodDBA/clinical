using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.FollowUp
{
    public class DALFollowUpSplitClaim
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_SPLIT_INSURANCE_AR = "Billing.sp_SplitInsuranceAR";

        #endregion

        #region "Parameters"

        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_CHARGE_ID = "@ChargeId";

        #endregion


        #region Constructors
        public DALFollowUpSplitClaim()
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
        
        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        public string SplitClaim(Int64 VisitId, string ChargeIds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            string result = "";
            if (VisitId > 0 && ChargeIds != "")
            {

                try
                {
                    dbManager.Open();
                    dbManager.CreateParameters(2);
                    dbManager.AddParameters(0, PARM_VISIT_ID, VisitId);
                    dbManager.AddParameters(1, PARM_CHARGE_ID, ChargeIds);
                    object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_SPLIT_INSURANCE_AR);

                    if (returnVal != null)
                        throw new Exception(returnVal.ToString());

                    return "";
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("DALFollowUpSplitClaim::SplitClaim", PROC_SPLIT_INSURANCE_AR, ex);
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
            else
            {
                return result;
            }
            
        }

        #endregion
    }
}
