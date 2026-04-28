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
using MDVision.Model.FaceSheet;
using System.Web.Configuration;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Common;
using MDVision.Model.Document;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using MDVision.Model.Clinical.LegacyNotes;

namespace MDVision.Business.BLL
{
    public class BLLPatient
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLPatient"/> class.
        /// </summary>
        public BLLPatient()
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

        #region Variable

        #endregion

        #region "Lawyer"
        /// <summary>
        /// Loads the lawyer.
        /// </summary>
        /// <param name="LawyerId">The lawyer identifier.</param>
        /// <param name="LawyerName">Name of the lawyer.</param>
        /// <param name="FirmName">Name of the firm.</param>
        /// <param name="Address">The address.</param>
        /// <param name="City">The city.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>BLObject&lt;DSPatientProfile&gt;.</returns>
        public BLObject<DSPatientProfile> LoadLawyer(long LawyerId, string LawyerName, string FirmName, string Address, string City, string IsActive)
        {
            try
            {
                DSPatientProfile ds = new DSPatientProfile();
                ds = new DALLawyer().LoadLawyer(LawyerId, LawyerName, FirmName, Address, City, IsActive);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadLawyer", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the lawyer.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatientProfile&gt;.</returns>
        public BLObject<DSPatientProfile> InsertLawyer(DSPatientProfile ds)
        {
            try
            {
                ds = new DALLawyer().InsertLawyer(ds);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertLawyer", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the lawyer.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatientProfile&gt;.</returns>
        public BLObject<DSPatientProfile> UpdateLawyer(DSPatientProfile ds)
        {
            try
            {
                ds = new DALLawyer().UpdateLawyer(ds);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdateLawyer", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the lawyer.
        /// </summary>
        /// <param name="LawyerId">The lawyer identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteLawyer(string LawyerId)
        {
            try
            {
                LawyerId = new DALLawyer().DeleteLawyer(LawyerId);
                return new BLObject<string>(LawyerId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeleteLawyer", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        #endregion

        #region "Employer"

        /// <summary>
        /// Loads the employer.
        /// </summary>
        /// <param name="LawyerId">The lawyer identifier.</param>
        /// <param name="LawyerName">Name of the lawyer.</param>
        /// <param name="FirmName">Name of the firm.</param>
        /// <param name="Address">The address.</param>
        /// <param name="City">The city.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>BLObject&lt;DSPatientProfile&gt;.</returns>
        public BLObject<DSPatientProfile> LoadEmployer(long EmployerId, string EmployerName, string Address, string City, string State, string Zip, string IsActive, string zipExt = "")
        {
            try
            {
                DSPatientProfile ds = new DSPatientProfile();
                ds = new DALEmployer().LoadEmployer(EmployerId, EmployerName, Address, City, State, Zip, IsActive, zipExt);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadEmployer", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the employer.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatientProfile&gt;.</returns>
        public BLObject<DSPatientProfile> InsertEmployer(DSPatientProfile ds)
        {
            try
            {
                ds = new DALEmployer().InsertEmployer(ds);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertEmployer", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the employer.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatientProfile&gt;.</returns>
        public BLObject<DSPatientProfile> UpdateEmployer(DSPatientProfile ds)
        {
            try
            {
                ds = new DALEmployer().UpdateEmployer(ds);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdateEmployer", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the employer.
        /// </summary>
        /// <param name="EmployerId">The employer identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteEmployer(string EmployerId)
        {
            try
            {
                EmployerId = new DALEmployer().DeleteEmployer(EmployerId);
                return new BLObject<string>(EmployerId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeleteEmployer", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "School"

        /// <summary>
        /// Loads the school.
        /// </summary>
        /// <param name="SchoolId">The school identifier.</param>
        /// <param name="SchoolName">Name of the school.</param>
        /// <param name="Address">The address.</param>
        /// <param name="City">The city.</param>
        /// <param name="Zip">The zip.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>BLObject&lt;DSPatientProfile&gt;.</returns>
        public BLObject<DSPatientProfile> LoadSchool(long SchoolId, string SchoolName, string Address, string City, string State, string Zip, string IsActive)
        {
            try
            {
                DSPatientProfile ds = new DSPatientProfile();
                ds = new DALSchool().LoadSchool(SchoolId, SchoolName, Address, City, State, Zip, IsActive);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadSchool", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }


        /// <summary>
        /// Inserts the school.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatientProfile&gt;.</returns>
        public BLObject<DSPatientProfile> InsertSchool(DSPatientProfile ds)
        {
            try
            {
                ds = new DALSchool().InsertSchool(ds);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertSchool", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }


        /// <summary>
        /// Updates the school.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatientProfile&gt;.</returns>
        public BLObject<DSPatientProfile> UpdateSchool(DSPatientProfile ds)
        {
            try
            {
                ds = new DALSchool().UpdateSchool(ds);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdateSchool", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }


        /// <summary>
        /// Deletes the school.
        /// </summary>
        /// <param name="SchoolId">The school identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteSchool(string SchoolId)
        {
            try
            {
                SchoolId = new DALSchool().DeleteSchool(SchoolId);
                return new BLObject<string>(SchoolId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeleteSchool", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "Guarantor"
        /// <summary>
        /// Loads the guarantor.
        /// </summary>
        /// <param name="GuarantorId">The guarantor identifier.</param>
        /// <param name="FirstName">The first name.</param>
        /// <param name="LastName">The last name.</param>
        /// <param name="Relation">The relation.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>BLObject&lt;DSPatientProfile&gt;.</returns>
        public BLObject<DSPatientProfile> LoadGuarantor(long GuarantorId, string FirstName, string LastName, string RelationId, string IsActive, Int64 PatientID = 0, int PageNumber = 1, int RowspPage = 15)
        {
            try
            {
                DSPatientProfile ds = new DSPatientProfile();
                ds = new DALGuarantor().LoadGuarantor(GuarantorId, FirstName, LastName, RelationId, IsActive, PatientID, PageNumber, RowspPage);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLGuarantor::LoadGuarantor", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// LoadPatientGuarantor
        /// </summary>
        /// <param name="GuarantorId"></param>
        /// <param name="PatientId"></param>
        /// <param name="IsActive"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowspPage"></param>
        /// <returns></returns>
        public BLObject<DSPatientProfile> LoadPatientGuarantor(long GuarantorId, long PatientId, string IsActive,
            int PageNumber = 1, int RowspPage = 15)
        {
            try
            {
                DSPatientProfile ds = new DSPatientProfile();
                ds =
                    new DALGuarantor().LoadPatientGuarantor(GuarantorId, PatientId, IsActive, PageNumber, RowspPage);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLGuarantor::LoadPatientGuarantor", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }


        /// <summary>
        /// Inserts the guarantor.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatientProfile&gt;.</returns>
        public BLObject<DSPatientProfile> InsertGuarantor(DSPatientProfile ds)
        {
            try
            {
                ds = new DALGuarantor().InsertGuarantor(ds);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLGuarantor::InsertGuarantor", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the guarantor.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatientProfile&gt;.</returns>
        public BLObject<DSPatientProfile> UpdateGuarantor(DSPatientProfile ds)
        {
            try
            {
                ds = new DALGuarantor().UpdateGuarantor(ds);
                return new BLObject<DSPatientProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLGuarantor::UpdateGuarantor", ex);
                return new BLObject<DSPatientProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the guarantor.
        /// </summary>
        /// <param name="GuarantorId">The guarantor identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteGuarantor(string GuarantorId)
        {
            try
            {
                GuarantorId = new DALGuarantor().DeleteGuarantor(GuarantorId);
                return new BLObject<string>(GuarantorId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLGuarantor::DeleteGuarantor", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the relation.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupRelation()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALGuarantor().LookupRelation();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupRelation", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        public BLObject<List<RelationModel>> LookupRelationDemographic()
        {
            try
            {
                var ds = new DALGuarantor().LookupRelationDemographic();
                return new BLObject<List<RelationModel>>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupRelationDemographic", ex);
                return new BLObject<List<RelationModel>>(null, ex.Message);
            }

        }
        /// <summary>
        /// Lookups the guarantor.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSPatientLookups> LookupGuarantor()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALGuarantor().LookupGuarantor();
                return new BLObject<DSPatientLookups>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupGuarantor", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }
        }

        public BLObject<DSPatientLookups> LookupGuarantor(string name)
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALGuarantor().LookupGuarantor(name);
                return new BLObject<DSPatientLookups>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupGuarantor", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }
        }



        #endregion

        #region "Patient"
        public Model.Patient.DemographicLabelModel getDemographicData(long PatientId)
        {
            try
            {
                var data = new DALUser().getDemographicData(PatientId);
                return data;
            }
            catch (Exception e)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::Login", e);
                return null;
            }
        }
        public BLObject<DSPatient> FillPatientAndInsuranceById(long PatientId, long AppointmentId = 0, short isFileStream = 0, string FormName = "")
        {
            try
            {
                DSPatient ds = new DSPatient();

                // ds = new DALPatient().FillPatient(PatientId,  "","");
                ds = new DALPatient().FillPatientPMS(PatientId, FormName);
                ds.Merge(new DALPatientInsurance().LoadPatientInsurance(0, PatientId, isFileStream));
                if (AppointmentId > 0)
                {
                    ds.Merge(new DALAppointment().LoadAppointment(AppointmentId, PatientId.ToString(), "", "", ""));
                    ds.Merge(new DALAppointment().LoadAppointmentCheckOutSelect(AppointmentId));
                }
                //ds.Merge(new DALPatientEmergencyContact().LoadPatientEmergencyContact(PatientId, 0));
                //ds.Merge(new DALPatientFamily().LoadPatientFamily(PatientId,0));
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::FillPatientAndInsuranceById", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> FillPatient_PictureById(long PatientId, string FormName = "")
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSPatient ds = new DSPatient();
            string AccountNumber = "";
            try
            {

                ds = new DALPatient().FillPatientPMS(PatientId, FormName);
                if (ds.Tables[ds.Patients.TableName].Rows.Count > 0)
                {
                    //DataRow dr = ds.Tables[ds.Patients.TableName].Rows[0];

                    //string ImagePath = "";

                    //if (dr[ds.Patients.PatientImageColumn.ColumnName].ToString() != "")
                    //{
                    //    ImagePath = MDVUtility.CreateImageFile((byte[])dr[ds.Patients.PatientImageColumn.ColumnName], .ImagePath, MDVSession.Current.EntityId, dr[ds.Patients.PatientIdColumn.ColumnName].ToString().Trim(), "jpg");
                    //    dr[ds.Patients.ImagePathColumn.ColumnName] = ImagePath;
                    //}
                    AccountNumber = ds.Patients.Rows[0][ds.Patients.AccountNumberColumn].ToString();
                }


                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.View, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Patient viewed", AccountNumber);
                ds.AcceptChanges();
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.View, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error during patient view : " + ex.ToString());
                MDVLogger.BLLErrorLog("BLLPatient::FillPatient_PictureById", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> GetRaceCodes(long PatientId)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatient().GetRaceCodes(PatientId);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::GetRaceCodes", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> GetEthnicityCodes(long PatientId)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatient().GetEthnicityCodes(PatientId);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::GetEthnicityCodes", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> FillPatientById(long PatientId, string FormName = "")
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSPatient ds = new DSPatient();
            string AccountNumber = "";
            try
            {

                ds = new DALPatient().FillPatientPMS(PatientId, FormName);
                if (ds.Patients.Rows.Count > 0)
                    AccountNumber = ds.Patients.Rows[0][ds.Patients.AccountNumberColumn].ToString();

                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.View, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Patient viewed", AccountNumber);

                ds.AcceptChanges();
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.View, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error during patient view : " + ex.ToString());

                MDVLogger.BLLErrorLog("BLLPatient::FillPatientById", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        public BLObject<DSPatient> LookupPatient(long PatientId, string AccountNo, string IsActive)
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSPatient ds = new DSPatient();
            try
            {

                ds = new DALPatient().LookupPatient(PatientId, AccountNo, IsActive);
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Search, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Patient search");
                ds.AcceptChanges();
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Search, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error during patient search : " + ex.ToString());
                MDVLogger.BLLErrorLog("BLLPatient::LookupPatient", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<List<PatientRepresentativeLookupModel>> LookupDocumentSource()
        {
            try
            {
                var obj = new DALPatient().LookupDocumentSource();
                return new BLObject<List<PatientRepresentativeLookupModel>>(obj);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupDocumentSource", ex);
                return new BLObject<List<PatientRepresentativeLookupModel>>(null, ex.Message);
            }
        }


        public BLObject<DSPatient> LookupPatientByName(long PatientId, string Searchstring, string IsActive)
        {
            DALUsersActivity obj = new DALUsersActivity();
            DSPatient ds = new DSPatient();
            try
            {

                ds = new DALPatient().LookupPatientByName(PatientId, Searchstring, IsActive);
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Search, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Patient search");
                ds.AcceptChanges();
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Search, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error during patient search : " + ex.ToString());
                MDVLogger.BLLErrorLog("BLLPatient::LookupPatientByName", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        /// <summary>
        /// Loads the patient.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="FirstName">The first name.</param>
        /// <param name="LastName">The last name.</param>
        /// <param name="AccountNumber">The account number.</param>
        /// <param name="SSN">The SSN.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public BLObject<DSPatient> LoadPatient(DSPatient ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            try
            {

                ds = new DALPatient().LoadPatient(ds);
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Search, DALUsersActivity.TITLE_SEARCH_PATIENT, true, "Patient search");


                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Search, DALUsersActivity.TITLE_SEARCH_PATIENT, false, "Error during patient search : " + ex.ToString());
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatient", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        public BLObject<DSPatient> PatientsSearch(DSPatient ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            try
            {

                ds = new DALPatient().PatientsSearch(ds);
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Search, DALUsersActivity.TITLE_SEARCH_PATIENT, true, "Patient search");


                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Search, DALUsersActivity.TITLE_SEARCH_PATIENT, false, "Error during patient search : " + ex.ToString());
                MDVLogger.BLLErrorLog("BLLPatient::PatientsSearch", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the patient.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSPatient> InsertPatient(DSPatient ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            string AccountNumber = "";
            try
            {


                ds = new DALPatient().InsertPatient(ds);

                if (ds.Patients.Rows.Count > 0)
                    AccountNumber = ds.Patients.Rows[0][ds.Patients.AccountNumberColumn].ToString();

                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Patient saved", AccountNumber);


                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error during patient save : " + ex.ToString());

                MDVLogger.BLLErrorLog("BLLPatient::InsertPatient", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> InsertPatient_CCDA(DSPatient ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            string AccountNumber = "";
            try
            {


                ds = new DALPatient().InsertPatient_CCDA(ds);

                if (ds.Patients.Rows.Count > 0)
                    AccountNumber = ds.Patients.Rows[0][ds.Patients.AccountNumberColumn].ToString();

                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Patient saved", AccountNumber);


                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error during patient save : " + ex.ToString());

                MDVLogger.BLLErrorLog("BLLPatient::InsertPatient_CCDA", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> InsertPatientsRcopialID(DSPatient ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            string AccountNumber = "";
            try
            {


                ds = new DALPatient().InsertPatientsRcopialID(ds);

                if (ds.Patients.Rows.Count > 0)
                    AccountNumber = ds.Patients.Rows[0][ds.Patients.AccountNumberColumn].ToString();

                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Patient saved", AccountNumber);


                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error during patient save : " + ex.ToString());

                MDVLogger.BLLErrorLog("BLLPatient::InsertPatient", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        /// <summary>
        /// Updates the patient.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSPatient> UpdatePatient(DSPatient ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            string AccountNumber = "";
            try
            {
                //if (ds.Tables[ds.Patients.TableName].Rows.Count > 0)
                //{
                //    DataRow dr = ds.Tables[ds.Patients.TableName].Rows[0];

                //    if (dr[ds.Patients.PatientImageColumn.ColumnName].ToString() == "")
                //    {

                //        dr[ds.Patients.PatientImageColumn.ColumnName] = MDVUtility.GetImageFile(.ImagePath, MDVSession.Current.EntityId, dr[ds.Patients.PatientIdColumn.ColumnName].ToString().Trim(), "jpg");
                //    }

                //}


                ds = new DALPatient().UpdatePatient(ds);

                if (ds.Patients.Rows.Count > 0)
                    AccountNumber = ds.Patients.Rows[0][ds.Patients.AccountNumberColumn].ToString();

                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_DEMOGRAPHIC, true, "Patient updated", AccountNumber);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error during patient update : " + ex.ToString());
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatient", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }


        public BLObject<DSPatient> UpdatePatientPic(DSPatient ds, string FilePath)
        {
            DALUsersActivity obj = new DALUsersActivity();
            try
            {
                ds = new DALPatient().UpdatePatientPic(ds, FilePath);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error during patient pic update : " + ex.ToString());
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientPic", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        public string DeletePatientPicNative(string ModifiedBy, DateTime ModifiedOn, Int64 PatientId)
        {
            string returnVal = "";
            try
            {
                returnVal = new DALPatient().DeletePatientPicNative(ModifiedBy, ModifiedOn, PatientId);
                return returnVal;
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientPicNative", ex);
                return ex.Message;
            }
        }
        /// <summary>
        /// Deletes the patient.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeletePatient(string PatientId)
        {
            try
            {
                PatientId = new DALPatient().DeletePatient(PatientId);
                return new BLObject<string>(PatientId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeletePatient", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public string GetIsDataPrivacy()
        {
            try
            {
                string IsDataPrivacy = "";
                IsDataPrivacy = new DALPatient().GetIsDataPrivacy();
                return IsDataPrivacy;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeletePatient", ex);
                return ex.Message;
            }
        }

        public void InsertBillingInquiryEmail(BillingInquiryEmailModel model, string MailFrom, string MailTo)
        {
            try
            {
                new DALPatient().InsertBillingInquiryEmail(model, MailFrom, MailTo);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMessage::InsertBillingInquiryEmail", ex);
            }
        }

        //public BLObject<string> DeletePatientPicture(string ModifiedBy, DateTime ModifiedOn, Int64 PatientId)
        //{
        //    try
        //    {
        //      //  string Result = "";
        //     //   result = new DALPatient().DeletePatientPicNative(ModifiedBy, ModifiedOn,PatientId);
        //     //   return new BLObject<string>(PatientId);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLPatient::DeletePatient", ex);
        //        return new BLObject<string>(null, ex.Message);
        //    }
        //}
        public List<Model.Patient.PatientReminderLog> getReminderLogData(long PatientId)
        {
            try
            {
                var data = new DALPatient().getReminderLogData(PatientId);
                return data;
            }
            catch (Exception e)
            {
                MDVLogger.BLLErrorLog("getReminderLogData::Patient", e);
                return null;
            }
        }
        public List<Model.Patient.PatientFaxLog> getFaxLogData(long PatientId)
        {
            try
            {
                var data = new DALPatient().getFaxLogData(PatientId);
                return data;
            }
            catch (Exception e)
            {
                MDVLogger.BLLErrorLog("getFaxLogData::Patient", e);
                return null;
            }
        }
        #endregion

        #region "Patient Insurance"
        /// <summary>
        /// Loads the patient insurance.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> LoadPatientInsurance(long PatientInsuranceId, long PatientId, short isFileStream = 0)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientInsurance().LoadPatientInsurance(PatientInsuranceId, PatientId, isFileStream);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientInsurance", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        public BLObject<DSPatient> GetPatientRelationshipInfo(long PatientId)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientInsurance().GetPatientRelationshipInfo(PatientId);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::GetPatientRelationshipInfo", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> LoadPatientVisitInsurance(long visitId, long patientId)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientInsurance().LoadPatientVisitInsurance(visitId, patientId);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientInsurance", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }


        /// <summary>
        /// Inserts the patient insurance.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> InsertPatientInsurance(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DataTable dtTemp = ds.PatientInsurance.GetChanges();
            Int32 Documentid = 0;
            try
            {
                //ds = new DALPatientInsurance().InsertPatientInsurance(ds);
                dbManager.BeginTransaction();
                ds = new DALPatientInsurance().InsertPatientInsurance(ds, dbManager);

                if (Convert.ToBoolean(ds.PatientInsurance.Rows[0][ds.PatientInsurance.IsFileStreamColumn]))
                {
                    //byte[] byteArr = ds.PatientInsurance.Rows[0][ds.PatientInsurance.FileStreamColumn.ColumnName] as byte[];

                    DSPatient.PatientDocumentRow drPatDoc = ds.PatientDocument.NewPatientDocumentRow();
                    drPatDoc.PatientId = MDVUtility.ToInt64(ds.PatientInsurance.Rows[0][ds.PatientInsurance.PatientIdColumn]);
                    drPatDoc.CreatedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                    drPatDoc.ModifiedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                    drPatDoc.InsuranceId = Convert.ToInt64(ds.PatientInsurance.Rows[0][ds.PatientInsurance.InsuranceIdColumn]);
                    drPatDoc.ModifiedOn = DateTime.Now;
                    drPatDoc.CreatedOn = DateTime.Now;
                    string dt = DateTime.Now.ToShortDateString();
                    string tm = DateTime.Now.ToShortTimeString();
                    string fName = dt + " " + tm.Replace(" ", "");
                    fName = fName.Replace("/", "-").Replace(":", "");

                    var mnth = fName.Split('-')[0];
                    var day = fName.Split('-')[1];
                    var year = fName.Split('-')[2];
                    mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                    day = day.Length == 1 ? "0" + day : day;
                    var mnt = year.Split(' ')[1];
                    year = year.Split(' ')[0];
                    mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                    //fName = mnth + "-" + day + "-" + year + " " + mnt + ".jpg";
                    fName = mnth + "." + day + "." + year + ".jpg";

                    drPatDoc.FilePath = fName;
                    drPatDoc.FileType = "image/jpg";
                    drPatDoc.BUpdateStream = true;
                    drPatDoc.IsActive = true;
                    //drPatDoc.FileStream = byteArr;
                    drPatDoc.Url = MDVUtility.ToStr(ds.PatientInsurance.Rows[0][ds.PatientInsurance.UrlColumn]);
                    ds.PatientDocument.AddPatientDocumentRow(drPatDoc);
                    ds = new DALPatientDocument().UpdatePatientInsuranceDocument(ds, 1, dbManager);
                    //Documentid = GetDocumentId(ds);

                    //byte[] byteArr = ds.PatientInsurance.Rows[0][ds.PatientInsurance.FileStreamColumn.ColumnName] as byte[];
                    //DSPatient.PatientDocumentRow drPatDoc = ds.PatientDocument.NewPatientDocumentRow();
                    //drPatDoc.Documentid = Documentid;
                    //drPatDoc.PatientId = MDVUtility.ToInt64(ds.PatientInsurance.Rows[0][ds.PatientInsurance.PatientIdColumn]);
                    //drPatDoc.CreatedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper()); //MDVUtility.ToStr(ds.PatientInsurance.Rows[0][ds.PatientInsurance.CreatedByColumn]);
                    //drPatDoc.ModifiedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper()); //MDVUtility.ToStr(ds.PatientInsurance.Rows[0][ds.PatientInsurance.ModifiedByColumn]);
                    //drPatDoc.InsuranceId = Convert.ToInt64(ds.PatientInsurance.Rows[0][ds.PatientInsurance.InsuranceIdColumn]);
                    //drPatDoc.ModifiedOn = DateTime.Now;
                    //drPatDoc.CreatedOn = DateTime.Now;
                    //drPatDoc.FileType = "image/jpg";
                    //drPatDoc.BUpdateStream = true;
                    //drPatDoc.IsActive = true;
                    //drPatDoc.FileStream = byteArr;

                    //ds.PatientDocument.AddPatientDocumentRow(drPatDoc);
                    //ds = new DALPatientDocument().InsertPatientDocument(ds);
                }
                dbManager.CommitTransaction();


            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientInsurance", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }

            DALPatientInsurance _objInsurance = new DALPatientInsurance();
            _objInsurance.PatientInsuranceDBAudit(ds, dtTemp, "INSERT");

            return new BLObject<DSPatient>(ds);

        }

        /// <summary>
        /// Updates the patient insurance.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> UpdatePatientInsurance(DSPatient ds, bool InsuranceCardChanged)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DataTable dtTemp = ds.PatientInsurance.GetChanges();
            //Int32 Documentid = 0;
            try
            {
                dbManager.BeginTransaction();

                ds = new DALPatientInsurance().UpdatePatientInsurance(ds, dbManager);
                if (Convert.ToBoolean(ds.PatientInsurance.Rows[0][ds.PatientInsurance.IsFileStreamColumn]))
                {
                    //byte[] byteArr = ds.PatientInsurance.Rows[0][ds.PatientInsurance.FileStreamColumn.ColumnName] as byte[];
                    if (InsuranceCardChanged)
                    {
                        if (ds.Tables[ds.PatientDocument.TableName].Rows.Count > 0)
                        {
                            foreach (DSPatient.PatientDocumentRow dr in ds.Tables[ds.PatientDocument.TableName].Rows)
                            {
                                dr.PatientId = MDVUtility.ToInt64(ds.PatientInsurance.Rows[0][ds.PatientInsurance.PatientIdColumn]);
                                dr.CreatedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                                dr.ModifiedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                                dr.InsuranceId = Convert.ToInt64(ds.PatientInsurance.Rows[0][ds.PatientInsurance.InsuranceIdColumn]);
                                dr.ModifiedOn = DateTime.Now;
                                dr.CreatedOn = DateTime.Now;
                                string dt = DateTime.Now.ToShortDateString();
                                string tm = DateTime.Now.ToShortTimeString();
                                string fName = dt + " " + tm.Replace(" ", "");
                                fName = fName.Replace("/", "-").Replace(":", "");

                                var mnth = fName.Split('-')[0];
                                var day = fName.Split('-')[1];
                                var year = fName.Split('-')[2];
                                mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                                day = day.Length == 1 ? "0" + day : day;
                                var mnt = year.Split(' ')[1];
                                year = year.Split(' ')[0];
                                mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                                //fName = mnth + "-" + day + "-" + year + " " + mnt + ".jpg";
                                fName = mnth + "." + day + "." + year + ".jpg";

                                dr.FilePath = fName;
                                dr.FileType = "image/jpg";
                                dr.BUpdateStream = true;
                                dr.IsActive = true;
                                dr.Url = MDVUtility.ToStr(ds.PatientInsurance.Rows[0][ds.PatientInsurance.UrlColumn]);
                                //dr.FileStream = byteArr;
                            }
                            ds = new DALPatientDocument().UpdatePatientInsuranceDocument(ds, 2, dbManager);
                        }
                        else
                        {
                            DSPatient.PatientDocumentRow drPatDoc = ds.PatientDocument.NewPatientDocumentRow();
                            drPatDoc.PatientId = MDVUtility.ToInt64(ds.PatientInsurance.Rows[0][ds.PatientInsurance.PatientIdColumn]);
                            drPatDoc.CreatedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                            drPatDoc.ModifiedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                            drPatDoc.InsuranceId = Convert.ToInt64(ds.PatientInsurance.Rows[0][ds.PatientInsurance.InsuranceIdColumn]);
                            drPatDoc.ModifiedOn = DateTime.Now;
                            drPatDoc.CreatedOn = DateTime.Now;
                            string dt = DateTime.Now.ToShortDateString();
                            string tm = DateTime.Now.ToShortTimeString();
                            string fName = dt + " " + tm.Replace(" ", "");
                            fName = fName.Replace("/", "-").Replace(":", "");

                            var mnth = fName.Split('-')[0];
                            var day = fName.Split('-')[1];
                            var year = fName.Split('-')[2];
                            mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                            day = day.Length == 1 ? "0" + day : day;
                            var mnt = year.Split(' ')[1];
                            year = year.Split(' ')[0];
                            mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                            //fName = mnth + "-" + day + "-" + year + " " + mnt + ".jpg";
                            fName = mnth + "." + day + "." + year + ".jpg";
                            drPatDoc.FilePath = fName;
                            drPatDoc.FileType = "image/jpg";
                            drPatDoc.BUpdateStream = true;
                            drPatDoc.IsActive = true;
                            //drPatDoc.FileStream = byteArr;
                            drPatDoc.Url = MDVUtility.ToStr(ds.PatientInsurance.Rows[0][ds.PatientInsurance.UrlColumn]);
                            ds.PatientDocument.AddPatientDocumentRow(drPatDoc);
                            ds = new DALPatientDocument().UpdatePatientInsuranceDocument(ds, 1, dbManager);
                        }
                    }
                }
                dbManager.CommitTransaction();

            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatient", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
            DALPatientInsurance _objInsurance = new DALPatientInsurance();
            _objInsurance.PatientInsuranceDBAudit(ds, dtTemp, "UPDATE");

            return new BLObject<DSPatient>(ds);
        }

        public Int32 GetDocumentId(DSPatient ds, string FolderName = "")
        {
            Int32 Documentid = 0;
            if (FolderName == "")
            {
                FolderName = "Insurance";
            }
            if (FolderName == "Notes Diagnostic Imaging Report")
            {
                FolderName = "Diagnostic Imaging Report";
            }
            DSDocument dsDoc = new DALDocument().LoadDocument(0, FolderName, 0, "", "", "");
            DataTable dtDocType = dsDoc.Documents;
            if (dtDocType != null && dtDocType.Rows.Count > 0)
            {
                Documentid = MDVUtility.ToInt32(dtDocType.Rows[0]["DocId"]);
            }
            else
            {
                dsDoc = new DSDocument();
                DSDocument.DocumentsRow drDoc = dsDoc.Documents.NewDocumentsRow();
                drDoc.ShortName = FolderName;
                drDoc.Description = FolderName;
                drDoc.DocTypeId = GetDocumentTypeId(ds, FolderName);
                //Start 28-04-2016 Humaira Yousaf for Lab Results
                if (FolderName == "Insurance")
                {
                    drDoc.CreatedBy = MDVUtility.ToStr(ds.PatientInsurance.Rows[0][ds.PatientInsurance.CreatedByColumn]);
                    drDoc.ModifiedBy = MDVUtility.ToStr(ds.PatientInsurance.Rows[0][ds.PatientInsurance.ModifiedByColumn]);
                }
                else
                {
                    drDoc.CreatedBy = MDVUtility.ToStr(ds.PatientDocument.Rows[0][ds.PatientDocument.CreatedByColumn]);
                    drDoc.ModifiedBy = MDVUtility.ToStr(ds.PatientDocument.Rows[0][ds.PatientDocument.ModifiedByColumn]);
                }
                //End 28-04-2016 Humaira Yousaf for Lab Results
                drDoc.ModifiedOn = DateTime.Now;
                drDoc.CreatedOn = DateTime.Now;
                drDoc.IsActive = true;
                //  drDoc.EntityId = 100;
                dsDoc.Documents.AddDocumentsRow(drDoc);
                dsDoc = new DALDocument().InsertDocument(dsDoc);
                Documentid = MDVUtility.ToInt32(dsDoc.Documents.Rows[0][dsDoc.Documents.DocIdColumn]);
            }
            return Documentid;
        }
        public Int32 GetDocumentTypeId(DSPatient ds, string TypeName = "")
        {
            Int32 DocTypeId = 0;
            if (TypeName == "")
            {
                TypeName = "Insurance";
            }
            DSDocument dsDoc = new DALDocument().LoadDocumentType(0, TypeName, "", "", "");
            DataTable dtDocType = dsDoc.DocumentType;
            if (dtDocType != null && dtDocType.Rows.Count > 0)
            {
                DocTypeId = MDVUtility.ToInt32(dtDocType.Rows[0]["DoctypeId"]);
            }
            else
            {
                dsDoc = new DSDocument();
                DSDocument.DocumentTypeRow drDoc = dsDoc.DocumentType.NewDocumentTypeRow();
                drDoc.ShortName = TypeName;
                drDoc.Description = TypeName;
                //Start 28-04-2016 Humaira Yousaf for Lab Results
                if (TypeName == "Insurance")
                {
                    drDoc.CreatedBy = MDVUtility.ToStr(ds.PatientInsurance.Rows[0][ds.PatientInsurance.CreatedByColumn]);
                    drDoc.ModifiedBy = MDVUtility.ToStr(ds.PatientInsurance.Rows[0][ds.PatientInsurance.ModifiedByColumn]);
                }
                else
                {
                    drDoc.CreatedBy = MDVUtility.ToStr(ds.PatientDocument.Rows[0][ds.PatientDocument.CreatedByColumn]);
                    drDoc.ModifiedBy = MDVUtility.ToStr(ds.PatientDocument.Rows[0][ds.PatientDocument.ModifiedByColumn]);
                }
                //End 28-04-2016 Humaira Yousaf for Lab Results
                drDoc.ModifiedOn = DateTime.Now;
                drDoc.CreatedOn = DateTime.Now;
                drDoc.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                drDoc.IsActive = true;
                dsDoc.DocumentType.AddDocumentTypeRow(drDoc);
                dsDoc = new DALDocument().InsertDocumentType(dsDoc);
                DocTypeId = MDVUtility.ToInt32(dsDoc.DocumentType.Rows[0][dsDoc.DocumentType.DoctypeIdColumn]);
            }
            return DocTypeId;
        }
        /// <summary>
        /// Deletes the patient insurance.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeletePatientInsurance(string PatientId)
        {
            try
            {
                PatientId = new DALPatientInsurance().DeletePatientInsurance(PatientId);
                return new BLObject<string>(PatientId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientInsurance", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient insurances priorities.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="InsuranceIDs">The insurance i ds.</param>
        /// <returns></returns>
        public string UpdatePatientInsurancesPriorities(Int64 PatientId, string InsuranceIDs)
        {
            try
            {
                string result = new DALPatientInsurance().UpdatePatientInsurancesPriorities(PatientId, InsuranceIDs);
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientInsurancesPriorities", ex);
                return ex.Message;
            }
        }
        public string UpdatePatientVisitInsurance(Int64 PatientId, Int64 VisitId)
        {
            try
            {
                string result = new DALPatientInsurance().UpdatePatientVisitInsurance(PatientId, VisitId);
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientVisitInsurance", ex);
                return ex.Message;
            }
        }
        #endregion

        public int CheckPatientVisitInsurance(Int64 PatientId, Int64 VisitId)
        {
            try
            {
                int result = new DALPatientInsurance().CheckPatientVisitInsurance(PatientId, VisitId);
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient:CheckPatientVisitInsurance", ex);
                return 0;
            }
        }

        #region "Patient Referral"


        /// <summary>
        /// Loads the patient referral.
        /// </summary>
        /// <param name="PatientInsuranceId">The patient insurance identifier.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> LoadPatientReferral(long PatientInsuranceId, long PatientReferralID, string ReferralType = "", long FacilityToId = 0, long ReferringToId = 0, DateTime? ReferringDate = null, string IsActive = "")
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientReferrals().LoadPatientReferrals(PatientInsuranceId, PatientReferralID, ReferralType, FacilityToId, ReferringToId, ReferringDate, IsActive);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientReferral", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> searchPatientReferrals(long PatientInsuranceId,string IsFromAppointment, int pageNumber = 1, int rowsPerPage = 1000)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientReferrals().searchPatientReferrals(PatientInsuranceId, IsFromAppointment, pageNumber, rowsPerPage);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::searchPatientReferrals", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }



        /// <summary>
        /// Inserts the patient referral.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> InsertPatientReferral(DSPatient ds)
        {
            try
            {
                ds = new DALPatientReferrals().InsertPatientReferrals(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientReferral", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }



        /// <summary>
        /// Updates the patient referral.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> UpdatePatientReferral(DSPatient ds)
        {
            try
            {
                ds = new DALPatientReferrals().UpdatePatientReferrals(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientReferral", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }



        /// <summary>
        /// Deletes the patient referral.
        /// </summary>
        /// <param name="PatientReferralId">The patient referral identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeletePatientReferral(string PatientReferralId)
        {
            try
            {
                PatientReferralId = new DALPatientReferrals().DeletePatientReferrals(PatientReferralId);
                return new BLObject<string>(PatientReferralId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientReferral", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region EmergencyContact

        /// <summary>
        /// Loads the patient emergency contact.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="PatientEmergencyContactID">The patient emergency contact identifier.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> LoadPatientEmergencyContact(long PatientId, long PatientEmergencyContactID)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientEmergencyContact().LoadPatientEmergencyContact(PatientId, PatientEmergencyContactID);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> loadPatientEmergencyContacts(long PatientId, int pageNumber = 1, int rowsPerPage = 1000)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientEmergencyContact().loadPatientEmergencyContacts(PatientId, pageNumber, rowsPerPage);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientEmergencyContact", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        public List<LoadMuiltPatientAddress> SearchPatienAdressMultiFilter(string NetWorkIP, string Ext, string Zip, string FirstName, string LastName, string DOB, string Gender, string AccountNo, string EntityId, string MobileNumber)
        {
            List<LoadMuiltPatientAddress> obj =new List<LoadMuiltPatientAddress>();
            try
            {

                 obj = new DALPatientEmergencyContact().SearchPatienAdressMultiFilter(NetWorkIP, Ext, Zip, FirstName, LastName, DOB, Gender, AccountNo, EntityId, MobileNumber);
                return  obj;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::SearchPatienAdressMultiFilter", ex);
                
                throw ex;
            }
        }
        public List<LoadMuiltPatientAddress> LoadPatientContact(string NetWorkIP, string PatientId)
        {
            List<LoadMuiltPatientAddress> obj = new List<LoadMuiltPatientAddress>();
            try
            {

                obj = new DALPatientEmergencyContact().LoadPatientContact(NetWorkIP, PatientId);
                return obj;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::SearchPatienAdressMultiFilter", ex);

                throw ex;
            }
        }

        public bool LoadPatientVisitType(string PatientId)
        {
            try
            {
                bool VisitExists = false;
                string obj = new DALPatientEmergencyContact().LoadPatientVisitType(PatientId);
                if(obj == "1")
                {
                     VisitExists = true;
                } else
                {
                    VisitExists = false;
                }

                return VisitExists;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientVisitType", ex);

                throw ex;
            }
        }
        public List<LoadMuiltPatientAddress> SavePatientSignature(LoadMuiltPatientAddress model)
        {
            List<LoadMuiltPatientAddress> obj = new List<LoadMuiltPatientAddress>();
            try
            {

                obj = new DALPatientEmergencyContact().SavePatientSignature(model);
                return obj;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::SearchPatienAdressMultiFilter", ex);

                throw ex;
            }
        }
        public List<LoadMuiltPatientAddress> LoadPatientRelations()
        {
            List<LoadMuiltPatientAddress> obj = new List<LoadMuiltPatientAddress>();
            try
            {

                obj = new DALPatientEmergencyContact().LoadPatientRelations();
                return obj;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::SearchPatienAdressMultiFilter", ex);

                throw ex;
            }
        }
        /// <summary>
        /// Inserts the patient emergency contact.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> InsertPatientEmergencyContact(DSPatient ds)
        {
            try
            {
                ds = new DALPatientEmergencyContact().InsertPatientEmergencyContact(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientEmergencyContact", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient emergency contact.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> UpdatePatientEmergencyContact(DSPatient ds)
        {
            try
            {
                ds = new DALPatientEmergencyContact().UpdatePatientEmergencyContact(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientEmergencyContact", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the patient emergency contact.
        /// </summary>
        /// <param name="PatientEmergencyContactId">The patient emergency contact identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeletePatientEmergencyContact(string PatientEmergencyContactId)
        {
            try
            {
                PatientEmergencyContactId = new DALPatientEmergencyContact().DeletePatientEmergencyContact(PatientEmergencyContactId);
                return new BLObject<string>(PatientEmergencyContactId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientEmergencyContact", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region Family

        /// <summary>
        /// Loads the patient family.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="PatientFamilyID">The patient family identifier.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> LoadPatientFamily(long PatientId, long PatientFamilyID)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientFamily().LoadPatientFamily(PatientId, PatientFamilyID);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientFamily", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> searchPatientFamily(long PatientId, long PatientRepresentativeId = 0, int pageNumber = 1, int rowsPerPage = 1000, string FirstName = "", string LastName = "", string AccountNo = "", string PhoneNo = "")
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientFamily().searchPatientFamily(PatientId, PatientRepresentativeId, pageNumber, rowsPerPage, FirstName, LastName, AccountNo, PhoneNo);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::searchPatientFamily", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public string patientReffralALert(long patientInsuranceId, long PatientId)
        {
            var isAlertDisplay = "";
            try
            {
                DSPatient ds = new DSPatient();
                isAlertDisplay = (string)new DALPatientInsurance().patientReffralALert(patientInsuranceId, PatientId);
                return isAlertDisplay;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::patientReffralALert", ex);
                return isAlertDisplay;
            }
        }

        /// <summary>
        /// Inserts the patient family.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> InsertPatientFamily(DSPatient ds)
        {
            try
            {
                ds = new DALPatientFamily().InsertPatientFamily(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientFamily", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient family.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> UpdatePatientFamily(DSPatient ds)
        {
            try
            {
                ds = new DALPatientFamily().UpdatePatientFamily(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientFamily", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the patient family.
        /// </summary>
        /// <param name="PatientFamilyId">The patient family identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeletePatientFamily(string PatientFamilyId)
        {
            try
            {
                PatientFamilyId = new DALPatientFamily().DeletePatientFamily(PatientFamilyId);
                return new BLObject<string>(PatientFamilyId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientFamily", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> LookupPatientFamily(string PatientId, string AccountNo, string FirstName, string LastName)
        {
            DSPatient ds = new DSPatient();
            try
            {
                ds = new DALPatientFamily().LookupPatientFamily(PatientId, AccountNo, FirstName, LastName);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPatientFamily", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        #endregion

        #region Authorization

        /// <summary>
        /// Loads the patient Authorization.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="PatientAuthorizationID">The patient Authorization identifier.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> LoadPatientAuthorization(long PatientId, long PatientAuthorizationID, long patientInsuranceId = 0, string CPTCode = null, DateTime? DOSFrom = null, DateTime? DOSTo = null)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientAuthorization().LoadPatientAuthorization(PatientId, PatientAuthorizationID, patientInsuranceId, CPTCode, DOSFrom, DOSTo);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientAuthorization", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the patient Authorization.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> InsertPatientAuthorization(DSPatient ds)
        {
            try
            {
                ds = new DALPatientAuthorization().InsertPatientAuthorization(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientAuthorization", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient Authorization.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> UpdatePatientAuthorization(DSPatient ds)
        {
            try
            {
                ds = new DALPatientAuthorization().UpdatePatientAuthorization(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientAuthorization", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the patient Authorization.
        /// </summary>
        /// <param name="PatientAuthorizationId">The patient Authorization identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeletePatientAuthorization(string PatientAuthorizationId)
        {
            try
            {
                PatientAuthorizationId = new DALPatientAuthorization().DeletePatientAuthorization(PatientAuthorizationId);
                return new BLObject<string>(PatientAuthorizationId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientAuthorization", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region Activity Log

        /// <summary>
        /// Loads the patient Activity Logs.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="PatientAuthorizationID">The patient Activity Log identifier.</param>
        /// <returns>BLObject&lt;DataSet&gt;.</returns>
        public BLObject<DataSet> LoadPatientActivityLogs(string ModuleName, string UserName, string ColumnKeyId, string ProfileName, string DBAuditId, string PatientId, string VisitId, DateTime? createdDate, string ActivityType, string DBTableName = null, string ParentKeyColumnId = "")
        {
            try
            {
                string insuranceId = "";
                int totalInsurances = 0;
                string PatientReferralIds = "";
                string ReferalProfileName = string.Empty;
                DSDBAudit ds = new DSDBAudit();
                if (ProfileName.Contains("["))
                {
                    ProfileName = ProfileName.Split('[')[0].Trim();

                    if (ProfileName == "Patient Referrals")
                    {
                        ProfileName = "Referral Management";
                    }
                }
                //PMS-640
                if (ProfileName.Contains("PatientCharges"))
                {
                    //in case of patient charges profile name we are removing extra string i.e cpt code and charge number
                    ProfileName = "PatientCharges";
                }

                if (ProfileName.Contains("Patient Referrals"))
                {
                    //in case of Patient Referrals  
                    ProfileName = "Referral Management";
                }
                ds = new DBActivityAudit().LoadDBAudit(ModuleName, UserName, ColumnKeyId, ProfileName, DBAuditId, PatientId, VisitId, createdDate, DBTableName, ParentKeyColumnId);

                //DSPatient ds = new DALActivityLog().LoadActivityLog(PatientId,PatientActivityLogId);
                DataSet dsResults = new DataSet();
                // This Column Key id is used to filter the first Record User Changes Result set of data table
                long ColumnPkId = -1;
                // ActivityType Can Be NewEntryUser for first load of activity Load view
                if (ActivityType.IndexOf("NewEntry") > -1)
                {
                    //New Entry DataTable
                    DataTable dtNewEntry = new DataTable("NewEntry");
                    dtNewEntry.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtNewEntry.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtNewEntry.Columns.Add(new DataColumn("User", typeof(string)));
                    dtNewEntry.Columns.Add(new DataColumn("ColumnKeyId", typeof(string)));
                    dtNewEntry.Columns.Add(new DataColumn("EntryDate", typeof(DateTime)));
                    //dtNewEntry.Columns.Add(new DataColumn("Field", typeof(string)));
                    //dtNewEntry.Columns.Add(new DataColumn("OriginalValue", typeof(string)));
                    //dtNewEntry.Columns.Add(new DataColumn("CurrentValue", typeof(string)));
                    //CDSPatientStatus added as for history of status modification. it is added here as CDSPatientStatus has no audit for insertion.
                    DataRow[] insertedRows = ds.Tables[ds.DBAudit.TableName].Select("DBAuditAction='Insert' or DBTableName='CDSPatientStatus' or (DBTableName='PatientAppointments' and DbAuditAction='Delete')");
                    foreach (var currentRow in insertedRows)
                    {
                        String CurrentValue = currentRow[ds.DBAudit.CurrentValueColumn.ColumnName].ToString();
                        String OriginalValue = currentRow[ds.DBAudit.OriginalValueColumn.ColumnName].ToString();
                        //Added single qoutes by Azeem Raza Tayyab on 19-May-2016 to Fix Bug#:PMS-5213
                        DataRow[] drExistingRows = dtNewEntry.Select("ProfileName='" + currentRow[ds.DBAudit.ProfileNameColumn.ColumnName].ToString() + "' AND ColumnKeyId=" + MDVUtility.ToLINQFormatString(currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]));
                        if (drExistingRows.Length < 1)
                        {

                            if (currentRow[ds.DBAudit.ColumnKeyNameColumn.ColumnName] != null && currentRow[ds.DBAudit.ColumnKeyNameColumn.ColumnName].ToString() == "InsuranceId")
                            {
                                insuranceId += currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName] + ",";
                            }
                            totalInsurances++;

                            // PRD-304 Get All Patient Referral Ids
                            if (currentRow[ds.DBAudit.ColumnKeyNameColumn.ColumnName] != null && currentRow[ds.DBAudit.ColumnKeyNameColumn.ColumnName].ToString() == "PatientReferralId"
                                && currentRow[ds.DBAudit.ProfileNameColumn.ColumnName].ToString() == "Referral Management"
                                )
                            {

                                PatientReferralIds += currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName] + ",";
                                ReferalProfileName = "Referral Management";
                            }
                            string profileName = currentRow[ds.DBAudit.ProfileNameColumn.ColumnName].ToString();
                            //bind cpt code and charge number with line item
                            if (profileName == "PatientCharges")
                            {
                                //DALCPTCode DALCPTCodeObj = new DALCPTCode();
                                DSCodes dsCPTCode = new DSCodes();
                                dsCPTCode = new DALCPTCode().LoadCPTCodeByChargeId(Convert.ToInt64(currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]));
                                if (dsCPTCode != null)
                                {
                                    profileName = profileName + " - " + dsCPTCode.Tables[dsCPTCode.ChargeCPT.TableName].Rows[0]["CPTCode"] + " - Charge Number: " + currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName];
                                }
                            }
                            dtNewEntry.Rows.Add(1, profileName, currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]));
                        }
                        //dtNewEntry.Rows.Add(1, currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]), currentRow[ds.DBAudit.DisplayNameColumn.ColumnName], OriginalValue, CurrentValue);
                    }

                    //PRD-304 Referral changes in Audit Log
                    #region Patient Refrrals profile name Setting
                    if (!string.IsNullOrEmpty(ReferalProfileName))
                    {
                        DSPatient DsPatientRefrral = new DSPatient();
                        string InsuracneName = "";
                        string RefrrelNumber = "";
                        string[] referalPrimaryKeys = PatientReferralIds.Split(',');
                        DataRow[] updateProfileRows = dtNewEntry.Select("ProfileName = '" + "Referral Management'");

                        foreach (DataRow item in updateProfileRows)
                        {
                            for (int i = 0; i < referalPrimaryKeys.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(referalPrimaryKeys[i]) && referalPrimaryKeys[i].ToString() == item["ColumnKeyId"].ToString())
                                {
                                    // get patient refrral row.
                                    var PatientReffralRow = insertedRows.Where(x => x.Field<string>("ColumnName") == "ReferralAuthNo"
                                    && x.Field<string>("ColumnKeyId") == referalPrimaryKeys[i].ToString()
                                    ).FirstOrDefault();

                                    // get  patient Insurance row against Patient Refrral. 
                                    var patientInsuranceRow = insertedRows.Where(x => x.Field<string>("ColumnName") == "PatientInsuranceId"
                                     && x.Field<string>("ColumnKeyId") == referalPrimaryKeys[i].ToString() && x.Field<string>("DBTableName") == "PatientReferrals"
                                  ).FirstOrDefault();

                                    // Set Profile name for Patient Refrral. 
                                    {

                                        RefrrelNumber = PatientReffralRow[ds.DBAudit.CurrentValueColumn.ColumnName].ToString();
                                        InsuracneName = patientInsuranceRow[ds.DBAudit.CurrentValueColumn].ToString();
                                        item["ProfileName"] = "Patient Referrals [" + RefrrelNumber + "]" + "[" + InsuracneName + "]";
                                        break;
                                    }
                                }

                            }
                        }


                    }
                    #endregion

                    //get Insurance Name
                    DSPatient DsPatientInsurance = new DSPatient();
                    if (insuranceId != "")
                    {
                        insuranceId = insuranceId.Remove(insuranceId.Length - 1);
                        DsPatientInsurance = new DALPatientInsurance().GetInsuranceName(insuranceId);
                        for (int i = 0; i < dtNewEntry.Rows.Count; i++)
                        {
                            var insuranceRow = DsPatientInsurance.Tables[DsPatientInsurance.InsurancePlan.TableName].Select("InsuranceId='" + dtNewEntry.Rows[i]["ColumnKeyId"] + "'");
                            if (insuranceRow.Length > 0)
                            {
                                //add insurance name with profile name in case of insurance row
                                dtNewEntry.Rows[i]["ProfileName"] = dtNewEntry.Rows[i]["ProfileName"] + " [" + (insuranceRow[0]).ItemArray[1] + "]";
                            }
                        }
                    }

                    // dsResults.Tables.Add(dtNewEntry);
                    dsResults.Tables.Add(dtNewEntry.DefaultView.ToTable(true, "ActivityLogId", "ProfileName", "User", "ColumnKeyId", "EntryDate"));
                    //dsResults.Tables.Add(dtNewEntry.DefaultView.ToTable(true, "ActivityLogId", "ProfileName", "User", "ColumnKeyId", "EntryDate", "Field", "OriginalValue", "CurrentValue"));
                    if (ActivityType.IndexOf("User") > -1 && dtNewEntry.Rows.Count > 0)
                    {
                        ColumnPkId = MDVUtility.ToLong(dtNewEntry.Rows[0]["ColumnKeyId"]);
                    }
                }
                // ActivityType Can Be NewEntryUser for first load of activity Load view
                if (ActivityType.IndexOf("User") > -1)
                {
                    //User Data Table
                    DataTable dtUser = new DataTable("User");
                    dtUser.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtUser.Columns.Add(new DataColumn("Actions", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("User", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                    //Start 25-04-2016 Humaira Yousaf
                    dtUser.Columns.Add(new DataColumn("ColumnKeyId", typeof(string)));

                    //pms-1179
                    DataRow[] modifiedRows = ds.Tables[ds.DBAudit.TableName].Select("(DBAuditAction='Insert' or DBAuditAction='Print HCFA Form' or DBAuditAction='update' or DBAuditAction='delete' or DBAuditAction='View' or DBAuditAction='Print') and ColumnName<>'NoteComments'");
                    foreach (var currentRow in modifiedRows)
                    {
                        //  Column Key id is used to filter the first Record User Changes Result set of data table
                        if (ActivityType.IndexOf("NewEntry") > -1 && ColumnPkId > 0)
                        {
                            if (ColumnPkId == MDVUtility.ToLong(currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]))
                            {
                                dtUser.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DBAuditActionColumn.ColumnName], currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).ToString("G"), currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]);
                            }
                        }
                        else
                        {
                            // PRD-304
                            if (currentRow[ds.DBAudit.ProfileNameColumn.ColumnName].ToString() == "Referral Management"){
                                currentRow[ds.DBAudit.ProfileNameColumn.ColumnName] = "Patient Referrals";
                            }
                            dtUser.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DBAuditActionColumn.ColumnName], currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).ToString("G"), currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]);
                        }

                    }
                    if (ActivityType.IndexOf("User") > -1 && dtUser.Rows.Count > 0)
                    {
                        ColumnPkId = MDVUtility.ToLong(dtUser.Rows[0]["ColumnKeyId"]);
                    }
                    dsResults.Tables.Add(dtUser.DefaultView.ToTable(true, "Actions", "ProfileName", "User", "Date", "ColumnKeyId"));
                    //End 25-04-2016 Humaira Yousaf
                }
                if (ActivityType.Equals("Changes"))
                {
                    //User Data Table
                    DataTable dtField = new DataTable("Field");
                    dtField.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtField.Columns.Add(new DataColumn("Field", typeof(string)));
                    dtField.Columns.Add(new DataColumn("OriginalValue", typeof(string)));
                    dtField.Columns.Add(new DataColumn("CurrentValue", typeof(string)));
                    dtField.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtField.Columns.Add(new DataColumn("ColumnKeyId", typeof(string)));
                    dtField.Columns.Add(new DataColumn("Date", typeof(DateTime)));

                    //pms-1179
                    DataRow[] FieldRows = ds.Tables[ds.DBAudit.TableName].Select("(DBAuditAction='Insert' or DBAuditAction='delete' or DBAuditAction='Update') AND (DBTableName<>'PatientCharges' OR ColumnName<>'StatusId')AND ( DBTableName<>'VisitId' AND ColumnName<>'SubmittedBy')AND ( DBTableName<>'VisitId' AND ColumnName<>'SubmittedDate')AND ( DBTableName<>'VisitId' AND ColumnName<>'ClaimStatusID')AND ( DBTableName<>'VisitId' AND ColumnName<>'VisitStatusID') AND ( DBTableName<>'VisitId' AND ColumnName<>'NoteComments')");
                    foreach (var currentRow in FieldRows)
                    {
                        String CurrentValue = currentRow[ds.DBAudit.CurrentValueColumn.ColumnName].ToString();
                        String OriginalValue = currentRow[ds.DBAudit.OriginalValueColumn.ColumnName].ToString();

                        if (currentRow[ds.DBAudit.ProfileNameColumn.ColumnName].ToString() == "Referral Management" && currentRow[ds.DBAudit.DisplayNameColumn.ColumnName].ToString() == "Active")
                        {
                            currentRow[ds.DBAudit.DisplayNameColumn.ColumnName] = "Referral Status";
                        }

                        if (createdDate != null)
                        {
                            DateTime CreatedDate = currentRow.IsNull("CreatedDate") ? new DateTime() : MDVUtility.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]);
                            if (currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOSFrom") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOSTo")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOD") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOB")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("UAWorkto") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("UAWorkFrom")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("LMPDate") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DischargeDate")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("AdmissionDate") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("IllnessDate")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("InjuryDate")
                                )
                            {
                                CurrentValue = string.IsNullOrEmpty(CurrentValue) ? "" : MDVUtility.GetDateMMDDYYY(Convert.ToDateTime(CurrentValue).ToString("G"));
                                OriginalValue = string.IsNullOrEmpty(OriginalValue) ? "" : MDVUtility.GetDateMMDDYYY(Convert.ToDateTime(OriginalValue).ToString("G"));
                            }
                            if (!(OriginalValue == "" && CurrentValue == "") && CreatedDate != DateTime.MinValue && createdDate.Equals(CreatedDate))
                            {
                                if (currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("PatientImageThumbnail") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("PatientProfileThumbnailPath"))
                                {
                                    string originalFileStream = "";
                                    string currentFileStream = "";
                                    string ServerPathForLoadFile = System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesPath"];

                                    if (OriginalValue != "")
                                    {
                                        string imageBase64 = string.Empty;
                                        string base64String = string.Empty;

                                        if (System.IO.Directory.Exists(ServerPathForLoadFile))
                                        {
                                            if (!OriginalValue.Equals(""))
                                            {
                                                string imgPath = System.IO.Path.Combine(ServerPathForLoadFile, OriginalValue);
                                                if (System.IO.File.Exists(imgPath))
                                                {
                                                    byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
                                                    base64String = Convert.ToBase64String(imageBytes);
                                                    originalFileStream = "data:image/png;base64," + base64String;
                                                }
                                            }
                                        }
                                    }

                                    if (CurrentValue != "")
                                    {
                                        if (System.IO.Directory.Exists(ServerPathForLoadFile))
                                        {
                                            if (!CurrentValue.Equals(""))
                                            {
                                                string imgPath = System.IO.Path.Combine(ServerPathForLoadFile, CurrentValue);
                                                if (System.IO.File.Exists(imgPath))
                                                {
                                                    byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
                                                    string base64String = Convert.ToBase64String(imageBytes);
                                                    currentFileStream = "data:image/png;base64," + base64String;

                                                }
                                            }
                                        }

                                    }
                                    dtField.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DisplayNameColumn.ColumnName],
                                    originalFileStream, currentFileStream, currentRow[ds.DBAudit.ProfileNameColumn.ColumnName]);
                                }
                                else
                                {
                                    dtField.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DisplayNameColumn.ColumnName],
                                    OriginalValue, CurrentValue, currentRow[ds.DBAudit.ProfileNameColumn.ColumnName]);
                                }
                            }
                        }
                        else
                        {
                            if (currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOSFrom") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOSTo")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOD") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOB")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("UAWorkto") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("UAWorkFrom")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("LMPDate") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DischargeDate")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("AdmissionDate") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("IllnessDate")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("InjuryDate"))
                            {
                                CurrentValue = string.IsNullOrEmpty(CurrentValue) ? "" : MDVUtility.GetDateMMDDYYY(Convert.ToDateTime(CurrentValue).ToString("G"));
                                OriginalValue = string.IsNullOrEmpty(OriginalValue) ? "" : MDVUtility.GetDateMMDDYYY(Convert.ToDateTime(OriginalValue).ToString("G"));
                            }

                            if (!(OriginalValue == "" && CurrentValue == ""))
                            {
                                dtField.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DisplayNameColumn.ColumnName],
                                OriginalValue, CurrentValue, currentRow[ds.DBAudit.ProfileNameColumn.ColumnName]);
                            }

                        }
                    }
                    dsResults.Tables.Add(dtField);
                }
                if (ActivityType.IndexOf("NewEntry") > -1)
                {
                    //Submit/Re-Submit Data Table
                    DataTable dtRequireSubmit = new DataTable("RequireSubmit");
                    dtRequireSubmit.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtRequireSubmit.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("DisplayName", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("UserName", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                    dtRequireSubmit.Columns.Add(new DataColumn("CurrentValue", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("PatientInsurance", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("PatientInsuranceId", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("IsCrossedOver", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("ColumnKeyId", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("ColumnDatatype", typeof(string)));

                    DataRow[] RequireSubmitRows = ds.Tables[ds.DBAudit.TableName].Select("(DBAuditAction='update' OR DBAuditAction='insert') AND ColumnName in('ClaimStatusId','StatusId') AND ColumnKeyName in ('ChargeCapId','VisitId') and currentValue in ('Submitted','ReSubmit')");
                    foreach (var currentRow in RequireSubmitRows)
                    {
                        dtRequireSubmit.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.DisplayNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.CreatedDateColumn.ColumnName], currentRow[ds.DBAudit.CurrentValueColumn.ColumnName], currentRow[ds.DBAudit.PatientInsuranceNameColumn.ColumnName], currentRow[ds.DBAudit.PatientInsuranceIdColumn.ColumnName], Convert.ToBoolean(currentRow[ds.DBAudit.IsCrossedOverColumn.ColumnName]) == true ? "Yes" : "No", currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName], currentRow[ds.DBAudit.SubmitTypeColumn.ColumnName]);
                    }
                    dsResults.Tables.Add(dtRequireSubmit);
                }
                if (ActivityType.Equals("DeletedCharges"))
                {
                    //Deleted Charges Table
                    DataTable dtUser = new DataTable("DeletedCharges");
                    dtUser.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtUser.Columns.Add(new DataColumn("Actions", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("User", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                    dtUser.Columns.Add(new DataColumn("ColumnKeyid", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("ColumnKeyName", typeof(string)));
                    DataRow[] modifiedRows = ds.Tables[ds.DBAudit.TableName].Select("(DBAuditAction='Delete' OR DBAuditAction='delete') AND DBTableName <> 'PatientVisitICD'");
                    foreach (var currentRow in modifiedRows)
                    {
                        dtUser.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DBAuditActionColumn.ColumnName], currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).ToString("G"), currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName], currentRow[ds.DBAudit.ColumnKeyNameColumn.ColumnName]);
                    }
                    dsResults.Tables.Add(dtUser.DefaultView.ToTable(true, "Actions", "ProfileName", "User", "Date", "ColumnKeyid", "ColumnKeyName"));
                }
                //DSPatient ds = new DSPatient();
                //ds = new DALActivityLog().LoadPatientAuthorization(PatientId, PatientActivityLogId);
                return new BLObject<DataSet>(dsResults);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientAuthorization", ex);
                return new BLObject<DataSet>(null, ex.Message);
            }
        }
        public BLObject<DataSet> LoadPatientActivityDetailLogs(string ModuleName, string UserName, string ColumnKeyId, string ProfileName, string DBAuditId, string PatientId, string VisitId, DateTime? createdDate, string ActivityType, string DBTableName = null, string ParentKeyColumnId = "")
        {
            try
            {

                DSDBAudit ds = new DSDBAudit();
                ds = new DBActivityAudit().LoadDBAuditDetail(ModuleName, UserName, ColumnKeyId, ProfileName, DBAuditId, PatientId, VisitId, createdDate, DBTableName, ParentKeyColumnId);

                //DSPatient ds = new DALActivityLog().LoadActivityLog(PatientId,PatientActivityLogId);
                DataSet dsResults = new DataSet();
                // This Column Key id is used to filter the first Record User Changes Result set of data table
                long ColumnPkId = -1;
                // ActivityType Can Be NewEntryUser for first load of activity Load view
                if (ActivityType.IndexOf("NewEntry") > -1)
                {
                    //New Entry DataTable
                    DataTable dtNewEntry = new DataTable("NewEntry");
                    dtNewEntry.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtNewEntry.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtNewEntry.Columns.Add(new DataColumn("User", typeof(string)));
                    dtNewEntry.Columns.Add(new DataColumn("ColumnKeyId", typeof(string)));
                    dtNewEntry.Columns.Add(new DataColumn("EntryDate", typeof(DateTime)));
                    //dtNewEntry.Columns.Add(new DataColumn("Field", typeof(string)));
                    //dtNewEntry.Columns.Add(new DataColumn("OriginalValue", typeof(string)));
                    //dtNewEntry.Columns.Add(new DataColumn("CurrentValue", typeof(string)));
                    DataRow[] insertedRows = ds.Tables[ds.DBAudit.TableName].Select("DBAuditAction='Insert'");
                    foreach (var currentRow in insertedRows)
                    {
                        String CurrentValue = currentRow[ds.DBAudit.CurrentValueColumn.ColumnName].ToString();
                        String OriginalValue = currentRow[ds.DBAudit.OriginalValueColumn.ColumnName].ToString();
                        //Added single qoutes by Azeem Raza Tayyab on 19-May-2016 to Fix Bug#:PMS-5213
                        DataRow[] drExistingRows = dtNewEntry.Select("ProfileName='" + currentRow[ds.DBAudit.ProfileNameColumn.ColumnName].ToString() + "' AND ColumnKeyId=" + MDVUtility.ToLINQFormatString(currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]));
                        if (drExistingRows.Length < 1)
                        {
                            dtNewEntry.Rows.Add(1, currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]));
                        }
                        //dtNewEntry.Rows.Add(1, currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]), currentRow[ds.DBAudit.DisplayNameColumn.ColumnName], OriginalValue, CurrentValue);
                    }
                    // dsResults.Tables.Add(dtNewEntry);
                    dsResults.Tables.Add(dtNewEntry.DefaultView.ToTable(true, "ActivityLogId", "ProfileName", "User", "ColumnKeyId", "EntryDate"));
                    //dsResults.Tables.Add(dtNewEntry.DefaultView.ToTable(true, "ActivityLogId", "ProfileName", "User", "ColumnKeyId", "EntryDate", "Field", "OriginalValue", "CurrentValue"));
                    if (ActivityType.IndexOf("User") > -1 && dtNewEntry.Rows.Count > 0)
                    {
                        ColumnPkId = MDVUtility.ToLong(dtNewEntry.Rows[0]["ColumnKeyId"]);
                    }
                }
                // ActivityType Can Be NewEntryUser for first load of activity Load view
                if (ActivityType.IndexOf("User") > -1)
                {
                    //User Data Table
                    DataTable dtUser = new DataTable("User");
                    dtUser.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtUser.Columns.Add(new DataColumn("Actions", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("User", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                    //Start 25-04-2016 Humaira Yousaf
                    dtUser.Columns.Add(new DataColumn("ColumnKeyId", typeof(string)));
                    DataRow[] modifiedRows = ds.Tables[ds.DBAudit.TableName].Select("DBAuditAction='Insert' or DBAuditAction='Print HCFA Form' or DBAuditAction='update' or DBAuditAction='delete' or DBAuditAction='View' or DBAuditAction='Print'");
                    foreach (var currentRow in modifiedRows)
                    {
                        //  Column Key id is used to filter the first Record User Changes Result set of data table
                        if (ActivityType.IndexOf("NewEntry") > -1 && ColumnPkId > 0)
                        {
                            if (ColumnPkId == MDVUtility.ToLong(currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]))
                            {
                                dtUser.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DBAuditActionColumn.ColumnName], currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).ToString("G"), currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]);
                            }
                        }
                        else
                        {
                            dtUser.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DBAuditActionColumn.ColumnName], currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).ToString("G"), currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]);
                        }

                    }
                    dsResults.Tables.Add(dtUser.DefaultView.ToTable(true, "Actions", "ProfileName", "User", "Date", "ColumnKeyId"));
                    //End 25-04-2016 Humaira Yousaf
                }
                if (ActivityType.Equals("Changes"))
                {
                    //User Data Table
                    DataTable dtField = new DataTable("Field");
                    dtField.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtField.Columns.Add(new DataColumn("Field", typeof(string)));
                    dtField.Columns.Add(new DataColumn("OriginalValue", typeof(string)));
                    dtField.Columns.Add(new DataColumn("CurrentValue", typeof(string)));
                    dtField.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtField.Columns.Add(new DataColumn("ColumnKeyId", typeof(string)));
                    dtField.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                    DataRow[] FieldRows = ds.Tables[ds.DBAudit.TableName].Select("(DBAuditAction='Insert' or DBAuditAction='delete' or DBAuditAction='Update') AND (DBTableName<>'PatientCharges' OR ColumnName<>'StatusId')AND ( DBTableName<>'VisitId' AND ColumnName<>'SubmittedBy')AND ( DBTableName<>'VisitId' AND ColumnName<>'SubmittedDate')AND ( DBTableName<>'VisitId' AND ColumnName<>'ClaimStatusID')AND ( DBTableName<>'VisitId' AND ColumnName<>'VisitStatusID')");
                    foreach (var currentRow in FieldRows)
                    {
                        String CurrentValue = currentRow[ds.DBAudit.CurrentValueColumn.ColumnName].ToString();
                        String OriginalValue = currentRow[ds.DBAudit.OriginalValueColumn.ColumnName].ToString();
                        if (currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOSFrom") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOSTo")
                            || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOD")
                            || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOB"))
                        {
                            CurrentValue = string.IsNullOrEmpty(CurrentValue) ? "" : MDVUtility.GetDateMMDDYYY(Convert.ToDateTime(CurrentValue).ToString("G"));
                            OriginalValue = string.IsNullOrEmpty(OriginalValue) ? "" : MDVUtility.GetDateMMDDYYY(Convert.ToDateTime(OriginalValue).ToString("G"));
                        }
                        if (!(OriginalValue == "" && CurrentValue == ""))
                        {
                            dtField.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DisplayNameColumn.ColumnName],
                            OriginalValue, CurrentValue);
                        }

                    }
                    dsResults.Tables.Add(dtField);
                }
                if (ActivityType.Equals("NewEntry"))
                {
                    //Submit/Re-Submit Data Table
                    DataTable dtRequireSubmit = new DataTable("RequireSubmit");
                    dtRequireSubmit.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtRequireSubmit.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("DisplayName", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("UserName", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                    dtRequireSubmit.Columns.Add(new DataColumn("CurrentValue", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("PatientInsurance", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("PatientInsuranceId", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("IsCrossedOver", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("ColumnKeyId", typeof(string)));

                    DataRow[] RequireSubmitRows = ds.Tables[ds.DBAudit.TableName].Select("(DBAuditAction='update' OR DBAuditAction='insert') AND ColumnName in('ClaimStatusId','StatusId') AND ColumnKeyName in ('ChargeCapId','VisitId') and currentValue in ('Submitted','ReSubmit')");
                    foreach (var currentRow in RequireSubmitRows)
                    {
                        dtRequireSubmit.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.DisplayNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.CreatedDateColumn.ColumnName], currentRow[ds.DBAudit.CurrentValueColumn.ColumnName], currentRow[ds.DBAudit.PatientInsuranceNameColumn.ColumnName], currentRow[ds.DBAudit.PatientInsuranceIdColumn.ColumnName], Convert.ToBoolean(currentRow[ds.DBAudit.IsCrossedOverColumn.ColumnName]) == true ? "Yes" : "No", currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]);
                    }
                    dsResults.Tables.Add(dtRequireSubmit);
                }
                if (ActivityType.Equals("DeletedCharges"))
                {
                    //Deleted Charges Table
                    DataTable dtUser = new DataTable("DeletedCharges");
                    dtUser.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtUser.Columns.Add(new DataColumn("Actions", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("User", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                    dtUser.Columns.Add(new DataColumn("ColumnKeyid", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("ColumnKeyName", typeof(string)));
                    DataRow[] modifiedRows = ds.Tables[ds.DBAudit.TableName].Select("DBAuditAction='delete'");
                    foreach (var currentRow in modifiedRows)
                    {
                        dtUser.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DBAuditActionColumn.ColumnName], currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).ToString("G"), currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName], currentRow[ds.DBAudit.ColumnKeyNameColumn.ColumnName]);
                    }
                    dsResults.Tables.Add(dtUser.DefaultView.ToTable(true, "Actions", "ProfileName", "User", "Date", "ColumnKeyid", "ColumnKeyName"));
                }
                //DSPatient ds = new DSPatient();
                //ds = new DALActivityLog().LoadPatientAuthorization(PatientId, PatientActivityLogId);
                return new BLObject<DataSet>(dsResults);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientAuthorization", ex);
                return new BLObject<DataSet>(null, ex.Message);
            }
        }


        /// <summary>
        /// Inserts the patient ActivityLogs.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> InsertPatientActivityLogs(DSPatient ds)
        {
            try
            {
                ds = new DALActivityLog().InsertActivityLog(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientActivityLogs", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }


        /// <summary>
        /// Updates the patient ActivityLogs.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> UpdatePatientActivityLogs(DSPatient ds)
        {
            try
            {
                ds = new DALActivityLog().UpdateActivityLog(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientActivityLogs", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the patient ActivityLogs.
        /// </summary>
        /// <param name="PatientAuthorizationId">The patient Authorization identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeletePatientActivityLogs(string ActivityLogId)
        {
            try
            {
                ActivityLogId = new DALActivityLog().DeleteActivityLog(ActivityLogId);
                return new BLObject<string>(ActivityLogId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientActivityLogs", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSSettings> LoadDashBoardPatientChanges(int Month, int Year, string ProfileName, int PageNumber = 1, int RowspPage = 15)
        {

            try
            {
                DSSettings ds = new DSSettings();
                ds = new DALDashBoard().LoadDashBoardPatientChanges(Month, Year, ProfileName, PageNumber, RowspPage);
                return new BLObject<DSSettings>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadDashBoardPatientChanges", ex);
                return new BLObject<DSSettings>(null, ex.Message);
            }

        }

        public BLObject<DSSettings> LoadDashBoardWidgetsCount(long Month, long Year, long CheckedId, DateTime AppointmentDate, long AssignedToId, string MsgtypeNot, string NoteStatus, long MsgStatusId, string MsgTypeShortName, DateTime FromDate, DateTime ToDate, long PageNumber = 1, long RowspPage = 15)
        {

            try
            {
                DSSettings ds = new DSSettings();
                ds = new DALDashBoard().LoadDashBoardWidgetsCount(Month, Year, CheckedId, AppointmentDate, AssignedToId, MsgtypeNot, NoteStatus, MsgStatusId, MsgTypeShortName, FromDate, ToDate, PageNumber, RowspPage);
                return new BLObject<DSSettings>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadDashBoardWidgetsCount", ex);
                return new BLObject<DSSettings>(null, ex.Message);
            }

        }

        #endregion

        #region Patient Document
        /// <summary>
        /// Loads the patient Document.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="PatientDocId">The patient document identifier.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> LoadPatientDocument(string PatientDocId, long PatientId, string PatientAccountNumber, string PatientLastName
                                    , string PatientFirstName, DateTime? FromDOS, DateTime? ToDOS, DateTime? FromEntryDate
                                    , DateTime? ToEntryDate, string EnteredBy, long AssignedById, long ReviewedById, string IsReviewed, int DocumentId, string IsActive, string FileStream = "0", Int64 AdvancePaymentId = 0, int PageNumber = 1, int RowspPage = 1000, Int32 AssignedToId = 0, Int64 TransitionId = 0, string RefModuleName = "", long OrderSetReferralId = 0, long OrderSetPatEducationId = 0, SharedVariable sharedVariable = null)
        {
            try
            {
                DSPatient ds = new DSPatient();
                if (sharedVariable != null)
                {
                    ds = new DALPatientDocument(sharedVariable).LoadPatientDocument(PatientDocId, PatientId, PatientAccountNumber, PatientLastName, PatientFirstName, FromDOS, ToDOS, FromEntryDate, ToEntryDate, EnteredBy, AssignedById, ReviewedById, IsReviewed, DocumentId, IsActive, FileStream, AdvancePaymentId, PageNumber, RowspPage, AssignedToId, TransitionId, RefModuleName, OrderSetReferralId, OrderSetPatEducationId, sharedVariable);
                }
                else
                {
                    ds = new DALPatientDocument().LoadPatientDocument(PatientDocId, PatientId, PatientAccountNumber, PatientLastName, PatientFirstName, FromDOS, ToDOS, FromEntryDate, ToEntryDate, EnteredBy, AssignedById, ReviewedById, IsReviewed, DocumentId, IsActive, FileStream, AdvancePaymentId, PageNumber, RowspPage, AssignedToId, TransitionId, RefModuleName, OrderSetReferralId, OrderSetPatEducationId);
                }
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientDocument", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> searchPatientDocument(string PatientDocId, long PatientId, string PatientAccountNumber, DateTime? FromDOS, DateTime? ToDOS, DateTime? FromEntryDate, DateTime? ToEntryDate, string EnteredBy, long ReviewedById, string IsReviewed, int DocumentId, string FileStream = "0", Int64 advancePaymentId = 0, int PageNumber = 1, int RowspPage = 1000, Int32 AssignedToId = 0, string PatientLastName = null, string PatientFirstName = null, long NoteId = 0, string DocPriority = "", DateTime? FromExpiry = null, DateTime? ToExpiry = null, Int64 TagId = 0, bool IsRecentDocs = false)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientDocument().searchPatientDocument(PatientDocId, PatientId, PatientAccountNumber, FromDOS, ToDOS, FromEntryDate, ToEntryDate, EnteredBy, ReviewedById, IsReviewed, DocumentId, FileStream, advancePaymentId, PageNumber, RowspPage, AssignedToId, PatientLastName, PatientFirstName, NoteId, DocPriority, FromExpiry, ToExpiry, TagId, IsRecentDocs);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::searchPatientDocument", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> searchPatientLinkedDocument(Int64 MedicalParentDocId)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientDocument().searchPatientLinkedDocument(MedicalParentDocId);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::searchPatientLinkedDocument", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        public FileData GetAudioFile(int VisitId)
        {
            try
            {
                FileData model = new DALPatientDocument().GetAudioFile(VisitId);
                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inserts the patient Document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> InsertPatientDocument(DSPatient ds, bool IsReviewed = false)
        {
            try
            {

                DSMessage dsMessage = new DSMessage();
                bool isMessageRowCreated = false;
                Int64 MessageId = 0;
                // Make Document Name(s) to be put into Message
                string DocumentName = "";

                foreach (DataRow drDocument in ds.Tables[ds.PatientDocument.TableName].Rows)
                {
                    if (DocumentName == "")
                        DocumentName = drDocument[ds.PatientDocument.FilePathColumn.ColumnName] + ", ";
                    else
                        DocumentName += drDocument[ds.PatientDocument.FilePathColumn.ColumnName] + (DocumentName == "" ? "" : ", ");
                }
                if (DocumentName.Length != 0)
                {
                    if (DocumentName[DocumentName.Length - 2] == ',')
                    {
                        DocumentName = DocumentName.Remove(DocumentName.Length - 2);

                    }
                }


                foreach (DataRow drDocument in ds.Tables[ds.PatientDocument.TableName].Rows)
                {
                    string refModuleName = MDVUtility.ToStr(drDocument[ds.PatientDocument.RefModuleNameColumn.ColumnName]);
                    if (refModuleName != "")
                    {
                        drDocument[ds.PatientDocument.DocumentidColumn.ColumnName] = GetDocumentId(ds, refModuleName);
                    }

                    Int64 AssignedToId = MDVUtility.ToInt64(drDocument[ds.PatientDocument.AssignedToIdColumn.ColumnName]);
                    if (!isMessageRowCreated && AssignedToId != 0)
                    {
                        DSMessage.PatMessagesRow drMessage = dsMessage.PatMessages.NewPatMessagesRow();
                        drMessage.PatientId = MDVUtility.ToInt64(drDocument[ds.PatientDocument.PatientIdColumn.ColumnName]);
                        drMessage.AssignedToId = AssignedToId;
                        drMessage.UserName = MDVUtility.ToStr(drDocument[ds.PatientDocument.ViewByColumn.ColumnName]);
                        drMessage.MsgTypeId = 4;// Review
                        drMessage.MsgStatusId = 2;// Unresolved
                        drMessage.MsgDetail = DocumentName + (DocumentName.Contains(",") ? " have" : " has") + " been assigned to " + MDVUtility.ToStr(drDocument[ds.PatientDocument.ViewByColumn.ColumnName]) + " for review. Please click on link to view the document.";
                        drMessage.EntryDate = DateTime.Now;
                        drMessage.IsActive = true;
                        drMessage.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                        drMessage.CreatedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                        drMessage.CreatedOn = DateTime.Now;
                        drMessage.ModifiedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                        drMessage.ModifiedOn = DateTime.Now;
                        dsMessage.PatMessages.AddPatMessagesRow(drMessage);
                        dsMessage = new DALMessage().InsertMessage(dsMessage);
                        MessageId = Convert.ToInt64(dsMessage.Tables[dsMessage.PatMessages.TableName].Rows[0][dsMessage.PatMessages.PatMsgIdColumn.ColumnName]);
                        isMessageRowCreated = true;
                    }
                    if (IsReviewed == true)
                    {
                        drDocument[ds.PatientDocument.ReviewByColumn.ColumnName] = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                        drDocument[ds.PatientDocument.ReviewDateColumn.ColumnName] = DateTime.Now;
                    }

                }

                if (MessageId > 0)
                {
                    foreach (DataRow drDocument in ds.Tables[ds.PatientDocument.TableName].Rows)
                    {
                        drDocument[ds.PatientDocument.MessageIdColumn.ColumnName] = MessageId;
                    }
                }

                ds = new DALPatientDocument().InsertPatientDocument(ds);
                ds.AcceptChanges();

                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientDocument", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        public BLObject<DSPatient> LoadPatientDocumentByFolder(long PatientId, string FolderIDs, SharedVariable sharedVariable = null)
        {
            try
            {
                DSPatient ds = new DSPatient();

                ds = new DALPatientDocument().LoadPatientDocumentByFolder(PatientId, FolderIDs);

                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientDocumentByFolder", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        public BLObject<DSPatient> InsertDocumentUserAccess(DSPatient ds)
        {
            try
            {
                ds = new DALPatientDocument().InsertDocumentUserAccess(ds);
                ds.AcceptChanges();

                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientDocument", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }


        public BLObject<DSPatient> UpdatePatientDocumentFromNotes(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                ds = new DALPatientDocument().UpdatePatientDocumentFromNotes(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientDocument", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        public BLObject<DSPatient> UpdatePatientDocumentFromClaim(DSPatient ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                ds = new DALPatientDocument().UpdatePatientDocumentFromClaim(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientDocumentFromClaim", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        public BLObject<bool> DeleteUnallocatedCopayDocument(Int64 PatientID, string FileName, string dtpDOS, Int64 VisitId = 0)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string fileUrl = "";
                fileUrl = new DALPatientDocument().DeleteUnallocatedCopayDocument(PatientID, FileName, dtpDOS, VisitId = 0);
                return new BLObject<bool>(true, fileUrl);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeleteUnallocatedCopayDocument", ex);
                return new BLObject<bool>(false, ex.Message);
            }
        }



        // PRD-458 for insert update delete documents from multiple folders
        public BLObject<DSPatient> InsertPatientDocMultipleFolder(DSPatient ds, Int64 PatientId, List<CommonSearch> FoldersList, List<CommonSearch> deletedNewFolderList, List<CommonSearch> AddedNewFoldersList, string CurrentForlderId, string CurrentForlderName, string strFolder = null, string IsReviewed = "0", string IsMessage = "0", string IsSigned = "0")
        {
            #region Update
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            bool IsTransaction = false;
            try
            {
                DSMessage dsMessage = new DSMessage();
                bool isMessageRowCreated = false;
                Int64 MessageId = 0;
                string strFilePath = string.Empty;
                string ServerFilePath = string.Empty;
                byte[] byteArr = null;
                byte[] NewbyteArr = null;
                byte[] AddDocbyteArr = null;
                string fileExt = string.Empty;
                string url = string.Empty;
                string targetPath = "";
                string sourcePath = "";
                string sourcePhysicalPath = "";

                // Make Document Name(s) to be put into Message
                string DocumentName = "";

                foreach (DataRow drDocument in ds.Tables[ds.PatientDocument.TableName].Rows) {
                    // get file name Source path and bytArr
                    DocumentName += drDocument[ds.PatientDocument.FilePathColumn.ColumnName] + (DocumentName == "" ? "" : ", ");
                    sourcePath = drDocument[ds.PatientDocument.UrlColumn.ColumnName].ToString();
                    byteArr = drDocument[ds.PatientDocument.FileStreamColumn.ColumnName] as byte[];
                }
                   
                if (!String.IsNullOrEmpty(DocumentName))
                    if (DocumentName[DocumentName.Length - 1] == ',')
                        DocumentName = DocumentName.Remove(DocumentName.Length - 1);
                foreach (DataRow drDocument in ds.Tables[ds.PatientDocument.TableName].Rows)
                {
                    //IsReviewed=1 means document is reviewed.
                    if (IsReviewed == "1")
                    {
                        drDocument[ds.PatientDocument.ReviewDateColumn.ColumnName] = DateTime.Now;
                        drDocument[ds.PatientDocument.ReviewByColumn.ColumnName] = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper();
                    }
                    if (IsSigned == "1")
                    {
                        strFilePath = MDVUtility.ToStr(drDocument[ds.PatientDocument.UrlColumn.ColumnName]);
                        if (!string.IsNullOrWhiteSpace(strFilePath))
                        {
                            ServerFilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                            strFilePath = ServerFilePath + strFilePath;
                            using (var stream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = new BinaryReader(stream))
                                {
                                    byteArr = reader.ReadBytes((int)stream.Length);
                                }
                            }
                        }
                        else
                            byteArr = drDocument[ds.PatientDocument.FileStreamColumn.ColumnName] as byte[];
                        string strWaterMark = null;
                        if (!String.IsNullOrEmpty(drDocument[ds.PatientDocument.ReviewByColumn.ColumnName].ToString()))
                            strWaterMark = "Electronically signed by " + MDVSession.Current.AppUserFullName.ToUpper() + " on " + String.Format("{0:F}", DateTime.Now.ToLocalTime());
                        else
                            strWaterMark = "Electronically signed by " + MDVSession.Current.AppUserFullName.ToUpper() + " on " + String.Format("{0:F}", DateTime.Now.ToLocalTime());

                        if (drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString().Split('/')[0] == "application")
                        {
                            string fileName = string.Empty;
                            if (drDocument[ds.PatientDocument.FilePathColumn.ColumnName].ToString().Split('.').Length > 1)
                                fileName = strFolder + "." + drDocument[ds.PatientDocument.FilePathColumn.ColumnName].ToString().Split('.')[1];
                            else
                                fileName = drDocument[ds.PatientDocument.FilePathColumn.ColumnName].ToString();
                            fileExt = Path.GetExtension(fileName);
                            if (string.IsNullOrEmpty(fileExt))
                            {
                                if (!string.IsNullOrEmpty(drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString()) && drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString() == "application/pdf")
                                {
                                    fileExt = ".pdf";
                                    fileName = fileName + ".pdf";
                                }
                                else if (drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString() == "application/html")
                                {
                                    fileExt = ".html";
                                    fileName = fileName + ".html";
                                }
                            }
                            if (fileExt == ".pdf")
                                NewbyteArr = AddWaterMarkToPDF(byteArr, strWaterMark);
                            else
                                NewbyteArr = byteArr;
                            drDocument[ds.PatientDocument.UrlColumn.ColumnName] = CommonFunc.SaveDocumentToFolder(null, "Patient Document", strFolder, PatientId, fileName, NewbyteArr);
                            if (File.Exists(strFilePath))
                                File.Delete(strFilePath);
                        }
                        else if (drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString().Split('/')[0] == "image")
                        {
                            string filePath = string.Empty;
                            string strFileExt = string.Empty;
                            filePath = drDocument[ds.PatientDocument.FilePathColumn.ColumnName].ToString();
                            int index = filePath.LastIndexOf('.');
                            if (!string.IsNullOrEmpty(strFilePath) && index == -1)
                            {
                                int getUrlExt = strFilePath.LastIndexOf('.');
                                strFileExt = "." + strFilePath.Substring(getUrlExt + 1);
                                filePath = filePath + strFileExt;
                            }
                            else if (!string.IsNullOrEmpty(drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString()))
                            {
                                strFileExt = "." + drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString().Split('/')[1];
                                filePath = filePath + strFileExt;
                            }
                            NewbyteArr = AddWaterMarkToImage(byteArr, strWaterMark, drDocument[ds.PatientDocument.FilePathColumn.ColumnName].ToString());
                            drDocument[ds.PatientDocument.UrlColumn.ColumnName] = CommonFunc.SaveDocumentToFolder(null, "Patient Document", strFolder, PatientId, filePath, NewbyteArr);
                            if (File.Exists(strFilePath))
                                File.Delete(strFilePath);
                        }

                        drDocument[ds.PatientDocument.BUpdateStreamColumn.ColumnName] = true;
                        drDocument[ds.PatientDocument.SignDateColumn.ColumnName] = DateTime.Now;
                        drDocument[ds.PatientDocument.SignByColumn.ColumnName] = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper();
                        drDocument[ds.PatientDocument.ReviewDateColumn.ColumnName] = DateTime.Now;
                        drDocument[ds.PatientDocument.ReviewByColumn.ColumnName] = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper();

                    }

                    Int64 AssignedToId = MDVUtility.ToInt64(drDocument[ds.PatientDocument.AssignedToIdColumn.ColumnName]);
                    if (!isMessageRowCreated && AssignedToId != 0)
                    {
                        if (IsMessage == "0")
                        {
                            DSMessage.PatMessagesRow drMessage = dsMessage.PatMessages.NewPatMessagesRow();
                            drMessage.PatientId = MDVUtility.ToInt64(drDocument[ds.PatientDocument.PatientIdColumn.ColumnName]);
                            drMessage.AssignedToId = AssignedToId;
                            drMessage.UserName = MDVUtility.ToStr(drDocument[ds.PatientDocument.ViewByColumn.ColumnName]);
                            drMessage.MsgTypeId = 4;// Review
                            drMessage.MsgStatusId = 2;// Unresolved
                            drMessage.MsgDetail = DocumentName + (DocumentName.Contains(",") ? " have" : " has") + " been assigned to " + MDVUtility.ToStr(drDocument[ds.PatientDocument.ViewByColumn.ColumnName]) + " for review. Please click on link to view the document.";
                            drMessage.EntryDate = DateTime.Now;
                            drMessage.IsActive = true;
                            drMessage.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                            drMessage.CreatedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                            drMessage.CreatedOn = DateTime.Now;
                            drMessage.ModifiedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                            drMessage.ModifiedOn = DateTime.Now;
                            dsMessage.PatMessages.AddPatMessagesRow(drMessage);
                            dbManager.BeginTransaction();
                            dsMessage = new DALMessage().InsertMessage(dsMessage, dbManager);
                            IsTransaction = true;
                            MessageId = Convert.ToInt64(dsMessage.Tables[dsMessage.PatMessages.TableName].Rows[0][dsMessage.PatMessages.PatMsgIdColumn.ColumnName]);
                        }
                        else if (IsMessage == "1")
                        {
                            MessageId = MDVUtility.ToInt64(drDocument[ds.PatientDocument.MessageIdColumn.ColumnName]);
                            DSMessage dsM = new DSMessage();
                            dsM = new DALMessage().LoadMessage(0, MessageId, "", 0, null, null, 0, "", 1, 1000);
                            if (dsM != null)
                            {
                                dsM.PatMessages.Rows[0][dsMessage.PatMessages.MsgStatusIdColumn.ColumnName] = 1;
                                dbManager.BeginTransaction();
                                new DALMessage().UpdateMessage(dsM, dbManager);
                                IsTransaction = true;
                            }
                        }
                        isMessageRowCreated = true;
                    }
                }
                if (MessageId > 0)
                {
                    foreach (DataRow drDocument in ds.Tables[ds.PatientDocument.TableName].Rows)
                        drDocument[ds.PatientDocument.MessageIdColumn.ColumnName] = MessageId;
                }

                //  Datatable for save Folder Documents
                DataSet Foldersdataset = new DataSet();                
                DataTable dtFolderIDs = new DataTable();
                DataColumn COLUMNID = new DataColumn();
                DataColumn ColumnUrl = new DataColumn();
                DataColumn ColumnStatus = new DataColumn();
                COLUMNID.ColumnName = "ID";
                ColumnUrl.ColumnName = "folderUrl";
                ColumnStatus.ColumnName = "Status";
                ColumnUrl.DataType= typeof(string);
                COLUMNID.DataType = typeof(long);
                ColumnStatus.DataType = typeof(string);
                dtFolderIDs.Columns.Add(COLUMNID);
                dtFolderIDs.Columns.Add(ColumnUrl);
                dtFolderIDs.Columns.Add(ColumnStatus);
                // Added new Folder Add in Data Table 
                foreach (var Folder in AddedNewFoldersList)
                {
                    DataRow Dr = dtFolderIDs.NewRow();
                    Dr["ID"] = Folder.Value;                   
                    Dr["folderUrl"] = GetFileUrl(true,"Patient Document", Folder.Name, PatientId, DocumentName);
                    Dr["Status"] = "Insert";
                    dtFolderIDs.Rows.Add(Dr);
                }
                ////Unselect mark  Folder Add in Data Table
                foreach (var Folder in deletedNewFolderList)
                {
                    DataRow Dr = dtFolderIDs.NewRow();
                    Dr["ID"] = Folder.Value;
                    Dr["folderUrl"] = GetFileUrl(true,"Patient Document", Folder.Name, PatientId, DocumentName);
                    Dr["Status"] = "Delete";
                    dtFolderIDs.Rows.Add(Dr);
                }


                Foldersdataset.Tables.Add(dtFolderIDs);
                string foldersXml = Foldersdataset.GetXml();

                if (!IsTransaction)
                    dbManager.BeginTransaction();

                //string ServerPath = System.Configuration.ConfigurationManager.AppSettings["PatientFilesPath"];
                //sourcePhysicalPath = ServerPath + sourcePath;
                targetPath = "";

                // loop for Copy Files 
                foreach (var Folder in AddedNewFoldersList)
                {
                    // get source file complete physical path
                    string ServerPath = System.Configuration.ConfigurationManager.AppSettings["PatientFilesPath"];
                    string sourcePhysciallyPath = ServerPath + sourcePath;
                    AddDocbyteArr = File.ReadAllBytes(sourcePhysciallyPath);
                    string FolderName  = Folder.Name;
                    CommonFunc.SaveDocumentToFolder(null, "Patient Document", FolderName, PatientId, DocumentName, AddDocbyteArr);
                    //targetPath = GetFileUrl(false, "Patient Document", Folder.Name, PatientId, DocumentName);
                    //CopyFile(sourcePath, targetPath, DocumentName);

                }

                ds = new DALPatientDocument().UpdatePatientDocumentMUltiple(ds, foldersXml, dbManager);
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientDocMultipleFolder", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }

        }


          // Mathod for Copy Files from one folder to Folder        
           public  bool CopyFile(string sourcePath, string targetPath,string fileName)
            {
            // get copy
            byte[] newBytArray = null;
            string ServerPath = System.Configuration.ConfigurationManager.AppSettings["PatientFilesPath"];
            string sourcePhysciallyPath = ServerPath + sourcePath;
            string targetPhysicallyPath = ServerPath + targetPath;
            // get physically source file path
            newBytArray=   File.ReadAllBytes(sourcePhysciallyPath);
            // Use Path class to manipulate file and directory paths.
            string sourceFile = sourcePhysciallyPath;
              string destFile = System.IO.Path.Combine(targetPhysicallyPath, fileName);                     
                if (!System.IO.Directory.Exists(targetPhysicallyPath))
                {
                    System.IO.Directory.CreateDirectory(targetPhysicallyPath);
                }

            if (!System.IO.File.Exists(destFile))
            {
                System.IO.File.Create(destFile);
                // File.Copy(sourceFile, destFile, true);
                BinaryWriter Writer = null;
                // Create a new stream to write to the file
                Writer = new BinaryWriter(File.Open(destFile, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite));
                // Writer raw data 
                Writer.Write(newBytArray);
                Writer.Flush();
                Writer.Close();
            }
            else
            {
                BinaryWriter Writer = null;
                // Create a new stream to write to the file
                Writer = new BinaryWriter(File.Open(destFile, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite));
                // Writer raw data 
                Writer.Write(newBytArray);
                Writer.Flush();
                Writer.Close();
                //System.IO.FileStream fs;
                //fs = System.IO.File.Open(destFile, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None);
                //fs.Close();
                //fs.Flush();
                //fs.Dispose();

                // File.SetAttributes(destFile, FileAttributes.Normal);
                //File.Copy(sourceFile, destFile, true);

            }
            
            
            return true;
                
            }


       
        public static string GetFileUrl(bool requireFileName,string MainHierarchy = null, string Folder = null, long OperationID = 0, string FileName = null)
        {
            string newFullPath = string.Empty;
            int count = 1;

            string ServerPath = System.Configuration.ConfigurationManager.AppSettings["PatientFilesPath"];

            string MainFolder = string.Format("Uploads");
            if (!System.IO.Directory.Exists(ServerPath + MainFolder))
                System.IO.Directory.CreateDirectory(ServerPath + MainFolder);

            MainHierarchy = string.Format(MainFolder + "\\" + MainHierarchy);
            if (!System.IO.Directory.Exists(ServerPath + MainHierarchy))
                System.IO.Directory.CreateDirectory(ServerPath + MainHierarchy);

            string PatientFolder = string.Empty;
            if (OperationID > 0)
            {
                if (MainHierarchy == "Charge Batch Documents")
                {
                    PatientFolder = MainHierarchy + "\\Charge_Batch_" + OperationID;
                }
                else if (MainHierarchy == "Payment Batch Documents")
                {
                    PatientFolder = MainHierarchy + "\\Payment_Batch_" + OperationID;
                }
                else if (MainHierarchy == "Patient Document")
                {
                    PatientFolder = MainHierarchy + "\\Patient_" + OperationID;
                }
                else
                {
                    PatientFolder = MainHierarchy + "\\Patient_" + OperationID;
                }
            }
            else
            {
                PatientFolder = MainHierarchy + "\\Date_" + DateTime.Now.Date.ToString("dd-MMM-yyy");
            }
            if (!System.IO.Directory.Exists(ServerPath + PatientFolder))
                System.IO.Directory.CreateDirectory(ServerPath + PatientFolder);

            string DocumentFolder = PatientFolder +"\\"+Folder;
            if (!System.IO.Directory.Exists(ServerPath + DocumentFolder))
                System.IO.Directory.CreateDirectory(ServerPath + DocumentFolder);

            DocumentFolder = DocumentFolder + "\\" + DateTime.Now.Date.ToString("dd-MMM-yyy");
            if (!System.IO.Directory.Exists(ServerPath + DocumentFolder))
                System.IO.Directory.CreateDirectory(ServerPath + DocumentFolder);
            
            if (requireFileName)
            {
                int idx = FileName.LastIndexOf('.');
                string fileNameOnly = FileName.Substring(0, idx);// FileName.Split('.')[0];
                string extension = FileName.Substring(idx + 1);// FileName.Split('.')[1];

                newFullPath = string.Format(DocumentFolder + "\\{0}.{1}", fileNameOnly, extension);

            }
            else {
                newFullPath = DocumentFolder;
            }
            return newFullPath;
        }


        /// <summary>
        /// Updates the patient Document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatient&gt;.</returns>
        public BLObject<DSPatient> UpdatePatientDocument(DSPatient ds, Int64 PatientId, string strFolder = null, string IsReviewed = "0", string IsMessage = "0", string IsSigned = "0")
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            bool IsTransaction = false;
            try
            {


                DSMessage dsMessage = new DSMessage();
                bool isMessageRowCreated = false;
                Int64 MessageId = 0;
                string strFilePath = string.Empty;
                string ServerFilePath = string.Empty;
                // Make Document Name(s) to be put into Message
                string DocumentName = "";
                foreach (DataRow drDocument in ds.Tables[ds.PatientDocument.TableName].Rows)
                {
                    DocumentName += drDocument[ds.PatientDocument.FilePathColumn.ColumnName] + (DocumentName == "" ? "" : ", ");
                }
                if (!String.IsNullOrEmpty(DocumentName))
                {
                    if (DocumentName[DocumentName.Length - 1] == ',')
                    {
                        DocumentName = DocumentName.Remove(DocumentName.Length - 1);
                    }
                }
                foreach (DataRow drDocument in ds.Tables[ds.PatientDocument.TableName].Rows)
                {
                    //IsReviewed=1 means document is reviewed.
                    if (IsReviewed == "1")
                    {
                        drDocument[ds.PatientDocument.ReviewDateColumn.ColumnName] = DateTime.Now;
                        drDocument[ds.PatientDocument.ReviewByColumn.ColumnName] = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper();
                    }
                    if (IsSigned == "1")
                    {
                        strFilePath = MDVUtility.ToStr(drDocument[ds.PatientDocument.UrlColumn.ColumnName]);
                        byte[] byteArr = null;
                        byte[] NewbyteArr = null;
                        if (!string.IsNullOrWhiteSpace(strFilePath))
                        {
                            ServerFilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                            strFilePath = ServerFilePath + strFilePath;
                            using (var stream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = new BinaryReader(stream))
                                {
                                    byteArr = reader.ReadBytes((int)stream.Length);
                                }
                            }
                        }
                        else
                        {
                            byteArr = drDocument[ds.PatientDocument.FileStreamColumn.ColumnName] as byte[];
                        }
                        string strWaterMark = null;
                        if (!String.IsNullOrEmpty(drDocument[ds.PatientDocument.ReviewByColumn.ColumnName].ToString()))
                        {


                            //if (!String.IsNullOrEmpty(drDocument[ds.PatientDocument.ProviderTypeColumn.ColumnName].ToString()) && drDocument[ds.PatientDocument.ProviderTypeColumn.ColumnName].ToString() == "Physician")
                            //{
                            //    strWaterMark = "Electronically signed by Dr " + MDVSession.Current.AppUserFullName.ToUpper() + " on " + String.Format("{0:F}", DateTime.Now.ToLocalTime());
                            //}
                            //else {
                            //    strWaterMark = "Electronically signed by " + MDVSession.Current.AppUserFullName.ToUpper() + " on " + String.Format("{0:F}", DateTime.Now.ToLocalTime());
                            //}
                            strWaterMark = "Electronically signed by " + MDVSession.Current.AppUserFullName.ToUpper() + " on " + String.Format("{0:F}", DateTime.Now.ToLocalTime());
                        }

                        else
                            strWaterMark = "Electronically signed by " + MDVSession.Current.AppUserFullName.ToUpper() + " on " + String.Format("{0:F}", DateTime.Now.ToLocalTime());

                        if (drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString().Split('/')[0] == "application")
                        {
                            string fileName = string.Empty;
                            if (drDocument[ds.PatientDocument.FilePathColumn.ColumnName].ToString().Split('.').Length > 1)
                                fileName = strFolder + "." + drDocument[ds.PatientDocument.FilePathColumn.ColumnName].ToString().Split('.')[1];
                            else
                                fileName = drDocument[ds.PatientDocument.FilePathColumn.ColumnName].ToString();
                            string fileExt = Path.GetExtension(fileName);
                            if (string.IsNullOrEmpty(fileExt))
                            {
                                if (!string.IsNullOrEmpty(drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString()) && drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString() == "application/pdf")
                                {
                                    fileExt = ".pdf";
                                    fileName = fileName + ".pdf";
                                }
                                else if (drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString() == "application/html")
                                {
                                    fileExt = ".html";
                                    fileName = fileName + ".html";
                                }
                            }
                            if (fileExt == ".pdf")
                                NewbyteArr = AddWaterMarkToPDF(byteArr, strWaterMark);
                            else
                                NewbyteArr = byteArr;

                            drDocument[ds.PatientDocument.UrlColumn.ColumnName] = CommonFunc.SaveDocumentToFolder(null, "Patient Document", strFolder, PatientId, fileName, NewbyteArr);
                            if (File.Exists(strFilePath))
                            {
                                File.Delete(strFilePath);
                            }
                            //drDocument[ds.PatientDocument.FileStreamColumn.ColumnName] = AddWaterMarkToPDF(byteArr, strWaterMark);
                        }
                        else if (drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString().Split('/')[0] == "image")
                        {
                            string filePath = string.Empty;
                            string strFileExt = string.Empty;
                            filePath = drDocument[ds.PatientDocument.FilePathColumn.ColumnName].ToString();
                            int index = filePath.LastIndexOf('.');
                            if (!string.IsNullOrEmpty(strFilePath) && index == -1)
                            {
                                int getUrlExt = strFilePath.LastIndexOf('.');
                                strFileExt = "." + strFilePath.Substring(getUrlExt + 1);
                                filePath = filePath + strFileExt;
                            }
                            else if (!string.IsNullOrEmpty(drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString()))
                            {
                                strFileExt = "." + drDocument[ds.PatientDocument.FileTypeColumn.ColumnName].ToString().Split('/')[1];
                                filePath = filePath + strFileExt;
                            }
                            NewbyteArr = AddWaterMarkToImage(byteArr, strWaterMark, drDocument[ds.PatientDocument.FilePathColumn.ColumnName].ToString());
                            drDocument[ds.PatientDocument.UrlColumn.ColumnName] = CommonFunc.SaveDocumentToFolder(null, "Patient Document", strFolder, PatientId, filePath, NewbyteArr);
                            if (File.Exists(strFilePath))
                            {
                                File.Delete(strFilePath);
                            }
                            //drDocument[ds.PatientDocument.FileStreamColumn.ColumnName] = AddWaterMarkToImage(byteArr, strWaterMark, drDocument[ds.PatientDocument.FilePathColumn.ColumnName].ToString());
                        }

                        drDocument[ds.PatientDocument.BUpdateStreamColumn.ColumnName] = true;
                        drDocument[ds.PatientDocument.SignDateColumn.ColumnName] = DateTime.Now;
                        drDocument[ds.PatientDocument.SignByColumn.ColumnName] = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper();
                        drDocument[ds.PatientDocument.ReviewDateColumn.ColumnName] = DateTime.Now;
                        drDocument[ds.PatientDocument.ReviewByColumn.ColumnName] = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper();

                    }

                    Int64 AssignedToId = MDVUtility.ToInt64(drDocument[ds.PatientDocument.AssignedToIdColumn.ColumnName]);
                    if (!isMessageRowCreated && AssignedToId != 0)
                    {
                        if (IsMessage == "0")
                        {
                            DSMessage.PatMessagesRow drMessage = dsMessage.PatMessages.NewPatMessagesRow();
                            drMessage.PatientId = MDVUtility.ToInt64(drDocument[ds.PatientDocument.PatientIdColumn.ColumnName]);
                            drMessage.AssignedToId = AssignedToId;
                            drMessage.UserName = MDVUtility.ToStr(drDocument[ds.PatientDocument.ViewByColumn.ColumnName]);
                            drMessage.MsgTypeId = 4;// Review
                            drMessage.MsgStatusId = 2;// Unresolved
                            drMessage.MsgDetail = DocumentName + (DocumentName.Contains(",") ? " have" : " has") + " been assigned to " + MDVUtility.ToStr(drDocument[ds.PatientDocument.ViewByColumn.ColumnName]) + " for review. Please click on link to view the document.";
                            drMessage.EntryDate = DateTime.Now;
                            drMessage.IsActive = true;
                            drMessage.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                            drMessage.CreatedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                            drMessage.CreatedOn = DateTime.Now;
                            drMessage.ModifiedBy = MDVUtility.ToStr(ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                            drMessage.ModifiedOn = DateTime.Now;
                            dsMessage.PatMessages.AddPatMessagesRow(drMessage);
                            dbManager.BeginTransaction();
                            dsMessage = new DALMessage().InsertMessage(dsMessage, dbManager);
                            IsTransaction = true;
                            MessageId = Convert.ToInt64(dsMessage.Tables[dsMessage.PatMessages.TableName].Rows[0][dsMessage.PatMessages.PatMsgIdColumn.ColumnName]);
                        }
                        else if (IsMessage == "1")
                        {
                            MessageId = MDVUtility.ToInt64(drDocument[ds.PatientDocument.MessageIdColumn.ColumnName]);
                            DSMessage dsM = new DSMessage();
                            dsM = new DALMessage().LoadMessage(0, MessageId, "", 0, null, null, 0, "", 1, 1000);
                            if (dsM != null)
                            {

                                dsM.PatMessages.Rows[0][dsMessage.PatMessages.MsgStatusIdColumn.ColumnName] = 1;

                                dbManager.BeginTransaction();
                                new DALMessage().UpdateMessage(dsM, dbManager);
                                IsTransaction = true;
                            }
                        }
                        isMessageRowCreated = true;

                    }

                }

                if (MessageId > 0)
                {
                    foreach (DataRow drDocument in ds.Tables[ds.PatientDocument.TableName].Rows)
                    {
                        drDocument[ds.PatientDocument.MessageIdColumn.ColumnName] = MessageId;
                    }
                }

                if (!IsTransaction)
                    dbManager.BeginTransaction();
                ds = new DALPatientDocument().UpdatePatientDocument(ds, dbManager);

                dbManager.CommitTransaction();
                ds.AcceptChanges();

                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientDocument", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public BLObject<DSPatient> UpdatePatientDocument(DSPatient dsPatient)
        {
            try
            {
                DSPatient ds = new DALPatientDocument().UpdatePatientDocument(dsPatient);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientDocument", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the patient Document.
        /// </summary>
        /// <param name="PatientDocId">The patient Document identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeletePatientDocument(string PatientDocId, string ChildPatientDocId, string LinkDocumentId)
        {
            try
            {
                PatientDocId = new DALPatientDocument().DeletePatientDocument(PatientDocId, ChildPatientDocId, LinkDocumentId);
                return new BLObject<string>(PatientDocId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientDocument", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        
          public BLObject<string> DeAttachClaimDocument(string PatientDocId)
           {
            try
            {
                PatientDocId = new DALPatientDocument().DeAttachClaimDocument(PatientDocId);
                return new BLObject<string>(PatientDocId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeAttachClaimDocument", ex);
                return new BLObject<string>(null, ex.Message);
            }
           }

        public BLObject<string> DeleteInsuranceDocument(Int64 PatientInsuranceId, Int64 PatientID)
        {
            try
            {
                string strPatientInsuranceId = string.Empty;
                strPatientInsuranceId = new DALPatientDocument().DeleteInsuranceDocument(PatientInsuranceId, PatientID);
                return new BLObject<string>(strPatientInsuranceId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientDocument", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<List<PatientDocumentModel>> LoadAttachedPatientDocument(string PatientDocId, long PatientId, string PatientAccountNumber, string PatientLastName
                                   , string PatientFirstName, DateTime? FromDOS, DateTime? ToDOS, DateTime? FromEntryDate
                                   , DateTime? ToEntryDate, string EnteredBy, long AssignedById, long ReviewedById, string IsReviewed, int DocumentId, string IsActive, string FileStream = "0", Int64 AdvancePaymentId = 0, int PageNumber = 1, int RowspPage = 1000, Int32 AssignedToId = 0, Int64 TransitionId = 0, string RefModuleName = "", long OrderSetReferralId = 0, long OrderSetPatEducationId = 0, SharedVariable sharedVariable = null)
        {
            try
            {
                List<PatientDocumentModel> documentModel = new List<PatientDocumentModel>();
                DSPatient ds = new DSPatient();
                if (sharedVariable != null)
                {
                    documentModel = new DALPatientDocument(sharedVariable).LoadAttachedPatientDocument(PatientDocId, PatientId, PatientAccountNumber, PatientLastName, PatientFirstName, FromDOS, ToDOS, FromEntryDate, ToEntryDate, EnteredBy, AssignedById, ReviewedById, IsReviewed, DocumentId, IsActive, FileStream, AdvancePaymentId, PageNumber, RowspPage, AssignedToId, TransitionId, RefModuleName, OrderSetReferralId, OrderSetPatEducationId, sharedVariable);
                }
                else
                {
                    documentModel = new DALPatientDocument().LoadAttachedPatientDocument(PatientDocId, PatientId, PatientAccountNumber, PatientLastName, PatientFirstName, FromDOS, ToDOS, FromEntryDate, ToEntryDate, EnteredBy, AssignedById, ReviewedById, IsReviewed, DocumentId, IsActive, FileStream, AdvancePaymentId, PageNumber, RowspPage, AssignedToId, TransitionId, RefModuleName, OrderSetReferralId, OrderSetPatEducationId);
                }
                return new BLObject<List<PatientDocumentModel>>(documentModel);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadAttachedPatientDocument", ex);
                return new BLObject<List<PatientDocumentModel>>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> LoadPatientInformationSubmission(long Id, long PatientId, string Status = "", int PageNumber = 1, int RowspPage = 15)
        {
            try
            {
                DSPatient ds = new DSPatient();
                ds = new DALPatientDocument().LoadPatientInformationSubmission(Id, PatientId, Status, PageNumber, RowspPage);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::SearchPatientInformationSubmission", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        public BLObject<DSPatient> UpdatePatientInformationSubmission(DSPatient ds)
        {
            try
            {
                ds = new DALPatientDocument().UpdatePatientInformationSubmission(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientInformationSubmission", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        public List<PatientDocumentPriorityModel> LoadPatientDocumentPriority()
        {
            return new DALPatientDocument().LoadPatientDocumentPriority();

        }
        /// <summary>
        /// Load Patient Visit DOS 
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public List<PatientVisitDOS> GetPatientVisitDOS(Int64 PatientId)
        {
            return new DALPatientDocument().GetPatientVisitDOS(PatientId);
        }

        public BLObject<List<PatientDocumentModel>> LoadPatientDocumentExpiryAlert(long UserId, long PatientId)
        {
            try
            {
                List<PatientDocumentModel> documentModel = new List<PatientDocumentModel>();
                documentModel = new DALPatientDocument().LoadPatientDocumentExpiryAlert(UserId, PatientId);
                return new BLObject<List<PatientDocumentModel>>(documentModel);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientDocumentExpiryAlert", ex);
                return new BLObject<List<PatientDocumentModel>>(null, ex.Message);
            }
        }

        public BLObject<string> InsertPatientDocumentExpiryAlert(long UserId, ref DataTable PatDocIds, string LookupName, DateTime CreatedOn)
        {
            try
            {
                string returnValue = "";
                returnValue = new DALPatientDocument().InsertPatientDocumentExpiryAlert(UserId, ref PatDocIds, LookupName, CreatedOn);
                return new BLObject<string>(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientDocumentExpiryAlert", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public List<DocumentPrivacyModel> CheckForPrivacy(Int64 PatientId, Int64 PatDocId)
        {
            try
            {
                return new DALPatientDocument().CheckForPrivacy(PatientId, PatDocId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientInformationSubmission", ex);
                return new List<DocumentPrivacyModel>();
            }
        }

        public List<DocumentPrivacyModel> DocPasswordMatch(string Password, Int64 PatDocId)
        {
            try
            {
                return new DALPatientDocument().DocPasswordMatch(Password, PatDocId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DocPasswordMatch", ex);
                return new List<DocumentPrivacyModel>();
            }
        }

        public List<DocumentPrivacyModel> DOCPasswordSave(string Password, Int64 PatDocId)
        {
            try
            {
                return new DALPatientDocument().DOCPasswordSave(Password, PatDocId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DocPasswordMatch", ex);
                return new List<DocumentPrivacyModel>();
            }
        }

        public List<DocumentPrivacyModel> ChangeOrRemovePassword(Int64 PatDocID, string OldPassword, string NewPassword)
        {
            try
            {
                return new DALPatientDocument().ChangeOrRemovePassword(PatDocID, OldPassword, NewPassword);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::ChangeOrRemovePassword", ex);
                return new List<DocumentPrivacyModel>();
            }
        }
        /// <summary>
        /// insert Document tags
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetPatientVisitDOS(PatientDocumentTag model)
        {
            return new DALPatientDocument().InsertPatientDocumentTags(model);
        }
        #endregion

        #region Case
        /// <summary>
        /// Loads the Case MAnagement.
        /// </summary>
        /// <param name="CaseMgmtId">The Case identifier.</param>

        /// <returns>BLObject&lt;DSCase&gt;.</returns>
        public BLObject<DSCase> LoadCaseManagement(string AccountNumber, string ClaimNumber, long PatientId, long CaseId, long PatientInsuranceId, int CaseTypeId, long FacilityId, long ReffProviderId, string IsActive, string CaseNumber, int PageNumber = 1, int RowspPage = 1000)
        {
            try
            {
                DSCase ds = new DSCase();
                ds = new DALCase().LoadCaseManagement(AccountNumber, ClaimNumber, PatientId, CaseId, PatientInsuranceId, CaseTypeId, FacilityId, ReffProviderId, IsActive, CaseNumber, PageNumber, RowspPage);
                return new BLObject<DSCase>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadCaseManagement", ex);
                return new BLObject<DSCase>(null, ex.Message);
            }
        }
        public BLObject<DSBatchCharge> SearchClaimDocuments(string CaseId)
        {
            try
            {
                DSBatchCharge ds = new DSBatchCharge();
                ds = new DALCase().LoadBatchChargeDocument(CaseId);

                return new BLObject<DSBatchCharge>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadBatchChargeDocument", ex);
                return new BLObject<DSBatchCharge>(null, ex.Message);
            }

        }

        /// <summary>
        /// Inserts the Case Management.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSCase&gt;.</returns>
        ///
        public BLObject<DSCase> InsertCaseManagement(DSCase ds)
        {
            try
            {
                ds = new DALCase().InsertCaseManagement(ds);
                return new BLObject<DSCase>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertCaseManagement", ex);
                return new BLObject<DSCase>(null, ex.Message);
            }
        }


        /// <summary>
        /// Updates the Case Management.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSCase&gt;.</returns>
        ///

        public BLObject<DSCase> UpdateCaseManagement(DSCase ds)
        {
            try
            {
                ds = new DALCase().UpdateCaseManagement(ds);
                return new BLObject<DSCase>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdateCaseManagement", ex);
                return new BLObject<DSCase>(null, ex.Message);
            }
        }


        /// <summary>
        /// Deletes the Case Management.
        /// </summary>
        /// <param name="CaseMgmtId">The case identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteCaseManagement(string CaseMgmtId)
        {
            try
            {
                CaseMgmtId = new DALCase().DeleteCaseManagement(CaseMgmtId);
                return new BLObject<string>(CaseMgmtId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeleteCaseManagement", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        #endregion

        #region WCNF
        /// <summary>
        /// Loads the Case MAnagement.
        /// </summary>
        /// <param name="CaseMgmtId">The Case identifier.</param>

        /// <returns>BLObject&lt;DSCase&gt;.</returns>
        public BLObject<DSWCNF> LoadWCNFCaseManagement(long CaseId)
        {
            try
            {
                DSWCNF ds = new DSWCNF();
                ds = new DALCaseWCNF().LoadCaseManagementWCNF(CaseId);
                return new BLObject<DSWCNF>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadCaseManagement", ex);
                return new BLObject<DSWCNF>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the Case Management.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSCase&gt;.</returns>
        ///
        public BLObject<DSWCNF> InsertWCNFCaseManagement(DSWCNF ds)
        {
            try
            {
                ds = new DALCaseWCNF().InsertWCNFCaseManagement(ds);
                return new BLObject<DSWCNF>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertWCNFCaseManagement", ex);
                return new BLObject<DSWCNF>(null, ex.Message);
            }
        }


        /// <summary>
        /// Updates the Case Management.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSCase&gt;.</returns>
        ///

        public BLObject<DSWCNF> UpdateWCNFCaseManagement(DSWCNF ds)
        {
            try
            {
                ds = new DALCaseWCNF().UpdateWCNFCaseManagement(ds);
                return new BLObject<DSWCNF>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdateCaseManagement", ex);
                return new BLObject<DSWCNF>(null, ex.Message);
            }
        }


        /// <summary>
        /// Deletes the WCNF Case Management.
        /// </summary>
        /// <param name="CaseMgmtId">The case identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteWCNFCaseManagement(string CaseMgmtId)
        {
            try
            {
                CaseMgmtId = new DALCaseWCNF().DeleteWNCFCaseManagement(CaseMgmtId);
                return new BLObject<string>(CaseMgmtId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeleteCaseManagement", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSWCNF> LoadVisitRecord(string CaseNumber)
        {
            try
            {
                DSWCNF ds = new DSWCNF();
                ds = new DALCaseWCNF().LoadVisitRecord(CaseNumber);
                return new BLObject<DSWCNF>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::SelectVisitCaseManagement", ex);
                return new BLObject<DSWCNF>(null, ex.Message);
            }
        }
        #endregion

        #region
        /// <summary>
        /// Loads the Case Adjuster.
        /// </summary>
        /// <param name="CaseAdjustertId">The Case identifier.</param>

        /// <returns>BLObject&lt;DSCase&gt;.</returns>
        public BLObject<DSCase> LoadCaseAdjuster(long CaseAdjusterId, string FirstName, string LastName, string Address, string City, string State, string Zip, string Extention, string IsActive, int PageNumber = 1, int RowspPage = 1000)
        {
            try
            {
                DSCase ds = new DSCase();
                ds = new DALCaseAdjuster().LoadCaseAdjuster(CaseAdjusterId, FirstName, LastName, Address, City, State, Zip, Extention, IsActive, PageNumber, RowspPage);
                return new BLObject<DSCase>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadCaseAdjuster", ex);
                return new BLObject<DSCase>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the Case Management.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSCase&gt;.</returns>
        ///
        public BLObject<DSCase> InsertCaseAdjuster(DSCase ds)
        {
            try
            {
                ds = new DALCaseAdjuster().InsertCaseAjuster(ds);
                return new BLObject<DSCase>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertCaseAdjuster", ex);
                return new BLObject<DSCase>(null, ex.Message);
            }
        }


        /// <summary>
        /// Updates the Case Management.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSCase&gt;.</returns>
        ///

        public BLObject<DSCase> UpdateCaseAdjuster(DSCase ds)
        {
            try
            {
                ds = new DALCaseAdjuster().UpdateAdjuster(ds);
                return new BLObject<DSCase>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdateCaseAdjuster", ex);
                return new BLObject<DSCase>(null, ex.Message);
            }
        }


        /// <summary>
        /// Deletes the Case Adjuster.
        /// </summary>
        /// <param name="CaseAdjuster">The case identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteAdjuster(string CaseAdjusterId)
        {
            try
            {
                CaseAdjusterId = new DALCaseAdjuster().DeleteCaseAdjuster(CaseAdjusterId);
                return new BLObject<string>(CaseAdjusterId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeleteAdjuster", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "Lookups"
        /// <summary>
        /// Lookups the ethnicity.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupEthnicity()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupEthnicity();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupEthnicity", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        public BLObject<List<EthnicityModel>> LookupEthnicityDemographic()
        {
            try
            {
                var ds = new DALPatient().LookupEthnicityDemographic();
                return new BLObject<List<EthnicityModel>>(ds);


            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupEthnicityDemographic", ex);
                return new BLObject<List<EthnicityModel>>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the ethnicity.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupReferralStatus()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupReferralStatus();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupReferralStatus", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }
        /// <summary>
        /// Lookups the race.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupRace()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupRace();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupRace", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }
        public BLObject<DSPatientLookups> LookupRaceByDescription(string Description)
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupRaceByDescription(Description);

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupRaceByDescription", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }
        public BLObject<List<LookUpModel>> LoadGenderIdentityLookUp()
        {
            try
            {
                List<LookUpModel> GenderIdentityList = new List<LookUpModel>();
                GenderIdentityList = new DALPatient().LoadGenderIdentityLookUp();
                return new BLObject<List<LookUpModel>>(GenderIdentityList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadGenderIdentityLookUp", ex);
                return new BLObject<List<LookUpModel>>(null, ex.Message);
            }
        }
        public BLObject<List<LookUpModel>> LoadSexualOrientationLookUp()
        {
            try
            {
                List<LookUpModel> SexualOrientationList = new List<LookUpModel>();
                SexualOrientationList = new DALPatient().LoadSexualOrientationLookUp();
                return new BLObject<List<LookUpModel>>(SexualOrientationList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadSexualOrientationLookUp", ex);
                return new BLObject<List<LookUpModel>>(null, ex.Message);
            }
        }
        public BLObject<List<CustomModel>> LookupPatientEthnicityDemographics()
        {
            try
            {
                var mdl = new DALPatient().LookupPatientEthnicityDemographics();
                return new BLObject<List<CustomModel>>(mdl);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPatientEthnicityDemographics", ex);
                return new BLObject<List<CustomModel>>(null, ex.Message);
            }
        }
        public BLObject<List<CustomModel>> LookupPatientRaceDemographics()
        {
            try
            {
                var mdl = new DALPatient().LookupPatientRaceDemographics();
                return new BLObject<List<CustomModel>>(mdl);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPatientEthnicityDemographic", ex);
                return new BLObject<List<CustomModel>>(null, ex.Message);
            }
        }

        public BLObject<List<RaceModel>> LookupRaceDemographic()
        {
            try
            {
                var ds = new DALPatient().LookupRaceDemographic();
                return new BLObject<List<RaceModel>>(ds);


            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupRaceDemographic", ex);
                return new BLObject<List<RaceModel>>(null, ex.Message);
            }

        }
        public BLObject<DSPatientLookups> GetStatusReasons(long StatusId)
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().GetStatusReasons(StatusId);
                return new BLObject<DSPatientLookups>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::GetStatusReasons", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }
    }

        
        /// <summary>
        /// Lookups the communication.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupCommunication()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupCommunication();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupCommunication", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the school.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupSchool()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupSchool();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupSchool", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the school status.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupSchoolStatus()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupSchoolStatus();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupSchoolStatus", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the smokers status.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupSmokersStatus()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupSmokersStatus();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupSmokersStatus", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the languages.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupLanguages(string LanguageName = "")
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupLanguages(LanguageName);

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupLanguages", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }
        public BLObject<DSPatientLookups> LookupCountries(string CountryName = "")
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupCountries(CountryName);
                return new BLObject<DSPatientLookups>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupCountries", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }
        }

        public BLObject<List<CityModel>> LookupCities(string CityName)
        {
            try
            {
                var cityList = new DALPatient().LookupCities(CityName);
                return new BLObject<List<CityModel>>(cityList);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupCities", ex);
                return new BLObject<List<CityModel>>(null, ex.Message);
            }

        }

        public BLObject<DSPatientLookups> LookupPreferredAddress()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupPreferredAddress();
                return new BLObject<DSPatientLookups>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPreferredAddress", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }
        }

        public BLObject<DSPatientLookups> LookupPreferredPhone()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupPreferredPhone();
                return new BLObject<DSPatientLookups>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPreferredPhone", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the marital status.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupMaritalStatus()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupMaritalStatus();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupMaritalStatus", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        public BLObject<List<MaritalStatusModel>> LookupMaritalStatusDemographic()
        {
            try
            {

                var ds = new DALPatient().LookupMaritalStatusDemographic();
                return new BLObject<List<MaritalStatusModel>>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupMaritalStatusDemographic", ex);
                return new BLObject<List<MaritalStatusModel>>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the prefix.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupPrefix()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupPrefix();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPrefix", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the suffix.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupSuffix()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatient().LookupSuffix();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupSuffix", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        public BLObject<List<SuffixModel>> LookupSuffixDemographic()
        {
            try
            {
                var ds = new DALPatient().LookupSuffixDemographic();
                return new BLObject<List<SuffixModel>>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupSuffixDemographic", ex);
                return new BLObject<List<SuffixModel>>(null, ex.Message);
            }

        }

        public BLObject<List<PrefixModel>> LookupPrefixDemographic()
        {
            try
            {
                var ds = new DALPatient().LookupPrefixDemographic();
                return new BLObject<List<PrefixModel>>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPrefixDemographic", ex);
                return new BLObject<List<PrefixModel>>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the type of the insurance plan.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupInsurancePlanType()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatientInsurance().LookupInsurancePlanType();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupInsurancePlanType", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        public BLObject<List<InsurancePlanTypeModel>> LookupInsurancePlanTypeDemographic()
        {
            try
            {
                var ds = new DALPatientInsurance().LookupInsurancePlanTypeDemographic();
                return new BLObject<List<InsurancePlanTypeModel>>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupInsurancePlanTypeDemographic", ex);
                return new BLObject<List<InsurancePlanTypeModel>>(null, ex.Message);
            }

        }

        public BLObject<DSPatientLookups> LookupPatientInsurance(long PatientId, string IsActive = null)
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatientInsurance().LookupPatientInsurance(PatientId, IsActive);

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPatientInsurance", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        public BLObject<List<GenericLookupModel>> LookupPatientVisitInsurance(long VisitId, string IsActive = null)
        {
            try
            {
                List<GenericLookupModel> list = new List<GenericLookupModel>();
                list = new DALPatientInsurance().LookupPatientVisitInsurance(VisitId, IsActive);

                return new BLObject<List<GenericLookupModel>>(list);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPatientInsurance", ex);
                return new BLObject<List<GenericLookupModel>>(null, ex.Message);
            }

        }


        /// <summary>
        /// Lookups the patient relation.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupPatientRelation()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatientInsurance().LookupPatientRelation();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPatientRelation", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the type of the MSP.
        /// </summary>
        /// <returns>BLObject&lt;DSPatientLookups&gt;.</returns>
        public BLObject<DSPatientLookups> LookupMSPType()
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALPatientInsurance().LookupMSPType();

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupMSPType", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }

        public BLObject<List<MSPTypeModel>> LookupMSPTypeDemographic()
        {
            try
            {
                var ds = new DALPatientInsurance().LookupMSPTypeDemographic();
                return new BLObject<List<MSPTypeModel>>(ds);


            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupMSPTypeDemographic", ex);
                return new BLObject<List<MSPTypeModel>>(null, ex.Message);
            }

        }
        /// <summary>
        /// Lookups the type of the MSP.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSPatientLookups> LookupEmployer(string EmployerName = "")
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALEmployer().LookupEmployer(EmployerName);
                return new BLObject<DSPatientLookups>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupEmployer", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the lawyer.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSPatientLookups> LookupLawyer(string LawyerName = "")
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();
                ds = new DALLawyer().LookupLawyer(LawyerName);
                return new BLObject<DSPatientLookups>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupLawyer", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }
        }


        /// <summary>
        /// Lookups the Case Type.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCaseLookup> LookupCaseType()
        {
            try
            {
                DSCaseLookup ds = new DSCaseLookup();
                ds = new DALCase().LookupCaseType();
                return new BLObject<DSCaseLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupCaseType", ex);
                return new BLObject<DSCaseLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the Patient Case.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCaseLookup> LookupCaseManagement(long PatientId)
        {
            try
            {
                DSCaseLookup ds = new DSCaseLookup();
                ds = new DALCase().LookupCaseManagement(PatientId);
                return new BLObject<DSCaseLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupCaseManagement", ex);
                return new BLObject<DSCaseLookup>(null, ex.Message);
            }
        }
        public BLObject<DSCaseLookup> LookupConditionCode()
        {
            try
            {
                DSCaseLookup ds = new DSCaseLookup();
                ds = new DALCase().LookupConditionCode();
                return new BLObject<DSCaseLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupConditionCode", ex);
                return new BLObject<DSCaseLookup>(null, ex.Message);
            }
        }


        public BLObject<DSPatientLookups> LookupPatientReferral(string IsActive = null)
        {
            try
            {
                DSPatientLookups ds = new DSPatientLookups();

                ds = new DALPatientInsurance().LookupPatientReferral(IsActive);

                return new BLObject<DSPatientLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPatientReferral", ex);
                return new BLObject<DSPatientLookups>(null, ex.Message);
            }

        }


        public BLObject<DSPatientPortal> LookupSecurityQuestions()
        {
            try
            {
                DSPatientPortal ds = new DSPatientPortal();
                ds = new DALPatientPortal().LookupSecurityQuestions();

                return new BLObject<DSPatientPortal>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupSecurityQuestion", ex);
                return new BLObject<DSPatientPortal>(null, ex.Message);
            }

        }
        public BLObject<List<CustomModel>> LookupPatientCareGiver(long PatientId)
        {
            try
            {
                var mdl = new DALPatient().LookupPatientCareGiver(PatientId);
                return new BLObject<List<CustomModel>>(mdl);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPatientCareGiver", ex);
                return new BLObject<List<CustomModel>>(null, ex.Message);
            }
        }
        #endregion

        #region "Helpers"
        public byte[] AddWaterMarkToPDF(byte[] byteArr, string strWaterMark)
        {
            //create pdfreader object to read sorce pdf
            PdfReader pdfReader = new PdfReader(byteArr);
            using (MemoryStream stream = new MemoryStream())
            {
                //create pdfstamper object which is used to add addtional content to source pdf file
                PdfStamper pdfStamper = new PdfStamper(pdfReader, stream);
                //iterate through all pages in source pdf
                for (int pageIndex = 1; pageIndex <= pdfReader.NumberOfPages; pageIndex++)
                {
                    //Rectangle class in iText represent geomatric representation... in this case, rectanle object would contain page geomatry
                    iTextSharp.text.Rectangle pageRectangle = pdfReader.GetPageSizeWithRotation(pageIndex);
                    //pdfcontentbyte object contains graphics and text content of page returned by pdfstamper

                    // That line was adding water mark beneath the content
                    //PdfContentByte pdfData = pdfStamper.GetUnderContent(pageIndex);
                    PdfContentByte pdfData = pdfStamper.GetOverContent(pageIndex);

                    //create fontsize for watermark

                    pdfData.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 14);
                    //create new graphics state and assign opacity
                    PdfGState graphicsState = new PdfGState();
                    graphicsState.FillOpacity = 0.4F;
                    //set graphics state to pdfcontentbyte
                    pdfData.SetGState(graphicsState);
                    //set color of watermark
                    pdfData.SetColorFill(BaseColor.BLACK);
                    //indicates start of writing of text
                    pdfData.BeginText();
                    //show text as per position and rotation
                    pdfData.ShowTextAligned(Element.ALIGN_BOTTOM, strWaterMark, pageRectangle.Width - (pageRectangle.Width - 20), pageRectangle.Height - (pageRectangle.Height - 35), 0);
                    //call endText to invalid font set
                    pdfData.EndText();
                }
                //close stamper and output filestream
                pdfStamper.Close();

                return stream.GetBuffer();
            }
        }
        public byte[] AddWaterMarkToImage(byte[] byteArr, string strWaterMark, string imageName)
        {
            MemoryStream ms = new MemoryStream(byteArr);
            System.Drawing.Image inputImage = System.Drawing.Image.FromStream(ms);
            string strGUID = Guid.NewGuid().ToString();
            string path = (System.AppDomain.CurrentDomain.BaseDirectory).Replace("\\", "/") + (MDVSession.Current.ImagePath).Replace("~/", "") + "/";


            Bitmap tempBitmap = new Bitmap(inputImage.Width, inputImage.Height);
            // From this bitmap, the graphics can be obtained, because it has the right PixelFormat
            Graphics grphs = Graphics.FromImage(tempBitmap);
            // Draw the original bitmap onto the graphics of the new bitmap
            grphs.DrawImage(inputImage, 0, 0);
            inputImage = (System.Drawing.Image)tempBitmap;

            // Get a graphics context
            Graphics g = Graphics.FromImage(inputImage);

            // Create a solid brush to write the watermark text on the image

            Brush myBrush = new SolidBrush(Color.FromArgb(100, Color.Black));
            int fontsize = (inputImage.Width / strWaterMark.Length);

            if (fontsize > 24)
            {
                fontsize = 19;
            }
            else if (fontsize < 10)
            {
                fontsize = 7;
            }

            System.Drawing.Font font = new System.Drawing.Font(FontFamily.GenericSansSerif, fontsize, System.Drawing.GraphicsUnit.Pixel);
            // Calculate the size of the text
            SizeF sz = g.MeasureString(strWaterMark, font);
            int X;
            int Y;
            // Set the drawing position based on the users
            // selection of placing the text at the bottom or top of the image

            if (inputImage.Width > sz.Width)
            {
                X = (int)(inputImage.Width - sz.Width) / 2;
            }
            else
            {
                X = 10;
            }

            // X = 320;
            Y = (int)(inputImage.Height - sz.Height);

            // draw the water mark text
            g.DrawString(strWaterMark, font, myBrush,
            new Point(X, Y));
            Bitmap bmp = new Bitmap(inputImage);
            bmp.Save(path + strGUID + Path.GetExtension(imageName));
            ImageConverter converter = new ImageConverter();
            byte[] returnByteArr = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
            System.IO.File.Delete(path + strGUID + Path.GetExtension(imageName));
            return returnByteArr;


        }
        #endregion

        #region patientPortal
        public List<PatientPortalSignupModel> LoadPatientPortalSignupReq(string Status, string ProviderId, string PageNumber, string RowsPerPage)
        {
            try
            {
                return new DALPatientPortal().LoadPatientPortalSignupReq(Status, ProviderId, PageNumber, RowsPerPage);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientPortalSignupReq", ex);
                return new List<PatientPortalSignupModel>( 0);
            }
        }

        public string DeletePatientPortalSignupReq(string ids)
        {
            try
            {
                DataTable ids_dt = new DataTable();
             var   COLUMN = new DataColumn();
                COLUMN.ColumnName = "Id";
                COLUMN.DataType = typeof(int);
                ids_dt.Columns.Add(COLUMN);
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    string[] strArry = ids.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        DataRow Dr = ids_dt.NewRow();
                        Dr[0] = strArry[i];
                        ids_dt.Rows.Add(Dr);
                    }
                }
                else
                {
                    DataRow Dr = ids_dt.NewRow();
                    Dr[0] = 0;
                    ids_dt.Rows.Add(Dr);
                }

                return new DALPatientPortal().DeletePatientPortalSignupReq(ids_dt);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeletePatientPortalSignupReq", ex);
                return  ex.Message;
            }
        }

        
       public string ApprovePatientPortalSignupReq(DashBoardModel model)
        {
            try
            {
                DataTable ids_dt = new DataTable();
                var COLUMN = new DataColumn();
                COLUMN.ColumnName = "Id";
                COLUMN.DataType = typeof(int);
                ids_dt.Columns.Add(COLUMN);
                if (!string.IsNullOrWhiteSpace(model.PatientId))
                {
                    string[] strArry = model.PatientId.Split(',');
                    for (int i = 0; i < strArry.Length; i++)
                    {
                        DataRow Dr = ids_dt.NewRow();
                        Dr[0] = strArry[i];
                        ids_dt.Rows.Add(Dr);
                    }
                }
                else
                {
                    DataRow Dr = ids_dt.NewRow();
                    Dr[0] = 0;
                    ids_dt.Rows.Add(Dr);
                }

                try
                {
                    var listPatModel = new DALPatientPortal().ApprovePatientPortalSignupReq(ids_dt, model);
                    foreach (var patModel in listPatModel)
                    {
                        if (!string.IsNullOrEmpty(patModel.EmailAddress))
                        {
                            SendEmailGeneric(patModel.EmailAddress, patModel.FirstName, patModel.lastName, patModel.UserName, patModel.Password);
                        }
                    }
                }
                catch (Exception ex) { };

                return "";

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::ApprovePatientPortalSignupReq", ex);
                return ex.Message;
            }
        }

        public static void SendEmailGeneric(string email, string firstName, string lastName, string username,string password)
        {
            if (email != "")
            {
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient("smtp.office365.com");
                mail.From = new MailAddress(ConfigurationManager.AppSettings["No_Reply_EmailAddress"]);
                mail.To.Add(email); //dr.EmailAddress "  ;
                mail.Subject = "Sovereign Health System Patient Portal";
                LinkedResource inlineLogo;
                string Body = GetFormattedEmailMessageGeneric(out inlineLogo,firstName, lastName, username,  password);
                var view = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
                view.LinkedResources.Add(inlineLogo);
                mail.AlternateViews.Add(view);
                mail.IsBodyHtml = true;
                client.Port = 587;
                client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["No_Reply_EmailAddress"], ConfigurationManager.AppSettings["No_Reply_EmailPassword"]);
                client.EnableSsl = true;

                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    mail.Dispose();
                }
            }
        }
        public static string GetFormattedEmailMessageGeneric(out LinkedResource inlineLogo, string firstName, string lastName, string username, string password)
        {

            StringBuilder str = new StringBuilder();
            const string newLine = "<br/>";

            inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/SHS-nav-logo.png"));
            inlineLogo.ContentId = Guid.NewGuid().ToString();
            string imageTag = string.Format(@"<img alt='Soverign Health System logo' src='cid:{0}'>", inlineLogo.ContentId);

            Func<string, string> makeAnchorTag = link => @"<a href=""" + link + @""">" + link + @"</a>";

            //Header
            str.Append(@"<html><body><div style='font-family: Calibri, Helvetica, sans-serif;'>");
            str.Append(newLine).Append("<table width='100%'><tr>" +
                "<tr style='background-color:#025b9a;'><td style='border:1px solid #025b9a;' colspan='2'></td></tr>" +
                "<td style=text-align:left;'>" + imageTag + "</td>" +
                "<td style='font-family: Calibri, Helvetica, sans-serif;text-align:right;font-size:70px;color:#025b9a;padding-left:70px'>Patient Portal</td>" +
                "</tr></table>");

            str.Append(newLine).Append(newLine).Append(newLine).Append("<b>").Append("Hello " + lastName + ", " + firstName + "!").Append("</b>");
            str.Append(newLine);

            //body
                str.Append(newLine).Append("Welcome to Sovereign Health System Patient Portal. Your request for a Patient Portal account has been approved.");
                str.Append(newLine).Append("Following are the credentials.");
                str.Append(newLine).Append("<b>URL:</b> https://portal.sovereignhealthsystem.com");
                 str.Append(newLine).Append("<b>Username:</b> " + username);
                 str.Append(newLine).Append("<b>Temporary Password:</b> " + password);
            str.Append(newLine); str.Append(newLine); str.Append(newLine);
          
            //Footer
            str.Append(newLine).Append("Sincerely,");
            str.Append(newLine).Append("<b>").Append("Soverign Health System").Append("</b>");
            str.Append(newLine); str.Append(newLine);

            str.Append(newLine).Append("<table width='100%'>" +
                  "<tr><td style='font-family: Calibri, Helvetica, sans-serif;text-align:center;'><b>Soverign Medical Services, Inc.<b></td></tr>" +
                  "<tr><td style='font-family: Calibri, Helvetica, sans-serif;text-align:center;'>85 Harristown Road, Glen Rock, NJ 07452 | 201-834-1100</td></tr>" +
                  "<tr><td style='font-family: Calibri, Helvetica, sans-serif;text-align:center;'>If you have any question feel free to contact us @ mdvsupport@sovms.com</td></tr>" +
                  "</table>");

            str.Append("</div></body></html>");

            return str.ToString();
        }
        #endregion patientPortal

        #region"Patient Login"

        public BLObject<DSPatientPortal> InsertPatientLogin(DSPatientPortal ds)
        {
            try
            {
                ds = new DALPatientPortal().InsertPatientLogin(ds);
                return new BLObject<DSPatientPortal>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientLogin", ex);
                return new BLObject<DSPatientPortal>(null, ex.Message);
            }
        }

        public BLObject<DSPatientPortal> UpdatePatientLogin(DSPatientPortal ds)
        {
            try
            {
                ds = new DALPatientPortal().UpdatePatientLogin(ds);
                return new BLObject<DSPatientPortal>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientLogin", ex);
                return new BLObject<DSPatientPortal>(null, ex.Message);
            }
        }


        public BLObject<DSPatientPortal> InsertPatientRepresentative(DSPatientPortal ds)
        {
            try
            {
                ds = new DALPatientPortal().InsertPatientRepresentative(ds);
                return new BLObject<DSPatientPortal>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertPatientRepresentative", ex);
                return new BLObject<DSPatientPortal>(null, ex.Message);
            }
        }


        public BLObject<DSPatientPortal> UpdatePatientRepresentative(DSPatientPortal ds)
        {
            try
            {
                ds = new DALPatientPortal().UpdatePatientRepresentative(ds);
                return new BLObject<DSPatientPortal>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientRepresentative", ex);
                return new BLObject<DSPatientPortal>(null, ex.Message);
            }
        }

        public BLObject<DSPatientPortal> LoadPatientLogin(long PatientLoginId, string UserName, long PatientId)
        {
            try
            {
                DSPatientPortal ds = new DSPatientPortal();
                ds = new DALPatientPortal().LoadPatientLogin(PatientLoginId, UserName, PatientId);
                return new BLObject<DSPatientPortal>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientLogin", ex);
                return new BLObject<DSPatientPortal>(null, ex.Message);
            }
        }
        public BLObject<DSPatientPortal> LoadPatientRepresentative(long PatRepresentativeId, long PatientId)
        {
            try
            {
                DSPatientPortal ds = new DSPatientPortal();
                ds = new DALPatientPortal().LoadPatientRepresentative(PatRepresentativeId, PatientId);
                return new BLObject<DSPatientPortal>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientRepresentative", ex);
                return new BLObject<DSPatientPortal>(null, ex.Message);
            }
        }

        public BLObject<string> DeletePatRepresentative(string PatRepresentativeId)
        {
            try
            {
                PatRepresentativeId = new DALPatientPortal().DeletePatRepresentative(PatRepresentativeId);
                return new BLObject<string>(PatRepresentativeId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeletePatRepresentative", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }
        #endregion

        #region "Patient Letter"
        #region "Patient Letter Lookups"
        public BLObject<DSClinicalLetterTemplateLookup> GetLetterTemplatesName(long providerId)
        {
            try
            {
                DSClinicalLetterTemplateLookup ds = new DSClinicalLetterTemplateLookup();
                ds = new DALPatientTemplateLetter().GetLetterTemplatesName(providerId);
                return new BLObject<DSClinicalLetterTemplateLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::GetLetterTemplatesName", ex);
                return new BLObject<DSClinicalLetterTemplateLookup>(null, ex.Message);
            }
        }

        #endregion
        #region CRUD for Patient Letter Templates

        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : To loadPatient Letter.
        /// Dat : 09-March-2016
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="isActive"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="categoryId"></param>
        /// <param name="pageNo"></param>
        /// <param name="rpp"></param>
        /// <returns></returns>
        public BLObject<DSLetter> loadPatientTemplateLetter(long Patient_Letter_Id, long patientId, bool isActive, string name, string description, int categoryId, int pageNo, int rpp, string visitDateFrom, string visitDateTo, long NotesId = 0)
        {
            try
            {
                DSLetter ds = new DSLetter();
                ds = new DALPatientTemplateLetter().loadPatientTemplateLetter(Patient_Letter_Id, patientId, isActive, name, description, categoryId, pageNo, rpp, visitDateFrom, visitDateTo, NotesId);
                return new BLObject<DSLetter>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::loadPatientTemplateLetter", ex);
                return new BLObject<DSLetter>(null, ex.Message);
            }
        }


        // Created By:  Muhammad Ahmad Imran
        // Created Date: 08/03/2016
        //OverView: Methods "GetPatientLetterContent" for Get Patient specific Letter content

        public BLObject<DSLetter> GetPatientLetterContent(long patientId, long TemplateLetterId, string Mode, long PatientLetterId, long ProviderId, long LabOrderResultId = 0, string LOINC = null)
        {
            try
            {
                DSLetter ds = new DSLetter();
                ds = new DALPatientTemplateLetter().GetPatientLetterContent(patientId, TemplateLetterId, Mode, PatientLetterId, ProviderId, LabOrderResultId, LOINC);
                return new BLObject<DSLetter>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::GetPatientLetterContent", ex);
                return new BLObject<DSLetter>(null, ex.Message);
            }
        }
        public BLObject<List<PatientLetter>> GetPatientLetters(CommonSearch search)
        {
            try
            {
                List<PatientLetter> lis = new List<PatientLetter>();
                lis = new DALPatientTemplateLetter().NotesPatientLetterSelect(search);
                return new BLObject<List<PatientLetter>>(lis);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::GetPatientLetters", ex);
                return new BLObject<List<PatientLetter>>(null, ex.Message);
            }
        }
        
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : To delete Patient Letter.
        /// Dat : 09-March-2016
        /// </summary>
        /// <param name="patientLetterId"></param>
        /// <returns></returns>
        public BLObject<string> deletePatientLetter(string patientLetterId)
        {
            try
            {
                patientLetterId = new DALPatientTemplateLetter().deletePatientLetter(patientLetterId);
                return new BLObject<string>(patientLetterId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::deletePatientLetter", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : To update Patient Letter.
        /// Dat : 09-March-2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSLetter> UpdatePatientLetter(DSLetter ds)
        {
            try
            {
                ds = new DALPatientTemplateLetter().UpdatePatientLetter(ds);
                return new BLObject<DSLetter>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientLetter", ex);
                return new BLObject<DSLetter>(null, ex.Message);
            }
        }

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 09/03/2016
        //OverView: Methods "InsertPatientLetter" for save Template letter
        public BLObject<DSLetter> InsertPatientLetter(DSLetter ds)
        {
            try
            {
                ds = new DALPatientTemplateLetter().InsertPatientLetter(ds);
                return new BLObject<DSLetter>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::InsertPatientLetter", ex);
                return new BLObject<DSLetter>(null, ex.Message);
            }
        }
        #endregion
        #endregion

        /*
         Author: Muhammad Azhar Shahzad
         Created On: April 16,2016
         Puspoe: Break the glass implementation for patient*/
        #region "Patient Break the Glass"
        public BLObject<DSPatientBreakGlass> loadPatientBreakGlass(long UserId, long PatientId, bool? IsBreakGlassAllow = null)
        {
            try
            {
                DSPatientBreakGlass ds = new DSPatientBreakGlass();
                ds = new DALPatientBreakGlass().loadPatientBreakGlass(UserId, PatientId, IsBreakGlassAllow);

                return new BLObject<DSPatientBreakGlass>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::loadPatientBreakGlass", ex);
                return new BLObject<DSPatientBreakGlass>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the Notes.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatientBreakGlass&gt;.</returns>
        public BLObject<DSPatientBreakGlass> insertPatientBreakGlass(DSPatientBreakGlass ds)
        {
            try
            {
                ds = new DALPatientBreakGlass().insertPatientBreakGlass(ds);
                return new BLObject<DSPatientBreakGlass>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::insertPatientBreakGlass", ex);
                return new BLObject<DSPatientBreakGlass>(null, ex.Message);
            }
        }
        public BLObject<DSPatientBreakGlass> insertPatientBreakGlassReason(DSPatientBreakGlass ds)
        {
            try
            {
                ds = new DALPatientBreakGlass().insertPatientBreakGlassReason(ds);
                return new BLObject<DSPatientBreakGlass>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::insertPatientBreakGlassReason", ex);
                return new BLObject<DSPatientBreakGlass>(null, ex.Message);
            }
        }


        /// <summary>
        /// Updates the Notes.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSPatientBreakGlass&gt;.</returns>
        public BLObject<string> updatePatientBreakGlass(long UserId, long PatientId, bool IsBreakGlassAllow, string ModifiedBy, DateTime ModifiedOn)
        {
            string returnVal = "";


            try
            {
                returnVal = new DALPatientBreakGlass().updatePatientBreakGlass(UserId, PatientId, ModifiedBy, ModifiedOn, IsBreakGlassAllow);
                return new BLObject<string>(returnVal);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::updatePatientBreakGlass", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the Notes.
        /// </summary>
        /// <param name="NoteId">The Notes identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> deletePatientBreakGlass(string PatientBreakGlassId)
        {
            try
            {
                PatientBreakGlassId = new DALPatientBreakGlass().deletePatientBreakGlass(PatientBreakGlassId);
                return new BLObject<string>(PatientBreakGlassId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::deletePatientBreakGlass", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "Recent Patients"
        public BLObject<DSPatient> InsertRecentPatient(DSPatient ds)
        {
            try
            {
                ds = new DALPatient().InsertRecentPatient(ds);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertRecentPatient", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }

        #endregion

        #region PatientPreferences

        public BLObject<DSPatient> UpdatePatientPreferences(DSPatient dsPatient)
        {

            try
            {

                DSPatient ds = new DSPatient();
                ds = new DALPatient().UpdatePatientPreferences(dsPatient);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientPreferences", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        public BLObject<DSPatient> UpdatePatientPreferencesNative(DSPatient dsPatient)
        {

            try
            {

                DSPatient ds = new DSPatient();
                ds = new DALPatient().UpdatePatientPreferencesNative(dsPatient);
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientPreferencesNative", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
        #endregion

        #region Patient Referral
        public BLObject<DSPatientReferral> loadReferral(long NoteId, string IsActive, long ReferralId, long patientId, string procedureName, long providerId, long RefproviderId, string DateFrom, string DateTo, int status, string Pan, string Type, int visits, long patientInsurance, long facilityfrom, long facilityTo, int pageNumber = 1, int rowsPerPage = 2000, string isViewOrder = "", string isPrintOrder = "", string IsDraft = "", string Source = "", long AssigneeId = 0, string StatusReasonIds = "", string Date = "", string Time = "", long ToSpecialityId = 0)
        {
            try
            {
                DSPatientReferral ds = new DSPatientReferral();
                //Start 21-03-2016 Humaira Yousaf
                //Start 15-08-2016 Humaira Yousaf for new parameters
                ds = new DALPatientReferral().loadReferral(NoteId, IsActive, ReferralId, patientId, procedureName, providerId, RefproviderId, DateFrom, DateTo, status, Pan, Type, visits, patientInsurance, facilityfrom, facilityTo, pageNumber, rowsPerPage, isViewOrder, isPrintOrder, IsDraft, Source, AssigneeId, StatusReasonIds, Date, Time, ToSpecialityId);
                //End 15-08-2016 Humaira Yousaf for new parameters
                //End 21-03-2016 Humaira Yousaf

                return new BLObject<DSPatientReferral>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadReferral", ex);
                return new BLObject<DSPatientReferral>(null, ex.Message);
            }
        }

        public BLObject<DSPatientReferral> searchReferral(long NoteId, string IsActive, long patientId, string procedureName, long providerId, long RefproviderId, string DateFrom, string DateTo, int status, string Pan, string Type, int pageNumber = 1, int rowsPerPage = 2000, string isViewOrder = "", string isPrintOrder = "")
        {
            try
            {
                DSPatientReferral ds = new DSPatientReferral();
                //Start 21-03-2016 Humaira Yousaf
                ds = new DALPatientReferral().searchReferral(NoteId, IsActive, patientId, procedureName, providerId, RefproviderId, DateFrom, DateTo, status, Pan, Type, pageNumber, rowsPerPage, isViewOrder, isPrintOrder);
                //End 21-03-2016 Humaira Yousaf

                return new BLObject<DSPatientReferral>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadReferral", ex);
                return new BLObject<DSPatientReferral>(null, ex.Message);
            }
        }

        public BLObject<DSPatientReferral> InsertUpdateReferral(DSPatientReferral ds)
        {
            try
            {
                ds = new DALPatientReferral().InsertUpdateReferral(ds);
                return new BLObject<DSPatientReferral>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::InsertUpdateReferral", ex);
                return new BLObject<DSPatientReferral>(null, ex.Message);
            }
        }

        // Author:  M Ahmad Imran
        // Created Date: 12/05/2016
        //OverView: Method to Delete Referral Problems
        public BLObject<string> DeleteReferralProblems(string ReferralId)
        {
            try
            {
                ReferralId = new DALPatientReferral().deleteReferralProblems(Convert.ToInt64(ReferralId));
                return new BLObject<string>(ReferralId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::DeleteReferralProblems", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        // Author:  M Ahmad Imran
        // Created Date: 12/05/2016
        //OverView: Method to Insert/Update Referral Problems
        public BLObject<DSPatientReferral> InsertReferralProblems(DSPatientReferral ds)
        {
            try
            {
                ds = new DALPatientReferral().insertReferralProblems(ds);
                return new BLObject<DSPatientReferral>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::InsertReferralProblems", ex);
                return new BLObject<DSPatientReferral>(null, ex.Message);
            }
        }

        // Author:  M Ahmad Imran
        // Created Date: 13/05/2016
        //OverView: Method to Load Referral Procedure
        public BLObject<DSPatientReferral> LoadReferralProcedure(long ReferralId, long ReferralProcedureId, long patientId, long ProviderId, string pageNumber, string rowsPerPage)
        {
            try
            {
                DSPatientReferral ds = new DSPatientReferral();
                ds = new DALPatientReferral().LoadReferralProcedure(ReferralId, ReferralProcedureId, patientId, ProviderId, pageNumber, rowsPerPage);
                return new BLObject<DSPatientReferral>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::LoadReferralProcedure", ex);
                return new BLObject<DSPatientReferral>(null, ex.Message);
            }
        }


        // Author:  M Ahmad Imran
        // Created Date: 13/05/2016
        //OverView: Method to Insert/Update Referral Procedure
        public BLObject<DSPatientReferral> insertUpdateReferralProcedure(DSPatientReferral ds)
        {
            try
            {
                ds = new DALPatientReferral().insertUpdateReferralProcedure(ds);
                return new BLObject<DSPatientReferral>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::insertUpdateReferralProcedure", ex);
                return new BLObject<DSPatientReferral>(null, ex.Message);
            }
        }

        // Author:  M Ahmad Imran
        // Created Date: 16/03/2016
        //OverView: Method to Load Referral Problems
        public BLObject<DSPatientReferral> LoadReferralProblems(long ReferralId, long patientId, int pageNumber, int rowsPerPage)
        {
            try
            {
                DSPatientReferral ds = new DSPatientReferral();
                ds = new DALPatientReferral().loadReferralProblems(0, ReferralId, patientId, pageNumber, rowsPerPage);
                return new BLObject<DSPatientReferral>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::LoadReferralProblems", ex);
                return new BLObject<DSPatientReferral>(null, ex.Message);
            }
        }

        public BLObject<string> deleteReferralProcedure(string ReferralProcedureId)
        {
            try
            {
                ReferralProcedureId = new DALPatientReferral().deleteReferralProcedure(Convert.ToInt64(ReferralProcedureId));
                return new BLObject<string>(ReferralProcedureId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::deleteReferralProcedure", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSProblemLists> LoadProblemLists(long patientId, int pageNumber = 1, int rowsPerPage = 1000)
        {
            try
            {
                DSProblemLists ds = new DSProblemLists();
                ds = new DALPatientReferral().LoadProblemLists(patientId, pageNumber, rowsPerPage);

                return new BLObject<DSProblemLists>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::LoadProblemListsOp", ex);
                return new BLObject<DSProblemLists>(null, ex.Message);
            }
        }

        public BLObject<string> deleteReferral(string ReferralIds)
        {
            try
            {
                ReferralIds = new DALPatientReferral().deleteReferral(ReferralIds);
                return new BLObject<string>(ReferralIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::deleteReferral", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        //Start Notes Work
        public BLObject<string> detachReferralFromNotes(string ReferralId, long NotesId)
        {
            try
            {
                var msgReferralId = new DALPatientReferral().detachReferralsFromNotes(ReferralId, NotesId);
                return new BLObject<string>(msgReferralId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::detachReferralFromNotes", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSPatientReferral> attachReferralListWithNotes(string ReferralId, long NotesId)
        {
            try
            {
                DSPatientReferral ds = new DSPatientReferral();
                ds = new DALPatientReferral().attachReferralWithNotes(ReferralId, NotesId);
                return new BLObject<DSPatientReferral>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::attachReferralListWithNotes", ex);
                return new BLObject<DSPatientReferral>(null, ex.Message);
            }
        }

        public BLObject<DSPatientReferral> getLatestReferralByPatientId(long PatientId, long ProviderId)
        {
            try
            {
                DSPatientReferral ds = new DSPatientReferral();
                ds = new DALPatientReferral().getLatestReferralByPatientId(PatientId, ProviderId);

                return new BLObject<DSPatientReferral>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::getLatestReferralByPatientId", ex);
                return new BLObject<DSPatientReferral>(null, ex.Message);
            }
        }

        #endregion



        #region "Referral For Notes Soap Text
        public BLObject<DSPatientReferral> loadReferralForSoap(string ProblemListId, long PatientId, long ProviderId)
        {
            try
            {
                DSPatientReferral ds = new DSPatientReferral();
                ds = new DALPatientReferral().loadReferralForSoap(ProblemListId, PatientId, ProviderId);
                return new BLObject<DSPatientReferral>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadReferralForSoap", ex);
                return new BLObject<DSPatientReferral>(null, ex.Message);
            }
        }
        #endregion
        //End Notes Work
        private static PdfPCell AddFacilityCellText(string text, iTextSharp.text.Font bodyFont)
        {
            Paragraph facilityParagraph = new Paragraph(text, bodyFont);
            facilityParagraph.Alignment = Element.ALIGN_RIGHT;
            PdfPCell facilityCell = new PdfPCell();
            facilityCell.AddElement(facilityParagraph);
            facilityCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            facilityCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            return facilityCell;
        }
        /// <summary>
        /// Module Name: previewReferral
        /// Author: M Ahmad Imran
        /// Created Date: 17-05-2016
        /// Description: Creates PDF to view Referral
        /// </summary>
        /// <param name="consultationOrderId" type="long">ReferralId</param>
        /// <param name="PatientId" type="long">PatientId</param>
        public BLObject<byte[]> previewReferral(long ReferralId, long patientId)
        {
            try
            {
                //loadReferral("", ReferralId, 0, "", 0, 0, "", "", 0, "", "", 1, 1);
                DSPatientReferral dsReferral = new DSPatientReferral();
                //Start 21-03-2016 Humaira Yousaf
                //Start 15-08-2016 Humaira Yousaf for new parameters
                dsReferral = new DALPatientReferral().loadReferral(0, "", ReferralId, patientId, "", 0, 0, "", "", 0, "", "", 0, 0, 0, 0, 1, 1, "", "");
                //End 15-08-2016 Humaira Yousaf for new parameters
                dsReferral.Merge(new DALPatient().FillPatient(patientId, "", ""));

                dsReferral.Merge(new DALPatientReferral().loadReferralProblems(0, ReferralId, patientId, 1, 2000));
                dsReferral.Merge(new DALPatientReferral().LoadReferralProcedure(ReferralId, 0, patientId, 0, "", ""));

                byte[] newByteArr = null;
                string referralType = string.Empty;
                using (MemoryStream stream_ = new MemoryStream())
                {
                    // Heading Font Style
                    var fontColour = new BaseColor(102, 178, 255);
                    iTextSharp.text.Font patientNameFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, fontColour);
                    iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                    iTextSharp.text.Font componentHeadingFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, fontColour);
                    iTextSharp.text.Font componentHeaderFont = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
                    iTextSharp.text.Font componentHeaderFontWithBlue = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, fontColour);
                    DSPatient dsPatient = new DSPatient();

                    #region Report Header
                    bool IsFooterExist = false;
                    bool isTemplateFound = false;
                    string FooterGeneratedBy = "";
                    PdfPTable ReportHeaderTable = new PdfPTable(2);
                    //  PdfPTable patientTable = new PdfPTable(2);
                    bool IsReportHeaderApplied = false;
                    #region Report Header Data setting to table
                    try
                    {
                        DSReportHeader.ReportHeaderTagsDataTable dtReportHeaderTags;

                        var dsReportHeader = new MDVision.DataAccess.DAL.ReportHeader.DALReportHeader().getReportHeaderTagsValue(patientId, 0, -1, "Referrals");
                        IsReportHeaderApplied = dsReportHeader.ReportHeaderTags.Count > 0;
                        if (IsReportHeaderApplied &&
                                (
                                    !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["PatientText"].ToString()) ||
                                    !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["ProviderText"].ToString()) ||
                                    !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["PracticeText"].ToString()) ||
                                    !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["HeaderLogo"].ToString())
                                )
                            )
                        {
                            isTemplateFound = true;
                        }
                        else
                        {
                            IsReportHeaderApplied = false;
                            IsFooterExist = false;
                        }

                        //------------------------------------  DSReportHeader.ReportHeaderTags dtReportHeaderTags =

                        if (isTemplateFound)
                        {
                            dtReportHeaderTags = dsReportHeader.ReportHeaderTags;
                            // return new BLObject<DSReportHeader>(dsReportHeader);
                            DSReportHeader.ReportHeaderTagsRow drReportHeader = (DSReportHeader.ReportHeaderTagsRow)dtReportHeaderTags.Rows[0];
                            ReportHeaderTable.TotalWidth = 575f;
                            ReportHeaderTable.LockedWidth = true;
                            ReportHeaderTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;


                            //LineSeparator line1 = new LineSeparator(1f, 100f, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -1);
                            //line1.Offset = 1;
                            //pdfDocument.Add(new Chunk(line1));

                            IsFooterExist = drReportHeader.Field<string>("FooterText") != null;
                            if (IsFooterExist)
                            {
                                FooterGeneratedBy = drReportHeader.FooterText;
                            }


                            #region Header Logo

                            if (drReportHeader.Field<string>("HeaderLogo") != null && drReportHeader.Field<string>("HeaderLogo") != "")
                            {


                                PdfPTable headerTable = new PdfPTable(1);
                                headerTable.TotalWidth = 575f;
                                headerTable.LockedWidth = true;
                                headerTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;


                                Byte[] buffer = Convert.FromBase64String(drReportHeader.HeaderLogo.Split(new string[] { "base64," }, StringSplitOptions.None)[1]);
                                var memoryStream = new MemoryStream(buffer); //new MemoryStream(buffer, offset, count);
                                System.Drawing.Image newImagej = System.Drawing.Image.FromStream(memoryStream);

                                //   System.Drawing.Imaging.ImageFormat format = new System.Drawing.Imaging.ImageFormat();



                                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(buffer, false);
                                logo.ScalePercent(59f);
                                logo.ScaleAbsoluteHeight(100);
                                logo.ScaleAbsoluteWidth(150);

                                PdfPCell cell1 = new PdfPCell();
                                cell1.AddElement(logo);
                                cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                ReportHeaderTable.AddCell(cell1);
                            }

                            else
                            {
                                PdfPTable EmptyHeaderTable = new PdfPTable(1);
                                EmptyHeaderTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;


                                //   foreach (string PatientColumn in PatientColumns)
                                //  {
                                EmptyHeaderTable.AddCell(new Paragraph("", bodyFont));
                                //   }

                                ReportHeaderTable.AddCell(EmptyHeaderTable);
                            }

                            #endregion

                            #region practice

                            PdfPTable PracticeTable = new PdfPTable(1);
                            PracticeTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            PracticeTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            if (drReportHeader.Field<string>("PracticeText") != null)
                            {
                                string[] PracticeColumns = drReportHeader.PracticeText.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                                foreach (string PracticeColumn in PracticeColumns)
                                {
                                    PracticeTable.AddCell(new Paragraph(PracticeColumn, bodyFont));
                                }

                            }
                            else
                            {
                                PracticeTable.AddCell(new Paragraph("", bodyFont));
                            }


                            ReportHeaderTable.AddCell(PracticeTable);


                            #endregion

                            //LineSeparator line2 = new LineSeparator(1f, 100f, iTextSharp.text.BaseColor.BLUE, iTextSharp.text.Element.ALIGN_CENTER, -1);
                            //line2.Offset = 1;
                            //pdfDocument.Add(new Chunk(line2));


                            #region  Patient

                            PdfPTable PatientTable = new PdfPTable(1);
                            PatientTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                            if (drReportHeader.Field<string>("PatientText") != null)
                            {
                                string[] PatientColumns = drReportHeader.PatientText.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                                foreach (string PatientColumn in PatientColumns)
                                {
                                    PatientTable.AddCell(new Paragraph(PatientColumn.Replace("12:00AM", ""), bodyFont));
                                }

                            }
                            else
                            {
                                PatientTable.AddCell(new Paragraph("", bodyFont));
                            }



                            ReportHeaderTable.AddCell(PatientTable);


                            #endregion


                            #region  Provider

                            PdfPTable ProviderTable = new PdfPTable(1);
                            ProviderTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            ProviderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            if (drReportHeader.Field<string>("ProviderText") != null)
                            {
                                string[] ProviderColumns = drReportHeader.ProviderText.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                                foreach (string ProviderColumn in ProviderColumns)
                                {
                                    ProviderTable.AddCell(new Paragraph(ProviderColumn, bodyFont));
                                }

                            }
                            else
                            {
                                ProviderTable.AddCell(new Paragraph("", bodyFont));
                            }



                            ReportHeaderTable.AddCell(ProviderTable);


                            #endregion

                            LineSeparator line3 = new LineSeparator(1f, 100f, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -1);
                            line3.Offset = 1;
                            //pdfDocument.Add(new Chunk(line3));
                            ReportHeaderTable.DefaultCell.Padding = 4;

                            //pdfDocument.Add(ReportHeaderTable);

                        }
                        else
                        {
                            //#region Patient's Data

                            // Start Append Patient's Data
                            ReportHeaderTable.TotalWidth = 575f;
                            ReportHeaderTable.LockedWidth = true;

                            ReportHeaderTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                            foreach (DSPatient.PatientsRow dr in dsReferral.Tables[dsPatient.Patients.TableName].Rows)
                            {
                                string age = GetAge(MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]));

                                PdfPTable ageTable = new PdfPTable(1);
                                ageTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                ageTable.AddCell(new Paragraph(dr[dsPatient.Patients.FullNameColumn.ColumnName].ToString(), patientNameFont));
                                ageTable.AddCell(new Paragraph(dr[dsPatient.Patients.AccountNumberColumn.ColumnName].ToString(), bodyFont));
                                ageTable.AddCell(new Paragraph(string.Format("{0} {1}", age.Trim(), dr[dsPatient.Patients.GenderColumn.ColumnName].ToString()), bodyFont));
                                ageTable.AddCell(new Paragraph(dr[dsPatient.Patients.Address1Column.ColumnName].ToString(), bodyFont));
                                ageTable.AddCell(new Paragraph(dr[dsPatient.Patients.CityColumn.ColumnName].ToString() + ", " + dr[dsPatient.Patients.StateColumn.ColumnName].ToString() + ", " + dr[dsPatient.Patients.ZIPCodeColumn.ColumnName].ToString() + (dr[dsPatient.Patients.ZIPCodeExtColumn.ColumnName].ToString() != "" ? "-" + dr[dsPatient.Patients.ZIPCodeExtColumn.ColumnName].ToString() : ""), bodyFont));
                                ageTable.AddCell(new Paragraph("Home Phone:" + dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName].ToString(), bodyFont));
                                ageTable.AddCell(new Paragraph("Cell Phone:" + dr[dsPatient.Patients.CellNoColumn.ColumnName].ToString(), bodyFont));
                                ageTable.AddCell(new Paragraph("Email:" + dr[dsPatient.Patients.EmailAddressColumn.ColumnName].ToString(), bodyFont));
                                ageTable.AddCell(new Paragraph(string.Empty));

                                PdfPTable FacilityTable = new PdfPTable(1);
                                FacilityTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                long FacilityId = MDVUtility.ToLong(dr[dsPatient.Patients.FacilityIdColumn.ColumnName].ToString());
                                DSProfile dsFacility = new DALFacility().LoadFacility(FacilityId, "", "", "", "", "");


                                if (MDVUtility.ToInt32(dsFacility.Facility.Rows.Count) > 0)
                                {
                                    DSProfile.FacilityRow FacilityRow = (DSProfile.FacilityRow)dsFacility.Facility.Rows[0];
                                    FacilityTable.AddCell(BLLPatient.AddFacilityCellText(FacilityRow.ShortName.ToString(), patientNameFont));
                                    FacilityTable.AddCell(BLLPatient.AddFacilityCellText(FacilityRow.Address, bodyFont));
                                    FacilityTable.AddCell(BLLPatient.AddFacilityCellText(FacilityRow.City + ", " + FacilityRow.State + ", " + FacilityRow.ZIPCode + (FacilityRow.ZIPCodeExt != "" ? "-" + FacilityRow.ZIPCodeExt : ""), bodyFont));
                                    FacilityTable.AddCell(BLLPatient.AddFacilityCellText("Phone: " + FacilityRow.PhoneNo, bodyFont));
                                    FacilityTable.AddCell(BLLPatient.AddFacilityCellText("Fax: " + FacilityRow.Fax, bodyFont));
                                }

                                ReportHeaderTable.AddCell(ageTable);


                                PdfPCell cell = new PdfPCell();
                                cell.AddElement(FacilityTable);
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                ReportHeaderTable.AddCell(cell);

                                ReportHeaderTable.DefaultCell.Padding = 4;
                                //pdfDocument.Add(Patient_Name);
                                //pdfDocument.Add(Patient_Body1);
                                IsReportHeaderApplied = true;

                            }
                            // End Append Patient's Data
                        }

                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLAdminClinical::getReportHeaderTagsValue", ex);
                        // return new BLObject<DSReportHeader>(null, ex.Message);
                    }
                    #endregion


                    float bottomMargin = 20;
                    float topMargin = 20;
                    if (IsReportHeaderApplied)
                    {
                        topMargin = ReportHeaderTable.CalculateHeights() + 30;
                        bottomMargin = 50;
                    }
                    Document pdfDocument = new Document(PageSize.LETTER, 20, 20, topMargin, bottomMargin);
                    MDVUtility.PDFCreator pdf = new MDVUtility.PDFCreator(ref pdfDocument, stream_, false, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Requisition");
                    pdf.Writer.PageEvent = new MDVision.Common.Utilities.MDVUtility.AddFooterHeader(ReportHeaderTable, IsFooterExist, FooterGeneratedBy, null);

                    #endregion

                    pdf.Document.Open();

                    //if (isTemplateFound == false)
                    //{
                    //    pdfDocument.Add(patientTable);
                    //}

                    //Begin 28-03-2016 Edit By Humaira Yousaf Bug# EMR-586
                    DataRow ReferralRow = dsReferral.Tables[dsReferral.Referrals.TableName].Rows[0];


                    Paragraph req_Heading = new Paragraph(ReferralRow["Type"] + " Referral".ToString(), patientNameFont);
                    //End 28-03-2016 Edit By Humaira Yousaf Bug# EMR-586
                    LineSeparator line = new LineSeparator(1f, 100f, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -1);
                    line.Offset = 1;
                    pdfDocument.Add(new Chunk(line));
                    req_Heading.SpacingAfter = -12;
                    req_Heading.SpacingBefore = -5;
                    pdfDocument.Add(req_Heading);
                    Chunk c = new Chunk(line);

                    pdfDocument.Add(new Chunk(line));
                    pdfDocument.Add(Chunk.NEWLINE);
                    #region Referral Data

                    // Start Append Order Information

                    if (dsReferral.Tables[dsReferral.Referrals.TableName].Rows.Count > 0)
                    {
                        DSPatientReferral.ReferralsRow dr = (DSPatientReferral.ReferralsRow)dsReferral.Tables[dsReferral.Referrals.TableName].Rows[0];
                        referralType = MDVUtility.ToStr(dr[dsReferral.Referrals.TypeColumn.ColumnName]);

                        if (referralType == "Incoming")
                        {

                            Paragraph order_Heading = new Paragraph("Referral Information \n".ToString(), componentHeadingFont);
                            order_Heading.SpacingBefore = order_Heading.SpacingAfter = 5;

                            pdfDocument.Add(order_Heading);

                            PdfPTable orderTable = new PdfPTable(4);
                            float[] widths = new float[] { 8f, 8f, 8f, 8f };
                            orderTable.SetWidths(widths);
                            orderTable.TotalWidth = 575f;
                            orderTable.LockedWidth = true;
                            orderTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                            orderTable.AddCell(new Paragraph("Date:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dr[dsReferral.Referrals.DateColumn.ColumnName]).ToShortDateString() + ' ' + MDVUtility.ToStr(dr[dsReferral.Referrals.TimeColumn.ColumnName]), bodyFont));
                            orderTable.AddCell(string.Empty);
                            orderTable.AddCell(string.Empty);
                            orderTable.AddCell(new Paragraph("Category:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.TypeColumn.ColumnName]), bodyFont));

                            orderTable.AddCell(string.Empty);
                            orderTable.AddCell(string.Empty);

                            //orderTable.AddCell(new Paragraph("Status:", componentHeaderFont));
                            //int status = MDVUtility.ToInt32(dr[dsReferral.Referrals.StatusColumn.ColumnName]);
                            //string statusName = string.Empty;

                            //if (status == 1)
                            //    statusName = "Not started";
                            //else if (status == 2)
                            //    statusName = "In progress";
                            //else
                            //    statusName = "Completed";

                            //orderTable.AddCell(new Paragraph(statusName, bodyFont));

                            orderTable.AddCell(new Paragraph("Visit Type:", componentHeaderFont));
                            string visitType = GetVisitTypeName(MDVUtility.ToInt32(dr[dsReferral.Referrals.VisitsColumn.ColumnName]));
                            orderTable.AddCell(new Paragraph(visitType, bodyFont));

                            orderTable.AddCell(new Paragraph("Reason:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.ReasonColumn.ColumnName]), bodyFont));

                            orderTable.AddCell(new Paragraph("Referral From:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderNameColumn.ColumnName]) + " MD", bodyFont));

                            orderTable.AddCell(new Paragraph("Referral To:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.ProviderNameColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsReferral.Referrals.ProviderQualificationColumn.ColumnName]), bodyFont));

                            // Referral From Phone
                            if (MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderPhoneNoColumn.ColumnName]) != "")
                            {
                                orderTable.AddCell(new Paragraph("Phone No:", componentHeaderFont));
                                orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderPhoneNoColumn.ColumnName]), bodyFont));

                            }

                            orderTable.AddCell(new Paragraph("Insurance Plan:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.PatientInsuranceNameColumn.ColumnName]), bodyFont));

                            if (MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderAddressColumn.ColumnName]) != "")
                            {
                                // Referral From Phone
                                orderTable.AddCell(new Paragraph("Address:", componentHeaderFont));
                                orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderAddressColumn.ColumnName]), bodyFont));
                            }
                            orderTable.AddCell(new Paragraph("Assigned To:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.AssigneeNameColumn.ColumnName]), bodyFont));

                            pdfDocument.Add(orderTable);
                        }
                        else
                        {

                            Paragraph order_Heading = new Paragraph("Referral Information \n".ToString(), componentHeadingFont);
                            order_Heading.SpacingBefore = order_Heading.SpacingAfter = 5;

                            pdfDocument.Add(order_Heading);

                            PdfPTable orderTable = new PdfPTable(4);
                            float[] widths = new float[] { 8f, 8f, 8f, 8f };
                            orderTable.SetWidths(widths);
                            orderTable.TotalWidth = 575f;
                            orderTable.LockedWidth = true;
                            orderTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                            orderTable.AddCell(new Paragraph("Date:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dr[dsReferral.Referrals.DateColumn.ColumnName]).ToShortDateString() + ' ' + MDVUtility.ToStr(dr[dsReferral.Referrals.TimeColumn.ColumnName]), bodyFont));
                            orderTable.AddCell(string.Empty);
                            orderTable.AddCell(string.Empty);
                            orderTable.AddCell(new Paragraph("Category:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.TypeColumn.ColumnName]), bodyFont));
                            orderTable.AddCell(string.Empty);
                            orderTable.AddCell(string.Empty);
                            orderTable.AddCell(new Phrase("Visit Type:", componentHeaderFont));
                            string visitType = GetVisitTypeName(MDVUtility.ToInt32(dr[dsReferral.Referrals.VisitsColumn.ColumnName]));
                            orderTable.AddCell(new Paragraph(visitType, bodyFont));

                            orderTable.AddCell(new Paragraph("Reason:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.ReasonColumn.ColumnName]), bodyFont));

                            orderTable.AddCell(new Paragraph("Referral To:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderNameColumn.ColumnName]) + " MD", bodyFont));
                            // Referral From Phone
                         

          
                         
                            orderTable.AddCell(new Paragraph("Referral From:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.ProviderNameColumn.ColumnName]) + " " + MDVUtility.ToStr(dr[dsReferral.Referrals.ProviderQualificationColumn.ColumnName]), bodyFont));
                            if (MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderPhoneNoColumn.ColumnName]) != "")
                            {
                                orderTable.AddCell(new Paragraph("Phone No:", componentHeaderFont));
                                orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderPhoneNoColumn.ColumnName]), bodyFont));
                                orderTable.AddCell(string.Empty);
                                orderTable.AddCell(string.Empty); //spaces for address
                            }
                            if (MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderAddressColumn.ColumnName]) != "")
                            {

                                // Referral From Phone
                                orderTable.AddCell(new Paragraph("Address:", componentHeaderFont));
                                orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderAddressColumn.ColumnName]), bodyFont));
                              
                            }
                            if (MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityFromNameColumn.ColumnName]) != "" || MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityToNameColumn.ColumnName]) != "")
                            {
                                if (MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityToNameColumn.ColumnName]) != "")
                                {
                                    orderTable.AddCell(new Paragraph("Facility To:", componentHeaderFont));
                                    orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityToNameColumn.ColumnName]), bodyFont));
                                }
                                else
                                {
                                    orderTable.AddCell(string.Empty);
                                    orderTable.AddCell(string.Empty);
                                }

                                if (MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityFromNameColumn.ColumnName]) != "")
                                {
                                    orderTable.AddCell(new Paragraph("Facility From:", componentHeaderFont));
                                    orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityFromNameColumn.ColumnName]), bodyFont));
                                }
                                else
                                {
                                    orderTable.AddCell(string.Empty);
                                    orderTable.AddCell(string.Empty);
                                }
                            }
                            if (MDVUtility.ToStr(dr[dsReferral.Referrals.ToSpecialtyNameColumn.ColumnName]) != "")
                            {
                                orderTable.AddCell(new Paragraph("Referral To Specialty:", componentHeaderFont));
                                orderTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.Referrals.ToSpecialtyNameColumn.ColumnName]), bodyFont));
                                orderTable.AddCell(string.Empty);
                                orderTable.AddCell(string.Empty);
                            }
                            else
                            {
                                orderTable.AddCell(string.Empty);
                                orderTable.AddCell(string.Empty);
                                orderTable.AddCell(string.Empty);
                                orderTable.AddCell(string.Empty);
                            }
                            pdfDocument.Add(orderTable);
                        }
                    }
                    else
                    {

                        Paragraph order_Heading = new Paragraph("Referral Information \n".ToString(), componentHeadingFont);
                        order_Heading.SpacingBefore = order_Heading.SpacingAfter = 5;
                        pdfDocument.Add(order_Heading);

                        PdfPTable orderTable = new PdfPTable(5);
                        float[] widths = new float[] { 1f, 8f, 8f, 8f, 8f };
                        orderTable.SetWidths(widths);
                        orderTable.TotalWidth = 575f;
                        orderTable.LockedWidth = true;
                        orderTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        orderTable.AddCell(string.Empty);
                        pdfDocument.Add(orderTable);
                        Paragraph noOrder = new Paragraph("No Referral Found".ToString());
                        noOrder.Alignment = Element.ALIGN_CENTER;
                        pdfDocument.Add(noOrder);
                    }
                    // End Append Order Information

                    #endregion

                    #region Referral Procedure
                    if (referralType == "Outgoing")
                    {
                        // Start Append Test Information
                        if (dsReferral.Tables[dsReferral.ReferralProcedure.TableName].Rows.Count > 0)
                        {
                            //Begin 28-03-2016 Edit By Humaira Yousaf Bug# EMR-586
                            Paragraph test_Heading = new Paragraph("Associated Procedures \n".ToString(), componentHeadingFont);
                            //End 28-03-2016 Edit By Humaira Yousaf Bug# EMR-586
                            test_Heading.SpacingBefore = test_Heading.SpacingAfter = 5;
                            pdfDocument.Add(test_Heading);

                            PdfPTable testTable = new PdfPTable(4);
                            float[] widths = new float[] { 12f, 4f, 4f, 2f };
                            testTable.SetWidths(widths);
                            testTable.TotalWidth = 575f;
                            testTable.LockedWidth = true;
                            testTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;


                            testTable.AddCell(new Paragraph("Procedure", componentHeaderFontWithBlue));
                            testTable.AddCell(string.Empty);
                            testTable.AddCell(string.Empty);
                            testTable.AddCell(new Paragraph("Urgency", componentHeaderFontWithBlue));

                            foreach (DSPatientReferral.ReferralProcedureRow dr in dsReferral.Tables[dsReferral.ReferralProcedure.TableName].Rows)
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr(MDVUtility.ToStr(dr[dsReferral.ReferralProcedure.CPTCodeColumn.ColumnName]) != "" ? (dr[dsReferral.ReferralProcedure.CPTCodeColumn.ColumnName] + " " + dr[dsReferral.ReferralProcedure.CPTCodeDescriptionColumn.ColumnName]) : dr[dsReferral.ReferralProcedure.CPTCodeDescriptionColumn.ColumnName]), bodyFont));
                                testTable.AddCell(string.Empty);
                                testTable.AddCell(string.Empty);
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr(dr[dsReferral.ReferralProcedure.UrgencyNameColumn.ColumnName]), bodyFont));
                            }
                            pdfDocument.Add(testTable);
                        }
                        else
                        {
                            //Begin 28-03-2016 Edit By Humaira Yousaf Bug# EMR-586
                            Paragraph test_Heading = new Paragraph("Associated Procedures \n".ToString(), componentHeadingFont);
                            //End 28-03-2016 Edit By Humaira Yousaf Bug# EMR-586
                            test_Heading.SpacingBefore = test_Heading.SpacingAfter = 5;
                            pdfDocument.Add(test_Heading);
                            PdfPTable testTable = new PdfPTable(5);
                            float[] widths = new float[] { 8f, 8f, 8f, 8f, 8f };
                            testTable.SetWidths(widths);
                            testTable.TotalWidth = 575f;
                            testTable.LockedWidth = true;
                            testTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            testTable.AddCell(string.Empty);
                            pdfDocument.Add(testTable);
                            Paragraph noTest = new Paragraph("No Referral Procedure Found".ToString());
                            noTest.Alignment = Element.ALIGN_CENTER;
                            pdfDocument.Add(noTest);
                        }
                        // End Append Associated Problems

                        #endregion

                        #region Problem List

                        // Start Append Associated Problems
                        if (dsReferral.Tables[dsReferral.ReferralProblems.TableName].Rows.Count > 0)
                        {

                            Paragraph problems_Heading = new Paragraph("Associated Problems \n".ToString(), componentHeadingFont);
                            problems_Heading.SpacingBefore = problems_Heading.SpacingAfter = 5;
                            pdfDocument.Add(problems_Heading);

                            PdfPTable problemsTable = new PdfPTable(1);
                            float[] widths = new float[] { 12f };
                            problemsTable.SetWidths(widths);
                            problemsTable.TotalWidth = 575f;
                            problemsTable.LockedWidth = true;
                            problemsTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            problemsTable.AddCell(string.Empty);

                            foreach (DSPatientReferral.ReferralProblemsRow dr in dsReferral.Tables[dsReferral.ReferralProblems.TableName].Rows)
                            {

                                problemsTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsReferral.ReferralProblems.ProblemNameColumn.ColumnName])), bodyFont));
                            }
                            pdfDocument.Add(problemsTable);
                        }
                        else
                        {

                            Paragraph problems_Heading = new Paragraph("Associated Problems \n".ToString(), componentHeadingFont);
                            problems_Heading.SpacingBefore = problems_Heading.SpacingAfter = 5;
                            pdfDocument.Add(problems_Heading);
                            PdfPTable problemsTable = new PdfPTable(5);
                            float[] widths = new float[] { 1f, 8f, 8f, 8f, 8f };
                            problemsTable.SetWidths(widths);
                            problemsTable.TotalWidth = 575f;
                            problemsTable.LockedWidth = true;
                            problemsTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            problemsTable.AddCell(string.Empty);
                            pdfDocument.Add(problemsTable);
                            Paragraph noProblems = new Paragraph("No Associated Problems Found".ToString());
                            noProblems.Alignment = Element.ALIGN_CENTER;
                            pdfDocument.Add(noProblems);
                        }
                        // End Append Associated Problems

                        #endregion

                    }


                    pdfDocument.Add(Chunk.NEWLINE);
                    Paragraph comments_Heading = new Paragraph("Comments \n".ToString(), componentHeadingFont);
                    comments_Heading.SpacingBefore = comments_Heading.SpacingAfter = 5;
                    pdfDocument.Add(comments_Heading);
                    Paragraph comments = new Paragraph(MDVUtility.ToStr(dsReferral.Tables[dsReferral.Referrals.TableName].Rows[0][dsReferral.Referrals.CommentsColumn.ColumnName]), bodyFont);
                    comments.Alignment = Element.ALIGN_LEFT;
                    pdfDocument.Add(comments);

                    pdfDocument.Add(Chunk.NEWLINE);
                    pdfDocument.Add(Chunk.NEWLINE);

                    pdf.Document.Close();
                    pdf.Writer.Close();
                    pdfDocument.Close();
                    // adding paging label in footer
                    MemoryStream stream = new MemoryStream(stream_.ToArray());

                    PdfReader npdf = new PdfReader(stream);
                    MemoryStream outstream = new MemoryStream();
                    bottomMargin = bottomMargin > 30 ? bottomMargin - 20 : 30;
                    using (PdfStamper stamper = new PdfStamper(npdf, outstream, '\0', true))
                    {
                        stamper.Writer.CloseStream = false;
                        int PageCount = npdf.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                        {
                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, new Paragraph(String.Format("Page {0} of {1}", i, PageCount), bodyFont), 555, bottomMargin, 0);
                        }
                    }
                    newByteArr = outstream.GetBuffer();
                    //  newByteArr = stream_.GetBuffer();
                }

                return new BLObject<byte[]>(newByteArr);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::previewReferral", ex);
                return new BLObject<byte[]>(null, ex.Message);
            }
        }

        private string GetVisitTypeName(int visitId)
        {
            string typeName = string.Empty;
            switch (visitId)
            {
                case 1:
                    typeName = "Follow Up";
                    break;
                case 2:
                    typeName = "New Patient";
                    break;
                case 3:
                    typeName = "Physical";
                    break;
                case 4:
                    typeName = "Surgical Clearance";
                    break;
                case 5:
                    typeName = "Consult";
                    break;
                case 6:
                    typeName = "Office Visit";
                    break;
                case 7:
                    typeName = "Immunization";
                    break;
                case 8:
                    typeName = "Injection";
                    break;
                case 9:
                    typeName = "Bloodwork";
                    break;
                case 10:
                    typeName = "Ultrasound";
                    break;
                case 11:
                    typeName = "Procedure";
                    break;
                case 12:
                    typeName = "Surgery";
                    break;
                case 13:
                    typeName = "Infusion";
                    break;
                case 14:
                    typeName = "Test";
                    break;
                default:
                    break;
            }

            return typeName;
        }
        private string GetAge(DateTime birthday)
        {
            DateTime today = DateTime.Now;
            int days = today.Day - birthday.Day;
            if (days < 0)
            {
                today = today.AddMonths(-1);
                days += DateTime.DaysInMonth(today.Year, today.Month);
            }
            int months = today.Month - birthday.Month;
            if (months < 0)
            {
                today = today.AddYears(-1);
                months += 12;
            }
            int years = today.Year - birthday.Year;
            //Start 27-01-2016 Muhammad Arshad Bug# EMR-245	Face Sheet in Clinical Module -> Patient Age in PDF
            //string ActAge = string.Format(" {0} Year(s), {1} Month(s), {2} Day(s)  ", years,
            //             months,
            //             days);
            string ActAge = string.Format(" {0} Year(s)  ", years);
            //End 27-01-2016 Muhammad Arshad Bug# EMR-245	Face Sheet in Clinical Module -> Patient Age in PDF
            return (ActAge);
        }

        #region Dashboard Document
        public BLObject<DPatientDoucmnetModel> LoadDashboardDocument(DateTime? DOSFrom, DateTime? DOSTo, int PageNumber = 1, int RowspPage = 1000, string DocPriority = "", string DocAssignToReview = "", string PatientId = "", string Status = "")
        {
            try
            {
                var result = new DALPatientDocument().LoadDashboardDocument(DOSFrom, DOSTo, PageNumber, RowspPage, DocPriority, DocAssignToReview, PatientId, Status, true);
                return new BLObject<DPatientDoucmnetModel>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadDashboardDocument", ex);
                return new BLObject<DPatientDoucmnetModel>(null, ex.Message);
            }
        }
        #endregion

        #region Active Accounts
        public BLObject<PatientPortalAccounts> LoadPatientPortalAccounts(long EntityId, string UserId, DateTime? DOB = null, string PatientId = null, int PageNumber = 1, int RowspPage = 1000)
        {
            try
            {
                var dashboard = new DALDashBoard();
                var result = dashboard.LoadPatientPortalAccounts(EntityId, UserId, DOB, PatientId, PageNumber, RowspPage);
                return new BLObject<PatientPortalAccounts>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientPortalAccounts", ex);
                return new BLObject<PatientPortalAccounts>(null, ex.Message);
            }
        }
        #endregion
        #region Dashboard Patient Changes
        public BLObject<List<DPatientChangeModel>> LoadDashboardPatientChanges(int Month, int Year, string ProfileName, int ProviderId, int PageNumber = 1, int RowspPage = 15)
        {
            try
            {

                var result = new DALDashBoard().LoadDashboardPatientChanges(Month, Year, ProfileName, ProviderId, PageNumber, RowspPage);
                return new BLObject<List<DPatientChangeModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadDashboardPatientChanges", ex);
                return new BLObject<List<DPatientChangeModel>>(null, ex.Message);
            }
        }

        #endregion


        #region NATIVE FUNCTIONS
        public List<PatientLookupModel> LookupMostViewedPatientNative(long EntityId, long UserId)
        {

            List<PatientLookupModel> patientLookupList;
            try
            {
                patientLookupList = new DALPatient(true).LookupMostViewedPatientNative(EntityId, UserId);

                //if (patientLookupList.Count <= 0)
                //{
                //    throw new Exception("No Patient Found");
                //}

                return patientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPatientByName", ex);
                throw ex;
            }
        }
        public List<PatientLookupModel> LookupPatientNative(string Searchstring, long EntityId, long UserId, string IsActive, int PageNumber)
        {

            List<PatientLookupModel> patientLookupList;
            try
            {
                patientLookupList = new DALPatient(true).LookupPatientNative(Searchstring, EntityId, UserId, IsActive, PageNumber);

                //if (patientLookupList.Count <= 0)
                //{
                //    throw new Exception("No Patient Found");
                //}

                return patientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupPatientByName", ex);
                throw ex;
            }
        }
        public PatientDemographicModel GetPatientDemographicsDetailsForCustomForms(long PatientId)
        {
            PatientDemographicModel patientDemographic = null;
            try
            {
                patientDemographic = new DALPatient().GetPatientDemographicsDetailsForCustomForms(PatientId);
                return patientDemographic;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::GetPatientDemographicsDetailsForCustomForms", ex);
                throw ex;
            }

        }
        public List<PatientDemographicModel> GetPatientDemographics(int PatientId, string FormName)
        {
            List<PatientDemographicModel> patientDemographicList;
            try
            {
                patientDemographicList = new DALPatient(true).GetPatientDemographics(PatientId, FormName);

                if (patientDemographicList.Count <= 0)
                {
                    throw new Exception("No Patient Demographic Found");
                }

                if (!string.IsNullOrEmpty(patientDemographicList[0].PatientProfileImagePath))
                {
                    string FileServerPath = System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesPath"];
                    // MDVSession.Current.ImageID = patientDemographicList[0].PatientProfileImagePath + "|" + patientDemographicList[0].PatientProfileThumbnailPath;
                    string imgFullPath = Path.Combine(FileServerPath, patientDemographicList[0].PatientProfileImagePath);
                    if (File.Exists(imgFullPath))
                    {
                        string ImgBase64String = Convert.ToBase64String(File.ReadAllBytes(imgFullPath));
                        patientDemographicList[0].imgPatient = "data:" + patientDemographicList[0].ImageType + ";base64," + ImgBase64String;
                    }
                }




                return patientDemographicList;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::GetPatientDemographics", ex);
                throw ex;
            }

        }
        public List<InsuranceLookupModel> LookupInsurancePlan(string ShortName, long EntityId, string IsActive)
        {

            List<InsuranceLookupModel> patientInsuranceList;
            try
            {
                patientInsuranceList = new DALPatientInsurance(true).LookupInsurancePlan(ShortName, EntityId, IsActive);


                return patientInsuranceList;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupInsurancePlan", ex);
                throw ex;
            }
        }
        public List<PatientLookupModel> LookupMaritalStatusNative()
        {

            List<PatientLookupModel> patientLookupList;
            try
            {
                patientLookupList = new DALPatient(true).LookupMaritalStatusNative();


                return patientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupMaritalStatusNative", ex);
                throw ex;
            }
        }
        public List<PatientLookupModel> LookupEthnicityNative()
        {

            List<PatientLookupModel> patientLookupList;
            try
            {
                patientLookupList = new DALPatient(true).LookupEthnicityNative();


                return patientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupEthnicityNative", ex);
                throw ex;
            }
        }
        public List<PatientLookupModel> LookupRaceNative()
        {

            List<PatientLookupModel> patientLookupList;
            try
            {
                patientLookupList = new DALPatient(true).LookupRaceNative();

                //if (patientLookupList.Count <= 0)
                //{
                //    throw new Exception("No Race Found");
                //}

                return patientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupRaceNative", ex);
                throw ex;
            }
        }
        public List<PatientInsuranceModel> LoadPatientInsuranceNative(long InsuranceId, long PatientId, long EntityId)
        {
            List<PatientInsuranceModel> patientInsuranceList;
            try
            {
                patientInsuranceList = new DALPatientInsurance(true).LoadPatientInsuranceNative(InsuranceId, PatientId, EntityId);

                //if (patientInsuranceList.Count <= 0)
                //{
                //    throw new Exception("No Insurance Found");
                //}

                return patientInsuranceList;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientInsuranceNative", ex);
                throw ex;
            }

        }
        public List<PatientLookupModel> LookupLanguagesNative()
        {

            List<PatientLookupModel> patientLookupList;
            try
            {
                patientLookupList = new DALPatient(true).LookupLanguagesNative();

                return patientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupLanguagesNative", ex);
                throw ex;
            }
        }
        public List<PatientLookupModel> LookupRelationshipNative()
        {

            List<PatientLookupModel> patientLookupList;
            try
            {
                patientLookupList = new DALPatient(true).LookupRelationshipNative();

                return patientLookupList;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupRelationshipNative", ex);
                throw ex;
            }
        }


        public string UpdatePatientDemographicsNative(SavePatientNative model)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();
            dbManager.BeginTransaction();

            try
            {
                string patientId;

                model.patientDemographic_JSON.ModifiedBy = ClientConfiguration.DecryptFrom64(model.UserName);
                model.patientDemographic_JSON.ModifiedOn = DateTime.Now;

                if (!string.IsNullOrEmpty(model.patientDemographic_JSON.strRaceIds))
                {
                    if (model.patientDemographic_JSON.strRaceIds.Length > 0)
                    {
                        model.patientDemographic_JSON.Race = MDVUtility.ToStr(model.patientDemographic_JSON.strRaceIds[0]);
                    }
                }

                //     patientId = new DALPatient(true).updateDemographicsNative(dbManager, model.patientDemographic_JSON);
                patientId = "1";
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
        //public PatientInsurancSave UpdatePatientInsuranceNative(PatientInsurancSave model)
        //{
        //    string ConnectionString = "";
        //    IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
        //    dbManager.Open();
        //    dbManager.BeginTransaction();
        //    PatientInsurancSave patientInsuranceSavemodel = new PatientInsurancSave();
        //    try
        //    {

        //        foreach (PatientInsuranceModel m in model.patientInsurance_JSON)
        //        {
        //            if (MDVUtility.ToInt64(m.InsuranceId) == -1)
        //            {
        //                m.IsActive = true;
        //                m.CreatedBy = ClientConfiguration.DecryptFrom64(model.UserName);
        //                m.CreatedOn = DateTime.Now;
        //                m.ModifiedBy = ClientConfiguration.DecryptFrom64(model.UserName);
        //                m.ModifiedOn = DateTime.Now;
        //                var retModel = new DALPatientInsurance(true).insertPatientInsuranceNative(dbManager, m);
        //                if (retModel != null)
        //                {
        //                    patientInsuranceSavemodel.patientInsurance_JSON.Add(retModel);
        //                }
        //                else
        //                {
        //                    throw new Exception("Insurance not saved.");
        //                }
        //            }
        //            else
        //            {
        //                m.ModifiedBy = ClientConfiguration.DecryptFrom64(model.UserName);
        //                m.ModifiedOn = DateTime.Now;
        //                var retModel = new DALPatientInsurance(true).updatePatientInsuranceNative(dbManager, m);
        //                if (retModel != null)
        //                {
        //                    patientInsuranceSavemodel.patientInsurance_JSON.Add(retModel);
        //                }
        //                else
        //                {
        //                    throw new Exception("Insurance not saved.");
        //                }
        //            }
        //        }

        //        dbManager.CommitTransaction();
        //        return patientInsuranceSavemodel;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLPatient::UpdatePatientInsuranceNative", ex);
        //        dbManager.RollBackTransaction();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        public string InsertUpdatePatientConsentNative(PatientConsentVM model, DSPatient ds)
        {
            string ConnectionString = "";
            IDBManager dbManager = ClientConfiguration.GetCustomerDBManager(ref ConnectionString, "");
            dbManager.Open();
            dbManager.BeginTransaction();
            try
            {
                string patientConsentId = new DALPatient().InsertUpdatePatientConsentNative(model, dbManager);
                ds = new DALPatientDocument().InsertPatientDocumentNative(ds, dbManager);

                dbManager.CommitTransaction();
                return patientConsentId;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertUpdatePatientConsent", ex);
                dbManager.RollBackTransaction();
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public PatientConsentVM LoadPatientConsentNative(long patientId)
        {
            try
            {
                PatientConsentVM model = new DALPatient().LoadPatientConsentNative(patientId);
                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadPatientConsentNative", ex);
                throw ex;
            }
        }
        #endregion


        #region Patient Custom Form

        #region Lookup Custom Form Name
        public BLObject<List<CustomFormLookupModel>> LookupCustomFormName()
        {
            try
            {
                var ds = new DALPatientCustomForm().LookupCustomFormName();
                return new BLObject<List<CustomFormLookupModel>>(ds);


            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupCustomFormName", ex);
                return new BLObject<List<CustomFormLookupModel>>(null, ex.Message);
            }

        }
        #endregion


        public BLObject<string> insertPatientCustomForm(PatientCustomFormModel model)
        {
            try
            {
                string formId;
                formId = new DALPatientCustomForm().InsertPatientCustomForm(model);
                return new BLObject<string>(formId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::insertPatientCustomForm", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<List<PatientCustomFormModel>> GetProviderCPTs(long CustomFormId, long ProviderId)
        {
            try
            {
                var result = new DALPatientCustomForm().GetProviderCPTs(CustomFormId, ProviderId);
                return new BLObject<List<PatientCustomFormModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::GetProviderCPTs", ex);
                return new BLObject<List<PatientCustomFormModel>>(null, ex.Message);
            }
        }

        public BLObject<string> GetPatientDocumentId(string CustomFormId)
        {
            try
            {
                string PatientDocumentId;
                PatientDocumentId = new DALPatientCustomForm().GetPatientDocumentId(CustomFormId);
                return new BLObject<string>(PatientDocumentId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::GetPatientDocumentId", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> updatePatientCustomForm(PatientCustomFormModel model)
        {
            try
            {
                string formId;
                formId = new DALPatientCustomForm().UpdatePatientCustomForm(model);
                return new BLObject<string>(formId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::updatePatientCustomForm", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<List<PatientCustomFormModel>> loadPatientCustomForm(string formName, int? isActive, string patientId, long pageNumber, long rowsPerPage)
        {
            try
            {
                var result = new DALPatientCustomForm().loadPatientCustomForm(formName, isActive, patientId, pageNumber, rowsPerPage);
                return new BLObject<List<PatientCustomFormModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadCustomForm", ex);
                return new BLObject<List<PatientCustomFormModel>>(null, ex.Message);
            }
        }

        public BLObject<string> deletePatientCustomForm(string formId)
        {
            try
            {
                formId = new DALPatientCustomForm().deletePatientCustomForm(formId);
                return new BLObject<string>(formId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::deleteCustomForm", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> activeInactivePatientCustomForm(string formId, string isActive)
        {
            try
            {
                string returnValue;
                returnValue = new DALPatientCustomForm().activeInactivePatientCustomForm(formId, isActive);
                return new BLObject<string>(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::activeInactiveCustomForm", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<List<PatientCustomFormModel>> fillPatientCustomForm(string formId)
        {
            try
            {
                var result = new DALPatientCustomForm().fillPatientCustomForm(formId);
                return new BLObject<List<PatientCustomFormModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::fillCustomForm", ex);
                return new BLObject<List<PatientCustomFormModel>>(null, ex.Message);
            }
        }
        #endregion


        #region " Note Document " 

        public BLObject<List<NoteDocumentModel>> LoadNoteDocument(long NoteDocumentId, int PageNumber = 1, int RowspPage = 15)
        {
            try
            {
                var result = new DALPatientDocument().LoadNoteDocument(NoteDocumentId, PageNumber, RowspPage);
                return new BLObject<List<NoteDocumentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LoadNoteDocument", ex);
                return new BLObject<List<NoteDocumentModel>>(null, ex.Message);
            }
        }

        public BLObject<NoteDocumentModel> InsertNoteDocument(NoteDocumentModel model)
        {
            try
            {
                model = new DALPatientDocument().InsertNoteDocument(model);
                return new BLObject<NoteDocumentModel>(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::InsertNoteDocument", ex);
                throw ex;
            }
        }

        public BLObject<string> UpdateNoteDocument(NoteDocumentModel model)
        {
            try
            {
                var returnVal = new DALPatientDocument().UpdateNoteDocument(model);
                return new BLObject<string>(returnVal);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::UpdateNoteDocument", ex);
                throw ex;
            }
        }

        public BLObject<string> DeleteNoteDocument(long NoteDocumentId, long NoteId, long PatDocId)
        {
            try
            {
                string returnValue = new DALPatientDocument().DeleteNoteDocument(NoteDocumentId, NoteId, PatDocId);
                return new BLObject<string>(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::DeleteNoteDocument", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        public BLObject<DSPatient> FillPatient(long PatientId)
        {
            DSPatient ds = new DSPatient();
            try
            {
                ds = new DALPatient().FillPatient(PatientId, "", "");

                ds.AcceptChanges();
                return new BLObject<DSPatient>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::FillPatient", ex);
                return new BLObject<DSPatient>(null, ex.Message);
            }
        }
    }

}
#endregion
