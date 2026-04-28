using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Datasets;

namespace MDVision.IEHR.EMR.HL7Folder.Summary.CCDA
{
    public class DataExtraction
    {

        private BLLPatient BLLPatientObj = null;
        public DataExtraction()
        {
            BLLPatientObj = new BLLPatient();
        }

        static DSPatient _dsProvider = null;

        //protected static DSCCDA GetPatient_CategoryOne(DSPatient dsPatient)
        //{
        //    var obj = new BLLPatient.LoadPatient(dsPatient); 
        //    _dsPatientDemoGraphic = obj.Data;
        //    return _dsPatientDemoGraphic;
        //}
    }
}