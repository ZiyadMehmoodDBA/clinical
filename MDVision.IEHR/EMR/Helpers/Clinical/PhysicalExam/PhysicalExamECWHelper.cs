using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.PhysicalExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.PhysicalExam
{
    public class PhysicalExamECWHelper
    {
        private BLLPhysicalExamECW BLLPhysicalExamECWObj = null;
        public PhysicalExamECWHelper()
        {
            BLLPhysicalExamECWObj = new BLLPhysicalExamECW();
        }

        private static PhysicalExamECWHelper _instance = null;
        public static PhysicalExamECWHelper Instance()
        {
            if (_instance == null)
                _instance = new PhysicalExamECWHelper();
            return _instance;
        }


        #region " Physical Exam System Fill, Save and Update Methods "

        public string loadPhysicalExamSystem(PhysicalExamECWSystemModel model)
        {
            try
            {
                DSPhysicalExamECW dsPE = null;
                BLObject<DSPhysicalExamECW> obj;
                obj = BLLPhysicalExamECWObj.loadPhysicalExamSystem(MDVUtility.ToLong(model.PESystemId), model.IsActive, model.Name, MDVUtility.ToLong(model.PageNumber), MDVUtility.ToLong(model.RowsPerPage));
                if (obj.Data != null)
                {
                    dsPE = obj.Data;
                    if (dsPE.Tables[dsPE.PESystem.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SystemCount = dsPE.Tables[dsPE.PESystem.TableName].Rows.Count,
                            iTotalDisplayRecords = dsPE.Tables[dsPE.PESystem.TableName].Rows[0][dsPE.PESystem.RecordCountColumn.ColumnName],
                            SystemLoad_JSON = MDVUtility.JSON_DataTable(dsPE.Tables[dsPE.PESystem.TableName]),
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

        public string fillPhysicalExamSystem(PhysicalExamECWSystemModel model)
        {
            try
            {
                DSPhysicalExamECW dsPE = null;
                BLObject<DSPhysicalExamECW> obj;
                obj = BLLPhysicalExamECWObj.loadPhysicalExamSystem(MDVUtility.ToLong(model.PESystemId), "", "", 1, 25);
                if (obj.Data != null)
                {
                    dsPE = obj.Data;
                    obj = null;

                    if (dsPE.Tables[dsPE.PESystem.TableName].Rows.Count > 0)
                    {
                        obj = BLLPhysicalExamECWObj.loadPhysicalExamSystemObservation(MDVUtility.ToLong(model.PESystemId));
                        if (obj.Data != null)
                            dsPE.Merge(obj.Data);

                        var response = new
                        {
                            status = true,
                            SystemCount = dsPE.Tables[dsPE.PESystem.TableName].Rows.Count,
                            SystemLoad_JSON = MDVUtility.JSON_DataTable(dsPE.Tables[dsPE.PESystem.TableName]),
                            ObservationCount = dsPE.Tables[dsPE.PESystemObservation.TableName].Rows.Count,
                            ObservationLoad_JSON = MDVUtility.JSON_DataTable(dsPE.Tables[dsPE.PESystemObservation.TableName]),
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

        public string deletePhysicalExamSystem(long PESystemId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PESystemId)))
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
                    BLObject<string> obj = BLLPhysicalExamECWObj.deletePhysicalExamSystem(MDVUtility.ToStr(PESystemId));
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

        public string updatePhysicalExamSystem(long PESystemId, string IsActive)
        {
            try
            {
                if (PESystemId > 0)
                {

                    DSPhysicalExamECW dsPE = new DSPhysicalExamECW();
                    BLObject<DSPhysicalExamECW> obj = BLLPhysicalExamECWObj.loadPhysicalExamSystem(PESystemId, "", "", 1, 25);
                    if (obj.Data != null)
                    {
                        dsPE = obj.Data;
                        foreach (DSPhysicalExamECW.PESystemRow dr in dsPE.Tables[dsPE.PESystem.TableName].Rows)
                        {
                            dr.IsActive = IsActive == "1" ? true : false;
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }
                        BLObject<DSPhysicalExamECW> objUpdate = BLLPhysicalExamECWObj.updatePhysicalExamSystem(dsPE);
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
                        Message = "Physical Exam System not found."
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

        public string updatePhysicalExamSystem(PhysicalExamECWSystemModel model)
        {
            try
            {
                if (MDVUtility.ToLong(model.PESystemId) > 0)
                {

                    DSPhysicalExamECW dsPE = new DSPhysicalExamECW();
                    BLObject<DSPhysicalExamECW> obj = BLLPhysicalExamECWObj.loadPhysicalExamSystem(MDVUtility.ToLong(model.PESystemId), "", "", 1, 25);
                    if (obj.Data != null)
                    {
                        dsPE = obj.Data;
                        foreach (DSPhysicalExamECW.PESystemRow dr in dsPE.Tables[dsPE.PESystem.TableName].Rows)
                        {
                            if (!string.IsNullOrEmpty(model.IsActive))
                                dr.IsActive = model.IsActive.ToLower() == "true" ? true : false;

                            if (!string.IsNullOrEmpty(model.Name))
                                dr.Name = model.Name;

                            if (!string.IsNullOrEmpty(model.IsGlobal))
                                dr.IsGlobal = model.IsGlobal == "1" ? true : false;

                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }

                        // update PE System
                        BLObject<DSPhysicalExamECW> objUpdate = BLLPhysicalExamECWObj.updatePhysicalExamSystem(dsPE);
                        if (objUpdate.Data != null)
                        {
                            // insert PE Observation if any
                            if (!string.IsNullOrEmpty(model.PEObservationIds))
                            {
                                PESystemModelResponse response_ = associatePEObservationforSystem(model);
                                if (response_.Status)
                                {
                                    var response = new
                                    {
                                        status = true,
                                        Message = Common.AppPrivileges.Update_Message,
                                        PEObservation_JSON = response_.Data,
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                }
                                else
                                {
                                    var response = new
                                    {
                                        status = true,
                                        Message = Common.AppPrivileges.Update_Message + " " + response_.Message,
                                        PEObservation_JSON = "[]"
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    Message = Common.AppPrivileges.Update_Message,
                                    PEObservation_JSON = "[]",
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
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
                        Message = "Physical Exam System not found."
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

        public PESystemModelResponse addPEObservationforSystem(PhysicalExamECWSystemModel model)
        {
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
            DSPhysicalExamECW.PEObservationRow drObservation = dsPhysicalExam.PEObservation.NewPEObservationRow();
            PESystemModelResponse response = new PESystemModelResponse();
            response.Status = false;

            if (!string.IsNullOrEmpty(model.IsActive))
                drObservation.IsActive = MDVUtility.ToStr(model.IsActive) == "true" ? true : false;

            //drObservation.PESystemId = MDVUtility.ToLong(model.PESystemId);
            drObservation.Name = model.Observations;
            drObservation.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            drObservation.CreatedOn = DateTime.Now;
            drObservation.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            drObservation.ModifiedOn = DateTime.Now;
            dsPhysicalExam.PEObservation.AddPEObservationRow(drObservation);

            BLObject<DSPhysicalExamECW> objInsertedObservation = BLLPhysicalExamECWObj.insertPhysicalExamObservation(dsPhysicalExam);
            if (objInsertedObservation.Data != null)
            {
                DSPhysicalExamECW dsSavedObservation = objInsertedObservation.Data;
                long PEObservationId = MDVUtility.ToInt64(dsSavedObservation.Tables[dsSavedObservation.PEObservation.TableName].Rows[0][dsSavedObservation.PEObservation.PEObservationIdColumn.ColumnName]);
                response.Status = true;
                response.Message = Common.AppPrivileges.Save_Message;
                response.Data = MDVUtility.JSON_DataTable(dsSavedObservation.Tables[dsSavedObservation.PEObservation.TableName]);

            }
            else
            {
                response.Status = false;
                response.Message = objInsertedObservation.Message;
                response.Data = "[]";
            }

            return response;
        }

        public PESystemModelResponse associatePEObservationforSystem(PhysicalExamECWSystemModel model)
        {

            List<string> PEObservationIds = model.PEObservationIds.Split(',').ToList().Select(s => s).ToList();
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
            PESystemModelResponse response = new PESystemModelResponse();
            response.Status = false;

            foreach (var ObservationId in PEObservationIds)
            {
                if (!string.IsNullOrEmpty(ObservationId))
                {
                    DSPhysicalExamECW.PESystemObservationRow dr = dsPhysicalExam.PESystemObservation.NewPESystemObservationRow();
                    dr.PESystemId = MDVUtility.ToLong(model.PESystemId);
                    dr.PEObservationId = MDVUtility.ToLong(ObservationId);
                    dsPhysicalExam.PESystemObservation.AddPESystemObservationRow(dr);
                }
            }
            BLObject<DSPhysicalExamECW> objObservation = BLLPhysicalExamECWObj.insertPhysicalExamSystemObservation(dsPhysicalExam);
            if (objObservation.Data != null)
            {
                DSPhysicalExamECW dsAssociatedObservation = objObservation.Data;
                response.Status = true;
                response.Message = Common.AppPrivileges.Save_Message;
                response.Data = MDVUtility.JSON_DataTable(dsAssociatedObservation.Tables[dsAssociatedObservation.PESystemObservation.TableName]);
            }
            else
            {
                response.Status = false;
                response.Message = objObservation.Message;
                response.Data = "[]";
            }

            return response;
        }

        public string insertPhysicalExamSystem(PhysicalExamECWSystemModel model)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
                DSPhysicalExamECW.PESystemRow drSystem = dsPhysicalExam.PESystem.NewPESystemRow();

                if (!string.IsNullOrEmpty(model.IsActive))
                    drSystem.IsActive = model.IsActive.ToLower() == "true" ? true : false;

                drSystem.IsGlobal = true;
                drSystem.Name = model.Name;
                drSystem.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drSystem.CreatedOn = DateTime.Now;
                drSystem.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drSystem.ModifiedOn = DateTime.Now;
                drSystem.PETemplateID = model.PETemplateID;
                dsPhysicalExam.PESystem.AddPESystemRow(drSystem);

                // insert PE System
                BLObject<DSPhysicalExamECW> objInsertedSystem = BLLPhysicalExamECWObj.insertPhysicalExamSystem(dsPhysicalExam);
                if (objInsertedSystem.Data != null)
                {
                    DSPhysicalExamECW dsSavedSystem = objInsertedSystem.Data;
                    model.PESystemId = MDVUtility.ToInt64(dsSavedSystem.Tables[dsSavedSystem.PESystem.TableName].Rows[0][dsSavedSystem.PESystem.PESystemIdColumn.ColumnName]).ToString();
                    model.PETemplateSystemId = MDVUtility.ToInt64(dsSavedSystem.Tables[dsSavedSystem.PESystem.TableName].Rows[0][dsSavedSystem.PESystem.PETemplateSystemIdColumn.ColumnName]).ToString();

                    // insert Associate PE Observations with System if there is
                    if (!string.IsNullOrEmpty(model.PEObservationIds))
                    {
                        PESystemModelResponse response_ = associatePEObservationforSystem(model);
                        if (response_.Status)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message,
                                PESystemId = model.PESystemId,
                                PETemplateSystemId = model.PETemplateSystemId,
                                PEObservation_JSON = response_.Data,
                                PESystem_JSON = MDVUtility.JSON_DataTable(dsSavedSystem.Tables[dsSavedSystem.PESystem.TableName]),

                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                PESystemId = model.PESystemId,
                                PETemplateSystemId = model.PETemplateSystemId,
                                PEObservation_JSON = "[]",
                                PESystem_JSON = MDVUtility.JSON_DataTable(dsSavedSystem.Tables[dsSavedSystem.PESystem.TableName]),
                                Message = Common.AppPrivileges.Save_Message + " " + response_.Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            PESystemId = model.PESystemId,
                            PETemplateSystemId = model.PETemplateSystemId,
                            PEObservation_JSON = "[]",
                            PESystem_JSON = MDVUtility.JSON_DataTable(dsSavedSystem.Tables[dsSavedSystem.PESystem.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objInsertedSystem.Message.Contains("UQ_PESystem_Name") ? "System already exist" : objInsertedSystem.Message,
                        PESystemId = 0,
                        PETemplateSystemId = 0,
                        PEObservation_JSON = "[]",
                        PESystem_JSON = "[]",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }


        }

        public string lookupPhysicalExamSystem(PhysicalExamECWSystemModel model)
        {
            try
            {
                DSPhysicalExamECWLookup dsPE = null;
                BLObject<DSPhysicalExamECWLookup> obj;
                obj = BLLPhysicalExamECWObj.lookupPESystem(model.IsActive);
                if (obj.Data != null)
                {
                    dsPE = obj.Data;
                    if (dsPE.Tables[dsPE.PESystemLookup.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SystemCount = dsPE.Tables[dsPE.PESystemLookup.TableName].Rows.Count,
                            System_JSON = MDVUtility.JSON_DataTable(dsPE.Tables[dsPE.PESystemLookup.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SystemCount = 0,
                            System_JSON = "[]",
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
                        System_JSON = "[]",
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

        #endregion

        #region " Physical Exam Observation Fill, Save and Update Methods "


        public PESystemModelResponse associatePEObservationforSystem(long PESystemId, long PEObservationId, long PETemplateSystemId = 0)
        {
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
            PESystemModelResponse response = new PESystemModelResponse();
            response.Status = false;

            DSPhysicalExamECW.PESystemObservationRow dr = dsPhysicalExam.PESystemObservation.NewPESystemObservationRow();
            dr.PESystemId = MDVUtility.ToLong(PESystemId);
            dr.PEObservationId = MDVUtility.ToLong(PEObservationId);
            dr.PETemplateSystemId = MDVUtility.ToLong(PETemplateSystemId);
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);          
            dsPhysicalExam.PESystemObservation.AddPESystemObservationRow(dr);


            BLObject<DSPhysicalExamECW> objObservation = BLLPhysicalExamECWObj.insertPhysicalExamSystemObservation(dsPhysicalExam);
            if (objObservation.Data != null)
            {
                DSPhysicalExamECW dsAssociatedObservation = objObservation.Data;
                response.Status = true;
                response.Message = Common.AppPrivileges.Save_Message;
                response.Data = MDVUtility.JSON_DataTable(dsAssociatedObservation.Tables[dsAssociatedObservation.PESystemObservation.TableName]);
            }
            else
            {
                response.Status = false;
                response.Message = objObservation.Message;
                response.Data = "[]";
            }

            return response;
        }

        public string insertPhysicalExamIObservation_(PhysicalExamECWObservationModel model)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
                DSPhysicalExamECW.PEObservationRow drObservation = dsPhysicalExam.PEObservation.NewPEObservationRow();

                if (!string.IsNullOrEmpty(model.IsActive))
                    drObservation.IsActive = (MDVUtility.ToStr(model.IsActive) == "true" || MDVUtility.ToStr(model.IsActive) == "True") ? true : false;

                drObservation.Name = model.Name;
                drObservation.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drObservation.CreatedOn = DateTime.Now;
                drObservation.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drObservation.ModifiedOn = DateTime.Now;
                dsPhysicalExam.PEObservation.AddPEObservationRow(drObservation);

                BLObject<DSPhysicalExamECW> objInsertedObservation = BLLPhysicalExamECWObj.insertPhysicalExamObservation(dsPhysicalExam);
                if (objInsertedObservation.Data != null)
                {
                    DSPhysicalExamECW dsSavedObservation = objInsertedObservation.Data;
                    long PEObservationId = MDVUtility.ToInt64(dsSavedObservation.Tables[dsSavedObservation.PEObservation.TableName].Rows[0][dsSavedObservation.PEObservation.PEObservationIdColumn.ColumnName]);

                    PESystemModelResponse response_ = associatePEObservationforSystem(MDVUtility.ToLong(model.PESystemId), PEObservationId, MDVUtility.ToLong(model.PETemplateSystemId));

                    if (response_.Status)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            PEObservation_JSON = response_.Data,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message + " " + response_.Message,
                            PEObservation_JSON = "[]"
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                    //var response = new
                    //{
                    //    status = true,
                    //    Message = Common.AppPrivileges.Save_Message,
                    //    PEObservationId = PEObservationId,

                    //};
                    //return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objInsertedObservation.Message
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

        public string insertPhysicalExamIObservation(PhysicalExamECWObservationModel model)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
                DSPhysicalExamECW.PEObservationRow drObservation = dsPhysicalExam.PEObservation.NewPEObservationRow();

                if (!string.IsNullOrEmpty(model.IsActive))
                    drObservation.IsActive = MDVUtility.ToStr(model.IsActive).ToLower() == "true" || MDVUtility.ToStr(model.IsActive).ToLower() == "1" ? true : false;

                drObservation.Name = model.Name;
                drObservation.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drObservation.CreatedOn = DateTime.Now;
                drObservation.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drObservation.ModifiedOn = DateTime.Now;
                dsPhysicalExam.PEObservation.AddPEObservationRow(drObservation);

                BLObject<DSPhysicalExamECW> objInsertedObservation = BLLPhysicalExamECWObj.insertPhysicalExamObservation(dsPhysicalExam);
                if (objInsertedObservation.Data != null)
                {
                    DSPhysicalExamECW dsSavedObservation = objInsertedObservation.Data;
                    long PEObservationId = MDVUtility.ToInt64(dsSavedObservation.Tables[dsSavedObservation.PEObservation.TableName].Rows[0][dsSavedObservation.PEObservation.PEObservationIdColumn.ColumnName]);

                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        PEObservationId = PEObservationId,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objInsertedObservation.Message
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

        public string updatePhysicalExamObservation(long PEObservationId, string IsActive)
        {
            try
            {
                if (PEObservationId > 0)
                {

                    DSPhysicalExamECW dsPE = new DSPhysicalExamECW();
                    BLObject<DSPhysicalExamECW> obj = BLLPhysicalExamECWObj.loadPhysicalExamObservation(PEObservationId, "", "", 1, 25);
                    if (obj.Data != null)
                    {
                        dsPE = obj.Data;
                        foreach (DSPhysicalExamECW.PEObservationRow dr in dsPE.Tables[dsPE.PEObservation.TableName].Rows)
                        {
                            dr.IsActive = IsActive.ToLower() == "1" ? true : false;
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }
                        BLObject<DSPhysicalExamECW> objUpdate = BLLPhysicalExamECWObj.updatePhysicalExamObservation(dsPE);
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
                        Message = "Physical Exam Observation not found."
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

        public string updatePhysicalExamObservation(PhysicalExamECWObservationModel model)
        {
            try
            {
                if (MDVUtility.ToLong(model.PEObservationId) > 0)
                {

                    DSPhysicalExamECW dsPE = new DSPhysicalExamECW();
                    BLObject<DSPhysicalExamECW> obj = BLLPhysicalExamECWObj.loadPhysicalExamObservation(MDVUtility.ToLong(model.PEObservationId), "", "", 1, 25);
                    if (obj.Data != null)
                    {
                        dsPE = obj.Data;
                        foreach (DSPhysicalExamECW.PEObservationRow dr in dsPE.Tables[dsPE.PEObservation.TableName].Rows)
                        {
                            if (!string.IsNullOrEmpty(model.IsActive))
                                dr.IsActive = model.IsActive.ToLower() == "true" ? true : false;

                            if (!string.IsNullOrEmpty(model.Name))
                                dr.Name = model.Name;

                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }

                        // update PE System
                        BLObject<DSPhysicalExamECW> objUpdate = BLLPhysicalExamECWObj.updatePhysicalExamObservation(dsPE);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message,
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
                        Message = "Physical Exam Observation not found."
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

        public string loadPhysicalExamObservation(PhysicalExamECWObservationModel model)
        {
            try
            {
                DSPhysicalExamECW dsPE = null;
                BLObject<DSPhysicalExamECW> obj;
                obj = BLLPhysicalExamECWObj.loadPhysicalExamObservation(MDVUtility.ToLong(model.PEObservationId), model.IsActive, model.Name, MDVUtility.ToLong(model.PageNumber), MDVUtility.ToLong(model.RowsPerPage));
                if (obj.Data != null)
                {
                    dsPE = obj.Data;
                    if (dsPE.Tables[dsPE.PEObservation.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = dsPE.Tables[dsPE.PEObservation.TableName].Rows.Count,
                            iTotalDisplayRecords = dsPE.Tables[dsPE.PEObservation.TableName].Rows[0][dsPE.PEObservation.RecordCountColumn.ColumnName],
                            ObservationLoad_JSON = MDVUtility.JSON_DataTable(dsPE.Tables[dsPE.PEObservation.TableName]),
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

        public string fillPhysicalExamObservation(PhysicalExamECWObservationModel model)
        {
            try
            {
                DSPhysicalExamECW dsPE = null;
                BLObject<DSPhysicalExamECW> obj;
                obj = BLLPhysicalExamECWObj.loadPhysicalExamObservation(MDVUtility.ToLong(model.PEObservationId), "", "", 1, 25);
                if (obj.Data != null)
                {
                    dsPE = obj.Data;
                    obj = null;

                    if (dsPE.Tables[dsPE.PEObservation.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = dsPE.Tables[dsPE.PEObservation.TableName].Rows.Count,
                            ObservationLoad_JSON = MDVUtility.JSON_DataTable(dsPE.Tables[dsPE.PEObservation.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = 0,
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

        public string deletePhysicalExamObservation(string ObservationId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ObservationId)))
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
                    BLObject<string> obj = BLLPhysicalExamECWObj.deletePhysicalExamObservation(ObservationId);
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

        public string lookupPhysicalExamObservation(PhysicalExamECWObservationModel model)
        {
            try
            {
                DSPhysicalExamECWLookup dsPE = null;
                BLObject<DSPhysicalExamECWLookup> obj;
                obj = BLLPhysicalExamECWObj.lookupPEObservation(model.IsActive);
                if (obj.Data != null)
                {
                    dsPE = obj.Data;
                    if (dsPE.Tables[dsPE.PEObservationLookup.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = dsPE.Tables[dsPE.PEObservationLookup.TableName].Rows.Count,
                            Observation_JSON = MDVUtility.JSON_DataTable(dsPE.Tables[dsPE.PEObservationLookup.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ObservationCount = 0,
                            Observation_JSON = "[]",
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
                        Observation_JSON = "[]",
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

        #endregion

        #region " Physical Exam System Observation Fill, Save and Update Methods "

        public string deletePhysicalExamSystemObservation(string PESystemObservationId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PESystemObservationId)))
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
                    BLObject<string> obj = BLLPhysicalExamECWObj.deletePhysicalExamSystemObservation(PESystemObservationId);
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

    }
}