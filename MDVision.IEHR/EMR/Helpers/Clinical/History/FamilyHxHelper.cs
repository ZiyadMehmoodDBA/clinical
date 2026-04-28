// Author:  Muhammad Arshad
// Created Date: 14/01/2016
//OverView: Helper class for FamilyHx
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
    public class FamilyHxHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public FamilyHxHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static FamilyHxHelper _instance = null;
        public static FamilyHxHelper Instance()
        {
            if (_instance == null)
                _instance = new FamilyHxHelper();
            return _instance;
        }

        #region fillFamilyHx

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Created Date: 22/01/2016
        ///OverView: This function will handle fill of FamilyHx
        /// </summary>
        /// <param name="model"></param>
        /// <param name="FamilyHxId"></param>
        /// <returns>string</returns>
        public string fillFamilyHx(FamilyHxModel model, Int64 familyHxId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)) && familyHxId == 0)
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
                    DSFamilyHx dsFamilyHx = null;
                    BLObject<DSFamilyHx> obj = BLLClinicalObj.LoadFamilyHx(MDVUtility.ToInt64(model.PatientId), familyHxId, "", "", "");
                    dsFamilyHx = obj.Data;
                    if (dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows[0];
                        var SoapText = string.Empty;
                        var IsCreatedOrModified = string.Empty;
                        var LastUpdated = string.Empty;
                        var SoapInfo = getCurrentSoapText(MDVUtility.ToInt64(dr[dsFamilyHx.FamilyHx.FamilyHxIdColumn.ColumnName]));
                        if (SoapInfo != null)
                        {
                            SoapText = SoapInfo["SoapText"];
                            IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                            LastUpdated = SoapInfo["LastUpdated"];
                        }

                        var SocialHxkeyValues = new Dictionary<string, string>
                        {
                            { "FamilyHxDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.FamilyHxDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsFamilyHx.FamilyHx.FamilyHxDateColumn.ColumnName]).ToShortDateString()},
                            { "FamilyHxId",  MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.FamilyHxIdColumn.ColumnName])},
                            { "FamilyHxUnremarkable", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.bUnremarkableColumn.ColumnName])},
                            { "FamilyOverallComments", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.CommentsColumn.ColumnName])},
                            { "SoapText", SoapText},
                            { "IsCreatedOrModified", IsCreatedOrModified},
                            { "LastUpdated",LastUpdated}
                        };

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            FamilyHxFill_JSON = js.Serialize(SocialHxkeyValues),
                            //obaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
                            FamilyHxLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName]),
                            DiseaseLoad_JSON = (MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_Disease.TableName])),
                            MemberLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName]),
                            MemberHasDisease_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_FamilyMemberHasDisease.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            FamilyHxFill_JSON = "[]",
                            //obaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
                            FamilyHxLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName]),
                            DiseaseLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_Disease.TableName])),
                            MemberLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName]),
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

        #region saveFamilyHx


        /// <summary>
        /// Author:  Muhammad Irfan
        /// Created Date: 20/01/2016
        /// OverView: This function will handle saving of FamilyHx
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lstDiseaseObject"></param>
        /// <param name="lstMembersObject"></param>
        /// <returns>string</returns>
        public string saveFamilyHx(FamilyHxModel model, List<object> lstDiseaseObject, List<object> lstMembersObject, Int64 patientId)
        {
            try
            {
                DSFamilyHx dsFamilyHx = new DSFamilyHx();

                DSFamilyHx.FamilyHxRow dr = dsFamilyHx.FamilyHx.NewFamilyHxRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.FamilyHxDate))
                {
                    dr.FamilyHxDate = MDVUtility.ToDateTime(model.FamilyHxDate);
                }
                else
                {
                    dr[dsFamilyHx.FamilyHx.FamilyHxDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.FamilyOverallComments))
                {
                    dr.Comments = MDVUtility.ToStr(model.FamilyOverallComments);
                }
                else
                {
                    dr[dsFamilyHx.FamilyHx.CommentsColumn] = DBNull.Value;

                }

                dr.bUnremarkable = model.FamilyHxUnremarkable.ToLower() == "true" ? true : false;

                dr.IsActive = true;

                if (model.AddedFromMobileApp == "1")
                {

                    dr.CreatedBy = model.CreatedBy;
                    dr.CreatedOn = Convert.ToDateTime(model.CreatedOn);
                    dr.ModifiedBy = model.ModifiedBy;
                    dr.ModifiedOn = Convert.ToDateTime(model.ModifiedOn);


                }
                else
                {
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }

                #region Database Insertion
                dsFamilyHx.FamilyHx.AddFamilyHxRow(dr);
                BLObject<DSFamilyHx> obj = BLLClinicalObj.InsertFamilyHx(dsFamilyHx, model.PatientId);
                dsFamilyHx = obj.Data;

                if (obj.Data != null)
                {
                    long diseaseid = 0;
                    long memberdetailid = 0;
                    Int64 familyhxid = MDVUtility.ToInt64(dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows[0][dsFamilyHx.FamilyHx.FamilyHxIdColumn.ColumnName]);
                    if (familyhxid > 0)
                    {
                        if (lstDiseaseObject.Count > 0)
                        {
                            long responsefamilyhxdisease = insertUpdateDisease(familyhxid, lstDiseaseObject, lstMembersObject, model.PatientId);
                            diseaseid = responsefamilyhxdisease;
                            if (responsefamilyhxdisease > 0)
                            {
                                Int64 memberid = insertUpdateFamilyMember(diseaseid, lstMembersObject, patientId, familyhxid);
                                memberdetailid = memberid;
                                //Start//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
                                if (memberdetailid > 0)
                                {
                                    BLObject<DSFamilyHx> ob = BLLClinicalObj.insertUpdateSoapTextForFamilyHxDisease(diseaseid);
                                    if (ob.Data != null)
                                    {
                                        //   BLObject<string> objV = BLLClinicalObj.updateSoapTextForFamilyHX(familyhxid);
                                    }
                                }
                                //End//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease

                            }

                        }
                    }



                    /*
                       Change Implement BY: Muhammad Azhar Shahzad
                       Reason: To update Soap Text of Social Hx in Insert mode
                       Created Date: Dec 15, 2015
                   */
                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForFamilyHX(familyhxid);

                    BLObject<DSFamilyHx> objAdded = BLLClinicalObj.LoadFamilyHx(MDVUtility.ToInt64(model.PatientId), familyhxid);
                    dsFamilyHx = objAdded.Data;
                    var SoapText = string.Empty;
                    var IsCreatedOrModified = string.Empty;
                    var LastUpdated = string.Empty;
                    var SoapInfo = getCurrentSoapText(familyhxid);
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
                        FamilyHxId = MDVUtility.ToInt64(dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows[0][dsFamilyHx.FamilyHx.FamilyHxIdColumn.ColumnName]),
                        diseaseId = diseaseid,
                        memberdetailid = memberdetailid,
                        SoapText = SoapText,
                        IsCreatedOrModified = IsCreatedOrModified,
                        LastUpdated = LastUpdated,

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

        public string saveFamilyHx(FamilyHxModel model, long noteId)
        {
            try
            {
                DSFamilyHx dsFamilyHx = new DSFamilyHx();

                DSFamilyHx.FamilyHxRow dr = dsFamilyHx.FamilyHx.NewFamilyHxRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.FamilyHxDate))
                {
                    dr.FamilyHxDate = MDVUtility.ToDateTime(model.FamilyHxDate);
                }
                else
                {
                    dr[dsFamilyHx.FamilyHx.FamilyHxDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.FamilyOverallComments))
                {
                    dr.Comments = MDVUtility.ToStr(model.FamilyOverallComments);
                }
                else
                {
                    dr[dsFamilyHx.FamilyHx.CommentsColumn] = DBNull.Value;

                }
                // Faizan Ameen.
                // IMP-736.
                if (noteId != 0)
                {
                    dr.NoteId = MDVUtility.ToStr(noteId);
                }
                else
                {
                    dr[dsFamilyHx.FamilyHx.NoteIdColumn] = DBNull.Value;

                }
                dr.bUnremarkable = model.FamilyHxUnremarkable.ToLower() == "true" ? true : false;


                dr.IsActive = true;
                if (model.AddedFromMobileApp == "1")
                {

                    dr.CreatedBy = model.CreatedBy;
                    dr.CreatedOn = Convert.ToDateTime(model.CreatedOn);
                    dr.ModifiedBy = model.ModifiedBy;
                    dr.ModifiedOn = Convert.ToDateTime(model.ModifiedOn);


                }
                else
                {
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }

                #region Database Insertion
                dsFamilyHx.FamilyHx.AddFamilyHxRow(dr);
                BLObject<DSFamilyHx> obj = BLLClinicalObj.InsertFamilyHx(dsFamilyHx, model.PatientId);
                dsFamilyHx = obj.Data;

                if (obj.Data != null)
                {
                    long diseaseid = 0;
                    long memberdetailid = 0;
                    Int64 familyhxid = MDVUtility.ToInt64(dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows[0][dsFamilyHx.FamilyHx.FamilyHxIdColumn.ColumnName]);
                    if (familyhxid > 0)
                    {

                        if (model.FamilyMemberDisease != null && model.FamilyMemberDisease.Count > 0)
                        {

                            List<object> lstMembersObject = new List<object>(model.FamilyMemberDiseaseDetail);

                            foreach (var item in model.FamilyMemberDisease)
                            {
                                List<object> lstDisease = new List<object>();
                                lstDisease.Add(item);

                                long responseFamilyHxDisease = insertUpdateDisease(familyhxid, lstDisease, lstMembersObject, model.PatientId);
                                diseaseid = responseFamilyHxDisease;

                                if (diseaseid > 0)
                                {
                                    List<object> lstDiseaseDetail = new List<object>();
                                    FamilyHxFamilyMemberModel diseaseDetail = model.FamilyMemberDiseaseDetail.FirstOrDefault(d => d.DiseaseId == item.DiseaseId && d.MemberId == item.FamilyMemberId);
                                    lstDiseaseDetail.Add(diseaseDetail);

                                    Int64 MemberId = insertUpdateFamilyMember(diseaseid, lstDiseaseDetail, MDVUtility.ToInt64(model.PatientId), familyhxid);
                                    memberdetailid = MemberId;
                                    //Start//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
                                    if (memberdetailid > 0)
                                    {
                                        BLObject<DSFamilyHx> ob = BLLClinicalObj.insertUpdateSoapTextForFamilyHxDisease(diseaseid);
                                        if (ob.Data != null)
                                        {
                                            //     BLObject<string> objV = BLLClinicalObj.updateSoapTextForFamilyHX(familyHxId);
                                        }
                                    }
                                    //End//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
                                }
                            }




                            //long responsefamilyhxdisease = insertUpdateDisease(familyhxid, lstDiseaseObject, lstMembersObject, model.PatientId);
                            //diseaseid = responsefamilyhxdisease;
                            //if (responsefamilyhxdisease > 0)
                            //{
                            //    Int64 memberid = insertUpdateFamilyMember(diseaseid, lstMembersObject, patientId, familyhxid);
                            //    memberdetailid = memberid;
                            //    //Start//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
                            //    if (memberdetailid > 0)
                            //    {
                            //        BLObject<DSFamilyHx> ob = BLLClinicalObj.insertUpdateSoapTextForFamilyHxDisease(diseaseid);
                            //        if (ob.Data != null)
                            //        {
                            //            //   BLObject<string> objV = BLLClinicalObj.updateSoapTextForFamilyHX(familyhxid);
                            //        }
                            //    }
                            //    //End//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease

                            //}

                        }

                        //if (lstDiseaseObject.Count > 0)
                        //{
                        //    long responsefamilyhxdisease = insertUpdateDisease(familyhxid, lstDiseaseObject, lstMembersObject, model.PatientId);
                        //    diseaseid = responsefamilyhxdisease;
                        //    if (responsefamilyhxdisease > 0)
                        //    {
                        //        Int64 memberid = insertUpdateFamilyMember(diseaseid, lstMembersObject, patientId, familyhxid);
                        //        memberdetailid = memberid;
                        //        //Start//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
                        //        if (memberdetailid > 0)
                        //        {
                        //            BLObject<DSFamilyHx> ob = BLLClinicalObj.insertUpdateSoapTextForFamilyHxDisease(diseaseid);
                        //            if (ob.Data != null)
                        //            {
                        //                //   BLObject<string> objV = BLLClinicalObj.updateSoapTextForFamilyHX(familyhxid);
                        //            }
                        //        }
                        //        //End//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease

                        //    }

                        //}
                    }



                    /*
                       Change Implement BY: Muhammad Azhar Shahzad
                       Reason: To update Soap Text of Social Hx in Insert mode
                       Created Date: Dec 15, 2015
                   */
                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForFamilyHX(familyhxid);

                    BLObject<DSFamilyHx> objAdded = BLLClinicalObj.LoadFamilyHx(MDVUtility.ToInt64(model.PatientId), familyhxid);
                    dsFamilyHx = objAdded.Data;
                    var SoapText = string.Empty;
                    var IsCreatedOrModified = string.Empty;
                    var LastUpdated = string.Empty;
                    var SoapInfo = getCurrentSoapText(familyhxid);
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
                        FamilyHxId = MDVUtility.ToInt64(dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows[0][dsFamilyHx.FamilyHx.FamilyHxIdColumn.ColumnName]),
                        diseaseId = diseaseid,
                        memberdetailid = memberdetailid,
                        SoapText = SoapText,
                        IsCreatedOrModified = IsCreatedOrModified,
                        LastUpdated = LastUpdated,

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

        #region updateFamilyHx

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Created Date: 21/01/2016
        /// OverView: This function will handle update of FamilyHx
        /// </summary>
        /// <param name="model"></param>
        /// <param name="FamilyHxId"></param>
        /// <param name="lstDiseaseObject"></param>
        /// <param name="lstMembersObject"></param>
        /// <returns>string</returns>
        public string updateFamilyHx(FamilyHxModel model, Int64 familyHxId, List<object> lstDiseaseObject, List<object> lstMembersObject, Int64 patientId)
        {
            try
            {
                if (familyHxId > 0)
                {

                    DSFamilyHx dsFamilyHx = new DSFamilyHx();
                    BLObject<DSFamilyHx> obj = BLLClinicalObj.LoadFamilyHx(MDVUtility.ToInt64(model.PatientId), familyHxId);
                    dsFamilyHx = obj.Data;
                    foreach (DSFamilyHx.FamilyHxRow dr in dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows)
                    {
                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                        if (!string.IsNullOrEmpty(model.FamilyHxDate))
                        {
                            dr.FamilyHxDate = MDVUtility.ToDateTime(model.FamilyHxDate);
                        }
                        else
                        {
                            dr[dsFamilyHx.FamilyHx.FamilyHxDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.FamilyOverallComments))
                        {
                            dr.Comments = MDVUtility.ToStr(model.FamilyOverallComments);
                        }
                        else
                        {
                            dr[dsFamilyHx.FamilyHx.CommentsColumn] = DBNull.Value;
                        }

                        dr.bUnremarkable = model.FamilyHxUnremarkable.ToLower() == "true" ? true : false;

                        if (model.AddedFromMobileApp == "1")
                        {


                            dr.ModifiedBy = model.ModifiedBy;
                            dr.ModifiedOn = Convert.ToDateTime(model.ModifiedOn);


                        }
                        else
                        {

                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }
                    }
                    Int64 diseaseID = 0;
                    Int64 memberDetailId = 0;
                    if (lstDiseaseObject.Count > 0)
                    {
                        long responseFamilyHxDisease = insertUpdateDisease(familyHxId, lstDiseaseObject, lstMembersObject, model.PatientId);
                        diseaseID = responseFamilyHxDisease;

                        if (diseaseID > 0)
                        {
                            Int64 MemberId = insertUpdateFamilyMember(responseFamilyHxDisease, lstMembersObject, patientId, familyHxId);
                            memberDetailId = MemberId;
                            //Start//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
                            if (memberDetailId > 0)
                            {
                                BLObject<DSFamilyHx> ob = BLLClinicalObj.insertUpdateSoapTextForFamilyHxDisease(diseaseID);
                                if (ob.Data != null)
                                {
                                    //     BLObject<string> objV = BLLClinicalObj.updateSoapTextForFamilyHX(familyHxId);
                                }
                            }
                            //End//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
                        }

                    }



                    //Start 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                    //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MedicalHxId);

                    //End 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                    #region Database Updation
                    if (dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows.Count > 0)
                    {
                        BLObject<DSFamilyHx> objUpdate = BLLClinicalObj.UpdateFamilyHx(dsFamilyHx, model.PatientId);
                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForFamilyHX(familyHxId);
                        if (objUpdate.Data != null)
                        {

                            BLObject<DSFamilyHx> objAdded = BLLClinicalObj.LoadFamilyHx(MDVUtility.ToInt64(model.PatientId), familyHxId);
                            dsFamilyHx = objAdded.Data;
                            var SoapText = string.Empty;
                            var IsCreatedOrModified = string.Empty;
                            var LastUpdated = string.Empty;


                            var SoapInfo = getCurrentSoapText(familyHxId);

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
                                memberDetailId = memberDetailId,
                                FamilyHxId = familyHxId,
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
                        message = "Family Hx not found."
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

        public string updateFamilyHx(FamilyHxModel model, long noteId)
        {
            try
            {
                long familyHxId = MDVUtility.ToInt64(model.FamilyHxId);
                if (familyHxId > 0)
                {

                    DSFamilyHx dsFamilyHx = new DSFamilyHx();
                    BLObject<DSFamilyHx> obj = BLLClinicalObj.LoadFamilyHx(MDVUtility.ToInt64(model.PatientId), familyHxId);
                    dsFamilyHx = obj.Data;
                    foreach (DSFamilyHx.FamilyHxRow dr in dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows)
                    {
                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                        if (!string.IsNullOrEmpty(model.FamilyHxDate))
                        {
                            dr.FamilyHxDate = MDVUtility.ToDateTime(model.FamilyHxDate);
                        }
                        else
                        {
                            dr[dsFamilyHx.FamilyHx.FamilyHxDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.FamilyOverallComments))
                        {
                            dr.Comments = MDVUtility.ToStr(model.FamilyOverallComments);
                        }
                        else
                        {
                            dr[dsFamilyHx.FamilyHx.CommentsColumn] = DBNull.Value;
                        }

                        dr.bUnremarkable = model.FamilyHxUnremarkable.ToLower() == "true" ? true : false;

                        if (model.AddedFromMobileApp == "1")
                        {
                            dr.ModifiedBy = model.ModifiedBy;
                            dr.ModifiedOn = Convert.ToDateTime(model.ModifiedOn);
                        }
                        else
                        {
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }
                    }
                    Int64 diseaseID = 0;
                    Int64 memberDetailId = 0;
                    if (model.FamilyMemberDisease != null && model.FamilyMemberDisease.Count > 0)
                    {
                        //List<object> lstDisease = new List<object>(model.FamilyMemberDisease);
                        List<object> lstMembersObject = new List<object>(model.FamilyMemberDiseaseDetail);

                        foreach (var item in model.FamilyMemberDisease)
                        {
                            List<object> lstDisease = new List<object>();
                            lstDisease.Add(item);

                            long responseFamilyHxDisease = insertUpdateDisease(familyHxId, lstDisease, lstMembersObject, model.PatientId);
                            diseaseID = responseFamilyHxDisease;

                            if (diseaseID > 0)
                            {
                                List<object> lstDiseaseDetail = new List<object>();
                                FamilyHxFamilyMemberModel diseaseDetail = model.FamilyMemberDiseaseDetail.FirstOrDefault(d => d.DiseaseId == item.DiseaseId && d.MemberId == item.FamilyMemberId);
                                lstDiseaseDetail.Add(diseaseDetail);

                                Int64 MemberId = insertUpdateFamilyMember(diseaseID, lstDiseaseDetail, MDVUtility.ToInt64(model.PatientId), familyHxId);
                                memberDetailId = MemberId;
                                //Start//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
                                if (memberDetailId > 0)
                                {
                                    BLObject<DSFamilyHx> ob = BLLClinicalObj.insertUpdateSoapTextForFamilyHxDisease(diseaseID);
                                    if (ob.Data != null)
                                    {
                                        //     BLObject<string> objV = BLLClinicalObj.updateSoapTextForFamilyHX(familyHxId);
                                    }
                                }
                                //End//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
                            }
                        }




                        //List<object> lstDisease = new List<object>(model.FamilyMemberDisease);
                        //List<object> lstMembersObject = new List<object>(model.FamilyMemberDiseaseDetail);

                        //long responseFamilyHxDisease = insertUpdateDisease(familyHxId, lstDisease, lstMembersObject, model.PatientId);
                        //diseaseID = responseFamilyHxDisease;

                        //if (diseaseID > 0)
                        //{
                        //    Int64 MemberId = insertUpdateFamilyMember(responseFamilyHxDisease, lstMembersObject, MDVUtility.ToInt64(model.PatientId), familyHxId);
                        //    memberDetailId = MemberId;
                        //    //Start//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
                        //    if (memberDetailId > 0)
                        //    {
                        //        BLObject<DSFamilyHx> ob = BLLClinicalObj.insertUpdateSoapTextForFamilyHxDisease(diseaseID);
                        //        if (ob.Data != null)
                        //        {
                        //            //     BLObject<string> objV = BLLClinicalObj.updateSoapTextForFamilyHX(familyHxId);
                        //        }
                        //    }
                        //    //End//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
                        //}

                    }



                    //Start 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                    //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MedicalHxId);

                    //End 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                    #region Database Updation
                    if (dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows.Count > 0)
                    {
                        BLObject<DSFamilyHx> objUpdate = BLLClinicalObj.UpdateFamilyHx(dsFamilyHx, model.PatientId);
                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForFamilyHX(familyHxId);
                        if (objUpdate.Data != null)
                        {

                            BLObject<DSFamilyHx> objAdded = BLLClinicalObj.LoadFamilyHx(MDVUtility.ToInt64(model.PatientId), familyHxId);
                            dsFamilyHx = objAdded.Data;
                            var SoapText = string.Empty;
                            var IsCreatedOrModified = string.Empty;
                            var LastUpdated = string.Empty;


                            var SoapInfo = getCurrentSoapText(familyHxId);

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
                                memberDetailId = memberDetailId,
                                FamilyHxId = familyHxId,
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
                        message = "Family Hx not found."
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
        //public string updateFamilyHx(FamilyHxModel model, long noteId)
        //{
        //    try
        //    {
        //        long familyHxId = MDVUtility.ToInt64(model.FamilyHxId);
        //        if (familyHxId > 0)
        //        {
        //            DSFamilyHx dsFamilyHx = new DSFamilyHx();
        //            BLObject<DSFamilyHx> obj = BLLClinicalObj.LoadFamilyHx(MDVUtility.ToInt64(model.PatientId), familyHxId);
        //            dsFamilyHx = obj.Data;
        //            foreach (DSFamilyHx.FamilyHxRow dr in dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows)
        //            {
        //                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

        //                if (!string.IsNullOrEmpty(model.FamilyHxDate))
        //                {
        //                    dr.FamilyHxDate = MDVUtility.ToDateTime(model.FamilyHxDate);
        //                }
        //                else
        //                {
        //                    dr[dsFamilyHx.FamilyHx.FamilyHxDateColumn] = DBNull.Value;
        //                }

        //                if (!string.IsNullOrEmpty(model.FamilyOverallComments))
        //                {
        //                    dr.Comments = MDVUtility.ToStr(model.FamilyOverallComments);
        //                }
        //                else
        //                {
        //                    dr[dsFamilyHx.FamilyHx.CommentsColumn] = DBNull.Value;
        //                }

        //                dr.bUnremarkable = model.FamilyHxUnremarkable.ToLower() == "true" ? true : false;

        //                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                dr.ModifiedOn = DateTime.Now;
        //            }
        //            Int64 diseaseid = 0;
        //            Int64 memberdetailid = 0;

        //            foreach (var item in model.FamilyMemberList)
        //            {
        //                long responsefamilyhxdisease = insertUpdateDisease(familyHxId, item, model.PatientId);
        //                diseaseid = responsefamilyhxdisease;
        //                if (responsefamilyhxdisease > 0)
        //                {
        //                    Int64 memberid = insertUpdateFamilyMember(diseaseid, item, MDVUtility.ToInt64(model.PatientId), familyHxId);
        //                    memberdetailid = memberid;
        //                    //Start//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
        //                    if (memberdetailid > 0)
        //                    {
        //                        BLObject<DSFamilyHx> ob = BLLClinicalObj.insertUpdateSoapTextForFamilyHxDisease(diseaseid);
        //                        if (ob.Data != null)
        //                        {
        //                            //   BLObject<string> objV = BLLClinicalObj.updateSoapTextForFamilyHX(familyhxid);
        //                        }
        //                    }
        //                    //End//27/01/2016//Ahmad Raza//updating soap text of member detail against selected disease

        //                }
        //            }

        //            //Start 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
        //            //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MedicalHxId);

        //            //End 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
        //            #region Database Updation
        //            if (dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows.Count > 0)
        //            {
        //                BLObject<DSFamilyHx> objUpdate = BLLClinicalObj.UpdateFamilyHx(dsFamilyHx, model.PatientId);
        //                BLObject<string> objValue = BLLClinicalObj.updateSoapTextForFamilyHX(familyHxId);
        //                if (objUpdate.Data != null)
        //                {

        //                    BLObject<DSFamilyHx> objAdded = BLLClinicalObj.LoadFamilyHx(MDVUtility.ToInt64(model.PatientId), familyHxId);
        //                    dsFamilyHx = objAdded.Data;
        //                    var SoapText = string.Empty;
        //                    var IsCreatedOrModified = string.Empty;
        //                    var LastUpdated = string.Empty;


        //                    var SoapInfo = getCurrentSoapText(familyHxId);

        //                    if (SoapInfo != null)
        //                    {
        //                        SoapText = SoapInfo["SoapText"];
        //                        IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
        //                        LastUpdated = SoapInfo["LastUpdated"];
        //                    }

        //                    var response = new
        //                    {
        //                        status = true,
        //                        message = Common.AppPrivileges.Update_Message,
        //                        diseaseId = diseaseid,
        //                        memberDetailId = memberdetailid,
        //                        FamilyHxId = familyHxId,
        //                        SoapText = SoapText,
        //                        IsCreatedOrModified = IsCreatedOrModified,
        //                        LastUpdated = LastUpdated
        //                    };
        //                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //                }
        //                else
        //                {
        //                    var response = new
        //                    {
        //                        status = false,
        //                        message = objUpdate.Message
        //                    };
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //                }
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    message = ""
        //                };
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                message = "Family Hx not found."
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        #endregion

        #region"Disease and Member Functions"

        /// <summary>
        /// Author:  Muhammad Irfan
        /// Created Date: 20/01/2016
        /// OverView: This function will handle saving and updating of disease
        /// </summary>
        /// <param name="FamilyHxId"></param>
        /// <param name="lstDiseaseObject"></param>
        /// <param name="lstMembersObject"></param>
        /// <returns>integer</returns>
        private Int64 insertUpdateDisease(long familyHxId, List<object> lstDiseaseObject, List<object> lstMembersObject, string patientId)
        {
            #region Disease
            DSFamilyHx dsFamily = new DSFamilyHx();
            List<FamilyHxDiseaseModel> lstDisease = lstDiseaseObject.OfType<FamilyHxDiseaseModel>().ToList();
            List<FamilyHxFamilyMemberModel> lstmember = lstMembersObject.OfType<FamilyHxFamilyMemberModel>().ToList();
            bool isFirstChild = false;
            foreach (FamilyHxDiseaseModel CurrentModel in lstDisease)
            {
                if (CurrentModel.DiseaseId != null)
                {
                    Int32 currentDiseaseId = MDVUtility.ToInt32(CurrentModel.DiseaseId);
                    Int32 familyMemberId = MDVUtility.ToInt32(CurrentModel.FamilyMemberId);
                    if (CurrentModel.AddedFromMobileApp == null)
                        currentDiseaseId = currentDiseaseId == 0 ? -1 : currentDiseaseId;

                    BLObject<DSFamilyHx> objDisease = BLLClinicalObj.LoadFamilyHx_Disease(familyHxId, currentDiseaseId, MDVUtility.ToInt32(CurrentModel.FamilyMemberId), "", "");
                    dsFamily = objDisease.Data;
                    DSFamilyHx.FamilyHx_DiseaseRow RowDisease = null;
                    if (dsFamily.FamilyHx_Disease.Rows.Count > 0)
                    {
                        RowDisease = (DSFamilyHx.FamilyHx_DiseaseRow)dsFamily.FamilyHx_Disease.Rows[0];
                    }
                    else
                    {
                        RowDisease = dsFamily.FamilyHx_Disease.NewFamilyHx_DiseaseRow();
                    }

                    if (RowDisease != null)
                    {
                        bool isValueDifferent = false;
                        bool istoUpdateRow = false;
                        if (dsFamily.FamilyHx_Disease.Rows.Count < 1)
                        {
                            RowDisease.DiseaseId = currentDiseaseId;
                        }
                        RowDisease.FamilyHxId = familyHxId;
                        RowDisease.FamilyMemberId = MDVUtility.ToInt32(CurrentModel.FamilyMemberId);

                        if (!string.IsNullOrEmpty(CurrentModel.ICD9Code))
                        {
                            RowDisease.ICD9Code = MDVUtility.ToStr(CurrentModel.ICD9Code);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.ICD9CodeColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.ICD9CodeDescription))
                        {
                            RowDisease.ICD9CodeDescription = MDVUtility.ToStr(CurrentModel.ICD9CodeDescription);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.ICD9CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10Code))
                        {
                            RowDisease.ICD10Code = MDVUtility.ToStr(CurrentModel.ICD10Code);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.ICD10CodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10CodeDescription))
                        {
                            RowDisease.ICD10CodeDescription = MDVUtility.ToStr(CurrentModel.ICD10CodeDescription);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.ICD10CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDID))
                        {
                            RowDisease.SNOMEDID = MDVUtility.ToStr(CurrentModel.SNOMEDID);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.SNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDDescription))
                        {
                            RowDisease.SNOMEDDescription = MDVUtility.ToStr(CurrentModel.SNOMEDDescription);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.SNOMEDDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCode))
                        {
                            RowDisease.LexiCode = MDVUtility.ToStr(CurrentModel.LexiCode);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.LexiCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCodeDescription))
                        {
                            RowDisease.LexiCodeDescription = MDVUtility.ToStr(CurrentModel.LexiCodeDescription);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.LexiCodeDescriptionColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.FreeTextICD))
                        {
                            RowDisease.FreeTextICD = MDVUtility.ToStr(CurrentModel.FreeTextICD);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.FreeTextICDColumn] = null;
                        }

                        RowDisease.IsActive = true;
                        if (CurrentModel.AddedFromMobileApp == "1")
                        {
                            RowDisease.CreatedBy = CurrentModel.CreatedBy;
                            RowDisease.CreatedOn = Convert.ToDateTime(CurrentModel.CreatedOn);
                            RowDisease.ModifiedBy = CurrentModel.ModifiedBy;
                            RowDisease.ModifiedOn = Convert.ToDateTime(CurrentModel.ModifiedOn);
                            RowDisease.AddedFromMobileApp = CurrentModel.AddedFromMobileApp;
                            //RowDisease.TempICDID = Convert.ToInt64(CurrentModel.ICDID);
                            if (CurrentModel.FamilyHxId == "-1")
                                CurrentModel.FamilyHxId = "";

                        }
                        else
                        {
                            RowDisease.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowDisease.CreatedOn = DateTime.Now;
                            RowDisease.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowDisease.ModifiedOn = DateTime.Now;

                        }



                        RowDisease[dsFamily.FamilyHx_Disease.SoapTextColumn] = insertUpdateFamilySoapText(dsFamily, CurrentModel, lstmember.FirstOrDefault(n => n.MemberId == CurrentModel.FamilyMemberId && n.DiseaseId == CurrentModel.DiseaseId));
                        if (dsFamily.FamilyHx_Disease.Rows.Count < 1)
                        {
                            dsFamily.FamilyHx_Disease.AddFamilyHx_DiseaseRow(RowDisease);
                        }
                    }
                }
            }
            //int counter = 0;
            //// start//26/01/2016//Ahmad Raza//for soap text
            //foreach (DataRow RowDisease in dsFamily.FamilyHx_Disease.Rows)
            //{
            //    RowDisease[dsFamily.FamilyHx_Disease.SoapTextColumn] = insertUpdateFamilySoapText(dsFamily, lstDisease[counter], lstmember[counter]);
            //    counter++;
            //}
            // End//26/01/2016//Ahmad Raza//for soap text
            #region Database Insertion/Updation

            Int64 diseaseId = 0;

            BLObject<DSFamilyHx> objInsertedDisease = BLLClinicalObj.insertUpdateFamilyHxDisease(dsFamily, patientId);
            if (objInsertedDisease.Data != null)
            {
                diseaseId = dsFamily.Tables[dsFamily.FamilyHx_Disease.TableName].Rows.Count > 0 ? MDVUtility.ToInt64(dsFamily.Tables[dsFamily.FamilyHx_Disease.TableName].Rows[0][dsFamily.FamilyHx_Disease.DiseaseIdColumn.ColumnName]) : 0;
                return diseaseId;
            }
            else
            {
                return 0;
            }

            #endregion


            #endregion
        }



        private Int64 insertUpdateDisease(long familyHxId, FamilyHxFamilyMemberModel lstMembersObject, string patientId)
        {
            #region Disease
            DSFamilyHx dsFamily = new DSFamilyHx();
            List<FamilyHxDiseaseModel> lstDisease = lstMembersObject.FamilyMemberDiseases;
            FamilyHxFamilyMemberModel lstmember = lstMembersObject;
            bool isFirstChild = false;

            BLObject<DSFamilyHx> objDisease = BLLClinicalObj.LoadFamilyHx_Disease(familyHxId, 0, MDVUtility.ToInt32(lstmember.MemberId), "", "");
            dsFamily = objDisease.Data;

            foreach (FamilyHxDiseaseModel CurrentModel in lstDisease)
            {
                if (CurrentModel.DiseaseId != null)
                {
                    Int32 currentDiseaseId = MDVUtility.ToInt32(CurrentModel.DiseaseId);
                    Int32 familyMemberId = MDVUtility.ToInt32(CurrentModel.FamilyMemberId);

                    currentDiseaseId = currentDiseaseId == 0 ? -1 : currentDiseaseId;

                    DSFamilyHx.FamilyHx_DiseaseRow RowDisease = null;
                    DSFamilyHx.FamilyHx_DiseaseRow[] arrDiseases = (DSFamilyHx.FamilyHx_DiseaseRow[])dsFamily.FamilyHx_Disease.Select(dsFamily.FamilyHx_Disease.DiseaseIdColumn.ColumnName + "=" + currentDiseaseId);

                    if (arrDiseases != null && arrDiseases.Length > 0)
                    {
                        RowDisease = arrDiseases[0];
                    }
                    else
                    {
                        RowDisease = dsFamily.FamilyHx_Disease.NewFamilyHx_DiseaseRow();
                    }

                    if (RowDisease != null)
                    {
                        bool isValueDifferent = false;
                        bool istoUpdateRow = false;
                        if (dsFamily.FamilyHx_Disease.Rows.Count < 1)
                        {
                            RowDisease.DiseaseId = currentDiseaseId;
                        }
                        RowDisease.FamilyHxId = familyHxId;
                        RowDisease.FamilyMemberId = MDVUtility.ToInt32(CurrentModel.FamilyMemberId);

                        if (!string.IsNullOrEmpty(CurrentModel.ICD9Code))
                        {
                            RowDisease.ICD9Code = MDVUtility.ToStr(CurrentModel.ICD9Code);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.ICD9CodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD9CodeDescription))
                        {
                            RowDisease.ICD9CodeDescription = MDVUtility.ToStr(CurrentModel.ICD9CodeDescription);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.ICD9CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10Code))
                        {
                            RowDisease.ICD10Code = MDVUtility.ToStr(CurrentModel.ICD10Code);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.ICD10CodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ICD10CodeDescription))
                        {
                            RowDisease.ICD10CodeDescription = MDVUtility.ToStr(CurrentModel.ICD10CodeDescription);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.ICD10CodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDID))
                        {
                            RowDisease.SNOMEDID = MDVUtility.ToStr(CurrentModel.SNOMEDID);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.SNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SNOMEDDescription))
                        {
                            RowDisease.SNOMEDDescription = MDVUtility.ToStr(CurrentModel.SNOMEDDescription);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.SNOMEDDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCode))
                        {
                            RowDisease.LexiCode = MDVUtility.ToStr(CurrentModel.LexiCode);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.LexiCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.LexiCodeDescription))
                        {
                            RowDisease.LexiCodeDescription = MDVUtility.ToStr(CurrentModel.LexiCodeDescription);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.LexiCodeDescriptionColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.FreeTextICD))
                        {
                            RowDisease.FreeTextICD = MDVUtility.ToStr(CurrentModel.FreeTextICD);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_Disease.FreeTextICDColumn] = null;
                        }

                        RowDisease.IsActive = true;
                        RowDisease.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowDisease.CreatedOn = DateTime.Now;
                        RowDisease.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowDisease.ModifiedOn = DateTime.Now;
                        RowDisease[dsFamily.FamilyHx_Disease.SoapTextColumn] = insertUpdateFamilySoapText(dsFamily, CurrentModel, lstmember);
                        if (arrDiseases.Length < 1)
                        {
                            dsFamily.FamilyHx_Disease.AddFamilyHx_DiseaseRow(RowDisease);
                        }
                    }
                }
            }
            //int counter = 0;
            //// start//26/01/2016//Ahmad Raza//for soap text
            //foreach (DataRow RowDisease in dsFamily.FamilyHx_Disease.Rows)
            //{
            //    RowDisease[dsFamily.FamilyHx_Disease.SoapTextColumn] = insertUpdateFamilySoapText(dsFamily, lstDisease[counter], lstmember);
            //    counter++;
            //}
            // End//26/01/2016//Ahmad Raza//for soap text
            #region Database Insertion/Updation

            Int64 diseaseId = 0;

            BLObject<DSFamilyHx> objInsertedDisease = BLLClinicalObj.insertUpdateFamilyHxDisease(dsFamily, patientId);
            if (objInsertedDisease.Data != null)
            {
                diseaseId = dsFamily.Tables[dsFamily.FamilyHx_Disease.TableName].Rows.Count > 0 ? MDVUtility.ToInt64(dsFamily.Tables[dsFamily.FamilyHx_Disease.TableName].Rows[0][dsFamily.FamilyHx_Disease.DiseaseIdColumn.ColumnName]) : 0;
                return diseaseId;
            }
            else
            {
                return 0;
            }

            #endregion


            #endregion
        }


        /// <summary>
        /// Start//27/01/2016//Ahmad Raza//method to create soap text for family hx
        /// </summary>
        /// <param name="dsFamilyHistory"></param>
        /// <param name="modelObj"></param>
        /// <param name="modelObj1"></param>
        /// <returns>soap text String</returns>
        internal string insertUpdateFamilySoapText(DSFamilyHx dsFamilyHistory, FamilyHxDiseaseModel modelObj, FamilyHxFamilyMemberModel modelObj1)
        {
            string soapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                if (MDVUtility.ToInt32(modelObj.DiseaseId) != 0)
                {
                    var diseaseName = string.Empty;

                    if (!string.IsNullOrEmpty(modelObj.ICD9CodeDescription))
                    {
                        diseaseName = modelObj.ICD9CodeDescription;
                    }
                    else if (!string.IsNullOrEmpty(modelObj.FreeTextICD) && !string.Equals(modelObj.FreeTextICD, "-1"))
                    {
                        diseaseName = modelObj.FreeTextICD;
                    }
                    else
                    {
                        diseaseName = string.Empty;
                    }

                    string healthStr = string.Empty;
                    if (!string.IsNullOrEmpty(diseaseName))
                    {
                        healthStr = ", Health Status is ";
                    }
                    else
                    {
                        healthStr = "Health Status is ";
                    }

                    sb.Append("<div id='familyHistory_" + modelObj.FamilyHxId + "' title='Family'  name='Family Hx'>");
                    sb.Append(diseaseName + (string.IsNullOrEmpty(modelObj1.HealthStatusText) ? "" : healthStr + modelObj1.HealthStatusText + "."));
                    sb.Append((string.IsNullOrEmpty(modelObj1.RadRelativeDied) ? "" : (modelObj1.RadRelativeDied == "True" ? " Death has occurred due to specified problem." : " Death has not occurred due to specified problem.")) + (string.IsNullOrEmpty(modelObj1.YearofBirth) ? "" : " Year of birth is " + modelObj1.YearofBirth + ".") + (string.IsNullOrEmpty(modelObj1.AgeAtDeath) ? "" : " Age at death is " + modelObj1.AgeAtDeath + ".") + (string.IsNullOrEmpty(modelObj1.AgeAtDiagnosis) ? "" : " Age at diagnosis was (approx) " + modelObj1.AgeAtDiagnosis + ".") + (string.IsNullOrEmpty(modelObj1.FamilyMemberComments) ? "" : " Comments: " + modelObj1.FamilyMemberComments) + "</div>");
                }
            }
            else
                if (dsFamilyHistory.FamilyHx_Disease != null && dsFamilyHistory.FamilyHx_Disease.Rows.Count > 0)
            {
                foreach (var item in dsFamilyHistory.FamilyHx_Disease)
                {
                    sb.Append("<div id='familyHistory_" + modelObj.FamilyHxId + "' title='Family'  name='Family Hx'>");
                    sb.Append((string.IsNullOrEmpty(modelObj1.FamilyMemberText) ? "" : modelObj1.FamilyMemberText) + (string.IsNullOrEmpty(modelObj1.HealthStatusText) ? "" : ", Health Status is " + modelObj1.HealthStatusText) + ".");
                    sb.Append((string.IsNullOrEmpty(modelObj1.RadRelativeDied) ? "" : (modelObj1.RadRelativeDied == "True" ? " Death has occurred due to specified problem." : " Death has not occurred due to specified problem.")) + (string.IsNullOrEmpty(modelObj1.YearofBirth) ? "" : " Year of birth is " + modelObj1.YearofBirth + ".") + (string.IsNullOrEmpty(modelObj1.AgeAtDeath) ? "" : " Age at death is " + modelObj1.AgeAtDeath + ".") + (string.IsNullOrEmpty(modelObj1.AgeAtDiagnosis) ? "" : " Age at diagnosis was (approx) " + modelObj1.AgeAtDiagnosis + ".") + (string.IsNullOrEmpty(modelObj1.FamilyMemberComments) ? "" : " Comments: " + HttpUtility.HtmlEncode(modelObj1.FamilyMemberComments)) + "</div>");
                }
            }

            else
            {
                return string.Empty;
            }
            return sb.ToString();

            //string soapText = string.Empty;
            //StringBuilder sb = new StringBuilder();

            //if (modelObj != null)
            //{
            //    if (MDVUtility.ToInt32(modelObj.DiseaseId) != 0)
            //    {
            //        sb.Append("<div id='familyHistory_" + modelObj.FamilyHxId + "' title='Family'  name='Family Hx'>");
            //        sb.Append((string.IsNullOrEmpty(modelObj1.FamilyMemberText) ? "" : modelObj1.FamilyMemberText) + (string.IsNullOrEmpty(modelObj1.HealthStatusText) ? "" : ", Health Status is " + modelObj1.HealthStatusText) + ".");
            //        sb.Append((string.IsNullOrEmpty(modelObj1.RadRelativeDied) ? "" : (modelObj1.RadRelativeDied == "True" ? " Relative has died as a result." : " Relative has not died as a result.")) + (string.IsNullOrEmpty(modelObj1.YearofBirth) ? "" : " Year of birth is " + modelObj1.YearofBirth + ".") + (string.IsNullOrEmpty(modelObj1.AgeAtDeath) ? "" : " Age at death is " + modelObj1.AgeAtDeath + ".") + (string.IsNullOrEmpty(modelObj1.AgeAtDiagnosis) ? "" : " Age at diagnosis was (approx) " + modelObj1.AgeAtDiagnosis + ".") + (string.IsNullOrEmpty(modelObj1.FamilyMemberComments) ? "" : " Comments: " + modelObj1.FamilyMemberComments) + "</div>");
            //    }
            //}
            //else
            //    if (dsFamilyHistory.FamilyHx_Disease != null && dsFamilyHistory.FamilyHx_Disease.Rows.Count > 0)
            //{
            //    foreach (var item in dsFamilyHistory.FamilyHx_Disease)
            //    {
            //        sb.Append("<div id='familyHistory_" + modelObj.FamilyHxId + "' title='Family'  name='Family Hx'>");
            //        sb.Append((string.IsNullOrEmpty(modelObj1.FamilyMemberText) ? "" : modelObj1.FamilyMemberText) + (string.IsNullOrEmpty(modelObj1.HealthStatusText) ? "" : ", Health Status is " + modelObj1.HealthStatusText) + ".");
            //        sb.Append((string.IsNullOrEmpty(modelObj1.RadRelativeDied) ? "" : (modelObj1.RadRelativeDied == "True" ? " Relative has died as a result. " : " Relative has not died as a result.")) + (string.IsNullOrEmpty(modelObj1.YearofBirth) ? "" : " Year of birth is " + modelObj1.YearofBirth + ".") + (string.IsNullOrEmpty(modelObj1.AgeAtDeath) ? "" : " Age at death is " + modelObj1.AgeAtDeath+".") + (string.IsNullOrEmpty(modelObj1.AgeAtDiagnosis) ? "" : " Age at diagnosis was (approx) " + modelObj1.AgeAtDiagnosis+".") + (string.IsNullOrEmpty(modelObj1.FamilyMemberComments) ? "" : " Comments: " + HttpUtility.HtmlEncode(modelObj1.FamilyMemberComments)) + "</div>");
            //    }
            //}

            //else
            //{
            //    return string.Empty;
            //}
            //return sb.ToString();
        }
        //End//27/01/2016//Ahmad Raza//method to create soap text for family hx

        /// <summary>
        ///  Author:  Muhammad Irfan
        ///  Created Date: 20/01/2016
        ///  OverView: This function will handle saving and updating of family member details
        /// </summary>
        /// <param name="DiseaseId"></param>
        /// <param name="lstMembersObject"></param>
        /// <returns>integer</returns>

        private Int64 insertUpdateFamilyMember(long diseaseId, List<object> lstMembersObject, Int64 patientId, Int64 familyhxid)
        {
            #region Disease
            DSFamilyHx dsFamily = new DSFamilyHx();
            List<FamilyHxFamilyMemberModel> lstMembers = lstMembersObject.OfType<FamilyHxFamilyMemberModel>().ToList();
            bool isFirstChild = false;
            FamilyHxFamilyMemberModel modelmember = new FamilyHxFamilyMemberModel();
            FamilyHxDiseaseModel modeldisease = new FamilyHxDiseaseModel();



            foreach (FamilyHxFamilyMemberModel CurrentModel in lstMembers)
            {
                modelmember = CurrentModel;
                if (CurrentModel.MemberDetailId != null)
                {
                    Int32 currentMemberId = MDVUtility.ToInt32(CurrentModel.MemberId);
                    currentMemberId = currentMemberId == 0 ? -1 : currentMemberId;

                    long memberDetailId = MDVUtility.ToInt64(CurrentModel.MemberDetailId) < 0 ? 0 : MDVUtility.ToInt64(CurrentModel.MemberDetailId);
                    BLObject<DSFamilyHx> objMember = BLLClinicalObj.LoadFamilyHx_FamilyMemberDetail(memberDetailId, currentMemberId, diseaseId, "", "", "");
                    dsFamily = objMember.Data;

                    DSFamilyHx.FamilyHx_FamilyMemberDetailRow RowDisease = null;
                    if (dsFamily.FamilyHx_FamilyMemberDetail.Rows.Count > 0)
                    {
                        RowDisease = (DSFamilyHx.FamilyHx_FamilyMemberDetailRow)dsFamily.FamilyHx_FamilyMemberDetail.Rows[0];
                    }
                    else
                    {
                        RowDisease = dsFamily.FamilyHx_FamilyMemberDetail.NewFamilyHx_FamilyMemberDetailRow();
                    }

                    if (RowDisease != null)
                    {
                        bool isValueDifferent = false;
                        bool istoUpdateRow = false;
                        if (dsFamily.FamilyHx_FamilyMemberDetail.Rows.Count < 1)
                        {
                            RowDisease.MemberDetailId = currentMemberId;
                        }
                        RowDisease.DiseaseId = diseaseId;
                        modeldisease.DiseaseId = RowDisease.DiseaseId.ToString();


                        if (!string.IsNullOrEmpty(CurrentModel.MemberId))
                        {
                            RowDisease.MemberId = MDVUtility.ToInt32(CurrentModel.MemberId);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.MemberIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.HealthStatus))
                        {
                            RowDisease.HealthStatusId = MDVUtility.ToInt32(CurrentModel.HealthStatus);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.HealthStatusIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.YearofBirth))
                        {
                            RowDisease.BirthYear = MDVUtility.ToStr(CurrentModel.YearofBirth);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.BirthYearColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AgeAtDeath))
                        {
                            RowDisease.AgeAtDeath = MDVUtility.ToStr(CurrentModel.AgeAtDeath);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.AgeAtDeathColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AgeAtDiagnosis))
                        {
                            RowDisease.AgeAtDiagnosis = MDVUtility.ToStr(CurrentModel.AgeAtDiagnosis);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.AgeAtDiagnosisColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.FamilyMemberComments))
                        {
                            RowDisease.Comments = MDVUtility.ToStr(CurrentModel.FamilyMemberComments);
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.CommentsColumn] = DBNull.Value;
                        }


                        //Start//27/01/2016//Ahmad Raza//setting checkbox value
                        //Start Sep 25, 2017 Edit By Humaira Yousaf Bug# EMR-4802                                                 
                        if (CurrentModel.RadRelativeDied != null)
                        {
                            if (CurrentModel.RadRelativeDied.ToLower() == "false" || CurrentModel.RadRelativeDied.ToLower() == "no")
                            {
                                RowDisease.IsRelativeDied = false;
                            }
                            else if (CurrentModel.RadRelativeDied.ToLower() == "true" || CurrentModel.RadRelativeDied.ToLower() == "yes")
                            {
                                RowDisease.IsRelativeDied = true;
                            }
                            else
                            {
                                RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.IsRelativeDiedColumn] = DBNull.Value;
                            }
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.IsRelativeDiedColumn] = DBNull.Value;
                        }
                        //End Sep 25, 2017 Edit By Humaira Yousaf Bug# EMR-4802

                        RowDisease.IsActive = true;

                        if (CurrentModel.AddedFromMobileApp == "1")
                        {
                            RowDisease.CreatedBy = CurrentModel.CreatedBy;
                            RowDisease.CreatedOn = Convert.ToDateTime(CurrentModel.CreatedOn);
                            RowDisease.ModifiedBy = CurrentModel.ModifiedBy;
                            RowDisease.ModifiedOn = Convert.ToDateTime(CurrentModel.ModifiedOn);

                        }
                        //End//27/01/2016//Ahmad Raza//setting checkbox value
                        else
                        {
                            RowDisease.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowDisease.CreatedOn = DateTime.Now;
                            RowDisease.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowDisease.ModifiedOn = DateTime.Now;
                        }


                        if (dsFamily.FamilyHx_FamilyMemberDetail.Rows.Count < 1)
                        {
                            dsFamily.FamilyHx_FamilyMemberDetail.AddFamilyHx_FamilyMemberDetailRow(RowDisease);
                        }
                    }
                }
            }
            int counter = 0;
            //  for Soap Text
            foreach (DataRow RowDisease in dsFamily.FamilyHx_FamilyMemberDetail.Rows)
            {
                RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.SoapTextColumn] = insertUpdateFamilySoapText(dsFamily, modeldisease, modelmember);
                counter++;
            }
            #region Database Insertion/Updation

            Int64 memberId = 0;

            BLObject<DSFamilyHx> objInsertedDisease = BLLClinicalObj.insertUpdateFamilyHx_FamilyMemberDetail(dsFamily, patientId, familyhxid);
            if (objInsertedDisease.Data != null)
            {
                memberId = dsFamily.Tables[dsFamily.FamilyHx_FamilyMemberDetail.TableName].Rows.Count > 0 ? MDVUtility.ToInt64(dsFamily.Tables[dsFamily.FamilyHx_FamilyMemberDetail.TableName].Rows[0][dsFamily.FamilyHx_FamilyMemberDetail.MemberDetailIdColumn.ColumnName]) : 0;
                return memberId;
            }
            else
            {
                return 0;
            }

            #endregion


            #endregion
        }

        private Int64 insertUpdateFamilyMember(long diseaseId, FamilyHxFamilyMemberModel lstMembersObject, Int64 patientId, Int64 familyhxid)
        {
            #region Disease
            DSFamilyHx dsFamily = new DSFamilyHx();
            FamilyHxFamilyMemberModel lstMembers = lstMembersObject;
            bool isFirstChild = false;
            FamilyHxFamilyMemberModel modelmember = new FamilyHxFamilyMemberModel();
            FamilyHxDiseaseModel modeldisease = new FamilyHxDiseaseModel();

            long memberDetailId = MDVUtility.ToInt64(lstMembers.MemberDetailId) < 0 ? 0 : MDVUtility.ToInt64(lstMembers.MemberDetailId);
            BLObject<DSFamilyHx> objMember = BLLClinicalObj.LoadFamilyHx_FamilyMemberDetail(memberDetailId, MDVUtility.ToInt64(lstMembers.MemberId), diseaseId, "", "", "");
            dsFamily = objMember.Data;

            //foreach (FamilyHxFamilyMemberModel CurrentModel in lstMembers)
            //{
            FamilyHxFamilyMemberModel CurrentModel = lstMembers;
            if (CurrentModel.MemberDetailId != null)
            {
                Int32 currentMemberId = MDVUtility.ToInt32(CurrentModel.MemberId);
                currentMemberId = currentMemberId == 0 ? -1 : currentMemberId;


                DSFamilyHx.FamilyHx_FamilyMemberDetailRow RowDisease = null;
                if (dsFamily.FamilyHx_FamilyMemberDetail.Rows.Count > 0)
                {
                    RowDisease = (DSFamilyHx.FamilyHx_FamilyMemberDetailRow)dsFamily.FamilyHx_FamilyMemberDetail.Rows[0];
                }
                else
                {
                    RowDisease = dsFamily.FamilyHx_FamilyMemberDetail.NewFamilyHx_FamilyMemberDetailRow();
                }

                if (RowDisease != null)
                {
                    bool isValueDifferent = false;
                    bool istoUpdateRow = false;
                    if (dsFamily.FamilyHx_FamilyMemberDetail.Rows.Count < 1)
                    {
                        RowDisease.MemberDetailId = currentMemberId;
                    }
                    RowDisease.DiseaseId = diseaseId;
                    modeldisease.DiseaseId = RowDisease.DiseaseId.ToString();
                    if (!string.IsNullOrEmpty(CurrentModel.MemberId))
                    {
                        RowDisease.MemberId = MDVUtility.ToInt32(CurrentModel.MemberId);
                    }
                    else
                    {
                        RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.MemberIdColumn] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(CurrentModel.HealthStatus))
                    {
                        RowDisease.HealthStatusId = MDVUtility.ToInt32(CurrentModel.HealthStatus);
                    }
                    else
                    {
                        RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.HealthStatusIdColumn] = DBNull.Value;
                    }


                    if (!string.IsNullOrEmpty(CurrentModel.YearofBirth))
                    {
                        RowDisease.BirthYear = MDVUtility.ToStr(CurrentModel.YearofBirth);
                    }
                    else
                    {
                        RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.BirthYearColumn] = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(CurrentModel.AgeAtDeath))
                    {
                        RowDisease.AgeAtDeath = MDVUtility.ToStr(CurrentModel.AgeAtDeath);
                    }
                    else
                    {
                        RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.AgeAtDeathColumn] = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(CurrentModel.AgeAtDiagnosis))
                    {
                        RowDisease.AgeAtDiagnosis = MDVUtility.ToStr(CurrentModel.AgeAtDiagnosis);
                    }
                    else
                    {
                        RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.AgeAtDiagnosisColumn] = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(CurrentModel.FamilyMemberComments))
                    {
                        RowDisease.Comments = MDVUtility.ToStr(CurrentModel.FamilyMemberComments);
                    }
                    else
                    {
                        RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.CommentsColumn] = DBNull.Value;
                    }


                    //Start//27/01/2016//Ahmad Raza//setting checkbox value
                    //Start Sep 25, 2017 Edit By Humaira Yousaf Bug# EMR-4802
                    if (CurrentModel.RadRelativeDied != null)
                    {
                        if (CurrentModel.RadRelativeDied == "False")
                        {
                            RowDisease.IsRelativeDied = false;
                        }
                        else if (CurrentModel.RadRelativeDied == "True")
                        {
                            RowDisease.IsRelativeDied = true;
                        }
                        else
                        {
                            RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.IsRelativeDiedColumn] = DBNull.Value;
                        }
                    }
                    else
                    {
                        RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.IsRelativeDiedColumn] = DBNull.Value;
                    }
                    //End Sep 25, 2017 Edit By Humaira Yousaf Bug# EMR-4802
                    RowDisease.IsActive = true;
                    //End//27/01/2016//Ahmad Raza//setting checkbox value
                    if (CurrentModel.AddedFromMobileApp == "1")
                    {

                        RowDisease.CreatedBy = CurrentModel.CreatedBy;
                        RowDisease.CreatedOn = Convert.ToDateTime(CurrentModel.CreatedOn);
                        RowDisease.ModifiedBy = CurrentModel.ModifiedBy;
                        RowDisease.ModifiedOn = Convert.ToDateTime(CurrentModel.ModifiedOn);
                        //RowDisease.AddedFromMobileApp = CurrentModel.AddedFromMobileApp;
                        //  RowDisease.ICDID = Convert.ToInt64(CurrentModel.ICDID);
                    }

                    else
                    {

                        RowDisease.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowDisease.CreatedOn = DateTime.Now;
                        RowDisease.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowDisease.ModifiedOn = DateTime.Now;
                    }


                    if (dsFamily.FamilyHx_FamilyMemberDetail.Rows.Count < 1)
                    {
                        dsFamily.FamilyHx_FamilyMemberDetail.AddFamilyHx_FamilyMemberDetailRow(RowDisease);
                    }
                }
            }
            //}
            int counter = 0;
            //  for Soap Text
            foreach (DataRow RowDisease in dsFamily.FamilyHx_FamilyMemberDetail.Rows)
            {
                RowDisease[dsFamily.FamilyHx_FamilyMemberDetail.SoapTextColumn] = insertUpdateFamilySoapText(dsFamily, modeldisease, modelmember);
                counter++;
            }
            #region Database Insertion/Updation

            Int64 memberId = 0;

            BLObject<DSFamilyHx> objInsertedDisease = BLLClinicalObj.insertUpdateFamilyHx_FamilyMemberDetail(dsFamily, patientId, familyhxid);
            if (objInsertedDisease.Data != null)
            {
                memberId = dsFamily.Tables[dsFamily.FamilyHx_FamilyMemberDetail.TableName].Rows.Count > 0 ? MDVUtility.ToInt64(dsFamily.Tables[dsFamily.FamilyHx_FamilyMemberDetail.TableName].Rows[0][dsFamily.FamilyHx_FamilyMemberDetail.MemberDetailIdColumn.ColumnName]) : 0;
                return memberId;
            }
            else
            {
                return 0;
            }

            #endregion


            #endregion
        }

        #endregion

        #region"Fill Family Members"
        /// <summary>
        ///  Author:  Muhammad Irfan
        ///   Created Date: 20/01/2016
        ///   OverView: This function will handle filling of members list
        /// </summary>
        /// <param name="model"></param>
        /// <returns>string</returns>
        public string fillFamilyMember(FamilyHxFamilyMemberModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.DiseaseId)))
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
                    DSFamilyHx dsFamilyHx = null;
                    BLObject<DSFamilyHx> obj = BLLClinicalObj.LoadFamilyHx_FamilyMemberDetail(MDVUtility.ToInt64(model.MemberDetailId), MDVUtility.ToInt64(model.MemberId), MDVUtility.ToInt64(model.DiseaseId), model.FamilyHxId);
                    if (obj.Data != null)
                    {
                        dsFamilyHx = obj.Data;
                        if (dsFamilyHx.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName].Rows.Count > 0)
                        {
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                MemberLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVCustomException.HumanReadableMessage(obj.Message),
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
        /// Author:  Muhammad Irfan
        ///  Created Date: 20/01/2016
        /// OverView: This function will handle filling of details of members
        /// </summary>
        /// <param name="model"></param>
        /// <returns>string</returns>
        public string fillFamilyMemberDetail(FamilyHxFamilyMemberModel model, string familyHxId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.DiseaseId)) && string.IsNullOrEmpty(MDVUtility.ToStr(model.MemberId)))
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
                    DSFamilyHx dsFamilyHx = null;
                    BLObject<DSFamilyHx> obj = BLLClinicalObj.LoadFamilyHx_FamilyMemberDetail(MDVUtility.ToInt64(model.MemberDetailId), MDVUtility.ToInt64(model.MemberId), MDVUtility.ToInt64(model.DiseaseId), familyHxId, "1", "", model.PatientId);
                    if (obj.Data != null)
                    {
                        dsFamilyHx = obj.Data;
                        if (dsFamilyHx.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName].Rows.Count > 0)
                        {

                            DataRow dr = dsFamilyHx.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName].Rows[0];
                            var MemberskeyValues = new Dictionary<string, string>
                            {
                                { "YearofBirth",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.BirthYearColumn.ColumnName]))?"": MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.BirthYearColumn.ColumnName]).ToString()},
                                { "HealthStatus",  MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.HealthStatusIdColumn.ColumnName])},
                                { "RadRelativeDied", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.IsRelativeDiedColumn.ColumnName])},
                                { "AgeAtDeath", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.AgeAtDeathColumn.ColumnName])},
                                { "AgeAtDiagnosis", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.AgeAtDiagnosisColumn.ColumnName])},
                                { "FamilyMemberComments", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.CommentsColumn.ColumnName])},
                                //{ "AgeAtDiagnosis", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx_FamilyMemberDetail.SoapTextColumn.ColumnName])}
                            };

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                FamilyHxFill_JSON = js.Serialize(MemberskeyValues),
                                MemberLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,                                
                                Message = Common.AppPrivileges.No_Record_Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVCustomException.HumanReadableMessage(obj.Message),
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


        public Dictionary<string, string> getCurrentSoapText(Int64 FamilyHxId)
        {
            try
            {
                DSHistorySummary dsHistorySummarySoap = null;
                BLObject<DSHistorySummary> objSummary;
                objSummary = BLLClinicalObj.loadHxLog(FamilyHxId, "FamilyHx", "Current", 1, 10);
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
                   {"FamilyHxId" ,MDVUtility.ToStr( FamilyHxId)}
                };
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region delete familyhx disease
        /// <summary>
        ///  Start//27/01/2016//Ahmad Raza//Methods to delete the disease and member details
        /// </summary>
        /// <param name="DiseaseId"></param>
        /// <param name="FamilyHxId"></param>
        /// <returns>string</returns>
        public string deleteFamilyHxDisease(string diseaseId, string familyHxId, int familyMemberId, string patientId)
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
                    BLObject<string> obj = BLLClinicalObj.deleteFamilyHxDisease(MDVUtility.ToStr(diseaseId), MDVUtility.ToStr(familyHxId), familyMemberId, patientId);
                    if (obj.Data == "")
                    {
                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForFamilyHX(MDVUtility.ToInt64(familyHxId));
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
        ///  Start//27/01/2016//Ahmad Raza//Methods to delete the member details
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="FamilyHxId"></param>
        /// <param name="DiseaseId"></param>
        /// <returns>string</returns>
        public string deleteFamilyHxMemberDetail(string memberId, string familyHxId, string diseaseId, long patientId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(memberId)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteFamilyHx_FamilyMemberDetail(MDVUtility.ToStr(memberId), MDVUtility.ToStr(diseaseId), MDVUtility.ToInt64(patientId), MDVUtility.ToInt64(familyHxId));
                    if (obj.Data == "")
                    {
                        //Start//28/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
                        BLObject<DSFamilyHx> ob = BLLClinicalObj.insertUpdateSoapTextForFamilyHxDisease(MDVUtility.ToInt64(diseaseId));
                        if (ob.Data != null)
                        {
                            BLObject<string> objValue = BLLClinicalObj.updateSoapTextForFamilyHX(MDVUtility.ToInt64(familyHxId));
                        }
                        //End//28/01/2016//Ahmad Raza//updating soap text of member detail against selected disease
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
        //End//27/01/2016//Ahmad Raza//Methods to delete the disease and member details



        //Start//22/01/2016//Ahmad Raza//Implimented methods for FamilyHx's association with Note
        #region Attach/Detach FamilyHx from Notes
        /// <summary>
        /// Attaching soap text with Note
        /// </summary>
        /// <param name="FamilyHxId"></param>
        /// <param name="NotesId"></param>
        /// <returns>string</returns>
        internal string attach_FamilyHx_With_Notes(string familyHxId, long notesId)
        {
            try
            {
                DSFamilyHx dsFamilyHx = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(familyHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<DSFamilyHx> obj = BLLClinicalObj.attachFamilyHxWithNotes(familyHxId, notesId);
                    if (obj.Data != null)
                    {
                        dsFamilyHx = obj.Data;
                        var response = new
                        {
                            status = true,
                            FamilyHxTotalCount = dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows.Count,
                            FamilyHxCount = dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName].Rows.Count,
                            FamilyHxLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName]),
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
        ///  detaching soap text from Note
        /// </summary>
        /// <param name="FamilyHxId"></param>
        /// <param name="NotesId"></param>
        /// <returns>string</returns>
        internal string detach_FamilyHx_From_Notes(long familyHxId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(familyHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachFamilyHxFromNotes(familyHxId, notesId);
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
        //End//22/01/2016//Ahmad Raza//Implimented methods for FamilyHx's association with Note

        /// <summary>
        ///  Method Name:fillFamilyMemberDiseases
        ///  Author: Humaira Yousaf
        ///  Created Date: 04-11-2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns>string</returns>
        public string fillFamilyMemberDiseases(FamilyHxDiseaseModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.FamilyHxId)) && string.IsNullOrEmpty(MDVUtility.ToStr(model.FamilyMemberId)))
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
                    DSFamilyHx dsFamilyHx = null;
                    BLObject<DSFamilyHx> obj = BLLClinicalObj.LoadFamilyHx_Disease(MDVUtility.ToInt64(model.FamilyHxId), MDVUtility.ToInt64(model.DiseaseId), MDVUtility.ToInt32(model.FamilyMemberId), "1", "");
                    if (obj.Data != null)
                    {
                        dsFamilyHx = obj.Data;
                        if (dsFamilyHx.Tables[dsFamilyHx.FamilyHx_Disease.TableName].Rows.Count > 0)
                        {
                            dsFamilyHx.Tables[dsFamilyHx.FamilyHx_Disease.TableName].Columns.Add("No_DeseaseExists", typeof(bool));
                            DataColumn col = dsFamilyHx.Tables[dsFamilyHx.FamilyHx_Disease.TableName].Columns["FreeTextICD"];
                            foreach (DataRow item in dsFamilyHx.Tables[dsFamilyHx.FamilyHx_Disease.TableName].Rows)
                            {
                                if (item[col].ToString() == "-1")
                                {
                                    item["No_DeseaseExists"] = true;
                                }
                                else
                                {
                                    item["No_DeseaseExists"] = false;
                                }
                            }
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                DiseaseLoad_JSON = (MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_Disease.TableName])),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.No_Record_Message,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVCustomException.HumanReadableMessage(obj.Message),
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

    }
}