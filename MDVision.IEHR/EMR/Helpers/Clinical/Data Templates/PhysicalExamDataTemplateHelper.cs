using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.IEHR.EMR.Model.DataTemplates;
using System;
using System.Collections.Generic;
using System.Data;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Data_Templates
{
    public class PhysicalExamDataTemplateHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public PhysicalExamDataTemplateHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static PhysicalExamDataTemplateHelper _instance = null;
        private const string Characteristic = "characteristic";
        private const string SubCharacteristic = "SubCharacteristic";
        public static PhysicalExamDataTemplateHelper Instance()
        {
            if (_instance == null)
                _instance = new PhysicalExamDataTemplateHelper();
            return _instance;
        }



        #region Build Datasets Rows (Utility Functions)

        // Author: Muhammad Arshad
        // Created Date: 11/03/2016
        //OverView: This function will build PhysExamTemplateSys Row
        public int buildDataTemplateSystemRow(long dataTemplateId, PhysicalDataTemplateSystem systemModel, DSPhysicalExamDataTemplate dsPhysExamDataTemplateSystem, int counter)
        {


            DSPhysicalExamDataTemplate.PhysExamDataTemplateSysRow rowDataTemplateSystem = null;

            DSPhysicalExamDataTemplate.PhysExamDataTemplateSysRow[] arrDataTemplateSystem = (DSPhysicalExamDataTemplate.PhysExamDataTemplateSysRow[])dsPhysExamDataTemplateSystem.PhysExamDataTemplateSys.Select(dsPhysExamDataTemplateSystem.PhysExamDataTemplateSys.SystemIdColumn.ColumnName + "=" + systemModel.SystemId);

            if (arrDataTemplateSystem.Length > 0)
            {
                rowDataTemplateSystem = arrDataTemplateSystem[0];

            }
            else
            {
                rowDataTemplateSystem = dsPhysExamDataTemplateSystem.PhysExamDataTemplateSys.NewPhysExamDataTemplateSysRow();
                rowDataTemplateSystem.DataTemplateSysId = --counter;
            }

            if (rowDataTemplateSystem != null)
            {
                rowDataTemplateSystem.SystemId = systemModel.SystemId;
                rowDataTemplateSystem.DataTemplateId = dataTemplateId;
                rowDataTemplateSystem.IsNormal = systemModel.IsNormal;

                rowDataTemplateSystem.IsActive = true;
                if (arrDataTemplateSystem.Length == 0)
                {
                    rowDataTemplateSystem.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    rowDataTemplateSystem.CreatedOn = DateTime.Now;
                }
                rowDataTemplateSystem.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                rowDataTemplateSystem.ModifiedOn = DateTime.Now;

                if (arrDataTemplateSystem.Length < 1)
                {
                    dsPhysExamDataTemplateSystem.PhysExamDataTemplateSys.AddPhysExamDataTemplateSysRow(rowDataTemplateSystem);
                }
            }
            return counter;
        }

        public int buildDataTemplateSectionRow(long dataTemplateId, PhysicalDataTemplateSection sectionModel, long systemId, DSPhysicalExamDataTemplate dsPhysExamDataTemplateSection, int counter)
        {

            DSPhysicalExamDataTemplate.PhysExamDataTemplateSectionRow drDataTemplateSection = null;
            DSPhysicalExamDataTemplate.PhysExamDataTemplateSectionRow[] arrDataTemplateSection = (DSPhysicalExamDataTemplate.PhysExamDataTemplateSectionRow[])dsPhysExamDataTemplateSection.PhysExamDataTemplateSection.Select(dsPhysExamDataTemplateSection.PhysExamDataTemplateSection.SectionIdColumn.ColumnName + "=" + sectionModel.SectionId);
            if (arrDataTemplateSection != null && arrDataTemplateSection.Length > 0)
            {
                drDataTemplateSection = arrDataTemplateSection[0];
            }
            else
            {
                drDataTemplateSection = dsPhysExamDataTemplateSection.PhysExamDataTemplateSection.NewPhysExamDataTemplateSectionRow();
                drDataTemplateSection.DataTemplateSectionId = --counter;
            }

            if (drDataTemplateSection != null)
            {
                drDataTemplateSection.SystemId = systemId;
                drDataTemplateSection.SectionId = MDVUtility.ToInt32(sectionModel.SectionId);
                drDataTemplateSection.DataTemplateId = dataTemplateId;

                drDataTemplateSection.IsActive = true;
                if (arrDataTemplateSection.Length == 0)
                {
                    drDataTemplateSection.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drDataTemplateSection.CreatedOn = DateTime.Now;
                }
                drDataTemplateSection.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drDataTemplateSection.ModifiedOn = DateTime.Now;

                if (arrDataTemplateSection.Length < 1)
                {
                    dsPhysExamDataTemplateSection.PhysExamDataTemplateSection.AddPhysExamDataTemplateSectionRow(drDataTemplateSection);
                }
            }
            return counter;
        }

        public int buildDataTemplateCharRow(long dataTemplateId, PhysicalDataTemplateChar charModel, long sectionId, DSPhysicalExamDataTemplate dsPhysExamDataTemplateChar, int counter)
        {

            DSPhysicalExamDataTemplate.PhysExamDataTemplateCharRow rowDataTemplateChar = null;
            DSPhysicalExamDataTemplate.PhysExamDataTemplateCharRow[] arrDataTemplateChars = (DSPhysicalExamDataTemplate.PhysExamDataTemplateCharRow[])dsPhysExamDataTemplateChar.PhysExamDataTemplateChar.Select(dsPhysExamDataTemplateChar.PhysExamDataTemplateChar.SectionIdColumn.ColumnName + "=" + sectionId + " and " + dsPhysExamDataTemplateChar.PhysExamDataTemplateChar.CharIdColumn.ColumnName + " = " + charModel.SectionCharacteristicId);

            if (arrDataTemplateChars != null && arrDataTemplateChars.Length > 0)
            {
                rowDataTemplateChar = arrDataTemplateChars[0];
            }
            else
            {
                rowDataTemplateChar = dsPhysExamDataTemplateChar.PhysExamDataTemplateChar.NewPhysExamDataTemplateCharRow();
                rowDataTemplateChar.DataTemplateCharId = --counter;
            }

            if (rowDataTemplateChar != null)
            {
                rowDataTemplateChar.SectionId = sectionId;
                rowDataTemplateChar.DataTemplateId = dataTemplateId;
                rowDataTemplateChar.CharId = charModel.SectionCharacteristicId;
                rowDataTemplateChar.IsActive = true;
                rowDataTemplateChar.IsPositive = charModel.IsPositive;
                rowDataTemplateChar.IsNegative = charModel.IsNegative;
                rowDataTemplateChar.Comments = charModel.Comments;


                rowDataTemplateChar.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                rowDataTemplateChar.ModifiedOn = DateTime.Now;

                if (arrDataTemplateChars.Length < 1)
                {
                    rowDataTemplateChar.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    rowDataTemplateChar.CreatedOn = DateTime.Now;
                    dsPhysExamDataTemplateChar.PhysExamDataTemplateChar.AddPhysExamDataTemplateCharRow(rowDataTemplateChar);
                }
            }
            return counter;
        }

        public int buildDataTemplateSubCharRow(long dataTemplateId, PhysicalDataTemplateSubChar subCharModel, DSPhysicalExamDataTemplate dsPhysExamDataTemplateSubChar, long charId, int counter)
        {

            DSPhysicalExamDataTemplate.PhysExamDataTemplateSubCharRow rowDataTemplateSubChar = null;
            DSPhysicalExamDataTemplate.PhysExamDataTemplateSubCharRow[] arrDataTemplateSubChars = (DSPhysicalExamDataTemplate.PhysExamDataTemplateSubCharRow[])dsPhysExamDataTemplateSubChar.PhysExamDataTemplateSubChar.Select(dsPhysExamDataTemplateSubChar.PhysExamDataTemplateSubChar.SubCharIdColumn.ColumnName + "=" + subCharModel.SubCharacteristicId);

            if (arrDataTemplateSubChars != null && arrDataTemplateSubChars.Length > 0)
            {
                rowDataTemplateSubChar = arrDataTemplateSubChars[0];
            }
            else
            {
                rowDataTemplateSubChar = dsPhysExamDataTemplateSubChar.PhysExamDataTemplateSubChar.NewPhysExamDataTemplateSubCharRow();
                rowDataTemplateSubChar.DataTemplateSubCharId = --counter;
            }

            if (rowDataTemplateSubChar != null)
            {
                rowDataTemplateSubChar.CharId = charId;
                rowDataTemplateSubChar.DataTemplateId = dataTemplateId;
                rowDataTemplateSubChar.SubCharId = subCharModel.SubCharacteristicId;
                rowDataTemplateSubChar.IsPositive = subCharModel.IsPositive;
                rowDataTemplateSubChar.IsNegative = subCharModel.IsNegative;
                rowDataTemplateSubChar.Comments = subCharModel.Comments;

                rowDataTemplateSubChar.IsActive = true;

                rowDataTemplateSubChar.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                rowDataTemplateSubChar.ModifiedOn = DateTime.Now;

                if (arrDataTemplateSubChars.Length < 1)
                {
                    rowDataTemplateSubChar.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    rowDataTemplateSubChar.CreatedOn = DateTime.Now;
                    dsPhysExamDataTemplateSubChar.PhysExamDataTemplateSubChar.AddPhysExamDataTemplateSubCharRow(rowDataTemplateSubChar);
                }
            }
            return counter;
        }

        public DSPhysicalExamDataTemplate.PhysExamDataTemplateDetailRow buildRowPhysicalExamDataTemplateDetail(Int64 physicalExamDataTemplateId, long parentId, string parentType, object model, ref int counter)
        {
            DSPhysicalExamDataTemplate dsDataTemplateDetailModel = new DSPhysicalExamDataTemplate();
            if (model == null)
            {
                return null;
            }
            PhysicalExamDataTemplateDetailModel detailModel = new PhysicalExamDataTemplateDetailModel();
            detailModel = (PhysicalExamDataTemplateDetailModel)model;
            BLObject<DSPhysicalExamDataTemplate> obj = BLLClinicalObj.LoadPhysicalExamDataTemplateDetail(physicalExamDataTemplateId, parentId, parentType, 0);
            dsDataTemplateDetailModel = obj.Data;

            DSPhysicalExamDataTemplate.PhysExamDataTemplateDetailRow RowExamDetail = null;
            DSPhysicalExamDataTemplate.PhysExamDataTemplateDetailRow[] arrExamDetails = (DSPhysicalExamDataTemplate.PhysExamDataTemplateDetailRow[])dsDataTemplateDetailModel.PhysExamDataTemplateDetail.Select(dsDataTemplateDetailModel.PhysExamDataTemplateDetail.ParentIdColumn.ColumnName + "=" + parentId);

            if (arrExamDetails != null && arrExamDetails.Length > 0)
            {
                RowExamDetail = arrExamDetails[0];
            }
            else
            {
                RowExamDetail = dsDataTemplateDetailModel.PhysExamDataTemplateDetail.NewPhysExamDataTemplateDetailRow();
                RowExamDetail.DetailId = --counter;
            }

            if (RowExamDetail != null)
            {
                RowExamDetail.PhysExamDataTemplateId = physicalExamDataTemplateId;
                RowExamDetail.ParentId = parentId;
                RowExamDetail.ParentType = parentType;

                if (!string.IsNullOrEmpty(detailModel.AggravatedBy))
                {
                    RowExamDetail.AggravatedById = MDVUtility.ToInt32(detailModel.AggravatedBy);
                }
                else
                {
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.AggravatedByIdColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(detailModel.Associatedwith))
                {
                    RowExamDetail.AssociatedWith = detailModel.Associatedwith;
                }
                else
                {
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.AssociatedWithColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(detailModel.Character))
                {
                    RowExamDetail.CharacterId = MDVUtility.ToInt64(detailModel.Character);
                }
                else
                {
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.CharacterIdColumn] = DBNull.Value;
                }


                if (!string.IsNullOrEmpty(detailModel.Context))
                    RowExamDetail.ContextId = MDVUtility.ToInt64(detailModel.Context);
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.CharacterIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.Course))
                    RowExamDetail.CourseId = MDVUtility.ToInt64(detailModel.Course);
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.CourseIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.DurationLength))
                    RowExamDetail.DurationLength = MDVUtility.ToInt32(detailModel.DurationLength);
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.DurationLengthColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.DurationPeriod))
                    RowExamDetail.DurationPeriodId = MDVUtility.ToInt32(detailModel.DurationPeriod);
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.DurationPeriodIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.Frequency))
                    RowExamDetail.FrequencyId = MDVUtility.ToInt64(detailModel.Frequency);
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.FrequencyIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.Location))
                    RowExamDetail.Location = detailModel.Location;
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.LocationColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.Onset))
                    RowExamDetail.Onset = detailModel.Onset;
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.OnsetColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.Pattern))
                    RowExamDetail.PatternId = MDVUtility.ToInt32(detailModel.Pattern);
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.PatternIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.PrevHistory))
                    RowExamDetail.PrevHistory = detailModel.PrevHistory;
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.PrevHistoryColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.Radiation))
                    RowExamDetail.RadiationId = MDVUtility.ToInt64(detailModel.Radiation);
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.RadiationIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.Relievedby))
                    RowExamDetail.RelievedbyId = MDVUtility.ToInt64(detailModel.Relievedby);
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.RelievedbyIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.Status))
                    RowExamDetail.StatusId = MDVUtility.ToInt32(detailModel.Status);
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.StatusIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.Severity))
                    RowExamDetail.SeverityId = MDVUtility.ToInt32(detailModel.Severity);
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.SeverityIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(detailModel.Percipitatedby))
                    RowExamDetail.Precipitatedby = detailModel.Percipitatedby;
                else
                    RowExamDetail[dsDataTemplateDetailModel.PhysExamDataTemplateDetail.PrecipitatedbyColumn] = DBNull.Value;
                //End Farooq Ahmad 18/02/2016 if Empty Store null value
                RowExamDetail.IsActive = true;
                if (arrExamDetails.Length == 0)
                {
                    RowExamDetail.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowExamDetail.CreatedOn = DateTime.Now;
                }
                RowExamDetail.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                RowExamDetail.ModifiedOn = DateTime.Now;

                return RowExamDetail;
            }
            return RowExamDetail;
        }

        #endregion


        #region Patient Physical Exam Data Template Fill, Save and Update Methods

        // Author: Abid Ali
        // Created Date: 15/June/2016
        //OverView: This function will handle fill of Physical Exam  Data Template
        public string searchPhysExamDataTemplate(PhysicalExamDataTemplateModel model)
        {
            try
            {
                DSPhysicalExamDataTemplate dsPhysicalExamDataTemplate = null;
                BLObject<DSPhysicalExamDataTemplate> objDataTemplate = BLLClinicalObj.loadPhysicalExamDataTemplate(MDVUtility.ToInt64(model.DataTemplateId), MDVUtility.ToInt64(model.TemplateId), MDVUtility.ToInt64(model.EntityId), model.ProviderIds, model.SpecialtyIds, model.PageNo, model.rpp, MDVUtility.ToInt32(model.IsActive));
                dsPhysicalExamDataTemplate = objDataTemplate.Data;
                if (dsPhysicalExamDataTemplate.Tables[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TableName].Rows.Count > 0)
                {
                    List<PhysicalExamDataTemplateModel> lstPhysExamDataTemplates = new List<PhysicalExamDataTemplateModel>();
                    foreach (DataRow dr in dsPhysicalExamDataTemplate.Tables[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TableName].Rows)
                    {
                        var physExamDataTemplate = new PhysicalExamDataTemplateModel
                        {
                            TemplateName = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TemplateNameColumn.ColumnName]),

                            TemplateId = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TemplateIdColumn.ColumnName]),
                            SpecialtyNames = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.SpecialtyNamesColumn.ColumnName]),
                            ProviderNames = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.ProviderNamesColumn.ColumnName]),
                            DataTemplateId = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.DataTemplateIdColumn.ColumnName]),
                            DataTemplateName = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.DataTemplateNameColumn.ColumnName]),
                            ModifiedBy = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.ModifiedByColumn.ColumnName]),
                            IsActive = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.IsActiveColumn.ColumnName]),
                            ModifiedOn = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.ModifiedOnColumn.ColumnName]),
                        };
                        lstPhysExamDataTemplates.Add(physExamDataTemplate);
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        PhysExamDataTemplateFill_JSON = js.Serialize(lstPhysExamDataTemplates),
                        dataTemplateCount = dsPhysicalExamDataTemplate.Tables[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TableName].Rows.Count,
                        iTotalDisplayRecords = (dsPhysicalExamDataTemplate.PhysExamDataTemplate.Rows.Count > 0) ? dsPhysicalExamDataTemplate.PhysExamDataTemplate.Rows[0][dsPhysicalExamDataTemplate.PhysExamDataTemplate.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "No Physical Exam Data Template Found",
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

        // Author: Abid Ali
        // Created Date: 15/June/2016
        //OverView: This function will handle fill of Physical Exam  Data Template
        public string fillPhysExamDataTemplate(PhysicalExamDataTemplateModel model)
        {
            try
            {
                DSPhysicalExamDataTemplate dsPhysicalExamDataTemplate = null;
                BLObject<DSPhysicalExamDataTemplate> objDataTemplate = BLLClinicalObj.loadPhysicalExamDataTemplate(MDVUtility.ToInt64(model.DataTemplateId), MDVUtility.ToInt64(model.TemplateId), MDVUtility.ToInt64(model.EntityId), model.ProviderIds, model.SpecialtyIds);
                if (objDataTemplate.Data != null)
                {
                    dsPhysicalExamDataTemplate = objDataTemplate.Data;
                    if (dsPhysicalExamDataTemplate.Tables[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TableName].Rows.Count > 0)
                    {
                        List<PhysicalExamDataTemplateModel> lstPhysExamDataTemplates = new List<PhysicalExamDataTemplateModel>();
                        foreach (DataRow dr in dsPhysicalExamDataTemplate.Tables[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TableName].Rows)
                        {
                            var physExamDataTemplate = new PhysicalExamDataTemplateModel
                            {
                                Comments = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.CommentsColumn.ColumnName]),
                                TemplateName = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TemplateNameColumn.ColumnName]),
                                DataTemplateId = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.DataTemplateIdColumn.ColumnName]),
                                DataTemplateName = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.DataTemplateNameColumn.ColumnName]),
                                ModifiedBy = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.ModifiedByColumn.ColumnName]),
                                IsActive = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.IsActiveColumn.ColumnName]),
                                ModifiedOn = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.ModifiedOnColumn.ColumnName]),
                            };
                            lstPhysExamDataTemplates.Add(physExamDataTemplate);
                        }

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            PhysExamDataTemplateFill_JSON = js.Serialize(lstPhysExamDataTemplates),
                            dataResponse = GetSerializedData(MDVUtility.ToInt64(model.DataTemplateId))
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else if (MDVUtility.ToInt64(model.DataTemplateId) > 0)
                    {
                        var response = new
                        {
                            status = false,
                            Message = "No Physical Exam Data Template Found",
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
                        Message = MDVCustomException.HumanReadableMessage(objDataTemplate.Message),
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

        // Author: Abid Ali
        // Created Date: 15/June/2016
        //OverView: This function will handle insert/update of Patient Physical Exam Data Template
        public string insertUpdatePhysExamDataTemplate(PhysicalExamDataTemplateModel model)
        {
            DSPhysicalExamDataTemplate.PhysExamDataTemplateRow dr = null;
            DSPhysicalExamDataTemplate.PhysExamDataTemplateRow[] arrPhysicalExamDataTemplateRows = null;
            bool isNewRecord = false;
            string message = string.Empty;

            //Load physExam Data Templates
            DSPhysicalExamDataTemplate dsPhysExamDataTemplate = new DSPhysicalExamDataTemplate();
            BLObject<DSPhysicalExamDataTemplate> obj = BLLClinicalObj.loadPhysicalExamDataTemplate(MDVUtility.ToInt64(model.DataTemplateId), MDVUtility.ToInt64(model.TemplateId), MDVUtility.ToInt64(model.EntityId), model.ProviderIds, model.SpecialtyIds);
            dsPhysExamDataTemplate = obj.Data;

            if (obj.Data != null)
            {

                arrPhysicalExamDataTemplateRows = (DSPhysicalExamDataTemplate.PhysExamDataTemplateRow[])dsPhysExamDataTemplate.PhysExamDataTemplate.Select(dsPhysExamDataTemplate.PhysExamDataTemplate.DataTemplateIdColumn.ColumnName + "=" + model.DataTemplateId);
                if (arrPhysicalExamDataTemplateRows.Length > 0)
                {
                    dr = arrPhysicalExamDataTemplateRows[0];
                    message = Common.AppPrivileges.Update_Message;

                }
                else
                {
                    dr = dsPhysExamDataTemplate.PhysExamDataTemplate.NewPhysExamDataTemplateRow();
                    dr.DataTemplateId = MDVUtility.ToInt64(model.DataTemplateId);
                    dr.IsActive = true;
                    message = Common.AppPrivileges.Save_Message;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    isNewRecord = true;
                }
            }
            if (dr != null)
            {
                dr.DataTemplateName = model.DataTemplateName;
                dr.TemplateId = MDVUtility.ToInt64(model.TemplateId);
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.Comments = model.Comments;

                if (isNewRecord)
                {
                    dsPhysExamDataTemplate.PhysExamDataTemplate.AddPhysExamDataTemplateRow(dr);
                }

                #region Database Insertion/Updation

                BLObject<DSPhysicalExamDataTemplate> objPhysExamDataTemplate = BLLClinicalObj.insertUpdatePhysicalExamDataTemplate(dsPhysExamDataTemplate);
                long dataTemplateId = 0;
                if (obj.Data != null)
                    dataTemplateId = MDVUtility.ToInt64(dsPhysExamDataTemplate.PhysExamDataTemplate.Rows[0][dsPhysExamDataTemplate.PhysExamDataTemplate.DataTemplateIdColumn.ColumnName]);

                if (objPhysExamDataTemplate.Data != null && dataTemplateId != -1)
                {
                    if (dsPhysExamDataTemplate.PhysExamDataTemplate.Rows.Count > 0)
                    {
                        long DataTemplateId = MDVUtility.ToInt64(dsPhysExamDataTemplate.PhysExamDataTemplate.Rows[0][dsPhysExamDataTemplate.PhysExamDataTemplate.DataTemplateIdColumn.ColumnName]);
                        if (DataTemplateId > 0)
                        {
                            InsertUpdatePhysicalExamDataTemplateSystem(model.Systems, DataTemplateId);
                        }
                    }
                    var response = new
                    {
                        status = true,
                        message = message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = dataTemplateId == -1 ? "A Data Template with this name already exists. Try a different name." : objPhysExamDataTemplate.Message
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

            #endregion
        }

        // Author: Muhamamd Arshad
        // Created Date: 23/06/2016
        //OverView: This function will handle Active/InActive of Physical Exam Data Template
        public string activeInactivePhysExamDataTemplate(PhysicalExamDataTemplateModel model)
        {
            try
            {
                DSPhysicalExamDataTemplate dsPhysicalExamDataTemplate = new DSPhysicalExamDataTemplate();
                DSPhysicalExamDataTemplate.PhysExamDataTemplateRow dr = null;
                BLObject<DSPhysicalExamDataTemplate> obj = BLLClinicalObj.loadPhysicalExamDataTemplate(MDVUtility.ToInt64(model.DataTemplateId), 0, 0, "", "");
                dsPhysicalExamDataTemplate = obj.Data;
                DSPhysicalExamDataTemplate.PhysExamDataTemplateRow[] arrPhysicalExamDataTemplateRows = null;
                string message = string.Empty;
                if (obj.Data != null)
                {
                    arrPhysicalExamDataTemplateRows = (DSPhysicalExamDataTemplate.PhysExamDataTemplateRow[])dsPhysicalExamDataTemplate.PhysExamDataTemplate.Select(dsPhysicalExamDataTemplate.PhysExamDataTemplate.DataTemplateIdColumn.ColumnName + "=" + model.DataTemplateId);
                    if (arrPhysicalExamDataTemplateRows.Length > 0)
                    {
                        dr = arrPhysicalExamDataTemplateRows[0];
                        message = Common.AppPrivileges.Update_Message;

                        if (model.IsActive == "True")
                            dr.IsActive = true;
                        else if (model.IsActive == "False")
                            dr.IsActive = false;
                    }
                }

                #region Database Insertion/Updation

                BLObject<DSPhysicalExamDataTemplate> objUpdate = BLLClinicalObj.insertUpdatePhysicalExamDataTemplate(dsPhysicalExamDataTemplate);
                if (obj.Data != null)
                {

                    Int64 DataTemplateId = MDVUtility.ToInt64(dsPhysicalExamDataTemplate.Tables[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TableName].Rows[0][dsPhysicalExamDataTemplate.PhysExamDataTemplate.DataTemplateIdColumn.ColumnName]);

                    var response = new
                    {
                        status = true,
                        Message = message,
                        DataTemplateId = DataTemplateId,
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

        // Author: Muhamamd Arshad
        // Created Date: 23/06/2016
        //OverView: This function will handle delete of Physical Exam Data Template
        public string deletePhysExamDataTemplate(long physExamDataTemplateId)
        {
            try
            {
                DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
                BLObject<string> objSystemDelete = new BLObject<string>();

                objSystemDelete = BLLClinicalObj.deletePhysicalExamDataTemplate(physExamDataTemplateId);

                if (objSystemDelete.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,

                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objSystemDelete.Data
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public string GetSerializedData(long physExamDataTemplateId)
        {
            List<PhysicalDataTemplateSystem> patientPhysicalExamSystems = new List<PhysicalDataTemplateSystem>();
            try
            {
                if (physExamDataTemplateId > 0)
                {
                    //Start/ populating systems
                    DSPhysicalExamDataTemplate dsPhysicalExamDataTemplateSystem = null;
                    BLObject<DSPhysicalExamDataTemplate> obj = BLLClinicalObj.LoadPhysicalExamDataTemplateSystem(physExamDataTemplateId);
                    dsPhysicalExamDataTemplateSystem = obj.Data;
                    if (dsPhysicalExamDataTemplateSystem.Tables[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow drSystem in dsPhysicalExamDataTemplateSystem.Tables[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.TableName].Rows)
                        {

                            //To save the System 
                            PhysicalDataTemplateSystem system = new PhysicalDataTemplateSystem();
                            long phyExamDataTemplateSystemId = MDVUtility.ToInt64(drSystem[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.DataTemplateSysIdColumn.ColumnName]);
                            long systemId = MDVUtility.ToInt64(drSystem[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.SystemIdColumn.ColumnName]);
                            if (phyExamDataTemplateSystemId > 0)
                            {
                                system.DataTemplateId = physExamDataTemplateId;
                                system.PhysicalExamSystemId = phyExamDataTemplateSystemId;
                                system.SystemId = systemId;
                                system.IsNormal = Convert.ToBoolean(drSystem[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.IsNormalColumn.ColumnName]);
                                system.DataTemplateSysId = phyExamDataTemplateSystemId;

                                //Start/ populating sections
                                DSPhysicalExamDataTemplate dsPhysicalExamDataTemplateSection = null;
                                BLObject<DSPhysicalExamDataTemplate> objSystemSection = BLLClinicalObj.LoadPhysicalExamDataTemplateSystemSection(physExamDataTemplateId, systemId);
                                dsPhysicalExamDataTemplateSection = objSystemSection.Data;
                                if (dsPhysicalExamDataTemplateSection.Tables[dsPhysicalExamDataTemplateSection.PhysExamDataTemplateSection.TableName].Rows.Count > 0)
                                {
                                    foreach (DataRow drSection in dsPhysicalExamDataTemplateSection.Tables[dsPhysicalExamDataTemplateSection.PhysExamDataTemplateSection.TableName].Rows)
                                    {
                                        long physicalExamSectionId = MDVUtility.ToInt64(drSection[dsPhysicalExamDataTemplateSection.PhysExamDataTemplateSection.DataTemplateSectionIdColumn.ColumnName]);
                                        long sectionId = MDVUtility.ToInt64(drSection[dsPhysicalExamDataTemplateSection.PhysExamDataTemplateSection.SectionIdColumn.ColumnName]);


                                        //To fetch the Section
                                        PhysicalDataTemplateSection section = new PhysicalDataTemplateSection();

                                        //Start/ populating Characteristics
                                        DSPhysicalExamDataTemplate dsPhysicalExamDataTemplateChar = null;
                                        BLObject<DSPhysicalExamDataTemplate> objSectionCharacteristic = BLLClinicalObj.LoadPhysicalExamDataTemplateSystemChar(physExamDataTemplateId, sectionId);
                                        dsPhysicalExamDataTemplateChar = objSectionCharacteristic.Data;

                                        if (dsPhysicalExamDataTemplateChar.Tables[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateChar.TableName].Rows.Count > 0)
                                        {
                                            foreach (DataRow drSectionCharacteristic in dsPhysicalExamDataTemplateChar.Tables[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateChar.TableName].Rows)
                                            {
                                                long sectionCharacteristicId = MDVUtility.ToInt64(drSectionCharacteristic[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateChar.DataTemplateCharIdColumn.ColumnName]);
                                                long characteristicId = MDVUtility.ToInt64(drSectionCharacteristic[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateChar.CharIdColumn.ColumnName]);
                                                string isCharPositive = Convert.ToBoolean(drSectionCharacteristic[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateChar.IsPositiveColumn.ColumnName]) ? "True" : "False";

                                                //To Store Characteristics
                                                PhysicalDataTemplateChar charicteristics = new PhysicalDataTemplateChar();

                                                charicteristics.SectionCharacteristicDetailModel = new PhysicalExamDataTemplateDetailModel();

                                                BLObject<DSPhysicalExamDataTemplate> objSectionCharacteristicDetail = BLLClinicalObj.LoadPhysicalExamDataTemplateDetail(physExamDataTemplateId, characteristicId, Characteristic, 0);
                                                dsPhysicalExamDataTemplateChar = objSectionCharacteristicDetail.Data;

                                                // Fill detail
                                                foreach (DataRow drSectionCharacteristicDetail in dsPhysicalExamDataTemplateChar.Tables[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.TableName].Rows)
                                                {

                                                    charicteristics.SectionCharacteristicDetailModel.DetailId = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.DetailIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.IsCharacteristicPositive = isCharPositive;

                                                    charicteristics.SectionCharacteristicDetailModel.SectionId = MDVUtility.ToStr(sectionId);
                                                    charicteristics.SectionCharacteristicDetailModel.SystemId = MDVUtility.ToStr(systemId);

                                                    charicteristics.SectionCharacteristicDetailModel.CharacteristicId = MDVUtility.ToStr(characteristicId);
                                                    charicteristics.SectionCharacteristicDetailModel.AggravatedBy = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.AggravatedByIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Agggravatedby_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Agggravatedby_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Associatedwith = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.AssociatedWithColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Character = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.CharacterIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Character_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Character_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Context = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.ContextIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Context_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Context_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Course = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.CourseIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Course_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Course_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.DurationLength = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.DurationLengthColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.DurationPeriod = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.DurationPeriodIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.DurationPeriod_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.DurationPeriod_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Frequency = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.FrequencyIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Frequency_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Frequency_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Location = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.LocationColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Onset = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.OnsetColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Pattern = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.PatternIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Pattern_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Pattern_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Percipitatedby = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.PrecipitatedbyColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PrevHistory = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.PrevHistoryColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Radiation = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.RadiationIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Radiation_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Radiation_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Relievedby = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.RelievedbyIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Relievedby_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Relievedby_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Severity = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.SeverityIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Severity_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Severity_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Status = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.StatusIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.Status_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Status_textColumn.ColumnName]);

                                                }

                                                //Start/ populating SubCharacteristics
                                                DSPhysicalExamDataTemplate dsPhysicalExamSectionSubCharacteristic = null;
                                                BLObject<DSPhysicalExamDataTemplate> objSectionSubCharacteristic = BLLClinicalObj.loadPhysicalExamDataTemplateSystemSubChar(physExamDataTemplateId, characteristicId);
                                                dsPhysicalExamSectionSubCharacteristic = objSectionSubCharacteristic.Data;
                                                if (dsPhysicalExamSectionSubCharacteristic.Tables[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateSubChar.TableName].Rows.Count > 0)
                                                {
                                                    foreach (DataRow drSectionSubCharacteristic in dsPhysicalExamSectionSubCharacteristic.Tables[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateSubChar.TableName].Rows)
                                                    {
                                                        long sectionSubCharacteristicId = MDVUtility.ToInt64(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateSubChar.DataTemplateSubCharIdColumn.ColumnName]);
                                                        long SubCharacteristicId = MDVUtility.ToInt64(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateSubChar.SubCharIdColumn.ColumnName]);

                                                        PhysicalDataTemplateSubChar charSubCharacteristic = new PhysicalDataTemplateSubChar();

                                                        BLObject<DSPhysicalExamDataTemplate> objSectionSubCharacteristicDetail = BLLClinicalObj.LoadPhysicalExamDataTemplateDetail(physExamDataTemplateId, SubCharacteristicId, SubCharacteristic, 0);
                                                        dsPhysicalExamSectionSubCharacteristic = objSectionSubCharacteristicDetail.Data;
                                                        charSubCharacteristic.SubCharacteristicDetailModel = new PhysicalExamDataTemplateDetailModel();

                                                        // Fill detail
                                                        foreach (DataRow drSectionSubCharacteristicDetail in dsPhysicalExamSectionSubCharacteristic.Tables[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.TableName].Rows)
                                                        {
                                                            charSubCharacteristic.SubCharacteristicDetailModel.DetailId = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.DetailIdColumn.ColumnName]);

                                                            charSubCharacteristic.SubCharacteristicDetailModel.SubCharacteristicId = MDVUtility.ToStr(SubCharacteristicId);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.IsSubCharacteristicPositive = Convert.ToBoolean(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateSubChar.IsPositiveColumn.ColumnName]) ? "True" : "False";
                                                            charSubCharacteristic.SubCharacteristicDetailModel.CharacteristicId = MDVUtility.ToStr(characteristicId);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.IsCharacteristicPositive = isCharPositive;
                                                            charSubCharacteristic.SubCharacteristicDetailModel.SectionId = MDVUtility.ToStr(sectionId);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.SystemId = MDVUtility.ToStr(systemId);

                                                            charSubCharacteristic.SubCharacteristicDetailModel.AggravatedBy = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.AggravatedByIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Agggravatedby_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Agggravatedby_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Associatedwith = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.AssociatedWithColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Character = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.CharacterIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Character_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Character_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Context = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.ContextIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Context_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Context_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Course = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.CourseIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Course_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Course_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.DurationLength = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.DurationLengthColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.DurationPeriod = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.DurationPeriodIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.DurationPeriod_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.DurationPeriod_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Frequency = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.FrequencyIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Frequency_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Frequency_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Location = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.LocationColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Onset = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.OnsetColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Pattern = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.PatternIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Pattern_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Pattern_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Percipitatedby = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.PrecipitatedbyColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PrevHistory = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.PrevHistoryColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Radiation = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.RadiationIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Radiation_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Radiation_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Relievedby = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.RelievedbyIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Relievedby_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Relievedby_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Severity = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.SeverityIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Severity_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Severity_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Status = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.StatusIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.Status_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Status_textColumn.ColumnName]);

                                                        }

                                                        charSubCharacteristic.DataTemplateSubCharId = sectionSubCharacteristicId;
                                                        charSubCharacteristic.SubCharacteristicId = SubCharacteristicId;
                                                        charSubCharacteristic.Comments = MDVUtility.ToStr(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateSubChar.CommentsColumn.ColumnName]);
                                                        charSubCharacteristic.IsPositive = MDVUtility.ToStr(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateSubChar.IsPositiveColumn.ColumnName]) == "True" ? true : false;
                                                        charSubCharacteristic.IsNegative = MDVUtility.ToStr(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateSubChar.IsNegativeColumn.ColumnName]) == "True" ? true : false;

                                                        if (charicteristics.SubCharacteristics == null)
                                                            charicteristics.SubCharacteristics = new List<PhysicalDataTemplateSubChar>();
                                                        charicteristics.SubCharacteristics.Add(charSubCharacteristic);
                                                    }
                                                }
                                                //End/ populating SubCharacteristics

                                                charicteristics.Comments = MDVUtility.ToStr(drSectionCharacteristic[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateChar.CommentsColumn.ColumnName]);
                                                charicteristics.IsPositive = MDVUtility.ToStr(drSectionCharacteristic[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateChar.IsPositiveColumn.ColumnName]) == "True" ? true : false;
                                                charicteristics.IsNegative = MDVUtility.ToStr(drSectionCharacteristic[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateChar.IsNegativeColumn.ColumnName]) == "True" ? true : false;
                                                charicteristics.DataTemplateCharId = sectionCharacteristicId;
                                                charicteristics.SectionCharacteristicId = characteristicId;
                                                if (section.Characteristics == null)
                                                    section.Characteristics = new List<PhysicalDataTemplateChar>();
                                                section.Characteristics.Add(charicteristics);
                                            }
                                            //End/ populating Characteristics
                                            section.DataTemplateSectionId = physicalExamSectionId;
                                            section.SectionId = sectionId;

                                            if (system.Sections == null)
                                                system.Sections = new List<PhysicalDataTemplateSection>();
                                            system.Sections.Add(section);
                                        }
                                    }

                                }
                                if (MDVUtility.ToStr(drSystem[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.IsNormalColumn.ColumnName]).ToLower() == "true")
                                {
                                    system.IsNormal = true;
                                    // system.Comments = MDVUtility.ToStr(drSystem[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.NormalCommentsColumn.ColumnName]);
                                }
                                else
                                    system.IsNormal = false;
                                patientPhysicalExamSystems.Add(system);

                                //End/ populating sections
                            }

                        }
                    }
                    //End/ populating systems
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    return js.Serialize(patientPhysicalExamSystems);
                }
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        #endregion


        #region Data Template sys,sec,char and subChar (Insert Update Delete)


        // Author: Muhammad Arshad
        // Created Date: 10/06/2016
        //OverView: This function will handle Insert/Update of Data Template System

        private void InsertUpdatePhysicalExamDataTemplateSystem(List<PhysicalDataTemplateSystem> lstSystemModel, long physicalExamDataTemplateId)
        {
            DSPhysicalExamDataTemplate dsPhysicalExamDataTemplate = new DSPhysicalExamDataTemplate();
            string currentPatPhysSysId = string.Empty;
            BLObject<DSPhysicalExamDataTemplate> obj = BLLClinicalObj.LoadPhysicalExamDataTemplateSystem(physicalExamDataTemplateId);
            dsPhysicalExamDataTemplate = obj.Data;

            //Start//Delete Systems
            List<long> systemIds = new List<long>();
            lstSystemModel.ForEach(x => systemIds.Add(x.SystemId));
            string deleteMessage = deletePhysExamDataTemplateSystem(systemIds, dsPhysicalExamDataTemplate);
            //End//Delete Systems
            int counter = 0;
            foreach (PhysicalDataTemplateSystem item in lstSystemModel)
            {

                DSPhysicalExamDataTemplate dsPhysicalExamDataTemplateSection = new DSPhysicalExamDataTemplate();
                DSPhysicalExamDataTemplate dsPhysicalExamDataTemplateChar = new DSPhysicalExamDataTemplate();
                DSPhysicalExamDataTemplate dsPhysicalExamDataTemplateSubChar = new DSPhysicalExamDataTemplate();

                counter = buildDataTemplateSystemRow(physicalExamDataTemplateId, item, dsPhysicalExamDataTemplate, counter);

                if (item.Sections != null)
                {
                    if (item.IsNormal == true)
                    {
                        List<long> sectionIds = new List<long>();
                        item.Sections.ForEach(x => sectionIds.Add(x.SectionId));
                        BLObject<DSPhysicalExamDataTemplate> objSection = BLLClinicalObj.LoadPhysicalExamDataTemplateSystemSection(physicalExamDataTemplateId, item.SystemId);
                        dsPhysicalExamDataTemplateSection = objSection.Data;
                        deletePhysExamDataTemplateSection(sectionIds, dsPhysicalExamDataTemplateSection, item.SystemId);
                    }
                    else
                    {
                        // section insert update call 
                        InsertUpdatePhysicalExamDataTemplateSection(item.Sections, dsPhysicalExamDataTemplateSection, physicalExamDataTemplateId, item.SystemId, dsPhysicalExamDataTemplateChar, dsPhysicalExamDataTemplateSubChar);

                    }
                }
            }

            #region Database Insertion/Updation For systems
            if (dsPhysicalExamDataTemplate.PhysExamDataTemplateSys.Rows.Count > 0)
            {
                BLObject<DSPhysicalExamDataTemplate> objInsertedDataTemplateSystem = BLLClinicalObj.InsertUpdatePhysicalExamDataTemplateSystem(dsPhysicalExamDataTemplate);
            }
            #endregion          
        }

        // Author: Muhammad Arshad
        // Created Date: 16/06/2016
        //OverView: This function will handle Insert/Update of Data Template Section
        private void InsertUpdatePhysicalExamDataTemplateSection(List<PhysicalDataTemplateSection> lstSectionModel, DSPhysicalExamDataTemplate dsPhysicalExamDataTemplateSection, long physicalExamDataTemplateId, long systemId, DSPhysicalExamDataTemplate dsChar, DSPhysicalExamDataTemplate dsSubChar)
        {

            string currentPatPhysSysId = string.Empty;
            BLObject<DSPhysicalExamDataTemplate> obj = BLLClinicalObj.LoadPhysicalExamDataTemplateSystemSection(physicalExamDataTemplateId, systemId);
            dsPhysicalExamDataTemplateSection = obj.Data;

            //Start//Delete Sections
            List<long> sectionIds = new List<long>();
            lstSectionModel.ForEach(x => sectionIds.Add(x.SectionId));
            string deleteMessage = deletePhysExamDataTemplateSection(sectionIds, dsPhysicalExamDataTemplateSection, systemId);
            //End//Delete Sections
            int counter = 0;
            foreach (PhysicalDataTemplateSection item in lstSectionModel)
            {
                counter = buildDataTemplateSectionRow(physicalExamDataTemplateId, item, systemId, dsPhysicalExamDataTemplateSection, counter);

                if (item.Characteristics != null)
                {
                    InsertUpdatePhysicalExamDataTemplateChar(item.Characteristics, dsChar, physicalExamDataTemplateId, item.SectionId, dsSubChar);

                }
            }
            if (dsPhysicalExamDataTemplateSection.PhysExamDataTemplateSection.Rows.Count > 0)
            {
                BLLClinicalObj.InsertUpdatePhysicalExamDataTemplateSection(dsPhysicalExamDataTemplateSection);
            }
        }


        // Author: Muhammad Arshad
        // Created Date: 16/06/2016
        //OverView: This function will handle Insert/Update of Data Template Char
        private void InsertUpdatePhysicalExamDataTemplateChar(List<PhysicalDataTemplateChar> lstCharModel, DSPhysicalExamDataTemplate dsPhysicalExamDataTemplateChar, long physicalExamDataTemplateId, long physicalExamDataTemplateSectionId, DSPhysicalExamDataTemplate dsPhysicalExamDataTemplateSubChar)
        {

            string currentPatPhysSysId = string.Empty;
            BLObject<DSPhysicalExamDataTemplate> obj = BLLClinicalObj.LoadPhysicalExamDataTemplateSystemChar(physicalExamDataTemplateId, physicalExamDataTemplateSectionId);
            dsPhysicalExamDataTemplateChar = obj.Data;

            //Start//Delete chars
            List<long> charIds = new List<long>();
            lstCharModel.ForEach(x => charIds.Add(x.SectionCharacteristicId));
            string deleteMessage = deletePhysExamDataTemplateChar(charIds, dsPhysicalExamDataTemplateChar);
            //End//Delete chars
            int counter = 0;

            //Start/Char Detail Model 
            int charDetailCounter = 0;
            DSPhysicalExamDataTemplate dsDataTemplateDetailModel = new DSPhysicalExamDataTemplate();
            //End/Char Detail Model 

            foreach (PhysicalDataTemplateChar item in lstCharModel)
            {

                //Start//For char Detail Insert/Update/Delete
                if (item.SectionCharacteristicDetailModel != null)
                {
                    dsDataTemplateDetailModel.PhysExamDataTemplateDetail.Rows.Add(buildRowPhysicalExamDataTemplateDetail(physicalExamDataTemplateId, item.SectionCharacteristicId, Characteristic, item.SectionCharacteristicDetailModel, ref charDetailCounter).ItemArray);
                    //if (dsDataTemplateDetailModelTemp != null)
                    //{
                    //    dsDataTemplateDetailModel.PhysExamDataTemplateDetail.AddPhysExamDataTemplateDetailRow()
                    //} 
                }
                else
                {
                    DSPhysicalExamDataTemplate dsPhysExamDataTemplateDetail = new DSPhysicalExamDataTemplate();
                    BLObject<DSPhysicalExamDataTemplate> objDetail = BLLClinicalObj.LoadPhysicalExamDataTemplateDetail(physicalExamDataTemplateId, item.SectionCharacteristicId, Characteristic, 0);
                    dsPhysExamDataTemplateDetail = objDetail.Data;


                    if (objDetail.Data != null)
                    {
                        //foreach (DSPhysicalExamDataTemplate.PhysExamDataTemplateDetailRow row in dsPhysExamDataTemplateDetail.PhysExamDataTemplateDetail.Rows)
                        // {

                        if (dsPhysExamDataTemplateDetail.PhysExamDataTemplateDetail.Rows.Count > 0)
                        {
                            deletePhysExamDataTemplateDetail(physicalExamDataTemplateId, item.SectionCharacteristicId, Characteristic, "0");
                        }
                        // }
                    }
                }
                //End//For char Detail Insert/Update/Delete
                counter = buildDataTemplateCharRow(physicalExamDataTemplateId, item, physicalExamDataTemplateSectionId, dsPhysicalExamDataTemplateChar, counter);

                if (item.SubCharacteristics != null)
                {
                    InsertUpdatePhysicalExamDataTemplateSubChar(item.SubCharacteristics, dsPhysicalExamDataTemplateSubChar, physicalExamDataTemplateId, item.SectionCharacteristicId);
                }
            }

            if (dsPhysicalExamDataTemplateChar.PhysExamDataTemplateChar.Rows.Count > 0)
            {
                BLLClinicalObj.InsertUpdatePhysicalExamDataTemplateChar(dsPhysicalExamDataTemplateChar);
            }

            if (dsDataTemplateDetailModel.PhysExamDataTemplateDetail.Rows.Count > 0)
            {
                BLLClinicalObj.insertUpdatePhysicalExamDataTemplateDetail(dsDataTemplateDetailModel, Characteristic);
            }
        }

        // Author: Muhammad Arshad
        // Created Date: 16/06/2016
        //OverView: This function will handle Insert/Update of Data Template SubChar
        private void InsertUpdatePhysicalExamDataTemplateSubChar(List<PhysicalDataTemplateSubChar> lstSubCharModel, DSPhysicalExamDataTemplate dsPhysicalExamDataTemplateSubChar, long physicalExamDataTemplateId, long physicalExamDataTemplateCharId)
        {

            string currentPatPhysSysId = string.Empty;
            BLObject<DSPhysicalExamDataTemplate> obj = BLLClinicalObj.loadPhysicalExamDataTemplateSystemSubChar(physicalExamDataTemplateId, physicalExamDataTemplateCharId);
            dsPhysicalExamDataTemplateSubChar = obj.Data;

            //Start//Delete subChars
            List<long> subCharIds = new List<long>();
            lstSubCharModel.ForEach(x => subCharIds.Add(x.SubCharacteristicId));
            string deleteMessage = deletePhysExamDataTemplateSubChar(subCharIds, dsPhysicalExamDataTemplateSubChar);
            //End//Delete subChars

            int counter = 0;

            //Start/SubChar Detail Model 
            int subCharDetailCounter = 0;
            DSPhysicalExamDataTemplate dsDataTemplateDetailModel = new DSPhysicalExamDataTemplate();
            //End/SubChar Detail Model 

            foreach (PhysicalDataTemplateSubChar item in lstSubCharModel)
            {
                //Start//For subChar Detail Insert/Update/Delete
                if (item.SubCharacteristicDetailModel != null)
                {
                    dsDataTemplateDetailModel.PhysExamDataTemplateDetail.Rows.Add(buildRowPhysicalExamDataTemplateDetail(physicalExamDataTemplateId, item.SubCharacteristicId, SubCharacteristic, item.SubCharacteristicDetailModel, ref subCharDetailCounter).ItemArray);
                    //if (dsDataTemplateDetailModelTemp != null)
                    //{
                    //    dsDataTemplateDetailModel.PhysExamDataTemplateDetail.Merge(dsDataTemplateDetailModelTemp.PhysExamDataTemplateDetail);
                    //}

                }
                else
                {
                    DSPhysicalExamDataTemplate dsPhysExamDataTemplateDetail = new DSPhysicalExamDataTemplate();
                    BLObject<DSPhysicalExamDataTemplate> objDetail = BLLClinicalObj.LoadPhysicalExamDataTemplateDetail(physicalExamDataTemplateId, item.SubCharacteristicId, SubCharacteristic, 0);
                    dsPhysExamDataTemplateDetail = objDetail.Data;

                    if (objDetail.Data != null)
                    {
                        // foreach (DSPhysicalExamDataTemplate.PhysExamDataTemplateDetailRow row in dsPhysExamDataTemplateDetail.PhysExamDataTemplateDetail.Rows)
                        //{
                        if (dsPhysExamDataTemplateDetail.PhysExamDataTemplateDetail.Rows.Count > 0)
                        {
                            deletePhysExamDataTemplateDetail(physicalExamDataTemplateId, item.SubCharacteristicId, SubCharacteristic, "0");
                        }
                        // }
                    }
                }
                //End//For subChar Detail Insert/Update/Delete
                counter = buildDataTemplateSubCharRow(physicalExamDataTemplateId, item, dsPhysicalExamDataTemplateSubChar, physicalExamDataTemplateCharId, counter);
            }
            if (dsPhysicalExamDataTemplateSubChar.PhysExamDataTemplateSubChar.Rows.Count > 0)
            {
                BLLClinicalObj.InsertUpdatePhysicalExamDataTemplateSubChar(dsPhysicalExamDataTemplateSubChar);
            }
            if (dsDataTemplateDetailModel.PhysExamDataTemplateDetail.Rows.Count > 0)
            {
                BLLClinicalObj.insertUpdatePhysicalExamDataTemplateDetail(dsDataTemplateDetailModel, SubCharacteristic);
            }
        }


        /// <summary>
        /// Module Name: deletePatientPhysicalExamSystem
        /// Author: Abid Ali
        /// Created Date: 18-02-2016
        /// Description: Deletes patient physical exam system
        /// </summary>
        /// <param name="patientPhysicalExamId" type="long">patientPhysicalExam Id</param> 
        /// <param name="systemId" type="long">systemId</param>    
        public string deletePhysExamDataTemplateSystem(List<long> systemIds, DSPhysicalExamDataTemplate dsPhysicalExamDataTemplate = null)
        {
            try
            {
                BLObject<string> objSectionDelete = new BLObject<string>();
                //Check if dataset is null then delete by template system ids
                if (dsPhysicalExamDataTemplate == null)
                {
                    //Data template system ids = systemIds
                    systemIds.ForEach(dataTemplateSysId =>
                    {
                        objSectionDelete = BLLClinicalObj.DeletePhysicalExamDataTemplateSystem(MDVUtility.ToStr(dataTemplateSysId));
                    });
                }
                else
                {
                    string filteredSystemIds = systemIds.Count > 0 ? string.Join<long>(",", systemIds) : "-1";

                    DSPhysicalExamDataTemplate.PhysExamDataTemplateSysRow[] arrCurrentRows = (DSPhysicalExamDataTemplate.PhysExamDataTemplateSysRow[])dsPhysicalExamDataTemplate.PhysExamDataTemplateSys.Select(dsPhysicalExamDataTemplate.PhysExamDataTemplateSys.SystemIdColumn.ColumnName + " NOT IN (" + filteredSystemIds + ")");
                    foreach (DSPhysicalExamDataTemplate.PhysExamDataTemplateSysRow row in arrCurrentRows)
                    {
                        objSectionDelete = BLLClinicalObj.DeletePhysicalExamDataTemplateSystem(MDVUtility.ToStr(row[dsPhysicalExamDataTemplate.PhysExamDataTemplateSys.DataTemplateSysIdColumn.ColumnName]));
                        row.Delete();
                        row.AcceptChanges();
                    }

                }
                return Common.AppPrivileges.Delete_Message;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Module Name: deletePhysicalExamDataTemplateSystemSection
        /// Author: Abid Ali
        /// Created Date: 18-02-2016
        /// Description: Deletes patient physical exam system section
        /// </summary>
        /// <param name="PhysicalExamDataTemplateId" type="long">PhysicalExamDataTemplate Id</param> 
        /// <param name="systemId" type="long">systemId</param> 
        /// <param name="sectionId" type="long">sectionId</param>  
        public string deletePhysExamDataTemplateSection(List<long> sectionIds, DSPhysicalExamDataTemplate dsPhysicalExamDataTemplate = null, long systemId = 0)
        {
            try
            {
                BLObject<string> objSectionDelete = new BLObject<string>();
                if (dsPhysicalExamDataTemplate == null)
                {
                    sectionIds.ForEach(id =>
                    {
                        objSectionDelete = BLLClinicalObj.DeletePhysicalExamDataTemplateSection(MDVUtility.ToStr(id));
                    });
                }
                else
                {
                    string filteredSectionIds = sectionIds.Count > 0 ? string.Join<long>(",", sectionIds) : "-1";
                    DSPhysicalExamDataTemplate.PhysExamDataTemplateSectionRow[] arrCurrentRows = (DSPhysicalExamDataTemplate.PhysExamDataTemplateSectionRow[])dsPhysicalExamDataTemplate.PhysExamDataTemplateSection.Select(dsPhysicalExamDataTemplate.PhysExamDataTemplateSection.SectionIdColumn.ColumnName + " NOT IN (" + filteredSectionIds + ")");
                    foreach (DSPhysicalExamDataTemplate.PhysExamDataTemplateSectionRow row in arrCurrentRows)
                    {
                        objSectionDelete = BLLClinicalObj.DeletePhysicalExamDataTemplateSection(MDVUtility.ToStr(row[dsPhysicalExamDataTemplate.PhysExamDataTemplateSection.DataTemplateSectionIdColumn.ColumnName]));
                        row.Delete();
                        row.AcceptChanges();
                    }

                }
                return Common.AppPrivileges.Delete_Message;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Module Name: deletePhysicalExamDataTemplateSystemSectionCharacteristic
        /// Author: Abid Ali
        /// Created Date: 18-02-2016
        /// Description: Deletes patient physical exam system section Characteristic
        /// </summary>
        /// <param name="PhysicalExamDataTemplateId" type="long">PhysicalExamDataTemplate Id</param> 
        /// <param name="sectionId" type="long">section Id</param>  
        /// <param name="characteristicId" type="long">characteristic Id</param> 
        public string deletePhysExamDataTemplateChar(List<long> charIds, DSPhysicalExamDataTemplate dsPhysicalExamDataTemplate = null)
        {
            try
            {
                BLObject<string> objSectionDelete = new BLObject<string>();
                if (dsPhysicalExamDataTemplate == null)
                {
                    charIds.ForEach(id =>
                    {
                        // id = datatemplateCharId (PK)
                        objSectionDelete = BLLClinicalObj.DeletePhysicalExamDataTemplateChar(MDVUtility.ToStr(id));
                    });
                }
                else
                {
                    //if (charIds.Count > 0)
                    //{
                    string filteredCharIds = charIds.Count > 0 ? string.Join<long>(",", charIds) : "-1";
                    // string filteredCharIds = string.Join<long>(",", charIds);
                    DSPhysicalExamDataTemplate.PhysExamDataTemplateCharRow[] arrCurrentRows = (DSPhysicalExamDataTemplate.PhysExamDataTemplateCharRow[])dsPhysicalExamDataTemplate.PhysExamDataTemplateChar.Select(dsPhysicalExamDataTemplate.PhysExamDataTemplateChar.CharIdColumn.ColumnName + " NOT IN (" + filteredCharIds + ")");
                    foreach (DSPhysicalExamDataTemplate.PhysExamDataTemplateCharRow row in arrCurrentRows)
                    {
                        objSectionDelete = BLLClinicalObj.DeletePhysicalExamDataTemplateChar(MDVUtility.ToStr(row[dsPhysicalExamDataTemplate.PhysExamDataTemplateChar.DataTemplateCharIdColumn.ColumnName]));
                        row.Delete();
                        row.AcceptChanges();
                    }
                    // }
                }
                return Common.AppPrivileges.Delete_Message;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Module Name: deletePhysicalExamDataTemplateSystemSectionCharacteristicSubCharacteristic
        /// Author: Abid Ali
        /// Created Date: 18-02-2016
        /// Description: Deletes patient physical exam system section Characteristic SubCharacteristic
        /// </summary>
        /// <param name="PhysicalExamDataTemplateId" type="long">PhysicalExamDataTemplate Id</param> 
        /// <param name="characteristicId" type="long">characteristic Id</param> 
        /// <param name="subCharacteristicId" type="long">subCharacteristic Id</param>  
        public string deletePhysExamDataTemplateSubChar(List<long> subCharIds, DSPhysicalExamDataTemplate dsPhysicalExamDataTemplate = null)
        {
            try
            {
                BLObject<string> objSectionDelete = new BLObject<string>();
                if (dsPhysicalExamDataTemplate == null)
                {
                    subCharIds.ForEach(id =>
                    {
                        // id = datatemplateSubCharId (PK)
                        objSectionDelete = BLLClinicalObj.DeletePhysicalExamDataTemplateSubChar(MDVUtility.ToStr(id));
                    });
                }
                else
                {
                    //if (subCharIds.Count > 0)
                    //{
                    string filteredSubCharIds = subCharIds.Count > 0 ? string.Join<long>(",", subCharIds) : "-1";
                    // string filteredSubCharIds = string.Join<long>(",", subCharIds);
                    DSPhysicalExamDataTemplate.PhysExamDataTemplateSubCharRow[] arrCurrentRows = (DSPhysicalExamDataTemplate.PhysExamDataTemplateSubCharRow[])dsPhysicalExamDataTemplate.PhysExamDataTemplateSubChar.Select(dsPhysicalExamDataTemplate.PhysExamDataTemplateSubChar.SubCharIdColumn.ColumnName + " NOT IN (" + filteredSubCharIds + ")");
                    foreach (DSPhysicalExamDataTemplate.PhysExamDataTemplateSubCharRow row in arrCurrentRows)
                    {
                        objSectionDelete = BLLClinicalObj.DeletePhysicalExamDataTemplateSubChar(MDVUtility.ToStr(row[dsPhysicalExamDataTemplate.PhysExamDataTemplateSubChar.DataTemplateSubCharIdColumn.ColumnName]));
                        row.Delete();
                        row.AcceptChanges();

                    }
                    // }
                }
                return Common.AppPrivileges.Delete_Message;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Module Name: deletePhysicalExamDataTemplateDetail
        /// Author: Abid Ali
        /// Created Date: 18-02-2016
        /// Description: Deletes patient physical exam detail 
        /// </summary>
        /// <param name="PhysicalExamDataTemplateId" type="long">PhysicalExamDataTemplate Id</param> 
        /// <param name="parentId" type="long">characteristic/subCharacteristic Id</param> 
        /// <param name="parentType" type="string">characteristic/subCharacteristic</param>  
        public string deletePhysExamDataTemplateDetail(long PhysicalExamDataTemplateId, long parentId, string parentType, string detailId)
        {
            try
            {

                BLObject<string> objDetailDelete = new BLObject<string>();
                objDetailDelete = BLLClinicalObj.DeletePhysicalExamDataTemplateDetail(PhysicalExamDataTemplateId, parentType, parentId, detailId);


                if (objDetailDelete.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Delete_Message,

                    };
                    return response.message.ToString();
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objDetailDelete.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

    }
}
