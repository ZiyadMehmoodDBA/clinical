using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Model.Clinical.AOETemplates;
using System.Data.SqlClient;
namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALProcedureTemplate
    {
        #region " Variable "

        #endregion

        #region " Stored Procedure Names "
        private const string PROC_Procedure_TEMPLATESAVE = "Clinical.sp_CreateNewProcedureTemplate";
        private const string PROC_Procedure_TEMPLATESSELECT = "Clinical.sp_ProcedureTemplateSelect";
        private const string PROC_Procedure_TEMPLATESActiveInactive = "[Clinical].[sp_ProcedureTemplateUpdateActiveInActive]";
        private const string PROC_PROC_Procedure_TEMP_SYS_OBSERVATION_SELECT = "Clinical.sp_ProcedureTempSysObservationSelect";
        private const string PROC_Procedure_SYS_OBSERS_SELECT = "clinical.sp_ProcedureNotesSystemObservationSelect";
        private const string PROC_ProcedureOrder_SYS_OBSERS_SELECT = "clinical.sp_ProcedureOrderNotesSystemObservationSelect";
        private const string PROC_Procedure_NOTES_SYS_OBSERS_DESC_UPDATE = "clinical.sp_ProcedureTemplateAOENotesSystemObservationDescUpdate";
        private const string PROC_ProcedureOrder_NOTES_SYS_OBSERS_DESC_UPDATE = "clinical.sp_ProcedureOrderTemplateAOENotesSystemObservationDescUpdate";
        private const string PROC_Procedure_SYS_OBSERS_RADIOLOGY_SELECT = "clinical.sp_ProcedureNotesSystemRadiologyObservationSelect";
        private const string PROC_Procedure_NOTES_SYS_OBSERS_RADIOLOGY_DESC_UPDATE = "clinical.sp_ProcedureNotesSystemRadiologyObservationDescUpdate";
        private const string PROC_Procedure_SYSTEM_OBSERVATION_INSERT = "Clinical.sp_ProcedureSystemObservationInsert";
        private const string PROC_ProcedureTemplate_DELETE = "Clinical.sp_ProcedureTemplateDelete";
        private const string PROC_ProcedureTemplateTests_SELECT = "[Clinical].[sp_ProcedureTemplateTestsSelect]";
        #endregion

        #region " Constructors "
        public DALProcedureTemplate()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALProcedureTemplate(SharedVariable SharedVariable)
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

        public DSProcedureTemplate SaveProcedureSystemObservations(DSProcedureTemplate dsProcedureTemplate)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(13);
                dbManager.AddParameters(0, "@Name", dsProcedureTemplate.ProcedureTemplateSystemObservations.NameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(1, "@CreatedBy", dsProcedureTemplate.ProcedureTemplateSystemObservations.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, "@CreatedOn", dsProcedureTemplate.ProcedureTemplateSystemObservations.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(3, "@ModifiedBy", dsProcedureTemplate.ProcedureTemplateSystemObservations.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@ModifiedOn", dsProcedureTemplate.ProcedureTemplateSystemObservations.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(5, "@ProviderXML", dsProcedureTemplate.ProcedureTemplateSystemObservations.ProviderXMLColumn.ColumnName, DbType.Xml);
                dbManager.AddParameters(6, "@ProceduresXML", dsProcedureTemplate.ProcedureTemplateSystemObservations.ProceduresXMLColumn.ColumnName, DbType.Xml);
                dbManager.AddParameters(7, "@SystemObservationXML", dsProcedureTemplate.ProcedureTemplateSystemObservations.SystemObservationXMLColumn.ColumnName, DbType.Xml);
                dbManager.AddParameters(8, "@EntityId", dsProcedureTemplate.ProcedureTemplateSystemObservations.EntityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(9, "@TemplatePreview", dsProcedureTemplate.ProcedureTemplateSystemObservations.TemplatePreviewColumn.ColumnName, DbType.String);
                dbManager.AddParameters(10, "@TemplateId", dsProcedureTemplate.ProcedureTemplateSystemObservations.ProcedureTemplateIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(11, "@IsActive", dsProcedureTemplate.ProcedureTemplateSystemObservations.IsActiveColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(12, "@AssociatedWithIds", dsProcedureTemplate.ProcedureTemplateSystemObservations.AssociatedWithIdsColumn.ColumnName, DbType.String);
                // dbManager.AddParameters(12, "@NoteViewTypeId", dsProcedureTemplate.ProcedureTemplateSystemObservations.NoteViewTypeIdColumn.ColumnName, DbType.Int32);
                dsProcedureTemplate = (DSProcedureTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_Procedure_TEMPLATESAVE, dsProcedureTemplate, dsProcedureTemplate.ProcedureTemplateSystemObservations.TableName);
                dsProcedureTemplate.AcceptChanges();
                return dsProcedureTemplate;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::SavePhysicalExamSystemObservatiosECW", PROC_Procedure_TEMPLATESAVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProcedureTemplate TempAssociationLookup()
        {
            DSProcedureTemplate ds = new DSProcedureTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSProcedureTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[Clinical].[sp_ProcedureTemplateAssociationLookup]", ds, ds.ProcedureTemplateAssociationLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::LabsCategoryLookup", "[Clinical].[sp_ProcedureTemplateAssociationLookup]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProcedureTemplate TempNoteViewLookup()
        {
            DSProcedureTemplate ds = new DSProcedureTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSProcedureTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[Clinical].[sp_ProcedureTemplateNoteViewLookup]", ds, ds.ProcedureTemplateNoteViewLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::LabsCategoryLookup", "[Clinical].[sp_ProcedureTemplateNoteViewLookup]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteProcedureTemplate(long templateId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@ProcedureTemplateId", templateId);
                dbManager.AddParameters(1, "@ErrorMessage", "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ProcedureTemplate_DELETE).ToString();
                //returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ICDGROUPS_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::deletePETemplate", PROC_ProcedureTemplate_DELETE, ex);
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
        public DSProcedureTemplate insertProcedureSystemObservation(DSProcedureTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, "@ProcedureSystemObservationId", ds.ProcedureSystemObservation.ProcedureSystemObservationIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, "@PESystemId", ds.ProcedureSystemObservation.PESystemIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, "@PEObservationId", ds.ProcedureSystemObservation.PEObservationIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(3, "@ProcedureTemplateSystemId", ds.ProcedureSystemObservation.ProcedureSystemObservationIdColumn.ColumnName, DbType.Int64);
                ds = (DSProcedureTemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_Procedure_SYSTEM_OBSERVATION_INSERT, ds, ds.ProcedureSystemObservation.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::insertPhysicalExamSystemObservation", PROC_Procedure_SYSTEM_OBSERVATION_INSERT, ex);
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

        public List<ProcedureTemplatesModel> LoadProcedureTemplates(ProcedureTemplatesModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<ProcedureTemplatesModel> List = new List<ProcedureTemplatesModel>();
                dbManager.Open();
                dbManager.AddParameters("@ProcedureTemplateId", model.ProcedureTemplateId);
                dbManager.AddParameters("@IsActive", model.IsActive);
                dbManager.AddParameters("@EntityId", MDVSession.Current.EntityId);
                dbManager.AddParameters("@UserId", MDVSession.Current.AppUserId);
                List = dbManager.ExecuteReaderMapper<ProcedureTemplatesModel>(PROC_Procedure_TEMPLATESSELECT);

                return List;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public string updateProcedureNotesDescription(long ProcedureNotesObservationId, string Desr)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@ProcedureNotesObservationId", ProcedureNotesObservationId);
                dbManager.AddParameters(1, "@Desc", Desr);
                // dbManager.AddParameters(2, "@ModifiedBy", MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                //dbManager.AddParameters(3, "@ModifiedOn", DateTime.Now);
                var returnVal = Convert.ToString(dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_Procedure_NOTES_SYS_OBSERS_DESC_UPDATE));
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::updateProceduNotesDescription", PROC_Procedure_NOTES_SYS_OBSERS_DESC_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string updateProcedureOrderNotesDescription(long ProcedureNotesObservationId, string Desr)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@ProcedureOrderNotesObservationId", ProcedureNotesObservationId);
                dbManager.AddParameters(1, "@Desc", Desr);
                // dbManager.AddParameters(2, "@ModifiedBy", MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                //dbManager.AddParameters(3, "@ModifiedOn", DateTime.Now);
                var returnVal = Convert.ToString(dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_ProcedureOrder_NOTES_SYS_OBSERS_DESC_UPDATE));
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::updateProceduNotesDescription", PROC_ProcedureOrder_NOTES_SYS_OBSERS_DESC_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProcedureTemplate loadProcedureTempSysObservations(long ProcedureTemplateId)
        {
            DSProcedureTemplate ds = new DSProcedureTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (ProcedureTemplateId < 0)
                    dbManager.AddParameters(0, "@ProcedureTemplateId", null);
                else
                    dbManager.AddParameters(0, "ProcedureTemplateId", ProcedureTemplateId);

                ds = (DSProcedureTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROC_Procedure_TEMP_SYS_OBSERVATION_SELECT, ds, ds.ProcedureTemplateSystemObservations.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPhysicalExam", PROC_PROC_Procedure_TEMP_SYS_OBSERVATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProcedureTemplate loadProcedureSystemObservationForNotes(long NotesId, long ProcedureTemplateSystemId, int ProcedureId)
        {
            DSProcedureTemplate ds = new DSProcedureTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, "@NotesId", NotesId);
                dbManager.AddParameters(1, "@ProcedureTemplateSystemId", ProcedureTemplateSystemId);
                dbManager.AddParameters(2, "@ProcedureId", ProcedureId);
                ds = (DSProcedureTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Procedure_SYS_OBSERS_SELECT, ds, ds.ProcedureNotesSystemObservation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::PROC_Procedure_SYS_OBSERS_SELECT", PROC_Procedure_SYS_OBSERS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string GetProcedureTemplateSoapText(int ProcedureId, long NotesId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@ProcedureId", ProcedureId);
                dbManager.AddParameters(1, "@NotesId", NotesId);
                IDataReader reader = dbManager.ExecuteReader(CommandType.StoredProcedure, "[Clinical].[GetProcedureTemplateSoapText]");
                while (reader.Read())
                {
                    returnVal = Convert.ToString(reader["ProcedureTemplateSoapText"]);
                }
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("::GetProcedureTemplateSoapText", "[Clinical].[GetProcedureTemplateSoapText]", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProcedureTemplate loadProcedureOrderSystemObservationForNotes(long NotesId, long ProcedureTemplateSystemId, long ProcedureOrderId)
        {
            DSProcedureTemplate ds = new DSProcedureTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, "@NotesId", NotesId);
                dbManager.AddParameters(1, "@ProcedureTemplateSystemId", ProcedureTemplateSystemId);
                dbManager.AddParameters(2, "@ProcedureOrderId", ProcedureOrderId);
                ds = (DSProcedureTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ProcedureOrder_SYS_OBSERS_SELECT, ds, ds.ProcedureNotesSystemObservation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::PROC_Procedure_SYS_OBSERS_SELECT", PROC_ProcedureOrder_SYS_OBSERS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //public DSProcedureTemplate loadProcedureSystemRadiologyObservationForNotes(long NotesId, long ProcedureTemplateSystemId)
        //{
        //    DSProcedureTemplate ds = new DSProcedureTemplate();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(2);
        //        dbManager.AddParameters(0, "@NotesId", NotesId);
        //        dbManager.AddParameters(1, "@ProcedureTemplateSystemId", ProcedureTemplateSystemId);
        //        ds = (DSProcedureTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Procedure_SYS_OBSERS_RADIOLOGY_SELECT, ds, ds.ProcedureNotesSystemRadiologyObservation.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.DALErrorLog("DALPhysicalExamECW::PROC_Procedure_SYS_OBSERS_SELECT", PROC_Procedure_SYS_OBSERS_RADIOLOGY_SELECT, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}
        public string ActiveInActiveProcedureTemplate(ProcedureTemplatesModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<ProcedureTemplatesModel> List = new List<ProcedureTemplatesModel>();
                dbManager.Open();
                dbManager.AddParameters("@ProcedureTemplateId", model.ProcedureTemplateId);
                dbManager.AddParameters("@IsActive", model.IsActive);
                dbManager.AddParameters("@ModifiedBy", MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                List = dbManager.ExecuteReaderMapper<ProcedureTemplatesModel>(PROC_Procedure_TEMPLATESActiveInactive);
                if (List != null)
                {
                    return List[0].ProcedureTemplateId.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public ProcedureTemplatesModel LoadProcedureTemplateTests(ProcedureTemplatesModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0,"@ProcedureTemplateId", Convert.ToInt64(model.ProcedureTemplateId));
                dbManager.AddParameters(1,"@IsActive", Convert.ToBoolean(model.IsActive));
                IDataReader reader = dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ProcedureTemplateTests_SELECT);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        ProcedureTemplatesModel.Procedures proc = new ProcedureTemplatesModel.Procedures();
                        proc.CPTCode = Convert.ToString(reader["CPTCode"]);
                        proc.CPTCodeDescription = Convert.ToString(reader["CPTCodeDescription"]);
                        proc.ProcedureTemplateTestsId = Convert.ToInt64(reader["ProcedureTemplateTestsId"]);
                        model.procedures.Add(proc);
                        
                    }
                }
                return model;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
