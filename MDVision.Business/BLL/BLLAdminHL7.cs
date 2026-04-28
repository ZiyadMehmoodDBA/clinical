using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.DataAccess.DCommon; 
using MDVision.Common.Shared;
using MDVision.DataAccess.DAL.Admin;
using MDVision.Datasets;
using System.Configuration;
using MDVision.Common.Logging;

namespace MDVision.Business.BLL
{
    public class BLLAdminHL7
    {
        
        #region Variable
        
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLAdminEDI"/> class.
        /// </summary>
        public BLLAdminHL7()
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
        public BLObject<DSHL7> LoadHL7SIULog(long MirthLogSIUId, string MrNumber, string PatientName, string Facility, string Provider, string MessageStatus, string ErrorMessage, string FileName, string CreatedOn , string appStatus, int PageNumber = 1, int RowsPerPage = 15)
        {
            try
            {
                DSHL7 ds = new DSHL7();
                ds = new DALHL7SIU().LoadHL7SIULog(MirthLogSIUId, MrNumber, PatientName, Facility, Provider, MessageStatus, ErrorMessage, FileName, CreatedOn, appStatus, PageNumber, RowsPerPage);
                return new BLObject<DSHL7>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminHL7::LoadHL7SIULog", ex);
                return new BLObject<DSHL7>(null, ex.Message);
            }
        }
        public BLObject<DSHL7> LoadHL7ADTLog(long MirthLogADTId, string MrNumber, string PatientName, string Facility, string Provider, string MessageStatus, string ErrorMessage, string FileName, string CreatedOn, string InsCompanyName, int PageNumber = 1, int RowsPerPage = 15)
        {
            try
            {
                DSHL7 ds = new DSHL7();
                ds = new DALHL7ADT().LoadHL7ADTLog(MirthLogADTId, MrNumber,PatientName, Facility, Provider, MessageStatus, ErrorMessage, FileName, CreatedOn,InsCompanyName, PageNumber, RowsPerPage);
                return new BLObject<DSHL7>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminHL7::LoadHL7ADTLog", ex);
                return new BLObject<DSHL7>(null, ex.Message);
            }
        }
        public BLObject<DSHL7> LoadHL7DFTLog(long MirthLogADTId, string MrNumber, string PatientName, string Facility, string Provider, string MessageStatus, string ErrorMessage, string FileName, string CreatedOn, string InsCompanyName, int PageNumber = 1, int RowsPerPage = 15)
        {
            try
            {
                DSHL7 ds = new DSHL7();
                ds = new DALHL7DFT().LoadHL7DFTLog(MirthLogADTId, MrNumber, PatientName, Facility, Provider, MessageStatus, ErrorMessage, FileName, CreatedOn, InsCompanyName, PageNumber, RowsPerPage);
                return new BLObject<DSHL7>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminHL7::LoadHL7DFTLog", ex);
                return new BLObject<DSHL7>(null, ex.Message);
            }
        }

        public BLObject<DSHL7> Load_Patient_Provider_Appointment_Visit(long patientID, long appointmentID)
        {
            try
            {
                DSHL7 ds = new DSHL7();
                ds = new DALHL7().Load_Patient_Provider_Appointment_Visit(patientID, appointmentID);
                return new BLObject<DSHL7>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminHL7::Load_Patient_Provider_Appointment_Visit", ex);
                return new BLObject<DSHL7>(null, ex.Message);
            }
        }

        public BLObject<DSHL7> InsertHl7PatientMessage(DSHL7 ds)
        {
            try
            {
                ds = new DALHL7().InsertHl7PatientMessage(ds);
                return new BLObject<DSHL7>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLSection::InsertSection", ex);
                return new BLObject<DSHL7>(null, ex.Message);
            }
        }

        public BLObject<DSHL7> Load_HL7_Error_ACK(long patientID)
        {
            try
            {
                DSHL7 ds = new DSHL7();
                ds = new DALHL7().Load_HL7_Error_ACK(patientID);
                return new BLObject<DSHL7>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminHL7::Load_HL7_Error_ACK", ex);
                return new BLObject<DSHL7>(null, ex.Message);
            }
        }




    
    }
}
