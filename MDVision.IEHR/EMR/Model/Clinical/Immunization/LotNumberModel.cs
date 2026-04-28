using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical.Immunization
{
    public class LotNumberModel
    {
        public string SearchText { get; set; }
        public string commandType { get; set; }
        public string VaccineId { get; set; }
        public string VacManufacturerId { get; set; }
        public string VaccineLotNoId { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string LotNo { get; set; }
        public string ExpiryDate { get; set; }
        public string Quantity { get; set; }
        public string QuantityLeft { get; set; }
        public string RouteId { get; set; }
        public string VISDate { get; set; }
        public string Active { get; set; }
        public string EntityId { get; set; }
        public string VaccineName { get; set; }
        public string ManufacturerName { get; set; }
        public string VISDateText { get; set; }
        public string NDCCode { get; set; }
        public string CVXCode { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string HTMLURL { get; set; }
        public string Type { get; set; }
        public string TherapeuticInjectionId { get; set; }
        public string ProviderIds { get; set; }
        public string ProviderNames { get; set; }

        public string ProviderId { get; set; }
        public bool OnlyExpired { get; set; }
        public bool OnlyLowQuantity { get; set; }
        public string Checkprivilegas { get; set; }

        public string LotVaccineIds { get; set; }
        public string VaccineFundingSourceId { get; set; }

    }
}