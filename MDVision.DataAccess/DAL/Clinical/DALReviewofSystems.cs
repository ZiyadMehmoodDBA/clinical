using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Model.Clinical.ReviewOfSystem;
using System.Data.SqlClient;

namespace MDVision.DataAccess.DAL.Clinical
{
    /// <summary>
    /// Author: Zia Mehmood
    /// Overview: Data Access Layer for new Review of System
    /// </summary>
    public class DALReviewofSystems
    {
        #region " Stored Procedure Names "
        // For Characteristics
        private const string PROC_REVIEWOFSYSTEM_CHARATRISTIC_INSERT = "Clinical.sp_ROSCharacteristicsInsert";
        private const string PROC_REVIEWOFSYSTEM_CHARATRISTIC_UPDATE = "Clinical.sp_ROSCharacteristicsUpdate";
        private const string PROC_REVIEWOFSYSTEM_CHARATRISTIC_SELECT = "Clinical.sp_ROSCharacteristicsSelect";
        private const string PROC_REVIEWOFSYSTEM_CHARATRISTIC_DELETE = "Clinical.sp_ROSCharacteristicsDelete";
        private const string PROC_REVIEWOFSYSTEM_CHARATRISTIC_LOOKUP = "Clinical.sp_ROSCharacteristicsLookup";

        // For Systems
        private const string PROC_REVIEWOFSYSTEM_SYSTEM_INSERT = "Clinical.sp_ROSSystemInsert";
        private const string PROC_REVIEWOFSYSTEM_SYSTEM_DELETE = "Clinical.sp_ROSSystemsDelete";
        private const string PROC_REVIEWOFSYSTEM_SYSTEM_ActiveInActive = "Clinical.sp_ROSSystemsActiveInActive";
        private const string PROC_REVIEWOFSYSTEM_SYSTEM_SELECT = "Clinical.sp_ROSSystemSelect";
        private const string PROC_REVIEWOFSYSTEM_SYSTEM_CHARATRISTICS_SELECT = "Clinical.sp_ROSSystemCharacteristicsSelect";

        // For Templates
        private const string PROC_ROSREVAMPTEMPLATE_SELECT = "Clinical.sp_ROSRevampTemplateSelect";
        private const string PROC_REVIEWOFSYSTEM_SYSTEM_LOOKUP = "Clinical.sp_ROSSystemsLookup";
        private const string PROC_REVIEWOFSYSTEM_TEMPLATE_INSERT = "Clinical.sp_ROSRevampTemplateInsert";
        private const string PROC_REVIEWOFSYSTEM_TEMPLATE_DELETE = "Clinical.sp_ROSRevampTemplateDelete";
        private const string PROC_REVIEWOFSYSTEM_TEMPLATE_UPDATE = "Clinical.sp_ROSSystemInsert";
        private const string PROC_REVIEWOFSYSTEM_TEMPLATE_SELECT = "Clinical.sp_ROSTempSysCharacteristicsSelect";
        private const string PROC_REVIEWOFSYSTEM_TEMPLATE_ACTIVEINACTIVE = "Clinical.sp_ROSRevampTemplateActiveInActive";
        private const string PROC_REVIEWOFSYSTEM_LOOKUPS_SELECT = "Clinical.sp_ROSCharacteristicsDetailLookup";

        //  For Note
        private const string PROC_ROSREVMAPTEMPLATE_NOTE_SELECT = "[Clinical].[sp_ROSNOtesTemplateList]";
        private const string PROC_REVIEWOFSYSTEM_TEMPLATE_SELECT_NOTE = "Clinical.sp_ROSNotesTempSysCharacteristicsSelect";
        private const string PROC_REVIEWOFSYSTEM_TEMPLATE_INSERT_NOTE = "Clinical.sp_ROSRevampNoteTemplateInsert";
        private const string PROC_REVIEWOFSYSTEM_TEMPLATE_DELETE_NOTE = "[Clinical].[ROSRevampNoteTemplateDelete]";
        private const string PROC_REVIEWOFSYSTEM_CHARDETAIL_TOGGLING_NOTE = "clinical.Sp_CharDetailToggling";
        private const string PROC_REVIEWOFSYSTEM_SELECT_DEFAULT_NOTE = "Clinical.sp_GetROSNoterevamptemplate";
        #endregion





        #region " Constructors "
        public DALReviewofSystems()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALReviewofSystems(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
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

        #region Parameters
        private const string PARM_Charatristic_ID = "@ROSCharacteristicsId";
        private const string PARM_Charatristic_Name = "@Name";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_PAGENUMBER = "@PageNumber";
        private const string PARM_ROWSPERPAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_IS_GLOBAL = "@IsGlobal";
        private const string PARM_CharatristicIDs = "@ROSCharacteristicIds";
        private const string PARM_SYSTEM_ID = "@ROSSystemId";

        // Template
        private const string PARM_PROVIDER_XML = "@ProviderXML ";
        private const string PARM_SYSTEMCHRATRISTIC_XML = "@SystemCharacteristicsXML ";
        private const string PARM_ENTITY_ID = "@EntityId ";
        private const string PARM_TEMPLATE_ID = "@TemplateId ";
        private const string PARM_SPECIALTY_XML = "@SpecialtyXML ";
        private const string PARM_ROSTEMPLATE_ID = "@ROSTemplateId ";
        private const string PARM_ROSEDIT_TEMPLATEID = "@ROSEdittemplateId";
        private const string PARM_ROSREVAMP_TEMPLATE_ID = "@ROSTemplateId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_BODYPART_ID = "@BodyPartId";

        // For Note
        private const string PARM_NOTE_ID = "@noteId";
        private const string PARM_SOAP_TEXT = "@SoapText";
        private const string PARM_Order_No = "@OrderNo";
        private const string PARM_NOTES_ID = "@NotesId";
        private const string PARM_IS_POSITIVE = "@IsPositive";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PREV_NOTES_ID = "@PrevNotesId";
        private const string PARM_IS_RECORD_DELETE = "@IsDel";
        #endregion

        #region "Charatristics"

        private void createROSCharatristicParameters(IDBManager dbManager, ROSCharacteristics model, bool isInsert = true)
        {
            if (isInsert == true)
            {

                dbManager.CreateParameters(7);
                dbManager.AddParameters(0, PARM_Charatristic_ID, null, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_Charatristic_Name, model.Name);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(3, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(4, PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(5, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(6, PARM_MODIFIED_ON, model.ModifiedOn);
            }
            else
            {
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_Charatristic_ID, model.ROSCharacteristicsId);
                dbManager.AddParameters(1, PARM_Charatristic_Name, model.Name);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(3, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(4, PARM_MODIFIED_ON, model.ModifiedOn);
            }

        }

        private void createrLoadROSCharatristicParameters(IDBManager dbManager, ROSCharacteristics model, bool isInsert = true)
        {

            if (model.Name == "")
            {
                model.Name = null;
            }
            if (model.IsActive == "")
            {
                model.IsActive = null;

            }
            if (model.ROSCharacteristicsId == "")
            {
                model.ROSCharacteristicsId = null;
            }


            dbManager.CreateParameters(6);
            dbManager.AddParameters(0, PARM_Charatristic_ID, model.ROSCharacteristicsId);
            dbManager.AddParameters(1, PARM_IS_ACTIVE, model.IsActive);
            dbManager.AddParameters(2, PARM_Charatristic_Name, model.Name);
            dbManager.AddParameters(3, PARM_PAGENUMBER, model.PageNumber);
            dbManager.AddParameters(4, PARM_ROWSPERPAGE, model.RowsPerPage);
            dbManager.AddParameters(5, PARM_RECORD_COUNT, null, DbType.Int64, ParamDirection.Output);


        }

        private void createrLoadROSSystemsParameters(IDBManager dbManager, ROSCharacteristics model, bool isInsert = true)
        {

            if (model.Name == "")
            {
                model.Name = null;
            }
            if (model.IsActive == "")
            {
                model.IsActive = null;

            }
            if (model.ROSSystemId == "")
            {
                model.ROSSystemId = null;
            }


            dbManager.CreateParameters(6);
            dbManager.AddParameters(0, PARM_SYSTEM_ID, model.ROSSystemId);
            dbManager.AddParameters(1, PARM_IS_ACTIVE, model.IsActive);
            dbManager.AddParameters(2, PARM_Charatristic_Name, model.Name);
            dbManager.AddParameters(3, PARM_PAGENUMBER, model.PageNumber);
            dbManager.AddParameters(4, PARM_ROWSPERPAGE, model.RowsPerPage);
            dbManager.AddParameters(5, PARM_RECORD_COUNT, null, DbType.Int64, ParamDirection.Output);


        }

        private void createROSystemParameters(IDBManager dbManager, ROSCharacteristics model, bool isInsert = true)
        {
            if (model.ROSCharatristicsids == "")
                model.ROSCharatristicsids = null;
            if (isInsert == true)
            {
                if (model.TemplateId == "")
                {
                    model.TemplateId = null;
                }
                dbManager.CreateParameters(10);
                dbManager.AddParameters(0, PARM_SYSTEM_ID, null, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_Charatristic_Name, model.Name);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(3, PARM_IS_GLOBAL, model.IsActive);
                dbManager.AddParameters(4, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(5, PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(6, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(7, PARM_MODIFIED_ON, model.ModifiedOn);
                dbManager.AddParameters(8, PARM_CharatristicIDs, model.ROSCharatristicsids);
                dbManager.AddParameters(9, PARM_TEMPLATE_ID, model.TemplateId);
            }
            else
            {
                dbManager.CreateParameters(9);
                dbManager.AddParameters(0, PARM_SYSTEM_ID, model.ROSSystemId);
                dbManager.AddParameters(1, PARM_Charatristic_Name, model.Name);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(3, PARM_IS_GLOBAL, model.IsActive);
                dbManager.AddParameters(4, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(5, PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(6, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(7, PARM_MODIFIED_ON, model.ModifiedOn);
                dbManager.AddParameters(8, PARM_CharatristicIDs, model.ROSCharatristicsids);
            }

        }

        public string InsertROSCharatrisctic(ROSCharacteristics model)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                createROSCharatristicParameters(dbManager, model, true);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_CHARATRISTIC_INSERT);


                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientInsurance::InsertROSCharatrisctic", PROC_REVIEWOFSYSTEM_CHARATRISTIC_INSERT, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<ROSCharacteristics> loadROSCharatrisctic(ROSCharacteristics model)
        {
            List<ROSCharacteristics> listobj = new List<ROSCharacteristics>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                createrLoadROSCharatristicParameters(dbManager, model, true);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_CHARATRISTIC_SELECT);
                while (reader.Read())
                {
                    ROSCharacteristics model1 = new ROSCharacteristics();
                    var properties = typeof(ROSCharacteristics).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    listobj.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::loadROSCharatrisctic", PROC_REVIEWOFSYSTEM_CHARATRISTIC_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;




        }

        public string deleteROSCharatristics(long ROSCharacteristicsId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_Charatristic_ID, ROSCharacteristicsId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_CHARATRISTIC_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::deleteROSCharatristics", PROC_REVIEWOFSYSTEM_CHARATRISTIC_DELETE, ex);
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
        public string updateROSCharatristics(ROSCharacteristics model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createROSCharatristicParameters(dbManager, model, false);
                var val = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_CHARATRISTIC_UPDATE);
                return val.ToString();
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::updateROSCharatristics", PROC_REVIEWOFSYSTEM_CHARATRISTIC_UPDATE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<ROSCharacteristics> lookupROSCharatristics(string IsActive)
        {

            List<ROSCharacteristics> listobj = new List<ROSCharacteristics>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_CHARATRISTIC_LOOKUP);
                while (reader.Read())
                {
                    ROSCharacteristics model1 = new ROSCharacteristics();
                    var properties = typeof(ROSCharacteristics).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    listobj.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::lookupROSCharatristics", PROC_REVIEWOFSYSTEM_CHARATRISTIC_LOOKUP, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;


            //DSPhysicalExamECWLookup ds = new DSPhysicalExamECWLookup();
            //IDBManager dbManager = ClientConfiguration.GetDBManager();
            //try
            //{
            //    dbManager.Open();
            //    dbManager.CreateParameters(1);
            //    dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);
            //    ds = (DSPhysicalExamECWLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_CHARATRISTIC_LOOKUP, ds, ds.PEObservationLookup.TableName);
            //    return ds;
            //}
            //catch (Exception ex)
            //{
            //    MDVLogger.DALErrorLog("DALPhysicalExamECW::lookupPEObservation", PROC_REVIEWOFSYSTEM_CHARATRISTIC_LOOKUP, ex);
            //    throw ex;
            //}
            //finally
            //{
            //    dbManager.Dispose();
            //}
        }

        public ROSCharacteristics InsertROSCharatriscticandUpadatesystem(ROSCharacteristics model)
        {

            if (model.TemplateId == "")
            {
                model.TemplateId = null;
            }


            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);
                dbManager.AddParameters(0, PARM_Charatristic_ID, null, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_Charatristic_Name, model.Name);
                dbManager.AddParameters(2, PARM_IS_ACTIVE, model.IsActive);
                dbManager.AddParameters(3, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(4, PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(5, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(6, PARM_MODIFIED_ON, model.ModifiedOn);
                dbManager.AddParameters(7, PARM_SYSTEM_ID, model.ROSSystemId);
                dbManager.AddParameters(8, PARM_TEMPLATE_ID, model.TemplateId);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_CHARATRISTIC_INSERT);
                model.ROSCharacteristicsId = dbManager.Parameters[0].Value.ToString();



                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::InsertROSCharatriscticandUpadatesystem", PROC_REVIEWOFSYSTEM_CHARATRISTIC_INSERT, ex);
                model.IsError = ex.Message;
                return model;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region " Support Functions ROS System "

        public List<ROSCharacteristics> lookupROSSystems(string IsActive)
        {

            List<ROSCharacteristics> listobj = new List<ROSCharacteristics>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_SYSTEM_LOOKUP);
                while (reader.Read())
                {
                    ROSCharacteristics model1 = new ROSCharacteristics();
                    var properties = typeof(ROSCharacteristics).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    listobj.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::lookupROSSystems", PROC_REVIEWOFSYSTEM_SYSTEM_LOOKUP, ex);

            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;



        }
        public List<ROSSystems> loadROSSystem(ROSCharacteristics model)
        {
            List<ROSSystems> listobj = new List<ROSSystems>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                createrLoadROSSystemsParameters(dbManager, model, true);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_SYSTEM_SELECT);
                while (reader.Read())
                {
                    ROSSystems model1 = new ROSSystems();
                    var properties = typeof(ROSSystems).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    listobj.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::loadROSSystem", PROC_REVIEWOFSYSTEM_SYSTEM_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;
        }

        public List<ROSCharacteristics> insert_reviewofsystem_system(ROSCharacteristics model, bool isInsert = true)
        {

            List<ROSCharacteristics> listobj = new List<ROSCharacteristics>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                createROSystemParameters(dbManager, model, isInsert);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_SYSTEM_INSERT);
                while (reader.Read())
                {
                    ROSCharacteristics model1 = new ROSCharacteristics();
                    var properties = typeof(ROSCharacteristics).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    listobj.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystem::insert_reviewofsystem_system", PROC_REVIEWOFSYSTEM_SYSTEM_INSERT, ex);
                model.IsError = ex.Message;
                listobj.Add(model);

            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;

            //IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            //try
            //{
            //    dbManager.Open();
            //    createROSystemParameters(dbManager, model, true);
            //    dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_SYSTEM_INSERT);


            //    return "";
            //}
            //catch (Exception ex)
            //{
            //    MDVLogger.DALErrorLog("DALPatientInsurance::insert_reviewofsystem_system", PROC_REVIEWOFSYSTEM_SYSTEM_INSERT, ex);
            //    return ex.Message;
            //}
            //finally
            //{
            //    dbManager.Dispose();
            //}
        }

        public string delete_reviewofsystem_system(long ROSSystemId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SYSTEM_ID, ROSSystemId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_SYSTEM_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::delete_reviewofsystem_system", PROC_REVIEWOFSYSTEM_SYSTEM_DELETE, ex);
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

        public string ActiveInActive_ROSSystem(long ROSSystemId, string IsActive)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SYSTEM_ID, ROSSystemId);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_SYSTEM_ActiveInActive).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystems::ActiveInActive_ROSSystem", PROC_REVIEWOFSYSTEM_SYSTEM_ActiveInActive, ex);
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

        public List<ROSCharacteristics> loadROSSystemCharatristics(string ROSSystemId)
        {
            List<ROSCharacteristics> listobj = new List<ROSCharacteristics>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_SYSTEM_ID, ROSSystemId);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_SYSTEM_CHARATRISTICS_SELECT);
                while (reader.Read())
                {
                    ROSCharacteristics model1 = new ROSCharacteristics();
                    var properties = typeof(ROSCharacteristics).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    listobj.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::loadROSSystemCharatristics", PROC_REVIEWOFSYSTEM_SYSTEM_CHARATRISTICS_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;




        }

        #endregion

        // ROS Revamp Methods


        public DSPhysicalExamECW loadROSRevampTemplates(long templateId, long entityId, int? IsActive = null)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (templateId == 0)
                    dbManager.AddParameters(0, PARM_ROSREVAMP_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ROSREVAMP_TEMPLATE_ID, templateId);

                if (entityId == 0)
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);

                if (IsActive == null || IsActive < 0)
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);
                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROSREVAMPTEMPLATE_SELECT, ds, ds.PETemplate.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::loadROSRevampTemplates", PROC_ROSREVAMPTEMPLATE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region ROS-- Template

        public List<ROSLookUps> loadROSLookUps()
        {
            List<ROSLookUps> listobj = new List<ROSLookUps>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                // createrLoadROSCharatristicParameters(dbManager, model, true);
                SqlDataReader reader = null;
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_LOOKUPS_SELECT);
                while (reader.Read())
                {
                    ROSLookUps model1 = new ROSLookUps();
                    var properties = typeof(ROSLookUps).GetProperties();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.SetValue(model1, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    listobj.Add(model1);
                }

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::loadROSLookUps", PROC_REVIEWOFSYSTEM_LOOKUPS_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return listobj;




        }
        public string Insert_reviewofsystem_Template(ROSTemplateModel model)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (model.TemplateId == "")
                {
                    model.TemplateId = null;
                }
                if ((model.BodyPartId ?? 0) == 0)
                    model.BodyPartId = null;
                dbManager.Open();
                dbManager.CreateParameters(12);

                dbManager.AddParameters(0, PARM_Charatristic_Name, model.Name);
                dbManager.AddParameters(1, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(2, PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(3, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(4, PARM_MODIFIED_ON, model.ModifiedOn);
                dbManager.AddParameters(5, PARM_PROVIDER_XML, model.ProviderXML);
                dbManager.AddParameters(6, PARM_SYSTEMCHRATRISTIC_XML, model.SystemCharacteristicsXML);
                dbManager.AddParameters(7, PARM_ENTITY_ID, model.EntityId);
                dbManager.AddParameters(8, PARM_TEMPLATE_ID, model.TemplateId, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(9, PARM_SPECIALTY_XML, model.SpecialtyXML);
                dbManager.AddParameters(10, PARM_ROSEDIT_TEMPLATEID, model.TemplateId);
                dbManager.AddParameters(11, PARM_BODYPART_ID, model.BodyPartId);
                returnVal = MDVUtility.ToStr(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_TEMPLATE_INSERT));
                if (returnVal != null && MDVUtility.ToStr(returnVal) != "")
                    throw new Exception(MDVUtility.ToStr(returnVal));

                return returnVal;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::Insert_reviewofsystem_Template", PROC_REVIEWOFSYSTEM_TEMPLATE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string update_reviewofsystem_Template(ROSTemplateModel model)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager(); ;
            try
            {
                if ((model.BodyPartId ?? 0) == 0)
                    model.BodyPartId = null;
                dbManager.Open();
                dbManager.CreateParameters(11);

                dbManager.AddParameters(0, PARM_Charatristic_Name, model.Name);
                dbManager.AddParameters(1, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(2, PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(3, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(4, PARM_MODIFIED_ON, model.ModifiedOn);
                dbManager.AddParameters(5, PARM_PROVIDER_XML, model.ProviderXML);
                dbManager.AddParameters(6, PARM_SYSTEMCHRATRISTIC_XML, model.SystemCharacteristicsXML);
                dbManager.AddParameters(7, PARM_ENTITY_ID, model.EntityId);
                dbManager.AddParameters(8, PARM_TEMPLATE_ID, null, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(9, PARM_SPECIALTY_XML, model.SpecialtyXML);
                dbManager.AddParameters(10, PARM_BODYPART_ID, model.BodyPartId);

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_TEMPLATE_INSERT);
                string val = dbManager.Parameters[0].Value.ToString();



                return val;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::update_reviewofsystem_Template", PROC_REVIEWOFSYSTEM_TEMPLATE_INSERT, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string delete_reviewofsystem_template(long ROSTemplateId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ROSTEMPLATE_ID, ROSTemplateId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_TEMPLATE_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::delete_reviewofsystem_template", PROC_REVIEWOFSYSTEM_TEMPLATE_DELETE, ex);
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
        public DataSet select_reviewofsystem_template(long ROSTemplateId, long? NotesId = null)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (NotesId == 0)
                {
                    NotesId = null;
                }

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ROSTEMPLATE_ID, ROSTemplateId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                // dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_TEMPLATE_SELECT);
                // dt = ds[0];

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::select_reviewofsystem_template", PROC_REVIEWOFSYSTEM_TEMPLATE_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return ds;
        }
        public string activeinactive_reviewofsystem_template(long ROSTemplateId, string IsActive, string ModifiedBy)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_ROSTEMPLATE_ID, ROSTemplateId);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(2, PARM_MODIFIED_BY, ModifiedBy);
                dbManager.AddParameters(3, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_TEMPLATE_ACTIVEINACTIVE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamECW::activeinactive_reviewofsystem_template", PROC_REVIEWOFSYSTEM_TEMPLATE_ACTIVEINACTIVE, ex);
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
        #endregion

        #region ROSTemplateRevmap Note
        public string Insert_reviewofsystem_Template_Note(ROSTemplateModel model)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (model.TemplateId == "")
                {
                    model.TemplateId = null;
                }
                dbManager.Open();
                dbManager.CreateParameters(15);

                dbManager.AddParameters(0, PARM_Charatristic_Name, model.Name);
                dbManager.AddParameters(1, PARM_CREATED_BY, model.CreatedBy);
                dbManager.AddParameters(2, PARM_CREATED_ON, model.CreatedOn);
                dbManager.AddParameters(3, PARM_MODIFIED_BY, model.ModifiedBy);
                dbManager.AddParameters(4, PARM_MODIFIED_ON, model.ModifiedOn);
                dbManager.AddParameters(5, PARM_PROVIDER_XML, model.ProviderXML);
                dbManager.AddParameters(6, PARM_SYSTEMCHRATRISTIC_XML, model.SystemCharacteristicsXML);
                dbManager.AddParameters(7, PARM_ENTITY_ID, model.EntityId);
                dbManager.AddParameters(8, PARM_TEMPLATE_ID, model.TemplateId, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(9, PARM_SPECIALTY_XML, model.SpecialtyXML);
                dbManager.AddParameters(10, PARM_ROSEDIT_TEMPLATEID, model.TemplateId);
                dbManager.AddParameters(11, PARM_NOTE_ID, model.NoteId);
                dbManager.AddParameters(12, PARM_SOAP_TEXT, model.SOAPText);
                dbManager.AddParameters(13, PARM_Order_No, 2);
                dbManager.AddParameters(14, PARM_IS_RECORD_DELETE, model.IsRecordDelete);

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_TEMPLATE_INSERT_NOTE);
                string val = dbManager.Parameters[8].Value.ToString();



                return val;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::Insert_reviewofsystem_Template_Note", PROC_REVIEWOFSYSTEM_TEMPLATE_INSERT_NOTE, ex);
                return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DataSet select_reviewofsystem_template_note(long ROSTemplateId, long? NotesId = null, long? SystemId = null)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (NotesId == 0)
                {
                    NotesId = null;
                }

                if (SystemId == 0)
                {
                    SystemId = null;
                }

                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_ROSTEMPLATE_ID, ROSTemplateId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NotesId); //p1
                dbManager.AddParameters(2, PARM_SYSTEM_ID, SystemId);
                // dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_TEMPLATE_SELECT_NOTE);
                // dt = ds[0];

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::select_reviewofsystem_template_note", PROC_REVIEWOFSYSTEM_TEMPLATE_SELECT_NOTE, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return ds;
        }

        public DSPhysicalExamECW load_ROSRevampTemplates_Note(ROSTemplateModel Model)
        {
            DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_NOTE_ID, Model.NoteId);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(2, PARM_ENTITY_ID, Model.EntityId);



                ds = (DSPhysicalExamECW)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ROSREVMAPTEMPLATE_NOTE_SELECT, ds, ds.PETemplate.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::load_ROSRevampTemplates_Note", PROC_ROSREVMAPTEMPLATE_NOTE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string toggle_Characteristics_note(long NotesID, long TemplateId, long ROSSystemId, long ROSCharacteristicsId, string IsPositive)
        {
            //string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_NOTES_ID, NotesID);
                dbManager.AddParameters(1, PARM_TEMPLATE_ID, TemplateId);
                dbManager.AddParameters(2, PARM_SYSTEM_ID, ROSSystemId);
                dbManager.AddParameters(3, PARM_Charatristic_ID, ROSCharacteristicsId);
                dbManager.AddParameters(4, PARM_IS_POSITIVE, IsPositive);
                // returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_CHARDETAIL_TOGGLING_NOTE).ToString();
                dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_CHARDETAIL_TOGGLING_NOTE).ToString();
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewOfSystems::toggle_Characteristics_note", PROC_REVIEWOFSYSTEM_CHARDETAIL_TOGGLING_NOTE, ex);
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
        public DataSet GetROSSystemPatientInfoFromNotes(long UserId, long PatientId, long NoteId, long ROSTemptId, long PrevNotesIdROS = 0)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_ROSTEMPLATE_ID, ROSTemptId);
                dbManager.AddParameters(1, PARM_NOTE_ID, NoteId);
                dbManager.AddParameters(2, PARM_USER_ID, UserId);
                if (PrevNotesIdROS > 0)
                    dbManager.AddParameters(3, PARM_PREV_NOTES_ID, PrevNotesIdROS);
                else
                    dbManager.AddParameters(3, PARM_PREV_NOTES_ID, null);
                ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_SELECT_DEFAULT_NOTE);


            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::GetROSSystemPatientInfoFromNotes", PROC_REVIEWOFSYSTEM_SELECT_DEFAULT_NOTE, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return ds;
        }
        public string delete_reviewofsystem_template_note(long ROSTemplateId, long NotesId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_ROSTEMPLATE_ID, ROSTemplateId);
                dbManager.AddParameters(1, PARM_NOTES_ID, NotesId);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REVIEWOFSYSTEM_TEMPLATE_DELETE_NOTE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReviewofSystems::delete_reviewofsystem_template", PROC_REVIEWOFSYSTEM_TEMPLATE_DELETE_NOTE, ex);
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
        #endregion

    }
}
