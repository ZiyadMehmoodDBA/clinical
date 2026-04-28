/*
Author : Khaleel Ur Rehman.
Purpose : Model class for ROSCharacteristicsDetails.
Date : 01 Feb 2016.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.ReviewofSystems
{
    public class ROSCharacteristicsDetailsModel
    {
        public long? ROSCharacteristicsDetailsId { get; set; }
        public long? ROSSystemPatientID { get; set; }
        public string PreviousHistory { get; set; }
        public int? ROSCharacteristicsDetailStatusId { get; set; }
        public string Onset { get; set; }
        public float? Duration { get; set; }
        public int? ROSCharacteristicsDetailDurationId { get; set; }
        public int? ROSCharacteristicsDetailPatternId { get; set; }
        public int? ROSCharacteristicsDetailSeverityId { get; set; }
        public int? ROSCharacteristicsDetailCourseId { get; set; }
        public int? ROSCharacteristicsDetailRadiationId { get; set; }
        public int? ROSCharacteristicsDetailFrequencyId { get; set; }
        public int? ROSCharacteristicsDetailContextId { get; set; }
        public int? ROSCharacteristicsDetailCharacterCSZId { get; set; }
        public int? ROSCharacteristicsDetailAggravedById { get; set; }
        public int? ROSCharacteristicsDetailRelievedById { get; set; }
        public string Location { get; set; }
        public string PrecipitatedBY { get; set; }
        public string AssociatedWith { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public long? PatientId { get; set; } //PatientId temporarily added as asked by AzharSial Sb.

        public long NotesId { get; set; }

        public int? ROSSystemID { get; set; }
        public int? ROSCharacteristicsId { get; set; }


        public string ROSCharacteristicsDetailStatusId_text { get; set; }
        public string ROSCharacteristicsDetailDurationId_text { get; set; }
        public string ROSCharacteristicsDetailPatternId_text { get; set; }
        public string ROSCharacteristicsDetailSeverityId_text { get; set; }
        public string ROSCharacteristicsDetailCourseId_text { get; set; }
        public string ROSCharacteristicsDetailRadiationId_text { get; set; }
        public string ROSCharacteristicsDetailFrequencyId_text { get; set; }
        public string ROSCharacteristicsDetailContextId_text { get; set; }
        public string ROSCharacteristicsDetailCharacterCSZId_text { get; set; }
        public string ROSCharacteristicsDetailAggravedById_text { get; set; }
        public string ROSCharacteristicsDetailRelievedById_text { get; set; }
    }

}