using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_ParticipentProvider
    {
        private BLLAdminProfile BLLAdminProfileObj = null;
        public Admin_ParticipentProvider()
        {
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        #region Singleton
        private static Admin_ParticipentProvider _obj = null;
        public static Admin_ParticipentProvider Instance()
        {
            if (_obj == null)
                _obj = new Admin_ParticipentProvider();
            return _obj;
        }
        #endregion
        #region Private Functions
        /// <summary>
        /// Load all the specialities for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The specialty identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadProviderParticipant(string fieldsJSON, Int64 ProviderParticipantID,string ProviderId ,int PageNumber, int RowsPerPage)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSProfile dsProfile = null;
                BLObject<DSProfile> obj;
                if (SearchedfieldsJSON == null)
                {
                    obj = BLLAdminProfileObj.LoadProviderParticipant(ProviderParticipantID,  null, null, ProviderId);
                }
               
                else
                {
                    obj = BLLAdminProfileObj.LoadProviderParticipant(ProviderParticipantID,SearchedfieldsJSON["txtAssignment"], SearchedfieldsJSON["chkActive"] , ProviderId, PageNumber, RowsPerPage);
                }
                dsProfile = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ParticipantCount = dsProfile.Tables[dsProfile.ProviderParticipant.TableName].Rows.Count,
                        iTotalDisplayRecords = (dsProfile.ProviderParticipant.Rows.Count > 0) ? dsProfile.ProviderParticipant.Rows[0][dsProfile.ProviderParticipant.RecordCountColumn.ColumnName] : 0,
                        ParticipantProviderLoad_JSON = MDVUtility.JSON_DataTable(dsProfile.Tables[dsProfile.ProviderParticipant.TableName]),
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        SpecialtyCount = 0,
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
        /// Handle the Specialty Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            switch (cammandAction)
            {
                case "SEARCH_PARTICIPANT_PROVIDER":
                    {
                        string fieldsJSON = context.Request["ParticipantProviderData"];
                        Int64 ParticipantProviderID = MDVUtility.ToInt64(context.Request["ParticipantProviderID"]);
                        string ProviderId = MDVUtility.ToStr(context.Request["ProviderId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadProviderParticipant(fieldsJSON, ParticipantProviderID, ProviderId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}