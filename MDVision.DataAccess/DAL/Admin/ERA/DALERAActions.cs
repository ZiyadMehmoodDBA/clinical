using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin.ERA
{
    public class DALERAActions
    {
         #region Variable
        
        #endregion

        #region " Stored Procedure Names"
        private const string PROC_ERAACTION_INSERT = "Billing.sp_ERAActionInsert";
        private const string PROC_ERAACTION_UPDATE = "Billing.sp_ERAActionUpdate";
        private const string PROC_ERAACTION_DELETE = "Billing.sp_ERAActionDelete";
        private const string PROC_ERAACTION_SELECT = "Billing.sp_ERAActionSelect";

   


        #endregion

        #region "Query "

        #endregion

        #region "Parameters"

        private const string PARM_ERAAction_ID = "@ERAActionId";
        private const string PARM_SHORTNAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_LEDGER_ACCOUNTTYPEID = "@LedgerAccountTypeId";
        private const string PARM_IS_ACTIVE = "@isActive";
       
       

       
        #endregion

     
        #region Constructors 
        
        public DALERAActions()
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
        private void CreateParameters(IDBManager dbManager, DSERA ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(5);


            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ERAAction_ID, ds.ERAAction.ERAActionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ERAAction_ID, ds.ERAAction.ERAActionIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORTNAME, ds.ERAAction.ShortNameColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ERAAction.DescriptionColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_LEDGER_ACCOUNTTYPEID, ds.ERAAction.LedgerAccountTypeIdColumn.ColumnName, DbType.Int64);
        

            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.ERAAction.isActiveColumn.ColumnName, DbType.Byte);
          
            //dbManager.AddParameters(13, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSERA LoadERAAction(long ERAActionId)
        {
            DSERA ds = new DSERA();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (ERAActionId <= 0)
                    dbManager.AddParameters(0, PARM_ERAAction_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ERAAction_ID, ERAActionId);

                //dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                //dbManager.AddParameters(2, PARM_DESCRIPTION, Description);

                ds = (DSERA)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ERAACTION_SELECT, ds, ds.ERAAction.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERAAction::LoadERAAction", PROC_ERAACTION_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }

        public DSERA UpdateERAAction(ref DSERA ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSERA)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ERAACTION_UPDATE, ds, ds.ERAAction.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERAAction::UpdateERAAction", PROC_ERAACTION_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }


        }


        public string DeleteERAAction(string ERAActionIds)
        {
            string returnValue = "";
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
             

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ERAAction_ID, ERAActionIds);
               
               // dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ERAACTION_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERAAction::DeleteERAAction", PROC_ERAACTION_DELETE, ex);
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

        public DSERA InsertERAAction(ref DSERA ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSERA)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ERAACTION_INSERT, ds, ds.ERAAction.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALERAAction::InsertERAAction", PROC_ERAACTION_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);       
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }

        }




        #region "use for transaction with dataset"
       
        #endregion

        #endregion
        #region "Lookups"

   
        #endregion
    }
}
