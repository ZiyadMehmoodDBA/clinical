using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Notes
{
    public class Syndromic
    {
        public List<SyndromicPatientModel> SyndromicPatientModel { get; set; }
        public List<SyndromicNotesModel> SyndromicNotesModel { get; set; }
        public List<SyndromicProviderModel> SyndromicProviderModel { get; set; }
        public List<SyndromicFacilityModel> SyndromicFacilityModel { get; set; }
        public List<SyndromicObservationModel> SyndromicObservationModel { get; set; }
        public List<SyndromicVitalsModel> SyndromicVitalsModel { get; set; }

        public Syndromic()
        {
            SyndromicPatientModel = new List<SyndromicPatientModel>();
            SyndromicNotesModel = new List<SyndromicNotesModel>();
            SyndromicProviderModel = new List<SyndromicProviderModel>();
            SyndromicFacilityModel = new List<SyndromicFacilityModel>();
            SyndromicVitalsModel = new List<SyndromicVitalsModel>();
        }
    }
    public class SyndromicPatientModel
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string RaceCodeAndName { get; set; }
        public string EthnicityCodeAndName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountyParishCode { get; set; }
        public string ZipCode { get; set; }
        public string DOD { get; set; }
        public string DOB { get; set; }
    }
    public class SyndromicNotesModel
    {
        public string EncounterReason { get; set; }
        public string NoteCreationDate { get; set; }
        public string NotesID { get; set; }
        public string VisitId { get; set; }
    }
    public class SyndromicProviderModel
    {
        public string ProviderName { get; set; }
        public string NPI { get; set; }
    }
    public class SyndromicFacilityModel
    {
        public string FacilityName { get; set; }
        public string NPI { get; set; }
    }
    public class SyndromicObservationModel
    {
        public string ICD10Code { get; set; }
        public string ICD10Description { get; set; }
        public string SNOMED_Description { get; set; }
        public string TobaccoStatus { get; set; }
        public string TobaccoStatusSCT { get; set; }
    }
    public class SyndromicVitalsModel
    {
        public string Height { get; set; }
        public string Weight { get; set; }
        public string ComplaintDescription { get; set; }
        public string OverallComments { get; set; }
    }
}
