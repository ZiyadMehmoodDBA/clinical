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
using MDVision.DataAccess.DAL.Admin;
using System.Threading.Tasks;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Patient.Insurance
{
    public class Patient_Eligibility
    {
        private BLLPatient BLLPatientObj = null;
        private BLLBilling BLLBillingObj = null;
        public Patient_Eligibility()
        {
            BLLPatientObj = new BLLPatient();
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Patient_Eligibility _obj = null;
        public static Patient_Eligibility Instance()
        {
            if (_obj == null)
                _obj = new Patient_Eligibility();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string CheckPatientEligibility(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                long PatientInsurancePlanId = MDVUtility.ToLong(SearchedfieldsJSON["ddlPatientInsurancePlan"]);
                long PatientProviderId = MDVUtility.ToLong(SearchedfieldsJSON["hfProvider"]);
                DateTime DOS = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSFrom"]);
                long PatientId = MDVUtility.ToLong(SearchedfieldsJSON["hfPatientId"]);
                //Set current time
                DateTime now = DateTime.Now;
                DOS = DOS.Date.AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second).AddMilliseconds(now.Millisecond);

                string ServiceType = MDVUtility.ToStr(SearchedfieldsJSON["ddlServiceTypeCode"]);

                BLObject<DSPatientEligibility> obj = BLLBillingObj.PatientEligibility(PatientId, PatientInsurancePlanId, PatientProviderId, DOS, ServiceType);
                if (obj.Data != null)
                {
                    DSPatientEligibility dsEligibility = obj.Data;
                    DS271 ds271 = new DS271(); ;

                    DataTable dt = dsEligibility.Tables[ds271.EDI271Benefits.TableName];


                    if (dt.Rows.Count > 0)
                        dt = dt.AsEnumerable()
                           .GroupBy(r => new { Col1 = r[ds271.EDI271Benefits.EB01Column.ColumnName] })
                           .Select(g => g.First())
                           .CopyToDataTable();

                    var response = new
                    {
                        status = true,
                        ServiceTypeCode = ServiceType,
                        EligibilityBatch_JSON = MDVUtility.JSON_DataTable(dsEligibility.Tables[dsEligibility.EDIEligibility.TableName]),
                        EligibilityHeader_JSON = MDVUtility.JSON_DataTable(dsEligibility.Tables[ds271.EDI271Header.TableName]),
                        EligibilityNames_JSON = MDVUtility.JSON_DataTable(dsEligibility.Tables[ds271.EDI271Names.TableName]),
                        EligibilityBenifits_JSON = MDVUtility.JSON_DataTable(dsEligibility.Tables[ds271.EDI271Benefits.TableName]),
                        EligibilityBenifitsArray_JSON = MDVUtility.JSON_DataTable(dt),
                        EligibilityBenifitsDetail_JSON = MDVUtility.JSON_DataTable(dsEligibility.Tables[ds271.EDI271BenefitsDetail.TableName]),
                        EDIEligibilityId = dsEligibility.EDIEligibility.Rows[0][dsEligibility.EDIEligibility.EDIEligibilityIdColumn.ColumnName],
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

        #region Shared Commmented for Service Testing Locally

        private static SharedVariable SetSharedObject(DataRow drSoftwareCustomerInfo)
        {
            SharedVariable sharedObj = new SharedVariable
            {
                ClientId = drSoftwareCustomerInfo[DSSoftwareCustomersInfo.FIELD_UNIQUE_CLIENT_ID].ToString(),
                EntityId = drSoftwareCustomerInfo[DSSoftwareCustomersInfo.FIELD_ENTITY_ID].ToString(),
                WebEntityURL = drSoftwareCustomerInfo[DSSoftwareCustomersInfo.FIELD_WEB_SERVICE_URL].ToString(),
                UserName = MDVUtility.EncryptTo64("mdvision"),
                AppPassWord = MDVUtility.EncryptTo64("Password1!"),
                AppUserId = Convert.ToInt64(drSoftwareCustomerInfo[DSSoftwareCustomersInfo.FIELD_USER_ID].ToString()),
            };
            return sharedObj;
        }
        //void TestPatientEligibilityService()
        //{
        //    BLObject<DSSoftwareCustomersInfo> obj11 = new BLLCommon().GetCustomerSettings(MDVUtility.EncryptTo64("mdvision"), MDVUtility.EncryptToSHA256("Password1!", "mdvision"));
        //    DSSoftwareCustomersInfo dsSoftwareCustomerInfo = obj11.Data;
        //    foreach (DataRow drSoftwareCustomerInfo in dsSoftwareCustomerInfo.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows)
        //    {
        //        SharedVariable SharedVariable = SetSharedObject(drSoftwareCustomerInfo);
        //        var dsPatientEligibilityService = new DALPatientEligibilityService(SharedVariable).LoadPatientEligibilityService(SharedVariable, 0, SharedVariable.EntityId, null, null, null, null, null, 1, 1000);
        //        BLLBilling bllBilling = new BLLBilling();

        //        foreach (DSPatientEligibilityService.PatientEligibilityServiceRow drPatientEligibilityService in dsPatientEligibilityService.PatientEligibilityService.Rows)
        //        {
        //            var dateTime = DateTime.Now;

        //            var daysToAdd = long.Parse(drPatientEligibilityService[dsPatientEligibilityService.PatientEligibilityService.ScheduleDaysColumn.ColumnName].ToString());
        //            dateTime = dateTime.AddDays(daysToAdd);

        //            BLObject<DSPatientEligibility> patientsToCheck = bllBilling.LoadPatientEligibilityService(SharedVariable, dateTime);
        //            DSPatientEligibility dsPatientsToCheck = patientsToCheck.Data;

        //            var time = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":00";

        //            var hours = DateTime.Now.Hour.ToString();
        //            if (hours.Length == 1) hours = "0" + hours;
        //            hours = hours.Length == 1 ? "0" + hours : hours;

        //            var minutes = DateTime.Now.Minute;
        //            minutes = minutes <= 5 ? 6 : minutes;
        //            if (minutes <= 5) minutes = 6; else if (minutes >= 55) minutes = 54;


        //            var edisrvcClearingHouse = drPatientEligibilityService.ClearingHouseId.ToString();
        //            if ((time == drPatientEligibilityService.Time) ||
        //                ((hours + ":" + (minutes + 1) + ":00") == drPatientEligibilityService.Time) ||
        //                ((hours + ":" + (minutes + 2) + ":00") == drPatientEligibilityService.Time) ||
        //                ((hours + ":" + (minutes + 3) + ":00") == drPatientEligibilityService.Time) ||
        //                ((hours + ":" + (minutes + 4) + ":00") == drPatientEligibilityService.Time) ||
        //                ((hours + ":" + (minutes + 5) + ":00") == drPatientEligibilityService.Time) ||
        //                ((hours + ":" + (minutes - 1) + ":00") == drPatientEligibilityService.Time) ||
        //                ((hours + ":" + (minutes - 2) + ":00") == drPatientEligibilityService.Time) ||
        //                ((hours + ":" + (minutes - 3) + ":00") == drPatientEligibilityService.Time) ||
        //                ((hours + ":" + (minutes - 4) + ":00") == drPatientEligibilityService.Time) ||
        //                ((hours + ":" + (minutes - 5) + ":00") == drPatientEligibilityService.Time))
        //            {
        //                var aaa = dsPatientsToCheck.Tables[dsPatientsToCheck.PatientEligibilityService.TableName];
        //                Parallel.ForEach(dsPatientsToCheck.Tables[dsPatientsToCheck.PatientEligibilityService.TableName].AsEnumerable(), drow =>
        //                {

        //                    var PatientId_ = MDVUtility.ToLong(drow[dsPatientsToCheck.PatientEligibilityService.PatientIdColumn.ColumnName]);
        //                    var PatientInsurancePlanId_ = MDVUtility.ToLong(drow[dsPatientsToCheck.PatientEligibilityService.InsurancePlanIdColumn.ColumnName]);
        //                    var PatientProviderId_ = MDVUtility.ToLong(drow[dsPatientsToCheck.PatientEligibilityService.ProviderIdColumn.ColumnName]);
        //                    var DOS_ = MDVUtility.ToDateTime(drow[dsPatientsToCheck.PatientEligibilityService.AppointmentDateColumn.ColumnName]);
        //                    var ServiceType_ = MDVUtility.ToStr(drow[dsPatientsToCheck.PatientEligibilityService.ServiceTypeIdColumn.ColumnName]);

        //                    BLObject<DSPatientEligibility> objDSPatientEligibility = BLLBillingObj.PatientEligibility(PatientId_, PatientInsurancePlanId_, PatientProviderId_, DOS_, ServiceType_);

        //                    if (objDSPatientEligibility.Data != null)
        //                    {

        //                    }
        //                });

        //            }
        //        }
        //    }
        //}

        #endregion
        private string LoadPatientEligibility(string fieldsJSON, long PatientID, int PageNumber, int RowspPage )
        {
            try
            {
                //TestPatientEligibilityService();

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                long InsurancePlanId = MDVUtility.ToLong(SearchedfieldsJSON["hfInsurancePlanId"]);
                long ProviderId = MDVUtility.ToLong(SearchedfieldsJSON["hfProvider"]);

                DateTime? DOSFrom = null;
                DateTime? DOSTo = null;
                DateTime? EligibiltyFrom = null;
                DateTime? EligibiltyTo = null;
                DOSFrom = String.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSFrom"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dtpDOSFrom"]);
                DOSTo = String.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSTo"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dtpDOSTo"]);
                EligibiltyFrom = String.IsNullOrEmpty(SearchedfieldsJSON["dtpEligibiltyFrom"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dtpEligibiltyFrom"]);
                EligibiltyTo = String.IsNullOrEmpty(SearchedfieldsJSON["dtpEligibiltyTo"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dtpEligibiltyTo"]);

                long PatientId = MDVUtility.ToLong(SearchedfieldsJSON["hfPatientId"]);
                string ServiceType = MDVUtility.ToStr(SearchedfieldsJSON["ddlServiceTypeCode"]);
                string LastName = MDVUtility.ToStr(SearchedfieldsJSON["txtLastName"]);
                string FirstName = MDVUtility.ToStr(SearchedfieldsJSON["txtFirstName"]);
                string Status = MDVUtility.ToStr(SearchedfieldsJSON["ddlStatus"]);


                BLObject<DSPatientEligibility> obj = new BLObject<DSPatientEligibility>();

                if (PatientID > 0)
                    obj = BLLBillingObj.LoadEDIEligibility(0, PatientID, 0, 0, null, null, null, null, null, null, PageNumber, RowspPage);
                else
                    obj = BLLBillingObj.LoadEDIEligibility(0, PatientId, InsurancePlanId, ProviderId, DOSFrom, DOSTo, ServiceType, LastName, FirstName, Status, PageNumber, RowspPage,EligibiltyFrom,EligibiltyTo);

                if (obj.Data != null)
                {
                    DSPatientEligibility dsEligibility = obj.Data;
                    var response = new
                    {
                        status = true,
                        PatientEligibilityCount = dsEligibility.Tables[dsEligibility.EDIEligibility.TableName].Rows.Count > 0 ? dsEligibility.Tables[dsEligibility.EDIEligibility.TableName].Rows.Count : 0,
                        iTotalDisplayRecords = dsEligibility.Tables[dsEligibility.EDIEligibility.TableName].Rows.Count > 0 ? dsEligibility.EDIEligibility.Rows[0][dsEligibility.EDIEligibility.RecordCountColumn.ColumnName] : 0,
                        PatientEligibilityLoad_JSON = MDVUtility.JSON_DataTable(dsEligibility.Tables[dsEligibility.EDIEligibility.TableName]),
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
        private string LoadPatientEligibilityExport(string fieldsJSON, long PatientID)
        {
            try
            {
                //TestPatientEligibilityService();

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                long InsurancePlanId = MDVUtility.ToLong(SearchedfieldsJSON["hfInsurancePlanId"]);
                long ProviderId = MDVUtility.ToLong(SearchedfieldsJSON["hfProvider"]);

                DateTime? DOSFrom = null;
                DateTime? DOSTo = null;
                DOSFrom = String.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSFrom"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dtpDOSFrom"]);
                DOSTo = String.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSTo"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dtpDOSTo"]);

                long PatientId = MDVUtility.ToLong(SearchedfieldsJSON["hfPatientId"]);
                string ServiceType = MDVUtility.ToStr(SearchedfieldsJSON["ddlServiceTypeCode"]);
                string LastName = MDVUtility.ToStr(SearchedfieldsJSON["txtLastName"]);
                string FirstName = MDVUtility.ToStr(SearchedfieldsJSON["txtFirstName"]);
                string Status = MDVUtility.ToStr(SearchedfieldsJSON["ddlStatus"]);


                BLObject<DSPatientEligibility> obj = new BLObject<DSPatientEligibility>();

                if (PatientID > 0)
                    obj = BLLBillingObj.LoadPatientEligibilityExport(0, PatientID, 0, 0, null, null, null, null, null, null);
                else
                    obj = BLLBillingObj.LoadPatientEligibilityExport(0, PatientId, InsurancePlanId, ProviderId, DOSFrom, DOSTo, ServiceType, LastName, FirstName, Status);

                if (obj.Data != null)
                {
                    DSPatientEligibility dsEligibility = obj.Data;
                    var response = new
                    {
                        status = true,
                        PatientEligibilityCount = dsEligibility.Tables[dsEligibility.EDIEligibility.TableName].Rows.Count > 0 ? dsEligibility.Tables[dsEligibility.EDIEligibility.TableName].Rows.Count : 0,
                        iTotalDisplayRecords = dsEligibility.Tables[dsEligibility.EDIEligibility.TableName].Rows.Count > 0 ? dsEligibility.EDIEligibility.Rows[0][dsEligibility.EDIEligibility.RecordCountColumn.ColumnName] : 0,
                        PatientEligibilityLoad_JSON = MDVUtility.JSON_DataTable(dsEligibility.Tables[dsEligibility.EDIEligibility.TableName]),
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
        private string LoadInsuranceEDIEligibility(long InsurancePlanId, long PatientId)
        {
            try
            {
                BLObject<DSPatient> obj = BLLPatientObj.LoadPatientInsurance(InsurancePlanId, PatientId);
                if (obj.Data != null)
                {
                    DSPatient dsPatient = obj.Data;
                    if (dsPatient.PatientInsurance.Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            EDIEligibility = dsPatient.PatientInsurance.Rows[0][dsPatient.PatientInsurance.EDIEligibilityColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            EDIEligibility = "No Record Found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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

        private string LoadInsuranceEDIEligibilityReport(long EDIEligibilityId, string FileType)
        {
            try
            {
                BLObject<DSPatientEligibility> obj = BLLBillingObj.LoadEDIEligibility(EDIEligibilityId, 0, 0, 0, null, null, null, null, null, null);
                if (obj.Data != null)
                {
                    DSPatientEligibility dsEligibility = obj.Data;
                    if (dsEligibility.Tables[dsEligibility.EDIEligibility.TableName].Rows.Count > 0)
                    {
                        DSPatientEligibility.EDIEligibilityRow dr = (DSPatientEligibility.EDIEligibilityRow)dsEligibility.Tables[dsEligibility.EDIEligibility.TableName].Rows[0];
                        string TextView = string.Empty;

                        if (FileType == "271")
                            TextView = dr.Str271;
                        else
                            TextView = dr.Str270;

                        if (string.IsNullOrEmpty(TextView))
                            throw new Exception("No data found.");


                        var keyValues = new Dictionary<string, string>
                        {
                            { "txtEligibilityTextView", MDVUtility.ToStr(TextView)}
                        };

                        var response = new
                        {
                            status = true,
                            PatientEligibilityReport_JSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(keyValues),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "No data found."
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }

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

        private string DeleteEDIEligibilityReport(long EDIEligibilityId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(EDIEligibilityId)))
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
                    BLObject<string> obj = BLLBillingObj.DeleteEDIEligibility(MDVUtility.ToLong(EDIEligibilityId));
                    if (obj.Data == "")
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

        #region Service Command Handler
        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            string privilegesMessage = string.Empty;
            switch (cammandAction)
            {
                case "CHECK_PATIENT_ELIGIBILITY":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Insurance", "VIEW")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["Eligibilitydata"];
                            strJSONData = CheckPatientEligibility(fieldsJSON);
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
                case "LOAD_PATIENT_ELIGIBILITY":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Insurance", "VIEW")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["Eligibilitydata"];
                            long PatientId = MDVUtility.ToLong(context.Request["PatientId"]);
                            int PageNumber = MDVUtility.ToInt(context.Request["PageNumber"]);
                            int RowspPage = MDVUtility.ToInt(context.Request["RowsPerPage"]);
                            strJSONData = LoadPatientEligibility(fieldsJSON, PatientId, PageNumber, RowspPage);
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
                case "LOAD_PATIENT_ELIGIBILITY_EXPORT":
                    {
                        string fieldsJSON = context.Request["Eligibilitydata"];
                        long PatientId = MDVUtility.ToLong(context.Request["PatientId"]);
                        string strJSONData = LoadPatientEligibilityExport(fieldsJSON, PatientId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_INSURANCE_EDI_ELIGIBILITY":
                    {
                        long InsurancePlanId = MDVUtility.ToLong(context.Request["InsurancePlanId"]);
                        long PatientId = MDVUtility.ToLong(context.Request["PatientId"]);
                        string strJSONData = LoadInsuranceEDIEligibility(InsurancePlanId, PatientId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_INSURANCE_EDI_ELIGIBILITY_REPORT":
                    {
                        long EDIEligibilityId = MDVUtility.ToLong(context.Request["EDIEligibilityId"]);
                        string FileType = MDVUtility.ToStr(context.Request["FileType"]);
                        string strJSONData = LoadInsuranceEDIEligibilityReport(EDIEligibilityId, FileType);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_EDI_ELIGIBILITY":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Insurance", "DELETE")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            long EDIEligibilityId = MDVUtility.ToLong(context.Request["EDIEligibilityId"]);
                            strJSONData = DeleteEDIEligibilityReport(EDIEligibilityId);
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