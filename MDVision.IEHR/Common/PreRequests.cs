using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;

using MDVision.IEHR.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace MDVision.IEHR.Common
{
    public class PreRequests
    {
        public PreRequestModel ApplicationServerContent()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PreRequestModel>(MDVSession.Current.RequestModel);
        }
    }


}
