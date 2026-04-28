using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Immunization 
{
    public class VaccineAndTerapuetic
    {
        public string TherapeuticId { get; set; }
        public string ImmunizationId { get; set; }
        public string TherInjName { get; set; }
        
        public string ImmunizationName { get; set; }
        public string Type { get; set; }
        public string commandType { get; set; }
        public string Status { get; set; }
        public string CVX { get; set; }
        public string CPT { get; set; }
        public string AdminCode { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string RecordCount { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }

        //Vaccine
        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }
        public string AdminCodeDescription { get; set; }
        public string Dose { get; set; }
        public string Amount { get; set; }
        public string ManufactureIds { get; set; }
        public string VISDate { get; set; }
        public string DocumentLink { get; set; }
        public string DocumentName { get; set; }
        public string NDCCode { get; set; }
        public string Id { get; set; }
        public string VaccineVISId { get; set; }
        public bool CptBaseSearch { get; set; }
        public List<VaccineProblem> VaccineProblems { get; set; }
        public List<VaccineVIS> VaccineVisInformation { get; set; }

    }

    public class VaccineProblem
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class VaccineVIS
    {
        public string VaccineVISId { get; set; }
        public string VaccineId { get; set; }
        public string VISDate { get; set; }
        public string VISDocumentName { get; set; }
        public string VaccineVIS_URLId { get; set; }
        public string VISDocumentLink { get; set; }
        public string VISFullyEncodedText { get; set; }
        public string CVX { get; set; }
        public string Mode { get; set; }
    }


}
