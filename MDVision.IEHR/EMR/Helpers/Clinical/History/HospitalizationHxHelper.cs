// Author:  Muhammad Arshad
// Created Date: 14/01/2016
//OverView: Helper class for HospitalizationHx
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using MDVision.IEHR.EMR.Model.History;
using System.Text;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical.History
{
    public class HospitalizationHxHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public HospitalizationHxHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static HospitalizationHxHelper _instance = null;
        public static HospitalizationHxHelper Instance()
        {
            if (_instance == null)
                _instance = new HospitalizationHxHelper();
            return _instance;
        }

        #region fillHospitalizationHx

        // Author:  Abid Ali
        // Created Date: 21/01/2016
        //OverView: This function will handle fill of HospitalizationHx
        public string fillHospitalizationHx(HospitalizationHxModel model, Int64 hospitalizationHxId, string hospitalizationHxType)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)) && hospitalizationHxId == 0)
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
                    DSHospitalizationHx dsHospitalizationHx = null;
                    BLObject<DSHospitalizationHx> obj = BLLClinicalObj.LoadHospitalizationHx(MDVUtility.ToInt64(model.PatientId), hospitalizationHxId, MDVUtility.ToInt64(model.DiseaseId), hospitalizationHxType, "1", "");
                    dsHospitalizationHx = obj.Data;
                    if (dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows[0];
                        var HospitalizationHxkeyValues = new Dictionary<string, string>
                        {
                            { "HospitalizationHxDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxDateColumn.ColumnName]).ToShortDateString()},
                            { "HospitalizationHxId",  MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxIdColumn.ColumnName])},
                            { "HospitalizationHxUnremarkable", MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.bUnremarkableColumn.ColumnName])},
                            { "HospitalizationHxComments", MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.CommentsColumn.ColumnName])},
                            { "HospitalizationHxSoapText", MDVUtility.ToStr(dr[dsHospitalizationHx.HospitalizationHx.SoapTextColumn.ColumnName])}
                        };

                        List<Dictionary<string, string>> lstDisease = new List<Dictionary<string, string>>();
                        var DiseaseHxkeyValues = new Dictionary<string, string> { { "", "" } };

                        //start Farooq Ahmad 21/01/2015 if model has disease detail then make the dictionary diseasehxkeyvalues for json
                        if (MDVUtility.ToInt64(model.DiseaseId) > 0)
                        {

                            DSHospitalizationHx.HospitalizationHx_DiseaseRow[] arrToComponentRows = (DSHospitalizationHx.HospitalizationHx_DiseaseRow[])dsHospitalizationHx.HospitalizationHx_Disease.Select(MDVUtility.ToStr(dsHospitalizationHx.HospitalizationHx_Disease.DiseaseIdColumn.ColumnName) + "=" + MDVUtility.ToStr(model.DiseaseId) + "");
                            DataRow drDisease = (DataRow)arrToComponentRows[0];

                            DiseaseHxkeyValues = new Dictionary<string, string>
                            {
                                { "HospitalizationDiseaseStayDuration",  MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.StayDurationColumn.ColumnName])},
                                { "HospitalizationDiseaseStayId",  MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.StayIdColumn.ColumnName])},
                                 { "HospitalizationDiseaseStatus",  MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.StatusIdColumn.ColumnName])},
                                { "HospitalizationDiseaseAdmissionDate",  String.IsNullOrEmpty(MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.AdmissionDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.AdmissionDateColumn.ColumnName]).ToShortDateString() },
                                { "HospitalizationDiseaseDischargeDate",  String.IsNullOrEmpty(MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.DischargeDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.DischargeDateColumn.ColumnName]).ToShortDateString() },
                                { "HospitalizationDiseaseHospital",String.IsNullOrEmpty(MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.HospitalColumn.ColumnName]))?"": MDVUtility.ToStr(arrToComponentRows[0][dsHospitalizationHx.HospitalizationHx_Disease.HospitalColumn.ColumnName]) },
                                { "HospitalizationDiseaseComments", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CommentsColumn.ColumnName])},
                                { "CPT",MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTCodeColumn.ColumnName]) !=""? (MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTCodeColumn.ColumnName])+" - "+MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTCodeDescriptionColumn.ColumnName])):""},
                                { "CPTCodeId", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTCodeColumn.ColumnName])},
                                { "CPTCode", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTCodeColumn.ColumnName])},
                                { "CPTDescription", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTCodeDescriptionColumn.ColumnName])},

                               { "CPTSNOMEDCodeId", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTSNOMEDIDColumn.ColumnName])},
                                { "CPTSNOMEDDescription", MDVUtility.ToStr(drDisease[dsHospitalizationHx.HospitalizationHx_Disease.CPTSNOMEDDescriptionColumn.ColumnName])}

                            };

                        }
                        //End Farooq Ahmad 21/01/2015 if model has disease detail then make the dictionary diseasehxkeyvalues for json

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                        var SoapText = string.Empty;
                        var IsCreatedOrModified = string.Empty;
                        var LastUpdated = string.Empty;
                        hospitalizationHxId = MDVUtility.ToInt64(dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxIdColumn.ColumnName]);
                        var SoapInfo = getCurrentSoapText(hospitalizationHxId);

                        if (SoapInfo != null)
                        {
                            SoapText = SoapInfo["SoapText"];
                            IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                            LastUpdated = SoapInfo["LastUpdated"];
                        }

                        var response = new
                        {
                            status = true,
                            HospitalizationHxFill_JSON = js.Serialize(HospitalizationHxkeyValues),
                            // HospitalizationHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx_Disease.TableName]),
                            HospitalizationHxLoad_JSON = MDVUtility.JSON_DataTable(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName]),
                            //start Farooq Ahmad 21/01/2015  diseasehx key values and hospitalization hx disease load to json
                            DiseaseFill_JSON = js.Serialize(DiseaseHxkeyValues),
                            HospitalizationHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx_Disease.TableName]),
                            //end Farooq Ahmad 21/01/2015  diseasehx key values and hospitalization hx disease load to json
                            SoapText = SoapText,
                            IsCreatedOrModified = IsCreatedOrModified,
                            LastUpdated = LastUpdated,
                            HospitalizationHxId = hospitalizationHxId

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            HospitalizationHxFill_JSON = "[]",
                            // HospitalizationHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx_Disease.TableName]),
                            HospitalizationHxLoad_JSON = MDVUtility.JSON_DataTable(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName]),
                            //start Farooq Ahmad 21/01/2015  diseasehx key values and hospitalization hx disease load to json
                            DiseaseFill_JSON = "[]",
                            HospitalizationHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx_Disease.TableName]),
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


        #endregion

        #region saveHospitalizationHx

        // Author:  Abid Ali
        // Created Date: 20/01/2016
        //OverView: This function will handle saving of HospitalizationHx
        public string saveHospitalizationHx(HospitalizationHxModel model, string fieldsJSON, List<object> lstDiseaseObject)
        {
            try
            {
                DSHospitalizationHx dsHospitalizationHx = new DSHospitalizationHx();

                DSHospitalizationHx.HospitalizationHxRow dr = dsHospitalizationHx.HospitalizationHx.NewHospitalizationHxRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.HospitalizationHxDate))
                {
                    dr.HospitalizationHxDate = MDVUtility.ToDateTime(model.HospitalizationHxDate);
                }
                else
                {
                    dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.HospitalizationHxComments))
                {
                    dr.Comments = MDVUtility.ToStr(model.HospitalizationHxComments);
                }
                else
                {
                    dr[dsHospitalizationHx.HospitalizationHx.CommentsColumn] = DBNull.Value;
                }

                dr.bUnremarkable = model.HospitalizationHxUnremarkable.ToLower() == "true" ? true : false;

                dr.IsActive = true;

                if (model.AddedFromMobileApp == "1")
                {
                    dr.CreatedBy = model.CreatedBy;
                    dr.CreatedOn = MDVUtility.ToDateTime(model.CreatedOn);
                    dr.ModifiedOn = MDVUtility.ToDateTime(model.ModifiedOn);
                    dr.ModifiedBy = model.ModifiedBy;
                }
                else
                {
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Insertion
                dsHospitalizationHx.HospitalizationHx.AddHospitalizationHxRow(dr);
                BLObject<DSHospitalizationHx> obj = BLLClinicalObj.InsertHospitalizationHx(dsHospitalizationHx);
                dsHospitalizationHx = obj.Data;

                if (obj.Data != null)
                {
                    var diseaseID = "";
                    Int64 HospitalizationHxId = MDVUtility.ToInt64(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows[0][dsHospitalizationHx.HospitalizationHx.HospitalizationHxIdColumn.ColumnName]);
                    //start Farooq Ahmad 22/01/2016 if hospitalization id is greater then zero then insert and update the disease data
                    if (HospitalizationHxId > 0)
                    {
                        if (lstDiseaseObject.Count > 0)
                        {
                            string responseHospitalizationHxDisease = insertUpdateDisease(HospitalizationHxId, lstDiseaseObject, model.PatientId);
                            diseaseID = responseHospitalizationHxDisease;
                        }
                    }
                    //end Farooq Ahmad 01/03/2016 if hospitalization id is greater then zero then insert and update the disease data

                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForHospitalizationHx(HospitalizationHxId);

                    var SoapText = string.Empty;
                    var IsCreatedOrModified = string.Empty;
                    var LastUpdated = string.Empty;

                    var SoapInfo = getCurrentSoapText(HospitalizationHxId);

                    if (SoapInfo != null)
                    {
                        SoapText = SoapInfo["SoapText"];
                        IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                        LastUpdated = SoapInfo["LastUpdated"];
                    }

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        HospitalizationHxId = MDVUtility.ToInt64(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows[0][dsHospitalizationHx.HospitalizationHx.HospitalizationHxIdColumn.ColumnName]),
                        diseaseId = diseaseID,
                        SoapText = SoapText,
                        IsCreatedOrModified = IsCreatedOrModified,
                        LastUpdated = LastUpdated
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


        public Dictionary<string, string> getCurrentSoapText(Int64 hospitalizationHxId)
        {
            try
            {
                DSHistorySummary dsHistorySummarySoap = null;
                BLObject<DSHistorySummary> objSummary;
                objSummary = BLLClinicalObj.loadHxLog(hospitalizationHxId, "HospitalizationHx", "Current", 1, 10);
                dsHistorySummarySoap = objSummary.Data;

                var SoapText = string.Empty;
                var IsCreatedOrModified = string.Empty;
                var LastUpdated = string.Empty;

                if (dsHistorySummarySoap != null && dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows.Count > 0)
                {
                    var Hxdr = dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows[0];
                    SoapText = MDVUtility.ToStr(Hxdr[dsHistorySummarySoap.HxLog.SoapTextColumn.ColumnName]);
                    IsCreatedOrModified = MDVUtility.ToStr(Hxdr[dsHistorySummarySoap.HxLog.ActionColumn.ColumnName]);
                    LastUpdated = string.Concat(MDVUtility.ToStr(Hxdr[dsHistorySummarySoap.HxLog.ModifiedOnColumn.ColumnName]), " ", MDVUtility.ToStr(Hxdr[dsHistorySummarySoap.HxLog.ModifiedByColumn.ColumnName]));
                }

                var response = new Dictionary<string, string>
                {
                   {"SoapText", SoapText},
                   {"IsCreatedOrModified" , IsCreatedOrModified},
                   {"LastUpdated" , LastUpdated},
                   {"HospitalizationHxId" ,MDVUtility.ToStr( hospitalizationHxId)}
                };
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region updateHospitalizationHx

        // Author:  Abid Ali
        // Created Date: 20/01/2016
        //OverView: This function will handle update of HospitalizationHx
        public string updateHospitalizationHx(HospitalizationHxModel model, Int64 hospitalizationHxId, List<object> lstDiseaseObject)
        {
            try
            {
                if (hospitalizationHxId > 0)
                {

                    DSHospitalizationHx dsHospitalizationHx = new DSHospitalizationHx();
                    BLObject<DSHospitalizationHx> obj = BLLClinicalObj.LoadHospitalizationHx(MDVUtility.ToInt64(model.PatientId), hospitalizationHxId);
                    dsHospitalizationHx = obj.Data;
                    foreach (DSHospitalizationHx.HospitalizationHxRow dr in dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows)
                    {
                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                        if (!string.IsNullOrEmpty(model.HospitalizationHxDate))
                        {
                            dr.HospitalizationHxDate = MDVUtility.ToDateTime(model.HospitalizationHxDate);
                        }
                        else
                        {
                            dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.HospitalizationHxComments))
                        {
                            dr.Comments = MDVUtility.ToStr(model.HospitalizationHxComments);
                        }
                        else
                        {
                            dr[dsHospitalizationHx.HospitalizationHx.CommentsColumn] = DBNull.Value;
                        }

                        dr.bUnremarkable = model.HospitalizationHxUnremarkable.ToLower() == "true" ? true : false;

                        if (model.AddedFromMobileApp == "1")
                        {
                           
                            dr.ModifiedOn = MDVUtility.ToDateTime(model.ModifiedOn);
                            dr.ModifiedBy = model.ModifiedBy;
                        }
                        else
                        {
                           
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }

                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }
                    var diseaseID = "";
                    //start Farooq Ahmad 22/01/2016 if hospitalization id is greater then zero then insert and update the disease data
                    if (lstDiseaseObject.Count > 0)
                    {
                        string responseMedicalHxDisease = insertUpdateDisease(hospitalizationHxId, lstDiseaseObject, model.PatientId);
                        diseaseID = responseMedicalHxDisease;
                    }
                    //end Farooq Ahmad 22/01/2016 if hospitalization id is greater then zero then insert and update the disease data


                    //Start 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                    //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MedicalHxId);

                    //End 08/01/2016 Muhammad Arshad Code to  Save/Update HospitalizationHx SoapText for MiscHx Tab
                    #region Database Updation
                    if (dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows.Count > 0)
                    {
                        BLObject<DSHospitalizationHx> objUpdate = BLLClinicalObj.UpdateHospitalizationHx(dsHospitalizationHx);
                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForHospitalizationHx(hospitalizationHxId);
                        if (objUpdate.Data != null)
                        {
                            var SoapText = string.Empty;
                            var IsCreatedOrModified = string.Empty;
                            var LastUpdated = string.Empty;

                            var SoapInfo = getCurrentSoapText(hospitalizationHxId);

                            if (SoapInfo != null)
                            {
                                SoapText = SoapInfo["SoapText"];
                                IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                                LastUpdated = SoapInfo["LastUpdated"];
                            }

                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,
                                diseaseId = diseaseID,
                                SoapText = SoapText,
                                IsCreatedOrModified = IsCreatedOrModified,
                                LastUpdated = LastUpdated,
                                HospitalizationHxId = hospitalizationHxId
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
                        message = "Hospitalization Hx not found."
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

        #region 'Attachment/Detachment of Hospitalization History with Progress note'

        // Author:  Farooq Ahmad
        // Created Date: 22/01/2016
        // OverView: This function will detach hospitalizationhx from notes
        internal string detachHospitalizationHxFromNotes(long hospitalizationHxId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(hospitalizationHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachHospitalizationHxFromNotes(hospitalizationHxId, notesId);
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

        // Author:  Farooq Ahmad
        // Created Date: 22/01/2016
        // OverView: This function will attach hospitalizationhx from notes
        internal string attachHospitalizationHxWithNotes(string hospitalizationHxId, long notesId)
        {
            try
            {
                DSHospitalizationHx dHospitalizationHx = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(hospitalizationHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<DSHospitalizationHx> obj = BLLClinicalObj.attachHospitalizationHxWithNotes(hospitalizationHxId, notesId);
                    if (obj.Data != null)
                    {
                        dHospitalizationHx = obj.Data;
                        var response = new
                        {
                            status = true,
                            HospitalizationHxTotalCount = dHospitalizationHx.Tables[dHospitalizationHx.HospitalizationHx.TableName].Rows.Count,
                            HospitalizationHxCount = dHospitalizationHx.Tables[dHospitalizationHx.HospitalizationHx.TableName].Rows.Count,
                            HospitalizationHxLoad_JSON = MDVUtility.JSON_DataTable(dHospitalizationHx.Tables[dHospitalizationHx.HospitalizationHx.TableName]),

                            Message = Common.AppPrivileges.Update_Message
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

        #region"Disease Functions"

        // Author:  Farooq Ahmad
        // Created Date: 21/01/2016
        //OverView: This function will Save/Update Diseases in HospitalizationHx 
        private string insertUpdateDisease(long hospitalizationHxId, List<object> lstDiseaseObject, string patientId)
        {
            #region Disease
            DSHospitalizationHx dsDisease = new DSHospitalizationHx();
            List<HospitalizationHxDiseaseModel> lstDisease = lstDiseaseObject.OfType<HospitalizationHxDiseaseModel>().ToList();
            bool isFirstChild = false;
            foreach (HospitalizationHxDiseaseModel CurrentModel in lstDisease)
            {
                if (CurrentModel.DiseaseId != null)
                {
                    Int32 currentDiseaseId = MDVUtility.ToInt32(CurrentModel.DiseaseId);
                    if (CurrentModel.AddedFromMobileApp == null)
                         currentDiseaseId = currentDiseaseId <= 0 ? -1 : currentDiseaseId;
                    BLObject<DSHospitalizationHx> objTobacco = BLLClinicalObj.loadHospitalizationHxDisease(hospitalizationHxId, currentDiseaseId);
                    dsDisease = objTobacco.Data;
                    DSHospitalizationHx.HospitalizationHx_DiseaseRow RowDisease = null;
                    if (dsDisease.HospitalizationHx_Disease.Rows.Count > 0)
                    {
                        RowDisease = (DSHospitalizationHx.HospitalizationHx_DiseaseRow)dsDisease.HospitalizationHx_Disease.Rows[0];
                    }
                    else
                    {
                        RowDisease = dsDisease.HospitalizationHx_Disease.NewHospitalizationHx_DiseaseRow();
                    }

                    if (RowDisease != null)
                    {
                        bool isValueDifferent = false;
                        bool istoUpdateRow = false;
                        if (dsDisease.HospitalizationHx_Disease.Rows.Count < 1)
                        {
                            RowDisease.DiseaseId = currentDiseaseId;
                        }
                        RowDisease.HospitalizationHxId = hospitalizationHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.FreeTextICD))
                        {
                            RowDisease.FreeTextICD = MDVUtility.ToStr(CurrentModel.FreeTextICD);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.FreeTextICDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD9Code))
                        {
                            RowDisease.ICD9Code = MDVUtility.ToStr(CurrentModel.ICD9Code);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.ICD9CodeColumn] = DBNull.Value;
                        }



                        if (!string.IsNullOrEmpty(CurrentModel.ICD9CodeDescription))
                        {
                            RowDisease.ICD9CodeDescription = MDVUtility.ToStr(CurrentModel.ICD9CodeDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.ICD9CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10Code))
                        {
                            RowDisease.ICD10Code = MDVUtility.ToStr(CurrentModel.ICD10Code);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.ICD10CodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10CodeDescription))
                        {
                            RowDisease.ICD10CodeDescription = MDVUtility.ToStr(CurrentModel.ICD10CodeDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.ICD10CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDID))
                        {
                            RowDisease.SNOMEDID = MDVUtility.ToStr(CurrentModel.SNOMEDID);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.SNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDDescription))
                        {
                            RowDisease.SNOMEDDescription = MDVUtility.ToStr(CurrentModel.SNOMEDDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.SNOMEDDescriptionColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDID))
                        {
                            RowDisease.CPTSNOMEDID = MDVUtility.ToStr(CurrentModel.CPTSNOMEDID);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.CPTSNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDDescription))
                        {
                            RowDisease.CPTSNOMEDDescription = MDVUtility.ToStr(CurrentModel.CPTSNOMEDDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.CPTSNOMEDDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCode))
                        {
                            RowDisease.LexiCode = MDVUtility.ToStr(CurrentModel.LexiCode);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.LexiCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCodeDescription))
                        {
                            RowDisease.LexiCodeDescription = MDVUtility.ToStr(CurrentModel.LexiCodeDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.LexiCodeDescriptionColumn] = DBNull.Value;
                        }



                        if (string.IsNullOrEmpty(CurrentModel.CPTCode))
                        {

                            RowDisease[dsDisease.HospitalizationHx_Disease.CPTCodeColumn] = DBNull.Value;

                            RowDisease[dsDisease.HospitalizationHx_Disease.CPTCodeDescriptionColumn] = DBNull.Value;

                        }
                        if (!string.IsNullOrEmpty(CurrentModel.CPTCodeId) || CurrentModel.AddedFromMobileApp == "1")
                        {
                            if(CurrentModel.AddedFromMobileApp != "1")
                            {
                                RowDisease.CPTCode = MDVUtility.ToStr(CurrentModel.CPTCodeId);
                            }
                            else
                            {
                                RowDisease.CPTCode = MDVUtility.ToStr(CurrentModel.CPTCode);
                                RowDisease.CPTCodeDescription = MDVUtility.ToStr(CurrentModel.CPTCode);
                            }
                            
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.CPTCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTDescription))
                        {
                            RowDisease.CPTCodeDescription = MDVUtility.ToStr(CurrentModel.CPTDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.CPTCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseStatus))
                        {
                            RowDisease.StatusId = MDVUtility.ToInt16(CurrentModel.HospitalizationDiseaseStatus);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.StatusIdColumn] = DBNull.Value;
                        }
                        ////

                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseStayDuration))
                        {
                            RowDisease.StayDuration = MDVUtility.ToInt16(CurrentModel.HospitalizationDiseaseStayDuration);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.StayDurationColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseStayId))
                        {
                            RowDisease.StayId = MDVUtility.ToInt16(CurrentModel.HospitalizationDiseaseStayId);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.StayIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseHospital))
                        {
                            RowDisease.Hospital = CurrentModel.HospitalizationDiseaseHospital;
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.HospitalColumn] = DBNull.Value;
                        }
                        ////
                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseAdmissionDate))
                        {
                            RowDisease.AdmissionDate = MDVUtility.ToDateTime(CurrentModel.HospitalizationDiseaseAdmissionDate);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.AdmissionDateColumn] = DBNull.Value;
                        }
                        //////

                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseDischargeDate))
                        {
                            RowDisease.DischargeDate = MDVUtility.ToDateTime(CurrentModel.HospitalizationDiseaseDischargeDate);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.DischargeDateColumn] = DBNull.Value;
                        }
                        ///////



                        ////////


                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseStatus))
                        {
                            RowDisease.StatusId = MDVUtility.ToInt16(CurrentModel.HospitalizationDiseaseStatus);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.StatusIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseComments))
                        {
                            RowDisease.Comments = MDVUtility.ToStr(CurrentModel.HospitalizationDiseaseComments);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.CommentsColumn] = DBNull.Value;
                        }

                        RowDisease.IsActive = true;
                        RowDisease.PatientId = MDVUtility.ToLong(patientId);
                        if (CurrentModel.AddedFromMobileApp == "1")
                        {
                            RowDisease.CreatedBy =  CurrentModel.CreatedBy;
                            RowDisease.CreatedOn = MDVUtility.ToDateTime(CurrentModel.CreatedOn);
                            RowDisease.ModifiedBy = CurrentModel.ModifiedBy;
                            RowDisease.ModifiedOn = MDVUtility.ToDateTime( CurrentModel.ModifiedOn);
                            RowDisease.ICDID = MDVUtility.ToInt64( CurrentModel.ICDID);
                            if( !string.IsNullOrEmpty(CurrentModel.CPTID))
                            RowDisease.CPTID = MDVUtility.ToInt64(CurrentModel.CPTID);

                            RowDisease.AddedFromMobileApp = CurrentModel.AddedFromMobileApp;
                        }
                        else
                        {
                            RowDisease.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowDisease.CreatedOn = DateTime.Now;
                            RowDisease.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowDisease.ModifiedOn = DateTime.Now;
                        }
                        // if no Tobacco is found against TobaccoId, it implies for new record
                        if (dsDisease.HospitalizationHx_Disease.Rows.Count < 1)
                        {
                            dsDisease.HospitalizationHx_Disease.AddHospitalizationHx_DiseaseRow(RowDisease);
                        }
                    }
                }
            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowDisease in dsDisease.HospitalizationHx_Disease.Rows)
            {
                RowDisease[dsDisease.HospitalizationHx_Disease.SoapTextColumn] = insertUpdateHospitalizationSoapText(dsDisease, lstDisease[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSHospitalizationHx> objInsertedDisease = BLLClinicalObj.insertUpdateHospitalizationHxDisease(dsDisease);
            if (objInsertedDisease.Data != null)
            {

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    diseaseId = dsDisease.Tables[dsDisease.HospitalizationHx_Disease.TableName].Rows.Count > 0 ? dsDisease.Tables[dsDisease.HospitalizationHx_Disease.TableName].Rows[0][dsDisease.HospitalizationHx_Disease.DiseaseIdColumn.ColumnName] : 0,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedDisease.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }
       

        // Author:  Farooq Ahmad
        // Created Date: 21/01/2016
        //OverView: This function will Save/Update Hospitalization SoapText
        private object insertUpdateHospitalizationSoapText(DSHospitalizationHx dsDisease, HospitalizationHxDiseaseModel modelObj)
        {
            string SoapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            switch (modelObj.HospitalizationDiseaseStayText)
            {
                case "Days":
                    modelObj.HospitalizationDiseaseStayText = "Day(s)";
                    break;
                case "Months":
                    modelObj.HospitalizationDiseaseStayText = "Month(s)";
                    break;
                case "Years":
                    modelObj.HospitalizationDiseaseStayText = "Year(s)";
                    break;
                case "Weeks":
                    modelObj.HospitalizationDiseaseStayText = "Week(s)";
                    break;
            }


            if (modelObj != null)
            {
                sb.Append("<div id='hospitalizationHistory_" + modelObj.HospitalizationHxId + "' title='Hospitalization'  name='Hospitalization Hx'><strong>Hospitalization: </strong>");
                sb.Append(!string.IsNullOrEmpty(modelObj.HospitalizationDiseaseHospital) ? string.Format("Patient was admitted to {0}", modelObj.HospitalizationDiseaseHospital) : "Patient was admitted to a health care delivery facility");
                sb.Append(!string.IsNullOrEmpty(modelObj.HospitalizationDiseaseAdmissionDate) ? ", on " + modelObj.HospitalizationDiseaseAdmissionDate : "");
                sb.Append(string.IsNullOrEmpty(modelObj.CPTDescription) ? "" : string.Format(", For {0}", modelObj.CPTDescription));
                sb.Append(string.Format(", based on the following assessment: <strong>{0}</strong>", string.IsNullOrEmpty(modelObj.ICD9CodeDescription) ? (string.IsNullOrEmpty(modelObj.FreeTextICD) ? "" : modelObj.FreeTextICD) : modelObj.ICD9CodeDescription));
                sb.Append(!string.IsNullOrEmpty(modelObj.HospitalizationDiseaseStatus) ? string.Format(", which was {0}", modelObj.HospitalizationDiseaseStatusText) : "");
                sb.Append((string.IsNullOrEmpty(modelObj.HospitalizationDiseaseStayDuration)
                    ? "" : string.Format(", Patient was in hospital for  {0} {1}", modelObj.HospitalizationDiseaseStayDuration, modelObj.HospitalizationDiseaseStayText == "- Select -" ? "" : modelObj.HospitalizationDiseaseStayText)
                    ));
                sb.Append(!string.IsNullOrEmpty(modelObj.HospitalizationDiseaseDischargeDate) ? string.Format(", and was discharged on {0}", modelObj.HospitalizationDiseaseDischargeDate) : "");
                sb.Append((string.IsNullOrEmpty(modelObj.HospitalizationDiseaseComments) ? "" : string.Format(", {0}", " Comments: " + modelObj.HospitalizationDiseaseComments)));

                sb.Append("</div>");                
            }

            return sb.ToString();
        }

        // Author:  Farooq Ahmad
        // Created Date: 21/01/2016
        //OverView: This function will delete Diseases in HospitalizationHx
        public string deleteHospitalizationHxDisease(string diseaseId, string hospitalizationHxId, string patientId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(diseaseId)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteHospitalizationHxDisease(MDVUtility.ToStr(diseaseId), patientId);
                    if (obj.Data == "")
                    {
                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForHospitalizationHx(MDVUtility.ToInt64(hospitalizationHxId));
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

        public string saveHospitalizationHx(HospitalizationHxModel model, long NoteId)
        {
            try
            {
                DSHospitalizationHx dsHospitalizationHx = new DSHospitalizationHx();
                DSHospitalizationHx.HospitalizationHxRow dr;

                if (!string.IsNullOrEmpty(model.HospitalizationHxId) && MDVUtility.ToInt64(model.HospitalizationHxId) > 0)
                {
                    BLObject<DSHospitalizationHx> obj1 = BLLClinicalObj.LoadHospitalizationHx(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.HospitalizationHxId));
                    dsHospitalizationHx = obj1.Data;
                    dr = dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows[0] as DSHospitalizationHx.HospitalizationHxRow;
                }
                else
                {
                    dr = dsHospitalizationHx.HospitalizationHx.NewHospitalizationHxRow();
                }

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.HospitalizationHxDate))
                {
                    dr.HospitalizationHxDate = MDVUtility.ToDateTime(model.HospitalizationHxDate);
                }
                else
                {
                    dr[dsHospitalizationHx.HospitalizationHx.HospitalizationHxDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.HospitalizationHxComments))
                {
                    dr.Comments = MDVUtility.ToStr(model.HospitalizationHxComments);
                }
                else
                {
                    dr[dsHospitalizationHx.HospitalizationHx.CommentsColumn] = DBNull.Value;
                }

                dr.bUnremarkable = model.HospitalizationHxUnremarkable.ToLower() == "true" ? true : false;

                if (NoteId > 0)
                {
                    dr.NotesId = NoteId;
                }
                else
                {
                    dr[dsHospitalizationHx.HospitalizationHx.NotesIdColumn] = DBNull.Value;
                }
                dr.IsActive = true;
                if (MDVUtility.ToInt64(model.HospitalizationHxId) <= 0)
                {
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                }

                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion



                BLObject<DSHospitalizationHx> objdata = new BLObject<DSHospitalizationHx>();
                if (!string.IsNullOrEmpty(model.HospitalizationHxId) && MDVUtility.ToInt64(model.HospitalizationHxId) > 0)
                {
                    objdata = BLLClinicalObj.UpdateHospitalizationHx(dsHospitalizationHx);
                }
                else
                {
                    dsHospitalizationHx.HospitalizationHx.AddHospitalizationHxRow(dr);
                    objdata = BLLClinicalObj.InsertHospitalizationHx(dsHospitalizationHx);
                }

                dsHospitalizationHx = objdata.Data;

                if (objdata.Data != null)
                {
                    var diseaseID = "";
                    Int64 HospitalizationHxId = MDVUtility.ToInt64(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows[0][dsHospitalizationHx.HospitalizationHx.HospitalizationHxIdColumn.ColumnName]);
                    //start Farooq Ahmad 22/01/2016 if hospitalization id is greater then zero then insert and update the disease data
                    if (HospitalizationHxId > 0)
                    {
                        if (model.HospitalizationDiseaseList.Count > 0)
                        {
                            string responseHospitalizationHxDisease = insertUpdateDisease(HospitalizationHxId, model.HospitalizationDiseaseList, model.PatientId);
                            diseaseID = responseHospitalizationHxDisease;
                        }
                    }
                    //end Farooq Ahmad 01/03/2016 if hospitalization id is greater then zero then insert and update the disease data

                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForHospitalizationHx(HospitalizationHxId);

                    var SoapText = string.Empty;
                    var IsCreatedOrModified = string.Empty;
                    var LastUpdated = string.Empty;

                    var SoapInfo = getCurrentSoapText(HospitalizationHxId);

                    if (SoapInfo != null)
                    {
                        SoapText = SoapInfo["SoapText"];
                        IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                        LastUpdated = SoapInfo["LastUpdated"];
                    }

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        HospitalizationHxId = MDVUtility.ToInt64(dsHospitalizationHx.Tables[dsHospitalizationHx.HospitalizationHx.TableName].Rows[0][dsHospitalizationHx.HospitalizationHx.HospitalizationHxIdColumn.ColumnName]),
                        diseaseId = diseaseID,
                        SoapText = SoapText,
                        IsCreatedOrModified = IsCreatedOrModified,
                        LastUpdated = LastUpdated
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objdata.Message
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

        private string insertUpdateDisease(long hospitalizationHxId, List<HospitalizationHxDiseaseModel> lstDisease, string patientId)
        {
            #region Disease
            DSHospitalizationHx dsDisease = new DSHospitalizationHx();
            BLObject<DSHospitalizationHx> objTobacco = BLLClinicalObj.loadHospitalizationHxDisease(hospitalizationHxId, 0);
            dsDisease = objTobacco.Data;
            bool isFirstChild = false;
            foreach (HospitalizationHxDiseaseModel CurrentModel in lstDisease)
            {
                if (CurrentModel.DiseaseId != null)
                {
                    Int32 currentDiseaseId = MDVUtility.ToInt32(CurrentModel.DiseaseId);
                    currentDiseaseId = currentDiseaseId == 0 ? -1 : currentDiseaseId;
                    DSHospitalizationHx.HospitalizationHx_DiseaseRow RowDisease = null;
                    DSHospitalizationHx.HospitalizationHx_DiseaseRow[] arrDiseases = (DSHospitalizationHx.HospitalizationHx_DiseaseRow[])dsDisease.HospitalizationHx_Disease.Select(dsDisease.HospitalizationHx_Disease.DiseaseIdColumn.ColumnName + "=" + currentDiseaseId);


                    if (arrDiseases != null && arrDiseases.Length > 0)
                    {
                        RowDisease = arrDiseases[0];
                    }
                    else
                    {
                        RowDisease = dsDisease.HospitalizationHx_Disease.NewHospitalizationHx_DiseaseRow();
                    }

                    if (RowDisease != null)
                    {
                        bool isValueDifferent = false;
                        bool istoUpdateRow = false;
                        if (dsDisease.HospitalizationHx_Disease.Rows.Count < 1)
                        {
                            RowDisease.DiseaseId = currentDiseaseId;
                        }
                        RowDisease.HospitalizationHxId = hospitalizationHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.FreeTextICD))
                        {
                            RowDisease.FreeTextICD = MDVUtility.ToStr(CurrentModel.FreeTextICD);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.FreeTextICDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD9Code))
                        {
                            RowDisease.ICD9Code = MDVUtility.ToStr(CurrentModel.ICD9Code);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.ICD9CodeColumn] = DBNull.Value;
                        }



                        if (!string.IsNullOrEmpty(CurrentModel.ICD9CodeDescription))
                        {
                            RowDisease.ICD9CodeDescription = MDVUtility.ToStr(CurrentModel.ICD9CodeDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.ICD9CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10Code))
                        {
                            RowDisease.ICD10Code = MDVUtility.ToStr(CurrentModel.ICD10Code);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.ICD10CodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10CodeDescription))
                        {
                            RowDisease.ICD10CodeDescription = MDVUtility.ToStr(CurrentModel.ICD10CodeDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.ICD10CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDID))
                        {
                            RowDisease.SNOMEDID = MDVUtility.ToStr(CurrentModel.SNOMEDID);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.SNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDDescription))
                        {
                            RowDisease.SNOMEDDescription = MDVUtility.ToStr(CurrentModel.SNOMEDDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.SNOMEDDescriptionColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDID))
                        {
                            RowDisease.CPTSNOMEDID = MDVUtility.ToStr(CurrentModel.CPTSNOMEDID);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.CPTSNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDDescription))
                        {
                            RowDisease.CPTSNOMEDDescription = MDVUtility.ToStr(CurrentModel.CPTSNOMEDDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.CPTSNOMEDDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCode))
                        {
                            RowDisease.LexiCode = MDVUtility.ToStr(CurrentModel.LexiCode);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.LexiCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCodeDescription))
                        {
                            RowDisease.LexiCodeDescription = MDVUtility.ToStr(CurrentModel.LexiCodeDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.LexiCodeDescriptionColumn] = DBNull.Value;
                        }



                        if (string.IsNullOrEmpty(CurrentModel.CPTCode))
                        {

                            RowDisease[dsDisease.HospitalizationHx_Disease.CPTCodeColumn] = DBNull.Value;

                            RowDisease[dsDisease.HospitalizationHx_Disease.CPTCodeDescriptionColumn] = DBNull.Value;

                        }
                        if (!string.IsNullOrEmpty(CurrentModel.CPTCodeId))
                        {
                            RowDisease.CPTCode = MDVUtility.ToStr(CurrentModel.CPTCodeId);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.CPTCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTDescription))
                        {
                            RowDisease.CPTCodeDescription = MDVUtility.ToStr(CurrentModel.CPTDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.CPTCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseStatus))
                        {
                            RowDisease.StatusId = MDVUtility.ToInt16(CurrentModel.HospitalizationDiseaseStatus);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.StatusIdColumn] = DBNull.Value;
                        }
                        ////

                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseStayDuration))
                        {
                            RowDisease.StayDuration = MDVUtility.ToInt16(CurrentModel.HospitalizationDiseaseStayDuration);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.StayDurationColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseStayId))
                        {
                            RowDisease.StayId = MDVUtility.ToInt16(CurrentModel.HospitalizationDiseaseStayId);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.StayIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseHospital))
                        {
                            RowDisease.Hospital = CurrentModel.HospitalizationDiseaseHospital;
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.HospitalColumn] = DBNull.Value;
                        }
                        ////
                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseAdmissionDate))
                        {
                            RowDisease.AdmissionDate = MDVUtility.ToDateTime(CurrentModel.HospitalizationDiseaseAdmissionDate);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.AdmissionDateColumn] = DBNull.Value;
                        }
                        //////

                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseDischargeDate))
                        {
                            RowDisease.DischargeDate = MDVUtility.ToDateTime(CurrentModel.HospitalizationDiseaseDischargeDate);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.DischargeDateColumn] = DBNull.Value;
                        }
                        ///////



                        ////////


                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseStatus))
                        {
                            RowDisease.StatusId = MDVUtility.ToInt16(CurrentModel.HospitalizationDiseaseStatus);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.StatusIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.HospitalizationDiseaseComments))
                        {
                            RowDisease.Comments = MDVUtility.ToStr(CurrentModel.HospitalizationDiseaseComments);
                        }
                        else
                        {
                            RowDisease[dsDisease.HospitalizationHx_Disease.CommentsColumn] = DBNull.Value;
                        }

                        RowDisease.IsActive = true;
                        RowDisease.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowDisease.CreatedOn = DateTime.Now;
                        RowDisease.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowDisease.ModifiedOn = DateTime.Now;
                        RowDisease.PatientId = MDVUtility.ToLong(patientId);
                        RowDisease[dsDisease.HospitalizationHx_Disease.SoapTextColumn] = insertUpdateHospitalizationSoapText(dsDisease, CurrentModel);

                        // if no Tobacco is found against TobaccoId, it implies for new record
                        if (arrDiseases.Length < 1)
                        {
                            dsDisease.HospitalizationHx_Disease.AddHospitalizationHx_DiseaseRow(RowDisease);
                        }
                    }
                }
            }
            //int counter = 0;
            //// Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            //foreach (DataRow RowDisease in dsDisease.HospitalizationHx_Disease.Rows)
            //{
            //    RowDisease[dsDisease.HospitalizationHx_Disease.SoapTextColumn] = insertUpdateHospitalizationSoapText(dsDisease, lstDisease[counter]);
            //    counter++;
            //}
            #region Database Insertion/Updation

            BLObject<DSHospitalizationHx> objInsertedDisease = BLLClinicalObj.insertUpdateHospitalizationHxDisease(dsDisease);
            if (objInsertedDisease.Data != null)
            {

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    diseaseId = dsDisease.Tables[dsDisease.HospitalizationHx_Disease.TableName].Rows.Count > 0 ? dsDisease.Tables[dsDisease.HospitalizationHx_Disease.TableName].Rows[0][dsDisease.HospitalizationHx_Disease.DiseaseIdColumn.ColumnName] : 0,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedDisease.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }
       

    }
}