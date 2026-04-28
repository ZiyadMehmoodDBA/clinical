using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALDrugCodeCost
    {
        #region Variable



        #endregion

        #region " Stored Procedure Names"
        private const string PROC_DRUGCODECOST_INSERT = "Provider.sp_DrugCodeCostInsert";
        private const string PROC_DRUGCODECOST_UPDATE = "Provider.sp_DrugCodeCostUpdate";
        private const string PROC_DRUGCODECOST_DELETE = "Provider.sp_DrugCodeCostDelete";
        private const string PROC_DRUGCODECOST_SELECT = "Provider.sp_DrugCodeCostSelect";
        private const string PROC_DRUGCODECOST_LOOKUP = "Provider.sp_DrugCodeCostLookup";
        private const string PROC_DRUGCODECOST_SELECT_ALL = "Provider.sp_DrugCodeCostSelect";

        private const string PROC_SPECIALTY_LOOKUP = "Provider.sp_SpecialtyLookup";
        private const string PROC_SPECIALTY_LOOKUP_ALL = "Provider.sp_AllSpecialities";

        private const string PROC_SPECIALTY_ENTITY_LOOKUP = "Provider.sp_SpecialtyEntityLookup";
        #endregion

        #region "Parameters"
        private const string PARM_CPTCodeCostID = "@CPTCodeCostID";
        private const string PARM_CPTCode = "@CPTCode ";
        private const string PARM_Cost = "@Cost";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@errormessage";
        private const string PARM_USER_ID = "@UserId";


        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALSpecialty"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALDrugCodeCost()
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
        private void CreateParameters(IDBManager dbManager, DSCodes ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(10);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CPTCodeCostID, ds.CPTCodeCost.CPTCodeCostIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CPTCodeCostID, ds.CPTCodeCost.CPTCodeCostIDColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CPTCode, ds.CPTCodeCost.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_Cost, ds.CPTCodeCost.CostColumn.ColumnName, DbType.Decimal);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.CPTCodeCost.IsActiveColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.CPTCodeCost.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.CPTCodeCost.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.CPTCodeCost.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.CPTCodeCost.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the specialty.
        /// </summary>
        /// <param name="SpecialtyId">The specialty identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public DSCodes LoadDrugCodeCost(long CPTCodeCostID, string CTPCode, string Cost, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            try
            {
                if (CTPCode == "")
                    CTPCode = null;

                if (Cost == "")
                    Cost = null;


                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(7);

                if (CPTCodeCostID <= 0)
                    dbManager.AddParameters(0, PARM_CPTCodeCostID, null);
                else
                    dbManager.AddParameters(0, PARM_CPTCodeCostID, CPTCodeCostID);

                dbManager.AddParameters(1, PARM_CPTCode, CTPCode);
                dbManager.AddParameters(2, PARM_Cost, Cost);
                // bool isMultipleEntities = false;
                //if (EntityId == null)
                //{
                //    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                //        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                //    else
                //        dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                //}
                //else
                //{
                //    if (EntityId.IndexOf(',') > -1)
                //    {
                //        isMultipleEntities = true;
                //        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                //    }
                //    else
                //    {
                //        dbManager.AddParameters(3, PARM_ENTITY_ID, Convert.ToInt32(EntityId) < 1 ? null : EntityId);
                //    }

                //}
                //dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, Active);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.CPTCodeCost.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                //if (isMultipleEntities)
                //{
                //    ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_ENTITY_LOOKUP, ds, ds.Specialty.TableName);
                //}
                //else
                //{
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DRUGCODECOST_SELECT, ds, ds.CPTCodeCost.TableName);
                //}

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSpecialty::LoadSpecialty", PROC_DRUGCODECOST_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSCodes LoadDrugCodeCostAll(long CPTCodeCostID, string CPTCode, string Cost, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (CPTCode == "")
                    CPTCode = null;

                if (Cost == "")
                    Cost = null;

                if (Active == "")
                    Active = null;


                dbManager.Open();
                dbManager.CreateParameters(7);

                if (CPTCodeCostID <= 0)
                    dbManager.AddParameters(0, PARM_CPTCodeCostID, null);
                else
                    dbManager.AddParameters(0, PARM_CPTCodeCostID, CPTCodeCostID);

                dbManager.AddParameters(1, PARM_CPTCode, CPTCode);
                dbManager.AddParameters(2, PARM_Cost, Cost);

                dbManager.AddParameters(3, PARM_IS_ACTIVE, Active);
                if (PageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.CPTCodeCost.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DRUGCODECOST_SELECT_ALL, ds, ds.CPTCodeCost.TableName);

                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDrugCodeCost::LoadDrugCodeCost", PROC_DRUGCODECOST_SELECT_ALL, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCodes GetDrugCodeCost()
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DRUGCODECOST_LOOKUP, ds, ds.DrugCodesLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReminders::GetRemindersType", PROC_DRUGCODECOST_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        /// <summary>
        /// Lookups the specialty.
        /// </summary>
        /// <returns></returns>
        //public DSProfileLookup LookupSpecialty(string Active)
        //{
        //    DSProfileLookup ds = new DSProfileLookup();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        if (Active == "")
        //            Active = null;

        //        dbManager.Open();
        //        dbManager.CreateParameters(3);

        //        dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

        //        if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
        //            dbManager.AddParameters(1, PARM_ENTITY_ID, null);
        //        else
        //            dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

        //        dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

        //        ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_LOOKUP, ds, ds.Specialty.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.DALErrorLog("DALSpecialty::LookupSpecialty", PROC_SPECIALTY_LOOKUP, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}


        //public DSProfileLookup LookupSpecialtiesAllEntities(string Active)
        //{
        //    DSProfileLookup ds = new DSProfileLookup();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        if (Active == "")
        //            Active = null;

        //        dbManager.Open();

        //        ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_LOOKUP_ALL, ds, ds.Specialty.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.DALErrorLog("DALSpecialty::LookupSpecialtyAll", PROC_SPECIALTY_LOOKUP_ALL, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}


        //public DSProfileLookup LookupSpecialty(string Active, string EntityId)
        //{
        //    DSProfileLookup ds = new DSProfileLookup();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        if (Active == "")
        //            Active = null;

        //        dbManager.Open();
        //        dbManager.CreateParameters(3);

        //        dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

        //        if (string.IsNullOrEmpty(EntityId))
        //            dbManager.AddParameters(1, PARM_ENTITY_ID, null);
        //        else
        //        {
        //            if (EntityId.IndexOf(',') > -1)
        //            {
        //                dbManager.AddParameters(1, PARM_ENTITY_ID, EntityId);
        //            }
        //            else
        //            {
        //                dbManager.AddParameters(1, PARM_ENTITY_ID, Convert.ToInt32(EntityId) > 0 ? EntityId : null);
        //            }

        //        }

        //        dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

        //        ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SPECIALTY_ENTITY_LOOKUP, ds, ds.Specialty.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.DALErrorLog("DALSpecialty::LookupSpecialty", PROC_SPECIALTY_ENTITY_LOOKUP, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}
        /// <summary>
        /// Updates the specialty.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes UpdateDrugCodeCost(ref DSCodes ds)
        {
            //DALUsersActivity obj = new DALUsersActivity();
            //DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.CPTCodeCost.GetChanges();
                dbManager.BeginTransaction();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSCodes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_DRUGCODECOST_UPDATE, ds, ds.CPTCodeCost.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.CPTCodeCost.Rows[0][ds.CPTCodeCost.CPTCodeCostIDColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}

                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DRUG_CODE_COST, true, "Update Drug Cost Code", ds.Tables[ds.CPTCodeCost.TableName].Rows[0][ds.CPTCodeCost.CPTCodeCostIDColumn.ColumnName].ToString());
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                //dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALDrugCodeCost::UpdateDrugCodeCost", PROC_DRUGCODECOST_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the specialty.
        /// </summary>
        /// <param name="SpecialtyIds">The specialty ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteDrugCodeCost(string DrugCodeCostIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSCodes ds = LoadDrugCodeCost(Convert.ToInt64(DrugCodeCostIds), null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.CPTCodeCost;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CPTCodeCostID, DrugCodeCostIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_SPECIALTY_DELETE);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_DRUGCODECOST_DELETE).ToString();
                if (returnValue != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.CPTCodeCost.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.CPTCodeCost.Rows[0][ds.CPTCodeCost.CPTCodeCostIDColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALDrugCodeCost::DeleteDrugCodeCost", PROC_DRUGCODECOST_DELETE, ex);
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

        /// <summary>
        /// Inserts the specialty.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCodes InsertDrugCodeCost(ref DSCodes ds)
        {
            //DALUsersActivity obj = new DALUsersActivity();
            //DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.CPTCodeCost.GetChanges();
                // dbManager.Open();
                dbManager.BeginTransaction();
                this.CreateParameters(dbManager, ds, true);
                ds = (DSCodes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_DRUGCODECOST_INSERT, ds, ds.CPTCodeCost.TableName);
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.CPTCodeCost.Rows[0][ds.CPTCodeCost.CPTCodeCostIDColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DRUG_CODE_COST, true, "Insert Drug Code Cost", ds.Tables[ds.CPTCodeCost.TableName].Rows[0][ds.CPTCodeCost.CPTCodeCostIDColumn.ColumnName].ToString());
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                //dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DRUG_CODE_COST, false, "Error while inserting the Drug Code Cost : " + ex.ToString());
                MDVLogger.DALErrorLog("DALSpecialty::InsertDrugCodeCost", PROC_DRUGCODECOST_INSERT, ex);
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
