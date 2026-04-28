
/* Author:  Muhammad Arshad
 * Created Date: 04/02/2016
 * OverView: Created to handel Physical Exam
 */

using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.IEHR.EMR.Model.PhysicalExam;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Text;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Common.Logging;

namespace MDVision.IEHR.EMR.Helpers.Clinical.PhysicalExam
{
    public class PhysicalExamHelper
    {

        private BLLClinical BLLClinicalObj = null;
        public PhysicalExamHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }

        private const string Characteristic = "characteristic";
        private const string SubCharacteristic = "SubCharacteristic";
        private static PhysicalExamHelper _instance = null;
        public static PhysicalExamHelper Instance()
        {
            if (_instance == null)
                _instance = new PhysicalExamHelper();
            return _instance;
        }

        #region Patient Physical Exam Fill, Save and Update Methods

        // Author: Abid Ali
        // Created Date: 08/02/2016
        //OverView: This function will handle fill of Patient Physical Exam
        public string fillPatientPhysicalExam(PatientPhysicalExamModel model, Int64 patientPhysicalExamId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)) && patientPhysicalExamId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = "",
                        data = new PatientPhysicalSystem()
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSPhysicalExam dsPhysicalExam = null;
                    //if (patientPhysicalExamId > 0)
                    //{
                    //    UpdateSoapTextForPatientPhysicalExam(0, 0, 0, 0, patientPhysicalExamId);
                    //}
                    BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExam(MDVUtility.ToInt64(model.PatientId), patientPhysicalExamId, MDVUtility.ToInt64(model.NotesId));
                    dsPhysicalExam = obj.Data;
                    if (dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows[0];
                        var physicalExamKeyValues = new Dictionary<string, string>
                        {
                            { "physicalExamId",  MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamIdColumn.ColumnName])},
                            { "PatientPhysicalExamDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamDateColumn.ColumnName]).ToShortDateString()},
                            { "physicalExamIsActive", MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.IsActiveColumn.ColumnName])},
                            { "bNormal", MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.bNormalExamColumn.ColumnName])},
                            { "physicalExamCreatedBy", MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.CreatedByColumn.ColumnName])},
                            { "physicalExamCreatedOn", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.CreatedOnColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsPhysicalExam.PatientPhysicalExam.CreatedOnColumn.ColumnName]).ToShortDateString()},
                            { "physicalExamModifiedBy", MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.ModifiedByColumn.ColumnName])},
                            { "physicalExamModifiedOn", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.ModifiedOnColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsPhysicalExam.PatientPhysicalExam.ModifiedOnColumn.ColumnName]).ToShortDateString()},
                            { "Comments", MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.CommentsColumn.ColumnName])},
                            { "physicalExamSoapText", MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.SoapTextColumn.ColumnName])},
                            { "NoteId", MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.NoteIdColumn.ColumnName])},
                            { "TemplateId", MDVUtility.ToStr(dr[dsPhysicalExam.PatientPhysicalExam.TemplateIdColumn.ColumnName])},
                        };
                        //retrieve systems data
                        patientPhysicalExamId = MDVUtility.ToInt64(dr[dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamIdColumn.ColumnName]);
                        bool bNormal = Convert.ToBoolean(dr[dsPhysicalExam.PatientPhysicalExam.bNormalExamColumn.ColumnName]);
                        if (bNormal)
                        {
                            DSPhysicalExam dsPhysicalExamSystem = null;
                            //retrieve all records
                            BLObject<DSPhysicalExam> ob = BLLClinicalObj.LoadPatientPhysicalExamSystem(patientPhysicalExamId, 0);
                            dsPhysicalExamSystem = ob.Data;
                            if (dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName].Rows.Count > 0)
                            {
                                DSPhysicalExam.PatientPhysicalExamSystemRow[] arrToComponentRows = (DSPhysicalExam.PatientPhysicalExamSystemRow[])dsPhysicalExamSystem.PatientPhysicalExamSystem.Select(MDVUtility.ToStr(dsPhysicalExamSystem.PatientPhysicalExamSystem.PatientPhysicalExamIdColumn.ColumnName) + "=" + MDVUtility.ToStr(patientPhysicalExamId) + "");
                                DataRow drPhysicalExamSystem = (DataRow)arrToComponentRows[0];
                                physicalExamKeyValues.Add("NormalExamsDetail", MDVUtility.ToStr(drPhysicalExamSystem[dsPhysicalExam.PatientPhysicalExamSystem.NormalCommentsColumn.ColumnName]));
                            }
                        }
                        string patientSystemsSerilized = string.Empty;
                        if (patientPhysicalExamId > 0)
                        {
                            PatientPhysicalSystem patientSystem = new PatientPhysicalSystem();
                            //patientSystemsSerilized = patientSystem.GetSerializedSystemsAndChildsData(patientPhysicalExamId);

                            patientSystemsSerilized = patientSystem.GetSerializedData(patientPhysicalExamId);
                        }
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            PatientPhysicalExamFill_JSON = js.Serialize(physicalExamKeyValues),
                            patientPhysicalExamSystemsFill_JSON = patientSystemsSerilized,
                            PatientPhysicalExamLoad_JSON = MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName]),
                            PatientPhysicalExamSystemLoad_JSON = MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamSystem.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientPhysicalExamFill_JSON = "[]",
                            patientPhysicalExamSystemsFill_JSON = "[]",
                            PatientPhysicalExamLoad_JSON = "[]",
                            PatientPhysicalExamSystemLoad_JSON = "[]",
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

        // Author: Abid Ali
        // Created Date: 08/02/2016
        //OverView: This function will insert Patient Physical Exam record
        public string savePatientPhysicalExam(PatientPhysicalExamModel model, List<object> lstCharacteristicModel, List<object> lstSubCharacteristicModel, List<object> lstCharacteristicDetailModel, List<object> lstSubCharacteristicDetailModel, PatientPhysicalExamSystemModel systemModel = null, long templateId = 0)
        {
            try
            {

                DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();

                DSPhysicalExam.PatientPhysicalExamRow dr = dsPhysicalExam.PatientPhysicalExam.NewPatientPhysicalExamRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.PatientPhysicalExamDate))
                {
                    dr.PatientPhysicalExamDate = MDVUtility.ToDateTime(model.PatientPhysicalExamDate);
                }
                else
                {
                    dr[dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.Comments))
                {
                    dr.Comments = MDVUtility.ToStr(model.Comments);
                }
                else
                {
                    dr[dsPhysicalExam.PatientPhysicalExam.CommentsColumn] = DBNull.Value;
                }
                if (model.NotesId == -1)
                {
                    dr[dsPhysicalExam.PatientPhysicalExam.NoteIdColumn] = DBNull.Value;

                }
                else
                {
                    dr.NoteId = model.NotesId;

                }
                if (model.TemplateId == -1)
                {
                    dr[dsPhysicalExam.PatientPhysicalExam.TemplateIdColumn] = DBNull.Value;

                }
                else
                {
                    dr.TemplateId = model.TemplateId;

                }


                dr.bNormalExam = model.bNormal.Value;
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsPhysicalExam.PatientPhysicalExam.AddPatientPhysicalExamRow(dr);
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.InsertPatientPhysicalExam(dsPhysicalExam, model.NormalExamsDetail);
                dsPhysicalExam = obj.Data;

                if (obj.Data != null)
                {
                    var systemId = "";
                    Int64 PhysicalExamId = MDVUtility.ToInt64(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows[0][dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamIdColumn.ColumnName]);
                    //Start 10-02-2016 Humaira Yousaf Modified to inserts/updates normal systems                    
                    if (PhysicalExamId > 0)
                    {
                        if (model.NormalSystemIds != null && model.NormalSystemIds.Count > 0)
                        {
                            string responseNormalSystem = InsertUpdatePatientPhysicalExamSystem(PhysicalExamId, model.NormalSystemIds, true);
                            systemId = responseNormalSystem;


                        }
                        //Start//18-02-2016//Ahmad Raza//Condition to insert update Normal system's inner detail
                        else if (systemModel != null && systemModel.PhysicalExamSystemId != null)
                        {
                            string responseNormalSystem = InsertUpdatePatientPhysicalExamSystem(PhysicalExamId, null, true, systemModel);
                            systemId = responseNormalSystem;
                        }
                        //End//18-02-2016//Ahmad Raza//Condition to insert update Normal system's inner detail
                        if (model.NormalSystemIds == null || model.NormalSystemIds.Count == 0)
                        {
                            string responseNormalSystem = InsertUpdatePatientPhysicalExamSystem(PhysicalExamId, null, false, null);
                            systemId = responseNormalSystem;
                        }

                        #region Characteristic Handling

                        //Start 15-02-2015 Humaira Yousaf Modified to insert update Characteristic and Sub Characteristic
                        //Insert/Update Characteristic     SAVE
                        if (lstCharacteristicModel != null && lstCharacteristicModel.Count > 0)
                        {
                            List<PatientPhysicalExamCharacteristicModel> lstCharacterModel = lstCharacteristicModel.OfType<PatientPhysicalExamCharacteristicModel>().ToList();
                            foreach (PatientPhysicalExamCharacteristicModel currentCharacteristicModel in lstCharacterModel)
                            {
                                List<int> lstCharacteristicId = new List<int>();
                                string sectionId = "";
                                lstCharacteristicId.Add(MDVUtility.ToInt(currentCharacteristicModel.SystemId));
                                string PatPhysExamSysId = InsertUpdatePatientPhysicalExamSystem(PhysicalExamId, lstCharacteristicId, false);
                                if (PatPhysExamSysId != "" && MDVUtility.ToInt64(PatPhysExamSysId) > 0)
                                {
                                    List<Int64> lstSectionId = new List<Int64>();
                                    lstSectionId.Add(MDVUtility.ToInt64(currentCharacteristicModel.SectionId));
                                    sectionId = insertUpdatePatientPhysicalExamSystemSection(MDVUtility.ToInt64(PatPhysExamSysId), lstSectionId);
                                    Int64 currentPatPhysExamSysSectionId = MDVUtility.ToInt64(sectionId);
                                    if (currentPatPhysExamSysSectionId > 0)
                                    {
                                        PatientPhysicalExamCharacteristicDetailModel pdetail =
                                             lstCharacteristicDetailModel != null && lstCharacteristicDetailModel.Count > 0 ?
                                            lstCharacteristicDetailModel.OfType<PatientPhysicalExamCharacteristicDetailModel>().ToList().Where(n => n.CharacteristicId == currentCharacteristicModel.CharacteristicId).FirstOrDefault()
                                            : new PatientPhysicalExamCharacteristicDetailModel();
                                        string sectionCharId = insertUpdatePatientPhysicalExamSystemSectionCharecteristics(currentPatPhysExamSysSectionId, currentCharacteristicModel, pdetail, lstCharacteristicDetailModel, PhysicalExamId);
                                        //Start 23-02-2016 Muhammad Arshad Update SoapText for inserted Data
                                        Int64 currentPatPhysExamSysSectionCharId = MDVUtility.ToInt64(sectionCharId);
                                        if (currentPatPhysExamSysSectionCharId > 0)
                                        {
                                            BLObject<DSPhysicalExam> objPhysExamChar = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamCharacteristics(currentPatPhysExamSysSectionCharId);
                                            if (objPhysExamChar.Data != null)
                                            {

                                            }
                                        }
                                        BLObject<DSPhysicalExam> objPhysExamSection = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSection(currentPatPhysExamSysSectionId);
                                        if (objPhysExamSection.Data != null)
                                        {

                                        }
                                        //End 23-02-2016 Muhammad Arshad Update SoapText for inserted Data
                                    }
                                    BLObject<DSPhysicalExam> objPhysExamSystem = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSystem(MDVUtility.ToInt64(PatPhysExamSysId));
                                    if (objPhysExamSystem.Data != null)
                                    {

                                    }
                                }
                            }
                        }

                        #endregion

                        #region Sub-Characteristics Handling
                        //Insert/Update SubCharacteristic       SAVE
                        if (lstSubCharacteristicModel != null && lstSubCharacteristicModel.Count > 0)
                        {
                            List<PatientPhysicalExamSubCharacteristicModel> lstSubCharacterModel = lstSubCharacteristicModel.OfType<PatientPhysicalExamSubCharacteristicModel>().ToList();
                            foreach (PatientPhysicalExamSubCharacteristicModel currentSubCharacteristicModel in lstSubCharacterModel)
                            {
                                List<int> lstSubCharacteristicId = new List<int>();
                                lstSubCharacteristicId.Add(MDVUtility.ToInt(currentSubCharacteristicModel.SystemId));
                                string PatPhysExamSysId = InsertUpdatePatientPhysicalExamSystem(PhysicalExamId, lstSubCharacteristicId, false);
                                if (PatPhysExamSysId != "" && MDVUtility.ToInt64(PatPhysExamSysId) > 0)
                                {
                                    List<Int64> lstSectionId = new List<Int64>();
                                    lstSectionId.Add(MDVUtility.ToInt64(currentSubCharacteristicModel.SectionId));
                                    string sectionId = insertUpdatePatientPhysicalExamSystemSection(MDVUtility.ToInt64(PatPhysExamSysId), lstSectionId);

                                    if (sectionId != "" && MDVUtility.ToInt64(sectionId) > 0)
                                    {
                                        PatientPhysicalExamCharacteristicDetailModel pdetail = lstCharacteristicDetailModel != null && lstCharacteristicDetailModel.Count > 0 ?
                                            lstCharacteristicDetailModel.OfType<PatientPhysicalExamCharacteristicDetailModel>().ToList().Where(n => n.CharacteristicId == currentSubCharacteristicModel.CharacteristicId).FirstOrDefault()
                                            : new PatientPhysicalExamCharacteristicDetailModel();
                                        string sectionCharId = insertUpdatePatientPhysicalExamSystemSectionCharecteristics(MDVUtility.ToInt64(sectionId), new PatientPhysicalExamCharacteristicModel { CharacteristicId = currentSubCharacteristicModel.CharacteristicId }, pdetail, lstCharacteristicDetailModel, PhysicalExamId);

                                        if (sectionCharId != "" && MDVUtility.ToInt64(sectionCharId) > 0)
                                        {
                                            string sectionCharSubCharId = insertUpdatePatientPhysicalExamSystemSectionCharecteristicsSubCharecteristics(MDVUtility.ToInt64(sectionCharId), currentSubCharacteristicModel, lstSubCharacteristicDetailModel, PhysicalExamId);
                                            //Start 23-02-2016 Muhammad Arshad Update SoapText for inserted Data
                                            if (sectionCharSubCharId != "" && MDVUtility.ToInt64(sectionCharSubCharId) > 0)
                                            {
                                                BLObject<DSPhysicalExam> objPhysExamSubChar = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSubCharacteristics(MDVUtility.ToInt64(sectionCharSubCharId));
                                                if (objPhysExamSubChar.Data != null)
                                                {
                                                }
                                            }
                                            BLObject<DSPhysicalExam> objPhysExamChar = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamCharacteristics(MDVUtility.ToInt64(sectionCharId));
                                            if (objPhysExamChar.Data != null)
                                            {

                                            }
                                            //End 23-02-2016 Muhammad Arshad Update SoapText for inserted Data
                                        }

                                        BLObject<DSPhysicalExam> objPhysExamSection = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSection(MDVUtility.ToInt64(sectionId));
                                        if (objPhysExamSection.Data != null)
                                        {

                                        }

                                    }
                                    BLObject<DSPhysicalExam> objPhysExamSystem = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSystem(MDVUtility.ToInt64(PatPhysExamSysId));
                                    if (objPhysExamSystem.Data != null)
                                    {

                                    }
                                }

                            }
                        }
                        #endregion

                        BLObject<DSPhysicalExam> objPhysExam = BLLClinicalObj.insertUpdateSoapTextForPhysicalExam(PhysicalExamId, templateId);
                        if (objPhysExam.Data != null)
                        {

                        }
                    }
                    //End
                    //End 15-02-2016 Humaira Yousaf

                    // BLObject<string> objValue = BLLClinicalObj.updateSoapTextForSurgicalHX(SurgicalHxId);
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        PhysicalExamId = MDVUtility.ToInt64(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows[0][dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamIdColumn.ColumnName]),
                        NoteId = MDVUtility.ToInt64(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows[0][dsPhysicalExam.PatientPhysicalExam.NoteIdColumn.ColumnName]),
                        // diseaseId = diseaseId
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

        internal string toggle_PhysicalExamCharacteristics(long PrimaryKeyId, bool IsPositive, string type)
        {
            if (type.Equals("PatientPhysicalExamSystemSectionCharacteristic"))
            {


                //throw new NotImplementedException();
                DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();

                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristic(0, PrimaryKeyId);
                dsPhysicalExam = obj.Data;
                string CurrentPatPhysExamSysCharId = string.Empty;

                foreach (DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow RowSectionChar in dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.Rows)
                {
                    RowSectionChar.IsPositive = IsPositive ? false : true;
                    RowSectionChar.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowSectionChar.ModifiedOn = DateTime.Now;
                }

                #region Database Insertion/Updation

                BLObject<DSPhysicalExam> objInsertedSystemSection = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystemSectionCharacteristic(dsPhysicalExam);
                if (objInsertedSystemSection.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        physicalExamSystemSectionCharId = CurrentPatPhysExamSysCharId != string.Empty ? CurrentPatPhysExamSysCharId : "0",
                    };
                    return response.physicalExamSystemSectionCharId.ToString();//(Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objInsertedSystemSection.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }

                #endregion
            }
            else if (type.Equals("PhysicalExamSystemSectionCharacteristicSubCharacteristic"))
            {
                DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
                string CurrentPatPhysExamSysCharSubCharId = string.Empty;
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(0, PrimaryKeyId);
                dsPhysicalExam = obj.Data;

                foreach (DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow RowSectionChar in dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.Rows)
                {
                    RowSectionChar.IsPositive = IsPositive ? false : true;
                    RowSectionChar.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowSectionChar.ModifiedOn = DateTime.Now;
                }


                #region Database Insertion/Updation

                BLObject<DSPhysicalExam> objInsertedSystemSection = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(dsPhysicalExam);
                if (objInsertedSystemSection.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objInsertedSystemSection.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }

                #endregion
            }
            var responseError = new
            {
                status = false,
                Message = "Error Message"
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(responseError);
        }

        // Author: Abid Ali
        // Created Date: 09/02/2016
        //OverView: This function will update Patient Physical Exam record
        public string updatePatientPhysicalExam(PatientPhysicalExamModel model, Int64 physicalExamId, List<object> lstCharacteristicModel, List<object> lstSubCharacteristicModel, List<object> lstCharacteristicDetailModel, List<object> lstSubCharacteristicDetailModel, PatientPhysicalExamSystemModel systemModel, long templateId = 0)
        {
            try
            {
                if (physicalExamId > 0)
                {

                    DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
                    BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExam(MDVUtility.ToInt64(model.PatientId), physicalExamId, MDVUtility.ToInt64(model.NotesId));
                    dsPhysicalExam = obj.Data;
                    foreach (DSPhysicalExam.PatientPhysicalExamRow dr in dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows)
                    {
                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                        if (!string.IsNullOrEmpty(model.PatientPhysicalExamDate))
                        {
                            dr.PatientPhysicalExamDate = MDVUtility.ToDateTime(model.PatientPhysicalExamDate);
                        }
                        else
                        {
                            dr[dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.Comments))
                        {
                            dr.Comments = MDVUtility.ToStr(model.Comments);
                        }
                        else
                        {
                            dr[dsPhysicalExam.PatientPhysicalExam.CommentsColumn] = DBNull.Value;
                        }
                        dr.NoteId = model.NotesId;
                        dr.TemplateId = model.TemplateId;
                        dr.bNormalExam = model.bNormal.Value;
                        //  dr.IsActive = true;
                        //  dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //  dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    //var diseaseID = "";
                    //if (lstDiseaseObject.Count > 0)
                    //{
                    //    string responseMedicalHxDisease = insertUpdateDisease(MedicalHxId, lstDiseaseObject);
                    //    diseaseID = responseMedicalHxDisease;
                    //}
                    //Start 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                    //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MedicalHxId);

                    //End 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                    #region Database Updation
                    if (dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows.Count > 0)
                    {
                        BLObject<DSPhysicalExam> objUpdate = BLLClinicalObj.UpdatePatientPhysicalExam(dsPhysicalExam, model.NormalExamsDetail);

                        //Start 10-02-2016 Humaira Yousaf Modified to inserts/updates normal systems
                        var systemId = "";
                        Int64 PhysicalExamId = MDVUtility.ToInt64(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows[0][dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamIdColumn.ColumnName]);
                        Int64 NoteId = MDVUtility.ToInt64(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows[0][dsPhysicalExam.PatientPhysicalExam.NoteIdColumn.ColumnName]);
                        if (PhysicalExamId > 0)
                        {
                            DSPhysicalExam dsPhysicalExam2 = new DSPhysicalExam();
                            string currentPatPhysSysId = string.Empty;
                            BLObject<DSPhysicalExam> objExam2 = BLLClinicalObj.LoadPatientPhysicalExamSystem(PhysicalExamId, 0);
                            dsPhysicalExam2 = objExam2.Data;
                            foreach (DataRow drTobeDeleted in dsPhysicalExam2.PatientPhysicalExamSystem.Rows)
                            {
                                if (systemModel != null && systemModel.PhysicalExamSystemId != null)
                                {
                                    //DSPhysicalExam.PatientPhysicalExamSystemRow[] arrCurrSystem = (DSPhysicalExam.PatientPhysicalExamSystemRow[])dsPhysicalExam2.PatientPhysicalExamSystem.Select(dsPhysicalExam2.PatientPhysicalExamSystem.SystemIdColumn.ColumnName + "=" + systemModel.PhysicalExamSystemId);
                                    //if (arrCurrSystem.Length > 0)
                                    //{
                                    //    Int64 currentPhysExamSysId = MDVUtility.ToInt64(arrCurrSystem[0][dsPhysicalExam2.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]);
                                    //    systemModel.SystemId = systemModel.PhysicalExamSystemId;
                                    //    systemModel.PhysicalExamSystemId = MDVUtility.ToStr(currentPhysExamSysId);
                                    //    string response = deletePatientPhysicalExamSystem(PhysicalExamId, MDVUtility.ToInt64(systemModel.PhysicalExamSystemId));
                                    //    systemModel.PhysicalExamSystemId = systemModel.SystemId;
                                    //    break;
                                    //}

                                }
                                else
                                {
                                    if (model.NormalSystemIds != null && model.NormalSystemIds.Count > 0)
                                    {
                                        if (model.NormalSystemIds.IndexOf(MDVUtility.ToInt32(drTobeDeleted[dsPhysicalExam.PatientPhysicalExamSystem.SystemIdColumn.ColumnName])) < 0)
                                        {
                                            string response = deletePatientPhysicalExamSystem(PhysicalExamId, MDVUtility.ToInt64(drTobeDeleted[dsPhysicalExam.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]));
                                        }
                                    }
                                    else
                                    {
                                        string response = deletePatientPhysicalExamSystem(PhysicalExamId, MDVUtility.ToInt64(drTobeDeleted[dsPhysicalExam.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]));
                                    }

                                }

                            }

                            if (model.NormalSystemIds != null && model.NormalSystemIds.Count > 0)
                            {
                                string responseNormalSystem = InsertUpdatePatientPhysicalExamSystem(PhysicalExamId, model.NormalSystemIds, true, physicalExamModelObj: model);
                                systemId = responseNormalSystem;
                            }
                            //Start//18-02-2016//Ahmad Raza//Condition to insert update Normal system's inner detail
                            else if (systemModel != null && systemModel.PhysicalExamSystemId != null)
                            {
                                string responseNormalSystem = InsertUpdatePatientPhysicalExamSystem(PhysicalExamId, null, true, systemModel);
                                systemId = responseNormalSystem;
                            }
                            //End//18-02-2016//Ahmad Raza//Condition to insert update Normal system's inner detail

                            if ((model.NormalSystemIds == null || model.NormalSystemIds.Count == 0) && (!model.isFromNormalComments.Value && systemModel == null))
                            {
                                string responseNormalSystem = InsertUpdatePatientPhysicalExamSystem(PhysicalExamId, null, false, null, false);
                                systemId = responseNormalSystem;
                            }

                            #region Characteristic Handling while updating Patient PhysicalExam

                            //Start 15-02-2016 Humaira Yousaf Modified to insert update Characteristic and Sub Characteristic
                            //Insert/Update Characteristic
                            if (lstCharacteristicModel != null && lstCharacteristicModel.Count > 0)
                            {
                                List<PatientPhysicalExamCharacteristicModel> lstCharacterModel = lstCharacteristicModel.OfType<PatientPhysicalExamCharacteristicModel>().ToList();
                                foreach (PatientPhysicalExamCharacteristicModel currentCharacteristicModel in lstCharacterModel)
                                {
                                    List<int> lstCharacteristicId = new List<int>();
                                    string sectionId = "";
                                    lstCharacteristicId.Add(MDVUtility.ToInt(currentCharacteristicModel.SystemId));
                                    string PatPhysExamSysId = InsertUpdatePatientPhysicalExamSystem(PhysicalExamId, lstCharacteristicId, false);
                                    if (PatPhysExamSysId != "" && MDVUtility.ToInt64(PatPhysExamSysId) > 0)
                                    {
                                        List<Int64> lstSectionId = new List<Int64>();
                                        lstSectionId.Add(MDVUtility.ToInt64(currentCharacteristicModel.SectionId));
                                        sectionId = insertUpdatePatientPhysicalExamSystemSection(MDVUtility.ToInt64(PatPhysExamSysId), lstSectionId);
                                        if (sectionId != "" && MDVUtility.ToInt64(sectionId) > 0)
                                        {
                                            //dsPhysicalExam

                                            PatientPhysicalExamCharacteristicDetailModel pdetail =
                                                lstCharacteristicDetailModel != null && lstCharacteristicDetailModel.Count > 0 ?
                                                lstCharacteristicDetailModel.OfType<PatientPhysicalExamCharacteristicDetailModel>().ToList().Where(n => n.CharacteristicId == currentCharacteristicModel.CharacteristicId).FirstOrDefault()
                                                : new PatientPhysicalExamCharacteristicDetailModel();
                                            string sectionCharId = insertUpdatePatientPhysicalExamSystemSectionCharecteristics(MDVUtility.ToInt64(sectionId), currentCharacteristicModel, pdetail, lstCharacteristicDetailModel, physicalExamId);
                                            //Start 23-02-2016 Muhammad Arshad Update SoapText for inserted Data
                                            if (sectionCharId != "" && MDVUtility.ToInt64(sectionCharId) > 0)
                                            {
                                                BLObject<DSPhysicalExam> objPhysExamChar = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamCharacteristics(MDVUtility.ToInt64(sectionCharId));
                                                if (objPhysExamChar.Data != null)
                                                {

                                                }
                                            }
                                            BLObject<DSPhysicalExam> objPhysExamSection = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSection(MDVUtility.ToInt64(sectionId));
                                            if (objPhysExamSection.Data != null)
                                            {

                                            }
                                            //End 23-02-2016 Muhammad Arshad Update SoapText for inserted Data
                                        }
                                        BLObject<DSPhysicalExam> objPhysExamSystem = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSystem(MDVUtility.ToInt64(PatPhysExamSysId));
                                        if (objPhysExamSystem.Data != null)
                                        {

                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Sub-Characteristic Handling while updating Patient PhysicalExam

                            //Insert/Update SubCharacteristic
                            if (lstSubCharacteristicModel != null && lstSubCharacteristicModel.Count > 0)
                            {
                                List<PatientPhysicalExamSubCharacteristicModel> lstSubCharacterModel = lstSubCharacteristicModel.OfType<PatientPhysicalExamSubCharacteristicModel>().ToList();
                                foreach (PatientPhysicalExamSubCharacteristicModel currentSubCharacteristicModel in lstSubCharacterModel)
                                {
                                    List<int> lstSubCharacteristicId = new List<int>();
                                    lstSubCharacteristicId.Add(MDVUtility.ToInt(currentSubCharacteristicModel.SystemId));
                                    string PatPhysExamSysId = InsertUpdatePatientPhysicalExamSystem(physicalExamId, lstSubCharacteristicId, false);
                                    if (PatPhysExamSysId != "" && MDVUtility.ToInt64(PatPhysExamSysId) > 0)
                                    {
                                        List<Int64> lstSectionId = new List<Int64>();
                                        lstSectionId.Add(MDVUtility.ToInt64(currentSubCharacteristicModel.SectionId));
                                        string sectionId = insertUpdatePatientPhysicalExamSystemSection(MDVUtility.ToInt64(PatPhysExamSysId), lstSectionId);

                                        if (sectionId != "" && MDVUtility.ToInt64(sectionId) > 0)
                                        {
                                            PatientPhysicalExamCharacteristicDetailModel pdetail =
                                            lstCharacteristicDetailModel != null && lstCharacteristicDetailModel.Count > 0 ? lstCharacteristicDetailModel.OfType<PatientPhysicalExamCharacteristicDetailModel>().ToList().Where(n => n.CharacteristicId == currentSubCharacteristicModel.CharacteristicId).FirstOrDefault() : new PatientPhysicalExamCharacteristicDetailModel();
                                            string sectionCharId = insertUpdatePatientPhysicalExamSystemSectionCharecteristics(MDVUtility.ToInt64(sectionId), new PatientPhysicalExamCharacteristicModel { CharacteristicId = currentSubCharacteristicModel.CharacteristicId }, pdetail, lstCharacteristicDetailModel, physicalExamId);
                                            if (sectionCharId != "" && MDVUtility.ToInt64(sectionCharId) > 0)
                                            {
                                                //Start 23-02-2016 Muhammad Arshad Update SoapText for inserted Data
                                                string sectionCharSubCharId = insertUpdatePatientPhysicalExamSystemSectionCharecteristicsSubCharecteristics(MDVUtility.ToInt64(sectionCharId), currentSubCharacteristicModel, lstSubCharacteristicDetailModel, physicalExamId);
                                                if (sectionCharSubCharId != "" && MDVUtility.ToInt64(sectionCharSubCharId) > 0)
                                                {
                                                    BLObject<DSPhysicalExam> objPhysExamSubChar = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSubCharacteristics(MDVUtility.ToInt64(sectionCharSubCharId));
                                                    if (objPhysExamSubChar.Data != null)
                                                    {
                                                    }
                                                }
                                                BLObject<DSPhysicalExam> objPhysExamChar = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamCharacteristics(MDVUtility.ToInt64(sectionCharId));
                                                if (objPhysExamChar.Data != null)
                                                {

                                                }
                                                //End 23-02-2016 Muhammad Arshad Update SoapText for inserted Data
                                            }
                                            BLObject<DSPhysicalExam> objPhysExamSection = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSection(MDVUtility.ToInt64(sectionId));
                                            if (objPhysExamSection.Data != null)
                                            {

                                            }
                                        }
                                        BLObject<DSPhysicalExam> objPhysExamSystem = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSystem(MDVUtility.ToInt64(PatPhysExamSysId));
                                        if (objPhysExamSystem.Data != null)
                                        {

                                        }
                                    }

                                }
                            }

                            #endregion

                            BLObject<DSPhysicalExam> objPhysExam = BLLClinicalObj.insertUpdateSoapTextForPhysicalExam(PhysicalExamId, templateId);
                            if (objPhysExam.Data != null)
                            {

                            }
                        }
                        //End

                        //End 15-02-2015 Humaira Yousaf

                        //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(physicalExamId);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,
                                PhysicalExamId = PhysicalExamId,
                                NoteId = NoteId
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
                        message = "Patient Physical Exam not found."
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

        // Author: Muhammad Arshad
        // Created Date: 16/02/2016
        //OverView: This function will Insert/Update SoapText for System, Section, Characteristic, SubCharacteristic
        internal void UpdateSoapTextForPatientPhysicalExam(Int64 sectionCharSubCharId, Int64 sectionCharId, Int64 sectionId, Int64 PatPhysExamSysId, Int64 PatPhysExamId)
        {
            //Start 16-02-2016 Muhammad Arshad InsertUpdateSoapText for parents like SubCharacteristic,Characteristic,Section,System
            if (sectionCharSubCharId > 0)
            {
                BLObject<DSPhysicalExam> objPhysExamSubChar = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSubCharacteristics(sectionCharSubCharId);
                if (objPhysExamSubChar.Data != null)
                {
                    if (sectionCharId > 0)
                    {
                        BLObject<DSPhysicalExam> objPhysExamChar = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamCharacteristics(sectionCharId);
                        if (objPhysExamChar.Data != null)
                        {
                            if (sectionId > 0)
                            {
                                BLObject<DSPhysicalExam> objPhysExamSection = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSection(sectionId);
                                if (objPhysExamSection.Data != null)
                                {
                                    if (PatPhysExamSysId > 0)
                                    {
                                        BLObject<DSPhysicalExam> objPhysExamSystem = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSystem(PatPhysExamSysId);

                                        if (objPhysExamSystem.Data != null)
                                        {
                                            if (PatPhysExamId > 0)
                                            {
                                                BLObject<DSPhysicalExam> objPhysExam = BLLClinicalObj.insertUpdateSoapTextForPhysicalExam(PatPhysExamId);
                                                if (objPhysExam.Data != null)
                                                {

                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            else if (sectionCharId > 0)
            {
                BLObject<DSPhysicalExam> objPhysExamChar = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamCharacteristics(MDVUtility.ToInt64(sectionCharId));
                if (objPhysExamChar.Data != null)
                {
                    if (sectionId > 0)
                    {
                        BLObject<DSPhysicalExam> objPhysExamSection = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSection(MDVUtility.ToInt64(sectionId));
                        if (objPhysExamSection.Data != null)
                        {
                            if (PatPhysExamSysId > 0)
                            {
                                BLObject<DSPhysicalExam> objPhysExamSystem = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSystem(MDVUtility.ToInt64(PatPhysExamSysId));
                                if (objPhysExamSystem.Data != null)
                                {
                                    if (PatPhysExamId > 0)
                                    {
                                        BLObject<DSPhysicalExam> objPhysExam = BLLClinicalObj.insertUpdateSoapTextForPhysicalExam(PatPhysExamId);
                                        if (objPhysExam.Data != null)
                                        {

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            else if (PatPhysExamSysId > 0)
            {
                BLObject<DSPhysicalExam> objPhysExamSystem = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamSystem(MDVUtility.ToInt64(PatPhysExamSysId));
                if (objPhysExamSystem.Data != null)
                {
                    if (PatPhysExamId > 0)
                    {
                        BLObject<DSPhysicalExam> objPhysExam = BLLClinicalObj.insertUpdateSoapTextForPhysicalExam(PatPhysExamId);
                        if (objPhysExam.Data != null)
                        {

                        }
                    }

                }
            }
            else if (PatPhysExamId > 0)
            {
                BLObject<DSPhysicalExam> objPhysExam = BLLClinicalObj.insertUpdateSoapTextForPhysicalExam(PatPhysExamId);
                if (objPhysExam.Data != null)
                {

                }
            }



            //End 16-02-2016 Muhammad Arshad InsertUpdateSoapText for parents like SubCharacteristic,Characteristic,Section,System
        }

        #endregion

        #region Patient Physical Exam System Fill, Save and Update Methods

        // Author: Abid Ali
        // Created Date: 08/02/2016
        //OverView: This function will handle fill of Patient Physical Exam system
        public string fillPatientPhysicalExamSystem(PatientPhysicalExamSystemModel model, Int64 patientPhysicalExamSystemId)
        {
            try
            {
                DSPhysicalExam dsPhysicalExamSystem = null;
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystem(MDVUtility.ToInt64(model.PatientPhysicalExamId), patientPhysicalExamSystemId);
                if (obj.Data != null)
                {
                    dsPhysicalExamSystem = obj.Data;
                    if (dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName].Rows[0];
                        var physicalExamSystemKeyValues = new Dictionary<string, string>
                        {
                            { "physicalExamId",  MDVUtility.ToStr(dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName])},
                            { "physicalExamSystemId",  MDVUtility.ToStr(dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.SystemIdColumn.ColumnName])},
                            { "physicalExamSystemIsActive", MDVUtility.ToStr(dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.IsActiveColumn.ColumnName])},
                            { "IsNormal", MDVUtility.ToStr(dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.IsNormalColumn.ColumnName])},
                            { "physicalExamSystemCreatedBy", MDVUtility.ToStr(dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.CreatedByColumn.ColumnName])},
                            { "physicalExamSystemCreatedOn", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.CreatedOnColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsPhysicalExamSystem.PatientPhysicalExam.CreatedOnColumn.ColumnName]).ToShortDateString()},
                            { "physicalExamSystemModifiedBy", MDVUtility.ToStr(dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.ModifiedByColumn.ColumnName])},
                            { "physicalExamSystemModifiedOn", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.ModifiedOnColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsPhysicalExamSystem.PatientPhysicalExam.ModifiedOnColumn.ColumnName]).ToShortDateString()},
                            { "Comments", MDVUtility.ToStr(dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.CommentsColumn.ColumnName])},
                            { "physicalExamSystemSoapText", MDVUtility.ToStr(dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.SoapTextColumn.ColumnName])},
                            {"NormalExamsDetail", MDVUtility.ToStr(dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.NormalCommentsColumn.ColumnName])}
                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            PatientPhysicalExamFill_JSON = js.Serialize(physicalExamSystemKeyValues),
                            //surgicalHxDiseaseFill_JSON = js.Serialize(DiseaseHxkeyValues),
                            // PatientPhysicalExamLoad_JSON = MDVUtility.JSON_DataTable(dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName]),
                            PatientPhysicalExamSystemLoad_JSON = MDVUtility.JSON_DataTable(dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName]),
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
                        Message = MDVCustomException.HumanReadableMessage(obj.Message),
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
        // Created Date: 08/02/2016
        //OverView: This function will insert Patient Physical Exam system record
        public string savePatientPhysicalExamSystem(PatientPhysicalExamSystemModel model)
        {
            try
            {
                DSPhysicalExam dsPhysicalExamSystem = new DSPhysicalExam();

                DSPhysicalExam.PatientPhysicalExamSystemRow dr = dsPhysicalExamSystem.PatientPhysicalExamSystem.NewPatientPhysicalExamSystemRow();

                dr.PatientPhysicalExamId = MDVUtility.ToInt64(model.PatientPhysicalExamId);
                dr.SystemId = MDVUtility.ToInt64(model.SystemId);

                if (!string.IsNullOrEmpty(model.NormalExamsDetail))
                {
                    dr.NormalComments = model.NormalExamsDetail;
                }
                else
                {
                    dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.NormalCommentsColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.Comments))
                {
                    dr.Comments = MDVUtility.ToStr(model.Comments);
                }
                else
                {
                    dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.CommentsColumn] = DBNull.Value;
                }
                dr.IsNormal = model.IsNormal;
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsPhysicalExamSystem.PatientPhysicalExamSystem.AddPatientPhysicalExamSystemRow(dr);
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.InsertPatientPhysicalExamSystem(dsPhysicalExamSystem, model.NormalExamsDetail);
                dsPhysicalExamSystem = obj.Data;

                if (obj.Data != null)
                {
                    //var diseaseId = "";
                    Int64 PhysicalExamSystemId = MDVUtility.ToInt64(dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName].Rows[0][dsPhysicalExamSystem.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]);
                    //if (SurgicalHxId > 0)
                    //{
                    //    if (lstDiseaseObject.Count > 0)
                    //    {
                    //        string responseMedicalHxDisease = insertUpdateDisease(SurgicalHxId, lstDiseaseObject);
                    //        diseaseId = responseMedicalHxDisease;
                    //    }
                    //}

                    // BLObject<string> objValue = BLLClinicalObj.updateSoapTextForSurgicalHX(SurgicalHxId);
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        PhysicalExamSystemId = MDVUtility.ToInt64(dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName].Rows[0][dsPhysicalExamSystem.PatientPhysicalExamSystem.PatientPhysicalExamIdColumn.ColumnName]),
                        // diseaseId = diseaseId
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

        // Author: Abid Ali
        // Created Date: 09/02/2016
        //OverView: This function will update Patient Physical Exam system record
        public string updatePatientPhysicalExamSystem(PatientPhysicalExamSystemModel model, Int64 physicalExamSystemId)
        {
            try
            {
                DSPhysicalExam dsPhysicalExamSystem = new DSPhysicalExam();
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystem(MDVUtility.ToInt64(model.PatientPhysicalExamId), physicalExamSystemId);
                if (obj.Data != null)
                {
                    dsPhysicalExamSystem = obj.Data;
                    foreach (DSPhysicalExam.PatientPhysicalExamSystemRow dr in dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.PatientPhysicalExamId))
                        {
                            dr.PatientPhysicalExamId = MDVUtility.ToInt64(model.PatientPhysicalExamId);
                        }
                        if (!string.IsNullOrEmpty(model.SystemId))
                        {
                            dr.SystemId = MDVUtility.ToInt64(model.SystemId);
                        }


                        if (!string.IsNullOrEmpty(model.NormalExamsDetail))
                        {
                            dr.NormalComments = model.NormalExamsDetail;
                        }
                        else
                        {
                            dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.NormalCommentsColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.Comments))
                        {
                            dr.Comments = MDVUtility.ToStr(model.Comments);
                        }
                        else
                        {
                            dr[dsPhysicalExamSystem.PatientPhysicalExamSystem.CommentsColumn] = DBNull.Value;
                        }
                        dr.IsNormal = model.IsNormal;
                        // dr.IsActive = true;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //  dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        //var diseaseID = "";
                        //if (lstDiseaseObject.Count > 0)
                        //{
                        //    string responseMedicalHxDisease = insertUpdateDisease(MedicalHxId, lstDiseaseObject);
                        //    diseaseID = responseMedicalHxDisease;
                        //}
                        //Start 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                        //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(MedicalHxId);

                        //End 08/01/2016 Muhammad Arshad Code to  Save/Update MedicalHx SoapText for MiscHx Tab
                    }

                    #region Database Updation
                    if (dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName].Rows.Count > 0)
                    {
                        BLObject<DSPhysicalExam> objUpdate = BLLClinicalObj.UpdatePatientPhysicalExamSystem(dsPhysicalExamSystem, model.NormalExamsDetail);
                        //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForMedicalHX(physicalExamId);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,
                                // diseaseId = diseaseID
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
                            message = Common.AppPrivileges.No_Record_Message
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
                        message = MDVCustomException.HumanReadableMessage(obj.Message),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Patient Physical Exam System Section Fill, Save and Update Methods

        // Author: Farooq Ahmad
        // Created Date: 11/02/2016
        //OverView: This function will handle fill of Patient Physical Exam system Section
        public string fillPatientPhysicalExamSystemSection(PatientPhysicalExamSystemSectionModel model, Int64 patientPhysicalExamSystemSectionId)
        {
            try
            {
                DSPhysicalExam dsPhysicalExamSystemSection = null;
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSection(MDVUtility.ToInt64(model.PatientPhysicalExamSystemId), patientPhysicalExamSystemSectionId);
                dsPhysicalExamSystemSection = obj.Data;
                if (dsPhysicalExamSystemSection.Tables[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsPhysicalExamSystemSection.Tables[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.TableName].Rows[0];
                    var physicalExamSystemKeyValues = new Dictionary<string, string>
                        {
                            { "patientPhysicalExamSystemId",  MDVUtility.ToStr(dr[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemIdColumn.ColumnName])},
                            { "patientPhysicalExamSystemSectionId",  MDVUtility.ToStr(dr[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName])},
                            { "sectionId",  MDVUtility.ToStr(dr[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.SectionIdColumn.ColumnName])},
                            { "isActive", MDVUtility.ToStr(dr[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.IsActiveColumn.ColumnName])},
                            { "physicalExamSystemSectionCreatedBy", MDVUtility.ToStr(dr[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.CreatedByColumn.ColumnName])},
                            { "physicalExamSystemSectionCreatedOn", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.CreatedOnColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsPhysicalExamSystemSection.PatientPhysicalExam.CreatedOnColumn.ColumnName]).ToShortDateString()},
                            { "physicalExamSystemSectionModifiedBy", MDVUtility.ToStr(dr[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.ModifiedByColumn.ColumnName])},
                            { "physicalExamSystemSectionModifiedOn", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.ModifiedOnColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsPhysicalExamSystemSection.PatientPhysicalExam.ModifiedOnColumn.ColumnName]).ToShortDateString()},
                            { "Comments", MDVUtility.ToStr(dr[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.CommentsColumn.ColumnName])},
                            { "physicalExamSystemSectionSoapText", MDVUtility.ToStr(dr[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.SoapTextColumn.ColumnName])},

                        };
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        PatientPhysicalExamFill_JSON = js.Serialize(physicalExamSystemKeyValues),
                        //surgicalHxDiseaseFill_JSON = js.Serialize(DiseaseHxkeyValues),
                        // PatientPhysicalExamLoad_JSON = MDVUtility.JSON_DataTable(dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName]),
                        PatientPhysicalExamSystemLoad_JSON = MDVUtility.JSON_DataTable(dsPhysicalExamSystemSection.Tables[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                return "";

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

        // Author: Farooq Ahmad
        // Created Date: 11/02/2016
        //OverView: This function will insert Patient Physical Exam system record
        public string savePatientPhysicalExamSystemSection(PatientPhysicalExamSystemSectionModel model)
        {
            try
            {
                DSPhysicalExam dsPhysicalExamSystemSection = new DSPhysicalExam();

                DSPhysicalExam.PatientPhysicalExamSystemSectionRow dr = dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.NewPatientPhysicalExamSystemSectionRow();

                dr.PatientPhysicalExamSystemId = MDVUtility.ToInt64(model.PatientPhysicalExamSystemId);
                dr.SectionId = MDVUtility.ToInt64(model.SectionId);



                if (!string.IsNullOrEmpty(model.Comments))
                {
                    dr.Comments = MDVUtility.ToStr(model.Comments);
                }
                else
                {
                    dr[dsPhysicalExamSystemSection.PatientPhysicalExamSystem.CommentsColumn] = DBNull.Value;
                }
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.AddPatientPhysicalExamSystemSectionRow(dr);
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystemSection(dsPhysicalExamSystemSection, string.Empty);
                dsPhysicalExamSystemSection = obj.Data;

                if (obj.Data != null)
                {

                    Int64 PhysicalExamSystemSectionId = MDVUtility.ToInt64(dsPhysicalExamSystemSection.Tables[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.TableName].Rows[0][dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName]);

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        PhysicalExamSystemSectionId = PhysicalExamSystemSectionId,
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

        // Author: Farooq Ahmad
        // Created Date: 11/02/2016
        //OverView: This function will update Patient Physical Exam system record
        public string updatePatientPhysicalExamSystemSection(PatientPhysicalExamSystemSectionModel model, Int64 physicalExamSystemSectionId)
        {
            try
            {
                DSPhysicalExam dsPhysicalExamSystemSection = new DSPhysicalExam();
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSection(MDVUtility.ToInt64(model.PatientPhysicalExamSystemId), physicalExamSystemSectionId);
                dsPhysicalExamSystemSection = obj.Data;
                foreach (DSPhysicalExam.PatientPhysicalExamSystemSectionRow dr in dsPhysicalExamSystemSection.Tables[dsPhysicalExamSystemSection.PatientPhysicalExamSystemSection.TableName].Rows)
                {
                    dr.PatientPhysicalExamSystemId = MDVUtility.ToInt64(model.PatientPhysicalExamSystemId);
                    dr.SectionId = MDVUtility.ToInt64(model.SectionId);
                    if (!string.IsNullOrEmpty(model.Comments))
                    {
                        dr.Comments = MDVUtility.ToStr(model.Comments);
                    }
                    else
                    {
                        dr[dsPhysicalExamSystemSection.PatientPhysicalExamSystem.CommentsColumn] = DBNull.Value;
                    }
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Updation
                    if (dsPhysicalExamSystemSection.Tables[dsPhysicalExamSystemSection.PatientPhysicalExamSystem.TableName].Rows.Count > 0)
                    {
                        BLObject<DSPhysicalExam> objUpdate = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystemSection(dsPhysicalExamSystemSection, string.Empty);

                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,

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

                return "";
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

        // Author:  Muhammad Arshad
        // Created Date: 12/02/2016
        //OverView: This function will handle insert/update of PatientPhysicalExamSystemSection
        public string insertUpdatePatientPhysicalExamSystemSection(Int64 patPhysExamSysId, List<Int64> sectionIds)
        {
            DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();

            BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSection(patPhysExamSysId, 0);
            dsPhysicalExam = obj.Data;
            string currentPatPhysExamSysSectionId = string.Empty;
            foreach (int SystemSectionId in sectionIds)
            {
                DSPhysicalExam.PatientPhysicalExamSystemSectionRow RowSystemSection = null;
                //Start Farooq Ahmad 17/02/2016
                string SelectionSelect = string.Concat(dsPhysicalExam.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemIdColumn.ColumnName, "=", patPhysExamSysId, " AND ", dsPhysicalExam.PatientPhysicalExamSystemSection.SectionIdColumn.ColumnName, "=", SystemSectionId);
                DSPhysicalExam.PatientPhysicalExamSystemSectionRow[] arrSystemSections = (DSPhysicalExam.PatientPhysicalExamSystemSectionRow[])dsPhysicalExam.PatientPhysicalExamSystemSection.Select(SelectionSelect);
                //End Farooq Ahmad 17/02/2016
                if (arrSystemSections.Length > 0)
                {
                    RowSystemSection = arrSystemSections[0];
                    currentPatPhysExamSysSectionId = MDVUtility.ToStr(RowSystemSection[dsPhysicalExam.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName]);
                }
                else
                {
                    RowSystemSection = dsPhysicalExam.PatientPhysicalExamSystemSection.NewPatientPhysicalExamSystemSectionRow();
                }

                if (RowSystemSection != null)
                {
                    //RowSystemSection.PatientPhysicalExamSystemSectionId = SystemSectionId;
                    RowSystemSection.PatientPhysicalExamSystemId = patPhysExamSysId;
                    RowSystemSection.SectionId = SystemSectionId;
                    RowSystemSection.IsActive = true;
                    if (arrSystemSections.Length == 0)
                    {
                        RowSystemSection.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowSystemSection.CreatedOn = DateTime.Now;
                    }
                    RowSystemSection.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowSystemSection.ModifiedOn = DateTime.Now;
                    RowSystemSection[dsPhysicalExam.PatientPhysicalExamSystemSection.SoapTextColumn] = DBNull.Value;
                    if (arrSystemSections.Length < 1)
                    {
                        dsPhysicalExam.PatientPhysicalExamSystemSection.AddPatientPhysicalExamSystemSectionRow(RowSystemSection);
                    }
                }
            }

            #region Database Insertion/Updation

            BLObject<DSPhysicalExam> objInsertedSystemSection = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystemSection(dsPhysicalExam, "");
            if (objInsertedSystemSection.Data != null)
            {
                DSPhysicalExam dsSavedSection = objInsertedSystemSection.Data;
                if (currentPatPhysExamSysSectionId == string.Empty)
                {
                    foreach (int SystemSectionId in sectionIds)
                    {
                        string SelectionSelect = string.Concat(dsPhysicalExam.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemIdColumn.ColumnName, "=", patPhysExamSysId, " AND ", dsPhysicalExam.PatientPhysicalExamSystemSection.SectionIdColumn.ColumnName, "=", SystemSectionId);
                        DSPhysicalExam.PatientPhysicalExamSystemSectionRow[] arrSystemSections = (DSPhysicalExam.PatientPhysicalExamSystemSectionRow[])dsSavedSection.PatientPhysicalExamSystemSection.Select(SelectionSelect);
                        if (arrSystemSections.Length > 0)
                        {
                            currentPatPhysExamSysSectionId = MDVUtility.ToStr(arrSystemSections[0][dsSavedSection.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName]);
                        }
                    }

                }
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    physicalExamSystemSectionId = currentPatPhysExamSysSectionId != string.Empty ? currentPatPhysExamSysSectionId : dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamSystemSection.TableName].Rows.Count > 0 ? dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamSystemSection.TableName].Rows[0][dsPhysicalExam.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName] : 0,
                };
                return response.physicalExamSystemSectionId.ToString();//(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedSystemSection.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }

        #endregion




        #region Patient Physical Exam System Section Charecteristics Fill, Save and Update Methods
        /// <summary>
        /// Module Name: insertUpdatePatientPhysicalExamSystemSectionCharecteristics
        /// Author: Humaira Yousaf
        /// Created Date: 15-02-2016
        /// Description: Inserts/updates Patient Physical Exam System Section Charecteristics/SubCharecteristics
        /// </summary>
        /// <param name="sectionId" type="long">section Id</param> 
        /// <param name="charecteristicModel" type="PatientPhysicalExamCharacteristicModel">charecteristicModel</param> 
        /// <param name="detailModel" type="PatientPhysicalExamCharacteristicDetailModel">detailModel</param>   
        /// <param name="lstCharacteristicDetails" type="List<object>">Characteristic Details</param>       
        /// <param name="patPhysExamId" type="Int64">patient physicalexam id</param>           
        public string insertUpdatePatientPhysicalExamSystemSectionCharecteristics(Int64 sectionId, PatientPhysicalExamCharacteristicModel charecteristicModel, PatientPhysicalExamCharacteristicDetailModel detailModel, List<object> lstCharacteristicDetails, Int64 patPhysExamId = 0)
        {
            DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();

            BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristic(sectionId, 0);
            dsPhysicalExam = obj.Data;
            string CurrentPatPhysExamSysCharId = string.Empty;

            DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow RowSectionChar = null;
            DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow[] arrSectionChars = (DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow[])dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.Select(dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionIdColumn + "=" + sectionId + " AND " + dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName + "=" + charecteristicModel.CharacteristicId);

            if (arrSectionChars.Length > 0)
            {

                RowSectionChar = arrSectionChars[0];
                CurrentPatPhysExamSysCharId = MDVUtility.ToStr(RowSectionChar[dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn.ColumnName]);
            }
            else
            {
                RowSectionChar = dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.NewPatientPhysicalExamSystemSectionCharacteristicRow();
            }

            if (RowSectionChar != null)
            {
                RowSectionChar.PatientPhysicalExamSystemSectionId = sectionId;
                RowSectionChar.CharacteristicId = MDVUtility.ToInt64(charecteristicModel.CharacteristicId);
                if (!string.IsNullOrEmpty(charecteristicModel.CharacteristicDetail))
                    RowSectionChar.Comments = charecteristicModel.CharacteristicDetail;
                RowSectionChar.IsPositive = Convert.ToBoolean(charecteristicModel.IsCharacteristicPositive);
                RowSectionChar.IsActive = true;
                if (arrSectionChars.Length == 0)
                {
                    RowSectionChar.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowSectionChar.CreatedOn = DateTime.Now;
                }
                RowSectionChar.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                RowSectionChar.ModifiedOn = DateTime.Now;
                if (arrSectionChars.Length < 1)
                {
                    dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.AddPatientPhysicalExamSystemSectionCharacteristicRow(RowSectionChar);

                }
            }
            //Start//16-02-2016//Ahmad Raza//creating soap text 
            int counter = 0;
            foreach (DataRow RowChar in dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.Rows)
            {

                RowChar[dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.SoapTextColumn] = getCharacteristicsDetailSoapText(detailModel);
                counter++;
            }
            //End//16-02-2016//Ahmad Raza//creating soap text 

            #region Database Insertion/Updation

            BLObject<DSPhysicalExam> objInsertedSystemSection = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystemSectionCharacteristic(dsPhysicalExam);
            if (objInsertedSystemSection.Data != null)
            {
                DSPhysicalExam dsSavedChar = objInsertedSystemSection.Data;

                if (CurrentPatPhysExamSysCharId == string.Empty)
                {
                    DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow[] arrSavedChar = (DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow[])dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.Select(dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionIdColumn + "=" + sectionId + " AND " + dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName + "=" + charecteristicModel.CharacteristicId);
                    if (arrSavedChar != null && arrSavedChar.Length > 0)
                    {
                        CurrentPatPhysExamSysCharId = MDVUtility.ToStr(arrSavedChar[0][dsSavedChar.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn.ColumnName]);

                    }
                }

                foreach (DataRow RowChar in dsSavedChar.PatientPhysicalExamSystemSectionCharacteristic.Rows)
                {
                    if (patPhysExamId > 0)
                    {
                        string patPhysSectionCharId = MDVUtility.ToStr(RowChar[dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn]);
                        string SectionCharId = MDVUtility.ToStr(RowChar[dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn]);

                        if (lstCharacteristicDetails != null && lstCharacteristicDetails.Count > 0)
                        {
                            PatientPhysicalExamCharacteristicDetailModel pdetail =
                                                   lstCharacteristicDetails != null && lstCharacteristicDetails.Count > 0 ? lstCharacteristicDetails.OfType<PatientPhysicalExamCharacteristicDetailModel>().ToList().Where(n => n.CharacteristicId == SectionCharId).FirstOrDefault() : new PatientPhysicalExamCharacteristicDetailModel();
                            string detailId = insertUpdatePatientPhysicalExamDetail(patPhysExamId, MDVUtility.ToInt64(patPhysSectionCharId), "Characteristic", pdetail);
                        }
                    }

                }
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    physicalExamSystemSectionCharId = CurrentPatPhysExamSysCharId != string.Empty ? CurrentPatPhysExamSysCharId : "0",
                };
                return response.physicalExamSystemSectionCharId.ToString();//(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedSystemSection.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }

        #endregion

        #region Patient Physical Exam System Section Charecteristics Sub Charecteristics Fill, Save and Update Methods
        /// <summary>
        /// Module Name: insertUpdatePatientPhysicalExamSystemSectionCharecteristicsSubCharecteristics
        /// Author: Humaira Yousaf
        /// Created Date: 15-02-2016
        /// Description: Inserts/updates Patient Physical Exam System Section Charecteristics/SubCharecteristics
        /// </summary>
        /// <param name="charecteristicId" type="Int64">charecteristic Id</param> 
        /// <param name="subCharecteristicModel" type="PatientPhysicalExamSubCharacteristicModel">charecteristicModel</param> 
        /// <param name="List<object>" type="lstSubCharacteristicDetails">Sub Characteristic Details</param>           
        /// <param name="patPhysExamId" type="Int64">patient physical exam id</param> 
        public string insertUpdatePatientPhysicalExamSystemSectionCharecteristicsSubCharecteristics(Int64 charecteristicId, PatientPhysicalExamSubCharacteristicModel subCharecteristicModel, List<object> lstSubCharacteristicDetails, Int64 patPhysExamId = 0)
        {
            DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
            string CurrentPatPhysExamSysCharSubCharId = string.Empty;
            BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(charecteristicId, 0);
            dsPhysicalExam = obj.Data;

            DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow RowSectionCharSubChars = null;
            DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow[] arrSectionCharSubChars = (DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow[])dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.Select(dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn + "=" + charecteristicId + " AND " + dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName + "=" + subCharecteristicModel.SubCharacteristicId);

            if (arrSectionCharSubChars.Length > 0)
            {
                RowSectionCharSubChars = arrSectionCharSubChars[0];
                CurrentPatPhysExamSysCharSubCharId = MDVUtility.ToStr(RowSectionCharSubChars[dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn.ColumnName]);
            }
            else
            {
                RowSectionCharSubChars = dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.NewPatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow();
            }

            if (RowSectionCharSubChars != null)
            {
                RowSectionCharSubChars.PatientPhysicalExamSystemSectionCharacteristicId = charecteristicId;
                RowSectionCharSubChars.SubCharacteristicId = MDVUtility.ToInt64(subCharecteristicModel.SubCharacteristicId);
                if (subCharecteristicModel.IsForComments.Value)
                    RowSectionCharSubChars.Comments = subCharecteristicModel.SubCharacteristicDetail == null ? "" : subCharecteristicModel.SubCharacteristicDetail;
                RowSectionCharSubChars.IsPositive = Convert.ToBoolean(subCharecteristicModel.IsSubCharacteristicPositive);
                RowSectionCharSubChars.IsActive = true;
                if (arrSectionCharSubChars.Length == 0)
                {
                    RowSectionCharSubChars.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowSectionCharSubChars.CreatedOn = DateTime.Now;
                }
                RowSectionCharSubChars.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                RowSectionCharSubChars.ModifiedOn = DateTime.Now;
                if (arrSectionCharSubChars.Length < 1)
                {
                    dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.AddPatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow(RowSectionCharSubChars);
                }
            }


            #region Database Insertion/Updation

            BLObject<DSPhysicalExam> objInsertedSystemSection = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(dsPhysicalExam);
            if (objInsertedSystemSection.Data != null)
            {
                DSPhysicalExam dsSavedChar = objInsertedSystemSection.Data;
                if (CurrentPatPhysExamSysCharSubCharId == string.Empty)
                {
                    DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow[] arrSavedChar = (DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow[])dsSavedChar.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.Select(dsSavedChar.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn + "=" + charecteristicId + " AND " + dsSavedChar.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName + "=" + subCharecteristicModel.SubCharacteristicId);
                    if (arrSavedChar != null && arrSavedChar.Length > 0)
                    {
                        CurrentPatPhysExamSysCharSubCharId = MDVUtility.ToStr(arrSavedChar[0][dsSavedChar.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn.ColumnName]);
                    }
                }

                foreach (DataRow RowChar in dsSavedChar.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.Rows)
                {
                    if (patPhysExamId > 0)
                    {
                        string patPhysSectionSubCharId = MDVUtility.ToStr(RowChar[dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn]);
                        string SectionSubCharId = MDVUtility.ToStr(RowChar[dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn]);
                        PatientPhysicalExamSubCharacteristicDetailModel pdetail =
                                               lstSubCharacteristicDetails != null && lstSubCharacteristicDetails.Count > 0 ? lstSubCharacteristicDetails.OfType<PatientPhysicalExamSubCharacteristicDetailModel>().ToList().Where(n => n.SubCharacteristicId == SectionSubCharId).FirstOrDefault() : new PatientPhysicalExamSubCharacteristicDetailModel();
                        string detailId = insertUpdatePatientPhysicalExamDetail(patPhysExamId, MDVUtility.ToInt64(patPhysSectionSubCharId), "SubCharacteristic", pdetail);
                    }

                }
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    physicalExamSystemSectionCharSubCharId = CurrentPatPhysExamSysCharSubCharId != string.Empty ? CurrentPatPhysExamSysCharSubCharId : "0",
                };
                return response.physicalExamSystemSectionCharSubCharId.ToString();//(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objInsertedSystemSection.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }

        #endregion



        #region Physical Exam User Sytem Load Save

        // Author: Humaira Yousaf
        // Created Date: 15/02/2016
        //OverView: This function will fill physical exam user system
        public string fillPhysicalExamUserSystem(long physicalExamId = 0, long templateId = 0)
        {
            try
            {
                DSPhysicalExam dsPhysicalExam = null;
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPhysicalExamUserSystems(templateId);
                dsPhysicalExam = obj.Data;
                if (dsPhysicalExam.Tables[dsPhysicalExam.PhysicalExam_UserSystem.TableName].Rows.Count > 0)
                {
                    List<Dictionary<string, string>> lstPhysicalExam_UserSystem = new List<Dictionary<string, string>>();
                    foreach (DataRow dr in dsPhysicalExam.Tables[dsPhysicalExam.PhysicalExam_UserSystem.TableName].Rows)
                    {
                        var PhysicalExam_UserSystem = new Dictionary<string, string>
                        {
                            { "UserSystemId",  MDVUtility.ToStr(dr[dsPhysicalExam.PhysicalExam_UserSystem.UserSystemIdColumn.ColumnName])},
                            { "SystemId",  MDVUtility.ToStr(dr[dsPhysicalExam.PhysicalExam_UserSystem.SystemIdColumn.ColumnName])},
                            { "ShortName",  MDVUtility.ToStr(dr[dsPhysicalExam.PhysicalExam_UserSystem.ShortNameColumn.ColumnName])},
                            { "SystemOrder",  MDVUtility.ToStr(dr[dsPhysicalExam.PhysicalExam_UserSystem.SystemOrderColumn.ColumnName])},

                        };
                        lstPhysicalExam_UserSystem.Add(PhysicalExam_UserSystem);
                    }

                    // Start 11-02-2016 Humaira Yousaf to get normal systems of physical exam
                    string normalSystemIds = "";
                    DSPhysicalExam dsPhysicalExamNormal = new DSPhysicalExam();
                    BLObject<DSPhysicalExam> objNormalSystem = BLLClinicalObj.LoadPatientPhysicalExamSystem(physicalExamId, 0);
                    dsPhysicalExamNormal = objNormalSystem.Data;

                    DSPhysicalExam.PatientPhysicalExamSystemRow[] arrNormalSystems = (DSPhysicalExam.PatientPhysicalExamSystemRow[])dsPhysicalExamNormal.PatientPhysicalExamSystem.Select(dsPhysicalExam.PatientPhysicalExamSystem.IsNormalColumn.ColumnName + "= true");
                    if (arrNormalSystems.Length > 0)
                    {
                        foreach (DSPhysicalExam.PatientPhysicalExamSystemRow system in arrNormalSystems)
                        {
                            normalSystemIds += system.SystemId + ",";
                        }

                        normalSystemIds = normalSystemIds.TrimEnd(',');
                    }
                    //End 11-02-2016 Humaira Yousaf to get normal systems of physical exam
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        PhysicalExamSystem_JSON = js.Serialize(lstPhysicalExam_UserSystem),
                        NormalSystemIds = normalSystemIds
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                return "";
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

        // Author: Farooq Ahmad
        // Created Date: 15/02/2016
        //OverView: This function will save physical exam user system
        public string savePhysicalExamUserSystem(PhysicalExamSystemModel model)
        {

            try
            {
                DSPhysicalExam dsPhysicalExam = null;
                BLObject<DSPhysicalExam> obj;

                obj = BLLClinicalObj.LoadPhysicalExamUserSystems(model.TemplateId);

                dsPhysicalExam = obj.Data;

                if (model != null && model.SystemCustomSorted != string.Empty)
                {
                    List<int> systemOrder = model.SystemCustomSorted.Split(',').Select(int.Parse).Reverse().ToList();
                    foreach (DSPhysicalExam.PhysicalExam_UserSystemRow dr in dsPhysicalExam.Tables[dsPhysicalExam.PhysicalExam_UserSystem.TableName].Rows)
                    {
                        dr.SystemOrder = systemOrder.IndexOf(dr.SystemOrder) + 1;

                    }
                }
                obj = BLLClinicalObj.insertUpdatePhysicalExamUserSystem(dsPhysicalExam);

                return fillPhysicalExamUserSystem(0, model.TemplateId);
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
        /// Module Name: InsertUpdatePatientPhysicalExamSystem
        /// Author: Humaira Yousaf
        /// Created Date: 10-02-2016
        /// Description: Inserts/updates Patient Physical Exam Normal System
        /// </summary>
        /// <param name="PhysicalExamId" type="long">Patient physical exam Id</param> 
        /// <param name="normalSystemIds" type="List<int>">list of normal system ids</param> 
        /// <param name="LisToMarkNormal" type="bool">isToMarkNormal</param>           
        /// <param name="systemModel" type="PatientPhysicalExamSystemModel">systemModel</param> 
        private string InsertUpdatePatientPhysicalExamSystem(long PhysicalExamId, List<int> normalSystemIds, bool isToMarkNormal = true, PatientPhysicalExamSystemModel systemModel = null, bool isAllNormal = true, PatientPhysicalExamModel physicalExamModelObj = null)
        {
            #region NormalSystems
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
            else
            {
                //Start 09-03-2016 Humaira Yousaf for normal systems

                //if (isToMarkNormal == true && normalSystemIds.Count > 0)
                //{
                //    string strNormalSystemIds = string.Join(",", normalSystemIds);
                //    DSPhysicalExam.PatientPhysicalExamSystemRow[] arrNotNormalSystems = (DSPhysicalExam.PatientPhysicalExamSystemRow[])dsPhysicalExam.PatientPhysicalExamSystem.Select(dsPhysicalExam.PatientPhysicalExamSystem.SystemIdColumn.ColumnName + " NOT IN (" + strNormalSystemIds + ")");
                //    foreach (DSPhysicalExam.PatientPhysicalExamSystemRow currentdr in arrNotNormalSystems)
                //    {
                //        if (currentdr.IsNormal == true)
                //        {
                //            currentdr.IsNormal = false;
                //            currentdr.NormalComments = "";
                //        }
                //    }
                //}

                if (!isAllNormal)
                {

                    foreach (DSPhysicalExam.PatientPhysicalExamSystemRow currentdr in dsPhysicalExam.PatientPhysicalExamSystem.Rows)
                    {
                        currentdr.IsNormal = isToMarkNormal;
                        currentdr.NormalComments = "";

                    }
                }
                //End 09-03-2016 Humaira Yousaf for normal systems
                if (normalSystemIds != null)
                {
                    foreach (int systemId in normalSystemIds)
                    {
                        DSPhysicalExam.PatientPhysicalExamSystemRow RowNormalSystem = null;
                        DSPhysicalExam.PatientPhysicalExamSystemRow[] arrNormalSystems = (DSPhysicalExam.PatientPhysicalExamSystemRow[])dsPhysicalExam.PatientPhysicalExamSystem.Select(dsPhysicalExam.PatientPhysicalExamSystem.SystemIdColumn.ColumnName + "=" + systemId);

                        if (arrNormalSystems.Length > 0)
                        {
                            RowNormalSystem = arrNormalSystems[0];
                        }
                        else
                        {
                            RowNormalSystem = dsPhysicalExam.PatientPhysicalExamSystem.NewPatientPhysicalExamSystemRow();
                        }

                        if (RowNormalSystem != null)
                        {
                            RowNormalSystem.SystemId = systemId;
                            RowNormalSystem.PatientPhysicalExamId = PhysicalExamId;
                            RowNormalSystem.IsNormal = isToMarkNormal;

                            RowNormalSystem.IsActive = true;
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
                                //if (physicalExamModelObj != null)
                                //{
                                //    RowNormalSystem.NormalComments = physicalExamModelObj.NormalExamsDetail;
                                //}
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
                    }
                }
            }

            #region Database Insertion/Updation

            BLObject<DSPhysicalExam> objInsertedNormalSystem = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystem(dsPhysicalExam);
            if (objInsertedNormalSystem.Data != null)
            {
                DSPhysicalExam dsSavedSystem = objInsertedNormalSystem.Data;

                if (currentPatPhysSysId == string.Empty)
                {
                    DSPhysicalExam.PatientPhysicalExamSystemRow[] arrNormalSystems = null;
                    if (systemModel != null)
                    {
                        arrNormalSystems = (DSPhysicalExam.PatientPhysicalExamSystemRow[])dsPhysicalExam.PatientPhysicalExamSystem.Select(dsPhysicalExam.PatientPhysicalExamSystem.SystemIdColumn.ColumnName + "=" + systemModel.PhysicalExamSystemId);
                    }
                    else
                    {
                        if (normalSystemIds != null && normalSystemIds.Count > 0)
                            arrNormalSystems = (DSPhysicalExam.PatientPhysicalExamSystemRow[])dsPhysicalExam.PatientPhysicalExamSystem.Select(dsPhysicalExam.PatientPhysicalExamSystem.SystemIdColumn.ColumnName + "=" + MDVUtility.ToStr(normalSystemIds[0]));

                    }
                    if (arrNormalSystems != null && arrNormalSystems.Length > 0)
                    {
                        currentPatPhysSysId = MDVUtility.ToStr(arrNormalSystems[0][dsSavedSystem.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]);
                    }
                }


                //else
                //{
                //    currentPatPhysSysId = dsSavedSystem.Tables[dsSavedSystem.PatientPhysicalExamSystem.TableName].Rows.Count > 0 ? MDVUtility.ToStr(dsSavedSystem.Tables[dsSavedSystem.PatientPhysicalExamSystem.TableName].Rows[dsSavedSystem.Tables[dsSavedSystem.PatientPhysicalExamSystem.TableName].Rows.Count - 1][dsSavedSystem.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]) : "0";
                //}
                UpdateSoapTextForPatientPhysicalExam(0, 0, 0, 0, PhysicalExamId);
                foreach (DataRow drPatPhysExamSystem in dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamSystem.TableName].Rows)
                {
                    if (Convert.ToBoolean(drPatPhysExamSystem[dsPhysicalExam.PatientPhysicalExamSystem.IsNormalColumn.ColumnName]) == true)
                    {
                        Int64 patPhysExamId = MDVUtility.ToInt64(drPatPhysExamSystem[dsPhysicalExam.PatientPhysicalExamSystem.PatientPhysicalExamIdColumn.ColumnName]);
                        Int64 patPhysExamSysId = MDVUtility.ToInt64(drPatPhysExamSystem[dsPhysicalExam.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]);
                        string isDeleted = deletePatientPhysicalExamSystemSection(patPhysExamId, patPhysExamSysId);
                    }
                }
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

            #endregion
        }

        #endregion

        #region PhysicalExam SoapText Generation

        // Author:  Muhammad Arshad
        // Created Date: 12/02/2016
        //OverView: Generates SoapText for Characteristic of a section
        internal string getCharacteristicsDetailSoapText(PatientPhysicalExamCharacteristicDetailModel modelObj)
        {
            string TobaccoSoapText = string.Empty;
            StringBuilder sb = new StringBuilder();
            // string frequency = "";
            if (modelObj != null)
            {
                //Start//16-02-2016//Ahmad Raza//creating SoapText for CharacteristicsDetail
                string spanStart = "<span id='physicalExamSubCharDetail_" + modelObj.CharacteristicId + "' title='PhysicalExamSubCharDetail'  name='Physical Exam SubCharDetail'>";
                string spanEnd = "</ span > ";
                string spanBody = string.Empty;
                spanBody += ((string.IsNullOrEmpty(modelObj.PhysicalExamPreviousHistory) ? "" : " Previous History is " + modelObj.PhysicalExamPreviousHistory + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamStatus_text) ? "" : modelObj.PhysicalExamStatus_text == "- Select -" ? "" : " Status is " + modelObj.PhysicalExamStatus_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamOnset) ? "" : " Onset is " + modelObj.PhysicalExamOnset + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamDurationLength) ? "" : modelObj.PhysicalExamDurationPeriod_text == "- Select -" ? "" : " Duration is " + modelObj.PhysicalExamDurationLength + ' ' + modelObj.PhysicalExamDurationPeriod_text + '.'));
                spanBody += ((string.IsNullOrEmpty(modelObj.PhysicalExamPattern_text) ? "" : modelObj.PhysicalExamPattern_text == "- Select -" ? "" : " Pattern is " + modelObj.PhysicalExamPattern_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamSeverity_text) ? "" : modelObj.PhysicalExamSeverity_text == "- Select -" ? "" : " Severity is " + modelObj.PhysicalExamSeverity_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamCourse_text) ? "" : modelObj.PhysicalExamCourse_text == "- Select -" ? "" : " Course is " + modelObj.PhysicalExamCourse_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamRadiation_text) ? "" : modelObj.PhysicalExamRadiation_text == "- Select -" ? "" : " Radiation to " + modelObj.PhysicalExamRadiation_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamFrequency_text) ? "" : modelObj.PhysicalExamFrequency_text == "- Select -" ? "" : " Frequency is " + modelObj.PhysicalExamFrequency_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamContext_text) ? "" : modelObj.PhysicalExamContext_text == "- Select -" ? "" : " Context is " + modelObj.PhysicalExamContext_text + '.'));
                spanBody += ((string.IsNullOrEmpty(modelObj.PhysicalExamCharacter_text) ? "" : modelObj.PhysicalExamCharacter_text == "- Select -" ? "" : " Character is " + modelObj.PhysicalExamCharacter_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamAgggravatedby_text) ? "" : modelObj.PhysicalExamAgggravatedby_text == "- Select -" ? "" : " it is aggravated by " + modelObj.PhysicalExamAgggravatedby_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamRelievedby_text) ? "" : modelObj.PhysicalExamRelievedby_text == "- Select -" ? "" : " it is relieved by " + modelObj.PhysicalExamRelievedby_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamLocation) ? "" : " Location is " + modelObj.PhysicalExamLocation + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamPercipitatedby) ? "" : " Precipitated by " + modelObj.PhysicalExamPercipitatedby + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamAssociatedwith) ? "" : " Associated feature are " + modelObj.PhysicalExamAssociatedwith + '.'));
                //End//16-02-2016//Ahmad Raza//creating SoapText for CharacteristicsDetail
                if (!string.IsNullOrWhiteSpace(sb.ToString()))
                {
                    sb.Append(spanStart);
                    sb.Append(spanBody);
                    sb.Append(spanEnd);

                }
            }
            else
            {
                return string.Empty;
            }
            return sb.ToString();
        }

        // Author:  Muhammad Arshad
        // Created Date: 12/02/2016
        //OverView: Generates SoapText for SubCharacteristic of a characteristic
        internal string getSubCharacteristicsDetailSoapText(PatientPhysicalExamSubCharacteristicDetailModel modelObj)
        {
            string TobaccoSoapText = string.Empty;
            StringBuilder sb = new StringBuilder();
            // string frequency = "";
            if (modelObj != null)
            {
                //Start//16-02-2016//Ahmad Raza//creating SoapText for SubCharacteristicsDetail
                string spanStart = "<span id='physicalExamSubCharDetail_" + modelObj.CharacteristicId + "' title='PhysicalExamSubCharDetail'  name='Physical Exam SubCharDetail'>";
                string spanEnd = "</ span > ";
                string spanBody = string.Empty;
                spanBody += ((string.IsNullOrEmpty(modelObj.PhysicalExamPreviousHistory) ? "" : " Previous History is " + modelObj.PhysicalExamPreviousHistory + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamStatus_text) ? "" : modelObj.PhysicalExamStatus_text == "- Select -" ? "" : " Status is " + modelObj.PhysicalExamStatus_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamOnset) ? "" : " Onset is " + modelObj.PhysicalExamOnset + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamDurationLength) ? "" : modelObj.PhysicalExamDurationPeriod_text == "- Select -" ? "" : " Duration is " + modelObj.PhysicalExamDurationLength + ' ' + modelObj.PhysicalExamDurationPeriod_text + '.'));
                spanBody += ((string.IsNullOrEmpty(modelObj.PhysicalExamPattern_text) ? "" : modelObj.PhysicalExamPattern_text == "- Select -" ? "" : " Pattern is " + modelObj.PhysicalExamPattern_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamSeverity_text) ? "" : modelObj.PhysicalExamSeverity_text == "- Select -" ? "" : " Severity is " + modelObj.PhysicalExamSeverity_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamCourse_text) ? "" : modelObj.PhysicalExamCourse_text == "- Select -" ? "" : " Course is " + modelObj.PhysicalExamCourse_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamRadiation_text) ? "" : modelObj.PhysicalExamRadiation_text == "- Select -" ? "" : " Radiation to " + modelObj.PhysicalExamRadiation_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamFrequency_text) ? "" : modelObj.PhysicalExamFrequency_text == "- Select -" ? "" : " Frequency is " + modelObj.PhysicalExamFrequency_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamContext_text) ? "" : modelObj.PhysicalExamContext_text == "- Select -" ? "" : " Context is " + modelObj.PhysicalExamContext_text + '.'));
                spanBody += ((string.IsNullOrEmpty(modelObj.PhysicalExamCharacter_text) ? "" : modelObj.PhysicalExamCharacter_text == "- Select -" ? "" : " Character is " + modelObj.PhysicalExamCharacter_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamAgggravatedby_text) ? "" : modelObj.PhysicalExamAgggravatedby_text == "- Select -" ? "" : " it is aggravated by " + modelObj.PhysicalExamAgggravatedby_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamRelievedby_text) ? "" : modelObj.PhysicalExamRelievedby_text == "- Select -" ? "" : " it is relieved by " + modelObj.PhysicalExamRelievedby_text + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamLocation) ? "" : " Location is " + modelObj.PhysicalExamLocation + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamPercipitatedby) ? "" : " Precipitated by " + modelObj.PhysicalExamPercipitatedby + '.') + (string.IsNullOrEmpty(modelObj.PhysicalExamAssociatedwith) ? "" : " Associated feature are " + modelObj.PhysicalExamAssociatedwith + '.'));
                //End//16-02-2016//Ahmad Raza//creating SoapText for CharacteristicsDetail
                if (!string.IsNullOrWhiteSpace(sb.ToString()))
                {
                    sb.Append(spanStart);
                    sb.Append(spanBody);
                    sb.Append(spanEnd);

                }
            }
            else
            {
                return string.Empty;
            }
            return sb.ToString();
        }

        #endregion


        //Start//12-02-2016//Ahmad Raza//Implimented methods for PhysicalExam's association with Note
        #region Attach/Detach PhysicalExam from Notes

        // Author: Ahmad Raza
        // Created Date: 12/02/2016
        //OverView:  This function will attach physical exam with notes
        internal string attachPhysicalExamWithNotes(PatientPhysicalExamModel physicalExamModel, long notesId, long templateId)
        {
            try
            {


                DSPhysicalExam dsPhysicalExam = null;
                //templateId
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExam(MDVUtility.ToInt64(physicalExamModel.PatientId), MDVUtility.ToInt64(physicalExamModel.PatientPhysicalExamId), MDVUtility.ToInt64(physicalExamModel.NotesId));
                dsPhysicalExam = obj.Data;

                if (dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows.Count > 0)
                {

                    obj = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamAndChild(MDVUtility.ToInt64(physicalExamModel.PatientPhysicalExamId), templateId);

                    var response = new
                    {
                        status = true,


                        PhysicalExamLoad_JSON = MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName]),
                        Message = Common.AppPrivileges.Update_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    string response = null;

                    response = savePatientPhysicalExam(physicalExamModel, null, null, null, null, null, templateId);


                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                //if (string.IsNullOrEmpty(MDVUtility.ToStr(physicalExamId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
                //{
                //    var response = new
                //    {
                //        status = false,
                //        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                //    };
                //    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                //}
                //else
                //{
                //    BLObject<DSPhysicalExam> obj = BLLClinicalObj.attachPhysicalExamWithNotes(physicalExamId, notesId);
                //    if (obj.Data != null)
                //    {
                //        dsPhysicalExam = obj.Data;
                //        var response = new
                //        {
                //            status = true,
                //            PhysicalExamTotalCount = dsPhysicalExam.Tables[dsPhysicalExam.PhysicalExam_UserSystem.TableName].Rows.Count,
                //            PhysicalExamCount = dsPhysicalExam.Tables[dsPhysicalExam.PhysicalExam_UserSystem.TableName].Rows.Count,
                //            PhysicalExamLoad_JSON = MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PhysicalExam_UserSystem.TableName]),
                //            Message = Common.AppPrivileges.Update_Message
                //        };

                //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                //    }
                //    else
                //    {
                //        var response = new
                //        {
                //            status = false,
                //            Message = obj.Data
                //        };
                //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                //    }
                //}
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

        // Author: Ahmad Raza
        // Created Date: 12/02/2016
        //OverView:  This function will detach physical exam with notes
        internal string detachPhysicalExamFromNotes(long physicalExamId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(physicalExamId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachPhysicalExamFromNotes(physicalExamId, notesId);
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

        #region Physical Exam Detail

        // Author: Farooq Ahmad
        // Created Date: 12/02/2016
        //OverView:  This function will insert update patient physical exam detail
        public string insertUpdatePatientPhysicalExamDetail(Int64 physicalExamId, long parentId, string parentType, object detailModel)
        {
            DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
            if (detailModel == null)
            {
                return "";
            }
            if (string.Equals(parentType, "Characteristic"))
            {
                PatientPhysicalExamCharacteristicDetailModel charDetailModel = new PatientPhysicalExamCharacteristicDetailModel();
                charDetailModel = (PatientPhysicalExamCharacteristicDetailModel)detailModel;
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamDetail(physicalExamId, parentId, parentType, 0);
                dsPhysicalExam = obj.Data;

                DSPhysicalExam.PatientPhysicalExamDetailRow RowExamDetail = null;
                DSPhysicalExam.PatientPhysicalExamDetailRow[] arrExamDetails = (DSPhysicalExam.PatientPhysicalExamDetailRow[])dsPhysicalExam.PatientPhysicalExamDetail.Select(dsPhysicalExam.PatientPhysicalExamDetail.ParentIdColumn.ColumnName + "=" + parentId);


                if (arrExamDetails.Length > 0)
                {
                    RowExamDetail = arrExamDetails[0];
                }
                else
                {
                    RowExamDetail = dsPhysicalExam.PatientPhysicalExamDetail.NewPatientPhysicalExamDetailRow();
                }

                if (RowExamDetail != null)
                {
                    RowExamDetail.PatientPhysicalExamId = physicalExamId;
                    RowExamDetail.ParentId = parentId;
                    RowExamDetail.ParentType = parentType;
                    //Start Farooq Ahmad 18/02/2016 if Empty Store null value
                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamAgggravatedby))
                    {
                        RowExamDetail.AggravatedById = MDVUtility.ToInt32(charDetailModel.PhysicalExamAgggravatedby);
                    }
                    else
                    {
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.AggravatedByIdColumn] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamAssociatedwith))
                    {
                        RowExamDetail.AssociatedWith = charDetailModel.PhysicalExamAssociatedwith;
                    }
                    else
                    {
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.AssociatedWithColumn] = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamAssociatedwith))
                    {
                        RowExamDetail.CharacterId = MDVUtility.ToInt64(charDetailModel.PhysicalExamCharacter);
                    }
                    else
                    {
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.CharacterIdColumn] = DBNull.Value;
                    }


                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamContext))
                        RowExamDetail.ContextId = MDVUtility.ToInt64(charDetailModel.PhysicalExamContext);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.CharacterIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamCourse))
                        RowExamDetail.CourseId = MDVUtility.ToInt64(charDetailModel.PhysicalExamCourse);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.CourseIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamDurationLength))
                        RowExamDetail.DurationLength = MDVUtility.ToInt32(charDetailModel.PhysicalExamDurationLength);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.DurationLengthColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamDurationPeriod))
                        RowExamDetail.DurationPeriodId = MDVUtility.ToInt32(charDetailModel.PhysicalExamDurationPeriod);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.DurationPeriodIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamFrequency))
                        RowExamDetail.FrequencyId = MDVUtility.ToInt64(charDetailModel.PhysicalExamFrequency);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.FrequencyIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamLocation))
                        RowExamDetail.Location = charDetailModel.PhysicalExamLocation;
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.LocationColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamOnset))
                        RowExamDetail.Onset = charDetailModel.PhysicalExamOnset;
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.OnsetColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamPattern))
                        RowExamDetail.PatternId = MDVUtility.ToInt32(charDetailModel.PhysicalExamPattern);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.PatternIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamPreviousHistory))
                        RowExamDetail.PrevHistory = charDetailModel.PhysicalExamPreviousHistory;
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.PrevHistoryColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamRadiation))
                        RowExamDetail.RadiationId = MDVUtility.ToInt64(charDetailModel.PhysicalExamRadiation);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.RadiationIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamRelievedby))
                        RowExamDetail.RelievedbyId = MDVUtility.ToInt64(charDetailModel.PhysicalExamRelievedby);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.RelievedbyIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamStatus))
                        RowExamDetail.StatusId = MDVUtility.ToInt32(charDetailModel.PhysicalExamStatus);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.StatusIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamSeverity))
                        RowExamDetail.SeverityId = MDVUtility.ToInt32(charDetailModel.PhysicalExamSeverity);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.SeverityIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(charDetailModel.PhysicalExamPercipitatedby))
                        RowExamDetail.Precipitatedby = charDetailModel.PhysicalExamPercipitatedby;
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.PrecipitatedbyColumn] = DBNull.Value;
                    //End Farooq Ahmad 18/02/2016 if Empty Store null value
                    RowExamDetail.IsActive = true;
                    if (arrExamDetails.Length == 0)
                    {
                        RowExamDetail.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowExamDetail.CreatedOn = DateTime.Now;
                    }
                    RowExamDetail.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowExamDetail.ModifiedOn = DateTime.Now;
                    RowExamDetail.SoapText = getCharacteristicsDetailSoapText(charDetailModel);

                    if (arrExamDetails.Length < 1)
                    {
                        dsPhysicalExam.PatientPhysicalExamDetail.AddPatientPhysicalExamDetailRow(RowExamDetail);
                    }
                }
            }
            else
            {
                PatientPhysicalExamSubCharacteristicDetailModel subCharDetailModel = new PatientPhysicalExamSubCharacteristicDetailModel();
                subCharDetailModel = (PatientPhysicalExamSubCharacteristicDetailModel)detailModel;
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamDetail(physicalExamId, parentId, parentType, 0);
                dsPhysicalExam = obj.Data;

                DSPhysicalExam.PatientPhysicalExamDetailRow RowExamDetail = null;
                DSPhysicalExam.PatientPhysicalExamDetailRow[] arrExamDetails = (DSPhysicalExam.PatientPhysicalExamDetailRow[])dsPhysicalExam.PatientPhysicalExamDetail.Select(dsPhysicalExam.PatientPhysicalExamDetail.ParentIdColumn.ColumnName + "=" + parentId);


                if (arrExamDetails.Length > 0)
                {
                    RowExamDetail = arrExamDetails[0];
                }
                else
                {
                    RowExamDetail = dsPhysicalExam.PatientPhysicalExamDetail.NewPatientPhysicalExamDetailRow();
                }

                if (RowExamDetail != null)
                {
                    RowExamDetail.PatientPhysicalExamId = physicalExamId;
                    RowExamDetail.ParentId = parentId;
                    RowExamDetail.ParentType = parentType;
                    //Start Farooq Ahmad 18/02/2016 if Empty Store null value
                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamAgggravatedby))
                        RowExamDetail.AggravatedById = MDVUtility.ToInt32(subCharDetailModel.PhysicalExamAgggravatedby);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.AggravatedByIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamAssociatedwith))
                        RowExamDetail.AssociatedWith = subCharDetailModel.PhysicalExamAssociatedwith;
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.AssociatedWithColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamCharacter))
                        RowExamDetail.CharacterId = MDVUtility.ToInt64(subCharDetailModel.PhysicalExamCharacter);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.CharacterIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamContext))
                        RowExamDetail.ContextId = MDVUtility.ToInt64(subCharDetailModel.PhysicalExamContext);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.ContextIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamCourse))
                        RowExamDetail.CourseId = MDVUtility.ToInt64(subCharDetailModel.PhysicalExamCourse);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.CourseIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamDurationLength))
                        RowExamDetail.DurationLength = MDVUtility.ToInt32(subCharDetailModel.PhysicalExamDurationLength);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.DurationLengthColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamDurationPeriod))
                        RowExamDetail.DurationPeriodId = MDVUtility.ToInt32(subCharDetailModel.PhysicalExamDurationPeriod);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.DurationPeriodIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamFrequency))
                        RowExamDetail.FrequencyId = MDVUtility.ToInt64(subCharDetailModel.PhysicalExamFrequency);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.FrequencyIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamLocation))
                        RowExamDetail.Location = subCharDetailModel.PhysicalExamLocation;
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.LocationColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamOnset))
                        RowExamDetail.Onset = subCharDetailModel.PhysicalExamOnset;
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.OnsetColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamPattern))
                        RowExamDetail.PatternId = MDVUtility.ToInt32(subCharDetailModel.PhysicalExamPattern);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.PatternIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamPreviousHistory))
                        RowExamDetail.PrevHistory = subCharDetailModel.PhysicalExamPreviousHistory;
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.PrevHistoryColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamRadiation))
                        RowExamDetail.RadiationId = MDVUtility.ToInt64(subCharDetailModel.PhysicalExamRadiation);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.RadiationIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamRelievedby))
                        RowExamDetail.RelievedbyId = MDVUtility.ToInt64(subCharDetailModel.PhysicalExamRelievedby);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.RelievedbyIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamStatus))
                        RowExamDetail.StatusId = MDVUtility.ToInt32(subCharDetailModel.PhysicalExamStatus);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.StatusIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamSeverity))
                        RowExamDetail.SeverityId = MDVUtility.ToInt32(subCharDetailModel.PhysicalExamSeverity);
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.SeverityIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(subCharDetailModel.PhysicalExamPercipitatedby))
                        RowExamDetail.Precipitatedby = subCharDetailModel.PhysicalExamPercipitatedby;
                    else
                        RowExamDetail[dsPhysicalExam.PatientPhysicalExamDetail.PrecipitatedbyColumn] = DBNull.Value;
                    //End Farooq Ahmad 18/02/2016 if Empty Store null value
                    RowExamDetail.IsActive = true;
                    if (arrExamDetails.Length == 0)
                    {
                        RowExamDetail.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowExamDetail.CreatedOn = DateTime.Now;
                    }

                    RowExamDetail.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowExamDetail.ModifiedOn = DateTime.Now;
                    RowExamDetail.SoapText = getSubCharacteristicsDetailSoapText(subCharDetailModel);

                    if (arrExamDetails.Length < 1)
                    {
                        dsPhysicalExam.PatientPhysicalExamDetail.AddPatientPhysicalExamDetailRow(RowExamDetail);
                    }
                }
            }


            #region Database Insertion/Updation
            //End//17-02-2016//Ahmad Raza//insert/update in exam detail table
            //Here InsertUpdate of Details will be called by specifying ParentId=sectionCharId and ParentType='Characteristic'
            BLObject<DSPhysicalExam> objExamDetail = BLLClinicalObj.insertUpdatePatientPhysicalExamDetail(dsPhysicalExam, parentType);
            if (objExamDetail.Data != null)
            {
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    physicalExamDetailId = dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamDetail.TableName].Rows.Count > 0 ? dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamDetail.TableName].Rows[0][dsPhysicalExam.PatientPhysicalExamDetail.DetailIdColumn.ColumnName] : 0,
                };
                return response.physicalExamDetailId.ToString();//(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = objExamDetail.Message
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

            #endregion
        }

        #endregion
        /// <summary>
        /// Module Name: deletePatientPhysicalExam
        /// Author: Humaira Yousaf
        /// Created Date: 18-02-2016
        /// Description: Deletes patient physical exam
        /// </summary>
        /// <param name="patientId" type="long">patient Id</param> 
        /// <param name="patientPhysicalExamId" type="long">patientPhysicalExam Id</param> 
        /// <param name="systemId" type="long">systemId</param>                   
        public string deletePatientPhysicalExam(long patientId, long patientPhysicalExamId, long systemId)
        {
            try
            {
                DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
                BLObject<DSPhysicalExam> obj;
                BLObject<string> objSystemDelete = new BLObject<string>();

                obj = BLLClinicalObj.LoadPatientPhysicalExamSystem(patientPhysicalExamId, 0);
                dsPhysicalExam = obj.Data;

                DSPhysicalExam.PatientPhysicalExamSystemRow RowExamSystem = null;
                DSPhysicalExam.PatientPhysicalExamSystemRow[] arrExamSystem = (DSPhysicalExam.PatientPhysicalExamSystemRow[])dsPhysicalExam.PatientPhysicalExamSystem.Select(dsPhysicalExam.PatientPhysicalExamSystem.PatientPhysicalExamIdColumn.ColumnName + "=" + patientPhysicalExamId + " AND " + dsPhysicalExam.PatientPhysicalExamSystem.SystemIdColumn + "=" + systemId);

                if (arrExamSystem.Length > 0)
                {
                    RowExamSystem = arrExamSystem[0];
                    objSystemDelete = BLLClinicalObj.DeletePatientPhysicalExamSystem(MDVUtility.ToStr(RowExamSystem[dsPhysicalExam.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn]));
                }

                if (objSystemDelete.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Delete_Message,

                    };
                    return response.message.ToString();//(Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Module Name: deletePatientPhysicalExamSystem
        /// Author: Humaira Yousaf
        /// Created Date: 18-02-2016
        /// Description: Deletes patient physical exam system
        /// </summary>
        /// <param name="patientPhysicalExamId" type="long">patientPhysicalExam Id</param> 
        /// <param name="systemId" type="long">systemId</param>    
        public string deletePatientPhysicalExamSystem(long patientPhysicalExamId, long systemId)
        {
            try
            {

                BLObject<string> objSystemDelete = new BLObject<string>();

                string msg = deletePatientPhysicalExamSystemSection(patientPhysicalExamId, systemId);
                objSystemDelete = BLLClinicalObj.DeletePatientPhysicalExamSystem(MDVUtility.ToStr(systemId));

                if (objSystemDelete.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Delete_Message,

                    };
                    return response.message.ToString();//(Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objSystemDelete.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Module Name: deletePatientPhysicalExamSystemSection
        /// Author: Humaira Yousaf
        /// Created Date: 18-02-2016
        /// Description: Deletes patient physical exam system section
        /// </summary>
        /// <param name="patientPhysicalExamId" type="long">patientPhysicalExam Id</param> 
        /// <param name="systemId" type="long">systemId</param> 
        /// <param name="sectionId" type="long">sectionId</param>  
        public string deletePatientPhysicalExamSystemSection(long patientPhysicalExamId, long systemId, long sectionId = 0)
        {
            try
            {

                DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSection(systemId, 0);
                dsPhysicalExam = obj.Data;
                BLObject<string> objSectionDelete = new BLObject<string>();

                //Start 19-02-2016 Humaira Yousaf modified for section Id
                if (sectionId == 0)
                {
                    foreach (DataRow row in dsPhysicalExam.PatientPhysicalExamSystemSection.Rows)
                    {
                        objSectionDelete = BLLClinicalObj.DeletePatientPhysicalExamSystemSection(MDVUtility.ToStr(row[dsPhysicalExam.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn]));
                    }
                }
                else
                {
                    DSPhysicalExam.PatientPhysicalExamSystemSectionRow RowExamSystemSection = (DSPhysicalExam.PatientPhysicalExamSystemSectionRow)dsPhysicalExam.PatientPhysicalExamSystemSection.Select(dsPhysicalExam.PatientPhysicalExamSystemSection.SectionIdColumn.ColumnName + "=" + sectionId).FirstOrDefault();
                    if (RowExamSystemSection != null)
                    {
                        objSectionDelete = BLLClinicalObj.DeletePatientPhysicalExamSystemSection(MDVUtility.ToStr(RowExamSystemSection[dsPhysicalExam.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn]));
                    }
                }

                //End 19-02-2016 Humaira Yousaf modified for section Id

                if (objSectionDelete.Data != null)
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
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Module Name: deletePatientPhysicalExamSystemSectionCharacteristic
        /// Author: Humaira Yousaf
        /// Created Date: 18-02-2016
        /// Description: Deletes patient physical exam system section Characteristic
        /// </summary>
        /// <param name="patientPhysicalExamId" type="long">patientPhysicalExam Id</param> 
        /// <param name="sectionId" type="long">section Id</param>  
        /// <param name="characteristicId" type="long">characteristic Id</param> 
        public string deletePatientPhysicalExamSystemSectionCharacteristic(long patientPhysicalExamId, long sectionId, long characteristicId)
        {
            try
            {
                DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristic(sectionId, 0);
                dsPhysicalExam = obj.Data;
                DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow[] CharRow = null;
                BLObject<string> objCharDelete = new BLObject<string>();

                if (characteristicId > 0)
                {
                    CharRow = (DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow[])dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.Select(dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn + "=" + characteristicId);
                    if (CharRow.Length > 0)
                    {
                        objCharDelete = BLLClinicalObj.DeletePatientPhysicalExamSystemSectionCharacteristic(MDVUtility.ToStr(CharRow[0][dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn]));

                    }
                }
                else
                {
                    foreach (DataRow row in dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.Rows)
                    {
                        objCharDelete = BLLClinicalObj.DeletePatientPhysicalExamSystemSectionCharacteristic(MDVUtility.ToStr(row[dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn]));

                    }
                }


                if (objCharDelete.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Delete_Message,
                    };
                    return response.message.ToString();//(Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Module Name: deletePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic
        /// Author: Humaira Yousaf
        /// Created Date: 18-02-2016
        /// Description: Deletes patient physical exam system section Characteristic SubCharacteristic
        /// </summary>
        /// <param name="patientPhysicalExamId" type="long">patientPhysicalExam Id</param> 
        /// <param name="characteristicId" type="long">characteristic Id</param> 
        /// <param name="subCharacteristicId" type="long">subCharacteristic Id</param>  
        public string deletePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(long patientPhysicalExamId, long characteristicId, long subCharacteristicId)
        {
            try
            {
                DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(characteristicId, 0);
                dsPhysicalExam = obj.Data;

                BLObject<string> objSubCharDelete = new BLObject<string>();

                if (subCharacteristicId > 0)
                {
                    DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow[] CharRow = null;


                    CharRow = (DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow[])dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.Select(dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn + "=" + subCharacteristicId);
                    if (CharRow.Length > 0)
                    {
                        objSubCharDelete = BLLClinicalObj.DeletePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(MDVUtility.ToStr(CharRow[0][dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn]));
                    }
                }
                else
                {
                    foreach (DataRow row in dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.Rows)
                    {
                        objSubCharDelete = BLLClinicalObj.DeletePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(MDVUtility.ToStr(row[dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn]));
                    }
                }

                if (objSubCharDelete.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Delete_Message,

                    };
                    return response.message.ToString();//(Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Module Name: deletePatientPhysicalExamDetail
        /// Author: Humaira Yousaf
        /// Created Date: 18-02-2016
        /// Description: Deletes patient physical exam detail 
        /// </summary>
        /// <param name="patientPhysicalExamId" type="long">patientPhysicalExam Id</param> 
        /// <param name="parentId" type="long">characteristic/subCharacteristic Id</param> 
        /// <param name="parentType" type="string">characteristic/subCharacteristic</param>  
        public string deletePatientPhysicalExamDetail(long patientPhysicalExamId, long parentId, string parentType)
        {
            try
            {

                BLObject<string> objDetailDelete = new BLObject<string>();
                objDetailDelete = BLLClinicalObj.DeletePatientPhysicalExamDetail(patientPhysicalExamId, parentType, parentId, "");


                if (objDetailDelete.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Delete_Message,

                    };
                    return response.message.ToString();//(Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        /// <summary>
        /// Module Name: getPhysicalExamForSoap
        /// Author: Ahmad Raza
        /// Created Date: 11-04-2016
        /// Description: load physical exam for soap text 
        /// </summary>
        /// <param name="physicalExamId"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        internal string getPhysicalExamForSoap(string physicalExamId, long patientId)
        {
            try
            {

                DSPhysicalExam dsPhysicalExam = null;
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.loadPhysicalExamForSoap(physicalExamId, patientId);
                dsPhysicalExam = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PhysicalExamSoapCount = dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows.Count,
                            PhysicalExamSoap_JSON = MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PhysicalExamSoapCount = 0,
                            PhysicalExamSoap_JSON = MDVUtility.JSON_DataTable(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName]),
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
        /// Module Name: savePatientPhysicalExam
        /// Author: Farooq Ahmad
        /// Created Date: 17-06-2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string savePatientPhysicalExam(PatientPhysicalExamModel model)
        {
            DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();

            DSPhysicalExam.PatientPhysicalExamRow dr = dsPhysicalExam.PatientPhysicalExam.NewPatientPhysicalExamRow();

            dr.PatientId = MDVUtility.ToInt64(model.PatientId);

            if (!string.IsNullOrEmpty(model.PatientPhysicalExamDate))
                dr.PatientPhysicalExamDate = MDVUtility.ToDateTime(model.PatientPhysicalExamDate);
            else
                dr[dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamDateColumn] = DBNull.Value;

            if (!string.IsNullOrEmpty(model.Comments))
                dr.Comments = MDVUtility.ToStr(model.Comments);
            else
                dr[dsPhysicalExam.PatientPhysicalExam.CommentsColumn] = DBNull.Value;

            if (model.NotesId == -1)
                dr[dsPhysicalExam.PatientPhysicalExam.NoteIdColumn] = DBNull.Value;
            else
                dr.NoteId = model.NotesId;

            if (model.TemplateId == -1)
                dr[dsPhysicalExam.PatientPhysicalExam.TemplateIdColumn] = DBNull.Value;
            else
                dr.TemplateId = model.TemplateId;

            dr.bNormalExam = model.bNormal.Value;
            dr.IsActive = true;
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;

            dsPhysicalExam.PatientPhysicalExam.AddPatientPhysicalExamRow(dr);
            BLObject<DSPhysicalExam> obj = BLLClinicalObj.InsertPatientPhysicalExam(dsPhysicalExam, model.NormalComments);
            dsPhysicalExam = obj.Data;

            if (obj.Data != null)
            {
                Int64 PhysicalExamId = MDVUtility.ToInt64(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows[0][dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamIdColumn.ColumnName]);

                insertUpdateSysSecCharSubChar(PhysicalExamId, model);

                BLLClinicalObj.insertUpdateSoapTextForPhysicalExamAndChild(MDVUtility.ToInt64(PhysicalExamId), model.TemplateId);
                attachPhysicalExamWithNotes(model, MDVUtility.ToInt64(model.NotesId), MDVUtility.ToInt64(model.TemplateId));

                var SoapTextObj = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamAndChild(MDVUtility.ToInt64(PhysicalExamId), model.TemplateId);
                var response = new
                {
                    status = true,
                    PhysicalExamId = PhysicalExamId,
                    NoteId = model.NotesId,
                    message = Common.AppPrivileges.Save_Message,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        private void insertUpdateSysSecCharSubChar(Int64 PhysicalExamId, PatientPhysicalExamModel model)
        {
            if (model.bNormal.Value)
            {
                if (model.Systems == null)
                    model.Systems = new List<PhysicalExamSystemModel>();
                DSPhysicalExam dsPhysicalExamSystem = null;
                BLObject<DSPhysicalExam> objSystem = BLLClinicalObj.LoadPhysicalExamUserSystems(model.TemplateId);
                dsPhysicalExamSystem = objSystem.Data;
                if (model.Systems == null || model.Systems.Count == 0)
                {
                    if (dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PhysicalExam_UserSystem.TableName].Rows.Count > 0)
                    {
                        List<Dictionary<string, string>> lstPhysicalExam_UserSystem = new List<Dictionary<string, string>>();
                        foreach (DataRow drSystem in dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PhysicalExam_UserSystem.TableName].Rows)
                        {
                            var system = new PhysicalExamSystemModel();
                            system.SystemId = MDVUtility.ToInt32(drSystem[dsPhysicalExamSystem.PhysicalExam_UserSystem.SystemIdColumn.ColumnName]);
                            system.isNormal = "true";
                            system.Comments = model.NormalComments;
                            model.Systems.Add(system);
                        }
                    }
                }
                else
                {
                    foreach (var system in model.Systems)
                    {
                        system.isNormal = "true";
                        system.Comments = model.NormalComments;
                    }
                }

            }

            foreach (var system in model.Systems)
            {
                bool containCharacteristics = false;
                if (system.Sections != null)
                    foreach (var section in system.Sections)
                    {
                        if (section.Characteristics != null && section.Characteristics.Count > 0)
                            containCharacteristics = true;
                    }
                if (!containCharacteristics && system.isNormal != null && system.isNormal.ToLower() != "true") continue;

                long PatientPhysicalExamSystemId = InsertSystem(PhysicalExamId, system);

                if (system.isNormal.ToLower() == "true") continue;
                if (system.Sections != null)
                {
                    foreach (var section in system.Sections)
                    {
                        long PatientPhysicalExamSectionId = InsertSection(PatientPhysicalExamSystemId, section);
                        if (section.Characteristics != null)
                        {
                            foreach (var characteristic in section.Characteristics)
                            {
                                long PatientPhysicalExamSectionCharId = InsertCharacteristic(PatientPhysicalExamSectionId, characteristic);
                                if (PatientPhysicalExamSectionCharId > 0)
                                    insertUpdatePatientPhysicalExamDetail(PhysicalExamId, PatientPhysicalExamSectionCharId, "Characteristic", characteristic.SectionCharacteristicDetailModel);
                                if (characteristic.SubCharacteristics != null)
                                {
                                    foreach (var subcharacteristics in characteristic.SubCharacteristics)
                                    {
                                        long PatientPhysicalExamSectionCharSubCharId = InsertSubCharacteristic(PatientPhysicalExamSectionCharId, subcharacteristics);
                                        if (PatientPhysicalExamSectionCharId > 0)
                                            insertUpdatePatientPhysicalExamDetail(PhysicalExamId, PatientPhysicalExamSectionCharSubCharId, "SubCharacteristic", subcharacteristics.SubCharacteristicDetailModel);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Module Name: InsertSubCharacteristic
        /// Author: Farooq Ahmad
        /// Created Date: 17-06-2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private long InsertSubCharacteristic(long PatientPhysicalExamSectionCharId, PatientPhysicalExamSubCharacteristicModel model)
        {
            DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
            BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(PatientPhysicalExamSectionCharId, 0);
            dsPhysicalExam = obj.Data;

            DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow RowNormalSystem = null;
            long SubCharacteristicId = MDVUtility.ToInt64(model.SubCharacteristicId);
            DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow[] arrNormalSystems = (DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow[])dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.Select(dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName + "=" + SubCharacteristicId);

            if (arrNormalSystems.Length > 0)
                RowNormalSystem = arrNormalSystems[0];
            else
                RowNormalSystem = dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.NewPatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow();

            if (RowNormalSystem != null)
            {
                RowNormalSystem.SubCharacteristicId = SubCharacteristicId;
                RowNormalSystem.PatientPhysicalExamSystemSectionCharacteristicId = PatientPhysicalExamSectionCharId;
                RowNormalSystem.IsPositive = Convert.ToBoolean(model.IsPositive);
                RowNormalSystem.Comments = model.Comments;


                RowNormalSystem.IsActive = true;
                if (arrNormalSystems.Length == 0)
                {
                    RowNormalSystem.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowNormalSystem.CreatedOn = DateTime.Now;
                }
                RowNormalSystem.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                RowNormalSystem.ModifiedOn = DateTime.Now;

                if (arrNormalSystems.Length < 1)
                    dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.AddPatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow(RowNormalSystem);
            }

            BLObject<DSPhysicalExam> objInsertedNormalSystem = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(dsPhysicalExam);
            if (objInsertedNormalSystem.Data != null)
            {
                DSPhysicalExam dsSavedSystem = objInsertedNormalSystem.Data;
                DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow[] row = (DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicRow[])dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.Select(dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName + "=" + SubCharacteristicId);
                return MDVUtility.ToInt64(row[0][dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn.ColumnName]);
                //int Count = dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName].Rows.Count;
                //if (Count > 0)
                //    return MDVUtility.ToInt64(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName].Rows[Count - 1][dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn.ColumnName]);
                //else
                //    return 0;
            }
            else
                return 0;
        }

        /// <summary>
        /// Module Name: InsertCharacteristic
        /// Author: Farooq Ahmad
        /// Created Date: 17-06-2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private long InsertCharacteristic(long PatientPhysicalExamSectionId, PatientPhysicalExamCharacteristicModel model)
        {
            DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
            BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristic(PatientPhysicalExamSectionId, 0);
            dsPhysicalExam = obj.Data;

            DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow RowNormalSystem = null;
            long CharacteristicId = MDVUtility.ToInt64(model.SectionCharacteristicId);
            DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow[] arrNormalSystems = (DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow[])dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.Select(dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName + "=" + CharacteristicId);

            if (arrNormalSystems.Length > 0)
                RowNormalSystem = arrNormalSystems[0];
            else
                RowNormalSystem = dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.NewPatientPhysicalExamSystemSectionCharacteristicRow();

            if (RowNormalSystem != null)
            {
                RowNormalSystem.CharacteristicId = CharacteristicId;
                RowNormalSystem.PatientPhysicalExamSystemSectionId = PatientPhysicalExamSectionId;
                RowNormalSystem.IsPositive = Convert.ToBoolean(model.IsPositive);
                RowNormalSystem.Comments = model.Comments;


                RowNormalSystem.IsActive = true;
                if (arrNormalSystems.Length == 0)
                {
                    RowNormalSystem.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowNormalSystem.CreatedOn = DateTime.Now;
                }
                RowNormalSystem.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                RowNormalSystem.ModifiedOn = DateTime.Now;

                if (arrNormalSystems.Length < 1)
                    dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.AddPatientPhysicalExamSystemSectionCharacteristicRow(RowNormalSystem);
            }

            BLObject<DSPhysicalExam> objInsertedNormalSystem = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystemSectionCharacteristic(dsPhysicalExam);
            if (objInsertedNormalSystem.Data != null)
            {
                DSPhysicalExam dsSavedSystem = objInsertedNormalSystem.Data;
                DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow[] row = (DSPhysicalExam.PatientPhysicalExamSystemSectionCharacteristicRow[])dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.Select(dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName + "=" + CharacteristicId);
                return MDVUtility.ToInt64(row[0][dsPhysicalExam.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn.ColumnName]);

            }
            else
                return 0;
        }

        /// <summary>
        /// Module Name: InsertSection
        /// Author: Farooq Ahmad
        /// Created Date: 17-06-2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private long InsertSection(long PatientPhysicalExamSystemId, PatientPhysicalExamSystemSectionModel model)
        {
            DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
            BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystemSection(PatientPhysicalExamSystemId, 0);
            dsPhysicalExam = obj.Data;

            DSPhysicalExam.PatientPhysicalExamSystemSectionRow RowNormalSystem = null;
            long SectionId = MDVUtility.ToInt64(model.SectionId);
            DSPhysicalExam.PatientPhysicalExamSystemSectionRow[] arrNormalSystems = (DSPhysicalExam.PatientPhysicalExamSystemSectionRow[])dsPhysicalExam.PatientPhysicalExamSystemSection.Select(dsPhysicalExam.PatientPhysicalExamSystemSection.SectionIdColumn.ColumnName + "=" + SectionId);

            if (arrNormalSystems.Length > 0)
                RowNormalSystem = arrNormalSystems[0];
            else
                RowNormalSystem = dsPhysicalExam.PatientPhysicalExamSystemSection.NewPatientPhysicalExamSystemSectionRow();

            if (RowNormalSystem != null)
            {
                RowNormalSystem.SectionId = SectionId;
                RowNormalSystem.PatientPhysicalExamSystemId = PatientPhysicalExamSystemId;


                RowNormalSystem.IsActive = true;
                if (arrNormalSystems.Length == 0)
                {
                    RowNormalSystem.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowNormalSystem.CreatedOn = DateTime.Now;
                }
                RowNormalSystem.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                RowNormalSystem.ModifiedOn = DateTime.Now;

                if (arrNormalSystems.Length < 1)
                    dsPhysicalExam.PatientPhysicalExamSystemSection.AddPatientPhysicalExamSystemSectionRow(RowNormalSystem);
            }

            BLObject<DSPhysicalExam> objInsertedNormalSystem = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystemSection(dsPhysicalExam, null);
            if (objInsertedNormalSystem.Data != null)
            {
                DSPhysicalExam dsSavedSystem = objInsertedNormalSystem.Data;

                DSPhysicalExam.PatientPhysicalExamSystemSectionRow[] row = (DSPhysicalExam.PatientPhysicalExamSystemSectionRow[])dsPhysicalExam.PatientPhysicalExamSystemSection.Select(dsPhysicalExam.PatientPhysicalExamSystemSection.SectionIdColumn.ColumnName + "=" + SectionId);
                return MDVUtility.ToInt64(row[0][dsPhysicalExam.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName]);

                //int Count = dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamSystemSection.TableName].Rows.Count;
                //if (Count > 0)
                //    return MDVUtility.ToInt64(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamSystemSection.TableName].Rows[Count - 1][dsPhysicalExam.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName]);
                //else
                //    return 0;
            }
            else
                return 0;
        }

        /// <summary>
        /// Module Name: InsertSystem
        /// Author: Farooq Ahmad
        /// Created Date: 17-06-2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private long InsertSystem(long PhysicalExamId, PhysicalExamSystemModel model)
        {
            DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
            BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystem(PhysicalExamId, 0);
            dsPhysicalExam = obj.Data;

            DSPhysicalExam.PatientPhysicalExamSystemRow RowNormalSystem = null;
            long SystemId = MDVUtility.ToInt64(model.SystemId);
            DSPhysicalExam.PatientPhysicalExamSystemRow[] arrNormalSystems = (DSPhysicalExam.PatientPhysicalExamSystemRow[])dsPhysicalExam.PatientPhysicalExamSystem.Select(dsPhysicalExam.PatientPhysicalExamSystem.SystemIdColumn.ColumnName + "=" + SystemId);

            if (arrNormalSystems.Length > 0)
                RowNormalSystem = arrNormalSystems[0];
            else
            {
                dsPhysicalExam = new DSPhysicalExam();
                RowNormalSystem = dsPhysicalExam.PatientPhysicalExamSystem.NewPatientPhysicalExamSystemRow();
            }


            if (RowNormalSystem != null)
            {
                RowNormalSystem.SystemId = SystemId;
                RowNormalSystem.PatientPhysicalExamId = PhysicalExamId;
                if (string.IsNullOrEmpty(model.isNormal))
                    model.isNormal = "false";

                try { RowNormalSystem.IsNormal = Convert.ToBoolean(model.isNormal); }
                catch (Exception ex) { }

                RowNormalSystem.IsActive = true;
                if (arrNormalSystems.Length == 0)
                {
                    RowNormalSystem.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowNormalSystem.CreatedOn = DateTime.Now;
                }
                RowNormalSystem.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                RowNormalSystem.ModifiedOn = DateTime.Now;


                if (Convert.ToBoolean(model.isNormal))
                {
                    RowNormalSystem.NormalComments = model.Comments;
                    RowNormalSystem[dsPhysicalExam.PatientPhysicalExamSystem.SoapTextColumn] = "This system is Normal.";
                }
                else
                    RowNormalSystem[dsPhysicalExam.PatientPhysicalExamSystem.SoapTextColumn] = DBNull.Value;

                if (arrNormalSystems.Length < 1)
                    dsPhysicalExam.PatientPhysicalExamSystem.AddPatientPhysicalExamSystemRow(RowNormalSystem);
            }

            BLObject<DSPhysicalExam> objInsertedNormalSystem = BLLClinicalObj.InsertUpdatePatientPhysicalExamSystem(dsPhysicalExam);
            if (objInsertedNormalSystem.Data != null)
            {
                DSPhysicalExam dsSavedSystem = objInsertedNormalSystem.Data;
                UpdateSoapTextForPatientPhysicalExam(0, 0, 0, 0, PhysicalExamId);
                foreach (DataRow drPatPhysExamSystem in dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamSystem.TableName].Rows)
                {
                    if (Convert.ToBoolean(drPatPhysExamSystem[dsPhysicalExam.PatientPhysicalExamSystem.IsNormalColumn.ColumnName]) == true)
                    {
                        Int64 patPhysExamSysId = MDVUtility.ToInt64(drPatPhysExamSystem[dsPhysicalExam.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]);
                        string isDeleted = deletePatientPhysicalExamSystemSection(PhysicalExamId, patPhysExamSysId);
                    }
                }
                var output = (DSPhysicalExam.PatientPhysicalExamSystemRow[])dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExamSystem.TableName].Select(dsPhysicalExam.PatientPhysicalExamSystem.SystemIdColumn.ColumnName + "=" + SystemId);

                int Count = output.Count(); //Result must always be one record
                if (Count > 0)
                    return MDVUtility.ToInt64(output[Count - 1][dsPhysicalExam.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]);
                else
                    return 0;
            }
            else
                return 0;
        }

        public string updatePatientPhysicalExam(PatientPhysicalExamModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.PatientPhysicalExamId) > 0)
                {

                    DSPhysicalExam dsPhysicalExam = new DSPhysicalExam();
                    BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExam(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.PatientPhysicalExamId), MDVUtility.ToInt64(model.NotesId));
                    dsPhysicalExam = obj.Data;
                    foreach (DSPhysicalExam.PatientPhysicalExamRow dr in dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows)
                    {
                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                        if (!string.IsNullOrEmpty(model.PatientPhysicalExamDate))
                        {
                            dr.PatientPhysicalExamDate = MDVUtility.ToDateTime(model.PatientPhysicalExamDate);
                        }
                        else
                        {
                            dr[dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.Comments))
                        {
                            dr.Comments = MDVUtility.ToStr(model.Comments);
                        }
                        else
                        {
                            dr[dsPhysicalExam.PatientPhysicalExam.CommentsColumn] = DBNull.Value;
                        }
                        dr.NoteId = model.NotesId;
                        dr.TemplateId = model.TemplateId;
                        dr.bNormalExam = model.bNormal.Value;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    if (dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows.Count > 0)
                    {
                        BLObject<DSPhysicalExam> objUpdate = BLLClinicalObj.UpdatePatientPhysicalExam(dsPhysicalExam, model.NormalExamsDetail);


                        var systemId = "";
                        Int64 PhysicalExamId = MDVUtility.ToInt64(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows[0][dsPhysicalExam.PatientPhysicalExam.PatientPhysicalExamIdColumn.ColumnName]);
                        Int64 NoteId = MDVUtility.ToInt64(dsPhysicalExam.Tables[dsPhysicalExam.PatientPhysicalExam.TableName].Rows[0][dsPhysicalExam.PatientPhysicalExam.NoteIdColumn.ColumnName]);
                        if (PhysicalExamId > 0)
                        {
                            DSPhysicalExam dsPhysicalExam2 = new DSPhysicalExam();
                            string currentPatPhysSysId = string.Empty;

                            #region Delete System If Not In JSON

                            BLObject<DSPhysicalExam> objExam2 = BLLClinicalObj.LoadPatientPhysicalExamSystem(PhysicalExamId, 0);
                            dsPhysicalExam2 = objExam2.Data;
                            foreach (DataRow drTobeDeleted in dsPhysicalExam2.PatientPhysicalExamSystem.Rows)
                            {
                                var containInRows = false;
                                if (model.Systems != null)
                                {

                                    List<PhysicalExamSystemModel> result = model.Systems.Where(p => p.SystemId == MDVUtility.ToInt32(drTobeDeleted[dsPhysicalExam2.PatientPhysicalExamSystem.SystemIdColumn.ColumnName])).ToList<PhysicalExamSystemModel>();

                                    if (result.Count > 0)
                                    {
                                        containInRows = true;

                                        #region Delete Section If Not IN JSON

                                        BLObject<DSPhysicalExam> objSection = BLLClinicalObj.LoadPatientPhysicalExamSystemSection(MDVUtility.ToInt32(drTobeDeleted[dsPhysicalExam2.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]), 0);
                                        DSPhysicalExam dsSection = objSection.Data;

                                        foreach (DataRow drTobeDeletedSection in dsSection.PatientPhysicalExamSystemSection.Rows)
                                        {
                                            var containInRowsSection = false;
                                            if (result[0].Sections != null)
                                            {
                                                var resultSection = result[0].Sections.Where(p => p.SectionId == MDVUtility.ToStr(drTobeDeletedSection[dsPhysicalExam2.PatientPhysicalExamSystemSection.SectionIdColumn.ColumnName])).ToList();
                                                if (resultSection.Count > 0)
                                                {
                                                    containInRowsSection = true;

                                                    #region Delete Char If Not In JSON

                                                    BLObject<DSPhysicalExam> objChar = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristic(MDVUtility.ToInt32(drTobeDeletedSection[dsPhysicalExam2.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName]), 0);
                                                    DSPhysicalExam dsChar = objChar.Data;

                                                    foreach (DataRow drTobeDeletedChar in dsChar.PatientPhysicalExamSystemSectionCharacteristic.Rows)
                                                    {
                                                        var containInRowsChar = false;
                                                        if (resultSection[0].Characteristics != null)
                                                        {
                                                            var resultChar = resultSection[0].Characteristics.Where(p => p.CharacteristicId == MDVUtility.ToStr(drTobeDeletedChar[dsPhysicalExam2.PatientPhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName])).ToList();
                                                            if (resultChar.Count > 0)
                                                            {
                                                                containInRowsChar = true;

                                                                #region Delete Sub Char If Not In JSON

                                                                BLObject<DSPhysicalExam> objSubChar = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(MDVUtility.ToInt32(drTobeDeletedChar[dsPhysicalExam2.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn.ColumnName]), 0);
                                                                DSPhysicalExam dsSubChar = objSubChar.Data;

                                                                foreach (DataRow drTobeDeletedSubChar in dsSubChar.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.Rows)
                                                                {
                                                                    var containInRowsSubChar = false;
                                                                    if (resultChar[0].SubCharacteristics != null)
                                                                    {
                                                                        var resultSubChar = resultChar[0].SubCharacteristics.Where(p => p.SubCharacteristicId == MDVUtility.ToStr(drTobeDeletedSubChar[dsPhysicalExam2.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName])).ToList();
                                                                        if (resultSubChar.Count > 0)
                                                                        {
                                                                            containInRowsSubChar = true;
                                                                        }
                                                                    }
                                                                    if (!containInRowsSubChar)
                                                                    {
                                                                        string response = deletePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(PhysicalExamId, MDVUtility.ToInt64(drTobeDeletedChar[dsPhysicalExam2.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn.ColumnName]), MDVUtility.ToInt64(drTobeDeletedSubChar[dsPhysicalExam2.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn.ColumnName]));
                                                                    }
                                                                }

                                                                #endregion

                                                            }
                                                        }
                                                        if (!containInRowsChar)
                                                        {
                                                            string response = deletePatientPhysicalExamSystemSectionCharacteristic(PhysicalExamId, MDVUtility.ToInt64(drTobeDeletedSection[dsPhysicalExam.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName]), MDVUtility.ToInt64(drTobeDeletedChar[dsPhysicalExam2.PatientPhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName]));
                                                        }
                                                    }

                                                    #endregion

                                                }
                                            }
                                            if (!containInRowsSection)
                                            {
                                                string response = deletePatientPhysicalExamSystemSection(PhysicalExamId, MDVUtility.ToInt32(drTobeDeleted[dsPhysicalExam2.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]), MDVUtility.ToInt64(drTobeDeletedSection[dsPhysicalExam2.PatientPhysicalExamSystemSection.SectionIdColumn.ColumnName]));
                                            }
                                        }

                                        #endregion
                                    }
                                    if (!containInRows)
                                    {
                                        string response = deletePatientPhysicalExamSystem(PhysicalExamId, MDVUtility.ToInt64(drTobeDeleted[dsPhysicalExam.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]));
                                    }
                                    foreach (PhysicalExamSystemModel modelObj in result)
                                    {
                                        PatientPhysicalExamSystemModel PatmodelObj = new PatientPhysicalExamSystemModel()
                                        {
                                            Comments = modelObj.Comments,
                                            IsNormal = string.IsNullOrEmpty(modelObj.isNormal) ? false : MDVUtility.ToBool(modelObj.isNormal),
                                            //  PatientPhysicalExamId = MDVUtility.ToStr(modelObj.PatientPhysicalExamId),
                                            SystemId = modelObj.ShortName
                                        };
                                        long PatientPhysicalExamSystemId = MDVUtility.ToInt64(drTobeDeleted[dsPhysicalExam.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]);
                                        if (PatientPhysicalExamSystemId > 0)
                                        {
                                            updatePatientPhysicalExamSystem(PatmodelObj, PatientPhysicalExamSystemId);
                                        }

                                    }

                                }

                            }

                            #endregion

                            BLLClinicalObj.insertUpdateSoapTextForPhysicalExamAndChild(MDVUtility.ToInt64(PhysicalExamId), model.TemplateId);
                            insertUpdateSysSecCharSubChar(PhysicalExamId, model);

                            var objSoapText = BLLClinicalObj.insertUpdateSoapTextForPhysicalExamAndChild(MDVUtility.ToInt64(PhysicalExamId), model.TemplateId);

                        }
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,
                                PhysicalExamId = PhysicalExamId,
                                NoteId = NoteId
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
                        message = "Patient Physical Exam not found."
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

        /// <summary>
        /// Author: Ahmad Raza
        /// Date: 24-06-2016
        /// Function Name: GetSerializedTemplateData
        /// Description: This function will load data for Data Templates
        /// </summary>
        /// <param name="dataTemplateId"></param>
        /// <returns></returns>
        /// 

        public string GetSerializedTemplateData(long physExamDataTemplateId)
        {
            List<PatientPhysicalExamSystemModel> patientPhysicalExamSystems = new List<PatientPhysicalExamSystemModel>();
            try
            {
                if (physExamDataTemplateId > 0)
                {
                    //Start/ populating systems
                    DSPhysicalExamDataTemplate dsPhysicalExamDataTemplateSystem = null;
                    //loadPhysicalExamDataTemplate

                    BLObject<DSPhysicalExamDataTemplate> obj = BLLClinicalObj.LoadPhysicalExamDataTemplateSystem(physExamDataTemplateId);
                    dsPhysicalExamDataTemplateSystem = obj.Data;
                    if (dsPhysicalExamDataTemplateSystem.Tables[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.TableName].Rows.Count > 0)
                    {
                        int sysIndex = -1, secIndex = -1, charIndex = -1, subCharIndex = -1;
                        foreach (DataRow drSystem in dsPhysicalExamDataTemplateSystem.Tables[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.TableName].Rows)
                        {

                            //To Store the System 
                            PatientPhysicalExamSystemModel system = new PatientPhysicalExamSystemModel();
                            long phyExamDataTemplateSystemId = MDVUtility.ToInt64(drSystem[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.DataTemplateSysIdColumn.ColumnName]);
                            long systemId = MDVUtility.ToInt64(drSystem[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.SystemIdColumn.ColumnName]);
                            if (phyExamDataTemplateSystemId > 0)
                            {
                                //system.DataTemplateId = physExamDataTemplateId;
                                //system.PhysicalExamSystemId = phyExamDataTemplateSystemId;
                                system.SystemId = MDVUtility.ToStr(systemId);
                                system.IsNormal = Convert.ToBoolean(drSystem[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.IsNormalColumn.ColumnName]);
                                system.NormalComments = string.Empty;
                                system.PatientPhysicalExamId = MDVUtility.ToStr(sysIndex);
                                system.PhysicalExamSystemId = MDVUtility.ToStr(sysIndex);
                                system.PhysicalExamDataTemplateSystemId = MDVUtility.ToStr(drSystem[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.DataTemplateSysIdColumn.ColumnName]);

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
                                        PatientPhysicalExamSystemSectionModel section = new PatientPhysicalExamSystemSectionModel();

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
                                                PatientPhysicalExamCharacteristicModel charicteristics = new PatientPhysicalExamCharacteristicModel();

                                                charicteristics.SectionCharacteristicDetailModel = new PatientPhysicalExamCharacteristicDetailModel();

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

                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamAgggravatedby = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.AggravatedByIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamAgggravatedby_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Agggravatedby_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamAssociatedwith = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.AssociatedWithColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamCharacter = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.CharacterIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamCharacter_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Character_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamContext = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.ContextIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamContext_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Context_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamCourse = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.CourseIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamCourse_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Course_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamDurationLength = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.DurationLengthColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamDurationPeriod = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.DurationPeriodIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamDurationPeriod_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.DurationPeriod_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamFrequency = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.FrequencyIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamFrequency_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Frequency_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamLocation = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.LocationColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamOnset = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.OnsetColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamPattern = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.PatternIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamPattern_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Pattern_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamPercipitatedby = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.PrecipitatedbyColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamPreviousHistory = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.PrevHistoryColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamRadiation = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.RadiationIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamRadiation_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Radiation_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamRelievedby = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.RelievedbyIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamRelievedby_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Relievedby_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamSeverity = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.SeverityIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamSeverity_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Severity_textColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamStatus = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.StatusIdColumn.ColumnName]);
                                                    charicteristics.SectionCharacteristicDetailModel.PhysicalExamStatus_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateDetail.Status_textColumn.ColumnName]);


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

                                                        PatientPhysicalExamSubCharacteristicModel charSubCharacteristic = new PatientPhysicalExamSubCharacteristicModel();

                                                        BLObject<DSPhysicalExamDataTemplate> objSectionSubCharacteristicDetail = BLLClinicalObj.LoadPhysicalExamDataTemplateDetail(physExamDataTemplateId, SubCharacteristicId, SubCharacteristic, 0);
                                                        dsPhysicalExamSectionSubCharacteristic = objSectionSubCharacteristicDetail.Data;
                                                        charSubCharacteristic.SubCharacteristicDetailModel = new PatientPhysicalExamSubCharacteristicDetailModel();

                                                        // Fill detail
                                                        foreach (DataRow drSectionSubCharacteristicDetail in dsPhysicalExamSectionSubCharacteristic.Tables[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.TableName].Rows)
                                                        {
                                                            charSubCharacteristic.SubCharacteristicDetailModel.DetailId = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.DetailIdColumn.ColumnName]);

                                                            //PatientPhysicalExamSubCharacteristicDetailModel subCharDetail = new PatientPhysicalExamSubCharacteristicDetailModel();
                                                            charSubCharacteristic.SubCharacteristicDetailModel.SubCharacteristicId = MDVUtility.ToStr(SubCharacteristicId);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.IsSubCharacteristicPositive = Convert.ToBoolean(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateSubChar.IsPositiveColumn.ColumnName]) ? "True" : "False";

                                                            charSubCharacteristic.SubCharacteristicDetailModel.CharacteristicId = MDVUtility.ToStr(characteristicId);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.IsCharacteristicPositive = isCharPositive;

                                                            charSubCharacteristic.SubCharacteristicDetailModel.SectionId = MDVUtility.ToStr(sectionId);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.SystemId = MDVUtility.ToStr(systemId);

                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamAgggravatedby = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.AggravatedByIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamAgggravatedby_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Agggravatedby_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamAssociatedwith = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.AssociatedWithColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamCharacter = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.CharacterIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamCharacter_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Character_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamContext = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.ContextIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamContext_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Context_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamCourse = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.CourseIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamCourse_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Course_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamDurationLength = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.DurationLengthColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamDurationPeriod = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.DurationPeriodIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamDurationPeriod_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.DurationPeriod_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamFrequency = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.FrequencyIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamFrequency_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Frequency_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamLocation = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.LocationColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamOnset = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.OnsetColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamPattern = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.PatternIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamPattern_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Pattern_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamPercipitatedby = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.PrecipitatedbyColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamPreviousHistory = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.PrevHistoryColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamRadiation = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.RadiationIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamRadiation_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Radiation_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamRelievedby = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.RelievedbyIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamRelievedby_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Relievedby_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamSeverity = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.SeverityIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamSeverity_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Severity_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamStatus = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.StatusIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamStatus_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateDetail.Status_textColumn.ColumnName]);
                                                        }


                                                        charSubCharacteristic.PatientPhysicalSubCharacteristicId = MDVUtility.ToStr(subCharIndex);
                                                        charSubCharacteristic.SubCharacteristicId = MDVUtility.ToStr(SubCharacteristicId);
                                                        charSubCharacteristic.Comments = MDVUtility.ToStr(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateSubChar.CommentsColumn.ColumnName]);
                                                        charSubCharacteristic.IsPositive = MDVUtility.ToStr(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PhysExamDataTemplateSubChar.IsPositiveColumn.ColumnName]);



                                                        if (charicteristics.SubCharacteristics == null)
                                                            charicteristics.SubCharacteristics = new List<PatientPhysicalExamSubCharacteristicModel>();
                                                        charicteristics.SubCharacteristics.Add(charSubCharacteristic);
                                                        subCharIndex--;
                                                    }
                                                }
                                                //End/ populating SubCharacteristics



                                                charicteristics.Comments = MDVUtility.ToStr(drSectionCharacteristic[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateChar.CommentsColumn.ColumnName]);
                                                charicteristics.IsPositive = MDVUtility.ToStr(drSectionCharacteristic[dsPhysicalExamDataTemplateChar.PhysExamDataTemplateChar.IsPositiveColumn.ColumnName]);

                                                charicteristics.PhysicalExamCharacteristicId = MDVUtility.ToStr(charIndex);
                                                charicteristics.SectionCharacteristicId = MDVUtility.ToStr(characteristicId);
                                                if (section.Characteristics == null)
                                                    section.Characteristics = new List<PatientPhysicalExamCharacteristicModel>();
                                                section.Characteristics.Add(charicteristics);
                                                charIndex--;
                                            }
                                            //End/ populating Characteristics


                                            section.PatientPhysicalExamSystemSectionId = MDVUtility.ToStr(secIndex);
                                            section.SectionId = MDVUtility.ToStr(sectionId);
                                            section.PhysicalExamSectionId = MDVUtility.ToStr(secIndex);
                                            if (system.Sections == null)
                                                system.Sections = new List<PatientPhysicalExamSystemSectionModel>();
                                            system.Sections.Add(section);
                                            secIndex--;
                                        }
                                    }

                                }
                                if (MDVUtility.ToStr(drSystem[dsPhysicalExamDataTemplateSystem.PhysExamDataTemplateSys.IsNormalColumn.ColumnName]).ToLower() == "true")
                                {
                                    system.IsNormal = true;
                                    // system.Comments = MDVUtility.ToStr(drSystem[dsPhysicalExamDataTemplate.PhysExamDataTemplateSys.NormalCommentsColumn.ColumnName]);
                                }
                                else
                                    system.IsNormal = false;
                                patientPhysicalExamSystems.Add(system);
                                sysIndex--;
                                //End/ populating sections
                            }

                        }
                    }

                    DSPhysicalExamDataTemplate dsPhysicalExamDataTemplateMain = new DSPhysicalExamDataTemplate();
                    BLObject<DSPhysicalExamDataTemplate> objMain = BLLClinicalObj.loadPhysicalExamDataTemplate(physExamDataTemplateId, 0);
                    dsPhysicalExamDataTemplateMain = objMain.Data;
                    if (dsPhysicalExamDataTemplateMain == null)
                        dsPhysicalExamDataTemplateMain = new DSPhysicalExamDataTemplate();

                    PatientPhysicalExamModel mainModel = new PatientPhysicalExamModel();

                    if (dsPhysicalExamDataTemplateMain.Tables[dsPhysicalExamDataTemplateMain.PhysExamDataTemplate.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPhysicalExamDataTemplateMain.Tables[dsPhysicalExamDataTemplateMain.PhysExamDataTemplate.TableName].Rows[0];
                        mainModel.Comments = MDVUtility.ToStr(dr[dsPhysicalExamDataTemplateMain.PhysExamDataTemplate.CommentsColumn.ColumnName]);
                    }
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    Dictionary<string, string> ObjResult = new Dictionary<string, string>();
                    ObjResult["patientPhysicalExamSystems"] = js.Serialize(patientPhysicalExamSystems);
                    ObjResult["Comments"] = mainModel.Comments;
                    //End/ populating systems

                    return js.Serialize(ObjResult);
                }
            }
            catch (Exception ex)
            {

            }
            return "";
        }




    }

    #region container classes for Loading physical exam
    /// <summary>
    /// List Names
    /// </summary>
    public static class ListName
    {
        const string characteristics = "Characteristic";
        const string subCharacteristic = "SubCharacteristic";
        public static string Characteristics { get { return characteristics; } }
        public static string SubCharacteristic { get { return subCharacteristic; } }
    }

    /// <summary>
    /// Author : Abid Ali
    /// Desc : system container class
    /// </summary>   
    public class PatientPhysicalSystem
    {
        public long Id { get; set; }
        public long SystemId { get; set; }
        public List<PatientPhysicalSystemSection> Sections { get; set; }
        public bool IsNormal { get; set; }

        public string Comments { get; set; }

        private BLLClinical BLLClinicalObj = null;

        public PatientPhysicalSystem()
        {
            Sections = new List<PatientPhysicalSystemSection> { new PatientPhysicalSystemSection() };
            BLLClinicalObj = new BLLClinical();
        }


        //Author: Abid Ali
        //Created Date: 18/02/2016
        //OverView: get serialized systems and childs data
        public string GetSerializedSystemsAndChildsData(long patientPysicalExamId)
        {
            List<PatientPhysicalSystem> patientPhysicalExamSystems = new List<PatientPhysicalSystem>();

            if (patientPysicalExamId > 0)
            {
                //Start/ populating systems
                DSPhysicalExam dsPhysicalExamSystem = null;
                BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystem(MDVUtility.ToInt64(patientPysicalExamId), 0);
                dsPhysicalExamSystem = obj.Data;
                if (dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName].Rows.Count > 0)
                {

                    foreach (DataRow drSystem in dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName].Rows)
                    {
                        PatientPhysicalSystem system = new PatientPhysicalSystem();
                        long physicalExamSystemId = MDVUtility.ToInt64(drSystem[dsPhysicalExamSystem.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]);
                        long systemId = MDVUtility.ToInt64(drSystem[dsPhysicalExamSystem.PatientPhysicalExamSystem.SystemIdColumn.ColumnName]);
                        if (physicalExamSystemId > 0)
                        {
                            system.SystemId = systemId;
                            system.Id = physicalExamSystemId;
                            //Start/ populating sections
                            DSPhysicalExam dsPhysicalExamSection = null;
                            BLObject<DSPhysicalExam> objSystemSection = BLLClinicalObj.LoadPatientPhysicalExamSystemSection(physicalExamSystemId, 0);
                            dsPhysicalExamSection = objSystemSection.Data;
                            if (dsPhysicalExamSection.Tables[dsPhysicalExamSection.PatientPhysicalExamSystemSection.TableName].Rows.Count > 0)
                            {
                                foreach (DataRow drSection in dsPhysicalExamSection.Tables[dsPhysicalExamSection.PatientPhysicalExamSystemSection.TableName].Rows)
                                {
                                    long physicalExamSectionId = MDVUtility.ToInt64(drSection[dsPhysicalExamSection.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName]);
                                    long sectionId = MDVUtility.ToInt64(drSection[dsPhysicalExamSection.PatientPhysicalExamSystemSection.SectionIdColumn.ColumnName]);
                                    PatientPhysicalSystemSection section = new PatientPhysicalSystemSection();

                                    //Start/ populating Characteristics
                                    DSPhysicalExam dsPhysicalExamSectionCharacteristic = null;
                                    BLObject<DSPhysicalExam> objSectionCharacteristic = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristic(physicalExamSectionId, 0);
                                    dsPhysicalExamSectionCharacteristic = objSectionCharacteristic.Data;
                                    if (dsPhysicalExamSectionCharacteristic.Tables[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.TableName].Rows.Count > 0)
                                    {
                                        foreach (DataRow drSectionCharacteristic in dsPhysicalExamSectionCharacteristic.Tables[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.TableName].Rows)
                                        {
                                            long sectionCharacteristicId = MDVUtility.ToInt64(drSectionCharacteristic[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn.ColumnName]);
                                            long characteristicId = MDVUtility.ToInt64(drSectionCharacteristic[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName]);
                                            string isCharPositive = Convert.ToBoolean(drSectionCharacteristic[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.IsPositiveColumn.ColumnName]) ? "True" : "False";
                                            PatientPhysicalSectionCharacteristic sectionCharacteristic = new PatientPhysicalSectionCharacteristic();
                                            sectionCharacteristic.IsDetailExists = false;
                                            BLObject<DSPhysicalExam> objSectionCharacteristicDetail = BLLClinicalObj.LoadPatientPhysicalExamDetail(patientPysicalExamId, sectionCharacteristicId, ListName.Characteristics, 0);
                                            dsPhysicalExamSectionCharacteristic = objSectionCharacteristicDetail.Data;

                                            // Fill detail
                                            foreach (DataRow drSectionCharacteristicDetail in dsPhysicalExamSectionCharacteristic.Tables[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.TableName].Rows)
                                            {
                                                sectionCharacteristic.IsDetailExists = true;
                                                //PatientPhysicalExamCharacteristicDetailModel charDetail = new PatientPhysicalExamCharacteristicDetailModel();


                                                sectionCharacteristic.SectionCharacteristicDetailModel.IsCharacteristicPositive = isCharPositive;

                                                sectionCharacteristic.SectionCharacteristicDetailModel.SectionId = MDVUtility.ToStr(sectionId);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.SystemId = MDVUtility.ToStr(systemId);

                                                sectionCharacteristic.SectionCharacteristicDetailModel.CharacteristicId = MDVUtility.ToStr(characteristicId);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamAgggravatedby = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                //charDetail.PhysicalExamAgggravatedby_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail..ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamAssociatedwith = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.AssociatedWithColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamCharacter = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.CharacterIdColumn.ColumnName]);
                                                //charDetail.PhysicalExamCharacter_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamContext = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.ContextIdColumn.ColumnName]);
                                                // charDetail.PhysicalExamContext_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamCourse = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.CourseIdColumn.ColumnName]);
                                                // charDetail.PhysicalExamCourse_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamDurationLength = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.DurationLengthColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamDurationPeriod = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.DurationPeriodIdColumn.ColumnName]);
                                                // charDetail.PhysicalExamDurationPeriod_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamFrequency = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.FrequencyIdColumn.ColumnName]);
                                                // charDetail.PhysicalExamFrequency_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.FrequencyIdColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamLocation = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.LocationColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamOnset = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.OnsetColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamPattern = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.PatternIdColumn.ColumnName]);
                                                // charDetail.PhysicalExamPattern_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.ParentIdColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamPercipitatedby = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.PrecipitatedbyColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamPreviousHistory = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.PrevHistoryColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamRadiation = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.RadiationIdColumn.ColumnName]);
                                                //  charDetail.PhysicalExamRadiation_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamRelievedby = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.RelievedbyIdColumn.ColumnName]);
                                                // charDetail.PhysicalExamRelievedby_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.OnsetColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamSeverity = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.SeverityIdColumn.ColumnName]);
                                                //charDetail.PhysicalExamSeverity_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.ParentIdColumn.ColumnName]);
                                                sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamStatus = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.StatusIdColumn.ColumnName]);


                                            }

                                            //Start/ populating SubCharacteristics
                                            DSPhysicalExam dsPhysicalExamSectionSubCharacteristic = null;
                                            BLObject<DSPhysicalExam> objSectionSubCharacteristic = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(sectionCharacteristicId, 0);
                                            dsPhysicalExamSectionSubCharacteristic = objSectionSubCharacteristic.Data;
                                            if (dsPhysicalExamSectionSubCharacteristic.Tables[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName].Rows.Count > 0)
                                            {
                                                foreach (DataRow drSectionSubCharacteristic in dsPhysicalExamSectionSubCharacteristic.Tables[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName].Rows)
                                                {
                                                    long sectionSubCharacteristicId = MDVUtility.ToInt64(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn.ColumnName]);
                                                    long SubCharacteristicId = MDVUtility.ToInt64(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName]);
                                                    PatientPhysicalCharSubCharacteristic charSubCharacteristic = new PatientPhysicalCharSubCharacteristic();

                                                    BLObject<DSPhysicalExam> objSectionSubCharacteristicDetail = BLLClinicalObj.LoadPatientPhysicalExamDetail(patientPysicalExamId, sectionSubCharacteristicId, ListName.SubCharacteristic, 0);
                                                    dsPhysicalExamSectionSubCharacteristic = objSectionSubCharacteristicDetail.Data;
                                                    charSubCharacteristic.IsDetailExists = false;
                                                    // Fill detail
                                                    foreach (DataRow drSectionSubCharacteristicDetail in dsPhysicalExamSectionSubCharacteristic.Tables[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.TableName].Rows)
                                                    {
                                                        charSubCharacteristic.IsDetailExists = true;

                                                        //PatientPhysicalExamSubCharacteristicDetailModel subCharDetail = new PatientPhysicalExamSubCharacteristicDetailModel();
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.SubCharacteristicId = MDVUtility.ToStr(SubCharacteristicId);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.IsSubCharacteristicPositive = Convert.ToBoolean(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.IsPositiveColumn.ColumnName]) ? "True" : "False";

                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.CharacteristicId = MDVUtility.ToStr(characteristicId);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.IsCharacteristicPositive = isCharPositive;

                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.SectionId = MDVUtility.ToStr(sectionId);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.SystemId = MDVUtility.ToStr(systemId);

                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamAgggravatedby = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                        //subCharDetail.PhysicalExamAgggravatedby_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail..ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamAssociatedwith = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.AssociatedWithColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamCharacter = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.CharacterIdColumn.ColumnName]);
                                                        //subCharDetail.PhysicalExamCharacter_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamContext = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.ContextIdColumn.ColumnName]);
                                                        // subCharDetail.PhysicalExamContext_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamCourse = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.CourseIdColumn.ColumnName]);
                                                        // subCharDetail.PhysicalExamCourse_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamDurationLength = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.DurationLengthColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamDurationPeriod = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.DurationPeriodIdColumn.ColumnName]);
                                                        // subCharDetail.PhysicalExamDurationPeriod_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamFrequency = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.FrequencyIdColumn.ColumnName]);
                                                        // subCharDetail.PhysicalExamFrequency_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.FrequencyIdColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamLocation = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.LocationColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamOnset = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.OnsetColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamPattern = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.PatternIdColumn.ColumnName]);
                                                        // subCharDetail.PhysicalExamPattern_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.ParentIdColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamPercipitatedby = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.PrecipitatedbyColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamPreviousHistory = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.PrevHistoryColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamRadiation = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.RadiationIdColumn.ColumnName]);
                                                        //  subCharDetail.PhysicalExamRadiation_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamRelievedby = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.RelievedbyIdColumn.ColumnName]);
                                                        // subCharDetail.PhysicalExamRelievedby_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.OnsetColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamSeverity = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.SeverityIdColumn.ColumnName]);
                                                        //subCharDetail.PhysicalExamSeverity_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.ParentIdColumn.ColumnName]);
                                                        charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.PhysicalExamStatus = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.StatusIdColumn.ColumnName]);

                                                        //  charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.Add(subCharDetail);
                                                    }


                                                    charSubCharacteristic.Id = sectionSubCharacteristicId;
                                                    charSubCharacteristic.CharSubCharacteristicId = SubCharacteristicId;
                                                    charSubCharacteristic.Comments = MDVUtility.ToStr(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.CommentsColumn.ColumnName]);
                                                    charSubCharacteristic.IsPositive = MDVUtility.ToStr(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.IsPositiveColumn.ColumnName]) == "True" ? true : false;
                                                    sectionCharacteristic.SubCharacteristics.Add(charSubCharacteristic);
                                                }
                                            }
                                            //End/ populating SubCharacteristics

                                            sectionCharacteristic.Comments = MDVUtility.ToStr(drSectionCharacteristic[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.CommentsColumn.ColumnName]);
                                            sectionCharacteristic.IsPositive = MDVUtility.ToStr(drSectionCharacteristic[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.IsPositiveColumn.ColumnName]) == "True" ? true : false;

                                            sectionCharacteristic.Id = sectionCharacteristicId;
                                            sectionCharacteristic.SectionCharacteristicId = characteristicId;
                                            section.Characteristics.Add(sectionCharacteristic);
                                        }
                                        //End/ populating Characteristics
                                        section.Id = physicalExamSectionId;
                                        section.SectionId = sectionId;
                                        system.Sections.Add(section);
                                    }
                                }

                            }
                            if (MDVUtility.ToStr(drSystem[dsPhysicalExamSystem.PatientPhysicalExamSystem.IsNormalColumn.ColumnName]).ToLower() == "true")
                            {
                                system.IsNormal = true;
                                system.Comments = MDVUtility.ToStr(drSystem[dsPhysicalExamSystem.PatientPhysicalExamSystem.NormalCommentsColumn.ColumnName]);
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
            return "";
        }



        public string GetSerializedData(long patientPysicalExamId)
        {
            List<PatientPhysicalExamSystemModel> patientPhysicalExamSystems = new List<PatientPhysicalExamSystemModel>();
            try
            {
                if (patientPysicalExamId > 0)
                {
                    //Start/ populating systems
                    DSPhysicalExam dsPhysicalExamSystem = null;
                    BLObject<DSPhysicalExam> obj = BLLClinicalObj.LoadPatientPhysicalExamSystem(MDVUtility.ToInt64(patientPysicalExamId), 0);
                    dsPhysicalExamSystem = obj.Data;
                    if (dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName].Rows.Count > 0)
                    {

                        foreach (DataRow drSystem in dsPhysicalExamSystem.Tables[dsPhysicalExamSystem.PatientPhysicalExamSystem.TableName].Rows)
                        {

                            //To Store the System 
                            PatientPhysicalExamSystemModel system = new PatientPhysicalExamSystemModel();
                            long physicalExamSystemId = MDVUtility.ToInt64(drSystem[dsPhysicalExamSystem.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]);
                            long systemId = MDVUtility.ToInt64(drSystem[dsPhysicalExamSystem.PatientPhysicalExamSystem.SystemIdColumn.ColumnName]);
                            if (physicalExamSystemId > 0)
                            {
                                system.SystemId = MDVUtility.ToStr(systemId);
                                system.IsNormal = Convert.ToBoolean(drSystem[dsPhysicalExamSystem.PatientPhysicalExamSystem.IsNormalColumn.ColumnName]);
                                system.NormalComments = MDVUtility.ToStr(drSystem[dsPhysicalExamSystem.PatientPhysicalExamSystem.CommentsColumn.ColumnName]);
                                system.PatientPhysicalExamId = MDVUtility.ToStr(physicalExamSystemId);
                                system.PhysicalExamSystemId = MDVUtility.ToStr(drSystem[dsPhysicalExamSystem.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName]);
                                //Start/ populating sections
                                DSPhysicalExam dsPhysicalExamSection = null;
                                BLObject<DSPhysicalExam> objSystemSection = BLLClinicalObj.LoadPatientPhysicalExamSystemSection(physicalExamSystemId, 0);
                                dsPhysicalExamSection = objSystemSection.Data;
                                if (dsPhysicalExamSection.Tables[dsPhysicalExamSection.PatientPhysicalExamSystemSection.TableName].Rows.Count > 0)
                                {
                                    foreach (DataRow drSection in dsPhysicalExamSection.Tables[dsPhysicalExamSection.PatientPhysicalExamSystemSection.TableName].Rows)
                                    {
                                        long physicalExamSectionId = MDVUtility.ToInt64(drSection[dsPhysicalExamSection.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName]);
                                        long sectionId = MDVUtility.ToInt64(drSection[dsPhysicalExamSection.PatientPhysicalExamSystemSection.SectionIdColumn.ColumnName]);


                                        //To Store the Section
                                        PatientPhysicalExamSystemSectionModel section = new PatientPhysicalExamSystemSectionModel();

                                        //Start/ populating Characteristics
                                        DSPhysicalExam dsPhysicalExamSectionCharacteristic = null;
                                        BLObject<DSPhysicalExam> objSectionCharacteristic = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristic(physicalExamSectionId, 0);
                                        dsPhysicalExamSectionCharacteristic = objSectionCharacteristic.Data;

                                        if (dsPhysicalExamSectionCharacteristic.Tables[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.TableName].Rows.Count > 0)
                                        {
                                            foreach (DataRow drSectionCharacteristic in dsPhysicalExamSectionCharacteristic.Tables[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.TableName].Rows)
                                            {
                                                long sectionCharacteristicId = MDVUtility.ToInt64(drSectionCharacteristic[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn.ColumnName]);
                                                long characteristicId = MDVUtility.ToInt64(drSectionCharacteristic[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName]);
                                                string isCharPositive = Convert.ToBoolean(drSectionCharacteristic[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.IsPositiveColumn.ColumnName]) ? "True" : "False";

                                                //To Store Characteristics
                                                PatientPhysicalExamCharacteristicModel sectionCharacteristic = new PatientPhysicalExamCharacteristicModel();

                                                sectionCharacteristic.SectionCharacteristicDetailModel = new PatientPhysicalExamCharacteristicDetailModel();

                                                BLObject<DSPhysicalExam> objSectionCharacteristicDetail = BLLClinicalObj.LoadPatientPhysicalExamDetail(patientPysicalExamId, sectionCharacteristicId, ListName.Characteristics, 0);
                                                dsPhysicalExamSectionCharacteristic = objSectionCharacteristicDetail.Data;

                                                // Fill detail
                                                foreach (DataRow drSectionCharacteristicDetail in dsPhysicalExamSectionCharacteristic.Tables[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.TableName].Rows)
                                                {

                                                    sectionCharacteristic.SectionCharacteristicDetailModel.DetailId = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.DetailIdColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.IsCharacteristicPositive = isCharPositive;

                                                    sectionCharacteristic.SectionCharacteristicDetailModel.SectionId = MDVUtility.ToStr(sectionId);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.SystemId = MDVUtility.ToStr(systemId);

                                                    sectionCharacteristic.SectionCharacteristicDetailModel.CharacteristicId = MDVUtility.ToStr(characteristicId);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamAgggravatedby = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamAgggravatedby_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.Agggravatedby_textColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamAssociatedwith = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.AssociatedWithColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamCharacter = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.CharacterIdColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamCharacter_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.Character_textColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamContext = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.ContextIdColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamContext_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.Context_textColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamCourse = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.CourseIdColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamCourse_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.Course_textColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamDurationLength = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.DurationLengthColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamDurationPeriod = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.DurationPeriodIdColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamDurationPeriod_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.DurationPeriod_textColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamFrequency = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.FrequencyIdColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamFrequency_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.Frequency_textColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamLocation = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.LocationColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamOnset = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.OnsetColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamPattern = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.PatternIdColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamPattern_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.Pattern_textColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamPercipitatedby = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.PrecipitatedbyColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamPreviousHistory = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.PrevHistoryColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamRadiation = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.RadiationIdColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamRadiation_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.Radiation_textColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamRelievedby = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.RelievedbyIdColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamRelievedby_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.Relievedby_textColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamSeverity = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.SeverityIdColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamSeverity_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.Severity_textColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamStatus = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.StatusIdColumn.ColumnName]);
                                                    sectionCharacteristic.SectionCharacteristicDetailModel.PhysicalExamStatus_text = MDVUtility.ToStr(drSectionCharacteristicDetail[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamDetail.Status_textColumn.ColumnName]);

                                                }

                                                //Start/ populating SubCharacteristics
                                                DSPhysicalExam dsPhysicalExamSectionSubCharacteristic = null;
                                                BLObject<DSPhysicalExam> objSectionSubCharacteristic = BLLClinicalObj.LoadPatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(sectionCharacteristicId, 0);
                                                dsPhysicalExamSectionSubCharacteristic = objSectionSubCharacteristic.Data;
                                                if (dsPhysicalExamSectionSubCharacteristic.Tables[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName].Rows.Count > 0)
                                                {
                                                    foreach (DataRow drSectionSubCharacteristic in dsPhysicalExamSectionSubCharacteristic.Tables[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName].Rows)
                                                    {
                                                        long sectionSubCharacteristicId = MDVUtility.ToInt64(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn.ColumnName]);
                                                        long SubCharacteristicId = MDVUtility.ToInt64(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName]);
                                                        PatientPhysicalExamSubCharacteristicModel charSubCharacteristic = new PatientPhysicalExamSubCharacteristicModel();

                                                        BLObject<DSPhysicalExam> objSectionSubCharacteristicDetail = BLLClinicalObj.LoadPatientPhysicalExamDetail(patientPysicalExamId, sectionSubCharacteristicId, ListName.SubCharacteristic, 0);
                                                        dsPhysicalExamSectionSubCharacteristic = objSectionSubCharacteristicDetail.Data;
                                                        charSubCharacteristic.SubCharacteristicDetailModel = new PatientPhysicalExamSubCharacteristicDetailModel();

                                                        // Fill detail
                                                        foreach (DataRow drSectionSubCharacteristicDetail in dsPhysicalExamSectionSubCharacteristic.Tables[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.TableName].Rows)
                                                        {
                                                            charSubCharacteristic.SubCharacteristicDetailModel.DetailId = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.DetailIdColumn.ColumnName]);

                                                            //PatientPhysicalExamSubCharacteristicDetailModel subCharDetail = new PatientPhysicalExamSubCharacteristicDetailModel();
                                                            charSubCharacteristic.SubCharacteristicDetailModel.SubCharacteristicId = MDVUtility.ToStr(SubCharacteristicId);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.IsSubCharacteristicPositive = Convert.ToBoolean(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.IsPositiveColumn.ColumnName]) ? "True" : "False";

                                                            charSubCharacteristic.SubCharacteristicDetailModel.CharacteristicId = MDVUtility.ToStr(characteristicId);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.IsCharacteristicPositive = isCharPositive;

                                                            charSubCharacteristic.SubCharacteristicDetailModel.SectionId = MDVUtility.ToStr(sectionId);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.SystemId = MDVUtility.ToStr(systemId);

                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamAgggravatedby = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamAgggravatedby_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.Agggravatedby_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamAssociatedwith = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.AssociatedWithColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamCharacter = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.CharacterIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamCharacter_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.Character_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamContext = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.ContextIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamContext_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.Context_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamCourse = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.CourseIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamCourse_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.Context_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamDurationLength = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.DurationLengthColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamDurationPeriod = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.DurationPeriodIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamDurationPeriod_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.DurationPeriod_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamFrequency = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.FrequencyIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamFrequency_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.Frequency_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamLocation = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.LocationColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamOnset = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.OnsetColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamPattern = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.PatternIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamPattern_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.Pattern_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamPercipitatedby = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.PrecipitatedbyColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamPreviousHistory = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.PrevHistoryColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamRadiation = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.RadiationIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamRadiation_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.Radiation_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamRelievedby = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.RelievedbyIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamRelievedby_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.Relievedby_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamSeverity = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.SeverityIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamSeverity_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.Severity_textColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamStatus = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.StatusIdColumn.ColumnName]);
                                                            charSubCharacteristic.SubCharacteristicDetailModel.PhysicalExamStatus_text = MDVUtility.ToStr(drSectionSubCharacteristicDetail[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamDetail.Status_textColumn.ColumnName]);
                                                            //  charSubCharacteristic.SectionCharacteristicSubCharacteristicDetailModel.Add(subCharDetail);
                                                        }


                                                        charSubCharacteristic.PatientPhysicalSubCharacteristicId = MDVUtility.ToStr(sectionSubCharacteristicId);
                                                        charSubCharacteristic.SubCharacteristicId = MDVUtility.ToStr(SubCharacteristicId);
                                                        charSubCharacteristic.Comments = MDVUtility.ToStr(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.CommentsColumn.ColumnName]);
                                                        charSubCharacteristic.IsPositive = MDVUtility.ToStr(drSectionSubCharacteristic[dsPhysicalExamSectionSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.IsPositiveColumn.ColumnName]);
                                                        if (sectionCharacteristic.SubCharacteristics == null)
                                                            sectionCharacteristic.SubCharacteristics = new List<PatientPhysicalExamSubCharacteristicModel>();
                                                        sectionCharacteristic.SubCharacteristics.Add(charSubCharacteristic);
                                                    }
                                                }
                                                //End/ populating SubCharacteristics

                                                sectionCharacteristic.Comments = MDVUtility.ToStr(drSectionCharacteristic[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.CommentsColumn.ColumnName]);
                                                sectionCharacteristic.IsPositive = MDVUtility.ToStr(drSectionCharacteristic[dsPhysicalExamSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristic.IsPositiveColumn.ColumnName]);

                                                sectionCharacteristic.PhysicalExamCharacteristicId = MDVUtility.ToStr(sectionCharacteristicId);
                                                sectionCharacteristic.SectionCharacteristicId = sectionCharacteristic.CharacteristicId = MDVUtility.ToStr(characteristicId);
                                                if (section.Characteristics == null)
                                                    section.Characteristics = new List<PatientPhysicalExamCharacteristicModel>();
                                                section.Characteristics.Add(sectionCharacteristic);
                                            }
                                            //End/ populating Characteristics
                                            section.PatientPhysicalExamSystemSectionId = MDVUtility.ToStr(physicalExamSectionId);
                                            section.SectionId = MDVUtility.ToStr(sectionId);
                                            section.PhysicalExamSectionId = MDVUtility.ToStr(drSection[dsPhysicalExamSection.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName]);
                                            if (system.Sections == null)
                                                system.Sections = new List<PatientPhysicalExamSystemSectionModel>();
                                            system.Sections.Add(section);
                                        }
                                    }

                                }
                                if (MDVUtility.ToStr(drSystem[dsPhysicalExamSystem.PatientPhysicalExamSystem.IsNormalColumn.ColumnName]).ToLower() == "true")
                                {
                                    system.IsNormal = true;
                                    system.NormalComments = system.Comments = MDVUtility.ToStr(drSystem[dsPhysicalExamSystem.PatientPhysicalExamSystem.NormalCommentsColumn.ColumnName]);

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


    }

    /// <summary>
    /// Author : Abid Ali
    /// Desc : system section container 
    /// </summary>
    public class PatientPhysicalSystemSection
    {
        public PatientPhysicalSystemSection()
        {
            Characteristics = new List<PatientPhysicalSectionCharacteristic>();
        }
        public long SectionId { get; set; }

        public long Id { get; set; }
        public List<PatientPhysicalSectionCharacteristic> Characteristics { get; set; }
    }
    /// <summary>
    /// Author : Abid Ali
    /// Desc :  section characteristic container
    /// </summary>
    public class PatientPhysicalSectionCharacteristic
    {
        public PatientPhysicalSectionCharacteristic()
        {
            SubCharacteristics = new List<PatientPhysicalCharSubCharacteristic>();
            SectionCharacteristicDetailModel = new PatientPhysicalExamCharacteristicDetailModel();
        }
        public long SectionCharacteristicId { get; set; }
        public string Comments { get; set; }
        public long Id { get; set; }
        public bool IsPositive { get; set; }
        public bool IsDetailExists { get; set; }
        public List<PatientPhysicalCharSubCharacteristic> SubCharacteristics { get; set; }
        public PatientPhysicalExamCharacteristicDetailModel SectionCharacteristicDetailModel { get; set; }
    }
    /// <summary>
    /// Author : Abid Ali
    /// Desc :  characteristic subCharacteristic container
    /// </summary>
    public class PatientPhysicalCharSubCharacteristic
    {
        public PatientPhysicalCharSubCharacteristic()
        {
            SectionCharacteristicSubCharacteristicDetailModel = new PatientPhysicalExamSubCharacteristicDetailModel();
        }
        public long CharSubCharacteristicId { get; set; }
        public bool IsPositive { get; set; }
        public string Comments { get; set; }
        public long Id { get; set; }

        public bool IsDetailExists { get; set; }
        public PatientPhysicalExamSubCharacteristicDetailModel SectionCharacteristicSubCharacteristicDetailModel { get; set; }
    }

    #endregion
}