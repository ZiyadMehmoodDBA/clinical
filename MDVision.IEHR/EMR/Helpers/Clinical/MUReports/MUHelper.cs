using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.EMR.Model.Medical;
using System.Text.RegularExpressions;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using MDVision.IEHR.Controls.Patient.Demographics;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using System.Threading;
using MDVision.IEHR.Common;
using Newtonsoft.Json.Linq;
using MDVision.IEHR.EMR.Model.Reports;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical.MUReports
{
    public class MUHelper
    {
         private BLLReports BLLReportObj = null;
         public MUHelper()
        {
            BLLReportObj = new BLLReports();
        }

        private static MUHelper _instance = null;
        public static MUHelper Instance()
        {
            if (_instance == null)
                _instance = new MUHelper();
            return _instance;
        }
        public string LoadMUReportData(MUModel model)
        {
            try
            {
                DSReports dsReports = null;
                BLObject<DSReports> obj = null;
                if (model.Provider != null && MDVUtility.ToInt64(model.Provider) > 0)
                {
                    obj = BLLReportObj.LoadMUReportData(MDVUtility.ToInt64(model.Provider), MDVUtility.ToInt64(model.PatientID), MDVUtility.ToInt32(model.reportType), model.FromDate, model.ToDate, MDVUtility.ToStr(model.MUID), MDVUtility.ToStr(model.ReportName));
                }
                /*else
                {
                    obj = BLLClinicalObj.loadProcedures(MDVUtility.ToInt32(model.ProcedureId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NotesId), "1", MDVUtility.ToStr(model.IsActive), MDVUtility.ToInt32(0), MDVUtility.ToInt32(0), "1", "0");
                }*/

                dsReports = obj.Data;
                if (obj.Data != null)
                {
                    //int ProcedureCount = 0;
                    //if (model.procedureDetailModel != null && model.procedureDetailModel.Count > 0 && model.procedureDetailModel[0].IsActive.Equals("1")) 
                    if (dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName].Rows.Count == 0)
                    {
                        //if (model.procedureDetailModel[0].IsActive.Equals("1"))
                        /*if (model.procedureDetailModel != null && model.procedureDetailModel.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(model.procedureDetailModel[0].IsActive) && model.procedureDetailModel[0].IsActive.Equals("1"))
                            {
                                obj = BLLClinicalObj.loadProcedures(MDVUtility.ToInt32(model.procedureDetailModel[0].ProcedureId), MDVUtility.ToInt64(model.procedureDetailModel[0].PatientId), MDVUtility.ToInt64(model.procedureDetailModel[0].NotesId), "0", "0");
                            }
                        }
                        else
                        {
                            obj = BLLClinicalObj.loadProcedures(MDVUtility.ToInt32(model.procedureDetailModel[0].ProcedureId), MDVUtility.ToInt64(model.procedureDetailModel[0].PatientId), MDVUtility.ToInt64(model.procedureDetailModel[0].NotesId), "0", "1");
                        }

                        if (obj.Data != null)
                        {
                            DSProcedures dsProceduresInActive = obj.Data;
                            ProcedureCount = dsProceduresInActive.Tables[dsProceduresInActive.Procedures.TableName].Rows.Count;
                        }*/
                    }
                    else
                    {
                        //ProcedureCount = dsProcedures.Tables[dsProcedures.Procedures.TableName].Rows.Count;
                    }
                    var response = new
                    {
                        status = true,
                        //ProcedureTotalCount = ProcedureCount,
                        MURecordCount = dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName].Rows.Count,
                        MU_JSON = MDVUtility.JSON_DataTable(dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName]),
                        MU_JSON_PatientWise = MDVUtility.JSON_DataTable(dsReports.Tables[dsReports.MU_AutomatedMeasurePatientWise.TableName]),
                        //ProcedureHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsReports.Tables[dsReports.ProcedureHistory.TableName]),
                        iTotalDisplayRecords = (dsReports.MU_AutomatedMeasure.Rows.Count > 0) ? dsReports.MU_AutomatedMeasure.Rows[0][dsReports.MU_AutomatedMeasure.PatientIDColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        MURecordCount = 0,
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