using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Orthopedic
{
    public class OrthopedicModel
    {
        public long PatientId { get; set; }
        public long NotesId { get; set; }
        public string BodyPart { get; set; }
        public string Position { get; set; }
        public bool IsSelected { get; set; }
        public string OrthopedicComplainId { get; set; }
        public string Complaint { get; set; }
        public long ComplaintId { get; set; }
        public long ComplaintDetailId { get; set; }
        public long NotesBodyPartId { get; set; }
        public bool IsDeleteBodyPartAssociation { get; set; }

        public string commandType { get; set; }
        public List<OrthoComplaints> Complaints { get; set; }

        public OrthopedicModel()
        {
            IsSelected = false;
            Complaints = new List<Orthopedic.OrthoComplaints>();
        }
    }

    public class OrthoComplaints
    {
        public long ComplaintDetailId { get; set; }
        public string ComplaintDescription { get; set; }
    }
    public class OrthoFavListModel
    {
        public long FavoriteListId { get; set; }
        public string Name { get; set; }
        public string ListType { get; set; }
        public string FavListBodyPartIds { get; set; }
        public string BodyPartId { get; set; }
        public string BodyPart { get; set; }
        public long ProviderId { get; set; }
        public string commandType { get; set; }
    }
}
