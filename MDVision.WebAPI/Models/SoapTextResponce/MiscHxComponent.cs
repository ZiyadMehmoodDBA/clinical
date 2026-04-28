using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Models.SoapTextResponce
{
    public class MiscHxComponent
    {
        public string ComponentId { get; set; }
        public string UserId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentOrder { get; set; }

        public string SortOrder { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }

       
    }
}