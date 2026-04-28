using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Model.Clinical.Medical.Implantable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALImplantable
    {
        #region "Stored Procedures Names"
        private const string PROC_IMPLANTABLE_DEVICES_SELECT = "Clinical.sp_ImplantableDeviceSelect1";
        private const string PROC_IMPLANTABLE_DEVICES_INSERT = "Clinical.sp_ImplantableDeviceInsert";
        private const string PROC_IMPLANTABLE_DEVICES_UPDATE = "Clinical.sp_ImplantableDeviceUpdate";
        private const string PROC_IMPLANTABLE_DEVICES_FILL = "Clinical.sp_ImplantableDeviceSelect";
        private const string PROC_IMPLANTABLE_DEVICES_DELETE = "Clinical.sp_ImplantableDeviceDelete";
        private const string PROC_IMPLANTABLE_DEVICES_ACTIVE_INACTIVE = "Clinical.Sp_implantabledeviceActiveInactive";

        private const string PROC_TARGET_SITE_LOOKUP = "Clinical.sp_TargetSitesSelect";
        private const string PROC_LATERALITY_LOOKUP = "Clinical.sp_LateralityLookup";
        private const string PROC_INFORMANT_LOOKUP = "Clinical.sp_InformantLookup";
        //private const string PROC_GET_LATEST_DEVICES_BY_PATIENT = "";
        private const string PROC_GET_DEVICES_FOR_SOAP = "clinical.sp_GetImplantableDevicesforSoap";
        private const string PROC_DETACH_DEVICES_FROM_NOTES = "clinical.sp_DetachImplantableDevicesFromNotes";
        private const string PROC_ATTACH_IMPLANTABLE_DEVICES_WITH_NOTES = "Clinical.sp_AttachImplantableDevicesWithNotes";
        private const string PROC_IMPLANTABLE_DEVICES_CCDA = "Clinical.sp_ImplantableDeviceSelect_CCDA";
        private const string PROC_IMPLANTABLE_DEVICES_PROCEDURES_INSERT = "Clinical.sp_ImplantableDeviceProceduresInsert";
        private const string PROC_IMPLANTABLE_DEVICES_PROCEDURE_DELETE = "Clinical.sp_ImplantableDeviceProcedureDelete";
        private const string PROC_ID_ASSOCIATED_PROCEDURES_INSERT = "Clinical.sp_ImplantableDeviceAssociatedProceduresInsert";
        #endregion

        #region "Parameters"
        private const string PARM_IMPLANTABLE_DEVICE_PK_ID = "@ImplantableDevicesPKId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_COMPANY_NAME = "@CompanyName";
        private const string PARM_VERSION_MODEL_NUMBER = "@VersionModelNumber";
        private const string PARM_MRI_SAFETY_STATUS = "@MRISafetyStatus";
        private const string PARM_UDI = "@UDI";
        private const string PARM_ISSUING_AGENCY = "@Issuing_agency";
        private const string PARM_DEVICE_ISSUING_AGENCY_ID = "@DeviceIdIssuingAgencyID";
        private const string PARM_LABELED_CONTAINS_NRL = "@LabeledContainsNRL";
        private const string PARM_DEVICE_NAME = "@GMDNPName";
        private const string PARM_DEVICE_ID = "@DI";
        private const string PARM_MANUFACTURER = "@BrandName";
        private const string PARM_SERIAL_NUM = "@Serial_Number";
        private const string PARM_LOT_NUM = "@Lot_Number";
        private const string PARM_MANUFACTURING_DATE = "@Manufacturing_Date";
        private const string PARM_EXPIRY_DATE = "@Expiration_Date";
        private const string PARM_ISSUING_AGENCY_DESCRIPTION = "@IssuingAgencyDescription";
        private const string PARM_GMDNPT_DEFINITION = "@GMDNPTDefinition";
        private const string PARM_IMPLANT_DATE = "@ImplantDate";
        private const string PARM_DEVICE_DESCRIPTION = "@DeviceDescription";
        private const string PARM_NOTE_PROVIDER_ID = "@ProviderId";

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_NOTE_ID = "@NotesId";
        private const string PARM_INFORMANT_ID = "@PersonalRelationshipID";
        private const string PARM_STATUS = "@Status";
        private const string PARM_AREA = "@Area";
        private const string PARM_LATERALITY_ID = "@LateralityId";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_TARGET_SITE_ID = "@TargetSiteId";
        private const string PARM_TARGET_SITE = "@TargetSite";
        private const string PARM_IMPLANTABLE_DEVICE_PROCEDURES_XML = "@ImplantableProceduresXML";
        private const string PARM_IMPLANTABLE_DEVICE_PROCEDURE_ID = "@ImplantableProcedureId";

        #endregion

        #region "Constructors"
        public DALImplantable()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region "Insert, Delete, Update and Load"

        public List<ImplantableDevicesWithNotes> loadImplantableDevice(long ImplantableDevicesPKId, long PatientId, long NotesId, int IsActive = 1, long PageNumber = 1, long RowsPerPage = 1000)
        {
            List<ImplantableDevices> listImplantableDevices = new List<ImplantableDevices>();
            ImplantableDevicesWithNotes devicesWithNotesList = new ImplantableDevicesWithNotes();
            List<ImplantableDevicesWithNotes> listNoteDevices = new List<ImplantableDevicesWithNotes>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);

                if (ImplantableDevicesPKId == 0)
                    dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, null);
                else
                    dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, ImplantableDevicesPKId);

                dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);

                if (PageNumber <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, RowsPerPage);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                if (PatientId == 0)
                    dbManager.AddParameters(5, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(5, PARM_PATIENT_ID, PatientId);

                if (NotesId == 0)
                    dbManager.AddParameters(6, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(6, PARM_NOTE_ID, NotesId);
                
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_IMPLANTABLE_DEVICES_SELECT);

                ImplantableDevices model = null;
                while (reader.Read())
                {
                    model = new ImplantableDevices();
                    model.ImplantableDevicesPKId = !String.IsNullOrEmpty(reader["ImplantableDevicesPKId"].ToString()) ? reader["ImplantableDevicesPKId"].ToString() : "";
                    model.GMDNPName = !String.IsNullOrEmpty(reader["GMDNPName"].ToString()) ? reader["GMDNPName"].ToString() : "";
                    model.DI = !String.IsNullOrEmpty(reader["DI"].ToString()) ? reader["DI"].ToString() : "";
                    model.Issuing_agency = !String.IsNullOrEmpty(reader["Issuing_agency"].ToString()) ? reader["Issuing_agency"].ToString() : "";
                    model.VersionModelNumber = !String.IsNullOrEmpty(reader["VersionModelNumber"].ToString()) ? reader["VersionModelNumber"].ToString() : "";
                    model.CompanyName = !String.IsNullOrEmpty(reader["CompanyName"].ToString()) ? reader["CompanyName"].ToString() : "";
                    model.UDI = !String.IsNullOrEmpty(reader["UDI"].ToString()) ? reader["UDI"].ToString() : "";
                    model.BrandName = !String.IsNullOrEmpty(reader["BrandName"].ToString()) ? reader["BrandName"].ToString() : "";
                    model.Serial_Number = !String.IsNullOrEmpty(reader["Serial_Number"].ToString()) ? reader["Serial_Number"].ToString() : "";
                    model.Lot_Number = !String.IsNullOrEmpty(reader["Lot_Number"].ToString()) ? reader["Lot_Number"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.Manufacturing_Date = !String.IsNullOrEmpty(reader["Manufacturing_Date"].ToString()) ? reader["Manufacturing_Date"].ToString() : "";
                    model.Expiration_Date = !String.IsNullOrEmpty(reader["Expiration_Date"].ToString()) ? reader["Expiration_Date"].ToString() : "";
                    model.TargetSite = !String.IsNullOrEmpty(reader["TargetSite"].ToString()) ? reader["TargetSite"].ToString() : "";
                    model.Status = !String.IsNullOrEmpty(reader["Status"].ToString()) ? reader["Status"].ToString() : "";

                    devicesWithNotesList._implantableDevices.Add(model);
                }
                reader.NextResult();
                DevicesAttachedWithNotes notesModel = null;
                while (reader.Read())
                {
                    notesModel = new DevicesAttachedWithNotes();
                    notesModel.ImplantableDevicesPKId = !String.IsNullOrEmpty(reader["ImplantableDevicesPKId"].ToString()) ? reader["ImplantableDevicesPKId"].ToString() : "";
                    notesModel.NotesId = !String.IsNullOrEmpty(reader["NotesId"].ToString()) ? reader["NotesId"].ToString() : "";
                    devicesWithNotesList._attachedNotes.Add(notesModel);
                }
                listNoteDevices.Add(devicesWithNotesList);
                return listNoteDevices;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::loadImplantableDevices", PROC_IMPLANTABLE_DEVICES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        private void CreateParametersForInsertUpdate(IDBManager dbManager, ImplantableDevices model, Boolean IsInsert)
        {
            if (IsInsert == true)
            {
                dbManager.CreateParameters(28);
                dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, model.ImplantableDevicesPKId, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(26, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(27, PARM_CREATED_ON, DateTime.Now);
            }
            else
            {
                dbManager.CreateParameters(26);
                dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, model.ImplantableDevicesPKId);
            }
            dbManager.AddParameters(1, PARM_COMPANY_NAME, model.CompanyName != "" ? model.CompanyName : null);
            dbManager.AddParameters(2, PARM_DEVICE_ID, model.DI != "" ? model.DI : null);
            dbManager.AddParameters(3, PARM_EXPIRY_DATE, model.Expiration_Date != "" ? model.Expiration_Date : null);
            dbManager.AddParameters(4, PARM_ISSUING_AGENCY, model.Issuing_agency != "" ? model.Issuing_agency : null);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, model.IsActive != "" ? model.IsActive : null);
            dbManager.AddParameters(6, PARM_LABELED_CONTAINS_NRL, model.LabeledContainsNRL != "" ? model.LabeledContainsNRL : null);
            dbManager.AddParameters(7, PARM_LOT_NUM, model.Lot_Number != "" ? model.Lot_Number : null);
            dbManager.AddParameters(8, PARM_MANUFACTURER, model.BrandName != "" ? model.BrandName : null);
            dbManager.AddParameters(9, PARM_MANUFACTURING_DATE, model.Manufacturing_Date != "" ? model.Manufacturing_Date : null);
            dbManager.AddParameters(10, PARM_MRI_SAFETY_STATUS, model.MRISafetyStatus != "" ? model.MRISafetyStatus : null);
            dbManager.AddParameters(11, PARM_PATIENT_ID, model.PatientId != "" ? model.PatientId : null);
            dbManager.AddParameters(12, PARM_SERIAL_NUM, model.Serial_Number != "" ? model.Serial_Number : null);
            dbManager.AddParameters(13, PARM_UDI, model.UDI != "" ? model.UDI : null);
            dbManager.AddParameters(14, PARM_VERSION_MODEL_NUMBER, model.VersionModelNumber != "" ? model.VersionModelNumber : null);
            dbManager.AddParameters(15, PARM_DEVICE_NAME, model.GMDNPName != "" ? model.GMDNPName : null);
            dbManager.AddParameters(16, PARM_IMPLANT_DATE, model.ImplantDate != "" ? model.ImplantDate : null);
            dbManager.AddParameters(17, PARM_DEVICE_DESCRIPTION, model.DeviceDescription != "" ? model.DeviceDescription : null);
            dbManager.AddParameters(18, PARM_INFORMANT_ID, model.InformantId != "" ? model.InformantId : null);
            dbManager.AddParameters(19, PARM_STATUS, model.Status != "" ? model.Status : null);
            dbManager.AddParameters(20, PARM_AREA, model.Area != "" ? model.Area : null);
            dbManager.AddParameters(21, PARM_LATERALITY_ID, model.LateralityId != "" ? model.LateralityId : null);
            dbManager.AddParameters(22, PARM_COMMENTS, model.Comments != "" ? model.Comments : null);
            dbManager.AddParameters(23, PARM_TARGET_SITE_ID, model.TargetSiteId != "" ? model.TargetSiteId : null);
            dbManager.AddParameters(24, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(25, PARM_MODIFIED_ON, DateTime.Now);
        }

        public string insertImplantableDevices(ImplantableDevices model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            { 
                string returnVal = "";
                //dbManager.Open();
                dbManager.BeginTransaction();

                CreateParametersForInsertUpdate(dbManager, model, true);
                
                returnVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMPLANTABLE_DEVICES_INSERT));

                if (returnVal == "UDI already exists")
                    throw new Exception(returnVal);
                else
                {
                    if (model.ImplantableDeviceProcedure.Count > 0)
                    {
                        try
                        {
                            dbManager.CreateParameters(2);
                            dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, returnVal);
                            dbManager.AddParameters(1, PARM_IMPLANTABLE_DEVICE_PROCEDURES_XML, model.ImplantableDeviceProceduresXML);
                            dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMPLANTABLE_DEVICES_PROCEDURES_INSERT);

                        }
                        catch (Exception ex)
                        {
                            MDVLogger.DALErrorLog("DALImplantable::ImplantableDevicesProceduresInsert", PROC_IMPLANTABLE_DEVICES_PROCEDURES_INSERT, ex);
                            throw ex;
                        }
                    }

                    dbManager.CommitTransaction();
                    return returnVal;
                }
                    
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALImplantable::insertImplantableDevices", PROC_IMPLANTABLE_DEVICES_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        
        public string updateImplantableDevices(ImplantableDevices model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string returnVal = "";
                //dbManager.Open();
                dbManager.BeginTransaction();
                CreateParametersForInsertUpdate(dbManager, model, false);

                returnVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMPLANTABLE_DEVICES_UPDATE));

                if (returnVal == "UDI already exists")
                    throw new Exception(returnVal);
                else
                {
                    if (model.ImplantableDeviceProcedure.Count > 0)
                    {
                        try
                        {
                            dbManager.CreateParameters(2);
                            dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, model.ImplantableDevicesPKId);
                            dbManager.AddParameters(1, PARM_IMPLANTABLE_DEVICE_PROCEDURES_XML, model.ImplantableDeviceProceduresXML);
                            dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMPLANTABLE_DEVICES_PROCEDURES_INSERT);
                        }
                        catch (Exception ex)
                        {
                            MDVLogger.DALErrorLog("DALImplantable::ImplantableDevicesProceduresUpdate", PROC_IMPLANTABLE_DEVICES_PROCEDURES_INSERT, ex);
                            throw ex;
                        }
                    }

                    dbManager.CommitTransaction();
                    return "";
                }
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALImplantable::updateImplantableDevices", PROC_IMPLANTABLE_DEVICES_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteImplantableDevices(string ImplantableDeviceId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, ImplantableDeviceId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMPLANTABLE_DEVICES_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::deleteImplantableDevices", PROC_IMPLANTABLE_DEVICES_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string deleteImplantableDeviceProcedure(string ImplantableDeviceProcedureId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PROCEDURE_ID, ImplantableDeviceProcedureId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMPLANTABLE_DEVICES_PROCEDURE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::deleteImplantableDeviceProcedure", PROC_IMPLANTABLE_DEVICES_PROCEDURE_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string activeInactiveImplantableDevices(Int64 ImplantableDeviceId, Int64 IsActive)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, ImplantableDeviceId);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                dbManager.AddParameters(3, PARM_MODIFIED_ON, DateTime.Now);

                dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_IMPLANTABLE_DEVICES_ACTIVE_INACTIVE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::activeInactiveImplantableDevices", PROC_IMPLANTABLE_DEVICES_ACTIVE_INACTIVE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<ImplantableDevicesWithProcedures> fillImplantableDevices(long ImplantableDevicesPKId, long PatientId, long PageNumber = 1, long RowsPerPage = 1000)
        {
            List<ImplantableDevices> listImplantableDevices = new List<ImplantableDevices>();
            ImplantableDevicesWithProcedures deviceWithProceduresList = new ImplantableDevicesWithProcedures();
            List<ImplantableDevicesWithProcedures> listProceduresDevice = new List<ImplantableDevicesWithProcedures>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (ImplantableDevicesPKId == 0)
                    dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, null);
                else
                    dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, ImplantableDevicesPKId);

                if (PatientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, PatientId);

                if (PageNumber <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(3, PARM_ROWSP_PAGE, RowsPerPage);

                dbManager.AddParameters(4, PARM_RECORD_COUNT, "RecordCount", DbType.Int64, ParamDirection.Output);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_IMPLANTABLE_DEVICES_FILL);

                ImplantableDevices model = null;
                while (reader.Read())
                {
                    model = new ImplantableDevices();

                    model.ImplantableDevicesPKId = !String.IsNullOrEmpty(reader["ImplantableDevicesPKId"].ToString()) ? reader["ImplantableDevicesPKId"].ToString() : "";
                    model.GMDNPName = !String.IsNullOrEmpty(reader["GMDNPName"].ToString()) ? reader["GMDNPName"].ToString() : "";
                    model.DI = !String.IsNullOrEmpty(reader["DI"].ToString()) ? reader["DI"].ToString() : "";
                    model.Issuing_agency = !String.IsNullOrEmpty(reader["Issuing_agency"].ToString()) ? reader["Issuing_agency"].ToString() : "";
                    model.VersionModelNumber = !String.IsNullOrEmpty(reader["VersionModelNumber"].ToString()) ? reader["VersionModelNumber"].ToString() : "";
                    model.CompanyName = !String.IsNullOrEmpty(reader["CompanyName"].ToString()) ? reader["CompanyName"].ToString() : "";
                    model.UDI = !String.IsNullOrEmpty(reader["UDI"].ToString()) ? reader["UDI"].ToString() : "";
                    model.BrandName = !String.IsNullOrEmpty(reader["BrandName"].ToString()) ? reader["BrandName"].ToString() : "";
                    model.Serial_Number = !String.IsNullOrEmpty(reader["Serial_Number"].ToString()) ? reader["Serial_Number"].ToString() : "";
                    model.Lot_Number = !String.IsNullOrEmpty(reader["Lot_Number"].ToString()) ? reader["Lot_Number"].ToString() : "";
                    model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.Manufacturing_Date = !String.IsNullOrEmpty(reader["Manufacturing_Date"].ToString()) ? reader["Manufacturing_Date"].ToString() : "";
                    model.Expiration_Date = !String.IsNullOrEmpty(reader["Expiration_Date"].ToString()) ? reader["Expiration_Date"].ToString() : "";
                    model.ImplantDate = !String.IsNullOrEmpty(reader["ImplantDate"].ToString()) ? reader["ImplantDate"].ToString() : "";
                    model.MRISafetyStatus = !String.IsNullOrEmpty(reader["MRISafetyStatus"].ToString()) ? reader["MRISafetyStatus"].ToString() : "";
                    model.LabeledContainsNRL = !String.IsNullOrEmpty(reader["LabeledContainsNRL"].ToString()) ? reader["LabeledContainsNRL"].ToString() : "";
                    model.DeviceDescription = !String.IsNullOrEmpty(reader["DeviceDescription"].ToString()) ? reader["DeviceDescription"].ToString() : "";
                    model.InformantId = !String.IsNullOrEmpty(reader["InformantId"].ToString()) ? reader["InformantId"].ToString() : "";
                    model.Status = !String.IsNullOrEmpty(reader["Status"].ToString()) ? reader["Status"].ToString() : "";
                    model.Area = !String.IsNullOrEmpty(reader["Area"].ToString()) ? reader["Area"].ToString() : "";
                    model.LateralityId = !String.IsNullOrEmpty(reader["LateralityId"].ToString()) ? reader["LateralityId"].ToString() : "";
                    model.TargetSite = !String.IsNullOrEmpty(reader["TargetSite"].ToString()) ? reader["TargetSite"].ToString() : "";
                    model.Comments = !String.IsNullOrEmpty(reader["Comments"].ToString()) ? reader["Comments"].ToString() : "";
                    model.TargetSiteId = !String.IsNullOrEmpty(reader["TargetSiteId"].ToString()) ? reader["TargetSiteId"].ToString() : "";

                    deviceWithProceduresList.implantableDevices.Add(model);
                }
                reader.NextResult();
                ImplantableDeviceProcedures implantableProcedures = null;
                while (reader.Read())
                {
                    implantableProcedures = new ImplantableDeviceProcedures();

                    implantableProcedures.ImplantableDeviceProcedureId = !String.IsNullOrEmpty(reader["ImplantableDeviceProcedureId"].ToString()) ? reader["ImplantableDeviceProcedureId"].ToString() : "";
                    implantableProcedures.Procedure = !String.IsNullOrEmpty(reader["Procedures"].ToString()) ? reader["Procedures"].ToString() : "";
                    implantableProcedures.CPTCode = !String.IsNullOrEmpty(reader["CPTCode"].ToString()) ? reader["CPTCode"].ToString() : "";
                    implantableProcedures.CPTCodeDescription = !String.IsNullOrEmpty(reader["CPTCodeDescription"].ToString()) ? reader["CPTCodeDescription"].ToString() : "";
                    implantableProcedures.SnomedID = !String.IsNullOrEmpty(reader["SnomedID"].ToString()) ? reader["SnomedID"].ToString() : "";
                    implantableProcedures.SnomedDescription = !String.IsNullOrEmpty(reader["SnomedDescription"].ToString()) ? reader["SnomedDescription"].ToString() : "";

                    deviceWithProceduresList.associatedProcedures.Add(implantableProcedures);
                }
                listProceduresDevice.Add(deviceWithProceduresList);

                return listProceduresDevice;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::fillImplantableDevices", PROC_IMPLANTABLE_DEVICES_FILL, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Implantable Device attached/detached with Notes"
        //public List<ImplantableDevices> getLatestImplantableDevicesByPatientId(long PatientId)
        //{
        //    List<ImplantableDevices> listImplantableDevices = new List<ImplantableDevices>();
        //    IDBManager dbManager = ClientConfiguration.GetDBManager();
        //    SqlDataReader reader = null;
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(3);

        //        if (PatientId == 0)
        //            dbManager.AddParameters(0, PARM_PATIENT_ID, null);
        //        else
        //            dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

        //        reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GET_LATEST_DEVICES_BY_PATIENT);

        //        ImplantableDevices model = null;
        //        while (reader.Read())
        //        {
        //            model = new ImplantableDevices();
        //            model.ImplantableDevicesPKId = !String.IsNullOrEmpty(reader["ImplantableDevicesPKId"].ToString()) ? reader["ImplantableDevicesPKId"].ToString() : "";
        //            model.GMDNPName = !String.IsNullOrEmpty(reader["GMDNPName"].ToString()) ? reader["GMDNPName"].ToString() : "";
        //            model.DevicePublishDate = !String.IsNullOrEmpty(reader["DevicePublishDate"].ToString()) ? reader["DevicePublishDate"].ToString() : "";
        //            model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";
        //            model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";

        //            listImplantableDevices.Add(model);
        //        }
        //        return listImplantableDevices;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.DALErrorLog("DALImplantable::getLatestImplantableDevicesByPatientId", PROC_GET_LATEST_DEVICES_BY_PATIENT, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        public List<ImplantableDevices> getDevicesForSoap(string ImplantableDevicesPKId, long PatientId, long NoteProviderId)
        {
            List<ImplantableDevices> listImplantableDevices = new List<ImplantableDevices>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (string.IsNullOrEmpty(ImplantableDevicesPKId))
                    dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, null);
                else
                    dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, ImplantableDevicesPKId);

                if(NoteProviderId == 0)
                    dbManager.AddParameters(1, PARM_NOTE_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTE_PROVIDER_ID, NoteProviderId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GET_DEVICES_FOR_SOAP);

                ImplantableDevices model = null;
                while (reader.Read())
                {
                    model = new ImplantableDevices();
                    model.ImplantableDevicesPKId = !String.IsNullOrEmpty(reader["ImplantableDevicesPKId"].ToString()) ? reader["ImplantableDevicesPKId"].ToString() : "";
                    model.GMDNPName = !String.IsNullOrEmpty(reader["GMDNPName"].ToString()) ? reader["GMDNPName"].ToString() : "";
                    model.ImplantDate = !String.IsNullOrEmpty(reader["ImplantDate"].ToString()) ? reader["ImplantDate"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.TargetSite = !String.IsNullOrEmpty(reader["TargetSite"].ToString()) ? reader["TargetSite"].ToString() : "";
                    model.Procedures = !String.IsNullOrEmpty(reader["Procedures"].ToString()) ? reader["Procedures"].ToString() : "";

                    listImplantableDevices.Add(model);
                }
                return listImplantableDevices;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::getDevicesForSoap", PROC_GET_DEVICES_FOR_SOAP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string detachDevicesFromNotes(string ImplantableDevicesPKId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, ImplantableDevicesPKId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_DEVICES_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::detachDevicesFromNotes", PROC_DETACH_DEVICES_FROM_NOTES, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string attachDevicesWithNotes(string ImplantableDevicesPKId, string NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, ImplantableDevicesPKId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                object ImplantableDevicesId = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ATTACH_IMPLANTABLE_DEVICES_WITH_NOTES);
                return Convert.ToString(ImplantableDevicesId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::attachDevicesWithNotes", PROC_ATTACH_IMPLANTABLE_DEVICES_WITH_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public string insertAssociatedProcedures(ImplantableDevices model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string ProcedureIds = "";
                dbManager.Open();
                dbManager.CreateParameters(8);

                dbManager.AddParameters(0, PARM_IMPLANTABLE_DEVICE_PK_ID, model.ImplantableDevicesPKId);
                dbManager.AddParameters(1, PARM_PATIENT_ID, model.PatientId != "" ? model.PatientId : null);
                dbManager.AddParameters(2, PARM_NOTE_ID, model.NotesId);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, true);
                dbManager.AddParameters(4, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(5, PARM_CREATED_ON, DateTime.Now);
                dbManager.AddParameters(6, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                dbManager.AddParameters(7, PARM_MODIFIED_ON, DateTime.Now);

                ProcedureIds = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ID_ASSOCIATED_PROCEDURES_INSERT));

                return ProcedureIds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::insertAssociatedProcedures", PROC_ID_ASSOCIATED_PROCEDURES_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Lookups"
        public List<InformantLookup> LookupInformant()
        {
            List<InformantLookup> listInformants = new List<InformantLookup>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_INFORMANT_LOOKUP);
                InformantLookup model = null;
                while (reader.Read())
                {
                    model = new InformantLookup();
                    model.Id = !String.IsNullOrEmpty(reader["Id"].ToString()) ? reader["Id"].ToString() : "";
                    model.Code = !String.IsNullOrEmpty(reader["Code"].ToString()) ? reader["Code"].ToString() : "";
                    model.Description = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";

                    listInformants.Add(model);
                }

                return listInformants;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::LookupInformant", PROC_INFORMANT_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<LateralityLookup> LookupLaterality()
        {
            List<LateralityLookup> listLaterality = new List<LateralityLookup>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_LATERALITY_LOOKUP);
                LateralityLookup model = null;
                while (reader.Read())
                {
                    model = new LateralityLookup();
                    model.Id = !String.IsNullOrEmpty(reader["Id"].ToString()) ? reader["Id"].ToString() : "";
                    model.Code = !String.IsNullOrEmpty(reader["Code"].ToString()) ? reader["Code"].ToString() : "";
                    model.Description = !String.IsNullOrEmpty(reader["Description"].ToString()) ? reader["Description"].ToString() : "";

                    listLaterality.Add(model);
                }

                return listLaterality;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::LookupLaterality", PROC_LATERALITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<TargetSiteLookup> LookupTargetSite(string TargetSite)
        {
            List<TargetSiteLookup> listTargetSites = new List<TargetSiteLookup>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_TARGET_SITE, TargetSite != "" ? TargetSite : null);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_TARGET_SITE_LOOKUP);
                TargetSiteLookup model = null;
                while (reader.Read())
                {
                    model = new TargetSiteLookup();
                    model.Id = !String.IsNullOrEmpty(reader["Id"].ToString()) ? reader["Id"].ToString() : "";
                    model.Code = !String.IsNullOrEmpty(reader["Code"].ToString()) ? reader["Code"].ToString() : "";
                    model.Name = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";

                    listTargetSites.Add(model);
                }

                return listTargetSites;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::LookupTargetSite", PROC_TARGET_SITE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        public List<ImplantableDevices> loadImplantableDeviceForCCDA(long PatientId, long NotesId)
        {
            List<ImplantableDevices> listImplantableDevices = new List<ImplantableDevices>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);

                if (NotesId == 0)
                    dbManager.AddParameters(1, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_IMPLANTABLE_DEVICES_CCDA);

                ImplantableDevices model = null;
                while (reader.Read())
                {
                    model = new ImplantableDevices();
                    model.ImplantableDevicesPKId = !String.IsNullOrEmpty(reader["ImplantableDevicesPKId"].ToString()) ? reader["ImplantableDevicesPKId"].ToString() : "";
                    model.GMDNPName = !String.IsNullOrEmpty(reader["GMDNPName"].ToString()) ? reader["GMDNPName"].ToString() : "";
                    model.DI = !String.IsNullOrEmpty(reader["DI"].ToString()) ? reader["DI"].ToString() : "";
                    model.Issuing_agency = !String.IsNullOrEmpty(reader["Issuing_agency"].ToString()) ? reader["Issuing_agency"].ToString() : "";
                    model.VersionModelNumber = !String.IsNullOrEmpty(reader["VersionModelNumber"].ToString()) ? reader["VersionModelNumber"].ToString() : "";
                    model.CompanyName = !String.IsNullOrEmpty(reader["CompanyName"].ToString()) ? reader["CompanyName"].ToString() : "";
                    model.UDI = !String.IsNullOrEmpty(reader["UDI"].ToString()) ? reader["UDI"].ToString() : "";
                    model.BrandName = !String.IsNullOrEmpty(reader["BrandName"].ToString()) ? reader["BrandName"].ToString() : "";
                    model.Serial_Number = !String.IsNullOrEmpty(reader["Serial_Number"].ToString()) ? reader["Serial_Number"].ToString() : "";
                    model.Lot_Number = !String.IsNullOrEmpty(reader["Lot_Number"].ToString()) ? reader["Lot_Number"].ToString() : "";
                    model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    model.Manufacturing_Date = !String.IsNullOrEmpty(reader["Manufacturing_Date"].ToString()) ? reader["Manufacturing_Date"].ToString() : "";
                    model.Expiration_Date = !String.IsNullOrEmpty(reader["Expiration_Date"].ToString()) ? reader["Expiration_Date"].ToString() : "";
                    //model.GMDNPTDefinition = !String.IsNullOrEmpty(reader["GMDNPTDefinition"].ToString()) ? reader["GMDNPTDefinition"].ToString() : "";

                    listImplantableDevices.Add(model);
                }

                return listImplantableDevices;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALImplantable::loadImplantableDeviceForCCDA", PROC_IMPLANTABLE_DEVICES_CCDA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

       
    }
}
