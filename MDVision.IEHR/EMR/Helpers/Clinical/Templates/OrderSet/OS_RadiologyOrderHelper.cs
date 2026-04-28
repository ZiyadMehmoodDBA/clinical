using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using System.Data;
using MDVision.Model.Lookups;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Model.Clinical.Templates.OrderSets;
using MDVision.IEHR.EMR.Model.OrdersAndResults.RadiologyOrder;
using MDVision.Model.Admin.Provider;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates.OrderSet
{
    public class OS_RadiologyOrderHelper
    {
        private BLLPatient BLLPatientObj = null;
        private static BLLOrderSet BLLOrderSetObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;

        public OS_RadiologyOrderHelper()
        {
            BLLPatientObj = new BLLPatient();
            BLLOrderSetObj = new BLLOrderSet();
            BLLAdminProfileObj = new BLLAdminProfile();
        }

        private static OS_RadiologyOrderHelper _instance = null;
        public static OS_RadiologyOrderHelper Instance()
        {
            if (_instance == null)
                _instance = new OS_RadiologyOrderHelper();
            return _instance;
        }

        #region Radiology Order Fill, Save and Update Methods

        // Author: Abid Ali
        // Created Date: 17/03/2016
        //OverView: This function will handle fill of Radiology Order
        public string fillRadiologyOrder(OS_RadiologyOrderModel model)
        {
            try
            {
                DSOS_RadiologyOrder dsRadiologyOrder = null, dsRadiologyProblems = null;
                BLObject<DSOS_RadiologyOrder> obj = BLLOrderSetObj.FillRadiologyOrder(MDVUtility.ToInt64(model.RadiologyOrderId));
                if (obj.Data != null)
                {
                    dsRadiologyOrder = obj.Data;
                    if (dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows[0];
                        var radiologyOrderKeyVlaues = new Dictionary<string, string>
                        {

                            { "RadiologyOderDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.OrderDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsRadiologyOrder.OS_RadiologyOrder.OrderDateColumn.ColumnName]).ToShortDateString()},
                            { "Assignee",  MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.AssigneeIdColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.FacilityColumn.ColumnName])},
                            { "hfFacility", MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.FacilityIdColumn.ColumnName])},
                            { "Provider", MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.ProviderColumn.ColumnName])},
                            { "hfProvider", MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.ProviderIdColumn.ColumnName])},
                            { "txtRadiologyOderDate", MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.OrderTimeColumn.ColumnName])},
                            { "bResultExists", MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.bResultExistsColumn.ColumnName])},
                            { "bResultAcknowledged", MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.bResultAcknowledgedColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.CommentsColumn.ColumnName])},

                            //{ "GuarantorName", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.SoapTextColumn.ColumnName])}
                        };
                        //Start Farooq Ahmad 21/3/2016 load the radiology order problems
                        if (MDVUtility.ToInt64(model.RadiologyOrderId) > 0)
                        {
                            obj = BLLOrderSetObj.LoadRadiologyOrderProblems(MDVUtility.ToInt32(model.RadiologyOrderId), MDVUtility.ToInt64(model.OrderSetId), 1, 2000);
                            dsRadiologyProblems = obj.Data;
                        }

                        List<Dictionary<string, string>> lstRadiologyTest = new List<Dictionary<string, string>>();

                        if (MDVUtility.ToInt64(model.RadiologyOrderId) > 0)
                        {
                            DSOS_RadiologyOrder dsRadiologyTest = new DSOS_RadiologyOrder();
                            BLObject<DSOS_RadiologyOrder> objTest = BLLOrderSetObj.LoadRadiologyOrderTest(MDVUtility.ToInt64(model.RadiologyOrderId), "1", "2000");
                            dsRadiologyTest = objTest.Data;

                            foreach (DataRow drRadiologyTest in dsRadiologyTest.Tables[dsRadiologyTest.OS_RadiologyOrderTest.TableName].Rows)
                            {
                                string radiologyTestId = MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.RadiologyOrderTestIdColumn.ColumnName]);
                                var RadiologyTestValues = new Dictionary<string, string>
                            {
                                //{"hfChargeId", radiologyTestId},
                                {"RadiologyOrderTestId", radiologyTestId},
                                {"CPTCode", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.CPTCodeColumn.ColumnName])},
                                {"CPTDescription", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.CPTCodeDescriptionColumn.ColumnName])},
                                {"CPTSNOMEDCodeId", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.CPTSNOMEDIDColumn.ColumnName])},
                                {"CPTSNOMEDDescription", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.CPTSNOMEDDescriptionColumn.ColumnName])},

                                {"RadiologyDate", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.TestDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.TestDateColumn.ColumnName]).ToShortDateString():""},
                                {"RadiologyTime", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.TestTimeColumn.ColumnName])},
                              //  { "RadiologyProcedure", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.CPTCodeDescriptionColumn.ColumnName])},
                                { "CollectedAt", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.CollectedAtIdColumn.ColumnName])},
                                { "Urgency", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.UrgencyIdColumn.ColumnName])},
                                { "Specimen", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.SpecimenIdColumn.ColumnName])},
                                { "PatientInstructions", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.PatientInstructionColumn.ColumnName])},
                                { "VolumeText", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.VolumeLengthColumn.ColumnName])},
                                { "VolumeDDL", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.VolumeIdColumn.ColumnName])},
                                { "FillerInstructions", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.FillerInstructionColumn.ColumnName])},
                                { "Reason", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.ReasonColumn.ColumnName])},
                                { "BodySite", MDVUtility.ToStr(drRadiologyTest[dsRadiologyTest.OS_RadiologyOrderTest.BodySiteColumn.ColumnName])},

                            };

                                lstRadiologyTest.Add(RadiologyTestValues);
                            }

                        }

                        if (dsRadiologyProblems == null)
                            dsRadiologyProblems = new DSOS_RadiologyOrder();
                        //End Farooq Ahmad 21/3/2016 load the radiology order problems
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            radiologyFill_JSON = js.Serialize(radiologyOrderKeyVlaues),
                            //obaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
                            radiologyOrderFill_JSON = MDVUtility.JSON_DataTable(dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName]),
                            //Start Farooq Ahmad 21/3/2016 load the radiology order problems
                            radiologyOrderProblems_JSON = MDVUtility.JSON_DataTable(dsRadiologyProblems.Tables[dsRadiologyProblems.OS_RadiologyOrderProblem.TableName]),
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
        // Created Date: 17/03/2016
        //OverView: This function will save Radiology Order
        public string saveRadiologyOrder(OS_RadiologyOrderModel model)
        {
            try
            {
                DSOS_RadiologyOrder dsRadiologyOrder = new DSOS_RadiologyOrder();
                DSOS_RadiologyOrder.OS_RadiologyOrderRow dr = dsRadiologyOrder.OS_RadiologyOrder.NewOS_RadiologyOrderRow();

                dr.RadiologyOrderId = MDVUtility.ToInt64(model.RadiologyOrderId);
                dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
                dr.LabId = MDVUtility.ToInt64(model.LabId);
                dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
                dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);

                //if (!string.IsNullOrEmpty(model.AssigneeId))
                //    dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
                //else
                //    dr[dsRadiologyOrder.OS_RadiologyOrder.AssigneeIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.OderDate))
                //    dr.OrderDate = MDVUtility.ToDateTime(model.OderDate);
                //else
                //    dr[dsRadiologyOrder.OS_RadiologyOrder.OrderDateColumn] = DBNull.Value;


                //if (!string.IsNullOrEmpty(model.RadiologyOderTime))
                //    dr.OrderTime = MDVUtility.ToStr(model.RadiologyOderTime);
                //else
                //    dr[dsRadiologyOrder.OS_RadiologyOrder.OrderTimeColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.RadiologyBillingTypeId))
                //    dr.BillingTypeId = MDVUtility.ToInt64(model.RadiologyBillingTypeId);
                //else
                //    dr[dsRadiologyOrder.OS_RadiologyOrder.BillingTypeIdColumn] = DBNull.Value;


                //if (!string.IsNullOrEmpty(model.RadiologyPrimaryInsuraceId))
                //    dr.PrimaryInsuraceId = MDVUtility.ToInt64(model.RadiologyPrimaryInsuraceId);
                //else
                //    dr[dsRadiologyOrder.OS_RadiologyOrder.PrimaryInsuraceIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.RadiologySecondaryInsuraceId))
                //    dr.SecondaryInsuraceId = MDVUtility.ToInt64(model.RadiologySecondaryInsuraceId);
                //else
                //    dr[dsRadiologyOrder.OS_RadiologyOrder.SecondaryInsuraceIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.RadiologyTertiaryInsuraceId))
                //    dr.TertiaryInsuraceId = MDVUtility.ToInt64(model.RadiologyTertiaryInsuraceId);
                //else
                //    dr[dsRadiologyOrder.OS_RadiologyOrder.TertiaryInsuraceIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.RadiologyGuarantorId))
                //    dr.GuarantorId = MDVUtility.ToInt64(model.RadiologyGuarantorId);
                //else
                //    dr[dsRadiologyOrder.OS_RadiologyOrder.GuarantorIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.RadiologyRelationShipId))
                //    dr.RelationShipId = MDVUtility.ToInt32(model.RadiologyRelationShipId);
                //else
                //    dr[dsRadiologyOrder.OS_RadiologyOrder.RelationShipIdColumn] = DBNull.Value;


                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                //dr.SoapText = model.soap

                #region Database Insertion

                dsRadiologyOrder.OS_RadiologyOrder.AddOS_RadiologyOrderRow(dr);
                BLObject<DSOS_RadiologyOrder> obj = BLLOrderSetObj.InsertUpdateRadiologyOrder(dsRadiologyOrder);
                dsRadiologyOrder = obj.Data;

                if (obj.Data != null)
                {

                    Int64 radiologicalOrderId = MDVUtility.ToInt64(dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows[0][dsRadiologyOrder.OS_RadiologyOrder.RadiologyOrderIdColumn.ColumnName]);

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        radiologicalOrderId = radiologicalOrderId,
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
        // Created Date: 17/03/2016
        //OverView: This function will update Radiology Order

        public string insertUpdateRadiologyOrder(OS_RadiologyOrderModel model, List<object> lstRadiologyObjects)
        {
            try
            {
                DSOS_RadiologyOrder dsRadiologyOrder = new DSOS_RadiologyOrder();
                DSOS_RadiologyOrder.OS_RadiologyOrderRow dr = null;
                BLObject<DSOS_RadiologyOrder> obj = BLLOrderSetObj.FillRadiologyOrder(MDVUtility.ToInt64(model.RadiologyOrderId));
                dsRadiologyOrder = obj.Data;
                bool isNewRecord = false;

                DSOS_RadiologyOrder.OS_RadiologyOrderRow[] arrRadiologyOrderRows = null;
                string message = string.Empty;
                if (obj.Data != null)
                {
                    arrRadiologyOrderRows = (DSOS_RadiologyOrder.OS_RadiologyOrderRow[])dsRadiologyOrder.OS_RadiologyOrder.Select(dsRadiologyOrder.OS_RadiologyOrder.RadiologyOrderIdColumn.ColumnName + "=" + model.RadiologyOrderId);
                    if (arrRadiologyOrderRows.Length > 0)
                    {
                        dr = arrRadiologyOrderRows[0];
                        message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        dr = dsRadiologyOrder.OS_RadiologyOrder.NewOS_RadiologyOrderRow();
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
                    dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
                    dr.LabId = MDVUtility.ToInt64(model.LabId);
                    dr[dsRadiologyOrder.OS_RadiologyOrder.FacilityColumn] = DBNull.Value;
                    dr[dsRadiologyOrder.OS_RadiologyOrder.ProviderColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.AssigneeId))
                        dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
                    else
                        dr[dsRadiologyOrder.OS_RadiologyOrder.AssigneeIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.OrderDate))
                        dr.OrderDate = MDVUtility.ToDateTime(model.OrderDate);
                    else
                        dr[dsRadiologyOrder.OS_RadiologyOrder.OrderDateColumn] = DBNull.Value;


                    if (!string.IsNullOrEmpty(model.OrderTime))
                        dr.OrderTime = MDVUtility.ToStr(model.OrderTime);
                    else
                        dr[dsRadiologyOrder.OS_RadiologyOrder.OrderTimeColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.BillingTypeId))
                        dr.BillingTypeId = MDVUtility.ToInt64(model.BillingTypeId);
                    else
                        dr[dsRadiologyOrder.OS_RadiologyOrder.BillingTypeIdColumn] = DBNull.Value;


                    if (!string.IsNullOrEmpty(model.PrimaryInsuraceId))
                        dr.PrimaryInsuraceId = MDVUtility.ToInt64(model.PrimaryInsuraceId);
                    else
                        dr[dsRadiologyOrder.OS_RadiologyOrder.PrimaryInsuraceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.SecondaryInsuraceId))
                        dr.SecondaryInsuraceId = MDVUtility.ToInt64(model.SecondaryInsuraceId);
                    else
                        dr[dsRadiologyOrder.OS_RadiologyOrder.SecondaryInsuraceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.TertiaryInsuraceId))
                        dr.TertiaryInsuraceId = MDVUtility.ToInt64(model.TertiaryInsuraceId);
                    else
                        dr[dsRadiologyOrder.OS_RadiologyOrder.TertiaryInsuraceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.GuarantorId))
                        dr.GuarantorId = MDVUtility.ToInt64(model.GuarantorId);
                    else
                        dr[dsRadiologyOrder.OS_RadiologyOrder.GuarantorIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.RelationShipId))
                        dr.RelationShipId = MDVUtility.ToInt32(model.RelationShipId);
                    else
                        dr[dsRadiologyOrder.OS_RadiologyOrder.RelationShipIdColumn] = DBNull.Value;


                    if (!string.IsNullOrEmpty(model.Comments))
                        dr.Comments = model.Comments;
                    else
                        dr[dsRadiologyOrder.OS_RadiologyOrder.CommentsColumn] = DBNull.Value;

                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    //Start 22-03-2016 Humaira Yousaf for status
                    dr.Status = model.Status;
                    //End 22-03-2016 Humaira Yousaf for status

                    if (isNewRecord)
                    {
                        dsRadiologyOrder.OS_RadiologyOrder.AddOS_RadiologyOrderRow(dr);
                    }
                }


                #region Database Insertion/Updation

                BLObject<DSOS_RadiologyOrder> objUpdate = BLLOrderSetObj.InsertUpdateRadiologyOrder(dsRadiologyOrder);
                dsRadiologyOrder = objUpdate.Data;

                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
                //if (objUpdate.Data != null)
                //{
                //    string res = LabOrderHelper.insertUpdateFavListSetting(model.FavListNames);
                //}
                //End 10-06-2016 Abid Ali for favorite list setting for all favLists
                if (obj.Data != null)
                {

                    Int64 RadiologyOrderId = MDVUtility.ToInt64(dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows[0][dsRadiologyOrder.OS_RadiologyOrder.RadiologyOrderIdColumn.ColumnName]);

                    string OrderNo = MDVUtility.ToStr(dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows[0][dsRadiologyOrder.OS_RadiologyOrder.OrderNoColumn.ColumnName]);
                    if (model.RadiologyOrderProblemList != null && model.RadiologyOrderProblemList.Count > 0)
                    {
                        model.RadiologyOrderProblemList.ForEach(cc => cc.RadiologyOrderId = MDVUtility.ToStr(RadiologyOrderId));
                        BLLOrderSetObj.DeleteRadiologyOrderProblems(MDVUtility.ToStr(RadiologyOrderId));

                        var id = saveRadiologyOrderProblem(model.RadiologyOrderProblemList);

                    }

                    insertUpdateRadiologyOrderTest(RadiologyOrderId, lstRadiologyObjects);

                    var response = new
                    {
                        status = true,
                        message = message,
                        radiologicalOrderId = RadiologyOrderId,
                        orderNo = OrderNo
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
        public string updateRadiologyOrder(OS_RadiologyOrderModel model)
        {
            try
            {
                DSOS_RadiologyOrder dsRadiologyOrder = new DSOS_RadiologyOrder();
                BLObject<DSOS_RadiologyOrder> obj = BLLOrderSetObj.FillRadiologyOrder(MDVUtility.ToInt64(model.RadiologyOrderId));
                dsRadiologyOrder = obj.Data;
                foreach (DSOS_RadiologyOrder.OS_RadiologyOrderRow dr in dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows)
                {
                    dr.RadiologyOrderId = MDVUtility.ToInt64(model.RadiologyOrderId);
                    dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
                    dr.LabId = MDVUtility.ToInt64(model.LabId);
                    dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
                    dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                    dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
                    //dr.OrderDate = MDVUtility.ToDateTime(model.RadiologyOderDate);
                    //dr.OrderTime = MDVUtility.ToStr(model.RadiologyOderTime);
                    //dr.BillingTypeId = MDVUtility.ToInt64(model.RadiologyBillingTypeId);
                    //dr.PrimaryInsuraceId = MDVUtility.ToInt64(model.RadiologyPrimaryInsuraceId);
                    //dr.SecondaryInsuraceId = MDVUtility.ToInt64(model.RadiologySecondaryInsuraceId);
                    //dr.TertiaryInsuraceId = MDVUtility.ToInt64(model.RadiologyTertiaryInsuraceId);
                    //dr.GuarantorId = MDVUtility.ToInt64(model.RadiologyGuarantorId);
                    //dr.RelationShipId = MDVUtility.ToInt32(model.RadiologyRelationShipId);

                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Updation
                    if (dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows.Count > 0)
                    {
                        BLObject<DSOS_RadiologyOrder> objUpdate = BLLOrderSetObj.InsertUpdateRadiologyOrder(dsRadiologyOrder);

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

        // Author: Abid Ali
        // Created Date: 17/03/2016
        //OverView: This function will delete Radiology Order
        //public string deleteRadiologyOrder(string radiologyOrderId)
        //{

        //    DSOS_RadiologyOrder dsRadiologyOrder = new DSOS_RadiologyOrder();
        //    BLObject<string> objRadiologyOderDelete = new BLObject<string>();
        //    objRadiologyOderDelete = BLLOrderSetObj.DeleteRadiologyOrder(radiologyOrderId);
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
        public string saveRadiologyOrderProblem(List<OS_RadiologyOrderProblemModel> model)
        {
            try
            {
                DSOS_RadiologyOrder dsRadiologyOrder = new DSOS_RadiologyOrder();

                foreach (var m in model)
                {
                    DSOS_RadiologyOrder.OS_RadiologyOrderProblemRow dr = dsRadiologyOrder.OS_RadiologyOrderProblem.NewOS_RadiologyOrderProblemRow();
                    dr.RadiologyOrderId = MDVUtility.ToInt64(m.RadiologyOrderId);
                    dr.ProblemId = MDVUtility.ToInt32(m.ProblemId);
                    dr.Comments = string.Empty;
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dsRadiologyOrder.OS_RadiologyOrderProblem.AddOS_RadiologyOrderProblemRow(dr);
                }
                #region Database Insertion
                BLObject<DSOS_RadiologyOrder> obj = BLLOrderSetObj.InsertUpdateRadiologyOrderProblems(dsRadiologyOrder);
                dsRadiologyOrder = obj.Data;

                if (obj.Data != null)
                {

                    Int64 RadiologyOrderId = MDVUtility.ToInt64(dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrderProblem.TableName].Rows[0][dsRadiologyOrder.OS_RadiologyOrderProblem.RadiologyOrderIdColumn.ColumnName]);

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
        /// <param name="model" type="OS_RadiologyOrderModel">OS_RadiologyOrderModel model containing data</param>
        public string loadRadiologyOrder(OS_RadiologyOrderModel model)
        {
            try
            {

                DSOS_RadiologyOrder dsRadiology = null;
                BLObject<DSOS_RadiologyOrder> obj;

                obj = BLLOrderSetObj.LoadRadiologyOrder(MDVUtility.ToInt64(model.OrderSetId), model.PageNumber, model.RowsPerPage);
                dsRadiology = obj.Data;

                if (dsRadiology.Tables[dsRadiology.OS_RadiologyOrder.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsRadiology.Tables[dsRadiology.OS_RadiologyOrder.TableName].Rows[dsRadiology.Tables[dsRadiology.OS_RadiologyOrder.TableName].Rows.Count - 1];
                    var RadiologyOrderkeyValues = new Dictionary<string, string>
                        {
                            { "RadiologyOrderId",  MDVUtility.ToStr(dr[dsRadiology.OS_RadiologyOrder.RadiologyOrderIdColumn.ColumnName])},
                            { "SoapText", MDVUtility.ToStr(dr[dsRadiology.OS_RadiologyOrder.SoapTextColumn.ColumnName])}
                        };

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        RadiologyOrderFill_JSON = js.Serialize(RadiologyOrderkeyValues),
                        radiologyOrderCount = dsRadiology.Tables[dsRadiology.OS_RadiologyOrder.TableName].Rows.Count,
                        RadiologyLoad_JSON = MDVUtility.JSON_DataTable(dsRadiology.Tables[dsRadiology.OS_RadiologyOrder.TableName]),
                        iTotalDisplayRecords = (dsRadiology.OS_RadiologyOrder.Rows.Count > 0) ? dsRadiology.OS_RadiologyOrder.Rows[0][dsRadiology.OS_RadiologyOrder.RecordCountColumn.ColumnName] : 0,
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
                        radiologyOrderCount = 0,
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
                DSOS_RadiologyOrder dsRadiologyOrder = null;
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
                    BLObject<DSOS_RadiologyOrder> obj = BLLOrderSetObj.attachRadiologyOrderWithNotes(radiologyOrderId, notesId);
                    if (obj.Data != null)
                    {
                        dsRadiologyOrder = obj.Data;
                        var response = new
                        {
                            status = true,
                            RadiologyOrderTotalCount = dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows.Count,
                            RadiologyOrderCount = dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows.Count,
                            RadiologyOrderLoad_JSON = MDVUtility.JSON_DataTable(dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName]),
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
                    BLObject<string> obj = BLLOrderSetObj.detachRadiologyOrderFromNotes(radiologyOrderId, notesId);
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


                DSOS_RadiologyOrder dsRadiologyOrder = new DSOS_RadiologyOrder();

                BLObject<DSOS_RadiologyOrder> objRadiologyOrderTest = BLLOrderSetObj.LoadRadiologyOrderTest(MDVUtility.ToInt64(RadiologyId), "1", "2000");
                dsRadiologyOrder = objRadiologyOrderTest.Data;
                //dsRadiologyOrder = objRadiologyOrder.Data;
                List<RadiologyOrderTestModel> lstRadiologyOrder = lstObjects.OfType<RadiologyOrderTestModel>().ToList();
                int id = -1;

                foreach (RadiologyOrderTestModel CurrentModel in lstRadiologyOrder)
                {
                    Int32 currentRadiologyTestId = MDVUtility.ToInt32(CurrentModel.RadiologyOrderTestId);
                    currentRadiologyTestId = currentRadiologyTestId == 0 ? id-- : currentRadiologyTestId;

                    DSOS_RadiologyOrder.OS_RadiologyOrderTestRow RowRadiologyOrderTest = null;
                    DSOS_RadiologyOrder.OS_RadiologyOrderTestRow[] arrRadiologyOrderTestRows = (DSOS_RadiologyOrder.OS_RadiologyOrderTestRow[])dsRadiologyOrder.OS_RadiologyOrderTest.Select(dsRadiologyOrder.OS_RadiologyOrderTest.RadiologyOrderTestIdColumn.ColumnName + "=" + CurrentModel.RadiologyOrderTestId);

                    if (arrRadiologyOrderTestRows.Length > 0)
                    {
                        RowRadiologyOrderTest = arrRadiologyOrderTestRows[0];
                    }
                    else
                    {
                        RowRadiologyOrderTest = dsRadiologyOrder.OS_RadiologyOrderTest.NewOS_RadiologyOrderTestRow();
                        RowRadiologyOrderTest.RadiologyOrderTestId = currentRadiologyTestId;
                    }

                    //RowRadiologyOrderTest = dsRadiologyOrder.OS_RadiologyOrderTest.NewRadiologyOrderTestRow();
                    if (RowRadiologyOrderTest != null)
                    {
                        RowRadiologyOrderTest.RadiologyOrderId = RadiologyId;

                        if (!string.IsNullOrEmpty(CurrentModel.CPTDescription))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.CPTCodeDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTDescription);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.CPTCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.CPTCodeColumn] = MDVUtility.ToStr(CurrentModel.CPTCode);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.CPTCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDDescription))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.CPTSNOMEDDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTSNOMEDDescription);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.CPTSNOMEDDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDCodeId))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.CPTSNOMEDIDColumn] = MDVUtility.ToStr(CurrentModel.CPTSNOMEDCodeId);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.CPTSNOMEDIDColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.dtpRadiologyDate))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.TestDateColumn] = MDVUtility.ToDateTime(CurrentModel.dtpRadiologyDate);
                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.TestDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.tpRadiologyTime))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.TestTimeColumn] = MDVUtility.ToStr(CurrentModel.tpRadiologyTime);
                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.TestTimeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.RadiologyProcedure))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.CPTCodeDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTDescription);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.CPTCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CollectedAt))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.CollectedAtIdColumn] = MDVUtility.ToInt32(CurrentModel.CollectedAt);

                        }

                        if (!string.IsNullOrEmpty(CurrentModel.Urgency))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.UrgencyIdColumn] = MDVUtility.ToInt32(CurrentModel.Urgency);

                        }

                        if (!string.IsNullOrEmpty(CurrentModel.Specimen))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.SpecimenIdColumn] = MDVUtility.ToInt32(CurrentModel.Specimen);

                        }

                        if (!string.IsNullOrEmpty(CurrentModel.PatientInstructions))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.PatientInstructionColumn] = MDVUtility.ToStr(CurrentModel.PatientInstructions);

                        }

                        if (!string.IsNullOrEmpty(CurrentModel.VolumeText))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.VolumeLengthColumn] = MDVUtility.ToStr(CurrentModel.VolumeText);

                        }

                        if (!string.IsNullOrEmpty(CurrentModel.VolumeDDL))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.VolumeIdColumn] = MDVUtility.ToInt32(CurrentModel.VolumeDDL);

                        }
                        if (!string.IsNullOrEmpty(CurrentModel.FillerInstructions))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.FillerInstructionColumn] = MDVUtility.ToStr(CurrentModel.FillerInstructions);

                        }

                        if (!string.IsNullOrEmpty(CurrentModel.Reason))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.ReasonColumn] = MDVUtility.ToStr(CurrentModel.Reason);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.ReasonColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.BodySite))
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.BodySiteColumn] = MDVUtility.ToStr(CurrentModel.BodySite);

                        }
                        else
                        {
                            RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.BodySiteColumn] = DBNull.Value;
                        }
                        //Start Farooq Ahmad 23/3/2016 Update Soap Text
                        RowRadiologyOrderTest[dsRadiologyOrder.OS_RadiologyOrderTest.SoapTextColumn] = string.Format("{0} {1} {2} {3} {4}", string.Empty, "<b>" + CurrentModel.RadiologyProcedure + "</b>", string.IsNullOrWhiteSpace(CurrentModel.Specimen) ? string.Empty : string.Concat(",Specimen: ", CurrentModel.Specimen_text), string.IsNullOrWhiteSpace(CurrentModel.VolumeText) ? string.Empty : string.Concat(",Volume: ", CurrentModel.VolumeText), string.IsNullOrWhiteSpace(CurrentModel.Urgency) ? string.Empty : string.Concat(",Urgency: ", CurrentModel.Urgency_text));
                        //End Farooq Ahmad 23/3/2016 Update Soap Text


                        RowRadiologyOrderTest.IsActive = true;
                        RowRadiologyOrderTest.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowRadiologyOrderTest.CreatedOn = DateTime.Now;
                        RowRadiologyOrderTest.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowRadiologyOrderTest.ModifiedOn = DateTime.Now;

                        if (arrRadiologyOrderTestRows.Length < 1)
                        {
                            dsRadiologyOrder.OS_RadiologyOrderTest.AddOS_RadiologyOrderTestRow(RowRadiologyOrderTest);
                        }
                        //dsRadiologyOrder.OS_RadiologyOrderTest.AddRadiologyOrderTestRow(RowRadiologyOrderTest);

                    }
                }

                #region Database Insertion/Updation

                BLObject<DSOS_RadiologyOrder> objInsertedRadiologyTest = BLLOrderSetObj.insertUpdateRadiologyOrderTest(dsRadiologyOrder);
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
                obj = BLLOrderSetObj.deleteRadiologyOrderTest(RadiologyOrderTestId);
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
                DSOS_RadiologyOrder dsRadiology = null, dsRadiologyProblems = null;
                BLObject<string> obj;
                obj = BLLOrderSetObj.DeleteRadiologyOrder(radiologyOrderId);
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


        public string loadPatientGuarantor(OS_RadiologyOrderModel model)
        {
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                //  DSPatient dsPatient = null;
                DSPatient dsPatientRelation = null;
                // DSPatientProfile dsRelationShip = null;
                string relationshipName = "";
                Int64 relationshipId = 0;
                //BLObject<DSPatient> objpatient = BLLPatientObj.FillPatientById(MDVUtility.ToInt32(model.OrderSetId));
                //dsPatient = objpatient.Data;


                BLObject<DSPatient> objPatientRelation = BLLPatientObj.FillPatientAndInsuranceById(MDVUtility.ToInt32(model.OrderSetId), 0, 0);
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
        /// <param name="model" type="OS_RadiologyOrderModel">model</param>
        public string previewRadiologyOrder(OS_RadiologyOrderModel model)
        {
            try
            {
                BLObject<byte[]> obj = BLLOrderSetObj.previewRadiologyOrder(MDVUtility.ToInt64(model.RadiologyOrderId), MDVUtility.ToInt64(model.OrderSetId));

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

        /// <summary>
        /// Method Name: loadOrderingProvider
        /// Author Name: Ahmad Raza
        /// Created Date: 08-03-2016
        /// Description: This function will handle Load of Provider for Radiology Order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string loadOrderingProvider(OS_RadiologyOrderModel model)
        {

            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> list = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();

            DSOS_RadiologyOrder dsRadiology = null;
            BLObject<DSProfileLookup> objProvider = BLLAdminProfileObj.LookupProvider("true");
            DSProfileLookup dsProvider = objProvider.Data;
            BLObject<DSOS_RadiologyOrder> obj;
            obj = BLLOrderSetObj.LoadRadiologyOrder(MDVUtility.ToInt64(model.OrderSetId), "", "");
            dsRadiology = obj.Data;
            list.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));
            if (dsProvider != null)
            {
                if (dsProvider.Tables[dsProvider.Provider.TableName] != null)
                {
                    DataView view = new DataView(dsRadiology.Tables[dsRadiology.OS_RadiologyOrder.TableName]);
                    DataTable distinctValues = view.ToTable(true, dsRadiology.OS_RadiologyOrder.ProviderIdColumn.ColumnName);
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

        internal string getOrdersForSoap(string orderIDs, long patientId)
        {
            try
            {

                DSOS_RadiologyOrder dsOrderSoap = null;
                BLObject<DSOS_RadiologyOrder> obj = BLLOrderSetObj.loadRadiologyOrdersForSoap(orderIDs, patientId);


                dsOrderSoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsOrderSoap.Tables[dsOrderSoap.OS_RadiologyOrder.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            radiologySoapCount = dsOrderSoap.Tables[dsOrderSoap.OS_RadiologyOrder.TableName].Rows.Count,
                            radiologySoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.OS_RadiologyOrder.TableName]),
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
                            MedicationSoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.OS_RadiologyOrder.TableName]),
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


        public string LookupRadiologyTestReport(OS_RadiologyOrderModel model)
        {
            try
            {
                BLObject<List<RadiologyTestLookup>> obj = BLLOrderSetObj.LookupRadiologyTestReport(model.Test);
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