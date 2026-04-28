using System;
using System.ComponentModel;
using System.Diagnostics;
using MDVision.Business.BCommon;
using MDVision.DataAccess.DAL.PQRS;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using MDVision.Common.Logging;

namespace MDVision.Business.BLL
{
    public class BLLPQRS
    {

        #region Variable

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLAdminEDI"/> class.
        /// </summary>
        public BLLPQRS()
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
        #region MeasureGroup
        public BLObject<DSPQRS> loadMeasureGroup(string MeasureGroupName, string ProviderIds, Int32 PageNumber = 1, Int32 RowsPerPage = 15, string IsActive = "1")
        {
            try
            {
                var ds = new DALMeasureGroup().LoadMeasureGroup(MeasureGroupName, ProviderIds, PageNumber, RowsPerPage, IsActive);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::LoadMeasureGroup", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> fillMeasureGroup(Int64 MeasureGroupId)
        {
            try
            {
                var ds = new DALMeasureGroup().FillMeasureGroup(MeasureGroupId);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::fillMeasureGroup", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> UpdateMeasureGroup(DSPQRS ds)
        {
            try
            {
                ds = new DALMeasureGroup().UpdateMeasureGroup(ds);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::UpdateMeasureGroup", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> insertMeasureGroup(DSPQRS ds)
        {
            try
            {
                ds = new DALMeasureGroup().InsertMeasureGroup(ds);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::InsertMeasureGroup", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }
        public BLObject<string> deleteMeasureGroup(Int64 measureGroupId)
        {
            try
            {
                if (measureGroupId > 0)
                {
                    return new BLObject<string>(new DALMeasureGroup().DeleteMeasureGroup(measureGroupId));
                }
                else
                {
                    return new BLObject<string>("Please select Record");
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::deleteMeasureGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion
        #region Measures

        public BLObject<DSPQRS> fillMeasures(Int64? MeasuresId = null)
        {
            try
            {
                var ds = new DALMeasureGroup().FillMeasures(MeasuresId);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::FillMeasures", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }
        #endregion

        #region  Individual Reportings
        public BLObject<DSPQRS> loadMeasureIndividual(Int64 IndividualReportingId, Int64 ProviderId, Int64 SpecialtyId, Int32 PageNumber = 1, Int32 RowsPerPage = 15, string IsActive = "1")
        {
            try
            {
                var ds = new DALMeasureIndividual().LoadMeasureIndividual(IndividualReportingId, ProviderId, SpecialtyId, PageNumber, RowsPerPage, IsActive);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::loadMeasureIndividual", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> fillMeasureIndividual(Int64 MeasureIndividualId)
        {
            try
            {
                var ds = new DALMeasureIndividual().FillMeasureIndividual(MeasureIndividualId);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::fillMeasureIndividual", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> fillMeasureGroupDetails(Int64 MeasureGrouplId)
        {
            try
            {
                var ds = new DALMeasureIndividual().FillMeasureGroup(MeasureGrouplId);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::fillMeasureGroupDetails", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> fillMeasureGroupData(string MeasureGroupId, string Providers, string IsActive, string PageNumber, string RowspPage)
        {
            try
            {
                var ds = new DALMeasureIndividual().FillMeasureGroupData( MeasureGroupId,  Providers,  IsActive,  PageNumber,  RowspPage);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::FillMeasureGroupData", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> UpdateMeasureIndividual(DSPQRS ds)
        {
            try
            {
                ds = new DALMeasureIndividual().UpdateMeasureIndividual(ds);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::UpdateMeasureIndividual", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> UpdateMeasureGroupData(DSPQRS ds)
        {
            try
            {
                ds = new DALMeasureIndividual().UpdateMeasureGroupData(ds);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::UpdateMeasureGroupData", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> insertMeasureIndividual(DSPQRS ds)
        {
            try
            {
                ds = new DALMeasureIndividual().InsertMeasureIndividual(ds);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::InsertMeasureIndividual", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> insertMeasureGroupData(DSPQRS ds)
        {
            try
            {
                ds = new DALMeasureIndividual().InsertMeasureGroup(ds);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::InsertMeasureGroup", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> insertPatientList(DSPQRS ds)
        {
            try
            {
                ds = new DALMeasureIndividual().InsertPatientList(ds);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::insertPatientList", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<string> deleteMeasureIndividual(Int64 measureGroupId)
        {
            try
            {
                if (measureGroupId > 0)
                {
                    return new BLObject<string>(new DALMeasureIndividual().DeleteMeasureIndividual(measureGroupId));
                }
                else
                {
                    return new BLObject<string>("Please select Record");
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::deleteMeasureIndividual", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        public BLObject<string> deleteMeasureGroupData(string measureGroupId)
        {
            try
            {
                if (! string.IsNullOrEmpty( measureGroupId))
                {
                    return new BLObject<string>(new DALMeasureIndividual().DeleteMeasureGroupData(measureGroupId));
                }
                else
                {
                    return new BLObject<string>("Please select Record");
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::deleteMeasureGroupData", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region Measure Provider Group Lookup

        public BLObject<DSPQRS> LookupMeasureProviderGroup()
        {
            try
            {
                DSPQRS ds = new DSPQRS();
                ds = new DALMeasureGroup().LookupMeasureProviderGroup();
                return new BLObject<DSPQRS>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::LookupMeasureProviderGroup", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }

        }
        #endregion

        #region PQRS Reports
        public BLObject<DSPQRS> generateIndividualReport(long ProviderId, string DateFrom, string DateTo)
        {
            try
            {
                DSPQRS ds = new DSPQRS();
                ds = new DALPQRSReports().generateIndividualReport(ProviderId, DateFrom, DateTo);
                return new BLObject<DSPQRS>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::generateIndividualReport", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> generateGPROReport(long ProviderGroupId, string DateFrom, string DateTo)
        {
            try
            {
                DSPQRS ds = new DSPQRS();
                ds = new DALPQRSReports().generateGPROReport(ProviderGroupId, DateFrom, DateTo);
                return new BLObject<DSPQRS>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::generateGPROReport", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> generateSummaryReport(long ProviderId, long ProviderGroupId)
        {
            try
            {
                DSPQRS ds = new DSPQRS();
                ds = new DALPQRSReports().generateSummaryReport(ProviderId, ProviderGroupId);
                return new BLObject<DSPQRS>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::generateSummaryReport", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        //
        public BLObject<DSPQRS> generateIndividualReportQRDA(int measureId, long ProviderId, string DateFrom, string DateTo)
        {
            try
            {
                DSPQRS ds = new DSPQRS();
                ds = new DALPQRSReports().generateIndividualReport(ProviderId, DateFrom, DateTo);
                return new BLObject<DSPQRS>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::generateIndividualReport", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        public BLObject<DSPQRS> getPatientsFromVisits(string visitIds)
        {
            try
            {
                var ds = new DALPQRSReports().getPatientsFromVisits(visitIds);
               return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::getPatientsFromVisits", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }
        public BLObject<DSPQRS> getMeasureReasons(int measureId)
        {
            try
            {
                var ds = new DALPQRSReports().getMeasureReasons(measureId);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::getMeasureReasons", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

        #endregion
        #region QRDA
        public BLObject<DSPQRS> Load_PQRS(long providerId, string from, string to, string patientId = null, long reportType = 0, int measureID = -1, int eitherDetail = 0)
        {
            try
            {
                var ds = new DALPQRSexport().Load_PQRS(providerId, from, to, patientId, reportType, measureID, eitherDetail);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::Load_PQRS", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }
        public BLObject<DSPQRS> Load_PQRS_Details(long providerId, string from, string to, string patientId = null, long reportType = 1, int measureID = -1, int eitherDetail = 0)
        {
            try
            {
                var ds = new DALPQRSexport().Load_PQRS_Details(providerId, from, to, patientId, reportType, measureID, eitherDetail);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::Load_PQRS_Details", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }
        public BLObject<DSPQRS> Load_PQRS_Codes(long providerId, string from, string to, string patientId = null, long reportType = 2, int measureID = -1, int eitherDetail = 0)
        {
            try
            {
                var ds = new DALPQRSexport().Load_PQRS_Codes(providerId, from, to, patientId, reportType, measureID, eitherDetail);
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::Load_PQRS_Codes", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }
        public BLObject<DSPQRS> FillPatientById(long patientId)
        {
            try
            {
                var ds = new DALPQRSexport().FillPatient(patientId, "", "");
                ds.AcceptChanges();
                return new BLObject<DSPQRS>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPQRS::FillPatientById", ex);
                return new BLObject<DSPQRS>(null, ex.Message);
            }
        }

      

        #endregion
    }
}
