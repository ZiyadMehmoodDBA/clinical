using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.FavoriteList
{
    public class FavoriteListImmunizationModel 
    {
        public string Tab { get; set; }
        public string VaccineIds { get; set; }
        public string TherapueticIds { get; set; }
        public string Type { get; set; }
        public string commandType { get; set; }
        public string EntityId { get; set; }
        public string FavoriteListName { get; set; }
        public string ProviderIds { get; set; }
        public bool IsActive { get; set; }
        public string VaccineGroupId { get; set; }
        public string FavoritiesListId { get; set; }
        public bool IsActivePrevious { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string CategoryText { get; set; }
        public string VaccineCVX { get; set; }
        public string VaccineName { get; set; }
        public string CPTCode { get; set; }
        public string AdministeredCode { get; set; }
        public string VaccineID { get; set; }
        public string InjectionId { get; set; }
        public string SearchData { get; set; }
    }
    public class BodyPartModel
    {
        public string BodyPartId { get; set; }
        public string BodyPart { get; set; }
        public string Position { get; set; }
        public bool IsSelected { get; set; }
    }
}