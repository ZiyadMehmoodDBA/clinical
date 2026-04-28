using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.EMR.Model.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Model.Clinical.Medical.Vitals;
using Newtonsoft.Json;
using MDVision.IEHR.Common;
using MDVision.DataAccess.DAL.Appointment;

//namespace MDVision.IEHR.EMR.Helpers.Clinical.Medical
namespace MDVision.IEHR.Controls.Clinical
{
    public class VitalsHelper
    {


        private BLLClinical BLLClinicalObj = null;
        private BLLSchedule BLLScheduleObj = null;
        public VitalsHelper()
        {
            BLLClinicalObj = new BLLClinical();
            BLLScheduleObj = new BLLSchedule();
        }

        private static VitalsHelper _instance = null;
        public static VitalsHelper Instance()
        {
            if (_instance == null)
                _instance = new VitalsHelper();
            return _instance;
        }
        public string LoadVitals(VitalModel model, Int64 VitalSignsId, int PageNumber, int RowsPerPage)
        {
            try
            {

                DSVitals dsVitals = null;
                BLObject<DSVitals> obj;
                DSVitals.VitalSignsChildDataTable dtFilteredChildVitals = new DSVitals.VitalSignsChildDataTable();
                obj = BLLClinicalObj.LoadVitals(MDVUtility.ToInt64(model.PatientId), VitalSignsId, PageNumber, RowsPerPage, "", "", MDVUtility.ToInt64(model.NotesId));
                dsVitals = obj.Data;
                foreach (DataRow drParentVital in dsVitals.VitalSignSoap.Rows)
                {
                    //DSVitals.VitalSignsChildRow[] arrDRSelectedChild = (DSVitals.VitalSignsChildRow[])dsVitals.VitalSignsChild.Select(MDVUtility.ToStr(dsVitals.VitalSignsChild.VitalSignIdColumn.ColumnName) + "=" + MDVUtility.ToStr(drParentVital[dsVitals.VitalSignSoap.VitalSignIdColumn]));
                    DSVitals.VitalSignsChildRow[] arrDRSelectedChild = (DSVitals.VitalSignsChildRow[])dsVitals.VitalSignsChild.Select(MDVUtility.ToStr(dsVitals.VitalSignsChild.VitalSignIdColumn.ColumnName) + "='" + MDVUtility.ToStr(drParentVital[dsVitals.VitalSignSoap.VitalSignIdColumn]) + "'");

                    foreach (DSVitals.VitalSignsChildRow drCurrentChildVital in arrDRSelectedChild)
                    {
                        /*  if (!(MDVUtility.ToStr(drParentVital[dsVitals.VitalSignSoap.VitalSignIdColumn]) == MDVUtility.ToStr(drCurrentChildVital[dsVitals.VitalSignsChild.VitalSignIdColumn])
                              && MDVUtility.ToStr(drParentVital[dsVitals.VitalSignSoap.BPIdColumn]) == MDVUtility.ToStr(drCurrentChildVital[dsVitals.VitalSignsChild.BPIdColumn])
                              && MDVUtility.ToStr(drParentVital[dsVitals.VitalSignSoap.TemperatureIdColumn]) == MDVUtility.ToStr(drCurrentChildVital[dsVitals.VitalSignsChild.TempratureIdColumn])
                              && MDVUtility.ToStr(drParentVital[dsVitals.VitalSignSoap.PulseIdColumn]) == MDVUtility.ToStr(drCurrentChildVital[dsVitals.VitalSignsChild.PulseIdColumn])
                              && MDVUtility.ToStr(drParentVital[dsVitals.VitalSignSoap.RespirationIdColumn]) == MDVUtility.ToStr(drCurrentChildVital[dsVitals.VitalSignsChild.RespirationIdColumn])))
                          {
                          */

                        DataRow drNewFilteredChildRow = dtFilteredChildVitals.NewRow();
                        drNewFilteredChildRow[dtFilteredChildVitals.VitalChildIdColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.VitalChildIdColumn.ColumnName];
                        drNewFilteredChildRow[dtFilteredChildVitals.VitalSignIdColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.VitalSignIdColumn.ColumnName];

                        // Get unique BPIds
                        DSVitals.VitalSignsChildRow[] arrselectedBPIdRows = (DSVitals.VitalSignsChildRow[])dtFilteredChildVitals.Select(MDVUtility.ToStr(dtFilteredChildVitals.BPIdColumn.ColumnName) + "=" + MDVUtility.ToLINQFormatString(drCurrentChildVital[dtFilteredChildVitals.BPIdColumn.ColumnName]));

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(dtFilteredChildVitals.BPIdColumn.ColumnName)) && !string.IsNullOrEmpty(MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.BPIdColumn.ColumnName])))
                        {
                            DataRow[] arrVSChildRowBPIdFromParent = (DataRow[])dsVitals.VitalSignSoap.Select(MDVUtility.ToStr(dtFilteredChildVitals.BPIdColumn.ColumnName) + "=" + MDVUtility.ToLINQFormatString(drCurrentChildVital[dtFilteredChildVitals.BPIdColumn.ColumnName]));
                            if (arrselectedBPIdRows.Length > 0 || arrVSChildRowBPIdFromParent.Length > 0)
                            {
                                drNewFilteredChildRow[dtFilteredChildVitals.BPIdColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.SystolicColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.DiastolicColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.BPModifiedByColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.BPModifiedOnColumn.ColumnName] = DBNull.Value;
                                drNewFilteredChildRow[dtFilteredChildVitals.BPNegationReasonIdColumn.ColumnName] = DBNull.Value;

                            }
                            else
                            {
                                drNewFilteredChildRow[dtFilteredChildVitals.BPIdColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.BPIdColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.SystolicColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.SystolicColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.DiastolicColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.DiastolicColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.BPModifiedByColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.BPModifiedByColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.BPModifiedOnColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.BPModifiedOnColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.BPNegationReasonIdColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.BPNegationReasonIdColumn.ColumnName];

                            }

                        }
                        else
                        {
                            drNewFilteredChildRow[dtFilteredChildVitals.BPIdColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.SystolicColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.DiastolicColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.BPModifiedByColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.BPModifiedOnColumn.ColumnName] = DBNull.Value;
                        }



                        //////////////////////


                        // Get unique PulseIds
                        DSVitals.VitalSignsChildRow[] arrselectedPulseIdRows = (DSVitals.VitalSignsChildRow[])dtFilteredChildVitals.Select(MDVUtility.ToStr(dtFilteredChildVitals.PulseIdColumn.ColumnName) + "='" + MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.PulseIdColumn.ColumnName]) + "'");
                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(dtFilteredChildVitals.PulseIdColumn.ColumnName)) && !string.IsNullOrEmpty(MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.PulseIdColumn.ColumnName])))
                        {
                            DataRow[] arrVSChildRowPulseIdFromParent = (DataRow[])dsVitals.VitalSignSoap.Select(MDVUtility.ToStr(dtFilteredChildVitals.PulseIdColumn.ColumnName) + "='" + MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.PulseIdColumn.ColumnName]) + "'");
                            if (arrselectedPulseIdRows.Length > 0 || arrVSChildRowPulseIdFromParent.Length > 0)
                            {
                                drNewFilteredChildRow[dtFilteredChildVitals.PulseIdColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.PulseResultColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.PulseModifiedByColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.PulseModifiedOnColumn.ColumnName] = DBNull.Value;
                            }
                            else
                            {
                                drNewFilteredChildRow[dtFilteredChildVitals.PulseIdColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.PulseIdColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.PulseResultColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.PulseResultColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.PulseModifiedByColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.PulseModifiedByColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.PulseModifiedOnColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.PulseModifiedOnColumn.ColumnName];

                            }
                        }
                        else
                        {
                            drNewFilteredChildRow[dtFilteredChildVitals.PulseIdColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.PulseResultColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.PulseModifiedByColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.PulseModifiedOnColumn.ColumnName] = DBNull.Value;
                        }



                        /////////////////////////////////////////////////


                        // Get unique TempratureIds
                        DSVitals.VitalSignsChildRow[] arrselectedTempratureIdRows = (DSVitals.VitalSignsChildRow[])dtFilteredChildVitals.Select(MDVUtility.ToStr(dtFilteredChildVitals.TemperatureIdColumn.ColumnName) + "='" + MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.TemperatureIdColumn.ColumnName]) + "'");

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(dtFilteredChildVitals.TemperatureIdColumn.ColumnName)) && !string.IsNullOrEmpty(MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.TemperatureIdColumn.ColumnName])))
                        {
                            DataRow[] arrVSChildRowTemperatureIdFromParent = (DataRow[])dsVitals.VitalSignSoap.Select(MDVUtility.ToStr(dsVitals.VitalSignSoap.TemperatureIdColumn.ColumnName) + "='" + MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.TemperatureIdColumn.ColumnName]) + "'");

                            if (arrselectedTempratureIdRows.Length > 0 || arrVSChildRowTemperatureIdFromParent.Length > 0)
                            {
                                drNewFilteredChildRow[dtFilteredChildVitals.TemperatureIdColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.TemperatureResultColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.TempModifiedByColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.TempModifiedOnColumn.ColumnName] = DBNull.Value;
                            }
                            else
                            {
                                drNewFilteredChildRow[dtFilteredChildVitals.TemperatureIdColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.TemperatureIdColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.TemperatureResultColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.TemperatureResultColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.TempModifiedByColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.TempModifiedByColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.TempModifiedOnColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.TempModifiedOnColumn.ColumnName];
                            }
                        }
                        else
                        {
                            drNewFilteredChildRow[dtFilteredChildVitals.TemperatureIdColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.TemperatureResultColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.TempModifiedByColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.TempModifiedOnColumn.ColumnName] = DBNull.Value;
                        }
                        /////////////////////////////////////////////////

                        // Get unique RespirationIds                            
                        DSVitals.VitalSignsChildRow[] arrselectedRespirationIdRows = (DSVitals.VitalSignsChildRow[])dtFilteredChildVitals.Select(MDVUtility.ToStr(dtFilteredChildVitals.RespirationIdColumn.ColumnName) + "='" + MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.RespirationIdColumn.ColumnName]) + "'");

                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(dtFilteredChildVitals.RespirationIdColumn.ColumnName)) && !string.IsNullOrEmpty(MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.RespirationIdColumn.ColumnName])))
                        {
                            DataRow[] arrVSChildRowRespirationIdFromParent = (DataRow[])dsVitals.VitalSignSoap.Select(MDVUtility.ToStr(dsVitals.VitalSignSoap.RespirationIdColumn.ColumnName) + "='" + MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.RespirationIdColumn.ColumnName]) + "'");
                            if (arrselectedRespirationIdRows.Length > 0 || arrVSChildRowRespirationIdFromParent.Length > 0)
                            {
                                drNewFilteredChildRow[dtFilteredChildVitals.RespirationIdColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.RespirationResultColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.RespModifiedByColumn.ColumnName] = "";
                                drNewFilteredChildRow[dtFilteredChildVitals.RespModifiedOnColumn.ColumnName] = DBNull.Value;
                            }
                            else
                            {
                                drNewFilteredChildRow[dtFilteredChildVitals.RespirationIdColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.RespirationIdColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.RespirationResultColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.RespirationResultColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.RespModifiedByColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.RespModifiedByColumn.ColumnName];
                                drNewFilteredChildRow[dtFilteredChildVitals.RespModifiedOnColumn.ColumnName] = drCurrentChildVital[dtFilteredChildVitals.RespModifiedOnColumn.ColumnName];

                            }
                        }
                        else
                        {
                            drNewFilteredChildRow[dtFilteredChildVitals.RespirationIdColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.RespirationResultColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.RespModifiedByColumn.ColumnName] = "";
                            drNewFilteredChildRow[dtFilteredChildVitals.RespModifiedOnColumn.ColumnName] = DBNull.Value;
                        }

                        /////////////////////////////////////////////////

                        if (drNewFilteredChildRow[dtFilteredChildVitals.BPIdColumn.ColumnName] == "" &&
                            drNewFilteredChildRow[dtFilteredChildVitals.PulseIdColumn.ColumnName] == "" &&
                            drNewFilteredChildRow[dtFilteredChildVitals.TemperatureIdColumn.ColumnName] == "" &&
                            drNewFilteredChildRow[dtFilteredChildVitals.RespirationIdColumn.ColumnName] == "")
                        {
                            // For the time being Do Nothing
                        }
                        else
                        {
                            dtFilteredChildVitals.Rows.Add(drNewFilteredChildRow);
                        }



                        /*
                                // To avoid duplication of result
                                if (string.IsNullOrEmpty(MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.BPIdColumn.ColumnName])) == true && string.IsNullOrEmpty(MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.PulseIdColumn.ColumnName])) == true && string.IsNullOrEmpty(MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.TempratureIdColumn.ColumnName])) == true && string.IsNullOrEmpty(MDVUtility.ToStr(drCurrentChildVital[dtFilteredChildVitals.RespirationIdColumn.ColumnName])) == true)
                                {
                                    string myvar = ""; 
                                    //drFilteredChildVitals.Rows.Add(drCurrentChildVital.ItemArray);
                                }
                                else
                                {

                                    dtFilteredChildVitals.ImportRow(drCurrentChildVital);
                                }
                            
                            }
                        */

                    }
                }
                var response = new
                {
                    status = true,
                    VitalsCount = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows.Count,
                    iTotalDisplayRecords = (dsVitals.VitalSignSoap.Rows.Count > 0) ? dsVitals.VitalSignSoap.Rows[0][dsVitals.VitalSignSoap.RecordCountColumn.ColumnName] : 0,
                    VitalsLoad_JSON = MDVUtility.JSON_DataTable(dsVitals.Tables[dsVitals.VitalSigns.TableName]),
                    ParentVitalsLoad_JSON = MDVUtility.JSON_DataTable(dsVitals.Tables[dsVitals.VitalSignSoap.TableName]),
                    ParentVitalsCount = (dsVitals.VitalSignSoap.Rows.Count > 0) ? dsVitals.VitalSignSoap.Rows.Count : 0,
                    ChildVitalsLoad_JSON = MDVUtility.JSON_DataTable(dtFilteredChildVitals),
                    ChildVitalsCount = (dtFilteredChildVitals.Rows.Count > 0) ? dtFilteredChildVitals.Rows.Count : 0,
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string DeleteVitals(VitalModel model, Int64 VitalSignsId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(VitalSignsId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.DeleteVitals(MDVUtility.ToStr(VitalSignsId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string FillVitals(VitalModel model, Int64 VitalSignsId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(VitalSignsId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSVitals dsVitals = null;
                    BLObject<DSVitals> obj = BLLClinicalObj.LoadVitals(MDVUtility.ToInt64(model.PatientId), VitalSignsId, 1, 1000, "1", "");
                    if (obj.Data != null)
                    {
                        dsVitals = obj.Data;
                        if (dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                            {
                                { "VitalSignDate",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsVitals.VitalSigns.VitalSignDateColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsVitals.VitalSigns.VitalSignDateColumn.ColumnName]).ToShortDateString()},
                                { "VitalsTime",  MDVUtility.ToStr(dr[dsVitals.VitalSigns.VitalSignTimeColumn.ColumnName])},
                                { "Height", MDVUtility.ToStr(dr[dsVitals.VitalSigns.HeightColumn.ColumnName])},
                                { "Weight", MDVUtility.ToStr(dr[dsVitals.VitalSigns.WeightColumn.ColumnName])},
                                { "SPO2", MDVUtility.ToStr(dr[dsVitals.VitalSigns.SPO2Column.ColumnName])},
                                { "NotesId", MDVUtility.ToStr(dr[dsVitals.VitalSigns.NotesIdColumn.ColumnName])},
                                { "OxygenSource", MDVUtility.ToStr(dr[dsVitals.VitalSigns.OxygenSourceColumn.ColumnName])},
                                { "PeakFlow", MDVUtility.ToStr(dr[dsVitals.VitalSigns.PeakFlowColumn.ColumnName])},
                                { "SeverityofPain", MDVUtility.ToStr(dr[dsVitals.VitalSigns.PainIdColumn.ColumnName])},
                                { "SmokingStatus", MDVUtility.ToStr(dr[dsVitals.VitalSigns.SmokeStatusIdColumn.ColumnName])},
                                { "Comments", MDVUtility.ToStr(dr[dsVitals.VitalSigns.CommentsColumn.ColumnName])},
                             //   { "VitalSignDate", MDVUtility.ToStr(dr[dsVitals.VitalSigns.VitalSignDateColumn.ColumnName])},
                                { "VitalSignTime", MDVUtility.ToStr(dr[dsVitals.VitalSigns.VitalSignTimeColumn.ColumnName])},
                                { "BSA", MDVUtility.ToStr(dr[dsVitals.VitalSigns.BSAColumn.ColumnName])},
                                { "BMI", MDVUtility.ToStr(dr[dsVitals.VitalSigns.BMIColumn.ColumnName])},
                                { "HeadCir", MDVUtility.ToStr(dr[dsVitals.VitalSigns.HeadCrColumn.ColumnName])},
                                { "BloodType", MDVUtility.ToStr(dr[dsVitals.VitalSigns.BloodTypeColumn.ColumnName])},
                                { "InhaledO2Concentration", MDVUtility.ToStr(dr[dsVitals.VitalSigns.InhaledO2ConcentrationColumn.ColumnName])},
                            };
                            List<Dictionary<string, string>> lstBloodPressure = new List<Dictionary<string, string>>();
                            foreach (DataRow drBloodPressure in dsVitals.Tables[dsVitals.VitalSignsBloodPressure.TableName].Rows)
                            {
                                var BloodPressurekeyValues = new Dictionary<string, string>
                            {
                                { "CurrentBloodPressureId", MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.BPIdColumn.ColumnName])},
                                { "BPId" + MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.BPIdColumn.ColumnName]), MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.BPIdColumn.ColumnName])},
                                { "Systolic" + MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.BPIdColumn.ColumnName]), MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.SystolicColumn.ColumnName])},
                                { "Diastolic" + MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.BPIdColumn.ColumnName]), MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.DiastolicColumn.ColumnName])},
                                { "BloodPressureTime" + MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.BPIdColumn.ColumnName]), MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.TimeColumn.ColumnName]) == "" ? DateTime.Now.ToShortTimeString() : MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.TimeColumn.ColumnName]) },
                                { "Position" + MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.BPIdColumn.ColumnName]), MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.PositionIdColumn.ColumnName])},
                                { "CuffLocation" + MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.BPIdColumn.ColumnName]), MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.CuffLocationIdColumn.ColumnName])},
                                { "CuffSize" + MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.BPIdColumn.ColumnName]), MDVUtility.ToStr(drBloodPressure[dsVitals.VitalSignsBloodPressure.CuffSizeIdColumn.ColumnName])}
                            };
                                lstBloodPressure.Add(BloodPressurekeyValues);
                            }

                            List<Dictionary<string, string>> lstPulse = new List<Dictionary<string, string>>();
                            foreach (DataRow drPulse in dsVitals.Tables[dsVitals.VitalSignsPulse.TableName].Rows)
                            {
                                var PulsekeyValues = new Dictionary<string, string>
                            {
                                { "CurrentPulseId", MDVUtility.ToStr(drPulse[dsVitals.VitalSignsPulse.PulseIdColumn.ColumnName])},
                                { "PulsId" + MDVUtility.ToStr(drPulse[dsVitals.VitalSignsPulse.PulseIdColumn.ColumnName]), MDVUtility.ToStr(drPulse[dsVitals.VitalSignsPulse.PulseIdColumn.ColumnName])},
                                { "PulseResult" + MDVUtility.ToStr(drPulse[dsVitals.VitalSignsPulse.PulseIdColumn.ColumnName]), MDVUtility.ToStr(drPulse[dsVitals.VitalSignsPulse.ResultColumn.ColumnName])},
                                { "Rythm" + MDVUtility.ToStr(drPulse[dsVitals.VitalSignsPulse.PulseIdColumn.ColumnName]), MDVUtility.ToStr(drPulse[dsVitals.VitalSignsPulse.RhythmIdColumn.ColumnName])},
                                { "PulseTime" + MDVUtility.ToStr(drPulse[dsVitals.VitalSignsPulse.PulseIdColumn.ColumnName]), MDVUtility.ToStr(drPulse[dsVitals.VitalSignsPulse.TimeColumn.ColumnName]) == "" ? DateTime.Now.ToShortTimeString() : MDVUtility.ToStr(drPulse[dsVitals.VitalSignsPulse.TimeColumn.ColumnName])}
                            };
                                lstPulse.Add(PulsekeyValues);
                            }

                            List<Dictionary<string, string>> lstTemperature = new List<Dictionary<string, string>>();
                            foreach (DataRow drTemperature in dsVitals.Tables[dsVitals.VitalSignsTemperature.TableName].Rows)
                            {
                                var TemperaturekeyValues = new Dictionary<string, string>
                            {
                                { "CurrentTemperatureId", MDVUtility.ToStr(drTemperature[dsVitals.VitalSignsTemperature.TempratureIdColumn.ColumnName])},
                                { "TempId" + MDVUtility.ToStr(drTemperature[dsVitals.VitalSignsTemperature.TempratureIdColumn.ColumnName]), MDVUtility.ToStr(drTemperature[dsVitals.VitalSignsTemperature.TempratureIdColumn.ColumnName])},
                                { "TemperatureResult" + MDVUtility.ToStr(drTemperature[dsVitals.VitalSignsTemperature.TempratureIdColumn.ColumnName]), MDVUtility.ToStr(drTemperature[dsVitals.VitalSignsTemperature.ResultColumn.ColumnName])},
                                { "Method" + MDVUtility.ToStr(drTemperature[dsVitals.VitalSignsTemperature.TempratureIdColumn.ColumnName]), MDVUtility.ToStr(drTemperature[dsVitals.VitalSignsTemperature.MethodIdColumn.ColumnName])},
                                { "TemperatureTime" + MDVUtility.ToStr(drTemperature[dsVitals.VitalSignsTemperature.TempratureIdColumn.ColumnName]), MDVUtility.ToStr(drTemperature[dsVitals.VitalSignsTemperature.TimeColumn.ColumnName]) == "" ? DateTime.Now.ToShortTimeString() : MDVUtility.ToStr(drTemperature[dsVitals.VitalSignsTemperature.TimeColumn.ColumnName])}
                            };
                                lstTemperature.Add(TemperaturekeyValues);
                            }

                            List<Dictionary<string, string>> lstRespiration = new List<Dictionary<string, string>>();
                            foreach (DataRow drRespiration in dsVitals.Tables[dsVitals.VitalSignsRespiration.TableName].Rows)
                            {
                                var RespirationkeyValues = new Dictionary<string, string>
                            {
                                { "CurrentRespirationId", MDVUtility.ToStr(drRespiration[dsVitals.VitalSignsRespiration.RespirationIdColumn.ColumnName])},
                                { "RespIdId" + MDVUtility.ToStr(drRespiration[dsVitals.VitalSignsRespiration.RespirationIdColumn.ColumnName]), MDVUtility.ToStr(drRespiration[dsVitals.VitalSignsRespiration.RespirationIdColumn.ColumnName])},
                                { "RespirationResult" + MDVUtility.ToStr(drRespiration[dsVitals.VitalSignsRespiration.RespirationIdColumn.ColumnName]), MDVUtility.ToStr(drRespiration[dsVitals.VitalSignsRespiration.ResultColumn.ColumnName])},
                                { "Pattern" + MDVUtility.ToStr(drRespiration[dsVitals.VitalSignsRespiration.RespirationIdColumn.ColumnName]), MDVUtility.ToStr(drRespiration[dsVitals.VitalSignsRespiration.PatternIdColumn.ColumnName])},
                                { "RespirationTime" + MDVUtility.ToStr(drRespiration[dsVitals.VitalSignsRespiration.RespirationIdColumn.ColumnName]), MDVUtility.ToStr(drRespiration[dsVitals.VitalSignsRespiration.TimeColumn.ColumnName]) == "" ? DateTime.Now.ToShortTimeString() : MDVUtility.ToStr(drRespiration[dsVitals.VitalSignsRespiration.TimeColumn.ColumnName])}
                            };
                                lstRespiration.Add(RespirationkeyValues);
                            }

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                VitalsFill_JSON = js.Serialize(keyValues),
                                VitalsBlooPressureFill_JSON = js.Serialize(lstBloodPressure),
                                VitalsPulseFill_JSON = js.Serialize(lstPulse),
                                VitalsTemperatureFill_JSON = js.Serialize(lstTemperature),
                                VitalsRespirationFill_JSON = js.Serialize(lstRespiration)
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message,
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVCustomException.HumanReadableMessage(obj.Message),
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string SaveVitals(VitalModel model, List<object> lstBloodPressureObjects, List<object> lstPulseObjects, List<object> lstTemperatureObjects, List<object> lstRespirationObjects, bool isFromGrid = false)
        {
            try
            {
                DSVitals dsVitals = new DSVitals();

                BLObject<DSVitals> obj = BLLClinicalObj.SaveVitals(model, lstBloodPressureObjects, lstPulseObjects, lstTemperatureObjects, lstRespirationObjects, isFromGrid);
                dsVitals = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName],
                        //Begin 13-07-2016 Edit By Humaira Yousaf Bug#1405
                        VitalsSignDate = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignDateColumn.ColumnName]
                        //End 13-07-2016 Edit By Humaira Yousaf Bug#1405
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return JsonConvert.SerializeObject(response);
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string UpdateVitals_(VitalModel model, Int64 VitalSignsId, List<object> lstBloodPressureObjects, List<object> lstPulseObjects, List<object> lstTemperatureObjects, List<object> lstRespirationObjects, bool isFromGrid = false)
        {
            try
            {

                if (VitalSignsId > 0)
                {

                    DSVitals dsVitals = new DSVitals();
                    BLObject<DSVitals> obj = BLLClinicalObj.LoadVitals(MDVUtility.ToInt64(model.PatientId), VitalSignsId, 1, 1000);
                    dsVitals = obj.Data;
                    foreach (DSVitals.VitalSignsRow dr in dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows)
                    {
                        if (isFromGrid == false)
                        {
                            if (!string.IsNullOrEmpty(model.OxygenSource))
                                dr.OxygenSource = MDVUtility.ToStr(model.OxygenSource);
                            if (!string.IsNullOrEmpty(model.PeakFlow))
                                dr.PeakFlow = MDVUtility.ToStr(model.PeakFlow);
                            if (!string.IsNullOrEmpty(model.SeverityofPain))
                                dr.PainId = MDVUtility.ToInt32(model.SeverityofPain);
                            if (!string.IsNullOrEmpty(model.SmokingStatus))
                                dr.SmokeStatusId = MDVUtility.ToInt32(model.SmokingStatus);

                            if (!string.IsNullOrEmpty(model.HeadCir))
                                dr.HeadCr = MDVUtility.ToDouble(model.HeadCir);
                            if (!string.IsNullOrEmpty(model.BloodType))
                                dr.BloodType = MDVUtility.ToInt32(model.BloodType);
                            if (model.RiskAssessmentId > 0)
                            {
                                dr.RiskAssessmentId = model.RiskAssessmentId;
                            }
                            else
                            {
                                dr[dsVitals.VitalSigns.RiskAssessmentIdColumn.ColumnName] = DBNull.Value;
                            }
                        }
                        if (!string.IsNullOrEmpty(model.Weight))
                            dr.Weight = MDVUtility.ToDouble(model.Weight);
                        else
                            dr[dsVitals.VitalSigns.WeightColumn.ColumnName] = DBNull.Value;
                        // if (!string.IsNullOrEmpty(model.Height))
                        dr.Height = model.Height;


                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;


                        //newly added
                        if (!string.IsNullOrEmpty(model.SPO2))
                        {
                            dr.SPO2 = MDVUtility.ToStr(model.SPO2);
                        }
                        else
                        {
                            dr[dsVitals.VitalSigns.SPO2Column] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.Comments))
                            dr.Comments = MDVUtility.ToStr(model.Comments);
                        else
                        {
                            dr[dsVitals.VitalSigns.CommentsColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.VitalSignDate))
                            dr.VitalSignDate = MDVUtility.ToDateTime(model.VitalSignDate);
                        if (!string.IsNullOrEmpty(model.VitalsTime))
                            dr.VitalSignTime = MDVUtility.ToStr(model.VitalsTime);
                        if (!string.IsNullOrEmpty(model.BSA))
                            dr.BSA = MDVUtility.ToDouble(model.BSA);
                        else
                            dr[dsVitals.VitalSigns.BSAColumn.ColumnName] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.BMI))
                            dr.BMI = MDVUtility.ToDouble(model.BMI);
                        else
                            dr[dsVitals.VitalSigns.BMIColumn.ColumnName] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.NotesId) && model.NotesId != "0")
                        {
                            dr.NotesId = MDVUtility.ToInt64(model.NotesId);
                            //dr.IsFromNote = true;
                        }
                        else
                        {
                            dr[dsVitals.VitalSigns.NotesIdColumn.ColumnName] = DBNull.Value;
                            //dr.IsFromNote = false;
                        }

                        if (!string.IsNullOrEmpty(model.VisitId) && model.NotesId != "0")
                            dr.NotesId = MDVUtility.ToInt64(model.VisitId);
                        else
                            dr[dsVitals.VitalSigns.VisitIdColumn.ColumnName] = DBNull.Value;

                        //end newly added
                    }
                    if (lstBloodPressureObjects.Count > 0)
                    {
                        // string responseBloodPressure = insertUpdateCurrentVitalChilds(dsVitals, "VitalSignsBloodPressureRow", true, null, VitalSignsId, lstBloodPressureObjects, isFromGrid, false, PatientId: model.PatientId, isInsertCase: false);
                    }
                    if (lstPulseObjects.Count > 0)
                    {
                        // string responsePulse = insertUpdateCurrentVitalChilds(dsVitals, "VitalSignsPulseRow", true, null, VitalSignsId, lstPulseObjects, isFromGrid, false, PatientId: model.PatientId, isInsertCase: false);
                    }
                    if (lstTemperatureObjects.Count > 0)
                    {
                        // string responsePulse = insertUpdateCurrentVitalChilds(dsVitals, "VitalSignsTempratureRow", true, null, VitalSignsId, lstTemperatureObjects, isFromGrid, false, PatientId: model.PatientId, isInsertCase: false);
                    }
                    if (lstRespirationObjects.Count > 0)
                    {
                        //string responsePulse = insertUpdateCurrentVitalChilds(dsVitals, "VitalSignsRespirationRow", true, null, VitalSignsId, lstRespirationObjects, isFromGrid, false, PatientId: model.PatientId, isInsertCase: false);
                    }

                    #region Database Updation
                    if (dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows.Count > 0)
                    {
                        BLObject<DSVitals> objUpdate = BLLClinicalObj.UpdateVitals(dsVitals);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = objUpdate.Message
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Vitals not found."
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string UpdateVitals(VitalModel model, Int64 VitalSignsId, List<object> lstBloodPressureObjects, List<object> lstPulseObjects, List<object> lstTemperatureObjects, List<object> lstRespirationObjects, bool isFromGrid = false)
        {
            try
            {
                if (VitalSignsId > 0)
                {
                    BLObject<DSVitals> objUpdate = BLLClinicalObj.UpdateVitals(model, VitalSignsId, lstBloodPressureObjects, lstPulseObjects, lstTemperatureObjects, lstRespirationObjects, isFromGrid);
                    if (objUpdate.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = objUpdate.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Vitals not found."
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }



        /*
        private string insertUpdateCurrentVitalChilds(DSVitals dsVitals, string RowType, bool isNewRow, object objRow, Int64 VitalSignId, List<object> lstObjects, bool isFromGrid = false, bool AddFirstChild = true, string PatientId = "", bool isInsertCase = false)
        {
            try
            {
                if (RowType == "VitalSignsBloodPressureRow")
                {
                    #region VitalSignsBloodPressureRow

                    BLObject<DSVitals> objVitalsBloodPressure = BLLClinicalObj.LoadVitalSignsBloodPressure(0, VitalSignId);
                    dsVitals = objVitalsBloodPressure.Data;
                    //dsVitals.Merge(objVitalsBloodPressure.Data);
                    List<BloodPressureModel> lstBloodPressureModel = lstObjects.OfType<BloodPressureModel>().ToList();
                    bool isFirstChild = false;

                    foreach (BloodPressureModel CurrentModel in lstBloodPressureModel)
                    {
                        bool isValidRow = false; int i = -1;
                        Int32 currentBPId = MDVUtility.ToInt32(CurrentModel.BPId);
                        currentBPId = currentBPId == 0 ? -1 : currentBPId;
                        Int16 currentSystolic = MDVUtility.ToInt16(CurrentModel.Systolic);
                        Int16 currentDiastolic = MDVUtility.ToInt16(CurrentModel.Diastolic);

                        DSVitals.VitalSignsBloodPressureRow RowBP = null;

                        if (string.IsNullOrEmpty(CurrentModel.BPId))
                            CurrentModel.BPId = i.ToString();

                        DSVitals.VitalSignsBloodPressureRow[] arrBloodPressureRows = (DSVitals.VitalSignsBloodPressureRow[])dsVitals.VitalSignsBloodPressure.Select(dsVitals.VitalSignsBloodPressure.BPIdColumn.ColumnName + "=" + CurrentModel.BPId);

                        if (arrBloodPressureRows.Length > 0)
                        {
                            //Start 13-12-2016 Humaira Yousaf Bug# EMR-2174
                            RowBP = arrBloodPressureRows[0];

                            ////Begin 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                            //if (currentSystolic > 0 && currentDiastolic > 0)
                            //{
                            //    RowBP = arrBloodPressureRows[0];
                            //}
                            ////End 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                            //End 13-12-2016 Humaira Yousaf Bug# EMR-2174
                        }
                        else
                        {
                            //Begin 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                            //Begin 11-30-2015 Muhammad Arshad Bug # EMR-122 Vitals Workflow in Clinical Module -> Past Vitals History -> Grid
                            if (currentBPId < 0 && isFirstChild == false && AddFirstChild == true)
                            {
                                isFirstChild = true;
                                isValidRow = true;
                            }
                            //End 11-30-2015 Muhammad Arshad Bug # EMR-122 Vitals Workflow in Clinical Module -> Past Vitals History -> Grid
                            else if (currentSystolic > 0 && currentDiastolic > 0)
                            {
                                isValidRow = true;
                            }

                            if (isInsertCase && VitalSignId > 0)
                                isValidRow = true;

                            if (isValidRow == true)
                            {
                                RowBP = dsVitals.VitalSignsBloodPressure.NewVitalSignsBloodPressureRow();
                            }
                            //End 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows

                        }

                        if (RowBP != null)
                        {
                            bool isValueDifferent = false;
                            bool istoUpdateRow = false;
                            if (arrBloodPressureRows.Length < 1)
                            {
                                RowBP.BPId = currentBPId;
                            }
                            RowBP.VitalSignId = VitalSignId;

                            if (arrBloodPressureRows.Length < 1)
                            {
                                isValueDifferent = true;
                            }
                            else if (RowBP.IsNull("Systolic") == false && currentSystolic != RowBP.Systolic)
                            {
                                isValueDifferent = true;
                            }
                            else if (RowBP.IsNull("Systolic") == true && currentSystolic > 0)
                            {
                                isValueDifferent = true;
                            }
                            else
                            {
                                isValueDifferent = false;
                            }

                            if (isValueDifferent == true)
                            {
                                istoUpdateRow = true;
                                if (!string.IsNullOrEmpty(CurrentModel.Systolic) && MDVUtility.ToInt64(CurrentModel.Systolic) > 0)
                                {
                                    RowBP.Systolic = MDVUtility.ToInt16(CurrentModel.Systolic);
                                }
                                else
                                {
                                    RowBP[dsVitals.VitalSignsBloodPressure.SystolicColumn] = DBNull.Value;
                                }
                            }


                            if (arrBloodPressureRows.Length < 1)
                            {
                                isValueDifferent = true;
                            }
                            else if (RowBP.IsNull("Diastolic") == false && currentDiastolic != RowBP.Diastolic)
                            {
                                isValueDifferent = true;
                            }
                            else if (RowBP.IsNull("Diastolic") == true && currentDiastolic > 0)
                            {
                                isValueDifferent = true;
                            }
                            else
                            {
                                isValueDifferent = false;
                            }

                            if (isValueDifferent == true)
                            {
                                istoUpdateRow = true;
                                if (!string.IsNullOrEmpty(CurrentModel.Diastolic) && MDVUtility.ToInt64(CurrentModel.Diastolic) > 0)
                                {
                                    RowBP.Diastolic = MDVUtility.ToInt16(CurrentModel.Diastolic);
                                }
                                else
                                {
                                    RowBP[dsVitals.VitalSignsBloodPressure.DiastolicColumn] = DBNull.Value;
                                }
                            }

                            if (isFromGrid == false)
                            {
                                string currentBPTime = MDVUtility.ToStr(CurrentModel.BloodPressureTime);
                                if (arrBloodPressureRows.Length < 1)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowBP.IsNull("Time") == false && currentBPTime != RowBP.Time)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowBP.IsNull("Time") == true && currentBPTime != "")
                                {
                                    isValueDifferent = true;
                                }
                                else
                                {
                                    isValueDifferent = false;
                                }

                                if (isValueDifferent == true)
                                {
                                    istoUpdateRow = true;
                                    if (!string.IsNullOrEmpty(CurrentModel.BloodPressureTime))
                                    {
                                        RowBP[dsVitals.VitalSignsBloodPressure.TimeColumn] = MDVUtility.ToStr(CurrentModel.BloodPressureTime);
                                        //TimeSpan currTime = TimeSpan.ParseExact(CurrentModel.BloodPressureTime, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                                        //RowBP.Time = currTime;
                                        //DateTime myDate = DateTime.ParseExact(CurrentModel.BloodPressureTime, "HH:mm", null);
                                        //RowBP.Time = new TimeSpan(myDate.Hour, myDate.Minute, myDate.Second);
                                    }
                                    else
                                    {
                                        RowBP[dsVitals.VitalSignsBloodPressure.TimeColumn] = DBNull.Value;
                                    }
                                }
                                int currentPosition = MDVUtility.ToInt(CurrentModel.Position);
                                if (arrBloodPressureRows.Length < 1)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowBP.IsNull("PositionId") == false && currentPosition != RowBP.PositionId)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowBP.IsNull("PositionId") == true && currentPosition > 0)
                                {
                                    isValueDifferent = true;
                                }
                                else
                                {
                                    isValueDifferent = false;
                                }

                                if (isValueDifferent == true)
                                {
                                    istoUpdateRow = true;
                                    if (MDVUtility.ToInt(CurrentModel.Position) > 0)
                                    {
                                        RowBP.PositionId = MDVUtility.ToInt(CurrentModel.Position);
                                    }
                                    else
                                    {
                                        RowBP[dsVitals.VitalSignsBloodPressure.PositionIdColumn] = DBNull.Value;
                                    }
                                }

                                int currentCuffLocation = MDVUtility.ToInt(CurrentModel.CuffLocation);
                                if (arrBloodPressureRows.Length < 1)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowBP.IsNull("CuffLocationId") == false && currentCuffLocation != RowBP.CuffLocationId)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowBP.IsNull("CuffLocationId") == true && currentCuffLocation > 0)
                                {
                                    isValueDifferent = true;
                                }
                                else
                                {
                                    isValueDifferent = false;
                                }
                                if (isValueDifferent == true)
                                {
                                    istoUpdateRow = true;
                                    if (MDVUtility.ToInt(CurrentModel.CuffLocation) > 0)
                                    {
                                        RowBP.CuffLocationId = MDVUtility.ToInt(CurrentModel.CuffLocation);
                                    }
                                    else
                                    {
                                        RowBP[dsVitals.VitalSignsBloodPressure.CuffLocationIdColumn] = DBNull.Value;
                                    }
                                }

                                int currentCuffSize = MDVUtility.ToInt(CurrentModel.CuffSize);
                                if (arrBloodPressureRows.Length < 1)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowBP.IsNull("CuffSizeId") == false && currentCuffSize != RowBP.CuffSizeId)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowBP.IsNull("CuffSizeId") == true && currentCuffSize > 0)
                                {
                                    isValueDifferent = true;
                                }
                                else
                                {
                                    isValueDifferent = false;
                                }

                                if (isValueDifferent == true)
                                {
                                    istoUpdateRow = true;
                                    if (MDVUtility.ToInt(CurrentModel.CuffSize) > 0)
                                    {
                                        RowBP.CuffSizeId = MDVUtility.ToInt(CurrentModel.CuffSize);
                                    }
                                    else
                                    {
                                        RowBP[dsVitals.VitalSignsBloodPressure.CuffSizeIdColumn] = DBNull.Value;
                                    }
                                }

                            }

                            if (istoUpdateRow == true)
                            {
                                RowBP.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                RowBP.CreatedOn = DateTime.Now;
                                RowBP.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                RowBP.ModifiedOn = DateTime.Now;
                                if (MDVUtility.ToInt(CurrentModel.Position) > 0)
                                {
                                    RowBP.PositionId = MDVUtility.ToInt(CurrentModel.Position);
                                }
                                else
                                {
                                    RowBP[dsVitals.VitalSignsBloodPressure.PositionIdColumn] = DBNull.Value;
                                }
                                if (MDVUtility.ToInt(CurrentModel.CuffSize) > 0)
                                {
                                    RowBP.CuffSizeId = MDVUtility.ToInt(CurrentModel.CuffSize);
                                }
                                else
                                {
                                    RowBP[dsVitals.VitalSignsBloodPressure.CuffSizeIdColumn] = DBNull.Value;
                                }
                                if (MDVUtility.ToInt(CurrentModel.CuffLocation) > 0)
                                {
                                    RowBP.CuffLocationId = MDVUtility.ToInt(CurrentModel.CuffLocation);
                                }
                                else
                                {
                                    RowBP[dsVitals.VitalSignsBloodPressure.CuffLocationIdColumn] = DBNull.Value;
                                }

                                // if no blood pressure is found against BPId, it implies for new record
                                if (arrBloodPressureRows.Length < 1)
                                {
                                    dsVitals.VitalSignsBloodPressure.AddVitalSignsBloodPressureRow(RowBP);
                                }
                            }
                        }
                        i--;
                    }

                    #region Database Insertion/Updation

                    BLObject<DSVitals> objInsertedBP = BLLClinicalObj.InsertUpdateVitalSignsBloodPressure(dsVitals, PatientId);
                    if (objInsertedBP.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message
                            //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objInsertedBP.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }

                    #endregion


                    #endregion
                }
                else if (RowType == "VitalSignsPulseRow")
                {
                    #region VitalSignsPulseRow

                    BLObject<DSVitals> objVitalsPulse = BLLClinicalObj.LoadVitalSignsPulse(0, VitalSignId);
                    dsVitals = objVitalsPulse.Data;
                    // dsVitals.Merge(objVitalsPulse.Data);
                    List<PulseModel> lstPulseModel = lstObjects.OfType<PulseModel>().ToList();
                    bool isFirstChild = false;
                    foreach (PulseModel CurrentModel in lstPulseModel)
                    {
                        bool isValidRow = false; int i = -1;
                        Int32 currentPulseId = MDVUtility.ToInt32(CurrentModel.PulsId);
                        currentPulseId = currentPulseId == 0 ? -1 : currentPulseId;

                        DSVitals.VitalSignsPulseRow RowPulse = null;

                        if (string.IsNullOrEmpty(CurrentModel.PulsId))
                            CurrentModel.PulsId = i.ToString();

                        DSVitals.VitalSignsPulseRow[] arrPulseRows = (DSVitals.VitalSignsPulseRow[])dsVitals.VitalSignsPulse.Select(dsVitals.VitalSignsPulse.PulseIdColumn.ColumnName + "=" + CurrentModel.PulsId);
                        if (arrPulseRows.Length > 0)
                        {
                            //Start 13-12-2016 Humaira Yousaf Bug# EMR-2174
                            RowPulse = arrPulseRows[0];

                            ////Begin 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                            //if (CurrentModel.PulseResult != "")
                            //{
                            //    RowPulse = arrPulseRows[0];
                            //}
                            ////End 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                            //End 13-12-2016 Humaira Yousaf Bug# EMR-2174

                        }
                        else
                        {
                            //Begin 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                            //Begin 11-30-2015 Muhammad Arshad Bug # EMR-122 Vitals Workflow in Clinical Module -> Past Vitals History -> Grid
                            if (currentPulseId < 0 && isFirstChild == false && AddFirstChild == true)
                            {
                                isFirstChild = true;
                                isValidRow = true;
                            }
                            //End 11-30-2015 Muhammad Arshad Bug # EMR-122 Vitals Workflow in Clinical Module -> Past Vitals History -> Grid
                            else if (CurrentModel.PulseResult != "")
                            {
                                isValidRow = true;
                            }

                            if (isInsertCase && VitalSignId > 0)
                                isValidRow = true;

                            if (isValidRow == true)
                            {
                                RowPulse = dsVitals.VitalSignsPulse.NewVitalSignsPulseRow();
                            }
                            //End 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows

                        }

                        if (RowPulse != null)
                        {
                            bool isValueDifferent = false;
                            bool istoUpdateRow = false;
                            if (arrPulseRows.Length < 1)
                            {
                                RowPulse.PulseId = currentPulseId;
                            }
                            RowPulse.VitalSignId = VitalSignId;
                            if (arrPulseRows.Length < 1)
                            {
                                isValueDifferent = true;
                            }
                            else if (RowPulse.IsNull("Result") == false && CurrentModel.PulseResult != RowPulse.Result)
                            {
                                isValueDifferent = true;
                            }
                            else if (RowPulse.IsNull("Result") == true && CurrentModel.PulseResult != "")
                            {
                                isValueDifferent = true;
                            }
                            else
                            {
                                isValueDifferent = false;
                            }
                            if (isValueDifferent == true)
                            {
                                istoUpdateRow = true;
                                if (!string.IsNullOrEmpty(CurrentModel.PulseResult))
                                {
                                    RowPulse[dsVitals.VitalSignsPulse.ResultColumn] = MDVUtility.ToStr(CurrentModel.PulseResult);
                                }
                                else
                                {
                                    RowPulse[dsVitals.VitalSignsPulse.ResultColumn] = DBNull.Value;
                                }
                            }

                            if (isFromGrid == false)
                            {
                                Int16 currentRhythm = MDVUtility.ToInt16(CurrentModel.Rythm);
                                if (arrPulseRows.Length < 1)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowPulse.IsNull("RhythmId") == false && currentRhythm != RowPulse.RhythmId)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowPulse.IsNull("RhythmId") == true && currentRhythm > 0)
                                {
                                    isValueDifferent = true;
                                }
                                else
                                {
                                    isValueDifferent = false;
                                }
                                if (isValueDifferent == true)
                                {
                                    istoUpdateRow = true;
                                    if (!string.IsNullOrEmpty(CurrentModel.Rythm))
                                    {
                                        RowPulse.RhythmId = MDVUtility.ToInt16(CurrentModel.Rythm);
                                    }
                                    else
                                    {
                                        RowPulse[dsVitals.VitalSignsPulse.RhythmIdColumn] = DBNull.Value;
                                    }
                                }

                                if (arrPulseRows.Length < 1)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowPulse.IsNull("Time") == false && CurrentModel.PulseTime != RowPulse.Time)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowPulse.IsNull("Time") == true && CurrentModel.PulseTime != "")
                                {
                                    isValueDifferent = true;
                                }
                                else
                                {
                                    isValueDifferent = false;
                                }
                                if (isValueDifferent == true)
                                {
                                    istoUpdateRow = true;
                                    if (!string.IsNullOrEmpty(CurrentModel.PulseTime))
                                    {
                                        RowPulse[dsVitals.VitalSignsPulse.TimeColumn] = MDVUtility.ToStr(CurrentModel.PulseTime);
                                        //TimeSpan currTime = TimeSpan.ParseExact(CurrentModel.BloodPressureTime, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                                        //RowPulse.Time = currTime;
                                        //DateTime myDate = DateTime.ParseExact(CurrentModel.BloodPressureTime, "HH:mm", null);
                                        //RowPulse.Time = new TimeSpan(myDate.Hour, myDate.Minute, myDate.Second);
                                    }
                                    else
                                    {
                                        RowPulse[dsVitals.VitalSignsPulse.TimeColumn] = DBNull.Value;
                                    }
                                }
                            }

                            if (istoUpdateRow == true)
                            {
                                RowPulse.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                RowPulse.CreatedOn = DateTime.Now;
                                RowPulse.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                RowPulse.ModifiedOn = DateTime.Now;
                                // if no pulse is found against PulseId, it implies for new record
                                if (arrPulseRows.Length < 1)
                                {
                                    dsVitals.VitalSignsPulse.AddVitalSignsPulseRow(RowPulse);
                                }
                            }
                        }
                        i--;
                    }

                    #region Database Insertion/Updation

                    BLObject<DSVitals> objInsertedPulse = BLLClinicalObj.InsertUpdateVitalSignsPulse(dsVitals, PatientId);
                    if (objInsertedPulse.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message
                            //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objInsertedPulse.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }

                    #endregion

                    #endregion

                }
                else if (RowType == "VitalSignsTempratureRow")
                {
                    #region VitalSignsTempratureRow

                    BLObject<DSVitals> objVitalsPulse = BLLClinicalObj.LoadVitalSignsTemperature(0, VitalSignId);
                    dsVitals = objVitalsPulse.Data;
                    //dsVitals.Merge(objVitalsPulse.Data);
                    List<TemperatureModel> lstTemperatureModel = lstObjects.OfType<TemperatureModel>().ToList();
                    bool isFirstChild = false; int i = -1;
                    foreach (TemperatureModel CurrentModel in lstTemperatureModel)
                    {
                        bool isValidRow = false;
                        Int32 currentTempId = MDVUtility.ToInt32(CurrentModel.TempId);
                        currentTempId = currentTempId == 0 ? -1 : currentTempId;


                        DSVitals.VitalSignsTempratureRow RowTemperature = null;

                        if (string.IsNullOrEmpty(CurrentModel.TempId))
                            CurrentModel.TempId = i.ToString();

                        DSVitals.VitalSignsTempratureRow[] arrTemperatureRows = (DSVitals.VitalSignsTempratureRow[])dsVitals.VitalSignsTemprature.Select(dsVitals.VitalSignsTemprature.TempratureIdColumn.ColumnName + "=" + CurrentModel.TempId);

                        if (arrTemperatureRows.Length > 0)
                        {
                            //Start 13-12-2016 Humaira Yousaf Bug# EMR-2174
                            RowTemperature = arrTemperatureRows[0];

                            ////Begin 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                            //if (CurrentModel.TemperatureResult != "")
                            //{
                            //    RowTemperature = arrTemperatureRows[0];
                            //}
                            ////End 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                            //Start 13-12-2016 Humaira Yousaf Bug# EMR-2174
                        }
                        else
                        {
                            //Begin 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                            //Begin 11-30-2015 Muhammad Arshad Bug # EMR-122 Vitals Workflow in Clinical Module -> Past Vitals History -> Grid
                            if (currentTempId < 0 && isFirstChild == false && AddFirstChild == true)
                            {
                                isFirstChild = true;
                                isValidRow = true;
                            }
                            //End 11-30-2015 Muhammad Arshad Bug # EMR-122 Vitals Workflow in Clinical Module -> Past Vitals History -> Grid
                            else if (CurrentModel.TemperatureResult != "")
                            {
                                isValidRow = true;
                            }

                            if (isInsertCase && VitalSignId > 0)
                                isValidRow = true;

                            if (isValidRow == true)
                            {
                                RowTemperature = dsVitals.VitalSignsTemprature.NewVitalSignsTempratureRow();
                            }
                            //End 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                        }


                        if (RowTemperature != null)
                        {
                            bool isValueDifferent = false;
                            bool istoUpdateRow = false;
                            if (arrTemperatureRows.Length < 1)
                            {
                                RowTemperature.TempratureId = currentTempId;
                            }
                            RowTemperature.VitalSignId = VitalSignId;
                            if (arrTemperatureRows.Length < 1)
                            {
                                isValueDifferent = true;
                            }
                            else if (RowTemperature.IsNull("Result") == false && CurrentModel.TemperatureResult != RowTemperature.Result)
                            {
                                isValueDifferent = true;
                            }
                            else if (RowTemperature.IsNull("Result") == true && CurrentModel.TemperatureResult != "")
                            {
                                isValueDifferent = true;
                            }
                            else
                            {
                                isValueDifferent = false;
                            }
                            if (isValueDifferent == true)
                            {
                                istoUpdateRow = true;
                                if (!string.IsNullOrEmpty(CurrentModel.TemperatureResult))
                                {
                                    RowTemperature[dsVitals.VitalSignsTemprature.ResultColumn] = CurrentModel.TemperatureResult;
                                }
                                else
                                {
                                    RowTemperature[dsVitals.VitalSignsTemprature.ResultColumn] = DBNull.Value;
                                }
                            }

                            if (isFromGrid == false)
                            {
                                Int16 currentMethod = MDVUtility.ToInt16(CurrentModel.Method);
                                if (arrTemperatureRows.Length < 1)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowTemperature.IsNull("MethodId") == false && currentMethod != RowTemperature.MethodId)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowTemperature.IsNull("MethodId") == true && currentMethod > 0)
                                {
                                    isValueDifferent = true;
                                }
                                else
                                {
                                    isValueDifferent = false;
                                }
                                if (isValueDifferent == true)
                                {
                                    istoUpdateRow = true;
                                    if (!string.IsNullOrEmpty(CurrentModel.Method))
                                    {
                                        RowTemperature.MethodId = MDVUtility.ToInt16(CurrentModel.Method);
                                    }
                                    else
                                    {
                                        RowTemperature[dsVitals.VitalSignsTemprature.MethodIdColumn] = DBNull.Value;
                                    }
                                }

                                if (arrTemperatureRows.Length < 1)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowTemperature.IsNull("Time") == false && CurrentModel.TemperatureTime != RowTemperature.Time)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowTemperature.IsNull("Time") == true && CurrentModel.TemperatureTime != "")
                                {
                                    isValueDifferent = true;
                                }
                                else
                                {
                                    isValueDifferent = false;
                                }
                                if (isValueDifferent == true)
                                {
                                    istoUpdateRow = true;
                                    if (!string.IsNullOrEmpty(CurrentModel.TemperatureTime))
                                    {
                                        RowTemperature[dsVitals.VitalSignsTemprature.TimeColumn] = MDVUtility.ToStr(CurrentModel.TemperatureTime);
                                    }
                                    else
                                    {
                                        RowTemperature[dsVitals.VitalSignsTemprature.TimeColumn] = DBNull.Value;
                                    }
                                }

                            }

                            if (istoUpdateRow == true)
                            {
                                RowTemperature.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                RowTemperature.CreatedOn = DateTime.Now;
                                RowTemperature.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                RowTemperature.ModifiedOn = DateTime.Now;
                                // if no Temperature is found against TemperatureId, it implies for new record
                                if (arrTemperatureRows.Length < 1)
                                {
                                    dsVitals.VitalSignsTemprature.AddVitalSignsTempratureRow(RowTemperature);
                                }
                            }

                        }
                        i--;
                    }

                    BLObject<DSVitals> objInsertedTemperature = BLLClinicalObj.InsertUpdateVitalSignsTemperature(dsVitals, PatientId);
                    if (objInsertedTemperature.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message
                            //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objInsertedTemperature.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }

                    #endregion

                }
                else if (RowType == "VitalSignsRespirationRow")
                {
                    BLObject<DSVitals> objVitalsPulse = BLLClinicalObj.LoadVitalSignsRespiration(0, VitalSignId);
                    dsVitals = objVitalsPulse.Data;
                    //dsVitals.Merge(objVitalsPulse.Data);
                    List<RespirationModel> lstRespirationModel = lstObjects.OfType<RespirationModel>().ToList();
                    bool isFirstChild = false; int i = -1;
                    foreach (RespirationModel CurrentModel in lstRespirationModel)
                    {
                        bool isValidRow = false;
                        Int32 currentRespId = MDVUtility.ToInt32(CurrentModel.RespId);
                        currentRespId = currentRespId == 0 ? -1 : currentRespId;

                        DSVitals.VitalSignsRespirationRow RowRespiration = null;

                        if (string.IsNullOrEmpty(CurrentModel.RespId))
                            CurrentModel.RespId = i.ToString();

                        DSVitals.VitalSignsRespirationRow[] arrRespirationRows = (DSVitals.VitalSignsRespirationRow[])dsVitals.VitalSignsRespiration.Select(dsVitals.VitalSignsRespiration.RespirationIdColumn.ColumnName + "=" + CurrentModel.RespId);

                        if (arrRespirationRows.Length > 0)
                        {
                            //Start 13-12-2016 Humaira Yousaf Bug# EMR-2174
                            RowRespiration = arrRespirationRows[0];

                            ////Begin 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                            //if (CurrentModel.RespirationResult != "")
                            //{
                            //    RowRespiration = arrRespirationRows[0];
                            //}
                            ////End 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                            //Start 13-12-2016 Humaira Yousaf Bug# EMR-2174

                        }
                        else
                        {
                            //Begin 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                            //Begin 11-30-2015 Muhammad Arshad Bug # EMR-122 Vitals Workflow in Clinical Module -> Past Vitals History -> Grid
                            if (currentRespId < 0 && isFirstChild == false && AddFirstChild == true)
                            {
                                isFirstChild = true;
                                isValidRow = true;
                            }
                            //End 11-30-2015 Muhammad Arshad Bug # EMR-122 Vitals Workflow in Clinical Module -> Past Vitals History -> Grid
                            else if (CurrentModel.RespirationResult != "")
                            {
                                isValidRow = true;
                            }

                            if (isInsertCase && VitalSignId > 0)
                                isValidRow = true;

                            if (isValidRow == true)
                            {
                                RowRespiration = dsVitals.VitalSignsRespiration.NewVitalSignsRespirationRow();
                            }
                            //End 11-27-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
                        }


                        if (RowRespiration != null)
                        {
                            bool isValueDifferent = false;
                            bool istoUpdateRow = false;
                            if (arrRespirationRows.Length < 1)
                            {
                                RowRespiration.RespirationId = currentRespId;
                            }
                            RowRespiration.VitalSignId = VitalSignId;

                            if (arrRespirationRows.Length < 1)
                            {
                                isValueDifferent = true;
                            }
                            else if (RowRespiration.IsNull("Result") == false && CurrentModel.RespirationResult != RowRespiration.Result)
                            {
                                isValueDifferent = true;
                            }
                            else if (RowRespiration.IsNull("Result") == true && CurrentModel.RespirationResult != "")
                            {
                                isValueDifferent = true;
                            }
                            else
                            {
                                isValueDifferent = false;
                            }

                            if (isValueDifferent == true)
                            {
                                istoUpdateRow = true;
                                if (!string.IsNullOrEmpty(CurrentModel.RespirationResult))
                                {
                                    RowRespiration.Result = MDVUtility.ToStr(CurrentModel.RespirationResult);
                                }
                                else
                                {
                                    RowRespiration[dsVitals.VitalSignsRespiration.ResultColumn] = DBNull.Value;
                                }
                            }

                            if (isFromGrid == false)
                            {
                                int currentPattern = MDVUtility.ToInt(CurrentModel.Pattern);
                                if (arrRespirationRows.Length < 1)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowRespiration.IsNull("PatternId") == false && currentPattern != RowRespiration.PatternId)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowRespiration.IsNull("PatternId") == true && currentPattern > 0)
                                {
                                    isValueDifferent = true;
                                }
                                else
                                {
                                    isValueDifferent = false;
                                }
                                if (isValueDifferent == true)
                                {
                                    istoUpdateRow = true;
                                    if (MDVUtility.ToInt(CurrentModel.Pattern) > 0)
                                    {
                                        RowRespiration.PatternId = MDVUtility.ToInt(CurrentModel.Pattern);
                                    }
                                    else
                                    {
                                        RowRespiration[dsVitals.VitalSignsRespiration.PatternIdColumn] = DBNull.Value;
                                    }
                                }

                                if (arrRespirationRows.Length < 1)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowRespiration.IsNull("Time") == false && CurrentModel.RespirationTime != RowRespiration.Time)
                                {
                                    isValueDifferent = true;
                                }
                                else if (RowRespiration.IsNull("Time") == true && CurrentModel.RespirationTime != "")
                                {
                                    isValueDifferent = true;
                                }
                                else
                                {
                                    isValueDifferent = false;
                                }
                                if (isValueDifferent == true)
                                {
                                    istoUpdateRow = true;
                                    if (!string.IsNullOrEmpty(CurrentModel.RespirationTime))
                                    {
                                        RowRespiration[dsVitals.VitalSignsRespiration.TimeColumn] = MDVUtility.ToStr(CurrentModel.RespirationTime);
                                    }
                                    else
                                    {
                                        RowRespiration[dsVitals.VitalSignsRespiration.TimeColumn] = DBNull.Value;
                                    }
                                }

                            }

                            if (istoUpdateRow == true)
                            {
                                RowRespiration.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                RowRespiration.CreatedOn = DateTime.Now;
                                RowRespiration.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                RowRespiration.ModifiedOn = DateTime.Now;
                                // if no Respiration is found against RespirationId, it implies for new record
                                if (arrRespirationRows.Length < 1)
                                {
                                    dsVitals.VitalSignsRespiration.AddVitalSignsRespirationRow(RowRespiration);
                                }
                            }

                        }
                        i--;
                    }

                    #region Database Insertion/Updation

                    BLObject<DSVitals> objInsertedRespiration = BLLClinicalObj.InsertUpdateVitalSignsRespiration(dsVitals, PatientId);
                    if (objInsertedRespiration.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message
                            //VitalsId = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows[0][dsVitals.VitalSigns.VitalSignIdColumn.ColumnName]
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objInsertedRespiration.Message
                        };
                        return JsonConvert.SerializeObject(response);
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
                return (JsonConvert.SerializeObject(response));
            }
        }
        */
        public string searchVisitsNotes(VitalModel model)
        {
            try
            {
                DSAppointment dsSchedule = null;
                BLObject<DSAppointment> obj;

                obj = BLLScheduleObj.LoadAppointmentsNotes(model.VisitFrom, model.VisitTo, model.NoteStatus, model.FirstName, model.LastName, model.NoteType, MDVUtility.ToInt64(model.provider), model.AccountNumber, MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt32(model.PageNo), MDVUtility.ToInt32(model.rpp));

                dsSchedule = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count > 0)
                    {
                        var privilegasMessage = JsonConvert.DeserializeObject(MDVision.IEHR.Common.AppPrivileges.GetFormSecurity("Notes_Notes", "DELETE")).ToString();
                        privilegasMessage = privilegasMessage == "" ? "Yes" : "No";
                        var response = new
                        {
                            status = true,
                            NotesCount = dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows[0]["RecordCount"],
                            //SchAppStatusCount = dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count,
                            NotesLoad_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName]),
                            HasDeleteRights = privilegasMessage,
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SchAppStatusCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = true,
                        SchAppStatusCount = 0,
                        Message = obj.Message
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string NotesCount()
        {
            try
            {
                string count = "";
                var listAppointmentNotes = new DALAppointment().LoadAppointmentsNotes_("", "", "Draft", "", "", "", 0, "");
                if (listAppointmentNotes.Count > 0)
                    count = listAppointmentNotes[0].RecordCount;
                var response = new
                {
                    status = true,
                    Count = count,
                };
                return (JsonConvert.SerializeObject(response));

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Count = "",
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string getLatestVitalByPatientId(Int64 PatientId, Int64 userId, Int64 entityId)
        {
            try
            {
                // Now User will get Soap Text to Attach with notes
                // Author : Azhar Shahzad
                // Date: 11 Dec, 2015
                DSVitals dsVitalSignSoap = null;
                BLObject<DSVitals> obj;

                obj = BLLClinicalObj.getLatestVitalByPatientId(PatientId, userId, entityId);
                dsVitalSignSoap = obj.Data;
                if (dsVitalSignSoap.Tables[dsVitalSignSoap.VitalSignSoap.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        VitalSignSoapCount = dsVitalSignSoap.Tables[dsVitalSignSoap.VitalSignSoap.TableName].Rows.Count,
                        VitalSignSoap_JSON = MDVUtility.JSON_DataTable(dsVitalSignSoap.Tables[dsVitalSignSoap.VitalSignSoap.TableName]),
                        VitalSignsBloodPressureSoap_JSON = MDVUtility.JSON_DataTable(dsVitalSignSoap.Tables["Table1"]),
                        VitalSignsPulseSoap_JSON = MDVUtility.JSON_DataTable(dsVitalSignSoap.Tables["Table2"]),
                        VitalSignsTempratureSoap_JSON = MDVUtility.JSON_DataTable(dsVitalSignSoap.Tables["Table3"]),
                        VitalSignsRespirationSoap_JSON = MDVUtility.JSON_DataTable(dsVitalSignSoap.Tables["Table4"]),
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        VitalSignSoapCount = 0,
                        VitalSignSoap_JSON = MDVUtility.JSON_DataTable(dsVitalSignSoap.Tables[dsVitalSignSoap.VitalSignSoap.TableName]),
                        Message = Common.AppPrivileges.No_Record_Message
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        private bool IsValidChange(DataRow drCurrentRow, DataRow[] arrCurrentRows, string colType, string columnName, string columnValue)
        {
            bool isValueDifferent = true;
            if (arrCurrentRows.Length < 1)
            {
                isValueDifferent = true;
            }
            else if (drCurrentRow.IsNull(columnName) == false && columnValue != drCurrentRow[columnName])
            {
                isValueDifferent = true;
            }
            else if (drCurrentRow.IsNull(columnName) == true && (colType.ToLower() == "string" ? columnValue != "" : (colType.ToLower().IndexOf("int") > -1 ? MDVUtility.ToInt64(columnValue) > 0 : MDVUtility.Tofloat(columnValue) > 0)))
            {
                isValueDifferent = true;
            }
            else
            {
                isValueDifferent = false;
            }
            return isValueDifferent;
        }


        internal string getVitalSignsForSoap(string VitalSignId)
        {
            try
            {

                DSVitals dsVitalSignSoap = null;
                BLObject<DSVitals> obj = BLLClinicalObj.loadVitalSignsForSoap(VitalSignId);
                dsVitalSignSoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsVitalSignSoap.Tables[dsVitalSignSoap.VitalSignSoap.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VitalSignSoapCount = dsVitalSignSoap.Tables[dsVitalSignSoap.VitalSignSoap.TableName].Rows.Count,
                            VitalSignSoap_JSON = MDVUtility.JSON_DataTable(dsVitalSignSoap.Tables[dsVitalSignSoap.VitalSignSoap.TableName]),
                            VitalSignsBloodPressureSoap_JSON = MDVUtility.JSON_DataTable(dsVitalSignSoap.Tables["Table1"]),
                            VitalSignsPulseSoap_JSON = MDVUtility.JSON_DataTable(dsVitalSignSoap.Tables["Table2"]),
                            VitalSignsTempratureSoap_JSON = MDVUtility.JSON_DataTable(dsVitalSignSoap.Tables["Table3"]),
                            VitalSignsRespirationSoap_JSON = MDVUtility.JSON_DataTable(dsVitalSignSoap.Tables["Table4"]),

                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VitalSignSoapCount = 0,
                            VitalSignSoap_JSON = MDVUtility.JSON_DataTable(dsVitalSignSoap.Tables[dsVitalSignSoap.VitalSignSoap.TableName]),
                            Message = Common.AppPrivileges.No_Record_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        internal string copyVitalSigns(long VitalSignId, long NotesId)
        {
            try
            {
                #region Database Insertion
                BLObject<string> obj = BLLClinicalObj.copyVitalSignsInsert(VitalSignId, NotesId, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        VitalsId = obj.Data

                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return JsonConvert.SerializeObject(response);
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
                return (JsonConvert.SerializeObject(response));
            }
        }
        //-------------------



        public string updateVitalsActiveInActive(VitalModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.VitalSignsId) > 0)
                {

                    DSVitals dsVitals = new DSVitals();
                    BLObject<DSVitals> obj = BLLClinicalObj.LoadVitals(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.VitalSignsId), 1, 1000);
                    dsVitals = obj.Data;
                    foreach (DSVitals.VitalSignsRow dr in dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows)
                    {

                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.IsActive = false;

                        if (!string.IsNullOrEmpty(model.DeleteComments))
                            dr.DeleteComments = model.DeleteComments;
                        else
                            dr[dsVitals.VitalSigns.DeleteCommentsColumn] = DBNull.Value;
                        if (MDVUtility.ToInt64(dr[dsVitals.VitalSigns.VisitIdColumn]) == 0)
                        {
                            dr[dsVitals.VitalSigns.VisitIdColumn] = DBNull.Value;
                        }
                        if (MDVUtility.ToInt64(dr[dsVitals.VitalSigns.NotesIdColumn]) == 0)
                        {
                            dr[dsVitals.VitalSigns.NotesIdColumn] = DBNull.Value;
                        }

                    }


                    #region Database Updation
                    if (dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows.Count > 0)
                    {
                        BLObject<DSVitals> objUpdate = BLLClinicalObj.updateVitalsActiveInActive(dsVitals);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = AppPrivileges.Update_Message
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = objUpdate.Message
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Vitals not found."
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }



        //FIXME DELETE BELOW FUNCTION LATER
        /*
        public string updateVitalsActiveInActive_(VitalModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.VitalSignsId) > 0)
                {

                    DSVitals dsVitals = new DSVitals();
                    BLObject<DSVitals> obj = BLLClinicalObj.LoadVitals(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.VitalSignsId), 1, 1000);
                    dsVitals = obj.Data;
                    foreach (DSVitals.VitalSignsRow dr in dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows)
                    {

                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        dr.IsActive = false;

                        if (!string.IsNullOrEmpty(model.DeleteComments))
                            dr.DeleteComments = model.DeleteComments;
                        else
                            dr[dsVitals.VitalSigns.DeleteCommentsColumn] = DBNull.Value;
                        if (MDVUtility.ToInt64(dr[dsVitals.VitalSigns.VisitIdColumn]) == 0)
                        {
                            dr[dsVitals.VitalSigns.VisitIdColumn] = DBNull.Value;
                        }
                        if (MDVUtility.ToInt64(dr[dsVitals.VitalSigns.NotesIdColumn]) == 0)
                        {
                            dr[dsVitals.VitalSigns.NotesIdColumn] = DBNull.Value;
                        }

                    }

                    #region Database Updation
                    if (dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows.Count > 0)
                    {
                        BLObject<DSVitals> objUpdate = BLLClinicalObj.UpdateVitals( dsVitals);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = AppPrivileges.Update_Message
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = objUpdate.Message
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Vitals not found."
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        */
    }
}