using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.Admin;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.DataAccess.DAL.Clinical.OrderSet;
using MDVision.DataAccess.DAL.Patient;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Model;
using MDVision.Model.Clinical.Medical;
using MDVision.Model.Patient;
using MDVision.Model.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using MDVision.Model.Clinical.FollowUp;
using MDVision.Model.Clinical.Templates.OrderSets;
using MDVision.Model.Clinical.Orderset;

namespace MDVision.Business.BLL
{
    public class BLLOrderSet
    {
        #region Constructors
        public BLLOrderSet()
        {
            //SharedVariable
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call
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


        #region "Order Set Region"

        public BLObject<string> insertOrderSet(OrderSetModel model)
        {
            try
            {
                string formId;
                model.EntityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
                formId = new DALOrderSet().insertOrderSet(model);
                return new BLObject<string>(formId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::insertOrderSet", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        public BLObject<DSOrderSet> detachOrderSetFromNotes(long PatientId, string OrderSetId, long NotesId)
        {
            try
            {
                DSOrderSet ds = new DALOrderSet().detachOrderSetFromNotes(PatientId, OrderSetId, NotesId);
                return new BLObject<DSOrderSet>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::detachOrderSetFromNotes", ex);
                return new BLObject<DSOrderSet>(null, ex.Message);
            }
        }


        public BLObject<string> updateOrderSet(OrderSetModel model)
        {
            try
            {
                string returnValue;
                returnValue = new DALOrderSet().updateOrderSet(model);
                return new BLObject<string>(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::updateOrderSet", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<List<OrderSetModel>> loadOrderSet(string formName, int? isActive, string providerIds, string specialityIds, long pageNumber, long rowsPerPage)
        {
            try
            {
                var result = new DALOrderSet().loadOrderSet(formName, isActive, providerIds, specialityIds, pageNumber, rowsPerPage);
                return new BLObject<List<OrderSetModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::loadOrderSet", ex);
                return new BLObject<List<OrderSetModel>>(null, ex.Message);
            }
        }

        public BLObject<List<OrderSetModel>> loadOrderSetName(String OrderSetIds)
        {
            try
            {
                var result = new DALOrderSet().loadOrderSetName(OrderSetIds);
                return new BLObject<List<OrderSetModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::loadOrderSetName", ex);
                return new BLObject<List<OrderSetModel>>(null, ex.Message);
            }
        }


        public BLObject<List<OrderSetModel>> fillOrderSet(string formId, string NoteId = "", string CDSId = "")
        {
            try
            {
                var result = new DALOrderSet().fillOrderSet(formId, NoteId, CDSId);
                return new BLObject<List<OrderSetModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::fillOrderSet", ex);
                return new BLObject<List<OrderSetModel>>(null, ex.Message);
            }
        }
        public BLObject<List<OrderSetProblemModel>> LoadOrderSetProblem(string OrderSetId, string OrderSetProblemId="")
        {
            try
            {
                var result = new DALOrderSet().LoadOrderSetProblem(OrderSetId, OrderSetProblemId);
                return new BLObject<List<OrderSetProblemModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadOrderSetProblem", ex);
                return new BLObject<List<OrderSetProblemModel>>(null, ex.Message);
            }
        }

        public BLObject<List<OrderSetProblemModel>> LoadPatientAndOrdProblems(string OrderSetId, string PatientId)
        {
            try
            {
                var result = new DALOrderSet().LoadPatientAndOrdProblems(OrderSetId, PatientId);
                return new BLObject<List<OrderSetProblemModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadPatientAndOrdProblems", ex);
                return new BLObject<List<OrderSetProblemModel>>(null, ex.Message);
            }
        }
        
        public BLObject<string> deleteOrderSetProblemList(string problemListId)
        {
            try
            {
                problemListId = new DALOrderSet().deleteOrderSetProblemList(MDVUtility.ToInt64(problemListId));
                return new BLObject<string>(problemListId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::deleteOrderSetProblemList", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> deleteOrderSet(string formId)
        {
            try
            {
                formId = new DALOrderSet().deleteOrderSet(formId);
                return new BLObject<string>(formId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::deleteOrderSet", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> updateOrderSetStatus(string formId, string isActive)
        {
            try
            {
                string returnValue;
                returnValue = new DALOrderSet().updateOrderSetStatus(formId, isActive);
                return new BLObject<string>(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::activeInactiveOrderSet", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> saveAsOrderSet(string OrderSetId, string OrderSetName , string DefaultFollowUpId)
        {
            try
            {
                string returnValue;
                returnValue = new DALOrderSet().saveAsOrderSet(OrderSetId, OrderSetName, DefaultFollowUpId);
                return new BLObject<string>(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::saveAsOrderSet", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "Problem Lists"

        public BLObject<DSOS_ProblemLists> InsertProblemLists(DSOS_ProblemLists ds)
        {
            try
            {
                ds = new DALOS_ProblemLists().InsertProblemLists(ds);
                return new BLObject<DSOS_ProblemLists>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::InsertProblemLists", ex);
                return new BLObject<DSOS_ProblemLists>(null, ex.Message);
            }
        }

        public BLObject<DSOS_ProblemLists> UpdateProblemLists(DSOS_ProblemLists ds)
        {
            try
            {
                ds = new DALOS_ProblemLists().UpdateProblemLists(ds);
                return new BLObject<DSOS_ProblemLists>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::UpdateProblemLists", ex);
                return new BLObject<DSOS_ProblemLists>(null, ex.Message);
            }
        }

        public BLObject<DSOS_ProblemLists> UpdateProblemListsOp(DSOS_ProblemLists ds)
        {
            try
            {
                ds = new DALOS_ProblemLists().UpdateProblemListsOp(ds);
                return new BLObject<DSOS_ProblemLists>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::UpdateProblemListsOp", ex);
                return new BLObject<DSOS_ProblemLists>(null, ex.Message);
            }
        }

        public BLObject<DSOS_ProblemLists> UpdateProblemListsForInActive(DSOS_ProblemLists ds)
        {
            try
            {
                ds = new DALOS_ProblemLists().UpdateProblemListsForInActive(ds);
                return new BLObject<DSOS_ProblemLists>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::UpdateProblemLists", ex);
                return new BLObject<DSOS_ProblemLists>(null, ex.Message);
            }
        }

        public BLObject<string> DeleteProblemLists(string ProblemListId)
        {
            try
            {
                ProblemListId = new DALOS_ProblemLists().DeleteProblemLists(ProblemListId);
                return new BLObject<string>(ProblemListId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::DeleteProblemLists", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> CheckProblemListExists(long OrderSetId)
        {
            try
            {
                string ProblemListExists = new DALOS_ProblemLists().CheckProblemListExists(OrderSetId);
                return new BLObject<string>(ProblemListExists);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::CheckProblemListExists", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSOS_ProblemLists> LoadProblemLists(long ProblemListId, long orderSetId, string isHistory = "0", string active = "", int pageNumber = 1, int rowsPerPage = 1000, string isViewProblemList = "", string isPrintProblemList = "")
        {
            try
            {
                DSOS_ProblemLists ds = new DSOS_ProblemLists();
                ds = new DALOS_ProblemLists().LoadProblemLists(ProblemListId, orderSetId, isHistory, active, pageNumber, rowsPerPage, isViewProblemList, isPrintProblemList);

                return new BLObject<DSOS_ProblemLists>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadProblemLists", ex);
                return new BLObject<DSOS_ProblemLists>(null, ex.Message);
            }
        }



        public BLObject<DSOS_ProblemLists> LoadProblemListsOp(long orderSetId, string isHistory = "0", string active = "", int pageNumber = 1, int rowsPerPage = 1000, string isViewProblemList = "", string isPrintProblemList = "")
        {
            try
            {
                DSOS_ProblemLists ds = new DSOS_ProblemLists();
                ds = new DALOS_ProblemLists().LoadProblemListsOp(orderSetId, isHistory, active, pageNumber, rowsPerPage, isViewProblemList, isPrintProblemList);

                return new BLObject<DSOS_ProblemLists>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadProblemListsOp", ex);
                return new BLObject<DSOS_ProblemLists>(null, ex.Message);
            }
        }

        public BLObject<DSClinicalComplaint> LoadAllComplaintsForFaceSheet(long orderSetId, int pageNumber = 1, int rowsPerPage = 1000, string isViewProblemList = "", string isPrintProblemList = "")
        {
            try
            {
                DSClinicalComplaint ds = new DSClinicalComplaint();
                ds = new DALComplaint().LoadAllComplaintsForFaceSheet(orderSetId, pageNumber, rowsPerPage, isViewProblemList, isPrintProblemList);

                return new BLObject<DSClinicalComplaint>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadAllComplaintsForFaceSheet", ex);
                return new BLObject<DSClinicalComplaint>(null, ex.Message);
            }
        }

        public BLObject<DSOS_ProblemLists> LoadProblemListsForInActive(long ProblemListId)
        {
            try
            {
                DSOS_ProblemLists ds = new DSOS_ProblemLists();
                ds = new DALOS_ProblemLists().LoadProblemListsForInActive(ProblemListId);

                return new BLObject<DSOS_ProblemLists>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadProblemListsForInActive", ex);
                return new BLObject<DSOS_ProblemLists>(null, ex.Message);
            }
        }

        public BLObject<DSOS_ProblemLists> LoadProblemListsForFillData(long ProblemListId, string isViewAction = "")
        {
            try
            {
                DSOS_ProblemLists ds = new DSOS_ProblemLists();
                ds = new DALOS_ProblemLists().LoadProblemListsForFillData(ProblemListId, isViewAction);

                return new BLObject<DSOS_ProblemLists>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadProblemListsForInActive", ex);
                return new BLObject<DSOS_ProblemLists>(null, ex.Message);
            }
        }
        #endregion

        #region " Order Set Patient Education "

        public BLObject<string> insertOrderSetPatientEducation(OrderSetPatientEducationModel model)
        {
            try
            {
                string formId;
                formId = new DALOrderSet().insertOrderSetPatientEducation(model);
                return new BLObject<string>(formId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::insertOrderSetPatientEducation", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> deleteOrderSetPatientEducation(string PatientEducationId, string DocId)
        {
            try
            {
                PatientEducationId = new DALOrderSet().deleteOrderSetPatientEducation(PatientEducationId, DocId);
                return new BLObject<string>(PatientEducationId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::deleteOrderSetPatientEducation", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        public BLObject<List<OrderSetPatientEducationModel>> loadOrderSetPatientEducation(long OrderSetPatEducationId, long OrderSetId, string DocType, long pageNumber, long rowsPerPage)
        {
            try
            {
                var result = new DALOrderSet().loadOrderSetPatientEducation(OrderSetPatEducationId, OrderSetId, DocType, pageNumber, rowsPerPage);
                return new BLObject<List<OrderSetPatientEducationModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::loadOrderSetPatientEducation", ex);
                return new BLObject<List<OrderSetPatientEducationModel>>(null, ex.Message);
            }
        }
        /// <summary>
        /// For inserting documents in OS Info Button
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSOrderSet> InsertAdmin_OSPatientEducation(DSOrderSet ds)
        {
            try
            {
                ds = new DALOrderSet().InsertAdmin_OSPatientEducation(ds);
                return new BLObject<DSOrderSet>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::InsertAdmin_OSPatientEducation", ex);
                return new BLObject<DSOrderSet>(null, ex.Message);
            }
        }
        #endregion

        #region " Order Set Patient Referrals "

        public BLObject<string> insertOrderSetPatientReferralsOutgoingDetail(OrderSetPatientReferralModel model)
        {
            try
            {
                string formId;
                model.XML = MDVUtility.GetXmlOfObject(typeof(List<ReferralProcedureModel>), model.ReferralProcedure);
                model.CreatedBy = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedBy = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedOn = DateTime.Now.ToString();

                if (string.IsNullOrEmpty(model.FacilityFrom))
                    model.FacilityFrom = null;

                if (string.IsNullOrEmpty(model.FacilityTo))
                    model.FacilityTo = null;


                formId = new DALOrderSet().insertOrderSetPatientReferralsOutgoingDetail(model);
                return new BLObject<string>(formId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::insertOrderSetPatientEducation", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> deleteOrderSetPatientReferralsOutgoingDetail(string OrderSetReferralId)
        {
            try
            {
                OrderSetReferralId = new DALOrderSet().deleteOrderSetPatientReferralsOutgoingDetail(OrderSetReferralId);
                return new BLObject<string>(OrderSetReferralId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::deleteOrderSetPatientReferralsOutgoingDetail", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        

        public BLObject<string> deleteOrderSetReferralsProcedure(string OrderSetReferralProcedureId)
        {
            try
            {
                OrderSetReferralProcedureId = new DALOrderSet().deleteOrderSetReferralsProcedure(OrderSetReferralProcedureId);
                return new BLObject<string>(OrderSetReferralProcedureId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::deleteOrderSetReferralsProcedure", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> updateOrderSetPatientReferralsOutgoingDetail(OrderSetPatientReferralModel model)
        {
            try
            {
                string formId;
                model.XML = MDVUtility.GetXmlOfObject(typeof(List<ReferralProcedureModel>), model.ReferralProcedure);
                model.ModifiedBy = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();

                if (string.IsNullOrEmpty(model.FacilityFrom))
                    model.FacilityFrom = null;

                if (string.IsNullOrEmpty(model.FacilityTo))
                    model.FacilityTo = null;


                formId = new DALOrderSet().updateOrderSetPatientReferralsOutgoingDetail(model);
                return new BLObject<string>(formId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::updateOrderSetPatientReferralsOutgoingDetail", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<List<OrderSetPatientReferralModel>> loadOrderSetPatientReferralsOutgoingDetail(long orderSetReferralId, long orderSetId, string IsActive, string procedureName, long providerId, long RefproviderId, string DateFrom, string DateTo, int status, string Pan, string Type, int pageNumber = 1, int rowsPerPage = 2000, string isViewOrder = "", string isPrintOrder = "")
        {
            try
            {
                var result = new DALOrderSet().loadOrderSetPatientReferralsOutgoingDetail(orderSetReferralId, orderSetId, IsActive, procedureName, providerId, RefproviderId, DateFrom, DateTo, status, Pan, Type, pageNumber, rowsPerPage, isViewOrder, isPrintOrder);
                return new BLObject<List<OrderSetPatientReferralModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::loadOrderSetPatientReferralsOutgoingDetail", ex);
                return new BLObject<List<OrderSetPatientReferralModel>>(null, ex.Message);
            }
        }
        public BLObject<List<ReferralProcedureModel>> loadOrderSetReferralsProcedure(long orderSetReferralId)
        {
            try
            {
                var result = new DALOrderSet().loadOrderSetReferralsProcedure(orderSetReferralId);
                return new BLObject<List<ReferralProcedureModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::loadOrderSetReferralsProcedure", ex);
                return new BLObject<List<ReferralProcedureModel>>(null, ex.Message);
            }
        }
        #endregion

        #region " Order Set FolowUp "

        public BLObject<string> insertOrderSetFollowUp(OrdertSetFollowUpModel model)
        {
            try
            {
                string formId;
                model.CreatedBy = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedBy = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedOn = DateTime.Now.ToString();

                formId = new DALOrderSet().insertOrderSetFollowUp(model);
                return new BLObject<string>(formId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::insertOrderSetFollowUp", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> deleteOrderSetFollowUp(string followUpId)
        {
            try
            {
                followUpId = new DALOrderSet().deleteOrderSetFollowUp(followUpId);
                return new BLObject<string>(followUpId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::deleteOrderSetFollowUp", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> updateOrderSetFollowUp(OrdertSetFollowUpModel model)
        {
            try
            {
                string formId;
                model.ModifiedBy = ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                formId = new DALOrderSet().updateOrderSetFollowUp(model);
                return new BLObject<string>(formId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::updateOrderSetFollowUp", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<List<OrdertSetFollowUpModel>> loadOrderSetFollowUp(long followUpId, long orderSetId, long pageNumber, long rowspPage)
        {
            try
            {
                var result = new DALOrderSet().loadOrderSetFollowUp(followUpId, orderSetId, pageNumber, rowspPage);
                return new BLObject<List<OrdertSetFollowUpModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadOrderSetFollowUp", ex);
                return new BLObject<List<OrdertSetFollowUpModel>>(null, ex.Message);
            }
        }

        #endregion

        #region Procedures
        public BLObject<DSOS_Procedures> loadProcedures(int ProcedureId, long orderSetId, string isHistory = "0", string active = "", int pageNumber = 1, int rowsPerPage = 1000, string isViewProcedure = "", string isPrintAllergy = "")
        {
            try
            {
                DSOS_Procedures ds = new DSOS_Procedures();
                ds = new DALOS_Procedures().loadProcedures(ProcedureId, orderSetId, isHistory, active, pageNumber, rowsPerPage, isViewProcedure, isPrintAllergy);
                return new BLObject<DSOS_Procedures>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::loadProcedures", ex);
                return new BLObject<DSOS_Procedures>(null, ex.Message);
            }
        }
        public BLObject<DSOS_Procedures> insertProcedure(DSOS_Procedures ds)
        {
            try
            {
                ds = new DALOS_Procedures().insertProcedure(ds);
                return new BLObject<DSOS_Procedures>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::insertProcedure", ex);
                return new BLObject<DSOS_Procedures>(null, ex.Message);
            }
        }

        public BLObject<DSOS_Procedures> updateProcedure(DSOS_Procedures ds)
        {
            try
            {
                ds = new DALOS_Procedures().updateProcedure(ds);
                return new BLObject<DSOS_Procedures>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::updateProcedure", ex);
                return new BLObject<DSOS_Procedures>(null, ex.Message);
            }
        }

        public BLObject<string> deleteProcedure(string procedureId, string VaccineHxId)
        {
            try
            {
                procedureId = new DALOS_Procedures().deleteProcedure(procedureId, VaccineHxId);
                return new BLObject<string>(procedureId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::deleteProcedure", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region Lab Order
        // Author:  Abid Ali
        // Created Date: 31/03/2016
        //OverView: Method to Insert/Update LabOrder
        public BLObject<DSOS_LabOrder> InsertUpdateLabOrder(DSOS_LabOrder ds)
        {
            try
            {
                ds = new DALOS_LabOrder().InsertUpdateLabOrder(ds);
                return new BLObject<DSOS_LabOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::InsertUpdateLabOrder", ex);
                return new BLObject<DSOS_LabOrder>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 31/03/2016
        //OverView: Method to Load LabOrder
        public BLObject<DSOS_LabOrder> LoadLabOrder(long orderSetId, string pageNumber, string rowsPerPage)
        {
            try
            {
                DSOS_LabOrder ds = new DSOS_LabOrder();
                //Start 21-03-2016 Humaira Yousaf
                ds = new DALOS_LabOrder().LoadLabOrder(orderSetId, pageNumber, rowsPerPage);
                //End 21-03-2016 Humaira Yousaf
                return new BLObject<DSOS_LabOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadLabOrder", ex);
                return new BLObject<DSOS_LabOrder>(null, ex.Message);
            }
        }


        public BLObject<string> SaveABNAgainstTest(long LabOrderTestId, bool IsABN)
        {
            try
            {
                string result = new DALOS_LabOrder().SaveABNAgainstTest(LabOrderTestId, IsABN);
                return new BLObject<string>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::SaveABNAgainstTest", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        // Author:  Azhar Shahzad
        // Created Date: Sep 01 2016
        //OverView: Lab test lookup
        public BLObject<List<RadiologyTestLookup>> LookupRadiologyTestReport(string Test)
        {
            try
            {

                var result = new DALRadiologyOrder().LookupRadiologyTestReport(Test);
                return new BLObject<List<RadiologyTestLookup>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LookupRadiologyTestReport", ex);
                return new BLObject<List<RadiologyTestLookup>>(null, ex.Message);
            }
        }
        // Author:  Abid Ali
        // Created Date: 31/03/2016
        //OverView: labOrderChangePatient
        public BLObject<string> labOrderChangePatient(long labOrderId, long PatientId)
        {
            try
            {


                var result = new DALOS_LabOrder().labOrderChangePatient(labOrderId, PatientId);
                return new BLObject<string>(result);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::labOrderChangePatient", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /*
         author: Azhar Shahzad
         Date Creation: May 27, 2016*/
        public BLObject<DSOS_LabOrder> LoadLabOrderPDF(long labOrderId, long patientId)
        {
            try
            {
                DSOS_LabOrder ds = new DSOS_LabOrder();
                ds = new DALOS_LabOrder().LoadLabOrderPDF(labOrderId, patientId);

                return new BLObject<DSOS_LabOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadLabOrderPDF", ex);
                return new BLObject<DSOS_LabOrder>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 31/03/2016
        //OverView: Method to Delete Lab Order
        public BLObject<string> DeleteLabOrder(string labOrderId)
        {
            try
            {
                labOrderId = new DALOS_LabOrder().DeleteLabOrder(labOrderId);
                return new BLObject<string>(labOrderId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::DeleteLabOrder", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 31/03/2016
        //OverView: Method to Insert/Update LabOrder Problems
        public BLObject<DSOS_LabOrder> InsertUpdateLabOrderProblems(DSOS_LabOrder ds)
        {
            try
            {
                ds = new DALOS_LabOrder().insertUpdateLabOrderProblems(ds);
                return new BLObject<DSOS_LabOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::InsertUpdateLabOrderProblems", ex);
                return new BLObject<DSOS_LabOrder>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 31/03/2016
        //OverView: Method to Load LabOrder Problems
        public BLObject<DSOS_LabOrder> LoadLabOrderProblems(long labOrderId, long orderSetId, int pageNumber, int rowsPerPage)
        {
            try
            {
                DSOS_LabOrder ds = new DSOS_LabOrder();
                ds = new DALOS_LabOrder().loadLabOrderProblems(0, labOrderId, orderSetId, pageNumber, rowsPerPage);
                return new BLObject<DSOS_LabOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadLabOrderProblems", ex);
                return new BLObject<DSOS_LabOrder>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 31/03/2016
        //OverView: Method to Delete LabOrder Problems
        public BLObject<string> DeleteLabOrderProblems(string labOrderId)
        {
            try
            {
                labOrderId = new DALOS_LabOrder().deleteLabOrderProblems(Convert.ToInt64(labOrderId));
                return new BLObject<string>(labOrderId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::DeleteLabOrderProblems", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 31/03/2016
        // Description: Attaching Lab Order with Notes
        public BLObject<DSOS_LabOrder> attachLabOrderWithNotes(string labOrderId, long noteId)
        {
            try
            {
                DSOS_LabOrder ds = new DSOS_LabOrder();
                ds = new DALOS_LabOrder().attachLabOrderWithNotes(labOrderId, noteId);
                return new BLObject<DSOS_LabOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::attachLabOrderWithNotes", ex);
                return new BLObject<DSOS_LabOrder>(null, ex.Message);
            }
        }

        public BLObject<DSLabResult> attachLabResultWithNotes(string labOrderResultId, long noteId)
        {
            try
            {
                DSLabResult ds = new DSLabResult();
                ds = new DALLabResult().attachLabResultWithNotes(labOrderResultId, noteId);
                return new BLObject<DSLabResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::attachLabResultWithNotes", ex);
                return new BLObject<DSLabResult>(null, ex.Message);
            }
        }

        public BLObject<DSRadiologyResult> attachRadiologyResultWithNotes(string labOrderResultId, long noteId)
        {
            try
            {
                DSRadiologyResult ds = new DSRadiologyResult();
                ds = new DALRadiologyResult().attachRadiologyResultWithNotes(labOrderResultId, noteId);
                return new BLObject<DSRadiologyResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::attachRadiologyResultWithNotes", ex);
                return new BLObject<DSRadiologyResult>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 31/03/2016
        // Description: Detaching Lab Order from Notes
        public BLObject<string> detachLabOrderFromNotes(string labOrderId, long noteId)
        {
            try
            {
                var msgLabOrderId = new DALOS_LabOrder().detachLabOrderFromNotes(labOrderId, noteId);
                return new BLObject<string>(msgLabOrderId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::detachLabOrderFromNotes", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> detachLabOrderResultFromNotes(string labOrderId, long noteId)
        {
            try
            {
                var msgLabOrderResultId = new DALLabResult().detachLabResultFromNotes(labOrderId, noteId);
                return new BLObject<string>(msgLabOrderResultId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::detachLabOrderResultFromNotes", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> detachRadiologyOrderResultFromNotes(string radiologyOrderId, long noteId)
        {
            try
            {
                var msgRadiologyOrderResultId = new DALRadiologyResult().detachRadiologyOrderResultFromNotes(radiologyOrderId, noteId);
                return new BLObject<string>(msgRadiologyOrderResultId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::detachRadiologyOrderResultFromNotes", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 31/03/2016
        // Description: Creates PDF to view Lab Order
        /// <summary>
        ///
        /// </summary>
        /// <param name="labId"></param>
        /// <param name="patientId"></param>
        /// <param name="BarCodeHtml"></param>
        /// <param name="ImagePath"></param>
        /// <returns></returns>
        ///

        enum dietDescriptions { Normal, Fasting };

        #region LabOrder, OrderInformation
        private void labOrderInformation(Document pdfDocument, DSOS_LabOrder dsLabOrder, Font componentHeaderFont, Font componentHeadingFont, Font bodyFont)
        {
            if (dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows.Count > 0)
            {
                Paragraph order_Heading = new Paragraph("Order Information \n".ToString(), componentHeadingFont);
                pdfDocument.Add(order_Heading);

                PdfPTable orderTable = new PdfPTable(4);
                float[] widths = new float[] { 3f, 8f, 4f, 3f };
                orderTable.SetWidths(widths);
                orderTable.TotalWidth = 575f;
                orderTable.LockedWidth = true;
                orderTable.HorizontalAlignment = Element.ALIGN_CENTER;
                orderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                foreach (DSOS_LabOrder.OS_LabOrderRow dr in dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows)
                {
                    orderTable.AddCell(new Paragraph("Laboratory:", componentHeaderFont));
                    orderTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.LabNameColumn.ColumnName])), bodyFont));
                    orderTable.AddCell(new Paragraph("Collection Date & Time:", componentHeaderFont));
                    orderTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dr[dsLabOrder.OS_LabOrder.OrderDateColumn.ColumnName]).ToShortDateString() + ' ' + dr[dsLabOrder.OS_LabOrder.OrderTimeColumn.ColumnName].ToString(), bodyFont));
                    orderTable.AddCell(new Paragraph("Facility:", componentHeaderFont));
                    orderTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.FacilityColumn.ColumnName])), bodyFont));
                    orderTable.AddCell(new Paragraph("Order Number:", componentHeaderFont));
                    orderTable.AddCell(new Paragraph(dr[dsLabOrder.OS_LabOrder.OrderNoColumn.ColumnName].ToString(), bodyFont));
                }
                pdfDocument.Add(orderTable);
            }
            else
            {
                Paragraph order_Heading = new Paragraph("Order Information \n".ToString(), componentHeadingFont);
                order_Heading.SpacingBefore = order_Heading.SpacingAfter = 5;
                pdfDocument.Add(order_Heading);

                PdfPTable orderTable = new PdfPTable(5);
                float[] widths = new float[] { 1f, 8f, 8f, 8f, 8f };
                orderTable.SetWidths(widths);
                orderTable.TotalWidth = 575f;
                orderTable.LockedWidth = true;
                orderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                orderTable.AddCell(string.Empty);
                pdfDocument.Add(orderTable);
                Paragraph noOrder = new Paragraph("No Order Information Found".ToString());
                noOrder.Alignment = Element.ALIGN_CENTER;
                pdfDocument.Add(noOrder);
            }
        }
        private void labOrderGuarantor(Document pdfDocument, Font componentHeadingFont, Font componentHeaderFont, Font bodyFont, DSOS_LabOrder dsLabOrder)
        {
            foreach (DSOS_LabOrder.OS_LabOrderRow dr in dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows)
            {
                Paragraph Relation_Heading = new Paragraph("Guarantor\n".ToString(), componentHeadingFont);
                pdfDocument.Add(Relation_Heading);
                if (!string.IsNullOrEmpty(dr[dsLabOrder.OS_LabOrder.GuarantorFirstNameColumn.ColumnName].ToString()))
                {
                    PdfPTable GuarantorTable = new PdfPTable(4);
                    Paragraph value = null;

                    float[] GuarantorWidths = new float[] { 3f, 8f, 4f, 3f };
                    GuarantorTable.SetWidths(GuarantorWidths);
                    GuarantorTable.TotalWidth = 575f;
                    GuarantorTable.LockedWidth = true;
                    GuarantorTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    GuarantorTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    Paragraph rel = new Paragraph("Guarantor Name: ", componentHeaderFont);
                    var guarantorLastName = string.IsNullOrEmpty(dr[dsLabOrder.OS_LabOrder.GuarantorLastNameColumn].ToString()) ? "N/A" : dr[dsLabOrder.OS_LabOrder.GuarantorLastNameColumn.ColumnName].ToString();
                    var guarantorFirstName = string.IsNullOrEmpty(dr[dsLabOrder.OS_LabOrder.GuarantorFirstNameColumn.ColumnName].ToString()) ? "N/A" : dr[dsLabOrder.OS_LabOrder.GuarantorFirstNameColumn.ColumnName].ToString();
                    if (guarantorFirstName == "N/A" && guarantorLastName == "N/A")
                    {
                        value = new Paragraph(("N/A"), bodyFont);
                    }
                    else
                    {
                        value = new Paragraph((guarantorLastName + ", " + guarantorFirstName), bodyFont);
                    }
                    Paragraph relName = new Paragraph("Guarantor Relation: ", componentHeaderFont);
                    Paragraph relValue = new Paragraph(string.IsNullOrEmpty(dr[dsLabOrder.OS_LabOrder.RelationShipColumn.ColumnName].ToString()) ? "N/A" : dr[dsLabOrder.OS_LabOrder.RelationShipColumn.ColumnName].ToString(), bodyFont);
                    Paragraph paraAddress = new Paragraph("Guarantor Address: ", componentHeaderFont);
                    Paragraph relAddValue = new Paragraph(string.IsNullOrEmpty(dr[dsLabOrder.OS_LabOrder.GuarantorAddressColumn.ColumnName].ToString()) ? "N/A" : dr[dsLabOrder.OS_LabOrder.GuarantorAddressColumn.ColumnName].ToString() + ", " + dr[dsLabOrder.OS_LabOrder.GuarantorCityColumn.ColumnName] + ", " + dr[dsLabOrder.OS_LabOrder.GuarantorStateColumn.ColumnName] + ", " + dr[dsLabOrder.OS_LabOrder.GuarantorZipCodeColumn.ColumnName], bodyFont);
                    GuarantorTable.AddCell(rel);
                    GuarantorTable.AddCell(value);
                    GuarantorTable.AddCell(relName);
                    GuarantorTable.AddCell(relValue);
                    GuarantorTable.AddCell(paraAddress);
                    GuarantorTable.AddCell(relAddValue);
                    GuarantorTable.AddCell(new Paragraph(string.Empty));
                    GuarantorTable.AddCell(new Paragraph(string.Empty));
                    pdfDocument.Add(GuarantorTable);

                }
                else
                {
                    Paragraph noProblems = new Paragraph("No Guarantor Found".ToString(), bodyFont);
                    noProblems.Alignment = Element.ALIGN_CENTER;
                    pdfDocument.Add(noProblems);
                }
            }
        }
        #endregion

        private PdfPTable setLabHeaderPDF(long practiceId, long patientId, DSLab dsLab, DSOS_LabOrder dsLabOrder, Font bodyFont, string BarCodeHtmlText, bool isLabOrder = true)
        {
            PdfPTable ReportHeaderTable = new PdfPTable(2);
            #region Practice

            string practiceAddress = GetPracticeText(practiceId);
            string[] practiceAddresses = practiceAddress.Split(new string[] { "<br/>" }, StringSplitOptions.None);

            practiceAddress = "";
            foreach (var column in practiceAddresses)
                if (column != string.Empty)
                    practiceAddress += column + "\n";

            practiceAddress = practiceAddress.Remove(practiceAddress.Length - 1);
            if (dsLab.Lab.Rows.Count > 0)
            {
                practiceAddress += "\n Account Number: " + dsLab.Lab.Rows[0][dsLab.Lab.ClientNoColumn.ColumnName].ToString();
            }

            Paragraph p_practiceAddress = new Paragraph(practiceAddress, bodyFont);

            PdfPCell headerSovreignAddress = new PdfPCell();
            headerSovreignAddress.AddElement(p_practiceAddress);
            headerSovreignAddress.Border = Rectangle.NO_BORDER;
            headerSovreignAddress.HorizontalAlignment = Element.ALIGN_LEFT;

            #endregion

            #region Header

            if (true || BarCodeHtmlText != string.Empty)
            {
                BCommon.Barcode39 b = new BCommon.Barcode39();
                System.Drawing.Image img;
                b.ShowString = true;

                b.IncludeCheckSumDigit = false;
                b.TextFont = new System.Drawing.Font("Courier New", 9);

                #region BarCode/String/Logo

                string barCodeCondition1 = ""; // PatientAccount
                string barCodeCondition2 = ""; // Order Number
                string barCodeCondition3 = ""; // PatientLastName
                string barCodeCondition4 = ""; // PatientFirstNameInitial

                StringBuilder strBarcode = new StringBuilder();
                if (dsLabOrder != null && dsLabOrder.OS_LabOrder.Rows.Count > 0)
                {
                    DSPatient dsPatient_BarCode = new DALPatient().FillPatient(patientId, "", "");
                    if (dsPatient_BarCode.Patients.Rows.Count > 0)
                    {
                        if (dsLab.Lab.Rows.Count > 0)
                        {
                            barCodeCondition1 = MDVUtility.ToStr(dsLab.Lab.Rows[0][dsLab.Lab.ClientNoColumn.ColumnName]);
                        }
                        barCodeCondition2 = MDVUtility.ToStr(dsLabOrder.OS_LabOrder.Rows[0][dsLabOrder.OS_LabOrder.OrderNoColumn.ColumnName]);
                        barCodeCondition3 = MDVUtility.ToStr(dsPatient_BarCode.Patients.Rows[0][dsPatient_BarCode.Patients.LastNameColumn.ColumnName]);
                        barCodeCondition4 = MDVUtility.ToStr(dsPatient_BarCode.Patients.Rows[0][dsPatient_BarCode.Patients.FirstNameColumn.ColumnName]);
                    }

                    barCodeCondition3 = barCodeCondition3[0].ToString();
                    barCodeCondition4 = barCodeCondition4[0].ToString();
                    strBarcode.Append(barCodeCondition1);
                    strBarcode.Append(",");
                    strBarcode.Append(barCodeCondition2);
                    strBarcode.Append(",");
                    strBarcode.Append(barCodeCondition3);
                    strBarcode.Append(" ");
                    strBarcode.Append(barCodeCondition4);
                }

                img = b.GenerateBarcodeImage(240, 64, strBarcode.ToString());
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(img, System.Drawing.Imaging.ImageFormat.Png);
                pic.SpacingAfter = 10;

                // -------------------------------------------------------------------------------------------------------- //

                PdfPTable ReportHeaderBarCode = new PdfPTable(1);
                ReportHeaderBarCode.TotalWidth = 190f;
                ReportHeaderBarCode.LockedWidth = true;
                ReportHeaderBarCode.HorizontalAlignment = Element.ALIGN_RIGHT;
                ReportHeaderBarCode.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell lineCell = new PdfPCell();

                if (BarCodeHtmlText != string.Empty && BarCodeHtmlText.ToLower() != "false" && isLabOrder)
                    lineCell.AddElement(pic);
                else
                    lineCell.AddElement(new Chunk());

                lineCell.Border = Rectangle.NO_BORDER;
                lineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                ReportHeaderBarCode.AddCell(lineCell);


                ReportHeaderTable.TotalWidth = 575f;
                ReportHeaderTable.SpacingBefore = 10f;
                ReportHeaderTable.LockedWidth = true;
                ReportHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                MemoryStream ms = new MemoryStream();
                byte[] bytes;
                using (FileStream file = new FileStream(System.Web.HttpContext.Current.Server.MapPath(@"~\content\images\SHS-nav-logo.png"), FileMode.Open, System.IO.FileAccess.Read))
                {
                    bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);
                }

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(bytes, false);
                logo.ScalePercent(59f);
                logo.ScaleAbsoluteHeight(100);
                logo.ScaleAbsoluteWidth(150);

                PdfPCell logoCell = new PdfPCell();
                logoCell.AddElement(logo);
                logoCell.Border = Rectangle.NO_BORDER;
                logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                ReportHeaderTable.AddCell(logoCell);

                #endregion

                #region Header Table

                ReportHeaderTable.AddCell(ReportHeaderBarCode);
                ReportHeaderTable.AddCell(headerSovreignAddress);

                string clientDetails = "";
                if (dsLab.Lab.Rows.Count > 0)
                {
                    clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.NameColumn.ColumnName] + "\n");
                    clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.AddressColumn.ColumnName], "\n");
                    clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.CityColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.CityColumn.ColumnName] + ", ");
                    clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.StateColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.StateColumn.ColumnName] + ", ");
                    clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName] + "\n");
                    clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.PhoneNoColumn.ColumnName]);
                }

                Paragraph p = new Paragraph(clientDetails, bodyFont);
                PdfPCell clientAccountNumber = new PdfPCell();
                p.Alignment = Element.ALIGN_RIGHT;
                clientAccountNumber.AddElement(p);
                clientAccountNumber.Border = Rectangle.NO_BORDER;
                clientAccountNumber.HorizontalAlignment = Element.ALIGN_RIGHT;

                ReportHeaderTable.AddCell(clientAccountNumber);
                ReportHeaderTable.SpacingAfter = 3f;
                #endregion
            }
            #endregion
            return ReportHeaderTable;
        }


        private PdfPTable setProcedureHeaderPDF(long practiceId, Font bodyFont)
        {
            PdfPTable ReportHeaderTable = new PdfPTable(2);
            #region Practice

            string practiceAddress = GetPracticeText(practiceId);
            string[] practiceAddresses = practiceAddress.Split(new string[] { "<br/>" }, StringSplitOptions.None);

            practiceAddress = "";
            foreach (var column in practiceAddresses)
                if (column != string.Empty)
                    practiceAddress += column + "\n";

            practiceAddress = practiceAddress.Remove(practiceAddress.Length - 1);
            //practiceAddress += "\n Account Number: " + dsLab.Lab.Rows[0][dsLab.Lab.ClientNoColumn.ColumnName].ToString();

            Paragraph p_practiceAddress = new Paragraph(practiceAddress, bodyFont);

            PdfPCell headerSovreignAddress = new PdfPCell();
            headerSovreignAddress.AddElement(p_practiceAddress);
            headerSovreignAddress.Border = Rectangle.NO_BORDER;
            headerSovreignAddress.HorizontalAlignment = Element.ALIGN_LEFT;
            //ReportHeaderTable.AddCell(headerSovreignAddress);
            #endregion


            //#region BarCode/String/Logo
            //BCommon.Barcode39 b = new BCommon.Barcode39();
            //System.Drawing.Image img;
            //b.ShowString = true;

            //b.IncludeCheckSumDigit = false;
            //b.TextFont = new System.Drawing.Font("Courier New", 9);

            //StringBuilder strBarcode = new StringBuilder();


            //img = b.GenerateBarcodeImage(240, 64, strBarcode.ToString());
            //iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(img, System.Drawing.Imaging.ImageFormat.Png);
            //pic.SpacingAfter = 10;

            //// -------------------------------------------------------------------------------------------------------- //

            //PdfPTable ReportHeaderBarCode = new PdfPTable(1);
            //ReportHeaderBarCode.TotalWidth = 190f;
            //ReportHeaderBarCode.LockedWidth = true;
            //ReportHeaderBarCode.HorizontalAlignment = Element.ALIGN_RIGHT;
            //ReportHeaderBarCode.DefaultCell.Border = Rectangle.NO_BORDER;

            //PdfPCell lineCell = new PdfPCell();

            //lineCell.AddElement(new Chunk());

            //lineCell.Border = Rectangle.NO_BORDER;
            //lineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //ReportHeaderBarCode.AddCell(lineCell);


            //ReportHeaderTable.TotalWidth = 575f;
            //ReportHeaderTable.SpacingBefore = 10f;
            //ReportHeaderTable.LockedWidth = true;
            //ReportHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;

            //MemoryStream ms = new MemoryStream();
            //byte[] bytes;
            //using (FileStream file = new FileStream(System.Web.HttpContext.Current.Server.MapPath(@"~\content\images\SHS-nav-logo.png"), FileMode.Open, System.IO.FileAccess.Read))
            //{
            //    bytes = new byte[file.Length];
            //    file.Read(bytes, 0, (int)file.Length);
            //    ms.Write(bytes, 0, (int)file.Length);
            //}

            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(bytes, false);
            //logo.ScalePercent(59f);
            //logo.ScaleAbsoluteHeight(100);
            //logo.ScaleAbsoluteWidth(150);

            //PdfPCell logoCell = new PdfPCell();
            //logoCell.AddElement(logo);
            //logoCell.Border = Rectangle.NO_BORDER;
            //logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //ReportHeaderTable.AddCell(logoCell);
            //ReportHeaderTable.AddCell(headerSovreignAddress);
            //ReportHeaderTable.SpacingAfter = 3f;
            //ReportHeaderTable.AddCell(ReportHeaderBarCode);

            //ReportHeaderTable.SpacingAfter = 3f;
            //#endregion



            return ReportHeaderTable;
        }

        private PdfPTable setRadiologyHeaderPDF(long practiceId, Font bodyFont)
        {
            PdfPTable ReportHeaderTable = new PdfPTable(2);
            #region Practice

            string practiceAddress = GetPracticeText(practiceId);
            string[] practiceAddresses = practiceAddress.Split(new string[] { "<br/>" }, StringSplitOptions.None);

            practiceAddress = "";
            foreach (var column in practiceAddresses)
                if (column != string.Empty)
                    practiceAddress += column + "\n";

            practiceAddress = practiceAddress.Remove(practiceAddress.Length - 1);
            //practiceAddress += "\n Account Number: " + dsLab.Lab.Rows[0][dsLab.Lab.ClientNoColumn.ColumnName].ToString();

            Paragraph p_practiceAddress = new Paragraph(practiceAddress, bodyFont);

            PdfPCell headerSovreignAddress = new PdfPCell();
            headerSovreignAddress.AddElement(p_practiceAddress);
            headerSovreignAddress.Border = Rectangle.NO_BORDER;
            headerSovreignAddress.HorizontalAlignment = Element.ALIGN_LEFT;
            ReportHeaderTable.AddCell(headerSovreignAddress);
            #endregion


            return ReportHeaderTable;
        }

        private PdfPTable setConsultationHeaderPDF(long practiceId, Font bodyFont)
        {
            PdfPTable ReportHeaderTable = new PdfPTable(2);
            #region Practice

            string practiceAddress = GetPracticeText(practiceId);
            string[] practiceAddresses = practiceAddress.Split(new string[] { "<br/>" }, StringSplitOptions.None);

            practiceAddress = "";
            foreach (var column in practiceAddresses)
                if (column != string.Empty)
                    practiceAddress += column + "\n";

            practiceAddress = practiceAddress.Remove(practiceAddress.Length - 1);
            //practiceAddress += "\n Account Number: " + dsLab.Lab.Rows[0][dsLab.Lab.ClientNoColumn.ColumnName].ToString();

            Paragraph p_practiceAddress = new Paragraph(practiceAddress, bodyFont);

            PdfPCell headerSovreignAddress = new PdfPCell();
            headerSovreignAddress.AddElement(p_practiceAddress);
            headerSovreignAddress.Border = Rectangle.NO_BORDER;
            headerSovreignAddress.HorizontalAlignment = Element.ALIGN_LEFT;
            ReportHeaderTable.AddCell(headerSovreignAddress);
            #endregion


            return ReportHeaderTable;
        }



        private PdfPTable setLabOrder_ResultFooterPDF(string generatedBy = "")
        {
            PdfPTable footer = new PdfPTable(1);
            footer.TotalWidth = 575f;
            footer.LockedWidth = true;
            footer.HorizontalAlignment = Element.ALIGN_CENTER;
            footer.DefaultCell.Border = Rectangle.NO_BORDER;
            footer.SpacingBefore = 5f;

            PdfPCell footerCell = new PdfPCell();
            var color = System.Drawing.ColorTranslator.FromHtml("#005da9");
            footerCell.BackgroundColor = new BaseColor(color);
            color = System.Drawing.ColorTranslator.FromHtml("#fff");
            var fontcolor = new BaseColor(color);
            Font componentFooterFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, fontcolor);

            Paragraph footerPara = new Paragraph(generatedBy == "" ? "Generated by: MDVision PMS EMR" : "Generated by: " + generatedBy, componentFooterFont);
            footerPara.SpacingAfter = 5f;
            footerCell.AddElement(footerPara);
            footer.AddCell(footerCell);
            // pdfDocument.Add(footer);
            return footer;
        }
        public BLObject<byte[]> previewLabOrder(long labId, long patientId, string BarCodeHtml, string ImagePath)
        {
            try
            {
                #region It starts with One thing I don't know why

                string patientName = "";
                byte[] newByteArr = null;
                var filePath = string.Empty;
                var folderPath = string.Empty;
                var pngfileName = string.Empty;
                bool IsInsuranceAvailable = false;

                DSOS_LabOrder dsLabOrder = new DSOS_LabOrder();
                DSInsurance dsInsurance = new DSInsurance();
                DSPatient dsPatient = new DSPatient();
                DSProfile dsProfile = new DSProfile();

                DSOS_LabOrder dsLabOrderExternalBillingInfoPrimary = null;
                DataRow drLabOrderExternalBillingInfoPrimary = null;

                #region Fonts

                var fontColour = new BaseColor(102, 178, 255);
                Font patientNameFont = FontFactory.GetFont("Arial", 12, Font.BOLD, fontColour);
                Font bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                Font bodyFontSmall = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                Font testDetailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                Font componentHeadingFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, fontColour);
                Font componentHeaderFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                Font gridbodyFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);

                #endregion

                #endregion

                #region Fetch Routine

                dsLabOrder = new DALOS_LabOrder().FillLabOrder(labId);
                DSLab dsLab = new DALLab().GetLab(MDVUtility.ToLong(dsLabOrder.OS_LabOrder.Rows[0][dsLabOrder.OS_LabOrder.LabIdColumn.ColumnName]), MDVUtility.ToLong(dsLabOrder.OS_LabOrder.Rows[0][dsLabOrder.OS_LabOrder.ProviderIdColumn.ColumnName]), MDVUtility.ToLong(dsLabOrder.OS_LabOrder.Rows[0][dsLabOrder.OS_LabOrder.FacilityIdColumn.ColumnName]));

                if (patientId == 0)
                    if (dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows.Count > 0)
                        patientId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows[0][dsLabOrder.OS_LabOrder.OrderSetIdColumn.ColumnName]);

                dsLabOrder.Merge(new DALPatient().FillPatient(patientId, "", ""));
                dsLabOrder.Merge(new DALOS_LabOrder().loadLabOrderProblems(0, labId, patientId));

                long practiceId = MDVUtility.ToInt64(dsLabOrder.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PracticeIdColumn.ColumnName]);
                long providerId = MDVUtility.ToInt64(dsLabOrder.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.ProviderIdColumn.ColumnName]);

                //if (dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows.Count > 0)
                //    providerId = MDVUtility.ToInt64(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows[0][dsLabOrder.OS_LabOrder.ProviderIdColumn.ColumnName]);

                dsLabOrder.Merge(new DALPractice().LoadPractice(practiceId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));

                #endregion

                using (MemoryStream stream_ = new MemoryStream())
                {
                    #region Header Segment

                    PdfPTable ReportHeaderTable = setLabHeaderPDF(practiceId, patientId, dsLab, dsLabOrder, bodyFont, BarCodeHtml, true);
                    float bottomMargin = 22;
                    float topMargin = 20;
                    //if (IsReportHeaderApplied)
                    //{
                    topMargin = ReportHeaderTable.CalculateHeights() + 33;
                    bottomMargin = 52;
                    //}
                    #endregion
                    #region MD Vision Footer

                    PdfPTable footer = setLabOrder_ResultFooterPDF();

                    #endregion
                    #region Document Object
                    Document pdfDocument = new Document(PageSize.LETTER, 20, 20, topMargin, bottomMargin);
                    MDVUtility.PDFCreator pdf = new MDVUtility.PDFCreator(ref pdfDocument, stream_, false, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Face Sheet");
                    pdf.Writer.PageEvent = new MDVision.Common.Utilities.MDVUtility.AddFooterHeader(ReportHeaderTable, true, null, footer);

                    #endregion

                    #region Commented
                    //                    #region Practice

                    //                    string practiceAddress = GetPracticeText(practiceId);
                    //                    string[] practiceAddresses = practiceAddress.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                    //                    practiceAddress = "";
                    //                    foreach (var column in practiceAddresses)
                    //                        if (column != string.Empty)
                    //                            practiceAddress += column + "\n";

                    //                    practiceAddress = practiceAddress.Remove(practiceAddress.Length - 1);
                    //                    practiceAddress += "\n Account Number: " + dsLab.Lab.Rows[0][dsLab.Lab.ClientNoColumn.ColumnName].ToString();

                    //                    Paragraph p_practiceAddress = new Paragraph(practiceAddress, bodyFont);

                    //                    PdfPCell headerSovreignAddress = new PdfPCell();
                    //                    headerSovreignAddress.AddElement(p_practiceAddress);
                    //                    headerSovreignAddress.Border = Rectangle.NO_BORDER;
                    //                    headerSovreignAddress.HorizontalAlignment = Element.ALIGN_LEFT;

                    //                    #endregion

                    //                    #region Header
                    //if (true || BarCodeHtml != string.Empty)
                    //                    {
                    //                        BCommon.Barcode39 b = new BCommon.Barcode39();
                    //                        System.Drawing.Image img;

                    //                        b.ShowString = true;

                    //                        b.IncludeCheckSumDigit = false;
                    //                        b.TextFont = new System.Drawing.Font("Courier New", 9);

                    //                        string barCodeCondition1 = ""; // PatientAccount
                    //                        string barCodeCondition2 = ""; // Order Number
                    //                        string barCodeCondition3 = ""; // PatientLastName
                    //                        string barCodeCondition4 = ""; // PatientFirstNameInitial

                    //                        StringBuilder strBarcode = new StringBuilder();


                    //                        if (dsLabOrder != null && dsLabOrder.OS_LabOrder.Rows.Count > 0)
                    //                        {
                    //                            DSPatient dsPatient1 = new DSPatient();
                    //                            dsPatient1 = new DALPatient().FillPatient(patientId, "", "");

                    //                            if (dsPatient1.Patients.Rows.Count > 0)
                    //                            {
                    //                                barCodeCondition1 = MDVUtility.ToStr(dsLab.Lab.Rows[0][dsLab.Lab.ClientNoColumn.ColumnName]);
                    //                                barCodeCondition2 = MDVUtility.ToStr(dsLabOrder.OS_LabOrder.Rows[0][dsLabOrder.OS_LabOrder.OrderNoColumn.ColumnName]);
                    //                                barCodeCondition3 = MDVUtility.ToStr(dsPatient1.Patients.Rows[0][dsPatient1.Patients.LastNameColumn.ColumnName]);
                    //                                barCodeCondition4 = MDVUtility.ToStr(dsPatient1.Patients.Rows[0][dsPatient1.Patients.FirstNameColumn.ColumnName]);
                    //                            }
                    //                            barCodeCondition3 = barCodeCondition3[0].ToString();
                    //                            barCodeCondition4 = barCodeCondition4[0].ToString();
                    //                            strBarcode.Append(barCodeCondition1);
                    //                            strBarcode.Append(",");
                    //                            strBarcode.Append(barCodeCondition2);
                    //                            strBarcode.Append(",");
                    //                            strBarcode.Append(barCodeCondition3);
                    //                            strBarcode.Append(" ");
                    //                            strBarcode.Append(barCodeCondition4);

                    //                        }
                    //                        img = b.GenerateBarcodeImage(240, 64, strBarcode.ToString());

                    //                        iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(img, System.Drawing.Imaging.ImageFormat.Png);
                    //                        pic.SpacingAfter = 10;

                    //                        PdfPTable ReportHeaderBarCode = new PdfPTable(1);
                    //                        ReportHeaderBarCode.TotalWidth = 190f;
                    //                        ReportHeaderBarCode.LockedWidth = true;
                    //                        ReportHeaderBarCode.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //                        ReportHeaderBarCode.DefaultCell.Border = Rectangle.NO_BORDER;

                    //                        PdfPCell lineCell = new PdfPCell();
                    //                        if (BarCodeHtml != string.Empty)
                    //                            lineCell.AddElement(pic);
                    //                        else
                    //                            lineCell.AddElement(new Chunk());

                    //                        lineCell.Border = Rectangle.NO_BORDER;
                    //                        lineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //                        ReportHeaderBarCode.AddCell(lineCell);



                    //                        //////////pdfDocument.Add(ReportHeaderLineTable);
                    //                        PdfPTable ReportHeaderTable = new PdfPTable(2);
                    //                        ReportHeaderTable.TotalWidth = 575f;
                    //                        ReportHeaderTable.SpacingBefore = 10f;
                    //                        ReportHeaderTable.LockedWidth = true;
                    //                        ReportHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    //                        ReportHeaderTable.SpacingAfter = 3f;

                    //                        #region Header Logo

                    //                        //if (!(drReportHeader.Field<string>("HeaderLogo") != null))
                    //                        //{
                    //                        //    //PdfPTable headerTable = new PdfPTable(1);
                    //                        //    //headerTable.TotalWidth = 575f;
                    //                        //    //headerTable.LockedWidth = true;
                    //                        //    //headerTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    //                        //    Byte[] buffer = Convert.FromBase64String(drReportHeader.HeaderLogo.Split(new string[] { "base64," }, StringSplitOptions.None)[1]);
                    //                        //    var memoryStream = new MemoryStream(buffer); //new MemoryStream(buffer, offset, count);
                    //                        //    System.Drawing.Image newImagej = System.Drawing.Image.FromStream(memoryStream);

                    //                        //    //   System.Drawing.Imaging.ImageFormat format = new System.Drawing.Imaging.ImageFormat();



                    //                        //    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(buffer, false);
                    //                        //    logo.ScalePercent(59f);
                    //                        //    logo.ScaleAbsoluteHeight(100);
                    //                        //    logo.ScaleAbsoluteWidth(150);

                    //                        //    PdfPCell headerLogoCell = new PdfPCell();
                    //                        //    headerLogoCell.AddElement(logo);
                    //                        //    headerLogoCell.Border = Rectangle.NO_BORDER;
                    //                        //    headerLogoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                        //    ReportHeaderTable.AddCell(headerLogoCell);

                    //                        //    #region practice

                    //                        //    //PdfPTable PracticeTable = new PdfPTable(1);

                    //                        //    //PracticeTable.DefaultCell.Border = Rectangle.NO_BORDER;



                    //                        //    //if (drReportHeader.Field<string>("PracticeText") != null)
                    //                        //    //{
                    //                        //    //    string[] PracticeColumns = drReportHeader.PracticeText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                    //                        //    //    List<Chunk> chunks = new List<Chunk>();
                    //                        //    //    float widest = 0f;
                    //                        //    //    foreach (string PracticeColumn in PracticeColumns)
                    //                        //    //    {
                    //                        //    //        Chunk chun = new Chunk(PracticeColumn);
                    //                        //    //        float w = chun.GetWidthPoint();
                    //                        //    //        chun.Font = bodyFont;
                    //                        //    //        if (w > widest) widest = w;
                    //                        //    //        chunks.Add(chun);

                    //                        //    //    }

                    //                        //    //    float indentation = pdfDocument.PageSize.Width
                    //                        //    //      - pdfDocument.RightMargin
                    //                        //    //      - pdfDocument.LeftMargin
                    //                        //    //      - widest
                    //                        //    //    ;
                    //                        //    //    foreach (Chunk chun in chunks)
                    //                        //    //    {
                    //                        //    //        PdfPCell Practicecell = new PdfPCell();
                    //                        //    //        Paragraph p = new Paragraph(chun);
                    //                        //    //        p.IndentationLeft = indentation;
                    //                        //    //        Practicecell.AddElement(p);
                    //                        //    //        Practicecell.Border = Rectangle.NO_BORDER;
                    //                        //    //        Practicecell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //                        //    //        PracticeTable.AddCell(Practicecell);
                    //                        //    //    }


                    //                        //    //}
                    //                        //    //else
                    //                        //    //{
                    //                        //    //    string PracticeText = GetPracticeText(practiceId);
                    //                        //    //    string[] PracticeColumns = PracticeText.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                    //                        //    //    foreach (string PracticeColumn in PracticeColumns)
                    //                        //    //    {
                    //                        //    //        Paragraph p = new Paragraph(PracticeColumn, bodyFont);
                    //                        //    //        p.Alignment = Element.ALIGN_LEFT;
                    //                        //    //        PdfPCell Practicecell = new PdfPCell();
                    //                        //    //        Practicecell.AddElement(p);
                    //                        //    //        Practicecell.Border = Rectangle.NO_BORDER;
                    //                        //    //        Practicecell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //                        //    //        PracticeTable.AddCell(Practicecell);
                    //                        //    //    }


                    //                        //    //}


                    //                        //    //ReportHeaderTable.AddCell(PracticeTable);

                    //                        //    #endregion

                    //                        //}

                    //                        //else
                    //                        //{

                    //                        //PdfPTable headerTable = new PdfPTable(1);
                    //                        //headerTable.TotalWidth = 575f;
                    //                        //headerTable.LockedWidth = true;
                    //                        //headerTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    //                        MemoryStream ms = new MemoryStream();
                    //                        byte[] bytes;
                    //                        using (FileStream file = new FileStream(System.Web.HttpContext.Current.Server.MapPath(@"~\content\images\SHS-nav-logo.png"), FileMode.Open, System.IO.FileAccess.Read))
                    //                        {
                    //                            bytes = new byte[file.Length];
                    //                            file.Read(bytes, 0, (int)file.Length);
                    //                            ms.Write(bytes, 0, (int)file.Length);
                    //                        }
                    //                        //   System.Drawing.Imaging.ImageFormat format = new System.Drawing.Imaging.ImageFormat();



                    //                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(bytes, false);
                    //                        logo.ScalePercent(59f);
                    //                        logo.ScaleAbsoluteHeight(100);
                    //                        logo.ScaleAbsoluteWidth(150);

                    //                        PdfPCell cell1 = new PdfPCell();
                    //                        cell1.AddElement(logo);
                    //                        cell1.Border = Rectangle.NO_BORDER;
                    //                        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                        ReportHeaderTable.AddCell(cell1);

                    //                        #region practice

                    //                        //PdfPTable PracticeTable = new PdfPTable(1);

                    //                        //PracticeTable.DefaultCell.Border = Rectangle.NO_BORDER;



                    //                        //if (drReportHeader.Field<string>("PracticeText") != null)
                    //                        //{
                    //                        //    string[] PracticeColumns = drReportHeader.PracticeText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                    //                        //    List<Chunk> chunks = new List<Chunk>();
                    //                        //    float widest = 0f;
                    //                        //    foreach (string PracticeColumn in PracticeColumns)
                    //                        //    {
                    //                        //        Chunk chun = new Chunk(PracticeColumn);
                    //                        //        float w = chun.GetWidthPoint();
                    //                        //        chun.Font = bodyFont;
                    //                        //        if (w > widest) widest = w;
                    //                        //        chunks.Add(chun);

                    //                        //    }

                    //                        //    float indentation = pdfDocument.PageSize.Width
                    //                        //      - pdfDocument.RightMargin
                    //                        //      - pdfDocument.LeftMargin
                    //                        //      - widest
                    //                        //    ;
                    //                        //    foreach (Chunk chun in chunks)
                    //                        //    {
                    //                        //        PdfPCell Practicecell = new PdfPCell();
                    //                        //        Paragraph p = new Paragraph(chun);
                    //                        //        p.IndentationLeft = indentation;
                    //                        //        Practicecell.AddElement(p);
                    //                        //        Practicecell.Border = Rectangle.NO_BORDER;
                    //                        //        Practicecell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //                        //        PracticeTable.AddCell(Practicecell);
                    //                        //    }


                    //                        //}
                    //                        //else
                    //                        //{
                    //                        //    string PracticeText = GetPracticeText(practiceId);
                    //                        //    string[] PracticeColumns = PracticeText.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                    //                        //    foreach (string PracticeColumn in PracticeColumns)
                    //                        //    {
                    //                        //        Paragraph p = new Paragraph(PracticeColumn, bodyFont);
                    //                        //        p.Alignment = Element.ALIGN_LEFT;
                    //                        //        PdfPCell Practicecell = new PdfPCell();
                    //                        //        Practicecell.AddElement(p);
                    //                        //        Practicecell.Border = Rectangle.NO_BORDER;
                    //                        //        Practicecell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //                        //        PracticeTable.AddCell(Practicecell);
                    //                        //    }


                    //                        //}


                    //                        //ReportHeaderTable.AddCell(PracticeTable);

                    //                        #endregion
                    //                        //}

                    //                        #endregion

                    //                        #region BarCode

                    //                        ReportHeaderTable.AddCell(ReportHeaderBarCode);

                    //                        ReportHeaderTable.AddCell(headerSovreignAddress);

                    //                        string clientDetails = "";
                    //                        clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.NameColumn.ColumnName] + "\n");
                    //                        clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.AddressColumn.ColumnName], "\n");
                    //                        clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.CityColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.CityColumn.ColumnName] + ", ");
                    //                        clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.StateColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.StateColumn.ColumnName] + ", ");
                    //                        clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName] + "\n");
                    //                        clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.PhoneNoColumn.ColumnName]);




                    //                        Paragraph p = new Paragraph(clientDetails, bodyFont);
                    //                        PdfPCell clientAccountNumber = new PdfPCell();
                    //                        p.Alignment = Element.ALIGN_RIGHT;
                    //                        clientAccountNumber.AddElement(p);
                    //                        clientAccountNumber.Border = Rectangle.NO_BORDER;
                    //                        clientAccountNumber.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //                        ReportHeaderTable.AddCell(clientAccountNumber);



                    //                        #endregion

                    //                        BaseColor myColor1 = WebColors.GetRGBColor("#66B2FF");
                    //                        LineSeparator line2 = new LineSeparator(1f, 100f, myColor1, iTextSharp.text.Element.ALIGN_CENTER, -1);
                    //                        // line2.Offset = 1;
                    //                        PdfPCell lineCell11 = new PdfPCell();
                    //                        lineCell11.Colspan = 2;
                    //                        lineCell11.Border = Rectangle.NO_BORDER;
                    //                        lineCell11.AddElement(new Chunk(line2));

                    //                        pdfDocument.Add(ReportHeaderTable);
                    //                        pdfDocument.Add(line2);
                    //                    }


                    //                    #region Patient/Provider

                    //                    try
                    //                    {
                    //                        if (IsReportHeaderApplied)
                    //                        {
                    //                            PdfPTable ReportPatientProvider = new PdfPTable(2);
                    //                            ReportPatientProvider.TotalWidth = 575f;
                    //                            ReportPatientProvider.SpacingBefore = 2f;
                    //                            ReportPatientProvider.SpacingAfter = 2f;
                    //                            ReportPatientProvider.LockedWidth = true;
                    //                            ReportPatientProvider.DefaultCell.Border = Rectangle.NO_BORDER;

                    //                            #region  Patient
                    //                            iTextSharp.text.Font PFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, fontColour);

                    //                            PdfPTable PatientTable = new PdfPTable(1);
                    //                            PatientTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    //                            string PatientText = GetPatientText(patientId);
                    //                            string[] PatientColumns = PatientText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                    //                            PatientTable.AddCell(new Paragraph("Patient", PFont));
                    //                            foreach (string PatientColumn in PatientColumns)
                    //                            {

                    //                                PatientTable.AddCell(new Paragraph(PatientColumn, bodyFont));


                    //                            }

                    //                            ReportPatientProvider.AddCell(PatientTable);

                    //                            #endregion

                    //                            #region  Provider

                    //                            PdfPTable ProviderTable = new PdfPTable(1);

                    //                            ProviderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    //                            string ProviderText = GetProviderText(providerId);
                    //                            string[] ProviderColumns = ProviderText.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                    //                            foreach (string ProviderColumn in ProviderColumns)
                    //                            {

                    //                                Paragraph p;
                    //                                if (ProviderColumn == "Provider")
                    //                                {
                    //                                    p = new Paragraph(ProviderColumn, PFont);
                    //                                }
                    //                                else
                    //                                {
                    //                                    p = new Paragraph(ProviderColumn, bodyFont);
                    //                                }
                    //                                p.Alignment = Element.ALIGN_RIGHT;
                    //                                PdfPCell Providercell = new PdfPCell();
                    //                                Providercell.AddElement(p);
                    //                                Providercell.Border = Rectangle.NO_BORDER;
                    //                                Providercell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //                                ProviderTable.AddCell(Providercell);
                    //                            }

                    //                            ReportPatientProvider.AddCell(ProviderTable);

                    //                            #endregion

                    //                            pdfDocument.Add(ReportPatientProvider);
                    //                        }


                    //                    }
                    //                    catch (Exception ex)
                    //                    {
                    //                        MDVLogger.BLLErrorLog("BLLAdminClinical::getReportHeaderTagsValue", ex);
                    //                    }


                    //                    #endregion

                    //                    //Line Seperator
                    //                    BaseColor myColor = WebColors.GetRGBColor("#66B2FF");
                    //                    LineSeparator Borderline = new LineSeparator(1f, 100f, myColor, iTextSharp.text.Element.ALIGN_CENTER, 0);
                    //                    pdfDocument.Add(Borderline);

                    //                    #endregion

                    #endregion


                    pdf.Document.Open();
                    BaseColor myColor1 = WebColors.GetRGBColor("#66B2FF");
                    LineSeparator line2 = new LineSeparator(1f, 100f, myColor1, Element.ALIGN_CENTER, -1);
                    pdfDocument.Add(line2);
                    #region Patient/Provider Segment
                    string AccountNumber = dsLabOrder.Tables[dsPatient.Patients.TableName].Rows[0]["AccountNumber"].ToString();
                    try
                    {
                        PdfPTable ReportPatientProvider = new PdfPTable(2);
                        ReportPatientProvider.TotalWidth = 575f;
                        ReportPatientProvider.SpacingBefore = 3f;
                        ReportPatientProvider.SpacingAfter = 8f;
                        ReportPatientProvider.LockedWidth = true;
                        ReportPatientProvider.DefaultCell.Border = Rectangle.NO_BORDER;

                        //ReportPatientProvider.AddCell(SetLabPatient(patientId, patientName, fontColour, bodyFont, componentHeadingFont, patientNameFont, AccountNumber));
                        //ReportPatientProvider.AddCell(SetLabProvider(providerId, patientNameFont, bodyFont));
                        pdfDocument.Add(ReportPatientProvider);
                    }
                    catch (Exception ex)
                    {
                        MDVLogger.BLLErrorLog("BLLAdminClinical::LabOrderPreview, Patient or Provider Segment", ex);
                    }

                    BaseColor myColor = WebColors.GetRGBColor("#66B2FF");
                    LineSeparator Borderline = new LineSeparator(1f, 100f, myColor, Element.ALIGN_CENTER, 4);
                    pdfDocument.Add(Borderline);

                    Paragraph requisitionHeading = new Paragraph("Lab Requisition".ToString(), patientNameFont);
                    LineSeparator requisitionHeadingLine = new LineSeparator(1f, 100f, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -1);
                    requisitionHeadingLine.Offset = 1;
                    pdfDocument.Add(requisitionHeading);


                    #endregion

                    #region Lab Requisition

                    // Order Information & Insured Relationship
                    labOrderInformation(pdfDocument, dsLabOrder, componentHeaderFont, componentHeadingFont, bodyFont);


                    // Guarantor
                    labOrderGuarantor(pdfDocument, componentHeadingFont, componentHeaderFont, bodyFont, dsLabOrder);


                    #endregion

                    #region Comments

                    Paragraph comments_Heading = new Paragraph("Comments\n".ToString(), componentHeadingFont);
                    pdfDocument.Add(comments_Heading);

                    #endregion

                    #region e-Signed

                    pdfDocument.Add(Chunk.NEWLINE);
                    pdfDocument.Add(Chunk.NEWLINE);
                    Paragraph signedBy = new Paragraph("e-Signed By: " + MDVUtility.ToStr(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows[0][dsLabOrder.OS_LabOrder.ModifiedByColumn.ColumnName]) + " on " + MDVUtility.ToDateTime(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows[0][dsLabOrder.OS_LabOrder.ModifiedOnColumn.ColumnName]).ToLongDateString() + " at " + MDVUtility.ToDateTime(dsLabOrder.Tables[dsLabOrder.OS_LabOrder.TableName].Rows[0][dsLabOrder.OS_LabOrder.ModifiedOnColumn.ColumnName]).ToLongTimeString(), bodyFont);
                    signedBy.Alignment = Element.ALIGN_LEFT;
                    pdfDocument.Add(signedBy);

                    #endregion



                    #region But in the end It doesn't even matter

                    pdf.Document.Close();
                    pdf.Writer.Close();
                    pdfDocument.Close();

                    MemoryStream stream = new MemoryStream(stream_.ToArray());
                    PdfReader npdf = new PdfReader(stream);
                    MemoryStream outstream = new MemoryStream();

                    var color = System.Drawing.ColorTranslator.FromHtml("#fff");
                    var fontcolor = new BaseColor(color);
                    Font componentFooterFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, fontcolor);
                    using (PdfStamper stamper = new PdfStamper(npdf, outstream, '\0', true))
                    {
                        stamper.Writer.CloseStream = false;
                        int PageCount = npdf.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, new Paragraph(String.Format("Page {0} of {1}", i, PageCount), componentFooterFont), 555, 18, 0);
                    }
                    newByteArr = outstream.GetBuffer();

                    #endregion


                }
                return new BLObject<byte[]>(newByteArr);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::previewLabOrder", ex);
                return new BLObject<byte[]>(null, ex.Message);
            }
        }
        private string GetPracticeText(long practiceId)
        {
            DSProfile ds = new DSProfile();
            ds = new DALPractice().LoadPractice(practiceId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            string PatientHtml = string.Empty;
            if (ds != null)
            {
                if (ds.Tables[ds.Practice.TableName].Rows.Count > 0)
                {
                    DataRow drPractice = ds.Tables[ds.Practice.TableName].Rows[ds.Tables[ds.Practice.TableName].Rows.Count - 1];

                    PatientHtml = string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.ShortNameColumn.ColumnName]), "<br/>");
                    PatientHtml = string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.AddressColumn.ColumnName]), "<br/>");
                    PatientHtml = string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.Address2Column.ColumnName]));
                    PatientHtml = MDVUtility.ToStr(drPractice[ds.Practice.CityColumn.ColumnName]) == string.Empty ? string.Empty : string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.CityColumn.ColumnName]), ", ");
                    PatientHtml = MDVUtility.ToStr(drPractice[ds.Practice.StateColumn.ColumnName]) == string.Empty ? string.Empty : string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.StateColumn.ColumnName]), ", ");
                    PatientHtml = MDVUtility.ToStr(drPractice[ds.Practice.ZIPCodeColumn.ColumnName]) == string.Empty ? string.Empty : string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.ZIPCodeColumn.ColumnName]));
                    PatientHtml = string.Concat(PatientHtml, "<br/>");
                    PatientHtml = string.Concat(PatientHtml, MDVUtility.ToStr(drPractice[ds.Practice.PhoneNoColumn.ColumnName]), "<br/>");

                }

            }
            return PatientHtml;
        }

        #region Procedures lookups
        public BLObject<DSOS_ProblemLists> LookupProblemListsForOrderSet(int orderSetId, int problemListId = -1, string ProblemName = null)
        {
            try
            {
                DSOS_ProblemLists ds = new DSOS_ProblemLists();
                ds = new DALOS_ProblemLists().LookupProblemListsForOrderSet(orderSetId, problemListId, ProblemName);
                return new BLObject<DSOS_ProblemLists>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LookupProblemListsForOrderSet", ex);
                return new BLObject<DSOS_ProblemLists>(null, ex.Message);
            }

        }
        #endregion

        public BLObject<DSOS_LabOrder> FillLabOrder(long labOrderId)
        {
            try
            {
                DSOS_LabOrder ds = new DSOS_LabOrder();
                ds = new DALOS_LabOrder().FillLabOrder(labOrderId);
                return new BLObject<DSOS_LabOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::FillLabOrder", ex);
                return new BLObject<DSOS_LabOrder>(null, ex.Message);
            }
        }
        #endregion

        #region "Lab Order Test"

        public BLObject<DSOS_LabOrder> insertUpdateLabOrderTest(DSOS_LabOrder ds)
        {
            try
            {
                ds = new DALOS_LabOrder().insertUpdateLabOrderTest(ds);
                return new BLObject<DSOS_LabOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::insertUpdateLabOrderTest", ex);
                return new BLObject<DSOS_LabOrder>(null, ex.Message);
            }
        }
        public BLObject<DSOS_LabOrder> LoadLabOrderTest(long labOrderId, Int32 LabOrderTestId, long OrderSetId, string pageNumber, string rowsPerPage)
        {
            try
            {
                DSOS_LabOrder ds = new DSOS_LabOrder();
                ds = new DALOS_LabOrder().LoadLabOrderTest(labOrderId, LabOrderTestId, OrderSetId, pageNumber, rowsPerPage);
                return new BLObject<DSOS_LabOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadLabOrderTest", ex);
                return new BLObject<DSOS_LabOrder>(null, ex.Message);
            }
        }

        public BLObject<string> deleteLabOrderTest(string labOrderTestId)
        {
            try
            {
                labOrderTestId = new DALOS_LabOrder().deleteLabOrderTest(Convert.ToInt64(labOrderTestId));
                return new BLObject<string>(labOrderTestId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::deleteLabOrderTest", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        // Author:  Abid Ali
        // Created Date: 18/04/2016
        //OverView: Methods to use functions of DALLabResult

        #region Lab Result

        // Author:  Abid Ali
        // Created Date: 18/04/2016
        //For loading ResultSpecimen
        public BLObject<DSLabResult> loadLabResultSpecimen(long specimenId, long labOrderTestId)
        {
            try
            {
                DSLabResult ds = new DSLabResult();
                ds = new DALLabResult().loadLabResultSpecimen(specimenId, labOrderTestId);
                return new BLObject<DSLabResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::loadLabResultSpecimen", ex);
                return new BLObject<DSLabResult>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 18/04/2016
        //For loading ResultSpecimenRejectReason
        public BLObject<DSLabResult> loadLabResultSpecimenRejectReason(long specimenRejectReasonId, long labOrderTestId)
        {
            try
            {
                DSLabResult ds = new DSLabResult();
                ds = new DALLabResult().loadLabResultSpecimenRejectReason(specimenRejectReasonId, labOrderTestId);
                return new BLObject<DSLabResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::loadLabResultSpecimenRejectReason", ex);
                return new BLObject<DSLabResult>(null, ex.Message);
            }
        }
        public BLObject<DSLabResult> insertLabResultSpecimen(DSLabResult ds)
        {
            try
            {
                ds = new DALLabResult().insertLabResultSpecimen(ds);
                return new BLObject<DSLabResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::insertLabResultSpecimen", ex);
                return new BLObject<DSLabResult>(null, ex.Message);
            }
        }
        public BLObject<DSLabResult> insertLabResultSpecimenRejectReason(DSLabResult ds)
        {
            try
            {
                ds = new DALLabResult().insertLabResultSpecimenRejectReason(ds);
                return new BLObject<DSLabResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::insertLabResultSpecimenRejectReason", ex);
                return new BLObject<DSLabResult>(null, ex.Message);
            }
        }
        // Author:  Abid Ali
        // Created Date: 18/04/2016
        //OverView: Method to Insert/Update LabResult
        public BLObject<DSLabResult> InsertUpdateLabResult(DSLabResult ds)
        {
            try
            {
                ds = new DALLabResult().InsertUpdateLabResult(ds);
                return new BLObject<DSLabResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::InsertUpdateLabResult", ex);
                return new BLObject<DSLabResult>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 18/04/2016
        //OverView: Method to Load LabResult
        public BLObject<DSLabResult> LoadLabResult(long labResultId, long labOrderId, string pageNumber, string rowsPerPage, string test, string orderNo, long providerId, string orderDateFrom, string orderDateTo, string status, string labId, long noteId, long patientId, string isViewOrder = "", string isPrintOrder = "")
        {
            try
            {
                DSLabResult ds = new DSLabResult();
                ds = new DALLabResult().LoadLabResult(labResultId, labOrderId, pageNumber, rowsPerPage, test, orderNo, providerId, orderDateFrom, orderDateTo, status, labId, noteId, patientId, isViewOrder, isPrintOrder);
                return new BLObject<DSLabResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadLabResult", ex);
                return new BLObject<DSLabResult>(null, ex.Message);
            }
        }

        // Author:  Farooq Ahmad
        // Created Date: 22/08/2016
        //OverView: Method to Load Lab UnsolicitedResult
        public BLObject<DSLabResult> LoadLabUnsolicitedResult(long labResultId, long labOrderId, string pageNumber, string rowsPerPage, string test, string orderNo, long providerId, string orderDateFrom, string orderDateTo, string status, string labId)
        {
            try
            {
                DSLabResult ds = new DSLabResult();
                ds = new DALLabResult().LoadLabUnsolicitedResult(labResultId, labOrderId, pageNumber, rowsPerPage, test, orderNo, providerId, orderDateFrom, orderDateTo, status, labId);
                return new BLObject<DSLabResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadLabResult", ex);
                return new BLObject<DSLabResult>(null, ex.Message);
            }
        }
        public BLObject<DSRadiologyResult> LoadRadiologyResult(long radiologyResultId, long radiologyOrderId, string pageNumber, string rowsPerPage, string test, string orderNo, long providerId, string orderDateFrom, string orderDateTo, string status, long patientId, long noteId = 0, long labId = 0, string isViewOrder = "", string isPrintOrder = "", bool AllResults = false)
        {
            try
            {
                DSRadiologyResult ds = new DSRadiologyResult();
                if (AllResults)
                {
                    ds = new DALRadiologyResult().LoadRadiologyResult(radiologyResultId, radiologyOrderId, pageNumber, rowsPerPage, test, orderNo, providerId, orderDateFrom, orderDateTo, status, patientId, isViewOrder, isPrintOrder, noteId, labId, 0, AllResults);
                }
                else
                {
                    ds = new DALRadiologyResult().LoadRadiologyResult(radiologyResultId, radiologyOrderId, pageNumber, rowsPerPage, test, orderNo, providerId, orderDateFrom, orderDateTo, status, patientId, isViewOrder, isPrintOrder, 0, labId, 0, AllResults);
                }

                return new BLObject<DSRadiologyResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadRadiologyResult", ex);
                return new BLObject<DSRadiologyResult>(null, ex.Message);
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 18/04/2016
        //OverView: Method for Order Result LOINC Lookup
        public BLObject<DSLabResult> LabResultLOINCLookup(string LOINCCode = "", string LOINCCOdeDescription = "", string LabId = "")
        {
            try
            {
                DSLabResult ds = new DSLabResult();
                ds = new DALLabResult().LookupLabResultLOINC(LOINCCode, LOINCCOdeDescription, LabId);
                return new BLObject<DSLabResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadLabResult", ex);
                return new BLObject<DSLabResult>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 18/04/2016
        //OverView: Method to Delete Lab Result
        public BLObject<string> DeleteLabResult(string labResultId)
        {
            try
            {
                labResultId = new DALLabResult().DeleteLabResult(MDVUtility.ToStr(labResultId));
                return new BLObject<string>(labResultId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::DeleteLabResult", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 18/04/2016
        //OverView: Method to Insert/Update LabResult Detail
        public BLObject<DSLabResult> InsertUpdateLabResultDetail(DSLabResult ds)
        {
            try
            {
                ds = new DALLabResult().insertUpdateLabResultDetail(ds);
                return new BLObject<DSLabResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::InsertUpdateLabResultDetail", ex);
                return new BLObject<DSLabResult>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 18/04/2016
        //OverView: Method to Load LabResult Detail
        public BLObject<DSLabResult> LoadLabResultDetail(long labResultDetailId, long labResultId)
        {
            try
            {
                DSLabResult ds = new DSLabResult();
                ds = new DALLabResult().loadLabResultDetail(labResultDetailId, labResultId);
                return new BLObject<DSLabResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadLabResultDetail", ex);
                return new BLObject<DSLabResult>(null, ex.Message);
            }
        }
        public BLObject<DSLabResult> LoadLabResultForCDS(long patientId)
        {
            try
            {
                DSLabResult ds = new DSLabResult();
                ds = new DALLabResult().LoadLabResultForCDS(patientId);
                return new BLObject<DSLabResult>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadLabResultForCDS", ex);
                return new BLObject<DSLabResult>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 31/03/2016
        //OverView: Method to Load LabOrder Problems
        //public BLObject<DSOS_LabOrder> LoadLabOrderProblems(long labOrderId, long patientId, int pageNumber, int rowsPerPage)
        //{
        //    try
        //    {
        //        DSOS_LabOrder ds = new DSOS_LabOrder();
        //        ds = new DALOS_LabOrder().loadLabOrderProblems(0, labOrderId, patientId, pageNumber, rowsPerPage);
        //        return new BLObject<DSOS_LabOrder>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLOrderSetl::LoadLabOrderProblems", ex);
        //        return new BLObject<DSOS_LabOrder>(null, ex.Message);
        //    }
        //}

        // Author:  Abid Ali
        // Created Date: 18/04/2016
        //OverView: Method to Delete LabOrder Problems
        public BLObject<string> DeleteLabResultDetail(string labResultDetailId)
        {
            try
            {
                labResultDetailId = new DALLabResult().deleteLabResultDetail(Convert.ToInt64(labResultDetailId));
                return new BLObject<string>(labResultDetailId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::DeleteLabResultDetail", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 31/03/2016
        // Description: Attaching Lab Order with Notes
        //public BLObject<DSOS_LabOrder> attachLabOrderWithNotes(string labOrderId, long noteId)
        //{
        //    try
        //    {
        //        DSOS_LabOrder ds = new DSOS_LabOrder();
        //        ds = new DALOS_LabOrder().attachLabOrderWithNotes(labOrderId, noteId);
        //        return new BLObject<DSOS_LabOrder>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLOrderSetl::attachLabOrderWithNotes", ex);
        //        return new BLObject<DSOS_LabOrder>(null, ex.Message);
        //    }
        //}

        //// Author:  Abid Ali
        //// Created Date: 31/03/2016
        //// Description: Detaching Lab Order from Notes
        //public BLObject<string> detachLabOrderFromNotes(string labOrderId, long noteId)
        //{
        //    try
        //    {
        //        var msgLabOrderId = new DALOS_LabOrder().detachLabOrderFromNotes(MDVUtility.ToInt64(labOrderId), noteId);
        //        return new BLObject<string>(msgLabOrderId);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLOrderSetl::detachLabOrderFromNotes", ex);
        //        return new BLObject<string>(null, ex.Message);
        //    }
        //}

        //// Author:  Abid Ali
        //// Created Date: 31/03/2016
        //// Description: Creates PDF to view Lab Order
        //public BLObject<byte[]> previewLabOrder(long labId, long patientId)
        //{
        //    try
        //    {
        //        DSOS_LabOrder dsLabOrder = new DSOS_LabOrder();

        //        dsLabOrder = new DALOS_LabOrder().LoadLabOrder(labId, patientId, "", "", "", "", 0, "", "", "", 0);
        //        dsLabOrder.Merge(new DALPatient().FillPatient(patientId, "", ""));
        //        dsLabOrder.Merge(new DALOS_LabOrder().loadLabOrderProblems(0, labId, patientId));
        //        dsLabOrder.Merge(new DALOS_LabOrder().LoadLabOrderTest(labId, 0, patientId, "", ""));

        //        byte[] newByteArr = null;
        //        using (MemoryStream stream_ = new MemoryStream())
        //        {
        //            Document pdfDocument = new Document(PageSize.LETTER, 20, 20, 20, 20);
        //            // Heading Font Style
        //            var fontColour = new BaseColor(102, 178, 255);
        //            iTextSharp.text.Font patientNameFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, fontColour);
        //            iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        //            iTextSharp.text.Font componentHeadingFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, fontColour);
        //            iTextSharp.text.Font componentHeaderFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

        //             MDVUtility.PDFCreator pdf = new  MDVUtility.PDFCreator(ref pdfDocument, stream_, false, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Requisition");
        //            pdf.Document.Open();

        //            DSPatient dsPatient = new DSPatient();

        //            #region Patient's Data

        //            // Start Append Patient's Data
        //            PdfPTable patientTable = new PdfPTable(2);
        //            patientTable.TotalWidth = 575f;
        //            patientTable.LockedWidth = true;
        //            patientTable.DefaultCell.Border = Rectangle.NO_BORDER;

        //            foreach (DSPatient.PatientsRow dr in dsLabOrder.Tables[dsPatient.Patients.TableName].Rows)
        //            {
        //                Paragraph Patient_Name = new Paragraph(dr[dsPatient.Patients.FullNameColumn.ColumnName].ToString(), patientNameFont);

        //                Paragraph Patient_Body1 = new Paragraph(dr[dsPatient.Patients.AccountNumberColumn.ColumnName].ToString(), bodyFont);

        //                string age = GetAge(MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]));

        //                PdfPTable ageTable = new PdfPTable(1);
        //                ageTable.DefaultCell.Border = Rectangle.NO_BORDER;
        //                ageTable.AddCell(new Paragraph(string.Format("{0} {1}", age.Trim(), dr[dsPatient.Patients.GenderColumn.ColumnName].ToString()), bodyFont));
        //                ageTable.AddCell(new Paragraph(string.Format("Speaks {0}", dr[dsPatient.Patients.LanguageNameColumn.ColumnName].ToString(), dr[dsPatient.Patients.LanguageNameColumn.ColumnName].ToString()), bodyFont));
        //                ageTable.AddCell(new Paragraph(dr[dsPatient.Patients.Address1Column.ColumnName].ToString(), bodyFont));


        //                PdfPTable emailTable = new PdfPTable(2);
        //                emailTable.DefaultCell.Border = Rectangle.NO_BORDER;
        //                emailTable.AddCell(new Paragraph("Email:", bodyFont));
        //                emailTable.AddCell(new Paragraph(dr[dsPatient.Patients.EmailAddressColumn.ColumnName].ToString(), bodyFont));
        //                emailTable.AddCell(new Paragraph("Home Phone:", bodyFont));
        //                emailTable.AddCell(new Paragraph(dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName].ToString(), bodyFont));
        //                emailTable.AddCell(new Paragraph("Work Phone:", bodyFont));
        //                emailTable.AddCell(new Paragraph(dr[dsPatient.Patients.WorkPhoneNoColumn.ColumnName].ToString(), bodyFont));
        //                emailTable.AddCell(new Paragraph("Cell Phone:", bodyFont));
        //                emailTable.AddCell(new Paragraph(dr[dsPatient.Patients.CellNoColumn.ColumnName].ToString(), bodyFont));
        //                patientTable.AddCell(ageTable);

        //                PdfPCell cell = new PdfPCell();
        //                cell.AddElement(emailTable);
        //                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                cell.Border = Rectangle.NO_BORDER;
        //                patientTable.AddCell(cell);

        //                patientTable.DefaultCell.Padding = 4;
        //                pdfDocument.Add(Patient_Name);
        //                pdfDocument.Add(Patient_Body1);
        //                pdfDocument.Add(patientTable);
        //            }
        //            // End Append Patient's Data

        //            #endregion
        //            //Start 28-03-2016 Humaira Yousaf
        //            Paragraph req_Heading = new Paragraph("Lab Requisition".ToString(), patientNameFont);
        //            //End 28-03-2016 Humaira Yousaf
        //            LineSeparator line = new LineSeparator(1f, 100f, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -1);
        //            line.Offset = 1;
        //            pdfDocument.Add(new Chunk(line));
        //            req_Heading.SpacingAfter = -12;
        //            req_Heading.SpacingBefore = -5;
        //            pdfDocument.Add(req_Heading);
        //            Chunk c = new Chunk(line);

        //            pdfDocument.Add(new Chunk(line));
        //            pdfDocument.Add(Chunk.NEWLINE);

        //            #region Order Information

        //            // Start Append Order Information
        //            if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
        //            {
        //                Paragraph order_Heading = new Paragraph("Order Information \n".ToString(), componentHeadingFont);
        //                order_Heading.SpacingBefore = order_Heading.SpacingAfter = 5;

        //                pdfDocument.Add(order_Heading);


        //                PdfPTable orderTable = new PdfPTable(5);
        //                float[] widths = new float[] { 8f, 8f, 8f, 8f, 8f };
        //                orderTable.SetWidths(widths);
        //                orderTable.TotalWidth = 575f;
        //                orderTable.LockedWidth = true;
        //                orderTable.DefaultCell.Border = Rectangle.NO_BORDER;
        //                foreach (DSOS_LabOrder.LabOrderRow dr in dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows)
        //                {
        //                    orderTable.AddCell(new Paragraph("Provider:", componentHeaderFont));
        //                    orderTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.ProviderColumn.ColumnName])), bodyFont));
        //                    orderTable.AddCell(new Paragraph("Assignee:", componentHeaderFont));
        //                    orderTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.AssigneeNameColumn.ColumnName])), bodyFont));
        //                    orderTable.AddCell(string.Empty);
        //                    orderTable.AddCell(new Paragraph("Date & Time:", componentHeaderFont));
        //                    orderTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dr[dsLabOrder.LabOrder.OrderDateColumn.ColumnName]).ToShortDateString() + ' ' + dr[dsLabOrder.LabOrder.OrderTimeColumn.ColumnName].ToString(), bodyFont));
        //                    orderTable.AddCell(new Paragraph("Order Number:", componentHeaderFont));
        //                    orderTable.AddCell(new Paragraph(dr[dsLabOrder.LabOrder.OrderNoColumn.ColumnName].ToString(), bodyFont));
        //                    orderTable.AddCell(string.Empty);
        //                }
        //                pdfDocument.Add(orderTable);
        //            }
        //            else
        //            {

        //                Paragraph order_Heading = new Paragraph("Order Information \n".ToString(), componentHeadingFont);
        //                order_Heading.SpacingBefore = order_Heading.SpacingAfter = 5;
        //                pdfDocument.Add(order_Heading);

        //                PdfPTable orderTable = new PdfPTable(5);
        //                float[] widths = new float[] { 1f, 8f, 8f, 8f, 8f };
        //                orderTable.SetWidths(widths);
        //                orderTable.TotalWidth = 575f;
        //                orderTable.LockedWidth = true;
        //                orderTable.DefaultCell.Border = Rectangle.NO_BORDER;
        //                orderTable.AddCell(string.Empty);
        //                pdfDocument.Add(orderTable);
        //                Paragraph noOrder = new Paragraph("No Order Information Found".ToString());
        //                noOrder.Alignment = Element.ALIGN_CENTER;
        //                pdfDocument.Add(noOrder);
        //            }
        //            // End Append Order Information

        //            #endregion

        //            #region Radiology Information

        //            // Start Append Test Information
        //            if (dsLabOrder.Tables[dsLabOrder.LabOrderTest.TableName].Rows.Count > 0)
        //            {
        //                //Start 28-03-2016 Humaira Yousaf
        //                Paragraph test_Heading = new Paragraph("Lab Information \n".ToString(), componentHeadingFont);
        //                //End 28-03-2016 Humaira Yousaf

        //                test_Heading.SpacingBefore = test_Heading.SpacingAfter = 5;
        //                pdfDocument.Add(test_Heading);

        //                PdfPTable testTable = new PdfPTable(5);
        //                float[] widths = new float[] { 0f, 8f, 8f, 8f, 8f };
        //                testTable.SetWidths(widths);
        //                testTable.TotalWidth = 575f;
        //                testTable.LockedWidth = true;
        //                testTable.DefaultCell.Border = Rectangle.NO_BORDER;

        //                foreach (DSOS_LabOrder.LabOrderTestRow dr in dsLabOrder.Tables[dsLabOrder.LabOrderTest.TableName].Rows)
        //                {
        //                    testTable.AddCell(string.Empty);
        //                    testTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dr[dsLabOrder.LabOrderTest.TestDateColumn.ColumnName]).ToShortDateString() + ' ' + dr[dsLabOrder.LabOrderTest.TestTimeColumn.ColumnName].ToString(), bodyFont));
        //                    testTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.LabOrderTest.CPTCodeDescriptionColumn.ColumnName])), bodyFont));
        //                    testTable.AddCell(new Paragraph("Urgency: " + dr[dsLabOrder.LabOrderTest.UrgencyNameColumn.ColumnName].ToString(), bodyFont));
        //                    testTable.AddCell(string.Empty);
        //                }
        //                pdfDocument.Add(testTable);
        //            }
        //            else
        //            {
        //                //Start 28-03-2016 Humaira Yousaf
        //                Paragraph test_Heading = new Paragraph("Lab Information \n".ToString(), componentHeadingFont);
        //                //End 28-03-2016 Humaira Yousaf
        //                test_Heading.SpacingBefore = test_Heading.SpacingAfter = 5;
        //                pdfDocument.Add(test_Heading);
        //                PdfPTable testTable = new PdfPTable(5);
        //                float[] widths = new float[] { 8f, 8f, 8f, 8f, 8f };
        //                testTable.SetWidths(widths);
        //                testTable.TotalWidth = 575f;
        //                testTable.LockedWidth = true;
        //                testTable.DefaultCell.Border = Rectangle.NO_BORDER;
        //                testTable.AddCell(string.Empty);
        //                pdfDocument.Add(testTable);
        //                //Start 28-03-2016 Humaira Yousaf
        //                Paragraph noTest = new Paragraph("No Lab Information Found".ToString());
        //                //End 28-03-2016 Humaira Yousaf
        //                noTest.Alignment = Element.ALIGN_CENTER;
        //                pdfDocument.Add(noTest);
        //            }
        //            // End Append Associated Problems

        //            #endregion

        //            #region Problem List

        //            // Start Append Associated Problems
        //            if (dsLabOrder.Tables[dsLabOrder.LabOrderProblem.TableName].Rows.Count > 0)
        //            {

        //                Paragraph problems_Heading = new Paragraph("Associated Problems \n".ToString(), componentHeadingFont);
        //                problems_Heading.SpacingBefore = problems_Heading.SpacingAfter = 5;
        //                pdfDocument.Add(problems_Heading);

        //                PdfPTable problemsTable = new PdfPTable(1);
        //                float[] widths = new float[] { 8f };
        //                problemsTable.SetWidths(widths);
        //                problemsTable.TotalWidth = 575f;
        //                problemsTable.LockedWidth = true;
        //                problemsTable.DefaultCell.Border = Rectangle.NO_BORDER;
        //                problemsTable.AddCell(string.Empty);

        //                foreach (DSOS_LabOrder.LabOrderProblemRow dr in dsLabOrder.Tables[dsLabOrder.LabOrderProblem.TableName].Rows)
        //                {
        //                    problemsTable.AddCell(string.Empty);
        //                    problemsTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.LabOrderProblem.ProblemNameColumn.ColumnName])), bodyFont));
        //                    problemsTable.AddCell(string.Empty);
        //                }
        //                pdfDocument.Add(problemsTable);
        //            }
        //            else
        //            {

        //                Paragraph problems_Heading = new Paragraph("Associated Problems \n".ToString(), componentHeadingFont);
        //                problems_Heading.SpacingBefore = problems_Heading.SpacingAfter = 5;
        //                pdfDocument.Add(problems_Heading);
        //                PdfPTable problemsTable = new PdfPTable(5);
        //                float[] widths = new float[] { 1f, 8f, 8f, 8f, 8f };
        //                problemsTable.SetWidths(widths);
        //                problemsTable.TotalWidth = 575f;
        //                problemsTable.LockedWidth = true;
        //                problemsTable.DefaultCell.Border = Rectangle.NO_BORDER;
        //                problemsTable.AddCell(string.Empty);
        //                pdfDocument.Add(problemsTable);
        //                Paragraph noProblems = new Paragraph("No Associated Problems Found".ToString());
        //                noProblems.Alignment = Element.ALIGN_CENTER;
        //                pdfDocument.Add(noProblems);
        //            }
        //            // End Append Associated Problems

        //            #endregion
        //            //Start 28-03-2016 Humaira Yousaf
        //            pdfDocument.Add(Chunk.NEWLINE);
        //            pdfDocument.Add(Chunk.NEWLINE);
        //            Paragraph signedByPara = new Paragraph("Signed By: " + MDVUtility.ToStr(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ModifiedByColumn.ColumnName]) + " on " + MDVUtility.ToDateTime(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ModifiedOnColumn.ColumnName]).DayOfWeek + "," + MDVUtility.ToDateTime(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ModifiedOnColumn.ColumnName]).ToShortDateString() + " " + MDVUtility.ToDateTime(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ModifiedOnColumn.ColumnName]).ToShortTimeString(), bodyFont);
        //            signedByPara.Alignment = Element.ALIGN_LEFT;
        //            pdfDocument.Add(signedByPara);
        //            //End 28-03-2016 Humaira Yousaf

        //            pdf.Document.Close();
        //            pdf.Writer.Close();

        //            newByteArr = stream_.GetBuffer();
        //        }

        //        return new BLObject<byte[]>(newByteArr);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLOrderSetl::previewLabOrder", ex);
        //        return new BLObject<byte[]>(null, ex.Message);
        //    }
        //}

        /// <summary>
        /// Module Name: previewLabResult
        /// Author: Humaira Yousaf
        /// Created Date: 26-04-2016
        /// Description: Creates PDF to view Lab Result
        /// </summary>
        /// <param name="labResultId" type="long">labResultId</param>
        /// <param name="labOrderId" type="long">labOrderId</param>
        /// <param name="patientId" type="long">patientId</param>

        #region LabOrder & LabOrderResult PDF

        private void setLabHeader(Document pdfDocument, long practiceId, long patientId, DSLab dsLab, DSOS_LabOrder dsLabOrder, Font bodyFont, string BarCodeHtmlText, bool isLabOrder = true)
        {
            PdfPTable HeaderLogoTable = new PdfPTable(2);
            #region Practice

            string practiceAddress = GetPracticeText(practiceId);
            string[] practiceAddresses = practiceAddress.Split(new string[] { "<br/>" }, StringSplitOptions.None);

            practiceAddress = "";
            foreach (var column in practiceAddresses)
                if (column != string.Empty)
                    practiceAddress += column + "\n";

            practiceAddress = practiceAddress.Remove(practiceAddress.Length - 1);
            practiceAddress += "\n Account Number: " + dsLab.Lab.Rows[0][dsLab.Lab.ClientNoColumn.ColumnName].ToString();

            Paragraph p_practiceAddress = new Paragraph(practiceAddress, bodyFont);

            PdfPCell headerSovreignAddress = new PdfPCell();
            headerSovreignAddress.AddElement(p_practiceAddress);
            headerSovreignAddress.Border = Rectangle.NO_BORDER;
            headerSovreignAddress.HorizontalAlignment = Element.ALIGN_LEFT;

            #endregion

            #region Header

            if (true || BarCodeHtmlText != string.Empty)
            {
                BCommon.Barcode39 b = new BCommon.Barcode39();
                System.Drawing.Image img;
                b.ShowString = true;

                b.IncludeCheckSumDigit = false;
                b.TextFont = new System.Drawing.Font("Courier New", 9);

                #region BarCode/String/Logo

                string barCodeCondition1 = ""; // PatientAccount
                string barCodeCondition2 = ""; // Order Number
                string barCodeCondition3 = ""; // PatientLastName
                string barCodeCondition4 = ""; // PatientFirstNameInitial

                StringBuilder strBarcode = new StringBuilder();
                if (dsLabOrder != null && dsLabOrder.OS_LabOrder.Rows.Count > 0)
                {
                    DSPatient dsPatient_BarCode = new DALPatient().FillPatient(patientId, "", "");
                    if (dsPatient_BarCode.Patients.Rows.Count > 0)
                    {
                        barCodeCondition1 = MDVUtility.ToStr(dsLab.Lab.Rows[0][dsLab.Lab.ClientNoColumn.ColumnName]);
                        barCodeCondition2 = MDVUtility.ToStr(dsLabOrder.OS_LabOrder.Rows[0][dsLabOrder.OS_LabOrder.OrderNoColumn.ColumnName]);
                        barCodeCondition3 = MDVUtility.ToStr(dsPatient_BarCode.Patients.Rows[0][dsPatient_BarCode.Patients.LastNameColumn.ColumnName]);
                        barCodeCondition4 = MDVUtility.ToStr(dsPatient_BarCode.Patients.Rows[0][dsPatient_BarCode.Patients.FirstNameColumn.ColumnName]);
                    }

                    barCodeCondition3 = barCodeCondition3[0].ToString();
                    barCodeCondition4 = barCodeCondition4[0].ToString();
                    strBarcode.Append(barCodeCondition1);
                    strBarcode.Append(",");
                    strBarcode.Append(barCodeCondition2);
                    strBarcode.Append(",");
                    strBarcode.Append(barCodeCondition3);
                    strBarcode.Append(" ");
                    strBarcode.Append(barCodeCondition4);
                }

                img = b.GenerateBarcodeImage(240, 64, strBarcode.ToString());
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(img, System.Drawing.Imaging.ImageFormat.Png);
                pic.SpacingAfter = 10;

                // -------------------------------------------------------------------------------------------------------- //

                PdfPTable ReportHeaderBarCode = new PdfPTable(1);
                ReportHeaderBarCode.TotalWidth = 190f;
                ReportHeaderBarCode.LockedWidth = true;
                ReportHeaderBarCode.HorizontalAlignment = Element.ALIGN_RIGHT;
                ReportHeaderBarCode.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell lineCell = new PdfPCell();

                if (BarCodeHtmlText != string.Empty && BarCodeHtmlText.ToLower() != "false" && isLabOrder)
                    lineCell.AddElement(pic);
                else
                    lineCell.AddElement(new Chunk());

                lineCell.Border = Rectangle.NO_BORDER;
                lineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                ReportHeaderBarCode.AddCell(lineCell);

                PdfPTable ReportHeaderTable = new PdfPTable(2);
                ReportHeaderTable.TotalWidth = 575f;
                ReportHeaderTable.SpacingBefore = 10f;
                ReportHeaderTable.LockedWidth = true;
                ReportHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                MemoryStream ms = new MemoryStream();
                byte[] bytes;
                using (FileStream file = new FileStream(System.Web.HttpContext.Current.Server.MapPath(@"~\content\images\SHS-nav-logo.png"), FileMode.Open, System.IO.FileAccess.Read))
                {
                    bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);
                }

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(bytes, false);
                logo.ScalePercent(59f);
                logo.ScaleAbsoluteHeight(100);
                logo.ScaleAbsoluteWidth(150);

                PdfPCell logoCell = new PdfPCell();
                logoCell.AddElement(logo);
                logoCell.Border = Rectangle.NO_BORDER;
                logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                ReportHeaderTable.AddCell(logoCell);

                #endregion

                #region Header Table

                ReportHeaderTable.AddCell(ReportHeaderBarCode);
                ReportHeaderTable.AddCell(headerSovreignAddress);

                string clientDetails = "";
                clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.NameColumn.ColumnName] + "\n");
                clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.AddressColumn.ColumnName], "\n");
                clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.CityColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.CityColumn.ColumnName] + ", ");
                clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.StateColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.StateColumn.ColumnName] + ", ");
                clientDetails = string.Concat(clientDetails, string.IsNullOrEmpty(dsLab.Lab.Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName].ToString()) ? "" : dsLab.Lab.Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName] + "\n");
                clientDetails = string.Concat(clientDetails, dsLab.Lab.Rows[0][dsLab.Lab.PhoneNoColumn.ColumnName]);

                Paragraph p = new Paragraph(clientDetails, bodyFont);
                PdfPCell clientAccountNumber = new PdfPCell();
                p.Alignment = Element.ALIGN_RIGHT;
                clientAccountNumber.AddElement(p);
                clientAccountNumber.Border = Rectangle.NO_BORDER;
                clientAccountNumber.HorizontalAlignment = Element.ALIGN_RIGHT;

                ReportHeaderTable.AddCell(clientAccountNumber);
                ReportHeaderTable.SpacingAfter = 3f;

                BaseColor myColor1 = WebColors.GetRGBColor("#66B2FF");
                LineSeparator line2 = new LineSeparator(1f, 100f, myColor1, Element.ALIGN_CENTER, -1);
                pdfDocument.Add(ReportHeaderTable);
                pdfDocument.Add(line2);

                #endregion
            }
            #endregion
        }
        private PdfPTable SetLabPatient(long patientId, string patientName, BaseColor fontColour, Font bodyFont, Font componentHeadingFont, Font PFont, string AccountNumber = "")
        {
            PdfPTable patientTable = new PdfPTable(1);
            patientTable.DefaultCell.Border = Rectangle.NO_BORDER;
            string PatientText = GetPatientText(patientId);
            string[] PatientColumns = PatientText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
            patientName = PatientColumns[0];

            //var x2 = new MyPdfPageEventHelpPageNo(patientName, bodyFont, MDVUtility.ToStr(dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.OrderNoColumn.ColumnName]), componentHeadingFont);
            //pdf.Writer.PageEvent = x2;
            patientTable.AddCell(new Paragraph("Patient", PFont));
            bool isInserted = false;
            foreach (string PatientColumn in PatientColumns)
            {
                patientTable.AddCell(new Paragraph(PatientColumn, bodyFont));
                if (isInserted == false && AccountNumber != "")
                {
                    patientTable.AddCell(new Paragraph(AccountNumber, bodyFont));
                }
                isInserted = true;
            }

            return patientTable;
        }
        private PdfPTable SetLabProvider(long providerId, Font PFont, Font bodyFont)
        {
            PdfPTable providerTable = new PdfPTable(1);
            providerTable.DefaultCell.Border = Rectangle.NO_BORDER;
            string ProviderText = GetProviderText(providerId);
            string[] ProviderColumns = ProviderText.Split(new string[] { "<br/>" }, StringSplitOptions.None);

            foreach (string providerColumn in ProviderColumns)
            {
                Paragraph providerParagraph;
                if (providerColumn == "Provider")
                    providerParagraph = new Paragraph(providerColumn, PFont);
                else
                    providerParagraph = new Paragraph(providerColumn, bodyFont);

                providerParagraph.Alignment = Element.ALIGN_RIGHT;
                PdfPCell Providercell = new PdfPCell();
                Providercell.AddElement(providerParagraph);
                Providercell.Border = Rectangle.NO_BORDER;
                Providercell.HorizontalAlignment = Element.ALIGN_RIGHT;
                providerTable.AddCell(Providercell);
            }
            return providerTable;
        }
        private PdfPTable setLabResultSample(DSOS_LabOrder dsLabOrder, DSLabResult dsLabResult, Font componentHeaderFont, Font bodyFont)
        {
            PdfPTable SampleTable = new PdfPTable(4);
            float[] SampleWidths = new float[] { 4f, 8f, 4f, 4f };
            SampleTable.SetWidths(SampleWidths);
            SampleTable.TotalWidth = 575f;
            SampleTable.LockedWidth = true;
            SampleTable.HorizontalAlignment = Element.ALIGN_CENTER;
            SampleTable.DefaultCell.Border = Rectangle.NO_BORDER;

            Paragraph spec = new Paragraph("Specimen ID:", componentHeaderFont);
            SampleTable.AddCell(spec);
            SampleTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabResultSpecimen.TableName].Rows[0][dsLabResult.LabResultSpecimen.ExternalSpecimenIdColumn.ColumnName])), bodyFont));
            SampleTable.AddCell(new Paragraph("Date of report:", componentHeaderFont));
            var dateTime = MDVUtility.ToDateTime(dsLabResult.Tables[dsLabResult.LabOrderResultDetail.TableName].Rows[0][dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName].ToString());
            var date = dateTime.ToShortDateString();

            SampleTable.AddCell(new Paragraph(date.ToString(), bodyFont));
            SampleTable.AddCell(new Paragraph("Collection Date & Time:\n", componentHeaderFont));
            SampleTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dsLabResult.Tables[dsLabOrder.OS_LabOrder.TableName].Rows[0][dsLabOrder.OS_LabOrder.OrderDateColumn.ColumnName]).ToShortDateString() + ' ' + dsLabResult.Tables[dsLabOrder.OS_LabOrder.TableName].Rows[0][dsLabOrder.OS_LabOrder.OrderTimeColumn.ColumnName].ToString(), bodyFont));

            SampleTable.AddCell(new Paragraph("Received Date & Time:", componentHeaderFont));
            SampleTable.AddCell(new Paragraph(dsLabResult.Tables[dsLabResult.LabResultSpecimen.TableName].Rows[0][dsLabResult.LabResultSpecimen.CollectionDateTimeColumn.ColumnName].ToString(), bodyFont));

            SampleTable.AddCell(new Paragraph(string.Empty));
            SampleTable.AddCell(new Paragraph(string.Empty));
            return SampleTable;

        }
        private void setLabResultOrderInformation(Document pdfDocument, DSLabResult dsLabResult, DSOS_LabOrder dsLabOrder, Font componentHeaderFont, Font componentHeadingFont, Font bodyFont)
        {
            Paragraph Lab_OrderNumber_Facility_Assigne_Heading = new Paragraph("Order Information \n".ToString(), componentHeadingFont);

            if (dsLabResult.Tables[dsLabOrder.OS_LabOrder.TableName].Rows.Count > 0)
            {
                pdfDocument.Add(Lab_OrderNumber_Facility_Assigne_Heading);

                PdfPTable Lab_OrderNumber_Facility_Assigne_Table = new PdfPTable(4);
                float[] orderWidths = new float[] { 4f, 8f, 4f, 4f };
                Lab_OrderNumber_Facility_Assigne_Table.SetWidths(orderWidths);
                Lab_OrderNumber_Facility_Assigne_Table.TotalWidth = 575f;
                Lab_OrderNumber_Facility_Assigne_Table.LockedWidth = true;
                Lab_OrderNumber_Facility_Assigne_Table.HorizontalAlignment = Element.ALIGN_CENTER;
                Lab_OrderNumber_Facility_Assigne_Table.DefaultCell.Border = Rectangle.NO_BORDER;

                foreach (DSOS_LabOrder.OS_LabOrderRow dr in dsLabResult.Tables[dsLabOrder.OS_LabOrder.TableName].Rows)
                {
                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph("Laboratory:", componentHeaderFont));
                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.LabNameColumn.ColumnName])), bodyFont));

                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph("Order Number:", componentHeaderFont));
                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.OrderNoColumn.ColumnName])), bodyFont));

                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph("Facility:", componentHeaderFont));
                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrder.FacilityColumn.ColumnName])), bodyFont));
                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph(string.Empty));
                    Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph(string.Empty));

                    if (!string.IsNullOrWhiteSpace(HttpUtility.HtmlDecode(MDVUtility.ToStr(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.AssignedToColumn.ColumnName]))))
                    {
                        Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph("Assignee:", componentHeaderFont));
                        Lab_OrderNumber_Facility_Assigne_Table.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.AssignedToColumn.ColumnName])), bodyFont));
                    }
                }
                pdfDocument.Add(Lab_OrderNumber_Facility_Assigne_Table);
            }
            else
            {
                Lab_OrderNumber_Facility_Assigne_Heading.SpacingBefore = Lab_OrderNumber_Facility_Assigne_Heading.SpacingAfter = 5;
                pdfDocument.Add(Lab_OrderNumber_Facility_Assigne_Heading);

                PdfPTable Lab_OrderNumber_Facility_Assigne_Table = new PdfPTable(5);
                float[] widths = new float[] { 1f, 8f, 8f, 8f, 8f };
                Lab_OrderNumber_Facility_Assigne_Table.SetWidths(widths);
                Lab_OrderNumber_Facility_Assigne_Table.TotalWidth = 575f;
                Lab_OrderNumber_Facility_Assigne_Table.LockedWidth = true;
                Lab_OrderNumber_Facility_Assigne_Table.DefaultCell.Border = Rectangle.NO_BORDER;
                Lab_OrderNumber_Facility_Assigne_Table.AddCell(string.Empty);
                Lab_OrderNumber_Facility_Assigne_Table.AddCell(string.Empty);
                pdfDocument.Add(Lab_OrderNumber_Facility_Assigne_Table);
                Paragraph orderInformation = new Paragraph("No Order Information Found".ToString());
                orderInformation.Alignment = Element.ALIGN_CENTER;
                pdfDocument.Add(orderInformation);
            }
        }
        private void setLabresultInformation(Document pdfDocument, DSOS_LabOrder dsLabOrder, DSLabResult dsLabResult, Font componentHeadingFont, Font componentHeaderFont, Font componentGridHeaderFont, Font bodyFont, Font uomFont, Font uomFontOrange, Font gridbodyFont, Font bodyFontSmall)
        {
            if (dsLabResult.Tables[dsLabResult.LabOrderResultDetail.TableName].Rows.Count > 0)
            {
                PdfPTable resultHeader = new PdfPTable(4);
                float[] resultHeaderwidths = new float[] { 4f, 8f, 4f, 4f };
                resultHeader.SetWidths(resultHeaderwidths);
                resultHeader.TotalWidth = 575f;
                resultHeader.SpacingBefore = 5f;
                resultHeader.LockedWidth = true;
                resultHeader.DefaultCell.Border = Rectangle.NO_BORDER;

                Paragraph test_Heading1 = new Paragraph("Result Information".ToString(), componentHeadingFont);
                test_Heading1.SpacingBefore = 5f;
                test_Heading1.SpacingAfter = 10f;
                resultHeader.AddCell(test_Heading1);
                resultHeader.AddCell(string.Empty);

                bool flagCorrected = false;
                for (int i = 0; i < dsLabResult.LabOrderResultDetail.Rows.Count; i++)
                    if (dsLabResult.LabOrderResultDetail.Rows[i][dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName].ToString() == "Correction")
                        flagCorrected = true;

                if (!flagCorrected)
                {
                    Paragraph test_Heading2 = new Paragraph("Status: " + dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.StatusColumn.ColumnName].ToString(), componentHeaderFont);
                    test_Heading2.Alignment = Element.ALIGN_LEFT;
                    resultHeader.AddCell(test_Heading2);
                }
                else
                {
                    Paragraph test_Heading2 = new Paragraph("Status: Revised", componentHeaderFont);
                    test_Heading2.Alignment = Element.ALIGN_LEFT;
                    resultHeader.AddCell(test_Heading2);
                }

                resultHeader.AddCell(string.Empty);
                pdfDocument.Add(resultHeader);

                PdfPTable testTable = new PdfPTable(6);
                float[] widths = new float[] { 3f, 9f, 2f, 2f, 2f, 2.5f };
                testTable.SetWidths(widths);
                testTable.TotalWidth = 575f;
                testTable.LockedWidth = true;
                testTable.DefaultCell.Border = Rectangle.NO_BORDER;
                testTable.AddCell(new Paragraph("Date & Time", componentGridHeaderFont));
                testTable.AddCell(new Paragraph("Observation", componentGridHeaderFont));
                testTable.AddCell(new Paragraph("Result", componentGridHeaderFont));
                testTable.AddCell(new Paragraph("UoM", componentGridHeaderFont));
                testTable.AddCell(new Paragraph("Flag", componentGridHeaderFont));
                testTable.AddCell(new Paragraph("Range", componentGridHeaderFont));


                foreach (DSOS_LabOrder.OS_LabOrderTestRow dr in dsLabResult.Tables[dsLabOrder.OS_LabOrderTest.TableName].Rows)
                {
                    testTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dr[dsLabOrder.OS_LabOrderTest.TestDateColumn.ColumnName]).ToShortDateString() + ' ' + dr[dsLabOrder.OS_LabOrderTest.TestTimeColumn.ColumnName].ToString(), gridbodyFont));
                    testTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsLabOrder.OS_LabOrderTest.CPTCodeColumn.ColumnName] + " " + dr[dsLabOrder.OS_LabOrderTest.CPTCodeDescriptionColumn.ColumnName])), gridbodyFont));

                    testTable.AddCell(string.Empty);
                    testTable.AddCell(string.Empty);
                    testTable.AddCell(string.Empty);
                    testTable.AddCell(string.Empty);

                    DSLabResult.LabOrderResultDetailRow[] labResultDetail = (DSLabResult.LabOrderResultDetailRow[])dsLabResult.LabOrderResultDetail.Select(dsLabResult.LabOrderResultDetail.CPTCodeColumn.ColumnName + '=' + MDVUtility.ToLINQFormatString(dr[dsLabOrder.OS_LabOrderTest.CPTCodeColumn.ColumnName]));
                    foreach (DSLabResult.LabOrderResultDetailRow drDetail in labResultDetail)
                    {
                        Font temp = null;
                        testTable.AddCell(new Paragraph(MDVUtility.ToDateTime(drDetail[dsLabResult.LabOrderResultDetail.ObservationDateColumn.ColumnName]).ToString("MM/dd/yyyy HH:mm"), bodyFont));
                        if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]) == "Normal")
                        {
                            if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + " - (Corrected)", bodyFont));
                                temp = bodyFont;
                            }
                            else
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), bodyFont));
                                temp = bodyFont;
                            }
                        }

                        else if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]) == "High")
                        {
                            if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + " - (Corrected)", uomFont));
                                temp = uomFont;
                            }
                            else
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), uomFont));
                                temp = uomFont;
                            }
                        }
                        else if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]).IndexOf("Abnormal") > -1)
                        {
                            if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + " - (Corrected)", uomFont));
                                temp = uomFont;
                            }
                            else
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), uomFont));
                                temp = uomFont;
                            }
                        }
                        else if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]) == "Low")
                        {
                            if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + " - (Corrected)", uomFontOrange));
                                temp = uomFontOrange;
                            }
                            else
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), uomFontOrange));
                                temp = uomFontOrange;
                            }
                        }
                        else if (string.IsNullOrEmpty(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName])))
                        {
                            if (MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.StatusColumn.ColumnName]) == "Correction")
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()) + " - (Corrected)", bodyFont));
                                temp = bodyFont;
                            }
                            else
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.LOINCColumn.ColumnName].ToString() + ' ' + drDetail[dsLabResult.LabOrderResultDetail.LOINCDescriptionColumn.ColumnName].ToString()), bodyFont));
                                temp = bodyFont;
                            }
                        }

                        testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.ResultColumn.ColumnName]), temp));
                        testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.UoMColumn.ColumnName]), bodyFont));
                        testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.FlagColumn.ColumnName]), temp));
                        testTable.AddCell(new Paragraph(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.RangeColumn.ColumnName]), bodyFont));
                        if (!String.IsNullOrEmpty(MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.NTETextColumn.ColumnName])))
                        {
                            string nte = MDVUtility.ToStr(drDetail[dsLabResult.LabOrderResultDetail.NTETextColumn.ColumnName]);
                            string[] nteColumns = nte.Split(new string[] { "~" }, StringSplitOptions.None);
                            nte = "";
                            string nteTemp = "";
                            foreach (string nteColumn in nteColumns)
                            {
                                nteTemp = nteColumn.TrimStart();
                                testTable.AddCell(String.Empty);
                                testTable.AddCell(new Phrase(nteTemp, bodyFontSmall));
                                testTable.AddCell(String.Empty);
                                testTable.AddCell(String.Empty);
                                testTable.AddCell(String.Empty);
                                testTable.AddCell(String.Empty);
                            }
                        }
                    }
                }
                pdfDocument.Add(testTable);
            }
            else
            {
                Paragraph test_Heading = new Paragraph("Result Information \n".ToString(), componentHeadingFont);
                test_Heading.SpacingBefore = test_Heading.SpacingAfter = 5;
                pdfDocument.Add(test_Heading);
                PdfPTable testTable = new PdfPTable(7);
                float[] widths = new float[] { 8f, 10f, 8f, 8f, 8f, 8f, 8f };
                testTable.SetWidths(widths);
                testTable.TotalWidth = 575f;
                testTable.LockedWidth = true;
                testTable.DefaultCell.Border = Rectangle.NO_BORDER;
                testTable.AddCell(string.Empty);
                pdfDocument.Add(testTable);
                Paragraph noTest = new Paragraph("No Result Information Found".ToString());
                noTest.Alignment = Element.ALIGN_CENTER;
                pdfDocument.Add(noTest);
            }
        }
        private void setCLIA(Document pdfDocument, DSLab dsLab, Font componentHeaderFont)
        {
            //Paragraph CLIA = new Paragraph(MDVUtility.ToStr("Lab Test Performed By: "
            //+ dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.CLIANoColumn.ColumnName]
            //+ ", " + dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.NameColumn.ColumnName]
            //+ ", " + dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.AddressColumn.ColumnName]
            //+ ", " + dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.CityColumn.ColumnName]
            //+ ", " + dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.StateColumn.ColumnName]
            //+ ", " + dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName]
            //+ ", " + dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab._FillersField_2Column.ColumnName]), componentHeaderFont);
            //return CLIA;
            if (dsLab.Lab.Rows.Count > 0)
            {
                string CLIANo = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.CLIANoColumn.ColumnName]);
                CLIANo = string.IsNullOrEmpty(CLIANo) ? "" : CLIANo + ", ";

                string labName = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.NameColumn.ColumnName]);
                labName = string.IsNullOrEmpty(labName) ? "" : labName + ", ";

                string labAddress = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.AddressColumn.ColumnName]);
                labAddress = string.IsNullOrEmpty(labAddress) ? "" : labAddress + ", ";

                string labCity = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.CityColumn.ColumnName]);
                labCity = string.IsNullOrEmpty(labCity) ? "" : labCity + ", ";

                string labState = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.StateColumn.ColumnName]);
                labState = string.IsNullOrEmpty(labState) ? "" : labState + ", ";

                string labZIPCode = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab.ZIPCodeColumn.ColumnName]);
                labZIPCode = string.IsNullOrEmpty(labZIPCode) ? "" : labZIPCode + ", ";

                string labFillersField_2 = MDVUtility.ToStr(dsLab.Tables[dsLab.Lab.TableName].Rows[0][dsLab.Lab._FillersField_2Column.ColumnName]);
                labFillersField_2 = string.IsNullOrEmpty(labFillersField_2) ? "" : labFillersField_2 + ", ";

                string commaSeparatedText = CLIANo + labName + labAddress + labCity + labState + labZIPCode + labFillersField_2;

                if (!string.IsNullOrEmpty(commaSeparatedText))
                {
                    string CLIAText = "Lab Test Performed By: " + commaSeparatedText;
                    CLIAText = CLIAText.Trim().Trim(',');

                    Paragraph CLIA = new Paragraph(CLIAText, componentHeaderFont);
                    pdfDocument.Add(CLIA);
                }
            }
        }
        private PdfPTable setLabOrder_ResultFooter(Document pdfDocument)
        {
            PdfPTable footer = new PdfPTable(1);
            footer.TotalWidth = 575f;
            footer.LockedWidth = true;
            footer.HorizontalAlignment = Element.ALIGN_CENTER;
            footer.DefaultCell.Border = Rectangle.NO_BORDER;
            footer.SpacingBefore = 5f;

            PdfPCell footerCell = new PdfPCell();
            var color = System.Drawing.ColorTranslator.FromHtml("#005da9");
            footerCell.BackgroundColor = new BaseColor(color);
            color = System.Drawing.ColorTranslator.FromHtml("#fff");
            var fontcolor = new BaseColor(color);
            Font componentFooterFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, fontcolor);

            Paragraph footerPara = new Paragraph("Generated by: MDVision PMS EMR", componentFooterFont);
            footerPara.SpacingAfter = 5f;
            footerCell.AddElement(footerPara);
            footer.AddCell(footerCell);
            // pdfDocument.Add(footer);
            return footer;
        }
        #endregion
        public BLObject<byte[]> previewLabResult(long labOrderResultId, long labOrderId, long patientId, string BarCodeHtmlText, string ImagePath)
        {
            try
            {
                #region It starts with One thing I don't know why

                DSLabResult dsLabResult = new DSLabResult();
                DSPatient dsPatient = new DSPatient();
                DSOS_LabOrder dsLabOrder = new DSOS_LabOrder();
                DSProfile dsProfile = new DSProfile();
                DSLab dsLab = new DSLab();

                string FirstName = "";
                string LastName = "";
                string patientName = "";
                string patientComments = "";
                long specimenId = MDVUtility.ToInt64(null);
                var folderPath = string.Empty;
                var pngfileName = string.Empty;
                //long practiceId = MDVUtility.ToInt64(null);
                //long providerId = MDVUtility.ToInt64(null);

                #region Fonts

                var fontColour = new BaseColor(102, 178, 255);
                Font patientNameFont = FontFactory.GetFont("Arial", 12, Font.BOLD, fontColour);
                Font bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                Font bodyFontSmall = FontFactory.GetFont("Courier", 9, Font.NORMAL, BaseColor.BLACK);
                Font bodyFontSmall1 = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                Font uomFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.RED);
                Font uomFontOrange = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.ORANGE);
                Font componentHeadingFont = FontFactory.GetFont("Arial", 11, Font.BOLD, fontColour);
                Font componentHeaderFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                Font componentGridHeaderFont = FontFactory.GetFont("Arial", 10, Font.BOLD, new BaseColor(102, 178, 255));
                Font gridbodyFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
                Font bodyFontParent = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
                Font gridbodyFontTest = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLUE);
                Font PFont = FontFactory.GetFont("Arial", 12, Font.BOLD, fontColour);

                #endregion

                #endregion

                #region Fetch Routine

                dsLabOrder = new DALOS_LabOrder().FillLabOrder(labOrderId);
                dsLabOrder.Merge(new DALOS_LabOrder().LoadLabOrderTest(labOrderId, 0, patientId, "", ""));
                dsLab = new DALLab().GetLab(MDVUtility.ToLong(dsLabOrder.OS_LabOrder.Rows[0][dsLabOrder.OS_LabOrder.LabIdColumn.ColumnName]), MDVUtility.ToLong(dsLabOrder.OS_LabOrder.Rows[0][dsLabOrder.OS_LabOrder.ProviderIdColumn.ColumnName]), MDVUtility.ToLong(dsLabOrder.OS_LabOrder.Rows[0][dsLabOrder.OS_LabOrder.FacilityIdColumn.ColumnName]));
                dsLabResult = new DALLabResult().LoadLabResult(labOrderResultId, labOrderId, "", "", "", "", 0, "", "", "", "", 0, patientId, "", "1");

                if (patientId == 0)
                    if (dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows.Count > 0)
                        patientId = MDVUtility.ToInt64(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.PatientIdColumn.ColumnName]);

                dsLabResult.Merge(new DALPatient().FillPatient(patientId, "", ""));
                long practiceId = MDVUtility.ToInt64(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PracticeIdColumn.ColumnName]);
                long providerId = MDVUtility.ToInt64(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.ProviderIdColumn.ColumnName]);
                dsLabResult.Merge(new DALLabResult().loadLabResultDetail(0, MDVUtility.ToInt64(dsLabResult.LabOrderResult.Rows[0][dsLabResult.LabOrderResult.LabOrderResultIdColumn.ColumnName])));
                dsLabResult.Merge(new DALOS_LabOrder().FillLabOrder(labOrderId));
                dsLabResult.Merge(new DALOS_LabOrder().LoadLabOrderTest(labOrderId, 0, patientId, "", ""));

                int testCount = 0;
                if (dsLabResult.Tables[dsLabOrder.OS_LabOrderTest.TableName].Rows.Count > 0)
                {
                    foreach (DSOS_LabOrder.OS_LabOrderTestRow r in dsLabResult.Tables[dsLabOrder.OS_LabOrderTest.TableName].Rows)
                    {
                        if (testCount == 0)
                        {
                            dsLabResult.Merge(new DALLabResult().loadLabResultSpecimen(specimenId, MDVUtility.ToInt64(r[dsLabOrder.OS_LabOrderTest.LabOrderTestIdColumn.ColumnName])));
                            testCount++;
                        }
                    }
                }

                if (dsLabResult.Tables[dsLabOrder.OS_LabOrder.TableName].Rows.Count > 0)
                {
                    //providerId = MDVUtility.ToInt64(dsLabResult.Tables[dsLabOrder.LabOrder.TableName].Rows[0][dsLabOrder.LabOrder.ProviderIdColumn.ColumnName]);
                    //practiceId = MDVUtility.ToInt64(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PracticeIdColumn.ColumnName]);
                    FirstName = MDVUtility.ToStr(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.FirstNameColumn.ColumnName]);
                    LastName = MDVUtility.ToStr(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.LastNameColumn.ColumnName]);
                    patientComments = MDVUtility.ToStr(dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.CommentsColumn.ColumnName]);
                }
                dsLabResult.Merge(new DALPractice().LoadPractice(practiceId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));

                #endregion

                #region Document Object

                //byte[] newByteArr = null;
                //MemoryStream stream_ = new MemoryStream();
                //Document pdfDocument = new Document(PageSize.LETTER, 20, 20, 20, 35);
                //MDVUtility.PDFCreator pdf = new MDVUtility.PDFCreator(ref pdfDocument, stream_, false, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Requisition");
                //pdf.Document.Open();

                #endregion

                #region PDF Segments

                #region Header Segment

                // setLabHeader(pdfDocument, practiceId, patientId, dsLab, dsLabOrder, bodyFont, BarCodeHtmlText, false);

                #endregion

                //#region Patient/Provider Segment

                //try
                //{
                //    PdfPTable ReportPatientProvider = new PdfPTable(2);
                //    ReportPatientProvider.TotalWidth = 575f;
                //    ReportPatientProvider.SpacingBefore = 3f;
                //    ReportPatientProvider.SpacingAfter = 8f;
                //    ReportPatientProvider.LockedWidth = true;
                //    ReportPatientProvider.DefaultCell.Border = Rectangle.NO_BORDER;

                //    //  ReportPatientProvider.AddCell(SetLabPatient(pdf, patientId, patientName, fontColour, bodyFont, componentHeadingFont, PFont, dsLabOrder));
                //    ReportPatientProvider.AddCell(SetLabProvider(providerId, PFont, bodyFont));
                //    pdfDocument.Add(ReportPatientProvider);
                //}
                //catch (Exception ex)
                //{
                //    MDVLogger.BLLErrorLog("BLLAdminClinical::LabOrderPreview, Patient or Provider Segment", ex);
                //}

                //#region Line Seperator and Heading

                //BaseColor myColor = WebColors.GetRGBColor("#66B2FF");
                //LineSeparator Borderline = new LineSeparator(1f, 100f, myColor, Element.ALIGN_CENTER, 4);
                //pdfDocument.Add(Borderline);

                //Paragraph faceSheetHeading = new Paragraph("Lab Result".ToString(), patientNameFont);
                //pdfDocument.Add(faceSheetHeading);

                //#endregion

                //#endregion





                #region Report Header
                DSReportHeader.ReportHeaderTagsDataTable dtReportHeaderTags;

                bool IsReportHeaderApplied = false;
                var dsReportHeader = new MDVision.DataAccess.DAL.ReportHeader.DALReportHeader().getReportHeaderTagsValue(patientId, 0, -1, "Results");
                IsReportHeaderApplied = dsReportHeader.ReportHeaderTags.Count > 0;
                dtReportHeaderTags = dsReportHeader.ReportHeaderTags;
                if (IsReportHeaderApplied &&
                     (
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["PatientText"].ToString()) ||
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["ProviderText"].ToString()) ||
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["PracticeText"].ToString()) ||
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["HeaderLogo"].ToString())
                            )
                    )
                {
                    IsReportHeaderApplied = true;
                }
                else
                {
                    IsReportHeaderApplied = false;
                }
                // return new BLObject<DSReportHeader>(dsReportHeader);

                byte[] newByteArr = null;
                using (MemoryStream stream_ = new MemoryStream())
                {
                    // Heading Font Style
                    //var fontColour = new BaseColor(102, 178, 255);
                    //iTextSharp.text.Font patientNameFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, fontColour);
                    //iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                    //iTextSharp.text.Font componentHeadingFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, fontColour);
                    //iTextSharp.text.Font componentHeaderFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
                    bool IsFooterExist = false;
                    string FooterGeneratedBy = "";
                    PdfPTable ReportHeaderTable = new PdfPTable(2);
                    PdfPTable footer = new PdfPTable(1);
                    float bottomMargin = 22;
                    float topMargin = 20;
                    PdfPTable patientTable = new PdfPTable(2);
                    Document pdfDocument = new Document(PageSize.LETTER, 20, 20, topMargin, bottomMargin);
                    if (IsReportHeaderApplied)
                    {
                        DSReportHeader.ReportHeaderTagsRow drReportHeader = (DSReportHeader.ReportHeaderTagsRow)dtReportHeaderTags.Rows[0];
                        #region Report Header

                        try
                        {
                            //------------------------------------  DSReportHeader.ReportHeaderTags dtReportHeaderTags =
                            if (IsReportHeaderApplied)
                            {
                                ReportHeaderTable.TotalWidth = 630f;
                                ReportHeaderTable.LockedWidth = true;
                                ReportHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                                IsFooterExist = drReportHeader.Field<string>("FooterText") != null;
                                if (IsFooterExist)
                                {
                                    FooterGeneratedBy = drReportHeader.FooterText;
                                    footer = setLabOrder_ResultFooterPDF(FooterGeneratedBy);
                                }
                                #region Header Logo
                                if (drReportHeader.Field<string>("HeaderLogo") != null)
                                {
                                    PdfPTable headerTable = new PdfPTable(1);
                                    headerTable.TotalWidth = 575f;
                                    headerTable.LockedWidth = true;
                                    headerTable.DefaultCell.Border = Rectangle.NO_BORDER;

                                    Byte[] buffer = Convert.FromBase64String(drReportHeader.HeaderLogo.Split(new string[] { "base64," }, StringSplitOptions.None)[1]);
                                    var memoryStream = new MemoryStream(buffer); //new MemoryStream(buffer, offset, count);
                                    System.Drawing.Image newImagej = System.Drawing.Image.FromStream(memoryStream);

                                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(buffer, false);
                                    logo.ScalePercent(59f);
                                    logo.ScaleAbsoluteHeight(100);
                                    logo.ScaleAbsoluteWidth(150);

                                    PdfPCell cell1 = new PdfPCell();
                                    cell1.AddElement(logo);
                                    cell1.Border = Rectangle.NO_BORDER;
                                    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    ReportHeaderTable.AddCell(cell1);
                                }

                                else
                                {
                                    PdfPTable EmptyHeaderTable = new PdfPTable(1);
                                    EmptyHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                                    EmptyHeaderTable.AddCell(new Paragraph("", bodyFont));
                                    ReportHeaderTable.AddCell(EmptyHeaderTable);
                                }

                                #endregion

                                #region practice

                                PdfPTable PracticeTable = new PdfPTable(1);
                                PracticeTable.DefaultCell.Border = Rectangle.NO_BORDER;
                                //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                                PracticeTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                PracticeTable.DefaultCell.PaddingRight = 50;
                                //END Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                                if (drReportHeader.Field<string>("PracticeText") != null)
                                {
                                    string[] PracticeColumns = drReportHeader.PracticeText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                                    foreach (string PracticeColumn in PracticeColumns)
                                    {
                                        if (!string.IsNullOrEmpty(PracticeColumn) && !string.IsNullOrWhiteSpace(PracticeColumn))
                                        {
                                            PracticeTable.AddCell(new Paragraph(PracticeColumn, bodyFont));
                                        }
                                    }
                                }
                                else
                                {
                                    PracticeTable.AddCell(new Paragraph("", bodyFont));
                                }
                                ReportHeaderTable.AddCell(PracticeTable);
                                #endregion

                                #region  Patient

                                PdfPTable PatientTable = new PdfPTable(1);
                                PatientTable.DefaultCell.Border = Rectangle.NO_BORDER;

                                if (drReportHeader.Field<string>("PatientText") != null)
                                {
                                    string[] PatientColumns = drReportHeader.PatientText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                                    foreach (string PatientColumn in PatientColumns)
                                    {
                                        if (!string.IsNullOrEmpty(PatientColumn) && !string.IsNullOrWhiteSpace(PatientColumn))
                                        {
                                            PatientTable.AddCell(new Paragraph(PatientColumn, bodyFont));
                                        }
                                    }
                                }
                                else
                                {
                                    PatientTable.AddCell(new Paragraph("", bodyFont));
                                }
                                PatientTable.DefaultCell.Padding = 0f;
                                PatientTable.DefaultCell.UseAscender = true;
                                ReportHeaderTable.AddCell(PatientTable);
                                #endregion

                                #region  Provider

                                PdfPTable ProviderTable = new PdfPTable(1);
                                ProviderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                                //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                                ProviderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                ProviderTable.DefaultCell.PaddingRight = 50;
                                //End Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                                if (drReportHeader.Field<string>("ProviderText") != null)
                                {
                                    string[] ProviderColumns = drReportHeader.ProviderText.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                                    foreach (string ProviderColumn in ProviderColumns)
                                    {
                                        if (!string.IsNullOrEmpty(ProviderColumn) && !string.IsNullOrWhiteSpace(ProviderColumn))
                                        {
                                            ProviderTable.AddCell(new Paragraph(ProviderColumn, bodyFont));
                                        }
                                    }

                                }
                                else
                                {
                                    ProviderTable.AddCell(new Paragraph("", bodyFont));
                                }
                                ReportHeaderTable.AddCell(ProviderTable);
                                #endregion
                                //ReportHeaderTable.DefaultCell.Padding = -1;
                            }
                        }
                        catch (Exception ex)
                        {
                            MDVLogger.BLLErrorLog("BLLAdminClinical::getReportHeaderTagsValue", ex);
                            // return new BLObject<DSReportHeader>(null, ex.Message);
                        }


                        #endregion
                    }
                    else
                    {
                        #region Patient's Data

                        #region Header Segment
                        ReportHeaderTable = setLabHeaderPDF(practiceId, patientId, dsLab, dsLabOrder, bodyFont, "", true);

                        //if (IsReportHeaderApplied)
                        //{
                        topMargin = ReportHeaderTable.CalculateHeights() + 33;
                        bottomMargin = 52;
                        //}
                        #endregion
                        #region MD Vision Footer

                        footer = setLabOrder_ResultFooterPDF(FooterGeneratedBy);
                        // ReportHeaderTable.AddCell(footer);

                        #endregion




                        #endregion
                    }

                    if (IsReportHeaderApplied)
                    {
                        topMargin = ReportHeaderTable.CalculateHeights() + 33;
                        bottomMargin = 52;
                    }
                    pdfDocument.SetMargins(20, 20, topMargin, bottomMargin);
                    //Document pdfDocument = new Document(PageSize.LETTER, 20, 20, topMargin, bottomMargin);
                    MDVUtility.PDFCreator pdf = new MDVUtility.PDFCreator(ref pdfDocument, stream_, false, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Lab Result");
                    pdf.Writer.PageEvent = new MDVision.Common.Utilities.MDVUtility.AddFooterHeader(ReportHeaderTable, true, null, footer);


                    pdf.Document.Open();

                    BaseColor myColor1 = WebColors.GetRGBColor("#66B2FF");
                    LineSeparator line2 = new LineSeparator(1f, 100f, myColor1, iTextSharp.text.Element.ALIGN_CENTER, 0);
                    pdfDocument.Add(line2);

                    if (!IsReportHeaderApplied)
                    {
                        #region Patient/Provider Segment
                        string AccountNumber = dsLabResult.Tables[dsPatient.Patients.TableName].Rows[0]["AccountNumber"].ToString();
                        try
                        {
                            PdfPTable ReportPatientProvider = new PdfPTable(2);
                            ReportPatientProvider.TotalWidth = 575f;
                            ReportPatientProvider.SpacingBefore = 3f;
                            ReportPatientProvider.SpacingAfter = 8f;
                            ReportPatientProvider.LockedWidth = true;
                            ReportPatientProvider.DefaultCell.Border = Rectangle.NO_BORDER;

                            ReportPatientProvider.AddCell(SetLabPatient(patientId, patientName, fontColour, bodyFont, componentHeadingFont, patientNameFont, AccountNumber));
                            ReportPatientProvider.AddCell(SetLabProvider(providerId, patientNameFont, bodyFont));
                            pdfDocument.Add(ReportPatientProvider);

                        }
                        catch (Exception ex)
                        {
                            MDVLogger.BLLErrorLog("BLLAdminClinical::LabOrderPreview, Patient or Provider Segment", ex);
                        }
                        pdfDocument.Add(line2);
                    }

                        #endregion









                #endregion


                    #region Lab, OrderNumber, Facility and Assigne Information Segment

                    setLabResultOrderInformation(pdfDocument, dsLabResult, dsLabOrder, componentHeaderFont, componentHeadingFont, bodyFont);

                    #endregion

                    #region Sample Segment

                    Paragraph SampleHeading = new Paragraph("Sample", componentHeadingFont);
                    pdfDocument.Add(SampleHeading);

                    if (dsLabResult.Tables[dsLabResult.LabResultSpecimen.TableName].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dsLabResult.Tables[dsLabResult.LabResultSpecimen.TableName].Rows[0][dsLabResult.LabResultSpecimen.ExternalSpecimenIdColumn.ColumnName].ToString()))
                        {
                            pdfDocument.Add(setLabResultSample(dsLabOrder, dsLabResult, componentHeaderFont, bodyFont));
                        }
                        else
                        {
                            Paragraph noProblems = new Paragraph("No Sample Found".ToString(), bodyFont);
                            noProblems.Alignment = Element.ALIGN_CENTER;
                            pdfDocument.Add(noProblems);
                        }
                    }

                    #endregion

                    #region Result Information Segment

                    setLabresultInformation(pdfDocument, dsLabOrder, dsLabResult, componentHeadingFont, componentHeaderFont, componentGridHeaderFont, bodyFont, uomFont, uomFontOrange, gridbodyFont, bodyFontSmall);

                    #endregion

                    #region CLIA

                    setCLIA(pdfDocument, dsLab, componentHeaderFont);

                    #endregion

                #endregion

                    #region Remarks

                    Paragraph remarks_Heading = new Paragraph("Remarks \n".ToString(), componentHeadingFont);
                    remarks_Heading.SpacingBefore = remarks_Heading.SpacingAfter = 5;
                    pdfDocument.Add(remarks_Heading);
                    Paragraph remarks = new Paragraph(MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.RemarksColumn.ColumnName]), bodyFont);
                    remarks.Alignment = Element.ALIGN_LEFT;
                    pdfDocument.Add(remarks);

                    #endregion

                    #region Comments

                    Paragraph comments_Heading = new Paragraph("Comments \n".ToString(), componentHeadingFont);
                    comments_Heading.SpacingBefore = comments_Heading.SpacingAfter = 5;
                    pdfDocument.Add(comments_Heading);

                    Paragraph comments = new Paragraph(MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.CommentsColumn.ColumnName]), bodyFont);
                    comments.Alignment = Element.ALIGN_LEFT;
                    pdfDocument.Add(comments);

                    pdfDocument.Add(Chunk.NEWLINE);
                    pdfDocument.Add(Chunk.NEWLINE);

                    #endregion

                    #region e-Signed

                    string modifiedDate = MDVUtility.ToDateTime(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.ModifiedOnColumn.ColumnName]).ToLongDateString();

                    if (!string.IsNullOrEmpty(modifiedDate))
                    {
                        modifiedDate = modifiedDate + " at ";
                    }
                    string modifiedTime = MDVUtility.ToDateTime(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.ModifiedOnColumn.ColumnName]).ToLongTimeString();

                    Paragraph signedByPara = new Paragraph("e-Signed By: " + MDVUtility.ToStr(dsLabResult.Tables[dsLabResult.LabOrderResult.TableName].Rows[0][dsLabResult.LabOrderResult.ModifiedByColumn.ColumnName])
                       + " on " + modifiedDate + modifiedTime, bodyFont);
                    signedByPara.Alignment = Element.ALIGN_LEFT;
                    pdfDocument.Add(signedByPara);

                    #endregion

                    #region Footer

                    #endregion


                    #region But in the end It doesn't even matter

                    newByteArr = stream_.GetBuffer();
                    pdf.Document.Close();
                    pdf.Writer.Close();
                    pdfDocument.Close();

                    MemoryStream stream = new MemoryStream(stream_.ToArray());
                    PdfReader npdf = new PdfReader(stream);
                    MemoryStream outstream = new MemoryStream();
                    using (PdfStamper stamper = new PdfStamper(npdf, outstream, '\0', true))
                    {
                        stamper.Writer.CloseStream = false;
                        int PageCount = npdf.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                        {
                            Paragraph para = new Paragraph(String.Format("Page {0}/{1}", i, PageCount), bodyFont);
                            para.Add(setLabOrder_ResultFooter(pdfDocument));
                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, para, 555, 20, 0);
                        }
                    }
                    newByteArr = outstream.GetBuffer();
                }
                return new BLObject<byte[]>(newByteArr);

                    #endregion
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::previewLabResult", ex);
                return new BLObject<byte[]>(null, ex.Message);
            }
        }

        #endregion

        #region Lab Order AOE
        public BLObject<DSOS_LabOrder> LoadLabOrderAOE(string testCode, long LabOrderTestId)
        {
            try
            {
                DSOS_LabOrder ds = new DSOS_LabOrder();
                ds = new DALOS_LabOrder().LoadLabOrderAOE(testCode);
                if (LabOrderTestId > 0)
                {
                    DSOS_LabOrder dsAnswers = new DALOS_LabOrder().LoadLabOrderAOEAnswers(testCode, MDVUtility.ToStr(LabOrderTestId));
                    if (dsAnswers != null)
                    {
                        ds.Merge(dsAnswers);
                    }
                }
                return new BLObject<DSOS_LabOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadLabOrder", ex);
                return new BLObject<DSOS_LabOrder>(null, ex.Message);
            }
        }


        public BLObject<DSOS_LabOrder> LoadLabOrderAOEAnswer(string testCode, string testId)
        {
            try
            {
                DSOS_LabOrder ds = new DSOS_LabOrder();
                ds = new DALOS_LabOrder().LoadLabOrderAOEAnswers(testCode, testId);
                return new BLObject<DSOS_LabOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadLabOrder", ex);
                return new BLObject<DSOS_LabOrder>(null, ex.Message);
            }
        }
        public BLObject<DSOS_LabOrder> insertUpdateLabOrderAOEAnswers(DSOS_LabOrder ds)
        {
            try
            {
                ds = new DALOS_LabOrder().insertUpdateLabOrderAOEAnswers(ds);
                return new BLObject<DSOS_LabOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::insertUpdateLabOrderAOEAnswers", ex);
                return new BLObject<DSOS_LabOrder>(null, ex.Message);
            }
        }
        private string GetPatientText(long patientId)
        {
            DSPatient ds = new DSPatient();
            ds = new DALPatient().FillPatient(patientId, "", "");
            string PatientHtml = string.Empty;
            if (ds != null)
            {
                if (ds.Patients.Rows.Count > 0)
                {
                    PatientHtml = string.Concat("Patient", "<br/>");
                    PatientHtml = string.Concat(MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.LastNameColumn.ColumnName]), ", ", MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.FirstNameColumn.ColumnName]), " ", MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.MIColumn.ColumnName]));
                    PatientHtml = string.Concat(PatientHtml, "<br/>");
                    PatientHtml = string.Concat(PatientHtml, MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.AgeColumn.ColumnName]), " Y, ", MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.GenderColumn.ColumnName]), ", ");
                    PatientHtml = string.Concat(PatientHtml, "DOB: ", Convert.ToDateTime(MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.DOBColumn.ColumnName])).ToString("MM/dd/yyyy"), "<br/>");
                    PatientHtml = string.Concat(PatientHtml, "Patient ID: ", MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.AccountNumberColumn.ColumnName]), "<br/>");
                    if (!string.IsNullOrWhiteSpace(MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.HomePhoneNoColumn.ColumnName])))
                        PatientHtml = string.Concat(PatientHtml, "Ph: ", MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.HomePhoneNoColumn.ColumnName]), "<br/>");
                    PatientHtml = string.Concat(PatientHtml,
                                (ds.Patients.Rows[0][ds.Patients.Address1Column.ColumnName].ToString() == "" ? "" : (MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.Address1Column.ColumnName])).ToString() + "<br/>")
                                + (ds.Patients.Rows[0][ds.Patients.Address2Column.ColumnName].ToString() == "" ? "" : (MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.Address2Column.ColumnName])).ToString() + "<br/>")
                                + (ds.Patients.Rows[0][ds.Patients.CityColumn.ColumnName].ToString() == "" ? "" : (MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.CityColumn.ColumnName])).ToString())
                                + (ds.Patients.Rows[0][ds.Patients.StateColumn.ColumnName].ToString() == "" ? "" : (", " + MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.StateColumn.ColumnName])).ToString())
                                + (ds.Patients.Rows[0][ds.Patients.ZIPCodeColumn.ColumnName].ToString() == "" ? "" : (", " + MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.ZIPCodeColumn.ColumnName])).ToString()), "<br/>");

                }
            }
            return PatientHtml;

        }
        private string GetProviderText(long providerId)
        {

            DSProfile ds = new DALProvider().LoadProvider(providerId, "", "", "", "", "", "", "", 1, 2000);
            string PatientHtml = string.Empty;
            if (ds != null)
            {
                if (ds.Provider.Rows.Count > 0)
                {
                    DataRow drProvider = ds.Tables[ds.Provider.TableName].Rows[ds.Tables[ds.Provider.TableName].Rows.Count - 1];
                    var providerFullName = MDVUtility.ToStr(drProvider[ds.Provider.LastNameColumn.ColumnName]) + ", " + MDVUtility.ToStr(drProvider[ds.Provider.FirstNameColumn.ColumnName]);
                    var providerNPI = MDVUtility.ToStr(drProvider[ds.Provider.NPIColumn.ColumnName]);
                    var providerOfficeAddress = MDVUtility.ToStr(drProvider[ds.Provider.OfficeAddressColumn.ColumnName]);
                    var providerOfficePhone = MDVUtility.ToStr(drProvider[ds.Provider.PhoneNoColumn.ColumnName]);
                    PatientHtml = string.Concat(PatientHtml, "Provider", "", "<br/>");
                    PatientHtml = string.Concat(PatientHtml, "Provider Name: ", providerFullName, "<br/>");
                    PatientHtml = string.Concat(PatientHtml, "Provider NPI: ", providerNPI, "<br/>");
                    PatientHtml = string.Concat(PatientHtml, "Speciality: ", MDVUtility.ToStr(drProvider["SpecialtyName"]), "<br/>");
                }
            }
            return PatientHtml;
        }
        #endregion

        #region Radiology Order


        public BLObject<DSOS_RadiologyOrder> loadRadiologyOrdersForSoap(string radiologyOrderId, long patientId)
        {
            try
            {
                DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
                ds = new DALOS_RadiologyOrder().loadRadiologyOrdersForSoap(radiologyOrderId, patientId);

                return new BLObject<DSOS_RadiologyOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::loadRadiologyOrdersForSoap", ex);
                return new BLObject<DSOS_RadiologyOrder>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 17/03/2016
        //OverView: Method to Insert/Update RadiologyOrder
        public BLObject<DSOS_RadiologyOrder> InsertUpdateRadiologyOrder(DSOS_RadiologyOrder ds)
        {
            try
            {
                ds = new DALOS_RadiologyOrder().InsertUpdateRadiologyOrder(ds);
                return new BLObject<DSOS_RadiologyOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::InsertUpdateRadiologyOrder", ex);
                return new BLObject<DSOS_RadiologyOrder>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 17/03/2016
        //OverView: Method to Load Radiology Order
        public BLObject<DSOS_RadiologyOrder> LoadRadiologyOrder(long orderSetId, string pageNumber, string rowsPerPage)
        {
            try
            {
                DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
                //Start 21-03-2016 Humaira Yousaf
                ds = new DALOS_RadiologyOrder().LoadRadiologyOrder(orderSetId, pageNumber, rowsPerPage);
                //End 21-03-2016 Humaira Yousaf
                return new BLObject<DSOS_RadiologyOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadRadiologyOrder", ex);
                return new BLObject<DSOS_RadiologyOrder>(null, ex.Message);
            }
        }

        // Author:  Abid Ali
        // Created Date: 17/03/2016
        //OverView: Method to Delete Radiology Order
        public BLObject<string> DeleteRadiologyOrder(string radiologyOrderId)
        {
            try
            {
                radiologyOrderId = new DALOS_RadiologyOrder().DeleteRadiologyOrder(Convert.ToInt64(radiologyOrderId));
                return new BLObject<string>(radiologyOrderId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::DeleteRadiologyOrder", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        // Author:  Farooq Ahmad
        // Created Date: 18/03/2016
        //OverView: Method to Insert/Update RadiologyOrder Problems
        public BLObject<DSOS_RadiologyOrder> InsertUpdateRadiologyOrderProblems(DSOS_RadiologyOrder ds)
        {
            try
            {
                ds = new DALOS_RadiologyOrder().insertUpdateRadiologyOrderProblems(ds);
                return new BLObject<DSOS_RadiologyOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::InsertUpdateRadiologyOrderProblems", ex);
                return new BLObject<DSOS_RadiologyOrder>(null, ex.Message);
            }
        }

        // Author:  Farooq Ahmad
        // Created Date: 18/03/2016
        //OverView: Method to Load RadiologyOrder Problems
        public BLObject<DSOS_RadiologyOrder> LoadRadiologyOrderProblems(long radiologyOrderId, long patientId, int pageNumber, int rowsPerPage)
        {
            try
            {
                DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
                ds = new DALOS_RadiologyOrder().loadRadiologyOrderProblems(radiologyOrderId, patientId, pageNumber, rowsPerPage);
                return new BLObject<DSOS_RadiologyOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadRadiologyOrderProblems", ex);
                return new BLObject<DSOS_RadiologyOrder>(null, ex.Message);
            }
        }

        // Author:  Farooq Ahmad
        // Created Date: 18/03/2016
        //OverView: Method to Delete RadiologyOrder Problems
        public BLObject<string> DeleteRadiologyOrderProblems(string radiologyOrderId)
        {
            try
            {
                radiologyOrderId = new DALOS_RadiologyOrder().deleteRadiologyOrderProblems(Convert.ToInt64(radiologyOrderId));
                return new BLObject<string>(radiologyOrderId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::DeleteRadiologyOrderProblems", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Method Name: attachRadiologyOrderWithNotes
        /// Author : Ahmad Raza
        /// Description: Attaching Radiology Order with Notes
        /// </summary>
        /// <param name="radiologyOrderId"></param>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public BLObject<DSOS_RadiologyOrder> attachRadiologyOrderWithNotes(string radiologyOrderId, long noteId)
        {
            try
            {
                DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
                ds = new DALOS_RadiologyOrder().attachRadiologyOrderWithNotes(radiologyOrderId, noteId);
                return new BLObject<DSOS_RadiologyOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::attachRadiologyOrderWithNotes", ex);
                return new BLObject<DSOS_RadiologyOrder>(null, ex.Message);
            }
        }

        /// <summary>
        ///   Method Name: detachRadiologyOrderFromNotes
        ///   Author : Ahmad Raza
        ///   Description: Detaching Radiology Order from Notes
        /// </summary>
        /// <param name="radiologyOrderId"></param>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public BLObject<string> detachRadiologyOrderFromNotes(string radiologyOrderId, long noteId)
        {
            try
            {
                var msgRadiologyOrderId = new DALOS_RadiologyOrder().detachRadiologyOrderFromNotes(radiologyOrderId, noteId);
                return new BLObject<string>(msgRadiologyOrderId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::detachRadiologyOrderFromNotes", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Module Name: previewRadiologyOrder
        /// Author: Humaira Yousaf
        /// Created Date: 23-03-2016
        /// Description: Creates PDF to view Radiology Order
        /// </summary>
        /// <param name="consultationOrderId" type="long">radiologyId</param>
        /// <param name="PatientId" type="long">PatientId</param>
        public BLObject<byte[]> previewRadiologyOrder(long radiologyId, long patientId)
        {
            try
            {
                //Start 04-05-2016 Humaira Yousaf for formatting and other changes
                DSOS_RadiologyOrder dsRadiologyOrder = new DSOS_RadiologyOrder();
                DSPatient dsPatient = new DSPatient();
                DSProfile dsProfile = new DSProfile();
                //Start 14-05-2016 Edit By Humaira Yousaf Bug# EMR-1003
                dsRadiologyOrder = new DALOS_RadiologyOrder().FillRadiologyOrder(radiologyId);
                //End 14-05-2016 Edit By Humaira Yousaf Bug# EMR-1003
                dsRadiologyOrder.Merge(new DALPatient().FillPatient(patientId, "", ""));
                dsRadiologyOrder.Merge(new DALOS_RadiologyOrder().loadRadiologyOrderProblems(radiologyId, patientId));
                dsRadiologyOrder.Merge(new DALOS_RadiologyOrder().LoadRadiologyOrderTest(radiologyId, "", ""));
                long practiceId = MDVUtility.ToInt64(dsRadiologyOrder.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PracticeIdColumn.ColumnName]);
                long providerId = MDVUtility.ToInt64(dsRadiologyOrder.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.ProviderIdColumn.ColumnName]);
                dsRadiologyOrder.Merge(new DALPractice().LoadPractice(practiceId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));

                string patientName = "";
                //long providerId = MDVUtility.ToInt64(null);


                DSReportHeader.ReportHeaderTagsDataTable dtReportHeaderTags;

                bool IsReportHeaderApplied = false;
                var dsReportHeader = new MDVision.DataAccess.DAL.ReportHeader.DALReportHeader().getReportHeaderTagsValue(patientId, 0, -1, "Others");
                IsReportHeaderApplied = dsReportHeader.ReportHeaderTags.Count > 0;
                dtReportHeaderTags = dsReportHeader.ReportHeaderTags;
                if (IsReportHeaderApplied &&
                     (
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["PatientText"].ToString()) ||
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["ProviderText"].ToString()) ||
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["PracticeText"].ToString()) ||
                            !string.IsNullOrEmpty(dsReportHeader.ReportHeaderTags[0]["HeaderLogo"].ToString())
                            )
                    )
                {
                    IsReportHeaderApplied = true;
                }
                else
                {
                    IsReportHeaderApplied = false;
                }
                // return new BLObject<DSReportHeader>(dsReportHeader);

                byte[] newByteArr = null;
                using (MemoryStream stream_ = new MemoryStream())
                {
                    bool IsFooterExist = false;
                    string FooterGeneratedBy = "";
                    PdfPTable ReportHeaderTable = new PdfPTable(2);
                    PdfPTable footer = new PdfPTable(1);
                    float bottomMargin = 22;
                    float topMargin = 20;
                    PdfPTable patientTable = new PdfPTable(2);
                    Document pdfDocument = new Document(PageSize.LETTER, 20, 20, topMargin, bottomMargin);

                    // Heading Font Style
                    var fontColour = new BaseColor(102, 178, 255);
                    iTextSharp.text.Font patientNameFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, fontColour);
                    iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                    iTextSharp.text.Font componentHeadingFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, fontColour);
                    iTextSharp.text.Font componentHeaderFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
                    iTextSharp.text.Font gridbodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

                    if (IsReportHeaderApplied)
                    {
                        DSReportHeader.ReportHeaderTagsRow drReportHeader = (DSReportHeader.ReportHeaderTagsRow)dtReportHeaderTags.Rows[0];
                        #region Report Header

                        try
                        {
                            //------------------------------------  DSReportHeader.ReportHeaderTags dtReportHeaderTags =
                            if (IsReportHeaderApplied)
                            {
                                ReportHeaderTable.TotalWidth = 630f;
                                ReportHeaderTable.LockedWidth = true;
                                ReportHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                                IsFooterExist = drReportHeader.Field<string>("FooterText") != null;
                                if (IsFooterExist)
                                {
                                    FooterGeneratedBy = drReportHeader.FooterText;
                                    footer = setLabOrder_ResultFooterPDF(FooterGeneratedBy);
                                }
                                #region Header Logo
                                if (drReportHeader.Field<string>("HeaderLogo") != null)
                                {
                                    PdfPTable headerTable = new PdfPTable(1);
                                    headerTable.TotalWidth = 575f;
                                    headerTable.LockedWidth = true;
                                    headerTable.DefaultCell.Border = Rectangle.NO_BORDER;

                                    Byte[] buffer = Convert.FromBase64String(drReportHeader.HeaderLogo.Split(new string[] { "base64," }, StringSplitOptions.None)[1]);
                                    var memoryStream = new MemoryStream(buffer); //new MemoryStream(buffer, offset, count);
                                    System.Drawing.Image newImagej = System.Drawing.Image.FromStream(memoryStream);

                                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(buffer, false);
                                    logo.ScalePercent(59f);
                                    logo.ScaleAbsoluteHeight(100);
                                    logo.ScaleAbsoluteWidth(150);

                                    PdfPCell cell1 = new PdfPCell();
                                    cell1.AddElement(logo);
                                    cell1.Border = Rectangle.NO_BORDER;
                                    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    ReportHeaderTable.AddCell(cell1);
                                }

                                else
                                {
                                    PdfPTable EmptyHeaderTable = new PdfPTable(1);
                                    EmptyHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                                    EmptyHeaderTable.AddCell(new Paragraph("", bodyFont));
                                    ReportHeaderTable.AddCell(EmptyHeaderTable);
                                }

                                #endregion

                                #region practice

                                PdfPTable PracticeTable = new PdfPTable(1);
                                PracticeTable.DefaultCell.Border = Rectangle.NO_BORDER;
                                //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                                PracticeTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                PracticeTable.DefaultCell.PaddingRight = 50;
                                //END Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                                if (drReportHeader.Field<string>("PracticeText") != null)
                                {
                                    string[] PracticeColumns = drReportHeader.PracticeText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                                    foreach (string PracticeColumn in PracticeColumns)
                                    {
                                        if (!string.IsNullOrEmpty(PracticeColumn) && !string.IsNullOrWhiteSpace(PracticeColumn))
                                        {
                                            PracticeTable.AddCell(new Paragraph(PracticeColumn, bodyFont));
                                        }
                                    }
                                }
                                else
                                {
                                    PracticeTable.AddCell(new Paragraph("", bodyFont));
                                }
                                ReportHeaderTable.AddCell(PracticeTable);
                                #endregion

                                #region  Patient

                                PdfPTable PatientTable = new PdfPTable(1);
                                PatientTable.DefaultCell.Border = Rectangle.NO_BORDER;

                                if (drReportHeader.Field<string>("PatientText") != null)
                                {
                                    string[] PatientColumns = drReportHeader.PatientText.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                                    foreach (string PatientColumn in PatientColumns)
                                    {
                                        if (!string.IsNullOrEmpty(PatientColumn) && !string.IsNullOrWhiteSpace(PatientColumn))
                                        {
                                            PatientTable.AddCell(new Paragraph(PatientColumn, bodyFont));
                                        }
                                    }
                                }
                                else
                                {
                                    PatientTable.AddCell(new Paragraph("", bodyFont));
                                }
                                PatientTable.DefaultCell.Padding = 0f;
                                PatientTable.DefaultCell.UseAscender = true;
                                ReportHeaderTable.AddCell(PatientTable);
                                #endregion

                                #region  Provider

                                PdfPTable ProviderTable = new PdfPTable(1);
                                ProviderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                                //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                                ProviderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                ProviderTable.DefaultCell.PaddingRight = 50;
                                //End Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                                if (drReportHeader.Field<string>("ProviderText") != null)
                                {
                                    string[] ProviderColumns = drReportHeader.ProviderText.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                                    foreach (string ProviderColumn in ProviderColumns)
                                    {
                                        if (!string.IsNullOrEmpty(ProviderColumn) && !string.IsNullOrWhiteSpace(ProviderColumn))
                                        {
                                            ProviderTable.AddCell(new Paragraph(ProviderColumn, bodyFont));
                                        }
                                    }

                                }
                                else
                                {
                                    ProviderTable.AddCell(new Paragraph("", bodyFont));
                                }
                                ReportHeaderTable.AddCell(ProviderTable);
                                #endregion
                                //ReportHeaderTable.DefaultCell.Padding = -1;
                            }
                        }
                        catch (Exception ex)
                        {
                            MDVLogger.BLLErrorLog("BLLAdminClinical::getReportHeaderTagsValue", ex);
                            // return new BLObject<DSReportHeader>(null, ex.Message);
                        }


                        #endregion
                    }
                    else
                    {
                        #region Patient's Data

                        #region Header Segment
                        ReportHeaderTable = setRadiologyHeaderPDF(practiceId, bodyFont);

                        //if (IsReportHeaderApplied)
                        //{
                        topMargin = ReportHeaderTable.CalculateHeights() + 33;
                        bottomMargin = 52;
                        //}
                        #endregion
                        #region MD Vision Footer

                        footer = setLabOrder_ResultFooterPDF(FooterGeneratedBy);
                        // ReportHeaderTable.AddCell(footer);

                        #endregion

                        #endregion
                    }


                    if (IsReportHeaderApplied)
                    {
                        topMargin = ReportHeaderTable.CalculateHeights() + 33;
                        bottomMargin = 52;
                    }

                    pdfDocument.SetMargins(20, 20, topMargin, bottomMargin);
                    //Document pdfDocument = new Document(PageSize.LETTER, 20, 20, topMargin, bottomMargin);
                    MDVUtility.PDFCreator pdf = new MDVUtility.PDFCreator(ref pdfDocument, stream_, false, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Lab Result");
                    pdf.Writer.PageEvent = new MDVision.Common.Utilities.MDVUtility.AddFooterHeader(ReportHeaderTable, true, null, footer);
                    pdf.Document.Open();
                    if (!IsReportHeaderApplied)
                    {
                        #region Patient/Provider Segment
                        string AccountNumber = dsRadiologyOrder.Tables[dsPatient.Patients.TableName].Rows[0]["AccountNumber"].ToString();
                        try
                        {
                            PdfPTable ReportPatientProvider = new PdfPTable(2);
                            ReportPatientProvider.TotalWidth = 575f;
                            ReportPatientProvider.SpacingBefore = 3f;
                            ReportPatientProvider.SpacingAfter = 8f;
                            ReportPatientProvider.LockedWidth = true;
                            ReportPatientProvider.DefaultCell.Border = Rectangle.NO_BORDER;

                            ReportPatientProvider.AddCell(SetLabPatient(patientId, patientName, fontColour, bodyFont, componentHeadingFont, patientNameFont, AccountNumber));
                            ReportPatientProvider.AddCell(SetLabProvider(providerId, patientNameFont, bodyFont));
                            pdfDocument.Add(ReportPatientProvider);
                        }
                        catch (Exception ex)
                        {
                            MDVLogger.BLLErrorLog("BLLAdminClinical::LabOrderPreview, Patient or Provider Segment", ex);
                        }

                    }

                        #endregion


                    //Start 28-03-2016 Humaira Yousaf
                    Paragraph req_Heading = new Paragraph("Radiology Requisition".ToString(), patientNameFont);
                    //End 28-03-2016 Humaira Yousaf
                    LineSeparator line = new LineSeparator(1f, 100f, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -1);
                    line.Offset = 1;
                    pdfDocument.Add(new Chunk(line));
                    req_Heading.SpacingAfter = -12;
                    req_Heading.SpacingBefore = -5;
                    pdfDocument.Add(req_Heading);
                    Chunk c = new Chunk(line);

                    pdfDocument.Add(new Chunk(line));
                    pdfDocument.Add(Chunk.NEWLINE);

                    #region Order Information

                    // Start Append Order Information
                    if (dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows.Count > 0)
                    {
                        Paragraph order_Heading = new Paragraph("Order Information \n".ToString(), componentHeadingFont);
                        order_Heading.SpacingBefore = order_Heading.SpacingAfter = 5;

                        pdfDocument.Add(order_Heading);


                        PdfPTable orderTable = new PdfPTable(4);
                        float[] widths = new float[] { 8f, 8f, 8f, 8f };
                        orderTable.SetWidths(widths);
                        orderTable.TotalWidth = 575f;
                        orderTable.LockedWidth = true;
                        orderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                        foreach (DSOS_RadiologyOrder.OS_RadiologyOrderRow dr in dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows)
                        {
                            orderTable.AddCell(new Paragraph("Laboratory:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.LabNameColumn.ColumnName])), bodyFont));
                            orderTable.AddCell(new Paragraph("Date & Time:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dr[dsRadiologyOrder.OS_RadiologyOrder.OrderDateColumn.ColumnName]).ToShortDateString() + ' ' + dr[dsRadiologyOrder.OS_RadiologyOrder.OrderTimeColumn.ColumnName].ToString(), bodyFont));
                            orderTable.AddCell(new Paragraph("Facility:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.FacilityColumn.ColumnName])), bodyFont));
                            orderTable.AddCell(new Paragraph("Order Number:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(dr[dsRadiologyOrder.OS_RadiologyOrder.OrderNoColumn.ColumnName].ToString(), bodyFont));
                            orderTable.AddCell(new Paragraph("Provider:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.ProviderColumn.ColumnName])), bodyFont));
                            orderTable.AddCell(new Paragraph("Assignee:", componentHeaderFont));
                            orderTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrder.AssigneeNameColumn.ColumnName])), bodyFont));
                        }
                        pdfDocument.Add(orderTable);
                    }
                    else
                    {

                        Paragraph order_Heading = new Paragraph("Order Information \n".ToString(), componentHeadingFont);
                        order_Heading.SpacingBefore = order_Heading.SpacingAfter = 5;
                        pdfDocument.Add(order_Heading);

                        PdfPTable orderTable = new PdfPTable(5);
                        float[] widths = new float[] { 1f, 8f, 8f, 8f, 8f };
                        orderTable.SetWidths(widths);
                        orderTable.TotalWidth = 575f;
                        orderTable.LockedWidth = true;
                        orderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                        orderTable.AddCell(string.Empty);
                        pdfDocument.Add(orderTable);
                        Paragraph noOrder = new Paragraph("No Order Information Found".ToString());
                        noOrder.Alignment = Element.ALIGN_CENTER;
                        pdfDocument.Add(noOrder);
                    }
                    // End Append Order Information

                    #endregion

                    #region Radiology Information

                    // Start Append Test Information
                    if (dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrderTest.TableName].Rows.Count > 0)
                    {
                        //Start 28-03-2016 Humaira Yousaf
                        Paragraph test_Heading = new Paragraph("Test Information \n".ToString(), componentHeadingFont);
                        //End 28-03-2016 Humaira Yousaf

                        test_Heading.SpacingBefore = test_Heading.SpacingAfter = 5;
                        pdfDocument.Add(test_Heading);

                        PdfPTable testTable = new PdfPTable(4);
                        float[] widths = new float[] { 8f, 8f, 8f, 8f };
                        testTable.SetWidths(widths);
                        testTable.TotalWidth = 575f;
                        testTable.LockedWidth = true;
                        testTable.DefaultCell.Border = Rectangle.NO_BORDER;

                        foreach (DSOS_RadiologyOrder.OS_RadiologyOrderTestRow dr in dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrderTest.TableName].Rows)
                        {

                            testTable.AddCell(new Paragraph(MDVUtility.ToDateTime(dr[dsRadiologyOrder.OS_RadiologyOrderTest.TestDateColumn.ColumnName]).ToShortDateString() + ' ' + dr[dsRadiologyOrder.OS_RadiologyOrderTest.TestTimeColumn.ColumnName].ToString(), bodyFont));
                            testTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrderTest.CPTCodeDescriptionColumn.ColumnName])), gridbodyFont));
                            testTable.AddCell(string.Empty);
                            if (!string.IsNullOrEmpty(dr[dsRadiologyOrder.OS_RadiologyOrderTest.UrgencyNameColumn.ColumnName].ToString()))
                            {
                                testTable.AddCell(new Paragraph("Urgency: " + dr[dsRadiologyOrder.OS_RadiologyOrderTest.UrgencyNameColumn.ColumnName].ToString(), bodyFont));
                            }
                            else
                            {
                                testTable.AddCell(string.Empty);
                            }

                            if (!string.IsNullOrEmpty(dr[dsRadiologyOrder.OS_RadiologyOrderTest.SpecimenColumn.ColumnName].ToString()))
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr("Specimen: " + dr[dsRadiologyOrder.OS_RadiologyOrderTest.SpecimenColumn.ColumnName]), bodyFont));
                            }
                            else
                            {
                                testTable.AddCell(string.Empty);
                            }

                            if (!string.IsNullOrEmpty(dr[dsRadiologyOrder.OS_RadiologyOrderTest.VolumeLengthColumn.ColumnName].ToString()))
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr("Volume: " + dr[dsRadiologyOrder.OS_RadiologyOrderTest.VolumeLengthColumn.ColumnName] + " " + dr[dsRadiologyOrder.OS_RadiologyOrderTest.VolumeColumn.ColumnName]), bodyFont));
                            }
                            else
                            {
                                testTable.AddCell(string.Empty);
                            }
                            if (!string.IsNullOrEmpty(dr[dsRadiologyOrder.OS_RadiologyOrderTest.PatientInstructionColumn.ColumnName].ToString()))
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr("Patient Instruction: " + dr[dsRadiologyOrder.OS_RadiologyOrderTest.PatientInstructionColumn.ColumnName]), bodyFont));
                            }
                            else
                            {
                                testTable.AddCell(string.Empty);
                            }
                            if (!string.IsNullOrEmpty(dr[dsRadiologyOrder.OS_RadiologyOrderTest.FillerInstructionColumn.ColumnName].ToString()))
                            {
                                testTable.AddCell(new Paragraph(MDVUtility.ToStr("Filler Instruction: " + dr[dsRadiologyOrder.OS_RadiologyOrderTest.FillerInstructionColumn.ColumnName]), bodyFont));
                            }
                            else
                            {
                                testTable.AddCell(string.Empty);
                            }


                        }
                        pdfDocument.Add(testTable);
                    }
                    else
                    {
                        //Start 28-03-2016 Humaira Yousaf
                        Paragraph test_Heading = new Paragraph("Test Information \n".ToString(), componentHeadingFont);
                        //End 28-03-2016 Humaira Yousaf
                        test_Heading.SpacingBefore = test_Heading.SpacingAfter = 5;
                        pdfDocument.Add(test_Heading);
                        PdfPTable testTable = new PdfPTable(5);
                        float[] widths = new float[] { 8f, 8f, 8f, 8f, 8f };
                        testTable.SetWidths(widths);
                        testTable.TotalWidth = 575f;
                        testTable.LockedWidth = true;
                        testTable.DefaultCell.Border = Rectangle.NO_BORDER;
                        testTable.AddCell(string.Empty);
                        pdfDocument.Add(testTable);
                        //Start 28-03-2016 Humaira Yousaf
                        Paragraph noTest = new Paragraph("No Test Information Found".ToString());
                        //End 28-03-2016 Humaira Yousaf
                        noTest.Alignment = Element.ALIGN_CENTER;
                        pdfDocument.Add(noTest);
                    }
                    // End Append Associated Problems

                    #endregion

                    #region Problem List

                    // Start Append Associated Problems
                    if (dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrderProblem.TableName].Rows.Count > 0)
                    {

                        Paragraph problems_Heading = new Paragraph("Associated Problems \n".ToString(), componentHeadingFont);
                        problems_Heading.SpacingBefore = problems_Heading.SpacingAfter = 5;
                        pdfDocument.Add(problems_Heading);

                        PdfPTable problemsTable = new PdfPTable(1);
                        float[] widths = new float[] { 8f };
                        problemsTable.SetWidths(widths);
                        problemsTable.TotalWidth = 575f;
                        problemsTable.LockedWidth = true;
                        problemsTable.DefaultCell.Border = Rectangle.NO_BORDER;
                        problemsTable.AddCell(string.Empty);

                        foreach (DSOS_RadiologyOrder.OS_RadiologyOrderProblemRow dr in dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrderProblem.TableName].Rows)
                        {
                            problemsTable.AddCell(string.Empty);
                            problemsTable.AddCell(new Paragraph(HttpUtility.HtmlDecode(MDVUtility.ToStr(dr[dsRadiologyOrder.OS_RadiologyOrderProblem.ProblemNameColumn.ColumnName])), bodyFont));
                            problemsTable.AddCell(string.Empty);
                        }
                        pdfDocument.Add(problemsTable);
                    }
                    else
                    {

                        Paragraph problems_Heading = new Paragraph("Associated Problems \n".ToString(), componentHeadingFont);
                        problems_Heading.SpacingBefore = problems_Heading.SpacingAfter = 5;
                        pdfDocument.Add(problems_Heading);
                        PdfPTable problemsTable = new PdfPTable(5);
                        float[] widths = new float[] { 1f, 8f, 8f, 8f, 8f };
                        problemsTable.SetWidths(widths);
                        problemsTable.TotalWidth = 575f;
                        problemsTable.LockedWidth = true;
                        problemsTable.DefaultCell.Border = Rectangle.NO_BORDER;
                        problemsTable.AddCell(string.Empty);
                        pdfDocument.Add(problemsTable);
                        Paragraph noProblems = new Paragraph("No Associated Problems Found".ToString(), bodyFont);
                        noProblems.Alignment = Element.ALIGN_CENTER;
                        pdfDocument.Add(noProblems);
                    }
                    // End Append Associated Problems

                    #endregion


                    //Start 28-03-2016 Humaira Yousaf
                    pdfDocument.Add(Chunk.NEWLINE);
                    pdfDocument.Add(Chunk.NEWLINE);

                    string modifiedDate = MDVUtility.ToDateTime(dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows[0][dsRadiologyOrder.OS_RadiologyOrder.ModifiedOnColumn.ColumnName]).ToLongDateString();

                    if (!string.IsNullOrEmpty(modifiedDate))
                    {
                        modifiedDate = modifiedDate + " at ";
                    }
                    string modifiedTime = MDVUtility.ToDateTime(dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows[0][dsRadiologyOrder.OS_RadiologyOrder.ModifiedOnColumn.ColumnName]).ToLongTimeString();

                    Paragraph signedByPara = new Paragraph("e-Signed By: " + MDVUtility.ToStr(dsRadiologyOrder.Tables[dsRadiologyOrder.OS_RadiologyOrder.TableName].Rows[0][dsRadiologyOrder.OS_RadiologyOrder.ModifiedByColumn.ColumnName]) + " on " + modifiedDate + modifiedTime, bodyFont);
                    signedByPara.Alignment = Element.ALIGN_LEFT;
                    pdfDocument.Add(signedByPara);
                    //End 28-03-2016 Humaira Yousaf

                    newByteArr = stream_.GetBuffer();
                    pdf.Document.Close();
                    pdf.Writer.Close();
                    pdfDocument.Close();

                    MemoryStream stream = new MemoryStream(stream_.ToArray());
                    PdfReader npdf = new PdfReader(stream);
                    MemoryStream outstream = new MemoryStream();
                    using (PdfStamper stamper = new PdfStamper(npdf, outstream, '\0', true))
                    {
                        stamper.Writer.CloseStream = false;
                        int PageCount = npdf.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                        {
                            Paragraph para = new Paragraph(String.Format("Page {0}/{1}", i, PageCount), bodyFont);
                            para.Add(setLabOrder_ResultFooter(pdfDocument));
                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, para, 555, 17, 0);
                        }
                    }
                    newByteArr = outstream.GetBuffer();
                }

                return new BLObject<byte[]>(newByteArr);
                //End 04-05-2016 Humaira Yousaf for formatting and other changes
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::previewRadiologyOrder", ex);
                return new BLObject<byte[]>(null, ex.Message);
            }
        }

        // Author:  Azeem Raza Tayyab
        // Created Date: 19-Jan-2017
        //OverView: Method to Fill Radiology Order
        public BLObject<DSOS_RadiologyOrder> FillRadiologyOrder(long radiologyOrderId)
        {
            try
            {
                DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
                //Start 21-03-2016 Humaira Yousaf
                ds = new DALOS_RadiologyOrder().FillRadiologyOrder(radiologyOrderId);
                //End 21-03-2016 Humaira Yousaf
                return new BLObject<DSOS_RadiologyOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadRadiologyOrder", ex);
                return new BLObject<DSOS_RadiologyOrder>(null, ex.Message);
            }
        }
        #endregion

        #region"Radiology Order Test"

        public BLObject<DSOS_RadiologyOrder> insertUpdateRadiologyOrderTest(DSOS_RadiologyOrder ds)
        {
            try
            {
                ds = new DALOS_RadiologyOrder().insertUpdateRadiologyOrderTest(ds);
                return new BLObject<DSOS_RadiologyOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::insertUpdateRadiologyOrderTest", ex);
                return new BLObject<DSOS_RadiologyOrder>(null, ex.Message);
            }
        }
        public BLObject<DSOS_RadiologyOrder> LoadRadiologyOrderTest(long radiologyOrderId, string pageNumber, string rowsPerPage)
        {
            try
            {
                DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
                ds = new DALOS_RadiologyOrder().LoadRadiologyOrderTest(radiologyOrderId, pageNumber, rowsPerPage);
                return new BLObject<DSOS_RadiologyOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::LoadRadiologyOrderTest", ex);
                return new BLObject<DSOS_RadiologyOrder>(null, ex.Message);
            }
        }
        public BLObject<DSOS_RadiologyOrder> FillRadiologyOrderTest(Int32 radiologyOrderTestId)
        {
            try
            {
                DSOS_RadiologyOrder ds = new DSOS_RadiologyOrder();
                ds = new DALOS_RadiologyOrder().FillRadiologyOrderTest(radiologyOrderTestId);
                return new BLObject<DSOS_RadiologyOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::FillRadiologyOrderTest", ex);
                return new BLObject<DSOS_RadiologyOrder>(null, ex.Message);
            }
        }
        public BLObject<string> deleteRadiologyOrderTest(string RadiologyOrderTestId)
        {
            try
            {
                RadiologyOrderTestId = new DALOS_RadiologyOrder().deleteRadiologyOrderTest(Convert.ToInt64(RadiologyOrderTestId));
                return new BLObject<string>(RadiologyOrderTestId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSetl::deleteConsultationOrderTest", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region 'attach to note
        public BLObject<DSOrderSet> attachOrderSetWithNote(long notesId, long patientId, string orderSetId, string OrderSetComponents = "", string ProblemIDs = "", string ProcedureIDs = "", string LabOrderIDs = "", string RadiologyOrderIDs = "", string FollowUpIDs = "", string PatientEducationIDs = "", string ReferralsIDs = "", string ImmunizationIDs = "", string TherapeuticIDs = "", string ProcedureOrderIDs = "", long ProviderId = 0, bool AddInValidAgeRecordsInHxTab = false, string PatientProblemIds = "", string OrderSetAssociatedProblemIds = "")
        {
            try
            {
                DSOrderSet ds = new DSOrderSet();
                ds = new DALOrderSet().attachOrderSetWithNote(notesId, patientId, orderSetId, OrderSetComponents, ProblemIDs, ProcedureIDs, LabOrderIDs, RadiologyOrderIDs, FollowUpIDs, PatientEducationIDs, ReferralsIDs, ImmunizationIDs, TherapeuticIDs,ProcedureOrderIDs, ProviderId, AddInValidAgeRecordsInHxTab,PatientProblemIds,OrderSetAssociatedProblemIds);

                return new BLObject<DSOrderSet>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::attachOrderSetWithNote", ex);
                return new BLObject<DSOrderSet>(null, ex.Message);
            }
        }

        #endregion

        #region " Note Order Set "

        public BLObject<string> insertNoteOrderSet(OS_NoteModel model)
        {
            try
            {
                string formId;
                formId = new DALOrderSet().insertNoteOrderSet(model);
                return new BLObject<string>(formId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::insertNoteOrderSet", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> updateNoteOrderSet(OS_NoteModel model)
        {
            try
            {
                string formId;
                formId = new DALOrderSet().updateNoteOrderSet(model);
                return new BLObject<string>(formId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::updateNoteOrderSet", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<DSOrderSet> deleteNoteOrderSet(string NoteOSId, long NoteId, string OrderSetId)
        {
            try
            {
                DSOrderSet ds = new DSOrderSet();
                ds = new DALOrderSet().deleteOrderSetfFromNote(NoteId, OrderSetId);
                NoteOSId = new DALOrderSet().deleteNoteOrderSet(NoteOSId);
                return new BLObject<DSOrderSet>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::deleteNoteOrderSet", ex);
                return new BLObject<DSOrderSet>(null, ex.Message);
            }
        }

        public BLObject<List<OS_NoteModel>> loadNoteOrderSet(long NoteOSId)
        {
            try
            {
                var result = new DALOrderSet().loadNoteOrderSet(NoteOSId);
                return new BLObject<List<OS_NoteModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadNoteOrderSet", ex);
                return new BLObject<List<OS_NoteModel>>(null, ex.Message);
            }
        }

        #endregion

        #region Immunization

        public BLObject<DSOS_Immunization> InsertVaccineHx(DSOS_Immunization ds)
        {
            try
            {
                ds = new DALOS_Immunization().InsertVaccineHx(ds);
                return new BLObject<DSOS_Immunization>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrdeSet::InsertVaccineHx", ex);
                return new BLObject<DSOS_Immunization>(null, ex.Message);
            }
        }
        public BLObject<DSOS_Immunization> loadParentChildImmunization(long OrderSetId,long NotesId, int pageNo = 0, int rpp = 0)
        {
            try
            {
                DSOS_Immunization ds = new DSOS_Immunization();
                ds = new DALOS_Immunization().loadParentChildImmunization(OrderSetId,NotesId, pageNo, rpp);
                return new BLObject<DSOS_Immunization>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::loadParentChildImmunization", ex);
                return new BLObject<DSOS_Immunization>(null, ex.Message);
            }
        }

        public BLObject<DSOS_Immunization> GetAgeLimScheCategAgainstVaccShedId(long VaccineScheduleId)
        {
            try
            {
                DSOS_Immunization ds = new DSOS_Immunization();
                ds = new DALOS_Immunization().GetAgeLimScheCategAgainstVaccShedId(VaccineScheduleId);
                return new BLObject<DSOS_Immunization>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::GetAgeLimScheCategAgainstVaccShedId", ex);
                return new BLObject<DSOS_Immunization>(null, ex.Message);
            }
        }
        public BLObject<DSOS_Immunization> SearchVacinehxForEdit(long vaccineHxId)
        //End   || 22 April, 2016 || ZeeshanAK || Changes for audit
        {

            try
            {
                var ds = new DSOS_Immunization();
                //Start || 22 April, 2016 || ZeeshanAK || Changes for audit
                ds = new DALOS_Immunization().SearchVacinehxForEdit(vaccineHxId);
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit
                return new BLObject<DSOS_Immunization>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::LoadVaccineHx", ex);
                return new BLObject<DSOS_Immunization>(null, ex.Message);
            }
        }
        public BLObject<DSOS_Immunization> UpdateVaccineHx(DSOS_Immunization ds)
        {
            try
            {
                ds = new DALOS_Immunization().UpdateVaccineHx(ds);
                return new BLObject<DSOS_Immunization>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::UpdateVaccineHx", ex);
                return new BLObject<DSOS_Immunization>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteOsVaccinehx(string VaccineHxId)
        {
            try
            {
                VaccineHxId = new DALOS_Immunization().DeleteOsVaccinehx(VaccineHxId);
                return new BLObject<string>(VaccineHxId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::DeleteOsVaccinehx", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> IsVaccineHxInValidAge(string OS_VaccineHxId, long PatientId)
        {
            try
            {
                var VaccineHxIds = new DALOS_Immunization().IsVaccineHxInValidAge(OS_VaccineHxId, PatientId);
                return new BLObject<string>(VaccineHxIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::IsVaccineHxInValidAge", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> IsVaccineHxLotIssue(long OS_VaccineHxId, string Type,string ImmunizationIds)
        {
            try
            {
                var VaccineHxIds = new DALOS_Immunization().IsVaccineHxLotIssue(OS_VaccineHxId, Type, ImmunizationIds);
                return new BLObject<string>(VaccineHxIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::IsVaccineHxLotIssue", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region Therapeutic
        public BLObject<DSImmunization> OS_InsertTherapeuticInjection(DSImmunization ds)
        {
            try
            {
                ds = new DALOS_Immunization().OS_InsertTherapeuticInjection(ds);
                return new BLObject<DSImmunization>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::OS_InsertTherapeuticInjection", ex);
                return new BLObject<DSImmunization>(null, ex.Message);
            }
        }
        public BLObject<DSImmunization> OS_LoadImmunizationTherapeuticInjection(long immTherapeuticInjectionId, long OrderSetId, long NotesId=0, int pageNumber = 1, int rowsPerPage = 1000)
        {
            try
            {
                DSImmunization ds = new DSImmunization();
                ds = new DALOS_Immunization().OS_LoadImmunizationTherapeuticInjection(immTherapeuticInjectionId, OrderSetId,NotesId, pageNumber, rowsPerPage);
                return new BLObject<DSImmunization>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::OS_LoadImmunizationTherapeuticInjection", ex);
                return new BLObject<DSImmunization>(null, ex.Message);
            }
        }

        public BLObject<DSImmunization> Os_UpdateTherapeuticInjection(DSImmunization ds)
        {
            try
            {
                ds = new DALOS_Immunization().OS_UpdateTherapeuticInjection(ds);
                return new BLObject<DSImmunization>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::OS_UpdateTherapeuticInjection", ex);
                return new BLObject<DSImmunization>(null, ex.Message);
            }
        }

        public BLObject<string> DeleteOsTherapeutichx(string immTherapeuticInjectionId)
        {
            try
            {
                immTherapeuticInjectionId = new DALOS_Immunization().DeleteOsTherapeutichx(immTherapeuticInjectionId);
                return new BLObject<string>(immTherapeuticInjectionId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::DeleteOsTherapeutichx", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion
        #region medication
        public BLObject<DSImmunization> GetMedicationOrdersetLookUp(string LookUpTypeName)
        {
            try
            {
                DSImmunization ds = new DSImmunization();
                ds = new DALOS_Medication().GetMedicationOrdersetLookUp(LookUpTypeName);
                return new BLObject<DSImmunization>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::GetMedicationOrdersetLookUp", ex);
                return new BLObject<DSImmunization>(null, ex.Message);
            }
        }

        public string SaveOSMedication(OS_MedicationModel model)
        {
            try
            {
                return new DALOS_Medication().SaveOSMedication(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::SaveOSMedication", ex);
                throw ex;
            }
            finally
            {

            }
        }
        public BLObject<List<OS_MedicationModel>> LoadMedication(string OrdersetId, string OS_MedicationId, int pageNumber, int rowsPerPage)
        {
            try
            {
                List<OS_MedicationModel> MedicationList = new List<OS_MedicationModel>();
                MedicationList = new DALOS_Medication().LoadMedication(OrdersetId, OS_MedicationId, pageNumber, rowsPerPage);
                return new BLObject<List<OS_MedicationModel>>(MedicationList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCLINICAL::LoadMedication", ex);
                return new BLObject<List<OS_MedicationModel>>(null, ex.Message);
            }
        }
        public BLObject<List<OS_MedicationModel>> ExistsOrNotExistsMedication(string Os_MedicationIds, long PatientId)
        {
            try
            {
                List<OS_MedicationModel> MedicationList = new List<OS_MedicationModel>();
                MedicationList = new DALOS_Medication().ExistsOrNotExistsMedication(Os_MedicationIds, PatientId);
                return new BLObject<List<OS_MedicationModel>>(MedicationList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCLINICAL::ExistsOrNotExistsMedication", ex);
                return new BLObject<List<OS_MedicationModel>>(null, ex.Message);
            }
        }
        public BLObject<List<OS_MedicationModel>> LoadMedicationForDeleteFromDrFirst(string MedicationIds)
        {
            try
            {
                List<OS_MedicationModel> MedicationList = new List<OS_MedicationModel>();
                MedicationList = new DALOS_Medication().LoadMedicationForDeleteFromDrFirst(MedicationIds);
                return new BLObject<List<OS_MedicationModel>>(MedicationList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCLINICAL::LoadMedication", ex);
                return new BLObject<List<OS_MedicationModel>>(null, ex.Message);
            }
        }
        public string UpdateOSMedication(OS_MedicationModel model)
        {
            try
            {
                return new DALOS_Medication().UpdateOSMedication(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::UpdateOSMedication", ex);
                throw ex;
            }
            finally
            {

            }
        }
        public BLObject<string> DeleteOsMedication(string OS_MedicationId)
        {
            try
            {
                OS_MedicationId = new DALOS_Medication().DeleteOsMedication(OS_MedicationId);
                return new BLObject<string>(OS_MedicationId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::DeleteOsMedication", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion
        #region Procedure Order
        public BLObject<DSProcedureOrder> LoadProcedureOrder(long procedureOrderId, Int64 OrderSetId, int pageNumber = 1, int rowsPerPage = 1000)
        {
            try
            {
                DSProcedureOrder ds = new DSProcedureOrder();
                ds = new DALOS_ProcedureOrder().loadProcedureOrder(procedureOrderId, OrderSetId, pageNumber, rowsPerPage);
                return new BLObject<DSProcedureOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadProcedureOrder", ex);
                return new BLObject<DSProcedureOrder>(null, ex.Message);
            }
        }

        public BLObject<DSProcedureOrder> InsertUpdateProcedureOrder(DSProcedureOrder ds)
        {
            try
            {
                ds = new DALOS_ProcedureOrder().insertUpdateProcedureOrder(ds);
                return new BLObject<DSProcedureOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::InsertUpdateRadiologyOrder", ex);
                return new BLObject<DSProcedureOrder>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteProcedureOrderProblems(string procedureOrderId)
        {
            try
            {
                procedureOrderId = new DALOS_ProcedureOrder().deleteProcedureOrderProblems(Convert.ToInt64(procedureOrderId));
                return new BLObject<string>(procedureOrderId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::DeleteProcedureOrderProblems", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSProcedureOrder> InsertUpdateProcedureOrderProblems(DSProcedureOrder ds)
        {
            try
            {
                ds = new DALOS_ProcedureOrder().insertUpdateProcedureOrderProblems(ds);
                return new BLObject<DSProcedureOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::InsertUpdateProcedureOrderProblems", ex);
                return new BLObject<DSProcedureOrder>(null, ex.Message);
            }
        }
        public BLObject<DSProcedureOrder> LoadProcedureOrderTest(long procedureOderId, Int32 procedureOderTestId, string pageNumber, string rowsPerPage)
        {
            try
            {
                DSProcedureOrder ds = new DSProcedureOrder();
                ds = new DALOS_ProcedureOrder().LoadProcedureOrderTest(procedureOderId, procedureOderTestId, pageNumber, rowsPerPage);
                return new BLObject<DSProcedureOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadProcedureOrderTest", ex);
                return new BLObject<DSProcedureOrder>(null, ex.Message);
            }
        }

        public BLObject<DSProcedureOrder> insertUpdateProcedureOrderTest(DSProcedureOrder ds)
        {
            try
            {
                ds = new DALOS_ProcedureOrder().insertUpdateProcedureOrderTest(ds);
                return new BLObject<DSProcedureOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::insertUpdateProcedureOrderTest", ex);
                return new BLObject<DSProcedureOrder>(null, ex.Message);
            }
        }

        public BLObject<DSProcedureOrder> LoadProcedureOrderProblems(long procedureOrderId, int pageNumber = 1, int rowsPerPage = 1000)
        {
            try
            {
                DSProcedureOrder ds = new DSProcedureOrder();
                ds = new DALOS_ProcedureOrder().loadProcedureOrderProblems(0, procedureOrderId, pageNumber, rowsPerPage);
                return new BLObject<DSProcedureOrder>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LoadProcedureOrderProblems", ex);
                return new BLObject<DSProcedureOrder>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteProcedureOrder(string procedureOrderId)
        {
            try
            {
                procedureOrderId = new DALOS_ProcedureOrder().deleteProcedureOrder(Convert.ToInt64(procedureOrderId));
                return new BLObject<string>(procedureOrderId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::DeleteProcedureOrder", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> deleteProcedureOrderTest(string procedureOrderTestId)
        {
            try
            {
                procedureOrderTestId = new DALOS_ProcedureOrder().deleteProcedureOrderTest(Convert.ToInt64(procedureOrderTestId));
                return new BLObject<string>(procedureOrderTestId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::deleteProcedureOrderTest", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<List<OrderSetModel>> LookupOrderSetTemplate(DataTable dtProvider, string TemplateID)
        {
            try
            {
                var result = new DALOrderSet().LookupOrderSetTemplate(dtProvider, TemplateID);
                return new BLObject<List<OrderSetModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LookupOrderSetTemplate", ex);
                return new BLObject<List<OrderSetModel>>(null, ex.Message);

            }
        }
        public BLObject<List<OrderSetModel>> LookupOrderSetTemplateByID(string TemplateID)
        {
            try
            {

                var result = new DALOrderSet().LookupOrderSetByTemplateID(TemplateID);
                return new BLObject<List<OrderSetModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLOrderSet::LookupOrderSetTemplateByID", ex);
                return new BLObject<List<OrderSetModel>>(null, ex.Message);

            }
        }
        #endregion
    }
}
