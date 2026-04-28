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
using MDVision.Model.AuditableEvents;
using System.Data.SqlClient;
using MDVision.Model.iTrack;

namespace MDVision.DataAccess.DAL.iTrack
{
    /// <summary>
    /// Author: Ahmad Raza
    /// Overview: Data Access Layer for iTrack
    /// </summary>
    public class DALiTrack
    {
        #region " Stored Procedure Names "

        private const string PROC_MIPS_KPIS_LOAD = "Reports.sp_KPISMIPSSelect";
        private const string PROC_ACI_INDIVIDUAL_PROVIDER_DETAIL = "provider.aci_individual_provider_detail";
        private const string PROC_ACI_GROUP_DETAIL = "provider.aci_Group_detail";
        private const string PROC_MIPS_SUMMARY_INDVIDUAL_PROVIDER_DETAIL = "provider.MIPSSummaryIndvidualProviderDetail";
        private const string PROC_MIPS_SUMMARY_GROUP_DETAIL = "provider.MIPSSummaryGroupDetail";
        private const string PROC_MIPS_PREFERENCE_PROVIDER_INDIVIDUAL_SELECT = "Provider.MipsProviderPreferencesSelect";
        private const string PROC_MIPS_PREFERENCE_PROVIDER_INDIVIDUAL_SEARCH = "Provider.MipsProviderPreferencesFill";
        private const string PROC_PATIENTAUDITLOG_LOAD_COMPONENTS = "System.PatientActivityComponents";
        private const string PROC_PATIENTAUDITLOG_LOAD_CHANGES = "System.PatientComponentsChange";
        private const string PROC_PRACTICE_LOAD = "Provider.Sp_SelectPractice";
        private const string PROC_ADMIN_MIPSPROVIDER_LOAD = "Provider.Sp_MipsProvider";
        private const string PROC_GROUPPREFERENCE_SAVE = "Provider.MipsGroupPreferencesinsert";
        private const string PROC_INDIVIDUALPREFERENCE_SAVE = "[Provider].[MipsProviderPreferencesInsert]";
        private const string PROC_INDIVIDUALPREFERENCE_SELECT = "[Provider].[MipsProviderPreferencesSelect]";
        private const string PROC_INDIVIDUALPREFERENCE_UPDATE = "[Provider].[MipsProviderPreferencesUpdate]";
        private const string PROC_GROUPPREFERENCE_UPDATE = "Provider.MipsGroupPreferencesUpdate";
        private const string PROC_MIPSGROUPPREFERENCES_SELECT = "[Provider].[MipsGroupPreferencesSelect]";
        private const string PROC_MIPSCATEGORY_LOOKUP = "[Provider].[MipsCategoryLookup]";
        private const string PROC_MIPSGroupName_LOOKUP = "[Provider].[MipsProviderLookup]";
        private const string PROC_MIPSGROUPPREFERENCES_SEARCH = "[Provider].[Sp_MipsGroupProviders]";
        private const string PROC_MIPSPROVIDERPREFERENCES_SEARCH = "[Provider].[sp_MipsProviderLookup]";
        private const string PROC_MIPSGROUPPREFERENCES_ACTIVEINACTIVE = "[Provider].[Sp_MipsGroupActiveInActive]";
        private const string PROC_MIPSINELIGIBLEREASON_LOOKUP = "[Provider].[MipsInEligibleReasonLookup]";
        private const string PROC_MIPSPARTICIPATINGEREASON_LOOKUP = "[Provider].[MipsParticipatingReasonLookup]";
        private const string PROC_MIPS_PREFERENCE_PROVIDER_INDIVIDUAL_DETAIL_SELECT = "Provider.MipsProviderPreferencesDetailSelect";
        private const string PROC_UPDATE_ITRACK_REPORTINGTYPE = "Provider.sp_UpdateItrackReporting";
        private const string PROC_LOOKUP_IMPROVEMENT_ACTIVITY = "Billing.sp_ImprovementActivitySelect";

        #endregion



        #region " Constructors "
        public DALiTrack()
        {

            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALiTrack(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
        }
        private IContainer components;
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Parameters
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_YEAR = "@Year";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_IS_ACTIVE = "@IsActive";


        #endregion

        #region "Methods"


        public List<Dashboard> LoadMIPSKPIs(Dashboard model, DataTable dtCQM, DataTable dtMu)
        {
            List<Dashboard> listobj = new List<Dashboard>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.AddParameters("@ProviderId", model.ProviderId);
                // dbManager.AddParameters("@Year", model.Year);
                dbManager.AddParameters("@CQMData", dtCQM);
                dbManager.AddParameters("@MUData", dtMu);
                listobj = dbManager.ExecuteReaderMapper<Dashboard>(PROC_MIPS_KPIS_LOAD);

                return listobj;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<IndvidualProvider> SelectIndividualProvider(IndvidualProvider model)
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(16);
                if (!string.IsNullOrEmpty(model.ObjectId))
                    dbManager.AddParameters(0, "@Id", MDVUtility.ToInt64(model.ObjectId));
                else
                    dbManager.AddParameters(0, "@Id", DBNull.Value);
                if (!string.IsNullOrEmpty(model.ProviderId))
                    dbManager.AddParameters(1, "@ProviderId", MDVUtility.ToInt64(model.ProviderId));
                else
                    dbManager.AddParameters(1, "@ProviderId", DBNull.Value);
                if (!string.IsNullOrEmpty(model.Specialty))
                    dbManager.AddParameters(2, "@SpecialtyId", MDVUtility.ToInt64(model.Specialty));
                else
                    dbManager.AddParameters(2, "@SpecialtyId", DBNull.Value);
                if (!string.IsNullOrEmpty(model.NPI))
                    dbManager.AddParameters(3, "@NPI", model.NPI);
                else
                    dbManager.AddParameters(3, "@NPI", DBNull.Value);
                if (!string.IsNullOrEmpty(model.EntityId))
                    dbManager.AddParameters(4, "@EntityId", MDVUtility.ToInt64(model.EntityId));
                else
                    dbManager.AddParameters(4, "@EntityId", DBNull.Value);
                if (!string.IsNullOrEmpty(model.PracticeType))
                    dbManager.AddParameters(5, "@PracticType", model.PracticeType);
                else
                    dbManager.AddParameters(5, "@PracticType", DBNull.Value);
                if (!string.IsNullOrEmpty(model.MIPSEligibilityStatus))
                    dbManager.AddParameters(6, "@MIPSEligilibility", model.MIPSEligibilityStatus);
                else
                    dbManager.AddParameters(6, "@MIPSEligilibility", DBNull.Value);
                if (!string.IsNullOrEmpty(model.InEligibileReason))
                    dbManager.AddParameters(7, "@Ineligbile", model.InEligibileReason);
                else
                    dbManager.AddParameters(7, "@Ineligbile", DBNull.Value);
                if (!string.IsNullOrEmpty(model.ReportingType))
                    dbManager.AddParameters(8, "@ReportingType", model.ReportingType);
                else
                    dbManager.AddParameters(8, "@ReportingType", DBNull.Value);
                if (!string.IsNullOrEmpty(model.ReportingMethod))
                    dbManager.AddParameters(9, "@ReportingMethod", model.ReportingMethod);
                else
                    dbManager.AddParameters(9, "@ReportingMethod", DBNull.Value);
                if (!string.IsNullOrEmpty(model.IsReporting))
                    dbManager.AddParameters(10, "@ReportingStatus", model.IsReporting);
                else
                    dbManager.AddParameters(10, "@ReportingStatus", DBNull.Value);
                if (!string.IsNullOrEmpty(model.IsActive))
                    dbManager.AddParameters(11, "@IsActive", MDVUtility.ToBool(model.IsActive));
                else
                    dbManager.AddParameters(11, "@IsActive", DBNull.Value);
                dbManager.AddParameters(12, "@PageNumber", model.PageNumber);
                dbManager.AddParameters(13, "@RowspPage", model.RowsPerPage);
                dbManager.AddParameters(14, "@RecordCount", null, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(15, "@UserId", MDVSession.Current.AppUserId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MIPS_PREFERENCE_PROVIDER_INDIVIDUAL_SEARCH);
                IndvidualProvider md = null;
                while (reader.Read())
                {
                    md = new IndvidualProvider();
                    md.ObjectId = MDVUtility.ToStr(reader["id"]);
                    md.ProviderId = MDVUtility.ToStr(reader["Providerid"]);
                    md.ProviderName = MDVUtility.ToStr(reader["ProviderName"]);
                    md.Specialty = MDVUtility.ToStr(reader["Specialty"]);
                    md.NPI = MDVUtility.ToStr(reader["NPI"]);
                    md.TIN = MDVUtility.ToStr(reader["TIN"]);
                    md.EntityName = MDVUtility.ToStr(reader["Entity"]);
                    md.MIPSEligibilityStatus = MDVUtility.ToStr(reader["MIPSEligilibility"]);
                    md.IsReporting = MDVUtility.ToStr(reader["ReportingMips"]);
                    md.IsEligibile = MDVUtility.ToStr(reader["Ineligbile"]);
                    md.ReportingType = MDVUtility.ToStr(reader["ReportingType"]);
                    md.ReportingMethod = MDVUtility.ToStr(reader["ReportingMethod"]);
                    md.ReportingYear = MDVUtility.ToStr(reader["ReportingYear"]);
                    md.IsActive = MDVUtility.ToStr(reader["IsActive"]);
                    md.PracticeType = MDVUtility.ToStr(reader["Practicetype"]);
                    md.NotReportingReason = MDVUtility.ToStr(reader["NotReportingReson"]);
                    md.OtherReason = MDVUtility.ToStr(reader["OtherReson"]);
                    md.PracticeName = MDVUtility.ToStr(reader["PracticeName"]);
                    md.GroupName = MDVUtility.ToStr(reader["GroupName"]);
                    md.GroupTIN = MDVUtility.ToStr(reader["GroupTIN"]);
                    md.GroupComments = MDVUtility.ToStr(reader["GroupComments"]);
                    md.JoiningDate = MDVUtility.ToStr(reader["GroupJoiningDate"]);
                    md.LeavingDate = MDVUtility.ToStr(reader["GroupLeavingDate"]);
                    md.RecordCount = MDVUtility.ToStr(reader["RecordCount"]);
                    md.iTrackReportingType = MDVUtility.ToStr(reader["iTrackReportingType"]);
                    md.GroupId = MDVUtility.ToStr(reader["groupid"]);
                    listobj.Add(md);
                }
                return listobj;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALiTrack::SelectIndividualProvider", PROC_MIPS_PREFERENCE_PROVIDER_INDIVIDUAL_SEARCH, ex);
                throw ex;
            }
        }
        public List<IndvidualProvider> LoadIndividualProvider(IndvidualProvider model)
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            SqlDataReader readerDetail = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(16);
                if (!string.IsNullOrEmpty(model.ObjectId))
                {
                    dbManager.AddParameters(0, "@Id", MDVUtility.ToInt64(model.ObjectId));
                }
                else
                {
                    dbManager.AddParameters(0, "@Id", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.ProviderId))
                {
                    dbManager.AddParameters(1, "@ProviderId", MDVUtility.ToInt64(model.ProviderId));
                }
                else
                {
                    dbManager.AddParameters(1, "@ProviderId", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.Specialty))
                {
                    dbManager.AddParameters(2, "@SpecialtyId", MDVUtility.ToInt64(model.Specialty));
                }
                else
                {
                    dbManager.AddParameters(2, "@SpecialtyId", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.NPI))
                {
                    dbManager.AddParameters(3, "@NPI", model.NPI);
                }
                else
                {
                    dbManager.AddParameters(3, "@NPI", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.EntityId))
                {
                    dbManager.AddParameters(4, "@EntityId", MDVUtility.ToInt64(model.EntityId));
                }
                else
                {
                    dbManager.AddParameters(4, "@EntityId", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.PracticeType))
                {
                    dbManager.AddParameters(5, "@PracticType", model.PracticeType);
                }
                else
                {
                    dbManager.AddParameters(5, "@PracticType", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.MIPSEligibilityStatus))
                {
                    dbManager.AddParameters(6, "@MIPSEligilibility", model.MIPSEligibilityStatus);
                }
                else
                {
                    dbManager.AddParameters(6, "@MIPSEligilibility", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.InEligibileReason))
                {
                    dbManager.AddParameters(7, "@Ineligbile", model.InEligibileReason);
                }
                else
                {
                    dbManager.AddParameters(7, "@Ineligbile", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.ReportingType))
                {
                    dbManager.AddParameters(8, "@ReportingType", model.ReportingType);
                }
                else
                {
                    dbManager.AddParameters(8, "@ReportingType", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.ReportingMethod))
                {
                    dbManager.AddParameters(9, "@ReportingMethod", model.ReportingMethod);
                }
                else
                {
                    dbManager.AddParameters(9, "@ReportingMethod", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.IsReporting))
                {
                    dbManager.AddParameters(10, "@ReportingStatus", model.IsReporting);
                }
                else
                {
                    dbManager.AddParameters(10, "@ReportingStatus", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.IsActive))
                {
                    dbManager.AddParameters(11, "@IsActive", MDVUtility.ToBool(model.IsActive));
                }
                else
                {
                    dbManager.AddParameters(11, "@IsActive", DBNull.Value);
                }
                dbManager.AddParameters(12, "@PageNumber", model.PageNumber);
                dbManager.AddParameters(13, "@RowspPage", model.RowsPerPage);
                dbManager.AddParameters(14, "@RecordCount", null, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(15, "@UserId", MDVSession.Current.AppUserId);
               

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MIPS_PREFERENCE_PROVIDER_INDIVIDUAL_SELECT);

                IndvidualProvider md = null;
                while (reader.Read())
                {
                    md = new IndvidualProvider();
                    md.ObjectId = MDVUtility.ToStr(reader["id"]);
                    md.ProviderId = MDVUtility.ToStr(reader["Providerid"]);
                    md.ProviderName = MDVUtility.ToStr(reader["ProviderName"]);
                    md.Specialty = MDVUtility.ToStr(reader["Specialty"]);
                    md.NPI = MDVUtility.ToStr(reader["NPI"]);
                    md.TIN = MDVUtility.ToStr(reader["TIN"]);
                    md.EntityName = MDVUtility.ToStr(reader["Entity"]);
                    md.MIPSEligibilityStatus = MDVUtility.ToStr(reader["MIPSEligilibility"]);
                    md.IsReporting = MDVUtility.ToStr(reader["ReportingMips"]);
                    md.IsEligibile = MDVUtility.ToStr(reader["Ineligbile"]);
                    md.ReportingType = MDVUtility.ToStr(reader["ReportingType"]);
                    md.ReportingMethod = MDVUtility.ToStr(reader["ReportingMethod"]);
                    md.ReportingYear = MDVUtility.ToStr(reader["ReportingYear"]);
                    md.IsActive = MDVUtility.ToStr(reader["IsActive"]);
                    md.PracticeType = MDVUtility.ToStr(reader["Practicetype"]);
                    md.NotReportingReason = MDVUtility.ToStr(reader["NotReportingReson"]);
                    md.OtherReason = MDVUtility.ToStr(reader["OtherReson"]);
                    md.PracticeName = MDVUtility.ToStr(reader["PracticeName"]);
                    md.GroupName = MDVUtility.ToStr(reader["GroupName"]);
                    md.GroupTIN = MDVUtility.ToStr(reader["GroupTIN"]);
                    md.GroupComments = MDVUtility.ToStr(reader["GroupComments"]);
                    md.JoiningDate = MDVUtility.ToStr(reader["GroupJoiningDate"]);
                    md.LeavingDate = MDVUtility.ToStr(reader["GroupLeavingDate"]);
                    md.ParticipatingReason = MDVUtility.ToStr(reader["ParticipatingReason"]);
                    md.InEligibileReason = MDVUtility.ToStr(reader["EligibleIneligible Reason"]);
                    md.OtherComments = MDVUtility.ToStr(reader["Other EligibleIneligible Reason"]);
                    md.RecordCount = MDVUtility.ToStr(reader["RecordCount"]);
                    listobj.Add(md);
                }

                return listobj;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IndvidualProvider LoadIndividualProviderData(long providerId)
        {
            IndvidualProvider obj = new IndvidualProvider();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@ProviderId", MDVUtility.ToInt64(providerId));
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ACI_INDIVIDUAL_PROVIDER_DETAIL);
                while (reader.Read())
                {
                    obj.IsFullYear = MDVUtility.ToStr(reader["IsFullYear"]);
                    obj.StartDate = MDVUtility.ToStr(reader["StartDate"]);
                    obj.EndDate = MDVUtility.ToStr(reader["EndDate"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALiTrack::LoadIndividualProviderData", PROC_ACI_INDIVIDUAL_PROVIDER_DETAIL, ex);
                throw ex;
            }
        }
        public IndvidualProvider LoadGroupDetailData(long GroupId)
        {
            IndvidualProvider obj = new IndvidualProvider();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@GroupID", MDVUtility.ToInt64(GroupId));
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_ACI_GROUP_DETAIL);
                while (reader.Read())
                {
                    obj.IsFullYear = MDVUtility.ToStr(reader["IsFullYear"]);
                    obj.StartDate = MDVUtility.ToStr(reader["StartDate"]);
                    obj.EndDate = MDVUtility.ToStr(reader["EndDate"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALiTrack::LoadGroupDetailData", PROC_ACI_GROUP_DETAIL, ex);
                throw ex;
            }
        }
        public List<IndvidualProvider> LoadMIPSSummaryGroupDetailData(long GroupId)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<IndvidualProvider> list = new List<IndvidualProvider>();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@GroupID", MDVUtility.ToInt64(GroupId));
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MIPS_SUMMARY_GROUP_DETAIL);
                while (reader.Read())
                {
                    IndvidualProvider model = new IndvidualProvider();
                    model.IsFullYear = !String.IsNullOrEmpty(reader["IsFullYear"].ToString()) ? MDVUtility.ToStr(reader["IsFullYear"]) : "";
                    model.StartDate = !String.IsNullOrEmpty(reader["StartDate"].ToString()) ? MDVUtility.ToStr(reader["StartDate"]) : "";
                    model.EndDate = !String.IsNullOrEmpty(reader["EndDate"].ToString()) ? MDVUtility.ToStr(reader["EndDate"]) : "";
                    model.CategoryName = !String.IsNullOrEmpty(reader["Name"].ToString()) ? MDVUtility.ToStr(reader["Name"]) : "";
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALiTrack::LoadMIPSSummaryGroupDetailData", PROC_MIPS_SUMMARY_GROUP_DETAIL, ex);
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }
        public List<IndvidualProvider> LoadMIPSSummaryIndividualProviderDetailData(long ProviderId)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<IndvidualProvider> list = new List<IndvidualProvider>();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@ProviderId", MDVUtility.ToInt64(ProviderId));
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MIPS_SUMMARY_INDVIDUAL_PROVIDER_DETAIL);
                while (reader.Read())
                {
                    IndvidualProvider model = new IndvidualProvider();
                    model.IsFullYear = !String.IsNullOrEmpty(reader["IsFullYear"].ToString()) ? MDVUtility.ToStr(reader["IsFullYear"]) : "";
                    model.StartDate = !String.IsNullOrEmpty(reader["StartDate"].ToString()) ? MDVUtility.ToStr(reader["StartDate"]) : "";
                    model.EndDate = !String.IsNullOrEmpty(reader["EndDate"].ToString()) ? MDVUtility.ToStr(reader["EndDate"]) : "";
                    model.CategoryName = !String.IsNullOrEmpty(reader["Name"].ToString()) ? MDVUtility.ToStr(reader["Name"]) : "";
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALiTrack::LoadMIPSSummaryIndividualProviderDetailData", PROC_MIPS_SUMMARY_INDVIDUAL_PROVIDER_DETAIL, ex);
                throw ex;
            }
            finally {
                if (reader != null)
                    reader.Close();
                dbManager.Dispose();
            }
        }

        public List<IndvidualProvider> LoadIndividualProviderDetail(long categoryId, long pageNumber, long rowsPPage)
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(4);

                dbManager.AddParameters(0, "@Id", categoryId);

                dbManager.AddParameters(1, "@PageNumber", pageNumber);


                dbManager.AddParameters(2, "@RowspPage", rowsPPage);


                dbManager.AddParameters(3, "@RecordCount", null, DbType.Int64, ParamDirection.Output);



                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MIPS_PREFERENCE_PROVIDER_INDIVIDUAL_DETAIL_SELECT);

                IndvidualProvider md = null;
                while (reader.Read())
                {
                    md = new IndvidualProvider();
                    md.CategoryId = MDVUtility.ToStr(reader["CategoryId"]);
                    md.IsFullYear = MDVUtility.ToStr(reader["IsFullYear"]);
                    md.StartDate = MDVUtility.ToStr(reader["StartDate"]);
                    md.EndDate = MDVUtility.ToStr(reader["EndDate"]);
                    md.ObjectType = MDVUtility.ToStr(reader["ObjectType"]);
                    md.CategoryName = MDVUtility.ToStr(reader["Name"]);

                    listobj.Add(md);
                }

                return listobj;

            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<IndvidualProvider> LoadPracticLookup(string EntityId)
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.AddParameters("@EntityId", EntityId);
                // dbManager.AddParameters("@Year", model.Year);

                listobj = dbManager.ExecuteReaderMapper<IndvidualProvider>(PROC_PRACTICE_LOAD);

                return listobj;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<IndvidualProvider> LoadGroupCatLookup()
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                listobj = dbManager.ExecuteReaderMapper<IndvidualProvider>(PROC_MIPSCATEGORY_LOOKUP);

                return listobj;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<IndvidualProvider> LoadIneligibleReasonLookup()
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                listobj = dbManager.ExecuteReaderMapper<IndvidualProvider>(PROC_MIPSINELIGIBLEREASON_LOOKUP);

                return listobj;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<IndvidualProvider> LoadParticipatingReasonLookup()
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                listobj = dbManager.ExecuteReaderMapper<IndvidualProvider>(PROC_MIPSPARTICIPATINGEREASON_LOOKUP);

                return listobj;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<IndvidualProvider> LoadGroupNameLookup()
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                listobj = dbManager.ExecuteReaderMapper<IndvidualProvider>(PROC_MIPSGroupName_LOOKUP);

                return listobj;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<IndvidualProvider> SaveMIPSPreferencesGroup(IndvidualProvider Model, string xml = null)
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.AddParameters("@Id", Model.GroupId);
                dbManager.AddParameters("@Name", Model.GroupName);
                dbManager.AddParameters("@TIN", Model.TIN);
                dbManager.AddParameters("@PracticeId", Model.PracticeId);
                dbManager.AddParameters("@Comments", Model.ReportingReason);
                dbManager.AddParameters("@IsActive", Model.IsActive);
                dbManager.AddParameters("@CreatedBy", Model.CreatedBy);
                dbManager.AddParameters("@CreatedOn", Model.CreatedOn);
                dbManager.AddParameters("@ModifiedBy", Model.ModifiedBy);
                dbManager.AddParameters("@ModifiedOn", Model.ModifiedOn);
                dbManager.AddParameters("@IsReporting", Model.IsReporting);
                dbManager.AddParameters("@Practicetype", Model.PracticeType);
                dbManager.AddParameters("@XMLEvent", xml);
                dbManager.AddParameters("@PerformanceYear", Model.SubmissionYear);
                dbManager.AddParameters("@EntityID", Model.EntityId);

                listobj = dbManager.ExecuteReaderMapper<IndvidualProvider>(PROC_GROUPPREFERENCE_SAVE);

                return listobj;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<IndvidualProvider> SaveMIPSPreferencesIndvidual(IndvidualProvider Model, string xml = null)
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.AddParameters("@Id", MDVUtility.ToInt64(Model.ObjectId));
                dbManager.AddParameters("@ProviderId", MDVUtility.ToInt64(Model.ProviderId));
                dbManager.AddParameters("@ReportingType", Model.ReportingType);
                dbManager.AddParameters("@ReportingMethod", Model.ReportingMethod);
                dbManager.AddParameters("@PerformanceYear", MDVUtility.ToInt64(Model.PerformanceYear));
                dbManager.AddParameters("@PracticeType", Model.PracticeType);
                dbManager.AddParameters("@IsEligible", MDVUtility.ToBool(Model.IsEligibile));
                dbManager.AddParameters("@IsReporting", MDVUtility.ToBool(Model.IsReporting));
                dbManager.AddParameters("@OtherComments", Model.OtherComments);
                dbManager.AddParameters("@Reason", Model.ReportingReason);
                dbManager.AddParameters("@IsActive", MDVUtility.ToBool(Model.IsActive));
                dbManager.AddParameters("@CreatedBy", Model.CreatedBy);
                dbManager.AddParameters("@CreatedOn", Model.CreatedOn);
                dbManager.AddParameters("@ModifiedBy", Model.ModifiedBy);
                dbManager.AddParameters("@ModifiedOn", Model.ModifiedOn);
                dbManager.AddParameters("@XMLEvent", xml);
                dbManager.AddParameters("@Comments", Model.JoiningComments);
                dbManager.AddParameters("@OtherReson", Model.ReportingReason);
                dbManager.AddParameters("@NotReportingReson", Model.ReportingReason);
                if (MDVUtility.ToInt64(Model.ParticipatingId) > 0)
                {
                    dbManager.AddParameters("@ParticipatingId", MDVUtility.ToInt64(Model.ParticipatingId));
                }else
                {
                    dbManager.AddParameters("@ParticipatingId", null);
                }
                if (MDVUtility.ToInt64(Model.GroupId) > 0)
                {
                    dbManager.AddParameters("@GroupId", MDVUtility.ToInt64(Model.GroupId));
                }else
                {
                    dbManager.AddParameters("@GroupId", null);
                }
               
                if (! string.IsNullOrEmpty(Model.JoiningDate))
                {
                    dbManager.AddParameters("@GroupJoiningDate", MDVUtility.ToDateTime(Model.JoiningDate));
                }else
                {
                    dbManager.AddParameters("@GroupJoiningDate", null);
                }
                if (!string.IsNullOrEmpty(Model.LeavingDate))
                {
                    dbManager.AddParameters("@GroupLeavingDate", MDVUtility.ToDateTime(Model.LeavingDate));
                }
                else
                {
                    dbManager.AddParameters("@GroupLeavingDate", null);
                }

                listobj = dbManager.ExecuteReaderMapper<IndvidualProvider>(PROC_INDIVIDUALPREFERENCE_SAVE);

                return listobj;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public string UpdateMIPSPreferencesIndvidual(IndvidualProvider Model, string xml = null)
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(21);
                dbManager.AddParameters(0,"@Id", MDVUtility.ToInt64(Model.ObjectId));
                dbManager.AddParameters(1,"@ProviderId", MDVUtility.ToInt64(Model.ProviderId));
                dbManager.AddParameters(2,"@ReportingType", Model.ReportingType);
                dbManager.AddParameters(3,"@ReportingMethod", Model.ReportingMethod);
                dbManager.AddParameters(4,"@PerformanceYear", Model.PerformanceYear);
                dbManager.AddParameters(5,"@PracticeType", Model.PracticeType);
                dbManager.AddParameters(6,"@IsEligible", MDVUtility.ToBool(Model.IsEligibile));
                dbManager.AddParameters(7,"@IsReporting", MDVUtility.ToBool(Model.IsReporting));
                dbManager.AddParameters(8,"@OtherComments", Model.OtherComments);
                dbManager.AddParameters(9,"@Reason", Model.ReportingReason);
                dbManager.AddParameters(10,"@IsActive", MDVUtility.ToBool(Model.IsActive));
                dbManager.AddParameters(11,"@ModifiedBy", Model.ModifiedBy);
                dbManager.AddParameters(12,"@ModifiedOn", Model.ModifiedOn);
                dbManager.AddParameters(13,"@Comments", Model.JoiningComments);
                dbManager.AddParameters(14,"@OtherReson", Model.ReportingReason);
                dbManager.AddParameters(15,"@NotReportingReson", Model.ReportingReason);
                if (MDVUtility.ToInt64(Model.ParticipatingId) > 0)
                {
                    dbManager.AddParameters(16, "@ParticipatingId", MDVUtility.ToInt64(Model.ParticipatingId));
                }else
                {
                    dbManager.AddParameters(16, "@ParticipatingId", null);
                }
                
                dbManager.AddParameters(17,"@XMLEvent", xml);
                if (MDVUtility.ToInt64(Model.GroupId) > 0)
                {
                    dbManager.AddParameters(18, "@groupid", MDVUtility.ToInt64(Model.GroupId));
                }else
                {
                    dbManager.AddParameters(18, "@groupid", null);
                }
                if (! string.IsNullOrEmpty(Model.JoiningDate))
                {
                    dbManager.AddParameters(19, "@GroupJoiningDate", MDVUtility.ToDateTime(Model.JoiningDate));
                }else
                {
                    dbManager.AddParameters(19, "@GroupJoiningDate", null);
                }
                if (!string.IsNullOrEmpty(Model.LeavingDate))
                {
                    dbManager.AddParameters(20, "@GroupLeavingDate", MDVUtility.ToDateTime(Model.LeavingDate));
                }else
                {
                    dbManager.AddParameters(20, "@GroupLeavingDate", null);
                }
                

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure,PROC_INDIVIDUALPREFERENCE_UPDATE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALiTrack::UpdateMIPSPreferencesIndvidual", PROC_INDIVIDUALPREFERENCE_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
        }

        public string UpdateiTrackReportingType(IndvidualProvider Model)
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@Id", MDVUtility.ToInt64(Model.ObjectId));
                dbManager.AddParameters(1, "@iTrackReportingType", Model.iTrackReportingType);
                
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_UPDATE_ITRACK_REPORTINGTYPE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALiTrack::UpdateiTrackReportingType", PROC_UPDATE_ITRACK_REPORTINGTYPE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
        }
        public List<IndvidualProvider> ActiveInActiveMIPSPreferencesGroup(IndvidualProvider Model)
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.AddParameters("@GroupId", Model.GroupId);
                dbManager.AddParameters("@IsActive", Model.IsActive);
                dbManager.AddParameters("@ModifiedBy", Model.ModifiedBy);
                dbManager.AddParameters("@ModifiedOn", Model.ModifiedOn);
                listobj = dbManager.ExecuteReaderMapper<IndvidualProvider>(PROC_MIPSGROUPPREFERENCES_ACTIVEINACTIVE);

                return listobj;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string UpdateMIPSPreferencesGroup(IndvidualProvider Model, string xml = null)
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            int rslt = 0;
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(13);
                dbManager.AddParameters(0,"@Id", Model.GroupId);
                dbManager.AddParameters(1,"@Name", Model.GroupName);
                dbManager.AddParameters(2,"@TIN", Model.TIN);
                dbManager.AddParameters(3,"@PracticeId", Model.PracticeId);
                dbManager.AddParameters(4,"@Comments", Model.ReportingReason);
                dbManager.AddParameters(5,"@IsActive", Model.IsActive);
                dbManager.AddParameters(6,"@ModifiedBy", Model.ModifiedBy);
                dbManager.AddParameters(7,"@ModifiedOn", Model.ModifiedOn);
                dbManager.AddParameters(8,"@IsReporting", Model.IsReporting);
                dbManager.AddParameters(9,"@Practicetype", Model.PracticeType);
                dbManager.AddParameters(10,"@PerformanceYear", Model.SubmissionYear);
                dbManager.AddParameters(11,"@EntityID", Model.EntityId);
                dbManager.AddParameters(12,"@XMLEvent", xml);

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_GROUPPREFERENCE_UPDATE);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALiTrack::UpdateMIPSPreferencesGroup", PROC_GROUPPREFERENCE_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
        }
        public List<IndvidualProvider> LoadMIPSProvider(IndvidualProvider model)
        {
            List<IndvidualProvider> listobj = new List<IndvidualProvider>();
            //DSPhysicalExamECW ds = new DSPhysicalExamECW();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.AddParameters("@EntityId", model.EntityId);
                dbManager.AddParameters("@ReportingMethod", model.ReportingMethod);
                dbManager.AddParameters("@GroupId", model.GroupId);
                dbManager.AddParameters("@UserId", MDVSession.Current.AppUserId);
                dbManager.AddParameters("@PageNumber", model.PageNumber);
                dbManager.AddParameters("@RowspPage", model.RowsPerPage);
                dbManager.AddParameters("@RecordCount", model.RecordCount, DbType.Int64, ParamDirection.Output);

                // dbManager.AddParameters("@Year", model.Year);

                listobj = dbManager.ExecuteReaderMapper<IndvidualProvider>(PROC_ADMIN_MIPSPROVIDER_LOAD);

                return listobj;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MIPSGrouupPreferenceList SearchMIPSGroupPreferences(IndvidualProvider model)
        {
            MIPSGrouupPreferenceList listobj = new MIPSGrouupPreferenceList();
            listobj.Groups = new List<IndvidualProvider>();
            listobj.GroupsDetail = new List<IndvidualProvider>();
            listobj.GroupDetail = new List<MIPSGroupDetail>();
          
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (model.IsActive == "" || model.IsActive == null)
                {
                    model.IsActive = "True";
                }
                dbManager.Open();
                dbManager.Open();
                dbManager.CreateParameters(8);
                dbManager.AddParameters(0, "@EntityId", MDVUtility.ToInt32(model.EntityId));
                dbManager.AddParameters(1, "@GroupName", model.GroupName);
                dbManager.AddParameters(2, "@performanceYear", model.PerformanceYear);
                dbManager.AddParameters(3, "@isactive", MDVUtility.ToBool(model.IsActive));
                dbManager.AddParameters(4, "@PageNumber", MDVUtility.ToInt32(model.PageNumber));
                dbManager.AddParameters(5, "@RowspPage", MDVUtility.ToInt32(model.RowsPerPage));
                dbManager.AddParameters(6, "@ReportingMethod", model.ReportingMethod);
                dbManager.AddParameters(7, "@RecordCount", model.RecordCount, DbType.Int64, ParamDirection.Output);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MIPSGROUPPREFERENCES_SEARCH);

                while (reader.Read())
                {
                    IndvidualProvider Groups = new IndvidualProvider();
                    Groups.GroupId = Convert.ToString(reader["Id"]);
                    Groups.GroupName = Convert.ToString(reader["GroupName"]);
                    Groups.TIN = Convert.ToString(reader["GroupTIN"]);
                    Groups.PerformanceYear = Convert.ToString(reader["PerformanceYear"]);
                    Groups.CreatedOn = Convert.ToString(reader["Created"]);
                    Groups.IsActive = Convert.ToString(reader["IsActive"]);
                    Groups.RecordCount = Convert.ToString(reader["RecordCount"]);
                    Groups.JoiningDate = Convert.ToString(reader["GroupJoiningDate"]);
                    Groups.LeavingDate = Convert.ToString(reader["GroupLeavingDate"]);
                    Groups.JoiningComments = Convert.ToString(reader["Comments"]);
                    Groups.IsReporting = Convert.ToString(reader["IsReporting"]);
                    Groups.PracticeType = Convert.ToString(reader["Practicetype"]);
                    listobj.Groups.Add(Groups);
                }
                reader.NextResult();
                while (reader.Read())
                {

                    MIPSGroupDetail Detail = new MIPSGroupDetail();
                    Detail.GroupId = Convert.ToString(reader["groupId"]);
                    Detail.ProviderName = Convert.ToString(reader["MemberProviders"]);
                    Detail.ProviderId = Convert.ToString(reader["providerid"]);

                    listobj.GroupDetail.Add(Detail);
                }
                reader.NextResult();
                while (reader.Read())
                {

                    IndvidualProvider GroupsDetail = new IndvidualProvider();
                    GroupsDetail.StartDate = Convert.ToString(reader["StartDate"]);
                    GroupsDetail.EndDate = Convert.ToString(reader["EndDate"]);
                    GroupsDetail.CategoryName = Convert.ToString(reader["CategoryName"]);

                    listobj.GroupsDetail.Add(GroupsDetail);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::LoadDashboardDocument", PROC_MIPSGROUPPREFERENCES_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public MIPSGrouupPreferenceList SearchMIPSProviderPreferences(IndvidualProvider model)
        {
            MIPSGrouupPreferenceList listobj = new MIPSGrouupPreferenceList();
            listobj.Groups = new List<IndvidualProvider>();
            listobj.GroupsDetail = new List<IndvidualProvider>();


            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (model.IsActive == "" || model.IsActive == null)
                {
                    model.IsActive = "True";
                }
                dbManager.Open();
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, "@UserId", MDVSession.Current.AppUserId);
                dbManager.AddParameters(1, "@EntityId", MDVUtility.ToInt32(model.EntityId));
                dbManager.AddParameters(2, "@IsActive", MDVUtility.ToBool(model.IsActive));
                dbManager.AddParameters(3, "@ShortName", model.ShortName);
               
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MIPSPROVIDERPREFERENCES_SEARCH);

                while (reader.Read())
                {
                    IndvidualProvider Groups = new IndvidualProvider();
                    Groups.ProviderId = Convert.ToString(reader["ProviderId"]);
                    Groups.ShortName = Convert.ToString(reader["ShortName"]);
                    Groups.ReportingMethod = Convert.ToString(reader["ReportingMethod"]);
                    Groups.PerformanceYear = Convert.ToString(reader["PerformanceYear"]);
                    Groups.IsReporting = Convert.ToString(reader["IsReporting"]);
                    Groups.PracticeType = Convert.ToString(reader["Practicetype"]);
                    Groups.JoiningComments = Convert.ToString(reader["NotReportingReson"]);


                    listobj.Groups.Add(Groups);
                }
                reader.NextResult();
                while (reader.Read())
                {

                    IndvidualProvider GroupsDetail = new IndvidualProvider();
                    GroupsDetail.StartDate = Convert.ToString(reader["StartDate"]);
                    GroupsDetail.EndDate = Convert.ToString(reader["EndDate"]);
                    GroupsDetail.CategoryName = Convert.ToString(reader["CategoryName"]);
                    GroupsDetail.ProviderId = Convert.ToString(reader["ProviderId"]);

                    listobj.GroupsDetail.Add(GroupsDetail);
                }


                return listobj;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::SearchMIPSProviderPreferences", PROC_MIPSPROVIDERPREFERENCES_SEARCH, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public MIPSGrouupPreferenceList SelectMIPSGroupPreferences(IndvidualProvider model)
        {
            MIPSGrouupPreferenceList listobj = new MIPSGrouupPreferenceList();
            listobj.Groups = new List<IndvidualProvider>();
            listobj.GroupDetail = new List<MIPSGroupDetail>();
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@Id", model.GroupId);
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_MIPSGROUPPREFERENCES_SELECT);
                IndvidualProvider IPModel = null;
                while (reader.Read())
                {
                    IPModel = new IndvidualProvider();
                    IPModel.GroupId = Convert.ToString(reader["Id"]);
                    IPModel.GroupName = Convert.ToString(reader["Name"]);
                    IPModel.TIN = Convert.ToString(reader["TIN"]);
                    IPModel.PracticeId = Convert.ToString(reader["PracticeId"]);
                    IPModel.ReportingReason = Convert.ToString(reader["Comments"]);
                    IPModel.PerformanceYear = Convert.ToString(reader["PerformanceYear"]);
                    IPModel.EntityId = Convert.ToString(reader["EntityId"]);
                    IPModel.IsActive = Convert.ToString(reader["IsActive"]);
                    IPModel.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    IPModel.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    IPModel.ModifiedBy = Convert.ToString(reader["ModifiedBy"]);
                    IPModel.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                    IPModel.IsReporting = Convert.ToString(reader["IsReporting"]);
                    IPModel.PracticeType = Convert.ToString(reader["PracticeType"]);
                    listobj.Groups.Add(IPModel);
                }
                reader.NextResult();

                while (reader.Read())
                {

                    MIPSGroupDetail Detail = new MIPSGroupDetail();
                    Detail.GroupId = Convert.ToString(reader["Id"]);
                    Detail.IsFullYear = Convert.ToString(reader["IsFullYear"]);
                    Detail.DateFrom = Convert.ToString(reader["StartDate"]);
                    Detail.DateTo = Convert.ToString(reader["EndDate"]);
                    Detail.ReportingCat = Convert.ToString(reader["Category"]);

                    listobj.GroupDetail.Add(Detail);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPatientDocument::LoadDashboardDocument", PROC_MIPSGROUPPREFERENCES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region Improvement Activit Lookup
        public DSiTrack LookupImprovementActivity(string Active)
        {
            DSiTrack ds = new DSiTrack();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(1);

                //if (POSId <= 0)
                //    dbManager.AddParameters(0, PARM_POS_ID, null);
                //else
                //    dbManager.AddParameters(0, PARM_POS_ID, POSId);

                dbManager.AddParameters(0, PARM_IS_ACTIVE, Active);

                ds = (DSiTrack)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOKUP_IMPROVEMENT_ACTIVITY, ds, ds.DT_IA_Lookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupProcedureCategory", PROC_LOOKUP_IMPROVEMENT_ACTIVITY, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
    }
}
