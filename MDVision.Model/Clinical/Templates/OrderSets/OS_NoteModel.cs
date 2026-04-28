using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Templates.OrderSets
{
    public class OS_NoteModel
    {
        public string NoteOSId { get; set; }
        public string NoteId { get; set; }
        public string CDSId { get; set; }
        public string OrderSetId { get; set; }
        public string IsDeleted { get; set; }
        public string OrderSetComponents { get; set; }

        public string commandType { get; set; }
        public string IsDefaultOrderSet { get; set; }

    }

    public class OS_NoteResponse
    {
        public bool status { get; set; }
        public string Message { get; set; }
        public string NoteOSId { get; set; }
    }

}
