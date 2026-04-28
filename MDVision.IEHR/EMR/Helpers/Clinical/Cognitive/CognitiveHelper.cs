using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using System.Text;
using MDVision.IEHR.EMR.Model.Clinical;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical
{
    public class CognitiveHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public CognitiveHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static CognitiveHelper _instance = null;
        public static CognitiveHelper Instance()
        {
            if (_instance == null)
                _instance = new CognitiveHelper();
            return _instance;
        }



        public string insertUpdateCognitive(CognitiveModel model)
        {
            try
            {
                DSClinicalSummary dsCognitive = new DSClinicalSummary();
                BLObject<DSClinicalSummary> obj = BLLClinicalObj.loadCognitive(MDVUtility.ToInt64(model.CognitiveId), MDVUtility.ToInt64(model.PatientId), 1, 1000, "", "");

                if (obj.Data != null)
                {
                    dsCognitive = obj.Data;
                    DSClinicalSummary.CognitiveRow cognitiveRow = null;
                    DSClinicalSummary.CognitiveRow[] cognitiveRows = (DSClinicalSummary.CognitiveRow[])dsCognitive.Cognitive.Select(dsCognitive.Cognitive.CognitiveIdColumn + "=" + MDVUtility.ToLINQFormatString(model.CognitiveId));
                    if (cognitiveRows.Length > 0)
                    {
                        cognitiveRow = cognitiveRows[0];
                    }
                    else
                    {
                        cognitiveRow = dsCognitive.Cognitive.NewCognitiveRow();
                    }
                    if (cognitiveRow != null)
                    {
                        cognitiveRow.PatientId = MDVUtility.ToInt64(model.PatientId);

                        if (MDVUtility.ToBool(model.IsFromNote) && MDVUtility.ToInt64(model.NoteId) > 0)
                            cognitiveRow.NoteId = MDVUtility.ToInt64(model.NoteId);
                        else
                            cognitiveRow[dsCognitive.Cognitive.NoteIdColumn] = DBNull.Value;

                        cognitiveRow.IsActive = true;
                        if (cognitiveRows.Length == 0)
                        {
                            cognitiveRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cognitiveRow.CreatedOn = DateTime.Now;
                        }
                        cognitiveRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        cognitiveRow.ModifiedOn = DateTime.Now;
                        cognitiveRow.SoapText = model.SoapText;
                        if (cognitiveRows.Length < 1)
                        {
                            dsCognitive.Cognitive.AddCognitiveRow(cognitiveRow);
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<DSClinicalSummary> objCognitive = BLLClinicalObj.insertUpdateCognitive(dsCognitive);

                    if (objCognitive.Data != null)
                    {
                        dsCognitive = objCognitive.Data;
                        DSClinicalSummary dsCognitiveResponse = new DSClinicalSummary();
                        Int64 cognitiveId = MDVUtility.ToInt64(dsCognitive.Tables[dsCognitive.Cognitive.TableName].Rows[0][dsCognitive.Cognitive.CognitiveIdColumn.ColumnName]);
                        if (cognitiveId > 0)
                        {
                            if (model.CognitiveStatusModel.Count > 0)
                            {
                                dsCognitiveResponse.Merge(insertUpdateCognitiveStatus(cognitiveId, model.CognitiveStatusModel));
                            }

                            if (model.FunctionalStatusModel.Count > 0)
                            {
                                dsCognitiveResponse.Merge(insertUpdateFunctionalStatus(cognitiveId, model.FunctionalStatusModel));
                            }

                            if (model.MentalStatusModel.Count > 0)
                            {
                                dsCognitiveResponse.Merge(insertUpdateMentalStatus(cognitiveId, model.MentalStatusModel));
                            }

                        }

                        BLObject<string> objValue = BLLClinicalObj.updateSoapTextForCognitive(cognitiveId);
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            cognitiveId = MDVUtility.ToInt64(dsCognitive.Tables[dsCognitive.Cognitive.TableName].Rows[0][dsCognitive.Cognitive.CognitiveIdColumn.ColumnName]),
                            Cognitive_Status = MDVUtility.JSON_DataTable(dsCognitiveResponse.Tables[dsCognitiveResponse.Cognitive_Status.TableName]),
                            Functional_Status = MDVUtility.JSON_DataTable(dsCognitiveResponse.Tables[dsCognitiveResponse.Cognitive_FunctionalStatus.TableName]),
                            Mental_Status = MDVUtility.JSON_DataTable(dsCognitiveResponse.Tables[dsCognitiveResponse.Cognitive_MentalStatus.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objCognitive.Message
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

        private DSClinicalSummary insertUpdateCognitiveStatus(long cognitiveId, List<CognitiveStatusModel> lstStatusObject)
        {
            #region Cognitive Status

            DSClinicalSummary dsCognitive = new DSClinicalSummary();
            foreach (CognitiveStatusModel CurrentModel in lstStatusObject)
            {
                if (CurrentModel.CognitiveStatusId != null)
                {
                    Int64 currentStatusId = MDVUtility.ToInt64(CurrentModel.CognitiveStatusId);
                    currentStatusId = currentStatusId == 0 ? -1 : currentStatusId;
                    BLObject<DSClinicalSummary> objGoal = BLLClinicalObj.loadCognitiveStatus(cognitiveId, currentStatusId);
                    if (objGoal.Data != null)
                    {
                        dsCognitive.Merge(objGoal.Data);
                        DSClinicalSummary.Cognitive_StatusRow RowCStatus = null;
                        bool isnewRow = false;
                        if (objGoal.Data.Cognitive_Status.Rows.Count > 0)
                        {
                            RowCStatus = (DSClinicalSummary.Cognitive_StatusRow)dsCognitive.Cognitive_Status.
                                Select(dsCognitive.Cognitive_Status.CognitiveIdColumn.ColumnName + " ='" + cognitiveId + "' AND "
                                + dsCognitive.Cognitive_Status.StatusIdColumn.ColumnName + " ='" + currentStatusId + "'")[0];
                        }
                        else
                        {
                            RowCStatus = dsCognitive.Cognitive_Status.NewCognitive_StatusRow();
                            isnewRow = true;
                        }

                        if (RowCStatus != null)
                        {

                            if (dsCognitive.Cognitive_Status.Rows.Count < 1)
                            {
                                RowCStatus.StatusId = currentStatusId;
                            }
                            RowCStatus.CognitiveId = cognitiveId;

                            if (!string.IsNullOrEmpty(CurrentModel.ICD9Code))
                            {
                                RowCStatus.ICD9Code = MDVUtility.ToStr(CurrentModel.ICD9Code);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_Status.ICD9CodeColumn] = DBNull.Value;
                            }


                            if (!string.IsNullOrEmpty(CurrentModel.ICD9CodeDescription))
                            {
                                RowCStatus.ICD9CodeDescription = MDVUtility.ToStr(CurrentModel.ICD9CodeDescription);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_Status.ICD9CodeDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.ICD10Code))
                            {
                                RowCStatus.ICD10Code = MDVUtility.ToStr(CurrentModel.ICD10Code);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_Status.ICD10CodeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.ICD10CodeDescription))
                            {
                                RowCStatus.ICD10CodeDescription = MDVUtility.ToStr(CurrentModel.ICD10CodeDescription);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_Status.ICD10CodeDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.SNOMEDID))
                            {
                                RowCStatus.SNOMEDID = MDVUtility.ToStr(CurrentModel.SNOMEDID);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_Status.SNOMEDIDColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.SNOMEDDescription))
                            {
                                RowCStatus.SNOMEDDescription = MDVUtility.ToStr(CurrentModel.SNOMEDDescription);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_Status.SNOMEDDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.LexiCode))
                            {
                                RowCStatus.LexiCode = MDVUtility.ToStr(CurrentModel.LexiCode);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_Status.LexiCodeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.LexiCodeDescription))
                            {
                                RowCStatus.LexiCodeDescription = MDVUtility.ToStr(CurrentModel.LexiCodeDescription);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_Status.LexiCodeDescriptionColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(CurrentModel.Instruction))
                            {
                                RowCStatus.Instruction = MDVUtility.ToStr(CurrentModel.Instruction);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_Status.InstructionColumn] = DBNull.Value;
                            }
                            RowCStatus.IsActive = true;
                            RowCStatus.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowCStatus.CreatedOn = DateTime.Now;
                            RowCStatus.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowCStatus.ModifiedOn = DateTime.Now;

                            if (isnewRow)
                            {
                                dsCognitive.Cognitive_Status.AddCognitive_StatusRow(RowCStatus);
                            }
                        }
                    }
                }
            }
            int counter = 0;
            // start//26/01/2016//Ahmad Raza//for soap text 
            foreach (DataRow RowCStatus in dsCognitive.Cognitive_Status.Rows)
            {
                RowCStatus[dsCognitive.Cognitive_Status.SoapTextColumn] = getSoapTextForCognitiveStatus(dsCognitive, lstStatusObject[counter]);
                counter++;
            }
            //// End//26/01/2016//Ahmad Raza//for soap text 
            //#region Database Insertion/Updation

            BLObject<DSClinicalSummary> objInsertedStatus = BLLClinicalObj.insertUpdateCognitiveStatus(dsCognitive);
            return objInsertedStatus.Data;

            #endregion

        }

        private DSClinicalSummary insertUpdateFunctionalStatus(long cognitiveId, List<FunctionalStatusModel> lstStatusObject)
        {
            #region Functional Status

            DSClinicalSummary dsCognitive = new DSClinicalSummary();
            foreach (FunctionalStatusModel CurrentModel in lstStatusObject)
            {
                if (CurrentModel.FunctionalStatusId != null)
                {
                    Int64 currentStatusId = MDVUtility.ToInt64(CurrentModel.FunctionalStatusId);
                    currentStatusId = currentStatusId == 0 ? -1 : currentStatusId;
                    BLObject<DSClinicalSummary> objGoal = BLLClinicalObj.loadCognitiveFunctionalStatus(cognitiveId, currentStatusId);
                    if (objGoal.Data != null)
                    {
                        dsCognitive.Merge(objGoal.Data);
                        DSClinicalSummary.Cognitive_FunctionalStatusRow RowCStatus = null;
                        bool isNewRow = false;
                        if (objGoal.Data.Cognitive_FunctionalStatus.Rows.Count > 0)
                        {
                            RowCStatus = (DSClinicalSummary.Cognitive_FunctionalStatusRow)dsCognitive.Cognitive_FunctionalStatus.
                                Select(dsCognitive.Cognitive_FunctionalStatus.CognitiveIdColumn.ColumnName + " ='" + cognitiveId + "' AND "
                                + dsCognitive.Cognitive_FunctionalStatus.FunctionalStatusIdColumn.ColumnName + " ='" + currentStatusId + "'")[0];

                            //RowCStatus = (DSClinicalSummary.Cognitive_FunctionalStatusRow)dsCognitive.Cognitive_FunctionalStatus.Rows[0];
                        }
                        else
                        {
                            RowCStatus = dsCognitive.Cognitive_FunctionalStatus.NewCognitive_FunctionalStatusRow();
                            isNewRow = true;
                        }

                        if (RowCStatus != null)
                        {

                            if (dsCognitive.Cognitive_FunctionalStatus.Rows.Count < 1)
                            {
                                RowCStatus.FunctionalStatusId = currentStatusId;
                            }
                            RowCStatus.CognitiveId = cognitiveId;

                            if (!string.IsNullOrEmpty(CurrentModel.FunctionalStatusICD9Code))
                            {
                                RowCStatus.ICD9Code = MDVUtility.ToStr(CurrentModel.FunctionalStatusICD9Code);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_FunctionalStatus.ICD9CodeColumn] = DBNull.Value;
                            }


                            if (!string.IsNullOrEmpty(CurrentModel.FunctionalStatusICD9CodeDescription))
                            {
                                RowCStatus.ICD9CodeDescription = MDVUtility.ToStr(CurrentModel.FunctionalStatusICD9CodeDescription);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_FunctionalStatus.ICD9CodeDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.FunctionalStatusICD10Code))
                            {
                                RowCStatus.ICD10Code = MDVUtility.ToStr(CurrentModel.FunctionalStatusICD10Code);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_FunctionalStatus.ICD10CodeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.FunctionalStatusICD10CodeDescription))
                            {
                                RowCStatus.ICD10CodeDescription = MDVUtility.ToStr(CurrentModel.FunctionalStatusICD10CodeDescription);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_FunctionalStatus.ICD10CodeDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.FunctionalStatusSNOMEDID))
                            {
                                RowCStatus.SNOMEDID = MDVUtility.ToStr(CurrentModel.FunctionalStatusSNOMEDID);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_FunctionalStatus.SNOMEDIDColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.FunctionalStatusSNOMEDDescription))
                            {
                                RowCStatus.SNOMEDDescription = MDVUtility.ToStr(CurrentModel.FunctionalStatusSNOMEDDescription);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_FunctionalStatus.SNOMEDDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.FunctionalStatusLexiCode))
                            {
                                RowCStatus.LexiCode = MDVUtility.ToStr(CurrentModel.FunctionalStatusLexiCode);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_FunctionalStatus.LexiCodeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.FunctionalStatusLexiCodeDescription))
                            {
                                RowCStatus.LexiCodeDescription = MDVUtility.ToStr(CurrentModel.FunctionalStatusLexiCodeDescription);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_FunctionalStatus.LexiCodeDescriptionColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(CurrentModel.FunctionalStatusInstruction))
                            {
                                RowCStatus.Instruction = MDVUtility.ToStr(CurrentModel.FunctionalStatusInstruction);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_FunctionalStatus.InstructionColumn] = DBNull.Value;
                            }
                            RowCStatus.IsActive = true;
                            RowCStatus.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowCStatus.CreatedOn = DateTime.Now;
                            RowCStatus.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowCStatus.ModifiedOn = DateTime.Now;

                            if (isNewRow)
                            {
                                dsCognitive.Cognitive_FunctionalStatus.AddCognitive_FunctionalStatusRow(RowCStatus);
                            }
                        }
                    }
                }
            }

            int counter = 0;
            // start//26/01/2016//Ahmad Raza//for soap text 
            foreach (DataRow RowCStatus in dsCognitive.Cognitive_FunctionalStatus.Rows)
            {
                RowCStatus[dsCognitive.Cognitive_FunctionalStatus.SoapTextColumn] = getSoapTextForFunctionalStatus(dsCognitive, lstStatusObject[counter]);
                counter++;
            }
            //// End//26/01/2016//Ahmad Raza//for soap text 
            //#region Database Insertion/Updation

            BLObject<DSClinicalSummary> objInsertedStatus = BLLClinicalObj.insertUpdateCognitiveFunctionalStatus(dsCognitive);
            return objInsertedStatus.Data;

            #endregion

        }

        private DSClinicalSummary insertUpdateMentalStatus(long cognitiveId, List<MentalStatusModel> lstStatusObject)
        {
            #region Mental Status

            DSClinicalSummary dsCognitive = new DSClinicalSummary();
            foreach (MentalStatusModel CurrentModel in lstStatusObject)
            {
                if (CurrentModel.MentalStatusId != null)
                {
                    Int64 currentStatusId = MDVUtility.ToInt64(CurrentModel.MentalStatusId);
                    currentStatusId = currentStatusId == 0 ? -1 : currentStatusId;
                    BLObject<DSClinicalSummary> objGoal = BLLClinicalObj.loadCognitiveMentalStatus(cognitiveId, currentStatusId);
                    if (objGoal.Data != null)
                    {
                        dsCognitive.Merge(objGoal.Data);
                        DSClinicalSummary.Cognitive_MentalStatusRow RowCStatus = null;
                        bool isNewRow = false;
                        if (objGoal.Data.Cognitive_MentalStatus.Rows.Count > 0)
                        {
                            RowCStatus = (DSClinicalSummary.Cognitive_MentalStatusRow)dsCognitive.Cognitive_MentalStatus.
                               Select(dsCognitive.Cognitive_MentalStatus.CognitiveIdColumn.ColumnName + " ='" + cognitiveId + "' AND "
                               + dsCognitive.Cognitive_MentalStatus.MentalStatusIdColumn.ColumnName + " ='" + currentStatusId + "'")[0];
                        }
                        else
                        {
                            RowCStatus = dsCognitive.Cognitive_MentalStatus.NewCognitive_MentalStatusRow();
                            isNewRow = true;
                        }

                        if (RowCStatus != null)
                        {

                            if (dsCognitive.Cognitive_MentalStatus.Rows.Count < 1)
                            {
                                RowCStatus.MentalStatusId = currentStatusId;
                            }
                            RowCStatus.CognitiveId = cognitiveId;

                            if (!string.IsNullOrEmpty(CurrentModel.MentalStatusICD9Code))
                            {
                                RowCStatus.ICD9Code = MDVUtility.ToStr(CurrentModel.MentalStatusICD9Code);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_MentalStatus.ICD9CodeColumn] = DBNull.Value;
                            }


                            if (!string.IsNullOrEmpty(CurrentModel.MentalStatusICD9CodeDescription))
                            {
                                RowCStatus.ICD9CodeDescription = MDVUtility.ToStr(CurrentModel.MentalStatusICD9CodeDescription);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_MentalStatus.ICD9CodeDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.MentalStatusICD10Code))
                            {
                                RowCStatus.ICD10Code = MDVUtility.ToStr(CurrentModel.MentalStatusICD10Code);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_MentalStatus.ICD10CodeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.MentalStatusICD10CodeDescription))
                            {
                                RowCStatus.ICD10CodeDescription = MDVUtility.ToStr(CurrentModel.MentalStatusICD10CodeDescription);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_MentalStatus.ICD10CodeDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.MentalStatusSNOMEDID))
                            {
                                RowCStatus.SNOMEDID = MDVUtility.ToStr(CurrentModel.MentalStatusSNOMEDID);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_MentalStatus.SNOMEDIDColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.MentalStatusSNOMEDDescription))
                            {
                                RowCStatus.SNOMEDDescription = MDVUtility.ToStr(CurrentModel.MentalStatusSNOMEDDescription);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_MentalStatus.SNOMEDDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.MentalStatusLexiCode))
                            {
                                RowCStatus.LexiCode = MDVUtility.ToStr(CurrentModel.MentalStatusLexiCode);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_MentalStatus.LexiCodeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.MentalStatusLexiCodeDescription))
                            {
                                RowCStatus.LexiCodeDescription = MDVUtility.ToStr(CurrentModel.MentalStatusLexiCodeDescription);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_MentalStatus.LexiCodeDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.FreeTextICD))
                            {
                                RowCStatus.FreeTextICD = MDVUtility.ToStr(CurrentModel.FreeTextICD);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_MentalStatus.FreeTextICDColumn] = DBNull.Value;
                            }


                            if (!string.IsNullOrEmpty(CurrentModel.MentalStatusInstruction))
                            {
                                RowCStatus.Instruction = MDVUtility.ToStr(CurrentModel.MentalStatusInstruction);
                            }
                            else
                            {
                                RowCStatus[dsCognitive.Cognitive_MentalStatus.InstructionColumn] = DBNull.Value;
                            }
                            RowCStatus.IsActive = true;
                            RowCStatus.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowCStatus.CreatedOn = DateTime.Now;
                            RowCStatus.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowCStatus.ModifiedOn = DateTime.Now;

                            if (isNewRow)
                            {
                                dsCognitive.Cognitive_MentalStatus.AddCognitive_MentalStatusRow(RowCStatus);
                            }
                        }
                    }
                }

            }

            // Code for soap text here
            int counter = 0;
            foreach (DataRow RowCStatus in dsCognitive.Cognitive_MentalStatus.Rows)
            {
                RowCStatus[dsCognitive.Cognitive_MentalStatus.SoapTextColumn] = getSoapTextForMentalStatus(dsCognitive, lstStatusObject[counter]);
                counter++;
            }

            //#region Database Insertion/Updation
            BLObject<DSClinicalSummary> objInsertedStatus = BLLClinicalObj.insertUpdateCognitiveMentalStatus(dsCognitive);
            return objInsertedStatus.Data;

            #endregion

        }


        internal string getSoapTextForCognitiveStatus(DSClinicalSummary dsCognitiveStatus, CognitiveStatusModel modelObj)
        {
            string soapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                sb.Append("<div id='cognitiveStatus_" + modelObj.CognitiveStatusId + "' title='Cognitive'  name='Cognitive'><strong>Cognitive Status: </strong><br/>");
                sb.Append((string.IsNullOrEmpty(modelObj.CognitiveStatusText) ? "" : "Cognitive Status, " + modelObj.CognitiveStatusText) + (string.IsNullOrEmpty(modelObj.Instruction) ? "" : "</br> Note, " + modelObj.Instruction) + "</div>");
            }
            else
            {
                return string.Empty;
            }
            return sb.ToString();
        }

        internal string getSoapTextForFunctionalStatus(DSClinicalSummary dsCognitiveStatus, FunctionalStatusModel modelObj)
        {
            string soapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                sb.Append("<div id='functionalStatus_" + modelObj.FunctionalStatusId + "' title='Functional Status'  name='Functional'><strong>Functional Status: </strong><br/>");
                sb.Append((string.IsNullOrEmpty(modelObj.FunctionalStatusText) ? "" : "Functional Status, " + modelObj.FunctionalStatusText) + (string.IsNullOrEmpty(modelObj.FunctionalStatusInstruction) ? "" : "</br> Note, " + modelObj.FunctionalStatusInstruction) + "</div>");
            }
            else
            {
                return string.Empty;
            }
            return sb.ToString();
        }

        internal string getSoapTextForMentalStatus(DSClinicalSummary dsCognitiveStatus, MentalStatusModel modelObj)
        {
            string soapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                sb.Append("<div id='mentalStatus_" + modelObj.MentalStatusId + "' title='Mental Status'  name='Mental'><strong>Mental Status: </strong><br/>");
                sb.Append((string.IsNullOrEmpty(modelObj.MentalStatusText) ? "" : "Mental Status, " + modelObj.MentalStatusText) + (string.IsNullOrEmpty(modelObj.MentalStatusInstruction) ? "" : "</br> Note, " + modelObj.MentalStatusInstruction) + "</div>");
            }
            else
            {
                return string.Empty;
            }
            return sb.ToString();
        }



        public string fillCognitive(CognitiveModel model, Int64 cognitiveId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)) && cognitiveId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSClinicalSummary dsCognitive = null;
                    BLObject<DSClinicalSummary> obj = null;
                    obj = BLLClinicalObj.loadCognitive(cognitiveId, MDVUtility.ToInt64(model.PatientId), 1, 1000, "1", "");
                    dsCognitive = obj.Data;
                    if (dsCognitive.Tables[dsCognitive.Cognitive.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsCognitive.Tables[dsCognitive.Cognitive.TableName].Rows[0];
                        Int64 CognitiveId = MDVUtility.ToInt64(dr[dsCognitive.Cognitive.CognitiveIdColumn.ColumnName]);
                        var PlanofCarekeyValues = new Dictionary<string, string>
                        {
                            { "CognitiveId",  MDVUtility.ToStr(dr[dsCognitive.Cognitive.CognitiveIdColumn.ColumnName])},
                            { "NoteId",  MDVUtility.ToStr(dr[dsCognitive.Cognitive.NoteIdColumn.ColumnName])},
                            { "IsAttatchedWithNote",  MDVUtility.ToStr(dr[dsCognitive.Cognitive.IsAttatchedWithNoteColumn.ColumnName])},
                            { "CognitiveSoapText", MDVUtility.ToStr(dr[dsCognitive.Cognitive.SoapTextColumn.ColumnName])},
                        };

                        Int64 congnitiveStatusId = MDVUtility.ToInt64(model.CognitiveStatusId);
                        Int64 congnitiveFunctionalStatusId = MDVUtility.ToInt64(model.FunctionalStatusId);
                        Int64 congnitiveMentalStatusId = MDVUtility.ToInt64(model.MentalStatusId);

                        DSClinicalSummary dsPlanOfCareGoal = null;
                        BLObject<DSClinicalSummary> objGoal = BLLClinicalObj.loadCognitiveStatus(CognitiveId, congnitiveStatusId);
                        dsPlanOfCareGoal = objGoal.Data;

                        DSClinicalSummary dsFunctionalStatus = null;
                        BLObject<DSClinicalSummary> FunctionalStatus = BLLClinicalObj.loadCognitiveFunctionalStatus(CognitiveId, congnitiveFunctionalStatusId);
                        dsFunctionalStatus = FunctionalStatus.Data;

                        DSClinicalSummary dsMentalStatus = null;
                        BLObject<DSClinicalSummary> MentalStatus = BLLClinicalObj.loadCognitiveMentalStatus(CognitiveId, congnitiveMentalStatusId);
                        dsMentalStatus = MentalStatus.Data;

                        List<Dictionary<string, string>> lstCognitiveStatus = new List<Dictionary<string, string>>();
                        List<Dictionary<string, string>> lstCognitiveFunctionalStatus = new List<Dictionary<string, string>>();
                        List<Dictionary<string, string>> lstCognitiveMentalStatus = new List<Dictionary<string, string>>();

                        if (dsPlanOfCareGoal.Cognitive_Status.Rows.Count > 0)
                        {
                            foreach (DataRow drCurrentStatus in dsPlanOfCareGoal.Cognitive_Status.Rows)
                            {
                                var GoalkeyValues = new Dictionary<string, string>
                                    {
                                            { "StatusId",  MDVUtility.ToStr(drCurrentStatus[dsPlanOfCareGoal.Cognitive_Status.StatusIdColumn.ColumnName])},
                                            { "Instruction", MDVUtility.ToStr(drCurrentStatus[dsPlanOfCareGoal.Cognitive_Status.InstructionColumn.ColumnName])},
                                            { "CognitiveId", MDVUtility.ToStr(drCurrentStatus[dsPlanOfCareGoal.Cognitive_Status.CognitiveIdColumn.ColumnName])},
                                            { "ICD9Code", MDVUtility.ToStr(drCurrentStatus[dsPlanOfCareGoal.Cognitive_Status.CognitiveIdColumn.ColumnName])},
                                    };
                                lstCognitiveStatus.Add(GoalkeyValues);
                            }

                        }

                        if (dsFunctionalStatus.Cognitive_FunctionalStatus.Rows.Count > 0)
                        {
                            foreach (DataRow drCurrentFunctionalStatus in dsFunctionalStatus.Cognitive_FunctionalStatus.Rows)
                            {
                                var FunctionalStatuskeyValues = new Dictionary<string, string>
                                    {
                                            { "StatusId",  MDVUtility.ToStr(drCurrentFunctionalStatus[dsFunctionalStatus.Cognitive_FunctionalStatus.FunctionalStatusIdColumn.ColumnName])},
                                            { "Instruction", MDVUtility.ToStr(drCurrentFunctionalStatus[dsFunctionalStatus.Cognitive_FunctionalStatus.InstructionColumn.ColumnName])},
                                            { "CognitiveId", MDVUtility.ToStr(drCurrentFunctionalStatus[dsFunctionalStatus.Cognitive_FunctionalStatus.CognitiveIdColumn.ColumnName])},
                                    };
                                lstCognitiveFunctionalStatus.Add(FunctionalStatuskeyValues);
                            }
                        }

                        if (dsMentalStatus.Cognitive_MentalStatus.Rows.Count > 0)
                        {
                            foreach (DataRow drCurrentMentalStatus in dsMentalStatus.Cognitive_MentalStatus.Rows)
                            {
                                var MentalStatuskeyValues = new Dictionary<string, string>
                                    {
                                            { "StatusId",  MDVUtility.ToStr(drCurrentMentalStatus[dsMentalStatus.Cognitive_MentalStatus.MentalStatusIdColumn.ColumnName])},
                                            { "Instruction", MDVUtility.ToStr(drCurrentMentalStatus[dsMentalStatus.Cognitive_MentalStatus.InstructionColumn.ColumnName])},
                                            { "CognitiveId", MDVUtility.ToStr(drCurrentMentalStatus[dsMentalStatus.Cognitive_MentalStatus.CognitiveIdColumn.ColumnName])},
                                    };
                                lstCognitiveMentalStatus.Add(MentalStatuskeyValues);
                            }
                        }

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            CognitiveFill_JSON = js.Serialize(PlanofCarekeyValues),
                            StatusFill_JSON = js.Serialize(lstCognitiveStatus),
                            FucntionalStatusFill_JSON = js.Serialize(lstCognitiveFunctionalStatus),
                            MentalStatusFill_JSON = js.Serialize(lstCognitiveMentalStatus),
                            StatusLoad_JSON = MDVUtility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.Cognitive_Status.TableName]),//js.Serialize(lstCognitiveStatus),
                            FucntionalStatusLoad_JSON = MDVUtility.JSON_DataTable(dsFunctionalStatus.Tables[dsFunctionalStatus.Cognitive_FunctionalStatus.TableName]),//js.Serialize(lstCognitiveFunctionalStatus),
                            MentalStatusLoad_JSON = MDVUtility.JSON_DataTable(dsMentalStatus.Tables[dsMentalStatus.Cognitive_MentalStatus.TableName])//js.Serialize(lstCognitiveMentalStatus)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,

                            CognitiveFill_JSON = "[]",
                            StatusFill_JSON = "[]",
                            FucntionalStatusFill_JSON = "[]",
                            MentalStatusFill_JSON = "[]",
                            StatusLoad_JSON = "[]",
                            FucntionalStatusLoad_JSON = "[]",
                            MentalStatusLoad_JSON = "[]"
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
        //public string fillCognitives(ClinicalSummaryModel model, Int64 cognitiveId)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)))
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        else
        //        {
        //            DSClinicalSummary dsPlanOfCareGoal = null;
        //            BLObject<DSClinicalSummary> objGoal = BLLClinicalObj.loadCognitivesStatus(MDVUtility.ToInt64(model.PatientId));
        //            dsPlanOfCareGoal = objGoal.Data;

        //            DSClinicalSummary dsFunctionalStatus = null;
        //            BLObject<DSClinicalSummary> FunctionalStatus = BLLClinicalObj.loadCognitivesFunctionalStatus(MDVUtility.ToInt64(model.PatientId));
        //            dsFunctionalStatus = FunctionalStatus.Data;

        //            DSClinicalSummary dsMentalStatus = null;
        //            BLObject<DSClinicalSummary> MentalStatus = BLLClinicalObj.loadCognitivesMentalStatus(MDVUtility.ToInt64(model.PatientId));
        //            dsMentalStatus = MentalStatus.Data;

        //            List<Dictionary<string, string>> lstCognitiveStatus = new List<Dictionary<string, string>>();
        //            List<Dictionary<string, string>> lstCognitiveFunctionalStatus = new List<Dictionary<string, string>>();
        //            List<Dictionary<string, string>> lstCognitiveMentalStatus = new List<Dictionary<string, string>>();

        //            if (dsPlanOfCareGoal.Cognitive_Status.Rows.Count > 0)
        //            {
        //                foreach (DataRow drCurrentStatus in dsPlanOfCareGoal.Cognitive_Status.Rows)
        //                {
        //                    var GoalkeyValues = new Dictionary<string, string>
        //                            {
        //                                    { "StatusId",  MDVUtility.ToStr(drCurrentStatus[dsPlanOfCareGoal.Cognitive_Status.StatusIdColumn.ColumnName])},
        //                                    { "Instruction", MDVUtility.ToStr(drCurrentStatus[dsPlanOfCareGoal.Cognitive_Status.InstructionColumn.ColumnName])},
        //                                    { "CognitiveId", MDVUtility.ToStr(drCurrentStatus[dsPlanOfCareGoal.Cognitive_Status.CognitiveIdColumn.ColumnName])},
        //                                    { "ICD9Code", MDVUtility.ToStr(drCurrentStatus[dsPlanOfCareGoal.Cognitive_Status.CognitiveIdColumn.ColumnName])},
        //                            };
        //                    lstCognitiveStatus.Add(GoalkeyValues);
        //                }

        //            }

        //            if (dsFunctionalStatus.Cognitive_FunctionalStatus.Rows.Count > 0)
        //            {
        //                foreach (DataRow drCurrentFunctionalStatus in dsFunctionalStatus.Cognitive_FunctionalStatus.Rows)
        //                {
        //                    var FunctionalStatuskeyValues = new Dictionary<string, string>
        //                            {
        //                                    { "StatusId",  MDVUtility.ToStr(drCurrentFunctionalStatus[dsFunctionalStatus.Cognitive_FunctionalStatus.FunctionalStatusIdColumn.ColumnName])},
        //                                    { "Instruction", MDVUtility.ToStr(drCurrentFunctionalStatus[dsFunctionalStatus.Cognitive_FunctionalStatus.InstructionColumn.ColumnName])},
        //                                    { "CognitiveId", MDVUtility.ToStr(drCurrentFunctionalStatus[dsFunctionalStatus.Cognitive_FunctionalStatus.CognitiveIdColumn.ColumnName])},
        //                            };
        //                    lstCognitiveFunctionalStatus.Add(FunctionalStatuskeyValues);
        //                }
        //            }

        //            if (dsMentalStatus.Cognitive_MentalStatus.Rows.Count > 0)
        //            {
        //                foreach (DataRow drCurrentMentalStatus in dsMentalStatus.Cognitive_MentalStatus.Rows)
        //                {
        //                    var MentalStatuskeyValues = new Dictionary<string, string>
        //                            {
        //                                    { "StatusId",  MDVUtility.ToStr(drCurrentMentalStatus[dsMentalStatus.Cognitive_MentalStatus.MentalStatusIdColumn.ColumnName])},
        //                                    { "Instruction", MDVUtility.ToStr(drCurrentMentalStatus[dsMentalStatus.Cognitive_MentalStatus.InstructionColumn.ColumnName])},
        //                                    { "CognitiveId", MDVUtility.ToStr(drCurrentMentalStatus[dsMentalStatus.Cognitive_MentalStatus.CognitiveIdColumn.ColumnName])},
        //                            };
        //                    lstCognitiveMentalStatus.Add(MentalStatuskeyValues);
        //                }
        //            }

        //            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        //            var response = new
        //            {
        //                status = true,
        //                CognitiveFill_JSON = "",//js.Serialize(PlanofCarekeyValues),
        //                StatusFill_JSON = js.Serialize(lstCognitiveStatus),
        //                FucntionalStatusFill_JSON = js.Serialize(lstCognitiveFunctionalStatus),
        //                MentalStatusFill_JSON = js.Serialize(lstCognitiveMentalStatus),
        //                StatusLoad_JSON = MDVUtility.JSON_DataTable(dsPlanOfCareGoal.Tables[dsPlanOfCareGoal.Cognitive_Status.TableName]),//js.Serialize(lstCognitiveStatus),
        //                FucntionalStatusLoad_JSON = MDVUtility.JSON_DataTable(dsFunctionalStatus.Tables[dsFunctionalStatus.Cognitive_FunctionalStatus.TableName]),//js.Serialize(lstCognitiveFunctionalStatus),
        //                MentalStatusLoad_JSON = MDVUtility.JSON_DataTable(dsMentalStatus.Tables[dsMentalStatus.Cognitive_MentalStatus.TableName])//js.Serialize(lstCognitiveMentalStatus)
        //            };
        //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message = MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}

        public string deletCognitiveStatus(string statusId, string cognitiveId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(statusId)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteCognitiveStatus(MDVUtility.ToStr(statusId));
                    if (obj.Data == "")
                    {
                        //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForCognitive(MDVUtility.ToInt64(cognitiveId));
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

        public string deletFunctionalStatus(string statusId, string cognitiveId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(statusId)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteCognitiveFunctionalStatus(MDVUtility.ToStr(statusId));
                    if (obj.Data == "")
                    {
                        //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForCognitive(MDVUtility.ToInt64(cognitiveId));
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

        public string deletMentalStatus(string statusId, string cognitiveId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(statusId)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteCognitiveMentalStatus(MDVUtility.ToStr(statusId));
                    if (obj.Data == "")
                    {
                        //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForCognitive(MDVUtility.ToInt64(cognitiveId));
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
        

        internal string attachCognitiveWithNotes(CognitiveModel model)
        {
            try
            {
                if (MDVUtility.ToLong(model.CognitiveId) <= 0 || MDVUtility.ToLong(model.NoteId) <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.attatchCognitiveWithNotes(model.CognitiveId, MDVUtility.ToLong(model.NoteId));
                    if (obj.Data == "")
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

        internal string detachCognitiveFromNotes(string CognitiveId, long notesId)
        {
            try
            {
                if (MDVUtility.ToLong(CognitiveId) <= 0 || MDVUtility.ToLong(notesId) <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.detachCognitiveFromNotes(CognitiveId, notesId);
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

    }
}