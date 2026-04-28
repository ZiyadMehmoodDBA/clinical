using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Lookups
{
    public class RaceModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }
    public class HierarchicalLookUpModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }
        public List<HierarchicalLookUpModel> Children { get; set; }
    }
    public class CustomModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string LookUpId { get; set; }
        public string DropDown { get; set; }
    }
}
