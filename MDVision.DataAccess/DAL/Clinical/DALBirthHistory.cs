/* Author:  KR
 * Created Date: 05/01/2016
 * OverView: Created for Birth History in Clinical Module
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
using System.Data.SqlClient;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Clinical
{
    public class DALBirthHistory
    {
        #region Variable

        #endregion

        #region "Stored Procedure Names"
        // SP names on 05/01/2016 By KR for BirthHx in Clinical

        private const string PROC_BIRTHHX_DELIVERYMETHODS_LOOKUP = "Clinical.sp_BirthHx_MaternalDelivery_DeliveryMethodLookup";
        private const string PROC_BIRTHHX_DELIVERYPRESENTATION_LOOKUP = "Clinical.sp_BirthHx_MaternalDelivery_DeliveryPresentationLookup";
        private const string PROC_BIRTHHX_MATERNALHISTORY_LOOKUP = "Clinical.sp_BirthHx_MaternalDelivery_MaternalHistoryLookup";

        private const string PROC_BIRTHHX_NEWBORN_PATIENT_BLOODTYPE_LOOKUP = "Clinical.sp_BirthHx_Newborn_PatientBloodTypeLookup";
        private const string PROC_BIRTHHX_NEWBORN_PROBLEMSATBIRTHLOOKUP = "Clinical.sp_BirthHx_Newborn_ProblemsAtBirthLookup";

        private const string PROC_BIRTHHX_INSERT = "Clinical.sp_BirthHxInsert";
        private const string PROC_BIRTHHX_UPDATE = "Clinical.sp_BirthHxUpdate";
        private const string PROC_BIRTHHX_DELETE = "Clinical.sp_BirthHxDelete";
        private const string PROC_BIRTHHX_SEELCT = "Clinical.sp_BirthHxSelect";
        private const string PROC_BIRTHHX_SEELCTForSoapText = "Clinical.sp_BirthHxSelectForSoapText";

        private const string PROC_BIRTHHX_GENERAL_INSERT = "Clinical.sp_BirthHx_GeneralInsert";
        private const string PROC_BIRTHHX_GENERAL_UPDATE = "Clinical.sp_BirthHx_GeneralUpdate";
        private const string PROC_BIRTHHX_GENERAL_DELETE = "Clinical.sp_BirthHx_GeneralDelete";
        private const string PROC_BIRTHHX_GENERAL_SEELCT = "Clinical.sp_BirthHx_GeneralSelect";

        private const string PROC_BIRTHHX_MATERNALDELIVERY_INSERT = "Clinical.sp_BirthHx_MaternalDeliveryInsert";
        private const string PROC_BIRTHHX_MATERNALDELIVERY_UPDATE = "Clinical.sp_BirthHx_MaternalDeliveryUpdate";
        private const string PROC_BIRTHHX_MATERNALDELIVERY_DELETE = "Clinical.sp_BirthHx_MaternalDeliveryDelete";
        private const string PROC_BIRTHHX_MATERNALDELIVERY_SEELCT = "Clinical.sp_BirthHx_MaternalDeliverySelect";

        private const string PROC_BIRTHHX_NEWBORN_INSERT = "Clinical.sp_BirthHx_NewbornInsert";
        private const string PROC_BIRTHHX_NEWBORN_UPDATE = "Clinical.sp_BirthHx_NewbornUpdate";
        private const string PROC_BIRTHHX_NEWBORN_DELETE = "Clinical.sp_BirthHx_NewbornDelete";
        private const string PROC_BIRTHHX_NEWBORN_SEELCT = "Clinical.sp_BirthHx_NewbornSelect";

        private const string PROC_ATTACH_BIRTHHX_FROM_NOTES = "Clinical.sp_AttachBirthHxWithNotes";
        private const string PROC_DETACH_BIRTHHX_FROM_NOTES = "Clinical.sp_DetachBirthHxFromNotes";

        private const string PROC_UPDATE_SOAPTEXT_FOR_BIRTHHX = "Clinical.sp_UpdateSoapTextForBirthHX";
        private const string PROC_NOTES_BIRTHH_SELECT = "[Clinical].[sp_NotesBirthHxSelect]";

        #endregion

        #region Constructors
        /// <summary>
        /// A Constructor of the Class
        /// </summary>
        /// <param name="Obj"></param>
        public DALBirthHistory()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }

        public DALBirthHistory(SharedVariable SharedVariable)
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

        #region "Parameters"
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : parameters for BirthHx.
        /// Date : 6 january 2016
        /// </summary>
        private const string PARM_BIRTHHX_ID = "@BirthHxId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_BIRTHHX_GENERAL_ID = "@GeneralId";
        private const string PARM_BIRTHHX_MATERNALDELIVERY_ID = "@MaternalDeliveryId";
        private const string PARM_BIRTHHX_NEWBORN_ID = "@NewbornId";
        private const string PARM_BIRTHHXDATE = "@BirthHxDate";
        private const string PARM_BUNREMARKABLE = "@bUnremarkable";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_SOAPTEXT = "@SoapText";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";

        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        //General
        private const string PARM_HOSPITAL_NAME = "@HospitalName";
        private const string PARM_PATIENTDOB = "@PatientDOB";
        private const string PARM_LENGTH_OF_STAY_AT_HOSPITAL = "@LengthStayatHospital";
        private const string PARM_DATE_ADMITTED = "@DateAdmitted";
        private const string PARM_OBSTETRICIAN = "@ObstetricianName";
        private const string PARM_PEDIATRICIAN = "@PediatricianName";
        private const string PARM_RESPONSIBLE_PHYSICIAN = "@ResponsiblePhysicianId";

        //Maternal Delivery
        private const string PARM_GESTATION = "@Gestation";
        private const string PARM_NUMBEROF_FETUSES = "@NumberOfFetuses";
        private const string PARM_NUMBEROF_LIVINGFETUSES = "@NumberOfLivingFetuses";
        private const string PARM_LABOR_LENGTH = "@LaborLength";
        private const string PARM_DELIVERY_METHOD_ID = "@DeliveryMethodId";
        private const string PARM_MATERNAL_HISTORY_ID = "@MaternalHistoryId";
        private const string PARM_DELIVERY_PRESENTATION_ID = "@DeliveryPresentationId";

        //New Born
        private const string PARM_HEAD_CIRCUM = "@HeadCircumference";
        private const string PARM_CHEST_CIRCUM = "@ChestCircumference";
        private const string PARM_WEIGHT_AT_BIRTH = "@WeightAtBirth";
        private const string PARM_LENGTH_AT_BIRTH = "@LengthAtBirth";
        private const string PARM_APGAR_AT_BIRTH = "@ApgarAtBirth";
        private const string PARM_APGAR_AT_5MIN = "@ApgarAt5Minutes";
        private const string PARM_WEIGHT_RELEASED = "@WeightReleased";
        private const string PARM_BLOODTYPE_ID = "@PatientBloodTypeId";
        private const string PARM_PROBLEMS_AT_BIRTH_ID = "@ProblemsAtBirthId";
        private const string PARM_BFATAL_DISTRESS = "@bFetalDistress";

        /*
           Change Implement BY: Muhammad Azhar Shahzad
           Reason:Paramters For Soap Text and attachement detachment of social history with progress note
           Created Date: JAN 07, 2016
       */
        private const string PARM_NOTE_ID = "@NoteId";
        private const string PARM_SOAPTEXT_BY = "@SoapText";
        //end azhar changed

        #endregion

        #region "Birth History Lookups"
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : lookup function for Birth Deliver methods.
        /// Date : 6 january 2016
        /// </summary>
        /// <returns></returns>
        public DSBirthHistory lookupBirthHxDeliveryMethods()
        {
            DSBirthHistory ds = new DSBirthHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSBirthHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_DELIVERYMETHODS_LOOKUP, ds, ds.BirthHx_MaternalDelivery_DeliveryMethod.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBirthHistory::lookupBirthHxDeliveryMethods", PROC_BIRTHHX_DELIVERYMETHODS_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : lookup function for Birth Deliver Presentations.
        /// Date : 6 january 2016
        /// </summary>
        /// <returns></returns>
        public DSBirthHistory lookupBirthHxDeliveryPresentation()
        {
            DSBirthHistory ds = new DSBirthHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSBirthHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_DELIVERYPRESENTATION_LOOKUP, ds, ds.BirthHx_MaternalDelivery_DeliveryPresentation.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBirthHistory::lookupBirthHxDeliveryPresentation", PROC_BIRTHHX_DELIVERYPRESENTATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : lookup function for Birth Maternal History.
        /// Date : 6 january 2016
        /// </summary>
        /// <returns></returns>
        public DSBirthHistory lookupBirthHxMaternalHistory()
        {
            DSBirthHistory ds = new DSBirthHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSBirthHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_MATERNALHISTORY_LOOKUP, ds, ds.BirthHx_MaternalDelivery_MaternalHistory.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBirthHistory::lookupBirthHxMaternalHistory", PROC_BIRTHHX_MATERNALHISTORY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /*This Lookup is used For New Born Problems At Birth in Birth History, 
         Author: ZeeshanAK
         Date: January 05, 2016*/
        public DSBirthHistory lookupBirthHxNewbornProblemsAtBirth()
        {
            DSBirthHistory ds = new DSBirthHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSBirthHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_NEWBORN_PROBLEMSATBIRTHLOOKUP, ds, ds.BirthHx_Newborn_ProblemsAtBirth.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBirthHistory::lookupBirthHxNewbornProblemsAtBirth", PROC_BIRTHHX_NEWBORN_PROBLEMSATBIRTHLOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /*This Lookup is used For new born tab in Birth History, 
         Author: Muhammad Azhar Shahzad
         Date: January 05, 2016*/
        public DSBirthHistory birthHxNewbornPatientBloodTypeLookup()
        {
            DSBirthHistory ds = new DSBirthHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSBirthHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_NEWBORN_PATIENT_BLOODTYPE_LOOKUP, ds, ds.BirthHx_Newborn_PatientBloodType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProblemLists::LookupNoteAction", PROC_BIRTHHX_NEWBORN_PATIENT_BLOODTYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Support Functions Birth History"

        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : Function to to Add Parameters for BirthHx.
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createParametersBirthHx(IDBManager dbManager, DSBirthHistory ds, Boolean isInsert)
        {
            dbManager.CreateParameters(12);

            if (isInsert == true)
                dbManager.AddParameters(0, PARM_BIRTHHX_ID, ds.BirthHx.BirthHxIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_BIRTHHX_ID, ds.BirthHx.BirthHxIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_PATIENT_ID, ds.BirthHx.PatientIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_BIRTHHXDATE, ds.BirthHx.BirthHxDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(3, PARM_BUNREMARKABLE, ds.BirthHx.bUnremarkableColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_COMMENTS, ds.BirthHx.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_SOAPTEXT, ds.BirthHx.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_IS_ACTIVE, ds.BirthHx.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(7, PARM_CREATED_BY, ds.BirthHx.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_CREATED_ON, ds.BirthHx.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(9, PARM_MODIFIED_BY, ds.BirthHx.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_MODIFIED_ON, ds.BirthHx.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(11, PARM_NOTE_ID, ds.BirthHx.NotesIdColumn.ColumnName, DbType.Int64);

        }
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : Function to to Add Parameters for BirthHx  General.
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createParametersBirthHxGeneral(IDBManager dbManager, DSBirthHistory ds, Boolean isInsert)
        {

            dbManager.CreateParameters(18);
            if (isInsert == true)
            {
                dbManager.AddParameters(0, PARM_BIRTHHX_GENERAL_ID, ds.BirthHx_General.GeneralIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                dbManager.AddParameters(17, "@tempGeneralId", ds.BirthHx_General.GeneralIdColumn.ColumnName, DbType.Int64);

            }
            else
            {
                dbManager.AddParameters(0, PARM_BIRTHHX_GENERAL_ID, ds.BirthHx_General.GeneralIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(17, "@ColumnsUpdatedFromMobileApp", ds.BirthHx_General.ColumnsUpdatedFromMobileAppColumn.ColumnName, DbType.String);

            }

            dbManager.AddParameters(1, PARM_BIRTHHX_ID, ds.BirthHx_General.BirthHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_HOSPITAL_NAME, ds.BirthHx_General.HospitalNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_PATIENTDOB, ds.BirthHx_General.PatientDOBColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(4, PARM_LENGTH_OF_STAY_AT_HOSPITAL, ds.BirthHx_General.LengthStayatHospitalColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_DATE_ADMITTED, ds.BirthHx_General.DateAdmittedColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(6, PARM_OBSTETRICIAN, ds.BirthHx_General.ObstetricianNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_PEDIATRICIAN, ds.BirthHx_General.PediatricianNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_RESPONSIBLE_PHYSICIAN, ds.BirthHx_General.ResponsiblePhysicianIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(9, PARM_COMMENTS, ds.BirthHx_General.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_IS_ACTIVE, ds.BirthHx_General.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(11, PARM_CREATED_BY, ds.BirthHx_General.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CREATED_ON, ds.BirthHx_General.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(13, PARM_MODIFIED_BY, ds.BirthHx_General.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_MODIFIED_ON, ds.BirthHx_General.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(15, PARM_SOAPTEXT, ds.BirthHx_General.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, "@AddFromMobile", ds.BirthHx_General.AddFromMobileColumn.ColumnName, DbType.String);


        }
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : Function to to Add Parameters for BirthHx Maternal Delivery.
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createParametersBirthHxMaternalDelivery(IDBManager dbManager, DSBirthHistory ds, Boolean isInsert)
        {
            dbManager.CreateParameters(19);

            if (isInsert == true)
            {
                dbManager.AddParameters(0, PARM_BIRTHHX_MATERNALDELIVERY_ID, ds.BirthHx_MaternalDelivery.MaternalDeliveryIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(18, "@tempMaternalDeliveryId", ds.BirthHx_MaternalDelivery.MaternalDeliveryIdColumn.ColumnName, DbType.Int64);
            }
            else
            {
                dbManager.AddParameters(0, PARM_BIRTHHX_MATERNALDELIVERY_ID, ds.BirthHx_MaternalDelivery.MaternalDeliveryIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(18, "@ColumnsUpdatedFromMobileApp", ds.BirthHx_MaternalDelivery.ColumnsUpdatedFromMobileAppColumn.ColumnName, DbType.String);
            }
            dbManager.AddParameters(1, PARM_BIRTHHX_ID, ds.BirthHx_MaternalDelivery.BirthHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_GESTATION, ds.BirthHx_MaternalDelivery.GestationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_NUMBEROF_FETUSES, ds.BirthHx_MaternalDelivery.NumberOfFetusesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_NUMBEROF_LIVINGFETUSES, ds.BirthHx_MaternalDelivery.NumberOfLivingFetusesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_LENGTH_OF_STAY_AT_HOSPITAL, ds.BirthHx_General.LengthStayatHospitalColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_LABOR_LENGTH, ds.BirthHx_MaternalDelivery.LaborLengthColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_DELIVERY_METHOD_ID, ds.BirthHx_MaternalDelivery.DeliveryMethodIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(8, PARM_DELIVERY_PRESENTATION_ID, ds.BirthHx_MaternalDelivery.DeliveryPresentationIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(9, PARM_MATERNAL_HISTORY_ID, ds.BirthHx_MaternalDelivery.MaternalHistoryIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(10, PARM_COMMENTS, ds.BirthHx_MaternalDelivery.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_IS_ACTIVE, ds.BirthHx_MaternalDelivery.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(12, PARM_CREATED_BY, ds.BirthHx_MaternalDelivery.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_CREATED_ON, ds.BirthHx_MaternalDelivery.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_MODIFIED_BY, ds.BirthHx_MaternalDelivery.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_MODIFIED_ON, ds.BirthHx_MaternalDelivery.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(16, PARM_SOAPTEXT, ds.BirthHx_MaternalDelivery.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, "@AddFromMobile", ds.BirthHx_MaternalDelivery.AddFromMobileColumn.ColumnName, DbType.String);
        }
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : Function to to Add Parameters for BirthHx New Born.
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="ds"></param>
        /// <param name="isInsert"></param>
        private void createParametersBirthHxNewBorn(IDBManager dbManager, DSBirthHistory ds, Boolean isInsert)
        {
            dbManager.CreateParameters(21);

            if (isInsert == true)
            {
                dbManager.AddParameters(0, PARM_BIRTHHX_NEWBORN_ID, ds.BirthHx_Newborn.NewbornIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                dbManager.AddParameters(20, "@tempNewbornId", ds.BirthHx_Newborn.NewbornIdColumn.ColumnName, DbType.Int64);
            }
            else
            {
                dbManager.AddParameters(0, PARM_BIRTHHX_NEWBORN_ID, ds.BirthHx_Newborn.NewbornIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(20, "@ColumnsUpdatedFromMobileApp", ds.BirthHx_Newborn.ColumnsUpdatedFromMobileAppColumn.ColumnName, DbType.String);
            }

            dbManager.AddParameters(1, PARM_BIRTHHX_ID, ds.BirthHx_Newborn.BirthHxIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_HEAD_CIRCUM, ds.BirthHx_Newborn.HeadCircumferenceColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(3, PARM_CHEST_CIRCUM, ds.BirthHx_Newborn.ChestCircumferenceColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(4, PARM_WEIGHT_AT_BIRTH, ds.BirthHx_Newborn.WeightAtBirthColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(5, PARM_LENGTH_AT_BIRTH, ds.BirthHx_Newborn.LengthAtBirthColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(6, PARM_APGAR_AT_BIRTH, ds.BirthHx_Newborn.ApgarAtBirthColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_APGAR_AT_5MIN, ds.BirthHx_Newborn.ApgarAt5MinutesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_WEIGHT_RELEASED, ds.BirthHx_Newborn.WeightReleasedColumn.ColumnName, DbType.Double);
            dbManager.AddParameters(9, PARM_BLOODTYPE_ID, ds.BirthHx_Newborn.PatientBloodTypeIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(10, PARM_PROBLEMS_AT_BIRTH_ID, ds.BirthHx_Newborn.ProblemsAtBirthIdColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(11, PARM_BFATAL_DISTRESS, ds.BirthHx_Newborn.bFetalDistressColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(12, PARM_COMMENTS, ds.BirthHx_Newborn.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_SOAPTEXT, ds.BirthHx_Newborn.SoapTextColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_IS_ACTIVE, ds.BirthHx_Newborn.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(15, PARM_CREATED_BY, ds.BirthHx_Newborn.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_CREATED_ON, ds.BirthHx_Newborn.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(17, PARM_MODIFIED_BY, ds.BirthHx_Newborn.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_MODIFIED_ON, ds.BirthHx_Newborn.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(19, "@AddFromMobile", ds.BirthHx_Newborn.AddFromMobileColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Birth History Insert/Update/Delete/Select"
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Insert BirthHx.
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSBirthHistory insertBirthHx(DSBirthHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.BirthHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                createParametersBirthHx(dbManager, ds, true);
                ds = (DSBirthHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_INSERT, ds, ds.BirthHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BirthHx.Rows[0][ds.BirthHx.BirthHxIdColumn].ToString(), null, ds.BirthHx.Rows[0][ds.BirthHx.BirthHxIdColumn].ToString());
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

                MDVLogger.DALErrorLog("DALBirthHistory::insertBirthHx", PROC_BIRTHHX_INSERT, ex);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for update BirthHx.
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSBirthHistory updateBirthHx(DSBirthHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.BirthHx.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                this.createParametersBirthHx(dbManager, ds, false);
                ds = (DSBirthHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_UPDATE, ds, ds.BirthHx.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BirthHx.Rows[0][ds.BirthHx.BirthHxIdColumn].ToString(), null, ds.BirthHx.Rows[0][ds.BirthHx.BirthHxIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALBirthHistory::updateBirthHx", PROC_BIRTHHX_UPDATE, ex);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for delete BirthHx.
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="birthHxId"></param>
        /// <returns></returns>
        public string deleteBirthHx(long birthHxId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSBirthHistory dsCurrentBirthHx = loadBirthHx(0, Convert.ToInt64(birthHxId), "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BIRTHHX_ID, birthHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BIRTHHX_DELETE).ToString();

                if (returnVal != "")
                {
                    throw new Exception(returnVal);
                }
                else
                {
                    DataTable dtTemp = dsCurrentBirthHx.BirthHx;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, birthHxId.ToString(), null, birthHxId.ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALBirthHistory::deleteBirthHx", PROC_BIRTHHX_DELETE, ex);
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

        public BirthHxModel LoadNoteBirthHx(long PatientId, long UserId, long EntityId, long NoteId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();

            BirthHxModel model = new BirthHxModel();
            try
            {
                dbManager.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, PatientId));
                parameters.Add(new SqlParameter(PARM_USER_ID, UserId));
                parameters.Add(new SqlParameter(PARM_ENTITY_ID, EntityId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, NoteId));

                using (var reader = dbManager.ExecuteReader(PROC_BIRTHHX_SEELCTForSoapText, parameters))
                {
                    while (reader.Read())
                    {
                        var properties = typeof(BirthHxModel).GetProperties();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBirthHistory::LoadNoteBirthHx", PROC_BIRTHHX_SEELCTForSoapText, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for load BirthHx.
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="birthHxId"></param>
        /// <returns></returns>
        public DSBirthHistory loadBirthHx(long patientId, long birthHxId, string isViewMedicalHx = "", string isPrintMedicalHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSBirthHistory ds = new DSBirthHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                //dbManager.Open();
                dbManager.CreateParameters(2);

                if (patientId == 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientId);
                if (birthHxId == 0)
                    dbManager.AddParameters(1, PARM_BIRTHHX_ID, null);
                else
                    dbManager.AddParameters(1, PARM_BIRTHHX_ID, birthHxId);
                ds = (DSBirthHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_SEELCT, ds, ds.BirthHx.TableName);
                if (ds.BirthHx.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.BirthHx.Rows[0]["BirthHxId"]) > 0)
                    {

                        DataTable dtTemp = ds.BirthHx;
                        if (dtTemp != null)
                        {
                            if (isViewMedicalHx == "1" || isPrintMedicalHx == "1")
                            {
                                bool isViewAction = isViewMedicalHx == "1" ? true : false;
                                bool isPrintAcion = isPrintMedicalHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BirthHx.Rows[0][ds.BirthHx.BirthHxIdColumn].ToString(), null, ds.BirthHx.Rows[0][ds.BirthHx.BirthHxIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALBirthHistory::loadBirthHx", PROC_BIRTHHX_SEELCT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Birth History (General) Insert/Update/Delete/Select"
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Insert BirthHx (General).
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSBirthHistory insertBirthHxGeneral(DSBirthHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.BirthHx_General.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                createParametersBirthHxGeneral(dbManager, ds, true);
                ds = (DSBirthHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_GENERAL_INSERT, ds, ds.BirthHx_General.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BirthHx_General.Rows[0][ds.BirthHx_General.GeneralIdColumn].ToString(), null, ds.BirthHx_General.Rows[0][ds.BirthHx_General.BirthHxIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALBirthHistory::insertBirthHxGeneral", PROC_BIRTHHX_GENERAL_INSERT, ex);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Update BirthHx (General).
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSBirthHistory updateBirthHxGeneral(DSBirthHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.BirthHx_General.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                this.createParametersBirthHxGeneral(dbManager, ds, false);
                ds = (DSBirthHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_GENERAL_UPDATE, ds, ds.BirthHx_General.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BirthHx_General.Rows[0][ds.BirthHx_General.GeneralIdColumn].ToString(), null, ds.BirthHx_General.Rows[0][ds.BirthHx_General.BirthHxIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALBirthHistory::updateBirthHxGeneral", PROC_BIRTHHX_GENERAL_UPDATE, ex);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Delete BirthHx (General).
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="generalId"></param>
        /// <returns></returns>
        public string deleteBirthHxGeneral(long generalId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSBirthHistory dsCurrentBirthHx = loadBirthHxGeneral(0, Convert.ToInt64(generalId), "", "");


                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BIRTHHX_ID, generalId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BIRTHHX_GENERAL_DELETE).ToString();

                if (returnVal != "")
                {
                    throw new Exception(returnVal);
                }
                else
                {
                    DataTable dtTemp = dsCurrentBirthHx.BirthHx_General;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, generalId.ToString(), null, dsCurrentBirthHx.BirthHx_General.Rows[0][dsCurrentBirthHx.BirthHx_General.BirthHxIdColumn].ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALBirthHistory::deleteBirthHxGeneral", PROC_BIRTHHX_GENERAL_DELETE, ex);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Load BirthHx (General).
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="birthHxId"></param>
        /// <param name="generalId"></param>
        /// <returns></returns>
        public DSBirthHistory loadBirthHxGeneral(long birthHxId, long generalId, string isViewMedicalHx = "", string isPrintMedicalHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSBirthHistory ds = new DSBirthHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                //dbManager.Open();
                dbManager.CreateParameters(2);

                if (birthHxId == 0)
                    dbManager.AddParameters(0, PARM_BIRTHHX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BIRTHHX_ID, birthHxId);
                if (generalId == 0)
                    dbManager.AddParameters(1, PARM_BIRTHHX_GENERAL_ID, null);
                else
                    dbManager.AddParameters(1, PARM_BIRTHHX_GENERAL_ID, generalId);
                ds = (DSBirthHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_GENERAL_SEELCT, ds, ds.BirthHx_General.TableName);
                if (ds.BirthHx_General.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.BirthHx_General.Rows[0]["GeneralId"]) > 0)
                    {

                        DataTable dtTemp = ds.BirthHx_General;
                        if (dtTemp != null)
                        {
                            if (isViewMedicalHx == "1" || isPrintMedicalHx == "1")
                            {
                                bool isViewAction = isViewMedicalHx == "1" ? true : false;
                                bool isPrintAcion = isPrintMedicalHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BirthHx_General.Rows[0][ds.BirthHx_General.GeneralIdColumn].ToString(), null, ds.BirthHx_General.Rows[0][ds.BirthHx_General.BirthHxIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALBirthHistory::loadBirthHxGeneral", PROC_BIRTHHX_GENERAL_SEELCT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Birth History (Maternal Delivery) Insert/Update/Delete/Select"
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Insert BirthHx (Maternal Delivery).
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSBirthHistory insertBirthHxMaternalDelivery(DSBirthHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.BirthHx_MaternalDelivery.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                createParametersBirthHxMaternalDelivery(dbManager, ds, true);
                ds = (DSBirthHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_MATERNALDELIVERY_INSERT, ds, ds.BirthHx_MaternalDelivery.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BirthHx_MaternalDelivery.Rows[0][ds.BirthHx_MaternalDelivery.MaternalDeliveryIdColumn].ToString(), null, ds.BirthHx_MaternalDelivery.Rows[0][ds.BirthHx_MaternalDelivery.BirthHxIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALBirthHistory::insertBirthHxMaternalDelivery", PROC_BIRTHHX_MATERNALDELIVERY_INSERT, ex);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Update BirthHx (Maternal Delivery).
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSBirthHistory updateBirthHxMaternalDelivery(DSBirthHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.BirthHx_MaternalDelivery.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                this.createParametersBirthHxMaternalDelivery(dbManager, ds, false);
                ds = (DSBirthHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_MATERNALDELIVERY_UPDATE, ds, ds.BirthHx_MaternalDelivery.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BirthHx_MaternalDelivery.Rows[0][ds.BirthHx_MaternalDelivery.MaternalDeliveryIdColumn].ToString(), null, ds.BirthHx_MaternalDelivery.Rows[0][ds.BirthHx_MaternalDelivery.BirthHxIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALBirthHistory::updateBirthHxMaternalDelivery", PROC_BIRTHHX_MATERNALDELIVERY_UPDATE, ex);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Delete BirthHx (Maternal Delivery).
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="maternalDeliveryId"></param>
        /// <returns></returns>
        public string deleteBirthHxMaternalDelivery(long maternalDeliveryId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                //dbManager.Open();
                dbManager.BeginTransaction();
                DSBirthHistory dsCurrentBirthHx = loadBirthHxMaternalDelivery(0, Convert.ToInt64(maternalDeliveryId), "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BIRTHHX_MATERNALDELIVERY_ID, maternalDeliveryId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BIRTHHX_MATERNALDELIVERY_DELETE).ToString();

                if (returnVal != "")
                {
                    throw new Exception(returnVal);
                }

                else
                {
                    DataTable dtTemp = dsCurrentBirthHx.BirthHx_MaternalDelivery;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, maternalDeliveryId.ToString(), null, dsCurrentBirthHx.BirthHx_MaternalDelivery.Rows[0][dsCurrentBirthHx.BirthHx_MaternalDelivery.BirthHxIdColumn].ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALBirthHistory::deleteBirthHxMaternalDelivery", PROC_BIRTHHX_MATERNALDELIVERY_DELETE, ex);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Load BirthHx (Maternal Delivery).
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="birthHxId"></param>
        /// <param name="maternalDeliveryId"></param>
        /// <returns></returns>
        public DSBirthHistory loadBirthHxMaternalDelivery(long birthHxId, long maternalDeliveryId, string isViewMedicalHx = "", string isPrintMedicalHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSBirthHistory ds = new DSBirthHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                //dbManager.Open();
                dbManager.CreateParameters(2);

                if (birthHxId == 0)
                    dbManager.AddParameters(0, PARM_BIRTHHX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BIRTHHX_ID, birthHxId);
                if (maternalDeliveryId == 0)
                    dbManager.AddParameters(1, PARM_BIRTHHX_MATERNALDELIVERY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_BIRTHHX_MATERNALDELIVERY_ID, maternalDeliveryId);
                ds = (DSBirthHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_MATERNALDELIVERY_SEELCT, ds, ds.BirthHx_MaternalDelivery.TableName);
                if (ds.BirthHx_MaternalDelivery.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.BirthHx_MaternalDelivery.Rows[0]["MaternalDeliveryId"]) > 0)
                    {

                        DataTable dtTemp = ds.BirthHx_MaternalDelivery;
                        if (dtTemp != null)
                        {
                            if (isViewMedicalHx == "1" || isPrintMedicalHx == "1")
                            {
                                bool isViewAction = isViewMedicalHx == "1" ? true : false;
                                bool isPrintAcion = isPrintMedicalHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BirthHx_MaternalDelivery.Rows[0][ds.BirthHx_MaternalDelivery.MaternalDeliveryIdColumn].ToString(), null, ds.BirthHx_MaternalDelivery.Rows[0][ds.BirthHx_MaternalDelivery.BirthHxIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALBirthHistory::loadBirthHxMaternalDelivery", PROC_BIRTHHX_MATERNALDELIVERY_SEELCT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        #region "Birth History (NewBorn) Insert/Update/Delete/Select"
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Insert BirthHx (New Born).
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSBirthHistory insertBirthHistoryNewBorn(DSBirthHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.BirthHx_Newborn.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                createParametersBirthHxNewBorn(dbManager, ds, true);
                ds = (DSBirthHistory)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_NEWBORN_INSERT, ds, ds.BirthHx_Newborn.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BirthHx_Newborn.Rows[0][ds.BirthHx_Newborn.NewbornIdColumn].ToString(), null, ds.BirthHx_Newborn.Rows[0][ds.BirthHx_Newborn.BirthHxIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALBirthHistory::insertBirthHistoryNewBorn", PROC_BIRTHHX_NEWBORN_INSERT, ex);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Update BirthHx (New Born).
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DSBirthHistory updateBirthHistoryNewBorn(DSBirthHistory ds)
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                DataTable dtTemp = ds.BirthHx_Newborn.GetChanges();
                //dbManager.Open();
                dbManager.BeginTransaction();
                this.createParametersBirthHxNewBorn(dbManager, ds, false);
                ds = (DSBirthHistory)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_NEWBORN_UPDATE, ds, ds.BirthHx_Newborn.TableName);
                if (dtTemp != null)
                {
                    dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BirthHx_Newborn.Rows[0][ds.BirthHx_Newborn.NewbornIdColumn].ToString(), null, ds.BirthHx_Newborn.Rows[0][ds.BirthHx_Newborn.BirthHxIdColumn].ToString());
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
                MDVLogger.DALErrorLog("DALBirthHistory::updateBirthHistoryNewBorn", PROC_BIRTHHX_NEWBORN_UPDATE, ex);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Delete BirthHx (New Born).
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="birthHxId"></param>
        /// <returns></returns>
        public string deleteBirthHistoryNewBorn(long birthHxId)
        {
            string returnVal = "";
            DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //dbManager.Open();
                dbManager.BeginTransaction();
                DSBirthHistory dsCurrentBirthHx = loadBirthHistoryNewBorn(0, Convert.ToInt64(birthHxId), "", "");

                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BIRTHHX_NEWBORN_ID, birthHxId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BIRTHHX_NEWBORN_DELETE).ToString();

                if (returnVal != "")
                {
                    throw new Exception(returnVal);
                }
                else
                {
                    DataTable dtTemp = dsCurrentBirthHx.BirthHx_Newborn;//.GetChanges();
                    if (dtTemp != null)
                    {
                        dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, birthHxId.ToString(), null, dsCurrentBirthHx.BirthHx_Newborn.Rows[0][dsCurrentBirthHx.BirthHx_Newborn.BirthHxIdColumn].ToString(), false, false, true);
                        dsDBAudit.AcceptChanges();
                    }
                    dbManager.CommitTransaction();
                }

                return "";
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALBirthHistory::deleteBirthHistoryNewBorn", PROC_BIRTHHX_NEWBORN_DELETE, ex);
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : function for Load BirthHx (New Born).
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="birthHxId"></param>
        /// <param name="newbornId"></param>
        /// <returns></returns>
        public DSBirthHistory loadBirthHistoryNewBorn(long birthHxId, long newbornId, string isViewMedicalHx = "", string isPrintMedicalHx = "")
        {
            DSDBAudit dsDBAudit = new DSDBAudit();
            DSBirthHistory ds = new DSBirthHistory();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.BeginTransaction();
                //dbManager.Open();
                dbManager.CreateParameters(2);

                if (birthHxId == 0)
                    dbManager.AddParameters(0, PARM_BIRTHHX_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BIRTHHX_ID, birthHxId);
                if (newbornId == 0)
                    dbManager.AddParameters(1, PARM_BIRTHHX_NEWBORN_ID, null);
                else
                    dbManager.AddParameters(1, PARM_BIRTHHX_NEWBORN_ID, newbornId);
                ds = (DSBirthHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BIRTHHX_NEWBORN_SEELCT, ds, ds.BirthHx_Newborn.TableName);
                if (ds.BirthHx_Newborn.Rows.Count > 0)
                {
                    if (Convert.ToInt64(ds.BirthHx_Newborn.Rows[0]["NewbornId"]) > 0)
                    {

                        DataTable dtTemp = ds.BirthHx_Newborn;
                        if (dtTemp != null)
                        {
                            if (isViewMedicalHx == "1" || isPrintMedicalHx == "1")
                            {
                                bool isViewAction = isViewMedicalHx == "1" ? true : false;
                                bool isPrintAcion = isPrintMedicalHx == "1" ? true : false;
                                dsDBAudit = new DBActivityAudit().InsertDBAudit(dtTemp, dbManager, ds.BirthHx_Newborn.Rows[0][ds.BirthHx_Newborn.NewbornIdColumn].ToString(), null, ds.BirthHx_Newborn.Rows[0][ds.BirthHx_Newborn.BirthHxIdColumn].ToString(), isViewAction, isPrintAcion);
                                dsDBAudit.AcceptChanges();
                            }
                        }
                    }
                }
                dbManager.CommitTransaction();
                return ds;
            }
            catch (Exception ex)
            {
                dsDBAudit.RejectChanges();
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DALBirthHistory::loadBirthHistoryNewBorn", PROC_BIRTHHX_NEWBORN_SEELCT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion


        /*
            Change Implement BY: Muhammad Azhar Shahzad
            Reason: Functions For Soap Text and attachement detachment of birth history with progress note
            Created Date: JAN 07, 2016
        */
        #region Notes and Birth History
        /// <summary>
        /// Author : Azhar Shahzad.
        /// Purpose : Detaching Birth History From Progress notes
        /// Date : 6 january 2016
        /// </summary>
        /// <param name="birthHxId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public string detachBirthHxFromNotes(long birthHxId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (birthHxId <= 0)
                {
                    dbManager.AddParameters(0, PARM_BIRTHHX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_BIRTHHX_ID, birthHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }

                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_BIRTHHX_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBirthHistory::detachBirthHxFromNotes", PROC_DETACH_BIRTHHX_FROM_NOTES, ex);
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
        /// Author : Azhar Shahzad.
        /// Purpose : Attaching Birth History With Progress notes
        /// Date : 6 january 2016
        /// <param name="birthHxId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        public DSBirthHistory attachBirthHxWithNotes(long birthHxId, long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                DSBirthHistory ds = new DSBirthHistory();

                dbManager.Open();

                dbManager.CreateParameters(2);
                if (birthHxId <= 0)
                {
                    dbManager.AddParameters(0, PARM_BIRTHHX_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_BIRTHHX_ID, birthHxId);
                }

                if (NotesId == 0)
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, DBNull.Value);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_NOTE_ID, NotesId);
                }


                ds = (DSBirthHistory)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ATTACH_BIRTHHX_FROM_NOTES, ds, ds.BirthHx.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBirthHistory::attachBirthHxWithNotes", PROC_ATTACH_BIRTHHX_FROM_NOTES, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /*
          Change Implement BY: Muhammad Azhar Shahzad
          Reason: Function to get updated Soap text of  social history for progress note
          Created Date: Dec 07, 2016
      */
        public string updateSoapTextForBirthHX(long birthHxId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_BIRTHHX_ID, birthHxId);

                dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_UPDATE_SOAPTEXT_FOR_BIRTHHX);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBirthHistory::updateSoapTextForBirthHX", PROC_UPDATE_SOAPTEXT_FOR_BIRTHHX, ex);
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

        // end azhar changed jan 07,2016
        #endregion

        #region Legacy Notes

        public List<BirthHx> NotesBirthHxSelect(CommonSearch objCommonSearch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<BirthHx> objList_BirthHx = new List<BirthHx>();
            try
            {
                dbManager.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter(PARM_PATIENT_ID, objCommonSearch.PatientId));
                parameters.Add(new SqlParameter(PARM_NOTE_ID, objCommonSearch.NotesId));
                using (var reader = dbManager.ExecuteReader(PROC_NOTES_BIRTHH_SELECT, parameters))
                {
                    while (reader.Read())
                    {
                        objList_BirthHx.Add(new BirthHx
                        {
                            BirthHxId = MDVUtility.ToInt64(reader["BirthHxId"]),
                            ID = MDVUtility.ToInt32(reader["ID"]),
                            BirthHxName = MDVUtility.ToStr(reader["BirthHxName"]),
                            HospitalName = MDVUtility.ToStr(reader["HospitalName"]),
                            PatientDOB = reader["PatientDOB"] != DBNull.Value ? MDVUtility.ToDateTime(reader["PatientDOB"]) :(DateTime?) null,
                            LengthStayatHospital = MDVUtility.ToStr(reader["LengthStayatHospital"]),
                            DateAdmitted = reader["DateAdmitted"] != DBNull.Value ? MDVUtility.ToDateTime(reader["DateAdmitted"]) : (DateTime?)null,
                            ObstetricianName = MDVUtility.ToStr(reader["ObstetricianName"]),
                            PediatricianName = MDVUtility.ToStr(reader["PediatricianName"]),
                            ResponsiblePhysicianName = MDVUtility.ToStr(reader["ResponsiblePhysicianName"]),
                            Gestation = MDVUtility.ToStr(reader["Gestation"]),
                            NumberOfFetuses = MDVUtility.ToStr(reader["NumberOfFetuses"]),
                            NumberOfLivingFetuses = MDVUtility.ToStr(reader["NumberOfLivingFetuses"]),
                            LaborLength = MDVUtility.ToStr(reader["LaborLength"]),
                            DeliveryMethod = MDVUtility.ToStr(reader["DeliveryMethod"]),
                            DeliveryPresentation = MDVUtility.ToStr(reader["DeliveryPresentation"]),
                            MaternalHistory = MDVUtility.ToStr(reader["MaternalHistory"]),
                            HeadCircumference = MDVUtility.ToStr(reader["HeadCircumference"]),
                            ChestCircumference = MDVUtility.ToStr(reader["ChestCircumference"]),
                            WeightAtBirth = MDVUtility.ToStr(reader["WeightAtBirth"]),
                            LengthAtBirth = MDVUtility.ToStr(reader["LengthAtBirth"]),
                            ApgarAtBirth = MDVUtility.ToStr(reader["ApgarAtBirth"]),
                            ApgarAt5Minutes = MDVUtility.ToStr(reader["ApgarAt5Minutes"]),
                            WeightReleased = MDVUtility.ToStr(reader["WeightReleased"]),
                            PatientBloodType = MDVUtility.ToStr(reader["PatientBloodType"]),
                            ProblemsAtBirth = MDVUtility.ToStr(reader["ProblemsAtBirth"]),
                            bFetalDistress = MDVUtility.ToBool(reader["bFetalDistress"]),
                            Comments = MDVUtility.ToStr(reader["Comments"]),
                            SoapText = MDVUtility.ToStr(reader["SoapText"]),
                            CreatedOn = MDVUtility.ToDateTime(reader["CreatedOn"]),
                            ModifiedOn = MDVUtility.ToDateTime(reader["ModifiedOn"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBirthHistory::NotesBirthHxSelect", PROC_NOTES_BIRTHH_SELECT, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return objList_BirthHx;
        }

        #endregion Legacy Notes

    }
}
