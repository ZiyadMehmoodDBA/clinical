using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.CCM.CCMHub
{
    public class CallDetailLookupModel
    {
        public string ProviderName { get; set; }
        public string Title { get; set; }
        public string CareTeamId { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class CareTeamLookupModel
    {
        public string CareTeamId { get; set; }
        public string CareTeamName { get; set; }
        public string ProviderName { get; set; }
        public string PCPName { get; set; }
        public string CareManagerName { get; set; }
        public string CareCoordinatorName { get; set; }
        public string CareGiverName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
