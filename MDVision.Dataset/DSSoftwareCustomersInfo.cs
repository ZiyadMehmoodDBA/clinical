/*
'=================================================================================
' Copyright (C) 2014-2015 MDVision Pvt
' All rights reserved.
'=================================================================================
 
 * Namespace:          : MDVision.Datasets
 * Class               : SoftwareCustomersInfo.cs 
 * Author              : Muhammad Naeem.
 * Created Date        : September 10,2014
 * Last Modified By    :  
 * Last Modified Date  :  

---------------------------------------------------------------------------------------------------------*/
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;



namespace MDVision.Datasets {
    public class DSSoftwareCustomersInfo : DataSet
    {


        //===================================================================================
        // Class Constants (Tables Name)
        //===================================================================================
        public const string TABLE_SOFTWARE_CUSTOMERS_INFO = "SoftwareCustomersInfo";
        //===================================================================================
        //Customer Login Info Table Fields     
        //===================================================================================

        public const string FIELD_CUSTOMER_ID = "CustomerId";
        public const string FIELD_CUSTOMER_REG_CODE = "CustomerRegCode";
        // reporting use
        public const string FIELD_REPORTURL = "ReportURL";
        public const string FIELD_DOMAINNAME = "DomainName";
        public const string FIELD_DOMAINUSERNAME = "DomainUserName";
        public const string FIELD_DOMAINPASSWORD = "DomainPassword";
        // end
        public const string FIELD_SHORT_NAME = "ShortName";
        public const string FIELD_ACTIVE = "IsActive";
        public const string FIELD_TOKEN = "Token";
        public const string FIELD_WEB_SERVICE_URL = "WebServiceURL";
        public const string FIELD_DB_NAME = "DBName";
        public const string FIELD_DATA_SOURCE = "DataSource";
        public const string FIELD_DB_USER_ID = "DBUserId";
        public const string FIELD_DB_PASSWORD = "DBPassword";
        public const string FIELD_PERSIST_SECURITY_INFO = "PersistSecurityInfo";
        public const string FIELD_IS_PROXY = "IsProxy";
        public const string FIELD_POOLING_STRING = "PoolingString";
        public const string FIELD_PROVIDER_TYPE = "ProviderType";
        public const string FIELD_IS_TEST_DATABASE = "IsTestDatabase";
        public const string FIELD_NO_OF_LICENSES = "NoOfLicenses";
        public const string FIELD_NO_OF_USERS = "NoOfUsers";

        public const string FIELD_DESCRIPTION = "Description";
        public const string FIELD_ADDRESS1 = "Address1";
        public const string FIELD_ADDRESS2 = "Address2";
        public const string FIELD_CITY = "City";

        public const string FIELD_STATE = "State";
        public const string FIELD_ZIP_Code = "ZIPCode";
        public const string FIELD_ZIPCodeExt = "ZIPCodeExt";
        public const string FIELD_COUNTRY = "Country";

        public const string FIELD_EMAIL = "Email";
        public const string FIELD_WEBSITE_URL = "WebSiteURL";
        public const string FIELD_PHONENO = "PhoneNo";
        public const string FIELD_FAXNO = "FaxNo";

        public const string FIELD_MESSAGE_AFTER_LOGIN = "MessageAfterLogin";
        public const string FIELD_CUSTOMER_ENTITY_ID = "CustomerEntityId";
        public const string FIELD_ENTITY_REG_CODE = "EntityRegCode";
        public const string FIELD_ENTITY_ID = "EntityId";
        public const string FIELD_UNIQUE_CLIENT_ID = "UniqueClientId";

        public const string FIELD_CUSTOMER_CONNECTION_STRING = "CustomerConnectionString";
        public const string FIELD_CUSTOMER_WEB_SERVER_URL = "WebServerURL";
        public const string FIELD_USER_ID = "UserId";
        public const string FIELD_IS_ADMIN = "IsAdmin";
        public const string FIELD_DATE_FORMAT = "DateFormats";
        // ForIMO CPT Code
        public const string FIELD_IMO_HOST_NAME = "IMOHostName";
        public const string FIELD_IMO_CPT_PORT = "IMOCPTPort";
        public const string FIELD_IMO_ICD_PORT = "IMOICDPort";
        public const string FIELD_IMO_ID = "IMO_ID";
        public const string FIELD_OCR_LICENSEKEY = "OCRLicenseKey";
        public const string FIELD_FILE_SIZE = "FileSize";

        public const string FIELD_FTP_PORT_NO = "Ftp_PortNo";
        public const string FIELD_DOCS_HOST_NAME = "Docs_HostName";
        public const string FIELD_DOCS_ALIAS = "Docs_Alias";

        public const string FIELD_REFRESH_TIME = "RefreshTime";

        public const string FIELD_CURRENCY = "Currency";

        public const string FIELD_DECIMAL_PLACES = "DecimalPlaces";

        public const string FIELD_CLAIM_SCRUBBER_EDI_SERVER = "ClaimScrubberEDIServer";
        public const string FIELD_CLAIM_SCRUBBER_PASSWORD = "ClaimScrubberPassword";
        public const string FIELD_CLAIM_SCRUBBER_SUBMITTER_ID = "ClaimScrubberSubmitterID";
        public const string FIELD_CLAIM_SCRUBBER_USER = "ClaimScrubberUser";
        public const string FIELD_IS_PASSWORD_EXPIRED = "IsPasswordExpired";
        public const string FIELD_PASSWORD_REGEX = "PasswordRegex";
        public const string FIELD_ERROR_MESSAGE = "ErrorMessage";
        public const string FIELD_DEFAULT_ENTITY = "DefaultEntity";
        public const string FIELD_First_Time_Login = "isFirstTimeLoggedIn";





        //----------------------------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------------------------
        public DSSoftwareCustomersInfo()            : base()
        {
            BuildSoftwareCustomersInfoDataTables();
         
        }


        //===================================================================================
        //Creating  Data Table
        //===================================================================================

        //----------------------------------------------------------------------------------------
        // Sub Routine:        BuildSoftwareCustomersInfoDataTables
        // Description:        Create Data Table In Dataset  i-e "DataTable SoftwareCustomersInfo"
        // Input Parameters:   [NONE]
        // Output Parameters:  [NONE]
        //-----------------------------------------------------------------------------------------

        private void BuildSoftwareCustomersInfoDataTables()
        {
            DataTable table = new DataTable(TABLE_SOFTWARE_CUSTOMERS_INFO);
       
            var _with = table.Columns;
            _with.Add(FIELD_CUSTOMER_ID, typeof(System.Int64));
            _with.Add(FIELD_CUSTOMER_REG_CODE, typeof(System.String));

            _with.Add(FIELD_REPORTURL, typeof(System.String));
            _with.Add(FIELD_DOMAINNAME, typeof(System.String));
            _with.Add(FIELD_DOMAINUSERNAME, typeof(System.String));
            _with.Add(FIELD_DOMAINPASSWORD, typeof(System.String));

            _with.Add(FIELD_SHORT_NAME, typeof(System.String));
            _with.Add(FIELD_ACTIVE, typeof(System.Boolean));
            _with.Add(FIELD_TOKEN, typeof(System.String));
            _with.Add(FIELD_WEB_SERVICE_URL, typeof(System.String));
            _with.Add(FIELD_DATA_SOURCE, typeof(System.String));
            _with.Add(FIELD_DB_NAME, typeof(System.String));
            _with.Add(FIELD_DB_USER_ID, typeof(System.String));
            _with.Add(FIELD_DB_PASSWORD, typeof(System.String));
            _with.Add(FIELD_PERSIST_SECURITY_INFO,typeof(System.String));
            _with.Add(FIELD_IS_PROXY, typeof(System.Boolean));
            _with.Add(FIELD_NO_OF_LICENSES, typeof(System.Int32));
            _with.Add(FIELD_NO_OF_USERS, typeof(System.String));
            _with.Add(FIELD_POOLING_STRING, typeof(System.String));
            _with.Add(FIELD_IS_TEST_DATABASE, typeof(System.Boolean));
                   

            _with.Add(FIELD_DESCRIPTION, typeof(System.String));
            _with.Add(FIELD_ADDRESS1, typeof(System.String));
            _with.Add(FIELD_ADDRESS2, typeof(System.Int16));
            _with.Add(FIELD_CITY, typeof(System.String));
            _with.Add(FIELD_STATE, typeof(System.String));
            _with.Add(FIELD_ZIP_Code, typeof(System.String));
            _with.Add(FIELD_ZIPCodeExt, typeof(System.String));
            _with.Add(FIELD_COUNTRY, typeof(System.String));
            _with.Add(FIELD_EMAIL, typeof(System.String));

            _with.Add(FIELD_WEBSITE_URL, typeof(System.String));
            _with.Add(FIELD_PHONENO, typeof(System.String));
            _with.Add(FIELD_FAXNO, typeof(System.String));
            _with.Add(FIELD_MESSAGE_AFTER_LOGIN, typeof(System.String));
            _with.Add(FIELD_CUSTOMER_ENTITY_ID, typeof(System.Int64 ));
            _with.Add(FIELD_ENTITY_REG_CODE, typeof(System.String));
            _with.Add(FIELD_ENTITY_ID, typeof(System.Int64));
            _with.Add(FIELD_UNIQUE_CLIENT_ID, typeof(System.String));
            _with.Add(FIELD_CUSTOMER_CONNECTION_STRING, typeof(System.String));
            _with.Add(FIELD_PROVIDER_TYPE, typeof(System.String));
            _with.Add(FIELD_CUSTOMER_WEB_SERVER_URL, typeof(System.String));
            _with.Add(FIELD_USER_ID, typeof(System.Int64));
            _with.Add(FIELD_IS_ADMIN, typeof(System.Boolean));
            _with.Add(FIELD_DATE_FORMAT, typeof(System.String));

            _with.Add(FIELD_IMO_HOST_NAME, typeof(System.String));
            _with.Add(FIELD_IMO_CPT_PORT, typeof(System.String));
            _with.Add(FIELD_IMO_ICD_PORT, typeof(System.String));
            _with.Add(FIELD_IMO_ID, typeof(System.String));
            _with.Add(FIELD_OCR_LICENSEKEY, typeof(System.String));
            _with.Add(FIELD_FILE_SIZE, typeof(System.Int32));

              _with.Add(FIELD_FTP_PORT_NO, typeof(System.String));
              _with.Add(FIELD_DOCS_HOST_NAME, typeof(System.String));

              _with.Add(FIELD_DOCS_ALIAS, typeof(System.String));
              _with.Add(FIELD_REFRESH_TIME, typeof(System.String));
              _with.Add(FIELD_CURRENCY, typeof(System.String));
              _with.Add(FIELD_DECIMAL_PLACES, typeof(System.String));

            _with.Add(FIELD_CLAIM_SCRUBBER_EDI_SERVER, typeof(System.String));
            _with.Add(FIELD_CLAIM_SCRUBBER_PASSWORD, typeof(System.String));
            _with.Add(FIELD_CLAIM_SCRUBBER_SUBMITTER_ID, typeof(System.String));
            _with.Add(FIELD_CLAIM_SCRUBBER_USER, typeof(System.String));
            _with.Add(FIELD_IS_PASSWORD_EXPIRED, typeof(System.Boolean));
            _with.Add(FIELD_PASSWORD_REGEX, typeof(System.String));
            _with.Add(FIELD_ERROR_MESSAGE, typeof(System.String));
            _with.Add(FIELD_DEFAULT_ENTITY, typeof(System.String));
            _with.Add(FIELD_First_Time_Login, typeof(System.Boolean));

            //===================================================================================
            //Define Primary Key Column
            //===================================================================================
            DataColumn PrimaryKeyColumn1 = table.Columns[FIELD_CUSTOMER_REG_CODE];
            DataColumn PrimaryKeyColumn2 = table.Columns[FIELD_ENTITY_REG_CODE];
            DataColumn PrimaryKeyColumn3 = table.Columns[FIELD_USER_ID];
            table.PrimaryKey = new DataColumn[] { PrimaryKeyColumn1 , PrimaryKeyColumn2,PrimaryKeyColumn3};

       
            this.Tables.Add(table);
        }
        
       

    }
}