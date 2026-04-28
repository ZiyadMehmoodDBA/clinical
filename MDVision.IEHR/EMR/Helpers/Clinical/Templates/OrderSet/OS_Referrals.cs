using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Model.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates.OrderSet
{
    public class OS_Referrals
    {
        private BLLOrderSet BLLOrderSetObj = null;
        public OS_Referrals()
        {
            BLLOrderSetObj = new BLLOrderSet();
        }
        private static OS_Referrals _instance = null;
        public static OS_Referrals Instance()
        {
            if (_instance == null)
                _instance = new OS_Referrals();
            return _instance;
        }

        #region " Order Set Patient Referrals Outgoing Detail "

        public string insertOrderSetPatientReferralsOutgoingDetail(OrderSetPatientReferralModel model)
        {
            OrderSetPatientReferralResponse responseModel = new OrderSetPatientReferralResponse();
            try
            {
                BLObject<string> obj = BLLOrderSetObj.insertOrderSetPatientReferralsOutgoingDetail(model);
                if (obj.Data != null && obj.Data != "-1")
                {
                    responseModel.OrderSetReferralId = obj.Data.ToString();
                    responseModel.status = true;
                    responseModel.Message = Common.AppPrivileges.Save_Message;
                }
                else
                {
                    responseModel.status = false;
                    responseModel.OrderSetReferralId = model.OrderSetReferralId;
                    responseModel.Message = obj.Message == "" ? "Order Set Patient Referral already exist" : obj.Message;
                }


            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.OrderSetReferralId = model.OrderSetReferralId;

            }
            if (responseModel.status)
            {
                var response = new
                {
                    status = true,
                    OrderSetReferralId = responseModel.OrderSetReferralId,
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

        public string updateOrderSetPatientReferralsOutgoingDetail(OrderSetPatientReferralModel model)
        {
            try
            {
                BLObject<string> obj = BLLOrderSetObj.updateOrderSetPatientReferralsOutgoingDetail(model);
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

        public string loadOrderSetPatientReferralsOutgoingDetail(OrderSetPatientReferralModel model)
        {
            try
            {
                List<OrderSetPatientReferralModel> listOrderSet = new List<OrderSetPatientReferralModel>();
                BLObject<List<OrderSetPatientReferralModel>> obj = BLLOrderSetObj.loadOrderSetPatientReferralsOutgoingDetail(MDVUtility.ToInt64(model.OrderSetReferralId), MDVUtility.ToInt64(model.OrderSetId), null, null, 0, 0, null, null, 0, null, null);
                
                if (obj.Data != null && obj.Data.Count != 0)
                {

                    
                    BLObject<List<ReferralProcedureModel>> obj1 = BLLOrderSetObj.loadOrderSetReferralsProcedure(MDVUtility.ToInt64(model.OrderSetReferralId));
                    if ((!string.IsNullOrEmpty(model.OrderSetReferralId)) && obj1.Data != null && obj1.Data.Count != 0)
                    {
                        var response = new
                        {
                            status = true,
                            OrderSetReferralProcedureJSON = obj1.Data,
                            OrderSetReferralProcedureCount = obj1.Data.Count,
                            OrderSetReferralsJSON = obj.Data,
                            OrderSetReferralsCount = obj.Data.Count,
                            iTotalDisplayRecords = obj.Data[0].RecordCount
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            OrderSetReferralProcedureJSON = "[]",
                            OrderSetReferralProcedureCount = 0,
                            OrderSetReferralsJSON = obj.Data,
                            OrderSetReferralsCount = obj.Data.Count,
                            iTotalDisplayRecords = 0,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        OrderSetReferralsJSON = "[]",
                        OrderSetReferralsCount = 0,
                        OrderSetReferralProcedureJSON = "[]",
                        OrderSetReferralProcedureCount = 0,
                        iTotalDisplayRecords = 0,
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

        public string deleteOrderSetPatientReferralsOutgoingDetail(string OrderSetReferralId)
        {
            try
            {
                if (string.IsNullOrEmpty(OrderSetReferralId))
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
                    BLObject<string> obj = BLLOrderSetObj.deleteOrderSetPatientReferralsOutgoingDetail(OrderSetReferralId);
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

        public string deleteOrderSetReferralsProcedure(string OrderSetReferralProcedureId)
        {
            try
            {
                if (string.IsNullOrEmpty(OrderSetReferralProcedureId))
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
                    BLObject<string> obj = BLLOrderSetObj.deleteOrderSetReferralsProcedure(OrderSetReferralProcedureId);
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