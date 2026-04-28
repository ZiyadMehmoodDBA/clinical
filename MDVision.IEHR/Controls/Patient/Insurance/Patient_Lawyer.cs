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
    public class Patient_Lawyer
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_Lawyer()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_Lawyer _obj = null;
        public static Patient_Lawyer Instance()
        {
            if (_obj == null)
                _obj = new Patient_Lawyer();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the lawyer.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="LawyerID">The lawyer identifier.</param>
        /// <returns></returns>
        private string LoadLawyer(string fieldsJSON, Int64 LawyerID)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatientProfile dsProfile = null;
                BLObject<DSPatientProfile> obj;
                if (SearchedfieldsJSON == null)
                    obj = BLLPatientObj.LoadLawyer(LawyerID, null, null, null, null, null);
                else
                    obj = BLLPatientObj.LoadLawyer(LawyerID, SearchedfieldsJSON["txtLawyerName"], SearchedfieldsJSON["txFirmName"], SearchedfieldsJSON["txtAddress"], SearchedfieldsJSON["txtCity"], SearchedfieldsJSON["chkIsActice"]);
                dsProfile = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        LawyerCount = dsProfile.Tables[dsProfile.Lawyer.TableName].Rows.Count > 0 ? dsProfile.Tables[dsProfile.Lawyer.TableName].Rows.Count : 0,
                        LawyerLoad_JSON = MDVUtility.JSON_DataTable(dsProfile.Tables[dsProfile.Lawyer.TableName]),
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        LawyerCount = 0,
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
        /// Handle the Lawyer Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_LAWYER":
                    {
                        string fieldsJSON = context.Request["LawyerData"];
                        Int64 LawyerID = MDVUtility.ToInt64(context.Request["LawyerID"]);
                        string strJSONData = LoadLawyer(fieldsJSON, LawyerID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}