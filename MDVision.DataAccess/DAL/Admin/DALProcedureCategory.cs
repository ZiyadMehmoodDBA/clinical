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
    public class DALProcedureCategory

    { 
        #region Variable
        
        #endregion

        #region " Stored Procedure Names"
        private const string PROC_PROCEDURE_CATEGORY_INSERT = "Provider.sp_ProcedureCategoryInsert";
        private const string PROC_PROCEDURE_CATEGORY_UPDATE = "Provider.sp_ProcedureCategoryUpdate";
        private const string PROC_PROCEDURE_CATEGORY_DELETE = "Provider.sp_ProcedureCategoryDelete";
        private const string PROC_PROCEDURE_CATEGORY_SELECT = "Provider.sp_ProcedureCategorySelect";
        private const string PROC_LOOKUP_PROCEDURE_CATEGORY = "Provider.sp_ProcedureCategoryLookup";
 
        #endregion

        #region "Query "

        #endregion

        #region "Parameters"

        private const string PARM_PROC_CATEGORY_ID = "@ProcCategoryId";
        private const string PARM_NAME = "@Name";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
       
       

        public struct Parameters
        {
            public int ID;
            public string FNAME;
            public string LNAME;
        }

        #endregion

     
        #region Constructors 
        
        public DALProcedureCategory()
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
        private void CreateParameters(IDBManager dbManager, DSCodes ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(9);
            //dbManager.CreateParameters(ds.Tables[ds.ProcedureCategory.TableName].Columns.Count);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PROC_CATEGORY_ID, ds.ProcedureCategory.ProcCategoryIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PROC_CATEGORY_ID, ds.ProcedureCategory.ProcCategoryIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_NAME, ds.ProcedureCategory.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.ProcedureCategory.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.ProcedureCategory.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_CREATED_BY, ds.ProcedureCategory.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_ON, ds.ProcedureCategory.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, ds.ProcedureCategory.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_MODIFIED_ON, ds.ProcedureCategory.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        public DSCodes LoadProcedureCategory(long ProcCategoryId, string Name, string Description, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (Name == "")
                    Name = null;
                if (Description == "")
                    Description = null;
                if (IsActive == "")
                    IsActive = null;

               
                
                dbManager.Open();
                dbManager.CreateParameters(7);

                if (ProcCategoryId <= 0)
                    dbManager.AddParameters(0, PARM_PROC_CATEGORY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROC_CATEGORY_ID, ProcCategoryId);

                dbManager.AddParameters(1, PARM_NAME, Name);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(5, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.ProcedureCategory.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                //dbManager.AddParameters(4, PARM_EIN, EIN);
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_CATEGORY_SELECT, ds, ds.ProcedureCategory.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureCategory::LoadProcedureCategory", PROC_PROCEDURE_CATEGORY_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }

        public DSCodes UpdateProcedureCategory(ref DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ProcedureCategory.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSCodes)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_CATEGORY_UPDATE, ds, ds.ProcedureCategory.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ProcedureCategory.Rows[0][ds.ProcedureCategory.ProcCategoryIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureCategory::UpdateProcedureCategory", PROC_PROCEDURE_CATEGORY_UPDATE, ex);
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


        public string DeleteProcedureCategory(string ProcedureCategoryIds)
        {
            string returnValue = "";
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                //DSCodes ds = LoadProcedureCategory(Convert.ToInt64(ProcedureCategoryIds), null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ProcedureCategory;

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROC_CATEGORY_ID, ProcedureCategoryIds);
               
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROCEDURE_CATEGORY_DELETE).ToString();
                if (returnValue != "")// && ds.Modifier.Rows.Count > 0)
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.ProcedureCategory.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ProcedureCategory.Rows[0][ds.ProcedureCategory.ProcCategoryIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureCategory::DeleteProcedureCategory", PROC_PROCEDURE_CATEGORY_DELETE, ex);
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

        public DSCodes InsertProcedureCategory(ref DSCodes ds)
        {
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ProcedureCategory.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSCodes)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_CATEGORY_INSERT, ds, ds.ProcedureCategory.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ProcedureCategory.Rows[0][ds.ProcedureCategory.ProcCategoryIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProcedureCategory::InsertProcedureCategory", PROC_PROCEDURE_CATEGORY_INSERT, ex);
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

        public DSCodeLookup LookupProcedureCategory(string Active)
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(1);

                //if (POSId <= 0)
                //    dbManager.AddParameters(0, PARM_POS_ID, null);
                //else
                //    dbManager.AddParameters(0, PARM_POS_ID, POSId);

                dbManager.AddParameters(0, PARM_IS_ACTIVE, Active);

                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOKUP_PROCEDURE_CATEGORY, ds, ds.ProcedureCategory.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupProcedureCategory", PROC_LOOKUP_PROCEDURE_CATEGORY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region "use for transaction with dataset"
       
        #endregion
        #endregion
    }
}

