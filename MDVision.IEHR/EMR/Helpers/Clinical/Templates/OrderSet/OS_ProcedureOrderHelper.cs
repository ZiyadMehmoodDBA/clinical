using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.OrdersAndResults.ProcedureOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates.OrderSet
{
    public class OS_ProcedureOrderHelper
    {
        private BLLOrderSet BLLClinicalObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;
        public OS_ProcedureOrderHelper()
        {
            BLLClinicalObj = new BLLOrderSet();
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        private static OS_ProcedureOrderHelper _instance = null;
        public static OS_ProcedureOrderHelper Instance()
        {
            if (_instance == null)
                _instance = new OS_ProcedureOrderHelper();
            return _instance;
        }

        public string saveProcedureOrder(ProcedureOrderModel model, List<object> lstProcedureObjects)
        {
            try
            {
                DSProcedureOrder dsProcedureOrder = new DSProcedureOrder();
                //Start 21-03-2016 Humaira Yousaf 
                BLObject<DSProcedureOrder> obj = BLLClinicalObj.LoadProcedureOrder(MDVUtility.ToInt32(model.ProcedureOrderId), MDVUtility.ToInt64(model.OrderSetId));
                //End 21-03-2016 Humaira Yousaf 
                if (obj.Data != null)
                    dsProcedureOrder = obj.Data;

                DSProcedureOrder.ProcedureOrderRow dr = null;
                DSProcedureOrder.ProcedureOrderRow[] arrdr = (DSProcedureOrder.ProcedureOrderRow[])dsProcedureOrder.ProcedureOrder.Select(dsProcedureOrder.ProcedureOrder.ProcedureOrderIdColumn.ColumnName + "=" + model.ProcedureOrderId);

                if (arrdr.Length > 0)
                {
                    dr = arrdr[0];
                }
                else
                {
                    dr = dsProcedureOrder.ProcedureOrder.NewProcedureOrderRow();
                }


                if (!string.IsNullOrEmpty(model.AssigneeId))
                    dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
                else
                    dr[dsProcedureOrder.ProcedureOrder.AssigneeIdColumn] = DBNull.Value;
                dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                //Start 22-03-2016 Humaira Yousaf for status            
                dr.Status = model.Status;
                //End 22-03-2016 Humaira Yousaf for status            

                #region Database Insertion
                if (arrdr.Length <= 0)
                    dsProcedureOrder.ProcedureOrder.AddProcedureOrderRow(dr);

                BLObject<DSProcedureOrder> obj2 = BLLClinicalObj.InsertUpdateProcedureOrder(dsProcedureOrder);

                dsProcedureOrder = obj2.Data;
                ////Start 12-04-2016 Humaira Yousaf to save favorite list setting
                //if (obj2.Data != null)
                //{
                //  string res =  LabOrderHelper.insertUpdateFavListSetting(model.FavListNames);                  
                //}
                ////End 12-04-2016 Humaira Yousaf to save favorite list setting
                if (obj.Data != null)
                {
                    Int64 procedureOrderId = MDVUtility.ToInt64(dsProcedureOrder.Tables[dsProcedureOrder.ProcedureOrder.TableName].Rows[0][dsProcedureOrder.ProcedureOrder.ProcedureOrderIdColumn.ColumnName]);
                    model.ProcedureOrderProblemList.ForEach(cc => cc.ProcedureOrderId = MDVUtility.ToStr(procedureOrderId));
                    BLLClinicalObj.DeleteProcedureOrderProblems(MDVUtility.ToStr(procedureOrderId));
                    var id = saveProcedureOrderProblem(model.ProcedureOrderProblemList);

                    insertUpdateProcedureOrderTest(procedureOrderId, lstProcedureObjects);

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        procedureOrderId = procedureOrderId,
                        // procedureFavList = model.FavListNames

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
        public string loadProcedureOrder(ProcedureOrderModel model)
        {
            try
            {

                DSProcedureOrder dsProcedure = null, dsProcedureProblems = null;
                BLObject<DSProcedureOrder> obj;
                obj = BLLClinicalObj.LoadProcedureOrder(MDVUtility.ToInt32(model.ProcedureOrderId),MDVUtility.ToInt64(model.OrderSetId),MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsProcedure = obj.Data;
                if (dsProcedure.Tables[dsProcedure.ProcedureOrder.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsProcedure.Tables[dsProcedure.ProcedureOrder.TableName].Rows[dsProcedure.Tables[dsProcedure.ProcedureOrder.TableName].Rows.Count - 1];
                    var ProcedureOrderkeyValues = new Dictionary<string, string>
                        {
                            { "ProcedureOrderId",  MDVUtility.ToStr(dr[dsProcedure.ProcedureOrder.ProcedureOrderIdColumn.ColumnName])},
                            { "SoapText", MDVUtility.ToStr(dr[dsProcedure.ProcedureOrder.SoapTextColumn.ColumnName])}
                        };
                    //Start Farooq Ahmad 21/3/2016 load the procedure order problems
                    if (MDVUtility.ToInt32(model.ProcedureOrderId) > 0)
                    {
                        obj = BLLClinicalObj.LoadProcedureOrderProblems(MDVUtility.ToInt32(model.ProcedureOrderId));
                        dsProcedureProblems = obj.Data;
                    }
                    if (dsProcedureProblems == null)
                        dsProcedureProblems = new DSProcedureOrder();
                    //End Farooq Ahmad 21/3/2016 load the procedure order problems

                    //----------------------

                    List<Dictionary<string, string>> lstProcedureTest = new List<Dictionary<string, string>>();

                    if (MDVUtility.ToInt64(model.ProcedureOrderId) > 0)
                    {
                        DSProcedureOrder dsProcedureTest = new DSProcedureOrder();
                        BLObject<DSProcedureOrder> objTest = BLLClinicalObj.LoadProcedureOrderTest(MDVUtility.ToInt64(model.ProcedureOrderId), 0, "1", "2000");
                        dsProcedureTest = objTest.Data;

                        foreach (DataRow drProcedureTest in dsProcedureTest.Tables[dsProcedureTest.ProcedureOrderTest.TableName].Rows)
                        {
                            string ProcedureTestId = MDVUtility.ToStr(drProcedureTest[dsProcedureTest.ProcedureOrderTest.ProcedureOrderTestIdColumn.ColumnName]);
                            var ProcedureTestValues = new Dictionary<string, string>
                            {
                                {"ProcedureOrderTestId", ProcedureTestId},
                                {"ProcedureDate", MDVUtility.ToStr(drProcedureTest[dsProcedureTest.ProcedureOrderTest.TestDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(drProcedureTest[dsProcedureTest.ProcedureOrderTest.TestDateColumn.ColumnName]).ToShortDateString():""},
                                {"ProcedureTime", MDVUtility.ToStr(drProcedureTest[dsProcedureTest.ProcedureOrderTest.TestTimeColumn.ColumnName])},
                               // { "ProcedureProcedure", MDVUtility.ToStr(drProcedureTest[dsProcedureTest.ProcedureOrderTest.CPTCodeDescriptionColumn.ColumnName])},
                                { "CPTCode", MDVUtility.ToStr(drProcedureTest[dsProcedureTest.ProcedureOrderTest.CPTCodeColumn.ColumnName])},
                                { "CPTDescription", MDVUtility.ToStr(drProcedureTest[dsProcedureTest.ProcedureOrderTest.CPTCodeDescriptionColumn.ColumnName])},
                                { "CPTSNOMEDCodeId", MDVUtility.ToStr(drProcedureTest[dsProcedureTest.ProcedureOrderTest.CPTSNOMEDIDColumn.ColumnName])},
                                { "CPTSNOMEDDescription", MDVUtility.ToStr(drProcedureTest[dsProcedureTest.ProcedureOrderTest.CPTSNOMEDDescriptionColumn.ColumnName])},

                                { "Urgency", MDVUtility.ToStr(drProcedureTest[dsProcedureTest.ProcedureOrderTest.UrgencyIdColumn.ColumnName])},
								//Start 05-04-2016 Humaira Yousaf for comments
                                {"Comments", MDVUtility.ToStr(drProcedureTest[dsProcedureTest.ProcedureOrderTest.CommentsColumn.ColumnName])}
								//End 05-04-2016 Humaira Yousaf for comments
                            };

                            lstProcedureTest.Add(ProcedureTestValues);
                        }

                    }

                    //----------------------

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ProcedureOrderFill_JSON = js.Serialize(ProcedureOrderkeyValues),
                        procedureOrderCount = dsProcedure.Tables[dsProcedure.ProcedureOrder.TableName].Rows.Count,
                        ProcedureLoad_JSON = MDVUtility.JSON_DataTable(dsProcedure.Tables[dsProcedure.ProcedureOrder.TableName]),
                        //Start Farooq Ahmad 21/3/2016 load the procedure order problems
                        ProcedureOrderProblems_JSON = MDVUtility.JSON_DataTable(dsProcedureProblems.Tables[dsProcedureProblems.ProcedureOrderProblem.TableName]),
                        //End Farooq Ahmad 21/3/2016 load the procedure order problems
                        iTotalDisplayRecords = (dsProcedure.ProcedureOrder.Rows.Count > 0) ? dsProcedure.ProcedureOrder.Rows[0][dsProcedure.ProcedureOrder.RecordCountColumn.ColumnName] : 0,

                        procedureOrderTest_JSON = js.Serialize(lstProcedureTest),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ProcedureOrderFill_JSON = "[]",
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
        public string saveProcedureOrderProblem(List<ProcedureOrderProblemModel> model)
        {
            try
            {
                DSProcedureOrder dsProcedureOrder = new DSProcedureOrder();

                foreach (var m in model)
                {
                    DSProcedureOrder.ProcedureOrderProblemRow dr = dsProcedureOrder.ProcedureOrderProblem.NewProcedureOrderProblemRow();
                    dr.ProcedureOrderId = MDVUtility.ToInt64(m.ProcedureOrderId);
                    dr.ProblemId = MDVUtility.ToInt32(m.ProblemId);
                    dr.Comments = string.Empty;
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dsProcedureOrder.ProcedureOrderProblem.AddProcedureOrderProblemRow(dr);
                }
                #region Database Insertion
                BLObject<DSProcedureOrder> obj = BLLClinicalObj.InsertUpdateProcedureOrderProblems(dsProcedureOrder);
                dsProcedureOrder = obj.Data;

                if (obj.Data != null)
                {

                    Int64 procedureOrderId = MDVUtility.ToInt64(dsProcedureOrder.Tables[dsProcedureOrder.ProcedureOrderProblem.TableName].Rows[0][dsProcedureOrder.ProcedureOrderProblem.ProcedureOrderIdColumn.ColumnName]);

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        procedureOrderId = procedureOrderId,
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
        public string insertUpdateProcedureOrderTest(Int64 ProcedureId, List<object> lstObjects)
        {
            try
            {
                #region ProcedureOrderTest

                DSProcedureOrder dsProcedureOrder = new DSProcedureOrder();

                BLObject<DSProcedureOrder> objProcedureOrderTest = BLLClinicalObj.LoadProcedureOrderTest(MDVUtility.ToInt64(ProcedureId), 0, "1", "2000");
                dsProcedureOrder = objProcedureOrderTest.Data;
                //dsProcedureOrder = objProcedureOrder.Data;
                List<ProcedureOrderTestModel> lstProcedureOrder = lstObjects.OfType<ProcedureOrderTestModel>().ToList();

                int id = -1;

                foreach (ProcedureOrderTestModel CurrentModel in lstProcedureOrder)
                {
                    Int32 currentProcedureTestId = MDVUtility.ToInt32(CurrentModel.ProcedureOrderTestId);
                    currentProcedureTestId = currentProcedureTestId == 0 ? id-- : currentProcedureTestId;



                    DSProcedureOrder.ProcedureOrderTestRow RowProcedureOrderTest = null;
                    DSProcedureOrder.ProcedureOrderTestRow[] arrProcedureOrderTestRows = (DSProcedureOrder.ProcedureOrderTestRow[])dsProcedureOrder.ProcedureOrderTest.Select(dsProcedureOrder.ProcedureOrderTest.ProcedureOrderTestIdColumn.ColumnName + "=" + CurrentModel.ProcedureOrderTestId);

                    if (arrProcedureOrderTestRows.Length > 0)
                    {
                        RowProcedureOrderTest = arrProcedureOrderTestRows[0];
                    }
                    else
                    {
                        RowProcedureOrderTest = dsProcedureOrder.ProcedureOrderTest.NewProcedureOrderTestRow();
                        RowProcedureOrderTest.ProcedureOrderTestId = currentProcedureTestId;
                    }
                    if (RowProcedureOrderTest != null)
                    {
                        RowProcedureOrderTest.ProcedureOrderId = ProcedureId;


                        if (!string.IsNullOrEmpty(CurrentModel.dtpProcedureDate))
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.TestDateColumn] = MDVUtility.ToDateTime(CurrentModel.dtpProcedureDate);
                        }
                        else
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.TestDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.tpProcedureTime))
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.TestTimeColumn] = MDVUtility.ToStr(CurrentModel.tpProcedureTime);
                        }
                        else
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.TestTimeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.CPTCodeColumn] = MDVUtility.ToStr(CurrentModel.CPTCode);

                        }
                        else
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.CPTCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTDescription))
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.CPTCodeDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTDescription);

                        }
                        else
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.CPTCodeDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDCodeId))
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.CPTSNOMEDIDColumn] = MDVUtility.ToStr(CurrentModel.CPTSNOMEDCodeId);

                        }
                        else
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.CPTSNOMEDIDColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.CPTSNOMEDDescription))
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.CPTSNOMEDDescriptionColumn] = MDVUtility.ToStr(CurrentModel.CPTSNOMEDDescription);

                        }
                        else
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.CPTSNOMEDDescriptionColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(CurrentModel.Urgency))
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.UrgencyIdColumn] = MDVUtility.ToInt32(CurrentModel.Urgency);

                        }
                        else
                        {
                            RowProcedureOrderTest[dsProcedureOrder.ProcedureOrderTest.UrgencyIdColumn] = DBNull.Value;
                        }
                        RowProcedureOrderTest.IsActive = true;
                        RowProcedureOrderTest.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowProcedureOrderTest.CreatedOn = DateTime.Now;
                        RowProcedureOrderTest.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowProcedureOrderTest.ModifiedOn = DateTime.Now;
                        //Start 05-04-2016 Humaira Yousaf for comments
                        RowProcedureOrderTest.Comments = CurrentModel.Comments;
                        //End 05-04-2016 Humaira Yousaf for comments

                        if (arrProcedureOrderTestRows.Length < 1)
                        {
                            dsProcedureOrder.ProcedureOrderTest.AddProcedureOrderTestRow(RowProcedureOrderTest);
                        }
                        //dsProcedureOrder.ProcedureOrderTest.AddProcedureOrderTestRow(RowProcedureOrderTest);

                    }
                }

                #region Database Insertion/Updation

                BLObject<DSProcedureOrder> objInsertedProcedureTest = BLLClinicalObj.insertUpdateProcedureOrderTest(dsProcedureOrder);
                if (objInsertedProcedureTest.Data != null)
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
                        Message = objInsertedProcedureTest.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }

                #endregion

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
        public string deleteProcedureOrder(string procedureOrderId)
        {
            try
            {
                string result = "";
                DSProcedureOrder dsProcedure = null, dsProcedureProblems = null;
                BLObject<string> obj;
                obj = BLLClinicalObj.DeleteProcedureOrder(procedureOrderId);
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
        public string deleteProcedureOrderTest(string procedureOrderTestId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.deleteProcedureOrderTest(procedureOrderTestId);
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
    }
}