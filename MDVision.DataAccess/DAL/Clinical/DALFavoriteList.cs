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
using MDVision.IEHR.EMR.Model.FavoriteList;
using MDVision.Common.Utilities;
using MDVision.Model.Clinical.Favorites;
using MDVision.Model.Clinical.Orthopedic;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALFavoriteList
    {
        #region Variable

        #endregion


        #region "Stored Procedure Names"


        private const string PROC_FAV_IMMUNIZATION_DELETE = "Clinical.sp_ImmunizationFavoritesDelete";
        private const string PROC_FAV_IMMUNIZATION_SELECT = "Clinical.sp_ImmunizationFavoritesSelect";
        private const string PROC_FAV_IMMUNIZATION_VACCINE_SELECT = "Clinical.sp_ImmunizationFavoritesVaccineSelect";
        private const string PROC_FAV_IMMUNIZATION_LOOKUP = "Clinical.sp_ImmunizationFavoritesLookup";
        private const string PROC_FAV_IMMUNIZATION_CLINICAL_LOOKUP = "Clinical.sp_ImmunizationFavoritesClinicalLookup";
        private const string PROC_FAV_VACCINE_INSERT = "Clinical.sp_ImmunizationFavoritesInsert";
        private const string PROC_FAV_VACCINE_UPDATE = "Clinical.sp_ImmunizationFavoritesUpdate";
        private const string PROC_GET_VACCINE_CVX_ADM_CODE = "Clinical.sp_GetCVXAndAdministeredCode";
        private const string PROC_FAVORITE_LIST_ICD_SELECT = "Clinical.sp_FavoriteListICDSelect";
        private const string PROC_FAVORITE_LIST_CUSTOMFORM_SELECT = "Clinical.sp_FavoriteListCustomFormsSelect";
        private const string PROC_FAVORITE_LIST_SELECT = "Clinical.sp_FavoriteListSelect";
        private const string PROC_FAVORITE_LIST_ORTHO_SELECT = "Clinical.sp_FavoriteListOrthoSelect";
        private const string PROC_FAVORITE_LIST_PROVIDER_SELECT = "Clinical.sp_FavoriteListProviderSelect";
        //private const string PROC_FAVORITE_LIST_FORMS_SELECT = "Clinical.sp_FavoriteListCustomFormSelect";
        private const string PROC_FAVORITE_LIST_CPT_DELETE = "Clinical.sp_FavoriteListCPTDelete";
        private const string PROC_FAVORITE_LIST_CPT_INSERT = "Clinical.sp_FavoriteListCPTInsert";
        private const string PROC_FAVORITE_LIST_CPT_UPDATE = "Clinical.sp_FavoriteListCPTUpdate";
        private const string PROC_FAVORITE_LIST_CPT_SELECT = "Clinical.sp_FavoriteListCPTSelect";
        private const string PROC_FAVORITE_LIST_DELETE = "Clinical.sp_FavoriteListDelete";

        private const string PROC_FAVORITE_LIST_CUSTOMFORMS_DELETE = "Clinical.sp_FavoriteListCustomFormsDelete";
        private const string PROC_FAVORITE_LIST_ICD_DELETE = "Clinical.sp_FavoriteListICDDelete";
        private const string PROC_FAVORITE_LIST_ICD_INSERT = "Clinical.sp_FavoriteListICDInsert";
        private const string PROC_FAVORITE_LIST_ICD_UPDATE = "Clinical.sp_FavoriteListICDUpdate";
        private const string PROC_FAVORITE_LIST_CUSTOMFORMS_UPDATE = "Clinical.sp_FavoriteListCustomFormUpdate";
        private const string PROC_FAVORITE_LIST_NAME_LOOKUP = "Clinical.sp_FavoriteListNameLookup";

        private const string PROC_FAVORITE_LIST_UPDATE = "Clinical.sp_FavoriteListUpdate";
        private const string PROC_FAVORITE_LIST_INSERT = "[Clinical].[sp_FavoriteListInsert]";


        private const string PROC_FAVORITE_LIST_INSERT_ICD = "[Clinical].[sp_FavoriteListICDInsert]";
        private const string PROC_FAVORITE_LIST_INSERT_CUSTOMFORMS = "[Clinical].[sp_FavoriteListCustomFormInsert]";
        private const string PROC_FAVORITE_LIST_INSERT_PROVIDERS = "Clinical.sp_FavoriteListProviderInsert";
        private const string PROC_FAVORITE_LIST_UPDATE_PROVIDERS = "Clinical.sp_FavoriteListProviderUpdate";

        private const string PROC_FAV_MEDICATION_SELECT = "Clinical.sp_FavMedicationSelect";
        private const string PROC_FAV_MEDICATION_DETAIL_INSERT = "Clinical.sp_FavMedicationDetailInsert";
        private const string PROC_FAV_MEDICATION_DETAIL_UPDATE = "Clinical.sp_FavMedicationDetailUpdate";
        private const string PROC_FAV_MEDICATION_DETAIL_SELECT = "Clinical.sp_FavMedicationDetailSelect";
        private const string PROC_FAV_MEDICATION_DETAIL_DELETE = "Clinical.sp_FavMedicationDetailDelete";

        private const string PROC_GET_FAVORITE_LIST_VAL = "Clinical.sp_GetFavoriteListVal";
        #endregion

        #region "Parameters"

        private const string PARM_TYPE = "@Type";
        private const string PARM_VACCINE_ID = "@VaccineId";
        private const string PARM_THERAPUETIC_ID = "@TherapueticId";
        private const string PARM_FAVORITE_LIST_CPT_ID = "@FavoriteListCPTId";
        private const string PARM_FAVORITE_LIST_MEDICATION_ID = "@Fav_MedicationId";
        private const string PARM_FAVORITE_LIST_ID = "@FavoriteListId";
        private const string PARM_LIST_TYPE = "@ListType";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PER_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_FAVORITE_LIST_ICD_ID = "@FavoriteListICDId";
        private const string PARM_FAVORITE_LIST_CUSTOMFORM_ID = "@FavoriteListCustomFormId";
        private const string PARM_LAB_ID = "@LabId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_IS_SELECT_FOR_LOOKUP = "@IsSelectForLookUp";
        private const string PARM_CUSTOMFORM_IDS = "@CustomFormId";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_MODIFY_ON = "@ModifiedOn";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_FAVORITELIST_ID = "@FavoriteListId";
        private const string PARM_CUSTOMFORM_ID = "@CustomFormId";
        private const string PARM_FAVORITELIST_NAME = "@FavoriteListName";
        private const string PARM_FAVORITELIST_VALUE = "@FavoriteListVal";
        private const string PARM_NAME = "@Name";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_ICD9Code = "@ICD9Code";
        private const string PARM_CUSTOMFORM_NAME = "@CustomFormName";
        private const string PARM_ICD9CodeDescription = "@ICD9CodeDescription";
        private const string PARM_ICD10Code = "@ICD10Code";
        private const string PARM_ICD10CodeDescription = "@ICD10CodeDescription";
        private const string PARM_SNOMEDID = "@SNOMEDID";
        private const string PARM_SNOMEDDescription = "@SNOMEDDescription";
        private const string PARM_LexiCode = "@LexiCode";
        private const string PARM_LexiCodeDescription = "@LexiCodeDescription";
        private const string PARM_BODYPART_ID = "@BodyPartId";

        private const string PARM_CPTCode = "@CPTCode";
        private const string PARM_CPTDescription = "@CPTCodeDescription";
        private const string PARM_UserID = "@UserId";
        private const string PARM_FAV_BODY_PART_IDs = "@BodyPartIds";
        

        private const string PARM_FAV_LIST_ID = "@FavListId";
        //Fav immunization
        private const string PARM_FAVORITIESLIST_ID = "@FavoritiesListId";
        private const string PARM_FAVORITIESLIST_NAME = "@FavoritiesListName";
        private const string PARM_PROVIDER_IDS = "@ProviderIds";
        private const string PARM_VACCINE_GROUPID = "@VaccineGroupId";
        private const string PARM_VACCINE_IDS = "@VaccineIds";
        private const string PARM_THERAPUETICINJECTION_IDS = "@TherapueticInjectionIds";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_TAB = "@Tab";
        private const string PARM_CATEGORY_ID = "@CategoryId";

        private const string PARM_UNIT = "@Unit";
        private const string PARM_MODIFIER = "@Modifier";
        private const string PARM_SEARCH_DATA = "@SearchData";

        #region Medication Detail
        private const string PARM_ID = "@Id";
        private const string PARM_ACTION = "@Action";
        private const string PARM_DOSE = "@Dose";
        private const string PARM_DOSE_UNIT = "@DoseUnit";
        private const string PARM_ROUTE = "@Route";
        private const string PARM_DOSE_TIMING = "@DoseTiming";
        private const string PARM_DOSE_OTHER = "@Doseother";
        private const string PARM_DURATION = "@Duration";
        private const string PARM_QUANTITY = "@Quantity";
        private const string PARM_QUANTITY_UNIT = "@QuantityUnit";
        private const string PARM_REFILL = "@Refill";
        private const string PARM_DIRECTIONS_TO_PHARMACIST = "@DirectionsToPharmacist";
        private const string PARM_ADD_DIRECTION_TO_PATIENT = "@AddDirectionToPatient";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_NDCID = "@NDCID";
        private const string PARM_BRAND_NAME = "@BrandName";
        private const string PARM_GENERIC_NAME = "@GenericName";
        private const string PARM_FORM = "@Form";
        private const string PARM_STRENGTH = "@Strength";
        private const string PARM_PRESCRIPTIONS_OTHER_NOTES = "@PrescriptionsOtherNotes";
        #endregion

        #endregion

        #region Constructors
        public DALFavoriteList()
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

        private void createFavVaccineParameters(IDBManager dbManager, FavoriteListImmunizationModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_FAVORITIESLIST_ID, model.FavoritiesListId, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_FAVORITIESLIST_ID, model.FavoritiesListId, DbType.Int64);
            dbManager.AddParameters(PARM_FAVORITIESLIST_NAME, model.FavoriteListName);
            dbManager.AddParameters(PARM_PROVIDER_IDS, model.ProviderIds);
            if (MDVUtility.ToInt64(model.VaccineGroupId) == 0)
            {
                dbManager.AddParameters(PARM_VACCINE_GROUPID, null);
            }
            else
            {
                dbManager.AddParameters(PARM_VACCINE_GROUPID, MDVUtility.ToInt64(model.VaccineGroupId));
            }

            dbManager.AddParameters(PARM_VACCINE_IDS, model.VaccineIds);
            dbManager.AddParameters(PARM_THERAPUETICINJECTION_IDS, model.TherapueticIds);
            dbManager.AddParameters(PARM_IS_ACTIVE, model.IsActive);
            dbManager.AddParameters(PARM_TYPE, model.Type);
            dbManager.AddParameters(PARM_CREATED_ON, DateTime.Now);
            dbManager.AddParameters(PARM_CREATED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            dbManager.AddParameters(PARM_MODIFIED_ON, DateTime.Now);
            dbManager.AddParameters(PARM_MODIFIED_BY, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
            if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                dbManager.AddParameters(PARM_ENTITY_ID, MDVUtility.ToInt64(model.EntityId));
            else
                dbManager.AddParameters(PARM_ENTITY_ID, MDVSession.Current.EntityId);
        }
        #region "Insert, delete, update and get FavoriteList using dataset Functions"
        public DSFavoriteList loadClinical_FavoriteList(long FavoriteListId, string ListType, long? EntityId, byte IsActive, Int32 PageNumber, Int32 RowsPerPage, string isViewOrder = "", string isPrintOrder = "", long LabId = 0, long ProviderId = 0, bool IsSelectForUpdate = false, bool IsSelectForLookUp = false)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSFavoriteList ds = new DSFavoriteList();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(12);

                if (FavoriteListId == 0)
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, FavoriteListId);
                if (string.IsNullOrEmpty(ListType))
                    dbManager.AddParameters(1, PARM_LIST_TYPE, null);
                else
                    dbManager.AddParameters(1, PARM_LIST_TYPE, ListType);
                if (EntityId <= 0)
                    dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(2, PARM_ENTITY_ID, EntityId);

                if (IsActive > 1)
                {
                    dbManager.AddParameters(3, PARM_IS_ACTIVE, null);
                }
                else
                {
                    dbManager.AddParameters(3, PARM_IS_ACTIVE, IsActive);
                }
                if (PageNumber <= 0)
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(4, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage <= 0)
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, 2000);
                else
                    dbManager.AddParameters(5, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.FavoriteList.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                if (LabId > 0)
                {
                    dbManager.AddParameters(7, PARM_LAB_ID, LabId);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_LAB_ID, null);
                }
                if (ProviderId > 0)
                {
                    dbManager.AddParameters(8, PARM_PROVIDER_ID, ProviderId);
                }
                else
                {
                    dbManager.AddParameters(8, PARM_PROVIDER_ID, null);
                }
                dbManager.AddParameters(9, PARM_UserID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(10, "@IsSelectForUpdate",IsSelectForUpdate);
                dbManager.AddParameters(11, PARM_IS_SELECT_FOR_LOOKUP, IsSelectForLookUp);
                ds = (DSFavoriteList)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_SELECT, ds, ds.FavoriteList.TableName);
                DataTable dtTemp = ds.FavoriteList;
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    if (isViewOrder == "1" || isPrintOrder == "1")
                    {
                        bool isViewAction = isViewOrder == "1" ? true : false;
                        bool isPrintAcion = isPrintOrder == "1" ? true : false;
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FavoriteList.Rows[0][ds.FavoriteList.FavoriteListIdColumn].ToString(), null, ds.FavoriteList.Rows[0][ds.FavoriteList.FavoriteListIdColumn].ToString(), isViewAction, isPrintAcion);
                        dsDBAudit.AcceptChanges();
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::LoadClinical_FavoriteList", PROC_FAVORITE_LIST_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<OrthoFavListModel> GetOrthopedicFavoriteList(string FavListType, long ProviderId, string FavListBodyPartIds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(PARM_LIST_TYPE, FavListType);
                dbManager.AddParameters(PARM_FAV_BODY_PART_IDs, FavListBodyPartIds);
                dbManager.AddParameters(PARM_ENTITY_ID, MDVUtility.ToInt64(MDVSession.Current.EntityId));
                dbManager.AddParameters(PARM_UserID, MDVUtility.ToInt64(MDVSession.Current.AppUserId));
                return dbManager.ExecuteReaders<OrthoFavListModel>(PROC_FAVORITE_LIST_ORTHO_SELECT);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::GetOrthopedicFavoriteList", PROC_FAVORITE_LIST_ORTHO_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFavoriteList loadClinical_FavoriteListProvider(long FavoriteListId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSFavoriteList ds = new DSFavoriteList();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, FavoriteListId);
                ds = (DSFavoriteList)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_PROVIDER_SELECT, ds, ds.FavoriteListProvider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::loadClinical_FavoriteListProvider", PROC_FAVORITE_LIST_PROVIDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSFavoriteList loadClinical_FavoriteListCustomForms(long FavoriteListId)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSFavoriteList ds = new DSFavoriteList();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, FavoriteListId);
                ds = (DSFavoriteList)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_CUSTOMFORM_SELECT, ds, ds.FavoriteListCustomForm.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::loadClinical_FavoriteListProvider", PROC_FAVORITE_LIST_CUSTOMFORM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFavoriteList loadClinical_FavoriteListICD(long FavoriteListICDId, long FavoriteListId, byte IsActive, Int32 PageNumber, Int32 RowsPerPage, string SearchData = "")
        {
            DSFavoriteList ds = new DSFavoriteList();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);

                if (FavoriteListICDId <= 0)
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_ICD_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_ICD_ID, FavoriteListICDId);

                if (FavoriteListId <= 0)
                    dbManager.AddParameters(1, PARM_FAVORITE_LIST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FAVORITE_LIST_ID, FavoriteListId);
                if (IsActive > 1)
                {
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, null);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                }

                if (PageNumber <= 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, DBNull.Value);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage <= 0)
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, DBNull.Value);
                else
                    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, RowsPerPage);
                if (!string.IsNullOrEmpty(SearchData))
                {
                    dbManager.AddParameters(5, PARM_SEARCH_DATA, SearchData);
                }
                else
                {
                    dbManager.AddParameters(5, PARM_SEARCH_DATA, DBNull.Value);
                }
                dbManager.AddParameters(6, PARM_RECORD_COUNT, ds.FavoriteListICD.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSFavoriteList)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_ICD_SELECT, ds, ds.FavoriteListICD.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::LoadClinical_FavoriteListICD", PROC_FAVORITE_LIST_ICD_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFavoriteList loadClinical_FavoriteListCustomForm(long FavoriteListCustomFormId, long FavoriteListId, byte IsActive, Int32 PageNumber, Int32 RowsPerPage)
        {
            DSFavoriteList ds = new DSFavoriteList();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                //if (FavoriteListCustomFormId <= 0)
                //    dbManager.AddParameters(0, PARM_FAVORITE_LIST_CUSTOMFORM_ID, null);
                //else
                //    dbManager.AddParameters(0, PARM_FAVORITE_LIST_CUSTOMFORM_ID, FavoriteListCustomFormId);

                if (FavoriteListId <= 0)
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, FavoriteListId);
                //if (IsActive > 1)
                //{
                //    dbManager.AddParameters(2, PARM_IS_ACTIVE, null);
                //}
                //else
                //{
                //    dbManager.AddParameters(2, PARM_IS_ACTIVE, IsActive);
                //}

                //if (PageNumber <= 0)
                //    dbManager.AddParameters(3, PARM_PAGE_NUMBER, DBNull.Value);
                //else
                //    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);
                //if (RowsPerPage <= 0)
                //    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, DBNull.Value);
                //else
                //    dbManager.AddParameters(4, PARM_ROWS_PER_PAGE, RowsPerPage);
                //dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.FavoriteListICD.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSFavoriteList)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_CUSTOMFORM_SELECT, ds, ds.FavoriteListCustomForm.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::loadClinical_FavoriteListCustomForm", PROC_FAVORITE_LIST_CUSTOMFORM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        private void CreateParameters(IDBManager dbManager, DSFavoriteList ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(12);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, ds.FavoriteList.FavoriteListIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, ds.FavoriteList.FavoriteListIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_NAME, ds.FavoriteList.NameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_DESCRIPTION, ds.FavoriteList.DescriptionColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(3, PARM_ENTITY_ID, ds.FavoriteList.EntityIdColumn.ColumnName, DbType.Int64);
            if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                dbManager.AddParameters(3, PARM_ENTITY_ID, ds.FavoriteList.EntityIdColumn.ColumnName, DbType.Int64);

            else
                dbManager.AddParameters(3, PARM_ENTITY_ID, MDVSession.Current.EntityId);

            dbManager.AddParameters(4, PARM_LIST_TYPE, ds.FavoriteList.ListTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_IS_ACTIVE, ds.FavoriteList.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(6, PARM_CREATED_BY, ds.FavoriteList.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_CREATED_ON, ds.FavoriteList.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(8, PARM_MODIFIED_BY, ds.FavoriteList.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_MODIFY_ON, ds.FavoriteList.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(10, PARM_LAB_ID, ds.FavoriteList.LabIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(11, PARM_BODYPART_ID, ds.FavoriteList.BodyPartIdColumn.ColumnName, DbType.Int64);
        }


        private void CreateInsetUpdateParametersForICD(IDBManager dbManager, DSFavoriteList ds, Boolean IsInsert)
        {


            if (IsInsert == true)
            {
                dbManager.CreateInsertParameters(15);
                dbManager.AddInsertUpdateParameters(0, PARM_FAVORITE_LIST_ICD_ID, ds.FavoriteListICD.FavoriteListICDIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(15);
                dbManager.AddInsertUpdateParameters(0, PARM_FAVORITE_LIST_ICD_ID, ds.FavoriteListICD.FavoriteListICDIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(1, PARM_FAVORITELIST_ID, ds.FavoriteListICD.FavoriteListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_ICD9Code, ds.FavoriteListICD.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_ICD9CodeDescription, ds.FavoriteListICD.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_ACTIVE, ds.FavoriteListICD.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.FavoriteListICD.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.FavoriteListICD.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.FavoriteListICD.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFY_ON, ds.FavoriteListICD.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_ICD10Code, ds.FavoriteListICD.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_ICD10CodeDescription, ds.FavoriteListICD.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_SNOMEDID, ds.FavoriteListICD.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(12, PARM_SNOMEDDescription, ds.FavoriteListICD.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_LexiCode, ds.FavoriteListICD.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(14, PARM_LexiCodeDescription, ds.FavoriteListICD.LexiCodeDescriptionColumn.ColumnName, DbType.String);
        }

        private void CreateInsetUpdateParametersForCustomForms(IDBManager dbManager, DSFavoriteList ds, Boolean IsInsert)
        {


            if (IsInsert == true)
            {
                dbManager.CreateInsertParameters(6);
                dbManager.AddInsertUpdateParameters(0, PARM_FAVORITE_LIST_CUSTOMFORM_ID, ds.FavoriteListCustomForm.FavoriteListCustomFormIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.CreateUpdateParameters(6);
                dbManager.AddInsertUpdateParameters(0, PARM_FAVORITE_LIST_CUSTOMFORM_ID, ds.FavoriteListCustomForm.FavoriteListCustomFormIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddInsertUpdateParameters(1, PARM_FAVORITELIST_ID, ds.FavoriteListCustomForm.FavoriteListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CUSTOMFORM_ID, ds.FavoriteListCustomForm.CustomFormIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(3, PARM_CUSTOMFORM_NAME, ds.FavoriteListCustomForm.CustomFormNameColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_CREATED_ON, ds.FavoriteListCustomForm.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(5, PARM_MODIFY_ON, ds.FavoriteListCustomForm.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }

        private void CreateParametersForICD(IDBManager dbManager, DSFavoriteList ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(15);

            if (IsInsert == true)
            {
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_ICD_ID, ds.FavoriteListICD.FavoriteListICDIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
            else
            {
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_ICD_ID, ds.FavoriteListICD.FavoriteListICDIdColumn.ColumnName, DbType.Int64);
            }
            dbManager.AddParameters(1, PARM_FAVORITELIST_ID, ds.FavoriteListICD.FavoriteListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_ICD9Code, ds.FavoriteListICD.ICD9CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_ICD9CodeDescription, ds.FavoriteListICD.ICD9CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.FavoriteListICD.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.FavoriteListICD.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.FavoriteListICD.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.FavoriteListICD.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFY_ON, ds.FavoriteListICD.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_ICD10Code, ds.FavoriteListICD.ICD10CodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_ICD10CodeDescription, ds.FavoriteListICD.ICD10CodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_SNOMEDID, ds.FavoriteListICD.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_SNOMEDDescription, ds.FavoriteListICD.SNOMEDDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_LexiCode, ds.FavoriteListICD.LexiCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_LexiCodeDescription, ds.FavoriteListICD.LexiCodeDescriptionColumn.ColumnName, DbType.String);
        }
        /// <summary>
        /// Metod Name: CreateParametersForCPT
        /// Author Name: Ahmad Raza
        /// Created Date: 23-03-2016
        /// Description: This function is creating parameters for insert update Favorite List CPT
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="IsInsert"></param>
        private void CreateParametersForCPT(IDBManager dbManager, DSFavoriteList ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(14);
          
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_CPT_ID, ds.FavoriteListCPT.FavoriteListCPTIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_CPT_ID, ds.FavoriteListCPT.FavoriteListCPTIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_FAVORITELIST_ID, ds.FavoriteListCPT.FavoriteListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_CPTCode, ds.FavoriteListCPT.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CPTDescription, ds.FavoriteListCPT.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_IS_ACTIVE, ds.FavoriteListCPT.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(5, PARM_CREATED_BY, ds.FavoriteListCPT.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CREATED_ON, ds.FavoriteListCPT.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(7, PARM_MODIFIED_BY, ds.FavoriteListCPT.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_MODIFY_ON, ds.FavoriteListCPT.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_SNOMEDID, ds.FavoriteListCPT.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_SNOMEDDescription, ds.FavoriteListCPT.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_LAB_ID, ds.FavoriteListCPT.LabIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(12, PARM_MODIFIER, ds.FavoriteListCPT.ModifierColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_UNIT, ds.FavoriteListCPT.UnitColumn.ColumnName, DbType.String);
        }


        private void CreateParametersForCPTInsertAndUpdate(IDBManager dbManager, DSFavoriteList ds, bool IsInsert)
        {           
            if (IsInsert == true) {
                dbManager.CreateInsertParameters(14);
                dbManager.AddInsertUpdateParameters(0, PARM_FAVORITE_LIST_CPT_ID, ds.FavoriteListCPT.FavoriteListCPTIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            }
           
            else
            {
                dbManager.CreateUpdateParameters(14);
                dbManager.AddInsertUpdateParameters(0, PARM_FAVORITE_LIST_CPT_ID, ds.FavoriteListCPT.FavoriteListCPTIdColumn.ColumnName, DbType.Int64);
            }
            
            dbManager.AddInsertUpdateParameters(1, PARM_FAVORITELIST_ID, ds.FavoriteListCPT.FavoriteListIdColumn.ColumnName, DbType.Int64);
            dbManager.AddInsertUpdateParameters(2, PARM_CPTCode, ds.FavoriteListCPT.CPTCodeColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(3, PARM_CPTDescription, ds.FavoriteListCPT.CPTCodeDescriptionColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(4, PARM_IS_ACTIVE, ds.FavoriteListCPT.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddInsertUpdateParameters(5, PARM_CREATED_BY, ds.FavoriteListCPT.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(6, PARM_CREATED_ON, ds.FavoriteListCPT.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(7, PARM_MODIFIED_BY, ds.FavoriteListCPT.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(8, PARM_MODIFY_ON, ds.FavoriteListCPT.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddInsertUpdateParameters(9, PARM_SNOMEDID, ds.FavoriteListCPT.SNOMEDIDColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(10, PARM_SNOMEDDescription, ds.FavoriteListCPT.SNOMED_DESCRIPTIONColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(11, PARM_LAB_ID, ds.FavoriteListCPT.LabIdColumn.ColumnName, DbType.Int32);
            dbManager.AddInsertUpdateParameters(12, PARM_MODIFIER, ds.FavoriteListCPT.ModifierColumn.ColumnName, DbType.String);
            dbManager.AddInsertUpdateParameters(13, PARM_UNIT, ds.FavoriteListCPT.UnitColumn.ColumnName, DbType.String);
        }
        #region FAv Complaint lookUps

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 22/03/2016
        // Purpose : Look up for Favorite Complaint Name.

        public DSFavoriteListLookup GetFavComplaint(string ListType)
        {
            DSFavoriteListLookup ds = new DSFavoriteListLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_LIST_TYPE, ListType);
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(2, PARM_UserID, MDVSession.Current.AppUserId);
                ds = (DSFavoriteListLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_NAME_LOOKUP, ds, ds.FavoriteListName.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::GetFavComplaint", PROC_FAVORITE_LIST_NAME_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFavoriteListLookup GetFavListByProvider(string ListType, string ProviderId = null)
        {
            DSFavoriteListLookup ds = new DSFavoriteListLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_LIST_TYPE, ListType);
                dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(2, PARM_UserID, MDVSession.Current.AppUserId);
                if (!string.IsNullOrWhiteSpace(ProviderId))
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);
                else
                    dbManager.AddParameters(3, PARM_PROVIDER_ID, DBNull.Value);
                ds = (DSFavoriteListLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_NAME_LOOKUP, ds, ds.FavoriteListName.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::GetFavListByProvider", PROC_FAVORITE_LIST_NAME_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
        public DSFavoriteList insertClinical_FavoriteList(DSFavoriteList ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.FavoriteList.GetChanges();
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSFavoriteList)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_INSERT, ds, ds.FavoriteList.TableName);
                //ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FavoriteList.Rows[0][ds.FavoriteList.FavoriteListIdColumn].ToString(), null, ds.FavoriteList.Rows[0][ds.FavoriteList.FavoriteListIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::insertFavoriteList", PROC_FAVORITE_LIST_INSERT, ex);
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

        public DSFavoriteList updateClinical_FavoriteList(DSFavoriteList ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.FavoriteList.GetChanges();
                dbManager.Open();
                CreateParameters(dbManager, ds, false);
                ds = (DSFavoriteList)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_UPDATE, ds, ds.FavoriteList.TableName);
                //ds.AcceptChanges();
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FavoriteList.Rows[0][ds.FavoriteList.FavoriteListIdColumn].ToString(), null, ds.FavoriteList.Rows[0][ds.FavoriteList.FavoriteListIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::updateClinical_FavoriteList", PROC_FAVORITE_LIST_UPDATE, ex);
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
        /// Module Name: insertClinicalFavoriteHistoryList
        /// Author: Humaira Yousaf
        /// Created Date: 16-06-2016
        /// Description: Inserts Favorite History List
        /// </summary>
        /// <param name="ds" type="DSPhysicalExam">dataset contains data to insert/update</param>
        public DSFavoriteList insertClinicalFavoriteHistoryList(DSFavoriteList ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.FavoriteList.GetChanges();
                dbManager.BeginTransaction();

                CreateParameters(dbManager, ds, true);
                ds = (DSFavoriteList)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_INSERT, ds, ds.FavoriteList.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FavoriteList.Rows[0][ds.FavoriteList.FavoriteListIdColumn].ToString(), null, ds.FavoriteList.Rows[0][ds.FavoriteList.FavoriteListIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALFavoriteList::insertClinicalFavoriteHistoryList", PROC_FAVORITE_LIST_INSERT, ex);
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
        /// Module Name: updateClinicalFavoriteHistoryList
        /// Author: Humaira Yousaf
        /// Created Date: 16-06-2016
        /// Description: Updates Favorite History List
        /// </summary>
        /// <param name="ds" type="DSPhysicalExam">dataset contains data to insert/update</param>
        public DSFavoriteList updateClinicalFavoriteHistoryList(DSFavoriteList ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.FavoriteList.GetChanges();
                dbManager.BeginTransaction();

                CreateParameters(dbManager, ds, false);
                ds = (DSFavoriteList)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_UPDATE, ds, ds.FavoriteList.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FavoriteList.Rows[0][ds.FavoriteList.FavoriteListIdColumn].ToString(), null, ds.FavoriteList.Rows[0][ds.FavoriteList.FavoriteListIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALFavoriteList::updateClinicalFavoriteHistoryList", PROC_FAVORITE_LIST_UPDATE, ex);
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
        public DSFavoriteList insertClinical_FavoriteListICD(DSFavoriteList ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateInsetUpdateParametersForICD(dbManager, ds, true);
                CreateInsetUpdateParametersForICD(dbManager, ds, false);
                ds = (DSFavoriteList)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_INSERT_ICD, PROC_FAVORITE_LIST_ICD_UPDATE, ds, ds.FavoriteListICD.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::insertFavoriteListICD", PROC_FAVORITE_LIST_INSERT_ICD + " " + PROC_FAVORITE_LIST_ICD_UPDATE, ex);
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

        public string UpdateClinical_FavoriteListProviders(string ProvidersID, string FavListId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_FAV_LIST_ID, Convert.ToInt64(FavListId));
                dbManager.AddParameters(1, PARM_PROVIDER_ID, ProvidersID);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 1000);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_FAVORITE_LIST_UPDATE_PROVIDERS);

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::UpdateClinical_FavoriteListProviders", PROC_FAVORITE_LIST_UPDATE_PROVIDERS, ex);
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

        public string UpdateClinical_FavoriteListCustomForms(string CustomFormsIds, string FavListId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, Convert.ToInt64(FavListId));
                dbManager.AddParameters(1, PARM_CUSTOMFORM_IDS, CustomFormsIds);
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 1000);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_FAVORITE_LIST_CUSTOMFORMS_UPDATE);

                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::UpdateClinical_FavoriteListCustomForms", PROC_FAVORITE_LIST_CUSTOMFORMS_UPDATE, ex);
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
        public DSFavoriteList insertClinical_FavoriteListProviders(DSFavoriteList ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_FAV_LIST_ID, ds.FavoriteListProvider.FavListIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.FavoriteListProvider.ProviderIdColumn.ColumnName, DbType.Int64);
                dbManager.Open();
                ds = (DSFavoriteList)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_INSERT_PROVIDERS, ds, ds.FavoriteListProvider.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::insertClinical_FavoriteListProviders", PROC_FAVORITE_LIST_INSERT_PROVIDERS, ex);
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

        public DSFavoriteList insertClinical_FavoriteListCustomForms(DSFavoriteList ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, ds.FavoriteListCustomForm.FavoriteListIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, PARM_CUSTOMFORM_ID, ds.FavoriteListCustomForm.CustomFormIdColumn.ColumnName, DbType.Int64);
                dbManager.Open();
                ds = (DSFavoriteList)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_INSERT_CUSTOMFORMS, ds, ds.FavoriteListCustomForm.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::insertClinical_FavoriteListProviders", PROC_FAVORITE_LIST_INSERT_CUSTOMFORMS, ex);
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
        /// Module Name: insertClinicalFavoriteHistoryListICD
        /// Author: Humaira Yousaf
        /// Created Date: 16-06-2016
        /// Description: Inserts Favorite History List ICD
        /// </summary>
        /// <param name="ds" type="DSPhysicalExam">dataset contains data to insert/update</param>
        public DSFavoriteList insertClinicalFavoriteHistoryListICD(DSFavoriteList ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.FavoriteListICD.GetChanges();
                dbManager.BeginTransaction();
                CreateInsetUpdateParametersForICD(dbManager, ds, true);
                CreateInsetUpdateParametersForICD(dbManager, ds, false);

                ds = (DSFavoriteList)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_INSERT_ICD, PROC_FAVORITE_LIST_ICD_UPDATE, ds, ds.FavoriteListICD.TableName);

                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.FavoriteListICD.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.FavoriteListICD.Rows[i][ds.FavoriteListICD.FavoriteListICDIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FavoriteListICD.Rows[0][ds.FavoriteListICD.FavoriteListICDIdColumn].ToString(), null, ds.FavoriteListICD.Rows[0][ds.FavoriteListICD.FavoriteListIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALFavoriteList::insertFavoriteListICD", PROC_FAVORITE_LIST_INSERT_ICD + " " + PROC_FAVORITE_LIST_ICD_UPDATE, ex);
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

        public DSFavoriteList insertClinicalFavoriteListCustomForms(DSFavoriteList ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                DataTable dtTemp = ds.FavoriteListCustomForm.GetChanges();
                dbManager.BeginTransaction();
                CreateInsetUpdateParametersForCustomForms(dbManager, ds, true);
                CreateInsetUpdateParametersForCustomForms(dbManager, ds, false);

                ds = (DSFavoriteList)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_INSERT_CUSTOMFORMS, PROC_FAVORITE_LIST_CUSTOMFORMS_UPDATE, ds, ds.FavoriteListCustomForm.TableName);

                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.FavoriteListCustomForm.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.FavoriteListCustomForm.Rows[i][ds.FavoriteListCustomForm.FavoriteListCustomFormIdColumn];
                    }

                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FavoriteListCustomForm.Rows[0][ds.FavoriteListCustomForm.FavoriteListCustomFormIdColumn].ToString(), null, ds.FavoriteListCustomForm.Rows[0][ds.FavoriteListCustomForm.FavoriteListIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALFavoriteList::insertFavoriteListCustomForm", PROC_FAVORITE_LIST_INSERT_CUSTOMFORMS + " " + PROC_FAVORITE_LIST_CUSTOMFORMS_UPDATE, ex);
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
        /// Metod Name: insertClinicalFavoriteListCPT
        /// Author Name: Ahmad Raza
        /// Created Date: 23-03-2016
        /// Description: This function will insert Favorite List CPT
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSFavoriteList insertClinicalFavoriteListCPT(DSFavoriteList ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            
            try
            {
                dbManager.Open();
                DSDBAudit dsDBAudit = new DSDBAudit();
                DataTable dtTemp = ds.FavoriteListCPT.GetChanges();

                CreateParametersForCPTInsertAndUpdate(dbManager, ds, true);
                CreateParametersForCPTInsertAndUpdate(dbManager, ds, false);

                //CreateParametersForCPT(dbManager, ds, false);

                //ds = (DSProcedureOrder)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PROCEDURE_ORDER_PROBLEM_INSERT, PROC_PROCEDURE_ORDER_PROBLEM_INSERT, ds, ds.ProcedureOrderProblem.TableName);
                ds = (DSFavoriteList)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_CPT_INSERT, PROC_FAVORITE_LIST_CPT_UPDATE,ds, ds.FavoriteListCPT.TableName);

                if (dtTemp != null)
                {
                    dtTemp.Columns.Add("PrimaryKey");
                    for (int i = 0; i < ds.FavoriteListCPT.Rows.Count; i++)
                    {
                        dtTemp.Rows[i]["PrimaryKey"] = ds.FavoriteListCPT.Rows[i][ds.FavoriteListCPT.FavoriteListCPTIdColumn];
                    }
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.FavoriteListCPT.Rows[0][ds.FavoriteListCPT.FavoriteListCPTIdColumn].ToString(), null, ds.FavoriteListCPT.Rows[0][ds.FavoriteListCPT.FavoriteListIdColumn].ToString());
                    dsDBAudit.AcceptChanges();
                }
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::insertClinicalFavoriteListCPT", PROC_FAVORITE_LIST_CPT_INSERT, ex);
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
        public string SaveFavMedicationDetail(FavoriteListMedicationModel model) {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createMedicationParameters(dbManager, model, true);
                object MedicationId = dbManager.ExecuteScalar(PROC_FAV_MEDICATION_DETAIL_INSERT);
                if (MDVUtility.ToStr(MedicationId) == "")
                    throw new Exception(MDVUtility.ToStr(MedicationId));
                return MDVUtility.ToStr(MedicationId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::SaveFavMedicationDetail", PROC_FAV_MEDICATION_DETAIL_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string UpdateFavMedicationDetail(FavoriteListMedicationModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createMedicationParameters(dbManager, model, false);
                object MedicationId = dbManager.ExecuteScalar(PROC_FAV_MEDICATION_DETAIL_UPDATE);
                if (MDVUtility.ToStr(MedicationId) == "")
                    throw new Exception(MDVUtility.ToStr(MedicationId));
                return MDVUtility.ToStr(MedicationId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::UpdateFavMedicationDetail", PROC_FAV_MEDICATION_DETAIL_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public FavoriteListMedicationModel LoadMedicationDetail(long FavoriteListId, long FavMedicationId)
        {
            FavoriteListMedicationModel MedicationObj = new FavoriteListMedicationModel();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (FavoriteListId > 0)
                    dbManager.AddParameters(PARM_FAVORITELIST_ID, FavoriteListId);
                else
                    dbManager.AddParameters(PARM_FAVORITELIST_ID, null);
                if (FavMedicationId > 0)
                    dbManager.AddParameters(PARM_FAVORITE_LIST_MEDICATION_ID, FavMedicationId);
                else
                    dbManager.AddParameters(PARM_FAVORITE_LIST_MEDICATION_ID, null);
                dbManager.AddParameters(PARM_RECORD_COUNT, null, DbType.String, ParamDirection.Output, 500);
                MedicationObj = dbManager.ExecuteReader<FavoriteListMedicationModel>(PROC_FAV_MEDICATION_SELECT);
                return MedicationObj;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::LoadMedication", PROC_FAV_MEDICATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteMedicationDetail(long FavMedicationId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_ID, FavMedicationId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FAV_MEDICATION_DETAIL_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);
                dbManager.CommitTransaction();
                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALFavoriteList::DeleteMedicationDetail", PROC_FAV_MEDICATION_DETAIL_DELETE, ex);
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
        private void createMedicationParameters(IDBManager dbManager, FavoriteListMedicationModel model, Boolean IsInsert)
        {
            if (IsInsert == true)
                dbManager.AddParameters(PARM_ID, model.Id, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(PARM_ID, model.Id, DbType.Int64);
            if (!string.IsNullOrEmpty(model.Action))
                dbManager.AddParameters(PARM_ACTION, model.Action);
            else
                dbManager.AddParameters(PARM_ACTION, null);
            if (!string.IsNullOrEmpty(model.Dose))
                dbManager.AddParameters(PARM_DOSE, model.Dose);
            else
                dbManager.AddParameters(PARM_DOSE, null);
            if (!string.IsNullOrEmpty(model.DoseUnit))
                dbManager.AddParameters(PARM_DOSE_UNIT, model.DoseUnit);
            else
                dbManager.AddParameters(PARM_DOSE_UNIT, null);
            if (!string.IsNullOrEmpty(model.Route))
                dbManager.AddParameters(PARM_ROUTE, model.Route);
            else
                dbManager.AddParameters(PARM_ROUTE, null);
            if (!string.IsNullOrEmpty(model.DoseTiming))
                dbManager.AddParameters(PARM_DOSE_TIMING, model.DoseTiming);
            else
                dbManager.AddParameters(PARM_DOSE_TIMING, null);
            if (!string.IsNullOrEmpty(model.DoseOther))
                dbManager.AddParameters(PARM_DOSE_OTHER, model.DoseOther);
            else
                dbManager.AddParameters(PARM_DOSE_OTHER, null);
            if (!string.IsNullOrEmpty(model.Duration))
                dbManager.AddParameters(PARM_DURATION, model.Duration);
            else
                dbManager.AddParameters(PARM_DURATION, null);
            if (!string.IsNullOrEmpty(model.Quantity))
                dbManager.AddParameters(PARM_QUANTITY, model.Quantity);
            else
                dbManager.AddParameters(PARM_QUANTITY, null);
            if (!string.IsNullOrEmpty(model.QuantityUnit))
                dbManager.AddParameters(PARM_QUANTITY_UNIT, model.QuantityUnit);
            else
                dbManager.AddParameters(PARM_QUANTITY_UNIT, null);
            if (!string.IsNullOrEmpty(model.Refill))
                dbManager.AddParameters(PARM_REFILL, model.Refill);
            else
                dbManager.AddParameters(PARM_REFILL, null);
            if (!string.IsNullOrEmpty(model.DirectionsToPharmacist))
                dbManager.AddParameters(PARM_DIRECTIONS_TO_PHARMACIST, model.DirectionsToPharmacist);
            else
                dbManager.AddParameters(PARM_DIRECTIONS_TO_PHARMACIST, null);
            if (!string.IsNullOrEmpty(model.AddDirectionToPatient))
                dbManager.AddParameters(PARM_ADD_DIRECTION_TO_PATIENT, model.AddDirectionToPatient);
            else
                dbManager.AddParameters(PARM_ADD_DIRECTION_TO_PATIENT, null);
            if (!string.IsNullOrEmpty(model.PrescriptionsOtherNotes))
                dbManager.AddParameters(PARM_PRESCRIPTIONS_OTHER_NOTES, model.PrescriptionsOtherNotes);
            else
                dbManager.AddParameters(PARM_PRESCRIPTIONS_OTHER_NOTES, null);
            if (!string.IsNullOrEmpty(model.Comments))
                dbManager.AddParameters(PARM_COMMENTS, model.Comments);
            else
                dbManager.AddParameters(PARM_COMMENTS, null);
            if (!string.IsNullOrEmpty(model.NDCID))
                dbManager.AddParameters(PARM_NDCID, model.NDCID);
            else
                dbManager.AddParameters(PARM_NDCID, null);
            if (!string.IsNullOrEmpty(model.BrandName))
                dbManager.AddParameters(PARM_BRAND_NAME, model.BrandName);
            else
                dbManager.AddParameters(PARM_BRAND_NAME, null);
            if (!string.IsNullOrEmpty(model.GenericName))
                dbManager.AddParameters(PARM_GENERIC_NAME, model.GenericName);
            else
                dbManager.AddParameters(PARM_GENERIC_NAME, null);
            if (!string.IsNullOrEmpty(model.Form))
                dbManager.AddParameters(PARM_FORM, model.Form);
            else
                dbManager.AddParameters(PARM_FORM, null);
            if (!string.IsNullOrEmpty(model.Strength))
                dbManager.AddParameters(PARM_STRENGTH, model.Strength);
            else
                dbManager.AddParameters(PARM_STRENGTH, null);
            if (!string.IsNullOrEmpty(model.FavoriteListId))
                dbManager.AddParameters(PARM_FAVORITELIST_ID, model.FavoriteListId);
            else
                dbManager.AddParameters(PARM_FAVORITELIST_ID, null);
        }
        public DSFavoriteList loadClinicalFavoriteListMedication(long FavoriteListMedId, long FavoriteListId, Int32 PageNumber, Int32 RowsPerPage)
        {
            DSFavoriteList ds = new DSFavoriteList();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                if (FavoriteListMedId <= 0)
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_MEDICATION_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_MEDICATION_ID, FavoriteListMedId);
                if (FavoriteListId <= 0)
                    dbManager.AddParameters(1, PARM_FAVORITE_LIST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FAVORITE_LIST_ID, FavoriteListId);
                if (PageNumber <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, 2000);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowsPerPage);
                dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.FavoriteListMedication.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSFavoriteList)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAV_MEDICATION_SELECT, ds, ds.FavoriteListMedication.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::loadClinicalFavoriteListMedication", PROC_FAV_MEDICATION_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Metod Name: update ClinicalFavoriteListCPT
        /// Author Name: Abid Ali
        /// Description: This function will update Favorite List CPT
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSFavoriteList updateClinicalFavoriteListCPT(DSFavoriteList ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForCPT(dbManager, ds, false);
                ds = (DSFavoriteList)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_CPT_UPDATE, ds, ds.FavoriteListCPT.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::updateClinicalFavoriteListCPT", PROC_FAVORITE_LIST_CPT_UPDATE, ex);
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

        public DSFavoriteList updateClinical_FavoriteListICD(DSFavoriteList ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParametersForICD(dbManager, ds, false);
                ds = (DSFavoriteList)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_ICD_UPDATE, ds, ds.FavoriteListICD.TableName);
                //ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::updateClinical_FavoriteListICD", PROC_FAVORITE_LIST_ICD_UPDATE, ex);
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
        public string deleteClinical_FavoriteListICD(string FavoriteListICDId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                DSFavoriteList dsCurrentOrder = loadClinical_FavoriteListICD(Convert.ToInt64(FavoriteListICDId), 0, 0, 1, 2000);

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_ICD_ID, FavoriteListICDId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FAVORITE_LIST_ICD_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrder.FavoriteListICD;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, FavoriteListICDId, null, dsCurrentOrder.FavoriteListICD.Rows[0][dsCurrentOrder.FavoriteListICD.FavoriteListIdColumn].ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALFavoriteList::deleteClinical_FavoriteListICD", PROC_FAVORITE_LIST_ICD_DELETE, ex);
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

        public string GetFavListValue(string FavoriteListName)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                dbManager.CreateParameters(3);
                dbManager.AddParameters(0, PARM_FAVORITELIST_NAME, FavoriteListName);
                dbManager.AddParameters(1, PARM_UserID, MDVSession.Current.AppUserId);

                dbManager.AddParameters(2, PARM_FAVORITELIST_VALUE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_GET_FAVORITE_LIST_VAL).ToString();
                if (returnVal.IndexOf("FavLstValue-") > -1)
                {
                    dbManager.CommitTransaction();
                    return returnVal.Replace("FavLstValue-", "");
                }
                else
                {
                    throw new Exception(returnVal);
                }
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALFavoriteList::GetFavListValue", PROC_GET_FAVORITE_LIST_VAL, ex);
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
        /// Metod Name: loadClinicalFavoriteListCPT
        /// Author Name: Ahmad Raza
        /// Created Date: 23-03-2016
        /// Description: This function will load Favorite List CPT
        /// </summary>
        /// <param name="FavoriteListCPTId"></param>
        /// <param name="FavoriteListId"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        /// 
        public DSFavoriteList loadClinicalFavoriteListCPT(long FavoriteListCPTId, long FavoriteListId, Int32 PageNumber, Int32 RowsPerPage, string SearchData = "")
        {
            DSFavoriteList ds = new DSFavoriteList();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);

                if (FavoriteListCPTId <= 0)
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_CPT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_CPT_ID, FavoriteListCPTId);

                if (FavoriteListId == 0)
                    dbManager.AddParameters(1, PARM_FAVORITE_LIST_ID, null);
                else
                    dbManager.AddParameters(1, PARM_FAVORITE_LIST_ID, FavoriteListId);



                if (PageNumber <= 0)
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, 1);
                else
                    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage <= 0)
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, 2000);
                else
                    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowsPerPage);
                if (string.IsNullOrEmpty(SearchData))
                {
                    dbManager.AddParameters(4, PARM_SEARCH_DATA, null);
                }
                else
                {
                    dbManager.AddParameters(4, PARM_SEARCH_DATA, SearchData);
                }
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.FavoriteListCPT.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSFavoriteList)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_CPT_SELECT, ds, ds.FavoriteListCPT.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::loadClinicalFavoriteListCPT", PROC_FAVORITE_LIST_CPT_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSFavoriteList loadClinicalFavoriteListCustomForm(long FavoriteListCustomFormId, long FavoriteListId, Int32 PageNumber, Int32 RowsPerPage)
        {
            DSFavoriteList ds = new DSFavoriteList();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                //if (FavoriteListCustomFormId <= 0)
                //    dbManager.AddParameters(0, PARM_FAVORITE_LIST_CUSTOMFORM_ID, null);
                //else
                //    dbManager.AddParameters(0, PARM_FAVORITE_LIST_CUSTOMFORM_ID, FavoriteListCustomFormId);

                if (FavoriteListId == 0)
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, FavoriteListId);



                //if (PageNumber <= 0)
                //    dbManager.AddParameters(2, PARM_PAGE_NUMBER, 1);
                //else
                //    dbManager.AddParameters(2, PARM_PAGE_NUMBER, PageNumber);
                //if (RowsPerPage <= 0)
                //    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, 2000);
                //else
                //    dbManager.AddParameters(3, PARM_ROWS_PER_PAGE, RowsPerPage);
                //dbManager.AddParameters(4, PARM_RECORD_COUNT, ds.FavoriteListCustomForm.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSFavoriteList)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FAVORITE_LIST_CUSTOMFORM_SELECT, ds, ds.FavoriteListCustomForm.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::loadClinicalFavoriteListCustomForm", PROC_FAVORITE_LIST_CUSTOMFORM_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Metod Name: deleteClinicalFavoriteListCPT
        /// Author Name: Ahmad Raza
        /// Created Date: 23-03-2016
        /// Description: This function will delete Favorite List
        /// </summary>
        /// <param name="FavoriteListCPTId"></param>
        /// <returns></returns>
        public string deleteClinicalFavoriteListCPT(long FavoriteListCPTId, long FavouriteListId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSDBAudit dsDBAudit = new DSDBAudit();
                DSFavoriteList dsCurrentOrder = loadClinicalFavoriteListCPT(Convert.ToInt64(FavoriteListCPTId), 0, 0, 1);
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (FavoriteListCPTId == -1)
                {
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_CPT_ID, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_CPT_ID, FavoriteListCPTId);
                }
                if (FavouriteListId == -1)
                {
                    dbManager.AddParameters(1, PARM_FAVORITE_LIST_ID, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_FAVORITE_LIST_ID, FavouriteListId);
                }
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_FAVORITE_LIST_CPT_DELETE);

                if (returnVal != "")
                {
                    throw new Exception(returnVal);
                }
                else
                {
                    DataTable dtTemp = dsCurrentOrder.FavoriteListCPT;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, FavoriteListCPTId.ToString(), null, dsCurrentOrder.FavoriteListCPT.Rows[0][dsCurrentOrder.FavoriteListCPT.FavoriteListIdColumn].ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                }
                return "";

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCDS::deleteClinicalFavoriteListCPT", PROC_FAVORITE_LIST_CPT_DELETE, ex);
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

        public string deleteClinicalFavoriteListICD(long FavoriteListICDId, long FavouriteListId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                DSFavoriteList dsCurrentOrder = loadClinical_FavoriteListICD(FavoriteListICDId, FavouriteListId, 0, 1, 15);

                dbManager.CreateParameters(3);
                if (FavoriteListICDId == -1)
                {
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_ICD_ID, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_FAVORITE_LIST_ICD_ID, FavoriteListICDId);
                }
                if (FavouriteListId == -1)
                {
                    dbManager.AddParameters(1, PARM_FAVORITE_LIST_ID, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_FAVORITE_LIST_ID, FavouriteListId);
                }
                dbManager.AddParameters(2, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_FAVORITE_LIST_ICD_DELETE);

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrder.FavoriteListICD;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, FavoriteListICDId.ToString(), null, FavouriteListId.ToString(), false, false, true);
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
                MDVLogger.DALErrorLog("DALCDS::CPTdeleteClinicalFavoriteListICD", PROC_FAVORITE_LIST_ICD_DELETE, ex);
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



        //Start//03/23/2016//Babur Rizwan//Delete Favorite Complaint
        public string deleteFavoriteList(string favoriteListId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                DSFavoriteList dsCurrentOrder = loadClinical_FavoriteList(Convert.ToInt64(favoriteListId), "", 0, 0, 1, 15, "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, favoriteListId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FAVORITE_LIST_DELETE).ToString();

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrder.FavoriteList;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, favoriteListId, null, favoriteListId, false, false, true);
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
                MDVLogger.DALErrorLog("DALFavoriteList::deleteFavoriteList", PROC_FAVORITE_LIST_DELETE, ex);
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

        public string deleteFavoriteListCustomForms(string favoriteListId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSDBAudit dsDBAudit = new DSDBAudit();

            try
            {
                dbManager.BeginTransaction();
                DSFavoriteList dsCurrentOrder = loadClinical_FavoriteList(Convert.ToInt64(favoriteListId), "", 0, 0, 1, 15, "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_FAVORITE_LIST_ID, favoriteListId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);
                returnVal = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FAVORITE_LIST_CUSTOMFORMS_DELETE));

                if (returnVal != "")
                    throw new Exception(returnVal);
                else
                {
                    DataTable dtTemp = dsCurrentOrder.FavoriteList;
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, favoriteListId, null, favoriteListId, false, false, true);
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
                MDVLogger.DALErrorLog("DALFavoriteList::deleteFavoriteList", PROC_FAVORITE_LIST_CUSTOMFORMS_DELETE, ex);
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
        //End//03/23/2016//Babur Rizwan//Delete Favorite Complaint

        #endregion

        #region immunization
        public DSFavoriteList GetCVXAndAdministeredCode(long VaccineId, long TherapueticId, string Type)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSFavoriteList ds = new DSFavoriteList();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(3);
                if (VaccineId == 0)
                {
                    dbManager.AddParameters(0, PARM_VACCINE_ID, null);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_VACCINE_ID, VaccineId);
                }
                if (TherapueticId == 0)
                {
                    dbManager.AddParameters(1, PARM_THERAPUETIC_ID, null);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_THERAPUETIC_ID, TherapueticId);
                }

                dbManager.AddParameters(2, PARM_TYPE, Type);
                ds = (DSFavoriteList)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_GET_VACCINE_CVX_ADM_CODE, ds, ds.VaccineData.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::GetCVXAndAdministeredCode", PROC_GET_VACCINE_CVX_ADM_CODE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string SaveFavVaccine(FavoriteListImmunizationModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createFavVaccineParameters(dbManager, model, true);
                var FavVaccineId = dbManager.ExecuteScalar(PROC_FAV_VACCINE_INSERT);
                return MDVUtility.ToStr(FavVaccineId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("SaveFavVaccine::SaveFavVaccine", PROC_FAV_VACCINE_INSERT, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }




        public List<FavoriteListImmunizationModel> LoadFavImmunization(string Type, bool IsActive, long ProviderId)
        {
            List<FavoriteListImmunizationModel> FavoriteListImmunizationModelList = new List<FavoriteListImmunizationModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.AddParameters(PARM_TYPE, Type);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                {
                    dbManager.AddParameters(PARM_ENTITY_ID, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                if (ProviderId == 0)
                {
                    dbManager.AddParameters(PARM_PROVIDER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_PROVIDER_ID, ProviderId);
                }
                dbManager.AddParameters(PARM_IS_ACTIVE, IsActive);
                FavoriteListImmunizationModelList = dbManager.ExecuteReaders<FavoriteListImmunizationModel>(PROC_FAV_IMMUNIZATION_LOOKUP);
                return FavoriteListImmunizationModelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::LoadFavImmunization", PROC_FAV_IMMUNIZATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public List<FavoriteListImmunizationModel> LoadFavImmunizationLookUp(Int64 ProviderId, string Tab, string Type, Int64 Category)
        {
            List<FavoriteListImmunizationModel> FavoriteListImmunizationModelList = new List<FavoriteListImmunizationModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {


                if (ProviderId == 0)
                {
                    dbManager.AddParameters(PARM_PROVIDER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_PROVIDER_ID, ProviderId);
                }
                if (Tab == "")
                {
                    dbManager.AddParameters(PARM_TAB, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_TAB, Tab);
                }

                if (Type == "")
                {
                    dbManager.AddParameters(PARM_TYPE, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_TYPE, Type);
                }
                if (Category == 0)
                {
                    dbManager.AddParameters(PARM_CATEGORY_ID, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_CATEGORY_ID, Category);
                }
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                {
                    dbManager.AddParameters(PARM_ENTITY_ID, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }

                dbManager.AddParameters(PARM_IS_ACTIVE, true);
                FavoriteListImmunizationModelList = dbManager.ExecuteReaders<FavoriteListImmunizationModel>(PROC_FAV_IMMUNIZATION_CLINICAL_LOOKUP);
                return FavoriteListImmunizationModelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::LoadFavImmunizationLookUp", PROC_FAV_IMMUNIZATION_CLINICAL_LOOKUP, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public List<FavoriteListImmunizationModel> LoadFavImmunizationDetail(long FavoritiesListId)
        {
            List<FavoriteListImmunizationModel> FavoriteListImmunizationModelList = new List<FavoriteListImmunizationModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_FAVORITIESLIST_ID, FavoritiesListId);
                FavoriteListImmunizationModelList = dbManager.ExecuteReaders<FavoriteListImmunizationModel>(PROC_FAV_IMMUNIZATION_SELECT);
                return FavoriteListImmunizationModelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::LoadFavImmunizationDetail", PROC_FAV_IMMUNIZATION_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }






        public List<FavoriteListImmunizationModel> LoadFavImmunizationVaccineDetail(long FavoritiesListId, string Tab, string SearchData = "")
        {
            List<FavoriteListImmunizationModel> FavoriteListImmunizationModelList = new List<FavoriteListImmunizationModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.AddParameters(PARM_FAVORITIESLIST_ID, FavoritiesListId);
                dbManager.AddParameters(PARM_TAB, Tab);
                if (string.IsNullOrEmpty(SearchData))
                {
                    dbManager.AddParameters(PARM_SEARCH_DATA, null);
                }
                else
                {
                    dbManager.AddParameters(PARM_SEARCH_DATA, SearchData);
                }
                FavoriteListImmunizationModelList = dbManager.ExecuteReaders<FavoriteListImmunizationModel>(PROC_FAV_IMMUNIZATION_VACCINE_SELECT);
                return FavoriteListImmunizationModelList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::LoadFavImmunizationVaccineDetail", PROC_FAV_IMMUNIZATION_VACCINE_SELECT, ex);
                throw ex;
            }
            finally
            {
            }
        }

        public string DeleteFavoriteImmunization(long FavoritiesListId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_FAVORITIESLIST_ID, FavoritiesListId);
                dbManager.AddParameters(PARM_ERROR_MESSAGE, null, DbType.String, ParamDirection.Output, 500);
                var dbCallId = dbManager.ExecuteScalar(PROC_FAV_IMMUNIZATION_DELETE);
                return MDVUtility.ToStr(dbCallId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::DeleteFavoriteImmunization", PROC_FAV_IMMUNIZATION_DELETE, ex);

                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public string UpdateFavVaccine(FavoriteListImmunizationModel model)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                createFavVaccineParameters(dbManager, model, false);
                var FavVaccineId = dbManager.ExecuteScalar(PROC_FAV_VACCINE_UPDATE);
                return MDVUtility.ToStr(FavVaccineId);
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFavoriteList::UpdateFavVaccine", PROC_FAV_VACCINE_UPDATE, ex);

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
