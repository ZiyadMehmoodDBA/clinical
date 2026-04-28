/* Author:  Muhammad Arshad
 * Created Date: 15/03/2016
 * OverView: Created to handel Radiology Result
 */
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using MDVision.Business.BCommon;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using System.Threading.Tasks;
using MDVision.IEHR.EMR.Model.OrdersAndResults.RadiologyResult;
using MDVision.IEHR.EMR.Model.OrdersAndResults.RadiologyOrder;
using MDVision.Model.Admin.Provider;

namespace MDVision.IEHR.EMR.Helpers.Results
{
    public class RadiologyResultHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public RadiologyResultHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static RadiologyResultHelper _instance = null;
        public static RadiologyResultHelper Instance()
        {
            if (_instance == null)
                _instance = new RadiologyResultHelper();
            return _instance;
        }

        #region Radiology Result Fill, Save and Update Methods

        // Author: Abid Ali
        // Created Date: 16/04/2016
        //OverView: This function will handle fill of Radiology Result
        public string fillRadiologyResult(RadiologyOrderResultModel model)
        {

            try
            {
                DSRadiologyOrder dsRadiologyOrder = null;
                BLObject<DSRadiologyOrder> obj = BLLClinicalObj.LoadRadiologyOrder(MDVUtility.ToInt64(model.RadiologyOrderId), MDVUtility.ToInt64(model.PatientId), 0, model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", 0);
                dsRadiologyOrder = obj.Data;
                if (dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName].Rows.Count > 0)
                {
                    DSRadiologyOrder dsRadiologyTest = new DSRadiologyOrder();

                    BLObject<DSRadiologyOrder> objTest = BLLClinicalObj.LoadRadiologyOrderTest(MDVUtility.ToInt64(model.RadiologyOrderId), 0, MDVUtility.ToInt64(model.PatientId), "1", "2000");
                    dsRadiologyTest = objTest.Data;
                    int i = 0;

                    //Load Radiology Result data
                    DSRadiologyResult dsRadiologyResult = new DSRadiologyResult();
                    DSRadiologyResult.RadiologyOrderResultRow drRadiologyResult = null;
                    BLObject<DSRadiologyResult> objResult = BLLClinicalObj.LoadRadiologyResult(0, MDVUtility.ToInt64(model.RadiologyOrderId), model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", MDVUtility.ToInt64(model.PatientId), 0, 0, "1", "");
                    dsRadiologyResult = objResult.Data;

                    Int64 RadiologyResultId = -1;
                    string comments = string.Empty;
                    string remarks = string.Empty;
                    string status = "Open";
                    string AssigneeId = string.Empty;
                    string Assignee = string.Empty;
                    string IsSentToPortal = string.Empty;
                    string IsAknowledged = string.Empty;
                    string Type = "Final";

                    if (objResult.Data != null)
                    {
                        if (dsRadiologyResult.RadiologyOrderResult.Rows.Count > 0)
                        {
                            //Load Parent Table data
                            drRadiologyResult = (DSRadiologyResult.RadiologyOrderResultRow)dsRadiologyResult.Tables[dsRadiologyResult.RadiologyOrderResult.TableName].Rows[0];
                            RadiologyResultId = MDVUtility.ToInt64(drRadiologyResult[dsRadiologyResult.RadiologyOrderResult.RadiologyOrderResultIdColumn.ColumnName]);
                            comments = MDVUtility.ToStr(drRadiologyResult[dsRadiologyResult.RadiologyOrderResult.CommentsColumn.ColumnName]);
                            remarks = MDVUtility.ToStr(drRadiologyResult[dsRadiologyResult.RadiologyOrderResult.RemarksColumn.ColumnName]);
                            status = MDVUtility.ToStr(drRadiologyResult[dsRadiologyResult.RadiologyOrderResult.StatusColumn.ColumnName]);
                            AssigneeId = MDVUtility.ToStr(drRadiologyResult[dsRadiologyResult.RadiologyOrderResult.AssigneeIdColumn.ColumnName]);
                            Assignee = MDVUtility.ToStr(drRadiologyResult[dsRadiologyResult.RadiologyOrderResult.AssignedToColumn.ColumnName]);
                            IsSentToPortal = MDVUtility.ToStr(drRadiologyResult[dsRadiologyResult.RadiologyOrderResult.IsSentToPortalColumn.ColumnName]);
                            IsAknowledged = MDVUtility.ToStr(drRadiologyResult[dsRadiologyResult.RadiologyOrderResult.IsAknowledgedColumn.ColumnName]);
                            Type = MDVUtility.ToStr(drRadiologyResult[dsRadiologyResult.RadiologyOrderResult.TypeColumn.ColumnName]);


                        }

                    }

                    //Load child Table data
                    DSRadiologyResult dsRadiologyResultDetail = new DSRadiologyResult();
                    BLObject<DSRadiologyResult> objRadiologyResultDetail = BLLClinicalObj.LoadRadiologyResultDetail(0, RadiologyResultId);
                    dsRadiologyResultDetail = objRadiologyResultDetail.Data;


                    DataRow dr = dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName].Rows[0];
                    var orderInfoKeyValues = new Dictionary<string, string>
                        {                           
                            //Start//For Radiology Result

                            { "Status", status },
                            {"Type",Type},
                            { "RadiologyOrderResultId",  MDVUtility.ToStr(RadiologyResultId)},
                            { "Comments", comments },
                            { "Remarks", remarks },

                            //End//For Radiology Result
                             
                            { "DateAndTime",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.OrderDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsRadiologyOrder.RadiologyOrder.OrderDateColumn.ColumnName]).ToShortDateString() +" "+dr[dsRadiologyOrder.RadiologyOrder.OrderTimeColumn.ColumnName]},
                            { "RadiologyOrderId",  MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.RadiologyOrderIdColumn.ColumnName])},

                            { "Laboratory",  MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.LabNameColumn.ColumnName])},
                            { "LaboratoryId",  MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.LabIdColumn.ColumnName])},

                            { "Facility", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.FacilityColumn.ColumnName])},
                            { "FacilityId", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.FacilityIdColumn.ColumnName])},

                            { "Provider", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.ProviderColumn.ColumnName])},
                            { "ProviderId", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.ProviderIdColumn.ColumnName])},

                            //Start//Fetch from Result table 
                            { "Assignee", Assignee},
                            { "AssigneeId", AssigneeId},
                            //End//Fetch from Result table 

                            { "IsSentToPortal", IsSentToPortal},
                             { "IsAknowledged", IsAknowledged},

                            { "OrderNo", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.OrderNoColumn.ColumnName])},
                            { "PatientId", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.PatientIdColumn.ColumnName])},

                        };

                    foreach (DataRow drRadiologyTest in dsRadiologyTest.Tables[dsRadiologyTest.RadiologyOrderTest.TableName].Rows)
                    {
                        string RadiologyResultTestId = MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.RadiologyOrderTestIdColumn.ColumnName]);
                        string RadiologyResultTestCPTCode = MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.CPTCodeColumn.ColumnName]);
                        string RadiologyResultTestCPTDescription = MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.CPTCodeDescriptionColumn.ColumnName]);

                        RadiologyOrderResultDetailModel detailModel = new RadiologyOrderResultDetailModel();
                        
                        // Faizan Ameen
                        // Dated: 04-11-2016
                        // EMR-1898
                        if (dsRadiologyResult != null)
                        {
                            if (dsRadiologyResult.Tables[dsRadiologyResult.RadiologyOrderResult.TableName].Rows.Count > 0)
                            {
                                if (objRadiologyResultDetail.Data != null)
                                {

                                    foreach (DataRow drRadiologyResultDetail in dsRadiologyResultDetail.Tables[dsRadiologyResultDetail.RadiologyOrderResultDetail.TableName].Rows)
                                    {
                                        ChildResultDetailModel child = new ChildResultDetailModel();
                                        var CptCode = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.CPTCodeColumn]);
                                        var CptDescription = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.CPTCodeDescriptionColumn]);

                                        //save child rows
                                        if (CptCode == RadiologyResultTestCPTCode && (RadiologyResultTestCPTDescription.IndexOf(CptDescription) > -1))
                                        {
                                            child.RadiologyOrderResultDetailId = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.RadiologyOrderResultDetailIdColumn]);

                                            child.LOINC = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.LOINCColumn]);
                                            child.LOINCDescription = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.LOINCDescriptionColumn]);

                                            child.CPTSNOMEDCodeId = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.CPTSNOMEDIDColumn]);
                                            child.CPTSNOMEDDescription = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.CPTSNOMEDDescriptionColumn]);


                                            child.ObservationDate = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.ObservationDateColumn]);
                                            child.Range = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.RangeColumn]);
                                            child.Result = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.ResultColumn]);
                                            child.UoM = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.UoMColumn]);
                                            child.Flag = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.FlagColumn]);
                                            child.Remarks = MDVUtility.ToStr(drRadiologyResultDetail[dsRadiologyResultDetail.RadiologyOrderResultDetail.RemarksColumn]);
                                            detailModel.ChildRows.Add(child);
                                        }

                                    }
                                }
                            }
                        }
                        detailModel.RadiologyOrderResultDetailId = RadiologyResultTestId;
                        detailModel.CPTCode = MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.CPTCodeColumn.ColumnName]);
                        detailModel.CPTCodeDescription = MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.CPTCodeDescriptionColumn.ColumnName]);


                        //detailModel.ObservationDate = MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.ModifiedOnColumn.ColumnName]);
                        DateTime dt = DateTime.Now;

                        detailModel.ObservationDate = null;
                        string Date = MDVUtility.ToStr(MDVUtility.ToDateTime(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.TestDateColumn.ColumnName]).ToShortDateString());
                        string Time = MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.TestTimeColumn.ColumnName]);
                        if (!string.IsNullOrEmpty(Date) && !string.IsNullOrEmpty(Time))
                        {
                            detailModel.ObservationDate = MDVUtility.ToStr(MDVUtility.ToDateTime(Date + " " + Time));
                        }
                        else
                        {

                        }



                        model.RadiologyOrderResultDetailModels.Add(detailModel);
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                    var response = new
                    {
                        status = true,
                        RadiologyResultOrderInfoFill_JSON = js.Serialize(orderInfoKeyValues),
                        RadiologyResultOrderTestFill_JSON = js.Serialize(model),

                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.No_Record_Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }


        // Author: Abid Ali
        // Created Date: 17/03/2016
        //OverView: This function will update Radiology Result

        public string insertUpdateRadiologyResult(RadiologyOrderResultModel model)
        {

            try
            {
                DSRadiologyResult dsRadiologyResult = new DSRadiologyResult();
                DSRadiologyResult.RadiologyOrderResultRow dr = null;
                if (MDVUtility.ToInt32(model.RadiologyResultId) < 0)
                    model.RadiologyResultId = "0";
                BLObject<DSRadiologyResult> obj;
                obj = BLLClinicalObj.LoadRadiologyResult(MDVUtility.ToInt64(model.RadiologyResultId), MDVUtility.ToInt64(model.RadiologyOrderId), model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", MDVUtility.ToInt64(model.PatientId));
                dsRadiologyResult = obj.Data;
                bool isNewRecord = false;

                DSRadiologyResult.RadiologyOrderResultRow[] arrRadiologyResultRows = null;
                string message = string.Empty;
                if (obj.Data != null)
                {
                    arrRadiologyResultRows = (DSRadiologyResult.RadiologyOrderResultRow[])dsRadiologyResult.RadiologyOrderResult.Select(dsRadiologyResult.RadiologyOrderResult.RadiologyOrderIdColumn.ColumnName + "=" + model.RadiologyOrderId);
                    if (arrRadiologyResultRows.Length > 0)
                    {
                        dr = arrRadiologyResultRows[0];
                        message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        dr = dsRadiologyResult.RadiologyOrderResult.NewRadiologyOrderResultRow();
                        dr.RadiologyOrderId = MDVUtility.ToInt64(model.RadiologyOrderId);
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

                    dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                    dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);

                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    //Start//For sent to Portal
                    dr.IsSentToPortal = model.IsSentToPortal;
                    //End//For sent to Portal

                    //Start//For Aknowledgement
                    dr.IsAknowledged = model.IsAknowledged;
                    //End//For Aknowledgement

                    //Changes made for Assignee
                    if (model.AssigneeId != "")
                        dr.AssigneeId = Convert.ToInt64(model.AssigneeId);
                    else
                    {
                        dr[dsRadiologyResult.RadiologyOrderResult.AssigneeIdColumn.ColumnName] = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(model.NoteId))
                        dr.NoteId = Convert.ToInt64(model.NoteId);
                    else
                    {
                        dr[dsRadiologyResult.RadiologyOrderResult.NoteIdColumn.ColumnName] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(model.Type))
                        dr.Type = Convert.ToString(model.Type);
                    else
                    {
                        dr[dsRadiologyResult.RadiologyOrderResult.TypeColumn.ColumnName] = DBNull.Value;
                    }
                    if (isNewRecord)
                    {
                        dsRadiologyResult.RadiologyOrderResult.AddRadiologyOrderResultRow(dr);
                    }
                }

                #region Database Insertion/Updation

                BLObject<DSRadiologyResult> objUpdate = BLLClinicalObj.InsertUpdateRadiologyResult(dsRadiologyResult);

                if (obj.Data != null)
                {

                    Int64 RadiologyOrderResultId = MDVUtility.ToInt64(dsRadiologyResult.Tables[dsRadiologyResult.RadiologyOrderResult.TableName].Rows[0][dsRadiologyResult.RadiologyOrderResult.RadiologyOrderResultIdColumn.ColumnName]);
                    insertUpdateRadiologyResultDetail(RadiologyOrderResultId, model);

                    var response = new
                    {
                        status = true,
                        message = message,
                        RadiologyOrderResultId = RadiologyOrderResultId,
                    };
                    return (JsonConvert.SerializeObject(response));

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

                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Radiology Result Load, Attach/Detach with Notes
        /// <summary>
        /// Module Name: loadRadiologyResult
        /// Author: Humaira Yousaf
        /// Created Date: 17-03-2016
        /// Description: Loads Radiology Results
        /// </summary> 
        /// <param name="model" type="RadiologyResultModel">RadiologyResultModel model containing data</param>
        public string loadRadiologyResult(RadiologyOrderResultModel model)
        {
            try
            {

                DSRadiologyResult dsRadiology = null;
                DSRadiologyResult dsRadilogyResultDetail = null;
                BLObject<DSRadiologyResult> obj;

                DSClinicalLab dsLab = null;

                if (model.RadiologyType == "External")
                {
                    BLObject<DSClinicalLab> objLab = BLLClinicalObj.loadClinicalLab(MDVUtility.ToInt64(model.LabId), "External Facility", "", 0, 1);
                    dsLab = objLab.Data;
                }
                else
                {
                    BLObject<DSClinicalLab> objLab = BLLClinicalObj.loadClinicalLab(MDVUtility.ToInt64(model.LabId), "Point Of Care", "", 0, 1);
                    dsLab = objLab.Data;
                }

                if (dsLab != null && dsLab.Tables[dsLab.Lab.TableName].Rows.Count > 0)
                {
                    model.LabId = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.LabIdColumn.ColumnName]);
                }

                obj = BLLClinicalObj.LoadRadiologyResult(MDVUtility.ToInt64(model.RadiologyResultId), MDVUtility.ToInt64(model.RadiologyOrderId), model.PageNumber, model.RowsPerPage, model.Test, model.OrderNo, MDVUtility.ToInt64(model.ProviderId), model.OrderFromDate, model.OrderToDate, model.Status, MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.LabId), string.Empty, string.Empty, model.AllResults);//,);
                dsRadiology = obj.Data;

                if (dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName].Rows.Count > 0)
                {
                    //to get top row
                    DataRow dr = dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName].Rows[0];

                    List<Dictionary<string, string>> lstRadiologyResults = new List<Dictionary<string, string>>();

                    var RadiologyResultkeyValues = new Dictionary<string, string>
                        {
                            { "RadiologyOrderResultId",  MDVUtility.ToStr(dr[dsRadiology.RadiologyOrderResult.RadiologyOrderResultIdColumn.ColumnName])},
                            { "SoapText", MDVUtility.ToStr(dr[dsRadiology.RadiologyOrderResult.SoapTextColumn.ColumnName])}
                        };
                    lstRadiologyResults.Add(RadiologyResultkeyValues);

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();


                    //Start// Abid Ali For getting result detail info for a specific result

                    long radiologyOrderResultId = MDVUtility.ToInt64(dr[dsRadiology.RadiologyOrderResult.RadiologyOrderResultIdColumn.ColumnName]);
                    BLObject<DSRadiologyResult> objResultDetail = BLLClinicalObj.LoadRadiologyResultDetail(0, radiologyOrderResultId);
                    dsRadilogyResultDetail = objResultDetail.Data;

                    SharedVariable sharedVariable = SharedVariable.GetSharedVariable();
                    //sharedVariable = null;
                    Parallel.ForEach(dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName].Rows.OfType<DSRadiologyResult.RadiologyOrderResultRow>(), (resultRow) =>
                    // foreach (DSRadiologyResult.RadiologyOrderResultRow resultRow in dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName].Rows)
                    {

                        long radiologyResultOrderId = MDVUtility.ToInt64(resultRow[dsRadiology.RadiologyOrderResult.RadiologyOrderIdColumn.ColumnName]);

                        string resultAttachement = LabResultHelper.Instance().loadOrderResultAttachment(MDVUtility.ToInt64(model.PatientId), radiologyResultOrderId, "Radiology Result", sharedVariable);
                        var attachements = JsonConvert.DeserializeObject<ViewAttachement>(MDVUtility.ToStr(resultAttachement));

                        lock (dsRadiology)
                        {
                            if (attachements.OrderResultAttachmentCount > 0)
                            {
                                resultRow.IsAttachmentExists = true;
                            }
                            else
                            {
                                resultRow.IsAttachmentExists = false;
                            }
                        }
                    });
                    // }

                    //End// Abid Ali// For getting result detail info for a specific result


                    var response = new
                    {
                        status = true,

                        //Start// Abid Ali For getting result detail info for a specific result
                        RadiologyResultDetailFill_JSON = MDVUtility.JSON_DataTable(dsRadilogyResultDetail.Tables[dsRadilogyResultDetail.RadiologyOrderResultDetail.TableName]),
                        //End// Abid Ali// For getting result detail info for a specific result

                        RadiologyResultFill_JSON = js.Serialize(lstRadiologyResults),
                        RadiologyResultCount = dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName].Rows.Count,
                        RadiologyLoad_JSON = MDVUtility.JSON_DataTable(dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName]),
                        iTotalDisplayRecords = (dsRadiology.RadiologyOrderResult.Rows.Count > 0) ? dsRadiology.RadiologyOrderResult.Rows[0][dsRadiology.RadiologyOrderResult.RecordCountColumn.ColumnName] : 0,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        RadiologyResultFill_JSON = "[]",
                        //DiseaseFill_JSON = string.Empty,
                        // MedicalHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsRadiology.Tables[dsRadiology.MedicalHx_Disease.TableName]),
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Method Name: attachRadiologyResultWithNotes
        /// Author: Abid Ali
        /// Description: attaching Radiology Result with notes
        /// </summary>
        /// <param name="RadiologyResultId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string attachRadiologyResultWithNotes(string RadiologyResultId, long notesId)
        {
            try
            {
                DSRadiologyResult dsRadiologyResult = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(RadiologyResultId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<DSRadiologyResult> obj = BLLClinicalObj.attachRadiologyResultWithNotes(RadiologyResultId, notesId);
                    if (obj.Data != null)
                    {
                        dsRadiologyResult = obj.Data;
                        var response = new
                        {
                            status = true,
                            RadiologyResultTotalCount = dsRadiologyResult.Tables[dsRadiologyResult.RadiologyOrderResult.TableName].Rows.Count,
                            RadiologyResultCount = dsRadiologyResult.Tables[dsRadiologyResult.RadiologyOrderResult.TableName].Rows.Count,
                            RadiologyResultLoad_JSON = MDVUtility.JSON_DataTable(dsRadiologyResult.Tables[dsRadiologyResult.RadiologyOrderResult.TableName]),
                            Message = Common.AppPrivileges.Update_Message
                        };

                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        ///   Method Name: detachRadiologyResultFromNotes
        ///   Author: Ahmad Raza
        ///   Description: Detaching Radiology Result from notes
        /// </summary>
        /// <param name="RadiologyResultId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string detachRadiologyResultFromNotes(string RadiologyResultId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(RadiologyResultId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.detachRadiologyOrderResultFromNotes(RadiologyResultId, notesId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }
        #endregion

        #region"Radiology Result Details"
        internal string RadiologyResultChildRowsSoapText(ChildResultDetailModel item)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr><td>" + item.ObservationDate + "</td>");
            sb.Append("<td>" + item.LOINCDescription + "</td>");
            // sb.Append("<td>" + item.Result + "</td>");
            sb.Append("<td>" + item.UoM + "</td>");
            sb.Append("<td>" + item.Flag + "</td>");
            sb.Append("<td>" + item.Remarks + "</td></tr>");
            return sb.ToString();
        }

        public List<RadiologyOrderResultDetailModel> matchCPTCodesForRadResult(List<ProviderCPTs> providerCpts, List<RadiologyOrderResultDetailModel> lstRadResult)
        {
            List<RadiologyOrderResultDetailModel> radResultWithProviderCPtsList = new List<RadiologyOrderResultDetailModel>();

            foreach (RadiologyOrderResultDetailModel CurrentModel in lstRadResult)
            {
                foreach (ProviderCPTs CurrentProviderCPTsModel in providerCpts)
                {
                    if (CurrentProviderCPTsModel.CPTCode == CurrentModel.CPTCode && CurrentProviderCPTsModel.CPTCodeDescription == CurrentModel.CPTCodeDescription)
                    {
                        CurrentModel.ShowCPTCode = "0";
                        break;
                    }
                }
                radResultWithProviderCPtsList.Add(CurrentModel);
            }

            return radResultWithProviderCPtsList;
        }

        public string insertUpdateRadiologyResultDetail(Int64 RadiologyResultId, RadiologyOrderResultModel model)
        {


            try
            {
                string remarks = model.Remarks;
                string comments = model.Comments;
                string status = model.Status;
                //If DeletedResultDetailIds has ids then delete
                if (model.DeletedResultDetailIds.Count > 0)
                {
                    foreach (Int64 item in model.DeletedResultDetailIds)
                    {
                        deleteRadiologyResultTest(item.ToString());
                    }

                }
                DSRadiologyResult dsRadiologyResult = new DSRadiologyResult();
                DSRadiologyResult dsRadiologyResultChild = new DSRadiologyResult();
                BLObject<DSRadiologyResult> objRadiologyResultDetail = BLLClinicalObj.LoadRadiologyResultDetail(0, RadiologyResultId);
                dsRadiologyResult = objRadiologyResultDetail.Data;
                dsRadiologyResultChild = objRadiologyResultDetail.Data;

                List<ProviderCPTs> ProviderCPTsList = new DataAccess.DAL.Clinical.DALLabOrder().GetProviderCPTs(MDVUtility.ToInt64(model.ProviderId));
                List<RadiologyOrderResultDetailModel> currLabOrder = matchCPTCodesForRadResult(ProviderCPTsList, model.RadiologyOrderResultDetailModels);
                model.RadiologyOrderResultDetailModels = currLabOrder;

                StringBuilder sb = new StringBuilder();
                foreach (RadiologyOrderResultDetailModel CurrentModel in model.RadiologyOrderResultDetailModels)
                {
                    DSRadiologyResult.RadiologyOrderResultDetailRow RowRadiologyResultDetail = null;
                    if(CurrentModel.ShowCPTCode == "0")
                    {
                        sb.Append("<div class='table-responsive'><table class='table table-bordered table-condensed'><thead><tr><th align='left' colspan='5'>" +
                        CurrentModel.CPTCodeDescription + "</th></tr>");
                        sb.Append("<tr style='background-color:#0188CC'><th>Date & Time</th><th>Observation</th><th>UoM</th><th>Result</th><th>Remarks</th></tr></thead><tbody>");
                    }
                    else
                    {
                        sb.Append("<div class='table-responsive'><table class='table table-bordered table-condensed'><thead><tr><th align='left' colspan='5'>" +
                        CurrentModel.CPTCode + " - " +
                        CurrentModel.CPTCodeDescription + "</th></tr>");
                        sb.Append("<tr style='background-color:#0188CC'><th>Date & Time</th><th>Observation</th><th>UoM</th><th>Result</th><th>Remarks</th></tr></thead><tbody>");
                    }
                    
                    //Insert Update child Rows
                    foreach (var childRow in CurrentModel.ChildRows)
                    {

                        DSRadiologyResult.RadiologyOrderResultDetailRow[] arrChildRadiologyResultDetailRows = (DSRadiologyResult.RadiologyOrderResultDetailRow[])dsRadiologyResultChild.RadiologyOrderResultDetail.Select(dsRadiologyResultChild.RadiologyOrderResultDetail.RadiologyOrderResultDetailIdColumn.ColumnName + " = " + childRow.RadiologyOrderResultDetailId);
                        //For child Rows
                        if (arrChildRadiologyResultDetailRows.Length > 0)
                        {
                            RowRadiologyResultDetail = arrChildRadiologyResultDetailRows[0];
                        }
                        else
                        {
                            RowRadiologyResultDetail = dsRadiologyResultChild.RadiologyOrderResultDetail.NewRadiologyOrderResultDetailRow();
                            RowRadiologyResultDetail.RadiologyOrderResultId = RadiologyResultId;
                            RowRadiologyResultDetail.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowRadiologyResultDetail.CreatedOn = DateTime.Now;
                        }

                        if (RowRadiologyResultDetail != null)
                        {
                            #region fill data
                            RowRadiologyResultDetail.RadiologyOrderResultId = RadiologyResultId;

                            //if (!string.IsNullOrEmpty(CurrentModel.CPTCode) )
                            //{
                            //    RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.SoapTextColumn] = RadiologyResultSoapText(CurrentModel);
                            //}


                            if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.CPTCodeColumn] = MDVUtility.ToStr(CurrentModel.CPTCode);
                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.CPTCodeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.CPTCodeDescription))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.CPTCodeDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTCodeDescription);
                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.CPTCodeDescriptionColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.CPTSNOMEDCodeId))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.CPTSNOMEDIDColumn] = MDVUtility.ToStr(childRow.CPTSNOMEDCodeId);
                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.CPTSNOMEDIDColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.CPTSNOMEDDescription))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.CPTSNOMEDDescriptionColumn] = MDVUtility.ToStr(childRow.CPTSNOMEDDescription);
                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.CPTSNOMEDDescriptionColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(CurrentModel.Status))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.StatusColumn] = MDVUtility.ToStr(CurrentModel.Status);

                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.StatusColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(CurrentModel.Comments))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.CommentsColumn] = MDVUtility.ToStr(CurrentModel.Comments);

                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.CommentsColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.ObservationDate))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.ObservationDateColumn] = MDVUtility.ToDateTime(childRow.ObservationDate);
                                //  RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.ObservationDateColumn] = DateTime.ParseExact(childRow.ObservationDate, "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.ObservationDateColumn] = DateTime.Now;
                            }

                            if (!string.IsNullOrEmpty(childRow.LOINC))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.LOINCColumn] = MDVUtility.ToStr(childRow.LOINC);

                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.LOINCColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.LOINCDescription))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.LOINCDescriptionColumn] = MDVUtility.ToStr(childRow.LOINCDescription);

                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.LOINCDescriptionColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.Result))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.ResultColumn] = MDVUtility.ToStr(childRow.Result);

                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.ResultColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.UoM))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.UoMColumn] = MDVUtility.ToStr(childRow.UoM);

                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.UoMColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(childRow.Flag))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.FlagColumn] = MDVUtility.ToStr(childRow.Flag);

                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.FlagColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.Remarks))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.RemarksColumn] = MDVUtility.ToStr(childRow.Remarks);

                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.RemarksColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(childRow.Range))
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.RangeColumn] = MDVUtility.ToStr(childRow.Range);
                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.RangeColumn] = DBNull.Value;
                            }

                            if (!string.IsNullOrEmpty(model.ProviderId))
                            {
                                childRow.PatientId = model.ProviderId;
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.ProviderIdColumn] = MDVUtility.ToStr(model.ProviderId);
                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.ProviderIdColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(model.PatientId))
                            {
                                childRow.PatientId = model.PatientId;
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.PatientIdColumn] = MDVUtility.ToStr(childRow.PatientId);
                            }
                            else
                            {
                                RowRadiologyResultDetail[dsRadiologyResultChild.RadiologyOrderResultDetail.PatientIdColumn] = DBNull.Value;
                            }

                            RowRadiologyResultDetail.IsActive = true;
                            RowRadiologyResultDetail.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowRadiologyResultDetail.ModifiedOn = DateTime.Now;
                            #endregion


                            sb.Append(RadiologyResultChildRowsSoapText(childRow));
                            if (arrChildRadiologyResultDetailRows.Length < 1)
                            {
                                dsRadiologyResultChild.RadiologyOrderResultDetail.AddRadiologyOrderResultDetailRow(RowRadiologyResultDetail);
                            }
                        }
                    }
                    sb.Append("</tbody></table></div>");

                }
                sb.Append(string.IsNullOrEmpty(status) ? "" : "</br><b>Status:</b> " + status + "");
                //sb.Append(string.IsNullOrEmpty(remarks) ? "" : "</br><b>Remarks:</b> " + remarks + "</br>");
                sb.Append(string.IsNullOrEmpty(comments) ? "" : "</br><b>Comments:</b> " + comments + "</br>");

                foreach (DataRow item in dsRadiologyResultChild.RadiologyOrderResultDetail.Rows)
                {
                    item[dsRadiologyResultChild.RadiologyOrderResultDetail.SoapTextColumn] = sb.ToString();
                }
                #region Database Insertion/Updation

                BLObject<DSRadiologyResult> objInsertedRadiologyDetail = BLLClinicalObj.InsertUpdateRadiologyResultDetail(dsRadiologyResultChild);

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                };
                return (JsonConvert.SerializeObject(response));

                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string deleteRadiologyResultTest(string RadiologyResultDetailId)
        {

            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.DeleteRadiologyResultDetail(RadiologyResultDetailId);
                result = obj.Data;

                if (result == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,

                    };
                    return (JsonConvert.SerializeObject(response));
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
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        /// <summary>
        /// Method Name: loadOrderingProvider
        /// Author Name: Abid Ali
        /// Created Date: 13-04-2016
        /// Description: This function will handle Load of Provider for Radiology Result
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string loadOrderingProvider(RadiologyOrderResultModel model)
        {
            return "";
            //HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> list = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();

            //DSRadiologyResult dsRadiologyResult = null;
            //BLObject<DSProfileLookup> objProvider = BusinessWrapper.BLLAdminProfileObj.LookupProvider("true");
            //DSProfileLookup dsProvider = objProvider.Data;
            //BLObject<DSRadiologyResult> obj;
            //obj = BLLClinicalObj.LoadRadiologyResult(0, MDVUtility.ToInt64(model.PatientId), "", "", "", "", 0, "", "", "", 0);
            //dsRadiologyResult = obj.Data;
            //list.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));
            //if (dsRadiologyResult != null)
            //{

            //    if (dsProvider.Tables[dsProvider.Provider.TableName] != null)
            //    {

            //        DataView view = new DataView(dsRadiologyResult.Tables[dsRadiologyResult.RadiologyResult.TableName]);
            //        DataTable distinctValues = view.ToTable(true, dsRadiologyResult.RadiologyResult.ProviderIdColumn.ColumnName);
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
        /// Method Name: deleteRadiologyResult
        /// Author : Ahmad Raza
        /// Description: This function will delete the selected Radiology Result
        /// </summary>
        /// <param name="RadiologyResultId"></param>
        /// <returns></returns>
        public string deleteRadiologyResult(string RadiologyResultId)
        {

            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.DeleteRadiologyResult(RadiologyResultId);
                result = obj.Data;

                if (result == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,

                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = result
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Method Name: previewRadiologyResult
        /// Author : Humaira Yousaf
        /// Created Date: 02-05-2016
        /// Description: Creates PDF to view Radiology Result 
        /// </summary>
        /// <param name="model" type="RadiologyResultModel">model</param>  
        public string previewRadiologyResult(RadiologyOrderResultModel model)
        {
            try
            {
                BLObject<byte[]> obj = BLLClinicalObj.previewRadiologyResult(MDVUtility.ToInt64(model.RadiologyResultId), MDVUtility.ToInt64(model.RadiologyOrderId), MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        RadiologyResultHTML = Convert.ToBase64String(obj.Data),
                    };
                    return (JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
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
            return JsonConvert.SerializeObject(list);

        }

        /// <summary>
        /// Method Name: RadiologyResultSoapText
        /// Author Name: Ahmad Raza
        /// Created Date: 25-04-2016
        /// Description: This function will create Soap Text for Radiology Order Result
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal string RadiologyResultSoapText(RadiologyOrderResultDetailModel model, string remarks, string comments, string status)
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
                        sb.Append("<tr><td>" + item.ObservationDate + "</td>");
                        sb.Append("<td>" + item.LOINCDescription + "</td>");
                        sb.Append("<td>" + item.Result + "</td>");
                        sb.Append("<td>" + item.UoM + "</td>");
                        sb.Append("<td>" + item.Flag + "</td>");
                        sb.Append("<td>" + item.Range + "</td></tr>");


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


        public string fillRadiologyResults(RadiologyOrderResultModel model)
        {
            try
            {

                DSRadiologyResult dsRadiologyResult = new DSRadiologyResult();

                BLObject<DSRadiologyResult> objResult = BLLClinicalObj.LoadRadiologyResult(MDVUtility.ToInt64(model.RadiologyResultId), MDVUtility.ToInt64(model.RadiologyOrderId), "1", "1000", model.Test, model.OrderNo, MDVUtility.ToInt64(model.ProviderId), model.OrderFromDate, model.OrderToDate, model.Status, MDVUtility.ToInt64(model.PatientId));
                dsRadiologyResult = objResult.Data;

                if (dsRadiologyResult.Tables[dsRadiologyResult.RadiologyOrderResult.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsRadiologyResult.Tables[dsRadiologyResult.RadiologyOrderResult.TableName].Rows[0];
                    var keyValues = new Dictionary<string, string>
                    {
                        { "RadiologyOrderResultId",  MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResult.RadiologyOrderResultIdColumn.ColumnName])},
                        { "Test",  MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResult.CPTCodeColumn.ColumnName]) +" "+MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResult.CPTCodeDescriptionColumn.ColumnName])},
                        { "Laboratory", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResult.LaboratoryColumn.ColumnName])},
                        { "Status", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResult.StatusColumn.ColumnName])},
                        { "OrderNumber", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResult.OrderNoColumn.ColumnName])},
                        { "AssignedTo", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResult.AssignedToColumn.ColumnName])},
                        { "Provider", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResult.ProviderNameColumn.ColumnName])},
                        { "SoapText", MDVUtility.ToStr(dr[dsRadiologyResult.RadiologyOrderResult.SoapTextColumn.ColumnName])}

                    };

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        RadiologyOrderLoad_JSON = MDVUtility.JSON_DataTable(dsRadiologyResult.Tables[dsRadiologyResult.RadiologyOrderResult.TableName]),
                        RadiologyResultCount = dsRadiologyResult.Tables[dsRadiologyResult.RadiologyOrderResult.TableName].Rows.Count,
                        iTotalDisplayRecords = (dsRadiologyResult.RadiologyOrderResult.Rows.Count > 0) ? dsRadiologyResult.RadiologyOrderResult.Rows[0][dsRadiologyResult.RadiologyOrderResult.RecordCountColumn.ColumnName] : 0,

                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        RadiologyResultFill_JSON = "[]",
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }

            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string loadResultsForSoap(RadiologyOrderResultModel model)
        {

            try
            {
                DSRadiologyResult dsRadiology = null;
                BLObject<DSRadiologyResult> obj;

                obj = BLLClinicalObj.LoadRadiologyResult(MDVUtility.ToInt64(model.RadiologyResultId), MDVUtility.ToInt64(model.RadiologyOrderId), model.PageNumber, model.RowsPerPage, model.Test, model.OrderNo, 0, model.OrderFromDate, model.OrderToDate, model.Status, MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.LabId), string.Empty, string.Empty, model.AllResults);//,);
                dsRadiology = obj.Data;

                if (dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName].Rows[0];//dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName].Rows[dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName].];

                    long resultId = MDVUtility.ToInt64(dr[dsRadiology.RadiologyOrderResult.RadiologyOrderResultIdColumn.ColumnName]);
                    BLObject<DSRadiologyResult> objResultDetail = BLLClinicalObj.loadRadiologyResultsForSoap(MDVUtility.ToStr(resultId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.ProviderId));
                    dsRadiology = objResultDetail.Data;

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        RadiologyResultFill_JSON = MDVUtility.JSON_DataTable(dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName]),
                        RadiologyOrderResultChild_JSON = MDVUtility.JSON_DataTable(dsRadiology.Tables[dsRadiology.RadiologyOrderResultDetail.TableName]),
                        RadiologyOrderResultParent_JSON = MDVUtility.JSON_DataTable(dsRadiology.Tables[dsRadiology.RadiologyOrderResultSoapText.TableName]),
                        ResultSoapCount = dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName].Rows.Count,
                        ResultSoap_JSON = MDVUtility.JSON_DataTable(dsRadiology.Tables[dsRadiology.RadiologyOrderResult.TableName]),
                        iTotalDisplayRecords = (dsRadiology.RadiologyOrderResult.Rows.Count > 0) ? dsRadiology.RadiologyOrderResult.Rows[0][dsRadiology.RadiologyOrderResult.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        RadiologyResultFill_JSON = "[]",
                        RadiologyOrderResultChild_JSON = "[]",
                        RadiologyOrderResultParent_JSON = "[]"
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

        internal string getResultsForSoap(string resultIDs, long patientId, long ProviderId)
        {
            try
            {

                DSRadiologyResult dsResultSoap = null;
                BLObject<DSRadiologyResult> obj = BLLClinicalObj.loadRadiologyResultsForSoap(resultIDs, patientId, ProviderId);


                dsResultSoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsResultSoap.Tables[dsResultSoap.RadiologyOrderResult.TableName].Rows.Count > 0)
                    {
                        //List<Dictionary<string, string>> lstRadiologyResults = new List<Dictionary<string, string>>();

                        //foreach (DataRow dr in dsResultSoap.Tables[dsResultSoap.RadiologyOrderResult.TableName].Rows)
                        //{

                        //    var LabResultkeyValues = new Dictionary<string, string>
                        //    {
                        //        { "RadiologyOrderResultId",  MDVUtility.ToStr(dr[dsResultSoap.RadiologyOrderResult.RadiologyOrderResultIdColumn.ColumnName])},
                        //        { "SoapText", MDVUtility.ToStr(dr[dsResultSoap.RadiologyOrderResult.SoapTextColumn.ColumnName])}
                        //    };
                        //    lstRadiologyResults.Add(LabResultkeyValues);
                        //}

                       System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            ResultSoapCount = dsResultSoap.Tables[dsResultSoap.RadiologyOrderResult.TableName].Rows.Count,
                            RadiologyResultFill_JSON = MDVUtility.JSON_DataTable(dsResultSoap.Tables[dsResultSoap.RadiologyOrderResult.TableName]), //js.Serialize(lstRadiologyResults), //Utility.JSON_DataTable(dsResultSoap.Tables[dsResultSoap.LabOrderResult.TableName]),
                            RadiologyOrderResultChild_JSON = MDVUtility.JSON_DataTable(dsResultSoap.Tables[dsResultSoap.RadiologyOrderResultDetail.TableName]),
                            RadiologyOrderResultParent_JSON = MDVUtility.JSON_DataTable(dsResultSoap.Tables[dsResultSoap.RadiologyOrderResultSoapText.TableName])

                            // MedicationReviewSoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.MedicationReview.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ResultSoapCount = 0,
                            RadiologyResultFill_JSON = "[]", //Utility.JSON_DataTable(dsResultSoap.Tables[dsResultSoap.LabOrderResult.TableName]),
                            RadiologyOrderResultChild_JSON = "[]",
                            RadiologyOrderResultParent_JSON = "[]",
                            Message = Common.AppPrivileges.No_Record_Message
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        /// <summary>
        /// Method Name: viewPdfRadiologyResult
        /// Author Name: Humaira Yousaf
        /// Created Date: 09-05-2016
        /// Description: Views PDF
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string viewPdfRadiologyResult(RadiologyOrderResultModel model)
        {
            try
            {
                BLObject<byte[]> obj = BLLClinicalObj.viewPdfRadiologyResult(MDVUtility.ToInt64(model.RadiologyResultId), MDVUtility.ToInt64(model.RadiologyOrderId), MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        RadiologyOrderHTML = Convert.ToBase64String(obj.Data),
                    };
                    return (JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string deleteRadiologyTest(string LabTestId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.DeleteRadiologyOrderResultTest(LabTestId);
                result = obj.Data;

                if (result == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,

                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        //Modified by Abid Ali
                        Message = result//obj.Message
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
