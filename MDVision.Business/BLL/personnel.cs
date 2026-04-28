using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.Admin;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Business.BCommon;
using MDVision.Common.Logging;
namespace MDVision.Business.BLL
{
    public class Personnel
    {
        #region Variable
        
        #endregion
        #region Singleton
        private static Personnel _instance = null;
        /// <summary>
        /// Singleton context
        /// </summary>
        public static Personnel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Personnel();
                return _instance;
            }
        }
        #endregion

        #region "Functions"
        public BLObject<DSPersonnel> InsertPersonnel(DSPersonnel ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            
            try
            {
                //IDBManager dbManager = new DBManager(DataProvider.SqlServer);
                
                //DALpersonnel.Instance.UpdateData(ref ds);
                //DALpersonnel.Instance.PersonnelParameters(dbManager,ds);   
                //dbManager.Open();
                dbManager.BeginTransaction();
             
               // ds = (Personnel)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PERSONNEL_INSERT, ds, ds.tblPersonnel.TableName);

                //ds = (Personnel)
               DALpersonnel  object1= new  DALpersonnel();
               object1.InsertAndUpdatePersonnel(ref ds, dbManager);

               //throw new Exception();

                foreach (DataRow dr in ds.Tables[ds.tblPersonnel.TableName].Rows)
                {
                    
                        foreach (DataRow drDetail in ds.Tables[ds.tblPersonnelAddress.TableName].Rows)
                        {
                            if (drDetail.RowState != DataRowState.Deleted)
                            {
                                drDetail[ds.tblPersonnelAddress.PersonnelIDColumn] = dr[ds.tblPersonnel.IDColumn];
                            }
                        }
                                        
                }
                DALpersonnelAddress object2= new DALpersonnelAddress();
                object2.InsertPersonnelAddress(ref ds, dbManager);

                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return new BLObject<DSPersonnel>(ds);
               
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("Personnel::InsertPersonnel", ex);
                return new BLObject<DSPersonnel>(null, ex.Message);
                //throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
           
        }
                
        #endregion

    }
}
