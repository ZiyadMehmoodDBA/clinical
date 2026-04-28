/* Author:  Muhammad Arshad
 * Created Date: 17/03/2016
 * OverView: Created to handel Procedure Order
 */
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.IEHR.EMR.Model.OrdersAndResults.ProcedureOrder;

using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Orders
{
    public class ProcedureOrderHelper
    {
        private BLLClinical BLLClinicalObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;
        public ProcedureOrderHelper()
        {
            BLLClinicalObj = new BLLClinical();
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        private static ProcedureOrderHelper _instance = null;
        public static ProcedureOrderHelper Instance()
        {
            if (_instance == null)
                _instance = new ProcedureOrderHelper();
            return _instance;
        }

        /// <summary>
        /// Method Name: loadOrderingProvider
        /// Author Name: Ahmad Raza
        /// Created Date: 08-03-2016
        /// Description: This function will handle Load of Provider for Procedure Order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string loadOrderingProvider(ProcedureOrderModel model)
        {

            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> list = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();

            DSProcedureOrder dsProcedure = null;
            BLObject<DSProfileLookup> objProvider = BLLAdminProfileObj.LookupProvider("true");
            DSProfileLookup dsProvider = objProvider.Data;
            BLObject<DSProcedureOrder> obj;
            obj = BLLClinicalObj.LoadProcedureOrder(0, MDVUtility.ToInt64(model.PatientId), "", "", 0, "", "", "", 1, 2000);
            dsProcedure = obj.Data;
            list.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));
            if (dsProvider != null)
            {

                if (dsProvider.Tables[dsProvider.Provider.TableName] != null)
                {

                    DataView view = new DataView(dsProcedure.Tables[dsProcedure.ProcedureOrder.TableName]);
                    DataTable distinctValues = view.ToTable(true, dsProcedure.ProcedureOrder.ProviderIdColumn.ColumnName);
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
        /// Module Name: loadProcedureOrder
        /// Author: Humaira Yousaf
        /// Created Date: 17-03-2016
        /// Description: Loads Procedure Orders
        /// </summary>
        /// <param name="model" type="ProcedureOrderModel">ProcedureOrderModel containing data</param>
        public string loadProcedureOrder(ProcedureOrderModel model)
        {
            try
            {

                DSProcedureOrder dsProcedure = null, dsProcedureProblems = null;
                BLObject<DSProcedureOrder> obj;
                //Start 21-03-2016 Humaira Yousaf
                obj = BLLClinicalObj.LoadProcedureOrder(MDVUtility.ToInt32(model.ProcedureOrderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToStr(model.ProcedureOrderTitle).Replace("- ", "||"), model.OrderNo, MDVUtility.ToInt64(model.ProviderId), model.OrderFromDate, model.OrderToDate, model.Status, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "1", "", MDVUtility.ToInt64(model.NoteId));
                //End 21-03-2016 Humaira Yousaf
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
                        obj = BLLClinicalObj.LoadProcedureOrderProblems(MDVUtility.ToInt32(model.ProcedureOrderId), MDVUtility.ToInt64(model.PatientId));
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
                        BLObject<DSProcedureOrder> objTest = BLLClinicalObj.LoadProcedureOrderTest(MDVUtility.ToInt64(model.ProcedureOrderId), 0, MDVUtility.ToInt64(model.PatientId), "1", "2000");
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        // Author: Farooq Ahmad
        // Created Date: 18/03/2016
        //OverView: This function will save Procedure Order
        //public string saveProcedureOrder(ProcedureOrderModel model, List<object> lstProcedureObjects)
        //{
        //    try
        //    {
        //        DSProcedureOrder dsProcedureOrder = new DSProcedureOrder();
        //        //Start 21-03-2016 Humaira Yousaf
        //        BLObject<DSProcedureOrder> obj = BLLClinicalObj.LoadProcedureOrder(MDVUtility.ToInt32(model.ProcedureOrderId), MDVUtility.ToInt64(model.PatientId), "", "", 0, "", "", "");
        //        //End 21-03-2016 Humaira Yousaf
        //        if (obj.Data != null)
        //            dsProcedureOrder = obj.Data;

        //        DSProcedureOrder.ProcedureOrderRow dr = null;
        //        DSProcedureOrder.ProcedureOrderRow[] arrdr = (DSProcedureOrder.ProcedureOrderRow[])dsProcedureOrder.ProcedureOrder.Select(dsProcedureOrder.ProcedureOrder.ProcedureOrderIdColumn.ColumnName + "=" + model.ProcedureOrderId);

        //        if (arrdr.Length > 0)
        //        {
        //            dr = arrdr[0];
        //        }
        //        else
        //        {
        //            dr = dsProcedureOrder.ProcedureOrder.NewProcedureOrderRow();
        //        }

        //        if (string.IsNullOrEmpty(model.PatientId))
        //            dr.PatientId = 0;
        //        else
        //            dr.PatientId = MDVUtility.ToInt64(model.PatientId);

        //        if (!string.IsNullOrEmpty(model.ProviderId))
        //            dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
        //        else
        //            dr[dsProcedureOrder.ProcedureOrder.ProviderIdColumn] = DBNull.Value;

        //        if (!string.IsNullOrEmpty(model.AssigneeId))
        //            dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
        //        else
        //            dr[dsProcedureOrder.ProcedureOrder.AssigneeIdColumn] = DBNull.Value;

        //        if (!string.IsNullOrEmpty(model.NoteId))
        //            dr.NoteId = MDVUtility.ToInt64(model.NoteId);
        //        else
        //            dr[dsProcedureOrder.ProcedureOrder.NoteIdColumn] = DBNull.Value;


        //        //if (string.IsNullOrEmpty(model.ProcedureOderDate))
        //        //    dr.OrderDate = DBNull.Value;
        //        //else
        //        if (!string.IsNullOrEmpty(model.ProcedureOderDate))
        //            dr.OrderDate = MDVUtility.ToDateTime(model.ProcedureOderDate);
        //        else
        //            dr[dsProcedureOrder.ProcedureOrder.OrderDateColumn] = DBNull.Value;
        //        dr.OrderTime = MDVUtility.ToStr(model.ProcedureOderTime);
        //        dr.IsActive = true;
        //        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //        dr.CreatedOn = DateTime.Now;
        //        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //        dr.ModifiedOn = DateTime.Now;
        //        //Start 22-03-2016 Humaira Yousaf for status
        //        dr.Status = model.Status;
        //        //End 22-03-2016 Humaira Yousaf for status

        //        #region Database Insertion
        //        if (arrdr.Length <= 0)
        //            dsProcedureOrder.ProcedureOrder.AddProcedureOrderRow(dr);

        //        BLObject<DSProcedureOrder> obj2 = BLLClinicalObj.InsertUpdateProcedureOrder(dsProcedureOrder);

        //        dsProcedureOrder = obj2.Data;
        //        ////Start 12-04-2016 Humaira Yousaf to save favorite list setting
        //        //if (obj2.Data != null)
        //        //{
        //        //  string res =  LabOrderHelper.insertUpdateFavListSetting(model.FavListNames);
        //        //}
        //        ////End 12-04-2016 Humaira Yousaf to save favorite list setting
        //        if (obj.Data != null)
        //        {
        //            Int64 procedureOrderId = MDVUtility.ToInt64(dsProcedureOrder.Tables[dsProcedureOrder.ProcedureOrder.TableName].Rows[0][dsProcedureOrder.ProcedureOrder.ProcedureOrderIdColumn.ColumnName]);
        //            model.ProcedureOrderProblemList.ForEach(cc => cc.ProcedureOrderId = MDVUtility.ToStr(procedureOrderId));
        //            BLLClinicalObj.DeleteProcedureOrderProblems(MDVUtility.ToStr(procedureOrderId));
        //            var id = saveProcedureOrderProblem(model.ProcedureOrderProblemList);

        //            insertUpdateProcedureOrderTest(procedureOrderId, lstProcedureObjects);

        //            var response = new
        //            {
        //                status = true,
        //                message = Common.AppPrivileges.Save_Message,
        //                procedureOrderId = procedureOrderId,
        //                // procedureFavList = model.FavListNames

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

        // Author: Farooq Ahmad
        // Created Date: 18/03/2016
        //OverView: This function will save Procedure Order
        public string saveProcedureOrder(ProcedureOrderModel model, List<object> lstProcedureObjects)
        {
            try
            {
                DSProcedureOrder dsProcedureOrder = new DSProcedureOrder();
                //Start 21-03-2016 Humaira Yousaf
                BLObject<DSProcedureOrder> obj = BLLClinicalObj.LoadProcedureOrder(MDVUtility.ToInt32(model.ProcedureOrderId), MDVUtility.ToInt64(model.PatientId), "", "", 0, "", "", "");
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

                if (string.IsNullOrEmpty(model.PatientId))
                    dr.PatientId = 0;
                else
                    dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.ProviderId))
                    dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                else
                    dr[dsProcedureOrder.ProcedureOrder.ProviderIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.AssigneeId))
                    dr.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
                else
                    dr[dsProcedureOrder.ProcedureOrder.AssigneeIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.NoteId))
                    dr.NoteId = MDVUtility.ToInt64(model.NoteId);
                else
                    dr[dsProcedureOrder.ProcedureOrder.NoteIdColumn] = DBNull.Value;


                //if (string.IsNullOrEmpty(model.ProcedureOderDate))
                //    dr.OrderDate = DBNull.Value;
                //else
                if (!string.IsNullOrEmpty(model.ProcedureOderDate))
                    dr.OrderDate = MDVUtility.ToDateTime(model.ProcedureOderDate);
                else
                    dr[dsProcedureOrder.ProcedureOrder.OrderDateColumn] = DBNull.Value;
                dr.OrderTime = MDVUtility.ToStr(model.ProcedureOderTime);
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

                BLLClinicalObj.InsertUpdateProcedureOrder(dsProcedureOrder, model, lstProcedureObjects);
                if (obj.Data != null)
                {
                    Int64 procedureOrderId = MDVUtility.ToInt64(dsProcedureOrder.Tables[dsProcedureOrder.ProcedureOrder.TableName].Rows[0][dsProcedureOrder.ProcedureOrder.ProcedureOrderIdColumn.ColumnName]);
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

        // Author: Muhammad Arshad
        // Created Date: 22/03/2016
        //OverView: This function will delete Procedure Order
        public string deleteProcedureOrder(string procedureOrderId)
        {
            try
            {
                string result = "";
                DSProcedureOrder dsProcedure = null, dsProcedureProblems = null;
                BLObject<string> obj;
                //Start 21-03-2016 Humaira Yousaf
                obj = BLLClinicalObj.DeleteProcedureOrder(procedureOrderId);
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
        // Author: Farooq Ahmad
        // Created Date: 18/03/2016
        //OverView: This function will save Procedure Order Problem
        //public string saveProcedureOrderProblem(List<ProcedureOrderProblemModel> model)
        //{
        //    try
        //    {
        //        DSProcedureOrder dsProcedureOrder = new DSProcedureOrder();

        //        foreach (var m in model)
        //        {
        //            DSProcedureOrder.ProcedureOrderProblemRow dr = dsProcedureOrder.ProcedureOrderProblem.NewProcedureOrderProblemRow();
        //            dr.ProcedureOrderId = MDVUtility.ToInt64(m.ProcedureOrderId);
        //            dr.ProblemId = MDVUtility.ToInt32(m.ProblemId);
        //            dr.Comments = string.Empty;
        //            dr.IsActive = true;
        //            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //            dr.CreatedOn = DateTime.Now;
        //            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //            dr.ModifiedOn = DateTime.Now;
        //            dsProcedureOrder.ProcedureOrderProblem.AddProcedureOrderProblemRow(dr);
        //        }
        //        #region Database Insertion
        //        BLObject<DSProcedureOrder> obj = BLLClinicalObj.InsertUpdateProcedureOrderProblems(dsProcedureOrder);
        //        dsProcedureOrder = obj.Data;

        //        if (obj.Data != null)
        //        {

        //            Int64 procedureOrderId = MDVUtility.ToInt64(dsProcedureOrder.Tables[dsProcedureOrder.ProcedureOrderProblem.TableName].Rows[0][dsProcedureOrder.ProcedureOrderProblem.ProcedureOrderIdColumn.ColumnName]);

        //            var response = new
        //            {
        //                status = true,
        //                message = Common.AppPrivileges.Save_Message,
        //                procedureOrderId = procedureOrderId,
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
        /// Method Name: attachProcedureOrderWithNotes
        /// Author: Ahmad Raza
        /// Description: attaching procedure order with notes
        /// </summary>
        /// <param name="procedureOrderId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string attachProcedureOrderWithNotes(string procedureOrderIDs, long notesId)
        {
            try
            {
                DSProcedureOrder dsProcedureOrder = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(procedureOrderIDs)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<DSProcedureOrder> obj = BLLClinicalObj.attachProcedureOrderWithNotes(procedureOrderIDs, notesId);
                    if (obj.Data != null)
                    {
                        dsProcedureOrder = obj.Data;
                        var response = new
                        {
                            status = true,
                            ProcedureOrderTotalCount = dsProcedureOrder.Tables[dsProcedureOrder.ProcedureOrder.TableName].Rows.Count,
                            ProcedureOrderCount = dsProcedureOrder.Tables[dsProcedureOrder.ProcedureOrder.TableName].Rows.Count,
                            ProcedureOrderLoad_JSON = MDVUtility.JSON_DataTable(dsProcedureOrder.Tables[dsProcedureOrder.ProcedureOrder.TableName]),
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        ///  Method Name: detachProcedureOrderFromNotes
        ///  Author: Ahmad Raza
        ///  Description: Detaching procedure order from notes
        /// </summary>
        /// <param name="procedureOrderId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string detachProcedureOrderFromNotes(string procedureOrderIDs, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(procedureOrderIDs)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachProcedureOrderFromNotes(procedureOrderIDs, notesId);
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
        /// <summary>
        /// Method Name: previewProcedureOrder
        /// Author : Humaira Yousaf
        /// Created Date: 22-03-2016
        /// Description: Creates PDF to view Procedure Order
        /// </summary>
        /// <param name="model" type="ProcedureOrderModel">model</param>
        public string previewProcedureOrder(ProcedureOrderModel model)
        {
            try
            {
                BLObject<byte[]> obj = BLLClinicalObj.previewProcedureOrder(MDVUtility.ToInt64(model.ProcedureOrderId), MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ProcedureOrderHTML = Convert.ToBase64String(obj.Data),
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #region"Procedure Order Test"
        public string insertUpdateProcedureOrderTest(Int64 ProcedureId, List<object> lstObjects)
        {
            try
            {
                #region ProcedureOrderTest

                DSProcedureOrder dsProcedureOrder = new DSProcedureOrder();

                BLObject<DSProcedureOrder> objProcedureOrderTest = BLLClinicalObj.LoadProcedureOrderTest(MDVUtility.ToInt64(ProcedureId), 0, 0, "1", "2000");
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string deleteProcedureOrderTest(string procedureOrderTestId, string patientId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.deleteProcedureOrderTest(procedureOrderTestId, patientId);
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
        /// Method Name: insertUpdateFavListSetting
        /// Author : Humaira Yousaf
        /// Created Date: 12-04-2016
        /// Description: Inserts/updates favorite list setting
        /// </summary>
        /// <param name="isFavListOpened" type="bool">isFavListOpened</param>
        //private string insertUpdateFavListSetting(bool isFavListOpened)
        //{
        //    try
        //    {

        //        DSUsers dsDefaultsetting = new DSUsers();

        //        DSUsers dsUser1 = new DSUsers();
        //        BLObject<DSUsers> objUser = null;
        //        BLObject<DSUsers> obj = null;

        //        objUser = BLLAdminSecurityObj.LoadEntityUserOption(ref dsUser1, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), MDVSession.Current.EntityId);
        //        dsDefaultsetting = objUser.Data;

        //        if (dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows.Count == 0)
        //        {
        //            DSUsers.EntityUserOptionRow dr = dsDefaultsetting.EntityUserOption.NewEntityUserOptionRow();
        //            dr.UserId = AppConfig.AppUserId;
        //            dr.EntityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
        //            dr.EntityRegCode = MDVSession.Current.EntityRegCode;
        //            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //            dr.IsDefault = false;
        //            dr.IsActive = true;
        //            dr.CreatedOn = DateTime.Now;
        //            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //            dr.ModifiedOn = DateTime.Now;
        //            dr.ProcedureFavList = isFavListOpened;

        //            dsDefaultsetting.EntityUserOption.AddEntityUserOptionRow(dr);
        //            obj =BLLAdminSecurityObj.InsertDefaultSettings(dsDefaultsetting);

        //        }
        //        else
        //        {
        //            foreach (DSUsers.EntityUserOptionRow dr in dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows)
        //            {
        //                dr.UserId = AppConfig.AppUserId;
        //                dr.EntityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
        //                dr.EntityRegCode = MDVSession.Current.EntityRegCode;
        //                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                dr.IsDefault = false;
        //                dr.IsActive = true;
        //                dr.CreatedOn = DateTime.Now;
        //                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //                dr.ModifiedOn = DateTime.Now;
        //                dr.ProcedureFavList = isFavListOpened;
        //            }
        //            obj =BLLAdminSecurityObj.UpdatDefaultSettings(dsDefaultsetting);
        //        }
        //        if (obj != null)
        //        {
        //            DashBoardSetting.SetApplicationConfig(dsDefaultsetting);
        //            var response = new
        //            {
        //                status = true,
        //            };
        //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = ""
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
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

        internal string getOrdersForSoap(string orderIDs, long patientId,long notesId)
        {
            try
            {

                DSProcedureOrder dsOrderSoap = null;
                DSProblemLists dsProblemSoap = null;
                BLObject<DSProcedureOrder> obj = BLLClinicalObj.loadProcedureOrdersForSoap(orderIDs, patientId);
                BLObject<DSProblemLists> objPrb = BLLClinicalObj.attachProcedureProblemsWithNoteForSoap(orderIDs, notesId);

                dsOrderSoap = obj.Data;
                dsProblemSoap = objPrb.Data;
                if (obj.Data != null)
                {
                    if (dsOrderSoap.Tables[dsOrderSoap.ProcedureOrder.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ProcedureOrderSoapCount = dsOrderSoap.Tables[dsOrderSoap.ProcedureOrder.TableName].Rows.Count,
                            ProcedureOrderSoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.ProcedureOrder.TableName]),
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
                            ProcedureOrderSoapCount = 0,
                            ProcedureOrderSoap_JSON = MDVUtility.JSON_DataTable(dsOrderSoap.Tables[dsOrderSoap.ProcedureOrder.TableName]),
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
    }
}