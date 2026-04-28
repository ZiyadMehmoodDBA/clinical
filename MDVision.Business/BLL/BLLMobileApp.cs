using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System.Data;
using MDVision.DataAccess.DAL.Patient;
using MDVision.DataAccess.DAL.Appointment;
using MDVision.DataAccess.DAL.Message;
using MDVision.DataAccess.DAL.Case;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;
using MDVision.DataAccess.DAL.Document;
using MDVision.DataAccess.DAL.PatientPortal;
using MDVision.DataAccess.DAL.Settings;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.DataAccess.DAL.Clinical.Patient;

using iTextSharp.text;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using HtmlAgilityPack;
using iTextSharp.text.pdf.draw;
using System.Web;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf.fonts;
using MDVision.DataAccess.DAL.Admin;
using MDVision.Model.Dashboard;
using MDVision.Model.Lookups;
using MDVision.Model;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.Model.Patient;
using MDVision.Model.Native.Patient;
using MDVision.Model.Native;
using MDVision.Model.FaceSheet;
using System.Web.Configuration;
using MDVision.Model.Clinical.Notes;
using MDVision.DataAccess.DAL.Admin.MobileApp;
using MDVision.Model.Native.Clinical;
using MDVision.Model.Native.Scheduler;

namespace MDVision.Business.BLL
{
   public class BLLMobileApp
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLPatient"/> class.
        /// </summary>
        public BLLMobileApp()
        {
            //SharedVariable
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call
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
        #region PatientsRegion
        public string SaveRecordInDBAuditNative(DataChangeRequest model)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();
       //     dbManager.BeginTransaction();

         


            try
            {
                string returnVal = new DALMobileApp().InsertupdateRecordInDBAuditNative(dbManager,model);
           //     dbManager.CommitTransaction();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientEmergencyContact", ex);
                dbManager.RollBackTransaction();
                return (ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        public int CheckExistingPatients(LoadMuiltPatientAddress model)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();
            try
            {
                int returnVal = new DALMobileApp().CheckExistingPatients(dbManager, model.FirstName == "" ? null : model.FirstName, model.LastName == "" ? null : model.LastName, model.DOB, model.MobileNumber, model.Gender == "" ? null : model.Gender);
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::CheckExistingPatients", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string CheckExistingPatientByInsurance(string SubscriberId,string expiryDate)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();
            try
            {
                string returnVal = new DALMobileApp().CheckExistingPatientByInsuranceId(dbManager, SubscriberId, expiryDate);
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::CheckExistingPatientByInsuranceId", ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public int CheckExistingRecord( string PatientId, string DbTableName, string ColumnKeyName)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();
            //     dbManager.BeginTransaction();




            try
            {
                int returnVal = new DALMobileApp().CheckExistingRecord(dbManager,    PatientId,  DbTableName, ColumnKeyName);
                //     dbManager.CommitTransaction();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientEmergencyContact", ex);
                dbManager.RollBackTransaction();
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public int loadMaxInsurancesPriority(long PatientId)
        {
            try
            {

                int returnVal = new DALMobileApp().loadMaxPatientInsurancesPriority(PatientId);
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                throw ex;
            }
        }
        public int CheckPlanPriorityAgainstInsuranceId(string PatientId, string DbTableName, string InsuranceId)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();
            //     dbManager.BeginTransaction();




            try
            {
                int returnVal = new DALMobileApp().CheckPlanPriority(dbManager, PatientId, DbTableName, InsuranceId);
                //     dbManager.CommitTransaction();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientEmergencyContact", ex);
                dbManager.RollBackTransaction();
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public int GetMaxPlanPriorityFromDbAuditNative(string PatientId)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();
            //     dbManager.BeginTransaction();




            try
            {
                int returnVal = new DALMobileApp().GetMaxPlanPriorityFromDbAuditNative(dbManager, PatientId);
                //     dbManager.CommitTransaction();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientEmergencyContact", ex);
                dbManager.RollBackTransaction();
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public int CheckExistingRecordForInsurance(string PatientId, long InsurancePlanId, string DbTableName)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();
            //     dbManager.BeginTransaction();

            try
            {
                int returnVal = new DALMobileApp().CheckExistingRecordForInsurance(dbManager, PatientId,InsurancePlanId, DbTableName);
                //     dbManager.CommitTransaction();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientEmergencyContact", ex);
                dbManager.RollBackTransaction();
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string SaveRecordInDBAuditNative(List< DataChangeRequest> lstmodel)
        {
           
            try
            {
                string returnVal = new DALMobileApp().InsertupdateRecordInDBAuditNative(lstmodel);
                //     dbManager.CommitTransaction();
                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientEmergencyContact", ex);
             
                return (ex.Message);
            }
            
        }
        public BLObject<DSProcedures> insertCPTLookup(DSProcedures ds)
        {
            try
            {
                ds = new DALMobileApp().InsertCPTLookup(ref ds);
                return new BLObject<DSProcedures>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::insertCPTLookup", ex);
                return new BLObject<DSProcedures>(null, ex.Message);
            }
        }
        public BLObject<DSCodeLookup> InsertICDLookup(ref DSCodeLookup ds)
        {
            try
            {
                ds = new DALMobileApp().InsertICDLookup(ref ds);
                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertICDLookup", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }
        public List<PatientEmergencyContactModel> loadPatientEmergencyContacts(long PatientId, string RequestStatus, int pageNumber = 1, int rowsPerPage = 1000)
        {
            try
            {
                List<PatientEmergencyContactModel> lstPEC = new List<PatientEmergencyContactModel>();
                lstPEC = new DALMobileApp().loadPatientEmergencyContacts(PatientId,RequestStatus, pageNumber, rowsPerPage);
                return lstPEC;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                throw ex;
            }
        }
        public BLObject<DSHospitalizationHx> loadHospitalizationHxDisease(long PatientId, string RequestStatus, Int64 HospitalizationHxDiseaseId = 0)
        {
            try
            {
                DSHospitalizationHx ds = new DSHospitalizationHx();
                ds = new DALMobileApp().LoadHospitalizationHx_Disease(PatientId, RequestStatus,HospitalizationHxDiseaseId);
                return new BLObject<DSHospitalizationHx>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadHospitalizationHxDisease", ex);
                return new BLObject<DSHospitalizationHx>(null, ex.Message);
            }
        }

        public DataTable loadFamilyHxDisease(long PatientId, string RequestStatus, long FamilyMemberId, Int64 FamilyHxDiseaseId = 0)
        {
            DataTable dtFamilyMemberDiseases = new DataTable();
            try
            {
                
                dtFamilyMemberDiseases = new DALMobileApp().LoadFamilyHx_Disease(PatientId, RequestStatus,FamilyMemberId,FamilyHxDiseaseId);
                return dtFamilyMemberDiseases;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMobileApp::loadFamilyHxDisease", ex);
                return dtFamilyMemberDiseases;
            }
        }

        public DataTable loadFamilyHxMembers(long PatientId, string RequestStatus)
        {
            DataTable dtFamilyMember = new DataTable();
            try
            {                
                dtFamilyMember = new DALMobileApp().LoadFamilyHx_Members(PatientId, RequestStatus);
                return dtFamilyMember;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMobileApp::loadFamilyHxMembers", ex);
                return dtFamilyMember;
            }
        }

        public BLObject<DSSurgicalHx> loadSurgicalHxDisease(long PatientId, string RequestStatus, Int64 SurgicalHxDiseaseId = 0)
        {
            try
            {
                DSSurgicalHx ds = new DSSurgicalHx();
                ds = new DALMobileApp().LoadSurgicalHx_Disease(PatientId, RequestStatus, SurgicalHxDiseaseId);
                return new BLObject<DSSurgicalHx>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadSurgicalHxDisease", ex);
                return new BLObject<DSSurgicalHx>(null, ex.Message);
            }
        }


        public List<PatientInsuranceModel> loadPatientInsurances(long PatientId, string RequestStatus)
        {
            try
            {
                List<PatientInsuranceModel> lstPEM = new List<PatientInsuranceModel>();
                lstPEM = new DALMobileApp().loadPatientInsurances(PatientId, RequestStatus);
                return lstPEM;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                throw ex;
            }
        }
        
        public PatientEmergencyContactModel FillPatientEmergencyContact(long PatientId, long EmergencyContactId, string RequestStatus)
        {
            try
            {
                PatientEmergencyContactModel PEC = null;
                PEC = new DALMobileApp().FillPatientEmergencyContact(PatientId, EmergencyContactId,  RequestStatus);
                return PEC;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                throw ex;
            }
        }
        public PatientInsuranceModel FillPatientInsurance(long PatientId, long InsuranceId, string RequestStatus)
        {
            try
            {
                PatientInsuranceModel PEM = null;
                PEM = new DALMobileApp().FillPatientInsurance(PatientId, InsuranceId,  RequestStatus);
                return PEM;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                throw ex;
            }
        }
        public EmptySlotModel FillPatientAppointment(long PatientId,string RequestStatus)
        {
            try
            {
              return new DALMobileApp().FillPatientAppointment(PatientId, RequestStatus);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMobileApp::FillPatientScheduler", ex);
                throw ex;
            }
        }
        
        public string DiscardRecord(long PatientID, long ColumnkeyId, string DBTableName,string changedColumnsString)
        {
            try
            {
                string ReturnVal = "";
               
                ReturnVal = new DALMobileApp().DiscardRecord(PatientID, ColumnkeyId, DBTableName, changedColumnsString);
                return ReturnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                throw ex;
            }
        }
        public string DiscardAllRecord(long PatientID)
        {
            try
            {
                string ReturnVal = "";

                ReturnVal = new DALMobileApp().DiscardAllRecord(PatientID);
                return ReturnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                throw ex;
            }
        }
        public string ApproveAllRecord(long PatientID)
        {
            try
            {
                string ReturnVal = "";

                ReturnVal = new DALMobileApp().ApproveAllRecord(PatientID);
                return ReturnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                throw ex;
            }
        }
        public PatientPreferenceModel FillPatientPreferences(long PatientId,string RequestStatus)
        {
            try
            {
                PatientPreferenceModel PP = null;
                PP = new DALMobileApp().FillPatientPreferences(PatientId, RequestStatus);
                return PP;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                throw ex;
            }
        }
        public string InsertPatientEmergencyContact(PatientEmergencyContactModel PEC)
        {
            try
            {
               
              string result=   new DALMobileApp().InsertPatientEmergencyContact(PEC);
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                throw ex;
            }
        }
        public string InsertPatientInsurance(PatientInsuranceModel PEC)
        {
            try
            {

                string result = new DALMobileApp().InsertPatientInsurance(PEC);
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientInsurance", ex);
                throw ex;
            }
        }
        public string UpdatePatientInsurance(PatientInsuranceModel PEC)
        {
            try
            {

                string result = new DALMobileApp().UpdatePatientInsurance(PEC);
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientInsurance", ex);
                throw ex;
            }
        }
        public string UpdatePatientEmergencyContact(PatientEmergencyContactModel PEC)
        {
            try
            {

                string result = new DALMobileApp().UpdatePatientEmergencyContact(PEC);
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                throw ex;
            }
        }
        public string UpdatePatientPreferences(PatientPreferenceModel Model)
        {

            try
            {

               
               string result = new DALMobileApp().UpdatePatientPreferences(Model);
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientPreferencesNative", ex);
                throw ex;
            }
        }
        #endregion
        public BLObject<List<MobileAppRequestModel>> LoadDashboardCheckInPatients(string Status, Int64 ProviderId,Int64 PatientId, long PageNumber = 1, long RowsPerPage = 15)
        {
            try
            {

                var result = new DALMobileApp().LoadDashboardCheckInPatients(Status, ProviderId, PatientId, PageNumber, RowsPerPage);
                return new BLObject<List<MobileAppRequestModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadDashboardTCMPatients", ex);
                return new BLObject<List<MobileAppRequestModel>>(null, ex.Message);
            }
        }

        public BLObject<List<MobileAppRequestModel>> LoadDashboardCheckInPatientsRequest(string Status, Int64 ProviderId, Int64 PatientId, long PageNumber = 1, long RowsPerPage = 15)
        {
            try
            {

                var result = new DALMobileApp().LoadDashboardCheckInPatientsRequest(Status, ProviderId, PatientId, PageNumber, RowsPerPage);
                return new BLObject<List<MobileAppRequestModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadDashboardTCMPatients", ex);
                return new BLObject<List<MobileAppRequestModel>>(null, ex.Message);
            }
        }
        public PatientDemographicModelNative FillPatient_Native(long? PatientId,string DimmyPatientId,string RequestStatus)
        {
            DALUsersActivity obj = new DALUsersActivity();

            PatientDemographicModelNative model = null;
            try
            {
                model = new PatientDemographicModelNative();
                model = new DALPatient().FillPatientNative(PatientId, DimmyPatientId, RequestStatus);
                //if (ds.Tables[ds.Patients.TableName].Rows.Count > 0)
                //{

                //    AccountNumber = ds.Patients.Rows[0][ds.Patients.AccountNumberColumn].ToString();
                //}


                //  obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.View, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Patient viewed", AccountNumber);
                //ds.AcceptChanges();
                return model;
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.View, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error during patient view : " + ex.ToString());
                MDVLogger.BLLErrorLog("BLLPatient::FillPatient_ID", ex);
             throw ex;
            }
        }
        public string UpdatePatientDemographicsNative(PatientDemographicModelNative model)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();


            try
            {
                dbManager.BeginTransaction();
                string patientId;





                patientId = new DALPatient(true).updateDemographicsNative(dbManager, model);

                dbManager.CommitTransaction();
                return (patientId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientDemographicsNative", ex);
                dbManager.RollBackTransaction();
                return (ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdatePatientInDBAuditNative(SavePatientNative model)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();
            dbManager.BeginTransaction();

            try
            {



                List<DataChangeRequest> lstdatachangeRequest = new List<DataChangeRequest>();

                lstdatachangeRequest = model.patientDemographic_JSON.DataChangeRequest;

                string Result = "";

                foreach (var item in lstdatachangeRequest)
                {
                    item.ColumnKeyId = model.patientDemographic_JSON.PatientID;
                    item.ColumnKeyName = "PatientId";
                    item.CreatedBy = ClientConfiguration.DecryptFrom64(model.UserName);
                    item.CreatedOn = DateTime.Now;
                    item.ModifiedBy = ClientConfiguration.DecryptFrom64(model.UserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = Convert.ToInt64(model.patientDemographic_JSON.PatientID);
                    item.IsSynced = false;
                    item.EntityId = model.EntityId;
                    item.DBTableName = "Patients";

                    string returnVal = new DALPatient(true).InsertupdateDemographicsInDBAuditNative(dbManager, item);

                    //    item.DbAuditId =Convert.ToInt64( DBAuditId);

                    //   string result=   new DALPatient(true).InsertDemographicsInDBAuditDetailNative(dbManager, item);

                    if (returnVal != "")
                    {
                        Result = Result + "  " + returnVal;
                    }














                }


                //   patientId = new DALPatient(true).updateDemographicsNative(dbManager, model.patientDemographic_JSON);

                dbManager.CommitTransaction();
                return Result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientDemographicsNative", ex);
                dbManager.RollBackTransaction();
                return (ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string SavePatientEmergencyContactInDBAuditNative(PatientEmergencyContactModel model)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();
            dbManager.BeginTransaction();

            try
            {



                //List<DataChangeRequest> lstdatachangeRequest = new List<DataChangeRequest>();

                //lstdatachangeRequest = model.DataChangedRequest;

                string Result = "";

                foreach (var item in model.DataChangeRequest)
                {
                    item.ColumnKeyId = model.ContactId;
                    item.ColumnKeyName = "ContactId";
                    item.CreatedBy = ClientConfiguration.DecryptFrom64(model.UserName);
                    item.CreatedOn = DateTime.Now;
                    item.ModifiedBy = ClientConfiguration.DecryptFrom64(model.UserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = Convert.ToInt64(model.PatientId);
                    item.IsSynced = false;
                    item.EntityId = model.EntityId;
                    item.DBTableName = "EmergencyContacts";

                    string returnVal = new DALPatient(true).InsertupdateDemographicsInDBAuditNative(dbManager, item);

                    //    item.DbAuditId =Convert.ToInt64( DBAuditId);

                    //   string result=   new DALPatient(true).InsertDemographicsInDBAuditDetailNative(dbManager, item);

                    if (returnVal != "")
                    {
                        Result = Result + "  " + returnVal;
                    }














                }


                //   patientId = new DALPatient(true).updateDemographicsNative(dbManager, model.patientDemographic_JSON);

                dbManager.CommitTransaction();
                return Result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientDemographicsNative", ex);
                dbManager.RollBackTransaction();
                return (ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public BirthHxNewbornModelNative FillBirthHx_NewBorn(long PatientId, string RequestStatus)
        {
            try
            {
                BirthHxNewbornModelNative BNM = null;
                BNM = new DALMobileApp().FillBirthHx_NewBorn(PatientId, RequestStatus);
                if (BNM != null)
                {
                    if (BNM.bFetalDistress != "")
                    {
                        if (BNM.bFetalDistress.ToLower() == "true")
                        {
                            BNM.bFetalDistressYes = "true";
                            BNM.bFetalDistressNo = "false";
                        }
                        else
                        {
                            BNM.bFetalDistressYes = "false";
                            BNM.bFetalDistressNo = "true";
                        }
                    }
                    else {
                        BNM.bFetalDistressYes = "false";
                        BNM.bFetalDistressNo = "false";
                    }
                }
                return BNM;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMobileApp::FillBirthHx_NewBorn", ex);
                throw ex;
            }
        }

        public BirthHxGeneralModelNative FillBirthHx_General(long PatientId, string RequestStatus)
        {
            try
            {
                BirthHxGeneralModelNative BNM = null;
                BNM = new DALMobileApp().FillBirthHx_General(PatientId, RequestStatus);
                return BNM;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMobileApp::FillBirthHx_General", ex);
                throw ex;
            }
        }

        public BirthHxMaternalDeliveryModelNative FillBirthHx_Maternal(long PatientId, string RequestStatus)
        {
            try
            {
                BirthHxMaternalDeliveryModelNative BNM = null;
                BNM = new DALMobileApp().FillBirthHx_Maternal(PatientId, RequestStatus);
                return BNM;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMobileApp::FillBirthHx_Maternal", ex);
                throw ex;
            }
        }

        public EmptySlotModel GetNearestEmptySlot( string providerId, string facilityId)
        {
            try
            {
                return new DALMobileApp().GetNearestEmptySlot(   providerId,  facilityId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMobileApp::GetNearestEmptySlot", ex);
                throw ex;
            }
        }

    }
}
