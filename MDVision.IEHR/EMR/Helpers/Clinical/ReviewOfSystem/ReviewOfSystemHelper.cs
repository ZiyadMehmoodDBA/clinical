using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.Model.Clinical.ReviewOfSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace MDVision.IEHR.EMR.Helpers.Clinical.ReviewOfSystem
{
    public class ReviewOfSystemHelper
    {
        private BLLReviewOfSystem BLLReviewofSytemObj = null;
        public ReviewOfSystemHelper()
        {
            BLLReviewofSytemObj = new BLLReviewOfSystem();
        }

        private static ReviewOfSystemHelper _instance = null;
        public static ReviewOfSystemHelper Instance()
        {
            if (_instance == null)
                _instance = new ReviewOfSystemHelper();
            return _instance;
        }


        #region " Review of Systems --> Systems Fill,Save and Update Methods "

        public string loadROSSystem(ROSCharacteristics model)
        {

            List<ROSSystems> objList_ROSSystems = new List<ROSSystems>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_ROSSystems = BLLReviewofSytemObj.loadROSSystem(model);
                if (objList_ROSSystems != null)
                {
                    if (objList_ROSSystems.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SystemCount = objList_ROSSystems.Count,
                            iTotalDisplayRecords = objList_ROSSystems[0].RecordCount,
                            SystemLoad_JSON = js.Serialize(objList_ROSSystems)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            SystemCount = 0,
                            iTotalDisplayRecords = 0,
                            SystemLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        SystemCount = 0,
                        iTotalDisplayRecords = 0,
                        SystemLoad_JSON = "[]",
                        Message = "",
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
            //try
            //{
            //    DSPhysicalExamECW dsPE = null;
            //    BLObject<DSPhysicalExamECW> obj;
            //    obj = BLLPhysicalExamECWObj.loadPhysicalExamSystem(MDVUtility.ToLong(model.PESystemId), model.IsActive, model.Name, MDVUtility.ToLong(model.PageNumber), MDVUtility.ToLong(model.RowsPerPage));
            //    if (obj.Data != null)
            //    {
            //        dsPE = obj.Data;
            //        if (dsPE.Tables[dsPE.PESystem.TableName].Rows.Count > 0)
            //        {
            //            var response = new
            //            {
            //                status = true,
            //                SystemCount = dsPE.Tables[dsPE.PESystem.TableName].Rows.Count,
            //                iTotalDisplayRecords = dsPE.Tables[dsPE.PESystem.TableName].Rows[0][dsPE.PESystem.RecordCountColumn.ColumnName],
            //                SystemLoad_JSON = MDVUtility.JSON_DataTable(dsPE.Tables[dsPE.PESystem.TableName]),
            //            };
            //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

            //        }
            //        else
            //        {
            //            var response = new
            //            {
            //                status = true,
            //                SystemCount = 0,
            //                iTotalDisplayRecords = 0,
            //                SystemLoad_JSON = "[]",
            //                Message = "Record not found."
            //            };
            //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //        }


            //    }
            //    else
            //    {
            //        var response = new
            //        {
            //            status = false,
            //            SystemCount = 0,
            //            iTotalDisplayRecords = 0,
            //            SystemLoad_JSON = "[]",
            //            Message = obj.Message
            //        };
            //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //    }

            //}
            //catch (Exception ex)
            //{
            //    var response = new
            //    {
            //        status = false,
            //        Message = MDVCustomException.HumanReadableMessage(ex.Message),
            //    };
            //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //}

        }

        public string fill_reviewofsystems_system(ROSCharacteristics model)
        {
            List<ROSSystems> objList_ROSSystems = new List<ROSSystems>();
            List<ROSCharacteristics> objList_ROSSystemCharatristics = new List<ROSCharacteristics>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                model.RowsPerPage = "25";
                model.PageNumber = "1";
                objList_ROSSystems = BLLReviewofSytemObj.loadROSSystem(model);
                if (objList_ROSSystems != null)
                {
                    if (objList_ROSSystems.Count > 0)
                    {
                        objList_ROSSystemCharatristics = BLLReviewofSytemObj.loadROSSystemCharatristics(model.ROSSystemId);
                        var response = new
                        {
                            status = true,
                            SystemCount = objList_ROSSystems.Count,
                            ObservationCount = objList_ROSSystemCharatristics.Count,
                            ObservationLoad_JSON = js.Serialize(objList_ROSSystemCharatristics),

                            SystemLoad_JSON = js.Serialize(objList_ROSSystems)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            SystemCount = 0,
                            ObservationCount = 0,
                            ObservationLoad_JSON = "[]",
                            SystemLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        SystemCount = 0,
                        Message = ""
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
            //try
            //{
            //    DSPhysicalExamECW dsPE = null;
            //    BLObject<DSPhysicalExamECW> obj;
            //    obj = BLLPhysicalExamECWObj.loadPhysicalExamSystem(MDVUtility.ToLong(model.ROSSystemId), "", "", 1, 25);
            //    if (obj.Data != null)
            //    {
            //        dsPE = obj.Data;
            //        obj = null;

            //        if (dsPE.Tables[dsPE.PESystem.TableName].Rows.Count > 0)
            //        {
            //            obj = BLLPhysicalExamECWObj.loadPhysicalExamSystemObservation(MDVUtility.ToLong(model.PESystemId));
            //            if (obj.Data != null)
            //                dsPE.Merge(obj.Data);

            //            var response = new
            //            {
            //                status = true,
            //                SystemCount = dsPE.Tables[dsPE.PESystem.TableName].Rows.Count,
            //                SystemLoad_JSON = MDVUtility.JSON_DataTable(dsPE.Tables[dsPE.PESystem.TableName]),
            //                ObservationCount = dsPE.Tables[dsPE.PESystemObservation.TableName].Rows.Count,
            //                ObservationLoad_JSON = MDVUtility.JSON_DataTable(dsPE.Tables[dsPE.PESystemObservation.TableName]),
            //            };
            //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

            //        }
            //        else
            //        {
            //            var response = new
            //            {
            //                status = true,
            //                SystemCount = 0,
            //                ObservationCount = 0,
            //                ObservationLoad_JSON = "[]",
            //                SystemLoad_JSON = "[]",
            //                Message = "Record not found."
            //            };
            //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //        }


            //    }
            //    else
            //    {
            //        var response = new
            //        {
            //            status = false,
            //            SystemCount = 0,
            //            Message = obj.Message
            //        };
            //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //    }

            //}
            //catch (Exception ex)
            //{
            //    var response = new
            //    {
            //        status = false,
            //        Message = MDVCustomException.HumanReadableMessage(ex.Message),
            //    };
            //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //}

        }

        //public string deletePhysicalExamSystem(long PESystemId)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(MDVUtility.ToStr(PESystemId)))
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        else
        //        {
        //            BLObject<string> obj = BLLReviewofSytemObj.deletePhysicalExamSystem(MDVUtility.ToStr(PESystemId));
        //            if (obj.Data == "")
        //            {
        //                var response = new
        //                {
        //                    status = true,
        //                    Message = Common.AppPrivileges.Delete_Message
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = obj.Data
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
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

        //public string updatePhysicalExamSystem(long PESystemId, string IsActive)
        //{
        //    try
        //    {
        //        if (PESystemId > 0)
        //        {

        //            DSPhysicalExamECW dsPE = new DSPhysicalExamECW();
        //            BLObject<DSPhysicalExamECW> obj = BLLPhysicalExamECWObj.loadPhysicalExamSystem(PESystemId, "", "", 1, 25);
        //            if (obj.Data != null)
        //            {
        //                dsPE = obj.Data;
        //                foreach (DSPhysicalExamECW.PESystemRow dr in dsPE.Tables[dsPE.PESystem.TableName].Rows)
        //                {
        //                    dr.IsActive = IsActive == "1" ? true : false;
        //                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                    dr.ModifiedOn = DateTime.Now;
        //                }
        //                BLObject<DSPhysicalExamECW> objUpdate = BLLPhysicalExamECWObj.updatePhysicalExamSystem(dsPE);
        //                if (objUpdate.Data != null)
        //                {
        //                    var response = new
        //                    {
        //                        status = true,
        //                        Message = Common.AppPrivileges.Update_Message
        //                    };
        //                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //                }
        //                else
        //                {
        //                    var response = new
        //                    {
        //                        status = false,
        //                        Message = objUpdate.Message
        //                    };
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //                }
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = obj.Message
        //                };
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //            }

        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = "Physical Exam System not found."
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

        //public string updatePhysicalExamSystem(PhysicalExamECWSystemModel model)
        //{
        //    try
        //    {
        //        if (MDVUtility.ToLong(model.PESystemId) > 0)
        //        {

        //            DSPhysicalExamECW dsPE = new DSPhysicalExamECW();
        //            BLObject<DSPhysicalExamECW> obj = BLLPhysicalExamECWObj.loadPhysicalExamSystem(MDVUtility.ToLong(model.PESystemId), "", "", 1, 25);
        //            if (obj.Data != null)
        //            {
        //                dsPE = obj.Data;
        //                foreach (DSPhysicalExamECW.PESystemRow dr in dsPE.Tables[dsPE.PESystem.TableName].Rows)
        //                {
        //                    if (!string.IsNullOrEmpty(model.IsActive))
        //                        dr.IsActive = model.IsActive.ToLower() == "true" ? true : false;

        //                    if (!string.IsNullOrEmpty(model.Name))
        //                        dr.Name = model.Name;

        //                    if (!string.IsNullOrEmpty(model.IsGlobal))
        //                        dr.IsGlobal = model.IsGlobal == "1" ? true : false;

        //                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                    dr.ModifiedOn = DateTime.Now;
        //                }

        //                update PE System
        //                BLObject<DSPhysicalExamECW> objUpdate = BLLPhysicalExamECWObj.updatePhysicalExamSystem(dsPE);
        //                if (objUpdate.Data != null)
        //                {
        //                    insert PE Observation if any
        //                    if (!string.IsNullOrEmpty(model.PEObservationIds))
        //                        {
        //                            PESystemModelResponse response_ = associatePEObservationforSystem(model);
        //                            if (response_.Status)
        //                            {
        //                                var response = new
        //                                {
        //                                    status = true,
        //                                    Message = Common.AppPrivileges.Update_Message,
        //                                    PEObservation_JSON = response_.Data,
        //                                };
        //                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //                            }
        //                            else
        //                            {
        //                                var response = new
        //                                {
        //                                    status = true,
        //                                    Message = Common.AppPrivileges.Update_Message + " " + response_.Message,
        //                                    PEObservation_JSON = "[]"
        //                                };
        //                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var response = new
        //                            {
        //                                status = true,
        //                                Message = Common.AppPrivileges.Update_Message,
        //                                PEObservation_JSON = "[]",
        //                            };
        //                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //                        }
        //                }
        //                else
        //                {
        //                    var response = new
        //                    {
        //                        status = false,
        //                        Message = objUpdate.Message
        //                    };
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //                }
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = obj.Message
        //                };
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //            }

        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = "Physical Exam System not found."
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

        //public PESystemModelResponse addPEObservationforSystem(PhysicalExamECWSystemModel model)
        //{
        //    DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
        //    DSPhysicalExamECW.PEObservationRow drObservation = dsPhysicalExam.PEObservation.NewPEObservationRow();
        //    PESystemModelResponse response = new PESystemModelResponse();
        //    response.Status = false;

        //    if (!string.IsNullOrEmpty(model.IsActive))
        //        drObservation.IsActive = MDVUtility.ToStr(model.IsActive) == "true" ? true : false;

        //    drObservation.PESystemId = MDVUtility.ToLong(model.PESystemId);
        //    drObservation.Name = model.Observations;
        //    drObservation.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //    drObservation.CreatedOn = DateTime.Now;
        //    drObservation.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //    drObservation.ModifiedOn = DateTime.Now;
        //    dsPhysicalExam.PEObservation.AddPEObservationRow(drObservation);

        //    BLObject<DSPhysicalExamECW> objInsertedObservation = BLLPhysicalExamECWObj.insertPhysicalExamObservation(dsPhysicalExam);
        //    if (objInsertedObservation.Data != null)
        //    {
        //        DSPhysicalExamECW dsSavedObservation = objInsertedObservation.Data;
        //        long PEObservationId = MDVUtility.ToInt64(dsSavedObservation.Tables[dsSavedObservation.PEObservation.TableName].Rows[0][dsSavedObservation.PEObservation.PEObservationIdColumn.ColumnName]);
        //        response.Status = true;
        //        response.Message = Common.AppPrivileges.Save_Message;
        //        response.Data = MDVUtility.JSON_DataTable(dsSavedObservation.Tables[dsSavedObservation.PEObservation.TableName]);

        //    }
        //    else
        //    {
        //        response.Status = false;
        //        response.Message = objInsertedObservation.Message;
        //        response.Data = "[]";
        //    }

        //    return response;
        //}

        //public PESystemModelResponse associatePEObservationforSystem(PhysicalExamECWSystemModel model)
        //{

        //    List<string> PEObservationIds = model.PEObservationIds.Split(',').ToList().Select(s => s).ToList();
        //    DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
        //    PESystemModelResponse response = new PESystemModelResponse();
        //    response.Status = false;

        //    foreach (var ObservationId in PEObservationIds)
        //    {
        //        if (!string.IsNullOrEmpty(ObservationId))
        //        {
        //            DSPhysicalExamECW.PESystemObservationRow dr = dsPhysicalExam.PESystemObservation.NewPESystemObservationRow();
        //            dr.PESystemId = MDVUtility.ToLong(model.PESystemId);
        //            dr.PEObservationId = MDVUtility.ToLong(ObservationId);
        //            dsPhysicalExam.PESystemObservation.AddPESystemObservationRow(dr);
        //        }
        //    }
        //    BLObject<DSPhysicalExamECW> objObservation = BLLPhysicalExamECWObj.insertPhysicalExamSystemObservation(dsPhysicalExam);
        //    if (objObservation.Data != null)
        //    {
        //        DSPhysicalExamECW dsAssociatedObservation = objObservation.Data;
        //        response.Status = true;
        //        response.Message = Common.AppPrivileges.Save_Message;
        //        response.Data = MDVUtility.JSON_DataTable(dsAssociatedObservation.Tables[dsAssociatedObservation.PESystemObservation.TableName]);
        //    }
        //    else
        //    {
        //        response.Status = false;
        //        response.Message = objObservation.Message;
        //        response.Data = "[]";
        //    }

        //    return response;
        //}

        public string insert_reviewofsystem_system(ROSCharacteristics model)
        {
            try
            {
                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<ROSCharacteristics> obj = BLLReviewofSytemObj.insert_reviewofsystem_system(model, true);
                if (obj.Count > 0)
                {
                    if (obj[0].IsError == "" || obj[0].IsError == null)
                    {

                        if (obj[0].ROSSystemCharacteristicsId != null || obj[0].ROSSystemCharacteristicsId == "")
                        {

                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                                ROSSystemId = obj[0].ROSSystemId,
                                PETemplateSystemId = obj[0].ROSSystemCharacteristicsId,
                                PEObservation_JSON = js.Serialize(obj),
                                PESystem_JSON = "[]"


                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                                ROSSystemId = obj[0].ROSSystemId,
                                PETemplateSystemId = obj[0].ROSSystemCharacteristicsId,
                                PEObservation_JSON = "[]",
                                PESystem_JSON = js.Serialize(obj)


                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj[0].IsError,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "",
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
            //try
            //{
            //    DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
            //    DSPhysicalExamECW.PESystemRow drSystem = dsPhysicalExam.PESystem.NewPESystemRow();

            //    if (!string.IsNullOrEmpty(model.IsActive))
            //        drSystem.IsActive = model.IsActive.ToLower() == "true" ? true : false;

            //    //drSystem.IsGlobal = true;
            //    //drSystem.Name = model.Name;
            //    //drSystem.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            //    //drSystem.CreatedOn = DateTime.Now;
            //    //drSystem.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            //    //drSystem.ModifiedOn = DateTime.Now;
            //    //drSystem.PETemplateID = model.ROSTemplateSystemId;
            //    //dsPhysicalExam.PESystem.AddPESystemRow(drSystem);

            //    // insert PE System
            //    //BLObject<DSPhysicalExamECW> objInsertedSystem = BLLPhysicalExamECWObj.insertPhysicalExamSystem(dsPhysicalExam);
            //    //if (objInsertedSystem.Data != null)
            //    //{
            //    //    DSPhysicalExamECW dsSavedSystem = objInsertedSystem.Data;
            //    //    model.PESystemId = MDVUtility.ToInt64(dsSavedSystem.Tables[dsSavedSystem.PESystem.TableName].Rows[0][dsSavedSystem.PESystem.PESystemIdColumn.ColumnName]).ToString();
            //    //    model.PETemplateSystemId = MDVUtility.ToInt64(dsSavedSystem.Tables[dsSavedSystem.PESystem.TableName].Rows[0][dsSavedSystem.PESystem.PETemplateSystemIdColumn.ColumnName]).ToString();

            //    //    // insert Associate PE Observations with System if there is
            //    //    if (!string.IsNullOrEmpty(model.PEObservationIds))
            //    //    {
            //    //        PESystemModelResponse response_ = associatePEObservationforSystem(model);
            //    //        if (response_.Status)
            //    //        {
            //    //            var response = new
            //    //            {
            //    //                status = true,
            //    //                Message = Common.AppPrivileges.Save_Message,
            //    //                PESystemId = model.PESystemId,
            //    //                PETemplateSystemId = model.PETemplateSystemId,
            //    //                PEObservation_JSON = response_.Data,
            //    //                PESystem_JSON = MDVUtility.JSON_DataTable(dsSavedSystem.Tables[dsSavedSystem.PESystem.TableName]),

            //    //            };
            //    //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //    //        }
            //    //        else
            //    //        {
            //    //            var response = new
            //    //            {
            //    //                status = true,
            //    //                PESystemId = model.PESystemId,
            //    //                PETemplateSystemId = model.PETemplateSystemId,
            //    //                PEObservation_JSON = "[]",
            //    //                PESystem_JSON = MDVUtility.JSON_DataTable(dsSavedSystem.Tables[dsSavedSystem.PESystem.TableName]),
            //    //                Message = Common.AppPrivileges.Save_Message + " " + response_.Message
            //    //            };
            //    //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //    //        }
            //    //    }
            //    //    else
            //    //    {
            //    //        var response = new
            //    //        {
            //    //            status = true,
            //    //            Message = Common.AppPrivileges.Save_Message,
            //    //            PESystemId = model.PESystemId,
            //    //            PETemplateSystemId = model.PETemplateSystemId,
            //    //            PEObservation_JSON = "[]",
            //    //            PESystem_JSON = MDVUtility.JSON_DataTable(dsSavedSystem.Tables[dsSavedSystem.PESystem.TableName]),
            //    //        };
            //    //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    var response = new
            //    //    {
            //    //        status = false,
            //    //        Message = "",// objInsertedSystem.Message.Contains("UQ_PESystem_Name") ? "System already exist" : objInsertedSystem.Message,
            //    //        PESystemId = 0,
            //    //        PETemplateSystemId = 0,
            //    //        PEObservation_JSON = "[]",
            //    //        PESystem_JSON = "[]",
            //    //    };
            //    //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

            //    //}
            //    return "";
            //}
            //catch (Exception ex)
            //{
            //    var response = new
            //    {
            //        status = false,
            //        Message = ex.Message
            //    };
            //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //}


        }

        public string update_reviewofsystem_system(ROSCharacteristics model)
        {
            try
            {
                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<ROSCharacteristics> obj = BLLReviewofSytemObj.insert_reviewofsystem_system(model, false);
                if (obj.Count > 0 || Common.AppPrivileges.Save_Message == "Successfully Saved")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        ROSSystemId = obj[0].ROSSystemId,
                        PETemplateSystemId = obj[0].ROSSystemCharacteristicsId,
                        PEObservation_JSON = js.Serialize(obj),
                        PESystem_JSON = "[]"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj[0].IsError,
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
        public string delete_reviewofsystem_system(string ROSSystemId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ROSSystemId)))
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
                    BLObject<string> obj = BLLReviewofSytemObj.delete_reviewofsystem_system(ROSSystemId);
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

        public string ActiveInActive_ROSSystem(string ROSSystemId, string IsActive)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ROSSystemId)))
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
                    BLObject<string> obj = BLLReviewofSytemObj.ActiveInActive_ROSSystem(ROSSystemId, IsActive);
                    if (obj.Data == "")
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

        public string InsertROSCharatriscticandUpadatesystem(ROSCharacteristics model)
        {
            try
            {
                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();


                BLObject<ROSCharacteristics> obj = BLLReviewofSytemObj.InsertROSCharatriscticandUpadatesystem(model);
                if (obj.Data.ROSCharacteristicsId != "" && obj.Data.ROSCharacteristicsId != null)
                {

                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        ROSCharacteristicsId = obj.Data.ROSCharacteristicsId,


                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Data.IsError,
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

        public string load_reviewofsystem_system_charatristics(ROSCharacteristics model)
        {

            List<ROSCharacteristics> objList_ROSSystemCharatristics = new List<ROSCharacteristics>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();


                if (model.ROSSystemId != "" || model.ROSSystemId != null)
                {
                    objList_ROSSystemCharatristics = BLLReviewofSytemObj.loadROSSystemCharatristics(model.ROSSystemId);
                    var response = new
                    {
                        status = true,

                        ObservationCount = objList_ROSSystemCharatristics.Count,
                        PEObservation_JSON = js.Serialize(objList_ROSSystemCharatristics),


                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,

                        ObservationCount = 0,
                        PEObservation_JSON = "[]",

                        Message = "Record not found."
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

        public string lookupROSSystems(ROSCharacteristics model)
        {
            List<ROSCharacteristics> objList_ROSSystems = new List<ROSCharacteristics>();
            try
            {
                model.RowsPerPage = "25";
                model.PageNumber = "1";
                model.IsActive = "1";
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_ROSSystems = BLLReviewofSytemObj.lookupROSSystems(model.IsActive);
                if (objList_ROSSystems != null)
                {
                    if (objList_ROSSystems.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PESystemsCount = objList_ROSSystems.Count,
                            PESystems_JSON = js.Serialize(objList_ROSSystems)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = 0,
                            iTotalDisplayRecords = 0,
                            ObservationLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ObservationCount = 0,
                        iTotalDisplayRecords = 0,
                        ObservationLoad_JSON = "[]",
                        Message = "",
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

        //public string lookupPhysicalExamSystem(PhysicalExamECWSystemModel model)
        //{
        //    try
        //    {
        //        DSPhysicalExamECWLookup dsPE = null;
        //        BLObject<DSPhysicalExamECWLookup> obj;
        //        obj = BLLPhysicalExamECWObj.lookupPESystem(model.IsActive);
        //        if (obj.Data != null)
        //        {
        //            dsPE = obj.Data;
        //            if (dsPE.Tables[dsPE.PESystemLookup.TableName].Rows.Count > 0)
        //            {
        //                var response = new
        //                {
        //                    status = true,
        //                    SystemCount = dsPE.Tables[dsPE.PESystemLookup.TableName].Rows.Count,
        //                    System_JSON = MDVUtility.JSON_DataTable(dsPE.Tables[dsPE.PESystemLookup.TableName]),
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = true,
        //                    SystemCount = 0,
        //                    System_JSON = "[]",
        //                    Message = "Record not found."
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }


        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                SystemCount = 0,
        //                System_JSON = "[]",
        //                Message = obj.Message
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
        #endregion

        #region "Review of Systems --> Chartristics Fill,Save and Update Methods"  
        public string InsertROSCharatrisctic(ROSCharacteristics model)
        {
            try
            {






                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();


                BLObject<string> obj = BLLReviewofSytemObj.InsertROSCharatrisctic(model);
                if (obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,


                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Charatristic with the same name already exists.",
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

        public string loadROSCharatrisctic(ROSCharacteristics model)
        {
            List<ROSCharacteristics> objList_ROSCharatristics = new List<ROSCharacteristics>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_ROSCharatristics = BLLReviewofSytemObj.loadROSCharatrisctic(model);
                if (objList_ROSCharatristics != null)
                {
                    if (objList_ROSCharatristics.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = objList_ROSCharatristics.Count,
                            iTotalDisplayRecords = objList_ROSCharatristics[0].RecordCount,
                            ObservationLoad_JSON = js.Serialize(objList_ROSCharatristics)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = 0,
                            iTotalDisplayRecords = 0,
                            ObservationLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ObservationCount = 0,
                        iTotalDisplayRecords = 0,
                        ObservationLoad_JSON = "[]",
                        Message = "",
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

        public string deleteROSCharatristics(string ROSCharacteristicsId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ROSCharacteristicsId)))
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
                    BLObject<string> obj = BLLReviewofSytemObj.deleteROSCharatristics(ROSCharacteristicsId);
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
        public string updateROSCharatristics(ROSCharacteristics model)
        {
            try
            {
                if (model.ROSCharacteristicsId != null && model.ROSCharacteristicsId != "")
                {

                    model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    model.ModifiedOn = DateTime.Now.ToString();
                    BLObject<string> obj = BLLReviewofSytemObj.updateROSCharatristics(model);
                    if (obj.Data == "" || obj.Data == "-1")
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
                            Message = "Charatristic with the same name already exists."
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }



                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Charatristic not found."
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

        public string fillROSCharatristics(ROSCharacteristics model)
        {
            List<ROSCharacteristics> objList_ROSCharatristics = new List<ROSCharacteristics>();
            try
            {
                model.RowsPerPage = "25";
                model.PageNumber = "1";

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_ROSCharatristics = BLLReviewofSytemObj.loadROSCharatrisctic(model);
                if (objList_ROSCharatristics != null)
                {
                    if (objList_ROSCharatristics.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = objList_ROSCharatristics.Count,
                            iTotalDisplayRecords = objList_ROSCharatristics[0].RecordCount,
                            ObservationLoad_JSON = js.Serialize(objList_ROSCharatristics)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = 0,
                            iTotalDisplayRecords = 0,
                            ObservationLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ObservationCount = 0,
                        iTotalDisplayRecords = 0,
                        ObservationLoad_JSON = "[]",
                        Message = "",
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

        public string lookupROSCharatristics(ROSCharacteristics model)
        {
            List<ROSCharacteristics> objList_ROSCharatristics = new List<ROSCharacteristics>();
            try
            {
                model.RowsPerPage = "25";
                model.PageNumber = "1";

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_ROSCharatristics = BLLReviewofSytemObj.lookupPEObservation(model.IsActive);
                if (objList_ROSCharatristics != null)
                {
                    if (objList_ROSCharatristics.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = objList_ROSCharatristics.Count,
                            iTotalDisplayRecords = objList_ROSCharatristics[0].RecordCount,
                            ObservationLoad_JSON = js.Serialize(objList_ROSCharatristics)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = 0,
                            iTotalDisplayRecords = 0,
                            ObservationLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ObservationCount = 0,
                        iTotalDisplayRecords = 0,
                        ObservationLoad_JSON = "[]",
                        Message = "",
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

            //try
            //{
            //    DSPhysicalExamECWLookup dsPE = null;
            //    BLObject<DSPhysicalExamECWLookup> obj;
            //    obj = BLLReviewofSytemObj.lookupPEObservation(model.IsActive);
            //    if (obj.Data != null)
            //    {
            //        dsPE = obj.Data;
            //        if (dsPE.Tables[dsPE.PEObservationLookup.TableName].Rows.Count > 0)
            //        {
            //            var response = new
            //            {
            //                status = true,
            //                ObservationCount = dsPE.Tables[dsPE.PEObservationLookup.TableName].Rows.Count,
            //                Observation_JSON = MDVUtility.JSON_DataTable(dsPE.Tables[dsPE.PEObservationLookup.TableName]),
            //            };
            //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

            //        }
            //        else
            //        {
            //            var response = new
            //            {
            //                status = true,
            //                ObservationCount = 0,
            //                Observation_JSON = "[]",
            //                Message = "Record not found."
            //            };
            //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //        }


            //    }
            //    else
            //    {
            //        var response = new
            //        {
            //            status = false,
            //            ObservationCount = 0,
            //            Observation_JSON = "[]",
            //            Message = obj.Message
            //        };
            //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //    }

            //}
            //catch (Exception ex)
            //{
            //    var response = new
            //    {
            //        status = false,
            //        Message = MDVCustomException.HumanReadableMessage(ex.Message),
            //    };
            //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //}

        }
        #endregion


        #region ROS Template
        public string loadROSLookUps()
        {
            List<ROSLookUps> objList_ROSLookUps = new List<ROSLookUps>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_ROSLookUps = BLLReviewofSytemObj.loadROSLookUps();

                if (objList_ROSLookUps != null)
                {
                    if (objList_ROSLookUps.Count > 0)
                    {
                        List<ROSCharateristicsDetail> listObj = new List<ROSCharateristicsDetail>();
                        foreach (ROSLookUps objROSLookUps in objList_ROSLookUps.GroupBy(m => m.LookupTypeName).Select(m => m.FirstOrDefault()))
                        {
                            ROSCharateristicsDetail objROSCharateristicsDetail = new ROSCharateristicsDetail();
                            objROSCharateristicsDetail.CharateristicsDetailName = objROSLookUps.LookupTypeName;
                            objROSCharateristicsDetail.CharatristicsDetail = new List<ROSLookUps>();
                            foreach (ROSLookUps objROSLookUpsInner in objList_ROSLookUps.Where(s => s.LookupTypeName == objROSLookUps.LookupTypeName))
                            {
                                objROSCharateristicsDetail.CharatristicsDetail.Add(objROSLookUpsInner);
                            }
                            listObj.Add(objROSCharateristicsDetail);
                        }
                        var response = new
                        {
                            status = true,
                            ROSLookupsCount = listObj.Count,
                            //iTotalDisplayRecords = objList_ROSLookUps[0].RecordCount,
                            ROSLookupsCount_JSON = js.Serialize(listObj)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = 0,
                            iTotalDisplayRecords = 0,
                            ObservationLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ObservationCount = 0,
                        iTotalDisplayRecords = 0,
                        ObservationLoad_JSON = "[]",
                        Message = "",
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

        DataTable GetProviderXMLTable(string providers)
        {
            DataTable ProviderTable = new DataTable() { TableName = "Providers" };
            ProviderTable.Columns.Add("Providerid", typeof(int));
            ProviderTable.Columns.Add("ShortName", typeof(string));

            string ProviderIds = providers;
            IList<string> providers_ = ProviderIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < providers_.Count; i++)
                ProviderTable.Rows.Add(providers_[i], "");

            return ProviderTable;
        }
        DataTable GetSpecialtyXMLTable(string Specialties)
        {
            DataTable SpecialtyTable = new DataTable() { TableName = "Specialties" };
            SpecialtyTable.Columns.Add("Specialtyid", typeof(int));
            SpecialtyTable.Columns.Add("ShortName", typeof(string));

            string SpecialtyIds = Specialties;
            IList<string> Specialties_ = SpecialtyIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < Specialties_.Count; i++)
                SpecialtyTable.Rows.Add(Specialties_[i], "");

            return SpecialtyTable;
        }
        //DataTable GetSystemObservationXMLTable(ROSTemplateModel modelROSTemplate)
        //{
        //    DataTable SystemObservationTable = new DataTable() { TableName = "PETempPart" };

        //    SystemObservationTable.Columns.Add("PETemplateId", typeof(string));
        //    SystemObservationTable.Columns.Add("PESystemId", typeof(int));
        //    SystemObservationTable.Columns.Add("PEObservationId", typeof(int));
        //    SystemObservationTable.Columns.Add("PESystemIsSelected", typeof(int));
        //    SystemObservationTable.Columns.Add("PEObservationIsSelected", typeof(int));
        //    SystemObservationTable.Columns.Add("IsSystemDeleted", typeof(int));
        //    SystemObservationTable.Columns.Add("IsObservationDeleted", typeof(int));

        //    if (MDVUtility.ToInt64(modelROSTemplate.TemplateId) != -1)
        //    {
        //        for (int i = 0; i < modelROSTemplate.SystemObservationData.Count; i++)
        //        {
        //            if (!string.IsNullOrEmpty(modelROSTemplate.SystemObservationData[i].ObservationId))
        //                if (MDVUtility.ToInt64(modelROSTemplate.SystemObservationData[i].ObservationId) != -1)
        //                    if (MDVUtility.ToInt64(modelROSTemplate.SystemObservationData[i].PESystemId) != -1)
        //                        SystemObservationTable.Rows.Add
        //                            (
        //                                string.IsNullOrEmpty(modelROSTemplate.SystemObservationData[i].TemplateId) ? "-1" : modelROSTemplate.SystemObservationData[i].TemplateId,
        //                                modelROSTemplate.SystemObservationData[i].PESystemId,
        //                                modelROSTemplate.SystemObservationData[i].ObservationId,
        //                                MDVUtility.ToBool(modelROSTemplate.SystemObservationData[i].IsSystemChecked) == true ? 1 : 0,
        //                                MDVUtility.ToBool(modelROSTemplate.SystemObservationData[i].IsChecked) == true ? 1 : 0,
        //                                modelROSTemplate.SystemObservationData[i].IsSystemDeleted,
        //                                modelROSTemplate.SystemObservationData[i].IsObservationDeleted
        //                            );
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < modelROSTemplate.SystemObservationData.Count; i++)
        //        {
        //            if (!string.IsNullOrEmpty(modelROSTemplate.SystemObservationData[i].ObservationId))
        //            {
        //                SystemObservationTable.Rows.Add
        //                    (
        //                        string.IsNullOrEmpty(modelROSTemplate.SystemObservationData[i].TemplateId) ? "-1" : modelROSTemplate.SystemObservationData[i].TemplateId,
        //                        modelROSTemplate.SystemObservationData[i].PESystemId,
        //                        modelROSTemplate.SystemObservationData[i].ObservationId,
        //                        MDVUtility.ToBool(modelROSTemplate.SystemObservationData[i].IsSystemChecked) == true ? 1 : 0,
        //                        MDVUtility.ToBool(modelROSTemplate.SystemObservationData[i].IsChecked) == true ? 1 : 0,
        //                        modelROSTemplate.SystemObservationData[i].IsSystemDeleted,
        //                        modelROSTemplate.SystemObservationData[i].IsObservationDeleted
        //                    );
        //            }
        //        }
        //    }
        //    return SystemObservationTable;
        //}
        public string insert_reviewofsystem_template(ROSTemplateModel model)
        {
            try
            {

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ROSTemplateModel));
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, model);
                string myxml = textWriter.ToString();

                string providerXMLString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetProviderXMLTable(model.ProviderIds).WriteXml(writer);
                    providerXMLString = writer.ToString();
                }

                string specialtyXMLString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetSpecialtyXMLTable(model.SpecialtyIds).WriteXml(writer);
                    specialtyXMLString = writer.ToString();
                }


                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                model.ProviderXML = providerXMLString;
                model.SpecialtyXML = specialtyXMLString;

                model.SystemCharacteristicsXML = myxml;

                BLObject<string> obj = BLLReviewofSytemObj.Insert_reviewofsystem_Template(model);
                if (obj.Data != null && obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message,
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
        public string update_reviewofsystem_template(ROSTemplateModel model)
        {
            try
            {

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ROSTemplateModel));
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, model);
                string myxml = textWriter.ToString();

                string providerXMLString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetProviderXMLTable(model.ProviderIds).WriteXml(writer);
                    providerXMLString = writer.ToString();
                }

                string specialtyXMLString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetSpecialtyXMLTable(model.SpecialtyIds).WriteXml(writer);
                    specialtyXMLString = writer.ToString();
                }


                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                model.ProviderXML = providerXMLString;
                model.SpecialtyXML = specialtyXMLString;

                model.SystemCharacteristicsXML = myxml;

                BLObject<string> obj = BLLReviewofSytemObj.update_reviewofsystem_Template(model);
                if (obj.Data != "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        ROSCharacteristicsId = obj.Data,


                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Template with the same name already exists.",
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
        public string delete_reviewofsystem_template(string ROSTemplateId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ROSTemplateId)))
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
                    BLObject<string> obj = BLLReviewofSytemObj.delete_reviewofsystem_template(ROSTemplateId);
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
                            Message = MDVCustomException.HumanReadableMessage(obj.Data)
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
        public string select_reviewofsystem_template(string ROSTemplateId, string NotesId = null)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ROSTemplateId)))
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
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    BLObject<DataSet> Obj = BLLReviewofSytemObj.select_reviewofsystem_template(ROSTemplateId, NotesId);




                    if (Obj.Data.Tables.Count > 0)
                    {
                        DataView view = new DataView(Obj.Data.Tables[0]);
                        DataView viewdetail = new DataView(Obj.Data.Tables[1]);

                        DataTable ROSTemp = view.ToTable(true, "ROSTemplateId", "TemplateName", "SpecialtyIds", "ProviderIds", "CreatedOn", "CreatedBy", "ModifiedBy",
                            "ModifiedOn", "TemplateComments", "BodyPartId");
                        DataTable ROSTempSystem = view.ToTable(true, "ROSTemplateId", "TemplateName", "ROSSystemId", "SystemName", "IsSelectedSystem", "ROSTemplateSystemId",
                            "NormalComments", "IsNormal", "TempSysIsSelected", "IsSystemSelectAll");
                        DataTable ROSSysCharatristics = view.ToTable(true, "ROSSystemId", "SystemName", "ROSCharacteristicsId", "CharacteristicsName", "TempSysCharIsSelected",
                            "TempSysCharComments", "isPositive");
                        DataTable ROSSysCharatristicsDetail = viewdetail.ToTable(true, "ROSSystemId", "ROSCharacteristicsId", "LookUpId", "LookupTypeName", "Value","LookupName");

                        var response = new
                        {
                            status = true,
                            PETemplate_JSON = js.Serialize(MDVUtility.JSON_DataTable(ROSTemp)),
                            PETemplateCount = ROSTemp.Rows.Count,
                            PETemplateSystems_JSON = js.Serialize(MDVUtility.JSON_DataTable(ROSTempSystem)),
                            PETemplateSystemsCount = ROSTempSystem.Rows.Count,
                            PESysObservations_JSON = js.Serialize(MDVUtility.JSON_DataTable(ROSSysCharatristics)),
                            PESysObservationsCount = ROSSysCharatristics.Rows.Count,
                            ROSSysCharaDetail = js.Serialize(MDVUtility.JSON_DataTable(ROSSysCharatristicsDetail)),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PETemplateSystems_JSON = "[]",
                            PETemplateSystemsCount = 0,
                            Message = MDVUtility.ToStr(AppPrivileges.No_Record_Message)
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
                    PETemplateSystemsCount = 0,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }
        public string activeinactive_reviewofsystem_template(string ROSTemplateId, string IsActive)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ROSTemplateId)))
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
                    BLObject<string> obj = BLLReviewofSytemObj.activeinactive_reviewofsystem_template(ROSTemplateId, IsActive, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                    if (obj.Data == "")
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
        public string loadROSRevampTemplates(long templateId, long entityId, int? IsActive = null)
        {
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
            string currentPhysTempId = string.Empty;
            BLObject<DSPhysicalExamECW> obj = null;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            obj = new BLLReviewOfSystem().loadROSRevampTemplates(templateId, entityId, IsActive);
            dsPhysicalExam = obj.Data;

            var response = new
            {
                status = true,
                PhysicalExamTemplate = js.Serialize(MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PETemplate.TableName])),
                PhysicalExamTemplateCount = dsPhysicalExam.Tables[dsPhysicalExam.PETemplate.TableName].Rows.Count,
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }
        #region ROSTemplateRevmap Note
        public string Insert_reviewofsystem_Template_Note(ROSTemplateModel model)
        {
            // Creating Soap Text
            //string soapText = string.Empty;
            //soapText = createROSsoapText(model);
            //model.SOAPText = soapText;
            try
            {

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ROSTemplateModel));
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, model);
                string myxml = textWriter.ToString();

                string providerXMLString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetProviderXMLTable(model.ProviderIds).WriteXml(writer);
                    providerXMLString = writer.ToString();
                }

                string specialtyXMLString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetSpecialtyXMLTable(model.SpecialtyIds).WriteXml(writer);
                    specialtyXMLString = writer.ToString();
                }


                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                model.ProviderXML = providerXMLString;
                model.SpecialtyXML = specialtyXMLString;

                model.SystemCharacteristicsXML = myxml;

                BLObject<string> obj = BLLReviewofSytemObj.Insert_reviewofsystem_Template_Note(model);
                if (obj.Data != "")
                {
                    string ROSTemplateId = obj.Data.ToString();
                    string ROSSoapText = string.Empty;
                    BLObject<DataSet> Obj = BLLReviewofSytemObj.select_reviewofsystem_template_note(model.TemplateId, model.NoteId);
                    if (obj.Data != null)
                    {
                        BLLClinical bllclinical = new BLLClinical();
                        ROSSoapText = bllclinical.createROSsoapText(Obj.Data, long.Parse(model.NoteId), long.Parse(model.TemplateId));
                    }
               
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        ROSTemplateId = ROSTemplateId,
                        ROSSOAPText= ROSSoapText,


                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Template with the same name already exists.",
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
        public string load_ROSRevampTemplates_Note(ROSTemplateModel Model)
        {
            Model.UserId = MDVSession.Current.AppUserId.ToString();
            Model.EntityId = MDVSession.Current.EntityId.ToString();
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
            string currentPhysTempId = string.Empty;
            BLObject<DSPhysicalExamECW> obj = null;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            obj = new BLLReviewOfSystem().load_ROSRevampTemplates_Note(Model);
            dsPhysicalExam = obj.Data;

            var response = new
            {
                status = true,
                ROSTemplate = js.Serialize(MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PETemplate.TableName])),
                PhysicalExamTemplateCount = dsPhysicalExam.Tables[dsPhysicalExam.PETemplate.TableName].Rows.Count,
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }
        public string toggle_Characteristics_note(string NotesID,string TemplateId, string ROSSystemId, string ROSCharacteristicsId, string IsPositive)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ROSSystemId)))
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
                    BLObject<string> obj = BLLReviewofSytemObj.toggle_Characteristics_note(NotesID, TemplateId, ROSSystemId, ROSCharacteristicsId, IsPositive);
                    if (obj.Data == "")
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
        public string select_reviewofsystem_template_note(string ROSTemplateId, string NotesId = null, string SystemId = null)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ROSTemplateId)))
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
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    BLObject<DataSet> Obj = BLLReviewofSytemObj.select_reviewofsystem_template_note(ROSTemplateId, NotesId, SystemId);




                    if (Obj.Data.Tables.Count > 0)
                    {
                        DataView view = new DataView(Obj.Data.Tables[0]);
                        DataView viewdetail = new DataView(Obj.Data.Tables[1]);

                        DataTable ROSTemp = view.ToTable(true, "ROSTemplateId", "TemplateName","IsNormalAll", "SpecialtyIds", "ProviderIds", "CreatedOn", "CreatedBy", "ModifiedBy",
                            "ModifiedOn", "TemplateComments");
                        DataTable ROSTempSystem = view.ToTable(true, "ROSTemplateId", "TemplateName", "ROSSystemId", "SystemName", 
                            "NormalComments", "IsNormal", "IsSystemSelectAll");
                        DataTable ROSSysCharatristics = view.ToTable(true, "ROSSystemId", "SystemName", "ROSCharacteristicsId", "CharacteristicsName",
                            "TempSysCharComments", "isPositive");
                        DataTable ROSSysCharatristicsDetail = viewdetail.ToTable(true, "ROSSystemId", "ROSCharacteristicsId", "LookUpId", "LookupTypeName", "Value", "LookupName");

                        var response = new
                        {
                            status = true,
                            PETemplate_JSON = js.Serialize(MDVUtility.JSON_DataTable(ROSTemp)),
                            PETemplateCount = ROSTemp.Rows.Count,
                            PETemplateSystems_JSON = js.Serialize(MDVUtility.JSON_DataTable(ROSTempSystem)),
                            PETemplateSystemsCount = ROSTempSystem.Rows.Count,
                            PESysObservations_JSON = js.Serialize(MDVUtility.JSON_DataTable(ROSSysCharatristics)),
                            PESysObservationsCount = ROSSysCharatristics.Rows.Count,
                            ROSSysCharaDetail = js.Serialize(MDVUtility.JSON_DataTable(ROSSysCharatristicsDetail)),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PETemplateSystems_JSON = "[]",
                            PETemplateSystemsCount = 0,
                            Message = MDVUtility.ToStr(AppPrivileges.No_Record_Message)
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
                    PETemplateSystemsCount = 0,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }
        public string delete_reviewofsystem_template_note(string ROSTemplateId,string NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLReviewofSytemObj.delete_reviewofsystem_template_note(ROSTemplateId,NotesId);
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
        //public string createROSsoapText(ROSTemplateModel Model)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (Model != null)
        //    {
        //        if (Model.TemplateId!="") // setting template info...
        //        {

        //            string overAllComments = string.Empty;
                    
        //                string GlobalNormalDescription = Model.Comments;
        //                overAllComments = Model.Comments;
        //                if (Model.IsNormalAll != "")
        //                {
        //                    if (Model.IsNormalAll == "true")
        //                    {
        //                        if (!string.IsNullOrEmpty(GlobalNormalDescription))
        //                        {
        //                            sb.Append(GlobalNormalDescription + "<br>");
        //                        }

        //                    }
        //                }

        //            if (Model.ROSSystems != null)
        //            {
        //                if (Model.ROSSystems.Count > 0)
        //                {
        //                    foreach (ROSSytemTemplateModel ROSSystems in Model.ROSSystems)
        //                    {
        //                        string SystemName = ROSSystems.SystemName;
        //                        string SystemId = ROSSystems.ROSSystemId;


        //                        string SystemNormalDescription = ROSSystems.IsNormalComments;


        //                        if (ROSSystems.IsNormal != "")
        //                        {

        //                            if (ROSSystems.IsNormal == "true")
        //                            {

        //                                sb.Append(generateSoapTextSystemName("true", SystemName, SystemId, SystemNormalDescription));

        //                                foreach (ROSCharTemplateModel RosChartristicsDetail in ROSSystems.RosChartristicsDetail)
        //                                {

        //                                    sb.Append("<span id='divCharc_" + RosChartristicsDetail.ROSCharacteristicsId + "' ROSSystemId='" + RosChartristicsDetail.ROSSystemId + "' ROSTemplateId='" + Model.TemplateId + "' title='Characteristics'  name='Characteristics'>" + ((RosChartristicsDetail.ROSCharacteristicsName != "") ? RosChartristicsDetail.ROSCharacteristicsName + " is Normal." : ""));
        //                                    sb.Append("</span> ");

        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (ROSSystems.RosChartristicsDetail != null)
        //                                {
        //                                    if (ROSSystems.RosChartristicsDetail.Count > 0)
        //                                    {

        //                                        bool systemExists = true;

        //                                        foreach (ROSCharTemplateModel RosChartristicsDetail in ROSSystems.RosChartristicsDetail)
        //                                        {


        //                                            string soapTextCharcName = generateSoapTextCharcName(RosChartristicsDetail, SystemName, RosChartristicsDetail.ROSSystemId, Model);
        //                                            if (!string.IsNullOrEmpty(soapTextCharcName))
        //                                            {
        //                                                if (systemExists)
        //                                                {

        //                                                    sb.Append(generateSoapTextSystemName("false", SystemName, SystemId, String.Empty));//"<div id='divSys_" + SystemId + "' title='System'  name='System'><strong>" + SystemName + " : </strong><br/>");
        //                                                    systemExists = false;
        //                                                }
        //                                                if (RosChartristicsDetail.IsPositive == "true")
        //                                                {
        //                                                    sb.Append("<span id='divCharc_" + RosChartristicsDetail.ROSCharacteristicsId + "' ROSSystemId='" + RosChartristicsDetail.ROSSystemId + "' title='Characteristics'  name='Characteristics'>" + ((RosChartristicsDetail.ROSCharacteristicsName != "") ? RosChartristicsDetail.ROSCharacteristicsName : ""));
        //                                                }
        //                                                else
        //                                                {
        //                                                    sb.Append("<span id='divCharc_" + RosChartristicsDetail.ROSCharacteristicsId + "' ROSSystemId='" + RosChartristicsDetail.ROSSystemId + "' title='Characteristics'  name='Characteristics'>" + ((RosChartristicsDetail.ROSCharacteristicsName != "") ? RosChartristicsDetail.ROSCharacteristicsName : ""));
        //                                                }

        //                                                sb.Append(soapTextCharcName);

        //                                                if (Model.ROSCharaDetailGenral != null && Model.ROSCharaDetailGenral.Count > 0)
        //                                                {

        //                                                    bool detailExists = false;
        //                                                    foreach (ROSCharaDetailGenralModel ROSCharaDetailGenral in Model.ROSCharaDetailGenral)
        //                                                    {
        //                                                        if (ROSCharaDetailGenral.ROSSystemId == RosChartristicsDetail.ROSSystemId && ROSCharaDetailGenral.ROSCharacteristicsId == RosChartristicsDetail.ROSCharacteristicsId)
        //                                                        {
        //                                                            string soapTextCharc = generateSoapTextForROSCharcDetails(ROSCharaDetailGenral);
        //                                                            detailExists = true;
        //                                                            if (!string.IsNullOrEmpty(soapTextCharc))
        //                                                            {

        //                                                                sb.Append(" " + soapTextCharc);
        //                                                                sb.Append("</span> ");
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                sb.Append("</span>, ");
        //                                                            }
        //                                                            //break;
        //                                                        }
        //                                                    }
        //                                                    if (!detailExists)
        //                                                    {
        //                                                        sb.Append("</span>, ");
        //                                                    }
        //                                                }
        //                                                else
        //                                                {
        //                                                    sb.Append("</span> ");

        //                                                }
        //                                            }

        //                                        }
        //                                        if (!systemExists)
        //                                        {
        //                                            sb.Append("</div>");
        //                                            string updatedSOAP = sb.ToString();
        //                                            if (!string.IsNullOrEmpty(updatedSOAP) && updatedSOAP.Length > 8 && updatedSOAP.Substring(updatedSOAP.Length - 8).Equals(", </div>"))
        //                                            {
        //                                                updatedSOAP = updatedSOAP.Remove(updatedSOAP.Length - 8);
        //                                                updatedSOAP += "</div>";
        //                                            }
        //                                            sb = new StringBuilder();
        //                                            sb.Append(updatedSOAP);
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    sb.Append("<br>" + overAllComments);
        //                }
        //            }
        //        }


        //    }

        //    string returnResult = sb.ToString();
        //    if (!string.IsNullOrEmpty(returnResult) && returnResult.Length > 8 && returnResult.Substring(returnResult.Length - 8).Equals(", </div>"))
        //    {
        //        returnResult = returnResult.Remove(returnResult.Length - 8);
        //        returnResult += "</div>";
        //    }
        //    return returnResult;

        //}
        //public string generateSoapTextForROSCharcDetails(ROSCharaDetailGenralModel ROSCharaDetailGenral)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    if (ROSCharaDetailGenral.LookupTypeName == "PreviousHistory")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Value.ToString()) ? "" : " Previous history is  " + ROSCharaDetailGenral.Value + "."));
                
        //    }         
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSCharacteristicsDetailStatus")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Text.ToString()) ? "" : " Status is  " + ROSCharaDetailGenral.Text + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSOnset")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Value.ToString()) ? "" : " Onset is  " + ROSCharaDetailGenral.Value + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSDuration")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Value.ToString()) ? "" : " Duration is  " + ROSCharaDetailGenral.Value));
                
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSCharacteristicsDetailDuration")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Text.ToString()) ? "" : "  " + ROSCharaDetailGenral.Text + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSCharacteristicsDetailPattern")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Text.ToString()) ? "" : " Pattern is  " + ROSCharaDetailGenral.Text + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSCharacteristicsDetailSeverity")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Text.ToString()) ? "" : " Severity is  " + ROSCharaDetailGenral.Text + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSCharacteristicsDetailCourse")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Text.ToString()) ? "" : " Course is  " + ROSCharaDetailGenral.Text + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSCharacteristicsDetailRadiation")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Text.ToString()) ? "" : " Radiation to  " + ROSCharaDetailGenral.Text + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSCharacteristicsDetailFrequency")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Text.ToString()) ? "" : " Frequency is  " + ROSCharaDetailGenral.Text + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSCharacteristicsDetailContext")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Text.ToString()) ? "" : " Context is  " + ROSCharaDetailGenral.Text + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSCharacteristicsDetailCharacterCSZ")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Text.ToString()) ? "" : " Character is  " + ROSCharaDetailGenral.Text + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSCharacteristicsDetailAggravedBy")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Text.ToString()) ? "" : " It is aggravated by  " + ROSCharaDetailGenral.Text + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "ROSCharacteristicsDetailRelievedBy")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Text.ToString()) ? "" : " It is relieved by  " + ROSCharaDetailGenral.Text + "."));
        //    }
        //    //End   || 12 July, 2016 || ZeeshanAK || Fix for EMR-1474
        //    if (ROSCharaDetailGenral.LookupTypeName == "Location")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Value.ToString()) ? "" : " Location is  " + ROSCharaDetailGenral.Value + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "PrecipitatedBy")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Value.ToString()) ? "" : " Precipitated by  " + ROSCharaDetailGenral.Value + "."));
        //    }
        //    if (ROSCharaDetailGenral.LookupTypeName == "AssociatedBy")
        //    {
        //        sb.Append((string.IsNullOrEmpty(ROSCharaDetailGenral.Value.ToString()) ? "" : " Associated feature are  " + ROSCharaDetailGenral.Value + "."));
        //    }

        //    return sb.ToString();


        //}
        //public string generateSoapTextCharcName(ROSCharTemplateModel RosChartristicsDetail, string SystemName, string SystemId, ROSTemplateModel Model)
        //{
        //    StringBuilder sb = new StringBuilder();


        //    // return charc name and positive and descriptoin
        //    if (RosChartristicsDetail.IsPositive != "" && RosChartristicsDetail.IsPositive != null)
        //    {
        //        if (RosChartristicsDetail.ROSCharacteristicsName != "")
        //        {
        //            bool isPositive = bool.Parse(RosChartristicsDetail.IsPositive);
        //            string jQueryFunction = " onclick='Clinical_ROSTemplateDetailRevamp.toggleAttribute(this," + (isPositive ? "true":"false" ) + "," + Model.NoteId + "," + Model.TemplateId + "," + RosChartristicsDetail.ROSSystemId + ","+ RosChartristicsDetail .ROSCharacteristicsId+ ",event);'";
        //            string cssClass = isPositive ? "red" : "green";
        //            string actionLink = "<a href='javascript:void(0);' " + jQueryFunction + " class='hoverToggle " + cssClass + "' >";
        //            sb.Append("<b>" + actionLink + (isPositive ? " ( + ) " : " ( - ) ") + "</a></b>");
        //        }

        //    }
        //    if (RosChartristicsDetail.CharComments != "")
        //    {
        //        sb.Append((string.IsNullOrEmpty(RosChartristicsDetail.CharComments) ? "" : RosChartristicsDetail.CharComments));
        //    }
        //    return sb.ToString();
        //}
        //public string generateSoapTextSystemName(string IsNormal, string SystemName, string SystemId, string SystemNormalDescription)
        //{
        //    if (IsNormal== "true")
        //    {

        //        return ("<span id='divSys_" + SystemId + "' title='" + SystemName + "'  name='ROSSystem'><strong>" + SystemName + "</strong> is Normal. " + (string.IsNullOrEmpty(SystemNormalDescription) ? "" : SystemNormalDescription + ". ") + "</span>");

        //    }
        //    else
        //    {
        //        return ("<div id='divSys_" + SystemId + "' title='" + SystemName + "'  name='ROSSystem'><strong>" + SystemName + "</strong>: ");
        //    }
        //}
    }
}