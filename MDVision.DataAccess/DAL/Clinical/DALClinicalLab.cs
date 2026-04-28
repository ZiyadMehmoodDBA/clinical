/* Author:  Muhammad Arshad
 * Created Date: 05/04/2016
 * OverView: Created for Lab in Admin Clinical Module
 */

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

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALClinicalLab
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"

        private const string PROC_CLINICAL_LAB_INSERT = "Clinical.sp_LabInsert";
        private const string PROC_CLINICAL_LAB_UPDATE = "Clinical.sp_LabUpdate";
        private const string PROC_CLINICAL_LAB_DELETE = "Clinical.sp_LabDelete";
        private const string PROC_CLINICAL_LAB_SELECT = "Clinical.sp_LabSelect";

        //Start//Abid Ali// Optimization
        private const string PROC_CLINICAL_LAB_SELECT_LOOKUP = "Clinical.sp_LabLookup";
        //End//Abid Ali// Optimization


        //Ahsan Nasir
        private const string PROC_CLINICAL_LAB_CATEGORY_LOOKUP = "Clinical.sp_LabCategoryLookup";

        private const string PROC_CLINICAL_LAB_Type_LOOKUP = "Clinical.sp_LabTypeLookup";

        private const string PROC_CLINICAL_LAB_CodeSystem_LOOKUP = "Clinical.sp_LabCodeSystemLookup";

        private const string PROC_CLINICAL_LAB_RequisitionTemplate_LOOKUP = "Clinical.sp_LabRequisitionTemplateLookup";


        private const string PROC_CLINICAL_LAB_TEST_INSERT = "Clinical.sp_LabTestInsert";
        private const string PROC_CLINICAL_LAB_TEST_UPDATE = "Clinical.sp_LabTestUpdate";
        private const string PROC_CLINICAL_LAB_TEST_DELETE = "Clinical.sp_LabTestDelete";
        private const string PROC_CLINICAL_LAB_TEST_SELECT = "Clinical.sp_LabTestSelect";

        private const string PROC_CLINICAL_LAB_TEST_ATTRIBUTES_INSERT = "Clinical.sp_LabTestAttributesInsert";
        private const string PROC_CLINICAL_LAB_TEST_ATTRIBUTES_UPDATE = "Clinical.sp_LabTestAttributesUpdate";
        private const string PROC_CLINICAL_LAB_TEST_ATTRIBUTES_DELETE = "Clinical.sp_LabTestAttributesDelete";
        private const string PROC_CLINICAL_LAB_TEST_ATTRIBUTES_SELECT = "Clinical.sp_LabTestAttributesSelect";

        private const string PROC_CLINICAL_LAB_TEST_ATTRIBUTES_RESULT_SELECT = "Clinical.sp_LabTestAttributesResultSelect";
        private const string PROC_CLINICAL_LAB_TEST_ATTRIBUTES_RESULT_DELETE = "Clinical.sp_LabTestAttributesResultDelete";
        private const string PROC_CLINICAL_LAB_TEST_ATTRIBUTES_RESULT_INSERT = "Clinical.sp_LabTestAttributesResultInsert";
        #endregion

        #region "Parameters"

        private const string PARM_LAB_Id = "@LabId";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_NAME = "@Name";
        private const string PARM_TYPE = "@Type";
        private const string PARM_CLIENT_NO = "@ClientNo";

        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";

        private const string PARM_MODIFIED_On = "@ModifiedOn";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_LABTEST_ID = "@LabTestId";
        private const string PARM_LOINC = "@LOINC";
        private const string PARM_LOINC_DESCRIPTION = "@LOINCDescription";
        private const string PARM_IS_TEMPLATE = "@IsTemplate";
        private const string PARM_LAB_TEST_ATTRIBUTE_ID = "@LabTestAttributeId";
        private const string PARM_ATTRIBUTE_NAME = "@AttributeName";
        private const string PARM_RESULT_NAME = "@ResultName";
        private const string PARM_LAB_TEST_ATTRIBUTE_RESULT_ID = "@LabTestAttributeResultId";
        private const string PARM_UoM = "@UoM";
        private const string PARM_RANGE = "@Range";
        private const string PARM_DESCRIPTION = "@Description";

        #endregion

        #region Constructors
        public DALClinicalLab()
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

        /// <summary>
        /// Metod Name: CreateUpdateParameters
        /// Author Name: Abid Ali
        /// Created Date: 05-04-2016
        /// Description: This function will Create Parameters.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private void CreateUpdateParameters(IDBManager dbManager, DSClinicalLab ds, Boolean IsInsert)
        {

            if (IsInsert)
            {
                dbManager.CreateInsertParameters(15);
                dbManager.AddInsertUpdateParameters(0, PARM_LAB_Id, ds.Lab.LabIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(15);
                dbManager.AddInsertUpdateParameters(0, PARM_LAB_Id, ds.Lab.LabIdColumn.ColumnName, DbType.Int64);
            }

            dbManager.AddInsertUpdateParameters(1, PARM_NAME, ds.Lab.NameColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(2, PARM_TYPE, ds.Lab.TypeColumn.ColumnName, DbType.String);

            if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                dbManager.AddInsertUpdateParameters(3, PARM_ENTITY_ID, ds.Lab.EntityIdColumn.ColumnName, DbType.Int64);

            else
                dbManager.AddInsertUpdateParameters(3, PARM_ENTITY_ID, ds.Lab.EntityIdColumn.ColumnName, DbType.Int64);

            dbManager.AddInsertUpdateParameters(4, PARM_COMMENTS, ds.Lab.CommentsColumn.ColumnName, DbType.String);

            dbManager.AddInsertUpdateParameters(5, PARM_IS_ACTIVE, ds.Lab.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_BY, ds.Lab.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(7, PARM_CREATED_ON, ds.Lab.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFIED_BY, ds.Lab.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(9, PARM_MODIFIED_On, ds.Lab.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(10, PARM_CLIENT_NO, ds.Lab.ClientNoColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, "@CategoryId", ds.Lab.CategoryIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(12, "@CodeSystemId", ds.Lab.CodeSystemIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(13, "@RequisitionTemplateId", ds.Lab.RequisitionTemplateIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(14, "@LabTypeId", ds.Lab.LabTypeIdColumn.ColumnName, DbType.Int64);

        }

        public DSClinicalLab loadClinicalLab(long labId, string name, string labType, long entityId, byte isActive, Int32 PageNumber, Int32 RowsPerPage)
        {
            DSClinicalLab ds = new DSClinicalLab();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(8);

                if (labId <= 0)
                    dbManager.AddParameters(0, PARM_LAB_Id, null);
                else
                    dbManager.AddParameters(0, PARM_LAB_Id, labId);

                if (entityId <= 0)
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, entityId);

                if (string.IsNullOrEmpty(name))
                    dbManager.AddParameters(2, PARM_NAME, null);
                else
                    dbManager.AddParameters(2, PARM_NAME, name);

                if (string.IsNullOrEmpty(labType))
                    dbManager.AddParameters(3, PARM_TYPE, null);
                else
                    dbManager.AddParameters(3, PARM_TYPE, labType);

                if (isActive == 1)
                    dbManager.AddParameters(4, PARM_IS_ACTIVE, 1);
                else if (isActive == 0)
                    dbManager.AddParameters(4, PARM_IS_ACTIVE, 0);
                else
                    dbManager.AddParameters(4, PARM_IS_ACTIVE, null);

                if (PageNumber <= 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);

                if (RowsPerPage <= 0)
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, 2000);
                else
                    dbManager.AddParameters(6, PARM_ROWS_PER_PAGE, RowsPerPage);

                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.Lab.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSClinicalLab)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_SELECT, ds, ds.Lab.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::LoadClinicalLab", PROC_CLINICAL_LAB_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Metod Name: insertClinicalLab
        /// Author Name: Abid Ali
        /// Created Date: 05-04-2016
        /// Description: This function will insert/update Clinical Lab
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSClinicalLab insertUpdateClinicalLab(DSClinicalLab ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.Open();
                CreateUpdateParameters(dbManager, ds, true);
                CreateUpdateParameters(dbManager, ds, false);
                ds = (DSClinicalLab)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_INSERT, PROC_CLINICAL_LAB_UPDATE, ds, ds.Lab.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::inserUpdatetClinicalLab", PROC_CLINICAL_LAB_INSERT, ex);
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


        /// <summary>
        /// Metod Name: deleteClinicalLab
        /// Author Name: Abid Ali
        /// Created Date: 05-04-2016
        /// Description: This function will delete Clinical Lab
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public string deleteClinicalLab(long labId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_LAB_Id, labId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CLINICAL_LAB_DELETE).ToString();

                if (returnVal != null && returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab :: deleteClinicalLab", PROC_CLINICAL_LAB_DELETE, ex);
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

        //Start//Abid Ali// For Optimization puprose
        #region Labs Lookups
        public DSClinicalLab getLabsLookup()
        {
            DSClinicalLab ds = new DSClinicalLab();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalLab)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_SELECT_LOOKUP, ds, ds.LabLookup.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::getLabsLookup", PROC_CLINICAL_LAB_SELECT_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSClinicalLab LabCategoryLookup()
        {
            DSClinicalLab ds = new DSClinicalLab();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalLab)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_CATEGORY_LOOKUP, ds, ds.LabCategory.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::LabsCategoryLookup", PROC_CLINICAL_LAB_CATEGORY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSClinicalLab LabTypeLookup()
        {
            DSClinicalLab ds = new DSClinicalLab();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalLab)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_Type_LOOKUP, ds, ds.LabType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::LabsTypeLookup", PROC_CLINICAL_LAB_Type_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalLab LabRequisitionTemplateLookup()
        {
            DSClinicalLab ds = new DSClinicalLab();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalLab)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_RequisitionTemplate_LOOKUP, ds, ds.LabRequisitionTemplate.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::LabsRequisitionTemplateLookup", PROC_CLINICAL_LAB_RequisitionTemplate_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSClinicalLab LabCodeSystemLookup()
        {
            DSClinicalLab ds = new DSClinicalLab();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSClinicalLab)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_CodeSystem_LOOKUP, ds, ds.LabCodeSystem.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::LabsCodeSystemLookup", PROC_CLINICAL_LAB_CodeSystem_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion
        //End//Abid Ali// For Optimization puprose


        #region "Lab Test"
        private void CreateParametersLabTest(IDBManager dbManager, DSClinicalLab ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(10);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_LABTEST_ID, ds.LabTest.LabTestIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_LABTEST_ID, ds.LabTest.LabTestIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_LAB_Id, ds.LabTest.LabIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_LOINC, ds.LabTest.LOINCColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_LOINC_DESCRIPTION, ds.LabTest.LOINCDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_TEMPLATE, ds.LabTest.IsTemplateColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.LabTest.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.LabTest.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.LabTest.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.LabTest.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFIED_On, ds.LabTest.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        public DSClinicalLab LoadLabTest(long LabTestId, long LabId, string LOINC = "", string IsActive = "", string LOINCDescription = "")
        {
            DSClinicalLab ds = new DSClinicalLab();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (LOINC == "")
                    LOINC = null;
                if (LOINCDescription == "")
                    LOINCDescription = null;
                if (IsActive == "")
                    IsActive = null;
                dbManager.Open();
                dbManager.CreateParameters(5);

                if (LabTestId == 0)
                    dbManager.AddParameters(0, PARM_LABTEST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LABTEST_ID, LabTestId);
                if (LabId == 0)
                    dbManager.AddParameters(1, PARM_LAB_Id, null);
                else
                    dbManager.AddParameters(1, PARM_LAB_Id, LabId);

                dbManager.AddParameters(2, PARM_LOINC, LOINC);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);
                dbManager.AddParameters(4, PARM_LOINC_DESCRIPTION, LOINCDescription);


                ds = (DSClinicalLab)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_TEST_SELECT, ds, ds.LabTest.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::LoadLabTest", PROC_CLINICAL_LAB_TEST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSClinicalLab UpdateLabTest(DSClinicalLab ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersLabTest(dbManager, ds, false);
                ds = (DSClinicalLab)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_TEST_UPDATE, ds, ds.LabTest.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::UpdateLabTest", PROC_CLINICAL_LAB_TEST_UPDATE, ex);
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
        public string DeleteLabTest(string LabTestId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_LABTEST_ID, LabTestId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CLINICAL_LAB_TEST_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::DeleteLabTest", PROC_CLINICAL_LAB_TEST_DELETE, ex);
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
        public DSClinicalLab InsertLabTest(DSClinicalLab ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersLabTest(dbManager, ds, true);
                ds = (DSClinicalLab)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_TEST_INSERT, ds, ds.LabTest.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::InsertLabTest", PROC_CLINICAL_LAB_TEST_INSERT, ex);
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

        #endregion

        #region "Lab Test Attributes"
        private void CreateParametersLabTestAttribute(IDBManager dbManager, DSClinicalLab ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(11);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_LAB_TEST_ATTRIBUTE_ID, ds.LabTestAttributes.LabTestAttributeIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_LAB_TEST_ATTRIBUTE_ID, ds.LabTestAttributes.LabTestAttributeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_LABTEST_ID, ds.LabTestAttributes.LabTestIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ATTRIBUTE_NAME, ds.LabTestAttributes.AttributeNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_UoM, ds.LabTestAttributes.UoMColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_RANGE, ds.LabTestAttributes.RangeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DESCRIPTION, ds.LabTestAttributes.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.LabTestAttributes.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.LabTestAttributes.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.LabTestAttributes.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.LabTestAttributes.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_On, ds.LabTestAttributes.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        public DSClinicalLab LoadLabTestAttribute(long LabTestAttributeId, long LabTestId)
        {
            DSClinicalLab ds = new DSClinicalLab();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (LabTestAttributeId == 0)
                    dbManager.AddParameters(0, PARM_LAB_TEST_ATTRIBUTE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_LAB_TEST_ATTRIBUTE_ID, LabTestAttributeId);
                if (LabTestId == 0)
                    dbManager.AddParameters(1, PARM_LABTEST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_LABTEST_ID, LabTestId);
                List<string> tableNames = new List<string>();
                tableNames.Add(ds.LabTestAttributes.TableName);
                tableNames.Add(ds.LabTestAttributeResult.TableName);
                ds = (DSClinicalLab)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_TEST_ATTRIBUTES_SELECT, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::LoadLabTestAttribute", PROC_CLINICAL_LAB_TEST_ATTRIBUTES_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSClinicalLab UpdateLabTestAttribute(DSClinicalLab ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParametersLabTestAttribute(dbManager, ds, false);
                ds = (DSClinicalLab)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_TEST_ATTRIBUTES_UPDATE, ds, ds.LabTestAttributes.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::UpdateLabTestAttribute", PROC_CLINICAL_LAB_TEST_ATTRIBUTES_UPDATE, ex);
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
        public string DeleteLabTestAttribute(string LabTestAttributeId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_LAB_TEST_ATTRIBUTE_ID, LabTestAttributeId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CLINICAL_LAB_TEST_ATTRIBUTES_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::DeleteLabTestAttribute", PROC_CLINICAL_LAB_TEST_ATTRIBUTES_DELETE, ex);
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
        public DSClinicalLab InsertLabTestAttribute(DSClinicalLab ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersLabTestAttribute(dbManager, ds, true);
                ds = (DSClinicalLab)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_TEST_ATTRIBUTES_INSERT, ds, ds.LabTestAttributes.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::InsertLabTestAttribute", PROC_CLINICAL_LAB_TEST_ATTRIBUTES_INSERT, ex);
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
        #endregion
        #region "Lab Test Attribute Result"
        public DSClinicalLab GetLabTestAttributeResult(long LabTestAttributeId)
        {
            DSClinicalLab ds = new DSClinicalLab();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_LAB_TEST_ATTRIBUTE_ID, LabTestAttributeId);
                ds = (DSClinicalLab)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_TEST_ATTRIBUTES_RESULT_SELECT, ds, ds.LabTestAttributeResult.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::GetLabTestAttributeResult", PROC_CLINICAL_LAB_TEST_ATTRIBUTES_RESULT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteLabTestAttributeResult(long LabTestAttributeResultId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_LAB_TEST_ATTRIBUTE_RESULT_ID, LabTestAttributeResultId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CLINICAL_LAB_TEST_ATTRIBUTES_RESULT_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::DeleteLabTestAttributeResult", PROC_CLINICAL_LAB_TEST_ATTRIBUTES_RESULT_DELETE, ex);
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
        public DSClinicalLab InsertLabTestAttributeResult(DSClinicalLab ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_LAB_TEST_ATTRIBUTE_RESULT_ID, ds.LabTestAttributeResult.LabTestAttributeResultIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(1, PARM_LAB_TEST_ATTRIBUTE_ID, ds.LabTestAttributeResult.LabTestAttributeIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, PARM_RESULT_NAME, ds.LabTestAttributeResult.ResultNameColumn.ColumnName, DbType.String);
                ds = (DSClinicalLab)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CLINICAL_LAB_TEST_ATTRIBUTES_RESULT_INSERT, ds, ds.LabTestAttributeResult.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALClinicalLab::InsertLabTestAttributeResult", PROC_CLINICAL_LAB_TEST_ATTRIBUTES_RESULT_INSERT, ex);
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

        #endregion

    }
}
