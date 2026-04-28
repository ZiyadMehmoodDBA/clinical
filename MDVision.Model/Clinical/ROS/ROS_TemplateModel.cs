using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.ROS
{
    public class ROS_TemplateModel
    {
        public long ROStemplateId { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public bool? IsPositive { get; set; }
        public string Comments { get; set; }
        public long ROSSystemInfoID { get; set; }
    }
    public class ROS_DetailModel
    {
        public long ROSDetailId { get; set; }
        public string Name { get; set; }
        public long NotesId { get; set; }
        public string Comments { get; set; }
        public long ROSTemplateId { get; set; }
        public long? ROSDataTemplateId { get; set; }
        public bool? IsPositive { get; set; }
        public long? ParentId { get; set; }
        public long? ROSSystemInfoID { get; set; }
        public string commandType { get; set; }
        public List<ROS_DetailUpdateModel> ROSDetailList { get; set; }
    }
public class ROS_DetailUpdateModel
{
        public ROS_DetailUpdateModel() { }
    public long ROSDetailId { get; set; }
    public string Comments { get; set; }
    public bool? IsPositive { get; set; }
    public long? ParentId { get; set; }
    }
    public class ROS_DetailFillModel
    {
        public long ROSDetailId { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string IsPositive { get; set; }
        public string Comments { get; set; }
    }
}
