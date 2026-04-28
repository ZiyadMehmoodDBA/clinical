using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MDVision.Datasets;
using System.ComponentModel;
using System.Xml;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.DataAccess.DAL.Admin;
using System.Threading;
using System.Diagnostics;
using MDVision.Model.AuditableEvents;
using System.Data.SqlClient;
using System.IO;

namespace MDVision.DataAccess.DCommon
{
    public class DBActivityAudit
    {


        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatient"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DBActivityAudit()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }


        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DBActivityAudit(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);

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

        #region "Stored Procedure Names"

        private const string PROC_DBAUDIT_INSERT = "System.[sp_DBAuditInsert]";
        private const string PROC_DBAUDIT_SELECT = "System.[sp_DBAuditSelect]";
        private const string PROC_DBAUDIT_SELECT_FOR_MULTIUSERS = "System.sp_DBAuditSelectForMultipleUsers";
        private const string PROC_DBAUDIT_INSERT_BULK = "System.sp_DBAuditInsertBulk";
        private const string PROC_DBAUDIT_ADMIN_INSERT_BULK = "System.sp_DBAuditAdminInsertBulk";
        private const string PROC_DBAUDIT_Detail = "System.sp_DBAuditSelect_Detail";
        private const string PROC_DBAUDIT_USER = "Clinical.Sp_AuditReport";
        private const string PROC_DBAUDIT_NOTE_COMMENT_SELECT = "Patient.sp_NoteCommentsLogSelect";
        private const string PROC_LOAD_PATIENTVISIT_PRINT_INFO = "Patient.sp_PatientVisitsPrintInfo";

        private const string PROC_DBAUDIT_ROLES_LOOKUP_SELECT = "clinical.GetRoleLookup";
        private const string PROC_DBAUDIT_APPOINTMENT_INSERT_BULK = "System.sp_DBAuditAppointmentInsertBulk";

        #endregion

        #region Parameters
        private const string PARM_DB_AUDIT_ID = "@DBAuditId";
        private const string PARM_DB_TABLENAME = "@DBTableName";
        private const string PARM_PROFILE_NAME = "@ProfileName";
        private const string PARM_COLUMN_NAME = "@ColumnName";
        private const string PARM_COLUMN_KEY_ID = "@ColumnKeyId";
        private const string PARM_DISPLAY_NAME = "@DisplayName";
        private const string PARM_COLUMN_KEY_NAME = "@ColumnKeyName";
        private const string PARM_ORIGINAL_VALUE = "@OriginalValue";
        private const string PARM_CURRENT_VALUE = "@CurrentValue";
        private const string PARM_DB_AUDIT_ACTION = "@DBAuditAction";
        private const string PARM_MODULE_NAME = "@ModuleName";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_USER_IDS = "@UserIds";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PATIENT_IDS = "@PatientIds";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_CREATED_DATE = "@CreatedDate";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_PATIENT_INSURANCE_ID = "@PatientInsuranceId";
        private const string PARM_IS_CROSSED_OVER = "@IsCrossedOver";
        private const string PARM_PARENT_COLMN_KEY_ID = "@ParentKeyColumnId";
        private const string PARM_DB_ACTION_NAME = "@DBAuditAction";
        private const string PARM_CREATED_DATE_FROM = "@CreatedDateFrom";
        private const string PARM_CREATED_DATE_TO = "@CreatedDateTo";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ASSIGNED_TO = "@AssignedTo";
        private const string PARM_XML_DATA = "@Data";
        private const string PARM_USER_TYPE = "@userrole ";
        private const string PARM_DATE_FROM = "@DateFrom ";
        private const string PARM_DATE_TO = "@DateTo   ";
        private const string PARM_ACTION = "@Action";
        private const string PARM_AUDIT_LOOKUP_ID = "@LookupId";
        


        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSDBAudit ds)
        {
            dbManager.CreateParameters(19);


            dbManager.AddParameters(0, PARM_DB_AUDIT_ID, ds.DBAudit.DBAuditIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

            dbManager.AddParameters(1, PARM_DB_TABLENAME, ds.DBAudit.DBTableNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_PROFILE_NAME, ds.DBAudit.ProfileNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_COLUMN_NAME, ds.DBAudit.ColumnNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_DISPLAY_NAME, ds.DBAudit.DisplayNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_COLUMN_KEY_ID, ds.DBAudit.ColumnKeyIdColumn.ColumnName, DbType.String);

            dbManager.AddParameters(6, PARM_COLUMN_KEY_NAME, ds.DBAudit.ColumnKeyNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_ORIGINAL_VALUE, ds.DBAudit.OriginalValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CURRENT_VALUE, ds.DBAudit.CurrentValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_DB_AUDIT_ACTION, ds.DBAudit.DBAuditActionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODULE_NAME, ds.DBAudit.ModuleNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_USER_NAME, ds.DBAudit.UserNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_PATIENT_ID, ds.DBAudit.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_VISIT_ID, ds.DBAudit.VisitIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(14, PARM_ENTITY_ID, ds.DBAudit.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(15, PARM_PATIENT_INSURANCE_ID, ds.DBAudit.PatientInsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(16, PARM_IS_CROSSED_OVER, ds.DBAudit.IsCrossedOverColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(17, PARM_PARENT_COLMN_KEY_ID, ds.DBAudit.ParentKeyColumnIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(18, PARM_ASSIGNED_TO, ds.DBAudit.AssignedToColumn.ColumnName, DbType.String);
        }

        public string GetDBAudit(DataTable dt, string PrimaryKeyId, string ChargesID = null, string ParentKeyColumnId = "", bool isViewAction = false, bool isPrintAction = false, bool isDeleteAction = false, string PatientId = "", SharedVariable shareVariable = null)
        {

            DSModuleForm ds = new DSModuleForm();
            string Formname = string.Empty;
            string listType = string.Empty;
            long EntityId = 0;
            string username = string.Empty;

            if (shareVariable == null)
            {
                username = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
            }
            else
            {
                username = ClientConfiguration.DecryptFrom64(shareVariable.UserName);
                EntityId = Convert.ToInt64(shareVariable.EntityId);
            }




            if (dt.TableName.Equals("PatientCharges"))
            {
                Formname = "PatientCharges";
            }
            if (shareVariable == null)
                ds = new DALModulesForms().LoadForms(0, Formname, "", dt.TableName);
            else
                ds = new DALModulesForms(shareVariable).LoadForms(0, Formname, "", dt.TableName);

            DSDBAudit.DBAuditDataTable dtDBAudit = new DSDBAudit.DBAuditDataTable();

            //if (ds.Forms.Rows.Count >0)
            //{

            foreach (DataRow dr in ds.Forms.Rows)
            {
                //get data from db form table using ----    dt.TableName;
                //get DBAudit schema
                // string ColumnsName = "FirstName|First Name,LastName|Last Name"; /// get colunm name using tableName
                //string Action = "I,U,D";
                //string dbTableName = "Patients";
                //string FormName = "Demographic";


                string ColumnsName = dr[ds.Forms.DBTableColumnsColumn.ColumnName].ToString();
                string Action = dr[ds.Forms.DBActionColumn.ColumnName].ToString();
                string dbTableName = dr[ds.Forms.DBTableNameColumn.ColumnName].ToString();
                string FormName = dr[ds.Forms.NameColumn.ColumnName].ToString();
        
                //Start 14-05-2016 Humaira Yousaf for specific form name
                string ModuleName = dr[ds.Forms.DescriptionColumn.ColumnName].ToString();
                if (string.IsNullOrEmpty(ModuleName))
                {
                    ModuleName = dr[ds.Forms.ModuleNameColumn.ColumnName].ToString();
                }
                //End 14-05-2016 Humaira Yousaf for specific form name
                Dictionary<string, string> ColumnsDictionary = GetColumnsName(ColumnsName);
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (listType == string.Empty)
                    {
                        if (dt.Columns.Contains("ListType") && (row["ListType"].ToString() == "FamilyHistory" || row["ListType"].ToString() == "MedicalHistory" || row["ListType"].ToString() == "SurgicalHistory"))
                        {
                            listType = row["ListType"].ToString();
                            if (listType == "FamilyHistory")
                            {
                                FormName = "Favorites_FamilyHistory";
                                ModuleName = "Favorites_FamilyHistory";
                            }
                            else if (listType == "MedicalHistory")
                            {
                                FormName = "Favorites_MedicalHistory";
                                ModuleName = "Favorites_MedicalHistory";
                            }
                            else
                            {
                                FormName = "Favorites_SurgicalHistory";
                                ModuleName = "Favorites_SurgicalHistory";
                            }

                        }
                    }

                    if ((row.RowState == DataRowState.Added))
                    {
                        if (Action.Contains('I'))
                        {
                            string isFirstRow = "";

                            foreach (DataColumn col in row.Table.Columns)
                            {
                                if (ColumnsDictionary.ContainsKey(col.ColumnName))
                                {
                                    //Edited by Azeem Raza Tayyab on 04-Feb-2016 to fix Bug# PMS-3656 // added  && row[col.ColumnName].ToString() != ""
                                    if (isFirstRow == "" && row[col.ColumnName].ToString() != "")
                                    {
                                        DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;

                                        if (columnsKey.Length > 0)
                                        {
                                            //Start 25-04-2016 Humaira Yousaf to get primary keys
                                            if (dt.Columns.Contains("PrimaryKey") == true)
                                            {
                                                drActivity.ColumnKeyId = row["PrimaryKey"].ToString();
                                            }
                                            else
                                            {
                                                drActivity.ColumnKeyId = PrimaryKeyId;
                                            }
                                            //End 25-04-2016 Humaira Yousaf to get primary keys
                                            drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                        }
                                        if (FormName == "PlanofCare_Goal" || Formname == "PatientCharges" || FormName == "Medical_Procedures" || FormName == "History_Medical Hx" || FormName == "History_Medical Hx_Disease")
                                        {
                                            if (Formname == "PatientCharges")
                                            {
                                                if (PrimaryKeyId.Split(',').Length > i)
                                                {
                                                    drActivity.ColumnKeyId = PrimaryKeyId.Split(',')[i];
                                                }
                                                else
                                                {
                                                    drActivity.ColumnKeyId = PrimaryKeyId;
                                                }
                                            }
                                            else
                                            {
                                                drActivity.ColumnKeyId = PrimaryKeyId;
                                            }

                                        }
                                        // drActivity["ColumnDataType"]    =  col.DataType;
                                        drActivity.OriginalValue = "";
                                        drActivity.CurrentValue = row[col.ColumnName].ToString();
                                        drActivity.ProfileName = FormName;
                                        drActivity.DBTableName = dbTableName;
                                        drActivity.ColumnName = col.ColumnName;
                                        if (FormName == "PatientVisitICD")
                                        {
                                            drActivity.DisplayName = row["ICDType"].ToString() + " - " + ColumnsDictionary[col.ColumnName];
                                        }
                                        else if (FormName == "Demographic" && ColumnsDictionary[col.ColumnName] == "Active")
                                        {
                                            drActivity.DisplayName = "Patient Status";
                                        }
                                        else
                                        {
                                            drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                        }
                                        drActivity.DBAuditAction = "Insert";
                                        drActivity.ModuleName = ModuleName;

                                        drActivity.UserName = username;


                                        if (PatientId != "")
                                            drActivity.PatientId = PatientId;
                                        if (row.Table.Columns.Contains("PatientId"))
                                        {
                                            string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                            if (Convert.ToDouble(patienID) > 0)
                                                drActivity.PatientId = row["PatientId"].ToString();
                                        }
                                        if (row.Table.Columns.Contains("VisitId"))
                                        {
                                            string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                                            if (Convert.ToDouble(VisitID) > 0)
                                                drActivity.VisitId = row["VisitId"].ToString();
                                        }
                                        drActivity.EntityId = EntityId;
                                        if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                        {
                                            drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                        }
                                        dtDBAudit.Rows.Add(drActivity);
                                        //isFirstRow = "1";
                                    }
                                }
                            }
                        }

                    }
                    else if ((row.RowState == DataRowState.Modified))
                    {
                        if (Action.Contains('U'))
                        {
                            //Start 26-10-2016 Humaira Yousaf to add view action log for DrFirst against each item
                            if (dt.Columns.Contains("PrimaryKey") && dt.Columns.Contains("ViewAction") && row["ViewAction"].ToString() == "View")
                            {

                                DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();
                                DataColumn[] columnsKey;
                                columnsKey = row.Table.PrimaryKey;

                                if (columnsKey.Length > 0)
                                {
                                    drActivity.ColumnKeyId = row["PrimaryKey"].ToString();
                                    drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                }
                                drActivity.OriginalValue = "";
                                drActivity.CurrentValue = "";
                                drActivity.ProfileName = FormName;
                                drActivity.DBTableName = dbTableName;
                                drActivity.ColumnName = columnsKey[0].ColumnName;
                                drActivity.DisplayName = "";
                                drActivity.DBAuditAction = "View";
                                drActivity.ModuleName = ModuleName;

                                drActivity.UserName = username;


                                if (PatientId != "")
                                    drActivity.PatientId = PatientId;

                                if (row.Table.Columns.Contains("PatientId"))
                                {
                                    string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                    if (Convert.ToDouble(patienID) > 0)
                                        drActivity.PatientId = row["PatientId"].ToString();
                                }
                                drActivity.EntityId = EntityId;
                                if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                {
                                    drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                }
                                dtDBAudit.Rows.Add(drActivity);

                            }
                            //End 26-10-2016 Humaira Yousaf to add view action log for DrFirst against each item
                            else
                            {
                                foreach (DataColumn col in row.Table.Columns)
                                {
                                    // removed byte[] conversion---MA-228
                                    //if ((ColumnsDictionary.ContainsKey(col.ColumnName) && !row.IsNull(col.ColumnName) && row[col.ColumnName] != null && !row[col.ColumnName].ToString().Trim().Equals(row[col.ColumnName, DataRowVersion.Original].ToString().Trim())) || (ColumnsDictionary.ContainsKey(col.ColumnName) && !row.IsNull(col.ColumnName) && row[col.ColumnName] != null && (col.ColumnName == "PatientProfileImagePath" || col.ColumnName == "PatientProfileThumbnailPath") && Convert.ToBase64String((byte[])(row[col.ColumnName, DataRowVersion.Original])) != Convert.ToBase64String((byte[])(row[col.ColumnName]))))
                                    if ((ColumnsDictionary.ContainsKey(col.ColumnName) && !row.IsNull(col.ColumnName) && row[col.ColumnName] != null && !row[col.ColumnName].ToString().Trim().Equals(row[col.ColumnName, DataRowVersion.Original].ToString().Trim())) || (ColumnsDictionary.ContainsKey(col.ColumnName) && !row.IsNull(col.ColumnName) && row[col.ColumnName] != null && (col.ColumnName == "PatientProfileImagePath" || col.ColumnName == "PatientProfileThumbnailPath") && (row[col.ColumnName, DataRowVersion.Original]) != row[col.ColumnName]))
                                    {
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;
                                        DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();


                                        if (columnsKey.Length > 0)
                                        {
                                            drActivity.ColumnKeyId = row[columnsKey[0].ColumnName, DataRowVersion.Original].ToString();
                                            drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                        }
                                        if (FormName == "PlanofCare_Goal" || Formname == "PatientCharges" || FormName == "Medical_Procedures" || FormName == "History_Medical Hx" || FormName == "History_Medical Hx_Disease")
                                        {
                                            if (Formname == "PatientCharges")
                                            {
                                                if (PrimaryKeyId.Split(',').Length > i)
                                                {
                                                    drActivity.ColumnKeyId = PrimaryKeyId.Split(',')[i];
                                                }
                                                else
                                                {
                                                    drActivity.ColumnKeyId = PrimaryKeyId;
                                                }
                                            }
                                            else
                                            {
                                                drActivity.ColumnKeyId = PrimaryKeyId;
                                            }

                                        }
                                        if ((col.ColumnName == "PatientProfileImagePath" || col.ColumnName == "PatientProfileThumbnailPath"))
                                        {
                                            if (!string.IsNullOrEmpty(row[col.ColumnName, DataRowVersion.Original].ToString()))
                                            {
                                                drActivity.OriginalValue = (row[col.ColumnName, DataRowVersion.Original]).ToString();
                                                //drActivity.OriginalValue = Convert.ToBase64String((byte[])(row[col.ColumnName, DataRowVersion.Original])).ToString();
                                            }
                                            else
                                            {
                                                drActivity.OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                            }
                                            if (!string.IsNullOrEmpty(row[col.ColumnName].ToString()))
                                            {
                                                //drActivity.CurrentValue = Convert.ToBase64String((byte[])(row[col.ColumnName])).ToString();
                                                drActivity.CurrentValue = row[col.ColumnName].ToString();
                                            }
                                            else
                                            {
                                                drActivity.CurrentValue = row[col.ColumnName].ToString();
                                            }
                                        }
                                        else
                                        {
                                            drActivity.OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                            drActivity.CurrentValue = row[col.ColumnName].ToString();
                                        }
                                        //drActivity.OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                        //drActivity.CurrentValue = row[col.ColumnName].ToString();
                                        drActivity.ProfileName = FormName;
                                        drActivity.DBTableName = dbTableName;
                                        drActivity.ColumnName = col.ColumnName;
                                        if (FormName == "PatientVisitICD")
                                        {
                                            drActivity.DisplayName = row["ICDType"].ToString() + " - " + ColumnsDictionary[col.ColumnName];
                                            drActivity.ColumnKeyId = PrimaryKeyId;
                                        }
                                        else if (FormName == "Demographic" && ColumnsDictionary[col.ColumnName] == "Active")
                                        {
                                            drActivity.DisplayName = "Patient Status";
                                        }
                                        else
                                        {
                                            drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                        }
                                        //Begin 15-07-2016 Edit By Humaira Yousaf Bug#1573
                                        if (string.Equals(FormName, "Medical_Vitals") && !string.IsNullOrEmpty(row["DeleteComments"].ToString()))
                                        {
                                            drActivity.DBAuditAction = "Delete";
                                        }
                                        else
                                        {
                                            drActivity.DBAuditAction = "Update";
                                        }
                                        //End 15-07-2016 Edit By Humaira Yousaf Bug#1573
                                        drActivity.ModuleName = ModuleName;
                                        drActivity.UserName = username;
                                        if (PatientId != "")
                                            drActivity.PatientId = PatientId;

                                        if (row.Table.Columns.Contains("PatientId"))
                                        {
                                            string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                            if (Convert.ToDouble(patienID) > 0)
                                                drActivity.PatientId = row["PatientId"].ToString();
                                        }
                                        if (row.Table.Columns.Contains("VisitId"))
                                        {
                                            string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                                            if (Convert.ToDouble(VisitID) > 0)
                                                drActivity.VisitId = row["VisitId"].ToString();
                                        }
                                        drActivity.EntityId = EntityId;
                                        if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                        {
                                            drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                        }
                                        dtDBAudit.Rows.Add(drActivity);
                                    }

                                }
                            }

                        }

                    }
                    else if ((row.RowState == DataRowState.Deleted) || !string.IsNullOrEmpty(ChargesID) || isDeleteAction == true)
                    {
                        if (Action.Contains('D'))
                        {
                            foreach (DataColumn col in row.Table.Columns)
                            {
                                if (!string.IsNullOrEmpty(ChargesID))
                                {
                                    if ((ColumnsDictionary.ContainsKey(col.ColumnName) && string.IsNullOrEmpty(ChargesID)) ||
                                    (ColumnsDictionary.ContainsKey(col.ColumnName) && col.ColumnName == "CPTCode") ||
                                    (col.ColumnName == "CPTCode" && !string.IsNullOrEmpty(ChargesID)))
                                    {
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;
                                        DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();
                                        if (columnsKey.Length > 0)
                                        {
                                            drActivity.ColumnKeyId = row[columnsKey[0].ColumnName, DataRowVersion.Original].ToString();
                                            drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                        }
                                        if (FormName == "PlanofCare_Goal" || Formname == "PatientCharges" || FormName == "Medical_Procedures" || FormName == "History_Medical Hx" || FormName == "History_Medical Hx_Disease")
                                        {
                                            drActivity.ColumnKeyId = PrimaryKeyId;
                                        }
                                        string currentValue = "";
                                        string OriginalValue = "";
                                        if (dt.TableName.Equals("PatientCharges"))
                                        {
                                            currentValue = "Deleted";
                                            OriginalValue = dt.Rows[0][4].ToString();
                                        }
                                        else
                                        {
                                            OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                        }
                                        drActivity.OriginalValue = OriginalValue;
                                        drActivity.CurrentValue = currentValue;
                                        drActivity.ProfileName = FormName;
                                        drActivity.DBTableName = dbTableName;
                                        if (FormName == "PatientVisitICD")
                                        {
                                            drActivity.DisplayName = row["ICDType"].ToString() + " - " + ColumnsDictionary[col.ColumnName];
                                        }
                                        else
                                        {
                                            drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                        }
                                        drActivity.ColumnName = col.ColumnName;
                                        drActivity.DBAuditAction = "Delete";
                                        drActivity.ModuleName = ModuleName;
                                        drActivity.UserName = username;
                                        if (PatientId != "")
                                            drActivity.PatientId = PatientId;

                                        if (row.Table.Columns.Contains("PatientId"))
                                        {
                                            string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                            if (Convert.ToDouble(patienID) > 0)
                                                drActivity.PatientId = row["PatientId"].ToString();
                                        }
                                        if (row.Table.Columns.Contains("VisitId"))
                                        {
                                            string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                                            if (Convert.ToDouble(VisitID) > 0)
                                                drActivity.VisitId = row["VisitId"].ToString();
                                        }
                                        drActivity.EntityId = EntityId;
                                        if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                        {
                                            drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                        }
                                        dtDBAudit.Rows.Add(drActivity);

                                    }
                                }
                                else if (isDeleteAction == true)
                                {
                                    if (ColumnsDictionary.ContainsKey(col.ColumnName) && row[col.ColumnName].ToString().Equals(row[col.ColumnName, DataRowVersion.Original].ToString()))
                                    {
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;
                                        DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();


                                        if (columnsKey.Length > 0)
                                        {
                                            drActivity.ColumnKeyId = row[columnsKey[0].ColumnName, DataRowVersion.Original].ToString();
                                            drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                        }
                                        if (FormName == "PlanofCare_Goal" || Formname == "PatientCharges" || FormName == "Medical_Procedures" || FormName == "History_Medical Hx" || FormName == "History_Medical Hx_Disease")
                                        {
                                            drActivity.ColumnKeyId = PrimaryKeyId;
                                        }
                                        drActivity.OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                        drActivity.CurrentValue = "";//row[col.ColumnName].ToString();
                                        drActivity.ProfileName = FormName;
                                        drActivity.DBTableName = dbTableName;
                                        drActivity.ColumnName = col.ColumnName;
                                        if (FormName == "PatientVisitICD")
                                        {
                                            drActivity.DisplayName = row["ICDType"].ToString() + " - " + ColumnsDictionary[col.ColumnName];
                                        }
                                        else
                                        {
                                            drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                        }
                                        drActivity.DBAuditAction = "Delete";
                                        drActivity.ModuleName = ModuleName;
                                        drActivity.UserName = username;
                                        if (PatientId != "")
                                            drActivity.PatientId = PatientId;

                                        if (row.Table.Columns.Contains("PatientId"))
                                        {
                                            string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                            if (Convert.ToDouble(patienID) > 0)
                                                drActivity.PatientId = row["PatientId"].ToString();
                                        }
                                        else if (PatientId != "")
                                        {
                                            if (Convert.ToDouble(PatientId) > 0)
                                                drActivity.PatientId = PatientId;
                                        }
                                        if (row.Table.Columns.Contains("VisitId"))
                                        {
                                            string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                                            if (Convert.ToDouble(VisitID) > 0)
                                                drActivity.VisitId = row["VisitId"].ToString();
                                        }
                                        drActivity.EntityId = EntityId;
                                        if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                        {
                                            drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                        }
                                        dtDBAudit.Rows.Add(drActivity);
                                    }
                                }

                            }

                        }

                    }
                    else if ((row.RowState == DataRowState.Unchanged))
                    {
                        string currentDBAuditAction = "";
                        if (isViewAction && Action.Contains("V"))
                        {
                            currentDBAuditAction = "View";
                        }
                        else if (isPrintAction && Action.Contains("P"))
                        {
                            currentDBAuditAction = "Print";
                        }
                        if (currentDBAuditAction != "")
                        {
                            //Start 24-05-2016 Edit By Humaira Yousaf Bug# EMR-1174
                            if (ModuleName == "FaceSheet" && dtDBAudit.Rows.Count > 0)
                            {
                                break;
                            }
                            //End 24-05-2016 Edit By Humaira Yousaf Bug# EMR-1174
                            DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();
                            DataColumn[] columnsKey;
                            columnsKey = row.Table.PrimaryKey;

                            if (columnsKey.Length > 0)
                            {
                                //Start 25-04-2016 Humaira Yousaf to get primary keys
                                if (dt.Columns.Contains("PrimaryKey") == true)
                                {
                                    drActivity.ColumnKeyId = row["PrimaryKey"].ToString();
                                }
                                else
                                {
                                    drActivity.ColumnKeyId = PrimaryKeyId;
                                }
                                //End 25-04-2016 Humaira Yousaf to get primary keys
                                drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                            }
                            if (FormName == "PlanofCare_Goal" || Formname == "PatientCharges" || FormName == "Medical_Procedures" || FormName == "History_Medical Hx" || FormName == "History_Medical Hx_Disease")
                            {
                                drActivity.ColumnKeyId = PrimaryKeyId;
                            }
                            // drActivity["ColumnDataType"]    =  col.DataType;
                            drActivity.OriginalValue = "";
                            drActivity.CurrentValue = "";
                            drActivity.ProfileName = FormName;
                            drActivity.DBTableName = dbTableName;
                            //drActivity.ColumnName = columnsKey[0].ColumnName;
                            drActivity.DisplayName = ""; //ColumnsDictionary[columnsKey[0].ColumnName];
                            drActivity.DBAuditAction = currentDBAuditAction;
                            drActivity.ModuleName = ModuleName;
                            drActivity.UserName = username;
                            if (PatientId != "")
                                drActivity.PatientId = PatientId;

                            if (row.Table.Columns.Contains("PatientId"))
                            {
                                string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                if (Convert.ToDouble(patienID) > 0)
                                    drActivity.PatientId = row["PatientId"].ToString();
                            }
                            if (row.Table.Columns.Contains("VisitId"))
                            {
                                string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                                if (Convert.ToDouble(VisitID) > 0)
                                    drActivity.VisitId = row["VisitId"].ToString();
                            }
                            drActivity.EntityId = EntityId;
                            if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                            {
                                drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                            }
                            dtDBAudit.Rows.Add(drActivity);
                        }
                    }
                    i++;
                }
                if ((listType == "FamilyHistory" || listType == "MedicalHistory" || listType == "SurgicalHistory") && dtDBAudit.Rows.Count > 0)
                {
                    break;
                }
            }
            string xmlDBAudit = "";
            #region Create XML of Chnages for Audit Log
            /******Create XML of the changes for Audit Log ************/
            /*****Added By Azeem Raza Tayyab on 25-May-2016 ***********/
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("DBAudits");
            xml.AppendChild(root);
            foreach (DataRow row in dtDBAudit.Rows)
            {
                XmlElement child = xml.CreateElement("DBAudit");
                root.AppendChild(child);
                for (int col = 0; col < row.ItemArray.Count(); col++)
                {
                    XmlElement colElement = xml.CreateElement(row.Table.Columns[col].ToString());
                    colElement.InnerText = row.ItemArray[col].ToString();
                    child.AppendChild(colElement);
                }
            }
            if (dtDBAudit.Rows.Count > 0)
            {
                xmlDBAudit = xml.OuterXml;
            }
            #endregion
            return xmlDBAudit;
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="dt"></param>
        /// <param name="PrimaryKeyId"></param>
        /// <param name="ChargesID"></param>
        /// <param name="ParentKeyColumnId"></param>
        /// <param name="isViewAction"></param>
        /// <param name="isPrintAction"></param>
        /// <param name="isDeleteAction"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public string GetDBAudit(SharedVariable SharedVariable, DataTable dt, string PrimaryKeyId, string ChargesID = null, string ParentKeyColumnId = "", bool isViewAction = false, bool isPrintAction = false, bool isDeleteAction = false, string PatientId = "")
        {

            DSModuleForm ds = new DSModuleForm();
            string Formname = string.Empty;
            string listType = string.Empty;
            if (dt.TableName.Equals("PatientCharges"))
            {
                Formname = "PatientCharges";
            }
            ds = new DALModulesForms(SharedVariable).LoadForms(SharedVariable, 0, Formname, "", dt.TableName);


            DSDBAudit.DBAuditDataTable dtDBAudit = new DSDBAudit.DBAuditDataTable();

            //if (ds.Forms.Rows.Count >0)
            //{

            foreach (DataRow dr in ds.Forms.Rows)
            {
                //get data from db form table using ----    dt.TableName;
                //get DBAudit schema
                // string ColumnsName = "FirstName|First Name,LastName|Last Name"; /// get colunm name using tableName
                //string Action = "I,U,D";
                //string dbTableName = "Patients";
                //string FormName = "Demographic";

                string ColumnsName = dr[ds.Forms.DBTableColumnsColumn.ColumnName].ToString();
                string Action = dr[ds.Forms.DBActionColumn.ColumnName].ToString();
                string dbTableName = dr[ds.Forms.DBTableNameColumn.ColumnName].ToString();
                string FormName = dr[ds.Forms.NameColumn.ColumnName].ToString();
                //Start 14-05-2016 Humaira Yousaf for specific form name
                string ModuleName = dr[ds.Forms.DescriptionColumn.ColumnName].ToString();
                if (string.IsNullOrEmpty(ModuleName))
                {
                    ModuleName = dr[ds.Forms.ModuleNameColumn.ColumnName].ToString();
                }
                //End 14-05-2016 Humaira Yousaf for specific form name
                Dictionary<string, string> ColumnsDictionary = GetColumnsName(ColumnsName);

                foreach (DataRow row in dt.Rows)
                {
                    if (listType == string.Empty)
                    {
                        if (dt.Columns.Contains("ListType") && (row["ListType"].ToString() == "FamilyHistory" || row["ListType"].ToString() == "MedicalHistory" || row["ListType"].ToString() == "SurgicalHistory"))
                        {
                            listType = row["ListType"].ToString();
                            if (listType == "FamilyHistory")
                            {
                                FormName = "Favorites_FamilyHistory";
                                ModuleName = "Favorites_FamilyHistory";
                            }
                            else if (listType == "MedicalHistory")
                            {
                                FormName = "Favorites_MedicalHistory";
                                ModuleName = "Favorites_MedicalHistory";
                            }
                            else
                            {
                                FormName = "Favorites_SurgicalHistory";
                                ModuleName = "Favorites_SurgicalHistory";
                            }

                        }
                    }

                    if ((row.RowState == DataRowState.Added))
                    {
                        if (Action.Contains('I'))
                        {
                            string isFirstRow = "";
                            foreach (DataColumn col in row.Table.Columns)
                            {
                                if (ColumnsDictionary.ContainsKey(col.ColumnName))
                                {
                                    //Edited by Azeem Raza Tayyab on 04-Feb-2016 to fix Bug# PMS-3656 // added  && row[col.ColumnName].ToString() != ""
                                    if (isFirstRow == "" && row[col.ColumnName].ToString() != "")
                                    {
                                        DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;

                                        if (columnsKey.Length > 0)
                                        {
                                            //Start 25-04-2016 Humaira Yousaf to get primary keys
                                            if (dt.Columns.Contains("PrimaryKey") == true)
                                            {
                                                drActivity.ColumnKeyId = row["PrimaryKey"].ToString();
                                            }
                                            else
                                            {
                                                drActivity.ColumnKeyId = PrimaryKeyId;
                                            }
                                            //End 25-04-2016 Humaira Yousaf to get primary keys
                                            drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                        }
                                        if (FormName == "PlanofCare_Goal" || Formname == "PatientCharges")
                                        {
                                            drActivity.ColumnKeyId = PrimaryKeyId;
                                        }
                                        // drActivity["ColumnDataType"]    =  col.DataType;
                                        drActivity.OriginalValue = "";
                                        drActivity.CurrentValue = row[col.ColumnName].ToString();
                                        drActivity.ProfileName = FormName;
                                        drActivity.DBTableName = dbTableName;
                                        drActivity.ColumnName = col.ColumnName;
                                        if (FormName == "PatientVisitICD")
                                        {
                                            drActivity.DisplayName = row["ICDType"].ToString() + " - " + ColumnsDictionary[col.ColumnName];
                                        }
                                        else
                                        {
                                            drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                        }
                                        drActivity.DBAuditAction = "Insert";
                                        drActivity.ModuleName = ModuleName;
                                        drActivity.UserName = ClientConfiguration.DecryptFrom64(SharedVariable.UserName);
                                        if (PatientId != "")
                                            drActivity.PatientId = PatientId;
                                        if (row.Table.Columns.Contains("PatientId"))
                                        {
                                            string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                            if (Convert.ToDouble(patienID) > 0)
                                                drActivity.PatientId = row["PatientId"].ToString();
                                        }
                                        if (row.Table.Columns.Contains("VisitId"))
                                        {
                                            string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                                            if (Convert.ToDouble(VisitID) > 0)
                                                drActivity.VisitId = row["VisitId"].ToString();
                                        }
                                        drActivity.EntityId = Convert.ToInt64(SharedVariable.EntityId);
                                        if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                        {
                                            drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                        }
                                        dtDBAudit.Rows.Add(drActivity);
                                        //isFirstRow = "1";
                                    }
                                }
                            }
                        }

                    }
                    else if ((row.RowState == DataRowState.Modified))
                    {
                        if (Action.Contains('U'))
                        {
                            foreach (DataColumn col in row.Table.Columns)
                            {
                                if (ColumnsDictionary.ContainsKey(col.ColumnName) && !row[col.ColumnName].ToString().Equals(row[col.ColumnName, DataRowVersion.Original].ToString()))
                                {
                                    DataColumn[] columnsKey;
                                    columnsKey = row.Table.PrimaryKey;
                                    DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();


                                    if (columnsKey.Length > 0)
                                    {
                                        drActivity.ColumnKeyId = row[columnsKey[0].ColumnName, DataRowVersion.Original].ToString();
                                        drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                    }
                                    if (FormName == "PlanofCare_Goal" || Formname == "PatientCharges")
                                    {
                                        drActivity.ColumnKeyId = PrimaryKeyId;
                                    }
                                    if ((col.ColumnName == "PatientImage" || col.ColumnName == "PatientImageThumbnail"))
                                    {
                                        if (!string.IsNullOrEmpty(row[col.ColumnName, DataRowVersion.Original].ToString()))
                                        {
                                            drActivity.OriginalValue = Convert.ToBase64String((byte[])(row[col.ColumnName, DataRowVersion.Original])).ToString();
                                        }
                                        else
                                        {
                                            drActivity.OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                        }
                                        if (!string.IsNullOrEmpty(row[col.ColumnName].ToString()))
                                        {
                                            drActivity.CurrentValue = Convert.ToBase64String((byte[])(row[col.ColumnName])).ToString();
                                        }
                                        else
                                        {
                                            drActivity.CurrentValue = row[col.ColumnName].ToString();
                                        }
                                    }
                                    else
                                    {
                                        drActivity.OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                        drActivity.CurrentValue = row[col.ColumnName].ToString();
                                    }
                                    if (row.Table.Columns.Contains("PatientId"))
                                    {
                                        string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                        if (Convert.ToDouble(patienID) > 0)
                                            drActivity.PatientId = row["PatientId"].ToString();
                                    }

                                    drActivity.ProfileName = FormName;
                                    drActivity.DBTableName = dbTableName;
                                    drActivity.ColumnName = col.ColumnName;
                                    if (FormName == "PatientVisitICD")
                                    {
                                        drActivity.DisplayName = row["ICDType"].ToString() + " - " + ColumnsDictionary[col.ColumnName];
                                    }
                                    else
                                    {
                                        drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                    }
                                    //Begin 15-07-2016 Edit By Humaira Yousaf Bug#1573
                                    if (string.Equals(FormName, "Medical_Vitals") && !string.IsNullOrEmpty(row["DeleteComments"].ToString()))
                                    {
                                        drActivity.DBAuditAction = "Delete";
                                    }
                                    else
                                    {
                                        drActivity.DBAuditAction = "Update";
                                    }
                                    //End 15-07-2016 Edit By Humaira Yousaf Bug#1573
                                    drActivity.ModuleName = ModuleName;
                                    drActivity.UserName = ClientConfiguration.DecryptFrom64(SharedVariable.UserName);
                                    if (PatientId != "")
                                        drActivity.PatientId = PatientId;

                                    if (row.Table.Columns.Contains("PatientId"))
                                    {
                                        string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                        if (Convert.ToDouble(patienID) > 0)
                                            drActivity.PatientId = row["PatientId"].ToString();
                                    }
                                    if (row.Table.Columns.Contains("VisitId"))
                                    {
                                        string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                                        if (Convert.ToDouble(VisitID) > 0)
                                            drActivity.VisitId = row["VisitId"].ToString();
                                    }
                                    drActivity.EntityId = Convert.ToInt64(SharedVariable.EntityId);
                                    if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                    {
                                        drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                    }
                                    dtDBAudit.Rows.Add(drActivity);
                                }

                            }

                        }

                    }
                    else if ((row.RowState == DataRowState.Deleted) || !string.IsNullOrEmpty(ChargesID) || isDeleteAction == true)
                    {
                        if (Action.Contains('D'))
                        {
                            foreach (DataColumn col in row.Table.Columns)
                            {
                                if (!string.IsNullOrEmpty(ChargesID))
                                {
                                    if ((ColumnsDictionary.ContainsKey(col.ColumnName) && string.IsNullOrEmpty(ChargesID)) ||
                                    (ColumnsDictionary.ContainsKey(col.ColumnName) && col.ColumnName == "CPTCode") ||
                                    (col.ColumnName == "CPTCode" && !string.IsNullOrEmpty(ChargesID)))
                                    {
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;
                                        DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();
                                        if (columnsKey.Length > 0)
                                        {
                                            drActivity.ColumnKeyId = row[columnsKey[0].ColumnName, DataRowVersion.Original].ToString();
                                            drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                        }
                                        if (FormName == "PlanofCare_Goal" || Formname == "PatientCharges")
                                        {
                                            drActivity.ColumnKeyId = PrimaryKeyId;
                                        }
                                        string currentValue = "";
                                        string OriginalValue = "";
                                        if (dt.TableName.Equals("PatientCharges"))
                                        {
                                            currentValue = "Deleted";
                                            OriginalValue = dt.Rows[0][4].ToString();
                                        }
                                        else
                                        {
                                            OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                        }
                                        drActivity.OriginalValue = OriginalValue;
                                        drActivity.CurrentValue = currentValue;
                                        drActivity.ProfileName = FormName;
                                        drActivity.DBTableName = dbTableName;
                                        if (FormName == "PatientVisitICD")
                                        {
                                            drActivity.DisplayName = row["ICDType"].ToString() + " - " + ColumnsDictionary[col.ColumnName];
                                        }
                                        else
                                        {
                                            drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                        }
                                        drActivity.ColumnName = col.ColumnName;
                                        drActivity.DBAuditAction = "Delete";
                                        drActivity.ModuleName = ModuleName;
                                        drActivity.UserName = ClientConfiguration.DecryptFrom64(SharedVariable.UserName);
                                        if (PatientId != "")
                                            drActivity.PatientId = PatientId;

                                        if (row.Table.Columns.Contains("PatientId"))
                                        {
                                            string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                            if (Convert.ToDouble(patienID) > 0)
                                                drActivity.PatientId = row["PatientId"].ToString();
                                        }
                                        if (row.Table.Columns.Contains("VisitId"))
                                        {
                                            string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                                            if (Convert.ToDouble(VisitID) > 0)
                                                drActivity.VisitId = row["VisitId"].ToString();
                                        }
                                        drActivity.EntityId = Convert.ToInt64(SharedVariable.EntityId);
                                        if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                        {
                                            drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                        }
                                        dtDBAudit.Rows.Add(drActivity);

                                    }
                                }
                                else if (isDeleteAction == true)
                                {
                                    if (ColumnsDictionary.ContainsKey(col.ColumnName) && row[col.ColumnName].ToString().Equals(row[col.ColumnName, DataRowVersion.Original].ToString()))
                                    {
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;
                                        DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();


                                        if (columnsKey.Length > 0)
                                        {
                                            drActivity.ColumnKeyId = row[columnsKey[0].ColumnName, DataRowVersion.Original].ToString();
                                            drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                        }
                                        if (FormName == "PlanofCare_Goal" || Formname == "PatientCharges")
                                        {
                                            drActivity.ColumnKeyId = PrimaryKeyId;
                                        }
                                        drActivity.OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                        drActivity.CurrentValue = "";//row[col.ColumnName].ToString();
                                        drActivity.ProfileName = FormName;
                                        drActivity.DBTableName = dbTableName;
                                        drActivity.ColumnName = col.ColumnName;
                                        if (FormName == "PatientVisitICD")
                                        {
                                            drActivity.DisplayName = row["ICDType"].ToString() + " - " + ColumnsDictionary[col.ColumnName];
                                        }
                                        else
                                        {
                                            drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                        }
                                        drActivity.DBAuditAction = "Delete";
                                        drActivity.ModuleName = ModuleName;
                                        drActivity.UserName = ClientConfiguration.DecryptFrom64(SharedVariable.UserName);
                                        if (PatientId != "")
                                            drActivity.PatientId = PatientId;

                                        if (row.Table.Columns.Contains("PatientId"))
                                        {
                                            string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                            if (Convert.ToDouble(patienID) > 0)
                                                drActivity.PatientId = row["PatientId"].ToString();
                                        }
                                        else if (PatientId != "")
                                        {
                                            if (Convert.ToDouble(PatientId) > 0)
                                                drActivity.PatientId = PatientId;
                                        }
                                        if (row.Table.Columns.Contains("VisitId"))
                                        {
                                            string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                                            if (Convert.ToDouble(VisitID) > 0)
                                                drActivity.VisitId = row["VisitId"].ToString();
                                        }
                                        drActivity.EntityId = Convert.ToInt64(SharedVariable.EntityId);
                                        if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                        {
                                            drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                        }
                                        dtDBAudit.Rows.Add(drActivity);
                                    }
                                }

                            }

                        }

                    }
                    else if ((row.RowState == DataRowState.Unchanged))
                    {
                        string currentDBAuditAction = "";
                        if (isViewAction && Action.Contains("V"))
                        {
                            currentDBAuditAction = "View";
                        }
                        else if (isPrintAction && Action.Contains("P"))
                        {
                            currentDBAuditAction = "Print";
                        }
                        if (currentDBAuditAction != "")
                        {
                            //Start 24-05-2016 Edit By Humaira Yousaf Bug# EMR-1174
                            if (ModuleName == "FaceSheet" && dtDBAudit.Rows.Count > 0)
                            {
                                break;
                            }
                            //End 24-05-2016 Edit By Humaira Yousaf Bug# EMR-1174
                            DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();
                            DataColumn[] columnsKey;
                            columnsKey = row.Table.PrimaryKey;

                            if (columnsKey.Length > 0)
                            {
                                //Start 25-04-2016 Humaira Yousaf to get primary keys
                                if (dt.Columns.Contains("PrimaryKey") == true)
                                {
                                    drActivity.ColumnKeyId = row["PrimaryKey"].ToString();
                                }
                                else
                                {
                                    drActivity.ColumnKeyId = PrimaryKeyId;
                                }
                                //End 25-04-2016 Humaira Yousaf to get primary keys
                                drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                            }
                            if (FormName == "PlanofCare_Goal" || Formname == "PatientCharges")
                            {
                                drActivity.ColumnKeyId = PrimaryKeyId;
                            }
                            // drActivity["ColumnDataType"]    =  col.DataType;
                            drActivity.OriginalValue = "";
                            drActivity.CurrentValue = "";
                            drActivity.ProfileName = FormName;
                            drActivity.DBTableName = dbTableName;
                            drActivity.ColumnName = columnsKey[0].ColumnName;
                            drActivity.DisplayName = ""; //ColumnsDictionary[columnsKey[0].ColumnName];
                            drActivity.DBAuditAction = currentDBAuditAction;
                            drActivity.ModuleName = ModuleName;
                            drActivity.UserName = ClientConfiguration.DecryptFrom64(SharedVariable.UserName);
                            if (PatientId != "")
                                drActivity.PatientId = PatientId;

                            if (row.Table.Columns.Contains("PatientId"))
                            {
                                string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                if (Convert.ToDouble(patienID) > 0)
                                    drActivity.PatientId = row["PatientId"].ToString();
                            }
                            if (row.Table.Columns.Contains("VisitId"))
                            {
                                string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                                if (Convert.ToDouble(VisitID) > 0)
                                    drActivity.VisitId = row["VisitId"].ToString();
                            }
                            drActivity.EntityId = Convert.ToInt64(SharedVariable.EntityId);
                            if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                            {
                                drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                            }
                            dtDBAudit.Rows.Add(drActivity);
                        }
                    }
                }
                if ((listType == "FamilyHistory" || listType == "MedicalHistory" || listType == "SurgicalHistory") && dtDBAudit.Rows.Count > 0)
                {
                    break;
                }
            }
            string xmlDBAudit = "";
            #region Create XML of Chnages for Audit Log
            /******Create XML of the changes for Audit Log ************/
            /*****Added By Azeem Raza Tayyab on 25-May-2016 ***********/
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("DBAudits");
            xml.AppendChild(root);
            foreach (DataRow row in dtDBAudit.Rows)
            {
                XmlElement child = xml.CreateElement("DBAudit");
                root.AppendChild(child);
                for (int col = 0; col < row.ItemArray.Count(); col++)
                {
                    XmlElement colElement = xml.CreateElement(row.Table.Columns[col].ToString());
                    colElement.InnerText = row.ItemArray[col].ToString();
                    child.AppendChild(colElement);
                }
            }
            if (dtDBAudit.Rows.Count > 0)
            {
                xmlDBAudit = xml.OuterXml;
            }
            #endregion
            return xmlDBAudit;
        }

        private DSDBAudit GetPrivilegeDBAudit(DataTable dt, string PrivilegeAction, string PrivilegeName, string FormName, string AssignedTo)
        {

            DSUsers ds = new DSUsers();

            DSDBAudit.DBAuditDataTable dtDBAudit = new DSDBAudit.DBAuditDataTable();

            // PrivilegeName;
            string dbTableName = "ModuleFormUsersPrivileges";
            string Formname = "UserPrivilege";
            if ((PrivilegeAction == "True"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string ColumnsName = row["PrivilegeName"].ToString();
                    Dictionary<string, string> ColumnsDictionary = GetColumnsName(ColumnsName);


                    DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();
                    drActivity.ModuleName = row["FormName"].ToString();// FormName;
                    drActivity.AssignedTo = row["AssignedTo"].ToString(); //AssignedTo;
                    drActivity.OriginalValue = "False";
                    drActivity.CurrentValue = "True";
                    drActivity.ProfileName = Formname;
                    drActivity.DBTableName = dbTableName;
                    drActivity.DisplayName = row["PrivilegeName"].ToString(); //PrivilegeName;
                    drActivity.DBAuditAction = "Insert";
                    drActivity.UserName = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                    drActivity.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    dtDBAudit.Rows.Add(drActivity);
                }
            }
            else if ((PrivilegeAction == "False"))
            {
                DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();
                drActivity.ModuleName = FormName;
                drActivity.AssignedTo = AssignedTo;
                drActivity.OriginalValue = "True";
                drActivity.CurrentValue = "False";
                drActivity.ProfileName = Formname;
                drActivity.DBTableName = dbTableName;
                drActivity.DisplayName = PrivilegeName;
                drActivity.DBAuditAction = "Insert";
                drActivity.UserName = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                drActivity.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                dtDBAudit.Rows.Add(drActivity);
            }

            DSDBAudit dsDBAudit = new DSDBAudit();
            dsDBAudit.Merge(dtDBAudit);
            return dsDBAudit;
        }

        private static Dictionary<string, string> GetColumnsName(string ColumnsName)
        {


            string[] ColumnsSplit = ColumnsName.Split(',');

            Dictionary<string, string> ColumnsDictionary = new Dictionary<string, string>();

            foreach (string Column in ColumnsSplit)
            {
                string[] nameCaption = Column.Split('|');
                if (nameCaption.Length == 2 && nameCaption[1].Trim() != "")
                    ColumnsDictionary.Add(nameCaption[0].Trim(), nameCaption[1].Trim());
                else if (nameCaption.Length == 2 && nameCaption[1].Trim() == "")
                    ColumnsDictionary.Add(nameCaption[0].Trim(), nameCaption[0].Trim());

            }



            return ColumnsDictionary;
        }

        public Boolean IsDeleteColumnsExist(DataTable dt)
        {


            DSModuleForm ds = new DSModuleForm();
            ds = new DALModulesForms().LoadForms(0, "", "", dt.TableName);

            if (ds.Forms.Rows.Count > 0)
            {
                string ColumnsName = ds.Forms.Rows[0][ds.Forms.DBTableColumnsColumn.ColumnName].ToString();
                string Action = ds.Forms.Rows[0][ds.Forms.DBActionColumn.ColumnName].ToString();
                Dictionary<string, string> ColumnsDictionary = GetColumnsName(ColumnsName);
                foreach (DataRow row in dt.Rows)
                {
                    if (Action.Contains('D'))
                    {
                        foreach (DataColumn col in row.Table.Columns)
                        {
                            if (ColumnsDictionary.ContainsKey(col.ColumnName))
                            {
                                return true;
                            }
                        }
                    }
                }
            }


            return false;
        }

        public string GetDBAuditAdmin(DataTable dt, string PrimaryKeyId, string ParentKeyColumnId = "", bool isViewAction = false, bool isPrintAction = false, bool isDeleteAction = false, SharedVariable shareVariable = null)
        {

            DSModuleForm ds = new DSModuleForm();
            string Formname = string.Empty;
            string listType = string.Empty;
            long EntityId = 0;
            long UserId = 0;

            if (shareVariable == null)
            {
                UserId = Convert.ToInt64(MDVSession.Current.AppUserId);
                EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
            }
            else
            {
                UserId = Convert.ToInt64(MDVSession.Current.AppUserId);
                EntityId = Convert.ToInt64(shareVariable.EntityId);
            }
            if (shareVariable == null)
                ds = new DALModulesForms().LoadForms(0, Formname, "", dt.TableName);
            else
                ds = new DALModulesForms(shareVariable).LoadForms(0, Formname, "", dt.TableName);

            DSDBAudit.DBAuditAdminDataTable dtDBAudit = new DSDBAudit.DBAuditAdminDataTable();

            foreach (DataRow dr in ds.Forms.Rows)
            {
                string ColumnsName = dr[ds.Forms.DBTableColumnsColumn.ColumnName].ToString();
                string Action = dr[ds.Forms.DBActionColumn.ColumnName].ToString();
                string dbTableName = dr[ds.Forms.DBTableNameColumn.ColumnName].ToString();
                string FormName = dr[ds.Forms.NameColumn.ColumnName].ToString();
                
                string ModuleName = dr[ds.Forms.DescriptionColumn.ColumnName].ToString();
                if (string.IsNullOrEmpty(ModuleName))
                {
                    ModuleName = dr[ds.Forms.ModuleNameColumn.ColumnName].ToString();
                }
                Dictionary<string, string> ColumnsDictionary = GetColumnsName(ColumnsName);
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if ((row.RowState == DataRowState.Added))
                    {
                        if (Action.Contains('I'))
                        {
                            string isFirstRow = "";
                            foreach (DataColumn col in row.Table.Columns)
                            {
                                if (ColumnsDictionary.ContainsKey(col.ColumnName))
                                {
                                    if (isFirstRow == "" && row[col.ColumnName].ToString() != "")
                                    {
                                        DSDBAudit.DBAuditAdminRow drActivity = dtDBAudit.NewDBAuditAdminRow();
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;

                                        if (columnsKey.Length > 0)
                                        {
                                            if (dt.Columns.Contains("PrimaryKey") == true)
                                            {
                                                drActivity.ColumnKeyId = row["PrimaryKey"].ToString();
                                            }
                                            else
                                            {
                                                drActivity.ColumnKeyId = PrimaryKeyId;
                                            }
                                            drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                        }

                                        drActivity.OriginalValue = "";
                                        drActivity.CurrentValue= row[col.ColumnName].ToString();
                                        drActivity.ProfileName = FormName;
                                        drActivity.DBTableName = dbTableName;
                                        drActivity.ColumnName = col.ColumnName;
                                        drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                        drActivity.DBAuditAction = "Insert";
                                        drActivity.ModuleName = ModuleName;
                                        drActivity.UserId = UserId;
                                        if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                        {
                                            drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                        }
                                        dtDBAudit.Rows.Add(drActivity);
                                    }
                                }
                            }
                        }

                    }
                    else if ((row.RowState == DataRowState.Modified))
                    {
                        if (Action.Contains('U'))
                        {
                            //Start 26-10-2016 Humaira Yousaf to add view action log for DrFirst against each item
                            if (dt.Columns.Contains("PrimaryKey") && dt.Columns.Contains("ViewAction") && row["ViewAction"].ToString() == "View")
                            {

                                DSDBAudit.DBAuditAdminRow drActivity = dtDBAudit.NewDBAuditAdminRow();
                                DataColumn[] columnsKey;
                                columnsKey = row.Table.PrimaryKey;

                                if (columnsKey.Length > 0)
                                {
                                    drActivity.ColumnKeyId = row["PrimaryKey"].ToString();
                                    drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                }
                                drActivity.OriginalValue = "";
                                drActivity.CurrentValue = "";
                                drActivity.ProfileName = FormName;
                                drActivity.DBTableName = dbTableName;
                                drActivity.ColumnName = columnsKey[0].ColumnName;
                                drActivity.DisplayName = "";
                                drActivity.DBAuditAction = "View";
                                drActivity.ModuleName = ModuleName;
                                drActivity.UserId = UserId;
                                if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                {
                                    drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                }
                                dtDBAudit.Rows.Add(drActivity);
                            }
                            //End 26-10-2016 Humaira Yousaf to add view action log for DrFirst against each item
                            else
                            {
                                foreach (DataColumn col in row.Table.Columns)
                                {
                                    // removed byte[] conversion---MA-228
                                    //if ((ColumnsDictionary.ContainsKey(col.ColumnName) && !row.IsNull(col.ColumnName) && row[col.ColumnName] != null && !row[col.ColumnName].ToString().Trim().Equals(row[col.ColumnName, DataRowVersion.Original].ToString().Trim())) || (ColumnsDictionary.ContainsKey(col.ColumnName) && !row.IsNull(col.ColumnName) && row[col.ColumnName] != null && (col.ColumnName == "PatientProfileImagePath" || col.ColumnName == "PatientProfileThumbnailPath") && Convert.ToBase64String((byte[])(row[col.ColumnName, DataRowVersion.Original])) != Convert.ToBase64String((byte[])(row[col.ColumnName]))))
                                    if ((ColumnsDictionary.ContainsKey(col.ColumnName) && !row.IsNull(col.ColumnName) && row[col.ColumnName] != null && !row[col.ColumnName].ToString().Trim().Equals(row[col.ColumnName, DataRowVersion.Original].ToString().Trim())) || (ColumnsDictionary.ContainsKey(col.ColumnName) && !row.IsNull(col.ColumnName) && row[col.ColumnName] != null && (col.ColumnName == "PatientProfileImagePath" || col.ColumnName == "PatientProfileThumbnailPath") && (row[col.ColumnName, DataRowVersion.Original]) != row[col.ColumnName]))
                                    {
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;
                                        DSDBAudit.DBAuditAdminRow drActivity = dtDBAudit.NewDBAuditAdminRow();

                                        if (columnsKey.Length > 0)
                                        {
                                            drActivity.ColumnKeyId = row[columnsKey[0].ColumnName, DataRowVersion.Original].ToString();
                                            drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                        }
                                        
                                        drActivity.OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                        drActivity.CurrentValue = row[col.ColumnName].ToString();
                                        drActivity.ProfileName = FormName;
                                        drActivity.DBTableName = dbTableName;
                                        drActivity.ColumnName = col.ColumnName;
                                        drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                        drActivity.DBAuditAction = "Update";
                                        drActivity.ModuleName = ModuleName;
                                        drActivity.UserId = UserId;
                                        if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                        {
                                            drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                        }
                                        dtDBAudit.Rows.Add(drActivity);
                                    }

                                }
                            }

                        }

                    }
                    else if ((row.RowState == DataRowState.Deleted) || isDeleteAction == true)
                    {
                        if (Action.Contains('D'))
                        {
                            foreach (DataColumn col in row.Table.Columns)
                            {
                                if (isDeleteAction == true)
                                {
                                    if (ColumnsDictionary.ContainsKey(col.ColumnName) && row[col.ColumnName].ToString().Equals(row[col.ColumnName, DataRowVersion.Original].ToString()))
                                    {
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;
                                        DSDBAudit.DBAuditAdminRow drActivity = dtDBAudit.NewDBAuditAdminRow();


                                        if (columnsKey.Length > 0)
                                        {
                                            drActivity.ColumnKeyId = row[columnsKey[0].ColumnName, DataRowVersion.Original].ToString();
                                            drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                        }
                                        drActivity.OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                        drActivity.CurrentValue = "";
                                        drActivity.ProfileName = FormName;
                                        drActivity.DBTableName = dbTableName;
                                        drActivity.ColumnName = col.ColumnName;
                                        drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                        drActivity.DBAuditAction = "Delete";
                                        drActivity.ModuleName = ModuleName;
                                        drActivity.UserId = UserId;
                                        if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                        {
                                            drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                        }
                                        dtDBAudit.Rows.Add(drActivity);
                                    }
                                }

                            }

                        }

                    }
                    else if ((row.RowState == DataRowState.Unchanged))
                    {
                        string currentDBAuditAction = "";
                        if (isViewAction && Action.Contains("V"))
                        {
                            currentDBAuditAction = "View";
                        }
                        else if (isPrintAction && Action.Contains("P"))
                        {
                            currentDBAuditAction = "Print";
                        }
                        if (currentDBAuditAction != "")
                        {
                            //Start 24-05-2016 Edit By Humaira Yousaf Bug# EMR-1174
                            if (ModuleName == "FaceSheet" && dtDBAudit.Rows.Count > 0)
                            {
                                break;
                            }
                            //End 24-05-2016 Edit By Humaira Yousaf Bug# EMR-1174
                            DSDBAudit.DBAuditAdminRow drActivity = dtDBAudit.NewDBAuditAdminRow();
                            DataColumn[] columnsKey;
                            columnsKey = row.Table.PrimaryKey;

                            if (columnsKey.Length > 0)
                            {
                                if (dt.Columns.Contains("PrimaryKey") == true)
                                {
                                    drActivity.ColumnKeyId = row["PrimaryKey"].ToString();
                                }
                                else
                                {
                                    drActivity.ColumnKeyId = PrimaryKeyId;
                                }
                                drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                            }
                            drActivity.OriginalValue = "";
                            drActivity.CurrentValue = "";
                            drActivity.ProfileName = FormName;
                            drActivity.DBTableName = dbTableName;
                            drActivity.DisplayName = ""; 
                            drActivity.DBAuditAction = currentDBAuditAction;
                            drActivity.ModuleName = ModuleName;
                            drActivity.UserId = UserId;
                            if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                            {
                                drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                            }
                            dtDBAudit.Rows.Add(drActivity);
                        }
                    }
                    i++;
                }
                if ((listType == "FamilyHistory" || listType == "MedicalHistory" || listType == "SurgicalHistory") && dtDBAudit.Rows.Count > 0)
                {
                    break;
                }
            }
            string xmlDBAudit = "";
            #region Create XML of Chnages for Audit Log
            /******Create XML of the changes for Audit Log ************/
            /*****Added By Azeem Raza Tayyab on 25-May-2016 ***********/
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("DBAudits");
            xml.AppendChild(root);
            foreach (DataRow row in dtDBAudit.Rows)
            {
                XmlElement child = xml.CreateElement("DBAudit");
                root.AppendChild(child);
                for (int col = 0; col < row.ItemArray.Count(); col++)
                {
                    XmlElement colElement = xml.CreateElement(row.Table.Columns[col].ToString());
                    colElement.InnerText = row.ItemArray[col].ToString();
                    child.AppendChild(colElement);
                }
            }
            if (dtDBAudit.Rows.Count > 0)
            {
                xmlDBAudit = xml.OuterXml;
            }
            #endregion
            return xmlDBAudit;
        }
        #endregion


        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="dt"></param>
        /// <param name="dbManager"></param>
        /// <param name="PrimaryKeyId"></param>
        /// <param name="chargesID"></param>
        /// <param name="ParentKeyColumnId"></param>
        /// <param name="isViewAction"></param>
        /// <param name="isPrintAction"></param>
        /// <param name="isDeleteAction"></param>
        /// <param name="patientId"></param>
        /// <param name="isClinicalSummaryCopy"></param>
        /// <returns></returns>
        public DSDBAudit InsertDBAudit(SharedVariable SharedVariable, DataTable dt, IDBManager dbManager, string PrimaryKeyId, string chargesID = null, string ParentKeyColumnId = "", bool isViewAction = false, bool isPrintAction = false, bool isDeleteAction = false, string patientId = "", string isClinicalSummaryCopy = "0")
        {
            try
            {
                string xml = "";
                DSDBAudit ds = new DSDBAudit();
                if (dt == null)
                {
                    return new DSDBAudit();
                }
                if (isClinicalSummaryCopy != "0")
                {
                    dbManager.Open();
                    string SummaryType = isClinicalSummaryCopy == "1" ? "Clinical Summary" : "Referral Summary";
                    DSDBAudit.DBAuditRow drActivity = ds.DBAudit.NewDBAuditRow();
                    drActivity.ColumnKeyId = "1";
                    drActivity.ColumnKeyName = SummaryType;
                    drActivity.OriginalValue = "";
                    drActivity.CurrentValue = "";

                    drActivity.ProfileName = SummaryType;
                    drActivity.DBTableName = SummaryType;
                    drActivity.ColumnName = SummaryType;
                    drActivity.DisplayName = SummaryType; //ColumnsDictionary[columnsKey[0].ColumnName];
                    drActivity.DBAuditAction = "Copy";
                    drActivity.ModuleName = SummaryType;
                    drActivity.UserName = ClientConfiguration.DecryptFrom64(SharedVariable.UserName);
                    drActivity.PatientId = patientId;
                    //if (row.Table.Columns.Contains("VisitId"))
                    //{
                    //    string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                    //    if (Convert.ToDouble(VisitID) > 0)
                    //        drActivity.VisitId = row["VisitId"].ToString();
                    //}
                    drActivity.EntityId = Convert.ToInt64(SharedVariable.EntityId);
                    //if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                    //{
                    //    drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                    //}

                    ds.DBAudit.Rows.Add(drActivity);
                    /******Create XML of the changes for Audit Log ************/
                    /*****Added By Azeem Raza Tayyab on 25-May-2016 ***********/
                    if (ds.DBAudit.Rows.Count > 0)
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        XmlElement root = xmlDoc.CreateElement("DBAudits");
                        xmlDoc.AppendChild(root);
                        foreach (DataRow row in ds.DBAudit.Rows)
                        {
                            XmlElement child = xmlDoc.CreateElement("DBAudit");
                            root.AppendChild(child);
                            for (int col = 0; col < row.ItemArray.Count(); col++)
                            {
                                XmlElement colElement = xmlDoc.CreateElement(row.Table.Columns[col].ToString());
                                if ((row.Table.Columns[col].ToString() == "PatientImage" || row.Table.Columns[col].ToString() == "PatientImageThumbnail"))
                                    colElement.InnerText = Convert.ToBase64String((byte[])(row.ItemArray[col])).ToString();
                                else
                                    colElement.InnerText = row.ItemArray[col].ToString();
                                child.AppendChild(colElement);
                            }
                        }

                        xml = xmlDoc.OuterXml;

                    }

                }
                else
                {
                    //Start//Abid Ali//for bug# EMR-1576//Passed patient id to getdbAudit
                    xml = GetDBAudit(SharedVariable, dt, PrimaryKeyId, chargesID, ParentKeyColumnId, isViewAction, isPrintAction, isDeleteAction, patientId);
                    //End//Abid Ali//for bug# EMR-1576//Passed patient id to getdbAudit
                }

                if (!string.IsNullOrEmpty(xml))
                {
                    dbManager.CreateParameters(1);
                    dbManager.AddParameters(0, PARM_XML_DATA, xml);
                    ds = (DSDBAudit)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DBAUDIT_INSERT_BULK, ds, ds.DBAuditIds.TableName);
                }
                //ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DBActivityAudit::InsertDBAudit", PROC_DBAUDIT_INSERT, ex);
                throw ex;
                //Usual code
            }

        }


        public DSDBAudit InsertDBAudit(DataTable dt, IDBManager dbManager, string PrimaryKeyId, string chargesID = null, string ParentKeyColumnId = "", bool isViewAction = false, bool isPrintAction = false, bool isDeleteAction = false, string patientId = "", string isClinicalSummaryCopy = "0", SharedVariable shareVariable = null)
        {
            try
            {
                DSDBAudit ds = new DSDBAudit();

                string xml = "";
                if (dt == null)
                {
                    return new DSDBAudit();
                }
                if (isClinicalSummaryCopy != "0")
                {
                    dbManager.Open();
                    string SummaryType = isClinicalSummaryCopy == "1" ? "Clinical Summary" : "Referral Summary";
                    DSDBAudit.DBAuditRow drActivity = ds.DBAudit.NewDBAuditRow();
                    drActivity.ColumnKeyId = "1";
                    drActivity.ColumnKeyName = SummaryType;
                    drActivity.OriginalValue = "N/A";
                    drActivity.CurrentValue = "N/A";

                    drActivity.ProfileName = SummaryType;
                    drActivity.DBTableName = SummaryType;
                    drActivity.ColumnName = SummaryType;
                    drActivity.DisplayName = SummaryType; //ColumnsDictionary[columnsKey[0].ColumnName];
                    drActivity.DBAuditAction = "Copy";
                    drActivity.ModuleName = SummaryType;
                    if (shareVariable == null)
                    {
                        drActivity.UserName = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                        drActivity.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    }
                    else
                    {

                        drActivity.UserName = ClientConfiguration.DecryptFrom64(shareVariable.UserName);
                        drActivity.EntityId = Convert.ToInt64(shareVariable.EntityId);
                    }
                    drActivity.PatientId = patientId;
                    //if (row.Table.Columns.Contains("VisitId"))
                    //{
                    //    string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                    //    if (Convert.ToDouble(VisitID) > 0)
                    //        drActivity.VisitId = row["VisitId"].ToString();
                    //}

                    //if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                    //{
                    //    drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                    //}

                    ds.DBAudit.Rows.Add(drActivity);
                    /******Create XML of the changes for Audit Log ************/
                    /*****Added By Azeem Raza Tayyab on 25-May-2016 ***********/
                    if (ds.DBAudit.Rows.Count > 0)
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        XmlElement root = xmlDoc.CreateElement("DBAudits");
                        xmlDoc.AppendChild(root);
                        foreach (DataRow row in ds.DBAudit.Rows)
                        {
                            XmlElement child = xmlDoc.CreateElement("DBAudit");
                            root.AppendChild(child);
                            for (int col = 0; col < row.ItemArray.Count(); col++)
                            {
                                XmlElement colElement = xmlDoc.CreateElement(row.Table.Columns[col].ToString());
                                if (row.Table.Columns[col].ToString() == "PatientImage" || row.Table.Columns[col].ToString() == "PatientImageThumbnail")
                                    colElement.InnerText = Convert.ToBase64String((byte[])(row.ItemArray[col])).ToString();
                                else
                                    colElement.InnerText = row.ItemArray[col].ToString();
                                child.AppendChild(colElement);
                            }
                        }

                        xml = xmlDoc.OuterXml;

                    }

                }
                else
                {
                    //Start//Abid Ali//for bug# EMR-1576//Passed patient id to getdbAudit
                    xml = GetDBAudit(dt, PrimaryKeyId, chargesID, ParentKeyColumnId, isViewAction, isPrintAction, isDeleteAction, patientId, shareVariable);
                    //End//Abid Ali//for bug# EMR-1576//Passed patient id to getdbAudit
                }

                if (!string.IsNullOrEmpty(xml))
                {
                    dbManager.CreateParameters(1);
                    dbManager.AddParameters(0, PARM_XML_DATA, xml);
                    ds = (DSDBAudit)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DBAUDIT_INSERT_BULK, ds, ds.DBAuditIds.TableName);

                }
                //ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                // MDVLogger.DALErrorLog("DBActivityAudit::InsertDBAudit", PROC_DBAUDIT_INSERT, ex);
                MDVLogger.SendExcepToDB(ex, "InsertDBAudit", PROC_DBAUDIT_INSERT);
                throw ex;
                //Usual code
            }

        }
        public DSDBAudit InsertDBAuditAppointmentHistory(DataTable dt, IDBManager dbManager, string PrimaryKeyId,  string ParentKeyColumnId = "", bool isViewAction = false,  bool isDeleteAction = false,  SharedVariable shareVariable = null)
        {
            try
            {
                DSDBAudit ds = new DSDBAudit();
                string xml = "";
                if (dt == null)
                {
                    return new DSDBAudit();
                }
              
               
                    //Start//Abid Ali//for bug# EMR-1576//Passed patient id to getdbAudit
                    xml = GetDBAuditAppointment(dt, PrimaryKeyId, ParentKeyColumnId, isViewAction, isDeleteAction, shareVariable);
                    //End//Abid Ali//for bug# EMR-1576//Passed patient id to getdbAudit
                

                if (!string.IsNullOrEmpty(xml))
                {


                    dbManager.CreateParameters(1);
                    dbManager.AddParameters(0, PARM_XML_DATA, xml);
                    ds = (DSDBAudit)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DBAUDIT_APPOINTMENT_INSERT_BULK, ds, ds.DBAuditIds.TableName);

                }
                //ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                // MDVLogger.DALErrorLog("DBActivityAudit::InsertDBAudit", PROC_DBAUDIT_INSERT, ex);
                MDVLogger.SendExcepToDB(ex, "InsertDBAudit", PROC_DBAUDIT_APPOINTMENT_INSERT_BULK);
                throw ex;
                //Usual code
            }

        }
        public string GetDBAuditAppointment(DataTable dt, string PrimaryKeyId, string ParentKeyColumnId = "", bool isViewAction = false,  bool isDeleteAction = false, SharedVariable shareVariable = null)
        {

            DSModuleForm ds = new DSModuleForm();
            string Formname = string.Empty;
            string listType = string.Empty;
            long EntityId = 0;
            long UserId = 0;

            if (shareVariable == null)
            {
                UserId = Convert.ToInt64(MDVSession.Current.AppUserId);
                EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
            }
            else
            {
                UserId = Convert.ToInt64(MDVSession.Current.AppUserId);
                EntityId = Convert.ToInt64(shareVariable.EntityId);
            }
            if (shareVariable == null)
                ds = new DALModulesForms().LoadForms(0, Formname, "", dt.TableName);
            else
                ds = new DALModulesForms(shareVariable).LoadForms(0, Formname, "", dt.TableName);

            DSDBAudit.DBAuditAppointmentDataTable dtDBAudit = new DSDBAudit.DBAuditAppointmentDataTable();

            foreach (DataRow dr in ds.Forms.Rows)
            {
                string ColumnsName = dr[ds.Forms.DBTableColumnsColumn.ColumnName].ToString();
                string Action = dr[ds.Forms.DBActionColumn.ColumnName].ToString();
                string dbTableName = dr[ds.Forms.DBTableNameColumn.ColumnName].ToString();
                string FormName = dr[ds.Forms.NameColumn.ColumnName].ToString();

                string ModuleName = dr[ds.Forms.DescriptionColumn.ColumnName].ToString();
                if (string.IsNullOrEmpty(ModuleName))
                {
                    ModuleName = dr[ds.Forms.ModuleNameColumn.ColumnName].ToString();
                }
                Dictionary<string, string> ColumnsDictionary = GetColumnsName(ColumnsName);
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if ((row.RowState == DataRowState.Added))
                    {
                        if (Action.Contains('I'))
                        {
                            string isFirstRow = "";
                            foreach (DataColumn col in row.Table.Columns)
                            {
                               
                                if (ColumnsDictionary.ContainsKey(col.ColumnName)&&(col.ColumnName!="ModifiedBy"&&col.ColumnName!="ModifiedOn"))
                                {
                                    if (isFirstRow == "" && row[col.ColumnName].ToString() != "")
                                    {
                                        DSDBAudit.DBAuditAppointmentRow drActivity = dtDBAudit.NewDBAuditAppointmentRow();
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;

                                        if (columnsKey.Length > 0)
                                        {
                                            if (dt.Columns.Contains("PrimaryKey") == true)
                                            {
                                                drActivity.ColumnKeyId = row["PrimaryKey"].ToString();
                                            }
                                            else
                                            {
                                                drActivity.ColumnKeyId = PrimaryKeyId;
                                            }
                                            drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                        }

                                        drActivity.OriginalValue = "";
                                        drActivity.CurrentValue = row[col.ColumnName].ToString();
                                        drActivity.ProfileName = FormName;
                                        drActivity.DBTableName = dbTableName;
                                        drActivity.ColumnName = col.ColumnName;
                                        drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                        drActivity.DBAuditAction = "Insert";
                                        drActivity.ModuleName = ModuleName;
                                        drActivity.UserId = UserId;
                                        drActivity.EntityId = EntityId;
                                        drActivity.UserName = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                                        if (row.Table.Columns.Contains("AppointmentId"))
                                        {
                                            string AppointmentId = PrimaryKeyId;
                                            {
                                                drActivity.AppointmentId = Convert.ToInt64(AppointmentId);
                                                drActivity.ColumnKeyId = AppointmentId.ToString();
                                            }
                                        }
                                        
                                        if (row.Table.Columns.Contains("PatientId"))
                                        {
                                            string PatientId = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                            if (Convert.ToDouble(PatientId) > 0)
                                            {
                                                drActivity.PatientId = Convert.ToString(row["PatientId"]);
                                               
                                            }
                                        }
                                        if (row.Table.Columns.Contains("ProviderId"))
                                        {
                                            string ProviderId = string.IsNullOrEmpty(row["ProviderId"].ToString()) ? "0" : row["ProviderId"].ToString();
                                            if (Convert.ToDouble(ProviderId) > 0)
                                            {
                                                drActivity.ProviderId = row["ProviderId"].ToString();
                                                
                                            }
                                        }
                                        if (row.Table.Columns.Contains("FacilityId"))
                                        {
                                            string FacilityId = string.IsNullOrEmpty(row["FacilityId"].ToString()) ? "0" : row["FacilityId"].ToString();
                                            if (Convert.ToDouble(FacilityId) > 0)
                                            {
                                                drActivity.FacilityId = row["FacilityId"].ToString();
                                               
                                            }
                                        }
                                        if (row.Table.Columns.Contains("ResourceProviderId"))
                                        {
                                            string ResourceProviderId = string.IsNullOrEmpty(row["ResourceProviderId"].ToString()) ? "0" : row["ResourceProviderId"].ToString();
                                            if (Convert.ToDouble(ResourceProviderId) > 0)
                                            {
                                                drActivity.ResourceProviderId = row["ResourceProviderId"].ToString();
                                              
                                            }
                                        }
                                        if (row.Table.Columns.Contains("PracticeId"))
                                        {
                                            string PracticeId = string.IsNullOrEmpty(row["PracticeId"].ToString()) ? "0" : row["PracticeId"].ToString();
                                            if (Convert.ToDouble(PracticeId) > 0)
                                            {
                                                drActivity.PracticeId = row["PracticeId"].ToString();
                                               
                                            }
                                        }
                                       
                                        if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                        {
                                            drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                        }
                                        dtDBAudit.Rows.Add(drActivity);
                                    }
                                }
                            }
                        }

                    }
                    else if ((row.RowState == DataRowState.Modified))
                    {
                        if (Action.Contains('U'))
                        {
                            //Start 26-10-2016 Humaira Yousaf to add view action log for DrFirst against each item
                            if (dt.Columns.Contains("PrimaryKey") && dt.Columns.Contains("ViewAction") && row["ViewAction"].ToString() == "View")
                            {

                                DSDBAudit.DBAuditAppointmentRow drActivity = dtDBAudit.NewDBAuditAppointmentRow();
                                DataColumn[] columnsKey;
                                columnsKey = row.Table.PrimaryKey;

                                if (columnsKey.Length > 0)
                                {
                                    drActivity.ColumnKeyId = row["PrimaryKey"].ToString();
                                    drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                }
                                drActivity.OriginalValue = "";
                                drActivity.CurrentValue = "";
                                drActivity.ProfileName = FormName;
                                drActivity.DBTableName = dbTableName;
                                drActivity.ColumnName = columnsKey[0].ColumnName;
                                drActivity.DisplayName = "";
                                drActivity.DBAuditAction = "View";
                                drActivity.ModuleName = ModuleName;
                                drActivity.UserId = UserId;
                                if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                {
                                    drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                }
                                dtDBAudit.Rows.Add(drActivity);
                            }
                            //End 26-10-2016 Humaira Yousaf to add view action log for DrFirst against each item
                            else
                            {
                                foreach (DataColumn col in row.Table.Columns)
                                {
                                    // removed byte[] conversion---MA-228                                 
                                    //if ((ColumnsDictionary.ContainsKey(col.ColumnName) && !row.IsNull(col.ColumnName) && row[col.ColumnName] != null && !row[col.ColumnName].ToString().Trim().Equals(row[col.ColumnName, DataRowVersion.Original].ToString().Trim())) || (ColumnsDictionary.ContainsKey(col.ColumnName) && !row.IsNull(col.ColumnName) && row[col.ColumnName] != null && (col.ColumnName == "PatientProfileImagePath" || col.ColumnName == "PatientProfileThumbnailPath") && Convert.ToBase64String((byte[])(row[col.ColumnName, DataRowVersion.Original])) != Convert.ToBase64String((byte[])(row[col.ColumnName]))))
                                    if ((ColumnsDictionary.ContainsKey(col.ColumnName) && !row.IsNull(col.ColumnName) && row[col.ColumnName] != null && !row[col.ColumnName].ToString().Trim().Equals(row[col.ColumnName, DataRowVersion.Original].ToString().Trim())) || (ColumnsDictionary.ContainsKey(col.ColumnName) && !row.IsNull(col.ColumnName) && row[col.ColumnName] != null && (col.ColumnName == "AppointmentDate" || col.ColumnName == "TimeTo") && (row[col.ColumnName, DataRowVersion.Original]) != row[col.ColumnName]))
                                    {
                                        bool InsertRecord = true;
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;
                                        DSDBAudit.DBAuditAppointmentRow drActivity = dtDBAudit.NewDBAuditAppointmentRow();
                                        if (col.ColumnName.ToString().Trim() == "AppointmentDate")
                                        {
                                            
                                            string OriginalDate = Convert.ToDateTime(row[col.ColumnName, DataRowVersion.Original]).ToShortDateString();

                                            string CurrentDate = row[col.ColumnName].ToString();
                                            if (OriginalDate == CurrentDate)
                                            {
                                                InsertRecord = false;

                                            }


                                        }
                                         if (col.ColumnName.ToString().Trim() == "TimeTo")
                                        {
                                            DateTime OriginalTime = DateTime.Parse(row[col.ColumnName, DataRowVersion.Original].ToString());
                                            DateTime CurrentTime = DateTime.Parse(row[col.ColumnName].ToString());
                                          
                                            if (OriginalTime == CurrentTime)
                                            {
                                                InsertRecord = false;

                                            }


                                        }
                                        //if (col.ColumnName.ToString().Trim() == "SchStatusId")
                                        //{
                                        //    DateTime OriginalTime = DateTime.Parse(row[col.ColumnName, DataRowVersion.Original].ToString());
                                        //    DateTime CurrentTime = DateTime.Parse(row[col.ColumnName].ToString());

                                        //    if (OriginalTime == CurrentTime)
                                        //    {
                                        //        InsertRecord = false;

                                        //    }


                                        //}
                                        


                                        if (InsertRecord)
                                        {
                                            if (columnsKey.Length > 0)
                                            {
                                                drActivity.ColumnKeyId = row[columnsKey[0].ColumnName, DataRowVersion.Original].ToString();
                                                drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                            }

                                            drActivity.OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                            drActivity.CurrentValue = row[col.ColumnName].ToString();
                                            drActivity.ProfileName = FormName;
                                            drActivity.DBTableName = dbTableName;
                                            drActivity.ColumnName = col.ColumnName;
                                            drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                            drActivity.DBAuditAction = "Update";
                                            drActivity.ModuleName = ModuleName;
                                            drActivity.UserId = UserId;
                                            drActivity.EntityId = EntityId;
                                            drActivity.UserName = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                                            if (row.Table.Columns.Contains("AppointmentId"))
                                            {
                                                string AppointmentId = PrimaryKeyId;
                                                {
                                                    drActivity.AppointmentId = Convert.ToInt64(AppointmentId);
                                                    drActivity.ColumnKeyId = AppointmentId.ToString();
                                                }
                                            }
                                            if (row.Table.Columns.Contains("PatientId"))
                                            {
                                                string PatientId = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                                if (Convert.ToDouble(PatientId) > 0)
                                                {
                                                    drActivity.PatientId = Convert.ToString(row["PatientId"]);
                                                    drActivity.ColumnKeyId = row["PatientId"].ToString();
                                                }
                                            }
                                            if (row.Table.Columns.Contains("ProviderId"))
                                            {
                                                string ProviderId = string.IsNullOrEmpty(row["ProviderId"].ToString()) ? "0" : row["ProviderId"].ToString();
                                                if (Convert.ToDouble(ProviderId) > 0)
                                                {
                                                    drActivity.ProviderId = row["ProviderId"].ToString();
                                                    drActivity.ColumnKeyId = row["ProviderId"].ToString();
                                                }
                                            }
                                            if (row.Table.Columns.Contains("FacilityId"))
                                            {
                                                string FacilityId = string.IsNullOrEmpty(row["FacilityId"].ToString()) ? "0" : row["FacilityId"].ToString();
                                                if (Convert.ToDouble(FacilityId) > 0)
                                                {
                                                    drActivity.FacilityId = row["FacilityId"].ToString();
                                                    drActivity.ColumnKeyId = row["FacilityId"].ToString();
                                                }
                                            }
                                            if (row.Table.Columns.Contains("ResourceProviderId"))
                                            {
                                                string ResourceProviderId = string.IsNullOrEmpty(row["ResourceProviderId"].ToString()) ? "0" : row["ResourceProviderId"].ToString();
                                                if (Convert.ToDouble(ResourceProviderId) > 0)
                                                {
                                                    drActivity.ResourceProviderId = row["ResourceProviderId"].ToString();
                                                    drActivity.ColumnKeyId = row["ResourceProviderId"].ToString();
                                                }
                                            }
                                            if (row.Table.Columns.Contains("PracticeId"))
                                            {
                                                string PracticeId = string.IsNullOrEmpty(row["PracticeId"].ToString()) ? "0" : row["PracticeId"].ToString();
                                                if (Convert.ToDouble(PracticeId) > 0)
                                                {
                                                    drActivity.PracticeId = row["PracticeId"].ToString();
                                                    drActivity.ColumnKeyId = row["PracticeId"].ToString();
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                            {
                                                drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                            }
                                            dtDBAudit.Rows.Add(drActivity);
                                        }

                                    }
                                   


                                }
                            }

                        }

                    }
                    else if ((row.RowState == DataRowState.Deleted) || isDeleteAction == true)
                    {
                        if (Action.Contains('D'))
                        {
                            foreach (DataColumn col in row.Table.Columns)
                            {
                                if (isDeleteAction == true)
                                {
                                    if (ColumnsDictionary.ContainsKey(col.ColumnName) && row[col.ColumnName].ToString().Equals(row[col.ColumnName, DataRowVersion.Original].ToString()))
                                    {
                                        DataColumn[] columnsKey;
                                        columnsKey = row.Table.PrimaryKey;
                                        DSDBAudit.DBAuditAppointmentRow drActivity = dtDBAudit.NewDBAuditAppointmentRow();


                                        if (columnsKey.Length > 0)
                                        {
                                            drActivity.ColumnKeyId = row[columnsKey[0].ColumnName, DataRowVersion.Original].ToString();
                                            drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                                        }
                                        drActivity.OriginalValue = row[col.ColumnName, DataRowVersion.Original].ToString();
                                        drActivity.CurrentValue = "";
                                        drActivity.ProfileName = FormName;
                                        drActivity.DBTableName = dbTableName;
                                        drActivity.ColumnName = col.ColumnName;
                                        drActivity.DisplayName = ColumnsDictionary[col.ColumnName];
                                        drActivity.DBAuditAction = "Delete";
                                        drActivity.ModuleName = ModuleName;
                                        drActivity.UserId = UserId;
                                        drActivity.EntityId = EntityId;
                                        drActivity.UserName = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                                        if (row.Table.Columns.Contains("AppointmentId"))
                                        {
                                            string AppointmentId = PrimaryKeyId;
                                            {
                                                drActivity.AppointmentId = Convert.ToInt64(AppointmentId);
                                                drActivity.ColumnKeyId = AppointmentId.ToString();
                                            }
                                        }
                                        if (row.Table.Columns.Contains("PatientId"))
                                        {
                                            string PatientId = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                                            if (Convert.ToDouble(PatientId) > 0)
                                            {
                                                drActivity.PatientId = Convert.ToString(row["PatientId"]);
                                                drActivity.ColumnKeyId = row["PatientId"].ToString();
                                            }
                                        }
                                        if (row.Table.Columns.Contains("ProviderId"))
                                        {
                                            string ProviderId = string.IsNullOrEmpty(row["ProviderId"].ToString()) ? "0" : row["ProviderId"].ToString();
                                            if (Convert.ToDouble(ProviderId) > 0)
                                            {
                                                drActivity.ProviderId = row["ProviderId"].ToString();
                                                drActivity.ColumnKeyId = row["ProviderId"].ToString();
                                            }
                                        }
                                        if (row.Table.Columns.Contains("FacilityId"))
                                        {
                                            string FacilityId = string.IsNullOrEmpty(row["FacilityId"].ToString()) ? "0" : row["FacilityId"].ToString();
                                            if (Convert.ToDouble(FacilityId) > 0)
                                            {
                                                drActivity.FacilityId = row["FacilityId"].ToString();
                                                drActivity.ColumnKeyId = row["FacilityId"].ToString();
                                            }
                                        }
                                        if (row.Table.Columns.Contains("ResourceProviderId"))
                                        {
                                            string ResourceProviderId = string.IsNullOrEmpty(row["ResourceProviderId"].ToString()) ? "0" : row["ResourceProviderId"].ToString();
                                            if (Convert.ToDouble(ResourceProviderId) > 0)
                                            {
                                                drActivity.ResourceProviderId = row["ResourceProviderId"].ToString();
                                                drActivity.ColumnKeyId = row["ResourceProviderId"].ToString();
                                            }
                                        }
                                        if (row.Table.Columns.Contains("PracticeId"))
                                        {
                                            string PracticeId = string.IsNullOrEmpty(row["PracticeId"].ToString()) ? "0" : row["PracticeId"].ToString();
                                            if (Convert.ToDouble(PracticeId) > 0)
                                            {
                                                drActivity.PracticeId = row["PracticeId"].ToString();
                                                drActivity.ColumnKeyId = row["PracticeId"].ToString();
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                                        {
                                            drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                                        }
                                        dtDBAudit.Rows.Add(drActivity);
                                    }
                                }

                            }

                        }

                    }
                    else if ((row.RowState == DataRowState.Unchanged))
                    {
                        string currentDBAuditAction = "";
                        if (isViewAction && Action.Contains("V"))
                        {
                            currentDBAuditAction = "View";
                        }
                       
                        if (currentDBAuditAction != "")
                        {
                           
                            //End 24-05-2016 Edit By Humaira Yousaf Bug# EMR-1174
                            DSDBAudit.DBAuditAppointmentRow drActivity = dtDBAudit.NewDBAuditAppointmentRow();
                            DataColumn[] columnsKey;
                            columnsKey = row.Table.PrimaryKey;

                            if (columnsKey.Length > 0)
                            {
                                if (dt.Columns.Contains("PrimaryKey") == true)
                                {
                                    drActivity.ColumnKeyId = row["PrimaryKey"].ToString();
                                }
                                else
                                {
                                    drActivity.ColumnKeyId = PrimaryKeyId;
                                }
                                drActivity.ColumnKeyName = columnsKey[0].ColumnName;
                            }
                            drActivity.OriginalValue = "";
                            drActivity.CurrentValue = "";
                            drActivity.ProfileName = FormName;
                            drActivity.DBTableName = dbTableName;
                            drActivity.DisplayName = "";
                            drActivity.DBAuditAction = currentDBAuditAction;
                            drActivity.ModuleName = ModuleName;
                            drActivity.UserId = UserId;
                            if (!string.IsNullOrEmpty(ParentKeyColumnId) && Convert.ToInt64(ParentKeyColumnId) > 0)
                            {
                                drActivity.ParentKeyColumnId = Convert.ToInt64(ParentKeyColumnId);
                            }
                            dtDBAudit.Rows.Add(drActivity);
                        }
                    }
                    i++;
                }
               
            }
            string xmlDBAudit = "";
            #region Create XML of Chnages for Audit Log
            /******Create XML of the changes for Audit Log ************/
            /*****Added By Azeem Raza Tayyab on 25-May-2016 ***********/
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("DBAudits");
            xml.AppendChild(root);
            foreach (DataRow row in dtDBAudit.Rows)
            {
                XmlElement child = xml.CreateElement("DBAudit");
                root.AppendChild(child);
                for (int col = 0; col < row.ItemArray.Count(); col++)
                {
                    XmlElement colElement = xml.CreateElement(row.Table.Columns[col].ToString());
                    colElement.InnerText = row.ItemArray[col].ToString();
                    child.AppendChild(colElement);
                }
            }
            if (dtDBAudit.Rows.Count > 0)
            {
                xmlDBAudit = xml.OuterXml;
            }
            #endregion
            return xmlDBAudit;
        }

        public void InsertDBAuditAsync(DataTable dt, string PrimaryKeyId, string chargesID = null, string ParentKeyColumnId = "", bool isViewAction = false, bool isPrintAction = false, bool isDeleteAction = false, string patientId = "", string isClinicalSummaryCopy = "0", SharedVariable sharedVariable = null)
        {
            if (sharedVariable == null)
                sharedVariable = SharedVariable.GetSharedVariable();
            Thread thread = new Thread(new ThreadStart(delegate ()
            {

                try
                {
                    IDBManager dbManager = ClientConfiguration.GetDBManager();
                    dbManager.Open();



                    DSDBAudit ds = new DSDBAudit();
                    ds = InsertDBAudit(dt, dbManager, PrimaryKeyId, chargesID, ParentKeyColumnId, isViewAction, isPrintAction, isDeleteAction, patientId, isClinicalSummaryCopy, sharedVariable);
                    ds.AcceptChanges();
                    //  Trace.WriteLine("db activity inserted at: " + DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond);
                }
                catch (Exception ex)
                {
                    // MDVLogger.DALErrorLog("DBActivityAudit::InsertDBAudit", PROC_DBAUDIT_INSERT, ex);
                    MDVLogger.SendExcepToDB(ex, "InsertDBAuditASync", null);
                    // throw ex;
                    //Usual code
                }
            }));
            thread.IsBackground = true;
            thread.Start();

        }
        public void InsertDBAuditAsync<T>(string DBTableName, List<T> list, string PrimaryKeyId, string chargesID = null, string ParentKeyColumnId = "", bool isViewAction = false, bool isPrintAction = false, bool isDeleteAction = false, string patientId = "", string isClinicalSummaryCopy = "0") where T : class
        {
            SharedVariable sharedVariable = SharedVariable.GetSharedVariable();

            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                try
                {
                    IDBManager dbManagerThread = ClientConfiguration.GetDBManager();
                    dbManagerThread.Open();

                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                    DataTable table = new DataTable(DBTableName);
                    for (int i = 0; i < props.Count; i++)
                    {
                        PropertyDescriptor prop = props[i];
                        table.Columns.Add(prop.Name, prop.PropertyType);
                    }
                    //add additional columns here !
                    object[] values = new object[props.Count];
                    foreach (T item in list)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = props[i].GetValue(item);
                        }

                        table.Rows.Add(values);
                    }

                    // EMR-4685/Fahad Malik row state is set as 'added'(before this line)  due to which DB audit make dublicate entries, i have set row state to 'UNCHANGED'
                    table.AcceptChanges();
                    DSDBAudit dsDBAudit = new DSDBAudit();

                    dsDBAudit = new DBActivityAudit(sharedVariable).InsertDBAudit(table, dbManagerThread, PrimaryKeyId, chargesID, "", isViewAction, isPrintAction, isDeleteAction, patientId, isClinicalSummaryCopy, sharedVariable);
                    dsDBAudit.AcceptChanges();
                    //  Trace.WriteLine("db activity for " + DBTableName + " inserted at: " + DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond);

                }
                catch (Exception ex)
                {
                    MDVLogger.SendExcepToDB(ex, "InsertDBAuditASync", null);
                }
            }));
            thread.IsBackground = true;
            thread.Start();

        }

        public DSDBAudit InsertPrivilegeDBAudit(DataTable dt, IDBManager dbManager, string PrivilegeAction, string PrivilegeName, string FormName, string AssignedTo)
        {
            try
            {
                DSDBAudit ds = GetPrivilegeDBAudit(dt, PrivilegeAction, PrivilegeName, FormName, AssignedTo);
                if (ds.DBAudit.Rows.Count > 0)
                {
                    this.CreateParameters(dbManager, ds);
                    ds = (DSDBAudit)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_DBAUDIT_INSERT, ds, ds.DBAudit.TableName);
                }
                //ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DBActivityAudit::InsertDBAudit", PROC_DBAUDIT_INSERT, ex);
                throw ex;
                //Usual code
            }

        }
        public DSDBAudit LoadDBAudit(string ModuleName, string UserName, string ColumnKeyId, string ProfileName, string DBAuditId, string PatientId, string VisitId, DateTime? CreatedDate, string DBTableName = null, string ParentKeyColumnId = "", string ActionName = "", string CreatedDateFrom = "", string CreatedDateTo = "", int PageNumber = 1, int RowsPerPage = 1000, string LookUpID = null)
        {
            if (DBTableName == "Allergy")
                DBTableName = "Allergies";

            DSDBAudit ds = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(18);

                if (DBAuditId == "")
                    dbManager.AddParameters(0, PARM_DB_AUDIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DB_AUDIT_ID, Convert.ToInt64(DBAuditId));

                if (ModuleName == "")
                    dbManager.AddParameters(1, PARM_MODULE_NAME, null);
                else
                    dbManager.AddParameters(1, PARM_MODULE_NAME, ModuleName);

                if (UserName == "- select -" || UserName == "")
                    dbManager.AddParameters(2, PARM_USER_NAME, null);
                else
                    dbManager.AddParameters(2, PARM_USER_NAME, UserName);

                if (ColumnKeyId == "")
                    dbManager.AddParameters(3, PARM_COLUMN_KEY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_COLUMN_KEY_ID, ColumnKeyId);

                if (ProfileName == "")
                    dbManager.AddParameters(4, PARM_PROFILE_NAME, null);
                else
                    dbManager.AddParameters(4, PARM_PROFILE_NAME, ProfileName);

                if (PatientId == "" || PatientId == null)
                    dbManager.AddParameters(5, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PATIENT_ID, Convert.ToInt64(PatientId));

                if (VisitId == "")
                    dbManager.AddParameters(6, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(6, PARM_VISIT_ID, Convert.ToInt32(VisitId));

                if (CreatedDate == null)
                    dbManager.AddParameters(7, PARM_CREATED_DATE, null);
                else
                    dbManager.AddParameters(7, PARM_CREATED_DATE, CreatedDate);
                if (string.IsNullOrEmpty(DBTableName))
                    dbManager.AddParameters(8, PARM_DB_TABLENAME, null);
                else
                    dbManager.AddParameters(8, PARM_DB_TABLENAME, DBTableName);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                if (ParentKeyColumnId == "")
                    dbManager.AddParameters(10, PARM_PARENT_COLMN_KEY_ID, null);
                else
                    dbManager.AddParameters(10, PARM_PARENT_COLMN_KEY_ID, Convert.ToInt64(ParentKeyColumnId));

                if (ActionName == "")
                    dbManager.AddParameters(11, PARM_DB_ACTION_NAME, null);
                else
                    dbManager.AddParameters(11, PARM_DB_ACTION_NAME, ActionName);

                if (CreatedDateFrom == "")
                    dbManager.AddParameters(12, PARM_CREATED_DATE_FROM, null);
                else
                    dbManager.AddParameters(12, PARM_CREATED_DATE_FROM, CreatedDateFrom);
                if (CreatedDateTo == "")
                    dbManager.AddParameters(13, PARM_CREATED_DATE_TO, null);
                else
                    dbManager.AddParameters(13, PARM_CREATED_DATE_TO, CreatedDateTo);
                if (PageNumber == 0)
                    dbManager.AddParameters(14, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(14, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(15, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(15, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(16, PARM_RECORD_COUNT, ds.DBAudit.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                if (LookUpID == "- select -" || LookUpID == "" || LookUpID == null)
                    dbManager.AddParameters(17, PARM_AUDIT_LOOKUP_ID, null);
                else
                    dbManager.AddParameters(17, PARM_AUDIT_LOOKUP_ID, Convert.ToInt64(LookUpID));

                ds = (DSDBAudit)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DBAUDIT_SELECT, ds, ds.DBAudit.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DBActivityAudit::LoadDBAudit", PROC_DBAUDIT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSDBAudit LoadDBAuditSelectForMultiUsers(string ModuleName, ref DataTable UserIds, string ColumnKeyId, string ProfileName, string DBAuditId, ref DataTable PatientIds, string VisitId, DateTime? CreatedDate, string DBTableName = null, string ParentKeyColumnId = "", string ActionName = "", string CreatedDateFrom = "", string CreatedDateTo = "", int PageNumber = 1, int RowsPerPage = 1000, string LookUpID = null)
        {
            if (DBTableName == "Allergy")
                DBTableName = "Allergies";

            DSDBAudit ds = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(18);

                if (DBAuditId == "")
                    dbManager.AddParameters(0, PARM_DB_AUDIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DB_AUDIT_ID, Convert.ToInt64(DBAuditId));

                if (ModuleName == "")
                    dbManager.AddParameters(1, PARM_MODULE_NAME, null);
                else
                    dbManager.AddParameters(1, PARM_MODULE_NAME, ModuleName);

                    dbManager.AddParameters(2, PARM_USER_IDS, UserIds);

                if (ColumnKeyId == "")
                    dbManager.AddParameters(3, PARM_COLUMN_KEY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_COLUMN_KEY_ID, ColumnKeyId);

                if (ProfileName == "")
                    dbManager.AddParameters(4, PARM_PROFILE_NAME, null);
                else
                    dbManager.AddParameters(4, PARM_PROFILE_NAME, ProfileName);

                    dbManager.AddParameters(5, PARM_PATIENT_IDS, PatientIds);

                if (VisitId == "")
                    dbManager.AddParameters(6, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(6, PARM_VISIT_ID, Convert.ToInt32(VisitId));

                if (CreatedDate == null)
                    dbManager.AddParameters(7, PARM_CREATED_DATE, null);
                else
                    dbManager.AddParameters(7, PARM_CREATED_DATE, CreatedDate);
                if (string.IsNullOrEmpty(DBTableName))
                    dbManager.AddParameters(8, PARM_DB_TABLENAME, null);
                else
                    dbManager.AddParameters(8, PARM_DB_TABLENAME, DBTableName);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                if (ParentKeyColumnId == "")
                    dbManager.AddParameters(10, PARM_PARENT_COLMN_KEY_ID, null);
                else
                    dbManager.AddParameters(10, PARM_PARENT_COLMN_KEY_ID, Convert.ToInt64(ParentKeyColumnId));

                if (ActionName == "")
                    dbManager.AddParameters(11, PARM_DB_ACTION_NAME, null);
                else
                    dbManager.AddParameters(11, PARM_DB_ACTION_NAME, ActionName);

                if (CreatedDateFrom == "")
                    dbManager.AddParameters(12, PARM_CREATED_DATE_FROM, null);
                else
                    dbManager.AddParameters(12, PARM_CREATED_DATE_FROM, CreatedDateFrom);
                if (CreatedDateTo == "")
                    dbManager.AddParameters(13, PARM_CREATED_DATE_TO, null);
                else
                    dbManager.AddParameters(13, PARM_CREATED_DATE_TO, CreatedDateTo);
                if (PageNumber == 0)
                    dbManager.AddParameters(14, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(14, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(15, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(15, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(16, PARM_RECORD_COUNT, ds.DBAudit.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                if (LookUpID == "- select -" || LookUpID == "" || LookUpID == null)
                    dbManager.AddParameters(17, PARM_AUDIT_LOOKUP_ID, null);
                else
                    dbManager.AddParameters(17, PARM_AUDIT_LOOKUP_ID, Convert.ToInt64(LookUpID));

                ds = (DSDBAudit)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DBAUDIT_SELECT_FOR_MULTIUSERS, ds, ds.DBAudit.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DBActivityAudit::LoadDBAuditSelectForMultiUsers", PROC_DBAUDIT_SELECT_FOR_MULTIUSERS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<ActivityLogUser> LoadUserAuditReport(string UserType, ref DataTable UserIds, string FromDate, string ToDate, string AuditAction, int PageNumber = 1, int RowsPerPage = 1000)
        {
            List<ActivityLogUser> listobj = new List<ActivityLogUser>();

           
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                
                dbManager.Open();
                dbManager.CreateParameters(8);

                if (UserType == "- select -" || UserType == "")
                    dbManager.AddParameters(0, PARM_USER_TYPE, null);
                else                        
                    dbManager.AddParameters(0, PARM_USER_TYPE, UserType);

                    dbManager.AddParameters(1, PARM_USER_IDS, UserIds);

                if (FromDate == "")
                    dbManager.AddParameters(2, PARM_DATE_FROM, null);
                else
                    dbManager.AddParameters(2, PARM_DATE_FROM, FromDate);

                if (ToDate == "")
                    dbManager.AddParameters(3, PARM_DATE_TO, null);
                else
                    dbManager.AddParameters(3, PARM_DATE_TO, ToDate);

                if (AuditAction == "" || AuditAction == null)
                    dbManager.AddParameters(4, PARM_ACTION, null);
                else
                    dbManager.AddParameters(4, PARM_ACTION, AuditAction);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);


                dbManager.AddParameters(7, PARM_RECORD_COUNT,"", DbType.Int64, ParamDirection.Output);


                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DBAUDIT_USER);
                while (reader.Read())
                {
                    ActivityLogUser model1 = new ActivityLogUser();
                    var properties = typeof(ActivityLogUser).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    listobj.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAuditbleEventsActivityLog::loadAcitivityLogComponents", PROC_DBAUDIT_USER, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;
        }
        public DSDBAudit LoadDBAuditDetail(string ModuleName, string UserName, string ColumnKeyId, string ProfileName, string DBAuditId, string PatientId, string VisitId, DateTime? CreatedDate, string DBTableName = null, string ParentKeyColumnId = "", string ActionName = "", string CreatedDateFrom = "", string CreatedDateTo = "", int PageNumber = 1, int RowsPerPage = 1000)
        {

            DSDBAudit ds = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(17);

                if (DBAuditId == "")
                    dbManager.AddParameters(0, PARM_DB_AUDIT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DB_AUDIT_ID, Convert.ToInt64(DBAuditId));

                if (ModuleName == "")
                    dbManager.AddParameters(1, PARM_MODULE_NAME, null);
                else
                    dbManager.AddParameters(1, PARM_MODULE_NAME, ModuleName);

                if (UserName == "- select -" || UserName == "")
                    dbManager.AddParameters(2, PARM_USER_NAME, null);
                else
                    dbManager.AddParameters(2, PARM_USER_NAME, UserName);

                if (ColumnKeyId == "")
                    dbManager.AddParameters(3, PARM_COLUMN_KEY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_COLUMN_KEY_ID, ColumnKeyId);

                if (ProfileName == "")
                    dbManager.AddParameters(4, PARM_PROFILE_NAME, null);
                else
                    dbManager.AddParameters(4, PARM_PROFILE_NAME, ProfileName);

                if (PatientId == "" || PatientId == null)
                    dbManager.AddParameters(5, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PATIENT_ID, Convert.ToInt64(PatientId));

                if (VisitId == "")
                    dbManager.AddParameters(6, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(6, PARM_VISIT_ID, Convert.ToInt32(VisitId));

                if (CreatedDate == null)
                    dbManager.AddParameters(7, PARM_CREATED_DATE, null);
                else
                    dbManager.AddParameters(7, PARM_CREATED_DATE, CreatedDate);
                if (string.IsNullOrEmpty(DBTableName))
                    dbManager.AddParameters(8, PARM_DB_TABLENAME, null);
                else
                    dbManager.AddParameters(8, PARM_DB_TABLENAME, DBTableName);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(9, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(9, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                if (ParentKeyColumnId == "")
                    dbManager.AddParameters(10, PARM_PARENT_COLMN_KEY_ID, null);
                else
                    dbManager.AddParameters(10, PARM_PARENT_COLMN_KEY_ID, Convert.ToInt64(ParentKeyColumnId));

                if (ActionName == "")
                    dbManager.AddParameters(11, PARM_DB_ACTION_NAME, null);
                else
                    dbManager.AddParameters(11, PARM_DB_ACTION_NAME, ActionName);

                if (CreatedDateFrom == "")
                    dbManager.AddParameters(12, PARM_CREATED_DATE_FROM, null);
                else
                    dbManager.AddParameters(12, PARM_CREATED_DATE_FROM, CreatedDateFrom);
                if (CreatedDateTo == "")
                    dbManager.AddParameters(13, PARM_CREATED_DATE_TO, null);
                else
                    dbManager.AddParameters(13, PARM_CREATED_DATE_TO, CreatedDateTo);

                if (PageNumber == 0)
                    dbManager.AddParameters(14, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(14, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(15, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(15, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(16, PARM_RECORD_COUNT, ds.DBAudit.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);


                ds = (DSDBAudit)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DBAUDIT_Detail, ds, ds.DBAudit.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DBActivityAudit::LoadDBAuditDetail", PROC_DBAUDIT_Detail, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<LookupRoles> GetLookupRoles()
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<LookupRoles> LstLookupRole = new List<LookupRoles>();
            try
            {
                dbManager.Open();

                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_DBAUDIT_ROLES_LOOKUP_SELECT);
                while (reader.Read())
                {
                    LookupRoles model1 = new LookupRoles();
                    var properties = typeof(LookupRoles).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    LstLookupRole.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DBActivityAudit::GetLookupRoles", PROC_DBAUDIT_ROLES_LOOKUP_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return LstLookupRole;

        }

        public DSDBAudit InsertDBAuditOfPaymentCrossOver(DSPayment dsPayment, IDBManager dbManager)
        {
            try
            {
                DSDBAudit dsCross = GetDBAuditCrossOver(dsPayment.Tables[dsPayment.PatientPayments.TableName]);
                if (dsCross.DBAudit.Rows.Count > 0)
                {
                    this.CreateParameters(dbManager, dsCross);
                    dsCross = (DSDBAudit)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_DBAUDIT_INSERT, dsCross, dsCross.DBAudit.TableName);
                }
                return dsCross;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DBActivityAudit::InsertDBAudit", PROC_DBAUDIT_INSERT, ex);
                throw ex;
            }

        }

        public DSDBAudit GetDBAuditCrossOver(DataTable dt)
        {
            DSDBAudit.DBAuditDataTable dtDBAudit = new DSDBAudit.DBAuditDataTable();
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();
                //For Submitted By
                drActivity.OriginalValue = null;
                drActivity.CurrentValue = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                drActivity.ProfileName = "PatientVisits";
                drActivity.DBTableName = "PatientVisits";
                drActivity.ColumnName = "SubmittedBy";
                drActivity.DisplayName = "Submitted By";
                drActivity.DBAuditAction = "Update";
                drActivity.ModuleName = "Patient";
                drActivity.UserName = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                drActivity.IsCrossedOver = true;
                if (row.Table.Columns.Contains("PatientId"))
                {
                    string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                    if (Convert.ToDouble(patienID) > 0)
                        drActivity.PatientId = row["PatientId"].ToString();
                }
                if (row.Table.Columns.Contains("VisitId"))
                {
                    string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                    if (Convert.ToDouble(VisitID) > 0)
                    {
                        drActivity.VisitId = row["VisitId"].ToString();
                        drActivity.ColumnKeyId = row["VisitId"].ToString();
                    }
                }
                drActivity.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                drActivity.ColumnKeyName = "VisitId";
                dtDBAudit.Rows.Add(drActivity);

                //For Submitted Date
                DSDBAudit.DBAuditRow drSubmitteddate = dtDBAudit.NewDBAuditRow();
                drSubmitteddate.OriginalValue = null;
                drSubmitteddate.CurrentValue = DateTime.Now.ToString();
                drSubmitteddate.ProfileName = "PatientVisits";
                drSubmitteddate.DBTableName = "PatientVisits";
                drSubmitteddate.ColumnName = "SubmittedDate";
                drSubmitteddate.DisplayName = "Submitted Date";
                drSubmitteddate.DBAuditAction = "Update";
                drSubmitteddate.ModuleName = "Patient";
                drSubmitteddate.UserName = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                drSubmitteddate.IsCrossedOver = true;
                if (row.Table.Columns.Contains("PatientId"))
                {
                    string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                    if (Convert.ToDouble(patienID) > 0)
                        drSubmitteddate.PatientId = row["PatientId"].ToString();
                }
                if (row.Table.Columns.Contains("VisitId"))
                {
                    string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                    if (Convert.ToDouble(VisitID) > 0)
                    {
                        drSubmitteddate.VisitId = row["VisitId"].ToString();
                        drSubmitteddate.ColumnKeyId = row["VisitId"].ToString();
                    }
                }
                drSubmitteddate.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                drSubmitteddate.ColumnKeyName = "VisitId";
                drSubmitteddate.CreatedDate = DateTime.Now;
                dtDBAudit.Rows.Add(drSubmitteddate);

                //For ClaimStatus Id
                DSDBAudit.DBAuditRow drSubmittedStatus = dtDBAudit.NewDBAuditRow();
                drSubmittedStatus.OriginalValue = null;
                drSubmittedStatus.CurrentValue = "2";
                drSubmittedStatus.ProfileName = "PatientVisits";
                drSubmittedStatus.DBTableName = "PatientVisits";
                drSubmittedStatus.ColumnName = "ClaimStatusId";
                drSubmittedStatus.DisplayName = "ClaimStatus Id";
                drSubmittedStatus.DBAuditAction = "Update";
                drSubmittedStatus.ModuleName = "Patient";
                drSubmittedStatus.UserName = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                drSubmittedStatus.IsCrossedOver = true;
                if (row.Table.Columns.Contains("PatientId"))
                {
                    string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                    if (Convert.ToDouble(patienID) > 0)
                        drSubmittedStatus.PatientId = row["PatientId"].ToString();
                }
                if (row.Table.Columns.Contains("VisitId"))
                {
                    string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                    if (Convert.ToDouble(VisitID) > 0)
                    {
                        drSubmittedStatus.VisitId = row["VisitId"].ToString();
                    }
                }
                drSubmittedStatus.ColumnKeyId = row["VisitId"].ToString();
                drSubmittedStatus.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                drSubmittedStatus.ColumnKeyName = "VisitId";
                drSubmittedStatus.CreatedDate = DateTime.Now;
                dtDBAudit.Rows.Add(drSubmittedStatus);
                /*
                 //As per discussion with Salman Sab CrossOver history would not be maintaind for charges
                //For charge Status Id
                DSDBAudit.DBAuditRow drChargeStatus = dtDBAudit.NewDBAuditRow();
                drChargeStatus.OriginalValue = null;
                drChargeStatus.CurrentValue = "2";
                drChargeStatus.ProfileName = "PatientCharges";
                drChargeStatus.DBTableName = "PatientCharges";
                drChargeStatus.ColumnName = "StatusId";
                drChargeStatus.DisplayName = "Status Id";
                drChargeStatus.DBAuditAction = "Update";
                drChargeStatus.ModuleName = "Patient";
                drChargeStatus.UserName = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                drChargeStatus.IsCrossedOver = true;
                if (row.Table.Columns.Contains("PatientId"))
                {
                    string patienID = string.IsNullOrEmpty(row["PatientId"].ToString()) ? "0" : row["PatientId"].ToString();
                    if (Convert.ToDouble(patienID) > 0)
                        drChargeStatus.PatientId = row["PatientId"].ToString();
                }
                if (row.Table.Columns.Contains("VisitId"))
                {
                    string VisitID = string.IsNullOrEmpty(row["VisitId"].ToString()) ? "0" : row["VisitId"].ToString();
                    if (Convert.ToDouble(VisitID) > 0)
                    {
                        drChargeStatus.VisitId = row["VisitId"].ToString();
                    }
                }
				drChargeStatus.ColumnKeyId = row["ChargeId"].ToString();
                drChargeStatus.EntityId = Convert.ToInt64(SharedObj.EntityId);
                drChargeStatus.ColumnKeyName = "ChargeCapId";
                drChargeStatus.CreatedDate = DateTime.Now;
                dtDBAudit.Rows.Add(drChargeStatus);
                */
            }

            DSDBAudit dsDBAudit = new DSDBAudit();
            dsDBAudit.Merge(dtDBAudit);
            return dsDBAudit;
        }
        public DSDBAudit LoadDBAudit_NoteComments(string PatientId, string VisitId, string DBTableName = null)
        {

            DSDBAudit ds = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);


                if (PatientId == "" || PatientId == null)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, Convert.ToInt64(PatientId));

                if (VisitId == "")
                    dbManager.AddParameters(1, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VISIT_ID, Convert.ToInt32(VisitId));

                if (string.IsNullOrEmpty(DBTableName))
                    dbManager.AddParameters(2, PARM_DB_TABLENAME, null);
                else
                    dbManager.AddParameters(2, PARM_DB_TABLENAME, DBTableName);


                ds = (DSDBAudit)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DBAUDIT_NOTE_COMMENT_SELECT, ds, ds.DBAudit.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DBActivityAudit::LoadDBAudit_NoteComments", PROC_DBAUDIT_NOTE_COMMENT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string GetModelsDBAudit<T>(T newModel, T model, string columnKeyId,long parentKeyColumnId, string columnKeyName, string action, string patientId, string tableName)
        {
            try
            {
                string xmlDBAudit = "";

                DSDBAudit.DBAuditDataTable dtDBAudit = new DSDBAudit.DBAuditDataTable();
                string username = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                long EntityId = Convert.ToInt64(MDVSession.Current.EntityId);

                DSModuleForm ds = new DSModuleForm();
                ds = new DALModulesForms().LoadForms(0, "", "", tableName);

                if (ds.Forms.Rows.Count > 0)
                {
                    string ColumnsName = ds.Forms.Rows[0][ds.Forms.DBTableColumnsColumn.ColumnName].ToString();
                    string Action = ds.Forms.Rows[0][ds.Forms.DBActionColumn.ColumnName].ToString();
                    string dbTableName = ds.Forms.Rows[0][ds.Forms.DBTableNameColumn.ColumnName].ToString();
                    string FormName = ds.Forms.Rows[0][ds.Forms.NameColumn.ColumnName].ToString();                  
                    string ModuleName = ds.Forms.Rows[0][ds.Forms.DescriptionColumn.ColumnName].ToString();
                    if (string.IsNullOrEmpty(ModuleName))
                    {
                        ModuleName = ds.Forms.Rows[0][ds.Forms.ModuleNameColumn.ColumnName].ToString();
                    }                  
                    Dictionary<string, string> ColumnsDictionary = GetColumnsName(ColumnsName);

                    if (action == "Insert" || action == "Delete" || action == "View")
                    {
                        var oType = newModel.GetType();

                        foreach (var oProperty in oType.GetProperties())
                        {                            
                            var oValue = oProperty.GetValue(newModel, null);

                            if (ColumnsDictionary.ContainsKey(oProperty.Name))
                            {
                                var sValue = oValue == null ? "" : oValue.ToString();

                                DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();

                                drActivity.DBTableName = tableName;
                                drActivity.ProfileName = FormName;
                                drActivity.ColumnName = oProperty.Name;
                                drActivity.ColumnKeyId = columnKeyId;
                                drActivity.ColumnKeyName = columnKeyName;
                                if (action == "Insert")
                                {
                                    drActivity.CurrentValue = sValue;
                                    drActivity.DisplayName = ColumnsDictionary[oProperty.Name];
                                }
                                else if (action == "Delete")
                                {
                                    drActivity.OriginalValue = sValue;
                                    drActivity.CurrentValue = "";
                                    drActivity.DisplayName = ColumnsDictionary[oProperty.Name];
                                }
                                else if(action == "View")
                                {
                                    drActivity.OriginalValue = "";
                                    drActivity.CurrentValue = "";
                                    drActivity.DisplayName = "";
                                }
                                drActivity.DBAuditAction = action;
                                drActivity.ModuleName = ModuleName;
                                drActivity.UserName = username;
                                drActivity.CreatedDate = DateTime.Now;
                                drActivity.PatientId = patientId;
                                drActivity.EntityId = EntityId;
                                drActivity.ParentKeyColumnId = parentKeyColumnId;
                                dtDBAudit.Rows.Add(drActivity);
                            }
                        }
                    }
                    else if (action == "Update")
                    {
                        var oType = model.GetType();

                        foreach (var oProperty in oType.GetProperties())
                        {
                            var oOldValue = oProperty.GetValue(model, null);
                            var oNewValue = oProperty.GetValue(newModel, null);

                            if (ColumnsDictionary.ContainsKey(oProperty.Name) && !object.Equals(oOldValue, oNewValue))
                            {

                                var sOldValue = oOldValue == null ? "" : oOldValue.ToString();
                                var sNewValue = oNewValue == null ? "" : oNewValue.ToString();

                                DSDBAudit.DBAuditRow drActivity = dtDBAudit.NewDBAuditRow();

                                drActivity.DBTableName = tableName;
                                drActivity.ProfileName = FormName;
                                drActivity.ColumnName = oProperty.Name;
                                drActivity.DisplayName = ColumnsDictionary[oProperty.Name];
                                drActivity.ColumnKeyId = columnKeyId;
                                drActivity.ColumnKeyName = columnKeyName;
                                drActivity.OriginalValue = sOldValue;
                                drActivity.CurrentValue = sNewValue;
                                drActivity.DBAuditAction = action;
                                drActivity.ModuleName = ModuleName;
                                drActivity.UserName = username;
                                drActivity.CreatedDate = DateTime.Now;
                                drActivity.PatientId = patientId;
                                drActivity.EntityId = EntityId;
                                drActivity.ParentKeyColumnId = parentKeyColumnId;
                                dtDBAudit.Rows.Add(drActivity);
                            }
                        }
                    }                 

                    #region Create XML of Chnages for Audit Log               
                    XmlDocument xml = new XmlDocument();
                    XmlElement root = xml.CreateElement("DBAudits");
                    xml.AppendChild(root);
                    foreach (DataRow row in dtDBAudit.Rows)
                    {
                        XmlElement child = xml.CreateElement("DBAudit");
                        root.AppendChild(child);
                        for (int col = 0; col < row.ItemArray.Count(); col++)
                        {
                            XmlElement colElement = xml.CreateElement(row.Table.Columns[col].ToString());
                            colElement.InnerText = row.ItemArray[col].ToString();
                            child.AppendChild(colElement);
                        }
                    }
                    if (dtDBAudit.Rows.Count > 0)
                    {
                        xmlDBAudit = xml.OuterXml;
                    }
                    #endregion
                }

                return xmlDBAudit;

            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public void InsertModelsDBAudit(string xml) {
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                DSDBAudit ds = new DSDBAudit();
                if (!string.IsNullOrEmpty(xml))
                {
                    dbManager.Open();
                    dbManager.CreateParameters(1);
                    dbManager.AddParameters(0, PARM_XML_DATA, xml);
                    ds = (DSDBAudit)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DBAUDIT_INSERT_BULK, ds, ds.DBAuditIds.TableName);

                }
               ds.AcceptChanges();               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DSDBAudit InsertDBAuditAdmin(DataTable dt, IDBManager dbManager, string PrimaryKeyId, string ParentKeyColumnId = "", bool isViewAction = false, bool isPrintAction = false, bool isDeleteAction = false, SharedVariable shareVariable = null)
        {
            try
            {
                DSDBAudit ds = new DSDBAudit();
                string xml = "";
                if (dt == null)
                {
                    return new DSDBAudit();
                }
              
                xml = GetDBAuditAdmin(dt, PrimaryKeyId, ParentKeyColumnId, isViewAction, isPrintAction, isDeleteAction, shareVariable);
                if (!string.IsNullOrEmpty(xml))
                {
                    dbManager.CreateParameters(1);
                    dbManager.AddParameters(0, PARM_XML_DATA, xml);
                    ds = (DSDBAudit)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DBAUDIT_ADMIN_INSERT_BULK, ds, ds.DBAuditIds.TableName);

                }
                //ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                // MDVLogger.DALErrorLog("DBActivityAudit::InsertDBAudit", PROC_DBAUDIT_INSERT, ex);
                MDVLogger.SendExcepToDB(ex, "InsertDBAuditAdmin", PROC_DBAUDIT_ADMIN_INSERT_BULK);
                throw ex;
                //Usual code
            }

        }

        public DSDBAudit LoadPatientVisitsPrintInfo(string PatientId, string VisitId)
        {
            DSDBAudit ds = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (PatientId == "" || PatientId == null)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, Convert.ToInt64(PatientId));

                if (VisitId == "")
                    dbManager.AddParameters(1, PARM_VISIT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_VISIT_ID, Convert.ToInt32(VisitId));

                ds = (DSDBAudit)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOAD_PATIENTVISIT_PRINT_INFO, ds, ds.DT_PatientVisit_Print_Info.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DBActivityAudit::LoadPatientVisitsPrintInfo", PROC_LOAD_PATIENTVISIT_PRINT_INFO, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
    }
}
