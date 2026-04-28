using MDVision.Business.BLL;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.PQRSReports.Schema
{
    public class PqrsQrdaOneXMLDataExtraction
    {
        private BLLPQRS BLLPQRSObj = null;

        public PqrsQrdaOneXMLDataExtraction()
        {
            BLLPQRSObj = new BLLPQRS();

        }

        #region DataSets

        static DSPQRS _dsPatientDemoGraphic;
        static DSProfile _dsProvider;
        static DSProfile _dsPractice;

        #endregion

        #region Patient



        protected static DSPQRS GetPatient_QrdaOne(Int64 patientId)
        {
            var obj = new BLLPQRS().FillPatientById(patientId);
            _dsPatientDemoGraphic = obj.Data;
            return _dsPatientDemoGraphic;
        }
        protected static DSPQRS GetPatient_QrdaOne(int measureId, Int64 providerId, string startDate, string endDate)
        {
            var obj = new BLLPQRS().Load_PQRS_Details(providerId, startDate, endDate, null, 1, measureId);
            //var obj = BusinessWrapper.CQM.BusinessObj.FillPatientById(patientId);
            _dsPatientDemoGraphic = obj.Data;
            return _dsPatientDemoGraphic;
        }

        #endregion

        #region Provider

        protected static DSProfile GetProvider_QrdaOne(Int64 providerId)
        {
            var obj = new BLLAdminProfile().LoadProvider(providerId, null, null, null, null, null, null, null);
            _dsProvider = obj.Data;
            return _dsProvider;
        }

        #endregion

        #region Practice

        protected static DSProfile GetPractice_QrdaOne(Int64 practiceId)
        {
            var obj = new BLLAdminProfile().LoadPractice(practiceId, null, null, null, null, null);
            _dsPractice = obj.Data;
            return _dsPractice;
        }

        #endregion
    }
}