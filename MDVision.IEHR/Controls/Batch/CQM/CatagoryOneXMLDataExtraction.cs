using System;
using System.Collections.Generic;
using MDVision.Datasets;

using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Batch.CQM
{
    public class CatagoryOneXMLDataExtraction //: Batch_ClinicalQualityMeasure
    {
        private BLLCQM BLLCQMObj = null;
       
        public CatagoryOneXMLDataExtraction()
        {
            BLLCQMObj = new BLLCQM();

        }

        #region DataSets

        static DSCQM _dsPatientDemoGraphic;
        static DSProfile _dsProvider;
        static DSProfile _dsPractice;

        #endregion

        #region Patient



        protected static DSCQM GetPatient_CategoryOne(Int64 patientId)
        {
            var obj = new BLLCQM().FillPatientById(patientId);
            _dsPatientDemoGraphic = obj.Data;
            return _dsPatientDemoGraphic;
        }
        protected static DSCQM GetPatient_CategoryOne(string cqmid, Int64 providerId, string startDate, string endDate,bool isC1 = false)
        {
            var obj = new BLLCQM().Load_CQM_Details(providerId, startDate, endDate, null, 1, cqmid,0,null,0,null,null,null,null,null,0,null,null,null, isC1);
            //var obj = BusinessWrapper.CQM.BusinessObj.FillPatientById(patientId);
            _dsPatientDemoGraphic = obj.Data;
            return _dsPatientDemoGraphic;
        }

        #endregion

        #region Provider

        protected static DSProfile GetProvider_CategoryOne(Int64 providerId)
        {
            var obj = new BLLAdminProfile().LoadProvider(providerId, null, null, null, null, null, null, null);
            _dsProvider = obj.Data;
            return _dsProvider;
        }

        #endregion

        #region Practice

        protected static DSProfile GetPractice_CategoryOne(Int64 practiceId)
        {
            var obj = new BLLAdminProfile().LoadPractice(practiceId, null, null, null, null, null);
            _dsPractice = obj.Data;
            return _dsPractice;
        }

        #endregion

    }
}