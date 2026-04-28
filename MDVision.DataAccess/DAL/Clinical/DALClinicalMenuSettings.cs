using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALClinicalMenuSettings
    {
        
        
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        private const string PROC_FORM_SELECT = "System.sp_FormsSelect";
        private const string PROC_CLINICALMENUSETTINGS_INSERT = "System.sp_ClinicalMenuSettingsInsert";
        private const string PROC_CLINICALMENUSETTINGS_UPDATE = "System.sp_ClinicalMenuSettingsUpdate";
        private const string PROC_CLINICALMENUSETTINGS_SELECT = "System.sp_ClinicalMenuSettingsSelect";
        

        #endregion

        #region "Parameters"

        private const string PARM_CLINICALMENUSETTINGS_ID = "@ClinicalMenuSettingsId";
        //private const string PARM_MODULE_ID = "@ModuleId";
        //private const string PARM_FORM_ID = "@FormId";
        private const string PARM_USER_ID = "@UserId";
        //private const string PARM_SORTORDER = "@SortOrder";
        //private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_HTML_CLINICALMENUSETTINGS = "@ClinicalMenuHTML";
        private const string PARM_FORMPARENTHTMLID = "@FormParentHTMLId";
        
        #endregion

        #region Constructors
        public DALClinicalMenuSettings()
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
        /// CreateParameters
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void CreateParameters(IDBManager dbManager, DSClinical ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(7);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CLINICALMENUSETTINGS_ID, ds.ClinicalMenuSettings.ClinicalMenuSettingsIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CLINICALMENUSETTINGS_ID, ds.ClinicalMenuSettings.ClinicalMenuSettingsIdColumn.ColumnName, DbType.Int64);

            //dbManager.AddParameters(1, PARM_MODULE_ID, ds.ClinicalMenuSettings.ModuleIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(2, PARM_FORM_ID, ds.ClinicalMenuSettings.FormIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_USER_ID, ds.ClinicalMenuSettings.UserIdColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(4, PARM_SORTORDER, ds.ClinicalMenuSettings.SortOrderColumn.ColumnName, DbType.Int64);
            //dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.ClinicalMenuSettings.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(2, PARM_CREATED_BY, ds.ClinicalMenuSettings.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CREATED_ON, ds.ClinicalMenuSettings.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(4, PARM_MODIFIED_BY, ds.ClinicalMenuSettings.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_MODIFIED_ON, ds.ClinicalMenuSettings.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_HTML_CLINICALMENUSETTINGS, ds.ClinicalMenuSettings.ClinicalMenuHTMLColumn.ColumnName, DbType.String);
            }

        private void CreateParameters_(IDBManager dbManager, DSModuleForm ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(1);
            dbManager.AddParameters(0, PARM_CREATED_BY, ds.Forms.FormsParentHTMLIdColumn.ColumnName, DbType.String);
        }


        #endregion

        #region "select, Insert and update using dataset Functions"

        /// <summary>
        /// LoadClinicalMenuSettings
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="userId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public DSClinical LoadClinicalMenuSettings(long userId)
        {

            DSClinical ds = new DSClinical();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //if (moduleId == 0)
                //    moduleId = long.Parse(null);
                if (userId == 0)
                    userId = long.Parse(null);
                //if (active == "")
                //    active = null;


                dbManager.Open();
                dbManager.CreateParameters(1);
                //if (moduleId <= 0)
                //    dbManager.AddParameters(0, PARM_MODULE_ID, null);
                //else
                //    dbManager.AddParameters(0, PARM_MODULE_ID, moduleId);

                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                // dbManager.AddParameters(2, PARM_IS_ACTIVE, active);

                ds = (DSClinical)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICALMENUSETTINGS_SELECT, ds, ds.ClinicalMenuSettings.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalMenuSettings::LoadClinicalMenuSettings", PROC_CLINICALMENUSETTINGS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

       /// <summary>
        /// InsertClinicalMenuSettings
       /// </summary>
       /// <param name="ds"></param>
       /// <returns></returns>
        public DSClinical InsertClinicalMenuSettings(DSClinical ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSClinical)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CLINICALMENUSETTINGS_INSERT, ds, ds.ClinicalMenuSettings.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalMenuSettings::InsertClinicalMenuSettings", PROC_CLINICALMENUSETTINGS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

       /// <summary>
        /// UpdateClinicalMenuSettings
       /// </summary>
       /// <param name="ds"></param>
       /// <returns></returns>
        public DSClinical UpdateClinicalMenuSettings(DSClinical ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSClinical)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CLINICALMENUSETTINGS_UPDATE, ds, ds.ClinicalMenuSettings.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalMenuSettings::UpdateClinicalMenuSettings", PROC_CLINICALMENUSETTINGS_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSModuleForm LoadForms(string FormParentHTMLId = "")
        {
            DSModuleForm ds = new DSModuleForm();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (FormParentHTMLId == "")
                    FormParentHTMLId = null;

                dbManager.AddParameters(0, PARM_FORMPARENTHTMLID, FormParentHTMLId);

                ds = (DSModuleForm)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FORM_SELECT, ds, ds.Forms.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAdmin::LoadForms", PROC_FORM_SELECT, ex);
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
