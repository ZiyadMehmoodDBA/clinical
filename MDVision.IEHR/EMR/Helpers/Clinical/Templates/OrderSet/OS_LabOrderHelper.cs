using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using MDVision.Business.BCommon;
using System.Data;

using MDVision.IEHR.Controls.DashBoard;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using System.Web.Script.Serialization;
using MDVision.IEHR.EMR.Model.Clinical.Templates.OrderSets;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates.OrderSet
{
    public class OS_LabOrderHelper
    {
        private BLLPatient BLLPatientObj = null;
        private static BLLOrderSet BLLOrderSetObj = null;
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;
        public OS_LabOrderHelper()
        {
            BLLPatientObj = new BLLPatient();
            BLLOrderSetObj = new BLLOrderSet();
            BLLAdminSecurityObj = new BLLAdminSecurity();
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        private static OS_LabOrderHelper _instance = null;
        public static OS_LabOrderHelper Instance()
        {
            if (_instance == null)
                _instance = new OS_LabOrderHelper();
            return _instance;
        }

        #region Lab Order Fill, Save and Update Methods

        // Author: Ahsan Nasir

        public string saveLabTestABN(string fieldsJSON)
        {


            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                var Json = SearchedfieldsJSON["data"];
                var fields = ser.Deserialize<dynamic>(Json);


                long id = Convert.ToInt64(fields["LabOrderTestId"]);
                var isABN = Convert.ToBoolean(fields["IsABN"]);

                BLObject<string> result = BLLOrderSetObj.SaveABNAgainstTest(id, isABN);

                var response = new
                {
                    status = true,
                    message = result
                };

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



        // Author: Azeem Raza Tayyab
        // Created Date: 13-Jan-2017
        //OverView: This function will handle fill of Lab Order


        public string fillLabOrder(OS_LabOrderModel model)
        {
            try
            {
                System.Diagnostics.Debug.Write("Start Time of Helper fillLabOrder = " + DateTime.Now);
                DSOS_LabOrder dsLabOrder = null, dsLabProblems = null;
                BLObject<DSOS_LabOrder> obj = BLLOrderSetObj.FillLabOrder(MDVUtility.ToInt64(model.LabOrderId));
                if (obj.Data != null)
                {
                    dsLabOrder = obj.Data;
                    if (dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows.Count > 0)
                    {
                        if (dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows[0];
                            var LabOrderKeyVlaues = new Dictionary<string, string>
                            {
                                { "LabOderDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.OrderDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsLabOrder.OS_LabOrder.OrderDateColumn.ColumnName]).ToShortDateString()},
                                { "Assignee",  MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.AssigneeIdColumn.ColumnName])},
                                { "txtFacility", MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.FacilityColumn.ColumnName])},
                                { "hfFacility", MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.FacilityIdColumn.ColumnName])},
                                { "Provider", MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.ProviderColumn.ColumnName])},
                                { "hfProvider", MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.ProviderIdColumn.ColumnName])},
                                { "txtLabOderDate", MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.OrderTimeColumn.ColumnName])},
                                { "bResultExists", MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.bResultExistsColumn.ColumnName])},
                                { "bResultAcknowledged", MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.bResultAcknowledgedColumn.ColumnName])},
                                { "ClientNo", MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.ClientNoColumn.ColumnName])},
                                { "FullName", MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.FullNameColumn.ColumnName])},
                                //{ "GuarantorName", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.SoapTextColumn.ColumnName])}
                            };
                            //Start Farooq Ahmad 21/3/2016 load the Lab order problems
                            if (MDVUtility.ToInt32(model.LabOrderId) > 0)
                            {
                                obj = BLLOrderSetObj.LoadLabOrderProblems(MDVUtility.ToInt32(model.LabOrderId), MDVUtility.ToInt64(model.OrderSetId), 1, 2000);
                                dsLabProblems = obj.Data;
                            }

                            List<Dictionary<string, string>> lstLabTest = new List<Dictionary<string, string>>();

                            if (MDVUtility.ToInt64(model.LabOrderId) > 0)
                            {
                                DSOS_LabOrder dsLabTest = new DSOS_LabOrder();
                                BLObject<DSOS_LabOrder> objTest = BLLOrderSetObj.LoadLabOrderTest(MDVUtility.ToInt64(model.LabOrderId), 0, MDVUtility.ToInt64(model.OrderSetId), "1", "2000");
                                dsLabTest = objTest.Data;

                                foreach (DataRow drLabTest in dsLabTest.Tables[dsLabTest.OS_LabOrderTest.TableName].Rows)
                                {
                                    string LabTestId = MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.LabOrderTestIdColumn.ColumnName]);
                                    var LabTestValues = new Dictionary<string, string>
                                    {
                                        {"LabOrderTestId", LabTestId},
                                        //{"LabOrderTestId", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.LabOrderTestIdColumn.ColumnName])},
                                        {"LabDate", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.TestDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drLabTest[dsLabTest.OS_LabOrderTest.TestDateColumn.ColumnName]).ToShortDateString():""},
                                        {"LabTime", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.TestTimeColumn.ColumnName])},
                                        //{ "LabProcedure2",MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.CPTCodeDescriptionColumn.ColumnName])},
                                        { "CollectedAt", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.CollectedAtIdColumn.ColumnName])},
                                        { "Urgency", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.UrgencyIdColumn.ColumnName])},
                                        { "Specimen", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.SpecimenIdColumn.ColumnName])},
                                         { "AlternativeSpecimen", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.AltSpecimenIdColumn.ColumnName])},
                                        { "PatientInstructions", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.PatientInstructionColumn.ColumnName])},
                                        { "VolumeText", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.VolumeLengthColumn.ColumnName])},

                                        { "CPTCode", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.CPTCodeColumn.ColumnName])},
                                        {"CPTDescription",MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.CPTCodeDescriptionColumn.ColumnName])},
                                        {"SampleStorage",MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.SampleStorageColumn.ColumnName])},
                                        { "ClinicalLabOrderDiet", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.DietIdColumn.ColumnName])},
                                        { "VolumeDDL", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.VolumeIdColumn.ColumnName])},
                                        { "FillerInstructions", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.FillerInstructionColumn.ColumnName])},
                                        { "AOEExists", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.AOEExistsColumn.ColumnName])},
                                        { "AnswerExists", MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.AnswerExistsColumn.ColumnName])},

                                    };
                                    lstLabTest.Add(LabTestValues);
                                }

                            }

                            if (dsLabProblems == null)
                                dsLabProblems = new DSOS_LabOrder();
                            //End Farooq Ahmad 21/3/2016 load the Lab order problems
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                LabFill_JSON = js.Serialize(LabOrderKeyVlaues),
                                //obaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
                                LabOderFill_JSON = MDVUtility.JSON_DataTable(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName]),
                                //Start Farooq Ahmad 21/3/2016 load the Lab order problems
                                LabOrderProblems_JSON = MDVUtility.JSON_DataTable(dsLabProblems.Tables[dsLabProblems.OS_LabOrderProblem.TableName]),
                                //End Farooq Ahmad 21/3/2016 load the Lab order problems
                                LabOrderTest_JSON = js.Serialize(lstLabTest),

                            };
                            System.Diagnostics.Debug.Write("\n End Time of Helper fillLabOrder = " + DateTime.Now);
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                LabOderFill_JSON = MDVUtility.JSON_DataTable(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName])

                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

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

        // Author: Azeem Raza Tayyab
        // Created Date: 13-Jan-2017
        //OverView: This function will save Lab Order
        public string saveLabOrder(OS_LabOrderModel model)
        {
            try
            {
                DSOS_LabOrder dsLabOrder = new DSOS_LabOrder();
                DSOS_LabOrder.OS_LabOrderRow dr = dsLabOrder.OS_LabOrder.NewOS_LabOrderRow();

                dr.LabOrderId = MDVUtility.ToInt64(model.LabOrderId);
                dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
                dr.LabId = MDVUtility.ToInt64(model.LabId);
                dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
                dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);

                //if (!string.IsNullOrEmpty(model.AssigneeId))
                //    dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
                //else
                //    dr[dsLabOrder.OS_LabOrder.AssigneeIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.OderDate))
                //    dr.OrderDate = MDVUtility.ToDateTime(model.OderDate);
                //else
                //    dr[dsLabOrder.OS_LabOrder.OrderDateColumn] = DBNull.Value;


                //if (!string.IsNullOrEmpty(model.LabOderTime))
                //    dr.OrderTime = MDVUtility.ToStr(model.LabOderTime);
                //else
                //    dr[dsLabOrder.OS_LabOrder.OrderTimeColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.LabBillingTypeId))
                //    dr.BillingTypeId = MDVUtility.ToInt64(model.LabBillingTypeId);
                //else
                //    dr[dsLabOrder.OS_LabOrder.BillingTypeIdColumn] = DBNull.Value;


                //if (!string.IsNullOrEmpty(model.LabPrimaryInsuraceId))
                //    dr.PrimaryInsuraceId = MDVUtility.ToInt64(model.LabPrimaryInsuraceId);
                //else
                //    dr[dsLabOrder.OS_LabOrder.PrimaryInsuraceIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.LabSecondaryInsuraceId))
                //    dr.SecondaryInsuraceId = MDVUtility.ToInt64(model.LabSecondaryInsuraceId);
                //else
                //    dr[dsLabOrder.OS_LabOrder.SecondaryInsuraceIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.LabTertiaryInsuraceId))
                //    dr.TertiaryInsuraceId = MDVUtility.ToInt64(model.LabTertiaryInsuraceId);
                //else
                //    dr[dsLabOrder.OS_LabOrder.TertiaryInsuraceIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.LabGuarantorId))
                //    dr.GuarantorId = MDVUtility.ToInt64(model.LabGuarantorId);
                //else
                //    dr[dsLabOrder.OS_LabOrder.GuarantorIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.LabRelationShipId))
                //    dr.RelationShipId = MDVUtility.ToInt32(model.LabRelationShipId);
                //else
                //    dr[dsLabOrder.OS_LabOrder.RelationShipIdColumn] = DBNull.Value;


                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                //dr.SoapText = model.soap

                #region Database Insertion

                dsLabOrder.OS_LabOrder.AddOS_LabOrderRow(dr);
                BLObject<DSOS_LabOrder> obj = BLLOrderSetObj.InsertUpdateLabOrder(dsLabOrder);
                dsLabOrder = obj.Data;

                if (obj.Data != null)
                {

                    Int64 radiologicalOrderId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows[0][dsLabOrder.OS_LabOrder.LabOrderIdColumn.ColumnName]);

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

        // Author: Azeem Raza Tayyab
        // Created Date: 13-Jan-2017
        //OverView: This function will update Lab Order

        public string insertUpdateLabOrder(OS_LabOrderModel model, List<object> lstLabObjects)
        {
            try
            {
                System.Diagnostics.Debug.Write("Start Time of Helper InsertUpdateLabOrder = " + DateTime.Now);
                DSOS_LabOrder dsLabOrder = new DSOS_LabOrder();
                DSOS_LabOrder.OS_LabOrderRow dr = null;
                BLObject<DSOS_LabOrder> obj = BLLOrderSetObj.FillLabOrder(MDVUtility.ToInt64(model.LabOrderId));
                dsLabOrder = obj.Data;
                bool isNewRecord = false;

                DSOS_LabOrder.OS_LabOrderRow[] arrLabOrderRows = null;
                string message = string.Empty;
                if (obj.Data != null)
                {
                    //A safety check
                    arrLabOrderRows = (DSOS_LabOrder.OS_LabOrderRow[])dsLabOrder.OS_LabOrder.Select(dsLabOrder.OS_LabOrder.LabOrderIdColumn.ColumnName + "=" + model.LabOrderId);
                    if (arrLabOrderRows.Length > 0)
                    {
                        dr = arrLabOrderRows[0];
                        message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        dr = dsLabOrder.OS_LabOrder.NewOS_LabOrderRow();
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
                    dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
                    dr.LabId = MDVUtility.ToInt64(model.LabId);
                    //dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
                    //dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                    dr[dsLabOrder.OS_LabOrder.FacilityColumn] = DBNull.Value;
                    dr[dsLabOrder.OS_LabOrder.ProviderColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.AssigneeId))
                        dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
                    else
                        dr[dsLabOrder.OS_LabOrder.AssigneeIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.OrderDate))
                        dr.OrderDate = MDVUtility.ToDateTime(model.OrderDate);
                    else
                        dr[dsLabOrder.OS_LabOrder.OrderDateColumn] = DBNull.Value;


                    if (!string.IsNullOrEmpty(model.OrderTime))
                        dr.OrderTime = MDVUtility.ToStr(model.OrderTime);
                    else
                        dr[dsLabOrder.OS_LabOrder.OrderTimeColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.BillingTypeId))
                        dr.BillingTypeId = MDVUtility.ToInt64(model.BillingTypeId);
                    else
                        dr[dsLabOrder.OS_LabOrder.BillingTypeIdColumn] = DBNull.Value;


                    if (!string.IsNullOrEmpty(model.PrimaryInsuraceId))
                        dr.PrimaryInsuraceId = MDVUtility.ToInt64(model.PrimaryInsuraceId);
                    else
                        dr[dsLabOrder.OS_LabOrder.PrimaryInsuraceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.SecondaryInsuraceId))
                        dr.SecondaryInsuraceId = MDVUtility.ToInt64(model.SecondaryInsuraceId);
                    else
                        dr[dsLabOrder.OS_LabOrder.SecondaryInsuraceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.TertiaryInsuraceId))
                        dr.TertiaryInsuraceId = MDVUtility.ToInt64(model.TertiaryInsuraceId);
                    else
                        dr[dsLabOrder.OS_LabOrder.TertiaryInsuraceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.GuarantorId))
                        dr.GuarantorId = MDVUtility.ToInt64(model.GuarantorId);
                    else
                        dr[dsLabOrder.OS_LabOrder.GuarantorIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.RelationShipId))
                        dr.RelationShipId = MDVUtility.ToInt32(model.RelationShipId);
                    else
                        dr[dsLabOrder.OS_LabOrder.RelationShipIdColumn] = DBNull.Value;

                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.Status = model.Status;

                    if (isNewRecord)
                    {
                        dsLabOrder.OS_LabOrder.AddOS_LabOrderRow(dr);
                    }
                }


                #region Database Insertion/Updation

                BLObject<DSOS_LabOrder> objUpdate = BLLOrderSetObj.InsertUpdateLabOrder(dsLabOrder);

                dsLabOrder = objUpdate.Data;
                //if (objUpdate.Data != null)
                //{
                //    string res = insertUpdateFavListSetting(model.FavListNames);
                //}


                if (obj.Data != null)
                {

                    Int64 LabOrderId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows[0][dsLabOrder.OS_LabOrder.LabOrderIdColumn.ColumnName]);
                    string OrderNo = MDVUtility.ToStr(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows[0][dsLabOrder.OS_LabOrder.OrderNoColumn.ColumnName]);
                    if (model.LabOrderProblemList != null && model.LabOrderProblemList.Count > 0)
                    {
                        model.LabOrderProblemList.ForEach(cc => cc.LabOrderId = MDVUtility.ToStr(LabOrderId));
                        BLLOrderSetObj.DeleteLabOrderProblems(MDVUtility.ToStr(LabOrderId));

                        var id = saveLabOrderProblem(model.LabOrderProblemList);

                    }

                    insertUpdateLabOrderTest(LabOrderId, lstLabObjects, model.CPTCodeQuestionsAnswers);
                    System.Diagnostics.Debug.Write("\n End Time of Helper InsertUpdateLabOrder = " + DateTime.Now);

                    #region 'Azhar Created HL7 Message Call Region'
                    string HL7MessageContent = string.Empty;
                    if (model.Status.Equals("Signed"))
                    {
                        //HL7MessageContent = ClinicalHL7Creation.Instance().CreateHL7LabOrder(LabOrderId, MDVUtility.ToInt64(model.OrderSetId));
                    }
                    #endregion
                    var response = new
                    {
                        status = true,
                        message = message,
                        radiologicalOrderId = LabOrderId,
                        orderNo = OrderNo,
                        HL7MessageContent = HL7MessageContent,
                        labFavList = model.FavListNames
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
        public string updateLabOrder(OS_LabOrderModel model)
        {
            try
            {
                DSOS_LabOrder dsLabOrder = new DSOS_LabOrder();
                BLObject<DSOS_LabOrder> obj = BLLOrderSetObj.FillLabOrder(MDVUtility.ToInt64(model.LabOrderId));
                dsLabOrder = obj.Data;
                foreach (DSOS_LabOrder.OS_LabOrderRow dr in dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows)
                {
                    dr.LabOrderId = MDVUtility.ToInt64(model.LabOrderId);
                    dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
                    dr.LabId = MDVUtility.ToInt64(model.LabId);
                    dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
                    dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                    dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
                    //dr.OrderDate = MDVUtility.ToDateTime(model.LabOderDate);
                    //dr.OrderTime = MDVUtility.ToStr(model.LabOderTime);
                    //dr.BillingTypeId = MDVUtility.ToInt64(model.LabBillingTypeId);
                    //dr.PrimaryInsuraceId = MDVUtility.ToInt64(model.LabPrimaryInsuraceId);
                    //dr.SecondaryInsuraceId = MDVUtility.ToInt64(model.LabSecondaryInsuraceId);
                    //dr.TertiaryInsuraceId = MDVUtility.ToInt64(model.LabTertiaryInsuraceId);
                    //dr.GuarantorId = MDVUtility.ToInt64(model.LabGuarantorId);
                    //dr.RelationShipId = MDVUtility.ToInt32(model.LabRelationShipId);

                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Updation
                    if (dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows.Count > 0)
                    {
                        BLObject<DSOS_LabOrder> objUpdate = BLLOrderSetObj.InsertUpdateLabOrder(dsLabOrder);

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



        // Author: Farooq Ahmad
        // Created Date: 18/03/2016
        //OverView: This function will save Lab Order Problem
        public string saveLabOrderProblem(List<LabOrderProblemModel> model)
        {
            try
            {
                DSOS_LabOrder dsLabOrder = new DSOS_LabOrder();
                Int64 LabOrderId = -1;
                foreach (var m in model)
                {
                    DSOS_LabOrder.OS_LabOrderProblemRow dr = dsLabOrder.OS_LabOrderProblem.NewOS_LabOrderProblemRow();
                    dr.LabOrderId = MDVUtility.ToInt64(m.LabOrderId);
                    dr.ProblemId = MDVUtility.ToInt32(m.ProblemId);
                    dr.Comments = string.Empty;
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dsLabOrder.OS_LabOrderProblem.AddOS_LabOrderProblemRow(dr);
                    LabOrderId = MDVUtility.ToInt64(m.LabOrderId);
                }
                #region Database Insertion
                BLObject<DSOS_LabOrder> obj = BLLOrderSetObj.InsertUpdateLabOrderProblems(dsLabOrder);
                dsLabOrder = obj.Data;

                if (obj.Data != null)
                {

                    if (dsLabOrder.Tables[dsLabOrder.OS_LabOrderProblem.TableName].Rows.Count > 0)
                    {
                        LabOrderId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.OS_LabOrderProblem.TableName].Rows[0][dsLabOrder.OS_LabOrderProblem.LabOrderIdColumn.ColumnName]);

                    }

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        LabOrderId = LabOrderId,
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

        #region Lab order Load, Attach/Detach with Notes
        /// <summary>
        /// Module Name: loadLabOrder
        /// Author: Azeem Raza Tayyab
        /// Created Date: 17-03-2016
        /// Description: Loads Lab Orders
        /// </summary>
        /// <param name="model" type="OS_LabOrderModel">OS_LabOrderModel model containing data</param>
        public string loadLabOrder(OS_LabOrderModel model)
        {
            try
            {

                DSOS_LabOrder dsLab = null;
                BLObject<DSOS_LabOrder> obj;

                obj = BLLOrderSetObj.LoadLabOrder(MDVUtility.ToInt64(model.OrderSetId), model.PageNumber, model.RowsPerPage);
                dsLab = obj.Data;

                if (dsLab.Tables[dsLab.OS_LabOrder.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsLab.Tables[dsLab.OS_LabOrder.TableName].Rows[dsLab.Tables[dsLab.OS_LabOrder.TableName].Rows.Count - 1];



                    var LabOrderkeyValues = new Dictionary<string, string>
                        {
                            { "LabOrderId",  MDVUtility.ToStr(dr[dsLab.OS_LabOrder.LabOrderIdColumn.ColumnName])},
                            { "SoapText", MDVUtility.ToStr(dr[dsLab.OS_LabOrder.SoapTextColumn.ColumnName])}
                        };


                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabOrderFill_JSON = js.Serialize(LabOrderkeyValues),
                        LabOrderCount = dsLab.Tables[dsLab.OS_LabOrder.TableName].Rows.Count,
                        LabLoad_JSON = MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.OS_LabOrder.TableName]),
                        iTotalDisplayRecords = (dsLab.OS_LabOrder.Rows.Count > 0) ? dsLab.OS_LabOrder.Rows[0][dsLab.OS_LabOrder.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabOrderCount = 0,
                        LabOrderFill_JSON = "[]",
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
        /// Method Name: attachLabOrderWithNotes
        /// Author: Azeem Raza Tayyab
        /// Description: attaching Lab order with notes
        /// </summary>
        /// <param name="LabOrderId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string attachLabOrderWithNotes(string LabOrderId, long notesId)
        {
            try
            {
                DSOS_LabOrder dsLabOrder = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(LabOrderId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<DSOS_LabOrder> obj = BLLOrderSetObj.attachLabOrderWithNotes(LabOrderId, notesId);
                    if (obj.Data != null)
                    {
                        dsLabOrder = obj.Data;
                        var response = new
                        {
                            status = true,
                            LabOrderTotalCount = dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows.Count,
                            LabOrderCount = dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows.Count,
                            LabOrderLoad_JSON = MDVUtility.JSON_DataTable(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName]),
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
        ///   Method Name: detachLabOrderFromNotes
        ///   Author: Azeem Raza Tayyab
        ///   Description: Detaching Lab order from notes
        /// </summary>
        /// <param name="LabOrderId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string detachLabOrderFromNotes(string LabOrderId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(LabOrderId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<string> obj = BLLOrderSetObj.detachLabOrderFromNotes(LabOrderId, notesId);
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

        /// <summary>
        /// Method Name: loadOrderingProvider
        /// Author Name: Azeem Raza Tayyab
        /// Created Date: 13-Jan-2017
        /// Description: This function will handle Load of Provider for Lab Order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string loadOrderingProvider(OS_LabOrderModel model)
        {

            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> list = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();

            DSOS_LabOrder dsLabOrder = null;
            BLObject<DSProfileLookup> objProvider = BLLAdminProfileObj.LookupProvider("true");
            DSProfileLookup dsProvider = objProvider.Data;
            BLObject<DSOS_LabOrder> obj;
            obj = BLLOrderSetObj.LoadLabOrder(MDVUtility.ToInt64(model.OrderSetId), "", "");
            dsLabOrder = obj.Data;
            list.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));
            if (dsLabOrder != null)
            {

                if (dsProvider.Tables[dsProvider.Provider.TableName] != null)
                {
                    DataView view = new DataView(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName]);
                    if (view.Count > 0)
                    {
                        DataTable distinctValues = view.ToTable(true, dsLabOrder.OS_LabOrder.ProviderIdColumn.ColumnName);
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
            }

            return getJSONofList(list);
        }


        /// <summary>
        /// Method Name: deleteLabOrder
        /// Author : Azeem Raza Tayyab
        /// Description: This function will delete the selected Lab Order
        /// </summary>
        /// <param name="LabOrderId"></param>
        /// <returns></returns>
        public string deleteLabOrder(string LabOrderId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLOrderSetObj.DeleteLabOrder(LabOrderId);
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


        public string loadPatientGuarantor(OS_LabOrderModel model)
        {
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSPatientProfile dsPatientRelation = null;
                // DSPatientProfile dsRelationShip = null;
                string relationshipName = "";
                Int64 relationshipId = 0;
                BLObject<DSPatientProfile> objpatient = BLLPatientObj.LoadPatientGuarantor(0, MDVUtility.ToInt32(model.OrderSetId), "1");
                dsPatientRelation = objpatient.Data;

                if (dsPatientRelation != null && dsPatientRelation.Tables[dsPatientRelation.Guarantor.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsPatientRelation.Tables[dsPatientRelation.Guarantor.TableName].Rows[0];
                    var GuarantorkeyValues = new Dictionary<string, string>
                        {
                            { "GuarantorId",  MDVUtility.ToStr(dr[dsPatientRelation.Guarantor.GuarantorIdColumn.ColumnName])},
                            { "GuarantorName", string.Concat( MDVUtility.ToStr(dr[dsPatientRelation.Guarantor.FirstNameColumn.ColumnName])," ",MDVUtility.ToStr(dr[dsPatientRelation.Guarantor.LastNameColumn.ColumnName]))},
                        };

                    relationshipName = MDVUtility.ToStr(dr[dsPatientRelation.Guarantor.RelationNameColumn.ColumnName]);
                    relationshipId = MDVUtility.ToInt64(dr[dsPatientRelation.Guarantor.RelationIdColumn.ColumnName]);
                    var response = new
                    {
                        status = true,
                        relationshipId = relationshipId,
                        relationName = relationshipName,
                        GuarantorFill_JSON = js.Serialize(GuarantorkeyValues),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
        /// Method Name: previewLabOrder
        /// Author : Azeem Raza Tayyab
        /// Created Date: 13-Jan-2017
        /// Description: Creates PDF to view Lab Order
        /// </summary>
        /// <param name="model" type="OS_LabOrderModel">model</param>
        public string previewLabOrder(OS_LabOrderModel model)
        {
            try
            {
                BLObject<byte[]> obj = BLLOrderSetObj.previewLabOrder(MDVUtility.ToInt64(model.LabOrderId), MDVUtility.ToInt64(model.OrderSetId), MDVUtility.ToStr(model.BarCodeHtml), MDVSession.Current.ImagePath);
                //  obj = null;
                if (obj.Data != null)
                {
                    string saveRequisition = Controls.Patient.Document.Patient_Document.Instance().SavePatientDocument("", MDVUtility.ToInt64(model.OrderSetId), null, null, null, null, null, "Lab Order", MDVUtility.ToInt64(model.LabOrderId), obj.Data);
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


        /// <summary>
        /// Method Name: getJSONofList
        /// Author Name: Azeem Raza Tayyab
        /// Created Date: 13-Jan-2017
        /// Description: This function will convert List objects to JSON
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static string getJSONofList(HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> list)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);

        }

        /// <summary>
        /// Method Name: insertUpdateFavListSetting
        /// Author : Azeem Raza Tayyab
        /// Created Date: 13-Jan-2017
        /// Description: Inserts/updates favorite list setting
        /// </summary>
        /// <param name="isFavListOpened" type="bool">isFavListOpened</param>
        public static string insertUpdateFavListSetting(string favListNames)
        {
            try
            {

                DSUsers dsDefaultsetting = new DSUsers();

                DSUsers dsUser = new DSUsers();
                BLObject<DSUsers> objUser = null;
                BLObject<DSUsers> obj = null;

                objUser = new BLLAdminSecurity().LoadEntityUserOption(ref dsUser, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), MDVSession.Current.EntityId);
                dsDefaultsetting = objUser.Data;

                if (dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows.Count == 0)
                {
                    DSUsers.EntityUserOptionRow dr = dsDefaultsetting.EntityUserOption.NewEntityUserOptionRow();
                    dr.UserId = MDVSession.Current.AppUserId;
                    dr.EntityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
                    dr.EntityRegCode = MDVSession.Current.EntityRegCode;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.IsDefault = false;
                    dr.IsActive = true;
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.FavListNames = favListNames;

                    dsDefaultsetting.EntityUserOption.AddEntityUserOptionRow(dr);
                    obj = new BLLAdminSecurity().InsertDefaultSettings(dsDefaultsetting);

                }
                else
                {
                    foreach (DSUsers.EntityUserOptionRow dr in dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows)
                    {
                        dr.UserId = MDVSession.Current.AppUserId;
                        dr.EntityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
                        dr.EntityRegCode = MDVSession.Current.EntityRegCode;
                        // dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.IsDefault = false;
                        dr.IsActive = true;
                        // dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.FavListNames = favListNames;
                    }
                    obj = new BLLAdminSecurity().UpdatDefaultSettings(dsDefaultsetting);
                }
                if (obj != null)
                {
                    DashBoardSetting.SetApplicationConfig(dsDefaultsetting);
                    var response = new
                    {
                        status = favListNames,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = "",
                        Message = ""
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


        /// Method Name: insertUpdateFreeTextSetting
        /// Author : Azeem Raza Tayyab
        /// Created Date: 13-Jan-2017
        /// Description: Inserts/updates Free Text setting
        /// </summary>
        /// <param name="isFavListOpened" type="bool">isFreeText</param>
        public static string insertUpdateFreeTextSetting(string FreeTextNames)
        {
            try
            {

                DSUsers dsDefaultsetting = new DSUsers();

                DSUsers dsUser = new DSUsers();
                BLObject<DSUsers> objUser = null;
                BLObject<DSUsers> obj = null;

                objUser = new BLLAdminSecurity().LoadEntityUserOption(ref dsUser, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), MDVSession.Current.EntityId);
                dsDefaultsetting = objUser.Data;

                if (dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows.Count == 0)
                {
                    DSUsers.EntityUserOptionRow dr = dsDefaultsetting.EntityUserOption.NewEntityUserOptionRow();
                    dr.UserId = MDVSession.Current.AppUserId;
                    dr.EntityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
                    dr.EntityRegCode = MDVSession.Current.EntityRegCode;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.IsDefault = false;
                    dr.IsActive = true;
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.FreeTextICD = FreeTextNames;
                    dsDefaultsetting.EntityUserOption.AddEntityUserOptionRow(dr);
                    obj = new BLLAdminSecurity().InsertDefaultSettings(dsDefaultsetting);

                }
                else
                {
                    foreach (DSUsers.EntityUserOptionRow dr in dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows)
                    {
                        dr.UserId = MDVSession.Current.AppUserId;
                        dr.EntityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
                        dr.EntityRegCode = MDVSession.Current.EntityRegCode;
                        // dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.IsDefault = false;
                        dr.IsActive = true;
                        // dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.FreeTextICD = FreeTextNames;
                    }
                    obj = new BLLAdminSecurity().UpdatDefaultSettings(dsDefaultsetting);
                }
                if (obj != null)
                {
                    DashBoardSetting.SetApplicationConfig(dsDefaultsetting);
                    var response = new
                    {
                        status = FreeTextNames,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = "",
                        Message = ""
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
        #region"Lab Order Test"
        public string insertUpdateLabOrderTest(Int64 LabId, List<object> lstObjects, List<OS_LabOrderQuestionAnswerModel> CPTQuestionsAnswers)
        {
            try
            {
                DSOS_LabOrder dsLabOrder = new DSOS_LabOrder();

                BLObject<DSOS_LabOrder> objLabOrderTest = BLLOrderSetObj.LoadLabOrderTest(MDVUtility.ToInt64(LabId), 0, 0, "1", "2000");
                dsLabOrder = objLabOrderTest.Data;
                List<LabOrderTestModel> lstLabOrder = lstObjects.OfType<LabOrderTestModel>().ToList();

                int id = -1;
                foreach (LabOrderTestModel CurrentModel in lstLabOrder)
                {
                    Int32 currentLabTestId = MDVUtility.ToInt32(CurrentModel.LabOrderTestId);
                    currentLabTestId = currentLabTestId == 0 ? id-- : currentLabTestId;

                    DSOS_LabOrder.OS_LabOrderTestRow RowLabOrderTest = null;
                    if (dsLabOrder == null)
                    {
                        dsLabOrder = new DSOS_LabOrder();
                    }

                    DSOS_LabOrder.OS_LabOrderTestRow[] arrLabOrderTestRows = (DSOS_LabOrder.OS_LabOrderTestRow[])dsLabOrder.OS_LabOrderTest.Select(dsLabOrder.OS_LabOrderTest.LabOrderTestIdColumn.ColumnName + "=" + currentLabTestId);

                    if (arrLabOrderTestRows != null && arrLabOrderTestRows.Length > 0)
                    {
                        RowLabOrderTest = arrLabOrderTestRows[0];
                    }
                    else
                    {
                        RowLabOrderTest = dsLabOrder.OS_LabOrderTest.NewOS_LabOrderTestRow();
                        RowLabOrderTest.LabOrderTestId = currentLabTestId;
                    }

                    if (RowLabOrderTest != null)
                    {
                        RowLabOrderTest.LabOrderId = LabId;
                        if (!string.IsNullOrEmpty(CurrentModel.dtpLabDate))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.TestDateColumn] = MDVUtility.ToDateTime(CurrentModel.dtpLabDate);
                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.TestDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.tpLabTime))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.TestTimeColumn] = MDVUtility.ToStr(CurrentModel.tpLabTime);
                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.TestTimeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTDescription))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.CPTCodeDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTDescription);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.CPTCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.CPTCodeColumn] = MDVUtility.ToStr(CurrentModel.CPTCode);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.CPTCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ClinicalLabOrderDiet))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.DietIdColumn] = MDVUtility.ToInt32(CurrentModel.ClinicalLabOrderDiet);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.DietIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CollectedAt))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.CollectedAtIdColumn] = MDVUtility.ToInt32(CurrentModel.CollectedAt);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.CollectedAtIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.Urgency))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.UrgencyIdColumn] = MDVUtility.ToInt32(CurrentModel.Urgency);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.UrgencyIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.Specimen))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.SpecimenIdColumn] = MDVUtility.ToInt32(CurrentModel.Specimen);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.SpecimenIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlternativeSpecimen))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.AltSpecimenIdColumn] = MDVUtility.ToInt32(CurrentModel.AlternativeSpecimen);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.AltSpecimenIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.PatientInstructions))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.PatientInstructionColumn] = MDVUtility.ToStr(CurrentModel.PatientInstructions);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.PatientInstructionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.VolumeText))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.VolumeLengthColumn] = MDVUtility.ToStr(CurrentModel.VolumeText);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.VolumeLengthColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.VolumeDDL))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.VolumeIdColumn] = MDVUtility.ToStr(CurrentModel.VolumeDDL);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.VolumeIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.FillerInstructions))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.FillerInstructionColumn] = MDVUtility.ToStr(CurrentModel.FillerInstructions);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.FillerInstructionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SampleStorage))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.SampleStorageColumn] = MDVUtility.ToStr(CurrentModel.SampleStorage);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.SampleStorageColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.Modifier))
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.ModifierColumn] = MDVUtility.ToStr(CurrentModel.Modifier);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.OS_LabOrderTest.ModifierColumn] = DBNull.Value;
                        }
                        RowLabOrderTest[dsLabOrder.OS_LabOrderTest.SoapTextColumn] = string.Format("<b>{0} {1}</b> {2} {3} {4}", string.Empty, CurrentModel.LabProcedure, string.IsNullOrWhiteSpace(CurrentModel.Specimen) ? string.Empty : string.Concat("Specimen: ", CurrentModel.Specimen_text), string.IsNullOrWhiteSpace(CurrentModel.VolumeText) ? string.Empty : string.Concat(",Volume: ", CurrentModel.VolumeText), string.IsNullOrWhiteSpace(CurrentModel.Urgency) ? string.Empty : string.Concat(",Urgency: ", CurrentModel.Urgency_text));   
                        RowLabOrderTest.IsActive = true;
                        RowLabOrderTest.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowLabOrderTest.CreatedOn = DateTime.Now;
                        RowLabOrderTest.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowLabOrderTest.ModifiedOn = DateTime.Now;

                        if (arrLabOrderTestRows.Length < 1)
                        {
                            dsLabOrder.OS_LabOrderTest.AddOS_LabOrderTestRow(RowLabOrderTest);
                        }
                    }
                }

                #region Database Insertion/Updation

                BLObject<DSOS_LabOrder> objInsertedLabTest = BLLOrderSetObj.insertUpdateLabOrderTest(dsLabOrder);
                if (objInsertedLabTest.Data != null)
                {
                    string message = "";
                    bool status = false;
                    DSOS_LabOrder dsLabOrderTest2 = objInsertedLabTest.Data;

                    foreach (DSOS_LabOrder.OS_LabOrderTestRow RowLabOrderTest in dsLabOrderTest2.OS_LabOrderTest.Rows)
                    {

                        DSOS_LabOrder dsLabOrderQAnswer = new DSOS_LabOrder();
                        if (CPTQuestionsAnswers != null)
                        {
                            List<OS_LabOrderQuestionAnswerModel> lsttestAnswers = CPTQuestionsAnswers.FindAll(p => p.CPTCode == RowLabOrderTest.CPTCode);

                            if (lsttestAnswers.Count > 0)
                            {
                                BLObject<DSOS_LabOrder> obj = BLLOrderSetObj.LoadLabOrderAOEAnswer(RowLabOrderTest.CPTCode, MDVUtility.ToStr(RowLabOrderTest.LabOrderTestId));
                                dsLabOrderQAnswer = obj.Data;

                                foreach (var quesionAnswer in lsttestAnswers)
                                {
                                    if (quesionAnswer.CPTCode == RowLabOrderTest.CPTCode && !(string.IsNullOrEmpty(quesionAnswer.Answer)))
                                    {
                                        DSOS_LabOrder.OS_LabOrderAOEAnswersRow[] arrLabOrderAOEAnswerRows = (DSOS_LabOrder.OS_LabOrderAOEAnswersRow[])dsLabOrderQAnswer.OS_LabOrderAOEAnswers.Select(dsLabOrderQAnswer.OS_LabOrderAOEAnswers.LabOrderTestIdColumn.ColumnName + "=" + RowLabOrderTest.LabOrderTestId + " and " + dsLabOrderQAnswer.OS_LabOrderAOEAnswers.QuestionColumn.ColumnName + " = " + MDVUtility.ToLINQFormatString(quesionAnswer.Question));
                                        quesionAnswer.LabOrderTestId = RowLabOrderTest.LabOrderTestId;
                                        insertUpdateLabOrderAOEAnswers(quesionAnswer, dsLabOrderQAnswer, arrLabOrderAOEAnswerRows);
                                    }

                                }
                            }
                        }
                        BLObject<DSOS_LabOrder> objUpdate = new BLObject<DSOS_LabOrder>();
                        if (dsLabOrderQAnswer.OS_LabOrderAOEAnswers.Rows.Count > 0)
                        {
                            objUpdate = BLLOrderSetObj.insertUpdateLabOrderAOEAnswers(dsLabOrderQAnswer);
                        }

                        if (objUpdate.Data != null)
                        {

                            message = Common.AppPrivileges.Save_Message;
                            status = true;
                        }
                        else
                        {
                            status = false;
                            message = objUpdate.Message;

                        }
                    }

                    var response = new
                    {
                        status = status,
                        message = message

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objInsertedLabTest.Message
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

        public string deleteLabOrderTest(string LabOrderTestId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLOrderSetObj.deleteLabOrderTest(LabOrderTestId);
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
        public static DSOS_LabOrder.OS_LabOrderAOEAnswersRow insertUpdateLabOrderAOEAnswers(OS_LabOrderQuestionAnswerModel quesAnswerModel, DSOS_LabOrder dsLabOrder, DSOS_LabOrder.OS_LabOrderAOEAnswersRow[] arrLabOrderAOEAnswerRows)
        {
            DSOS_LabOrder.OS_LabOrderAOEAnswersRow dr = null;
            try
            {


                bool isNewRecord = false;


                string message = string.Empty;
                //A safety check

                if (arrLabOrderAOEAnswerRows.Length > 0)
                {
                    dr = arrLabOrderAOEAnswerRows[0];
                    message = Common.AppPrivileges.Update_Message;

                }
                else
                {
                    dr = dsLabOrder.OS_LabOrderAOEAnswers.NewOS_LabOrderAOEAnswersRow();
                    dr.LabOrderTestId = quesAnswerModel.LabOrderTestId;

                    message = Common.AppPrivileges.Save_Message;
                    // dr. = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    // dr.CreatedOn = DateTime.Now;
                    isNewRecord = true;
                }


                if (dr != null)
                {
                    dr.TestCode = quesAnswerModel.CPTCode;

                    dr.Question = quesAnswerModel.Question;
                    dr.Answer = quesAnswerModel.Answer;

                    if (isNewRecord)
                    {
                        dsLabOrder.OS_LabOrderAOEAnswers.AddOS_LabOrderAOEAnswersRow(dr);
                    }
                }
                return dr;
                #region Database Insertion/Updation



                #endregion
            }
            catch (Exception ex)
            {
                //var response = new
                //{
                //    status = false,
                //    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                //};
                //return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                return dr;
            }

        }

        #endregion
        public string loadLabOrderDashBorad(OS_LabOrderModel model)
        {
            try
            {

                DSOS_LabOrder dsLabOrder = null;
                BLObject<DSOS_LabOrder> obj;

                DSOS_LabOrder dsLabTest = new DSOS_LabOrder();

                obj = BLLOrderSetObj.LoadLabOrder(MDVUtility.ToInt64(model.OrderSetId), model.PageNumber, model.RowsPerPage);
                dsLabOrder = obj.Data;
                List<OS_LabOrderModel> labOrders = new List<OS_LabOrderModel>();
                if (dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows.Count > 0)
                {

                    foreach (DSOS_LabOrder.OS_LabOrderRow dr in dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows)
                    {
                        OS_LabOrderModel labOrderModel = new OS_LabOrderModel();

                        labOrderModel.LabOrderId = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.LabOrderIdColumn.ColumnName]);
                        labOrderModel.OrderDate = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.OrderDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsLabOrder.OS_LabOrder.OrderDateColumn.ColumnName]).ToShortDateString();
                        labOrderModel.AssigneeId = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.AssigneeIdColumn.ColumnName]);
                        labOrderModel.Facility = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.FacilityColumn.ColumnName]);
                        labOrderModel.FacilityId = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.FacilityIdColumn.ColumnName]);
                        labOrderModel.Provider = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.ProviderColumn.ColumnName]);
                        labOrderModel.ProviderId = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.ProviderIdColumn.ColumnName]);
                        labOrderModel.OrderTime = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.OrderTimeColumn.ColumnName]);
                        labOrderModel.bResultExists = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.bResultExistsColumn.ColumnName]);
                        labOrderModel.bResultAcknowledged = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.bResultAcknowledgedColumn.ColumnName]);
                        labOrderModel.Test = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.TestColumn.ColumnName]);
                        labOrderModel.Status = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.StatusColumn.ColumnName]);
                        labOrderModel.LabName = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.LabNameColumn.ColumnName]);
                        labOrderModel.OrderSetId = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.OrderSetIdColumn.ColumnName]);
                        labOrderModel.OrderNo = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.OrderNoColumn.ColumnName]);
                        labOrderModel.AssigneeName = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.AssigneeNameColumn.ColumnName]);
                        //  labOrderModel.PatientName = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.PatientNameColumn.ColumnName]);
                        //labOrderModel.IsNoteLinked = MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.IsNoteLinkedColumn.ColumnName]);

                        BLObject<DSOS_LabOrder> objTest = BLLOrderSetObj.LoadLabOrderTest(MDVUtility.ToInt64(labOrderModel.LabOrderId), 0, 0, "1", "15");
                        dsLabTest = objTest.Data;

                        foreach (DataRow drLabTest in dsLabTest.Tables[dsLabTest.OS_LabOrderTest.TableName].Rows)
                        {
                            OS_LabOrderTestModel testModel = new OS_LabOrderTestModel();
                            string LabOrderTestId = MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.LabOrderTestIdColumn.ColumnName]);
                            string LabOrderTestCPTCode = MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.CPTCodeColumn.ColumnName]);
                            string LabOrdetTestCPTDescription = MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.CPTCodeDescriptionColumn.ColumnName]);
                            string LabOrderModifier = MDVUtility.ToStr(drLabTest[dsLabTest.OS_LabOrderTest.ModifierColumn.ColumnName]);

                            //Fill labOrderTestModel
                            testModel.LabOrderTestId = LabOrderTestId;
                            testModel.CPTCode = LabOrderTestCPTCode;
                            testModel.CPTDescription = LabOrdetTestCPTDescription;
                            testModel.Modifier = LabOrderModifier;
                            labOrderModel.LabOrderTests.Add(testModel);
                        }
                        labOrders.Add(labOrderModel);
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabOrderFill_JSON = js.Serialize(labOrders),
                        LabOrderCount = dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows.Count,
                        LabLoad_JSON = MDVUtility.JSON_DataTable(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName]),
                        iTotalDisplayRecords = (dsLabOrder.OS_LabOrder.Rows.Count > 0) ? dsLabOrder.OS_LabOrder.Rows[0][dsLabOrder.OS_LabOrder.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabOrderCount = 0,
                        LabOrderFill_JSON = "[]",
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

        public string getQuestionsAnswersByCPTCode(OS_LabOrderQuestionAnswerModel model)
        {
            try
            {

                DSOS_LabOrder dsLab = null;
                BLObject<DSOS_LabOrder> obj;

                obj = BLLOrderSetObj.LoadLabOrderAOE(model.CPTCode, model.LabOrderTestId);
                dsLab = obj.Data;

                if (dsLab.Tables[dsLab.OS_LabOrderAOE.TableName].Rows.Count > 0)
                {

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabOrderAOE_Fill_JSON = dsLab.Tables[dsLab.OS_LabOrderAOE.TableName] != null ? MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.OS_LabOrderAOE.TableName]) : "[]",
                        LabOrderAOE_Count = dsLab.Tables[dsLab.OS_LabOrderAOE.TableName] != null ? dsLab.Tables[dsLab.OS_LabOrderAOE.TableName].Rows.Count : 0,
                        LabOrderAOE_Answers_Fill_JSON = dsLab.Tables[dsLab.OS_LabOrderAOEAnswers.TableName] != null ? MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.OS_LabOrderAOEAnswers.TableName]) : "[]",
                        LabOrderAOE_Answers_Count = dsLab.Tables[dsLab.OS_LabOrderAOEAnswers.TableName] != null ? dsLab.Tables[dsLab.OS_LabOrderAOEAnswers.TableName].Rows.Count : 0,
                        // LabLoad_JSON = MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.LabOrder.TableName]),
                        //  iTotalDisplayRecords = (dsLab.LabOrder.Rows.Count > 0) ? dsLab.LabOrder.Rows[0][dsLab.LabOrder.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabOrderAOE_Fill_JSON = "[]",
                        LabOrderAOE_Count = 0
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
        public string saveLabOrderProblem(List<OS_LabOrderProblemModel> model)
        {
            try
            {
                DSOS_LabOrder dsLabOrder = new DSOS_LabOrder();
                Int64 LabOrderId = -1;
                foreach (var m in model)
                {
                    DSOS_LabOrder.OS_LabOrderProblemRow dr = dsLabOrder.OS_LabOrderProblem.NewOS_LabOrderProblemRow();
                    dr.LabOrderId = MDVUtility.ToInt64(m.LabOrderId);
                    dr.ProblemId = MDVUtility.ToInt32(m.ProblemId);
                    dr.Comments = string.Empty;
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dsLabOrder.OS_LabOrderProblem.AddOS_LabOrderProblemRow(dr);
                    LabOrderId = MDVUtility.ToInt64(m.LabOrderId);
                }
                #region Database Insertion
                BLObject<DSOS_LabOrder> obj = BLLOrderSetObj.InsertUpdateLabOrderProblems(dsLabOrder);
                dsLabOrder = obj.Data;

                if (obj.Data != null)
                {

                    if (dsLabOrder.Tables[dsLabOrder.OS_LabOrderProblem.TableName].Rows.Count > 0)
                    {
                        LabOrderId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.OS_LabOrderProblem.TableName].Rows[0][dsLabOrder.OS_LabOrderProblem.LabOrderIdColumn.ColumnName]);

                    }

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        LabOrderId = LabOrderId,
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
    }
}
