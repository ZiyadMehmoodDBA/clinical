/* Author:  Muhammad Arshad
 * Created Date: 17/03/2016
 * OverView: Created to handel Consultation Order
 */
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.CDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.Model.Clinical.OrdersAndResults.ConsultationOrder;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Orders
{
    public class ConsultationOrderHelper
    {
        private BLLClinical BLLClinicalObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;
        public ConsultationOrderHelper()
        {
            BLLClinicalObj = new BLLClinical();
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        private static ConsultationOrderHelper _instance = null;
        public static ConsultationOrderHelper Instance()
        {
            if (_instance == null)
                _instance = new ConsultationOrderHelper();
            return _instance;
        }


        /// <summary>
        /// Module Name: loadConsultationOrder
        /// Author: Humaira Yousaf
        /// Created Date: 17-03-2016
        /// Description: Loads Consultation Orders
        /// </summary>
        /// <param name="model" type="ConsultationOrderModel">ConsultationOrderModel containing data</param>
        public string loadConsultationOrder(ConsultationOrderModel model)
        {
            try
            {
                DSConsultationOrder dsConsultation = null, dsConsultationProblems = null;
                BLObject<DSConsultationOrder> obj;
                //Start 21-03-2016 Humaira Yousaf
                obj = BLLClinicalObj.loadconsultationOrder(MDVUtility.ToInt32(model.ConsultationOrderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToStr(model.CPTCode), model.OrderNo, MDVUtility.ToInt64(model.ProviderId), model.OrderFromDate, model.OrderToDate, model.Status, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "", "", MDVUtility.ToInt64(model.NoteId));
                //End 21-03-2016 Humaira Yousaf
                dsConsultation = obj.Data;
                if (dsConsultation.Tables[dsConsultation.ConsultationOrder.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsConsultation.Tables[dsConsultation.ConsultationOrder.TableName].Rows[dsConsultation.Tables[dsConsultation.ConsultationOrder.TableName].Rows.Count - 1];
                    var ConsultationOrderkeyValues = new Dictionary<string, string>
                        {
                            { "ConsultationOrderId",  MDVUtility.ToStr(dr[dsConsultation.ConsultationOrder.ConsultationOrderIdColumn.ColumnName])},
                            { "SoapText", MDVUtility.ToStr(dr[dsConsultation.ConsultationOrder.SoapTextColumn.ColumnName])}
                        };
                    //Start Farooq Ahmad 21/3/2016 load the consultation order problems
                    if (MDVUtility.ToInt32(model.ConsultationOrderId) > 0)
                    {
                        obj = BLLClinicalObj.LoadConsultationOrderProblems(MDVUtility.ToInt32(model.ConsultationOrderId), MDVUtility.ToInt64(model.PatientId), 1, 2000);
                        dsConsultationProblems = obj.Data;
                    }
                    if (dsConsultationProblems == null)
                        dsConsultationProblems = new DSConsultationOrder();
                    //End Farooq Ahmad 21/3/2016 load the consultation order problems

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ConsultationOrderFill_JSON = js.Serialize(ConsultationOrderkeyValues),
                        consultationOrderCount = dsConsultation.Tables[dsConsultation.ConsultationOrder.TableName].Rows.Count,
                        ConsultationLoad_JSON = MDVUtility.JSON_DataTable(dsConsultation.Tables[dsConsultation.ConsultationOrder.TableName]),
                        //Start Farooq Ahmad 21/3/2016 load the consultation order problems
                        ConsultationOrderProblems_JSON = MDVUtility.JSON_DataTable(dsConsultationProblems.Tables[dsConsultationProblems.ConsultationOrderProblem.TableName]),
                        //End Farooq Ahmad 21/3/2016 load the consultation order problems
                        iTotalDisplayRecords = (dsConsultation.ConsultationOrder.Rows.Count > 0) ? dsConsultation.ConsultationOrder.Rows[0][dsConsultation.ConsultationOrder.RecordCountColumn.ColumnName] : 0,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ConsultationOrderFill_JSON = "[]",
                        //DiseaseFill_JSON = string.Empty,
                        // MedicalHxDiseaseLoad_JSON = MDVUtility.JSON_DataTable(dsConsultation.Tables[dsConsultation.MedicalHx_Disease.TableName]),
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
        /// Module Name: insertUpdateConsultationOrder
        /// Author: Humaira Yousaf
        /// Created Date: 18-03-2016
        /// Description: Inserts/Updates Consultation Order
        /// </summary>
        /// <param name="model" type="ConsultationOrderModel">ConsultationOrderModel containing data</param>
        public string insertUpdateConsultationOrder(ConsultationOrderModel model, List<object> lstConsultationObjects)
        {
            try
            {
                DSConsultationOrder dsConsultation = new DSConsultationOrder();
                //Start 21-03-2016 Humaira Yousaf for search criteria
                BLObject<DSConsultationOrder> obj = BLLClinicalObj.loadconsultationOrder(MDVUtility.ToInt32(model.ConsultationOrderId), MDVUtility.ToInt32(model.PatientId), "", "", 0, null, null, "", 1, 1);
                //End 21-03-2016 Humaira Yousaf for search criteria

                dsConsultation = obj.Data;
                if (obj.Data != null)
                {
                    DSConsultationOrder.ConsultationOrderRow consultationRow = null;
                    DSConsultationOrder.ConsultationOrderRow[] consultationRows = (DSConsultationOrder.ConsultationOrderRow[])dsConsultation.ConsultationOrder.Select(dsConsultation.ConsultationOrder.ConsultationOrderIdColumn + "=" + model.ConsultationOrderId);
                    if (consultationRows.Length > 0)
                    {
                        consultationRow = consultationRows[0];
                    }
                    else
                    {
                        consultationRow = dsConsultation.ConsultationOrder.NewConsultationOrderRow();
                    }
                    if (consultationRow != null)
                    {
                        consultationRow.PatientId = MDVUtility.ToInt64(model.PatientId);
                        consultationRow.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                        consultationRow.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);

                        if (!string.IsNullOrEmpty(model.OrderDate))
                            consultationRow.OrderDate = MDVUtility.ToDateTime(model.OrderDate);
                        else
                            consultationRow[dsConsultation.ConsultationOrder.OrderDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.NoteId))
                            consultationRow.NoteId = MDVUtility.ToInt64(model.NoteId);
                        else
                            consultationRow[dsConsultation.ConsultationOrder.NoteIdColumn] = DBNull.Value;


                        consultationRow.OrderTime = model.OrderTime;
                        consultationRow.IsActive = true;
                        if (consultationRows.Length == 0)
                        {
                            consultationRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            consultationRow.CreatedOn = DateTime.Now;
                        }
                        consultationRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        consultationRow.ModifiedOn = DateTime.Now;
                        consultationRow.SoapText = model.SoapText;
                        //Start 22-03-2016 Humaira Yousaf for status
                        consultationRow.Status = model.Status;
                        //End 22-03-2016 Humaira Yousaf for status

                        if (consultationRows.Length < 1)
                        {
                            dsConsultation.ConsultationOrder.AddConsultationOrderRow(consultationRow);
                        }
                    }

                    #region Database Insertion/Updation
                    BLObject<DSConsultationOrder> objConsultation = BLLClinicalObj.insertUpdateConsultationOrder(dsConsultation, model, lstConsultationObjects);
                    dsConsultation = objConsultation.Data;

                    if (objConsultation.Data != null)
                    {
                        long insertedConsultationId = MDVUtility.ToInt64(dsConsultation.Tables[dsConsultation.ConsultationOrder.TableName].Rows[0][dsConsultation.ConsultationOrder.ConsultationOrderIdColumn.ColumnName]);

                        //model.ConsultationOrderProblemList.ForEach(cc => cc.ConsultationOrderId = MDVUtility.ToStr(insertedConsultationId));
                        //BLLClinicalObj.DeleteConsultationOrderProblems(MDVUtility.ToStr(insertedConsultationId));
                        //var id = saveConsultationOrderProblem(model.ConsultationOrderProblemList);

                        //insertUpdateConsultationOrderTest(insertedConsultationId, lstConsultationObjects);

                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ConsultationOrderId = insertedConsultationId,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objConsultation.Message
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

        // Author: Farooq Ahmad
        // Created Date: 18/03/2016
        //OverView: This function will save Consultation Order Problem
        //public string saveConsultationOrderProblem(List<ConsultationOrderProblemModel> model)
        //{
        //    try
        //    {
        //        DSConsultationOrder dsConsultationOrder = new DSConsultationOrder();

        //        foreach (var m in model)
        //        {
        //            DSConsultationOrder.ConsultationOrderProblemRow dr = dsConsultationOrder.ConsultationOrderProblem.NewConsultationOrderProblemRow();
        //            dr.ConsultationOrderId = MDVUtility.ToInt64(m.ConsultationOrderId);
        //            dr.ProblemId = MDVUtility.ToInt32(m.ProblemId);
        //            dr.Comments = string.Empty;
        //            dr.IsActive = true;
        //            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //            dr.CreatedOn = DateTime.Now;
        //            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //            dr.ModifiedOn = DateTime.Now;
        //            dsConsultationOrder.ConsultationOrderProblem.AddConsultationOrderProblemRow(dr);
        //        }
        //        #region Database Insertion
        //        BLObject<DSConsultationOrder> obj = BLLClinicalObj.InsertUpdateConsultationOrderProblems(dsConsultationOrder);
        //        dsConsultationOrder = obj.Data;

        //        if (obj.Data != null)
        //        {

        //            Int64 ConsultationOrderId = MDVUtility.ToInt64(dsConsultationOrder.Tables[dsConsultationOrder.ConsultationOrderProblem.TableName].Rows[0][dsConsultationOrder.ConsultationOrderProblem.ConsultationOrderIdColumn.ColumnName]);

        //            var response = new
        //            {
        //                status = true,
        //                message = Common.AppPrivileges.Save_Message,
        //                ConsultationOrderId = ConsultationOrderId,
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
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }

        //}

        /// <summary>
        /// Module Name: fillConsultationOrder
        /// Author: Humaira Yousaf
        /// Created Date: 18-03-2016
        /// Description: Fills Consultation Order
        /// </summary>
        /// <param name="model" type="ConsultationOrderModel">ConsultationOrderModel containing data</param>
        public string fillConsultationOrder(ConsultationOrderModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ConsultationOrderId)))
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
                    DSConsultationOrder dsConsultation = null, dsConsultationProblems = null; ;
                    //Start 21-03-2016 Humaira Yousaf
                    BLObject<DSConsultationOrder> obj = BLLClinicalObj.loadconsultationOrder(MDVUtility.ToInt32(model.ConsultationOrderId), MDVUtility.ToInt64(model.PatientId), "", "", 0, null, null, "", 1, 1000, "1", "");
                    //End 21-03-2016 Humaira Yousaf  
                    if (obj.Data != null)
                    {
                        dsConsultation = obj.Data;
                        if (dsConsultation.Tables[dsConsultation.ConsultationOrder.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsConsultation.Tables[dsConsultation.ConsultationOrder.TableName].Rows[0];

                            var keyValues = new Dictionary<string, string>
                            {
                                {"ConsultationOrderId",MDVUtility.ToStr(dr[dsConsultation.ConsultationOrder.ConsultationOrderIdColumn.ColumnName])},
                                { "dtOrderDate", MDVUtility.ToStr(dr[dsConsultation.ConsultationOrder.OrderDateColumn.ColumnName])},
                                { "tmOrderTime", MDVUtility.ToStr(dr[dsConsultation.ConsultationOrder.OrderTimeColumn.ColumnName])},
                                { "txtOrderNumber", MDVUtility.ToStr(dr[dsConsultation.ConsultationOrder.OrderNoColumn.ColumnName])},
                                { "txtProvider", MDVUtility.ToStr(dr[dsConsultation.ConsultationOrder.ProviderNameColumn.ColumnName])},
                                { "hfProvider", MDVUtility.ToStr(dr[dsConsultation.ConsultationOrder.ProviderIdColumn.ColumnName])},
                                { "txtAssignee", MDVUtility.ToStr(dr[dsConsultation.ConsultationOrder.AssigneeNameColumn.ColumnName])},
                                { "hfAssignee", MDVUtility.ToStr(dr[dsConsultation.ConsultationOrder.AssigneeIdColumn.ColumnName])},
                                { "Status", MDVUtility.ToStr(dr[dsConsultation.ConsultationOrder.StatusColumn.ColumnName])},
                            };
                            //Start Farooq Ahmad 21/3/2016 load the consultation order problems
                            if (MDVUtility.ToInt32(model.ConsultationOrderId) > 0)
                            {
                                obj = BLLClinicalObj.LoadConsultationOrderProblems(MDVUtility.ToInt32(model.ConsultationOrderId), MDVUtility.ToInt64(model.PatientId), 1, 2000);
                                dsConsultationProblems = obj.Data;
                            }
                            if (dsConsultationProblems == null)
                                dsConsultationProblems = new DSConsultationOrder();
                            //End Farooq Ahmad 21/3/2016 load the consultation order problems
                            //----------------------

                            List<Dictionary<string, string>> lstConsultationTest = new List<Dictionary<string, string>>();

                            if (MDVUtility.ToInt64(model.ConsultationOrderId) > 0)
                            {
                                DSConsultationOrder dsConsultationTest = new DSConsultationOrder();
                                BLObject<DSConsultationOrder> objTest = BLLClinicalObj.LoadConsultationOrderTest(MDVUtility.ToInt64(model.ConsultationOrderId), 0, MDVUtility.ToInt64(model.PatientId), "1", "2000");
                                dsConsultationTest = objTest.Data;

                                foreach (DataRow drConsultationTest in dsConsultationTest.Tables[dsConsultationTest.ConsultationOrderTest.TableName].Rows)
                                {
                                    string ConsultationTestId = MDVUtility.ToStr(drConsultationTest[dsConsultationTest.ConsultationOrderTest.ConsultationOrderTestIdColumn.ColumnName]);
                                    var ConsultationTestValues = new Dictionary<string, string>
                                {
                                {"ConsultationOrderTestId", ConsultationTestId},
                                {"ConsultationDate", MDVUtility.ToStr(drConsultationTest[dsConsultationTest.ConsultationOrderTest.TestDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drConsultationTest[dsConsultationTest.ConsultationOrderTest.TestDateColumn.ColumnName]).ToShortDateString():""},
                                {"ConsultationTime", MDVUtility.ToStr(drConsultationTest[dsConsultationTest.ConsultationOrderTest.TestTimeColumn.ColumnName])},
                                //{ "ConsultationProcedure", MDVUtility.ToStr(drConsultationTest[dsConsultationTest.ConsultationOrderTest.CPTCodeDescriptionColumn.ColumnName])},
                                { "CPTCode", MDVUtility.ToStr(drConsultationTest[dsConsultationTest.ConsultationOrderTest.CPTCodeColumn.ColumnName])},
                                { "CPTDescription", MDVUtility.ToStr(drConsultationTest[dsConsultationTest.ConsultationOrderTest.CPTCodeDescriptionColumn.ColumnName])},
                                { "CPTSNOMEDCodeId", MDVUtility.ToStr(drConsultationTest[dsConsultationTest.ConsultationOrderTest.CPTSNOMEDIDColumn.ColumnName])},
                                { "CPTSNOMEDDescription", MDVUtility.ToStr(drConsultationTest[dsConsultationTest.ConsultationOrderTest.CPTSNOMEDDescriptionColumn.ColumnName])},

                                { "Urgency", MDVUtility.ToStr(drConsultationTest[dsConsultationTest.ConsultationOrderTest.UrgencyIdColumn.ColumnName])},
                                };

                                    lstConsultationTest.Add(ConsultationTestValues);
                                }

                            }

                            //----------------------
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ConsultationOrderJSON = js.Serialize(keyValues),
                                //Start Farooq Ahmad 21/3/2016 load the consultation order problems
                                ConsultationOrderProblems_JSON = MDVUtility.JSON_DataTable(dsConsultationProblems.Tables[dsConsultationProblems.ConsultationOrderProblem.TableName]),
                                //End Farooq Ahmad 21/3/2016 load the consultation order problems
                                consultationOrderTest_JSON = js.Serialize(lstConsultationTest),
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
        /// Method Name:  attachConsultationOrderWithNotes
        /// Author Name: Ahmad Raza
        /// Description: attaching consultation order with Notes
        /// </summary>
        /// <param name="ConsultationOrderId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string attachConsultationOrderWithNotes(string consultationOrderId, long notesId)
        {
            try
            {
                DSConsultationOrder dsConsultationOrder = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(consultationOrderId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<DSConsultationOrder> obj = BLLClinicalObj.attachConsultationOrderWithNotes(consultationOrderId, notesId);
                    if (obj.Data != null)
                    {
                        dsConsultationOrder = obj.Data;
                        var response = new
                        {
                            status = true,
                            ConsultationOrderTotalCount = dsConsultationOrder.Tables[dsConsultationOrder.ConsultationOrder.TableName].Rows.Count,
                            ConsultationOrderCount = dsConsultationOrder.Tables[dsConsultationOrder.ConsultationOrder.TableName].Rows.Count,
                            ConsultationOrderLoad_JSON = MDVUtility.JSON_DataTable(dsConsultationOrder.Tables[dsConsultationOrder.ConsultationOrder.TableName]),
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
        ///  Method Name:  detachConsultationOrderFromNotes
        ///  Author Name: Ahmad Raza
        ///  Description: Detaching consultation order from Notes
        /// </summary>
        /// <param name="consultationOrderId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string detachConsultationOrderFromNotes(string consultationOrderIDs, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(consultationOrderIDs)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachConsultationOrderFromNotes(consultationOrderIDs, notesId);
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

        /// <summary>
        /// Method Name: deleteConsultationOrder
        /// Author : Ahmad Raza
        /// Description: This function will delete the consultation order
        /// </summary>
        /// <param name="consultationOrderId"></param>
        /// <returns></returns>
        public string deleteConsultationOrder(string consultationOrderId)
        {
            try
            {
                string result = "";
                DSConsultationOrder dsConsultation = null, dsConsultationProblems = null;
                BLObject<string> obj;
                //Start 21-03-2016 Humaira Yousaf
                obj = BLLClinicalObj.deleteConsultationOrder(consultationOrderId);
                //End 21-03-2016 Humaira Yousaf
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

        #region"Consultation Order Test"
        //public string insertUpdateConsultationOrderTest(Int64 ConsultationId, List<object> lstObjects)
        //{
        //    try
        //    {
        //        #region ConsultationOrderTest

        //        DSConsultationOrder dsConsultationOrder = new DSConsultationOrder();

        //        BLObject<DSConsultationOrder> objConsultationOrderTest = BLLClinicalObj.LoadConsultationOrderTest(MDVUtility.ToInt64(ConsultationId), 0, 0, "1", "2000");
        //        dsConsultationOrder = objConsultationOrderTest.Data;
        //        List<ConsultationOrderTestModel> lstConsultationOrder = lstObjects.OfType<ConsultationOrderTestModel>().ToList();
        //        int id = -1;
        //        foreach (ConsultationOrderTestModel CurrentModel in lstConsultationOrder)
        //        {
        //            Int32 currentConsultationTestId = MDVUtility.ToInt32(CurrentModel.ConsultationOrderTestId);
        //            currentConsultationTestId = currentConsultationTestId == 0 ? id-- : currentConsultationTestId;
        //            DSConsultationOrder.ConsultationOrderTestRow RowConsultationOrderTest = null;
        //            DSConsultationOrder.ConsultationOrderTestRow[] arrConsultationOrderTestRows = (DSConsultationOrder.ConsultationOrderTestRow[])dsConsultationOrder.ConsultationOrderTest.Select(dsConsultationOrder.ConsultationOrderTest.ConsultationOrderTestIdColumn.ColumnName + "=" + CurrentModel.ConsultationOrderTestId);

        //            if (arrConsultationOrderTestRows.Length > 0)
        //            {
        //                RowConsultationOrderTest = arrConsultationOrderTestRows[0];
        //            }
        //            else
        //            {
        //                RowConsultationOrderTest = dsConsultationOrder.ConsultationOrderTest.NewConsultationOrderTestRow();
        //                RowConsultationOrderTest.ConsultationOrderTestId = currentConsultationTestId;
        //            }
        //            if (RowConsultationOrderTest != null)
        //            {
        //                RowConsultationOrderTest.ConsultationOrderId = ConsultationId;


        //                if (!string.IsNullOrEmpty(CurrentModel.dtpConsultationDate))
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.TestDateColumn] = MDVUtility.ToDateTime(CurrentModel.dtpConsultationDate);
        //                }
        //                else
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.TestDateColumn] = DBNull.Value;
        //                }

        //                if (!string.IsNullOrEmpty(CurrentModel.tpConsultationTime))
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.TestTimeColumn] = MDVUtility.ToStr(CurrentModel.tpConsultationTime);
        //                }
        //                else
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.TestTimeColumn] = DBNull.Value;
        //                }

        //                if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.CPTCodeColumn] = MDVUtility.ToStr(CurrentModel.CPTCode);

        //                }
        //                else
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.CPTCodeColumn] = DBNull.Value;
        //                }

        //                if (!string.IsNullOrEmpty(CurrentModel.CPTDescription))
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.CPTCodeDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTDescription);

        //                }
        //                else
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.CPTCodeDescriptionColumn] = DBNull.Value;
        //                }
        //                if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDCodeId))
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.CPTSNOMEDIDColumn] = MDVUtility.ToStr(CurrentModel.CPTSNOMEDCodeId);

        //                }
        //                else
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.CPTSNOMEDIDColumn] = DBNull.Value;
        //                }

        //                if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDDescription))
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.CPTSNOMEDDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTSNOMEDDescription);

        //                }
        //                else
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.CPTSNOMEDDescriptionColumn] = DBNull.Value;
        //                }
        //                if (!string.IsNullOrEmpty(CurrentModel.Urgency))
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.UrgencyIdColumn] = MDVUtility.ToInt32(CurrentModel.Urgency);

        //                }
        //                else
        //                {
        //                    RowConsultationOrderTest[dsConsultationOrder.ConsultationOrderTest.UrgencyIdColumn] = DBNull.Value;
        //                }
        //                RowConsultationOrderTest.IsActive = true;
        //                RowConsultationOrderTest.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                RowConsultationOrderTest.CreatedOn = DateTime.Now;
        //                RowConsultationOrderTest.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                RowConsultationOrderTest.ModifiedOn = DateTime.Now;

        //                if (arrConsultationOrderTestRows.Length < 1)
        //                {
        //                    dsConsultationOrder.ConsultationOrderTest.AddConsultationOrderTestRow(RowConsultationOrderTest);
        //                }
        //            }
        //        }

        //        #region Database Insertion/Updation

        //        BLObject<DSConsultationOrder> objInsertedConsultationTest = BLLClinicalObj.insertUpdateConsultationOrderTest(dsConsultationOrder);
        //        if (objInsertedConsultationTest.Data != null)
        //        {
        //            var response = new
        //            {
        //                status = true,
        //                message = Common.AppPrivileges.Save_Message
        //            };
        //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = objInsertedConsultationTest.Message
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }

        //        #endregion

        //        #endregion
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}
        public string deleteConsultationOrderTest(string ConsultationOrderTestId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.deleteConsultationOrderTest(ConsultationOrderTestId);
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
        /// Method Name: previewConsultationOrder
        /// Author : Humaira Yousaf
        /// Created Date: 23-03-2016
        /// Description: Creates PDF to view Consultation Order
        /// </summary>
        /// <param name="model" type="ConsultationOrderModel">model</param>
        public string previewConsultationOrder(ConsultationOrderModel model)
        {
            try
            {
                BLObject<byte[]> obj = BLLClinicalObj.previewConsultationOrder(MDVUtility.ToInt64(model.ConsultationOrderId), MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ConsultationOrderHTML = Convert.ToBase64String(obj.Data),
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
        /// Description: This function will handle Load of Provider for Consultation Order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string loadOrderingProvider(ConsultationOrderModel model)
        {

            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> list = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();

            DSConsultationOrder dsConsultation = null;
            BLObject<DSProfileLookup> objProvider = BLLAdminProfileObj.LookupProvider("true");
            DSProfileLookup dsProvider = objProvider.Data;
            BLObject<DSConsultationOrder> obj;
            obj = BLLClinicalObj.loadconsultationOrder(0, MDVUtility.ToInt64(model.PatientId), "", "", 0, "", "", "", 1, 2000);
            dsConsultation = obj.Data;
            list.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));
            if (dsProvider != null)
            {

                if (dsProvider.Tables[dsProvider.Provider.TableName] != null)
                {

                    DataView view = new DataView(dsConsultation.Tables[dsConsultation.ConsultationOrder.TableName]);
                    DataTable distinctValues = view.ToTable(true, dsConsultation.ConsultationOrder.ProviderIdColumn.ColumnName);
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

        /// <summary>
        /// Author: Ahmad Raza
        /// Method Name: getOrdersForSoap
        /// Date: 28-06-2016
        /// Desription: This function will get Consultation Orders for Soap text
        /// </summary>
        /// <param name="orderIDs"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        internal string getOrdersForSoap(string orderIDs, long patientId, long notesId)
        {
            try
            {

                DSConsultationOrder dsOrderSoap = null;
                DSProblemLists dsProblemSoap = null;
                BLObject<DSConsultationOrder> obj = BLLClinicalObj.loadConsultationOrdersForSoap(orderIDs, patientId);
                BLObject<DSProblemLists> objPrb = BLLClinicalObj.attachConsultationProblemsWithNoteForSoap(orderIDs, notesId);


                dsOrderSoap = obj.Data;
                dsProblemSoap = objPrb.Data;
                if (obj.Data != null)
                {
                    if (dsOrderSoap.Tables[dsOrderSoap.ConsultationOrder.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ConsultationOrderSoapCount = dsOrderSoap.Tables[dsOrderSoap.ConsultationOrder.TableName].Rows.Count,
                            ConsultationOrderSoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.ConsultationOrder.TableName]),
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
                            ConsultationOrderSoapCount = 0,
                            ConsultationOrderSoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.ConsultationOrder.TableName]),
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

        /// <summary>
        /// Author: Ahmad Raza
        /// Method Name: attachOrdersWithNotes
        /// Date: 28-06-2016
        /// Desription: This function will attach Consultation Orders with Notes
        /// </summary>
        /// <param name="consultationOrderIDs"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string attachOrdersWithNotes(string consultationOrderIDs, long notesId)
        {
            try
            {
                DSConsultationOrder dsOrders = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(consultationOrderIDs)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<DSConsultationOrder> obj = BLLClinicalObj.attachConsultationOrderWithNotes(consultationOrderIDs, notesId);
                    if (obj.Data != null)
                    {
                        dsOrders = obj.Data;
                        var response = new
                        {
                            status = true,
                            ConsultationOrderTotalCount = dsOrders.Tables[dsOrders.ConsultationOrder.TableName].Rows.Count,
                            ConsultationOrderCount = dsOrders.Tables[dsOrders.ConsultationOrder.TableName].Rows.Count,
                            ConsultationOrderLoad_JSON = MDVUtility.JSON_DataTable(dsOrders.Tables[dsOrders.ConsultationOrder.TableName]),
                            ConsultationOrderHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsOrders.Tables[dsOrders.ConsultationOrder.TableName]),
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

    }
}