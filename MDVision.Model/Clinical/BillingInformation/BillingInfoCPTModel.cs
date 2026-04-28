using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.BillingInformation
{
    public class BillingInfoCPTModel
    {
        public string BillingInfoCPTId { get; set; }
        public string BillingInfoId { get; set; }
        public string CPTCode { get; set; }
        public string CPTDescription { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string DxPointer1 { get; set; }
        public string DxPointer2 { get; set; }
        public string DxPointer3 { get; set; }
        public string DxPointer4 { get; set; }
        public string UnitsId { get; set; }
        public string DOSFrom { get; set; }
        public string DOSTo { get; set; }
        public string POS { get; set; }
        public string CPTSNOMEDCodeId { get; set; }
        public string CPTSNOMEDDescription { get; set; }
        public string Type { get; set; }
        public string BillingInfoTimeId { get; set; }
        public string IsLabBasedCPT { get; set; }
        public string CustomFormId { get; set; }
        public string ShowCPTCode { get; set; }
    }
}
