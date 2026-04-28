using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDVision.Model.Security
{
    public class SoftwareCustomerInfoModel
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string CustomerRegCode { get; set; }
        public string EntityId { get; set; }
        public string EntityRegCode { get; set; }
        public string IsAdmin { get; set; }
        // Mapp-128 IsFullSSN field is required to set it from webAPI
        public string IsFullSSN { get; set; }
        public string IsMobileLogin { get; set; }
        public string MobSessionExpTime { get; set; }
        public List<FacilityList> Facilities { get; set; }
        
    }
    public class EntityList
    {
        public string EntityId { get; set; }
        public string EntityRegCode { get; set; }
        public string FacilityId { get; set; }
        public string FacilityName { get; set; }
    }
    public class EntityData : IBaseModel
    {
        public string NetWorkIP { get; set; }
        public string EntityId { get; set; }
        public string ErrorMessage { get; set; }
        public string UserId { get; set; }
        public string FacilityId { get; set; }
        public string DeviceId { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ConfigurationId { get; set; }
        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            ConfigurationId = ModelUtility.ToStr(ModelUtility.MapValue(reader, "ConfigurationId", incommingColumnList));
        }
    }
    public class Entity
    {
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public List<FacilityList> Facilities{ get; set; }


    }
    public class FacilityList
    {
        public string FacilityId { get; set; }
        public string FacilityName { get; set; }


    }

    public class SoftwareCustomerInfoModelwithEintiy
    {
        public List<SoftwareCustomerInfoModel> SoftwareCustomerInfoModelList { get; set; }
        public List<EntityList> CustomerRegCodeList { get; set; }
    }

    public class SoftwareCustomerInfoModel_
    {
        public string CustomerId { get; set; }
        public string CustomerRegCode { get; set; }
        public string ReportURL { get; set; }
        public string DomainName { get; set; }
        public string DomainUserName { get; set; }
        public string DomainPassword { get; set; }
        //public string ShortName { get; set; }
        public string IsActive { get; set; }
        //public string Token { get; set; }
        public string WebServiceURL { get; set; }
        public string DBName { get; set; }
        public string DataSource { get; set; }
        public string DBUserId { get; set; }
        public string DBPassword { get; set; }
        public string PersistSecurityInfo { get; set; }
        public string IsProxy { get; set; }
        public string PoolingString { get; set; }
        public string ProviderType { get; set; }
        public string IsTestDatabase { get; set; }
        public string NoOfLicenses { get; set; }
        public string NoOfUsers { get; set; }
        //public string Description { get; set; }
        //public string Address1 { get; set; }
        //public string Address2 { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string ZIPCode { get; set; }
        //public string ZIPCodeExt { get; set; }
        //public string Country { get; set; }
        //public string Email { get; set; }
        //public string WebSiteURL { get; set; }
        //public string PhoneNo { get; set; }
        //public string FaxNo { get; set; }
        //public string MessageAfterLogin { get; set; }
        //public string CustomerEntityId { get; set; }
        public string EntityRegCode { get; set; }
        public string EntityId { get; set; }
        public string UniqueClientId { get; set; }
        public string CustomerConnectionString { get; set; }
        public string WebServerURL { get; set; }
        public string UserId { get; set; }
        public string IsAdmin { get; set; }
        public string DateFormats { get; set; }
        public string IMOHostName { get; set; }
        public string IMOCPTPort { get; set; }
        public string IMOICDPort { get; set; }
        public string IMO_ID { get; set; }
        public string OCRLicenseKey { get; set; }
        public string FileSize { get; set; }
        public string Ftp_PortNo { get; set; }
        public string Docs_HostName { get; set; }
        public string Docs_Alias { get; set; }
        public string RefreshTime { get; set; }
        public string Currency { get; set; }
        public string DecimalPlaces { get; set; }
        public string ClaimScrubberEDIServer { get; set; }
        public string ClaimScrubberPassword { get; set; }
        public string ClaimScrubberSubmitterID { get; set; }
        public string ClaimScrubberUser { get; set; }
        public string IsPasswordExpired { get; set; }
        public string PasswordRegex { get; set; }
        //public string ErrorMessage { get; set; }
        public string DefaultEntity { get; set; }
        public string isFirstTimeLoggedIn { get; set; }
        public string ProviderBulkSign { get; set; }


    }
    public class NetworkIp : IBaseModel
    {
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string IPAdress { get; set; }
        public string ErrorMessage { get; set; }
        public void Map(IDataReader reader, List<string> incommingColumnList)
        {
            IPAdress = ModelUtility.ToStr(ModelUtility.MapValue(reader, "IP", incommingColumnList));
        }
    }
}
