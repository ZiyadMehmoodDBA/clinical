/*  
    Author: Muhammad Azhar Shahzad
    Creation Date: 23 jan 2017
    OverView:This File Is created for CQM Functionality
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical
{
    public class CQMModel
    {
        public string PatientId { get; set; }
        public string MeasureId { get; set; }
        public string NoteId { get; set; }
        public string Systolic { get; set; }
        public string SystolicLOINC { get; set; }
        public string Diastolic { get; set; }
        public string DiastolicLOINC { get; set; }
        public string SNOMED { get; set; }
        public string CPT { get; set; }
        public string CVX { get; set; }
        public string commandType { get; set; }
        public string HCPCS { get; set; }
        public string RXNORM { get; set; }
        public string LOINC { get; set; }
        public string ReportFromDate { get; set; }
        public string ReportToDate { get; set; }
        public string ICD9CM { get; set; }
        public string ICD10CM { get; set; }
        public string BMI { get; set; }
        public string BMILOINC { get; set; }
        public string ActionResult { get; set; }
        public string bSignNote { get; set; }
    }
}