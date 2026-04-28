/* Author: Wasim Malik
 * OverView: Helper for Complaints
 * Date : Feb 10, 2016
 */


using MDVision.Datasets;
using MDVision.Business.BCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using System.Text;
using MDVision.IEHR.EMR.Model.Medical;
namespace MDVision.IEHR.EMR.Helpers.Clinical.Medical
{
    public class ComplaintHelper
    {
        private static ComplaintHelper _instance = null;
        public static ComplaintHelper Instance()
        {
            if (_instance == null)
                _instance = new ComplaintHelper();
            return _instance;
        }
        

    }
}