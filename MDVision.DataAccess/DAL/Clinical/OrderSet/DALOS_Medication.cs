using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.DataAccess.DCommon;
using System.ComponentModel;
using MDVision.Datasets;
using MDVision.Model.Clinical.Reports;
using System.Data.SqlClient;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Clinical.Immunization;
using MDVision.Common.Utilities;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Model.Clinical.Orderset;


namespace MDVision.DataAccess.DAL.Clinical.OrderSet
{
    public class DALOS_Medication
    {
         #region Variable

        #endregion

        #region "Stored Procedure Names"


        private const string PROC_GET_MEDICATION_ORDERSET_LOOKUP = "Clinical.sp_GetMedicationOrdersetLookUp";
        private const string PROC_OS_MEDICATION_INSERT = "Clinical.sp_OsMedicationInsert";
        private const string PROC_OS_MEDICATION_UPDATE = "Clinical.sp_OsMedicationUpdate";
        private const string PROC_OS_MEDICATION_SELECT = "Clinical.sp_OsMedicationSelect";
        private const string PROC_OS_MEDICATION_DELETE = "Clinical.sp_OsMedicationDelete";
        private const string PROC_MEDICATION_SELECT_FOR_DELETE_FROM_DRFIRST = "Clinical.sp_MedicationSelectForDeleteFromDrFirst";
        private const string PROC_OS_MEDICATION_EXISTS_OR_NOT = "Clinical.sp_OsMedicationExistsOrNot";
        
        #endregion "Stored Procedure Names"

        #region "Parameters"
        private const string PARM_LOOKUP_TYPE_NAME = "@LookupTypeName";
        private const string PARM_OS_MEDICATION_ID = "@OS_MedicationId";
        private const string PARM_ACTION = "@Action";
        private const string PARM_DOSE = "@Dose";
        private const string PARM_DOSE_UNIT = "@DoseUnit";
        private const string PARM_ROUTE = "@Route";
        private const string PARM_DOSE_TIMING = "@DoseTiming";
        private const string PARM_DOSE_OTHER = "@Doseother";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_QUANTITY = "@Quantity";
        private const string PARM_QUANTITY_UNIT = "@QuantityUnit";
        private const string PARM_REFILL = "@Refill";
        private const string PARM_DIRECTIONS_TO_PHARMACIST = "@DirectionsToPharmacist";
        private const string PARM_ADD_DIRECTION_TO_PATIENT = "@AddDirectionToPatient";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_NDCID = "@NDCID";
        private const string PARM_BRAND_NAME = "@BrandName";
        private const string PARM_GENERIC_NAME = "@GenericName";
        private const string PARM_FORM = "@Form";
        private const string PARM_STRENGTH = "@Strength";
        private const string PARM_ORDERSET_ID = "@OrdersetId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PRESCRIPTIONS_OTHER_NOTES = "@PrescriptionsOtherNotes";
        private const string PARM_MEDICATION_IDS = "@MedicationIds";
        private const string PARM_PATIENT_ID = "@PatientId";
        
            
        #endregion

        #region Constructors

        public DALOS_Medication()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }

        public DALOS_Medication(SharedVariable SharedVariable)
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

        #region "Support Functions For Immunization"

        /// <summary>
        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function for Create Parameters For Category
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        /// 
        private void createMedicationParameters(IDBManager dbManager, OS_MedicationModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_OS_MEDICATION_ID, model.OS_MedicationId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_OS_MEDICATION_ID, model.OS_MedicationId, DbType.Int64);
            if (!string.IsNullOrEmpty(model.Action))
            {
                dbManager.AddParameters(PARM_ACTION, model.Action);
            }
            else
            {
                dbManager.AddParameters(PARM_ACTION, null);
            }
            if (!string.IsNullOrEmpty(model.Dose))
            {
                dbManager.AddParameters(PARM_DOSE, model.Dose);
            }
            else
            {
                dbManager.AddParameters(PARM_DOSE, null);
            }

            if (!string.IsNullOrEmpty(model.DoseUnit))
            {
                dbManager.AddParameters(PARM_DOSE_UNIT, model.DoseUnit);
            }
            else
            {
                dbManager.AddParameters(PARM_DOSE_UNIT, null);
            }

            if (!string.IsNullOrEmpty(model.Route))
            {
                dbManager.AddParameters(PARM_ROUTE, model.Route);
            }
            else
            {
                dbManager.AddParameters(PARM_ROUTE, null);
            }


            if (!string.IsNullOrEmpty(model.DoseTiming))
            {
                dbManager.AddParameters(PARM_DOSE_TIMING, model.DoseTiming);
            }
            else
            {
                dbManager.AddParameters(PARM_DOSE_TIMING, null);
            }

            if (!string.IsNullOrEmpty(model.DoseOther))
            {
                dbManager.AddParameters(PARM_DOSE_OTHER, model.DoseOther);
            }
            else
            {
                dbManager.AddParameters(PARM_DOSE_OTHER, null);
            }

            if (!string.IsNullOrEmpty(model.Duration))
            {
                dbManager.AddParameters(PARM_DURATION, model.Duration);
            }
            else
            {
                dbManager.AddParameters(PARM_DURATION, null);
            }

            if (!string.IsNullOrEmpty(model.Quantity))
            {
                dbManager.AddParameters(PARM_QUANTITY, model.Quantity);
            }
            else
            {
                dbManager.AddParameters(PARM_QUANTITY, null);
            }

            if (!string.IsNullOrEmpty(model.QuantityUnit))
            {
                dbManager.AddParameters(PARM_QUANTITY_UNIT, model.QuantityUnit);
            }
            else
            {
                dbManager.AddParameters(PARM_QUANTITY_UNIT, null);
            }

            if (!string.IsNullOrEmpty(model.Refill))
            {
                dbManager.AddParameters(PARM_REFILL, model.Refill);
            }
            else
            {
                dbManager.AddParameters(PARM_REFILL, null);
            }

            if (!string.IsNullOrEmpty(model.DirectionsToPharmacist))
            {
                dbManager.AddParameters(PARM_DIRECTIONS_TO_PHARMACIST, model.DirectionsToPharmacist);
            }
            else
            {
                dbManager.AddParameters(PARM_DIRECTIONS_TO_PHARMACIST, null);
            }

            if (!string.IsNullOrEmpty(model.AddDirectionToPatient))
            {
                dbManager.AddParameters(PARM_ADD_DIRECTION_TO_PATIENT, model.AddDirectionToPatient);
            }
            else
            {
                dbManager.AddParameters(PARM_ADD_DIRECTION_TO_PATIENT, null);
            }

            if (!string.IsNullOrEmpty(model.PrescriptionsOtherNotes))
            {
                dbManager.AddParameters(PARM_PRESCRIPTIONS_OTHER_NOTES, model.PrescriptionsOtherNotes);
            }
            else
            {
                dbManager.AddParameters(PARM_PRESCRIPTIONS_OTHER_NOTES, null);
            }
            if (!string.IsNullOrEmpty(model.Comments))
            {
                dbManager.AddParameters(PARM_COMMENTS, model.Comments);
            }
            else
            {
                dbManager.AddParameters(PARM_COMMENTS, null);
            }

            if (!string.IsNullOrEmpty(model.NDCID))
            {
                dbManager.AddParameters(PARM_NDCID, model.NDCID);
            }
            else
            {
                dbManager.AddParameters(PARM_NDCID, null);
            }

            if (!string.IsNullOrEmpty(model.BrandName))
            {
                dbManager.AddParameters(PARM_BRAND_NAME, model.BrandName);
            }
            else
            {
                dbManager.AddParameters(PARM_BRAND_NAME, null);
            }

            if (!string.IsNullOrEmpty(model.GenericName))
            {
                dbManager.AddParameters(PARM_GENERIC_NAME, model.GenericName);
            }
            else
            {
                dbManager.AddParameters(PARM_GENERIC_NAME, null);
            }

            if (!string.IsNullOrEmpty(model.Form))
            {
                dbManager.AddParameters(PARM_FORM, model.Form);
            }
            else
            {
                dbManager.AddParameters(PARM_FORM, null);
            }

            if (!string.IsNullOrEmpty(model.Strength))
            {
                dbManager.AddParameters(PARM_STRENGTH, model.Strength);
            }
            else
            {
                dbManager.AddParameters(PARM_STRENGTH, null);
            }

            if (!string.IsNullOrEmpty(model.OrdersetId))
            {
                dbManager.AddParameters(PARM_ORDERSET_ID, model.OrdersetId);
            }
            else
            {
                dbManager.AddParameters(PARM_ORDERSET_ID, null);
            }

            

            
        }
        #endregion

        public DSImmunization GetMedicationOrdersetLookUp(string LookUpTypeName)
        {
            DSImmunization ds = new DSImmunization();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_LOOKUP_TYPE_NAME, LookUpTypeName);
                ds = (DSImmunization)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_MEDICATION_ORDERSET_LOOKUP, ds, ds.VaccineRefusalReason.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_Medication::GetMedicationOrdersetLookUp", PROC_GET_MEDICATION_ORDERSET_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string SaveOSMedication(OS_MedicationModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createMedicationParameters(dbManager, model, true);
                var OS_MedicationId = dbManager.ExecuteScalar(PROC_OS_MEDICATION_INSERT);
                return MDVUtility.ToStr("");
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_Medication::SaveOSMedication", PROC_OS_MEDICATION_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<OS_MedicationModel> LoadMedication(string OrdersetId, string OS_MedicationId, int pageNumber, int rowsPerPage)
        {
            List<OS_MedicationModel> MedicationList = new List<OS_MedicationModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (!string.IsNullOrEmpty(OS_MedicationId))
                {
                    dbManager.AddParameters(PARM_OS_MEDICATION_ID, OS_MedicationId);
                }
                else
                {
                    dbManager.AddParameters(PARM_OS_MEDICATION_ID, null);
                }

                if (!string.IsNullOrEmpty(OrdersetId))
                {
                    dbManager.AddParameters(PARM_ORDERSET_ID, OrdersetId);
                }
                else
                {
                    dbManager.AddParameters(PARM_ORDERSET_ID, null);
                }
                dbManager.AddParameters(PARM_PAGE_NUMBER, pageNumber);
                dbManager.AddParameters(PARM_ROWS_PER_PAGE, rowsPerPage);
                dbManager.AddParameters(PARM_RECORD_COUNT, null, DbType.String, ParamDirection.Output, 500);
                MedicationList = dbManager.ExecuteReaders<OS_MedicationModel>(PROC_OS_MEDICATION_SELECT);
                return MedicationList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_Medication::LoadMedication", PROC_OS_MEDICATION_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }



        public List<OS_MedicationModel> ExistsOrNotExistsMedication(string Os_MedicationIds, long PatientId)
        {
            List<OS_MedicationModel> MedicationList = new List<OS_MedicationModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (!string.IsNullOrEmpty(Os_MedicationIds))
                {
                    dbManager.AddParameters(PARM_OS_MEDICATION_ID, Os_MedicationIds);
                }
                else
                {
                    dbManager.AddParameters(PARM_OS_MEDICATION_ID, null);
                }
                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);
                MedicationList = dbManager.ExecuteReaders<OS_MedicationModel>(PROC_OS_MEDICATION_EXISTS_OR_NOT);
                return MedicationList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_Medication::ExistsOrNotExistsMedication", PROC_OS_MEDICATION_EXISTS_OR_NOT, ex);
                throw ex;
            }
            finally
            {
            }
        }
        public List<OS_MedicationModel> LoadMedicationForDeleteFromDrFirst(string MedicationIds)
        {
            List<OS_MedicationModel> MedicationList = new List<OS_MedicationModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_MEDICATION_IDS, MedicationIds);
                MedicationList = dbManager.ExecuteReaders<OS_MedicationModel>(PROC_MEDICATION_SELECT_FOR_DELETE_FROM_DRFIRST);
                return MedicationList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_Medication::LoadMedicationForDeleteFromDrFirst", PROC_MEDICATION_SELECT_FOR_DELETE_FROM_DRFIRST, ex);
                throw ex;
            }
            finally
            {
            }
        }
        
        public string UpdateOSMedication(OS_MedicationModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createMedicationParameters(dbManager, model, false);
                var MedicationId = dbManager.ExecuteScalar(PROC_OS_MEDICATION_UPDATE);
                return MDVUtility.ToStr(MedicationId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALOS_Medication::UpdateFavVaccine", PROC_OS_MEDICATION_UPDATE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string DeleteOsMedication(string OS_MedicationId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_OS_MEDICATION_ID, OS_MedicationId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_OS_MEDICATION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                }
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALOrderSet::DeleteOsMedication", PROC_OS_MEDICATION_DELETE, ex);
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
        
    }
}
