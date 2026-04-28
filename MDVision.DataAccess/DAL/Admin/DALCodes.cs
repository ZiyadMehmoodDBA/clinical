using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Model.Lookups;
using System.Data.SqlClient;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALCodes
    {
        #region Variable
        
        #endregion
        #region "Stored Procedure Names"
        private const string PROC_GENDER_LOOKUP = "Provider.sp_GenderLookup";
        private const string PROC_PQRSTREATMENT_TYPE_LOOKUP = "Clinical.sp_PQRSTreatmentTypeLookup";
        private const string PROC_PQRSREASON_TYPE_LOOKUP = "Clinical.sp_PQRSReasonTypeLookup";
        private const string PROC_PLACE_OF_SERVICE_SELECT = "Provider.sp_PlaceOfServiceSelect";
        private const string PROC_STATE_SELECT = "System.sp_StateSelect";
        private const string PROC_STATE_LOOKUP = "System.sp_StateLookup";
        private const string PROC_TYPE_OF_SERVICE_LOOKUP = "Provider.sp_TypeOfServiceLookup";
        private const string PROC_NDC_MEASUREMENT_CODE_LOOKUP = "Provider.sp_NDCMeasurementCodeLookup";
        private const string PROC_SERVICE_TYPE_LOOKUP = "Billing.sp_ServiceTypeLookup";
        private const string PROC_ProviderParticipentStatusLookup = "Provider.sp_ProviderParticipentStatusLookup";
        private const string PROC_CASE_ADJUSTER_LOOKUP = "Patient.sp_CaseAdjusterLookup";
        #endregion

        #region "Parameters"
        private const string PARM_POS_ID = "@POSId";

        #endregion

        #region Constructors

        public DALCodes()
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

        /// <summary>
        /// Lookups the place of service.
        /// </summary>
        /// <returns></returns>
        public DSCodeLookup LookupNDCMeasurementCode()
        {
            DSCodeLookup ds = new DSCodeLookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_NDC_MEASUREMENT_CODE_LOOKUP, ds, ds.NDCMeasurementCode.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALNDCMeasurementCode::LookupNDCMeasurementCode", PROC_NDC_MEASUREMENT_CODE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }




        /// <summary>
        /// Loads the type of service.
        /// </summary>
        /// <param name="POSId">The position identifier.</param>
        /// <returns></returns>
        //public DSCodes LookupTypeOfService()
        //{
        //    DSCodes ds = new DSCodes();
        //    IDBManager dbManager =ClientConfiguration.GetDBManager(SharedObj);
        //    try
        //    {
        //        dbManager.Open();

        //        ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_TYPE_OF_SERVICE_LOOKUP, ds, ds.TypeOfService.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALCodes::LookupTypeOfService", PROC_TYPE_OF_SERVICE_LOOKUP, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}


        /// <summary>
        /// Loads the gender.
        /// </summary>
        /// <returns></returns>
        public DSCodes LookupGender()
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GENDER_LOOKUP, ds, ds.gender.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCodes::LoadGender", PROC_GENDER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<GenderModel> LookupGenderDemographic()
        {
            List<GenderModel> listAllergies = new List<GenderModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GENDER_LOOKUP);

                GenderModel model = null;
                while (reader.Read())
                {
                    model = new GenderModel();
                    model.Id = reader["Id"].ToString();
                    model.GenderName = reader["GenderName"].ToString();

                    listAllergies.Add(model);
                }

                return listAllergies;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCodes::LoadGender", PROC_GENDER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }
                                 
        public List<TreatmentModel> LookupPQRSTreatmentType()
        {
            List<TreatmentModel> listTreatments = new List<TreatmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PQRSTREATMENT_TYPE_LOOKUP);

                TreatmentModel model = null;
                while (reader.Read())
                {
                    model = new TreatmentModel();
                    model.Id = reader["Id"].ToString();
                    model.Description = reader["Description"].ToString();

                    listTreatments.Add(model);
                }

                return listTreatments;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCodes::LookupPQRSTreatmentType", PROC_PQRSTREATMENT_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        public List<TreatmentModel> LookupPQRSReasonType()
        {
            List<TreatmentModel> listTreatments = new List<TreatmentModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PQRSREASON_TYPE_LOOKUP);

                TreatmentModel model = null;
                while (reader.Read())
                {
                    model = new TreatmentModel();
                    model.Id = reader["Id"].ToString();
                    model.Description = reader["Description"].ToString();

                    listTreatments.Add(model);
                }

                return listTreatments;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCodes::LookupPQRSReasonType", PROC_PQRSREASON_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Loads the states.
        /// </summary>
        /// <returns></returns>
        public DSCodes LookupStates()
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_STATE_LOOKUP, ds, ds.gender.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCodes::LookupStates", PROC_STATE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSCodes LookupServiceType(string IsActive)
        {
            DSCodes ds = new DSCodes();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSCodes)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_SERVICE_TYPE_LOOKUP, ds, ds.ServiceType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCodes::LookupServiceType", PROC_SERVICE_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<ProviderParticipationStatusModel> LookupParticipentStatus()
        {
            List<ProviderParticipationStatusModel> providerparticipentstatus = new List<ProviderParticipationStatusModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ProviderParticipentStatusLookup);
                ProviderParticipationStatusModel model = null;
                while (reader.Read())
                {
                    model = new ProviderParticipationStatusModel();
                    model.ProviderParticipentStatusId = reader["ProviderParticipationStatusId"].ToString();
                    model.ShortName = reader["ShortName"].ToString();
                    model.Description = reader["Description"].ToString();
                    model.IsActive = Convert.ToBoolean(reader["IsActive"]);

                    providerparticipentstatus.Add(model);
                }

                return providerparticipentstatus;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCodes::LoadParticipentProvider", PROC_ProviderParticipentStatusLookup, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }
        public List<CaseAdjusterLookup> LookupCaseAdjuster()
        {
            List<CaseAdjusterLookup> CaseAdjuster = new List<CaseAdjusterLookup>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_CASE_ADJUSTER_LOOKUP);
                CaseAdjusterLookup model = null;
                while (reader.Read())
                {
                    model = new CaseAdjusterLookup();
                    model.CaseAdjusterId = reader["CaseAdjusterId"].ToString();
                    model.LastName = reader["LastName"].ToString();
                    model.FirstName = reader["FirstName"].ToString();
                    CaseAdjuster.Add(model);
                }
                return CaseAdjuster;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCodes::LoadCaseAdjuster", PROC_CASE_ADJUSTER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                dbManager.Dispose();
            }
        }
    }
}
