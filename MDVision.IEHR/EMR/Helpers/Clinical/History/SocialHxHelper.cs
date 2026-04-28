// Author:  Muhammad Arshad
// Created Date: 04/12/2015
//OverView: Helper class for socialHx
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
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace MDVision.IEHR.EMR.Helpers.Clinical.History
{
    public class SocialHxHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public SocialHxHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static SocialHxHelper _instance = null;
        public static SocialHxHelper Instance()
        {
            if (_instance == null)
                _instance = new SocialHxHelper();
            return _instance;
        }

        #region saveSocialHx

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle saving of SocialHx
        public string saveSocialHx(SocialHxModel model, string fieldsJSON, List<object> lstTobaccoObjects, List<object> lstAlcoholObjects, List<object> lstDrugAbuseObjects, List<object> lstSexualHxObjects, List<object> lstOccupationHxObjects, List<object> lstCaffeineIntakHxObjects, List<object> lstSleepHxObjects, List<object> lstExercisesHxObjects, List<object> lstHousingHxObjects, List<object> lstTravelHxObjects)
        {
            try
            {
                DSSocialHistory dsSocialHx = new DSSocialHistory();

                DSSocialHistory.SocialHxRow dr = dsSocialHx.SocialHx.NewSocialHxRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.SocialHxDate))
                {
                    dr.SocialHxDate = MDVUtility.ToDateTime(model.SocialHxDate);
                }
                else
                {
                    dr[dsSocialHx.SocialHx.SocialHxDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.SocialComments))
                {
                    dr.Comments = "<div id='overallcomment' title='overallcomment'  name='overallcomment'><strong>Overall Comments:</strong> " + MDVUtility.ToStr(model.SocialComments) + "</div>";
                }
                else
                {
                    dr[dsSocialHx.SocialHx.CommentsColumn] = DBNull.Value;
                }

                dr.bUnremarkable = model.SocialHxUnremarkable.ToLower() == "true" ? true : false;

                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsSocialHx.SocialHx.AddSocialHxRow(dr);
                BLObject<DSSocialHistory> obj = BLLClinicalObj.InsertSocialHx(dsSocialHx);

                if (obj.Data != null)
                {
                    dsSocialHx = obj.Data;
                    Int64 SocialHxId = MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]);
                    if (SocialHxId > 0)
                    {
                        if (lstTobaccoObjects.Count > 0)
                        {
                            string responseBloodPressure = insertUpdateTobacco(SocialHxId, lstTobaccoObjects, model.PatientId);
                        }
                        if (lstAlcoholObjects.Count > 0)
                        {
                            string responseAlcohol = insertUpdateAlcohol(SocialHxId, lstAlcoholObjects, model.PatientId);
                        }
                        if (lstDrugAbuseObjects.Count > 0)
                        {
                            string responseDrugAbuse = insertUpdateDrugAbuse(SocialHxId, lstDrugAbuseObjects, model.PatientId);
                        }
                        if (lstSexualHxObjects.Count > 0)
                        {
                            string responseSexualHx = insertUpdateSexualHx(SocialHxId, lstSexualHxObjects, model.SocialComments, model.PatientId);
                        }
                        if (lstOccupationHxObjects.Count > 0 || lstCaffeineIntakHxObjects.Count > 0 || lstSleepHxObjects.Count > 0 || lstExercisesHxObjects.Count > 0 || lstHousingHxObjects.Count > 0 || lstTravelHxObjects.Count > 0)
                        {
                            string MiscHxResponse = insertUpdateMiscHx(SocialHxId);
                            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                            ser.MaxJsonLength = Int32.MaxValue;
                            var MiscHxResponseJSON = ser.Deserialize<dynamic>(MiscHxResponse);
                            Int64 MiscHxId = MDVUtility.ToStr(MiscHxResponseJSON["MiscHxId"]) != "" ? MDVUtility.ToInt64(MiscHxResponseJSON["MiscHxId"]) : 0;
                            if (MiscHxId > 0)
                            {

                                //Start 07/01/2016 Syed Zia Code to  Save/Update MiscHx Occupation
                                if (lstOccupationHxObjects.Count > 0)
                                {
                                    string responseOccupation = insertUpdateMiscHxOccupationHx(MiscHxId, lstOccupationHxObjects, model.PatientId);
                                }
                                if (lstCaffeineIntakHxObjects.Count > 0)
                                {
                                    string responseUpdateCaffeineIntakeHx = insertUpdateCaffeineIntakeHx(MiscHxId, lstCaffeineIntakHxObjects, model.PatientId);
                                }
                                //End 07/01/2016 Syed Zia Code to  Save/Update Occupation


                                //Start 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Sleep
                                if (lstSleepHxObjects.Count > 0)
                                {
                                    string SleepHxResponse = insertUpdateSleepHx(MiscHxId, lstSleepHxObjects, model.PatientId);
                                }
                                //End 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Sleep

                                //Start 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Exercises
                                if (lstExercisesHxObjects.Count > 0)
                                {
                                    string ExercisesHxResponse = insertUpdateExercisesHx(MiscHxId, lstExercisesHxObjects, model.PatientId);
                                }
                                //End 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Exercises

                                //Start 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Housing
                                if (lstHousingHxObjects.Count > 0)
                                {
                                    string HousingHxResponse = insertUpdateHousingHx(MiscHxId, lstHousingHxObjects, model.PatientId);
                                }
                                //if (lstTravelHxObjects.Count > 0)
                                //{
                                //    string TravelHxResponse = insertUpdateTravelHx(MiscHxId, lstTravelHxObjects, model.PatientId);
                                //}

                                //End 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Housing


                                //Start 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Caffeine Intake
                                //End 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Caffeine Intake
                            }
                        }

                    }
                    /*
                       Change Implement BY: Muhammad Azhar Shahzad
                       Reason: To update Soap Text of Social Hx in Insert mode
                       Created Date: Dec 15, 2015
                   */
                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForSocialHX(SocialHxId.ToString());

                    var SoapText = string.Empty;
                    var IsCreatedOrModified = string.Empty;
                    var LastUpdated = string.Empty;
                    var SoapInfo = getCurrentSoapText(MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]));
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
                        SocialHxId = MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]),
                        SoapText = SoapText,
                        IsCreatedOrModified = IsCreatedOrModified,
                        LastUpdated = LastUpdated,
                        MUAlertsCount = dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.MUAlertsCountColumn.ColumnName],

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


        public string saveSocialHx(SocialHxModel model, string fieldsJSON, List<SocialHxTobaccoModel> lstTobaccoObjects, List<SocialHxAlcoholModel> lstAlcoholObjects, List<SocialHxDrugAbuseModel> lstDrugAbuseObjects, List<SocialHxSexualHxModel> lstSexualHxObjects, List<SocialHxMiscHxOccupationModel> lstOccupationHxObjects, List<SocialHxMiscHxCaffeineIntakeModel> lstCaffeineIntakHxObjects, List<SocialHxMiscHxSleepModel> lstSleepHxObjects, List<SocialHxMiscHxExercisesModel> lstExercisesHxObjects, List<SocialHxMiscHxHousingModel> lstHousingHxObjects, long NoteId)
        {
            try
            {
                DSSocialHistory dsSocialHx = new DSSocialHistory();
                DSSocialHistory.SocialHxRow dr;
                if (!string.IsNullOrEmpty(model.SocialHxId) && MDVUtility.ToInt64(model.SocialHxId) > 0)
                {
                    BLObject<DSSocialHistory> obj1 = BLLClinicalObj.LoadSocialHx(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.SocialHxId));
                    dsSocialHx = obj1.Data;
                    dr = dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0] as DSSocialHistory.SocialHxRow;
                }
                else
                {
                    dr = dsSocialHx.SocialHx.NewSocialHxRow();
                }



                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.SocialHxDate))
                {
                    dr.SocialHxDate = MDVUtility.ToDateTime(model.SocialHxDate);
                }
                else
                {
                    dr[dsSocialHx.SocialHx.SocialHxDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.SocialComments))
                {
                    dr.Comments = "<div id='overallcomment' title='overallcomment'  name='overallcomment'><strong>Overall Comments:</strong> " + MDVUtility.ToStr(model.SocialComments) + "</div>";
                }
                else
                {
                    dr[dsSocialHx.SocialHx.CommentsColumn] = DBNull.Value;
                }

                dr.bUnremarkable = model.SocialHxUnremarkable.ToLower() == "true" ? true : false;
                if (NoteId > 0)
                {
                    dr.NotesId = NoteId;
                }
                else
                {
                    dr[dsSocialHx.SocialHx.NotesIdColumn] = DBNull.Value;
                }
                dr.IsActive = true;

                if (MDVUtility.ToInt64(model.SocialHxId) <= 0)
                {
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                }

                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                BLObject<DSSocialHistory> obj = new BLObject<DSSocialHistory>();

                if (!string.IsNullOrEmpty(model.SocialHxId) && MDVUtility.ToInt64(model.SocialHxId) > 0)
                {
                    obj = BLLClinicalObj.UpdateSocialHx(dsSocialHx);
                }
                else
                {
                    dsSocialHx.SocialHx.AddSocialHxRow(dr);
                    obj = BLLClinicalObj.InsertSocialHx(dsSocialHx);
                }

                dsSocialHx = obj.Data;

                if (obj.Data != null)
                {
                    Int64 SocialHxId = MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]);
                    if (SocialHxId > 0)
                    {
                        if (lstTobaccoObjects.Count > 0)
                        {
                            string responseBloodPressure = insertUpdateTobacco(SocialHxId, lstTobaccoObjects, model.PatientId);
                        }
                        if (lstAlcoholObjects.Count > 0)
                        {
                            string responseAlcohol = insertUpdateAlcohol(SocialHxId, lstAlcoholObjects, model.PatientId);
                        }
                        if (lstDrugAbuseObjects.Count > 0)
                        {
                            string responseDrugAbuse = insertUpdateDrugAbuse(SocialHxId, lstDrugAbuseObjects, model.PatientId);
                        }
                        if (lstSexualHxObjects.Count > 0)
                        {
                            string responseSexualHx = insertUpdateSexualHx(SocialHxId, lstSexualHxObjects, model.SocialComments, model.PatientId);
                        }
                        if (lstOccupationHxObjects.Count > 0 || lstCaffeineIntakHxObjects.Count > 0 || lstSleepHxObjects.Count > 0 || lstExercisesHxObjects.Count > 0 || lstHousingHxObjects.Count > 0)
                        {
                            string MiscHxResponse = insertUpdateMiscHx(SocialHxId);
                            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                            ser.MaxJsonLength = Int32.MaxValue;
                            var MiscHxResponseJSON = ser.Deserialize<dynamic>(MiscHxResponse);
                            Int64 MiscHxId = MDVUtility.ToStr(MiscHxResponseJSON["MiscHxId"]) != "" ? MDVUtility.ToInt64(MiscHxResponseJSON["MiscHxId"]) : 0;
                            if (MiscHxId > 0)
                            {

                                //Start 07/01/2016 Syed Zia Code to  Save/Update MiscHx Occupation
                                if (lstOccupationHxObjects.Count > 0)
                                {
                                    string responseOccupation = insertUpdateMiscHxOccupationHx(MiscHxId, lstOccupationHxObjects, model.PatientId);
                                }
                                if (lstCaffeineIntakHxObjects.Count > 0)
                                {
                                    string responseUpdateCaffeineIntakeHx = insertUpdateCaffeineIntakeHx(MiscHxId, lstCaffeineIntakHxObjects, model.PatientId);
                                }
                                //End 07/01/2016 Syed Zia Code to  Save/Update Occupation


                                //Start 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Sleep
                                if (lstSleepHxObjects.Count > 0)
                                {
                                    string SleepHxResponse = insertUpdateSleepHx(MiscHxId, lstSleepHxObjects, model.PatientId);
                                }
                                //End 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Sleep

                                //Start 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Exercises
                                if (lstExercisesHxObjects.Count > 0)
                                {
                                    string ExercisesHxResponse = insertUpdateExercisesHx(MiscHxId, lstExercisesHxObjects, model.PatientId);
                                }
                                //End 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Exercises

                                //Start 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Housing
                                if (lstHousingHxObjects.Count > 0)
                                {
                                    string HousingHxResponse = insertUpdateHousingHx(MiscHxId, lstHousingHxObjects, model.PatientId);
                                }

                                //End 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Housing


                                //Start 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Caffeine Intake
                                //End 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Caffeine Intake
                            }
                        }

                    }
                    /*
                       Change Implement BY: Muhammad Azhar Shahzad
                       Reason: To update Soap Text of Social Hx in Insert mode
                       Created Date: Dec 15, 2015
                   */
                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForSocialHX(SocialHxId.ToString());

                    var SoapText = string.Empty;
                    var IsCreatedOrModified = string.Empty;
                    var LastUpdated = string.Empty;
                    var SoapInfo = getCurrentSoapText(MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]));
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
                        SocialHxId = MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]),
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

        #region updateSocialHx

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle update of SocialHx
        public string updateSocialHx(SocialHxModel model, Int64 SocialHxId, List<object> lstTobaccoObjects, List<object> lstAlcoholObjects, List<object> lstDrugAbuseObjects, List<object> lstSexualHxObjects, List<object> lstOccupationHxObjects, List<object> lstCaffeineIntakHxObjects, List<object> lstSleepHxObjects, List<object> lstExercisesHxObjects, List<object> lstHousingHxObjects, List<object> lstTravelHxModel)
        {
            try
            {
                if (SocialHxId > 0)
                {

                    DSSocialHistory dsSocialHx = new DSSocialHistory();
                    BLObject<DSSocialHistory> obj = BLLClinicalObj.LoadSocialHx(MDVUtility.ToInt64(model.PatientId), SocialHxId);
                    dsSocialHx = obj.Data;
                    foreach (DSSocialHistory.SocialHxRow dr in dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows)
                    {
                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                        if (!string.IsNullOrEmpty(model.SocialHxDate))
                        {
                            dr.SocialHxDate = MDVUtility.ToDateTime(model.SocialHxDate);
                        }
                        else
                        {
                            dr[dsSocialHx.SocialHx.SocialHxDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.SocialComments))
                        {
                            dr.Comments = "<div id='overallcomment' title='overallcomment'  name='overallcomment'><strong>Overall Comments:</strong> " + MDVUtility.ToStr(model.SocialComments) + "</div>";
                        }
                        else
                        {
                            dr[dsSocialHx.SocialHx.CommentsColumn] = DBNull.Value;
                        }

                        dr.bUnremarkable = model.SocialHxUnremarkable.ToLower() == "true" ? true : false;

                        //dr.IsActive = true;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }
                    if (lstTobaccoObjects.Count > 0)
                    {
                        string responseTobacco = insertUpdateTobacco(SocialHxId, lstTobaccoObjects, model.PatientId);
                    }
                    if (lstAlcoholObjects.Count > 0)
                    {
                        string responseAlcohol = insertUpdateAlcohol(SocialHxId, lstAlcoholObjects, model.PatientId);
                    }
                    if (lstDrugAbuseObjects.Count > 0)
                    {
                        string responseAlcohol = insertUpdateDrugAbuse(SocialHxId, lstDrugAbuseObjects, model.PatientId);
                    }
                    if (lstSexualHxObjects.Count > 0)
                    {
                        string responseAlcohol = insertUpdateSexualHx(SocialHxId, lstSexualHxObjects, model.SocialComments, model.PatientId);
                    }
                    if (lstOccupationHxObjects.Count > 0 || lstCaffeineIntakHxObjects.Count > 0 || lstSleepHxObjects.Count > 0 || lstExercisesHxObjects.Count > 0 || lstHousingHxObjects.Count > 0 || lstTravelHxModel.Count > 0)
                    {
                        string MiscHxResponse = insertUpdateMiscHx(SocialHxId);
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        ser.MaxJsonLength = Int32.MaxValue;
                        var MiscHxResponseJSON = ser.Deserialize<dynamic>(MiscHxResponse);
                        Int64 MiscHxId = MDVUtility.ToStr(MiscHxResponseJSON["MiscHxId"]) != "" ? MDVUtility.ToInt64(MiscHxResponseJSON["MiscHxId"]) : 0;
                        if (MiscHxId > 0)
                        {

                            //Start 07/01/2016 Syed Zia Code to  Save/Update MiscHx Occupation
                            if (lstOccupationHxObjects.Count > 0)
                            {
                                string responseOccupation = insertUpdateMiscHxOccupationHx(MiscHxId, lstOccupationHxObjects, model.PatientId);
                            }
                            if (lstCaffeineIntakHxObjects.Count > 0)
                            {
                                string responseUpdateCaffeineIntakeHx = insertUpdateCaffeineIntakeHx(MiscHxId, lstCaffeineIntakHxObjects, model.PatientId);
                            }
                            //End 07/01/2016 Syed Zia Code to  Save/Update Occupation


                            //Start 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Sleep
                            if (lstSleepHxObjects.Count > 0)
                            {
                                string SleepHxResponse = insertUpdateSleepHx(MiscHxId, lstSleepHxObjects, model.PatientId);
                            }
                            //End 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Sleep

                            //Start 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Exercises
                            if (lstExercisesHxObjects.Count > 0)
                            {
                                string ExercisesHxResponse = insertUpdateExercisesHx(MiscHxId, lstExercisesHxObjects, model.PatientId);
                            }
                            //End 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Exercises

                            //Start 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Housing
                            if (lstHousingHxObjects.Count > 0)
                            {
                                string HousingHxResponse = insertUpdateHousingHx(MiscHxId, lstHousingHxObjects, model.PatientId);
                            }
                            //if (lstTravelHxModel.Count > 0)
                            //{
                            //    string TravelHxResponse = insertUpdateTravelHx(MiscHxId, lstTravelHxModel, model.PatientId);
                            //}

                            //End 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Housing


                            //Start 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Caffeine Intake
                            //End 07/01/2016 Muhammad Arshad Code to  Save/Update MiscHx Caffeine Intake
                        }
                    }
                    //Start 08/01/2016 Muhammad Arshad Code to  Save/Update SocialHx SoapText for MiscHx Tab
                    //  BLObject<string> objValue = BLLClinicalObj.updateSoapTextForSocialHX(SocialHxId.ToString());

                    //End 08/01/2016 Muhammad Arshad Code to  Save/Update SocialHx SoapText for MiscHx Tab
                    #region Database Updation
                    if (dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows.Count > 0)
                    {
                        BLObject<DSSocialHistory> objUpdate = BLLClinicalObj.UpdateSocialHx(dsSocialHx);

                        var SoapText = string.Empty;
                        var IsCreatedOrModified = string.Empty;
                        var LastUpdated = string.Empty;
                        var SoapInfo = getCurrentSoapText(SocialHxId);
                        if (SoapInfo != null)
                        {
                            SoapText = SoapInfo["SoapText"];
                            IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                            LastUpdated = SoapInfo["LastUpdated"];
                        }
                        if (objUpdate.Data != null)
                        {
                            dsSocialHx = objUpdate.Data;
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,
                                SoapText = SoapText,
                                IsCreatedOrModified = IsCreatedOrModified,
                                LastUpdated = LastUpdated,
                                MUAlertsCount = dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.MUAlertsCountColumn.ColumnName],
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
                        message = "Social History not found."
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

        #region fillSocialHx

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle fill of SocialHx
        public string fillSocialHx(SocialHxModel model, Int64 SocialHxId, string SocialHxType)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)) && SocialHxId == 0)
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
                    DSSocialHistory dsSocialHx = null;
                    BLObject<DSSocialHistory> obj = BLLClinicalObj.LoadSocialHx(MDVUtility.ToInt64(model.PatientId), SocialHxId, SocialHxType, "1", "");
                    dsSocialHx = obj.Data;
                    if (dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0];

                        var SoapText = string.Empty;
                        var IsCreatedOrModified = string.Empty;
                        var LastUpdated = string.Empty;
                        var SoapInfo = getCurrentSoapText(MDVUtility.ToInt64(dr[dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]));
                        if (SoapInfo != null)
                        {
                            SoapText = SoapInfo["SoapText"];
                            IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                            LastUpdated = SoapInfo["LastUpdated"];
                        }

                        string socialcomment = MDVUtility.ToStr(dr[dsSocialHx.SocialHx.CommentsColumn.ColumnName]);
                        if (socialcomment.Length > 0)
                        {
                            int pos1 = socialcomment.IndexOf("</strong>") + 9;
                            int pos2 = socialcomment.IndexOf("</div>");
                            socialcomment = socialcomment.Substring(pos1, pos2 - pos1);
                        }

                        var SocialHxkeyValues = new Dictionary<string, string>
                        {
                            { "SocialHxDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsSocialHx.SocialHx.SocialHxDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsSocialHx.SocialHx.SocialHxDateColumn.ColumnName]).ToShortDateString()},
                            { "SocialHxId",  MDVUtility.ToStr(dr[dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName])},
                            { "SocialHxUnremarkable", MDVUtility.ToStr(dr[dsSocialHx.SocialHx.bUnremarkableColumn.ColumnName])},
                            { "SocialComments", socialcomment},
                            { "SocialHxSoapText", MDVUtility.ToStr(dr[dsSocialHx.SocialHx.SoapTextColumn.ColumnName])},
                            { "SoapText", SoapText},
                            { "IsCreatedOrModified", IsCreatedOrModified},
                            { "LastUpdated",LastUpdated}
                        };

                        List<Dictionary<string, string>> lstTobaccoHx = new List<Dictionary<string, string>>();
                        foreach (DataRow drTobacco in dsSocialHx.Tables[dsSocialHx.SocialHx_Tobacco.TableName].Rows)
                        {
                            var TobaccoHxkeyValues = new Dictionary<string, string>
                            {
                                { "TobaccoId", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.TobaccoIdColumn.ColumnName])},
                                { "SmokingStatus", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.StatusIdColumn.ColumnName])},
                                { "TobaccoType", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.TypeIdColumn.ColumnName])},
                                { "TobaccoUsagePeriod", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.UsagePeriodIdColumn.ColumnName])},
                                { "TobaccoFrequencyDaily", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.FrequencyIdColumn.ColumnName])},
                                { "TobaccoCounsellingPeriod", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.CounsellingPeriodIdColumn.ColumnName])},
                                { "TobaccoCounsellingTopic", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.CounsellingTopicIdColumn.ColumnName])},
                                { "TobaccoCessationLength", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.CessationLengthColumn.ColumnName])},
                                { "TobaccoCessationPeriod", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.CessationPeriodIdColumn.ColumnName])},
                                { "TobaccoRecentlyQuit", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.bRecentlyQuitColumn.ColumnName])},
                                { "TobaccoWouldQuit", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.bWouldQuitColumn.ColumnName])},
                                { "TobaccoComments", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.CommentsColumn.ColumnName])},
                                { "TobaccoSoapText", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.SoapTextColumn.ColumnName])}
                            };
                            lstTobaccoHx.Add(TobaccoHxkeyValues);
                        }

                        List<Dictionary<string, string>> lstAlcoholHx = new List<Dictionary<string, string>>();
                        foreach (DataRow drAlcohol in dsSocialHx.Tables[dsSocialHx.SocialHx_Alcohol.TableName].Rows)
                        {
                            var AlcoholHxkeyValues = new Dictionary<string, string>
                            {
                                { "AlcoholId", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.AlcoholIdColumn.ColumnName])},
                                { "AlcoholStatus", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.StatusIdColumn.ColumnName])},
                                { "AlcoholType", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.TypeIdColumn.ColumnName])},
                                { "AlcoholUsagePeriod", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.UsagePeriodIdColumn.ColumnName])},
                                { "AlcoholFrequencyDays", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.FrequencyIdColumn.ColumnName])},
                                { "AlcoholCounsellingPeriod", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.CounsellingPeriodIdColumn.ColumnName])},
                                { "AlcoholCounsellingTopic", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.CounsellingTopicIdColumn.ColumnName])},
                                { "AlcoholCessationLength", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.CessationLengthColumn.ColumnName])},
                                { "AlcoholCessationPeriod", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.CessationPeriodIdColumn.ColumnName])},
                                { "AlcoholRecentlyQuit", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.bRecentlyQuitColumn.ColumnName])},
                                { "AlcoholNotReadyToQuit", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.bNotReadyToQuitColumn.ColumnName])},
                                { "AlcoholWouldQuit", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.bWouldQuitColumn.ColumnName])},
                                { "AlcoholComments", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.CommentsColumn.ColumnName])},
                                { "AlcoholSoapText", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.SoapTextColumn.ColumnName])}
                            };
                            lstAlcoholHx.Add(AlcoholHxkeyValues);
                        }

                        List<Dictionary<string, string>> lstDrugAbuse = new List<Dictionary<string, string>>();
                        foreach (DataRow drDrugAbuse in dsSocialHx.Tables[dsSocialHx.SocialHx_DrugAbuse.TableName].Rows)
                        {
                            var DrugAbusekeyValues = new Dictionary<string, string>
                            {
                                { "DrugAbuseId", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.DrugAbuseIdColumn.ColumnName])},
                                { "DrugStatus", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.StatusIdColumn.ColumnName])},
                                { "DrugType", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.DrugIdColumn.ColumnName])},
                                { "DrugRoute", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.RouteIdColumn.ColumnName])},
                                { "DrugFrequencyDay", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.FrequencyDailyIdColumn.ColumnName])},
                                { "DrugFrequencyMonth", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.FrequencyMonthlyIdColumn.ColumnName])},
                                { "DrugUsagePeriod", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.UsagePeriodIdColumn.ColumnName])},
                                { "DrugCessationLength", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.CessationLengthColumn.ColumnName])},
                                { "DrugCessationPeriod", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.CessationPeriodIdColumn.ColumnName])},
                                { "DrugRecentlyQuit", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.bRecentlyQuitColumn.ColumnName])},
                                { "DrugWouldQuit", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.bWouldQuitColumn.ColumnName])},
                                { "DrugComments", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.CommentsColumn.ColumnName])},
                                { "DrugSoapText", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.SoapTextColumn.ColumnName])}
                            };
                            lstDrugAbuse.Add(DrugAbusekeyValues);
                        }

                        List<Dictionary<string, string>> lstSexualHx = new List<Dictionary<string, string>>();
                        foreach (DataRow drSexualHx in dsSocialHx.Tables[dsSocialHx.SocialHx_SexualHx.TableName].Rows)
                        {
                            var SexualHxkeyValues = new Dictionary<string, string>
                            {
                                { "SexualHxId", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.SexualHxIdColumn.ColumnName])},
                                { "SexualStatus", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.StatusIdColumn.ColumnName])},
                                { "SexualPreferences", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.PreferenceIdColumn.ColumnName])},
                                { "SexualbUsingProtection", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bUSingProtectionColumn.ColumnName]).ToString().ToLower() == "true" ? "Yes" :"No"},
                                { "RadSexualYesPregnant", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bPregnancyStatusColumn.ColumnName]).ToString().ToLower() == "true" ? "Yes" :"No"},
                                { "SexualHxPregnancyDuration", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.PregnancyDurationColumn.ColumnName])},
                                { "SexualProtectionMethod", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.ProtectionMethodIdColumn.ColumnName])},
                                { "SexualProtectionPeriod", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.ProtectionPeriodIdColumn.ColumnName])},
                                { "RadSexualYesPainWithIntercourse", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bPainWithIntercourseColumn.ColumnName]).ToString().ToLower() == "true" ? "Yes" :"No"},
                                { "SexualComplaints", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.ComplaintIdColumn.ColumnName])},
                                { "SexualExposedToSTD", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bExposedToSTDColumn.ColumnName]).ToString().ToLower() == "true" ? "Yes" :"No"},
                                { "SexualSTD", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.STDIdsColumn.ColumnName])},
                                { "RadSexualYesAbusedSexually", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bSexuallyAbusedColumn.ColumnName]).ToString().ToLower() == "true" ? "Yes" :"No"},
                                { "SexualComments", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.CommentsColumn.ColumnName])},
                                { "SexualLMP",  String.IsNullOrEmpty(MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.LMPColumn.ColumnName]))?"": MDVUtility.GetDateMMDDYYY(drSexualHx[dsSocialHx.SocialHx_SexualHx.LMPColumn.ColumnName].ToString())},
                                { "SexualSoapText", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.SoapTextColumn.ColumnName])}
                            };
                            lstSexualHx.Add(SexualHxkeyValues);
                        }

                        List<Dictionary<string, string>> lstMiscHxOccupationHx = new List<Dictionary<string, string>>();
                        foreach (DataRow drMiscHx in dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_OccupationHx.TableName].Rows)
                        {
                            var MiscHxkeyValues = new Dictionary<string, string>
                            {
                                { "MiscHxId",MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.MiscHxIdColumn.ColumnName])},
                                { "OccupationHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.OccupationHxIdColumn.ColumnName])},
                                { "MiscChildStatus", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.StatusIdColumn.ColumnName])},
                                { "OccupationPresent", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.PresentColumn.ColumnName])},
                                { "OccupationPast", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.PastColumn.ColumnName])},
                                { "OccupationComments", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.CommentsColumn.ColumnName])},
                                { "OccupationSoapText", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx.SoapTextColumn.ColumnName])}
                            };
                            lstMiscHxOccupationHx.Add(MiscHxkeyValues);
                        }

                        List<Dictionary<string, string>> lstMiscHxCaffeineIntakeHx = new List<Dictionary<string, string>>();
                        foreach (DataRow drMiscHx in dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows)
                        {
                            var MiscHxkeyValues = new Dictionary<string, string>
                            {
                                { "MiscHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.MiscHxIdColumn.ColumnName])},
                                { "CaffeineIntakeHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.CaffeineIntakHxIdColumn.ColumnName])},
                                { "MiscChildStatus", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.StatusIdColumn.ColumnName])},
                                { "CaffeineIntakFrequency", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.FrequencyIdColumn.ColumnName])},
                                { "CaffeineHarmful", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.IsHarmfulColumn.ColumnName])},
                                { "CaffeineComments", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.CommentsColumn.ColumnName])},
                                { "OccupationSoapText", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.SoapTextColumn.ColumnName])}
                            };
                            lstMiscHxCaffeineIntakeHx.Add(MiscHxkeyValues);
                        }
                        List<Dictionary<string, string>> lstMiscHxTravelIntakeHx = new List<Dictionary<string, string>>();
                        foreach (DataRow drMiscHx in dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_TravelHx.TableName].Rows)
                        {
                            var MiscHxkeyValues = new Dictionary<string, string>
                            {
                                { "MiscHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.MiscHxIdColumn.ColumnName])},
                                { "TravelHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.TravelHxIdColumn.ColumnName])},
                                { "MiscChildStatus", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.StatusIdColumn.ColumnName])},
                                { "TravelHxFromDate", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.FromDateColumn.ColumnName])},
                                { "TravelHxToDate", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.TodateColumn.ColumnName])},
                                { "TravelHxComments", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.CommentsColumn.ColumnName])},
                                { "TravelHxLocation", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.LocationColumn.ColumnName])}
                            };
                            lstMiscHxTravelIntakeHx.Add(MiscHxkeyValues);
                        }
                        //Start Farooq Ahmad 11/01/2016 SocialHx_MiscHx_SleepHx table to list of dictionary
                        List<Dictionary<string, string>> lstMiscHxSleepHx = new List<Dictionary<string, string>>();
                        foreach (DataRow drMiscHx in dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_SleepHx.TableName].Rows)
                        {
                            var MiscHxSleepHx = new Dictionary<string, string>
                            {
                                { "MiscHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.MiscHxIdColumn.ColumnName])},
                                { "SleepHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.SleepHxIdColumn.ColumnName])},
                                { "MiscChildStatus", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.StatusIdColumn.ColumnName])},
                                { "SleepHours", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.SleepHoursColumn.ColumnName])},
                                { "SleepComments", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.CommentsColumn.ColumnName])},
                                { "SleepSoapText", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.SoapTextColumn.ColumnName])}
                            };
                            lstMiscHxSleepHx.Add(MiscHxSleepHx);
                        }
                        //End Farooq Ahmad 11/01/2016 SocialHx_MiscHx_SleepHx table to list of dictionary

                        //Start Farooq Ahmad 11/01/2016 SocialHx_MiscHx_ExercisesHx table to list of dictionary
                        List<Dictionary<string, string>> lstMiscHxExercisesHx = new List<Dictionary<string, string>>();
                        foreach (DataRow drMiscHx in dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_ExercisesHx.TableName].Rows)
                        {
                            var MiscHxExercisesHx = new Dictionary<string, string>
                            {
                                { "MiscHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.MiscHxIdColumn.ColumnName])},
                                { "ExercisesHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.ExercisesHxIdColumn])},
                                { "MiscChildStatus", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.StatusIdColumn.ColumnName])},
                                { "ExercisesType", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.TypeIdColumn.ColumnName])},
                                { "ExercisesDiet", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.DietIdColumn.ColumnName])},
                                { "ExercisesComments", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.CommentsColumn.ColumnName])},
                                { "ExercisesSoapText", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.SoapTextColumn.ColumnName])}
                            };
                            lstMiscHxExercisesHx.Add(MiscHxExercisesHx);
                        }
                        //Start Farooq Ahmad 11/01/2016 SocialHx_MiscHx_ExercisesHx table to list of dictionary

                        //Start Farooq Ahmad 11/01/2016 SocialHx_MiscHx_HousingHx table to list of dictionary
                        List<Dictionary<string, string>> lstMiscHxHousingHx = new List<Dictionary<string, string>>();
                        foreach (DataRow drMiscHx in dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_HousingHx.TableName].Rows)
                        {
                            var MiscHxHousingHx = new Dictionary<string, string>
                            {
                                { "MiscHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.MiscHxIdColumn.ColumnName])},
                                { "HousingHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.HousingHxIdColumn])},
                                { "MiscChildStatus", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.StatusIdColumn.ColumnName])},
                                { "HousingPresent", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.PresentColumn.ColumnName])},
                                { "HousingPast", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.PastColumn.ColumnName])},
                                { "HousingComments", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.CommentsColumn.ColumnName])},
                                { "HousingSoapText", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.SoapTextColumn.ColumnName])}
                            };
                            lstMiscHxHousingHx.Add(MiscHxHousingHx);
                        }
                        //Start Farooq Ahmad 11/01/2016 SocialHx_MiscHx_HousingHx table to list of dictionary

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            SocialHxFill_JSON = js.Serialize(SocialHxkeyValues),
                            TobaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
                            AlcoholHxFill_JSON = js.Serialize(lstAlcoholHx),
                            DrugAbuseFill_JSON = js.Serialize(lstDrugAbuse),
                            SexualHxFill_JSON = js.Serialize(lstSexualHx),
                            OccupationHxFill_JSON = js.Serialize(lstMiscHxOccupationHx),
                            //Start Farooq Ahmad 11/01/2016 Adding to Json Array
                            SleepHxFill_JSON = js.Serialize(lstMiscHxSleepHx),
                            ExercisesHxFill_JSON = js.Serialize(lstMiscHxExercisesHx),
                            HousingHxFill_JSON = js.Serialize(lstMiscHxHousingHx),
                            //End Farooq Ahmad 11/01/2016 Adding to Json Array
                            CaffeineIntakeHxFill_JSON = js.Serialize(lstMiscHxCaffeineIntakeHx),
                            TravelHxFill_JSON = js.Serialize(lstMiscHxTravelIntakeHx),
                            socialHxLoad_JSON = MDVUtility.JSON_DataTable(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName]),
                            socialHxMiscHxComponentLoad_JSON = MDVUtility.JSON_DataTable(dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_Component.TableName]),
                            SoapText = SoapText,
                            IsCreatedOrModified = IsCreatedOrModified,
                            LastUpdated = LastUpdated
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            socialHxMiscHxComponentLoad_JSON = MDVUtility.JSON_DataTable(dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_Component.TableName])
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

        public string FillSocialHxNative(SocialHxModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)))
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
                    DSHxMobileApp dsSocialHx = null;
                    BLObject<DSHxMobileApp> obj = BLLClinicalObj.LoadSocialHxMobileApp(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToStr(model.Status));
                    dsSocialHx = obj.Data;
                    //if (dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows.Count > 0)
                    //{
                    //    DataRow dr = dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0];

                    List<Dictionary<string, string>> lstTobaccoHx = new List<Dictionary<string, string>>();
                    foreach (DataRow drTobacco in dsSocialHx.Tables[dsSocialHx.SocialHx_Tobacco.TableName].Rows)
                    {
                        var TobaccoHxkeyValues = new Dictionary<string, string>
                            {
                                //{ "TobaccoId", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.TobaccoIdColumn.ColumnName])},
                                { "ddlSmokingStatus", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.StatusIdColumn.ColumnName])},
                                { "ddlTobaccoType", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.TypeIdColumn.ColumnName])},
                                { "ddlTobaccoUsagePeriod", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.UsagePeriodIdColumn.ColumnName])},
                                { "ddlTobaccoFrequencyDaily", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.FrequencyIdColumn.ColumnName])},
                                { "ddlTobaccoCounsellingPeriod", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.CounsellingPeriodIdColumn.ColumnName])},
                                { "ddlTobaccoCounsellingTopic", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.CounsellingTopicIdColumn.ColumnName])},
                                { "ddlTobaccoCessationPeriod", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.CessationPeriodIdColumn.ColumnName])},
                                { "chkTobaccoRecentlyQuit", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.bRecentlyQuitColumn.ColumnName])},
                                { "chkTobaccoWouldQuit", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.bWouldQuitColumn.ColumnName])},
                                { "txtTobaccoComments", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.CommentsColumn.ColumnName])},
                                { "txtTobaccoCessationLength", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.CessationLengthColumn.ColumnName])},
                                { "hfCreatedbyTabbacco", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.CreatedByColumn.ColumnName])},
                                { "hfCreatedonTabbacco", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.CreatedOnColumn.ColumnName])},
                                { "hfModifiedbyTabbacco", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.ModifiedByColumn.ColumnName])},
                                { "hfModifiedonTabbacco", MDVUtility.ToStr(drTobacco[dsSocialHx.SocialHx_Tobacco.ModifiedOnColumn.ColumnName])},
                            };
                        lstTobaccoHx.Add(TobaccoHxkeyValues);
                    }

                    List<Dictionary<string, string>> lstAlcoholHx = new List<Dictionary<string, string>>();
                    foreach (DataRow drAlcohol in dsSocialHx.Tables[dsSocialHx.SocialHx_Alcohol.TableName].Rows)
                    {
                        var AlcoholHxkeyValues = new Dictionary<string, string>
                            {
                                //{ "AlcoholId", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.AlcoholIdColumn.ColumnName])},
                                { "ddlAlcoholStatus", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.StatusIdColumn.ColumnName])},
                                { "ddlAlcoholType", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.TypeIdColumn.ColumnName])},
                                { "ddlAlcoholUsagePeriod", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.UsagePeriodIdColumn.ColumnName])},
                                { "ddlAlcoholFrequencyDays", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.FrequencyIdColumn.ColumnName])},
                                { "ddlAlcoholCounsellingPeriod", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.CounsellingPeriodIdColumn.ColumnName])},
                                { "ddlAlcoholCounsellingTopic", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.CounsellingTopicIdColumn.ColumnName])},
                                { "txtAlcoholCessationLength", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.CessationLengthColumn.ColumnName])},
                                { "ddlAlcoholCessationPeriod", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.CessationPeriodIdColumn.ColumnName])},
                                { "chkAlcoholRecentlyQuit", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.bRecentlyQuitColumn.ColumnName])},
                                { "chkAlcoholWouldQuit", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.bWouldQuitColumn.ColumnName])},
                                { "txtAlcoholComments", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.CommentsColumn.ColumnName])},
                                { "chkAlcoholNotReadyToQuit", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.bNotReadyToQuitColumn.ColumnName])},
                                { "hfCreatedbyAlcohol", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.CreatedByColumn.ColumnName])},
                                { "hfCreatedonAlcohol", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.CreatedOnColumn.ColumnName])},
                                { "hfModifiedbyAlcohol", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.ModifiedByColumn.ColumnName])},
                                { "hfModifiedonAlcohol", MDVUtility.ToStr(drAlcohol[dsSocialHx.SocialHx_Alcohol.ModifiedOnColumn.ColumnName])},
                            };
                        lstAlcoholHx.Add(AlcoholHxkeyValues);
                    }

                    List<Dictionary<string, string>> lstDrugAbuse = new List<Dictionary<string, string>>();
                    foreach (DataRow drDrugAbuse in dsSocialHx.Tables[dsSocialHx.SocialHx_DrugAbuse.TableName].Rows)
                    {
                        var DrugAbusekeyValues = new Dictionary<string, string>
                            {
                                //{ "DrugAbuseId", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.DrugAbuseIdColumn.ColumnName])},
                                { "ddlDrugStatus", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.StatusIdColumn.ColumnName])},
                                { "ddlDrugType", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.DrugIdColumn.ColumnName])},
                                { "ddlDrugRoute", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.RouteIdColumn.ColumnName])},
                                { "txtDrugCessationLength", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.CessationLengthColumn.ColumnName])},
                                { "ddlDrugCessationPeriod", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.CessationPeriodIdColumn.ColumnName])},
                                { "chkDrugRecentlyQuit", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.bRecentlyQuitColumn.ColumnName])},
                                { "chkDrugWouldQuit", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.bWouldQuitColumn.ColumnName])},
                                { "txtDrugComments", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.CommentsColumn.ColumnName])},
                                { "ddlDrugUsagePeriod", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.UsagePeriodIdColumn.ColumnName])},
                                { "ddlDrugFrequencyDaily", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.FrequencyDailyIdColumn.ColumnName])},
                                { "hfCreatedbyDrugAbuse", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.CreatedByColumn.ColumnName])},
                                { "hfCreatedonDrugAbuse", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.CreatedOnColumn.ColumnName])},
                                { "hfModifiedbyDrugAbuse", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.ModifiedByColumn.ColumnName])},
                                { "hfModifiedonDrugAbuse", MDVUtility.ToStr(drDrugAbuse[dsSocialHx.SocialHx_DrugAbuse.ModifiedOnColumn.ColumnName])},
                            };
                        lstDrugAbuse.Add(DrugAbusekeyValues);
                    }

                    List<Dictionary<string, string>> lstSexualHx = new List<Dictionary<string, string>>();
                    foreach (DataRow drSexualHx in dsSocialHx.Tables[dsSocialHx.SocialHx_SexualHx.TableName].Rows)
                    {
                        if (MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bUSingProtectionColumn.ColumnName]).ToString().ToLower() == "1")
                        {
                            drSexualHx[dsSocialHx.SocialHx_SexualHx.bUSingProtectionColumn.ColumnName]= "yes";
                        }
                        if (MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bExposedToSTDColumn.ColumnName]).ToString().ToLower()=="1")
                        {
                            drSexualHx[dsSocialHx.SocialHx_SexualHx.bExposedToSTDColumn.ColumnName] = "yes";
                        }
                        var SexualHxkeyValues = new Dictionary<string, string>
                            {
                                //{ "SexualHxId", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.SexualHxIdColumn.ColumnName])},
                                { "ddlSexualStatus", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.StatusIdColumn.ColumnName])},
                                { "ddlSexualPreferences", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.PreferenceIdColumn.ColumnName])},
                               
                                { "ddlSexualProtectionMethod", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.ProtectionMethodIdColumn.ColumnName])},
                                { "ddlSexualProtectionPeriod", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.ProtectionPeriodIdColumn.ColumnName])},
                                { "ddlSexualbUsingProtection", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bUSingProtectionColumn.ColumnName]).ToString().ToLower() == "yes" ? "1" :"0" },
                                { "RadSexualYesPainWithIntercourse", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bPainWithIntercourseColumn.ColumnName]).ToString().ToLower() == "true" ? "true" :"false"},
                                { "RadSexualNoPainWithIntercourse", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bPainWithIntercourseColumn.ColumnName]).ToString().ToLower() == "false" ? "true" :"false"},
                                { "ddlSexualComplaints", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.ComplaintIdColumn.ColumnName])},
                                { "ddlSexualExposedToSTD", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bExposedToSTDColumn.ColumnName]).ToString().ToLower() == "yes" ? "1" :"0"},
                                { "ddlSexualSTD", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.STDIdsColumn.ColumnName])},
                                { "RadSexualYesAbusedSexually", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bSexuallyAbusedColumn.ColumnName]).ToString().ToLower() == "true" ? "true" :"false"},
                                { "RadSexualNoAbusedSexually", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bSexuallyAbusedColumn.ColumnName]).ToString().ToLower() == "false" ? "true" :"false"},
                                { "SexualLMP",  String.IsNullOrEmpty(MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.LMPColumn.ColumnName]))?"": MDVUtility.ToDateTime(drSexualHx[dsSocialHx.SocialHx_SexualHx.LMPColumn.ColumnName]).ToShortDateString()},
                                { "hfCreatedbySexual", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.CreatedByColumn.ColumnName])},
                                { "hfCreatedonSexual", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.CreatedOnColumn.ColumnName])},
                                { "hfModifiedbySexual", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.ModifiedByColumn.ColumnName])},
                                { "hfModifiedonSexual", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.ModifiedOnColumn.ColumnName])},
                                { "RadSexualYesPregnant", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bPregnancyStatusColumn.ColumnName]).ToString().ToLower() == "true" ? "true" :"false"},
                                { "RadSexualNoPregnant", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.bPregnancyStatusColumn.ColumnName]).ToString().ToLower() == "false" ? "true" :"false"},
                                { "SocialHxComments", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.CommentsColumn.ColumnName])},
                                { "PregnancyDuration", MDVUtility.ToStr(drSexualHx[dsSocialHx.SocialHx_SexualHx.PregnancyDurationColumn.ColumnName])},
                            };
                        lstSexualHx.Add(SexualHxkeyValues);
                    }

                    List<Dictionary<string, string>> lstMiscHxOccupationHx = new List<Dictionary<string, string>>();
                    foreach (DataRow drMiscHx in dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_OccupationHx.TableName].Rows)
                    {
                        var MiscHxkeyValues = new Dictionary<string, string>
                            {
                                //{ "MiscHxId",MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.MiscHxIdColumn.ColumnName])},
                                //{ "OccupationHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.OccupationHxIdColumn.ColumnName])},
                                { "ddlOccupationStatus", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.StatusIdColumn.ColumnName])},
                                { "RadPresentYes", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.IsPastColumn.ColumnName]).ToString().ToLower()=="false"?"true":"false"},
                                { "RadPresentNo", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.IsPastColumn.ColumnName])},
                                { "txtOccupationDetail", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.OccupationDetailColumn.ColumnName])},
                                { "TxtOccupationComments", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.CommentsColumn.ColumnName])},
                                { "dtOccupationHxStartDate", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.StartDateColumn.ColumnName])},
                                { "dtOccupationHxEndDate", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.EndDateColumn.ColumnName])},
                                { "hfCreatedbyOccupation", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.CreatedByColumn.ColumnName])},
                                { "hfCreatedonOccupation", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.CreatedOnColumn.ColumnName])},
                                { "hfModifiedbyOccupation", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.ModifiedByColumn.ColumnName])},
                                { "hfModifiedonOccupation", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_OccupationHx.ModifiedOnColumn.ColumnName])},
                            };
                        lstMiscHxOccupationHx.Add(MiscHxkeyValues);
                    }

                    List<Dictionary<string, string>> lstMiscHxCaffeineIntakeHx = new List<Dictionary<string, string>>();
                    foreach (DataRow drMiscHx in dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.TableName].Rows)
                    {
                        var MiscHxkeyValues = new Dictionary<string, string>
                            {
                                //{ "MiscHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.MiscHxIdColumn.ColumnName])},
                                //{ "CaffeineIntakeHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.CaffeineIntakHxIdColumn.ColumnName])},
                                { "ddlCaffineStatus", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.StatusIdColumn.ColumnName])},
                                { "ddlCaffineFrequency", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.FrequencyIdColumn.ColumnName])},
                                { "RadIsHarmfulYes", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.IsHarmfulColumn.ColumnName])},
                                { "RadIsHarmfulNo", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.IsHarmfulColumn.ColumnName]).ToLower() == "0" ? "1" : "0"},
                                { "TxtCaffineComments", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.CommentsColumn.ColumnName])},
                                { "hfCreatedbyCaffeine", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.CreatedByColumn.ColumnName])},
                                { "hfCreatedonCaffeine", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.CreatedOnColumn.ColumnName])},
                                { "hfModifiedbyCaffeine", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.ModifiedByColumn.ColumnName])},
                                { "hfModifiedonCaffeine", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_CaffeineIntakHx.ModifiedOnColumn.ColumnName])},
                            };
                        lstMiscHxCaffeineIntakeHx.Add(MiscHxkeyValues);
                    }

                    List<Dictionary<string, string>> lstMiscHxSleepHx = new List<Dictionary<string, string>>();
                    foreach (DataRow drMiscHx in dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_SleepHx.TableName].Rows)
                    {
                        var MiscHxSleepHx = new Dictionary<string, string>
                            {
                                //{ "MiscHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.MiscHxIdColumn.ColumnName])},
                                //{ "SleepHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.SleepHxIdColumn.ColumnName])},
                                { "ddlSleepStatus", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.StatusIdColumn.ColumnName])},
                                { "TxtSleepHours", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.SleepHoursColumn.ColumnName])},
                                { "TxtSleepComments", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.CommentsColumn.ColumnName])},
                                { "hfCreatedbySleep", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.CreatedByColumn.ColumnName])},
                                { "hfCreatedonSleep", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.CreatedOnColumn.ColumnName])},
                                { "hfModifiedbySleep", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.ModifiedByColumn.ColumnName])},
                                { "hfModifiedonSleep", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_SleepHx.ModifiedOnColumn.ColumnName])},
                            };
                        lstMiscHxSleepHx.Add(MiscHxSleepHx);
                    }

                    List<Dictionary<string, string>> lstMiscHxExercisesHx = new List<Dictionary<string, string>>();
                    foreach (DataRow drMiscHx in dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_ExercisesHx.TableName].Rows)
                    {
                        var MiscHxExercisesHx = new Dictionary<string, string>
                            {
                                //{ "MiscHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.MiscHxIdColumn.ColumnName])},
                                //{ "ExercisesHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.ExercisesHxIdColumn])},
                                { "ddlExerciseStatus", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.StatusIdColumn.ColumnName])},
                                { "ddlExerciseType", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.TypeIdColumn.ColumnName])},
                                { "ddlExerciseDiet", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.DietIdColumn.ColumnName])},
                                { "TxtExerciseComments", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.CommentsColumn.ColumnName])},
                                { "hfCreatedbyExercise", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.CreatedByColumn.ColumnName])},
                                { "hfCreatedonExercise", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.CreatedOnColumn.ColumnName])},
                                { "hfModifiedbyExercise", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.ModifiedByColumn.ColumnName])},
                                { "hfModifiedonExercise", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_ExercisesHx.ModifiedOnColumn.ColumnName])},
                            };
                        lstMiscHxExercisesHx.Add(MiscHxExercisesHx);
                    }

                    List<Dictionary<string, string>> lstMiscHxHousingHx = new List<Dictionary<string, string>>();
                    foreach (DataRow drMiscHx in dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_HousingHx.TableName].Rows)
                    {
                        var MiscHxHousingHx = new Dictionary<string, string>
                            {
                                //{ "MiscHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.MiscHxIdColumn.ColumnName])},
                                //{ "HousingHxId", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.HousingHxIdColumn])},
                                { "ddlHousingStatus", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.StatusIdColumn.ColumnName])},
                                { "TxtHousingPresent", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.PresentColumn.ColumnName])},
                                { "TxtHousingPast", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.PastColumn.ColumnName])},
                                { "TxtHousingComments", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.CommentsColumn.ColumnName])},
                                { "hfCreatedbyHousing", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.CreatedByColumn.ColumnName])},
                                { "hfCreatedonHousing", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.CreatedOnColumn.ColumnName])},
                                { "hfModifiedbyHousing", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.ModifiedByColumn.ColumnName])},
                                { "hfModifiedonHousing", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.ModifiedOnColumn.ColumnName])},
                            };
                        lstMiscHxHousingHx.Add(MiscHxHousingHx);
                    }

                    List<Dictionary<string, string>> lstMiscHxTravelHx = new List<Dictionary<string, string>>();
                    foreach (DataRow drMiscHx in dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_TravelHx.TableName].Rows)
                    {
                        var MiscHxTravelHx = new Dictionary<string, string>
                            {
                                
                                { "ddlTravelStatus", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.StatusIdColumn.ColumnName])},
                                { "dtTravelHxFromDate", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.FromDateColumn.ColumnName])},
                                { "dtTravelHxToDate", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.ToDateColumn.ColumnName])},
                                { "txtTravelHxLocation", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.LocationColumn.ColumnName])},
                                { "txtTravelHxComments", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.CommentsColumn.ColumnName])},
                                { "hfCreatedbyTravel", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.CreatedByColumn.ColumnName])},
                                { "hfCreatedonTravel", MDVUtility.GetDateMMDDYYY(drMiscHx[dsSocialHx.SocialHx_MiscHx_TravelHx.CreatedOnColumn.ColumnName].ToString())},
                                { "hfModifiedbyTravel", MDVUtility.ToStr(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.ModifiedByColumn.ColumnName])},
                                { "hfModifiedonTravel", MDVUtility.GetDateMMDDYYY(drMiscHx[dsSocialHx.SocialHx_MiscHx_HousingHx.ModifiedOnColumn.ColumnName].ToString())},
                            };
                        lstMiscHxTravelHx.Add(MiscHxTravelHx);
                    }
                    string ChangedColumns = "";

                    DataColumn dc = dsSocialHx.Tables["SocialHx_ChangedList"].Columns[2] as DataColumn;
                    ChangedColumns = string.Join(", ", dsSocialHx.Tables["SocialHx_ChangedList"].Rows.OfType<DataRow>().Select(r => r[dc]));


                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ChangedColumns,
                        TobaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
                        AlcoholHxFill_JSON = js.Serialize(lstAlcoholHx),
                        DrugAbuseFill_JSON = js.Serialize(lstDrugAbuse),
                        SexualHxFill_JSON = js.Serialize(lstSexualHx),
                        OccupationHxFill_JSON = js.Serialize(lstMiscHxOccupationHx),
                        SleepHxFill_JSON = js.Serialize(lstMiscHxSleepHx),
                        ExercisesHxFill_JSON = js.Serialize(lstMiscHxExercisesHx),
                        HousingHxFill_JSON = js.Serialize(lstMiscHxHousingHx),
                        CaffeineIntakeHxFill_JSON = js.Serialize(lstMiscHxCaffeineIntakeHx),
                        TravelHxFill_JSON=js.Serialize(lstMiscHxTravelHx),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    //}
                    //else
                    //{
                    //    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    //    var response = new
                    //    {
                    //        status = false,
                    //    };
                    //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    //}
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

        public string saveSocialHxNative(SocialHxModel model)//, string fieldsJSON, List<object> lstTobaccoObjects, List<object> lstAlcoholObjects, List<object> lstDrugAbuseObjects, List<object> lstSexualHxObjects, List<object> lstOccupationHxObjects, List<object> lstCaffeineIntakHxObjects, List<object> lstSleepHxObjects, List<object> lstExercisesHxObjects, List<object> lstHousingHxObjects)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                DSSocialHistory dsSocialHx = new DSSocialHistory();
                Int64 SocialHxId = 0;
                BLObject<DSSocialHistory> objLoad = BLLClinicalObj.CheckSocialHxExists(MDVUtility.ToInt64(model.PatientId));
                dsSocialHx = objLoad.Data;
                if (dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows.Count > 0)
                {
                    SocialHxId = MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]);
                }
                else
                {
                    DSSocialHistory.SocialHxRow dr = dsSocialHx.SocialHx.NewSocialHxRow();

                    dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                    if (!string.IsNullOrEmpty(model.SocialHxDate))
                    {
                        dr.SocialHxDate = MDVUtility.ToDateTime(model.SocialHxDate);
                    }
                    else
                    {
                        dr[dsSocialHx.SocialHx.SocialHxDateColumn] = DateTime.Now;
                    }

                    if (!string.IsNullOrEmpty(model.SocialComments))
                    {
                        dr.Comments = "<div id='overallcomment' title='overallcomment'  name='overallcomment'><strong>Overall Comments:</strong> " + MDVUtility.ToStr(model.SocialComments) + "</div>";
                    }
                    else
                    {
                        dr[dsSocialHx.SocialHx.CommentsColumn] = DBNull.Value;
                    }

                    dr.bUnremarkable = model.SocialHxUnremarkable.ToLower() == "true" ? true : false;

                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsSocialHx.SocialHx.AddSocialHxRow(dr);
                    BLObject<DSSocialHistory> obj = BLLClinicalObj.InsertSocialHx(dsSocialHx);
                    dsSocialHx = obj.Data;

                    if (obj.Data != null)
                    {
                        SocialHxId = MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]);
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
                if (SocialHxId > 0)
                {
                    var TabbaccoJSON = ser.Deserialize<dynamic>(model.TabbacooData);
                    var AlcoholJSON = ser.Deserialize<dynamic>(model.AlcoholData);
                    var DrugAbuseJSON = ser.Deserialize<dynamic>(model.DrugAbuseData);
                    var SexualJSON = ser.Deserialize<dynamic>(model.SexualData);
                    var OccupationJSON = ser.Deserialize<dynamic>(model.OccupationData);
                    var SleepJSON = ser.Deserialize<dynamic>(model.SleepData);
                    var ExerciseJSON = ser.Deserialize<dynamic>(model.ExerciseData);
                    var HousingJSON = ser.Deserialize<dynamic>(model.HousingData);
                    var CaffeineJSON = ser.Deserialize<dynamic>(model.CaffeineData);
                    var TravelJSON = ser.Deserialize<dynamic>(model.TravelData);
                    DSHxMobileApp dsHxNative = new DSHxMobileApp();

                    DSHxMobileApp.SocialHx_TobaccoRow dtr = dsHxNative.SocialHx_Tobacco.NewSocialHx_TobaccoRow();
                    DSHxMobileApp.SocialHx_AlcoholRow dar = dsHxNative.SocialHx_Alcohol.NewSocialHx_AlcoholRow();
                    DSHxMobileApp.SocialHx_DrugAbuseRow ddr = dsHxNative.SocialHx_DrugAbuse.NewSocialHx_DrugAbuseRow();
                    DSHxMobileApp.SocialHx_SexualHxRow dsr = dsHxNative.SocialHx_SexualHx.NewSocialHx_SexualHxRow();
                    DSHxMobileApp.SocialHx_MiscHx_OccupationHxRow dmor = dsHxNative.SocialHx_MiscHx_OccupationHx.NewSocialHx_MiscHx_OccupationHxRow();
                    DSHxMobileApp.SocialHx_MiscHx_SleepHxRow dmsr = dsHxNative.SocialHx_MiscHx_SleepHx.NewSocialHx_MiscHx_SleepHxRow();
                    DSHxMobileApp.SocialHx_MiscHx_ExercisesHxRow dmer = dsHxNative.SocialHx_MiscHx_ExercisesHx.NewSocialHx_MiscHx_ExercisesHxRow();
                    DSHxMobileApp.SocialHx_MiscHx_HousingHxRow dmhr = dsHxNative.SocialHx_MiscHx_HousingHx.NewSocialHx_MiscHx_HousingHxRow();
                    DSHxMobileApp.SocialHx_MiscHx_CaffeineIntakHxRow dmcr = dsHxNative.SocialHx_MiscHx_CaffeineIntakHx.NewSocialHx_MiscHx_CaffeineIntakHxRow();

                    DSHxMobileApp.SocialHx_MiscHx_TravelHxRow dmT = dsHxNative.SocialHx_MiscHx_TravelHx.NewSocialHx_MiscHx_TravelHxRow();
                    StringBuilder sbTabbacco = new StringBuilder();
                    StringBuilder sbAlcohol = new StringBuilder();
                    StringBuilder sbDrugAbuse = new StringBuilder();
                    StringBuilder sbSexual = new StringBuilder();
                    StringBuilder sbOccupation = new StringBuilder();
                    StringBuilder sbSleep = new StringBuilder();
                    StringBuilder sbExercise = new StringBuilder();
                    StringBuilder sbHousing = new StringBuilder();
                    StringBuilder sbCaffeine = new StringBuilder();
                    StringBuilder sbtravel = new StringBuilder();
                    dtr.SocialHxId = SocialHxId;
                    dtr.StatusId = TabbaccoJSON["SmokingStatus"];
                    dtr.TypeId = TabbaccoJSON["TobaccoType"];
                    dtr.UsagePeriodId = TabbaccoJSON["TobaccoUsagePeriod"];
                    dtr.FrequencyId = TabbaccoJSON["TobaccoFrequencyDaily"];
                    dtr.CounsellingPeriodId= TabbaccoJSON["TobaccoCounsellingPeriod"];
                    dtr.CounsellingTopicId = TabbaccoJSON["TobaccoCounsellingTopic"];
                    dtr.CessationLength = TabbaccoJSON["TobaccoCessationLength"];
                    dtr.CessationPeriodId = TabbaccoJSON["TobaccoCessationPeriod"];
                    dtr.bRecentlyQuit = MDVUtility.ToStr( TabbaccoJSON["TobaccoRecentlyQuit"]);
                    dtr.bWouldQuit = MDVUtility.ToStr( TabbaccoJSON["TobaccoWouldQuit"]);
                    dtr.Comments = TabbaccoJSON["TobaccoComments"];
                    dtr.CreatedBy = TabbaccoJSON["CreatedbyTabbacco"];
                    dtr.CreatedOn = MDVUtility.ToDateTime(TabbaccoJSON["CreatedonTabbacco"]);
                    dtr.ModifiedBy = TabbaccoJSON["ModifiedbyTabbacco"];
                    dtr.ModifiedOn = MDVUtility.ToDateTime(TabbaccoJSON["ModifiedonTabbacco"]);
                    //foreach (var item in dsHxNative.SocialHx_Tobacco)
                    //{
                    sbTabbacco.Append("<div id='socialTobacco_" + TabbaccoJSON["SmokingStatus"] + "' title='Tobacco'  name='Social Hx'><strong>Tobacco: </strong>");
                    sbTabbacco.Append((string.IsNullOrEmpty(TabbaccoJSON["SmokingStatus"]) ? "" : TabbaccoJSON["SmokingStatus_text"]) + ", " + IsNullReturnSoapValue(TabbaccoJSON["TobaccoType_text"], TabbaccoJSON["TobaccoType"]) + (string.IsNullOrEmpty(TabbaccoJSON["TobaccoFrequencyDaily"]) ? "" : TabbaccoJSON["TobaccoFrequencyDaily_text"] == "- Select -" ? "" : TabbaccoJSON["TobaccoFrequencyDaily_text"]) + " ");
                    sbTabbacco.Append((string.IsNullOrEmpty(TabbaccoJSON["TobaccoUsagePeriod"]) ? "" : TabbaccoJSON["TobaccoUsagePeriod_text"]));
                    sbTabbacco.Append((string.IsNullOrEmpty(TabbaccoJSON["TobaccoCounsellingPeriod"]) ? "" : TabbaccoJSON["TobaccoCounsellingPeriod_text"]) + ", ");
                    sbTabbacco.Append((string.IsNullOrEmpty(TabbaccoJSON["TobaccoCounsellingTopic"]) ? "" : TabbaccoJSON["TobaccoCounsellingTopic_text"]) + ", ");
                    sbTabbacco.Append((string.IsNullOrEmpty(TabbaccoJSON["TobaccoCessationLength"]) ? "" : TabbaccoJSON["TobaccoCessationLength"]) + ", ");
                    sbTabbacco.Append((string.IsNullOrEmpty(TabbaccoJSON["TobaccoCessationPeriod"]) ? "" : TabbaccoJSON["TobaccoCessationPeriod_text"]) + ", ");
                    if (TabbaccoJSON["TobaccoRecentlyQuit"] == true)
                    {
                        sbTabbacco.Append("Patient Recently Quit " + ",");

                    }
                    if (TabbaccoJSON["TobaccoWouldQuit"] == true)
                    {
                        sbTabbacco.Append("Patient Recently Quit " + ",");

                    }
                    sbTabbacco.Append("Comments: " + TabbaccoJSON["TobaccoComments"]);
                    sbTabbacco.Append("</div>");
                   // sbTabbacco.Append((string.IsNullOrEmpty(TabbaccoJSON["TobaccoCounsellingPeriod"]) ? "" : TabbaccoJSON["TobaccoCounsellingPeriod_text"]));
                   // sbTabbacco.Append((string.IsNullOrEmpty(TabbaccoJSON["TobaccoCounsellingTopic"]) ? "" : TabbaccoJSON["TobaccoCounsellingTopic_text"]));
                   // sbTabbacco.Append((string.IsNullOrEmpty(TabbaccoJSON["TobaccoCessationLength"]) ? "" : TabbaccoJSON["TobaccoCessationLength_text"]));
                   // sbTabbacco.Append((string.IsNullOrEmpty(TabbaccoJSON["TobaccoCessationPeriod"]) ? "" : TabbaccoJSON["TobaccoCessationPeriod_text"]) + "</div>");
                    
                   
                    dtr.SoapText = MDVUtility.ToStr(sbTabbacco);

                    dar.SocialHxId = SocialHxId;
                    dar.StatusId = AlcoholJSON["AlcoholStatus"];
                    dar.TypeId = AlcoholJSON["AlcoholType"];
                    dar.UsagePeriodId = AlcoholJSON["AlcoholUsagePeriod"];
                    dar.FrequencyId = AlcoholJSON["AlcoholFrequencyDays"];
                    dar.CounsellingPeriodId = AlcoholJSON["AlcoholCounsellingPeriod"];
                    dar.CounsellingTopicId = AlcoholJSON["AlcoholCounsellingTopic"];
                    dar.CessationLength = AlcoholJSON["AlcoholCessationLength"];
                    dar.CessationPeriodId = AlcoholJSON["AlcoholCessationPeriod"];
                    dar.bRecentlyQuit = MDVUtility.ToStr( AlcoholJSON["AlcoholRecentlyQuit"]);
                    dar.bWouldQuit = MDVUtility.ToStr( AlcoholJSON["AlcoholWouldQuit"]);
                    dar.bNotReadyToQuit = MDVUtility.ToStr (AlcoholJSON["AlcoholNotReadyToQuit"]);
                    dar.Comments = AlcoholJSON["AlcoholComments"];
                    dar.CreatedBy = AlcoholJSON["CreatedbyAlcohol"]; 
                    dar.CreatedOn = MDVUtility.ToDateTime(AlcoholJSON["CreatedonAlcohol"]);
                    dar.ModifiedBy = AlcoholJSON["ModifiedbyAlcohol"];
                    dar.ModifiedOn = MDVUtility.ToDateTime(AlcoholJSON["ModifiedonAlcohol"]);

                    sbAlcohol.Append("<div id='socialAlcohol_" + /*item.AlcoholId + */"' title='Alcohol'  name='Social Hx'><strong>Alcohol: </strong>");
                    sbAlcohol.Append(IsNullReturnSoapValue(AlcoholJSON["AlcoholStatus_text"], AlcoholJSON["AlcoholStatus"]) + IsNullReturnSoapValue(AlcoholJSON["AlcoholType_text"], AlcoholJSON["AlcoholType"]) + (string.IsNullOrEmpty(AlcoholJSON["AlcoholFrequencyDays"]) ? "" : AlcoholJSON["AlcoholFrequencyDays_text"]) + (string.IsNullOrEmpty(AlcoholJSON["AlcoholUsagePeriod"]) ? "" : " for " + AlcoholJSON["AlcoholUsagePeriod_text"]) + IsNullReturnSoapValue(AlcoholJSON["AlcoholCounsellingPeriod_text"], AlcoholJSON["AlcoholCounsellingPeriod"]) + IsNullReturnSoapValue(AlcoholJSON["AlcoholCounsellingTopic_text"], AlcoholJSON["AlcoholCounsellingTopic"]) + IsNullReturnSoapValue(AlcoholJSON["AlcoholCessationLength"], AlcoholJSON["AlcoholCessationLength"]) + IsNullReturnSoapValue(AlcoholJSON["AlcoholCessationPeriod_text"], AlcoholJSON["AlcoholCessationPeriod"])  );

                    sbAlcohol.Append((string.IsNullOrEmpty(AlcoholJSON["AlcoholCounsellingPeriod"]) ? "" : AlcoholJSON["AlcoholCounsellingPeriod_text"]) + ", ");
                    sbAlcohol.Append((string.IsNullOrEmpty(AlcoholJSON["AlcoholCounsellingTopic"]) ? "" : AlcoholJSON["AlcoholCounsellingTopic_text"]) + ", ");
                    sbAlcohol.Append((string.IsNullOrEmpty(AlcoholJSON["AlcoholCessationLength"]) ? "" : AlcoholJSON["AlcoholCessationLength"]) + ", ");
                    sbAlcohol.Append((string.IsNullOrEmpty(AlcoholJSON["AlcoholCessationPeriod"]) ? "" : AlcoholJSON["AlcoholCessationPeriod_text"]) + ", ");
                    if (AlcoholJSON["AlcoholRecentlyQuit"]== true)
                    {
                        sbAlcohol.Append("Patient Recently Quit "+",");

                    }
                    if (AlcoholJSON["AlcoholWouldQuit"] == true)
                    {
                        sbAlcohol.Append("Patient Would Quit " + ",");

                    }
                    if (AlcoholJSON["AlcoholNotReadyToQuit"] == true)
                    {
                        sbAlcohol.Append("Patient Not Ready To Quit " + ",");

                    }
                    sbAlcohol.Append(IsNullReturnSoapValue(AlcoholJSON["AlcoholComments"], AlcoholJSON["AlcoholComments"]) + "</div>");
                    //sbAlcohol.Append((item.IsNull("CessationLength") ? "" : " Patient has quit " + item.CessationLength + " ago, "));
                    //sbAlcohol.Append((item.bWouldQuit ? " Patient would quit, " : "") + (item.bRecentlyQuit ? " Patient recently quit, " : "") + (item.bNotReadyToQuit ? " Patient not ready to quit, " : ""));
                    //sbAlcohol.Append(" Counselling: " + item.CounsellingPeriod + " For " + item.CounsellingTopic + (string.IsNullOrEmpty(item.Comments) ? "." : ", Comments: " + item.Comments) + "</div>");

                    dar.SoapText = MDVUtility.ToStr(sbAlcohol);

                    ddr.SocialHxId = SocialHxId;
                    ddr.StatusId = DrugAbuseJSON["DrugStatus"];
                    ddr.DrugId = DrugAbuseJSON["DrugType"];
                    ddr.RouteId = DrugAbuseJSON["DrugRoute"];
                    ddr.FrequencyDailyId = DrugAbuseJSON["DrugFrequencyDay"];
                    ddr.UsagePeriodId = DrugAbuseJSON["DrugUsagePeriod"];
                    ddr.CessationLength = DrugAbuseJSON["DrugCessationLength"];
                    ddr.CessationPeriodId = DrugAbuseJSON["DrugCessationPeriod"];
                    ddr.bRecentlyQuit =MDVUtility.ToStr(DrugAbuseJSON["DrugRecentlyQuit"]);
                    ddr.bWouldQuit = MDVUtility.ToStr( DrugAbuseJSON["DrugWouldQuit"]);
                    ddr.Comments = DrugAbuseJSON["DrugComments"];
                    ddr.CreatedBy = DrugAbuseJSON["CreatedbyDrugAbuse"];
                    ddr.CreatedOn = MDVUtility.ToDateTime(DrugAbuseJSON["CreatedonDrugAbuse"]);
                    ddr.ModifiedBy = DrugAbuseJSON["ModifiedbyDrugAbuse"];
                    ddr.ModifiedOn = MDVUtility.ToDateTime(DrugAbuseJSON["ModifiedonDrugAbuse"]);

                    sbDrugAbuse.Append("<div id='socialDrugAbuse_" + /*item.DrugAbuseId + */"' title='Drug Abuse'  name='Social Hx'><strong>Drug Abuse: </strong>");
                    sbDrugAbuse.Append(IsNullReturnSoapValue(DrugAbuseJSON["DrugStatus_text"], DrugAbuseJSON["DrugStatus"])
                        +/* " of " + IsNullReturnSoapValue(SocialHxId.ToString(), SocialHxId.ToString()) + */ 
                        IsNullReturnSoapValue(" by " + DrugAbuseJSON["DrugType_text"], DrugAbuseJSON["DrugType"])
                        + IsNullReturnSoapValue(" for " + DrugAbuseJSON["DrugFrequencyDay_text"], DrugAbuseJSON["DrugFrequencyDay"]) 
                        + " " + DrugAbuseJSON["DrugUsagePeriod"] + ", ");
                    sbDrugAbuse.Append(DrugAbuseJSON["DrugCessationLength"]+",");
                    sbDrugAbuse.Append(string.IsNullOrEmpty(DrugAbuseJSON["DrugCessationPeriod"])?"": DrugAbuseJSON["DrugCessationPeriod_text"]);
                    //sbDrugAbuse.Append(IsNullReturnSoapValue(DrugAbuseJSON["DrugCessationLength"], DrugAbuseJSON["DrugCessationLength"])
                    //  + sbDrugAbuse.Append(IsNullReturnSoapValue(DrugAbuseJSON["DrugCessationPeriod_text"], DrugAbuseJSON["DrugCessationPeriod"])));
                    if (DrugAbuseJSON["DrugWouldQuit"] == true)
                    {
                        sbDrugAbuse.Append("Patient Would Quit " + ",");

                    }
                    if (DrugAbuseJSON["DrugRecentlyQuit"] == true)
                    {
                        sbDrugAbuse.Append("Patient Recently Quit " + ",");

                    }
                    sbDrugAbuse.Append("Rout: " + IsNullReturnSoapValue(DrugAbuseJSON["DrugRoute_text"], DrugAbuseJSON["DrugRoute"]) + ",");
                    sbDrugAbuse.Append("Comments: "+DrugAbuseJSON["DrugComments"]);
                    sbDrugAbuse.Append("</div>");//sbDrugAbuse.Append((item.IsNull("CessationLength") ? "" : " Patient has quit " + item.CessationLength + " ago, "));
                    //sbDrugAbuse.Append((item.bWouldQuit ? " Patient would quit" : "") + (item.bRecentlyQuit ? " Patient recently quit" : "") + (string.IsNullOrEmpty(item.Comments) ? "." : " Comments: " + item.Comments) + "</div>");

                    ddr.SoapText = MDVUtility.ToStr(sbDrugAbuse);

                    dsr.SocialHxId = SocialHxId;
                    dsr.StatusId = SexualJSON["SexualStatus"];
                    dsr.PreferenceId = SexualJSON["SexualPreferences"];
                    dsr.bUSingProtection =SexualJSON["SexualbUsingProtection"] == "" ?"": SexualJSON["SexualbUsingProtection"];
                    dsr.ProtectionMethodId = SexualJSON["SexualProtectionMethod"];
                    dsr.ProtectionPeriodId = SexualJSON["SexualProtectionPeriod"];
                    dsr.bPainWithIntercourse = MDVUtility.ToStr(SexualJSON["RadSexualPainWithIntercourse"]);
                    dsr.ComplaintId = SexualJSON["SexualComplaints"];
                    dsr.bExposedToSTD = SexualJSON["SexualExposedToSTD"];
                    dsr.bPregnancyStatus = SexualJSON["RadSexualYesPregnant"] == true ? "1" : "0"; ;
                    dsr.STDIds= SexualJSON["SexualSTD"]; 
                    dsr.bSexuallyAbused = SexualJSON["RadSexualYesAbusedSexually"]==true?"1":"0";
                    dsr.LMP = SexualJSON["SexualLMP"];
                    dsr.CreatedBy = SexualJSON["CreatedbySexual"];
                    dsr.CreatedOn = MDVUtility.ToDateTime(SexualJSON["CreatedonSexual"]);
                    dsr.ModifiedBy = SexualJSON["ModifiedbySexual"];
                    dsr.ModifiedOn = MDVUtility.ToDateTime(SexualJSON["ModifiedonSexual"]);
                    dsr.Comments = MDVUtility.ToStr(SexualJSON["SocialHxComments"]);
                    dsr.PregnancyDuration = MDVUtility.ToStr(SexualJSON["PregnancyDuration"]);
                    sbSexual.Append("<div id='socialSexualHx_" + /*modelObj.SexualHxId + */"' title='Sexual Hx'  name='Social Hx'><strong>Sexual Hx: </strong>");
                    sbSexual.Append((string.IsNullOrEmpty(SexualJSON["SexualStatus"]) ? "" : SexualJSON["SexualStatus_text"].TrimEnd()) + 
                        IsNullReturnSoapValue(", Prefers " + SexualJSON["SexualPreferences_text"], SexualJSON["SexualPreferences"])
                        /* + (string.IsNullOrEmpty(SexualJSON["SexualbUsingProtection"]) ? "" : "Using Protection: " + (SexualJSON["SexualbUsingProtection"].ToLower() + " "))*/);
                    sbSexual.Append(string.IsNullOrEmpty(SexualJSON["SexualProtectionMethod"])?"": "Using Protection: Yes, Method: "  + SexualJSON["SexualProtectionMethod_text"] 
                        + IsNullReturnSoapValue("How often: " + SexualJSON["SexualProtectionPeriod_text"], SexualJSON["SexualProtectionPeriod"]) 
                        + IsNullReturnSoapValue("Patient has complaints of " + SexualJSON["SexualComplaints_text"], SexualJSON["SexualComplaints"])
                        + (string.IsNullOrEmpty(SexualJSON["SexualExposedToSTD"]) ? "" : " Exposed to STD: " + (SexualJSON["SexualExposedToSTD"].ToLower() == "1" ? "Yes, " : "No, ")));
                    sbSexual.Append((string.IsNullOrEmpty(SexualJSON["SexualSTD"])?"":"STD: "+ SexualJSON["SexualSTD_text"] +" ,")+ (string.IsNullOrEmpty(MDVUtility.ToStr(SexualJSON["RadSexualPainWithIntercourse"])) ? "" : " Experiences pain with intercourse: " + (SexualJSON["RadSexualPainWithIntercourse"] == true ? "Yes" : "No") + (string.IsNullOrEmpty(MDVUtility.ToStr(SexualJSON["RadSexualYesAbusedSexually"])) ? "" : ", Abused sexually: " + (SexualJSON["RadSexualYesAbusedSexually"] == true ? "Yes" : "No"))) + "</div>");//+ (string.IsNullOrEmpty(modelObj.SexualComments) ? "" : ", Comments: " + modelObj.SexualComments) + "</div>");
                    sbSexual.Append("Pregnant: " + (SexualJSON["RadSexualYesPregnant"] == true ? "Yes" : "No" +","));
                    if (!string.IsNullOrEmpty(SexualJSON["SexualLMP"]))
                    { sbSexual.Append("Last Menstrual Period is: " + MDVUtility.GetDateDDMMYYY(SexualJSON["SexualLMP"]) +",");
                    }
                    if (!string.IsNullOrEmpty(SexualJSON["PregnancyDuration"]))
                    { sbSexual.Append("Pregnancy Duration :" + SexualJSON["PregnancyDuration"] + ","); }
                    sbSexual.Append("Comments : " + SexualJSON["SocialHxComments"] );
                    dsr.SoapText = MDVUtility.ToStr(sbSexual);

                    dmor.SocialHxId = SocialHxId;
                    dmor.StatusId = OccupationJSON["MiscChildStatus"];
                    dmor.Present =MDVUtility.ToStr(OccupationJSON["OccupationPresent"]);
                    dmor.Past =MDVUtility.ToStr(OccupationJSON["OccupationPast"]);
                    dmor.Comments = OccupationJSON["OccupationComments"];
                    dmor.StartDate = OccupationJSON["OccupationHxStartDate"];
                    dmor.EndDate = OccupationJSON["OccupationHxEndDate"];
                    dmor.OccupationDetail = OccupationJSON["OccupationDetail"];
                    dmor.CreatedBy = OccupationJSON["CreatedbyOccupation"];
                    dmor.CreatedOn = MDVUtility.ToDateTime(OccupationJSON["CreatedonOccupation"]);
                    dmor.ModifiedBy = OccupationJSON["ModifiedbyOccupation"];
                    dmor.ModifiedOn = MDVUtility.ToDateTime(OccupationJSON["ModifiedonOccupation"]);

                    sbOccupation.Append("<div id='socialMiscHxOccupation_" /*+ modelObj.OccupationHxId */+ "' title='Occupation Hx'  name='Social Hx'><strong>Occupation: </strong>");
                    if (OccupationJSON["MiscChildStatus_text"].Trim() == "Unemployed" || OccupationJSON["MiscChildStatus_text"].Trim() == "Office worker" || OccupationJSON["MiscChildStatus_text"].Trim() == "Professional" || OccupationJSON["MiscChildStatus_text"].Trim() == "Manual worker")
                    {
                        sbOccupation.Append((string.IsNullOrEmpty(OccupationJSON["MiscChildStatus_text"].Trim()) ? "" : "Patient is " + OccupationJSON["MiscChildStatus_text"].Trim()));
                    }
                    else
                    {
                        sbOccupation.Append((string.IsNullOrEmpty(OccupationJSON["MiscChildStatus_text"].Trim()) ? "" : "Patient " + OccupationJSON["MiscChildStatus_text"].Trim()));
                    }
                    if( OccupationJSON["OccupationPresent"]==false)
                    {
                        sbOccupation.Append("Present ,");
                    }
                    if (OccupationJSON["OccupationPast"] == true)
                    {
                        sbOccupation.Append("Past ,");
                    }
                    // sbOccupation.Append((string.IsNullOrEmpty(OccupationJSON["OccupationPresent"].ToString().Trim()) ? "" : ", Present: " + OccupationJSON["OccupationPresent"].ToString().Trim()));
                   // sbOccupation.Append((string.IsNullOrEmpty(OccupationJSON["OccupationPast"].ToString().Trim()) ? "" : ", Past: " + OccupationJSON["OccupationPast"].ToString().Trim()));
                    sbOccupation.Append(", Start Date:" + OccupationJSON["OccupationHxStartDate"] + ", ");
                    sbOccupation.Append("End Date:" + OccupationJSON["OccupationHxEndDate"] + ", ");
                    sbOccupation.Append("Detail:" + OccupationJSON["OccupationDetail"] + ", ");
                    sbOccupation.Append((string.IsNullOrEmpty(OccupationJSON["OccupationComments"]) ? "." : "  Comments: " + OccupationJSON["OccupationComments"].Trim()));
                    sbOccupation.Append("<br></div>");

                    dmor.SoapText = MDVUtility.ToStr(sbOccupation);

                    dmsr.SocialHxId = SocialHxId;
                    dmsr.StatusId = SleepJSON["MiscChildStatus"];
                    dmsr.SleepHours = SleepJSON["SleepHours"];
                    dmsr.Comments = SleepJSON["SleepComments"];
                    dmsr.CreatedBy = SleepJSON["CreatedbySleep"];
                    dmsr.CreatedOn = MDVUtility.ToDateTime(SleepJSON["CreatedonSleep"]);
                    dmsr.ModifiedBy = SleepJSON["ModifiedbySleep"];
                    dmsr.ModifiedOn = MDVUtility.ToDateTime(SleepJSON["ModifiedonSleep"]);

                    sbSleep.Append("<div id='sleepHx{0}' title='Sleep Hx'  name='Sleep Hx'>"/*, modelObj.SleepHxId*/);
                    sbSleep.Append("<strong>Sleep:</strong>");
                    if (!string.IsNullOrEmpty(SleepJSON["MiscChildStatus"]))
                    {
                        if (SleepJSON["MiscChildStatus"] != "6")
                        {
                            sbSleep.Append(" Patient sleeps ");
                        }
                        sbSleep.Append(" " + SleepJSON["MiscChildStatus_text"].Trim() + ". ");
                    }
                    sbSleep.Append((string.IsNullOrEmpty(SleepJSON["SleepHours"]) ? "" : "Sleeping hours are " + SleepJSON["SleepHours"].Trim() + ". "));
                    sbSleep.Append(string.IsNullOrEmpty(SleepJSON["SleepComments"]) ? "" : "Comments: " + SleepJSON["SleepComments"].Trim());
                    sbSleep.Append("<br/>");
                    sbSleep.Append("</div>");

                    ReplaceSoapTextBy(sbSleep);

                    dmsr.SoapText = MDVUtility.ToStr(sbSleep);

                    dmer.SocialHxId = SocialHxId;
                    dmer.StatusId = ExerciseJSON["MiscChildStatus"];
                    dmer.TypeId = ExerciseJSON["ExercisesType"];
                    dmer.DietId = ExerciseJSON["ExercisesDiet"];
                    dmer.Comments = ExerciseJSON["ExercisesComments"];
                    dmer.CreatedBy = ExerciseJSON["CreatedbyExercise"];
                    dmer.CreatedOn = MDVUtility.ToDateTime(ExerciseJSON["CreatedonExercise"]);
                    dmer.ModifiedBy = ExerciseJSON["ModifiedbyExercise"];
                    dmer.ModifiedOn = MDVUtility.ToDateTime(ExerciseJSON["ModifiedonExercise"]);
                    sbExercise.Append("<div id='exercisesHx{0}' title='Exercises Hx'  name='Exercises Hx'>"/*, modelObj.ExercisesHxId*/);
                    sbExercise.Append("<strong>Exercises:</strong> Patient ");
                    if (ExerciseJSON["MiscChildStatus"] != "6")
                        sbExercise.Append("exercises ");
                    sbExercise.Append((string.IsNullOrEmpty(ExerciseJSON["MiscChildStatus"].Trim()) ? "" : ExerciseJSON["MiscChildStatus_text"].Trim()));
                    sbExercise.Append((string.IsNullOrEmpty(ExerciseJSON["ExercisesType"].Trim()) ? "." : " with " + ExerciseJSON["ExercisesType_text"].Trim() + " intensity."));
                    sbExercise.Append((string.IsNullOrEmpty(ExerciseJSON["ExercisesDiet"].Trim()) ? "" : " Patient diet is " + ExerciseJSON["ExercisesDiet_text"].Trim() + "."));
                    sbExercise.Append(string.IsNullOrEmpty(ExerciseJSON["ExercisesComments"]) ? "" : " Comments: " + ExerciseJSON["ExercisesComments"].Trim());
                    sbExercise.Append("<br/>");
                    sbExercise.Append("</div>");
                    ReplaceSoapTextBy(sbExercise);
                    dmer.SoapText = MDVUtility.ToStr(sbExercise);

                    dmhr.SocialHxId = SocialHxId;
                    dmhr.StatusId = HousingJSON["MiscChildStatus"];
                    dmhr.Present = HousingJSON["HousingPresent"];
                    dmhr.Past = HousingJSON["HousingPast"];
                    dmhr.Comments = HousingJSON["HousingComments"];
                    dmhr.CreatedBy = HousingJSON["CreatedByHousing"];
                    dmhr.CreatedOn = MDVUtility.ToDateTime(HousingJSON["CreatedonHousing"]);
                    dmhr.ModifiedBy = HousingJSON["ModifiedbyHousing"];
                    dmhr.ModifiedOn = MDVUtility.ToDateTime(HousingJSON["ModifiedOnHousing"]);
                    sbHousing.Append("<div id='housingHx{0}' title='Housing Hx'  name='Housing Hx'>"/*, modelObj.HousingHxId*/);
                    sbHousing.Append("<strong>Housing:</strong>" + (string.IsNullOrEmpty(HousingJSON["MiscChildStatus"].Trim()) ? "" : " Housing Status: " + HousingJSON["MiscChildStatus_text"].Trim()));
                    sbHousing.Append((string.IsNullOrEmpty(HousingJSON["HousingPresent"].Trim()) ? "" : ", Present: " + HousingJSON["HousingPresent"].Trim()));
                    sbHousing.Append((string.IsNullOrEmpty(HousingJSON["HousingPast"].Trim()) ? "" : ", Past: " + HousingJSON["HousingPast"].Trim()));
                    sbHousing.Append(string.IsNullOrEmpty(HousingJSON["HousingComments"]) ? "." : ", Comments: " + HousingJSON["HousingComments"].Trim());
                    sbHousing.Append("<br/>");
                    sbHousing.Append("</div>");
                    ReplaceSoapTextBy(sbHousing);
                    dmhr.SoapText = MDVUtility.ToStr(sbHousing);

                    dmcr.SocialHxId = SocialHxId;
                    dmcr.StatusId = CaffeineJSON["MiscChildStatus"];
                    dmcr.FrequencyId = CaffeineJSON["CaffeineIntakFrequency"];
                    if (MDVUtility.ToStr(CaffeineJSON["RadCaffieneharmful"]).ToLower() == "false" && MDVUtility.ToStr(CaffeineJSON["RadCaffieneharmfulNo"]).ToLower() == "false")
                    {
                        dmcr.IsHarmful = null;
                    }
                    else
                    {
                        dmcr.IsHarmful = MDVUtility.ToStr(CaffeineJSON["RadCaffieneharmful"]);
                    }
                    dmcr.Comments = CaffeineJSON["CaffeineComments"];
                    dmcr.CreatedBy = CaffeineJSON["CreatedByCaffeine"];
                    dmcr.CreatedOn = MDVUtility.ToDateTime(CaffeineJSON["CreatedonCaffeine"]);
                    dmcr.ModifiedBy = CaffeineJSON["ModifiedbyCaffeine"];
                    dmcr.ModifiedOn = MDVUtility.ToDateTime(CaffeineJSON["ModifiedOnCaffeine"]);

                    sbCaffeine.Append("<div id='socialMiscHxCaffeineIntake_" /*+ modelObj.CaffeineIntakeHxId */+ "' title='Caffeine Intake'  name='Social Hx'><strong>Caffeine intake: </strong> ");
                    if (CaffeineJSON["MiscChildStatus"].Trim() == "Does not use")
                    {
                        sbCaffeine.Append(" Patient does not intake caffeine.");
                    }
                    else
                    {
                        string statusText = CaffeineJSON["MiscChildStatus_text"].Trim();
                        if (string.Equals(statusText, "Abnormal"))
                        {
                            statusText = "Abnormally";
                        }
                        sbCaffeine.Append(" Patients intakes caffeine " + statusText);
                    }
                    sbCaffeine.Append((string.IsNullOrEmpty(CaffeineJSON["CaffeineIntakFrequency"].Trim()) ? "" : ", Frequency: " + CaffeineJSON["CaffeineIntakFrequency_text"].Trim()));
                    sbCaffeine.Append((string.IsNullOrEmpty(CaffeineJSON["CaffeineComments"].Trim()) ? "" : ", Comments: " + CaffeineJSON["CaffeineComments"].Trim() + "<br/>"));
                    ReplaceSoapTextBy(sbCaffeine);

                    dmcr.SoapText = MDVUtility.ToStr(sbCaffeine);

                    dmT.StatusId = TravelJSON["TravelChildStatus"];
                    dmT.FromDate = TravelJSON["TravelHxFromDate"];
                    dmT.ToDate = TravelJSON["TravelHxToDate"];
                    dmT.Location = TravelJSON["TravelHxLocation"];
                    dmT.Comments = TravelJSON["TravelHxComments"];
                    dmT.CreatedBy = TravelJSON["CreatedByTravel"];
                    dmT.CreatedOn = TravelJSON["CreatedonTravel"];
                    dmT.ModifiedBy = TravelJSON["ModifiedbyTravel"];
                    dmT.ModifiedOn = TravelJSON["ModifiedOnTravel"];

                    sbtravel.Append("<div id='socialTravel_" +"' title='Travel'  name='Social Hx'><strong>Travel: </strong>");
                    sbtravel.Append(IsNullReturnSoapValue(TravelJSON["TravelChildStatus_text"], TravelJSON["TravelChildStatus"]));

                    sbtravel.Append((string.IsNullOrEmpty(TravelJSON["TravelHxFromDate"]) ? "" : TravelJSON["TravelHxFromDate"]) + ", ");
                    sbtravel.Append((string.IsNullOrEmpty(TravelJSON["TravelHxToDate"]) ? "" : TravelJSON["TravelHxToDate"]) + ", ");
                    sbtravel.Append((string.IsNullOrEmpty(TravelJSON["TravelHxLocation"]) ? "" : TravelJSON["TravelHxLocation"]) + ", ");
                    sbtravel.Append((string.IsNullOrEmpty(TravelJSON["TravelHxComments"]) ? "" : TravelJSON["TravelHxComments"]) + ", ");
                    sbtravel.Append("</div>");

                    dmT.SoapText = sbtravel.ToString();
                    dsHxNative.SocialHx_Tobacco.AddSocialHx_TobaccoRow(dtr);
                    dsHxNative.SocialHx_Alcohol.AddSocialHx_AlcoholRow(dar);
                    dsHxNative.SocialHx_DrugAbuse.AddSocialHx_DrugAbuseRow(ddr);
                    dsHxNative.SocialHx_SexualHx.AddSocialHx_SexualHxRow(dsr);
                    dsHxNative.SocialHx_MiscHx_OccupationHx.AddSocialHx_MiscHx_OccupationHxRow(dmor);
                    dsHxNative.SocialHx_MiscHx_SleepHx.AddSocialHx_MiscHx_SleepHxRow(dmsr);
                    dsHxNative.SocialHx_MiscHx_ExercisesHx.AddSocialHx_MiscHx_ExercisesHxRow(dmer);
                    dsHxNative.SocialHx_MiscHx_HousingHx.AddSocialHx_MiscHx_HousingHxRow(dmhr);
                    dsHxNative.SocialHx_MiscHx_CaffeineIntakHx.AddSocialHx_MiscHx_CaffeineIntakHxRow(dmcr);
                    dsHxNative.SocialHx_MiscHx_TravelHx.AddSocialHx_MiscHx_TravelHxRow(dmT);
                    ////BLObject<DSProfile> objPractice = BLLAdminProfileObj.InsertPractice(ref dsProfile);
                    BLObject<DSHxMobileApp> obj = BLLClinicalObj.InsertSocialHxComponentsNative(dsHxNative, model.PatientId, model.IsSynced);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            //CPTPlanInfoId = dsCodes.Tables[dsCodes.CPTPlan.TableName].Rows[0][dsCodes.CPTPlan.CPTPlanIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            //status = false,
                            //Message = obj.Message
                            status = false,
                            Message = Common.AppPrivileges.Save_Error_Message,
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.No_Record_Message,
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                // /*
                //    Change Implement BY: Muhammad Azhar Shahzad
                //    Reason: To update Soap Text of Social Hx in Insert mode
                //    Created Date: Dec 15, 2015
                //*/
                // BLObject<string> objValue = BLLClinicalObj.updateSoapTextForSocialHX(SocialHxId.ToString());

                // var SoapText = string.Empty;
                // var IsCreatedOrModified = string.Empty;
                // var LastUpdated = string.Empty;
                // var SoapInfo = getCurrentSoapText(MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]));
                // if (SoapInfo != null)
                // {
                //     SoapText = SoapInfo["SoapText"];
                //     IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                //     LastUpdated = SoapInfo["LastUpdated"];
                // }
                // var response = new
                // {
                //     status = true,
                //     message = Common.AppPrivileges.Save_Message,
                //     SocialHxId = MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]),
                //     SoapText = SoapText,
                //     IsCreatedOrModified = IsCreatedOrModified,
                //     LastUpdated = LastUpdated
                // };
                //return "";//(Newtonsoft.Json.JsonConvert.SerializeObject(response));

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

        public Dictionary<string, string> getCurrentSoapText(Int64 SocialHxId)
        {
            try
            {
                DSHistorySummary dsHistorySummarySoap = null;
                BLObject<DSHistorySummary> objSummary;
                objSummary = BLLClinicalObj.loadHxLog(SocialHxId, "SocialHx", "Current", 1, 10);
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
                   {"SocialHxId" ,MDVUtility.ToStr( SocialHxId)}
                };
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region insertUpdateTobacco

        // Author:  Muhammad Arshad
        // Created Date: 04/12/2015
        //OverView: This function will handle insert/update of Tobacco for current SocialHx on basis of SocialHxId
        private string insertUpdateTobacco(long SocialHxId, List<object> lstTobaccoObjects, string PatientId)
        {
            #region Tobacco
            DSSocialHistory dsTobacco = new DSSocialHistory();
            List<SocialHxTobaccoModel> lstTobaccoModel = lstTobaccoObjects.OfType<SocialHxTobaccoModel>().ToList();
            bool isFirstChild = false;
            foreach (SocialHxTobaccoModel CurrentModel in lstTobaccoModel)
            {
                if (CurrentModel.TobaccoId != null)
                {
                    Int32 currentTobaccoId = MDVUtility.ToInt32(CurrentModel.TobaccoId);
                    currentTobaccoId = currentTobaccoId == 0 ? -1 : currentTobaccoId;
                    BLObject<DSSocialHistory> objTobacco = BLLClinicalObj.LoadTobaccoHistory(currentTobaccoId, SocialHxId, "1", "");
                    dsTobacco = objTobacco.Data;
                    DSSocialHistory.SocialHx_TobaccoRow RowTobacco = null;
                    if (dsTobacco.SocialHx_Tobacco.Rows.Count > 0)
                    {
                        RowTobacco = (DSSocialHistory.SocialHx_TobaccoRow)dsTobacco.SocialHx_Tobacco.Rows[0];
                    }
                    else
                    {
                        RowTobacco = dsTobacco.SocialHx_Tobacco.NewSocialHx_TobaccoRow();
                    }

                    if (RowTobacco != null)
                    {
                        bool isValueDifferent = false;
                        bool istoUpdateRow = false;
                        if (dsTobacco.SocialHx_Tobacco.Rows.Count < 1)
                        {
                            RowTobacco.TobaccoId = currentTobaccoId;
                        }
                        RowTobacco.SocialHxId = SocialHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.SmokingStatus))
                        {
                            RowTobacco.StatusId = MDVUtility.ToInt16(CurrentModel.SmokingStatus);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.StatusIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoType))
                        {
                            RowTobacco.TypeId = MDVUtility.ToInt16(CurrentModel.TobaccoType);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.TypeIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoUsagePeriod))
                        {
                            RowTobacco.UsagePeriodId = MDVUtility.ToInt16(CurrentModel.TobaccoUsagePeriod);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.UsagePeriodIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoFrequencyDaily))
                        {
                            RowTobacco.FrequencyId = MDVUtility.ToInt16(CurrentModel.TobaccoFrequencyDaily);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.FrequencyIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoCounsellingPeriod))
                        {
                            RowTobacco.CounsellingPeriodId = MDVUtility.ToInt16(CurrentModel.TobaccoCounsellingPeriod);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.CounsellingPeriodIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoCounsellingTopic))
                        {
                            RowTobacco.CounsellingTopicId = MDVUtility.ToInt16(CurrentModel.TobaccoCounsellingTopic);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.CounsellingTopicIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoCessationLength))
                        {
                            RowTobacco.CessationLength = MDVUtility.ToDouble(CurrentModel.TobaccoCessationLength);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.CessationLengthColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoCessationPeriod))
                        {
                            RowTobacco.CessationPeriodId = MDVUtility.ToInt16(CurrentModel.TobaccoCessationPeriod);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.CessationPeriodIdColumn] = DBNull.Value;
                        }

                        RowTobacco.bRecentlyQuit = CurrentModel.TobaccoRecentlyQuit == true ? true : false;

                        RowTobacco.bWouldQuit = CurrentModel.TobaccoWouldQuit == true ? true : false;

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoComments))
                        {
                            RowTobacco.Comments = MDVUtility.ToStr(CurrentModel.TobaccoComments);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.CommentsColumn] = DBNull.Value;
                        }
                        RowTobacco.IsActive = true;
                        if (dsTobacco.SocialHx_Tobacco.Rows.Count == 0)
                        {
                            RowTobacco.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowTobacco.CreatedOn = DateTime.Now;
                        }

                        RowTobacco.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowTobacco.ModifiedOn = DateTime.Now;
                        RowTobacco.PatientId = MDVUtility.ToLong(PatientId);

                        // if no Tobacco is found against TobaccoId, it implies for new record
                        if (dsTobacco.SocialHx_Tobacco.Rows.Count < 1)
                        {
                            dsTobacco.SocialHx_Tobacco.AddSocialHx_TobaccoRow(RowTobacco);
                        }
                    }
                }
            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowTobacco in dsTobacco.SocialHx_Tobacco.Rows)
            {
                RowTobacco[dsTobacco.SocialHx_Tobacco.SoapTextColumn] = insertUpdateTobaccoSoapText(dsTobacco, lstTobaccoModel[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedTobacco = BLLClinicalObj.InsertUpdateTobaccoHistory(dsTobacco);
            if (objInsertedTobacco.Data != null)
            {

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedTobacco.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }

        private string insertUpdateTobacco(long SocialHxId, List<SocialHxTobaccoModel> lstTobaccoModel, string PatientId)
        {
            #region Tobacco
            DSSocialHistory dsTobacco = new DSSocialHistory();
            bool isFirstChild = false;
            foreach (SocialHxTobaccoModel CurrentModel in lstTobaccoModel)
            {
                if (CurrentModel.TobaccoId != null && CurrentModel.IsLast)
                {
                    Int32 currentTobaccoId = MDVUtility.ToInt32(CurrentModel.TobaccoId);
                    currentTobaccoId = currentTobaccoId == 0 ? -1 : currentTobaccoId;
                    BLObject<DSSocialHistory> objTobacco = BLLClinicalObj.LoadTobaccoHistory(currentTobaccoId, SocialHxId, "1", "");
                    dsTobacco = objTobacco.Data;
                    DSSocialHistory.SocialHx_TobaccoRow RowTobacco = null;
                    if (dsTobacco.SocialHx_Tobacco.Rows.Count > 0)
                    {
                        RowTobacco = (DSSocialHistory.SocialHx_TobaccoRow)dsTobacco.SocialHx_Tobacco.Rows[0];
                    }
                    else
                    {
                        RowTobacco = dsTobacco.SocialHx_Tobacco.NewSocialHx_TobaccoRow();
                    }

                    if (RowTobacco != null)
                    {
                        bool isValueDifferent = false;
                        bool istoUpdateRow = false;
                        if (dsTobacco.SocialHx_Tobacco.Rows.Count < 1)
                        {
                            RowTobacco.TobaccoId = currentTobaccoId;
                        }
                        RowTobacco.SocialHxId = SocialHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.SmokingStatus))
                        {
                            RowTobacco.StatusId = MDVUtility.ToInt16(CurrentModel.SmokingStatus);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.StatusIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoType))
                        {
                            RowTobacco.TypeId = MDVUtility.ToInt16(CurrentModel.TobaccoType);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.TypeIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoUsagePeriod))
                        {
                            RowTobacco.UsagePeriodId = MDVUtility.ToInt16(CurrentModel.TobaccoUsagePeriod);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.UsagePeriodIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoFrequencyDaily))
                        {
                            RowTobacco.FrequencyId = MDVUtility.ToInt16(CurrentModel.TobaccoFrequencyDaily);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.FrequencyIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoCounsellingPeriod))
                        {
                            RowTobacco.CounsellingPeriodId = MDVUtility.ToInt16(CurrentModel.TobaccoCounsellingPeriod);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.CounsellingPeriodIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoCounsellingTopic))
                        {
                            RowTobacco.CounsellingTopicId = MDVUtility.ToInt16(CurrentModel.TobaccoCounsellingTopic);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.CounsellingTopicIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoCessationLength))
                        {
                            RowTobacco.CessationLength = MDVUtility.ToDouble(CurrentModel.TobaccoCessationLength);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.CessationLengthColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoCessationPeriod))
                        {
                            RowTobacco.CessationPeriodId = MDVUtility.ToInt16(CurrentModel.TobaccoCessationPeriod);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.CessationPeriodIdColumn] = DBNull.Value;
                        }

                        RowTobacco.bRecentlyQuit = CurrentModel.TobaccoRecentlyQuit == true ? true : false;

                        RowTobacco.bWouldQuit = CurrentModel.TobaccoWouldQuit == true ? true : false;

                        if (!string.IsNullOrEmpty(CurrentModel.TobaccoComments))
                        {
                            RowTobacco.Comments = MDVUtility.ToStr(CurrentModel.TobaccoComments);
                        }
                        else
                        {
                            RowTobacco[dsTobacco.SocialHx_Tobacco.CommentsColumn] = DBNull.Value;
                        }
                        RowTobacco.IsActive = true;
                        if (dsTobacco.SocialHx_Tobacco.Rows.Count == 0)
                        {
                            RowTobacco.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowTobacco.CreatedOn = DateTime.Now;
                        }

                        RowTobacco.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowTobacco.ModifiedOn = DateTime.Now;
                        RowTobacco.PatientId = MDVUtility.ToLong(PatientId);
                        // if no Tobacco is found against TobaccoId, it implies for new record
                        if (dsTobacco.SocialHx_Tobacco.Rows.Count < 1)
                        {
                            dsTobacco.SocialHx_Tobacco.AddSocialHx_TobaccoRow(RowTobacco);
                        }
                    }
                }
            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowTobacco in dsTobacco.SocialHx_Tobacco.Rows)
            {
                RowTobacco[dsTobacco.SocialHx_Tobacco.SoapTextColumn] = insertUpdateTobaccoSoapText(dsTobacco, lstTobaccoModel[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedTobacco = BLLClinicalObj.InsertUpdateTobaccoHistory(dsTobacco);
            if (objInsertedTobacco.Data != null)
            {

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedTobacco.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }
        #endregion

        #region insertUpdateAlcohol

        // Author:  Muhammad Arshad
        // Created Date: 07/12/2015
        //OverView: This function will handle insert/update of Alcohol for current SocialHx on basis of SocialHxId
        private string insertUpdateAlcohol(long SocialHxId, List<object> lstAlcoholObjects, string PatientId)
        {
            #region Alcohol
            DSSocialHistory dsAlcohol = new DSSocialHistory();
            List<SocialHxAlcoholModel> lstAlcoholModel = lstAlcoholObjects.OfType<SocialHxAlcoholModel>().ToList();
            foreach (SocialHxAlcoholModel CurrentModel in lstAlcoholModel)
            {
                if (CurrentModel.AlcoholId != null)
                {
                    Int32 currentAlcoholId = MDVUtility.ToInt32(CurrentModel.AlcoholId);
                    currentAlcoholId = currentAlcoholId == 0 ? -1 : currentAlcoholId;
                    BLObject<DSSocialHistory> objAlcohol = BLLClinicalObj.LoadAlcoholHistory(currentAlcoholId, SocialHxId);
                    dsAlcohol = objAlcohol.Data;
                    DSSocialHistory.SocialHx_AlcoholRow RowAlcohol = null;
                    if (dsAlcohol.SocialHx_Alcohol.Rows.Count > 0)
                    {
                        RowAlcohol = (DSSocialHistory.SocialHx_AlcoholRow)dsAlcohol.SocialHx_Alcohol.Rows[0];
                    }
                    else
                    {
                        RowAlcohol = dsAlcohol.SocialHx_Alcohol.NewSocialHx_AlcoholRow();
                    }

                    if (RowAlcohol != null)
                    {
                        if (dsAlcohol.SocialHx_Alcohol.Rows.Count < 1)
                        {
                            RowAlcohol.AlcoholId = currentAlcoholId;
                        }
                        RowAlcohol.SocialHxId = SocialHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholStatus))
                        {
                            RowAlcohol.StatusId = MDVUtility.ToInt16(CurrentModel.AlcoholStatus);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.StatusIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholType))
                        {
                            RowAlcohol.TypeId = MDVUtility.ToInt16(CurrentModel.AlcoholType);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.TypeIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholType))
                        {
                            RowAlcohol.TypeId = MDVUtility.ToInt16(CurrentModel.AlcoholType);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.TypeIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholUsagePeriod))
                        {
                            RowAlcohol.UsagePeriodId = MDVUtility.ToInt16(CurrentModel.AlcoholUsagePeriod);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.UsagePeriodIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholFrequencyDays))
                        {
                            RowAlcohol.FrequencyId = MDVUtility.ToInt16(CurrentModel.AlcoholFrequencyDays);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.FrequencyIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholCounsellingPeriod))
                        {
                            RowAlcohol.CounsellingPeriodId = MDVUtility.ToInt16(CurrentModel.AlcoholCounsellingPeriod);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.CounsellingPeriodIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholCounsellingTopic))
                        {
                            RowAlcohol.CounsellingTopicId = MDVUtility.ToInt16(CurrentModel.AlcoholCounsellingTopic);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.CounsellingTopicIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholCessationLength))
                        {
                            RowAlcohol.CessationLength = MDVUtility.ToInt16(CurrentModel.AlcoholCessationLength);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.CessationLengthColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholCessationPeriod))
                        {
                            RowAlcohol.CessationPeriodId = MDVUtility.ToInt16(CurrentModel.AlcoholCessationPeriod);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.CessationPeriodIdColumn] = DBNull.Value;
                        }

                        RowAlcohol.bRecentlyQuit = CurrentModel.AlcoholRecentlyQuit;

                        RowAlcohol.bNotReadyToQuit = CurrentModel.AlcoholNotReadyToQuit;

                        RowAlcohol.bWouldQuit = CurrentModel.AlcoholWouldQuit;

                        //Added By Abid Ali
                        //CurrentModel.bRecentlyQuit = RowAlcohol.bRecentlyQuit;

                        //CurrentModel.bNotReadyToQuit = RowAlcohol.bNotReadyToQuit;

                        //CurrentModel.bWouldQuit = RowAlcohol.bNotReadyToQuit;

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholComments))
                        {
                            RowAlcohol.Comments = MDVUtility.ToStr(CurrentModel.AlcoholComments);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.CommentsColumn] = DBNull.Value;
                        }
                        RowAlcohol.IsActive = true;
                        if (dsAlcohol.SocialHx_Alcohol.Rows.Count < 1)
                        {
                            RowAlcohol.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowAlcohol.CreatedOn = DateTime.Now;
                        }

                        RowAlcohol.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowAlcohol.ModifiedOn = DateTime.Now;
                        RowAlcohol.PatientId = MDVUtility.ToLong(PatientId);
                        // if no Alcohol is found against AlcoholId, it implies for new record
                        if (dsAlcohol.SocialHx_Alcohol.Rows.Count < 1)
                        {
                            dsAlcohol.SocialHx_Alcohol.AddSocialHx_AlcoholRow(RowAlcohol);
                        }
                    }
                }

            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowAlcohol in dsAlcohol.SocialHx_Alcohol.Rows)
            {
                RowAlcohol[dsAlcohol.SocialHx_Alcohol.SoapTextColumn] = insertUpdateAlcoholSoapText(dsAlcohol, lstAlcoholModel[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedAlcohol = BLLClinicalObj.InsertUpdateAlcoholHistory(dsAlcohol);
            if (objInsertedAlcohol.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedAlcohol.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }
        private string insertUpdateAlcohol(long SocialHxId, List<SocialHxAlcoholModel> lstAlcoholModel, string PatientId)
        {
            #region Alcohol
            DSSocialHistory dsAlcohol = new DSSocialHistory();

            foreach (SocialHxAlcoholModel CurrentModel in lstAlcoholModel)
            {
                if (CurrentModel.AlcoholId != null && CurrentModel.IsLast)
                {
                    Int32 currentAlcoholId = MDVUtility.ToInt32(CurrentModel.AlcoholId);
                    currentAlcoholId = currentAlcoholId == 0 ? -1 : currentAlcoholId;
                    BLObject<DSSocialHistory> objAlcohol = BLLClinicalObj.LoadAlcoholHistory(currentAlcoholId, SocialHxId);
                    dsAlcohol = objAlcohol.Data;
                    DSSocialHistory.SocialHx_AlcoholRow RowAlcohol = null;
                    if (dsAlcohol.SocialHx_Alcohol.Rows.Count > 0)
                    {
                        RowAlcohol = (DSSocialHistory.SocialHx_AlcoholRow)dsAlcohol.SocialHx_Alcohol.Rows[0];
                    }
                    else
                    {
                        RowAlcohol = dsAlcohol.SocialHx_Alcohol.NewSocialHx_AlcoholRow();
                    }

                    if (RowAlcohol != null)
                    {
                        if (dsAlcohol.SocialHx_Alcohol.Rows.Count < 1)
                        {
                            RowAlcohol.AlcoholId = currentAlcoholId;
                        }
                        RowAlcohol.SocialHxId = SocialHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholStatus))
                        {
                            RowAlcohol.StatusId = MDVUtility.ToInt16(CurrentModel.AlcoholStatus);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.StatusIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholType))
                        {
                            RowAlcohol.TypeId = MDVUtility.ToInt16(CurrentModel.AlcoholType);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.TypeIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholType))
                        {
                            RowAlcohol.TypeId = MDVUtility.ToInt16(CurrentModel.AlcoholType);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.TypeIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholUsagePeriod))
                        {
                            RowAlcohol.UsagePeriodId = MDVUtility.ToInt16(CurrentModel.AlcoholUsagePeriod);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.UsagePeriodIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholFrequencyDays))
                        {
                            RowAlcohol.FrequencyId = MDVUtility.ToInt16(CurrentModel.AlcoholFrequencyDays);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.FrequencyIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholCounsellingPeriod))
                        {
                            RowAlcohol.CounsellingPeriodId = MDVUtility.ToInt16(CurrentModel.AlcoholCounsellingPeriod);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.CounsellingPeriodIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholCounsellingTopic))
                        {
                            RowAlcohol.CounsellingTopicId = MDVUtility.ToInt16(CurrentModel.AlcoholCounsellingTopic);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.CounsellingTopicIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholCessationLength))
                        {
                            RowAlcohol.CessationLength = MDVUtility.ToInt16(CurrentModel.AlcoholCessationLength);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.CessationLengthColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholCessationPeriod))
                        {
                            RowAlcohol.CessationPeriodId = MDVUtility.ToInt16(CurrentModel.AlcoholCessationPeriod);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.CessationPeriodIdColumn] = DBNull.Value;
                        }

                        RowAlcohol.bRecentlyQuit = CurrentModel.AlcoholRecentlyQuit;

                        RowAlcohol.bNotReadyToQuit = CurrentModel.AlcoholNotReadyToQuit;

                        RowAlcohol.bWouldQuit = CurrentModel.AlcoholWouldQuit;

                        //Added By Abid Ali
                        //CurrentModel.bRecentlyQuit = RowAlcohol.bRecentlyQuit;

                        //CurrentModel.bNotReadyToQuit = RowAlcohol.bNotReadyToQuit;

                        //CurrentModel.bWouldQuit = RowAlcohol.bNotReadyToQuit;

                        if (!string.IsNullOrEmpty(CurrentModel.AlcoholComments))
                        {
                            RowAlcohol.Comments = MDVUtility.ToStr(CurrentModel.AlcoholComments);
                        }
                        else
                        {
                            RowAlcohol[dsAlcohol.SocialHx_Alcohol.CommentsColumn] = DBNull.Value;
                        }
                        RowAlcohol.IsActive = true;
                        if (dsAlcohol.SocialHx_Alcohol.Rows.Count < 1)
                        {
                            RowAlcohol.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowAlcohol.CreatedOn = DateTime.Now;
                        }

                        RowAlcohol.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowAlcohol.ModifiedOn = DateTime.Now;
                        RowAlcohol.PatientId = MDVUtility.ToLong(PatientId);

                        // if no Alcohol is found against AlcoholId, it implies for new record
                        if (dsAlcohol.SocialHx_Alcohol.Rows.Count < 1)
                        {
                            dsAlcohol.SocialHx_Alcohol.AddSocialHx_AlcoholRow(RowAlcohol);
                        }
                    }
                }

            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowAlcohol in dsAlcohol.SocialHx_Alcohol.Rows)
            {
                RowAlcohol[dsAlcohol.SocialHx_Alcohol.SoapTextColumn] = insertUpdateAlcoholSoapText(dsAlcohol, lstAlcoholModel[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedAlcohol = BLLClinicalObj.InsertUpdateAlcoholHistory(dsAlcohol);
            if (objInsertedAlcohol.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedAlcohol.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }
        #endregion

        #region insertUpdateDrugAbuse

        // Author:  Muhammad Arshad
        // Created Date: 07/12/2015
        //OverView: This function will handle insert/update of DrugAbuse for current SocialHx on basis of SocialHxId
        private string insertUpdateDrugAbuse(long SocialHxId, List<object> lstDrugAbuseObjects, string PatientId)
        {
            #region DrugAbuse
            DSSocialHistory dsDrugAbuse = new DSSocialHistory();
            List<SocialHxDrugAbuseModel> lstDrugAbuseModel = lstDrugAbuseObjects.OfType<SocialHxDrugAbuseModel>().ToList();
            foreach (SocialHxDrugAbuseModel CurrentModel in lstDrugAbuseModel)
            {
                if (CurrentModel.DrugAbuseId != null)
                {
                    Int32 currentDrugAbuseId = MDVUtility.ToInt32(CurrentModel.DrugAbuseId);
                    currentDrugAbuseId = currentDrugAbuseId == 0 ? -1 : currentDrugAbuseId;
                    BLObject<DSSocialHistory> objDrugAbuse = BLLClinicalObj.LoadDrugHistory(currentDrugAbuseId, SocialHxId);
                    dsDrugAbuse = objDrugAbuse.Data;
                    DSSocialHistory.SocialHx_DrugAbuseRow RowDrugAbuse = null;
                    if (dsDrugAbuse.SocialHx_DrugAbuse.Rows.Count > 0)
                    {
                        RowDrugAbuse = (DSSocialHistory.SocialHx_DrugAbuseRow)dsDrugAbuse.SocialHx_DrugAbuse.Rows[0];
                    }
                    else
                    {
                        RowDrugAbuse = dsDrugAbuse.SocialHx_DrugAbuse.NewSocialHx_DrugAbuseRow();
                    }

                    if (RowDrugAbuse != null)
                    {
                        if (dsDrugAbuse.SocialHx_DrugAbuse.Rows.Count < 1)
                        {
                            RowDrugAbuse.DrugAbuseId = currentDrugAbuseId;
                        }
                        RowDrugAbuse.SocialHxId = SocialHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.DrugStatus))
                        {
                            RowDrugAbuse.StatusId = MDVUtility.ToInt16(CurrentModel.DrugStatus);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.StatusIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.DrugType))
                        {
                            RowDrugAbuse.DrugId = MDVUtility.ToStr(CurrentModel.DrugType);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.DrugIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.DrugRoute))
                        {
                            RowDrugAbuse.RouteId = MDVUtility.ToInt16(CurrentModel.DrugRoute);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.RouteIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.DrugFrequencyDay))
                        {
                            RowDrugAbuse.FrequencyDailyId = MDVUtility.ToInt16(CurrentModel.DrugFrequencyDay);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.FrequencyDailyIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.DrugFrequencyMonth))
                        {
                            RowDrugAbuse.FrequencyMonthlyId = MDVUtility.ToInt16(CurrentModel.DrugFrequencyMonth);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.FrequencyMonthlyIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.DrugUsagePeriod))
                        {
                            RowDrugAbuse.UsagePeriodId = MDVUtility.ToInt16(CurrentModel.DrugUsagePeriod);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.UsagePeriodIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.DrugCessationLength))
                        {
                            RowDrugAbuse.CessationLength = MDVUtility.ToInt16(CurrentModel.DrugCessationLength);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.CessationLengthColumn] = DBNull.Value;
                        }
                        //Start//14/12/2015//Ahmad Raza//DrugCessationPeriod Data not saving issue fixed
                        if (!string.IsNullOrEmpty(CurrentModel.DrugCessationPeriod))
                        {
                            RowDrugAbuse.CessationPeriodId = MDVUtility.ToInt16(CurrentModel.DrugCessationPeriod);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.CessationPeriodIdColumn] = DBNull.Value;
                        }
                        //End//14/12/2015//Ahmad Raza//DrugCessationPeriod Data not saving issue fixed
                        RowDrugAbuse.bRecentlyQuit = CurrentModel.DrugRecentlyQuit;

                        RowDrugAbuse.bWouldQuit = CurrentModel.DrugWouldQuit;

                        if (!string.IsNullOrEmpty(CurrentModel.DrugComments))
                        {
                            RowDrugAbuse.Comments = MDVUtility.ToStr(CurrentModel.DrugComments);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.CommentsColumn] = DBNull.Value;
                        }
                        RowDrugAbuse.IsActive = true;
                        if (dsDrugAbuse.SocialHx_DrugAbuse.Rows.Count < 1)
                        {
                            RowDrugAbuse.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowDrugAbuse.CreatedOn = DateTime.Now;
                        }

                        RowDrugAbuse.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowDrugAbuse.ModifiedOn = DateTime.Now;
                        RowDrugAbuse.PatientId = MDVUtility.ToLong(PatientId);
                        // if no DrugAbuse is found against DrugAbuseId, it implies for new record
                        if (dsDrugAbuse.SocialHx_DrugAbuse.Rows.Count < 1)
                        {
                            dsDrugAbuse.SocialHx_DrugAbuse.AddSocialHx_DrugAbuseRow(RowDrugAbuse);
                        }
                    }
                }

            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowDrugAbuse in dsDrugAbuse.SocialHx_DrugAbuse.Rows)
            {
                RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.SoapTextColumn] = insertUpdateDrugAbuseSoapText(dsDrugAbuse, lstDrugAbuseModel[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedDrugAbuse = BLLClinicalObj.InsertUpdateDrugHistory(dsDrugAbuse);
            if (objInsertedDrugAbuse.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedDrugAbuse.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }
        private string insertUpdateDrugAbuse(long SocialHxId, List<SocialHxDrugAbuseModel> lstDrugAbuseModel, string PatientId)
        {
            #region DrugAbuse
            DSSocialHistory dsDrugAbuse = new DSSocialHistory();

            foreach (SocialHxDrugAbuseModel CurrentModel in lstDrugAbuseModel)
            {
                if (CurrentModel.DrugAbuseId != null && CurrentModel.IsLast)
                {
                    Int32 currentDrugAbuseId = MDVUtility.ToInt32(CurrentModel.DrugAbuseId);
                    currentDrugAbuseId = currentDrugAbuseId == 0 ? -1 : currentDrugAbuseId;
                    BLObject<DSSocialHistory> objDrugAbuse = BLLClinicalObj.LoadDrugHistory(currentDrugAbuseId, SocialHxId);
                    dsDrugAbuse = objDrugAbuse.Data;
                    DSSocialHistory.SocialHx_DrugAbuseRow RowDrugAbuse = null;
                    if (dsDrugAbuse.SocialHx_DrugAbuse.Rows.Count > 0)
                    {
                        RowDrugAbuse = (DSSocialHistory.SocialHx_DrugAbuseRow)dsDrugAbuse.SocialHx_DrugAbuse.Rows[0];
                    }
                    else
                    {
                        RowDrugAbuse = dsDrugAbuse.SocialHx_DrugAbuse.NewSocialHx_DrugAbuseRow();
                    }

                    if (RowDrugAbuse != null)
                    {
                        if (dsDrugAbuse.SocialHx_DrugAbuse.Rows.Count < 1)
                        {
                            RowDrugAbuse.DrugAbuseId = currentDrugAbuseId;
                        }
                        RowDrugAbuse.SocialHxId = SocialHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.DrugStatus))
                        {
                            RowDrugAbuse.StatusId = MDVUtility.ToInt16(CurrentModel.DrugStatus);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.StatusIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.DrugType))
                        {
                            RowDrugAbuse.DrugId = MDVUtility.ToStr(CurrentModel.DrugType);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.DrugIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.DrugRoute))
                        {
                            RowDrugAbuse.RouteId = MDVUtility.ToInt16(CurrentModel.DrugRoute);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.RouteIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.DrugFrequencyDay))
                        {
                            RowDrugAbuse.FrequencyDailyId = MDVUtility.ToInt16(CurrentModel.DrugFrequencyDay);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.FrequencyDailyIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.DrugFrequencyMonth))
                        {
                            RowDrugAbuse.FrequencyMonthlyId = MDVUtility.ToInt16(CurrentModel.DrugFrequencyMonth);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.FrequencyMonthlyIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.DrugUsagePeriod))
                        {
                            RowDrugAbuse.UsagePeriodId = MDVUtility.ToInt16(CurrentModel.DrugUsagePeriod);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.UsagePeriodIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.DrugCessationLength))
                        {
                            RowDrugAbuse.CessationLength = MDVUtility.ToInt16(CurrentModel.DrugCessationLength);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.CessationLengthColumn] = DBNull.Value;
                        }
                        //Start//14/12/2015//Ahmad Raza//DrugCessationPeriod Data not saving issue fixed
                        if (!string.IsNullOrEmpty(CurrentModel.DrugCessationPeriod))
                        {
                            RowDrugAbuse.CessationPeriodId = MDVUtility.ToInt16(CurrentModel.DrugCessationPeriod);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.CessationPeriodIdColumn] = DBNull.Value;
                        }
                        //End//14/12/2015//Ahmad Raza//DrugCessationPeriod Data not saving issue fixed
                        RowDrugAbuse.bRecentlyQuit = CurrentModel.DrugRecentlyQuit;

                        RowDrugAbuse.bWouldQuit = CurrentModel.DrugWouldQuit;

                        if (!string.IsNullOrEmpty(CurrentModel.DrugComments))
                        {
                            RowDrugAbuse.Comments = MDVUtility.ToStr(CurrentModel.DrugComments);
                        }
                        else
                        {
                            RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.CommentsColumn] = DBNull.Value;
                        }
                        RowDrugAbuse.IsActive = true;
                        if (dsDrugAbuse.SocialHx_DrugAbuse.Rows.Count < 1)
                        {
                            RowDrugAbuse.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowDrugAbuse.CreatedOn = DateTime.Now;
                        }

                        RowDrugAbuse.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowDrugAbuse.ModifiedOn = DateTime.Now;
                        RowDrugAbuse.PatientId = MDVUtility.ToLong(PatientId);

                        // if no DrugAbuse is found against DrugAbuseId, it implies for new record
                        if (dsDrugAbuse.SocialHx_DrugAbuse.Rows.Count < 1)
                        {
                            dsDrugAbuse.SocialHx_DrugAbuse.AddSocialHx_DrugAbuseRow(RowDrugAbuse);
                        }
                    }
                }

            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowDrugAbuse in dsDrugAbuse.SocialHx_DrugAbuse.Rows)
            {
                RowDrugAbuse[dsDrugAbuse.SocialHx_DrugAbuse.SoapTextColumn] = insertUpdateDrugAbuseSoapText(dsDrugAbuse, lstDrugAbuseModel[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedDrugAbuse = BLLClinicalObj.InsertUpdateDrugHistory(dsDrugAbuse);
            if (objInsertedDrugAbuse.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedDrugAbuse.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }
        #endregion

        #region insertUpdateSexualHx

        // Author:  Muhammad Arshad
        // Created Date: 07/12/2015
        //OverView: This function will handle insert/update of SexualHx for current SocialHx on basis of SocialHxId
        private string insertUpdateSexualHx(long SocialHxId, List<object> lstSexualHxObjects, string SocialComments, string PatientId)
        {
            #region SexualHx
            DSSocialHistory dsSexualHx = new DSSocialHistory();
            List<SocialHxSexualHxModel> lstSexualHxModel = lstSexualHxObjects.OfType<SocialHxSexualHxModel>().ToList();
            foreach (SocialHxSexualHxModel CurrentModel in lstSexualHxModel)
            {
                if (CurrentModel.SexualHxId != null)
                {
                    Int32 currentSexualHxId = MDVUtility.ToInt32(CurrentModel.SexualHxId);
                    currentSexualHxId = currentSexualHxId == 0 ? -1 : currentSexualHxId;
                    BLObject<DSSocialHistory> objSexualHx = BLLClinicalObj.LoadSexualHistory(currentSexualHxId, SocialHxId);
                    dsSexualHx = objSexualHx.Data;
                    DSSocialHistory.SocialHx_SexualHxRow RowSexualHx = null;
                    if (dsSexualHx.SocialHx_SexualHx.Rows.Count > 0)
                    {
                        RowSexualHx = (DSSocialHistory.SocialHx_SexualHxRow)dsSexualHx.SocialHx_SexualHx.Rows[0];
                    }
                    else
                    {
                        RowSexualHx = dsSexualHx.SocialHx_SexualHx.NewSocialHx_SexualHxRow();
                    }

                    if (RowSexualHx != null)
                    {
                        if (dsSexualHx.SocialHx_SexualHx.Rows.Count < 1)
                        {
                            RowSexualHx.SexualHxId = currentSexualHxId;
                        }
                        RowSexualHx.SocialHxId = SocialHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.SexualStatus))
                        {
                            RowSexualHx.StatusId = MDVUtility.ToInt16(CurrentModel.SexualStatus);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.StatusIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualPreferences))
                        {
                            RowSexualHx.PreferenceId = MDVUtility.ToInt16(CurrentModel.SexualPreferences);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.PreferenceIdColumn] = DBNull.Value;
                        }





                        if (!string.IsNullOrEmpty(CurrentModel.SexualbUsingProtection))
                        {
                            RowSexualHx.bUSingProtection = CurrentModel.SexualbUsingProtection.ToLower() == "yes" ? true : false;
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.bUSingProtectionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualProtectionMethod))
                        {
                            RowSexualHx.ProtectionMethodId = MDVUtility.ToInt16(CurrentModel.SexualProtectionMethod);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.ProtectionMethodIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualProtectionPeriod))
                        {
                            RowSexualHx.ProtectionPeriodId = MDVUtility.ToInt16(CurrentModel.SexualProtectionPeriod);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.ProtectionPeriodIdColumn] = DBNull.Value;
                        }

                        // RadSexualPainWithIntercourse=true means RadNo is checked
                        if (!string.IsNullOrEmpty(CurrentModel.RadSexualPainWithIntercourse))
                        {
                            RowSexualHx.bPainWithIntercourse = CurrentModel.RadSexualPainWithIntercourse.ToLower() == "true" ? true : false;
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.bPainWithIntercourseColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.RadSexualPregnant))
                        {
                            RowSexualHx.bPregnancyStatus = CurrentModel.RadSexualPregnant.ToLower() == "true" ? true : false;
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.bPregnancyStatusColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.SexualHxPregnancyDuration))
                        {
                            RowSexualHx.PregnancyDuration = MDVUtility.ToStr(CurrentModel.SexualHxPregnancyDuration);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.PregnancyDurationColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.SexualComplaints))
                        {
                            RowSexualHx.ComplaintId = MDVUtility.ToInt16(CurrentModel.SexualComplaints);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.ComplaintIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualExposedToSTD))
                        {
                            RowSexualHx.bExposedToSTD = CurrentModel.SexualExposedToSTD.ToLower() == "yes" ? true : false;
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.bExposedToSTDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualSTD))
                        {
                            RowSexualHx.STDIds = MDVUtility.ToStr(CurrentModel.SexualSTD);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.STDIdsColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.RadSexualAbusedSexually))
                        {
                            RowSexualHx.bSexuallyAbused = CurrentModel.RadSexualAbusedSexually.ToLower() == "true" ? true : false;
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.bSexuallyAbusedColumn] = DBNull.Value;
                        }

                        // RadSexualAbusedSexually=true means RadNo is checked


                        if (!string.IsNullOrEmpty(CurrentModel.SexualComments))
                        {
                            RowSexualHx.Comments = MDVUtility.ToStr(CurrentModel.SexualComments);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.CommentsColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualLMP))
                        {
                            RowSexualHx.LMP = MDVUtility.ToDateTime(CurrentModel.SexualLMP);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.LMPColumn] = DBNull.Value;
                        }
                        RowSexualHx.IsActive = true;
                        if (dsSexualHx.SocialHx_SexualHx.Rows.Count < 1)
                        {
                            RowSexualHx.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowSexualHx.CreatedOn = DateTime.Now;
                        }

                        RowSexualHx.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowSexualHx.ModifiedOn = DateTime.Now;
                        RowSexualHx.PatientId = MDVUtility.ToLong(PatientId);
                        RowSexualHx[dsSexualHx.SocialHx_SexualHx.SoapTextColumn] = insertUpdateSexualHxSoapText(dsSexualHx, CurrentModel, SocialComments);

                        // if no SexualHx is found against SexualHxId, it implies for new record
                        if (dsSexualHx.SocialHx_SexualHx.Rows.Count < 1)
                        {
                            dsSexualHx.SocialHx_SexualHx.AddSocialHx_SexualHxRow(RowSexualHx);
                        }

                        try
                        {
                            CurrentModel.bPainWithIntercourse = (bool)RowSexualHx[dsSexualHx.SocialHx_SexualHx.bPainWithIntercourseColumn];
                            CurrentModel.bSexuallyAbused = (bool)RowSexualHx[dsSexualHx.SocialHx_SexualHx.bSexuallyAbusedColumn];
                            CurrentModel.bUSingProtection = (bool)RowSexualHx[dsSexualHx.SocialHx_SexualHx.bUSingProtectionColumn];
                            CurrentModel.bExposedToSTD = (bool)RowSexualHx[dsSexualHx.SocialHx_SexualHx.bExposedToSTDColumn];
                        }
                        catch (Exception ex) { }


                    }
                }

            }
            //int counter = 0;
            //// Azhar Added this code on dec 14 2015 on 4pm for Soap Text


            //foreach (DataRow RowSexualHx in dsSexualHx.SocialHx_SexualHx.Rows)
            //{

            //    RowSexualHx[dsSexualHx.SocialHx_SexualHx.SoapTextColumn] = insertUpdateSexualHxSoapText(dsSexualHx, lstSexualHxModel[counter], SocialComments);
            //    counter++;
            //}
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedDrugAbuse = BLLClinicalObj.InsertUpdateSexualHistory(dsSexualHx);
            if (objInsertedDrugAbuse.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedDrugAbuse.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }
        private string insertUpdateSexualHx(long SocialHxId, List<SocialHxSexualHxModel> lstSexualHxModel, string SocialComments, string PatientId)
        {
            #region SexualHx
            DSSocialHistory dsSexualHx = new DSSocialHistory();

            foreach (SocialHxSexualHxModel CurrentModel in lstSexualHxModel)
            {
                if (CurrentModel.SexualHxId != null && CurrentModel.IsLast)
                {
                    Int32 currentSexualHxId = MDVUtility.ToInt32(CurrentModel.SexualHxId);
                    currentSexualHxId = currentSexualHxId == 0 ? -1 : currentSexualHxId;
                    BLObject<DSSocialHistory> objSexualHx = BLLClinicalObj.LoadSexualHistory(currentSexualHxId, SocialHxId);
                    dsSexualHx = objSexualHx.Data;
                    DSSocialHistory.SocialHx_SexualHxRow RowSexualHx = null;
                    if (dsSexualHx.SocialHx_SexualHx.Rows.Count > 0)
                    {
                        RowSexualHx = (DSSocialHistory.SocialHx_SexualHxRow)dsSexualHx.SocialHx_SexualHx.Rows[0];
                    }
                    else
                    {
                        RowSexualHx = dsSexualHx.SocialHx_SexualHx.NewSocialHx_SexualHxRow();
                    }

                    if (RowSexualHx != null)
                    {
                        if (dsSexualHx.SocialHx_SexualHx.Rows.Count < 1)
                        {
                            RowSexualHx.SexualHxId = currentSexualHxId;
                        }
                        RowSexualHx.SocialHxId = SocialHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.SexualStatus))
                        {
                            RowSexualHx.StatusId = MDVUtility.ToInt16(CurrentModel.SexualStatus);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.StatusIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualPreferences))
                        {
                            RowSexualHx.PreferenceId = MDVUtility.ToInt16(CurrentModel.SexualPreferences);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.PreferenceIdColumn] = DBNull.Value;
                        }



                        if (!string.IsNullOrEmpty(CurrentModel.RadSexualPregnant))
                        {
                            RowSexualHx.bPregnancyStatus = CurrentModel.RadSexualPregnant.ToLower() == "true" ? true : false;
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.bPregnancyStatusColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.SexualHxPregnancyDuration))
                        {
                            RowSexualHx.PregnancyDuration = MDVUtility.ToStr(CurrentModel.SexualHxPregnancyDuration);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.PregnancyDurationColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualbUsingProtection))
                        {
                            RowSexualHx.bUSingProtection = CurrentModel.SexualbUsingProtection.ToLower() == "yes" ? true : false;
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.bUSingProtectionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualProtectionMethod))
                        {
                            RowSexualHx.ProtectionMethodId = MDVUtility.ToInt16(CurrentModel.SexualProtectionMethod);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.ProtectionMethodIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualProtectionPeriod))
                        {
                            RowSexualHx.ProtectionPeriodId = MDVUtility.ToInt16(CurrentModel.SexualProtectionPeriod);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.ProtectionPeriodIdColumn] = DBNull.Value;
                        }

                        // RadSexualPainWithIntercourse=true means RadNo is checked
                        if (!string.IsNullOrEmpty(CurrentModel.RadSexualPainWithIntercourse))
                        {
                            RowSexualHx.bPainWithIntercourse = CurrentModel.RadSexualPainWithIntercourse.ToLower() == "true" ? true : false;
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.bPainWithIntercourseColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.SexualComplaints))
                        {
                            RowSexualHx.ComplaintId = MDVUtility.ToInt16(CurrentModel.SexualComplaints);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.ComplaintIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualExposedToSTD))
                        {
                            RowSexualHx.bExposedToSTD = CurrentModel.SexualExposedToSTD.ToLower() == "yes" ? true : false;
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.bExposedToSTDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualSTD))
                        {
                            RowSexualHx.STDIds = MDVUtility.ToStr(CurrentModel.SexualSTD);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.STDIdsColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.RadSexualAbusedSexually))
                        {
                            RowSexualHx.bSexuallyAbused = CurrentModel.RadSexualAbusedSexually.ToLower() == "true" ? true : false;
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.bSexuallyAbusedColumn] = DBNull.Value;
                        }

                        // RadSexualAbusedSexually=true means RadNo is checked


                        if (!string.IsNullOrEmpty(CurrentModel.SexualComments))
                        {
                            RowSexualHx.Comments = MDVUtility.ToStr(CurrentModel.SexualComments);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.CommentsColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SexualLMP))
                        {
                            RowSexualHx.LMP = MDVUtility.ToDateTime(CurrentModel.SexualLMP);
                        }
                        else
                        {
                            RowSexualHx[dsSexualHx.SocialHx_SexualHx.LMPColumn] = DBNull.Value;
                        }
                        RowSexualHx.IsActive = true;
                        if (dsSexualHx.SocialHx_SexualHx.Rows.Count < 1)
                        {
                            RowSexualHx.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowSexualHx.CreatedOn = DateTime.Now;
                        }

                        RowSexualHx.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowSexualHx.ModifiedOn = DateTime.Now;
                        RowSexualHx.PatientId = MDVUtility.ToLong(PatientId);
                        RowSexualHx[dsSexualHx.SocialHx_SexualHx.SoapTextColumn] = insertUpdateSexualHxSoapText(dsSexualHx, CurrentModel, SocialComments);

                        // if no SexualHx is found against SexualHxId, it implies for new record
                        if (dsSexualHx.SocialHx_SexualHx.Rows.Count < 1)
                        {
                            dsSexualHx.SocialHx_SexualHx.AddSocialHx_SexualHxRow(RowSexualHx);
                        }

                        try
                        {
                            CurrentModel.bPainWithIntercourse = (bool)RowSexualHx[dsSexualHx.SocialHx_SexualHx.bPainWithIntercourseColumn];
                            CurrentModel.bSexuallyAbused = (bool)RowSexualHx[dsSexualHx.SocialHx_SexualHx.bSexuallyAbusedColumn];
                            CurrentModel.bUSingProtection = (bool)RowSexualHx[dsSexualHx.SocialHx_SexualHx.bUSingProtectionColumn];
                            CurrentModel.bExposedToSTD = (bool)RowSexualHx[dsSexualHx.SocialHx_SexualHx.bExposedToSTDColumn];
                        }
                        catch (Exception ex) { }


                    }
                }

            }
            //int counter = 0;
            //// Azhar Added this code on dec 14 2015 on 4pm for Soap Text


            //foreach (DataRow RowSexualHx in dsSexualHx.SocialHx_SexualHx.Rows)
            //{

            //    RowSexualHx[dsSexualHx.SocialHx_SexualHx.SoapTextColumn] = insertUpdateSexualHxSoapText(dsSexualHx, lstSexualHxModel[counter], SocialComments);
            //    counter++;
            //}
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedDrugAbuse = BLLClinicalObj.InsertUpdateSexualHistory(dsSexualHx);
            if (objInsertedDrugAbuse.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedDrugAbuse.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }
        #endregion

        #region insertUpdateSocialHxMiscHx

        // Author:  Muhammad Arshad
        // Created Date: 06/01/2016
        //OverView: This function will handle insert/update of MiscHx for current SocialHx on basis of SocialHxId
        private string insertUpdateMiscHx(long SocialHxId)
        {
            #region SexualHx
            DSSocialHistory dsMiscHx = new DSSocialHistory();

            if (SocialHxId > 0)
            {
                BLObject<DSSocialHistory> objMiscHx = BLLClinicalObj.LoadMiscHx(0, SocialHxId);
                dsMiscHx = objMiscHx.Data;
                DSSocialHistory.SocialHx_MiscHxRow RowMiscHx = null;
                if (dsMiscHx != null && dsMiscHx.SocialHx_MiscHx.Rows.Count > 0)
                {
                    RowMiscHx = (DSSocialHistory.SocialHx_MiscHxRow)dsMiscHx.SocialHx_MiscHx.Rows[0];
                }
                else
                {
                    RowMiscHx = dsMiscHx.SocialHx_MiscHx.NewSocialHx_MiscHxRow();
                }

                if (RowMiscHx != null)
                {
                    if (dsMiscHx.SocialHx_MiscHx.Rows.Count < 1)
                    {
                        RowMiscHx.MiscHxId = -1;
                    }
                    RowMiscHx.SocialHxId = SocialHxId;

                    if (dsMiscHx.SocialHx_MiscHx.Rows.Count < 1)
                    {
                        RowMiscHx.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowMiscHx.CreatedOn = DateTime.Now;
                    }

                    RowMiscHx.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowMiscHx.ModifiedOn = DateTime.Now;

                    // if no MiscHx is found against MiscHxId, it implies for new record
                    if (dsMiscHx.SocialHx_MiscHx.Rows.Count < 1)
                    {
                        dsMiscHx.SocialHx_MiscHx.AddSocialHx_MiscHxRow(RowMiscHx);
                    }
                }
            }
            //// Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            //foreach (DataRow RowSexualHx in dsMiscHx.SocialHx_MiscHx.Rows)
            //{
            //    RowSexualHx[dsSexualHx.SocialHx_SexualHx.SoapTextColumn] = insertUpdateSexualHxSoapText(dsSexualHx, lstSexualHxModel[counter], SocialComments);
            //    counter++;
            //}
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedMiscHx = BLLClinicalObj.InsertUpdateMiscHx(dsMiscHx);
            if (objInsertedMiscHx.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    MiscHxId = dsMiscHx.Tables[dsMiscHx.SocialHx_MiscHx.TableName].Rows[0][dsMiscHx.SocialHx_MiscHx.MiscHxIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedMiscHx.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }

        #endregion

        //Start 13/01/2016 Muhammad Arshad Save/Update MiscHx_Component
        #region insertUpdateMiscHx_Component
        private string insertUpdateMiscHx_Component(string fromComponentId, string toComponentId)
        {

            DSSocialHistory dsMiscHxComponent = new DSSocialHistory();
            BLObject<DSSocialHistory> obj;
            obj = BLLClinicalObj.loadMiscHx_Component(0);
            dsMiscHxComponent = obj.Data;
            if (obj.Data != null)
            {
                DSSocialHistory.SocialHx_MiscHx_ComponentRow[] arrFromComponentRows = (DSSocialHistory.SocialHx_MiscHx_ComponentRow[])dsMiscHxComponent.SocialHx_MiscHx_Component.Select(MDVUtility.ToStr(dsMiscHxComponent.SocialHx_MiscHx_Component.ComponentIdColumn.ColumnName) + "=" + MDVUtility.ToStr(fromComponentId));
                DSSocialHistory.SocialHx_MiscHx_ComponentRow[] arrToComponentRows = (DSSocialHistory.SocialHx_MiscHx_ComponentRow[])dsMiscHxComponent.SocialHx_MiscHx_Component.Select(MDVUtility.ToStr(dsMiscHxComponent.SocialHx_MiscHx_Component.ComponentIdColumn.ColumnName) + "=" + MDVUtility.ToStr(toComponentId));
                Int32 fromComponentOrder = 0;
                Int32 toComponentOrder = 0;
                if (arrToComponentRows.Length > 0)
                {
                    fromComponentOrder = arrFromComponentRows[0].ComponentOrder;
                }

                if (arrToComponentRows.Length > 0)
                {
                    toComponentOrder = arrToComponentRows[0].ComponentOrder;
                }

                if (fromComponentOrder > -1 && toComponentOrder > -1)
                {
                    foreach (DataRow drCurrentComponent in dsMiscHxComponent.SocialHx_MiscHx_Component.Rows)
                    {
                        string currentComponentId = MDVUtility.ToStr(drCurrentComponent[dsMiscHxComponent.SocialHx_MiscHx_Component.ComponentIdColumn.ColumnName]);
                        if (currentComponentId == fromComponentId)
                        {
                            drCurrentComponent[dsMiscHxComponent.SocialHx_MiscHx_Component.ComponentOrderColumn.ColumnName] = toComponentOrder;
                        }
                        if (currentComponentId == toComponentId)
                        {
                            drCurrentComponent[dsMiscHxComponent.SocialHx_MiscHx_Component.ComponentOrderColumn.ColumnName] = fromComponentOrder;
                        }
                    }
                }
                #region Database Insertion/Updation

                obj = BLLClinicalObj.insertUpdateMiscHx_Component(dsMiscHxComponent);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        //FaceSheetCount = dsFaceSheet.Tables[dsFaceSheet.FaceSheet.TableName].Rows.Count,
                        //FaceSheetLoad_JSON = MDVUtility.JSON_DataTable(dsFaceSheet.Tables[dsFaceSheet.FaceSheet.TableName]),
                        //ProblemListHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsFaceSheet.Tables[dsFaceSheet.FaceSheet.TableName]),                        
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ComponentsCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                #endregion

            }
            else
            {
                var response = new
                {
                    status = true,
                    ComponentsCount = 0,
                    Message = obj.Message
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion
        //End 13/01/2016 Muhammad Arshad Save/Update MiscHx_Component

        //Start 07/01/2016 Syed Zia Code to  Save/Update MiscHx sub tabs
        #region insertOccupationHx
        private string insertUpdateMiscHxOccupationHx(long MiscHxId, List<object> lstOccupationObjects, string patientId)
        {

            DSSocialHistory dsOccupation = new DSSocialHistory();
            List<SocialHxMiscHxOccupationModel> lstOccupationModel = lstOccupationObjects.OfType<SocialHxMiscHxOccupationModel>().ToList();
            //  bool isFirstChild = false;
            foreach (SocialHxMiscHxOccupationModel CurrentModel in lstOccupationModel)
            {
                if (CurrentModel.OccupationHxId != null)
                {
                    Int32 currentOccupationId = MDVUtility.ToInt32(CurrentModel.OccupationHxId);
                    currentOccupationId = currentOccupationId == 0 ? -1 : currentOccupationId;
                    BLObject<DSSocialHistory> OccupationObj = BLLClinicalObj.loadMiscHxOccupationHx(Convert.ToInt64(CurrentModel.OccupationHxId), MiscHxId, "", "");
                    dsOccupation = OccupationObj.Data;
                    DSSocialHistory.SocialHx_MiscHx_OccupationHxRow RowOccupation = null;
                    if (dsOccupation.SocialHx_MiscHx_OccupationHx.Rows.Count > 0)
                    {
                        RowOccupation = (DSSocialHistory.SocialHx_MiscHx_OccupationHxRow)dsOccupation.SocialHx_MiscHx_OccupationHx.Rows[0];
                    }
                    else
                    {
                        RowOccupation = dsOccupation.SocialHx_MiscHx_OccupationHx.NewSocialHx_MiscHx_OccupationHxRow();
                    }

                    if (RowOccupation != null)
                    {
                        // bool isValueDifferent = false;
                        //bool istoUpdateRow = false;
                        if (dsOccupation.SocialHx_MiscHx_OccupationHx.Rows.Count < 1)
                        {
                            RowOccupation.OccupationHxId = currentOccupationId;
                        }
                        RowOccupation.MiscHxId = MiscHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.MiscChildStatus))
                        {
                            RowOccupation.StatusId = MDVUtility.ToInt16(CurrentModel.MiscChildStatus);
                        }
                        else
                        {
                            RowOccupation[dsOccupation.SocialHx_MiscHx_OccupationHx.StatusIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.OccupationPresent))
                        {
                            RowOccupation.Present = MDVUtility.ToStr(CurrentModel.OccupationPresent);
                        }
                        else
                        {
                            RowOccupation[dsOccupation.SocialHx_MiscHx_OccupationHx.PresentColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.OccupationPast))
                        {
                            RowOccupation.Past = MDVUtility.ToStr(CurrentModel.OccupationPast);
                        }
                        else
                        {
                            RowOccupation[dsOccupation.SocialHx_MiscHx_OccupationHx.PastColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.OccupationComments))
                        {
                            RowOccupation.Comments = MDVUtility.ToStr(CurrentModel.OccupationComments);
                        }
                        else
                        {
                            RowOccupation[dsOccupation.SocialHx_MiscHx_OccupationHx.CommentsColumn] = DBNull.Value;
                        }


                        RowOccupation.IsActive = true;

                        if (dsOccupation.SocialHx_MiscHx_OccupationHx.Rows.Count < 1)
                        {
                            RowOccupation.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowOccupation.CreatedOn = DateTime.Now;
                        }

                        RowOccupation.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowOccupation.ModifiedOn = DateTime.Now;

                        // if no occupation is found against OccupationHxId, it implies for new record
                        if (dsOccupation.SocialHx_MiscHx_OccupationHx.Rows.Count < 1)
                        {
                            dsOccupation.SocialHx_MiscHx_OccupationHx.AddSocialHx_MiscHx_OccupationHxRow(RowOccupation);
                        }
                    }
                }
            }
            int counter = 0;

            foreach (DataRow RowOccupation in dsOccupation.SocialHx_MiscHx_OccupationHx.Rows)
            {
                RowOccupation[dsOccupation.SocialHx_MiscHx_OccupationHx.SoapTextColumn] = insertUpdateOccupationText(dsOccupation, lstOccupationModel[counter]);
                counter++;
            }

            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedOccupation = BLLClinicalObj.insertUpdateMiscHxOccupationHx(dsOccupation, patientId);
            if (objInsertedOccupation.Data != null)
            {

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedOccupation.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }

        private string insertUpdateMiscHxOccupationHx(long MiscHxId, List<SocialHxMiscHxOccupationModel> lstOccupationModel, string patientId)
        {

            DSSocialHistory dsOccupation = new DSSocialHistory();

            //  bool isFirstChild = false;
            foreach (SocialHxMiscHxOccupationModel CurrentModel in lstOccupationModel)
            {
                if (CurrentModel.OccupationHxId != null && CurrentModel.IsLast)
                {
                    Int32 currentOccupationId = MDVUtility.ToInt32(CurrentModel.OccupationHxId);
                    currentOccupationId = currentOccupationId == 0 ? -1 : currentOccupationId;
                    BLObject<DSSocialHistory> OccupationObj = BLLClinicalObj.loadMiscHxOccupationHx(Convert.ToInt64(CurrentModel.OccupationHxId), MiscHxId, "", "");
                    dsOccupation = OccupationObj.Data;
                    DSSocialHistory.SocialHx_MiscHx_OccupationHxRow RowOccupation = null;
                    if (dsOccupation.SocialHx_MiscHx_OccupationHx.Rows.Count > 0)
                    {
                        RowOccupation = (DSSocialHistory.SocialHx_MiscHx_OccupationHxRow)dsOccupation.SocialHx_MiscHx_OccupationHx.Rows[0];
                    }
                    else
                    {
                        RowOccupation = dsOccupation.SocialHx_MiscHx_OccupationHx.NewSocialHx_MiscHx_OccupationHxRow();
                    }

                    if (RowOccupation != null)
                    {
                        // bool isValueDifferent = false;
                        //bool istoUpdateRow = false;
                        if (dsOccupation.SocialHx_MiscHx_OccupationHx.Rows.Count < 1)
                        {
                            RowOccupation.OccupationHxId = currentOccupationId;
                        }
                        RowOccupation.MiscHxId = MiscHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.MiscChildStatus))
                        {
                            RowOccupation.StatusId = MDVUtility.ToInt16(CurrentModel.MiscChildStatus);
                        }
                        else
                        {
                            RowOccupation[dsOccupation.SocialHx_MiscHx_OccupationHx.StatusIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.OccupationPresent))
                        {
                            RowOccupation.Present = MDVUtility.ToStr(CurrentModel.OccupationPresent);
                        }
                        else
                        {
                            RowOccupation[dsOccupation.SocialHx_MiscHx_OccupationHx.PresentColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.OccupationPast))
                        {
                            RowOccupation.Past = MDVUtility.ToStr(CurrentModel.OccupationPast);
                        }
                        else
                        {
                            RowOccupation[dsOccupation.SocialHx_MiscHx_OccupationHx.PastColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.OccupationComments))
                        {
                            RowOccupation.Comments = MDVUtility.ToStr(CurrentModel.OccupationComments);
                        }
                        else
                        {
                            RowOccupation[dsOccupation.SocialHx_MiscHx_OccupationHx.CommentsColumn] = DBNull.Value;
                        }


                        RowOccupation.IsActive = true;

                        if (dsOccupation.SocialHx_MiscHx_OccupationHx.Rows.Count < 1)
                        {
                            RowOccupation.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowOccupation.CreatedOn = DateTime.Now;
                        }

                        RowOccupation.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowOccupation.ModifiedOn = DateTime.Now;

                        // if no occupation is found against OccupationHxId, it implies for new record
                        if (dsOccupation.SocialHx_MiscHx_OccupationHx.Rows.Count < 1)
                        {
                            dsOccupation.SocialHx_MiscHx_OccupationHx.AddSocialHx_MiscHx_OccupationHxRow(RowOccupation);
                        }
                    }
                }
            }
            int counter = 0;

            foreach (DataRow RowOccupation in dsOccupation.SocialHx_MiscHx_OccupationHx.Rows)
            {
                RowOccupation[dsOccupation.SocialHx_MiscHx_OccupationHx.SoapTextColumn] = insertUpdateOccupationText(dsOccupation, lstOccupationModel[counter]);
                counter++;
            }

            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedOccupation = BLLClinicalObj.insertUpdateMiscHxOccupationHx(dsOccupation, patientId);
            if (objInsertedOccupation.Data != null)
            {

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedOccupation.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }

        #endregion

        #region insertUpdateSocialHxMiscHxSleepHx

        // Author:  Farooq Ahmad
        // Created Date: 07/01/2016
        //OverView: This function will handle insert/update of DrugAbuse for current SocialHxMiscHxSleepHx on basis of SleepHxId
        private string insertUpdateSleepHx(long MiscHxId, List<object> lstSleepHxObjects, string patientId)
        {
            #region SleepHx
            DSSocialHistory dsMiscHxSleep = new DSSocialHistory();
            List<SocialHxMiscHxSleepModel> lstSleepHxModel = lstSleepHxObjects.OfType<SocialHxMiscHxSleepModel>().ToList();
            foreach (SocialHxMiscHxSleepModel CurrentModel in lstSleepHxModel)
            {
                if (CurrentModel.SleepHxId != null)
                {
                    Int32 currentSleepHxId = MDVUtility.ToInt32(CurrentModel.SleepHxId);
                    currentSleepHxId = currentSleepHxId == 0 ? -1 : currentSleepHxId;
                    BLObject<DSSocialHistory> objMiscHxSleep = BLLClinicalObj.loadMiscHxSleepHx(currentSleepHxId, MiscHxId, "", "");
                    dsMiscHxSleep = objMiscHxSleep.Data;


                    DSSocialHistory.SocialHx_MiscHx_SleepHxRow RowMiscHxSleepHx = null;
                    if (dsMiscHxSleep.SocialHx_MiscHx_SleepHx.Rows.Count > 0)
                    {
                        RowMiscHxSleepHx = (DSSocialHistory.SocialHx_MiscHx_SleepHxRow)dsMiscHxSleep.SocialHx_MiscHx_SleepHx.Rows[0];
                    }
                    else
                    {
                        RowMiscHxSleepHx = dsMiscHxSleep.SocialHx_MiscHx_SleepHx.NewSocialHx_MiscHx_SleepHxRow();
                    }

                    if (RowMiscHxSleepHx != null)
                    {
                        if (dsMiscHxSleep.SocialHx_MiscHx_SleepHx.Rows.Count < 1)
                        {
                            RowMiscHxSleepHx.SleepHxId = currentSleepHxId;
                        }
                        RowMiscHxSleepHx.MiscHxId = MiscHxId;

                        RowMiscHxSleepHx.StatusId = MDVUtility.ToInt32(CurrentModel.MiscChildStatus);

                        if (!string.IsNullOrEmpty(CurrentModel.SleepHours))
                        {
                            RowMiscHxSleepHx.SleepHours = MDVUtility.ToDouble(CurrentModel.SleepHours);
                        }
                        else
                        {
                            RowMiscHxSleepHx[dsMiscHxSleep.SocialHx_MiscHx_SleepHx.SleepHoursColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SleepComments))
                        {
                            RowMiscHxSleepHx.Comments = MDVUtility.ToStr(CurrentModel.SleepComments);
                        }
                        else
                        {
                            RowMiscHxSleepHx[dsMiscHxSleep.SocialHx_MiscHx_SleepHx.CommentsColumn] = DBNull.Value;
                        }


                        string.Format("Patient sleeps <Status>. Sleeping hours are <Sleep Hours>. <Comments>");
                        RowMiscHxSleepHx.IsActive = true;
                        if (dsMiscHxSleep.SocialHx_MiscHx_SleepHx.Rows.Count < 1)
                        {
                            RowMiscHxSleepHx.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowMiscHxSleepHx.CreatedOn = DateTime.Now;

                        }


                        RowMiscHxSleepHx.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowMiscHxSleepHx.ModifiedOn = DateTime.Now;

                        // if no DrugAbuse is found against DrugAbuseId, it implies for new record
                        if (dsMiscHxSleep.SocialHx_MiscHx_SleepHx.Rows.Count < 1)
                        {
                            dsMiscHxSleep.SocialHx_MiscHx_SleepHx.AddSocialHx_MiscHx_SleepHxRow(RowMiscHxSleepHx);
                        }
                    }
                }

            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowSocialHxMiscHxSleepHx in dsMiscHxSleep.SocialHx_MiscHx_SleepHx.Rows)
            {
                RowSocialHxMiscHxSleepHx[dsMiscHxSleep.SocialHx_MiscHx_SleepHx.SoapTextColumn] = insertUpdateSleepHxSoapText(dsMiscHxSleep, lstSleepHxModel[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedMiscHxSleepHx = BLLClinicalObj.insertUpdateMiscHxSleepHx(dsMiscHxSleep, patientId);

            if (objInsertedMiscHxSleepHx.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedMiscHxSleepHx.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }

        private string insertUpdateSleepHx(long MiscHxId, List<SocialHxMiscHxSleepModel> lstSleepHxModel, string patientId)
        {
            #region SleepHx
            DSSocialHistory dsMiscHxSleep = new DSSocialHistory();

            foreach (SocialHxMiscHxSleepModel CurrentModel in lstSleepHxModel)
            {
                if (CurrentModel.SleepHxId != null && CurrentModel.IsLast)
                {
                    Int32 currentSleepHxId = MDVUtility.ToInt32(CurrentModel.SleepHxId);
                    currentSleepHxId = currentSleepHxId == 0 ? -1 : currentSleepHxId;
                    BLObject<DSSocialHistory> objMiscHxSleep = BLLClinicalObj.loadMiscHxSleepHx(currentSleepHxId, MiscHxId, "", "");
                    dsMiscHxSleep = objMiscHxSleep.Data;


                    DSSocialHistory.SocialHx_MiscHx_SleepHxRow RowMiscHxSleepHx = null;
                    if (dsMiscHxSleep.SocialHx_MiscHx_SleepHx.Rows.Count > 0)
                    {
                        RowMiscHxSleepHx = (DSSocialHistory.SocialHx_MiscHx_SleepHxRow)dsMiscHxSleep.SocialHx_MiscHx_SleepHx.Rows[0];
                    }
                    else
                    {
                        RowMiscHxSleepHx = dsMiscHxSleep.SocialHx_MiscHx_SleepHx.NewSocialHx_MiscHx_SleepHxRow();
                    }

                    if (RowMiscHxSleepHx != null)
                    {
                        if (dsMiscHxSleep.SocialHx_MiscHx_SleepHx.Rows.Count < 1)
                        {
                            RowMiscHxSleepHx.SleepHxId = currentSleepHxId;
                        }
                        RowMiscHxSleepHx.MiscHxId = MiscHxId;

                        RowMiscHxSleepHx.StatusId = MDVUtility.ToInt32(CurrentModel.MiscChildStatus);

                        if (!string.IsNullOrEmpty(CurrentModel.SleepHours))
                        {
                            RowMiscHxSleepHx.SleepHours = MDVUtility.ToDouble(CurrentModel.SleepHours);
                        }
                        else
                        {
                            RowMiscHxSleepHx[dsMiscHxSleep.SocialHx_MiscHx_SleepHx.SleepHoursColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SleepComments))
                        {
                            RowMiscHxSleepHx.Comments = MDVUtility.ToStr(CurrentModel.SleepComments);
                        }
                        else
                        {
                            RowMiscHxSleepHx[dsMiscHxSleep.SocialHx_MiscHx_SleepHx.CommentsColumn] = DBNull.Value;
                        }


                        string.Format("Patient sleeps <Status>. Sleeping hours are <Sleep Hours>. <Comments>");
                        RowMiscHxSleepHx.IsActive = true;
                        if (dsMiscHxSleep.SocialHx_MiscHx_SleepHx.Rows.Count < 1)
                        {
                            RowMiscHxSleepHx.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowMiscHxSleepHx.CreatedOn = DateTime.Now;

                        }


                        RowMiscHxSleepHx.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowMiscHxSleepHx.ModifiedOn = DateTime.Now;

                        // if no DrugAbuse is found against DrugAbuseId, it implies for new record
                        if (dsMiscHxSleep.SocialHx_MiscHx_SleepHx.Rows.Count < 1)
                        {
                            dsMiscHxSleep.SocialHx_MiscHx_SleepHx.AddSocialHx_MiscHx_SleepHxRow(RowMiscHxSleepHx);
                        }
                    }
                }

            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowSocialHxMiscHxSleepHx in dsMiscHxSleep.SocialHx_MiscHx_SleepHx.Rows)
            {
                RowSocialHxMiscHxSleepHx[dsMiscHxSleep.SocialHx_MiscHx_SleepHx.SoapTextColumn] = insertUpdateSleepHxSoapText(dsMiscHxSleep, lstSleepHxModel[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedMiscHxSleepHx = BLLClinicalObj.insertUpdateMiscHxSleepHx(dsMiscHxSleep, patientId);

            if (objInsertedMiscHxSleepHx.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedMiscHxSleepHx.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }



        #endregion

        #region insertUpdateSocialHxMiscHxExercisesHx

        // Author:  Farooq Ahmad
        // Created Date: 08/01/2016
        //OverView: This function will handle insert/update of DrugAbuse for current SocialHxMiscHxSleepHx on basis of SleepHxId
        private string insertUpdateExercisesHx(long MiscHxId, List<object> lstExercisesHxObjects, string patientId)
        {
            #region ExercisesHx
            DSSocialHistory dsMiscHxExercises = new DSSocialHistory();
            List<SocialHxMiscHxExercisesModel> lstExercisesHxModel = lstExercisesHxObjects.OfType<SocialHxMiscHxExercisesModel>().ToList();
            foreach (SocialHxMiscHxExercisesModel CurrentModel in lstExercisesHxModel)
            {
                if (CurrentModel.ExercisesHxId != null)
                {
                    Int32 currentExercisesHxId = MDVUtility.ToInt32(CurrentModel.ExercisesHxId);
                    currentExercisesHxId = currentExercisesHxId == 0 ? -1 : currentExercisesHxId;
                    BLObject<DSSocialHistory> objMiscHxExercises = BLLClinicalObj.loadMiscHxExercisesHx(currentExercisesHxId, MiscHxId, "", "");
                    dsMiscHxExercises = objMiscHxExercises.Data;


                    DSSocialHistory.SocialHx_MiscHx_ExercisesHxRow RowMiscHxExercises = null;
                    if (dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.Rows.Count > 0)
                    {
                        RowMiscHxExercises = (DSSocialHistory.SocialHx_MiscHx_ExercisesHxRow)dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.Rows[0];
                    }
                    else
                    {
                        RowMiscHxExercises = dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.NewSocialHx_MiscHx_ExercisesHxRow();
                    }

                    if (RowMiscHxExercises != null)
                    {
                        if (dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.Rows.Count < 1)
                        {
                            RowMiscHxExercises.ExercisesHxId = currentExercisesHxId;
                        }
                        RowMiscHxExercises.MiscHxId = MiscHxId;

                        RowMiscHxExercises.StatusId = MDVUtility.ToInt32(CurrentModel.MiscChildStatus);

                        if (!string.IsNullOrEmpty(CurrentModel.ExercisesType))
                        {
                            RowMiscHxExercises.TypeId = MDVUtility.ToInt16(CurrentModel.ExercisesType);
                        }
                        else
                        {
                            RowMiscHxExercises[dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.TypeIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ExercisesDiet))
                        {
                            RowMiscHxExercises.DietId = MDVUtility.ToInt16(CurrentModel.ExercisesDiet);
                        }
                        else
                        {
                            RowMiscHxExercises[dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.DietIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ExercisesComments))
                        {
                            RowMiscHxExercises.Comments = MDVUtility.ToStr(CurrentModel.ExercisesComments);
                        }
                        else
                        {
                            RowMiscHxExercises[dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.CommentsColumn] = DBNull.Value;
                        }



                        RowMiscHxExercises.IsActive = true;
                        if (dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.Rows.Count < 1)
                        {
                            RowMiscHxExercises.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowMiscHxExercises.CreatedOn = DateTime.Now;
                        }

                        RowMiscHxExercises.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowMiscHxExercises.ModifiedOn = DateTime.Now;

                        // if no DrugAbuse is found against DrugAbuseId, it implies for new record
                        if (dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.Rows.Count < 1)
                        {
                            dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.AddSocialHx_MiscHx_ExercisesHxRow(RowMiscHxExercises);
                        }
                    }
                }

            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowSocialHxMiscHxExercisesHx in dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.Rows)
            {
                RowSocialHxMiscHxExercisesHx[dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.SoapTextColumn] = insertUpdateExercisesHxSoapText(dsMiscHxExercises, lstExercisesHxModel[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedMiscHxExercisesHx = BLLClinicalObj.insertUpdateMiscHxExercisesHx(dsMiscHxExercises, patientId);

            if (objInsertedMiscHxExercisesHx.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedMiscHxExercisesHx.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }

        private string insertUpdateExercisesHx(long MiscHxId, List<SocialHxMiscHxExercisesModel> lstExercisesHxModel, string patientId)
        {
            #region ExercisesHx
            DSSocialHistory dsMiscHxExercises = new DSSocialHistory();

            foreach (SocialHxMiscHxExercisesModel CurrentModel in lstExercisesHxModel)
            {
                if (CurrentModel.ExercisesHxId != null && CurrentModel.IsLast)
                {
                    Int32 currentExercisesHxId = MDVUtility.ToInt32(CurrentModel.ExercisesHxId);
                    currentExercisesHxId = currentExercisesHxId == 0 ? -1 : currentExercisesHxId;
                    BLObject<DSSocialHistory> objMiscHxExercises = BLLClinicalObj.loadMiscHxExercisesHx(currentExercisesHxId, MiscHxId, "", "");
                    dsMiscHxExercises = objMiscHxExercises.Data;


                    DSSocialHistory.SocialHx_MiscHx_ExercisesHxRow RowMiscHxExercises = null;
                    if (dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.Rows.Count > 0)
                    {
                        RowMiscHxExercises = (DSSocialHistory.SocialHx_MiscHx_ExercisesHxRow)dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.Rows[0];
                    }
                    else
                    {
                        RowMiscHxExercises = dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.NewSocialHx_MiscHx_ExercisesHxRow();
                    }

                    if (RowMiscHxExercises != null)
                    {
                        if (dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.Rows.Count < 1)
                        {
                            RowMiscHxExercises.ExercisesHxId = currentExercisesHxId;
                        }
                        RowMiscHxExercises.MiscHxId = MiscHxId;

                        RowMiscHxExercises.StatusId = MDVUtility.ToInt32(CurrentModel.MiscChildStatus);

                        if (!string.IsNullOrEmpty(CurrentModel.ExercisesType))
                        {
                            RowMiscHxExercises.TypeId = MDVUtility.ToInt16(CurrentModel.ExercisesType);
                        }
                        else
                        {
                            RowMiscHxExercises[dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.TypeIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ExercisesDiet))
                        {
                            RowMiscHxExercises.DietId = MDVUtility.ToInt16(CurrentModel.ExercisesDiet);
                        }
                        else
                        {
                            RowMiscHxExercises[dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.DietIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ExercisesComments))
                        {
                            RowMiscHxExercises.Comments = MDVUtility.ToStr(CurrentModel.ExercisesComments);
                        }
                        else
                        {
                            RowMiscHxExercises[dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.CommentsColumn] = DBNull.Value;
                        }



                        RowMiscHxExercises.IsActive = true;
                        if (dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.Rows.Count < 1)
                        {
                            RowMiscHxExercises.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowMiscHxExercises.CreatedOn = DateTime.Now;
                        }

                        RowMiscHxExercises.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowMiscHxExercises.ModifiedOn = DateTime.Now;

                        // if no DrugAbuse is found against DrugAbuseId, it implies for new record
                        if (dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.Rows.Count < 1)
                        {
                            dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.AddSocialHx_MiscHx_ExercisesHxRow(RowMiscHxExercises);
                        }
                    }
                }

            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowSocialHxMiscHxExercisesHx in dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.Rows)
            {
                RowSocialHxMiscHxExercisesHx[dsMiscHxExercises.SocialHx_MiscHx_ExercisesHx.SoapTextColumn] = insertUpdateExercisesHxSoapText(dsMiscHxExercises, lstExercisesHxModel[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedMiscHxExercisesHx = BLLClinicalObj.insertUpdateMiscHxExercisesHx(dsMiscHxExercises, patientId);

            if (objInsertedMiscHxExercisesHx.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedMiscHxExercisesHx.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }




        #endregion

        #region insertUpdateSocialHxMiscHxHousingHx

        // Author:  Farooq Ahmad
        // Created Date: 08/01/2016
        //OverView: This function will handle insert/update of DrugAbuse for current SocialHxMiscHxSleepHx on basis of SleepHxId
        private string insertUpdateHousingHx(long MiscHxId, List<object> lstHousingHxObjects, string patientId)
        {
            #region HousingHx
            DSSocialHistory dsMiscHxHousing = new DSSocialHistory();
            List<SocialHxMiscHxHousingModel> lstHousingHxModel = lstHousingHxObjects.OfType<SocialHxMiscHxHousingModel>().ToList();
            foreach (SocialHxMiscHxHousingModel CurrentModel in lstHousingHxModel)
            {
                if (CurrentModel.HousingHxId != null)
                {
                    Int32 currentHousingHxId = MDVUtility.ToInt32(CurrentModel.HousingHxId);
                    currentHousingHxId = currentHousingHxId == 0 ? -1 : currentHousingHxId;
                    BLObject<DSSocialHistory> objMiscHxHousing = BLLClinicalObj.loadMiscHxHousingHx(currentHousingHxId, MiscHxId, "", "");
                    dsMiscHxHousing = objMiscHxHousing.Data;


                    DSSocialHistory.SocialHx_MiscHx_HousingHxRow RowMiscHxHousing = null;
                    if (dsMiscHxHousing.SocialHx_MiscHx_HousingHx.Rows.Count > 0)
                    {
                        RowMiscHxHousing = (DSSocialHistory.SocialHx_MiscHx_HousingHxRow)dsMiscHxHousing.SocialHx_MiscHx_HousingHx.Rows[0];
                    }
                    else
                    {
                        RowMiscHxHousing = dsMiscHxHousing.SocialHx_MiscHx_HousingHx.NewSocialHx_MiscHx_HousingHxRow();
                    }

                    if (RowMiscHxHousing != null)
                    {
                        if (dsMiscHxHousing.SocialHx_MiscHx_HousingHx.Rows.Count < 1)
                        {
                            RowMiscHxHousing.HousingHxId = currentHousingHxId;
                        }
                        RowMiscHxHousing.MiscHxId = MiscHxId;

                        RowMiscHxHousing.StatusId = MDVUtility.ToInt32(CurrentModel.MiscChildStatus);

                        if (!string.IsNullOrEmpty(CurrentModel.HousingPresent))
                        {
                            RowMiscHxHousing.Present = CurrentModel.HousingPresent;
                        }
                        else
                        {
                            RowMiscHxHousing[dsMiscHxHousing.SocialHx_MiscHx_HousingHx.PresentColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.HousingPast))
                        {
                            RowMiscHxHousing.Past = CurrentModel.HousingPast;
                        }
                        else
                        {
                            RowMiscHxHousing[dsMiscHxHousing.SocialHx_MiscHx_HousingHx.PastColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.HousingComments))
                        {
                            RowMiscHxHousing.Comments = MDVUtility.ToStr(CurrentModel.HousingComments);
                        }
                        else
                        {
                            RowMiscHxHousing[dsMiscHxHousing.SocialHx_MiscHx_HousingHx.CommentsColumn] = DBNull.Value;
                        }



                        RowMiscHxHousing.IsActive = true;
                        if (dsMiscHxHousing.SocialHx_MiscHx_HousingHx.Rows.Count < 1)
                        {
                            RowMiscHxHousing.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowMiscHxHousing.CreatedOn = DateTime.Now;
                        }

                        RowMiscHxHousing.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowMiscHxHousing.ModifiedOn = DateTime.Now;

                        // if no DrugAbuse is found against DrugAbuseId, it implies for new record
                        if (dsMiscHxHousing.SocialHx_MiscHx_HousingHx.Rows.Count < 1)
                        {
                            dsMiscHxHousing.SocialHx_MiscHx_HousingHx.AddSocialHx_MiscHx_HousingHxRow(RowMiscHxHousing);
                        }
                    }
                }

            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowSocialHxMiscHxHousingHx in dsMiscHxHousing.SocialHx_MiscHx_HousingHx.Rows)
            {
                RowSocialHxMiscHxHousingHx[dsMiscHxHousing.SocialHx_MiscHx_HousingHx.SoapTextColumn] = insertUpdateHousingHxSoapText(dsMiscHxHousing, lstHousingHxModel[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedMiscHxHousingHx = BLLClinicalObj.insertUpdateMiscHxHousingHx(dsMiscHxHousing, patientId);

            if (objInsertedMiscHxHousingHx.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedMiscHxHousingHx.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }

        private string insertUpdateHousingHx(long MiscHxId, List<SocialHxMiscHxHousingModel> lstHousingHxModel, string patientId)
        {
            #region HousingHx
            DSSocialHistory dsMiscHxHousing = new DSSocialHistory();

            foreach (SocialHxMiscHxHousingModel CurrentModel in lstHousingHxModel)
            {
                if (CurrentModel.HousingHxId != null && CurrentModel.IsLast)
                {
                    Int32 currentHousingHxId = MDVUtility.ToInt32(CurrentModel.HousingHxId);
                    currentHousingHxId = currentHousingHxId == 0 ? -1 : currentHousingHxId;
                    BLObject<DSSocialHistory> objMiscHxHousing = BLLClinicalObj.loadMiscHxHousingHx(currentHousingHxId, MiscHxId, "", "");
                    dsMiscHxHousing = objMiscHxHousing.Data;


                    DSSocialHistory.SocialHx_MiscHx_HousingHxRow RowMiscHxHousing = null;
                    if (dsMiscHxHousing.SocialHx_MiscHx_HousingHx.Rows.Count > 0)
                    {
                        RowMiscHxHousing = (DSSocialHistory.SocialHx_MiscHx_HousingHxRow)dsMiscHxHousing.SocialHx_MiscHx_HousingHx.Rows[0];
                    }
                    else
                    {
                        RowMiscHxHousing = dsMiscHxHousing.SocialHx_MiscHx_HousingHx.NewSocialHx_MiscHx_HousingHxRow();
                    }

                    if (RowMiscHxHousing != null)
                    {
                        if (dsMiscHxHousing.SocialHx_MiscHx_HousingHx.Rows.Count < 1)
                        {
                            RowMiscHxHousing.HousingHxId = currentHousingHxId;
                        }
                        RowMiscHxHousing.MiscHxId = MiscHxId;

                        RowMiscHxHousing.StatusId = MDVUtility.ToInt32(CurrentModel.MiscChildStatus);

                        if (!string.IsNullOrEmpty(CurrentModel.HousingPresent))
                        {
                            RowMiscHxHousing.Present = CurrentModel.HousingPresent;
                        }
                        else
                        {
                            RowMiscHxHousing[dsMiscHxHousing.SocialHx_MiscHx_HousingHx.PresentColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.HousingPast))
                        {
                            RowMiscHxHousing.Past = CurrentModel.HousingPast;
                        }
                        else
                        {
                            RowMiscHxHousing[dsMiscHxHousing.SocialHx_MiscHx_HousingHx.PastColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.HousingComments))
                        {
                            RowMiscHxHousing.Comments = MDVUtility.ToStr(CurrentModel.HousingComments);
                        }
                        else
                        {
                            RowMiscHxHousing[dsMiscHxHousing.SocialHx_MiscHx_HousingHx.CommentsColumn] = DBNull.Value;
                        }



                        RowMiscHxHousing.IsActive = true;
                        if (dsMiscHxHousing.SocialHx_MiscHx_HousingHx.Rows.Count < 1)
                        {
                            RowMiscHxHousing.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowMiscHxHousing.CreatedOn = DateTime.Now;
                        }

                        RowMiscHxHousing.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowMiscHxHousing.ModifiedOn = DateTime.Now;

                        // if no DrugAbuse is found against DrugAbuseId, it implies for new record
                        if (dsMiscHxHousing.SocialHx_MiscHx_HousingHx.Rows.Count < 1)
                        {
                            dsMiscHxHousing.SocialHx_MiscHx_HousingHx.AddSocialHx_MiscHx_HousingHxRow(RowMiscHxHousing);
                        }
                    }
                }

            }
            int counter = 0;
            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowSocialHxMiscHxHousingHx in dsMiscHxHousing.SocialHx_MiscHx_HousingHx.Rows)
            {
                RowSocialHxMiscHxHousingHx[dsMiscHxHousing.SocialHx_MiscHx_HousingHx.SoapTextColumn] = insertUpdateHousingHxSoapText(dsMiscHxHousing, lstHousingHxModel[counter]);
                counter++;
            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedMiscHxHousingHx = BLLClinicalObj.insertUpdateMiscHxHousingHx(dsMiscHxHousing, patientId);

            if (objInsertedMiscHxHousingHx.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedMiscHxHousingHx.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }






        #endregion

        #region insertCaffeineIntakeHx
        private string insertUpdateCaffeineIntakeHx(long MiscHxId, List<object> lstCaffeineIntakHxObjects, string patientId)
        {

            DSSocialHistory dsCaffeineIntakHx = new DSSocialHistory();
            List<SocialHxMiscHxCaffeineIntakeModel> lstCaffeineIntakHxModel = lstCaffeineIntakHxObjects.OfType<SocialHxMiscHxCaffeineIntakeModel>().ToList();
            //  bool isFirstChild = false;
            foreach (SocialHxMiscHxCaffeineIntakeModel CurrentModel in lstCaffeineIntakHxModel)
            {
                if (CurrentModel.CaffeineIntakeHxId != null)
                {
                    Int32 currentIntakeHxId = MDVUtility.ToInt32(CurrentModel.CaffeineIntakeHxId);
                    currentIntakeHxId = currentIntakeHxId == 0 ? -1 : currentIntakeHxId;
                    BLObject<DSSocialHistory> CaffeineIntakHxObj = BLLClinicalObj.loadMiscHxCaffeineIntakeHx(currentIntakeHxId, MiscHxId, "", "");
                    dsCaffeineIntakHx = CaffeineIntakHxObj.Data;
                    DSSocialHistory.SocialHx_MiscHx_CaffeineIntakHxRow RowCaffeineIntakHx = null;
                    if (dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.Rows.Count > 0)
                    {
                        RowCaffeineIntakHx = (DSSocialHistory.SocialHx_MiscHx_CaffeineIntakHxRow)dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.Rows[0];
                    }
                    else
                    {
                        RowCaffeineIntakHx = dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.NewSocialHx_MiscHx_CaffeineIntakHxRow();
                    }

                    if (RowCaffeineIntakHx != null)
                    {
                        // bool isValueDifferent = false;
                        //bool istoUpdateRow = false;
                        if (dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.Rows.Count < 1)
                        {
                            RowCaffeineIntakHx.CaffeineIntakHxId = currentIntakeHxId;
                        }
                        RowCaffeineIntakHx.MiscHxId = MiscHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.MiscChildStatus))
                        {
                            RowCaffeineIntakHx.StatusId = MDVUtility.ToInt16(CurrentModel.MiscChildStatus);
                        }
                        else
                        {
                            RowCaffeineIntakHx[dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.StatusIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.CaffeineIntakFrequency))
                        {
                            RowCaffeineIntakHx.FrequencyId = MDVUtility.ToInt32(CurrentModel.CaffeineIntakFrequency);
                        }
                        else
                        {
                            RowCaffeineIntakHx[dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.FrequencyIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.RadCaffieneharmful))
                        {
                            if (CurrentModel.RadCaffieneharmful == "False")
                            {
                                RowCaffeineIntakHx.IsHarmful = false;
                            }
                            else if (CurrentModel.RadCaffieneharmful == "True")
                            {
                                RowCaffeineIntakHx.IsHarmful = true;
                            }
                            else
                            {
                                RowCaffeineIntakHx[dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.IsHarmfulColumn] = DBNull.Value;
                            }
                        }
                        else
                        {
                            RowCaffeineIntakHx[dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.IsHarmfulColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CaffeineComments))
                        {
                            RowCaffeineIntakHx.Comments = MDVUtility.ToStr(CurrentModel.CaffeineComments);
                        }
                        else
                        {
                            RowCaffeineIntakHx[dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.CommentsColumn] = DBNull.Value;
                        }


                        RowCaffeineIntakHx.IsActive = true;
                        if (dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.Rows.Count < 1)
                        {
                            RowCaffeineIntakHx.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowCaffeineIntakHx.CreatedOn = DateTime.Now;
                        }
                        RowCaffeineIntakHx.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowCaffeineIntakHx.ModifiedOn = DateTime.Now;

                        // if no occupation is found against OccupationHxId, it implies for new record
                        if (dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.Rows.Count < 1)
                        {
                            dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.AddSocialHx_MiscHx_CaffeineIntakHxRow(RowCaffeineIntakHx);
                        }
                    }
                }
            }
            int counter = 0;

            foreach (DataRow RowOccupation in dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.Rows)
            {
                RowOccupation[dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.SoapTextColumn] = insertUpdateCaffeineIntakeHxText(dsCaffeineIntakHx, lstCaffeineIntakHxModel[counter]);
                counter++;
            }

            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedOccupation = BLLClinicalObj.insertUpdateMiscHxCaffeineIntakeHx(dsCaffeineIntakHx, patientId);
            if (objInsertedOccupation.Data != null)
            {

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedOccupation.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }

        private string insertUpdateCaffeineIntakeHx(long MiscHxId, List<SocialHxMiscHxCaffeineIntakeModel> lstCaffeineIntakHxModel, string patientId)
        {

            DSSocialHistory dsCaffeineIntakHx = new DSSocialHistory();

            //  bool isFirstChild = false;
            foreach (SocialHxMiscHxCaffeineIntakeModel CurrentModel in lstCaffeineIntakHxModel)
            {
                if (CurrentModel.CaffeineIntakeHxId != null && CurrentModel.IsLast)
                {
                    Int32 currentIntakeHxId = MDVUtility.ToInt32(CurrentModel.CaffeineIntakeHxId);
                    currentIntakeHxId = currentIntakeHxId == 0 ? -1 : currentIntakeHxId;
                    BLObject<DSSocialHistory> CaffeineIntakHxObj = BLLClinicalObj.loadMiscHxCaffeineIntakeHx(currentIntakeHxId, MiscHxId, "", "");
                    dsCaffeineIntakHx = CaffeineIntakHxObj.Data;
                    DSSocialHistory.SocialHx_MiscHx_CaffeineIntakHxRow RowCaffeineIntakHx = null;
                    if (dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.Rows.Count > 0)
                    {
                        RowCaffeineIntakHx = (DSSocialHistory.SocialHx_MiscHx_CaffeineIntakHxRow)dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.Rows[0];
                    }
                    else
                    {
                        RowCaffeineIntakHx = dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.NewSocialHx_MiscHx_CaffeineIntakHxRow();
                    }

                    if (RowCaffeineIntakHx != null)
                    {
                        // bool isValueDifferent = false;
                        //bool istoUpdateRow = false;
                        if (dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.Rows.Count < 1)
                        {
                            RowCaffeineIntakHx.CaffeineIntakHxId = currentIntakeHxId;
                        }
                        RowCaffeineIntakHx.MiscHxId = MiscHxId;

                        if (!string.IsNullOrEmpty(CurrentModel.MiscChildStatus))
                        {
                            RowCaffeineIntakHx.StatusId = MDVUtility.ToInt16(CurrentModel.MiscChildStatus);
                        }
                        else
                        {
                            RowCaffeineIntakHx[dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.StatusIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(CurrentModel.CaffeineIntakFrequency))
                        {
                            RowCaffeineIntakHx.FrequencyId = MDVUtility.ToInt32(CurrentModel.CaffeineIntakFrequency);
                        }
                        else
                        {
                            RowCaffeineIntakHx[dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.FrequencyIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.RadCaffieneharmful))
                        {
                            if (CurrentModel.RadCaffieneharmful == "False")
                            {
                                RowCaffeineIntakHx.IsHarmful = false;
                            }
                            else if (CurrentModel.RadCaffieneharmful == "True")
                            {
                                RowCaffeineIntakHx.IsHarmful = true;
                            }
                            else
                            {
                                RowCaffeineIntakHx[dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.IsHarmfulColumn] = DBNull.Value;
                            }
                        }
                        else
                        {
                            RowCaffeineIntakHx[dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.IsHarmfulColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CaffeineComments))
                        {
                            RowCaffeineIntakHx.Comments = MDVUtility.ToStr(CurrentModel.CaffeineComments);
                        }
                        else
                        {
                            RowCaffeineIntakHx[dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.CommentsColumn] = DBNull.Value;
                        }


                        RowCaffeineIntakHx.IsActive = true;
                        if (dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.Rows.Count < 1)
                        {
                            RowCaffeineIntakHx.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowCaffeineIntakHx.CreatedOn = DateTime.Now;
                        }
                        RowCaffeineIntakHx.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowCaffeineIntakHx.ModifiedOn = DateTime.Now;

                        // if no occupation is found against OccupationHxId, it implies for new record
                        if (dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.Rows.Count < 1)
                        {
                            dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.AddSocialHx_MiscHx_CaffeineIntakHxRow(RowCaffeineIntakHx);
                        }
                    }
                }
            }
            int counter = 0;

            foreach (DataRow RowOccupation in dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.Rows)
            {
                RowOccupation[dsCaffeineIntakHx.SocialHx_MiscHx_CaffeineIntakHx.SoapTextColumn] = insertUpdateCaffeineIntakeHxText(dsCaffeineIntakHx, lstCaffeineIntakHxModel[counter]);
                counter++;
            }

            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedOccupation = BLLClinicalObj.insertUpdateMiscHxCaffeineIntakeHx(dsCaffeineIntakHx, patientId);
            if (objInsertedOccupation.Data != null)
            {

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                    //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedOccupation.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }
        #endregion

        #region InserUpdateSocialHxTravel
        public string insertUpdateTravelHx(long MiscHxId, List<object> lstTravelHxObjects, string patientId)
        {
            #region TraveHx
            DSSocialHistory dsMiscHxTravel = new DSSocialHistory();
            List<SocialHxMiscHxTravelHxModel> lstTravelHxModel = lstTravelHxObjects.OfType<SocialHxMiscHxTravelHxModel>().ToList();
            foreach (SocialHxMiscHxTravelHxModel CurrentModel in lstTravelHxModel)
            {
                if (CurrentModel.TravelHxId != null)
                {
                    Int32 TravelHxHxId = MDVUtility.ToInt32(CurrentModel.TravelHxId);
                    TravelHxHxId = TravelHxHxId == 0 ? -1 : TravelHxHxId;
                    BLObject<DSSocialHistory> objMiscHxTravel = BLLClinicalObj.loadMiscHxTravelHx(TravelHxHxId, MiscHxId, "", "");
                    dsMiscHxTravel = objMiscHxTravel.Data;


                    DSSocialHistory.SocialHx_MiscHx_TravelHxRow RowMiscHxTravel = null;
                    if (dsMiscHxTravel.SocialHx_MiscHx_TravelHx.Rows.Count > 0)
                    {
                        RowMiscHxTravel = (DSSocialHistory.SocialHx_MiscHx_TravelHxRow)dsMiscHxTravel.SocialHx_MiscHx_TravelHx.Rows[0];
                    }
                    else
                    {
                        RowMiscHxTravel = dsMiscHxTravel.SocialHx_MiscHx_TravelHx.NewSocialHx_MiscHx_TravelHxRow();
                    }

                    if (RowMiscHxTravel != null)
                    {
                        if (dsMiscHxTravel.SocialHx_MiscHx_TravelHx.Rows.Count < 1)
                        {
                            RowMiscHxTravel.TravelHxId = TravelHxHxId;
                        }
                        RowMiscHxTravel.MiscHxId = MiscHxId;

                        RowMiscHxTravel.StatusId = MDVUtility.ToInt32(CurrentModel.MiscChildStatus);

                        if (!string.IsNullOrEmpty(CurrentModel.TravelHxComments))
                        {
                            RowMiscHxTravel.Comments = CurrentModel.TravelHxComments;
                        }
                        else
                        {
                            RowMiscHxTravel[dsMiscHxTravel.SocialHx_MiscHx_TravelHx.CommentsColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.TravelHxToDate))
                        {
                            RowMiscHxTravel.Todate = MDVUtility.StringToDate(CurrentModel.TravelHxToDate);
                        }
                        else
                        {
                            RowMiscHxTravel[dsMiscHxTravel.SocialHx_MiscHx_TravelHx.TodateColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.TravelHxFromDate))
                        {
                            RowMiscHxTravel.FromDate = MDVUtility.StringToDate(CurrentModel.TravelHxFromDate);
                        }
                        else
                        {
                            RowMiscHxTravel[dsMiscHxTravel.SocialHx_MiscHx_TravelHx.TodateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.TravelHxLocation))
                        {
                            RowMiscHxTravel.Location = CurrentModel.TravelHxLocation;
                        }
                        else
                        {
                            RowMiscHxTravel[dsMiscHxTravel.SocialHx_MiscHx_TravelHx.LocationColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.PatientId))
                        {
                            RowMiscHxTravel.PatientId = CurrentModel.PatientId;
                        }
                        else
                        {
                            RowMiscHxTravel[dsMiscHxTravel.SocialHx_MiscHx_TravelHx.PatientIdColumn] = DBNull.Value;
                        }
                        RowMiscHxTravel.IsActive = true;
                        RowMiscHxTravel.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowMiscHxTravel.CreatedOn = DateTime.Now;
                        RowMiscHxTravel.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowMiscHxTravel.ModifiedOn = DateTime.Now;
                        if (dsMiscHxTravel.SocialHx_MiscHx_TravelHx.Rows.Count < 1)
                        {
                            dsMiscHxTravel.SocialHx_MiscHx_TravelHx.AddSocialHx_MiscHx_TravelHxRow(RowMiscHxTravel);
                        }
                    }
                }

            }
            #region Database Insertion/Updation

            BLObject<DSSocialHistory> objInsertedMiscHxHousingHx = BLLClinicalObj.insertUpdateMiscHxTravelHx(dsMiscHxTravel);

            if (objInsertedMiscHxHousingHx.Data != null)
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
                    Message = objInsertedMiscHxHousingHx.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion


            #endregion
        }
        public string insertUpdateTravelHx(SocialHxMiscHxTravelHxModel CurrentModel)
        {
            try
            {
                #region TraveHx
                DSSocialHistory dsSocialHx = new DSSocialHistory();
                BLObject<DSSocialHistory> obj = null;
                if (!string.IsNullOrEmpty(CurrentModel.MiscChildStatus))
                {
                    DSSocialHistory.SocialHxRow dr;
                    string InsertSocialSoapText = string.Empty;

                    obj = BLLClinicalObj.LoadSocialHx(MDVUtility.ToInt64(CurrentModel.PatientId), MDVUtility.ToInt64(CurrentModel.SocialHxId) > 0 ? MDVUtility.ToInt64(CurrentModel.SocialHxId) : 0);
                    if (obj.Data != null)
                    {
                        dsSocialHx = obj.Data;
                        if (dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows.Count > 0)
                        {
                            dr = dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0] as DSSocialHistory.SocialHxRow;
                            CurrentModel.SocialHxId = MDVUtility.ToStr(dr.SocialHxId);
                        }
                        else
                        {
                            dr = dsSocialHx.SocialHx.NewSocialHxRow();
                            dr.SocialHxId = -1;
                        }
                        dr.PatientId = MDVUtility.ToInt64(CurrentModel.PatientId);
                        if (!string.IsNullOrEmpty(CurrentModel.SocialHxDate))
                            dr.SocialHxDate = MDVUtility.ToDateTime(CurrentModel.SocialHxDate);
                        else
                            dr[dsSocialHx.SocialHx.SocialHxDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(CurrentModel.SocialComments))
                            dr.Comments = "<div id='overallcomment' title='overallcomment'  name='overallcomment'><strong>Overall Comments:</strong> " + MDVUtility.ToStr(CurrentModel.SocialComments) + "</div>";
                        else
                            dr[dsSocialHx.SocialHx.CommentsColumn] = DBNull.Value;
                        dr.bUnremarkable = CurrentModel.SocialHxUnremarkable.ToLower() == "true" ? true : false;
                        dr.IsActive = true;
                        if (MDVUtility.ToInt64(CurrentModel.SocialHxId) <= 0)
                        {
                            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.CreatedOn = DateTime.Now;
                        }
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        obj = null;
                        if (!string.IsNullOrEmpty(CurrentModel.SocialHxId) && MDVUtility.ToInt64(CurrentModel.SocialHxId) > 0)
                            obj = BLLClinicalObj.UpdateSocialHx(dsSocialHx);
                        else
                        {
                            dsSocialHx.SocialHx.AddSocialHxRow(dr);
                            obj = BLLClinicalObj.InsertSocialHx(dsSocialHx);
                        }
                        if (obj.Data != null)
                        {
                            dsSocialHx = obj.Data;
                            Int64 SocialHxId = MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]);
                            string MiscHxResponse = insertUpdateMiscHx(MDVUtility.ToLong(SocialHxId));
                            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                            ser.MaxJsonLength = Int32.MaxValue;
                            var MiscHxResponseJSON = ser.Deserialize<dynamic>(MiscHxResponse);
                            Int64 MiscHxId = MDVUtility.ToStr(MiscHxResponseJSON["MiscHxId"]) != "" ? MDVUtility.ToInt64(MiscHxResponseJSON["MiscHxId"]) : 0;
                            if (MiscHxId > 0)
                            {
                                DateTime _datetime;
                                Int32 TravelHxHxId = MDVUtility.ToInt32(CurrentModel.TravelHxId);
                                TravelHxHxId = TravelHxHxId == 0 || TravelHxHxId == -1 ? -1 : TravelHxHxId;
                                obj = null;
                                DSSocialHistory.SocialHx_MiscHx_TravelHxRow RowMiscHxTravel = null;
                                obj = BLLClinicalObj.FillMiscHxTravelHx(MDVUtility.ToLong(TravelHxHxId), MDVUtility.ToLong(0), 15, 1);
                                if (obj.Data != null)
                                {
                                    dsSocialHx.Merge(obj.Data);
                                    if (dsSocialHx.SocialHx_MiscHx_TravelHx.Rows.Count > 0)
                                        RowMiscHxTravel = (DSSocialHistory.SocialHx_MiscHx_TravelHxRow)dsSocialHx.SocialHx_MiscHx_TravelHx.Rows[0];
                                    else
                                    {
                                        RowMiscHxTravel = dsSocialHx.SocialHx_MiscHx_TravelHx.NewSocialHx_MiscHx_TravelHxRow();
                                        RowMiscHxTravel.TravelHxId = -1;
                                    }
                                    RowMiscHxTravel.MiscHxId = MiscHxId;
                                    RowMiscHxTravel.StatusId = MDVUtility.ToInt32(CurrentModel.MiscChildStatus);
                                    if (!string.IsNullOrEmpty(CurrentModel.TravelHxComments))
                                        RowMiscHxTravel.Comments = CurrentModel.TravelHxComments;
                                    else
                                        RowMiscHxTravel[dsSocialHx.SocialHx_MiscHx_TravelHx.CommentsColumn] = DBNull.Value;
                                    if (!string.IsNullOrEmpty(CurrentModel.TravelHxToDate))
                                        if (DateTime.TryParse(CurrentModel.TravelHxToDate, out _datetime))
                                            RowMiscHxTravel.Todate = _datetime;
                                        else
                                            RowMiscHxTravel[dsSocialHx.SocialHx_MiscHx_TravelHx.TodateColumn] = DBNull.Value;
                                    else
                                        RowMiscHxTravel[dsSocialHx.SocialHx_MiscHx_TravelHx.TodateColumn] = DBNull.Value;

                                    if (!string.IsNullOrEmpty(CurrentModel.TravelHxFromDate))
                                        if (DateTime.TryParse(CurrentModel.TravelHxFromDate, out _datetime))
                                            RowMiscHxTravel.FromDate = _datetime;
                                        else
                                            RowMiscHxTravel[dsSocialHx.SocialHx_MiscHx_TravelHx.FromDateColumn] = DBNull.Value;
                                    else
                                        RowMiscHxTravel[dsSocialHx.SocialHx_MiscHx_TravelHx.FromDateColumn] = DBNull.Value;
                                    if (!string.IsNullOrEmpty(CurrentModel.TravelHxLocation))
                                        RowMiscHxTravel.Location = CurrentModel.TravelHxLocation;
                                    else
                                        RowMiscHxTravel[dsSocialHx.SocialHx_MiscHx_TravelHx.LocationColumn] = DBNull.Value;
                                    RowMiscHxTravel.IsActive = true;
                                    if ((MDVUtility.ToInt64(CurrentModel.TravelHxId) <= 0))
                                    {
                                        RowMiscHxTravel.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                        RowMiscHxTravel.CreatedOn = DateTime.Now;
                                    }
                                    else
                                        RowMiscHxTravel[dsSocialHx.SocialHx_MiscHx_TravelHx.SoapTextColumn] = insertUpdateTravelText(CurrentModel);
                                    RowMiscHxTravel.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                    RowMiscHxTravel.ModifiedOn = DateTime.Now;
                                    if (RowMiscHxTravel.TravelHxId == -1)
                                        dsSocialHx.SocialHx_MiscHx_TravelHx.AddSocialHx_MiscHx_TravelHxRow(RowMiscHxTravel);
                                    obj = null;
                                    obj = BLLClinicalObj.insertUpdateMiscHxTravelHx(dsSocialHx);
                                    if (obj.Data != null)
                                    {
                                        dsSocialHx.Merge(obj.Data);
                                        if (dsSocialHx.SocialHx_MiscHx_TravelHx.Rows.Count > 0 && (MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_TravelHx.TableName].Rows[0][dsSocialHx.SocialHx_MiscHx_TravelHx.TravelHxIdColumn.ColumnName]) > 0))
                                        {
                                            Int64 SocialMiscTravelHxId = MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_TravelHx.TableName].Rows[0][dsSocialHx.SocialHx_MiscHx_TravelHx.TravelHxIdColumn.ColumnName]);
                                            CurrentModel.TravelHxId = MDVUtility.ToStr(SocialMiscTravelHxId);
                                            RowMiscHxTravel[dsSocialHx.SocialHx_MiscHx_TravelHx.SoapTextColumn] = insertUpdateTravelText(CurrentModel);
                                            obj = BLLClinicalObj.insertUpdateMiscHxTravelHx(dsSocialHx);
                                            if (obj.Data != null)
                                            {
                                                // update SOCIAL HX SOAPTEXT
                                                BLLClinicalObj.updateSoapTextForSocialHX(SocialHxId.ToString());
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
                                                    Message = obj.Message
                                                };
                                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                                            }
                                        }
                                        else
                                        {
                                            // update SOCIAL HX SOAPTEXT
                                            BLLClinicalObj.updateSoapTextForSocialHX(SocialHxId.ToString());
                                            getCurrentSoapText(SocialHxId);
                                            var response = new
                                            {
                                                status = true,
                                                message = Common.AppPrivileges.Save_Message
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
                                else
                                {
                                    var response = new
                                    {
                                        status = false,
                                        Message = "Failed to save/update the record"
                                    };
                                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = "Failed to save/update the record"
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "Failed to save/update the record"
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Failed to save/update the record"
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Please select status first."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception)
            {
                var response = new
                {
                    status = false,
                    Message = "Failed to save/update the record"
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
        }
        public string insertUpdateOccupationHx(SocialHxMiscHxOccupationModel CurrentModel)
        {
            try
            {
                #region OccupationHx
                DSSocialHistory dsSocialHx = new DSSocialHistory();
                BLObject<DSSocialHistory> obj = null;
                if (!string.IsNullOrEmpty(CurrentModel.MiscChildStatus))
                {
                    DSSocialHistory.SocialHxRow dr;
                    obj = BLLClinicalObj.LoadSocialHx(MDVUtility.ToInt64(CurrentModel.PatientId), MDVUtility.ToInt64(CurrentModel.SocialHxId) > 0 ? MDVUtility.ToInt64(CurrentModel.SocialHxId) : 0);
                    if (obj.Data != null)
                    {
                        dsSocialHx = obj.Data;
                        if (dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows.Count > 0)
                        {
                            dr = dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0] as DSSocialHistory.SocialHxRow;
                            CurrentModel.SocialHxId = MDVUtility.ToStr(dr.SocialHxId);
                        }
                        else {
                            dr = dsSocialHx.SocialHx.NewSocialHxRow();
                            dr.SocialHxId = -1;
                        }

                        dr.PatientId = MDVUtility.ToInt64(CurrentModel.PatientId);
                        if (!string.IsNullOrEmpty(CurrentModel.SocialHxDate))
                            dr.SocialHxDate = MDVUtility.ToDateTime(CurrentModel.SocialHxDate);
                        else
                            dr[dsSocialHx.SocialHx.SocialHxDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(CurrentModel.SocialComments))
                            dr.Comments = "<div id='overallcomment' title='overallcomment'  name='overallcomment'><strong>Overall Comments:</strong> " + MDVUtility.ToStr(CurrentModel.SocialComments) + "</div>";
                        else
                            dr[dsSocialHx.SocialHx.CommentsColumn] = DBNull.Value;
                        dr.bUnremarkable = CurrentModel.SocialHxUnremarkable.ToLower() == "true" ? true : false;
                        dr.IsActive = true;
                        if (MDVUtility.ToInt64(CurrentModel.SocialHxId) <= 0)
                        {
                            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.CreatedOn = DateTime.Now;
                        }
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(CurrentModel.SocialHxId) && MDVUtility.ToInt64(CurrentModel.SocialHxId) > 0)
                            obj = BLLClinicalObj.UpdateSocialHx(dsSocialHx);
                        else
                        {
                            dsSocialHx.SocialHx.AddSocialHxRow(dr);
                            obj = BLLClinicalObj.InsertSocialHx(dsSocialHx);
                        }
                        if (obj.Data != null)
                        {
                            dsSocialHx = obj.Data;
                            Int64 SocialHxId = MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows[0][dsSocialHx.SocialHx.SocialHxIdColumn.ColumnName]);
                            string MiscHxResponse = insertUpdateMiscHx(MDVUtility.ToLong(SocialHxId));
                            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                            ser.MaxJsonLength = Int32.MaxValue;
                            var MiscHxResponseJSON = ser.Deserialize<dynamic>(MiscHxResponse);
                            Int64 MiscHxId = MDVUtility.ToStr(MiscHxResponseJSON["MiscHxId"]) != "" ? MDVUtility.ToInt64(MiscHxResponseJSON["MiscHxId"]) : 0;
                            if (MiscHxId > 0)
                            {
                                DateTime _datetime;
                                Int32 OccupationHxId = MDVUtility.ToInt32(CurrentModel.OccupationHxId);
                                OccupationHxId = OccupationHxId == 0 || OccupationHxId == -1 ? -1 : OccupationHxId;
                                obj = null;
                                obj = BLLClinicalObj.FillMiscHxOccupationHx(OccupationHxId, 0 , 15, 1);
                                DSSocialHistory.SocialHx_MiscHx_OccupationHxRow RowMiscHxOccupation = null;
                                if (obj.Data != null)
                                {
                                    dsSocialHx.Merge(obj.Data);
                                    if (dsSocialHx.SocialHx_MiscHx_OccupationHx.Rows.Count > 0)
                                        RowMiscHxOccupation = (DSSocialHistory.SocialHx_MiscHx_OccupationHxRow)dsSocialHx.SocialHx_MiscHx_OccupationHx.Rows[0];
                                    else
                                    {
                                        RowMiscHxOccupation = dsSocialHx.SocialHx_MiscHx_OccupationHx.NewSocialHx_MiscHx_OccupationHxRow();
                                        RowMiscHxOccupation.OccupationHxId = -1;
                                    }

                                    RowMiscHxOccupation.MiscHxId = MiscHxId;
                                    RowMiscHxOccupation.StatusId = MDVUtility.ToInt32(CurrentModel.MiscChildStatus);
                                    if (!string.IsNullOrEmpty(CurrentModel.OccupationHxDetails))
                                        RowMiscHxOccupation.OccupationDetail = CurrentModel.OccupationHxDetails;
                                    else
                                        RowMiscHxOccupation[dsSocialHx.SocialHx_MiscHx_OccupationHx.OccupationDetailColumn] = DBNull.Value;
                                    if (!string.IsNullOrEmpty(CurrentModel.OccupationComments))
                                        RowMiscHxOccupation.Comments = CurrentModel.OccupationComments;
                                    else
                                        RowMiscHxOccupation[dsSocialHx.SocialHx_MiscHx_OccupationHx.CommentsColumn] = DBNull.Value;
                                    if (!string.IsNullOrEmpty(CurrentModel.OccupationHxEndDate))
                                        if (DateTime.TryParse(CurrentModel.OccupationHxEndDate, out _datetime))
                                            RowMiscHxOccupation.EndDate = _datetime;
                                        else
                                            RowMiscHxOccupation[dsSocialHx.SocialHx_MiscHx_OccupationHx.EndDateColumn] = DBNull.Value;
                                    else
                                        RowMiscHxOccupation[dsSocialHx.SocialHx_MiscHx_OccupationHx.EndDateColumn] = DBNull.Value;

                                    if (!string.IsNullOrEmpty(CurrentModel.OccupationHxStartDate))
                                        if (DateTime.TryParse(CurrentModel.OccupationHxStartDate, out _datetime))
                                            RowMiscHxOccupation.StartDate = _datetime;
                                        else
                                            RowMiscHxOccupation[dsSocialHx.SocialHx_MiscHx_OccupationHx.StartDateColumn] = DBNull.Value;
                                    else
                                        RowMiscHxOccupation[dsSocialHx.SocialHx_MiscHx_OccupationHx.StartDateColumn] = DBNull.Value;
                                    if (!string.IsNullOrEmpty(CurrentModel.RadOccupation))
                                        RowMiscHxOccupation.IsPast = MDVUtility.ToBool(CurrentModel.RadOccupation);
                                    else
                                        RowMiscHxOccupation[dsSocialHx.SocialHx_MiscHx_OccupationHx.IsPastColumn] = DBNull.Value;

                                    RowMiscHxOccupation.IsActive = true;
                                    if ((MDVUtility.ToInt64(CurrentModel.OccupationHxId) <= 0))
                                    {
                                        RowMiscHxOccupation.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                        RowMiscHxOccupation.CreatedOn = DateTime.Now;
                                    }
                                    else
                                        RowMiscHxOccupation[dsSocialHx.SocialHx_MiscHx_OccupationHx.SoapTextColumn] = insertUpdateOccupationText(dsSocialHx, CurrentModel);
                                    RowMiscHxOccupation.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                    RowMiscHxOccupation.ModifiedOn = DateTime.Now;
                                    if (RowMiscHxOccupation.OccupationHxId == -1)
                                        dsSocialHx.SocialHx_MiscHx_OccupationHx.AddSocialHx_MiscHx_OccupationHxRow(RowMiscHxOccupation);
                                    obj = null;
                                    obj = BLLClinicalObj.insertUpdateMiscHxOccupationHx(dsSocialHx, CurrentModel.PatientId);
                                    if (obj.Data != null)
                                    {
                                        dsSocialHx.Merge(obj.Data);
                                        if (dsSocialHx.SocialHx_MiscHx_OccupationHx.Rows.Count > 0 && (MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][dsSocialHx.SocialHx_MiscHx_OccupationHx.OccupationHxIdColumn.ColumnName]) > 0))
                                        {
                                            Int64 SocialMiscOccupationHxId = MDVUtility.ToInt64(dsSocialHx.Tables[dsSocialHx.SocialHx_MiscHx_OccupationHx.TableName].Rows[0][dsSocialHx.SocialHx_MiscHx_OccupationHx.OccupationHxIdColumn.ColumnName]);
                                            CurrentModel.OccupationHxId = MDVUtility.ToStr(SocialMiscOccupationHxId);
                                            RowMiscHxOccupation[dsSocialHx.SocialHx_MiscHx_OccupationHx.SoapTextColumn] = insertUpdateOccupationText(dsSocialHx, CurrentModel);
                                            obj = BLLClinicalObj.insertUpdateMiscHxOccupationHx(dsSocialHx, CurrentModel.PatientId);
                                            if (obj.Data != null)
                                            {
                                                // update SOCIAL HX SOAPTEXT
                                                BLLClinicalObj.updateSoapTextForSocialHX(SocialHxId.ToString());
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
                                                    Message = obj.Message
                                                };
                                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                                            }
                                        }
                                        else
                                        {
                                            // update SOCIAL HX SOAPTEXT
                                            BLLClinicalObj.updateSoapTextForSocialHX(SocialHxId.ToString());
                                            var response = new
                                            {
                                                status = true,
                                                message = Common.AppPrivileges.Save_Message
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
                                else
                                {
                                    var response = new
                                    {
                                        status = false,
                                        Message = "Failed to save/update the record"
                                    };
                                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = "Failed to save/update the record"
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "Failed to save/update the record"
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Failed to save/update the record"
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Please select status first."
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
                    Message = "Failed to save/update the record"
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
        }

        public string DeleteSocialMiscOccupationHx(string occupationId, string SocialHxId)
        {
            try
            {
                if (!string.IsNullOrEmpty(occupationId) && !string.IsNullOrEmpty(SocialHxId))
                {
                    BLObject<string> obj = BLLClinicalObj.deleteMiscHxOccupationHx(occupationId);

                    if (obj.Data != null)
                    {
                        obj = BLLClinicalObj.updateSoapTextForSocialHX(SocialHxId.ToString());
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Delete_Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.Delete_Error_Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.Delete_Error_Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Delete_Error_Message
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
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
        }
        public string DeleteSocialMiscTravelHx(string TravelId,string SocialHxId)
        {
            try
            {
                if (!string.IsNullOrEmpty(TravelId) && !string.IsNullOrEmpty(SocialHxId))
                {
                    BLObject<string> obj = BLLClinicalObj.deleteMiscHxTravelHx(TravelId);
                    if (obj.Data != null)
                    {
                        obj =  BLLClinicalObj.updateSoapTextForSocialHX(SocialHxId.ToString());
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Delete_Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.Delete_Error_Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.Delete_Error_Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Delete_Error_Message
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
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
        }
        #endregion

        //End 08/01/2016 Syed Zia Code to  Save/Update MiscHx sub tabs
        /*
                   Change Implement BY: Muhammad Azhar Shahzad
                   Reason: These functions are used for Progress Note Soap Attachment, creation and detachment
                   Created Date: Dec 15, 2015
               */
        #region Soap Text for Social History
        internal string insertUpdateTobaccoSoapText(DSSocialHistory dsSocialHistory, SocialHxTobaccoModel modelObj)
        {
            //dsTobacco.SocialHx_Tobacco
            string TobaccoSoapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                //if (string.IsNullOrEmpty(modelObj.SmokingStatus_text))
                //{
                //    return string.Empty;
                //}
                sb.Append("<div id='socialTobacco_" + modelObj.TobaccoId + "' title='Tobacco'  name='Social Hx'><strong>Tobacco: </strong>");
                //Start 19/01/2016 Farooq Ahmad fixing soap text*
                sb.Append((string.IsNullOrEmpty(modelObj.SmokingStatus_text) ? "" : modelObj.SmokingStatus_text.TrimEnd() + ", ") + IsNullReturnSoapValue(modelObj.TobaccoType_text, modelObj.TobaccoType) + (string.IsNullOrEmpty(modelObj.TobaccoFrequencyDaily_text) ? "" : modelObj.TobaccoFrequencyDaily_text == "- Select -" ? "" : modelObj.TobaccoFrequencyDaily_text));
                //End 19/01/2016 Farooq Ahmad fixing soap text*
                //Start//06/01/2015//Ahmad Raza//Fixed bug# EMR-154
                sb.Append((string.IsNullOrEmpty(modelObj.TobaccoUsagePeriod) ? "" : " " + modelObj.TobaccoUsagePeriod_text) + (string.IsNullOrEmpty(modelObj.TobaccoCessationLength) ? "" : ", Patient has quit " + modelObj.TobaccoCessationLength + " " + modelObj.TobaccoCessationPeriod_text + " ago"));
                sb.Append((modelObj.TobaccoRecentlyQuit ? ", Patient recently quit" : "") + (modelObj.TobaccoWouldQuit ? ", Patient would quit" : "") + (string.IsNullOrEmpty(modelObj.TobaccoCounsellingPeriod) ? "" : ", Counselling: " + modelObj.TobaccoCounsellingPeriod_text) + (string.IsNullOrEmpty(modelObj.TobaccoCounsellingTopic) ? "" : " for " + modelObj.TobaccoCounsellingTopic_text) + (string.IsNullOrEmpty(modelObj.TobaccoComments) ? "" : ", Comments: " + modelObj.TobaccoComments) + "</div>");
                //End//06/01/2015//Ahmad Raza//Fixed bug# EMR-154
                ReplaceSoapTextBy(sb);
            }
            else
                if (dsSocialHistory.SocialHx_Tobacco != null && dsSocialHistory.SocialHx_Tobacco.Rows.Count > 0)
            {
                foreach (var item in dsSocialHistory.SocialHx_Tobacco)
                {
                    sb.Append("<div id='socialTobacco_" + item.TobaccoId + "' title='Tobacco'  name='Social Hx'><strong>Tobacco: </strong>");
                    sb.Append(IsNullReturnSoapValue(item.Status, item.Status) + IsNullReturnSoapValue(item.Type, item.Type) + IsNullReturnSoapValue(item.Frequency, item.Frequency));
                    sb.Append((string.IsNullOrEmpty(item.UsagePeriod) ? "" : ", " + item.UsagePeriod + ", "));
                    sb.Append((item.IsNull("CessationLength") ? "" : " Patient has quit " + item.CessationLength + " ago, "));
                    sb.Append((item.bRecentlyQuit ? " Patient recently quit, " : "") + (item.bWouldQuit ? " Patient would quit, " : "") + " Counselling " + item.CounsellingPeriod + " For " + item.CounsellingTopic + (string.IsNullOrEmpty(item.Comments) ? "." : ", Comments: " + item.Comments) + "</div>");
                }
            }

            else
            {
                return string.Empty;
            }
            return sb.ToString();
        }
        private string IsNullReturnSoapValue(String SoapValue, string CompareValue)
        {
            return (string.IsNullOrEmpty(CompareValue) ? "" : SoapValue.Trim() + ", ");
        }
        internal StringBuilder ReplaceSoapTextBy(StringBuilder sb)
        {
            sb.Replace(@", ,", ", ").Replace(",,", ",");
            string truncatedString = Regex.Replace(sb.ToString(), @"\s+", " ");
            sb.Clear();
            return sb.Append(truncatedString);
        }
        internal string insertUpdateAlcoholSoapText(DSSocialHistory dsSocialHistory, SocialHxAlcoholModel modelObj)
        {
            //dsTobacco.SocialHx_Tobacco
            string TobaccoSoapText = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (modelObj != null)
            {
                if (string.IsNullOrEmpty(modelObj.AlcoholStatus_text))
                {
                    return string.Empty;
                }
                sb.Append("<div id='socialAlcohol_" + modelObj.AlcoholId + "' title='Alcohol'  name='Social Hx'><strong>Alcohol: </strong>");
                //status needed to send
                sb.Append((string.IsNullOrEmpty(modelObj.AlcoholStatus_text) ? "" : modelObj.AlcoholStatus_text.TrimEnd()) + IsNullReturnSoapValue(", " + modelObj.AlcoholType_text, modelObj.AlcoholType) + (string.IsNullOrEmpty(modelObj.AlcoholFrequencyDays_text) ? "" : modelObj.AlcoholFrequencyDays_text == "- Select -" ? "" : modelObj.AlcoholFrequencyDays_text.TrimEnd()) + (string.IsNullOrEmpty(modelObj.AlcoholUsagePeriod) ? "" : " for " + modelObj.AlcoholUsagePeriod_text));
                sb.Append((string.IsNullOrEmpty(modelObj.AlcoholCessationLength) ? "" : ", Patient has quit " + modelObj.AlcoholCessationLength + " " + modelObj.AlcoholCessationPeriod_text + " ago") + (modelObj.AlcoholWouldQuit ? ", Patient would quit" : "") + (modelObj.AlcoholRecentlyQuit ? ", Patient recently quit" : "") + (modelObj.AlcoholNotReadyToQuit ? ", Patient not ready to quit " : ""));
                sb.Append((string.IsNullOrEmpty(modelObj.AlcoholCounsellingPeriod) ? "" : ", Counselling: " + modelObj.AlcoholCounsellingPeriod_text) + (string.IsNullOrEmpty(modelObj.AlcoholCounsellingTopic) ? "" : " for " + modelObj.AlcoholCounsellingTopic_text) + (string.IsNullOrEmpty(modelObj.AlcoholComments) ? "" : ", Comments: " + modelObj.AlcoholComments) + "</div>");
                ReplaceSoapTextBy(sb);
            }
            else
                if (dsSocialHistory.SocialHx_Alcohol != null && dsSocialHistory.SocialHx_Alcohol.Rows.Count > 0)
            {
                foreach (var item in dsSocialHistory.SocialHx_Alcohol)
                {
                    sb.Append("<div id='socialAlcohol_" + item.AlcoholId + "' title='Alcohol'  name='Social Hx'><strong>Alcohol: </strong>");
                    sb.Append(IsNullReturnSoapValue(item.Status, item.Status) + IsNullReturnSoapValue(item.Type, item.Type) + IsNullReturnSoapValue(item.Frequency, item.Frequency) + (string.IsNullOrEmpty(item.UsagePeriod) ? "" : " for " + item.UsagePeriod + ","));
                    sb.Append((item.IsNull("CessationLength") ? "" : " Patient has quit " + item.CessationLength + " ago, "));
                    sb.Append((item.bWouldQuit ? " Patient would quit, " : "") + (item.bRecentlyQuit ? " Patient recently quit, " : "") + (item.bNotReadyToQuit ? " Patient not ready to quit, " : ""));
                    sb.Append(" Counselling: " + item.CounsellingPeriod + " For " + item.CounsellingTopic + (string.IsNullOrEmpty(item.Comments) ? "." : ", Comments: " + item.Comments) + "</div>");
                }
            }

            else
            {
                return string.Empty;
            }
            return sb.ToString();
        }
        internal string insertUpdateDrugAbuseSoapText(DSSocialHistory dsSocialHistory, SocialHxDrugAbuseModel modelObj)
        {
            StringBuilder sb = new StringBuilder();
            if (modelObj != null)
            {
                if (string.IsNullOrEmpty(modelObj.DrugStatus_text))
                {
                    return string.Empty;
                }
                string drugs = modelObj.DrugType_text.Replace(",", ", ");

                string status = modelObj.DrugStatus_text.Trim() == "Denies Usage" ? modelObj.DrugStatus_text : ((string.IsNullOrEmpty(modelObj.DrugStatus_text) ? "" : modelObj.DrugStatus_text.TrimEnd()) + (string.IsNullOrEmpty(drugs) ? "" : (drugs == "- Select -" ? "" : " of " + drugs.TrimEnd() + " by ")) + (string.IsNullOrEmpty(modelObj.DrugRoute_text) ? "" : (modelObj.DrugRoute_text == "- Select -" ? "" : modelObj.DrugRoute_text + " for ")) +
                 (string.IsNullOrEmpty(modelObj.DrugFrequencyDay_text) ? "" : (modelObj.DrugFrequencyDay_text == "- Select -" ? "" : modelObj.DrugFrequencyDay_text)) + (string.IsNullOrEmpty(modelObj.DrugUsagePeriod_text) ? "" : (modelObj.DrugUsagePeriod_text == "- Select -" ? "" : " " + modelObj.DrugUsagePeriod_text)));



                sb.Append("<div id='socialDrugAbuse_" + modelObj.DrugAbuseId + "' title='Drug Abuse'  name='Social Hx'><strong>Drug Abuse: </strong>");
                sb.Append(status);
                sb.Append((string.IsNullOrEmpty(modelObj.DrugCessationLength) ? "" : ", Patient has quit " + modelObj.DrugCessationLength + " " + modelObj.DrugCessationPeriod_text + " ago") + (modelObj.DrugWouldQuit ? ", Patient would quit" : "") + (modelObj.DrugRecentlyQuit ? ", Patient recently quit" : "") + (string.IsNullOrEmpty(modelObj.DrugComments) ? "" : ", Comments: " + modelObj.DrugComments) + "</div>");

                //sb.Append("<div id='socialDrugAbuse_" + modelObj.DrugAbuseId + "' title='Drug Abuse'  name='Social Hx'><strong>Drug Abuse: </strong>");
                //sb.Append((string.IsNullOrEmpty(modelObj.DrugStatus_text) ? "" : modelObj.DrugStatus_text.TrimEnd()) + " of " + (string.IsNullOrEmpty(modelObj.DrugType_text) ? "" : (modelObj.DrugType_text == "- Select -" ? "" : " " + modelObj.DrugType_text.TrimEnd() + " by ")) + (string.IsNullOrEmpty(modelObj.DrugRoute_text) ? "" : (modelObj.DrugRoute_text == "- Select -" ? "" : modelObj.DrugRoute_text + " for ")) +
                //    (string.IsNullOrEmpty(modelObj.DrugFrequencyDay_text) ? "" : (modelObj.DrugFrequencyDay_text == "- Select -" ? "" : modelObj.DrugFrequencyDay_text)) + (string.IsNullOrEmpty(modelObj.DrugUsagePeriod_text) ? "" : (modelObj.DrugUsagePeriod_text == "- Select -" ? "" : " " + modelObj.DrugUsagePeriod_text)));
                //sb.Append((string.IsNullOrEmpty(modelObj.DrugCessationLength) ? "" : ", Patient has quit " + modelObj.DrugCessationLength + " " + modelObj.DrugCessationPeriod_text + " ago") + (modelObj.DrugWouldQuit ? ", Patient would quit" : "") + (modelObj.DrugRecentlyQuit ? ", Patient recently quit" : "") + (string.IsNullOrEmpty(modelObj.DrugComments) ? "" : ", Comments: " + modelObj.DrugComments) + "</div>");

                ReplaceSoapTextBy(sb);
            }
            else
                if (dsSocialHistory.SocialHx_DrugAbuse != null && dsSocialHistory.SocialHx_DrugAbuse.Rows.Count > 0)
            {
                foreach (var item in dsSocialHistory.SocialHx_DrugAbuse)
                {
                    sb.Append("<div id='socialDrugAbuse_" + item.DrugAbuseId + "' title='Drug Abuse'  name='Social Hx'><strong>Drug Abuse: </strong>");
                    sb.Append(IsNullReturnSoapValue(item.Status, item.Status) + " of " + IsNullReturnSoapValue(item.DrugAbuseId.ToString(), item.DrugAbuseId.ToString()) + " by " + IsNullReturnSoapValue(item.Route, item.Route) + " for " + IsNullReturnSoapValue(item.FrequencyDaily, item.FrequencyDaily) + " " + item.UsagePeriod + ", ");
                    sb.Append((item.IsNull("CessationLength") ? "" : " Patient has quit " + item.CessationLength + " ago, "));
                    sb.Append((item.bWouldQuit ? " Patient would quit" : "") + (item.bRecentlyQuit ? " Patient recently quit" : "") + (string.IsNullOrEmpty(item.Comments) ? "." : " Comments: " + item.Comments) + "</div>");
                }
            }

            else
            {
                return string.Empty;
            }
            return sb.ToString();
        }
        internal string insertUpdateSexualHxSoapText(DSSocialHistory dsSocialHistory, SocialHxSexualHxModel modelObj, string SocialComments)
        {
            //dsTobacco.SocialHx_Tobacco
            string TobaccoSoapText = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (modelObj != null)
            {
                if (string.IsNullOrEmpty(modelObj.SexualStatus_text))
                {
                    return string.Empty;
                }
                sb.Append("<div id='socialSexualHx_" + modelObj.SexualHxId + "' title='Sexual Hx'  name='Social Hx'><strong>Sexual Hx: </strong>");
                sb.Append((string.IsNullOrEmpty(modelObj.SexualStatus_text) ? "" : modelObj.SexualStatus_text.TrimEnd()) + IsNullReturnSoapValue(", Prefers " + modelObj.SexualPreferences_text, modelObj.SexualPreferences) + (string.IsNullOrEmpty(modelObj.SexualbUsingProtection) ? "" : "Using Protection: " + (modelObj.SexualbUsingProtection.ToLower() == "yes" ? "Yes, " : "No, ")));
                sb.Append(IsNullReturnSoapValue("Method: " + modelObj.SexualProtectionMethod_text, modelObj.SexualProtectionMethod) + IsNullReturnSoapValue("How often: " + modelObj.SexualProtectionPeriod_text, modelObj.SexualProtectionPeriod) + IsNullReturnSoapValue("Patient has complaints of " + modelObj.SexualComplaints_text, modelObj.SexualComplaints) + (string.IsNullOrEmpty(modelObj.SexualExposedToSTD) ? "" : " Exposed to STD: " + (modelObj.SexualExposedToSTD.ToLower() == "yes" ? "Yes, " : "No, ")));
                sb.Append(IsNullReturnSoapValue("STD: " + modelObj.SexualSTD_text, modelObj.SexualSTD) + (string.IsNullOrEmpty(modelObj.RadSexualPainWithIntercourse) ? "" : " Experiences pain with intercourse: " + (modelObj.RadSexualPainWithIntercourse.ToLower() == "true" ? "Yes" : "No")) + (string.IsNullOrEmpty(modelObj.RadSexualPregnant) ? "" : (modelObj.RadSexualPregnant.ToLower() == "true" ? " , Pregnant " + (string.IsNullOrEmpty(modelObj.SexualHxPregnancyDuration) ? "" : " since " + modelObj.SexualHxPregnancyDuration) : " , Not pregnant " + (string.IsNullOrEmpty(modelObj.SexualHxPregnancyDuration) ? "" : " since " + modelObj.SexualHxPregnancyDuration))) + (string.IsNullOrEmpty(modelObj.RadSexualAbusedSexually) ? "" : ", Abused sexually: " + (modelObj.RadSexualAbusedSexually.ToLower() == "true" ? "Yes" : "No")) + (string.IsNullOrEmpty(modelObj.SexualLMP) ? "" : ", Last Menstrual Period is " + modelObj.SexualLMP.ToString()) + (string.IsNullOrEmpty(modelObj.SexualComments) ? "" : ", Comments: " + modelObj.SexualComments) + "</div>");

                ReplaceSoapTextBy(sb);
            }
            else
                if (dsSocialHistory.SocialHx_SexualHx != null && dsSocialHistory.SocialHx_SexualHx.Rows.Count > 0)
            {
                foreach (var item in dsSocialHistory.SocialHx_SexualHx)
                {
                    sb.Append("<div id='socialSexualHx_" + item.SexualHxId + "' title='Sexual Hx' name='Social Hx'><strong>Sexual Hx: </strong>");
                    sb.Append(IsNullReturnSoapValue(item.Status, item.Status) + (string.IsNullOrEmpty(item.Preference) ? "" : " Prefers " + item.Preference + ", ") + " Using Protection: " + (item.bUSingProtection ? "Yes," : "No,"));
                    sb.Append(" Method: " + item.ProtectionMethod + ", How often: +item.howOften+, Patient has complaints of " + item.Complaint + ", Exposed to STD: " + (item.bExposedToSTD ? "Yes, " : "No, "));
                    sb.Append(" STD: + item.DrugName+, Experiences pain with intercourse: " + (item.bPainWithIntercourse ? "Yes, " : "No, ") + (string.IsNullOrEmpty(MDVUtility.ToStr(item.bPregnancyStatus)) ? "" : (MDVUtility.ToStr(item.bPregnancyStatus).ToLower() == "true" ? " , Pregnant " + (string.IsNullOrEmpty(item.PregnancyDuration) ? "" : " since " + item.PregnancyDuration) : " , Not pregnant " + (string.IsNullOrEmpty(item.PregnancyDuration) ? "" : " since " + item.PregnancyDuration))) + " Abused sexually: " + (item.bSexuallyAbused ? "Yes, " : "No, ") + (item.IsNull("LMP") ? "" : " Last Menstrual Period is " + item.LMP.ToString()) + (string.IsNullOrEmpty(item.Comments) ? "." : ", Comments: " + item.Comments) + "</div>");
                }
            }

            else
            {
                return string.Empty;
            }

            return sb.ToString();
        }

        //Creating soap text
        internal string insertUpdateOccupationText(DSSocialHistory dsSocialHistory, SocialHxMiscHxOccupationModel modelObj)
        {
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                if (string.IsNullOrEmpty(modelObj.MiscChildStatus))
                {
                    return string.Empty;
                }
                var EmploymentStatus = false;
                sb.Append("<div id='socialMiscHxOccupation_" + modelObj.OccupationHxId + "' title='Occupation Hx'  name='Social Hx'><strong>Occupation: </strong>");
                if (!string.IsNullOrEmpty(modelObj.RadOccupation))
                    EmploymentStatus = MDVUtility.ToBool(modelObj.RadOccupation);

                if (modelObj.MiscChildStatusText.Trim().ToLower() == "unemployed")
                    if (EmploymentStatus)
                        sb.Append("Patient was " + modelObj.MiscChildStatusText.Trim().ToLower() + " from " + modelObj.OccupationHxStartDate + " to " + modelObj.OccupationHxEndDate);
                    else
                        sb.Append("Patient is " + modelObj.MiscChildStatusText.Trim().ToLower() + " since " + modelObj.OccupationHxStartDate);
                else if(modelObj.MiscChildStatusText.Trim().ToLower() == "works at home")
                {
                    if (EmploymentStatus)
                        sb.Append("Patient worked at home from " + modelObj.OccupationHxStartDate + " to " + modelObj.OccupationHxEndDate);
                    else
                        sb.Append("Patient " + modelObj.MiscChildStatusText.Trim().ToLower() + " since " + modelObj.OccupationHxStartDate);
                }
                else if (modelObj.MiscChildStatusText.Trim().ToLower() == "works part time" )
                {
                    if (EmploymentStatus)
                        sb.Append("Patient worked part time at " + modelObj.OccupationHxDetails + " from " + modelObj.OccupationHxStartDate + " to " + modelObj.OccupationHxEndDate);
                    else
                        sb.Append("Patient " + modelObj.MiscChildStatusText.Trim().ToLower() + " at " + modelObj.OccupationHxDetails + " since " + modelObj.OccupationHxStartDate);
                }
                else if (modelObj.MiscChildStatusText.Trim().ToLower() == "works full time")
                {
                    if (EmploymentStatus)
                        sb.Append("Patient worked full time at " + modelObj.OccupationHxDetails + " from " + modelObj.OccupationHxStartDate + " to " + modelObj.OccupationHxEndDate);
                    else
                        sb.Append("Patient " + modelObj.MiscChildStatusText.Trim().ToLower() + " at " + modelObj.OccupationHxDetails + " since " + modelObj.OccupationHxStartDate);
                }
                else if (modelObj.MiscChildStatusText.Trim().ToLower() == "office worker")
                {
                    if (EmploymentStatus)
                        sb.Append("Patient worked as " + modelObj.MiscChildStatusText.Trim().ToLower() + " at " + modelObj.OccupationHxDetails + " from " + modelObj.OccupationHxStartDate + " to " + modelObj.OccupationHxEndDate);
                    else
                        sb.Append("Patient works as " + modelObj.MiscChildStatusText.Trim().ToLower() + " at " + modelObj.OccupationHxDetails + " since " + modelObj.OccupationHxStartDate);
                }
                else if (modelObj.MiscChildStatusText.Trim().ToLower() == "professional")
                {
                    if (EmploymentStatus)
                        sb.Append("Patient worked as " + modelObj.MiscChildStatusText.Trim().ToLower() + " at " + modelObj.OccupationHxDetails + " from " + modelObj.OccupationHxStartDate + " to " + modelObj.OccupationHxEndDate);
                    else
                        sb.Append("Patient works as " + modelObj.MiscChildStatusText.Trim().ToLower() + " at " + modelObj.OccupationHxDetails + " since " + modelObj.OccupationHxStartDate);
                }
                else if (modelObj.MiscChildStatusText.Trim().ToLower() == "manual worker")
                {
                    if (EmploymentStatus)
                        sb.Append("Patient worked as " + modelObj.MiscChildStatusText.Trim().ToLower() + " at " + modelObj.OccupationHxDetails + " from " + modelObj.OccupationHxStartDate + " to " + modelObj.OccupationHxEndDate);
                    else
                        sb.Append("Patient works as " + modelObj.MiscChildStatusText.Trim().ToLower() + " at " + modelObj.OccupationHxDetails + " since " + modelObj.OccupationHxStartDate);
                }
                else
                {
                    sb.Append(modelObj.MiscChildStatusText.Trim().ToLower());
                    if (!string.IsNullOrWhiteSpace(modelObj.OccupationHxDetails))
                    {
                        sb.Append(" at " + modelObj.OccupationHxDetails);
                    }
                    if (!string.IsNullOrWhiteSpace(modelObj.OccupationHxStartDate))
                    {
                        sb.Append(" from " + modelObj.OccupationHxStartDate + " to ");
                        if (!string.IsNullOrWhiteSpace(modelObj.OccupationHxEndDate))
                        {
                            sb.Append(modelObj.OccupationHxEndDate);
                        }
                        else
                        {
                            sb.Append("present");
                        }
                    }
                }
                sb.Append("<br>");
                //End 18/01/2016 Syed Zia , Soap text

                ReplaceSoapTextBy(sb);
            }
            return sb.ToString();
        }

        internal string insertUpdateTravelText(SocialHxMiscHxTravelHxModel modelObj)
        {
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                if (string.IsNullOrEmpty(modelObj.MiscChildStatus))
                    return string.Empty;
                sb.Append("<div id='socialMiscHxTravel_" + modelObj.TravelHxId + "' title='Travel Hx'  name='Social Hx'><strong>Travel: </strong>");
                if (modelObj.MiscChildStatusText.Trim().ToLower() == "does travel")
                    sb.Append("Patient travelled " + (string.IsNullOrEmpty(modelObj.TravelHxLocation) ? string.Empty : " to " + modelObj.TravelHxLocation) + (string.IsNullOrEmpty(modelObj.TravelHxFromDate) ? " from N/A " : " from " + modelObj.TravelHxFromDate) + (string.IsNullOrEmpty(modelObj.TravelHxToDate) ? " to N/A " : " to " + modelObj.TravelHxToDate));
                else if (modelObj.MiscChildStatusText.Trim().ToLower() == "does not travel")
                    sb.Append("Patient " + modelObj.MiscChildStatusText.Trim().ToLower() + ".");
                sb.Append("<br>");
                ReplaceSoapTextBy(sb);
            }
            return sb.ToString();
        }


        //Creating soap text
        internal string insertUpdateCaffeineIntakeHxText(DSSocialHistory dsSocialHistory, SocialHxMiscHxCaffeineIntakeModel modelObj)
        {
            string TobaccoSoapText = string.Empty;
            StringBuilder sb = new StringBuilder();
            // string frequency = "";
            if (modelObj != null)
            {
                if (string.IsNullOrEmpty(modelObj.MiscChildStatus))
                {
                    return string.Empty;
                }

                //Start 18/01/2016 Syed Zia , Soap text
                //Start || 15 July, 2016 || ZeeshanAK || Fix for EMR-1614
                sb.Append("<div id='socialMiscHxCaffeineIntake_" + modelObj.CaffeineIntakeHxId + "' title='Caffeine Intake'  name='Social Hx'><strong>Caffeine intake: </strong> ");
                if (modelObj.MiscChildStatusText.Trim() == "Does not use")
                {
                    sb.Append(" Patient does not intake caffeine.");
                }
                else
                {
                    //Start 20-10-2016 Edit By Humaira Yousaf Bug# QAC2-489
                    string statusText = modelObj.MiscChildStatusText.Trim();
                    if (string.Equals(statusText, "Abnormal"))
                    {
                        statusText = "Abnormally";
                    }
                    sb.Append(" Patients intakes caffeine " + statusText);
                    //End 20-10-2016 Edit By Humaira Yousaf Bug# QAC2-489
                }
                //End   || 15 July, 2016 || ZeeshanAK || Fix for EMR-1614
                sb.Append((string.IsNullOrEmpty(modelObj.CaffeineIntakFrequency.Trim()) ? "" : ", Frequency: " + modelObj.CaffeineIntakFrequency_text.Trim()));
                sb.Append((string.IsNullOrEmpty(modelObj.CaffeineComments.Trim()) ? "" : ", Comments: " + modelObj.CaffeineComments.Trim() + "<br/>"));
                //End 18/01/2016 Syed Zia , Soap text            
                ReplaceSoapTextBy(sb);
            }
            return sb.ToString();
        }

        // Author:  Farooq Ahmad
        // Created Date: 11/01/2016
        //OverView: This function will handle insert/update soaptext of sleephx
        internal string insertUpdateSleepHxSoapText(DSSocialHistory dsSocialHistory, SocialHxMiscHxSleepModel modelObj)
        {
            string TobaccoSoapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                //if (string.IsNullOrEmpty(modelObj.Status_text))
                //{
                //    return string.Empty;
                //}
                sb.Append(string.Format("<div id='sleepHx{0}' title='Sleep Hx'  name='Sleep Hx'>", modelObj.SleepHxId));

                sb.Append(string.Format("<strong>Sleep:</strong>"));

                //if (modelObj.MiscChildStatus != "6")
                //    sb.Append(" Patient sleeps");

                //start 18/01/2016 Syed Zia , Soap text
                if (!string.IsNullOrEmpty(modelObj.MiscChildStatus))
                {
                    if (modelObj.MiscChildStatus != "6")
                    {
                        sb.Append(" Patient sleeps ");
                    }
                    sb.Append(" " + modelObj.MiscChildStatusText.Trim() + ". ");

                }

                sb.Append((string.IsNullOrEmpty(modelObj.SleepHours) ? "" : "Sleeping hours are " + modelObj.SleepHours.Trim() + ". "));
                //start/ 1-14-2016 / Abid Ali for bug fixing 
                sb.Append(string.IsNullOrEmpty(modelObj.SleepComments) ? "" : "Comments: " + modelObj.SleepComments.Trim());
                //End 18/01/2016 Syed Zia , Soap text

                sb.Append("<br/>");
                sb.Append("</div>");

                ReplaceSoapTextBy(sb);
            }
            return sb.ToString();
        }

        // Author:  Farooq Ahmad
        // Created Date: 11/01/2016
        //OverView: This function will handle insert/update soaptext of exerciseshx
        internal string insertUpdateExercisesHxSoapText(DSSocialHistory dsSocialHistory, SocialHxMiscHxExercisesModel modelObj)
        {
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                //if (string.IsNullOrEmpty(modelObj.Status_text))
                //{
                //    return string.Empty;
                //}

                //start 18/01/2016 Syed Zia , Soap text
                sb.Append(string.Format("<div id='exercisesHx{0}' title='Exercises Hx'  name='Exercises Hx'>", modelObj.ExercisesHxId));
                sb.Append(string.Format("<strong>Exercises:</strong> Patient "));
                if (modelObj.MiscChildStatus != "6")
                    sb.Append("exercises ");

                sb.Append((string.IsNullOrEmpty(modelObj.MiscChildStatusText.Trim()) ? "" : modelObj.MiscChildStatusText.Trim()));
                sb.Append((string.IsNullOrEmpty(modelObj.ExercisesType.Trim()) ? "." : " with " + modelObj.ExercisesTypeText.Trim() + " intensity."));
                sb.Append((string.IsNullOrEmpty(modelObj.ExercisesDiet.Trim()) ? "" : " Patient diet is " + modelObj.ExercisesDietText.Trim() + "."));
                sb.Append(string.IsNullOrEmpty(modelObj.ExercisesComments) ? "" : " Comments: " + modelObj.ExercisesComments.Trim());
                //End 18/01/2016 Syed Zia , Soap text

                sb.Append("<br/>");
                sb.Append("</div>");
                ReplaceSoapTextBy(sb);

            }
            return sb.ToString();
        }

        // Author:  Farooq Ahmad
        // Created Date: 11/01/2016
        //OverView: This function will handle insert/update soaptext of housinghx
        internal string insertUpdateHousingHxSoapText(DSSocialHistory dsMiscHxHousing, SocialHxMiscHxHousingModel modelObj)
        {
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                //start 18/01/2016 Syed Zia , Soap text
                sb.Append(string.Format("<div id='housingHx{0}' title='Housing Hx'  name='Housing Hx'>", modelObj.HousingHxId));

                sb.Append("<strong>Housing:</strong>" + (string.IsNullOrEmpty(modelObj.MiscChildStatusText.Trim()) ? "" : " Housing Status: " + modelObj.MiscChildStatusText.Trim()));
                sb.Append((string.IsNullOrEmpty(modelObj.HousingPresent.Trim()) ? "" : ", Present: " + modelObj.HousingPresent.Trim()));
                sb.Append((string.IsNullOrEmpty(modelObj.HousingPast.Trim()) ? "" : ", Past: " + modelObj.HousingPast.Trim()));
                sb.Append(string.IsNullOrEmpty(modelObj.HousingComments) ? "." : ", Comments: " + modelObj.HousingComments.Trim());

                //End 18/01/2016 Syed Zia , Soap text

                sb.Append("<br/>");
                sb.Append("</div>");
                ReplaceSoapTextBy(sb);

            }
            return sb.ToString();
        }

        #endregion

        #region 'Attachment/Detachment of Soical History with Progress note'
        /// <summary>
        /// This Function will detach SocialHx from notes
        /// </summary>
        /// <param name="SocialHxId"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string detach_SocialHx_From_Notes(string SocialHxId, long NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(SocialHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachSocialHxFromNotes(SocialHxId, NotesId);
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
        /// This Function will attach SocialHx to notes
        /// </summary>
        /// <param name="SocialHxId"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string attach_SocialHx_With_Notes(string SocialHxId, long NotesId)
        {
            try
            {
                DSSocialHistory dsSocialHx = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(SocialHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<DSSocialHistory> obj = BLLClinicalObj.attachSocialHxWithNotes(SocialHxId, NotesId);
                    if (obj.Data != null)
                    {
                        dsSocialHx = obj.Data;
                        var response = new
                        {
                            status = true,
                            SocialHxTotalCount = dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows.Count,
                            SocialHxCount = dsSocialHx.Tables[dsSocialHx.SocialHx.TableName].Rows.Count,
                            SocialHxLoad_JSON = MDVUtility.JSON_DataTable(dsSocialHx.Tables[dsSocialHx.SocialHx.TableName]),
                            //    SocialHxHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsSocialHx.Tables[dsSocialHx.SocialHxHistory.TableName]),
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
        //end azhar changed Dec 15,2015

        // Date: 14/01/2016
        // Author: Muhammad Irfan
        // Overview: This function will update and save sorting order
        public string updateComponentOrderSorting(SocialHxModel model)
        {
            try
            {
                DSSocialHistory dsSocialHistory = null;
                BLObject<DSSocialHistory> obj;

                obj = BLLClinicalObj.loadMiscHx_Component(MDVUtility.ToInt64(0));

                dsSocialHistory = obj.Data;

                foreach (DSSocialHistory.SocialHx_MiscHx_ComponentRow dr in dsSocialHistory.Tables[dsSocialHistory.SocialHx_MiscHx_Component.TableName].Rows)
                {
                    dr.ComponentOrder = Array.IndexOf(model.MiscComponentSortedOrder.Split(','), dr.ComponentName) + 1;
                }
                obj = BLLClinicalObj.insertUpdateMiscHx_Component(dsSocialHistory);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        MiscCount = dsSocialHistory.Tables[dsSocialHistory.SocialHx_MiscHx_Component.TableName].Rows.Count,
                        MiscLoad_JSON = MDVUtility.JSON_DataTable(dsSocialHistory.Tables[dsSocialHistory.SocialHx_MiscHx_Component.TableName])
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
        public string SocialHxMiscHxTravelHxFill(SocialHxMiscHxTravelHxModel model)
        {
            try
            {
                DSSocialHistory ds = new DSSocialHistory();
                BLObject<DSSocialHistory> objMiscHxTravel = BLLClinicalObj.FillMiscHxTravelHx(MDVUtility.ToLong(model.TravelHxId), MDVUtility.ToLong(model.PatientId), model.RowsPerPage, model.PageNumber);

                if (objMiscHxTravel.Data != null)
                {
                    ds = objMiscHxTravel.Data;
                    if (ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MiscHxTravelHxCount = ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName].Rows.Count,
                            iTotalSocialHxMiscHxTravelHxDisplayRecords = ds.SocialHx_MiscHx_TravelHx.Rows[0][ds.SocialHx_MiscHx_TravelHx.RecordCountColumn.ColumnName],
                            SocialHxMiscHxTravelHx_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.SocialHx_MiscHx_TravelHx.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MiscHxTravelHxCount = 0,
                            iTotalSocialHxMiscHxTravelHxDisplayRecords = 0,
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
                        MiscHxTravelHxCount = 0,
                        iTotalSocialHxMiscHxTravelHxDisplayRecords = 0,
                        Message = Common.AppPrivileges.No_Record_Message
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
        public string SocialHxMiscHxOccupationHxFill(SocialHxMiscHxOccupationModel model)
        {
            try
            {
                DSSocialHistory ds = new DSSocialHistory();
                BLObject<DSSocialHistory> objMiscHxOccupation = BLLClinicalObj.FillMiscHxOccupationHx(MDVUtility.ToLong(model.OccupationHxId), MDVUtility.ToLong(model.PatientId), model.RowsPerPage, model.PageNumber);
                ds = objMiscHxOccupation.Data;
                if (ds != null)
                {
                    if (ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MiscHxOccupationHxCount = ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName].Rows.Count,
                            iTotalSocialHxMiscHxOccupationHxDisplayRecords = ds.SocialHx_MiscHx_OccupationHx.Rows[0][ds.SocialHx_MiscHx_OccupationHx.RecordCountColumn.ColumnName],
                            SocialHxMiscHxOccupationHx_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.SocialHx_MiscHx_OccupationHx.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MiscHxOccupationHxCount = 0,
                            iTotalSocialHxMiscHxOccupationHxDisplayRecords = 0,
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
                        MiscHxOccupationHxCount = 0,
                        iTotalSocialHxMiscHxOccupationHxDisplayRecords = 0,
                        Message = Common.AppPrivileges.No_Record_Message
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
    }
}