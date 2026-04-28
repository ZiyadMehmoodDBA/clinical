using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Batch.HL7ImmunizationBatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MDVision.DataAccess.DAL.Batch
{
    public class DALBatch
    {
        #region "Stored Procedure Names"
        private const string PROC_IMPORT_HL7_BATCH_SELECT_BATCH = "Provider.sp_ImportHL7BatchSelectBatch";
        private const string PROC_IMPORT_HL7_BATCH_SELECT_QUEUE = "Provider.sp_ImportHL7BatchSelectQueue";
        private const string PROC_IMPORT_HL7_IMMUNIZATION_BATCH_DELETE = "Provider.sp_ImportHL7BatchDelete";
        private const string PROC_IMPORT_HL7_IMMUNIZATION_BATCH_MARK_AS_COMPLETED = "Provider.sp_ImportHL7BatchMarkAsCompleted";
        private const string PROC_IMPORT_HL7_IMMUNIZATION_BATCH_REPROCESS = "Provider.sp_ImportHL7BatchReprocess";
        private const string PROC_IMPORT_HL7_IMMUNIZATION_BATCH_MESSAGE_TYPE_LOOK_UP = "Provider.sp_HL7MessageTypeLookup";
        private const string PROC_IMPORT_HL7_IMMUNIZATION_BATCH_STATUS_LOOK_UP = "Provider.sp_HL7StatusLookup";
        private const string PROC_IMPORT_HL7_BATCH_SELECT_BATCH_BY_ID = "Provider.sp_ImportHL7BatchSelectBatchById";
        #endregion

        #region "Parameters"

        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_BATCH_IDs = "@HL7BatchId";
        private const string PARM_DOS_FROM = "@DOSFrom";
        private const string PARM_DOS_TO = "@DOSTo";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PATIENT_LAST_NAME = "@PatientLastName";
        private const string PARM_PATIENT_FIRST_NAME = "@PatientFirstName";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_TYPE = "@Type";
        private const string PARM_STATUS = "@StatusId";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        #endregion

        #region Constructors
        public DALBatch()
        {
            ClientConfiguration.SetClientObject();
        }
        #endregion

        #region "Import HL7 Batch (Mirth)"
        private void CreateParametersLoadHL7ImmunizationQueue(IDBManager dbManager, HL7ImmunizationBatchModel model)
        {
            dbManager.CreateParameters(12);

            dbManager.AddParameters(0, PARM_PAGE_NUMBER, model.PageNumber);
            dbManager.AddParameters(1, PARM_ROWSP_PAGE, model.RowsPerPage);

            if (string.IsNullOrEmpty(model.PatientId))
                dbManager.AddParameters(2, PARM_PATIENT_ID, null);
            else
                dbManager.AddParameters(2, PARM_PATIENT_ID, model.PatientId);

            if (string.IsNullOrEmpty(model.PatientFirstName))
                dbManager.AddParameters(3, PARM_PATIENT_FIRST_NAME, null);
            else
                dbManager.AddParameters(3, PARM_PATIENT_FIRST_NAME, model.PatientFirstName);

            if (string.IsNullOrEmpty(model.PatientLastName))
                dbManager.AddParameters(4, PARM_PATIENT_LAST_NAME, null);
            else
                dbManager.AddParameters(4, PARM_PATIENT_LAST_NAME, model.PatientLastName);

            if (string.IsNullOrEmpty(model.DOSFrom))
                dbManager.AddParameters(5, PARM_DOS_FROM, null);
            else
                dbManager.AddParameters(5, PARM_DOS_FROM, model.DOSFrom);

            if (string.IsNullOrEmpty(model.DOSTo))
                dbManager.AddParameters(6, PARM_DOS_TO, null);
            else
                dbManager.AddParameters(6, PARM_DOS_TO, model.DOSTo);

            if (string.IsNullOrEmpty(model.GivenByProviderid))
                dbManager.AddParameters(7, PARM_PROVIDER_ID, null);
            else
                dbManager.AddParameters(7, PARM_PROVIDER_ID, model.GivenByProviderid);

            if (string.IsNullOrEmpty(model.FacilityId))
                dbManager.AddParameters(8, PARM_FACILITY_ID, null);
            else
                dbManager.AddParameters(8, PARM_FACILITY_ID, model.FacilityId);

            if (string.IsNullOrEmpty(model.StatusId))
                dbManager.AddParameters(9, PARM_STATUS, null);
            else
                dbManager.AddParameters(9, PARM_STATUS, model.StatusId);

            if (string.IsNullOrEmpty(model.Type))
                dbManager.AddParameters(10, PARM_TYPE, null);
            else
                dbManager.AddParameters(10, PARM_TYPE, model.Type);
            dbManager.AddParameters(11, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
        }

        private void CreateParametersLoadHL7ImmunizationBatch(IDBManager dbManager, HL7ImmunizationBatchModel model)
        {
            dbManager.CreateParameters(7);

            dbManager.AddParameters(0, PARM_PAGE_NUMBER, model.PageNumber);
            dbManager.AddParameters(1, PARM_ROWSP_PAGE, model.RowsPerPage);

            if (string.IsNullOrEmpty(model.Providerid))
                dbManager.AddParameters(2, PARM_PROVIDER_ID, null);
            else
                dbManager.AddParameters(2, PARM_PROVIDER_ID, MDVUtility.ToInt64(model.Providerid));

            if (string.IsNullOrEmpty(model.DOSFrom))
                dbManager.AddParameters(3, PARM_DOS_FROM, null);
            else
                dbManager.AddParameters(3, PARM_DOS_FROM, model.DOSFrom);

            if (string.IsNullOrEmpty(model.DOSTo))
                dbManager.AddParameters(4, PARM_DOS_TO, null);
            else
                dbManager.AddParameters(4, PARM_DOS_TO, model.DOSTo);

            if (string.IsNullOrEmpty(model.StatusId))
                dbManager.AddParameters(5, PARM_STATUS, null);
            else
                dbManager.AddParameters(5, PARM_STATUS, model.StatusId);

            dbManager.AddParameters(6, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);
        }
        public List<HL7ImmunizationBatchModel> LoadHL7ImmunizationQueue(HL7ImmunizationBatchModel model)
        {
            List<HL7ImmunizationBatchModel> listCustomForms = new List<HL7ImmunizationBatchModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                this.CreateParametersLoadHL7ImmunizationQueue(dbManager, model);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_IMPORT_HL7_BATCH_SELECT_QUEUE);
                while (reader.Read())
                {
                    model = new HL7ImmunizationBatchModel();
                    model.HL7BatchId = !String.IsNullOrEmpty(reader["HL7BatchId"].ToString()) ? reader["HL7BatchId"].ToString() : "";
                    model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";
                    model.PatientName = !String.IsNullOrEmpty(reader["PatientName"].ToString()) ? reader["PatientName"].ToString() : "";
                    model.AccountNumber = !String.IsNullOrEmpty(reader["AccountNumber"].ToString()) ? reader["AccountNumber"].ToString() : "";
                    model.FacilityName = !String.IsNullOrEmpty(reader["FacilityName"].ToString()) ? reader["FacilityName"].ToString() : "";
                    model.GivenByProviderName = !String.IsNullOrEmpty(reader["GivenByProviderName"].ToString()) ? reader["GivenByProviderName"].ToString() : "";
                    model.Type = !String.IsNullOrEmpty(reader["Type"].ToString()) ? reader["Type"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.StatusName = !String.IsNullOrEmpty(reader["StatusName"].ToString()) ? reader["StatusName"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.DOB = !String.IsNullOrEmpty(reader["DOB"].ToString()) ? reader["DOB"].ToString() : "";
                    model.Gender = !String.IsNullOrEmpty(reader["Gender"].ToString()) ? reader["Gender"].ToString() : "";
                    model.VaccineHxId = !String.IsNullOrEmpty(reader["VaccineHxId"].ToString()) ? reader["VaccineHxId"].ToString() : "";
                    model.years = !String.IsNullOrEmpty(reader["years"].ToString()) ? reader["years"].ToString() : "";
                    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? reader["PatientId"].ToString() : "";
                    model.VaccineScheduleId = !String.IsNullOrEmpty(reader["VaccineScheduleId"].ToString()) ? reader["VaccineScheduleId"].ToString() : "";
                    model.VaccineCategory = !String.IsNullOrEmpty(reader["VaccineCategory"].ToString()) ? reader["VaccineCategory"].ToString() : "";
                    model.VaccineCategoryId = !String.IsNullOrEmpty(reader["VaccineCategoryId"].ToString()) ? reader["VaccineCategoryId"].ToString() : "";
                    model.TabId = !String.IsNullOrEmpty(reader["TabId"].ToString()) ? reader["TabId"].ToString() : "";
                    model.AcknowledgementCode = !String.IsNullOrEmpty(reader["AcknowledgementCode"].ToString()) ? reader["AcknowledgementCode"].ToString() : "";
                    model.VaccineHxType = !String.IsNullOrEmpty(reader["VaccineHxType"].ToString()) ? reader["VaccineHxType"].ToString() : "";
                    listCustomForms.Add(model);
                }
                return listCustomForms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatch::loadHL7ImmunizationBatch", PROC_IMPORT_HL7_BATCH_SELECT_BATCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<HL7ImmunizationBatchModel> LoadHL7ImmunizationBatch(HL7ImmunizationBatchModel model)
        {
            List<HL7ImmunizationBatchModel> listCustomForms = new List<HL7ImmunizationBatchModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                this.CreateParametersLoadHL7ImmunizationBatch(dbManager, model);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_IMPORT_HL7_BATCH_SELECT_BATCH);
                while (reader.Read())
                {
                    model = new HL7ImmunizationBatchModel();
                    model.HL7BatchId = !String.IsNullOrEmpty(reader["HL7BatchId"].ToString()) ? reader["HL7BatchId"].ToString() : "";
                    model.ProviderName = !String.IsNullOrEmpty(reader["ProviderName"].ToString()) ? reader["ProviderName"].ToString() : "";
                    model.Records = !String.IsNullOrEmpty(reader["Records"].ToString()) ? reader["Records"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.FileName = !String.IsNullOrEmpty(reader["FileName"].ToString()) ? reader["FileName"].ToString() : "";
                    model.StatusName = !String.IsNullOrEmpty(reader["StatusName"].ToString()) ? reader["StatusName"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.IsCompleted = !String.IsNullOrEmpty(reader["IsCompleted"].ToString()) ? reader["IsCompleted"].ToString() : "";
                    model.completionDate = !String.IsNullOrEmpty(reader["completionDate"].ToString()) ? reader["completionDate"].ToString() : "";
                    listCustomForms.Add(model);
                }
                return listCustomForms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatch::loadHL7ImmunizationBatch", PROC_IMPORT_HL7_BATCH_SELECT_BATCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteHL7ImmunizationBatch(string BatchIds)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BATCH_IDs, BatchIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMPORT_HL7_IMMUNIZATION_BATCH_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatch::DeleteHL7ImmunizationBatch", PROC_IMPORT_HL7_IMMUNIZATION_BATCH_DELETE, ex);
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
        public string MarkBatchAsCompleted(string BatchIds)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BATCH_IDs, BatchIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMPORT_HL7_IMMUNIZATION_BATCH_MARK_AS_COMPLETED).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatch::MarkBatchAsCompleted", PROC_IMPORT_HL7_IMMUNIZATION_BATCH_MARK_AS_COMPLETED, ex);
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
        public string ReProcessBatch(string BatchIds)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BATCH_IDs, BatchIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMPORT_HL7_IMMUNIZATION_BATCH_REPROCESS).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatch::ReProcessBatch", PROC_IMPORT_HL7_IMMUNIZATION_BATCH_REPROCESS, ex);
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

        public DSImmunizationHL7 LookupHL7BatchMessageType()
        {
            DSImmunizationHL7 ds = new DSImmunizationHL7();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSImmunizationHL7)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_IMPORT_HL7_IMMUNIZATION_BATCH_MESSAGE_TYPE_LOOK_UP, ds, ds.HL7MessageTypeLookUp.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatch::LookupHL7BatchMessageType", PROC_IMPORT_HL7_IMMUNIZATION_BATCH_MESSAGE_TYPE_LOOK_UP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSImmunizationHL7 LookupHL7BatchStatus()
        {
            DSImmunizationHL7 ds = new DSImmunizationHL7();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSImmunizationHL7)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_IMPORT_HL7_IMMUNIZATION_BATCH_STATUS_LOOK_UP, ds, ds.HL7StatusLookUp.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatch::LookupHL7BatchStatus", PROC_IMPORT_HL7_IMMUNIZATION_BATCH_STATUS_LOOK_UP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<HL7ImmunizationBatchModel> LoadHL7ImmunizationBatchById(HL7ImmunizationBatchModel model)
        {
            List<HL7ImmunizationBatchModel> listCustomForms = new List<HL7ImmunizationBatchModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_BATCH_IDs, model.HL7BatchId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_IMPORT_HL7_BATCH_SELECT_BATCH_BY_ID);
                while (reader.Read())
                {
                    model = new HL7ImmunizationBatchModel();
                    model.HL7BatchId = !String.IsNullOrEmpty(reader["HL7BatchId"].ToString()) ? reader["HL7BatchId"].ToString() : "";
                    model.FileName = !String.IsNullOrEmpty(reader["FileName"].ToString()) ? reader["FileName"].ToString() : "";
                    model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    model.Hl7MsgText = !String.IsNullOrEmpty(reader["Hl7MsgText"].ToString()) ? reader["Hl7MsgText"].ToString() : "";
                    listCustomForms.Add(model);
                }
                return listCustomForms;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBatch::LoadHL7ImmunizationBatchById", PROC_IMPORT_HL7_BATCH_SELECT_BATCH_BY_ID, ex);
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
