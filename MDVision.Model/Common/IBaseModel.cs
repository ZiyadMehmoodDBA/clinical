using System;
using System.Collections.Generic;
using System.Data;

namespace MDVision.Model.Common
{

    public interface IBaseModel
    {
        string IsActive { get; set; }
        string CreatedBy { get; set; }
        string CreatedOn { get; set; }
        string ModifiedBy { get; set; }
        string ModifiedOn { get; set; }
        void Map(IDataReader reader, List<string> incommingColumnList);
    }
}
