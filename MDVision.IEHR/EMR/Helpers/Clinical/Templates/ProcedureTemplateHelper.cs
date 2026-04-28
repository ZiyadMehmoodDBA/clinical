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
    public class ProcedureTemplateHelper
    {
        private BLLClinical BLLClinicalObj = null;
        private BLLProcedureTemplates BLLProcedureObj = null;
        public ProcedureTemplateHelper()
        {
            BLLClinicalObj = new BLLClinical();
            BLLProcedureObj = new BLLProcedureTemplates();
        }
        private static ProcedureTemplateHelper _instance = null;
        public static ProcedureTemplateHelper Instance()
        {
            if (_instance == null)
                _instance = new ProcedureTemplateHelper();
            return _instance;
        }

        public string insertUpdateProcedureTemplate(long TemplateId, ProcedureTemplatesModel modelPESystemObservations)
        {
            DSProcedureTemplate dsProcedureTemplate = new DSProcedureTemplate();
            try
            {
                string providerXMLString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetProviderXMLTable(modelPESystemObservations.ProviderIds).WriteXml(writer);
                    providerXMLString = writer.ToString();
                }

                string SystemObservationXMString = string.Empty;
                string ProceduresXMLString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetSystemObservationXMLTable(modelPESystemObservations).WriteXml(writer);
                    SystemObservationXMString = writer.ToString();
                }

                string ProceduesString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetProceduresXMLTable(modelPESystemObservations).WriteXml(writer);
                    ProceduresXMLString = writer.ToString();
                }

                string specialtyXMLString = string.Empty;
                using (TextWriter writer = new StringWriter())
                {
                    GetSpecialtyXMLTable(modelPESystemObservations.SpecialtyIds).WriteXml(writer);
                    specialtyXMLString = writer.ToString();
                }

                DSProcedureTemplate.ProcedureTemplateSystemObservationsRow dr = dsProcedureTemplate.ProcedureTemplateSystemObservations.NewProcedureTemplateSystemObservationsRow();
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
                dr.AssociatedWithIds = modelPESystemObservations.AssociatedWithIds;
                dr.CPTCode = modelPESystemObservations.CPTCode;
                dr.CPTCodeDescription = modelPESystemObservations.CPTCodeDescription;
                dr.ProviderXML = providerXMLString;
                dr.SpecialtyXML = specialtyXMLString;
                dr.ProceduresXML = ProceduresXMLString;
                dr.EntityId = MDVUtility.ToInt64(modelPESystemObservations.PhysicalExamTemplateEntity);
                dr.SystemObservationXML = SystemObservationXMString;
                dr.TemplatePreview = modelPESystemObservations.TemplatePreview.Trim();
                dr.ProcedureTemplateId = TemplateId.ToString();
                dr.NoteViewTypeId = modelPESystemObservations.NoteViewTypeId;

                dsProcedureTemplate.ProcedureTemplateSystemObservations.AddProcedureTemplateSystemObservationsRow(dr);
                BLObject<DSProcedureTemplate> obj = BLLProcedureObj.SaveProcedureTemplateSystemObservatios(dsProcedureTemplate);
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
        public string deleteProcedureTemplate(long templateId)
        {
            string currentPhysTempId = string.Empty;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            BLObject<string> obj = BLLProcedureObj.deleteProcedureTemplate(templateId);
            currentPhysTempId = obj.Data;
            if (string.IsNullOrEmpty(currentPhysTempId) || currentPhysTempId == "Successfully Deleted")
            {
                var response = new { status = true, message = "Procedure template deleted successfully." };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new { status = false, message = currentPhysTempId };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }
        public string associateProcedureObservationAndSystem(long PESystemId, long PEObservationId, long ProcedureTemplateSystemId)
        {
            try
            {
                DSProcedureTemplate dsPhysicalExam = new DSProcedureTemplate();

                DSProcedureTemplate.ProcedureSystemObservationRow dr = dsPhysicalExam.ProcedureSystemObservation.NewProcedureSystemObservationRow();
                dr.PESystemId = PESystemId;
                dr.PEObservationId = PEObservationId;
                dr.ProcedureTemplateSystemId = ProcedureTemplateSystemId;
                dsPhysicalExam.ProcedureSystemObservation.AddProcedureSystemObservationRow(dr);


                BLObject<DSProcedureTemplate> objObservation = BLLProcedureObj.insertProcedureSystemObservation(dsPhysicalExam);
                if (objObservation.Data != null)
                {
                    DSProcedureTemplate dsAssociatedObservation = objObservation.Data;
                    var response = new
                    {
                        Status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        ProcedureObservation_JSON = MDVUtility.JSON_DataTable(dsAssociatedObservation.Tables[dsAssociatedObservation.ProcedureSystemObservation.TableName]),
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
        public string loadProcedureTemplates(ProcedureTemplatesModel model)
        {
            List<ProcedureTemplatesModel> obj = BLLProcedureObj.LoadProcedureTemplates(model);
            if (obj != null)
            {
                var response = new
                {
                    status = true,
                    ProcedureTemplates = obj
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
        DataTable GetProceduresXMLTable(ProcedureTemplatesModel model)
        {
            DataTable CPTs = new DataTable() { TableName = "ProceduesPart" };

            CPTs.Columns.Add("CPTCode", typeof(string));
            CPTs.Columns.Add("CPTCodeDescription", typeof(string));

                for (int i = 0; i < model.procedures.Count; i++)
                {
                    CPTs.Rows.Add(
                        model.procedures[i].CPTCode,
                        model.procedures[i].CPTCodeDescription
                        );
                }
            
            return CPTs;
        }
        DataTable GetSystemObservationXMLTable(ProcedureTemplatesModel modelPESystemObservations)
        {
            DataTable SystemObservationTable = new DataTable() { TableName = "PETempPart" };

            SystemObservationTable.Columns.Add("PETemplateId", typeof(string));
            SystemObservationTable.Columns.Add("PESystemId", typeof(int));
            SystemObservationTable.Columns.Add("PEObservationId", typeof(int));
            SystemObservationTable.Columns.Add("PESystemIsSelected", typeof(int));
            SystemObservationTable.Columns.Add("PEObservationIsSelected", typeof(int));
            SystemObservationTable.Columns.Add("IsSystemDeleted", typeof(int));
            SystemObservationTable.Columns.Add("IsObservationDeleted", typeof(int));

            if (MDVUtility.ToInt64(modelPESystemObservations.ProcedureTemplateId) != -1)
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

        public string ProcedureTemplateTests(ProcedureTemplatesModel model)
        {
            ProcedureTemplatesModel result = BLLProcedureObj.LoadProcedureTemplateTests(model);
            if (result == null)
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
                    Procedures = result.procedures
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }
        public string ActiveInactiveProcedureTemplate(ProcedureTemplatesModel model)
        {
            string result = BLLProcedureObj.ActiveInActiveProcedureTemplate(model);
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

        public string UpdateProcedureNotesDescription(long ProcedureNotesObservationId, string Desr)
        {
            try
            {
                BLObject<string> objInsertedNormalSystem = BLLProcedureObj.updateProcedureNotesDescription(ProcedureNotesObservationId, Desr);
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

        public string UpdateProcedureOrderNotesDescription(long ProcedureOrderNotesObservationId, string Desr)
        {
            try
            {
                BLObject<string> objInsertedNormalSystem = BLLProcedureObj.updateProcedureOrderNotesDescription(ProcedureOrderNotesObservationId, Desr);
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
        public string loadProcedureSystemObservationForNotes(long ProcedureTemplateSystemId, long NotesId, int ProcedureId)
        {
            try
            {
                DSProcedureTemplate dsPhysicalExam = new DSProcedureTemplate();
                string currentPhysTempId = string.Empty;
                BLObject<DSProcedureTemplate> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                obj = BLLProcedureObj.loadProcedureSystemObservationForNotes(NotesId, ProcedureTemplateSystemId, ProcedureId);

                if (obj.Data != null)
                {
                    dsPhysicalExam = obj.Data;
                    DataView view = new DataView(dsPhysicalExam.ProcedureNotesSystemObservation);
                    DataTable ProcedureObservation = view.ToTable(true, "PEObservationId", "ObservationName", "PESystemId", "SystemName", "ProcedureTemplateSystemId", "ProcedureNotesObservationId", "SystemDescription");
                    var response = new
                    {
                        status = true,

                        ProcedureObservation_JSON = js.Serialize(MDVUtility.JSON_DataTable(ProcedureObservation)),
                        ProcedureObservationCount = ProcedureObservation.Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ProcedureObservation_JSON = "[]",
                        ProcedureObservationCount = 0,
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
        public string GetProcedureTemplateSoapText(int ProcedureId, long NotesId)
        {
            string TemplateSoapText = "";
            TemplateSoapText = BLLProcedureObj.GetProcedureTemplateSoapText(ProcedureId, NotesId);
            if (TemplateSoapText != null && TemplateSoapText != "")
            {
                var response = new
                {
                    status = true,
                    ProcedureTemplateSoapText = TemplateSoapText
                    
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else {
                  var response = new
                   {
                    status = false,
                    Message = "An Error Occured"
                   };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string loadProcedureOrderSystemObservationForNotes(long ProcedureTemplateSystemId, long NotesId, long ProcedureOrderId)
        {
            try
            {
                DSProcedureTemplate dsPhysicalExam = new DSProcedureTemplate();
                string currentPhysTempId = string.Empty;
                BLObject<DSProcedureTemplate> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                obj = BLLProcedureObj.loadProcedureOrderSystemObservationForNotes(NotesId, ProcedureTemplateSystemId, ProcedureOrderId);

                if (obj.Data != null)
                {
                    dsPhysicalExam = obj.Data;
                    DataView view = new DataView(dsPhysicalExam.ProcedureNotesSystemObservation);
                    DataTable ProcedureObservation = view.ToTable(true, "PEObservationId", "ObservationName", "PESystemId", "SystemName", "ProcedureTemplateSystemId", "ProcedureOrderNotesObservationId", "SystemDescription");
                    var response = new
                    {
                        status = true,

                        ProcedureOrderObservation_JSON = js.Serialize(MDVUtility.JSON_DataTable(ProcedureObservation)),
                        ProcedureOrderObservationCount = ProcedureObservation.Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ProcedureObservation_JSON = "[]",
                        ProcedureObservationCount = 0,
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

        public string loadProcedureSystemRadiologyObservationForNotes(long ProcedureTemplateSystemId, long NotesId)
        {
            try
            {
                DSProcedureTemplate dsPhysicalExam = new DSProcedureTemplate();
                string currentPhysTempId = string.Empty;
                BLObject<DSProcedureTemplate> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                obj = BLLProcedureObj.loadProcedureSystemRadiologyObservationForNotes(NotesId, ProcedureTemplateSystemId);

                if (obj.Data != null)
                {
                    dsPhysicalExam = obj.Data;
                    DataView view = new DataView(dsPhysicalExam.ProcedureNotesSystemObservation);
                    DataTable ProcedureObservation = view.ToTable(true, "PEObservationId", "ObservationName", "PESystemId", "SystemName", "ProcedureTemplateSystemId", "ProcedureNotesRadiologyObservationId", "SystemDescription");
                    var response = new
                    {
                        status = true,

                        ProcedureObservation_JSON = js.Serialize(MDVUtility.JSON_DataTable(ProcedureObservation)),
                        ProcedureObservationCount = ProcedureObservation.Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ProcedureObservation_JSON = "[]",
                        ProcedureObservationCount = 0,
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
        public string loadProcedureTemplates(long templateId)
        {
            try
            {
                DSProcedureTemplate dsProcedureTemplate = new DSProcedureTemplate();
                string currentPhysTempId = string.Empty;
                BLObject<DSProcedureTemplate> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                obj = BLLProcedureObj.loadProcedureTempSysObservations(templateId);

                if (obj.Data != null)
                {
                    dsProcedureTemplate = obj.Data;
                    DataView view = new DataView(dsProcedureTemplate.ProcedureTemplateSystemObservations);
                    DataTable ProcedureTemp = view.ToTable(true, "ProcedureTemplateId", "TemplateName",  "ProviderIds", "CreatedOn", "CreatedBy", "ModifiedBy", "TemplatePreview",
                        "ModifiedOn", "CPTCode", "CPTCodeDescription", "IsActive", "AssociatedWithIds");
                    DataTable ProcedureTempSystem = view.ToTable(true, "ProcedureTemplateId", "TemplateName", "PESystemId", "SystemName", "IsSelectedSystem", "ProcedureTemplateSystemId");
                    DataTable ProcedureSysObservations = view.ToTable(true, "PESystemId", "SystemName", "PEObservationId", "ObservationName", "IsSelected");
                    var response = new
                    {
                        status = true,
                        ProcedureTemplate_JSON = js.Serialize(MDVUtility.JSON_DataTable(ProcedureTemp)),
                        ProcedureTemplateCount = ProcedureTemp.Rows.Count,
                        ProcedureTemplateSystems_JSON = js.Serialize(MDVUtility.JSON_DataTable(ProcedureTempSystem)),
                        ProcedureTemplateSystemsCount = ProcedureTempSystem.Rows.Count,
                        ProcedureSysObservations_JSON = js.Serialize(MDVUtility.JSON_DataTable(ProcedureSysObservations)),
                        ProcedureSysObservationsCount = ProcedureSysObservations.Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ProcedureTemplateSystems_JSON = "[]",
                        ProcedureTemplateSystemsCount = 0,
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