/* Author:  Muhammad Arshad
 * Created Date: 03/03/2016
 * OverView: Created for Physical Exam Template in Clinical Module
 */

using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALPhysicalExamTemplate
    {
       

        #region "Stored Procedure Names"
        //Start Farooq Ahmad 03/03/2016 Variable to store the select sp of the physical exam template
        private const string PROC_PHYSEXAMTEMPLATE_SELECT = "Clinical.sp_PhysExamTemplateSelect";
        //End Farooq Ahmad 03/03/2016 Variable to store the select sp of the physical exam template

        private const string PROC_PHYSEXAMTEMPLATE_SELECTECW = "Clinical.sp_PETemplateSelect";


        //Start Farooq Ahmad 03/07/2016 Variable to store the insert sp of the physical exam template
        private const string PROC_PHYSEXAMTEMPLATE_INSERT = "Clinical.sp_PhysExamTemplateInsert";
        private const string PROC_PHYSEXAMTEMPLATE_UPDATE = "Clinical.sp_PhysExamTemplateUpdate";
        private const string PROC_PHYSEXAMTEMPLATE_DELETE = "Clinical.sp_PhysExamTemplateDelete";
        //End Farooq Ahmad 03/07/2016 Variable to store the insert sp of the physical exam template
        //Start Abid Ali 03/09/2016 physical exam template
        private const string PROC_PHYSEXAMTEMPLATE_SYS_INSERT = "Clinical.sp_PhysExamTemplateSysInsert";
        private const string PROC_PHYSEXAMTEMPLATE_SYS_SELECT = "Clinical.sp_PhysExamTemplateSysSelect";
        private const string PROC_PHYSEXAMTEMPLATE_SYS_UPDATE = "Clinical.sp_PhysExamTemplateSysUpdate";
        private const string PROC_PHYSEXAMTEMPLATE_SYS_DELETE = "Clinical.sp_PhysExamTemplateSysDelete";

        private const string PROC_PHYSEXAMTEMPLATE_SECTION_DELETE = "Clinical.sp_PhysExamTemplateSectionDelete";


        private const string PROC_PHYSEXAMTEMPLATE_SECTION_SELECT_FOR_DATA_TEMPLATE = "Clinical.sp_PhysExamTemplateSectionSelectForDataTemplate";
        private const string PROC_PHYSEXAMTEMPLATE_SECTION_SELECT = "Clinical.sp_PhysExamTemplateSectionSelect";

        private const string PROC_PHYSEXAMTEMPLATE_SECTION_INSERT = "Clinical.sp_PhysExamTemplateSectionInsert";
        private const string PROC_PHYSEXAMTEMPLATE_SECTION_UPDATE = "Clinical.sp_PhysExamTemplateSectionUpdate";


        private const string PROC_PHYSEXAMTEMPLATE_CHAR_DELETE = "Clinical.sp_PhysExamTemplateCharDelete";
        private const string PROC_PHYSEXAMTEMPLATE_CHAR_UPDATE = "Clinical.sp_PhysExamTemplateCharUpdate";


        private const string PROC_PHYSEXAMTEMPLATE_CHAR_SELECT_FOR_DATA_TEMPLATE = "Clinical.sp_PhysExamTemplateCharSelectForDataTemplate";
        private const string PROC_PHYSEXAMTEMPLATE_CHAR_SELECT = "Clinical.sp_PhysExamTemplateCharSelect";


        private const string PROC_PHYSEXAMTEMPLATE_CHAR_INSERT = "Clinical.sp_PhysExamTemplateCharInsert";

        private const string PROC_PHYSEXAMTEMPLATE_SUBCHAR_SELECT_FOR_DATA_TEMPLATE = "Clinical.sp_PhysExamTemplateSubCharSelectForDataTemplate";

        private const string PROC_PHYSEXAMTEMPLATE_SUBCHAR_SELECT = "Clinical.sp_PhysExamTemplateSubCharSelect";
        private const string PROC_PHYSEXAMTEMPLATE_SUBCHAR_UPDATE = "Clinical.sp_PhysExamTemplateSubCharUpdate";
        private const string PROC_PHYSEXAMTEMPLATE_SUBCHAR_DELETE = "Clinical.sp_PhysExamTemplateSubCharDelete";
        private const string PROC_PHYSEXAMTEMPLATE_SUBCHAR_INSERT = "Clinical.sp_PhysExamTemplateSubCharInsert";

        //Start Abid Ali 03/09/2016 physical exam template

        private const string PROC_PHYSEXAM_TEMPLATE_SAVEAS = "Clinical.sp_PhysExamTemplateSaveAs";

        private const string PROC_PHYSEXAM_SYS_INSERT = "Clinical.sp_PhysicalExamSystemInsert";
        private const string PROC_PHYSEXAM_SECTION_INSERT = "Clinical.sp_PhysicalExamSystemSectionInsert";
        private const string PROC_PHYSEXAM_CHAR_INSERT = "Clinical.sp_PhysicalExamSystemSectionCharacteristicInsert";
        private const string PROC_PHYSEXAM_SUBCHAR_INSERT = "Clinical.sp_PhysicalExamSystemSectionCharacteristicSubCharacteristicInsert";


        private const string PROC_PHYSEXAM_SEC_CHAR_SUBCHAR_ISCHECKED_UPDATE = "Clinical.sp_PhysExamTemplateSecCharSubCharIsCheckedUpdate";    
        private const string PROC_PHYSEXAM_CHAR_SUBCHAR_ISCHECKED_UPDATE = "Clinical.sp_PhysExamTemplateCharSubCharIsCheckedUpdate";
        private const string PROC_PHYSEXAM_SUBCHAR_ISCHECKED_UPDATE = "Clinical.sp_PhysExamTemplateSubCharIsCheckedUpdate";



        #endregion

        #region "Parameters"

        private const string PARM_TEMPLATE_ID = "@TemplateId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_SPECIALTY_IDS = "@SpecialtyIds";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_TEMPLATE_NAME = "@TemplateName";

        private const string PARM_TEMPLATE_SPECIALTY_IDS = "@SpecialtyIds";
        private const string PARM_TEMPLATE_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_TEMPLATE_ENTITY_ID = "@EntityId";

        private const string PARM_TEMPLATE_SYS_ID = "@TemplateSysId";
        private const string PARM_TEMPLATE_SECTION_ID = "@TemplateSectionId";
        private const string PARM_TEMPLATE_CHAR_ID = "@TemplateCharId";
        private const string PARM_TEMPLATE_SUBCHAR_ID = "@TemplateSubCharId";

        private const string PARM_IS_CHECKED = "@IsChecked";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";

        private const string PARM_SYSID = "@SystemId";
        private const string PARM_SYSName = "@SystemName";
        private const string PARM_PHYSICALEXAMSYSTEMID = "@PhysicalExamSystemId";

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


        private const string PARM_OLD_TEMPLATE_ID = "@OldTemplateId";
        private const string PARM_NEW_TEMPLATE_ID = "@NewTemplateId";

        private const string PARM_NEW_TEMPLATE_SYSTEM_IDs = "@SystemIds";
        private const string PARM_NEW_TEMPLATE_SECTION_IDs = "@SectionIds";
        private const string PARM_NEW_TEMPLATE_CHAR_IDs = "@CharIds";
        private const string PARM_NEW_TEMPLATE_SUBCHAR_IDs = "@SubCharIds";

        #endregion

        #region Constructors
        //Start Farooq Ahmad 03/03/2016   Constructors 


        public DALPhysicalExamTemplate()
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

        //End Farooq Ahmad 03/03/2016   Constructors 

        #endregion

        #region Support Functions PhysicalExam_System

        //Author: Farooq Ahmad
        //Date: 07/03/2016
        //This function create the update parameter of the PhysicalExamTemplate table
        private void CreatePhysicalExamTemplateUpdateParameters(IDBManager dbManager, DSPhysicalExamTemplate ds)
        {
            dbManager.CreateUpdateParameters(10);
            dbManager.AddInsertUpdateParameters(0, PARM_TEMPLATE_ID, ds.PhysExamTemplate.TemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(1, PARM_ENTITY_ID, ds.PhysExamTemplate.EntityIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(2, PARM_TEMPLATE_NAME, ds.PhysExamTemplate.TemplateNameColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_SPECIALTY_IDS, ds.PhysExamTemplate.SpecialtyIdsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_PROVIDER_IDS, ds.PhysExamTemplate.ProviderIdsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.PhysExamTemplate.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.PhysExamTemplate.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.PhysExamTemplate.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.PhysExamTemplate.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.PhysExamTemplate.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        //Author: Farooq Ahmad
        //Date: 08/02/2016
        //This function create the create parameter of the PhysicalExamTemplate table
        private void CreatePhysicalExamTemplateInsertParameters(IDBManager dbManager, DSPhysicalExamTemplate ds)
        {
            dbManager.CreateInsertParameters(10);


            dbManager.AddInsertUpdateParameters(0, PARM_TEMPLATE_ID, ds.PhysExamTemplate.TemplateIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddInsertUpdateParameters(1, PARM_ENTITY_ID, ds.PhysExamTemplate.EntityIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(2, PARM_TEMPLATE_NAME, ds.PhysExamTemplate.TemplateNameColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_SPECIALTY_IDS, ds.PhysExamTemplate.SpecialtyIdsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_PROVIDER_IDS, ds.PhysExamTemplate.ProviderIdsColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.PhysExamTemplate.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.PhysExamTemplate.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.PhysExamTemplate.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.PhysExamTemplate.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.PhysExamTemplate.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        //Author: Farooq Ahmad
        //Date: 15/13/2016
        //This function create PhysicalExamSystemInsertParameters
        private void CreatePhysicalExamSystemInsertParameters(IDBManager dbManager, DSPhysicalExamLookup ds)
        {
            dbManager.CreateParameters(5);


            dbManager.AddParameters(0, PARM_SYSID, ds.PhysicalExamSystem.SystemIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_SHORTNAME, ds.PhysicalExamSystem.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.PhysicalExamSystem.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_IS_ACTIVE, ds.PhysicalExamSystem.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(4, PARM_TEMPLATE_ID, ds.PhysicalExamSystem.TemplateIdColumn.ColumnName, DbType.String);

        }

        //Author: Farooq Ahmad
        //Date: 15/13/2016
        //This function create PhysicalExamSystemSectionInsertParameters
        private void CreatePhysicalExamSystemSectionInsertParameters(IDBManager dbManager, DSPhysicalExamLookup ds)
        {
            dbManager.CreateParameters(6);

            dbManager.AddParameters(0, PARM_SEC_ID, ds.PhysicalExamSystemSection.SectionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_PHYSICALEXAMSYSTEMID, ds.PhysicalExamSystemSection.PhysicalExamSystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SHORTNAME, ds.PhysicalExamSystemSection.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.PhysicalExamSystemSection.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.PhysicalExamSystemSection.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_TEMPLATE_ID, ds.PhysicalExamSystemSection.TemplateIdColumn.ColumnName, DbType.String);
        }

        //Author: Farooq Ahmad
        //Date: 15/13/2016
        //This function create PhysicalExamSystemSectionCharacteristicsInsertParameters
        private void CreatePhysicalExamSystemSectionCharacteristicsInsertParameters(IDBManager dbManager, DSPhysicalExamLookup ds)
        {
            dbManager.CreateParameters(6);

            dbManager.AddParameters(0, PARM_CHARACTERISTIC_ID, ds.PhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_SEC_ID, ds.PhysicalExamSystemSectionCharacteristic.SectionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SHORTNAME, ds.PhysicalExamSystemSectionCharacteristic.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.PhysicalExamSystemSectionCharacteristic.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.PhysicalExamSystemSectionCharacteristic.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_TEMPLATE_ID, ds.PhysicalExamSystemSectionCharacteristic.TemplateIdColumn.ColumnName, DbType.String);
        }

        //Author: Farooq Ahmad
        //Date: 15/13/2016
        //This function create PhysicalExamSystemSectionCharacteristicsSubCharacteristicInsertParameters

        private void CreatePhysicalExamSystemSectionCharacteristicsSubCharacteristicInsertParameters(IDBManager dbManager, DSPhysicalExamLookup ds)
        {
            dbManager.CreateParameters(6);

            dbManager.AddParameters(0, PARM_SUBCHARACTERISTIC_ID, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            dbManager.AddParameters(1, PARM_CHARACTERISTIC_ID, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.CharacteristicIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_SHORTNAME, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_DESCRIPTION, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_TEMPLATE_ID, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.TemplateIdColumn.ColumnName, DbType.String);
        }

        #endregion



        #region Support Functions PhysicalExamTemplate System, section, Characteristics, SubCharacteristics

        //Author: Abid Ali
        //Date: 09/03/2016
        //This function create the create parameter of the PhysicalExamTemplateSystem table
        private void CreatePhysicalExamTemplateSystemInsertUpdateParameters(IDBManager dbManager, DSPhysicalExamTemplate ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_TEMPLATE_SYS_ID, ds.PhysExamTemplateSys.TemplateSysIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(10);
                dbManager.AddInsertUpdateParameters(0, PARM_TEMPLATE_SYS_ID, ds.PhysExamTemplateSys.TemplateSysIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_TEMPLATE_ID, ds.PhysExamTemplateSys.TemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SYSID, ds.PhysExamTemplateSys.SystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_SYSName, ds.PhysExamTemplateSys.SystemNameColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_CHECKED, ds.PhysExamTemplateSys.IsCheckedColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.PhysExamTemplateSys.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.PhysExamTemplateSys.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.PhysExamTemplateSys.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.PhysExamTemplateSys.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_ON, ds.PhysExamTemplateSys.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        //Author: Abid Ali
        //Date: 09/03/2016
        //This function create the create parameter of the PhysicalExamTemplateSection table
        private void CreatePhysicalExamTemplateSectionInsertUpdateParameters(IDBManager dbManager, DSPhysicalExamTemplate ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(11);
                dbManager.AddInsertUpdateParameters(0, PARM_TEMPLATE_SECTION_ID, ds.PhysExamTemplateSection.TemplateSectionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(11);
                dbManager.AddInsertUpdateParameters(0, PARM_TEMPLATE_SECTION_ID, ds.PhysExamTemplateSection.TemplateSectionIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_TEMPLATE_ID, ds.PhysExamTemplateSection.TemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SYSID, ds.PhysExamTemplateSection.SystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_SEC_ID, ds.PhysExamTemplateSection.SectionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_SEC_NAME, ds.PhysExamTemplateSection.SectionNameColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_CHECKED, ds.PhysExamTemplateSection.IsCheckedColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.PhysExamTemplateSection.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.PhysExamTemplateSection.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.PhysExamTemplateSection.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.PhysExamTemplateSection.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.PhysExamTemplateSection.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        //Author: Abid Ali
        //Date: 09/03/2016
        //This function create the create parameter of the PhysicalExamTemplateChar table
        private void CreatePhysicalExamTemplateCharInsertUpdateParameters(IDBManager dbManager, DSPhysicalExamTemplate ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(11);
                dbManager.AddInsertUpdateParameters(0, PARM_TEMPLATE_CHAR_ID, ds.PhysExamTemplateChar.TemplateCharIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(11);
                dbManager.AddInsertUpdateParameters(0, PARM_TEMPLATE_CHAR_ID, ds.PhysExamTemplateChar.TemplateCharIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_TEMPLATE_ID, ds.PhysExamTemplateChar.TemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_SEC_ID, ds.PhysExamTemplateChar.SectionIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_CHAR_ID, ds.PhysExamTemplateChar.CharIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(4, PARM_CHAR_NAME, ds.PhysExamTemplateChar.CharNameColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_CHECKED, ds.PhysExamTemplateChar.IsCheckedColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.PhysExamTemplateChar.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.PhysExamTemplateChar.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.PhysExamTemplateChar.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.PhysExamTemplateChar.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.PhysExamTemplateChar.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        //Author: Abid Ali
        //Date: 09/03/2016
        //This function create the create parameter of the PhysicalExamTemplateSubChar table
        private void CreatePhysicalExamTemplateSubCharInsertUpdateParameters(IDBManager dbManager, DSPhysicalExamTemplate ds, bool isInsert = true)
        {
            if (isInsert == true)
            {
                dbManager.CreateInsertParameters(11);
                dbManager.AddInsertUpdateParameters(0, PARM_TEMPLATE_SUBCHAR_ID, ds.PhysExamTemplateSubChar.TemplateSubCharIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(11);
                dbManager.AddInsertUpdateParameters(0, PARM_TEMPLATE_SUBCHAR_ID, ds.PhysExamTemplateSubChar.TemplateSubCharIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_TEMPLATE_ID, ds.PhysExamTemplateSubChar.TemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CHAR_ID, ds.PhysExamTemplateSubChar.CharIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_SUBCHAR_ID, ds.PhysExamTemplateSubChar.SubCharIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(4, PARM_SUBCHAR_NAME, ds.PhysExamTemplateSubChar.SubCharNameColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(5, PARM_IS_CHECKED, ds.PhysExamTemplateSubChar.IsCheckedColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(6, PARM_IS_ACTIVE, ds.PhysExamTemplateSubChar.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_BY, ds.PhysExamTemplateSubChar.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_CREATED_ON, ds.PhysExamTemplateSubChar.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_BY, ds.PhysExamTemplateSubChar.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_MODIFIED_ON, ds.PhysExamTemplateSubChar.ModifiedOnColumn.ColumnName, DbType.DateTime);

        }

        #endregion





        #region Physical Exam template System, Section, Characteristics, SubCharacteristics ( Insert/Update/Delete/Select" )

        //Author: Abid Ali
        //Date: 09/03/2016        
        //This function will insert/update Physical Exam Template System
        public DSPhysicalExamTemplate InsertUpdatePhysicalExamTemplateSystem(DSPhysicalExamTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.PhysExamTemplateSys.GetChanges();
                dbManager.BeginTransaction();

                this.CreatePhysicalExamTemplateSystemInsertUpdateParameters(dbManager, ds, true);
                this.CreatePhysicalExamTemplateSystemInsertUpdateParameters(dbManager, ds, false);

                ds = (DSPhysicalExamTemplate)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SYS_INSERT, PROC_PHYSEXAMTEMPLATE_SYS_UPDATE, ds, ds.PhysExamTemplateSys.TableName);
                if (dtTemp != null)
                {
                    //dtTemp.Columns.Add("PrimaryKey");
                    //for (int i = 0; i < ds.PhysExamTemplateSys.Rows.Count; i++)
                    //{
                    //    dtTemp.Rows[i]["PrimaryKey"] = ds.PhysExamTemplateSys.Rows[i][ds.PhysExamTemplateSys.TemplateSysIdColumn];
                    //}

                    //dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplateSys.Rows[0][ds.PhysExamTemplateSys.TemplateSysIdColumn].ToString(), null, ds.PhysExamTemplateSys.Rows[0][ds.PhysExamTemplateSys.TemplateIdColumn].ToString());
                    //dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExam::PROC_PHYSICALEXAM_TEMPLATE_SYSTEM_INSERT", PROC_PHYSEXAMTEMPLATE_SYS_INSERT + " " + PROC_PHYSEXAMTEMPLATE_SYS_UPDATE, ex);
                throw ex;
            }
        }

        //Author: Abid Ali
        //Date: 09/03/2016      
        //This function will load Physical Exam Template System
        public DSPhysicalExamTemplate LoadPhysicalExamTemplateSystem(long templateId, string isViewOrder = "", string isPrintOrder = "")
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {

                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);

                if (templateId <= 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);

                ds = (DSPhysicalExamTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SYS_SELECT, ds, ds.PhysExamTemplateSys.TableName);
                if (templateId > 0)
                {

                    DataTable dtTemp = ds.PhysExamTemplateSys;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplateSys.Rows[0][ds.PhysExamTemplateSys.TemplateSysIdColumn].ToString(), null, ds.PhysExamTemplateSys.Rows[0][ds.PhysExamTemplateSys.TemplateIdColumn].ToString(), isViewAction, isPrintAcion);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExam::PhysicalExamTemplate_System", PROC_PHYSEXAMTEMPLATE_SYS_SELECT, ex);
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
        public string DeletePhysicalExamTemplateSystem(long templateSystemId, long templateId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                DSPhysicalExamTemplate dsCurrentOrder = LoadPhysicalExamTemplateSystem(templateId, "", ""); // template Id

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TEMPLATE_SYS_ID, templateSystemId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SYS_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrder.PhysExamTemplateSys;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, templateSystemId.ToString(), null, templateSystemId.ToString(), false, false, true); // template id is missing
                        dsDBAudit.AcceptChanges();
                    }
                }
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePhysicalExamTemplateSystem", PROC_PHYSEXAMTEMPLATE_SYS_DELETE, ex);
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
        public DSPhysicalExamTemplate InsertUpdatePhysicalExamTemplateSection(DSPhysicalExamTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.PhysExamTemplateSection.GetChanges();
                dbManager.BeginTransaction();

                this.CreatePhysicalExamTemplateSectionInsertUpdateParameters(dbManager, ds, true);
                this.CreatePhysicalExamTemplateSectionInsertUpdateParameters(dbManager, ds, false);

                ds = (DSPhysicalExamTemplate)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SECTION_INSERT, PROC_PHYSEXAMTEMPLATE_SECTION_UPDATE, ds, ds.PhysExamTemplateSection.TableName);
                if (dtTemp != null)
                {
                    //dtTemp.Columns.Add("PrimaryKey");
                    //for (int i = 0; i < ds.PhysExamTemplateSection.Rows.Count; i++)
                    //{
                    //    dtTemp.Rows[i]["PrimaryKey"] = ds.PhysExamTemplateSection.Rows[i][ds.PhysExamTemplateSection.TemplateSectionIdColumn];
                    //}

                    //dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplateSection.Rows[0][ds.PhysExamTemplateSection.TemplateSectionIdColumn].ToString(), null, ds.PhysExamTemplateSection.Rows[0][ds.PhysExamTemplateSection.TemplateIdColumn].ToString());
                    //dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExam::PROC_PHYSICALEXAM_TEMPLATE_SECTION_INSERT", PROC_PHYSEXAMTEMPLATE_SECTION_INSERT + " " + PROC_PHYSEXAMTEMPLATE_SECTION_UPDATE, ex);
                throw ex;
            }
        }

        //Author: Abid Ali
        //Date: 09/03/2016      
        //This function will load Physical Exam Template Section
        public DSPhysicalExamTemplate LoadPhysicalExamTemplateSection(long templateId, long systemId, string isViewOrder = "", string isPrintOrder = "")
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (templateId <= 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);

                if (systemId <= 0)
                    dbManager.AddParameters(1, PARM_SYSID, null);
                else
                    dbManager.AddParameters(1, PARM_SYSID, systemId);

                ds = (DSPhysicalExamTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SECTION_SELECT, ds, ds.PhysExamTemplateSection.TableName);
                if (templateId > 0)
                {

                    DataTable dtTemp = ds.PhysExamTemplateSection;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplateSection.Rows[0][ds.PhysExamTemplateSection.TemplateSectionIdColumn].ToString(), null, ds.PhysExamTemplateSection.Rows[0][ds.PhysExamTemplateSection.TemplateIdColumn].ToString(), isViewAction, isPrintAcion);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExam::PhysicalExamTemplate_Section", PROC_PHYSEXAMTEMPLATE_SECTION_SELECT, ex);
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
        public string DeletePhysicalExamTemplateSection(long templateSectionId, long templateId, long systemId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                DSPhysicalExamTemplate dsCurrentOrder = LoadPhysicalExamTemplateSection(templateId, systemId, "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TEMPLATE_SECTION_ID, templateSectionId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SECTION_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrder.PhysExamTemplateSection;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, templateSectionId.ToString(), null, templateSectionId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                }
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePhysicalExamTemplateSection", PROC_PHYSEXAMTEMPLATE_SECTION_DELETE, ex);
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
        //This function will insert/update Physical Exam Template Characteristics
        public DSPhysicalExamTemplate InsertUpdatePhysicalExamTemplateChar(DSPhysicalExamTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.PhysExamTemplateChar.GetChanges();
                dbManager.BeginTransaction();
                this.CreatePhysicalExamTemplateCharInsertUpdateParameters(dbManager, ds, true);
                this.CreatePhysicalExamTemplateCharInsertUpdateParameters(dbManager, ds, false);

                ds = (DSPhysicalExamTemplate)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_CHAR_INSERT, PROC_PHYSEXAMTEMPLATE_CHAR_UPDATE, ds, ds.PhysExamTemplateChar.TableName);
                if (dtTemp != null)
                {
                    //dtTemp.Columns.Add("PrimaryKey");
                    //for (int i = 0; i < ds.PhysExamTemplateChar.Rows.Count; i++)
                    //{
                    //    dtTemp.Rows[i]["PrimaryKey"] = ds.PhysExamTemplateChar.Rows[i][ds.PhysExamTemplateChar.TemplateCharIdColumn];
                    //}

                    //dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplateChar.Rows[0][ds.PhysExamTemplateChar.TemplateCharIdColumn].ToString(), null, ds.PhysExamTemplateChar.Rows[0][ds.PhysExamTemplateChar.TemplateIdColumn].ToString());
                    //dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExam::PROC_PHYSEXAM_TEMPLATE__CHAR_INSERT", PROC_PHYSEXAMTEMPLATE_CHAR_INSERT + " " + PROC_PHYSEXAMTEMPLATE_CHAR_UPDATE, ex);
                throw ex;
            }
        }

        //Author: Abid Ali
        //Date: 09/03/2016      
        //This function will load Physical Exam Template Characteristic
        public DSPhysicalExamTemplate LoadPhysicalExamTemplateChar(long templateId, long sectionId, string isViewOrder = "", string isPrintOrder = "")
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {

                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (templateId <= 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);

                if (sectionId <= 0)
                    dbManager.AddParameters(1, PARM_SEC_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SEC_ID, sectionId);

                ds = (DSPhysicalExamTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_CHAR_SELECT, ds, ds.PhysExamTemplateChar.TableName);
                if (templateId > 0)
                {

                    DataTable dtTemp = ds.PhysExamTemplateChar;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplateChar.Rows[0][ds.PhysExamTemplateChar.TemplateCharIdColumn].ToString(), null, ds.PhysExamTemplateChar.Rows[0][ds.PhysExamTemplateChar.TemplateIdColumn].ToString(), isViewAction, isPrintAcion);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExam::PhysicalExamTemplate_Characteristic", PROC_PHYSEXAMTEMPLATE_SECTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //Author: Abid Ali
        //Date: 09/03/2016            
        //OverView: Delete PhysicalExam Template Characteristic
        public string DeletePhysicalExamTemplateChar(long templateCharId, long templateId, long sectionId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                DSPhysicalExamTemplate dsCurrentOrder = LoadPhysicalExamTemplateChar(templateId, sectionId, "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TEMPLATE_CHAR_ID, templateCharId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_CHAR_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrder.PhysExamTemplateChar;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, templateCharId.ToString(), null, templateCharId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                }
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePhysicalExamTemplateCharacteristic", PROC_PHYSEXAMTEMPLATE_CHAR_DELETE, ex);
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
        //This function will insert/update Physical Exam Template SubCharacteristics
        public DSPhysicalExamTemplate InsertUpdatePhysicalExamTemplateSubChar(DSPhysicalExamTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.PhysExamTemplateSubChar.GetChanges();
                dbManager.BeginTransaction();

                this.CreatePhysicalExamTemplateSubCharInsertUpdateParameters(dbManager, ds, true);
                this.CreatePhysicalExamTemplateSubCharInsertUpdateParameters(dbManager, ds, false);

                ds = (DSPhysicalExamTemplate)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SUBCHAR_INSERT, PROC_PHYSEXAMTEMPLATE_SUBCHAR_UPDATE, ds, ds.PhysExamTemplateSubChar.TableName);
                if (dtTemp != null)
                {
                    //dtTemp.Columns.Add("PrimaryKey");
                    //for (int i = 0; i < ds.PhysExamTemplateSubChar.Rows.Count; i++)
                    //{
                    //    dtTemp.Rows[i]["PrimaryKey"] = ds.PhysExamTemplateSubChar.Rows[i][ds.PhysExamTemplateSubChar.TemplateSubCharIdColumn];
                    //}

                    //dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplateSubChar.Rows[0][ds.PhysExamTemplateSubChar.SubCharIdColumn].ToString(), null, ds.PhysExamTemplateSubChar.Rows[0][ds.PhysExamTemplateSubChar.TemplateIdColumn].ToString());
                    //dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExam::PROC_PHYSEXAM_TEMPLATE__SUBCHAR_INSERT", PROC_PHYSEXAMTEMPLATE_SUBCHAR_INSERT + " " + PROC_PHYSEXAMTEMPLATE_SUBCHAR_UPDATE, ex);
                throw ex;
            }
        }

        //Author: Abid Ali
        //Date: 09/03/2016      
        //This function will load Physical Exam Template SubCharacteristic
        public DSPhysicalExamTemplate LoadPhysicalExamTemplateSubChar(long templateId, long charId, string isViewOrder = "", string isPrintOrder = "")
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {

                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (templateId <= 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);

                if (charId <= 0)
                    dbManager.AddParameters(1, PARM_CHAR_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CHAR_ID, charId);

                ds = (DSPhysicalExamTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SUBCHAR_SELECT, ds, ds.PhysExamTemplateSubChar.TableName);
                if (charId > 0)
                {

                    DataTable dtTemp = ds.PhysExamTemplateSubChar;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplateSubChar.Rows[0][ds.PhysExamTemplateSubChar.SubCharIdColumn].ToString(), null, ds.PhysExamTemplateSubChar.Rows[0][ds.PhysExamTemplateSubChar.CharIdColumn].ToString(), isViewAction, isPrintAcion);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExam::PhysicalExamTemplate_SubCharacteristic", PROC_PHYSEXAMTEMPLATE_SUBCHAR_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        //Author: Abid Ali
        //Date: 09/03/2016            
        //OverView: Delete PhysicalExam Template SubCharacteristic
        public string DeletePhysicalExamTemplateSubChar(long templateSubCharId, long templateId, long charId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                DSPhysicalExamTemplate dsCurrentOrder = LoadPhysicalExamTemplateSubChar(templateId, charId, "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_SUBCHAR_ID, templateSubCharId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SUBCHAR_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrder.PhysExamTemplateSubChar;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, templateSubCharId.ToString(), null, templateSubCharId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                }
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamSystem::DeletePhysicalExamTemplateSubChar", PROC_PHYSEXAMTEMPLATE_SUBCHAR_DELETE, ex);
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


        #region Physical Exam Template section, char, subchar Select Methods For Data Template
        
        public DSPhysicalExamTemplate LoadPhysicalExamTemplateSectionForDataTemplate(long templateId, long systemId, string isViewOrder = "", string isPrintOrder = "")
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {

                dbManager.Open();
                dbManager.CreateParameters(2);

                if (templateId <= 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);

                if (systemId <= 0)
                    dbManager.AddParameters(1, PARM_SYSID, null);
                else
                    dbManager.AddParameters(1, PARM_SYSID, systemId);

                ds = (DSPhysicalExamTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SECTION_SELECT_FOR_DATA_TEMPLATE, ds, ds.PhysExamTemplateSection.TableName);
                if (templateId > 0)
                {

                    DataTable dtTemp = ds.PhysExamTemplateSection;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplateSection.Rows[0][ds.PhysExamTemplateSection.TemplateSectionIdColumn].ToString(), null, ds.PhysExamTemplateSection.Rows[0][ds.PhysExamTemplateSection.TemplateIdColumn].ToString(), isViewAction, isPrintAcion);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExam::PhysicalExamTemplate_Section", PROC_PHYSEXAMTEMPLATE_SECTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamTemplate LoadPhysicalExamTemplateCharForDataTemplate(long templateId, long sectionId, string isViewOrder = "", string isPrintOrder = "")
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {

                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (templateId <= 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);

                if (sectionId <= 0)
                    dbManager.AddParameters(1, PARM_SEC_ID, null);
                else
                    dbManager.AddParameters(1, PARM_SEC_ID, sectionId);

                ds = (DSPhysicalExamTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_CHAR_SELECT_FOR_DATA_TEMPLATE, ds, ds.PhysExamTemplateChar.TableName);
                if (templateId > 0)
                {

                    DataTable dtTemp = ds.PhysExamTemplateChar;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplateChar.Rows[0][ds.PhysExamTemplateChar.TemplateCharIdColumn].ToString(), null, ds.PhysExamTemplateChar.Rows[0][ds.PhysExamTemplateChar.TemplateIdColumn].ToString(), isViewAction, isPrintAcion);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExam::PhysicalExamTemplate_Characteristic", PROC_PHYSEXAMTEMPLATE_SECTION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSPhysicalExamTemplate LoadPhysicalExamTemplateSubCharForDataTemplate(long templateId, long charId, string isViewOrder = "", string isPrintOrder = "")
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {

                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (templateId <= 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);

                if (charId <= 0)
                    dbManager.AddParameters(1, PARM_CHAR_ID, null);
                else
                    dbManager.AddParameters(1, PARM_CHAR_ID, charId);

                ds = (DSPhysicalExamTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SUBCHAR_SELECT_FOR_DATA_TEMPLATE, ds, ds.PhysExamTemplateSubChar.TableName);
                if (charId > 0)
                {

                    DataTable dtTemp = ds.PhysExamTemplateSubChar;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplateSubChar.Rows[0][ds.PhysExamTemplateSubChar.SubCharIdColumn].ToString(), null, ds.PhysExamTemplateSubChar.Rows[0][ds.PhysExamTemplateSubChar.CharIdColumn].ToString(), isViewAction, isPrintAcion);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExam::PhysicalExamTemplate_SubCharacteristic", PROC_PHYSEXAMTEMPLATE_SUBCHAR_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "PhysicalExamTemplate Insert/Update/Delete/Select"

        //Author: Farooq Ahmad
        //Date: 03/03/2016
        //This function will load of PhysicalExamTemplate
        public DSPhysicalExamTemplate loadPhysicalExamTemplate(long templateId, long entityId,int? IsActive=null, string providerIds = "", string specialtyIds = "", string isViewOrder = "", string isPrintOrder = "")
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(5);

                if (templateId == 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);

                if (entityId == 0)
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);

                if (IsActive==null || IsActive < 0)
                {
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, null);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                }


                if (providerIds == "")
                {
                    providerIds = null;
                }

                if (specialtyIds == "")
                {
                    specialtyIds = null;
                }

                dbManager.AddParameters(3, PARM_TEMPLATE_PROVIDER_IDS, providerIds);
                dbManager.AddParameters(4, PARM_TEMPLATE_SPECIALTY_IDS, specialtyIds);

                ds = (DSPhysicalExamTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SELECT, ds, ds.PhysExamTemplate.TableName);
                if (templateId > 0)
                {

                    DataTable dtTemp = ds.PhysExamTemplate;
                    if (dtTemp != null)
                    {
                        if (isViewOrder == "1" || isPrintOrder == "1")
                        {
                            bool isViewAction = isViewOrder == "1" ? true : false;
                            bool isPrintAcion = isPrintOrder == "1" ? true : false;
                            dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplate.Rows[0][ds.PhysExamTemplate.TemplateIdColumn].ToString(), null, ds.PhysExamTemplate.Rows[0][ds.PhysExamTemplate.TemplateIdColumn].ToString(), isViewAction, isPrintAcion);
                            dsDBAudit.AcceptChanges();
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamTemplate::loadPhysicalExamTemplate", PROC_PHYSEXAMTEMPLATE_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        // Author: Farooq Ahmad
        // Date: 08/02/2016
        //This function will insert/update Physical Exam Template
        public DSPhysicalExamTemplate insertUpdatePhysicalExamTemplate(DSPhysicalExamTemplate ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.PhysExamTemplate.GetChanges();
                dbManager.BeginTransaction();

                this.CreatePhysicalExamTemplateInsertParameters(dbManager, ds);
                this.CreatePhysicalExamTemplateUpdateParameters(dbManager, ds);

                ds = (DSPhysicalExamTemplate)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_INSERT, PROC_PHYSEXAMTEMPLATE_UPDATE, ds, ds.PhysExamTemplate.TableName);
                if (dtTemp != null)
                {
                    int insertedRowIndex = ds.Tables[ds.PhysExamTemplate.TableName].Rows.Count - 1;
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysExamTemplate.Rows[insertedRowIndex][ds.PhysExamTemplate.TemplateIdColumn].ToString(), null, ds.PhysExamTemplate.Rows[insertedRowIndex][ds.PhysExamTemplate.TemplateIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamTemplate::insertUpdatePhysicalExamTemplate", PROC_PHYSEXAMTEMPLATE_INSERT + " " + PROC_PHYSEXAMTEMPLATE_UPDATE, ex);
                throw ex;
            }
        }

        // Author: Abid Ali
        // Date: 01/09/2016
        //This function will insert/update Physical Exam Save As Template
        public string physicalExamSaveAsTemplate(long oldTemplateId, long newTemplateId, string systemIds = "", string sectionIds = "", string charIds = "", string subCharIds = "")
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(6);

                if (oldTemplateId == 0)
                    dbManager.AddParameters(0, PARM_OLD_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_OLD_TEMPLATE_ID, oldTemplateId);

                if (newTemplateId == 0)
                    dbManager.AddParameters(1, PARM_NEW_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_NEW_TEMPLATE_ID, newTemplateId);

                if (systemIds == "")
                    dbManager.AddParameters(2, PARM_NEW_TEMPLATE_SYSTEM_IDs, null);
                else
                    dbManager.AddParameters(2, PARM_NEW_TEMPLATE_SYSTEM_IDs, systemIds);

                if (sectionIds == "")
                    dbManager.AddParameters(3, PARM_NEW_TEMPLATE_SECTION_IDs, null);
                else
                    dbManager.AddParameters(3, PARM_NEW_TEMPLATE_SECTION_IDs, sectionIds);

                if (charIds == "")
                    dbManager.AddParameters(4, PARM_NEW_TEMPLATE_CHAR_IDs, null);
                else
                    dbManager.AddParameters(4, PARM_NEW_TEMPLATE_CHAR_IDs, charIds);

                if (subCharIds == "")
                    dbManager.AddParameters(5, PARM_NEW_TEMPLATE_SUBCHAR_IDs, null);
                else
                    dbManager.AddParameters(5, PARM_NEW_TEMPLATE_SUBCHAR_IDs, subCharIds);


                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PHYSEXAM_TEMPLATE_SAVEAS);
                string Message = ((System.Data.SqlClient.SqlParameter)(dbManager.Parameters[1])).SqlValue.ToString();
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return Message;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamTemplate::physicalExamSaveAsTemplate", PROC_PHYSEXAM_TEMPLATE_SAVEAS, ex);
                return ex.Message;
            }
        }


        // Author: Abid Ali
        // Date: 02/09/2016
        public string physicalExamTemplateSecCharSubCharIsCheckedUpdate(long systemId, long templateId)
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (systemId == 0)
                    dbManager.AddParameters(0, PARM_SYSID, null);
                else
                    dbManager.AddParameters(0, PARM_SYSID, systemId);

                if (templateId == 0)
                    dbManager.AddParameters(1,PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, templateId);
              

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PHYSEXAM_SEC_CHAR_SUBCHAR_ISCHECKED_UPDATE);
                string Message = ((System.Data.SqlClient.SqlParameter)(dbManager.Parameters[1])).SqlValue.ToString();
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return Message;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamTemplate::physicalExamTemplateSecCharSubCharIsCheckedUpdate", PROC_PHYSEXAM_SEC_CHAR_SUBCHAR_ISCHECKED_UPDATE, ex);
                return ex.Message;
            }
        }

        // Author: Abid Ali
        // Date: 02/09/2016
        public string physicalExamTemplateCharSubCharIsCheckedUpdate(long sectionId, long templateId)
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (sectionId == 0)
                    dbManager.AddParameters(0, PARM_SEC_ID, null);
                else
                    dbManager.AddParameters(0, PARM_SEC_ID, sectionId);

                if (templateId == 0)
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, templateId);


                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PHYSEXAM_CHAR_SUBCHAR_ISCHECKED_UPDATE);
                string Message = ((System.Data.SqlClient.SqlParameter)(dbManager.Parameters[1])).SqlValue.ToString();
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return Message;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamTemplate::physicalExamTemplateCharSubCharIsCheckedUpdate", PROC_PHYSEXAM_CHAR_SUBCHAR_ISCHECKED_UPDATE, ex);
                return ex.Message;
            }
        }

        // Author: Abid Ali
        // Date: 02/09/2016
        public string physicalExamTemplateSubCharIsCheckedUpdate(long charId, long templateId)
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);

                if (charId == 0)
                    dbManager.AddParameters(0, PARM_CHAR_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CHAR_ID, charId);

                if (templateId == 0)
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_TEMPLATE_ID, templateId);


                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PHYSEXAM_SUBCHAR_ISCHECKED_UPDATE);
                string Message = ((System.Data.SqlClient.SqlParameter)(dbManager.Parameters[1])).SqlValue.ToString();
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return Message;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamTemplate::physicalExamTemplateSubCharIsCheckedUpdate", PROC_PHYSEXAM_SUBCHAR_ISCHECKED_UPDATE, ex);
                return ex.Message;
            }
        }

        // Author: Farooq Ahmad
        // Date: 08/02/2016
        //This function will delete Physical Exam Template
        public string deletePhysicalExamTemplate(long templateId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                DSPhysicalExamTemplate dsCurrentOrder = loadPhysicalExamTemplate(templateId, 0, null, "", "", "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_DELETE);
                string Message = ((System.Data.SqlClient.SqlParameter)(dbManager.Parameters[1])).SqlValue.ToString();
                if (Message != "" && Message != "Null")
                    throw new Exception(Message);
                else
                {
                    DataTable dtTemp = dsCurrentOrder.PhysExamTemplate;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, templateId.ToString(), null, templateId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                }
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamTemplate::deletePhysicalExamTemplate", PROC_PHYSEXAMTEMPLATE_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 0)
                {
                    string customMessage = "";
                    if (str[0].IndexOf("PhysExamDataTemplate") > -1)
                        customMessage = "This template is associated with Data Template";
                    else if (str[0].IndexOf("FK__PatientPh") > -1)
                    {
                        customMessage = "This template is associated with Physical Exam";
                    }
                    else if (str[0].IndexOf("Clinical.PhysicalExam_UserSystem") > -1)
                    {
                        customMessage = "This template is associated with Physical Exam";
                    }
                    else
                        customMessage = str[0].ToString();
                    return customMessage;
                }
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }



        #endregion

        #region PE Revamp, MK

        public DSPhysicalExamTemplate loadPhysicalExamTemplatesECW(long templateId, long entityId, int? IsActive = null)
        {
            DSPhysicalExamTemplate ds = new DSPhysicalExamTemplate();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();
            try
            {
                dbManager.CreateParameters(5);

                if (templateId == 0)
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_TEMPLATE_ID, templateId);

                if (entityId == 0)
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);

                if (IsActive == null || IsActive < 0)
                {
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, null);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                }

                ds = (DSPhysicalExamTemplate)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PHYSEXAMTEMPLATE_SELECTECW, ds, ds.PhysExamTemplate.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                MDVLogger.DALErrorLog("DALPhysicalExamTemplate::loadPhysicalExamTemplatesECW", PROC_PHYSEXAMTEMPLATE_SELECTECW, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        #endregion

        #region Inser PhysicalExam System, section, Characteristics, SubCharacteristics

        // Author: Farooq Ahmad
        // Date: 15/03/2016
        //This function will insert/update Physical Exam Template
        public DSPhysicalExamLookup insertPhysicalExamSystem(DSPhysicalExamLookup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.PhysicalExamSystem.GetChanges();
                dbManager.BeginTransaction();

                this.CreatePhysicalExamSystemInsertParameters(dbManager, ds);

                ds = (DSPhysicalExamLookup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PHYSEXAM_SYS_INSERT, ds, ds.PhysicalExamSystem.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.PhysicalExamSystem.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.PhysicalExamSystem.Rows[i][ds.PhysicalExamSystem.SystemIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysicalExamSystem.Rows[0][ds.PhysicalExamSystem.SystemIdColumn].ToString(), null, ds.PhysicalExamSystem.Rows[0][ds.PhysicalExamSystem.TemplateIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamTemplate::insertPhysicalExamSystem", PROC_PHYSEXAM_SYS_INSERT, ex);
                throw ex;
            }
        }

        // Author: Farooq Ahmad
        // Date: 15/03/2016
        //This function will insert/update Physical Exam Template
        public DSPhysicalExamLookup insertPhysicalExamSystemSection(DSPhysicalExamLookup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.PhysicalExamSystemSection.GetChanges();
                dbManager.BeginTransaction();
                this.CreatePhysicalExamSystemSectionInsertParameters(dbManager, ds);

                ds = (DSPhysicalExamLookup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PHYSEXAM_SECTION_INSERT, ds, ds.PhysicalExamSystemSection.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.PhysicalExamSystemSection.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.PhysicalExamSystemSection.Rows[i][ds.PhysicalExamSystemSection.SectionIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysicalExamSystemSection.Rows[0][ds.PhysicalExamSystemSection.SectionIdColumn].ToString(), null, ds.PhysicalExamSystemSection.Rows[0][ds.PhysicalExamSystemSection.PhysicalExamSystemIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamTemplate::insertPhysicalExamSystemSection", PROC_PHYSEXAM_SECTION_INSERT, ex);
                throw ex;
            }
        }

        // Author: Farooq Ahmad
        // Date: 15/03/2016
        //This function will insert/update Physical Exam Template
        public DSPhysicalExamLookup insertPhysicalExamSystemSectionCharacteristics(DSPhysicalExamLookup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.PhysicalExamSystemSectionCharacteristic.GetChanges();
                dbManager.BeginTransaction();
                this.CreatePhysicalExamSystemSectionCharacteristicsInsertParameters(dbManager, ds);

                ds = (DSPhysicalExamLookup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PHYSEXAM_CHAR_INSERT, ds, ds.PhysicalExamSystemSectionCharacteristic.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.PhysicalExamSystemSectionCharacteristic.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.PhysicalExamSystemSectionCharacteristic.Rows[i][ds.PhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn];
                    }
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysicalExamSystemSectionCharacteristic.Rows[0][ds.PhysicalExamSystemSectionCharacteristic.CharacteristicIdColumn].ToString(), null, ds.PhysicalExamSystemSectionCharacteristic.Rows[0][ds.PhysicalExamSystemSectionCharacteristic.SectionIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamTemplate::insertPhysicalExamSystemSectionCharacteristics", PROC_PHYSEXAM_CHAR_INSERT, ex);
                throw ex;
            }
        }

        // Author: Farooq Ahmad
        // Date: 15/03/2016
        //This function will insert/update Physical Exam Template
        public DSPhysicalExamLookup insertPhysicalExamSystemSectionCharacteristicsSubCharacteristic(DSPhysicalExamLookup ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.GetChanges();
                dbManager.BeginTransaction();
                this.CreatePhysicalExamSystemSectionCharacteristicsSubCharacteristicInsertParameters(dbManager, ds);

                ds = (DSPhysicalExamLookup)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PHYSEXAM_SUBCHAR_INSERT, ds, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.TableName);
                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.Rows[i][ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn];
                    }
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.Rows[0][ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.SubCharacteristicIdColumn].ToString(), null, ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.Rows[0][ds.PhysicalExamSystemSectionCharacteristicSubCharacteristic.CharacteristicIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALPhysicalExamTemplate::insertPhysicalExamSystemSectionCharacteristicsSubCharacteristic", PROC_PHYSEXAM_SUBCHAR_INSERT, ex);
                throw ex;
            }
        }





        #endregion


    }
}
