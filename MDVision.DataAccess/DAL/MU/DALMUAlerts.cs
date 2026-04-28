using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Model.MU;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.DataAccess.DAL.MU
{
    public class DALMUAlerts
    {
        #region " Constructors "
        /// <summary>
        /// Initializes a new instance of the <see cref="DALMUAlerts"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALMUAlerts()
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

        #region " Stored Procedure Names "

        private const string PROC_MU3_ALERTS_INSERT = "Clinical.sp_MUAlertsInsert";
        private const string PROC_MU3_ALERTS_UPDATE = "Clinical.sp_MUAlertsUpdate";
        private const string PROC_MU3_ALERTS_LOAD = "Clinical.sp_MUAlertsLoad";

        #endregion

        #region " Parameters "

        private const string PARM_ALERT_ID = "@AlertId";
        private const string PARM_PROFILE_NAMES = "@ProfileNames";
        private const string PARM_FIELDS = "@Fields";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_IS_SHOW_ALERT = "@IsShowAlert";
        private const string PARM_TYPE = "@Type";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_MU_ALERT_XML = "@MUAlertXML";
        private const string PARM_IS_FROM_NOTE = "@IsFromNote";

        #endregion

        #region " Support Functions "

        private void CreateParameters(IDBManager dbManager, string AlertXML,bool IsFromNote)
        {
            dbManager.CreateParameters(4);
            int i = 0;
            dbManager.AddParameters(i++, PARM_MU_ALERT_XML, AlertXML);
            dbManager.AddParameters(i++, PARM_USER_ID, MDVSession.Current.AppUserId);
            dbManager.AddParameters(i++, PARM_ENTITY_ID, MDVSession.Current.EntityId);
            dbManager.AddParameters(i++, PARM_IS_FROM_NOTE, IsFromNote);
            
        }

        #endregion

        #region " Insert, delete, update and get Message using dataset Functions "

        public string InsertMUAlerts(string AlertXML)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string returnValue = "";
                dbManager.Open();
                CreateParameters(dbManager, AlertXML, false);
                object returnVal = (SqlDataReader)dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_MU3_ALERTS_INSERT);
                if (returnVal != null && returnVal.ToString() != "")
                {
                    throw new Exception(returnVal.ToString());
                }
                return MDVUtility.ToStr(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMUAlerts::InsertMUAlerts", PROC_MU3_ALERTS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<MUAlertsModel> UpdateMUAlerts(string AlertXML,bool IsFromNote)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                List<MUAlertsModel> list = new List<MUAlertsModel>();
                dbManager.Open();
                CreateParameters(dbManager, AlertXML, IsFromNote);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MU3_ALERTS_UPDATE);
                while (reader.Read())
                {
                    MUAlertsModel model = new MUAlertsModel();
                    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? MDVUtility.ToLong(reader["PatientId"]) : 0;
                    model.IsShowAlert = MDVUtility.StringToBoolean(reader["IsShowAlert"].ToString());
                    model.MissingDataAlertCount = MDVUtility.ToInt64(reader["MissingDataAlertsCount"]) != 0 ? MDVUtility.ToInt64(reader["MissingDataAlertsCount"]) : 0;
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMUAlerts::UpdateMUAlerts", PROC_MU3_ALERTS_UPDATE, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<MUAlertsModel> LoadMUAlerts(long PatientId, bool IsShowAlert, string Type,string ProfileName)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                List<MUAlertsModel> list = new List<MUAlertsModel>();
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (PatientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientId);


                dbManager.AddParameters(1, PARM_IS_SHOW_ALERT, IsShowAlert);

                if (string.IsNullOrEmpty(Type))
                    dbManager.AddParameters(2, PARM_TYPE, null);
                else
                    dbManager.AddParameters(2, PARM_TYPE, Type);

                if (string.IsNullOrEmpty(ProfileName))
                    dbManager.AddParameters(3, PARM_PROFILE_NAMES, null);
                else
                    dbManager.AddParameters(3, PARM_PROFILE_NAMES, ProfileName);

                dbManager.AddParameters(4, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(5, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MU3_ALERTS_LOAD);
                while (reader.Read())
                {
                    MUAlertsModel model = new MUAlertsModel();
                    model.AlertId = !String.IsNullOrEmpty(reader["AlertId"].ToString()) ? MDVUtility.ToLong(reader["AlertId"]) : 0;
                    model.ProfileName = !String.IsNullOrEmpty(reader["ProfileName"].ToString()) ? reader["ProfileName"].ToString() : "";
                    model.Fields = !String.IsNullOrEmpty(reader["Fields"].ToString()) ? reader["Fields"].ToString() : "";
                    model.Type = !String.IsNullOrEmpty(reader["Type"].ToString()) ? reader["Type"].ToString() : "";
                    model.IsShowAlert = MDVUtility.StringToBoolean(reader["IsShowAlert"].ToString());
                    model.UserId = !String.IsNullOrEmpty(reader["UserId"].ToString()) ? MDVUtility.ToLong(reader["UserId"]) : 0;
                    model.PatientId = !String.IsNullOrEmpty(reader["PatientId"].ToString()) ? MDVUtility.ToLong(reader["PatientId"]) : 0;
                    model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    model.ModifiedOn = MDVUtility.ToDateTime(reader["ModifiedOn"]);
                    model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    model.CreatedOn = MDVUtility.ToDateTime(reader["CreatedOn"]);
                    model.NotesId = !String.IsNullOrEmpty(reader["NotesId"].ToString()) ? MDVUtility.ToLong(reader["NotesId"]) : 0;
                    model.ProviderId = !String.IsNullOrEmpty(reader["ProviderId"].ToString()) ? MDVUtility.ToLong(reader["ProviderId"]) : 0;
                    model.PrescriptionId = !String.IsNullOrEmpty(reader["PrescriptionId"].ToString()) ? MDVUtility.ToLong(reader["PrescriptionId"]) : 0;
                    model.NoteDate = !String.IsNullOrEmpty(reader["NoteDate"].ToString()) ? reader["NoteDate"].ToString() : "";
                    model.NoteTime = !String.IsNullOrEmpty(reader["NoteTime"].ToString()) ? reader["NoteTime"].ToString() : "";
                    model.MissingDataAlertCount = MDVUtility.ToInt64(reader["MissingDataAlertsCount"]) != 0 ? MDVUtility.ToInt64(reader["MissingDataAlertsCount"]) : 0;
                    model.IsHighPriority = MDVUtility.StringToBoolean(reader["IsHighPriority"].ToString());
                    model.MeasureType = !String.IsNullOrEmpty(reader["MeasureType"].ToString()) ? reader["MeasureType"].ToString() : "";
                    model.Process = !String.IsNullOrEmpty(reader["Process"].ToString()) ? reader["Process"].ToString() : "";
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMUAlerts::LoadMUAlerts", PROC_MU3_ALERTS_LOAD, ex);

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        #endregion
    }
}
