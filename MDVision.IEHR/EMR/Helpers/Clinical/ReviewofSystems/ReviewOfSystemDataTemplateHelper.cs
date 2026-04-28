
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
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems
{
    public class ReviewOfSystemDataTemplateHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public ReviewOfSystemDataTemplateHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static ReviewOfSystemDataTemplateHelper _instance = null;
        public static ReviewOfSystemDataTemplateHelper Instance()
        {
            if (_instance == null)
                _instance = new ReviewOfSystemDataTemplateHelper();
            return _instance;
        }

        /// Author: ZeeshanAK
        /// Purpose: To load Review Of Systems Templates
        /// Date : March 01, 2016
        #region Load ROS Templates
        public string loadROSDataTemplates(ROSDataTemplateWrapperModel model)
        {
            try
            {
                DSROSDataTemplate dsROSDataTemplate = null;
                BLObject<DSROSDataTemplate> obj;
                obj = BLLClinicalObj.loadROSDataTemplates(model.IsActive, model.ROSTemplateId, model.ROSDataTemplateId, MDVUtility.ToInt64(model.PageNumber), MDVUtility.ToInt64(model.RowsPerPage), MDVUtility.ToInt64(model.EntityId));

                dsROSDataTemplate = obj.Data;
                if (obj.Data != null)
                {
                    if (dsROSDataTemplate.Tables[dsROSDataTemplate.ROSDataTemplate.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ROSDataTemplateCount = dsROSDataTemplate.Tables[dsROSDataTemplate.ROSDataTemplate.TableName].Rows.Count,
                            iTotalDisplayRecords = dsROSDataTemplate.ROSDataTemplate.Rows[0][dsROSDataTemplate.ROSDataTemplate.RecordCountColumn.ColumnName],
                            ROSDataTemplateLoad_JSON = MDVUtility.JSON_DataTable(dsROSDataTemplate.Tables[dsROSDataTemplate.ROSDataTemplate.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ROSDataTemplateCount = 0,
                            iTotalDisplayRecords = 0,
                            ROSDataTemplateLoad_JSON = MDVUtility.JSON_DataTable(dsROSDataTemplate.Tables[dsROSDataTemplate.ROSDataTemplate.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ROSDataTemplateCount = 0,
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

        public string updateClinical_ROSDataTemplateIsActive(Int64 rosDataTemplateID, Int64 templateID, Int64 IsActive, Int64 EntityId)
        {
            try
            {
                if (rosDataTemplateID > 0)
                {

                    DSROSDataTemplate dsROSTemplate = null;
                    BLObject<DSROSDataTemplate> obj = BLLClinicalObj.loadROSDataTemplates(null, templateID, rosDataTemplateID, 1, 1, EntityId);
                    dsROSTemplate = obj.Data;
                    if (dsROSTemplate.Tables[dsROSTemplate.ROSDataTemplate.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsROSTemplate.Tables[dsROSTemplate.ROSDataTemplate.TableName].Rows[0];
                        dr[dsROSTemplate.ROSDataTemplate.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSROSDataTemplate> objNotes = BLLClinicalObj.updateClinical_ROSDataTemplate(dsROSTemplate);
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
                        Message = "ROS Data Template not found."
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

        public ROSDataTempResponseModel updateClinical_ROSDataTemplate(ROSDataTemplateWrapperModel model)
        {
            ROSDataTempResponseModel response = new ROSDataTempResponseModel();
            try
            {
                if (model.ROSDataTemplateId > 0)
                {
                    long EntityId = -1;
                    if (model.EntityId == null)
                    {
                        EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    }
                    else
                    {
                        EntityId = MDVUtility.ToInt64(model.EntityId);
                    }
                    DSROSDataTemplate dsROSTemplate = null;
                    BLObject<DSROSDataTemplate> obj = BLLClinicalObj.loadROSDataTemplates(null, model.ROSTemplateId, model.ROSDataTemplateId, 1, 1, EntityId);
                    dsROSTemplate = obj.Data;
                    if (dsROSTemplate.Tables[dsROSTemplate.ROSDataTemplate.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsROSTemplate.Tables[dsROSTemplate.ROSDataTemplate.TableName].Rows[0];
                        dr[dsROSTemplate.ROSDataTemplate.DataTemplateNameColumn.ColumnName] = model.DataTemplateName;
                        response.ROSTemplateId = MDVUtility.ToInt64(dr[dsROSTemplate.ROSDataTemplate.ROSTemplateIdColumn.ColumnName]);
                        if (response.ROSTemplateId != model.ROSTemplateId)
                        {
                            response.IsTemplateChanged = true;
                        }
                        else
                        {
                            response.IsTemplateChanged = false;
                        }
                        dr[dsROSTemplate.ROSDataTemplate.ROSTemplateIdColumn.ColumnName] = model.ROSTemplateId;
                        dr[dsROSTemplate.ROSDataTemplate.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr[dsROSTemplate.ROSDataTemplate.ModifiedOnColumn.ColumnName] = DateTime.Now;
                        dr[dsROSTemplate.ROSDataTemplate.ROSTemplateIdColumn.ColumnName] = model.ROSTemplateId;
                        dr[dsROSTemplate.ROSDataTemplate.EntityIdColumn.ColumnName] = EntityId;

                        BLObject<DSROSDataTemplate> objNotes = BLLClinicalObj.updateClinical_ROSDataTemplate(dsROSTemplate);

                        long rosDataTemplateId = 0;
                        if (objNotes.Data != null)
                            rosDataTemplateId = MDVUtility.ToInt64(dsROSTemplate.Tables[dsROSTemplate.ROSDataTemplate.TableName].Rows[0][objNotes.Data.ROSDataTemplate.ROSDataTemplateIdColumn.ColumnName].ToString());

                        if (objNotes.Data != null && rosDataTemplateId != -1)
                        {
                            dsROSTemplate = objNotes.Data;
                            dr = dsROSTemplate.Tables[dsROSTemplate.ROSDataTemplate.TableName].Rows[0];
                            response.ROSDataTemplateId = MDVUtility.ToInt64(dr[dsROSTemplate.ROSDataTemplate.ROSDataTemplateIdColumn.ColumnName].ToString());
                            response.ROSTemplateId = MDVUtility.ToInt64(dr[dsROSTemplate.ROSDataTemplate.ROSTemplateIdColumn.ColumnName].ToString());
                            response.Status = true;
                            response.Message = Common.AppPrivileges.Update_Message;

                        }
                        else
                        {
                            response.Status = false;
                            response.Message = rosDataTemplateId == -1 ? "A Data Template with this name already exists. Try a different name." : objNotes.Message;
                        }
                    }
                    else
                    {
                        response.Status = false;
                        response.Message = obj.Message;
                    }
                }
                else
                {
                    response.Status = false;
                    response.Message = "ROS Data Template not found.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = MDVCustomException.HumanReadableMessage(ex.Message);
            }
            return response;
        }

        internal string deleteClinical_ROSDataTemplate_CharcDetail(long rOSDataSystemCharcID,bool removeSystemCharcDetails)
        {
            try
            {
                if (rOSDataSystemCharcID <= 0 || removeSystemCharcDetails != true)
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
                    BLObject<string> obj = BLLClinicalObj.deleteRosDataCharacteristicsDetails(rOSDataSystemCharcID, removeSystemCharcDetails);
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
        /// Deletes the Clinical Notes.
        /// </summary>
        /// <param name="NotesID">The Notes identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string deleteClinical_ROSDataTemplate(long rosDataTemplateID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(rosDataTemplateID)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteClinical_ROSDataTemplate(MDVUtility.ToStr(rosDataTemplateID));
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

        public long saveAsROSDataTemplateInfo(DSROSDataTemplate dsROSDataTemplateWrapper, ROSDataTempInfoModel infoModel)
        {

            DSROSDataTemplate.ROSDataTempInfoRow drDataTempInfo = null;

            drDataTempInfo = dsROSDataTemplateWrapper.ROSDataTempInfo.NewROSDataTempInfoRow();


            drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSDataTempInfoIdColumn] = -1;

            if (!string.IsNullOrEmpty(infoModel.Comments))
            {
                drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.CommentsColumn] = infoModel.Comments;
            }
            else
            {
                drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.CommentsColumn] = DBNull.Value;
            }
            if (!string.IsNullOrEmpty(infoModel.Description))
            {
                drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.DescriptionColumn] = infoModel.Description;
            }
            else
            {
                drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.DescriptionColumn] = DBNull.Value;
            }
            if (infoModel.IsNormal)
            {
                drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.IsNormalColumn] = infoModel.IsNormal;
            }
            else
            {
                drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.IsNormalColumn] = DBNull.Value;
            }
            if (!string.IsNullOrEmpty(infoModel.ROSSystemDate))
            {
                drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSSystemDateColumn] = MDVUtility.ToDateTime(infoModel.ROSSystemDate);
            }
            else
            {
                drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSSystemDateColumn] = DBNull.Value;
            }

            if (infoModel.ROSDataTemplateId > 0)
            {
                drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSDataTemplateIdColumn] = infoModel.ROSDataTemplateId;
            }
            else
            {
                drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSDataTemplateIdColumn] = DBNull.Value;
            }


            //// if no details is found against CharacteristicId, it implies for new record
            if (dsROSDataTemplateWrapper.ROSDataTempInfo.Rows.Count < 1)
            {
                dsROSDataTemplateWrapper.ROSDataTempInfo.AddROSDataTempInfoRow(drDataTempInfo);
            }


            BLObject<DSROSDataTemplate> objDetails = new BLObject<DSROSDataTemplate>();

            objDetails = BLLClinicalObj.insertROSDataTempInfo(dsROSDataTemplateWrapper);
            dsROSDataTemplateWrapper = objDetails.Data;
            return MDVUtility.ToInt64(dsROSDataTemplateWrapper.ROSDataTempInfo.Rows[0][dsROSDataTemplateWrapper.ROSDataTempInfo.ROSDataTempInfoIdColumn]);
        }
        public ROSDataTempResponseModel saveAsROSDataTemplate_Systems(ROSDataTempInfoModel model, ROSDataTemplateWrapperModel wrapperModel)
        {
            ROSDataTempResponseModel response = new ROSDataTempResponseModel();
            try
            {
                if (wrapperModel.EntityId == null)
                {
                    wrapperModel.EntityId = MDVUtility.ToInt32(MDVSession.Current.EntityId);
                }
                else
                {
                    wrapperModel.EntityId = MDVUtility.ToInt32(wrapperModel.EntityId);
                }
                DSROSDataTemplate dsROSTemplate = new DSROSDataTemplate();
                DSROSDataTemplate.ROSDataTemplateSaveAsRow dr = dsROSTemplate.ROSDataTemplateSaveAs.NewROSDataTemplateSaveAsRow();

                dr[dsROSTemplate.ROSDataTemplateSaveAs.DataTemplateNameColumn.ColumnName] = model.DataTemplateName;
                dr[dsROSTemplate.ROSDataTemplateSaveAs.ROSTemplateIdColumn.ColumnName] = model.ROSTemplateId;
                dr[dsROSTemplate.ROSDataTemplateSaveAs.CopyFromROSDataTemplateIdColumn.ColumnName] = wrapperModel.ROSDataTemplateId;
                //dr[dsROSTemplate.ROSDataTemplateSaveAs.ROSDataTemplateIdColumn.ColumnName] = wrapperModel..ROSTemplateId;
                //dr[dsROSTemplate.ROSDataTemplateSaveAs.ROSSystemDateColumn.ColumnName] = wrapperModel.;

                dr[dsROSTemplate.ROSDataTemplateSaveAs.CommentsColumn.ColumnName] = model.Comments;
                dr[dsROSTemplate.ROSDataTemplateSaveAs.EntityIdColumn.ColumnName] = wrapperModel.EntityId;
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                if (wrapperModel.ROSSystemInfoID > 0)
                {
                    dr.ROSSystemInfoId = wrapperModel.ROSSystemInfoID;
                    dr[dsROSTemplate.ROSDataTemplateSaveAs.CopyFromROSDataTemplateIdColumn.ColumnName] = DBNull.Value;
                }
                else
                {
                    dr[dsROSTemplate.ROSDataTemplateSaveAs.ROSSystemInfoIdColumn.ColumnName] = DBNull.Value;// wrapperModel.EntityId;

                }

                //// if no details is found against CharacteristicId, it implies for new record
                if (dsROSTemplate.ROSDataTemplateSaveAs.Rows.Count < 1)
                {
                    dsROSTemplate.ROSDataTemplateSaveAs.AddROSDataTemplateSaveAsRow(dr);
                }
                BLObject<DSROSDataTemplate> objROSDataTemp = BLLClinicalObj.saveAsROSDataTemplate(dsROSTemplate, wrapperModel.ROSSystemInfoID);

                if (objROSDataTemp.Data != null)
                {
                    dsROSTemplate = objROSDataTemp.Data;
                    response.ROSDataTemplateId = MDVUtility.ToInt64(dr[dsROSTemplate.ROSDataTemplateSaveAs.ROSDataTemplateIdColumn.ColumnName].ToString());
                    response.ROSTemplateId = MDVUtility.ToInt64(dr[dsROSTemplate.ROSDataTemplateSaveAs.ROSTemplateIdColumn.ColumnName].ToString());
                    response.ROSDataTempInfoId = MDVUtility.ToInt64(dr[dsROSTemplate.ROSDataTemplateSaveAs.ROSDataTempInfoIdColumn.ColumnName].ToString());
                    response.Status = true;
                    response.Message = Common.AppPrivileges.Update_Message;

                }
                else
                {
                    response.Status = false;
                    response.Message = objROSDataTemp.Message;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = MDVCustomException.HumanReadableMessage(ex.Message);
            }
            return response;


        }
        public void saveAsROSTemplate_SystemsCharc(ROSSystemsWrapperModel wrapperModel, DSROSDataTemplate dsROSDataTemplateWrapper, long ROSDataTemplateId)
        {
            BLObject<DSROSDataTemplate> objROSSystemPatientWrapper = null;
            if (wrapperModel.ROSSystemInfoID > 0)
            {
                if (dsROSDataTemplateWrapper.ROSDataSystem.Rows.Count > 0 && ((wrapperModel.charcNegCharacteristics != null && wrapperModel.charcNegCharacteristics.Count > 0) || (wrapperModel.charcPosCharacteristics != null && wrapperModel.charcPosCharacteristics.Count > 0) || (wrapperModel.SystemsWithDetails != null && wrapperModel.SystemsWithDetails.Count > 0)))
                {
                    objROSSystemPatientWrapper = BLLClinicalObj.loadROSDataSystemCharacteristics(ROSDataTemplateId, 0, -1);//model.ROSSystemPatientID);//wrapperModel.rosSystemPatientID);//.loadROSPatientInfo(MDVUtility.ToLong(wrapperModel.PatientId), wrapperModel.ROSSystemInfoID);
                    DSROSDataTemplate dsROSSystemPatientCharac = objROSSystemPatientWrapper.Data;
                    #region ROSSystemPatientCharacteristics

                    #region for loop for -ve and +ve charc
                    List<ROSSystemPatientCharacteristicsModel> sysPatientCharcModelList = new List<ROSSystemPatientCharacteristicsModel>();
                    for (int i = 0; i < wrapperModel.charcNegCharacteristics.Count; i++)
                    {
                        ROSSystemPatientCharacteristicsModel sysPatientCharcModel = new ROSSystemPatientCharacteristicsModel();
                        int charcNegCharacteristicsId = wrapperModel.charcNegCharacteristics[i] != null ? MDVUtility.ToInt32(wrapperModel.charcNegCharacteristics[i]) : 0;
                        int charcNegSystemId = wrapperModel.charcNegSystem[i] != null ? MDVUtility.ToInt32(wrapperModel.charcNegSystem[i]) : 0;
                        sysPatientCharcModel.CharacteristicsId = MDVUtility.ToInt32(charcNegCharacteristicsId);//wrapperModel.charcNegCharacteristics
                        if (wrapperModel.charcDescNeg != null && wrapperModel.charcNegSystem.Count > i && wrapperModel.charcDescNeg[i] != null)
                        {
                            sysPatientCharcModel.Description = wrapperModel.charcDescNeg[i];
                        }
                        if (wrapperModel.AllPositiveSystems.Any(s => charcNegSystemId.ToString().Contains(s)))
                        {
                            sysPatientCharcModel.IsPositive = true;
                        }
                        else
                        {
                            sysPatientCharcModel.IsPositive = false;
                        }

                        sysPatientCharcModel.ROSSystemCharacteristicsId = -1;
                        sysPatientCharcModel.ROSSystemPatientCharacteristicsID = -1;
                        // sysPatientCharcModel.ROSSystemPatientID = 0;
                        sysPatientCharcModel.CharcName = wrapperModel.charcNegName[i];
                        sysPatientCharcModel.SystemID = charcNegSystemId;
                        foreach (DSROSDataTemplate.ROSDataSystemRow drSystemPatient in dsROSDataTemplateWrapper.ROSDataSystem.Rows)
                        {
                            string SysId = drSystemPatient[dsROSDataTemplateWrapper.ROSDataSystem.ROSSystemIdColumn].ToString();
                            if (MDVUtility.ToInt32(SysId) == sysPatientCharcModel.SystemID)
                            {
                                sysPatientCharcModel.ROSSystemPatientID = MDVUtility.ToLong(drSystemPatient[dsROSDataTemplateWrapper.ROSDataSystem.ROSDataSystemIdColumn].ToString());
                                sysPatientCharcModelList.Add(sysPatientCharcModel);
                                break;
                            }
                            else
                            {
                                // sysPatientCharcModel.ROSSystemPatientID = -i + 1;
                            }
                        }

                    }
                    for (int i = 0; i < wrapperModel.charcPosCharacteristics.Count; i++)
                    {
                        ROSSystemPatientCharacteristicsModel sysPatientCharcModel = new ROSSystemPatientCharacteristicsModel();
                        int charcPosCharacteristicsId = wrapperModel.charcPosCharacteristics[i] != null ? MDVUtility.ToInt32(wrapperModel.charcPosCharacteristics[i]) : 0;
                        int charcPosSystemId = wrapperModel.charcPosSystem[i] != null ? MDVUtility.ToInt32(wrapperModel.charcPosSystem[i]) : 0;
                        sysPatientCharcModel.CharacteristicsId = MDVUtility.ToInt32(charcPosCharacteristicsId);//wrapperModel.charcNegCharacteristics
                        if (wrapperModel.charcDescPos != null && wrapperModel.charcDescPos[i] != null)
                        {
                            sysPatientCharcModel.Description = wrapperModel.charcDescPos[i];
                        }
                        sysPatientCharcModel.CharcName = wrapperModel.charcPosName[i];
                        if (wrapperModel.AllNegativeSystems.Any(s => charcPosSystemId.ToString().Contains(s)))
                        {
                            sysPatientCharcModel.IsPositive = false;
                        }
                        else
                        {
                            sysPatientCharcModel.IsPositive = true;
                        }
                        // sysPatientCharcModel.IsPositive = true;
                        sysPatientCharcModel.ROSSystemCharacteristicsId = -1;
                        sysPatientCharcModel.ROSSystemPatientCharacteristicsID = -1;
                        sysPatientCharcModel.SystemID = charcPosSystemId;

                        foreach (DSROSDataTemplate.ROSDataSystemRow drSystemPatient in dsROSDataTemplateWrapper.ROSDataSystem.Rows)
                        {
                            string SysId = drSystemPatient[dsROSDataTemplateWrapper.ROSDataSystem.ROSSystemIdColumn].ToString();
                            if (MDVUtility.ToInt32(SysId) == sysPatientCharcModel.SystemID)
                            {
                                sysPatientCharcModel.ROSSystemPatientID = MDVUtility.ToLong(drSystemPatient[dsROSDataTemplateWrapper.ROSDataSystem.ROSDataSystemIdColumn].ToString());
                                sysPatientCharcModelList.Add(sysPatientCharcModel);
                                break;
                            }
                            else
                            {
                                // sysPatientCharcModel.ROSSystemPatientID = -i + 1;
                            }
                        }


                    }

                    #endregion

                    #region update  ROSSystemPatient Characteristics
                    if (dsROSSystemPatientCharac != null && dsROSSystemPatientCharac.ROSDataSystemCharc.Rows.Count > 0)
                    {

                        List<string> delROSSystemCharac = new List<string>();
                        foreach (DSROSDataTemplate.ROSDataSystemCharcRow drROSDataSystemCharc in dsROSSystemPatientCharac.ROSDataSystemCharc.Rows)
                        {
                            if (sysPatientCharcModelList.Count == 0)
                            {

                                break;
                            }
                            string ROSDataSystemCharcId = drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSDataSystemCharcIDColumn].ToString();
                            string ROSDataSystemID = drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSDataSystemIDColumn].ToString();
                            string ROSSystemCharacteristicsId = drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn].ToString();
                            ROSSystemPatientCharacteristicsModel model = sysPatientCharcModelList.FirstOrDefault(n => n.ROSSystemPatientID == MDVUtility.ToLong(ROSDataSystemID) && n.CharacteristicsId == MDVUtility.ToLong(ROSSystemCharacteristicsId));
                            if (model == null)
                            {
                                //foreach (var item in wrapperModel.SystemsWithDetails)
                                //{
                                //    if (wrapperModel.NormalSystems.Any(s => ROSSystemPatientID.Contains(s)))
                                //    {
                                //    }
                                //}
                                // delROSSystemCharac.Add(ROSSystemPatientCharacteristicsId);
                                //drROSSystemPatientCharacteristics[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.DescriptionColumn] = DBNull.Value;
                                //drROSSystemPatientCharacteristics[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.IsPositiveColumn] = DBNull.Value;
                            }
                            else
                            {
                                sysPatientCharcModelList.Remove(model);
                                if (model.ROSSystemPatientCharacteristicsID > 0)
                                {
                                    drROSDataSystemCharc.ROSDataSystemCharcID = MDVUtility.ToInt64(model.ROSSystemPatientCharacteristicsID);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(ROSDataSystemCharcId))
                                    {
                                        drROSDataSystemCharc.ROSDataSystemCharcID = MDVUtility.ToInt64(ROSDataSystemCharcId);
                                    }
                                    else
                                    {
                                        drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSDataSystemCharcIDColumn] = DBNull.Value;
                                    }

                                }

                                if (model.ROSSystemPatientID > 0)
                                {
                                    drROSDataSystemCharc.ROSDataSystemID = MDVUtility.ToInt64(model.ROSSystemPatientID);
                                }
                                else
                                {
                                    drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSDataSystemIDColumn] = DBNull.Value;
                                }

                                //if (model.ROSSystemCharacteristicsId > 0)
                                //{
                                //    drROSDataSystemCharc.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.ROSSystemCharacteristicsId);
                                //}
                                //else
                                //{
                                //    drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn] = DBNull.Value;
                                //}
                                if (model.ROSSystemCharacteristicsId > 0)
                                {
                                    drROSDataSystemCharc.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.ROSSystemCharacteristicsId);
                                }
                                else
                                {
                                    if (model.CharacteristicsId > 0)
                                    {
                                        drROSDataSystemCharc.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.CharacteristicsId);
                                    }
                                    else
                                    {
                                        drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn] = DBNull.Value;
                                    }

                                }
                                if (!string.IsNullOrEmpty(model.Description))
                                {
                                    drROSDataSystemCharc.Description = MDVUtility.ToStr(model.Description);
                                }
                                else
                                {
                                    drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.DescriptionColumn] = DBNull.Value;
                                }

                                //if (!string.IsNullOrEmpty(model.CharcName))
                                //{
                                //    drROSDataSystemCharc.CharacteristicsName = MDVUtility.ToStr(model.CharcName);
                                //}
                                //else
                                //{
                                //    drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.CharacteristicsNameColumn] = DBNull.Value;
                                //}

                                drROSDataSystemCharc.IsPositive = model.IsPositive;
                                // dsROSSystemPatientWrapper.ROSDataSystemCharc.AddROSDataSystemCharcRow(dr);

                            }
                        }
                        //if (delROSSystemCharac != null && delROSSystemCharac.Count > 0)
                        //{
                        //    foreach (var item in delROSSystemCharac)
                        //    {
                        //        if (!string.IsNullOrEmpty(item))
                        //        {
                        //            BLLClinicalObj.deleteROSSystemPatientCharacteristics(MDVUtility.ToLong(item));
                        //        }

                        //    }

                        //}
                        if (sysPatientCharcModelList.Count == 0)
                        {
                            BLObject<DSROSDataTemplate> obj = BLLClinicalObj.updateROSDataSystemCharacteristics(dsROSSystemPatientCharac);
                            //delROSSystemCharac
                            dsROSSystemPatientCharac = obj.Data;

                        }
                    }
                    #endregion
                    #region insert  ROSSystemPatient Characteristics
                    // else
                    // {
                    if (sysPatientCharcModelList.Count > 0)
                    {


                        foreach (ROSSystemPatientCharacteristicsModel model in sysPatientCharcModelList)
                        {
                            DSROSDataTemplate.ROSDataSystemCharcRow dr = dsROSSystemPatientCharac.ROSDataSystemCharc.NewROSDataSystemCharcRow();
                            if (model.ROSSystemPatientCharacteristicsID > 0)
                            {
                                dr.ROSDataSystemCharcID = MDVUtility.ToInt64(model.ROSSystemPatientCharacteristicsID);
                            }
                            else
                            {
                                dr[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSDataSystemCharcIDColumn] = DBNull.Value;
                            }

                            if (model.ROSSystemPatientID > 0)
                            {
                                dr.ROSDataSystemID = MDVUtility.ToInt64(model.ROSSystemPatientID);
                            }
                            else
                            {
                                dr[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSDataSystemIDColumn] = DBNull.Value;
                            }
                            if (model.ROSSystemCharacteristicsId > 0)
                            {
                                dr.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.ROSSystemCharacteristicsId);
                            }
                            else
                            {
                                if (model.CharacteristicsId > 0)
                                {
                                    dr.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.CharacteristicsId);
                                }
                                else
                                {
                                    dr[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn] = DBNull.Value;
                                }

                            }

                            if (!string.IsNullOrEmpty(model.Description))
                            {
                                dr.Description = MDVUtility.ToStr(model.Description);
                            }
                            else
                            {
                                dr[dsROSSystemPatientCharac.ROSDataSystemCharc.DescriptionColumn] = DBNull.Value;
                            }
                            //if (!string.IsNullOrEmpty(model.CharcName))
                            //{
                            //    dr.CharacteristicsName = MDVUtility.ToStr(model.CharcName);
                            //}
                            //else
                            //{
                            //    dr[dsROSSystemPatientCharac.ROSDataSystemCharc.CharacteristicsNameColumn] = DBNull.Value;
                            //}
                            dr.IsPositive = model.IsPositive;
                            dsROSSystemPatientCharac.ROSDataSystemCharc.AddROSDataSystemCharcRow(dr);
                        }
                        BLObject<DSROSDataTemplate> objIns = BLLClinicalObj.insertROSDataSystemCharacteristics(dsROSSystemPatientCharac);
                        dsROSSystemPatientCharac = objIns.Data;
                        objIns = BLLClinicalObj.loadROSDataSystemCharacteristics(ROSDataTemplateId, 0, 0);//model.ROSSystemPatientID);//wrapperModel.rosSystemPatientID);//.loadROSPatientInfo(MDVUtility.ToLong(wrapperModel.PatientId), wrapperModel.ROSSystemInfoID);
                        dsROSSystemPatientCharac = objIns.Data;
                    }
                    else
                    {
                        //BLObject<DSROSDataTemplate> objIns = BLLClinicalObj.insertROSDataSystemSaveAsCharacteristics();
                    }
                    #endregion
                    if (dsROSSystemPatientCharac != null)
                    {

                        dsROSDataTemplateWrapper.Merge(dsROSSystemPatientCharac);
                    }
                    //  }
                    #endregion

                    #region charc details update
                    if (dsROSSystemPatientCharac != null)
                    {
                        foreach (DSROSDataTemplate.ROSDataSystemCharcRow drROSDataSystemCharc in dsROSDataTemplateWrapper.ROSDataSystemCharc.Rows)
                        {
                            //if (objCharacteristicsDetails.ROSCharacteristicsId == drROSDataSystemCharc.ROSSystemCharacteristicsId)
                            //{
                            //    systemPatientCharacteristicsId = drROSDataSystemCharc.ROSDataSystemCharcID;

                            Int64 ROSSystemCharacteristicsId = MDVUtility.ToInt64(drROSDataSystemCharc[dsROSDataTemplateWrapper.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn.ColumnName]);
                            Int64 ROSDataSystemID = MDVUtility.ToInt64(drROSDataSystemCharc[dsROSDataTemplateWrapper.ROSDataSystemCharc.ROSDataSystemIDColumn.ColumnName]);
                            //Int64 ROSDataSystemCharcID = MDVUtility.ToInt64(drROSDataSystemCharc[dsROSSystemPatientWrapper.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn.ColumnName]);

                            if (ROSSystemCharacteristicsId > 0)
                            {
                                Int64 ROSCharacteristicsDetailsId = 0;
                                DSROSDataTemplate responseCharacteristicsDetails = new DSROSDataTemplate();
                                if (drROSDataSystemCharc[dsROSDataTemplateWrapper.ROSDataSystemCharc.DetailsIDColumn.ColumnName] != DBNull.Value)
                                {
                                    ROSCharacteristicsDetailsId = MDVUtility.ToInt64(drROSDataSystemCharc[dsROSDataTemplateWrapper.ROSDataSystemCharc.DetailsIDColumn.ColumnName]);
                                    BLObject<DSROSDataTemplate> objDetail = BLLClinicalObj.loadROSDataSystemCharacDetails(0, ROSCharacteristicsDetailsId);
                                    responseCharacteristicsDetails = objDetail.Data;
                                }

                                if (wrapperModel.rosPatientCharcObjList != null && wrapperModel.rosPatientCharcObjList.Count > 0)
                                {

                                    if (wrapperModel.rosPatientCharcObjList.Any(s => s.ROSCharacteristicsId == ROSSystemCharacteristicsId))
                                    {
                                        List<ROSCharacteristicsDetailsModel> modelObjList = wrapperModel.rosPatientCharcObjList.Where(s => s.ROSCharacteristicsId == ROSSystemCharacteristicsId).ToList<ROSCharacteristicsDetailsModel>();

                                        //responseCharacteristicsDetails = insertUpdateCharacteristicsDetails(ROSDataSystemCharcID, wrapperModel.rosPatientCharcObjList, dsROSSystemPatientWrapper);
                                        responseCharacteristicsDetails = insertUpdateCharacteristicsDetails(ROSSystemCharacteristicsId, modelObjList, dsROSDataTemplateWrapper);
                                        wrapperModel.rosPatientCharcObjList.RemoveAll(s => s.ROSCharacteristicsId == ROSSystemCharacteristicsId);
                                    }
                                    else if (wrapperModel.rosPatientCharcObjList.Any(s => s.ROSCharacteristicsId == -1))
                                    {
                                        List<ROSCharacteristicsDetailsModel> modelObjList = wrapperModel.rosPatientCharcObjList.Where(s => s.ROSCharacteristicsId == -1).ToList<ROSCharacteristicsDetailsModel>();
                                        responseCharacteristicsDetails = insertUpdateCharacteristicsDetails(ROSSystemCharacteristicsId, modelObjList, dsROSDataTemplateWrapper);
                                        wrapperModel.rosPatientCharcObjList.RemoveAll(s => s.ROSCharacteristicsId == -1);
                                    }

                                    if (responseCharacteristicsDetails != null)
                                    {
                                        dsROSDataTemplateWrapper.Merge(responseCharacteristicsDetails);
                                    }
                                }
                                else
                                {
                                    dsROSDataTemplateWrapper.Merge(responseCharacteristicsDetails);
                                }

                                //foreach (DSClinicalReviewofSystem.ROSSystemInfoRow drROSSystemInfo in dsROSSystemPatientWrapper.ROSSystemInfo.Rows)
                                //{
                                //    drROSSystemInfo.SoapText = soapText;
                                //}
                                //   objDetails = BLLClinicalObj.updateROSPatientInfo(dsROSSystemPatientWrapper);
                                // dsROSSystemPatientWrapper = objDetails.Data;

                            }
                        }
                    }
                    #endregion
                }

            }
        }

        #region new change for save
        public string saveAsROSDataTemplateInfo(ROSDataTemplateWrapperModel DataTemplateWrapperModel)
        {

            ROSDataTempResponseModel responseModel = new ROSDataTempResponseModel();
            try
            {
                ROSSystemsWrapperModel wrapperModel = DataTemplateWrapperModel.SystemsWrapperModel;
                DSROSDataTemplate dsROSDataTemplateWrapper = new DSROSDataTemplate();
                #region ROSDataTemp Info
                ROSDataTempInfoModel infoModel = new ROSDataTempInfoModel();
                infoModel.Comments = wrapperModel.ROSComments;
                infoModel.Description = wrapperModel.ROSNormalDescription;
                infoModel.IsNormal = wrapperModel.ROSisNormal;
                infoModel.ROSSystemDate = wrapperModel.ReviewofSystemsDate;
                infoModel.DataTemplateName = DataTemplateWrapperModel.DataTemplateName;
                infoModel.ROSDataTempInfoId = wrapperModel.ROSSystemInfoID < 0 ? DataTemplateWrapperModel.ROSDataTempInfoId : wrapperModel.ROSSystemInfoID;
                infoModel.ROSDataTemplateId = DataTemplateWrapperModel.ROSDataTemplateId;
                infoModel.ROSTemplateId = DataTemplateWrapperModel.ROSTemplateId;
                //infoModel.ROSDataTempInfoId = saveAsROSDataTemplateInfo(dsROSDataTemplateWrapper, infoModel);

                // MDVUtility.ToLong(drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSDataTempInfoIdColumn]);
                responseModel = saveAsROSDataTemplate_Systems(infoModel, DataTemplateWrapperModel);
                if (responseModel.Status)
                {
                    wrapperModel.ROSSystemInfoID = responseModel.ROSDataTempInfoId;
                    wrapperModel.ROSTemplateId = responseModel.ROSTemplateId;
                    wrapperModel.ROSDataTemplateId = responseModel.ROSDataTemplateId;
                    DataTemplateWrapperModel.ROSDataTemplateId = wrapperModel.ROSDataTemplateId;
                    DataTemplateWrapperModel.ROSTemplateId = wrapperModel.ROSTemplateId;
                    DataTemplateWrapperModel.ROSDataTempInfoId = wrapperModel.ROSSystemInfoID;
                    DataTemplateWrapperModel.SystemsWrapperModel = wrapperModel;
                    return saveROSDataTemplateInfo(DataTemplateWrapperModel, responseModel);
                }

                #endregion
                var response = new
                {
                    status = responseModel.Status,
                    Message = responseModel.Message
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

        public string saveAsROSDataTemplate(ROSDataTemplateWrapperModel DataTemplateWrapperModel)
        {

            ROSDataTempResponseModel responseModel = new ROSDataTempResponseModel();
            try
            {
                ROSSystemsWrapperModel wrapperModel = DataTemplateWrapperModel.SystemsWrapperModel;
                DSROSDataTemplate dsROSDataTemplateWrapper = new DSROSDataTemplate();
                #region ROSDataTemp Info
                ROSDataTempInfoModel infoModel = new ROSDataTempInfoModel();
                infoModel.Comments = string.IsNullOrEmpty(wrapperModel.ROSComments) ? DataTemplateWrapperModel.ROSComments : wrapperModel.ROSComments;
                infoModel.Description = wrapperModel.ROSNormalDescription;
                infoModel.IsNormal = wrapperModel.ROSisNormal;
                infoModel.ROSSystemDate = wrapperModel.ReviewofSystemsDate;
                infoModel.DataTemplateName = DataTemplateWrapperModel.DataTemplateName;
                // infoModel.ROSDataTempInfoId = wrapperModel.ROSSystemInfoID < 0 ? DataTemplateWrapperModel.ROSDataTempInfoId : wrapperModel.ROSSystemInfoID;
                infoModel.ROSDataTemplateId = DataTemplateWrapperModel.ROSDataTemplateId;
                infoModel.ROSTemplateId = DataTemplateWrapperModel.ROSTemplateId;
                //infoModel.ROSDataTempInfoId = saveAsROSDataTemplateInfo(dsROSDataTemplateWrapper, infoModel);

                // MDVUtility.ToLong(drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSDataTempInfoIdColumn]);
                responseModel = saveAsROSDataTemplate_Systems(infoModel, DataTemplateWrapperModel);
                if (responseModel.Status)
                {
                    wrapperModel.ROSSystemInfoID = responseModel.ROSDataTempInfoId;
                    wrapperModel.ROSTemplateId = responseModel.ROSTemplateId;
                    wrapperModel.ROSDataTemplateId = responseModel.ROSDataTemplateId;
                    DataTemplateWrapperModel.ROSDataTemplateId = wrapperModel.ROSDataTemplateId;
                    DataTemplateWrapperModel.ROSTemplateId = wrapperModel.ROSTemplateId;
                    DataTemplateWrapperModel.ROSDataTempInfoId = wrapperModel.ROSSystemInfoID;
                    DataTemplateWrapperModel.SystemsWrapperModel = wrapperModel;
                    return saveROSDataTemplateInfo(DataTemplateWrapperModel, responseModel);
                }

                #endregion
                var response = new
                {
                    status = responseModel.Status,
                    Message = responseModel.Message
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

        public ROSDataTempResponseModel saveClinical_ROSDataTemplate(ROSDataTemplateWrapperModel model)
        {
            ROSDataTempResponseModel response = new ROSDataTempResponseModel();
            try
            {
                long EntityId = -1;
                if (model.EntityId == null)
                {
                    EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                }
                else
                {
                    EntityId = MDVUtility.ToInt64(model.EntityId);
                }
                DSROSDataTemplate dsROSTemplate = new DSROSDataTemplate();
                DSROSDataTemplate.ROSDataTemplateRow dr = dsROSTemplate.ROSDataTemplate.NewROSDataTemplateRow();

                dr[dsROSTemplate.ROSDataTemplate.DataTemplateNameColumn.ColumnName] = model.DataTemplateName;
                dr[dsROSTemplate.ROSDataTemplate.ROSTemplateIdColumn.ColumnName] = model.ROSTemplateId;
                dr[dsROSTemplate.ROSDataTemplate.EntityIdColumn.ColumnName] = EntityId;
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                //// if no details is found against CharacteristicId, it implies for new record
                if (dsROSTemplate.ROSDataTemplate.Rows.Count < 1)
                {
                    dsROSTemplate.ROSDataTemplate.AddROSDataTemplateRow(dr);
                }
                BLObject<DSROSDataTemplate> objNotes = BLLClinicalObj.insertClinical_ROSDataTemplate(dsROSTemplate);

                long rosDataTemplateId = 0;
                if (objNotes.Data != null)
                    rosDataTemplateId = MDVUtility.ToInt64(dr[dsROSTemplate.ROSDataTemplate.ROSDataTemplateIdColumn.ColumnName].ToString());

                if (objNotes.Data != null && rosDataTemplateId != -1)
                {
                    dsROSTemplate = objNotes.Data;
                    response.ROSDataTemplateId = MDVUtility.ToInt64(dr[dsROSTemplate.ROSDataTemplate.ROSDataTemplateIdColumn.ColumnName].ToString());
                    response.ROSTemplateId = MDVUtility.ToInt64(dr[dsROSTemplate.ROSDataTemplate.ROSTemplateIdColumn.ColumnName].ToString());
                    response.Status = true;
                    response.Message = Common.AppPrivileges.Update_Message;

                }
                else
                {
                    response.Status = false;
                    response.Message = rosDataTemplateId == -1 ? "A Data Template with this name already exists. Try a different name." : objNotes.Message;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = MDVCustomException.HumanReadableMessage(ex.Message);
            }
            return response;
        }

        #region azhar implementations
        public string saveROSDataTemplateInfo(ROSDataTemplateWrapperModel DataTemplateWrapperModel, ROSDataTempResponseModel responseModel = null)
        {

            responseModel = responseModel == null ? new ROSDataTempResponseModel() : responseModel;
            try
            {
                if (DataTemplateWrapperModel.ROSDataTemplateId <= 0)
                {
                    responseModel = saveClinical_ROSDataTemplate(DataTemplateWrapperModel);
                }
                else
                {
                    if (DataTemplateWrapperModel.isSaveAS == false)
                    {
                        responseModel = updateClinical_ROSDataTemplate(DataTemplateWrapperModel);
                    }
                    else
                    {
                        responseModel.Status = true;
                    }

                }
                if (responseModel.Status)
                {
                    ROSSystemsWrapperModel wrapperModel = DataTemplateWrapperModel.SystemsWrapperModel;
                    DSROSDataTemplate dsROSDataTemplateWrapper = new DSROSDataTemplate();
                    #region ROSDataTemp Info
                    ROSDataTempInfoModel infoModel = new ROSDataTempInfoModel();
                    if (responseModel.IsTemplateChanged)
                    {
                        infoModel.IsTemplateChanged = true;
                    }
                    infoModel.Comments = wrapperModel.ROSComments;
                    infoModel.Description = wrapperModel.ROSNormalDescription;
                    infoModel.IsNormal = wrapperModel.ROSisNormal;
                    infoModel.ROSSystemDate = wrapperModel.ReviewofSystemsDate;
                    infoModel.ROSDataTempInfoId = wrapperModel.ROSSystemInfoID < 0 ? DataTemplateWrapperModel.ROSDataTempInfoId : wrapperModel.ROSSystemInfoID;
                    infoModel.ROSDataTemplateId = responseModel.ROSDataTemplateId;
                    infoModel.ROSTemplateId = responseModel.ROSTemplateId;

                    BLObject<DSROSDataTemplate> objROSSystemPatientWrapper = null;
                    DSROSDataTemplate.ROSDataTempInfoRow drDataTempInfo = null;
                    if (infoModel.ROSDataTempInfoId > 0 || infoModel.ROSDataTemplateId > 0)
                    {
                        objROSSystemPatientWrapper = BLLClinicalObj.loadROSDataTempInfo(infoModel.ROSDataTempInfoId, infoModel.ROSDataTemplateId);
                        dsROSDataTemplateWrapper = objROSSystemPatientWrapper.Data;
                        if (dsROSDataTemplateWrapper.ROSDataTempInfo.Rows.Count > 0)
                        {
                            drDataTempInfo = (DSROSDataTemplate.ROSDataTempInfoRow)dsROSDataTemplateWrapper.ROSDataTempInfo.Rows[0];
                        }
                        else
                        {
                            drDataTempInfo = dsROSDataTemplateWrapper.ROSDataTempInfo.NewROSDataTempInfoRow();
                        }
                    }
                    else
                    {
                        drDataTempInfo = dsROSDataTemplateWrapper.ROSDataTempInfo.NewROSDataTempInfoRow();
                    }


                    if (infoModel.ROSDataTempInfoId > 0)
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSDataTempInfoIdColumn] = infoModel.ROSDataTempInfoId;
                    }
                    else
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSDataTempInfoIdColumn] = -1;
                    }
                    if (!string.IsNullOrEmpty(infoModel.Comments))
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.CommentsColumn] = infoModel.Comments;
                    }
                    else
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.CommentsColumn] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(infoModel.Description))
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.DescriptionColumn] = infoModel.Description;
                    }
                    else
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.DescriptionColumn] = DBNull.Value;
                    }
                    if (infoModel.IsNormal)
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.IsNormalColumn] = infoModel.IsNormal;
                    }
                    else
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.IsNormalColumn] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(infoModel.ROSSystemDate))
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSSystemDateColumn] = MDVUtility.ToDateTime(infoModel.ROSSystemDate);
                    }
                    else
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSSystemDateColumn] = DBNull.Value;
                    }

                    if (infoModel.ROSDataTemplateId > 0)
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSDataTemplateIdColumn] = infoModel.ROSDataTemplateId;
                    }
                    else
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSDataTemplateIdColumn] = DBNull.Value;
                    }

                    if (infoModel.ROSTemplateId > 0)
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSTemplateIdColumn] = infoModel.ROSTemplateId;
                    }
                    else
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSTemplateIdColumn] = DBNull.Value;
                    }
                    if (infoModel.IsTemplateChanged)
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.IsTemplateChangedColumn] = true;
                    }
                    else
                    {
                        drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.IsTemplateChangedColumn] = false;
                    }


                    //// if no details is found against CharacteristicId, it implies for new record
                    if (dsROSDataTemplateWrapper.ROSDataTempInfo.Rows.Count < 1)
                    {
                        dsROSDataTemplateWrapper.ROSDataTempInfo.AddROSDataTempInfoRow(drDataTempInfo);
                    }

                    #endregion
                    BLObject<DSROSDataTemplate> objDetails = new BLObject<DSROSDataTemplate>();
                    if (infoModel.ROSDataTempInfoId > 0)
                    {
                        objDetails = BLLClinicalObj.updateROSDataTempInfo(dsROSDataTemplateWrapper);
                        dsROSDataTemplateWrapper = objDetails.Data;
                    }
                    else
                    {
                        objDetails = BLLClinicalObj.insertROSDataTempInfo(dsROSDataTemplateWrapper);
                        dsROSDataTemplateWrapper = objDetails.Data;

                    }

                    if (infoModel.IsNormal && (wrapperModel.isNormalDescription == null || wrapperModel.isNormalDescription.Count == 0))
                    {

                    }
                    else
                    {
                        wrapperModel.ROSSystemInfoID = MDVUtility.ToLong(drDataTempInfo[dsROSDataTemplateWrapper.ROSDataTempInfo.ROSDataTempInfoIdColumn]);

                        #region SystemPatient
                        // ROSSystemPatientModel sysPatientModel = new ROSSystemPatientModel();
                        if (wrapperModel.ROSSystemIds != null && wrapperModel.ROSSystemIds.Count > 0)
                        {
                            if (wrapperModel.ROSSystemInfoID > 0)
                            {

                                objROSSystemPatientWrapper = BLLClinicalObj.loadROSDataSystem(MDVSession.Current.AppUserId, 0, wrapperModel.ROSSystemInfoID);//wrapperModel.rosSystemPatientID);//.loadROSPatientInfo(MDVUtility.ToLong(wrapperModel.PatientId), wrapperModel.ROSSystemInfoID);
                                DSROSDataTemplate dsROSSystemPatienT = objROSSystemPatientWrapper.Data;
                                dsROSSystemPatienT = insertUpdateRosSystemPatient(wrapperModel, dsROSSystemPatienT);
                                dsROSDataTemplateWrapper.Merge(dsROSSystemPatienT);
                            }
                        }

                        #endregion

                        if (wrapperModel.ROSSystemInfoID > 0)
                        {
                            if (dsROSDataTemplateWrapper.ROSDataSystem.Rows.Count > 0 && ((wrapperModel.charcNegCharacteristics != null && wrapperModel.charcNegCharacteristics.Count > 0) || (wrapperModel.charcPosCharacteristics != null && wrapperModel.charcPosCharacteristics.Count > 0) || (wrapperModel.SystemsWithDetails != null && wrapperModel.SystemsWithDetails.Count > 0)))
                            {
                                objROSSystemPatientWrapper = BLLClinicalObj.loadROSDataSystemCharacteristics(infoModel.ROSDataTemplateId, 0, -1);//model.ROSSystemPatientID);//wrapperModel.rosSystemPatientID);//.loadROSPatientInfo(MDVUtility.ToLong(wrapperModel.PatientId), wrapperModel.ROSSystemInfoID);
                                DSROSDataTemplate dsROSSystemPatientCharac = objROSSystemPatientWrapper.Data;
                                #region ROSSystemPatientCharacteristics

                                #region for loop for -ve and +ve charc
                                List<ROSSystemPatientCharacteristicsModel> sysPatientCharcModelList = new List<ROSSystemPatientCharacteristicsModel>();
                                for (int i = 0; i < wrapperModel.charcNegCharacteristics.Count; i++)
                                {
                                    ROSSystemPatientCharacteristicsModel sysPatientCharcModel = new ROSSystemPatientCharacteristicsModel();
                                    int charcNegCharacteristicsId = wrapperModel.charcNegCharacteristics[i] != null ? MDVUtility.ToInt32(wrapperModel.charcNegCharacteristics[i]) : 0;
                                    int charcNegSystemId = wrapperModel.charcNegSystem[i] != null ? MDVUtility.ToInt32(wrapperModel.charcNegSystem[i]) : 0;
                                    sysPatientCharcModel.CharacteristicsId = MDVUtility.ToInt32(charcNegCharacteristicsId);//wrapperModel.charcNegCharacteristics
                                    if (wrapperModel.charcDescNeg != null && wrapperModel.charcNegSystem.Count > i && wrapperModel.charcDescNeg[i] != null)
                                    {
                                        sysPatientCharcModel.Description = wrapperModel.charcDescNeg[i];
                                    }
                                    if (wrapperModel.AllPositiveSystems.Any(s => charcNegSystemId.ToString().Contains(s)))
                                    {
                                        sysPatientCharcModel.IsPositive = true;
                                    }
                                    else
                                    {
                                        sysPatientCharcModel.IsPositive = false;
                                    }

                                    sysPatientCharcModel.ROSSystemCharacteristicsId = -1;
                                    sysPatientCharcModel.ROSSystemPatientCharacteristicsID = -1;
                                    // sysPatientCharcModel.ROSSystemPatientID = 0;
                                    sysPatientCharcModel.CharcName = wrapperModel.charcNegName[i];
                                    sysPatientCharcModel.SystemID = charcNegSystemId;
                                    foreach (DSROSDataTemplate.ROSDataSystemRow drSystemPatient in dsROSDataTemplateWrapper.ROSDataSystem.Rows)
                                    {
                                        string SysId = drSystemPatient[dsROSDataTemplateWrapper.ROSDataSystem.ROSSystemIdColumn].ToString();
                                        if (MDVUtility.ToInt32(SysId) == sysPatientCharcModel.SystemID)
                                        {
                                            sysPatientCharcModel.ROSSystemPatientID = MDVUtility.ToLong(drSystemPatient[dsROSDataTemplateWrapper.ROSDataSystem.ROSDataSystemIdColumn].ToString());
                                            sysPatientCharcModelList.Add(sysPatientCharcModel);
                                            break;
                                        }
                                        else
                                        {
                                            // sysPatientCharcModel.ROSSystemPatientID = -i + 1;
                                        }
                                    }

                                }
                                for (int i = 0; i < wrapperModel.charcPosCharacteristics.Count; i++)
                                {
                                    ROSSystemPatientCharacteristicsModel sysPatientCharcModel = new ROSSystemPatientCharacteristicsModel();
                                    int charcPosCharacteristicsId = wrapperModel.charcPosCharacteristics[i] != null ? MDVUtility.ToInt32(wrapperModel.charcPosCharacteristics[i]) : 0;
                                    int charcPosSystemId = wrapperModel.charcPosSystem[i] != null ? MDVUtility.ToInt32(wrapperModel.charcPosSystem[i]) : 0;
                                    sysPatientCharcModel.CharacteristicsId = MDVUtility.ToInt32(charcPosCharacteristicsId);//wrapperModel.charcNegCharacteristics
                                    if (wrapperModel.charcDescPos != null && wrapperModel.charcDescPos[i] != null)
                                    {
                                        sysPatientCharcModel.Description = wrapperModel.charcDescPos[i];
                                    }
                                    sysPatientCharcModel.CharcName = wrapperModel.charcPosName[i];
                                    if (wrapperModel.AllNegativeSystems.Any(s => charcPosSystemId.ToString().Contains(s)))
                                    {
                                        sysPatientCharcModel.IsPositive = false;
                                    }
                                    else
                                    {
                                        sysPatientCharcModel.IsPositive = true;
                                    }
                                    // sysPatientCharcModel.IsPositive = true;
                                    sysPatientCharcModel.ROSSystemCharacteristicsId = -1;
                                    sysPatientCharcModel.ROSSystemPatientCharacteristicsID = -1;
                                    sysPatientCharcModel.SystemID = charcPosSystemId;

                                    foreach (DSROSDataTemplate.ROSDataSystemRow drSystemPatient in dsROSDataTemplateWrapper.ROSDataSystem.Rows)
                                    {
                                        string SysId = drSystemPatient[dsROSDataTemplateWrapper.ROSDataSystem.ROSSystemIdColumn].ToString();
                                        if (MDVUtility.ToInt32(SysId) == sysPatientCharcModel.SystemID)
                                        {
                                            sysPatientCharcModel.ROSSystemPatientID = MDVUtility.ToLong(drSystemPatient[dsROSDataTemplateWrapper.ROSDataSystem.ROSDataSystemIdColumn].ToString());
                                            sysPatientCharcModelList.Add(sysPatientCharcModel);
                                            break;
                                        }
                                        else
                                        {
                                            // sysPatientCharcModel.ROSSystemPatientID = -i + 1;
                                        }
                                    }


                                }

                                #endregion

                                #region update  ROSSystemPatient Characteristics
                                if (dsROSSystemPatientCharac != null && dsROSSystemPatientCharac.ROSDataSystemCharc.Rows.Count > 0)
                                {

                                    List<string> delROSSystemCharac = new List<string>();
                                    foreach (DSROSDataTemplate.ROSDataSystemCharcRow drROSDataSystemCharc in dsROSSystemPatientCharac.ROSDataSystemCharc.Rows)
                                    {
                                        if (sysPatientCharcModelList.Count == 0)
                                        {

                                            break;
                                        }
                                        string ROSDataSystemCharcId = drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSDataSystemCharcIDColumn].ToString();
                                        string ROSDataSystemID = drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSDataSystemIDColumn].ToString();
                                        string ROSSystemCharacteristicsId = drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn].ToString();
                                        ROSSystemPatientCharacteristicsModel model = sysPatientCharcModelList.FirstOrDefault(n => n.ROSSystemPatientID == MDVUtility.ToLong(ROSDataSystemID));
                                        if (model != null)
                                        {
                                            model = sysPatientCharcModelList.FirstOrDefault(n => n.ROSSystemPatientID == MDVUtility.ToLong(ROSDataSystemID) && n.CharacteristicsId == MDVUtility.ToLong(ROSSystemCharacteristicsId));
                                            if (model == null)
                                            {
                                                //foreach (var item in wrapperModel.SystemsWithDetails)
                                                //{
                                                //    if (wrapperModel.NormalSystems.Any(s => ROSSystemPatientID.Contains(s)))
                                                //    {
                                                //    }
                                                //}
                                                // delROSSystemCharac.Add(ROSSystemPatientCharacteristicsId);
                                                drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.DescriptionColumn] = DBNull.Value;
                                                drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.IsPositiveColumn] = DBNull.Value;
                                            }
                                            else
                                            {
                                                sysPatientCharcModelList.Remove(model);
                                                if (model.ROSSystemPatientCharacteristicsID > 0)
                                                {
                                                    drROSDataSystemCharc.ROSDataSystemCharcID = MDVUtility.ToInt64(model.ROSSystemPatientCharacteristicsID);
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(ROSDataSystemCharcId))
                                                    {
                                                        drROSDataSystemCharc.ROSDataSystemCharcID = MDVUtility.ToInt64(ROSDataSystemCharcId);
                                                    }
                                                    else
                                                    {
                                                        drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSDataSystemCharcIDColumn] = DBNull.Value;
                                                    }

                                                }

                                                if (model.ROSSystemPatientID > 0)
                                                {
                                                    drROSDataSystemCharc.ROSDataSystemID = MDVUtility.ToInt64(model.ROSSystemPatientID);
                                                }
                                                else
                                                {
                                                    drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSDataSystemIDColumn] = DBNull.Value;
                                                }

                                                //if (model.ROSSystemCharacteristicsId > 0)
                                                //{
                                                //    drROSDataSystemCharc.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.ROSSystemCharacteristicsId);
                                                //}
                                                //else
                                                //{
                                                //    drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn] = DBNull.Value;
                                                //}
                                                if (model.ROSSystemCharacteristicsId > 0)
                                                {
                                                    drROSDataSystemCharc.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.ROSSystemCharacteristicsId);
                                                }
                                                else
                                                {
                                                    if (model.CharacteristicsId > 0)
                                                    {
                                                        drROSDataSystemCharc.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.CharacteristicsId);
                                                    }
                                                    else
                                                    {
                                                        drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn] = DBNull.Value;
                                                    }

                                                }
                                                if (!string.IsNullOrEmpty(model.Description))
                                                {
                                                    drROSDataSystemCharc.Description = MDVUtility.ToStr(model.Description);
                                                }
                                                else
                                                {
                                                    drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.DescriptionColumn] = DBNull.Value;
                                                }

                                                //if (!string.IsNullOrEmpty(model.CharcName))
                                                //{
                                                //    drROSDataSystemCharc.CharacteristicsName = MDVUtility.ToStr(model.CharcName);
                                                //}
                                                //else
                                                //{
                                                //    drROSDataSystemCharc[dsROSSystemPatientCharac.ROSDataSystemCharc.CharacteristicsNameColumn] = DBNull.Value;
                                                //}

                                                drROSDataSystemCharc.IsPositive = model.IsPositive;
                                                // dsROSSystemPatientWrapper.ROSDataSystemCharc.AddROSDataSystemCharcRow(dr);

                                            }
                                        }
                                    }
                                    //if (delROSSystemCharac != null && delROSSystemCharac.Count > 0)
                                    //{
                                    //    foreach (var item in delROSSystemCharac)
                                    //    {
                                    //        if (!string.IsNullOrEmpty(item))
                                    //        {
                                    //            BLLClinicalObj.deleteROSSystemPatientCharacteristics(MDVUtility.ToLong(item));
                                    //        }

                                    //    }

                                    //}
                                    if (sysPatientCharcModelList.Count == 0)
                                    {
                                        BLObject<DSROSDataTemplate> obj = BLLClinicalObj.updateROSDataSystemCharacteristics(dsROSSystemPatientCharac);
                                        //delROSSystemCharac
                                        dsROSSystemPatientCharac = obj.Data;

                                    }
                                }
                                #endregion
                                #region insert  ROSSystemPatient Characteristics
                                // else
                                // {
                                if (sysPatientCharcModelList.Count > 0)
                                {


                                    foreach (ROSSystemPatientCharacteristicsModel model in sysPatientCharcModelList)
                                    {
                                        DSROSDataTemplate.ROSDataSystemCharcRow dr = dsROSSystemPatientCharac.ROSDataSystemCharc.NewROSDataSystemCharcRow();
                                        if (model.ROSSystemPatientCharacteristicsID > 0)
                                        {
                                            dr.ROSDataSystemCharcID = MDVUtility.ToInt64(model.ROSSystemPatientCharacteristicsID);
                                        }
                                        else
                                        {
                                            dr[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSDataSystemCharcIDColumn] = DBNull.Value;
                                        }

                                        if (model.ROSSystemPatientID > 0)
                                        {
                                            dr.ROSDataSystemID = MDVUtility.ToInt64(model.ROSSystemPatientID);
                                        }
                                        else
                                        {
                                            dr[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSDataSystemIDColumn] = DBNull.Value;
                                        }
                                        if (model.ROSSystemCharacteristicsId > 0)
                                        {
                                            dr.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.ROSSystemCharacteristicsId);
                                        }
                                        else
                                        {
                                            if (model.CharacteristicsId > 0)
                                            {
                                                dr.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.CharacteristicsId);
                                            }
                                            else
                                            {
                                                dr[dsROSSystemPatientCharac.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn] = DBNull.Value;
                                            }

                                        }

                                        if (!string.IsNullOrEmpty(model.Description))
                                        {
                                            dr.Description = MDVUtility.ToStr(model.Description);
                                        }
                                        else
                                        {
                                            dr[dsROSSystemPatientCharac.ROSDataSystemCharc.DescriptionColumn] = DBNull.Value;
                                        }
                                        //if (!string.IsNullOrEmpty(model.CharcName))
                                        //{
                                        //    dr.CharacteristicsName = MDVUtility.ToStr(model.CharcName);
                                        //}
                                        //else
                                        //{
                                        //    dr[dsROSSystemPatientCharac.ROSDataSystemCharc.CharacteristicsNameColumn] = DBNull.Value;
                                        //}
                                        dr.IsPositive = model.IsPositive;
                                        dsROSSystemPatientCharac.ROSDataSystemCharc.AddROSDataSystemCharcRow(dr);
                                    }
                                    BLObject<DSROSDataTemplate> objIns = BLLClinicalObj.insertROSDataSystemCharacteristics(dsROSSystemPatientCharac);
                                    dsROSSystemPatientCharac = objIns.Data;
                                    objIns = BLLClinicalObj.loadROSDataSystemCharacteristics(infoModel.ROSDataTemplateId, 0, 0);//model.ROSSystemPatientID);//wrapperModel.rosSystemPatientID);//.loadROSPatientInfo(MDVUtility.ToLong(wrapperModel.PatientId), wrapperModel.ROSSystemInfoID);
                                    dsROSSystemPatientCharac = objIns.Data;
                                }
                                #endregion
                                if (dsROSSystemPatientCharac != null)
                                {
                                    dsROSDataTemplateWrapper.Merge(dsROSSystemPatientCharac);
                                }
                                //  }
                                #endregion

                                #region charc details update
                                if (dsROSSystemPatientCharac != null)
                                {
                                    foreach (DSROSDataTemplate.ROSDataSystemCharcRow drROSDataSystemCharc in dsROSDataTemplateWrapper.ROSDataSystemCharc.Rows)
                                    {
                                        //if (objCharacteristicsDetails.ROSCharacteristicsId == drROSDataSystemCharc.ROSSystemCharacteristicsId)
                                        //{
                                        //    systemPatientCharacteristicsId = drROSDataSystemCharc.ROSDataSystemCharcID;

                                        Int64 ROSSystemCharacteristicsId = MDVUtility.ToInt64(drROSDataSystemCharc[dsROSDataTemplateWrapper.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn.ColumnName]);
                                        Int64 ROSDataSystemID = MDVUtility.ToInt64(drROSDataSystemCharc[dsROSDataTemplateWrapper.ROSDataSystemCharc.ROSDataSystemIDColumn.ColumnName]);
                                        //Int64 ROSDataSystemCharcID = MDVUtility.ToInt64(drROSDataSystemCharc[dsROSSystemPatientWrapper.ROSDataSystemCharc.ROSSystemCharacteristicsIdColumn.ColumnName]);

                                        if (ROSSystemCharacteristicsId > 0)
                                        {
                                            Int64 ROSCharacteristicsDetailsId = 0;
                                            DSROSDataTemplate responseCharacteristicsDetails = new DSROSDataTemplate();
                                            if (drROSDataSystemCharc[dsROSDataTemplateWrapper.ROSDataSystemCharc.DetailsIDColumn.ColumnName] != DBNull.Value)
                                            {
                                                ROSCharacteristicsDetailsId = MDVUtility.ToInt64(drROSDataSystemCharc[dsROSDataTemplateWrapper.ROSDataSystemCharc.DetailsIDColumn.ColumnName]);
                                                BLObject<DSROSDataTemplate> objDetail = BLLClinicalObj.loadROSDataSystemCharacDetails(0, ROSCharacteristicsDetailsId);
                                                responseCharacteristicsDetails = objDetail.Data;
                                            }

                                            if (wrapperModel.rosPatientCharcObjList != null && wrapperModel.rosPatientCharcObjList.Count > 0)
                                            {

                                                if (wrapperModel.rosPatientCharcObjList.Any(s => s.ROSCharacteristicsId == ROSSystemCharacteristicsId))
                                                {
                                                    List<ROSCharacteristicsDetailsModel> modelObjList = wrapperModel.rosPatientCharcObjList.Where(s => s.ROSCharacteristicsId == ROSSystemCharacteristicsId).ToList<ROSCharacteristicsDetailsModel>();

                                                    //responseCharacteristicsDetails = insertUpdateCharacteristicsDetails(ROSDataSystemCharcID, wrapperModel.rosPatientCharcObjList, dsROSSystemPatientWrapper);
                                                    responseCharacteristicsDetails = insertUpdateCharacteristicsDetails(ROSSystemCharacteristicsId, modelObjList, dsROSDataTemplateWrapper);
                                                    wrapperModel.rosPatientCharcObjList.RemoveAll(s => s.ROSCharacteristicsId == ROSSystemCharacteristicsId);
                                                }
                                                else if (wrapperModel.rosPatientCharcObjList.Any(s => s.ROSCharacteristicsId == -1))
                                                {
                                                    List<ROSCharacteristicsDetailsModel> modelObjList = wrapperModel.rosPatientCharcObjList.Where(s => s.ROSCharacteristicsId == -1).ToList<ROSCharacteristicsDetailsModel>();
                                                    responseCharacteristicsDetails = insertUpdateCharacteristicsDetails(ROSSystemCharacteristicsId, modelObjList, dsROSDataTemplateWrapper);
                                                    wrapperModel.rosPatientCharcObjList.RemoveAll(s => s.ROSCharacteristicsId == -1);
                                                }

                                                if (responseCharacteristicsDetails != null)
                                                {
                                                    dsROSDataTemplateWrapper.Merge(responseCharacteristicsDetails);
                                                }
                                            }
                                            else
                                            {
                                                dsROSDataTemplateWrapper.Merge(responseCharacteristicsDetails);
                                            }

                                            //foreach (DSClinicalReviewofSystem.ROSSystemInfoRow drROSSystemInfo in dsROSSystemPatientWrapper.ROSSystemInfo.Rows)
                                            //{
                                            //    drROSSystemInfo.SoapText = soapText;
                                            //}
                                            //   objDetails = BLLClinicalObj.updateROSPatientInfo(dsROSSystemPatientWrapper);
                                            // dsROSSystemPatientWrapper = objDetails.Data;

                                        }
                                    }
                                }
                                #endregion
                            }

                        }

                    }
                    var response = new
                    {
                        status = responseModel.Status,
                        Message = (responseModel.Status) ? Common.AppPrivileges.Save_Message : responseModel.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {

                    var response = new
                    {
                        status = responseModel.Status,
                        Message = responseModel.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));// return response
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

        public DSROSDataTemplate insertUpdateRosSystemPatient(ROSSystemsWrapperModel wrapperModel, DSROSDataTemplate dsROSSystemPatientWrapper)
        {
            BLObject<DSROSDataTemplate> objDetails = new BLObject<DSROSDataTemplate>();
            DSROSDataTemplate dsROSSystemPatient = new DSROSDataTemplate();
            if (wrapperModel.ROSSystemInfoID > 0)
            {
                if (dsROSSystemPatientWrapper.ROSDataSystem.Rows.Count > 0)
                {
                    dsROSSystemPatient = dsROSSystemPatientWrapper;
                    foreach (DSROSDataTemplate.ROSDataSystemRow drSystemPatient in dsROSSystemPatient.ROSDataSystem.Rows)
                    {

                        //  drSystemPatient[dsROSSystemPatient.ROSSystemPatient.PatientIDColumn] = wrapperModel.PatientId;
                        string ROSSystemId = drSystemPatient[dsROSSystemPatient.ROSDataSystem.ROSSystemIdColumn].ToString();
                        if (wrapperModel.NormalSystems != null && wrapperModel.NormalSystems.Count > 0)
                        {
                            if (wrapperModel.NormalSystems.Any(s => ROSSystemId.Contains(s)))
                            {

                                if (wrapperModel.NormalSystems.IndexOf(ROSSystemId) > -1)
                                {
                                    drSystemPatient[dsROSSystemPatient.ROSDataSystem.IsNormalColumn] = true;
                                    drSystemPatient[dsROSSystemPatient.ROSDataSystem.DescriptionColumn] = wrapperModel.isNormalDescription[wrapperModel.NormalSystems.IndexOf(ROSSystemId)];
                                }
                                else
                                {
                                    drSystemPatient[dsROSSystemPatient.ROSDataSystem.IsNormalColumn] = false;
                                    drSystemPatient[dsROSSystemPatient.ROSDataSystem.DescriptionColumn] = DBNull.Value;
                                }

                            }
                            else
                            {
                                drSystemPatient[dsROSSystemPatient.ROSDataSystem.IsNormalColumn] = false;
                                drSystemPatient[dsROSSystemPatient.ROSDataSystem.DescriptionColumn] = DBNull.Value;
                            }
                        }
                        else
                        {
                            drSystemPatient[dsROSSystemPatient.ROSDataSystem.IsNormalColumn] = false;
                            drSystemPatient[dsROSSystemPatient.ROSDataSystem.DescriptionColumn] = DBNull.Value;

                        }

                        //  drSystemPatient[dsROSSystemPatient.ROSDataSystem.SoapTextColumn] = DBNull.Value;
                        if (wrapperModel.ROSisNormal)
                        {
                            drSystemPatient[dsROSSystemPatientWrapper.ROSDataSystem.IsNormalColumn] = true;
                        }
                        if (wrapperModel.ROSSystemInfoID > 0)
                        {
                            drSystemPatient[dsROSSystemPatient.ROSDataSystem.ROSDataTempInfoIdColumn] = wrapperModel.ROSSystemInfoID;
                        }
                    }

                }
                else
                {

                    foreach (string ROSSystemId in wrapperModel.ROSSystemIds)
                    {
                        DSROSDataTemplate.ROSDataSystemRow drSystemPatient = dsROSSystemPatient.ROSDataSystem.NewROSDataSystemRow();
                        if (wrapperModel.NormalSystems != null && wrapperModel.NormalSystems.Count > 0)
                        {
                            if (wrapperModel.NormalSystems.Any(s => ROSSystemId.Contains(s)))
                            {

                                if (wrapperModel.NormalSystems.IndexOf(ROSSystemId) > -1)
                                {
                                    drSystemPatient[dsROSSystemPatient.ROSDataSystem.IsNormalColumn] = true;
                                    drSystemPatient[dsROSSystemPatient.ROSDataSystem.DescriptionColumn] = wrapperModel.isNormalDescription[wrapperModel.NormalSystems.IndexOf(ROSSystemId)];
                                }
                                else
                                {
                                    drSystemPatient[dsROSSystemPatient.ROSDataSystem.IsNormalColumn] = false;
                                    drSystemPatient[dsROSSystemPatient.ROSDataSystem.DescriptionColumn] = DBNull.Value;
                                }
                            }
                            else
                            {
                                drSystemPatient[dsROSSystemPatient.ROSDataSystem.IsNormalColumn] = false;
                                drSystemPatient[dsROSSystemPatient.ROSDataSystem.DescriptionColumn] = DBNull.Value;
                            }
                        }
                        else
                        {

                            drSystemPatient[dsROSSystemPatient.ROSDataSystem.IsNormalColumn] = false;
                            drSystemPatient[dsROSSystemPatient.ROSDataSystem.DescriptionColumn] = DBNull.Value;

                        }
                        //   drSystemPatient[dsROSSystemPatient.ROSSystemPatient.PatientIDColumn] = wrapperModel.PatientId;
                        drSystemPatient[dsROSSystemPatient.ROSDataSystem.ROSSystemIdColumn] = MDVUtility.ToInt32(ROSSystemId);
                        drSystemPatient[dsROSSystemPatient.ROSDataSystem.ROSDataTempInfoIdColumn] = wrapperModel.ROSSystemInfoID;
                        //   drSystemPatient[dsROSSystemPatient.ROSSystemPatient.ROSSystemPatientIDColumn] = -1;
                        if (wrapperModel.ROSisNormal)
                        {
                            drSystemPatient[dsROSSystemPatient.ROSDataSystem.IsNormalColumn] = true;
                        }

                        //drSystemPatient[dsROSSystemPatient.ROSDataSystem.SoapTextColumn] = DBNull.Value;
                        //drSystemPatient[dsROSSystemPatient.ROSDataSystem.SystemNameColumn] = wrapperModel.ROSSystemNames[wrapperModel.ROSSystemIds.IndexOf(ROSSystemId)];
                        dsROSSystemPatient.ROSDataSystem.AddROSDataSystemRow(drSystemPatient);

                    }
                }

                if (dsROSSystemPatientWrapper.ROSDataSystem.Rows.Count > 0)
                {
                    objDetails = BLLClinicalObj.updateROSDataSystem(dsROSSystemPatient);
                    dsROSSystemPatient = objDetails.Data;
                }
                else
                {
                    objDetails = BLLClinicalObj.insertROSDataSystem(dsROSSystemPatient);
                    dsROSSystemPatient = objDetails.Data;
                }
            }
            return dsROSSystemPatient;
        }

        private DSROSDataTemplate insertUpdateCharacteristicsDetails(long systemPatientCharacteristicsId, List<ROSCharacteristicsDetailsModel> objCharacteristicsDetailsList, DSROSDataTemplate dsWrapper)
        {
            try
            {



                DSROSDataTemplate resultDataset = new DSROSDataTemplate();
                foreach (ROSCharacteristicsDetailsModel objCharacteristicsDetails in objCharacteristicsDetailsList)
                {
                    DSROSDataTemplate dsDetails = new DSROSDataTemplate();
                    if (objCharacteristicsDetails != null)
                    {
                        foreach (DSROSDataTemplate.ROSDataSystemCharcRow drROSSystemPatientCharacteristics in dsWrapper.ROSDataSystemCharc.Rows)
                        {
                            if (objCharacteristicsDetails.ROSCharacteristicsId == drROSSystemPatientCharacteristics.ROSSystemCharacteristicsId)
                            {
                                systemPatientCharacteristicsId = drROSSystemPatientCharacteristics.ROSDataSystemCharcID;
                                break;
                            }
                        }
                        if (systemPatientCharacteristicsId > 0)
                        {

                            long currentDetailsId = MDVUtility.ToInt64(objCharacteristicsDetails.ROSCharacteristicsDetailsId);
                            currentDetailsId = currentDetailsId == 0 ? -1 : currentDetailsId;
                            BLObject<DSROSDataTemplate> obj = null;
                            DSROSDataTemplate.ROSDataCharcDetailRow RowDetails = null;
                            bool isUpdate = false;
                            //  if (currentDetailsId > 0)
                            //   {
                            //kr
                            obj = BLLClinicalObj.loadROSDataSystemCharacDetails(systemPatientCharacteristicsId, currentDetailsId);
                            dsDetails = obj.Data;
                            if (dsDetails.ROSDataCharcDetail.Rows.Count > 0)
                            {
                                RowDetails = (DSROSDataTemplate.ROSDataCharcDetailRow)dsDetails.ROSDataCharcDetail.Rows[0];
                                isUpdate = true;
                            }
                            else
                            {
                                RowDetails = dsDetails.ROSDataCharcDetail.NewROSDataCharcDetailRow();
                            }
                            // }
                            //  else
                            //  {
                            //   RowDetails = dsDetails.ROSDataCharcDetail.NewROSDataCharcDetailRow();
                            // }

                            if (RowDetails != null)
                            {
                                if (dsDetails.ROSDataCharcDetail.Rows.Count < 1)
                                {
                                    RowDetails.ROSDataCharcDetailId = currentDetailsId;
                                }
                                if (systemPatientCharacteristicsId > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSDataSystemCharcIDColumn] = systemPatientCharacteristicsId;
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSDataSystemCharcIDColumn] = DBNull.Value;
                                }
                                //RowDetails.BirthHxId = birthHxId;
                                //------------------------
                                if (!string.IsNullOrEmpty(objCharacteristicsDetails.PreviousHistory))
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.PreviousHistoryColumn] = MDVUtility.ToStr(objCharacteristicsDetails.PreviousHistory);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.PreviousHistoryColumn] = DBNull.Value;
                                }


                                if (objCharacteristicsDetails.ROSCharacteristicsDetailStatusId > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailStatusIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailStatusId);
                                    // RowDetails[dsDetails.ROSDataCharcDetail.DetailStatusNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailStatusId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailStatusIdColumn] = DBNull.Value;
                                }


                                if (!string.IsNullOrEmpty(objCharacteristicsDetails.Onset))
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.OnsetColumn] = MDVUtility.ToStr(objCharacteristicsDetails.Onset);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.OnsetColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.Duration > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.DurationColumn] = MDVUtility.Tofloat(objCharacteristicsDetails.Duration);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.DurationColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailDurationId > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailDurationIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailDurationId);
                                    // RowDetails[dsDetails.ROSDataCharcDetail.DurationNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailDurationId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailDurationIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailPatternId > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailPatternIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailPatternId);
                                    //  RowDetails[dsDetails.ROSDataCharcDetail.PatternNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailPatternId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailPatternIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailSeverityId > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailSeverityIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailSeverityId);
                                    //  RowDetails[dsDetails.ROSDataCharcDetail.SeverityNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailSeverityId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailSeverityIdColumn] = DBNull.Value;
                                }


                                if (objCharacteristicsDetails.ROSCharacteristicsDetailCourseId > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailCourseIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailCourseId);
                                    // RowDetails[dsDetails.ROSDataCharcDetail.CourseNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailCourseId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailCourseIdColumn] = DBNull.Value;
                                }
                                //-------------------------------------
                                if (objCharacteristicsDetails.ROSCharacteristicsDetailRadiationId > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailRadiationIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailRadiationId);
                                    // RowDetails[dsDetails.ROSDataCharcDetail.RadiationNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailRadiationId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailRadiationIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailFrequencyId > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailFrequencyIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailFrequencyId);
                                    // RowDetails[dsDetails.ROSDataCharcDetail.FrequencyNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailFrequencyId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailFrequencyIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailContextId > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailContextIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailContextId);
                                    //  RowDetails[dsDetails.ROSDataCharcDetail.ContextNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailContextId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailContextIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailCharacterCSZId > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailCharacterCSZIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailCharacterCSZId);
                                    //  RowDetails[dsDetails.ROSDataCharcDetail.CharacterCSZNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailCharacterCSZId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailCharacterCSZIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailAggravedById > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailAggravedByIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailAggravedById);
                                    // RowDetails[dsDetails.ROSDataCharcDetail.AggravedByNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailAggravedById_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailAggravedByIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailRelievedById > 0)
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailRelievedByIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailRelievedById);
                                    // RowDetails[dsDetails.ROSDataCharcDetail.RelievedByNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailRelievedById_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.ROSCharacteristicsDetailRelievedByIdColumn] = DBNull.Value;
                                }
                                //---------------------------
                                if (!string.IsNullOrEmpty(objCharacteristicsDetails.Location))
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.LocationColumn] = MDVUtility.ToStr(objCharacteristicsDetails.Location);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.LocationColumn] = DBNull.Value;
                                }

                                if (!string.IsNullOrEmpty(objCharacteristicsDetails.PrecipitatedBY))
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.PrecipitatedBYColumn] = MDVUtility.ToStr(objCharacteristicsDetails.PrecipitatedBY);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.PrecipitatedBYColumn] = DBNull.Value;
                                }

                                if (!string.IsNullOrEmpty(objCharacteristicsDetails.AssociatedWith))
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.AssociatedWithColumn] = MDVUtility.ToStr(objCharacteristicsDetails.AssociatedWith);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSDataCharcDetail.AssociatedWithColumn] = DBNull.Value;
                                }
                                //---------------------------


                                //// if no details is found against CharacteristicId, it implies for new record
                                if (!isUpdate)
                                {
                                    dsDetails.ROSDataCharcDetail.AddROSDataCharcDetailRow(RowDetails);
                                }




                                #region Database Insertion/Updation

                                BLObject<DSROSDataTemplate> objDetails = new BLObject<DSROSDataTemplate>();
                                if (isUpdate)
                                {
                                    /*foreach (DataRow RowGeneralObj in dsDetails.ROSSystemPatientCharacteristics.Rows)
                                    {
                                        RowGeneralObj[dsDetails.ROSSystemPatientCharacteristics.SoapTextColumn] = createROSsoapText(dsDetails);
                                    }*/
                                    objDetails = BLLClinicalObj.updateROSDataCharcDetail(dsDetails);
                                    dsDetails = objDetails.Data;
                                    resultDataset.Merge(dsDetails);
                                }
                                else
                                {
                                    objDetails = BLLClinicalObj.insertROSDataCharcDetail(dsDetails);
                                    dsDetails = objDetails.Data;
                                    resultDataset.Merge(dsDetails);
                                    /*foreach (DataRow RowGeneralObj in dsDetails.ROSSystemPatientCharacteristics.Rows)
                                    {
                                        RowGeneralObj[dsDetails.ROSSystemPatientCharacteristics.SoapTextColumn] = createROSsoapText(dsDetails);
                                    }*/
                                    //     objDetails = BLLClinicalObj.updateROSDataCharcDetail(dsDetails);

                                }



                                #endregion
                            }
                        }
                    }




                }
                return resultDataset;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        #endregion



        internal string rOSDataSystemReset(long ROSDataSystemID, bool removeSystemDetails)
        {

            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ROSDataSystemID)))
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
                    BLObject<string> obj = BLLClinicalObj.ROSDataSystemReset(ROSDataSystemID, removeSystemDetails);
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


        #region "ROS Data Template Provider Base"

        public string loadROSForProvider(long providerId)
        {
            DSROSDataTemplate dsROS = new DSROSDataTemplate();
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            BLObject<DSROSDataTemplate> obj = null;

            obj = BLLClinicalObj.loadPhysicalExamForProvider(providerId);

            if (obj.Data != null)
            {
                dsROS = obj.Data;
                var response = new
                {
                    status = true,
                    ROSDataTemplateLoad_JSON = MDVUtility.JSON_DataTable(dsROS.Tables[dsROS.ROSDataTemplate.TableName]),
                    ROSDataTemplateCount = dsROS.Tables[dsROS.ROSDataTemplate.TableName].Rows.Count,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = true,
                    ROSDataTemplateLoad_JSON = "[]",
                    ROSDataTemplateCount = 0,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion
    }
}