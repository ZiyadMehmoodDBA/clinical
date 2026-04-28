
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;
using System.Data.SqlClient;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Model.Clinical.Macros;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALMacro
    {
        #region Variable

        #endregion



        #region Parameters
        //Start//18-04-2016//Abid Ali// Paramenters Names

        private const string PARM_NOTE_ID = "NoteId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_Entity_ID = "@EntityId";
        private const string PARM_User_ID = "@UserId";
        private const string PARM_LAB_ORDER_RESULT_ID = "@LabOrderResultId";
        private const string PARM_LAB_TEST_ID = "@LabOrderTestId";
        private const string PARM_LAB_ORDER_ID = "@LabOrderId";
        private const string PARM_TEST = "@Test";
        private const string PARM_ORDER_NO = "@OrderNo";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_ORDER_DATE_FROM = "@OrderDateFrom";
        private const string PARM_ORDER_DATE_TO = "@OrderDateTo";
        private const string PARM_STATUS = "@Status";
        private const string PARM_LAB_ID = "@LabId";
        private const string PARM_REFERENCE_RANGE_INTERPRATION = "@ReferenceRangeInterpration";
        private const string PARM_TEST_ANTIMICROBIAL = "@TestAntimicrobial";
        private const string PARM_REFERENCE_RANGE_DESCRIPTION = "@ReferenceRangeDescription";

        private const string PARM_OBSERVATION_DATE = "@ObservationDate";

        private const string PARM_LAB_ORDER_RESULT_DETAIL_ID = "@LabOrderResultDetailId";

        private const string PARM_CPT_CODE = "@CPTCode";
        private const string PARM_CPT_DESCRIPTION = "@CPTCodeDescription";

        private const string PARM_LOINC = "@LOINC";
        private const string PARM_LOINC_DECSRIPTION = "@LOINCDescription";

        private const string PARM_RESULT = "@Result";
        private const string PARM_CONDITION_STATEMENT = "@ConditionStatement";
        private const string PARM_UoM = "@UoM";
        private const string PARM_FLAG = "@Flag";
        private const string PARM_RANGE = "@Range";

        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_REMARKS = "@Remarks";
        private const string PARM_FINAL_INTERPRETATION = "@FinalInterpretation";


        private const string PARM_IS_ACTIVE = "@IsActive";

        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";

        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_MODIFIED_BY = "ModifiedBy";
        private const string PARM_SOAP_TEXT = "@SoapText";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        private const string PARM_ASSIGNEE_ID = "@AssigneeId";

        private const string PARM_REVIEWEDBY_ID = "@ReviewedById";

        //End//18-04-2016//Abid Ali// Paramenters Names


        //Start 18-04-2016 Muhammad Arshad Lookup LOINC
        private const string PARM_LOINC_CODE = "@LOINCCode";
        private const string PARM_LOINC_CODE_DESCRIPTION = "@LOINCCodeDescription";
        //End 18-04-2016 Muhammad Arshad Lookup LOINC


        private const string PARM_LABID = "@LabId";

        //Start 26-04-2016 Muhammad Azhar Shahzad HL7 data 
        private const string PARM_OBSERVATION_RESULT_STATUS = "@ObservationResultStatus";
        private const string PARM_OBSERVATION_VALUE = "@ObservationValue";
        //End 26-04-2016 Muhammad Azhar Shahzad HL7 data 

        private const string PARM_LABORDER_ID = "@LabOrderId";
        private const string PARM_SPECIMEN_ID = "@SpecimenId";
        private const string PARM_COLLECTION_DATETIME = "@CollectionDateTime";
        private const string PARM_QUANTITY = "@Quantity";
        private const string PARM_NAME_OF_CODINGSYSTEM = "@NameofCodingSystem";
        private const string PARM_ORIGINAL_TEXT = "@OriginalText";
        private const string PARM_TEXT = "@Text";
        private const string PARM_LABRESULT_SPECIMEN_ID = "@LabResultSpecimenId";
        private const string PARM_SPECIMEN_TYPE = "@SpecimenType";
        private const string PARM_SPECIMEN_REJECT_REASON_ID = "@SpecimenRejectReasonId";
        private const string PARM_LABORDER_TEST_ID = "@LabOrderTestId";
        private const string PARM_ALTERNATE_TEXT = "@AlternateText";
        private const string PARM_NAMEOF_ALTERNATE_CODING_SYSTEM = "@NameofAlternateCodingSystem";
        private const string PARM_ALTERNATE_IDENTIFIER = "@AlternateIdentifier";
        private const string PARM_LAB_RESULT_SPECIMEN_ID = "@LabResultSpecimenId";
        private const string PARM_IDENTIFIER = "@Identifier";

        private const string PARM_CONDITION_NOC_SYSTEM = "@ConditionNOCSystem";
        private const string PARM_CONDITION_TEXT = "@ConditionText";

        private const string PARM_NAME_OF_ALTERNATE_CODING_SYSTEM = "@NameofAlternateCodingSystem";
        private const string PARM_CONDITION_ORIGINAL_TEXT = "@ConditionOriginalText";

        private const string PARM_IS_SENT_TO_POTRTAL = "@IsSentToPortal";

        private const string PARM_IS_AKNOWLEDGED = "@IsAknowledged";
        private const string PARM_MARKASREVIEWED = "@MarkAsReviewed";

        private const string PARM_IS_Reviewed = "@IsReviewed";
        private const string PARM_IS_All_Result = "@IsAllResult";
        private const string PARM_IS_ATTRIBUTE = "@IsAttribute";

        private const string PARM_LABTEST_ID = "@LabTestId";
        private const string PARM_LABTESTATTRIBUTE_ID = "@LabTestAttributeId";
        private const string PARM_Url = "@Url";


        #endregion

        #region Constructors

        public DALMacro()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        public DALMacro(SharedVariable SharedVariable)
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
        public MacroModel SaveDetails(string name, string keyword, string Description, bool IsIndependent, string UsersIds, string NoteComponentsIds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            MacroModel model = new MacroModel();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Name", name));
                parameters.Add(new SqlParameter("@Keyword", keyword));
                parameters.Add(new SqlParameter("@Description", Description));
                parameters.Add(new SqlParameter("@IsIndependent", IsIndependent));
                parameters.Add(new SqlParameter("@UsersId", UsersIds));
                parameters.Add(new SqlParameter("@NoteComponentsId", NoteComponentsIds));
                parameters.Add(new SqlParameter("@CreatedBy", MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName)));
                parameters.Add(new SqlParameter("@CreatedOn", MDVUtility.ToStr(DateTime.Now)));
                parameters.Add(new SqlParameter("@ModifiedBy", MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName)));
                parameters.Add(new SqlParameter("@ModifiedOn", MDVUtility.ToStr(DateTime.Now)));
                using (var reader = dbManager.ExecuteReader("[Clinical].[sp_MacroInsert]", parameters))
                {
                    while (reader.Read())
                    {
                        model.MacroId = Convert.ToInt64(reader["MacroId"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMacro::SaveDetails", "[Clinical].[sp_MacroInsert]", ex);
                model.ErrorMessage = ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
            return model;
        }
        public List<MacroModel> SearchDetailsForNotes(long MacroId, string Name, string Keyword, long UserId, long NoteComponentLookupId,string NoteComponentName)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<MacroModel> Models = new List<MacroModel>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (MacroId != 0)
                {
                    parameters.Add(new SqlParameter("@MacroId", MacroId));
                }
                else
                {
                    parameters.Add(new SqlParameter("@MacroId", DBNull.Value));
                }
                if (!string.IsNullOrEmpty(Name))
                { parameters.Add(new SqlParameter("@MacroName", Name)); }
                else
                {
                    parameters.Add(new SqlParameter("@MacroName", DBNull.Value));
                }
                parameters.Add(new SqlParameter("@Keyword", Keyword));
                if (UserId != 0)
                {
                    parameters.Add(new SqlParameter("@UserId", UserId));
                }
                else
                {
                    parameters.Add(new SqlParameter("@UserId", DBNull.Value));
                }
                if (NoteComponentLookupId != 0)
                {
                    parameters.Add(new SqlParameter("@NoteComponentId", NoteComponentLookupId));
                }
                else
                {
                    parameters.Add(new SqlParameter("@NoteComponentId", DBNull.Value));
                }
                if (!string.IsNullOrEmpty(NoteComponentName))
                {
                    parameters.Add(new SqlParameter("@NoteComponentName", NoteComponentName));
                }
                else
                {
                    parameters.Add(new SqlParameter("@NoteComponentName", DBNull.Value));
                }
                using (var reader = dbManager.ExecuteReader("[Clinical].[sp_Macroselect_ForNotes]", parameters))
                {
                    while (reader.Read())
                    {
                        MacroModel model = new MacroModel();
                        model.MacroId = Convert.ToInt64(reader["MacroId"]);
                        model.MacroName = Convert.ToString(reader["Name"]);
                        model.Description = Convert.ToString(reader["Description"]);
                        model.IsIndependent = Convert.ToString(reader["IsIndependent"]);
                        model.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                        model.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                        model.ModifiedBy = Convert.ToString(reader["ModifiedBy"]);
                        model.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                        Models.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALMacro::SearchDetailsForNotes", "[Clinical].[sp_Macroselect_ForNotes]", ex);

            }
            finally
            {
                dbManager.Dispose();
            }
            return Models;
        }
    
    public List<MacroModel> ShowDetails(long Macroid = 0, string MacroName = null, string Keyword = null, string NoteComponentIds = null, string UserIds = null, string DateFrom = null, string DateTo = null)
    {
        IDBManager dbManager = ClientConfiguration.GetDBManager();

        List<MacroModel> macroList = new List<MacroModel>();
        try
        {
            dbManager.Open();
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (Macroid != 0)
            {
                parameters.Add(new SqlParameter("@MacroId", Macroid));
            }
            else
                {
                    parameters.Add(new SqlParameter("@MacroId", DBNull.Value));

                    if (string.IsNullOrEmpty(MacroName))
                        parameters.Add(new SqlParameter("@MacroName", DBNull.Value));
                    else
                        parameters.Add(new SqlParameter("@MacroName", MacroName));

                    if(string.IsNullOrEmpty(Keyword))
                        parameters.Add(new SqlParameter("@Keyword", DBNull.Value));
                    else
                        parameters.Add(new SqlParameter("@Keyword", Keyword));

                    if(string.IsNullOrEmpty(UserIds))
                        parameters.Add(new SqlParameter("@UserIds", DBNull.Value));
                    else
                        parameters.Add(new SqlParameter("@UserIds", UserIds));

                    if(string.IsNullOrEmpty(NoteComponentIds))
                        parameters.Add(new SqlParameter("@NoteComponentIds", DBNull.Value));
                    else
                        parameters.Add(new SqlParameter("@NoteComponentIds", NoteComponentIds));

                    if(string.IsNullOrEmpty(DateFrom))
                        parameters.Add(new SqlParameter("@DateFrom", DBNull.Value));
                    else
                        parameters.Add(new SqlParameter("@DateFrom", DateFrom));

                    if(string.IsNullOrEmpty(DateTo))
                        parameters.Add(new SqlParameter("@DateTo", DBNull.Value));
                    else
                        parameters.Add(new SqlParameter("@DateTo", DateTo));
                }
            using (var reader = dbManager.ExecuteReader("[Clinical].[sp_Macroselect]", parameters))
            {
                while (reader.Read())
                {
                    MacroModel model = new MacroModel();
                    model.MacroId = Convert.ToInt64(reader["MacroId"]);
                    model.MacroName = Convert.ToString(reader["Name"]);
                    model.Keyword = Convert.ToString(reader["Keyword"]);
                    model.Description = Convert.ToString(reader["Description"]);
                    model.NoteComponentsNames = Convert.ToString(reader["NoteComponentsNames"]);
                    model.NoteComponentIds = Convert.ToString(reader["NoteComponentsIds"]);
                    model.UserNames = Convert.ToString(reader["UserNames"]);
                    model.UsersIds = Convert.ToString(reader["UserIds"]);
                    model.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    model.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    model.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                    model.ModifiedBy = Convert.ToString(reader["ModifiedBy"]);
                    macroList.Add(model);
                }
            }
        }
        catch (Exception ex)
        {
            MDVLogger.DALErrorLog("DALMacro::ShowDetails", "[Clinical].[sp_Macroselect]", ex);
            macroList[0].ErrorMessage = ex.Message;
        }
        finally
        {
            dbManager.Dispose();
        }
        return macroList;
    }

    public bool DeleteDetails(long Macroid)
    {
        IDBManager dbManager = ClientConfiguration.GetDBManager();

        List<MacroModel> macroList = new List<MacroModel>();
        try
        {
            dbManager.Open();
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (Macroid != 0)
            {
                parameters.Add(new SqlParameter("@MacroId", Macroid));
            }
            dbManager.ExecuteReader("[Clinical].[sp_MacroDelete]", parameters);
        }
        catch (Exception ex)
        {
            MDVLogger.DALErrorLog("DALMacro::DeleteDetails", "[Clinical].[sp_MacroDelete]", ex);
            macroList[0].ErrorMessage = ex.Message;
            return false;
        }
        finally
        {
            dbManager.Dispose();
        }
        return true;
    }
    public MacroModel UpdateDetails(long id, string name, string keyword, string Description,bool IsIndependent, string UsersIds, string NoteComponentsIds)
    {
        IDBManager dbManager = ClientConfiguration.GetDBManager();
        MacroModel model = new MacroModel();
        try
        {
            dbManager.Open();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@MacroId", id));
            parameters.Add(new SqlParameter("@Name", name));
            parameters.Add(new SqlParameter("@Keyword", keyword));
            parameters.Add(new SqlParameter("@Description", Description));
            parameters.Add(new SqlParameter("@IsIndependent", IsIndependent));
            parameters.Add(new SqlParameter("@ModifiedBy", MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName)));
            parameters.Add(new SqlParameter("@ModifiedOn", MDVUtility.ToStr(DateTime.Now)));
            parameters.Add(new SqlParameter("@UsersId", UsersIds));
            parameters.Add(new SqlParameter("@NoteComponentsId", NoteComponentsIds));


            dbManager.ExecuteReader("[Clinical].[sp_Macroupdate]", parameters);//sp_Macroupdate

        }
        catch (Exception ex)
        {
            MDVLogger.DALErrorLog("DALMacro::UpdateDetails", "[Clinical].[sp_Macroupdate]", ex);
            model.ErrorMessage = ex.Message;
        }
        finally
        {
            dbManager.Dispose();
        }
        return model;
    }
}

}
