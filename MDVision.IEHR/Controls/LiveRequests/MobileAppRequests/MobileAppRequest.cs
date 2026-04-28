using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.History;
using MDVision.Model.Native;
using MDVision.Model.Native.Clinical;
using MDVision.Model.Native.Patient;
using MDVision.Model.Patient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MDVision.IEHR.Controls.Clinical;
using MDVision.Common.Logging;
using Newtonsoft.Json.Linq;
using MDVision.Model.Native.Scheduler;

namespace MDVision.IEHR.Controls.Live_Requests.MobileAppRequest
{
    public class MobileAppRequest
    {
        private BLLMobileApp BLLMobileAppObj = null;
        public MobileAppRequest()
        {
            BLLMobileAppObj = new BLLMobileApp();
        }
        #region Singleton
        private static MobileAppRequest _obj = null;
        public static MobileAppRequest Instance()
        {
            if (_obj == null)
                _obj = new MobileAppRequest();
            return _obj;
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Saves the emergency contact.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string SaveEmergencyContact(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                PatientEmergencyContactModel SearchedfieldsJSON = ser.Deserialize<PatientEmergencyContactModel>(fieldsJSON);
                Int64 PatientId = Convert.ToInt64(SearchedfieldsJSON.PatientId);
                if (PatientId > 0)
                {

                    if (SearchedfieldsJSON.DOB == ""){
                        SearchedfieldsJSON.DOB = null;
                    }
                    string Result =BLLMobileAppObj.InsertPatientEmergencyContact(SearchedfieldsJSON);
                    if (Result=="")
                    {
                        var response = new
                        {
                            status = true,
                            message = "Approved Successfully!..."

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Result
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                   
                }
                else
                {
                    throw new Exception("Patient not found.");
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
       

        private string FillPatientPreferences(long PatientID,string RequestStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    MDVision.Model.Patient.PatientPreferenceModel Model = null;
                    Model = BLLMobileAppObj.FillPatientPreferences(PatientID, RequestStatus);

                    //   var dictionary = lstModel.Select(s => s.ContactId);

                    if (Model.PatientId != null)
                    {
                        //List<string> ContactIds = new List<string>();

                        //ContactIds = lstModel.Select(i=>i.ContactId).ToList();
                        //  var list=    lstModel.Select new PatientEmergencyContactModel(i => new PatientEmergencyContactModel() { ContactId = i.ContactId }).ToList();

                        // var list = lstModel.AsEnumerable().Select(i => new { ContactId = i.ContactId }).ToList();
                        var response = new
                        {
                            status = true,

                            PatientPreferences_JSON = Model,

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            EmergencyContactsCount = 0,
                            Message = "No Patient Found"
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
        private string SavePatientInsurance(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                PatientInsuranceModel SearchedfieldsJSON = ser.Deserialize<PatientInsuranceModel>(fieldsJSON);
                Int64 PatientId = Convert.ToInt64(SearchedfieldsJSON.PatientId);
                if (PatientId > 0)
                {
                    
                    string Result = BLLMobileAppObj.InsertPatientInsurance(SearchedfieldsJSON);
                    if (Result == "")
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Result
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }

                }
                else
                {
                    throw new Exception("Patient not found.");
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
        private string UpdatePatientInsurance(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                PatientInsuranceModel SearchedfieldsJSON = ser.Deserialize<PatientInsuranceModel>(fieldsJSON);
                Int64 PatientId = Convert.ToInt64(SearchedfieldsJSON.PatientId);
                if (PatientId > 0)
                {
                    if (SearchedfieldsJSON.lstChangedColumns.Count > 0)
                    {
                        //  IList<string> strings = SearchedfieldsJSON.lstChangedColumns;
                        SearchedfieldsJSON.listChangedColumns = string.Join(",", SearchedfieldsJSON.lstChangedColumns.Select(i => i.columnName));
                    }
                    string Result = BLLMobileAppObj.UpdatePatientInsurance(SearchedfieldsJSON);
                    if (Result == "")
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Result
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }

                }
                else
                {
                    throw new Exception("Patient not found.");
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
        private string LoadPatientEmergencyContacts(long PatientID,string RequestStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    List<PatientEmergencyContactModel> emcModel = new List<PatientEmergencyContactModel>();
                    emcModel = BLLMobileAppObj.loadPatientEmergencyContacts(PatientID,RequestStatus);

                    //   var dictionary = lstModel.Select(s => s.ContactId);

                    if (emcModel.Count >0)
                    {
                        //List<string> ContactIds = new List<string>();

                        //ContactIds = lstModel.Select(i=>i.ContactId).ToList();
                          var list= emcModel.Select(i => new  { ContactId = i.ContactId }).ToList();

                        // var list = lstModel.AsEnumerable().Select(i => new { ContactId = i.ContactId }).ToList();
                        var response = new
                        {
                            status = true,

                            EmergencyContactsCount = emcModel.Count,
                            EmergencyContactsLoad_JSON= list,

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            EmergencyContactsCount = 0,
                            Message = "No Contact Found"
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
        private string LoadPatientInsurances(long PatientID,string RequestStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    List<PatientInsuranceModel> Model = new List<PatientInsuranceModel>();
                    Model = BLLMobileAppObj.loadPatientInsurances(PatientID, RequestStatus);

                    //   var dictionary = lstModel.Select(s => s.ContactId);

                    if (Model.Count > 0)
                    {
                        //List<string> ContactIds = new List<string>();

                        //ContactIds = lstModel.Select(i=>i.ContactId).ToList();
                        var list = Model.Select(i => new { insurancePlanId = i.InsurancePlanId, ColumnKeyId=i.ColumnKeyId, PatientId = i.PatientId, PlanPriority = i.PlanPriority, SubscriberId = i.SubscriberId, GroupId = i.GroupId, SubscriberDOB = i.SubscriberDOB, SubscriberFirstName = i.SubscriberFirstName, SubscriberMI = i.SubscriberMI, SubscriberLastName = i.SubscriberLastName, RelationShipId = i.RelationShipId, Gender = i.Gender, Comments = i.Comments }).ToList();

                        // var list = lstModel.AsEnumerable().Select(i => new { ContactId = i.ContactId }).ToList();
                        var response = new
                        {
                            status = true,

                            PatientInsurancesCount = Model.Count,
                            PatientInsuranceList_JSON = list,

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientInsurancesCount = 0,
                            Message = "No InsuranceFound Found"
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
        private string LoadMaxInsurancesPriority(long PatientID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    int returnVal = 0;
                   returnVal = BLLMobileAppObj.loadMaxInsurancesPriority(PatientID);
                 var response = new
                        {
                            status = true,

                            PlanPriority = returnVal,
                            

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
        /// <summary>
        /// Updates the emergency contact.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="EmergencyContactID">The emergency contact identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string UpdateEmergencyContact(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                PatientEmergencyContactModel SearchedfieldsJSON = ser.Deserialize<PatientEmergencyContactModel>(fieldsJSON);

                Int64 EmergencyContactID = Convert.ToInt64( SearchedfieldsJSON.ContactId);
                if (EmergencyContactID > 0)
                {


                    if (SearchedfieldsJSON.DOB == "")
                    {
                        SearchedfieldsJSON.DOB = null;
                    }

                    if (string.IsNullOrEmpty( SearchedfieldsJSON.HomePhone)  && string.IsNullOrEmpty( SearchedfieldsJSON.CellNo))
                        {
                            if (SearchedfieldsJSON.IsPrimary == "1")
                            {
                            SearchedfieldsJSON.IsPrimary ="0";
                            }
                        }
                        //else
                        //{
                        //SearchedfieldsJSON.IsPrimary ="1";
                        //}


                    if (SearchedfieldsJSON.lstChangedColumns.Count > 0)
                    {
                      //  IList<string> strings = SearchedfieldsJSON.lstChangedColumns;
                        SearchedfieldsJSON.listChangedColumns= string.Join(",", SearchedfieldsJSON.lstChangedColumns.Select(i=>i.columnName)); 
                    }
                    


                    #region Database Updation
                   
                        string Result = BLLMobileAppObj.UpdatePatientEmergencyContact(SearchedfieldsJSON);
                        if (Result=="")
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Result
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    
                   
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Emergency Contact not found."
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
            //return "";
        }

        private string UpdatePatientPreferences(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                PatientPreferenceModel SearchedfieldsJSON = ser.Deserialize<PatientPreferenceModel>(fieldsJSON);

                Int64 PatientId = Convert.ToInt64(SearchedfieldsJSON.PatientId);
                if (PatientId > 0)
                {
                    if (SearchedfieldsJSON.lstChangedColumns.Count > 0)
                    {
                        //  IList<string> strings = SearchedfieldsJSON.lstChangedColumns;
                        SearchedfieldsJSON.listChangedColumns = string.Join(",", SearchedfieldsJSON.lstChangedColumns.Select(i => i.columnName));
                    }

                    string Result = BLLMobileAppObj.UpdatePatientPreferences(SearchedfieldsJSON);
                    if (Result == "")
                    {
                        var response = new
                        {
                            status = true,
                            message = "Approved Successfully!..."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Result
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Emergency Contact not found."
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
            //return "";
        }
        private string FillEmergencyContact(long PatientID, long EmergencyContactID,string RequestStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    PatientEmergencyContactModel PEC = null;
                    PEC = BLLMobileAppObj.FillPatientEmergencyContact(PatientID, EmergencyContactID,RequestStatus);
                    if (PEC.ContactId!=null)
                   {
                       
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                EmergencyContactFill_JSON = js.Serialize(PEC)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        
                       
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Record Not Found",
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
        private string FillPatientInsurance(long PatientID, long EmergencyContactID,string RequestStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    PatientInsuranceModel PEM = null;
                    PEM = BLLMobileAppObj.FillPatientInsurance(PatientID, EmergencyContactID,  RequestStatus);
                    if (PEM.InsuranceId != null)
                    {

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            PatientInsuranceDetail_JSON = js.Serialize(PEM)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Record Not Found",
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

        
              private string FillPatientAppointment(long PatientID, string RequestStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    EmptySlotModel ESM = null;
                    ESM = BLLMobileAppObj.FillPatientAppointment(PatientID, RequestStatus);
                    if (ESM.PatientId != null)
                    {

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            PatientAppointmentDetail_JSON = js.Serialize(ESM)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Record Not Found",
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
        private string DiscardRecord(long PatientID, long ColumnkeyId,string DBTableName,string changedColumnsArray)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    string ReturnVal = "";
                    string ChangedColumnsString = "";
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                    if (changedColumnsArray != "")
                    {

                        List<ChangedColumnsNative> lstchangedColumns = new List<ChangedColumnsNative>();

                        lstchangedColumns = ser.Deserialize<List<ChangedColumnsNative>>(changedColumnsArray);

                         ChangedColumnsString = String.Join(",", lstchangedColumns.Select(i => i.columnName));
                    }

                    

                    ReturnVal = BLLMobileAppObj.DiscardRecord(PatientID, ColumnkeyId,DBTableName, ChangedColumnsString);
                    if (ReturnVal=="")
                    {

                       
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ReturnVal,
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

        private string DiscardAllRecord(long PatientID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    string ReturnVal = "";
                  
                 



                    ReturnVal = BLLMobileAppObj.DiscardAllRecord(PatientID);
                    if (ReturnVal == "")
                    {


                        var response = new
                        {
                            status = true,
                            message = ReturnVal,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ReturnVal,
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
        private string ApproveAllRecord(long PatientID, long isDemogrphicExist,PatientDemographicModelNative request_model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    string ReturnVal = "";





                    ReturnVal = BLLMobileAppObj.ApproveAllRecord(PatientID);
                    if (ReturnVal == "")
                    {
                        if (isDemogrphicExist==1) {

                            DSPatient dsPatient = new DSPatient();
                            DataRow row = dsPatient.Tables["Patients"].NewRow();
                            row["PatientId"] = request_model.PatientID;
                            row["AccountNumber"] = request_model.AccountNo;
                            row["FirstName"] = request_model.FirstName;
                            row["MI"] = request_model.MI;
                            row["LastName"] = request_model.LastName;

                            row["DOB"] = request_model.DOB;
                            row["Gender"] = request_model.Gender;
                            row["Address1"] = request_model.Address1;
                            row["City"] = request_model.City;
                            row["State"] = request_model.State;
                            row["ZIPCode"] = request_model.ZipCode;

                            dynamic ResponseOfDrFirst = null;
                            try
                                {
                                BLLRcopia BLLRcopiaObj  = new BLLRcopia();
                                ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("UpdatePatient", row, request_model.PatientID));
                                }
                                catch (Exception ex)
                                {
                                    MDVLogger.SendExcepToDB(ex, "UpdatePatientNative", null);
                                }

                              }

                        var response = new
                        {
                            status = true,
                            message = ReturnVal,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ReturnVal,
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
        public string fillHospitalizationHx( Int64 PatientId, string RequestStatus,Int64 HospitalizationHxDiseaseId=0)
        {
            try
            {
                long hospitalizationHxId=0;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                   // HospitalizationHxModel model = null;
                    DSHospitalizationHx dsHospitalizationHx = null;
                    BLObject<DSHospitalizationHx> obj = BLLMobileAppObj.loadHospitalizationHxDisease(MDVUtility.ToInt64(PatientId),RequestStatus,HospitalizationHxDiseaseId);
                    dsHospitalizationHx = obj.Data;
                    if (dsHospitalizationHx != null)
                    {
                        //if (dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows.Count > 0)
                        //{
                        //    DataRow dr = dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows[0];
                        //    var HospitalizationHxkeyValues = new Dictionary<string, string>
                        //{
                        //    { "HospitalizationHxDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxDateColumn.ColumnName]).ToShortDateString()},
                        //    { "HospitalizationHxId",  MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxIdColumn.ColumnName])},
                        //    { "HospitalizationHxUnremarkable", MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.bUnremarkableColumn.ColumnName])},
                        //    { "HospitalizationHxComments", MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.CommentsColumn.ColumnName])},
                        //    { "HospitalizationHxSoapText", MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.SoapTextColumn.ColumnName])}
                        //};

                      
                            List<Dictionary<string, string>> lstDisease = new List<Dictionary<string, string>>();
                            var DiseaseHxkeyValues = new Dictionary<string, string> { { "", "" } };

                            //start Farooq Ahmad 21/01/2015 if model has disease detail then make the dictionary diseasehxkeyvalues for json
                            if (MDVUtility.ToInt64(HospitalizationHxDiseaseId) != 0)
                            {

                                DSHospitalizationHx.HospitalizationHx_DiseaseRow[] arrToComponentRows = (DSHospitalizationHx.HospitalizationHx_DiseaseRow[])dsHospitalizationHx.HospitalizationHx_Disease.Select(MDVUtility.ToStr(dsHospitalizationHx.HospitalizationHx_Disease.DiseaseIdColumn.ColumnName) + "=" + MDVUtility.ToStr(HospitalizationHxDiseaseId) + "");
                                DataRow drDisease = (DataRow)arrToComponentRows[0];

                                DiseaseHxkeyValues = new Dictionary<string, string>
                             {  { "HospitalizationHxId",  MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.HospitalizationHxIdColumn.ColumnName])},
                              { "ICDID",  MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.ICDIDColumn.ColumnName])},
                               { "CPTID",  MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.CPTIDColumn.ColumnName])},
                                { "DiseaseId",  MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.DiseaseIdColumn.ColumnName])},
                                { "HospitalizationDiseaseStayDuration",  MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.StayDurationColumn.ColumnName])},
                                { "HospitalizationDiseaseStayId",  MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.StayIdColumn.ColumnName])},
                                { "HospitalizationDiseaseStatus",  MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.StatusIdColumn.ColumnName])},
                                { "HospitalizationDiseaseAdmissionDate",  String.IsNullOrEmpty(MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.AdmissionDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.AdmissionDateColumn.ColumnName]).ToShortDateString() },
                                { "HospitalizationDiseaseDischargeDate",  String.IsNullOrEmpty(MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.DischargeDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.DischargeDateColumn.ColumnName]).ToShortDateString() },
                                { "HospitalizationDiseaseHospital",String.IsNullOrEmpty(MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.HospitalColumn.ColumnName]))?"": MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.HospitalColumn.ColumnName]) },
                                { "HospitalizationDiseaseComments", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CommentsColumn.ColumnName])},
                                //{ "CPT",MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTCodeColumn.ColumnName]) !=""? (MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTCodeColumn.ColumnName])+" - "+MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTCodeDescriptionColumn.ColumnName])):""},
                                { "CPTCodeId", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTCodeColumn.ColumnName])},
                                //{ "CPTCode", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTCodeColumn.ColumnName])},
                                { "CPTDescription", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTCodeDescriptionColumn.ColumnName])},

                               { "CPTSNOMEDCodeId", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTSNOMEDIDColumn.ColumnName])},
                                { "CPTSNOMEDDescription", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTSNOMEDDescriptionColumn.ColumnName])},

                                 { "CreatedBy", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CreatedByColumn.ColumnName])},
                                  { "CreatedOn", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CreatedOnColumn.ColumnName])},
                                   { "ModifiedBy", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.ModifiedByColumn.ColumnName])},
                                    { "ModifiedOn", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.ModifiedOnColumn.ColumnName])},

                                     { "ICD10Code", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.ICD10CodeColumn.ColumnName])},
                                { "ICD10CodeDescription", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.ICD10CodeDescriptionColumn.ColumnName])},

                                 { "ICD9Code", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.ICD9CodeColumn.ColumnName])},
                                  { "ICD9CodeDescription", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.ICD9CodeDescriptionColumn.ColumnName])},
                                   { "SNOMEDID", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.SNOMEDIDColumn.ColumnName])},
                                      { "SNOMEDDescription", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.SNOMEDDescriptionColumn.ColumnName])},
                                      { "FreeTextICD", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.FreeTextICDColumn.ColumnName])},
                                      { "CPTCode", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.FreeTextCPTColumn.ColumnName])},

                            };

                            }
                            //End Farooq Ahmad 21/01/2015 if model has disease detail then make the dictionary diseasehxkeyvalues for json

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                            var SoapText = string.Empty;
                            var IsCreatedOrModified = string.Empty;
                            var LastUpdated = string.Empty;
                          //  hospitalizationHxId = MDVUtility.ToInt64(dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxIdColumn.ColumnName]);
                            //   var SoapInfo = getCurrentSoapText(hospitalizationHxId);

                            //  if (SoapInfo != null)
                            //   {
                            //     SoapText = SoapInfo["SoapText"];
                            //      IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                            //  LastUpdated = SoapInfo["LastUpdated"];
                            //  }

                            var response = new
                            {
                                status = true,
                              
                              //  Message = "Record Updated Successfully !",
                                DiseaseFill_JSON = js.Serialize(DiseaseHxkeyValues),
                                HospitalizationHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx_Disease.TableName]),
                               
                                IsCreatedOrModified = IsCreatedOrModified,
                                LastUpdated = LastUpdated,
                                HospitalizationHxId = hospitalizationHxId

                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                     //   }
                        //else
                        //{
                        //    //System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        //    var response1 = new
                        //    {
                        //        status = true,
                        //        HospitalizationHxFill_JSON = "[]",
                        //        // HospitalizationHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx_Disease.TableName]),
                        //        HospitalizationHxLoad_JSON = MDVUtility.JSON_DataTable(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName]),
                        //        //start Farooq Ahmad 21/01/2015  diseasehx key values and hospitalization hx disease load to json
                        //        DiseaseFill_JSON = "[]",
                        //        HospitalizationHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx_Disease.TableName]),
                        //        //end Farooq Ahmad 21/01/2015  diseasehx key values and hospitalization hx disease load to json
                        //    };
                        //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
                        //}
                    }
                    else
                    {
                        

                       

                        var response = new
                        {
                            status = false,
                            Message = "Record Not Found",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                }
                // return "";
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
        public string fillSurgicalHx(Int64 PatientId, string RequestStatus, Int64 SurgicalHxDiseaseId = 0)
        {
            try
            {
                long surgicalHxId = 0;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {

                    DSSurgicalHx dsSurgicalHx = null;
                    BLObject<DSSurgicalHx> obj = BLLMobileAppObj.loadSurgicalHxDisease(MDVUtility.ToInt64(PatientId), RequestStatus, SurgicalHxDiseaseId);
                    dsSurgicalHx = obj.Data;
                    if (dsSurgicalHx != null)
                    {
                     
                        List<Dictionary<string, string>> lstDisease = new List<Dictionary<string, string>>();
                        var DiseaseHxkeyValues = new Dictionary<string, string> { { "", "" } };

                        //start Farooq Ahmad 21/01/2015 if model has disease detail then make the dictionary diseasehxkeyvalues for json
                        if (MDVUtility.ToInt64(SurgicalHxDiseaseId) != 0)
                        {

                            DSSurgicalHx.SurgicalHx_DiseaseRow[] arrToComponentRows = (DSSurgicalHx.SurgicalHx_DiseaseRow[])dsSurgicalHx.SurgicalHx_Disease.Select(MDVUtility.ToStr(dsSurgicalHx.SurgicalHx_Disease.DiseaseIdColumn.ColumnName) + "=" + MDVUtility.ToStr(SurgicalHxDiseaseId) + "");
                            DataRow drDisease = (DataRow)arrToComponentRows[0];

                            DiseaseHxkeyValues = new Dictionary<string, string>
                             {  { "SurgicalHxId",  MDVUtility.ToStr(arrToComponentRows[0][dsSurgicalHx.SurgicalHx_Disease.SurgicalHxIdColumn.ColumnName])},
                                { "DiseaseId",  MDVUtility.ToStr(arrToComponentRows[0][dsSurgicalHx.SurgicalHx_Disease.DiseaseIdColumn.ColumnName])},
                                { "SurgicalSurgeryDate",  MDVUtility.ToStr(arrToComponentRows[0][dsSurgicalHx.SurgicalHx_Disease.SurgeryDateColumn.ColumnName])},
                                { "SurgicalStatus",  MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.StatusIdColumn.ColumnName])},
                                { "SurgicalLocation",  MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.LocationColumn.ColumnName])},
                                { "AgeAtSurgery",  MDVUtility.ToStr(arrToComponentRows[0][dsSurgicalHx.SurgicalHx_Disease.AgeAtSurgeryColumn.ColumnName])},
                                { "SurgicalReason",  MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.SurgeryReasonColumn.ColumnName])},
                                { "OrderingProviderId",  MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.OrderingProviderIdColumn.ColumnName])},
                                { "SurgicalOrderingProvider",  MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.OrderingProviderNameColumn.ColumnName])},
                                 { "SurgicalPerformingProvider",  MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.PerformingProviderNameColumn.ColumnName])},
                                 { "PerformingProviderId",  MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.PerformingProviderIdColumn.ColumnName])},

                                 { "SurgicalDiseaseComments",  MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CommentsColumn.ColumnName])},
                                 { "CPTID",  MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTIDColumn.ColumnName])},
                               { "CPT",MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTCodeColumn.ColumnName]) !=""? (MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTCodeColumn.ColumnName])+" - "+MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTCodeDescriptionColumn.ColumnName])):""},
                                { "CPTCodeId", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTCodeColumn.ColumnName])},
                                { "CPTCode", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTCodeColumn.ColumnName])},
                                { "CPTDescription", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTCodeDescriptionColumn.ColumnName])},

                               { "CPTSNOMEDCodeId", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTSNOMEDIDColumn.ColumnName])},
                                { "CPTSNOMEDDescription", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTSNOMEDDescriptionColumn.ColumnName])},

                                 { "CreatedBy", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CreatedByColumn.ColumnName])},
                                  { "CreatedOn", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CreatedOnColumn.ColumnName])},
                                   { "ModifiedBy", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.ModifiedByColumn.ColumnName])},
                                   { "ModifiedOn", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.ModifiedOnColumn.ColumnName])},
                                   { "FreeTextProcedure", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.FreeTextCPTColumn.ColumnName])},
                            };

                        }
                       
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                        var SoapText = string.Empty;
                        var IsCreatedOrModified = string.Empty;
                        var LastUpdated = string.Empty;
                       

                        var response = new
                        {
                            status = true,
                           DiseaseFill_JSON = js.Serialize(DiseaseHxkeyValues),
                            SurgicalHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx_Disease.TableName]),
                            //end Farooq Ahmad 21/01/2015  diseasehx key values and hospitalization hx disease load to json
                            //   SoapText = SoapText,
                            IsCreatedOrModified = IsCreatedOrModified,
                            LastUpdated = LastUpdated,
                            SurgicalHxId = surgicalHxId

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                        var DiseaseHxkeyValues = new Dictionary<string, string> { { "", "" } };



                        var response = new
                        {
                            status = true,
                            SurgicalHxFill_JSON = "[]",
                            // HospitalizationHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx_Disease.TableName]),
                            SurgicalHxLoad_JSON = MDVUtility.JSON_DataTable(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName]),
                            //start Farooq Ahmad 21/01/2015  diseasehx key values and hospitalization hx disease load to json
                            DiseaseFill_JSON = "[]",
                            SurgicalHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx_Disease.TableName]),
                            //end Farooq Ahmad 21/01/2015  diseasehx key values and hospitalization hx disease load to json
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

        private string FillBirthHx_NewBorn(long PatientId, string RequestStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BirthHxNewbornModelNative BHM = null;
                    BHM = BLLMobileAppObj.FillBirthHx_NewBorn(PatientId, RequestStatus);
                    if (BHM.NewbornId != "")
                    {

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            NewBornFill_JSON = js.Serialize(BHM)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Record Not Found",
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

        private string FillBirthHx_General(long PatientId, string RequestStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BirthHxGeneralModelNative BHM = null;
                    BHM = BLLMobileAppObj.FillBirthHx_General(PatientId, RequestStatus);
                    if (BHM.GeneralId != "")
                    {

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            GeneralFill_JSON = js.Serialize(BHM)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Record Not Found",
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
        //BirthHxMaternalDeliveryModelNative
         private string FillBirthHx_Maternal(long PatientId, string RequestStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BirthHxMaternalDeliveryModelNative BHM = null;
                    BHM = BLLMobileAppObj.FillBirthHx_Maternal(PatientId, RequestStatus);
                    if (BHM.MaternalDeliveryId != "")
                    {

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            MaternalFill_JSON = js.Serialize(BHM)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Record Not Found",
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

        public string fillFamilyHx(Int64 PatientId, string RequestStatus,Int64 FamilyMemberId, Int64 FamilyHxDiseaseId = 0)
        {
            try
            {
                DataTable obj;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    obj = BLLMobileAppObj.loadFamilyHxDisease(MDVUtility.ToInt64(PatientId), RequestStatus, FamilyMemberId,FamilyHxDiseaseId);
                    if (obj != null)
                    {
                        if (obj.Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                FamilyMemberDiseasesCount = obj.Rows.Count,
                                FamilyMemberDiseasesHxLoad_JSON = MDVUtility.JSON_DataTable(obj),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                FamilyMemberDiseasesCount = 0,
                                FamilyMemberDiseasesHxLoad_JSON = MDVUtility.JSON_DataTable(obj),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            SystemsCount = 0,
                            // Message = "Could not load data"
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

        public string fillFamilyMembers(Int64 PatientId, string RequestStatus)
        {
            try
            {
               DataTable obj;
               if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {                   
                    obj = BLLMobileAppObj.loadFamilyHxMembers(MDVUtility.ToInt64(PatientId), RequestStatus);                  
                    if (obj != null)
                    {
                        if (obj.Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                FamilyMemberCount = obj.Rows.Count,                                
                                FamilyMemberHxLoad_JSON = MDVUtility.JSON_DataTable(obj),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                FamilyMemberCount = 0,
                                FamilyMemberHxLoad_JSON = MDVUtility.JSON_DataTable(obj),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                        }
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            SystemsCount = 0,
                           // Message = "Could not load data"
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

            switch (cammandAction)
            {
                case "SAVE_EMERGENCYCONTACT":
                    {
                        string fieldsJSON = context.Request["EmergencyContactData"];
                     //   Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = SaveEmergencyContact(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
              

                case "UPDATE_EMERGENCYCONTACT":
                    {
                        string fieldsJSON = context.Request["EmergencyContactData"];
                      //  Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                     //    Int64 EmergencyContactID = MDVUtility.ToInt64(context.Request["EmergencyContactID"]);
                        string strJSONData = UpdateEmergencyContact(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_PREFERENCES":
                    {
                          string RequestStatus = context.Request["RequestStatus"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        //    Int64 EmergencyContactID = MDVUtility.ToInt64(context.Request["EmergencyContactID"]);
                        string strJSONData = FillPatientPreferences(PatientID,RequestStatus);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_PREFERENCES":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        string fieldsJSON = context.Request["PatientPreferencesData"];
                        //    Int64 EmergencyContactID = MDVUtility.ToInt64(context.Request["EmergencyContactID"]);
                        string strJSONData = UpdatePatientPreferences(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_EMERGENCYCONTACTS_NATIVE":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                           string RequestStatus = MDVUtility.ToStr(context.Request["RequestStatus"]);
                        string strJSONData = LoadPatientEmergencyContacts(PatientID,RequestStatus);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_EMERGENCYCONTACT_NATIVE":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            Int64 EmergencyContactID = MDVUtility.ToInt64(context.Request["EmergencyContactID"]);
                        string  RequestStatus = MDVUtility.ToStr(context.Request["RequestStatus"]);
                        string strJSONData = FillEmergencyContact(PatientID,EmergencyContactID, RequestStatus);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_PATIENT_INSURANCE":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string RequestStatus = MDVUtility.ToStr(context.Request["RequestStatus"]);
                        string strJSONData = LoadPatientInsurances(PatientID, RequestStatus);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_PATIENT_INSURANCE_PRIORITY":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                   
                        string strJSONData = LoadMaxInsurancesPriority(PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_INSURANCE":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 InsuranceID = MDVUtility.ToInt64(context.Request["InsuranceId"]);
                        string RequestStatus = MDVUtility.ToStr(context.Request["RequestStatus"]);
                        string strJSONData = FillPatientInsurance(PatientID, InsuranceID, RequestStatus);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_APPOINTMENT":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string RequestStatus = MDVUtility.ToStr(context.Request["RequestStatus"]);
                        string strJSONData = FillPatientAppointment(PatientID, RequestStatus);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                    
                case "SAVE_PATIENT_INSURANCE":
                    {
                        string fieldsJSON = context.Request["PatientInsuranceData"];
                        string strJSONData = SavePatientInsurance(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_INSURANCE":
                    {
                        string fieldsJSON = context.Request["PatientInsuranceData"];
                        string strJSONData = UpdatePatientInsurance(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DISCARD_RECORD":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 ColumnKeyId = MDVUtility.ToInt64(context.Request["ColumnKeyId"]);
                        string DBTableName = MDVUtility.ToStr(context.Request["DBtableName"]);
                        string changedColumnsArray = MDVUtility.ToStr(context.Request["changedColumnsArray"]);
                        string strJSONData = DiscardRecord(PatientID, ColumnKeyId, DBTableName, changedColumnsArray);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DISCARD_ALL_RECORD":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                       
                        string strJSONData = DiscardAllRecord(PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "APPROVE_ALL_RECORD":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);                     
                        Int64 isDemogrphicExist= MDVUtility.ToInt64(context.Request["IsDemographicExist"]);
                        PatientDemographicModelNative request_model = new PatientDemographicModelNative();
                        if (isDemogrphicExist == 1){
                            request_model.PatientID = (context.Request["PatientID"]);
                            request_model.AccountNo = (context.Request["AccountNo"]);
                            request_model.FirstName = (context.Request["FirstName"]);
                            request_model.MI=(context.Request["MI"]);
                            request_model.LastName =(context.Request["LastName"]);
                            request_model.DOB = (context.Request["DOB"]);
                            request_model.Gender = (context.Request["Gender"]);
                            request_model.Address1 = (context.Request["Address1"]);
                            request_model.City = (context.Request["City"]);
                            request_model.State = (context.Request["State"]);
                            request_model.ZipCode = (context.Request["ZipCode"]);
                        }

                        string strJSONData = ApproveAllRecord(PatientID,isDemogrphicExist, request_model);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_HOSPITALIZATIONHX_DISEASES":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string RequestStatus = MDVUtility.ToStr(context.Request["RequestStatus"]);
                        Int64 DiseaseID = MDVUtility.ToInt64(context.Request["DiseaseId"]);
                        string strJSONData = fillHospitalizationHx(PatientID, RequestStatus,DiseaseID);                      
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "LOAD_FAMILYHX_DISEASES":
                    {
                        
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string RequestStatus = MDVUtility.ToStr(context.Request["RequestStatus"]);
                        Int64 DiseaseID = MDVUtility.ToInt64(context.Request["DiseaseID"]);
                        Int64 FamilyMemberId = MDVUtility.ToInt64(context.Request["FamilyMemberId"]);
                        string strJSONData = fillFamilyHx(PatientID, RequestStatus, FamilyMemberId, DiseaseID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_SURGICALHX_DISEASES":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string RequestStatus = MDVUtility.ToStr(context.Request["RequestStatus"]);
                        Int64 DiseaseID = MDVUtility.ToInt64(context.Request["DiseaseId"]);
                        string strJSONData = fillSurgicalHx(PatientID, RequestStatus,DiseaseID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "LOAD_NEWBORN_HISTORY_NATIVE":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string RequestStatus = MDVUtility.ToStr(context.Request["RequestStatus"]);
                        string strJSONData = FillBirthHx_NewBorn(PatientID, RequestStatus);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "LOAD_GENERAL_HISTORY_NATIVE":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string RequestStatus = MDVUtility.ToStr(context.Request["RequestStatus"]);
                        string strJSONData = FillBirthHx_General(PatientID, RequestStatus);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        
                    }
                    break;
                case "LOAD_FAMILYHX_MEMBERS":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string RequestStatus = MDVUtility.ToStr(context.Request["RequestStatus"]);
                        Int64 DiseaseID = MDVUtility.ToInt64(context.Request["DiseaseId"]);
                        string strJSONData = fillFamilyMembers(PatientID, RequestStatus);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "LOAD_MATERNAL_HISTORY_NATIVE":
                    {
                        //  string fieldsJSON = context.Request["EmergencyContactData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string RequestStatus = MDVUtility.ToStr(context.Request["RequestStatus"]);
                        string strJSONData = FillBirthHx_Maternal(PatientID, RequestStatus);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion
    }
}
