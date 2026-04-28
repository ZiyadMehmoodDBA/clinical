using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native
{
    public class UserPrivileges
    {
        public string RoleName { get; set; }
        public string ModuleName { get; set; }
        public string FormName { get; set; }
        public string PrivilegeName { get; set; }
        public bool IsPrivileged { get; set; }
    }
}
