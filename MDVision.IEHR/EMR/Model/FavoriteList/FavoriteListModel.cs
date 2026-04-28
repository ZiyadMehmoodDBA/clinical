using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.FavoriteList
{
    public class FavoriteListModel
    {
        public Int64 FavoriteListId { get; set; }
        public string FavoriteListName { get; set; }
        public Int64 FavoriteListICDId { get; set; }
        public Int64 FavoriteListCustomFormId { get; set; }
        public Int64 FavoriteListCPTId { get; set; }
        public string ListType { get; set; }
        public string EntityId { get; set; }
        public string BodyPartId { get; set; }
        public Int64 FavoriteListMedicationId { get; set; }

        public Int64 FavoriteListCount { get; set; }
        public Int64 iTotalDisplayRecords { get; set; }
        public bool IsActive { get; set; }

        public string IsActivePrevious { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string commandType { get; set; }

        public int PageNumber { get; set; }

        public int RowsPerPage { get; set; }
        public string FavListVal { get; set; }
        public string LabId { get; set; }
        public string ProviderId { get; set; }

        public List<FavoriteListICDModel> FavoriteListIcd { get; set; }
        public List<MDVision.Model.CustomFormModel> FavoriteListCustomForms { get; set; }

        public List<FavoriteListCPTModel> FavoriteListCPT { get; set; }
        public List<FavoriteListProviderModel> FavoriteListProvider { get; set; }
        public string ProviderIds { get; set; }
        public string CustomFormsIds { get; set; }
        public bool? IsSelectForLookUp { get; set; }
        public Int64 CustomFormIds { get; set; }
        public string SearchData { get; set; }
    }
}