using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.Batch.PatientList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Batch.PatientList
{
    public class BatchPatientListHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public BatchPatientListHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static BatchPatientListHelper _instance = null;
        public static BatchPatientListHelper Instance()
        {
            if (_instance == null)
                _instance = new BatchPatientListHelper();
            return _instance;
        }


        #region Load ROS Systems
        private DSBatchPatientList mergeSearchFiltersWithDS(DSBatchPatientList dsPatientList)
        {
            DSBatchPatientList dsPatientListCopy = new DSBatchPatientList();
            foreach (DataRow dr in dsPatientList.BatchPatientList.Rows)
            {
                //DataRow newDr=dsPatientListCopy.BatchPatientList.NewBatchPatientListRow();

                long PatientId = MDVUtility.ToLong(MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.PatientIdColumn.ColumnName]));
                string Medication = MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.MedicationColumn.ColumnName]);
                string MedicationDate = MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.MedicationDateColumn.ColumnName]);

                string Allergy = MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.AllergyColumn.ColumnName]);
                string AllergyDate = MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.AllergyDateColumn.ColumnName]);

                string ProblemName = MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.ProblemNameColumn.ColumnName]);
                string ProblemDate = MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.ProblemDateColumn.ColumnName]);


                string LabResults = MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.LabResultsColumn.ColumnName]);
                string LabResultsDate = MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.LabResultsDateColumn.ColumnName]);
                if (!string.IsNullOrEmpty(Medication) || !string.IsNullOrEmpty(MedicationDate) || !string.IsNullOrEmpty(Allergy) || !string.IsNullOrEmpty(AllergyDate)
                   || !string.IsNullOrEmpty(ProblemName) || !string.IsNullOrEmpty(ProblemDate) || !string.IsNullOrEmpty(LabResults) || !string.IsNullOrEmpty(LabResultsDate))
                {
                    int RowHeight = 0;
                    if (!string.IsNullOrEmpty(Medication) || !string.IsNullOrEmpty(MedicationDate))
                    {
                        RowHeight = Medication.Split('|').Length;
                    }

                    if (!string.IsNullOrEmpty(ProblemName) || !string.IsNullOrEmpty(ProblemDate))
                    {
                        int ProblemNameCount = ProblemName.Split('|').Length;
                        RowHeight = RowHeight > ProblemNameCount ? RowHeight : ProblemNameCount;
                    }
                    if (!string.IsNullOrEmpty(Allergy) || !string.IsNullOrEmpty(AllergyDate))
                    {
                        int AllergyCount = Allergy.Split('|').Length;
                        RowHeight = RowHeight > AllergyCount ? RowHeight : AllergyCount;
                    }
                    if (!string.IsNullOrEmpty(LabResults) || !string.IsNullOrEmpty(LabResultsDate))
                    {
                        int LabResultsCount = LabResults.Split('|').Length;
                        RowHeight = RowHeight > LabResultsCount ? RowHeight : LabResultsCount;
                    }
                    for (int i = 0; i < RowHeight; i++)
                    {
                        if (string.IsNullOrEmpty(LabResults) || (LabResults.Split('|').Length <= i))
                        {
                            dr[dsPatientList.BatchPatientList.LabResultsColumn.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsPatientList.BatchPatientList.LabResultsColumn.ColumnName] = LabResults.Split('|')[i];
                        }
                        if (string.IsNullOrEmpty(LabResultsDate) || (LabResultsDate.Split('|').Length <= i))
                        {
                            dr[dsPatientList.BatchPatientList.LabResultsDateColumn.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsPatientList.BatchPatientList.LabResultsDateColumn.ColumnName] = LabResultsDate.Split('|')[i];
                        }
                        if (string.IsNullOrEmpty(ProblemName) || (ProblemName.Split('|').Length <= i))
                        {
                            dr[dsPatientList.BatchPatientList.ProblemNameColumn.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsPatientList.BatchPatientList.ProblemNameColumn.ColumnName] = ProblemName.Split('|')[i];
                        }
                        if (string.IsNullOrEmpty(ProblemDate) || (ProblemDate.Split('|').Length <= i))
                        {
                            dr[dsPatientList.BatchPatientList.ProblemDateColumn.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsPatientList.BatchPatientList.ProblemDateColumn.ColumnName] = ProblemDate.Split('|')[i];
                        }
                        //
                        if (string.IsNullOrEmpty(Medication) || (Medication.Split('|').Length <= i))
                        {
                            dr[dsPatientList.BatchPatientList.MedicationColumn.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsPatientList.BatchPatientList.MedicationColumn.ColumnName] = Medication.Split('|')[i];
                        }
                        if (string.IsNullOrEmpty(MedicationDate) || (MedicationDate.Split('|').Length <= i))
                        {
                            dr[dsPatientList.BatchPatientList.MedicationDateColumn.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsPatientList.BatchPatientList.MedicationDateColumn.ColumnName] = MedicationDate.Split('|')[i];
                        }
                        if (string.IsNullOrEmpty(Allergy) || (Allergy.Split('|').Length <= i))
                        {
                            dr[dsPatientList.BatchPatientList.AllergyColumn.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsPatientList.BatchPatientList.AllergyColumn.ColumnName] = Allergy.Split('|')[i];
                        }
                        if (string.IsNullOrEmpty(AllergyDate) || (AllergyDate.Split('|').Length <= i))
                        {
                            dr[dsPatientList.BatchPatientList.AllergyDateColumn.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsPatientList.BatchPatientList.AllergyDateColumn.ColumnName] = AllergyDate.Split('|')[i];
                        }
                        dsPatientListCopy.BatchPatientList.ImportRow(dr);
                    }

                }
                else
                {
                    dsPatientListCopy.BatchPatientList.ImportRow(dr);
                }
            }
            return dsPatientListCopy;
        }
        /*
    Author: Muhammad Azhar Shahzad
    Purpose: To Show Grid Data
    Created on April 06, 2016*/
        public string loadBatchPatientList(BatchPatientListModelSearch model)
        {
            try
            {
                DSBatchPatientList dsPatientList = null;
                BLObject<DSBatchPatientList> obj;
                obj = BLLClinicalObj.loadClinical_BatchPatientList(model.ageFrom, model.ageTo, model.gender, model.SmokingStatusId, model.RaceId,
           model.EthnicityId, model.PrefLanguageId, model.PrefCommunicationId, model.Pt_CreationFrom, model.Pt_CreationTo, model.Problems,
          model.ProblemsFrom, model.ProblemsTo, model.Medications, model.MedicationsFrom, model.MedicationsTo, model.Allergies, model.AllergiesFrom,
          model.AllergiesTo, model.LabResults, model.LabResultsFrom, model.LabResultsTo, model.EntityId, model.PageNumber, model.RowsPerPage);

                dsPatientList = obj.Data;
                if (dsPatientList != null && dsPatientList.BatchPatientList.Rows.Count > 0)
                {
                    dsPatientList = mergeSearchFiltersWithDS(dsPatientList);
                    var response = new
                        {
                            status = true,
                            BatchPatientListCount = dsPatientList.Tables[dsPatientList.BatchPatientList.TableName].Rows.Count,
                            iTotalDisplayRecords = dsPatientList.BatchPatientList.Rows[0][dsPatientList.BatchPatientList.RecordCountColumn.ColumnName],
                            BatchPatientListLoad_JSON = MDVUtility.JSON_DataTable(dsPatientList.Tables[dsPatientList.BatchPatientList.TableName]),
                        };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        BatchPatientListCount = 0,
                        iTotalDisplayRecords = 0,
                        BatchPatientListLoad_JSON = MDVUtility.JSON_DataTable(dsPatientList.Tables[dsPatientList.BatchPatientList.TableName]),
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
        #endregion

        /// <summary>
        /// Function for print Patient list
        /// </summary>
        /// <param name="modelFilter"></param>
        /// <returns></returns>
        public string printBatchPatientList(BatchPatientListModelSearch modelFilter)
        {
            StringBuilder str = new StringBuilder();
            try
            {
                DSBatchPatientList dsPatientList = null;
                BLObject<DSBatchPatientList> obj;
                obj = BLLClinicalObj.loadClinical_BatchPatientList(modelFilter.ageFrom, modelFilter.ageTo, modelFilter.gender, modelFilter.SmokingStatusId, modelFilter.RaceId,
           modelFilter.EthnicityId, modelFilter.PrefLanguageId, modelFilter.PrefCommunicationId, modelFilter.Pt_CreationFrom, modelFilter.Pt_CreationTo, modelFilter.Problems,
          modelFilter.ProblemsFrom, modelFilter.ProblemsTo, modelFilter.Medications, modelFilter.MedicationsFrom, modelFilter.MedicationsTo, modelFilter.Allergies, modelFilter.AllergiesFrom,
          modelFilter.AllergiesTo, modelFilter.LabResults, modelFilter.LabResultsFrom, modelFilter.LabResultsTo, modelFilter.EntityId, modelFilter.PageNumber, modelFilter.RowsPerPage);

                dsPatientList = obj.Data;
                if (obj.Data != null)
                {
                    dsPatientList = mergeSearchFiltersWithDS(dsPatientList);
                    if (dsPatientList.Tables[dsPatientList.BatchPatientList.TableName].Rows.Count > 0)
                    {

                        str.Append("<table border=`" + "1px" + "`b>");
                        str.Append("<tr bgcolor='#468cec'>");
                        str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Account</font></b></td>");
                        str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Patient</font></b></td>");
                        str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Gender</font></b></td>");
                        str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>DOB</font></b></td>");
                        str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Smoking</font></b></td>");
                        str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Race</font></b></td>");
                        str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Ethnicity</font></b></td>");
                        str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Language</font></b></td>");
                        str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Communication</font></b></td>");
                        str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Pt. Creation</font></b></td>");
                        if (!string.IsNullOrEmpty(modelFilter.Problems))
                        {
                            str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Problem</font></b></td>");
                            str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Problem Date</font></b></td>");
                        }
                        if (!string.IsNullOrEmpty(modelFilter.Medications))
                        {
                            str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Medication</font></b></td>");
                            str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Medication Date</font></b></td>");
                        }
                        if (!string.IsNullOrEmpty(modelFilter.Allergies))
                        {
                            str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Allergy</font></b></td>");
                            str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Allergy Date</font></b></td>");
                        }
                        if (!string.IsNullOrEmpty(modelFilter.LabResults))
                        {
                            str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Lab Results</font></b></td>");
                            str.Append("<td><b><font face=Arial Narrow size=3 color= #ffffff>Lab Results Date</font></b></td>");
                        }
                        str.Append("</tr>");
                        long xPatientId = 0;
                        foreach (DataRow dr in dsPatientList.Tables[dsPatientList.BatchPatientList.TableName].Rows)
                        {
                            str.Append("<tr>");
                            int rowSpan = 0;
                            long PatientId = MDVUtility.ToInt64(dr[dsPatientList.BatchPatientList.PatientIdColumn.ColumnName]);
                            if (xPatientId != PatientId)
                            {
                                DataRow[] foundRows = dsPatientList.Tables["BatchPatientList"].Select("PatientId=" + PatientId);
                                rowSpan = foundRows.Count();
                                xPatientId = PatientId;
                                str.Append("<td valign='top' rowspan='" + rowSpan + "'>" + MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.AccountNumberColumn.ColumnName]) + "</td>");
                                str.Append("<td valign='top' rowspan='" + rowSpan + "'>" + MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.PatientFullNameColumn.ColumnName]) + "</td>");
                                str.Append("<td valign='top' rowspan='" + rowSpan + "'>" + MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.GenderColumn.ColumnName]) + "</td>");
                                str.Append("<td valign='top' rowspan='" + rowSpan + "'>" + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.DOBColumn.ColumnName])) + "</td>");
                                str.Append("<td valign='top' rowspan='" + rowSpan + "'>" + MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.SmokingStatusColumn.ColumnName]) + "</td>");
                                str.Append("<td valign='top' rowspan='" + rowSpan + "'>" + MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.RaceColumn.ColumnName]) + "</td>");
                                str.Append("<td valign='top' rowspan='" + rowSpan + "'>" + MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.EthnicityColumn.ColumnName]) + "</td>");
                                str.Append("<td valign='top' rowspan='" + rowSpan + "'>" + MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.LanguageColumn.ColumnName]) + "</td>");
                                str.Append("<td valign='top' rowspan='" + rowSpan + "'>" + MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.CommunicationColumn.ColumnName]) + "</td>");
                                str.Append("<td valign='top' rowspan='" + rowSpan + "'>" + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.CreatedOnColumn.ColumnName])) + "</td>");

                            }
                            if (!string.IsNullOrEmpty(modelFilter.Problems))
                            {
                                str.Append("<td>" + MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.ProblemNameColumn.ColumnName]) + "</td>");
                                if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.ProblemDateColumn.ColumnName])))
                                {
                                    str.Append("<td>" + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsPatientList.BatchPatientList.ProblemDateColumn.ColumnName]))) + "</td>");
                                }
                                else
                                {
                                    str.Append("<td></td>");
                                }

                            }
                            if (!string.IsNullOrEmpty(modelFilter.Medications))
                            {
                                str.Append("<td>" + MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.MedicationColumn.ColumnName]) + "</td>");
                                if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.MedicationDateColumn.ColumnName])))
                                {
                                    str.Append("<td>" + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsPatientList.BatchPatientList.MedicationDateColumn.ColumnName]))) + "</td>");
                                }
                                else
                                {
                                    str.Append("<td></td>");
                                }
                            }
                            if (!string.IsNullOrEmpty(modelFilter.Allergies))
                            {
                                str.Append("<td>" + MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.AllergyColumn.ColumnName]) + "</td>");
                                if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.AllergyDateColumn.ColumnName])))
                                {
                                    str.Append("<td>" + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsPatientList.BatchPatientList.AllergyDateColumn.ColumnName]))) + "</td>");
                                }
                                else
                                {
                                    str.Append("<td></td>");
                                }
                            }
                            if (!string.IsNullOrEmpty(modelFilter.LabResults))
                            {
                                str.Append("<td>" + MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.LabResultsColumn.ColumnName]) + "</td>");
                                if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatientList.BatchPatientList.LabResultsDateColumn.ColumnName])))
                                {
                                    str.Append("<td>" + MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsPatientList.BatchPatientList.LabResultsDateColumn.ColumnName]))) + "</td>");
                                }
                                else
                                {
                                    str.Append("<td></td>");
                                }
                            } str.Append("</tr>");

                        }
                        str.Append("</table>");
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return str.ToString();
        }
    }
}