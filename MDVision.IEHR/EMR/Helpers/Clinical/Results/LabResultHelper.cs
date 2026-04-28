/* Author:  Muhammad Arshad
 * Created Date: 15/03/2016
 * OverView: Created to handel Lab Result
 */
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.OrdersAndResults.LabResult;
using System;
using System.Collections.Generic;
using System.Linq;
using MDVision.Business.BCommon;
using System.Data;
using System.Text;
using MDVision.IEHR.EMR.Model.HL7;
using System.Web;
using System.IO;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.IEHR.EMR.Helpers.Clinical.Requisitions;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using MDVision.Model.Clinical.LabTrends;
using System.Xml.Linq;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.Model.Admin.Provider;

namespace MDVision.IEHR.EMR.Helpers.Results
{
    public class LabResultHelper
    {
        private static LabResultHelper _instance = null;
        private BLLMessage BLLMessageObj = null;
        private BLLPatient BLLPatientObj = null;
        private BLLClinical BLLClinicalObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;
        private LabOrderResultRequisitions RequisitionsObj = null;

        public LabResultHelper()
        {
            BLLMessageObj = new BLLMessage();
            BLLPatientObj = new BLLPatient();
            BLLClinicalObj = new BLLClinical();
            BLLAdminProfileObj = new BLLAdminProfile();
            RequisitionsObj = new LabOrderResultRequisitions();
        }
        public static LabResultHelper Instance()
        {
            if (_instance == null)
            {
                _instance = new LabResultHelper();
            }
            return _instance;
        }




        #region Lab Result Fill, Save and Update Methods



        // Author: Abid Ali
        // Created Date: 16/04/2016
        //OverView: This function will handle fill of Lab Result
        public string fillLabResult(LabOrderResultModel model)
        {

            try
            {
                DSLabOrder dsLabOrder = null;
                DSClinicalLab dsClinicalLab = null;
                BLObject<DSLabOrder> obj = BLLClinicalObj.LoadLabOrder(MDVUtility.ToInt64(model.LabOrderId), MDVUtility.ToInt64(model.PatientId), 0, model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", 0);
                dsLabOrder = obj.Data;
                if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
                {
                    DSLabOrder dsLabTest = new DSLabOrder();
                    DSLabResult dsLabResultSpecimen = new DSLabResult();
                    DSLabResult dsLabResultRejectReasonSpecimen = new DSLabResult();

                    //load All specimen Result Reason
                    BLObject<DSLabResult> objLabResultSpecimen = BLLClinicalObj.loadLabResultSpecimen(0, 0);
                    dsLabResultSpecimen = objLabResultSpecimen.Data;

                    //load All specimen Result Reject Reason
                    BLObject<DSLabResult> objLabResultRejectReasonSpecimen = BLLClinicalObj.loadLabResultSpecimenRejectReason(0, 0);
                    dsLabResultRejectReasonSpecimen = objLabResultRejectReasonSpecimen.Data;


                    BLObject<DSLabOrder> objTest = BLLClinicalObj.LoadLabOrderTest(MDVUtility.ToInt64(model.LabOrderId), 0, MDVUtility.ToInt64(model.PatientId), "1", "2000");
                    dsLabTest = objTest.Data;
                    int i = 0;

                    //Load Lab Result data
                    DSLabResult dsLabResult = new DSLabResult();
                    DSLabResult.LabOrderResultRow drLabResult = null;
                    //Start 01-11-2016 Humaira Yousaf for dbaudit
                    BLObject<DSLabResult> objResult = BLLClinicalObj.LoadLabResult(0, MDVUtility.ToInt64(model.LabOrderId), model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", "", MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.PatientId), "1", "");
                    //End 01-11-2016 Humaira Yousaf for dbaudit
                    dsLabResult = objResult.Data;

                    Int64 labResultId = -1;
                    string comments = string.Empty;
                    string remarks = string.Empty;
                    string status = string.Empty;
                    string AssigneeId = string.Empty;
                    string Assignee = string.Empty;
                    string IsSentToPortal = string.Empty;
                    string IsAknowledged = string.Empty;
                    string IsMarKAsReviewed = string.Empty;
                    string FinalInterpretation = string.Empty;

                    int resultCount = 0;
                    int rejectionReasonCount = 0;
                    if (objResult.Data != null)
                    {
                        if (dsLabResult.LabOrderResult.Rows.Count > 0)
                        {
                            //Load Parent Table data
                            drLabResult = (DSLabResult.LabOrderResultRow)dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0];
                            labResultId = MDVUtility.ToInt64(drLabResult[dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName]);
                            comments = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.CommentsColumn.ColumnName]);
                            remarks = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.RemarksColumn.ColumnName]);
                            status = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.StatusColumn.ColumnName]);
                            AssigneeId = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.AssigneeIdColumn.ColumnName]);
                            Assignee = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.AssignedToColumn.ColumnName]);
                            IsSentToPortal = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.IsSentToPortalColumn.ColumnName]);
                            IsAknowledged = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.IsAknowledgedColumn.ColumnName]);
                            IsMarKAsReviewed = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.MarkAsReviewedColumn.ColumnName]);
                            FinalInterpretation = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.FinalInterpretationColumn.ColumnName]);
                        }

                    }

                    //Load child Table data
                    DSLabResult dsLabResultDetail = new DSLabResult();
                    BLObject<DSLabResult> objLabResultDetail = BLLClinicalObj.LoadLabResultDetail(0, labResultId);
                    dsLabResultDetail = objLabResultDetail.Data;


                    DataRow dr = dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0];
                    if (dsLabResult.LabOrderResult.Rows.Count == 0 && string.IsNullOrEmpty(Assignee) && string.IsNullOrEmpty(AssigneeId) && MDVUtility.ToInt64(dr[dsLabOrder.LabOrder.AssigneeIdColumn.ColumnName]) > 0)
                    {
                        Assignee = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.AssigneeNameColumn.ColumnName]);
                        AssigneeId = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.AssigneeIdColumn.ColumnName]);
                    }
                    var orderInfoKeyValues = new Dictionary<string, string>
                        {                           
                            //Start//For Lab Result

                            { "Status", status },
                            { "LabOrderResultId",  MDVUtility.ToStr(labResultId)},
                            { "Comments", comments },
                            { "Remarks", remarks },
                            { "FinalInterpretation", FinalInterpretation },

                            //End//For Lab Result
                             
                            { "DateAndTime",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.OrderDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsLabOrder.LabOrder.OrderDateColumn.ColumnName]).ToShortDateString() +" "+dr[dsLabOrder.LabOrder.OrderTimeColumn.ColumnName]},
                            { "LabOrderId",  MDVUtility.ToStr(dr[dsLabOrder.LabOrder.LabOrderIdColumn.ColumnName])},

                            { "Laboratory",  MDVUtility.ToStr(dr[dsLabOrder.LabOrder.LabNameColumn.ColumnName])},
                            { "LaboratoryId",  MDVUtility.ToStr(dr[dsLabOrder.LabOrder.LabIdColumn.ColumnName])},

                            { "Facility", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.FacilityColumn.ColumnName])},
                            { "FacilityId", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.FacilityIdColumn.ColumnName])},
                            { "Location", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.LocationColumn.ColumnName])},

                            { "Provider", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.ProviderColumn.ColumnName])},
                            { "ProviderId", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.ProviderIdColumn.ColumnName])},

                            //Start//Fetch from Result table 
                            { "Assignee", Assignee},
                            { "AssigneeId", AssigneeId},
                            //End//Fetch from Result table 

                            { "IsSentToPortal", IsSentToPortal},
                            { "IsAknowledged", IsAknowledged},
                            { "MarkAsReviewed", IsMarKAsReviewed},

                            { "OrderNo", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.OrderNoColumn.ColumnName])},
                            { "PatientId", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.PatientIdColumn.ColumnName])},

                            { "CodeSystemId", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.CodeSystemIdColumn.ColumnName])},
                            { "CodeSystemName", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.CodeSystemNameColumn.ColumnName])},
                        };

                    foreach (DataRow drLabTest in dsLabTest.Tables[dsLabTest.LabOrderTest.TableName].Rows)
                    {
                        string LabResultTestId = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.LabOrderTestIdColumn.ColumnName]);
                        string LabResultTestCPTCode = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeColumn.ColumnName]);
                        string LabResultTestCPTDescription = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeDescriptionColumn.ColumnName]);

                        LabOrderResultDetailModel detailModel = new LabOrderResultDetailModel();

                        if (dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count > 0)
                        {
                            if (objLabResultDetail.Data != null)
                            {

                                foreach (DataRow drLabResultDetail in dsLabResultDetail.Tables[dsLabResultDetail.LabOrderResultDetail.TableName].Rows)
                                {
                                    ChildResultDetailModel child = new ChildResultDetailModel();
                                    var CptCode = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.CPTCodeColumn]);
                                    var CptDescription = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.CPTCodeDescriptionColumn]);
                                    LabResultTestCPTDescription = string.Join("", LabResultTestCPTDescription.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                                    CptDescription = string.Join("", CptDescription.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                                    //save child rows

                                    if (CptCode == LabResultTestCPTCode && (LabResultTestCPTDescription.ToLower().IndexOf(CptDescription.ToLower()) > -1))
                                    {
                                        child.LabOrderResultDetailId = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LabOrderResultDetailIdColumn]);
                                        child.LOINC = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LOINCColumn]);
                                        child.LOINCDescription = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LOINCDescriptionColumn]);
                                        child.ObservationDate = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ObservationDateColumn]);
                                        child.Range = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.RangeColumn]);
                                        child.ReferenceRangeDescription = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ReferenceRangeDescriptionColumn]);
                                        child.ReferenceRangeInterpration = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ReferenceRangeInterprationColumn]);
                                        child.TestAntimicrobial = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.TestAntimicrobialColumn]);
                                        child.ConditionStatement = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ConditionStatementColumn]);
                                        child.Result = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ResultColumn]);
                                        child.UoM = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.UoMColumn]);
                                        child.Flag = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.FlagColumn]);
                                        child.IsAttribute = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.IsAttributeColumn]);
                                        child.IsElectronicResult = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.IsElectronicResultColumn]);
                                        child.LabId = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LabIdColumn]);
                                        child.LabTestId = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LabTestIdColumn]);
                                        child.LabTestAttributeId = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LabTestAttributeIdColumn]);
                                        child.Organism = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.IsOrganismAssociatedColumn]);
                                        BLObject<DSClinicalLab> objLabTestResultAttr = BLLClinicalObj.GetLabTestAttributeResult(MDVUtility.ToInt64(child.LabTestAttributeId));
                                        if (objLabTestResultAttr.Data != null)
                                        {
                                            dsClinicalLab = objLabTestResultAttr.Data;
                                            foreach (DataRow drResultAttrDetail in dsClinicalLab.Tables[dsClinicalLab.LabTestAttributeResult.TableName].Rows)
                                            {
                                                child.childResultAttribueModel.Add(new ChildResultAttribueModel()
                                                {
                                                    LabTestAttributeResultId = MDVUtility.ToStr(drResultAttrDetail[dsClinicalLab.LabTestAttributeResult.LabTestAttributeResultIdColumn]),
                                                    LabTestAttributeId = MDVUtility.ToStr(drResultAttrDetail[dsClinicalLab.LabTestAttributeResult.LabTestAttributeIdColumn]),
                                                    ResultName = MDVUtility.ToStr(drResultAttrDetail[dsClinicalLab.LabTestAttributeResult.ResultNameColumn]),
                                                });
                                            }
                                        }

                                        if (CptCode.IndexOf("50545") != -1 && model.LabOrderResultDetailModels.Count > 0)
                                        {
                                            foreach (var item in model.LabOrderResultDetailModels)
                                            {
                                                List<ChildResultDetailModel> objDetail = item.ChildRows.Where(n => n.LOINC.Equals("625-4")).ToList<ChildResultDetailModel>();
                                                if (objDetail != null)
                                                {
                                                    foreach (var itemRows in objDetail)
                                                    {
                                                        if (itemRows.ChildRows == null)
                                                        {
                                                            itemRows.ChildRows = new List<ChildResultDetailModel>();
                                                        }

                                                        itemRows.ChildRows.Add(child);
                                                        resultCount++;
                                                    }


                                                }
                                            }
                                            //foreach (var item in model.LabOrderResultDetailModels)
                                            //{
                                            //    List<ChildResultDetailModel> objDetail = item.ChildRows.Where(n => n.LOINC.Equals("625-4")).ToList<ChildResultDetailModel>();//&& n.ChildRows == null);
                                            //    foreach (var itemparent in objDetail)
                                            //    {
                                            //        if (itemparent != null)
                                            //        {
                                            //            itemparent.ChildRows = new List<ChildResultDetailModel>();
                                            //            itemparent.ChildRows.Add(child);
                                            //            break;
                                            //        }
                                            //    }

                                            //}
                                        }
                                        else
                                        {
                                            detailModel.ChildRows.Add(child);
                                            resultCount++;
                                        }

                                    }
                                }
                            }
                        }

                        if (objLabResultSpecimen.Data != null)
                        {
                            DSLabResult.LabResultSpecimenRow[] resultSpecimenRows = (DSLabResult.LabResultSpecimenRow[])dsLabResultSpecimen.LabResultSpecimen.Select(dsLabResultSpecimen.LabResultSpecimen.LabOrderTestIdColumn.ColumnName + "=" + LabResultTestId);
                            foreach (DSLabResult.LabResultSpecimenRow drResultSpecimen in resultSpecimenRows)
                            {
                                //For populating Result Specimen
                                ResultSpecimen specimen = new ResultSpecimen();
                                specimen.CollectionDateTime = MDVUtility.ToStr(drResultSpecimen[dsLabResultSpecimen.LabResultSpecimen.CollectionDateTimeColumn.ColumnName]);
                                specimen.NameOfCodingSystem = MDVUtility.ToStr(drResultSpecimen[dsLabResultSpecimen.LabResultSpecimen.NameofCodingSystemColumn.ColumnName]);
                                specimen.OriginalText = MDVUtility.ToStr(drResultSpecimen[dsLabResultSpecimen.LabResultSpecimen.OriginalTextColumn.ColumnName]);
                                specimen.Quantity = MDVUtility.ToStr(drResultSpecimen[dsLabResultSpecimen.LabResultSpecimen.QuantityColumn.ColumnName]);
                                specimen.SpecimenType = MDVUtility.ToStr(drResultSpecimen[dsLabResultSpecimen.LabResultSpecimen.SpecimenTypeColumn.ColumnName]);
                                specimen.Text = MDVUtility.ToStr(drResultSpecimen[dsLabResultSpecimen.LabResultSpecimen.TextColumn.ColumnName]);

                                long resultSpecimenId = MDVUtility.ToInt64(drResultSpecimen[dsLabResultSpecimen.LabResultSpecimen.LabResultSpecimenIdColumn.ColumnName]);

                                if (objLabResultRejectReasonSpecimen.Data != null)
                                {
                                    DSLabResult.SpecimenRejectReasonRow[] resultRejectReasonSpecimenRows = (DSLabResult.SpecimenRejectReasonRow[])dsLabResultRejectReasonSpecimen.SpecimenRejectReason.Select(dsLabResultRejectReasonSpecimen.SpecimenRejectReason.LabResultSpecimenIdColumn.ColumnName + "=" + resultSpecimenId);
                                    //For populating Result Reject Specimen
                                    foreach (DSLabResult.SpecimenRejectReasonRow drResultRejectReasonSpecimen in resultRejectReasonSpecimenRows)
                                    {
                                        ResultSpecimenRejectReason rejectReasonSpecimen = new ResultSpecimenRejectReason();
                                        rejectReasonSpecimen.NameOfAletrnateCodingSystem = MDVUtility.ToStr(drResultRejectReasonSpecimen[dsLabResultRejectReasonSpecimen.SpecimenRejectReason.NameofAlternateCodingSystemColumn.ColumnName]);
                                        rejectReasonSpecimen.NameOfCodingSystem = MDVUtility.ToStr(drResultRejectReasonSpecimen[dsLabResultRejectReasonSpecimen.SpecimenRejectReason.NameofCodingSystemColumn.ColumnName]);
                                        rejectReasonSpecimen.OriginalText = MDVUtility.ToStr(drResultRejectReasonSpecimen[dsLabResultRejectReasonSpecimen.SpecimenRejectReason.OriginalTextColumn.ColumnName]);
                                        rejectReasonSpecimen.AlternateIdentifier = MDVUtility.ToStr(drResultRejectReasonSpecimen[dsLabResultRejectReasonSpecimen.SpecimenRejectReason.AlternateIdentifierColumn.ColumnName]);
                                        rejectReasonSpecimen.AlternateText = MDVUtility.ToStr(drResultRejectReasonSpecimen[dsLabResultRejectReasonSpecimen.SpecimenRejectReason.AlternateTextColumn.ColumnName]);
                                        rejectReasonSpecimen.Identifer = MDVUtility.ToStr(drResultRejectReasonSpecimen[dsLabResultRejectReasonSpecimen.SpecimenRejectReason.IdentifierColumn.ColumnName]);
                                        rejectReasonSpecimen.Text = MDVUtility.ToStr(drResultRejectReasonSpecimen[dsLabResultRejectReasonSpecimen.SpecimenRejectReason.TextColumn.ColumnName]);

                                        specimen.ResultSpecimenRejectReasons.Add(rejectReasonSpecimen);
                                        rejectionReasonCount++;
                                    }
                                }
                                detailModel.ResultSpecimens.Add(specimen);
                            }
                        }

                        detailModel.LabOrderResultDetailId = (--i).ToString();
                        detailModel.CPTCode = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeColumn.ColumnName]);
                        detailModel.CPTCodeDescription = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeDescriptionColumn.ColumnName]);
                        DateTime dt = DateTime.Now;

                        detailModel.ObservationDate = null;
                        string Date = MDVUtility.ToStr(MDVUtility.ToDateTime(drLabTest[dsLabTest.LabOrderTest.TestDateColumn.ColumnName]).ToShortDateString());
                        string Time = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.TestTimeColumn.ColumnName]);
                        if (!string.IsNullOrEmpty(Date) && !string.IsNullOrEmpty(Time))
                        {
                            detailModel.ObservationDate = MDVUtility.ToStr(MDVUtility.ToDateTime(Date + " " + Time));
                        }
                        else
                        {

                        }
                        if (detailModel.CPTCode.IndexOf("50545") != -1 && model.LabOrderResultDetailModels.Count > 0)
                        {
                            var existss = model.LabOrderResultDetailModels.FirstOrDefault(n => n.CPTCode.Equals("625-4"));
                            if (existss == null)
                            {
                                model.LabOrderResultDetailModels.Add(detailModel);

                            }
                        }
                        else
                        {
                            model.LabOrderResultDetailModels.Add(detailModel);
                        }

                    }

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                    var response = new
                    {
                        status = true,
                        LabResultOrderInfoFill_JSON = js.Serialize(orderInfoKeyValues),
                        LabResultOrderTestFill_JSON = js.Serialize(model),
                        LabResultTestsFill_JSON = MDVUtility.JSON_DataTable(dsLabTest.Tables[dsLabTest.LabOrderTest.TableName]),
                        ResultCount = resultCount,
                        RejectionReasonCount = rejectionReasonCount

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.No_Record_Message
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
        // Created Date: 17/03/2016
        //OverView: This function will update Lab Result

        public string insertUpdateLabResult(LabOrderResultModel model)
        {

            try
            {
                bool isInsertedPatch = false; Int64 LabOrderResultId = 0;
                DSLabResult dsLabResult = new DSLabResult();
                DSLabResult.LabOrderResultRow dr = null;
                if (MDVUtility.ToInt32(model.LabResultId) < 0)
                    model.LabResultId = "0";
                BLObject<DSLabResult> obj = BLLClinicalObj.LoadLabResult(MDVUtility.ToInt64(model.LabResultId), MDVUtility.ToInt64(model.LabOrderId), model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", "", MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.PatientId));
                dsLabResult = obj.Data;
                bool isNewRecord = false;

                DSLabResult.LabOrderResultRow[] arrLabResultRows = null;
                string message = string.Empty;
                if (obj.Data != null)
                {
                    arrLabResultRows = (DSLabResult.LabOrderResultRow[])dsLabResult.LabOrderResult.Select(dsLabResult.LabOrderResult.LabOrderIdColumn.ColumnName + "=" + model.LabOrderId);
                    if (arrLabResultRows.Length > 0)
                    {
                        dr = arrLabResultRows[0];
                        message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        dr = dsLabResult.LabOrderResult.NewLabOrderResultRow();
                        dr.LabOrderId = MDVUtility.ToInt64(model.LabOrderId);
                        dr.IsActive = true;
                        message = Common.AppPrivileges.Save_Message;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        isNewRecord = true;
                    }
                }


                if (dr != null)
                {
                    dr.OrderNo = model.OrderNo;
                    dr.Status = model.Status;
                    dr.Comments = model.Comments;
                    dr.Remarks = model.Remarks;
                    dr.FinalInterpretation = model.FinalInterpretation;
                    dr.IsActive = true;
                    if (!string.IsNullOrEmpty(model.PatientId))
                    {
                        dr.PatientId = model.PatientId;
                    }
                    else
                    {
                        dr[dsLabResult.LabOrderResult.PatientIdColumn] = DBNull.Value;
                    }

                    dr.ProviderId = model.ProviderId;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    if (!string.IsNullOrEmpty(model.ReviewedById))
                    {
                        dr.ReviewedById = Convert.ToInt64(model.ReviewedById);
                    }
                    else
                    {
                        dr[dsLabResult.LabOrderResult.ReviewedByIdColumn.ColumnName] = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(model.NoteId))
                    {
                        dr.NoteId = Convert.ToInt64(model.NoteId);
                    }
                    else
                    {
                        dr[dsLabResult.LabOrderResult.NoteIdColumn.ColumnName] = DBNull.Value;
                    }

                    if (model.AssigneeId != "")
                        dr.AssigneeId = Convert.ToInt64(model.AssigneeId);
                    else
                        dr[dsLabResult.LabOrderResult.AssigneeIdColumn.ColumnName] = DBNull.Value;

                    dr.IsSentToPortal = model.IsSentToPortal;
                    dr.IsAknowledged = model.IsAknowledged;
                    dr.MarkAsReviewed = model.MarkAsReviewed;

                    if (isNewRecord)
                    {
                        dsLabResult.LabOrderResult.AddLabOrderResultRow(dr);
                    }
                }

                #region Database Insertion/Updation 
                BLObject<DSLabResult> objUpdate = BLLClinicalObj.InsertUpdateLabResult(dsLabResult);

                if (obj.Data != null)
                {
                    Int64 LabOrderResultId_ = MDVUtility.ToInt64(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName]);

                    if (string.IsNullOrEmpty(model.callFromGrid))
                    {
                        insertUpdateLabResultDetail(LabOrderResultId_, model);
                    }
                    if (!string.IsNullOrEmpty(model.PracticeId))
                    {
                        if (model.IsSentToPortal == true)
                        {
                            SendNotificationToPortal(model.PracticeId, model.PatientFullName, model.PatientId, model.PatientFacilityId);
                        }
                    }
                    // MK
                    if (dr.MarkAsReviewed)
                    {
                        try
                        {
                            string folderName = "Lab Result";
                            model.LabResultId = MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName]);
                            DSLabOrder dsLabTest = new DSLabOrder();
                            BLObject<DSLabOrder> objTest = BLLClinicalObj.LoadLabOrderTest(MDVUtility.ToInt64(model.LabOrderId), 0, MDVUtility.ToLong(model.PatientId), "1", "2000");
                            dsLabTest = objTest.Data;
                            if (dsLabTest != null && dsLabTest.LabOrderTest.Rows.Count > 0)
                            {
                                string cptdesc = MDVUtility.ToStr(dsLabTest.Tables[dsLabTest.LabOrderTest.TableName].Rows[0][dsLabTest.LabOrderTest.CPTCodeDescriptionColumn.ColumnName]);
                                if (!string.IsNullOrEmpty(cptdesc))
                                {
                                    folderName = GetLabResultFolderName(cptdesc);
                                }
                            }

                            string CCMStream = previewLabResult(model, folderName, true);
                            var serializer = new JavaScriptSerializer();
                            dynamic jsonObject = serializer.Deserialize<dynamic>(CCMStream);
                            if (jsonObject != null)
                            {
                                CCMStream = jsonObject["LabResultHTML"];
                                byte[] Filestream = null;
                                if (!String.IsNullOrEmpty(CCMStream))
                                {
                                    CCMStream = CCMStream.Replace(" ", "+");
                                    int mod4 = CCMStream.Length % 4;
                                    if (mod4 > 0)
                                    {
                                        CCMStream += new string('=', 4 - mod4);
                                    }

                                    Filestream = Convert.FromBase64String(CCMStream);
                                    dr.Url = CommonFunc.SaveDocumentToFolder(null, folderName, folderName, MDVUtility.ToLong(model.LabResultId), model.OrderNo + ".pdf", Filestream);
                                    dsLabResult.LabOrderResult[0]["Url"] = dr.Url;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // who cares
                        }
                        BLObject<DSLabResult> o_objUpdate = BLLClinicalObj.InsertUpdateLabResult(dsLabResult);
                        //MoveLabResultToPatientDocs(model.PracticeId, model.PatientFullName, model.PatientId, model.PatientFacilityId);
                    }
                    var response = new
                    {
                        status = true,
                        message = message,
                        labOrderResultId = LabOrderResultId_,
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

        #endregion

        #region Lab Result Load, Attach/Detach with Notes
        /// <summary>
        /// Module Name: loadLabResult
        /// Author: Humaira Yousaf
        /// Created Date: 17-03-2016
        /// Description: Loads Lab Results
        /// </summary> 
        /// <param name="model" type="LabResultModel">LabResultModel model containing data</param>
        public string loadLabResult(LabOrderResultModel model)
        {

            try
            {
                DSLabResult dsLab = null;
                BLObject<DSLabResult> obj;

                obj = BLLClinicalObj.LoadLabResult(MDVUtility.ToInt64(model.LabResultId), MDVUtility.ToInt64(model.LabOrderId), model.PageNumber, model.RowsPerPage, MDVUtility.ToStr(model.Test).Replace("- ", ""), model.OrderNo, 0, model.OrderFromDate, model.OrderToDate, model.Status, model.LabId, MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.PatientId));
                dsLab = obj.Data;

                if (dsLab.Tables[dsLab.LabOrderResult.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsLab.Tables[dsLab.LabOrderResult.TableName].Rows[dsLab.Tables[dsLab.LabOrderResult.TableName].Rows.Count - 1];

                    //List<Dictionary<string, string>> lstLabResults = new List<Dictionary<string, string>>();

                    long resultId = MDVUtility.ToInt64(dr[dsLab.LabOrderResult.LabOrderResultIdColumn.ColumnName]);
                    BLObject<DSLabResult> objResultDetail = BLLClinicalObj.loadResultsForSoap(MDVUtility.ToStr(resultId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.ProviderId));
                    dsLab = objResultDetail.Data;
                    //var LabResultkeyValues = new Dictionary<string, string>
                    //    {
                    //        { "LabOrderResultId",  MDVUtility.ToStr(resultId)},
                    //        { "SoapText", MDVUtility.ToStr(dr[dsLab.LabOrderResult.SoapTextColumn.ColumnName])} ,
                    //        { "LabOrderId", MDVUtility.ToStr(dr[dsLab.LabOrderResult.LabOrderIdColumn.ColumnName])}
                    //    };
                    //lstLabResults.Add(LabResultkeyValues);

                    //DSLabResult dsLabResultDetail = null;
                    //BLObject<DSLabResult> objLabResultDetail;
                    //objLabResultDetail = BLLClinicalObj.LoadLabResultDetail(0, resultId);
                    //dsLabResultDetail = objLabResultDetail.Data;

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,

                        LabResultFill_JSON = MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.LabOrderResult.TableName]),//js.Serialize(lstLabResults),
                        //LabResultDetailFill_JSON = MDVUtility.JSON_DataTable(dsLabResultDetail.Tables[dsLab.LabOrderResultDetail.TableName]),
                        LabOrderResultChild_JSON = MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.LabOrderResultDetail.TableName]),
                        LabOrderResultParent_JSON = MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.LabOrderResultSoapText.TableName]),
                        ResultSoapCount = dsLab.Tables[dsLab.LabOrderResult.TableName].Rows.Count,
                        ResultSoap_JSON = MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.LabOrderResult.TableName]),
                        iTotalDisplayRecords = (dsLab.LabOrderResult.Rows.Count > 0) ? dsLab.LabOrderResult.Rows[0][dsLab.LabOrderResult.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabResultFill_JSON = "[]",
                        LabOrderResultChild_JSON = "[]",
                        LabOrderResultParent_JSON = "[]"
                        //DiseaseFill_JSON = string.Empty,
                        // MedicalHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.MedicalHx_Disease.TableName]),
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


        public string loadLabResultDashboard(LabOrderResultModel model)
        {
            try
            {

                DSLabResult dsLabResult = new DSLabResult();
                DSLabResult dsLabResultDetail = new DSLabResult();
                List<LabOrderResultModel> resultsModel = new List<LabOrderResultModel>();

                if (model.PatientPortalStatus == "Enabled")
                {
                    model.IsPatientPortalStatus = true;
                    model.IsDisabledAccount = false;
                    model.IsUnlockAccount = true;
                }
                else if (model.PatientPortalStatus == "Not Enabled")
                {
                    model.IsPatientPortalStatus = false;
                }
                else if (model.PatientPortalStatus == "Disabled")
                {
                    model.IsPatientPortalStatus = true;
                    model.IsDisabledAccount = true;
                }
                else if (model.PatientPortalStatus == "Locked")
                {
                    model.IsPatientPortalStatus = true;
                    model.IsUnlockAccount = false;
                }

                
                BLObject<DSLabResult> objResult = BLLClinicalObj.LoadLabResult(MDVUtility.ToInt64(model.LabResultId), MDVUtility.ToInt64(model.LabOrderId), model.PageNumber, model.RowsPerPage, model.Test, model.OrderNo, MDVUtility.ToInt64(model.ProviderId), model.OrderFromDate, model.OrderToDate, model.Status, model.LabId, MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.PatientId), "", "", model.IsReviewed, model.IsAllResult, model.isReviewedFromDashBoard, null, model.Flag, model.IsPatientPortalStatus, model.IsDisabledAccount, model.IsUnlockAccount, model.PatientPortalStatus, model.AssigneeId);

                dsLabResult = objResult.Data;

                if (objResult.Data != null)
                {
                    SharedVariable sharedVariable = SharedVariable.GetSharedVariable();

                  BLObject<DSLabResult> objUnsolicitedResult =  BLLClinicalObj.LoadLabUnsolicitedResult(MDVUtility.ToInt64(model.LabResultId), MDVUtility.ToInt64(model.LabOrderId), "1", "1", model.Test, model.OrderNo, MDVUtility.ToInt64(model.ProviderId), model.CountOrderFromDate, model.CountOrderToDate, model.CountStatus, model.CountLabId, model.CountIsReviewed, model.CountIsAllResult, model.isReviewedDashBoardUnsolicited, sharedVariable);

                    // foreach (DSLabResult.LabOrderResultRow dr in dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows)
                    Parallel.ForEach(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows.OfType<DSLabResult.LabOrderResultRow>(),
                    new ParallelOptions { MaxDegreeOfParallelism = 2 },
                    (dr) =>
                        {
                           // bool isResultExists = false;
                           LabOrderResultModel resultModel = new LabOrderResultModel();
                           // DataRow dr = dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0];

                           long resultId = MDVUtility.ToInt64(dr[dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName]);

                            resultModel.LabResultId = MDVUtility.ToStr(resultId);
                            resultModel.IsSentToPortal = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.IsSentToPortalColumn.ColumnName]).ToLower() == "true" ? true : false;
                            resultModel.LabOrderId = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.LabOrderIdColumn.ColumnName]);
                            resultModel.PatientId = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.PatientIdColumn.ColumnName]);
                            resultModel.PatientName = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.PatientNameColumn.ColumnName]);
                            resultModel.PatientFullName = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.FullNameColumn.ColumnName]);
                            resultModel.AccountNumber = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.AccountNumberColumn.ColumnName]);
                            resultModel.DOB = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.DOBColumn.ColumnName]);
                            resultModel.LabName = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.LaboratoryColumn.ColumnName]);
                            resultModel.Status = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.StatusColumn.ColumnName]);
                            resultModel.OrderNo = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.OrderNoColumn.ColumnName]);
                            resultModel.AssigneeName = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.AssignedToColumn.ColumnName]);
                            resultModel.AssigneeId = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.AssigneeIdColumn.ColumnName]);
                            resultModel.Provider = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ProviderNameColumn.ColumnName]);
                            resultModel.ProviderId = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ProviderIdColumn.ColumnName]);
                            resultModel.Remarks = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.RemarksColumn.ColumnName]);
                            resultModel.Comments = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CommentsColumn.ColumnName]);
                            resultModel.CreatedBy = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CreatedByColumn.ColumnName]);
                            resultModel.CreatedOn = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CreatedOnColumn.ColumnName])) ? "" : MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CreatedOnColumn.ColumnName]);
                            resultModel.ModifiedOn = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ModifiedOnColumn.ColumnName])) ? "" : MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ModifiedOnColumn.ColumnName]);
                            resultModel.ReviewedById = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ReviewedByIdColumn.ColumnName]);
                            resultModel.ReviewedBy = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ReviewedByColumn.ColumnName]);
                            resultModel.isManually = Convert.ToBoolean(MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.isManuallyColumn.ColumnName]));
                            resultModel.IsAknowledged = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.IsAknowledgedColumn.ColumnName]).ToLower() == "true" ? true : false;
                            resultModel.IsNoteLinked = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.IsNoteLinkedColumn.ColumnName]);
                            resultModel.LabOrderResultExternalPDFId = MDVUtility.ToLong(dr[dsLabResult.LabOrderResult.LabOrderResultExternalPDFIdColumn.ColumnName]);
                            resultModel.MarkAsReviewed = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.MarkAsReviewedColumn.ColumnName]).ToLower() == "true" ? true : false;
                            resultModel.CollectionDateTime = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CollectionDateTimeColumn.ColumnName])) ? "" : MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CollectionDateTimeColumn.ColumnName]);


                            if (resultModel.IsNoteLinked == "True")
                            {
                                MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder.LabOrderResultLatestNoteModel noteModel = new MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder.LabOrderResultLatestNoteModel();
                                noteModel.NoteId = MDVUtility.ToLong(dr[dsLabResult.LabOrderResult.LatestNoteIdColumn.ColumnName]);
                                noteModel.NoteStatus = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.LatestNoteStatusColumn.ColumnName]);
                                noteModel.ProviderId = MDVUtility.ToLong(dr[dsLabResult.LabOrderResult.LatestNoteProviderIdColumn.ColumnName]);
                                resultModel.LabOrderResultLatestNoteModel = noteModel;
                            }
                           // Start PRD-423 
                           if (MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.PatientPortalStatusColumn.ColumnName]).ToLower() == "1")
                            {
                                if (MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.DisableAccountColumn.ColumnName]).ToLower() == "true" && (MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.UnlockAccountColumn.ColumnName]).ToLower() == "true" || MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.UnlockAccountColumn.ColumnName]).ToLower() == "false"))
                                {
                                    resultModel.PatientPortalStatus = "Disabled";
                                }
                                else if (MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.DisableAccountColumn.ColumnName]).ToLower() == "false" && MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.UnlockAccountColumn.ColumnName]).ToLower() == "false")
                                {
                                    resultModel.PatientPortalStatus = "Locked";
                                }
                                else
                                    resultModel.PatientPortalStatus = "Enabled";
                            }

                            else if (MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.PatientPortalStatusColumn.ColumnName]).ToLower() == "0")
                                resultModel.PatientPortalStatus = "Not Enabled";
                           // End PRD-423
                           DSLabOrder dsLabTest = new DSLabOrder();
                            
                           BLObject<DSLabOrder> objTestTask = BLLClinicalObj.LoadLabOrderTest(MDVUtility.ToInt64(resultModel.LabOrderId), 0, 0, "1", "2000", sharedVariable, MDVUtility.ToInt64(model.NoteId));

                           //Load LabOrder Result Details
                           
                           BLObject<DSLabResult> objLabResultDetailTask =  BLLClinicalObj.LoadLabResultDetail(0, MDVUtility.ToInt64(resultModel.LabResultId), sharedVariable);
                           
                           dsLabTest = objTestTask.Data;
                           dsLabResultDetail = objLabResultDetailTask.Data;


                            foreach (DataRow drLabTest in dsLabTest.Tables[dsLabTest.LabOrderTest.TableName].Rows)
                            {
                                MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder.LabOrderTestModel testModel = new MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder.LabOrderTestModel();
                                testModel.LabOrderTestId = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.LabOrderTestIdColumn.ColumnName]);
                                string LabOrderTestCPTCode = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeColumn.ColumnName]);
                                string LabOrdetTestCPTDescription = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeDescriptionColumn.ColumnName]);

                               //Fill labOrderTestModel
                               //testModel.LabOrderTestId = LabOrderTestId;
                               testModel.CPTCode = LabOrderTestCPTCode;
                                testModel.CPTDescription = LabOrdetTestCPTDescription;
                                testModel.IsResultTestNoteLinked = MDVUtility.ToBool(drLabTest[dsLabTest.LabOrderTest.IsResultTestNoteLinkedColumn.ColumnName]);

                                if (objLabResultDetailTask.Data != null)
                                {
                                    foreach (DataRow drLabResultDetail in dsLabResultDetail.Tables[dsLabResultDetail.LabOrderResultDetail.TableName].Rows)
                                    {
                                        MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder.LabOrderResultDetailModel resultDetailModel = new MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder.LabOrderResultDetailModel();

                                        var CptCode = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.CPTCodeColumn.ColumnName]);
                                        var CptDescription = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.CPTCodeDescriptionColumn.ColumnName]);
                                        LabOrdetTestCPTDescription = string.Join("", LabOrdetTestCPTDescription.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                                        CptDescription = string.Join("", CptDescription.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                                       //Push Lab Order Details
                                       if (CptCode == LabOrderTestCPTCode && (LabOrdetTestCPTDescription.ToLower().IndexOf(CptDescription.ToLower()) > -1))
                                        {
                                            resultDetailModel.LabOrderResultDetailId = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LabOrderResultDetailIdColumn.ColumnName]);
                                            resultDetailModel.LOINC = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LOINCColumn.ColumnName]);
                                            resultDetailModel.LOINCDescription = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName]);
                                            resultDetailModel.ObservationDate = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ObservationDateColumn.ColumnName]);
                                            resultDetailModel.Range = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.RangeColumn.ColumnName]);
                                            resultDetailModel.ReferenceRangeDescription = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ReferenceRangeDescriptionColumn.ColumnName]);
                                            resultDetailModel.ReferenceRangeInterpration = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ReferenceRangeInterprationColumn.ColumnName]);
                                            resultDetailModel.TestAntimicrobial = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.TestAntimicrobialColumn.ColumnName]);
                                            resultDetailModel.Result = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ResultColumn.ColumnName]);
                                            resultDetailModel.UoM = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.UoMColumn.ColumnName]);
                                            resultDetailModel.Flag = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.FlagColumn.ColumnName]);
                                            resultDetailModel.NTEText = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.NTETextColumn.ColumnName]);

                                            testModel.LabOrderResultDetails.Add(resultDetailModel);
                                           //isResultExists = true;
                                       }
                                    }
                                    if (!string.IsNullOrEmpty(testModel.CPTCode))
                                    {
                                        resultModel.LabOrderTests.Add(testModel);
                                    }

                                }
                            }
                           //   if (isResultExists)
                           //  {
                           resultsModel.Add(resultModel);

                           //   }

                       });



                    //  objUnsolicitedResult.Wait();
                    DSLabResult dsUnsolicitedResult = new DSLabResult();
                    //  dsUnsolicitedResult = objUnsolicitedResult.Result.Data;
                    dsUnsolicitedResult = objUnsolicitedResult.Data;

                    int TotalCount = (dsLabResult.LabOrderResult.Rows.Count > 0) ? MDVUtility.ToInt32(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.RecordCountColumn.ColumnName]) : 0;
                    if (objUnsolicitedResult.Data != null)
                    {
                        var UnsolicitedCount = (dsUnsolicitedResult.LabOrderResult.Rows.Count > 0) ? MDVUtility.ToInt32(dsUnsolicitedResult.LabOrderResult.Rows[0][dsUnsolicitedResult.LabOrderResult.RecordCountColumn.ColumnName]) : 0;
                        TotalCount += UnsolicitedCount;
                    }

                    var response = new
                    {
                        status = true,
                        LabOrderResultModel_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(resultsModel.OrderByDescending(x => x.ModifiedOn)),
                        LabOrderLoad_JSON = MDVUtility.JSON_DataTable(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName]),
                        LabResultDetailFill_JSON = MDVUtility.JSON_DataTable(dsLabResultDetail.Tables[dsLabResultDetail.LabOrderResultDetail.TableName]),
                        LabResultCount = dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count,
                        iTotalDisplayRecords = (dsLabResult.LabOrderResult.Rows.Count > 0) ? MDVUtility.ToInt32(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.RecordCountColumn.ColumnName]) : 0,
                        UnsolicitedPlusSolicited = TotalCount
                    };
                    // System.Diagnostics.Debug.WriteLine(@"//End//Lab test and detail load time = " + DateTime.Now.ToShortTimeString());


                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        LabOrderResultModel_JSON = "[]",
                        LabResultFill_JSON = "[]",
                        LabResultDetailFill_JSON = "[]",
                        Message = "Record Not Found"
                        //obaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
                        // FamilyHxLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName]),
                        // DiseaseLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_Disease.TableName])),
                        // MemberLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName]),
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

        public string GetAssignedLabResultsCount(string AssigneeId)
        {
            try
            {
                if (string.IsNullOrEmpty(AssigneeId))
                {
                    var response = new
                    {
                        status = false,
                        Message = "AssigneeId is null or empty."
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var objCount = BLLClinicalObj.GetAssignedLabResultsCount(MDVUtility.ToInt64(AssigneeId));
                    if (objCount.Data != "")
                    {
                        var response = new
                        {
                            status = true,
                            AssignedResultsCount = objCount.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            AssignedResultsCount = 0,
                            Message = objCount.Message
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

        public string loadLabResultUnsolicitedDashboard(LabOrderResultModel model)
        {
            try
            {

                DSLabResult dsLabResult = new DSLabResult();
                DSLabResult dsLabResultDetail = new DSLabResult();
                List<LabOrderResultModel> resultsModel = new List<LabOrderResultModel>();



                BLObject<DSLabResult> objResult = BLLClinicalObj.LoadLabUnsolicitedResult(MDVUtility.ToInt64(model.LabResultId), MDVUtility.ToInt64(model.LabOrderId), model.PageNumber, model.RowsPerPage, model.Test, model.OrderNo, MDVUtility.ToInt64(model.ProviderId), model.OrderFromDate, model.OrderToDate, model.Status, model.LabId, model.IsReviewed, model.IsAllResult, model.isReviewedDashBoardUnsolicited);
                dsLabResult = objResult.Data;

                if (objResult.Data != null)
                {
                    foreach (DSLabResult.LabOrderResultRow dr in dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows)
             
                    {
                        // bool isResultExists = false;
                        LabOrderResultModel resultModel = new LabOrderResultModel();
                        // DataRow dr = dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0];

                        long resultId = MDVUtility.ToInt64(dr[dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName]);

                        resultModel.LabResultId = MDVUtility.ToStr(resultId);
                        resultModel.LabOrderId = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.LabOrderIdColumn.ColumnName]);
                        resultModel.PatientId = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.PatientIdColumn.ColumnName]);
                        resultModel.PatientName = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.PatientNameColumn.ColumnName]);
                        resultModel.LabName = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.LaboratoryColumn.ColumnName]);
                        resultModel.Status = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.StatusColumn.ColumnName]);
                        resultModel.OrderNo = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.OrderNoColumn.ColumnName]);
                        resultModel.AssigneeName = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.AssignedToColumn.ColumnName]);
                        resultModel.AssigneeId = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.AssigneeIdColumn.ColumnName]);
                        resultModel.Provider = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ProviderNameColumn.ColumnName]);
                        resultModel.ProviderId = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ProviderIdColumn.ColumnName]);
                        resultModel.Remarks = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.RemarksColumn.ColumnName]);
                        resultModel.Comments = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CommentsColumn.ColumnName]);
                        resultModel.CreatedBy = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CreatedByColumn.ColumnName]);
                        resultModel.CreatedOn = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CreatedOnColumn.ColumnName])) ? "" : MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CreatedOnColumn.ColumnName]);
                        resultModel.ModifiedOn = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ModifiedOnColumn.ColumnName])) ? "" : MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ModifiedOnColumn.ColumnName]);
                        resultModel.ReviewedById = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ReviewedByIdColumn.ColumnName])) ? "" : MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ReviewedByIdColumn.ColumnName]);
                        resultModel.IsNoteLinked = MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.IsNoteLinkedColumn.ColumnName]);


                        DSLabOrder dsLabTest = new DSLabOrder();

                        BLObject<DSLabOrder> objTest = BLLClinicalObj.LoadLabOrderTest(MDVUtility.ToInt64(resultModel.LabOrderId), 0, 0, "1", "2000");

                        //Load LabOrder Result Details

                        BLObject<DSLabResult> objLabResultDetail = BLLClinicalObj.LoadLabResultDetail(0, MDVUtility.ToInt64(resultModel.LabResultId));

                        dsLabTest = objTest.Data;
                        dsLabResultDetail = objLabResultDetail.Data;

                        foreach (DataRow drLabTest in dsLabTest.Tables[dsLabTest.LabOrderTest.TableName].Rows)
                        {
                            MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder.LabOrderTestModel testModel = new MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder.LabOrderTestModel();
                            string LabOrderTestId = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.LabOrderTestIdColumn.ColumnName]);
                            string LabOrderTestCPTCode = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeColumn.ColumnName]);
                            string LabOrdetTestCPTDescription = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeDescriptionColumn.ColumnName]);

                            //Fill labOrderTestModel
                            testModel.LabOrderTestId = LabOrderTestId;
                            testModel.CPTCode = LabOrderTestCPTCode;
                            testModel.CPTDescription = LabOrdetTestCPTDescription;


                            if (objLabResultDetail.Data != null)
                            {
                                foreach (DataRow drLabResultDetail in dsLabResultDetail.Tables[dsLabResultDetail.LabOrderResultDetail.TableName].Rows)
                                {
                                    MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder.LabOrderResultDetailModel resultDetailModel = new MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder.LabOrderResultDetailModel();

                                    var CptCode = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.CPTCodeColumn]);
                                    var CptDescription = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.CPTCodeDescriptionColumn]);
                                    LabOrdetTestCPTDescription = string.Join("", LabOrdetTestCPTDescription.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                                    CptDescription = string.Join("", CptDescription.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                                    //Push Lab Order Details
                                    if (CptCode == LabOrderTestCPTCode && (LabOrdetTestCPTDescription.ToLower().IndexOf(CptDescription.ToLower()) > -1))
                                    {
                                        resultDetailModel.LabOrderResultDetailId = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LabOrderResultDetailIdColumn]);
                                        resultDetailModel.LOINC = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LOINCColumn]);
                                        resultDetailModel.LOINCDescription = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LOINCDescriptionColumn]);
                                        resultDetailModel.ObservationDate = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ObservationDateColumn]);
                                        resultDetailModel.Range = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.RangeColumn]);
                                        resultDetailModel.ReferenceRangeDescription = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ReferenceRangeDescriptionColumn]);
                                        resultDetailModel.ReferenceRangeInterpration = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ReferenceRangeInterprationColumn]);
                                        resultDetailModel.TestAntimicrobial = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.TestAntimicrobialColumn]);
                                        resultDetailModel.Result = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ResultColumn]);
                                        resultDetailModel.UoM = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.UoMColumn]);
                                        resultDetailModel.Flag = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.FlagColumn]);
                                        resultDetailModel.NTEText = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.NTETextColumn]);
                                        testModel.LabOrderResultDetails.Add(resultDetailModel);
                                        //isResultExists = true;
                                    }
                                }
                            }

                            resultModel.LabOrderTests.Add(testModel);
                        }
                        //   if (isResultExists)
                        //  {
                        resultsModel.Add(resultModel);

                        // }

                    }
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                    BLObject<DSLabResult> objNormalResult = BLLClinicalObj.LoadLabResult(MDVUtility.ToInt64(model.LabResultId), MDVUtility.ToInt64(model.LabOrderId), "1", "1", model.Test, model.OrderNo, MDVUtility.ToInt64(model.ProviderId), model.CountOrderFromDate, model.CountOrderToDate, model.CountStatus, model.CountLabId, MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.PatientId), "", "", model.CountIsReviewed, model.CountIsAllResult, model.isReviewedFromDashBoard);

                    DSLabResult dsNormalResult = new DSLabResult();
                    dsNormalResult = objNormalResult.Data;

                    int TotalCount = (dsLabResult.LabOrderResult.Rows.Count > 0) ? MDVUtility.ToInt32(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.RecordCountColumn.ColumnName]) : 0;
                    if (objNormalResult.Data != null)
                    {
                        var NormalCount = (dsNormalResult.LabOrderResult.Rows.Count > 0) ? MDVUtility.ToInt32(dsNormalResult.LabOrderResult.Rows[0][dsNormalResult.LabOrderResult.RecordCountColumn.ColumnName]) : 0;
                        TotalCount += NormalCount;
                    }

                    var response = new
                    {
                        status = true,
                        LabOrderUnsolicitedResultModel_JSON = js.Serialize(resultsModel),
                        LabOrderLoad_JSON = MDVUtility.JSON_DataTable(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName]),
                        LabResultDetailFill_JSON = MDVUtility.JSON_DataTable(dsLabResultDetail.Tables[dsLabResultDetail.LabOrderResultDetail.TableName]),
                        LabResultCount = dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count,
                        iTotalDisplayRecords = (dsLabResult.LabOrderResult.Rows.Count > 0) ? dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.RecordCountColumn.ColumnName] : 0,
                        UnsolicitedPlusSolicited = TotalCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabOrderResultModel_JSON = "[]",
                        LabResultFill_JSON = "[]",
                        LabResultDetailFill_JSON = "[]",
                        LabOrderLoad_JSON = "[]",
                        Message = "Record Not Found"
                        //obaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
                        // FamilyHxLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName]),
                        // DiseaseLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_Disease.TableName])),
                        // MemberLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName]),
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
        /// Method Name: attachLabResultWithNotes
        /// Author: Abid Ali
        /// Description: attaching Lab Result with notes
        /// </summary>
        /// <param name="LabResultId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string attachLabResultWithNotes(string LabResultId, long notesId)
        {
            try
            {
                DSLabResult dsLabResult = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(LabResultId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<DSLabResult> obj = BLLClinicalObj.attachLabResultWithNotes(LabResultId, notesId);
                    if (obj.Data != null)
                    {
                        dsLabResult = obj.Data;
                        var response = new
                        {
                            status = true,
                            LabResultTotalCount = dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count,
                            LabResultCount = dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count,
                            LabResultLoad_JSON = MDVUtility.JSON_DataTable(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName]),
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

        /// <summary>
        ///   Method Name: detachLabResultFromNotes
        ///   Author: Ahmad Raza
        ///   Description: Detaching Lab Result from notes
        /// </summary>
        /// <param name="LabResultId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string detachLabResultFromNotes(string LabResultId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(LabResultId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachLabOrderResultFromNotes(LabResultId, notesId);
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

        #region"Lab Result Details"
        public List<LabOrderResultDetailModel> matchCPTCodesForLabResult(List<ProviderCPTs> providerCpts, List<LabOrderResultDetailModel> lstLabOrder)
        {
            List<LabOrderResultDetailModel> labOrderWithProviderCPtsList = new List<LabOrderResultDetailModel>();

            foreach (LabOrderResultDetailModel CurrentModel in lstLabOrder)
            {
                foreach (ProviderCPTs CurrentProviderCPTsModel in providerCpts)
                {
                    if (CurrentProviderCPTsModel.CPTCode == CurrentModel.CPTCode && CurrentProviderCPTsModel.CPTCodeDescription == CurrentModel.CPTCodeDescription)
                    {
                        CurrentModel.ShowCPTCode = "0";
                        break;
                    }
                }
                labOrderWithProviderCPtsList.Add(CurrentModel);
            }

            return labOrderWithProviderCPtsList;
        }
        public string insertUpdateLabResultDetail(Int64 LabResultId, LabOrderResultModel model)
        {

            bool isError = false;
            string errorMessage = "";
            try
            {
                string remarks = model.Remarks;
                string comments = model.Comments;
                string status = model.Status;
                #region LabOrder Detail

                //If DeletedResultDetailIds has ids then delete
                if (model.DeletedResultDetailIds.Count > 0)
                {
                    foreach (Int64 item in model.DeletedResultDetailIds)
                    {

                        deleteLabResultTest(item.ToString());
                    }
                }
                DSLabResult dsLabResult = new DSLabResult();
                DSLabResult dsLabResultChild = new DSLabResult();
                BLObject<DSLabResult> objLabResultDetail = BLLClinicalObj.LoadLabResultDetail(0, LabResultId);
                dsLabResult = objLabResultDetail.Data;
                dsLabResultChild = objLabResultDetail.Data;
                List<ProviderCPTs> ProviderCPTsList = new DALLabOrder().GetProviderCPTs(MDVUtility.ToInt64(model.ProviderId));
                List<LabOrderResultDetailModel> currLabOrder = matchCPTCodesForLabResult(ProviderCPTsList, model.LabOrderResultDetailModels);
                model.LabOrderResultDetailModels = currLabOrder;

                StringBuilder sb = new StringBuilder();
                foreach (LabOrderResultDetailModel CurrentModel in model.LabOrderResultDetailModels)
                {

                    DSLabResult.LabOrderResultDetailRow RowLabResultDetail = null;
                    if (CurrentModel.ShowCPTCode == "0")
                    {
                        sb.Append("<div class='table-responsive'><table class='table table-bordered table-condensed'><thead><tr><th align='left' colspan='6'>"
                        + CurrentModel.CPTCodeDescription + "</th></tr>");
                        sb.Append("<tr style='background-color:#0188CC'><th>Date & Time</th><th>Observation</th><th>Result</th><th>UoM</th><th>Flag</th><th>Range</th></tr></thead><tbody>");
                    }
                    else
                    {
                        sb.Append("<div class='table-responsive'><table class='table table-bordered table-condensed'><thead><tr><th align='left' colspan='6'>"
                        + CurrentModel.CPTCode
                        + CurrentModel.CPTCodeDescription + "</th></tr>");
                        sb.Append("<tr style='background-color:#0188CC'><th>Date & Time</th><th>Observation</th><th>Result</th><th>UoM</th><th>Flag</th><th>Range</th></tr></thead><tbody>");
                    }

                    //Insert Update child Rows
                    foreach (var childRow in CurrentModel.ChildRows)
                    {
                        DSLabResult.LabOrderResultDetailRow[] arrChildLabResultDetailRows = (DSLabResult.LabOrderResultDetailRow[])dsLabResultChild.LabOrderResultDetail.Select(dsLabResultChild.LabOrderResultDetail.LabOrderResultDetailIdColumn.ColumnName + " = " + childRow.LabOrderResultDetailId);
                        //For child Rows
                        if (arrChildLabResultDetailRows.Length > 0)
                        {
                            RowLabResultDetail = arrChildLabResultDetailRows[0];
                        }
                        else
                        {
                            RowLabResultDetail = dsLabResultChild.LabOrderResultDetail.NewLabOrderResultDetailRow();
                            RowLabResultDetail.LabOrderResultId = LabResultId;
                            RowLabResultDetail.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowLabResultDetail.CreatedOn = DateTime.Now;
                        }

                        if (RowLabResultDetail != null)
                        {
                            #region row
                            RowLabResultDetail.LabOrderResultId = LabResultId;


                            if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.CPTCodeColumn] = MDVUtility.ToStr(CurrentModel.CPTCode);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.CPTCodeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.CPTCodeDescription))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.CPTCodeDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTCodeDescription);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.CPTCodeDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.Status))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.StatusColumn] = MDVUtility.ToStr(CurrentModel.Status);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.StatusColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.Comments))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.CommentsColumn] = MDVUtility.ToStr(CurrentModel.Comments);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.CommentsColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.ObservationDate))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ObservationDateColumn] = Convert.ToDateTime(childRow.ObservationDate);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ObservationDateColumn] = DateTime.Now;
                            }

                            if (!string.IsNullOrEmpty(childRow.LOINC))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.LOINCColumn] = MDVUtility.ToStr(childRow.LOINC);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.LOINCColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.LOINCDescription))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.LOINCDescriptionColumn] = MDVUtility.ToStr(childRow.LOINCDescription);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.LOINCDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.ConditionStatement))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ConditionStatementColumn] = MDVUtility.ToStr(childRow.ConditionStatement);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ConditionStatementColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.Result))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ResultColumn] = MDVUtility.ToStr(childRow.Result);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ResultColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.UoM))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.UoMColumn] = MDVUtility.ToStr(childRow.UoM);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.UoMColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.Flag))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.FlagColumn] = MDVUtility.ToStr(childRow.Flag);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.FlagColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.Range))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.RangeColumn] = MDVUtility.ToStr(childRow.Range);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.RangeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.ReferenceRangeDescription))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ReferenceRangeDescriptionColumn] = MDVUtility.ToStr(childRow.ReferenceRangeDescription);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ReferenceRangeDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.ReferenceRangeInterpration))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ReferenceRangeInterprationColumn] = MDVUtility.ToStr(childRow.ReferenceRangeInterpration);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ReferenceRangeInterprationColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.TestAntimicrobial))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.TestAntimicrobialColumn] = MDVUtility.ToStr(childRow.TestAntimicrobial);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.TestAntimicrobialColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(model.ProviderId))
                            {
                                childRow.PatientId = model.ProviderId;
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ProviderIdColumn] = MDVUtility.ToStr(model.ProviderId);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ProviderIdColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(model.PatientId))
                            {
                                childRow.PatientId = model.PatientId;
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.PatientIdColumn] = MDVUtility.ToStr(childRow.PatientId);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.PatientIdColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.Organism))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.IsOrganismAssociatedColumn] = MDVUtility.ToBool(childRow.Organism);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.IsOrganismAssociatedColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.OrganismCode))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.OrganismCodeColumn] = MDVUtility.ToStr(childRow.OrganismCode);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.OrganismCodeColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.OrganismCodeDescription))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.OrganismCodeDescriptionColumn] = MDVUtility.ToStr(childRow.OrganismCodeDescription);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.OrganismCodeDescriptionColumn] = DBNull.Value;
                            }

                            RowLabResultDetail.IsActive = true;
                            RowLabResultDetail.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowLabResultDetail.ModifiedOn = DateTime.Now;

                            if (arrChildLabResultDetailRows.Length < 1)
                            {
                                dsLabResultChild.LabOrderResultDetail.AddLabOrderResultDetailRow(RowLabResultDetail);
                            }
                            if (!string.IsNullOrEmpty(childRow.IsAttribute))
                            {
                                RowLabResultDetail.IsAttribute = childRow.IsAttribute == "True" ? true : false;
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.IsAttributeColumn] = DBNull.Value;
                            }

                            //-------------------------------

                            if (!string.IsNullOrEmpty(model.LabId))
                            {
                                RowLabResultDetail.LabId = MDVUtility.ToInt64(model.LabId);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.LabIdColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.LabTestId))
                            {
                                RowLabResultDetail.LabTestId = MDVUtility.ToInt64(childRow.LabTestId);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.LabTestIdColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.LabTestAttributeId))
                            {
                                RowLabResultDetail.LabTestAttributeId = MDVUtility.ToInt64(childRow.LabTestAttributeId);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.LabTestAttributeIdColumn] = DBNull.Value;
                            }
                            #endregion
                            sb.Append(labResultChildRowsSoapText(childRow));
                            //-------------------------------
                        }
                    }
                    sb.Append("</tbody></table></div>");

                }

                sb.Append(string.IsNullOrEmpty(status) ? "" : "</br><b>Status:</b> " + status + "</br>");
                sb.Append(string.IsNullOrEmpty(remarks) ? "" : "</br><b>Remarks:</b> " + remarks + "</br>");
                sb.Append(string.IsNullOrEmpty(comments) ? "" : "</br><b>Comments:</b> " + comments + "</br>");

                foreach (DataRow item in dsLabResultChild.LabOrderResultDetail.Rows)
                {
                    item[dsLabResultChild.LabOrderResultDetail.SoapTextColumn] = sb.ToString();
                }
                #region Database Insertion/Updation
                try
                {
                    BLObject<DSLabResult> objInsertedLabDetail = BLLClinicalObj.InsertUpdateLabResultDetail(dsLabResultChild);

                }
                catch (Exception ex)
                {
                    isError = true;
                    errorMessage = MDVCustomException.HumanReadableMessage(ex.Message);
                }
                #endregion
                if (!isError)
                {

                    var response = new
                    {
                        status = true,

                        message = Common.AppPrivileges.Save_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = errorMessage
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

        public string deleteLabResultTest(string LabResultDetailId)
        {

            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.DeleteLabResultDetail(LabResultDetailId);
                result = obj.Data;

                if (result == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,

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
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Lab Result LOINC

        /// <summary>
        /// Module Name: loadLabResult
        /// Author: Muhammad Arshad
        /// Created Date: 18-04-2016
        /// Description: Loads Lab Result LOINC
        /// </summary> 

        public string OrderResultLOINCLookup(string LOINCCode = "", string LOINCCOdeDescription = "", string LabId = "")
        {
            try
            {

                DSLabResult dsLab = null;
                BLObject<DSLabResult> obj;

                obj = BLLClinicalObj.LabResultLOINCLookup(LOINCCode, LOINCCOdeDescription, LabId);
                dsLab = obj.Data;

                if (dsLab.Tables[dsLab.OrderResultLOINC.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsLab.Tables[dsLab.OrderResultLOINC.TableName].Rows[dsLab.Tables[dsLab.OrderResultLOINC.TableName].Rows.Count - 1];

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabResultLOINCCount = dsLab.Tables[dsLab.OrderResultLOINC.TableName].Rows.Count,
                        LabResultLOINCLoad_JSON = MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.OrderResultLOINC.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabResultLOINCCount = 0,
                        LabResultLOINCLoad_JSON = "[]",
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
        public string OrderResultOrganismLookup(string OrganismCOdeDescription = "")
        {
            try
            {

                DSLabResult dsLab = null;
                BLObject<DSLabResult> obj;

                obj = BLLClinicalObj.LabResultOrganismLookup(OrganismCOdeDescription);
                dsLab = obj.Data;

                if (dsLab.Tables[dsLab.OrderResultOrganisms.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsLab.Tables[dsLab.OrderResultOrganisms.TableName].Rows[dsLab.Tables[dsLab.OrderResultOrganisms.TableName].Rows.Count - 1];

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabResultOrganismCount = dsLab.Tables[dsLab.OrderResultOrganisms.TableName].Rows.Count,
                        LabResultOrganismLoad_JSON = MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.OrderResultOrganisms.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabResultOrganismCount = 0,
                        LabResultOrganismLoad_JSON = "[]",
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

        #region Lab Result Attachments

        /// <summary>
        /// Module Name: loadLabResult
        /// Author: Muhammad Arshad
        /// Created Date: 26-04-2016
        /// Description: Loads Lab Result Attachment
        /// TransitionId represents LabResultId/RadiologyResultId
        /// RefModuleName represents "Lab Result" OR "Radiology Result" as specified in System.RefModule Table
        /// </summary> 

        public string loadOrderResultAttachment(Int64 PatientId, Int64 TransitionId, string RefModuleName, SharedVariable sharedVariable = null)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> objDocument = null;
                objDocument = BLLPatientObj.LoadPatientDocument("", PatientId, "", "", "", null, null, null, null, "", 0, 0, "", 0, "", "0", 0, 1, 1000, 0, TransitionId, RefModuleName, 0, 0, sharedVariable);
                dsPatient = objDocument.Data;
                if (dsPatient != null)
                {
                    List<Dictionary<string, string>> lstAttachment = new List<Dictionary<string, string>>();
                    if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows)
                        {
                            var AttachmentkeyValues = new Dictionary<string, string>
                            {
                                { "PatDocId", MDVUtility.ToStr(dr[dsPatient.PatientDocument.PatDocIdColumn.ColumnName])},
                                { "DocumentName", MDVUtility.ToStr(dr[dsPatient.PatientDocument.DocumentNameColumn.ColumnName])},
                                { "TransitionId", MDVUtility.ToStr(dr[dsPatient.PatientDocument.TransitionIdColumn.ColumnName])},
                                { "ModifiedOn", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.PatientDocument.ModifiedOnColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsPatient.PatientDocument.ModifiedOnColumn.ColumnName]).ToString()},
                                { "ModifiedBy", MDVUtility.ToStr(dr[dsPatient.PatientDocument.ModifiedByColumn.ColumnName])}
                            };
                            lstAttachment.Add(AttachmentkeyValues);
                            //byte[] byteArr = dr["FileStream"] as byte[];
                            //if (byteArr != null)
                            //{
                            //    string strBase64 = Convert.ToBase64String(byteArr);
                            //    // Add a New Column to Store the Base64 String
                            //    if (!dr.Table.Columns.Contains("Base64FileStream"))
                            //    {

                            //        dsPatient.Tables[dsPatient.PatientDocument.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                            //    }
                            //    dr["Base64FileStream"] = strBase64;
                            //}
                        }
                    }
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        OrderResultAttachmentCount = dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count,
                        OrderResultAttachmentLoad_JSON = js.Serialize(lstAttachment),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        OrderResultAttachmentCount = 0,
                        OrderResultAttachmentLoad_JSON = "[]",
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
        /// Module Name: loadLabResult
        /// Author: Muhammad Arshad
        /// Created Date: 26-04-2016
        /// Description: Loads Lab Result Attachment Preview
        /// TransitionId represents LabResultId/RadiologyResultId
        /// RefModuleName represents "Lab Result" OR "Radiology Result" as specified in System.RefModule Table
        /// </summary> 

        public string previewLoadOrderResultAttachment(string PatientDocId, Int64 PatientId, Int64 TransitionId, string RefModuleName)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> objDocument = null;
                objDocument = BLLPatientObj.LoadPatientDocument(PatientDocId, PatientId, "", "", "", null, null, null, null, "", 0, 0, "", 0, "", "1", 0, 1, 1000, 0, TransitionId, RefModuleName);
                dsPatient = objDocument.Data;
                if (dsPatient != null)
                {
                    string strBase64 = "";

                    if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count > 0)
                    {
                        bool isFirstRow = true;
                        foreach (DataRow dr in dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows)
                        {
                            if (isFirstRow == true)
                            {
                                byte[] byteArr = dr[dsPatient.PatientDocument.FileStreamColumn.ColumnName] as byte[];
                                if (byteArr != null)
                                {
                                    strBase64 = Convert.ToBase64String(byteArr);
                                    //// Add a New Column to Store the Base64 String
                                    //if (!dr.Table.Columns.Contains("Base64FileStream"))
                                    //{

                                    //    dsPatient.Tables[dsPatient.PatientDocument.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                    //}
                                    //dr["Base64FileStream"] = strBase64;
                                }

                                isFirstRow = false;
                            }

                        }
                    }
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        OrderResultAttachmentCount = dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count,
                        OrderResultAttachmentHTML = strBase64,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        OrderResultAttachmentCount = 0,
                        OrderResultAttachmentHTML = "[]",
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

        /// <summary>
        /// Method Name: loadOrderingProvider
        /// Author Name: Abid Ali
        /// Created Date: 13-04-2016
        /// Description: This function will handle Load of Provider for Lab Result
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string loadOrderingProvider(LabOrderResultModel model)
        {
            return "";
            //HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> list = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();

            //DSLabResult dsLabResult = null;
            //BLObject<DSProfileLookup> objProvider = BusinessWrapper.BLLAdminProfileObj.LookupProvider("true");
            //DSProfileLookup dsProvider = objProvider.Data;
            //BLObject<DSLabResult> obj;
            //obj = BLLClinicalObj.LoadLabResult(0, MDVUtility.ToInt64(model.PatientId), "", "", "", "", 0, "", "", "", 0);
            //dsLabResult = obj.Data;
            //list.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));
            //if (dsLabResult != null)
            //{

            //    if (dsProvider.Tables[dsProvider.Provider.TableName] != null)
            //    {

            //        DataView view = new DataView(dsLabResult.Tables[dsLabResult.LabResult.TableName]);
            //        DataTable distinctValues = view.ToTable(true, dsLabResult.LabResult.ProviderIdColumn.ColumnName);
            //        foreach (DataRow drProv in dsProvider.Tables[dsProvider.Provider.TableName].Rows)
            //        {
            //            for (int i = 0; i < distinctValues.Rows.Count; i++)
            //            {
            //                if (MDVUtility.ToInt64(drProv[dsProvider.Provider.ProviderIdColumn]) == MDVUtility.ToInt64(distinctValues.Rows[i][0].ToString()))
            //                {
            //                    list.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair(drProv[dsProvider.Provider.ShortNameColumn.ColumnName].ToString(), drProv[dsProvider.Provider.ProviderIdColumn.ColumnName].ToString(), drProv[dsProvider.Provider.EntityIdColumn.ColumnName].ToString(), drProv[dsProvider.Provider.SpecialtyIdColumn.ColumnName].ToString()));
            //                }
            //            }

            //        }
            //    }
            //}

            //return getJSONofList(list);
        }


        /// <summary>
        /// Method Name: deleteLabResult
        /// Author : Ahmad Raza
        /// Description: This function will delete the selected Lab Result
        /// </summary>
        /// <param name="LabResultId"></param>
        /// <returns></returns>
        public string deleteLabResult(string LabResultId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.DeleteLabResult(LabResultId);
                result = obj.Data;

                if (result == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        //Modified by Abid Ali
                        Message = result//obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string AcknowledgeLabResult(string LabResultId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.AcknowledgeLabResult(LabResultId);
                result = obj.Data;

                if (result == "")
                {
                    var response = new
                    {
                        status = true,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        //Modified by Abid Ali
                        Message = result//obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string deleteLabTest(string LabTestId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.DeleteLabOrderResultTest(LabTestId);
                result = obj.Data;

                if (result == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        //Modified by Abid Ali
                        Message = result//obj.Message
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
        /// Method Name: previewLabResult
        /// Author : Humaira Yousaf
        /// Created Date: 25-04-2016
        /// Description: Creates PDF to view Lab Result 
        /// </summary>
        /// <param name="model" type="LabResultModel">model</param>  
        public string previewLabResult(LabOrderResultModel model, string folderName, bool isReviewed)
        {
            try
            {
                BLObject<byte[]> obj = RequisitionsObj.GenerateLabResultReq(MDVUtility.ToInt64(model.LabResultId), MDVUtility.ToInt64(model.LabOrderId), MDVUtility.ToInt64(model.PatientId), model.BarCodeHtml, MDVSession.Current.ImagePath);

                if (obj.Data != null)
                {
                    string folderDetails = BLLClinicalObj.GetLabOrderResultDocumentFolderType(MDVUtility.ToInt64(model.LabResultId));

                    if (!string.IsNullOrEmpty(folderDetails))
                    {
                        if (folderDetails.Split(';')[1] == "0" && folderDetails.Split(';')[2] == "0")
                        {
                            folderName = folderName;
                        }
                        else if (folderDetails.Split(';')[1] == "1") // IsPSA
                        {
                            folderName = "PSA";
                        }
                        else if (folderDetails.Split(';')[2] == "1") // IsUrine
                        {
                            folderName = "urine";
                        }

                    }
                    bool IsReviewed = false;
                    if (model.MarkAsReviewed)
                    {
                        IsReviewed = true;
                    }
                    else
                    {
                        IsReviewed = BLLClinicalObj.CheckDocumentIsReviewed(MDVUtility.ToInt64(model.LabResultId), MDVUtility.ToInt64(model.PatientId), folderName);
                    }
                    string saveRequisition = Controls.Patient.Document.Patient_Document.Instance().SavePatientDocument("", MDVUtility.ToInt64(model.PatientId), null, null, null, null, null, folderName, MDVUtility.ToInt64(model.LabResultId), obj.Data, 0, 0, IsReviewed);

                    var response = new
                    {
                        status = true,
                        LabResultHTML = Convert.ToBase64String(obj.Data),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = false,
                        FaceSheetCount = 0,
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

        public string GetLabResultFolderName(string testName)
        {
            try
            {
                string folderName = "Lab Result";
                if (!string.IsNullOrEmpty(testName))
                {
                    if (testName.ToLower().Contains("psa"))
                    {
                        folderName = "PSA";
                    }
                    else if (testName.ToLower().Contains("urine"))
                    {
                        folderName = "urine";
                    }
                    else
                    {
                        folderName = "Lab Result";
                    }
                }

                return folderName;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string previewLabResultExternalPDFId(LabOrderResultModel model)
        {
            LabOrderResultExternalPDFModel pdfModel = new LabOrderResultExternalPDFModel();
            BLObject<DSLabResult> dsLab = BLLClinicalObj.LoadLabResultExternalPDF(model.LabOrderResultExternalPDFId);
            DSLabResult ds = dsLab.Data;
            pdfModel.LabOrderResultExternalPDFId = MDVUtility.ToInt64(ds.Tables[ds.LabOrderResultExternalPDF.TableName].Rows[0][ds.LabOrderResultExternalPDF.LabOrderResultExternalPDFIdColumn.ColumnName]);
            pdfModel.LabOrderResultId = MDVUtility.ToInt64(ds.Tables[ds.LabOrderResultExternalPDF.TableName].Rows[0][ds.LabOrderResultExternalPDF.LabOrderResultIdColumn.ColumnName]);
            pdfModel.FileName = MDVUtility.ToStr(ds.Tables[ds.LabOrderResultExternalPDF.TableName].Rows[0][ds.LabOrderResultExternalPDF.FileNameColumn.ColumnName]);
            pdfModel.FileBase64 = MDVUtility.ToStr(ds.Tables[ds.LabOrderResultExternalPDF.TableName].Rows[0][ds.LabOrderResultExternalPDF.FileBase64Column.ColumnName]);
            pdfModel.Status = pdfModel.FileName = MDVUtility.ToStr(ds.Tables[ds.LabOrderResultExternalPDF.TableName].Rows[0][ds.LabOrderResultExternalPDF.StatusColumn.ColumnName]);

            var response = new
            {
                status = true,
                PDFData = pdfModel
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

        }
        /// <summary>
        /// Method Name: getJSONofList
        /// Author Name: Ahmad Raza
        /// Created Date: 08-03-2016
        /// Description: This function will convert List objects to JSON
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static string getJSONofList(HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> list)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);

        }
        internal string labResultChildRowsSoapText(ChildResultDetailModel item)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr><td>" + item.ObservationDate + "</td>");
            sb.Append("<td>" + item.LOINCDescription + "<a onclick=\"Clinical_InfoButtonView.getInfofromMediPlus('" + item.LOINC + "', 'clinicalTabProgressNote','3','')\" style=\"cursor:pointer\"><b>(Info)</b></a> " + "</td>");
            sb.Append("<td>" + item.Result + "</td>");
            sb.Append("<td>" + item.UoM + "</td>");
            sb.Append("<td>" + item.Flag + "</td>");
            sb.Append("<td>" + item.Range + "</td></tr>");
            return sb.ToString();
        }
        /// <summary>
        /// Method Name: labResultSoapText
        /// Author Name: Ahmad Raza
        /// Created Date: 25-04-2016
        /// Description: This function will create Soap Text for Lab Order Result
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal string labResultSoapText(LabOrderResultDetailModel model, string status, string remarks, string comments)
        {
            string soapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (model != null)
            {
                sb.Append("<div class='table-responsive'><table class='table table-bordered table-condensed'><thead><tr><th align='left' colspan='6'>" + model.CPTCodeDescription + "</th></tr></thead><tbody>");
                sb.Append("<tr><th align='left' colspan='6'> Result (s) of " + model.CPTCodeDescription + "</th></tr>");
                sb.Append("<tr><th>Date & Time</th><th>Observation</th><th>Result</th><th>UoM</th><th>Flag</th><th>Range</th></tr>");
                if (model.ChildRows != null && model.ChildRows.Count > 0)
                {
                    foreach (var item in model.ChildRows)
                    {
                        //sb.Append("<tr><th>Date & Time</th><th>Observation</th><th>Result</th><th>UoM</th><th>Flag</th><th>Range</th></tr>");
                        sb.Append(labResultChildRowsSoapText(item));
                    }

                }
                sb.Append("</tbody></table></div>");
                sb.Append(string.IsNullOrEmpty(status) ? "" : "</br><b>Status:</b> " + status + "</br>");
                sb.Append(string.IsNullOrEmpty(remarks) ? "" : "</br><b>Remarks:</b> " + remarks + "</br>");
                sb.Append(string.IsNullOrEmpty(comments) ? "" : "</br><b>Comments:</b> " + comments + "</br>");
            }
            else
            {
                return string.Empty;
            }
            return sb.ToString();

        }



        public string fillLabResults(LabOrderResultModel model)
        {
            //test
            try
            {

                DSLabResult dsLabResult = new DSLabResult();

                BLObject<DSLabResult> objResult = BLLClinicalObj.LoadLabResult(MDVUtility.ToInt64(model.LabResultId), MDVUtility.ToInt64(model.LabOrderId), model.PageNumber, model.RowsPerPage, model.Test, model.OrderNo, MDVUtility.ToInt64(model.ProviderId), model.OrderFromDate, model.OrderToDate, model.Status, model.LabId, MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.PatientId));
                dsLabResult = objResult.Data;


                DSLabResult dsLabResultDetail = new DSLabResult();



                if (dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0];

                    long resultId = MDVUtility.ToInt64(dr[dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName]);
                    var keyValues = new Dictionary<string, string>
                    {
                        { "LabOrderResultId",  MDVUtility.ToStr(resultId)},
                        { "LabOrderId",  MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.LabOrderIdColumn.ColumnName])},
                        { "Test",  MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CPTCodeColumn.ColumnName]) +" "+ MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.CPTCodeDescriptionColumn.ColumnName])},
                        { "Laboratory", MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.LaboratoryColumn.ColumnName])},
                        { "Status", MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.StatusColumn.ColumnName])},
                        { "OrderNumber", MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.OrderNoColumn.ColumnName])},
                        { "AssignedTo", MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.AssignedToColumn.ColumnName])},
                        { "Provider", MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.ProviderNameColumn.ColumnName])},
                        { "SoapText", MDVUtility.ToStr(dr[dsLabResult.LabOrderResult.SoapTextColumn.ColumnName])}

                    };
                    DSLabResult dsLabResultDetail1 = null;
                    BLObject<DSLabResult> objLabResultDetail1;
                    objLabResultDetail1 = BLLClinicalObj.LoadLabResultDetail(0, 0);
                    dsLabResultDetail1 = objLabResultDetail1.Data;


                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabOrderLoad_JSON = MDVUtility.JSON_DataTable(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName]),
                        LabResultDetailFill_JSON = MDVUtility.JSON_DataTable(dsLabResultDetail1.Tables[dsLabResult.LabOrderResultDetail.TableName]),
                        LabResultCount = dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count,
                        iTotalDisplayRecords = (dsLabResult.LabOrderResult.Rows.Count > 0) ? dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.RecordCountColumn.ColumnName] : 0
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabResultFill_JSON = "[]",
                        LabResultDetailFill_JSON = "[]",
                        Message = "Record Not Found"
                        //obaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
                        // FamilyHxLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx.TableName]),
                        // DiseaseLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_Disease.TableName])),
                        // MemberLoad_JSON = MDVUtility.JSON_DataTable(dsFamilyHx.Tables[dsFamilyHx.FamilyHx_FamilyMemberDetail.TableName]),
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

        public string fetch_LabResultTrends(long LabOrderResultId, string FilterSearch, string DateFrom, string DateTo)
        {
            List<LabTrends> trends = BLLClinicalObj.fetch_LabResultTrends(LabOrderResultId, FilterSearch, DateFrom, DateTo);

            List<LabResultDetailTrendsModel> detail = new List<LabResultDetailTrendsModel>();

            foreach (var item in trends)
            {
                LabResultDetailTrendsModel model = new LabResultDetailTrendsModel();
                model.TestCode = item.TestCode;
                model.TestDescription = item.TestDescription;

                if (item.ResultDatesXML != "")
                {
                    XElement element = XElement.Parse(item.ResultDatesXML);
                    foreach (var resultDate in element.Elements("Date").Elements())
                    {

                        model.ResultDates.Add(Convert.ToString(resultDate.Value));
                    }
                }
                if (item.TestsXML != "")
                {
                    XElement element = XElement.Parse(item.TestsXML);

                    foreach (var resultTest in element.Elements("Test"))
                    {
                        ResultTestsTrends Tests = new ResultTestsTrends();
                        Tests.LOINC = resultTest.Element("LOINC").Value;
                        Tests.LOINCDescription = resultTest.Element("LOINCDescription").Value;
                        foreach (var resultValue in resultTest.Element("Results").Elements("DateValuePair"))
                        {
                            ResultValue rv = new ResultValue();
                            rv.Value = resultValue.Element("Result").Value;
                            rv.Date = resultValue.Element("Date").Value;
                            rv.ReferenceRange = resultValue.Element("Range").Value;
                            rv.Unit = resultValue.Element("Unit").Value;
                            rv.Flag = resultValue.Element("Flag").Value;
                            rv.Comments = resultValue.Element("Comments") == null ? "" : resultValue.Element("Comments").Value;
                            rv.ReferenceRangeInterpration = resultValue.Element("ReferenceRangeInterpration") == null ? "" : resultValue.Element("ReferenceRangeInterpration").Value;
                            rv.ReferenceRangeDescription = resultValue.Element("ReferenceRangeDescription") == null ? "" : resultValue.Element("ReferenceRangeDescription").Value;
                            rv.OrderNumber = resultValue.Element("OrderNumber").Value;
                            Tests.ResultsValues.Add(rv);
                        }

                        model.TestTrends.Add(Tests);
                    }

                }
                detail.Add(model);
            }


            var response = new
            {
                status = false,
                Trends = detail,
                PatPracticeDetails = trends[0].PatPracticeModel
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }

        public string get_LabTemps()
        {
            var trends = BLLClinicalObj.get_LabTemps();
            var response = new
            {
                letterDetails = trends
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }
        internal string getResultsForSoap(string resultIDs, long patientId, long notesid, long ProviderId)
        {
            try
            {

                DSLabResult dsResultSoap = null;
                BLObject<DSLabResult> obj = BLLClinicalObj.loadResultsForSoap(resultIDs, patientId, notesid, ProviderId);


                dsResultSoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsResultSoap.Tables[dsResultSoap.LabOrderResult.TableName].Rows.Count > 0)
                    {
                        //List<Dictionary<string, string>> lstLabResults = new List<Dictionary<string, string>>();

                        //foreach (DataRow dr in dsResultSoap.Tables[dsResultSoap.LabOrderResult.TableName].Rows)
                        //{

                        //    var LabResultkeyValues = new Dictionary<string, string>
                        //    {
                        //        { "LabOrderResultId",  MDVUtility.ToStr(dr[dsResultSoap.LabOrderResult.LabOrderResultIdColumn.ColumnName])},
                        //        { "SoapText", MDVUtility.ToStr(dr[dsResultSoap.LabOrderResult.SoapTextColumn.ColumnName])},
                        //        { "LabOrderId",  MDVUtility.ToStr(dr[dsResultSoap.LabOrderResult.LabOrderIdColumn.ColumnName])},

                        //    };
                        //    lstLabResults.Add(LabResultkeyValues);
                        //}



                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ResultSoapCount = dsResultSoap.Tables[dsResultSoap.LabOrderResult.TableName].Rows.Count,
                            LabResultFill_JSON = MDVUtility.JSON_DataTable(dsResultSoap.Tables[dsResultSoap.LabOrderResult.TableName]), //js.Serialize(lstLabResults), //Utility.JSON_DataTable(dsResultSoap.Tables[dsResultSoap.LabOrderResult.TableName]),
                            LabOrderResultChild_JSON = MDVUtility.JSON_DataTable(dsResultSoap.Tables[dsResultSoap.LabOrderResultDetail.TableName]),
                            LabOrderResultParent_JSON = MDVUtility.JSON_DataTable(dsResultSoap.Tables[dsResultSoap.LabOrderResultSoapText.TableName])
                            // MedicationReviewSoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.MedicationReview.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ResultSoapCount = 0,
                            LabResultFill_JSON = "[]", //Utility.JSON_DataTable(dsResultSoap.Tables[dsResultSoap.LabOrderResult.TableName]),
                            LabOrderResultChild_JSON = "[]",
                            LabOrderResultParent_JSON = "[]",
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

        #region HL7 Lab Results

        public string parseLabResultHL7Message()
        {
            bool isError = false;
            string PatientId = string.Empty;
            try
            {

                string labResultInboxPath = "~/EMR/HL7Folder/LabResults/InBox/";
                string labResultReadFilesPath = "~/EMR/HL7Folder/LabResults/ReadFiles/";
                string labResultErrorFilesPath = "~/EMR/HL7Folder/LabResults/ErrorFiles/";
                // Get list of files in the specific directory. txt files only
                string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath(labResultInboxPath), "*.txt", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string FileName = file;
                    string LabInboxFilePath = FileName;// HttpContext.Current.Server.MapPath(labResultInboxPath + FileName);
                    if (File.Exists(LabInboxFilePath))
                    {
                        string message = File.ReadAllText(LabInboxFilePath, Encoding.UTF8);
                        List<OBR_HL7> ORUDetailInfo = ClinicalHL7Reader.Instance().getHL7Message(message);
                        if (ORUDetailInfo != null && ORUDetailInfo.Count > 0)
                        {
                            foreach (OBR_HL7 itemOBR in ORUDetailInfo)
                            {
                                bool isChild = false;
                                if (itemOBR.OrderId == null && !string.IsNullOrEmpty(itemOBR.USIIdentifier))
                                {
                                    itemOBR.OrderId = ORUDetailInfo[0].OrderId;
                                    isChild = true;
                                }
                                if (itemOBR.OrderId != null)
                                {
                                    DSLabOrder dsLabOrder = null;//, dsLabProblems = null;
                                    BLObject<DSLabOrder> obj = BLLClinicalObj.LoadLabOrder(MDVUtility.ToInt64(itemOBR.OrderId), 0, 0, null, null, "", "", 0, "", "", "", 0);
                                    if (obj.Data != null)
                                    {
                                        dsLabOrder = obj.Data;
                                        //BLObject<DSLabOrder> objTest = BLLClinicalObj.LoadLabOrderTest(MDVUtility.ToInt64(model.LabOrderId), 0, MDVUtility.ToInt64(model.PatientId), "1", "2000");
                                        //dsLabTest = objTest.Data;
                                        if (dsLabOrder.LabOrder.Rows.Count > 0)
                                        {
                                            LabOrderResultModel model = new LabOrderResultModel();
                                            DataRow drLabOrder = dsLabOrder.LabOrder.Rows[0];
                                            PatientId = MDVUtility.ToStr(drLabOrder[dsLabOrder.LabOrder.PatientIdColumn.ColumnName]);
                                            string ProviderId = MDVUtility.ToStr(drLabOrder[dsLabOrder.LabOrder.ProviderIdColumn.ColumnName]);
                                            string OrderNo = MDVUtility.ToStr(drLabOrder[dsLabOrder.LabOrder.OrderNoColumn.ColumnName]);
                                            LabOrderResultDetailModel modelDetail = new LabOrderResultDetailModel();
                                            modelDetail.DateTime = itemOBR.ObserVationDateTime.ToShortDateString();
                                            modelDetail.LabOrderResultDetailId = "";
                                            modelDetail.LabOrderResultId = "";
                                            modelDetail.ObservationDate = itemOBR.ObserVationDateTime.ToShortDateString();
                                            modelDetail.Status = itemOBR.ResultStatus;
                                            modelDetail.CPTCode = itemOBR.USIIdentifier;
                                            modelDetail.CPTCodeDescription = itemOBR.USIText;
                                            modelDetail.Comments = itemOBR.Comments;
                                            foreach (var itemOBX in itemOBR.OBXmodalList)
                                            {
                                                ChildResultDetailModel dm = new ChildResultDetailModel();
                                                dm.Flag = itemOBX.AbnormalFlags;
                                                dm.LOINC = itemOBX.LoincCode;
                                                dm.LOINCDescription = itemOBX.LoincDesc;
                                                dm.ObservationDate = (itemOBX.DateTimeOfTheObservation).ToString();
                                                dm.ObservationResultStatus = itemOBX.ObservationResultStatus;
                                                dm.ObservationValue = itemOBX.ObservationValue;
                                                dm.PatientId = PatientId;
                                                dm.ProviderId = ProviderId;
                                                dm.Range = itemOBX.ReferencesRange;
                                                dm.Result = itemOBX.ObservationResultStatus;
                                                dm.UoM = itemOBX.Units;
                                                dm.LabOrderResultId = "";
                                                dm.LabOrderResultDetailId = "";

                                                modelDetail.ChildRows.Add(dm);
                                            }
                                            //   model.LabOrderId = itemOBR.PlacerOrderNumberLabTestId;
                                            model.LabOrderId = itemOBR.OrderId;

                                            model.ProviderId = ProviderId;
                                            model.PatientId = PatientId;
                                            model.OrderNo = OrderNo;
                                            model.LabResultId = null;
                                            model.Status = itemOBR.ResultStatus;

                                            model.LabOrderResultDetailModels.Add(modelDetail);

                                            // Insert / Update Lab Results
                                            isError = insertUpdateLabResultHL7(model, itemOBR);

                                            //  insertUpdateLabResultDetailHL7(-1, model);
                                            if (!isError)
                                            {
                                                BLObject<DSLabOrder> objTest = BLLClinicalObj.LoadLabOrderTest(MDVUtility.ToInt64(model.LabOrderId), 0, MDVUtility.ToInt64(model.PatientId), "1", "2000");
                                                DSLabOrder dsLabTest = objTest.Data;
                                                if (dsLabTest != null && dsLabTest.LabOrderTest.Rows.Count > 0)
                                                {
                                                    foreach (DataRow RowLabOrderTest in dsLabTest.LabOrderTest.Rows)
                                                    {
                                                        long LabOrderTestId = MDVUtility.ToLong(RowLabOrderTest[dsLabTest.LabOrderTest.LabOrderTestIdColumn].ToString());
                                                        int counterOBR = MDVUtility.ToInt32(itemOBR.OBRSetId) - 1;
                                                        counterOBR = counterOBR > itemOBR.OBXmodalList.Count ? counterOBR : itemOBR.OBXmodalList.Count - 1;
                                                        if (itemOBR.OBXmodalList != null && itemOBR.OBXmodalList.Count > 0 && !string.IsNullOrEmpty(itemOBR.OBXmodalList[counterOBR].FillerInstructions))
                                                        {
                                                            RowLabOrderTest[dsLabTest.LabOrderTest.FillerInstructionColumn] = MDVUtility.ToStr(itemOBR.OBXmodalList[counterOBR].FillerInstructions);

                                                        }
                                                        RowLabOrderTest[dsLabTest.LabOrderTest.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                                        RowLabOrderTest[dsLabTest.LabOrderTest.ModifiedOnColumn] = DateTime.Now;
                                                        BLObject<DSLabOrder> objInsertedLabTest = BLLClinicalObj.insertUpdateLabOrderTest(dsLabTest, PatientId);
                                                        if (objInsertedLabTest.Data == null)
                                                        {
                                                            isError = true;
                                                        }
                                                        #region SPM insert to db
                                                        if (!isError)
                                                        {
                                                            if (itemOBR.SPMmodalList.Count > 0)
                                                            {
                                                                DSLabResult dsSPM = new DSLabResult();
                                                                foreach (SPM_HL7 item in itemOBR.SPMmodalList)
                                                                {
                                                                    DSLabResult.LabResultSpecimenRow drSPM = dsSPM.LabResultSpecimen.NewLabResultSpecimenRow();
                                                                    drSPM[dsSPM.LabResultSpecimen.LabOrderTestIdColumn] = LabOrderTestId;// item.LabOrderId;
                                                                    if (item.CollectionDateTime != null)
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.CollectionDateTimeColumn] = MDVUtility.ToDateTime(item.CollectionDateTime);
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.CollectionDateTimeColumn] = DBNull.Value;
                                                                    }
                                                                    if (!string.IsNullOrEmpty(item.NameofCodingSystem))
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.NameofCodingSystemColumn] = item.NameofCodingSystem;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.NameofCodingSystemColumn] = DBNull.Value;
                                                                    }
                                                                    if (!string.IsNullOrEmpty(item.OriginalText))
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.OriginalTextColumn] = item.OriginalText;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.OriginalTextColumn] = DBNull.Value;
                                                                    }
                                                                    //
                                                                    if (!string.IsNullOrEmpty(item.Quantity))
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.QuantityColumn] = item.Quantity;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.QuantityColumn] = DBNull.Value;
                                                                    }
                                                                    if (item.SpecimenId > 0)
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.SpecimenIdColumn] = item.SpecimenId;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.SpecimenIdColumn] = DBNull.Value;
                                                                    }
                                                                    if (!string.IsNullOrEmpty(item.SpecimenType))
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.SpecimenTypeColumn] = item.SpecimenType;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.SpecimenTypeColumn] = DBNull.Value;
                                                                    }
                                                                    if (!string.IsNullOrEmpty(item.Text))
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.TextColumn] = item.Text;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.TextColumn] = DBNull.Value;
                                                                    }
                                                                    // SPECIMEN CONDTION
                                                                    if (!string.IsNullOrEmpty(item.Identifier))
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.IdentifierColumn] = item.Identifier;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.IdentifierColumn] = DBNull.Value;
                                                                    }
                                                                    if (!string.IsNullOrEmpty(item.ConditionText))
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.ConditionTextColumn] = item.ConditionText;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.ConditionTextColumn] = DBNull.Value;
                                                                    }
                                                                    if (!string.IsNullOrEmpty(item.Text))
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.ConditionNOCSystemColumn] = item.Text;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.ConditionNOCSystemColumn] = DBNull.Value;
                                                                    }
                                                                    if (!string.IsNullOrEmpty(item.Text))
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.AlternateIdentifierColumn] = item.Text;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.AlternateIdentifierColumn] = DBNull.Value;
                                                                    }
                                                                    //
                                                                    if (!string.IsNullOrEmpty(item.AlternateText))
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.AlternateTextColumn] = item.AlternateText;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.AlternateTextColumn] = DBNull.Value;
                                                                    }
                                                                    if (!string.IsNullOrEmpty(item.NameofAlternateCodingSystem))
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.NameofAlternateCodingSystemColumn] = item.NameofAlternateCodingSystem;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.NameofAlternateCodingSystemColumn] = DBNull.Value;
                                                                    }
                                                                    if (!string.IsNullOrEmpty(item.ConditionOriginalText))
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.ConditionOriginalTextColumn] = item.ConditionOriginalText;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSPM[dsSPM.LabResultSpecimen.ConditionOriginalTextColumn] = DBNull.Value;
                                                                    }


                                                                    dsSPM.LabResultSpecimen.AddLabResultSpecimenRow(drSPM);
                                                                }
                                                                BLObject<DSLabResult> objLabResultSpecimen = BLLClinicalObj.insertLabResultSpecimen(dsSPM);
                                                                if (objLabResultSpecimen.Data != null)
                                                                {
                                                                    dsSPM = objLabResultSpecimen.Data;
                                                                    foreach (var item in itemOBR.SPMmodalList)
                                                                    {
                                                                        if (item.RejectReasonList.Count > 0)
                                                                        {
                                                                            foreach (SPM_RejectReason_HL7 itemRejectReason in item.RejectReasonList)
                                                                            {
                                                                                DSLabResult.SpecimenRejectReasonRow drSPM = dsSPM.SpecimenRejectReason.NewSpecimenRejectReasonRow();
                                                                                drSPM[dsSPM.SpecimenRejectReason.LabOrderTestIdColumn] = LabOrderTestId;// item.LabOrderId;

                                                                                drSPM[dsSPM.SpecimenRejectReason.IdentifierColumn] = itemRejectReason.Identifier;
                                                                                drSPM[dsSPM.SpecimenRejectReason.TextColumn] = itemRejectReason.Text;
                                                                                drSPM[dsSPM.SpecimenRejectReason.NameofCodingSystemColumn] = itemRejectReason.NameofCodingSystem;
                                                                                drSPM[dsSPM.SpecimenRejectReason.AlternateIdentifierColumn] = itemRejectReason.AlternateIdentifier;
                                                                                drSPM[dsSPM.SpecimenRejectReason.AlternateTextColumn] = itemRejectReason.AlternateText;
                                                                                drSPM[dsSPM.SpecimenRejectReason.OriginalTextColumn] = itemRejectReason.OriginalText;
                                                                                drSPM[dsSPM.SpecimenRejectReason.LabResultSpecimenIdColumn] = dsSPM.Tables[dsSPM.LabResultSpecimen.TableName].Rows[0][dsSPM.LabResultSpecimen.LabResultSpecimenIdColumn];
                                                                                dsSPM.SpecimenRejectReason.AddSpecimenRejectReasonRow(drSPM);
                                                                            }
                                                                            BLObject<DSLabResult> objSpecimenRejectReason = BLLClinicalObj.insertLabResultSpecimenRejectReason(dsSPM);
                                                                            if (objSpecimenRejectReason.Data == null)
                                                                            {
                                                                                isError = true;
                                                                            }
                                                                            else
                                                                            {
                                                                                isError = false;
                                                                            }
                                                                        }
                                                                    }


                                                                }
                                                                else
                                                                {
                                                                    isError = false;
                                                                }
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                }



                                            }

                                        }
                                        else
                                        {
                                            isError = true;
                                        }
                                    }
                                }
                            }
                            if (!isError)
                            {
                                fileMove(labResultInboxPath, labResultReadFilesPath, FileName);
                            }
                            //If HL7 Lab result message has some error, than move that message to error files folder of HL7
                            else
                            {
                                fileMove(labResultInboxPath, labResultErrorFilesPath, FileName);
                            }
                        }
                        else
                        {
                            fileMove(labResultInboxPath, labResultErrorFilesPath, FileName);
                            var response = new
                            {
                                status = false,
                                Message = "HL7 Message not exists"
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            if (isError)
            {
                var response = new
                {
                    status = false,
                    Message = "Message has some errors, could not Parsed"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = true,
                    PatientId = PatientId,
                    Message = "Message Successfully Parsed"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private void fileMove(string PathFrom, string PathTo, string FileName)
        {
            string filename = Path.GetFileName(FileName);
            string filenameNoExtension = Path.GetFileNameWithoutExtension(FileName);
            if (!File.Exists(HttpContext.Current.Server.MapPath(PathTo + filename)))
            {
                File.Move(HttpContext.Current.Server.MapPath(PathFrom + filename), HttpContext.Current.Server.MapPath(PathTo + filename));
            }
            else
            {
                File.Move(HttpContext.Current.Server.MapPath(PathFrom + filename), HttpContext.Current.Server.MapPath(PathTo + SpecialFileName(filenameNoExtension)));
            }
        }
        private string SpecialFileName(string Filename)
        {
            // A
            return string.Format("{0}{1}" + Filename + "-{2:yyyy-MM-dd_hh-mm-ss-tt}.txt",
            // B
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            // C
            Path.DirectorySeparatorChar,
            // D
            DateTime.Now);
        }
        #region"Lab Result Details"

        public bool insertUpdateLabResultHL7(LabOrderResultModel model, OBR_HL7 itemOBR)
        {

            try
            {
                DSLabResult dsLabResult = new DSLabResult();
                DSLabResult.LabOrderResultRow dr = null;
                if (MDVUtility.ToInt32(model.LabResultId) < 0)
                    model.LabResultId = "0";
                BLObject<DSLabResult> obj = BLLClinicalObj.LoadLabResult(MDVUtility.ToInt64(model.LabResultId), MDVUtility.ToInt64(model.LabOrderId), model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", "", MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.PatientId));
                dsLabResult = obj.Data;
                bool isNewRecord = false;

                DSLabResult.LabOrderResultRow[] arrLabResultRows = null;
                string message = string.Empty;
                if (obj.Data != null)
                {
                    arrLabResultRows = (DSLabResult.LabOrderResultRow[])dsLabResult.LabOrderResult.Select(dsLabResult.LabOrderResult.LabOrderIdColumn.ColumnName + "=" + model.LabOrderId);
                    if (arrLabResultRows.Length > 0)
                    {
                        dr = arrLabResultRows[0];
                        message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        dr = dsLabResult.LabOrderResult.NewLabOrderResultRow();
                        dr.LabOrderId = MDVUtility.ToInt64(model.LabOrderId);
                        dr.IsActive = true;
                        message = Common.AppPrivileges.Save_Message;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        isNewRecord = true;
                    }
                }


                if (dr != null)
                {
                    dr.OrderNo = model.OrderNo;
                    dr.Status = model.Status;
                    dr.Comments = model.Comments;
                    dr.Remarks = model.Remarks;
                    dr.IsActive = true;
                    dr.PatientId = model.PatientId;
                    dr.ProviderId = model.ProviderId;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    if (isNewRecord)
                    {
                        dsLabResult.LabOrderResult.AddLabOrderResultRow(dr);
                    }
                }

                #region Database Insertion/Updation

                BLObject<DSLabResult> objUpdate = BLLClinicalObj.InsertUpdateLabResult(dsLabResult);

                if (obj.Data != null)
                {

                    Int64 LabOrderResultId = MDVUtility.ToInt64(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName]);
                    bool result = insertUpdateLabResultDetailHL7(LabOrderResultId, model);

                    return result;

                }
                else
                {
                    return false;

                }

                #endregion


            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
        public bool insertUpdateLabResultDetailHL7(Int64 LabResultId, LabOrderResultModel model)
        {

            bool isError = false;
            string errorMessage = "";
            try
            {
                string remarks = model.Remarks;
                string comments = model.Comments;
                string status = model.Status;
                #region LabOrder Detail

                ////If DeletedResultDetailIds has ids then delete
                //if (model.DeletedResultDetailIds.Count > 0)
                //{
                //    foreach (Int64 item in model.DeletedResultDetailIds)
                //    {

                //        deleteLabResultTest(item.ToString());
                //    }

                //}
                DSLabResult dsLabResult = new DSLabResult();
                DSLabResult dsLabResultChild = new DSLabResult();
                BLObject<DSLabResult> objLabResultDetail = BLLClinicalObj.LoadLabResultDetail(0, LabResultId);
                dsLabResult = objLabResultDetail.Data;
                dsLabResultChild = objLabResultDetail.Data;



                foreach (LabOrderResultDetailModel CurrentModel in model.LabOrderResultDetailModels)
                {

                    DSLabResult.LabOrderResultDetailRow RowLabResultDetail = null;
                    //Insert Update child Rows
                    foreach (var childRow in CurrentModel.ChildRows)
                    {
                        Int64 LabOrderResultDetailId = (string.IsNullOrEmpty(childRow.LabOrderResultDetailId) ? -1 : MDVUtility.ToInt64(childRow.LabOrderResultDetailId));
                        //DSLabResult.LabOrderResultDetailRow[] arrChildLabResultDetailRows = (DSLabResult.LabOrderResultDetailRow[])dsLabResultChild.LabOrderResultDetail.Select(dsLabResultChild.LabOrderResultDetail.CPTCodeColumn.ColumnName + " = '" + CurrentModel.CPTCode + "' or " + dsLabResultChild.LabOrderResultDetail.LOINCColumn.ColumnName + " = '" + childRow.LOINC + "'");
                        //DSLabResult.LabOrderResultDetailRow[] arrChildLabResultDetailRows = (DSLabResult.LabOrderResultDetailRow[])dsLabResultChild.LabOrderResultDetail.Select(dsLabResultChild.LabOrderResultDetail.LabOrderResultDetailIdColumn.ColumnName + " = " + LabOrderResultDetailId + " and "
                        DSLabResult.LabOrderResultDetailRow[] arrChildLabResultDetailRows = (DSLabResult.LabOrderResultDetailRow[])dsLabResultChild.LabOrderResultDetail.Select(dsLabResultChild.LabOrderResultDetail.LOINCColumn.ColumnName + " = " + MDVUtility.ToLINQFormatString(childRow.LOINC) + " and "
                            + dsLabResultChild.LabOrderResultDetail.LabOrderResultIdColumn.ColumnName + " = '" + LabResultId + "'");
                        //    + dsLabResultChild.LabOrderResultDetail.ResultColumn.ColumnName + " = '" + childRow.ObservationValue + "'");
                        // For child Rows
                        if (arrChildLabResultDetailRows.Length > 0 && arrChildLabResultDetailRows[0].LabOrderResultDetailId > 0)
                        {
                            RowLabResultDetail = arrChildLabResultDetailRows[0];
                        }
                        else
                        {
                            RowLabResultDetail = dsLabResultChild.LabOrderResultDetail.NewLabOrderResultDetailRow();
                            RowLabResultDetail.LabOrderResultId = LabResultId;
                            RowLabResultDetail.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowLabResultDetail.CreatedOn = DateTime.Now;
                        }

                        if (RowLabResultDetail != null)
                        {

                            RowLabResultDetail.LabOrderResultId = LabResultId;

                            if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.SoapTextColumn] = labResultSoapText(CurrentModel, status, remarks, comments);
                            }


                            if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.CPTCodeColumn] = MDVUtility.ToStr(CurrentModel.CPTCode);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.CPTCodeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.CPTCodeDescription))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.CPTCodeDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTCodeDescription);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.CPTCodeDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.Status))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.StatusColumn] = MDVUtility.ToStr(CurrentModel.Status);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.StatusColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.Comments))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.CommentsColumn] = MDVUtility.ToStr(CurrentModel.Comments);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.CommentsColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.ObservationDate))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ObservationDateColumn] = Convert.ToDateTime(childRow.ObservationDate);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ObservationDateColumn] = DateTime.Now;
                            }

                            if (!string.IsNullOrEmpty(childRow.LOINC))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.LOINCColumn] = MDVUtility.ToStr(childRow.LOINC);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.LOINCColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.LOINCDescription))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.LOINCDescriptionColumn] = MDVUtility.ToStr(childRow.LOINCDescription);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.LOINCDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.Result))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ObservationValueColumn] = MDVUtility.ToStr(childRow.Result);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ObservationValueColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.UoM))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.UoMColumn] = MDVUtility.ToStr(childRow.UoM);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.UoMColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.Flag))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.FlagColumn] = MDVUtility.ToStr(childRow.Flag);

                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.FlagColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.Range))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.RangeColumn] = MDVUtility.ToStr(childRow.Range);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.RangeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.ReferenceRangeDescription))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ReferenceRangeDescriptionColumn] = MDVUtility.ToStr(childRow.ReferenceRangeDescription);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ReferenceRangeDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.ReferenceRangeInterpration))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ReferenceRangeInterprationColumn] = MDVUtility.ToStr(childRow.ReferenceRangeInterpration);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ReferenceRangeInterprationColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.TestAntimicrobial))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.TestAntimicrobialColumn] = MDVUtility.ToStr(childRow.TestAntimicrobial);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.TestAntimicrobialColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(model.ProviderId))
                            {
                                childRow.PatientId = model.ProviderId;
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ProviderIdColumn] = MDVUtility.ToStr(model.ProviderId);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ProviderIdColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(model.PatientId))
                            {
                                childRow.PatientId = model.PatientId;
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.PatientIdColumn] = MDVUtility.ToStr(childRow.PatientId);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.PatientIdColumn] = DBNull.Value;
                            }
                            //add by azhar
                            if (!string.IsNullOrEmpty(childRow.ObservationValue))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ResultColumn] = MDVUtility.ToStr(childRow.ObservationValue);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ResultColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.ObservationResultStatus))
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ObservationResultStatusColumn] = MDVUtility.ToStr(childRow.ObservationResultStatus);
                            }
                            else
                            {
                                RowLabResultDetail[dsLabResultChild.LabOrderResultDetail.ObservationResultStatusColumn] = DBNull.Value;
                            }
                            //end added by Azhar

                            RowLabResultDetail.IsActive = true;
                            RowLabResultDetail.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowLabResultDetail.ModifiedOn = DateTime.Now;

                            // if (arrChildLabResultDetailRows.Length < 1)
                            //{
                            if (arrChildLabResultDetailRows.Length < 1 || RowLabResultDetail.LabOrderResultDetailId < 1)
                            {
                                dsLabResultChild.LabOrderResultDetail.AddLabOrderResultDetailRow(RowLabResultDetail);
                            }
                        }
                    }

                }
                #region Database Insertion/Updation
                try
                {
                    BLObject<DSLabResult> objInsertedLabDetail = BLLClinicalObj.InsertUpdateLabResultDetail(dsLabResultChild);

                }
                catch (Exception ex)
                {
                    isError = true;
                    errorMessage = MDVCustomException.HumanReadableMessage(ex.Message);
                }
                #endregion
                return isError;

                #endregion
            }
            catch (Exception ex)
            {
                errorMessage = MDVCustomException.HumanReadableMessage(ex.Message);
                return isError = false;
            }
        }

        #endregion

        #endregion

        public string attachLabOrderTestWithNotes(long LabOrderTestId, long NotesId)
        {
            try
            {
                string obj = BLLClinicalObj.attachLabOrderTestWithNotes(LabOrderTestId, NotesId);
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string detachLabOrderTestWithNotes(long LabOrderTestId, long NotesId)
        {
            try
            {
                string obj = BLLClinicalObj.detachLabOrderTestWithNotes(LabOrderTestId, NotesId);
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string UploadLabResultHL7Messages(HttpFileCollection files)
        {
            try
            {
                int iUploadedCnt = 0;

                // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
                string sPath = "";
                sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/EMR/HL7Folder/LabResults/InBox/");
                System.Web.HttpFileCollection hfc = files;

                // CHECK THE FILE COUNT.
                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    System.Web.HttpPostedFile hpf = hfc[iCnt];

                    if (hpf.ContentLength > 0)
                    {
                        // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                        if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                        {
                            // SAVE THE FILES IN THE FOLDER.
                            hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                            iUploadedCnt = iUploadedCnt + 1;
                        }
                    }
                }

                // RETURN A MESSAGE (OPTIONAL).
                if (iUploadedCnt > 0)
                {
                    return parseLabResultHL7Message();
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Upload Failed"
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

        #region Service Command Handler
        /// <summary>
        /// Handle the Basic Free Group Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "SAVE_PATIENT_DOCUMENT":
                    {

                        string strJSONData = "";
                        try
                        {
                            if (context.Request.Files.Count > 0)
                            {
                                strJSONData = UploadLabResultHL7Messages(context.Request.Files);
                            }
                        }
                        catch (Exception ex)
                        {
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);

                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

            }
        }
        #endregion
        /// <summary>
        /// Method Name: viewPdfLabResult
        /// Author Name: Humaira Yousaf
        /// Created Date: 06-05-2016
        /// Description: Views pdf
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string viewPdfLabResult(LabOrderResultModel model)
        {
            try
            {
                BLObject<byte[]> obj = BLLClinicalObj.viewPdfLabResult(MDVUtility.ToInt64(model.LabResultId), MDVUtility.ToInt64(model.LabOrderId), MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        LabOrderHTML = Convert.ToBase64String(obj.Data),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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

        #region "Send Notification to Patient Portal"

        private string SendNotificationToPortal(string pracId, string patientName, string patId, string facId)
        {
            try
            {
                string[] Patient_Name = patientName.Split(',');
                string pat_name = Patient_Name[1] + " " + Patient_Name[0];
                string message = GetFormattedMessage(pracId, pat_name, facId);
                Guid id = Guid.NewGuid();
                string unique_no = id.ToString();
                //DSMessage dsMessage = new DSMessage();
                //DSMessage.PatMessagesRow dr = dsMessage.PatMessages.NewPatMessagesRow();
                //dr.PatientId = MDVUtility.ToInt64(patId);
                //dr.MsgDetail = message;
                //dr.UserId = MDVUtility.ToStr(MDVSession.Current.AppUserId);
                //dr.IsActive = true;
                //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                //dr.CreatedOn = DateTime.Now;
                //dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                //dr.ModifiedOn = DateTime.Now;
                //dr.Subject = "New Remarks regarding Lab Results";
                //dr.IsRead = false;

                DSMessage dsMessage = new DSMessage();
                DSMessage.UserMessagesRow dr = dsMessage.UserMessages.NewUserMessagesRow();

                dr.AssignedFromId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                dr.AttatchedPatientId = MDVUtility.ToInt64(patId);
                dr.Subject = "New Remarks regarding Lab Results";
                dr.MessageDetail = message;
                dr.UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.IsRead = false;
                dr.Entityid = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                dr.MessagerType = "Patient";
                dr.UniqueNumber = unique_no;

                #region Database Insertion
                dsMessage.UserMessages.AddUserMessagesRow(dr);
                BLObject<DSMessage> obj = BLLMessageObj.InsertPracticeMessage(dsMessage);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        UserMessageId = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0][dsMessage.UserMessages.UserMessagesIdColumn.ColumnName],
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

        private string GetFormattedMessage(string pracId, string patientName, string facId)
        {
            DSProfile dsFacility = null;
            //BLObject<DSProfile> objPractice = BLLAdminProfileObj.LoadPractice(MDVUtility.ToInt64(pracId), null, null, null, null, null);
            BLObject<DSProfile> objFacility = BLLAdminProfileObj.LoadFacility(MDVUtility.ToInt64(facId), null, null, null, null, null);
            dsFacility = objFacility.Data;

            DataRow drPractice = dsFacility.Tables[dsFacility.Facility.TableName].Rows[0];

            string facilityname = MDVUtility.ToStr(drPractice[dsFacility.Facility.ShortNameColumn.ColumnName]);
            string facilityaddress = MDVUtility.ToStr(drPractice[dsFacility.Facility.AddressColumn.ColumnName]);
            string facilityaddress2 = MDVUtility.ToStr(drPractice[dsFacility.Facility.AddressColumn.ColumnName]);
            string facilityCity = MDVUtility.ToStr(drPractice[dsFacility.Facility.CityColumn.ColumnName]);
            string facilityState = MDVUtility.ToStr(drPractice[dsFacility.Facility.StateColumn.ColumnName]);
            string facilityZip = MDVUtility.ToStr(drPractice[dsFacility.Facility.ZIPCodeColumn.ColumnName]);
            string facilitPhone = MDVUtility.ToStr(drPractice[dsFacility.Facility.PhoneNoColumn.ColumnName]);
            string facilitFax = MDVUtility.ToStr(drPractice[dsFacility.Facility.FaxColumn.ColumnName]);


            //string practicename = MDVUtility.ToStr(drPractice[dsPractice.Practice.ShortNameColumn.ColumnName]);
            //string practiceadd1 = MDVUtility.ToStr(drPractice[dsPractice.Practice.AddressColumn.ColumnName]);
            //string practiceadd2 = MDVUtility.ToStr(drPractice[dsPractice.Practice.Address2Column.ColumnName]);
            //string practicecity = MDVUtility.ToStr(drPractice[dsPractice.Practice.CityColumn.ColumnName]);
            //string practicestate = MDVUtility.ToStr(drPractice[dsPractice.Practice.StateColumn.ColumnName]);
            //string practicezip = MDVUtility.ToStr(drPractice[dsPractice.Practice.ZIPCodeColumn.ColumnName]);
            //string practicephone = MDVUtility.ToStr(drPractice[dsPractice.Practice.PhoneNoColumn.ColumnName]);
            //string practicefax = MDVUtility.ToStr(drPractice[dsPractice.Practice.FaxColumn.ColumnName]);

            string message = "Dear " + patientName + "," + "\n";
            message += "New remarks are available regarding your lab results. Please visit Lab Results section in your Patient Portal to view. Please call the practice for further assistance. \n \n";
            message += "Thank you, \n";
            message += facilityname + "\n";
            message += facilityaddress + "\n";
            message += facilityCity + ", " + facilityState + " " + facilityZip + "\n";
            message += "Phone: " + facilitPhone + "\n";
            message += "Fax: " + facilitFax + "\n";

            return message;

        }

        #endregion

        #region Lab Results to Patient Documents

        private string MoveLabResultToPatientDocs(string pracId, string patientName, string patId, string facId)
        {
            try
            {
                string[] Patient_Name = patientName.Split(',');
                string pat_name = Patient_Name[1] + " " + Patient_Name[0];
                string message = GetFormattedMessage(pracId, pat_name, facId);
                Guid id = Guid.NewGuid();
                string unique_no = id.ToString();
                //DSMessage dsMessage = new DSMessage();
                //DSMessage.PatMessagesRow dr = dsMessage.PatMessages.NewPatMessagesRow();
                //dr.PatientId = MDVUtility.ToInt64(patId);
                //dr.MsgDetail = message;
                //dr.UserId = MDVUtility.ToStr(MDVSession.Current.AppUserId);
                //dr.IsActive = true;
                //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                //dr.CreatedOn = DateTime.Now;
                //dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                //dr.ModifiedOn = DateTime.Now;
                //dr.Subject = "New Remarks regarding Lab Results";
                //dr.IsRead = false;

                DSMessage dsMessage = new DSMessage();
                DSMessage.UserMessagesRow dr = dsMessage.UserMessages.NewUserMessagesRow();

                dr.AssignedFromId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                dr.AttatchedPatientId = MDVUtility.ToInt64(patId);
                dr.Subject = "New Remarks regarding Lab Results";
                dr.MessageDetail = message;
                dr.UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.IsRead = false;
                dr.Entityid = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                dr.MessagerType = "Patient";
                dr.UniqueNumber = unique_no;

                #region Database Insertion
                dsMessage.UserMessages.AddUserMessagesRow(dr);
                BLObject<DSMessage> obj = BLLMessageObj.InsertPracticeMessage(dsMessage);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        UserMessageId = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0][dsMessage.UserMessages.UserMessagesIdColumn.ColumnName],
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


        #endregion
        public string UpdateLabResultComments(LabOrderResultModel model)
        {
            try
            {
                dynamic response = null;
                if (MDVUtility.ToInt64(model.LabResultId) > 0)
                {
                    DSLabResult dsLabResult = new DSLabResult();
                    DSLabResult.LabOrderResultRow dr = null;
                    BLObject<DSLabResult> obj = BLLClinicalObj.LoadLabResult(MDVUtility.ToInt64(model.LabResultId), MDVUtility.ToInt64(model.LabOrderId), model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", "", MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.PatientId));
                    dsLabResult = obj.Data;
                    DSLabResult.LabOrderResultRow[] arrLabResultRows = null;
                    string message = string.Empty;
                    if (obj.Data != null)
                    {
                        arrLabResultRows = (DSLabResult.LabOrderResultRow[])dsLabResult.LabOrderResult.Select(dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName + "=" + model.LabResultId);
                        if (arrLabResultRows.Length > 0)
                            dr = arrLabResultRows[0];
                        if (dr != null)
                        {
                            dr.Comments = model.Comments;
                            BLObject<DSLabResult> objUpdate = BLLClinicalObj.InsertUpdateLabResult(dsLabResult);
                            if (objUpdate.Data != null)
                            {
                                if (dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count > 0)
                                {
                                    message = Common.AppPrivileges.Update_Message;
                                    Int64 LabOrderResultId_ = MDVUtility.ToInt64(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName]);
                                    response = new
                                    {
                                        status = true,
                                        message = message,
                                        labOrderResultId = LabOrderResultId_,
                                    };
                                }
                                else
                                {
                                    response = new
                                    {
                                        status = false,
                                        Message = message
                                    };
                                }
                            }
                            else
                            {
                                response = new
                                {
                                    status = false,
                                    Message = objUpdate.Message
                                };
                            }
                        }
                        else
                        {
                            response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.Update_Error_Message
                            };

                        }
                    }
                    else
                    {
                        response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                    }
                }
                else
                {
                    response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Update_Error_Message
                    };
                }
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
    }


}
