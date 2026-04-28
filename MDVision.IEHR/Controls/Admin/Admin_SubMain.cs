using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_SubMain
    {
        private static Admin_SubMain _obj = null;
        public static Admin_SubMain Instance()
        {
            if (_obj == null)
                _obj = new Admin_SubMain();
            return _obj;
        }
    }
}