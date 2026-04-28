using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MDVision.Datasets;
using MDVision.DataAccess;
using MDVision.Common.Shared;

namespace MDVision.DataAccess.DAL.Admin
{
    public class example
    {
        #region Variable
        #endregion
        #region " Stored Procedure Names"
        private const string PROC_INSERT = "";
        private const string PROC_UPDATE = "";
        private const string PROC_DELETE = "";
        private const string PROC_SELECT = "";


        #endregion

        #region "Query "
        private const string QRY_SELECT = "select * from table name";
        #endregion

        #region "Parameters"
        private const string PARM_ID = "@ID";
        private const string PARM_FNAME = "@FName";
        private const string PARM_LNAME = "@LName";


        public struct Parameters
        {
            public int ID;
            public string FNAME;
            public string LNAME;
        }

        #endregion

        //#region Singleton
        //private static DALpersonnel _instance = null;
        ///// <summary>
        ///// Singleton context
        ///// </summary>
        //public static DALpersonnel Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = new DALpersonnel();
        //        return _instance;
        //    }
        //}
        //#endregion

        //#region Initialize
        //// private DALpersonnel() { }
        //#endregion

        #region "Support Functions"

       
        #endregion

        #region "Insert, delete, update and get using dataset Functions"


        #endregion
    }
}
