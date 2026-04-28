using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Model.Clinical.FollowUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates.OrderSet
{
    public class OS_FollowUp
    {
        private BLLOrderSet BLLOrderSetObj = null;
        public OS_FollowUp()
        {
            BLLOrderSetObj = new BLLOrderSet();
        }
        private static OS_FollowUp _instance = null;
        public static OS_FollowUp Instance()
        {
            if (_instance == null)
                _instance = new OS_FollowUp();
            return _instance;
        }

        #region " Order Set FollowUp "

        public string insertOrderSetFollowUp(OrdertSetFollowUpModel model)
        {
            OrdertSetFollowUpResponse responseModel = new OrdertSetFollowUpResponse();
            try
            {
                BLObject<string> obj = BLLOrderSetObj.insertOrderSetFollowUp(model);
                if (obj.Data != null && obj.Data != "-1")
                {
                    responseModel.FollowUpId = obj.Data.ToString();
                    responseModel.status = true;
                    responseModel.Message = Common.AppPrivileges.Save_Message;
                }
                else
                {
                    responseModel.status = false;
                    responseModel.FollowUpId = model.FollowUpId;
                    responseModel.Message = obj.Message == "" ? "Order Set FollowUp already exist" : obj.Message;
                }


            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.FollowUpId = model.FollowUpId;

            }
            if (responseModel.status)
            {
                var response = new
                {
                    status = true,
                    OrderSetReferralId = responseModel.FollowUpId,
                    Message = Common.AppPrivileges.Save_Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = responseModel.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string updateOrderSetFollowUp(OrdertSetFollowUpModel model)
        {
            try
            {
                BLObject<string> obj = BLLOrderSetObj.updateOrderSetFollowUp(model);
                if (obj.Data != null && obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message == "" ? "FormName Already Exists" : obj.Message
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

        public string loadOrderSetFollowUp(OrdertSetFollowUpModel model)
        {
            try
            {
                List<OrdertSetFollowUpModel> listOrderSet = new List<OrdertSetFollowUpModel>();
                BLObject<List<OrdertSetFollowUpModel>> obj = BLLOrderSetObj.loadOrderSetFollowUp(MDVUtility.ToInt64(model.FollowUpId), MDVUtility.ToInt64(model.OrderSetId), MDVUtility.ToInt64(model.PageNumber), MDVUtility.ToInt64(model.RowspPage));

                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        iTotalDisplayRecords = MDVUtility.ToLong(obj.Data[0].RecordCount),
                        OrderSetFollowUpJSON = obj.Data,
                        OrderSetFollowUpCount = obj.Data.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        OrderSetFollowUpJSON = "[]",
                        OrderSetFollowUpCount = 0,
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

        public string deleteOrderSetFollowUp(string followUpId)
        {
            try
            {
                if (string.IsNullOrEmpty(followUpId))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLOrderSetObj.deleteOrderSetFollowUp(followUpId);
                    if (obj.Data != null && obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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
    }
}