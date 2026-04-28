using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Shared;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALInsurance
    {
        #region Variable
        
        #endregion

        #region " Stored Procedure Names"
        private const string PROC_INSURANCE_INSERT = "Provider.sp_InsuranceInsert";
        private const string PROC_INSURANCE_UPDATE = "Provider.sp_InsuranceUpdate";
        private const string PROC_INSURANCE_DELETE = "Provider.sp_InsuranceDelete";
        private const string PROC_INSURANCE_SELECT = "Provider.sp_InsuranceSelect";
        private const string PROC_INSURANCE_LOOKUP = "Provider.sp_InsuranceLookup";
        #endregion

        #region "Parameters"
        private const string PARM_INSURANCE_ID = "@InsuranceId";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_WEBSITE_URL = "@WebSiteURL";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region Constructors
        public DALInsurance()
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
        private void CreateParameters(IDBManager dbManager, DSInsurance ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_INSURANCE_ID, ds.Insurance.InsuranceIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_INSURANCE_ID, ds.Insurance.InsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.Insurance.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.Insurance.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_EMAIL_ADDRESS, ds.Insurance.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_WEBSITE_URL, ds.Insurance.WebSiteURLColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.Insurance.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.Insurance.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.Insurance.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.Insurance.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.Insurance.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_ENTITY_ID, ds.Insurance.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the insurance.
        /// </summary>
        /// <param name="InsuranceId">The insurance identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public DSInsurance LoadInsurance(long InsuranceId, string ShortName, string Description, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSInsurance ds = new DSInsurance();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                if (EntityId == "")
                    EntityId = null;

                if (IsActive == "")
                    IsActive = null;
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (InsuranceId <= 0)
                    dbManager.AddParameters(0, PARM_INSURANCE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_INSURANCE_ID, InsuranceId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(4, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.Insurance.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(8, PARM_IS_ACTIVE, IsActive);
                ds = (DSInsurance)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_SELECT, ds, ds.Insurance.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurance::LoadInsurance", PROC_INSURANCE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the insurance.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSInsurance UpdateInsurance(ref DSInsurance ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Insurance.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSInsurance)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_INSURANCE_UPDATE, ds, ds.Insurance.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Insurance.Rows[0][ds.Insurance.InsuranceIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurance::UpdateInsurance", PROC_INSURANCE_UPDATE, ex);
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

        /// <summary>
        /// Deletes the insurance.
        /// </summary>
        /// <param name="InsuranceIds">The insurance ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteInsurance(string InsuranceIds)
        {
            string returnValue;
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                DSInsurance ds = LoadInsurance(Convert.ToInt64(InsuranceIds),null,null,null,null);
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Insurance;
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_INSURANCE_ID, InsuranceIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_INSURANCE_DELETE);
                
                //returnValue = returnVal.ToString();
                if (returnVal != null && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal.ToString());
                }
                //else
                //{
                //    if (dtTemp != null && ds.Insurance.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Insurance.Rows[0][ds.Insurance.InsuranceIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurance::DeleteInsurance", PROC_INSURANCE_DELETE, ex);
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
        /// Inserts the insurance.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSInsurance InsertInsurance(ref DSInsurance ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Insurance.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSInsurance)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSURANCE_INSERT, ds, ds.Insurance.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Insurance.Rows[0][ds.Insurance.InsuranceIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurance::InsertInsurance", PROC_INSURANCE_INSERT, ex);
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

        /// <summary>
        /// Lookups the insurance.
        /// </summary>
        /// <returns></returns>
        public DSProfileLookup LookupInsurance(string Active)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_INSURANCE_LOOKUP, ds, ds.Insurance.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsurance::LookupInsurance", PROC_INSURANCE_LOOKUP, ex);
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
