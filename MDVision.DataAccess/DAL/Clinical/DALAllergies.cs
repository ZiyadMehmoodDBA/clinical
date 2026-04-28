using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.DataAccess.DCommon;
using System.ComponentModel;
using MDVision.Datasets;
using System.Data.SqlClient;
using MDVision.Model.Lookups;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Clinical.Medical.Allergy;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALAllergies
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_ALLERGY_INSERT = "Clinical.sp_AllergyInsert";
        private const string PROC_ALLERGY_UPDATE = "Clinical.sp_AllergyUpdate";
        private const string PROC_ALLERGY_DELETE = "Clinical.sp_AllergyDelete";
        private const string PROC_ALLERGY_SELECT = "Clinical.sp_AllergySelect";
        private const string PROC_ALLERGY_SELECT_OP = "Clinical.sp_CAllergy_AllergySelect";
        private const string PROC_SEVERITY_LOOKUP = "Clinical.sp_SeverityLookup";
        private const string PROC_ALLERGY_TYPE_LOOKUP = "Clinical.sp_AllergyTypeLookup";
        private const string PROC_GET_LATEST_ALLERGY_BY_PATIENTI = "Clinical.sp_GetLatestAllergyByPatientId";

        private const string PROC_GET_LATEST_ALLERGY_FORSOAPTEXT = "Clinical.sp_GetLatestAllergyForSoapText";

        private const string PROC_ALLERGY_SELECT_FORSOAPTEXT = "Clinical.sp_AllergiesSelectForSoapText";
        private const string PROC_ALLERGY_SELECT_FORSOAPTEXT_OP1 = "Clinical.sp_CAllergy_AllergiesSelectForSoapText";
        private const string PROC_DETACH_ALLERGY_FROM_NOTES = "Clinical.sp_DetachAllergyFromNotes";
        private const string PROC_ATTACH_ALLERGY_FROM_NOTES = "Clinical.sp_AttachAllergyWithNotes";
        private const string PROC_ATTACH_ALLERGY_FROM_NOTES_OP = "Clinical.sp_CAllergy_AttachAllergyWithNotes";

        private const string PROC_INSERT_LASTUPDATEPATIENTINFO = "Clinical.sp_Rcopia_PatientLastUpdateInfoInsert";

        private const string PROC_ALLERGY_HISTORY_SELECT = "Clinical.sp_AllergyHistorySelect";
        private const string PROC_INSERT_ALLERGY_REVIEW = "Clinical.sp_AllergyReviewInsert";
        private const string PROC_ALLERGY_REVIEW_SELECT = "Clinical.sp_AllergyReviewSelect";

        private const string PROC_ALLERGIES_LOOKUP = "Clinical.sp_AllergyLookup";
        private const string GET_ALLERGY_BY_RCOPIAID = "Clinical.sp_GetAllergyAgainstRcopiaID";
        private const string PROC_ALLERGIES_REPORTS_LOOKUP = "Clinical.sp_AllergyLookup";
        private const string PROC_REACTIONS_REPORTS_LOOKUP = "Clinical.sp_ReactionLookup";

        private const string PROC_ALLERGY_SELECT_FOR_CDSAllergies = "Clinical.[sp_LookupAllergies]";
        private const string PROC_ALLERGY_SELECT_FOR_CDS = "Clinical.sp_Lookup";
        private const string PROC_All_ALLERGY_SELECT = "Clinical.sp_AllergyAllSelect";

        private const string PROC_NOTES_ALLERGY_SELECT = "[Clinical].[sp_NotesAllergySelect]";


        #endregion

        #region "Parameters"

        private const string PARM_ALLERGY_ID = "@AllergyId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_ALLERGEN = "@Allergen";
        private const string PARM_TYPE = "@Type";
        private const string PARM_REACTION = "@Reaction";
        private const string PARM_SEVERITY = "@Severity";
        private const string PARM_ONSET_DATE = "@OnSetDate";
        private const string PARM_LAST_MODIFIED = "@LastModified";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_INACTIVE_CHECKBOXVALUE = "@InActiveCheckBoxValue";
        private const string PARM_INACTIVE_REASON = "@InActiveReason";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_IS_HISTORY = "@IsHistory";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_VISIT_ID = "@VisitId";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_CREATED_BY = "@CreatedBy";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_RCOPIAID = "@RcopiaID";
        private const string PARM_ISDELETED = "@IsDeleted";
        private const string PARM_AllergyLastUpdateDate = "@AllergyLastUpdateDate";
        private const string PARM_R_PLASTUPDATEINFO = "@R_PLastUpdateInfo";

        private const string PARM_MODIFY_ON = "@ModifiedOn";
        private const string PARM_CREATED_ON = "@CreatedOn";

        private const string LOOKUP_TYPE = "@LookupType";

        private const string PARM_ALLERGY_REVIEW_ID = "@AllergyReviewId";
        private const string PARM_REVIEWED_BY = "@ReviewedBy";
        private const string PARM_REVIEWED_ON = "@ReviewedOn";
        private const string PARM_RXNORMID = "@RxnormID";
        private const string PARM_RXNORMIDTYPE = "@RxnormIDType";

        private const string PARM_ISNEWROW = "@IsNewRow";





        #endregion

        #region Constructors
        public DALAllergies()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }

        public DALAllergies(SharedVariable SharedVariable)
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

        #region "Support Functions For Allergies"

        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">Is insert ?</param>
        private void CreateParameters(IDBManager dbManager, DSAllergies ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(20);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ALLERGY_ID, ds.Allergy.AllergyIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ALLERGY_ID, ds.Allergy.AllergyIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_ALLERGEN, ds.Allergy.AllergenColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_TYPE, ds.Allergy.TypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_REACTION, ds.Allergy.ReactionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_SEVERITY, ds.Allergy.SeverityColumn.ColumnName, DbType.String);

            dbManager.AddParameters(5, PARM_ONSET_DATE, ds.Allergy.OnSetDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_LAST_MODIFIED, ds.Allergy.LastModifiedColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_IS_ACTIVE, ds.Allergy.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_COMMENTS, ds.Allergy.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_NOTE_ID, ds.Allergy.NoteIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(10, PARM_INACTIVE_CHECKBOXVALUE, ds.Allergy.InActiveCheckBoxValueColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_INACTIVE_REASON, ds.Allergy.InActiveReasonColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_PATIENT_ID, ds.Allergy.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.Allergy.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CREATED_BY, ds.Allergy.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_RCOPIAID, ds.Allergy.RcopiaIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_ISDELETED, ds.Allergy.IsDeletedColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_RXNORMID, ds.Allergy.RxnormIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_RXNORMIDTYPE, ds.Allergy.RxnormIDTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_ISNEWROW, ds.Allergy.IsNewRowColumn.ColumnName, DbType.Byte, ParamDirection.Output);

        }

        private void CreateParametersForLastUpdatePatientInfo(IDBManager dbManager, DSRcopia ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(8);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_R_PLASTUPDATEINFO, ds.Rcopia_PatientLastUpdateInfo.R_PLastUpdateInfoColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_R_PLASTUPDATEINFO, ds.Rcopia_PatientLastUpdateInfo.R_PLastUpdateInfoColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.Rcopia_PatientLastUpdateInfo.PatientIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_AllergyLastUpdateDate, ds.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn.ColumnName, DbType.DateTime);

            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.Rcopia_PatientLastUpdateInfo.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_MODIFIED_BY, ds.Rcopia_PatientLastUpdateInfo.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.Rcopia_PatientLastUpdateInfo.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.Rcopia_PatientLastUpdateInfo.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFY_ON, ds.Rcopia_PatientLastUpdateInfo.ModifiedOnColumn.ColumnName, DbType.DateTime);


        }

        private void CreateParametersForInsertAllergyReviews(IDBManager dbManager, DSAllergies ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(4);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ALLERGY_REVIEW_ID, ds.AllergyReview.AllergyReviewIDColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_ALLERGY_REVIEW_ID, ds.AllergyReview.AllergyReviewIDColumn.ColumnName, DbType.Int64);


            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.AllergyReview.PatientIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(2, PARM_REVIEWED_BY, ds.AllergyReview.ReviewedByColumn.ColumnName, DbType.String);

            dbManager.AddParameters(3, PARM_REVIEWED_ON, ds.AllergyReview.ReviewedOnColumn.ColumnName, DbType.DateTime);



        }
        #endregion

        #region "LastUpdatePatientInfo"
        public DSRcopia InsertLastUpdatePatientInfo(DSRcopia ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForLastUpdatePatientInfo(dbManager, ds, true);
                ds = (DSRcopia)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSERT_LASTUPDATEPATIENTINFO, ds, ds.Rcopia_PatientLastUpdateInfo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::insertAllergies", PROC_ALLERGY_INSERT, ex);
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
        #endregion

        #region "Allergies"
        //Start//01/12/2015//Ahmad Raza//method to Add Allergy
        public DSAllergies insertAllergies(DSAllergies ds, SharedVariable sharedVariable = null)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Allergy.GetChanges();

                //1)Get Old data by AllergyID
                //-----------------------------------
                DSAllergies dsOldDataAllergy = new DSAllergies();

                for (int i = 0; i < ds.Allergy.Rows.Count; i++)
                {
                    string RcopiaID = ds.Allergy.Rows[i]["RcopiaID"].ToString();
                    if (RcopiaID != "")
                    {
                        var asas = getAllergyByRcopiaID(RcopiaID);
                        dsOldDataAllergy.Merge(asas);
                        //dsOldDataAllergy.Allergy.Merge(getAllergyByRcopiaID(Convert.ToInt64(RcopiaID)).Tables["Allergy"]);
                    }
                }

                DataTable dt = dsOldDataAllergy.Tables["Allergy"];

                //-----------------------------------
                //dbManager.Open();
                dbManager.BeginTransaction();
                CreateParameters(dbManager, ds, true);
                ds = (DSAllergies)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_ALLERGY_INSERT, ds, ds.Allergy.TableName);
                DataRow row;
                for (int i = 0; i < ds.Allergy.Rows.Count; i++)
                {
                    if (Boolean.Parse((ds.Allergy.Rows[i]["IsNewRow"]).ToString()) == true)
                    {
					//Start 07-11-2016 Humaira Yousaf for dbaudit
                        if (!string.Equals(ds.Allergy.Rows[i]["Allergen"], "No Known Allergies"))
                        {
                            row = dt.NewRow();
                            row = ds.Allergy.Rows[i];
                            dt.Rows.Add(row.ItemArray);
                        }
					//End 07-11-2016 Humaira Yousaf for dbaudit
                    }
                    else if (Boolean.Parse((ds.Allergy.Rows[i]["IsNewRow"]).ToString()) == false)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dt.Rows[j]["RcopiaID"].ToString() == ds.Allergy.Rows[i]["RcopiaID"].ToString())
                            {
                                dt.Rows[j]["RcopiaID"] = ds.Allergy.Rows[i]["RcopiaID"];
                                dt.Rows[j]["OnSetDate"] = ds.Allergy.Rows[i]["OnSetDate"];
                                dt.Rows[j]["Allergen"] = ds.Allergy.Rows[i]["Allergen"];
                                dt.Rows[j]["Reaction"] = ds.Allergy.Rows[i]["Reaction"];
                                dt.Rows[j]["IsActive"] = ds.Allergy.Rows[i]["IsActive"];
                                dt.Rows[j]["IsDeleted"] = ds.Allergy.Rows[i]["IsDeleted"];
                            }
                        }

                    }
                }
                //dtTemp will be old..
                dtTemp = ds.Allergy.GetChanges();
                if (dt != null)
                {
                    //for (int i = 0; i < ds.Allergy.Rows.Count; i++)
                    //{

                    dt.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.Allergy.Rows.Count; i++)
                    {
					  //Start 07-11-2016 Humaira Yousaf for dbaudit
                        if (!string.Equals(ds.Allergy.Rows[i]["Allergen"], "No Known Allergies"))
                        {
                            dt.Rows[i]["PrimaryKey"] = ds.Allergy.Rows[i][ds.Allergy.AllergyIdColumn];
                        }
   				      //End 07-11-2016 Humaira Yousaf for dbaudit
                    }
                    if (sharedVariable == null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dt, dbManager, ds.Allergy.Rows[0][ds.Allergy.AllergyIdColumn].ToString());
                    }
                    else
                    {
                        dsDBAudit = new DBActivityAudit(sharedVariable).InsertDBAudit(dt, dbManager, ds.Allergy.Rows[0][ds.Allergy.AllergyIdColumn].ToString(), null, "", false, false, false, "", "0", sharedVariable);
                    }
                    
                    dsDBAudit.AcceptChanges();
                    //}
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                //MDVLogger.DALErrorLog("DALAllergies::insertAllergies", PROC_ALLERGY_INSERT, ex);
                MDVLogger.SendExcepToDB(ex, "DALAllergies::insertAllergies", null);
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
        //End//01/12/2015//Ahmad Raza//method to Add Allergy

        //Start//01/12/2015//Ahmad Raza//method to update Allergy
        public DSAllergies updateAllergies(DSAllergies ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.Allergy.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSAllergies)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_ALLERGY_UPDATE, ds, ds.Allergy.TableName);
                if (dtTemp != null)
                {
                    for (int i = 0; i < ds.Allergy.Rows.Count; i++)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Allergy.Rows[i][ds.Allergy.AllergyIdColumn].ToString());
                        dsDBAudit.AcceptChanges();
                    }
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALAllergies::updateAllergies", PROC_ALLERGY_UPDATE, ex);
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
        //End//01/12/2015//Ahmad Raza//method to update Allergy

        //Start//01/12/2015//Ahmad Raza//method to delete allergy
        public string deleteAllergies(string AllergyId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSAllergies ds = loadAllergies_Obsolete(Convert.ToInt64(AllergyId), 0, 0, "0", "", 1, 1000);
                DataTable dtTemp = ds.Allergy;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ALLERGY_ID, AllergyId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ALLERGY_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Allergy.Rows[0][ds.Allergy.AllergyIdColumn].ToString(), null, "", false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                }
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALAllergies::deleteAllergies", PROC_ALLERGY_DELETE, ex);
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
        //End//01/12/2015//Ahmad Raza//method to delete allergy

        //Start//01/12/2015//Ahmad Raza//method to load allergies for grid
        public DSAllergies loadAllergies_Obsolete(long allergyId, long patientId, long NoteId, string isHistory = "0", string active = "", int pageNumber = 1, int rowsPerPage = 1000, string isViewAllergy = "", string isPrintAllergy = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSAllergies ds = new DSAllergies();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "")
                    active = null;

                DataTable dtTemp = ds.Allergy;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(8);

                if (allergyId == 0)
                    dbManager.AddParameters(0, PARM_ALLERGY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ALLERGY_ID, allergyId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);

                dbManager.AddParameters(2, PARM_IS_HISTORY, "0");
                if (NoteId == 0)
                    dbManager.AddParameters(3, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_NOTE_ID, NoteId);

                dbManager.AddParameters(4, PARM_IS_ACTIVE, active);
                if (pageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.Allergy.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLERGY_SELECT, ds, ds.Allergy.TableName);

                if (isHistory == "1")
                {
                    dbManager.AddParameters(2, PARM_IS_HISTORY, isHistory);
                    DSAllergies dsAllergyHistory = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLERGY_SELECT, ds, ds.AllergyHistory.TableName);
                    if (dsAllergyHistory != null)
                    {
                        ds.Merge(dsAllergyHistory);
                    }
                }
                if (ds.Allergy.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.Allergy.Rows[0]["AllergyId"]) > 0)
                    {
                        if (dtTemp != null)
                        {
                            if (isViewAllergy == "1" || isPrintAllergy == "1")
                            {
                                bool isViewAction = isViewAllergy == "1" ? true : false;
                                bool isPrintAcion = isPrintAllergy == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Allergy.Rows[0][ds.Allergy.AllergyIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALAllergies::loadAllergies", PROC_ALLERGY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSAllergies loadAllAllergies_Obsolete(long allergyId, long patientId, long NoteId, string isHistory = "0", string active = "",  string isViewAllergy = "", string isPrintAllergy = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSAllergies ds = new DSAllergies();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "")
                    active = null;

                DataTable dtTemp = ds.Allergy;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(5);

                if (allergyId == 0)
                    dbManager.AddParameters(0, PARM_ALLERGY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ALLERGY_ID, allergyId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);

                dbManager.AddParameters(2, PARM_IS_HISTORY, "0");
                if (NoteId == 0)
                    dbManager.AddParameters(3, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(3, PARM_NOTE_ID, NoteId);

                dbManager.AddParameters(4, PARM_IS_ACTIVE, active);
              

                ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_All_ALLERGY_SELECT, ds, ds.Allergy.TableName);

                if (isHistory == "1")
                {
                    dbManager.AddParameters(2, PARM_IS_HISTORY, isHistory);
                    DSAllergies dsAllergyHistory = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_All_ALLERGY_SELECT, ds, ds.AllergyHistory.TableName);
                    if (dsAllergyHistory != null)
                    {
                        ds.Merge(dsAllergyHistory);
                    }
                }
                if (ds.Allergy.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.Allergy.Rows[0]["AllergyId"]) > 0)
                    {
                        if (dtTemp != null)
                        {
                            if (isViewAllergy == "1" || isPrintAllergy == "1")
                            {
                                bool isViewAction = isViewAllergy == "1" ? true : false;
                                bool isPrintAcion = isPrintAllergy == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Allergy.Rows[0][ds.Allergy.AllergyIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALAllergies::loadAllAllergies", PROC_All_ALLERGY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAllergies loadAllergiesOp_Obsolete(long patientId, long NoteId, string isHistory = "0", string active = "", int pageNumber = 1, int rowsPerPage = 1000, string isViewAllergy = "", string isPrintAllergy = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSAllergies ds = new DSAllergies();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "")
                    active = null;

                DataTable dtTemp = ds.Allergy;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(7);


                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);

                dbManager.AddParameters(1, PARM_IS_HISTORY, "0");
                if (NoteId == 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, NoteId);

                dbManager.AddParameters(3, PARM_IS_ACTIVE, active);
                if (pageNumber == 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(5, PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.Allergy.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLERGY_SELECT_OP, ds, ds.Allergy.TableName);
                if (ds.Allergy.Rows.Count > 0)
                {
                    if (isHistory == "1")
                    {
                        dbManager.AddParameters(1, PARM_IS_HISTORY, isHistory);
                        DSAllergies dsAllergyHistory = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLERGY_SELECT_OP, ds, ds.AllergyHistory.TableName);
                        if (dsAllergyHistory != null)
                        {
                            ds.Merge(dsAllergyHistory);
                        }
                    }
                }
                if (isViewAllergy == "1" && ds.Allergy.Rows.Count > 0 )
                {
                    if (Convert.ToInt64(ds.Allergy.Rows[0]["AllergyId"]) > 0)
                    {
                        if (dtTemp != null)
                        {
                            if (isViewAllergy == "1" || isPrintAllergy == "1")
                            {
                                bool isViewAction = isViewAllergy == "1" ? true : false;
                                bool isPrintAcion = isPrintAllergy == "1" ? true : false;

                                //Start 26-10-2016 Humaira Yousaf to add view action log against each item
                                dtTemp.Columns.Add("PrimaryKey");
                                dtTemp.Columns.Add("ViewAction");

                                for (int i = 0; i < ds.Allergy.Rows.Count; i++)
                                {
                                    dtTemp.Rows[i]["PrimaryKey"] = ds.Allergy.Rows[i][ds.Allergy.AllergyIdColumn];
                                    dtTemp.Rows[i]["ViewAction"] = "View";
                                }
                                //End 26-10-2016 Humaira Yousaf to add view action log against each item

                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Allergy.Rows[0][ds.Allergy.AllergyIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALAllergies::loadAllergies", PROC_ALLERGY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        
        public Tuple<List<AllergyReaderModel>, List<AllergyHistoryModel>> loadAllergiesOp(long patientId, long NoteId, string isHistory = "0", string active = "", int pageNumber = 1, int rowsPerPage = 1000, string isViewAllergy = "", string isPrintAllergy = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();

            List<AllergyReaderModel> allergyList = new List<AllergyReaderModel>();
            List<AllergyHistoryModel> allergyHistoryList = new List<AllergyHistoryModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (active == "")
                    active = null;

                dbManager.Open();
                dbManager.BeginTransaction();


                if (patientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, patientId);

                dbManager.AddParameters(PARM_IS_HISTORY, "0");
                if (NoteId == 0)
                    dbManager.AddParameters(PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(PARM_NOTE_ID, NoteId);

                dbManager.AddParameters(PARM_IS_ACTIVE, active);
                if (pageNumber == 0)
                    dbManager.AddParameters(PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(PARM_RECORD_COUNT, 0, DbType.Int64, ParamDirection.Output);

                allergyList = dbManager.ExecuteReaderMapper<AllergyReaderModel>(PROC_ALLERGY_SELECT_OP);

                if (allergyList.Count > 0)
                {
                    if (isHistory == "1")
                    {
                        dbManager.AddUpdateParameterValue(PARM_IS_HISTORY, isHistory);
                        allergyHistoryList = dbManager.ExecuteReaderMapper<AllergyHistoryModel>(PROC_ALLERGY_SELECT_OP);
                    }
                }
                //if (isViewAllergy == "1" && ds.Allergy.Rows.Count > 0)
                //{
                //    if (Convert.ToInt64(ds.Allergy.Rows[0]["AllergyId"]) > 0)
                //    {
                //        if (dtTemp != null)
                //        {
                //            if (isViewAllergy == "1" || isPrintAllergy == "1")
                //            {
                //                bool isViewAction = isViewAllergy == "1" ? true : false;
                //                bool isPrintAcion = isPrintAllergy == "1" ? true : false;

                //                //Start 26-10-2016 Humaira Yousaf to add view action log against each item
                //                dtTemp.Columns.Add("PrimaryKey");
                //                dtTemp.Columns.Add("ViewAction");

                //                for (int i = 0; i < ds.Allergy.Rows.Count; i++)
                //                {
                //                    dtTemp.Rows[i]["PrimaryKey"] = ds.Allergy.Rows[i][ds.Allergy.AllergyIdColumn];
                //                    dtTemp.Rows[i]["ViewAction"] = "View";
                //                }
                //                //End 26-10-2016 Humaira Yousaf to add view action log against each item

                //                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.Allergy.Rows[0][ds.Allergy.AllergyIdColumn].ToString(), null, "", isViewAction, isPrintAcion, false);
                //                dsDBAudit.AcceptChanges();
                //            }
                //        }
                //    }
                //}
                Tuple<List<AllergyReaderModel>, List<AllergyHistoryModel>> tuple = new Tuple<List<AllergyReaderModel>, List<AllergyHistoryModel>>(allergyList, allergyHistoryList);
                dbManager.CommitTransaction();
                return tuple;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALAllergies::loadAllergies", PROC_ALLERGY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAllergies loadAllergiesforCDS(string SearchAllergen)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSAllergies ds = new DSAllergies();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                DataTable dtTemp = ds.Allergy;
                //dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                if (!string.IsNullOrEmpty(SearchAllergen))
                {
                    dbManager.AddParameters(0, "@SearchAllergen", SearchAllergen);

                    ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLERGY_SELECT_FOR_CDSAllergies, ds, ds.Allergy.TableName);
                }
                else
                {
                    dbManager.AddParameters(0, LOOKUP_TYPE, "Allergen");

                    ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLERGY_SELECT_FOR_CDS, ds, ds.Allergy.TableName);
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALAllergies::loadAllergiesforCDS", PROC_ALLERGY_SELECT_FOR_CDS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<AllergiesModel> getNoteAllergiesByPatientId(long PatientId, long userId, long entityId,long NoteId)
        {
            try
            {
                IDBManager dbManager = ClientConfiguration.GetDBManager();
                //List<SqlParameter> parameters = new List<SqlParameter>();

                //parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                //parameters.Add(new SqlParameter(PARM_USER_ID, userId));
                dbManager.AddParameters(PARM_USER_ID, userId);
                //parameters.Add(new SqlParameter(PARM_ENTITY_ID, entityId));
                dbManager.AddParameters(PARM_ENTITY_ID, entityId);
                //parameters.Add(new SqlParameter(PARM_NOTE_ID, NoteId));
                dbManager.AddParameters(PARM_NOTE_ID, NoteId);

                return dbManager.ExecuteReaders<AllergiesModel>(PROC_GET_LATEST_ALLERGY_FORSOAPTEXT);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::getLatestProblemListByPatientId", PROC_GET_LATEST_ALLERGY_FORSOAPTEXT, ex);
                throw ex;
            }
        }

        public DSAllergies getLatestAllergiesByPatientId(long PatientId, string AllergyIds, long userId, long entityId)
        {
            DSAllergies ds = new DSAllergies();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);


                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);
                if (AllergyIds == "")
                    dbManager.AddParameters(1, PARM_ALLERGY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ALLERGY_ID, AllergyIds);
                if (userId <= 0)
                    dbManager.AddParameters(2, PARM_USER_ID, null);
                else
                    dbManager.AddParameters(2, PARM_USER_ID, userId);
                if (entityId <= 0)
                    dbManager.AddParameters(3, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(3, PARM_ENTITY_ID, entityId);

                ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_LATEST_ALLERGY_BY_PATIENTI, ds, ds.AllergySoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::getLatestProblemListByPatientId", PROC_GET_LATEST_ALLERGY_BY_PATIENTI, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Allergy Lookups

        /// Author: ZeeshanAK
        /// Purpose:  to load Allergies for Batch Patient List
        /// Date : April 06, 2016
        public DSAllergies LookupAllergies(int patientId, int allergyId = -1, string allergen = null)
        {
            DSAllergies ds = new DSAllergies();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (patientId == 0)
                {
                    dbManager.AddParameters(0, PARM_PATIENT_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                }
                if (allergyId <= 0)
                {
                    dbManager.AddParameters(1, PARM_ALLERGY_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_ALLERGY_ID, allergyId);
                }

                dbManager.AddParameters(2, PARM_ALLERGEN, allergen);
                ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLERGIES_LOOKUP, ds, ds.Allergy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadAllergies", PROC_ALLERGIES_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// Author: Azhar Shahzad
        /// Purpose:  to load Allergies for Clinical Reports
        /// Date : August 29, 2016
        public List<AllergyLookupModel> getAllAllergiesforReports()
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<AllergyLookupModel> listModel = new List<AllergyLookupModel>();
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ALLERGIES_REPORTS_LOOKUP);
                AllergyLookupModel model = null;
                while (reader.Read())
                {
                    model = new AllergyLookupModel();
                    model.AllergyId = Convert.ToInt64(reader["AllergyId"]);
                    model.Allergen = Convert.ToString(reader["Allergen"]);
                    listModel.Add(model);
                }
                return listModel;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::getAllAllergiesforReports", PROC_ALLERGIES_REPORTS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<ReactionLookupModel> getAllReactionsforReports()
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<ReactionLookupModel> listModel = new List<ReactionLookupModel>();
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_REACTIONS_REPORTS_LOOKUP);
                ReactionLookupModel model = null;
                while (reader.Read())
                {
                    model = new ReactionLookupModel();
                    model.ReactionId = Convert.ToInt64(reader["ReactionId"]);
                    model.Reaction = Convert.ToString(reader["Reaction"]);
                    listModel.Add(model);
                }
                return listModel;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::getAllReactionsforReports", PROC_REACTIONS_REPORTS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //Start//02/12/2015//Lookup for Severity Dropdown List
        public DSAllergyLookup lookupSeverity()
        {
            DSAllergyLookup ds = new DSAllergyLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSAllergyLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SEVERITY_LOOKUP, ds, ds.Severity.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::lookupSeverity", PROC_SEVERITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        //End//02/12/2015//Lookup for Severity Dropdown List

        //Start//02/12/2015//Lookup for AllergyType Dropdown List
        public DSAllergyLookup lookupAllergyType()
        {
            DSAllergyLookup ds = new DSAllergyLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSAllergyLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLERGY_TYPE_LOOKUP, ds, ds.AllergyType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::lookupAllergyType", PROC_ALLERGY_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        //End//02/12/2015//Lookup for AllergyType Dropdown List

        #endregion

        #region "Allergies For Notes Soap"


        /// <summary>
        /// getting Dataset of Allergies Soap Text
        /// </summary>
        /// <param name="AllergyId"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public DSAllergies loadAllergiesForSoap(string allergyId)
        {
            DSAllergies ds = new DSAllergies();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (string.IsNullOrEmpty(allergyId))
                    dbManager.AddParameters(0, PARM_ALLERGY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ALLERGY_ID, allergyId);


                ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLERGY_SELECT_FORSOAPTEXT_OP1, ds, ds.AllergySoap.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::loadAllergiesForSoap", PROC_ALLERGY_SELECT_FORSOAPTEXT_OP1, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion


        #region Notes and Allergies
        /// <summary>
        /// Detaching Allergies With Progress notes
        /// </summary>
        /// <param name="AllergyId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachAllergiesFromNotes(string AllergyId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ALLERGY_ID, AllergyId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_ALLERGY_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::detachAllergiesFromNotes", PROC_DETACH_ALLERGY_FROM_NOTES, ex);
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
        /// Attaching Allergies With Progress notes
        /// </summary>
        /// <param name="AllergyId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSAllergies attachAllergiesWithNotes(string AllergyId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSAllergies ds = new DSAllergies();

                dbManager.Open();

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ALLERGY_ID, AllergyId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_ALLERGY_FROM_NOTES_OP, ds, ds.Allergy.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALAllergies::attachAllergiesFromNotes", PROC_ATTACH_ALLERGY_FROM_NOTES_OP, ex);
                MDVLogger.SendExcepToDB(ex, "DALAllergies::attachAllergiesFromNotes", null);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region allergy Review
        public DSAllergies InsertAllergyReviews(DSAllergies ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForInsertAllergyReviews(dbManager, ds, true);
                ds = (DSAllergies)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_INSERT_ALLERGY_REVIEW, ds, ds.AllergyReview.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::InsertAllergyReviews", PROC_INSERT_ALLERGY_REVIEW, ex);
                MDVLogger.SendExcepToDB(ex, "DALMedications::InsertAllergyReviews", null);
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
        #endregion

        /// Author: ZeeshanAK
        /// Purpose:  Load Allergies Reviewed by 
        /// Date : February 09, 2016
        #region "Load Allergies Reviewed by"
        public DSAllergies loadAllergiesReviewedBy_Obsolete(long patientId)
        {
            DSAllergies ds = new DSAllergies();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);


                ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLERGY_REVIEW_SELECT, ds, ds.AllergyReview.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadAllergiesReviewedBy", PROC_ALLERGY_REVIEW_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<AllergyReviewModel> loadAllergiesReviewedBy(long patientId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();


                if (patientId == 0)
                    dbManager.AddParameters(PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(PARM_PATIENT_ID, patientId);


                List<AllergyReviewModel> allergyReviewList = dbManager.ExecuteReaderMapper<AllergyReviewModel>(PROC_ALLERGY_REVIEW_SELECT);
                return allergyReviewList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMedications::loadAllergiesReviewedBy", PROC_ALLERGY_REVIEW_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        //Get Allergy by RcopiaID
        public DSAllergies getAllergyByRcopiaID(string RcopiaID)
        {
            DSAllergies ds = new DSAllergies();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (RcopiaID == "")
                    dbManager.AddParameters(0, PARM_RCOPIAID, DBNull.Value);
                else
                    dbManager.AddParameters(0, PARM_RCOPIAID, RcopiaID);

                ds = (DSAllergies)dbManager.ExecuteDataSet(CommandType.StoredProcedure, GET_ALLERGY_BY_RCOPIAID, ds, ds.Allergy.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::getAllergyByRcopiaID", GET_ALLERGY_BY_RCOPIAID, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion


        #region Legacy Notes

        public List<AllergyHx> NotesAllergyHxSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<AllergyHx> objList_AllergyHx = new List<AllergyHx>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_ALLERGY_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        AllergyHx model = new AllergyHx();
                        var properties = typeof(AllergyHx).GetProperties();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }

                        objList_AllergyHx.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALAllergies::NotesAllergyHxSelect", PROC_NOTES_ALLERGY_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_AllergyHx;
        }

        #endregion Legacy Notes


    }
}
