using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Billing.PatientStatement;
using MDVision.IEHR.Model.Billing.PatientStatement;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.Billing.PatientStatement
{
    public class PatientStatementsController : ApiController
    {
        [HttpPost]
        public string PatientStatement(JObject objData)
        {

            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                PatientStatementModel model = ser.Deserialize<PatientStatementModel>(MDVUtility.ToStr(objData["data"]));


                if (model.CommandType.ToLower() == "search")
                {
                    string response = null;
                    response = Bill_PatientStatement.Instance().SearchPatientStatement(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "search_submitted_statements")
                {
                    string response = null;
                    response = Bill_PatientStatement.Instance().SearchSubmittedPatientStatement(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "print_patient_statements")
                {
                    string response = null;
                    response = Bill_PatientStatement.Instance().PrintPatientStatement(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                //if (model.CommandType.ToLower() == "print_patient_submitted_statements")
                //{
                //    string response = null;
                //    response = Bill_PatientStatement.Instance().PrintPatientSubmitStatement(model);
                //    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                //}

                if (model.CommandType.ToLower() == "get_submitted_statement_html")
                {
                    string response = null;
                    response = Bill_PatientStatement.Instance().GetSubmittedStatementHTML(MDVUtility.ToInt64(model.SubmittedStatementId));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                

                if (model.CommandType.ToLower() == "view_statement_xml")
                {
                    string response = null;
                    response = Bill_PatientStatement.Instance().PatientStatementXML(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "save_patient_statement")
                {
                    string response = null;
                    response = Bill_PatientStatement.Instance().SavePatientStatement(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "delete_statement")
                {
                    string response = null;
                    response = Bill_PatientStatement.Instance().DeletePatientStatement(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "search_patient_statements_batch")
                {
                    string response = null;
                    response = Bill_PatientStatement.Instance().SearchPatientStatementsBatch(model);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }

                if (model.CommandType.ToLower() == "view_batch_xml")
                {

                    string response = null;
                    response = Bill_PatientStatement.Instance().SearchBatchXML(MDVUtility.ToInt64(model.BatchId));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (model.CommandType.ToLower() == "search_batch_detail")
                {
                    string response = null;
                    response = Bill_PatientStatement.Instance().SearchBatchDetail(MDVUtility.ToInt64(model.BatchId));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }


            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        public string SubmittedPatientStatement(JObject objData)
        {
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                List<PatientStatementModel> model = ser.Deserialize<List<PatientStatementModel>>(MDVUtility.ToStr(objData["data"]));
                //System.Collections.Generic.PatientStatementModel lstModel = ser.Deserialize < System.Collections.Generic.PatientStatementModel(Utility.ToStr(objData["data"]));
                //if (lstModel[0].CommandType.ToLower() == "save_patient_statement")
                //{
                //    string response = null;
                //    response = Bill_PatientStatement.Instance().SavePatientStatement(lstModel);
                //    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                //}
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }
    }
}
