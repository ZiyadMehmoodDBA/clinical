/* Author:  Muhammad Arshad
 * Created Date: 04/02/2016
 * OverView: Created for Physical Exam in Clinical Module
 */

using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using System.Data.SqlClient;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALPhysicalExam
    {
        #region Variable
        
        #endregion

        #region Stored Procedure Names
        private const string PROC_PHYSICALEXAM_USERSYSTEM_INSERT = "Clinical.sp_PhysicalExam_UserSystemInsert";
        private const string PROC_PHYSICALEXAM_USERSYSTEM_UPDATE = "Clinical.sp_PhysicalExam_UserSystemUpdate";
        private const string PROC_PHYSICALEXAM_USERSYSTEM_SELECT = "Clinical.sp_PhysicalExam_UserSystemSelect";
        //Start Muhammad Arshad 08-02-2016 System Section Lookups
        private const string PROC_PHYSICALEXAM_SECTION_LOOKUP = "Clinical.sp_PhysicalExamSystemSectionSelect";

        private const string PROC_PHYSICALEXAM_SECTION_FOR_TEMPLATE_LOOKUP = "Clinical.sp_PhysExamSectionSelectForTemplate";
        private const string PROC_PHYSICALEXAM_CHAR_FOR_TEMPLATE_LOOKUP = "Clinical.sp_PhysExamCharSelectForTemplate";
        private const string PROC_PHYSICALEXAM_SUBCHAR_FOR_TEMPLATE_LOOKUP = "Clinical.sp_PhysExamSubCharSelectForTemplate";

        
        //End Muhammad Arshad 08-02-2016 System Section Lookups

        //Start Farooq Ahmad 10-02-2016 System Section Characteristic
        private const string PROC_PHYSICALEXAM_CHARACTERISTIC_LOOKUP = "Clinical.sp_PhysicalExamSystemSectionCharacteristicSelect";
        //End Farooq Ahmad 10-02-2016 System Section Characteristic
        private const string PROC_PATIENT_PHYSICAL_EXAM_DELETE = "Clinical.sp_PatientPhysicalExamDelete";
        private const string PROC_PATIENT_PHYSICAL_EXAM_INSERT = "Clinical.sp_PatientPhysicalExamInsert";
        private const string PROC_PATIENT_PHYSICAL_EXAM_UPDATE = "Clinical.sp_PatientPhysicalExamUpdate";
        private const string PROC_PATIENT_PHYSICAL_EXAM_SELECT = "Clinical.sp_PatientPhysicalExamSelect";

        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_DELETE = "Clinical.sp_PatientPhysicalExamSystemDelete";
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_INSERT = "Clinical.sp_PatientPhysicalExamSystemInsert";
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SELECT = "Clinical.sp_PatientPhysicalExamSystemSelect";
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_UPDATE = "Clinical.sp_PatientPhysicalExamSystemUpdate";


        //Start Farooq Ahmad 10-02-2016 System Section 
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_DELETE = "Clinical.sp_PatientPhysicalExamSystemSectionDelete";
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_INSERT = "Clinical.sp_PatientPhysicalExamSystemSectionInsert";
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_SELECT = "Clinical.sp_PatientPhysicalExamSystemSectionSelect";
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_UPDATE = "Clinical.sp_PatientPhysicalExamSystemSectionUpdate";
        //End Farooq Ahmad 10-02-2016 System Section 

        //Start//12-02-2016//Ahmad Raza// attach/detach procedures parameters created
        private const string PROC_ATTACH_PHYSICALEXAM_WITH_NOTES = "Clinical.sp_AttachPatientPhysicalExamWithNotes";
        private const string PROC_DETACH_PHYSICALEXAM_FROM_NOTES = "Clinical.sp_DetachPatientPhysicalExamFromNotes";
        //End//12-02-2016//Ahmad Raza// attach/detach procedures parameters created

        //Start Humaira Yousaf 11-02-2016 Section Charecteristic Sub Characteristic 
        private const string PROC_PHYSICALEXAM_SUBCHARACTERISTIC_LOOKUP = "Clinical.sp_PhysicalExamSystemSectionCharacteristicSubCharacteristicSelect";
        //End Humaira Yousaf 11-02-2016 Section Charecteristic Sub Characteristic 

        //Start Humaira Yousaf 12-02-2016  
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_INSERT = "Clinical.sp_PatientPhysicalExamSystemSectionCharacteristicInsert";
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_UPDATE = "Clinical.sp_PatientPhysicalExamSystemSectionCharacteristicUpdate";

        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_SUBCHAR_INSERT = "Clinical.sp_PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicInsert";
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_SUBCHAR_UPDATE = "Clinical.sp_PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicUpdate";
        //End Humaira Yousaf 12-02-2016  

        //Start Humaira Yousaf 15-02-2016  
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_SELECT = "Clinical.sp_PatientPhysicalExamSystemSectionCharacteristicSelect";
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_SUBCHAR_SELECT = "Clinical.sp_PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicSelect";
        //End Humaira Yousaf 15-02-2016  

        //Start//15-02-2016//Ahmad Raza//lookup procedures
        private const string PROC_PHYSICALEXAM_SYSTEM_SOAP_INSERT = "Clinical.sp_UpdateSoapTextForPatientPhysicalExamSystem";
        private const string PROC_PHYSICALEXAM_SECTION_SOAP_INSERT = "Clinical.sp_UpdateSoapTextForPatientPhysicalExamSystemSection";
        private const string PROC_PHYSICALEXAM_CHARACTERISTICS_SOAP_INSERT = "Clinical.sp_UpdateSoapTextForPatientPhysicalExamSystemSectionCharacteristic";
        private const string PROC_CHARACTER_LOOKUP = "Clinical.sp_PhysicalExamCharacterSelect";
        private const string PROC_CONTEXT_LOOKUP = "Clinical.sp_PhysicalExamContextSelect";
        private const string PROC_COURSE_LOOKUP = "Clinical.sp_PhysicalExamCourseSelect";
        private const string PROC_FREQUENCY_LOOKUP = "Clinical.sp_PhysicalExamFrequencySelect";
        private const string PROC_RADIATION_LOOKUP = "Clinical.sp_PhysicalExamRadiationSelect";
        private const string PROC_RELIEVED_BY_LOOKUP = "Clinical.sp_PhysicalExamRelievedbySelect";
        private const string PROC_PHYSICALEXAM_SUB_CHARACTERISTICS_SOAP_INSERT = "Clinical.sp_UpdateSoapTextForPhysicalExamSubCharacteristic";
        //End//15-02-2016//Ahmad Raza//lookup procedures
        //Start//16-02-2016//Ahmad Raza//Procedure initialized        
        private const string PROC_PATIENTPHYSICALEXAM_DETAIL_INSERT = "Clinical.sp_PatientPhysicalExamDetailInsert";
        private const string PROC_PATIENTPHYSICALEXAM_DETAIL_Update = "Clinical.sp_PatientPhysicalExamDetailUpdate";
        //End//16-02-2016//Ahmad Raza//Procedure initialized

        //Start Farooq Ahmad 16-02-2016
        private const string PROC_UPDATE_SOAP_TEXT_FOR_PATIENT_PHYSICALEXAM = "[Clinical].[sp_UpdateSoapTextForPatientPhysicalExam]";
        private const string PROC_UPDATE_SOAP_TEXT_FOR_PATIENT_PHYSICALEXAMANDCHILD = "[Clinical].[sp_UpdateSoapTextForPatientPhysicalExamAndChild]";

        //End Farooq Ahmad 16-02-2016

        //Start Humaira Yousaf 17-02-2016
        private const string PROC_PATIENT_PHYSICAL_EXAM_DETAIL_SELECT = "Clinical.sp_PatientPhysicalExamDetailSelect";
        //End Humaira Yousaf 17-02-2016

        //Start Humaira Yousaf 18-02-2016
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_DELETE = "Clinical.sp_PatientPhysicalExamSystemSectionCharacteristicDelete";
        private const string PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_SUBCHAR_DELETE = "Clinical.sp_PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicDelete";
        private const string PROC_PATIENT_PHYSICAL_EXAM_DETAIL_DELETE = "Clinical.sp_PatientPhysicalExamDetailDelete";
        //End Humaira Yousaf 18-02-2016

        //Start 26-02-2016 Muhammad Arshad Physical Exam Lookup
        private const string PROC_PHYSICAL_EXAM_SYSTEM_SELECT = "Clinical.sp_PhysicalExamSystemSelect";
        private const string PROC_PHYSICAL_EXAM_SELECT_FOR_SOAP_TEXT = "Clinical.";


        private const string PROC_NOTES_OBSERVATION_SELECT = "[Clinical].[sp_NotesObservationSelect]";
        //End 26-02-2016 Muhammad Arshad Physical Exam Lookup


        #endregion

        #region Parameters

        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PATIENT_PHYSICAL_EXAM_ID = "@PatientPhysicalExamId";
        private const string PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID = "@PatientPhysicalExamSystemId";
        private const string PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_ID = "@PatientPhysicalExamSystemSectionId";
        private const string PARM_PATIENT_PHYSICAL_EXAM_DATE = "@PatientPhysicalExamDate";

        private const string PARM_BNORMAL_EXAM = "@bNormalExam";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_SOAP_TEXT = "@SoapText";
        private const string PARM_SYSTEM_ID = "@SystemId";
        private const string PARM_SECTION_ID = "@SectionId";
        private const string PARM_USER_SYSTEM_ID = "@UserSystemId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_SYSTEM_ORDER = "@SystemOrder";
        private const string PARM_SORT_ORDER = "@SortOrder";
        private const string PARM_IS_NORMAL = "@IsNormal";
        private const string PARM_NORMAL_COMMENTS = "@NormalComments";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_CHARACTERISTIC_ID = "@CharacteristicId";

        //Start//12-02-2016//Ahmad Raza// notesid parameter created
        private const string PARM_NOTE_ID = "@NoteId";
        //End//12-02-2016//Ahmad Raza// notesid parameter created

        //Start//16-02-2016//Ahmad Raza//creating Parameters 
        private const string PARM_DETAIL_ID = "@DetailId";
        private const string PARM_PARENT_TYPE = "@ParentType";
        private const string PARM_PARENT_ID = "@ParentId";
        private const string PARM_PREVIOUS_HISTORY = "@PrevHistory";
        private const string PARM_STATUS_ID = "@StatusId";
        private const string PARM_ONSET = "@Onset";
        private const string PARM_DURATION_LENGTH = "@DurationLength";
        private const string PARM_DURATION_PERIOD_ID = "@DurationPeriodId";
        private const string PARM_PATTERN_ID = "@PatternId";
        private const string PARM_SEVERITY_ID = "@SeverityId";
        private const string PARM_COURSE_ID = "@CourseId";
        private const string PARM_RADIATION_ID = "@RadiationId";
        private const string PARM_FREQUENCY_ID = "@FrequencyId";
        private const string PARM_CONTEXT_ID = "@ContextId";
        private const string PARM_CHARACTER_ID = "@CharacterId";
        private const string PARM_AGGRAVATED_BY_ID = "@AggravatedById";
        private const string PARM_RELIEVED_BY_ID = "@RelievedbyId";
        private const string PARM_LOCATION = "@Location";
        private const string PARM_PRECIPITATED_BY = "@Precipitatedby";
        private const string PARM_ASSOCIATED_WITH = "@AssociatedWith";

        //End//16-02-2016//Ahmad Raza//Creating Parameters


        private const string PARM_SYSTEMSECTION_CHAR_ID = "@PatientPhysicalExamSystemSectionCharacteristicId";
        private const string PARM_SYSTEMSECTION_ID = "@PatientPhysicalExamSystemSectionId";
        private const string PARM_ISPOSITIVE = "@IsPositive";

        private const string PARM_SYSTEMSECTION_SUBCHAR_ID = "@PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicId";
        private const string PARM_SUBCHAR_ID = "@SubCharacteristicId";

        //start 07-03-2015 Muhammad Arshad TemplateId Parameter
        private const string PARM_TEMPLATE_ID = "@TemplateId";
        //End 07-03-2015 Muhammad Arshad TemplateId Parameter
        #endregion

        #region Constructors
        public DALPhysicalExam()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }

        public DALPhysicalExam(SharedVariable SharedVariable)
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

        #region Support Functions PhysicalExam_System

        //Author: Farooq Ahmad
        //Date: 08/02/2016
        //This function create the create parameter of the PhysicalExam_UserSystem table
        private void CreatePhysicalExamSystemInsertUpdateParameters(IDBManager dbManager, DSPhysicalExam ds, bool isInsert = true)
        {
           //Start 30-03-2016 Humaira Yousaf to resolve output paramter issue
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_USER_SYSTEM_ID, ds.PhysicalExam_UserSystem.UserSystemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else 
            {
                dbManager.CreateUpdateParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_USER_SYSTEM_ID, ds.PhysicalExam_UserSystem.UserSystemIdColumn.ColumnName, DbType.Int64);
            }
            //End 30-03-2016 Humaira Yousaf to resolve output paramter issue
            
            dbManager.AddInsertUpdateParameters(1, PARM_USER_ID, ds.PhysicalExam_UserSystem.UserIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(2, PARM_SYSTEM_ORDER, ds.PhysicalExam_UserSystem.SystemOrderColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(3, PARM_SORT_ORDER, ds.PhysicalExam_UserSystem.SortOrderColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(4, PARM_IS_ACTIVE, ds.PhysicalExam_UserSystem.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.PhysicalExam_UserSystem.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.PhysicalExam_UserSystem.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.PhysicalExam_UserSystem.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.PhysicalExam_UserSystem.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_TEMPLATE_ID, ds.PhysicalExam_UserSystem.TemplateIdColumn.ColumnName, DbType.Int64);
        }

        #endregion

        #region Physical Exam

        // Author: Farooq Ahmad
        // Date: 08/02/2016
        //This function will insert/update Physical Exam User System
        public DSPhysicalExam insertUpdatePhysicalExamUserSystem(DSPhysicalExam ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreatePhysicalExamSystemInsertUpdateParameters(dbManager, ds, true);
                this.CreatePhysicalExamSystemInsertUpdateParameters(dbManager, ds, false);

                ds = (DSPhysicalExam)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_USERSYSTEM_INSERT, PROC_PHYSICALEXAM_USERSYSTEM_UPDATE, ds, ds.PhysicalExam_UserSystem.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::PROC_PHYSICALEXAM_System_INSERT", PROC_PHYSICALEXAM_USERSYSTEM_INSERT + " " + PROC_PHYSICALEXAM_USERSYSTEM_UPDATE, ex);
                throw ex;
            }
        }

        // Author: Farooq Ahmad
        // Date: 08/02/2016
        //This function will load Physical Exam User System
        public DSPhysicalExam loadPhysicalExamUserSystem(long systemId, long templateId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(3);

                if (systemId == 0)
                    dbManager.AddParameters(0, PARM_SYSTEM_ID, null);
                else 
                    dbManager.AddParameters(0, PARM_SYSTEM_ID, systemId);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (templateId <= 1)
                    dbManager.AddParameters(2, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_TEMPLATE_ID, templateId);
                
               // ds.EnforceConstraints = false;
              
                
                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_USERSYSTEM_SELECT, ds, ds.PhysicalExam_UserSystem.TableName);
               
              //  ds.EnforceConstraints = true;
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::PhysicalExam_System", PROC_PHYSICALEXAM_USERSYSTEM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region PhysicalExam System Section

        /* Author:  Muhammad Arshad
         * Created Date: 08/02/2016
         * OverView: Created for Physical Exam Section Lookup
        */
        public DSPhysicalExamLookup LookupPhysicalExamSection(Int64 systemId, Int64 templateId)
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SYSTEM_ID, systemId);
                if (templateId <= 1)
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, templateId);
                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_SECTION_LOOKUP, ds, ds.PhysicalExamSystemSection.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::LookupPhysicalExamSection", PROC_PHYSICALEXAM_SECTION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamLookup LookupPhysicalExamSectionForTemplate(Int64 systemId, Int64 templateId)
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SYSTEM_ID, systemId);
                if (templateId <= 1)
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, templateId);
                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_SECTION_FOR_TEMPLATE_LOOKUP, ds, ds.PhysicalExamSystemSection.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::LookupPhysicalExamSectionForTemplate", PROC_PHYSICALEXAM_SECTION_FOR_TEMPLATE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region PhysicalExam System Charcteristic

        /* Author:  Farooq Ahmad
         * Created Date: 10/02/2016
         * OverView: Created for Physical Exam Charcteristic Lookup 
        */
        public DSPhysicalExamLookup LookupPhysicalExamCharcteristic(Int64 sectionId, Int64 templateId)
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SECTION_ID, sectionId);
                if (templateId <= 1)
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, templateId);
                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_CHARACTERISTIC_LOOKUP, ds, ds.PhysicalExamSystemSectionCharacteristic.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::LookupPhysicalExamCharcteristic", PROC_PHYSICALEXAM_CHARACTERISTIC_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamLookup LookupPhysicalExamCharForTemplate(Int64 sectionId, Int64 templateId)
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SECTION_ID, sectionId);
                if (templateId <= 1)
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, templateId);
                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_CHAR_FOR_TEMPLATE_LOOKUP, ds, ds.PhysicalExamSystemSectionCharacteristic.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::LookupPhysicalExamCharForTemplate", PROC_PHYSICALEXAM_CHAR_FOR_TEMPLATE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region PhysicalExam System Characteristics SubCharacteristics
        /// <summary>
        /// Module Name: LookupPhysicalExamSubCharcteristic
        /// Author: Humaira Yousaf
        /// Created Date: 11-02-2016
        /// Description: Gets sub characteristics against characteristic id for physical exam
        /// </summary>
        /// <param name="CharacteristicId" type="Int64">Gets specific sub characteristic</param>        
        public DSPhysicalExamLookup LookupPhysicalExamSubCharcteristic(Int64 characteristicId, Int64 templateId)
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CHARACTERISTIC_ID, characteristicId);
                if (templateId <= 1)
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, templateId);
                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_SUBCHARACTERISTIC_LOOKUP, ds, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::LookupPhysicalExamSubCharcteristic", PROC_PHYSICALEXAM_SUBCHARACTERISTIC_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamLookup LookupPhysicalExamSubCharForTemplate(Int64 characteristicId, Int64 templateId)
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CHARACTERISTIC_ID, characteristicId);
                if (templateId <= 1)
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, templateId);
                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_SUBCHAR_FOR_TEMPLATE_LOOKUP, ds, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::LookupPhysicalExamSubCharForTemplate", PROC_PHYSICALEXAM_SUBCHAR_FOR_TEMPLATE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Support Functions"

        // Author:  Ahmad Raza
        // Created Date: 11-02-2016
        //OverView: Creates Physical Exam parameters.
        private void createPatientPhysicalExamParameters(IDBManager dbManager, DSPhysicalExam ds, Boolean isInsert, string noramlExamsDetail = null)
        {
            dbManager.CreateParameters(14);

            if (isInsert == true)
                dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, ds.PatientPhysicalExam.PatientPhysicalExamIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, ds.PatientPhysicalExam.PatientPhysicalExamIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.PatientPhysicalExam.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_PHYSICAL_EXAM_DATE, ds.PatientPhysicalExam.PatientPhysicalExamDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_BNORMAL_EXAM, ds.PatientPhysicalExam.bNormalExamColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_COMMENTS, ds.PatientPhysicalExam.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.PatientPhysicalExam.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.PatientPhysicalExam.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.PatientPhysicalExam.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.PatientPhysicalExam.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.PatientPhysicalExam.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_SOAP_TEXT, ds.PatientPhysicalExam.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_NORMAL_COMMENTS, noramlExamsDetail);
            dbManager.AddParameters(12, PARM_NOTE_ID, ds.PatientPhysicalExam.NoteIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(13, PARM_TEMPLATE_ID, ds.PatientPhysicalExam.TemplateIdColumn.ColumnName, DbType.Int64);
        }

        // Author:  Ahmad Raza
        // Created Date: 11-02-2016
        //OverView: Creates Patient Physical Exam parameters.
        private void createPatientPhysicalExamSystemParameters(IDBManager dbManager, DSPhysicalExam ds, Boolean isInsert, string noramlExamsDetail = null)
        {
            dbManager.CreateParameters(12);

            if (isInsert == true)
                dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID, ds.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID, ds.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SYSTEM_ID, ds.PatientPhysicalExamSystem.SystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_PATIENT_PHYSICAL_EXAM_ID, ds.PatientPhysicalExamSystem.PatientPhysicalExamIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_IS_NORMAL, ds.PatientPhysicalExamSystem.IsNormalColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_COMMENTS, ds.PatientPhysicalExamSystem.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.PatientPhysicalExamSystem.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.PatientPhysicalExamSystem.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.PatientPhysicalExamSystem.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.PatientPhysicalExamSystem.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_ON, ds.PatientPhysicalExamSystem.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_SOAP_TEXT, ds.PatientPhysicalExamSystem.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_NORMAL_COMMENTS, ds.PatientPhysicalExamSystem.NormalCommentsColumn.ColumnName, DbType.String);

        }

        // Author:  Ahmad Raza
        // Created Date: 11-02-2016
        //OverView: Creates Patient Physical Exam System Section parameters.
        private void createPatientPhysicalExamSystemSectionParameters(IDBManager dbManager, DSPhysicalExam ds, Boolean isInsert, string noramlExamsDetail = null)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_ID, ds.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_ID, ds.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemSectionIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID, ds.PatientPhysicalExamSystemSection.PatientPhysicalExamSystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SECTION_ID, ds.PatientPhysicalExamSystemSection.SectionIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(3, PARM_COMMENTS, ds.PatientPhysicalExamSystemSection.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_ACTIVE, ds.PatientPhysicalExamSystemSection.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.PatientPhysicalExamSystemSection.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.PatientPhysicalExamSystemSection.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.PatientPhysicalExamSystemSection.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.PatientPhysicalExamSystemSection.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_SOAP_TEXT, ds.PatientPhysicalExamSystemSection.SoapTextColumn.ColumnName, DbType.String);
        }

        // Author:  Ahmad Raza
        // Created Date: 16-02-2016
        //OverView: method to add parameters
        private void createPatientPhysicalExamDetailParameters(IDBManager dbManager, DSPhysicalExam ds, Boolean isInsert, string parentType = null)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(28);
                dbManager.AddInsertUpdateParameters(0, PARM_DETAIL_ID, ds.PatientPhysicalExamDetail.DetailIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(28);
                dbManager.AddInsertUpdateParameters(0, PARM_DETAIL_ID, ds.PatientPhysicalExamDetail.DetailIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_PHYSICAL_EXAM_ID, ds.PatientPhysicalExamDetail.PatientPhysicalExamIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PARENT_TYPE, ds.PatientPhysicalExamDetail.ParentTypeColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(3, PARM_PARENT_ID, ds.PatientPhysicalExamDetail.ParentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_PREVIOUS_HISTORY, ds.PatientPhysicalExamDetail.PrevHistoryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_STATUS_ID, ds.PatientPhysicalExamDetail.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(6, PARM_ONSET, ds.PatientPhysicalExamDetail.OnsetColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_DURATION_LENGTH, ds.PatientPhysicalExamDetail.DurationLengthColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(8, PARM_DURATION_PERIOD_ID, ds.PatientPhysicalExamDetail.DurationPeriodIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(9, PARM_PATTERN_ID, ds.PatientPhysicalExamDetail.PatternIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(10, PARM_SEVERITY_ID, ds.PatientPhysicalExamDetail.SeverityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(11, PARM_COURSE_ID, ds.PatientPhysicalExamDetail.CourseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(12, PARM_RADIATION_ID, ds.PatientPhysicalExamDetail.RadiationIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(13, PARM_FREQUENCY_ID, ds.PatientPhysicalExamDetail.FrequencyIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(14, PARM_CONTEXT_ID, ds.PatientPhysicalExamDetail.ContextIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(15, PARM_CHARACTER_ID, ds.PatientPhysicalExamDetail.CharacterIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(16, PARM_AGGRAVATED_BY_ID, ds.PatientPhysicalExamDetail.AggravatedByIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(17, PARM_RELIEVED_BY_ID, ds.PatientPhysicalExamDetail.RelievedbyIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(18, PARM_LOCATION, ds.PatientPhysicalExamDetail.LocationColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_PRECIPITATED_BY, ds.PatientPhysicalExamDetail.PrecipitatedbyColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_ASSOCIATED_WITH, ds.PatientPhysicalExamDetail.AssociatedWithColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(21, PARM_COMMENTS, ds.PatientPhysicalExamDetail.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(22, PARM_IS_ACTIVE, ds.PatientPhysicalExamDetail.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(23, PARM_CREATED_BY, ds.PatientPhysicalExamDetail.CreatedByColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(24, PARM_CREATED_ON, ds.PatientPhysicalExamDetail.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(25, PARM_MODIFIED_BY, ds.PatientPhysicalExamDetail.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(26, PARM_MODIFIED_ON, ds.PatientPhysicalExamDetail.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(27, PARM_SOAP_TEXT, ds.PatientPhysicalExamDetail.SoapTextColumn.ColumnName, DbType.String);


        }

        #endregion

        #region "Insert, delete, update and get patient physical exam using dataset Functions"

        // Author:  Ahmad Raza
        // Created Date: 16-02-2016
        //OverView: Loads Physical Exam.
        public DSPhysicalExam loadPatientPhysicalExam(long patientId, long patientPhysicalExamId, long noteId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);

                if (patientPhysicalExamId <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, patientPhysicalExamId);

                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);
                if (noteId == 0)
                    dbManager.AddParameters(2, PARM_NOTE_ID, null);
                else
                    dbManager.AddParameters(2, PARM_NOTE_ID, noteId);

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SELECT, ds, ds.PatientPhysicalExam.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::LoadPhysicalExam", PROC_PATIENT_PHYSICAL_EXAM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Farooq Ahmad
        // Created Date: 16-02-2016
        //OverView: insert patient physical exam
        public DSPhysicalExam insertPatientPhysicalExam(DSPhysicalExam ds, string noramlExamsDetail)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createPatientPhysicalExamParameters(dbManager, ds, true, noramlExamsDetail);
                ds = (DSPhysicalExam)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_INSERT, ds, ds.PatientPhysicalExam.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsertPhysicalExam::InsertPhysicalExam", PROC_PATIENT_PHYSICAL_EXAM_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Ahmad Raza
        // Created Date: 16-02-2016
        //OverView: update patient physical exam
        public DSPhysicalExam updatePatientPhysicalExam(DSPhysicalExam ds, string noramlExamsDetail)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createPatientPhysicalExamParameters(dbManager, ds, false, noramlExamsDetail);
                ds = (DSPhysicalExam)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_UPDATE, ds, ds.PatientPhysicalExam.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::UpdatePhysicalExam", PROC_PATIENT_PHYSICAL_EXAM_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Humaira Yousaf 
        // Created Date: 16-02-2016
        //OverView: delete patient physical exam
        public string DeletePatientPhysicalExam(long patientPhysicalExamId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, patientPhysicalExamId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::DeletePatientPhysicalExam", PROC_PATIENT_PHYSICAL_EXAM_DELETE, ex);
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

        #region "Insert, delete, update and get patient physical exam system using dataset Functions"

        // Author:  Humaira Yousaf 
        // Created Date: 16-02-2016
        //OverView: load patient physical exam system
        public DSPhysicalExam loadPatientPhysicalExamSystem(long patientPhysicalExamId, long patientPhysicalExamSystemId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (patientPhysicalExamSystemId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID, patientPhysicalExamSystemId);

                if (patientPhysicalExamId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_PHYSICAL_EXAM_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_PHYSICAL_EXAM_ID, patientPhysicalExamId);

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SELECT, ds, ds.PatientPhysicalExamSystem.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::LoadPhysicalExamSystem", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Abid Ali
        // Created Date: 16-02-2016
        //OverView: insert patient physical exam system
        public DSPhysicalExam insertPatientPhysicalExamSystem(DSPhysicalExam ds, string noramlExamsDetail)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createPatientPhysicalExamSystemParameters(dbManager, ds, true, noramlExamsDetail);
                ds = (DSPhysicalExam)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_INSERT, ds, ds.PatientPhysicalExamSystem.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsertPhysicalExamSystem::InsertPhysicalExamSystem", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Abid Ali
        // Created Date: 16-02-2016
        //OverView: update patient physical exam system
        public DSPhysicalExam updatePatientPhysicalExamSystem(DSPhysicalExam ds, string noramlExamsDetail)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createPatientPhysicalExamSystemParameters(dbManager, ds, false, noramlExamsDetail);
                ds = (DSPhysicalExam)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_UPDATE, ds, ds.PatientPhysicalExamSystem.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::UpdatePhysicalExamSystem", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author:  Farooq Ahmad
        // Created Date: 16-02-2016
        //OverView: delete patient physical examsystem
        public string DeletePatientPhysicalExamSystem(long patientPhysicalExamSystemId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID, patientPhysicalExamSystemId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePatientPhysicalExamSystem", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_DELETE, ex);
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

        /// <summary>
        /// Module Name: InsertUpdatePatientPhysicalExamSystem
        /// Author: Humaira Yousaf
        /// Created Date: 10-02-2016
        /// Description: Inserts and Updates Patient Physical Exam System
        /// </summary>
        /// <param name="ds" type="DSPhysicalExam">contains data to insert/update</param>                
        public DSPhysicalExam InsertUpdatePatientPhysicalExamSystem(DSPhysicalExam ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreatePatientPhysicalExamSystemInsertParameters(dbManager, ds);
                CreatePatientPhysicalExamSystemUpdateParameters(dbManager, ds);

                ds = (DSPhysicalExam)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_INSERT, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_UPDATE, ds, ds.PatientPhysicalExamSystem.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DSPhysicalExam::markSystemsAsNormal", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_INSERT + " " + PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_UPDATE, ex);
                throw ex;
            }
        }

        // Author: Humaira Yousaf
        // Created Date: 10-02-2016
        //OverView: create patient physical exam system insert parameters
        private void CreatePatientPhysicalExamSystemInsertParameters(IDBManager dbManager, DSPhysicalExam ds)
        {
            dbManager.CreateInsertParameters(12);

            dbManager.AddInsertUpdateParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID, ds.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_PHYSICAL_EXAM_ID, ds.PatientPhysicalExamSystem.PatientPhysicalExamIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SYSTEM_ID, ds.PatientPhysicalExamSystem.SystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_IS_NORMAL, ds.PatientPhysicalExamSystem.IsNormalColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(4, PARM_NORMAL_COMMENTS, ds.PatientPhysicalExamSystem.NormalCommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_COMMENTS, ds.PatientPhysicalExamSystem.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.PatientPhysicalExamSystem.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.PatientPhysicalExamSystem.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.PatientPhysicalExamSystem.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.PatientPhysicalExamSystem.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.PatientPhysicalExamSystem.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_SOAP_TEXT, ds.PatientPhysicalExamSystem.SoapTextColumn.ColumnName, DbType.String);

        }

        // Author: Humaira Yousaf
        // Created Date: 10-02-2016
        //OverView: create patient physical exam system update parameters
        private void CreatePatientPhysicalExamSystemUpdateParameters(IDBManager dbManager, DSPhysicalExam ds)
        {
            dbManager.CreateUpdateParameters(12);

            dbManager.AddInsertUpdateParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID, ds.PatientPhysicalExamSystem.PatientPhysicalExamSystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_PATIENT_PHYSICAL_EXAM_ID, ds.PatientPhysicalExamSystem.PatientPhysicalExamIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SYSTEM_ID, ds.PatientPhysicalExamSystem.SystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_IS_NORMAL, ds.PatientPhysicalExamSystem.IsNormalColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(4, PARM_NORMAL_COMMENTS, ds.PatientPhysicalExamSystem.NormalCommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_COMMENTS, ds.PatientPhysicalExamSystem.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.PatientPhysicalExamSystem.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.PatientPhysicalExamSystem.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.PatientPhysicalExamSystem.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.PatientPhysicalExamSystem.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.PatientPhysicalExamSystem.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(11, PARM_SOAP_TEXT, ds.PatientPhysicalExamSystem.SoapTextColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Insert, delete, update and get patient physical exam system section using dataset Functions"

        // Author: Farooq Ahmad
        // Created Date: 10/02/2016
        //OverView: load patient physical exam system section
        public DSPhysicalExam loadPatientPhysicalExamSystemSection(long patientPhysicalExamSystemId, long patientPhysicalExamSystemSectionId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (patientPhysicalExamSystemId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID, patientPhysicalExamSystemId);

                if (patientPhysicalExamSystemSectionId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_ID, patientPhysicalExamSystemSectionId);

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_SELECT, ds, ds.PatientPhysicalExamSystemSection.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::loadPatientPhysicalExamSystemSection", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Farooq Ahmad
        // Created Date: 10/02/2016
        //OverView: insert update patient physical exam system section
        public DSPhysicalExam insertUpdatePatientPhysicalExamSystemSection(DSPhysicalExam ds, string noramlExamsDetail)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createPatientPhysicalExamSystemSectionParameters(dbManager, ds, true, noramlExamsDetail);
                createPatientPhysicalExamSystemSectionParameters(dbManager, ds, false, noramlExamsDetail);
                ds = (DSPhysicalExam)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_INSERT, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_UPDATE, ds, ds.PatientPhysicalExamSystemSection.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALInsertPhysicalExamSystem::InsertPhysicalExamSystemSection", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Farooq Ahmad
        // Created Date: 10/02/2016
        //OverView: delete patient physical exam system section
        public string DeletePatientPhysicalExamSystemSection(long patientPhysicalExamSystemSectionId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_ID, patientPhysicalExamSystemSectionId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePatientPhysicalExamSystem", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_DELETE, ex);
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


        #region "Insert, delete, update and get patient physical exam system section characteristic using dataset Functions"

        /// <summary>
        /// Module Name: InsertUpdatePatientPhysicalExamSystemSectionCharacteristic
        /// Author: Humaira Yousaf
        /// Created Date: 12-02-2016
        /// Description: Inserts and Updates Patient Physical Exam System Section Characteristic
        /// </summary>
        /// <param name="ds" type="DSPhysicalExam">contains data to insert/update</param>         
        public DSPhysicalExam InsertUpdatePatientPhysicalExamSystemSectionCharacteristic(DSPhysicalExam ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreatePatientPhysicalExamSystemSectionCharacteristicParameters(dbManager, ds, true);
                CreatePatientPhysicalExamSystemSectionCharacteristicParameters(dbManager, ds, false);

                ds = (DSPhysicalExam)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_INSERT, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_UPDATE, ds, ds.PatientPhysicalExamSystemSectionCharacteristic.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DSPhysicalExam::InsertUpdatePatientPhysicalExamSystemSectionCharacteristic", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_INSERT + " " + PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_UPDATE, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Module Name: CreatePatientPhysicalExamSystemSectionCharacteristicParameters
        /// Author: Humaira Yousaf
        /// Created Date: 12-02-2016
        /// Description: Creates parameters to insert/update Characteristics
        /// </summary>
        /// <param name="dbManager" type="IDBManager">database manager</param>  
        /// <param name="ds" type="DSPhysicalExam">contains data to insert/update</param>  
        /// <param name="isInsertParameter" type="bool">decides paramters creation type</param>          
        private void CreatePatientPhysicalExamSystemSectionCharacteristicParameters(IDBManager dbManager, DSPhysicalExam ds, bool isInsertParameter)
        {
            if (isInsertParameter == true)
            {
                dbManager.CreateInsertParameters(11);
                dbManager.AddInsertUpdateParameters(0, PARM_SYSTEMSECTION_CHAR_ID, ds.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(11);
                dbManager.AddInsertUpdateParameters(0, PARM_SYSTEMSECTION_CHAR_ID, ds.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_SYSTEMSECTION_ID, ds.PatientPhysicalExamSystemSectionCharacteristic.PatientPhysicalExamSystemSectionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CHARACTERISTIC_ID, ds.PatientPhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_ISPOSITIVE, ds.PatientPhysicalExamSystemSectionCharacteristic.IsPositiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.PatientPhysicalExamSystemSectionCharacteristic.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.PatientPhysicalExamSystemSectionCharacteristic.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.PatientPhysicalExamSystemSectionCharacteristic.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.PatientPhysicalExamSystemSectionCharacteristic.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.PatientPhysicalExamSystemSectionCharacteristic.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.PatientPhysicalExamSystemSectionCharacteristic.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAP_TEXT, ds.PatientPhysicalExamSystemSectionCharacteristic.SoapTextColumn.ColumnName, DbType.String);

        }
        /// <summary>
        /// Module Name: LoadPatientPhysicalExamSystemSectionCharacteristic
        /// Author: Humaira Yousaf
        /// Created Date: 15-02-2016
        /// Description: Loads Section Characteristics
        /// </summary>
        /// <param name="systemSectionId" type="long">section id</param>  
        /// <param name="characteristicId" type="long">characteristic Id</param>          
        public DSPhysicalExam LoadPatientPhysicalExamSystemSectionCharacteristic(long systemSectionId, long characteristicId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (systemSectionId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_ID, systemSectionId);

                if (characteristicId == 0)
                    dbManager.AddParameters(1, PARM_SYSTEMSECTION_CHAR_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SYSTEMSECTION_CHAR_ID, characteristicId);

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_SELECT, ds, ds.PatientPhysicalExamSystemSectionCharacteristic.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::loadPatientPhysicalExamSystemSectionCharacteristic", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //End Humaira Yousaf 12-02-2016  

        /// <summary>
        /// Module Name: DeletePatientPhysicalExamSystemSectionCharacteristic
        /// Author: Humaira Yousaf
        /// Created Date: 18-02-2016
        /// Description: Deletes Patient Physical Exam System Section Characteristic
        /// </summary>
        /// <param name="PatientPhysicalExamSystemSectionCharId" type="long">characteristic id to delete</param>          
        public string DeletePatientPhysicalExamSystemSectionCharacteristic(long patientPhysicalExamSystemSectionCharId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SYSTEMSECTION_CHAR_ID, patientPhysicalExamSystemSectionCharId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePatientPhysicalExamSystemSectionCharacteristic", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_DELETE, ex);
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

        #region "Insert, delete, update and get patient physical exam system section characteristic sub characteristic using dataset Functions"
        /// <summary>
        /// Module Name: InsertUpdatePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic
        /// Author: Humaira Yousaf
        /// Created Date: 18-02-2016
        /// Description: Insert/update Patient Physical Exam Sub Characteristic
        /// </summary>
        /// <param name="ds" type="DSPhysicalExam">contains data to insert/update</param>    
        public DSPhysicalExam InsertUpdatePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(DSPhysicalExam ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreatePatientPhysicalExamSystemSectionCharacteristicSubCharacteristicParameters(dbManager, ds, true);
                CreatePatientPhysicalExamSystemSectionCharacteristicSubCharacteristicParameters(dbManager, ds, false);

                ds = (DSPhysicalExam)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_SUBCHAR_INSERT, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_SUBCHAR_UPDATE, ds, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DSPhysicalExam::InsertUpdatePatientPhysicalExamSystemSectionCharacteristic", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_SUBCHAR_INSERT + " " + PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_SUBCHAR_UPDATE, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Module Name: CreatePatientPhysicalExamSystemSectionCharacteristicSubCharacteristicParameters
        /// Author: Humaira Yousaf
        /// Created Date: 18-02-2016
        /// Description: Create SubCharacteristic Parameters
        /// </summary>
        /// <param name="dbManager" type="IDBManager">database manager</param> 
        /// <param name="ds" type="DSPhysicalExam">dataset contains data to insert/update</param> 
        /// <param name="isInsertParameter" type="bool">decides parameter type</param>         
        private void CreatePatientPhysicalExamSystemSectionCharacteristicSubCharacteristicParameters(IDBManager dbManager, DSPhysicalExam ds, bool isInsertParameter)
        {

            if (isInsertParameter == true)
            {
                dbManager.CreateInsertParameters(11);
                dbManager.AddInsertUpdateParameters(0, PARM_SYSTEMSECTION_SUBCHAR_ID, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(11);
                dbManager.AddInsertUpdateParameters(0, PARM_SYSTEMSECTION_SUBCHAR_ID, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristicIdColumn.ColumnName, DbType.Int64);

            }
            dbManager.AddInsertUpdateParameters(1, PARM_SYSTEMSECTION_CHAR_ID, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.PatientPhysicalExamSystemSectionCharacteristicIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SUBCHAR_ID, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_ISPOSITIVE, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.IsPositiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_SOAP_TEXT, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.SoapTextColumn.ColumnName, DbType.String);

        }

        /// <summary>
        /// Module Name: LoadPatientPhysicalExamSystemSectionCharacteristicSubCharacteristic
        /// Author: Humaira Yousaf
        /// Created Date: 15-02-2016
        /// Description: Loads Section Characteristics Sub Characteristics
        /// </summary>
        /// <param name="characteristicId" type="long">characteristic Id</param> 
        /// <param name="subCharacteristicId" type="long">subCharacteristic Id</param>          
        public DSPhysicalExam LoadPatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(long characteristicId, long subCharacteristicId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (characteristicId == 0)
                    dbManager.AddParameters(0, PARM_SYSTEMSECTION_CHAR_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SYSTEMSECTION_CHAR_ID, characteristicId);

                if (subCharacteristicId == 0)
                    dbManager.AddParameters(1, PARM_SYSTEMSECTION_SUBCHAR_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SYSTEMSECTION_SUBCHAR_ID, subCharacteristicId);

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_SUBCHAR_SELECT, ds, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::loadPatientPhysicalExamSystemSectionSubCharacteristic", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_SUBCHAR_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Module Name: DeletePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic
        /// Author: Humaira Yousaf
        /// Created Date: 18-02-2016
        /// Description: Delete Patient Physical Exam System Section Characteristic Sub Characteristic
        /// </summary>
        /// <param name="PatientPhysicalExamSystemSectionCharSubCharId" type="long">subcharacteristic Id to delete</param>        
        public string DeletePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic(long patientPhysicalExamSystemSectionCharSubCharId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SYSTEMSECTION_SUBCHAR_ID, patientPhysicalExamSystemSectionCharSubCharId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_SUBCHAR_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePatientPhysicalExamSystemSectionCharacteristicSubCharacteristic", PROC_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_CHAR_SUBCHAR_DELETE, ex);
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

        //Start 17-02-2016 Humaira Yousaf to load patient physical exam detail
        #region Physical Exam Detail
        /// <summary>
        /// Module Name: LoadPatientPhysicalExamDetail
        /// Author: Humaira Yousaf
        /// Created Date: 17-02-2016
        /// Description: Loads patient physical exam detail
        /// </summary>
        /// <param name="patientPhysicalExamId" type="long">patient Physical Exam Id</param> 
        /// <param name="parentId" type="long">parent characteristic/ subCharacteristic Id</param> 
        /// <param name="detailId" type="long">detailId to load</param>         
        public DSPhysicalExam LoadPatientPhysicalExamDetail(long patientPhysicalExamId, long parentId, string parentType, long detailId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (patientPhysicalExamId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, patientPhysicalExamId);

                if (parentId == 0)
                    dbManager.AddParameters(1, PARM_PARENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PARENT_ID, parentId);

                dbManager.AddParameters(2, PARM_PARENT_TYPE, parentType);

                if (detailId == 0)
                    dbManager.AddParameters(3, PARM_DETAIL_ID, null);
                else
                    dbManager.AddParameters(3, PARM_DETAIL_ID, detailId);

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_DETAIL_SELECT, ds, ds.PatientPhysicalExamDetail.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::LoadPatientPhysicalExamDetail", PROC_PATIENT_PHYSICAL_EXAM_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Module Name: DeletePatientPhysicalExamDetail
        /// Author: Humaira Yousaf
        /// Created Date: 18-02-2016
        /// Description: Deletes Patient Physical Exam Detail
        /// </summary>
        /// <param name="patientPhysicalExamId" type="long">patient Physical Exam Id</param> 
        /// <param name="parentType" type="string">characteristic/subCharacteristic</param>         
        /// <param name="parentId" type="long">parent characteristic/subCharacteristic Id</param> 
        /// <param name="patientPhysicalExamDetailId" type="long">detail Id to delete</param>           
        public string DeletePatientPhysicalExamDetail(long patientPhysicalExamId, string parentType, long parentId, long patientPhysicalExamDetailId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (patientPhysicalExamId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, patientPhysicalExamId);

                if (parentType == "")
                    dbManager.AddParameters(1, PARM_PARENT_TYPE, null);
                else
                    dbManager.AddParameters(1, PARM_PARENT_TYPE, parentType);

                if (parentId == 0)
                    dbManager.AddParameters(2, PARM_PARENT_ID, null);
                else
                    dbManager.AddParameters(2, PARM_PARENT_ID, parentId);

                if (patientPhysicalExamDetailId == 0)
                    dbManager.AddParameters(3, PARM_DETAIL_ID, null);
                else
                    dbManager.AddParameters(3, PARM_DETAIL_ID, patientPhysicalExamDetailId);

                dbManager.AddParameters(4, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PATIENT_PHYSICAL_EXAM_DETAIL_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePatientPhysicalExamDetail", PROC_PATIENT_PHYSICAL_EXAM_DETAIL_DELETE, ex);
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

        #region Association with Notes

        // Author: Ahmad Raza
        // Created Date:12-02-2016
        //OverView: methods to attach physical exam with notes
        public DSPhysicalExam attachPhysicalExamWithNotes(string physicalExamId, long notesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSPhysicalExam ds = new DSPhysicalExam();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (string.IsNullOrEmpty(physicalExamId))
                {
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, physicalExamId);
                }

                if (notesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, notesId);
                }


                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_PHYSICALEXAM_WITH_NOTES, ds, ds.PhysicalExam_UserSystem.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::attachPhysicalExamWithNotes", PROC_ATTACH_PHYSICALEXAM_WITH_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Ahmad Raza
        // Created Date:12-02-2016
        //OverView: detach physical exam from notes
        public string detachPhysicalExamFromNotes(long physicalExamId, long notesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (physicalExamId <= 0)
                {
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, physicalExamId);
                }

                if (notesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, notesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_PHYSICALEXAM_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::detachPhysicalExamFromNotes", PROC_DETACH_PHYSICALEXAM_FROM_NOTES, ex);
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

        #region lookup

        //Author:  Muhammad Arshad
        //Created Date: 26/02/2016
        //OverView: Created for Physical ExamSystem Lookup
        public DSPhysicalExamLookup lookupPhysicalExamSystem(long templateId)
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                  dbManager.CreateParameters(1);
                if (templateId == 0)
                    dbManager.AddParameters(0,PARM_TEMPLATE_ID , null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);

                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICAL_EXAM_SYSTEM_SELECT, ds, ds.PhysicalExamSystem.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::lookupCharacter", PROC_CHARACTER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Muhammad Arshad
        // Created Date: 18-02-2016
        //OverView: lookup character
        public DSPhysicalExamLookup lookupCharacter()
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CHARACTER_LOOKUP, ds, ds.PhysicalExamCharacter.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::lookupCharacter", PROC_CHARACTER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Ahmad Raza
        // Created Date: 18-02-2016
        //OverView: lookup context
        public DSPhysicalExamLookup lookupContext()
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CONTEXT_LOOKUP, ds, ds.PhysicalExamContext.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::lookupContext", PROC_CONTEXT_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Ahmad Raza
        // Created Date: 18-02-2016
        //OverView: lookup course
        public DSPhysicalExamLookup lookupCourse()
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_COURSE_LOOKUP, ds, ds.PhysicalExamCourse.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::lookupCourse", PROC_COURSE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Ahmad Raza
        // Created Date: 18-02-2016
        //OverView: lookup frequency
        public DSPhysicalExamLookup lookupFrequency()
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FREQUENCY_LOOKUP, ds, ds.PhysicalExamFrequency.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::lookupFrequency", PROC_FREQUENCY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Ahmad Raza
        // Created Date: 18-02-2016
        //OverView: lookup radiation
        public DSPhysicalExamLookup lookupRadiation()
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RADIATION_LOOKUP, ds, ds.PhysicalExamRadiation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::lookupRadiation", PROC_RADIATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Ahmad Raza
        // Created Date: 18-02-2016
        //OverView: lookup relievedby
        public DSPhysicalExamLookup lookupRelievedBy()
        {
            DSPhysicalExamLookup ds = new DSPhysicalExamLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSPhysicalExamLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_RELIEVED_BY_LOOKUP, ds, ds.PhysicalExamRelievedby.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::lookupRelievedBy", PROC_RELIEVED_BY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region [Soap text insert/update methods]

        // Author: Ahmad Raza
        // Created Date: 18-02-2016
        //OverView: insert update soap text for physical exam subcharacteristics
        public DSPhysicalExam insertUpdateSoapTextForPhysicalExamSubCharacteristics(long subCharacteristicId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (subCharacteristicId == 0)
                    dbManager.AddParameters(0, PARM_SUBCHAR_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SUBCHAR_ID, subCharacteristicId);

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_SUB_CHARACTERISTICS_SOAP_INSERT, ds, ds.PatientPhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::insertUpdateSoapTextForPhysicalExamSubCharacteristics", PROC_PHYSICALEXAM_SUB_CHARACTERISTICS_SOAP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Muhammad Arshad
        // Created Date: 18-02-2016
        //OverView: insert update soap text for physical exam characteristics
        public DSPhysicalExam insertUpdateSoapTextForPhysicalExamCharacteristics(long characteristicId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (characteristicId == 0)
                    dbManager.AddParameters(0, PARM_SYSTEMSECTION_CHAR_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SYSTEMSECTION_CHAR_ID, characteristicId);

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_CHARACTERISTICS_SOAP_INSERT, ds, ds.PatientPhysicalExamSystemSectionCharacteristic.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::insertUpdateSoapTextForPhysicalExamCharacteristics", PROC_PHYSICALEXAM_CHARACTERISTICS_SOAP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Muhammad Arshad
        // Created Date: 18-02-2016
        //OverView:insert update soap text for physical exam section
        public DSPhysicalExam insertUpdateSoapTextForPhysicalExamSection(long sectionId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (sectionId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_SECTION_ID, sectionId);

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_SECTION_SOAP_INSERT, ds, ds.PatientPhysicalExamSystemSection.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::insertUpdateSoapTextForPhysicalExamSection", PROC_PHYSICALEXAM_SECTION_SOAP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Muhammad Arshad
        // Created Date: 18-02-2016
        //OverView:insert update soap text for physical exam system
        public DSPhysicalExam insertUpdateSoapTextForPhysicalExamSystem(long systemId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);


                if (systemId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_SYSTEM_ID, systemId);

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICALEXAM_SYSTEM_SOAP_INSERT, ds, ds.PatientPhysicalExamSystem.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::insertUpdateSoapTextForPhysicalExamSystem", PROC_PHYSICALEXAM_SYSTEM_SOAP_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Farooq Ahmad
        // Created Date: 16/02/2016
        //OverView:insert update soap text for physical exam
        public DSPhysicalExam insertUpdateSoapTextForPhysicalExam(long patientPhysicalExamId,long templateId = 0)
        {

            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);


                if (patientPhysicalExamId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, patientPhysicalExamId);

                if (templateId == 0)
                {
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, null);
                    dbManager.AddParameters(2, PARM_USER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, templateId);
                    dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);
                }

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_UPDATE_SOAP_TEXT_FOR_PATIENT_PHYSICALEXAM, ds, ds.PatientPhysicalExam.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::insertUpdateSoapTextForPhysicalExam", PROC_UPDATE_SOAP_TEXT_FOR_PATIENT_PHYSICALEXAM, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }


        }

        // Author: Ahmad Raza
        // Created Date: 16/02/2016
        //OverView:insert update patient physical exam detail
        public DSPhysicalExam insertUpdatePatientPhysicalExamDetail(DSPhysicalExam ds, string parentType)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createPatientPhysicalExamDetailParameters(dbManager, ds, true, parentType);
                createPatientPhysicalExamDetailParameters(dbManager, ds, false, parentType);
                ds = (DSPhysicalExam)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PATIENTPHYSICALEXAM_DETAIL_INSERT, PROC_PATIENTPHYSICALEXAM_DETAIL_Update, ds, ds.PatientPhysicalExamDetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::insertUpdatePatientPhysicalExamDetail", PROC_PATIENTPHYSICALEXAM_DETAIL_INSERT, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        // Author: Farooq Ahmad
        // Created Date: 23/06/2016
        //OverView:insert update soap text for physical exam
        public DSPhysicalExam insertUpdateSoapTextForPhysicalExamAndChild(long patientPhysicalExamId, long templateId = 0)
        {

            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);


                if (patientPhysicalExamId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, patientPhysicalExamId);

                if (templateId == 0)
                {
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, null);
                    dbManager.AddParameters(2, PARM_USER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, templateId);
                    dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);
                }

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_UPDATE_SOAP_TEXT_FOR_PATIENT_PHYSICALEXAMANDCHILD, ds, ds.PatientPhysicalExam.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::sp_UpdateSoapTextForPatientPhysicalExamAndChild", PROC_UPDATE_SOAP_TEXT_FOR_PATIENT_PHYSICALEXAMANDCHILD, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }


        }


        #endregion

        /// <summary>
        /// Module Name: loadPhysicalExamForSoap
        /// Author: Ahmad Raza
        /// Created Date: 11-04-2016
        /// Description: load physical exam for soap text
        /// </summary>
        /// <param name="physicalExamId"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public DSPhysicalExam loadPhysicalExamForSoap(string physicalExamId, long patientId)
        {
            DSPhysicalExam ds = new DSPhysicalExam();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);


                if (string.IsNullOrEmpty(physicalExamId))
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_PHYSICAL_EXAM_ID, physicalExamId);
                if (patientId == 0)
                    dbManager.AddParameters(1, PARM_PATIENT_ID, DBNull.Value);
                else
                    dbManager.AddParameters(1, PARM_PATIENT_ID, patientId);

                ds = (DSPhysicalExam)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSICAL_EXAM_SELECT_FOR_SOAP_TEXT, ds, ds.PatientPhysicalExam.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::loadPhysicalExamForSoap", PROC_PHYSICAL_EXAM_SELECT_FOR_SOAP_TEXT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region Legacy Notes

        public List<PhysicalExam> NotesPhysicalExamSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<PhysicalExam> objList_PhysicalExam = new List<PhysicalExam>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_OBSERVATION_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        PhysicalExam model = new PhysicalExam();
                        var properties = typeof(PhysicalExam).GetProperties();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }

                        objList_PhysicalExam.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::NotesPhysicalExamSelect", PROC_NOTES_OBSERVATION_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_PhysicalExam;
        }

        #endregion Legacy Notes

    }
}
