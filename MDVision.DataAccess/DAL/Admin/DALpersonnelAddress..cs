using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using System.Data;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.ComponentModel;
using MDVision.Common.Logging;
namespace MDVision.DataAccess.DAL.Admin
{
    public class DALpersonnelAddress
    {
        #region Variable

        #endregion
        #region " Stored Procedure Names"
        //private const string PROC_PERSONNEL_INSERT = "PersonnelInsert";
        //private const string PROC_PERSONNEL_UPDATE = "PersonnelUpdate";
        //private const string PROC_PERSONNEL_DELETE = "PersonnelDelete";
        //private const string PROC_PERSONNEL_SELECT = "PersonnelGet";

        private const string PROC_PERSONNEL_ADDRESS_INSERT = "PersonnelAddressInsert";
        

        #endregion

        #region "Parameters"
        private const string PARM_ID = "@ID";
        private const string PARM_PERSONNEL_ID = "@PersonnelID";
        private const string PARM_ADDRESS = "@Address";
        #endregion

        //#region Singleton
        //private static DALpersonnelAddress _instance = null;
        ///// <summary>
        ///// Singleton context
        ///// </summary>
        //public static DALpersonnelAddress Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = new DALpersonnelAddress();
        //        return _instance;
        //    }
        //}
        //#endregion
         #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPlanFeeLink"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALpersonnelAddress()
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
        #region "Functions"
        public void CreateParameters(IDBManager dbManager, DSPersonnel ds, Boolean IsInsert )
        {
           
            dbManager.CreateParameters(ds.Tables[ds.tblPersonnelAddress.TableName].Columns.Count);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ID, ds.tblPersonnelAddress.IDColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ID, ds.tblPersonnelAddress.IDColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(1, PARM_PERSONNEL_ID, ds.tblPersonnelAddress.PersonnelIDColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(2, PARM_ADDRESS, ds.tblPersonnelAddress.AddressColumn.ColumnName, DbType.String);

        }
        public void CreateInsertUpdateParameters(IDBManager dbManager, DSPersonnel ds)
        {

            dbManager.CreateInsertUpdateDeleteParameters(ds.Tables[ds.tblPersonnelAddress.TableName].Columns.Count, 1);
            dbManager.AddInsertUpdateDeleteParameters(0, PARM_ID, ds.tblPersonnelAddress.IDColumn.ColumnName, DbType.Int32, ParamDirection.Output);
            dbManager.AddInsertUpdateDeleteParameters(1, PARM_PERSONNEL_ID, ds.tblPersonnelAddress.PersonnelIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateDeleteParameters(2, PARM_ADDRESS, ds.tblPersonnelAddress.AddressColumn.ColumnName, DbType.String);

        }

        #region "insert, delete, update and get using dataset Functions"
        public DSPersonnel InsertPersonnelAddress(ref DSPersonnel ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                

                dbManager.Open();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSPersonnel)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PERSONNEL_ADDRESS_INSERT, ds, ds.tblPersonnelAddress.TableName);
                ds.AcceptChanges();
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALpersonnelAddress::InsertPersonnelAddress", PROC_PERSONNEL_ADDRESS_INSERT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                
                   dbManager.Dispose();
            }
            
            return ds;
        }

        public DSPersonnel InsertPersonnelAddress(ref DSPersonnel ds, IDBManager dbManager)
        {
            
            try
            {
                
                this.CreateParameters(dbManager, ds,true);

                ds = (DSPersonnel)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PERSONNEL_ADDRESS_INSERT, ds, ds.tblPersonnelAddress.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALpersonnelAddress::InsertPersonnelAddress", PROC_PERSONNEL_ADDRESS_INSERT, ex);
                throw ex;
                //Usual code              
            }
          
           
        }
       
        #endregion
        #endregion
    }
}
