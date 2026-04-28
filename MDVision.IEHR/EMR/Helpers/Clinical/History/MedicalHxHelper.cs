// Author:  Muhammad Arshad
// Created Date: 04/01/2016
//OverView: Helper class for MedicalHx
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
    public class MedicalHxHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public MedicalHxHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static MedicalHxHelper _instance = null;
        public static MedicalHxHelper Instance()
        {
            if (_instance == null)
                _instance = new MedicalHxHelper();
            return _instance;
        }

        #region saveMedicalHx

        // Author:  Muhammad Arshad
        // Created Date: 07/01/2016
        //OverView: This function will handle saving of MedicalHx
        public string saveMedicalHx(MedicalHxModel model, string fieldsJSON, List<object> lstDiseaseObject)
        {
            try
            {
                DSMedicalHx dsMedicalHx = new DSMedicalHx();

                DSMedicalHx.MedicalHxRow dr = dsMedicalHx.MedicalHx.NewMedicalHxRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.MedicalHxDate))
                {
                    dr.MedicalHxDate = MDVUtility.ToDateTime(model.MedicalHxDate);
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.MedicalHxDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.MedicalHxComments))
                {
                    dr.Comments = MDVUtility.ToStr(model.MedicalHxComments);
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.CommentsColumn] = DBNull.Value;
                }

                if (!model.IsRCPneumococcal != null)
                {
                    dr.IsRCPneumococcal = MDVUtility.ToBool(model.IsRCPneumococcal);
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.IsRCPneumococcalColumn] = DBNull.Value;
                }
                if (!model.IsRCInfluenza != null)
                {
                    dr.IsRCInfluenza = MDVUtility.ToBool(model.IsRCInfluenza);
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.IsRCInfluenzaColumn] = DBNull.Value;
                }
                if (model.RCPneumococcalDate != null)
                {
                    dr.RCPneumococcalDate = MDVUtility.ToDateTime(model.RCPneumococcalDate);
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.RCPneumococcalDateColumn] = DBNull.Value;
                }
                if (model.RCInfluenzaDate != null)
                {
                    dr.RCInfluenzaDate = MDVUtility.ToDateTime(model.RCInfluenzaDate);
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.RCInfluenzaDateColumn] = DBNull.Value;
                }

                dr.bUnremarkable = model.MedicalHxUnremarkable.ToLower() == "true" ? true : false;

                dr.IsActive = true;

                if (model.AddedFromMobileApp == "1")
                {

                    dr.CreatedBy = model.CreatedBy;
                    dr.CreatedOn =Convert.ToDateTime(model.CreatedOn);
                    dr.ModifiedBy = model.ModifiedBy;
                    dr.ModifiedOn = Convert.ToDateTime( model.ModifiedOn);
                  
                }
                else
                {
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Insertion
                dsMedicalHx.MedicalHx.AddMedicalHxRow(dr);
                BLObject<DSMedicalHx> obj = BLLClinicalObj.InsertMedicalHx(dsMedicalHx, model.PatientId);
                dsMedicalHx = obj.Data;

                if (obj.Data != null)
                {
                    var diseaseID = "";
                    Int64 MedicalHxId = MDVUtility.ToInt64(dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName].Rows[0][dsMedicalHx.MedicalHx.MedicalHxIdColumn.ColumnName]);
                    if (MedicalHxId > 0)
                    {
                        if (lstDiseaseObject.Count > 0)
                        {
                            string responseMedicalHxDisease = insertUpdateDisease(MedicalHxId, lstDiseaseObject, model.PatientId,0,model.FromClinicalSide);
                            diseaseID = responseMedicalHxDisease;
                        }
                    }
                    /*
                       Change Implement BY: Muhammad Azhar Shahzad
                       Reason: To update Soap Text of Social Hx in Insert mode
                       Created Date: Dec 15, 2015
                   */
                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MedicalHxId);


                    DSHistorySummary dsHistorySummarySoap = null;
                    BLObject<DSHistorySummary> objSummary;
                    objSummary = BLLClinicalObj.loadHxLog(MedicalHxId, "MedicalHx", "Current", 1, 10);
                    dsHistorySummarySoap = objSummary.Data;

                    var SoapText = string.Empty;
                    var IsCreatedOrModified = string.Empty;
                    var LastUpdated = string.Empty;

                    if (dsHistorySummarySoap != null && dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows.Count > 0)
                    {
                        var Medicaldr = dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows[0];
                        SoapText = MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.SoapTextColumn.ColumnName]);
                        IsCreatedOrModified = MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.ActionColumn.ColumnName]);
                        LastUpdated = string.Concat(MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.ModifiedOnColumn.ColumnName]), " ", MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.ModifiedByColumn.ColumnName]));
                    }


                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        MedicalHxId = MDVUtility.ToInt64(dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName].Rows[0][dsMedicalHx.MedicalHx.MedicalHxIdColumn.ColumnName]),
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
       
        public string saveMedicalHx(MedicalHxModel model, long noteId)
        {
            try
            {               
                DSMedicalHx dsMedicalHx = new DSMedicalHx();

                DSMedicalHx.MedicalHxRow dr = dsMedicalHx.MedicalHx.NewMedicalHxRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.MedicalHxDate))
                {
                    dr.MedicalHxDate = MDVUtility.ToDateTime(model.MedicalHxDate);
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.MedicalHxDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.MedicalHxComments))
                {
                    dr.Comments = MDVUtility.ToStr(model.MedicalHxComments);
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.CommentsColumn] = DBNull.Value;
                }

                if (!model.IsRCPneumococcal != null)
                {
                    dr.IsRCPneumococcal = MDVUtility.ToBool(model.IsRCPneumococcal);
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.IsRCPneumococcalColumn] = DBNull.Value;
                }
                if (!model.IsRCInfluenza != null)
                {
                    dr.IsRCInfluenza = MDVUtility.ToBool(model.IsRCInfluenza);
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.IsRCInfluenzaColumn] = DBNull.Value;
                }
                if (model.RCPneumococcalDate != null)
                {
                    dr.RCPneumococcalDate = MDVUtility.ToDateTime(model.RCPneumococcalDate);
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.RCPneumococcalDateColumn] = DBNull.Value;
                }
                if (model.RCInfluenzaDate != null)
                {
                    dr.RCInfluenzaDate = MDVUtility.ToDateTime(model.RCInfluenzaDate);
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.RCInfluenzaDateColumn] = DBNull.Value;
                }
                if (noteId > 0)
                {
                    dr.NoteId = noteId;
                }
                else
                {
                    dr[dsMedicalHx.MedicalHx.NoteIdColumn] = DBNull.Value;
                }

                dr.bUnremarkable = model.MedicalHxUnremarkable.ToLower() == "true" ? true : false;

                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsMedicalHx.MedicalHx.AddMedicalHxRow(dr);
                BLObject<DSMedicalHx> obj = BLLClinicalObj.InsertMedicalHx(dsMedicalHx, model.PatientId);
                dsMedicalHx = obj.Data;

                if (obj.Data != null)
                {

                    var diseaseID = "";

                    Int64 MedicalHxId = MDVUtility.ToInt64(dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName].Rows[0][dsMedicalHx.MedicalHx.MedicalHxIdColumn.ColumnName]);
                    if (MedicalHxId > 0)
                    {
                        if (model.MedicalDiseaseList != null && model.MedicalDiseaseList.Count > 0)
                        {
                            List<object> diseaseList = new List<object>(model.MedicalDiseaseList);
                            string responseMedicalHxDisease = insertUpdateDisease(MedicalHxId, diseaseList, model.PatientId, 0);
                            diseaseID = responseMedicalHxDisease;
                        }
                    }
                    /*
                       Change Implement BY: Muhammad Azhar Shahzad
                       Reason: To update Soap Text of Social Hx in Insert mode
                       Created Date: Dec 15, 2015
                   */
                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MedicalHxId);


                    DSHistorySummary dsHistorySummarySoap = null;
                    BLObject<DSHistorySummary> objSummary;
                    objSummary = BLLClinicalObj.loadHxLog(MedicalHxId, "MedicalHx", "Current", 1, 10);
                    dsHistorySummarySoap = objSummary.Data;

                    var SoapText = string.Empty;
                    var IsCreatedOrModified = string.Empty;
                    var LastUpdated = string.Empty;

                    if (dsHistorySummarySoap != null && dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows.Count > 0)
                    {
                        var Medicaldr = dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows[0];
                        SoapText = MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.SoapTextColumn.ColumnName]);
                        IsCreatedOrModified = MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.ActionColumn.ColumnName]);
                        LastUpdated = string.Concat(MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.ModifiedOnColumn.ColumnName]), " ", MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.ModifiedByColumn.ColumnName]));
                    }


                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        MedicalHxId = MDVUtility.ToInt64(dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName].Rows[0][dsMedicalHx.MedicalHx.MedicalHxIdColumn.ColumnName]),
                       // diseaseId = diseaseID,
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

        #region updateMedicalHx
        // Author:  Muhammad Arshad
        // Created Date: 08/01/2016
        //OverView: This function will handle update of MedicalHx
        public string updateMedicalHx(MedicalHxModel model, Int64 MedicalHxId, List<object> lstDiseaseObject)
        {
            try
            {
                if (MedicalHxId > 0)
                {

                    DSMedicalHx dsMedicalHx = new DSMedicalHx();
                    BLObject<DSMedicalHx> obj = BLLClinicalObj.LoadMedicalHx(MDVUtility.ToInt64(model.PatientId), MedicalHxId);
                    dsMedicalHx = obj.Data;
                    foreach (DSMedicalHx.MedicalHxRow dr in dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName].Rows)
                    {
                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                        if (!string.IsNullOrEmpty(model.MedicalHxDate))
                        {
                            dr.MedicalHxDate = MDVUtility.ToDateTime(model.MedicalHxDate);
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.MedicalHxDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.MedicalHxComments))
                        {
                            dr.Comments = MDVUtility.ToStr(model.MedicalHxComments);
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.CommentsColumn] = DBNull.Value;
                        }
                        if (!model.IsRCPneumococcal != null)
                        {
                            dr.IsRCPneumococcal = MDVUtility.ToBool(model.IsRCPneumococcal);
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.IsRCPneumococcalColumn] = DBNull.Value;
                        }
                        if (!model.IsRCInfluenza != null)
                        {
                            dr.IsRCInfluenza = MDVUtility.ToBool(model.IsRCInfluenza);
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.IsRCInfluenzaColumn] = DBNull.Value;
                        }
                        if (model.RCPneumococcalDate != null)
                        {
                            dr.RCPneumococcalDate = MDVUtility.ToDateTime(model.RCPneumococcalDate);
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.RCPneumococcalDateColumn] = DBNull.Value;
                        }
                        if (model.RCInfluenzaDate != null)
                        {
                            dr.RCInfluenzaDate = MDVUtility.ToDateTime(model.RCInfluenzaDate);
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.RCInfluenzaDateColumn] = DBNull.Value;
                        }
                        dr.bUnremarkable = model.MedicalHxUnremarkable.ToLower() == "true" ? true : false;

                        //dr.IsActive = true;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }
                    var diseaseID = "";
                    if (lstDiseaseObject.Count > 0)
                    {
                        int diseasesAdded = dsMedicalHx.Tables[dsMedicalHx.MedicalHx_Disease.TableName].Rows.Count;
                        string responseMedicalHxDisease = insertUpdateDisease(MedicalHxId, lstDiseaseObject, model.PatientId, diseasesAdded, model.FromClinicalSide);
                        diseaseID = responseMedicalHxDisease;
                    }
                    //Start 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                    //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MedicalHxId);

                    //End 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                    #region Database Updation
                    if (dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName].Rows.Count > 0)
                    {
                        BLObject<DSMedicalHx> objUpdate = BLLClinicalObj.UpdateMedicalHx(dsMedicalHx, model.PatientId);



                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MedicalHxId);


                        if (objUpdate.Data != null)
                        {
                            DSHistorySummary dsHistorySummarySoap = null;
                            BLObject<DSHistorySummary> objSummary;
                            objSummary = BLLClinicalObj.loadHxLog(MedicalHxId, "MedicalHx", "Current", 1, 10);
                            dsHistorySummarySoap = objSummary.Data;

                            var SoapText = string.Empty;
                            var IsCreatedOrModified = string.Empty;
                            var LastUpdated = string.Empty;

                            if (dsHistorySummarySoap != null && dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows.Count > 0)
                            {
                                var Medicaldr = dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows[0];
                                SoapText =  MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.SoapTextColumn.ColumnName]);
                                IsCreatedOrModified = MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.ActionColumn.ColumnName]);
                                LastUpdated = string.Concat(MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.ModifiedOnColumn.ColumnName]), " ", MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.ModifiedByColumn.ColumnName]));
                            }


                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,
                                diseaseId = diseaseID,
                                SoapText = SoapText,
                                MedicalHxId = MedicalHxId,
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

        public string updateMedicalHx(MedicalHxModel model, long noteId)
        {
            try
            {
                long MedicalHxId = MDVUtility.ToInt64(model.MedicalHxId);
                if (MedicalHxId > 0)
                {
                    DSMedicalHx dsMedicalHx = new DSMedicalHx();
                    BLObject<DSMedicalHx> obj = BLLClinicalObj.LoadMedicalHx(MDVUtility.ToInt64(model.PatientId), MedicalHxId);
                    dsMedicalHx = obj.Data;
                    foreach (DSMedicalHx.MedicalHxRow dr in dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName].Rows)
                    {
                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                        if (!string.IsNullOrEmpty(model.MedicalHxDate))
                        {
                            dr.MedicalHxDate = MDVUtility.ToDateTime(model.MedicalHxDate);
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.MedicalHxDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.MedicalHxComments))
                        {
                            dr.Comments = MDVUtility.ToStr(model.MedicalHxComments);
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.CommentsColumn] = DBNull.Value;
                        }
                        if (!model.IsRCPneumococcal != null)
                        {
                            dr.IsRCPneumococcal = MDVUtility.ToBool(model.IsRCPneumococcal);
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.IsRCPneumococcalColumn] = DBNull.Value;
                        }
                        if (!model.IsRCInfluenza != null)
                        {
                            dr.IsRCInfluenza = MDVUtility.ToBool(model.IsRCInfluenza);
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.IsRCInfluenzaColumn] = DBNull.Value;
                        }
                        if (model.RCPneumococcalDate != null)
                        {
                            dr.RCPneumococcalDate = MDVUtility.ToDateTime(model.RCPneumococcalDate);
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.RCPneumococcalDateColumn] = DBNull.Value;
                        }
                        if (model.RCInfluenzaDate != null)
                        {
                            dr.RCInfluenzaDate = MDVUtility.ToDateTime(model.RCInfluenzaDate);
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.RCInfluenzaDateColumn] = DBNull.Value;
                        }
                        dr.bUnremarkable = model.MedicalHxUnremarkable.ToLower() == "true" ? true : false;

                        if (noteId > 0)
                        {
                            dr.NoteId = noteId;
                        }
                        else
                        {
                            dr[dsMedicalHx.MedicalHx.NoteIdColumn] = DBNull.Value;
                        }

                        //dr.IsActive = true;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }
                    var diseaseID = "";
                    if (model.MedicalDiseaseList != null && model.MedicalDiseaseList.Count > 0)
                    {
                        if (model.MedicalDiseaseList != null && model.MedicalDiseaseList.Count > 0)
                        {
                            int diseasesAdded = dsMedicalHx.Tables[dsMedicalHx.MedicalHx_Disease.TableName].Rows.Count;
                            List<object> diseaseList = new List<object>(model.MedicalDiseaseList);
                            string responseMedicalHxDisease = insertUpdateDisease(MedicalHxId, diseaseList, model.PatientId, diseasesAdded);
                            diseaseID = responseMedicalHxDisease;
                        }                       
                    }
                    //Start 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                    //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MedicalHxId);

                    //End 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                    #region Database Updation
                    if (dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName].Rows.Count > 0)
                    {
                        BLObject<DSMedicalHx> objUpdate = BLLClinicalObj.UpdateMedicalHx(dsMedicalHx, model.PatientId);



                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MedicalHxId);


                        if (objUpdate.Data != null)
                        {
                            DSHistorySummary dsHistorySummarySoap = null;
                            BLObject<DSHistorySummary> objSummary;
                            objSummary = BLLClinicalObj.loadHxLog(MedicalHxId, "MedicalHx", "Current", 1, 10);
                            dsHistorySummarySoap = objSummary.Data;

                            var SoapText = string.Empty;
                            var IsCreatedOrModified = string.Empty;
                            var LastUpdated = string.Empty;

                            if (dsHistorySummarySoap != null && dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows.Count > 0)
                            {
                                var Medicaldr = dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows[0];
                                SoapText = MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.SoapTextColumn.ColumnName]);
                                IsCreatedOrModified = MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.ActionColumn.ColumnName]);
                                LastUpdated = string.Concat(MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.ModifiedOnColumn.ColumnName]), " ", MDVUtility.ToStr(Medicaldr[dsHistorySummarySoap.HxLog.ModifiedByColumn.ColumnName]));
                            }


                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,
                                diseaseId = diseaseID,
                                SoapText = SoapText,
                                MedicalHxId = MedicalHxId,
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

        #region fillMedicalHx
        // Author:  Muhammad Arshad
        // Created Date: 08/01/2016
        //OverView: This function will handle fill of MedicalHx
        public string fillMedicalHx(MedicalHxModel model, Int64 MedicalHxId, string MedicalHxType)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)) && MedicalHxId == 0)
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
                    DSMedicalHx dsMedicalHx = null;
                    BLObject<DSMedicalHx> obj = BLLClinicalObj.LoadMedicalHx(MDVUtility.ToInt64(model.PatientId), MedicalHxId, MDVUtility.ToInt64(model.DiseaseId), MedicalHxType, "1", "");
                    dsMedicalHx = obj.Data;
                    if (dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName].Rows[0];
                        var MedicalHxkeyValues = new Dictionary<string, string>
                        {
                            { "MedicalHxDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx.MedicalHxDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsMedicalHx.MedicalHx.MedicalHxDateColumn.ColumnName]).ToShortDateString()},
                            { "MedicalHxId",  MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx.MedicalHxIdColumn.ColumnName])},
                            { "MedicalHxUnremarkable", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx.bUnremarkableColumn.ColumnName])},
                            { "MedicalHxComments", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx.CommentsColumn.ColumnName])},
                            { "MedicalHxSoapText", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx.SoapTextColumn.ColumnName])},
                            { "IsCreatedOrModified", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx.CreatedOnColumn.ColumnName]) == MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx.ModifiedOnColumn.ColumnName]) ? "Added Medical Hx": "Updated Medical Hx"},
                            { "LastUpdated",string.Concat( Convert.ToDateTime(dr[dsMedicalHx.MedicalHx.ModifiedOnColumn.ColumnName]).ToString("MM-dd-yyyy hh:mm:ss tt")," " , MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx.ModifiedByColumn.ColumnName]))}
                        };

                        //Start 13/01/2016 Muhammad Irfan Fill Diseases in MedicalHx
                        List<Dictionary<string, string>> lstDisease = new List<Dictionary<string, string>>();
                        var DiseaseHxkeyValues = new Dictionary<string, string> { { "", "" } };
                        if (MDVUtility.ToInt64(model.DiseaseId) > 0 && dsMedicalHx.Tables[dsMedicalHx.MedicalHx_Disease.TableName].Rows.Count > 0)
                        {
                            //DSMedicalHx dsDisease = new DSMedicalHx();

                            //BLObject<DSMedicalHx> objDisease = BLLClinicalObj.loadMedicalHxDisease(MedicalHxId, MDVUtility.ToInt64(model.DiseaseId));
                            //dsDisease = obj.Data;
                            DSMedicalHx.MedicalHx_DiseaseRow[] arrToComponentRows = (DSMedicalHx.MedicalHx_DiseaseRow[])dsMedicalHx.MedicalHx_Disease.Select(MDVUtility.ToStr(dsMedicalHx.MedicalHx_Disease.DiseaseIdColumn.ColumnName) + "=" + MDVUtility.ToStr(model.DiseaseId) + "");
                            DataRow drDisease = (DataRow)arrToComponentRows[0];

                            DiseaseHxkeyValues = new Dictionary<string, string>
                        {
                            { "MedicalDiseaseFromDate",  String.IsNullOrEmpty(MDVUtility.ToStr(arrToComponentRows[0][dsMedicalHx.MedicalHx_Disease.FromDateColumn.ColumnName]))?"": MDVUtility.GetDateMMDDYYY(arrToComponentRows[0][dsMedicalHx.MedicalHx_Disease.FromDateColumn.ColumnName].ToString())},
                            { "MedicalDiseaseToDate",  String.IsNullOrEmpty(MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.ToDateColumn.ColumnName]))?"": MDVUtility.GetDateMMDDYYY(drDisease[dsMedicalHx.MedicalHx_Disease.ToDateColumn.ColumnName].ToString())},
                            { "MedicalDiseaseOnset",  MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.OnsetColumn.ColumnName])},
                            { "MedicalDiseaseDurationLength", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.DurationLengthColumn.ColumnName])},
                            { "MedicalDiseaseDurationPeriod", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.DurationPeriodIdColumn.ColumnName])},
                            { "CPTCode",MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.CPTCodeColumn.ColumnName]) != ""? (MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.CPTCodeColumn.ColumnName])+" - "+MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.CPTCodeDescriptionColumn.ColumnName])):""},
                            { "CPTCodeId", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.CPTCodeColumn.ColumnName])},
                            { "CPTDescription", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.CPTCodeDescriptionColumn.ColumnName])},

                            { "CPTSNOMEDCodeId", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.CPTSNOMEDIDColumn.ColumnName])},
                            { "CPTSNOMEDDescription", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.CPTSNOMEDDescriptionColumn.ColumnName])},





                            { "MedicalDiseaseTestResult", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.TestResultIdColumn.ColumnName])},
                            { "MedicalDiseaseStatus", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.StatusIdColumn.ColumnName])},
                            { "MedicalDiseaseLocation", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.LocationColumn.ColumnName])},
                            { "MedicalDiseaseSeverity", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.SeverityIdColumn.ColumnName])},
                            { "MedicalDiseasePattern", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.PatternIdColumn.ColumnName])},
                            { "MedicalDiseaseAgggravatedBy", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.AggravatedByIdColumn.ColumnName])},
                            { "MedicalDiseaseComments", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.CommentsColumn.ColumnName])}
                        };

                        }
                        //End 13/01/2016 Muhammad Irfan Fill Diseases in MedicalHx


                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            MedicalHxFill_JSON = js.Serialize(MedicalHxkeyValues),
                            DiseaseFill_JSON = js.Serialize(DiseaseHxkeyValues),
                            MedicalHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsMedicalHx.Tables[dsMedicalHx.MedicalHx_Disease.TableName]),
                            //TobaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
                            //AlcoholHxFill_JSON = js.Serialize(lstAlcoholHx),
                            //DrugAbuseFill_JSON = js.Serialize(lstDrugAbuse),
                            //SexualHxFill_JSON = js.Serialize(lstSexualHx),
                            //OccupationHxFill_JSON = js.Serialize(lstMiscHxOccupationHx),
                            //MedicalHxLoad_JSON = MDVUtility.JSON_DataTable(dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            MedicalHxFill_JSON = "[]",
                            DiseaseFill_JSON = "[]",
                            MedicalHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsMedicalHx.Tables[dsMedicalHx.MedicalHx_Disease.TableName]),
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
        public string fillMedicalHxNative(MedicalHxModel model, Int64 PatientId, string RequestStatus,Int64 DiseaseId=0)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)) && PatientId == 0)
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
                    DSMedicalHx dsMedicalHx = null;
                    BLObject<DSMedicalHx> obj = BLLClinicalObj.LoadMedicalHxNative(MDVUtility.ToInt64(model.PatientId), RequestStatus, DiseaseId);
                    dsMedicalHx = obj.Data;
                    if (dsMedicalHx.Tables[dsMedicalHx.MedicalHx_Disease.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsMedicalHx.Tables[dsMedicalHx.MedicalHx_Disease.TableName].Rows[0];
                        string FromDate = MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.FromDateColumn.ColumnName]));
                        string ToDate = MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.ToDateColumn.ColumnName]));
                        var MedicalHxkeyValues = new Dictionary<string, string>
                        {
                           // { "MedicalHxDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx.MedicalHxDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsMedicalHx.MedicalHx.MedicalHxDateColumn.ColumnName]).ToShortDateString()},
                            { "MedicalHxId",  MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.MedicalHxIdColumn.ColumnName])},
                            { "FreeTextICD", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.FreeTextICDColumn.ColumnName])},
                            { "MedicalDiseaseFromDate", FromDate },
                            { "MedicalDiseaseToDate", ToDate},
                            { "MedicalDiseaseOnset", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.OnsetColumn.ColumnName])},
                            { "MedicalDiseaseDurationLength",MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.DurationLengthColumn.ColumnName])},
                            { "MedicalDiseaseDurationPeriod", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.DurationPeriodIdColumn.ColumnName])},
                            { "CPTDescription", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.FreeTextCPTColumn.ColumnName])},
                            { "MedicalDiseaseTestResult", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.TestResultIdColumn.ColumnName])},
                            { "MedicalDiseaseStatus",MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.StatusIdColumn.ColumnName])},
                            { "MedicalDiseaseLocation", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.LocationColumn.ColumnName])},
                            { "MedicalDiseaseSeverity", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.SeverityIdColumn.ColumnName])},
                            { "MedicalDiseasePattern", MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.PatternIdColumn.ColumnName])},
                            { "MedicalDiseaseAgggravatedBy",MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.AggravatedByIdColumn.ColumnName])},
                            { "MedicalDiseaseComments",MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.CommentsColumn.ColumnName])},
                            { "CreatedBy",MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.CreatedByColumn.ColumnName])},
                            { "CreatedOn",MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.CreatedOnColumn.ColumnName])},
                            { "ModifiedBy",MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.ModifiedByColumn.ColumnName])},
                            { "ModifiedOn",MDVUtility.ToStr(dr[dsMedicalHx.MedicalHx_Disease.ModifiedOnColumn.ColumnName])},

                        };

                        //Start 13/01/2016 Muhammad Irfan Fill Diseases in MedicalHx
                        List<Dictionary<string, string>> lstDisease = new List<Dictionary<string, string>>();
                        var DiseaseHxkeyValues = new Dictionary<string, string> { { "", "" } };
                        if (MDVUtility.ToInt64(model.DiseaseId) != 0 && dsMedicalHx.Tables[dsMedicalHx.MedicalHx_Disease.TableName].Rows.Count > 0)
                        {
                            //DSMedicalHx dsDisease = new DSMedicalHx();

                            //BLObject<DSMedicalHx> objDisease = BLLClinicalObj.loadMedicalHxDisease(MedicalHxId, MDVUtility.ToInt64(model.DiseaseId));
                            //dsDisease = obj.Data;
                            DSMedicalHx.MedicalHx_DiseaseRow[] arrToComponentRows = (DSMedicalHx.MedicalHx_DiseaseRow[])dsMedicalHx.MedicalHx_Disease.Select(MDVUtility.ToStr(dsMedicalHx.MedicalHx_Disease.DiseaseIdColumn.ColumnName) + "=" + MDVUtility.ToStr(model.DiseaseId) + "");
                            DataRow drDisease = (DataRow)arrToComponentRows[0];

                            DiseaseHxkeyValues = new Dictionary<string, string>
                        {



                               { "DiseaseId", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.DiseaseIdColumn.ColumnName])},
                                { "ICDID", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.ICDIDColumn.ColumnName])},
                            { "ICD10Code", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.ICD10CodeColumn.ColumnName])},
                            { "ICD10CodeDescription", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.ICD10CodeDescriptionColumn.ColumnName])},

                            { "ICD9Code", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.ICD9CodeColumn.ColumnName])},
                            { "ICD9CodeDescription", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.ICD9CodeDescriptionColumn.ColumnName])},
                              { "SNOMEDID", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.SNOMEDIDColumn.ColumnName])},
                               { "SNOMEDDescription", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.SNOMEDDescriptionColumn.ColumnName])},


                                 { "CreatedBy", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.CreatedByColumn.ColumnName])},
                            { "CreatedOn", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.CreatedOnColumn.ColumnName])},
                              { "ModifiedBy", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.ModifiedByColumn.ColumnName])},
                               { "ModifiedOn", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.ModifiedOnColumn.ColumnName])},


                            { "MedicalDiseaseStatus", MDVUtility.ToStr(drDisease[dsMedicalHx.MedicalHx_Disease.StatusIdColumn.ColumnName])}
                           
                        };

                        }

                        string ChangedColumns = "";

                        DataColumn dc = dsMedicalHx.Tables["MedicalHx_NativeChangeset"].Columns[2] as DataColumn;
                        ChangedColumns = string.Join(", ", dsMedicalHx.Tables["MedicalHx_NativeChangeset"].Rows.OfType<DataRow>().Select(r => r[dc]));
                        //End 13/01/2016 Muhammad Irfan Fill Diseases in MedicalHx


                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ChangedColumns,
                            MedicalHxFill_JSON = js.Serialize(MedicalHxkeyValues),
                            DiseaseFill_JSON = js.Serialize(DiseaseHxkeyValues),
                            MedicalHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsMedicalHx.Tables[dsMedicalHx.MedicalHx_Disease.TableName]),
                         
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            MedicalHxFill_JSON = "[]",
                            DiseaseFill_JSON = "[]",
                            MedicalHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsMedicalHx.Tables[dsMedicalHx.MedicalHx_Disease.TableName]),
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



        #region 'Attachment/Detachment of Soical History with Progress note'
        /// <summary>
        /// This Function will detach MedicalHx from notes
        /// </summary>
        /// <param name="MedicalHxId"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string detach_MedicalHx_From_Notes(long MedicalHxId, long NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(MedicalHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachMedicalHxFromNotes(MedicalHxId, NotesId);
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
        /// This Function will attach MedicalHx to notes
        /// </summary>
        /// <param name="MedicalHxId"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string attach_MedicalHx_With_Notes(string MedicalHxId, long NotesId)
        {
            try
            {
                DSMedicalHx dsMedicalHx = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(MedicalHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<DSMedicalHx> obj = BLLClinicalObj.attachMedicalHxWithNotes(MedicalHxId, NotesId);
                    if (obj.Data != null)
                    {
                        dsMedicalHx = obj.Data;
                        var response = new
                        {
                            status = true,
                            MedicalHxTotalCount = dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName].Rows.Count,
                            MedicalHxCount = dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName].Rows.Count,
                            MedicalHxLoad_JSON = MDVUtility.JSON_DataTable(dsMedicalHx.Tables[dsMedicalHx.MedicalHx.TableName]),
                            //    MedicalHxHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsMedicalHx.Tables[dsMedicalHx.MedicalHxHistory.TableName]),
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
        //Start//14/01/2016//Ahmad Raza//Logic implimented for MedicalHx Soap Text
        internal string insertUpdateMedicalSoapText(DSMedicalHx dsMedicalHistory, MedicalHxDiseaseModel modelObj, int diseasesAdded)
        {
            string SoapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                string procedureStr = string.Empty;
                if (!string.IsNullOrEmpty(modelObj.CPTDescription))
                {
                    
                    if (diseasesAdded > 0)
                    {
                        procedureStr = "The patient also underwent " + modelObj.CPTDescription + ", based on the following assessment: ";                        
                    }
                    else
                    {
                        procedureStr = "The patient underwent " + modelObj.CPTDescription + ", based on the following assessment: ";
                    }

                    sb.Append("<div id='medicalHistory_" + modelObj.MedicalHxId + "' title='Medical'  name='Medical Hx'>");
                    sb.Append(procedureStr + (string.IsNullOrEmpty(modelObj.ICD9CodeDescription) ? ((string.IsNullOrEmpty(modelObj.FreeTextICD)) ? "" : modelObj.FreeTextICD) : modelObj.ICD9CodeDescription) + (string.IsNullOrEmpty(modelObj.MedicalDiseaseStatusText) ? "" : " (" + modelObj.MedicalDiseaseStatusText + ")"));
                    sb.Append((string.IsNullOrEmpty(modelObj.MedicalDiseaseFromDate) ? "" : ", from " + modelObj.MedicalDiseaseFromDate) + (string.IsNullOrEmpty(modelObj.MedicalDiseaseToDate) ? "." : " to " + modelObj.MedicalDiseaseToDate + ".") +
                              (string.IsNullOrEmpty(modelObj.MedicalDiseaseTestResultText) ? "" : " The Test Result is " + modelObj.MedicalDiseaseTestResultText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseOnset) ? "" : " Onset is " + modelObj.MedicalDiseaseOnset + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseDurationLength) ? "" : " Duration is " + modelObj.MedicalDiseaseDurationLength + " " + modelObj.MedicalDiseaseDurationPeriodText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseSeverityText) ? "" : " Severity is " + modelObj.MedicalDiseaseSeverityText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseasePatternText) ? "" : " Pattern is " + modelObj.MedicalDiseasePatternText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseAgggravatedByText) ? "" : " Aggravated  by " + modelObj.MedicalDiseaseAgggravatedByText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseLocation) ? "" : " Location is " + modelObj.MedicalDiseaseLocation + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseComments) ? "" : " Comments: " + modelObj.MedicalDiseaseComments + ".") + "</div>");               
                }
                else
                {
                   sb.Append("<div id='medicalHistory_" + modelObj.MedicalHxId + "' title='Medical'  name='Medical Hx'>");
                    sb.Append((string.IsNullOrEmpty(modelObj.ICD9CodeDescription) ? ((string.IsNullOrEmpty(modelObj.FreeTextICD)) ? "" : modelObj.FreeTextICD) : modelObj.ICD9CodeDescription) + (string.IsNullOrEmpty(modelObj.MedicalDiseaseStatusText) ? ":" : " (" + modelObj.MedicalDiseaseStatusText + "):"));
                    sb.Append((string.IsNullOrEmpty(modelObj.MedicalDiseaseTestResultText) ? "" : " The Test Result is " + modelObj.MedicalDiseaseTestResultText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseOnset) ? "" : " Onset is " + modelObj.MedicalDiseaseOnset + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseDurationLength) ? "" : " Duration is " + modelObj.MedicalDiseaseDurationLength + " " + modelObj.MedicalDiseaseDurationPeriodText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseSeverityText) ? "" : " Severity is " + modelObj.MedicalDiseaseSeverityText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseasePatternText) ? "" : " Pattern is " + modelObj.MedicalDiseasePatternText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseAgggravatedByText) ? "" : " Aggravated  by " + modelObj.MedicalDiseaseAgggravatedByText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseLocation) ? "" : " Location is " + modelObj.MedicalDiseaseLocation + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseComments) ? "" : " Comments: " + modelObj.MedicalDiseaseComments + ".") + "</div>");              
                }
                ////string procedureStr = "";
                //string colonStr = "";
                //if (!string.IsNullOrEmpty(modelObj.CPTDescription) && diseasesAdded > 0)
                //{
                //    procedureStr = "The patient also underwent " + modelObj.CPTDescription + ", based on the following assessment: ";
                //    colonStr = string.Empty;
                //}
                //if (!string.IsNullOrEmpty(modelObj.CPTCodeId) && diseasesAdded == 0)
                //{
                //    procedureStr = "The patient underwent " + modelObj.CPTDescription + ", based on the following assessment: ";
                //    colonStr = string.Empty;
                //}
                //else
                //{
                //    procedureStr = string.Empty;
                //    colonStr = ":";
                //}
                //sb.Append("<div id='medicalHistory_" + modelObj.MedicalHxId + "' title='Medical'  name='Medical Hx'>");
                //sb.Append(procedureStr + (string.IsNullOrEmpty(modelObj.ICD9CodeDescription) ? ((string.IsNullOrEmpty(modelObj.FreeTextICD)) ? "" : modelObj.FreeTextICD) : modelObj.ICD9CodeDescription) + (string.IsNullOrEmpty(modelObj.MedicalDiseaseStatusText) ? colonStr : " (" + modelObj.MedicalDiseaseStatusText + ")" + colonStr));
                //sb.Append((!string.IsNullOrEmpty(procedureStr) ? (string.IsNullOrEmpty(modelObj.MedicalDiseaseFromDate) ? "" : ", from " + modelObj.MedicalDiseaseFromDate) + (string.IsNullOrEmpty(modelObj.MedicalDiseaseToDate) ? "" : " to " + modelObj.MedicalDiseaseToDate + ".") : "") +
                //          (string.IsNullOrEmpty(modelObj.MedicalDiseaseTestResultText) ? "" : " The Test Result is " + modelObj.MedicalDiseaseTestResultText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseOnset) ? "" : " Onset is " + modelObj.MedicalDiseaseOnset + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseDurationLength) ? "" : " Duration is " + modelObj.MedicalDiseaseDurationLength + " " + modelObj.MedicalDiseaseDurationPeriodText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseSeverityText) ? "" : " Severity is " + modelObj.MedicalDiseaseSeverityText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseasePatternText) ? "" : " Pattern is " + modelObj.MedicalDiseasePatternText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseAgggravatedByText) ? "" : " Aggravated  by " + modelObj.MedicalDiseaseAgggravatedByText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseLocation) ? "" : " Location is " + modelObj.MedicalDiseaseLocation + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseComments) ? "" : " Comments: " + modelObj.MedicalDiseaseComments + ".") + "</div>");


                //sb.Append((string.IsNullOrEmpty(modelObj.MedicalDiseaseFromDate) ? "" : ", from " + modelObj.MedicalDiseaseFromDate) + (string.IsNullOrEmpty(modelObj.MedicalDiseaseToDate) ? "" : " to " + modelObj.MedicalDiseaseToDate + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseTestResultText) ? "" : " The Test Result is " + modelObj.MedicalDiseaseTestResultText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseOnset) ? "" : " Onset is " + modelObj.MedicalDiseaseOnset + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseDurationLength) ? "" : " Duration is " + modelObj.MedicalDiseaseDurationLength + " " + modelObj.MedicalDiseaseDurationPeriodText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseSeverityText) ? "" : " Severity is " + modelObj.MedicalDiseaseSeverityText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseasePatternText) ? "" : " Pattern is " + modelObj.MedicalDiseasePatternText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseAgggravatedByText) ? "" : " Aggravated  by " + modelObj.MedicalDiseaseAgggravatedByText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseLocation) ? "" : " Location is " + modelObj.MedicalDiseaseLocation + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseComments) ? "" : " Comments: " + modelObj.MedicalDiseaseComments + ".") + "</div>");

                //sb.Append("<div id='medicalHistory_" + modelObj.MedicalHxId + "' title='Medical'  name='Medical Hx'>");
                //sb.Append((string.IsNullOrEmpty(modelObj.CPTCodeId) ? "" : " Patient underwent " + modelObj.CPTDescription + ", ") + (string.IsNullOrEmpty(modelObj.ICD9CodeDescription) ? ((string.IsNullOrEmpty(modelObj.FreeTextICD)) ? "" : "based on the following assessment: " + modelObj.FreeTextICD) : " based on the following assessment: " + modelObj.ICD9CodeDescription) + " " + (string.IsNullOrEmpty(modelObj.MedicalDiseaseStatusText) ? "" : "(" + modelObj.MedicalDiseaseStatusText + ")"));
                //sb.Append((string.IsNullOrEmpty(modelObj.MedicalDiseaseFromDate) ? "" : ", From " + modelObj.MedicalDiseaseFromDate) + (string.IsNullOrEmpty(modelObj.MedicalDiseaseToDate) ? "" : " To " + modelObj.MedicalDiseaseToDate + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseTestResultText) ? "" : " The test result is " + modelObj.MedicalDiseaseTestResultText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseOnset) ? "" : " Onset is " + modelObj.MedicalDiseaseOnset + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseDurationLength) ? "" : " Duration is " + modelObj.MedicalDiseaseDurationLength + " " + modelObj.MedicalDiseaseDurationPeriodText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseSeverityText) ? "" : " Severity is " + modelObj.MedicalDiseaseSeverityText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseasePatternText) ? "" : " Pattern is " + modelObj.MedicalDiseasePatternText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseAgggravatedByText) ? "" : " Aggravated  by " + modelObj.MedicalDiseaseAgggravatedByText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseLocation) ? "" : " Location is " + modelObj.MedicalDiseaseLocation + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseComments) ? "" : " Comments: " + modelObj.MedicalDiseaseComments + ".") + "</div>");
            }
            else
                if (dsMedicalHistory.MedicalHx_Disease != null && dsMedicalHistory.MedicalHx_Disease.Rows.Count > 0)
                {
                    foreach (var item in dsMedicalHistory.MedicalHx_Disease)
                    {
                        sb.Append("<div id='medicalHistory_" + modelObj.MedicalHxId + "' title='Medical'  name='Medical Hx'><strong>Medical: </strong>");
                        sb.Append((string.IsNullOrEmpty(modelObj.CPTCodeId) ? "" : "Patient underwent " + modelObj.CPTDescription + ", ") + (string.IsNullOrEmpty(modelObj.ICD9CodeDescription) ? ((string.IsNullOrEmpty(modelObj.FreeTextICD)) ? "" : "based on the following assessment: " + modelObj.FreeTextICD) : " based on the following assessment: " + modelObj.ICD9CodeDescription) + " " + (string.IsNullOrEmpty(modelObj.MedicalDiseaseStatusText) ? "" : "(" + modelObj.MedicalDiseaseStatusText + ")"));
                        sb.Append((string.IsNullOrEmpty(modelObj.MedicalDiseaseFromDate) ? "" : " From" + modelObj.MedicalDiseaseFromDate) + (string.IsNullOrEmpty(modelObj.MedicalDiseaseToDate) ? "" : " to" + modelObj.MedicalDiseaseToDate + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseTestResultText) ? "" : " The test result is " + modelObj.MedicalDiseaseTestResultText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseOnset) ? "" : " Onset is" + modelObj.MedicalDiseaseOnset + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseDurationLength) ? "" : " Duration is" + modelObj.MedicalDiseaseDurationLength + " " + modelObj.MedicalDiseaseDurationPeriodText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseSeverityText) ? "" : " Severity is " + modelObj.MedicalDiseaseSeverityText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseasePatternText) ? "" : " Pattern is " + modelObj.MedicalDiseasePatternText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseAgggravatedByText) ? "" : " Aggravated  by" + modelObj.MedicalDiseaseAgggravatedByText + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseLocation) ? "" : " Location is" + modelObj.MedicalDiseaseLocation + ".") + (string.IsNullOrEmpty(modelObj.MedicalDiseaseComments) ? "" : " Comments: " + modelObj.MedicalDiseaseComments + ".") + "</div>");
                    }
                }

                else
                {
                    return string.Empty;
                }
            return sb.ToString();
        }
        //End//14/01/2016//Ahmad Raza//Logic implimented for MedicalHx Soap Text
        #endregion


        #region"Disease Functions"
        /* Start 12/01/2016 Muhammad Irfan Save/Update Diseases in MedicalHx */
        private string insertUpdateDisease(long MedicalHxId, List<object> lstDiseaseObject, string patientId, int diseasesAdded,string FromClinicalSide="")
        {
            #region Disease
            DSMedicalHx dsDisease = new DSMedicalHx();
            DSMedicalHx dsDiseaseData = new DSMedicalHx();

            List<MedicalHxDiseaseModel> lstDisease = lstDiseaseObject.OfType<MedicalHxDiseaseModel>().ToList();
            bool isFirstChild = false;
            BLObject<DSMedicalHx> objTobacco=new BLObject<DSMedicalHx>();
            if (!string.IsNullOrEmpty(FromClinicalSide) && FromClinicalSide == "1")
            {
                objTobacco = BLLClinicalObj.loadMedicalHxDisease(MedicalHxId, MDVUtility.ToInt64(lstDisease[0].DiseaseId));
            }
            else
            {
                objTobacco = BLLClinicalObj.loadMedicalHxDisease(MedicalHxId, 0);
            }
            dsDisease = objTobacco.Data;

            foreach (MedicalHxDiseaseModel CurrentModel in lstDisease)
            {
                Int32 currentDiseaseId = MDVUtility.ToInt32(CurrentModel.DiseaseId);
                if (CurrentModel.AddedFromMobileApp == "1")
                    currentDiseaseId = currentDiseaseId == 0 ? -1 : currentDiseaseId;
                if (currentDiseaseId != null)
                {
                    DSMedicalHx.MedicalHx_DiseaseRow RowDisease = null;
                    DSMedicalHx.MedicalHx_DiseaseRow[] arrDiseases = (DSMedicalHx.MedicalHx_DiseaseRow[])dsDisease.MedicalHx_Disease.Select(dsDisease.MedicalHx_Disease.DiseaseIdColumn.ColumnName + "=" + currentDiseaseId);

                    if (arrDiseases != null && arrDiseases.Length > 0)
                    {
                        RowDisease = arrDiseases[0];
                    }
                    else
                    {
                        RowDisease = dsDisease.MedicalHx_Disease.NewMedicalHx_DiseaseRow();
                        RowDisease.DiseaseId = currentDiseaseId;              
                    }

                    if (RowDisease != null)
                    {
                        bool isValueDifferent = false;
                        bool istoUpdateRow = false;
                        if (dsDisease.MedicalHx_Disease.Rows.Count < 1)
                        {
                            RowDisease.DiseaseId = currentDiseaseId;
                        }
                        

                        RowDisease.MedicalHxId = MedicalHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.FreeTextICD))
                        {
                            RowDisease.FreeTextICD = MDVUtility.ToStr(CurrentModel.FreeTextICD);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.FreeTextICDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD9Code))
                        {
                            RowDisease.ICD9Code = MDVUtility.ToStr(CurrentModel.ICD9Code);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.ICD9CodeColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.ICD9CodeDescription))
                        {
                            RowDisease.ICD9CodeDescription = MDVUtility.ToStr(CurrentModel.ICD9CodeDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.ICD9CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10Code))
                        {
                            RowDisease.ICD10Code = MDVUtility.ToStr(CurrentModel.ICD10Code);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.ICD10CodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10CodeDescription))
                        {
                            RowDisease.ICD10CodeDescription = MDVUtility.ToStr(CurrentModel.ICD10CodeDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.ICD10CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDCodeId))
                        {
                            RowDisease.CPTSNOMEDID = MDVUtility.ToStr(CurrentModel.CPTSNOMEDCodeId);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.CPTSNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDDescription))
                        {
                            RowDisease.CPTSNOMEDDescription = MDVUtility.ToStr(CurrentModel.CPTSNOMEDDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.CPTSNOMEDDescriptionColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDID))
                        {
                            RowDisease.SNOMEDID = MDVUtility.ToStr(CurrentModel.SNOMEDID);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.SNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDDescription))
                        {
                            RowDisease.SNOMEDDescription = MDVUtility.ToStr(CurrentModel.SNOMEDDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.SNOMEDDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCode))
                        {
                            RowDisease.LexiCode = MDVUtility.ToStr(CurrentModel.LexiCode);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.LexiCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCodeDescription))
                        {
                            RowDisease.LexiCodeDescription = MDVUtility.ToStr(CurrentModel.LexiCodeDescription);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.LexiCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.MedicalDiseaseFromDate))
                        {
                            RowDisease.FromDate = MDVUtility.ToDateTime(CurrentModel.MedicalDiseaseFromDate);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.FromDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.MedicalDiseaseToDate))
                        {
                            RowDisease.ToDate = MDVUtility.ToDateTime(CurrentModel.MedicalDiseaseToDate);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.ToDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.MedicalDiseaseOnset))
                        {
                            RowDisease.Onset = MDVUtility.ToStr(CurrentModel.MedicalDiseaseOnset);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.OnsetColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.MedicalDiseaseDurationLength))
                        {
                            RowDisease.DurationLength = MDVUtility.ToInt16(CurrentModel.MedicalDiseaseDurationLength);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.DurationLengthColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.MedicalDiseaseDurationPeriod))
                        {
                            RowDisease.DurationPeriodId = MDVUtility.ToInt16(CurrentModel.MedicalDiseaseDurationPeriod);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.DurationPeriodIdColumn] = DBNull.Value;
                        }
                        if (string.IsNullOrEmpty(CurrentModel.CPTCode))
                        {

                            RowDisease[dsDisease.MedicalHx_Disease.CPTCodeColumn] = DBNull.Value;

                            RowDisease[dsDisease.MedicalHx_Disease.CPTCodeDescriptionColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(CurrentModel.CPTDescription))
                            {
                                RowDisease.FreeTextCPT = MDVUtility.ToStr(CurrentModel.CPTDescription);
                                RowDisease.CPTCodeDescription = MDVUtility.ToStr(CurrentModel.CPTDescription);
                            } else
                            {
                                RowDisease[dsDisease.MedicalHx_Disease.CPTCodeDescriptionColumn] = DBNull.Value;
                                RowDisease[dsDisease.MedicalHx_Disease.FreeTextCPTColumn] = DBNull.Value;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(CurrentModel.CPTCodeId))
                            {
                                RowDisease.CPTCode = MDVUtility.ToStr(CurrentModel.CPTCodeId);
                            }
                            else
                            {
                                RowDisease[dsDisease.MedicalHx_Disease.CPTCodeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.CPTDescription))
                            {
                                RowDisease.CPTCodeDescription = MDVUtility.ToStr(CurrentModel.CPTDescription);
                            }
                            else
                            {
                                RowDisease[dsDisease.MedicalHx_Disease.CPTCodeDescriptionColumn] = DBNull.Value;
                            }
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.MedicalDiseaseTestResult))
                        {
                            RowDisease.TestResultId = MDVUtility.ToInt16(CurrentModel.MedicalDiseaseTestResult);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.TestResultIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.MedicalDiseaseStatus))
                        {
                            RowDisease.StatusId = MDVUtility.ToInt16(CurrentModel.MedicalDiseaseStatus);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.StatusIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.MedicalDiseaseLocation))
                        {
                            RowDisease.Location = MDVUtility.ToStr(CurrentModel.MedicalDiseaseLocation);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.LocationColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.MedicalDiseaseSeverity))
                        {
                            RowDisease.SeverityId = MDVUtility.ToInt16(CurrentModel.MedicalDiseaseSeverity);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.SeverityIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.MedicalDiseasePattern))
                        {
                            RowDisease.PatternId = MDVUtility.ToInt16(CurrentModel.MedicalDiseasePattern);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.PatternIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.MedicalDiseaseAgggravatedBy))
                        {
                            RowDisease.AggravatedById = MDVUtility.ToInt16(CurrentModel.MedicalDiseaseAgggravatedBy);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.AggravatedByIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.MedicalDiseaseComments))
                        {
                            RowDisease.Comments = MDVUtility.ToStr(CurrentModel.MedicalDiseaseComments);
                        }
                        else
                        {
                            RowDisease[dsDisease.MedicalHx_Disease.CommentsColumn] = DBNull.Value;
                        }

                        RowDisease.IsActive = true;
                        if (CurrentModel.AddedFromMobileApp == "1")
                        {

                            RowDisease.CreatedBy = CurrentModel.CreatedBy;
                            RowDisease.CreatedOn = Convert.ToDateTime(CurrentModel.CreatedOn);
                            RowDisease.ModifiedBy = CurrentModel.ModifiedBy;
                            RowDisease.ModifiedOn = Convert.ToDateTime(CurrentModel.ModifiedOn);
                            RowDisease.AddedFromMobileApp = CurrentModel.AddedFromMobileApp;
                            RowDisease.ICDID = Convert.ToInt64(CurrentModel.ICDID);
                        }
                        else
                        {
                            RowDisease.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowDisease.CreatedOn = DateTime.Now;
                            RowDisease.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowDisease.ModifiedOn = DateTime.Now;
                        }
                    

                        RowDisease[dsDisease.MedicalHx_Disease.SoapTextColumn] = insertUpdateMedicalSoapText(dsDisease, CurrentModel, diseasesAdded);
                        if (arrDiseases.Length < 1)
                        {
                            dsDisease.MedicalHx_Disease.AddMedicalHx_DiseaseRow(RowDisease);
                        }

                       // dsDisease.MedicalHx_Disease.AddMedicalHx_DiseaseRow(RowDisease);

                     //   dsDiseaseData.MedicalHx_Disease.AddMedicalHx_DiseaseRow(RowDisease);
                       
                        // if no Tobacco is found against TobaccoId, it implies for new record

                        //if (dsDisease.MedicalHx_Disease.Rows.Count < 1)
                        //{
                        //    dsDisease.MedicalHx_Disease.AddMedicalHx_DiseaseRow(RowDisease);
                        //}
                    }
                }
            }
            //int counter = 0;
            //// Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            //foreach (DataRow RowDisease in dsDisease.MedicalHx_Disease.Rows)
            //{
            //    RowDisease[dsDisease.MedicalHx_Disease.SoapTextColumn] = insertUpdateMedicalSoapText(dsDisease, lstDisease[counter], diseasesAdded);
            //    counter++;
            //}
            #region Database Insertion/Updation

            BLObject<DSMedicalHx> objInsertedDisease = BLLClinicalObj.insertUpdateMedicalHxDisease(dsDisease, patientId);
            if (objInsertedDisease.Data != null)
            {                                
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    diseaseId = dsDisease.Tables[dsDisease.MedicalHx_Disease.TableName].Rows.Count > 0 ? dsDisease.Tables[dsDisease.MedicalHx_Disease.TableName].Rows[0][dsDisease.MedicalHx_Disease.DiseaseIdColumn.ColumnName] : 0,
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

      
        /* End 12/01/2016 Muhammad Irfan Save/Update Diseases in MedicalHx */
        public string deleteMedicalHxDisease(string DiseaseId, string MedicalHxId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(DiseaseId)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteMedicalHxDisease(MDVUtility.ToStr(DiseaseId));
                    if (obj.Data == "")
                    {
                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MDVUtility.ToInt64(MedicalHxId));
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