using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native.Clinical
{
    public class SocialHxModelNative : NativeBaseModel
    {
        public string SocialHxId { get; set; }
        public string UserId { get; set; }
        public string PatientId { get; set; }
        public string SocialHxDate { get; set; }
        public string SocialHxUnremarkable { get; set; }
        public string SocialComments { get; set; }
        public string commandType { get; set; }
        public string SocialHxType { get; set; }

        /*
             Change Implement BY: Muhammad Azhar Shahzad
             Reason: This property is being used to attach Social History to Progress note
             Created Date: Dec 15, 2015
         */
        public long NotesId { get; set; }

        // Date: 14/01/2016
        // Author: Muhammad Irfan
        // Overview: This property is used for sorting miscHx sorting
        public string MiscComponentSortedOrder { get; set; }
    }
}
