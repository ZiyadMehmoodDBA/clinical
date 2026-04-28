/* Author:  Abid Ali
 * Created Date: 15/June/2016
 * OverView: Created for Physical Exam Data Template in Clinical Module
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

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALPhysicalExamDataTemplate
    {

        #region Variable
        
        #endregion

        #region "Stored Procedure Names"

        //Start Abid Ali 15/June/2016 Variable to store the insert sp of the physical exam data template
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SELECT = "Clinical.sp_PhysExamDataTemplateSelect";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_INSERT = "Clinical.sp_PhysExamDataTemplateInsert";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_UPDATE = "Clinical.sp_PhysExamDataTemplateUpdate";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_DELETE = "Clinical.sp_PhysExamDataTemplateDelete";
        //End Abid Ali 15/June/2016 Variable to store the insert sp of the physical exam data template

        //Start Abid Ali 15/June/2016 physical exam data template (sys,section,char,subChar)
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SYS_INSERT = "Clinical.sp_PhysExamDataTemplateSysInsert";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SYS_SELECT = "Clinical.sp_PhysExamDataTemplateSysSelect";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SYS_UPDATE = "Clinical.sp_PhysExamDataTemplateSysUpdate";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SYS_DELETE = "Clinical.sp_PhysExamDataTemplateSysDelete";

        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_DELETE = "Clinical.sp_PhysExamDataTemplateSectionDelete";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_SELECT = "Clinical.sp_PhysExamDataTemplateSectionSelect";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_INSERT = "Clinical.sp_PhysExamDataTemplateSectionInsert";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_UPDATE = "Clinical.sp_PhysExamDataTemplateSectionUpdate";


        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_CHAR_DELETE = "Clinical.sp_PhysExamDataTemplateCharDelete";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_CHAR_UPDATE = "Clinical.sp_PhysExamDataTemplateCharUpdate";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_CHAR_SELECT = "Clinical.sp_PhysExamDataTemplateCharSelect";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_CHAR_INSERT = "Clinical.sp_PhysExamDataTemplateCharInsert";

        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SUBCHAR_SELECT = "Clinical.sp_PhysExamDataTemplateSubCharSelect";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SUBCHAR_UPDATE = "Clinical.sp_PhysExamDataTemplateSubCharUpdate";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SUBCHAR_DELETE = "Clinical.sp_PhysExamDataTemplateSubCharDelete";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_SUBCHAR_INSERT = "Clinical.sp_PhysExamDataTemplateSubCharInsert";

        //Start Abid Ali 15/June/2016 physical exam (sys,section,char,subChar) template

        private const string PROC_PHYSEXAM_SYS_INSERT = "Clinical.sp_PhysicalExamSystemInsert";
        private const string PROC_PHYSEXAM_SECTION_INSERT = "Clinical.sp_PhysicalExamSystemSectionInsert";
        private const string PROC_PHYSEXAM_CHAR_INSERT = "Clinical.sp_PhysicalExamSystemSectionCharacteristicInsert";
        private const string PROC_PHYSEXAM_SUBCHAR_INSERT = "Clinical.sp_PhysicalExamSystemSectionCharacteristicSubCharacteristicInsert";

        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_DETAIL_DELETE = "Clinical.sp_PhysExamDataTemplateDetailDelete";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_DETAIL_UPDATE = "Clinical.sp_PhysExamDataTemplateDetailUpdate";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_DETAIL_SELECT = "Clinical.sp_PhysExamDataTemplateDetailSelect";
        private const string PROC_PHYS_EXAM_DATA_TEMPLATE_DETAIL_INSERT = "Clinical.sp_PhysExamDataTemplateDetailInsert";


        //End Abid Ali 15/June/2016 physical exam (sys,section,char,subChar) template

        #endregion

        #region "Parameters"

        private const string PARM_DATA_TEMPLATE_ID = "@DataTemplateId";
        private const string PARM_TEMPLATE_ID = "@TemplateId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_SPECIALTY_IDS = "@SpecialtyIds";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_DATA_TEMPLATE_NAME = "@DataTemplateName";

        //  private const string PARM_TEMPLATE_SPECIALTY_IDS = "@SpecialtyIds";
        //  private const string PARM_TEMPLATE_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_SOAP_TEXT = "@SoapText";

        private const string PARM_DATA_TEMPLATE_SYSID = "@DataTemplateSysId";
        private const string PARM_DATA_TEMPLATE_SECTION_ID = "@DataTemplateSectionId";
        private const string PARM_PHYSEXAM_DATA_TEMPLATE_ID = "@PhysExamDataTemplateId";


        private const string PARM_IS_NORMAL = "@IsNormal";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";



        private const string PARM_SYSID = "@SystemId";
        private const string PARM_SYSName = "@SystemName";
        // private const string PARM_PHYSICALEXAMSYSTEMID = "@PhysicalExamSystemId";

        private const string PARM_SEC_ID = "@SectionId";
        private const string PARM_SEC_NAME = "@SectionName";


        private const string PARM_CHAR_ID = "@CharId";
        private const string PARM_CHARACTERISTIC_ID = "@CharacteristicId";
        private const string PARM_CHAR_NAME = "@CharName";

        private const string PARM_SUBCHAR_ID = "@SubCharId";
        private const string PARM_SUBCHARACTERISTIC_ID = "@SubCharacteristicId";
        private const string PARM_SUBCHAR_NAME = "@SubCharName";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_SHORTNAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";

        //Start//16-02-2016//Ahmad Raza//For Detail section
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

        //End//16-02-2016//Ahmad Raza//For Detail section

        //Start 17-06-2016 Muhammad Arshad Parameters for Data Template Childs
        private const string PARM_DATA_TEMPLATE_CHAR_ID = "@DataTemplateCharId";
        private const string PARM_DATA_TEMPLATE_SUBCHAR_ID = "@DataTemplateSubCharId";

        private const string PARM_IS_POSITIVE = "@IsPositive";
        private const string PARM_IS_NEGATIVE = "@IsNegative";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
     
        //End 17-06-2016 Muhammad Arshad Parameters for Data Template Childs


        #endregion

        #region Constructors
        //Start Abid Ali 15/June/2016   Constructors 


        public DALPhysicalExamDataTemplate()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }
        private IContainer components;

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        //End Abid Ali 15/June/2016   Constructors 

        #endregion

        #region Support Functions PhysicalExam Data Template

        //Author: Abid Ali
        //Date: 15/June/2016
        //This function create the update parameter of the PhysicalExamDataTemplate table
        private void CreatePhysicalExamDataTemplateInsertUpdateParameters(IDBManager dbManager, DSPhysicalExamDataTemplate ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(9);
                dbManager.AddInsertUpdateParameters(0, PARM_DATA_TEMPLATE_ID, ds.PhysExamDataTemplate.DataTemplateIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(9);
                dbManager.AddInsertUpdateParameters(0, PARM_DATA_TEMPLATE_ID, ds.PhysExamDataTemplate.DataTemplateIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(1, PARM_DATA_TEMPLATE_NAME, ds.PhysExamDataTemplate.DataTemplateNameColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(2, PARM_TEMPLATE_ID, ds.PhysExamDataTemplate.TemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_IS_ACTIVE, ds.PhysExamDataTemplate.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(4, PARM_CREATED_BY, ds.PhysExamDataTemplate.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_ON, ds.PhysExamDataTemplate.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(6, PARM_MODIFIED_BY, ds.PhysExamDataTemplate.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_ON, ds.PhysExamDataTemplate.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_COMMENTS, ds.PhysExamDataTemplate.CommentsColumn.ColumnName, DbType.String);
        }

        //Author: Abid Ali
        //Date: 15/13/2016
        //This function create PhysicalExamSystemSectionCharacteristicsInsertParameters
        //private void CreatePhysicalExamSystemSectionCharacteristicsInsertParameters(IDBManager dbManager, DSPhysicalExamLookup ds)
        //{
        //    dbManager.CreateParameters(6);

        //    dbManager.AddParameters(0, PARM_CHARACTERISTIC_ID, ds.PhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
        //    dbManager.AddParameters(1, PARM_SEC_ID, ds.PhysicalExamSystemSectionCharacteristic.SectionIdColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(2, PARM_SHORTNAME, ds.PhysicalExamSystemSectionCharacteristic.ShortNameColumn.ColumnName, DbType.String);
        //    dbManager.AddParameters(3, PARM_DESCRIPTION, ds.PhysicalExamSystemSectionCharacteristic.DescriptionColumn.ColumnName, DbType.String);
        //    dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.PhysicalExamSystemSectionCharacteristic.IsActiveColumn.ColumnName, DbType.Byte);
        //    dbManager.AddParameters(5, PARM_TEMPLATE_ID, ds.PhysicalExamSystemSectionCharacteristic.TemplateIdColumn.ColumnName, DbType.String);
        //}

        //Author: Abid Ali
        //Date: 15/13/2016
        //This function create PhysicalExamSystemSectionCharacteristicsSubCharacteristicInsertParameters

        //private void CreatePhysicalExamSystemSectionCharacteristicsSubCharacteristicInsertParameters(IDBManager dbManager, DSPhysicalExamLookup ds)
        //{
        //    dbManager.CreateParameters(6);

        //    dbManager.AddParameters(0, PARM_SUBCHARACTERISTIC_ID, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
        //    dbManager.AddParameters(1, PARM_CHARACTERISTIC_ID, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.CharacteristicIdColumn.ColumnName, DbType.Int64);
        //    dbManager.AddParameters(2, PARM_SHORTNAME, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.ShortNameColumn.ColumnName, DbType.String);
        //    dbManager.AddParameters(3, PARM_DESCRIPTION, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.DescriptionColumn.ColumnName, DbType.String);
        //    dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.IsActiveColumn.ColumnName, DbType.Byte);
        //    dbManager.AddParameters(5, PARM_TEMPLATE_ID, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.TemplateIdColumn.ColumnName, DbType.String);
        //}

        // Author:  Abid Ali
        // Created Date: 17-June-2016
        //OverView: method to add parameters
        private void createPhysicalExamDataTemplateDetailInsertUpdateParameters(IDBManager dbManager, DSPhysicalExamDataTemplate ds, Boolean isInsert, string parentType = null)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(28);
                dbManager.AddInsertUpdateParameters(0, PARM_DETAIL_ID, ds.PhysExamDataTemplateDetail.DetailIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(28);
                dbManager.AddInsertUpdateParameters(0, PARM_DETAIL_ID, ds.PhysExamDataTemplateDetail.DetailIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_PHYSEXAM_DATA_TEMPLATE_ID, ds.PhysExamDataTemplateDetail.PhysExamDataTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_PARENT_TYPE, ds.PhysExamDataTemplateDetail.ParentTypeColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(3, PARM_PARENT_ID, ds.PhysExamDataTemplateDetail.ParentIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_PREVIOUS_HISTORY, ds.PhysExamDataTemplateDetail.PrevHistoryColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_STATUS_ID, ds.PhysExamDataTemplateDetail.StatusIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(6, PARM_ONSET, ds.PhysExamDataTemplateDetail.OnsetColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_DURATION_LENGTH, ds.PhysExamDataTemplateDetail.DurationLengthColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(8, PARM_DURATION_PERIOD_ID, ds.PhysExamDataTemplateDetail.DurationPeriodIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(9, PARM_PATTERN_ID, ds.PhysExamDataTemplateDetail.PatternIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(10, PARM_SEVERITY_ID, ds.PhysExamDataTemplateDetail.SeverityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(11, PARM_COURSE_ID, ds.PhysExamDataTemplateDetail.CourseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(12, PARM_RADIATION_ID, ds.PhysExamDataTemplateDetail.RadiationIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(13, PARM_FREQUENCY_ID, ds.PhysExamDataTemplateDetail.FrequencyIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(14, PARM_CONTEXT_ID, ds.PhysExamDataTemplateDetail.ContextIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(15, PARM_CHARACTER_ID, ds.PhysExamDataTemplateDetail.CharacterIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(16, PARM_AGGRAVATED_BY_ID, ds.PhysExamDataTemplateDetail.AggravatedByIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(17, PARM_RELIEVED_BY_ID, ds.PhysExamDataTemplateDetail.RelievedbyIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(18, PARM_LOCATION, ds.PhysExamDataTemplateDetail.LocationColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(19, PARM_PRECIPITATED_BY, ds.PhysExamDataTemplateDetail.PrecipitatedbyColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(20, PARM_ASSOCIATED_WITH, ds.PhysExamDataTemplateDetail.AssociatedWithColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(21, PARM_COMMENTS, ds.PhysExamDataTemplateDetail.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(22, PARM_IS_ACTIVE, ds.PhysExamDataTemplateDetail.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(23, PARM_CREATED_BY, ds.PhysExamDataTemplateDetail.CreatedByColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(24, PARM_CREATED_ON, ds.PhysExamDataTemplateDetail.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(25, PARM_MODIFIED_BY, ds.PhysExamDataTemplateDetail.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(26, PARM_MODIFIED_ON, ds.PhysExamDataTemplateDetail.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(27, PARM_SOAP_TEXT, ds.PhysExamDataTemplateDetail.SoapTextColumn.ColumnName, DbType.String);


        }


        #endregion

        #region Support Functions PhysicalExamDataTemplate System, section, Characteristics, SubCharacteristics (insert update)

        //Author: Abid Ali
        //Date: 09/03/2016
        //This function create the create parameter of the PhysicalExamDataTemplateSystem table
        private void CreatePhysicalExamDataTemplateSystemInsertUpdateParameters(IDBManager dbManager, DSPhysicalExamDataTemplate ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(9);
                dbManager.AddInsertUpdateParameters(0, PARM_DATA_TEMPLATE_SYSID, ds.PhysExamDataTemplateSys.DataTemplateSysIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(9);
                dbManager.AddInsertUpdateParameters(0, PARM_DATA_TEMPLATE_SYSID, ds.PhysExamDataTemplateSys.DataTemplateSysIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_DATA_TEMPLATE_ID, ds.PhysExamDataTemplateSys.DataTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SYSID, ds.PhysExamDataTemplateSys.SystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_IS_NORMAL, ds.PhysExamDataTemplateSys.IsNormalColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_ACTIVE, ds.PhysExamDataTemplateSys.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.PhysExamDataTemplateSys.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.PhysExamDataTemplateSys.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.PhysExamDataTemplateSys.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.PhysExamDataTemplateSys.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        //Author: Abid Ali
        //Date: 09/03/2016
        //This function create the create parameter of the PhysicalExamDataTemplateSection table
        private void CreatePhysicalExamDataTemplateSectionInsertUpdateParameters(IDBManager dbManager, DSPhysicalExamDataTemplate ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(9);
                dbManager.AddInsertUpdateParameters(0, PARM_DATA_TEMPLATE_SECTION_ID, ds.PhysExamDataTemplateSection.DataTemplateSectionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(9);
                dbManager.AddInsertUpdateParameters(0, PARM_DATA_TEMPLATE_SECTION_ID, ds.PhysExamDataTemplateSection.DataTemplateSectionIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_DATA_TEMPLATE_ID, ds.PhysExamDataTemplateSection.DataTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SYSID, ds.PhysExamDataTemplateSection.SystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_SEC_ID, ds.PhysExamDataTemplateSection.SectionIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(4, PARM_IS_ACTIVE, ds.PhysExamDataTemplateSection.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.PhysExamDataTemplateSection.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.PhysExamDataTemplateSection.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.PhysExamDataTemplateSection.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_ON, ds.PhysExamDataTemplateSection.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        //Author: Muhammad Arshad
        //Date: 17/06/2016
        //This function create the create parameter of the PhysExamDataTemplateChar table
        private void CreatePhysicalExamDataTemplateCharInsertUpdateParameters(IDBManager dbManager, DSPhysicalExamDataTemplate ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(12);
                dbManager.AddInsertUpdateParameters(0, PARM_DATA_TEMPLATE_CHAR_ID, ds.PhysExamDataTemplateChar.DataTemplateCharIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(12);
                dbManager.AddInsertUpdateParameters(0, PARM_DATA_TEMPLATE_CHAR_ID, ds.PhysExamDataTemplateChar.DataTemplateCharIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_DATA_TEMPLATE_ID, ds.PhysExamDataTemplateChar.DataTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SEC_ID, ds.PhysExamDataTemplateChar.SectionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_CHAR_ID, ds.PhysExamDataTemplateChar.CharIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_POSITIVE, ds.PhysExamDataTemplateChar.IsPositiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_NEGATIVE, ds.PhysExamDataTemplateChar.IsNegativeColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_COMMENTS, ds.PhysExamDataTemplateChar.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_IS_ACTIVE, ds.PhysExamDataTemplateChar.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_BY, ds.PhysExamDataTemplateChar.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CREATED_ON, ds.PhysExamDataTemplateChar.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_BY, ds.PhysExamDataTemplateChar.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_MODIFIED_ON, ds.PhysExamDataTemplateChar.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        //Author: Muhammad Arshad
        //Date: 17/06/2016
        //This function create the create parameter of the PhysicalExamDataTemplateSubChar table
        private void CreatePhysicalExamDataTemplateSubCharInsertUpdateParameters(IDBManager dbManager, DSPhysicalExamDataTemplate ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(12);
                dbManager.AddInsertUpdateParameters(0, PARM_DATA_TEMPLATE_SUBCHAR_ID, ds.PhysExamDataTemplateSubChar.DataTemplateSubCharIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(12);
                dbManager.AddInsertUpdateParameters(0, PARM_DATA_TEMPLATE_SUBCHAR_ID, ds.PhysExamDataTemplateSubChar.DataTemplateSubCharIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_DATA_TEMPLATE_ID, ds.PhysExamDataTemplateSubChar.DataTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CHAR_ID, ds.PhysExamDataTemplateSubChar.CharIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_SUBCHAR_ID, ds.PhysExamDataTemplateSubChar.SubCharIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_POSITIVE, ds.PhysExamDataTemplateChar.IsPositiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_NEGATIVE, ds.PhysExamDataTemplateChar.IsNegativeColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_COMMENTS, ds.PhysExamDataTemplateChar.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_IS_ACTIVE, ds.PhysExamDataTemplateSubChar.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_BY, ds.PhysExamDataTemplateSubChar.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_CREATED_ON, ds.PhysExamDataTemplateSubChar.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_BY, ds.PhysExamDataTemplateSubChar.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_MODIFIED_ON, ds.PhysExamDataTemplateSubChar.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        #endregion

        #region Physical Exam data template System, Section, Characteristics, SubCharacteristics ( Insert/Update/Delete/Select" )

        //Author: Abid Ali
        //Date: 09/03/2016        
        //This function will insert/update Physical Exam Template System
        public DSPhysicalExamDataTemplate InsertUpdatePhysicalExamDataTemplateSystem(DSPhysicalExamDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreatePhysicalExamDataTemplateSystemInsertUpdateParameters(dbManager, ds, true);
                this.CreatePhysicalExamDataTemplateSystemInsertUpdateParameters(dbManager, ds, false);

                ds = (DSPhysicalExamDataTemplate)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_SYS_INSERT, PROC_PHYS_EXAM_DATA_TEMPLATE_SYS_UPDATE, ds, ds.PhysExamDataTemplateSys.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::PROC_PHYSICALEXAM_DATA_TEMPLATE_SYSTEM_INSERT", PROC_PHYS_EXAM_DATA_TEMPLATE_SYS_INSERT + " " + PROC_PHYS_EXAM_DATA_TEMPLATE_SYS_UPDATE, ex);
                throw ex;
            }
        }

        //Author: Abid Ali
        //Date: 09/03/2016      
        //This function will load Physical Exam Template System
        public DSPhysicalExamDataTemplate LoadPhysicalExamDataTemplateSystem(long dataTemplateId)
        {
            DSPhysicalExamDataTemplate ds = new DSPhysicalExamDataTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(1);

                if (dataTemplateId <= 0)
                    dbManager.AddParameters(0, PARM_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DATA_TEMPLATE_ID, dataTemplateId);

                ds = (DSPhysicalExamDataTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_SYS_SELECT, ds, ds.PhysExamDataTemplateSys.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::PhysicalExamDataTemplate_System", PROC_PHYS_EXAM_DATA_TEMPLATE_SYS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //Author: Abid Ali
        //Date: 09/03/2016            
        //OverView: Delete PhysicalExam Template System
        public string DeletePhysicalExamDataTemplateSystem(long dataTemplateSysId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_DATA_TEMPLATE_SYSID, dataTemplateSysId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_SYS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePhysicalExamDataTemplateSystem", PROC_PHYS_EXAM_DATA_TEMPLATE_SYS_DELETE, ex);
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

        //Author: Abid Ali
        //Date: 09/03/2016        
        //This function will insert/update Physical Exam Template Section
        public DSPhysicalExamDataTemplate InsertUpdatePhysicalExamDataTemplateSection(DSPhysicalExamDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreatePhysicalExamDataTemplateSectionInsertUpdateParameters(dbManager, ds, true);
                this.CreatePhysicalExamDataTemplateSectionInsertUpdateParameters(dbManager, ds, false);

                ds = (DSPhysicalExamDataTemplate)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_INSERT, PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_UPDATE, ds, ds.PhysExamDataTemplateSection.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::PROC_PHYSICALEXAM_DATA_TEMPLATE_SECTION_INSERT", PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_INSERT + " " + PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_UPDATE, ex);
                throw ex;
            }
        }

        //Author: Abid Ali
        //Date: 09/03/2016      
        //This function will load Physical Exam Template Section
        public DSPhysicalExamDataTemplate LoadPhysicalExamDataTemplateSection(long dataTemplateId, long systemId)
        {
            DSPhysicalExamDataTemplate ds = new DSPhysicalExamDataTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (dataTemplateId <= 0)
                    dbManager.AddParameters(0, PARM_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DATA_TEMPLATE_ID, dataTemplateId);

                if (systemId <= 0)
                    dbManager.AddParameters(1, PARM_SYSID, null);
                else
                    dbManager.AddParameters(1, PARM_SYSID, systemId);

                ds = (DSPhysicalExamDataTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_SELECT, ds, ds.PhysExamDataTemplateSection.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::PhysicalExamDataTemplate_Section", PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //Author: Abid Ali
        //Date: 09/03/2016            
        //OverView: Delete PhysicalExam Template Section
        public string DeletePhysicalExamDataTemplateSection(long dataTemplateSectionId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_DATA_TEMPLATE_SECTION_ID, dataTemplateSectionId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePhysicalExamDataTemplateSection", PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_DELETE, ex);
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

        //Author: Muhammad Arshad
        //Date: 17/06/2016    
        //This function will insert/update Physical Exam Template Characteristics
        public DSPhysicalExamDataTemplate InsertUpdatePhysicalExamDataTemplateChar(DSPhysicalExamDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreatePhysicalExamDataTemplateCharInsertUpdateParameters(dbManager, ds, true);
                this.CreatePhysicalExamDataTemplateCharInsertUpdateParameters(dbManager, ds, false);

                ds = (DSPhysicalExamDataTemplate)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_CHAR_INSERT, PROC_PHYS_EXAM_DATA_TEMPLATE_CHAR_UPDATE, ds, ds.PhysExamDataTemplateChar.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::PROC_PHYSEXAM_TEMPLATE__CHAR_INSERT", PROC_PHYS_EXAM_DATA_TEMPLATE_CHAR_INSERT + " " + PROC_PHYS_EXAM_DATA_TEMPLATE_CHAR_UPDATE, ex);
                throw ex;
            }
        }

        //Author: Muhammad Arshad
        //Date: 17/06/2016 
        //This function will load Physical Exam Template Characteristic
        public DSPhysicalExamDataTemplate LoadPhysicalExamDataTemplateChar(long dataTemplateId, long sectionId)
        {
            DSPhysicalExamDataTemplate ds = new DSPhysicalExamDataTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (dataTemplateId <= 0)
                    dbManager.AddParameters(0, PARM_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DATA_TEMPLATE_ID, dataTemplateId);

                if (sectionId <= 0)
                    dbManager.AddParameters(1, PARM_SEC_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SEC_ID, sectionId);

                ds = (DSPhysicalExamDataTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_CHAR_SELECT, ds, ds.PhysExamDataTemplateChar.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::PhysicalExamDataTemplate_Characteristic", PROC_PHYS_EXAM_DATA_TEMPLATE_SECTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //Author: Muhammad Arshad
        //Date: 17/06/2016            
        //OverView: Delete PhysicalExam Template Characteristic
        public string DeletePhysicalExamDataTemplateChar(long templateCharId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_DATA_TEMPLATE_CHAR_ID, templateCharId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_CHAR_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePhysicalExamDataTemplateCharacteristic", PROC_PHYS_EXAM_DATA_TEMPLATE_CHAR_DELETE, ex);
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

        //Author: Muhammad Arshad
        //Date: 17/06/2016     
        //This function will insert/update Physical Exam Template SubCharacteristics
        public DSPhysicalExamDataTemplate InsertUpdatePhysicalExamDataTemplateSubChar(DSPhysicalExamDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreatePhysicalExamDataTemplateSubCharInsertUpdateParameters(dbManager, ds, true);
                this.CreatePhysicalExamDataTemplateSubCharInsertUpdateParameters(dbManager, ds, false);

                ds = (DSPhysicalExamDataTemplate)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_SUBCHAR_INSERT, PROC_PHYS_EXAM_DATA_TEMPLATE_SUBCHAR_UPDATE, ds, ds.PhysExamDataTemplateSubChar.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::PROC_PHYSEXAM_TEMPLATE__SUBCHAR_INSERT", PROC_PHYS_EXAM_DATA_TEMPLATE_SUBCHAR_INSERT + " " + PROC_PHYS_EXAM_DATA_TEMPLATE_SUBCHAR_UPDATE, ex);
                throw ex;
            }
        }

        //Author: Muhammad Arshad
        //Date: 17/06/2016      
        //This function will load Physical Exam Template SubCharacteristic
        public DSPhysicalExamDataTemplate LoadPhysicalExamDataTemplateSubChar(long dataTemplateId, long charId)
        {
            DSPhysicalExamDataTemplate ds = new DSPhysicalExamDataTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (dataTemplateId <= 0)
                    dbManager.AddParameters(0, PARM_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DATA_TEMPLATE_ID, dataTemplateId);

                if (charId <= 0)
                    dbManager.AddParameters(1, PARM_CHAR_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CHAR_ID, charId);

                ds = (DSPhysicalExamDataTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_SUBCHAR_SELECT, ds, ds.PhysExamDataTemplateSubChar.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::PhysicalExamDataTemplate_SubCharacteristic", PROC_PHYS_EXAM_DATA_TEMPLATE_SUBCHAR_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //Author: Muhammad Arshad
        //Date: 17/06/2016            
        //OverView: Delete PhysicalExam Template SubCharacteristic
        public string DeletePhysicalExamDataTemplateSubChar(long templateSubCharId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_DATA_TEMPLATE_SUBCHAR_ID, templateSubCharId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_SUBCHAR_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePhysicalExamDataTemplateSubChar", PROC_PHYS_EXAM_DATA_TEMPLATE_SUBCHAR_DELETE, ex);
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

        #region "Physical Exam Data Template Insert/Update/Delete/Select"

        //Author: Abid Ali
        //Date: 15/June/2016
        //This function will load of Physical Exam Data Template
        public DSPhysicalExamDataTemplate loadPhysicalExamDataTemplate(long dataTemplateId, long templateId, long entityId = 0, string providerIds = "", string specialtyIds = "",int pageNumber=0,int rpp=0,int isActive = -1)
        {
            DSPhysicalExamDataTemplate ds = new DSPhysicalExamDataTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(9);

                if (dataTemplateId == 0)
                    dbManager.AddParameters(0, PARM_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_DATA_TEMPLATE_ID, dataTemplateId);

                if (templateId == 0)
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, templateId);

                if (entityId == 0)
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, entityId);

                if (providerIds == "")
                    dbManager.AddParameters(3, PARM_PROVIDER_IDS, null);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_IDS, providerIds);

                if (specialtyIds == "")
                    dbManager.AddParameters(4, PARM_SPECIALTY_IDS, null);
                else
                    dbManager.AddParameters(4, PARM_SPECIALTY_IDS, specialtyIds);
                if (pageNumber <= 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, pageNumber);

                if (rpp <= 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, 15);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, rpp);               

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.PhysExamDataTemplate.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                
                if (isActive <= -1)
                    dbManager.AddParameters(8, PARM_IS_ACTIVE, null);
                else
                    dbManager.AddParameters(8, PARM_IS_ACTIVE, isActive);

                ds = (DSPhysicalExamDataTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_SELECT, ds, ds.PhysExamDataTemplate.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamDataTemplate::loadPhysicalExamDataTemplate", PROC_PHYS_EXAM_DATA_TEMPLATE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Abid Ali
        // Date: 15/June/2016
        //This function will insert/update Physical Exam data Template
        public DSPhysicalExamDataTemplate insertUpdatePhysicalExamDataTemplate(DSPhysicalExamDataTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreatePhysicalExamDataTemplateInsertUpdateParameters(dbManager, ds, true);
                this.CreatePhysicalExamDataTemplateInsertUpdateParameters(dbManager, ds, false);

                ds = (DSPhysicalExamDataTemplate)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_INSERT, PROC_PHYS_EXAM_DATA_TEMPLATE_UPDATE, ds, ds.PhysExamDataTemplate.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamDataTemplate::insertUpdatePhysicalExamDataTemplate", PROC_PHYS_EXAM_DATA_TEMPLATE_INSERT + " " + PROC_PHYS_EXAM_DATA_TEMPLATE_UPDATE, ex);
                throw ex;
            }
        }

        // Author: Abid Ali
        // Date: 15/June/2016
        //This function will delete Physical Exam Data Template
        public string deletePhysicalExamDataTemplate(long dataTemplateId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_DATA_TEMPLATE_ID, dataTemplateId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_DELETE);
                string Message = ((System.Data.SqlClient.SqlParameter)(dbManager.Parameters[1])).SqlValue.ToString();

                if (Message != "" && Message != "Null")
                    throw new Exception(Message);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamDataTemplate::deletePhysicalExamDataTemplate", PROC_PHYS_EXAM_DATA_TEMPLATE_DELETE, ex);
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



        //Start//For Data Template Detail

        // Author: Abid Ali
        // Date: 17/June/2016
        // Description: Loads Patient Physical Exam Detail
        public DSPhysicalExamDataTemplate LoadPhysicalExamDataTemplateDetail(long physExamDataTemplateId, long parentId, string parentType, long detailId)
        {
            DSPhysicalExamDataTemplate ds = new DSPhysicalExamDataTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);

                if (physExamDataTemplateId == 0)
                    dbManager.AddParameters(0, PARM_PHYSEXAM_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PHYSEXAM_DATA_TEMPLATE_ID, physExamDataTemplateId);

                if (parentId == 0)
                    dbManager.AddParameters(1, PARM_PARENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PARENT_ID, parentId);

                dbManager.AddParameters(2, PARM_PARENT_TYPE, parentType);

                if (detailId == 0)
                    dbManager.AddParameters(3, PARM_DETAIL_ID, null);
                else
                    dbManager.AddParameters(3, PARM_DETAIL_ID, detailId);

                ds = (DSPhysicalExamDataTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_DETAIL_SELECT, ds, ds.PhysExamDataTemplateDetail.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::LoadPhysicalExamDataTemplateDetail", PROC_PHYS_EXAM_DATA_TEMPLATE_DETAIL_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Abid Ali
        // Date: 17/June/2016
        // Description: Deletes Patient Physical Exam Detail
        public string DeletePhysicalExamDataTemplateDetail(long physExamDataTemplateId, string parentType, long parentId, long patientPhysicalExamDetailId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (physExamDataTemplateId == 0)
                    dbManager.AddParameters(0, PARM_PHYSEXAM_DATA_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PHYSEXAM_DATA_TEMPLATE_ID, physExamDataTemplateId);

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

                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_DETAIL_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePhysicalExamDataTemplateDetail", PROC_PHYS_EXAM_DATA_TEMPLATE_DETAIL_DELETE, ex);
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

        // Author: Abid Ali
        // Date: 17/June/2016
        //OverView:insert update patient physical exam detail
        public DSPhysicalExamDataTemplate insertUpdatePhysicalExamDataTemplateDetail(DSPhysicalExamDataTemplate ds, string parentType)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.createPhysicalExamDataTemplateDetailInsertUpdateParameters(dbManager, ds, true, parentType);
                this.createPhysicalExamDataTemplateDetailInsertUpdateParameters(dbManager, ds, false, parentType);
                ds = (DSPhysicalExamDataTemplate)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PHYS_EXAM_DATA_TEMPLATE_DETAIL_INSERT, PROC_PHYS_EXAM_DATA_TEMPLATE_DETAIL_UPDATE, ds, ds.PhysExamDataTemplateDetail.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExam::insertUpdatePhysicalExamDataTemplateDetail", PROC_PHYS_EXAM_DATA_TEMPLATE_DETAIL_INSERT, ex);
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

        //END//For Data Template Detail

        #endregion


    }

}
