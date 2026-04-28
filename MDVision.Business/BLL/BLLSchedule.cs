using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.Schedule;
using MDVision.DataAccess.DAL.Appointment;
using MDVision.DataAccess.DAL.Admin;
using System.Data;
using System.Collections;
using MDVision.Model.Dashboard;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Model.Schedule;
using MDVision.Model.Clinical.FollowUp;
using MDVision.DataAccess.DAL.Visits;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Model.PMSSchedule;
using MDVision.Model.Native.Scheduler;
using MDVision.Model.Lookups;
using MDVision.Model.Clinical.Notes;

namespace MDVision.Business.BLL
{
    public class BLLSchedule
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLSchedule"/> class.
        /// </summary>
        public BLLSchedule()
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

        #region "Holidays"

        /// <summary>
        /// Loads the holidays.
        /// </summary>
        /// <param name="HolidayId">The holiday identifier.</param>
        /// <param name="Date">The date.</param>
        /// <param name="Holiday">The holiday.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> LoadHolidays(long HolidayId, string Date, string Holiday, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSScheduleSetup ds = new DSScheduleSetup();
                ds = new DALHolidays().LoadHolidays(HolidayId, Date, Holiday, EntityId, IsActive, PageNumber, RowsPerPage);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadHolidays", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the holidays.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> InsertHolidays(DSScheduleSetup ds)
        {
            try
            {
                ds = new DALHolidays().InsertHolidays(ds);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertHolidays", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the holidays.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> UpdateHolidays(DSScheduleSetup ds)
        {
            try
            {
                ds = new DALHolidays().UpdateHolidays(ds);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateHolidays", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the holidays.
        /// </summary>
        /// <param name="HolidaysId">The holidays identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteHolidays(string HolidaysId)
        {
            try
            {
                HolidaysId = new DALHolidays().DeleteHolidays(HolidaysId);
                return new BLObject<string>(HolidaysId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::DeleteHolidays", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "Reasons"

        /// <summary>
        /// Loads the reasons.
        /// </summary>
        /// <param name="ReasonId">The reason identifier.</param>
        /// <param name="Reason">The reason.</param>
        /// <param name="Description">The description.</param>
        /// <param name="Duration">The duration.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> LoadReasons(long ReasonId, string ShortName, string Description, string IsActive, string EntityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSScheduleSetup ds = new DSScheduleSetup();
                ds = new DALReasons().LoadReasons(ReasonId, ShortName, Description, IsActive, EntityId, PageNumber, RowsPerPage);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadReasons", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the reasons.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> InsertReasons(DSScheduleSetup ds)
        {
            try
            {
                ds = new DALReasons().InsertReasons(ds);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertReasons", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the reasons.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> UpdateReasons(DSScheduleSetup ds)
        {
            try
            {
                ds = new DALReasons().UpdateReasons(ds);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateReasons", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }
        /// <summary>
        /// Lookups the reasons.
        /// </summary>
        /// <returns>BLObject&lt;DSScheduleLookups&gt;.</returns>
        public BLObject<DSScheduleLookups> LookupReasons(string Active, string EntityId = null)
        {
            try
            {
                DSScheduleLookups ds = new DSScheduleLookups();
                ds = new DALReasons().LookupReasons(Active, EntityId);

                return new BLObject<DSScheduleLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupReasons", ex);
                return new BLObject<DSScheduleLookups>(null, ex.Message);
            }

        }

        public BLObject<DSScheduleLookups> LookupReasonsAutoComplete(string Active, string Name = null)
        {
            try
            {
                DSScheduleLookups ds = new DSScheduleLookups();
                ds = new DALBlockHours().LookupReasonsAutoComplete(Active, Name);

                return new BLObject<DSScheduleLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupReasons", ex);
                return new BLObject<DSScheduleLookups>(null, ex.Message);
            }

        }

        public BLObject<string> DeleteReasons(string ReasonsId)
        {
            try
            {
                ReasonsId = new DALReasons().DeleteReasons(ReasonsId);
                return new BLObject<string>(ReasonsId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::DeleteReasons", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "Block Hours"
        /// <summary>
        /// Loads the block hours.
        /// </summary>
        /// <param name="BlockHoursId">The block hours identifier.</param>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <param name="FacilityId">The facility identifier.</param>
        /// <param name="ResourceId">The resource identifier.</param>
        /// <param name="FromDate">From date.</param>
        /// <param name="ToDate">To date.</param>
        /// <param name="FromTime">From time.</param>
        /// <param name="ToTime">To time.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> LoadBlockHours(long BlockHoursId, string Provider, string Resource, string Facility, string FromDate, string ToDate, string FromTime, string ToTime, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSScheduleSetup ds = new DSScheduleSetup();
                ds = new DALBlockHours().LoadBlockHours(BlockHoursId, Provider, Resource, Facility, FromDate, ToDate, FromTime, ToTime, PageNumber, RowsPerPage);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadBlockHours", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the block hours.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> InsertBlockHours(DSScheduleSetup ds)
        {
            try
            {

                ds = new DALBlockHours().InsertBlockHours(ds);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertBlockHours", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }
        /// <summary>
        /// Updates the block hours.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> UpdateBlockHours(DSScheduleSetup ds)
        {
            try
            {
                ds = new DALBlockHours().UpdateBlockHours(ds);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateBlockHours", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }
        /// <summary>
        /// Deletes the block hours.
        /// </summary>
        /// <param name="BlockHoursId">The block hours identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteBlockHours(string BlockHoursId)
        {
            try
            {
                BlockHoursId = new DALBlockHours().DeleteBlockHours(BlockHoursId);
                return new BLObject<string>(BlockHoursId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::DeleteBlockHours", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "Provider Schedule"

        /// <summary>
        /// Inserts the provider schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> InsertProviderSchedule(DSScheduleSetup ds)
        {
            try
            {

                ds = new DALProviderSchedule().InsertProviderSchedule(ds);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertProviderSchedule", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Loads the provider schedule.
        /// </summary>
        /// <param name="ScheduleId">The schedule identifier.</param>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <param name="FacilityId">The facility identifier.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> LoadProviderSchedule(long ScheduleId, string ProviderId, string FacilityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSScheduleSetup ds = new DSScheduleSetup();
                ds = new DALProviderSchedule().LoadProviderSchedule(ScheduleId, ProviderId, FacilityId, PageNumber, RowsPerPage);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadProviderSchedule", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the provider schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> UpdateProviderSchedule(DSScheduleSetup ds)
        {
            try
            {
                ds = new DALProviderSchedule().UpdateProviderSchedule(ds);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateProviderSchedule", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the provider schedule.
        /// </summary>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteProviderSchedule(string ProviderId)
        {
            try
            {
                ProviderId = new DALProviderSchedule().DeleteProviderSchedule(ProviderId);
                return new BLObject<string>(ProviderId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::DeleteProviderSchedule", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleSetup> LoadScheduleReasonDuration(long ScheduleReasonId)
        {
            try
            {
                DSScheduleSetup ds = new DSScheduleSetup();
                ds = new DALProviderSchedule().LoadScheduleReasonDuration(ScheduleReasonId);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadScheduleReasonDuration", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        #endregion

        #region "AppointmentStatus"

        /// <summary>
        /// Loads the appointment status.
        /// </summary>
        /// <param name="AppointmentId">The appointment identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> LoadAppointmentStatus(long AppointmentId, string ShortName, string Description, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSScheduleSetup ds = new DSScheduleSetup();
                ds = new DALAppointmentStatus().LoadAppointmentStatus(AppointmentId, ShortName, Description, IsActive, PageNumber, RowsPerPage);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadAppointmentStatus", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the appointment status.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> InsertAppointmentStatus(DSScheduleSetup ds)
        {
            try
            {
                ds = new DALAppointmentStatus().InsertAppointmentStatus(ds);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertAppointmentStatus", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the appointment status.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>BLObject&lt;DSScheduleSetup&gt;.</returns>
        public BLObject<DSScheduleSetup> UpdateAppointmentStatus(DSScheduleSetup ds)
        {
            try
            {
                ds = new DALAppointmentStatus().UpdateAppointmentStatus(ds);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateAppointmentStatus", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the appointment status.
        /// </summary>
        /// <param name="AppointmentId">The appointment identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteAppointmentStatus(string AppointmentId)
        {
            try
            {
                AppointmentId = new DALAppointmentStatus().DeleteAppointmentStatus(AppointmentId);
                return new BLObject<string>(AppointmentId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::DeleteAppointmentStatus", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleLookups> LookupAppointmentStatus()
        {
            try
            {
                DSScheduleLookups ds = new DSScheduleLookups();
                ds = new DALAppointmentStatus().LookupAppointmentStatus();

                return new BLObject<DSScheduleLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupAppointmentStatus", ex);
                return new BLObject<DSScheduleLookups>(null, ex.Message);
            }

        }

        public BLObject<List<PatientFutureAppointment>> GetPatientFutureAppointmentForCopay(Int64 PatientId, Int64 InsuranceId)
        {
            try
            {
                List<PatientFutureAppointment> PatientFutureAppointmentList = new List<PatientFutureAppointment>();
                PatientFutureAppointmentList = new DALAppointmentStatus().GetPatientFutureAppointmentForCopay(PatientId, InsuranceId);

                return new BLObject<List<PatientFutureAppointment>>(PatientFutureAppointmentList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::GetPatientFutureAppointmentForCopay", ex);
                return new BLObject<List<PatientFutureAppointment>>(null, ex.Message);
            }
        }

        public BLObject<List<PatientFutureAppointment>> GetPatientFutureAppointment(Int64 PatientId)
        {
            try
            {
                List<PatientFutureAppointment> PatientFutureAppointmentList = new List<PatientFutureAppointment>();
                PatientFutureAppointmentList = new DALAppointmentStatus().GetPatientFutureAppointment(PatientId);

                return new BLObject<List<PatientFutureAppointment>>(PatientFutureAppointmentList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::GetPatientFutureAppointment", ex);
                return new BLObject<List<PatientFutureAppointment>>(null, ex.Message);
            }
        }
        #endregion

        #region "Resource Schedule"

        public BLObject<DSScheduleSetup> InsertResourceSchedule(DSScheduleSetup ds)
        {
            try
            {

                ds = new DALResourceSchedule().InsertResourceSchedule(ds);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertResourceSchedule", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleSetup> LoadResourceSchedule(long ResScheduleId, string ResourceId, string FacilityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSScheduleSetup ds = new DSScheduleSetup();
                ds = new DALResourceSchedule().LoadResourceSchedule(ResScheduleId, ResourceId, FacilityId, PageNumber, RowsPerPage);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadResourceSchedule", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        public BLObject<string> DeleteResourceSchedule(string ResScheduleId)
        {
            try
            {
                ResScheduleId = new DALResourceSchedule().DeleteResourceSchedule(ResScheduleId);
                return new BLObject<string>(ResScheduleId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::DeleteResourceSchedule", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleSetup> UpdateResourceSchedule(DSScheduleSetup ds)
        {
            try
            {
                ds = new DALResourceSchedule().UpdateResourceSchedule(ds);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateResourceSchedule", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleSetup> LoadScheduleSchReasonDuration(long ScheduleReasonId)
        {
            try
            {
                DSScheduleSetup ds = new DSScheduleSetup();
                ds = new DALResourceSchedule().LoadScheduleSchReasonDuration(ScheduleReasonId);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadScheduleSchReasonDuration", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        #endregion

        #region "Appointment"
        public bool CheckifCancelledAppointmentExists(long PatientId, long FacilityId, string AppDate, string FromTime, string ToTime, long ProviderId, long ResourceId = 0)
        {
            try
            {
                return new DALAppointment().CheckifCancelledAppointmentExists(PatientId, FacilityId, AppDate, FromTime, ToTime, ProviderId, ResourceId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::CheckifCancelledAppointmentExists", ex);
                return false;
            }
        }


        public BLObject<DSAppointment> InsertAppointment(DSAppointment ds)
        {
            try
            {
                ds = new DALAppointment().InsertAppointment(ds);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertAppointment", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        public BLObject<DSAppointment> LoadProviderAppointmentSlotInfo(long ProviderId, long FacilityId, string AppointmentDate, long Duration)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadProviderAppointmentSlotInfo(ProviderId, FacilityId, AppointmentDate, Duration);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadProviderAppointmentSlotInfo", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }
        public BLObject<DSAppointment> LoadAppointment(long AppointmentId, string PatientId, string ProviderId, string ResourceId, string FacilityId)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadAppointment(AppointmentId, PatientId, ProviderId, ResourceId, FacilityId);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadAppointment", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }
        public BLObject<DSDBAudit> LoadAppointmentHistory(long AppointmentId, DateTime? CreatedOn)
        {
            try
            {
                DSDBAudit ds = new DSDBAudit();
                ds = new DALAppointment().LoadAppointmentHistory(AppointmentId, CreatedOn);
                return new BLObject<DSDBAudit>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadAppointmentHistory", ex);
                return new BLObject<DSDBAudit>(null, ex.Message);
            }
        }

        public BLObject<List<FollowUpAppointmentModel>> LoadPatientAppointment(long patientId, long ProviderId, long FacilityId, string AppointmentDate)
        {
            try
            {
                List<FollowUpAppointmentModel> PatientAppointment = new List<FollowUpAppointmentModel>();
                PatientAppointment = new DALAppointment().LoadPatientAppointment(patientId, ProviderId, FacilityId, AppointmentDate);
                return new BLObject<List<FollowUpAppointmentModel>>(PatientAppointment);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::LoadPreviousProblemLists", ex);
                return new BLObject<List<FollowUpAppointmentModel>>(null, ex.Message);
            }
        }

        public BLObject<List<FollowUpAppointmentModel>> LoadAvailableSlots(long ProviderId, long FacilityId, string ScheduleDate)
        {
            try
            {
                List<FollowUpAppointmentModel> AvailableSlots = new List<FollowUpAppointmentModel>();
                AvailableSlots = new DALAppointment().LoadAvailableSlots(ProviderId, FacilityId, ScheduleDate);
                return new BLObject<List<FollowUpAppointmentModel>>(AvailableSlots);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::LoadAvailableSlots", ex);
                return new BLObject<List<FollowUpAppointmentModel>>(null, ex.Message);
            }
        }
        public BLObject<DSAppointment> UpdateAppointment(DSAppointment ds)
        {
            try
            {
                ds = new DALAppointment().UpdateAppointment(ds);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateAppointment", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }
        public BLObject<DSAppointment> UpdateAppointmentStatus(DSAppointment ds)
        {
            try
            {
                ds = new DALAppointment().UpdateAppointmentStatus(ds);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateAppointmentStatus", ex);
                string Message = "";
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    Message = str[1].ToString();
                else
                    Message = ex.Message;
                return new BLObject<DSAppointment>(null, Message);
            }
        }
        public BLObject<DSAppointment> UpdateAppointmentCancellationReason(DSAppointment ds)
        {
            try
            {
                ds = new DALAppointment().UpdateAppointmentCancellationReason(ds);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateAppointmentCancellationReason", ex);
                string Message = "";
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    Message = str[1].ToString();
                else
                    Message = ex.Message;
                return new BLObject<DSAppointment>(null, Message);
            }
        }
        public BLObject<DSAppointment> UpdateAppointmentWithTran(DSAppointment ds, IDBManager dbManager)
        {
            try
            {
                ds = new DALAppointment().UpdateAppointmentStatuswithTrns(ds, dbManager);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateAppointment", ex);
                string Message = "";
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    Message = str[1].ToString();
                else
                    Message = ex.Message;
                return new BLObject<DSAppointment>(null, Message);
            }
        }


        public BLObject<ResponseModel> UpdateAppointmentAndVisit(DSAppointment dsAppointment, DSVisits dsVisits, bool IsAlreadyVisitCreated, string VisitId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DataTable dtTemp = null;
            ResponseModel model = new ResponseModel();
            model.IsSuccess = false;
            try
            {
                dtTemp = dsVisits.PatientVisits.GetChanges();
                dbManager.BeginTransaction();
                dsAppointment = new DALAppointment().UpdateAppointmentStatuswithTrns(dsAppointment, dbManager);

                //*********End Appointment Status Update***********\\
                if (dsAppointment.PatientAppointments.Rows.Count > 0)
                {
                    if (IsAlreadyVisitCreated == false)
                    {
                        dsVisits = new DALVisits().InsertPatientVisitsWithTrans(dsVisits, dbManager);
                        if (dsVisits.PatientVisits.Rows.Count > 0)
                        {
                            //dsVisits = obj.Data;
                            VisitId = MDVUtility.ToStr(dsVisits.Tables[dsVisits.PatientVisits.TableName].Rows[0][dsVisits.PatientVisits.VisitIdColumn.ColumnName]);
                        }
                        else
                        {
                            dbManager.RollBackTransaction();
                            //return "";
                            model.Message = "unable to create visit";
                            return new BLObject<ResponseModel>(model);
                        }
                    }
                    dbManager.CommitTransaction();


                }
                else
                {
                    //if appointment updation failed and no exception occured rollback transaction
                    dbManager.RollBackTransaction();
                    //return "";
                    model.Message = "unable to update appointment";
                    return new BLObject<ResponseModel>(model);
                }

            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                //throw ex;
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    model.Message = str[1].ToString();
                else
                    model.Message = ex.Message;

                return new BLObject<ResponseModel>(model);
            }
            finally
            {
                dbManager.Close();
            }

            //for Audit log 
            try
            {
                //Audit log entry after transaction commit
                new DALVisits().InsertPatientVisitsAuditlog(dtTemp, dsVisits);
                model.Data = VisitId;
                model.IsSuccess = true;
                return new BLObject<ResponseModel>(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BLObject<DSVisits> UpdateVisit(DSVisits ds)
        {
            try
            {
                ds = new DALAppointment().UpdateVisit(ds);
                return new BLObject<DSVisits>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateAppointment", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }
        }
        public BLObject<DSScheduleSetup> LoadScheduleAppReasonDuration(long ScheduleReasonId)
        {
            try
            {
                DSScheduleSetup ds = new DSScheduleSetup();
                ds = new DALAppointment().LoadScheduleAppReasonDuration(ScheduleReasonId);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadScheduleAppReasonDuration", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }
        public BLObject<string> DeletePatAppointment(string AppointmentId)
        {
            try
            {
                AppointmentId = new DALAppointment().DeletePatAppointment(AppointmentId);
                return new BLObject<string>(AppointmentId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::DeletePatAppointment", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> MoveAppointment(string AppointmentId, string PasteTimeSlotDtlId, string NewAppDate, string NewAppTime, string IsMoveToNextDay = "0")
        {
            try
            {
                AppointmentId = new DALAppointment().MoveAppointment(AppointmentId, PasteTimeSlotDtlId, NewAppDate, NewAppTime, IsMoveToNextDay);
                return new BLObject<string>(AppointmentId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::MoveAppointment", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        public BLObject<DSAppointment> UpdateAppointmentReferral(long appointmentId, long referralId)
        {
            DSAppointment ds = new DSAppointment();
            try
            {
                ds = new DALAppointment().UpdateAppointmentReferral(appointmentId, referralId);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateAppointmentReferral", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }
        /// <summary>
        /// Load Provider ALL appointment on multiple facilites for print
        /// </summary>
        /// <param name="ProviderId"></param>
        /// <param name="FacilityId"></param>
        /// <param name="AppointmentDate"></param>
        /// <returns></returns>
        public BLObject<List<ProviderAppointmentPrint>> LoadProviderAppointmentPrint(string ProviderId, string FacilityId, string AppointmentDate, DataTable dtAppointmentStatus, string ResourceId)
        {
            try
            {
                List<ProviderAppointmentPrint> providerAppointmentPrint = new List<ProviderAppointmentPrint>();
                providerAppointmentPrint = new DALAppointment().LoadProviderAppointmentPrint(ProviderId, FacilityId, AppointmentDate, dtAppointmentStatus, ResourceId);

                return new BLObject<List<ProviderAppointmentPrint>>(providerAppointmentPrint);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadProviderAppointmentPrint", ex);
                return new BLObject<List<ProviderAppointmentPrint>>(null, ex.Message);
            }
        }
        public BLObject<List<AppointmentCopayAlert>> LoadAppointmentAlert(string AppointmentId)
        {
            try
            {
                List<AppointmentCopayAlert> Appointmentalert = new List<AppointmentCopayAlert>();
                Appointmentalert = new DALAppointment().LoadAppointmentAlert(AppointmentId);

                return new BLObject<List<AppointmentCopayAlert>>(Appointmentalert);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::LoadAppointmentAlert", ex);
                return new BLObject<List<AppointmentCopayAlert>>(null, ex.Message);
            }
        }
        public BLObject<List<AppointmentModel>> GetAppointmentReferralNo(string ProviderId, string FacilityId, string PatientInsuranceId, string PatientId, string AppointmentDate)
        {
            try
            {
                var result = new DALAppointment().GetAppointmentReferralNo(ProviderId, FacilityId, PatientInsuranceId, PatientId, AppointmentDate);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::GetAppointmentReferralNo", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }

        #endregion

        #region "Search Daily Slot"

        public BLObject<DSAppointment> SearchDailySlots(long ProviderId, long ResourceId, long FacilityId, string SlotDate, string StatusId, string PatientTypeId = "", string VisitTypeId = "")
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().SearchDailySlots(ProviderId, ResourceId, FacilityId, SlotDate, StatusId, PatientTypeId, VisitTypeId);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::SearchDailySlots", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        private void ScheduleMarginCalculate(ref DSAppointment dsSchedule)
        {
            int maxCountApp = 0;
            int maxCountAppRemove = 0;

            for (int i = 0; i < dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count; i++)
            {

                string[] AppDtl = dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i][dsSchedule.DaySlots.AppDtlColumn.ColumnName].ToString().Split('|');

                Stack appids = new Stack();
                for (int j = 0; j < AppDtl.Length; j++)
                {

                    string[] split = AppDtl[j].Split(',');
                    if (split.Length == 22 || split.Length == 24)
                    {
                        appids.Push(split[0]);


                        int showapp = 0;
                        for (int z = 0; z < appids.Count; z++)
                        {
                            if (appids.Contains(split[0]))
                            {
                                showapp = showapp + 1;
                            }

                        }

                        int appcount = Convert.ToInt32(split[7]);
                        if (appcount > 0 && showapp >= 1)
                        {
                            if (maxCountAppRemove == 0)
                            {
                                maxCountApp = AppDtl.Length - 1;
                                maxCountAppRemove = appcount;
                            }

                            for (int k = i + 1; k < (i + appcount); k++)
                            {

                                if (k < dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count)
                                {
                                    string[] AppDtl1 = dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[k][dsSchedule.DaySlots.AppDtlColumn.ColumnName].ToString().Split('|');

                                    if (maxCountApp < AppDtl1.Length - 1)
                                    {
                                        maxCountApp = AppDtl1.Length - 1;
                                        maxCountAppRemove = appcount;
                                        goto ExitFor;
                                    }
                                }
                            }

                        }
                    }

                }
                ExitFor:
                dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i][dsSchedule.DaySlots.MaxCountAppColumn.ColumnName] = maxCountApp;

                if (maxCountAppRemove > 0)
                    maxCountAppRemove = maxCountAppRemove - 1;

                if (maxCountAppRemove == 0)
                    maxCountApp = 0;

            }

        }

        public void ScheduleAppSort(ref DSAppointment dsSchedule)
        {
            int k = 1;
            // string[] emty = {",,,,,,,,"};


            for (int i = 0; i < dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count; i++)
            {
                List<string> myCollection = new List<string>();
                string formatedapp = "";

                string[] AppDtl = dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i][dsSchedule.DaySlots.AppDtlColumn.ColumnName].ToString().Split('|');
                if (AppDtl[0] == "")
                {
                    dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i][dsSchedule.DaySlots.AppDtlColumn.ColumnName] = "";
                    k++;
                    continue;
                }
                else
                {

                    //dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i][dsSchedule.DaySlots.AppDtlColumn.ColumnName].ToString();

                }
                if (k < dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count)
                {
                    for (int j = 0; j < dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count; j++)
                    {

                        string[] AppDtl1 = dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[k][dsSchedule.DaySlots.AppDtlColumn.ColumnName].ToString().Split('|');
                        if (AppDtl1[0] == "")
                        {
                            // dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i][dsSchedule.DaySlots.AppDtlColumn.ColumnName] = "";

                        }
                        else
                        {

                            for (int y = 0; y < AppDtl.Length; y++)
                            {
                                if (AppDtl[y] != "")
                                {
                                    for (int z = 0; z < AppDtl1.Length; z++)
                                    {
                                        if (AppDtl[y] == AppDtl1[z])
                                        {
                                            ////isExist = true;
                                            //if (y != z)
                                            //{
                                            //    for (int kk = 0; kk < y; kk++)
                                            //    {
                                            //        List<string> emty = new List<string>();
                                            //        string[] str = AppDtl[y].Split(',');

                                            //        emty.Add("0,,,,,,," + str[7] + ",");
                                            //        myCollection.Add(emty[0]);
                                            //    }
                                            //}
                                            myCollection.Add(AppDtl[y]);
                                        }
                                        //else
                                        //{
                                        //    if (isExist==true)
                                        //        myCollection.Add(emty[0]);
                                        //}
                                    }
                                }

                            }
                            for (int z = 0; z < AppDtl1.Length; z++)
                            {
                                if (AppDtl1[z] != "")
                                    myCollection.Add(AppDtl1[z]);


                            }

                            IEnumerable<string> distinctAppointment = myCollection.Distinct();
                            foreach (string app in distinctAppointment)
                            {
                                if (app != "")
                                    formatedapp += app + '|';
                                // Console.WriteLine(app);
                            }

                            dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[k][dsSchedule.DaySlots.AppDtlColumn.ColumnName] = formatedapp;
                        }
                        k++;
                        break;
                    }
                }

            }
            ScheduleEmptyAppAdd(ref dsSchedule);
            ScheduleMarginCalculate(ref dsSchedule);
        }

        private void ScheduleEmptyAppAdd(ref DSAppointment dsSchedule)
        {
            int k = 1;

            for (int i = 0; i < dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count; i++)
            {
                List<string> myCollection = new List<string>();
                string formatedapp = "";

                string[] AppDtl = dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i][dsSchedule.DaySlots.AppDtlColumn.ColumnName].ToString().Split('|');
                if (AppDtl[0] == "")
                {
                    dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i][dsSchedule.DaySlots.AppDtlColumn.ColumnName] = "";
                    k++;
                    continue;
                }
                else
                {

                    //dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i][dsSchedule.DaySlots.AppDtlColumn.ColumnName].ToString();

                }
                if (k < dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count)
                {
                    for (int j = 0; j < dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows.Count; j++)
                    {

                        string[] AppDtl1 = dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[k][dsSchedule.DaySlots.AppDtlColumn.ColumnName].ToString().Split('|');
                        if (AppDtl1[0] == "")
                        {
                            // dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[i][dsSchedule.DaySlots.AppDtlColumn.ColumnName] = "";

                        }
                        else
                        {
                            int kkk = 0;
                            for (int y = 0; y < AppDtl.Length; y++)
                            {

                                if (AppDtl[y] != "")
                                {
                                    for (int z = 0; z < AppDtl1.Length; z++)
                                    {
                                        if (AppDtl[y] == AppDtl1[z])
                                        {
                                            //isExist = true;
                                            if (y != z)
                                            {
                                                for (int kk = kkk; kk < y; kk++)
                                                {
                                                    List<string> emty = new List<string>();
                                                    string[] str = AppDtl[y].Split(',');

                                                    emty.Add("0,,,,,,," + str[7] + ",,,,,,,,,,,," + kk);
                                                    kkk++;
                                                    myCollection.Add(emty[0]);
                                                }
                                            }

                                            kkk++;

                                            myCollection.Add(AppDtl[y]);
                                        }

                                    }
                                }

                            }
                            for (int z = 0; z < AppDtl1.Length; z++)
                            {
                                if (AppDtl1[z] != "")
                                    myCollection.Add(AppDtl1[z]);


                            }

                            IEnumerable<string> distinctAppointment = myCollection.Distinct();
                            foreach (string app in distinctAppointment)
                            {
                                if (app != "")
                                    formatedapp += app + '|';
                                // Console.WriteLine(app);
                            }

                            dsSchedule.Tables[dsSchedule.DaySlots.TableName].Rows[k][dsSchedule.DaySlots.AppDtlColumn.ColumnName] = formatedapp;
                        }
                        k++;
                        break;
                    }
                }
            }
        }
        #endregion

        #region "Weekly Appointment"

        public BLObject<DSAppointment> SelectWeeklyAppointment(long ProviderId, long FacilityId, long ResourceId, string StartDate, string EndDate, string DaysOfWeek)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().SelectWeeklyAppointment(ProviderId, FacilityId, ResourceId, StartDate, EndDate, DaysOfWeek);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::SelectWeeklyAppointment", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        #endregion

        #region "Batch Scheduling"

        public BLObject<DSAppointment> SchedulingSearch(long PatientId, long FacilityId, long PracticeId, long ProviderId, long ResourceId, string FromDate, string ToDate, string FromTime, string ToTime, string AMPM, string GetStatus, string GetDays, string prresbit, int PageNumber, int RowspPage)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().SchedulingSearch(PatientId, FacilityId, PracticeId, ProviderId, ResourceId, FromDate, ToDate, FromTime, ToTime, AMPM, GetStatus, GetDays, prresbit, PageNumber, RowspPage);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::SchedulingSearch", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }
        public BLObject<string> ChangeSchFacility(string SlotDtlIds, long MoveFacilityId)
        {
            try
            {
                SlotDtlIds = new DALAppointment().ChangeSchFacility(SlotDtlIds, MoveFacilityId);
                return new BLObject<string>(SlotDtlIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::ChangeSchFacility", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        #endregion

        #region "Scheduler Functions"
        public BLObject<DSAppointment> LoadSchAppointment(long ProviderId, long FacilityId, string SlotDate, string Color, long TimeSlotDtlId, long ResourceId, int PageNumber = 0, int RowspPage = 0, int IsPaging = 0)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadSchAppointment(ProviderId, FacilityId, SlotDate, Color, TimeSlotDtlId, ResourceId, PageNumber, RowspPage, IsPaging);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadSchAppointment", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        public BLObject<DSAppointment> LoadWeeklySlotAppointment(long ProviderId, long FacilityId, long ResourceId, string SlotDate)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadWeeklySlotAppointment(ProviderId, FacilityId, ResourceId, SlotDate);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadWeeklySlotAppointment", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        public BLObject<DSAppointment> SelectMonthlyAppointment(long ProviderId, long FacilityId, long ResourceId, string MonthYear, string StatusId, string PatientTypeId = "", string VisitTypeId = "")
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().SelectMonthlyAppointment(ProviderId, FacilityId, ResourceId, MonthYear, StatusId, PatientTypeId, VisitTypeId);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::SelectMonthlyAppointment", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        #endregion

        #region "Slot Functions"
        public BLObject<DSAppointment> UpdateSchSlot(long TimeSlotDtlId, Int32 PatientAllowed, Boolean OverBookAllowed, Int32 BlockReasonId, string Comments)
        {

            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().UpdateSchSlot(TimeSlotDtlId, PatientAllowed, OverBookAllowed, BlockReasonId, Comments);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateSchSlot", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        public BLObject<DSAppointment> LoadScheduleSlot(long TimeSlotDtlId, long ProviderId, long ResourceId)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadScheduleSlot(TimeSlotDtlId);

                ds.Merge(new DALAppointment().LoadSchAppointment(ProviderId, 0, "", "", TimeSlotDtlId, ResourceId));

                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadScheduleSlot", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        public BLObject<string> UpdateSlotStatus(string SlotId, long BlockReasonId, string BlockStatus, string Comments)
        {
            try
            {
                SlotId = new DALAppointment().UpdateSlotStatus(SlotId, BlockReasonId, BlockStatus, Comments);
                return new BLObject<string>(SlotId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateSlotStatus", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region "Multiple Schedule Group "

        public BLObject<DSScheduleGroup> InsertScheduleGroups(DSScheduleGroup ds)
        {
            try
            {

                ds = new DALScheduleGroups().InsertScheduleGroups(ds);
                return new BLObject<DSScheduleGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertScheduleGroups", ex);
                return new BLObject<DSScheduleGroup>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleGroup> LoadScheduleGroups(int MSGroupId, string ShortName, string Description, string IsActive, string EntityId)
        {
            try
            {
                DSScheduleGroup ds = new DSScheduleGroup();
                ds = new DALScheduleGroups().LoadScheduleGroups(MSGroupId, ShortName, Description, IsActive, EntityId);

                //ds.Merge(new DALScheduleGroups().LoadScheduleResources());
                //ds.Merge(new DALScheduleGroups().LoadScheduleProvider());

                return new BLObject<DSScheduleGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadScheduleGroups", ex);
                return new BLObject<DSScheduleGroup>(null, ex.Message);
            }
        }
        public BLObject<DSScheduleGroup> LoadScheduleGroupsByMSGroupId(int MSGroupId)
        {
            try
            {
                DSScheduleGroup ds = new DSScheduleGroup();
                ds = new DALScheduleGroups().LoadScheduleGroups(MSGroupId, "", "", "", "");

                ds.Merge(new DALScheduleGroups().LoadScheduleProvider());
                ds.Merge(new DALScheduleGroups().LoadGroupProvider(MSGroupId));

                foreach (DataRow dr in ds.Tables[ds.ScheduleProvider.TableName].Rows)
                {
                    DataRow[] drGroupProvider = ds.Tables[ds.GroupProviders.TableName].Select(ds.GroupProviders.ProviderIdColumn.ColumnName + "=" + dr[ds.ScheduleProvider.ProviderIdColumn.ColumnName] + " AND " + ds.GroupProviders.FacilityIdColumn.ColumnName + "=" + dr[ds.ScheduleProvider.FacilityIdColumn.ColumnName] + " AND " + ds.GroupProviders.ProScheduleIdColumn.ColumnName + "=" + dr[ds.ScheduleProvider.ProScheduleIdColumn.ColumnName]);
                    if (drGroupProvider.Length > 0)
                        dr[ds.ScheduleProvider.GrpProvidersIdColumn.ColumnName] = drGroupProvider[0][ds.GroupProviders.GrpProvidersIdColumn.ColumnName];
                }

                ds.Merge(new DALScheduleGroups().LoadScheduleResources());
                ds.Merge(new DALScheduleGroups().LoadGroupResource(MSGroupId));

                foreach (DataRow dr in ds.Tables[ds.ScheduleResource.TableName].Rows)
                {
                    DataRow[] drGroupResource = ds.Tables[ds.GroupResources.TableName].Select(ds.GroupResources.ResourceIdColumn.ColumnName + "=" + dr[ds.ScheduleResource.ResourceIdColumn.ColumnName] + " AND " + ds.GroupResources.FacilityIdColumn.ColumnName + "=" + dr[ds.ScheduleResource.FacilityIdColumn.ColumnName] + " AND " + ds.GroupResources.ResScheduleIdColumn.ColumnName + "=" + dr[ds.ScheduleResource.ResScheduleIdColumn.ColumnName]);
                    if (drGroupResource.Length > 0)
                        dr[ds.ScheduleResource.GrpResourcesIdColumn.ColumnName] = drGroupResource[0][ds.GroupResources.GrpResourcesIdColumn.ColumnName];
                }





                return new BLObject<DSScheduleGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadScheduleGroups", ex);
                return new BLObject<DSScheduleGroup>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteScheduleGroups(string MSGroupId)
        {
            try
            {
                MSGroupId = new DALScheduleGroups().DeleteScheduleGroups(MSGroupId);
                return new BLObject<string>(MSGroupId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::DeleteScheduleGroups", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleGroup> UpdateScheduleGroups(DSScheduleGroup ds)
        {
            try
            {
                ds = new DALScheduleGroups().UpdateScheduleGroups(ds);
                return new BLObject<DSScheduleGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateScheduleGroups", ex);
                return new BLObject<DSScheduleGroup>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleGroup> LoadSchGroupProvResByID(long MSGroupId)
        {
            try
            {
                DSScheduleGroup ds = new DSScheduleGroup();
                ds = new DALScheduleGroups().LoadSchGroupProvResByID(MSGroupId);

                return new BLObject<DSScheduleGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadSchGroupProvResByID", ex);
                return new BLObject<DSScheduleGroup>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleGroup> LoadGroupProvider(long MSGroupId)
        {
            try
            {
                DSScheduleGroup ds = new DSScheduleGroup();
                ds = new DALScheduleGroups().LoadGroupProvider(MSGroupId);

                return new BLObject<DSScheduleGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadGroupProvider", ex);
                return new BLObject<DSScheduleGroup>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleGroup> LoadGroupResource(long MSGroupId)
        {
            try
            {
                DSScheduleGroup ds = new DSScheduleGroup();
                ds = new DALScheduleGroups().LoadGroupResource(MSGroupId);

                return new BLObject<DSScheduleGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadGroupResource", ex);
                return new BLObject<DSScheduleGroup>(null, ex.Message);
            }
        }


        public BLObject<string> DeleteGroupProvider(string GrpProvidersId)
        {
            try
            {
                GrpProvidersId = new DALScheduleGroups().DeleteGroupProvider(GrpProvidersId);
                return new BLObject<string>(GrpProvidersId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::DeleteGroupProvider", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> DeleteGroupResource(string GrpResourcesId)
        {
            try
            {
                GrpResourcesId = new DALScheduleGroups().DeleteGroupResource(GrpResourcesId);
                return new BLObject<string>(GrpResourcesId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::DeleteGroupResource", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        public BLObject<DSScheduleGroup> InsertSchGroupProvider(ref DSScheduleGroup ds)
        {
            try
            {
                ds = new DALScheduleGroups().InsertSchGroupProvider(ref ds);
                return new BLObject<DSScheduleGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertSchGroupProvider", ex);
                return new BLObject<DSScheduleGroup>(null, ex.Message);
            }
        }


        public BLObject<DSScheduleGroup> InsertSchGroupResource(ref DSScheduleGroup ds)
        {
            try
            {
                ds = new DALScheduleGroups().InsertSchGroupResource(ref ds);
                return new BLObject<DSScheduleGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertSchGroupResource", ex);
                return new BLObject<DSScheduleGroup>(null, ex.Message);
            }
        }


        # region Lookup
        /// <summary>
        /// Lookups the  LookupSchedule Groups.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSScheduleLookups> LookupScheduleGroups()
        {
            try
            {
                DSScheduleLookups ds = new DSScheduleLookups();
                ds = new DALScheduleGroups().LookupScheduleGroups();

                return new BLObject<DSScheduleLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupScheduleGroups", ex);
                return new BLObject<DSScheduleLookups>(null, ex.Message);
            }

        }
        #endregion

        #endregion

        #region "Multiple Slots With Appointment"

        public BLObject<DSAppointment> LoadMultipleSlotAppointment(long ProviderId, long FacilityId, long ResourceId, string SlotDate)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadMultipleSlotAppointment(ProviderId, FacilityId, ResourceId, SlotDate);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadMultipleSlotAppointment", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        #endregion

        #region "Wait List"
        public BLObject<DSAppointment> LookupWaitListStatus()
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LookupWaitListStatus();

                return new BLObject<DSAppointment>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupWaitListStatus", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }

        }
        public BLObject<DSAppointment> LookupPreferredTime()
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LookupPreferredTime();

                return new BLObject<DSAppointment>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupPreferredTime", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }

        }
        public BLObject<DSAppointment> LoadWaitList(long WaitListId, long PatientId, long FacilityId, long ProviderId, long ResourceId, int PrfTimeId, int WtListStatusId, DateTime? FromDate = null, DateTime? PreferredDate = null, DateTime? ToDate = null, int PageNumber = 1, int RowspPage = 1000)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadWaitList(WaitListId, PatientId, FacilityId, ProviderId, ResourceId, PrfTimeId, WtListStatusId, FromDate, PreferredDate, ToDate, PageNumber, RowspPage);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadWaitList", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }
        public BLObject<DSAppointment> InsertWaitList(DSAppointment ds)
        {
            try
            {
                ds = new DALAppointment().InsertWaitList(ds);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertWaitList", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }
        public BLObject<DSAppointment> UpdateWaitList(DSAppointment ds)
        {
            try
            {
                ds = new DALAppointment().UpdateWaitList(ds);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateWaitList", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteWaitList(string WaitListId)
        {
            try
            {
                WaitListId = new DALAppointment().DeleteWaitList(WaitListId);
                return new BLObject<string>(WaitListId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::DeleteWaitList", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        #endregion

        #region "Patient Balance"

        public BLObject<DSAppointment> LoadPatientBalance(long PatientId)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadPatientBalance(PatientId);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadPatientBalance", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        #endregion

        #region "Dasboard Appointments Visits"

        public BLObject<DSAppointment> LoadAppointmentsVisits(long ProviderId, long FacilityId, long ResourceId, string AppointmentDate, string LastName, string FirstName, string AccountNumber, string Status, string IsCheckedIn, Int32 PageNumber, Int32 RowspPage, string Action, long? PatientId = 0, string IsFaceSheet = "0", string AppointmentStatusIds = "")
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadAppointmentsVisits(ProviderId, FacilityId, ResourceId, AppointmentDate, LastName, FirstName, AccountNumber, Status, IsCheckedIn, PageNumber, RowspPage, Action, PatientId, IsFaceSheet, AppointmentStatusIds);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadAppointmentsVisits", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }
        public BLObject<DSAppointment> LoadPatientAppointments(long PatientId, long ProviderId, long FacilityId, Int32 PageNumber, Int32 RowspPage, string AppointmentStatusIds = "")
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadPatientAppointments(PatientId, ProviderId, FacilityId, PageNumber, RowspPage, AppointmentStatusIds);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadPatientAppointments", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        #endregion

        #region "Dasboard Appointments Notes"
        public BLObject<DSAppointment> LoadAppointmentsNotes(string VisitFrom, string VisitTo, string NoteStatus, string FName, string LName, string NoteType, long ProviderId, string AccountNo, long? PatientId = 0, Int32 PageNumber = 0, Int32 RowsPerPage = 0, bool IsDraftNote = false)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadAppointmentsNotes(VisitFrom, VisitTo, NoteStatus, FName, LName, NoteType, ProviderId, AccountNo, PatientId, PageNumber, RowsPerPage, IsDraftNote);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadAppointmentsNotes", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }
        public BLObject<DSAppointment> LookupAppointmentAndVisitStatus()
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LookupAppointmentAndVisitStatus();

                return new BLObject<DSAppointment>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupAppointmentAndVisitStatus", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }

        }
        public BLObject<DSAppointment> LoadNotesDraftCount()
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadNotesDraftCount();

                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadNotesDraftCount", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        public BLObject<List<DAppointmentNoteModel>> LoadDashboardAppointmentsNotes(String VisitFrom, string VisitTo, string NoteStatus, Int32 PageNumber = 1, Int32 RowsPerPage = 15)
        {
            try
            {
                var result = new DALAppointment().LoadDashboardAppointmentsNotes(VisitFrom, VisitTo, NoteStatus, PageNumber, RowsPerPage);
                return new BLObject<List<DAppointmentNoteModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadDashboardAppointmentsNotes", ex);
                return new BLObject<List<DAppointmentNoteModel>>(null, ex.Message);
            }
        }


        public BLObject<List<NotesModel>> LoadAppointmentsNotesBulkSign(string VisitFrom, string VisitTo, string NoteStatus, string FName, string LName, string NoteType, long ProviderId, string AccountNo,
                long? PatientId = 0, Int32 PageNumber = 0, Int32 RowsPerPage = 0, int IsReadyOrMissing = 1, string ddlMissingInfo = null)
        {
            try
            {
                var result = new DALAppointment().LoadAppointmentsNotesBulkSign(VisitFrom, VisitTo, NoteStatus, FName, LName, NoteType, ProviderId,
                                    AccountNo, PatientId, PageNumber, RowsPerPage, IsReadyOrMissing, ddlMissingInfo);
                return new BLObject<List<NotesModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadAppointmentsNotes", ex);
                return new BLObject<List<NotesModel>>(null, ex.Message);
            }
        }

        #endregion

        #region Appointment Summary
        public BLObject<DSAppointment> LoadAppointmentSummary(ref DataTable dtFacilityIds, ref DataTable dtProviderIds, string FromDate, string ToDate, ref DataTable dtResourceIds)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadAppointmentSummary(ref dtFacilityIds, ref dtProviderIds, FromDate, ToDate, ref dtResourceIds);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadAppointmentSummary", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        #endregion

        #region Lookup
        public BLObject<DSScheduleLookups> LookupPatientType()
        {
            try
            {
                DSScheduleLookups ds = new DSScheduleLookups();
                ds = new DALAppointment().LookupPatientType();

                return new BLObject<DSScheduleLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupPatientType", ex);
                return new BLObject<DSScheduleLookups>(null, ex.Message);
            }

        }
        public BLObject<DSScheduleLookups> LookupPatientVisitType()
        {
            try
            {
                DSScheduleLookups ds = new DSScheduleLookups();
                ds = new DALAppointment().LookupPatientVisitType();

                return new BLObject<DSScheduleLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupPatientVisitType", ex);
                return new BLObject<DSScheduleLookups>(null, ex.Message);
            }

        }
        public BLObject<List<PatientVisitTypeLookUpModel>> LookupPatientVisitType_WO_CancerRegistries()
        {
            try
            {
                List<PatientVisitTypeLookUpModel> model = new List<PatientVisitTypeLookUpModel>();
                model = new DALAppointment().LookupPatientVisitType_WO_CancerRegistries();
                return new BLObject<List<PatientVisitTypeLookUpModel>>(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupPatientVisitType_WO_CancerRegistries", ex);
                return new BLObject<List<PatientVisitTypeLookUpModel>>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleLookups> LookupVisitType()
        {
            try
            {
                DSScheduleLookups ds = new DSScheduleLookups();
                ds = new DALAppointment().LookupVisitType();

                return new BLObject<DSScheduleLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupVisitType", ex);
                return new BLObject<DSScheduleLookups>(null, ex.Message);
            }

        }

        public BLObject<DSScheduleLookups> LookupPatientVisitType(string ProviderId)
        {
            try
            {
                DSScheduleLookups ds = new DSScheduleLookups();
                ds = new DALAppointment().LookupPatientVisitType(ProviderId);

                return new BLObject<DSScheduleLookups>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupPatientVisitType", ex);
                return new BLObject<DSScheduleLookups>(null, ex.Message);
            }

        }
        #endregion

        #region"week select new function"

        public BLObject<DSAppointment> SelectWeeklySlots(long ProviderId, long ResourceId, long FacilityId, string SlotDate, string StatusId, string PatientTypeId = "", string VisitTypeId = "")
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().SelectWeeklySlots(ProviderId, ResourceId, FacilityId, SlotDate, StatusId, PatientTypeId, VisitTypeId);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::SelectWeeklySlots", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        #endregion

        #region "Block Appointment Summary"
        public BLObject<DSAppointment> LoadBlockAppointmentSummary(ref DataTable dtFacilityIds, ref DataTable dtProviderIds, string FromDate, string ToDate, ref DataTable dtResourceIds)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadBlockAppointmentSummary(ref dtFacilityIds, ref dtProviderIds, FromDate, ToDate, ref dtResourceIds);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadBlockAppointmentSummary", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        public BLObject<DSAppointment> LookupSchedulingFromTime(Int64 ProviderId, Int64 FacilityId, Int64 ResourceId, string SchDate)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LookupSchedulingFromTime(ProviderId, FacilityId, ResourceId, SchDate);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LookupSchedulingFromTime", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        #endregion

        #region "Cancel CheckIn"

        public BLObject<string> AppointmentCancelCheckIn(string VisitId)
        {
            try
            {

                VisitId = new DALAppointment().AppointmentCancelCheckIn(VisitId);

                return new BLObject<string>(VisitId);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::AppointmentCancelCheckIn", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        #endregion

        #region Dasboard Appointments

        public BLObject<List<DAppointmentModel>> LoadDashboardAppointments(string AppointmentDate, string IsCheckedIn, Int32 PageNumber, Int32 RowspPage, string IsFaceSheet = "0")
        {
            try
            {

                var result = new DALAppointment().LoadDashboardAppointments(AppointmentDate, IsCheckedIn, PageNumber, RowspPage, IsFaceSheet);
                return new BLObject<List<DAppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadAppointmentsVisits", ex);
                return new BLObject<List<DAppointmentModel>>(null, ex.Message);
            }
        }

        #endregion

        #region "Portal Appointment Request"

        public BLObject<DSAppointment> InsertPortalAppRequest(DSAppointment ds)
        {
            try
            {

                ds = new DALAppointment().InsertPortalAppRequest(ds);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertPortalAppRequest", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }
        public BLObject<DSAppointment> LoadPortalAppRequest(long PortalAppRequestId, string RequestStatus, string AppDate)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().LoadPortalAppRequest(PortalAppRequestId, RequestStatus, AppDate);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadPortalAppRequest", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }
        public BLObject<DSAppointment> UpdatePortalAppRequest(DSAppointment ds)
        {
            try
            {
                ds = new DALAppointment().UpdatePortalAppRequest(ds);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdatePortalAppRequest", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        public BLObject<DSAppointment> AcceptRejectMultiplePortalAppRequest(string PortalRequestIds, string RequestStatus)
        {
            try
            {
                DSAppointment ds = new DSAppointment();
                ds = new DALAppointment().AcceptRejectMultiplePortalAppRequest(PortalRequestIds, RequestStatus);
                return new BLObject<DSAppointment>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::AcceptRejectMultiplePortalAppRequest", ex);
                return new BLObject<DSAppointment>(null, ex.Message);
            }
        }

        #endregion
        #region "UpdatePatientInsuranceINFutureAppointments"
        public string UpdateInsuranceInFutureAppointment(Int64 InsuranceId, string AppointmentId)
        {
            string Result;
            try
            {

                Result = new DALAppointment().UpdateInsuranceInFutureAppointments(InsuranceId, AppointmentId);
                return Result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateAppointment", ex);
                return ex.InnerException.ToString();
            }
        }

        public string UpdateAppointmentCopay(string AppointmentIds, Int64 InsuranceId)
        {
            try
            {
                string Result = new DALAppointment().UpdateAppointmentCopay(AppointmentIds, InsuranceId);
                return Result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateAppointmentCopay", ex);
                return ex.InnerException.ToString();
            }
        }
        #endregion

        #region VisitTypeDurationGroup
        public BLObject<DSScheduleSetup> LoadVisitTypeDurationGroup(long VisitTypeDurationGroupId, string Name, string IsActive, string EntityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSScheduleSetup ds = new DSScheduleSetup();
                ds = new DALVisitTypeDurationGroup().LoadVisitTypeDurationGroup(VisitTypeDurationGroupId, Name, IsActive, EntityId, PageNumber, RowsPerPage);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadVisitTypeDurationGroup", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }
        public BLObject<string> LoadDurationOnVisitType(Int64 providerId, Int64 PatientVisitTypeId)
        {
            string Duration = "";
            try
            {
                Duration = new DALVisitTypeDurationGroup().LoadDurationOnVisitType(providerId, PatientVisitTypeId);
                return new BLObject<string>(Duration);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadDurationOnVisitType", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleSetup> LoadVisitTypeDurationGroupForm(long VisitTypeDurationGroupId, string Name, string IsActive, string EntityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSScheduleSetup ds = new DSScheduleSetup();
                ds = new DALVisitTypeDurationGroup().LoadVisitTypeDurationGroupForm(VisitTypeDurationGroupId, Name, IsActive, EntityId, PageNumber, RowsPerPage);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadVisitTypeDurationGroup", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleLookups> LookupVisitTypeDurationGroup()
        {
            try
            {
                DSScheduleLookups ds = new DSScheduleLookups();
                ds = new DALVisitTypeDurationGroup().LookupVisitTypeDurationGroup();
                return new BLObject<DSScheduleLookups>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::GetVisitTypeDurationGroup", ex);
                return new BLObject<DSScheduleLookups>(null, ex.Message);
            }
        }

        public BLObject<DSScheduleSetup> InsertVisitTypeDurationGroup(DSScheduleSetup ds)
        {
            try
            {
                ds = new DALVisitTypeDurationGroup().InsertVisitTypeDurationGroup(ds, true);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertVisitTypeDurationGroup", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }

        public BLObject<string> InsertVisitTypeDurations(Int64 VisitTypeId, Int64 VisitGroupId, Int64 VisitDurationId, float Duration, bool IsActive, string CreatedBy, DateTime CreatedOn, string ModifiedBy, DateTime ModifiedOn, string color)
        {
            try
            {
                string retval = "";
                retval = new DALVisitTypeDurationGroup().InsertVisitTypeDurations(VisitTypeId, VisitGroupId, VisitDurationId, Duration, IsActive, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, color);
                return new BLObject<string>(retval);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::InsertVisitTypeDurations", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> UpdateVisitTypeDurationGroup(string GroupName, Int64 VisitTypeId, Int64 VisitGroupId, Int64 VisitDurationId, float Duration, bool IsActive, string CreatedBy, DateTime CreatedOn, string ModifiedBy, DateTime ModifiedOn, string EntityId, string color)
        {
            try
            {
                string retval = "";
                retval = new DALVisitTypeDurationGroup().UpdateVisitTypeDurationGroup(GroupName, VisitTypeId, VisitGroupId, VisitDurationId, Duration, IsActive, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, EntityId, color);
                return new BLObject<string>(retval);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateVisitTypeDurationGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> DeleteVisitTypeDurationGroup(string VisitTypeDurationGroupId)
        {
            try
            {
                VisitTypeDurationGroupId = new DALVisitTypeDurationGroup().DeleteVisitTypeDurationGroup(VisitTypeDurationGroupId);
                return new BLObject<string>(VisitTypeDurationGroupId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::DeleteVisitTypeDurationGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> UpdateVisitTypeDurationGroupActiveInActive(Int64 VisitTypeDurationGroupId, Int64 IsActive)
        {
            try
            {
                string message = null;
                message = new DALVisitTypeDurationGroup().UpdateVisitTypeDurationGroupActiveInActive(VisitTypeDurationGroupId, IsActive);
                return new BLObject<string>(message);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::UpdateVisitTypeDurationGroupActiveInActive", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region Scheduler Revamp
        public BLObject<List<AppointmentModel>> LoadProviderAppointmentDayView(DataTable dtProvider, DataTable dtFacility, long patientTypeId, DataTable dtVisitType, DataTable dtAppointmentStatus, string startDate)
        {
            try
            {
                var result = new DALAppointment().LoadProviderAppointmentDayView(dtProvider, dtFacility, patientTypeId, dtVisitType, dtAppointmentStatus, startDate);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadProviderAppointmentDayView", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }

        public string EDIEligibilityIdSelect(AppointmentModel model)
        {
            try
            {
                var result = new DALAppointment().EDIEligibilityIdSelect(model);
                return result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadProviderAppointmentDayView", ex);
                return ex.Message;
            }
        }
        public BLObject<List<AppointmentModel>> LoadResourceAppointmentDayView(DataTable dtResource, DataTable dtFacility, long patientTypeId, DataTable dtVisitType, DataTable dtAppointmentStatus, string startDate)
        {
            try
            {
                var result = new DALAppointment().LoadResourceAppointmentDayView(dtResource, dtFacility, patientTypeId, dtVisitType, dtAppointmentStatus, startDate);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadResourceAppointmentDayView", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }

        public BLObject<List<AppointmentModel>> LoadResourceAppointmentDayViewScheduel(DataTable dtResources, DataTable dtFacility, string startDate)
        {
            try
            {
                var result = new DALAppointment().LoadResourceAppointmentDayViewSchedule(dtResources, dtFacility, startDate);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadResourceAppointmentDayViewScheduel", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }

        public BLObject<List<AppointmentModel>> LoadProviderAppointmentDayViewScheduel(DataTable dtProviders, DataTable dtFacility, string startDate, string viewType = null)
        {
            try
            {
                var result = new DALAppointment().LoadProviderAppointmentDayViewSchedule(dtProviders, dtFacility, startDate, viewType);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadProviderAppointmentDayViewScheduel", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }
        public BLObject<List<AppointmentModel>> LoadProviderAppointmentWeekView(DataTable dtProvider, DataTable dtFacility, long patientTypeId, DataTable dtVisitType, DataTable dtAppointmentStatus, string startDate, string endDate)
        {
            try
            {
                var result = new DALAppointment().LoadProviderAppointmentWeekView(dtProvider, dtFacility, patientTypeId, dtVisitType, dtAppointmentStatus, startDate, endDate);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadProviderAppointmentWeekView", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }
        public BLObject<List<AppointmentModel>> LoadResourceAppointmentWeekView(DataTable dtResource, DataTable dtFacility, long patientTypeId, DataTable dtVisitType, DataTable dtAppointmentStatus, string startDate, string endDate)
        {
            try
            {
                var result = new DALAppointment().LoadResourceAppointmentWeekView(dtResource, dtFacility, patientTypeId, dtVisitType, dtAppointmentStatus, startDate, endDate);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadResourceAppointmentWeekView", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }
        public BLObject<List<AppointmentModel>> LoadProviderAppointmentWorkWeekView(DataTable dtProvider, DataTable dtFacility, long patientTypeId, DataTable dtVisitType, DataTable dtAppointmentStatus, string startDate, string endDate, string WorkWeekDates = null)
        {
            try
            {
                var result = new DALAppointment().LoadProviderAppointmentWorkWeekView(dtProvider, dtFacility, patientTypeId, dtVisitType, dtAppointmentStatus, startDate, endDate, WorkWeekDates);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadProviderAppointmentWorkWeekView", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }

        public BLObject<List<AppointmentModel>> LoadProviderWorkWeekSchedules(DataTable dtProvider, DataTable dtFacility, string startDate)
        {
            try
            {
                var result = new DALAppointment().LoadProviderWorkWeekSchedules(dtProvider, dtFacility, startDate);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadProviderWorkWeekSchedules", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }

        public BLObject<List<AppointmentModel>> LoadResourceWorkWeekSchedules(DataTable dtResources, DataTable dtFacility, string startDate)
        {
            try
            {
                var result = new DALAppointment().LoadResourceWorkWeekSchedules(dtResources, dtFacility, startDate);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadResourceWorkWeekSchedules", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }
        public BLObject<List<AppointmentModel>> LoadResourceAppointmentWorkWeekView(DataTable dtResource, DataTable dtFacility, long patientTypeId, DataTable dtVisitType, DataTable dtAppointmentStatus, string startDate, string endDate, string WorkWeekDays = null)
        {
            try
            {
                var result = new DALAppointment().LoadResourceAppointmentWorkWeekView(dtResource, dtFacility, patientTypeId, dtVisitType, dtAppointmentStatus, startDate, endDate, WorkWeekDays);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadResourceAppointmentWorkWeekView", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }

        public BLObject<List<AppointmentModel>> LoadProviderAppointmentMonthView(DataTable dtProvider, DataTable dtFacility, long patientTypeId, DataTable dtVisitType, DataTable dtAppointmentStatus, int Month = 0, int Year = 0)
        {
            try
            {

                var result = new DALAppointment().LoadProviderAppointmentMonthView(dtProvider, dtFacility, patientTypeId, dtVisitType, dtAppointmentStatus, Month, Year);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadProviderAppointmentMonthView", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }
        public BLObject<List<AppointmentModel>> LoadResourceAppointmentMonthView(DataTable dtResource, DataTable dtFacility, long patientTypeId, DataTable dtVisitType, DataTable dtAppointmentStatus, int Month = 0, int Year = 0)
        {
            try
            {

                var result = new DALAppointment().LoadResourceAppointmentMonthView(dtResource, dtFacility, patientTypeId, dtVisitType, dtAppointmentStatus, Month, Year);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadResourceAppointmentMonthView", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }
        public BLObject<List<AppointmentStatusOptionModel>> AppointmentStatusOptions(long AppointmentStatusId, string AppointmentStatus)
        {
            try
            {

                var result = new DALAppointment().AppointmentStatusOptions(AppointmentStatusId, AppointmentStatus);
                return new BLObject<List<AppointmentStatusOptionModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::AppointmentStatusOptions", ex);
                return new BLObject<List<AppointmentStatusOptionModel>>(null, ex.Message);
            }
        }
        public AppointmentTooltipModel FillToolTipData(string AppointmentId)
        {
            try
            {
                AppointmentTooltipModel TooltipModel = new AppointmentTooltipModel();
                TooltipModel = new DALAppointment().FillToolTipData(AppointmentId);
                return TooltipModel;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::FillToolTipData", ex);
                throw ex;
            }
        }


        public BLObject<List<AppointmentModel>> LoadWaitListSchedule(AppointmentModel model)
        {
            try
            {
                var result = new DALAppointment().LoadWaitListSchedule(model);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadWaitListSchedule", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }
        public BLObject<List<AppointmentModel>> LoadResourceWaitListSchedule(AppointmentModel model)
        {
            try
            {
                var result = new DALAppointment().LoadResourceWaitListSchedule(model);
                return new BLObject<List<AppointmentModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadWaitListSchedule", ex);
                return new BLObject<List<AppointmentModel>>(null, ex.Message);
            }
        }
        public BLObject<string> CutPasteAppointment(AppointmentModel model)
        {
            try
            {

                var result = new DALAppointment().CutPasteAppointment(model);
                return new BLObject<string>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::CutPasteAppointment", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> CopyPasteAppointment(AppointmentModel model)
        {
            try
            {

                var result = new DALAppointment().CopyPasteAppointment(model);
                return new BLObject<string>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::CopyPasteAppointment", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSScheduleSetup> LoadSchBlockHours(DataTable dtProvider, DataTable dtResource, DateTime date, string viewType = "0")
        {
            try
            {
                DSScheduleSetup ds = new DSScheduleSetup();
                ds = new DALAppointment().LoadSchBlockHours(dtProvider, dtResource, date, viewType);
                return new BLObject<DSScheduleSetup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSchedule::LoadSchBlockHours", ex);
                return new BLObject<DSScheduleSetup>(null, ex.Message);
            }
        }
        #endregion Scheduler Revamp

        public string SaveAppointmentNative(EmptySlotModel model)
        {
            return new DALAppointment().SaveAppointmentNative(model);
        }
    }
}
