using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Model.Clinical.Orthopedic;
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
    public class DALLOrthopedic
    {

        #region " Constructors "

        public DALLOrthopedic()
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
        
        private const string PROC_ORTHOPEDIC_COMPLAINTS_SELECT = "Clinical.sp_OrthopedicComplaintsSelect";
        private const string PROC_ORTHOPEDIC_COMPLAINTS_INSERT = "Clinical.sp_OrthopedicComplaintsInsert";
        private const string PROC_ORTHOPEDIC_COMPLAINTS_DELETE = "Clinical.sp_OrthopedicComplaintsDelete";
        private const string PROC_NOTES_BODY_PART_INSERT = "Clinical.sp_NotesBodyPartsInsert";
        private const string PROC_NOTES_BODY_PART_AND_COMPLAINTS_DELETE = "Clinical.sp_NotesBodyPartsAndComplaintsDelete";
        private const string PROC_NOTES_BODY_PART_COMPLAINS_SELECT = "Clinical.sp_NoteBodyPartComplaintsSelect";

        #endregion

        #region " Parameters "

        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ORTHOPEDIC_COMPLAIN_ID = "OrthopedicComplainId";
        private const string PARM_COMPLAINT = "@Complaint";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_BODY_PART= "@BodyPart";
        private const string PARM_NOTES_ID = "@NotesId";
        private const string PARM_IS_DELETE = "@IsDeleteBodyPartAssociation";
        private const string PARM_NOTES_BODY_PART_ID = "@NotesBodyPartId";
        private const string PARM_COMPLAINT_DETAIL_ID = "@ComplaintDetailId";

        #endregion

        private void createParameters(IDBManager dbManager, OrthopedicComplaintsModel model, Boolean IsInsert)
        {
            dbManager.CreateParameters(8);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_ORTHOPEDIC_COMPLAIN_ID, model.OrthopedicComplainId);
            else
                dbManager.AddParameters(0, PARM_ORTHOPEDIC_COMPLAIN_ID, model.OrthopedicComplainId);

            dbManager.AddParameters(1, PARM_COMPLAINT, model.Complaint);
            dbManager.AddParameters(2, PARM_USER_ID, model.UserId);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, model.IsActive);
            dbManager.AddParameters(4, PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(5, PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(6, PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(7, PARM_MODIFIED_ON, DateTime.Now);
        }

        public List<OrthopedicComplaintsModel> LoadOrthopedicComplaints()
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<OrthopedicComplaintsModel> objList = new List<OrthopedicComplaintsModel>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_USER_ID, MDVUtility.ToInt64(MDVSession.Current.AppUserId)));
                using (var reader = dbManager.ExecuteReader(PROC_ORTHOPEDIC_COMPLAINTS_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        objList.Add(new OrthopedicComplaintsModel
                        {
                            OrthopedicComplainId = MDVUtility.ToInt64(reader["OrthopedicComplainId"]),
                            UserId = MDVUtility.ToInt64(reader["UserId"]),
                            Complaint = MDVUtility.ToStr(reader["Complaint"]),
                            CreatedBy = MDVUtility.ToStr(reader["CreatedBy"]),
                            ModifiedBy = MDVUtility.ToStr(reader["ModifiedBy"]),
                            IsActive = MDVUtility.StringToBoolean(MDVUtility.ToStr(reader["IsActive"])),
                            CreatedOn = MDVUtility.ToDateTime(reader["CreatedOn"]),
                            ModifiedOn = MDVUtility.ToDateTime(reader["ModifiedOn"]),
                        });
                    }
                }

                return objList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLOrthopedic::LoadOrthopedicComplaints", PROC_ORTHOPEDIC_COMPLAINTS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
           
        }

        public string SaveOrthopedicComplaints(OrthopedicComplaintsModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                this.createParameters(dbManager, model, true);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORTHOPEDIC_COMPLAINTS_INSERT));
                if (retunVal == "Complaint Already Exists")
                    throw new Exception(retunVal);
                else
                    return retunVal;
                
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLOrthopedic::SaveOrthopedicComplaints", PROC_ORTHOPEDIC_COMPLAINTS_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
            
        }

        public string DeleteOrthopedicComplaints(string OrthopedicComplainId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_ORTHOPEDIC_COMPLAIN_ID, OrthopedicComplainId);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_ORTHOPEDIC_COMPLAINTS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLOrthopedic::DeleteOrthopedicComplaints", PROC_ORTHOPEDIC_COMPLAINTS_DELETE, ex);
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

        public string SaveNotesBodyPart(string BodyPart,long NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_NOTES_BODY_PART_ID, 0);
                dbManager.AddParameters(1, PARM_BODY_PART, BodyPart);
                dbManager.AddParameters(2, PARM_NOTES_ID, NotesId);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_NOTES_BODY_PART_INSERT));
                if (!string.IsNullOrEmpty(retunVal) && retunVal.Contains("Body Part"))
                    throw new Exception(retunVal);
                else
                    return retunVal;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLOrthopedic::SaveNotesBodyPart", PROC_NOTES_BODY_PART_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        public string DeleteNotesBodyPart(long ComplaintDetailId, long NotesBodyPartId,long NotesId,bool IsDeleteBodyPartAssociation)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                string retunVal = "";
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_NOTES_BODY_PART_ID, NotesBodyPartId);
                dbManager.AddParameters(1, PARM_COMPLAINT_DETAIL_ID, ComplaintDetailId);
                dbManager.AddParameters(2, PARM_NOTES_ID, NotesId);
                dbManager.AddParameters(3, PARM_IS_DELETE, IsDeleteBodyPartAssociation);
                retunVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_NOTES_BODY_PART_AND_COMPLAINTS_DELETE));
                if (!string.IsNullOrEmpty(retunVal))
                    throw new Exception(retunVal);
                else
                    return retunVal;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLOrthopedic::DeleteNotesBodyPart", PROC_NOTES_BODY_PART_AND_COMPLAINTS_DELETE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }


        public List<OrthopedicComplaintsModel> LoadNotesBodyPartsComplaints(long NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<OrthopedicComplaintsModel> objList = new List<OrthopedicComplaintsModel>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_NOTES_ID, NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_BODY_PART_COMPLAINS_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        objList.Add(new OrthopedicComplaintsModel
                        {
                            ComplaintDetailId = MDVUtility.ToInt64(reader["ComplaintDetailId"]),
                            Complaint = MDVUtility.ToStr(reader["Complaint"]),
                            NotesBodyPartId = MDVUtility.ToLong(reader["NotesBodyPartId"]),
                            BodyPart = MDVUtility.ToStr(reader["BodyPart"]),
                            NotesId = MDVUtility.ToLong(reader["NotesId"]),
                        });
                    }
                }

                return objList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALLOrthopedic::LoadNotesBodyPartsComplaints", PROC_NOTES_BODY_PART_COMPLAINS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }

        }
    }
}
