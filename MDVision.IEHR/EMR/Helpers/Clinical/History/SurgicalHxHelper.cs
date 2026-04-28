// Author:  Muhammad Arshad
// Created Date: 14/01/2016
//OverView: Helper class for SurgicalHx
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
    public class SurgicalHxHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public SurgicalHxHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static SurgicalHxHelper _instance = null;
        public static SurgicalHxHelper Instance()
        {
            if (_instance == null)
                _instance = new SurgicalHxHelper();
            return _instance;
        }

        #region fillSurgicalHx


        /// <summary>
        /// Author:  Muhammad Arshad
        /// Created Date: 14/01/2016
        /// OverView: This function will handle fill of SurgicalHx
        /// </summary>
        /// <param name="model"></param>
        /// <param name="surgicalHxId"></param>
        /// <param name="diseaseId"></param>
        /// <returns>string</returns>
        public string fillSurgicalHx(SurgicalHxModel model, Int64 surgicalHxId, Int64 diseaseId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)) && surgicalHxId == 0)
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

                    /* start 21/01/2016 Syed Zia, load surgical Hx*/
                    DSSurgicalHx dsSurgicalHx = null;
                    BLObject<DSSurgicalHx> obj = BLLClinicalObj.LoadSurgicalHx(MDVUtility.ToInt64(model.PatientId), surgicalHxId, diseaseId, "1", "");
                    dsSurgicalHx = obj.Data;
                    if (dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows[0];

                        var SoapText = string.Empty;
                        var IsCreatedOrModified = string.Empty;
                        var LastUpdated = string.Empty;


                        var SoapInfo = getCurrentSoapText(MDVUtility.ToInt64(dr[dsSurgicalHx.SurgicalHx.SurgicalHxIdColumn.ColumnName]));

                        if (SoapInfo != null)
                        {
                            SoapText = SoapInfo["SoapText"];
                            IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                            LastUpdated = SoapInfo["LastUpdated"];
                        }

                        var SocialHxkeyValues = new Dictionary<string, string>
                        {
                            { "SurgicalHxDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsSurgicalHx.SurgicalHx.SurgicalHxDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsSurgicalHx.SurgicalHx.SurgicalHxDateColumn.ColumnName]).ToShortDateString()},
                            { "SurgicalHxId",  MDVUtility.ToStr(dr[dsSurgicalHx.SurgicalHx.SurgicalHxIdColumn.ColumnName])},
                            { "SurgicalHxUnremarkable", MDVUtility.ToStr(dr[dsSurgicalHx.SurgicalHx.bUnremarkableColumn.ColumnName])},
                            { "SurgicalHxComments", MDVUtility.ToStr(dr[dsSurgicalHx.SurgicalHx.CommentsColumn.ColumnName])},
                            { "SurgicalHxSoapText", SoapText},
                            { "IsCreatedOrModified", IsCreatedOrModified},
                            { "LastUpdated",LastUpdated}
                        };

                        //List<Dictionary<string, string>> lstSurgicalHxStatus = new List<Dictionary<string, string>>();
                        //foreach (DataRow drTobacco in dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx_Disease.TableName].Rows)
                        //{
                        //    var TobaccoHxkeyValues = new Dictionary<string, string>
                        //    {
                        //        { "CPTCode", MDVUtility.ToStr(drTobacco[dsSurgicalHx.SurgicalHx_Disease.CPTCodeColumn.ColumnName])},
                        //        { "SurgicalStatus", MDVUtility.ToStr(drTobacco[dsSurgicalHx.SurgicalHx_Disease.StatusIdColumn.ColumnName])},
                        //        { "SurgicalLocation", MDVUtility.ToStr(drTobacco[dsSurgicalHx.SurgicalHx_Disease.LocationIdColumn.ColumnName])},
                        //        { "SurgicalSurgeryDate", MDVUtility.ToStr(drTobacco[dsSurgicalHx.SurgicalHx_Disease.SurgeryDateColumn.ColumnName])},
                        //        { "AgeAtSurgery", MDVUtility.ToStr(drTobacco[dsSurgicalHx.SurgicalHx_Disease.AgeAtSurgeryColumn.ColumnName])},
                        //        { "SurgicalReason", MDVUtility.ToStr(drTobacco[dsSurgicalHx.SurgicalHx_Disease.SurgeryReasonColumn.ColumnName])},
                        //        { "SurgicalOrderingProvider", MDVUtility.ToStr(drTobacco[dsSurgicalHx.SurgicalHx_Disease.OrderingProviderIdColumn.ColumnName])},
                        //        { "SurgicalPerformingProvider", MDVUtility.ToStr(drTobacco[dsSurgicalHx.SurgicalHx_Disease.PerformingProviderIdColumn.ColumnName])},
                        //        { "SurgicalComments", MDVUtility.ToStr(drTobacco[dsSurgicalHx.SurgicalHx_Disease.CommentsColumn.ColumnName])}

                        //    };
                        //    lstSurgicalHxStatus.Add(TobaccoHxkeyValues);
                        //}

                        //Start/27-1-2016/Abid Ali, for Disease Fill
                        var DiseaseHxkeyValues = new Dictionary<string, string> { { "", "" } };
                        if (MDVUtility.ToInt64(model.DiseaseId) > 0)
                        {

                            DSSurgicalHx.SurgicalHx_DiseaseRow[] arrToComponentRows = (DSSurgicalHx.SurgicalHx_DiseaseRow[])dsSurgicalHx.SurgicalHx_Disease.Select(MDVUtility.ToStr(dsSurgicalHx.SurgicalHx_Disease.DiseaseIdColumn.ColumnName) + "=" + MDVUtility.ToStr(model.DiseaseId) + "");
                            DataRow drDisease = (DataRow)arrToComponentRows[0];

                            DiseaseHxkeyValues = new Dictionary<string, string>
                        {


                                { "CPTCode", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTCodeColumn.ColumnName]) != ""? (MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTCodeColumn.ColumnName])+" - "+MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTCodeDescriptionColumn.ColumnName])):""},
                                { "CPTCodeId", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTCodeColumn.ColumnName])},
                                { "CPTDescription", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTCodeDescriptionColumn.ColumnName])},


                                { "CPTSNOMEDCodeId", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTSNOMEDIDColumn.ColumnName])},
                                { "CPTSNOMEDDescription", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CPTSNOMEDDescriptionColumn.ColumnName])},



                                { "SurgicalStatus", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.StatusIdColumn.ColumnName])},
                                { "SurgicalLocation", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.LocationColumn.ColumnName])},
                               
                                { "SurgicalSurgeryDate",  String.IsNullOrEmpty(MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.SurgeryDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(drDisease[dsSurgicalHx.SurgicalHx_Disease.SurgeryDateColumn.ColumnName]).ToShortDateString()},
                                
                                { "AgeAtSurgery", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.AgeAtSurgeryColumn.ColumnName])},
                                { "SurgicalReason", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.SurgeryReasonColumn.ColumnName])},

                                { "SurgicalOrderingProviderId", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.OrderingProviderIdColumn.ColumnName])},
                                { "SurgicalPerformingProviderId", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.PerformingProviderIdColumn.ColumnName])},

                                { "SurgicalOrderingProvider", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.OrderingProviderNameColumn.ColumnName])},
                                { "SurgicalPerformingProvider", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.PerformingProviderNameColumn.ColumnName])},
                                { "SurgicalComments", MDVUtility.ToStr(drDisease[dsSurgicalHx.SurgicalHx_Disease.CommentsColumn.ColumnName])}
                        };
                        }
                        //End/27-1-2016/Abid Ali, for Disease Fill

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            SurgicalHxFill_JSON = js.Serialize(SocialHxkeyValues),
                            surgicalHxDiseaseFill_JSON = js.Serialize(DiseaseHxkeyValues),
                            surgicalHxLoad_JSON = MDVUtility.JSON_DataTable(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName]),
                            surgicalHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx_Disease.TableName]),
                            DiseaseFillCount = dsSurgicalHx.SurgicalHx_Disease.Count,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                        /* End 21/01/2016 Syed Zia, load surgical Hx*/
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            SurgicalHxFill_JSON = "[]",
                            surgicalHxDiseaseFill_JSON = "[]",
                            surgicalHxLoad_JSON = MDVUtility.JSON_DataTable(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName]),
                            surgicalHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx_Disease.TableName]),
                            DiseaseFillCount = dsSurgicalHx.SurgicalHx_Disease.Count,
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



        public Dictionary<string, string> getCurrentSoapText(Int64 SurgicalHxId)
        {
            try
            {
                DSHistorySummary dsHistorySummarySoap = null;
                BLObject<DSHistorySummary> objSummary;
                objSummary = BLLClinicalObj.loadHxLog(SurgicalHxId, "SurgicalHx", "Current", 1, 10);
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
                   {"SurgicalHxId" ,MDVUtility.ToStr( SurgicalHxId)}
                };
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Author:  Muhammad Arshad
        /// Created Date: 14/01/2016
        /// OverView: This function will handle save of SurgicalHx
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fieldsJSON"></param>
        /// <param name="lstDiseaseObject"></param>
        /// <returns>string</returns>
        public string saveSurgicalHx(SurgicalHxModel model, string fieldsJSON, List<object> lstDiseaseObject)
        {
            try
            {
                DSSurgicalHx dsSurgicalHx = new DSSurgicalHx();

                DSSurgicalHx.SurgicalHxRow dr = dsSurgicalHx.SurgicalHx.NewSurgicalHxRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.SurgicalHxDate))
                {
                    dr.SurgicalHxDate = MDVUtility.ToDateTime(model.SurgicalHxDate);
                }
                else
                {
                    dr[dsSurgicalHx.SurgicalHx.SurgicalHxDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.SurgicalHxComments))
                {
                    dr.Comments = MDVUtility.ToStr(model.SurgicalHxComments);
                }
                else
                {
                    dr[dsSurgicalHx.SurgicalHx.CommentsColumn] = DBNull.Value;
                }

                dr.bUnremarkable = model.SurgicalHxUnremarkable.ToLower() == "true" ? true : false;

                if (model.AddFromMobile == "1")
                {
                    dr.CreatedBy = model.CreatedBy;
                    dr.CreatedOn = MDVUtility.ToDateTime(model.CreatedOn);
                    dr.ModifiedBy = model.ModifiedBy;
                    dr.ModifiedOn = MDVUtility.ToDateTime(model.ModifiedOn);
                }
                else
                {
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }

                dr.IsActive = true;
                

                #region Database Insertion
                dsSurgicalHx.SurgicalHx.AddSurgicalHxRow(dr);
                BLObject<DSSurgicalHx> obj = BLLClinicalObj.insertSurgicalHx(dsSurgicalHx);
                dsSurgicalHx = obj.Data;

                if (obj.Data != null)
                {
                    var diseaseId = "";
                    Int64 SurgicalHxId = MDVUtility.ToInt64(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows[0][dsSurgicalHx.SurgicalHx.SurgicalHxIdColumn.ColumnName]);
                    if (SurgicalHxId > 0)
                    {
                        if (lstDiseaseObject.Count > 0)
                        {
                            string responseMedicalHxDisease = insertUpdateDisease(SurgicalHxId, lstDiseaseObject, model.PatientId);
                            diseaseId = responseMedicalHxDisease;
                        }
                    }

                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForSurgicalHX(SurgicalHxId);

                    BLObject<DSSurgicalHx> objAdded = BLLClinicalObj.LoadSurgicalHx(MDVUtility.ToInt64(model.PatientId), SurgicalHxId, MDVUtility.ToInt64(diseaseId), "1", "");
                    dsSurgicalHx = objAdded.Data;
                    var SoapText = string.Empty;
                    var IsCreatedOrModified = string.Empty;
                    var LastUpdated = string.Empty;



                    var SoapInfo = getCurrentSoapText(MDVUtility.ToInt64(dr[dsSurgicalHx.SurgicalHx.SurgicalHxIdColumn.ColumnName]));

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
                            SurgicalHxId = MDVUtility.ToInt64(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows[0][dsSurgicalHx.SurgicalHx.SurgicalHxIdColumn.ColumnName]),
                            diseaseId = diseaseId,
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

        public string saveSurgicalHxNative(SurgicalHxModel model, string fieldsJSON, SurgicalHxDiseaseModel DiseaseObject)
        {
            try
            {
                long SurgicalHxId = Convert.ToInt64(DiseaseObject.SurgicalHxId);
                DSSurgicalHx dsSurgicalHx = new DSSurgicalHx();
                var diseaseId = "";
                if (SurgicalHxId <= 0)
                {
                    DSSurgicalHx.SurgicalHxRow dr = dsSurgicalHx.SurgicalHx.NewSurgicalHxRow();

                    dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                    if (!string.IsNullOrEmpty(model.SurgicalHxDate))
                    {
                        dr.SurgicalHxDate = MDVUtility.ToDateTime(DiseaseObject.CreatedOn);
                    }
                    else
                    {
                        dr[dsSurgicalHx.SurgicalHx.SurgicalHxDateColumn] = DBNull.Value;
                    }



                    dr.bUnremarkable = model.SurgicalHxUnremarkable.ToLower() == "true" ? true : false;

                    dr.IsActive = true;
                    dr.CreatedBy = DiseaseObject.CreatedBy;
                    dr.CreatedOn = MDVUtility.ToDateTime(DiseaseObject.CreatedOn);
                    dr.ModifiedBy = DiseaseObject.ModifiedBy;
                    dr.ModifiedOn = MDVUtility.ToDateTime(DiseaseObject.ModifiedOn);

                    #region Database Insertion
                    dsSurgicalHx.SurgicalHx.AddSurgicalHxRow(dr);
                    BLObject<DSSurgicalHx> obj = BLLClinicalObj.insertSurgicalHx(dsSurgicalHx);
                    dsSurgicalHx = obj.Data;



                    SurgicalHxId = MDVUtility.ToInt64(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows[0][dsSurgicalHx.SurgicalHx.SurgicalHxIdColumn.ColumnName]);
                }
                    if (SurgicalHxId > 0)
                    {
                        
                            string responseMedicalHxDisease = insertSurgicalDiseaseNative(SurgicalHxId, DiseaseObject);
                            diseaseId = responseMedicalHxDisease;
                        
                    }

                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForSurgicalHX(SurgicalHxId);

                    BLObject<DSSurgicalHx> objAdded = BLLClinicalObj.LoadSurgicalHx(MDVUtility.ToInt64(model.PatientId), SurgicalHxId, MDVUtility.ToInt64(diseaseId), "1", "");
                    dsSurgicalHx = objAdded.Data;
                    var SoapText = string.Empty;
                    var IsCreatedOrModified = string.Empty;
                    var LastUpdated = string.Empty;



                 //   var SoapInfo = getCurrentSoapText(MDVUtility.ToInt64(dr[dsSurgicalHx.SurgicalHx.SurgicalHxIdColumn.ColumnName]));

                    //if (SoapInfo != null)
                    //{
                    //    SoapText = SoapInfo["SoapText"];
                    //    IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                    //    LastUpdated = SoapInfo["LastUpdated"];
                    //}


                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        SurgicalHxId = MDVUtility.ToInt64(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows[0][dsSurgicalHx.SurgicalHx.SurgicalHxIdColumn.ColumnName]),
                        diseaseId = diseaseId,
                        SoapText = SoapText,
                        IsCreatedOrModified = IsCreatedOrModified,
                        LastUpdated = LastUpdated
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                
               
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.InnerException.ToString()),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        #region updateSurgicalHx

        /// <summary>
        ///  Author:  Abid Ali
        ///  Created Date: 28/01/2016
        ///  OverView: This function will handle update of SurgicalHx
        /// </summary>
        /// <param name="model"></param>
        /// <param name="surgicalHxId"></param>
        /// <param name="lstDiseaseObject"></param>
        /// <returns>string</returns>
        public string updateSurgicalHx(SurgicalHxModel model, Int64 surgicalHxId, List<object> lstDiseaseObject)
        {
            try
            {
                if (surgicalHxId > 0)
                {

                    DSSurgicalHx dsSurgicalHx = new DSSurgicalHx();
                    BLObject<DSSurgicalHx> obj = BLLClinicalObj.LoadSurgicalHx(MDVUtility.ToInt64(model.PatientId), surgicalHxId);
                    dsSurgicalHx = obj.Data;
                    foreach (DSSurgicalHx.SurgicalHxRow dr in dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows)
                    {
                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                        if (!string.IsNullOrEmpty(model.SurgicalHxDate))
                        {
                            dr.SurgicalHxDate = MDVUtility.ToDateTime(model.SurgicalHxDate);
                        }
                        else
                        {
                            dr[dsSurgicalHx.SurgicalHx.SurgicalHxDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.SurgicalHxComments))
                        {
                            dr.Comments = MDVUtility.ToStr(model.SurgicalHxComments);
                        }
                        else
                        {
                            dr[dsSurgicalHx.SurgicalHx.CommentsColumn] = DBNull.Value;
                        }

                        dr.bUnremarkable = model.SurgicalHxUnremarkable.ToLower() == "true" ? true : false;



                        if (model.AddFromMobile == "1")
                        {
                           
                            dr.ModifiedBy = model.ModifiedBy;
                            dr.ModifiedOn = MDVUtility.ToDateTime(model.ModifiedOn);
                        }
                        else
                        {
                           
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }

                        
                    }
                    var diseaseID = "";
                    if (lstDiseaseObject.Count > 0)
                    {
                        string responseMedicalHxDisease = insertUpdateDisease(surgicalHxId, lstDiseaseObject, model.PatientId);
                        diseaseID = responseMedicalHxDisease;
                    }

                    // Save/update of Surgical Hx Soap text 
                    #region Database Updation
                    if (dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows.Count > 0)
                    {
                        BLObject<DSSurgicalHx> objUpdate = BLLClinicalObj.updateSurgicalHx(dsSurgicalHx);
                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForSurgicalHX(surgicalHxId);
                        if (objUpdate.Data != null)
                        {
                            BLObject<DSSurgicalHx> objAdded = BLLClinicalObj.LoadSurgicalHx(MDVUtility.ToInt64(model.PatientId), surgicalHxId, MDVUtility.ToInt64(diseaseID), "1", "");
                            dsSurgicalHx = objAdded.Data;

                            var SoapText = string.Empty;
                            var IsCreatedOrModified = string.Empty;
                            var LastUpdated = string.Empty;


                            var SoapInfo = getCurrentSoapText(MDVUtility.ToInt64(surgicalHxId));

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
                                surgicalHxId = surgicalHxId
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
                        message = "Medical Hx not found."
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


        /// <summary>
        ///  Author:  Abid Ali
        ///  Created Date: 28/01/2016
        ///  OverView: Deletes SurgicalHx Disease and then updates soaptext of surgicalHx
        /// </summary>
        /// <param name="DiseaseId"></param>
        /// <param name="SurgicalHxId"></param>
        /// <returns>string</returns>
        public string deleteSurgicalHxDisease(string diseaseId, string surgicalHxId, string patientId)
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
                    BLObject<string> obj = BLLClinicalObj.deleteSurgicalHxDisease(MDVUtility.ToStr(diseaseId),patientId);
                    if (obj.Data == "")
                    {
                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForSurgicalHX(MDVUtility.ToInt64(surgicalHxId));
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
        ///  Author:  Abid Ali
        ///  Created Date: 28/01/2016
        ///  OverView: this function will handle insert/update of SurgicalHx disease
        ///  </summary>
        /// <OverView: Deletes SurgicalHx Disease and then updates soaptext of surgicalHxparam name="SurgicalHxId"></param>
        /// <param name="lstDiseaseObject"></param>
        /// <returns>string</returns>
        private string insertUpdateDisease(long surgicalHxId, List<object> lstDiseaseObject, string patientId)
        {
            #region Disease
            DSSurgicalHx dsDisease = new DSSurgicalHx();
            List<SurgicalHxDiseaseModel> lstDisease = lstDiseaseObject.OfType<SurgicalHxDiseaseModel>().ToList();
            //bool isFirstChild = false;
            foreach (SurgicalHxDiseaseModel CurrentModel in lstDisease)
            {
                if (CurrentModel.DiseaseId != null)
                {
                    Int32 currentDiseaseId = MDVUtility.ToInt32(CurrentModel.DiseaseId);
                    if(CurrentModel.AddFromMobile==null)
                    currentDiseaseId = currentDiseaseId == 0 ? -1 : currentDiseaseId;
                    BLObject<DSSurgicalHx> objSurgicalHx_Disease = BLLClinicalObj.LoadSurgicalHx_Disease(surgicalHxId, currentDiseaseId, "", "");
                    dsDisease = objSurgicalHx_Disease.Data;
                    DSSurgicalHx.SurgicalHx_DiseaseRow RowDisease = null;
                    if (dsDisease.SurgicalHx_Disease.Rows.Count > 0)
                    {
                        RowDisease = (DSSurgicalHx.SurgicalHx_DiseaseRow)dsDisease.SurgicalHx_Disease.Rows[0];
                    }
                    else
                    {
                        RowDisease = dsDisease.SurgicalHx_Disease.NewSurgicalHx_DiseaseRow();
                    }

                    if (RowDisease != null)
                    {
                        // bool isValueDifferent = false;
                        // bool istoUpdateRow = false;
                        if (dsDisease.SurgicalHx_Disease.Rows.Count < 1)
                        {
                            RowDisease.DiseaseId = currentDiseaseId;
                        }
                        RowDisease.SurgicalHxId = surgicalHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
                        {
                            RowDisease.CPTCode = MDVUtility.ToStr(CurrentModel.CPTCode);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CPTCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTDescription))
                        {
                            RowDisease.CPTCodeDescription = MDVUtility.ToStr(CurrentModel.CPTDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CPTCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDCodeId))
                        {
                            RowDisease.CPTSNOMEDID = MDVUtility.ToStr(CurrentModel.CPTSNOMEDCodeId);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CPTSNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDDescription))
                        {
                            RowDisease.CPTSNOMEDDescription = MDVUtility.ToStr(CurrentModel.CPTSNOMEDDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CPTSNOMEDDescriptionColumn] = DBNull.Value;
                        }
                      

                        

                        if (!string.IsNullOrEmpty(CurrentModel.SurgicalStatus))
                        {
                            RowDisease.StatusId = MDVUtility.ToInt32(CurrentModel.SurgicalStatus);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.StatusIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.SurgicalLocation))
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.LocationColumn] = MDVUtility.ToStr(CurrentModel.SurgicalLocation);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.LocationColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.PerformingProviderId))
                        {
                            RowDisease.PerformingProviderId = MDVUtility.ToInt32(CurrentModel.PerformingProviderId);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.PerformingProviderIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.OrderingProviderId))
                        {
                            RowDisease.OrderingProviderId = MDVUtility.ToInt32(CurrentModel.OrderingProviderId);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.OrderingProviderIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SurgicalSurgeryDate))
                        {
                            RowDisease.SurgeryDate = MDVUtility.ToDateTime(CurrentModel.SurgicalSurgeryDate);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.SurgeryDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AgeAtSurgery))
                        {
                            RowDisease.AgeAtSurgery = MDVUtility.ToStr(CurrentModel.AgeAtSurgery);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.AgeAtSurgeryColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SurgicalReason))
                        {
                            RowDisease.SurgeryReason = MDVUtility.ToStr(CurrentModel.SurgicalReason);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.SurgeryReasonColumn] = DBNull.Value;
                        }

                        //if (!string.IsNullOrEmpty(CurrentModel.SurgicalOrderingProvider))
                        //{
                        //    RowDisease.OrderingProviderId = MDVUtility.ToInt32(CurrentModel.SurgicalOrderingProvider);
                        //}
                        //else
                        //{
                        //    RowDisease[dsDisease.SurgicalHx_Disease.OrderingProviderIdColumn] = DBNull.Value;
                        //}


                        //if (!string.IsNullOrEmpty(CurrentModel.SurgicalPerformingProvider))
                        //{
                        //    RowDisease.PerformingProviderId = MDVUtility.ToInt32(CurrentModel.SurgicalPerformingProvider);
                        //}
                        //else
                        //{
                        //    RowDisease[dsDisease.SurgicalHx_Disease.PerformingProviderIdColumn] = DBNull.Value;
                        //}

                        if (!string.IsNullOrEmpty(CurrentModel.SurgicalDiseaseComments))
                        {
                            RowDisease.Comments = MDVUtility.ToStr(CurrentModel.SurgicalDiseaseComments);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CommentsColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.FreeTextProcedure))
                        {
                            RowDisease.FreeTextProcedure = MDVUtility.ToStr(CurrentModel.FreeTextProcedure);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.FreeTextProcedureColumn] = DBNull.Value;
                        }

                        RowDisease.IsActive = true;
                        RowDisease.PatientId = MDVUtility.ToLong(patientId);
                        if (CurrentModel.AddFromMobile != "1")
                        {
                            RowDisease.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowDisease.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowDisease.CreatedOn = DateTime.Now;
                            RowDisease.ModifiedOn = DateTime.Now;
                        }
                        else {
                            RowDisease.CreatedBy = CurrentModel.CreatedBy;
                            RowDisease.ModifiedBy = CurrentModel.ModifiedBy;
                            RowDisease.CreatedOn = MDVUtility.ToDateTime( CurrentModel.CreatedOn);
                            RowDisease.ModifiedOn = MDVUtility.ToDateTime(CurrentModel.ModifiedOn);
                            RowDisease.AddFromMobile = CurrentModel.AddFromMobile;
                            RowDisease.CPTID = CurrentModel.CPTID == "" ? 0 : Convert.ToInt64( CurrentModel.CPTID);
                            
                        }
                       

                        // if no record found against surgical disease, it implies for new record
                        if (dsDisease.SurgicalHx_Disease.Rows.Count < 1)
                        {
                            dsDisease.SurgicalHx_Disease.AddSurgicalHx_DiseaseRow(RowDisease);
                        }


                    }
                }
            }
            int counter = 0;

            //Soap text
            foreach (DataRow RowDisease in dsDisease.SurgicalHx_Disease.Rows)
            {
                RowDisease[dsDisease.SurgicalHx_Disease.SoapTextColumn] = insertUpdateSurgicalSoapText(dsDisease, lstDisease[counter]);
                counter++;
            }
            #endregion
            #region Database Insertion/Updation

            BLObject<DSSurgicalHx> objInsertedDisease = BLLClinicalObj.insertUpdateSurgicalHxDisease(dsDisease);
            if (objInsertedDisease.Data != null)
            {

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    diseaseId = dsDisease.Tables[dsDisease.SurgicalHx_Disease.TableName].Rows.Count > 0 ? dsDisease.Tables[dsDisease.SurgicalHx_Disease.TableName].Rows[0][dsDisease.SurgicalHx_Disease.DiseaseIdColumn.ColumnName] : 0,
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


            //#endregion
        }

        private string insertSurgicalDiseaseNative(long surgicalHxId, SurgicalHxDiseaseModel DiseaseObject)
        {
            #region Disease
            DSSurgicalHx dsDisease = new DSSurgicalHx();
           // List<SurgicalHxDiseaseModel> lstDisease = lstDiseaseObject.OfType<SurgicalHxDiseaseModel>().ToList();
            //bool isFirstChild = false;
            //foreach (SurgicalHxDiseaseModel CurrentModel in lstDisease)
            //{
                if (DiseaseObject.DiseaseId != null)
                {
                    Int32 currentDiseaseId = MDVUtility.ToInt32(DiseaseObject.DiseaseId);
                    currentDiseaseId = currentDiseaseId == 0 ? -1 : currentDiseaseId;
                    BLObject<DSSurgicalHx> objSurgicalHx_Disease = BLLClinicalObj.LoadSurgicalHx_Disease(surgicalHxId, currentDiseaseId, "", "");
                    dsDisease = objSurgicalHx_Disease.Data;
                    DSSurgicalHx.SurgicalHx_DiseaseRow RowDisease = null;
                    if (dsDisease.SurgicalHx_Disease.Rows.Count > 0)
                    {
                        RowDisease = (DSSurgicalHx.SurgicalHx_DiseaseRow)dsDisease.SurgicalHx_Disease.Rows[0];
                    }
                    else
                    {
                        RowDisease = dsDisease.SurgicalHx_Disease.NewSurgicalHx_DiseaseRow();
                    }

                    if (RowDisease != null)
                    {
                        // bool isValueDifferent = false;
                        // bool istoUpdateRow = false;
                        if (dsDisease.SurgicalHx_Disease.Rows.Count < 1)
                        {
                            RowDisease.DiseaseId = currentDiseaseId;
                        }
                        RowDisease.SurgicalHxId = surgicalHxId;

                        if (!string.IsNullOrEmpty(DiseaseObject.CPTCode))
                        {
                            RowDisease.CPTCode = MDVUtility.ToStr(DiseaseObject.CPTCode);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CPTCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(DiseaseObject.CPTDescription))
                        {
                            RowDisease.CPTCodeDescription = MDVUtility.ToStr(DiseaseObject.CPTDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CPTCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(DiseaseObject.CPTSNOMEDCodeId))
                        {
                            RowDisease.CPTSNOMEDID = MDVUtility.ToStr(DiseaseObject.CPTSNOMEDCodeId);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CPTSNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(DiseaseObject.CPTSNOMEDDescription))
                        {
                            RowDisease.CPTSNOMEDDescription = MDVUtility.ToStr(DiseaseObject.CPTSNOMEDDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CPTSNOMEDDescriptionColumn] = DBNull.Value;
                        }




                        if (!string.IsNullOrEmpty(DiseaseObject.SurgicalStatus))
                        {
                            RowDisease.StatusId = MDVUtility.ToInt32(DiseaseObject.SurgicalStatus);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.StatusIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(DiseaseObject.SurgicalLocation))
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.LocationColumn] = MDVUtility.ToStr(DiseaseObject.SurgicalLocation);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.LocationColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(DiseaseObject.PerformingProviderId))
                        {
                            RowDisease.PerformingProviderId = MDVUtility.ToInt32(DiseaseObject.PerformingProviderId);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.PerformingProviderIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(DiseaseObject.OrderingProviderId))
                        {
                            RowDisease.OrderingProviderId = MDVUtility.ToInt32(DiseaseObject.OrderingProviderId);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.OrderingProviderIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(DiseaseObject.SurgicalSurgeryDate))
                        {
                            RowDisease.SurgeryDate = MDVUtility.ToDateTime(DiseaseObject.SurgicalSurgeryDate);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.SurgeryDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(DiseaseObject.AgeAtSurgery))
                        {
                            RowDisease.AgeAtSurgery = MDVUtility.ToStr(DiseaseObject.AgeAtSurgery);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.AgeAtSurgeryColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(DiseaseObject.SurgicalReason))
                        {
                            RowDisease.SurgeryReason = MDVUtility.ToStr(DiseaseObject.SurgicalReason);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.SurgeryReasonColumn] = DBNull.Value;
                        }

                        //if (!string.IsNullOrEmpty(DiseaseObject.SurgicalOrderingProvider))
                        //{
                        //    RowDisease.OrderingProviderId = MDVUtility.ToInt32(DiseaseObject.SurgicalOrderingProvider);
                        //}
                        //else
                        //{
                        //    RowDisease[dsDisease.SurgicalHx_Disease.OrderingProviderIdColumn] = DBNull.Value;
                        //}


                        //if (!string.IsNullOrEmpty(DiseaseObject.SurgicalPerformingProvider))
                        //{
                        //    RowDisease.PerformingProviderId = MDVUtility.ToInt32(DiseaseObject.SurgicalPerformingProvider);
                        //}
                        //else
                        //{
                        //    RowDisease[dsDisease.SurgicalHx_Disease.PerformingProviderIdColumn] = DBNull.Value;
                        //}

                        if (!string.IsNullOrEmpty(DiseaseObject.SurgicalDiseaseComments))
                        {
                            RowDisease.Comments = MDVUtility.ToStr(DiseaseObject.SurgicalDiseaseComments);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CommentsColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(DiseaseObject.FreeTextProcedure))
                        {
                            RowDisease.FreeTextProcedure = MDVUtility.ToStr(DiseaseObject.FreeTextProcedure);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.FreeTextProcedureColumn] = DBNull.Value;
                        }

                        RowDisease.IsActive = true;
                        RowDisease.CreatedBy = DiseaseObject.CreatedBy;
                        RowDisease.CreatedOn = Convert.ToDateTime(DiseaseObject.CreatedOn);
                        RowDisease.ModifiedBy = DiseaseObject.ModifiedBy;
                        RowDisease.ModifiedOn = Convert.ToDateTime(DiseaseObject.ModifiedOn);

                    // if no record found against surgical disease, it implies for new record
                    if (dsDisease.SurgicalHx_Disease.Rows.Count < 1)
                        {
                            dsDisease.SurgicalHx_Disease.AddSurgicalHx_DiseaseRow(RowDisease);
                        }


                    }
                }
           // }
            int counter = 0;

            //Soap text
            foreach (DataRow RowDisease in dsDisease.SurgicalHx_Disease.Rows)
            {
                RowDisease[dsDisease.SurgicalHx_Disease.SoapTextColumn] = insertUpdateSurgicalSoapText(dsDisease, DiseaseObject);
                counter++;
            }
            #endregion
            #region Database Insertion/Updation

            BLObject<DSSurgicalHx> objInsertedDisease = BLLClinicalObj.insertUpdateSurgicalHxDisease(dsDisease);
            if (objInsertedDisease.Data != null)
            {

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    diseaseId = dsDisease.Tables[dsDisease.SurgicalHx_Disease.TableName].Rows.Count > 0 ? dsDisease.Tables[dsDisease.SurgicalHx_Disease.TableName].Rows[0][dsDisease.SurgicalHx_Disease.DiseaseIdColumn.ColumnName] : 0,
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


            //#endregion
        }
        /// <summary>
        /// Author:  Farooq Ahmad
        ///  Created Date: 28/01/2016
        /// OverView: thois function will create soap text for surgical Hx
        /// </summary>
        /// <param name="dsSurgicalHistory"></param>
        /// <param name="modelObj"></param>
        /// <returns>string</returns>
        internal string insertUpdateSurgicalSoapText(DSSurgicalHx dsSurgicalHistory, SurgicalHxDiseaseModel modelObj)
        {
            string TobaccoSoapText = string.Empty;
            StringBuilder sb = new StringBuilder();
            // string frequency = "";
            if (modelObj != null)
            {
                sb.Append("<div id=SurgicalHistory_" + modelObj.SurgicalHxId + "' title='Surgical hx'  name='Surgical Hx'>");

                sb.Append((!string.IsNullOrEmpty(modelObj.FreeTextProcedure) ? " Patient underwent " + modelObj.FreeTextProcedure : (string.IsNullOrEmpty(modelObj.CPTDescription) ? "" : " Patient underwent " + modelObj.CPTDescription)));
                //sb.Append("Patient underwent " + modelObj.CPTDescription + " based on the following assessment: ");
                // sb.Append((string.IsNullOrEmpty(modelObj.Disease) ? "" : modelObj.Disease));
                sb.Append((string.IsNullOrEmpty(modelObj.SurgicalSurgeryDate) ? "" : ", on " + modelObj.SurgicalSurgeryDate));
                sb.Append((string.IsNullOrEmpty(modelObj.AgeAtSurgery) ? "" : ", at the age of " + modelObj.AgeAtSurgery.Trim()));
                sb.Append((string.IsNullOrEmpty(modelObj.SurgicalReason) ? "" : ", for the following reason: " + modelObj.SurgicalReason));
                if (!string.IsNullOrEmpty(modelObj.SurgicalStatus))
                {
                    sb.Append((string.IsNullOrEmpty(modelObj.SurgicalStatusText) ? "" : ", Status was " + modelObj.SurgicalStatusText));
                }
                sb.Append((string.IsNullOrEmpty(modelObj.SurgicalLocation) ? "" : ", Surgery location was " + modelObj.SurgicalLocation));
                sb.Append((string.IsNullOrEmpty(modelObj.SurgicalOrderingProvider) ? "" : ", Ordering Provider was " + modelObj.SurgicalOrderingProvider.Replace(",", " ").Trim()));
                //Start Farooq Ahmad 10/02/2016 changing Performing to Referring
                sb.Append((string.IsNullOrEmpty(modelObj.SurgicalPerformingProvider) ? "" : ", Performing Provider was " + modelObj.SurgicalPerformingProvider.Replace(",", " ").Trim()));
                //End Farooq Ahmad 10/02/2016 changing Performing to Referring
                sb.Append((string.IsNullOrEmpty(modelObj.SurgicalDiseaseComments) ? "" : ", Comments: " + modelObj.SurgicalDiseaseComments) + "<br/>");
            }
            else
            {
                if (dsSurgicalHistory.SurgicalHx_Disease != null && dsSurgicalHistory.SurgicalHx_Disease.Rows.Count > 0)
                {
                    //foreach (var item in dsSurgicalHistory.SurgicalHx_Disease)
                    //{
                    //    sb.Append("<div id='medicalHistory_" + modelObj.SurgicalHxId + "' title='Medical'  name='Medical Hx'><strong>Medical: </strong>");
                    //    sb.Append((string.IsNullOrEmpty(modelObj.CPTCodeId) ? "" : "Patient underwent " + modelObj.CPTDescription + ", ") + (string.IsNullOrEmpty(modelObj.ICD9CodeDescription) ? "" : " based on the following assessment: " + modelObj.ICD9CodeDescription) + " " + (string.IsNullOrEmpty(modelObj.MedicalDiseaseStatusText) ? "" : "(" + modelObj.MedicalDiseaseStatusText + ")"));
                    //    sb.Append((string.IsNullOrEmpty(modelObj.MedicalDiseaseFromDate) ? "" : " From" + modelObj.MedicalDiseaseFromDate) + (string.IsNullOrEmpty(modelObj.MedicalDiseaseToDate) ? "" : " to" + modelObj.MedicalDiseaseToDate + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseTestResultText) ? "" : " The test result is " + modelObj.MedicalDiseaseTestResultText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseOnset) ? "" : " Onset is" + modelObj.MedicalDiseaseOnset + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseDurationLength) ? "" : " Duration is" + modelObj.MedicalDiseaseDurationLength + " " + modelObj.MedicalDiseaseDurationPeriodText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseSeverityText) ? "" : " Severity is " + modelObj.MedicalDiseaseSeverityText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseasePatternText) ? "" : " Pattern is " + modelObj.MedicalDiseasePatternText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseAgggravatedByText) ? "" : " Aggravated  by" + modelObj.MedicalDiseaseAgggravatedByText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseLocation) ? "" : " Location is" + modelObj.MedicalDiseaseLocation + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseComments) ? "" : " Comments: " + modelObj.MedicalDiseaseComments + ".") + "</div>");
                    //}
                }
                else
                {
                    return string.Empty;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        ///   Author:  Ahmad Raza
        ///   Created Date: 22/01/2016
        ///   OverView: thois function will attach surgical hx with note
        /// </summary>
        /// <param name="SurgicalHxId"></param>
        /// <param name="NotesId"></param>
        /// <returns>string</returns>
        #region Attach/Detach SurgicalHx from Notes
        internal string attach_SurgicalHx_With_Notes(string surgicalHxId, long notesId)
        {
            try
            {
                DSSurgicalHx dsSurgicalHx = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(surgicalHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<DSSurgicalHx> obj = BLLClinicalObj.attachSurgicalHxWithNotes(surgicalHxId, notesId);
                    if (obj.Data != null)
                    {
                        dsSurgicalHx = obj.Data;
                        var response = new
                        {
                            status = true,
                            SurgicalHxTotalCount = dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows.Count,
                            SurgicalHxCount = dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows.Count,
                            SurgicalHxLoad_JSON = MDVUtility.JSON_DataTable(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName]),
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

        /// <summary>
        ///   Author:  Ahmad Raza
        ///   Created Date: 22/01/2016
        ///   OverView: thois function will detach surgical hx from note
        /// </summary>
        /// <param name="SurgicalHxId"></param>
        /// <param name="NotesId"></param>
        /// <returns>string</returns>
        internal string detach_SurgicalHx_From_Notes(long surgicalHxId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(surgicalHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachSurgicalHxFromNotes(surgicalHxId, notesId);
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

        public SurgicalHxModel saveSurgicalHx(SurgicalHxModel model, string fieldsJSON, List<SurgicalHxDiseaseModel> lstDiseaseObject, long NoteId)
        {
            try
            {
                DSSurgicalHx dsSurgicalHx = new DSSurgicalHx();
                DSSurgicalHx.SurgicalHxRow dr;
                if (!string.IsNullOrEmpty(model.SurgicalHxId) && MDVUtility.ToInt64(model.SurgicalHxId) > 0)
                {
                    BLObject<DSSurgicalHx> obj = BLLClinicalObj.LoadSurgicalHx(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.SurgicalHxId));
                    dsSurgicalHx = obj.Data;
                    dr = dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows[0] as DSSurgicalHx.SurgicalHxRow;
                }
                else
                {
                    dr = dsSurgicalHx.SurgicalHx.NewSurgicalHxRow();
                }

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                if (!string.IsNullOrEmpty(model.SurgicalHxDate))
                {
                    dr.SurgicalHxDate = MDVUtility.ToDateTime(model.SurgicalHxDate);
                }
                else
                {
                    dr[dsSurgicalHx.SurgicalHx.SurgicalHxDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.SurgicalHxComments))
                {
                    dr.Comments = MDVUtility.ToStr(model.SurgicalHxComments);
                }
                else
                {
                    dr[dsSurgicalHx.SurgicalHx.CommentsColumn] = DBNull.Value;
                }

                dr.bUnremarkable = model.SurgicalHxUnremarkable.ToLower() == "true" ? true : false;

                if (NoteId > 0)
                {
                    dr.NotesId = NoteId;
                }
                else
                {
                    dr[dsSurgicalHx.SurgicalHx.NotesIdColumn] = DBNull.Value;
                }

                dr.IsActive = true;
                if (MDVUtility.ToInt64(model.SurgicalHxId) <= 0)
                {
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                }
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                BLObject<DSSurgicalHx> objdata = new BLObject<DSSurgicalHx>();
               // MDVLogger.BLLErrorLog("DownloadPrescriptionCount::", null);
                if (!string.IsNullOrEmpty(model.SurgicalHxId) && MDVUtility.ToInt64(model.SurgicalHxId) > 0)
                {
                    objdata = BLLClinicalObj.updateSurgicalHx(dsSurgicalHx);
                }
                else
                {
                    dsSurgicalHx.SurgicalHx.AddSurgicalHxRow(dr);
                    objdata = BLLClinicalObj.insertSurgicalHx(dsSurgicalHx);
                }

                dsSurgicalHx = objdata.Data;

                if (objdata.Data != null)
                {
                    var diseaseId = "";
                    model.SurgicalHxId = MDVUtility.ToStr(dsSurgicalHx.Tables[dsSurgicalHx.SurgicalHx.TableName].Rows[0][dsSurgicalHx.SurgicalHx.SurgicalHxIdColumn.ColumnName]);
                    if (!string.IsNullOrEmpty(model.SurgicalHxId))
                    {
                        if (lstDiseaseObject.Count > 0)
                        {
                            string responseMedicalHxDisease = insertUpdateDisease(MDVUtility.ToInt64(model.SurgicalHxId), lstDiseaseObject);
                            diseaseId = responseMedicalHxDisease;
                        }
                    }

                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForSurgicalHX(MDVUtility.ToInt64(model.SurgicalHxId));

                    //BLObject<DSSurgicalHx> objAdded = BLLClinicalObj.LoadSurgicalHx(MDVUtility.ToInt64(model.PatientId), SurgicalHxId, MDVUtility.ToInt64(diseaseId), "1", "");
                    //dsSurgicalHx = objAdded.Data;
                    //var SoapText = string.Empty;
                    //var IsCreatedOrModified = string.Empty;
                    //var LastUpdated = string.Empty;



                    //var SoapInfo = getCurrentSoapText(MDVUtility.ToInt64(dr[dsSurgicalHx.SurgicalHx.SurgicalHxIdColumn.ColumnName]));

                    //if (SoapInfo != null)
                    //{
                    //    SoapText = SoapInfo["SoapText"];
                    //    IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                    //    LastUpdated = SoapInfo["LastUpdated"];
                    //}



                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
        private string insertUpdateDisease(long surgicalHxId, List<SurgicalHxDiseaseModel> lstDisease)
        {
            #region Disease
            DSSurgicalHx dsDisease = new DSSurgicalHx();

            //bool isFirstChild = false;
            BLObject<DSSurgicalHx> objSurgicalHx_Disease = BLLClinicalObj.LoadSurgicalHx_Disease(surgicalHxId, 0, "", "");
            dsDisease = objSurgicalHx_Disease.Data;
            foreach (SurgicalHxDiseaseModel CurrentModel in lstDisease)
            {
                if (CurrentModel.DiseaseId != null)
                {
                    Int32 currentDiseaseId = MDVUtility.ToInt32(CurrentModel.DiseaseId);
                    currentDiseaseId = currentDiseaseId == 0 ? -1 : currentDiseaseId;

                    DSSurgicalHx.SurgicalHx_DiseaseRow RowDisease = null;
                    DSSurgicalHx.SurgicalHx_DiseaseRow[] arrDiseases = (DSSurgicalHx.SurgicalHx_DiseaseRow[])dsDisease.SurgicalHx_Disease.Select(dsDisease.SurgicalHx_Disease.DiseaseIdColumn.ColumnName + "=" + currentDiseaseId);
                    if (arrDiseases != null && arrDiseases.Length > 0)
                    {
                        RowDisease = arrDiseases[0];
                    }
                    else
                    {
                        RowDisease = dsDisease.SurgicalHx_Disease.NewSurgicalHx_DiseaseRow();
                    }





                    if (RowDisease != null)
                    {
                        // bool isValueDifferent = false;
                        // bool istoUpdateRow = false;
                        if (dsDisease.SurgicalHx_Disease.Rows.Count < 1)
                        {
                            RowDisease.DiseaseId = currentDiseaseId;
                        }
                        RowDisease.SurgicalHxId = surgicalHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
                        {
                            RowDisease.CPTCode = MDVUtility.ToStr(CurrentModel.CPTCode);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CPTCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTDescription))
                        {
                            RowDisease.CPTCodeDescription = MDVUtility.ToStr(CurrentModel.CPTDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CPTCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDCodeId))
                        {
                            RowDisease.CPTSNOMEDID = MDVUtility.ToStr(CurrentModel.CPTSNOMEDCodeId);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CPTSNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDDescription))
                        {
                            RowDisease.CPTSNOMEDDescription = MDVUtility.ToStr(CurrentModel.CPTSNOMEDDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CPTSNOMEDDescriptionColumn] = DBNull.Value;
                        }




                        if (!string.IsNullOrEmpty(CurrentModel.SurgicalStatus))
                        {
                            RowDisease.StatusId = MDVUtility.ToInt32(CurrentModel.SurgicalStatus);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.StatusIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.SurgicalLocation))
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.LocationColumn] = MDVUtility.ToStr(CurrentModel.SurgicalLocation);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.LocationColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.PerformingProviderId))
                        {
                            RowDisease.PerformingProviderId = MDVUtility.ToInt32(CurrentModel.PerformingProviderId);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.PerformingProviderIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.OrderingProviderId))
                        {
                            RowDisease.OrderingProviderId = MDVUtility.ToInt32(CurrentModel.OrderingProviderId);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.OrderingProviderIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SurgicalSurgeryDate))
                        {
                            RowDisease.SurgeryDate = MDVUtility.ToDateTime(CurrentModel.SurgicalSurgeryDate);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.SurgeryDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AgeAtSurgery))
                        {
                            RowDisease.AgeAtSurgery = MDVUtility.ToStr(CurrentModel.AgeAtSurgery);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.AgeAtSurgeryColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SurgicalReason))
                        {
                            RowDisease.SurgeryReason = MDVUtility.ToStr(CurrentModel.SurgicalReason);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.SurgeryReasonColumn] = DBNull.Value;
                        }

                        //if (!string.IsNullOrEmpty(CurrentModel.SurgicalOrderingProvider))
                        //{
                        //    RowDisease.OrderingProviderId = MDVUtility.ToInt32(CurrentModel.SurgicalOrderingProvider);
                        //}
                        //else
                        //{
                        //    RowDisease[dsDisease.SurgicalHx_Disease.OrderingProviderIdColumn] = DBNull.Value;
                        //}


                        //if (!string.IsNullOrEmpty(CurrentModel.SurgicalPerformingProvider))
                        //{
                        //    RowDisease.PerformingProviderId = MDVUtility.ToInt32(CurrentModel.SurgicalPerformingProvider);
                        //}
                        //else
                        //{
                        //    RowDisease[dsDisease.SurgicalHx_Disease.PerformingProviderIdColumn] = DBNull.Value;
                        //}

                        if (!string.IsNullOrEmpty(CurrentModel.SurgicalDiseaseComments))
                        {
                            RowDisease.Comments = MDVUtility.ToStr(CurrentModel.SurgicalDiseaseComments);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.CommentsColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.FreeTextProcedure))
                        {
                            RowDisease.FreeTextProcedure = MDVUtility.ToStr(CurrentModel.FreeTextProcedure);
                        }
                        else
                        {
                            RowDisease[dsDisease.SurgicalHx_Disease.FreeTextProcedureColumn] = DBNull.Value;
                        }

                        RowDisease.IsActive = true;
                        RowDisease.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowDisease.CreatedOn = DateTime.Now;
                        RowDisease.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowDisease.ModifiedOn = DateTime.Now;
                        RowDisease[dsDisease.SurgicalHx_Disease.SoapTextColumn] = insertUpdateSurgicalSoapText(dsDisease, CurrentModel);

                        // if no record found against surgical disease, it implies for new record
                        if (arrDiseases.Length < 1)
                        {
                            dsDisease.SurgicalHx_Disease.AddSurgicalHx_DiseaseRow(RowDisease);
                        }
                    }
                }
            }
            //int counter = 0;

            ////Soap text
            //foreach (DataRow RowDisease in dsDisease.SurgicalHx_Disease.Rows)
            //{
            //    RowDisease[dsDisease.SurgicalHx_Disease.SoapTextColumn] = insertUpdateSurgicalSoapText(dsDisease, lstDisease[counter]);
            //    counter++;
            //}
            #endregion
            #region Database Insertion/Updation

            BLObject<DSSurgicalHx> objInsertedDisease = BLLClinicalObj.insertUpdateSurgicalHxDisease(dsDisease);
            if (objInsertedDisease.Data != null)
            {

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    diseaseId = dsDisease.Tables[dsDisease.SurgicalHx_Disease.TableName].Rows.Count > 0 ? dsDisease.Tables[dsDisease.SurgicalHx_Disease.TableName].Rows[0][dsDisease.SurgicalHx_Disease.DiseaseIdColumn.ColumnName] : 0,
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


            //#endregion
        }
    }
}