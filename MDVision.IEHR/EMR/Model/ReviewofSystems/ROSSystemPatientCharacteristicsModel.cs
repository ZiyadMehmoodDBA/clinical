/*
Author : Khaleel Ur Rehman.
Purpose : Model class for ROSSystemPatientCharacteristics.
Date : 01 Feb 2016.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.ReviewofSystems
{
    public class ROSSystemPatientCharacteristicsModel
    {
        public long ROSSystemPatientCharacteristicsID { get; set; }
        public long ROSSystemPatientID { get; set; }
        public long ROSSystemCharacteristicsId { get; set; }
        public int SystemID { get; set; }
        public int CharacteristicsId { get; set; }
        public string Description { get; set; }
        public bool IsPositive { get; set; }
       
        public string CharcName { get; set; }
        public bool RemoveSystemCharcDetails { get; set; }
    }
}