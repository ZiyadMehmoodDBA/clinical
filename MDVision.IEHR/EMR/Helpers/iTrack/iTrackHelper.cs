/// Author: Zia Mehmood
/// Date : March 26, 2018

using System;
using System.Collections.Generic;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Model.AuditableEvents;
using MDVision.Model.iTrack;
using System.Data;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.CQM;
using MDVision.DataAccess.DAL.Reports;
using System.Xml.Serialization;
using System.IO;
using MDVision.Common.Utilities;
using MDVision.Business.BCommon;
using Newtonsoft.Json;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.EMR.Helpers.Clinical.iTrack
{
    public class iTrackHelper
    {
        private BLLiTrack BLLiTrackObj = null;
        private BLLCQM BLLCQMObj = null;
        public iTrackHelper()
        {
            BLLCQMObj = new BLLCQM();
            BLLiTrackObj = new BLLiTrack();
        }
        private static iTrackHelper _instance = null;
        public static iTrackHelper Instance()
        {
            if (_instance == null)
                _instance = new iTrackHelper();
            return _instance;
        }


        #region iTrack Methods
        public string LoadMIPSKPIs(Dashboard model)    
        {

            List<Dashboard> objList_iTrack = new List<Dashboard>();
            try
            {
                DALCQM objCQM = new DALCQM();
                BLObject<DSCQM> obj;
                DSCQM CQMds = new DSCQM();
                DataColumn COLUMN = new DataColumn();
                DataColumn COLUMN2 = new DataColumn();
                DataColumn COLUMN3 = new DataColumn();
              
                // i guess it should load Provider's prefrence instead like this. 
                model.DateFrom = string.IsNullOrEmpty(model.DateFrom) ? "01/01/2018" : model.DateFrom;
                model.DateTo = string.IsNullOrEmpty(model.DateTo) ? "12/31/2018" : model.DateTo;
                //Cheers

                //// DSCQM CQMds= objCQM.Load_CQM(int.Parse(model.ProviderId),null, "01 / 01 /" +model.Year, "31 / 12 /" + model.Year,"",0,"",null,0,"",null,null,null,null,0,null,null,0,0,0,false);
                if (model.TIN != null)
                {
                    obj = BLLCQMObj.Load_CQM_iTrackDashboard(MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToStr(model.DateFrom), MDVUtility.ToStr(model.DateTo), 4,model.TIN);
                    //CQMds = objCQM.Load_CQM(MDVUtility.ToInt32(model.ProviderId), null, model.DateFrom, model.DateTo, null, 0, null, model.TIN);
                }
                else
                {
                    obj = BLLCQMObj.Load_CQM_iTrackDashboard(MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToStr(model.DateFrom), MDVUtility.ToStr(model.DateTo),  4, null);
                    //CQMds = objCQM.Load_CQM(MDVUtility.ToInt32(model.ProviderId), null, model.DateFrom, model.DateTo);
                }
                var dsCqm = obj.Data;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                #region CQM Data Table
                DataTable dtCQM = new DataTable();
                COLUMN = new DataColumn();
                COLUMN.ColumnName = "KeyName";
                COLUMN.DataType = typeof(int);
                COLUMN2 = new DataColumn();
                COLUMN2.ColumnName = "Value";
                COLUMN2.DataType = typeof(float);
                COLUMN3 = new DataColumn();
                COLUMN3.ColumnName = "Value2";
                COLUMN3.DataType = typeof(float);
               
                dtCQM.Columns.Add(COLUMN);
                dtCQM.Columns.Add(COLUMN2);
                dtCQM.Columns.Add(COLUMN3);
               
                if (dsCqm.Tables[dsCqm.CQM.TableName].Rows.Count > 0)
                {
                    //string[] strArry = model.AppointmentStatusIds.Split(',');
                    foreach (DataRow dr in dsCqm.Tables[dsCqm.CQM.TableName].Rows)
                    {
                        DataRow Dr = dtCQM.NewRow();
                        Dr[0] = dr["ID"];
                        Dr[1] = dr["Optional2"];
                        Dr[2] = dr["BonusPoint"];
                      
                        dtCQM.Rows.Add(Dr);
                    }
                }
                else
                {
                    DataRow Dr = dtCQM.NewRow();
                    Dr[0] = 0;
                    dtCQM.Rows.Add(Dr);
                }
                #endregion CQM Data Table
                #region CQM Data Table
                DALReports objDalReport = new DALReports();
                DSReports dsReports = null;
                BLObject<List<IndvidualProvider>> objDetail = null;
                if (model.GroupId != null)
                {
                    dsReports = objDalReport.LoadACIGroupData(0, 0, 0, model.DateFrom, model.DateTo, "", "MU Stage 3 Report", model.GroupId);
                    objDetail = BLLiTrackObj.LoadMIPSSummaryGroupDetailData(MDVUtility.ToInt64(model.GroupId));
                }
                else
                {
                    dsReports = objDalReport.LoadMUReportData(MDVUtility.ToInt32(model.ProviderId), 0, 0, model.DateFrom, model.DateTo, "", "MU Stage 3 Report");
                    objDetail = BLLiTrackObj.LoadMIPSSummaryIndividualProviderDetailData(MDVUtility.ToInt64(model.ProviderId));
                }
                DataTable dtMu = new DataTable();
                COLUMN = new DataColumn();
                COLUMN.ColumnName = "KeyName";
                COLUMN.DataType = typeof(int);
                COLUMN2 = new DataColumn();
                COLUMN2.ColumnName = "Value";
                COLUMN2.DataType = typeof(float);
                COLUMN3 = new DataColumn();
                COLUMN3.ColumnName = "Value2";
                COLUMN3.DataType = typeof(float);
                dtMu.Columns.Add(COLUMN);
                dtMu.Columns.Add(COLUMN2);
                dtMu.Columns.Add(COLUMN3);
                if (dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName].Rows.Count > 0)
                {
                    //string[] strArry = model.AppointmentStatusIds.Split(',');
                    foreach (DataRow dr in dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName].Rows)
                    {
                        DataRow Dr = dtMu.NewRow();
                        Dr[0] = dr["ID"];
                        Dr[1] = dr["PerfromanceRate1"];
                        Dr[2] = 0;
                        dtMu.Rows.Add(Dr);
                    }
                }
                else
                {
                    DataRow Dr = dtMu.NewRow();
                    Dr[0] = 0;
                    dtMu.Rows.Add(Dr);
                }
                #endregion CQM Data Table
                objList_iTrack = BLLiTrackObj.LoadMIPSKPIs(model, dtCQM, dtMu);
                if (objList_iTrack != null)
                {
                    if (objList_iTrack.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MIPSKPIsCount = objList_iTrack.Count,
                            iTotalDisplayRecords = objList_iTrack[0].RecordCount,
                            MIPSKPIsLoad_JSON = js.Serialize(objList_iTrack),
                            MIPSProviderDetailData = objList_iTrack != null ? js.Serialize(objDetail.Data) : "[]",
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogCompCount = 0,

                            ActivityLogCompLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogCompCount = 0,

                        ActivityLogCompLoad_JSON = "[]",
                        Message = "",
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

        public string GetAdminPrefPI_DatesByProviderid(Dashboard model)
        {
            List<Dashboard> objList_iTrack = new List<Dashboard>();
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                if (!string.IsNullOrEmpty(model.ProviderId))
                {
                    BLObject<IndvidualProvider> objDetail = BLLiTrackObj.LoadIndividualProviderData(MDVUtility.ToInt64(model.ProviderId));
                    if (objDetail != null)
                    {
                        var response = new
                        {
                            status = true,
                            IsFullYear = (!string.IsNullOrEmpty(objDetail.Data.IsFullYear)) ? objDetail.Data.IsFullYear : "",
                            StartDate = (!string.IsNullOrEmpty(objDetail.Data.StartDate)) ? objDetail.Data.StartDate : "",
                            EndDate = (!string.IsNullOrEmpty(objDetail.Data.EndDate)) ? objDetail.Data.EndDate : "",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = AppPrivileges.No_Record_Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = AppPrivileges.No_Record_Message,
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
        public string GetAdminPrefPI_DatesByGroupid(Dashboard model)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                if (!string.IsNullOrEmpty(model.GroupId))
                {
                    BLObject<IndvidualProvider> objDetail = BLLiTrackObj.LoadGroupDetailData(MDVUtility.ToInt64(model.GroupId));
                    if (objDetail != null)
                    {
                        var response = new
                        {
                            status = true,
                            IsFullYear = (!string.IsNullOrEmpty(objDetail.Data.IsFullYear)) ? objDetail.Data.IsFullYear : "",
                            StartDate = (!string.IsNullOrEmpty(objDetail.Data.StartDate)) ? objDetail.Data.StartDate : "",
                            EndDate = (!string.IsNullOrEmpty(objDetail.Data.EndDate)) ? objDetail.Data.EndDate : "",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = AppPrivileges.No_Record_Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = AppPrivileges.No_Record_Message,
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
        public string LoadGroupQualityMeasures(Dashboard model)
        {

            List<Dashboard> objList_iTrack = new List<Dashboard>();
            try
            {
                DALCQM objCQM = new DALCQM();
                BLObject<DSCQM> obj;
                DSCQM CQMds = new DSCQM();
              
                model.DateFrom = string.IsNullOrEmpty(model.DateFrom) ? "01/01/2018" : model.DateFrom;
                model.DateTo = string.IsNullOrEmpty(model.DateTo) ? "12/31/2018" : model.DateTo;
                if (model.TIN != null)
                {
                    obj = BLLCQMObj.Load_CQM_iTrackDashboard(MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToStr(model.DateFrom), MDVUtility.ToStr(model.DateTo), 0, model.TIN);
                }
                else
                {
                    obj = BLLCQMObj.Load_CQM_iTrackDashboard(MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToStr(model.DateFrom), MDVUtility.ToStr(model.DateTo), 0, null);
                }
                var dsCqm = obj.Data;
               
                if (obj.Data != null)
                {
                    if (dsCqm.Tables[dsCqm.CQM.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            BatchClinicalQualityMeasureCount = dsCqm.Tables[dsCqm.CQM.TableName].Rows.Count,
                            iTotalDisplayRecords = dsCqm.Tables[dsCqm.CQM.TableName].Rows.Count,
                            dsCqm.CQM.Rows.Count,
                            BatchClinicalQualityMeasureLoad_JSON = MDVUtility.JSON_DataTable(dsCqm.Tables[dsCqm.CQM.TableName]),
                            ProviderId = model.ProviderId,
                            DateFrom = model.DateFrom,
                            DateTo = model.DateTo
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            MessageCount = 0,
                            iTotalDisplayRecords = 0,
                            Message = "No Records Found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogCompCount = 0,

                        ActivityLogCompLoad_JSON = "[]",
                        Message = "",
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
        public string SelectIndividualProvider(IndvidualProvider model)
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            List<IndvidualProvider> objList_IProviderDetail = new List<IndvidualProvider>();
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                BLObject<List<IndvidualProvider>> obj = BLLiTrackObj.SelectIndividualProvider(model);
                if (obj.Data != null)
                {
                    objList_IProvider = obj.Data;
                    if (objList_IProvider != null && objList_IProvider.Count > 0)
                    {
                        BLObject<List<IndvidualProvider>> objDetail = BLLiTrackObj.LoadIndividualProviderDetail(MDVUtility.ToInt64(model.ObjectId), 1, 15);
                        if (objDetail != null)
                            objList_IProviderDetail = objDetail.Data;
                        var response = new
                        {
                            status = true,
                            iTotalDisplayRecords = objList_IProvider[0].RecordCount,
                            IndividualProCount = objList_IProvider.Count,
                            IndividualProCountLoad_JSON = js.Serialize(objList_IProvider),
                            IndividualProDetailLoad_JSON = js.Serialize(objList_IProviderDetail),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            iTotalDisplayRecords = 0,
                            Message = Common.AppPrivileges.No_Record_Message,
                            IndividualProCountLoad_JSON = objList_IProvider,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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
        public string LoadIndividualProvider(IndvidualProvider model)
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            List<IndvidualProvider> objList_IProviderDetail = new List<IndvidualProvider>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                BLObject<List<IndvidualProvider>> obj = BLLiTrackObj.LoadIndividualProvider(model);


                if (obj.Data != null)
                {
                    objList_IProvider = obj.Data;

                    if (objList_IProvider != null && objList_IProvider.Count > 0)
                    {
                        BLObject<List<IndvidualProvider>> objDetail = BLLiTrackObj.LoadIndividualProviderDetail(MDVUtility.ToInt64(model.ObjectId), 1, 15);

                        if (objDetail != null)
                        {
                            objList_IProviderDetail = objDetail.Data;
                        }
                        var response = new
                        {
                            status = true,
                            iTotalDisplayRecords = objList_IProvider[0].RecordCount,
                            IndividualProCount = objList_IProvider.Count,
                            IndividualProCountLoad_JSON = js.Serialize(objList_IProvider),
                            IndividualProDetailLoad_JSON = js.Serialize(objList_IProviderDetail),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            iTotalDisplayRecords = 0,
                            Message = Common.AppPrivileges.No_Record_Message,
                            IndividualProCountLoad_JSON = objList_IProvider,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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

        public string LoadPracticLookup(string EntityId)
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_IProvider = BLLiTrackObj.LoadPracticLookup(EntityId);
                if (objList_IProvider != null)
                {
                    if (objList_IProvider.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = objList_IProvider.Count,
                            iTotalDisplayRecords = objList_IProvider[0].RecordCount,
                            IndividualProCountLoad_JSON = js.Serialize(objList_IProvider)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = 0,
                            iTotalDisplayRecords = 0,
                            ActivityLogChangesLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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
        public string LoadGroupCatLookup()
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_IProvider = BLLiTrackObj.LoadGroupCatLookup();
                if (objList_IProvider != null)
                {
                    if (objList_IProvider.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = objList_IProvider.Count,
                            iTotalDisplayRecords = objList_IProvider[0].RecordCount,
                            IndividualProCountLoad_JSON = js.Serialize(objList_IProvider)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = 0,
                            iTotalDisplayRecords = 0,
                            ActivityLogChangesLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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
        public string LoadIneligibleReasonLookup()
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_IProvider = BLLiTrackObj.LoadIneligibleReasonLookup();
                if (objList_IProvider != null)
                {
                    if (objList_IProvider.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = objList_IProvider.Count,
                            iTotalDisplayRecords = objList_IProvider[0].RecordCount,
                            IndividualProCountLoad_JSON = js.Serialize(objList_IProvider)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = 0,
                            iTotalDisplayRecords = 0,
                            ActivityLogChangesLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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
        public string LoadParticipatingReasonLookup()
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_IProvider = BLLiTrackObj.LoadParticipatingReasonLookup();
                if (objList_IProvider != null)
                {
                    if (objList_IProvider.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = objList_IProvider.Count,
                            iTotalDisplayRecords = objList_IProvider[0].RecordCount,
                            IndividualProCountLoad_JSON = js.Serialize(objList_IProvider)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = 0,
                            iTotalDisplayRecords = 0,
                            ActivityLogChangesLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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

        public string LoadGroupNameLookup()
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_IProvider = BLLiTrackObj.LoadGroupNameLookup();
                if (objList_IProvider != null)
                {
                    if (objList_IProvider.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = objList_IProvider.Count,
                            iTotalDisplayRecords = objList_IProvider[0].RecordCount,
                            IndividualProCountLoad_JSON = js.Serialize(objList_IProvider)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = 0,
                            iTotalDisplayRecords = 0,
                            ActivityLogChangesLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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
        public string SaveMIPSPreferencesGroup(IndvidualProvider model)
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(IndvidualProvider));
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, model);
                string myxml = textWriter.ToString();
                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_IProvider = BLLiTrackObj.SaveMIPSPreferencesGroup(model, myxml);
                if (objList_IProvider != null)
                {
                    if (objList_IProvider.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = objList_IProvider.Count,
                            Message = Common.AppPrivileges.Save_Message
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = 0,
                            iTotalDisplayRecords = 0,
                            ActivityLogChangesLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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
        public string SaveMIPSPreferencesIndvidual(IndvidualProvider model)
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(IndvidualProvider));
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, model);
                string myxml = textWriter.ToString();
                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_IProvider = BLLiTrackObj.SaveMIPSPreferencesIndvidual(model, myxml);
                if (objList_IProvider != null)
                {
                    if (objList_IProvider.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = objList_IProvider.Count,
                            Message = Common.AppPrivileges.Save_Message
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = 0,
                            iTotalDisplayRecords = 0,
                            ActivityLogChangesLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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
        public string UpdateMIPSPreferencesIndvidual(IndvidualProvider model)
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(IndvidualProvider));
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, model); 
                string myxml = textWriter.ToString();
                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                BLObject<string> obj = BLLiTrackObj.UpdateMIPSPreferencesIndvidual(model, myxml);
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
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = obj.Data,
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
        public string UpdateiTrackReportingType(IndvidualProvider model)
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            try
            {
              
                
                BLObject<string> obj = BLLiTrackObj.UpdateiTrackReportingType(model);
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
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = obj.Data,
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
        public string ActiveInActiveMIPSPreferencesGroup(IndvidualProvider model)
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            try
            {
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_IProvider = BLLiTrackObj.ActiveInActiveMIPSPreferencesGroup(model);
                if (objList_IProvider != null)
                {
                    if (objList_IProvider.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = objList_IProvider.Count,
                            Message = Common.AppPrivileges.Update_Message
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = 0,
                            iTotalDisplayRecords = 0,
                            ActivityLogChangesLoad_JSON = "[]",
                            Message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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
        public string UpdateMIPSPreferencesGroup(IndvidualProvider model)
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(IndvidualProvider));
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, model);
                string myxml = textWriter.ToString();
                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                BLObject<string> obj = BLLiTrackObj.UpdateMIPSPreferencesGroup(model, myxml);
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
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = obj.Data,
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
        public string LoadMIPSProvider(IndvidualProvider model)
        {

            List<IndvidualProvider> objList_IProvider = new List<IndvidualProvider>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_IProvider = BLLiTrackObj.LoadMIPSProvider(model);
                if (objList_IProvider != null)
                {
                    if (objList_IProvider.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = objList_IProvider.Count,
                            iTotalDisplayRecords = objList_IProvider[0].RecordCount,
                            IndividualProCountLoad_JSON = js.Serialize(objList_IProvider)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = 0,
                            iTotalDisplayRecords = 0,
                            IndividualProCountLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        IndividualProCount = 0,
                        iTotalDisplayRecords = 0,
                        IndividualProCountLoad_JSON = "[]",
                        Message = "",
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
        public string SearchMIPSGroupPreferences(IndvidualProvider model)
        {

            MIPSGrouupPreferenceList objList_IProvider = new MIPSGrouupPreferenceList();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_IProvider = BLLiTrackObj.SearchMIPSGroupPreferences(model);
                if (objList_IProvider != null)
                {
                    if (objList_IProvider.Groups.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = objList_IProvider.Groups.Count,
                            iTotalDisplayRecords = objList_IProvider.Groups[0].RecordCount,
                            IndividualProCountLoad_JSON = js.Serialize(objList_IProvider)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = 0,
                            iTotalDisplayRecords = 0,
                            IndividualProCountLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        IndividualProCount = 0,
                        iTotalDisplayRecords = 0,
                        IndividualProCountLoad_JSON = "[]",
                        Message = "",
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
        public string SearchMIPSProviderPreferences(IndvidualProvider model)
        {

            MIPSGrouupPreferenceList objList_IProvider = new MIPSGrouupPreferenceList();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_IProvider = BLLiTrackObj.SearchMIPSProviderPreferences(model);
                if (objList_IProvider != null)
                {
                    if (objList_IProvider.Groups.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = objList_IProvider.Groups.Count,
                            iTotalDisplayRecords = objList_IProvider.Groups[0].RecordCount,
                            IndividualProCountLoad_JSON = js.Serialize(objList_IProvider)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = 0,
                            iTotalDisplayRecords = 0,
                            IndividualProCountLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        IndividualProCount = 0,
                        iTotalDisplayRecords = 0,
                        IndividualProCountLoad_JSON = "[]",
                        Message = "",
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
        public string SelectMIPSGroupPreferences(IndvidualProvider model)
        {

            MIPSGrouupPreferenceList objList_IProvider = new MIPSGrouupPreferenceList();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_IProvider = BLLiTrackObj.SelectMIPSGroupPreferences(model);
                if (objList_IProvider != null)
                {
                    if (objList_IProvider.Groups.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = objList_IProvider.Groups.Count,
                            iTotalDisplayRecords = objList_IProvider.Groups[0].RecordCount,
                            IndividualProCountLoad_JSON = js.Serialize(objList_IProvider)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            IndividualProCount = 0,
                            iTotalDisplayRecords = 0,
                            IndividualProCountLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        IndividualProCount = 0,
                        iTotalDisplayRecords = 0,
                        IndividualProCountLoad_JSON = "[]",
                        Message = "",
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
        public string loadAcitivityLogChanges(ActivityLog model)
        {

            List<ActivityLog> objList_ActivityLog = new List<ActivityLog>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_ActivityLog = null;// BLLActivityLogObj.loadAcitivityLogChanges(model);
                if (objList_ActivityLog != null)
                {
                    if (objList_ActivityLog.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = objList_ActivityLog.Count,
                            iTotalDisplayRecords = objList_ActivityLog[0].RecordCount,
                            ActivityLogChangesLoad_JSON = js.Serialize(objList_ActivityLog)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = 0,
                            iTotalDisplayRecords = 0,
                            ActivityLogChangesLoad_JSON = "[]",
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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
    }
}