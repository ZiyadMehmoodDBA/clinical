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
    public class Patient_Guarantor
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_Guarantor()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_Guarantor _obj = null;
        public static Patient_Guarantor Instance()
        {
            if (_obj == null)
                _obj = new Patient_Guarantor();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the guarantor.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="GuarantorID">The guarantor identifier.</param>
        /// <returns></returns>
        private string LoadGuarantor(string fieldsJSON, Int64 GuarantorID, int PageNumber, int RowsPerPage)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatientProfile dsProfile = null;
                BLObject<DSPatientProfile> obj;
                if (SearchedfieldsJSON == null)
                    obj = BLLPatientObj.LoadGuarantor(GuarantorID, null, null, null, null);
                else
                    obj = BLLPatientObj.LoadGuarantor(GuarantorID, SearchedfieldsJSON["txtFirstName"], SearchedfieldsJSON["txtLastName"], SearchedfieldsJSON["ddlRelation"], SearchedfieldsJSON["chkIsActice"],0, PageNumber, RowsPerPage);
                dsProfile = obj.Data;
                if (dsProfile.Tables[dsProfile.Guarantor.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        GuarantorCount = dsProfile.Tables[dsProfile.Guarantor.TableName].Rows.Count > 0 ? dsProfile.Tables[dsProfile.Guarantor.TableName].Rows.Count : 0,
                        iTotalDisplayRecords = dsProfile.Guarantor.Rows[0][dsProfile.Guarantor.RecordCountColumn.ColumnName],
                        GuarantorLoad_JSON = MDVUtility.JSON_DataTable(dsProfile.Tables[dsProfile.Guarantor.TableName]),
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        GuarantorCount = 0,
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
        /// Handle the Guarantor Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_GUARANTOR":
                    {
                        string fieldsJSON = context.Request["GuarantorData"];
                        Int64 GuarantorID = MDVUtility.ToInt64(context.Request["GuarantorID"]);
                        int PageNumber = MDVUtility.ToInt(context.Request["PageNumber"]);
                        int RowsPerPage = MDVUtility.ToInt(context.Request["RowsPerPage"]);
                        string strJSONData = LoadGuarantor(fieldsJSON, GuarantorID, PageNumber, RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}