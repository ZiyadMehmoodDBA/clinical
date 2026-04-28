/*
 Author : Khaleel Ur Rehman
 Date : 9june 2016
 Purpose : CategoryModel class
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical.Immunization
{
    public class CategoryModel
    {
        public string VaccineGroupID { get; set; }
        public string ShortName { get; set; }
        public string SnomedCode { get; set; }
        public string EntityId { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifyOn { get; set; }
        public string commandType { get; set; }

        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
    }
}