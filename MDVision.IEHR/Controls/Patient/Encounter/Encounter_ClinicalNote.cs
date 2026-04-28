using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using MDVision.IEHR.Common;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Patient.Encounter
{
    public class Encounter_ClinicalNote
    {
        private BLLClinical BLLClinicalObj = null;
        public Encounter_ClinicalNote()
        {
            BLLClinicalObj = new BLLClinical();
        }
        #region Singleton
        private static Encounter_ClinicalNote _obj = null;
        public static Encounter_ClinicalNote Instance()
        {
            if (_obj == null)
                _obj = new Encounter_ClinicalNote();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Fill Visits Template 
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="visitId"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        private string Fill_Visit_Template(string fieldsJson, Int64 visitId, Int64 patientId)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                BLObject<DSProgressNote>
                    obj = BLLClinicalObj.LoadProgressNote(0, visitId, patientId, 0);

                var dsProgressNote = obj.Data;
                if (obj.Data != null)
                {
                    if (dsProgressNote.Tables[dsProgressNote.ProgressNote.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VisitTemplateLoad_JSON = MDVUtility.JSON_DataTable(dsProgressNote.Tables[dsProgressNote.ProgressNote.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            MessageCount = 0,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Saves the user Answers HTML
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="visitId"></param>
        /// <param name="patientId"></param>
        /// <param name="progressNoteHTML"></param>
        /// <returns></returns>
        private string SaveTemplateDesignView(string fieldsJson, Int64 visitId, Int64 patientId, string progressNoteHTML, string previousHTML, string currentHTML, List<string> answers)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                var obj = BLLClinicalObj.LoadProgressNote(0, visitId, patientId, 0);
                var dsProgressNote = obj.Data;
                string TemplateId;
                string ProgressNoteId = "";
                if (dsProgressNote.Tables[dsProgressNote.ProgressNote.TableName].Rows.Count > 0)
                {

                    if (!string.IsNullOrEmpty(fieldsJson) && !fieldsJson.Equals("null"))
                    {

                        foreach (DataRow dr in dsProgressNote.Tables[dsProgressNote.ProgressNote.TableName].Rows)
                        {
                            // dr[dsProgressNote.ProgressNote.HTMLTemplateColumn] = progressNoteHTML;
                            dr[dsProgressNote.ProgressNote.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr[dsProgressNote.ProgressNote.ModifiedOnColumn] = DateTime.Now;
                            TemplateId = dr[dsProgressNote.ProgressNote.TemplateIDColumn].ToString();
                            ProgressNoteId = dr[dsProgressNote.ProgressNote.ProgressNoteIDColumn].ToString();
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dsProgressNote.Tables[dsProgressNote.ProgressNote.TableName].Rows)
                        {
                            //if (!string.IsNullOrEmpty(progressNoteHTML))
                            //    dr[dsProgressNote.ProgressNote.HTMLTemplateColumn] = progressNoteHTML;

                            dr[dsProgressNote.ProgressNote.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr[dsProgressNote.ProgressNote.ModifiedOnColumn] = DateTime.Now;
                            TemplateId = dr[dsProgressNote.ProgressNote.TemplateIDColumn].ToString();
                            ProgressNoteId = dr[dsProgressNote.ProgressNote.ProgressNoteIDColumn].ToString();
                        }
                    }

                    //var objTemplate = BLLClinicalObj.UpdateProgressNote(ref dsProgressNote);

                    long progressNoteId = long.Parse(ProgressNoteId);
                    var objTemplate = BLLClinicalObj.UpdateProgressNoteHTML(progressNoteId, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), DateTime.Now, previousHTML, currentHTML, ref dsProgressNote);

                    #region AnswerSheet Archiving
                    DeleteAnswerSheet(ProgressNoteId);
                    AnswerSheetArchiving(answers, patientId, visitId, objTemplate);

                    #endregion

                    if (objTemplate.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = AppPrivileges.Update_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objTemplate.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return JsonConvert.SerializeObject(response);
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Arching Answer of Template
        /// </summary>
        /// <param name="answers"></param>
        /// <param name="patientId"></param>
        /// <param name="visitId"></param>
        /// <param name="objTemplate"></param>
        private void AnswerSheetArchiving(List<string> answers, long patientId, long visitId, BLObject<DSProgressNote> objTemplate)
        {

            DSProgressNote dsClinicalAnswerSheets = new DSProgressNote();
            DSProgressNote.AnswerSheetRow drAnswerSheet;

            var counter = answers.Count;
            for (int i = 0; i < counter; i++)
            {
                drAnswerSheet = dsClinicalAnswerSheets.AnswerSheet.NewAnswerSheetRow();
                var TemplateID = objTemplate.Data.Tables[0].Rows[0][3].ToString();
                var SectionID = answers[i].Split('_')[0];
                var QuestionGroupID = answers[i].Split('_')[1];
                var QuestionID = answers[i].Split('_')[3];
                var AnswerText = answers[i].Split('_')[5];
                var AnswerValue = answers[i].Split('_')[6];
                var RadioStatus = answers[i].Split('_')[2] == "RadioTrue" ? "true" : "false";
                var ProgressNoteID = objTemplate.Data.Tables[0].Rows[0][0].ToString();

                drAnswerSheet.AnswerSheetID = -1;
                drAnswerSheet.TemplateID = long.Parse(TemplateID);
                drAnswerSheet.SectionID = long.Parse(SectionID);
                drAnswerSheet.QuestionGroupID = long.Parse(QuestionGroupID);
                drAnswerSheet.QuestionID = long.Parse(QuestionID);
                if (answers[i].Contains("Radio"))
                {
                    drAnswerSheet.AnswerText = answers[i].Split('_')[8];
                    drAnswerSheet.AnswerValue = RadioStatus;
                }
                else if (answers[i].Contains("Fraction"))
                {
                    var fractionField2_AnswerText = answers[i + 1].Split('_')[5];
                    var fractionField2_AnswerValue = answers[i + 1].Split('_')[6];
                    drAnswerSheet.AnswerText = AnswerText + " / " + fractionField2_AnswerText;
                    drAnswerSheet.AnswerValue = AnswerValue + " / " + fractionField2_AnswerValue;
                    drAnswerSheet.AnswerComments = null;
                    i++;
                }
                else
                {
                    drAnswerSheet.AnswerText = AnswerText;
                    drAnswerSheet.AnswerValue = AnswerValue;
                }

                drAnswerSheet.RowID = null;
                drAnswerSheet.ProgressNoteID = long.Parse(ProgressNoteID);
                drAnswerSheet.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName); ;
                drAnswerSheet.CreatedOn = DateTime.Now;
                drAnswerSheet.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName); ;
                drAnswerSheet.ModifiedOn = DateTime.Now;
                drAnswerSheet.PatientId = patientId;
                drAnswerSheet.VisitId = visitId;
                dsClinicalAnswerSheets.AnswerSheet.AddAnswerSheetRow(drAnswerSheet);
            }
            //foreach (string answer in answers)
            //{
            //    drAnswerSheet = dsClinicalAnswerSheets.AnswerSheet.NewAnswerSheetRow();
            //    var TemplateID = objTemplate.Data.Tables[0].Rows[0][3].ToString();
            //    var SectionID = answer.Split('_')[0];
            //    var QuestionGroupID = answer.Split('_')[1];
            //    var QuestionID = answer.Split('_')[3];
            //    var AnswerText = answer.Split('_')[5];
            //    var AnswerValue = answer.Split('_')[6];
            //    var RadioStatus = answer.Split('_')[2] == "RadioTrue" ? "false" : "true";
            //    var ProgressNoteID = objTemplate.Data.Tables[0].Rows[0][0].ToString();

            //    drAnswerSheet.AnswerSheetID = -1;
            //    drAnswerSheet.TemplateID = long.Parse(TemplateID);
            //    drAnswerSheet.SectionID = long.Parse(SectionID);
            //    drAnswerSheet.QuestionGroupID = long.Parse(QuestionGroupID);
            //    drAnswerSheet.QuestionID = long.Parse(QuestionID);
            //    if (answer.Contains("Radio"))
            //    {
            //        drAnswerSheet.AnswerText = RadioStatus;
            //        drAnswerSheet.AnswerValue = RadioStatus;
            //    }
            //    else
            //    {
            //        drAnswerSheet.AnswerText = AnswerText;
            //        drAnswerSheet.AnswerValue = AnswerValue;
            //    }
            //    drAnswerSheet.AnswerText = AnswerText;
            //    drAnswerSheet.AnswerValue = AnswerValue;
            //    drAnswerSheet.AnswerComments = null;
            //    drAnswerSheet.RowID = null;
            //    drAnswerSheet.ProgressNoteID = long.Parse(ProgressNoteID);
            //    drAnswerSheet.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName); ;
            //    drAnswerSheet.CreatedOn = DateTime.Now;
            //    drAnswerSheet.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName); ;
            //    drAnswerSheet.ModifiedOn = DateTime.Now;
            //    drAnswerSheet.PatientId = patientId;
            //    drAnswerSheet.VisitId = visitId;
            //    dsClinicalAnswerSheets.AnswerSheet.AddAnswerSheetRow(drAnswerSheet);
            //}

            var objAnswerSheet = BLLClinicalObj.Insert_PatientVisit(dsClinicalAnswerSheets);

        }

        /// <summary>
        /// Delete All Answers Against Visit in case of update
        /// </summary>
        /// <param name="progressNoteId"></param>
        /// <returns></returns>
        private string DeleteAnswerSheet(string progressNoteId)
        {
            try
            {
                if (string.IsNullOrEmpty(progressNoteId))
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
                    BLObject<string> obj = BLLClinicalObj.DeleteAnswerSheet(progressNoteId);
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

        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Employer Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "SEARCH_VISITS":
                    {
                        //string fieldsJSON = context.Request["VisitData"];
                        //Int64 VisitID = MDVUtility.ToInt64(context.Request["VisitID"]);
                        //Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        //string strJSONData = SearchVisits(fieldsJSON, PatientID, VisitID);
                        //context.Response.ContentType = "text/plain";
                        //context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_VISIT_TEMPLATE":
                    {
                        Int64 VisitID = MDVUtility.ToInt64(context.Request["visitId"]);
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["patientId"]);
                        string fieldsJson = context.Request["visitData"];
                        string strJSONData = Fill_Visit_Template(fieldsJson, VisitID, PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_VISIT_TEMPLATE_DESIGN_VIEW":
                    {
                        string clinicalTemplateDesignViewData = context.Request["clinicalTemplateDesignViewData"];
                        string ProgressNoteHTML = context.Request["ProgressNoteHTML"];
                        string Answer = context.Request["Answer"];
                        string previousHTML = context.Request["previousHTML"];
                        string currentHTML = context.Request["currentHTML"];
                        List<string> answers = Answer.Split(',').ToList<string>();
                        Int64 VisitId = MDVUtility.ToInt64(context.Request["VisitId"]);
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJsonData = SaveTemplateDesignView(clinicalTemplateDesignViewData, VisitId, PatientID, ProgressNoteHTML, previousHTML, currentHTML, answers);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }

        #endregion
    }
}