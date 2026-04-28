using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;
using MDVision.Model.Patient;

namespace MDVision.DataAccess.DAL.Patient
{
    public class DALPatientEmergencyContact
    {
        #region Variable
        
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPatientEmergencyContact"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALPatientEmergencyContact()
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

        #region "Stored Procedure Names"
        private const string PROC_PATIENT_EMERGENCYCONTACT_DELETE = "Patient.sp_EmergencyContactsDelete";
        private const string PROC_PATIENT_EMERGENCYCONTACT_INSERT = "Patient.sp_EmergencyContactsInsert";
        private const string PROC_PATIENT_EMERGENCYCONTACT_SELECT = "Patient.sp_EmergencyContactsSelect";
        private const string PROC_PATIENT_EMERGENCYCONTACTS_SELECT = "Patient.sp_EmergencyContactsSearch_New";
        private const string PROC_PATIENT_EMERGENCYCONTACT_UPDATE = "Patient.sp_EmergencyContactsUpdate";
        private const string PROC_PATIENADRESSMULTIFILTER_LOAD = "[Mobile].[sp_PatientVerification]";
        private const string PROC_PATIENTCONTACT_LOAD = "[Mobile].[sp_PatientPhoneSelect]";
        private const string PROC_PATIENTRELATIONS_LOAD = "[Patient].[sp_RelationShipLookup]";
        private const string PROC_APPOINTMENTCHECKIN_LOAD = "[Mobile].[sp_PatientUpdate]";
        private const string PROC_PATIENT_VISITTYPE_CHECK = "[Mobile].[sp_PatientVisitTypeCheck]";

        #endregion

        #region Parameters
        private const string PARM_CONTACT_ID = "@ContactId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_Last_Name = "@LastName";
        private const string PARM_First_Name = "@FirstName";
        private const string PARM_MI = "@MI";
        private const string PARM_DOB = "@DOB";
        private const string PARM_ADDRESS1 = "@Address1";
        private const string PARM_ADDRESS2 = "@Address2";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP_CODE = "@Zipcode";
        private const string PARM_ZIP_EXT = "@Zipext";
        private const string PARM_HOME_PHONE_NO = "@HomePhone";
        private const string PARM_WORK_PHONE_NO = "@WorkPhone";
        private const string PARM_WORK_PHONE_EXT = "@WorkPhext";
        private const string PARM_CELL_NO = "@CellNo";
        private const string PARM_FAX_NO = "@FaxNo";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_IS_PRIMARY = "@IsPrimary";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_RELATION_SHIP_ID = "@RelationShipId";
         private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSPatient ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(25);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CONTACT_ID, ds.EmergencyContacts.ContactIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CONTACT_ID, ds.EmergencyContacts.ContactIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.EmergencyContacts.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_Last_Name, ds.EmergencyContacts.LastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_First_Name, ds.EmergencyContacts.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_MI, ds.EmergencyContacts.MIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DOB, ds.EmergencyContacts.DOBColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_ADDRESS1, ds.EmergencyContacts.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_ADDRESS2, ds.EmergencyContacts.Address2Column.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CITY, ds.EmergencyContacts.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_STATE, ds.EmergencyContacts.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_ZIP_CODE, ds.EmergencyContacts.ZipcodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_ZIP_EXT, ds.EmergencyContacts.ZipextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_HOME_PHONE_NO, ds.EmergencyContacts.HomePhoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_WORK_PHONE_NO, ds.EmergencyContacts.WorkPhoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_WORK_PHONE_EXT, ds.EmergencyContacts.WorkPhextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_CELL_NO, ds.EmergencyContacts.CellNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_FAX_NO, ds.EmergencyContacts.FaxNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_EMAIL_ADDRESS, ds.EmergencyContacts.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_IS_PRIMARY, ds.EmergencyContacts.IsPrimaryColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(19, PARM_IS_ACTIVE, ds.EmergencyContacts.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(20, PARM_CREATED_BY, ds.EmergencyContacts.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_CREATED_ON, ds.EmergencyContacts.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(22, PARM_MODIFIED_BY, ds.EmergencyContacts.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_MODIFIED_ON, ds.EmergencyContacts.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(24, PARM_RELATION_SHIP_ID, ds.PatientInsurance.RelationShipIdColumn.ColumnName, DbType.Int32);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        /// <summary>
        /// Loads the patient emergency contact.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="EmergencyContactId">The emergency contact identifier.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient LoadPatientEmergencyContact(long PatientId, long EmergencyContactId)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);
                if (EmergencyContactId <= 0)
                    dbManager.AddParameters(0, PARM_CONTACT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CONTACT_ID, EmergencyContactId);

                if (PatientId <= 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);
                dbManager.AddParameters(2, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));
                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_EMERGENCYCONTACT_SELECT, ds, ds.EmergencyContacts.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::LoadPatientEmergencyContact", PROC_PATIENT_EMERGENCYCONTACT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPatient loadPatientEmergencyContacts(long PatientId,int pageNumber = 1, int rowsPerPage = 1000)
        {
            DSPatient ds = new DSPatient();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(5);
               
                if (PatientId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                    dbManager.AddParameters(1, PARM_ENTITY_ID, Convert.ToInt64(MDVSession.Current.EntityId));

                 if (pageNumber == 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, pageNumber);
                if (rowsPerPage == 0)
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, rowsPerPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.EmergencyContactsSearch.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSPatient)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_EMERGENCYCONTACTS_SELECT, ds, ds.EmergencyContactsSearch.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::loadPatientEmergencyContacts", PROC_PATIENT_EMERGENCYCONTACTS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<LoadMuiltPatientAddress> SearchPatienAdressMultiFilter(string NetWorkIP, string Ext, string Zip, string FirstName, string LastName, string DOB, string Gender, string AccountNo, string EntityId, string MobileNumber)
        {
            List<LoadMuiltPatientAddress> listobj = new List<LoadMuiltPatientAddress>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.AddParameters("@ZIPCodeExt", Ext);
                dbManager.AddParameters("@ZipCode", Zip);
                dbManager.AddParameters("@FirstName", FirstName);
                dbManager.AddParameters("@LastName", LastName);
                dbManager.AddParameters("@DOB",DOB);
                dbManager.AddParameters("@Gender", Gender);
                dbManager.AddParameters("@AccountNumber", AccountNo);
                dbManager.AddParameters("@MobileNumber", MobileNumber);
                listobj = dbManager.ExecuteReaderMapper<LoadMuiltPatientAddress>(PROC_PATIENADRESSMULTIFILTER_LOAD);

                return listobj;
            }
            catch (Exception e)
            {

                MDVLogger.DALErrorLog("DALPatientEmergencyContact::SearchPatienAdressMultiFilter", PROC_PATIENADRESSMULTIFILTER_LOAD, e);
                throw e;
            }


        }
        public List<LoadMuiltPatientAddress> LoadPatientContact(string NetWorkIP, string PatientId)
        {
            List<LoadMuiltPatientAddress> listobj = new List<LoadMuiltPatientAddress>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.AddParameters("@PatientId", PatientId);
                listobj = dbManager.ExecuteReaderMapper<LoadMuiltPatientAddress>(PROC_PATIENTCONTACT_LOAD);

                return listobj;
            }
            catch (Exception e)
            {

                MDVLogger.DALErrorLog("DALPatientEmergencyContact::SearchPatienAdressMultiFilter", PROC_PATIENTCONTACT_LOAD, e);
                throw e;
            }


        }

        public string LoadPatientVisitType(string PatientId)
        {
         
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string VisitType = "";
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@PatientId", PatientId);
                VisitType = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_VISITTYPE_CHECK).ToString();

                return VisitType;
            }
            catch (Exception e)
            {

                MDVLogger.DALErrorLog("DALPatientEmergencyContact::LoadPatientVisitType", PROC_PATIENT_VISITTYPE_CHECK, e);
                throw e;
            }


        }
        public List<LoadMuiltPatientAddress> SavePatientSignature(LoadMuiltPatientAddress model)
        {
            List<LoadMuiltPatientAddress> listobj = new List<LoadMuiltPatientAddress>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.AddParameters("@PatientId", model.PatientId);
                dbManager.AddParameters("@eSignature", model.SignaturePic);
                dbManager.AddParameters("@RelationShipId", model.RelationShipId);
                dbManager.AddParameters("@ModifiedBy", model.UserName);
                dbManager.AddParameters("@ModifiedOn", model.ModifiedOn);
                listobj = dbManager.ExecuteReaderMapper<LoadMuiltPatientAddress>(PROC_APPOINTMENTCHECKIN_LOAD);

                return listobj;
            }
            catch (Exception e)
            {

                MDVLogger.DALErrorLog("DALPatientEmergencyContact::SavePatientSignature", PROC_APPOINTMENTCHECKIN_LOAD, e);
                throw e;
            }


        }
        public List<LoadMuiltPatientAddress> LoadPatientRelations()
        {
            List<LoadMuiltPatientAddress> listobj = new List<LoadMuiltPatientAddress>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                listobj = dbManager.ExecuteReaderMapper<LoadMuiltPatientAddress>(PROC_PATIENTRELATIONS_LOAD);
                return listobj;
            }
            catch (Exception e)
            {

                MDVLogger.DALErrorLog("DALPatientEmergencyContact::LoadPatientRelations", PROC_PATIENTRELATIONS_LOAD, e);
                throw e;
            }


        }

        /// <summary>
        /// Updates the patient emergency contact.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient UpdatePatientEmergencyContact(DSPatient ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.EmergencyContacts.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSPatient)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_EMERGENCYCONTACT_UPDATE, ds, ds.EmergencyContacts.TableName);
                ds.AcceptChanges();
                if (dtTemp!=null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.EmergencyContacts.Rows[0][ds.EmergencyContacts.ContactIdColumn].ToString());
                    
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::UpdatePatientEmergencyContact", PROC_PATIENT_EMERGENCYCONTACT_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the patient emergency contact.
        /// </summary>
        /// <param name="EmergencyContactId">The emergency contact identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeletePatientEmergencyContact(string EmergencyContactId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_CONTACT_ID, EmergencyContactId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PATIENT_EMERGENCYCONTACT_DELETE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::DeletePatientEmergencyContact", PROC_PATIENT_EMERGENCYCONTACT_DELETE, ex);
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the patient emergency contact.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>DSPatient.</returns>
        public DSPatient InsertPatientEmergencyContact(DSPatient ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.EmergencyContacts.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateParameters(dbManager, ds, true);
                ds = (DSPatient)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_EMERGENCYCONTACT_INSERT, ds, ds.EmergencyContacts.TableName);
                ds.AcceptChanges();
                if (dtTemp!=null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.EmergencyContacts.Rows[0][ds.EmergencyContacts.ContactIdColumn].ToString());
                    
                    dsDBAudit.AcceptChanges(); 
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPatientEmergencyContact::InsertPatientEmergencyContact", PROC_PATIENT_EMERGENCYCONTACT_INSERT, ex);
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
