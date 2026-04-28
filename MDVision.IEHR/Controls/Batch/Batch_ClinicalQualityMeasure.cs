using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using com.itextpdf.text.pdf;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.Model.Batch.CQM;
using MDVision.IEHR.Controls.Batch.CQM;
using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using System.Text;
using MDVision.DataAccess.DAL.Reports;
using MDVision.Model.iTrack;

namespace MDVision.IEHR.Controls.Batch
{
    public class Batch_ClinicalQualityMeasure
    {
        private BLLCQM BLLCQMObj = null;
        private BLLiTrack BLLiTrackObj = null;
        public Batch_ClinicalQualityMeasure()
        {
            BLLCQMObj = new BLLCQM();
            BLLiTrackObj = new BLLiTrack();
        }

        #region Singleton
        private static Batch_ClinicalQualityMeasure _obj = null;
        public static Batch_ClinicalQualityMeasure Instance()
        {
            if (_obj == null)
                _obj = new Batch_ClinicalQualityMeasure();
            return _obj;
        }

        #endregion

        private List<string> _listPatients = new List<string>();
        private string providerName = "";

        #region CRUD

        private string LoadCqm(string fieldsJson, string CQMId, string PageNumber, string rpp)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);

                //var providerId = searchedfieldsJson["ddlprovider"] == "" ? null : searchedfieldsJson["ddlprovider"];
                //var dtpDateFrom = searchedfieldsJson["dtpDateFrom"] == "" ? null : searchedfieldsJson["dtpDateFrom"];
                //var dtpDateTo = searchedfieldsJson["dtpDateTo"] == "" ? null : searchedfieldsJson["dtpAppointmentDateTo"];
                //providerName = searchedfieldsJson["ddlprovider_text"] == "" ? null : searchedfieldsJson["ddlprovider_text"];
                var ProblemsList = ser.Deserialize<CQMProblemSearch>(MDVUtility.ToStr(fieldsJson));
                BLObject<DSCQM> obj;
                BLObject<DSCQM> objMips;

                if (string.IsNullOrEmpty(ProblemsList.ProviderId) && string.IsNullOrEmpty(ProblemsList.DateFrom) && string.IsNullOrEmpty(ProblemsList.DateTo))
                {
                    var response = new
                    {
                        status = false,
                        MessageCount = 0,
                        iTotalDisplayRecords = 0,
                        Message = "Please select search criteria."
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    if (!string.IsNullOrEmpty(ProblemsList.ProviderId))
                    {
                        //dtpDateFrom = string.IsNullOrEmpty(dtpDateFrom) ? "01/01/2013" : dtpDateFrom;
                        //dtpDateTo = string.IsNullOrEmpty(dtpDateTo) ? DateTime.Now.ToShortDateString() : dtpDateTo;
                        //dtpDateTo = string.IsNullOrEmpty(dtpDateTo) ? "12/31/2013" : dtpDateTo;

                        if (ProblemsList.Problems.Count > 0)
                        {
                            ProblemsList.ProblemListXML = MDVUtility.GetXmlOfObject(typeof(List<ProblemsList>), ProblemsList.Problems);
                        }

                        obj = BLLCQMObj.Load_CQM_Quality(MDVUtility.ToInt64(ProblemsList.ProviderId), MDVUtility.ToStr(ProblemsList.NPI), MDVUtility.ToStr(ProblemsList.DateFrom), MDVUtility.ToStr(ProblemsList.DateTo), MDVUtility.ToStr(ProblemsList.PatientId), 0, CQMId, MDVUtility.ToStr(ProblemsList.TIN), MDVUtility.ToInt64(ProblemsList.ProviderTypeId), MDVUtility.ToStr(ProblemsList.Address), MDVUtility.ToStr(ProblemsList.InsurancePlan), MDVUtility.ToStr(ProblemsList.EthnicityIds), MDVUtility.ToStr(ProblemsList.RaceIds), MDVUtility.ToStr(ProblemsList.AgeCondition_text), MDVUtility.ToInt64(ProblemsList.Age), MDVUtility.ToStr(ProblemsList.Sex_text), ProblemsList.ProblemListXML, MDVUtility.ToInt64(PageNumber), MDVUtility.ToInt64(rpp));
                        var dsCqm = obj.Data;
                        //if (obj.Data != null && dsCqm.Tables[dsCqm.CQM.TableName].Rows.Count > 0)
                        //{
                        //    string Measures = "";
                        //    string MeasuresPerformanceRate = "";
                        //    foreach (var item  in dsCqm.Tables[dsCqm.CQM.TableName].Rows)
                        //    {
                        //        Measures += ((MDVision.Datasets.DSCQM.CQMRow)item).CQMID + ",";
                        //        MeasuresPerformanceRate += ((MDVision.Datasets.DSCQM.CQMRow)item).PerfromanceRate1 + ",";

                        //        objMips = BLLCQMObj.getMIPSScore(MDVUtility.ToInt64(ProblemsList.ProviderId), Measures, MeasuresPerformanceRate);
                        //    }
                        //}

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
                                    ProviderId = ProblemsList.ProviderId,
                                    DateFrom = ProblemsList.DateFrom,
                                    DateTo = ProblemsList.DateTo
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
                                obj.Message
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
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

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string LoadACIData(string fieldsJson, string CQMId, string PageNumber, string rpp)
        {
            try
            {
                BLLReports BLLReportObj = new BLLReports();
                DSReports dsReports = null;
                DSReports ds_Bonus = null;
                BLObject<DSReports> obj = null;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var model = ser.Deserialize<CQMProblemSearch>(MDVUtility.ToStr(fieldsJson));
                DALReports objDalReport = new DALReports();
                if (model.ProviderId != null && MDVUtility.ToInt64(model.ProviderId) > 0)
                {
                    dsReports = objDalReport.LoadACIIndividualData(MDVUtility.ToInt32(model.ProviderId), 0, 0, model.DateFrom, model.DateTo, "", "MU Stage 3 Report");
                    ds_Bonus = objDalReport.LoadACIIndividualData_BonusMeasures();
                    if (ds_Bonus != null & ds_Bonus.MU_AutomatedMeasure.Rows.Count > 0)
                        dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName].Merge(ds_Bonus.Tables[ds_Bonus.MU_AutomatedMeasure.TableName]);      
                }
                DSReportHeader dsreportHeader = null;
                BLObject<DSReportHeader> objPrint = new BLLAdminClinical().getReportHeaderTagsValue(0, MDVUtility.ToInt64(model.ProviderId), -1, "Custom Forms");
                dsreportHeader = objPrint.Data;
                if (dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName].Rows.Count > 0)
                {
                    DSReportHeader.ReportHeaderTagsRow dr = (DSReportHeader.ReportHeaderTagsRow)dsreportHeader.Tables[dsreportHeader.ReportHeaderTags.TableName].Rows[0];

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        MURecordCount = dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName].Rows.Count,
                        MU_JSON = MDVUtility.JSON_DataTable(dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName]),
                        MU_JSON_PatientWise = MDVUtility.JSON_DataTable(dsReports.Tables[dsReports.MU_AutomatedMeasurePatientWise.TableName]),
                        iTotalDisplayRecords = (dsReports.MU_AutomatedMeasure.Rows.Count > 0) ? dsReports.MU_AutomatedMeasure.Rows[0][dsReports.MU_AutomatedMeasure.PatientIDColumn.ColumnName] : 0,


                        HeaderLogo = dr["HeaderLogo"],
                        FooterText = dr["FooterText"],
                        PatientText = dr["PatientText"],
                        ProviderText = dr["ProviderText"],
                        PracticeText = dr["PracticeText"],
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string LoadACIGroupData(string fieldsJson, string CQMId, string PageNumber, string rpp)
        {
            try
            {
                BLLReports BLLReportObj = new BLLReports();
                DSReports dsReports = null;
                DSReports ds_Bonus = null;
                BLObject<DSReports> obj = null;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var model = ser.Deserialize<CQMProblemSearch>(MDVUtility.ToStr(fieldsJson));
                DALReports objDalReport = new DALReports();
                if (model.GroupId != null && MDVUtility.ToInt64(model.GroupId) > 0)
                {
                    dsReports = objDalReport.LoadACIGroupData(0, 0, 0, model.DateFrom, model.DateTo, "", "MU Stage 3 Report", model.GroupId);
                    ds_Bonus = objDalReport.LoadACIIndividualData_BonusMeasures();
                    if (ds_Bonus != null & ds_Bonus.MU_AutomatedMeasure.Rows.Count > 0)
                        dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName].Merge(ds_Bonus.Tables[ds_Bonus.MU_AutomatedMeasure.TableName]);
                }
                if (dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName].Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        MURecordCount = dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName].Rows.Count,
                        MU_JSON = MDVUtility.JSON_DataTable(dsReports.Tables[dsReports.MU_AutomatedMeasure.TableName]),
                        MU_JSON_PatientWise = MDVUtility.JSON_DataTable(dsReports.Tables[dsReports.MU_AutomatedMeasurePatientWise.TableName]),
                        iTotalDisplayRecords = (dsReports.MU_AutomatedMeasure.Rows.Count > 0) ? dsReports.MU_AutomatedMeasure.Rows[0][dsReports.MU_AutomatedMeasure.PatientIDColumn.ColumnName] : 0,


                        HeaderLogo = "",// dr["HeaderLogo"],
                        FooterText = "",// dr["FooterText"],
                        PatientText = "",// dr["PatientText"],
                        ProviderText = "",// dr["ProviderText"],
                        PracticeText = "",// dr["PracticeText"],
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string LoadImprovementActivitiesData(string fieldsJson, string CQMId, string PageNumber, string rpp)
        {
            try
            {
                BLLReports BLLReportObj = new BLLReports();
                BLObject<DSCQM> obj = null;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var model = ser.Deserialize<CQMProblemSearch>(MDVUtility.ToStr(fieldsJson));
                if (model.ProviderId != null && MDVUtility.ToInt64(model.ProviderId) > 0)
                {
                    obj = BLLCQMObj.Load_ImprovementActivities(MDVUtility.ToInt64(model.ProviderId));
                }
                var ds = obj.Data;

                DSReportHeader dsreportHeader = null;
                BLObject<DSReportHeader> objPrint = new BLLAdminClinical().getReportHeaderTagsValue(0, MDVUtility.ToInt64(model.ProviderId), -1, "Custom Forms");
                dsreportHeader = objPrint.Data;
                if (obj.Data != null)
                {
                    DSReportHeader.ReportHeaderTagsRow dr = (DSReportHeader.ReportHeaderTagsRow)dsreportHeader.Tables[dsreportHeader.ReportHeaderTags.TableName].Rows[0];
                    var response = new
                    {
                        status = true,
                        RecordCount = ds.Tables[ds.ImprovementActivities.TableName].Rows.Count,
                        ImprovementActivities_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.ImprovementActivities.TableName]),
                        iTotalDisplayRecords = (ds.ImprovementActivities.Rows.Count > 0) ? ds.ImprovementActivities.Rows[0][ds.ImprovementActivities.IdColumn.ColumnName] : 0,

                        HeaderLogo = dr["HeaderLogo"],
                        FooterText = dr["FooterText"],
                        PatientText = dr["PatientText"],
                        ProviderText = dr["ProviderText"],
                        PracticeText = dr["PracticeText"],
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string LoadImprovementActivitiesGroupData(string fieldsJson, string CQMId, string PageNumber, string rpp)
        {
            try
            {
                BLLReports BLLReportObj = new BLLReports();
                BLObject<DSCQM> obj = null;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var model = ser.Deserialize<CQMProblemSearch>(MDVUtility.ToStr(fieldsJson));
                if (model.GroupId != null && MDVUtility.ToInt64(model.GroupId) > 0)
                {
                    obj = BLLCQMObj.Load_ImprovementActivitiesGroup(MDVUtility.ToInt64(model.GroupId));
                }
                var ds = obj.Data;

                //DSReportHeader dsreportHeader = null;
                //BLObject<DSReportHeader> objPrint = new BLLAdminClinical().getReportHeaderTagsValue(0, MDVUtility.ToInt64(model.ProviderId), -1, "Custom Forms");
                //dsreportHeader = objPrint.Data;
                if (obj.Data != null)
                {
                    //DSReportHeader.ReportHeaderTagsRow dr = (DSReportHeader.ReportHeaderTagsRow)dsreportHeader.Tables[dsreportHeader.ReportHeaderTags.TableName].Rows[0];
                    var response = new
                    {
                        status = true,
                        RecordCount = ds.Tables[ds.ImprovementActivities.TableName].Rows.Count,
                        ImprovementActivities_JSON = MDVUtility.JSON_DataTable(ds.Tables[ds.ImprovementActivities.TableName]),
                        iTotalDisplayRecords = (ds.ImprovementActivities.Rows.Count > 0) ? ds.ImprovementActivities.Rows[0][ds.ImprovementActivities.IdColumn.ColumnName] : 0,

                        HeaderLogo = "",//dr["HeaderLogo"],
                        FooterText = "",//dr["FooterText"],
                        PatientText = "",//dr["PatientText"],
                        ProviderText = "",//dr["ProviderText"],
                        PracticeText = "",//dr["PracticeText"],
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string LoadCQM_Details(string fieldsJson, string cqmid, string providerId, string dateFrom, string dateTo, bool isExport = false, bool isC1 = false)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                var CQMSearchData = new CQMProblemSearch();
                var txtAccountNumber = searchedfieldsJson["txtPatientName"] == "" ? null : searchedfieldsJson["txtPatientName"];
                var txtFullName = searchedfieldsJson["txtFullName"] == "" ? null : searchedfieldsJson["txtFullName"];
                var LastName = "";
                var FirstName = "";
                if (! string.IsNullOrEmpty(txtFullName))
                {
                     LastName = txtFullName.Split(',')[0];
                     FirstName = txtFullName.Split(',')[1];
                }
               
                var PatientId = searchedfieldsJson["PatientId"] == "" ? null : searchedfieldsJson["PatientId"];
                var ddlDenominator = searchedfieldsJson.ContainsKey("ddlDenominator") ? (searchedfieldsJson["ddlDenominator"] == "" ? null : searchedfieldsJson["ddlDenominator"]) : null;
                var ddlNumerator = searchedfieldsJson.ContainsKey("ddlNumerator") ? (searchedfieldsJson["ddlNumerator"] == "" ? null : searchedfieldsJson["ddlNumerator"]) : null;
                var ddlExclusion = searchedfieldsJson.ContainsKey("ddlExclusion") ? (searchedfieldsJson["ddlExclusion"] == "" ? null : searchedfieldsJson["ddlExclusion"]) : null;
                var ddlException = searchedfieldsJson.ContainsKey("ddlException") ? (searchedfieldsJson["ddlException"] == "" ? null : searchedfieldsJson["ddlException"]) : null;

                var measure421ArB = 0;

                if (searchedfieldsJson.ContainsKey("CQMSearchData"))
                {
                    CQMSearchData = ser.Deserialize<CQMProblemSearch>(MDVUtility.ToStr(searchedfieldsJson["CQMSearchData"]));
                    CQMSearchData.AgeCondition_text = CQMSearchData.AgeCondition_text != "- Select -" ? CQMSearchData.AgeCondition_text : null;
                    CQMSearchData.Sex_text = CQMSearchData.Sex_text != "- Select -" ? CQMSearchData.Sex_text : null;
                    if (CQMSearchData.Problems.Count > 0)
                    {
                        CQMSearchData.ProblemListXML = MDVUtility.GetXmlOfObject(typeof(List<ProblemsList>), CQMSearchData.Problems);
                    }
                }
                else
                {
                    CQMSearchData.Address = null;
                    CQMSearchData.Age = "0";
                    CQMSearchData.AgeCondition_text = null;
                    CQMSearchData.EthnicityIds = null;
                    CQMSearchData.InsurancePlan = null;
                    CQMSearchData.NPI = null;
                    CQMSearchData.ProblemListXML = null;
                    CQMSearchData.ProviderTypeId = "0";
                    CQMSearchData.RaceIds = null;
                    CQMSearchData.Sex_text = null;
                    CQMSearchData.TIN = null;
                }


                if (cqmid == "0421a")
                {
                    measure421ArB = 1;
                    cqmid = "0421";
                }
                else if (cqmid == "0421b")
                {
                    measure421ArB = 2;
                    cqmid = "0421";
                }
                else
                {
                    measure421ArB = 0;
                }
                if (isExport)
                {
                    measure421ArB = 0;
                }

                var obj = BLLCQMObj.Load_CQM_Details(MDVUtility.ToInt64(providerId), dateFrom, dateTo, PatientId, 1, cqmid, measure421ArB, MDVUtility.ToStr(CQMSearchData.TIN), MDVUtility.ToInt64(CQMSearchData.ProviderTypeId), MDVUtility.ToStr(CQMSearchData.Address), MDVUtility.ToStr(CQMSearchData.InsurancePlan), MDVUtility.ToStr(CQMSearchData.EthnicityIds), MDVUtility.ToStr(CQMSearchData.RaceIds), MDVUtility.ToStr(CQMSearchData.AgeCondition_text), MDVUtility.ToInt64(CQMSearchData.Age), MDVUtility.ToStr(CQMSearchData.Sex_text), CQMSearchData.ProblemListXML, MDVUtility.ToStr(CQMSearchData.NPI), isC1);
                var dsCqm = obj.Data;

                DataView dvCqmDetail = null;
                dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName]);

                if (!string.IsNullOrEmpty(ddlDenominator) && ddlDenominator != "All")
                {
                    dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName])
                    {
                        RowFilter = "Denominator = " + ddlDenominator
                    };
                }
                if (!string.IsNullOrEmpty(ddlExclusion) && ddlExclusion != "All")
                {
                    dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName])
                    {
                        RowFilter = "DenominatorExclusion = " + ddlExclusion
                    };
                }
                if (!string.IsNullOrEmpty(ddlException) && ddlException != "All")
                {
                    dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName])
                    {
                        RowFilter = "DenominatorException = " + ddlException
                    };
                }

                if (!string.IsNullOrEmpty(ddlNumerator) && ddlNumerator != "All")
                {
                    dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName])
                    {
                        RowFilter = "Numerator = " + ddlNumerator
                    };
                }

                if (!string.IsNullOrEmpty(ddlDenominator) && ddlDenominator != "All" &&
                    !string.IsNullOrEmpty(ddlNumerator) && ddlNumerator != "All")
                {
                    dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName])
                    {
                        RowFilter = "Denominator = " + ddlDenominator + " AND Numerator = " + ddlNumerator
                    };
                }

                if (!string.IsNullOrEmpty(ddlException) && ddlException != "All" &&
                    !string.IsNullOrEmpty(ddlExclusion) && ddlExclusion != "All")
                {
                    dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName])
                    {
                        RowFilter = "DenominatorExclusion = " + ddlExclusion + " AND DenominatorException = " + ddlException
                    };
                }
                if (!string.IsNullOrEmpty(ddlExclusion) && ddlExclusion != "All" &&
                   !string.IsNullOrEmpty(ddlNumerator) && ddlNumerator != "All")
                {
                    dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName])
                    {
                        RowFilter = "DenominatorExclusion = " + ddlExclusion + " AND Numerator = " + ddlNumerator
                    };
                }
                if (!string.IsNullOrEmpty(ddlExclusion) && ddlExclusion != "All" &&
                   !string.IsNullOrEmpty(ddlNumerator) && ddlNumerator != "All" &&
                   !string.IsNullOrEmpty(ddlException) && ddlException != "All")
                {
                    dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName])
                    {
                        RowFilter = "DenominatorExclusion = " + ddlDenominator + " AND Numerator = " + ddlNumerator + " AND DenominatorException = " + ddlException
                    };
                }
                if (ddlNumerator == "All" && ddlDenominator == "All")
                {
                    dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName]);
                }

                //if (ddlExclusion == "All" && ddlException == "All")
                //{
                //    dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName]);
                //}
                if (!string.IsNullOrEmpty(txtAccountNumber))
                {
                    dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName])
                    {
                        RowFilter = "[AccountNumber] = '" + txtAccountNumber + "'"
                    };
                }
                if (!string.IsNullOrEmpty(txtFullName))
                {
                    dvCqmDetail = new DataView(dsCqm.Tables[dsCqm.CQM_PatientsList.TableName])
                    {
                        RowFilter = "[FirstName] = '" + FirstName.Trim() + "' AND [LastName] = '" + LastName.Trim() +"'"
                    };
                }
                DataTable dtCqmDetailFiltered = null;

                if (dvCqmDetail != null)
                {
                    dtCqmDetailFiltered = dvCqmDetail.ToTable();
                }

                if (obj.Data != null)
                {
                    if (dtCqmDetailFiltered != null && dtCqmDetailFiltered.Rows.Count > 0)
                    {
                        _listPatients = dtCqmDetailFiltered.Rows.OfType<DataRow>().Select(dr => dr.Field<string>("PatientID")).ToList();
                        var response = new
                        {
                            status = true,
                            CQMID = cqmid,
                            ClinicalQualityMeasureDetailCount = dtCqmDetailFiltered.Rows.Count,
                            iTotalDisplayRecords = dtCqmDetailFiltered.Rows.Count,
                            ClinicalQualityMeasureDetailLoad_JSON = MDVUtility.JSON_DataTable(dtCqmDetailFiltered),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
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
                        obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        private string DwonloadCatagory3(string base64String, string cqmid)
        {


            string MeasureName = "";
            if (cqmid == "CMS50v3")
            {
                MeasureName = "CMS50v5 - Category III";
            }
            else if (cqmid == "0041")
            {
                MeasureName = "CMS147v6 - Category III";
            }
            else if (cqmid == "0418")
            {
                MeasureName = "CMS2v6 - Category III";
            }
            else if (cqmid == "0419")
            {
                MeasureName = "CMS68v6 - Category III";
            }
            else if (cqmid == "0421a" || cqmid == "0421")
            {
                MeasureName = "CMS69v5 - Category III";
            }
            else if (cqmid == "0043")
            {
                MeasureName = "CMS127v5 - Category III";
            }
            else if (cqmid == "0028")
            {
                MeasureName = "CMS138v5 - Category III";
            }
            else if (cqmid == "0022(A)" || cqmid == "0022(B)" || cqmid == "0022")
            {
                MeasureName = "CMS156v5 - Category III";
            }
            else if (cqmid == "0068")
            {
                MeasureName = "CMS164v5 - Category III";
            }
            else if (cqmid == "0018")
            {
                MeasureName = "CMS165v5 - Category III";
            }
            else if (cqmid == "0075(A)" || cqmid == "0075(B)" || cqmid == "0075")
            {
                MeasureName = "CMS182v6 - Category III";
            }
            else if (cqmid == "0059")
            {
                MeasureName = "CMS122v5 - Category III";
            }
            else if (cqmid == "0031")
            {
                MeasureName = "CMS125v5 - Category III";
            }
            else if (cqmid == "0712(A)" || cqmid == "0712(B)" || cqmid == "0712(C)" || cqmid == "0712")
            {
                MeasureName = "CMS160v5 - Category III";
            }
            else if (cqmid == "0056")
            {
                MeasureName = "CMS123v5 - Category III";
            }
            else if (cqmid == "0034")
            {
                MeasureName = "CMS130v5 - Category III";
            }
            else if (cqmid == "0062")
            {
                MeasureName = "CMS134v5 - Category III";
            }
            else if (cqmid == "0101")
            {
                MeasureName = "CMS139v5 - Category III";
            }
            else if (cqmid == "0065")
            {
                MeasureName = "CMS65v6 - Category III";
            }
            else if (cqmid == "2872")
            {
                MeasureName = "CMS149v5 - Category III";
            }
            else if (cqmid == "CMS22v5")
            {
                MeasureName = "CMS22v5 - Category III";
            }
            if (!string.IsNullOrEmpty(base64String))
            {
                var response = new
                {
                    status = true,
                    DownloadFile = base64String,
                    FileName = MeasureName != "" ? MeasureName : "Category III" //providerName + " - Category III"
                };
                return (JsonConvert.SerializeObject(response));

            }
            else
            {
                var response = new
                {
                    status = true,
                    Message = "Nothing To Download."
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        private string DwonloadCatagory1(List<KeyValuePair<string, string>> base64Strings, string cqmid)
        {
            if (base64Strings.Count > 0)
            {
                string MeasureName = "";
                if (cqmid == "CMS50v3")
                {
                    MeasureName = "CMS50v5 - Category I";
                }
                else if (cqmid == "0041")
                {
                    MeasureName = "CMS147v6 - Category I";
                }
                else if (cqmid == "0418")
                {
                    MeasureName = "CMS2v6 - Category I";
                }
                else if (cqmid == "0419")
                {
                    MeasureName = "CMS68v6 - Category I";
                }
                else if (cqmid == "0421a" || cqmid == "0421")
                {
                    MeasureName = "CMS69v5 - Category I";
                }
                else if (cqmid == "0043")
                {
                    MeasureName = "CMS127v5 - Category I";
                }
                else if (cqmid == "0028")
                {
                    MeasureName = "CMS138v5 - Category I";
                }
                else if (cqmid == "0022(A)" || cqmid == "0022(B)" || cqmid == "0022")
                {
                    MeasureName = "CMS156v5 - Category I";
                }
                else if (cqmid == "0068")
                {
                    MeasureName = "CMS164v5 - Category I";
                }
                else if (cqmid == "0018")
                {
                    MeasureName = "CMS165v5 - Category I";
                }
                else if (cqmid == "0075(A)" || cqmid == "0075(B)" || cqmid == "0075")
                {
                    MeasureName = "CMS182v6 - Category I";
                }
                else if (cqmid == "0059")
                {
                    MeasureName = "CMS122v5 - Category I";
                }
                else if (cqmid == "0031")
                {
                    MeasureName = "CMS125v5 - Category I";
                }
                else if (cqmid == "0712(A)" || cqmid == "0712(B)" || cqmid == "0712(C)" || cqmid == "0712")
                {
                    MeasureName = "CMS160v5 - Category I";
                }
                else if (cqmid == "0056")
                {
                    MeasureName = "CMS123v5 - Category I";
                }
                else if (cqmid == "0034")
                {
                    MeasureName = "CMS130v5 - Category I";
                }
                else if (cqmid == "0062")
                {
                    MeasureName = "CMS134v5 - Category I";
                }
                else if (cqmid == "0101")
                {
                    MeasureName = "CMS139v5 - Category I";
                }
                else if (cqmid == "0065")
                {
                    MeasureName = "CMS65v6 - Category I";
                }
                else if (cqmid == "2872")
                {
                    MeasureName = "CMS149v5 - Category I";
                }
                else if (cqmid == "CMS22v5")
                {
                    MeasureName = "CMS22v5 - Category I";
                }

                var response = new
                {
                    status = true,
                    DownloadFile = base64Strings,
                    FileName = MeasureName, // DateTime.Now.ToShortDateString() + " - Catagory I",
                    ProviderName = providerName
                };
                return (JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = true,
                    Message = "Nothing To Download."
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string GetMeasureDocument(string fileName)
        {
            string base64String = string.Empty;
            try
            {
                string imgPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Measure\" + fileName;
                if (!string.IsNullOrEmpty(imgPath))
                {
                    if (System.IO.File.Exists(imgPath))
                    {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
                        base64String = Convert.ToBase64String(imageBytes);

                        var response = new
                        {
                            status = true,
                            pdfHelperBase64 = base64String,
                            FileName = fileName,
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Document is not available.\nPlease contact your Administrator."

                        };

                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Invalid path key.\nPlease contact your Administrator."

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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

            }

        }
        private string GetProviderNpi(string NPI, string ProviderId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(NPI)) || string.IsNullOrEmpty(MDVUtility.ToStr(ProviderId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr("Please select a record first")
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    ProviderNPI dsProviderNpi = null;
                    BLObject<ProviderNPI> obj = new BLLCQM().GetProviderNpi(NPI, ProviderId);
                    dsProviderNpi = obj.Data;

                    if (obj.Data != null)
                    {
                        if (dsProviderNpi.Tables[dsProviderNpi.ProviderNpi.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsProviderNpi.Tables[dsProviderNpi.ProviderNpi.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                            {
                                { "txtProvider", MDVUtility.ToStr(dr["Provider"])},
                                { "txtNpi", MDVUtility.ToStr(dr["NPI"])},
                                { "txtProviderId", MDVUtility.ToStr(dr["ProviderId"])},
                            };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ProviderNpi_JSON = js.Serialize(keyValues)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = ""
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

        #endregion

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                #region Catagory3

                case "SEARCH_CQM_MEASURES":
                    {
                        var fieldsJson = context.Request["BatchClinicalQualityMeasureData"];
                        var CQMId = context.Request["CQMID"];
                        var PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        var RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        var strJsonData = LoadCqm(fieldsJson, CQMId, PageNumber, RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "SEARCH_ACI_MEASURES":
                    {
                        var fieldsJson = context.Request["BatchClinicalQualityMeasureData"];
                        var CQMId = context.Request["CQMID"];
                        var PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        var RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        var strJsonData = LoadACIData(fieldsJson, CQMId, PageNumber, RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "SEARCH_ACI_MEASURESGROUP":
                    {
                        var fieldsJson = context.Request["BatchClinicalQualityMeasureData"];
                        var CQMId = context.Request["CQMID"];
                        var PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        var RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        var strJsonData = LoadACIGroupData(fieldsJson, CQMId, PageNumber, RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "SEARCH_IMPROVEMENTACTIVITIES_MEASURES":
                    {
                        var fieldsJson = context.Request["FieldsData"];
                        var CQMId = context.Request["CQMID"];
                        var PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        var RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        var strJsonData = LoadImprovementActivitiesData(fieldsJson, CQMId, PageNumber, RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "SEARCH_IMPROVEMENTACTIVITIES_MEASURES_GROUP":
                    {
                        var fieldsJson = context.Request["FieldsData"];
                        var CQMId = context.Request["CQMID"];
                        var PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        var RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        var strJsonData = LoadImprovementActivitiesGroupData(fieldsJson, CQMId, PageNumber, RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "EXPORT_CQM_MEASURES":
                    {
                        var fieldsJson = context.Request["BatchClinicalQualityMeasureData"];
                        var cqmid = MDVUtility.ToStr(context.Request["CQMID"]);
                        var isFrom = MDVUtility.ToStr(context.Request["FROM"]);
                        var providerId = MDVUtility.ToInt64(context.Request["providerId"]);
                        var startDate = MDVUtility.ToStr(context.Request["AppointmentDateFrom"]);
                        var endDate = MDVUtility.ToStr(context.Request["AppointmentDateTo"]);
                        if (isFrom != "Detail")
                        {
                            fieldsJson = null;
                        }
                        var base64StringToDownload = CatagoryThreeXMLGenerator.InitializeCategoryThree_DataSets(cqmid, providerId, 1, startDate, endDate, fieldsJson);
                        var strJsonData = DwonloadCatagory3(base64StringToDownload, cqmid);

                        context.Response.ContentType = "text /plain";

                        context.Response.Write(strJsonData);
                    }
                    break;
                case "PROVIDER_NPI":
                    {
                        string NPI = context.Request["npi"];
                        string ProviderId = context.Request["providerId"];

                        string strJSONData = GetProviderNpi(NPI, ProviderId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                #endregion

                #region Catagory1

                case "SEARCH_CQM_MEASURE_DETAILS":
                    {
                        var fieldsJson = context.Request["ClinicalQualityMeasureDetailData"];
                        var cqmid = MDVUtility.ToStr(context.Request["CQMID"]);
                        var providerId = MDVUtility.ToStr(context.Request["providerId"]);
                        var dateFrom = MDVUtility.ToStr(context.Request["dateFrom"]);
                        var dateTo = MDVUtility.ToStr(context.Request["dateTo"]);

                        var strJsonData = LoadCQM_Details(fieldsJson, cqmid, providerId, dateFrom, dateTo);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "EXPORT_CQM_MEASURE_DETAILS":
                    {
                        var fieldsJson = context.Request["ClinicalQualityMeasureDetailData"];
                        var patientId = MDVUtility.ToInt64(context.Request["PatientId"]);
                        bool isC1 = MDVUtility.ToBool(context.Request["isC1"]);
                        var providerId = MDVUtility.ToInt64(context.Request["providerId"]);
                        var cqmid = context.Request["CQMID"];
                        var startDate = MDVUtility.ToStr(context.Request["dateFrom"]);
                        var endDate = MDVUtility.ToStr(context.Request["dateTo"]);

                        if (cqmid == "0421a" || cqmid == "0421b")
                        {
                            cqmid = "0421";
                            LoadCQM_Details(fieldsJson, cqmid, providerId.ToString(), startDate, endDate, true, isC1);
                        }

                        var base64StringToDownload = CatagoryOneXMLGenerator.InitializeCategoryOne_DataSets(cqmid, Int64.Parse(patientId.ToString()), providerId, 1, startDate, endDate, _listPatients, isC1);
                        var strJsonData = DwonloadCatagory1(base64StringToDownload, cqmid);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "GET_MEASURE_DOCUMENT":
                    {
                        string fileName = MDVUtility.ToStr(context.Request["fileName"]);
                        string strJsonData = GetMeasureDocument(fileName);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                    #endregion
            }
        }

        #endregion
    }
}