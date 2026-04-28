using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Orderset
{
    public class OS_MedicationModel
    {
        public string Action { get; set; }
        public string Dose { get; set; }
        public string DoseUnit { get; set; }
        public string Route { get; set; }
        public string DoseTiming { get; set; }
        public string DoseOther { get; set; }
        public string Duration { get; set; }
        public string QuantityUnit { get; set; }
        public string Refill { get; set; }
        public string Comments { get; set; }
        public string DirectionsToPharmacist { get; set; }
        public string NDCID { get; set; }
        public string hfMedication { get; set; }
        public string AddDirectionToPatient { get; set; }
        public string PrescriptionsOtherNotes { get; set; }
        public string BrandName { get; set; }
        public string GenericName { get; set; }
        public string Form { get; set; }
        public string Strength { get; set; }
        public string OrdersetId { get; set; }
        public string commandType { get; set; }
        public string DrugName { get; set; }
        public string OS_MedicationId { get; set; }
        public string Quantity { get; set; }
        public string pageNumber { get; set; }
        public string rowsPerPage { get; set; }
        public string RecordCount { get; set; }
        public string ActionName { get; set; }
        public string DoseUnitName { get; set; }
        public string RouteName { get; set; }
        public string DoseTimingName { get; set; }
        public string DoseOtherName { get; set; }
        public string DurationName { get; set; }
        public string QuantityUnitName { get; set; }
        public string RcopiaID { get; set; }
        public string Delete { get; set; }
        public bool alreadyExists { get; set; }
    }
}
