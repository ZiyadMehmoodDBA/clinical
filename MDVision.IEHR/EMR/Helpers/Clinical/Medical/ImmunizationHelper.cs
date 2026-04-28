using System;
using MDVision.Business.BCommon;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.Datasets;
using System.Collections.Generic;
using System.Data;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using System.Linq;
using NHapi.Base;
using NHapi.Model.V251;
using NHapi.Model.V251.Message;
using NHapi.Model.V251.Segment;
using MDVision.IEHR.EMR.Model.Clinical.Immunization;
using MDVision.Model.Clinical.Reports;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using System.Web.Script.Serialization;
using MDVision.Model.Clinical.Immunization;
using MDVision.IEHR.EMR.Helpers.Clinical.ClinicalNotes;
using System.Globalization;
using NHapi.Base.Parser;
using System.IO;
using HL7Parser;
using MDVision.Model.Clinical.HL7;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Medical
{
    public class ImmunizationHelper
    {
        private BLLClinical BLLClinicalObj = null;

        public ImmunizationHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static ImmunizationHelper _instance = null;
        public static ImmunizationHelper Instance()
        {
            if (_instance == null)
                _instance = new ImmunizationHelper();
            return _instance;
        }

        //  public  SharedVariable SharedObj;

        public string GetProviderId(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.NotesId)))
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

                    BLObject<string> obj = BLLClinicalObj.GetProviderId(MDVUtility.ToLong(model.NotesId));
                    if (obj.Data != "")
                    {
                        var response = new
                        {
                            status = true,
                            ProviderId = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Exception in sp_GetPatientProviderId"
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

        public string CheckPatientInsuranceIsMedicare(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)))
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

                    BLObject<string> obj = BLLClinicalObj.CheckPatientInsuranceIsMedicare(MDVUtility.ToLong(model.PatientId));
                    if (obj.Data != "")
                    {
                        var response = new
                        {
                            status = true,
                            IsMedicare = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Exception in sp_CheckPatientInsuranceIsMedicare"
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

        public string IsLastAdministeredDoes(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.VaccineHxId)))
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
                    BLObject<string> obj = BLLClinicalObj.IsLastAdministeredDoes(MDVUtility.ToLong(model.VaccineHxId), MDVUtility.ToLong(model.PatientId), model.VoidDose, MDVUtility.ToLong(model.VaccineScheduleId), MDVUtility.ToLong(model.VaccineID));
                    if (obj.Data != "0" || obj.Data != "1")
                    {
                        var response = new
                        {
                            status = true,
                            IsLastAdministeredDoes = obj.Data
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

        public string IsAdministrationPeriodOver(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.VaccineScheduleId)))
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
                    BLObject<string> obj = BLLClinicalObj.IsAdministrationPeriodOver(MDVUtility.ToLong(model.VaccineScheduleId), MDVUtility.ToLong(model.PatientId));
                    if (obj.Data != "0" || obj.Data != "1")
                    {
                        var response = new
                        {
                            status = true,
                            IsOver = obj.Data
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



        public string GetCptOfVaccine(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.VaccineID)))
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

                    BLObject<string> obj = BLLClinicalObj.GetCptOfVaccine(MDVUtility.ToLong(model.VaccineID));
                    if (obj.Data != "")
                    {
                        if (obj.Data == "-1")
                        {
                            var response = new
                            {
                                status = true,
                                Cpt = ""
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                Cpt = obj.Data
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Exception in sp_GetCptOfVaccine"
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

        public string GetVaccineSchedulerId(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.CategoryID)) || string.IsNullOrEmpty(model.ScheduleShortName))
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

                    BLObject<string> obj = BLLClinicalObj.GetVaccineSchedulerId(MDVUtility.ToLong(model.CategoryID), model.ScheduleShortName);
                    if (obj.Data != "")
                    {
                        var response = new
                        {
                            status = true,
                            VaccineSchedulerId = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Exception in sp_GetVaccineScheduleId"
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

        public string WhyLotIsNotAvailable(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.VaccineID)) && string.IsNullOrEmpty(MDVUtility.ToStr(model.ProviderId)))
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

                    BLObject<string> obj = BLLClinicalObj.WhyLotIsNotAvailable(MDVUtility.ToLong(model.VaccineID), MDVUtility.ToLong(model.ProviderId), model.Type);
                    if (obj.Data == "1" || obj.Data == "2" || obj.Data == "3" || obj.Data == "4")
                    {
                        var response = new
                        {
                            status = true,
                            WhyLotIsNotAvailable = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Exception in WhyLotIsNotAvailable"
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
        public string GetVaccineHxIds(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.VaccineHxId)))
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

                    BLObject<string> obj = BLLClinicalObj.GetVaccineHxIds(MDVUtility.ToLong(model.VaccineHxId));
                    if (obj.Data != "")
                    {
                        var response = new
                        {
                            status = true,
                            VaccineHxIds = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Exception in GetVaccineHxIds"
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

        public string GetLotManufanucture(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.LotId)))
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
                    List<Manufacturer> ManufacturerList = new List<Manufacturer>();
                    BLObject<List<Manufacturer>> obj = BLLClinicalObj.GetLotManufanucture(MDVUtility.ToLong(model.LotId));
                    ManufacturerList = obj.Data;
                    if (obj.Data != null)
                    {

                        if (ManufacturerList.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                ManufactureId = ManufacturerList[0].ManufacturerId,
                                ManufacturerName = ManufacturerList[0].ManufacturerName,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                ManufactureId = "No Data Found"
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Exception in GetLotManufanucture"
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


        public string GetProcedureIdsAgainstVaccAndImm(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.VaccineHxIds)) && string.IsNullOrEmpty(MDVUtility.ToStr(model.ImmTherInjectionIds)))
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

                    BLObject<string> obj = BLLClinicalObj.GetProcedureIdsAgainstVaccAndImm(model.VaccineHxIds, model.ImmTherInjectionIds);
                    if (obj.Data != "")
                    {
                        var response = new
                        {
                            status = true,
                            ProcedureIds = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ProcedureIds = 0,
                            Message = "No Procedure Found"
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

        public string loadImmunization(ImmunizationModel model)
        {
            try
            {
                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;
                //   obj = BLLClinicalObj.loadImmunization(MDVUtility.ToLong(model.ImmunizationHxId), MDVUtility.ToLong(model.PatientId), MDVUtility.ToLong(model.NoteId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                obj = BLLClinicalObj.loadImmunization(9, 9, 9, "", "", 9, 9);

                dsImmunization = obj.Data;
                if (obj.Data != null)
                {
                    int ImmunizationTotalCount = 0;
                    if (dsImmunization.Tables[dsImmunization.ImmunizationHx.TableName].Rows.Count == 0)
                    {
                        if (model.IsActive.Equals("1"))
                        {
                            //  obj = BLLClinicalObj.loadImmunization(MDVUtility.ToInt64(model.ImmunizationId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToLong(model.NoteId), "0", "0", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                        }
                        else
                        {
                            //    obj = BLLClinicalObj.loadImmunization(MDVUtility.ToInt64(model.ImmunizationId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToLong(model.NoteId), "0", "1", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                        }

                        if (obj.Data != null)
                        {
                            DSImmunization dsImmunizationInActive = obj.Data;
                            ImmunizationTotalCount = dsImmunizationInActive.Tables[dsImmunization.ImmunizationHx.TableName].Rows.Count;
                        }
                    }
                    else
                    {
                        ImmunizationTotalCount = dsImmunization.Tables[dsImmunization.ImmunizationHx.TableName].Rows.Count;
                    }
                    var response = new
                    {
                        status = true,
                        //ImmunizationTotalCount = ImmunizationTotalCount,
                        //ImmunizationCount = dsImmunization.Tables[dsImmunization.ImmunizationHx.TableName].Rows.Count,
                        //ImmunizationLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.ImmunizationHx.TableName]),
                        //ImmunizationHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.ImmunizationHistory.TableName]),
                        //ImmunizationReview_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.ImmunizationHxReview.TableName]),
                        //iTotalDisplayRecords = (dsImmunization.ImmunizationHx.Rows.Count > 0) ? dsImmunization.ImmunizationHx.Rows[0][dsImmunization.ImmunizationHx.RecordCountColumn.ColumnName] : 0,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ImmunizationCount = 0,
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


        // Created By Talha Tanweer
        // Created Date: 08/04/2016
        //OverView:
        public string GetVISDate(ImmunizationModel model)
        {
            try
            {
                DSImmunization dsImmunization = null;
                BLObject<string> obj;
                //   obj = BLLClinicalObj.loadImmunization(MDVUtility.ToLong(model.ImmunizationHxId), MDVUtility.ToLong(model.PatientId), MDVUtility.ToLong(model.NoteId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                obj = BLLClinicalObj.GetVaccineVISDate(model.VaccineID);
                /////////////////////////////////////// dsImmunization = obj.Data;


                JavaScriptSerializer js = new JavaScriptSerializer();
                if (!string.IsNullOrEmpty(obj.Data))
                {
                    var VisDateOrId = obj.Data.Split(',');
                    var VisId = 0;
                    var VisDate = "";
                    if (VisDateOrId.Length > 0)
                    {
                        VisId = MDVUtility.ToInt(VisDateOrId[0]);
                    }
                    if (VisDateOrId.Length > 1)
                    {
                        VisDate = VisDateOrId[1];
                    }

                    var response = new
                    {
                        status = true,
                        VISDate = VisDate,
                        VISId = VisId
                    };
                    return (JsonConvert.SerializeObject(response));


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "no data found"
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
        public string GetVISDateAndVisURL(ImmunizationModel model)
        {
            try
            {
                List<VaccineVIS> VaccineVISDetail = null;
                BLObject<List<VaccineVIS>> obj;

                obj = BLLClinicalObj.GetVISDateAndVisURL(model.VaccineID);
                VaccineVISDetail = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        VaccineVISCount = VaccineVISDetail.Count,
                        VaccineVIS_JSON = VaccineVISDetail,
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

        // Created By Talha Tanweer
        // Created Date: 08/04/2016
        //OverView:
        public string GetVIS_URL(ImmunizationModel model)
        {
            try
            {
                DSImmunization dsImmunization = null;
                BLObject<string> obj;
                obj = BLLClinicalObj.GetVaccineVISURL(model.VaccineID);

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var response = new
                {
                    status = true,
                    VIS_url = obj.Data == "Object reference not set to an instance of an object." ? "" : obj.Data
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

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



        // Created By Talha Tanweer
        // Created Date: 08/04/2016
        //OverView: Methods "GetUserME" get data of current user from app.config
        public string GetUserME_IdandName(ImmunizationModel model)
        {
            try
            {
                //   long userId =
                //string userName =   AppConfig.AppUserFullName;


                var response = new
                {
                    userId = MDVSession.Current.AppUserId,
                    userName = MDVSession.Current.AppUserFullName
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);


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


        // Created By Talha Tanweer
        // Created Date: 08/04/2016
        //OverView: Methods "SaveAministerVaccine" for Call BLL for Save Administer Vaccine Data

        //public string SaveAministerVaccine(ImmunizationModel model)
        //{
        //    try
        //    {
        //        DSImmunization dsImmunization = new DSImmunization();
        //        DSImmunization.VaccineHxRow dr = dsImmunization.VaccineHx.NewVaccineHxRow();

        //        dr.VaccineGroupCategory = MDVUtility.ToInt64(model.CategoryID);
        //        if (model.VaccineScheduleId != "0" && model.VaccineScheduleId != "")
        //        {
        //            dr.VaccineScheduleId = MDVUtility.ToInt64(model.VaccineScheduleId);
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.VaccineScheduleIdColumn] = DBNull.Value;
        //        }
        //        dr.PatientAge = model.PatientAge;
        //        if (!string.IsNullOrEmpty(model.VisitDate) && model.VisitDate != "- Select -")
        //            dr.VisitDate = MDVUtility.ToDateTime(model.VisitDate);
        //        else
        //            dr[dsImmunization.VaccineHx.VisitDateColumn] = DBNull.Value;
        //        if (!string.IsNullOrEmpty(model.VisitDateId))
        //            dr.VisitDateId = MDVUtility.ToInt64(model.VisitDateId);
        //        else
        //            dr[dsImmunization.VaccineHx.VisitDateIdColumn] = DBNull.Value;
        //        if (!string.IsNullOrEmpty(model.ProviderId))
        //        {
        //            dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.ProviderIdColumn] = DBNull.Value;
        //        }
        //        dr.Vaccine = MDVUtility.ToInt64(model.VaccineID);
        //        if (!string.IsNullOrEmpty(model.AdministrationDate))
        //        {
        //            DateTime Visdate = MDVUtility.ToDateTime(model.AdministrationDate);
        //            DateTime combined;



        //            if (!string.IsNullOrEmpty(model.Time))
        //            {
        //                var cSplit = model.Time.Split(':');
        //                var hours = "12";
        //                if (cSplit.Length > 0)
        //                {
        //                    hours = cSplit[0];
        //                }
        //                var mints = "0";
        //                var AmOrPm = "AM";
        //                if (cSplit.Length > 1)
        //                {
        //                    var sSplit = cSplit[1].Split(' ');
        //                    if (sSplit.Length > 0)
        //                    {
        //                        mints = sSplit[0];
        //                    }
        //                    if (sSplit.Length > 1)
        //                    {
        //                        AmOrPm = sSplit[1];
        //                    }
        //                }
        //                if (AmOrPm == "AM")
        //                {
        //                    if (hours == "12")
        //                    {
        //                        string time = "0:" + mints + ":0";
        //                        combined = Visdate.Add(TimeSpan.Parse(time));
        //                    }
        //                    else
        //                    {
        //                        string time = hours + ":" + mints + ":0";
        //                        combined = Visdate.Add(TimeSpan.Parse(time));
        //                    }
        //                }
        //                else
        //                {
        //                    hours = hours == "12" ? hours : (MDVUtility.ToInt(hours) + 12).ToString();
        //                    string time = hours + ":" + mints + ":0";
        //                    combined = Visdate.Add(TimeSpan.Parse(time));
        //                }
        //            }
        //            else
        //            {
        //                combined = MDVUtility.ToDateTime(model.AdministrationDate);
        //            }
        //            dr.AdministrationDate = combined;
        //        }
        //        else
        //            dr[dsImmunization.VaccineHx.AdministrationDateColumn] = DBNull.Value;

        //        if (!string.IsNullOrEmpty(model.Dose))
        //        {
        //            dr.Dose = MDVUtility.ToDecimal(model.Dose);
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.DoseColumn] = DBNull.Value;
        //        }
        //        dr.Amount = model.Amount;
        //        if (!string.IsNullOrEmpty(model.LotNo))
        //        {
        //            dr.LotNumber = MDVUtility.ToLong(model.LotNo);
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.LotNumberColumn] = DBNull.Value;
        //        }
        //        if (!string.IsNullOrEmpty(model.ManufacturerId))
        //        {
        //            dr.Manufacturer = MDVUtility.ToInt64(model.ManufacturerId);
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.ManufacturerColumn] = DBNull.Value;
        //        }
        //        if (!string.IsNullOrEmpty(model.RouteId))
        //        {
        //            dr.Route = MDVUtility.ToInt64(model.RouteId);
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.RouteColumn] = DBNull.Value;
        //        }
        //        if (!string.IsNullOrEmpty(model.SourceOfHxId))
        //        {
        //            dr.SourceOfHx = MDVUtility.ToInt64(model.SourceOfHxId);
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.SourceOfHxColumn] = DBNull.Value;
        //        }
        //        if (!string.IsNullOrEmpty(model.SiteId))
        //        {
        //            dr.Site = MDVUtility.ToInt64(model.SiteId);
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.SiteColumn] = DBNull.Value;
        //        }
        //        if (!string.IsNullOrEmpty(model.ExpiryDate))
        //            dr.ExpiryDate = MDVUtility.ToDateTime(model.ExpiryDate);
        //        else
        //            dr[dsImmunization.VaccineHx.ExpiryDateColumn] = DBNull.Value;

        //        if (!string.IsNullOrEmpty(model.VfcId))
        //        {
        //            dr.VFC = MDVUtility.ToInt64(model.VfcId);
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.VFCColumn] = DBNull.Value;
        //        }


        //        if (MDVUtility.ToInt64(model.VisDateId) != 0)
        //        {
        //            dr.VISDate = MDVUtility.ToInt64(model.VisDateId);
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.VISDateColumn] = DBNull.Value;
        //        }
        //        if (!string.IsNullOrEmpty(model.Reaction))
        //        {
        //            dr.Reaction = MDVUtility.ToInt64(model.Reaction);
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.ReactionColumn] = DBNull.Value;
        //        }
        //        dr.VoidDose = model.VoidDose;

        //        if (!string.IsNullOrEmpty(model.Comments))
        //        {
        //            dr.Comments = model.Comments;
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.CommentsColumn] = DBNull.Value;
        //        }

        //        if (!string.IsNullOrEmpty(model.PublicityCode))
        //        {
        //            dr.PublicityCode = model.PublicityCode;
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.PublicityCodeColumn] = DBNull.Value;
        //        }

        //        if (!string.IsNullOrEmpty(model.PublicityCodeExpiryDate))
        //            dr.PublicityCodeExpiryDate = MDVUtility.ToDateTime(model.PublicityCodeExpiryDate);
        //        else
        //            dr[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn] = DBNull.Value;

        //        if (!string.IsNullOrEmpty(model.ImmunizationRegistryStatusCode))
        //        {
        //            dr.ImmunizationRegistryStatusCode = model.ImmunizationRegistryStatusCode;
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.ImmunizationRegistryStatusCodeColumn] = DBNull.Value;
        //        }

        //        if (!string.IsNullOrEmpty(model.IRSEffectiveDate))
        //            dr.IRSEffectiveDate = MDVUtility.ToDateTime(model.IRSEffectiveDate);
        //        else
        //            dr[dsImmunization.VaccineHx.IRSEffectiveDateColumn] = DBNull.Value;


        //        dr.ProtectionIndicator = model.ProtectionIndicator;
        //        if (!string.IsNullOrEmpty(model.PIEffectiveDate))
        //            dr.PIEffectiveDate = MDVUtility.ToDateTime(model.PIEffectiveDate);
        //        else
        //            dr[dsImmunization.VaccineHx.PIEffectiveDateColumn] = DBNull.Value;
        //        if (!string.IsNullOrEmpty(model.RefusalReasonID))
        //            dr.RefusalReasonId = MDVUtility.ToLong(model.RefusalReasonID);
        //        else
        //            dr[dsImmunization.VaccineHx.RefusalReasonIdColumn] = DBNull.Value;

        //        if (!string.IsNullOrEmpty(model.Type) && model.Type == "ADMINISTER")
        //        {
        //            dr.OverrideRule = model.OverrideRule;
        //        }
        //        else
        //        {
        //            dr[dsImmunization.VaccineHx.OverrideRuleColumn] = DBNull.Value;
        //        }
        //        dr.GivenBy = MDVSession.Current.AppUserId;
        //        dr.Type = (model.Type);
        //        dr.PatientId = MDVUtility.ToInt64(model.PatientId);
        //        dr.ISActive = MDVUtility.ToByte(model.IsActive);

        //        //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields
        //        dr.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
        //        dr.CreatedOn = DateTime.Now;
        //        dr.ModifiedOn = DateTime.Now;
        //        dr.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

        //        #region Database Insertion
        //        dsImmunization.VaccineHx.AddVaccineHxRow(dr);
        //        BLObject<DSImmunization> obj = BLLClinicalObj.InsertVaccineHx(dsImmunization);
        //        dsImmunization = obj.Data;

        //        if (obj.Data != null)
        //        {
        //            var response = new
        //            {
        //                status = true,
        //                message = Common.AppPrivileges.Save_Message,
        //                VaccineHxIdColumn = dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows[0][dsImmunization.VaccineHx.VaccineHxIdColumn.ColumnName]
        //            };
        //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = obj.Message
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        #endregion
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

        public string InsertAministerVaccine(ImmunizationModel model)
        {
            try
            {
                string privilegasMessage = string.Empty;
                bool IsAddProcedures = false;
                if (model.Type.ToLower() == "administer")
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "ADD")).ToString();
                    if (string.IsNullOrWhiteSpace(privilegasMessage))
                    {
                        IsAddProcedures = true;
                    }
                }
                DSImmunization dsImmunization = new DSImmunization();
                DSImmunization.VaccineHxRow dr = dsImmunization.VaccineHx.NewVaccineHxRow();

                dr.VaccineGroupCategory = MDVUtility.ToInt64(model.CategoryID);
                if (model.VaccineScheduleId != "0" && model.VaccineScheduleId != "")
                {
                    dr.VaccineScheduleId = MDVUtility.ToInt64(model.VaccineScheduleId);
                }
                else
                {
                    dr[dsImmunization.VaccineHx.VaccineScheduleIdColumn] = DBNull.Value;
                }
                dr.PatientAge = model.PatientAge;
                if (!string.IsNullOrEmpty(model.VisitDate) && model.VisitDate != "- Select -")
                    dr.VisitDate = MDVUtility.ToDateTime(model.VisitDate);
                else
                    dr[dsImmunization.VaccineHx.VisitDateColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.VisitDateId))
                    dr.VisitDateId = MDVUtility.ToInt64(model.VisitDateId);
                else
                    dr[dsImmunization.VaccineHx.VisitDateIdColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.ProviderId))
                {
                    dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                }
                else
                {
                    dr[dsImmunization.VaccineHx.ProviderIdColumn] = DBNull.Value;
                }
                dr.Vaccine = MDVUtility.ToInt64(model.VaccineID);
                if (!string.IsNullOrEmpty(model.AdministrationDate))
                {
                    DateTime Visdate = MDVUtility.ToDateTime(model.AdministrationDate);
                    DateTime combined;
                    if (!string.IsNullOrEmpty(model.Time))
                    {
                        var cSplit = model.Time.Split(':');
                        var hours = "12";
                        if (cSplit.Length > 0)
                        {
                            hours = cSplit[0];
                        }
                        var mints = "0";
                        var AmOrPm = "AM";
                        if (cSplit.Length > 1)
                        {
                            var sSplit = cSplit[1].Split(' ');
                            if (sSplit.Length > 0)
                            {
                                mints = sSplit[0];
                            }
                            if (sSplit.Length > 1)
                            {
                                AmOrPm = sSplit[1];
                            }
                        }
                        if (AmOrPm == "AM")
                        {
                            if (hours == "12")
                            {
                                string time = "0:" + mints + ":0";
                                combined = Visdate.Add(TimeSpan.Parse(time));
                            }
                            else
                            {
                                string time = hours + ":" + mints + ":0";
                                combined = Visdate.Add(TimeSpan.Parse(time));
                            }
                        }
                        else
                        {
                            hours = hours == "12" ? hours : (MDVUtility.ToInt(hours) + 12).ToString();
                            string time = hours + ":" + mints + ":0";
                            combined = Visdate.Add(TimeSpan.Parse(time));
                        }
                    }
                    else
                    {
                        combined = MDVUtility.ToDateTime(model.AdministrationDate);
                    }
                    dr.AdministrationDate = combined;
                }
                else
                    dr[dsImmunization.VaccineHx.AdministrationDateColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.Dose))
                {
                    dr.Dose = MDVUtility.ToDecimal(model.Dose);
                }
                else
                {
                    dr[dsImmunization.VaccineHx.DoseColumn] = DBNull.Value;
                }
                dr.Amount = model.Amount;
                if (!string.IsNullOrEmpty(model.LotNo))
                {
                    dr.LotNumber = MDVUtility.ToLong(model.LotNo);
                }
                else
                {
                    dr[dsImmunization.VaccineHx.LotNumberColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.ManufacturerId))
                {
                    dr.Manufacturer = MDVUtility.ToInt64(model.ManufacturerId);
                }
                else
                {
                    dr[dsImmunization.VaccineHx.ManufacturerColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.RouteId))
                {
                    dr.Route = MDVUtility.ToInt64(model.RouteId);
                }
                else
                {
                    dr[dsImmunization.VaccineHx.RouteColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.SourceOfHxId))
                {
                    dr.SourceOfHx = MDVUtility.ToInt64(model.SourceOfHxId);
                }
                else
                {
                    dr[dsImmunization.VaccineHx.SourceOfHxColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.SiteId))
                {
                    dr.Site = MDVUtility.ToInt64(model.SiteId);
                }
                else
                {
                    dr[dsImmunization.VaccineHx.SiteColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.ExpiryDate))
                    dr.ExpiryDate = MDVUtility.ToDateTime(model.ExpiryDate);
                else
                    dr[dsImmunization.VaccineHx.ExpiryDateColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.VfcId))
                {
                    dr.VFC = MDVUtility.ToInt64(model.VfcId);
                }
                else
                {
                    dr[dsImmunization.VaccineHx.VFCColumn] = DBNull.Value;
                }


                if (MDVUtility.ToInt64(model.VisDateId) != 0)
                {
                    dr.VISDate = MDVUtility.ToInt64(model.VisDateId);
                }
                else
                {
                    dr[dsImmunization.VaccineHx.VISDateColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.Reaction))
                {
                    dr.Reaction = MDVUtility.ToInt64(model.Reaction);
                }
                else
                {
                    dr[dsImmunization.VaccineHx.ReactionColumn] = DBNull.Value;
                }
                dr.VoidDose = model.VoidDose;

                if (!string.IsNullOrEmpty(model.Comments))
                {
                    dr.Comments = model.Comments;
                }
                else
                {
                    dr[dsImmunization.VaccineHx.CommentsColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.PublicityCode))
                {
                    dr.PublicityCode = model.PublicityCode;
                }
                else
                {
                    dr[dsImmunization.VaccineHx.PublicityCodeColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.PublicityCodeExpiryDate))
                    dr.PublicityCodeExpiryDate = MDVUtility.ToDateTime(model.PublicityCodeExpiryDate);
                else
                    dr[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.ImmunizationRegistryStatusCode))
                {
                    dr.ImmunizationRegistryStatusCode = model.ImmunizationRegistryStatusCode;
                }
                else
                {
                    dr[dsImmunization.VaccineHx.ImmunizationRegistryStatusCodeColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.IRSEffectiveDate))
                    dr.IRSEffectiveDate = MDVUtility.ToDateTime(model.IRSEffectiveDate);
                else
                    dr[dsImmunization.VaccineHx.IRSEffectiveDateColumn] = DBNull.Value;


                dr.ProtectionIndicator = model.ProtectionIndicator;
                if (!string.IsNullOrEmpty(model.PIEffectiveDate))
                    dr.PIEffectiveDate = MDVUtility.ToDateTime(model.PIEffectiveDate);
                else
                    dr[dsImmunization.VaccineHx.PIEffectiveDateColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.FacilityId))
                    dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
                else
                    dr[dsImmunization.VaccineHx.FacilityIdColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.UserId))
                    dr.UserId = MDVUtility.ToInt64(model.UserId);
                else
                    dr[dsImmunization.VaccineHx.UserIdColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.RegisteryId))
                    dr.ReceivingApplicationId = MDVUtility.ToInt64(model.RegisteryId);
                else
                    dr[dsImmunization.VaccineHx.ReceivingApplicationIdColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.RefusalReasonID))
                    dr.RefusalReasonId = MDVUtility.ToLong(model.RefusalReasonID);
                else
                    dr[dsImmunization.VaccineHx.RefusalReasonIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.Type) && model.Type == "ADMINISTER")
                {
                    dr.OverrideRule = model.OverrideRule;
                }
                else
                {
                    dr[dsImmunization.VaccineHx.OverrideRuleColumn] = DBNull.Value;
                }
                dr.GivenBy = MDVSession.Current.AppUserId;
                dr.Type = (model.Type);
                dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                dr.ISActive = MDVUtility.ToByte(model.IsActive);

                //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields
                dr.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                dr.IsHistoryDose = model.IsHistoryDose;
                #region Database Insertion
                dsImmunization.VaccineHx.AddVaccineHxRow(dr);
                BLObject<DSImmunization> obj = BLLClinicalObj.InsertAministerVaccine(dsImmunization, model.PreviousVoid, IsAddProcedures, model.NotesId, model.FavListNames, model.FavListVal);
                dsImmunization = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        VaccineHxIdColumn = dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows[0][dsImmunization.VaccineHx.VaccineHxIdColumn.ColumnName]
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

        public string Save_Therapeutic_Injection(TherapeuticInjectionModel model)
        {
            try
            {
                DSImmunization dsImmunization = new DSImmunization();
                DSImmunization.TherapeuticInjectionRow dr = dsImmunization.TherapeuticInjection.NewTherapeuticInjectionRow();

                dr.TherapeuticInjectionId = MDVUtility.ToInt64(model.TherapeuticInjectionId);


                if (!string.IsNullOrEmpty(model.VisitDate_text) && model.VisitDate != "- Select -")
                    dr.VisitDate = MDVUtility.ToDateTime(model.VisitDate_text);
                else
                    dr[dsImmunization.TherapeuticInjection.VisitDateColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.ProviderId))
                    dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                else
                    dr[dsImmunization.TherapeuticInjection.ProviderIdColumn] = DBNull.Value;


                if (!string.IsNullOrEmpty(model.AdministrationDate))
                {
                    DateTime Visdate = MDVUtility.ToDateTime(model.AdministrationDate);
                    DateTime combined;



                    if (!string.IsNullOrEmpty(model.AdministrationTime))
                    {
                        var cSplit = model.AdministrationTime.Split(':');
                        var hours = "12";
                        if (cSplit.Length > 0)
                        {
                            hours = cSplit[0];
                        }
                        var mints = "0";
                        var AmOrPm = "AM";
                        if (cSplit.Length > 1)
                        {
                            var sSplit = cSplit[1].Split(' ');
                            if (sSplit.Length > 0)
                            {
                                mints = sSplit[0];
                            }
                            if (sSplit.Length > 1)
                            {
                                AmOrPm = sSplit[1];
                            }
                        }
                        if (AmOrPm == "AM")
                        {
                            if (hours == "12")
                            {
                                string time = "0:" + mints + ":0";
                                combined = Visdate.Add(TimeSpan.Parse(time));
                            }
                            else
                            {
                                string time = hours + ":" + mints + ":0";
                                combined = Visdate.Add(TimeSpan.Parse(time));
                            }
                        }
                        else
                        {
                            hours = hours == "12" ? hours : (MDVUtility.ToInt(hours) + 12).ToString();
                            string time = hours + ":" + mints + ":0";
                            combined = Visdate.Add(TimeSpan.Parse(time));
                        }
                    }
                    else
                    {
                        combined = MDVUtility.ToDateTime(model.AdministrationDate);
                    }
                    dr.AdministrationDate = combined;
                }
                else
                    dr[dsImmunization.TherapeuticInjection.AdministrationDateColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.Dose))
                {
                    dr.Dose = MDVUtility.ToDouble(model.Dose);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.DoseColumn] = DBNull.Value;
                }
                dr.Amount = model.Amount;

                if (!string.IsNullOrEmpty(model.ManufacturerId))
                {
                    dr.ManufacturerId = MDVUtility.ToInt64(model.ManufacturerId);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.ManufacturerIdColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.RouteId))
                {
                    dr.RouteId = MDVUtility.ToInt64(model.RouteId);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.RouteIdColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.SiteId))
                {
                    dr.SiteId = MDVUtility.ToInt64(model.SiteId);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.SiteIdColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.ExpiryDate))
                    dr.ExpiryDate = MDVUtility.ToDateTime(model.ExpiryDate);
                else
                    dr[dsImmunization.TherapeuticInjection.ExpiryDateColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.VFC))
                {
                    dr.VFC = MDVUtility.ToInt64(model.VFC);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.VFCColumn] = DBNull.Value;
                }



                if (!string.IsNullOrEmpty(model.ReactionId))
                {
                    dr.ReactionId = MDVUtility.ToInt16(model.ReactionId);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.ReactionIdColumn] = DBNull.Value;
                }


                if (!string.IsNullOrEmpty(model.Comments))
                {
                    dr.Comments = model.Comments;
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.CommentsColumn] = DBNull.Value;
                }


                if (!string.IsNullOrEmpty(model.SourceOfHx))
                {
                    dr.SourceOfHx = MDVUtility.ToInt64(model.SourceOfHx);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.SourceOfHxColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.LotNumber))
                {
                    dr.LotNumber = MDVUtility.ToInt64(model.LotNumber);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.LotNumberColumn] = DBNull.Value;
                }

                dr.Type = model.Type;

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields

                dr.ModifiedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                dr.CreatedOn = DateTime.Now;
                dr.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

                #region Database Insertion
                dsImmunization.TherapeuticInjection.AddTherapeuticInjectionRow(dr);
                BLObject<DSImmunization> obj = BLLClinicalObj.InsertTherapeuticInjection(dsImmunization);
                dsImmunization = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        TherapeuticInjectionIdColumn = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows[0][dsImmunization.TherapeuticInjection.ImmTherInjectionIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = obj.Message
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

        public string Update_Therapeutic_Injection(TherapeuticInjectionModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ImmTherInjectionId) > 0)
                {

                    DSImmunization dsImmunization = new DSImmunization();
                    BLObject<DSImmunization> obj = BLLClinicalObj.LoadImmunizationTherapeuticInjection(MDVUtility.ToInt64(model.ImmTherInjectionId), 0, 0);
                    dsImmunization = obj.Data;
                    DSImmunization.TherapeuticInjectionRow dr1 = null;
                    foreach (DSImmunization.TherapeuticInjectionRow dr in dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows)
                    {
                        dr.TherapeuticInjectionId = MDVUtility.ToInt64(model.TherapeuticInjectionId);


                        if (!string.IsNullOrEmpty(model.VisitDate_text) && model.VisitDate != "- Select -")
                            dr.VisitDate = MDVUtility.ToDateTime(model.VisitDate_text);
                        else
                            dr[dsImmunization.TherapeuticInjection.VisitDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.ProviderId))
                            dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                        else
                            dr[dsImmunization.TherapeuticInjection.ProviderIdColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.AdministrationDate))
                        {
                            DateTime Visdate = MDVUtility.ToDateTime(model.AdministrationDate);
                            DateTime combined;



                            if (!string.IsNullOrEmpty(model.AdministrationTime))
                            {
                                var cSplit = model.AdministrationTime.Split(':');
                                var hours = "12";
                                if (cSplit.Length > 0)
                                {
                                    hours = cSplit[0];
                                }
                                var mints = "0";
                                var AmOrPm = "AM";
                                if (cSplit.Length > 1)
                                {
                                    var sSplit = cSplit[1].Split(' ');
                                    if (sSplit.Length > 0)
                                    {
                                        mints = sSplit[0];
                                    }
                                    if (sSplit.Length > 1)
                                    {
                                        AmOrPm = sSplit[1];
                                    }
                                }
                                if (AmOrPm == "AM")
                                {
                                    if (hours == "12")
                                    {
                                        string time = "0:" + mints + ":0";
                                        combined = Visdate.Add(TimeSpan.Parse(time));
                                    }
                                    else
                                    {
                                        string time = hours + ":" + mints + ":0";
                                        combined = Visdate.Add(TimeSpan.Parse(time));
                                    }
                                }
                                else
                                {
                                    hours = hours == "12" ? hours : (MDVUtility.ToInt(hours) + 12).ToString();
                                    string time = hours + ":" + mints + ":0";
                                    combined = Visdate.Add(TimeSpan.Parse(time));
                                }
                            }
                            else
                            {
                                combined = MDVUtility.ToDateTime(model.AdministrationDate);
                            }
                            dr.AdministrationDate = combined;
                        }
                        else
                            dr[dsImmunization.TherapeuticInjection.AdministrationDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.Dose))
                        {
                            dr.Dose = MDVUtility.ToDouble(model.Dose);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.DoseColumn] = DBNull.Value;
                        }
                        dr.Amount = model.Amount;

                        if (!string.IsNullOrEmpty(model.ManufacturerId))
                        {
                            dr.ManufacturerId = MDVUtility.ToInt64(model.ManufacturerId);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.ManufacturerIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.RouteId))
                        {
                            dr.RouteId = MDVUtility.ToInt64(model.RouteId);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.RouteIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.SiteId))
                        {
                            dr.SiteId = MDVUtility.ToInt64(model.SiteId);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.SiteIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.ExpiryDate))
                            dr.ExpiryDate = MDVUtility.ToDateTime(model.ExpiryDate);
                        else
                            dr[dsImmunization.TherapeuticInjection.ExpiryDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.VFC))
                        {
                            dr.VFC = MDVUtility.ToInt64(model.VFC);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.VFCColumn] = DBNull.Value;
                        }



                        if (!string.IsNullOrEmpty(model.ReactionId))
                        {
                            dr.ReactionId = MDVUtility.ToInt16(model.ReactionId);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.ReactionIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(model.Comments))
                        {
                            dr.Comments = model.Comments;
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.CommentsColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.SourceOfHx))
                        {
                            dr.SourceOfHx = MDVUtility.ToInt64(model.SourceOfHx);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.SourceOfHxColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.LotNumber))
                        {
                            dr.LotNumber = MDVUtility.ToInt64(model.LotNumber);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.LotNumberColumn] = DBNull.Value;
                        }

                        dr.Type = model.Type;


                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);


                        //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields
                        dr.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                        dr1 = dr;
                        //end newly added
                    }



                    #region Database Updation
                    if (dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count > 0)
                    {
                        BLObject<DSImmunization> objUpdate = BLLClinicalObj.UpdateTherapeuticInjection(dsImmunization);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                TherapeuticInjectionIdColumn = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows[0][dsImmunization.TherapeuticInjection.ImmTherInjectionIdColumn.ColumnName],
                                message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
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
                        message = "Therapeutic Injection not found."
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

        // Created By M Ahmad Imran
        // Created Date: 28/11/2016
        //OverView: To load Therapeutic Injection in Grid
        public string SearchImmunizationTherapeuticInjection(TherapeuticInjectionModel model)
        {
            try
            {

                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;

                obj = BLLClinicalObj.LoadImmunizationTherapeuticInjection(MDVUtility.ToInt64(model.ImmTherInjectionId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                dsImmunization = obj.Data;
                if (obj.Data != null)
                {

                    if (!string.IsNullOrEmpty(model.ImmTherInjectionId))
                    {
                        if (model.Type == "DocumentHx")
                        {
                            List<TherapeuticInjectionHistoryModel> TherapeuticInjectionList = new List<TherapeuticInjectionHistoryModel>();

                            if (dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count > 0)
                            {

                                foreach (DSImmunization.TherapeuticInjectionRow row in dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows)
                                {
                                    TherapeuticInjectionHistoryModel therapeuticInjectionModel = new TherapeuticInjectionHistoryModel();
                                    therapeuticInjectionModel.ImmTherInjectionId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.ImmTherInjectionIdColumn]);
                                    therapeuticInjectionModel.TherapeuticInjectionIdHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.TherapeuticInjectionIdColumn]);
                                    therapeuticInjectionModel.ProviderIdHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.ProviderIdColumn]);
                                    if (row[dsImmunization.TherapeuticInjection.AdministrationDateColumn] != DBNull.Value)
                                    {
                                        therapeuticInjectionModel.AdministrationDateHistory = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.TherapeuticInjection.AdministrationDateColumn]).ToShortDateString());
                                    }
                                    else
                                    {
                                        therapeuticInjectionModel.AdministrationDateHistory = "";
                                    }
                                    if (row[dsImmunization.TherapeuticInjection.AdministrationDateColumn] != DBNull.Value)
                                    {
                                        therapeuticInjectionModel.AdministrationTimeHistory = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.TherapeuticInjection.AdministrationDateColumn]).ToShortTimeString());
                                    }
                                    else
                                    {
                                        therapeuticInjectionModel.AdministrationDateHistory = "";
                                    }

                                    therapeuticInjectionModel.DoseHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.DoseColumn]);
                                    therapeuticInjectionModel.AmountHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.AmountColumn]);
                                    therapeuticInjectionModel.SiteIdHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.SiteIdColumn]);
                                    therapeuticInjectionModel.RouteIdHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.RouteIdColumn]);
                                    therapeuticInjectionModel.CommentsHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.CommentsColumn]);
                                    therapeuticInjectionModel.SourceOfHx = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.SourceOfHxColumn]);
                                    therapeuticInjectionModel.CPTCode = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.CPTCodeColumn]);
                                    therapeuticInjectionModel.TherapeuticInjection = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.TherapeuticInjectionColumn]);
                                    therapeuticInjectionModel.LinkedWithAnyNote = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.LinkedWithAnyNoteColumn]);
                                    therapeuticInjectionModel.GivenBy = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.GivenByColumn]);

                                    TherapeuticInjectionList.Add(therapeuticInjectionModel);
                                }
                                var response = new
                                {
                                    status = true,
                                    TherapeuticInjectionCount = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count,
                                    TherapeuticInjectionLoad_JSON = js.Serialize(TherapeuticInjectionList),
                                    iTotalDisplayRecords = (dsImmunization.TherapeuticInjection.Rows.Count > 0) ? dsImmunization.TherapeuticInjection.Rows[0][dsImmunization.TherapeuticInjection.RecordCountColumn.ColumnName] : 0,
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    TherapeuticInjectionCount = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count,
                                    TherapeuticInjectionLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName]),
                                    iTotalDisplayRecords = (dsImmunization.TherapeuticInjection.Rows.Count > 0) ? dsImmunization.TherapeuticInjection.Rows[0][dsImmunization.TherapeuticInjection.RecordCountColumn.ColumnName] : 0,
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }
                        else if (model.Type == "Administered")
                        {
                            List<TherapeuticInjectionModel> TherapeuticInjectionList = new List<TherapeuticInjectionModel>();

                            if (dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count > 0)
                            {

                                foreach (DSImmunization.TherapeuticInjectionRow row in dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows)
                                {
                                    TherapeuticInjectionModel therapeuticInjectionModel = new TherapeuticInjectionModel();
                                    therapeuticInjectionModel.ImmTherInjectionId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.ImmTherInjectionIdColumn]);
                                    therapeuticInjectionModel.TherapeuticInjectionId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.TherapeuticInjectionIdColumn]);
                                    therapeuticInjectionModel.VisitDate = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.TherapeuticInjection.VisitDateColumn]).ToShortDateString());
                                    therapeuticInjectionModel.ProviderId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.ProviderIdColumn]);
                                    therapeuticInjectionModel.AdministrationDate = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.TherapeuticInjection.AdministrationDateColumn]).ToShortDateString());
                                    therapeuticInjectionModel.AdministrationTime = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.TherapeuticInjection.AdministrationDateColumn]).ToShortTimeString());
                                    therapeuticInjectionModel.Dose = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.DoseColumn]);
                                    therapeuticInjectionModel.Amount = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.AmountColumn]);
                                    therapeuticInjectionModel.ManufacturerId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.ManufacturerIdColumn]);
                                    therapeuticInjectionModel.ManufacturerName = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.ManufacturerNameColumn]);
                                    therapeuticInjectionModel.SiteId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.SiteIdColumn]);
                                    therapeuticInjectionModel.RouteId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.RouteIdColumn]);
                                    therapeuticInjectionModel.Comments = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.CommentsColumn]);
                                    if (row[dsImmunization.TherapeuticInjection.ExpiryDateColumn] != DBNull.Value)
                                    {
                                        therapeuticInjectionModel.ExpiryDate = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.TherapeuticInjection.ExpiryDateColumn]).ToShortDateString());
                                    }
                                    else
                                    {
                                        therapeuticInjectionModel.ExpiryDate = "";
                                    }

                                    therapeuticInjectionModel.VFC = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.VFCColumn]);
                                    therapeuticInjectionModel.ReactionId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.ReactionIdColumn]);
                                    therapeuticInjectionModel.Type = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.TypeColumn]);
                                    therapeuticInjectionModel.CPTCode = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.CPTCodeColumn]);
                                    therapeuticInjectionModel.TherapeuticInjection = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.TherapeuticInjectionColumn]);
                                    therapeuticInjectionModel.LotNumber = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.LotNumberColumn]);
                                    therapeuticInjectionModel.LotText = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.LotTextColumn]);
                                    therapeuticInjectionModel.LinkedWithAnyNote = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.LinkedWithAnyNoteColumn]);
                                    therapeuticInjectionModel.GivenBy = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.GivenByColumn]);
                                    TherapeuticInjectionList.Add(therapeuticInjectionModel);
                                }
                                var response = new
                                {
                                    status = true,
                                    TherapeuticInjectionCount = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count,
                                    TherapeuticInjectionLoad_JSON = js.Serialize(TherapeuticInjectionList),
                                    iTotalDisplayRecords = (dsImmunization.TherapeuticInjection.Rows.Count > 0) ? dsImmunization.TherapeuticInjection.Rows[0][dsImmunization.TherapeuticInjection.RecordCountColumn.ColumnName] : 0,
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    TherapeuticInjectionCount = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count,
                                    TherapeuticInjectionLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName]),
                                    iTotalDisplayRecords = (dsImmunization.TherapeuticInjection.Rows.Count > 0) ? dsImmunization.TherapeuticInjection.Rows[0][dsImmunization.TherapeuticInjection.RecordCountColumn.ColumnName] : 0,
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                TherapeuticInjectionCount = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count,
                                TherapeuticInjectionLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName]),
                                iTotalDisplayRecords = (dsImmunization.TherapeuticInjection.Rows.Count > 0) ? dsImmunization.TherapeuticInjection.Rows[0][dsImmunization.TherapeuticInjection.RecordCountColumn.ColumnName] : 0,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            TherapeuticInjectionCount = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count,
                            TherapeuticInjectionLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName]),
                            iTotalDisplayRecords = (dsImmunization.TherapeuticInjection.Rows.Count > 0) ? dsImmunization.TherapeuticInjection.Rows[0][dsImmunization.TherapeuticInjection.RecordCountColumn.ColumnName] : 0,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }

                else
                {
                    var response = new
                    {
                        status = true,
                        TherapeuticInjectionCount = 0,
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
        #region Registery
        public string Search_Immunization_Registery(ImmunizationRegistery model)
        {
            try
            {

                DSImmunizationHL7 dsImmunizationHL7 = null;
                BLObject<DSImmunizationHL7> obj;
                bool active = true;
                if (!String.IsNullOrEmpty(model.IsActive))
                    active = (model.IsActive == "1") ? true : false;
                if (String.IsNullOrEmpty(model.PageNumber))
                    model.PageNumber = "1";
                if (String.IsNullOrEmpty(model.RowsPerPage))
                    model.RowsPerPage = "15";

                obj = BLLClinicalObj.LoadImunizationHL7Registery(model.RegistryConfigurationId, model.ProviderId, model.RegisteryId, active, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsImmunizationHL7 = obj.Data;

                if (dsImmunizationHL7.Tables[dsImmunizationHL7.RegistryConfiguration.TableName].Rows.Count > 0)
                {

                    var response = new
                    {
                        status = true,
                        RecordCount = dsImmunizationHL7.Tables[dsImmunizationHL7.RegistryConfiguration.TableName].Rows.Count,
                        RegisteryLoad_JSON = MDVUtility.JSON_DataTable(dsImmunizationHL7.Tables[dsImmunizationHL7.RegistryConfiguration.TableName]),
                        iTotalDisplayRecords = dsImmunizationHL7.Tables[dsImmunizationHL7.RegistryConfiguration.TableName].Rows[0][dsImmunizationHL7.RegistryConfiguration.RecordCountColumn.ColumnName],

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        RecordCount = 0,
                        iTotalDisplayRecords = 0,
                        RegisteryLoad_JSON = MDVUtility.JSON_DataTable(dsImmunizationHL7.Tables[dsImmunizationHL7.RegistryConfiguration.TableName]),
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

        public string SaveImmunizationRegistery(ImmunizationRegistery model)
        {
            try
            {
                ImmunizationRegisteryWrapperModel wrprmdl = new ImmunizationRegisteryWrapperModel();

                if (string.IsNullOrEmpty(model.RegistryConfigurationId) || model.RegistryConfigurationId == "0")
                    wrprmdl.RegistryConfigurationId = -1;
                else
                    wrprmdl.RegistryConfigurationId = MDVUtility.ToInt64(model.RegistryConfigurationId);

                wrprmdl.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                wrprmdl.SendingApplication = model.SendingApplication;
                wrprmdl.ReceivingApplicationId = MDVUtility.ToInt64(model.ReceivingApplicationId);
                wrprmdl.PoviderFacilityId = model.PoviderFacilityId;
                wrprmdl.SendingFacility = model.SendingFacility;
                wrprmdl.RegistrySubmissionId = MDVUtility.ToInt64(model.RegistrySubmissionId);
                wrprmdl.IsActive = MDVUtility.ToBool(model.IsActive);
                wrprmdl.IsDeleted = MDVUtility.ToBool(model.IsDeleted);
                wrprmdl.IsAdministered = MDVUtility.ToBool(model.IsAdministered);
                wrprmdl.IsRefusal = MDVUtility.ToBool(model.IsRefusal);
                wrprmdl.IsHistoryDose = MDVUtility.ToBool(model.IsHistoryDose);
                wrprmdl.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                wrprmdl.CreatedOn = DateTime.Now;
                wrprmdl.ModifiedOn = DateTime.Now;
                wrprmdl.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                wrprmdl.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                // Queue
                if (model.RegistrySubmissionId == "1")
                {
                    wrprmdl.Timeslot = model.Timeslot;
                    wrprmdl.FilesPerBatch = "";
                }
                else if (model.RegistrySubmissionId == "2") //batch
                {
                    wrprmdl.FilesPerBatch = model.FilesPerBatch;
                    wrprmdl.Timeslot = "";
                }
                if (model.Production)
                    wrprmdl.Status = "P";
                else
                    wrprmdl.Status = "T";
                BLObject<string> obj = BLLClinicalObj.InsertImunizationHL7RegisteryDetails(wrprmdl);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        RegisteryId = MDVUtility.ToStr(obj.Data)
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
        public string UpdateImmunizationRegistery(ImmunizationRegistery model)
        {
            try
            {
                ImmunizationRegisteryWrapperModel wrprmdl = new ImmunizationRegisteryWrapperModel();
                if (!string.IsNullOrEmpty(model.RegistryConfigurationId) || model.RegistryConfigurationId != "0")
                {
                    wrprmdl.RegistryConfigurationId = MDVUtility.ToInt64(model.RegistryConfigurationId);
                    wrprmdl.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                    wrprmdl.SendingApplication = model.SendingApplication;
                    wrprmdl.ReceivingApplicationId = MDVUtility.ToInt64(model.ReceivingApplicationId);
                    wrprmdl.PoviderFacilityId = model.PoviderFacilityId;
                    wrprmdl.SendingFacility = model.SendingFacility;
                    wrprmdl.RegistrySubmissionId = MDVUtility.ToInt64(model.RegistrySubmissionId);
                    wrprmdl.IsActive = MDVUtility.ToBool(model.IsActive);
                    wrprmdl.IsDeleted = MDVUtility.ToBool(model.IsDeleted);
                    wrprmdl.IsAdministered = MDVUtility.ToBool(model.IsAdministered);
                    wrprmdl.IsRefusal = MDVUtility.ToBool(model.IsRefusal);
                    wrprmdl.IsHistoryDose = MDVUtility.ToBool(model.IsHistoryDose);
                    wrprmdl.ModifiedOn = DateTime.Now;
                    wrprmdl.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                    wrprmdl.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    // Queue
                    if (model.RegistrySubmissionId == "1")
                    {
                        wrprmdl.Timeslot = model.Timeslot;
                        wrprmdl.FilesPerBatch = "";
                    }
                    else if (model.RegistrySubmissionId == "2") //batch
                    {
                        wrprmdl.FilesPerBatch = model.FilesPerBatch;
                        wrprmdl.Timeslot = "";
                    }
                    if (model.Production)
                        wrprmdl.Status = "P";
                    else
                        wrprmdl.Status = "T";
                    BLObject<string> obj = BLLClinicalObj.UpdateImunizationHL7RegisteryDetails(wrprmdl);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message,
                            RegisteryId = MDVUtility.ToStr(obj.Data)
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Update_Error_Message
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
        public string DeleteImmunizationRegistery(ImmunizationRegistery model)
        {
            try
            {
                ImmunizationRegisteryWrapperModel wrprmdl = new ImmunizationRegisteryWrapperModel();
                if (!string.IsNullOrEmpty(model.RegistryConfigurationId) || model.RegistryConfigurationId != "0")
                {
                    BLObject<string> obj = BLLClinicalObj.DeleteImunizationHL7RegisteryDetails(model.RegistryConfigurationId);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Delete_Message,
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Update_Error_Message
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
        public string GetCptCodeAndAdministeredCode(Int16 TherapeuticInjectionId)
        {
            try
            {
                if (MDVUtility.ToStr(TherapeuticInjectionId) != "" || MDVUtility.ToStr(TherapeuticInjectionId) != "0")
                {
                    DSImmunization dsImmunization = null;
                    BLObject<DSImmunization> obj;

                    obj = BLLClinicalObj.GetCptCodeAndAdministeredCode(TherapeuticInjectionId);
                    dsImmunization = obj.Data;


                    // int ImmunizationTotalCount = 0;
                    if (dsImmunization.Tables[dsImmunization.Cpts.TableName].Rows.Count > 0)
                    {

                        var response = new
                        {
                            status = true,
                            VaccineCount = dsImmunization.Tables[dsImmunization.Cpts.TableName].Rows.Count,
                            CptLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.Cpts.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VaccineCount = 0
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }



                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "No Give TherapeuticInjectionId"
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

        // Created By Abid Ali
        // Created Date: 11/04/2016
        //OverView: To load ParentChild Imunization in Grid
        public string loadParentChildImmunization(ImmunizationModel model)
        {
            try
            {
                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;

                obj = BLLClinicalObj.loadParentChildImmunization(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.EntityId), model.IsActive == "1" ? true : false, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), MDVUtility.ToInt64(model.NotesId));
                dsImmunization = obj.Data;

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, string>> listChilImunization = new List<Dictionary<string, string>>();

                if (obj.Data != null)
                {
                    // int ImmunizationTotalCount = 0;
                    List<ParentChildImunizationModel> immunizationList = new List<ParentChildImunizationModel>();
                    if (dsImmunization.Tables[dsImmunization.ParentVaccineHx.TableName].Rows.Count > 0)
                    {

                        foreach (DSImmunization.ParentVaccineHxRow row in dsImmunization.Tables[dsImmunization.ParentVaccineHx.TableName].Rows)
                        {
                            ParentChildImunizationModel immunizationViewModel = new ParentChildImunizationModel();
                            //Add Parent Records
                            immunizationViewModel.ParentImmunization.VaccineHxId = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.VaccineHxIdColumn]);
                            immunizationViewModel.ParentImmunization.VaccineId = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.vaccineColumn]);
                            immunizationViewModel.ParentImmunization.VaccineName = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.CVXShortDescriptionColumn]);
                            immunizationViewModel.ParentImmunization.Dose = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.DoseColumn]).Trim() == "ml"
                                                                           ? "" : MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.DoseColumn]);
                            immunizationViewModel.ParentImmunization.AdministrationDate = string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.AdministrationDateColumn])) ? "" : String.Format("{0:MM/dd/yyyy}", MDVUtility.ToDateTime(row[dsImmunization.ParentVaccineHx.AdministrationDateColumn]));
                            immunizationViewModel.ParentImmunization.GivenBy = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.GivenByColumn]);
                            immunizationViewModel.ParentImmunization.GivenByName = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.GivenByNameColumn]);
                            immunizationViewModel.ParentImmunization.Location = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.LocationColumn]);
                            immunizationViewModel.ParentImmunization.LotNumber = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.LotNumberColumn]);
                            immunizationViewModel.ParentImmunization.Type = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.TypeColumn]);
                            immunizationViewModel.ParentImmunization.IsNoteLinked = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.IsNoteLinkedColumn]);
                            immunizationViewModel.ParentImmunization.Category = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.CategoryColumn]);
                            immunizationViewModel.ParentImmunization.VaccineScheduleId = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.VaccineScheduleIdColumn]);
                            immunizationViewModel.ParentImmunization.ProviderName = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.ProviderNameColumn]);
                            immunizationViewModel.ParentImmunization.OrderSetId = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.OrderSetIdColumn]);
                            immunizationViewModel.ParentImmunization.AcknowledgementCode = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.AcknowledgementCodeColumn]);
                            immunizationViewModel.ParentImmunization.IsHistoryDose = MDVUtility.ToBool(row[dsImmunization.ParentVaccineHx.IsHistoryDoseColumn]);
                            immunizationViewModel.ParentImmunization.CategoryName = MDVUtility.ToStr(row[dsImmunization.ParentVaccineHx.CategoryNameColumn]);
                            if (dsImmunization.ChildVaccineHx.Rows.Count > 0)
                            {
                                DSImmunization.ChildVaccineHxRow[] childImunizationRows = (DSImmunization.ChildVaccineHxRow[])dsImmunization.ChildVaccineHx.Select(dsImmunization.ChildVaccineHx.vaccineColumn.ColumnName + " = " + row.vaccine);
                                // Add child Records
                                foreach (var dr in childImunizationRows)
                                {
                                    var childImunizationRow = new Immunization
                                    {
                                        VaccineId = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.vaccineColumn]),
                                        VaccineName = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.CVXShortDescriptionColumn]),
                                        Dose = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.DoseColumn]),
                                        AdministrationDate = string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.AdministrationDateColumn])) ? "" : String.Format("{0:MM/dd/yyyy}", MDVUtility.ToDateTime(dr[dsImmunization.ChildVaccineHx.AdministrationDateColumn])),
                                        GivenBy = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.GivenByColumn]),
                                        GivenByName = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.GivenByNameColumn]),
                                        Location = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.LocationColumn]),
                                        LotNumber = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.LotNumberColumn]),
                                        Type = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.TypeColumn]),
                                        IsNoteLinked = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.IsNoteLinkedColumn]),
                                        VaccineHxId = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.VaccineHxIdColumn]),
                                        Category = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.CategoryColumn]),
                                        VaccineScheduleId = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.VaccineScheduleIdColumn]),
                                        ProviderName = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.ProviderNameColumn]),
                                        OrderSetId = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.OrderSetIdColumn]),
                                        AcknowledgementCode = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.AcknowledgementCodeColumn]),
                                        IsHistoryDose = MDVUtility.ToBool(dr[dsImmunization.ChildVaccineHx.IsHistoryDoseColumn]),
                                        CategoryName = MDVUtility.ToStr(dr[dsImmunization.ChildVaccineHx.CategoryNameColumn]),
                                    };
                                    immunizationViewModel.ChildImmunizationList.Add(childImunizationRow);
                                }
                            }
                            immunizationList.Add(immunizationViewModel);
                        }
                        var response = new
                        {
                            status = true,
                            ImmunizationParentChildLoad_JSON = js.Serialize(immunizationList),
                            iTotalDisplayRecords = MDVUtility.ToStr(dsImmunization.ParentVaccineHx.Rows[0][dsImmunization.ParentVaccineHx.RecordCountColumn.ColumnName]),
                            ParentImmunizationCount = dsImmunization.Tables[dsImmunization.ParentVaccineHx.TableName].Rows.Count,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ImmunizationParentChildLoad_JSON = js.Serialize(immunizationList),
                            ParentImmunizationCount = 0
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ImmunizationCount = 0,
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


        // Created By Abid Ali
        // Created Date: 11/04/2016
        //OverView: To update VaccineHx
        //public string updateAdministerVaccine(ImmunizationModel model)
        //{
        //    try
        //    {
        //        DSImmunization dsImmunization = new DSImmunization();
        //        DSImmunization.VaccineHxRow dr = null;

        //        var obj = BLLClinicalObj.LoadVaccineHx(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.VaccineHxId));

        //        dsImmunization = obj.Data;
        //        if (obj.Data != null)
        //        {

        //            if (dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows.Count > 0)
        //            {
        //                dr = (DSImmunization.VaccineHxRow)dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows[0];

        //                dr.VaccineGroupCategory = MDVUtility.ToInt64(model.CategoryID);
        //                if (!string.IsNullOrEmpty(model.VaccineScheduleId))
        //                {
        //                    dr.VaccineScheduleId = MDVUtility.ToInt64(model.VaccineScheduleId);
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.VaccineScheduleIdColumn] = DBNull.Value;
        //                }

        //                if (!string.IsNullOrEmpty(model.OrdersetId))
        //                {
        //                    dr.OrderSetId = MDVUtility.ToInt64(model.OrdersetId);
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.OrderSetIdColumn] = DBNull.Value;
        //                }

        //                dr.PatientAge = model.PatientAge;
        //                if (!string.IsNullOrEmpty(model.VisitDate) && model.VisitDate != "- Select -")
        //                    dr.VisitDate = MDVUtility.ToDateTime(model.VisitDate);
        //                else
        //                    dr[dsImmunization.VaccineHx.VisitDateColumn] = DBNull.Value;
        //                if (!string.IsNullOrEmpty(model.VisitDateId))
        //                    dr.VisitDateId = MDVUtility.ToInt64(model.VisitDateId);
        //                else
        //                    dr[dsImmunization.VaccineHx.VisitDateIdColumn] = DBNull.Value;
        //                if (!string.IsNullOrEmpty(model.ProviderId))
        //                {
        //                    dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.ProviderIdColumn] = DBNull.Value;
        //                }
        //                dr.Vaccine = MDVUtility.ToInt64(model.VaccineID);
        //                if (!string.IsNullOrEmpty(model.AdministrationDate))
        //                {
        //                    DateTime Visdate = MDVUtility.ToDateTime(model.AdministrationDate);
        //                    DateTime combined;



        //                    if (!string.IsNullOrEmpty(model.Time))
        //                    {
        //                        var cSplit = model.Time.Split(':');
        //                        var hours = "12";
        //                        if (cSplit.Length > 0)
        //                        {
        //                            hours = cSplit[0];
        //                        }
        //                        var mints = "0";
        //                        var AmOrPm = "AM";
        //                        if (cSplit.Length > 1)
        //                        {
        //                            var sSplit = cSplit[1].Split(' ');
        //                            if (sSplit.Length > 0)
        //                            {
        //                                mints = sSplit[0];
        //                            }
        //                            if (sSplit.Length > 1)
        //                            {
        //                                AmOrPm = sSplit[1];
        //                            }
        //                        }
        //                        if (AmOrPm == "AM")
        //                        {
        //                            if (hours == "12")
        //                            {
        //                                string time = "0:" + mints + ":0";
        //                                combined = Visdate.Add(TimeSpan.Parse(time));
        //                            }
        //                            else
        //                            {
        //                                string time = hours + ":" + mints + ":0";
        //                                combined = Visdate.Add(TimeSpan.Parse(time));
        //                            }
        //                        }
        //                        else
        //                        {
        //                            hours = hours == "12" ? hours : (MDVUtility.ToInt(hours) + 12).ToString();
        //                            string time = hours + ":" + mints + ":0";
        //                            combined = Visdate.Add(TimeSpan.Parse(time));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        combined = MDVUtility.ToDateTime(model.AdministrationDate);
        //                    }
        //                    dr.AdministrationDate = combined;
        //                }
        //                else
        //                    dr[dsImmunization.VaccineHx.AdministrationDateColumn] = DBNull.Value;

        //                if (!string.IsNullOrEmpty(model.Dose))
        //                {
        //                    dr.Dose = MDVUtility.ToDecimal(model.Dose);
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.DoseColumn] = DBNull.Value;
        //                }
        //                dr.Amount = model.Amount;
        //                if (!string.IsNullOrEmpty(model.LotNo))
        //                {
        //                    dr.LotNumber = MDVUtility.ToLong(model.LotNo);
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.LotNumberColumn] = DBNull.Value;
        //                }
        //                if (!string.IsNullOrEmpty(model.ManufacturerId))
        //                {
        //                    dr.Manufacturer = MDVUtility.ToInt64(model.ManufacturerId);
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.ManufacturerColumn] = DBNull.Value;
        //                }
        //                if (!string.IsNullOrEmpty(model.RouteId))
        //                {
        //                    dr.Route = MDVUtility.ToInt64(model.RouteId);
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.RouteColumn] = DBNull.Value;
        //                }
        //                if (!string.IsNullOrEmpty(model.SourceOfHxId))
        //                {
        //                    dr.SourceOfHx = MDVUtility.ToInt64(model.SourceOfHxId);
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.SourceOfHxColumn] = DBNull.Value;
        //                }
        //                if (!string.IsNullOrEmpty(model.SiteId))
        //                {
        //                    dr.Site = MDVUtility.ToInt64(model.SiteId);
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.SiteColumn] = DBNull.Value;
        //                }
        //                if (!string.IsNullOrEmpty(model.ExpiryDate))
        //                    dr.ExpiryDate = MDVUtility.ToDateTime(model.ExpiryDate);
        //                else
        //                    dr[dsImmunization.VaccineHx.ExpiryDateColumn] = DBNull.Value;

        //                if (!string.IsNullOrEmpty(model.VfcId))
        //                {
        //                    dr.VFC = MDVUtility.ToInt64(model.VfcId);
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.VFCColumn] = DBNull.Value;
        //                }


        //                if (MDVUtility.ToInt64(model.VisDateId) != 0)
        //                {
        //                    dr.VISDate = MDVUtility.ToInt64(model.VisDateId);
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.VISDateColumn] = DBNull.Value;
        //                }
        //                if (!string.IsNullOrEmpty(model.Reaction))
        //                {
        //                    dr.Reaction = MDVUtility.ToInt64(model.Reaction);
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.ReactionColumn] = DBNull.Value;
        //                }
        //                dr.VoidDose = model.VoidDose;

        //                if (!string.IsNullOrEmpty(model.Comments))
        //                {
        //                    dr.Comments = model.Comments;
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.CommentsColumn] = DBNull.Value;
        //                }

        //                if (!string.IsNullOrEmpty(model.PublicityCode))
        //                {
        //                    dr.PublicityCode = model.PublicityCode;
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.PublicityCodeColumn] = DBNull.Value;
        //                }

        //                if (!string.IsNullOrEmpty(model.PublicityCodeExpiryDate))
        //                    dr.PublicityCodeExpiryDate = MDVUtility.ToDateTime(model.PublicityCodeExpiryDate);
        //                else
        //                    dr[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn] = DBNull.Value;

        //                if (!string.IsNullOrEmpty(model.ImmunizationRegistryStatusCode))
        //                {
        //                    dr.ImmunizationRegistryStatusCode = model.ImmunizationRegistryStatusCode;
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.ImmunizationRegistryStatusCodeColumn] = DBNull.Value;
        //                }

        //                if (!string.IsNullOrEmpty(model.IRSEffectiveDate))
        //                    dr.IRSEffectiveDate = MDVUtility.ToDateTime(model.IRSEffectiveDate);
        //                else
        //                    dr[dsImmunization.VaccineHx.IRSEffectiveDateColumn] = DBNull.Value;

        //                if (!string.IsNullOrEmpty(model.Type) && model.Type == "ADMINISTER")
        //                {
        //                    dr.OverrideRule = model.OverrideRule;
        //                }
        //                else
        //                {
        //                    dr[dsImmunization.VaccineHx.OverrideRuleColumn] = DBNull.Value;
        //                }


        //                dr.ProtectionIndicator = model.ProtectionIndicator;
        //                if (!string.IsNullOrEmpty(model.PIEffectiveDate))
        //                    dr.PIEffectiveDate = MDVUtility.ToDateTime(model.PIEffectiveDate);
        //                else
        //                    dr[dsImmunization.VaccineHx.PIEffectiveDateColumn] = DBNull.Value;
        //                if (!string.IsNullOrEmpty(model.RefusalReasonID))
        //                    dr.RefusalReasonId = MDVUtility.ToLong(model.RefusalReasonID);
        //                else
        //                    dr[dsImmunization.VaccineHx.RefusalReasonIdColumn] = DBNull.Value;


        //                dr.GivenBy = MDVSession.Current.AppUserId;
        //                dr.Type = (model.Type);
        //                dr.PatientId = MDVUtility.ToInt64(model.PatientId);
        //                dr.ISActive = MDVUtility.ToByte(model.IsActive);

        //                //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields
        //                dr.ModifiedOn = DateTime.Now;
        //                dr.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

        //            }

        //            obj = BLLClinicalObj.UpdateVaccineHx(dsImmunization);
        //            dsImmunization = obj.Data;

        //            if (obj.Data != null)
        //            {
        //                var response = new
        //                {
        //                    status = true,
        //                    message = Common.AppPrivileges.Update_Message,
        //                    VaccineHxIdColumn = dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows[0][dsImmunization.VaccineHx.VaccineHxIdColumn.ColumnName]
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    message = obj.Message,
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                message = obj.Message,
        //            };
        //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //        }

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

        public string AdministerVaccineUpdate(ImmunizationModel model)
        {
            try
            {
                string privilegasMessage = string.Empty;
                if (!string.IsNullOrWhiteSpace(model.DeleteProcedureIds))
                {
                    privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "DELETE")).ToString();
                }
                if (string.IsNullOrWhiteSpace(privilegasMessage))
                {
                    DSImmunization dsImmunization = new DSImmunization();
                    DSImmunization.VaccineHxRow dr = null;

                    var obj = BLLClinicalObj.LoadVaccineHx(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.VaccineHxId));

                    dsImmunization = obj.Data;
                    if (obj.Data != null)
                    {

                        if (dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows.Count > 0)
                        {
                            dr = (DSImmunization.VaccineHxRow)dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows[0];

                            dr.VaccineGroupCategory = MDVUtility.ToInt64(model.CategoryID);
                            if (!string.IsNullOrEmpty(model.VaccineScheduleId))
                            {
                                dr.VaccineScheduleId = MDVUtility.ToInt64(model.VaccineScheduleId);
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.VaccineScheduleIdColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(model.OrdersetId))
                            {
                                dr.OrderSetId = MDVUtility.ToInt64(model.OrdersetId);
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.OrderSetIdColumn] = DBNull.Value;
                            }

                            dr.PatientAge = model.PatientAge;
                            if (!string.IsNullOrEmpty(model.VisitDate) && model.VisitDate != "- Select -")
                                dr.VisitDate = MDVUtility.ToDateTime(model.VisitDate);
                            else
                                dr[dsImmunization.VaccineHx.VisitDateColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(model.VisitDateId))
                                dr.VisitDateId = MDVUtility.ToInt64(model.VisitDateId);
                            else
                                dr[dsImmunization.VaccineHx.VisitDateIdColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(model.ProviderId))
                            {
                                dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.ProviderIdColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(model.FacilityId))
                                dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
                            else
                                dr[dsImmunization.VaccineHx.FacilityIdColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(model.UserId))
                                dr.UserId = MDVUtility.ToInt64(model.UserId);
                            else
                                dr[dsImmunization.VaccineHx.UserIdColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(model.RegisteryId))
                                dr.ReceivingApplicationId = MDVUtility.ToInt64(model.RegisteryId);
                            else
                                dr[dsImmunization.VaccineHx.ReceivingApplicationIdColumn] = DBNull.Value;
                            dr.Vaccine = MDVUtility.ToInt64(model.VaccineID);
                            if (!string.IsNullOrEmpty(model.AdministrationDate))
                            {
                                DateTime Visdate = MDVUtility.ToDateTime(model.AdministrationDate);
                                DateTime combined;
                                if (!string.IsNullOrEmpty(model.Time))
                                {
                                    var cSplit = model.Time.Split(':');
                                    var hours = "12";
                                    if (cSplit.Length > 0)
                                    {
                                        hours = cSplit[0];
                                    }
                                    var mints = "0";
                                    var AmOrPm = "AM";
                                    if (cSplit.Length > 1)
                                    {
                                        var sSplit = cSplit[1].Split(' ');
                                        if (sSplit.Length > 0)
                                        {
                                            mints = sSplit[0];
                                        }
                                        if (sSplit.Length > 1)
                                        {
                                            AmOrPm = sSplit[1];
                                        }
                                    }
                                    if (AmOrPm == "AM")
                                    {
                                        if (hours == "12")
                                        {
                                            string time = "0:" + mints + ":0";
                                            combined = Visdate.Add(TimeSpan.Parse(time));
                                        }
                                        else
                                        {
                                            string time = hours + ":" + mints + ":0";
                                            combined = Visdate.Add(TimeSpan.Parse(time));
                                        }
                                    }
                                    else
                                    {
                                        hours = hours == "12" ? hours : (MDVUtility.ToInt(hours) + 12).ToString();
                                        string time = hours + ":" + mints + ":0";
                                        combined = Visdate.Add(TimeSpan.Parse(time));
                                    }
                                }
                                else
                                {
                                    combined = MDVUtility.ToDateTime(model.AdministrationDate);
                                }
                                dr.AdministrationDate = combined;
                            }
                            else
                                dr[dsImmunization.VaccineHx.AdministrationDateColumn] = DBNull.Value;

                            if (!string.IsNullOrEmpty(model.Dose))
                            {
                                dr.Dose = MDVUtility.ToDecimal(model.Dose);
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.DoseColumn] = DBNull.Value;
                            }
                            dr.Amount = model.Amount;
                            if (!string.IsNullOrEmpty(model.LotNo))
                            {
                                dr.LotNumber = MDVUtility.ToLong(model.LotNo);
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.LotNumberColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(model.ManufacturerId))
                            {
                                dr.Manufacturer = MDVUtility.ToInt64(model.ManufacturerId);
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.ManufacturerColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(model.RouteId))
                            {
                                dr.Route = MDVUtility.ToInt64(model.RouteId);
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.RouteColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(model.SourceOfHxId))
                            {
                                dr.SourceOfHx = MDVUtility.ToInt64(model.SourceOfHxId);
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.SourceOfHxColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(model.SiteId))
                            {
                                dr.Site = MDVUtility.ToInt64(model.SiteId);
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.SiteColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(model.ExpiryDate))
                                dr.ExpiryDate = MDVUtility.ToDateTime(model.ExpiryDate);
                            else
                                dr[dsImmunization.VaccineHx.ExpiryDateColumn] = DBNull.Value;

                            if (!string.IsNullOrEmpty(model.VfcId))
                            {
                                dr.VFC = MDVUtility.ToInt64(model.VfcId);
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.VFCColumn] = DBNull.Value;
                            }


                            if (MDVUtility.ToInt64(model.VisDateId) != 0)
                            {
                                dr.VISDate = MDVUtility.ToInt64(model.VisDateId);
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.VISDateColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(model.Reaction))
                            {
                                dr.Reaction = MDVUtility.ToInt64(model.Reaction);
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.ReactionColumn] = DBNull.Value;
                            }
                            dr.VoidDose = model.VoidDose;

                            if (!string.IsNullOrEmpty(model.Comments))
                            {
                                dr.Comments = model.Comments;
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.CommentsColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(model.PublicityCode))
                            {
                                dr.PublicityCode = model.PublicityCode;
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.PublicityCodeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(model.PublicityCodeExpiryDate))
                                dr.PublicityCodeExpiryDate = MDVUtility.ToDateTime(model.PublicityCodeExpiryDate);
                            else
                                dr[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn] = DBNull.Value;

                            if (!string.IsNullOrEmpty(model.ImmunizationRegistryStatusCode))
                            {
                                dr.ImmunizationRegistryStatusCode = model.ImmunizationRegistryStatusCode;
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.ImmunizationRegistryStatusCodeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(model.IRSEffectiveDate))
                                dr.IRSEffectiveDate = MDVUtility.ToDateTime(model.IRSEffectiveDate);
                            else
                                dr[dsImmunization.VaccineHx.IRSEffectiveDateColumn] = DBNull.Value;

                            if (!string.IsNullOrEmpty(model.Type) && model.Type == "ADMINISTER")
                            {
                                dr.OverrideRule = model.OverrideRule;
                            }
                            else
                            {
                                dr[dsImmunization.VaccineHx.OverrideRuleColumn] = DBNull.Value;
                            }


                            dr.ProtectionIndicator = model.ProtectionIndicator;
                            if (!string.IsNullOrEmpty(model.PIEffectiveDate))
                                dr.PIEffectiveDate = MDVUtility.ToDateTime(model.PIEffectiveDate);
                            else
                                dr[dsImmunization.VaccineHx.PIEffectiveDateColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(model.RefusalReasonID))
                                dr.RefusalReasonId = MDVUtility.ToLong(model.RefusalReasonID);
                            else
                                dr[dsImmunization.VaccineHx.RefusalReasonIdColumn] = DBNull.Value;


                            dr.GivenBy = MDVSession.Current.AppUserId;
                            dr.Type = (model.Type);
                            dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                            dr.ISActive = MDVUtility.ToByte(model.IsActive);

                            //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields
                            dr.ModifiedOn = DateTime.Now;
                            dr.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

                        }
                        privilegasMessage = string.Empty;
                        bool IsAddProcedures = false;
                        if (model.Type.ToLower() == "administer" && model.PreviousVoid && !model.VoidDose)
                        {
                            privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Medical_Procedures", "ADD")).ToString();
                            if (string.IsNullOrWhiteSpace(privilegasMessage))
                            {
                                IsAddProcedures = true;
                            }
                        }
                        obj = BLLClinicalObj.VaccineHxUpdate(dsImmunization, model.DeleteProcedureIds, model.PreviousVoid, IsAddProcedures, model.NotesId);
                        dsImmunization = obj.Data;

                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = string.IsNullOrWhiteSpace(obj.Message) ? Common.AppPrivileges.Update_Message : obj.Message,
                                VaccineHxIdColumn = dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows[0][dsImmunization.VaccineHx.VaccineHxIdColumn.ColumnName]
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = obj.Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = obj.Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
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

        // Created By Abid Ali
        // Created Date: 11/04/2016
        //OverView: To search VaccineHx
        public string loadAdministerVaccine(ImmunizationModel model)
        {

            try
            {
                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;

                //Start || 22 April, 2016 || ZeeshanAK || Changes for audit
                obj = BLLClinicalObj.LoadVaccineHx(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.VaccineHxId), "1", "");
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                dsImmunization = obj.Data;
                if (obj.Data != null)
                {
                    List<ImmunizationAdministerVaccineHx> viewModelAdmin = new List<ImmunizationAdministerVaccineHx>();
                    List<ImmunizationDocumentHxDoseHx> viewModelDoc = new List<ImmunizationDocumentHxDoseHx>();
                    List<ImmunizationRefusalRecord> viewModelRef = new List<ImmunizationRefusalRecord>();

                    if (dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows.Count > 0)
                    {
                        foreach (DSImmunization.VaccineHxRow row in dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows)
                        {
                            if (row[dsImmunization.VaccineHx.TypeColumn].ToString().Trim().ToLower() == "administer")
                            {
                                ImmunizationAdministerVaccineHx immunizationViccineHxModel = new ImmunizationAdministerVaccineHx();

                                immunizationViccineHxModel.VaccineHxId = MDVUtility.ToStr(row[dsImmunization.VaccineHx.VaccineHxIdColumn]);
                                immunizationViccineHxModel.AdministerVaccine_Category = MDVUtility.ToStr(row[dsImmunization.VaccineHx.VaccineGroupCategoryColumn]);
                                //immunizationViccineHxModel.AdministerVaccine_VisitDate = string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.VisitDateColumn])) ? "" : MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.VisitDateColumn]).ToString());
                                immunizationViccineHxModel.AdministerVaccine_Provider = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ProviderIdColumn]);
                                immunizationViccineHxModel.AdministerVaccine_Vaccine = MDVUtility.ToStr(row[dsImmunization.VaccineHx.VaccineColumn]);
                                immunizationViccineHxModel.AdministerVaccine_AdministrationDate = string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.AdministrationDateColumn])) ? "" : MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.AdministrationDateColumn]).ToShortDateString());
                                DateTime AdministerTime = MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.AdministrationDateColumn]);
                                immunizationViccineHxModel.AdministerVaccine_AdministrationTime = ((MDVUtility.ToStr(AdministerTime).IndexOf("AM") > 0 ? (AdministerTime.Hour == 0 ? 12 : AdministerTime.Hour) : (AdministerTime.Hour == 12 ? AdministerTime.Hour : ((AdministerTime.Hour) - 12))) + ":" + ((AdministerTime.Minute.ToString().Length) > 1 ? AdministerTime.Minute.ToString() : '0' + AdministerTime.Minute.ToString()) + " " + (MDVUtility.ToStr(AdministerTime).IndexOf("PM") > 0 ? "PM" : "AM"));
                                immunizationViccineHxModel.AdministerVaccine_Dose = MDVUtility.ToStr(row[dsImmunization.VaccineHx.DoseColumn]);
                                immunizationViccineHxModel.AdministerVaccine_Amount = MDVUtility.ToStr(row[dsImmunization.VaccineHx.AmountColumn]);
                                immunizationViccineHxModel.AdministerVaccine_LotNumber = MDVUtility.ToStr(row[dsImmunization.VaccineHx.LotNumberColumn]);
                                immunizationViccineHxModel.AdministerVaccine_Manufacturer = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ManufacturerColumn]);
                                immunizationViccineHxModel.AdministerTabManufacturer = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ManufacturerNameColumn]);

                                immunizationViccineHxModel.AdministerVaccine_Route = MDVUtility.ToStr(row[dsImmunization.VaccineHx.RouteColumn]);
                                immunizationViccineHxModel.AdministerVaccine_Site = MDVUtility.ToStr(row[dsImmunization.VaccineHx.SiteColumn]);
                                immunizationViccineHxModel.AdministerVaccine_ExpiryDate = string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.ExpiryDateColumn])) ? "" : MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.ExpiryDateColumn]).ToShortDateString());
                                immunizationViccineHxModel.AdministerVaccine_VFC = MDVUtility.ToStr(row[dsImmunization.VaccineHx.VFCColumn]);
                                immunizationViccineHxModel.AdministerVaccine_VISDate = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.VIS_EditionDateColumn]).ToShortDateString());
                                immunizationViccineHxModel.AdministerReaction = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ReactionColumn]);
                                if (row[dsImmunization.VaccineHx.VoidDoseColumn] != DBNull.Value)
                                {
                                    immunizationViccineHxModel.AdministerVoidDose = Convert.ToBoolean(row[dsImmunization.VaccineHx.VoidDoseColumn]) == true ? "1" : "0";
                                }
                                else
                                {
                                    immunizationViccineHxModel.AdministerVoidDose = "0";
                                }
                                immunizationViccineHxModel.AdministerVaccine_Comments = MDVUtility.ToStr(row[dsImmunization.VaccineHx.CommentsColumn]);
                                immunizationViccineHxModel.AdministerVaccine_PublicityCode = MDVUtility.ToStr(row[dsImmunization.VaccineHx.PublicityCodeColumn]);
                                immunizationViccineHxModel.AdministerVaccine_PublicityExpiryDate = MDVUtility.ToStr(MDVUtility.ToStr(row[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn]) != "" ? MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn]).ToShortDateString() : row[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn]);
                                immunizationViccineHxModel.AdministerVaccine_IRS = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ImmunizationRegistryStatusCodeColumn]);
                                immunizationViccineHxModel.AdministerVaccine_IRSEffectiveDate = MDVUtility.ToStr(MDVUtility.ToStr(row[dsImmunization.VaccineHx.IRSEffectiveDateColumn]) != "" ? (MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.IRSEffectiveDateColumn]).ToShortDateString()) : row[dsImmunization.VaccineHx.IRSEffectiveDateColumn]);
                                immunizationViccineHxModel.AdministerVaccine_ProtectionIndicator = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ProtectionIndicatorColumn]);
                                immunizationViccineHxModel.AdministerVaccine_PIEffectiveDate = MDVUtility.ToStr(MDVUtility.ToStr(row[dsImmunization.VaccineHx.PIEffectiveDateColumn]) != "" ? MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.PIEffectiveDateColumn]).ToShortDateString() : row[dsImmunization.VaccineHx.PIEffectiveDateColumn]);
                                immunizationViccineHxModel.AdministerVaccine_VisitDate = MDVUtility.ToStr(row[dsImmunization.VaccineHx.VisitDateIdColumn]);
                                immunizationViccineHxModel.AdministerVaccine_IsActive = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ISActiveColumn]);
                                immunizationViccineHxModel.LotText = MDVUtility.ToStr(row[dsImmunization.VaccineHx.LotTextColumn]);
                                immunizationViccineHxModel.AdministerOverrideRule = MDVUtility.ToStr(row[dsImmunization.VaccineHx.OverrideRuleColumn]);
                                immunizationViccineHxModel.OrderSetId = MDVUtility.ToStr(row[dsImmunization.VaccineHx.OrderSetIdColumn]);
                                immunizationViccineHxModel.FacilityId = !string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.FacilityIdColumn])) ? MDVUtility.ToStr(row[dsImmunization.VaccineHx.FacilityIdColumn]) : "";
                                immunizationViccineHxModel.Facility = !string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.FacilityNameColumn])) ? MDVUtility.ToStr(row[dsImmunization.VaccineHx.FacilityNameColumn]) : "";
                                immunizationViccineHxModel.AdministerEnteredBy = !string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.UserIdColumn])) ? MDVUtility.ToStr(row[dsImmunization.VaccineHx.UserIdColumn]) : "";
                                immunizationViccineHxModel.AdministerRegistery = !string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.ReceivingApplicationIdColumn])) ? MDVUtility.ToStr(row[dsImmunization.VaccineHx.ReceivingApplicationIdColumn]) : "";
                                viewModelAdmin.Add(immunizationViccineHxModel);
                            }
                            else if (row[dsImmunization.VaccineHx.TypeColumn].ToString().Trim().ToLower() == "documenthx")
                            {
                                ImmunizationDocumentHxDoseHx immunizationViccineHxModel = new ImmunizationDocumentHxDoseHx();

                                immunizationViccineHxModel.VaccineHxId = MDVUtility.ToStr(row[dsImmunization.VaccineHx.VaccineHxIdColumn]);

                                immunizationViccineHxModel.DocumentHxDose_SourceOfHx = MDVUtility.ToStr(row[dsImmunization.VaccineHx.SourceOfHxColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Category = MDVUtility.ToStr(row[dsImmunization.VaccineHx.VaccineGroupCategoryColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Vaccine = MDVUtility.ToStr(row[dsImmunization.VaccineHx.VaccineColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Provider = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ProviderIdColumn]);
                                immunizationViccineHxModel.DocumentHxDose_AdministrationDate = string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.AdministrationDateColumn])) ? "" : MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.AdministrationDateColumn]).ToShortDateString());
                                DateTime AdministerTime = MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.AdministrationDateColumn]);
                                immunizationViccineHxModel.DocumentHxDose_AdministrationTime = ((MDVUtility.ToStr(AdministerTime).IndexOf("AM") > 0 ? (AdministerTime.Hour == 0 ? 12 : AdministerTime.Hour) : (AdministerTime.Hour == 12 ? AdministerTime.Hour : ((AdministerTime.Hour) - 12))) + ":" + ((AdministerTime.Minute.ToString().Length) > 1 ? AdministerTime.Minute.ToString() : '0' + (AdministerTime.Minute.ToString())) + " " + (MDVUtility.ToStr(AdministerTime).IndexOf("PM") > 0 ? "PM" : "AM"));
                                immunizationViccineHxModel.DocumentHxDose_Dose = MDVUtility.ToStr(row[dsImmunization.VaccineHx.DoseColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Amount = MDVUtility.ToStr(row[dsImmunization.VaccineHx.AmountColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Site = MDVUtility.ToStr(row[dsImmunization.VaccineHx.SiteColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Route = MDVUtility.ToStr(row[dsImmunization.VaccineHx.RouteColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Comments = MDVUtility.ToStr(row[dsImmunization.VaccineHx.CommentsColumn]);
                                if (row[dsImmunization.VaccineHx.VoidDoseColumn] != DBNull.Value)
                                {
                                    immunizationViccineHxModel.DocumentHxVoidDose = Convert.ToBoolean(row[dsImmunization.VaccineHx.VoidDoseColumn]) == true ? "1" : "0";

                                }
                                else
                                {
                                    immunizationViccineHxModel.DocumentHxVoidDose = "0";
                                }
                                immunizationViccineHxModel.DocumentHxDose_IsActive = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ISActiveColumn]);
                                immunizationViccineHxModel.OrderSetId = MDVUtility.ToStr(row[dsImmunization.VaccineHx.OrderSetIdColumn]);
                                immunizationViccineHxModel.DocumentVaccine_PublicityCode = MDVUtility.ToStr(row[dsImmunization.VaccineHx.PublicityCodeColumn]);
                                immunizationViccineHxModel.DocumentVaccine_PublicityExpiryDate = MDVUtility.ToStr(MDVUtility.ToStr(row[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn]) != "" ? MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn]).ToShortDateString() : row[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn]);
                                immunizationViccineHxModel.DocumentVaccine_IRS = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ImmunizationRegistryStatusCodeColumn]);
                                immunizationViccineHxModel.DocumentVaccine_IRSEffectiveDate = MDVUtility.ToStr(MDVUtility.ToStr(row[dsImmunization.VaccineHx.IRSEffectiveDateColumn]) != "" ? (MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.IRSEffectiveDateColumn]).ToShortDateString()) : row[dsImmunization.VaccineHx.IRSEffectiveDateColumn]);
                                immunizationViccineHxModel.DocumentVaccine_ProtectionIndicator = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ProtectionIndicatorColumn]);
                                immunizationViccineHxModel.DocumentVaccine_PIEffectiveDate = MDVUtility.ToStr(MDVUtility.ToStr(row[dsImmunization.VaccineHx.PIEffectiveDateColumn]) != "" ? MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.PIEffectiveDateColumn]).ToShortDateString() : row[dsImmunization.VaccineHx.PIEffectiveDateColumn]);
                                viewModelDoc.Add(immunizationViccineHxModel);
                            }
                            else if (row[dsImmunization.VaccineHx.TypeColumn].ToString().Trim().ToLower() == "refusal")
                            {
                                ImmunizationRefusalRecord immunizationViccineHxModel = new ImmunizationRefusalRecord();

                                immunizationViccineHxModel.VaccineHxId = MDVUtility.ToStr(row[dsImmunization.VaccineHx.VaccineHxIdColumn]);

                                immunizationViccineHxModel.RecordRefusal_Category = MDVUtility.ToStr(row[dsImmunization.VaccineHx.VaccineGroupCategoryColumn]);
                                immunizationViccineHxModel.RecordRefusal_Vaccine = MDVUtility.ToStr(row[dsImmunization.VaccineHx.VaccineColumn]);
                                immunizationViccineHxModel.RecordRefusal_Provider = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ProviderIdColumn]);
                                immunizationViccineHxModel.RecordRefusalReason = MDVUtility.ToStr(row[dsImmunization.VaccineHx.RefusalReasonIdColumn]);
                                immunizationViccineHxModel.RecordRefusalVaccine_ExpiryDate = string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.ExpiryDateColumn])) ? "" : MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.ExpiryDateColumn]).ToShortDateString());
                                immunizationViccineHxModel.RecordRefusal_Comments = MDVUtility.ToStr(row[dsImmunization.VaccineHx.CommentsColumn]);
                                if (row[dsImmunization.VaccineHx.VoidDoseColumn] != DBNull.Value)
                                {
                                    immunizationViccineHxModel.RecordRefusalVoidDose = Convert.ToBoolean(row[dsImmunization.VaccineHx.VoidDoseColumn]) == true ? "1" : "0";
                                }
                                else
                                {
                                    immunizationViccineHxModel.RecordRefusalVoidDose = "0";
                                }

                                immunizationViccineHxModel.RecordRefusal_IsActive = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ISActiveColumn]);
                                immunizationViccineHxModel.OrderSetId = MDVUtility.ToStr(row[dsImmunization.VaccineHx.OrderSetIdColumn]);
                                immunizationViccineHxModel.FacilityId = !string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.FacilityIdColumn])) ? MDVUtility.ToStr(row[dsImmunization.VaccineHx.FacilityIdColumn]) : "";
                                immunizationViccineHxModel.Facility = !string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.FacilityNameColumn])) ? MDVUtility.ToStr(row[dsImmunization.VaccineHx.FacilityNameColumn]) : "";
                                immunizationViccineHxModel.RecordRefusalEnteredBy = !string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.UserIdColumn])) ? MDVUtility.ToStr(row[dsImmunization.VaccineHx.UserIdColumn]) : "";
                                immunizationViccineHxModel.RecordRefusalRegistery = !string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineHx.ReceivingApplicationIdColumn])) ? MDVUtility.ToStr(row[dsImmunization.VaccineHx.ReceivingApplicationIdColumn]) : "";
                                immunizationViccineHxModel.RefusalVaccine_PublicityCode = MDVUtility.ToStr(row[dsImmunization.VaccineHx.PublicityCodeColumn]);
                                immunizationViccineHxModel.RefusalVaccine_PublicityExpiryDate = MDVUtility.ToStr(MDVUtility.ToStr(row[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn]) != "" ? MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn]).ToShortDateString() : row[dsImmunization.VaccineHx.PublicityCodeExpiryDateColumn]);
                                immunizationViccineHxModel.RefusalVaccine_IRS = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ImmunizationRegistryStatusCodeColumn]);
                                immunizationViccineHxModel.RefusalVaccine_IRSEffectiveDate = MDVUtility.ToStr(MDVUtility.ToStr(row[dsImmunization.VaccineHx.IRSEffectiveDateColumn]) != "" ? (MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.IRSEffectiveDateColumn]).ToShortDateString()) : row[dsImmunization.VaccineHx.IRSEffectiveDateColumn]);
                                immunizationViccineHxModel.RefusalVaccine_ProtectionIndicator = MDVUtility.ToStr(row[dsImmunization.VaccineHx.ProtectionIndicatorColumn]);
                                immunizationViccineHxModel.RefusalVaccine_PIEffectiveDate = MDVUtility.ToStr(MDVUtility.ToStr(row[dsImmunization.VaccineHx.PIEffectiveDateColumn]) != "" ? MDVUtility.ToDateTime(row[dsImmunization.VaccineHx.PIEffectiveDateColumn]).ToShortDateString() : row[dsImmunization.VaccineHx.PIEffectiveDateColumn]);
                                viewModelRef.Add(immunizationViccineHxModel);
                            }
                        }
                    }

                    var response = new
                    {
                        status = true,
                        AdminVaccineHxLoad_JSON = js.Serialize(viewModelAdmin),
                        DocVaccineHxLoad_JSON = js.Serialize(viewModelDoc),
                        RefusalVaccineLoad_JSON = js.Serialize(viewModelRef)

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ImmunizationCount = 0,
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





        public string Search_Immunization_Alerts(ImmunizationModel model)
        {
            try
            {

                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;

                obj = BLLClinicalObj.LoadImmunizationAlert(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                dsImmunization = obj.Data;
                if (obj.Data != null)
                {

                    if (dsImmunization.Tables[dsImmunization.PatientImmunizationAlert.TableName].Rows.Count == 0)
                    {
                        var response = new
                        {
                            status = true,
                            ImmunizationAlertCount = 0,
                            iTotalDisplayRecords = 0,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        List<ImmunizationAlert> ImmunizationAlertList = new List<ImmunizationAlert>();
                        foreach (DSImmunization.PatientImmunizationAlertRow dr in dsImmunization.PatientImmunizationAlert)
                        {

                            ImmunizationAlert ImmunizationAlertModel = new ImmunizationAlert();
                            ImmunizationAlertModel.AlertId = MDVUtility.ToStr(dr[dsImmunization.PatientImmunizationAlert.AlertIdColumn]);
                            ImmunizationAlertModel.VaccineName = MDVUtility.ToStr(dr[dsImmunization.PatientImmunizationAlert.VaccineNameColumn]);
                            ImmunizationAlertModel.Alert = MDVUtility.ToStr(dr[dsImmunization.PatientImmunizationAlert.AlertColumn]);
                            ImmunizationAlertModel.NoOfDays = MDVUtility.ToStr(dr[dsImmunization.PatientImmunizationAlert.NoOfDaysColumn]);
                            ImmunizationAlertModel.DueDate = MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsImmunization.PatientImmunizationAlert.DueDateColumn]).ToShortDateString());
                            ImmunizationAlertModel.Category = MDVUtility.ToStr(dr[dsImmunization.PatientImmunizationAlert.CategoryColumn]);
                            ImmunizationAlertModel.VaccineScheduleId = MDVUtility.ToStr(dr[dsImmunization.PatientImmunizationAlert.VaccineScheduleIdColumn]);
                            ImmunizationAlertList.Add(ImmunizationAlertModel);
                        }

                        var response = new
                        {
                            status = true,
                            ImmunizationAlertCount = dsImmunization.Tables[dsImmunization.PatientImmunizationAlert.TableName].Rows.Count,
                            ImmunizationAlert_Json = js.Serialize(ImmunizationAlertList),
                            iTotalDisplayRecords = (dsImmunization.PatientImmunizationAlert.Rows.Count > 0) ? dsImmunization.PatientImmunizationAlert.Rows[0][dsImmunization.PatientImmunizationAlert.RecordCountColumn.ColumnName] : 0,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ImmunizationAlertCount = 0,
                        iTotalDisplayRecords = 0,
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


        #region commented HL7 immunization
        //public string Generate_HL7Immunization_Message(ImmunizationModel model)
        //{
        //    try
        //    {
        //        BLObject<DSPatient> dsPatient = BLLPatientObj.FillPatient_PictureById(MDVUtility.ToInt64(model.PatientId));
        //        var dsVaccineCompleteDataSet = BLLClinicalObj.Generate_HL7Immunization_Message(model.VaccineHxIds);
        //        Generate_HL7_VXU_Message(dsVaccineCompleteDataSet, dsPatient);



        //        return Get_HL7Immunization_Message_Generation_Response(new object());

        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}


        //public void Generate_HL7_VXU_Message(BLObject<DSImmunizationHL7> dsImmunizationHl7, BLObject<DSPatient> dsPatient)
        //{
        //    VXU_V04 vxu = new VXU_V04();
        //    Get_RXA_Segment(ref vxu, dsImmunizationHl7);
        //}

        //public void Get_RXA_Segment(ref VXU_V04 vxu, BLObject<DSImmunizationHL7> dsImmunizationHl7)
        //{
        //    DSImmunizationHL7.VaccineDataTable vaccine = dsImmunizationHl7.Data.Vaccine;
        //   // vaccine[]

        //   vxu.ORDER.RXA.GiveSubIDCounter.Value = "0";
        //   vxu.ORDER.RXA.AdministrationSubIDCounter.Value = "1";
        //   vxu.ORDER.RXA.DateTimeStartOfAdministration.Time.Value =GetFormattedValue(dsImmunizationHl7.Data.VaccineHx[0].AdministrationDate.ToShortDateString());
        //   vxu.ORDER.RXA.DateTimeEndOfAdministration.Time.Value = GetFormattedValue(dsImmunizationHl7.Data.VaccineHx[0].)
        //   vxu.ORDER.RXA.DateTimeEndOfAdministration.Time.Value = "";
        //   vxu.ORDER.RXA.AdministeredCode.Identifier                       .Value = ;
        //   vxu.ORDER.RXA.AdministeredCode.Text                             .Value = ;
        //   vxu.ORDER.RXA.AdministeredCode.NameOfCodingSystem               .Value = ;
        //   vxu.ORDER.RXA.AdministeredCode.AlternateIdentifier              .Value = ;
        //   vxu.ORDER.RXA.AdministeredCode.AlternateText                    .Value = ;
        //   vxu.ORDER.RXA.AdministeredCode.NameOfAlternateCodingSystem      .Value = ;
        //   vxu.ORDER.RXA.
        //   vxu.ORDER.RXA.
        //   vxu.ORDER.RXA.




        //}

        //public string GetFormattedValue(string value)
        //{
        //    return (string.IsNullOrWhiteSpace(value) ? "" : value);
        //}

        //public string Get_HL7Immunization_Message_Generation_Response(object obj)
        //{
        //    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    var response = new
        //    {
        //        status = true,
        //        //   VIS_url = obj.ToString() == "Object reference not set to an instance of an object." ? "" : obj
        //    };
        //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //}

        #endregion

        #region "ProblemLists with Notes"

        /// <summary>
        /// this function will get latest allergy for notes attachment
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        //public string getLatestVaccineByPatientId(Int64 PatientId, Int64 userId)
        //{
        //    try
        //    {

        //        DSImmunization dsVaccine = null;
        //        BLObject<DSImmunization> obj;

        //        obj = BLLClinicalObj.LoadVaccine("", PatientId, userId);

        //        dsVaccine = obj.Data;
        //        DSImmunization dsVaccine1 = null;
        //        BLObject<DSImmunization> obj1;

        //        obj1 = BLLClinicalObj.LoadImmTherapeuticInjectionForSoapText("", PatientId, userId);

        //        dsVaccine1 = obj1.Data;
        //        dsVaccine.Merge(dsVaccine1.TherapeuticInjection);
        //        var response = new
        //        {
        //            status = true,
        //            VaccineCount = dsVaccine.Tables[dsVaccine.Vaccine.TableName].Rows.Count,
        //            VaccineLoad_JSON = MDVUtility.JSON_DataTable(dsVaccine.Tables[dsVaccine.Vaccine.TableName]),
        //            TheraeuticInjectionCount = dsVaccine.Tables[dsVaccine.TherapeuticInjection.TableName].Rows.Count,
        //            TheraeuticInjectionLoad_JSON = MDVUtility.JSON_DataTable(dsVaccine.Tables[dsVaccine.TherapeuticInjection.TableName])
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


        /// <summary>
        /// this function will retrive allergy information for Notes attachment
        /// </summary>
        /// <param name="VaccineId"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        internal string getVaccineForSoap(string VaccineId, long PatientId)
        {
            try
            {

                DSImmunization dsVaccine = null;
                BLObject<DSImmunization> obj = BLLClinicalObj.LoadVaccine("", MDVUtility.ToLong(PatientId), 0);
                dsVaccine = obj.Data;
                if (obj.Data != null)
                {
                    if (dsVaccine.Tables[dsVaccine.Vaccine.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VaccineCount = dsVaccine.Tables[dsVaccine.Vaccine.TableName].Rows.Count,
                            VaccineLoad_JSON = MDVUtility.JSON_DataTable(dsVaccine.Tables[dsVaccine.Vaccine.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VaccineCount = 0,
                            VaccineLoad_JSON = MDVUtility.JSON_DataTable(dsVaccine.Tables[dsVaccine.Vaccine.TableName]),
                            Message = Common.AppPrivileges.No_Record_Message
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
        /// This Function will detach Vital sign from notes
        /// </summary>
        /// <param name="VitalSignId"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string detach_Vaccine_From_Notes(string ProcedureId, long NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(ProcedureId) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachVaccineFromNotes(ProcedureId, NotesId);
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

        internal string detachTheraInjectionwithnotes(string ImmTherInjectionId, long NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(ImmTherInjectionId) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachTheraInjectionwithnotes(ImmTherInjectionId, NotesId);
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

        /// <summary>
        /// This Function will attach vital sign to notes
        /// </summary>
        /// <param name="VitalSignId"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string attach_Vaccine_With_Notes(string ProcedureId, long NotesId)
        {
            try
            {
                DSImmunization dsProcedure = null;
                if (string.IsNullOrEmpty(ProcedureId) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<DSImmunization> obj = BLLClinicalObj.attachVaccineWithNotes(ProcedureId, NotesId);
                    if (obj.Data != null)
                    {
                        dsProcedure = obj.Data;
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
                            Message = obj.Message
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



        internal string attachTheraInjectionwithnotes(string ImmTherInjectionId, long NotesId)
        {
            try
            {
                DSImmunization dsProcedure = null;
                if (string.IsNullOrEmpty(ImmTherInjectionId) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<DSImmunization> obj = BLLClinicalObj.attachTheraInjectionwithnotes(ImmTherInjectionId, NotesId);
                    if (obj.Data != null)
                    {
                        dsProcedure = obj.Data;
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
                            Message = obj.Message
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

        public string ActiveInActiveVaccine(ImmunizationModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.VaccineHxId) > 0)
                {

                    DSImmunization dsImmunization = new DSImmunization();
                    //BLObject<DSProblemLists> obj = BLLClinicalObj.LoadProblemLists(MDVUtility.ToInt64(model.VaccineHxId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId));
                    var obj = BLLClinicalObj.LoadVaccineHx(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.VaccineHxId), "", "");
                    dsImmunization = obj.Data;
                    foreach (DSImmunization.VaccineHxRow dr in dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows)
                    {


                        dr.ISActive = MDVUtility.ToByte(model.IsActiveRecord);

                        //dr.IsActive = false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        //end newly added
                    }
                    #region Database Updation
                    if (dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows.Count > 0)
                    {
                        obj = BLLClinicalObj.UpdateVaccineHx(dsImmunization);
                        dsImmunization = obj.Data;

                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,
                                VaccineHxIdColumn = dsImmunization.Tables[dsImmunization.VaccineHx.TableName].Rows[0][dsImmunization.VaccineHx.VaccineHxIdColumn.ColumnName]
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = obj.Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
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
                        message = "Vaccine not found."
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



        public string load_SearchSchedlerData(ImmunizationModel model)
        {
            try
            {
                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj = null;
                if (model.TabId != null)
                {
                    obj = BLLClinicalObj.loadSchedulerData(model.TabId, MDVUtility.ToLong(model.PatientId));
                }

                dsImmunization = obj.Data;
                if (obj.Data != null)
                {
                    if (dsImmunization.VaccineSchedule.Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SchedulerCount = (dsImmunization.VaccineSchedule.Rows.Count > 0) ? dsImmunization.VaccineSchedule.Rows.Count : 0,
                            SchedulerLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.VaccineSchedule.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SchedulerCount = 0,
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

        public string GetPreviewSchedulerData(ImmunizationModel model)
        {
            try
            {
                DSImmunization dsImmunization = new DSImmunization();
                DSNotes dsHeaderScheduler = new DSNotes();
                BLObject<DSNotes> objHeaderLoad = BLLClinicalObj.loadClinicalSchedulerHeaderData(MDVUtility.ToLong(model.PatientId));
                dsHeaderScheduler = objHeaderLoad.Data;
                dsImmunization.Merge(dsHeaderScheduler);

                DSImmunization dsSchedulerTreeData = new DSImmunization();
                BLObject<DSImmunization> objdsSchedulerTreeData = BLLClinicalObj.loadClinicalSchedulerDataForPreView(MDVUtility.ToLong(model.PatientId));
                dsSchedulerTreeData = objdsSchedulerTreeData.Data;
                dsImmunization.Merge(dsSchedulerTreeData);

                DSPatient dsPatientInfo = new DSPatient();
                DSProfile dsProvider = new DSProfile();
                List<ImmunizationScheduler> SchedulerData = new List<ImmunizationScheduler>();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                foreach (DSImmunization.VaccineScheduleRow row in dsImmunization.Tables[dsImmunization.VaccineSchedule.TableName].Rows)
                {

                    ImmunizationScheduler immunizationSchedulerModel = new ImmunizationScheduler();

                    immunizationSchedulerModel.VaccineHxId = MDVUtility.ToStr(row[dsImmunization.VaccineSchedule.VaccineHxIdColumn]);
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineSchedule.AdministrationDateColumn])))
                    {
                        immunizationSchedulerModel.AdministrationDate = string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.VaccineSchedule.AdministrationDateColumn])) ? "" : MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.VaccineSchedule.AdministrationDateColumn]).ToShortDateString());
                        DateTime AdministerTime = MDVUtility.ToDateTime(row[dsImmunization.VaccineSchedule.AdministrationDateColumn]);
                        immunizationSchedulerModel.AdministrationTime = ((MDVUtility.ToStr(AdministerTime).IndexOf("AM") > 0 ? (AdministerTime.Hour == 0 ? 12 : AdministerTime.Hour) : (AdministerTime.Hour == 12 ? AdministerTime.Hour : ((AdministerTime.Hour) - 12))) + ":" + ((AdministerTime.Minute.ToString().Length) > 1 ? AdministerTime.Minute.ToString() : '0' + AdministerTime.Minute.ToString()) + " " + (MDVUtility.ToStr(AdministerTime).IndexOf("PM") > 0 ? "PM" : "AM"));
                    }
                    else
                    {
                        immunizationSchedulerModel.AdministrationDate = "";
                        immunizationSchedulerModel.AdministrationTime = "";
                    }
                    immunizationSchedulerModel.PatientAge = MDVUtility.ToStr(row[dsImmunization.VaccineSchedule.PatientAgeColumn]);
                    immunizationSchedulerModel.GivenBy = MDVUtility.ToStr(row[dsImmunization.VaccineSchedule.GivenByColumn]);
                    immunizationSchedulerModel.Category = MDVUtility.ToStr(row[dsImmunization.VaccineSchedule.CategoryColumn]);
                    immunizationSchedulerModel.Schedule = MDVUtility.ToStr(row[dsImmunization.VaccineSchedule.ScheduleColumn]);
                    immunizationSchedulerModel.Type = MDVUtility.ToStr(row[dsImmunization.VaccineSchedule.TypeColumn]);
                    immunizationSchedulerModel.VaccineScheduleId = MDVUtility.ToStr(row[dsImmunization.VaccineSchedule.VaccineScheduleIdColumn]);


                    SchedulerData.Add(immunizationSchedulerModel);

                }
                if (dsImmunization.Tables[dsProvider.Practice.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        SchedulerHeaderPatientData = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsPatientInfo.Patients.TableName]),
                        SchedulerPracticeData = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsProvider.Practice.TableName]),
                        CategoryData = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.LookUpVaccineGroup.TableName]),
                        SchedulerData = js.Serialize(SchedulerData)
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Notes = "Notes not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Notes = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        ////// Method Name: Preview_Immunization
        /// Author : M Ahmad Imran
        /// Created Date: 15-08-2016
        /// Description: Creates PDF to view Immunization
        /// </summary>
        /// <param name="model" type="ConsultationOrderModel">model</param>      
        public string Preview_Immunization(ImmunizationModel model)
        {
            try
            {
                //previewReferral

                BLObject<byte[]> obj = BLLClinicalObj.Preview_Immunization(MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ImmunizationHTML = Convert.ToBase64String(obj.Data),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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


        ////// Method Name: GetImmunizationAlertForPrint
        /// Author : M Ahmad Imran
        /// Created Date: 23-08-2016
        /// Description: Creates PDF to view Immunization
        /// </summary>
        /// <param name="model" type="ConsultationOrderModel">model</param>      
        public string GetImmunizationAlertForPrint(ImmunizationModel model)
        {
            try
            {
                //previewReferral

                BLObject<byte[]> obj = BLLClinicalObj.GetImmunizationAlertForPrint(MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ImmunizationAlertHTML = Convert.ToBase64String(obj.Data),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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



        public bool GetImmunizationPrivileges(ImmunizationModel model)
        {
            try
            {
                if (MDVSession.Current.isEMR && MDVSession.Current.ListUserPrivileges != null)
                {
                    return MDVSession.Current.ListUserPrivileges.FirstOrDefault(a => a.FormName == model.FormName) != null;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LotNumber
        /// Author: M Ahmad Imran
        /// Purpose:  to load Vaccines for LotNumber
        /// Date : June 08, 2016
        public string GetAllVaccines(LotNumberModel model)
        {
            try
            {

                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;
                obj = BLLClinicalObj.SearchVaccine(model.SearchText);
                dsImmunization = obj.Data;
                var response = new
                {
                    status = true,
                    VaccineCount = dsImmunization.Tables[dsImmunization.SearchVaccine.TableName].Rows.Count,
                    VaccineLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.SearchVaccine.TableName]),
                    iTotalDisplayRecords = (dsImmunization.SearchVaccine.Rows.Count > 0) ? dsImmunization.SearchVaccine.Rows[0][dsImmunization.SearchVaccine.RecordCountColumn.ColumnName] : 0,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        /// Author: M Ahmad Imran
        /// Purpose:  to load Manufacturers for LotNumber
        /// Date : June 08, 2016
        public string GetAllManufacturers(LotNumberModel model)
        {
            try
            {

                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;
                obj = BLLClinicalObj.SearchManufacturer(model.SearchText);
                dsImmunization = obj.Data;
                var response = new
                {
                    status = true,
                    ManufacturerCount = dsImmunization.Tables[dsImmunization.SearchManufacturer.TableName].Rows.Count,
                    ManufacturerLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.SearchManufacturer.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        #region Immunization Category
        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function Load Immunization Category
        public string LoadImmunizationCategory(CategoryModel model)
        {
            try
            {

                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;
                obj = BLLClinicalObj.LoadImmunizationCategory(MDVUtility.ToLong(model.VaccineGroupID), MDVUtility.ToStr(model.ShortName), MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsImmunization = obj.Data;
                var response = new
                {
                    status = true,
                    CategoryCount = dsImmunization.Tables[dsImmunization.Category.TableName].Rows.Count,
                    iTotalDisplayRecords = (dsImmunization.Category.Rows.Count > 0) ? dsImmunization.Category.Rows[0][dsImmunization.Category.RecordCountColumn.ColumnName] : 0,
                    CategoryLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.Category.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function to Activate/DeActivate Immunization Category
        public string ActiveInActiveCategory(CategoryModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.VaccineGroupID) > 0)
                {

                    DSImmunization dsDSImmunization = new DSImmunization();
                    BLObject<DSImmunization> obj = BLLClinicalObj.LoadImmunizationCategory(MDVUtility.ToInt64(model.VaccineGroupID), null, null, 1, 1);
                    dsDSImmunization = obj.Data;
                    foreach (DSImmunization.CategoryRow dr in dsDSImmunization.Tables[dsDSImmunization.Category.TableName].Rows)
                    {
                        dr.IsActive = MDVUtility.ToStr(model.IsActive) == "1" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifyOn = DateTime.Now;
                        dr.EntityId = MDVUtility.ToInt64(model.EntityId);
                    }

                    #region Database Updation
                    if (dsDSImmunization.Tables[dsDSImmunization.Category.TableName].Rows.Count > 0)
                    {
                        BLObject<DSImmunization> objUpdate = BLLClinicalObj.UpdateImmunizationCategory(dsDSImmunization);
                        if (objUpdate.Data != null)
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
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
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
                        message = "Category not found."
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
        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function to delete Immunization Category
        public string DeleteCategory(CategoryModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.VaccineGroupID)))
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

                    BLObject<string> obj = BLLClinicalObj.DeleteImmunizationCategory(MDVUtility.ToStr(model.VaccineGroupID));
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




        /// Author: M Ahmad Imran.
        /// Date : 22-08-2016
        /// Purpose : Function to Insert Or Update Patient Immunization Alert
        public string InsertOrUpdatePatientImmunizationAlert(long PatientId, bool IsVaccineInsert)
        {
            try
            {
                ImmunizationModel model = new ImmunizationModel();
                model.FormName = "Medical_Immunization";
                if (GetImmunizationPrivileges(model) && !string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    BLObject<string> obj = BLLClinicalObj.InsertOrUpdatePatientImmunizationAlertData(PatientId, IsVaccineInsert);
                    if (obj.Data == "")
                    {
                        BLObject<string> obj_ = BLLClinicalObj.Get_ImmunizationAlertCount(PatientId);
                        var response = new
                        {
                            status = true,
                            ImmunizationAlertCount = obj_.Data
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = string.Empty
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

        /// Author: M Ahmad Imran
        /// Date : 19/8/2016
        /// Purpose : Function to Get Immunization Alert Count
        public string Get_ImmunizationAlertCount(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)))
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

                    BLObject<string> obj = BLLClinicalObj.Get_ImmunizationAlertCount(MDVUtility.ToLong(model.PatientId));
                    var response = new
                    {
                        status = true,
                        ImmunizationAlertCount = obj.Data
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


        public string GetProcedureIdAgainstTherapeuticInjectionId(TherapeuticInjectionModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ImmTherInjectionId)))
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

                    BLObject<string> obj = BLLClinicalObj.GetProcedureIdAgainstImmTherapeuticInjectionId(MDVUtility.ToLong(model.ImmTherInjectionId));
                    var response = new
                    {
                        status = true,
                        ProcedureId = obj.Data
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





        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function to save Immunization Category
        public string SaveImmunizationCategory(CategoryModel model)
        {
            try
            {
                DSImmunization dsImmCat = new DSImmunization();
                DSImmunization.CategoryRow dr = dsImmCat.Category.NewCategoryRow();
                bool active = model.IsActive == "1" ? active = true : active = false;
                dr.ShortName = MDVUtility.ToStr(model.ShortName);
                dr.IsActive = active;
                dr.EntityId = MDVUtility.ToInt64(model.EntityId);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                //dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                //dr.ModifyOn = DateTime.Now;

                #region Database Insertion
                dsImmCat.Category.AddCategoryRow(dr);
                BLObject<DSImmunization> obj = BLLClinicalObj.InsertImmunizationCategory(dsImmCat);
                dsImmCat = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        VaccineGroupId = dsImmCat.Tables[dsImmCat.Category.TableName].Rows[0][dsImmCat.Category.VaccineGroupIDColumn.ColumnName]
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        /// Author: Khaleel Ur Rehman.
        /// Date : 14-06-2016
        /// Purpose : Function to update Immunization Category
        public string UpdateImmunizationCategory(CategoryModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.VaccineGroupID) > 0)
                {

                    DSImmunization dsImmCat = new DSImmunization();
                    BLObject<DSImmunization> obj = BLLClinicalObj.LoadImmunizationCategory(MDVUtility.ToInt64(model.VaccineGroupID), null, null, 1, 1);
                    dsImmCat = obj.Data;
                    foreach (DSImmunization.CategoryRow dr in dsImmCat.Tables[dsImmCat.Category.TableName].Rows)
                    {
                        bool active = model.IsActive == "1" ? active = true : active = false;
                        if (!string.IsNullOrEmpty(model.ShortName))
                            dr.ShortName = MDVUtility.ToStr(model.ShortName);
                        if (!string.IsNullOrEmpty(model.IsActive))
                            dr.IsActive = active;
                        if (!string.IsNullOrEmpty(model.EntityId))
                            dr.EntityId = MDVUtility.ToInt64(model.EntityId);
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifyOn = DateTime.Now;
                    }
                    #region Database Updation
                    if (dsImmCat.Tables[dsImmCat.Category.TableName].Rows.Count > 0)
                    {
                        BLObject<DSImmunization> objUpdate = BLLClinicalObj.UpdateImmunizationCategory(dsImmCat);
                        if (objUpdate.Data != null)
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
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
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
                        message = "Category not found."
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

        #region Immunization VaccineCrosswalk
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Load Immunization VaccineCrosswalk
        public string LoadImmunizationVaccineCrosswalk(VaccineCrosswalk model)
        {
            try
            {

                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;
                obj = BLLClinicalObj.LoadImmunizationVaccineCrosswalk(MDVUtility.ToLong(model.VaccineCrosswalkID), MDVUtility.ToLong(model.VaccineGroupID), MDVUtility.ToLong(model.VaccineId), MDVUtility.ToStr(model.IsActive), MDVUtility.ToStr(model.IsDefault), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsImmunization = obj.Data;
                var response = new
                {
                    status = true,
                    VaccineCrosswalkCount = dsImmunization.Tables[dsImmunization.VaccineCrosswalk.TableName].Rows.Count,
                    iTotalDisplayRecords = (dsImmunization.VaccineCrosswalk.Rows.Count > 0) ? dsImmunization.VaccineCrosswalk.Rows[0][dsImmunization.VaccineCrosswalk.RecordCountColumn.ColumnName] : 0,
                    VaccineCrosswalkLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.VaccineCrosswalk.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Active/Inactive Immunization VaccineCrosswalk
        public string ActiveInActiveVaccineCrosswalk(VaccineCrosswalk model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.VaccineCrosswalkID) > 0)
                {

                    DSImmunization dsDSImmunization = new DSImmunization();
                    BLObject<DSImmunization> obj = BLLClinicalObj.LoadImmunizationVaccineCrosswalk(MDVUtility.ToInt64(model.VaccineCrosswalkID), 0, 0, "", "", 1, 1);
                    dsDSImmunization = obj.Data;
                    foreach (DSImmunization.VaccineCrosswalkRow dr in dsDSImmunization.Tables[dsDSImmunization.VaccineCrosswalk.TableName].Rows)
                    {
                        dr.IsActive = MDVUtility.ToStr(model.IsActive) == "1" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    if (dsDSImmunization.Tables[dsDSImmunization.VaccineCrosswalk.TableName].Rows.Count > 0)
                    {
                        BLObject<DSImmunization> objUpdate = BLLClinicalObj.UpdateImmunizationVaccineCrosswalk(dsDSImmunization);
                        if (objUpdate.Data != null)
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
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
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
                        message = "Vaccine Crosswalk not found."
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
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Delete Immunization VaccineCrosswalk
        public string DeleteVaccineCrosswalk(VaccineCrosswalk model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.VaccineCrosswalkID)))
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

                    BLObject<string> obj = BLLClinicalObj.DeleteImmunizationVaccineCrosswalk(MDVUtility.ToStr(model.VaccineCrosswalkID));
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
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Save Immunization VaccineCrosswalk
        public string SaveImmunizationVaccineCrosswalk(VaccineCrosswalk model)
        {
            try
            {
                DSImmunization dsImm = new DSImmunization();
                DSImmunization.VaccineCrosswalkRow dr = dsImm.VaccineCrosswalk.NewVaccineCrosswalkRow();
                bool active = model.IsActive == "True" ? true : false;
                bool isdefault = model.IsDefault == "True" ? true : false;
                dr.VaccineGroupId = MDVUtility.ToLong(model.VaccineGroupID);
                dr.VaccineId = MDVUtility.ToInt64(model.VaccineId);
                dr.IsActive = active;
                dr.IsDefault = isdefault;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;

                #region Database Insertion
                dsImm.VaccineCrosswalk.AddVaccineCrosswalkRow(dr);
                BLObject<DSImmunization> obj = BLLClinicalObj.InsertImmunizationVaccineCrosswalk(dsImm);
                dsImm = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        VaccineCrosswalkId = dsImm.Tables[dsImm.VaccineCrosswalk.TableName].Rows[0][dsImm.VaccineCrosswalk.VaccineCrosswalkIdColumn.ColumnName]
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to update Immunization VaccineCrosswalk
        public string UpdateImmunizationVaccineCrosswalk(VaccineCrosswalk model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.VaccineCrosswalkID) > 0)
                {

                    DSImmunization dsImm = new DSImmunization();
                    BLObject<DSImmunization> obj = BLLClinicalObj.LoadImmunizationVaccineCrosswalk(MDVUtility.ToInt64(model.VaccineCrosswalkID), 0, 0, "", "", 1, 1);
                    dsImm = obj.Data;
                    foreach (DSImmunization.VaccineCrosswalkRow dr in dsImm.Tables[dsImm.VaccineCrosswalk.TableName].Rows)
                    {
                        bool active = model.IsActive == "True" ? true : false;
                        bool isdefault = model.IsDefault == "True" ? true : false;
                        if (!string.IsNullOrEmpty(model.VaccineGroupID))
                            dr.VaccineGroupId = MDVUtility.ToLong(model.VaccineGroupID);
                        if (!string.IsNullOrEmpty(model.IsActive))
                            dr.IsActive = active;
                        if (!string.IsNullOrEmpty(model.IsDefault))
                            dr.IsDefault = isdefault;
                        if (!string.IsNullOrEmpty(model.VaccineId))
                            dr.VaccineId = MDVUtility.ToLong(model.VaccineId);
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }
                    #region Database Updation
                    if (dsImm.Tables[dsImm.VaccineCrosswalk.TableName].Rows.Count > 0)
                    {
                        BLObject<DSImmunization> objUpdate = BLLClinicalObj.UpdateImmunizationVaccineCrosswalk(dsImm);
                        if (objUpdate.Data != null)
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
                                Message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ""
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
                        Message = "Vaccine Crosswalk not found."
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


        #region Add Vaccine And Therapeutic
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Load Immunization VaccineCrosswalk

        public string GetManufacturerArray(Manufacturer model)
        {
            try
            {
                List<Manufacturer> ManufacturerList = null;
                BLObject<List<Manufacturer>> obj;

                obj = BLLClinicalObj.GetManufacturerArray(model.ManufacturerName, model.VaccineId, model.TherapeuticId);
                ManufacturerList = obj.Data;
                if (obj.Data != null)
                {
                    if (ManufacturerList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ManufacturerCount = ManufacturerList.Count,
                            Manufacturer_JSON = ManufacturerList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ManufacturerCount = 0,
                            Message = "Record not found."
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
        public string GetImmAndTheraArray(VaccineAndTerapuetic model)
        {
            try
            {
                List<VaccineAndTerapuetic> VaccineNameList = null;
                BLObject<List<VaccineAndTerapuetic>> obj;

                obj = BLLClinicalObj.GetImmAndTheraArray(model.ImmunizationName, MDVUtility.ToInt(model.Type), model.CptBaseSearch);
                VaccineNameList = obj.Data;
                if (obj.Data != null)
                {
                    if (VaccineNameList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VaccineNameCount = VaccineNameList.Count,
                            VaccineName_JSON = VaccineNameList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VaccineNameCount = 0,
                            Message = "Record not found."
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

        public string LoadManufacturer(Manufacturer model)
        {
            try
            {
                List<Manufacturer> ManufacturerList = null;
                BLObject<List<Manufacturer>> obj;

                obj = BLLClinicalObj.LoadManufacturer(model.ManufacturerName, model.MVXCode, model.Status, MDVUtility.ToInt64(model.PageNumber), MDVUtility.ToInt64(model.RowsPerPage));
                ManufacturerList = obj.Data;
                if (obj.Data != null)
                {
                    if (ManufacturerList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ManufacturerCount = ManufacturerList.Count,
                            Manufacturer_JSON = ManufacturerList,
                            iTotalDisplayRecords = ManufacturerList[0].RecordCount,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VaccineCount = 0,
                            Message = "Record not found."
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
        public string loadVaccineAndTherapeutic(VaccineAndTerapuetic model)
        {
            try
            {
                List<VaccineAndTerapuetic> VaccineList = null;
                BLObject<List<VaccineAndTerapuetic>> obj;

                obj = BLLClinicalObj.loadVaccineAndTherapeutic(model.ImmunizationId, MDVUtility.ToInt(model.Type), model.Status, MDVUtility.ToInt64(model.PageNumber), MDVUtility.ToInt64(model.RowsPerPage));
                VaccineList = obj.Data;
                if (obj.Data != null)
                {
                    if (VaccineList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VaccineCount = VaccineList.Count,
                            Vaccine_JSON = VaccineList,
                            iTotalDisplayRecords = VaccineList[0].RecordCount,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VaccineCount = 0,
                            Message = "Record not found."
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


        public string GetVaccineInformationForAutoPopu(VaccineAndTerapuetic model)
        {
            try
            {
                List<VaccineAndTerapuetic> VaccineDetail = null;
                BLObject<List<VaccineAndTerapuetic>> obj;

                obj = BLLClinicalObj.GetVaccineInformationForAutoPopu(model.Id, model.Type);
                VaccineDetail = obj.Data;
                if (obj.Data != null)
                {
                    if (VaccineDetail.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VaccineCount = 1,
                            Vaccine_JSON = VaccineDetail,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            VaccineCount = 0,
                            Message = "Record not found."
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

        public string LoadManufacturerDetail(Manufacturer model)
        {
            try
            {
                List<Manufacturer> ManufacturerDetail = null;
                BLObject<List<Manufacturer>> obj;

                obj = BLLClinicalObj.LoadManufacturerDetail(MDVUtility.ToInt64(model.ManufacturerId));
                ManufacturerDetail = obj.Data;
                if (obj.Data != null)
                {
                    if (ManufacturerDetail != null)
                    {
                        var response = new
                        {
                            status = true,
                            ManufacturerCount = 1,
                            Manufacturer_JSON = ManufacturerDetail,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            VaccineCount = 0,
                            Message = "Record not found."
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
        public string LoadVaccineDetail(VaccineAndTerapuetic model)
        {
            try
            {
                VaccineAndTerapuetic VaccineDetail = null;
                BLObject<VaccineAndTerapuetic> obj;

                obj = BLLClinicalObj.LoadVaccineDetail(MDVUtility.ToInt64(model.ImmunizationId));
                VaccineDetail = obj.Data;
                if (obj.Data != null)
                {
                    if (VaccineDetail != null)
                    {
                        var response = new
                        {
                            status = true,
                            VaccineCount = 1,
                            Vaccine_JSON = VaccineDetail,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            VaccineCount = 0,
                            Message = "Record not found."
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

        public string LoadTherapeuticDetail(VaccineAndTerapuetic model)
        {
            try
            {
                List<VaccineAndTerapuetic> TherapeuticDetail = null;
                BLObject<List<VaccineAndTerapuetic>> obj;

                obj = BLLClinicalObj.LoadTherapeuticDetail(MDVUtility.ToInt(model.TherapeuticId));
                TherapeuticDetail = obj.Data;
                if (obj.Data != null)
                {
                    if (TherapeuticDetail.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            TherapeuticCount = 1,
                            Therapeutic_JSON = TherapeuticDetail,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            TherapeuticCount = 0,
                            Message = "Record not found."
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
        public string SaveManufacturer(Manufacturer model)
        {
            try
            {
                model.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

                string ManufacturerId = BLLClinicalObj.SaveManufacturer(model);

                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
                };
                return (JsonConvert.SerializeObject(response));
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
        public string SaveVaccine(VaccineAndTerapuetic model)
        {
            try
            {
                model.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

                string VaccineId = BLLClinicalObj.SaveVaccine(model);
                foreach (VaccineVIS item in model.VaccineVisInformation)
                {
                    item.VaccineId = VaccineId;
                    item.CVX = model.CVX;
                    string VaccineVISId = BLLClinicalObj.SaveVaccineVIS(item);
                }


                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
                };
                return (JsonConvert.SerializeObject(response));
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
        public string SaveTherapeutic(VaccineAndTerapuetic model)
        {
            try
            {
                model.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                string TherapeuticId = BLLClinicalObj.SaveTherapeutic(model);
                if (MDVUtility.ToInt(TherapeuticId) > 0)
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = TherapeuticId,
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }


        public string ActiveOrInactiveManufacturer(Manufacturer model)
        {
            try
            {


                string ManufacturerId = BLLClinicalObj.ActiveOrInactiveManufacturer(model);
                var Message = AppPrivileges.Active_Message;
                if (model.Status == "Active")
                {
                    Message = AppPrivileges.Active_Message;
                }
                else
                {
                    Message = AppPrivileges.Inactive_Message;
                }

                var response = new
                {
                    status = true,
                    Message = Message,
                };
                return (JsonConvert.SerializeObject(response));
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

        public string ActiveOrInactiveImmOrThera(VaccineAndTerapuetic model)
        {
            try
            {


                string VaccineId = BLLClinicalObj.ActiveOrInactiveImmOrThera(model);
                var Message = AppPrivileges.Active_Message;
                if (model.Status == "1")
                {
                    Message = AppPrivileges.Active_Message;
                }
                else
                {
                    Message = AppPrivileges.Inactive_Message;
                }

                var response = new
                {
                    status = true,
                    Message = Message,
                };
                return (JsonConvert.SerializeObject(response));
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

        public string UpdateManufacturer(Manufacturer model)
        {
            try
            {
                model.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

                string ManufacturerId = BLLClinicalObj.UpdateManufacturer(model);



                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Update_Message,
                };
                return (JsonConvert.SerializeObject(response));
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
        public string UpdateVaccine(VaccineAndTerapuetic model)
        {
            try
            {
                model.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

                string VaccineId = BLLClinicalObj.UpdateVaccine(model);
                foreach (VaccineVIS item in model.VaccineVisInformation)
                {
                    if (item.Mode == "Add")
                    {
                        item.VaccineId = model.ImmunizationId;
                        item.CVX = model.CVX;
                        string VaccineVISId = BLLClinicalObj.SaveVaccineVIS(item);
                    }
                    else
                    {
                        item.CVX = model.CVX;
                        string VaccineVISId = BLLClinicalObj.UpdateVaccineVis(item);
                    }

                }


                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Update_Message,
                };
                return (JsonConvert.SerializeObject(response));
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

        public string UpdateTherapeutic(VaccineAndTerapuetic model)
        {
            try
            {
                model.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

                string TherapeuticId = BLLClinicalObj.UpdateTherapeutic(model);
                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Update_Message,
                };
                return (JsonConvert.SerializeObject(response));
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


        public string DeleteImmunization(VaccineAndTerapuetic model)
        {
            try
            {
                long ImmunizationId = MDVUtility.ToInt64(model.Id);
                if (ImmunizationId > 0)
                {

                    string ResponseOfDeleteCall = BLLClinicalObj.DeleteImmunization(ImmunizationId);
                    if (ResponseOfDeleteCall.Contains("Delete issue."))
                    {
                        var response = new
                        {
                            status = false,
                            ResponseOfDeleteCall = ResponseOfDeleteCall,
                            Message = ResponseOfDeleteCall.Replace("Delete issue.", ""),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ResponseOfDeleteCall = ResponseOfDeleteCall,
                            Message = AppPrivileges.Delete_Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = AppPrivileges.Delete_Error_Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string DeleteVISInformation(VaccineAndTerapuetic model)
        {
            try
            {
                long VISInformationId = MDVUtility.ToInt64(model.VaccineVISId);
                if (VISInformationId > 0)
                {

                    string ResponseOfDeleteCall = BLLClinicalObj.DeleteVISInformation(VISInformationId);
                    if (ResponseOfDeleteCall.Contains("Delete issue."))
                    {
                        var response = new
                        {
                            status = false,
                            ResponseOfDeleteCall = ResponseOfDeleteCall,
                            Message = ResponseOfDeleteCall.Replace("Delete issue.", ""),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ResponseOfDeleteCall = ResponseOfDeleteCall,
                            Message = AppPrivileges.Delete_Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = AppPrivileges.Delete_Error_Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }


        public string DeleteManufacturer(Manufacturer model)
        {
            try
            {
                long ManufacturerId = MDVUtility.ToInt64(model.ManufacturerId);
                if (ManufacturerId > 0)
                {

                    string ResponseOfDeleteCall = BLLClinicalObj.DeleteManufacturer(ManufacturerId);
                    if (ResponseOfDeleteCall.Contains("Delete issue."))
                    {
                        var response = new
                        {
                            status = false,
                            ResponseOfDeleteCall = ResponseOfDeleteCall,
                            Message = ResponseOfDeleteCall.Replace("Delete issue.", ""),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ResponseOfDeleteCall = ResponseOfDeleteCall,
                            Message = AppPrivileges.Delete_Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = AppPrivileges.Delete_Error_Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }
        public string DeleteTherapeutic(VaccineAndTerapuetic model)
        {
            try
            {
                long TherapeuticId = MDVUtility.ToInt64(model.Id);
                if (TherapeuticId > 0)
                {

                    string ResponseOfDeleteCall = BLLClinicalObj.DeleteTherapeutic(TherapeuticId);
                    if (ResponseOfDeleteCall.Contains("Delete issue."))
                    {
                        var response = new
                        {
                            status = false,
                            ResponseOfDeleteCall = ResponseOfDeleteCall,
                            Message = ResponseOfDeleteCall.Replace("Delete issue.", ""),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ResponseOfDeleteCall = ResponseOfDeleteCall,
                            Message = AppPrivileges.Delete_Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = AppPrivileges.Delete_Error_Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }
        #endregion

        /// Author: Azeem Raza Tayyab
        /// Purpose:  to load Vaccines
        /// Date : 22-07-2016
        public string LoadVaccineList(Vaccine model)
        {
            try
            {

                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;
                obj = BLLClinicalObj.LoadVaccineList(MDVUtility.ToLong(model.VaccineID), MDVUtility.ToLong(model.VaccineGroupID), MDVUtility.ToStr(model.VaccineStatus), MDVUtility.ToStr(model.CVXShortDescription), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsImmunization = obj.Data;
                var response = new
                {
                    status = true,
                    VaccineCount = dsImmunization.Tables[dsImmunization.VaccineList.TableName].Rows.Count,
                    VaccineLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.VaccineList.TableName]),
                    iTotalDisplayRecords = (dsImmunization.VaccineList.Rows.Count > 0) ? dsImmunization.VaccineList.Rows[0][dsImmunization.VaccineList.RecordCountColumn.ColumnName] : 0,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        #region Immunization ScheduleSetup
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Load Immunization ScheduleSetup
        public string LoadImmunizationScheduleSetup(ImmunizationModel model)
        {
            try
            {

                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;
                obj = BLLClinicalObj.LoadImmunizationScheduleSetup(MDVUtility.ToStr(model.ScheduleTypeId), MDVUtility.ToLong(model.VaccineScheduleId), MDVUtility.ToStr(model.ScheduleId), MDVUtility.ToStr(model.VaccineGroupID), MDVUtility.ToStr(model.IsActive));
                dsImmunization = obj.Data;
                var response = new
                {
                    status = true,
                    ScheduleSetupCount = dsImmunization.Tables[dsImmunization.VaccineSchedule.TableName].Rows.Count,
                    iTotalDisplayRecords = (dsImmunization.VaccineSchedule.Rows.Count > 0) ? dsImmunization.VaccineSchedule.Rows[0][dsImmunization.VaccineSchedule.RecordCountColumn.ColumnName] : 0,
                    ScheduleSetupLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.VaccineSchedule.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Active/Inactive Immunization ScheduleSetup
        public string ActiveInActiveScheduleSetup(ImmunizationModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.VaccineScheduleId) > 0)
                {

                    DSImmunization dsDSImmunization = new DSImmunization();
                    BLObject<DSImmunization> obj = BLLClinicalObj.LoadImmunizationScheduleSetup(MDVUtility.ToStr(model.ScheduleTypeId), MDVUtility.ToInt64(model.VaccineScheduleId), "", "", "");
                    dsDSImmunization = obj.Data;
                    foreach (DSImmunization.VaccineScheduleRow dr in dsDSImmunization.Tables[dsDSImmunization.VaccineSchedule.TableName].Rows)
                    {
                        dr.IsActive = MDVUtility.ToStr(model.IsActive) == "1" ? true : false;
                        /* dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                         dr.ModifiedOn = DateTime.Now;*/
                    }

                    #region Database Updation
                    if (dsDSImmunization.Tables[dsDSImmunization.VaccineSchedule.TableName].Rows.Count > 0)
                    {
                        BLObject<DSImmunization> objUpdate = BLLClinicalObj.UpdateImmunizationScheduleSetup(dsDSImmunization);
                        if (objUpdate.Data != null)
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
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
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
                        message = "Schedule Setup not found."
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
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Delete Immunization ScheduleSetup
        public string DeleteScheduleSetup(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.VaccineScheduleId)))
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

                    BLObject<string> obj = BLLClinicalObj.DeleteImmunizationScheduleSetup(MDVUtility.ToStr(model.VaccineScheduleId));
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
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Save Immunization ScheduleSetup
        public string SaveImmunizationScheduleSetup(ImmunizationModel model)
        {
            try
            {
                DSImmunization dsImm = new DSImmunization();
                DSImmunization.VaccineScheduleRow dr = dsImm.VaccineSchedule.NewVaccineScheduleRow();
                bool active = model.IsActive == "True" ? true : false;
                /*bool isdefault = model.IsDefault == "True" ? true : false;*/
                dr.VaccineGroupID = MDVUtility.ToInt32(model.VaccineGroupID);
                dr.ScheduleId = MDVUtility.ToInt64(model.ScheduleId);
                dr.ScheduleTypeId = MDVUtility.ToInt32(model.ScheduleTypeId);
                if (!string.IsNullOrEmpty(model.PatientId))
                    dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                else
                    dr[dsImm.VaccineSchedule.PatientIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.StartDueDate))
                    dr.StartDueDate = model.StartDueDate;
                else
                    dr[dsImm.VaccineSchedule.StartDueDateColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.EndOverDueDate))
                    dr.EndOverDueDate = model.EndOverDueDate;
                else
                    dr[dsImm.VaccineSchedule.EndOverDueDateColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.Maxage))
                    dr.MaleMaxAge = model.Maxage;
                else
                    dr[dsImm.VaccineSchedule.MaleMaxAgeColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.Maxage))
                    dr.FemaleMaxAge = model.Maxage;
                else
                    dr[dsImm.VaccineSchedule.FemaleMaxAgeColumn] = DBNull.Value;
                dr.IsActive = active;

                /*dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;*/

                #region Database Insertion
                dsImm.VaccineSchedule.AddVaccineScheduleRow(dr);
                BLObject<DSImmunization> obj = BLLClinicalObj.InsertImmunizationScheduleSetup(dsImm);
                dsImm = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        ScheduleSetupId = dsImm.Tables[dsImm.VaccineSchedule.TableName].Rows[0][dsImm.VaccineSchedule.VaccineScheduleIdColumn.ColumnName]
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to update Immunization ScheduleSetup
        public string UpdateImmunizationScheduleSetup(ImmunizationModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.VaccineScheduleId) > 0)
                {

                    DSImmunization dsImm = new DSImmunization();
                    BLObject<DSImmunization> obj = BLLClinicalObj.LoadImmunizationScheduleSetup(MDVUtility.ToStr(model.ScheduleTypeId), MDVUtility.ToInt64(model.VaccineScheduleId), "", "", "");
                    dsImm = obj.Data;
                    foreach (DSImmunization.VaccineScheduleRow dr in dsImm.Tables[dsImm.VaccineSchedule.TableName].Rows)
                    {

                        bool active = model.IsActive == "True" ? true : false;
                        if (!string.IsNullOrEmpty(model.PatientId))
                            dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                        else
                            dr[dsImm.VaccineSchedule.PatientIdColumn] = DBNull.Value;


                        if (!string.IsNullOrEmpty(model.VaccineGroupID))
                            dr.VaccineGroupID = MDVUtility.ToInt32(model.VaccineGroupID);
                        if (!string.IsNullOrEmpty(model.IsActive))
                            dr.IsActive = active;
                        if (!string.IsNullOrEmpty(model.ScheduleId))
                            dr.ScheduleId = MDVUtility.ToInt64(model.ScheduleId);
                        if (!string.IsNullOrEmpty(model.ScheduleTypeId))
                            dr.ScheduleTypeId = MDVUtility.ToInt32(model.ScheduleTypeId);
                        if (!string.IsNullOrEmpty(model.StartDueDate))
                            dr.StartDueDate = model.StartDueDate;
                        else
                            dr[dsImm.VaccineSchedule.StartDueDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.EndOverDueDate))
                            dr.EndOverDueDate = model.EndOverDueDate;
                        else
                            dr[dsImm.VaccineSchedule.EndOverDueDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.Maxage))
                            dr.MaleMaxAge = model.Maxage;
                        else
                            dr[dsImm.VaccineSchedule.MaleMaxAgeColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.Maxage))
                            dr.FemaleMaxAge = model.Maxage;
                        else
                            dr[dsImm.VaccineSchedule.FemaleMaxAgeColumn] = DBNull.Value;
                        /*  dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                          dr.ModifiedOn = DateTime.Now;*/
                    }
                    #region Database Updation
                    if (dsImm.Tables[dsImm.VaccineSchedule.TableName].Rows.Count > 0)
                    {
                        BLObject<DSImmunization> objUpdate = BLLClinicalObj.UpdateImmunizationScheduleSetup(dsImm);
                        if (objUpdate.Data != null)
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
                                Message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
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
                        Message = "Lot Number not found."
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


        public string SetPriority(ImmunizationModel model)
        {

            BLObject<DSImmunization> objImmunizationFrom;
            BLObject<DSImmunization> objImmunizationTo;

            DSImmunization dsImmunizationFrom = new DSImmunization();
            DSImmunization dsImmunizationTo = new DSImmunization();

            objImmunizationFrom = BLLClinicalObj.LoadImmunizationScheduleSetupForSort(MDVUtility.ToInt64(model.fromId));
            objImmunizationTo = BLLClinicalObj.LoadImmunizationScheduleSetupForSort(MDVUtility.ToInt64(model.toId));

            if (objImmunizationFrom.Data != null && objImmunizationTo.Data != null)
            {
                Int64 from_sortId = 0;
                Int64 to_sortId = 0;
                foreach (DSImmunization.VaccineScheduleRow dr in objImmunizationFrom.Data.Tables[dsImmunizationFrom.VaccineSchedule.TableName].Rows)
                {
                    from_sortId = dr.Priority;
                }
                foreach (DSImmunization.VaccineScheduleRow dr in objImmunizationTo.Data.Tables[dsImmunizationTo.VaccineSchedule.TableName].Rows)
                {
                    to_sortId = dr.Priority;
                    dr.Priority = from_sortId;
                }
                foreach (DSImmunization.VaccineScheduleRow dr in objImmunizationFrom.Data.Tables[dsImmunizationFrom.VaccineSchedule.TableName].Rows)
                {
                    dr.Priority = to_sortId;
                }

                dsImmunizationTo = objImmunizationTo.Data;
                dsImmunizationFrom = objImmunizationFrom.Data;

                #region Database Updation



                BLObject<DSImmunization> objto = BLLClinicalObj.UpdateImmunizationScheduleSetupForSort(dsImmunizationTo);
                BLObject<DSImmunization> objfrom = BLLClinicalObj.UpdateImmunizationScheduleSetupForSort(dsImmunizationFrom);


                if (objto.Data != null && objfrom != null)
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
                        Message = ""
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
                    Message = ""
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            //try
            //{
            //    if (MDVUtility.ToInt64(model.fromId) > 0 && MDVUtility.ToInt64(model.toId)>0)
            //    {

            //        DSImmunization dsImm = new DSImmunization();
            //        BLObject<DSImmunization> obj = BLLClinicalObj.LoadImmunizationScheduleSetup(MDVUtility.ToStr(model.ScheduleTypeId), MDVUtility.ToInt64(model.VaccineScheduleId), "", "", "");
            //        dsImm = obj.Data;
            //        foreach (DSImmunization.VaccineScheduleRow dr in dsImm.Tables[dsImm.VaccineSchedule.TableName].Rows)
            //        {
            //            bool active = model.IsActive == "True" ? true : false;
            //            if (!string.IsNullOrEmpty(model.VaccineGroupID))
            //                dr.VaccineGroupID = MDVUtility.ToInt32(model.VaccineGroupID);
            //            if (!string.IsNullOrEmpty(model.IsActive))
            //                dr.IsActive = active;
            //            if (!string.IsNullOrEmpty(model.ScheduleId))
            //                dr.ScheduleId = MDVUtility.ToInt64(model.ScheduleId);
            //            if (!string.IsNullOrEmpty(model.ScheduleTypeId))
            //                dr.ScheduleTypeId = MDVUtility.ToInt32(model.ScheduleTypeId);
            //            if (!string.IsNullOrEmpty(model.StartDueDate))
            //                dr.StartDueDate = model.StartDueDate;
            //            else
            //                dr[dsImm.VaccineSchedule.StartDueDateColumn] = DBNull.Value;
            //            if (!string.IsNullOrEmpty(model.EndOverDueDate))
            //                dr.EndOverDueDate = model.EndOverDueDate;
            //            else
            //                dr[dsImm.VaccineSchedule.EndOverDueDateColumn] = DBNull.Value;
            //            if (!string.IsNullOrEmpty(model.Maxage))
            //                dr.MaleMaxAge = model.Maxage;
            //            else
            //                dr[dsImm.VaccineSchedule.MaleMaxAgeColumn] = DBNull.Value;
            //            if (!string.IsNullOrEmpty(model.Maxage))
            //                dr.FemaleMaxAge = model.Maxage;
            //            else
            //                dr[dsImm.VaccineSchedule.FemaleMaxAgeColumn] = DBNull.Value;
            //            /*  dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            //              dr.ModifiedOn = DateTime.Now;*/
            //        }
            //        #region Database Updation
            //        if (dsImm.Tables[dsImm.VaccineSchedule.TableName].Rows.Count > 0)
            //        {
            //            BLObject<DSImmunization> objUpdate = BLLClinicalObj.UpdateImmunizationScheduleSetup(dsImm);
            //            if (objUpdate.Data != null)
            //            {
            //                var response = new
            //                {
            //                    status = true,
            //                    message = Common.AppPrivileges.Update_Message
            //                };
            //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //            }
            //            else
            //            {
            //                var response = new
            //                {
            //                    status = false,
            //                    message = objUpdate.Message
            //                };
            //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            //            }
            //        }
            //        else
            //        {
            //            var response = new
            //            {
            //                status = false,
            //                message = ""
            //            };
            //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            //        }
            //        #endregion
            //    }
            //    else
            //    {
            //        var response = new
            //        {
            //            status = false,
            //            message = "Lot Number not found."
            //        };
            //        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    var response = new
            //    {
            //        status = false,
            //        Message =MDVCustomException.HumanReadableMessage(ex.Message),
            //    };
            //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //}
        }
        #endregion

        #region Immunization LotNumber
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Load Immunization LotNumber
        public string LoadImmunizationLotNumber(LotNumberModel model)
        {
            try
            {

                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;
                obj = BLLClinicalObj.LoadImmunizationLotNumber(MDVUtility.ToLong(model.VaccineLotNoId), MDVUtility.ToStr(model.Active), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), MDVUtility.ToLong(model.VaccineId != "" ? (model.VaccineId) : "0"), (model.Type != "" ? model.Type : ""), (model.TherapeuticInjectionId != "" ? MDVUtility.ToInt(model.TherapeuticInjectionId) : 0), (model.ProviderId != "" ? MDVUtility.ToInt64(model.ProviderId) : 0), (model.OnlyExpired == true ? true : false), (model.OnlyLowQuantity == true ? true : false));
                dsImmunization = obj.Data;
                var response = new
                {
                    status = true,
                    LotNumberCount = dsImmunization.Tables[dsImmunization.VaccineLotNo.TableName].Rows.Count,
                    iTotalDisplayRecords = (dsImmunization.VaccineLotNo.Rows.Count > 0) ? dsImmunization.VaccineLotNo.Rows[0][dsImmunization.VaccineLotNo.RecordCountColumn.ColumnName] : 0,
                    LotNumberLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.VaccineLotNo.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Active/Inactive Immunization LotNumber
        public string ActiveInActiveLotNumber(LotNumberModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.VaccineLotNoId) > 0)
                {

                    DSImmunization dsDSImmunization = new DSImmunization();
                    BLObject<DSImmunization> obj = BLLClinicalObj.LoadImmunizationLotNumber(MDVUtility.ToInt64(model.VaccineLotNoId), "", 1, 100);
                    dsDSImmunization = obj.Data;
                    foreach (DSImmunization.VaccineLotNoRow dr in dsDSImmunization.Tables[dsDSImmunization.VaccineLotNo.TableName].Rows)
                    {
                        dr.Active = MDVUtility.ToStr(model.Active) == "1" ? true : false;
                        /* dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                         dr.ModifiedOn = DateTime.Now;*/
                    }

                    #region Database Updation
                    if (dsDSImmunization.Tables[dsDSImmunization.VaccineLotNo.TableName].Rows.Count > 0)
                    {
                        BLObject<DSImmunization> objUpdate = BLLClinicalObj.UpdateImmunizationLotNumber(dsDSImmunization);
                        if (objUpdate.Data != null)
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
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
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
                        message = "Lot Number not found."
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
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Delete Immunization LotNumber
        public string DeleteLotNumber(LotNumberModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.VaccineLotNoId)))
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

                    BLObject<string> obj = BLLClinicalObj.DeleteImmunizationLotNumber(MDVUtility.ToStr(model.VaccineLotNoId));
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
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to Save Immunization LotNumber
        public string SaveImmunizationLotNumber(LotNumberModel model)
        {
            try
            {
                DSImmunization dsImm = new DSImmunization();
                DSImmunization.VaccineLotNoRow dr = dsImm.VaccineLotNo.NewVaccineLotNoRow();
                bool active = model.Active == "True" ? true : false;
                /*bool isdefault = model.IsDefault == "True" ? true : false;*/
                dr.LotNo = MDVUtility.ToStr(model.LotNo);
                //if (!string.IsNullOrEmpty(model.VaccineId))
                //{
                //    dr.VaccineId = MDVUtility.ToInt64(model.VaccineId);
                //}
                //else
                //{
                //    dr[dsImm.VaccineLotNo.VaccineIdColumn] = DBNull.Value;
                //}
                dr.LotVaccineIds = model.LotVaccineIds;
                dr.EntityId = MDVUtility.ToInt64(model.EntityId);
                dr.ProviderIds = model.ProviderIds;
                dr.ManufacturerId = MDVUtility.ToInt64(model.VacManufacturerId);
                if (!string.IsNullOrEmpty(model.ExpiryDate))
                    dr.ExpiryDate = MDVUtility.ToDateTime(model.ExpiryDate);
                if (!string.IsNullOrEmpty(model.Quantity))
                    dr.Quantity = MDVUtility.ToInt32(model.Quantity);
                if (!string.IsNullOrEmpty(model.QuantityLeft))
                    dr.QuantityLeft = MDVUtility.ToInt32(model.QuantityLeft);
                if (!string.IsNullOrEmpty(model.RouteId))
                {
                    dr.RouteId = MDVUtility.ToInt64(model.RouteId);
                }
                else
                {
                    dr[dsImm.VaccineLotNo.RouteIdColumn] = DBNull.Value;
                }
                dr.Active = active;
                dr.NDCCode = MDVUtility.ToStr(model.NDCCode);
                if (!string.IsNullOrEmpty(model.VISDate))
                    dr.VISDate = MDVUtility.ToDateTime(model.VISDate);

                if (!string.IsNullOrEmpty(model.TherapeuticInjectionId))
                {
                    dr.TherapeuticInjectionId = MDVUtility.ToInt(model.TherapeuticInjectionId);
                }
                else
                {
                    dr[dsImm.VaccineLotNo.TherapeuticInjectionIdColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.VaccineFundingSourceId))
                    dr.VaccineFundingSourceId = MDVUtility.ToInt64(model.VaccineFundingSourceId);
                else
                    dr[dsImm.VaccineLotNo.VaccineFundingSourceIdColumn] = DBNull.Value;

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsImm.VaccineLotNo.AddVaccineLotNoRow(dr);
                BLObject<DSImmunization> obj = BLLClinicalObj.InsertImmunizationLotNumber(dsImm);
                dsImm = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        LotNumberId = dsImm.Tables[dsImm.VaccineLotNo.TableName].Rows[0][dsImm.VaccineLotNo.VaccineLotNoIdColumn.ColumnName]
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        ///  Author : Azeem Raza Tayyab
        ///  Date : 20 july 2016
        ///  Purpose : Function to update Immunization LotNumber
        public string UpdateImmunizationLotNumber(LotNumberModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.VaccineLotNoId) > 0)
                {

                    DSImmunization dsImm = new DSImmunization();
                    BLObject<DSImmunization> obj = BLLClinicalObj.LoadImmunizationLotNumber(MDVUtility.ToInt64(model.VaccineLotNoId), "", 1, 100);
                    dsImm = obj.Data;
                    foreach (DSImmunization.VaccineLotNoRow dr in dsImm.Tables[dsImm.VaccineLotNo.TableName].Rows)
                    {
                        bool active = model.Active == "True" ? true : false;
                        if (!string.IsNullOrEmpty(model.Active))
                            dr.Active = active;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(model.LotNo))
                            dr.LotNo = MDVUtility.ToStr(model.LotNo);
                        //if (!string.IsNullOrEmpty(model.VaccineId))
                        //{
                        //    dr.VaccineId = MDVUtility.ToInt64(model.VaccineId);
                        //}
                        //else
                        //{
                        //    dr[dsImm.VaccineLotNo.VaccineIdColumn] = DBNull.Value;
                        //}
                        dr.LotVaccineIds = model.LotVaccineIds;
                        dr.EntityId = MDVUtility.ToInt64(model.EntityId);
                        dr.ProviderIds = model.ProviderIds;
                        if (!string.IsNullOrEmpty(model.VacManufacturerId))
                            dr.ManufacturerId = MDVUtility.ToInt64(model.VacManufacturerId);
                        if (!string.IsNullOrEmpty(model.ExpiryDate))
                            dr.ExpiryDate = MDVUtility.ToDateTime(model.ExpiryDate);
                        if (!string.IsNullOrEmpty(model.Quantity))
                            dr.Quantity = MDVUtility.ToInt32(model.Quantity);
                        if (!string.IsNullOrEmpty(model.QuantityLeft))
                            dr.QuantityLeft = MDVUtility.ToInt32(model.QuantityLeft);
                        if (!string.IsNullOrEmpty(model.RouteId))
                        {
                            dr.RouteId = MDVUtility.ToInt64(model.RouteId);
                        }
                        else
                        {
                            dr[dsImm.VaccineLotNo.RouteIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.TherapeuticInjectionId))
                        {
                            dr.TherapeuticInjectionId = MDVUtility.ToInt(model.TherapeuticInjectionId);
                        }
                        else
                        {
                            dr[dsImm.VaccineLotNo.TherapeuticInjectionIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.VaccineFundingSourceId))
                            dr.VaccineFundingSourceId = MDVUtility.ToInt64(model.VaccineFundingSourceId);
                        else
                            dr[dsImm.VaccineLotNo.VaccineFundingSourceIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.NDCCode))
                            dr.NDCCode = MDVUtility.ToStr(model.NDCCode);
                        if (!string.IsNullOrEmpty(model.VISDate))
                            dr.VISDate = MDVUtility.ToDateTime(model.VISDate);

                    }
                    #region Database Updation
                    if (dsImm.Tables[dsImm.VaccineLotNo.TableName].Rows.Count > 0)
                    {
                        BLObject<DSImmunization> objUpdate = BLLClinicalObj.UpdateImmunizationLotNumber(dsImm);
                        if (objUpdate.Data != null)
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
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
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
                        message = "Lot Number not found."
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

        public string LoadImmunizationLotNumberByVaccineLotNoId(LotNumberModel model)
        {
            try
            {

                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;
                obj = BLLClinicalObj.LoadImmunizationLotNumber(MDVUtility.ToLong(model.VaccineLotNoId), MDVUtility.ToStr(model.Active), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), 0);
                dsImmunization = obj.Data;
                List<LotNumberModel> ViewLotNumberModel = new List<LotNumberModel>();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                if (dsImmunization.Tables[dsImmunization.VaccineLotNo.TableName].Rows.Count > 0)
                {
                    foreach (DSImmunization.VaccineLotNoRow row in dsImmunization.Tables[dsImmunization.VaccineLotNo.TableName].Rows)
                    {
                        LotNumberModel lotNumberModel = new LotNumberModel();
                        lotNumberModel.VaccineLotNoId = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.VaccineLotNoIdColumn]);
                        lotNumberModel.LotNo = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.LotNoColumn]);
                        lotNumberModel.VaccineId = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.VaccineIdColumn]);
                        lotNumberModel.VacManufacturerId = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.ManufacturerIdColumn]);
                        lotNumberModel.ExpiryDate = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.ExpiryDateColumn]) == "" ? "" : MDVUtility.ToDateTime(row[dsImmunization.VaccineLotNo.ExpiryDateColumn]).ToShortDateString();
                        lotNumberModel.Quantity = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.QuantityColumn]);
                        lotNumberModel.QuantityLeft = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.QuantityLeftColumn]);
                        lotNumberModel.RouteId = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.RouteIdColumn]);
                        lotNumberModel.Active = Convert.ToBoolean(row[dsImmunization.VaccineLotNo.ActiveColumn]) == true ? "1" : "0"; MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.ActiveColumn]);
                        lotNumberModel.NDCCode = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.NDCCodeColumn]);
                        lotNumberModel.VISDate = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.VISDateColumn]) == "" ? "" : MDVUtility.ToDateTime(row[dsImmunization.VaccineLotNo.VISDateColumn]).ToShortDateString();
                        lotNumberModel.VaccineName = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.VaccineNameColumn]);
                        lotNumberModel.HTMLURL = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.HTMLURLColumn]);
                        lotNumberModel.TherapeuticInjectionId = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.TherapeuticInjectionIdColumn]);
                        lotNumberModel.EntityId = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.EntityIdColumn]);
                        lotNumberModel.ProviderIds = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.ProviderIdsColumn]);
                        lotNumberModel.ManufacturerName = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.ManufacturerNameColumn]);
                        lotNumberModel.LotVaccineIds = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.LotVaccineIdsColumn]);
                        lotNumberModel.VaccineFundingSourceId = MDVUtility.ToStr(row[dsImmunization.VaccineLotNo.VaccineFundingSourceIdColumn]);
                        ViewLotNumberModel.Add(lotNumberModel);
                    }
                }
                var response = new
                {
                    status = true,
                    LotNumberCount = dsImmunization.Tables[dsImmunization.VaccineLotNo.TableName].Rows.Count,
                    LotNumberLoad_JSON = js.Serialize(ViewLotNumberModel),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        /// Author: M Ahmad Imran
        /// Purpose:  to load Immunization Lookup for Clinical Reports
        /// Date : August 30, 2016
        internal string getAllImmunizationLookupforReports()
        {
            try
            {

                List<LookupModel> modelList = null;
                BLObject<List<LookupModel>> obj;

                obj = BLLClinicalObj.getAllImmunizationLookupforReports();

                modelList = obj.Data;

                var CategoryList = modelList.Where(y => y.LookUpType == "Category").ToList();
                var RouteList = modelList.Where(y => y.LookUpType == "Route").ToList();
                var SiteList = modelList.Where(y => y.LookUpType == "Site").ToList();
                var ReactionList = modelList.Where(y => y.LookUpType == "Reaction").ToList();
                var AlertsList = modelList.Where(y => y.LookUpType == "Alert").ToList();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var response = new
                {
                    status = true,
                    CategoryList = js.Serialize(CategoryList),
                    RouteList = js.Serialize(RouteList),
                    SiteList = js.Serialize(SiteList),
                    ReactionList = js.Serialize(ReactionList),
                    AlertsList = js.Serialize(AlertsList),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        internal string GetVaccineDropdownForReports(ImmunizationModel model)
        {
            try
            {

                List<LookupModel> modelList = null;
                BLObject<List<LookupModel>> obj;

                obj = BLLClinicalObj.GetVaccineDropdownForReports(MDVUtility.ToLong(model.CategoryID));

                modelList = obj.Data;

                //var CategoryList = modelList.Where(y => y.LookUpType == "Vaccine").ToList();

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var response = new
                {
                    status = true,
                    VaccineList = js.Serialize(modelList),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        #region Immunization With Data Model

        public string getLatestVaccineByPatientId(Int64 PatientId, Int64 UserId)
        {
            try
            {
                Int64 EntityID = MDVUtility.ToLong(MDVSession.Current.EntityId);
                List<MDVision.Model.Clinical.Notes.VaccineSoapModel> vaccines = new List<MDVision.Model.Clinical.Notes.VaccineSoapModel>();
                List<MDVision.Model.Clinical.Notes.TherapeuticInjectionSoapModel> injections = new List<MDVision.Model.Clinical.Notes.TherapeuticInjectionSoapModel>();
                vaccines = BLLClinicalObj.LoadVaccine("", PatientId, UserId, EntityID);
                injections = BLLClinicalObj.LoadImmTherapeuticInjectionForSoapText("", PatientId, UserId, EntityID);
                var response = new
                {
                    status = true,
                    VaccineCount = vaccines.Count,
                    Vaccines = vaccines,
                    TheraeuticInjectionCount = injections.Count,
                    Injections = injections
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        public string LoadVaccine(ImmunizationModel model)
        {
            try
            {
                Int64 EntityID = MDVUtility.ToLong(MDVSession.Current.EntityId);
                List<MDVision.Model.Clinical.Notes.VaccineSoapModel> vaccines = new List<MDVision.Model.Clinical.Notes.VaccineSoapModel>();
                List<MDVision.Model.Clinical.Notes.TherapeuticInjectionSoapModel> injections = new List<MDVision.Model.Clinical.Notes.TherapeuticInjectionSoapModel>();
                if (!string.IsNullOrWhiteSpace(model.VaccineHxIds))
                {
                    vaccines = BLLClinicalObj.LoadVaccine(model.VaccineHxIds, MDVUtility.ToLong(model.PatientId), MDVUtility.ToLong(model.UserId), EntityID);
                }
                if (!string.IsNullOrWhiteSpace(model.ImmTherInjectionIds))
                {
                    injections = BLLClinicalObj.LoadImmTherapeuticInjectionForSoapText(model.ImmTherInjectionIds, MDVUtility.ToLong(model.PatientId), MDVUtility.ToLong(model.UserId), EntityID);
                }
                if (vaccines.Count > 0 || injections.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        VaccineCount = vaccines.Count,
                        Vaccines = vaccines,
                        TheraeuticInjectionCount = injections.Count,
                        Injections = injections
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "No record found"
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

        public string LoadVaccineHxById(ImmunizationQueryModel model)
        {
            try
            {
                List<ImmunizationQueryModel> ImmAcknowledgeList = null;
                BLObject<List<ImmunizationQueryModel>> obj;

                obj = BLLClinicalObj.LoadVaccineHxById(model.VaccineHxId);

                if (obj.Data != null)
                {
                    ImmAcknowledgeList = obj.Data;
                    if (ImmAcknowledgeList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            iTotalDisplayRecords = ImmAcknowledgeList[0].RecordCount,
                            ImmAcknowCount = ImmAcknowledgeList.Count,
                            ImmAcknow_JSON = ImmAcknowledgeList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ImmQueryCount = 0,
                            ImmQuery_JSON = "{}"
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

        #endregion Immunization With Data Model

        #region Immunization Query

        public string SendQuery(ImmunizationQueryModel model)
        {
            try
            {
                var dateTime = DateTime.Now;
                model.RequestDateTime = MDVUtility.ToStr(dateTime);
                model.GivenBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                model.Status = "Successful";
                string QueryId = BLLClinicalObj.SendQuery(model);
                if (MDVUtility.ToInt64(QueryId) > 0)
                {
                    BLLPatient BLLPatientObj = new BLLPatient();
                    BLObject<DSPatient> dsPatient = BLLPatientObj.FillPatient_PictureById(MDVUtility.ToInt64(model.PatientId));
                    DSPatient.PatientsRow patientRow = (from p in dsPatient.Data.Patients where p.PatientId == MDVUtility.ToInt64(model.PatientId) select p).First();


                    NHapi.Model.V25.Message.ADT_A01 adt = new NHapi.Model.V25.Message.ADT_A01();

                    var SendQueryHL7 = "";
                    #region MSH
                    SendQueryHL7 += "MSH|^~\\&|NISTEHRAPP|NISTEHRFAC|NISTIISAPP|NISTIIFAC|" + dateTime.ToString("yyyyMMddHHmmss") + "-0500||QBP^Q11^QBP_Q11|MDVISION_IM_Q_" + QueryId + "|P|2.5.1|||ER|AL|||||Z44^CDCPHINVS|NISTEHRFAC^^^^^NIST-AA-1^XX^^^100-6482|NISTIISFAC^^^^^NIST-AA-1^XX^^^100-3322" + Environment.NewLine;
                    #endregion

                    #region QPD
                    var tagMrnStateIDRegistery = "";
                    if (model.MrnStateIDRegisteryId == "1")
                    {
                        tagMrnStateIDRegistery = model.MrnStateIDRegisteryData + "^^^NIST-MPI-1^MR";
                        // tagMrnStateIDRegistery=model.MrnStateIDRegisteryData+"^^^325^MR~^^^NJIIS^SR~^^^NJA^BR";
                    }
                    else if (model.MrnStateIDRegisteryId == "2")
                    {
                        //tagMrnStateIDRegistery="^^^325^MR~"+model.MrnStateIDRegisteryData+"^^^NJIIS^SR~^^^NJA^BR";
                    }
                    else if (model.MrnStateIDRegisteryId == "3")
                    {
                        //tagMrnStateIDRegistery="^^^325^MR~^^^NJIIS^SR~"+model.MrnStateIDRegisteryData+"^^^NJA^BR";
                    }

                    var Gender = "";
                    if (!patientRow.IsGenderNull())
                    {
                        if (patientRow.Gender.ToLower() == "male")
                        {
                            Gender = "M";
                        }
                        else if (patientRow.Gender.ToLower() == "female")
                        {
                            Gender = "F";
                        }
                        else
                        {
                            Gender = "U";
                        }
                    }

                    var cellPhonNoCode = "";
                    var cellPhonNo = "";
                    var primaryPhonNoCode = "";
                    var primaryPhonNo = "";

                    if (!patientRow.IsHomePhoneNoNull() && patientRow.HomePhoneNo != "")
                    {
                        primaryPhonNoCode = patientRow.HomePhoneNo.Substring(1, 3);
                        primaryPhonNo = patientRow.HomePhoneNo.Substring(5, 9).Trim().Replace("-", "");
                    }
                    if (!patientRow.IsCellNoNull() && patientRow.CellNo != "")
                    {
                        cellPhonNoCode = patientRow.CellNo.Substring(1, 3);
                        cellPhonNo = patientRow.CellNo.Substring(5, 9).Trim().Replace("-", "");
                    }

                    else
                    {
                        if (!patientRow.IsWorkPhoneNoNull() && patientRow.WorkPhoneNo != "")
                        {
                            primaryPhonNoCode = patientRow.WorkPhoneNo.Substring(1, 3);
                            primaryPhonNo = patientRow.WorkPhoneNo.Substring(5, 9).Trim().Replace("-", "");
                        }
                        else if (!patientRow.IsWorkPhoneExtNull() && patientRow.WorkPhoneExt != "")
                        {
                            primaryPhonNoCode = patientRow.WorkPhoneExt.Substring(1, 3);
                            primaryPhonNo = patientRow.WorkPhoneExt.Substring(5, 9).Trim().Replace("-", "");
                        }
                    }
                    SendQueryHL7 += "QPD|Z44^Request Evaluated History and Forecast^CDCPHINVS|MDVISION_IM_Q_" + QueryId + "|" + tagMrnStateIDRegistery + "|" + (patientRow.IsLastNameNull() ? "" : patientRow.LastName) + "^" + (patientRow.IsFirstNameNull() ? "" : patientRow.FirstName) + "^" + (patientRow.IsMINull() ? "" : patientRow.MI) + "^^^^L|" + ((patientRow.IsMotherMaidenNameNull() || patientRow.MotherMaidenName == "") ? "" : patientRow.MotherMaidenName + "^^^^^^M") + "|" + ((patientRow.IsDOBNull() || patientRow.DOB.ToString() == "") ? "" : Convert.ToDateTime(patientRow.DOB).ToString("yyyyMMdd")) + "|" + Gender + "|" + (patientRow.IsAddress1Null() ? "" : patientRow.Address1) + "^" + (patientRow.IsAddress2Null() ? "" : patientRow.Address2) + "^" + (patientRow.IsCityNull() ? "" : patientRow.City) + "^" + (patientRow.IsStateNull() ? "" : patientRow.State) + "^" + (patientRow.IsZIPCodeNull() ? "" : patientRow.ZIPCode) + "^USA^P" + "|" + (primaryPhonNoCode != "" ? "^PRN^PH^^^" + primaryPhonNoCode + "^" : "") + primaryPhonNo + "|" + (model.BirthIndicator != "" ? model.BirthIndicator : "") + "|" + ((MDVUtility.ToInt(model.BirthOrder) > 0 ? model.BirthOrder : "")) + Environment.NewLine;
                    SendQueryHL7 += "RCP|I|1^RD&Records&HL70126";
                    #endregion


                    BLObject<string> obj = BLLClinicalObj.UpdateHL7Message(MDVUtility.ToInt64(QueryId), SendQueryHL7);
                    if (obj.Data != "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = "Successfully Send",
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
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "error found"
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

        public string LoadImmQuery(ImmunizationQueryModel model)
        {
            try
            {
                List<ImmunizationQueryModel> ImmQueryList = null;
                BLObject<List<ImmunizationQueryModel>> obj;

                obj = BLLClinicalObj.LoadImmQuery(model.QueryId, model.PatientId, MDVUtility.ToInt(model.pageNumber), MDVUtility.ToInt(model.rowsPerPage));

                if (obj.Data != null)
                {
                    ImmQueryList = obj.Data;
                    if (ImmQueryList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            iTotalDisplayRecords = ImmQueryList[0].RecordCount,
                            ImmQueryCount = ImmQueryList.Count,
                            ImmQuery_JSON = ImmQueryList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ImmQueryCount = 0,
                            ImmQuery_JSON = "{}"
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

        public string LoadImmQueryResponse(ImmunizationQueryResponseModel model)
        {
            try
            {
                List<ImmunizationQueryResponseModel> ImmQueryresponseList = null;
                BLObject<List<ImmunizationQueryResponseModel>> obj;

                obj = BLLClinicalObj.LoadImmQueryResponse(model.ImmunizationQueryResponseId, model.PatientId, MDVUtility.ToInt(model.pageNumber), MDVUtility.ToInt(model.rowsPerPage));

                if (obj.Data != null)
                {
                    ImmQueryresponseList = obj.Data;
                    if (ImmQueryresponseList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            iTotalDisplayRecords = ImmQueryresponseList[0].RecordCount,
                            ImmQueryResponseCount = ImmQueryresponseList.Count,
                            ImmQueryResponse_JSON = ImmQueryresponseList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ImmQueryResponseCount = 0,
                            ImmQueryResponse_JSON = "{}"
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

        public string LoadImmQueryResponsePatient(ImmunizationQueryResponseModel model)
        {
            try
            {
                List<ImmunizationQueryResponseModel> ImmQueryresponseDetail = null;
                BLObject<List<ImmunizationQueryResponseModel>> obj;

                obj = BLLClinicalObj.LoadImmQueryResponsePatientDetail(model.ImmunizationQueryResponseId);

                if (obj.Data != null)
                {
                    ImmQueryresponseDetail = obj.Data;
                    var response = new
                    {
                        status = true,
                        ImmQueryResponsePatientCount = ImmQueryresponseDetail.Count,
                        ImmQueryResponsePatient_JSON = ImmQueryresponseDetail,
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


        public string SearchQueryResponseHX(ImmunizationQueryResponseModel model)
        {
            try
            {

                List<EvaluatedImmunizationHistoryModel> ImmQueryresponseHx = null;
                BLObject<List<EvaluatedImmunizationHistoryModel>> obj;


                obj = BLLClinicalObj.SearchQueryResponseHX(model.ImmunizationQueryResponseId, model.PatientId, MDVUtility.ToInt(model.pageNumber), MDVUtility.ToInt(model.rowsPerPage));

                if (obj.Data != null)
                {
                    ImmQueryresponseHx = obj.Data;

                    var response = new
                    {
                        status = true,
                        ImmQueryResponseHXCount = ImmQueryresponseHx.Count,
                        ImmQueryResponseHX_JSON = ImmQueryresponseHx
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

        public string SearchQueryResponseForecast(ImmunizationQueryResponseModel model)
        {
            try
            {

                List<ImmunizationForecastModel> ImmQueryresponseForecast = null;
                BLObject<List<ImmunizationForecastModel>> obj;


                obj = BLLClinicalObj.SearchQueryResponseForecast(model.ImmunizationQueryResponseId, MDVUtility.ToInt(model.pageNumber), MDVUtility.ToInt(model.rowsPerPage));

                if (obj.Data != null)
                {
                    ImmQueryresponseForecast = obj.Data;

                    var response = new
                    {
                        status = true,
                        ImmQueryResponseForecastCount = ImmQueryresponseForecast.Count,
                        ImmQueryResponseForecast_JSON = ImmQueryresponseForecast
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


        public string SaveImmResponseFile(ImmunizationQueryResponseModel model)
        {
            try
            {
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                model.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                model.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

                model.File = MDVUtility.ReplaceSpecialCharacters(MDVUtility.ToStr(model.File));
                string path = "HL7_Jsons\\ImmResponse.json";
                string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + path);/*File.ReadAllText("Files/default.json");*/
                List<EvaluatedImmunizationHistoryModel> EvaluatedImmunizationHistoryList = new List<EvaluatedImmunizationHistoryModel>();
                List<ImmunizationForecastModel> ImmunizationForecastList = new List<ImmunizationForecastModel>();
                //List<ADT> adt = HL7Converter.DeserializeObject<ADT>(hl7, json);
                string Hl7 = model.File;
                string[] lines = Hl7.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
                );

                List<string> subMessages = new List<string>();
                string data = "";
                string patientInfo = "";
                bool isORC = false;
                foreach (string line in lines)
                {
                    if (line.StartsWith("ORC"))
                    {
                        if (data.Length > 0)
                        {
                            subMessages.Add(data);
                            data = "";
                        }
                        isORC = true;
                        data += line + Environment.NewLine;
                    }
                    else if (isORC && (line.StartsWith("RXA") || line.StartsWith("OBX")))
                    {
                        data += line + Environment.NewLine;
                    }
                    else
                    {
                        patientInfo += line + Environment.NewLine;
                    }
                }
                if (data.Length > 0)
                {
                    subMessages.Add(data);
                }

                List<ImmunizationHL7ResponseModel> Response = HL7Converter.DeserializeObject<ImmunizationHL7ResponseModel>(patientInfo, json);

                if (Response[0].MSH.MessageCode.ToLower().Trim() == "rsp" && Response[0].MSH.TriggerEvent.ToLower().Trim() == "k11" && Response[0].MSH.MessageStructure.ToLower().Trim() == "rsp_k11")
                {

                    if (Response[0].QAK.QueryResponseStatus.ToLower().Trim() == "ok" && Response[0].MSA.AcknowledgmentCode.ToLower().Trim() == "aa")
                    {
                        model.ResponseType = "Single (Exact) Match Found";
                        model.PatientName = Response[0].PID.LastName + ", " + Response[0].PID.FirstName;
                        model.DOB = Response[0].PID.DateTimeOfBirth;
                        model.Gender = Response[0].PID.AdministrativeSex.ToLower() == "m" ? "Male" : (Response[0].PID.AdministrativeSex.ToLower() == "f" ? "Female" : "Unknown");
                        model.MothersMaidenName = Response[0].PID.MotherMaidenName;
                        model.Address = Response[0].PID.Address1;
                        //BLLClinicalObj.SaveQueryResponse(model);

                        foreach (string ImmHistory in subMessages)
                        {
                            List<ImmunizationHL7ResponseRXA_OBXModel> ResponseRXA_OBX = HL7Converter.DeserializeObject<ImmunizationHL7ResponseRXA_OBXModel>(patientInfo + ImmHistory, json);
                            EvaluatedImmunizationHistoryModel EvImmHisModel = new EvaluatedImmunizationHistoryModel();
                            EvImmHisModel.AdministrationDate = ResponseRXA_OBX[0].RXA.AdministrationDate;
                            EvImmHisModel.VaccineDescription = ResponseRXA_OBX[0].RXA.VaccineDescription;
                            EvImmHisModel.VaccineCVX = ResponseRXA_OBX[0].RXA.Code;
                            EvImmHisModel.CompletionStatus = ResponseRXA_OBX[0].RXA.CompletionStatus == "CP" ? "Complete" : "";
                            EvImmHisModel.Dose = ResponseRXA_OBX[0].RXA.Dose;
                            if (EvImmHisModel.VaccineDescription.ToLower().Trim() == "no vaccine admin" && EvImmHisModel.VaccineCVX == "998")
                            {
                                for (int j = 0; j < ResponseRXA_OBX[0].OBX.Count; j++)
                                {
                                    if (ResponseRXA_OBX[0].OBX[j].Question.ToLower().Trim() == "vaccine type" && ResponseRXA_OBX[0].OBX[j].QuestionCode.ToLower().Trim() == "30956-7")
                                    {
                                        ImmunizationForecastModel ImmunizationForecast = new ImmunizationForecastModel();
                                        ImmunizationForecast.VaccineGroupCVX = ResponseRXA_OBX[0].OBX[j].AnswerCode;
                                        ImmunizationForecast.VaccineGroupDescription = ResponseRXA_OBX[0].OBX[j].Answer;
                                        j++;
                                        for (int k = j; k < ResponseRXA_OBX[0].OBX.Count; k++)
                                        {
                                            if (ResponseRXA_OBX[0].OBX[j].Question.ToLower().Trim() == "date vaccination due" && ResponseRXA_OBX[0].OBX[j].QuestionCode.ToLower().Trim() == "30980-7")
                                            {
                                                ImmunizationForecast.DueDate = ResponseRXA_OBX[0].OBX[j].AnswerCode;
                                            }
                                            else if (ResponseRXA_OBX[0].OBX[j].Question.ToLower().Trim() == "earliest date to give" && ResponseRXA_OBX[0].OBX[j].QuestionCode.ToLower().Trim() == "30981-5")
                                            {
                                                ImmunizationForecast.EarliestDateToGive = ResponseRXA_OBX[0].OBX[j].AnswerCode;
                                            }
                                            else if (ResponseRXA_OBX[0].OBX[j].Question.ToLower().Trim() == "vaccine type" && ResponseRXA_OBX[0].OBX[j].QuestionCode.ToLower().Trim() == "30956-7")
                                            {
                                                j--;
                                                break;
                                            }
                                            j++;
                                        }
                                        ImmunizationForecastList.Add(ImmunizationForecast);
                                    }
                                }
                            }
                            else
                            {
                                var vaccineCount = 0;
                                foreach (MDVision.Model.Clinical.HL7.OBX obx in ResponseRXA_OBX[0].OBX)
                                {
                                    if (obx.Question.ToLower().Trim() == "vaccine type" && obx.QuestionCode.ToLower().Trim() == "30956-7")
                                    {
                                        vaccineCount++;
                                        if (vaccineCount > 1)
                                        {
                                            EvaluatedImmunizationHistoryList.Add(EvImmHisModel);
                                            EvImmHisModel = new EvaluatedImmunizationHistoryModel();
                                            EvImmHisModel.AdministrationDate = ResponseRXA_OBX[0].RXA.AdministrationDate;
                                            EvImmHisModel.VaccineDescription = ResponseRXA_OBX[0].RXA.VaccineDescription;
                                            EvImmHisModel.VaccineCVX = ResponseRXA_OBX[0].RXA.Code;
                                            EvImmHisModel.CompletionStatus = ResponseRXA_OBX[0].RXA.CompletionStatus == "CP" ? "Complete" : "";
                                            EvImmHisModel.Dose = ResponseRXA_OBX[0].RXA.Dose;
                                        }
                                        EvImmHisModel.VaccineGroupDescription = obx.Answer;
                                        EvImmHisModel.VaccineGroupCVX = obx.AnswerCode;
                                    }
                                    else if (obx.Question.ToLower().Trim() == "dose validity" && obx.QuestionCode.ToLower().Trim() == "59781-5")
                                    {
                                        EvImmHisModel.ValidDose = obx.AnswerCode == "Y" ? "YES" : "NO";
                                    }
                                    else if (obx.Question.ToLower().Trim() == "reason applied" && obx.QuestionCode.ToLower().Trim() == "30982-3")
                                    {
                                        EvImmHisModel.ValidityReason = obx.AnswerCode;
                                    }
                                }
                                if (EvImmHisModel != null)
                                {
                                    EvaluatedImmunizationHistoryList.Add(EvImmHisModel);
                                }
                            }
                        }
                        model.XML = MDVUtility.GetXmlOfObject(typeof(List<EvaluatedImmunizationHistoryModel>), EvaluatedImmunizationHistoryList) + MDVUtility.GetXmlOfObject(typeof(List<ImmunizationForecastModel>), ImmunizationForecastList);
                        string ResponseId = BLLClinicalObj.SaveResponse(model);

                        if (MDVUtility.ToInt64(ResponseId) > 0)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "error Found"
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        if ((Response[0].QAK.QueryResponseStatus.ToLower().Trim() == "nf" || Response[0].QAK.QueryResponseStatus.ToLower().Trim() == "tm") && Response[0].MSA.AcknowledgmentCode.ToLower().Trim() == "aa")
                        {
                            if (Response[0].QAK.QueryResponseStatus.ToLower().Trim() == "nf")
                            {
                                model.ResponseType = "No Patients Found";
                            }
                            else
                            {
                                model.ResponseType = "Multiple Patients Found";
                            }
                            string ResponseId = BLLClinicalObj.SaveResponse(model);
                            if (MDVUtility.ToInt64(ResponseId) > 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    Message = Common.AppPrivileges.Save_Message,
                                };
                                return (JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = "error Found"
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }
                        else if (Response[0].QAK.QueryResponseStatus.ToLower().Trim() == "ae" && Response[0].MSA.AcknowledgmentCode.ToLower().Trim() == "ae")
                        {
                            var response = new
                            {
                                status = false,
                                Message = "non-fatal errors in the QBP message"
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "Error In File reading"
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "This Is not Immunization Response File"
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



        public string DeleteImmunizationQueryResponse(ImmunizationQueryResponseModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ImmunizationQueryResponseId)))
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

                    BLObject<string> obj = BLLClinicalObj.DeleteImmQueryResponse(model.ImmunizationQueryResponseId);
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
        public string DeleteImmunizationQuery(ImmunizationQueryModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.QueryId)))
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

                    BLObject<string> obj = BLLClinicalObj.DeleteImmQuery(model.QueryId);
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

        public string AddToHxTab(ImmunizationQueryResponseModel model)
        {
            try
            {
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                model.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                model.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                model.UserId = MDVUtility.ToStr(MDVSession.Current.AppUserId);
                string VaccineHxId = BLLClinicalObj.AddToHxTab(model);
                if (VaccineHxId != "")
                {
                    var response = new
                    {
                        status = true,
                        Message = "Successfully Added",
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
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

        public string SaveImmAcknowledgementFile(ImmunizationQueryResponseModel model)
        {
            try
            {
                model.File = MDVUtility.ReplaceSpecialCharacters(MDVUtility.ToStr(model.File));
                string path = "HL7_Jsons\\ImmResponse.json";
                string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + path);/*File.ReadAllText("Files/default.json");*/
                List<EvaluatedImmunizationHistoryModel> EvaluatedImmunizationHistoryList = new List<EvaluatedImmunizationHistoryModel>();
                List<ImmunizationForecastModel> ImmunizationForecastList = new List<ImmunizationForecastModel>();
                //List<ADT> adt = HL7Converter.DeserializeObject<ADT>(hl7, json);
                string Hl7 = model.File;




                List<ImmunizationHL7ResponseModel> Response = HL7Converter.DeserializeObject<ImmunizationHL7ResponseModel>(Hl7, json);

                if (Response[0].MSH.MessageCode.ToLower().Trim() == "ack" && Response[0].MSH.TriggerEvent.ToLower().Trim() == "v04" && Response[0].MSH.MessageStructure.ToLower().Trim() == "ack")
                {
                    var AcknowledgmentCode = Response[0].MSA.AcknowledgmentCode.ToLower().Trim();
                    if (AcknowledgmentCode == "aa" || AcknowledgmentCode == "ae" || AcknowledgmentCode == "ar")
                    {

                        model.AcknowledgementCode = AcknowledgmentCode;
                        string AcknowledgmentId = BLLClinicalObj.SaveAcknowledgement(model);

                        if (MDVUtility.ToInt64(AcknowledgmentId) > 0)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "error Found"
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "error Found Save Acknowledgement"
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "error Found Save Acknowledgement"
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


        #region treatment
        public string GetVaccineScheduleId(ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.MainAgeGroup)) || string.IsNullOrEmpty(MDVUtility.ToStr(model.MainSchedule)) || string.IsNullOrEmpty(MDVUtility.ToStr(model.MainCategory)))
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

                    BLObject<string> obj = BLLClinicalObj.GetVaccineScheduleId(model.MainAgeGroup, model.MainSchedule, model.MainCategory);
                    if (obj.Data != "")
                    {
                        var response = new
                        {
                            status = true,
                            VaccineScheduleId = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Exception in GetVaccineHxIds"
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
