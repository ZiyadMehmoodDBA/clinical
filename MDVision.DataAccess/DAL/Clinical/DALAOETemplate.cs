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
    public class DALAOETemplate
    {
        #region " Variable "

        #endregion

        #region " Stored Procedure Names "
        private const string PROC_AOE_TEMPLATESAVE = "Clinical.sp_CreateNewAOETemplate";
        private const string PROC_AOE_TEMPLATESSELECT = "Clinical.sp_AOETemplateSelect";
        private const string PROC_AOE_TEMPLATESActiveInactive = "[Clinical].[sp_AOETemplateUpdateActiveInActive]";
        private const string PROC_PROC_AOE_TEMP_SYS_OBSERVATION_SELECT = "Clinical.sp_AOETempSysObservationSelect";
        private const string PROC_AOE_SYS_OBSERS_SELECT = "clinical.sp_AOENotesSystemObservationSelect";
        private const string PROC_AOE_NOTES_SYS_OBSERS_DESC_UPDATE = "clinical.sp_AOENotesSystemObservationDescUpdate";
        private const string PROC_AOE_SYS_OBSERS_RADIOLOGY_SELECT = "clinical.sp_AOENotesSystemRadiologyObservationSelect";
        private const string PROC_AOE_NOTES_SYS_OBSERS_RADIOLOGY_DESC_UPDATE = "clinical.sp_AOENotesSystemRadiologyObservationDescUpdate";
        private const string PROC_AOE_SYSTEM_OBSERVATION_INSERT = "Clinical.sp_AOESystemObservationInsert";
        private const string PROC_AOETEMPLATE_DELETE = "Clinical.sp_AOETemplateDelete";
        #endregion

        #region " Constructors "
        public DALAOETemplate()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALAOETemplate(SharedVariable SharedVariable)
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

        public DSAOETemplate SaveAOESystemObservations(DSAOETemplate dsAOETemplate)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(15);
                dbManager.AddParameters(0, "@Name", dsAOETemplate.AOETemplateSystemObservations.NameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(1, "@CreatedBy", dsAOETemplate.AOETemplateSystemObservations.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, "@CreatedOn", dsAOETemplate.AOETemplateSystemObservations.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(3, "@ModifiedBy", dsAOETemplate.AOETemplateSystemObservations.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@ModifiedOn", dsAOETemplate.AOETemplateSystemObservations.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(5, "@LabId", dsAOETemplate.AOETemplateSystemObservations.LabIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(6, "@CPTCode", dsAOETemplate.AOETemplateSystemObservations.CPTCodeColumn.ColumnName, DbType.String);
                dbManager.AddParameters(7, "@CPTCodeDescription", dsAOETemplate.AOETemplateSystemObservations.CPTCodeDescriptionColumn.ColumnName, DbType.String);
                dbManager.AddParameters(8, "@ProviderXML", dsAOETemplate.AOETemplateSystemObservations.ProviderXMLColumn.ColumnName, DbType.Xml);
                dbManager.AddParameters(9, "@SystemObservationXML", dsAOETemplate.AOETemplateSystemObservations.SystemObservationXMLColumn.ColumnName, DbType.Xml);
                dbManager.AddParameters(10, "@EntityId", dsAOETemplate.AOETemplateSystemObservations.EntityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(11, "@TemplatePreview", dsAOETemplate.AOETemplateSystemObservations.TemplatePreviewColumn.ColumnName, DbType.String);
                dbManager.AddParameters(12, "@TemplateId", dsAOETemplate.AOETemplateSystemObservations.AOETemplateIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(13, "@SpecialtyXML", dsAOETemplate.AOETemplateSystemObservations.SpecialtyXMLColumn.ColumnName, DbType.Xml);
                dbManager.AddParameters(14, "@IsActive", dsAOETemplate.AOETemplateSystemObservations.IsActiveColumn.ColumnName, DbType.Boolean);
                dsAOETemplate = (DSAOETemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_AOE_TEMPLATESAVE, dsAOETemplate, dsAOETemplate.AOETemplateSystemObservations.TableName);
                dsAOETemplate.AcceptChanges();
                return dsAOETemplate;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::SavePhysicalExamSystemObservatiosECW", PROC_AOE_TEMPLATESAVE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string deleteAOETemplate(long templateId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@AOETemplateId", templateId);
                dbManager.AddParameters(1, "@ErrorMessage", "", DbType.String, ParamDirection.Output, null, 255);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_AOETEMPLATE_DELETE).ToString();
                //returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ICDGROUPS_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);
                return returnValue;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::deletePETemplate", PROC_AOETEMPLATE_DELETE, ex);
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
        public DSAOETemplate insertAOESystemObservation(DSAOETemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, "@AOESystemObservationId", ds.AOESystemObservation.AOESystemObservationIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, "@PESystemId", ds.AOESystemObservation.PESystemIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, "@PEObservationId", ds.AOESystemObservation.PEObservationIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(3, "@AOETemplateSystemId", ds.AOESystemObservation.AOESystemObservationIdColumn.ColumnName, DbType.Int64);
                ds = (DSAOETemplate)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_AOE_SYSTEM_OBSERVATION_INSERT, ds, ds.AOESystemObservation.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::insertPhysicalExamSystemObservation", PROC_AOE_SYSTEM_OBSERVATION_INSERT, ex);
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
        public List<AOETemplatesModel> LoadAOETemplates(AOETemplatesModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<AOETemplatesModel> List = new List<AOETemplatesModel>();
                dbManager.Open();
                dbManager.AddParameters("@AOETemplateId", model.AOETemplateId);
                dbManager.AddParameters("@IsActive", model.IsActive);
                dbManager.AddParameters("@EntityId", MDVSession.Current.EntityId);
                dbManager.AddParameters("@UserId", MDVSession.Current.AppUserId);
                List = dbManager.ExecuteReaderMapper<AOETemplatesModel>(PROC_AOE_TEMPLATESSELECT);

                return List;
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public string updateAOENotesDescription(long AOENotesObservationId, string Desr)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@AOENotesObservationId", AOENotesObservationId);
                dbManager.AddParameters(1, "@Desc", Desr);
               // dbManager.AddParameters(2, "@ModifiedBy", MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                //dbManager.AddParameters(3, "@ModifiedOn", DateTime.Now);
                var returnVal = Convert.ToString(dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_AOE_NOTES_SYS_OBSERS_DESC_UPDATE));
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::updatePENotesDescription", PROC_AOE_NOTES_SYS_OBSERS_DESC_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string updateAOENotesRadiologyDescription(long AOENotesObservationId, string Desr)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@AOENotesRadiologyObservationId", AOENotesObservationId);
                dbManager.AddParameters(1, "@Desc", Desr);
                var returnVal = Convert.ToString(dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_AOE_NOTES_SYS_OBSERS_RADIOLOGY_DESC_UPDATE));
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::updatePENotesDescription", PROC_AOE_NOTES_SYS_OBSERS_RADIOLOGY_DESC_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSAOETemplate loadAOETempSysObservations(long AOETemplateId)
        {
            DSAOETemplate ds = new DSAOETemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (AOETemplateId < 0)
                    dbManager.AddParameters(0, "@AOETemplateId", null);
                else
                    dbManager.AddParameters(0, "AOETemplateId", AOETemplateId);

                ds = (DSAOETemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROC_AOE_TEMP_SYS_OBSERVATION_SELECT, ds, ds.AOETemplateSystemObservations.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::loadPhysicalExam", PROC_PROC_AOE_TEMP_SYS_OBSERVATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSAOETemplate loadAOESystemObservationForNotes(long NotesId, long AOETemplateSystemId)
        {
            DSAOETemplate ds = new DSAOETemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@NotesId", NotesId);
                dbManager.AddParameters(1, "@AOETemplateSystemId", AOETemplateSystemId);
                ds = (DSAOETemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_AOE_SYS_OBSERS_SELECT, ds, ds.AOENotesSystemObservation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::PROC_AOE_SYS_OBSERS_SELECT", PROC_AOE_SYS_OBSERS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSAOETemplate loadAOESystemRadiologyObservationForNotes(long NotesId, long AOETemplateSystemId)
        {
            DSAOETemplate ds = new DSAOETemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@NotesId", NotesId);
                dbManager.AddParameters(1, "@AOETemplateSystemId", AOETemplateSystemId);
                ds = (DSAOETemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_AOE_SYS_OBSERS_RADIOLOGY_SELECT, ds, ds.AOENotesSystemRadiologyObservation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::PROC_AOE_SYS_OBSERS_SELECT", PROC_AOE_SYS_OBSERS_RADIOLOGY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string ActiveInActiveAOETemplate(AOETemplatesModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<AOETemplatesModel> List = new List<AOETemplatesModel>();
                dbManager.Open();
                dbManager.AddParameters("@AOETemplateId", model.AOETemplateId);
                dbManager.AddParameters("@IsActive", model.IsActive);
                dbManager.AddParameters("@ModifiedBy", MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                List = dbManager.ExecuteReaderMapper<AOETemplatesModel>(PROC_AOE_TEMPLATESActiveInactive);
                if (List != null)
                {
                    return List[0].AOETemplateId.ToString();
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
    }
}
