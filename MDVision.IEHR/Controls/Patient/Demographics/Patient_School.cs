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

namespace MDVision.IEHR.Controls.Patient.Demographics
{
    public class Patient_School
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_School()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_School _obj = null;
        public static Patient_School Instance()
        {
            if (_obj == null)
                _obj = new Patient_School();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the school.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="SchoolID">The school identifier.</param>
        /// <returns></returns>
        private string LoadSchool(string fieldsJSON, Int64 SchoolID)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatientProfile dsProfile = null;
                BLObject<DSPatientProfile> obj;
                if (SearchedfieldsJSON == null)
                    obj = BLLPatientObj.LoadSchool(SchoolID, null, null, null, null, null, null);
                else
                    obj = BLLPatientObj.LoadSchool(SchoolID, SearchedfieldsJSON["txtName"], SearchedfieldsJSON["txtAddress"], SearchedfieldsJSON["txtCity"], SearchedfieldsJSON["txtState"], SearchedfieldsJSON["txtZip"], SearchedfieldsJSON["chkIsActice"]);
                dsProfile = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        SchoolCount = dsProfile.Tables[dsProfile.School.TableName].Rows.Count > 0 ? dsProfile.Tables[dsProfile.School.TableName].Rows.Count : 0,
                        SchoolLoad_JSON = MDVUtility.JSON_DataTable(dsProfile.Tables[dsProfile.School.TableName]),
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        SchoolCount = 0,
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
        /// Handle the School Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_SCHOOL":
                    {
                        string fieldsJSON = context.Request["SchoolData"];
                        Int64 SchoolID = MDVUtility.ToInt64(context.Request["SchoolID"]);
                        string strJSONData = LoadSchool(fieldsJSON, SchoolID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}