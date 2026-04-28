using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Orthopedic
{
    public class OrthopedicComplaintsModel
    {

        public long OrthopedicComplainId { get; set; }
        public string Complaint { get; set; }
        public long UserId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public long ComplaintDetailId { get; set; }
        public long NotesBodyPartId { get; set; }
        public string BodyPart { get; set; }
        public long NotesId { get; set; }
    }
}
