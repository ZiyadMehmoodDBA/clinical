using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Admin.Provider
{
    public class ProviderModel
    {
        public ProviderModel()
        {
            this.ListProviderCPTs = new List<ProviderCPTs>();

        }
        public string ProviderCPTsXML { get; set; }
        public List<ProviderCPTs> ListProviderCPTs { get; set; }
    }
    public class ProviderCPTs
    {
        public string ProcedureId { get; set; }
        public string CPTId { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public string SNOMED_Description { get; set; }
        public string SNOMEDID { get; set; }
        public string Description { get; set; }
        public string ShowCPTCode { get; set; }
    }
}
