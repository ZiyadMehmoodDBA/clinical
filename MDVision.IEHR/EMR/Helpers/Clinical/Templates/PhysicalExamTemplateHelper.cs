/* Author:  Muhammad Arshad
 * Created Date: 26/02/2016
 * OverView: Created to handel Physical Exam Template
 */

using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.IEHR.EMR.Model.PhysicalExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MDVision.IEHR.EMR.Model.Templates;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using System.IO;
using MDVision.IEHR.Common;
using System.Reflection;
using System.Xml.Serialization;
using MDVision.IEHR.EMR.Helpers.Clinical.ClinicalNotes;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates
{
    public class PhysicalExamTemplateHelper
    {
        private BLLClinical BLLClinicalObj = null;
        private BLLPhysicalExamECW BLLPhysicalExamECWObj = null;
        public PhysicalExamTemplateHelper()
        {
            BLLClinicalObj = new BLLClinical();
            BLLPhysicalExamECWObj = new BLLPhysicalExamECW();
        }
        private static PhysicalExamTemplateHelper _instance = null;
        public static PhysicalExamTemplateHelper Instance()
        {
            if (_instance == null)
                _instance = new PhysicalExamTemplateHelper();
            return _instance;
        }

        #region Patient Physical Exam Fill, Save and Update Methods

        // Author: Muhammad Arshad
        // Created Date: 04/03/2016
        //OverView: This function will handle fill of Physical Exam Template
        public string fillPatientPhysicalExamTemplate(Int64 templateId, Int64 entityId, string providerIds = "", string specialtyIds = "", int? IsActive = 1)
        {
            try
            {
                DSPhysicalExamTemplate dsPhysicalExamTemplate = null;
                BLObject<DSPhysicalExamTemplate> objTemplate = BLLClinicalObj.loadPhysicalExamTemplate(templateId, entityId, IsActive, providerIds, specialtyIds, "1", "");
                if (objTemplate.Data != null)
                {
                    dsPhysicalExamTemplate = objTemplate.Data;
                    if (dsPhysicalExamTemplate.Tables[dsPhysicalExamTemplate.PhysExamTemplate.TableName].Rows.Count > 0)
                    {
                        List<Dictionary<string, string>> lstPhysExamTemplates = new List<Dictionary<string, string>>();
                        foreach (DataRow dr in dsPhysicalExamTemplate.Tables[dsPhysicalExamTemplate.PhysExamTemplate.TableName].Rows)
                        {
                            //DataRow dr = dsPhysicalExamTemplate.Tables[dsPhysicalExamTemplate.PhysExamTemplate.TableName].Rows[0];
                            var physicalExamTemplateKeyValues = new Dictionary<string, string>
                        {
                            { "physExamTemplateId",  MDVUtility.ToStr(dr[dsPhysicalExamTemplate.PhysExamTemplate.TemplateIdColumn.ColumnName])},
                            { "physExamTemplateName",  MDVUtility.ToStr(dr[dsPhysicalExamTemplate.PhysExamTemplate.TemplateNameColumn.ColumnName])},
                            { "SpecialtyIds", MDVUtility.ToStr(dr[dsPhysicalExamTemplate.PhysExamTemplate.SpecialtyIdsColumn.ColumnName])},
                            { "ProviderIds", MDVUtility.ToStr(dr[dsPhysicalExamTemplate.PhysExamTemplate.ProviderIdsColumn.ColumnName])},
                            { "Comments", MDVUtility.ToStr(dr[dsPhysicalExamTemplate.PhysExamTemplate.CommentsColumn.ColumnName])},
                            { "IsDefault", MDVUtility.ToStr(dr[dsPhysicalExamTemplate.PhysExamTemplate.IsDefaultColumn.ColumnName])},
                            { "IsActive", MDVUtility.ToStr(dr[dsPhysicalExamTemplate.PhysExamTemplate.IsActiveColumn.ColumnName])},
                        };
                            lstPhysExamTemplates.Add(physicalExamTemplateKeyValues);
                        }

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            PhysExamTemplateFill_JSON = js.Serialize(lstPhysExamTemplates),
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
                        Message = MDVCustomException.HumanReadableMessage(objTemplate.Message),
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
        /// Author: Ahmad Raza
        /// Date: 23-06-2016
        /// Function Name:  fillPatientPhysicalExamDataTemplate
        /// Description: This function will load the physical exam data templates
        /// </summary>
        /// <param name="dataTemplateId"></param>
        /// <param name="templateId"></param>
        /// <param name="entityId"></param>
        /// <param name="providerIds"></param>
        /// <param name="specialtyIds"></param>
        /// <returns></returns>
        public string fillPatientPhysicalExamDataTemplate(Int64 dataTemplateId, Int64 templateId, Int64 entityId, string providerIds = "", string specialtyIds = "")
        {
            try
            {
                DSPhysicalExamDataTemplate dsPhysicalExamDataTemplate = null;
                BLObject<DSPhysicalExamDataTemplate> objTemplate = BLLClinicalObj.loadPhysicalExamDataTemplate(dataTemplateId, templateId, entityId, providerIds, specialtyIds, 0, 0, 1);
                if (objTemplate.Data != null)
                {
                    dsPhysicalExamDataTemplate = objTemplate.Data;
                    if (dsPhysicalExamDataTemplate.Tables[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TableName].Rows.Count > 0)
                    {
                        List<Dictionary<string, string>> lstPhysExamDataTemplates = new List<Dictionary<string, string>>();
                        foreach (DataRow dr in dsPhysicalExamDataTemplate.Tables[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TableName].Rows)
                        {
                            //DataRow dr = dsPhysicalExamTemplate.Tables[dsPhysicalExamTemplate.PhysExamTemplate.TableName].Rows[0];
                            var physicalExamDataTemplateKeyValues = new Dictionary<string, string>
                        {
                            { "physExamDataTemplateId",  MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.DataTemplateIdColumn.ColumnName])},
                            { "physExamTemplateId",  MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TemplateIdColumn.ColumnName])},
                            { "physExamDataTemplateName",  MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.DataTemplateNameColumn.ColumnName])},
                            { "physExamTemplateName",  MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.TemplateNameColumn.ColumnName])},
                            { "SpecialtyIds", MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.SpecialtyIdsColumn.ColumnName])},
                            { "ProviderIds", MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.ProviderIdsColumn.ColumnName])},
                            { "Comments", MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.CommentsColumn.ColumnName])},
                            { "IsDefault", MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.IsDefaultColumn.ColumnName])},
                            { "IsActive", MDVUtility.ToStr(dr[dsPhysicalExamDataTemplate.PhysExamDataTemplate.IsActiveColumn.ColumnName])},
                        };
                            lstPhysExamDataTemplates.Add(physicalExamDataTemplateKeyValues);
                        }

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            PhysExamDataTemplateFill_JSON = js.Serialize(lstPhysExamDataTemplates),
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
                        Message = MDVCustomException.HumanReadableMessage(objTemplate.Message),
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
        // Author: Muhammad Arshad
        // Created Date: 26/02/2016
        //OverView: This function will handle insert/update of Patient Physical Exam Template

        /// <summary>
        /// Inserts the update patient physical exam template.
        /// </summary>
        /// <param name="PhysicalExamId">The physical exam identifier.</param>
        /// <param name="normalSystemIds">The normal system ids.</param>
        /// <param name="isToMarkNormal">if set to <c>true</c> [is to mark normal].</param>
        /// <param name="systemModel">The system model.</param>
        /// <returns>System.String.</returns>
        private string insertUpdatePatientPhysicalExamTemplate(long PhysicalExamId, List<int> normalSystemIds, bool isToMarkNormal = true, PatientPhysicalExamSystemModel systemModel = null)
        {
            DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
            string currentPatPhysSysId = string.Empty;
            BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystem(PhysicalExamId, 0);
            dsPhysicalExam = obj.Data;
            //Start//18-02-2016//Ahmad Raza//Condition to insert update Normal system's inner detail
            if (systemModel != null)
            {
                DSPhysicalExam.PatientPhysicalExamSystemRow RowNormalSystem = null;
                DSPhysicalExam.PatientPhysicalExamSystemRow[] arrNormalSystems = (DSPhysicalExam.PatientPhysicalExamSystemRow[])dsPhysicalExam.PatientPhysicalExamSystem.Select(dsPhysicalExam.PatientPhysicalExamSystem.SystemIdColumn.ColumnName + "=" + systemModel.PhysicalExamSystemId);

                if (arrNormalSystems.Length > 0)
                {
                    RowNormalSystem = arrNormalSystems[0];
                    currentPatPhysSysId = MDVUtility.ToStr(RowNormalSystem[dsPhysicalExam.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]);
                }
                else
                {
                    RowNormalSystem = dsPhysicalExam.PatientPhysicalExamSystem.NewPatientPhysicalExamSystemRow();
                }

                if (RowNormalSystem != null)
                {
                    RowNormalSystem.SystemId = MDVUtility.ToInt64(systemModel.PhysicalExamSystemId);
                    RowNormalSystem.PatientPhysicalExamId = PhysicalExamId;
                    RowNormalSystem.IsNormal = isToMarkNormal;
                    RowNormalSystem.IsActive = true;
                    RowNormalSystem.NormalComments = systemModel.NormalComments;
                    if (arrNormalSystems.Length == 0)
                    {
                        RowNormalSystem.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowNormalSystem.CreatedOn = DateTime.Now;
                    }
                    RowNormalSystem.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowNormalSystem.ModifiedOn = DateTime.Now;

                    if (isToMarkNormal == true)
                    {
                        RowNormalSystem[dsPhysicalExam.PatientPhysicalExamSystem.SoapTextColumn] = "This system is Normal.";
                    }
                    else
                    {
                        RowNormalSystem[dsPhysicalExam.PatientPhysicalExamSystem.SoapTextColumn] = DBNull.Value;
                    }

                    if (arrNormalSystems.Length < 1)
                    {
                        dsPhysicalExam.PatientPhysicalExamSystem.AddPatientPhysicalExamSystemRow(RowNormalSystem);
                    }
                }
                //End//18-02-2016//Ahmad Raza//Condition to insert update Normal system's inner detail
            }

            #region Database Insertion/Updation

            BLObject<DSPhysicalExam> objInsertedNormalSystem = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystem(dsPhysicalExam);
            if (objInsertedNormalSystem.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    physicalExamSystemId = currentPatPhysSysId != string.Empty ? currentPatPhysSysId : dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamSystem.TableName].Rows.Count > 0 ? dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamSystem.TableName].Rows[0][dsPhysicalExam.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName] : 0,
                };
                return response.physicalExamSystemId.ToString();//(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedNormalSystem.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }

        #endregion

        #region  Physical Exam Template Fill, Save and Update Methods

        // Author: Farooq Ahmad
        // Created Date: 07/03/2016
        //OverView: This function will handle insert/update of Physical Exam Template
        public string insertUpdatePhysicalExamTemplate(long TemplateId, PhysicalExamTemplateModel systemModel)
        {
            DSPhysicalExamTemplate dsPhysicalExam = new DSPhysicalExamTemplate();
            string currentPhysTempId = string.Empty;
            long oldTemplateId = TemplateId;
            List<string> lstSystemIdsToIgnore = new List<string>();
            List<string> lstSectionIdsToIgnore = new List<string>();
            List<string> lstCharIdsToIgnore = new List<string>();
            List<string> lstSubCharIdsToIgnore = new List<string>();
            if (!string.IsNullOrEmpty(systemModel.SaveAsTemplateName))
            {
                TemplateId = -1;
            }
            BLObject<DSPhysicalExamTemplate> obj = BLLClinicalObj.loadPhysicalExamTemplate(TemplateId, MDVUtility.ToLong(systemModel.PhysicalExamTemplateEntity));
            dsPhysicalExam = obj.Data;

            if (systemModel != null)
            {
                DSPhysicalExamTemplate.PhysExamTemplateRow RowNormalSystem = null;
                //DSPhysicalExamTemplate.PhysExamTemplateRow[] arrNormalSystems = (DSPhysicalExamTemplate.PhysExamTemplateRow[])dsPhysicalExam.PhysExamTemplate.Select(dsPhysicalExam.PhysExamTemplate.TemplateNameColumn.ColumnName + "='" + systemModel.PhysicalExamTemplateName+ "' and " + dsPhysicalExam.PhysExamTemplate.EntityIdColumn.ColumnName + "=" + systemModel.EntityId);
                DSPhysicalExamTemplate.PhysExamTemplateRow[] arrNormalSystems = (DSPhysicalExamTemplate.PhysExamTemplateRow[])dsPhysicalExam.PhysExamTemplate.Select(dsPhysicalExam.PhysExamTemplate.TemplateIdColumn.ColumnName + "=" + TemplateId);

                if (arrNormalSystems.Length > 0)
                {
                    RowNormalSystem = arrNormalSystems[0];
                    currentPhysTempId = MDVUtility.ToStr(RowNormalSystem[dsPhysicalExam.PhysExamTemplate.TemplateIdColumn.ColumnName]);
                }
                else
                {
                    RowNormalSystem = dsPhysicalExam.PhysExamTemplate.NewPhysExamTemplateRow();
                }

                if (RowNormalSystem != null)
                {
                    if (!string.IsNullOrEmpty(systemModel.SaveAsTemplateName))
                    {
                        RowNormalSystem.TemplateName = systemModel.SaveAsTemplateName;//Utility.ToInt64(systemModel.PhysicalExamSystemId);
                    }
                    else
                    {
                        RowNormalSystem.TemplateName = systemModel.PhysicalExamTemplateName;//Utility.ToInt64(systemModel.PhysicalExamSystemId);
                    }


                    if (systemModel.SpecialtyIds != string.Empty)
                        RowNormalSystem.SpecialtyIds = systemModel.SpecialtyIds;
                    else
                        RowNormalSystem[dsPhysicalExam.PhysExamTemplate.SpecialtyIdsColumn.ColumnName] = DBNull.Value;

                    if (systemModel.ProviderIds != string.Empty)
                        RowNormalSystem.ProviderIds = systemModel.ProviderIds;
                    else
                        RowNormalSystem[dsPhysicalExam.PhysExamTemplate.ProviderIdsColumn.ColumnName] = DBNull.Value;

                    RowNormalSystem.EntityId = MDVUtility.ToInt64(systemModel.PhysicalExamTemplateEntity);

                    RowNormalSystem.IsActive = systemModel.IsActive == "1" ? true : false;

                    if (arrNormalSystems.Length == 0)
                    {
                        RowNormalSystem.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowNormalSystem.CreatedOn = DateTime.Now;
                    }
                    RowNormalSystem.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowNormalSystem.ModifiedOn = DateTime.Now;



                    if (arrNormalSystems.Length < 1)
                    {
                        dsPhysicalExam.PhysExamTemplate.AddPhysExamTemplateRow(RowNormalSystem);
                    }
                }

            }

            #region Database Insertion/Updation

            BLObject<DSPhysicalExamTemplate> objInsertedNormalSystem = BLLClinicalObj.insertUpdatePhysicalExamTemplate(dsPhysicalExam);

            long templateId = 0;
            if (objInsertedNormalSystem.Data != null)
                templateId = MDVUtility.ToLong(objInsertedNormalSystem.Data.Tables[objInsertedNormalSystem.Data.PhysExamTemplate.TableName].Rows[objInsertedNormalSystem.Data.Tables[objInsertedNormalSystem.Data.PhysExamTemplate.TableName].Rows.Count - 1][objInsertedNormalSystem.Data.PhysExamTemplate.TemplateIdColumn.ColumnName]);

            if (objInsertedNormalSystem.Data != null && templateId != -1)
            {
                dsPhysicalExam = objInsertedNormalSystem.Data;
                string currentTemplateName = systemModel.PhysicalExamTemplateName;
                if (!string.IsNullOrEmpty(systemModel.SaveAsTemplateName))
                {
                    currentTemplateName = systemModel.SaveAsTemplateName;
                    // TemplateId = MDVUtility.ToLong(dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplate.TableName].Rows[dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplate.TableName].Rows.Count - 1][dsPhysicalExam.PhysExamTemplate.TemplateIdColumn.ColumnName]);
                    TemplateId = MDVUtility.ToLong(dsPhysicalExam.PhysExamTemplate.Rows[dsPhysicalExam.PhysExamTemplate.Rows.Count - 1][dsPhysicalExam.PhysExamTemplate.TemplateIdColumn.ColumnName]);
                }

                int insertedRowIndex = dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplate.TableName].Rows.Count - 1;
                DSPhysicalExamTemplate.PhysExamTemplateRow[] arrNormalSystems = (DSPhysicalExamTemplate.PhysExamTemplateRow[])dsPhysicalExam.PhysExamTemplate.Select(dsPhysicalExam.PhysExamTemplate.TemplateNameColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(currentTemplateName));
                if (arrNormalSystems.Length > 0)
                {
                    Int64 PhysTempId = currentPhysTempId != string.Empty ? MDVUtility.ToInt64(currentPhysTempId) : MDVUtility.ToInt64(arrNormalSystems[0][dsPhysicalExam.PhysExamTemplate.TemplateIdColumn.ColumnName]);
                    if (systemModel.SysSecCharSubcharData != null)
                    {
                        //loads all systems of currentTemplate
                        DSPhysicalExamTemplate dsPhysExamTemplateSystem = new DSPhysicalExamTemplate();
                        BLObject<DSPhysicalExamTemplate> objPhysExamTemplateSystem = BLLClinicalObj.LoadPhysicalExamTemplateSystem(PhysTempId);
                        if (objPhysExamTemplateSystem.Data != null)
                            dsPhysExamTemplateSystem = objPhysExamTemplateSystem.Data;
                        if (systemModel.SysSecCharSubcharData == null)
                            systemModel.SysSecCharSubcharData = new List<PhysExamSysTemplateModel>();
                        foreach (var Sys in systemModel.SysSecCharSubcharData)
                        {
                            if (Sys.IsModified != "1")
                            {
                                continue;
                            }

                            if (!string.IsNullOrEmpty(systemModel.SaveAsTemplateName) && Sys.IsChecked.ToLower() == "false")
                            {
                                lstSystemIdsToIgnore.Add(Sys.SystemId);
                                continue;
                            }
                            Sys.TemplateId = MDVUtility.ToStr(PhysTempId);
                            bool isNewSystem = false;
                            if (Sys.SystemId != "" && MDVUtility.ToInt64(Sys.SystemId) < 0)
                            {
                                Sys.SystemId = insertPhysicalExamSystem(Sys);
                                isNewSystem = true;
                            }

                            //Added by Abid Ali
                            if (Sys.IsChecked.ToLower() == "false" && PhysTempId > 0)
                            {
                                BLLClinicalObj.physicalExamTemplateSecCharSubCharIsCheckedUpdate(MDVUtility.ToInt64(Sys.SystemId), PhysTempId);
                                continue;
                            }

                            buildTemplateSystemRow(PhysTempId, Sys, dsPhysExamTemplateSystem, systemModel.SaveAsTemplateName, isNewSystem);

                            //loads all sections of system
                            DSPhysicalExamTemplate dsPhysExamTemplateSection = new DSPhysicalExamTemplate();
                            if (Sys.Sections != null)
                            {
                                BLObject<DSPhysicalExamTemplate> objPhysExamTemplateSection = BLLClinicalObj.LoadPhysicalExamTemplateSystemSection(PhysTempId, MDVUtility.ToInt64(Sys.SystemId));
                                if (objPhysExamTemplateSection.Data != null)
                                    dsPhysExamTemplateSection = objPhysExamTemplateSection.Data;
                            }

                            if (Sys.Sections == null)
                                Sys.Sections = new List<PhysExamSecTemplateModel>();
                            foreach (var Sec in Sys.Sections)
                            {
                                if (Sec.IsModified != "1")
                                {
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(systemModel.SaveAsTemplateName) && Sec.IsChecked.ToLower() == "false")
                                {
                                    lstSectionIdsToIgnore.Add(Sec.SectionId);
                                    continue;
                                }
                                Sec.TemplateId = MDVUtility.ToStr(PhysTempId);
                                Sec.SystemId = Sys.SystemId;

                                bool isNewSection = false;
                                if (Sec.SectionId != "" && MDVUtility.ToInt64(Sec.SectionId) < 0)
                                {
                                    Sec.SystemId = Sys.SystemId;
                                    Sec.SectionId = insertPhysicalExamSystemSection(Sec);
                                    isNewSection = true;
                                }


                                //Added by Abid Ali
                                if (Sec.IsChecked.ToLower() == "false" && PhysTempId > 0)
                                {
                                    BLLClinicalObj.physicalExamTemplateCharSubCharIsCheckedUpdate(MDVUtility.ToInt64(Sec.SectionId), PhysTempId);
                                    continue;
                                }


                                buildTemplateSectionRow(PhysTempId, Sec, dsPhysExamTemplateSection, systemModel.SaveAsTemplateName, isNewSection);

                                //loads all Chars of system
                                DSPhysicalExamTemplate dsPhysExamTemplateChar = new DSPhysicalExamTemplate();
                                if (Sec.Characteristics != null)
                                {
                                    BLObject<DSPhysicalExamTemplate> objPhysExamTemplateChar = BLLClinicalObj.LoadPhysicalExamTemplateSystemChar(PhysTempId, MDVUtility.ToInt64(Sec.SectionId));
                                    if (objPhysExamTemplateChar.Data != null)
                                        dsPhysExamTemplateChar = objPhysExamTemplateChar.Data;
                                }

                                if (Sec.Characteristics == null)
                                    Sec.Characteristics = new List<PhysExamCharTemplateModel>();
                                foreach (var Char in Sec.Characteristics)
                                {

                                    if (Char.IsModified != "1")
                                    {
                                        continue;
                                    }
                                    if (!string.IsNullOrEmpty(systemModel.SaveAsTemplateName) && Char.IsChecked.ToLower() == "false")
                                    {
                                        lstCharIdsToIgnore.Add(Char.CharacteristicId);
                                        continue;
                                    }
                                    Char.TemplateId = MDVUtility.ToStr(PhysTempId);

                                    bool isNewChar = false;
                                    if (Char.CharacteristicId != "" && MDVUtility.ToInt64(Char.CharacteristicId) < 0)
                                    {
                                        Char.SectionId = Sec.SectionId;
                                        Char.CharacteristicId = insertPhysicalExamSystemSectionCharacteristics(Char);
                                        isNewChar = true;
                                    }

                                    //Added by Abid Ali
                                    if (Char.IsChecked.ToLower() == "false" && PhysTempId > 0)
                                    {
                                        BLLClinicalObj.physicalExamTemplateSubCharIsCheckedUpdate(MDVUtility.ToInt64(Char.CharacteristicId), PhysTempId);
                                        continue;
                                    }

                                    buildTemplateCharRow(PhysTempId, Char, dsPhysExamTemplateChar, systemModel.SaveAsTemplateName, isNewChar);

                                    //loads all SubChars of system
                                    DSPhysicalExamTemplate dsPhysExamTemplateSubChar = new DSPhysicalExamTemplate();
                                    if (Char.SubCharacteristics != null)
                                    {
                                        BLObject<DSPhysicalExamTemplate> objPhysExamTemplateSubChar = BLLClinicalObj.loadPhysicalExamTemplateSystemSubChar(PhysTempId, MDVUtility.ToInt64(Char.CharacteristicId));
                                        if (objPhysExamTemplateSubChar.Data != null)
                                            dsPhysExamTemplateSubChar = objPhysExamTemplateSubChar.Data;
                                    }

                                    if (Char.SubCharacteristics == null)
                                        Char.SubCharacteristics = new List<PhysExamSubCharTemplateModel>();
                                    foreach (var Subchar in Char.SubCharacteristics)
                                    {
                                        if (Subchar.IsModified != "1")
                                        {
                                            continue;
                                        }
                                        if (!string.IsNullOrEmpty(systemModel.SaveAsTemplateName) && Subchar.IsChecked.ToLower() == "false")
                                        {
                                            lstSubCharIdsToIgnore.Add(Subchar.SubCharacteristicId);
                                            continue;
                                        }

                                        bool isNewSubChar = false;
                                        Subchar.TemplateId = MDVUtility.ToStr(PhysTempId);
                                        if (Subchar.SubCharacteristicId != "" && MDVUtility.ToInt64(Subchar.SubCharacteristicId) < 0)
                                        {
                                            Subchar.CharacteristicId = Char.CharacteristicId;
                                            Subchar.SubCharacteristicId = insertPhysicalExamSystemSectionCharacteristicsSubCharacteristic(Subchar);
                                            isNewSubChar = true;
                                        }
                                        buildTemplateSubCharRow(PhysTempId, Subchar, dsPhysExamTemplateSubChar, systemModel.SaveAsTemplateName, isNewSubChar);
                                    }

                                    if (dsPhysExamTemplateSubChar.PhysExamTemplateSubChar.Rows.Count > 0)
                                    {
                                        insertUpdatePhysicalExamTemplateSubChar(dsPhysExamTemplateSubChar);
                                    }

                                    if (!string.IsNullOrEmpty(systemModel.SaveAsTemplateName))
                                    {
                                        //BLLClinicalObj.physicalExamSaveAsTemplate(oldTemplateId, TemplateId, subCharIds: String.Join(",", lstSubCharIdsToIgnore));
                                    }

                                }

                                if (dsPhysExamTemplateChar.PhysExamTemplateChar.Rows.Count > 0)
                                {
                                    insertUpdatePhysicalExamTemplateChar(dsPhysExamTemplateChar);
                                }

                                if (!string.IsNullOrEmpty(systemModel.SaveAsTemplateName))
                                {
                                    //call for saveAs SP for oldTemplateId and current TemplateId

                                    //BLLClinicalObj.physicalExamSaveAsTemplate(oldTemplateId, TemplateId, charIds: String.Join(",", lstCharIdsToIgnore));
                                }

                            }

                            if (dsPhysExamTemplateSection.PhysExamTemplateSection.Rows.Count > 0)
                            {
                                var responsePhysExamTemplateSection = insertUpdatePhysicalExamTemplateSection(dsPhysExamTemplateSection);
                            }

                            if (!string.IsNullOrEmpty(systemModel.SaveAsTemplateName))
                            {
                                //call for saveAs SP for oldTemplateId and current TemplateId

                                //BLLClinicalObj.physicalExamSaveAsTemplate(oldTemplateId, TemplateId, sectionIds: String.Join(",", lstSectionIdsToIgnore));
                            }
                        }

                        if (dsPhysExamTemplateSystem.PhysExamTemplateSys.Rows.Count > 0)
                        {
                            var responsePhysExamTemplateSystem = insertUpdatePhysicalExamTemplateSystem(dsPhysExamTemplateSystem);
                        }

                        if (!string.IsNullOrEmpty(systemModel.SaveAsTemplateName))
                        {
                            //call for saveAs SP for oldTemplateId and current TemplateId

                            BLLClinicalObj.physicalExamSaveAsTemplate(oldTemplateId, TemplateId, String.Join(",", lstSystemIdsToIgnore), String.Join(",", lstSectionIdsToIgnore), String.Join(",", lstCharIdsToIgnore), String.Join(",", lstSubCharIdsToIgnore));
                        }

                    }

                    ////For Save As Template //Load and save with -1 id
                    //if (TemplateId > 0 && (!string.IsNullOrEmpty(systemModel.SaveAsTemplateName)))
                    //{
                    //    string isSaved = saveAsPhysicalExamTemplate(PhysTempId, MDVUtility.ToInt64(systemModel.EntityId), systemModel.SaveAsTemplateName);
                    //}

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        phyExamTemplateId = PhysTempId,
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Physical Exam Template could not be inserted."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = (objInsertedNormalSystem.Message.Contains("Cannot insert duplicate key") || templateId == -1) ? "A Template with this name already exists. Try a different name." : objInsertedNormalSystem.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }



        public string saveAsPhysicalExamTemplate(long templateId, long entityId, string newTemplateName)
        {
            DSPhysicalExamTemplate dsPhysicalExam = new DSPhysicalExamTemplate();
            string currentPhysTempId = string.Empty;
            BLObject<DSPhysicalExamTemplate> obj = null;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            if (templateId > 0)
            {
                obj = BLLClinicalObj.loadPhysicalExamTemplate(templateId, entityId, null, "", "", "1", "");
                dsPhysicalExam = obj.Data;
            }
            else
            {
                obj = BLLClinicalObj.loadPhysicalExamTemplate(templateId, entityId);
                dsPhysicalExam = obj.Data;
            }

            if (dsPhysicalExam != null && dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplate.TableName].Rows.Count > 0)
            {
                PhysicalExamTemplateModel PhysicalExamTemplate = new PhysicalExamTemplateModel();
                DataRow dr = dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplate.TableName].Rows[0];

                PhysicalExamTemplate.TemplateId = "-1";
                PhysicalExamTemplate.PhysicalExamTemplateName = newTemplateName;

                PhysicalExamTemplate.SpecialtyIds = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.SpecialtyIdsColumn.ColumnName]);
                PhysicalExamTemplate.ProviderIds = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.ProviderIdsColumn.ColumnName]);
                PhysicalExamTemplate.PhysicalExamTemplateEntity = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.EntityIdColumn.ColumnName]);
                PhysicalExamTemplate.IsActive = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.IsActiveColumn.ColumnName]);
                PhysicalExamTemplate.IsDefault = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.IsDefaultColumn.ColumnName]);

                //PhysicalExamTemplate.LastUpdated = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.ModifiedOnColumn.ColumnName])) ? "" : MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsPhysicalExam.PhysExamTemplate.ModifiedOnColumn.ColumnName]));
                //PhysicalExamTemplate.ModifiedBy = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.ModifiedByColumn.ColumnName]);

                if (templateId > 0)
                {
                    BLObject<DSPhysicalExamTemplate> objSys = BLLClinicalObj.LoadPhysicalExamTemplateSystem(templateId);
                    List<PhysExamSysTemplateModel> lstSysSecCharSubcharData = new List<PhysExamSysTemplateModel>();
                    DSPhysicalExamTemplate dsPhysicalExamSys = new DSPhysicalExamTemplate();
                    dsPhysicalExamSys = objSys.Data;
                    if (dsPhysicalExamSys != null && dsPhysicalExamSys.Tables[dsPhysicalExamSys.PhysExamTemplateSys.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drSys in dsPhysicalExamSys.Tables[dsPhysicalExamSys.PhysExamTemplateSys.TableName].Rows)
                        {
                            PhysExamSysTemplateModel SysSecCharSubcharData = new PhysExamSysTemplateModel();
                            SysSecCharSubcharData.IsChecked = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.IsCheckedColumn.ColumnName]);
                            SysSecCharSubcharData.SystemId = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.SystemIdColumn.ColumnName]);
                            SysSecCharSubcharData.SystemName = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.SystemNameColumn.ColumnName]);
                            // SysSecCharSubcharData.TemplateId = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.TemplateIdColumn.ColumnName]);
                            //  SysSecCharSubcharData.TemplateSysId = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.TemplateSysIdColumn.ColumnName]);
                            SysSecCharSubcharData.IsModified = "1";
                            BLObject<DSPhysicalExamTemplate> objSection = BLLClinicalObj.LoadPhysicalExamTemplateSystemSection(templateId, MDVUtility.ToInt64(SysSecCharSubcharData.SystemId));
                            List<PhysExamSecTemplateModel> lstPhysExamSecTemplateModel = new List<PhysExamSecTemplateModel>();
                            DSPhysicalExamTemplate dsPhysicalExamSec = new DSPhysicalExamTemplate();
                            dsPhysicalExamSec = objSection.Data;

                            if (dsPhysicalExamSec != null && dsPhysicalExamSec.Tables[dsPhysicalExam.PhysExamTemplateSection.TableName].Rows.Count > 0)
                            {

                                foreach (DataRow drSec in dsPhysicalExamSec.Tables[dsPhysicalExamSec.PhysExamTemplateSection.TableName].Rows)
                                {
                                    PhysExamSecTemplateModel objPhysExamSecTemplateModel = new PhysExamSecTemplateModel();
                                    objPhysExamSecTemplateModel.IsChecked = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.IsCheckedColumn.ColumnName]);
                                    objPhysExamSecTemplateModel.SystemId = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.SystemIdColumn.ColumnName]);
                                    objPhysExamSecTemplateModel.SectionId = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.SectionIdColumn.ColumnName]);
                                    objPhysExamSecTemplateModel.SectionName = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.SectionNameColumn.ColumnName]);
                                    //  objPhysExamSecTemplateModel.TemplateId = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.TemplateIdColumn.ColumnName]);
                                    //  objPhysExamSecTemplateModel.TemplateSectionId = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.TemplateSectionIdColumn.ColumnName]);
                                    objPhysExamSecTemplateModel.IsModified = "1";
                                    BLObject<DSPhysicalExamTemplate> objChar = BLLClinicalObj.LoadPhysicalExamTemplateSystemChar(templateId, MDVUtility.ToInt64(objPhysExamSecTemplateModel.SectionId));
                                    List<PhysExamCharTemplateModel> lstPhysExamCharTemplateModel = new List<PhysExamCharTemplateModel>();
                                    DSPhysicalExamTemplate dsPhysicalExamChar = new DSPhysicalExamTemplate();
                                    dsPhysicalExamChar = objChar.Data;

                                    if (dsPhysicalExamChar != null && dsPhysicalExamChar.Tables[dsPhysicalExamChar.PhysExamTemplateChar.TableName].Rows.Count > 0)
                                    {

                                        foreach (DataRow drChar in dsPhysicalExamChar.Tables[dsPhysicalExamChar.PhysExamTemplateChar.TableName].Rows)
                                        {
                                            PhysExamCharTemplateModel objPhysExamCharTemplateModel = new PhysExamCharTemplateModel();
                                            objPhysExamCharTemplateModel.IsChecked = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.IsCheckedColumn.ColumnName]);
                                            objPhysExamCharTemplateModel.CharacteristicId = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.CharIdColumn.ColumnName]);
                                            objPhysExamCharTemplateModel.CharName = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.CharNameColumn.ColumnName]);
                                            objPhysExamCharTemplateModel.SectionId = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.SectionIdColumn.ColumnName]);
                                            //  objPhysExamCharTemplateModel.TemplateId = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.TemplateIdColumn.ColumnName]);
                                            //  objPhysExamCharTemplateModel.TemplateCharId = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.TemplateCharIdColumn.ColumnName]);
                                            objPhysExamCharTemplateModel.IsModified = "1";
                                            BLObject<DSPhysicalExamTemplate> objSubChar = BLLClinicalObj.loadPhysicalExamTemplateSystemSubChar(templateId, MDVUtility.ToInt64(objPhysExamCharTemplateModel.CharacteristicId));
                                            List<PhysExamSubCharTemplateModel> lstPhysExamSubCharTemplateModel = new List<PhysExamSubCharTemplateModel>();
                                            DSPhysicalExamTemplate dsPhysicalExamSubChar = new DSPhysicalExamTemplate();
                                            dsPhysicalExamSubChar = objSubChar.Data;

                                            if (dsPhysicalExamSubChar != null && dsPhysicalExamSubChar.Tables[dsPhysicalExamSubChar.PhysExamTemplateSubChar.TableName].Rows.Count > 0)
                                            {

                                                foreach (DataRow drSubChar in dsPhysicalExamSubChar.Tables[dsPhysicalExamSubChar.PhysExamTemplateSubChar.TableName].Rows)
                                                {
                                                    PhysExamSubCharTemplateModel objPhysExamSubCharTemplateModel = new PhysExamSubCharTemplateModel();
                                                    objPhysExamSubCharTemplateModel.IsChecked = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.IsCheckedColumn.ColumnName]);
                                                    objPhysExamSubCharTemplateModel.CharacteristicId = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.CharIdColumn.ColumnName]);
                                                    objPhysExamSubCharTemplateModel.SubCharacteristicId = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.SubCharIdColumn.ColumnName]);
                                                    objPhysExamSubCharTemplateModel.SubCharName = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.SubCharNameColumn.ColumnName]);
                                                    //  objPhysExamSubCharTemplateModel.TemplateId = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.TemplateIdColumn.ColumnName]);
                                                    //  objPhysExamSubCharTemplateModel.TemplateSubCharId = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.TemplateSubCharIdColumn.ColumnName]);
                                                    objPhysExamSubCharTemplateModel.IsModified = "1";

                                                    lstPhysExamSubCharTemplateModel.Add(objPhysExamSubCharTemplateModel);
                                                }
                                                objPhysExamCharTemplateModel.SubCharacteristics = lstPhysExamSubCharTemplateModel;
                                            }

                                            lstPhysExamCharTemplateModel.Add(objPhysExamCharTemplateModel);
                                        }
                                        objPhysExamSecTemplateModel.Characteristics = lstPhysExamCharTemplateModel;
                                    }

                                    lstPhysExamSecTemplateModel.Add(objPhysExamSecTemplateModel);
                                }

                                SysSecCharSubcharData.Sections = lstPhysExamSecTemplateModel;

                            }
                            lstSysSecCharSubcharData.Add(SysSecCharSubcharData);
                        }

                        PhysicalExamTemplate.SysSecCharSubcharData = lstSysSecCharSubcharData;
                    }
                }
                insertUpdatePhysicalExamTemplate(-1, PhysicalExamTemplate);
            }
            return "";
        }

        // Author: Farooq Ahmad
        // Created Date: 08/03/2016
        //OverView: This function will load Physical Exam Template
        public string loadPhysicalExamTemplate(long templateId, long entityId)
        {
            DSPhysicalExamTemplate dsPhysicalExam = new DSPhysicalExamTemplate();
            string currentPhysTempId = string.Empty;
            BLObject<DSPhysicalExamTemplate> obj = null;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            if (templateId > 0)
            {
                obj = BLLClinicalObj.loadPhysicalExamTemplate(templateId, entityId, null, "", "", "1", "");
                dsPhysicalExam = obj.Data;
            }
            else
            {
                obj = BLLClinicalObj.loadPhysicalExamTemplate(templateId, entityId);
                dsPhysicalExam = obj.Data;
            }

            if (dsPhysicalExam != null && dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplate.TableName].Rows.Count > 0)
            {
                List<PhysicalExamTemplateModel> lstPhysicalExamTemplate = new List<PhysicalExamTemplateModel>();
                foreach (DataRow dr in dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplate.TableName].Rows)
                {
                    PhysicalExamTemplateModel PhysicalExamTemplate = new PhysicalExamTemplateModel();
                    PhysicalExamTemplate.TemplateId = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.TemplateIdColumn.ColumnName]);
                    PhysicalExamTemplate.PhysicalExamTemplateName = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.TemplateNameColumn.ColumnName]);
                    PhysicalExamTemplate.SpecialtyIds = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.SpecialtyIdsColumn.ColumnName]);
                    PhysicalExamTemplate.ProviderIds = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.ProviderIdsColumn.ColumnName]);
                    PhysicalExamTemplate.PhysicalExamTemplateEntity = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.EntityIdColumn.ColumnName]);
                    PhysicalExamTemplate.IsActive = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.IsActiveColumn.ColumnName]);
                    PhysicalExamTemplate.IsDefault = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.IsDefaultColumn.ColumnName]);

                    PhysicalExamTemplate.LastUpdated = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.ModifiedOnColumn.ColumnName])) ? "" : MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsPhysicalExam.PhysExamTemplate.ModifiedOnColumn.ColumnName]));
                    PhysicalExamTemplate.ModifiedBy = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.ModifiedByColumn.ColumnName]);
                    if (templateId > 0)
                    {
                        BLObject<DSPhysicalExamTemplate> objSys = BLLClinicalObj.LoadPhysicalExamTemplateSystem(templateId);
                        List<PhysExamSysTemplateModel> lstSysSecCharSubcharData = new List<PhysExamSysTemplateModel>();
                        DSPhysicalExamTemplate dsPhysicalExamSys = new DSPhysicalExamTemplate();
                        dsPhysicalExamSys = objSys.Data;
                        if (dsPhysicalExamSys != null && dsPhysicalExamSys.Tables[dsPhysicalExamSys.PhysExamTemplateSys.TableName].Rows.Count > 0)
                        {
                            foreach (DataRow drSys in dsPhysicalExamSys.Tables[dsPhysicalExamSys.PhysExamTemplateSys.TableName].Rows)
                            {
                                PhysExamSysTemplateModel SysSecCharSubcharData = new PhysExamSysTemplateModel();
                                SysSecCharSubcharData.IsChecked = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.IsCheckedColumn.ColumnName]);
                                SysSecCharSubcharData.SystemId = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.SystemIdColumn.ColumnName]);
                                SysSecCharSubcharData.SystemName = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.SystemNameColumn.ColumnName]);
                                SysSecCharSubcharData.TemplateId = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.TemplateIdColumn.ColumnName]);
                                SysSecCharSubcharData.TemplateSysId = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.TemplateSysIdColumn.ColumnName]);

                                BLObject<DSPhysicalExamTemplate> objSection = BLLClinicalObj.LoadPhysicalExamTemplateSystemSection(templateId, MDVUtility.ToInt64(SysSecCharSubcharData.SystemId));
                                List<PhysExamSecTemplateModel> lstPhysExamSecTemplateModel = new List<PhysExamSecTemplateModel>();
                                DSPhysicalExamTemplate dsPhysicalExamSec = new DSPhysicalExamTemplate();
                                dsPhysicalExamSec = objSection.Data;

                                if (dsPhysicalExamSec != null && dsPhysicalExamSec.Tables[dsPhysicalExam.PhysExamTemplateSection.TableName].Rows.Count > 0)
                                {

                                    foreach (DataRow drSec in dsPhysicalExamSec.Tables[dsPhysicalExamSec.PhysExamTemplateSection.TableName].Rows)
                                    {
                                        PhysExamSecTemplateModel objPhysExamSecTemplateModel = new PhysExamSecTemplateModel();
                                        objPhysExamSecTemplateModel.IsChecked = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.IsCheckedColumn.ColumnName]);
                                        objPhysExamSecTemplateModel.SystemId = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.SystemIdColumn.ColumnName]);
                                        objPhysExamSecTemplateModel.SectionId = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.SectionIdColumn.ColumnName]);
                                        objPhysExamSecTemplateModel.SectionName = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.SectionNameColumn.ColumnName]);
                                        objPhysExamSecTemplateModel.TemplateId = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.TemplateIdColumn.ColumnName]);
                                        objPhysExamSecTemplateModel.TemplateSectionId = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.TemplateSectionIdColumn.ColumnName]);

                                        BLObject<DSPhysicalExamTemplate> objChar = BLLClinicalObj.LoadPhysicalExamTemplateSystemChar(templateId, MDVUtility.ToInt64(objPhysExamSecTemplateModel.SectionId));
                                        List<PhysExamCharTemplateModel> lstPhysExamCharTemplateModel = new List<PhysExamCharTemplateModel>();
                                        DSPhysicalExamTemplate dsPhysicalExamChar = new DSPhysicalExamTemplate();
                                        dsPhysicalExamChar = objChar.Data;

                                        if (dsPhysicalExamChar != null && dsPhysicalExamChar.Tables[dsPhysicalExamChar.PhysExamTemplateChar.TableName].Rows.Count > 0)
                                        {

                                            foreach (DataRow drChar in dsPhysicalExamChar.Tables[dsPhysicalExamChar.PhysExamTemplateChar.TableName].Rows)
                                            {
                                                PhysExamCharTemplateModel objPhysExamCharTemplateModel = new PhysExamCharTemplateModel();
                                                objPhysExamCharTemplateModel.IsChecked = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.IsCheckedColumn.ColumnName]);
                                                objPhysExamCharTemplateModel.CharacteristicId = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.CharIdColumn.ColumnName]);
                                                objPhysExamCharTemplateModel.CharName = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.CharNameColumn.ColumnName]);
                                                objPhysExamCharTemplateModel.SectionId = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.SectionIdColumn.ColumnName]);
                                                objPhysExamCharTemplateModel.TemplateId = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.TemplateIdColumn.ColumnName]);
                                                objPhysExamCharTemplateModel.TemplateCharId = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.TemplateCharIdColumn.ColumnName]);

                                                BLObject<DSPhysicalExamTemplate> objSubChar = BLLClinicalObj.loadPhysicalExamTemplateSystemSubChar(templateId, MDVUtility.ToInt64(objPhysExamCharTemplateModel.CharacteristicId));
                                                List<PhysExamSubCharTemplateModel> lstPhysExamSubCharTemplateModel = new List<PhysExamSubCharTemplateModel>();
                                                DSPhysicalExamTemplate dsPhysicalExamSubChar = new DSPhysicalExamTemplate();
                                                dsPhysicalExamSubChar = objSubChar.Data;

                                                if (dsPhysicalExamSubChar != null && dsPhysicalExamSubChar.Tables[dsPhysicalExamSubChar.PhysExamTemplateSubChar.TableName].Rows.Count > 0)
                                                {

                                                    foreach (DataRow drSubChar in dsPhysicalExamSubChar.Tables[dsPhysicalExamSubChar.PhysExamTemplateSubChar.TableName].Rows)
                                                    {
                                                        PhysExamSubCharTemplateModel objPhysExamSubCharTemplateModel = new PhysExamSubCharTemplateModel();
                                                        objPhysExamSubCharTemplateModel.IsChecked = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.IsCheckedColumn.ColumnName]);
                                                        objPhysExamSubCharTemplateModel.CharacteristicId = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.CharIdColumn.ColumnName]);
                                                        objPhysExamSubCharTemplateModel.SubCharacteristicId = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.SubCharIdColumn.ColumnName]);
                                                        objPhysExamSubCharTemplateModel.SubCharName = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.SubCharNameColumn.ColumnName]);
                                                        objPhysExamSubCharTemplateModel.TemplateId = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.TemplateIdColumn.ColumnName]);
                                                        objPhysExamSubCharTemplateModel.TemplateSubCharId = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.TemplateSubCharIdColumn.ColumnName]);
                                                        lstPhysExamSubCharTemplateModel.Add(objPhysExamSubCharTemplateModel);
                                                    }
                                                    objPhysExamCharTemplateModel.SubCharacteristics = lstPhysExamSubCharTemplateModel;
                                                }

                                                lstPhysExamCharTemplateModel.Add(objPhysExamCharTemplateModel);
                                            }
                                            objPhysExamSecTemplateModel.Characteristics = lstPhysExamCharTemplateModel;
                                        }

                                        lstPhysExamSecTemplateModel.Add(objPhysExamSecTemplateModel);
                                    }

                                    SysSecCharSubcharData.Sections = lstPhysExamSecTemplateModel;

                                }
                                lstSysSecCharSubcharData.Add(SysSecCharSubcharData);
                            }

                            PhysicalExamTemplate.SysSecCharSubcharData = lstSysSecCharSubcharData;


                        }
                    }


                    lstPhysicalExamTemplate.Add(PhysicalExamTemplate);
                }

                var response = new
                {
                    status = true,
                    PhysicalExamTemplate = js.Serialize(lstPhysicalExamTemplate),
                    PhysicalExamTemplateCount = lstPhysicalExamTemplate.Count
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            var response2 = new
            {
                status = false,
                PhysicalExamTemplate = js.Serialize(null),
                PhysicalExamTemplateCount = 0
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response2));
        }



        public string loadPhysicalExamTemplateNew(long templateId, long entityId, int? IsActive = 1)
        {
            DSPhysicalExamTemplate dsPhysicalExam = new DSPhysicalExamTemplate();
            string currentPhysTempId = string.Empty;
            BLObject<DSPhysicalExamTemplate> obj = null;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            if (templateId > 0)
            {
                obj = BLLClinicalObj.loadPhysicalExamTemplate(templateId, entityId, null, "", "", "1", "");
                dsPhysicalExam = obj.Data;
            }
            else
            {
                obj = BLLClinicalObj.loadPhysicalExamTemplate(templateId, entityId, IsActive);
                dsPhysicalExam = obj.Data;
            }

            if (dsPhysicalExam != null && dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplate.TableName].Rows.Count > 0)
            {
                List<PhysicalExamTemplateModel> lstPhysicalExamTemplate = new List<PhysicalExamTemplateModel>();
                foreach (DataRow dr in dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplate.TableName].Rows)
                {
                    PhysicalExamTemplateModel PhysicalExamTemplate = new PhysicalExamTemplateModel();
                    PhysicalExamTemplate.TemplateId = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.TemplateIdColumn.ColumnName]);
                    PhysicalExamTemplate.PhysicalExamTemplateName = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.TemplateNameColumn.ColumnName]);
                    PhysicalExamTemplate.SpecialtyIds = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.SpecialtyIdsColumn.ColumnName]);
                    PhysicalExamTemplate.ProviderIds = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.ProviderIdsColumn.ColumnName]);
                    PhysicalExamTemplate.PhysicalExamTemplateEntity = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.EntityIdColumn.ColumnName]);
                    PhysicalExamTemplate.IsActive = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.IsActiveColumn.ColumnName]);
                    PhysicalExamTemplate.IsDefault = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.IsDefaultColumn.ColumnName]);
                    //PhysicalExamTemplate.LastUpdated = MDVUtility.DateToString(Convert.ToDateTime(dr[dsPhysicalExam.PhysExamTemplate.ModifiedOnColumn.ColumnName]));

                    PhysicalExamTemplate.LastUpdated = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.ModifiedOnColumn.ColumnName])) ? "" : MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsPhysicalExam.PhysExamTemplate.ModifiedOnColumn.ColumnName]));
                    PhysicalExamTemplate.ModifiedBy = MDVUtility.ToStr(dr[dsPhysicalExam.PhysExamTemplate.ModifiedByColumn.ColumnName]);
                    if (templateId > 0)
                    {
                        BLObject<DSPhysicalExamTemplate> objSys = BLLClinicalObj.LoadPhysicalExamTemplateSystem(templateId);
                        List<PhysExamSysTemplateModel> lstSysSecCharSubcharData = new List<PhysExamSysTemplateModel>();
                        DSPhysicalExamTemplate dsPhysicalExamSys = new DSPhysicalExamTemplate();
                        dsPhysicalExamSys = objSys.Data;
                        if (dsPhysicalExamSys != null && dsPhysicalExamSys.Tables[dsPhysicalExamSys.PhysExamTemplateSys.TableName].Rows.Count > 0)
                        {
                            foreach (DataRow drSys in dsPhysicalExamSys.Tables[dsPhysicalExamSys.PhysExamTemplateSys.TableName].Rows)
                            {
                                PhysExamSysTemplateModel SysSecCharSubcharData = new PhysExamSysTemplateModel();
                                SysSecCharSubcharData.IsChecked = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.IsCheckedColumn.ColumnName]);
                                SysSecCharSubcharData.SystemId = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.SystemIdColumn.ColumnName]);
                                SysSecCharSubcharData.SystemName = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.SystemNameColumn.ColumnName]);
                                SysSecCharSubcharData.TemplateId = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.TemplateIdColumn.ColumnName]);
                                SysSecCharSubcharData.TemplateSysId = MDVUtility.ToStr(drSys[dsPhysicalExamSys.PhysExamTemplateSys.TemplateSysIdColumn.ColumnName]);


                                lstSysSecCharSubcharData.Add(SysSecCharSubcharData);
                            }

                            PhysicalExamTemplate.SysSecCharSubcharData = lstSysSecCharSubcharData;


                        }
                    }


                    lstPhysicalExamTemplate.Add(PhysicalExamTemplate);
                }

                var response = new
                {
                    status = true,
                    PhysicalExamTemplate = js.Serialize(lstPhysicalExamTemplate),
                    PhysicalExamTemplateCount = lstPhysicalExamTemplate.Count
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            var response2 = new
            {
                status = true,
                PhysicalExamTemplate = js.Serialize(null),
                PhysicalExamTemplateCount = 0
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response2));
        }

        // MKMKMK

        public string loadPhysicalExamTemplatesECW(long templateId, long entityId, int? IsActive = 1, int? isSelected = 1)
        {
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
            string currentPhysTempId = string.Empty;
            BLObject<DSPhysicalExamECW> obj = null;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            if (templateId > 0)
            {
                obj = BLLPhysicalExamECWObj.loadPhysicalExamTemplatesECW(templateId, entityId, IsActive, isSelected);
            }
            else
            {
                obj = BLLPhysicalExamECWObj.loadPhysicalExamTemplatesECW(templateId, entityId, IsActive, isSelected);
            }
            if (obj.Data != null)
            {
                dsPhysicalExam = obj.Data;
                var response = new
                {
                    status = true,
                    PETemplateSystems_JSON = js.Serialize(MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PETemplateSystem.TableName])),
                    PETemplateSystemsCount = dsPhysicalExam.Tables[dsPhysicalExam.PETemplateSystem.TableName].Rows.Count,
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
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string loadPhysicalExamSystemsECW(int? IsActive = 1)
        {
            DSPhysicalExamECWLookup dsPhysicalExam = new DSPhysicalExamECWLookup();
            string currentPhysTempId = string.Empty;
            BLObject<DSPhysicalExamECWLookup> obj = null;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            obj = BLLPhysicalExamECWObj.loadPhysicalExamSystemsECW(IsActive);
            dsPhysicalExam = obj.Data;

            var response = new
            {
                status = true,
                PESystems_JSON = js.Serialize(MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PESystemLookup.TableName])),
                PESystemsCount = dsPhysicalExam.Tables[dsPhysicalExam.PESystemLookup.TableName].Rows.Count,
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }

        public string loadPhysicalExamSystemObservatiosECW(long templateId, long systemId)
        {
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
            string currentPhysTempId = string.Empty;
            BLObject<DSPhysicalExamECW> obj = null;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            obj = BLLPhysicalExamECWObj.loadPETempSystemObservatiosECW(templateId, systemId);
            dsPhysicalExam = obj.Data;

            var response = new
            {
                status = true,
                PEObservation_JSON = js.Serialize(MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PESystemObservation.TableName])),
                PEObservationCount = dsPhysicalExam.Tables[dsPhysicalExam.PESystemObservation.TableName].Rows.Count,
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }

        public string loadPETempSystemObservationNote(long templateId, long systemId, long NotesId)
        {
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
            string currentPhysTempId = string.Empty;
            BLObject<DSPhysicalExamECW> obj = null;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            obj = BLLPhysicalExamECWObj.loadPETempSystemObservationNote(templateId, systemId, NotesId);
            dsPhysicalExam = obj.Data;

            var response = new
            {
                status = true,
                PEObservation_JSON = js.Serialize(MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PESystemObservation.TableName])),
                PENotesObservation_JSON = js.Serialize(MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PENotesObservation.TableName])),
                PEObservationCount = dsPhysicalExam.Tables[dsPhysicalExam.PESystemObservation.TableName].Rows.Count,
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }

        //public string GetSpecialtyProvider(long SpecialtyId)
        //{
        //    DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
        //    string currentPhysTempId = string.Empty;
        //    BLObject<DSPhysicalExamECW> obj = null;
        //    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

        //    obj = BLLPhysicalExamECWObj.GetSpecialtyProvider(SpecialtyId);
        //    dsPhysicalExam = obj.Data;

        //    var response = new
        //    {
        //        status = true,
        //        PEProvider_JSON = js.Serialize(MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.SpecialityProvider.TableName])),
        //        PEProviderCount = dsPhysicalExam.Tables[dsPhysicalExam.SpecialityProvider.TableName].Rows.Count,
        //    };
        //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //}

        public string loadPhysicalExamTemplateSection(long templateId, long SystemId)
        {
            DSPhysicalExamTemplate dsPhysicalExam = new DSPhysicalExamTemplate();
            string currentPhysTempId = string.Empty;

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            BLObject<DSPhysicalExamTemplate> objSection = BLLClinicalObj.LoadPhysicalExamTemplateSystemSection(templateId, SystemId);
            List<PhysExamSecTemplateModel> lstPhysExamSecTemplateModel = new List<PhysExamSecTemplateModel>();
            DSPhysicalExamTemplate dsPhysicalExamSec = new DSPhysicalExamTemplate();
            dsPhysicalExamSec = objSection.Data;

            if (dsPhysicalExamSec != null && dsPhysicalExamSec.Tables[dsPhysicalExam.PhysExamTemplateSection.TableName].Rows.Count > 0)
            {

                foreach (DataRow drSec in dsPhysicalExamSec.Tables[dsPhysicalExamSec.PhysExamTemplateSection.TableName].Rows)
                {
                    PhysExamSecTemplateModel objPhysExamSecTemplateModel = new PhysExamSecTemplateModel();
                    objPhysExamSecTemplateModel.IsChecked = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.IsCheckedColumn.ColumnName]);
                    objPhysExamSecTemplateModel.SystemId = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.SystemIdColumn.ColumnName]);
                    objPhysExamSecTemplateModel.SectionId = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.SectionIdColumn.ColumnName]);
                    objPhysExamSecTemplateModel.SectionName = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.SectionNameColumn.ColumnName]);
                    objPhysExamSecTemplateModel.TemplateId = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.TemplateIdColumn.ColumnName]);
                    objPhysExamSecTemplateModel.TemplateSectionId = MDVUtility.ToStr(drSec[dsPhysicalExamSec.PhysExamTemplateSection.TemplateSectionIdColumn.ColumnName]);


                    lstPhysExamSecTemplateModel.Add(objPhysExamSecTemplateModel);
                }



            }

            var response = new
            {
                status = true,
                PhysicalExamTemplateSection = js.Serialize(lstPhysExamSecTemplateModel),
                PhysicalExamTemplateSectionCount = lstPhysExamSecTemplateModel.Count
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

        }


        public string loadPhysicalExamTemplateChar(long templateId, long SectionId)
        {
            DSPhysicalExamTemplate dsPhysicalExam = new DSPhysicalExamTemplate();
            string currentPhysTempId = string.Empty;

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            BLObject<DSPhysicalExamTemplate> objChar = BLLClinicalObj.LoadPhysicalExamTemplateSystemChar(templateId, SectionId);
            List<PhysExamCharTemplateModel> lstPhysExamCharTemplateModel = new List<PhysExamCharTemplateModel>();
            DSPhysicalExamTemplate dsPhysicalExamChar = new DSPhysicalExamTemplate();
            dsPhysicalExamChar = objChar.Data;

            if (dsPhysicalExamChar != null && dsPhysicalExamChar.Tables[dsPhysicalExamChar.PhysExamTemplateChar.TableName].Rows.Count > 0)
            {

                foreach (DataRow drChar in dsPhysicalExamChar.Tables[dsPhysicalExamChar.PhysExamTemplateChar.TableName].Rows)
                {
                    PhysExamCharTemplateModel objPhysExamCharTemplateModel = new PhysExamCharTemplateModel();
                    objPhysExamCharTemplateModel.IsChecked = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.IsCheckedColumn.ColumnName]);
                    objPhysExamCharTemplateModel.CharacteristicId = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.CharIdColumn.ColumnName]);
                    objPhysExamCharTemplateModel.CharName = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.CharNameColumn.ColumnName]);
                    objPhysExamCharTemplateModel.SectionId = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.SectionIdColumn.ColumnName]);
                    objPhysExamCharTemplateModel.TemplateId = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.TemplateIdColumn.ColumnName]);
                    objPhysExamCharTemplateModel.TemplateCharId = MDVUtility.ToStr(drChar[dsPhysicalExamChar.PhysExamTemplateChar.TemplateCharIdColumn.ColumnName]);

                    lstPhysExamCharTemplateModel.Add(objPhysExamCharTemplateModel);
                }
            }

            var response = new
            {
                status = true,
                PhysicalExamTemplateChar = js.Serialize(lstPhysExamCharTemplateModel),
                PhysicalExamTemplateCharCount = lstPhysExamCharTemplateModel.Count
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


        }


        public string loadPhysicalExamTemplateSubChar(long templateId, long CharId)
        {
            DSPhysicalExamTemplate dsPhysicalExam = new DSPhysicalExamTemplate();
            string currentPhysTempId = string.Empty;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            BLObject<DSPhysicalExamTemplate> objSubChar = BLLClinicalObj.loadPhysicalExamTemplateSystemSubChar(templateId, CharId);
            List<PhysExamSubCharTemplateModel> lstPhysExamSubCharTemplateModel = new List<PhysExamSubCharTemplateModel>();
            DSPhysicalExamTemplate dsPhysicalExamSubChar = new DSPhysicalExamTemplate();
            dsPhysicalExamSubChar = objSubChar.Data;

            if (dsPhysicalExamSubChar != null && dsPhysicalExamSubChar.Tables[dsPhysicalExamSubChar.PhysExamTemplateSubChar.TableName].Rows.Count > 0)
            {

                foreach (DataRow drSubChar in dsPhysicalExamSubChar.Tables[dsPhysicalExamSubChar.PhysExamTemplateSubChar.TableName].Rows)
                {
                    PhysExamSubCharTemplateModel objPhysExamSubCharTemplateModel = new PhysExamSubCharTemplateModel();
                    objPhysExamSubCharTemplateModel.IsChecked = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.IsCheckedColumn.ColumnName]);
                    objPhysExamSubCharTemplateModel.CharacteristicId = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.CharIdColumn.ColumnName]);
                    objPhysExamSubCharTemplateModel.SubCharacteristicId = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.SubCharIdColumn.ColumnName]);
                    objPhysExamSubCharTemplateModel.SubCharName = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.SubCharNameColumn.ColumnName]);
                    objPhysExamSubCharTemplateModel.TemplateId = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.TemplateIdColumn.ColumnName]);
                    objPhysExamSubCharTemplateModel.TemplateSubCharId = MDVUtility.ToStr(drSubChar[dsPhysicalExamSubChar.PhysExamTemplateSubChar.TemplateSubCharIdColumn.ColumnName]);
                    lstPhysExamSubCharTemplateModel.Add(objPhysExamSubCharTemplateModel);
                }

            }
            var response = new
            {
                status = true,
                PhysicalExamTemplateSubChar = js.Serialize(lstPhysExamSubCharTemplateModel),
                PhysicalExamTemplateSubCharCount = lstPhysExamSubCharTemplateModel.Count
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


        }

        public string loadPhysicalExamTemplatesECW(long templateId, long entityId, int? IsActive = null)
        {
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
            string currentPhysTempId = string.Empty;
            BLObject<DSPhysicalExamECW> obj = null;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            obj = BLLPhysicalExamECWObj.loadPhysicalExamTemplatesECW(templateId, entityId, IsActive);
            dsPhysicalExam = obj.Data;

            var response = new
            {
                status = true,
                PhysicalExamTemplate = js.Serialize(MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PETemplate.TableName])),
                PhysicalExamTemplateCount = dsPhysicalExam.Tables[dsPhysicalExam.PETemplate.TableName].Rows.Count,
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }

        // Author: Farooq Ahmad
        // Created Date: 16/03/2016
        //OverView: This function will delete PhysicalExamTemplate
        public string deletePhysicalExamTemplate(long templateId)
        {
            string currentPhysTempId = string.Empty;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            BLObject<string> obj = BLLClinicalObj.deletePhysicalExamTemplate(templateId);
            currentPhysTempId = obj.Data;
            if (currentPhysTempId == "")
            {
                var response = new { status = true, message = "Physical exam template deleted successfully." };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new { status = false, message = currentPhysTempId };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        public string deletePETemplate(long templateId)
        {
            string currentPhysTempId = string.Empty;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            BLObject<string> obj = BLLPhysicalExamECWObj.deletePETemplate(templateId);
            currentPhysTempId = obj.Data;
            if (string.IsNullOrEmpty(currentPhysTempId) || currentPhysTempId == "Successfully Deleted")
            {
                var response = new { status = true, message = "Physical exam template deleted successfully." };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new { status = false, message = currentPhysTempId };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        // Author: Farooq Ahmad
        // Created Date: 06/05/2016
        //OverView: This function will delete PhysicalExamTemplate
        public string updatePhysicalExamTemplateIsActive(Int64 templateID, Int64 IsActive, Int64 EntityId)
        {
            try
            {
                if (templateID > 0)
                {
                    DSPhysicalExamTemplate dsPhysicalExam = new DSPhysicalExamTemplate();
                    BLObject<DSPhysicalExamTemplate> obj = BLLClinicalObj.loadPhysicalExamTemplate(templateID, EntityId);
                    dsPhysicalExam = obj.Data;
                    if (dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplate.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplate.TableName].Rows[0];
                        dr[dsPhysicalExam.PhysExamTemplate.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPhysicalExamTemplate> objInsertedNormalSystem = BLLClinicalObj.insertUpdatePhysicalExamTemplate(dsPhysicalExam);
                        string successMsg;
                        if (objInsertedNormalSystem.Data != null)
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
                                Message = objInsertedNormalSystem.Message
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string PETemplateIsActive(long templateID, long IsActive)
        {
            try
            {
                BLObject<string> objInsertedNormalSystem = BLLPhysicalExamECWObj.PETemplateIsActive(templateID, IsActive);
                if (objInsertedNormalSystem.Data == "")
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
                        Message = objInsertedNormalSystem.Data
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

        #region Physical Exam Template system Fill,Save and Update Methods

        // Author: Muhammad Arshad
        // Created Date: 11/03/2016
        //OverView: This function will build PhysExamTemplateSys Row
        public void buildTemplateSystemRow(long TemplateId, PhysExamSysTemplateModel systemModel, DSPhysicalExamTemplate dsPhysExamTemplateSystem, string saveAsTemplateName, bool isNewSystem = false)
        {
            if (!string.IsNullOrEmpty(saveAsTemplateName))
            {
                if (!isNewSystem)
                {
                    return;
                }
            }
            DSPhysicalExamTemplate.PhysExamTemplateSysRow drTemplateSystem = null;

            DSPhysicalExamTemplate.PhysExamTemplateSysRow[] arrTemplateSystem = (DSPhysicalExamTemplate.PhysExamTemplateSysRow[])dsPhysExamTemplateSystem.PhysExamTemplateSys.Select(dsPhysExamTemplateSystem.PhysExamTemplateSys.SystemIdColumn.ColumnName + "=" + systemModel.SystemId);
            if (arrTemplateSystem.Length > 0)
            {
                drTemplateSystem = arrTemplateSystem[0];
            }
            else
            {
                drTemplateSystem = dsPhysExamTemplateSystem.PhysExamTemplateSys.NewPhysExamTemplateSysRow();
            }

            if (drTemplateSystem != null)
            {
                drTemplateSystem.TemplateId = TemplateId;//Utility.ToInt64(systemModel.PhysicalExamSystemId);
                drTemplateSystem.SystemId = MDVUtility.ToInt64(systemModel.SystemId);
                drTemplateSystem.SystemName = systemModel.SystemName;
                drTemplateSystem.IsChecked = systemModel.IsChecked.ToLower() == "true" ? true : false;

                drTemplateSystem.IsActive = true;

                if (arrTemplateSystem.Length < 1)
                {
                    drTemplateSystem.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drTemplateSystem.CreatedOn = DateTime.Now;
                }
                drTemplateSystem.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drTemplateSystem.ModifiedOn = DateTime.Now;

                if (arrTemplateSystem.Length < 1)
                {
                    dsPhysExamTemplateSystem.PhysExamTemplateSys.AddPhysExamTemplateSysRow(drTemplateSystem);
                }
            }
        }

        // Author: Muhammad Arshad
        // Created Date: 11/03/2016
        //OverView: This function will handle insert/update of Physical Exam Template
        public string insertUpdatePhysicalExamTemplateSystem(DSPhysicalExamTemplate dsPhysicalExam)
        {
            #region Database Insertion/Updation

            BLObject<DSPhysicalExamTemplate> objInsertedNormalSystem = BLLClinicalObj.InsertUpdatePatientPhysicalExamTemplateSystem(dsPhysicalExam);
            if (objInsertedNormalSystem.Data != null)
            {

                int TempSystemId = dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplateSys.TableName].Rows.Count > 0 ? MDVUtility.ToInt32(dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplateSys.TableName].Rows[0][dsPhysicalExam.PhysExamTemplateSys.SystemIdColumn.ColumnName]) : 0;

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    phyExamTemplateSystemId = TempSystemId,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedNormalSystem.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }



        #endregion

        #region Physical Template section Fill, Save and Update Methods

        // Author: Muhammad Arshad
        // Created Date: 11/03/2016
        //OverView: This function will build PhysExamTemplateSection Row
        public void buildTemplateSectionRow(long TemplateId, PhysExamSecTemplateModel sectionModel, DSPhysicalExamTemplate dsPhysExamTemplateSection, string saveAsTemplateName, bool isNewSection = false)
        {
            if (!string.IsNullOrEmpty(saveAsTemplateName))
            {
                if (!isNewSection)
                {
                    return;
                }
            }
            DSPhysicalExamTemplate.PhysExamTemplateSectionRow drTemplateSection = null;
            DSPhysicalExamTemplate.PhysExamTemplateSectionRow[] arrTemplateSection = (DSPhysicalExamTemplate.PhysExamTemplateSectionRow[])dsPhysExamTemplateSection.PhysExamTemplateSection.Select(dsPhysExamTemplateSection.PhysExamTemplateSection.SectionIdColumn.ColumnName + "=" + sectionModel.SectionId);
            if (arrTemplateSection.Length > 0)
            {
                drTemplateSection = arrTemplateSection[0];
            }
            else
            {
                drTemplateSection = dsPhysExamTemplateSection.PhysExamTemplateSection.NewPhysExamTemplateSectionRow();
            }

            if (drTemplateSection != null)
            {
                drTemplateSection.TemplateId = TemplateId;
                drTemplateSection.SystemId = MDVUtility.ToInt64(sectionModel.SystemId);
                drTemplateSection.SectionId = MDVUtility.ToInt64(sectionModel.SectionId);
                drTemplateSection.SectionName = sectionModel.SectionName;
                drTemplateSection.IsChecked = sectionModel.IsChecked.ToLower() == "true" ? true : false;

                drTemplateSection.IsActive = true;

                if (arrTemplateSection.Length < 1)
                {
                    drTemplateSection.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drTemplateSection.CreatedOn = DateTime.Now;
                }
                drTemplateSection.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drTemplateSection.ModifiedOn = DateTime.Now;

                if (arrTemplateSection.Length < 1)
                {
                    dsPhysExamTemplateSection.PhysExamTemplateSection.AddPhysExamTemplateSectionRow(drTemplateSection);
                }
            }
        }

        // Author: Muhammad Arshad
        // Created Date: 11/03/2016
        //OverView: This function will handle insert/update of PhysExamTemplateSection
        public string insertUpdatePhysicalExamTemplateSection(DSPhysicalExamTemplate dsPhysicalExam)
        {
            #region Database Insertion/Updation

            BLObject<DSPhysicalExamTemplate> objInsertedNormalSystem = BLLClinicalObj.InsertUpdatePatientPhysicalExamTemplateSection(dsPhysicalExam);
            if (objInsertedNormalSystem.Data != null)
            {

                int TempSectionId = dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplateSection.TableName].Rows.Count > 0 ? MDVUtility.ToInt32(dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplateSection.TableName].Rows[0][dsPhysicalExam.PhysExamTemplateSection.SectionIdColumn.ColumnName]) : 0;

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    phyExamTemplateSectionId = TempSectionId,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedNormalSystem.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }



        #endregion

        #region Physical Exam Template Char Fill, Save and Update Methods

        // Author: Muhammad Arshad
        // Created Date: 11/03/2016
        //OverView: This function will build PhysExamTemplateChar Row
        public void buildTemplateCharRow(long TemplateId, PhysExamCharTemplateModel CharModel, DSPhysicalExamTemplate dsPhysExamTemplateChar, string saveAsTemplateName, bool isNewChar)
        {
            if (!string.IsNullOrEmpty(saveAsTemplateName))
            {
                if (!isNewChar)
                {
                    return;
                }
            }
            DSPhysicalExamTemplate.PhysExamTemplateCharRow drTemplateChar = null;
            DSPhysicalExamTemplate.PhysExamTemplateCharRow[] arrTemplateChar = (DSPhysicalExamTemplate.PhysExamTemplateCharRow[])dsPhysExamTemplateChar.PhysExamTemplateChar.Select(dsPhysExamTemplateChar.PhysExamTemplateChar.CharIdColumn.ColumnName + "=" + CharModel.CharacteristicId);
            if (arrTemplateChar.Length > 0)
            {
                drTemplateChar = arrTemplateChar[0];
            }
            else
            {
                drTemplateChar = dsPhysExamTemplateChar.PhysExamTemplateChar.NewPhysExamTemplateCharRow();
            }

            if (drTemplateChar != null)
            {
                drTemplateChar.TemplateId = TemplateId;
                drTemplateChar.SectionId = MDVUtility.ToInt64(CharModel.SectionId);
                drTemplateChar.CharId = MDVUtility.ToInt64(CharModel.CharacteristicId);
                drTemplateChar.CharName = CharModel.CharName;
                drTemplateChar.IsChecked = CharModel.IsChecked.ToLower() == "true" ? true : false;

                drTemplateChar.IsActive = true;

                if (arrTemplateChar.Length < 1)
                {
                    drTemplateChar.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drTemplateChar.CreatedOn = DateTime.Now;
                }
                drTemplateChar.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drTemplateChar.ModifiedOn = DateTime.Now;

                if (arrTemplateChar.Length < 1)
                {
                    dsPhysExamTemplateChar.PhysExamTemplateChar.AddPhysExamTemplateCharRow(drTemplateChar);
                }
            }
        }

        // Author: Muhammad Arshad
        // Created Date: 11/03/2016
        //OverView: This function will handle insert/update of PhysExamTemplateChar
        public string insertUpdatePhysicalExamTemplateChar(DSPhysicalExamTemplate dsPhysicalExam)
        {
            #region Database Insertion/Updation

            BLObject<DSPhysicalExamTemplate> objInsertedNormalSystem = BLLClinicalObj.InsertUpdatePatientPhysicalExamTemplateChar(dsPhysicalExam);
            if (objInsertedNormalSystem.Data != null)
            {

                int TempCharId = dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplateChar.TableName].Rows.Count > 0 ? MDVUtility.ToInt32(dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplateChar.TableName].Rows[0][dsPhysicalExam.PhysExamTemplateChar.CharIdColumn.ColumnName]) : 0;

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    phyExamTemplateCharId = TempCharId,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedNormalSystem.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }



        #endregion

        #region Physical Exam Template SubChar Fill, Save and Update Methods

        // Author: Muhammad Arshad
        // Created Date: 11/03/2016
        //OverView: This function will build PhysExamTemplateSubChar Row
        public void buildTemplateSubCharRow(long TemplateId, PhysExamSubCharTemplateModel SubCharModel, DSPhysicalExamTemplate dsPhysExamTemplateSubChar, string saveAsTemplateName, bool isNewSubChar)
        {
            if (!string.IsNullOrEmpty(saveAsTemplateName))
            {
                if (!isNewSubChar)
                {
                    return;
                }
            }
            DSPhysicalExamTemplate.PhysExamTemplateSubCharRow drTemplateSubChar = null;
            DSPhysicalExamTemplate.PhysExamTemplateSubCharRow[] arrTemplateSubChar = (DSPhysicalExamTemplate.PhysExamTemplateSubCharRow[])dsPhysExamTemplateSubChar.PhysExamTemplateSubChar.Select(dsPhysExamTemplateSubChar.PhysExamTemplateSubChar.SubCharIdColumn.ColumnName + "=" + SubCharModel.SubCharacteristicId);
            if (arrTemplateSubChar.Length > 0)
            {
                drTemplateSubChar = arrTemplateSubChar[0];
            }
            else
            {
                drTemplateSubChar = dsPhysExamTemplateSubChar.PhysExamTemplateSubChar.NewPhysExamTemplateSubCharRow();
            }

            if (drTemplateSubChar != null)
            {
                drTemplateSubChar.TemplateId = TemplateId;
                drTemplateSubChar.CharId = MDVUtility.ToInt64(SubCharModel.CharacteristicId);
                drTemplateSubChar.SubCharId = MDVUtility.ToInt64(SubCharModel.SubCharacteristicId);
                drTemplateSubChar.SubCharName = SubCharModel.SubCharName;
                drTemplateSubChar.IsChecked = SubCharModel.IsChecked.ToLower() == "true" ? true : false;

                drTemplateSubChar.IsActive = true;

                if (arrTemplateSubChar.Length < 1)
                {
                    drTemplateSubChar.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drTemplateSubChar.CreatedOn = DateTime.Now;
                }
                drTemplateSubChar.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                drTemplateSubChar.ModifiedOn = DateTime.Now;

                if (arrTemplateSubChar.Length < 1)
                {
                    dsPhysExamTemplateSubChar.PhysExamTemplateSubChar.AddPhysExamTemplateSubCharRow(drTemplateSubChar);
                }
            }
        }

        // Author: Muhammad Arshad
        // Created Date: 11/03/2016
        //OverView: This function will handle insert/update of PhysExamTemplateSubChar
        public string insertUpdatePhysicalExamTemplateSubChar(DSPhysicalExamTemplate dsPhysicalExam)
        {
            #region Database Insertion/Updation

            BLObject<DSPhysicalExamTemplate> objInsertedNormalSystem = BLLClinicalObj.InsertUpdatePatientPhysicalExamTemplateSubChar(dsPhysicalExam);
            if (objInsertedNormalSystem.Data != null)
            {

                int TempSubCharId = dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplateSubChar.TableName].Rows.Count > 0 ? MDVUtility.ToInt32(dsPhysicalExam.Tables[dsPhysicalExam.PhysExamTemplateSubChar.TableName].Rows[0][dsPhysicalExam.PhysExamTemplateSubChar.SubCharIdColumn.ColumnName]) : 0;

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    phyExamTemplateSubCharId = TempSubCharId,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedNormalSystem.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }

        #endregion

        #region Insert PhysicalExam System, section, Characteristics, SubCharacteristics

        // Author: Farooq Ahmad
        // Date: 15/03/2016
        //This function will insert/update Physical Exam Template
        public string insertPhysicalExamSystem(PhysExamSysTemplateModel model)
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            if (model != null)
            {
                DSPhysicalExamLookup.PhysicalExamSystemRow RowNormalSystem = ds.PhysicalExamSystem.NewPhysicalExamSystemRow();
                if (RowNormalSystem != null)
                {
                    RowNormalSystem.SystemId = -1;
                    RowNormalSystem.ShortName = model.SystemName;
                    RowNormalSystem.Description = model.SystemName;
                    RowNormalSystem.TemplateId = MDVUtility.ToInt64(model.TemplateId);
                    RowNormalSystem.IsActive = true;
                    ds.PhysicalExamSystem.AddPhysicalExamSystemRow(RowNormalSystem);
                }
            }
            #region Database Insertion/Updation

            BLObject<DSPhysicalExamLookup> objInsertedNormalSystem = BLLClinicalObj.insertPhysicalExamSystem(ds);
            if (objInsertedNormalSystem.Data != null)
            {

                int TempSystemId = ds.Tables[ds.PhysicalExamSystem.TableName].Rows.Count > 0 ? MDVUtility.ToInt32(ds.Tables[ds.PhysicalExamSystem.TableName].Rows[0][ds.PhysicalExamSystem.SystemIdColumn.ColumnName]) : 0;


                return TempSystemId.ToString();
            }
            else
            {
                return string.Empty;
            }

            #endregion
        }

        // Author: Farooq Ahmad
        // Date: 15/03/2016
        //This function will insert/update Physical Exam Template
        public string insertPhysicalExamSystemSection(PhysExamSecTemplateModel model)
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            if (model != null)
            {
                DSPhysicalExamLookup.PhysicalExamSystemSectionRow RowNormalSystem = ds.PhysicalExamSystemSection.NewPhysicalExamSystemSectionRow();
                if (RowNormalSystem != null)
                {
                    RowNormalSystem.SectionId = -1;
                    RowNormalSystem.PhysicalExamSystemId = MDVUtility.ToInt64(model.SystemId);
                    RowNormalSystem.ShortName = model.SectionName;
                    RowNormalSystem.Description = model.SectionName;
                    RowNormalSystem.TemplateId = MDVUtility.ToInt64(model.TemplateId);
                    RowNormalSystem.IsActive = true;
                    ds.PhysicalExamSystemSection.AddPhysicalExamSystemSectionRow(RowNormalSystem);
                }
            }
            #region Database Insertion/Updation

            BLObject<DSPhysicalExamLookup> objInsertedNormalSystem = BLLClinicalObj.insertPhysicalExamSystemSection(ds);
            if (objInsertedNormalSystem.Data != null)
            {

                int TempSectionId = ds.Tables[ds.PhysicalExamSystemSection.TableName].Rows.Count > 0 ? MDVUtility.ToInt32(ds.Tables[ds.PhysicalExamSystemSection.TableName].Rows[0][ds.PhysicalExamSystemSection.SectionIdColumn.ColumnName]) : 0;
                return TempSectionId.ToString();

            }
            else
            {

                return string.Empty;
            }

            #endregion
        }

        // Author: Farooq Ahmad
        // Date: 15/03/2016
        //This function will insert/update Physical Exam Template
        public string insertPhysicalExamSystemSectionCharacteristics(PhysExamCharTemplateModel model)
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            if (model != null)
            {
                DSPhysicalExamLookup.PhysicalExamSystemSectionCharacteristicRow RowNormalSystem = ds.PhysicalExamSystemSectionCharacteristic.NewPhysicalExamSystemSectionCharacteristicRow();
                if (RowNormalSystem != null)
                {
                    RowNormalSystem.CharacteristicId = -1;
                    RowNormalSystem.SectionId = MDVUtility.ToInt64(model.SectionId);
                    RowNormalSystem.ShortName = model.CharName;
                    RowNormalSystem.Description = model.CharName;
                    RowNormalSystem.TemplateId = MDVUtility.ToInt64(model.TemplateId);
                    RowNormalSystem.IsActive = true;
                    ds.PhysicalExamSystemSectionCharacteristic.AddPhysicalExamSystemSectionCharacteristicRow(RowNormalSystem);
                }
            }
            #region Database Insertion/Updation

            BLObject<DSPhysicalExamLookup> objInsertedNormalSystem = BLLClinicalObj.insertPhysicalExamSystemSectionCharacteristics(ds);
            if (objInsertedNormalSystem.Data != null)
            {

                int TempCharacteristicId = ds.Tables[ds.PhysicalExamSystemSectionCharacteristic.TableName].Rows.Count > 0 ? MDVUtility.ToInt32(ds.Tables[ds.PhysicalExamSystemSectionCharacteristic.TableName].Rows[0][ds.PhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName]) : 0;


                return TempCharacteristicId.ToString();
            }
            else
            {

                return string.Empty;
            }

            #endregion
        }

        // Author: Farooq Ahmad
        // Date: 15/03/2016
        //This function will insert/update Physical Exam Template
        public string insertPhysicalExamSystemSectionCharacteristicsSubCharacteristic(PhysExamSubCharTemplateModel model)
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            if (model != null)
            {
                DSPhysicalExamLookup.PhysicalExamSystemSectionCharacteristicSubCharacteristicRow RowNormalSystem = ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.NewPhysicalExamSystemSectionCharacteristicSubCharacteristicRow();
                if (RowNormalSystem != null)
                {
                    RowNormalSystem.SubCharacteristicId = -1;
                    RowNormalSystem.CharacteristicId = MDVUtility.ToInt64(model.CharacteristicId);
                    RowNormalSystem.ShortName = model.SubCharName;
                    RowNormalSystem.Description = model.SubCharName;
                    RowNormalSystem.TemplateId = MDVUtility.ToInt64(model.TemplateId);
                    RowNormalSystem.IsActive = true;
                    ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.AddPhysicalExamSystemSectionCharacteristicSubCharacteristicRow(RowNormalSystem);
                }
            }
            #region Database Insertion/Updation

            BLObject<DSPhysicalExamLookup> objInsertedNormalSystem = BLLClinicalObj.insertPhysicalExamSystemSectionCharacteristicsSubCharacteristic(ds);
            if (objInsertedNormalSystem.Data != null)
            {

                int TempSubCharacteristicId = ds.Tables[ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName].Rows.Count > 0 ? MDVUtility.ToInt32(ds.Tables[ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName].Rows[0][ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName]) : 0;


                return TempSubCharacteristicId.ToString();
            }
            else
            {

                return string.Empty;
            }

            #endregion
        }

        public DSPhysicalExamLookup insertPhysicalExamSystemSectionCharacteristicsSubCharacteristic(PhysExamSubCharTemplateModel model, DSPhysicalExamLookup ds)
        {

            if (model != null)
            {
                DSPhysicalExamLookup.PhysicalExamSystemSectionCharacteristicSubCharacteristicRow RowNormalSystem = ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.NewPhysicalExamSystemSectionCharacteristicSubCharacteristicRow();
                if (RowNormalSystem != null)
                {
                    RowNormalSystem.SubCharacteristicId = -1;
                    RowNormalSystem.CharacteristicId = MDVUtility.ToInt64(model.CharacteristicId);
                    RowNormalSystem.ShortName = model.SubCharName;
                    RowNormalSystem.Description = model.SubCharName;
                    RowNormalSystem.TemplateId = MDVUtility.ToInt64(model.TemplateId);
                    RowNormalSystem.IsActive = true;
                    ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.AddPhysicalExamSystemSectionCharacteristicSubCharacteristicRow(RowNormalSystem);
                }
            }
            return ds;
        }



        #endregion

        #region PE ECW, MK

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

        DataTable GetSystemObservationXMLTable(PhysicalExamTemplateModelECW modelPESystemObservations)
        {
            DataTable SystemObservationTable = new DataTable() { TableName = "PETempPart" };

            SystemObservationTable.Columns.Add("PETemplateId", typeof(string));
            SystemObservationTable.Columns.Add("PESystemId", typeof(int));
            SystemObservationTable.Columns.Add("PEObservationId", typeof(int));
            SystemObservationTable.Columns.Add("PESystemIsSelected", typeof(int));
            SystemObservationTable.Columns.Add("PEObservationIsSelected", typeof(int));
            SystemObservationTable.Columns.Add("IsSystemDeleted", typeof(int));
            SystemObservationTable.Columns.Add("IsObservationDeleted", typeof(int));
            SystemObservationTable.Columns.Add("ObservationOrder", typeof(int));

            if (MDVUtility.ToInt64(modelPESystemObservations.TemplateId) != -1)
            {
                for (int i = 0; i < modelPESystemObservations.SystemObservationData.Count; i++)
                {
                    if (!string.IsNullOrEmpty(modelPESystemObservations.SystemObservationData[i].ObservationId))
                        if (MDVUtility.ToInt64(modelPESystemObservations.SystemObservationData[i].ObservationId) != -1)
                            if (MDVUtility.ToInt64(modelPESystemObservations.SystemObservationData[i].PESystemId) != -1)
                                SystemObservationTable.Rows.Add
                                    (
                                        string.IsNullOrEmpty(modelPESystemObservations.SystemObservationData[i].TemplateId) ? "-1" : modelPESystemObservations.SystemObservationData[i].TemplateId,
                                        modelPESystemObservations.SystemObservationData[i].PESystemId,
                                        modelPESystemObservations.SystemObservationData[i].ObservationId,
                                        MDVUtility.ToBool(modelPESystemObservations.SystemObservationData[i].IsSystemChecked) == true ? 1 : 0,
                                        MDVUtility.ToBool(modelPESystemObservations.SystemObservationData[i].IsChecked) == true ? 1 : 0,
                                        modelPESystemObservations.SystemObservationData[i].IsSystemDeleted,
                                        modelPESystemObservations.SystemObservationData[i].IsObservationDeleted,
                                        modelPESystemObservations.SystemObservationData[i].ObservationOrder
                                    );
                }
            }
            else
            {
                for (int i = 0; i < modelPESystemObservations.SystemObservationData.Count; i++)
                {
                    if (!string.IsNullOrEmpty(modelPESystemObservations.SystemObservationData[i].ObservationId))
                    {
                        SystemObservationTable.Rows.Add
                            (
                                string.IsNullOrEmpty(modelPESystemObservations.SystemObservationData[i].TemplateId) ? "-1" : modelPESystemObservations.SystemObservationData[i].TemplateId,
                                modelPESystemObservations.SystemObservationData[i].PESystemId,
                                modelPESystemObservations.SystemObservationData[i].ObservationId,
                                MDVUtility.ToBool(modelPESystemObservations.SystemObservationData[i].IsSystemChecked) == true ? 1 : 0,
                                MDVUtility.ToBool(modelPESystemObservations.SystemObservationData[i].IsChecked) == true ? 1 : 0,
                                modelPESystemObservations.SystemObservationData[i].IsSystemDeleted,
                                modelPESystemObservations.SystemObservationData[i].IsObservationDeleted,
                                modelPESystemObservations.SystemObservationData[i].ObservationOrder
                            );
                    }
                }
            }
            return SystemObservationTable;
        }


        public string insertUpdatePhysicalExamTemplateECW(long TemplateId, PhysicalExamTemplateModelECW modelPESystemObservations)
        {
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();

            try
            {

                string providerXMLString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetProviderXMLTable(modelPESystemObservations.ProviderIds).WriteXml(writer);
                    providerXMLString = writer.ToString();
                }

                string SystemObservationXMString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetSystemObservationXMLTable(modelPESystemObservations).WriteXml(writer);
                    SystemObservationXMString = writer.ToString();
                }

                string specialtyXMLString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetSpecialtyXMLTable(modelPESystemObservations.SpecialtyIds).WriteXml(writer);
                    specialtyXMLString = writer.ToString();
                }

                DSPhysicalExamECW.PETemplateSystemObservationsRow dr = dsPhysicalExam.PETemplateSystemObservations.NewPETemplateSystemObservationsRow();
                dr.Name = modelPESystemObservations.PhysicalExamTemplateName;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                if (!string.IsNullOrEmpty(modelPESystemObservations.BodyPartId))
                    dr.BodyPartId = MDVUtility.ToInt64(modelPESystemObservations.BodyPartId);
                else
                    dr[dsPhysicalExam.PETemplateSystemObservations.BodyPartIdColumn] = DBNull.Value;
                dr.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                dr.ProviderXML = providerXMLString;
                dr.SpecialtyXML = specialtyXMLString;
                dr.EntityId = MDVUtility.ToInt64(modelPESystemObservations.PhysicalExamTemplateEntity);
                dr.SystemObservationXML = SystemObservationXMString;
                dr.TemplatePreview = modelPESystemObservations.TemplatePreview.Trim();
                dr.PETemplateId = TemplateId.ToString();

                dsPhysicalExam.PETemplateSystemObservations.AddPETemplateSystemObservationsRow(dr);

                BLObject<DSPhysicalExamECW> obj = BLLPhysicalExamECWObj.SavePhysicalExamSystemObservatiosECW(dsPhysicalExam);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = "Saved Successfully!"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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
        public string loadPhysicalExamTemplatesFillECW(long templateId)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
                string currentPhysTempId = string.Empty;
                BLObject<DSPhysicalExamECW> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                obj = BLLPhysicalExamECWObj.loadPhysicalExamTempSysObservations(templateId);

                if (obj.Data != null)
                {
                    dsPhysicalExam = obj.Data;
                    DataView view = new DataView(dsPhysicalExam.PETemplateSystemObservations);
                    DataTable PETemp = view.ToTable(true, "PETemplateId", "TemplateName", "SpecialtyIds", "ProviderIds", "CreatedOn", "CreatedBy", "ModifiedBy", "TemplatePreview",
                        "ModifiedOn", "BodyPartId");
                    DataTable PETempSystem = view.ToTable(true, "PETemplateId", "TemplateName", "PESystemId", "SystemName", "IsSelectedSystem", "PETemplateSystemId");
                    DataTable PESysObservations = view.ToTable(true, "PESystemId", "SystemName", "PEObservationId", "ObservationName", "IsSelected", "ObservationOrder");
                    var response = new
                    {
                        status = true,
                        PETemplate_JSON = js.Serialize(MDVUtility.JSON_DataTable(PETemp)),
                        PETemplateCount = PETemp.Rows.Count,
                        PETemplateSystems_JSON = js.Serialize(MDVUtility.JSON_DataTable(PETempSystem)),
                        PETemplateSystemsCount = PETempSystem.Rows.Count,
                        PESysObservations_JSON = js.Serialize(MDVUtility.JSON_DataTable(PESysObservations)),
                        PESysObservationsCount = PESysObservations.Rows.Count,
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
        public string loadAllPhysicalExamECW(int? IsActive = 1)
        {
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
            string currentPhysTempId = string.Empty;
            BLObject<DSPhysicalExamECW> obj = null;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            obj = BLLPhysicalExamECWObj.loadPhysicalExamECW(0, IsActive);

            if (obj.Data != null)
            {
                dsPhysicalExam = obj.Data;
                var response = new
                {
                    status = true,
                    PETemplate_JSON = js.Serialize(MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PETemplate.TableName])),
                    PETemplateCount = dsPhysicalExam.Tables[dsPhysicalExam.PETemplate.TableName].Rows.Count,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = true,
                    PETemplate_JSON = "[]",
                    PETemplateCount = 0,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string deletePhysicalExamTemplateSystem(string PETemplateSystemId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PETemplateSystemId)))
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
                    BLObject<string> obj = BLLPhysicalExamECWObj.deletePhysicalExamTemplateSystem(PETemplateSystemId);
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
        public string savePhysicalExamNotesObservation(List<PhysExamNotesObservationModelECW> modelList, long NotesId)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PhysExamNotesObservationModelECW>));
                StringWriter textWriter = new StringWriter();
                xmlSerializer.Serialize(textWriter, modelList);
                string xml = textWriter.ToString();
                BLObject<DSPhysicalExamECW> obj = null;
                if (modelList.Count > 0)
                    obj = BLLPhysicalExamECWObj.SavePhysicalExamNotesObservation(xml);
                if (obj != null && obj.Data != null)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                    dsPhysicalExam = obj.Data;
                    DataView view = new DataView(dsPhysicalExam.PENotesObservation);
                    DataTable PETemp = view.ToTable(true, "PETemplateId", "TemplateName", "NotesId");
                    DataTable PETempSystem = view.ToTable(true, "PETemplateId", "TemplateName", "PESystemId", "SystemName", "SystemDescription", "PETemplateSystemId", "PENotesObservationId", "IsSelected");
                    var response = new
                    {
                        status = true,
                        PETemplate_JSON = js.Serialize(MDVUtility.JSON_DataTable(PETemp)),
                        PETemplateCount = PETemp.Rows.Count,
                        PETemplateSystems_JSON = js.Serialize(MDVUtility.JSON_DataTable(PETempSystem)),
                        PETemplateSystemsCount = PETempSystem.Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Save_Error_Message,
                        PhysicalExamFill_JSON = "[]",
                        PhysicalExamCount = 0
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
                    PhysicalExamFill_JSON = "[]",
                    PhysicalExamCount = 0
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string detachPhysicalExamTemplateFromNotes(string NotesId, string PETemplateId)
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
                    BLObject<string> obj = BLLPhysicalExamECWObj.detachPhysicalExamTemplateFromNotes(NotesId, PETemplateId);
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
        public string loadPETempSystemObservationForNotes(long NotesId)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
                string currentPhysTempId = string.Empty;
                BLObject<DSPhysicalExamECW> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                obj = BLLPhysicalExamECWObj.loadPETempSystemObservationForNotes(NotesId);

                if (obj.Data != null)
                {
                    dsPhysicalExam = obj.Data;
                    DataView view = new DataView(dsPhysicalExam.PENotesObservation);
                    DataTable PETemp = view.ToTable(true, "PETemplateId", "TemplateName", "NotesId");
                    DataTable PETempSystem = view.ToTable(true, "PETemplateId", "TemplateName", "PESystemId", "SystemName", "SystemDescription", "PENotesObservationId", "PETemplateSystemId", "IsSelected");
                    var response = new
                    {
                        status = true,
                        PETemplate_JSON = js.Serialize(MDVUtility.JSON_DataTable(PETemp)),
                        PETemplateCount = PETemp.Rows.Count,
                        PETemplateSystems_JSON = js.Serialize(MDVUtility.JSON_DataTable(PETempSystem)),
                        PETemplateSystemsCount = PETempSystem.Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PETemplate_JSON = "[]",
                        PETemplateCount = 0,
                        PETemplateSystems_JSON = "[]",
                        PETemplateSystemsCount = 0,
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
        public string loadPESystemObservationForNotes(long PETemplateSystemId, long NotesId)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
                string currentPhysTempId = string.Empty;
                BLObject<DSPhysicalExamECW> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                obj = BLLPhysicalExamECWObj.loadPESystemObservationForNotes(NotesId, PETemplateSystemId);

                if (obj.Data != null)
                {
                    dsPhysicalExam = obj.Data;
                    DataView view = new DataView(dsPhysicalExam.PENotesSystemObservation);
                    DataTable PEObservation = view.ToTable(true, "PEObservationId", "ObservationName", "PESystemId", "SystemName", "PETemplateSystemId", "PENotesObservationId", "SystemDescription");
                    var response = new
                    {
                        status = true,

                        PEObservation_JSON = js.Serialize(MDVUtility.JSON_DataTable(PEObservation)),
                        PEObservationCount = PEObservation.Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PEObservation_JSON = "[]",
                        PEObservationCount = 0,
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
        public string UpdatePENotesDescription(long PENotesObservationId, string Desr)
        {
            try
            {
                BLObject<string> objInsertedNormalSystem = BLLPhysicalExamECWObj.updatePENotesDescription(PENotesObservationId, Desr);
                if (objInsertedNormalSystem.Data == "")
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
                        Message = objInsertedNormalSystem.Data
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
        public string associatePEObservationAndSystem(long PESystemId, long PEObservationId, long PETemplateSystemId)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();

                DSPhysicalExamECW.PESystemObservationRow dr = dsPhysicalExam.PESystemObservation.NewPESystemObservationRow();
                dr.PESystemId = PESystemId;
                dr.PEObservationId = PEObservationId;
                dr.PETemplateSystemId = PETemplateSystemId;
                dr.CreatedBy= MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dsPhysicalExam.PESystemObservation.AddPESystemObservationRow(dr);


                BLObject<DSPhysicalExamECW> objObservation = BLLPhysicalExamECWObj.insertPhysicalExamSystemObservation(dsPhysicalExam);
                if (objObservation.Data != null)
                {
                    DSPhysicalExamECW dsAssociatedObservation = objObservation.Data;
                    var response = new
                    {
                        Status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        PEObservation_JSON = MDVUtility.JSON_DataTable(dsAssociatedObservation.Tables[dsAssociatedObservation.PESystemObservation.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        PEObservation_JSON = "[]"
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
        public string associatePESystemAndTemplate(long PETemplateId, long PESystemId)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();

                DSPhysicalExamECW.PETemplateSystemRow dr = dsPhysicalExam.PETemplateSystem.NewPETemplateSystemRow();
                dr.PESystemId = PESystemId;
                dr.PETemplateId = PETemplateId;
                dr.IsActive = true;
                dr.IsSelected = true;
                dr.CreatedOn = DateTime.Now;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr[dsPhysicalExam.PETemplateSystem.NotesIdColumn] = DBNull.Value;
                dsPhysicalExam.PETemplateSystem.AddPETemplateSystemRow(dr);


                BLObject<DSPhysicalExamECW> objPETemplateSystem = BLLPhysicalExamECWObj.insertPETemplateSystem(dsPhysicalExam);
                if (objPETemplateSystem.Data != null)
                {
                    dsPhysicalExam = objPETemplateSystem.Data;
                    var response = new
                    {
                        Status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        PETemplateSystem_JSON = MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PETemplateSystem.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        PETemplateSystem_JSON = "[]"
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
        public string loadPhysicalExamForProvider(long providerId)
        {
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
            string currentPhysTempId = string.Empty;
            BLObject<DSPhysicalExamECW> obj = null;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            obj = BLLPhysicalExamECWObj.loadPhysicalExamForProvider(providerId);

            if (obj.Data != null)
            {
                dsPhysicalExam = obj.Data;
                var response = new
                {
                    status = true,
                    PETemplate_JSON = js.Serialize(MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PETemplate.TableName])),
                    PETemplateCount = dsPhysicalExam.Tables[dsPhysicalExam.PETemplate.TableName].Rows.Count,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = true,
                    PETemplate_JSON = "[]",
                    PETemplateCount = 0,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string insertPhysicalExamTemplateForProvider(PhysicalExamTemplateModelECW modelPESystemObservations)
        {
            DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();

            try
            {
                DSPhysicalExamECW.PETemplateRow dr = dsPhysicalExam.PETemplate.NewPETemplateRow();
                dr.Name = modelPESystemObservations.PhysicalExamTemplateName;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.IsActive = true;
                dr.EntityId = MDVUtility.ToStr(MDVSession.Current.EntityId);
                dr.ProviderId = MDVUtility.ToLong(modelPESystemObservations.ProviderId);
                dsPhysicalExam.PETemplate.AddPETemplateRow(dr);
                BLObject<DSPhysicalExamECW> obj = BLLPhysicalExamECWObj.SavePhysicalExamForProvider(dsPhysicalExam);
                if (obj.Data != null)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                    dsPhysicalExam = obj.Data;
                    var response = new
                    {
                        status = true,
                        PETemplateId = dsPhysicalExam.Tables[dsPhysicalExam.PETemplate.TableName].Rows.Count > 0 ? dsPhysicalExam.Tables[dsPhysicalExam.PETemplate.TableName].Rows[0][dsPhysicalExam.PETemplate.PETemplateIdColumn.ColumnName] : 0,
                        Message = Common.AppPrivileges.Save_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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
        public string LoadPhyscialExamForSOAPNote(long templateId)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam = new DSPhysicalExamECW();
                string currentPhysTempId = string.Empty;
                BLObject<DSPhysicalExamECW> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                obj = BLLPhysicalExamECWObj.LoadPhyscialExamForSOAPNote(templateId);

                if (obj.Data != null)
                {
                    dsPhysicalExam = obj.Data;
                    DataView view = new DataView(dsPhysicalExam.PETemplateSystemObservations);
                    DataTable PETemp = view.ToTable(true, "PETemplateId", "TemplateName", "SpecialityIds", "ProviderIds", "CreatedOn", "CreatedBy", "ModifiedBy", "TemplatePreview",
                        "ModifiedOn");
                    DataTable PETempSystem = view.ToTable(true, "PETemplateId", "TemplateName", "PESystemId", "SystemName", "IsSelectedSystem", "PETemplateSystemId");
                    DataTable PESysObservations = view.ToTable(true, "PESystemId", "SystemName", "PEObservationId", "ObservationName", "IsSelected");
                    var response = new
                    {
                        status = true,
                        PETemplate_JSON = js.Serialize(MDVUtility.JSON_DataTable(PETemp)),
                        PETemplateCount = PETemp.Rows.Count,
                        PETemplateSystems_JSON = js.Serialize(MDVUtility.JSON_DataTable(PETempSystem)),
                        PETemplateSystemsCount = PETempSystem.Rows.Count,
                        PESysObservations_JSON = js.Serialize(MDVUtility.JSON_DataTable(PESysObservations)),
                        PESysObservationsCount = PESysObservations.Rows.Count,
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
    }
}