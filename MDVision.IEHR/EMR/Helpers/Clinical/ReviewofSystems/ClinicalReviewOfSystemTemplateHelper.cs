
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using System.Text;
using MDVision.IEHR.EMR.Model.ReviewofSystems;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems
{
    public class ClinicalReviewOfSystemTemplateHelper
    {
         private BLLClinical BLLClinicalObj = null;
         public ClinicalReviewOfSystemTemplateHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static ClinicalReviewOfSystemTemplateHelper _instance = null;
        public static ClinicalReviewOfSystemTemplateHelper Instance()
        {
            if (_instance == null)
                _instance = new ClinicalReviewOfSystemTemplateHelper();
            return _instance;
        }

        /// Author: ZeeshanAK
        /// Purpose: To load Review Of Systems Templates
        /// Date : March 01, 2016
        #region Load ROS Templates

        public string fillROSTemplates(long templateID)
        {
            try
            {
                DSClinicalReviewofSystemTemplate dsROSTemplate = null;
                BLObject<DSClinicalReviewofSystemTemplate> obj;
                obj = BLLClinicalObj.fillROSTemplates(templateID);

                dsROSTemplate = obj.Data;
                if (obj.Data != null)
                {
                    if (dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ROSTemplateCount = dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName].Rows.Count,
                            iTotalDisplayRecords = dsROSTemplate.ROSTemplate.Rows[0][dsROSTemplate.ROSTemplate.RecordCountColumn.ColumnName],
                            ROSTemplateLoad_JSON = MDVUtility.JSON_DataTable(dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ROSTemplateCount = 0,
                            iTotalDisplayRecords = 0,
                            ROSTemplateLoad_JSON = MDVUtility.JSON_DataTable(dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ROSTemplateCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string loadROSTemplates(int isActive, long templateID, long pageNumber, long rowsPerPage, long entityID)
        {
            try
            {
                DSClinicalReviewofSystemTemplate dsROSTemplate = null;
                BLObject<DSClinicalReviewofSystemTemplate> obj;
                obj = BLLClinicalObj.loadROSTemplates(isActive, templateID, pageNumber, rowsPerPage, entityID);

                dsROSTemplate = obj.Data;
                if (obj.Data != null)
                {
                    if (dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ROSTemplateCount = dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName].Rows.Count,
                            iTotalDisplayRecords = dsROSTemplate.ROSTemplate.Rows[0][dsROSTemplate.ROSTemplate.RecordCountColumn.ColumnName],
                            ROSTemplateLoad_JSON = MDVUtility.JSON_DataTable(dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ROSTemplateCount = 0,
                            iTotalDisplayRecords = 0,
                            ROSTemplateLoad_JSON = MDVUtility.JSON_DataTable(dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ROSTemplateCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string updateClinical_ROSTemplateIsActive(Int64 templateID, Int64 IsActive, Int64 EntityId)
        {
            try
            {
                if (templateID > 0)
                {

                    DSClinicalReviewofSystemTemplate dsROSTemplate = null;
                    BLObject<DSClinicalReviewofSystemTemplate> obj = BLLClinicalObj.fillROSTemplates(templateID);
                    dsROSTemplate = obj.Data;
                    if (dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName].Rows[0];
                        dr[dsROSTemplate.ROSTemplate.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSClinicalReviewofSystemTemplate> objNotes = BLLClinicalObj.updateClinical_ROSTemplate(dsROSTemplate);
                        string successMsg;
                        if (objNotes.Data != null)
                        {
                            if (IsActive == 0)
                                successMsg = Common.AppPrivileges.Inactive_Message;
                            else
                                successMsg = Common.AppPrivileges.Active_Message;
                            var response = new
                            {
                                status = true,
                                Message = successMsg
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objNotes.Message
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
                        Message = "ROS Template not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Deletes the Clinical Notes.
        /// </summary>
        /// <param name="NotesID">The Notes identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string deleteClinical_ROSTemplate(long templateID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(templateID)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteClinical_ROSTemplate(MDVUtility.ToStr(templateID));
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public ROSTemplateResponse updateROSTemplateInfo(ROSTemplateWrapperModel model)
        {
            ROSTemplateResponse responseModel = new ROSTemplateResponse();
            try
            {
                if (model.ROSTemplateId > 0)
                {

                    DSClinicalReviewofSystemTemplate dsROSTemplate = null;
                    if (model.EntityId==null)
                    {
                        model.EntityId = Convert.ToInt32(MDVSession.Current.EntityId);
                    }
                    BLObject<DSClinicalReviewofSystemTemplate> obj = BLLClinicalObj.fillROSTemplates(model.ROSTemplateId);
                    dsROSTemplate = obj.Data;
                    if (dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName].Rows[0];
                        dr[dsROSTemplate.ROSTemplate.ROSTemplateIdColumn.ColumnName] = model.ROSTemplateId;

                        //dr[dsROSTemplate.ROSTemplate.IsDefaultColumn.ColumnName] = DateTime.Now;
                        dr[dsROSTemplate.ROSTemplate.IsProviderAllColumn.ColumnName] = model.IsProviderAll;
                        //if (!model.IsProviderAll)
                        //{
                            dr[dsROSTemplate.ROSTemplate.ProviderIdsColumn.ColumnName] = model.ProviderIds;
                            dr[dsROSTemplate.ROSTemplate.ProviderNamesColumn.ColumnName] = model.ProviderNames;
                        //}


                        dr[dsROSTemplate.ROSTemplate.IsSpecialtyAllColumn.ColumnName] = model.IsSpecialityAll;
                        //if (!model.IsSpecialityAll)
                        //{
                            dr[dsROSTemplate.ROSTemplate.SpecialtyIdsColumn.ColumnName] = model.SpecialityIds;
                            dr[dsROSTemplate.ROSTemplate.SpecialtyNamesColumn.ColumnName] = model.SpecialityNames;
                        //}


                        dr[dsROSTemplate.ROSTemplate.TemplateNameColumn.ColumnName] = model.TemplateName;
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToLower().Equals("mdvision"))
                        {
                            dr[dsROSTemplate.ROSTemplate.EntityIdColumn.ColumnName] = model.EntityId;
                        }
                        else
                        {
                            dr[dsROSTemplate.ROSTemplate.EntityIdColumn.ColumnName] = MDVUtility.ToLong(MDVSession.Current.EntityId);
                        }

                        dr[dsROSTemplate.ROSTemplate.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsROSTemplate.ROSTemplate.ModifiedOnColumn.ColumnName] = DateTime.Now;

                        BLObject<DSClinicalReviewofSystemTemplate> objNotes = BLLClinicalObj.updateClinical_ROSTemplate(dsROSTemplate);

                        long rsoTemplateId = 0;
                        if (objNotes.Data != null)
                            rsoTemplateId = MDVUtility.ToInt64(objNotes.Data.Tables[objNotes.Data.ROSTemplate.TableName].Rows[0][objNotes.Data.ROSTemplate.ROSTemplateIdColumn.ColumnName]);

                        if (objNotes.Data != null && rsoTemplateId != -1)
                        {
                            responseModel.ROSTemplateId = model.ROSTemplateId;
                            responseModel.status = true;
                            responseModel.Message = Common.AppPrivileges.Update_Message;

                        }
                        else
                        {
                            responseModel.ROSTemplateId = -1;
                            responseModel.status = false;
                            responseModel.Message = rsoTemplateId == -1 ? "A Template with this name already exists. Try a different name." : objNotes.Message;

                        }
                    }
                    else
                    {
                        responseModel.ROSTemplateId = -1;
                        responseModel.status = false;
                        responseModel.Message = obj.Message;

                    }
                }
                else
                {
                    responseModel.ROSTemplateId = -1;
                    responseModel.status = false;
                    responseModel.Message = "ROS Template not found.";

                }
            }
            catch (Exception ex)
            {
                responseModel.status = false;
                responseModel.Message =MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.ROSTemplateId = -1;
            }
            return responseModel;
        }

        public ROSTemplateResponse insertROSTemplateInfo(ROSTemplateWrapperModel model)
        {
            ROSTemplateResponse responseModel = new ROSTemplateResponse();
            try
            {
                DSClinicalReviewofSystemTemplate dsROSTemplate = new DSClinicalReviewofSystemTemplate();
                DSClinicalReviewofSystemTemplate.ROSTemplateRow dr = dsROSTemplate.ROSTemplate.NewROSTemplateRow();


                dr[dsROSTemplate.ROSTemplate.ROSTemplateIdColumn.ColumnName] = model.ROSTemplateId;

                dr[dsROSTemplate.ROSTemplate.IsDefaultColumn.ColumnName] = false;
                dr[dsROSTemplate.ROSTemplate.IsProviderAllColumn.ColumnName] = model.IsProviderAll;
                //if (!model.IsProviderAll)
                //{
                    dr[dsROSTemplate.ROSTemplate.ProviderIdsColumn.ColumnName] = model.ProviderIds;
                    dr[dsROSTemplate.ROSTemplate.ProviderNamesColumn.ColumnName] = model.ProviderNames;
                //}


                dr[dsROSTemplate.ROSTemplate.IsSpecialtyAllColumn.ColumnName] = model.IsSpecialityAll;
                //if (!model.IsSpecialityAll)
                //{
                    dr[dsROSTemplate.ROSTemplate.SpecialtyIdsColumn.ColumnName] = model.SpecialityIds;
                    dr[dsROSTemplate.ROSTemplate.SpecialtyNamesColumn.ColumnName] = model.SpecialityNames;
                //}


                dr[dsROSTemplate.ROSTemplate.TemplateNameColumn.ColumnName] = model.TemplateName;
                if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToLower().Equals("mdvision"))
                {
                    dr[dsROSTemplate.ROSTemplate.EntityIdColumn.ColumnName] = model.EntityId;
                }
                else
                {
                    dr[dsROSTemplate.ROSTemplate.EntityIdColumn.ColumnName] = MDVUtility.ToLong(MDVSession.Current.EntityId);
                }

                dr[dsROSTemplate.ROSTemplate.IsActiveColumn.ColumnName] = true;
                dr[dsROSTemplate.ROSTemplate.CreatedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr[dsROSTemplate.ROSTemplate.CreatedOnColumn.ColumnName] = DateTime.Now;
                dr[dsROSTemplate.ROSTemplate.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr[dsROSTemplate.ROSTemplate.ModifiedOnColumn.ColumnName] = DateTime.Now;
                dsROSTemplate.ROSTemplate.AddROSTemplateRow(dr);
                BLObject<DSClinicalReviewofSystemTemplate> objNotes = BLLClinicalObj.insertClinical_ROSTemplate(dsROSTemplate);

                long rosTemplateId = 0;
                if (objNotes.Data != null)
                    rosTemplateId = MDVUtility.ToLong(dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName].Rows[0][dsROSTemplate.ROSTemplate.ROSTemplateIdColumn.ColumnName]);

                if (objNotes.Data != null && rosTemplateId != -1)
                {
                    DataRow drUpdated = dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName].Rows[0];
                    responseModel.ROSTemplateId = MDVUtility.ToLong(drUpdated[dsROSTemplate.ROSTemplate.ROSTemplateIdColumn.ColumnName].ToString());
                    responseModel.status = true;
                    responseModel.Message = Common.AppPrivileges.Update_Message;

                }
                else
                {
                    responseModel.status = false;
                    responseModel.ROSTemplateId = model.ROSTemplateId;
                    responseModel.Message = rosTemplateId == -1 ? "A Template with this name already exists. Try a different name." : Common.AppPrivileges.Update_Error_Message;
                }


            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message =MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.ROSTemplateId = model.ROSTemplateId;

            }
            return responseModel;
        }


        #region save as ROS template

        public ROSTemplateResponse saveAsROSTemplateInfo(ROSTemplateWrapperModel model)
        {
            ROSTemplateResponse responseModel = new ROSTemplateResponse();
            try
            {
                DSClinicalReviewofSystemTemplate dsROSTemplate = new DSClinicalReviewofSystemTemplate();
                DSClinicalReviewofSystemTemplate.ROSTemplateRow dr = dsROSTemplate.ROSTemplate.NewROSTemplateRow();

                dr[dsROSTemplate.ROSTemplate.ROSTemplateIdColumn.ColumnName] = -1;

                dr[dsROSTemplate.ROSTemplate.IsDefaultColumn.ColumnName] = false;
                dr[dsROSTemplate.ROSTemplate.IsProviderAllColumn.ColumnName] = model.IsProviderAll;
                //if (!model.IsProviderAll)
                //{
                    dr[dsROSTemplate.ROSTemplate.ProviderIdsColumn.ColumnName] = model.ProviderIds;
                    dr[dsROSTemplate.ROSTemplate.ProviderNamesColumn.ColumnName] = model.ProviderNames;
                //}

                dr[dsROSTemplate.ROSTemplate.IsSpecialtyAllColumn.ColumnName] = model.IsSpecialityAll;
                //if (!model.IsSpecialityAll)
                //{
                    dr[dsROSTemplate.ROSTemplate.SpecialtyIdsColumn.ColumnName] = model.SpecialityIds;
                    dr[dsROSTemplate.ROSTemplate.SpecialtyNamesColumn.ColumnName] = model.SpecialityNames;
                //}

                dr[dsROSTemplate.ROSTemplate.TemplateNameColumn.ColumnName] = model.TemplateName;
                if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToLower().Equals("mdvision"))
                {
                    dr[dsROSTemplate.ROSTemplate.EntityIdColumn.ColumnName] = model.EntityId;
                }
                else
                {
                    dr[dsROSTemplate.ROSTemplate.EntityIdColumn.ColumnName] = MDVUtility.ToLong(MDVSession.Current.EntityId);
                }

                dr[dsROSTemplate.ROSTemplate.IsActiveColumn.ColumnName] = true;
                dr[dsROSTemplate.ROSTemplate.CreatedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr[dsROSTemplate.ROSTemplate.CreatedOnColumn.ColumnName] = DateTime.Now;
                dr[dsROSTemplate.ROSTemplate.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr[dsROSTemplate.ROSTemplate.ModifiedOnColumn.ColumnName] = DateTime.Now;
                dsROSTemplate.ROSTemplate.AddROSTemplateRow(dr);
                BLObject<DSClinicalReviewofSystemTemplate> objNotes = BLLClinicalObj.insertClinical_ROSTemplate(dsROSTemplate);

                long rosTemplateId = 0;
                if (objNotes.Data != null)
                    rosTemplateId = MDVUtility.ToLong(dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName].Rows[0][dsROSTemplate.ROSTemplate.ROSTemplateIdColumn.ColumnName].ToString());

                if (objNotes.Data != null && rosTemplateId != -1)
                {
                    DataRow drUpdated = dsROSTemplate.Tables[dsROSTemplate.ROSTemplate.TableName].Rows[0];
                    responseModel.ROSTemplateId = MDVUtility.ToLong(drUpdated[dsROSTemplate.ROSTemplate.ROSTemplateIdColumn.ColumnName].ToString());
                    responseModel.status = true;
                    responseModel.Message = Common.AppPrivileges.Update_Message;

                }
                else
                {
                    responseModel.status = false;
                    responseModel.ROSTemplateId = model.ROSTemplateId;
                    responseModel.Message = rosTemplateId == -1 ? "A Template with this name already exists. Try a different name." : Common.AppPrivileges.Update_Error_Message;
                }
            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message =MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.ROSTemplateId = model.ROSTemplateId;

            }
            return responseModel;
        }

        public ROSTemplateResponse saveAsROSTemplate_Systems(ROSTemplateWrapperModel model, long ROSTemplateId)
        {
            ROSTemplateResponse responseModel = new ROSTemplateResponse();
            DSClinicalReviewofSystemTemplate dsSystemWrapper = new DSClinicalReviewofSystemTemplate();
            responseModel.status = true;
            try
            {
                if (ROSTemplateId > 0)
                {
                    DSClinicalReviewofSystemTemplate dsROSTemplateSaveAsInsert = new DSClinicalReviewofSystemTemplate();
                    #region update
                    DSClinicalReviewofSystemTemplate dsROSTemplateUpdate = null;
                    BLObject<DSClinicalReviewofSystemTemplate> obj = BLLClinicalObj.loadROSTemplateSystems(model.ROSTemplateId);
                    dsROSTemplateUpdate = obj.Data;
                    if (model.SystemsList != null && model.SystemsList.Count > 0)
                    {
                        if (dsROSTemplateUpdate.Tables[dsROSTemplateUpdate.ROSSystem.TableName].Rows.Count > 0)
                        {
                            int counter = 1;
                            foreach (DataRow dr in dsROSTemplateUpdate.Tables[dsROSTemplateUpdate.ROSSystem.TableName].Rows)
                            {
                                DSClinicalReviewofSystemTemplate.ROSSystemRow drInsert = dsROSTemplateSaveAsInsert.ROSSystem.NewROSSystemRow();
                                string ROSSystemId = dr[dsROSTemplateUpdate.ROSSystem.ROSSystemIdColumn].ToString();
                                ROSSystemsModel systemObj = model.SystemsList.FirstOrDefault(s => s.ROSSystemId == MDVUtility.ToLong(ROSSystemId));
                                if (systemObj != null)
                                {
                                    model.SystemsList.Remove(systemObj);
                                    drInsert[dsROSTemplateUpdate.ROSSystem.SystemNameColumn.ColumnName] = systemObj.SystemName;
                                    drInsert[dsROSTemplateUpdate.ROSSystem.SortingOrderColumn.ColumnName] = systemObj.SortingOrder;
                                    drInsert[dsROSTemplateUpdate.ROSSystem.ROSTemplateIdColumn.ColumnName] = ROSTemplateId;
                                    drInsert[dsROSTemplateSaveAsInsert.ROSSystem.ROSSystemIdColumn.ColumnName] = -counter;
                                    dsROSTemplateSaveAsInsert.ROSSystem.AddROSSystemRow(drInsert);
                                    counter++;
                                }

                            }
                            dsSystemWrapper.Merge(dsROSTemplateUpdate);
                        }
                    }

                    #endregion

                    #region insert/Delete
                    if (responseModel.status)
                    {
                        if (model.SystemsList != null && model.SystemsList.Count > 0)
                        {
                            DSClinicalReviewofSystemTemplate dsROSTemplateInsert = new DSClinicalReviewofSystemTemplate();
                            foreach (ROSSystemsModel systemObj in model.SystemsList)
                            {
                                DSClinicalReviewofSystemTemplate.ROSSystemRow dr = dsROSTemplateInsert.ROSSystem.NewROSSystemRow();

                                if (systemObj.ROSSystemId < 0)
                                {
                                    dr[dsROSTemplateInsert.ROSSystem.SystemNameColumn.ColumnName] = systemObj.SystemName;
                                    dr[dsROSTemplateInsert.ROSSystem.SortingOrderColumn.ColumnName] = systemObj.SortingOrder;
                                    dr[dsROSTemplateInsert.ROSSystem.ROSTemplateIdColumn.ColumnName] = ROSTemplateId;
                                    dsROSTemplateInsert.ROSSystem.AddROSSystemRow(dr);
                                }
                            }

                            dsROSTemplateSaveAsInsert.Merge(dsROSTemplateInsert);
                            obj = BLLClinicalObj.insertROSTemplateSystems(dsROSTemplateSaveAsInsert);
                            dsROSTemplateSaveAsInsert = obj.Data;
                            dsSystemWrapper.Merge(dsROSTemplateSaveAsInsert);
                        }
                        else
                        {
                            obj = BLLClinicalObj.insertROSTemplateSystems(dsROSTemplateSaveAsInsert);
                            dsROSTemplateSaveAsInsert = obj.Data;
                            //  dsSystemWrapper.Merge(dsROSTemplateSaveAsInsert);
                        }

                        if (obj.Data != null)
                        {
                            //*******************************
                            //******** insert update characteristics
                            var myDataInsert = dsROSTemplateSaveAsInsert.ROSSystem.AsEnumerable().Select(r => new ROSSystemsModel
                            {
                                SystemName = r.Field<string>("SystemName"),
                                SortingOrder = r.Field<int>("SortingOrder"),
                                ROSSystemId = r.Field<long>("ROSSystemId"),
                                ROSTemplateId = ROSTemplateId
                            });
                            var SystemsListInsert = myDataInsert.ToList(); // For if you really need a List and not IEnumerable
                            var myDataOld = dsSystemWrapper.ROSSystem.AsEnumerable().Select(r => new ROSSystemsModel
                            {
                                SystemName = r.Field<string>("SystemName"),
                                SortingOrder = r.Field<int>("SortingOrder"),
                                ROSSystemId = r.Field<long>("ROSSystemId"),
                                ROSTemplateId = ROSTemplateId
                            });
                            var SystemsListOld = myDataOld.ToList(); // For if you really need a List and not IEnumerable
                            foreach (ROSSystemsModel systemObj in SystemsListInsert)
                            {
                                ROSSystemsModel systemObjold = SystemsListOld.FirstOrDefault(n => n.SystemName.Equals(systemObj.SystemName));
                                responseModel = saveAsROSTemplate_SystemsCharc(model, ROSTemplateId, systemObj.ROSSystemId, systemObj.SystemName, systemObjold.ROSSystemId);
                            }


                            //***********************************
                            // responseModel.ROSTemplateId = model.ROSTemplateId;
                            responseModel.status = true;
                            responseModel.Message = Common.AppPrivileges.Update_Message;

                        }
                        else
                        {
                            responseModel.status = false;
                            responseModel.Message = Common.AppPrivileges.Update_Error_Message;
                        }
                    }
                    #endregion
                }
                else
                {

                    responseModel.status = false;
                    responseModel.Message = "ROS Template not found.";
                }
            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message =MDVCustomException.HumanReadableMessage(ex.Message);

            }
            return responseModel;
        }
        public ROSTemplateResponse saveAsROSTemplate_SystemsCharc(ROSTemplateWrapperModel model, long ROSTemplateId, long ROSSystemId, string SystemName, long oldROSSystemId)
        {
            ROSTemplateResponse responseModel = new ROSTemplateResponse();
            responseModel.status = true;
            try
            {
                DSClinicalReviewofSystemTemplate dsROSTemplateSaveAsInsert = new DSClinicalReviewofSystemTemplate();
                List<ROSTemptSystemCharCModel> CharacteristicsList = model.CharacteristicsList.Where(n => n.SystemName.Equals(SystemName)).ToList<ROSTemptSystemCharCModel>();// as List<ROSTemptSystemCharCModel>;
                if (ROSTemplateId > 0 && oldROSSystemId > 0 && CharacteristicsList != null && CharacteristicsList.Count > 0)
                {
                    #region update
                    DSClinicalReviewofSystemTemplate dsROSTemplate = null;
                    BLObject<DSClinicalReviewofSystemTemplate> obj = BLLClinicalObj.loadROSTemplateSystemsCharc(model.ROSTemplateId, oldROSSystemId);
                    dsROSTemplate = obj.Data;
                    List<long> charaDetachList = new List<long>();
                    if (model.CharacteristicsList != null && model.CharacteristicsList.Count > 0)
                    {
                        if (dsROSTemplate.Tables[dsROSTemplate.ROSSystemCharC.TableName].Rows.Count > 0)
                        {
                            int counter = 1;
                            foreach (DataRow dr in dsROSTemplate.Tables[dsROSTemplate.ROSSystemCharC.TableName].Rows)
                            {
                                DSClinicalReviewofSystemTemplate.ROSSystemCharCRow drInsert = dsROSTemplateSaveAsInsert.ROSSystemCharC.NewROSSystemCharCRow();
                                string ROSSystemCharacteristicsId = dr[dsROSTemplate.ROSSystemCharC.ROSSystemCharacteristicsIdColumn].ToString();
                                ROSTemptSystemCharCModel CharCObj = CharacteristicsList.FirstOrDefault(s => s.CharacteristicsId == MDVUtility.ToLong(ROSSystemCharacteristicsId));
                                if (CharCObj != null)
                                {
                                    CharacteristicsList.Remove(CharCObj);
                                    drInsert[dsROSTemplate.ROSSystemCharC.CharacteristicsNameColumn.ColumnName] = CharCObj.CharacteristicsName;
                                    drInsert[dsROSTemplate.ROSSystemCharC.SortingOrderColumn.ColumnName] = CharCObj.SortingOrder;
                                    drInsert[dsROSTemplate.ROSSystemCharC.ROSSystemIdColumn.ColumnName] = ROSSystemId;
                                    drInsert[dsROSTemplate.ROSSystemCharC.ROSSystemCharacteristicsIdColumn.ColumnName] = -counter;
                                    dsROSTemplateSaveAsInsert.ROSSystemCharC.AddROSSystemCharCRow(drInsert);
                                }

                                counter++;
                            }
                        }
                    }
                    #endregion
                    #region insert/Delete
                    if (responseModel.status)
                    {
                        if (model.CharacteristicsList != null && model.CharacteristicsList.Count > 0)
                        {
                            DSClinicalReviewofSystemTemplate dsROSTemplateInsert = new DSClinicalReviewofSystemTemplate();
                            foreach (ROSTemptSystemCharCModel CharCObj in CharacteristicsList)
                            {
                                DSClinicalReviewofSystemTemplate.ROSSystemCharCRow dr = dsROSTemplateInsert.ROSSystemCharC.NewROSSystemCharCRow();

                                if (CharCObj.CharacteristicsId < 0 || CharCObj.ROSSystemId < 0)
                                {
                                    dr[dsROSTemplate.ROSSystemCharC.CharacteristicsNameColumn.ColumnName] = CharCObj.CharacteristicsName;
                                    dr[dsROSTemplate.ROSSystemCharC.SortingOrderColumn.ColumnName] = CharCObj.SortingOrder;
                                    dr[dsROSTemplate.ROSSystemCharC.ROSSystemIdColumn.ColumnName] = ROSSystemId;
                                    dsROSTemplateInsert.ROSSystemCharC.AddROSSystemCharCRow(dr);
                                }
                            }
                            dsROSTemplateSaveAsInsert.Merge(dsROSTemplateInsert);
                            obj = BLLClinicalObj.insertROSTemplateSystemsCharc(dsROSTemplateSaveAsInsert);
                        }

                    }
                    #endregion

                    if (obj.Data != null)
                    {
                        responseModel.status = true;
                        responseModel.Message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        responseModel.status = false;
                        responseModel.Message = Common.AppPrivileges.Update_Error_Message;
                    }

                }
                else
                {
                    DSClinicalReviewofSystemTemplate dsROSTemplate = null;
                    BLObject<DSClinicalReviewofSystemTemplate> obj = BLLClinicalObj.loadROSTemplateSystemsCharc(model.ROSTemplateId, oldROSSystemId);
                    dsROSTemplate = obj.Data;
                    DSClinicalReviewofSystemTemplate dsROSTemplateInsert = new DSClinicalReviewofSystemTemplate();
                    if (dsROSTemplate.Tables[dsROSTemplate.ROSSystemCharC.TableName].Rows.Count > 0)
                    {
                        int counter = 1;
                        foreach (DataRow dr in dsROSTemplate.Tables[dsROSTemplate.ROSSystemCharC.TableName].Rows)
                        {
                            DSClinicalReviewofSystemTemplate.ROSSystemCharCRow drIns = dsROSTemplateInsert.ROSSystemCharC.NewROSSystemCharCRow();

                            drIns[dsROSTemplate.ROSSystemCharC.CharacteristicsNameColumn.ColumnName] = dr[dsROSTemplate.ROSSystemCharC.CharacteristicsNameColumn.ColumnName];
                            drIns[dsROSTemplate.ROSSystemCharC.SortingOrderColumn.ColumnName] = dr[dsROSTemplate.ROSSystemCharC.SortingOrderColumn.ColumnName];
                            drIns[dsROSTemplate.ROSSystemCharC.ROSSystemIdColumn.ColumnName] = ROSSystemId;
                            drIns[dsROSTemplate.ROSSystemCharC.ROSSystemCharacteristicsIdColumn.ColumnName] = -counter;
                            dsROSTemplateInsert.ROSSystemCharC.AddROSSystemCharCRow(drIns);
                            counter++;
                        }
                        obj = BLLClinicalObj.insertROSTemplateSystemsCharc(dsROSTemplateInsert);
                        if (obj.Data != null)
                        {
                            responseModel.status = true;
                        }
                        else
                        {
                            responseModel.status = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message =MDVCustomException.HumanReadableMessage(ex.Message);
            }
            return responseModel;
        }
        #endregion
        public string saveClinical_ROSTemplate(ROSTemplateWrapperModel model)
        {
            try
            {
                long ROSTemplateId = 0;
                ROSTemplateResponse responseModel = new ROSTemplateResponse();
                if (model.isSaveAS == true)
                {
                    responseModel = saveAsROSTemplateInfo(model);
                    ROSTemplateId = responseModel.ROSTemplateId;
                }
                else if (model.ROSTemplateId > 0)
                {
                    responseModel = updateROSTemplateInfo(model);
                    ROSTemplateId = model.ROSTemplateId;
                }
                else
                {
                    responseModel = insertROSTemplateInfo(model);
                    ROSTemplateId = responseModel.ROSTemplateId;
                    if (responseModel.status)
                    {
                        model.ROSTemplateId = MDVUtility.ToLong(responseModel.ROSTemplateId);
                    }

                }
                if (responseModel.status)
                {
                    if (model.isSaveAS == true)
                    {
                        //save as  for systems and characteristics
                        responseModel = saveAsROSTemplate_Systems(model, responseModel.ROSTemplateId);
                    }
                    else
                    {
                        responseModel = saveROSTemplate_Systems(model, responseModel.ROSTemplateId);
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = responseModel.Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                if (responseModel.status)
                {
                    var response = new
                    {
                        status = true,
                        ROSTemplateId = ROSTemplateId,
                        Message = Common.AppPrivileges.Update_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = responseModel.Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion


        #region Insert?update ros template system
        public ROSTemplateResponse saveROSTemplate_Systems(ROSTemplateWrapperModel model, long ROSTemplateId)
        {
            ROSTemplateResponse responseModel = new ROSTemplateResponse();
            DSClinicalReviewofSystemTemplate dsSystemWrapper = new DSClinicalReviewofSystemTemplate();
            responseModel.status = true;
            try
            {
                if (ROSTemplateId > 0)
                {
                    #region update
                    DSClinicalReviewofSystemTemplate dsROSTemplateUpdate = null;
                    BLObject<DSClinicalReviewofSystemTemplate> obj = BLLClinicalObj.loadROSTemplateSystems(ROSTemplateId);
                    dsROSTemplateUpdate = obj.Data;
                    List<long> systemDetachList = new List<long>();
                    if (model.SystemsList != null && model.SystemsList.Count > 0)
                    {
                        if (dsROSTemplateUpdate.Tables[dsROSTemplateUpdate.ROSSystem.TableName].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsROSTemplateUpdate.Tables[dsROSTemplateUpdate.ROSSystem.TableName].Rows)
                            {
                                string ROSSystemId = dr[dsROSTemplateUpdate.ROSSystem.ROSSystemIdColumn].ToString();
                                ROSSystemsModel systemObj = model.SystemsList.FirstOrDefault(s => s.ROSSystemId == MDVUtility.ToLong(ROSSystemId));
                                if (systemObj != null)
                                {
                                    model.SystemsList.Remove(systemObj);
                                    dr[dsROSTemplateUpdate.ROSSystem.SystemNameColumn.ColumnName] = systemObj.SystemName;
                                    dr[dsROSTemplateUpdate.ROSSystem.SortingOrderColumn.ColumnName] = systemObj.SortingOrder;
                                    dr[dsROSTemplateUpdate.ROSSystem.ROSTemplateIdColumn.ColumnName] = ROSTemplateId;
                                }
                                else
                                {
                                    systemDetachList.Add(MDVUtility.ToLong(ROSSystemId));
                                    //delete system from Template
                                }

                            }
                            BLObject<DSClinicalReviewofSystemTemplate> objUpdate = BLLClinicalObj.updateROSTemplateSystems(dsROSTemplateUpdate);
                            if (obj.Data != null)
                            {
                                dsROSTemplateUpdate = obj.Data;
                                dsSystemWrapper.Merge(dsROSTemplateUpdate);
                                responseModel.status = true;
                            }
                            else
                            {
                                responseModel.status = false;
                            }
                        }
                    }
                    else
                    {
                        if (dsROSTemplateUpdate.Tables[dsROSTemplateUpdate.ROSSystem.TableName].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsROSTemplateUpdate.Tables[dsROSTemplateUpdate.ROSSystem.TableName].Rows)
                            {
                                string ROSSystemId = dr[dsROSTemplateUpdate.ROSSystem.ROSSystemIdColumn].ToString();
                                systemDetachList.Add(MDVUtility.ToLong(ROSSystemId));
                            }
                        }
                    }
                    #endregion
                    #region insert/Delete
                    if (responseModel.status)
                    {
                        if (model.SystemsList != null && model.SystemsList.Count > 0)
                        {
                            DSClinicalReviewofSystemTemplate dsROSTemplateInsert = new DSClinicalReviewofSystemTemplate();
                            foreach (ROSSystemsModel systemObj in model.SystemsList)
                            {
                                DSClinicalReviewofSystemTemplate.ROSSystemRow dr = dsROSTemplateInsert.ROSSystem.NewROSSystemRow();

                                if (systemObj.ROSSystemId <= 0)
                                {
                                    dr[dsROSTemplateInsert.ROSSystem.SystemNameColumn.ColumnName] = systemObj.SystemName;
                                    dr[dsROSTemplateInsert.ROSSystem.SortingOrderColumn.ColumnName] = systemObj.SortingOrder;
                                    dr[dsROSTemplateInsert.ROSSystem.ROSTemplateIdColumn.ColumnName] = ROSTemplateId;
                                    dsROSTemplateInsert.ROSSystem.AddROSSystemRow(dr);
                                }
                            }

                            obj = BLLClinicalObj.insertROSTemplateSystems(dsROSTemplateInsert);
                            dsROSTemplateInsert = obj.Data;
                            dsSystemWrapper.Merge(dsROSTemplateInsert);
                        }
                        if (systemDetachList != null && systemDetachList.Count > 0)
                        {
                            foreach (long SystemId in systemDetachList)
                            {
                                BLObject<string> isDeleted = BLLClinicalObj.deleteROSTemplateSystems(ROSTemplateId, SystemId);
                                if (isDeleted.Data == "")
                                {
                                    responseModel.status = true;
                                }
                                else
                                {
                                    responseModel.status = false;
                                }
                            }
                        }
                        if (obj.Data != null)
                        {
                            //*******************************
                            //******** insert update characteristics
                            var myData = dsSystemWrapper.ROSSystem.AsEnumerable().Select(r => new ROSSystemsModel
                            {
                                SystemName = r.Field<string>("SystemName"),
                                SortingOrder = r.Field<int>("SortingOrder"),
                                ROSSystemId = r.Field<long>("ROSSystemId"),
                                ROSTemplateId = ROSTemplateId
                            });
                            var SystemsList = myData.ToList(); // For if you really need a List and not IEnumerable
                            foreach (ROSSystemsModel systemObj in SystemsList)
                            {
                                if (responseModel.status)
                                {
                                    responseModel = saveROSTemplate_SystemsCharc(model, ROSTemplateId, systemObj.ROSSystemId, systemObj.SystemName);
                                }
                                else
                                {
                                    return responseModel;
                                }
                            }


                            //***********************************
                            if (responseModel.status)
                            {
                                responseModel.ROSTemplateId = model.ROSTemplateId;
                                responseModel.status = true;
                                responseModel.Message = Common.AppPrivileges.Update_Message;
                            }

                        }
                        else
                        {
                            responseModel.status = false;
                            responseModel.ROSTemplateId = model.ROSTemplateId;
                            responseModel.Message = Common.AppPrivileges.Update_Error_Message;
                        }
                    }
                    #endregion



                }
                else
                {

                    responseModel.status = false;
                    responseModel.ROSTemplateId = model.ROSTemplateId;
                    responseModel.Message = "ROS Template not found.";

                }
            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message =MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.ROSTemplateId = model.ROSTemplateId;

            }
            return responseModel;
        }
        #endregion

        #region Insert?update ros template system Characteristics
        public ROSTemplateResponse saveROSTemplate_SystemsCharc(ROSTemplateWrapperModel model, long ROSTemplateId, long ROSSystemId, string SystemName)
        {
            ROSTemplateResponse responseModel = new ROSTemplateResponse();
            responseModel.status = true;
            try
            {
                List<ROSTemptSystemCharCModel> CharacteristicsList = model.CharacteristicsList.Where(n => n.SystemName.Equals(SystemName)).ToList<ROSTemptSystemCharCModel>();// as List<ROSTemptSystemCharCModel>;
                if (ROSTemplateId > 0 && ROSSystemId > 0 && CharacteristicsList != null && CharacteristicsList.Count > 0)
                {
                    #region update
                    DSClinicalReviewofSystemTemplate dsROSTemplate = null;
                    BLObject<DSClinicalReviewofSystemTemplate> obj = BLLClinicalObj.loadROSTemplateSystemsCharc(ROSTemplateId, ROSSystemId);
                    dsROSTemplate = obj.Data;
                    List<long> charaDetachList = new List<long>();
                    if (model.CharacteristicsList != null && model.CharacteristicsList.Count > 0)
                    {
                        if (dsROSTemplate.Tables[dsROSTemplate.ROSSystemCharC.TableName].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsROSTemplate.Tables[dsROSTemplate.ROSSystemCharC.TableName].Rows)
                            {
                                string ROSSystemCharacteristicsId = dr[dsROSTemplate.ROSSystemCharC.ROSSystemCharacteristicsIdColumn].ToString();
                                ROSTemptSystemCharCModel CharCObj = CharacteristicsList.FirstOrDefault(s => s.CharacteristicsId == MDVUtility.ToLong(ROSSystemCharacteristicsId));
                                if (CharCObj != null)
                                {
                                    CharacteristicsList.Remove(CharCObj);
                                    dr[dsROSTemplate.ROSSystemCharC.CharacteristicsNameColumn.ColumnName] = CharCObj.CharacteristicsName;
                                    dr[dsROSTemplate.ROSSystemCharC.SortingOrderColumn.ColumnName] = CharCObj.SortingOrder;
                                }
                                else
                                {
                                    charaDetachList.Add(MDVUtility.ToLong(ROSSystemCharacteristicsId));
                                    //delete system from Template
                                }

                            }
                            BLObject<DSClinicalReviewofSystemTemplate> objUpdate = BLLClinicalObj.updateROSTemplateSystemsCharc(dsROSTemplate);
                            if (obj.Data != null)
                            {
                                responseModel.status = true;
                            }
                            else
                            {
                                responseModel.status = false;
                            }
                        }
                    }
                    //else
                    //{
                    //    if (dsROSTemplate.Tables[dsROSTemplate.ROSSystemCharC.TableName].Rows.Count > 0)
                    //    {
                    //        foreach (DataRow dr in dsROSTemplate.Tables[dsROSTemplate.ROSSystemCharC.TableName].Rows)
                    //        {
                    //            string ROSSystemCharacteristicsId = dr[dsROSTemplate.ROSSystemCharC.ROSSystemCharacteristicsIdColumn].ToString();
                    //            charaDetachList.Add(MDVUtility.ToLong(ROSSystemCharacteristicsId));
                    //        }
                    //    }
                    //}
                    #endregion
                    #region insert/Delete
                    if (responseModel.status)
                    {
                        if (model.CharacteristicsList != null && model.CharacteristicsList.Count > 0)
                        {
                            DSClinicalReviewofSystemTemplate dsROSTemplateInsert = new DSClinicalReviewofSystemTemplate();
                            foreach (ROSTemptSystemCharCModel CharCObj in CharacteristicsList)
                            {
                                DSClinicalReviewofSystemTemplate.ROSSystemCharCRow dr = dsROSTemplateInsert.ROSSystemCharC.NewROSSystemCharCRow();

                                if (CharCObj.CharacteristicsId <= 0 || CharCObj.ROSSystemId <= 0)
                                {
                                    dr[dsROSTemplate.ROSSystemCharC.CharacteristicsNameColumn.ColumnName] = CharCObj.CharacteristicsName;
                                    dr[dsROSTemplate.ROSSystemCharC.SortingOrderColumn.ColumnName] = CharCObj.SortingOrder;
                                    dr[dsROSTemplate.ROSSystemCharC.ROSSystemIdColumn.ColumnName] = ROSSystemId;
                                    dsROSTemplateInsert.ROSSystemCharC.AddROSSystemCharCRow(dr);
                                }
                            }

                            obj = BLLClinicalObj.insertROSTemplateSystemsCharc(dsROSTemplateInsert);
                        }
                        if (charaDetachList != null && charaDetachList.Count > 0)
                        {
                            
                                foreach (long CharacteristicsId in charaDetachList)
                                {
                                    BLObject<string> isDeleted = BLLClinicalObj.deleteROSTemplateSystemsCharc(CharacteristicsId, ROSSystemId);
                                    if (isDeleted.Data == "")
                                    {
                                        responseModel.status = true;
                                    }
                                    else
                                    {
                                        responseModel.status = false;
                                        responseModel.Message = "You cannot detach charateristics which are used in Progress/Encounter Note.";
                                        return responseModel;
                                    }
                                }
                            
                        }
                        
                       
                    }
                    #endregion

                    if (obj.Data != null)
                    {
                        responseModel.ROSTemplateId = model.ROSTemplateId;
                        responseModel.status = true;
                        responseModel.Message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        responseModel.status = false;
                        responseModel.ROSTemplateId = model.ROSTemplateId;
                        responseModel.Message = Common.AppPrivileges.Update_Error_Message;
                    }

                }
                else
                {

                    //BLObject<string> isDeleted = BLLClinicalObj.deleteROSTemplateSystemsCharc(-1, ROSSystemId);
                    //if (isDeleted.Data == "")
                    //{
                    //    responseModel.status = true;
                    //}
                    //else
                    //{
                    //    responseModel.status = false;
                    //}
                    responseModel.status = true;
                }
            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message =MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.ROSTemplateId = model.ROSTemplateId;

            }
            return responseModel;
        }
        #endregion
        #region load ros template system

        public string loadROSTemplateSystems(long templateID)
        {
            try
            {
                DSClinicalReviewofSystemTemplate dsROSTemplate = null;
                BLObject<DSClinicalReviewofSystemTemplate> obj;
                obj = BLLClinicalObj.loadROSTemplateSystems(templateID);

                dsROSTemplate = obj.Data;
                if (obj.Data != null)
                {
                    if (dsROSTemplate.Tables[dsROSTemplate.ROSSystem.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SystemsCount = dsROSTemplate.Tables[dsROSTemplate.ROSSystem.TableName].Rows.Count,
                            Systems_JSON = MDVUtility.JSON_DataTable(dsROSTemplate.Tables[dsROSTemplate.ROSSystem.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SystemsCount = 0,
                            Systems_JSON = MDVUtility.JSON_DataTable(dsROSTemplate.Tables[dsROSTemplate.ROSSystem.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }

                else
                {
                    var response = new
                    {
                        status = true,
                        SystemsCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string loadROSTemplateSystemsCharc(long templateID, long ROSSystemId)
        {
            try
            {
                DSClinicalReviewofSystemTemplate dsROSTemplate = null;
                BLObject<DSClinicalReviewofSystemTemplate> obj;
                obj = BLLClinicalObj.loadROSTemplateSystemsCharc(templateID, ROSSystemId);

                dsROSTemplate = obj.Data;
                if (obj.Data != null)
                {
                    if (dsROSTemplate.Tables[dsROSTemplate.ROSSystemCharC.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CharcCount = dsROSTemplate.Tables[dsROSTemplate.ROSSystemCharC.TableName].Rows.Count,
                            SystemCharacteristics_JSON = MDVUtility.JSON_DataTable(dsROSTemplate.Tables[dsROSTemplate.ROSSystemCharC.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            CharcCount = 0,
                            SystemCharacteristics_JSON = MDVUtility.JSON_DataTable(dsROSTemplate.Tables[dsROSTemplate.ROSSystemCharC.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }

                else
                {
                    var response = new
                    {
                        status = true,
                        CharcCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        #endregion
    }
}