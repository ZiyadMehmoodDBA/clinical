/* Author:  Muhammad Arshad
 * Created Date: 26/02/2016
 * OverView: Created to handel Physical Exam Template
 */

using MDVision.Business.BCommon;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.PhysicalExam;
using MDVision.IEHR.EMR.Helpers.Clinical.Templates;
using MDVision.IEHR.EMR.Model.PhysicalExam;
using MDVision.IEHR.EMR.Model.Templates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class PhysicalExamTemplateController : ApiController
    {
        [HttpPost]

        // Author:  Muhammad Arshad
        // Created Date: 24/02/2016
        //OverView: Entry point for Physical Exam Template

        public string PhysicalExamTemplate(JObject AllData)
        {
            string response = null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            PhysicalExamTemplateModel model = ser.Deserialize<PhysicalExamTemplateModel>(MDVUtility.ToStr(AllData["data"]));
            PhysicalExamTemplateHelper helperPhysicalExam = new PhysicalExamTemplateHelper();
            PhysicalExamHelper helperPhysicalExam1 = new PhysicalExamHelper();
            if (model.commandType == "Load_PhyscialExam_Template")
            {
                response = null;
                //response = helperPhysicalExam.loadPhysicalExamTemplate(MDVUtility.ToLong(model.TemplateId), MDVUtility.ToLong(model.EntityId));
                //response = helperPhysicalExam.loadPhysicalExamTemplateNew(MDVUtility.ToLong(model.TemplateId), MDVUtility.ToLong(model.EntityId), MDVUtility.ToInt32(model.IsActive));
                response = helperPhysicalExam.loadPhysicalExamTemplatesECW(MDVUtility.ToLong(model.TemplateId), MDVUtility.ToLong(model.EntityId), MDVUtility.ToInt32(model.IsActive));


            }
            if (model.commandType == "Load_PhyscialExam_Template_Section")
            {
                response = null;
                PhysExamSysTemplateModel modelSys = ser.Deserialize<PhysExamSysTemplateModel>(MDVUtility.ToStr(AllData["data"]));
                //response = helperPhysicalExam.loadPhysicalExamTemplate(MDVUtility.ToLong(model.TemplateId), MDVUtility.ToLong(model.EntityId));
                response = helperPhysicalExam.loadPhysicalExamTemplateSection(MDVUtility.ToLong(modelSys.TemplateId), MDVUtility.ToLong(modelSys.SystemId));


            }
            if (model.commandType == "Load_PhyscialExam_Template_Char")
            {
                response = null;
                PhysExamSecTemplateModel modelSec = ser.Deserialize<PhysExamSecTemplateModel>(MDVUtility.ToStr(AllData["data"]));
                //response = helperPhysicalExam.loadPhysicalExamTemplate(MDVUtility.ToLong(model.TemplateId), MDVUtility.ToLong(model.EntityId));
                response = helperPhysicalExam.loadPhysicalExamTemplateChar(MDVUtility.ToLong(modelSec.TemplateId), MDVUtility.ToLong(modelSec.SectionId));


            }
            if (model.commandType == "Load_PhyscialExam_Template_SubChar")
            {
                response = null;
                PhysExamCharTemplateModel modelChar = ser.Deserialize<PhysExamCharTemplateModel>(MDVUtility.ToStr(AllData["data"]));
                //response = helperPhysicalExam.loadPhysicalExamTemplate(MDVUtility.ToLong(model.TemplateId), MDVUtility.ToLong(model.EntityId));
                response = helperPhysicalExam.loadPhysicalExamTemplateSubChar(MDVUtility.ToLong(modelChar.TemplateId), MDVUtility.ToLong(modelChar.CharacteristicId));


            }
            else if (model.commandType == "Save_PhyscialExam_Template")
            {
                response = null;
                response = helperPhysicalExam.insertUpdatePhysicalExamTemplate(MDVUtility.ToLong(model.TemplateId), model);
            }
            else if (model.commandType == "Save_PhyscialExam_ECW")
            {
                response = null;

                PhysicalExamTemplateModelECW modelECW = ser.Deserialize<PhysicalExamTemplateModelECW>(MDVUtility.ToStr(AllData["data"]));

                response = helperPhysicalExam.insertUpdatePhysicalExamTemplateECW(MDVUtility.ToLong(modelECW.TemplateId), modelECW);
            }
            else if (model.commandType.ToLower() == "load_physcialexam_default_template")
            {
                response = null;
                response = helperPhysicalExam.fillPatientPhysicalExamTemplate(0, MDVUtility.ToLong(model.EntityId), model.ProviderIds, model.SpecialtyIds, MDVUtility.ToInt32(model.IsActive));
            }
            else if (model.commandType.ToLower() == "load_physcialexam_data_template")
            {
                response = null;
                response = helperPhysicalExam.fillPatientPhysicalExamDataTemplate(0, MDVUtility.ToLong(model.TemplateId), MDVUtility.ToLong(model.EntityId), model.ProviderIds, model.SpecialtyIds);
            }
            else if (model.commandType.ToLower() == "load_physcialexam_template_data")
            {
                response = null;
                response = helperPhysicalExam1.GetSerializedTemplateData(MDVUtility.ToLong(model.DataTemplateId));
            }

            else if (model.commandType == "Load_PhyscialExam_Template_Detail")
            {
                response = null;
                //response = helperPhysicalExam.loadPhysicalExamTemplate(MDVUtility.ToLong(model.TemplateId), 0);
                response = helperPhysicalExam.loadPhysicalExamTemplateNew(MDVUtility.ToLong(model.TemplateId), 0);
            }

            // MKMKMK

            else if (model.commandType == "Load_PhyscialExam_Templates_ECW")
            {
                response = null;
                response = helperPhysicalExam.loadPhysicalExamTemplatesECW(MDVUtility.ToLong(model.TemplateId), MDVUtility.ToLong(model.EntityId), 1, null);
            }
            else if (model.commandType == "Load_PhyscialExam_Systems")
            {
                response = null;
                response = helperPhysicalExam.loadPhysicalExamSystemsECW(1);
            }
            else if (model.commandType == "Load_PhyscialExam_System_Observations")
            {
                response = null;
                PhysExamSysTemplateModel modelSys = ser.Deserialize<PhysExamSysTemplateModel>(MDVUtility.ToStr(AllData["data"]));
                response = helperPhysicalExam.loadPhysicalExamSystemObservatiosECW(MDVUtility.ToLong(modelSys.TemplateId), MDVUtility.ToLong(modelSys.SystemId));
            }
            else if (model.commandType == "Load_PhyscialExam_System_Observations_Note")
            {
                response = null;
                PhysExamSysTemplateModel modelSys = ser.Deserialize<PhysExamSysTemplateModel>(MDVUtility.ToStr(AllData["data"]));
                response = helperPhysicalExam.loadPETempSystemObservationNote(MDVUtility.ToLong(modelSys.TemplateId), MDVUtility.ToLong(modelSys.SystemId), MDVUtility.ToLong(modelSys.NotesId));
            }
            else if (model.commandType == "Load_PhyscialExam_SpecialtyProvider")
            {
                //response = null;
                //response = helperPhysicalExam.GetSpecialtyProvider(MDVUtility.ToLong(model.SpecialtyIds));
            }

            else if (model.commandType == "DELETE_PhysicalExamTemplate")
            {
                response = null;
                //response = helperPhysicalExam.deletePhysicalExamTemplate(MDVUtility.ToLong(model.TemplateId));
                response = helperPhysicalExam.deletePETemplate(MDVUtility.ToLong(model.TemplateId));

            }
            else if (model.commandType == "UPDATE_PhysicalExamTemplate_ACTIVE_INACTIVE")
            {
                response = null;
                response = helperPhysicalExam.PETemplateIsActive(MDVUtility.ToLong(model.TemplateId), MDVUtility.ToLong(model.IsActive));

            }
            else if (model.commandType == "Load_PhyscialExam_TemplatesFill_ECW")
            {
                response = null;
                response = helperPhysicalExam.loadPhysicalExamTemplatesFillECW(MDVUtility.ToLong(model.TemplateId));
            }
            else if (model.commandType == "Load_All_PhyscialExam_ECW")
            {
                response = null;
                response = helperPhysicalExam.loadAllPhysicalExamECW(1);
            }
            else if (model.commandType == "RemoveTemplate_System")
            {
                response = null;
                PhysicalExamTemplateSystemsModel modeltemSys = ser.Deserialize<PhysicalExamTemplateSystemsModel>(MDVUtility.ToStr(AllData["data"]));
                response = helperPhysicalExam.deletePhysicalExamTemplateSystem(modeltemSys.PETemplateSystemId);
            }
            else if (model.commandType == "Save_PhysExam_NotesObservation")
            {
                response = null;
                response = helperPhysicalExam.savePhysicalExamNotesObservation(model.NotesObservationList, MDVUtility.ToLong(model.NotesId));
            }
            else if (model.commandType == "Detach_PE_From_Notes")
            {
                response = null;
                PhysicalExamTemplateSystemsModel modeltemSys = ser.Deserialize<PhysicalExamTemplateSystemsModel>(MDVUtility.ToStr(AllData["data"]));
                response = helperPhysicalExam.detachPhysicalExamTemplateFromNotes(modeltemSys.NotesId, modeltemSys.PETemplateId);
            }
            else if (model.commandType == "Fill_PE_For_Notes")
            {
                response = null;
                PhysicalExamTemplateSystemsModel modeltemSys = ser.Deserialize<PhysicalExamTemplateSystemsModel>(MDVUtility.ToStr(AllData["data"]));
                response = helperPhysicalExam.loadPETempSystemObservationForNotes(MDVUtility.ToLong(modeltemSys.NotesId));
            }
            else if (model.commandType == "Fill_PE_Observations_For_Notes")
            {
                response = null;
                PhysExamNotesObservationModelECW modeltemSys = ser.Deserialize<PhysExamNotesObservationModelECW>(MDVUtility.ToStr(AllData["data"]));
                response = helperPhysicalExam.loadPESystemObservationForNotes(MDVUtility.ToLong(modeltemSys.PETemplateSystemId), MDVUtility.ToLong(modeltemSys.NotesId));
            }
            else if (model.commandType == "Update_PE_Selected_Observations_Desc_For_Notes")
            {
                response = null;
                PhysExamNotesObservationModelECW modeltem = ser.Deserialize<PhysExamNotesObservationModelECW>(MDVUtility.ToStr(AllData["data"]));
                response = helperPhysicalExam.UpdatePENotesDescription(MDVUtility.ToLong(modeltem.PENotesObservationId), MDVUtility.ToStr(modeltem.Descr));
            }
            else if (model.commandType == "Insert_Observation_System_Assosication")
            {
                response = null;
                PhysExamTemplateSystemObservationsModelECW modeltem = ser.Deserialize<PhysExamTemplateSystemObservationsModelECW>(MDVUtility.ToStr(AllData["data"]));
                response = helperPhysicalExam.associatePEObservationAndSystem(MDVUtility.ToLong(modeltem.PESystemId), MDVUtility.ToLong(modeltem.ObservationId), MDVUtility.ToLong(modeltem.PETemplateSystemId));
            }
            else if (model.commandType == "Insert_Template_System_Assosication")
            {
                response = null;
                PhysExamTemplateSystemModelECW modeltem = ser.Deserialize<PhysExamTemplateSystemModelECW>(MDVUtility.ToStr(AllData["data"]));
                response = helperPhysicalExam.associatePESystemAndTemplate(MDVUtility.ToLong(modeltem.TemplateId), MDVUtility.ToLong(modeltem.PESystemId));
            }
            else if (model.commandType == "Load_PhyscialExam_ForProvider")
            {
                response = null;
                response = helperPhysicalExam.loadPhysicalExamForProvider(MDVUtility.ToInt64(model.ProviderId));
            }
            else if (model.commandType == "Save_PhyscialExam_For_Provider")
            {
                response = null;

                PhysicalExamTemplateModelECW modelECW = ser.Deserialize<PhysicalExamTemplateModelECW>(MDVUtility.ToStr(AllData["data"]));

                response = helperPhysicalExam.insertPhysicalExamTemplateForProvider( modelECW);
            }
            else if (model.commandType == "Load_PhyscialExam_For_SOAP_Note")
            {
                response = null;
                response = helperPhysicalExam.LoadPhyscialExamForSOAPNote(MDVUtility.ToLong(model.TemplateId));
            }
            /*
              Dictionary<string, dynamic> arrJSON = ser.DeserializeObject(MDVUtility.ToStr(AllData["data"])) as Dictionary<string, dynamic>;
              string characteristicIds = arrJSON.ContainsKey("characteristicIds") == true ? MDVUtility.ToStr(arrJSON["characteristicIds"]) : "";
              string subcharacteristicIds = arrJSON.ContainsKey("subcharacteristicIds") == true ? MDVUtility.ToStr(arrJSON["subcharacteristicIds"]) : "";
              string characteristicdata = arrJSON.ContainsKey("characteristicdata") == true ? MDVUtility.ToStr(arrJSON["characteristicdata"]) : "";
              Dictionary<string, dynamic> charcteristicsJSON = ser.DeserializeObject(characteristicdata) as Dictionary<string, dynamic>;
              string subcharacteristicdata = arrJSON.ContainsKey("subcharacteristicdata") == true ? MDVUtility.ToStr(arrJSON["subcharacteristicdata"]) : "";
              Dictionary<string, dynamic> subcharcteristicsJSON = ser.DeserializeObject(subcharacteristicdata) as Dictionary<string, dynamic>;

              PatientPhysicalExamModel patientPhysicalExamModel = ser.Deserialize<PatientPhysicalExamModel>(MDVUtility.ToStr(AllData["data"]));
              PatientPhysicalExamSystemModel patientPhysicalExamSystemModel = ser.Deserialize<PatientPhysicalExamSystemModel>(MDVUtility.ToStr(AllData["data"]));
              PhysicalExamHelper helperPhysicalExam = new PhysicalExamHelper();

              if (model.commandType.ToLower() == "update_sectionordersorting")
              {
                  helperPhysicalExam.savePhysicalExamUserSystem(model);
              }
              if (model.commandType.ToLower() == "fill_patientphysicalexam")
              {
                  response = null;
                  response = helperPhysicalExam.fillPatientPhysicalExam(patientPhysicalExamModel, Convert.ToInt64(patientPhysicalExamModel.PatientPhysicalExamId));
              }*/
            return response;
        }
    }
}