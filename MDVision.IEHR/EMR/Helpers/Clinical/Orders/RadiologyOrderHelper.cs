/* Author:  Muhammad Arshad
 * Created Date: 15/03/2016
 * OverView: Created to handel Radiology Order
 */
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.OrdersAndResults.RadiologyOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using System.Data;
using MDVision.Model.Lookups;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.IEHR.Controls.Patient.Document;
using MDVision.Model.Patient;
using Newtonsoft.Json;


namespace MDVision.IEHR.EMR.Helpers.Clinical.Orders
{
    public class RadiologyOrderHelper
    {
        private BLLPatient BLLPatientObj = null;
        private static BLLClinical BLLClinicalObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;

        public RadiologyOrderHelper()
        {
            BLLPatientObj = new BLLPatient();
            BLLClinicalObj = new BLLClinical();
            BLLAdminProfileObj = new BLLAdminProfile();
        }

        private static RadiologyOrderHelper _instance = null;
        public static RadiologyOrderHelper Instance()
        {
            if (_instance == null)
                _instance = new RadiologyOrderHelper();
            return _instance;
        }

        #region Radiology Order Fill, Save and Update Methods

        // Author: Abid Ali
        // Created Date: 17/03/2016
        //OverView: This function will handle fill of Radiology Order
        public string fillRadiologyOrder(RadiologyOrderModel model)
        {
            try
            {
                DSRadiologyOrder dsRadiologyOrder = null, dsRadiologyProblems = null;
                BLObject<DSRadiologyOrder> obj = BLLClinicalObj.LoadRadiologyOrder(MDVUtility.ToInt64(model.RadiologyOrderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", 0, "1", "");
                if (obj.Data != null)
                {
                dsRadiologyOrder = obj.Data;
                if (dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName].Rows[0];
                    var radiologyOrderKeyVlaues = new Dictionary<string, string>
                        {

                            { "RadiologyOderDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.OrderDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsRadiologyOrder.RadiologyOrder.OrderDateColumn.ColumnName]).ToShortDateString()},
                            { "Assignee",  MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.AssigneeIdColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.FacilityColumn.ColumnName])},
                            { "hfFacility", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.FacilityIdColumn.ColumnName])},
                            { "Provider", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.ProviderColumn.ColumnName])},
                            { "hfProvider", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.ProviderIdColumn.ColumnName])},
                            { "txtRadiologyOderDate", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.OrderTimeColumn.ColumnName])},
                            { "bResultExists", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.bResultExistsColumn.ColumnName])},
                            { "bResultAcknowledged", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.bResultAcknowledgedColumn.ColumnName])},
                            { "txtFacilityTo", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.FacilityToNameColumn.ColumnName])},
                            { "hfFacilityTo", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.FacilityToColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.CommentsColumn.ColumnName])},
                            { "IsIncludeComments", MDVUtility.ToStr(dr[dsRadiologyOrder.RadiologyOrder.IncludeCommentsColumn.ColumnName])},

                            //{ "GuarantorName", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.SoapTextColumn.ColumnName])}
                        };
                    //Start Farooq Ahmad 21/3/2016 load the radiology order problems
                    if (MDVUtility.ToInt64(model.RadiologyOrderId) > 0)
                    {
                        obj = BLLClinicalObj.LoadRadiologyOrderProblems(MDVUtility.ToInt32(model.RadiologyOrderId), MDVUtility.ToInt64(model.PatientId), 1, 2000);
                        dsRadiologyProblems = obj.Data;
                    }

                    List<Dictionary<string, string>> lstRadiologyTest = new List<Dictionary<string, string>>();

                    if (MDVUtility.ToInt64(model.RadiologyOrderId) > 0)
                    {
                        DSRadiologyOrder dsRadiologyTest = new DSRadiologyOrder();
                        BLObject<DSRadiologyOrder> objTest = BLLClinicalObj.LoadRadiologyOrderTest(MDVUtility.ToInt64(model.RadiologyOrderId), 0, MDVUtility.ToInt64(model.PatientId), "1", "2000");
                        dsRadiologyTest = objTest.Data;

                        foreach (DataRow drRadiologyTest in dsRadiologyTest.Tables[dsRadiologyTest.RadiologyOrderTest.TableName].Rows)
                        {
                            string radiologyTestId = MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.RadiologyOrderTestIdColumn.ColumnName]);
                            var RadiologyTestValues = new Dictionary<string, string>
                            {
                                //{"hfChargeId", radiologyTestId},
                                {"RadiologyOrderTestId", radiologyTestId},
                                {"CPTCode", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.CPTCodeColumn.ColumnName])},
                                {"CPTDescription", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.CPTCodeDescriptionColumn.ColumnName])},
                                {"CPTSNOMEDCodeId", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.CPTSNOMEDIDColumn.ColumnName])},
                                {"CPTSNOMEDDescription", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.CPTSNOMEDDescriptionColumn.ColumnName])},

                                {"RadiologyDate", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.TestDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.TestDateColumn.ColumnName]).ToShortDateString():""},
                                {"RadiologyTime", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.TestTimeColumn.ColumnName])},
                              //  { "RadiologyProcedure", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.CPTCodeDescriptionColumn.ColumnName])},
                                { "CollectedAt", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.CollectedAtIdColumn.ColumnName])},
                                { "Urgency", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.UrgencyIdColumn.ColumnName])},
                                { "Specimen", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.SpecimenIdColumn.ColumnName])},
                                { "PatientInstructions", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.PatientInstructionColumn.ColumnName])},
                                { "VolumeText", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.VolumeLengthColumn.ColumnName])},
                                { "VolumeDDL", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.VolumeIdColumn.ColumnName])},
                                { "FillerInstructions", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.FillerInstructionColumn.ColumnName])},
                                { "Reason", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.ReasonColumn.ColumnName])},
                                { "BodySite", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.RadiologyOrderTest.BodySiteColumn.ColumnName])},

                            };

                            lstRadiologyTest.Add(RadiologyTestValues);
                        }

                    }

                    if (dsRadiologyProblems == null)
                        dsRadiologyProblems = new DSRadiologyOrder();
                    //End Farooq Ahmad 21/3/2016 load the radiology order problems
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        radiologyFill_JSON = js.Serialize(radiologyOrderKeyVlaues),
                        //obaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
                        radiologyOrderFill_JSON = MDVUtility.JSON_DataTable(dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName]),
                        //Start Farooq Ahmad 21/3/2016 load the radiology order problems
                        radiologyOrderProblems_JSON = MDVUtility.JSON_DataTable(dsRadiologyProblems.Tables[dsRadiologyProblems.RadiologyOrderProblem.TableName]),
                        //End Farooq Ahmad 21/3/2016 load the radiology order problems
                        radiologyOrderTest_JSON = js.Serialize(lstRadiologyTest),

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
        // Created Date: 17/03/2016
        //OverView: This function will save Radiology Order
        //public string saveRadiologyOrder(RadiologyOrderModel model)
        //{
        //    try
        //    {
        //        DSRadiologyOrder dsRadiologyOrder = new DSRadiologyOrder();
        //        DSRadiologyOrder.RadiologyOrderRow dr = dsRadiologyOrder.RadiologyOrder.NewRadiologyOrderRow();

        //        dr.RadiologyOrderId = MDVUtility.ToInt64(model.RadiologyOrderId);
        //        dr.PatientId = MDVUtility.ToInt64(model.PatientId);
        //        dr.LabId = MDVUtility.ToInt64(model.LabId);
        //        dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
        //        dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);

        //        //if (!string.IsNullOrEmpty(model.AssigneeId))
        //        //    dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
        //        //else
        //        //    dr[dsRadiologyOrder.RadiologyOrder.AssigneeIdColumn] = DBNull.Value;

        //        //if (!string.IsNullOrEmpty(model.OderDate))
        //        //    dr.OrderDate = MDVUtility.ToDateTime(model.OderDate);
        //        //else
        //        //    dr[dsRadiologyOrder.RadiologyOrder.OrderDateColumn] = DBNull.Value;


        //        //if (!string.IsNullOrEmpty(model.RadiologyOderTime))
        //        //    dr.OrderTime = MDVUtility.ToStr(model.RadiologyOderTime);
        //        //else
        //        //    dr[dsRadiologyOrder.RadiologyOrder.OrderTimeColumn] = DBNull.Value;

        //        //if (!string.IsNullOrEmpty(model.RadiologyBillingTypeId))
        //        //    dr.BillingTypeId = MDVUtility.ToInt64(model.RadiologyBillingTypeId);
        //        //else
        //        //    dr[dsRadiologyOrder.RadiologyOrder.BillingTypeIdColumn] = DBNull.Value;


        //        //if (!string.IsNullOrEmpty(model.RadiologyPrimaryInsuraceId))
        //        //    dr.PrimaryInsuraceId = MDVUtility.ToInt64(model.RadiologyPrimaryInsuraceId);
        //        //else
        //        //    dr[dsRadiologyOrder.RadiologyOrder.PrimaryInsuraceIdColumn] = DBNull.Value;

        //        //if (!string.IsNullOrEmpty(model.RadiologySecondaryInsuraceId))
        //        //    dr.SecondaryInsuraceId = MDVUtility.ToInt64(model.RadiologySecondaryInsuraceId);
        //        //else
        //        //    dr[dsRadiologyOrder.RadiologyOrder.SecondaryInsuraceIdColumn] = DBNull.Value;

        //        //if (!string.IsNullOrEmpty(model.RadiologyTertiaryInsuraceId))
        //        //    dr.TertiaryInsuraceId = MDVUtility.ToInt64(model.RadiologyTertiaryInsuraceId);
        //        //else
        //        //    dr[dsRadiologyOrder.RadiologyOrder.TertiaryInsuraceIdColumn] = DBNull.Value;

        //        //if (!string.IsNullOrEmpty(model.RadiologyGuarantorId))
        //        //    dr.GuarantorId = MDVUtility.ToInt64(model.RadiologyGuarantorId);
        //        //else
        //        //    dr[dsRadiologyOrder.RadiologyOrder.GuarantorIdColumn] = DBNull.Value;

        //        //if (!string.IsNullOrEmpty(model.RadiologyRelationShipId))
        //        //    dr.RelationShipId = MDVUtility.ToInt32(model.RadiologyRelationShipId);
        //        //else
        //        //    dr[dsRadiologyOrder.RadiologyOrder.RelationShipIdColumn] = DBNull.Value;


        //        dr.IsActive = true;
        //        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //        dr.CreatedOn = DateTime.Now;
        //        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //        dr.ModifiedOn = DateTime.Now;
        //        //dr.SoapText = model.soap

        //        #region Database Insertion

        //        dsRadiologyOrder.RadiologyOrder.AddRadiologyOrderRow(dr);
        //        BLObject<DSRadiologyOrder> obj = BLLClinicalObj.InsertUpdateRadiologyOrder(dsRadiologyOrder);
        //        dsRadiologyOrder = obj.Data;

        //        if (obj.Data != null)
        //        {

        //            Int64 radiologicalOrderId = MDVUtility.ToInt64(dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName].Rows[0][dsRadiologyOrder.RadiologyOrder.RadiologyOrderIdColumn.ColumnName]);

        //            var response = new
        //            {
        //                status = true,
        //                message = Common.AppPrivileges.Save_Message,
        //                radiologicalOrderId = radiologicalOrderId,
        //            };
        //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = obj.Message
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //        #endregion
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

        // Author: Abid Ali
        // Created Date: 17/03/2016
        //OverView: This function will update Radiology Order

        public string insertUpdateRadiologyOrder(RadiologyOrderModel model, List<object> lstRadiologyObjects)
        {
            try
            {
                DSRadiologyOrder dsRadiologyOrder = new DSRadiologyOrder();
                DSRadiologyOrder.RadiologyOrderRow dr = null;
                BLObject<DSRadiologyOrder> obj = BLLClinicalObj.LoadRadiologyOrder(MDVUtility.ToInt64(model.RadiologyOrderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", 0); ;
                dsRadiologyOrder = obj.Data;
                bool isNewRecord = false;

                DSRadiologyOrder.RadiologyOrderRow[] arrRadiologyOrderRows = null;
                string message = string.Empty;
                if (obj.Data != null)
                {
                    arrRadiologyOrderRows = (DSRadiologyOrder.RadiologyOrderRow[])dsRadiologyOrder.RadiologyOrder.Select(dsRadiologyOrder.RadiologyOrder.RadiologyOrderIdColumn.ColumnName + "=" + model.RadiologyOrderId);
                    if (arrRadiologyOrderRows.Length > 0)
                    {
                        dr = arrRadiologyOrderRows[0];
                        message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        dr = dsRadiologyOrder.RadiologyOrder.NewRadiologyOrderRow();
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
                    dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                    dr.IncludeComments = MDVUtility.ToBool(model.IsIncludeComments);
                    if (model.LabId != "")
                    {
                        dr.LabId = MDVUtility.ToInt64(model.LabId);
                    }
                    dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
                    dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);


                    if (!string.IsNullOrEmpty(model.FacilityTo))
                        dr.FacilityTo = MDVUtility.ToInt64(model.FacilityTo);
                    else
                    {
                        dr[dsRadiologyOrder.RadiologyOrder.FacilityToColumn] = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(model.Comments))
                        dr.Comments = MDVUtility.ToStr(model.Comments);
                    else
                    {
                        dr[dsRadiologyOrder.RadiologyOrder.CommentsColumn] = DBNull.Value;
                    }


                    if (!string.IsNullOrEmpty(model.AssigneeId))
                        dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
                    else
                        dr[dsRadiologyOrder.RadiologyOrder.AssigneeIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.NoteId))
                        dr.NoteId = MDVUtility.ToInt64(model.NoteId);
                    else
                        dr[dsRadiologyOrder.RadiologyOrder.NoteIdColumn] = DBNull.Value;

                    //if (!string.IsNullOrEmpty(model.OrderDate))
                    //    dr.OrderDate = MDVUtility.ToDateTime(model.OrderDate);
                    //else
                    //    dr[dsRadiologyOrder.RadiologyOrder.OrderDateColumn] = DBNull.Value;


                    //if (!string.IsNullOrEmpty(model.OrderTime))
                    //    dr.OrderTime = MDVUtility.ToStr(model.OrderTime);
                    //else
                    //    dr[dsRadiologyOrder.RadiologyOrder.OrderTimeColumn] = DBNull.Value;


                    //-------------------

                    if (!string.IsNullOrEmpty(model.OrderDate))
                        dr.OrderDate = MDVUtility.ToDateTime(model.OrderDate);
                    else
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(dr[dsRadiologyOrder.RadiologyOrder.OrderDateColumn])))
                        {
                            dr[dsRadiologyOrder.RadiologyOrder.OrderDateColumn] = DBNull.Value;
                        }
                    }

                    if (!string.IsNullOrEmpty(model.OrderTime))
                        dr.OrderTime = MDVUtility.ToStr(model.OrderTime);
                    else
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(dr[dsRadiologyOrder.RadiologyOrder.OrderTimeColumn])))
                        {
                            dr[dsRadiologyOrder.RadiologyOrder.OrderTimeColumn] = DBNull.Value;
                        }
                    }

                    //-------------------

                    if (!string.IsNullOrEmpty(model.BillingTypeId))
                        dr.BillingTypeId = MDVUtility.ToInt64(model.BillingTypeId);
                    else
                        dr[dsRadiologyOrder.RadiologyOrder.BillingTypeIdColumn] = DBNull.Value;


                    if (!string.IsNullOrEmpty(model.PrimaryInsuraceId))
                        dr.PrimaryInsuraceId = MDVUtility.ToInt64(model.PrimaryInsuraceId);
                    else
                        dr[dsRadiologyOrder.RadiologyOrder.PrimaryInsuraceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.SecondaryInsuraceId))
                        dr.SecondaryInsuraceId = MDVUtility.ToInt64(model.SecondaryInsuraceId);
                    else
                        dr[dsRadiologyOrder.RadiologyOrder.SecondaryInsuraceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.TertiaryInsuraceId))
                        dr.TertiaryInsuraceId = MDVUtility.ToInt64(model.TertiaryInsuraceId);
                    else
                        dr[dsRadiologyOrder.RadiologyOrder.TertiaryInsuraceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.GuarantorId))
                        dr.GuarantorId = MDVUtility.ToInt64(model.GuarantorId);
                    else
                        dr[dsRadiologyOrder.RadiologyOrder.GuarantorIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.RelationShipId))
                        dr.RelationShipId = MDVUtility.ToInt32(model.RelationShipId);
                    else
                        dr[dsRadiologyOrder.RadiologyOrder.RelationShipIdColumn] = DBNull.Value;

                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    //Start 22-03-2016 Humaira Yousaf for status
                    dr.Status = model.Status;
                    //End 22-03-2016 Humaira Yousaf for status
                    if (!string.IsNullOrEmpty(model.NegationReasonId))
                        dr.NegationReasonId = model.NegationReasonId;
                    else
                    {
                        dr[dsRadiologyOrder.RadiologyOrder.NegationReasonIdColumn] = DBNull.Value;
                    }

                    if (isNewRecord)
                    {
                        dsRadiologyOrder.RadiologyOrder.AddRadiologyOrderRow(dr);
                    }
                }

                #region Database Insertion/Updation

                BLObject<DSRadiologyOrder> objUpdate = BLLClinicalObj.InsertUpdateRadiologyOrder(dsRadiologyOrder, model, lstRadiologyObjects);
                dsRadiologyOrder = objUpdate.Data;

                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
                //if (objUpdate.Data != null)
                //{
                //    string res = LabOrderHelper.insertUpdateFavListSetting(model.FavListNames);
                //}
                //End 10-06-2016 Abid Ali for favorite list setting for all favLists
                if (obj.Data != null)
                {

                    Int64 RadiologyOrderId = MDVUtility.ToInt64(dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName].Rows[0][dsRadiologyOrder.RadiologyOrder.RadiologyOrderIdColumn.ColumnName]);

                    string OrderNo = MDVUtility.ToStr(dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName].Rows[0][dsRadiologyOrder.RadiologyOrder.OrderNoColumn.ColumnName]);
                    string IsIncludeComments = MDVUtility.ToStr(dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName].Rows[0][dsRadiologyOrder.RadiologyOrder.IncludeCommentsColumn.ColumnName]);
                    //if (model.RadiologyOrderProblemList != null && model.RadiologyOrderProblemList.Count > 0)
                    //{
                    //    model.RadiologyOrderProblemList.ForEach(cc => cc.RadiologyOrderId = MDVUtility.ToStr(RadiologyOrderId));
                    //BLLClinicalObj.DeleteRadiologyOrderProblems(MDVUtility.ToStr(RadiologyOrderId));

                    //var id = saveRadiologyOrderProblem(model.RadiologyOrderProblemList);

                    //}

                    //insertUpdateRadiologyOrderTest(RadiologyOrderId, lstRadiologyObjects);

                    var response = new
                    {
                        status = true,
                        message = message,
                        radiologicalOrderId = RadiologyOrderId,
                        orderNo = OrderNo,
                        IsIncludeComments = IsIncludeComments
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
        //public string updateRadiologyOrder(RadiologyOrderModel model)
        //{
        //    try
        //    {
        //        DSRadiologyOrder dsRadiologyOrder = new DSRadiologyOrder();
        //        BLObject<DSRadiologyOrder> obj = BLLClinicalObj.LoadRadiologyOrder(MDVUtility.ToInt64(model.RadiologyOrderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", 0);
        //        dsRadiologyOrder = obj.Data;
        //        foreach (DSRadiologyOrder.RadiologyOrderRow dr in dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName].Rows)
        //        {
        //            dr.RadiologyOrderId = MDVUtility.ToInt64(model.RadiologyOrderId);
        //            dr.PatientId = MDVUtility.ToInt64(model.PatientId);
        //            dr.LabId = MDVUtility.ToInt64(model.LabId);
        //            dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
        //            dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
        //            dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
        //            //dr.OrderDate = MDVUtility.ToDateTime(model.RadiologyOderDate);
        //            //dr.OrderTime = MDVUtility.ToStr(model.RadiologyOderTime);
        //            //dr.BillingTypeId = MDVUtility.ToInt64(model.RadiologyBillingTypeId);
        //            //dr.PrimaryInsuraceId = MDVUtility.ToInt64(model.RadiologyPrimaryInsuraceId);
        //            //dr.SecondaryInsuraceId = MDVUtility.ToInt64(model.RadiologySecondaryInsuraceId);
        //            //dr.TertiaryInsuraceId = MDVUtility.ToInt64(model.RadiologyTertiaryInsuraceId);
        //            //dr.GuarantorId = MDVUtility.ToInt64(model.RadiologyGuarantorId);
        //            //dr.RelationShipId = MDVUtility.ToInt32(model.RadiologyRelationShipId);

        //            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //            dr.ModifiedOn = DateTime.Now;

        //            #region Database Updation
        //            if (dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName].Rows.Count > 0)
        //            {
        //                BLObject<DSRadiologyOrder> objUpdate = BLLClinicalObj.InsertUpdateRadiologyOrder(dsRadiologyOrder);

        //                if (objUpdate.Data != null)
        //                {
        //                    var response = new
        //                    {
        //                        status = true,
        //                        message = Common.AppPrivileges.Update_Message,

        //                    };
        //                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //                }
        //                else
        //                {
        //                    var response = new
        //                    {
        //                        status = false,
        //                        message = objUpdate.Message
        //                    };
        //                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //                }
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    message = ""
        //                };
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //            }
        //            #endregion
        //        }

        //        return "";
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

        // Author: Abid Ali
        // Created Date: 17/03/2016
        //OverView: This function will delete Radiology Order
        //public string deleteRadiologyOrder(string radiologyOrderId)
        //{

        //    DSRadiologyOrder dsRadiologyOrder = new DSRadiologyOrder();
        //    BLObject<string> objRadiologyOderDelete = new BLObject<string>();
        //    objRadiologyOderDelete = BLLClinicalObj.DeleteRadiologyOrder(radiologyOrderId);
        //    string result = objRadiologyOderDelete.Data;

        //    if (result == "")
        //    {
        //        var response = new { status = true, message = "Rdiology deleted successfully." };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //    else
        //    {
        //        var response = new { status = false, message = result };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }

        //}

        // Author: Farooq Ahmad
        // Created Date: 18/03/2016
        //OverView: This function will save Radiology Order Problem
        public string saveRadiologyOrderProblem(List<RadiologyOrderProblemModel> model)
        {
            try
            {
                DSRadiologyOrder dsRadiologyOrder = new DSRadiologyOrder();

                foreach (var m in model)
                {
                    DSRadiologyOrder.RadiologyOrderProblemRow dr = dsRadiologyOrder.RadiologyOrderProblem.NewRadiologyOrderProblemRow();
                    dr.RadiologyOrderId = MDVUtility.ToInt64(m.RadiologyOrderId);
                    dr.ProblemId = MDVUtility.ToInt32(m.ProblemId);
                    dr.Comments = string.Empty;
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dsRadiologyOrder.RadiologyOrderProblem.AddRadiologyOrderProblemRow(dr);
                }
                #region Database Insertion
                BLObject<DSRadiologyOrder> obj = BLLClinicalObj.InsertUpdateRadiologyOrderProblems(dsRadiologyOrder);
                dsRadiologyOrder = obj.Data;

                if (obj.Data != null)
                {

                    Int64 RadiologyOrderId = MDVUtility.ToInt64(dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrderProblem.TableName].Rows[0][dsRadiologyOrder.RadiologyOrderProblem.RadiologyOrderIdColumn.ColumnName]);

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        RadiologyOrderId = RadiologyOrderId,
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

        #region Radiology order Load, Attach/Detach with Notes
        /// <summary>
        /// Module Name: loadRadiologyOrder
        /// Author: Humaira Yousaf
        /// Created Date: 17-03-2016
        /// Description: Loads Radiology Orders
        /// </summary>
        /// <param name="model" type="RadiologyOrderModel">RadiologyOrderModel model containing data</param>
        public string loadRadiologyOrder(RadiologyOrderModel model)
        {
            try
            {

                DSRadiologyOrder dsRadiology = null;
                BLObject<DSRadiologyOrder> obj;

                DSClinicalLab dsLab = null;

                if (model.RadiologyType == "External")
                {
                    BLObject<DSClinicalLab> objLab = BLLClinicalObj.loadClinicalLab(MDVUtility.ToInt64(model.LabId), "External Facility", "", 0, 1);
                    dsLab = objLab.Data;
                }
                else if (model.RadiologyType == "both")
                {
                    model.LabId = "";
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

                obj = BLLClinicalObj.LoadRadiologyOrder(MDVUtility.ToInt64(model.RadiologyOrderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), model.PageNumber, model.RowsPerPage, model.Test, model.OrderNo, MDVUtility.ToInt64(model.ProviderId), model.OrderFromDate, model.OrderToDate, model.Status, MDVUtility.ToInt64(model.LabId));
                dsRadiology = obj.Data;

                if (dsRadiology.Tables[dsRadiology.RadiologyOrder.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsRadiology.Tables[dsRadiology.RadiologyOrder.TableName].Rows[dsRadiology.Tables[dsRadiology.RadiologyOrder.TableName].Rows.Count - 1];
                    var RadiologyOrderkeyValues = new Dictionary<string, string>
                        {
                            { "RadiologyOrderId",  MDVUtility.ToStr(dr[dsRadiology.RadiologyOrder.RadiologyOrderIdColumn.ColumnName])},
                            { "SoapText", MDVUtility.ToStr(dr[dsRadiology.RadiologyOrder.SoapTextColumn.ColumnName])}
                        };

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        RadiologyOrderFill_JSON = js.Serialize(RadiologyOrderkeyValues),
                        radiologyOrderCount = dsRadiology.Tables[dsRadiology.RadiologyOrder.TableName].Rows.Count,
                        RadiologyLoad_JSON = MDVUtility.JSON_DataTable(dsRadiology.Tables[dsRadiology.RadiologyOrder.TableName]),
                        iTotalDisplayRecords = (dsRadiology.RadiologyOrder.Rows.Count > 0) ? dsRadiology.RadiologyOrder.Rows[0][dsRadiology.RadiologyOrder.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        RadiologyOrderFill_JSON = "[]",
                        //DiseaseFill_JSON = "[]",
                        // MedicalHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsRadiology.Tables[dsRadiology.MedicalHx_Disease.TableName]),
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
        /// Method Name: attachRadiologyOrderWithNotes
        /// Author: Ahmad Raza
        /// Description: attaching radiology order with notes
        /// </summary>
        /// <param name="radiologyOrderId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string attachRadiologyOrderWithNotes(string radiologyOrderId, long notesId)
        {
            try
            {
                DSRadiologyOrder dsRadiologyOrder = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(radiologyOrderId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<DSRadiologyOrder> obj = BLLClinicalObj.attachRadiologyOrderWithNotes(radiologyOrderId, notesId);
                    if (obj.Data != null)
                    {
                        dsRadiologyOrder = obj.Data;
                        var response = new
                        {
                            status = true,
                            RadiologyOrderTotalCount = dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName].Rows.Count,
                            RadiologyOrderCount = dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName].Rows.Count,
                            RadiologyOrderLoad_JSON = MDVUtility.JSON_DataTable(dsRadiologyOrder.Tables[dsRadiologyOrder.RadiologyOrder.TableName]),
                            Message = Common.AppPrivileges.Update_Message
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
        ///   Method Name: detachRadiologyOrderFromNotes
        ///   Author: Ahmad Raza
        ///   Description: Detaching radiology order from notes
        /// </summary>
        /// <param name="radiologyOrderId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string detachRadiologyOrderFromNotes(string radiologyOrderId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(radiologyOrderId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachRadiologyOrderFromNotes(radiologyOrderId, notesId);
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

        #region"Radiology Order Test"
        public string insertUpdateRadiologyOrderTest(Int64 RadiologyId, List<object> lstObjects)
        {
            try
            {


                DSRadiologyOrder dsRadiologyOrder = new DSRadiologyOrder();

                BLObject<DSRadiologyOrder> objRadiologyOrderTest = BLLClinicalObj.LoadRadiologyOrderTest(MDVUtility.ToInt64(RadiologyId), 0, 0, "1", "2000");
                dsRadiologyOrder = objRadiologyOrderTest.Data;
                //dsRadiologyOrder = objRadiologyOrder.Data;
                List<RadiologyOrderTestModel> lstRadiologyOrder = lstObjects.OfType<RadiologyOrderTestModel>().ToList();

                int id = -1;

                foreach (RadiologyOrderTestModel CurrentModel in lstRadiologyOrder)
                {
                    Int32 currentRadiologyTestId = MDVUtility.ToInt32(CurrentModel.RadiologyOrderTestId);
                    currentRadiologyTestId = currentRadiologyTestId == 0 ? id-- : currentRadiologyTestId;

                    DSRadiologyOrder.RadiologyOrderTestRow RowRadiologyOrderTest = null;
                    DSRadiologyOrder.RadiologyOrderTestRow[] arrRadiologyOrderTestRows = (DSRadiologyOrder.RadiologyOrderTestRow[])dsRadiologyOrder.RadiologyOrderTest.Select(dsRadiologyOrder.RadiologyOrderTest.RadiologyOrderTestIdColumn.ColumnName + "=" + CurrentModel.RadiologyOrderTestId);

                    if (arrRadiologyOrderTestRows.Length > 0)
                    {
                        RowRadiologyOrderTest = arrRadiologyOrderTestRows[0];
                    }
                    else
                    {
                        RowRadiologyOrderTest = dsRadiologyOrder.RadiologyOrderTest.NewRadiologyOrderTestRow();
                        RowRadiologyOrderTest.RadiologyOrderTestId = currentRadiologyTestId;
                    }

                    //RowRadiologyOrderTest = dsRadiologyOrder.RadiologyOrderTest.NewRadiologyOrderTestRow();
                    if (RowRadiologyOrderTest != null)
                    {
                        RowRadiologyOrderTest.RadiologyOrderId = RadiologyId;

                        if (!string.IsNullOrEmpty(CurrentModel.CPTDescription))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.CPTCodeDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTDescription);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.CPTCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.CPTCodeColumn] = MDVUtility.ToStr(CurrentModel.CPTCode);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.CPTCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDDescription))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.CPTSNOMEDDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTSNOMEDDescription);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.CPTSNOMEDDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDCodeId))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.CPTSNOMEDIDColumn] = MDVUtility.ToStr(CurrentModel.CPTSNOMEDCodeId);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.CPTSNOMEDIDColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.dtpRadiologyDate))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.TestDateColumn] = MDVUtility.ToDateTime(CurrentModel.dtpRadiologyDate);
                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.TestDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.tpRadiologyTime))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.TestTimeColumn] = MDVUtility.ToStr(CurrentModel.tpRadiologyTime);
                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.TestTimeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.RadiologyProcedure))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.CPTCodeDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTDescription);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.CPTCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CollectedAt))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.CollectedAtIdColumn] = MDVUtility.ToInt32(CurrentModel.CollectedAt);

                        }

                        if (!string.IsNullOrEmpty(CurrentModel.Urgency))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.UrgencyIdColumn] = MDVUtility.ToInt32(CurrentModel.Urgency);

                        }

                        if (!string.IsNullOrEmpty(CurrentModel.Specimen))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.SpecimenIdColumn] = MDVUtility.ToInt32(CurrentModel.Specimen);

                        }

                        if (!string.IsNullOrEmpty(CurrentModel.PatientInstructions))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.PatientInstructionColumn] = MDVUtility.ToStr(CurrentModel.PatientInstructions);

                        }

                        if (!string.IsNullOrEmpty(CurrentModel.VolumeText))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.VolumeLengthColumn] = MDVUtility.ToStr(CurrentModel.VolumeText);

                        }

                        if (!string.IsNullOrEmpty(CurrentModel.VolumeDDL))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.VolumeIdColumn] = MDVUtility.ToInt32(CurrentModel.VolumeDDL);

                        }
                        if (!string.IsNullOrEmpty(CurrentModel.FillerInstructions))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.FillerInstructionColumn] = MDVUtility.ToStr(CurrentModel.FillerInstructions);

                        }
                        if (!string.IsNullOrEmpty(CurrentModel.Reason))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.ReasonColumn] = MDVUtility.ToStr(CurrentModel.Reason);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.ReasonColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.BodySite))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.BodySiteColumn] = MDVUtility.ToStr(CurrentModel.BodySite);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.BodySiteColumn] = DBNull.Value;
                        }
                        //Start Farooq Ahmad 23/3/2016 Update Soap Text
                        RowRadiologyOrderTest[dsRadiologyOrder.RadiologyOrderTest.SoapTextColumn] = string.Format("{0} {1} {2} {3} {4} {5}", string.Empty, "<b>" + CurrentModel.RadiologyProcedure + "</b>", string.IsNullOrWhiteSpace(CurrentModel.Specimen) ? string.Empty : string.Concat(",Specimen: ", CurrentModel.Specimen_text), string.IsNullOrWhiteSpace(CurrentModel.VolumeText) ? string.Empty : string.Concat(",Volume: ", CurrentModel.VolumeText), string.IsNullOrWhiteSpace(CurrentModel.Urgency) ? string.Empty : CurrentModel.Urgency_text == "- Select -" ? string.Empty : string.Concat(",Urgency: ", CurrentModel.Urgency_text), "<br>");
                        //End Farooq Ahmad 23/3/2016 Update Soap Text
                        RowRadiologyOrderTest.IsActive = true;
                        RowRadiologyOrderTest.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowRadiologyOrderTest.CreatedOn = DateTime.Now;
                        RowRadiologyOrderTest.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowRadiologyOrderTest.ModifiedOn = DateTime.Now;



                        if (arrRadiologyOrderTestRows.Length < 1)
                        {
                            dsRadiologyOrder.RadiologyOrderTest.AddRadiologyOrderTestRow(RowRadiologyOrderTest);
                        }
                        //dsRadiologyOrder.RadiologyOrderTest.AddRadiologyOrderTestRow(RowRadiologyOrderTest);

                    }
                }

                #region Database Insertion/Updation

                BLObject<DSRadiologyOrder> objInsertedRadiologyTest = BLLClinicalObj.insertUpdateRadiologyOrderTest(dsRadiologyOrder);
                if (objInsertedRadiologyTest.Data != null)
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
                        Message = objInsertedRadiologyTest.Message
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

        public string deleteRadiologyOrderTest(string RadiologyOrderTestId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.deleteRadiologyOrderTest(RadiologyOrderTestId);
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

        /// <summary>
        /// Method Name: deleteRadiologyOrder
        /// Author : Ahmad Raza
        /// Description: This function will delete the selected Radiology Order
        /// </summary>
        /// <param name="radiologyOrderId"></param>
        /// <returns></returns>
        public string deleteRadiologyOrder(string radiologyOrderId)
        {
            try
            {
                string result = "";
                DSRadiologyOrder dsRadiology = null, dsRadiologyProblems = null;
                BLObject<string> obj;
                obj = BLLClinicalObj.DeleteRadiologyOrder(radiologyOrderId);
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
                        Message = result
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        public string loadPatientGuarantor(RadiologyOrderModel model)
        {
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                //  DSPatient dsPatient = null;
                DSPatient dsPatientRelation = null;
                // DSPatientProfile dsRelationShip = null;
                string relationshipName = "";
                Int64 relationshipId = 0;
                //BLObject<DSPatient> objpatient = BLLPatientObj.FillPatientById(MDVUtility.ToInt32(model.PatientId));
                //dsPatient = objpatient.Data;


                BLObject<DSPatient> objPatientRelation = BLLPatientObj.FillPatientAndInsuranceById(MDVUtility.ToInt32(model.PatientId), 0, 0);
                dsPatientRelation = objPatientRelation.Data;

                if (dsPatientRelation.Tables[dsPatientRelation.PatientInsurance.TableName].Rows.Count > 0)
                {
                    DataRow drPatientRelation = dsPatientRelation.Tables[dsPatientRelation.PatientInsurance.TableName].Rows[0];
                    relationshipName = MDVUtility.ToStr(drPatientRelation[dsPatientRelation.PatientInsurance.RelationNameColumn.ColumnName]);
                    relationshipId = MDVUtility.ToInt64(drPatientRelation[dsPatientRelation.PatientInsurance.RelationShipIdColumn.ColumnName]);
                }


                if (dsPatientRelation.Tables[dsPatientRelation.Patients.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsPatientRelation.Tables[dsPatientRelation.Patients.TableName].Rows[0];
                    //string guarantorId = MDVUtility.ToStr(dr[dsPatientRelation.Patients.GuarantorIdColumn.ColumnName]);
                    //if (!string.IsNullOrEmpty(guarantorId))
                    //{
                    //    BLObject<DSPatientProfile> objRelationShip = BLLPatientObj.LoadGuarantor(MDVUtility.ToInt64(guarantorId), "", "", "", "", 1, 15);
                    //    dsRelationShip = objRelationShip.Data;
                    //    if (dsRelationShip.Tables[dsRelationShip.Guarantor.TableName].Rows.Count > 0)
                    //    {
                    //        DataRow drGuarantor = dsRelationShip.Tables[dsRelationShip.Guarantor.TableName].Rows[0];
                    //      //  relationshipId = MDVUtility.ToInt32(drGuarantor[dsRelationShip.Guarantor.RelationIdColumn.ColumnName]);
                    //      //  relationName = MDVUtility.ToStr(drGuarantor[dsRelationShip.Guarantor.RelationNameColumn.ColumnName]);
                    //    }



                    var GuarantorkeyValues = new Dictionary<string, string>
                        {
                            { "GuarantorId",  MDVUtility.ToStr(dr[dsPatientRelation.Patients.GuarantorIdColumn.ColumnName])},
                            { "GuarantorName",  MDVUtility.ToStr(dr[dsPatientRelation.Patients.GuarantorNameColumn.ColumnName])},
                        };

                    var response = new
                    {
                        status = true,
                        relationshipId = relationshipId,
                        relationName = relationshipName,
                        GuarantorFill_JSON = js.Serialize(GuarantorkeyValues),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    // }
                    //else
                    //{
                    //    var response = new
                    //    {
                    //        status = true,
                    //        GuarantorFill_JSON = "[]"
                    //    };
                    //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    //}
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        GuarantorFill_JSON = "[]"
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
        /// Method Name: previewRadiologyOrder
        /// Author : Humaira Yousaf
        /// Created Date: 23-03-2016
        /// Description: Creates PDF to view Radiology Order
        /// </summary>
        /// <param name="model" type="RadiologyOrderModel">model</param>
        public string previewRadiologyOrder(RadiologyOrderModel model)
        {
            try
            {
                BLObject<byte[]> obj = BLLClinicalObj.previewRadiologyOrder(MDVUtility.ToInt64(model.RadiologyOrderId), MDVUtility.ToInt64(model.PatientId), false, 0, model.Mode, MDVUtility.ToBool(model.IsIncludeComments));

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        RadiologyOrderHTML = Convert.ToBase64String(obj.Data),
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
        public string SaveAndAttachProcedureReport(RadiologyOrderModel model)
        {
            try
            {
                string base64string = "";
                BLObject<byte[]> obj = BLLClinicalObj.CreateProcedureReport(MDVUtility.ToInt64(model.NoteId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToBool(model.IsFindingUpdated));
                if (obj.Data != null)
                {
                    base64string = Convert.ToBase64String(obj.Data);
                    string docResponse = new Patient_Document().SaveSignedDocument("", MDVUtility.ToInt64(model.PatientId), MDVUtility.ToLong(model.NoteId), "Proc Report", base64string, "application/pdf", model.FileName, "Proc Report");
                    PatientDocumentResponse result = JsonConvert.DeserializeObject<PatientDocumentResponse>(docResponse);
                    if (result.status == true)
                    {
                        string DocId = result.PatDocId;
                        string attachDocRes = new Patient_Document_Image_Annotation().AttachPatientDocumentToNote(DocId, MDVUtility.ToInt64(model.NoteId));
                        PatientDocumentResponse attachResult = JsonConvert.DeserializeObject<PatientDocumentResponse>(attachDocRes);
                        if (attachResult.status == true)
                        {
                            var response = new
                            {
                                status = true,
                                attachDocId = DocId
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = attachResult.Message
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = result.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }
                else
                {
                    var response = new
                    {
                        status = true,
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
        /// Method Name: loadOrderingProvider
        /// Author Name: Ahmad Raza
        /// Created Date: 08-03-2016
        /// Description: This function will handle Load of Provider for Radiology Order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string loadOrderingProvider(RadiologyOrderModel model)
        {

            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> list = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();

            DSRadiologyOrder dsRadiology = null;
            BLObject<DSProfileLookup> objProvider = BLLAdminProfileObj.LookupProvider("true");
            DSProfileLookup dsProvider = objProvider.Data;
            BLObject<DSRadiologyOrder> obj;
            obj = BLLClinicalObj.LoadRadiologyOrder(0, MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), "", "", "", "", 0, "", "", "", 0);
            dsRadiology = obj.Data;
            list.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));
            if (dsProvider != null)
            {

                if (dsProvider.Tables[dsProvider.Provider.TableName] != null)
                {

                    DataView view = new DataView(dsRadiology.Tables[dsRadiology.RadiologyOrder.TableName]);
                    DataTable distinctValues = view.ToTable(true, dsRadiology.RadiologyOrder.ProviderIdColumn.ColumnName);
                    foreach (DataRow drProv in dsProvider.Tables[dsProvider.Provider.TableName].Rows)
                    {
                        for (int i = 0; i < distinctValues.Rows.Count; i++)
                        {
                            if (MDVUtility.ToInt64(drProv[dsProvider.Provider.ProviderIdColumn]) == MDVUtility.ToInt64(distinctValues.Rows[i][0].ToString()))
                            {
                                list.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair(drProv[dsProvider.Provider.ShortNameColumn.ColumnName].ToString(), drProv[dsProvider.Provider.ProviderIdColumn.ColumnName].ToString(), drProv[dsProvider.Provider.EntityIdColumn.ColumnName].ToString(), drProv[dsProvider.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                            }
                        }

                    }
                }
            }

            return getJSONofList(list);
        }


        public string LoadUsers(string userName)
        {
            try
            {
                DSUsers dsUsers = null;
                BLObject<DSUsers> obj;
                obj = BLLClinicalObj.LookupUsers(userName);
                if (obj.Data != null)
                {
                    dsUsers = obj.Data;
                    if (dsUsers.Tables[dsUsers.UserLookup.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            UsersCount = dsUsers.Tables[dsUsers.UserLookup.TableName].Rows.Count,
                            UsersLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsUsers.Tables[dsUsers.UserLookup.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            UsersCount = 0,
                            Message = obj.Message
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

        internal string getOrdersForSoap(string orderIDs, long patientId, long notesId, long ProviderId)
        {
            try
            {

                DSRadiologyOrder dsOrderSoap = null;
                DSProblemLists dsProblemSoap = null;
                BLObject<DSRadiologyOrder> obj = BLLClinicalObj.loadRadiologyOrdersForSoap(orderIDs, patientId, ProviderId);
                BLObject<DSProblemLists> objPrb = BLLClinicalObj.attachRadiologyProblemsWithNoteForSoap(orderIDs, notesId);

                dsOrderSoap = obj.Data;
                dsProblemSoap = objPrb.Data;
                if (obj.Data != null)
                {
                    if (dsOrderSoap.Tables[dsOrderSoap.RadiologyOrder.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            radiologySoapCount = dsOrderSoap.Tables[dsOrderSoap.RadiologyOrder.TableName].Rows.Count,
                            radiologySoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.RadiologyOrder.TableName]),
                            radiologyOrderTest_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.RadiologyOrderTest.TableName]),
                            radiologyOrderProblem_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.RadiologyOrderProblem.TableName]),
                            ProblemListSoapCount = objPrb.Data != null && dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName].Rows.Count > 0 ? dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName].Rows.Count : 0,
                            ProblemListSoap_JSON = objPrb.Data != null && dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName].Rows.Count > 0 ? MDVUtility.JSON_DataTable(dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName]) : "[]",
                            // MedicationReviewSoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.MedicationReview.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MedicationSoapCount = 0,
                            MedicationSoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.RadiologyOrder.TableName]),
                            ProblemListSoapCount = objPrb.Data != null && dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName].Rows.Count > 0 ? dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName].Rows.Count : 0,
                            ProblemListSoap_JSON = objPrb.Data != null && dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName].Rows.Count > 0 ? MDVUtility.JSON_DataTable(dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName]) : "[]",
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


        internal static string LookupRadiologyTestReport(RadiologyOrderModel model)
        {
            try
            {
                BLObject<List<RadiologyTestLookup>> obj = BLLClinicalObj.LookupRadiologyTestReport(model.Test);
                List<RadiologyTestLookup> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        radiologyTestCount = modelList.Count,
                        radiologyTestList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        medicationCount = 0,
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

    }
}