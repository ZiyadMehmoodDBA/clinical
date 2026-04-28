using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Patient.Insurance
{
    public class Patient_Employer
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_Employer()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_Employer _obj = null;
        public static Patient_Employer Instance()
        {
            if (_obj == null)
                _obj = new Patient_Employer();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the employer.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EmployerID">The employer identifier.</param>
        /// <returns></returns>
        private string LoadEmployer(string fieldsJSON, Int64 EmployerID)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatientProfile dsProfile = null;
                BLObject<DSPatientProfile> obj;
                if (SearchedfieldsJSON == null)
                    obj = BLLPatientObj.LoadEmployer(EmployerID, null, null, null, null, null, null, "");
                else
                    obj = BLLPatientObj.LoadEmployer(EmployerID, SearchedfieldsJSON["txtName"], SearchedfieldsJSON["txtAddress"], SearchedfieldsJSON["txtCity"], SearchedfieldsJSON["txtState"], SearchedfieldsJSON["txtZip"], SearchedfieldsJSON["chkIsActice"], SearchedfieldsJSON["txtExt"]);
                dsProfile = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        EmployerCount = dsProfile.Tables[dsProfile.Employer.TableName].Rows.Count > 0 ? dsProfile.Tables[dsProfile.Employer.TableName].Rows.Count : 0,
                        EmployerLoad_JSON = MDVUtility.JSON_DataTable(dsProfile.Tables[dsProfile.Employer.TableName]),
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        EmployerCount = 0,
                        Message = obj.Message
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Employer Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_EMPLOYER":
                    {
                        string fieldsJSON = context.Request["EmployerData"];
                        Int64 EmployerID = MDVUtility.ToInt64(context.Request["EmployerID"]);
                        string strJSONData = LoadEmployer(fieldsJSON, EmployerID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}