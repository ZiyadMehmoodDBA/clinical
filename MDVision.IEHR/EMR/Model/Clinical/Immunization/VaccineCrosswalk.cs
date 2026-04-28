/*
 Author : Azeem Raza Tayyab
 Date : 20 july 2016
 Purpose : VaccineCrosswalk class
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical.Immunization
{
    public class VaccineCrosswalk
    {
        public string VaccineCrosswalkID { get; set; }
        public string VaccineGroupID { get; set; }
        public string VaccineId { get; set; }
        public string IsActive { get; set; }
        public string IsDefault { get; set; }
        public string CVXCode { get; set; }
        public string CPTCode { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifyOn { get; set; }
        public string commandType { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
    }
}