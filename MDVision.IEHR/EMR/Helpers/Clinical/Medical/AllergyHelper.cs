using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Text.RegularExpressions;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using MDVision.Model.Lookups;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Model.Clinical.Medical.Allergy;
using Newtonsoft.Json;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Medical
{
    public class AllergyHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public AllergyHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static AllergyHelper _instance = null;
        public static AllergyHelper Instance()
        {
            if (_instance == null)
                _instance = new AllergyHelper();
            return _instance;
        }

        //Start//01/12/2015//Ahmad Raza//Load Allergies for Grid
        /// <summary>
        /// This Function will Load Allergies
        /// </summary>
        /// <param name="AllergyModel"></param>
        /// <returns></returns>
        /// 



        public string loadAllergy_Obsolete(AllergyModel model, bool isViewed = false)
        {

            try
            {

                DSAllergies dsAllergy = null;
                BLObject<DSAllergies> obj;

                string isView = "";
                string isPrint = "0";
                if (isViewed == true)
                {
                    isView = "1";
                    isPrint = "0";
                }

                obj = BLLClinicalObj.loadAllergiesOP_Obsolete(MDVUtility.ToLong(model.PatientId), MDVUtility.ToLong(model.NoteId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), isView, isPrint);
                //obj = BLLClinicalObj.loadAllergies(MDVUtility.ToLong(model.AllergyId),Utility.ToLong(model.PatientId), MDVUtility.ToLong(model.NoteId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), isView, isPrint);

                dsAllergy = obj.Data;
                if (obj.Data != null)
                {
                    int allergiesTotalCount = 0;
                    //if (dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows.Count == 0)
                    //{
                    //    if (model.IsActive.Equals("1"))
                    //    {
                    //        obj = BLLClinicalObj.loadAllergiesOP(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToLong(model.NoteId), "0", "0", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    //        //obj = BLLClinicalObj.loadAllergies(MDVUtility.ToLong(model.AllergyId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToLong(model.NoteId), "0", "0", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    //    }
                    //    else
                    //    {
                    //        obj = BLLClinicalObj.loadAllergiesOP(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToLong(model.NoteId), "0", "1", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    //        //obj = BLLClinicalObj.loadAllergies(MDVUtility.ToLong(model.AllergyId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToLong(model.NoteId), "0", "1", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    //    }

                    //    if (obj.Data != null)
                    //    {
                    //        DSAllergies dsAllergyInActive = obj.Data;
                    //        allergiesTotalCount = dsAllergyInActive.Tables[dsAllergy.Allergy.TableName].Rows.Count;
                    //    }
                    //}
                    //else
                    //{
                    allergiesTotalCount = dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows.Count;
                    //  }
                    var response = new
                    {
                        status = true,
                        allergiesTotalCount = allergiesTotalCount,
                        allergiesCount = dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows.Count,
                        allergiesLoad_JSON = MDVUtility.JSON_DataTable(dsAllergy.Tables[dsAllergy.Allergy.TableName]),
                        allergyHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsAllergy.Tables[dsAllergy.AllergyHistory.TableName]),
                        allergyReview_JSON = MDVUtility.JSON_DataTable(dsAllergy.Tables[dsAllergy.AllergyReview.TableName]),
                        iTotalDisplayRecords = (dsAllergy.Allergy.Rows.Count > 0) ? dsAllergy.Allergy.Rows[0][dsAllergy.Allergy.RecordCountColumn.ColumnName] : 0,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        allergiesCount = 0,
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

        public string loadAllergy(AllergyModel model, bool isViewed = false)
        {

            try
            {

                string isView = "";
                string isPrint = "0";
                if (isViewed == true)
                {
                    isView = "1";
                    isPrint = "0";
                }

                Tuple < List<AllergyReaderModel>, List<AllergyHistoryModel>, List < AllergyReviewModel >> tuple = BLLClinicalObj.loadAllergiesOP(MDVUtility.ToLong(model.PatientId), MDVUtility.ToLong(model.NoteId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), isView, isPrint);
                //obj = BLLClinicalObj.loadAllergies(MDVUtility.ToLong(model.AllergyId),Utility.ToLong(model.PatientId), MDVUtility.ToLong(model.NoteId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), isView, isPrint);

                if (tuple != null)
                {
                    int allergiesTotalCount = 0;
                    //if (dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows.Count == 0)
                    //{
                    //    if (model.IsActive.Equals("1"))
                    //    {
                    //        obj = BLLClinicalObj.loadAllergiesOP(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToLong(model.NoteId), "0", "0", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    //        //obj = BLLClinicalObj.loadAllergies(MDVUtility.ToLong(model.AllergyId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToLong(model.NoteId), "0", "0", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    //    }
                    //    else
                    //    {
                    //        obj = BLLClinicalObj.loadAllergiesOP(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToLong(model.NoteId), "0", "1", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    //        //obj = BLLClinicalObj.loadAllergies(MDVUtility.ToLong(model.AllergyId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToLong(model.NoteId), "0", "1", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                    //    }

                    //    if (obj.Data != null)
                    //    {
                    //        DSAllergies dsAllergyInActive = obj.Data;
                    //        allergiesTotalCount = dsAllergyInActive.Tables[dsAllergy.Allergy.TableName].Rows.Count;
                    //    }
                    //}
                    //else
                    //{
                    allergiesTotalCount = tuple.Item1.Count;
                    //  }
                    var response = new
                    {
                        status = true,
                        allergiesTotalCount = allergiesTotalCount,
                        allergiesCount = tuple.Item1.Count,
                        allergiesLoad_JSON = JsonConvert.SerializeObject(tuple.Item1),
                        allergyHistoryLoad_JSON = JsonConvert.SerializeObject(tuple.Item2),
                        allergyReview_JSON = JsonConvert.SerializeObject(tuple.Item3),
                        iTotalDisplayRecords = (tuple.Item1.Count > 0) ? MDVUtility.ToInt64(tuple.Item1[0].RecordCount) : 0,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        allergiesCount = 0,
                        Message = "No record found."
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


        public string loadAllergiesReviewedBy_Obsolete(AllergyModel model)
        {

            try
            {

                DSAllergies dsAllergy = null;
                BLObject<DSAllergies> obj;
                obj = BLLClinicalObj.loadAllergiesReviewedBy_Obsolete(MDVUtility.ToLong(model.PatientId));
                dsAllergy = obj.Data;
                if (obj.Data != null)
                {
                    int AllergyReviewTotalCount = 0;
                    AllergyReviewTotalCount = dsAllergy.Tables[dsAllergy.AllergyReview.TableName].Rows.Count;
                    var response = new
                    {
                        status = true,
                        AllergyReviewTotalCount = AllergyReviewTotalCount,
                        allergyReview_JSON = MDVUtility.JSON_DataTable(dsAllergy.Tables[dsAllergy.AllergyReview.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        AllergyReviewTotalCount = 0,
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





        public string loadAllergyForCDS(string SearchAllergen)
        {

            try
            {

                DSAllergies dsAllergy = null;
                BLObject<DSAllergies> obj;
                obj = BLLClinicalObj.loadAllergiesForCDS(SearchAllergen);

                dsAllergy = obj.Data;
                if (obj.Data != null)
                {
                    int allergiesTotalCount = 0;
                    allergiesTotalCount = dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows.Count;

                    var response = new
                    {
                        status = true,
                        //allergiesTotalCount = allergiesTotalCount,
                        //  allergiesCount = dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows.Count,
                        allergiesLoad_JSON = MDVUtility.JSON_DataTable(dsAllergy.Tables[dsAllergy.Allergy.TableName]),
                        // iTotalDisplayRecords = (dsAllergy.Allergy.Rows.Count > 0) ? dsAllergy.Allergy.Rows[0][dsAllergy.Allergy.RecordCountColumn.ColumnName] : 0,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        // allergiesCount = 0,
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
        //End//01/12/2015//Ahmad Raza//Load Allergies for Grid

        //Start//01/12/2015//Ahmad Raza//Save Allergy
        /// <summary>
        /// This Function will Save Allergy
        /// </summary>
        /// <param name="AllergyModel"></param>
        /// <returns></returns>

        public string saveAllergy(List<AllergyModel> model, SharedVariable sharedVariable = null, string UserName = null)
        {
            try
            {
                if (UserName == null)
                {
                    UserName=MDVSession.Current.AppUserName;
                }
                DSAllergies dsAllergy = new DSAllergies();
                for (int i = 0; i < model.Count; i++)
                {
                    DSAllergies.AllergyRow dr = dsAllergy.Allergy.NewAllergyRow();
                    //dr.AllergyId = MDVUtility.ToInt64(model.AllergyId);

                    dr.PatientId = MDVUtility.ToInt64(model[i].PatientId);
                    dr.Allergen = string.IsNullOrEmpty(MDVUtility.ToStr(model[i].Allergen)) == true ? "No Known Allergies" : MDVUtility.ToStr(model[i].Allergen);
                    if (dr.Allergen == "nkda")
                    {
                        dr.Allergen = "No Known Allergies";
                    }
                    dr.Type = MDVUtility.ToStr(model[i].Type);
                    model[i].Reaction = model[i].Reaction.Replace(": ", ", ");
                    dr.Reaction = MDVUtility.ToStr(model[i].Reaction);
                    dr.Severity = MDVUtility.ToStr(model[i].Severity);
                    dr.CreatedBy = MDVUtility.DecryptFrom64(UserName);
                    if (!string.IsNullOrEmpty(model[i].OnSetDate))
                        dr.OnSetDate = MDVUtility.ToDateTime(model[i].OnSetDate);
                    else
                        dr[dsAllergy.Allergy.OnSetDateColumn] = DBNull.Value;

                    //dr.OnSetDate = DateTime.Now;
                    dr.Comments = MDVUtility.ToStr(model[i].Comments);

                    dr.IsDeleted = false;
                    dr.IsActive = true;
                    if (model[i].IsActive.Equals("<Active />"))
                    {
                        dr.IsActive = true;
                    }
                    else if (model[i].IsActive.Equals("<Inactive />"))
                    {
                        dr.IsActive = false;
                        dr.InActiveCheckBoxValue = "Inactive";
                    }
                    else if (model[i].IsActive.Equals("<Resolved />"))
                    {
                        dr.IsActive = false;
                        dr.InActiveCheckBoxValue = "Resolved";
                    }
                    else if (model[i].IsActive.Equals("<Deleted />"))
                    {
                        dr.IsActive = false;
                        dr.InActiveCheckBoxValue = "Deleted";
                        dr.IsDeleted = true;
                    }


                    //         dr.IsDeleted = model[i].IsDeleted;

                    /*        if (model[i].IsActive.Equals("<Deleted />"))
                            {
                                dr.IsDeleted = "y";
                            }
                            else
                            {
                                dr.IsDeleted = "n";
                            }
                    */
                    dr.LastModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //       dr.InActiveCheckBoxValue = model[i].InActiveChkBoxValue;
                    dr.InActiveReason = model[i].InActiveReason;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(UserName);
                    dr.RcopiaID = model[i].RcopiaID;
                    //dr.NoteId = MDVUtility.ToInt64(model.NoteId);
                    dr.RxnormID = model[i].RxnormID;
                    dr.RxnormIDType = model[i].RxnormIDType;
                    dsAllergy.Allergy.AddAllergyRow(dr);
                }
                #region Database Insertion

                BLObject<DSAllergies> obj = BLLClinicalObj.insertAllergies(dsAllergy, sharedVariable);


                dsAllergy = obj.Data;

                if (obj.Data != null)
                {
                    string SavedAllergyIds = "";
                    int i = 1;
                    foreach (DSAllergies.AllergyRow row in dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows)
                    {
                        if (dsAllergy.Allergy.Rows.Count == 1)
                        {
                            SavedAllergyIds = MDVUtility.ToStr(row[dsAllergy.Allergy.AllergyIdColumn]);
                        }
                        else
                        {
                            if (i == 1)
                            {
                                SavedAllergyIds = MDVUtility.ToStr(row[dsAllergy.Allergy.AllergyIdColumn]) + ",";
                            }
                            else
                            {
                                if (i == 2)
                                {
                                    SavedAllergyIds = SavedAllergyIds + MDVUtility.ToStr(row[dsAllergy.Allergy.AllergyIdColumn]);
                                }
                                else
                                {
                                    SavedAllergyIds = SavedAllergyIds + "," + MDVUtility.ToStr(row[dsAllergy.Allergy.AllergyIdColumn]);
                                }
                            }

                        }
                        i++;
                    }
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        AllergyId = dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows[0][dsAllergy.Allergy.AllergyIdColumn.ColumnName],
                        SavedAllergyIds = SavedAllergyIds
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
        //End//01/12/2015//Ahmad Raza//Save Allergy

        //Start//01/12/2015//Ahmad Raza//Update Allergies
        /// <summary>
        /// This Function will Update Allergies
        /// </summary>
        /// <param name="AllergyModel"></param>
        /// <returns></returns>
        /// 
        public class Data
        {
            public string status { set; get; }
            public string Message { set; get; }

        }

        /// Author: ZeeshanAK
        /// Purpose:  to load Allergies for Batch Patient List
        /// Date : April 06, 2016
        public string getAllAllergies(AllergyModel model)
        {
            try
            {

                DSAllergies dsAllergies = null;
                BLObject<DSAllergies> obj;

                obj = BLLClinicalObj.LookupAllergies(MDVUtility.ToInt32(model.PatientId), MDVUtility.ToInt32(model.AllergyId), MDVUtility.ToStr(model.Allergen));

                dsAllergies = obj.Data;
                var response = new
                {
                    status = true,
                    AllergiesCount = dsAllergies.Tables[dsAllergies.Allergy.TableName].Rows.Count,
                    AllergiesLoad_JSON = MDVUtility.JSON_DataTable(dsAllergies.Tables[dsAllergies.Allergy.TableName]),
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

        public string updateAllergy(AllergyModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.AllergyId) > 0)
                {

                    DSAllergies dsAllergy = new DSAllergies();
                    BLObject<DSAllergies> obj = BLLClinicalObj.loadAllergies_Obsolete(MDVUtility.ToInt64(model.AllergyId), MDVUtility.ToInt64(model.PatientId), 0);
                    dsAllergy = obj.Data;
                    foreach (DSAllergies.AllergyRow dr in dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows)
                    {
                        //if (!string.IsNullOrEmpty(model.AllergyId))
                        //    dr.AllergyId = MDVUtility.ToInt64(model.AllergyId);
                        //if (!string.IsNullOrEmpty(model.PatientId))
                        //    dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                        if (!string.IsNullOrEmpty(model.Allergen))
                            dr.Allergen = MDVUtility.ToStr(model.Allergen);
                        if (!string.IsNullOrEmpty(model.AllergyType))
                            dr.Type = MDVUtility.ToStr(model.AllergyType);
                        if (!string.IsNullOrEmpty(model.Reaction))
                            model.Reaction = model.Reaction.Replace(": ", ", ");
                        dr.Reaction = MDVUtility.ToStr(model.Reaction);
                        if (!string.IsNullOrEmpty(model.AllergySeverityType))
                            dr.Severity = MDVUtility.ToStr(model.AllergySeverityType);
                        if (!string.IsNullOrEmpty(model.OnSetDate))
                            dr.OnSetDate = MDVUtility.ToDateTime(model.OnSetDate);
                        else
                            dr[dsAllergy.Allergy.OnSetDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.Comments))
                            dr.Comments = MDVUtility.ToStr(model.Comments);
                        dr.IsActive = MDVUtility.ToStr(model.IsActive) == "1" ? true : false;
                        dr.LastModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dr.NoteId = null;
                        dr.InActiveCheckBoxValue = model.InActiveChkBoxValue;
                        dr.InActiveReason = model.InActiveReason;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //end newly added
                    }
                    #region Database Updation
                    if (dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows.Count > 0)
                    {
                        BLObject<DSAllergies> objUpdate = BLLClinicalObj.updateAllergies(dsAllergy);
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
                        message = "Allergy not found."
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
        //End//01/12/2015//Ahmad Raza//Update Allergies

        //Start//01/12/2015//Ahmad Raza//Update Allergy Comments
        /// <summary>
        /// This Function will update Allergy Comments
        /// </summary>
        /// <param name="AllergyModel"></param>
        /// <returns></returns>
        public string updateAllergyComments(AllergyModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.AllergyId) > 0)
                {

                    DSAllergies dsAllergy = new DSAllergies();
                    BLObject<DSAllergies> obj = BLLClinicalObj.loadAllergies_Obsolete(MDVUtility.ToInt64(model.AllergyId), MDVUtility.ToInt64(model.PatientId), 0, "1");
                    dsAllergy = obj.Data;
                    foreach (DSAllergies.AllergyRow dr in dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows)
                    {

                        if (!string.IsNullOrEmpty(model.Comments))
                            dr.Comments = MDVUtility.ToStr(model.Comments);
                        dr.IsActive = true;
                        dr.LastModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //end newly added
                    }
                    #region Database Updation
                    if (dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows.Count > 0)
                    {
                        BLObject<DSAllergies> objUpdate = BLLClinicalObj.updateAllergies(dsAllergy);
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
                        message = "Allergy not found."
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
        //End//01/12/2015//Ahmad Raza//Update Allergy Comments

        //Start//01/12/2015//Ahmad Raza//Delete Allergy
        /// <summary>
        /// This Function will delete Allergy
        /// </summary>
        /// <param name="AllergyModel"></param>
        /// <returns></returns>
        public string deleteAllergy(AllergyModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.AllergyId)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteAllergies(MDVUtility.ToStr(model.AllergyId));
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
        //End//01/12/2015//Ahmad Raza//Delete Allergy

        //Start//01/12/2015//Ahmad Raza//Active/Inactive Allergy
        /// <summary>
        /// This Function will Active/InActive Allergy
        /// </summary>
        /// <param name="AllergyModel"></param>
        /// <returns></returns>

        public string activeInActiveAllergy(AllergyModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.AllergyId) > 0)
                {

                    DSAllergies dsAllergy = new DSAllergies();
                    BLObject<DSAllergies> obj = BLLClinicalObj.loadAllergies_Obsolete(MDVUtility.ToInt64(model.AllergyId), MDVUtility.ToInt64(model.PatientId), 0);
                    dsAllergy = obj.Data;
                    foreach (DSAllergies.AllergyRow dr in dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.OnSetDate))
                            dr.OnSetDate = MDVUtility.ToDateTime(model.OnSetDate);
                        else
                        {
                            if (string.IsNullOrEmpty(dr.OnSetDate.ToString()))
                            {
                                dr[dsAllergy.Allergy.OnSetDateColumn] = DBNull.Value;
                            }

                        }

                        dr.InActiveCheckBoxValue = MDVUtility.ToStr(model.InActiveChkBoxValue);
                        dr.InActiveReason = MDVUtility.ToStr(model.InActiveReason);
                        dr.IsActive = MDVUtility.ToStr(model.IsActiveRecord) == "1" ? true : false;
                        dr.LastModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //end newly added
                    }
                    #region Database Updation
                    if (dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows.Count > 0)
                    {
                        BLObject<DSAllergies> objUpdate = BLLClinicalObj.updateAllergies(dsAllergy);
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
                        message = "Allergy not found."
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

        #region "Allergies with Notes"
        /// <summary>
        /// this function will get latest allergy for notes attachment
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public string getLatestAllergyByPatientId(Int64 PatientId, string AllergyIds, long userId, long entityId)
        {
            try
            {

                DSAllergies dsAllergy = null;
                BLObject<DSAllergies> obj;
                if (AllergyIds == "")
                {
                    obj = BLLClinicalObj.getLatestAllergyByPatientId(PatientId, "", userId, entityId);
                }
                else
                {
                    obj = BLLClinicalObj.getLatestAllergyByPatientId(PatientId, AllergyIds, userId, entityId);
                }

                dsAllergy = obj.Data;
                var response = new
                {
                    status = true,
                    AllergySoapCount = dsAllergy.Tables[dsAllergy.AllergySoap.TableName].Rows.Count,
                    AllergySoap_JSON = MDVUtility.JSON_DataTable(dsAllergy.Tables[dsAllergy.AllergySoap.TableName]),
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
        /// <summary>
        /// this function will retrive allergy information for Notes attachment
        /// </summary>
        /// <param name="AllergyId"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        internal string getAllergiesForSoap(string AllergyId)
        {
            try
            {

                DSAllergies dsAllergySoap = null;
                BLObject<DSAllergies> obj = BLLClinicalObj.loadAllergiesForSoap(AllergyId);
                dsAllergySoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsAllergySoap.Tables[dsAllergySoap.AllergySoap.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            AllergySoapCount = dsAllergySoap.Tables[dsAllergySoap.AllergySoap.TableName].Rows.Count,
                            AllergySoap_JSON = MDVUtility.JSON_DataTable(dsAllergySoap.Tables[dsAllergySoap.AllergySoap.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            AllergySoapCount = 0,
                            AllergySoap_JSON = MDVUtility.JSON_DataTable(dsAllergySoap.Tables[dsAllergySoap.AllergySoap.TableName]),
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
        /// This Function will detach Allergy from notes
        /// </summary>
        /// <param name="AllergyId"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string detach_Allergy_From_Notes(string AllergyId, long NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AllergyId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachAllergiesFromNotes(AllergyId, NotesId);
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
        /// This Function will attach Allergy to notes
        /// </summary>
        /// <param name="AllergyId"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string attach_Allergy_With_Notes(string AllergyId, long NotesId)
        {
            try
            {
                DSAllergies dsAllergy = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(AllergyId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<DSAllergies> obj = BLLClinicalObj.attachAllergiesWithNotes(AllergyId, NotesId);
                    if (obj.Data != null)
                    {
                        dsAllergy = obj.Data;
                        var response = new
                        {
                            status = true,
                            AllergyTotalCount = dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows.Count,
                            AllergyCount = dsAllergy.Tables[dsAllergy.Allergy.TableName].Rows.Count,
                            AllergyLoad_JSON = MDVUtility.JSON_DataTable(dsAllergy.Tables[dsAllergy.Allergy.TableName]),
                            AllergyHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsAllergy.Tables[dsAllergy.AllergyHistory.TableName]),
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

        /// Author: Azhar Shahzad
        /// Purpose:  to load Allergies for Clinical Reports
        /// Date : August 29, 2016
        internal string getAllAllergiesforReports()
        {
            try
            {

                List<AllergyLookupModel> modelList = null;
                BLObject<List<AllergyLookupModel>> obj;

                obj = BLLClinicalObj.getAllAllergiesforReports();

                modelList = obj.Data;
                //var ReactionList = modelList.GroupBy(x => x.Reaction).Select(y => y.First()).Distinct();
                var AllergiesList = modelList.GroupBy(x => x.Allergen).Select(y => y.First()).Distinct();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var response = new
                {
                    status = true,
                    AllergiesList = js.Serialize(AllergiesList),
                    AllergiesCount = modelList.Count
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

        /// Author: Humaira Yousaf
        /// Purpose:  to load reactions for Clinical Reports
        /// Date : 08-09-2016
        internal string getAllReactionsforReports()
        {
            try
            {

                List<ReactionLookupModel> modelList = null;
                BLObject<List<ReactionLookupModel>> obj;

                obj = BLLClinicalObj.getAllReactionsforReports();

                modelList = obj.Data;
                var ReactionList = modelList.GroupBy(x => x.Reaction).Select(y => y.First()).Distinct();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var response = new
                {
                    status = true,
                    ReactionList = js.Serialize(ReactionList),
                    ReactionCount = modelList.Count
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
    }
}