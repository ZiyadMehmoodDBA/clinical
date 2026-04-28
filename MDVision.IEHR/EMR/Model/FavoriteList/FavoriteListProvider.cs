using System;

namespace MDVision.IEHR.EMR.Model.FavoriteList
{
    public class FavoriteListProviderModel
    {
        public Int64 FavoriteListId { get; set; }
        public Int64 FavListProviderId { get; set; }
        public string ProviderId { get; set; }
    }
}