using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.CommonModels
{
    public class FormPrivilegeModel
    {
        public FormPrivilegeModel()
        {
            this.FormName = new List<string>();
            this.Permission = new List<string>();
        }
        public List<string> FormName { get; set; }
        public List<string> Permission { get; set; }
    }

    public class FormPrivileg_ResponseeModel
    {
        public  FormPrivileg_ResponseeModel(){
            this.FormName = new List<string>();
            this.Permission = new List<string>();
            this.privilegasMessage = new List<string>();
        }
        public List<string> FormName { get; set; }
        public List<string> Permission { get; set; }
        public List<string> privilegasMessage { get; set; }
    }
}