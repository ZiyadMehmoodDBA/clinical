using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Model.Common;

namespace MDVision.Model.Clinical.Medical.Implantable
{
    public class DevicesAttachedWithNotes
    {
        public string ImplantableDevicesPKId { get; set; }
        public string NotesId { get; set; }
    }
    public class ImplantableDevicesWithNotes
    {
        public ImplantableDevicesWithNotes()
        {
            this._attachedNotes = new List<DevicesAttachedWithNotes>();
            this._implantableDevices = new List<ImplantableDevices>();
        }
        public List<DevicesAttachedWithNotes> _attachedNotes { get; set; }
        public List<ImplantableDevices> _implantableDevices { get; set; }
    }

    public class ImplantableDevicesWithProcedures
    {
        public ImplantableDevicesWithProcedures()
        {
            this.associatedProcedures = new List<ImplantableDeviceProcedures>();
            this.implantableDevices = new List<ImplantableDevices>();
        }
        public List<ImplantableDeviceProcedures> associatedProcedures { get; set; }
        public List<ImplantableDevices> implantableDevices { get; set; }
    }

    public class InformantLookup
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class LateralityLookup
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class TargetSiteLookup
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ImplantableDeviceProcedures
    {
        public string ImplantableDeviceProcedureId { get; set; }
        public string Procedure { get; set; }
        public string CPTCode { get; set; }
        public string CPTCodeDescription { get; set; }
        public string SnomedID { get; set; }
        public string SnomedDescription { get; set; }
    }

    public class ImplantableDevices
    {
        public string commandType { get; set; }
        public string ImplantableDevicesPKId { get; set; }
        public string PatientId { get; set; }
        public string DeviceIdIssuingAgencyID { get; set; }
        public string DeviceRecordStatus { get; set; }
        public string DevicePublishDate { get; set; }
        public string DeviceCommDistributionEndDate { get; set; }
        public string DeviceCommDistributionStatus { get; set; }
        public string BrandName { get; set; }
        public string VersionModelNumber { get; set; }
        public string CatalogNumber { get; set; }
        public string CompanyName { get; set; }
        public string DeviceCount { get; set; }
        public string DeviceDescription { get; set; }
        public string DMExempt { get; set; }
        public string PremarketExempt { get; set; }
        public string DeviceHCTP { get; set; }
        public string DeviceKit { get; set; }
        public string DeviceCombinationProduct { get; set; }
        public string SingleUse { get; set; }
        public string LotBatch { get; set; }
        public string Serial_Number { get; set; }
        public string DonationIdNumber { get; set; }
        public string LabeledContainsNRL { get; set; }
        public string LabeledNoNRL { get; set; }
        public string MRISafetyStatus { get; set; }
        public string Rx { get; set; }
        public string Otc { get; set; }
        public string DeviceSizes { get; set; }
        public string DeviceSterile { get; set; }
        public string SterilizationPriorToUse { get; set; }
        public string MethodTypes { get; set; }
        public string UDI { get; set; }
        public string Issuing_agency { get; set; }
        public string DI { get; set; }
        public string Donation_Id { get; set; }
        public string Expiration_Date_Original_Format { get; set; }
        public string Expiration_Date_Original { get; set; }
        public string Expiration_Date { get; set; }
        public string Manufacturing_Date_Original_Format { get; set; }
        public string Manufacturing_Date_Original { get; set; }
        public string Manufacturing_Date { get; set; }
        public string Lot_Number { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string RecordCount { get; set; }
        public string GMDNPName { get; set; }
        public string GMDNPTDefinition { get; set; }
        public string ErrorMessage { get; set; }
        public string NotesId { get; set; }
        public string ImplantDate { get; set; }
        public string Status { get; set; }
        public string Area { get; set; }
        public string LateralityId { get; set; }
        public string TargetSiteId { get; set; }
        public string InformantId { get; set; }
        public string ProcedureId { get; set; }
        public string Procedures { get; set; }
        public string Comments { get; set; }
        public string TargetSite { get; set; }
        public string ImplantableDeviceProceduresXML { get; set; }
        public List<ImplantableDeviceProcedures> ImplantableDeviceProcedure { get; set; }
        public string NoteProviderId { get; set; }
    }

    public class ImplantableDeviceGMPTName
    {
        public string ImplantableDeviceGMPTNameId { get; set; }
        public string ImplantableDevicesPKId { get; set; }
        public string DeviceId { get; set; }
        public string GMDNPName { get; set; }
        public string GMDNPTDefinition { get; set; }
    }
    public class ImplantableDeviceIdentifier
    {
        public string ImplantableDeviceIdentifierId { get; set; }
        public string ImplantableDevicesPKId { get; set; }
        public string DeviceId { get; set; }
        public string DeviceIdType { get; set; }
        public string DeviceIdIssuingAgency { get; set; }
        public string ContainsDINumber { get; set; }
        public string PkgQuantity { get; set; }
        public string PkgDiscontinueDate { get; set; }
        public string PkgStatus { get; set; }
        public string PkgType { get; set; }
    }

}