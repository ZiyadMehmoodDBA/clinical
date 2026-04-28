using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.DataAccess.DAL.ERA;
using MDVision.Datasets;
using MDVision.Model.Billing.ERA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.Business.BLL
{
   public class BLLEOBManualPosting
    {
        #region Variable

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLEOBManualPosting"/> class.
        /// </summary>
        public BLLEOBManualPosting()
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

        #region EDI Submit Insurance
        
         public List<InsurancePaymentDetail> LoadInsurancePaymentSearch(InsurancePaymentDetail model)
        {
            try
            {
                List<InsurancePaymentDetail> mlist = new List<InsurancePaymentDetail>();
                mlist = new EOBManualPostingDAL().LoadInsurancePaymentSearch(model);
                return mlist;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLEOBManualPosting::LoadInsurancePaymentSearch", ex);
                return new List<InsurancePaymentDetail>();
            }
        }
        public InsurancePaymentDetail InsertEOBManualPosting(InsurancePaymentDetail model)
        {
            try
            {
                model = new EOBManualPostingDAL().InsertEOBManualPosting(model);
                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLEOBManualPosting::InsertEOBManualPosting", ex);
                return model;
            }
        }
        public InsurancePaymentDetail UpdateEOBManualPosting(InsurancePaymentDetail model)
        {
            try
            {
                model = new EOBManualPostingDAL().UpdateEOBManualPosting(model);
                return model;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLEOBManualPosting::InsertEOBManualPosting", ex);
                return model;
            }
        }
        public BLObject<string> EobManualPostingExistORNot(long VisitId,long InsurancePaymentDetailId)
        {
            try
            {
                var result = new EOBManualPostingDAL().EobManualPostingExistORNot(VisitId, InsurancePaymentDetailId);
                return new BLObject<string>(result); ;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLEOBManualPosting::EobManualPostingExistORNot", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<List<EOBManualChargeLoad>> LoadPatientCharges(long VisitId)
        {
            try
            {
                List<EOBManualChargeLoad> chargeModel = new List<EOBManualChargeLoad>();
                chargeModel = new EOBManualPostingDAL().LoadPatientCharges(VisitId);
                return new BLObject<List<EOBManualChargeLoad>>(chargeModel);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLEOBManualPosting::LoadPatientCharges", ex);
                return new BLObject<List<EOBManualChargeLoad>>(null, ex.Message);
            }
        }
        public long LoadEOBManualPostingDocument(long EOBId)
        {
            long PatDocId = 0;
            try
            {
               
                PatDocId = new EOBManualPostingDAL().LoadEOBManualPostingDocument(EOBId);
                return PatDocId;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLEOBManualPosting::LoadPatientCharges", ex);
                return PatDocId;
            }
        }
        public BLObject<List<EOBManualPaymentPost>> LoadEOBPaymentDetail(long VisitId=0,long EOBPostingId=0, long EOBDetlId=0)
        {
            try
            {
                List<EOBManualPaymentPost> chargeModel = new List<EOBManualPaymentPost>();
                chargeModel = new EOBManualPostingDAL().LoadEOBPaymentDetail(VisitId, EOBPostingId, EOBDetlId);
                return new BLObject<List<EOBManualPaymentPost>>(chargeModel);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLEOBManualPosting::LoadEOBPaymentDetail", ex);
                return new BLObject<List<EOBManualPaymentPost>>(null, ex.Message);
            }
        }
        public string DeleteEOBManualPostingDetail(long EOBDetlId = 0)
        {
            try
            {

                string deleteDetail = new EOBManualPostingDAL().DeleteEOBManualPostingDetail(EOBDetlId);
                return deleteDetail;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLEOBManualPosting::DeleteEOBManualPostingDetail", ex);
                return "";
            }
        }
        public string DeleteEOBManualPosting(long EOBPostingId = 0)
        {
            try
            {

                string deleteDetail = new EOBManualPostingDAL().DeleteEOBManualPosting(EOBPostingId);
                return deleteDetail;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLEOBManualPosting::DeleteEOBManualPosting", ex);
                return "";
            }
        }
        public string InsertEOBManualPostingDetail(DataTable dtpaymentlist)
        {
            try
            {
               
              string  insertDetail = new EOBManualPostingDAL().InsertEOBManualPostingDetail(dtpaymentlist);
                return insertDetail;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLEOBManualPosting::LoadPatientCharges", ex);
                return "";
            }
            
        }
        public string UpdateEOBManualPostingDetail(EOBManualPaymentPost modal)
        {
            try
            {

                string UpdateDetail = new EOBManualPostingDAL().UpdateEOBManualPostingDetail(modal);
                return UpdateDetail;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLEOBManualPosting::UpdateEOBManualPostingDetail", ex);
                return "";
            }

        }
        public string PostEOBManualPosting(long EOBPostingId = 0, long EOBDetlId = 0)
        {
            try
            {

                string UpdateDetail = new EOBManualPostingDAL().PostEOBManualPosting(EOBPostingId, EOBDetlId);
                return UpdateDetail;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLEOBManualPosting::PostEOBManualPosting", ex);
                return "";
            }
        }
        #endregion
    }
}
