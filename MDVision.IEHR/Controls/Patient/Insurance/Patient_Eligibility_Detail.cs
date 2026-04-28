using MDVision.Business.BCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using EDIParser;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Patient.Insurance
{
    public class Patient_Eligibility_Detail
    {
        private BLLBilling BLLBillingObj = null;
        public Patient_Eligibility_Detail()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Patient_Eligibility_Detail _obj = null;
        public static Patient_Eligibility_Detail Instance()
        {
            if (_obj == null)
                _obj = new Patient_Eligibility_Detail();
            return _obj;
        }
        #endregion


        #region Private Functions

        private string LoadEligibilityDetail(long EDIEligibilityId)
        {
            try
            {
                BLObject<DS271> obj = BLLBillingObj.PatientEligibilityReport(EDIEligibilityId);
                if (obj.Data != null)
                {
                    DS271 dsEligibility = obj.Data;
                    DSPatientEligibility dsEligibilityBatch = new DSPatientEligibility();
                    string ServiceTypeCode = string.Empty;

                    DataTable dt = dsEligibility.Tables[dsEligibility.EDI271Benefits.TableName];

                    if (dsEligibility.Tables[dsEligibilityBatch.EDIEligibility.TableName].Rows.Count <= 0)
                    {
                        BLObject<DSPatientEligibility> objBatch = BLLBillingObj.LoadEDIEligibility(EDIEligibilityId, 0, 0, 0, null, null, null, null, null, null);
                        if (objBatch.Data != null)
                        {
                            dsEligibility.Merge(objBatch.Data);
                            ServiceTypeCode = MDVUtility.ToStr(objBatch.Data.Tables[dsEligibilityBatch.EDIEligibility.TableName].Rows[0][dsEligibilityBatch.EDIEligibility.EQSeviceColumn.ColumnName]);
                        }
                    }
                    else
                    {
                        ServiceTypeCode = MDVUtility.ToStr(dsEligibility.Tables[dsEligibilityBatch.EDIEligibility.TableName].Rows[0][dsEligibilityBatch.EDIEligibility.EQSeviceColumn.ColumnName]);
                    }

                    if (dt.Rows.Count > 0)
                        dt = dt.AsEnumerable()
                           .GroupBy(r => new { Col1 = r[dsEligibility.EDI271Benefits.EB01Column.ColumnName] })
                           .Select(g => g.First())
                           .CopyToDataTable();

                    var response = new
                    {
                        status = true,
                        ServiceTypeCode = ServiceTypeCode,
                        EligibilityBatch_JSON = MDVUtility.JSON_DataTable(dsEligibility.Tables[dsEligibilityBatch.EDIEligibility.TableName]),
                        EligibilityHeader_JSON = MDVUtility.JSON_DataTable(dsEligibility.Tables[dsEligibility.EDI271Header.TableName]),
                        EligibilityNames_JSON = MDVUtility.JSON_DataTable(dsEligibility.Tables[dsEligibility.EDI271Names.TableName]),
                        EligibilityBenifits_JSON = MDVUtility.JSON_DataTable(dsEligibility.Tables[dsEligibility.EDI271Benefits.TableName]),
                        EligibilityBenifitsArray_JSON = MDVUtility.JSON_DataTable(dt),
                        EligibilityBenifitsDetail_JSON = MDVUtility.JSON_DataTable(dsEligibility.Tables[dsEligibility.EDI271BenefitsDetail.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient Eligibility request has no response."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            string privilegesMessage = string.Empty;
            switch (cammandAction)
            {
                case "LOAD_ELIGIBILITY_DETAIL":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Insurance", "VIEW")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            long EDIEligibilityId = MDVUtility.ToLong(context.Request["EDIEligibilityId"]);
                            strJSONData = LoadEligibilityDetail(EDIEligibilityId);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }



        #endregion

    }
}