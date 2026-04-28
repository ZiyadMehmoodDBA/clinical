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
    public class Vaccine
    {
        public string VaccineID { get; set; }
        public string VaccineGroupID { get; set; }
        public string VaccineStatus { get; set; }
        public string CVXShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string CVXCode { get; set; }
        public string CPTCode { get; set; }
        public string CategoryName { get; set; }
        public string commandType { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
    }
}