using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Clinical.Immunization
{
    public class TherapeuticInjectionModel
    {
        public string ManufacturerName { get; set; }
        public string TherapeuticInjectionId { get; set; }
        public string VisitDate { get; set; }
        public string VisitDate_text { get; set; }
        public string ProviderId { get; set; }
        public string AdministrationDate { get; set; }
        public string AdministrationTime { get; set; }
        public string Dose { get; set; }
        public string Amount { get; set; }
        public string ManufacturerId { get; set; }
        public string RouteId { get; set; }
        public string SiteId { get; set; }
        public string ExpiryDate { get; set; }
        public string VFC { get; set; }
        public string ReactionId { get; set; }
        public string Comments { get; set; }
        public string PatientId { get; set; }
        public string commandType { get; set; }

        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string NoteId { get; set; }
        public string ImmTherInjectionId { get; set; }
        public string ImmTherInjectionIdsForSoapText { get; set; }

        public string NotesId { get; set; }
        public string SourceOfHx { get; set; }
        public string Type { get; set; }
        public string TherapeuticInjection { get; set; }
        public string CPTCode { get; set; }
        public string LotNumber { get; set; }
        public string LotText { get; set; }
        public string LinkedWithAnyNote { get; set; }
        public string GivenBy { get; set; }

        public string OSImmTherInjectionId { get; set; }
        public string OrderSetId { get; set; }
    }

    public class TherapeuticInjectionHistoryModel
    {
        public string TherapeuticInjectionIdHistory { get; set; }
        public string ProviderIdHistory { get; set; }
        public string AdministrationDateHistory { get; set; }
        public string AdministrationTimeHistory { get; set; }
        public string DoseHistory { get; set; }
        public string AmountHistory { get; set; }
        public string RouteIdHistory { get; set; }
        public string SiteIdHistory { get; set; }
        public string CommentsHistory { get; set; }
        public string commandType { get; set; }
        public string ImmTherInjectionId { get; set; }
        public string ImmTherInjectionIdsForSoapText { get; set; }
        public string SourceOfHx { get; set; }
        public string Type { get; set; }
        public string TherapeuticInjection { get; set; }
        public string CPTCode { get; set; }
        public string LinkedWithAnyNote { get; set; }
        public string GivenBy { get; set; }
        public string OSImmTherInjectionId { get; set; }
        public string OrderSetId { get; set; }
    }
}