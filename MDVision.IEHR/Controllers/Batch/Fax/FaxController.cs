using MDVision.Business.BLL;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MDVision.IEHR.Controllers.Batch.Fax
{
    public class FaxController : ApiController
    {
        private BLLBatchFax bllBatchFax;

        public FaxController()
        {
            bllBatchFax = new BLLBatchFax();
        }
        // GET: Fax

        [HttpPost]
        public HttpResponseMessage QueueFax(JObject AllData)
        {
            try
            {
                Dictionary<string, string> input = JsonConvert.DeserializeObject<Dictionary<string, string>>(MDVUtility.ToStr(AllData["data"]));
                return Request.CreateResponse(HttpStatusCode.OK, bllBatchFax.Queue_Fax(input));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetFaxInbox(JObject AllData)
        {
            try
            {
                Dictionary<string, string> input = JsonConvert.DeserializeObject<Dictionary<string, string>>(MDVUtility.ToStr(AllData["data"]));
                return Request.CreateResponse(HttpStatusCode.OK, bllBatchFax.Get_Fax_Inbox(input));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetFaxOutbox(JObject AllData)
        {
            try
            {
                Dictionary<string, string> input = JsonConvert.DeserializeObject<Dictionary<string, string>>(MDVUtility.ToStr(AllData["data"]));
                return Request.CreateResponse(HttpStatusCode.OK, bllBatchFax.GetFaxOutboxFromDB(input));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage RetrieveFax(JObject AllData)
        {
            try
            {
                Dictionary<string, string> input = JsonConvert.DeserializeObject<Dictionary<string, string>>(MDVUtility.ToStr(AllData["data"]));
                string result = bllBatchFax.Retrieve_Fax(input);
                if (bllBatchFax.Get_Last_Status())
                {
                    Task.Factory.StartNew(() => bllBatchFax.Update_Viewed_Status(input));
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteFax(JObject AllData)
        {
            try
            {
                string privilegesMessage = string.Empty;
                string strJSONData = string.Empty;
                privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Fax", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    Dictionary<string, string> input = JsonConvert.DeserializeObject<Dictionary<string, string>>(MDVUtility.ToStr(AllData["data"]));
                    return Request.CreateResponse(HttpStatusCode.OK, bllBatchFax.Delete_Fax(input));
                }
                else
                {
                    var responseObj = new
                    {
                        Status = false,
                        Result = privilegesMessage
                    };
                    strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                    return Request.CreateResponse(HttpStatusCode.OK, strJSONData);
                }
                
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetFaxStatus(JObject AllData)
        {
            try
            {
                Dictionary<string, string> input = JsonConvert.DeserializeObject<Dictionary<string, string>>(MDVUtility.ToStr(AllData["data"]));
                return Request.CreateResponse(HttpStatusCode.OK, bllBatchFax.Get_FaxStatus(input));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [HttpPost]
        public HttpResponseMessage ForwardFax(JObject AllData)
        {
            try
            {
                Dictionary<string, string> input = JsonConvert.DeserializeObject<Dictionary<string, string>>(MDVUtility.ToStr(AllData["data"]));
                return Request.CreateResponse(HttpStatusCode.OK, bllBatchFax.Forward_Fax(input));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}