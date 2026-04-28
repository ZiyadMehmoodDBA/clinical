using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Immunization
{
    public class ImmunizationRegistery
    {
        public string RegistryConfigurationId { get; set; }
        public string ProviderId { get; set; }
        public string RegisteryId { get; set; }
        public string IsActive { get; set; }
        public string SendingApplication { get; set; }
        public string ReceivingApplicationId { get; set; }
        public string PoviderFacilityId { get; set; }
        public string SendingFacility { get; set; }
        public string RegistrySubmissionId { get; set; }
        public string Timeslot { get; set; }
        public string IsAdministered { get; set; }
        public string IsRefusal { get; set; }
        public string IsHistoryDose { get; set; }
        public string IsDeleted { get; set; }
        public string Status { get; set; }
        public bool Production { get; set; }
        public bool Testing { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string EntityId { get; set; }
        public string FilesPerBatch { get; set; }
        public string RecordCount { get; set; }
        public string commandType { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
    }
    public class ImmunizationRegisteryWrapperModel
    {
        public long RegistryConfigurationId { get; set; }
        public long ProviderId { get; set; }
        public long RegisteryId { get; set; }
        public bool IsActive { get; set; }
        public string SendingApplication { get; set; }
        public long ReceivingApplicationId { get; set; }
        public string PoviderFacilityId { get; set; }
        public string SendingFacility { get; set; }
        public long RegistrySubmissionId { get; set; }
        public string Timeslot { get; set; }
        public bool IsAdministered { get; set; }
        public bool IsRefusal { get; set; }
        public bool IsHistoryDose { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public long EntityId { get; set; }
        public string FilesPerBatch { get; set; }
    }
}
