using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.Model.Clinical.Medical.Implantable;
using Newtonsoft.Json;
using System.Data;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Medical
{
    public class ImplantableHelper
    {
        private BLLClinical BLLClinicalObj = null;

        #region Constructors
        public ImplantableHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }

        private static ImplantableHelper _instance;

        public static ImplantableHelper Instance()
        {
            return _instance ?? (_instance = new ImplantableHelper());
        }

        #endregion

        #region Insert, Update, Delete, Select
        public string LoadImplantableDevice(ImplantableDevices model)
        {
            try
            {
                var devicesList = new List<ImplantableDevicesWithNotes>();
                var obj = BLLClinicalObj.loadImplantableDevice(MDVUtility.ToInt64(model.ImplantableDevicesPKId),
                        MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NotesId),
                        MDVUtility.ToInt16(model.IsActive), MDVUtility.ToInt64(model.PageNumber),
                        MDVUtility.ToInt64(model.RowsPerPage));
                if (obj.Data != null)
                    devicesList = obj.Data;
                if (devicesList.Count > 0)
                {
                    var listDevices = new List<ImplantableDevices>(devicesList[0]._implantableDevices);
                    var listNoteDevices = new List<DevicesAttachedWithNotes>(devicesList[0]._attachedNotes);
                    var result = listDevices.Where( p => listNoteDevices.Any(p2 => p2.ImplantableDevicesPKId == p.ImplantableDevicesPKId));
                    var ser = new JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        listImplantableDevices = ser.Serialize(listDevices),
                        implantableDevicesCount = listDevices.Count,
                        iTotalDisplayRecords = listDevices[0].RecordCount,
                        listNotesLinked = ser.Serialize(result)
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        listImplantableDevices = "[]",
                        implantableDevicesCount = 0,
                        iTotalDisplayRecords = 0
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string FillImplantableDevices(ImplantableDevices model)
        {
            try
            {
                var devicesList = new List<ImplantableDevicesWithProcedures>();
                var obj = BLLClinicalObj.fillImplantableDevices(MDVUtility.ToInt64(model.ImplantableDevicesPKId),
                        MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.PageNumber),
                        MDVUtility.ToInt64(model.RowsPerPage));
                if (obj.Data != null)
                {
                    devicesList = obj.Data;
                    if (devicesList.Count > 0)
                    {
                        var listDevices = new List<ImplantableDevices>(devicesList[0].implantableDevices);
                        var listProcedureDevices = new List<ImplantableDeviceProcedures>(devicesList[0].associatedProcedures);
                        var ser = new JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            implantableDevicesList = ser.Serialize(listDevices),
                            implantableDevicesCount = listDevices.Count,
                            implantableProceduresList=ser.Serialize(listProcedureDevices),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = AppPrivileges.No_Record_Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string VerifyUdi(ImplantableDevices model)
        {
            try
            {
                var listImplantableDevices = new List<ImplantableDevices>();
                var listGmdnp = new List<ImplantableDeviceGMPTName>();
                var listIdentifier = new List<ImplantableDeviceIdentifier>();
                var encodedUdi = HttpUtility.UrlEncode(model.UDI); //convert UDI into Percent-encoding(URL encoding)
                var url = @"https://accessgudid.nlm.nih.gov/api/v1/parse_udi.json?udi=" + MDVUtility.ToStr(encodedUdi);
                var syncClient = new WebClient();
                var json = syncClient.DownloadString(url);
                var ser = new JavaScriptSerializer();
                dynamic measureDetail = ser.Deserialize<object>(json);
                var udi = measureDetail["udi"] != null ? measureDetail["udi"] : "";
                
                if (model.UDI == MDVUtility.ToStr(udi))
                {
                    model.UDI = MDVUtility.ToStr(udi);
                    if (measureDetail.ContainsKey("di"))
                        model.DI = measureDetail["di"] != null ? MDVUtility.ToStr(measureDetail["di"]) : "";

                    if (measureDetail.ContainsKey("issuing_agency"))
                        model.Issuing_agency = measureDetail["issuing_agency"] != null ? MDVUtility.ToStr(measureDetail["issuing_agency"]) : "";

                    if (measureDetail.ContainsKey("manufacturing_date"))
                        model.Manufacturing_Date = measureDetail["manufacturing_date"] != null ? MDVUtility.ToStr(DateTime.Parse(MDVUtility.ToStr(measureDetail["manufacturing_date"]))) : "";

                    if (measureDetail.ContainsKey("serial_number"))
                        model.Serial_Number = measureDetail["serial_number"] != null ? MDVUtility.ToStr(measureDetail["serial_number"]) : "";

                    if (measureDetail.ContainsKey("expiration_date"))
                        model.Expiration_Date = measureDetail["expiration_date"] != null ? MDVUtility.ToStr(DateTime.Parse(MDVUtility.ToStr(measureDetail["expiration_date"]))) : "";

                    if (measureDetail.ContainsKey("lot_number"))
                        model.Lot_Number = measureDetail["lot_number"] != null ? MDVUtility.ToStr(measureDetail["lot_number"]) : "";

                    var urlDi = @"https://accessgudid.nlm.nih.gov/api/v1/devices/lookup.json?di=" + model.DI;
                    var syncClientDi = new WebClient();
                    var jsonDi = syncClientDi.DownloadString(urlDi);
                    var serDi = new JavaScriptSerializer();
                    dynamic item = serDi.Deserialize<object>(jsonDi);

                    model.BrandName = item["gudid"]["device"]["brandName"] != null ? MDVUtility.ToStr(item["gudid"]["device"]["brandName"]) : "";
                    model.VersionModelNumber = item["gudid"]["device"]["versionModelNumber"] != null ? MDVUtility.ToStr(item["gudid"]["device"]["versionModelNumber"]) : "";
                    model.CompanyName = item["gudid"]["device"]["companyName"] != null ? MDVUtility.ToStr(item["gudid"]["device"]["companyName"]) : "";
                    model.MRISafetyStatus = item["gudid"]["device"]["MRISafetyStatus"] != null ? MDVUtility.ToStr(item["gudid"]["device"]["MRISafetyStatus"]) : "";
                    model.LabeledContainsNRL = item["gudid"]["device"]["labeledContainsNRL"] != null ? MDVUtility.ToStr(item["gudid"]["device"]["labeledContainsNRL"]) : "";
                    model.DeviceDescription = item["gudid"]["device"]["deviceDescription"] != null ? MDVUtility.ToStr(item["gudid"]["device"]["deviceDescription"]) : "";

                    if (item["gudid"]["device"]["identifiers"] != null)
                    {
                        try
                        {
                            var count = ((object[]) item["gudid"]["device"]["identifiers"]["identifier"]).Count();
                            var countDiList = MDVUtility.ToInt(count);
                            if (countDiList > 1)
                            {
                                var data = ((object[]) item["gudid"]["device"]["identifiers"]["identifier"]);

                                listIdentifier.AddRange((from a in data let type = a.GetType() select a).Cast<Dictionary<string, object>>().Select(arr => new ImplantableDeviceIdentifier
                                {
                                    DeviceId = MDVUtility.ToStr(arr["deviceId"]),
                                    DeviceIdType = MDVUtility.ToStr(arr["deviceIdType"]),
                                    DeviceIdIssuingAgency = MDVUtility.ToStr(arr["deviceIdIssuingAgency"])
                                }));
                            }
                        }
                        catch (Exception ex)
                        {
                            model.DI = MDVUtility.ToStr(item["gudid"]["device"]["identifiers"]["identifier"]["deviceId"]);
                        }
                    }

                    if (item["gudid"]["device"]["gmdnTerms"] != null)
                    {
                        try
                        {
                            var count = ((object[]) item["gudid"]["device"]["gmdnTerms"]["gmdn"]).Count();
                            var countGmdnpList = MDVUtility.ToInt(count);
                            if (countGmdnpList > 1)
                            {
                                var data = ((object[]) item["gudid"]["device"]["gmdnTerms"]["gmdn"]);

                                listGmdnp.AddRange((from a in data let type = a.GetType() select a).Cast<Dictionary<string, object>>().Select(arr => new ImplantableDeviceGMPTName
                                {
                                    GMDNPName = MDVUtility.ToStr(arr["gmdnPTName"]),
                                    GMDNPTDefinition = MDVUtility.ToStr(arr["gmdnPTDefinition"])
                                }));
                            }
                        }
                        catch (Exception ex)
                        {
                            model.GMDNPName = MDVUtility.ToStr(item["gudid"]["device"]["gmdnTerms"]["gmdn"]["gmdnPTName"]);
                        }
                    }
                    listImplantableDevices.Add(model);
                    var response = new
                    {
                        status = true,
                        implantableDevicesList = listImplantableDevices,
                        GMDNPNamesList = listGmdnp,
                        DIList = listIdentifier
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "The Unique Device Identifier you have entered was not found in the Global UDI Database. Please re-enter the Unique Device Identifier."

                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (WebException exception)
            {
                string responseText = "";
                var responseStream = exception.Response != null ? exception.Response.GetResponseStream() : null;
                if (responseStream == null)
                    responseText = exception.Message;
                else
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                var response = new
                {
                    status = false,
                    message = responseText
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public ImplantableDevices GetUDIData(ImplantableDevices model)
        {
            try
            {
                var listImplantableDevices = new List<ImplantableDevices>();
                var listGmdnp = new List<ImplantableDeviceGMPTName>();
                var listIdentifier = new List<ImplantableDeviceIdentifier>();
                var encodedUdi = HttpUtility.UrlEncode(model.UDI); //convert UDI into Percent-encoding(URL encoding)
                var url = @"https://accessgudid.nlm.nih.gov/api/v1/parse_udi.json?udi=" + MDVUtility.ToStr(encodedUdi);
                var syncClient = new WebClient();
                var json = syncClient.DownloadString(url);
                var ser = new JavaScriptSerializer();
                dynamic measureDetail = ser.Deserialize<object>(json);
                var udi = measureDetail["udi"] != null ? measureDetail["udi"] : "";

                if (model.UDI == MDVUtility.ToStr(udi))
                {
                    model.UDI = MDVUtility.ToStr(udi);
                    if (measureDetail.ContainsKey("di"))
                        model.DI = measureDetail["di"] != null ? MDVUtility.ToStr(measureDetail["di"]) : "";

                    if (measureDetail.ContainsKey("issuing_agency"))
                        model.Issuing_agency = measureDetail["issuing_agency"] != null ? MDVUtility.ToStr(measureDetail["issuing_agency"]) : "";

                    if (measureDetail.ContainsKey("manufacturing_date"))
                        model.Manufacturing_Date = measureDetail["manufacturing_date"] != null ? MDVUtility.ToStr(DateTime.Parse(MDVUtility.ToStr(measureDetail["manufacturing_date"]))) : "";

                    if (measureDetail.ContainsKey("serial_number"))
                        model.Serial_Number = measureDetail["serial_number"] != null ? MDVUtility.ToStr(measureDetail["serial_number"]) : "";

                    if (measureDetail.ContainsKey("expiration_date"))
                        model.Expiration_Date = measureDetail["expiration_date"] != null ? MDVUtility.ToStr(DateTime.Parse(MDVUtility.ToStr(measureDetail["expiration_date"]))) : "";

                    if (measureDetail.ContainsKey("lot_number"))
                        model.Lot_Number = measureDetail["lot_number"] != null ? MDVUtility.ToStr(measureDetail["lot_number"]) : "";

                    var urlDi = @"https://accessgudid.nlm.nih.gov/api/v1/devices/lookup.json?di=" + model.DI;
                    var syncClientDi = new WebClient();
                    var jsonDi = syncClientDi.DownloadString(urlDi);
                    var serDi = new JavaScriptSerializer();
                    dynamic item = serDi.Deserialize<object>(jsonDi);

                    model.BrandName = item["gudid"]["device"]["brandName"] != null ? MDVUtility.ToStr(item["gudid"]["device"]["brandName"]) : "";
                    model.VersionModelNumber = item["gudid"]["device"]["versionModelNumber"] != null ? MDVUtility.ToStr(item["gudid"]["device"]["versionModelNumber"]) : "";
                    model.CompanyName = item["gudid"]["device"]["companyName"] != null ? MDVUtility.ToStr(item["gudid"]["device"]["companyName"]) : "";
                    model.MRISafetyStatus = item["gudid"]["device"]["MRISafetyStatus"] != null ? MDVUtility.ToStr(item["gudid"]["device"]["MRISafetyStatus"]) : "";
                    model.LabeledContainsNRL = item["gudid"]["device"]["labeledContainsNRL"] != null ? MDVUtility.ToStr(item["gudid"]["device"]["labeledContainsNRL"]) : "";
                    model.DeviceDescription = item["gudid"]["device"]["deviceDescription"] != null ? MDVUtility.ToStr(item["gudid"]["device"]["deviceDescription"]) : "";

                    if (item["gudid"]["device"]["identifiers"] != null)
                    {
                        try
                        {
                            model.DI = MDVUtility.ToStr(item["gudid"]["device"]["identifiers"]["identifier"]["deviceId"]);
                        }
                        catch (Exception ex)
                        {
                            model.DI = MDVUtility.ToStr(item["gudid"]["device"]["identifiers"]["identifier"]["deviceId"]);
                        }
                    }

                    if (item["gudid"]["device"]["gmdnTerms"] != null)
                    {
                        try
                        {
                            model.GMDNPName = MDVUtility.ToStr(item["gudid"]["device"]["gmdnTerms"]["gmdn"]["gmdnPTName"]);
                        }
                        catch (Exception ex)
                        {
                            model.GMDNPName = MDVUtility.ToStr(item["gudid"]["device"]["gmdnTerms"]["gmdn"]["gmdnPTName"]);
                        }
                    }
                    model.Status = "UDI Known";
                }
                else
                {
                    model.Status = "UDI Unknown";
                }
            }
            catch (WebException exception)
            {
                var responseStream = exception.Response != null ? exception.Response.GetResponseStream() : null;
                model.Status = "UDI Unknown";
            }
            return model;
        }

        public string InsertImplantableDevices(ImplantableDevices model)
        {
            try
            {
                if(model.ImplantableDeviceProcedure.Count > 0)
                {
                    model.ImplantableDeviceProceduresXML = MDVUtility.GetXmlOfObject(typeof(List<ImplantableDeviceProcedures>), model.ImplantableDeviceProcedure);
                }

                var obj = BLLClinicalObj.insertImplantableDevices(model);
                if (obj.Data != null)
                {
                    var implantableDevicesPkId = MDVUtility.ToInt64(model.ImplantableDevicesPKId);
                    if (implantableDevicesPkId != 0)
                    {
                        var response = new
                        {
                            status = true,
                            ImplantableDevicesPKId = implantableDevicesPkId,
                            message = AppPrivileges.Save_Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = model.ErrorMessage
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = obj.Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string UpdateImplantableDevices(ImplantableDevices model)
        {
            try
            {
                if (model.ImplantableDeviceProcedure.Count > 0)
                {
                    model.ImplantableDeviceProceduresXML = MDVUtility.GetXmlOfObject(typeof(List<ImplantableDeviceProcedures>), model.ImplantableDeviceProcedure);
                }

                var obj = BLLClinicalObj.updateImplantableDevices(model);
                if (obj.Data != null && obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        message = AppPrivileges.Update_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string DeleteImplantableDevices(ImplantableDevices model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ImplantableDevicesPKId)))
                {
                    var response = new
                    {
                        status = false,
                        message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    var obj = BLLClinicalObj.deleteImplantableDevices(model.ImplantableDevicesPKId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string DeleteImplantableDeviceProcedure(string ImplantableDeviceProcedureId)
        {
            try
            {
                if (string.IsNullOrEmpty(ImplantableDeviceProcedureId))
                {
                    var response = new
                    {
                        status = false,
                        message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    var obj = BLLClinicalObj.deleteImplantableDeviceProcedure(ImplantableDeviceProcedureId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string ActiveInactiveImplantableDevices(ImplantableDevices model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ImplantableDevicesPKId)))
                {
                    var response = new
                    {
                        status = false,
                        message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    var obj =
                        BLLClinicalObj.activeInactiveImplantableDevices(MDVUtility.ToInt64(model.ImplantableDevicesPKId), MDVUtility.ToInt64(model.IsActive));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Update_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Implantable Devices attached/detached with Note
        //public string getLatestImplantableDevicesByPatientId(Int64 PatientId)
        //{
        //    try
        //    {
        //        List<ImplantableDevices> devicesList = new List<ImplantableDevices>();
        //        BLObject<List<ImplantableDevices>> obj = BLLClinicalObj.getLatestImplantableDevicesByPatientId(PatientId);

        //        var response = new
        //        {
        //            status = true,
        //            listImplantableDevices = obj.Data,
        //            implantableDevicesCount = obj.Data.Count
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message = MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}

        public string GetDevicesForSoap(string implantableDevicesPkId, long patientId, long NoteProviderId)
        {
            try
            {
                var obj = BLLClinicalObj.getDevicesForSoap(implantableDevicesPkId, patientId, NoteProviderId);
                if (obj.Data != null && obj.Data.Count != 0)
                {
                    var response = new
                    {
                        status = true,
                        listImplantableDevices = obj.Data,
                        implantableDevicesCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        listImplantableDevices = "[]",
                        implantableDevicesCount = 0,
                        iTotalDisplayRecords = 0
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string AttachDevicesWithNotes(string implantableDevicesPkId, string notesId)
        {
            try
            {
                var obj = BLLClinicalObj.attachDevicesWithNotes(implantableDevicesPkId, notesId);
                if (obj.Data != null && obj.Data != "-1")
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Message
                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string DetachDevicesFromNotes(string implantableDevicesPkId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(implantableDevicesPkId)) ||
                    string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    var obj = BLLClinicalObj.detachDevicesFromNotes(implantableDevicesPkId, notesId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string insertAssociatedProcedures(ImplantableDevices model)
        {
            try
            {
                var obj = BLLClinicalObj.insertAssociatedProcedures(model);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Message,
                        ProcedureIds = obj.Data
                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Lookups
        public string TargetSiteLookUp(string TargetSite)
        {
            try
            {
                var obj = BLLClinicalObj.LookupTargetSite(TargetSite);
                if (obj.Data != null)
                {
                    if (obj.Data.Count > 0)
                    {
                        var ser = new JavaScriptSerializer();
                        ser.MaxJsonLength = 5000000;
                        var response = new
                        {
                            status = true,
                            TargetSiteCount = obj.Data.Count,
                            TargetSiteLoad_JSON = ser.Serialize(obj.Data),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            TargetSiteCount = 0,
                            Message = obj.Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion

    }
}
