using MDVision.Business.BCommon;
using MDVision.DataAccess.DAL.ReportHeader;
using MDVision.DataAccess.DCommon; 
using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.Business.BLL
{
    public class BLLAdminClinical
    {
        #region Variable
        
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLAdminEDI"/> class.
        /// </summary>
        public BLLAdminClinical()
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
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new Container();
        }
        #endregion

        #region ReportHeader
        public BLObject<DSReportHeader> loadReportHeader(string ReportHeaderName, string SpecialtyIds, string ProviderIds, string FacilityIds, Int32 PageNumber, Int32 RowsPerPage, string IsActive)
        {
            try
            {
                var ds = new DALReportHeader().LoadReportHeader(ReportHeaderName, SpecialtyIds, ProviderIds,  FacilityIds,  PageNumber, RowsPerPage, IsActive);
                return new BLObject<DSReportHeader>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminClinical::LoadReportHeader", ex);
                return new BLObject<DSReportHeader>(null, ex.Message);
            }
        }

        public BLObject<DSReportHeader> fillReportHeader(Int64 ReportHeaderId)
        {
            try
            {
                var ds = new DALReportHeader().FillReportHeader(ReportHeaderId);
                return new BLObject<DSReportHeader>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminClinical::fillReportHeader", ex);
                return new BLObject<DSReportHeader>(null, ex.Message);
            }
        }

        public BLObject<DSReportHeader> UpdateReportHeader(DSReportHeader ds)
        {
            try
            {
                ds = new DALReportHeader().UpdateReportHeader(ds);
                return new BLObject<DSReportHeader>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminClinical::UpdateReportHeader", ex);
                return new BLObject<DSReportHeader>(null, ex.Message);
            }
        }

        public BLObject<DSReportHeader> insertReportHeader(DSReportHeader ds)
        {
            try
            {
                ds = new DALReportHeader().InsertReportHeader(ds);
                return new BLObject<DSReportHeader>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminClinical::InsertReportHeader", ex);
                return new BLObject<DSReportHeader>(null, ex.Message);
            }
        }
        public BLObject<string> deleteReportHeader(Int64 reportHeaderId)
        {
            try
            {
                if (reportHeaderId > 0)
                {
                    return new BLObject<string>(new DALReportHeader().DeleteReportHeader(reportHeaderId));
                }
                else
                {
                    return new BLObject<string>("Please select Record");
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminClinical::deleteReportHeader", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> UpdateReportHeaderActiveInactive(Int64 reportHeaderId, bool IsActive)
        {
            try
            {
                if (reportHeaderId > 0)
                {
                    return new BLObject<string>(new DALReportHeader().UpdateReportHeaderActiveInactive(reportHeaderId, IsActive));
                }
                else
                {
                    return new BLObject<string>("Please select Record");
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminClinical::UpdateReportHeaderActiveInactive", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSReportHeader> LookupReportHeader(long providerId)
        {
            try
            {
                DSReportHeader ds = new DSReportHeader();
                ds = new DALReportHeader().LookupReportHeader(providerId);

                return new BLObject<DSReportHeader>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminClinical::LookupReportHeader", ex);
                return new BLObject<DSReportHeader>(null, ex.Message);
            }
        }

        public BLObject<DSReportHeader> LoadReportHeaderConfiguration()
        {
            try
            {
                var ds = new DALReportHeader().LoadReportHeaderConfiguration();
                return new BLObject<DSReportHeader>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminClinical::LoadReportHeaderConfiguration", ex);
                return new BLObject<DSReportHeader>(null, ex.Message);
            }
        }

        public BLObject<DSReportHeader> UpdateReportHeaderSettings(DSReportHeader ds)
        {
            try
            {
                ds = new DALReportHeader().UpdateReportHeaderSettings(ds);
                return new BLObject<DSReportHeader>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminClinical::InsertUpdateReportHeaderSettings", ex);
                return new BLObject<DSReportHeader>(null, ex.Message);
            }
        }

        public BLObject<DSReportHeader> LoadReportHeaderSettings(long reportHeaderId)
        {
            try
            {
                var ds = new DALReportHeader().LoadReportHeaderSettings(reportHeaderId);
                return new BLObject<DSReportHeader>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminClinical::LoadReportHeaderSettings", ex);
                return new BLObject<DSReportHeader>(null, ex.Message);
            }
        }
        public BLObject<DSReportHeader> getReportHeaderTagsValue(long PatientId, long ProviderId, long NotesId, string formName, string PreviewStyle = null)
        {
            try
            {
                var ds = new DALReportHeader().getReportHeaderTagsValue(PatientId, ProviderId, NotesId, formName, PreviewStyle);
                return new BLObject<DSReportHeader>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminClinical::getReportHeaderTagsValue", ex);
                return new BLObject<DSReportHeader>(null, ex.Message);
            }
        }
        #endregion
    }
}
