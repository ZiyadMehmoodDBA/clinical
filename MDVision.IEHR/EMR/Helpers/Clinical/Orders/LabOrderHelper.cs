/* Author:  Muhammad Arshad
 * Created Date: 15/03/2016
 * OverView: Created to handel Lab Order
 */
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using MDVision.Business.BCommon;
using System.Data;

using MDVision.IEHR.Controls.DashBoard;
using MDVision.Model.Lookups;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using System.Web.Script.Serialization;
using MDVision.IEHR.EMR.Helpers.Clinical.Requisitions;
using System.Threading.Tasks;
using System.Text;
using MDVision.Model.Clinical.Procedures;


namespace MDVision.IEHR.EMR.Helpers.Clinical.Orders
{
    public class LabOrderHelper
    {
        private BLLPatient BLLPatientObj = null;
        private static BLLClinical BLLClinicalObj = null;
        private LabOrderResultRequisitions RequisitionsObj = null;
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;
        public LabOrderHelper()
        {
            BLLPatientObj = new BLLPatient();
            BLLClinicalObj = new BLLClinical();
            RequisitionsObj = new LabOrderResultRequisitions();
            BLLAdminSecurityObj = new BLLAdminSecurity();
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        private static LabOrderHelper _instance = null;
        public static LabOrderHelper Instance()
        {
            if (_instance == null)
                _instance = new LabOrderHelper();
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

                BLObject<string> result = BLLClinicalObj.SaveABNAgainstTest(id, isABN);

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

        public string fillFavGroupLabOrder(LabOrderModel model)
        {
            try
            {
                //System.Diagnostics.Debug.Write("Start Time of Helper fillLabOrder = " + DateTime.Now);
                List<BarCodeLabelPrint> bclp = new List<BarCodeLabelPrint>();
                model.LabOrderIDs = model.LabOrderIDs.Trim(new Char[] { ',' });
                string[] LabOrderIds = model.LabOrderIDs.Split(',');
                foreach (string LabOrderId in LabOrderIds)
                {
                    BLObject<DSLabOrder> obj = BLLClinicalObj.LoadLabOrder(MDVUtility.ToInt64(LabOrderId), 0, 0, model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", 0, "1", "");
                    if (obj.Data.LabOrder.Count > 0)
                    {
                        bclp.Add(new BarCodeLabelPrint()
                        {
                            ClientNo = MDVUtility.ToStr(obj.Data.LabOrder.Rows[0][obj.Data.LabOrder.ClientNoColumn.ColumnName]),
                            OrderNo = MDVUtility.ToStr(obj.Data.LabOrder.Rows[0][obj.Data.LabOrder.OrderNoColumn.ColumnName]),
                            FullName = MDVUtility.ToStr(obj.Data.LabOrder.Rows[0][obj.Data.LabOrder.FullNameColumn.ColumnName]),
                            LabName = MDVUtility.ToStr(obj.Data.LabOrder.Rows[0][obj.Data.LabOrder.LabNameColumn.ColumnName]),
                            PatientDOB = MDVUtility.ToStr(obj.Data.LabOrder.Rows[0][obj.Data.LabOrder.PatientDOBColumn.ColumnName]),
                            TestName = MDVUtility.ToStr(obj.Data.LabOrder.Rows[0][obj.Data.LabOrder.TestColumn.ColumnName]),
                            TxtNoOfPrintName = ""
                        });
                    }
                }


                if (bclp.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabOderFill_JSON = bclp

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
        //OverView: This function will handle fill of Lab Order


        public string fillLabOrder(LabOrderModel model)
        {
            try
            {
                //System.Diagnostics.Debug.Write("Start Time of Helper fillLabOrder = " + DateTime.Now);
                DSLabOrder dsLabOrder = null, dsLabProblems = null;
                BLObject<DSLabOrder> obj = BLLClinicalObj.LoadLabOrder(MDVUtility.ToInt64(model.LabOrderId), MDVUtility.ToInt64(model.PatientId), 0, model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", 0, "1", "");
                dsLabOrder = obj.Data;
                if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
                {

                    if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0];
                        var LabOrderKeyVlaues = new Dictionary<string, string>
                        {

                            { "LabOderDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.OrderDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsLabOrder.LabOrder.OrderDateColumn.ColumnName]).ToShortDateString()},
                            { "Assignee",  MDVUtility.ToStr(dr[dsLabOrder.LabOrder.AssigneeIdColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.FacilityColumn.ColumnName])},
                            { "hfFacility", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.FacilityIdColumn.ColumnName])},
                            { "Provider", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.ProviderColumn.ColumnName])},
                            { "hfProvider", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.ProviderIdColumn.ColumnName])},
                            { "txtLabOderDate", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.OrderTimeColumn.ColumnName])},
                            { "bResultExists", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.bResultExistsColumn.ColumnName])},
                            { "bResultAcknowledged", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.bResultAcknowledgedColumn.ColumnName])},
                            { "ClientNo", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.ClientNoColumn.ColumnName])},
                            { "FullName", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.FullNameColumn.ColumnName])},
                            //{ "GuarantorName", MDVUtility.ToStr(dr[dsFamilyHx.FamilyHx.SoapTextColumn.ColumnName])}
                            { "PatientDOB", MDVUtility.ToStr(dr[dsLabOrder.LabOrder.PatientDOBColumn.ColumnName]) }
                        };
                        //Start Farooq Ahmad 21/3/2016 load the Lab order problems
                        if (MDVUtility.ToInt32(model.LabOrderId) > 0)
                        {
                            obj = BLLClinicalObj.LoadLabOrderProblems(MDVUtility.ToInt32(model.LabOrderId), MDVUtility.ToInt64(model.PatientId), 1, 2000);
                            dsLabProblems = obj.Data;
                        }

                        List<Dictionary<string, string>> lstLabTest = new List<Dictionary<string, string>>();

                        if (MDVUtility.ToInt64(model.LabOrderId) > 0)
                        {
                            DSLabOrder dsLabTest = new DSLabOrder();
                            BLObject<DSLabOrder> objTest = BLLClinicalObj.LoadLabOrderTest(MDVUtility.ToInt64(model.LabOrderId), 0, MDVUtility.ToInt64(model.PatientId), "1", "2000");
                            dsLabTest = objTest.Data;

                            foreach (DataRow drLabTest in dsLabTest.Tables[dsLabTest.LabOrderTest.TableName].Rows)
                            {
                                string LabTestId = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.LabOrderTestIdColumn.ColumnName]);
                                var LabTestValues = new Dictionary<string, string>
                            {

                                {"LabOrderTestId", LabTestId},
                                //{"LabOrderTestId", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.LabOrderTestIdColumn.ColumnName])},
                                {"LabDate", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.TestDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drLabTest[dsLabTest.LabOrderTest.TestDateColumn.ColumnName]).ToShortDateString():""},
                                {"LabTime", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.TestTimeColumn.ColumnName])},
                                //{ "LabProcedure2",MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeDescriptionColumn.ColumnName])},
                                { "CollectedAt", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CollectedAtIdColumn.ColumnName])},
                                { "Urgency", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.UrgencyIdColumn.ColumnName])},
                                { "Specimen", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.SpecimenIdColumn.ColumnName])},
                                { "SpecimenSource", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.SpecimenSourceIdColumn.ColumnName])},
                                { "Organism", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.OrganismIdColumn.ColumnName])},
                                { "AlternativeSpecimen", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.AltSpecimenIdColumn.ColumnName])},
                                { "AlternativeSpecimenSource", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.AltSpecimenSourceIdColumn.ColumnName])},
                                { "Antimicrobials", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.AntimicrobialIdsColumn.ColumnName])},
                                { "PatientInstructions", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.PatientInstructionColumn.ColumnName])},
                                { "VolumeText", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.VolumeLengthColumn.ColumnName])},
                                { "CPTCode", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeColumn.ColumnName])},
                                {"CPTDescription",MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeDescriptionColumn.ColumnName])},
                                {"SampleStorage",MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.SampleStorageColumn.ColumnName])},
                                { "ClinicalLabOrderDiet", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.DietIdColumn.ColumnName])},
                                { "VolumeDDL", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.UnitColumn.ColumnName])},
                                { "FillerInstructions", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.FillerInstructionColumn.ColumnName])},
                                { "AOEExists", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.AOEExistsColumn.ColumnName])},
                                { "AnswerExists", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.AnswerExistsColumn.ColumnName])},
                                { "AOEs", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.AOEsColumn.ColumnName])},
                                { "TestTypeId", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.TestTypeIdColumn.ColumnName])},
                                { "Modifier", MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.ModifierColumn.ColumnName])}
                            };
                                lstLabTest.Add(LabTestValues);
                            }

                        }

                        if (dsLabProblems == null)
                            dsLabProblems = new DSLabOrder();
                        //End Farooq Ahmad 21/3/2016 load the Lab order problems
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            LabFill_JSON = js.Serialize(LabOrderKeyVlaues),
                            //obaccoHxFill_JSON = js.Serialize(lstTobaccoHx),
                            LabOderFill_JSON = MDVUtility.JSON_DataTable(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName]),
                            //Start Farooq Ahmad 21/3/2016 load the Lab order problems
                            LabOrderProblems_JSON = MDVUtility.JSON_DataTable(dsLabProblems.Tables[dsLabProblems.LabOrderProblem.TableName]),
                            //End Farooq Ahmad 21/3/2016 load the Lab order problems
                            LabOrderTest_JSON = js.Serialize(lstLabTest),

                        };
                        //System.Diagnostics.Debug.Write("\n End Time of Helper fillLabOrder = " + DateTime.Now);
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            LabOderFill_JSON = MDVUtility.JSON_DataTable(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName])

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

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
        //OverView: This function will save Lab Order
        [Obsolete]
        public string saveLabOrder_Obsolete(LabOrderModel model)
        {
            try
            {
                DSLabOrder dsLabOrder = new DSLabOrder();
                DSLabOrder.LabOrderRow dr = dsLabOrder.LabOrder.NewLabOrderRow();

                dr.LabOrderId = MDVUtility.ToInt64(model.LabOrderId);
                dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                dr.LabId = MDVUtility.ToInt64(model.LabId);
                dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
                dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);

                //if (!string.IsNullOrEmpty(model.AssigneeId))
                //    dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
                //else
                //    dr[dsLabOrder.LabOrder.AssigneeIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.OderDate))
                //    dr.OrderDate = MDVUtility.ToDateTime(model.OderDate);
                //else
                //    dr[dsLabOrder.LabOrder.OrderDateColumn] = DBNull.Value;


                //if (!string.IsNullOrEmpty(model.LabOderTime))
                //    dr.OrderTime = MDVUtility.ToStr(model.LabOderTime);
                //else
                //    dr[dsLabOrder.LabOrder.OrderTimeColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.LabBillingTypeId))
                //    dr.BillingTypeId = MDVUtility.ToInt64(model.LabBillingTypeId);
                //else
                //    dr[dsLabOrder.LabOrder.BillingTypeIdColumn] = DBNull.Value;


                //if (!string.IsNullOrEmpty(model.LabPrimaryInsuraceId))
                //    dr.PrimaryInsuraceId = MDVUtility.ToInt64(model.LabPrimaryInsuraceId);
                //else
                //    dr[dsLabOrder.LabOrder.PrimaryInsuraceIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.LabSecondaryInsuraceId))
                //    dr.SecondaryInsuraceId = MDVUtility.ToInt64(model.LabSecondaryInsuraceId);
                //else
                //    dr[dsLabOrder.LabOrder.SecondaryInsuraceIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.LabTertiaryInsuraceId))
                //    dr.TertiaryInsuraceId = MDVUtility.ToInt64(model.LabTertiaryInsuraceId);
                //else
                //    dr[dsLabOrder.LabOrder.TertiaryInsuraceIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.LabGuarantorId))
                //    dr.GuarantorId = MDVUtility.ToInt64(model.LabGuarantorId);
                //else
                //    dr[dsLabOrder.LabOrder.GuarantorIdColumn] = DBNull.Value;

                //if (!string.IsNullOrEmpty(model.LabRelationShipId))
                //    dr.RelationShipId = MDVUtility.ToInt32(model.LabRelationShipId);
                //else
                //    dr[dsLabOrder.LabOrder.RelationShipIdColumn] = DBNull.Value;


                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                //dr.SoapText = model.soap

                #region Database Insertion

                dsLabOrder.LabOrder.AddLabOrderRow(dr);
                BLObject<DSLabOrder> obj = BLLClinicalObj.InsertUpdateLabOrder(dsLabOrder);
                dsLabOrder = obj.Data;

                if (obj.Data != null)
                {

                    Int64 radiologicalOrderId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.LabOrderIdColumn.ColumnName]);

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

        [Obsolete]
        public string SaveLabOrder_Obsolete(LabOrderModel model, List<LabOrderTestModel> IstLabObjects)
        {
            // If tests are added from favorite groups then save multi lab orders according to LabId
            if (model.IsFavoriteGroupTypeSave)
            {
                try
                {
                    StringBuilder laborderIDs = new StringBuilder();
                    var LabsArray = model.LabIds.Split(',');
                    foreach (var Lab in LabsArray)
                    {
                        List<LabOrderTestModel> LabOrderTests = new List<LabOrderTestModel>();
                        LabOrderTests = IstLabObjects.Where(t => t.FavoriteGroupTestLabId == Lab).ToList();
                        if (LabOrderTests != null && LabOrderTests.Count > 0)
                        {
                            model.LabId = LabOrderTests[0].FavoriteGroupTestLabId;
                            var a = insertUpdateLabOrder_Obsolete(model, LabOrderTests);
                            var b = a.Split(':')[3];
                            laborderIDs.Append(',');
                            laborderIDs.Append(b.Split(',')[0]);
                        }
                    }
                    var response = new
                    {
                        status = true,
                        LabOrderIDs = laborderIDs.ToString(),
                        InsertType = "FavoriteGroup",
                        Message = "Successfully Saved"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                catch (Exception e)
                {
                    var response = new
                    {
                        status = false,
                        InsertType = "FavoriteGroup",
                        Message = e.Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            else
            {
                return insertUpdateLabOrder_Obsolete(model, IstLabObjects);
            }
        }


        public string SaveLabOrder(LabOrderModel model, List<LabOrderTestModel> IstLabObjects)
        {
            try
            {
                var ds = BLLClinicalObj.InsertUpdateLabOrder(model, IstLabObjects);
                var dr = ds.Data.BLLCustomReturn.Rows[0];
                if (model.IsFavoriteGroupTypeSave)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        status = Convert.ToBoolean(dr[ds.Data.BLLCustomReturn.StatusColumn]),
                        LabOrderIDs = Convert.ToString(dr[ds.Data.BLLCustomReturn.LabOrderIDsColumn]),
                        InsertType = "FavoriteGroup",
                        message = Convert.ToString(dr[ds.Data.BLLCustomReturn.MessageColumn])
                    });
                }

                var response = new
                {
                    status = Convert.ToBoolean(dr[ds.Data.BLLCustomReturn.StatusColumn]),
                    radiologicalOrderId = Convert.ToInt64(dr[ds.Data.BLLCustomReturn.LabOrderIdColumn]),
                    orderNo = Convert.ToString(dr[ds.Data.BLLCustomReturn.OrderNoColumn]),
                    message = Convert.ToString(dr[ds.Data.BLLCustomReturn.MessageColumn]),
                    HL7MessageContent = "",
                    labFavList = model.FavListNames

                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = ex.Message
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
        }
        // Author: Abid Ali
        // Created Date: 17/03/2016
        //OverView: This function will update Lab Order
        [Obsolete]
        public string insertUpdateLabOrder_Obsolete(LabOrderModel model, List<LabOrderTestModel> lstLabObjects)
        {
            try
            {
                //System.Diagnostics.Debug.Write("Start Time of Helper InsertUpdateLabOrder = " + DateTime.Now);
                DSLabOrder dsLabOrder = new DSLabOrder();
                DSLabOrder.LabOrderRow dr = null;
                BLObject<DSLabOrder> obj = BLLClinicalObj.LoadLabOrder(MDVUtility.ToInt64(model.LabOrderId), MDVUtility.ToInt64(model.PatientId), 0, model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", 0);
                dsLabOrder = obj.Data;
                bool isNewRecord = false;

                DSLabOrder.LabOrderRow[] arrLabOrderRows = null;
                string message = string.Empty;
                if (obj.Data != null)
                {
                    //A safety check
                    arrLabOrderRows = (DSLabOrder.LabOrderRow[])dsLabOrder.LabOrder.Select(dsLabOrder.LabOrder.LabOrderIdColumn.ColumnName + "=" + model.LabOrderId);
                    if (arrLabOrderRows.Length > 0)
                    {
                        dr = arrLabOrderRows[0];
                        message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        dr = dsLabOrder.LabOrder.NewLabOrderRow();
                        dr.LabOrderId = MDVUtility.ToInt64(model.LabOrderId);
                        dr.IsActive = true;
                        message = Common.AppPrivileges.Save_Message;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        isNewRecord = true;
                        if (!string.IsNullOrEmpty(model.NegationReasonId))
                            dr.NegationReasonId = model.NegationReasonId;
                        else
                            dr[dsLabOrder.LabOrder.NegationReasonIdColumn] = DBNull.Value;
                    }
                }


                if (dr != null)
                {
                    dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                    if (model.LabId != "")
                    {
                        dr.LabId = MDVUtility.ToInt64(model.LabId);
                    }
                    dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);
                    dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);

                    if (!string.IsNullOrEmpty(model.AssigneeId))
                        dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
                    else
                        dr[dsLabOrder.LabOrder.AssigneeIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.OrderDate))
                        dr.OrderDate = MDVUtility.ToDateTime(model.OrderDate);
                    else
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(dr[dsLabOrder.LabOrder.OrderDateColumn])))
                        {
                            dr[dsLabOrder.LabOrder.OrderDateColumn] = DBNull.Value;
                        }
                    }



                    if (!string.IsNullOrEmpty(model.OrderTime))
                        dr.OrderTime = MDVUtility.ToStr(model.OrderTime);
                    else
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(dr[dsLabOrder.LabOrder.OrderTimeColumn])))
                        {
                            dr[dsLabOrder.LabOrder.OrderTimeColumn] = DBNull.Value;
                        }
                    }


                    if (!string.IsNullOrEmpty(model.BillingTypeId))
                        dr.BillingTypeId = MDVUtility.ToInt64(model.BillingTypeId);
                    else
                        dr[dsLabOrder.LabOrder.BillingTypeIdColumn] = DBNull.Value;


                    if (!string.IsNullOrEmpty(model.PrimaryInsuraceId))
                        dr.PrimaryInsuraceId = MDVUtility.ToInt64(model.PrimaryInsuraceId);
                    else
                        dr[dsLabOrder.LabOrder.PrimaryInsuraceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.SecondaryInsuraceId))
                        dr.SecondaryInsuraceId = MDVUtility.ToInt64(model.SecondaryInsuraceId);
                    else
                        dr[dsLabOrder.LabOrder.SecondaryInsuraceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.TertiaryInsuraceId))
                        dr.TertiaryInsuraceId = MDVUtility.ToInt64(model.TertiaryInsuraceId);
                    else
                        dr[dsLabOrder.LabOrder.TertiaryInsuraceIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.GuarantorId))
                        dr.GuarantorId = MDVUtility.ToInt64(model.GuarantorId);
                    else
                        dr[dsLabOrder.LabOrder.GuarantorIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.RelationShipId))
                        dr.RelationShipId = MDVUtility.ToInt32(model.RelationShipId);
                    else
                        dr[dsLabOrder.LabOrder.RelationShipIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.NoteId))
                        dr.NoteId = MDVUtility.ToInt64(model.NoteId);
                    else
                        dr[dsLabOrder.LabOrder.NoteIdColumn] = DBNull.Value;


                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    //Start 22-03-2016 Humaira Yousaf for status
                    dr.Status = model.Status;
                    //End 22-03-2016 Humaira Yousaf for status
                    if (!string.IsNullOrEmpty(model.NegationReasonId))
                        dr.NegationReasonId = model.NegationReasonId;
                    else
                        dr[dsLabOrder.LabOrder.NegationReasonIdColumn] = DBNull.Value;

                    if (isNewRecord)
                    {
                        dsLabOrder.LabOrder.AddLabOrderRow(dr);
                    }
                }


                #region Database Insertion/Updation

                BLObject<DSLabOrder> objUpdate = BLLClinicalObj.InsertUpdateLabOrder(dsLabOrder);
                //Start 31-05-2016 Humaira Yousaf to save favorite list setting
                dsLabOrder = objUpdate.Data;
                //if (objUpdate.Data != null)
                //{
                //    string res = insertUpdateFavListSetting(model.FavListNames);
                //}
                //End 31-05-2016 Humaira Yousaf to save favorite list setting

                if (obj.Data != null)
                {

                    Int64 LabOrderId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.LabOrderIdColumn.ColumnName]);
                    string OrderNo = MDVUtility.ToStr(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.OrderNoColumn.ColumnName]);


                    if (model.LabOrderProblemList != null)
                    {
                        model.LabOrderProblemList.ForEach(cc => cc.LabOrderId = MDVUtility.ToStr(LabOrderId));
                        BLLClinicalObj.DeleteLabOrderProblems(MDVUtility.ToStr(LabOrderId), "");

                        var id = saveLabOrderProblem_Obsolete(model.LabOrderProblemList);

                    }

                    insertUpdateLabOrderTest_Obsolete(LabOrderId, lstLabObjects, model.CPTCodeQuestionsAnswers);

                    //System.Diagnostics.Debug.Write("\n End Time of Helper InsertUpdateLabOrder = " + DateTime.Now);

                    #region 'Azhar Created HL7 Message Call Region'
                    string HL7MessageContent = string.Empty;
                    if (model.Status.Equals("Transmitted"))
                    {
                        //HL7MessageContent = ClinicalHL7Creation.Instance().CreateHL7LabOrder(LabOrderId, MDVUtility.ToInt64(model.PatientId));
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
        public string updateLabOrder(LabOrderModel model)
        {
            try
            {
                DSLabOrder dsLabOrder = new DSLabOrder();
                BLObject<DSLabOrder> obj = BLLClinicalObj.LoadLabOrder(MDVUtility.ToInt64(model.LabOrderId), MDVUtility.ToInt64(model.PatientId), 0, model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", 0);
                dsLabOrder = obj.Data;
                foreach (DSLabOrder.LabOrderRow dr in dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows)
                {
                    dr.LabOrderId = MDVUtility.ToInt64(model.LabOrderId);
                    dr.PatientId = MDVUtility.ToInt64(model.PatientId);
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
                    if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
                    {
                        BLObject<DSLabOrder> objUpdate = BLLClinicalObj.InsertUpdateLabOrder(dsLabOrder);

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
        [Obsolete]
        public string saveLabOrderProblem_Obsolete(List<LabOrderProblemModel> model)
        {
            try
            {
                DSLabOrder dsLabOrder = new DSLabOrder();
                Int64 LabOrderId = -1;
                foreach (var m in model)
                {
                    DSLabOrder.LabOrderProblemRow dr = dsLabOrder.LabOrderProblem.NewLabOrderProblemRow();
                    dr.LabOrderId = MDVUtility.ToInt64(m.LabOrderId);
                    dr.ProblemId = MDVUtility.ToInt32(m.ProblemId);
                    dr.Comments = string.Empty;
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dsLabOrder.LabOrderProblem.AddLabOrderProblemRow(dr);
                    LabOrderId = MDVUtility.ToInt64(m.LabOrderId);
                }
                #region Database Insertion
                BLObject<DSLabOrder> obj = BLLClinicalObj.InsertUpdateLabOrderProblems(dsLabOrder, "");
                dsLabOrder = obj.Data;

                if (obj.Data != null)
                {

                    if (dsLabOrder.Tables[dsLabOrder.LabOrderProblem.TableName].Rows.Count > 0)
                    {
                        LabOrderId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.LabOrderProblem.TableName].Rows[0][dsLabOrder.LabOrderProblem.LabOrderIdColumn.ColumnName]);

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
        /// Author: Humaira Yousaf
        /// Created Date: 17-03-2016
        /// Description: Loads Lab Orders
        /// </summary>
        /// <param name="model" type="LabOrderModel">LabOrderModel model containing data</param>
        public string loadLabOrder(LabOrderModel model)
        {
            try
            {

                DSLabOrder dsLab = null;
                BLObject<DSLabOrder> obj;

                obj = BLLClinicalObj.LoadLabOrder(MDVUtility.ToInt64(model.LabOrderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), model.PageNumber, model.RowsPerPage, MDVUtility.ToStr(model.Test).Replace("- ", ""), model.OrderNo, MDVUtility.ToInt64(model.ProviderId), model.OrderFromDate, model.OrderToDate, model.Status, MDVUtility.ToInt64(model.LabId));
                dsLab = obj.Data;

                if (dsLab.Tables[dsLab.LabOrder.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsLab.Tables[dsLab.LabOrder.TableName].Rows[dsLab.Tables[dsLab.LabOrder.TableName].Rows.Count - 1];



                    var LabOrderkeyValues = new Dictionary<string, string>
                        {
                            { "LabOrderId",  MDVUtility.ToStr(dr[dsLab.LabOrder.LabOrderIdColumn.ColumnName])},
                            { "SoapText", MDVUtility.ToStr(dr[dsLab.LabOrder.SoapTextColumn.ColumnName])}
                        };


                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabOrderFill_JSON = js.Serialize(LabOrderkeyValues),
                        LabOrderCount = dsLab.Tables[dsLab.LabOrder.TableName].Rows.Count,
                        LabLoad_JSON = MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.LabOrder.TableName]),
                        iTotalDisplayRecords = (dsLab.LabOrder.Rows.Count > 0) ? dsLab.LabOrder.Rows[0][dsLab.LabOrder.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
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

        public static string loadLabOrderDashBorad(LabOrderModel model)
        {
            try
            {

                DSLabOrder dsLabOrder = null;
                BLObject<DSLabOrder> obj;

                DSLabOrder dsLabTest = new DSLabOrder();
                DSLabResult dsLabResult = new DSLabResult();
                DSLabResult dsLabResultDetail = new DSLabResult();

                obj = BLLClinicalObj.LoadLabOrder(MDVUtility.ToInt64(model.LabOrderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), model.PageNumber, model.RowsPerPage, MDVUtility.ToStr(model.Test).Replace("- ", ""), model.OrderNo, MDVUtility.ToInt64(model.ProviderId), model.OrderFromDate, model.OrderToDate, model.Status, MDVUtility.ToInt64(model.LabId));
                dsLabOrder = obj.Data;
                List<LabOrderModel> labOrders = new List<LabOrderModel>();
                if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
                {
                    SharedVariable sharedVariable = SharedVariable.GetSharedVariable();
                    //Stopwatch sw = Stopwatch.StartNew();
                   // Parallel.ForEach(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows, (DSLabOrder.LabOrderRow dr) =>
                    Parallel.ForEach(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.OfType<DSLabOrder.LabOrderRow>(),
                        new ParallelOptions { MaxDegreeOfParallelism = 3 },
                        (dr) =>
                    {
                        LabOrderModel labOrderModel = new LabOrderModel();

                        labOrderModel.LabOrderId = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.LabOrderIdColumn.ColumnName]);
                        labOrderModel.OrderDate = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.OrderDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsLabOrder.LabOrder.OrderDateColumn.ColumnName]).ToShortDateString();
                        labOrderModel.AssigneeId = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.AssigneeIdColumn.ColumnName]);
                        labOrderModel.Facility = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.FacilityColumn.ColumnName]);
                        labOrderModel.FacilityId = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.FacilityIdColumn.ColumnName]);
                        labOrderModel.Provider = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.ProviderColumn.ColumnName]);
                        labOrderModel.ProviderId = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.ProviderIdColumn.ColumnName]);
                        labOrderModel.OrderTime = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.OrderTimeColumn.ColumnName]);
                        labOrderModel.bResultExists = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.bResultExistsColumn.ColumnName]);
                        labOrderModel.bResultAcknowledged = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.bResultAcknowledgedColumn.ColumnName]);
                        labOrderModel.Test = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.TestColumn.ColumnName]);
                        labOrderModel.Status = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.StatusColumn.ColumnName]);
                        labOrderModel.LabName = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.LabNameColumn.ColumnName]);
                        labOrderModel.PatientId = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.PatientIdColumn.ColumnName]);
                        labOrderModel.Specimen = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.SpecimenColumn.ColumnName]);
                        labOrderModel.SpecimenSource = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.SpecimenSourceColumn.ColumnName]);
                        labOrderModel.OrderNo = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.OrderNoColumn.ColumnName]);
                        labOrderModel.AssigneeName = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.AssigneeNameColumn.ColumnName]);
                        labOrderModel.PatientName = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.PatientNameColumn.ColumnName]);
                        labOrderModel.IsNoteLinked = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.IsNoteLinkedColumn.ColumnName]);


                        if (labOrderModel.IsNoteLinked == "True")
                        {
                            LabOrderResultLatestNoteModel noteModel = new LabOrderResultLatestNoteModel();
                            noteModel.NoteId = MDVUtility.ToLong(dr[dsLabOrder.LabOrder.LatestNoteIdColumn.ColumnName]);
                            noteModel.NoteStatus = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.LatestNoteStatusColumn.ColumnName]);
                            noteModel.ProviderId = MDVUtility.ToLong(dr[dsLabOrder.LabOrder.LatestNoteProviderIdColumn.ColumnName]);
                            labOrderModel.LabOrderResultLatestNoteModel = noteModel;
                        }

                        Task<BLObject<DSLabOrder>> objTestTask = Task<BLObject<DSLabOrder>>.Factory.StartNew(() => BLLClinicalObj.LoadLabOrderTest(MDVUtility.ToInt64(labOrderModel.LabOrderId), 0, 0, "1", "15", sharedVariable));
                        Task<BLObject<DSLabResult>> objLabOrderResultTask = Task<BLObject<DSLabResult>>.Factory.StartNew(() => BLLClinicalObj.LoadLabResult(0, MDVUtility.ToInt64(labOrderModel.LabOrderId), "1", "15", "", "", 0, "", "", "", "", 0, 0, "", "", false, true, false, sharedVariable));

                        Task.WaitAll(objTestTask, objLabOrderResultTask);
                        dsLabTest = objTestTask.Result.Data;
                        dsLabResult = objLabOrderResultTask.Result.Data;

                        Int64 labResultId = -1;
                        // int testCounter = 0;

                        if (objLabOrderResultTask.Result.Data != null)
                        {
                            if (dsLabResult.LabOrderResult.Rows.Count > 0)
                            {
                                DSLabResult.LabOrderResultRow drLabResult = (DSLabResult.LabOrderResultRow)dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0];
                                labResultId = MDVUtility.ToInt64(drLabResult[dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName]);
                                // comments = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.CommentsColumn.ColumnName]);
                                // remarks = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.RemarksColumn.ColumnName]);
                                //  status = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.StatusColumn.ColumnName]);
                                //  AssigneeId = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.AssigneeIdColumn.ColumnName]);
                                //  Assignee = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.AssignedToColumn.ColumnName]);
                                //  IsSentToPortal = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.IsSentToPortalColumn.ColumnName]);
                                //  IsAknowledged = MDVUtility.ToStr(drLabResult[dsLabResult.LabOrderResult.IsAknowledgedColumn.ColumnName]);
                            }

                        }
                        BLObject<DSLabResult> objLabResultDetail = new BLObject<DSLabResult>();
                        if (labResultId > -1)
                        {
                            //Load LabOrder Result Details
                            objLabResultDetail = BLLClinicalObj.LoadLabResultDetail(0, labResultId, sharedVariable);
                            dsLabResultDetail = objLabResultDetail.Data;
                        }
                        foreach (DataRow drLabTest in dsLabTest.Tables[dsLabTest.LabOrderTest.TableName].Rows)
                        {
                            LabOrderTestModel testModel = new LabOrderTestModel();
                            testModel.LabOrderTestId = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.LabOrderTestIdColumn.ColumnName]);
                            testModel.CPTCode = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeColumn.ColumnName]);
                            string LabOrdetTestCPTDescription = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.CPTCodeDescriptionColumn.ColumnName]);
                            testModel.AOEs = MDVUtility.ToStr(drLabTest[dsLabTest.LabOrderTest.AOEsColumn.ColumnName]);
                            //Fill labOrderTestModel
                            //testModel.LabOrderTestId = LabOrderTestId;
                            //testModel.CPTCode = LabOrderTestCPTCode;
                            testModel.CPTDescription = LabOrdetTestCPTDescription;
                            //testModel.AOEs = AOEs;
                            if (labResultId > -1)
                            {
                                if (objLabResultDetail.Data != null)
                                {
                                    foreach (DataRow drLabResultDetail in dsLabResultDetail.Tables[dsLabResultDetail.LabOrderResultDetail.TableName].Rows)
                                    {
                                        LabOrderResultDetailModel child = new LabOrderResultDetailModel();
                                        var CptCode = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.CPTCodeColumn]);
                                        var CptDescription = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.CPTCodeDescriptionColumn]);
                                        LabOrdetTestCPTDescription = string.Join("", LabOrdetTestCPTDescription.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                                        CptDescription = string.Join("", CptDescription.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                                        //Push Lab Order Details
                                        if (CptCode == testModel.CPTCode && (LabOrdetTestCPTDescription.ToLower().IndexOf(CptDescription.ToLower()) > -1))
                                        {
                                            child.LabOrderResultDetailId = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LabOrderResultDetailIdColumn]);
                                            child.LOINC = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LOINCColumn]);
                                            child.LOINCDescription = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.LOINCDescriptionColumn]);
                                            child.ObservationDate = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ObservationDateColumn]);
                                            child.Range = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.RangeColumn]);
                                            child.Result = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.ResultColumn]);
                                            child.UoM = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.UoMColumn]);
                                            child.Flag = MDVUtility.ToStr(drLabResultDetail[dsLabResultDetail.LabOrderResultDetail.FlagColumn]);

                                            testModel.LabOrderResultDetails.Add(child);
                                        }
                                    }
                                }
                            }
                            labOrderModel.LabOrderTests.Add(testModel);
                        }
                        labOrders.Add(labOrderModel);
                    });

                    //Debug.WriteLine("for loop complete | " + sw.ElapsedMilliseconds);
                    var response = new
                    {
                        status = true,
                        LabOrderFill_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(labOrders.OrderByDescending(x => x.OrderDate).ThenByDescending(c => c.OrderTime)),
                        LabOrderCount = dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count,
                        LabLoad_JSON = MDVUtility.JSON_DataTable(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName]),
                        iTotalDisplayRecords = (dsLabOrder.LabOrder.Rows.Count > 0) ? dsLabOrder.LabOrder.Rows[0][dsLabOrder.LabOrder.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
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
        /// Author: Abid Ali
        /// Description: attaching Lab order with notes
        /// </summary>
        /// <param name="LabOrderId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string attachLabOrderWithNotes(string LabOrderId, long notesId)
        {
            try
            {
                DSLabOrder dsLabOrder = null;
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
                    BLObject<DSLabOrder> obj = BLLClinicalObj.attachLabOrderWithNotes(LabOrderId, notesId);
                    if (obj.Data != null)
                    {
                        dsLabOrder = obj.Data;
                        var response = new
                        {
                            status = true,
                            LabOrderTotalCount = dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count,
                            LabOrderCount = dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count,
                            LabOrderLoad_JSON = MDVUtility.JSON_DataTable(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName]),
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
        ///   Author: Ahmad Raza
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
                    BLObject<string> obj = BLLClinicalObj.detachLabOrderFromNotes(LabOrderId, notesId);
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

        #region"Lab Order Test"
        [Obsolete]
        public string insertUpdateLabOrderTest_Obsolete(Int64 LabId, List<LabOrderTestModel> lstLabOrder, List<LabOrderQuestionAnswerModel> CPTQuestionsAnswers)
        {
            try
            {
                DSLabOrder dsLabOrder = new DSLabOrder();

                BLObject<DSLabOrder> objLabOrderTest = BLLClinicalObj.LoadLabOrderTest(MDVUtility.ToInt64(LabId), 0, 0, "1", "2000");
                dsLabOrder = objLabOrderTest.Data;
                // List<LabOrderTestModel> lstLabOrder = lstObjects.OfType<LabOrderTestModel>().ToList();

                int id = -1;
                foreach (LabOrderTestModel CurrentModel in lstLabOrder)
                {
                    Int32 currentLabTestId = MDVUtility.ToInt32(CurrentModel.LabOrderTestId);
                    currentLabTestId = currentLabTestId == 0 ? id-- : currentLabTestId;

                    DSLabOrder.LabOrderTestRow RowLabOrderTest = null;
                    if (dsLabOrder == null)
                    {
                        dsLabOrder = new DSLabOrder();
                    }

                    DSLabOrder.LabOrderTestRow[] arrLabOrderTestRows = (DSLabOrder.LabOrderTestRow[])dsLabOrder.LabOrderTest.Select(dsLabOrder.LabOrderTest.LabOrderTestIdColumn.ColumnName + "=" + currentLabTestId);

                    if (arrLabOrderTestRows != null && arrLabOrderTestRows.Length > 0)
                    {
                        RowLabOrderTest = arrLabOrderTestRows[0];
                    }
                    else
                    {
                        RowLabOrderTest = dsLabOrder.LabOrderTest.NewLabOrderTestRow();
                        RowLabOrderTest.LabOrderTestId = currentLabTestId;
                    }

                    if (RowLabOrderTest != null)
                    {
                        RowLabOrderTest.LabOrderId = LabId;
                        if (!string.IsNullOrEmpty(CurrentModel.dtpLabDate))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.TestDateColumn] = MDVUtility.ToDateTime(CurrentModel.dtpLabDate);
                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.TestDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.tpLabTime))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.TestTimeColumn] = MDVUtility.ToStr(CurrentModel.tpLabTime);
                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.TestTimeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTDescription))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.CPTCodeDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTDescription);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.CPTCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.CPTCodeColumn] = MDVUtility.ToStr(CurrentModel.CPTCode);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.CPTCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.ClinicalLabOrderDiet))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.DietIdColumn] = MDVUtility.ToInt32(CurrentModel.ClinicalLabOrderDiet);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.DietIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CollectedAt))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.CollectedAtIdColumn] = MDVUtility.ToInt32(CurrentModel.CollectedAt);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.CollectedAtIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.Urgency))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.UrgencyIdColumn] = MDVUtility.ToInt32(CurrentModel.Urgency);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.UrgencyIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.Specimen))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.SpecimenIdColumn] = MDVUtility.ToInt32(CurrentModel.Specimen);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.SpecimenIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.AlternativeSpecimen))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.AltSpecimenIdColumn] = MDVUtility.ToInt32(CurrentModel.AlternativeSpecimen);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.AltSpecimenIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.PatientInstructions))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.PatientInstructionColumn] = MDVUtility.ToStr(CurrentModel.PatientInstructions);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.PatientInstructionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.VolumeText))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.VolumeLengthColumn] = MDVUtility.ToStr(CurrentModel.VolumeText);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.VolumeLengthColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.VolumeDDL))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.VolumeIdColumn] = MDVUtility.ToStr(CurrentModel.VolumeDDL);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.VolumeIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(CurrentModel.FillerInstructions))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.FillerInstructionColumn] = MDVUtility.ToStr(CurrentModel.FillerInstructions);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.FillerInstructionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.SampleStorage))
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.SampleStorageColumn] = MDVUtility.ToStr(CurrentModel.SampleStorage);

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.SampleStorageColumn] = DBNull.Value;
                        }

                        // Clinical Chemistry has some AOE's that need to be shown
                        if (CurrentModel.CPTCode == "CLINICALCHEM" && CurrentModel.CPTDescription == "Clinical Chemistry")
                        {
                            if (CPTQuestionsAnswers != null)
                            {
                                var RequestedTests = CPTQuestionsAnswers.FirstOrDefault(x => x.Question == "RequestedTests");
                                RowLabOrderTest[dsLabOrder.LabOrderTest.SoapTextColumn] = string.Format("<b>{0} {1} {2} {3} {4}", string.Empty, CurrentModel.LabProcedure + "</b> (" + (MDVUtility.ToStr(RequestedTests.Answer)) + ") ", string.IsNullOrWhiteSpace(CurrentModel.Specimen) ? string.Empty : string.Concat("Specimen: ", CurrentModel.Specimen_text), string.IsNullOrWhiteSpace(CurrentModel.VolumeText) ? string.Empty : string.Concat(",Volume: ", CurrentModel.VolumeText), string.IsNullOrWhiteSpace(CurrentModel.Urgency) ? string.Empty : string.Concat(",Urgency: ", (CurrentModel.Urgency == "1" ? "Normal" : "STAT")));
                            }
                            else
                            {
                                DSLabOrder dsLabOrderTestAOEAnswer = new DSLabOrder();
                                dsLabOrderTestAOEAnswer = new MDVision.DataAccess.DAL.Clinical.DALLabOrder().LoadLabOrderAOEAnswers(CurrentModel.CPTCode, currentLabTestId.ToString());
                                if (dsLabOrderTestAOEAnswer.Tables[dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.TableName].Rows.Count > 0)
                                {
                                    RowLabOrderTest[dsLabOrder.LabOrderTest.SoapTextColumn] = string.Format("<b>{0} {1} {2} {3} {4}", string.Empty, CurrentModel.LabProcedure + "</b> ("
                                        + (MDVUtility.ToStr(dsLabOrderTestAOEAnswer.Tables[dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.TableName].Rows[0][dsLabOrderTestAOEAnswer.LabOrderAOEAnswers.AnswerColumn.ColumnName].ToString())) +
                                        ") ", string.IsNullOrWhiteSpace(CurrentModel.Specimen) ? string.Empty : string.Concat("Specimen: ", CurrentModel.Specimen_text),
                                        string.IsNullOrWhiteSpace(CurrentModel.VolumeText) ? string.Empty : string.Concat(",Volume: ", CurrentModel.VolumeText), string.IsNullOrWhiteSpace(CurrentModel.Urgency) ? string.Empty : string.Concat(",Urgency: ", (CurrentModel.Urgency == "1" ? "Normal" : "STAT")));
                                }
                            }

                        }
                        else
                        {
                            RowLabOrderTest[dsLabOrder.LabOrderTest.SoapTextColumn] = string.Format("<b>{0} {1}</b> {2} {3} {4}", string.Empty, CurrentModel.LabProcedure, string.IsNullOrWhiteSpace(CurrentModel.Specimen) ? string.Empty : string.Concat("Specimen: ", CurrentModel.Specimen_text), string.IsNullOrWhiteSpace(CurrentModel.VolumeText) ? string.Empty : string.Concat(",Volume: ", CurrentModel.VolumeText), string.IsNullOrWhiteSpace(CurrentModel.Urgency) ? string.Empty : string.Concat(",Urgency: ", (CurrentModel.Urgency == "1" ? "Normal" : "STAT")));
                        }
                        RowLabOrderTest.IsActive = true;
                        RowLabOrderTest.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowLabOrderTest.CreatedOn = DateTime.Now;
                        RowLabOrderTest.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowLabOrderTest.ModifiedOn = DateTime.Now;

                        if (arrLabOrderTestRows.Length < 1)
                        {
                            dsLabOrder.LabOrderTest.AddLabOrderTestRow(RowLabOrderTest);
                        }
                    }
                }

                #region Database Insertion/Updation

                BLObject<DSLabOrder> objInsertedLabTest = BLLClinicalObj.insertUpdateLabOrderTest(dsLabOrder, "");
                if (objInsertedLabTest.Data != null)
                {
                    string message = "";
                    bool status = false;
                    DSLabOrder dsLabOrderTest2 = objInsertedLabTest.Data;

                    foreach (DSLabOrder.LabOrderTestRow RowLabOrderTest in dsLabOrderTest2.LabOrderTest.Rows)
                    {

                        DSLabOrder dsLabOrderQAnswer = new DSLabOrder();
                        if (CPTQuestionsAnswers != null)
                        {
                            List<LabOrderQuestionAnswerModel> lsttestAnswers = CPTQuestionsAnswers.FindAll(p => p.CPTCode == RowLabOrderTest.CPTCode);

                            if (lsttestAnswers.Count > 0)
                            {
                                BLObject<DSLabOrder> obj = BLLClinicalObj.LoadLabOrderAOEAnswer(RowLabOrderTest.CPTCode, MDVUtility.ToStr(RowLabOrderTest.LabOrderTestId));
                                dsLabOrderQAnswer = obj.Data;

                                foreach (var quesionAnswer in lsttestAnswers)
                                {
                                    if (quesionAnswer.CPTCode == RowLabOrderTest.CPTCode && !(string.IsNullOrEmpty(quesionAnswer.Answer)))
                                    {
                                        DSLabOrder.LabOrderAOEAnswersRow[] arrLabOrderAOEAnswerRows = (DSLabOrder.LabOrderAOEAnswersRow[])dsLabOrderQAnswer.LabOrderAOEAnswers.Select(dsLabOrderQAnswer.LabOrderAOEAnswers.LabOrderTestIdColumn.ColumnName + "=" + RowLabOrderTest.LabOrderTestId + " and " + dsLabOrderQAnswer.LabOrderAOEAnswers.QuestionColumn.ColumnName + " = " + MDVUtility.ToLINQFormatString(quesionAnswer.Question));
                                        quesionAnswer.LabOrderTestId = RowLabOrderTest.LabOrderTestId;
                                        insertUpdateLabOrderAOEAnswers(quesionAnswer, dsLabOrderQAnswer, arrLabOrderAOEAnswerRows);
                                    }

                                }
                            }
                        }
                        BLObject<DSLabOrder> objUpdate = new BLObject<DSLabOrder>();
                        if (dsLabOrderQAnswer.LabOrderAOEAnswers.Rows.Count > 0)
                        {
                            objUpdate = BLLClinicalObj.insertUpdateLabOrderAOEAnswers(dsLabOrderQAnswer);
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

        public string deleteLabOrderTest(string LabOrderTestId, string patientId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.deleteLabOrderTest(LabOrderTestId, patientId);
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
        /// Method Name: loadOrderingProvider
        /// Author Name: Abid Ali
        /// Created Date: 13-04-2016
        /// Description: This function will handle Load of Provider for Lab Order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string loadOrderingProvider(LabOrderModel model)
        {

            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> list = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();

            DSLabOrder dsLabOrder = null;
            BLObject<DSProfileLookup> objProvider = BLLAdminProfileObj.LookupProvider("true");
            DSProfileLookup dsProvider = objProvider.Data;
            BLObject<DSLabOrder> obj;
            obj = BLLClinicalObj.LoadLabOrder(0, MDVUtility.ToInt64(model.PatientId), 0, "", "", "", "", 0, "", "", "", 0);
            dsLabOrder = obj.Data;
            list.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));
            if (dsLabOrder != null)
            {

                if (dsProvider.Tables[dsProvider.Provider.TableName] != null)
                {
                    DataView view = new DataView(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName]);
                    if (view.Count > 0)
                    {
                        DataTable distinctValues = view.ToTable(true, dsLabOrder.LabOrder.ProviderIdColumn.ColumnName);
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
        /// Author : Ahmad Raza
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
                obj = BLLClinicalObj.DeleteLabOrder(LabOrderId);
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
                        Message = result
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
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
        }


        public string loadPatientGuarantor(LabOrderModel model)
        {
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSPatientProfile dsPatientRelation = null;
                // DSPatientProfile dsRelationShip = null;
                string relationshipName = "";
                Int64 relationshipId = 0;
                BLObject<DSPatientProfile> objpatient = BLLPatientObj.LoadPatientGuarantor(0, MDVUtility.ToInt32(model.PatientId), "1");
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
        /// Author : Humaira Yousaf
        /// Created Date: 23-03-2016
        /// Description: Creates PDF to view Lab Order
        /// </summary>
        /// <param name="model" type="LabOrderModel">model</param>
        public string previewLabOrder(LabOrderModel model)
        {
            try
            {
                BLObject<byte[]> obj = null;
                if (model.LabOrderIDs != null)
                {
                    var IDs = model.LabOrderIDs.Split(',');
                    List<byte[]> lstByteArray = new List<byte[]>();
                    foreach (var item in IDs)
                    {
                        if (item != "")
                        {
                            model.LabOrderId = item;
                            obj = RequisitionsObj.GenerateLabOrderReq(MDVUtility.ToInt64(model.LabOrderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToStr(model.BarCodeHtml), MDVSession.Current.ImagePath);
                            if (obj.Data != null)
                            {
                                lstByteArray.Add(obj.Data);
                                string saveRequisition = Controls.Patient.Document.Patient_Document.Instance().SavePatientDocument("", MDVUtility.ToInt64(model.PatientId), null, null, null, null, null, "Lab Order", MDVUtility.ToInt64(model.LabOrderId), obj.Data);

                            }
                        }
                    }

                    byte[] finalBytes = MDVUtility.CombineMultipleByteArrays(lstByteArray);


                    var response = new
                    {
                        status = true,
                        LabOrderHTML = finalBytes
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    obj = RequisitionsObj.GenerateLabOrderReq(MDVUtility.ToInt64(model.LabOrderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToStr(model.BarCodeHtml), MDVSession.Current.ImagePath);
                    //  obj = null;
                    if (obj.Data != null)
                    {
                        string saveRequisition = Controls.Patient.Document.Patient_Document.Instance().SavePatientDocument("", MDVUtility.ToInt64(model.PatientId), null, null, null, null, null, "Lab Order", MDVUtility.ToInt64(model.LabOrderId), obj.Data,0,0,true);
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
                            status = false,
                            FaceSheetCount = 0,
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

        //Ahsan Nasir
        public string previewLabOrderABN(LabOrderModel model, string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                var json = SearchedfieldsJSON["data"];
                var fields = ser.Deserialize<dynamic>(json);
                object[] TestDesc = fields["Tests"];
                // Using LINQ to convert object type array into a string type array
                string[] Tests = Array.ConvertAll(TestDesc, x => x.ToString());
                BLObject<byte[]> obj = BLLClinicalObj.previewLabOrderABN(MDVUtility.ToInt64(model.LabOrderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToStr(model.BarCodeHtml), MDVSession.Current.ImagePath, Tests);

                if (obj.Data != null)
                {
                    string saveRequisition = Controls.Patient.Document.Patient_Document.Instance().SavePatientDocument("", MDVUtility.ToInt64(model.PatientId), null, null, null, null, null, "Lab Order", MDVUtility.ToInt64(model.LabOrderId), obj.Data);
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

        /// <summary>
        /// Method Name: getOrdersForSoap
        /// Author Name: Ahmad Raza
        /// Created Date: 21-04-2016
        /// Description: This function will getOrdersForSoap
        /// </summary>
        /// <param name="orderIDs"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        internal string getOrdersForSoap(string orderIDs, long patientId, long notesId, long ProviderId)
        {
            try
            {

                DSLabOrder dsOrderSoap = null;
                DSProblemLists dsProblemSoap = null;
                BLObject<DSLabOrder> obj = BLLClinicalObj.loadOrdersForSoap(orderIDs, patientId, ProviderId);
                BLObject<DSProblemLists> objPrb = BLLClinicalObj.attachLabOrdProblemsWithNoteForSoap(orderIDs, notesId);

                dsOrderSoap = obj.Data;
                dsProblemSoap = objPrb.Data;
                if (obj.Data != null)
                {
                    if (dsOrderSoap.Tables[dsOrderSoap.LabOrder.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MedicationSoapCount = dsOrderSoap.Tables[dsOrderSoap.LabOrder.TableName].Rows.Count,
                            MedicationSoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.LabOrder.TableName]),
                            LabOrderTest_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.LabOrderTest.TableName]),
                            LabOrderProblem_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.LabOrderProblem.TableName]),
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
                            MedicationSoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.LabOrder.TableName]),
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



        internal string attachOrdersWithNotes(string medicationID, long notesId)
        {
            try
            {
                DSLabOrder dsOrders = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(medicationID)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<DSLabOrder> obj = BLLClinicalObj.attachLabOrderWithNotes(medicationID, notesId);
                    if (obj.Data != null)
                    {
                        dsOrders = obj.Data;
                        var response = new
                        {
                            status = true,
                            MedicationTotalCount = dsOrders.Tables[dsOrders.LabOrder.TableName].Rows.Count,
                            MedicationCount = dsOrders.Tables[dsOrders.LabOrder.TableName].Rows.Count,
                            MedicationLoad_JSON = MDVUtility.JSON_DataTable(dsOrders.Tables[dsOrders.LabOrder.TableName]),
                            MedicationHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsOrders.Tables[dsOrders.LabOrder.TableName]),
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
        /// Method Name: insertUpdateFavListSetting
        /// Author : Humaira Yousaf
        /// Created Date: 31-05-2016
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
        /// Author : M Ahmad Imran
        /// Created Date: 4-1-2016
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

        public static string getQuestionsAnswersByCPTCode(LabOrderQuestionAnswerModel model)
        {
            try
            {

                DSLabOrder dsLab = null;
                BLObject<DSLabOrder> obj;

                obj = BLLClinicalObj.LoadLabOrderAOE(model.CPTCode, model.LabOrderTestId);
                dsLab = obj.Data;

                if (dsLab.Tables[dsLab.LabOrderAOE.TableName].Rows.Count > 0)
                {

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        LabOrderAOE_Fill_JSON = dsLab.Tables[dsLab.LabOrderAOE.TableName] != null ? MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.LabOrderAOE.TableName]) : "[]",
                        LabOrderAOE_Count = dsLab.Tables[dsLab.LabOrderAOE.TableName] != null ? dsLab.Tables[dsLab.LabOrderAOE.TableName].Rows.Count : 0,
                        LabOrderAOE_Answers_Fill_JSON = dsLab.Tables[dsLab.LabOrderAOEAnswers.TableName] != null ? MDVUtility.JSON_DataTable(dsLab.Tables[dsLab.LabOrderAOEAnswers.TableName]) : "[]",
                        LabOrderAOE_Answers_Count = dsLab.Tables[dsLab.LabOrderAOEAnswers.TableName] != null ? dsLab.Tables[dsLab.LabOrderAOEAnswers.TableName].Rows.Count : 0,
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
        public static DSLabOrder.LabOrderAOEAnswersRow insertUpdateLabOrderAOEAnswers(LabOrderQuestionAnswerModel quesAnswerModel, DSLabOrder dsLabOrder, DSLabOrder.LabOrderAOEAnswersRow[] arrLabOrderAOEAnswerRows)
        {
            DSLabOrder.LabOrderAOEAnswersRow dr = null;
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
                    dr = dsLabOrder.LabOrderAOEAnswers.NewLabOrderAOEAnswersRow();
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
                        dsLabOrder.LabOrderAOEAnswers.AddLabOrderAOEAnswersRow(dr);
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


        public string labOrderChangePatient(long labOrderId, long PatientId)
        {
            try
            {
                string result = null;
                BLObject<string> obj = BLLClinicalObj.labOrderChangePatient(labOrderId, PatientId);
                result = obj.Data;
                if (obj.Data != null)
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

        internal static string LookupLabTestReport(LabOrderModel model)
        {
            try
            {
                BLObject<List<LabTestLookupModel>> obj = BLLClinicalObj.LookupLabTestReport(model.Test);
                List<LabTestLookupModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        labTestCount = modelList.Count,
                        labTestList_JSON = js.Serialize(modelList),
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