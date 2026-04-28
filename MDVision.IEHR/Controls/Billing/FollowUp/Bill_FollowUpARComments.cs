using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Model.Billing.FollowUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Billing.FollowUp
{
    public class Bill_FollowUpARComments
    {
        private BLLAdminFollowUp BLLBillingFollowUpObj = null;
        public Bill_FollowUpARComments()
        {
            BLLBillingFollowUpObj = new BLLAdminFollowUp();
        }

        #region Singleton
        private static Bill_FollowUpARComments _obj = null;
        public static Bill_FollowUpARComments Instance()
        {
            if (_obj == null)
                _obj = new Bill_FollowUpARComments();
            return _obj;
        }
        

        #region Private Functions
        private string GetFollowUpComments(Int64 FollowUpCommentId,Int64 VisitId)
        {
            try
            {
                List<FollowUpARComments> objList = BLLBillingFollowUpObj.GetFollowUpComments(FollowUpCommentId,VisitId);
                if (objList.Count>=0)
                {
                    var response = new
                    {
                        status = true,
                        FollowUpCommentInfo = objList,
                        FollowUpCommentInfoCount = objList.Count
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Unable to Get Comment List"
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
        private string SaveFollowUpComments(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                FollowUpARComments model = new FollowUpARComments();
                model.followUpComments = MDVUtility.ToStr(SearchedfieldsJSON["txtFollowUpArComments"]);
                model.VisitId = MDVUtility.ToLong(SearchedfieldsJSON["hfVisitId"]);
                model.IsFromClaim = MDVUtility.ToBool(SearchedfieldsJSON["hfIsFromClaim"]);
                model.IsDeleted = false;
                #region Database Insertion
                
                BLObject<string> obj = BLLBillingFollowUpObj.InsertNewFollowUpComments(model);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
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
        private string UpdateFollowUpComments(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                FollowUpARComments model = new FollowUpARComments();
                model.Id = MDVUtility.ToLong(SearchedfieldsJSON["hfFollowUpCommentId"]);
                model.followUpComments = MDVUtility.ToStr(SearchedfieldsJSON["txtFollowUpArComments"]);
                model.VisitId = MDVUtility.ToLong(SearchedfieldsJSON["hfVisitId"]);
                model.IsFromClaim = MDVUtility.ToBool(SearchedfieldsJSON["hfIsFromClaim"]);
                model.IsDeleted = false;
                #region Database Insertion

                BLObject<string> obj = BLLBillingFollowUpObj.UpdateNewFollowUpComments(model);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
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

        private string DeleteFollowUpComments(Int64 FollowUpCommentId)
        {
            try
            {
                BLObject<string> obj = BLLBillingFollowUpObj.DeleteNewFollowUpComments(FollowUpCommentId);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
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
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "SAVE_FOLLOWUP_AR_COMMENTS":
                    {
                        string JSONData = context.Request["Comment_Data"];
                        string strJsonData = SaveFollowUpComments(JSONData);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "GET_FOLLOWUP_AR_COMMENTS":
                    {
                        Int64 VisitId = MDVUtility.ToLong(context.Request["VisitId"]);
                        Int64 FollowUpCommentId =MDVUtility.ToLong( context.Request["FollowUpCommentId"]);
                        string strJsonData = GetFollowUpComments(FollowUpCommentId, VisitId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "UPDATE_FOLLOWUP_AR_COMMENTS":
                    {
                        string JSONData = context.Request["Comment_Data"];
                        string strJsonData = UpdateFollowUpComments(JSONData);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "DELETE_FOLLOWUP_AR_COMMENTS":
                    {
                        Int64 FollowUpCommentId =MDVUtility.ToLong(context.Request["FollowUpCommentId"]);
                        string strJsonData = DeleteFollowUpComments(FollowUpCommentId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

            }
        }
        #endregion

    }
}