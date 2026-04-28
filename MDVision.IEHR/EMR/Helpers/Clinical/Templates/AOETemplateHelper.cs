using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.IEHR.EMR.Model.PhysicalExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using System.IO;
using MDVision.IEHR.Common;
using System.Reflection;
using System.Xml.Serialization;
using MDVision.Model.Clinical.AOETemplates;
namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates
{
    public class AOETemplateHelper
    {
                private BLLClinical BLLClinicalObj = null;
        private BLLAOETemplates BLLAOEObj = null;
        public AOETemplateHelper()
        {
            BLLClinicalObj = new BLLClinical();
            BLLAOEObj = new BLLAOETemplates();
        }
        private static AOETemplateHelper _instance = null;
        public static AOETemplateHelper Instance()
        {
            if (_instance == null)
                _instance = new AOETemplateHelper();
            return _instance;
        }

        public string insertUpdateAOETemplate(long TemplateId, AOETemplatesModel modelPESystemObservations)
        {
            DSAOETemplate dsAOETemplate = new DSAOETemplate();
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

                DSAOETemplate.AOETemplateSystemObservationsRow dr = dsAOETemplate.AOETemplateSystemObservations.NewAOETemplateSystemObservationsRow();
                dr.Name = modelPESystemObservations.Name;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                dr.LabId = MDVUtility.ToInt64(modelPESystemObservations.LabId);
                if (modelPESystemObservations.IsActive == "1" || modelPESystemObservations.IsActive.ToLower() == "true")
                {
                    dr.IsActive = true;
                }
                else
                {
                    dr.IsActive = false;
                }
                dr.CPTCode = modelPESystemObservations.CPTCode;
                dr.CPTCodeDescription = modelPESystemObservations.CPTCodeDescription;
                dr.ProviderXML = providerXMLString;
                dr.SpecialtyXML = specialtyXMLString;
                dr.EntityId = MDVUtility.ToInt64(modelPESystemObservations.PhysicalExamTemplateEntity);
                dr.SystemObservationXML = SystemObservationXMString;
                dr.TemplatePreview = modelPESystemObservations.TemplatePreview.Trim();
                dr.AOETemplateId = TemplateId.ToString();


                dsAOETemplate.AOETemplateSystemObservations.AddAOETemplateSystemObservationsRow(dr);
                BLObject<DSAOETemplate> obj = BLLAOEObj.SaveAOETemplateSystemObservatios(dsAOETemplate);
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
        public string deleteAOETemplate(long templateId)
        {
            string currentPhysTempId = string.Empty;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            BLObject<string> obj = BLLAOEObj.deleteAOETemplate(templateId);
            currentPhysTempId = obj.Data;
            if (string.IsNullOrEmpty(currentPhysTempId) || currentPhysTempId == "Successfully Deleted")
            {
                var response = new { status = true, message = "AOE template deleted successfully." };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new { status = false, message = currentPhysTempId };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }
        public string associateAOEObservationAndSystem(long PESystemId, long PEObservationId, long AOETemplateSystemId)
        {
            try
            {
                DSAOETemplate dsPhysicalExam = new DSAOETemplate();

                DSAOETemplate.AOESystemObservationRow dr = dsPhysicalExam.AOESystemObservation.NewAOESystemObservationRow();
                dr.PESystemId = PESystemId;
                dr.PEObservationId = PEObservationId;
                dr.AOETemplateSystemId = AOETemplateSystemId;
                dsPhysicalExam.AOESystemObservation.AddAOESystemObservationRow(dr);


                BLObject<DSAOETemplate> objObservation = BLLAOEObj.insertAOESystemObservation(dsPhysicalExam);
                if (objObservation.Data != null)
                {
                    DSAOETemplate dsAssociatedObservation = objObservation.Data;
                    var response = new
                    {
                        Status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        AOEObservation_JSON = MDVUtility.JSON_DataTable(dsAssociatedObservation.Tables[dsAssociatedObservation.AOESystemObservation.TableName]),
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
        public string loadAOETemplates(AOETemplatesModel model) {
            List<AOETemplatesModel> obj = BLLAOEObj.LoadAOETemplates(model);
            if (obj != null)
            {
                var response = new
                {
                    status = true,
                    AOETemplates = obj
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            {
                var response = new
                {
                    status = false,
                    Message = "Couldn't load data"
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        DataTable GetSystemObservationXMLTable(AOETemplatesModel modelPESystemObservations)
        {
            DataTable SystemObservationTable = new DataTable() { TableName = "PETempPart" };

            SystemObservationTable.Columns.Add("PETemplateId", typeof(string));
            SystemObservationTable.Columns.Add("PESystemId", typeof(int));
            SystemObservationTable.Columns.Add("PEObservationId", typeof(int));
            SystemObservationTable.Columns.Add("PESystemIsSelected", typeof(int));
            SystemObservationTable.Columns.Add("PEObservationIsSelected", typeof(int));
            SystemObservationTable.Columns.Add("IsSystemDeleted", typeof(int));
            SystemObservationTable.Columns.Add("IsObservationDeleted", typeof(int));

            if (MDVUtility.ToInt64(modelPESystemObservations.AOETemplateId) != -1)
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
                                        modelPESystemObservations.SystemObservationData[i].IsObservationDeleted
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
                                modelPESystemObservations.SystemObservationData[i].IsObservationDeleted
                            );
                    }
                }
            }
            return SystemObservationTable;
        }

        public string ActiveInactiveAOETemplate(AOETemplatesModel model)
        {
            string result = BLLAOEObj.ActiveInActiveAOETemplate(model);
            if (result == "")
            {
                var response = new
                {
                    status = false,
                    Message = "Couldn't perform this operation."
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else
            {
                var response = new
                {
                    status = true,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            
        }

        public string UpdateAOENotesDescription(long AOENotesObservationId, string Desr)
        {
            try
            {
                BLObject<string> objInsertedNormalSystem = BLLAOEObj.updateAOENotesDescription(AOENotesObservationId, Desr);
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
        public string UpdateAOENotesRadiologyDescription(long AOENotesRadiologyObservationId, string Desr)
        {
            try
            {
                BLObject<string> objInsertedNormalSystem = BLLAOEObj.updateAOENotesRadiologyDescription(AOENotesRadiologyObservationId, Desr);
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


        public string loadAOESystemObservationForNotes(long AOETemplateSystemId, long NotesId)
        {
            try
            {
                DSAOETemplate dsPhysicalExam = new DSAOETemplate();
                string currentPhysTempId = string.Empty;
                BLObject<DSAOETemplate> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                obj = BLLAOEObj.loadAOESystemObservationForNotes(NotesId, AOETemplateSystemId);

                if (obj.Data != null)
                {
                    dsPhysicalExam = obj.Data;
                    DataView view = new DataView(dsPhysicalExam.AOENotesSystemObservation);
                    DataTable AOEObservation = view.ToTable(true, "PEObservationId", "ObservationName", "PESystemId", "SystemName", "AOETemplateSystemId", "AOENotesObservationId", "SystemDescription");
                    var response = new
                    {
                        status = true,

                        AOEObservation_JSON = js.Serialize(MDVUtility.JSON_DataTable(AOEObservation)),
                        AOEObservationCount = AOEObservation.Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        AOEObservation_JSON = "[]",
                        AOEObservationCount = 0,
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

        public string loadAOESystemRadiologyObservationForNotes(long AOETemplateSystemId, long NotesId)
        {
            try
            {
                DSAOETemplate dsPhysicalExam = new DSAOETemplate();
                string currentPhysTempId = string.Empty;
                BLObject<DSAOETemplate> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                obj = BLLAOEObj.loadAOESystemRadiologyObservationForNotes(NotesId, AOETemplateSystemId);

                if (obj.Data != null)
                {
                    dsPhysicalExam = obj.Data;
                    DataView view = new DataView(dsPhysicalExam.AOENotesSystemRadiologyObservation);
                    DataTable AOEObservation = view.ToTable(true, "PEObservationId", "ObservationName", "PESystemId", "SystemName", "AOETemplateSystemId", "AOENotesRadiologyObservationId", "SystemDescription");
                    var response = new
                    {
                        status = true,

                        AOEObservation_JSON = js.Serialize(MDVUtility.JSON_DataTable(AOEObservation)),
                        AOEObservationCount = AOEObservation.Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        AOEObservation_JSON = "[]",
                        AOEObservationCount = 0,
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
        public string loadAOETemplates(long templateId)
        {
            try
            {
                DSAOETemplate dsAOETemplate = new DSAOETemplate();
                string currentPhysTempId = string.Empty;
                BLObject<DSAOETemplate> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                obj = BLLAOEObj.loadAOETempSysObservations(templateId);

                if (obj.Data != null)
                {
                    dsAOETemplate = obj.Data;
                    DataView view = new DataView(dsAOETemplate.AOETemplateSystemObservations);
                    DataTable AOETemp = view.ToTable(true, "AOETemplateId", "TemplateName", "SpecialtyIds", "ProviderIds", "CreatedOn", "CreatedBy", "ModifiedBy", "TemplatePreview",
                        "ModifiedOn", "CPTCode", "CPTCodeDescription", "LabId", "IsActive");
                    DataTable AOETempSystem = view.ToTable(true, "AOETemplateId", "TemplateName", "PESystemId", "SystemName", "IsSelectedSystem", "AOETemplateSystemId");
                    DataTable AOESysObservations = view.ToTable(true, "PESystemId", "SystemName", "PEObservationId", "ObservationName", "IsSelected");
                    var response = new
                    {
                        status = true,
                        AOETemplate_JSON = js.Serialize(MDVUtility.JSON_DataTable(AOETemp)),
                        AOETemplateCount = AOETemp.Rows.Count,
                        AOETemplateSystems_JSON = js.Serialize(MDVUtility.JSON_DataTable(AOETempSystem)),
                        AOETemplateSystemsCount = AOETempSystem.Rows.Count,
                        AOESysObservations_JSON = js.Serialize(MDVUtility.JSON_DataTable(AOESysObservations)),
                        AOESysObservationsCount = AOESysObservations.Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        AOETemplateSystems_JSON = "[]",
                        AOETemplateSystemsCount = 0,
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