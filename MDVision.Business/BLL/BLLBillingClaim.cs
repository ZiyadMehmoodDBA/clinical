using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.DataAccess.DAL.Claim;
using MDVision.DataAccess.DCommon;
using MDVision.DataAccess.DAL.Admin;
using MDVision.DataAccess.DAL.Charges;
using MDVision.DataAccess.DAL.Visits;
using System.IO;
using System.Data;
using iTextSharp.text;
using EDIParser.Professional;
using EDIParser;
using MDVision.DataAccess.DAL.ERA;
using MDVision.DataAccess.DAL.Admin.FollowUp;
using System.Web;
using System.Configuration;
using MDVision.Business.AlphaIIService;
using System.Xml.Serialization;
using System.Xml;
using MDVision.Business.ClaimScrubber;
using System.Threading.Tasks;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Model.Billing.ERA;
using Newtonsoft.Json.Linq;

namespace MDVision.Business.BLL
{

    public class BLLBillingClaim
    {

        private static object LockObj = new object();

        #region Constructors
        public BLLBillingClaim()
        {
            //SharedVariable SharedObj
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this.SharedObj = SharedObj;
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

        #region "Claim"
        public BLObject<DSVisits> LoadPatientClaim(string AccountNumber, string FirstName, string LastName, long ProviderId, long BatchNumber, long FacilityId, long PracticeId, long PatientInsuranceId, DateTime? DOSForm, DateTime? DOSTo, long ClearingHouseId, long BillerId, string ClaimType, string BillType, int ClaimStatusId, string ClaimErroredId, string SubmitionMod, string ClaimNumber, int PageNumber, int RowspPage, string SubmissionErrorId, int submitUserId)
        {
            try
            {
                DSVisits ds = new DSVisits();
                ds = new DALClaim().LoadPatientClaim(AccountNumber, FirstName, LastName, ProviderId, BatchNumber, FacilityId, PracticeId, PatientInsuranceId, DOSForm, DOSTo, ClearingHouseId, BillerId, ClaimType, BillType, ClaimStatusId, ClaimErroredId, SubmitionMod, ClaimNumber, PageNumber, RowspPage, SubmissionErrorId, submitUserId);

                return new BLObject<DSVisits>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::LoadPatientClaim", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }

        }

        public BLObject<DSVisits> LoadPatientClaim(SharedVariable SharedVariable, string AccountNumber, string FirstName, string LastName, long ProviderId, long BatchNumber, long FacilityId, long PracticeId, long PatientInsuranceId, DateTime? DOSForm, DateTime? DOSTo, long ClearingHouseId, long BillerId, string ClaimType, string BillType, int ClaimStatusId, string ClaimErroredId, string SubmitionMod, string ClaimNumber, int PageNumber, int RowspPage)
        {
            try
            {
                DSVisits ds = new DSVisits();
                ds = new DALClaim(SharedVariable).LoadPatientClaim(SharedVariable, AccountNumber, FirstName, LastName, ProviderId, BatchNumber, FacilityId, PracticeId, PatientInsuranceId, DOSForm, DOSTo, ClearingHouseId, BillerId, ClaimType, BillType, ClaimStatusId, ClaimErroredId, SubmitionMod, ClaimNumber, PageNumber, RowspPage);

                return new BLObject<DSVisits>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLBillingClaim::LoadPatientClaim", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }

        }


        public BLObject<DSVisits> LoadClaimSubmitHistory(string AccountNumber, long ClearingHouseId, string BatchNumber, string ClaimNumber, string SubmitionMod, string SubmittedDate, int PageNumber, int RowspPage)
        {
            try
            {
                DSVisits ds = new DSVisits();
                ds = new DALClaim().LoadClaimSubmitHistory(AccountNumber, ClearingHouseId, BatchNumber, ClaimNumber, SubmitionMod, SubmittedDate, PageNumber, RowspPage);

                return new BLObject<DSVisits>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::LoadClaimSubmitHistory", ex);
                return new BLObject<DSVisits>(null, ex.Message);
            }

        }





        #endregion

        #region " Claim Scrubber "
        public string InsertClaimScrubHistory(string editext, string jobnumber, List<string> stringvisits, string ServiceResponseText)
        {
            try
            {
                var VisitsString = String.Join(",", stringvisits);
                DSVisits ClaimSrubHistory = new DSVisits();
                DSVisits.ClaimScrubHistoryRow drClaimScrubHistory = ClaimSrubHistory.ClaimScrubHistory.NewClaimScrubHistoryRow();
                drClaimScrubHistory.EDIText = editext;
                drClaimScrubHistory.JobNumber = jobnumber;
                drClaimScrubHistory.VisitID = VisitsString;
                drClaimScrubHistory.ServiceResponseText = ServiceResponseText;
                drClaimScrubHistory.CreatedOn = DateTime.Now;
                drClaimScrubHistory.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                ClaimSrubHistory.ClaimScrubHistory.AddClaimScrubHistoryRow(drClaimScrubHistory);

                DSVisits dsClaimScrubHistory = new DALVisits().InsertClaimScrubHistory(ClaimSrubHistory);
                return "";
            }
            catch (Exception)
            {

                throw;
            }

        }
        public BLObject<ClaimScrub> ScrubClaims(List<long> Visits)
        {
            try
            {
                /*
                * API Limitation's
                *
                * 1- API accept's maximum of 250 Professional Claim's in  single request
                * 2- API accept's maximum of 50 Institutional Claim's in  single request
                *
                */
                var editext = "";
                if (Visits.Count <= 250)
                {
                    BLObject<string> EDIObj = new BLObject<string>();
                    DSHCFA ds = new DSHCFA();
                    DSEDILookup ds_edi = new DSEDILookup();

                    ds_edi = new DALClearingHouse().GetClearingHouseId();
                    if (ds_edi.ClearingHouse.Rows.Count > 0)
                    {
                        long ChearningHouseId = ds_edi.ClearingHouse.FirstOrDefault().ClearingHouseId;
                        EDIObj = Genrate837EDIString(Visits, ChearningHouseId, ref ds);
                        if (!string.IsNullOrEmpty(EDIObj.Data))
                        {
                            if (EDIObj.Data.Contains("CLM*"))
                            {
                                editext = EDIObj.Data;
                                string SubmittedID = MDVSession.Current.ClaimScrubberSubmitterID;// ConfigurationManager.AppSettings["AlphaIISubmitterID"];
                                string User = MDVSession.Current.ClaimScrubberUser; //ConfigurationManager.AppSettings["AlphaIIUser"];
                                string Password = MDVSession.Current.ClaimScrubberPassword; //ConfigurationManager.AppSettings["AlphaIIPassword"];
                                string AlphaIIEDIServer = MDVSession.Current.ClaimScrubberEDIServer; //ConfigurationManager.AppSettings["AlphaIIEDIServer"];

                                if (!string.IsNullOrEmpty(SubmittedID) && !string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Password))
                                {
                                    ScrubMethodSoapClient service = new ScrubMethodSoapClient();
                                    string response = service.Scrub(AlphaIIEDIServer, SubmittedID, User, Password, "XML", editext);

                                    if (!string.IsNullOrEmpty(response))
                                    {
                                        List<string> VisitsString = new List<string>();
                                        DSVisits dsvisits = new DSVisits();
                                        ClaimScrub ClaimScrub = new ClaimScrub();
                                        reporterrors response_obj = new reporterrors();

                                        try
                                        {
                                            XmlReader xmlReader = XmlReader.Create(new StringReader(response));
                                            response_obj = (reporterrors)new XmlSerializer(typeof(reporterrors)).Deserialize(xmlReader);

                                            #region " Process Service Response"

                                            //1- fill all info for response.
                                            //2- Delete Previous Errors of related Visits.
                                            //3- Insert into Claim Error table.

                                            foreach (var item in Visits)
                                            {
                                                long VisitId = MDVUtility.ToLong(item);

                                                // Delete Previous Errors
                                                new DALVisits().DeleteClaimErrors(0, VisitId);

                                                DSHCFA._837ClaimRow row = null;
                                                row = (DSHCFA._837ClaimRow)ds.Tables[ds._837Claim.TableName].Select("" + ds._837Claim.VisitIdColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString(VisitId)).FirstOrDefault();
                                                if (row != null)
                                                {
                                                    Claims claim = new Claims();
                                                    claim.VisitId = VisitId;
                                                    VisitsString.Add(Convert.ToString(VisitId));
                                                    claim.ClaimNumber = row.CLM01;
                                                    ClaimScrubber.claim obj = response_obj.file.batch.claim.FirstOrDefault(p => p.patientid == row.CLM01);
                                                    if (obj != null)
                                                    {
                                                        claim.HasErrors = obj.claimerrors.claimerror.Count > 0 ? true : false;
                                                        claim.ErrorsCount = obj.claimerrors.claimerror.Count;

                                                        // Errors
                                                        foreach (var errors in obj.errmaps.errmap)
                                                        {
                                                            DSVisits.ClaimErrorsRow drError = dsvisits.ClaimErrors.NewClaimErrorsRow();
                                                            drError.ErrorCode = errors.claimerrorcode;
                                                            claimerror claimerror = obj.claimerrors.claimerror.FirstOrDefault(p => p.claimerrorcode == drError.ErrorCode);
                                                            if (claimerror != null)
                                                            {
                                                                string seq = errors.seq;
                                                                if (!string.IsNullOrEmpty(errors.controlnumber))
                                                                {
                                                                    Item cptitem = obj.items.Item.FirstOrDefault(p => p.lineitemcontrolnumber == errors.controlnumber);
                                                                    if (cptitem != null)
                                                                        seq = cptitem.cpt + " (" + seq + ") ";
                                                                }
                                                                drError.Description = "[" + seq + "] " + claimerror.claimerrormsg;
                                                                drError.Action = claimerror.claimerroraction;
                                                            }
                                                            drError.VisitId = VisitId;
                                                            drError.JobNumber = response_obj.job.number;
                                                            dsvisits.ClaimErrors.AddClaimErrorsRow(drError);
                                                        }

                                                        //// Claim Level Errors
                                                        //foreach (var error in obj.claimerrors.claimerror)
                                                        //{
                                                        //    DSVisits.ClaimErrorsRow drError = dsvisits.ClaimErrors.NewClaimErrorsRow();
                                                        //    drError.ErrorCode = error.claimerrorcode;
                                                        //    drError.Description = error.claimerrormsg;
                                                        //    drError.Action = error.claimerroraction;
                                                        //    drError.VisitId = VisitId;
                                                        //    drError.JobNumber = response_obj.job.number;
                                                        //    dsvisits.ClaimErrors.AddClaimErrorsRow(drError);
                                                        //}

                                                        //// Line Item Level Errors
                                                        //foreach (var error in obj.items.Item)
                                                        //{
                                                        //    if (!string.IsNullOrEmpty(error.codes))
                                                        //    {
                                                        //        DSVisits.ClaimErrorsRow drError = dsvisits.ClaimErrors.NewClaimErrorsRow();
                                                        //        drError.ErrorCode = error.codes;
                                                        //        string desc = string.Join(",", error.policyfiles.policyfile.Select(n => n.ToString()).ToArray());

                                                        //        if (!string.IsNullOrEmpty(desc))
                                                        //            drError.Description = "CPT: " + error.cpt + ", Sequence: " + error.seq + ", Detail:" + desc;
                                                        //        else
                                                        //            drError.Description = "CPT: " + error.cpt + ", Sequence: " + error.seq;

                                                        //        drError.VisitId = VisitId;
                                                        //        drError.JobNumber = response_obj.job.number;
                                                        //        dsvisits.ClaimErrors.AddClaimErrorsRow(drError);
                                                        //    }
                                                        //}

                                                    }
                                                    else
                                                    {
                                                        claim.HasErrors = false;
                                                        claim.ErrorsCount = 0;
                                                    }

                                                    ClaimScrub.Claims.Add(claim);
                                                }
                                            }

                                            #endregion

                                            if (VisitsString.Count > 0)
                                            {
                                                InsertClaimScrubHistory(editext, response_obj.job.number, VisitsString, "Processed Visits" + " Service Response Text:" + response);

                                            }

                                            //Add Errors
                                            DSVisits ds_ = new DALVisits().InsertClaimErrors(dsvisits);

                                            ClaimScrub.ErrorReport = response_obj;
                                            return new BLObject<ClaimScrub>(ClaimScrub);

                                        }
                                        catch (Exception ex)
                                        {
                                            InsertClaimScrubHistory(editext, response_obj.job.number, Visits.Cast<string>().ToList(), "Requested Visits,Exception Message:" + ex.Message + " Service Response Text:" + response);
                                            throw ex;
                                        }
                                    }
                                    else
                                        return new BLObject<ClaimScrub>(null, "Credentials are not setup please contact to you'r Administrator.");
                                }
                                else
                                    return new BLObject<ClaimScrub>(null, "Authentication failure please contact to you'r Administrator.");
                            }
                            else
                            {
                                return new BLObject<ClaimScrub>(null, "Claim Data is not filled properly.");
                            }

                        }
                        else
                        {
                            return new BLObject<ClaimScrub>(null, EDIObj.Message);
                        }

                    }
                    else
                        return new BLObject<ClaimScrub>(null, "EDI Header Information is missing.");
                }
                else
                    return new BLObject<ClaimScrub>(null, "Maximum 250 Claim's are allowed to be scrubbed.");


            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::ValidateClaims", ex);
                return new BLObject<ClaimScrub>(null, ex.Message);

            }
        }

        #endregion

        #region "837 Batch and Claim"

        #region "837 Batch Claim"


        public void ThreadSleep()
        {
            System.Threading.Thread.Sleep(2000);
        }

        public BLObject<BLLClaimSubmissionModel> UploadClaim(List<long> VisitIds, long ClearingHouseId, bool MarkSubmitted)
        {

            // For locking functionality
            //lock (LockObj)
            //{
            DSEDI dsEDI = new DSEDI();
            string submit_Message = "";
            BLLClaimSubmissionModel Status_obj = new BLLClaimSubmissionModel();
            Status_obj.Status = false;
            Status_obj.ClaimsCount = VisitIds.Count;

            bool Isreadytosubmit = false;
            SharedVariable SharedVariable = new SharedVariable();
            SharedVariable.EntityId = MDVSession.Current.EntityId;
            SharedVariable.AppUserId = MDVSession.Current.AppUserId;
            SharedVariable.UserName = MDVSession.Current.AppUserName;
            SharedVariable.ClientId = MDVSession.Current.ClientId;
            SharedVariable.AppPassWord = MDVSession.Current.AppPassWord;
            SharedVariable.WebEntityURL = MDVSession.Current.WebEntityURL;

            try
            {
                dsEDI = new DALClearingHouse().LoadClearingHouse(ClearingHouseId, null, null, null, null);

                string ftp = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTPColumn]);
                string userName = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserNameColumn]);
                string userPassword = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserPasswordColumn]);
                Int32 ftpPortNo = MDVUtility.ToInt32(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_PORTNOColumn]);
                string ftpHostKey = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_HOSTKEYColumn]);
                string FTPUploadFolder = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.IN_UPLOADEDColumn]);
                string WinscpPatch = string.Empty;
                if (HttpContext.Current != null)
                    WinscpPatch = MDVUtility.GetDLLPath("~/DLL", "winscp", "exe");
                else
                    WinscpPatch = System.Configuration.ConfigurationManager.AppSettings["WinSCP_Path"];

                if (MarkSubmitted)
                    Isreadytosubmit = true;
                else if (ftp != "" && userName != "" && userPassword != "" && FTPUploadFolder != "")
                    Isreadytosubmit = true;

                // Here we will check either FTP information is correct or not. If correct then we will continue further process, other wise we will prompt information was incorrect.
                if (Isreadytosubmit)
                {



                    Dictionary<int, Task<BLLClaimSubmissionModel>> tasks = new Dictionary<int, Task<BLLClaimSubmissionModel>>();
                    double Claims_count = VisitIds.Count;
                    if (Claims_count > 25)
                    {
                        if (Claims_count > 50)
                        {
                            if (Claims_count > 75)
                            {
                                //four threads
                                ClaimSubmission claim_obj_taks1 = new ClaimSubmission(SharedVariable, VisitIds.Skip(0).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                ClaimSubmission claim_obj_taks2 = new ClaimSubmission(SharedVariable, VisitIds.Skip(25).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                ClaimSubmission claim_obj_taks3 = new ClaimSubmission(SharedVariable, VisitIds.Skip(50).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                ClaimSubmission claim_obj_taks4 = new ClaimSubmission(SharedVariable, VisitIds.Skip(75).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                Task<BLLClaimSubmissionModel> task1 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks1.SubmitClaims());
                                Task<BLLClaimSubmissionModel> task2 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks2.SubmitClaims());
                                Task<BLLClaimSubmissionModel> task3 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks3.SubmitClaims());
                                Task<BLLClaimSubmissionModel> task4 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks4.SubmitClaims());
                                tasks.Add(1, task1); tasks.Add(2, task2); tasks.Add(3, task3); tasks.Add(4, task4);

                            }
                            else
                            {
                                // three threads
                                ClaimSubmission claim_obj_taks1 = new ClaimSubmission(SharedVariable, VisitIds.Skip(0).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                ClaimSubmission claim_obj_taks2 = new ClaimSubmission(SharedVariable, VisitIds.Skip(25).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                ClaimSubmission claim_obj_taks3 = new ClaimSubmission(SharedVariable, VisitIds.Skip(50).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                Task<BLLClaimSubmissionModel> task1 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks1.SubmitClaims());
                                Task<BLLClaimSubmissionModel> task2 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks2.SubmitClaims());
                                Task<BLLClaimSubmissionModel> task3 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks3.SubmitClaims());
                                tasks.Add(1, task1); tasks.Add(2, task2); tasks.Add(3, task3);
                            }
                        }
                        else
                        {
                            // two threads
                            ClaimSubmission claim_obj_taks1 = new ClaimSubmission(SharedVariable, VisitIds.Skip(0).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                            ClaimSubmission claim_obj_taks2 = new ClaimSubmission(SharedVariable, VisitIds.Skip(25).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                            Task<BLLClaimSubmissionModel> task1 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks1.SubmitClaims());
                            Task<BLLClaimSubmissionModel> task2 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks2.SubmitClaims());
                            tasks.Add(1, task1); tasks.Add(2, task2);
                        }
                    }
                    else
                    {
                        // single thread
                        List<long> VisitsList_taks1 = VisitIds.Skip(0).Take(25).ToList();
                        ClaimSubmission claim_obj = new ClaimSubmission(SharedVariable, VisitsList_taks1, ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                        Task<BLLClaimSubmissionModel> task = new Task<BLLClaimSubmissionModel>(() => claim_obj.SubmitClaims());
                        tasks.Add(1, task);
                    }

                    foreach (var item in tasks)
                    {
                        item.Value.Start();
                        ThreadSleep();
                    }

                    Task.WaitAll(tasks.Values.ToArray());
                    tasks.OrderByDescending(p => p.Key);

                    int total_claims = VisitIds.Count;
                    int total_submitted_claims = 0;
                    int total_Error_claims_count = 0;
                    int BatchStatusCount = 0;
                    double totalBatch = Math.Ceiling(Claims_count / 25);

                    List<BLLClaimSubmissionModel> list = tasks.Values.ToList<Task<BLLClaimSubmissionModel>>().Select(p => p.Result).ToList<BLLClaimSubmissionModel>();

                    //Count number of failed batches due to Db exception
                    BatchStatusCount = list.Where(q => q.Status == false && q.isDBException == true).Count();

                    total_Error_claims_count = list.Select(q => q.EroredClaims.Count).Sum();
                    total_submitted_claims = total_claims - total_Error_claims_count;


                    //if Db exception occure  in all batches then throw exception
                    if (totalBatch == BatchStatusCount)
                    {
                        throw new Exception("Claim submission failed. please contact to your administrator.");
                    }
                    //if Db exception occure in some batches then throw exception
                    else if (BatchStatusCount > 0)
                    {
                        throw new Exception(BatchStatusCount + " batch failed to submit. please contact to your administrartor.");
                    }
                    else
                    {

                        if (total_submitted_claims > 0)
                        {
                            if (MarkSubmitted == false)
                            {
                                submit_Message = total_submitted_claims + " claim(s) uploaded successfully.";
                            }
                            else
                                submit_Message = total_submitted_claims + " claim(s) marked as Transmitted successfully.";
                        }

                        if (total_claims == total_submitted_claims)
                        {
                            Status_obj.Status = true;
                            Status_obj.Message = submit_Message;
                            Status_obj.isDBException = false;
                        }
                        else
                        {
                            //handle other exceptions.i.e related to parsing of data
                            Status_obj.Status = false;
                            Status_obj.isDBException = false;
                            Status_obj.Message = submit_Message;
                            Status_obj.ClaimsCount = total_submitted_claims;
                            foreach (var item in list)
                                foreach (var errored_ in item.EroredClaims)
                                    Status_obj.EroredClaims.Add(errored_.Key, errored_.Value);
                        }
                    }
                }
                else
                    throw new Exception("Please setup FTP for selected clearing house");

                return new BLObject<BLLClaimSubmissionModel>(Status_obj);
            }
            catch (Exception ex)
            {
                Status_obj.Status = false;
                Status_obj.Message = ex.Message;
                Status_obj.isDBException = true;
                MDVLogger.BLLErrorLog(SharedVariable, "BLLBillingClaim::UploadClaim", ex);
                return new BLObject<BLLClaimSubmissionModel>(Status_obj);
            }
            //}
        }
        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="VisitIds"></param>
        /// <param name="ClearingHouseId"></param>
        /// <param name="MarkSubmitted"></param>
        /// <returns></returns>
        public BLObject<bool> UploadClaim(SharedVariable SharedVariable, List<long> VisitIds, long ClearingHouseId, bool MarkSubmitted)
        {

            // For locking functionality
            //lock (LockObj)
            //{
            DSEDI dsEDI = new DSEDI();
            string submit_Message = "";
            bool submit_Status = false;
            bool Isreadytosubmit = false;
            try
            {
                dsEDI = new DALClearingHouse(SharedVariable).LoadClearingHouse(SharedVariable, ClearingHouseId, null, null, null, null);

                string ftp = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTPColumn]);
                string userName = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserNameColumn]);
                string userPassword = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserPasswordColumn]);
                Int32 ftpPortNo = MDVUtility.ToInt32(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_PORTNOColumn]);
                string ftpHostKey = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_HOSTKEYColumn]);
                string FTPUploadFolder = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.IN_UPLOADEDColumn]);
                string WinscpPatch = string.Empty;
                if (HttpContext.Current != null)
                    WinscpPatch = MDVUtility.GetDLLPath("~/DLL", "winscp", "exe");
                else
                    WinscpPatch = System.Configuration.ConfigurationManager.AppSettings["WinSCP_Path"];

                if (MarkSubmitted)
                    Isreadytosubmit = true;
                else if (ftp != "" && userName != "" && userPassword != "" && FTPUploadFolder != "")
                    Isreadytosubmit = true;

                // Here we will check either FTP information is correct or not. If correct then we will continue further process, other wise we will prompt information was incorrect.
                if (Isreadytosubmit)
                {
                    Dictionary<int, Task<BLLClaimSubmissionModel>> tasks = new Dictionary<int, Task<BLLClaimSubmissionModel>>();
                    double Claims_count = VisitIds.Count;
                    if (Claims_count > 25)
                    {
                        if (Claims_count > 50)
                        {
                            if (Claims_count > 75)
                            {
                                //four threads
                                ClaimSubmission claim_obj_taks1 = new ClaimSubmission(SharedVariable, VisitIds.Skip(0).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                ClaimSubmission claim_obj_taks2 = new ClaimSubmission(SharedVariable, VisitIds.Skip(25).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                ClaimSubmission claim_obj_taks3 = new ClaimSubmission(SharedVariable, VisitIds.Skip(50).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                ClaimSubmission claim_obj_taks4 = new ClaimSubmission(SharedVariable, VisitIds.Skip(75).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                Task<BLLClaimSubmissionModel> task1 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks1.SubmitClaims());
                                Task<BLLClaimSubmissionModel> task2 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks2.SubmitClaims());
                                Task<BLLClaimSubmissionModel> task3 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks3.SubmitClaims());
                                Task<BLLClaimSubmissionModel> task4 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks4.SubmitClaims());
                                tasks.Add(1, task1); tasks.Add(2, task2); tasks.Add(3, task3); tasks.Add(4, task4);

                            }
                            else
                            {
                                // three threads
                                ClaimSubmission claim_obj_taks1 = new ClaimSubmission(SharedVariable, VisitIds.Skip(0).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                ClaimSubmission claim_obj_taks2 = new ClaimSubmission(SharedVariable, VisitIds.Skip(25).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                ClaimSubmission claim_obj_taks3 = new ClaimSubmission(SharedVariable, VisitIds.Skip(50).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                                Task<BLLClaimSubmissionModel> task1 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks1.SubmitClaims());
                                Task<BLLClaimSubmissionModel> task2 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks2.SubmitClaims());
                                Task<BLLClaimSubmissionModel> task3 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks3.SubmitClaims());
                                tasks.Add(1, task1); tasks.Add(2, task2); tasks.Add(3, task3);
                            }
                        }
                        else
                        {
                            // two threads
                            ClaimSubmission claim_obj_taks1 = new ClaimSubmission(SharedVariable, VisitIds.Skip(0).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                            ClaimSubmission claim_obj_taks2 = new ClaimSubmission(SharedVariable, VisitIds.Skip(25).Take(25).ToList(), ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                            Task<BLLClaimSubmissionModel> task1 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks1.SubmitClaims());
                            Task<BLLClaimSubmissionModel> task2 = new Task<BLLClaimSubmissionModel>(() => claim_obj_taks2.SubmitClaims());
                            tasks.Add(1, task1); tasks.Add(2, task2);
                        }
                    }
                    else
                    {
                        // single thread
                        List<long> VisitsList_taks1 = VisitIds.Skip(0).Take(25).ToList();
                        ClaimSubmission claim_obj = new ClaimSubmission(SharedVariable, VisitsList_taks1, ClearingHouseId, MarkSubmitted, WinscpPatch, dsEDI);
                        Task<BLLClaimSubmissionModel> task = new Task<BLLClaimSubmissionModel>(() => claim_obj.SubmitClaims());
                        tasks.Add(1, task);
                    }

                    foreach (var item in tasks)
                    {
                        item.Value.Start();
                        ThreadSleep();
                    }

                    Task.WaitAll(tasks.Values.ToArray());
                    tasks.OrderByDescending(p => p.Key);
                    int total_submitted_claims = 0;
                    List<BLLClaimSubmissionModel> list = tasks.Values.ToList<Task<BLLClaimSubmissionModel>>().Select(p => p.Result).ToList<BLLClaimSubmissionModel>();
                    if (list.Where(p => p.Status == true).Count() > 0)
                        total_submitted_claims = list.Where(p => p.Status == true).Sum(q => q.ClaimsCount);


                    if (total_submitted_claims > 0)
                    {
                        string claims_str = "claims";
                        submit_Status = true;

                        if (total_submitted_claims == 1)
                            claims_str = "claim";

                        if (MarkSubmitted == false)
                        {
                            submit_Message = total_submitted_claims + " " + claims_str + " uploaded successfully.";
                        }
                        else
                            submit_Message = total_submitted_claims + " " + claims_str + " marked as Transmitted successfully.";
                    }
                    else
                    {
                        submit_Message = list.FirstOrDefault(p => p.Message != null).Message;
                    }

                }
                else
                    throw new Exception("Please setup FTP for selected clearing house");

                return new BLObject<bool>(submit_Status, submit_Message);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLBillingClaim::UploadClaim", ex);
                return new BLObject<bool>(false, ex.Message);
            }
            //}
        }

        public BLObject<bool> UpdateResubmitStatusOfVisitsAndCharges(long BatchId, long StatusId, string VisitIds, int VisitStatusId, int ChargeStatusId)
        {
            bool flag = false;
            try
            {
                string objUpdate = new DALVisits().UpdateResubmitStatusOfVisitsAndCharges(BatchId, StatusId, VisitIds, VisitStatusId, ChargeStatusId);
                if (!string.IsNullOrEmpty(objUpdate))
                    throw (new Exception(objUpdate));
                flag = true;
            }
            catch (Exception ex)
            {
                return new BLObject<bool>(false, ex.Message);
            }
            return new BLObject<bool>(flag);
        }
        public BLObject<bool> UpdateVisitChargeStatus(int _837Batchid, int SubmitStatus, List<long> VisitIds = null, long VisitId = 0)
        {
            bool flag = false;
            DSVisits dsVisit = new DSVisits();
            DSCharge dsCharge = new DSCharge();
            DSCharge dsChargeTemp = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (VisitId == 0)
                {
                    // Get Patient Visits
                    dsVisit = new DALVisits().LoadPatientVisits(0, 0, 0, 0, null, null, "", "", "", _837Batchid);
                    // Get Patient Charges
                    dsCharge = new DALCharge().LoadPatientCharges(0, "", "", 0, 0, "", 0, "", "", null, null, 0, 0, null, _837Batchid);
                }
                else
                { // Get Patient Visits
                    dsVisit = new DALVisits().LoadPatientVisits(VisitId, 0, 0, 0, null, null, "", "", "", 0);
                    // Get Patient Charges
                    dsCharge = new DALCharge().LoadPatientCharges(0, "", "", 0, 0, "", 0, "", "", null, null, VisitId, 0, null, 0);
                }
                foreach (DataRow row in dsCharge.PatientCharges.Rows)
                {
                    dsChargeTemp.PatientCharges.ImportRow(row);
                }
                foreach (DSVisits.PatientVisitsRow drVisit in dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows)
                {
                    if (SubmitStatus == 3)
                    {
                        if (VisitIds.Contains(drVisit.VisitId))
                        {
                            drVisit.SubmittedBy = null;
                            drVisit.SubmittedDate = null;
                            // ReSubmit
                            drVisit.VisitStatus = "Seen";
                            drVisit.ClaimStatusId = SubmitStatus;
                        }
                    }
                    else
                    {
                        drVisit.SubmittedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        drVisit.SubmittedDate = DateTime.Now.ToString();
                        // Submitted
                        drVisit.VisitStatus = "Bill";
                        drVisit.ClaimStatusId = SubmitStatus;
                    }
                }
                // Transaction Begin
                dbManager.BeginTransaction();
                dsVisit = new DALVisits().UpdatePatientVisits(dsVisit, dbManager);
                //
                DateTime today = DateTime.Now;
                //
                foreach (DSCharge.PatientChargesRow drCharge in dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows)
                {
                    if (SubmitStatus == 3)
                    {
                        if (VisitIds.Contains(drCharge.VisitId))
                        {
                            if (drCharge.IsActive)
                            {
                                /*    TimeSpan ts = today - drCharge.HoldFrom;
                                    if (!drCharge.HoldDays.ToString().Equals(""))
                                    {
                                        if (ts.Days > drCharge.HoldDays)
                                        {
                                            //drCharge.StatusId = 3;
                                            drCharge.Status = "ReSubmit";
                                        }
                                    }
                                }
                                else
                                {*/
                                //drCharge.StatusId = 3;
                                drCharge.Status = "ReSubmit";
                            }
                        }
                    }
                    else
                    {
                        if (drCharge.IsActive)
                        {
                            /* TimeSpan ts = today - drCharge.HoldFrom;

                             if (!drCharge.HoldDays.ToString().Equals(""))
                             {
                                 if (ts.Days > drCharge.HoldDays)
                                 {
                                     //drCharge.StatusId = 2;
                                     if (drCharge.Status != "Submitted")
                                         drCharge.Status = "Submitted";
                                 }
                             }
                         }
                         else
                         {*/
                            //drCharge.StatusId = 2;
                            if (drCharge.Status != "Submitted")
                                drCharge.Status = "Submitted";
                        }
                    }

                    drCharge.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drCharge.ModifiedOn = DateTime.Now;
                }

                dsCharge = new DALCharge().UpdatePatientCharges(dsCharge, dbManager, dsChargeTemp);
                // Transaction Commit
                dbManager.CommitTransaction();
                flag = true;
            }
            catch (Exception ex)
            {
                dsVisit.RejectChanges();
                dbManager.RollBackTransaction();
                return new BLObject<bool>(false, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
            return new BLObject<bool>(flag);
        }

        public BLObject<bool> UpdateVisitChargeStatus(int SubmitStatus, long VisitId, long ChargeId = 0)
        {
            bool flag = false;
            DSVisits dsVisit = new DSVisits();
            DSCharge dsCharge = new DSCharge();
            DSCharge dsChargeTemp = new DSCharge();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                // Transaction Begin
                dbManager.BeginTransaction();
                // Get Patient Visits
                dsVisit = new DALVisits().LoadPatientVisits(VisitId, 0, 0, 0, null, null, "", "", "", 0);
                // Get Patient Charges
                dsCharge = new DALCharge().LoadPatientCharges(ChargeId, "", "", 0, 0, "", 0, "", "", null, null, VisitId, 0, null, 0);

                //UPDATE VISITS
                foreach (DSVisits.PatientVisitsRow drVisit in dsVisit.Tables[dsVisit.PatientVisits.TableName].Rows)
                {
                    if (SubmitStatus == 3)
                    {
                        //if (VisitIds.Contains(drVisit.VisitId))
                        //{
                        drVisit.SubmittedBy = null;
                        drVisit.SubmittedDate = null;
                        // ReSubmit
                        drVisit.VisitStatus = "Seen";
                        drVisit.ClaimStatusId = SubmitStatus;
                        //  }
                    }
                    else
                    {
                        drVisit.SubmittedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        drVisit.SubmittedDate = DateTime.Now.ToString();
                        // Submitted
                        drVisit.VisitStatus = "Bill";
                        drVisit.ClaimStatusId = SubmitStatus;
                    }

                    drVisit.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drVisit.ModifiedOn = DateTime.Now;
                }


                dsVisit = new DALVisits().UpdatePatientVisits(dsVisit, dbManager);
                foreach (DataRow row in dsCharge.PatientCharges.Rows)
                {
                    dsChargeTemp.PatientCharges.ImportRow(row);
                }

                // dsChargeTemp = dsCharge.PatientCharges.Rows;


                // UPDATE CHARGES
                DateTime today = DateTime.Now;
                //
                foreach (DSCharge.PatientChargesRow drCharge in dsCharge.Tables[dsCharge.PatientCharges.TableName].Rows)
                {
                    if (SubmitStatus == 3)
                    {

                        //if (VisitIds.Contains(drCharge.VisitId))
                        //{
                        if (drCharge.IsActive)
                        {
                            /*  TimeSpan ts = today - drCharge.HoldFrom;
                              if (!drCharge.HoldDays.ToString().Equals(""))
                              {
                                  if (ts.Days > drCharge.HoldDays)
                                  {
                                      drCharge.StatusId = 3;
                                      drCharge.Status = "ReSubmit";
                                  }
                              }
                          }
                          else
                          {*/
                            drCharge.StatusId = 3;
                            drCharge.Status = "ReSubmit";
                        }
                        //  }
                    }
                    else
                    {
                        if (drCharge.IsActive)
                        {
                            /* TimeSpan ts = today - drCharge.HoldFrom;

                             if (!drCharge.HoldDays.ToString().Equals(""))
                             {
                                 if (ts.Days > drCharge.HoldDays)
                                 {
                                     drCharge.StatusId = 2;
                                     if (drCharge.Status != "Submitted")
                                         drCharge.Status = "Submitted";
                                 }
                             }
                         }
                         else
                         {*/
                            drCharge.StatusId = 2;
                            if (drCharge.Status != "Submitted")
                                drCharge.Status = "Submitted";
                        }
                    }

                    //PMS-1640
                    drCharge.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drCharge.ModifiedOn = DateTime.Now;
                }

                dsCharge = new DALCharge().UpdatePatientCharges(dsCharge, dbManager, dsChargeTemp);
                // Transaction Commit
                dbManager.CommitTransaction();
                flag = true;
            }
            catch (Exception ex)
            {
                dsVisit.RejectChanges();
                dbManager.RollBackTransaction();
                return new BLObject<bool>(false, ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
            return new BLObject<bool>(flag);
        }
        public BLObject<bool> UpdateVisitsSubmitStatus(string VisitIds, int SubmitStatusId)
        {
            bool flag = false;
            try
            {
                string objUpdate = new DALVisits().UpdateVisitsSubmitStatus(VisitIds, SubmitStatusId);
                if (!string.IsNullOrEmpty(objUpdate))
                    throw (new Exception(objUpdate));
                flag = true;
                return new BLObject<bool>(flag);
            }
            catch (Exception ex)
            {
                return new BLObject<bool>(false, ex.Message);
            }
        }
        public BLObject<List<DSHCFA>> PrintClaim(List<long> VisitIds, long ClearingHouseId, bool IsSubmit, bool MarkSubmitted, bool ViewOnly = false)
        {
            try
            {
                List<DSHCFA> ds_list = new List<DSHCFA>();
                DS837Batch dsBatch = null;
                int batchId = 0;
                BLObject<DS837Batch> ds837Batch = new BLObject<DS837Batch>();
                bool IsMarkSubmitted = false;
                if (MarkSubmitted)
                    IsMarkSubmitted = MarkSubmitted;

                if (IsSubmit)
                {
                    SharedVariable SharedVariable = new SharedVariable();
                    SharedVariable.EntityId = MDVSession.Current.EntityId;
                    SharedVariable.AppUserId = MDVSession.Current.AppUserId;
                    SharedVariable.UserName = MDVSession.Current.AppUserName;
                    SharedVariable.ClientId = MDVSession.Current.ClientId;
                    SharedVariable.AppPassWord = MDVSession.Current.AppPassWord;
                    SharedVariable.WebEntityURL = MDVSession.Current.WebEntityURL;

                    //Create Batch
                    ds837Batch = new ClaimSubmission(SharedVariable, "").Create837BatchClaim(VisitIds, ClearingHouseId, false, IsMarkSubmitted, true);

                    if (ds837Batch.Data == null)
                    {
                        throw new Exception(ds837Batch.Message);
                    }
                    else
                    {
                        dsBatch = ds837Batch.Data;
                        if (dsBatch != null)
                            batchId = Convert.ToInt32(dsBatch._837Batch.Rows[0][dsBatch._837Batch._837BatchIdColumn]);
                    }
                }

                try
                {
                    if (MarkSubmitted == false)
                    {
                        DSHCFA dsTemp = new DSHCFA();
                        dsTemp.Merge(new DAL837().LoadClaimSubmission(string.Join(",", VisitIds.Select(n => n.ToString()).ToArray()), 0, ViewOnly));
                        dsTemp.Merge(new DAL837().LoadClaimICDS(string.Join(",", VisitIds.Select(n => n.ToString()).ToArray())));
                        ds_list = GetHCFAClaimsList(ref dsTemp); //get Claim vise list for HCFA.

                        //if (batchId != 0)
                        //{
                        //    DSHCFA dsTemp = new DSHCFA();
                        //    dsTemp.Merge(new DAL837().Load837Claim(0, batchId));
                        //    dsTemp.Merge(new DAL837().Load837NamesByBatchId(batchId));
                        //    dsTemp.Merge(new DAL837().Load837ServiceLine(0, batchId, ViewOnly));
                        //    dsTemp.Merge(new DAL837().Load837ServiceLineSVDByBatchId(batchId));
                        //    ds_list = GetHCFAClaimsList(ref dsTemp); //get Claim vise list for HCFA.
                        //}
                        //else
                        //{
                        //    foreach (var visitId in VisitIds)
                        //    {
                        //        DSHCFA dsTemp = new DSHCFA();
                        //        dsTemp = new DAL837().Load837Claim(visitId, 0);
                        //        dsTemp.Merge(new DAL837().Load837NamesByVisitId(visitId));
                        //        dsTemp.Merge(new DAL837().Load837ServiceLine(visitId, 0, ViewOnly));
                        //        dsTemp.Merge(new DAL837().Load837ServiceLineSVDByVisitId(visitId));
                        //        ds_list.Add(dsTemp);
                        //    }
                        //}
                    }

                    if (dsBatch != null)
                    {
                        dsBatch._837Batch.Rows[0][dsBatch._837Batch.BatchStatusColumn] = "Submitted";
                        dsBatch._837Batch.Rows[0][dsBatch._837Batch.SendStatusColumn] = "S";
                        dsBatch._837Batch.Rows[0][dsBatch._837Batch.IsCompletedColumn] = true;

                        //Updated Batch Status
                        dsBatch = new DAL837().Update837Batch(dsBatch);

                        // Update Patient Visit and Charge Status
                        string objUpdate = new DALVisits().UpdatePatientVisitsAndChargesStatusUpdate(batchId, 2);
                        if (!string.IsNullOrEmpty(objUpdate))
                            throw (new Exception(objUpdate));
                    }
                }
                catch (Exception ex)
                {
                    if (dsBatch != null)
                    {
                        // roll back all process.
                        new DAL837().DeleteEDI837Batch(batchId);
                    }

                    MDVLogger.BLLErrorLog("BLLBillingClaim::PrintClaim", ex);
                    return new BLObject<List<DSHCFA>>(null, ex.Message);
                }

                if (MarkSubmitted == false)
                    return new BLObject<List<DSHCFA>>(GetHCFA(ds_list));
                else
                    return new BLObject<List<DSHCFA>>(null, "Success");
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::PrintClaim", ex);
                return new BLObject<List<DSHCFA>>(null, ex.Message);
            }
        }

        //public BLObject<string> CreateClaimXML(DSHCFA dsHCFA)
        //{
        //    try
        //    {
        //        DSHCFA dsTemp = new DSHCFA();
        //        dsTemp.Merge(dsHCFA.HCFACharges);
        //        dsTemp.Merge(dsHCFA.HCFAClaims);

        //        string xml_string = null;
        //        using (var stream = new MemoryStream())
        //        {
        //            StreamWriter xmlDoc = new StreamWriter(stream);
        //            dsTemp.Namespace = "";
        //            dsTemp.WriteXml(xmlDoc, XmlWriteMode.IgnoreSchema);

        //            stream.Position = 0;

        //            using (var streamReader = new StreamReader(stream))
        //            {
        //                xml_string = streamReader.ReadToEnd();
        //            }

        //            xmlDoc.Close();
        //        }

        //        return new BLObject<string>(xml_string);
        //    }
        //    catch (Exception ex)
        //    {

        //        MDVLogger.BLLErrorLog("BLLBillingClaim::CreateClaimXML", ex);
        //        return new BLObject<string>(null, ex.Message);
        //    }
        //}

        /// <summary>
        /// Gets the hcfa.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        ///

        public List<DSHCFA> GetHCFA(List<DSHCFA> ds_List)
        {
            List<DSHCFA> res_list = new List<DSHCFA>();
            foreach (DSHCFA ds in ds_List)
            {
                //Fill Claims
                DSHCFA dsTemp = new DSHCFA();
                dsTemp = ds;
                DSHCFA.HCFAClaimsRow drclaim = ds.HCFAClaims.NewHCFAClaimsRow();
                EDI837Parser Parser = new EDI837Parser(ref dsTemp);
                DSHCFA._837NameRow Subscriber = null;
                DSHCFA._837NameRow[] OtherSubscriber = null;
                DSHCFA._837NameRow OtherSubscriberFirst = null;
                DSHCFA._837NameRow OtherSubscriberSecond = null;
                DSHCFA._837NameRow Patient = null;
                DSHCFA._837NameRow ReferringProvider = null;
                DSHCFA._837NameRow RenderingProvider = null;
                DSHCFA._837NameRow ServiceLocation = null;
                DSHCFA._837NameRow Provider = null;
                DSHCFA._837NameRow ProviderPayToAddress = null;
                DSHCFA._837NameRow SupervisingProvider = null;
                DSHCFA._837NameRow Payer = null;
                DSHCFA._837ClaimRow Claim = null;

                #region " Name "

                if (ds._837Claim.Rows.Count > 0)
                {
                    Claim = (DSHCFA._837ClaimRow)ds._837Claim.Rows[0];
                }

                //FILL claim from Name
                if (ds._837Name.Rows.Count > 0)
                {
                    long VisitId = MDVUtility.ToLong(ds._837Name.Rows[0][ds._837Name.VisitIdColumn.ColumnName]);
                    long VisitPriority = MDVUtility.ToLong(ds._837Name.Rows[0][ds._837Name.OriginalPriorityColumn.ColumnName]);

                    #region " Payer "

                    Payer = Parser.GetPayer(VisitId, VisitPriority);

                    if (Payer != null)
                    {

                        if (Payer.InsDescription.Length > 35)
                            drclaim.HCFA_Payer_1 = Payer.InsDescription.Remove(35);
                        else
                            drclaim.HCFA_Payer_1 = Payer.InsDescription;


                        if (Payer.N301.Length > 35)
                            drclaim.HCFA_Payer_2 = Payer.N301.Remove(35);
                        else
                            drclaim.HCFA_Payer_2 = Payer.N301;

                        if (Payer.N302.Length > 35)
                            drclaim.HCFA_Payer_3 = Payer.N302.Remove(35);
                        else
                            drclaim.HCFA_Payer_3 = Payer.N302;

                        string payer_4 = Payer.N401 + " " + Payer.N402 + " " + Payer.N403;

                        if (payer_4.Length > 30)
                            drclaim.HCFA_Payer_4 = payer_4.Remove(30);
                        else
                            drclaim.HCFA_Payer_4 = payer_4;

                    }

                    #endregion

                    #region " Subscriber "

                    Subscriber = Parser.GetSubscriber(VisitId, VisitPriority);

                    if (Subscriber != null)
                    {
                        string Plan = MDVUtility.ToStr(Subscriber.SBR09);
                        switch (Plan)
                        {
                            case "CH":
                                {
                                    drclaim.HCFA_1_A = "false";
                                    drclaim.HCFA_1_B = "false";
                                    drclaim.HCFA_1_C = "true";
                                    drclaim.HCFA_1_D = "false";
                                    drclaim.HCFA_1_E = "false";
                                    drclaim.HCFA_1_F = "false";
                                    drclaim.HCFA_1_G = "false";
                                }
                                break;
                            case "MB":
                                {
                                    drclaim.HCFA_1_A = "true";
                                    drclaim.HCFA_1_B = "false";
                                    drclaim.HCFA_1_C = "false";
                                    drclaim.HCFA_1_D = "false";
                                    drclaim.HCFA_1_E = "false";
                                    drclaim.HCFA_1_F = "false";
                                    drclaim.HCFA_1_G = "false";
                                }
                                break;
                            case "MC":
                                {
                                    drclaim.HCFA_1_A = "false";
                                    drclaim.HCFA_1_B = "true";
                                    drclaim.HCFA_1_C = "false";
                                    drclaim.HCFA_1_D = "false";
                                    drclaim.HCFA_1_E = "false";
                                    drclaim.HCFA_1_F = "false";
                                    drclaim.HCFA_1_G = "false";
                                }
                                break;
                            case "VA":
                                {
                                    drclaim.HCFA_1_A = "false";
                                    drclaim.HCFA_1_B = "false";
                                    drclaim.HCFA_1_C = "false";
                                    drclaim.HCFA_1_D = "true";
                                    drclaim.HCFA_1_E = "false";
                                    drclaim.HCFA_1_F = "false";
                                    drclaim.HCFA_1_G = "false";
                                }
                                break;
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                            case "16":
                            case "BL":
                            //case "CI":
                            case "HM":
                                {
                                    drclaim.HCFA_1_A = "false";
                                    drclaim.HCFA_1_B = "false";
                                    drclaim.HCFA_1_C = "false";
                                    drclaim.HCFA_1_D = "false";
                                    drclaim.HCFA_1_E = "false";
                                    drclaim.HCFA_1_F = "true";
                                    drclaim.HCFA_1_G = "false";
                                }
                                break;
                            default:
                                {
                                    drclaim.HCFA_1_A = "false";
                                    drclaim.HCFA_1_B = "false";
                                    drclaim.HCFA_1_C = "false";
                                    drclaim.HCFA_1_D = "false";
                                    drclaim.HCFA_1_E = "false";
                                    drclaim.HCFA_1_F = "false";
                                    drclaim.HCFA_1_G = "true";
                                }
                                break;
                        }




                        drclaim.HCFA_1A = MDVUtility.ToStr(Subscriber.NM109);

                        drclaim.HCFA_4 = MDVUtility.ToStr(Subscriber.NM103);

                        if (!string.IsNullOrEmpty(Subscriber.NM104))
                            drclaim.HCFA_4 += ", " + MDVUtility.ToStr(Subscriber.NM104);

                        if (!string.IsNullOrEmpty(Subscriber.NM105))
                            drclaim.HCFA_4 += ", " + MDVUtility.ToStr(Subscriber.NM105);


                        string Relation = MDVUtility.ToStr(Subscriber.SBR02);
                        switch (Relation)
                        {
                            case "18":
                                {
                                    drclaim.HCFA_6_A = "true";
                                    drclaim.HCFA_6_B = "false";
                                    drclaim.HCFA_6_C = "false";
                                    drclaim.HCFA_6_D = "false";
                                }
                                break;
                            case "01":
                                {
                                    drclaim.HCFA_6_A = "false";
                                    drclaim.HCFA_6_B = "true";
                                    drclaim.HCFA_6_C = "false";
                                    drclaim.HCFA_6_D = "false";
                                }
                                break;
                            case "19":
                                {
                                    drclaim.HCFA_6_A = "false";
                                    drclaim.HCFA_6_B = "false";
                                    drclaim.HCFA_6_C = "true";
                                    drclaim.HCFA_6_D = "false";
                                }
                                break;
                            default:
                                {
                                    drclaim.HCFA_6_A = "false";
                                    drclaim.HCFA_6_B = "false";
                                    drclaim.HCFA_6_C = "false";
                                    drclaim.HCFA_6_D = "true";
                                }
                                break;
                        }

                        drclaim.HCFA_7 = MDVUtility.ToStr(Subscriber.N301) + " " + MDVUtility.ToStr(Subscriber.N302);

                        if (drclaim.HCFA_7.Length > 35)
                            drclaim.HCFA_7 = drclaim.HCFA_7.Remove(35);

                        drclaim.HCFA_7_A = MDVUtility.ToStr(Subscriber.N401);
                        drclaim.HCFA_7_B = MDVUtility.ToStr(Subscriber.N402);
                        drclaim.HCFA_7_C = MDVUtility.ToStr(Subscriber.N403);

                        if (drclaim.HCFA_7_C.Length >= 9)
                            drclaim.HCFA_7_C = drclaim.HCFA_7_C.Insert(5, "-");

                        string HCFA_7_D = MDVUtility.ToStr(Subscriber.N404);
                        if (HCFA_7_D.Contains("(") || HCFA_7_D.Contains(")"))
                        {
                            drclaim.HCFA_7_D = HCFA_7_D.Replace(")", "  ").Replace("(", " ");
                        }
                        else if (HCFA_7_D.Length >= 6)
                        {
                            drclaim.HCFA_7_D = HCFA_7_D.Insert(6, "-").Insert(3, "  ").Insert(0, "  ");
                        }
                        else
                            drclaim.HCFA_7_D = HCFA_7_D;

                        drclaim.HCFA_11 = MDVUtility.ToStr(Subscriber.SBR03);

                        drclaim.HCFA_11_A_MM = GetHcfaMonth(Subscriber.DMG02);
                        drclaim.HCFA_11_A_DD = GetHcfaDay(Subscriber.DMG02);
                        drclaim.HCFA_11_A_YY = GetHcfaYear(Subscriber.DMG02);

                        drclaim.HCFA_11_A_M = false;
                        drclaim.HCFA_11_A_F = false;
                        if (Subscriber.DMG03.ToUpper() == "M")
                            drclaim.HCFA_11_A_M = true;
                        else if (Subscriber.DMG03.ToUpper() == "F")
                            drclaim.HCFA_11_A_F = true;

                        drclaim.HCFA_11_C = MDVUtility.ToStr(Subscriber.SBR04);
                        drclaim.HCFA_11_D = "false";

                    }


                    #endregion

                    #region " Other Subscriber "

                    OtherSubscriber = Parser.GetOtherSubscriber(VisitId, VisitPriority);

                    if (OtherSubscriber != null)
                    {
                        if (OtherSubscriber.Count() == 1)
                            OtherSubscriberFirst = OtherSubscriber[0];
                        else
                        {
                            if (Subscriber.SBR01 != "P")
                            {
                                foreach (var item in OtherSubscriber)
                                {
                                    if (item.SBR01 == "P")
                                    {
                                        OtherSubscriberSecond = item;
                                        break;
                                    }
                                }
                            }
                        }

                        if (OtherSubscriberFirst != null)
                        {
                            drclaim.HCFA_11_D = "true";

                            drclaim.HCFA_9 = MDVUtility.ToStr(OtherSubscriberFirst.NM103)
                                                           + " " + MDVUtility.ToStr(OtherSubscriberFirst.NM104)
                                                           + " " + MDVUtility.ToStr(OtherSubscriberFirst.NM105);

                            drclaim.HCFA_9_A = MDVUtility.ToStr(OtherSubscriberFirst.SBR03);
                            drclaim.HCFA_9_D = MDVUtility.ToStr(OtherSubscriberFirst.SBR04);

                            //if (!string.IsNullOrEmpty(OtherSubscriberFirst.AMT02))
                            //{
                            //    string[] PaidAmount = OtherSubscriberFirst.AMT02.Split('.');
                            //    drclaim.HCFA_29_A = PaidAmount[0];

                            //    if (PaidAmount.Length > 1)
                            //        drclaim.HCFA_29_B = PaidAmount[1];
                            //    else
                            //        drclaim.HCFA_29_B = "00";

                            //}
                            //else
                            //{
                            //    drclaim.HCFA_29_A = "00";
                            //    drclaim.HCFA_29_B = "00";
                            //}
                        }

                        if (OtherSubscriberSecond != null)
                        {
                            drclaim.HCFA_11_D = "true";

                            drclaim.HCFA_9 = MDVUtility.ToStr(OtherSubscriberSecond.NM103)
                                                           + " " + MDVUtility.ToStr(OtherSubscriberSecond.NM104)
                                                           + " " + MDVUtility.ToStr(OtherSubscriberSecond.NM105);

                            drclaim.HCFA_9_A = MDVUtility.ToStr(OtherSubscriberSecond.SBR03);
                            drclaim.HCFA_9_D = MDVUtility.ToStr(OtherSubscriberSecond.SBR04);
                        }

                    }


                    #endregion

                    #region " Patient "

                    Patient = Parser.GetPatient(VisitId);

                    if (Patient != null)
                    {
                        drclaim.HCFA_2 = MDVUtility.ToStr(Patient.NM103);

                        if (!string.IsNullOrEmpty(Patient.NM104))
                            drclaim.HCFA_2 += ", " + MDVUtility.ToStr(Patient.NM104);

                        if (!string.IsNullOrEmpty(Patient.NM105))
                            drclaim.HCFA_2 += ", " + MDVUtility.ToStr(Patient.NM105);

                        drclaim.HCFA_3_MM = GetHcfaMonth(Patient.DMG02);
                        drclaim.HCFA_3_DD = GetHcfaDay(Patient.DMG02);
                        drclaim.HCFA_3_YY = GetHcfaYear(Patient.DMG02);

                        drclaim.HCFA_3_F = false;
                        drclaim.HCFA_3_M = false;
                        if (MDVUtility.ToStr(Patient.DMG03).ToUpper().Trim() == "M")
                            drclaim.HCFA_3_M = true;
                        else
                            drclaim.HCFA_3_F = true;

                        drclaim.HCFA_5 = MDVUtility.ToStr(Patient.N301) + " " + MDVUtility.ToStr(Patient.N302);

                        if (drclaim.HCFA_5.Length > 35)
                            drclaim.HCFA_5 = drclaim.HCFA_5.Remove(35);

                        drclaim.HCFA_5_A = MDVUtility.ToStr(Patient.N401);
                        drclaim.HCFA_5_B = MDVUtility.ToStr(Patient.N402);
                        drclaim.HCFA_5_C = MDVUtility.ToStr(Patient.N403);

                        if (drclaim.HCFA_5_C.Length >= 9)
                            drclaim.HCFA_5_C = drclaim.HCFA_5_C.Insert(5, "-");

                        //drclaim.HCFA_5_D = MDVUtility.ToStr(Patient.N404).Replace(")", "  ").Replace("(", " ");
                        string HCFA_5_D = MDVUtility.ToStr(Patient.N404);
                        if (HCFA_5_D.Contains("(") || HCFA_5_D.Contains(")"))
                        {
                            drclaim.HCFA_5_D = HCFA_5_D.Replace(")", "  ").Replace("(", " ");
                        }
                        else if (HCFA_5_D.Length >= 6)
                        {
                            drclaim.HCFA_5_D = HCFA_5_D.Insert(6, "-").Insert(3, "  ").Insert(0, "  ");
                        }
                        else
                            drclaim.HCFA_5_D = HCFA_5_D;
                    }


                    #endregion

                    #region " ReferringProvider "

                    ReferringProvider = Parser.GetReferringProvider(VisitId);

                    if (ReferringProvider != null)
                    {
                        drclaim.HCFA_17_QUAL = ReferringProvider.NM101;
                        drclaim.HCFA_17 = MDVUtility.ToStr(ReferringProvider.NM103)
                                                       + " " + MDVUtility.ToStr(ReferringProvider.NM104)
                                                       + " " + MDVUtility.ToStr(ReferringProvider.NM105);

                        drclaim.HCFA_17B = MDVUtility.ToStr(ReferringProvider.NM109);

                    }
                    else
                    {
                        SupervisingProvider = Parser.GetSupervisingProvider(VisitId);
                        if (SupervisingProvider != null)
                        {
                            drclaim.HCFA_17_QUAL = SupervisingProvider.NM101;
                            drclaim.HCFA_17 = MDVUtility.ToStr(SupervisingProvider.NM103)
                                                     + " " + MDVUtility.ToStr(SupervisingProvider.NM104)
                                                     + " " + MDVUtility.ToStr(SupervisingProvider.NM105);

                            drclaim.HCFA_17B = MDVUtility.ToStr(SupervisingProvider.NM109);

                        }
                    }


                    #endregion

                    #region " RenderingProvider "

                    RenderingProvider = Parser.GetRenderingProvider(VisitId);

                    if (Claim != null)
                    {
                        if (Claim.CLM06 == "Y")
                        {
                            drclaim.HCFA_31_A_1 = MDVUtility.ToStr(RenderingProvider.NM104);

                            if (!string.IsNullOrEmpty(RenderingProvider.NM103))
                                drclaim.HCFA_31_A_1 += " " + MDVUtility.ToStr(RenderingProvider.NM103);

                            if (!string.IsNullOrEmpty(RenderingProvider.Qualification))
                                drclaim.HCFA_31_A_1 += " " + MDVUtility.ToStr(RenderingProvider.Qualification);


                            if (drclaim.HCFA_31_A_1.Length > 26)
                            {
                                string line1 = drclaim.HCFA_31_A_1.Remove(26);
                                string line2 = drclaim.HCFA_31_A_1.Substring(26);
                                if (!string.IsNullOrEmpty(line2))
                                {
                                    if (line2.Length > 20)
                                        line1 += "\n" + line2.Remove(20);
                                    else
                                        line1 += "\n" + line2;
                                }

                                drclaim.HCFA_31_A_1 = line1;

                            }

                            //drclaim.HCFA_31_A_2 = "SIGNATURE ON FILE";
                            drclaim.HCFA_31_B = DateTime.Now.ToString(MDVSession.Current.DateFormat.Replace("mm", "MM"));
                        }
                    }

                    #endregion

                    #region " ServiceLocation "

                    ServiceLocation = Parser.GetServiceLocation(VisitId);

                    if (ServiceLocation != null)
                    {
                        string Description = MDVUtility.ToStr(ServiceLocation.NM103);
                        string Address = MDVUtility.ToStr(ServiceLocation.N301);

                        if (Description.Length > 35)
                            Description = Description.Remove(35);

                        if (Address.Length > 35)
                            Address = Address.Remove(35);

                        drclaim.HCFA_32_A_1 = Description + " \n" + Address;

                        drclaim.HCFA_32_A_2 = MDVUtility.ToStr(ServiceLocation.N401)
                                                    + " " + MDVUtility.ToStr(ServiceLocation.N402)
                                                    + " " + MDVUtility.ToStr(ServiceLocation.N403);

                        if (drclaim.HCFA_32_A_2.Length > 35)
                            drclaim.HCFA_32_A_2 = drclaim.HCFA_32_A_2.Remove(35);

                        drclaim.HCFA_32_B = MDVUtility.ToStr(ServiceLocation.NM109);
                    }

                    #endregion

                    #region " Provider "

                    Provider = Parser.GetProvider(VisitId);
                    ProviderPayToAddress = Parser.GetPayToAddressProvider(VisitId);

                    if (Provider != null)
                    {

                        drclaim.HCFA_25_EIN = false;
                        drclaim.HCFA_25_SSN = false;

                        if (MDVUtility.ToStr(Provider.REF01) != "")
                        {
                            if (MDVUtility.ToStr(Provider.REF01) == "EI")
                                drclaim.HCFA_25_EIN = true;
                            else if (MDVUtility.ToStr(Provider.REF01) == "SY")
                                drclaim.HCFA_25_SSN = true;

                            //for 25
                            drclaim.HCFA_25 = MDVUtility.ToStr(Provider.REF02);
                            //PRD-44
                            // for Box 24 'I' & 'J' Shaded set value from claim submisson form 
                            if (Claim.Box24IJShaded == 2)
                            {
                                drclaim.HCFA_24_J = MDVUtility.ToStr(Claim.TaxonomyCode);
                                drclaim.HCFA_24_I = "ZZ ";
                            }
                            else if (Claim.Box24IJShaded == 3)
                            {

                                drclaim.HCFA_24_J = "";
                                drclaim.HCFA_24_I = "";
                            }
                            else if (Claim.Box24IJShaded == 1)
                            {
                                drclaim.HCFA_24_J = MDVUtility.ToStr(Provider.REF02);
                                drclaim.HCFA_24_I = "G2 ";

                            }
                            // Box 24 'I' & 'J' Shaded is at 'select' or no value select in claim submission form then value wil be select from insurance plan form
                            else
                            {
                                if (Claim.Ins24IJ == 2)
                                {
                                    drclaim.HCFA_24_J = MDVUtility.ToStr(Claim.TaxonomyCode);
                                    drclaim.HCFA_24_I = "ZZ ";

                                }
                                else if (Claim.Ins24IJ == 3)
                                {
                                    drclaim.HCFA_24_J = "";
                                    drclaim.HCFA_24_I = "";
                                }
                                else
                                {
                                    drclaim.HCFA_24_J = MDVUtility.ToStr(Provider.REF02);
                                    drclaim.HCFA_24_I = "G2 ";
                                }

                            } //end else


                            // for Box 33 'b' Shaded set value from claim submisson form 
                            if (Claim.Box24BShaded == 2 || Claim.Box24BShaded == 3)
                            {

                                drclaim.HCFA_33 = MDVUtility.ToStr(Claim.TaxonomyCode);
                                drclaim.HCFA_33_Qualifier = "ZZ ";
                            }
                            else if (Claim.Box24BShaded == 4)
                            {

                                drclaim.HCFA_33 = "";
                                drclaim.HCFA_33_Qualifier = "";
                            }
                            else if (Claim.Box24BShaded == 1)
                            {
                                drclaim.HCFA_33 = MDVUtility.ToStr(Provider.REF02);
                                drclaim.HCFA_33_Qualifier = "G2 ";

                            }
                            // if Box 33 'b' Shaded is at 'select' or no value select in claim submission form then value wil be select from insurance plan form 
                            else
                            {
                                if (Claim.Ins33BJ == 2 || Claim.Ins33BJ == 3)
                                {
                                    drclaim.HCFA_33 = MDVUtility.ToStr(Claim.TaxonomyCode);
                                    drclaim.HCFA_33_Qualifier = "ZZ ";
                                }
                                else if (Claim.Ins33BJ == 4)
                                {
                                    drclaim.HCFA_33 = "";
                                    drclaim.HCFA_33_Qualifier = "";
                                }
                                // if there is also not set box Box 33 'b' shaded value then Tax id will be print.
                                else
                                {
                                    drclaim.HCFA_33 = MDVUtility.ToStr(Provider.REF02);
                                    drclaim.HCFA_33_Qualifier = "G2 ";
                                }
                            }



                        }

                        if (ProviderPayToAddress != null)
                        {
                            drclaim.HCFA_33_A_1 = MDVUtility.ToStr(Provider.NM103)
                                          + " " + MDVUtility.ToStr(Provider.NM104)
                                          + " " + MDVUtility.ToStr(Provider.NM105)
                                          + "\n" + MDVUtility.ToStr(ProviderPayToAddress.N301);
                        }
                        else
                        {
                            drclaim.HCFA_33_A_1 = MDVUtility.ToStr(Provider.NM103)
                                          + " " + MDVUtility.ToStr(Provider.NM104)
                                          + " " + MDVUtility.ToStr(Provider.NM105)
                                          + "\n" + MDVUtility.ToStr(Provider.N301);
                        }




                        if (ProviderPayToAddress != null)
                        {
                            drclaim.HCFA_33_A_2 = MDVUtility.ToStr(ProviderPayToAddress.N401)
                                         + " " + MDVUtility.ToStr(ProviderPayToAddress.N402)
                                         + " " + MDVUtility.ToStr(ProviderPayToAddress.N403);
                        }
                        else
                        {
                            drclaim.HCFA_33_A_2 = MDVUtility.ToStr(Provider.N401)
                                         + " " + MDVUtility.ToStr(Provider.N402)
                                         + " " + MDVUtility.ToStr(Provider.N403);
                        }



                        drclaim.HCFA_33_B = MDVUtility.ToStr(Provider.NM109);


                        string HCFA_33_C = MDVUtility.ToStr(Provider.PER04);
                        if (HCFA_33_C.Contains("(") || HCFA_33_C.Contains(")"))
                        {
                            drclaim.HCFA_33_C = HCFA_33_C.Replace(")", "  ").Replace("(", " ");
                        }
                        else if (HCFA_33_C.Length >= 6)
                        {
                            drclaim.HCFA_33_C = HCFA_33_C.Insert(6, "-").Insert(3, "  ").Insert(0, "  ");
                        }
                        else
                            drclaim.HCFA_33_C = HCFA_33_C;
                    }

                    #endregion
                }

                #endregion

                #region " Claim "

                //Fill claim from Claim
                if (Claim != null)
                {
                    if (Claim.CLM11_1A == "EM")
                        drclaim.HCFA_10_A = true;
                    else
                        drclaim.HCFA_10_A = false;

                    if (Claim.CLM11_1B == "AA")
                        drclaim.HCFA_10_B = true;
                    else
                        drclaim.HCFA_10_B = false;

                    if (Claim.CLM11_1C == "OA")
                        drclaim.HCFA_10_C = true;
                    else
                        drclaim.HCFA_10_C = false;

                    if (drclaim.HCFA_10_B)
                        drclaim.HCFA_10_B_STATE = MDVUtility.ToStr(Claim.CLM11_4);

                    if (Claim.CLM09 == "Y")
                        drclaim.HCFA_12_A = "SIGNATURE ON FILE";

                    drclaim.HCFA_12_B = MDVUtility.ToStr(DateTime.Now.ToString(MDVSession.Current.DateFormat.Replace("mm", "MM")));


                    if (Claim.CLM08 == "Y")
                        drclaim.HCFA_13 = "SIGNATURE ON FILE";

                    bool IsHCFA14 = false;
                    if (!string.IsNullOrEmpty(Claim.DTP03_B))
                    {
                        //if DTP03_B Date is LMP Date then Patient gender will checked as Female.
                        if (Claim.DTP01_B == "484" && drclaim.HCFA_3_F)
                        {
                            drclaim.HCFA_14_MM = GetHcfaMonth(Claim.DTP03_B);
                            drclaim.HCFA_14_DD = GetHcfaDay(Claim.DTP03_B);
                            drclaim.HCFA_14_YY = GetHcfaYear(Claim.DTP03_B);
                            drclaim.HCFA_14_QUAL = Claim.DTP01_B;
                            IsHCFA14 = true;
                        }
                        else if (Claim.DTP01_B != "484")
                        {
                            drclaim.HCFA_14_MM = GetHcfaMonth(Claim.DTP03_B);
                            drclaim.HCFA_14_DD = GetHcfaDay(Claim.DTP03_B);
                            drclaim.HCFA_14_YY = GetHcfaYear(Claim.DTP03_B);
                            drclaim.HCFA_14_QUAL = Claim.DTP01_B;
                            IsHCFA14 = true;
                        }

                        if (!string.IsNullOrEmpty(Claim.DTP03_A))
                        {
                            if (IsHCFA14)
                            {
                                drclaim.HCFA_15_MM = GetHcfaMonth(Claim.DTP03_A);
                                drclaim.HCFA_15_DD = GetHcfaDay(Claim.DTP03_A);
                                drclaim.HCFA_15_YY = GetHcfaYear(Claim.DTP03_A);
                                drclaim.HCFA_15_QUAL = Claim.DTP01_A;
                            }
                            else
                            {
                                drclaim.HCFA_14_MM = GetHcfaMonth(Claim.DTP03_A);
                                drclaim.HCFA_14_DD = GetHcfaDay(Claim.DTP03_A);
                                drclaim.HCFA_14_YY = GetHcfaYear(Claim.DTP03_A);
                                drclaim.HCFA_14_QUAL = Claim.DTP01_A;
                            }
                        }

                    }
                    else if (!string.IsNullOrEmpty(Claim.DTP03_A))
                    {
                        drclaim.HCFA_14_MM = GetHcfaMonth(Claim.DTP03_A);
                        drclaim.HCFA_14_DD = GetHcfaDay(Claim.DTP03_A);
                        drclaim.HCFA_14_YY = GetHcfaYear(Claim.DTP03_A);
                        drclaim.HCFA_14_QUAL = Claim.DTP01_A;

                    }


                    string[] temp = Claim.DTP03_C.Split('-');
                    if (temp.Length > 0)
                    {
                        drclaim.HCFA_16_A_DD = GetHcfaDay(temp[0]);
                        drclaim.HCFA_16_A_MM = GetHcfaMonth(temp[0]);
                        drclaim.HCFA_16_A_YY = GetHcfaYear(temp[0]);

                        drclaim.HCFA_16_B_DD = drclaim.HCFA_16_A_DD;
                        drclaim.HCFA_16_B_MM = drclaim.HCFA_16_A_MM;
                        drclaim.HCFA_16_B_YY = drclaim.HCFA_16_A_YY;
                    }
                    if (temp.Length > 1)
                    {
                        drclaim.HCFA_16_B_DD = GetHcfaDay(temp[1]);
                        drclaim.HCFA_16_B_MM = GetHcfaMonth(temp[1]);
                        drclaim.HCFA_16_B_YY = GetHcfaYear(temp[1]);
                    }

                    drclaim.HCFA_18_A_MM = GetHcfaMonth(Claim.DTP03_D);
                    drclaim.HCFA_18_A_DD = GetHcfaDay(Claim.DTP03_D);
                    drclaim.HCFA_18_A_YY = GetHcfaYear(Claim.DTP03_D);

                    drclaim.HCFA_18_B_MM = GetHcfaMonth(Claim.DTP03_E);
                    drclaim.HCFA_18_B_DD = GetHcfaDay(Claim.DTP03_E);
                    drclaim.HCFA_18_B_YY = GetHcfaYear(Claim.DTP03_E);

                    drclaim.HCFA_19 = Claim.NTE02;

                    ////////////////////////
                    if (drclaim.HCFA_19.Length > 65)
                        drclaim.HCFA_19 = drclaim.HCFA_19.Remove(65);

                    if (!string.IsNullOrEmpty(Claim.REF02_PayerClmCtrlNo) && !string.IsNullOrEmpty(Claim.CLM05_3) && Claim.CLM05_3 != "1")
                    {
                        drclaim.HCFA_22_A = MDVUtility.ToStr(Claim.CLM05_3);
                        drclaim.HCFA_22_B = MDVUtility.ToStr(Claim.REF02_PayerClmCtrlNo);
                    }

                    drclaim.HCFA_23 = MDVUtility.ToStr(Claim.REF02_PRIOR_AUTH);
                    drclaim.HCFA_26 = MDVUtility.ToStr(Claim.CLM01);

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(Claim.REF02_CLIA)))
                    {
                        if (string.IsNullOrEmpty(drclaim.HCFA_23))
                            drclaim.HCFA_23 = MDVUtility.ToStr(Claim.REF02_CLIA);
                        else if (string.IsNullOrEmpty(drclaim.HCFA_19))
                            drclaim.HCFA_19 = MDVUtility.ToStr(Claim.REF02_CLIA);
                        else
                        {
                            if (drclaim.HCFA_19.Length > 54)
                                drclaim.HCFA_19 = drclaim.HCFA_19.Remove(54);

                            drclaim.HCFA_19 += ", " + MDVUtility.ToStr(Claim.REF02_CLIA);
                        }

                    }





                    if (Claim.CLM07.ToUpper() == "Y")
                        drclaim.HCFA_27 = true;
                    else
                        drclaim.HCFA_27 = false;

                    //if (!string.IsNullOrEmpty(Claim.CLM02))
                    //{
                    //    string[] fee = Claim.CLM02.Split('.');
                    //    drclaim.HCFA_28_A = fee[0];

                    //    if (fee.Length > 1)
                    //        drclaim.HCFA_28_B = fee[1];
                    //    else
                    //        drclaim.HCFA_28_B = "00";

                    //}

                    // check Is there is any Service line that have Prior Auth. Number
                    if (string.IsNullOrEmpty(drclaim.HCFA_23))
                    {

                        DSHCFA._837ServiceLineRow[] rows = (DSHCFA._837ServiceLineRow[])ds._837ServiceLine.Select(ds._837ServiceLine.REF02_PANColumn.ColumnName + " <> ''", ds._837ServiceLine.CHARGE_ORDERColumn.ColumnName);
                        foreach (DSHCFA._837ServiceLineRow item in rows)
                        {
                            drclaim.HCFA_23 = item.REF02_PAN;
                            break;
                        }
                    }
                }

                ds.HCFAClaims.AddHCFAClaimsRow(drclaim);

                #endregion

                #region " Service Line "

                //char icd_number = 'A';
                bool IsICD10 = false;
                //Dictionary<string, string> ICD = new Dictionary<string, string>();

                //Fill claim from Service Line
                foreach (DSHCFA._837ServiceLineRow dr in ds._837ServiceLine.Rows)
                {
                    DSHCFA.HCFAChargesRow drcharges = ds.HCFACharges.NewHCFAChargesRow();

                    //IsICD10 = dr.IsICD10;

                    //Get Single Charge Paid Amount.
                    float PaidAmount = 0;
                    PaidAmount = GetServiceLinePaidAmount(MDVUtility.ToLong(dr.VisitId), MDVUtility.ToLong(dr.ChargeId), MDVUtility.ToStr(dr.SV101_2), ds._837SVDServiceLine);

                    if (PaidAmount > 0)
                    {
                        string[] amount = MDVUtility.ToStr(PaidAmount).Split('.');
                        drcharges.HCFA_29_A = amount[0];

                        if (amount.Length > 1)
                            drcharges.HCFA_29_B = amount[1];
                        else
                            drcharges.HCFA_29_B = "00";
                    }
                    else
                    {
                        drcharges.HCFA_29_A = "00";
                        drcharges.HCFA_29_B = "00";
                    }


                    #region" ICD Pointers "

                    if (!string.IsNullOrEmpty(dr.ICDPointer1))
                        drcharges.HCFA_24_E += getServiceLinePointer(MDVUtility.ToInt(dr.ICDPointer1));
                    if (!string.IsNullOrEmpty(dr.ICDPointer2))
                        drcharges.HCFA_24_E += getServiceLinePointer(MDVUtility.ToInt(dr.ICDPointer2));
                    if (!string.IsNullOrEmpty(dr.ICDPointer3))
                        drcharges.HCFA_24_E += getServiceLinePointer(MDVUtility.ToInt(dr.ICDPointer3));
                    if (!string.IsNullOrEmpty(dr.ICDPointer4))
                        drcharges.HCFA_24_E += getServiceLinePointer(MDVUtility.ToInt(dr.ICDPointer4));

                    //if (!string.IsNullOrEmpty(dr.ICDCode1))
                    //{
                    //    if (!ICD.ContainsValue(dr.ICDCode1))
                    //    {
                    //        ICD.Add(icd_number.ToString(), dr.ICDCode1);
                    //        drcharges.HCFA_24_E += icd_number.ToString();
                    //        icd_number++;
                    //    }
                    //    else
                    //    {
                    //        drcharges.HCFA_24_E += ICD.FirstOrDefault(p => p.Value == dr.ICDCode1).Key;
                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(dr.ICDCode2))
                    //{
                    //    if (!ICD.ContainsValue(dr.ICDCode2))
                    //    {
                    //        ICD.Add(icd_number.ToString(), dr.ICDCode2);
                    //        drcharges.HCFA_24_E += icd_number.ToString();
                    //        icd_number++;
                    //    }
                    //    else
                    //    {
                    //        drcharges.HCFA_24_E += ICD.FirstOrDefault(p => p.Value == dr.ICDCode2).Key;
                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(dr.ICDCode3))
                    //{
                    //    if (!ICD.ContainsValue(dr.ICDCode3))
                    //    {
                    //        ICD.Add(icd_number.ToString(), dr.ICDCode3);
                    //        drcharges.HCFA_24_E += icd_number.ToString();
                    //        icd_number++;
                    //    }
                    //    else
                    //    {
                    //        drcharges.HCFA_24_E += ICD.FirstOrDefault(p => p.Value == dr.ICDCode3).Key;
                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(dr.ICDCode4))
                    //{
                    //    if (!ICD.ContainsValue(dr.ICDCode4))
                    //    {
                    //        ICD.Add(icd_number.ToString(), dr.ICDCode4);
                    //        drcharges.HCFA_24_E += icd_number.ToString();
                    //        icd_number++;
                    //    }
                    //    else
                    //    {
                    //        drcharges.HCFA_24_E += ICD.FirstOrDefault(p => p.Value == dr.ICDCode4).Key;
                    //    }
                    //}

                    #endregion

                    string[] temp = dr.DTP03.Split('-');
                    if (temp.Length > 0)
                    {
                        drcharges.HCFA_24_A_From_DD = GetHcfaDay(temp[0]);
                        drcharges.HCFA_24_A_From_MM = GetHcfaMonth(temp[0]);
                        drcharges.HCFA_24_A_From_YY = GetHcfaYear(temp[0]);

                        drcharges.HCFA_24_A_To_DD = drcharges.HCFA_24_A_From_DD;
                        drcharges.HCFA_24_A_To_MM = drcharges.HCFA_24_A_From_MM;
                        drcharges.HCFA_24_A_To_YY = drcharges.HCFA_24_A_From_YY;
                    }
                    if (temp.Length > 1)
                    {
                        drcharges.HCFA_24_A_To_DD = GetHcfaDay(temp[1]);
                        drcharges.HCFA_24_A_To_MM = GetHcfaMonth(temp[1]);
                        drcharges.HCFA_24_A_To_YY = GetHcfaYear(temp[1]);
                    }

                    //if (Claim.CLM05_1 != dr.SV105)
                    drcharges.HCFA_24_B = MDVUtility.ToStr(dr.POSCharge);

                    drcharges.HCFA_24_C = MDVUtility.ToStr(dr.SV109);


                    drcharges.HCFA_24_D_CPT = MDVUtility.ToStr(dr.SV101_2);

                    drcharges.HCFA_24_D_MOD_1 = MDVUtility.ToStr(dr.SV101_3);
                    drcharges.HCFA_24_D_MOD_2 = MDVUtility.ToStr(dr.SV101_4);
                    drcharges.HCFA_24_D_MOD_3 = MDVUtility.ToStr(dr.SV101_5);
                    drcharges.HCFA_24_D_MOD_4 = MDVUtility.ToStr(dr.SV101_6);

                    // drcharges.HCFA_24_E = MDVUtility.ToStr(dr.SV107);

                    if (!string.IsNullOrEmpty(dr.SV102))
                    {
                        string[] fee = dr.SV102.Split('.');
                        drcharges.HCFA_24_F_A = fee[0];

                        if (fee.Length > 1)
                            drcharges.HCFA_24_F_B = fee[1];
                        else
                            drcharges.HCFA_24_F_B = "00";
                    }

                    drcharges.HCFA_24_G = MDVUtility.ToStr(dr.SV104);


                    if (RenderingProvider != null && RenderingProvider.IsReportNPI == "True")
                    {
                        drcharges.HCFA_24_J = MDVUtility.ToStr(RenderingProvider.NM109);
                    }
                    else
                    {
                        drcharges.HCFA_24_J = "";
                    }


                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr.LIN03)))
                    {
                        drcharges.NDC = MDVUtility.ToStr(dr.LIN02) + MDVUtility.ToStr(dr.LIN03);
                        drcharges.NDCUnit = MDVUtility.ToStr(dr.CTP04);
                        drcharges.NDCMeasurementCode = MDVUtility.ToStr(dr.CTP05);
                    }
                    //change against bug # PMS-2797
                    drcharges.LineNotes = MDVUtility.ToStr(dr.LIN04);
                    int length = 0;
                    int length_ = MDVUtility.ToStr(drcharges.NDC + "  " + drcharges.NDCUnit + drcharges.NDCMeasurementCode).Length;
                    if (length_ <= 15)
                        length = 75;
                    else if (length_ <= 20)
                        length = 70;
                    else if (length_ <= 25)
                        length = 65;
                    else if (length_ <= 30)
                        length = 60;
                    else
                        length = 55;

                    if (drcharges.LineNotes.Length > length)
                        drcharges.LineNotes = drcharges.LineNotes.Remove(length);


                    if (!string.IsNullOrEmpty(dr.AnesthesiaTime))
                        drcharges.AnesthesiaTime = dr.AnesthesiaTime;



                    ds.HCFACharges.AddHCFAChargesRow(drcharges);
                }

                #region " Claim ICDS "

                DSHCFA.VisitICDsRow[] icds = (DSHCFA.VisitICDsRow[])ds.VisitICDs.Select(ds.VisitICDs.VisitIdColumn.ColumnName + "='" + Claim.VisitId + "'");

                if (icds.Length > 0)
                {
                    int counter = 1;
                    foreach (var item in icds)
                    {
                        IsICD10 = item.IsICD10;

                        if (counter == 1)
                            drclaim.HCFA_21_A = item.ICDCode;
                        else if (counter == 2)
                            drclaim.HCFA_21_B = item.ICDCode;
                        else if (counter == 3)
                            drclaim.HCFA_21_C = item.ICDCode;
                        else if (counter == 4)
                            drclaim.HCFA_21_D = item.ICDCode;
                        else if (counter == 5)
                            drclaim.HCFA_21_E = item.ICDCode;
                        else if (counter == 6)
                            drclaim.HCFA_21_F = item.ICDCode;
                        else if (counter == 7)
                            drclaim.HCFA_21_G = item.ICDCode;
                        else if (counter == 8)
                            drclaim.HCFA_21_H = item.ICDCode;
                        else if (counter == 9)
                            drclaim.HCFA_21_I = item.ICDCode;
                        else if (counter == 10)
                            drclaim.HCFA_21_J = item.ICDCode;
                        else if (counter == 11)
                            drclaim.HCFA_21_K = item.ICDCode;
                        else if (counter == 12)
                            drclaim.HCFA_21_L = item.ICDCode;

                        counter++;
                    }

                    if (IsICD10)
                        drclaim.HCFA_21_ICD = "0";
                    else
                        drclaim.HCFA_21_ICD = "9";
                }
                #endregion

                //if (ICD.Count > 0)
                //{
                //    if (IsICD10)
                //        drclaim.HCFA_21_ICD = "0";
                //    else
                //        drclaim.HCFA_21_ICD = "9";

                //    if (ICD.Count <= 12)
                //    {
                //        foreach (var item in ICD)
                //        {
                //            if (item.Key == "A")
                //                drclaim.HCFA_21_A = item.Value;
                //            else if (item.Key == "B")
                //                drclaim.HCFA_21_B = item.Value;
                //            else if (item.Key == "C")
                //                drclaim.HCFA_21_C = item.Value;
                //            else if (item.Key == "D")
                //                drclaim.HCFA_21_D = item.Value;
                //            else if (item.Key == "E")
                //                drclaim.HCFA_21_E = item.Value;
                //            else if (item.Key == "F")
                //                drclaim.HCFA_21_F = item.Value;
                //            else if (item.Key == "G")
                //                drclaim.HCFA_21_G = item.Value;
                //            else if (item.Key == "H")
                //                drclaim.HCFA_21_H = item.Value;
                //            else if (item.Key == "I")
                //                drclaim.HCFA_21_I = item.Value;
                //            else if (item.Key == "J")
                //                drclaim.HCFA_21_J = item.Value;
                //            else if (item.Key == "K")
                //                drclaim.HCFA_21_K = item.Value;
                //            else if (item.Key == "L")
                //                drclaim.HCFA_21_L = item.Value;
                //        }
                //    }
                //    else if (ICD.Count > 12)
                //    {
                //        // need some more logic :p
                //    }
                //}

                #endregion


                //to Uppercase HCFA Values.
                //MDVUtility.GetUpperLowerCaseDataSet(ds, true);

                res_list.Add(ds);

            }

            if (res_list.Count > 0)
                return res_list;
            else
                throw new Exception("No Data found.");
        }

        public string getServiceLinePointer(int IntPointer)
        {
            string Pointer = "";
            try
            {
                if (IntPointer > 0)
                {
                    int Input = 64;
                    Input = Input + IntPointer;
                    Pointer = ((char)Input).ToString();
                }

            }
            catch (Exception)
            {
                Pointer = "";
            }

            return Pointer;
        }

        public BLObject<byte[]> CreateClaimForm(List<DSHCFA> dsHCFA_List, string UserBrowser, bool IsBackground = false)
        {
            try
            {
                if (dsHCFA_List.Count <= 0)
                    throw new Exception("No Data found.");

                byte[] newByteArr = null;
                using (MemoryStream stream_ = new MemoryStream())
                {

                    Document document = new Document(PageSize.LETTER, 2, 2, 2, 0);

                    MDVUtility.PDFCreator pdf = new MDVUtility.PDFCreator(ref document, stream_, true, ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName), "Health Insurance Claim Form");
                    pdf.Document.Open();
                    foreach (DSHCFA dsHCFA in dsHCFA_List)
                    {

                        DSHCFA dsTemp = new DSHCFA();
                        dsTemp.Merge(dsHCFA.HCFACharges);
                        dsTemp.Merge(dsHCFA.HCFAClaims);

                        //UserBrowser only for blank HCFA for print , View HCFA works fine for all browsers.
                        if (IsBackground == false)
                        {
                            switch (UserBrowser)
                            {
                                case "ie":
                                    CreatePageForIE(pdf, dsTemp, IsBackground);
                                    break;
                                case "chrome":
                                    CreatePageForChrome(pdf, dsTemp, IsBackground);
                                    break;
                                default:
                                    CreatePage(pdf, dsTemp, IsBackground);
                                    break;
                            }
                        }
                        else
                            CreatePage(pdf, dsTemp, IsBackground);

                    }


                    pdf.Document.Close();
                    pdf.Writer.Close();

                    newByteArr = stream_.GetBuffer();
                }

                return new BLObject<byte[]>(newByteArr);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::CreateClaimForm", ex);
                return new BLObject<byte[]>(null, ex.Message);
            }

        }

        private void CreatePage(MDVUtility.PDFCreator pdf, DSHCFA dsTemp, bool IsBackground)
        {
            string imageFilePath = "";
            try
            {
                pdf.Document.NewPage();

                #region " Claims "

                DSHCFA.HCFAClaimsRow claims = (DSHCFA.HCFAClaimsRow)dsTemp.HCFAClaims.Rows[0];

                //Header
                pdf.WriteText(claims.HCFA_Payer_1, pdf.Column * 27 - 4.39f, pdf.RowMargin * 1.5f);
                pdf.WriteText(claims.HCFA_Payer_2, pdf.Column * 27 - 4.39f, pdf.RowMargin * 2f);
                pdf.WriteText(claims.HCFA_Payer_3, pdf.Column * 27 - 4.39f, pdf.RowMargin * 2.5f);
                pdf.WriteText(claims.HCFA_Payer_4, pdf.Column * 27 - 4.39f, pdf.RowMargin * 3f);

                //1
                if (ToBool(claims.HCFA_1_A))
                    pdf.WriteText("x", pdf.Column - 3.19f, pdf.RowMargin * 4);
                else if (ToBool(claims.HCFA_1_B))
                    pdf.WriteText("x", pdf.Column * 4 + 4.19f, pdf.RowMargin * 4);
                else if (ToBool(claims.HCFA_1_C))
                    pdf.WriteText("x", pdf.Column * 8 + 1.19f, pdf.RowMargin * 4);
                else if (ToBool(claims.HCFA_1_D))
                    pdf.WriteText("x", pdf.Column * 12 + 11.50f, pdf.RowMargin * 4);
                else if (ToBool(claims.HCFA_1_E))
                    pdf.WriteText("x", pdf.Column * 17 - 9.19f, pdf.RowMargin * 4);
                else if (ToBool(claims.HCFA_1_F))
                    pdf.WriteText("x", pdf.Column * 21 - 5.95f, pdf.RowMargin * 4);
                else if (ToBool(claims.HCFA_1_G))
                    pdf.WriteText("x", pdf.Column * 24 - 4.50f, pdf.RowMargin * 4);

                pdf.WriteText(claims.HCFA_2, pdf.Column, pdf.RowMargin * 5 - 4.39f);
                pdf.WriteText(claims.HCFA_5, pdf.Column, pdf.RowMargin * 6 - 4.39f);
                pdf.WriteText(claims.HCFA_5_A, pdf.Column, pdf.RowMargin * 7 - (2 * 2.96f));
                pdf.WriteText(claims.HCFA_5_B, pdf.Column * 14, pdf.RowMargin * 7 - (2 * 2.96f));
                pdf.WriteText(claims.HCFA_5_C, pdf.Column, pdf.RowMargin * 8 - (3 * 2.96f));


                pdf.WriteText(claims.HCFA_5_D, pdf.Column * 8 + 2f, pdf.RowMargin * 8 - (2 * 2.96f));

                //9
                pdf.WriteText(claims.HCFA_9, pdf.Column, pdf.RowMargin * 9 - (2 * 2.96f + 2.39f));
                pdf.WriteText(claims.HCFA_9_A, pdf.Column, pdf.RowMargin * 10 - (3 * 2.96f));
                pdf.WriteText(claims.HCFA_9_D, pdf.Column, pdf.RowMargin * 13 - (5 * 2.96f));



                pdf.WriteText(claims.HCFA_1A, pdf.Column * 27 - 4.39f, pdf.RowMargin * 4);
                pdf.WriteText(claims.HCFA_4, pdf.Column * 27 - 4.39f, pdf.RowMargin * 5 - 4.39f);
                pdf.WriteText(claims.HCFA_7, pdf.Column * 27 - 4.39f, pdf.RowMargin * 6 - 4.39f);
                pdf.WriteText(claims.HCFA_7_A, pdf.Column * 27 - 4.39f, pdf.RowMargin * 7 - (2 * 2.96f));
                pdf.WriteText(claims.HCFA_7_B, pdf.Column * 39f, pdf.RowMargin * 7 - (2 * 2.96f));
                pdf.WriteText(claims.HCFA_7_C, pdf.Column * 27 - 4.39f, pdf.RowMargin * 8 - (2 * 2.96f));
                pdf.WriteText(claims.HCFA_7_D, pdf.Column * 34 + 4.0f, pdf.RowMargin * 8 - (2 * 2.96f));


                //3
                pdf.WriteText(claims.HCFA_3_MM, pdf.Column * 17 - 3.39f, pdf.RowMargin * 5 + 0.96f);
                pdf.WriteText(claims.HCFA_3_DD, pdf.Column * 18 + 1.79f, pdf.RowMargin * 5 + 0.96f);
                pdf.WriteText(claims.HCFA_3_YY, pdf.Column * 19 + 6.58f, pdf.RowMargin * 5 + 0.96f);


                if (claims.HCFA_3_M)
                    pdf.WriteText("x", pdf.Column * 22 + 2.00f, pdf.RowMargin * 5 - 1f);
                else if (claims.HCFA_3_F)
                    pdf.WriteText("x", pdf.Column * 25 - 3.39f, pdf.RowMargin * 5 - 1f);

                //6
                if (ToBool(claims.HCFA_6_A))
                    pdf.WriteText("x", pdf.Column * 17 + 6.39f, pdf.RowMargin * 6 - 3.39f);
                if (ToBool(claims.HCFA_6_B))
                    pdf.WriteText("x", pdf.Column * 19 + 14.39f, pdf.RowMargin * 6 - 3.39f);
                if (ToBool(claims.HCFA_6_C))
                    pdf.WriteText("x", pdf.Column * 22 + 1.79f, pdf.RowMargin * 6 - 3.39f);
                if (ToBool(claims.HCFA_6_D))
                    pdf.WriteText("x", pdf.Column * 25 - 3.39f, pdf.RowMargin * 6 - 3.39f);


                //10a
                if (claims.HCFA_10_A)
                    pdf.WriteText("x", pdf.Column * 18 + 8.39f, pdf.RowMargin * 10 - 10.39f);
                else
                    pdf.WriteText("x", pdf.Column * 21 + 8.95f, pdf.RowMargin * 10 - 10.39f);

                //10b
                if (claims.HCFA_10_B)
                    pdf.WriteText("x", pdf.Column * 18 + 8.39f, pdf.RowMargin * 11 - (4 * 2.96f));
                else
                    pdf.WriteText("x", pdf.Column * 21 + 8.95f, pdf.RowMargin * 11 - (4 * 2.96f));

                pdf.WriteText(claims.HCFA_10_B_STATE, pdf.Column * 23 + 10.95f, pdf.RowMargin * 11 - (4 * 2.96f));

                //10c
                if (claims.HCFA_10_C)
                    pdf.WriteText("x", pdf.Column * 18 + 8.39f, pdf.RowMargin * 12 - (5 * 2.96f));
                else
                    pdf.WriteText("x", pdf.Column * 21 + 8.95f, pdf.RowMargin * 12 - (5 * 2.96f));

                //11
                pdf.WriteText(claims.HCFA_11, pdf.Column * 27 - 4.39f, pdf.RowMargin * 9 - (2 * 2.96f));

                //11a
                pdf.WriteText(claims.HCFA_11_A_MM, pdf.Column * 28 + 3.39f, pdf.RowMargin * 10 - 7.96f);
                pdf.WriteText(claims.HCFA_11_A_DD, pdf.Column * 30 - 3.39f, pdf.RowMargin * 10 - 7.96f);
                pdf.WriteText(claims.HCFA_11_A_YY, pdf.Column * 31 + 3.39f, pdf.RowMargin * 10 - 7.96f);

                if (claims.HCFA_11_A_M)
                    pdf.WriteText("x", pdf.Column * 36 - 2.79f, pdf.RowMargin * 10 - (3 * 2.96f + 1.96f));
                else if (claims.HCFA_11_A_F)
                    pdf.WriteText("x", pdf.Column * 39 + 6.39f, pdf.RowMargin * 10 - (3 * 2.96f + 1.96f));

                //11c
                pdf.WriteText(claims.HCFA_11_C, pdf.Column * 27 - 4.39f, pdf.RowMargin * 12 - (4 * 2.96f));

                //11d
                if (ToBool(claims.HCFA_11_D))
                    pdf.WriteText("x", pdf.Column * 27 + 6.39f, pdf.RowMargin * 13 - (5 * 2.96f));
                else
                    pdf.WriteText("x", pdf.Column * 30 + 0.96f, pdf.RowMargin * 13 - (5 * 2.96f));

                //12
                pdf.WriteText(claims.HCFA_12_A, pdf.Column * 3 + 1.39f, pdf.RowMargin * 15 - (7 * 2.96f));
                pdf.WriteText(claims.HCFA_12_B, pdf.Column * 19 + 10.58f, pdf.RowMargin * 15 - (7 * 2.96f));

                //13
                pdf.WriteText(claims.HCFA_13, pdf.Column * 29, pdf.RowMargin * 15 - (7 * 2.96f));

                //14
                pdf.WriteText(claims.HCFA_14_MM, pdf.Column + 6.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_14_DD, pdf.Column * 3 - 0.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_14_YY, pdf.Column * 4 + 9.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_14_QUAL, pdf.Column * 8 + 5.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));

                //15
                pdf.WriteText(claims.HCFA_15_QUAL, pdf.Column * 17 - 8.19f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_15_MM, pdf.Column * 19 + 10.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_15_DD, pdf.Column * 21 + 2.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_15_YY, pdf.Column * 22 + 14.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));

                //16a
                pdf.WriteText(claims.HCFA_16_A_MM, pdf.Column * 29 - 4.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_16_A_DD, pdf.Column * 31 - 10.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_16_A_YY, pdf.Column * 32 + 3.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));

                //16b
                pdf.WriteText(claims.HCFA_16_B_MM, pdf.Column * 36 - 0.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_16_B_DD, pdf.Column * 38 - 6.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_16_B_YY, pdf.Column * 39 + 3.39f, pdf.RowMargin * 16 - (7 * 2.96f - 2.39f));

                //17
                pdf.WriteText(claims.HCFA_17, pdf.Column * 3 - 6.19f, pdf.RowMargin * 17 - (7 * 2.96f));
                pdf.WriteText(claims.HCFA_17_QUAL, pdf.Column, pdf.RowMargin * 17 - (7 * 2.96f));

                pdf.WriteText(claims.HCFA_17B, pdf.Column * 17 + 6.19f, pdf.RowMargin * 17 - (7 * 2.96f));

                //18a
                pdf.WriteText(claims.HCFA_18_A_MM, pdf.Column * 29 - 4.39f, pdf.RowMargin * 17 - (7 * 2.96f));
                pdf.WriteText(claims.HCFA_18_A_DD, pdf.Column * 31 - 10.39f, pdf.RowMargin * 17 - (7 * 2.96f));
                pdf.WriteText(claims.HCFA_18_A_YY, pdf.Column * 32 + 3.39f, pdf.RowMargin * 17 - (7 * 2.96f));

                //18b
                pdf.WriteText(claims.HCFA_18_B_MM, pdf.Column * 36 - 0.39f, pdf.RowMargin * 17 - (7 * 2.96f));
                pdf.WriteText(claims.HCFA_18_B_DD, pdf.Column * 38 - 6.39f, pdf.RowMargin * 17 - (7 * 2.96f));
                pdf.WriteText(claims.HCFA_18_B_YY, pdf.Column * 39 + 3.39f, pdf.RowMargin * 17 - (7 * 2.96f));

                //19
                pdf.WriteText(claims.HCFA_19, pdf.Column, pdf.RowMargin * 18 - (8 * 2.96f));


                //21
                pdf.WriteText(claims.HCFA_21_ICD, pdf.Column * 21 + 17.95f, pdf.RowMargin * 18.5f - (7 * 2.96f + 2.39f));

                //21a
                pdf.WriteText(claims.HCFA_21_A, pdf.Column + 8.95f, pdf.RowMargin * 19 - (7 * 2.96f + 3.50f));
                pdf.WriteText(claims.HCFA_21_B, pdf.Column * 8 + 7.39f, pdf.RowMargin * 19 - (7 * 2.96f + 3.50f));
                pdf.WriteText(claims.HCFA_21_C, pdf.Column * 15 + 2.39f, pdf.RowMargin * 19 - (7 * 2.96f + 3.50f));
                pdf.WriteText(claims.HCFA_21_D, pdf.Column * 21 + 14.95f, pdf.RowMargin * 19 - (7 * 2.96f + 3.50f));

                //21b
                pdf.WriteText(claims.HCFA_21_E, pdf.Column + 8.95f, pdf.RowMargin * 19.5f - (7 * 2.96f + 3.50f));
                pdf.WriteText(claims.HCFA_21_F, pdf.Column * 8 + 7.39f, pdf.RowMargin * 19.5f - (7 * 2.96f + 3.50f));
                pdf.WriteText(claims.HCFA_21_G, pdf.Column * 15 + 2.39f, pdf.RowMargin * 19.5f - (7 * 2.96f + 3.50f));
                pdf.WriteText(claims.HCFA_21_H, pdf.Column * 21 + 14.95f, pdf.RowMargin * 19.5f - (7 * 2.96f + 3.50f));

                //21c
                pdf.WriteText(claims.HCFA_21_I, pdf.Column + 8.95f, pdf.RowMargin * 20f - (7 * 2.96f + 3.50f));
                pdf.WriteText(claims.HCFA_21_J, pdf.Column * 8 + 7.39f, pdf.RowMargin * 20f - (7 * 2.96f + 3.50f));
                pdf.WriteText(claims.HCFA_21_K, pdf.Column * 15 + 2.39f, pdf.RowMargin * 20f - (7 * 2.96f + 3.50f));
                pdf.WriteText(claims.HCFA_21_L, pdf.Column * 21 + 14.95f, pdf.RowMargin * 20f - (7 * 2.96f + 3.50f));

                //22
                pdf.WriteText(claims.HCFA_22_A, pdf.Column * 27 - 4.39f, pdf.RowMargin * 19 - (7 * 2.96f + 2.39f));
                pdf.WriteText(claims.HCFA_22_B, pdf.Column * 32 + 4.39f, pdf.RowMargin * 19 - (7 * 2.96f + 2.39f));

                //23
                pdf.WriteText(claims.HCFA_23, pdf.Column * 27 - 4.39f, pdf.RowMargin * 20 - (7 * 2.96f + 4.39f));

                //25
                pdf.WriteText(claims.HCFA_25, pdf.Column + 4.39f, pdf.RowMargin * 27 - (7 * 2.96f - 6.39f));
                if (claims.HCFA_25_EIN)
                    pdf.WriteText("x", pdf.Column * 10 + 2.96f, pdf.RowMargin * 27 - (7 * 2.96f - 6.39f));
                else if (claims.HCFA_25_SSN)
                    pdf.WriteText("x", pdf.Column * 9 + 2.96f, pdf.RowMargin * 27 - (7 * 2.96f - 6.39f));

                //26
                pdf.WriteText(claims.HCFA_26, pdf.Column * 13 - 4.39f, pdf.RowMargin * 27 - (7 * 2.96f - 6.39f));

                //27
                if (claims.HCFA_27)
                    pdf.WriteText("x", pdf.Column * 20 + 2.96f, pdf.RowMargin * 27 - (7 * 2.96f - 6.39f));
                else
                    pdf.WriteText("x", pdf.Column * 23 - 4.96f, pdf.RowMargin * 27 - (7 * 2.96f - 6.39f));


                //31
                pdf.WriteText(claims.HCFA_31_A_1, pdf.Column + 2.39f, pdf.RowMargin * 29 - (7 * 2.96f + 5.39f), true);
                //pdf.WriteText(claims.HCFA_31_A_2, pdf.Column + 2.39f, pdf.RowMargin * 29.5f - (7 * 2.96f + 5.39f));
                pdf.WriteText(claims.HCFA_31_B, pdf.Column * 9 - 6.96f, pdf.RowMargin * 29.5f - (7 * 2.96f + 2.39f));

                //32
                float line_32 = pdf.WriteText(claims.HCFA_32_A_1, pdf.Column * 13 - 6.39f, pdf.RowMargin * 28 - (7 * 2.96f - 4.39f), true);
                pdf.WriteText(claims.HCFA_32_A_2, pdf.Column * 13 - 6.39f, pdf.RowMargin * (28.5f + line_32) - (7 * 2.96f - 4.39f));
                pdf.WriteText(claims.HCFA_32_B, pdf.Column * 13 - 5.30f, pdf.RowMargin * 29.5f - (7 * 2.96f - 4.39f));
                //pdf.WriteText(claims.HCFA_32_B, pdf.Column * 18 + 8.39f, pdf.RowMargin * 29.5f - (7 * 2.96f - 4.39f));

                //33
                float line_33 = pdf.WriteText(claims.HCFA_33_A_1, pdf.Column * 27 - 6.39f, pdf.RowMargin * 28 - (7 * 2.96f - 4.39f), true);
                pdf.WriteText(claims.HCFA_33_A_2, pdf.Column * 27 - 6.39f, pdf.RowMargin * (28.5f + line_33) - (7 * 2.96f - 4.39f));
                pdf.WriteText(claims.HCFA_33_B, pdf.Column * 27 - 4.30f, pdf.RowMargin * 29.5f - (7 * 2.96f - 4.39f));
                pdf.WriteText(claims.HCFA_33_C, pdf.Column * 35 - 4.39f, pdf.RowMargin * 27 - 2f);


                //33b print tax id or taxonomy code
                if (IsRequired())
                {
                    if (!string.IsNullOrEmpty(claims.HCFA_33))
                        pdf.WriteText(claims.HCFA_33_Qualifier + claims.HCFA_33, pdf.Column * 32 + 10.95f, pdf.RowMargin * 29.5f - (7 * 2.96f - 4.39f));
                }

                #endregion

                #region " Charges "


                float row_margin = 4.76f;
                float row_ = 21.5f;
                int counter = 0;
                float totalcharge = 0;
                float totalPaidAmount = 0;

                DSHCFA.HCFAChargesDataTable extra_charges = new DSHCFA.HCFAChargesDataTable();

                foreach (DataRow item in dsTemp.HCFACharges.Rows)
                {
                    DSHCFA.HCFAChargesRow charges = (DSHCFA.HCFAChargesRow)item;
                    if (counter == 5)
                        row_margin = row_margin + 1.5f;

                    if (counter < 6)
                    {
                        //24
                        //from
                        pdf.WriteText(charges.HCFA_24_A_From_MM, pdf.Column - 2.39f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));
                        pdf.WriteText(charges.HCFA_24_A_From_DD, pdf.Column * 2 + 2.39f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));
                        pdf.WriteText(charges.HCFA_24_A_From_YY, pdf.Column * 3 + 9.39f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));

                        //to
                        pdf.WriteText(charges.HCFA_24_A_To_MM, pdf.Column * 5 + 4.39f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));
                        pdf.WriteText(charges.HCFA_24_A_To_DD, pdf.Column * 7 + 1.39f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));
                        pdf.WriteText(charges.HCFA_24_A_To_YY, pdf.Column * 8 + 8.39f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));

                        pdf.WriteText(charges.HCFA_24_B, pdf.Column * 10 + 2.39f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));
                        pdf.WriteText(charges.HCFA_24_C, pdf.Column * 11 + 9.39f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));
                        pdf.WriteText(charges.HCFA_24_D_CPT, pdf.Column * 13 + 6.39f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));

                        //modifier
                        pdf.WriteText(charges.HCFA_24_D_MOD_1, pdf.Column * 17 + 4.19f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));
                        pdf.WriteText(charges.HCFA_24_D_MOD_2, pdf.Column * 18 + 12.39f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));
                        pdf.WriteText(charges.HCFA_24_D_MOD_3, pdf.Column * 20 + 6.19f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));
                        pdf.WriteText(charges.HCFA_24_D_MOD_4, pdf.Column * 21 + 14.95f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));

                        //pointers
                        pdf.WriteText(charges.HCFA_24_E, pdf.Column * 23 + 10.95f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));

                        //charges
                        pdf.WriteText(charges.HCFA_24_F_A, pdf.Column * 27 - 6.39f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));
                        pdf.WriteText(charges.HCFA_24_F_B, pdf.Column * 29 + 8.95f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));

                        pdf.WriteText(charges.HCFA_24_G, pdf.Column * 31 + 2.95f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));
                        pdf.WriteText(charges.HCFA_24_H, pdf.Column * 32 + 12.95f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));
                        pdf.WriteText(charges.HCFA_24_J, pdf.Column * 35 + 6.39f, pdf.RowMargin * row_ - (7 * 2.96f - row_margin));

                        //NDC
                        pdf.WriteText(charges.NDC + "  " + charges.NDCUnit + charges.NDCMeasurementCode + " " + charges.LineNotes + " " + charges.AnesthesiaTime, pdf.Column - 2.39f, pdf.RowMargin * row_ - (7 * 4.96f - row_margin));

                        if (IsRequired())
                        {
                            //Tax ID Number or taxonomy code for 24 IJ
                            if (!string.IsNullOrEmpty(claims.HCFA_24_J))
                            {
                                pdf.WriteText(claims.HCFA_24_I, pdf.Column * 35 - 10.60f, pdf.RowMargin * row_ - (7 * 4.96f - row_margin));
                                pdf.WriteText(claims.HCFA_24_J, pdf.Column * 35 + 6.39f, pdf.RowMargin * row_ - (7 * 4.96f - row_margin));
                            }
                        }

                        totalcharge += (MDVUtility.Tofloat(charges.HCFA_24_F_A + "." + charges.HCFA_24_F_B));
                        totalPaidAmount += (MDVUtility.Tofloat(charges.HCFA_29_A + "." + charges.HCFA_29_B));

                    }
                    else
                    {
                        //add extra rows
                        extra_charges.ImportRow(charges);
                    }

                    row_margin = row_margin - 2;
                    row_++;
                    counter++;
                }

                #endregion

                #region " Total Charge "

                string HCFA_28_A = "00";
                string HCFA_28_B = "00";

                if (totalcharge != 0)
                {
                    string[] charge = MDVUtility.ToStr(String.Format("{0:0.00}", totalcharge)).Split('.');
                    HCFA_28_A = charge[0];

                    if (charge.Length > 1)
                        HCFA_28_B = charge[1];
                    else
                        HCFA_28_B = "00";
                }

                //28
                pdf.WriteText(HCFA_28_A, pdf.Column * 30 + 4.39f, pdf.RowMargin * 27 - (7 * 2.96f - 6.39f), false, iTextSharp.text.pdf.PdfContentByte.ALIGN_RIGHT);
                pdf.WriteText(HCFA_28_B, pdf.Column * 30 + 10.95f, pdf.RowMargin * 27 - (7 * 2.96f - 6.39f));

                #endregion

                #region " Total Paid Amount "

                string HCFA_29_A = "00";
                string HCFA_29_B = "00";

                if (totalPaidAmount != 0)
                {

                    string[] amount = MDVUtility.ToStr(String.Format("{0:0.00}", totalPaidAmount)).Split('.');
                    HCFA_29_A = amount[0];

                    if (amount.Length > 1)
                        HCFA_29_B = amount[1];
                    else
                        HCFA_29_B = "00";

                }

                //29
                pdf.WriteText(HCFA_29_A, pdf.Column * 36 - 6.39f, pdf.RowMargin * 27 - (7 * 2.96f - 6.39f), false, iTextSharp.text.pdf.PdfContentByte.ALIGN_RIGHT);
                pdf.WriteText(HCFA_29_B, pdf.Column * 36 - 2.39f, pdf.RowMargin * 27 - (7 * 2.96f - 6.39f));

                #endregion

                if (IsBackground)
                {
                    imageFilePath = MDVUtility.GetImagePath(MDVSession.Current.ImagePath, "bgfill_form", "jpg");
                    //string imageFilePath = (System.AppDomain.CurrentDomain.BaseDirectory).Replace("\\", "/") + (SharedObj.ImagePath).Replace("~/", "") + "/bgfill_form.jpg";
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageFilePath);
                    img.ScaleToFit(3500, 800);
                    img.Alignment = iTextSharp.text.Image.UNDERLYING;
                    img.SetAbsolutePosition(-4f, -4);
                    pdf.Document.Add(img);
                }

                // Print Extra Charges more then 6
                if (extra_charges.Rows.Count > 0)
                {
                    DSHCFA ds_extraCharges = new DSHCFA();
                    ds_extraCharges.Merge(dsTemp.HCFAClaims);
                    ds_extraCharges.Merge(extra_charges);

                    CreatePage(pdf, ds_extraCharges, IsBackground);
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::CreatePage", ex);

            }
        }

        private void CreatePageForChrome(MDVUtility.PDFCreator pdf, DSHCFA dsTemp, bool IsBackground)
        {
            string imageFilePath = "";
            try
            {
                pdf.Document.NewPage();

                #region " Claims "

                DSHCFA.HCFAClaimsRow claims = (DSHCFA.HCFAClaimsRow)dsTemp.HCFAClaims.Rows[0];

                #region " Azam Aftab "

                //Header
                pdf.WriteText(claims.HCFA_Payer_1, pdf.Column * 27 - 4.39f, pdf.RowMargin * 2f);
                pdf.WriteText(claims.HCFA_Payer_2, pdf.Column * 27 - 4.39f, pdf.RowMargin * 2.5f);
                pdf.WriteText(claims.HCFA_Payer_3, pdf.Column * 27 - 4.39f, pdf.RowMargin * 3f);
                pdf.WriteText(claims.HCFA_Payer_4, pdf.Column * 27 - 4.39f, pdf.RowMargin * 3.5f);

                //1
                if (ToBool(claims.HCFA_1_A))
                    pdf.WriteText("x", pdf.Column + 9.19f, pdf.RowMargin * 4 + 9.0f);
                else if (ToBool(claims.HCFA_1_B))
                    pdf.WriteText("x", pdf.Column * 4 + 11.19f, pdf.RowMargin * 4 + 9.0f);
                else if (ToBool(claims.HCFA_1_C))
                    pdf.WriteText("x", pdf.Column * 8 + 8.0f, pdf.RowMargin * 4 + 9.0f);
                else if (ToBool(claims.HCFA_1_D))
                    pdf.WriteText("x", pdf.Column * 12 + 15.0f, pdf.RowMargin * 4 + 9.0f);
                else if (ToBool(claims.HCFA_1_E))
                    pdf.WriteText("x", pdf.Column * 17 - 8.19f, pdf.RowMargin * 4 + 9.0f);
                else if (ToBool(claims.HCFA_1_F))
                    pdf.WriteText("x", pdf.Column * 21 - 7.50f, pdf.RowMargin * 4 + 9.0f);
                else if (ToBool(claims.HCFA_1_G))
                    pdf.WriteText("x", pdf.Column * 24 - 8.20f, pdf.RowMargin * 4 + 9.0f);

                pdf.WriteText(claims.HCFA_2, pdf.Column + 9.0f, pdf.RowMargin * 5 + 8.39f);
                pdf.WriteText(claims.HCFA_5, pdf.Column + 9.0f, pdf.RowMargin * 6 + 5.39f);
                pdf.WriteText(claims.HCFA_5_A, pdf.Column + 9.0f, pdf.RowMargin * 7 + (1 * 1.96f));
                pdf.WriteText(claims.HCFA_5_B, pdf.Column * 14 + 9.0f, pdf.RowMargin * 7 + (2 * 1.0f));
                pdf.WriteText(claims.HCFA_5_C, pdf.Column + 9.0f, pdf.RowMargin * 8 - (2 * 2.0f));
                pdf.WriteText(claims.HCFA_5_D, pdf.Column * 8 + 9.0f, pdf.RowMargin * 8 - (2 * 2.0f));

                //9
                pdf.WriteText(claims.HCFA_9, pdf.Column + 9.0f, pdf.RowMargin * 9 + (2 * -1.0f));
                pdf.WriteText(claims.HCFA_9_A, pdf.Column + 9.0f, pdf.RowMargin * 10 - (3 * 2.0f));
                pdf.WriteText(claims.HCFA_9_D, pdf.Column + 9.0f, pdf.RowMargin * 13 + (6 * -2.5f));

                pdf.WriteText(claims.HCFA_1A, pdf.Column * 27 - 4.39f, pdf.RowMargin * 4.5f - 4);
                pdf.WriteText(claims.HCFA_4, pdf.Column * 27 - 4.39f, pdf.RowMargin * 5 + 8.39f);
                pdf.WriteText(claims.HCFA_7, pdf.Column * 27 - 4.39f, pdf.RowMargin * 6 + 5.39f);
                pdf.WriteText(claims.HCFA_7_A, pdf.Column * 27 - 1.39f, pdf.RowMargin * 7 + (1 * 1.96f));
                pdf.WriteText(claims.HCFA_7_B, pdf.Column * 39f, pdf.RowMargin * 7 + (2 * 0.0f));
                pdf.WriteText(claims.HCFA_7_C, pdf.Column * 27 - 4.39f, pdf.RowMargin * 8 - (2 * 2.0f));
                pdf.WriteText(claims.HCFA_7_D, pdf.Column * 34 - 1.0f, pdf.RowMargin * 8 - (2 * 3.5f));

                //3
                pdf.WriteText(claims.HCFA_3_MM, pdf.Column * 17 - 4.39f, pdf.RowMargin * 5 + 8.39f);
                pdf.WriteText(claims.HCFA_3_DD, pdf.Column * 18 + 1.79f, pdf.RowMargin * 5 + 8.39f);
                pdf.WriteText(claims.HCFA_3_YY, pdf.Column * 19 + 6.98f, pdf.RowMargin * 5 + 8.39f);


                if (claims.HCFA_3_M)
                    pdf.WriteText("x", pdf.Column * 22 + 0f, pdf.RowMargin * 5 + 7.39f);
                else if (claims.HCFA_3_F)
                    pdf.WriteText("x", pdf.Column * 25 - 8.39f, pdf.RowMargin * 5 + 7.39f);

                #endregion

                #region " Azeem "

                //6
                if (ToBool(claims.HCFA_6_A))
                    pdf.WriteText("x", pdf.Column * 17 + 5.39f, pdf.RowMargin * 6 + 2.89f);
                if (ToBool(claims.HCFA_6_B))
                    pdf.WriteText("x", pdf.Column * 19 + 13.39f, pdf.RowMargin * 6 + 2.89f);
                if (ToBool(claims.HCFA_6_C))
                    pdf.WriteText("x", pdf.Column * 21 + 11.9f, pdf.RowMargin * 6 + 2.89f);
                if (ToBool(claims.HCFA_6_D))
                    pdf.WriteText("x", pdf.Column * 25 - 8.39f, pdf.RowMargin * 6 + 2.89f);

                //10a
                if (claims.HCFA_10_A)
                    pdf.WriteText("x", pdf.Column * 18 + 7.25f, pdf.RowMargin * 10 - 7.39f);
                else
                    pdf.WriteText("x", pdf.Column * 21 + 7.65f, pdf.RowMargin * 10 - 7.39f);

                //10b
                if (claims.HCFA_10_B)
                    pdf.WriteText("x", pdf.Column * 18 + 7.25f, pdf.RowMargin * 11 - 12.0f);
                else
                    pdf.WriteText("x", pdf.Column * 21 + 7.65f, pdf.RowMargin * 11 - 12.0f);

                pdf.WriteText(claims.HCFA_10_B_STATE, pdf.Column * 23 + 10.95f, pdf.RowMargin * 11 - 12.0f);

                //10c
                if (claims.HCFA_10_C)
                    pdf.WriteText("x", pdf.Column * 18 + 7.25f, pdf.RowMargin * 12 - 14.0f);
                else
                    pdf.WriteText("x", pdf.Column * 21 + 7.65f, pdf.RowMargin * 12 - 14.0f);

                //12
                pdf.WriteText(claims.HCFA_12_A, pdf.Column * 4 + 2.39f, pdf.RowMargin * 15 - (8 * 3.2f));
                pdf.WriteText(claims.HCFA_12_B, pdf.Column * 19 + 10.58f, pdf.RowMargin * 15 - (8 * 3.2f));

                //13
                pdf.WriteText(claims.HCFA_13, pdf.Column * 29, pdf.RowMargin * 15 - (8 * 3.2f));

                //14
                pdf.WriteText(claims.HCFA_14_MM, pdf.Column * 2 + 0.39f, pdf.RowMargin * 16 - (7 * 2.96f + 3.40f));
                pdf.WriteText(claims.HCFA_14_DD, pdf.Column * 3 + 7.55f, pdf.RowMargin * 16 - (7 * 2.96f + 3.40f));
                pdf.WriteText(claims.HCFA_14_YY, pdf.Column * 5 + 6.39f, pdf.RowMargin * 16 - (7 * 2.96f + 3.40f));
                pdf.WriteText(claims.HCFA_14_QUAL, pdf.Column * 9 + 1.39f, pdf.RowMargin * 16 - (7 * 2.96f + 3.40f));

                #endregion

                #region " Zeshan "

                //11
                pdf.WriteText(claims.HCFA_11, pdf.Column * 27 - 4.39f, pdf.RowMargin * 9 - (2 * 2.96f));

                //11a
                pdf.WriteText(claims.HCFA_11_A_MM, pdf.Column * 27 + 6.39f, pdf.RowMargin * 10 - 6.96f);
                pdf.WriteText(claims.HCFA_11_A_DD, pdf.Column * 29 - 0.39f, pdf.RowMargin * 10 - 6.96f);
                pdf.WriteText(claims.HCFA_11_A_YY, pdf.Column * 30 + 9.99f, pdf.RowMargin * 10 - 6.96f);

                if (claims.HCFA_11_A_M)
                    pdf.WriteText("x", pdf.Column * 35 - 1.55f, pdf.RowMargin * 10 - (2 * 2.96f + 1.96f));
                else if (claims.HCFA_11_A_F)
                    pdf.WriteText("x", pdf.Column * 38 + 5.39f, pdf.RowMargin * 10 - (2 * 2.96f + 1.96f));

                //11c
                pdf.WriteText(claims.HCFA_11_C, pdf.Column * 27 - 4.39f, pdf.RowMargin * 12 - (4 * 2.96f));

                //11d
                if (ToBool(claims.HCFA_11_D))
                    pdf.WriteText("x", pdf.Column * 27 + 0.0f, pdf.RowMargin * 13 - (6 * 2.96f));
                else
                    pdf.WriteText("x", pdf.Column * 29 + 4.20f, pdf.RowMargin * 13 - (6 * 2.96f));

                //15
                pdf.WriteText(claims.HCFA_15_QUAL, pdf.Column * 17 - 8.19f, pdf.RowMargin * 16 - (9 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_15_MM, pdf.Column * 19 + 10.39f, pdf.RowMargin * 16 - (9 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_15_DD, pdf.Column * 21 + 2.39f, pdf.RowMargin * 16 - (9 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_15_YY, pdf.Column * 22 + 14.39f, pdf.RowMargin * 16 - (9 * 2.96f - 2.39f));

                //16a
                pdf.WriteText(claims.HCFA_16_A_MM, pdf.Column * 28 - 2.39f, pdf.RowMargin * 16 - (9 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_16_A_DD, pdf.Column * 30 - 8.39f, pdf.RowMargin * 16 - (9 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_16_A_YY, pdf.Column * 31 + 5.39f, pdf.RowMargin * 16 - (9 * 2.96f - 2.39f));

                //16b
                pdf.WriteText(claims.HCFA_16_B_MM, pdf.Column * 35 - 0.39f, pdf.RowMargin * 16 - (9 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_16_B_DD, pdf.Column * 37 - 6.39f, pdf.RowMargin * 16 - (9 * 2.96f - 2.39f));
                pdf.WriteText(claims.HCFA_16_B_YY, pdf.Column * 38 + 3.39f, pdf.RowMargin * 16 - (9 * 2.96f - 2.39f));

                #endregion

                #region " Adnan "

                // 17
                pdf.WriteText(claims.HCFA_17, pdf.Column * 4 - 3f, pdf.RowMargin * 17 - (6 * 4.56f));
                pdf.WriteText(claims.HCFA_17_QUAL, pdf.Column * (1.5f), pdf.RowMargin * 17 - (6 * 4.56f));
                pdf.WriteText(claims.HCFA_17B, pdf.Column * 18.5f + 1.19f, pdf.RowMargin * 17 - (7 * 4.00f));

                //  18a
                pdf.WriteText(claims.HCFA_18_A_MM, pdf.Column * 28 - 0.00f, pdf.RowMargin * 17 - (7 * 3.70f));
                pdf.WriteText(claims.HCFA_18_A_DD, pdf.Column * 30 - 7.59f, pdf.RowMargin * 17 - (7 * 3.70f));
                pdf.WriteText(claims.HCFA_18_A_YY, pdf.Column * 31 + 3.39f, pdf.RowMargin * 17 - (7 * 3.70f));

                // 18b
                pdf.WriteText(claims.HCFA_18_B_MM, pdf.Column * 35f, pdf.RowMargin * 17 - (7 * 3.70f));
                pdf.WriteText(claims.HCFA_18_B_DD, pdf.Column * 36.4f, pdf.RowMargin * 17 - (7 * 3.70f));
                pdf.WriteText(claims.HCFA_18_B_YY, pdf.Column * 38 + 5.39f, pdf.RowMargin * 17 - (7 * 3.70f));

                // 19
                pdf.WriteText(claims.HCFA_19, pdf.Column * 2.1f, pdf.RowMargin * 18 - (8 * 3.96f));

                // 21
                pdf.WriteText(claims.HCFA_21_ICD, pdf.Column * 20 + 30f, pdf.RowMargin * 18.5f - (7 * 3.96f + 2.39f));

                // 21a
                pdf.WriteText(claims.HCFA_21_A, pdf.Column + 20.95f, pdf.RowMargin * 19 - (7 * 3.96f + 3.50f));
                pdf.WriteText(claims.HCFA_21_B, pdf.Column * 8 + 12.89f, pdf.RowMargin * 19 - (7 * 3.96f + 3.50f));
                pdf.WriteText(claims.HCFA_21_C, pdf.Column * 15 + 5.39f, pdf.RowMargin * 19 - (7 * 3.96f + 3.50f));
                pdf.WriteText(claims.HCFA_21_D, pdf.Column * 21 + 14.95f, pdf.RowMargin * 19 - (7 * 3.96f + 3.50f));

                // 21b
                pdf.WriteText(claims.HCFA_21_E, pdf.Column + 20.95f, pdf.RowMargin * 19.5f - (8 * 3.65f + 3.50f));
                pdf.WriteText(claims.HCFA_21_F, pdf.Column * 8 + 12.89f, pdf.RowMargin * 19.5f - (8 * 3.65f + 3.50f));
                pdf.WriteText(claims.HCFA_21_G, pdf.Column * 15 + 5.39f, pdf.RowMargin * 19.5f - (8 * 3.65f + 3.50f));
                pdf.WriteText(claims.HCFA_21_H, pdf.Column * 21 + 14.95f, pdf.RowMargin * 19.5f - (8 * 3.65f + 3.50f));

                #endregion

                #region " Abdul Rahman "

                //21c
                pdf.WriteText(claims.HCFA_21_I, pdf.Column * 2 + 5.39f, pdf.RowMargin * 19.5f - (7 * 2.96f));
                pdf.WriteText(claims.HCFA_21_J, pdf.Column * 8 + 12.89f, pdf.RowMargin * 19.5f - (7 * 2.96f));
                pdf.WriteText(claims.HCFA_21_K, pdf.Column * 15 + 5.39f, pdf.RowMargin * 19.5f - (7 * 2.96f));
                pdf.WriteText(claims.HCFA_21_L, pdf.Column * 21 + 14f, pdf.RowMargin * 19.5f - (7 * 2.96f));

                //22
                pdf.WriteText(claims.HCFA_22_A, pdf.Column * 27 - 4.39f, pdf.RowMargin * 19f - (7 * 2.96f + 11f));
                pdf.WriteText(claims.HCFA_22_B, pdf.Column * 32 + 4.39f, pdf.RowMargin * 19f - (7 * 2.96f + 11f));

                //23
                pdf.WriteText(claims.HCFA_23, pdf.Column * 27 - 4.39f, pdf.RowMargin * 19.5f - (7 * 2.96f));

                //25
                pdf.WriteText(claims.HCFA_25, pdf.Column * 2, pdf.RowMargin * 27 - (7 * 4.30f + 2f));
                if (claims.HCFA_25_EIN)
                    pdf.WriteText("x", pdf.Column * 10 + 8f, pdf.RowMargin * 27 - (7 * 2.96f + 12.50f));
                else if (claims.HCFA_25_SSN)
                    pdf.WriteText("x", pdf.Column * 10 - 5f, pdf.RowMargin * 27 - (7 * 2.96f + 12.50f));

                //26
                pdf.WriteText(claims.HCFA_26, pdf.Column * 13 - 4.39f, pdf.RowMargin * 27 - (7 * 4.30f + 2f));


                //27
                if (claims.HCFA_27)
                    pdf.WriteText("x", pdf.Column * 20, pdf.RowMargin * 27 - (7 * 2.96f + 11.50f));
                else
                    pdf.WriteText("x", pdf.Column * 23 - 6.96f, pdf.RowMargin * 27 - (7 * 2.96f + 11.50f));


                //31
                pdf.WriteText(claims.HCFA_31_A_1, pdf.Column * 2 - 2f, pdf.RowMargin * 28 - (7 * 2.96f), true);
                //pdf.WriteText(claims.HCFA_31_A_2, pdf.Column * 2 - 2f, pdf.RowMargin * 28.5f - (7 * 2.96f + 5.39f));
                pdf.WriteText(claims.HCFA_31_B, pdf.Column * 9 - 6.96f, pdf.RowMargin * 28.5f - (7 * 2.96f - 2.39f));

                //32
                float line_32 = pdf.WriteText(claims.HCFA_32_A_1, pdf.Column * 13 - 6.39f, pdf.RowMargin * 27f - (7 * 2.96f - 7.39f), true);
                pdf.WriteText(claims.HCFA_32_A_2, pdf.Column * 13 - 6.39f, pdf.RowMargin * (28f + line_32) - (7 * 2.96f + 4.50f));
                pdf.WriteText(claims.HCFA_32_B, pdf.Column * 13 - 5.30f, pdf.RowMargin * 29f - (7 * 2.96f + 4.39f));
                //pdf.WriteText(claims.HCFA_32_B, pdf.Column * 18 + 8.39f, pdf.RowMargin * 29.5f - (7 * 2.96f - 4.39f));

                #endregion

                #region " zia shah "

                //33
                // pdf.WriteText(claims.HCFA_33_C, pdf.Column * 35 - 4.39f, pdf.RowMargin * 27 - 2f);
                float line_33 = 0;
                if (IsBackground == false)
                {
                    line_33 = pdf.WriteText(claims.HCFA_33_A_1, pdf.Column * 27 - 6.39f, pdf.RowMargin * 27.2f - (7 * 2.36f - 4.39f), true);
                    pdf.WriteText(claims.HCFA_33_A_2, pdf.Column * 27 - 6.39f, pdf.RowMargin * (27.6f + line_33) - (7 * 2.50f - 4.39f));
                    pdf.WriteText(claims.HCFA_33_B, pdf.Column * 27 - 4.30f, pdf.RowMargin * 28.7f - (7 * 2.96f - 4.39f));
                    pdf.WriteText(claims.HCFA_33_C, pdf.Column * 34.5f - 4.39f, pdf.RowMargin * 26.4f - 2f);
                  
                }
                else
                {
                    line_33 = pdf.WriteText(claims.HCFA_33_A_1, pdf.Column * 27 - 6.39f, pdf.RowMargin * 27 - (7 * 2.50f - 4.39f), true);
                    pdf.WriteText(claims.HCFA_33_A_2, pdf.Column * 27 - 6.39f, pdf.RowMargin * (27.5f + line_33) - (7 * 2.50f - 4.39f));
                    pdf.WriteText(claims.HCFA_33_B, pdf.Column * 27 - 4.30f, pdf.RowMargin * 28.5f - (7 * 2.70f - 4.39f));
                    pdf.WriteText(claims.HCFA_33_C, pdf.Column * 35 - 4.39f, pdf.RowMargin * 27 - 2f);
                }

               

                //print tax id or taxonomy code
                if (IsRequired())
                {
                        if (!string.IsNullOrEmpty(claims.HCFA_33))
                            pdf.WriteText(claims.HCFA_33_Qualifier + claims.HCFA_33, pdf.Column * 32 + 10.95f, pdf.RowMargin * 28.65f - (7 * 2.70f - 4.39f));
                }



                #region " Charges "


                float row_margin = 2.50f;
                float row_ = 21.5f;
                int counter = 0;
                float totalcharge = 0;
                float totalPaidAmount = 0;

                DSHCFA.HCFAChargesDataTable extra_charges = new DSHCFA.HCFAChargesDataTable();

                foreach (DataRow item in dsTemp.HCFACharges.Rows)
                {
                    DSHCFA.HCFAChargesRow charges = (DSHCFA.HCFAChargesRow)item;

                    if (counter == 3 || counter == 4)
                        row_margin = row_margin - 1f;
                    else if (counter == 5)
                        row_margin = row_margin - 1.5f;

                    if (counter < 6)
                    {
                        //24
                        //from
                        pdf.WriteText(charges.HCFA_24_A_From_MM, pdf.Column * 2 - 4.39f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));
                        pdf.WriteText(charges.HCFA_24_A_From_DD, pdf.Column * 3 + 2.39f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));
                        pdf.WriteText(charges.HCFA_24_A_From_YY, pdf.Column * 4 + 9.39f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));

                        //to
                        pdf.WriteText(charges.HCFA_24_A_To_MM, pdf.Column * 6 - 2.39f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));
                        pdf.WriteText(charges.HCFA_24_A_To_DD, pdf.Column * 8 - 4.39f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));
                        pdf.WriteText(charges.HCFA_24_A_To_YY, pdf.Column * 9 + 4.39f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));

                        pdf.WriteText(charges.HCFA_24_B, pdf.Column * 10 + 6.39f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));
                        pdf.WriteText(charges.HCFA_24_C, pdf.Column * 11 + 15.39f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));
                        pdf.WriteText(charges.HCFA_24_D_CPT, pdf.Column * 13 + 11.39f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));

                        //modifier
                        pdf.WriteText(charges.HCFA_24_D_MOD_1, pdf.Column * 17 + 4.19f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));
                        pdf.WriteText(charges.HCFA_24_D_MOD_2, pdf.Column * 18 + 12.39f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));
                        pdf.WriteText(charges.HCFA_24_D_MOD_3, pdf.Column * 20 + 6.19f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));
                        pdf.WriteText(charges.HCFA_24_D_MOD_4, pdf.Column * 21 + 14.95f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));

                        //pointers
                        pdf.WriteText(charges.HCFA_24_E, pdf.Column * 23 + 8.95f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));

                        //charges
                        pdf.WriteText(charges.HCFA_24_F_A, pdf.Column * 27 - 6.39f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));
                        pdf.WriteText(charges.HCFA_24_F_B, pdf.Column * 29 + 2.95f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));

                        pdf.WriteText(charges.HCFA_24_G, pdf.Column * 31 + 2.95f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));
                        pdf.WriteText(charges.HCFA_24_H, pdf.Column * 32 + 12.95f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));
                        pdf.WriteText(charges.HCFA_24_J, pdf.Column * 35 - 2.39f, pdf.RowMargin * row_ - (7 * 4.30f - row_margin));

                        //NDC
                        pdf.WriteText(charges.NDC + "  " + charges.NDCUnit + charges.NDCMeasurementCode + " " + charges.LineNotes + " " + charges.AnesthesiaTime, pdf.Column * 2 - 4.39f, pdf.RowMargin * row_ - (7 * 6.10f - row_margin));

                        if (IsRequired())
                        {
                            //Tax ID Number or taxonomy code
                            if (!string.IsNullOrEmpty(claims.HCFA_24_J))
                            {
                                pdf.WriteText(claims.HCFA_24_I + claims.HCFA_24_J, pdf.Column * 35 - 4.60f, pdf.RowMargin * row_ - (7 * 5.50f - row_margin + 1.50f));
                            }
                        }

                        totalcharge += (MDVUtility.Tofloat(charges.HCFA_24_F_A + "." + charges.HCFA_24_F_B));
                        totalPaidAmount += (MDVUtility.Tofloat(charges.HCFA_29_A + "." + charges.HCFA_29_B));

                    }
                    else
                    {
                        //add extra rows
                        extra_charges.ImportRow(charges);
                    }

                    row_margin = row_margin - 2;
                    row_++;
                    counter++;
                }

                #endregion

                #region " Total Charge "

                string HCFA_28_A = "00";
                string HCFA_28_B = "00";

                if (totalcharge != 0)
                {
                    string[] charge = MDVUtility.ToStr(String.Format("{0:0.00}", totalcharge)).Split('.');
                    HCFA_28_A = charge[0];

                    if (charge.Length > 1)
                        HCFA_28_B = charge[1];
                    else
                        HCFA_28_B = "00";
                }

                //28
                pdf.WriteText(HCFA_28_A, pdf.Column * 30 - 2.39f, pdf.RowMargin * 27 - (7 * 4.30f + 2f), false, iTextSharp.text.pdf.PdfContentByte.ALIGN_RIGHT);
                pdf.WriteText(HCFA_28_B, pdf.Column * 30 + 2.95f, pdf.RowMargin * 27 - (7 * 4.30f + 2f));

                #endregion

                #region " Total Paid Amount "

                string HCFA_29_A = "00";
                string HCFA_29_B = "00";

                if (totalPaidAmount != 0)
                {

                    string[] amount = MDVUtility.ToStr(String.Format("{0:0.00}", totalPaidAmount)).Split('.');
                    HCFA_29_A = amount[0];

                    if (amount.Length > 1)
                        HCFA_29_B = amount[1];
                    else
                        HCFA_29_B = "00";

                }

                //29
                pdf.WriteText(HCFA_29_A, pdf.Column * 35 - 4.39f, pdf.RowMargin * 27 - (7 * 4.30f + 2f), false, iTextSharp.text.pdf.PdfContentByte.ALIGN_RIGHT);
                pdf.WriteText(HCFA_29_B, pdf.Column * 36 - 11.39f, pdf.RowMargin * 27 - (7 * 4.30f + 2f));

                #endregion

                #endregion

                #endregion

                if (IsBackground)
                {
                    imageFilePath = MDVUtility.GetImagePath(MDVSession.Current.ImagePath, "bgfill_form", "jpg");
                    //string imageFilePath = (System.AppDomain.CurrentDomain.BaseDirectory).Replace("\\", "/") + (SharedObj.ImagePath).Replace("~/", "") + "/bgfill_form.jpg";
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageFilePath);
                    img.ScaleToFit(3500, 800);
                    img.Alignment = iTextSharp.text.Image.UNDERLYING;
                    img.SetAbsolutePosition(-4f, -4);
                    pdf.Document.Add(img);
                }

                // Print Extra Charges more then 6
                if (extra_charges.Rows.Count > 0)
                {
                    DSHCFA ds_extraCharges = new DSHCFA();
                    ds_extraCharges.Merge(dsTemp.HCFAClaims);
                    ds_extraCharges.Merge(extra_charges);

                    CreatePageForChrome(pdf, ds_extraCharges, IsBackground);
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::CreatePage", ex);

            }
        }

        private void CreatePageForIE(MDVUtility.PDFCreator pdf, DSHCFA dsTemp, bool IsBackground)
        {
            string imageFilePath = "";
            try
            {
                pdf.Document.NewPage();

                #region " Claims "

                DSHCFA.HCFAClaimsRow claims = (DSHCFA.HCFAClaimsRow)dsTemp.HCFAClaims.Rows[0];

                #region " Azam Aftab "

                //Header
                pdf.WriteText(claims.HCFA_Payer_1, pdf.Column * 27 - 4.39f, pdf.RowMargin * 2f);
                pdf.WriteText(claims.HCFA_Payer_2, pdf.Column * 27 - 4.39f, pdf.RowMargin * 2.5f);
                pdf.WriteText(claims.HCFA_Payer_3, pdf.Column * 27 - 4.39f, pdf.RowMargin * 3f);
                pdf.WriteText(claims.HCFA_Payer_4, pdf.Column * 27 - 4.39f, pdf.RowMargin * 3.4f);

                //1
                if (ToBool(claims.HCFA_1_A))
                    pdf.WriteText("x", pdf.Column + 8.8f, pdf.RowMargin * 4.1f + 9.5f);
                else if (ToBool(claims.HCFA_1_B))
                    pdf.WriteText("x", pdf.Column * 4 + 13.99f, pdf.RowMargin * 4.1f + 9.5f);
                else if (ToBool(claims.HCFA_1_C))
                    pdf.WriteText("x", pdf.Column * 8 + 8.7f, pdf.RowMargin * 4.1f + 9.5f);
                else if (ToBool(claims.HCFA_1_D))
                    pdf.WriteText("x", pdf.Column * 12 + 16.5f, pdf.RowMargin * 4.1f + 9.0f);
                else if (ToBool(claims.HCFA_1_E))
                    pdf.WriteText("x", pdf.Column * 17 - 4.90f, pdf.RowMargin * 4.1f + 9.0f);
                else if (ToBool(claims.HCFA_1_F))
                    pdf.WriteText("x", pdf.Column * 21 - 4.60f, pdf.RowMargin * 4.1f + 9.0f);
                else if (ToBool(claims.HCFA_1_G))
                    pdf.WriteText("x", pdf.Column * 24 - 3.80f, pdf.RowMargin * 4.1f + 9.0f);

                pdf.WriteText(claims.HCFA_2, pdf.Column + 9.0f, pdf.RowMargin * 5 + 8.39f);
                pdf.WriteText(claims.HCFA_5, pdf.Column + 9.0f, pdf.RowMargin * 6 + 5.39f);
                pdf.WriteText(claims.HCFA_5_A, pdf.Column + 9.0f, pdf.RowMargin * 7 + (1 * 1.96f));
                pdf.WriteText(claims.HCFA_5_B, pdf.Column * 14 + 9.0f, pdf.RowMargin * 7 + (2 * 1.0f));
                pdf.WriteText(claims.HCFA_5_C, pdf.Column + 9.0f, pdf.RowMargin * 8 - (2 * 2.0f));
                pdf.WriteText(claims.HCFA_5_D, pdf.Column * 8 + 9.0f, pdf.RowMargin * 8 - (2 * 2.0f));

                //9
                pdf.WriteText(claims.HCFA_9, pdf.Column + 9.0f, pdf.RowMargin * 9 + (2 * -1.0f));
                pdf.WriteText(claims.HCFA_9_A, pdf.Column + 9.0f, pdf.RowMargin * 10 - (3 * 2.0f));
                pdf.WriteText(claims.HCFA_9_D, pdf.Column + 9.0f, pdf.RowMargin * 13 + (6 * -2.5f));

                pdf.WriteText(claims.HCFA_1A, pdf.Column * 27 - 4.39f, pdf.RowMargin * 4.5f - 4);
                pdf.WriteText(claims.HCFA_4, pdf.Column * 27 - 4.39f, pdf.RowMargin * 5 + 8.39f);
                pdf.WriteText(claims.HCFA_7, pdf.Column * 27 - 4.39f, pdf.RowMargin * 6 + 5.39f);
                pdf.WriteText(claims.HCFA_7_A, pdf.Column * 27 - 1.39f, pdf.RowMargin * 7 + (1 * 1.96f));
                pdf.WriteText(claims.HCFA_7_B, pdf.Column * 39f, pdf.RowMargin * 7 + (2 * 0.0f));
                pdf.WriteText(claims.HCFA_7_C, pdf.Column * 27 - 4.39f, pdf.RowMargin * 8 - (2 * 1.0f));
                pdf.WriteText(claims.HCFA_7_D, pdf.Column * 34 - 1.0f, pdf.RowMargin * 8 - (2 * 1.0f));

                //3
                pdf.WriteText(claims.HCFA_3_MM, pdf.Column * 17.1f - 4.39f, pdf.RowMargin * 5 + 8.39f);
                pdf.WriteText(claims.HCFA_3_DD, pdf.Column * 18 + 1.85f, pdf.RowMargin * 5 + 8.39f);
                pdf.WriteText(claims.HCFA_3_YY, pdf.Column * 19.3f + 10f, pdf.RowMargin * 5 + 8.39f);


                if (claims.HCFA_3_M)
                    pdf.WriteText("x", pdf.Column * 22 + 1f, pdf.RowMargin * 5 + 7.39f);
                else if (claims.HCFA_3_F)
                    pdf.WriteText("x", pdf.Column * 25.2f - 9.39f, pdf.RowMargin * 5 + 7.39f);

                #endregion

                #region " Azeem "

                //6
                if (ToBool(claims.HCFA_6_A))
                    pdf.WriteText("x", pdf.Column * 17 + 10.31f, pdf.RowMargin * 6 + 4.70f);
                if (ToBool(claims.HCFA_6_B))
                    pdf.WriteText("x", pdf.Column * 19 + 16.39f, pdf.RowMargin * 6 + 4.70f);
                if (ToBool(claims.HCFA_6_C))
                    pdf.WriteText("x", pdf.Column * 22 + 3f, pdf.RowMargin * 6 + 4.70f);
                if (ToBool(claims.HCFA_6_D))
                    pdf.WriteText("x", pdf.Column * 25.2f - 7.39f, pdf.RowMargin * 6 + 4.70f);

                //10a
                if (claims.HCFA_10_A)
                    pdf.WriteText("x", pdf.Column * 18 + 8.25f, pdf.RowMargin * 10 - 5.39f);
                else
                    pdf.WriteText("x", pdf.Column * 21 + 8.66f, pdf.RowMargin * 10 - 5.4f);

                //10b
                if (claims.HCFA_10_B)
                    pdf.WriteText("x", pdf.Column * 18 + 8.26f, pdf.RowMargin * 10.9f - 3.9f);
                else
                    pdf.WriteText("x", pdf.Column * 21 + 8.65f, pdf.RowMargin * 10.9f - 3.9f);

                pdf.WriteText(claims.HCFA_10_B_STATE, pdf.Column * 23 + 10.95f, pdf.RowMargin * 10.9f - 3.9f);

                //10c
                if (claims.HCFA_10_C)
                    pdf.WriteText("x", pdf.Column * 18 + 8.25f, pdf.RowMargin * 12 - 12.0f);
                else
                    pdf.WriteText("x", pdf.Column * 21 + 8.66f, pdf.RowMargin * 12 - 11.9f);

                //12
                pdf.WriteText(claims.HCFA_12_A, pdf.Column * 4 + 2.39f, pdf.RowMargin * 15 - (8 * 2.3f));
                pdf.WriteText(claims.HCFA_12_B, pdf.Column * 19 + 10.58f, pdf.RowMargin * 15 - (8 * 2.3f));

                //13
                pdf.WriteText(claims.HCFA_13, pdf.Column * 29, pdf.RowMargin * 15 - (8 * 2.3f));

                //14
                pdf.WriteText(claims.HCFA_14_MM, pdf.Column * 2 + 0.39f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));
                pdf.WriteText(claims.HCFA_14_DD, pdf.Column * 3 + 7.55f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));
                pdf.WriteText(claims.HCFA_14_YY, pdf.Column * 5 + 6.39f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));
                pdf.WriteText(claims.HCFA_14_QUAL, pdf.Column * 9 + 1.39f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));

                #endregion

                #region " Zeshan "

                //11
                pdf.WriteText(claims.HCFA_11, pdf.Column * 27 - 4.39f, pdf.RowMargin * 9 - (2 * 2.96f));

                //11a
                pdf.WriteText(claims.HCFA_11_A_MM, pdf.Column * 27.16f + 10.7f, pdf.RowMargin * 10 - 2.86f);
                pdf.WriteText(claims.HCFA_11_A_DD, pdf.Column * 29.1f + 4.79f, pdf.RowMargin * 10 - 2.86f);
                pdf.WriteText(claims.HCFA_11_A_YY, pdf.Column * 30.16f + 18f, pdf.RowMargin * 10 - 2.86f);

                if (claims.HCFA_11_A_M)
                    pdf.WriteText("x", pdf.Column * 35.7f - 4.23f, pdf.RowMargin * 10 - 2.86f);
                else if (claims.HCFA_11_A_F)
                    pdf.WriteText("x", pdf.Column * 38.7f + 4.4f, pdf.RowMargin * 10 - 2.86f);

                //11c
                pdf.WriteText(claims.HCFA_11_C, pdf.Column * 27 - 4.39f, pdf.RowMargin * 12 - (4 * 2.96f));

                //11d
                if (ToBool(claims.HCFA_11_D))
                    pdf.WriteText("x", pdf.Column * 27 + 3.2f, pdf.RowMargin * 13 - (6 * 1.96f));
                else
                    pdf.WriteText("x", pdf.Column * 29 + 10.7f, pdf.RowMargin * 13 - (6 * 1.91f));

                //15
                pdf.WriteText(claims.HCFA_15_QUAL, pdf.Column * 17 - 6.19f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));
                pdf.WriteText(claims.HCFA_15_MM, pdf.Column * 19 + 10.39f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));
                pdf.WriteText(claims.HCFA_15_DD, pdf.Column * 21 + 2.39f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));
                pdf.WriteText(claims.HCFA_15_YY, pdf.Column * 22 + 14.39f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));

                //16a
                pdf.WriteText(claims.HCFA_16_A_MM, pdf.Column * 28.5f - 2.39f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));
                pdf.WriteText(claims.HCFA_16_A_DD, pdf.Column * 30.5f - 8.39f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));
                pdf.WriteText(claims.HCFA_16_A_YY, pdf.Column * 31.5f + 5.39f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));

                //16b
                pdf.WriteText(claims.HCFA_16_B_MM, pdf.Column * 35.5f - 0.39f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));
                pdf.WriteText(claims.HCFA_16_B_DD, pdf.Column * 37.3f - 6.39f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));
                pdf.WriteText(claims.HCFA_16_B_YY, pdf.Column * 38.5f + 3.39f, pdf.RowMargin * 16.1f - (6.0f * 2.96f + 3.0f));

                #endregion

                #region " Adnan "

                // 17
                pdf.WriteText(claims.HCFA_17, pdf.Column * 4 - 3f, pdf.RowMargin * 16.7f - (6 * 2.4f));
                pdf.WriteText(claims.HCFA_17_QUAL, pdf.Column * (1.5f), pdf.RowMargin * 16.7f - (6 * 2.4f));
                pdf.WriteText(claims.HCFA_17B, pdf.Column * 17.8f + 0.1f, pdf.RowMargin * 16.7f - (6 * 2.4f));

                //  18a
                pdf.WriteText(claims.HCFA_18_A_MM, pdf.Column * 28.5f - 2.39f, pdf.RowMargin * 16.8f - (6 * 2.5f));
                pdf.WriteText(claims.HCFA_18_A_DD, pdf.Column * 30.5f - 8.39f, pdf.RowMargin * 16.8f - (6 * 2.5f));
                pdf.WriteText(claims.HCFA_18_A_YY, pdf.Column * 31.5f + 5.39f, pdf.RowMargin * 16.8f - (6 * 2.5f));

                // 18b
                pdf.WriteText(claims.HCFA_18_B_MM, pdf.Column * 35.5f - 0.39f, pdf.RowMargin * 16.8f - (6 * 2.5f));
                pdf.WriteText(claims.HCFA_18_B_DD, pdf.Column * 37.3f - 6.39f, pdf.RowMargin * 16.8f - (6 * 2.5f));
                pdf.WriteText(claims.HCFA_18_B_YY, pdf.Column * 38.5f + 3.39f, pdf.RowMargin * 16.8f - (6 * 2.5f));

                // 19
                pdf.WriteText(claims.HCFA_19, pdf.Column * 2.1f, pdf.RowMargin * 18 - (8 * 3.7f));

                // 21
                pdf.WriteText(claims.HCFA_21_ICD, pdf.Column * 20 + 32f, pdf.RowMargin * 18.5f - (7 * 2.96f + 3.1f));

                // 21a
                pdf.WriteText(claims.HCFA_21_A, pdf.Column + 20.95f, pdf.RowMargin * 19 - (7 * 3.57f + 2.50f));
                pdf.WriteText(claims.HCFA_21_B, pdf.Column * 8 + 12.93f, pdf.RowMargin * 19 - (7 * 3.57f + 2.50f));
                pdf.WriteText(claims.HCFA_21_C, pdf.Column * 15 + 5.39f, pdf.RowMargin * 19 - (7 * 3.57f + 2.50f));
                pdf.WriteText(claims.HCFA_21_D, pdf.Column * 21 + 14.95f, pdf.RowMargin * 19 - (7 * 3.57f + 2.50f));

                // 21b
                pdf.WriteText(claims.HCFA_21_E, pdf.Column + 20.95f, pdf.RowMargin * 19.5f - (8 * 2.9f + 3.50f));
                pdf.WriteText(claims.HCFA_21_F, pdf.Column * 8 + 12.89f, pdf.RowMargin * 19.5f - (8 * 2.9f + 3.50f));
                pdf.WriteText(claims.HCFA_21_G, pdf.Column * 15 + 5.39f, pdf.RowMargin * 19.5f - (8 * 2.9f + 3.50f));
                pdf.WriteText(claims.HCFA_21_H, pdf.Column * 21 + 14.95f, pdf.RowMargin * 19.5f - (8 * 2.9f + 3.50f));

                #endregion

                #region " Abdul Rahman "

                //21c
                pdf.WriteText(claims.HCFA_21_I, pdf.Column * 2 + 5.39f, pdf.RowMargin * 19.5f - (7 * 1.96f));
                pdf.WriteText(claims.HCFA_21_J, pdf.Column * 8 + 12.89f, pdf.RowMargin * 19.5f - (7 * 1.96f));
                pdf.WriteText(claims.HCFA_21_K, pdf.Column * 15 + 5.39f, pdf.RowMargin * 19.5f - (7 * 1.96f));
                pdf.WriteText(claims.HCFA_21_L, pdf.Column * 21 + 14f, pdf.RowMargin * 19.5f - (7 * 1.96f));

                //22
                pdf.WriteText(claims.HCFA_22_A, pdf.Column * 27 - 4.39f, pdf.RowMargin * 19f - (7 * 2.96f + 5f));
                pdf.WriteText(claims.HCFA_22_B, pdf.Column * 32 + 4.39f, pdf.RowMargin * 19f - (7 * 2.96f + 5f));

                //23
                pdf.WriteText(claims.HCFA_23, pdf.Column * 27 - 4.39f, pdf.RowMargin * 19.5f - (7 * 1.96f));

                //25
                pdf.WriteText(claims.HCFA_25, pdf.Column * 2, pdf.RowMargin * 27 - (7 * 3.90f + 1f));
                if (claims.HCFA_25_EIN)
                    pdf.WriteText("x", pdf.Column * 9 + 8.8f, pdf.RowMargin * 27 - (7 * 2.96f + 1.50f));
                else if (claims.HCFA_25_SSN)
                    pdf.WriteText("x", pdf.Column * 10.3f + 4.9f, pdf.RowMargin * 27 - (7 * 2.96f + 1.50f));

                //26
                pdf.WriteText(claims.HCFA_26, pdf.Column * 13 - 4.39f, pdf.RowMargin * 27 - (7 * 3.80f + 1f));


                //27
                if (claims.HCFA_27)
                    pdf.WriteText("x", pdf.Column * 20.3f, pdf.RowMargin * 27 - (7 * 1.96f + 9.0f));
                else
                    pdf.WriteText("x", pdf.Column * 23 - 2.96f, pdf.RowMargin * 27 - (7 * 1.96f + 9.0f));


                //31
                pdf.WriteText(claims.HCFA_31_A_1, pdf.Column * 2 - 2f, pdf.RowMargin * 28 - (6.5f), true);
                //pdf.WriteText(claims.HCFA_31_A_2, pdf.Column * 2 - 2f, pdf.RowMargin * 28.5f - (7 * 2.96f + 5.39f));
                pdf.WriteText(claims.HCFA_31_B, pdf.Column * 9 - 6.96f, pdf.RowMargin * 28.7f - (7 * 2.36f - 2.39f));

                //32
                float line_32 = pdf.WriteText(claims.HCFA_32_A_1, pdf.Column * 13.2f - 6.19f, pdf.RowMargin * 27f - (7 * 0.10f - 0.39f), true);
                pdf.WriteText(claims.HCFA_32_A_2, pdf.Column * 13.2f - 5.89f, pdf.RowMargin * (28f + line_32) - (7 * 2.38f + 0.9f));
                pdf.WriteText(claims.HCFA_32_B, pdf.Column * 13.3f - 2.10f, pdf.RowMargin * 29f - (7 * 1.96f + 2.39f));
                //pdf.WriteText(claims.HCFA_32_B, pdf.Column * 18 + 8.39f, pdf.RowMargin * 29.5f - (7 * 2.96f - 4.39f));

                #endregion

                #region " Arsalan Javed "

                //33
                float line_33 = pdf.WriteText(claims.HCFA_33_A_1, pdf.Column * 27 - 6.39f, pdf.RowMargin * 27 - (7 * 0.60f - 1.39f), true);
                pdf.WriteText(claims.HCFA_33_A_2, pdf.Column * 27 - 6.29f, pdf.RowMargin * (27.5f + line_33) - (7 * 0.60f - 1.39f));
                pdf.WriteText(claims.HCFA_33_B, pdf.Column * 27 - 4.30f, pdf.RowMargin * 28.5f - (7 * 1.80f - 11.39f));
                pdf.WriteText(claims.HCFA_33_C, pdf.Column * 35 - 4.39f, pdf.RowMargin * 27 - 2f);
              
                //print tax id or taxonomy code
                if (IsRequired())
                {
                    if (!string.IsNullOrEmpty(claims.HCFA_33))
                        pdf.WriteText(claims.HCFA_33_Qualifier + claims.HCFA_33, pdf.Column * 32 + 10.95f, pdf.RowMargin * 28.5f - (7 * 1.80f - 11.39f));

                }

                #region " Charges "


                float row_margin = 1.50f;
                float row_ = 21.8f;
                int counter = 0;
                float totalcharge = 0;
                float totalPaidAmount = 0;

                DSHCFA.HCFAChargesDataTable extra_charges = new DSHCFA.HCFAChargesDataTable();

                foreach (DataRow item in dsTemp.HCFACharges.Rows)
                {
                    DSHCFA.HCFAChargesRow charges = (DSHCFA.HCFAChargesRow)item;

                    if (counter == 3 || counter == 4)
                        row_margin = row_margin - 1f;
                    else if (counter == 5)
                        row_margin = row_margin - 1.5f;

                    if (counter < 6)
                    {
                        //24
                        //from
                        pdf.WriteText(charges.HCFA_24_A_From_MM, pdf.Column * 2 - 4.39f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));
                        pdf.WriteText(charges.HCFA_24_A_From_DD, pdf.Column * 3 + 2.39f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));
                        pdf.WriteText(charges.HCFA_24_A_From_YY, pdf.Column * 4 + 9.39f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));

                        //to
                        pdf.WriteText(charges.HCFA_24_A_To_MM, pdf.Column * 6 - 2.39f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));
                        pdf.WriteText(charges.HCFA_24_A_To_DD, pdf.Column * 8 - 4.39f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));
                        pdf.WriteText(charges.HCFA_24_A_To_YY, pdf.Column * 9 + 4.39f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));

                        pdf.WriteText(charges.HCFA_24_B, pdf.Column * 10 + 6.39f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));
                        pdf.WriteText(charges.HCFA_24_C, pdf.Column * 11 + 15.39f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));
                        pdf.WriteText(charges.HCFA_24_D_CPT, pdf.Column * 13 + 11.39f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));

                        //modifier
                        pdf.WriteText(charges.HCFA_24_D_MOD_1, pdf.Column * 17 + 4.19f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));
                        pdf.WriteText(charges.HCFA_24_D_MOD_2, pdf.Column * 18 + 12.39f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));
                        pdf.WriteText(charges.HCFA_24_D_MOD_3, pdf.Column * 20 + 6.19f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));
                        pdf.WriteText(charges.HCFA_24_D_MOD_4, pdf.Column * 21 + 14.95f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));

                        //pointers
                        pdf.WriteText(charges.HCFA_24_E, pdf.Column * 23 + 8.95f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));

                        //charges
                        pdf.WriteText(charges.HCFA_24_F_A, pdf.Column * 27 - 6.39f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));
                        pdf.WriteText(charges.HCFA_24_F_B, pdf.Column * 29 + 5.95f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));

                        pdf.WriteText(charges.HCFA_24_G, pdf.Column * 31 + 2.95f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));
                        pdf.WriteText(charges.HCFA_24_H, pdf.Column * 32 + 12.95f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));
                        pdf.WriteText(charges.HCFA_24_J, pdf.Column * 35.24f - 2.39f, pdf.RowMargin * row_ - (7 * 4.10f - row_margin));

                        //NDC
                        pdf.WriteText(charges.NDC + "  " + charges.NDCUnit + charges.NDCMeasurementCode + " " + charges.LineNotes + " " + charges.AnesthesiaTime, pdf.Column * 2 - 4.39f, pdf.RowMargin * row_ - (7 * 6.10f - row_margin));

                        if (IsRequired())
                        {
                            //Tax ID Number
                            if (!string.IsNullOrEmpty(claims.HCFA_24_J))
                            {
                                pdf.WriteText(claims.HCFA_24_I + claims.HCFA_24_J, pdf.Column * 35.24f - 2.39f, pdf.RowMargin * row_ - (7 * 5.50f - row_margin + 1.50f));
                            }
                        }

                        totalcharge += (MDVUtility.Tofloat(charges.HCFA_24_F_A + "." + charges.HCFA_24_F_B));
                        totalPaidAmount += (MDVUtility.Tofloat(charges.HCFA_29_A + "." + charges.HCFA_29_B));

                    }
                    else
                    {
                        //add extra rows
                        extra_charges.ImportRow(charges);
                    }

                    row_margin = row_margin - 2;
                    row_++;
                    counter++;
                }

                #endregion

                #region " Total Charge "

                string HCFA_28_A = "00";
                string HCFA_28_B = "00";

                if (totalcharge != 0)
                {
                    string[] charge = MDVUtility.ToStr(String.Format("{0:0.00}", totalcharge)).Split('.');
                    HCFA_28_A = charge[0];

                    if (charge.Length > 1)
                        HCFA_28_B = charge[1];
                    else
                        HCFA_28_B = "00";
                }

                //28
                pdf.WriteText(HCFA_28_A, pdf.Column * 30.3f - 2.39f, pdf.RowMargin * 27 - (7 * 3.30f + 2f), false, iTextSharp.text.pdf.PdfContentByte.ALIGN_RIGHT);
                pdf.WriteText(HCFA_28_B, pdf.Column * 30 + 6.99f, pdf.RowMargin * 27 - (7 * 3.30f + 2f));

                #endregion

                #region " Total Paid Amount "

                string HCFA_29_A = "00";
                string HCFA_29_B = "00";

                if (totalPaidAmount != 0)
                {

                    string[] amount = MDVUtility.ToStr(String.Format("{0:0.00}", totalPaidAmount)).Split('.');
                    HCFA_29_A = amount[0];

                    if (amount.Length > 1)
                        HCFA_29_B = amount[1];
                    else
                        HCFA_29_B = "00";

                }

                //29
                pdf.WriteText(HCFA_29_A, pdf.Column * 35.3f - 2.39f, pdf.RowMargin * 27 - (7 * 3.30f + 2f), false, iTextSharp.text.pdf.PdfContentByte.ALIGN_RIGHT);
                pdf.WriteText(HCFA_29_B, pdf.Column * 36 - 7.39f, pdf.RowMargin * 27 - (7 * 3.30f + 2f));

                #endregion

                #endregion

                #endregion

                if (IsBackground)
                {
                    imageFilePath = MDVUtility.GetImagePath(MDVSession.Current.ImagePath, "bgfill_form", "jpg");
                    //string imageFilePath = (System.AppDomain.CurrentDomain.BaseDirectory).Replace("\\", "/") + (SharedObj.ImagePath).Replace("~/", "") + "/bgfill_form.jpg";
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageFilePath);
                    img.ScaleToFit(3500, 800);
                    img.Alignment = iTextSharp.text.Image.UNDERLYING;
                    img.SetAbsolutePosition(-4f, -4);
                    pdf.Document.Add(img);
                }

                // Print Extra Charges more then 6
                if (extra_charges.Rows.Count > 0)
                {
                    DSHCFA ds_extraCharges = new DSHCFA();
                    ds_extraCharges.Merge(dsTemp.HCFAClaims);
                    ds_extraCharges.Merge(extra_charges);

                    CreatePageForIE(pdf, ds_extraCharges, IsBackground);
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::CreatePage", ex);

            }
        }

        public float GetServiceLinePaidAmount(long VisitId, long ChargeId, string CPTCode, DSHCFA._837SVDServiceLineDataTable dsSVD)
        {
            float PaidAmount = 0;
            float WriteOffAmount = 0;
            try
            {

                string expression = dsSVD.VisitIdColumn.ColumnName + " = " + VisitId
                    + " and " + dsSVD.ChargeIdColumn.ColumnName + " = " + ChargeId
                    + " and " + dsSVD.SVD03_2Column.ColumnName + " = " + MDVUtility.ToLINQFormatString(CPTCode);

                DataRow[] foundRows = dsSVD.Select(expression);
                foreach (var item in foundRows)
                {
                    DSHCFA._837SVDServiceLineRow dr = (DSHCFA._837SVDServiceLineRow)item;
                    PaidAmount += MDVUtility.Tofloat(dr.AMT02);
                    WriteOffAmount += MDVUtility.Tofloat(dr.CAS03_B);
                }

                //PMS-2634
                // return PaidAmount + WriteOffAmount;
                return PaidAmount;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::GetServiceLinePaidAmount", ex);
                throw new Exception(ex.Message);
            }
        }

        public BLObject<string> Genrate837EDIString(List<long> ListVisits, long ChearningHouseId)
        {
            try
            {
                DSHCFA ds = new DSHCFA();
                String ControlNUmber = "609201501";
                string EDIString = null;
                ds = new DAL837().Load837Header(ChearningHouseId);

                if (ds._837Header.Rows.Count > 0)
                {
                    // Set Control Number
                    ds._837Header.Rows[0][ds._837Header.ISA13Column] = ControlNUmber;
                    ds._837Header.Rows[0][ds._837Header.GS06Column] = ControlNUmber;
                    ds._837Header.Rows[0][ds._837Header.ST02Column] = ControlNUmber;
                    ds._837Header.Rows[0][ds._837Header.BHT03Column] = ControlNUmber;
                    ds.AcceptChanges();

                    ds.Merge(new DAL837().LoadClaimSubmission(string.Join(",", ListVisits.Select(n => n.ToString()).ToArray()), 0));
                    ds.Merge(new DAL837().LoadClaimICDS(string.Join(",", ListVisits.Select(n => n.ToString()).ToArray())));

                    //foreach (long VisitId in ListVisits)
                    //{
                    //    ds.Merge(new DAL837().Load837NamesByVisitId(VisitId));
                    //    ds.Merge(new DAL837().Load837Claim(VisitId, 0));
                    //    ds.Merge(new DAL837().Load837ServiceLine(VisitId, 0));
                    //    ds.Merge(new DAL837().Load837ServiceLineSVDByVisitId(VisitId));
                    //}

                    //Merge Header
                    DSHCFA ds_Temp = new DSHCFA();
                    ds_Temp._837Header.Merge(ds._837Header);

                    //Merge Claim ICDS
                    ds_Temp.VisitICDs.Merge(ds.VisitICDs);


                    //Order Name
                    DSHCFA._837NameRow[] name_rows = (DSHCFA._837NameRow[])ds.Tables[ds._837Name.TableName].Select("1=1", "NM101, NM103, NM104, VisitId DESC");
                    foreach (var item in name_rows)
                    {
                        ds_Temp._837Name.ImportRow(item);
                    }


                    //Order 837Claim
                    DSHCFA._837ClaimRow[] claim_rows = (DSHCFA._837ClaimRow[])ds.Tables[ds._837Claim.TableName].Select("1=1", "VisitId DESC");
                    foreach (var item in claim_rows)
                    {
                        ds_Temp._837Claim.ImportRow(item);
                    }

                    //Order 837ServiceLine
                    DSHCFA._837ServiceLineRow[] ServiceLine_rows = (DSHCFA._837ServiceLineRow[])ds.Tables[ds._837ServiceLine.TableName].Select("1=1", "VisitId DESC");
                    foreach (var item in ServiceLine_rows)
                    {
                        ds_Temp._837ServiceLine.ImportRow(item);
                    }

                    //Order SVD 837ServiceLine
                    DSHCFA._837SVDServiceLineRow[] SVDServiceLine_rows = (DSHCFA._837SVDServiceLineRow[])ds.Tables[ds._837SVDServiceLine.TableName].Select("1=1", "VisitId DESC");
                    foreach (var item in SVDServiceLine_rows)
                    {
                        ds_Temp._837SVDServiceLine.ImportRow(item);
                    }


                    EDI837Parser parser = new EDI837Parser(ref ds_Temp);
                    EDIString = parser.Get837();

                    return new BLObject<string>(EDIString);
                }
                else
                    throw new Exception("Please setup the EDI receiver ID and submitter ID for selected clearing house.");
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Genrate837EDIString", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> Genrate837EDIString(List<long> ListVisits, long ChearningHouseId, ref DSHCFA RefTable)
        {
            try
            {
                DSHCFA ds = new DSHCFA();
                String ControlNUmber = "609201501";
                string EDIString = null;
                ds = new DAL837().Load837Header(ChearningHouseId);

                if (ds._837Header.Rows.Count > 0)
                {
                    // Set Control Number
                    ds._837Header.Rows[0][ds._837Header.ISA13Column] = ControlNUmber;
                    ds._837Header.Rows[0][ds._837Header.GS06Column] = ControlNUmber;
                    ds._837Header.Rows[0][ds._837Header.ST02Column] = ControlNUmber;
                    ds._837Header.Rows[0][ds._837Header.BHT03Column] = ControlNUmber;
                    ds.AcceptChanges();

                    ds.Merge(new DAL837().LoadClaimSubmission(string.Join(",", ListVisits.Select(n => n.ToString()).ToArray()), 0, true));
                    ds.Merge(new DAL837().LoadClaimICDS(string.Join(",", ListVisits.Select(n => n.ToString()).ToArray())));

                    //foreach (long VisitId in ListVisits)
                    //{
                    //    ds.Merge(new DAL837().Load837NamesByVisitId(VisitId));
                    //    ds.Merge(new DAL837().Load837Claim(VisitId, 0));
                    //    ds.Merge(new DAL837().Load837ServiceLine(VisitId, 0));
                    //    ds.Merge(new DAL837().Load837ServiceLineSVDByVisitId(VisitId));
                    //}

                    //Merge Header
                    DSHCFA ds_Temp = new DSHCFA();
                    ds_Temp._837Header.Merge(ds._837Header);

                    //Merge Claim ICDS
                    ds_Temp.VisitICDs.Merge(ds.VisitICDs);

                    //To scrub only those claims their Provider allowed to scrub.
                    Dictionary<long, bool> ClaimsScrubList = new Dictionary<long, bool>();
                    bool HaveAnyClaim = false;

                    //Order 837Claim
                    DSHCFA._837ClaimRow[] claim_rows = (DSHCFA._837ClaimRow[])ds.Tables[ds._837Claim.TableName].Select("1=1", "VisitId DESC");
                    foreach (var item in claim_rows)
                    {
                        ClaimsScrubList.Add(MDVUtility.ToLong(item.VisitId), item.IsScrubClaim);
                        if (item.IsScrubClaim == true)
                        {
                            ds_Temp._837Claim.ImportRow(item);
                            RefTable._837Claim.ImportRow(item);
                            HaveAnyClaim = true;
                        }

                    }

                    //Order Name
                    DSHCFA._837NameRow[] name_rows = (DSHCFA._837NameRow[])ds.Tables[ds._837Name.TableName].Select("1=1", "NM101, NM103, NM104, VisitId DESC");
                    foreach (var item in name_rows)
                    {
                        if (ClaimsScrubList.FirstOrDefault(p => p.Key == MDVUtility.ToLong(item.VisitId)).Value == true)
                        {
                            // Case when claim type is paper or EDI submitter setup for that insurance so payer ID will be empty
                            // in that case hard code the Payer ID to scrub claim.
                            if (item.NM101 == "PR" && string.IsNullOrEmpty(item.NM109))
                                item.NM109 = "123456789";
                            ds_Temp._837Name.ImportRow(item);
                        }

                    }


                    //Order 837ServiceLine
                    DSHCFA._837ServiceLineRow[] ServiceLine_rows = (DSHCFA._837ServiceLineRow[])ds.Tables[ds._837ServiceLine.TableName].Select("1=1", "VisitId DESC");
                    foreach (var item in ServiceLine_rows)
                    {
                        if (ClaimsScrubList.FirstOrDefault(p => p.Key == MDVUtility.ToLong(item.VisitId)).Value == true)
                        {
                            ds_Temp._837ServiceLine.ImportRow(item);
                        }
                    }

                    //Order SVD 837ServiceLine
                    DSHCFA._837SVDServiceLineRow[] SVDServiceLine_rows = (DSHCFA._837SVDServiceLineRow[])ds.Tables[ds._837SVDServiceLine.TableName].Select("1=1", "VisitId DESC");
                    foreach (var item in SVDServiceLine_rows)
                    {
                        if (ClaimsScrubList.FirstOrDefault(p => p.Key == MDVUtility.ToLong(item.VisitId)).Value == true)
                        {
                            ds_Temp._837SVDServiceLine.ImportRow(item);
                        }
                    }

                    if (HaveAnyClaim == true)
                    {
                        EDI837Parser parser = new EDI837Parser(ref ds_Temp);
                        EDIString = parser.Get837();
                        return new BLObject<string>(EDIString);
                    }
                    else
                    {
                        return new BLObject<string>(null, "selected provider is not enrolled to scrub the claim.");
                    }


                }
                else
                    throw new Exception("Please setup the EDI receiver ID and submitter ID for selected clearing house.");
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Genrate837EDIString", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> PrintClaimHistory(string VisitId, string PatientId)
        {
            try
            {
                VisitId = new DALVisits().PrintClaimHistory(VisitId, PatientId);
                return new BLObject<string>(VisitId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::PrintClaimHistory", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public bool IsRequired()
        {
            bool flag = true;
            try
            {
                List<string> DoNotReportG2ForEntityId = MDVUtility.ToStr(ConfigurationManager.AppSettings["DoNotReportG2ForEntityId"]).Split(',').ToList();
                if (DoNotReportG2ForEntityId.Contains(MDVSession.Current.EntityId))
                    flag = false;
            }
            catch (Exception)
            {
                flag = true;
            }

            return flag;
        }

        #region " Support Function "

        private List<DSHCFA> GetHCFAClaimsList(ref DSHCFA dsTemp)
        {
            List<DSHCFA> ds_list = new List<DSHCFA>();
            foreach (DSHCFA._837ClaimRow item_claim in dsTemp._837Claim)
            {
                DSHCFA._837NameRow[] name_rows = (DSHCFA._837NameRow[])dsTemp.Tables[dsTemp._837Name.TableName].Select(dsTemp._837Name.VisitIdColumn.ColumnName + "='" + item_claim.VisitId + "'");
                DSHCFA._837ServiceLineRow[] service_line_rows = (DSHCFA._837ServiceLineRow[])dsTemp.Tables[dsTemp._837ServiceLine.TableName].Select(dsTemp._837ServiceLine.VisitIdColumn.ColumnName + "='" + item_claim.VisitId + "'");
                DSHCFA._837SVDServiceLineRow[] svd_service_line_rows = (DSHCFA._837SVDServiceLineRow[])dsTemp.Tables[dsTemp._837SVDServiceLine.TableName].Select(dsTemp._837SVDServiceLine.VisitIdColumn.ColumnName + "='" + item_claim.VisitId + "'");
                DSHCFA.VisitICDsRow[] icds_service_line_rows = (DSHCFA.VisitICDsRow[])dsTemp.Tables[dsTemp.VisitICDs.TableName].Select(dsTemp.VisitICDs.VisitIdColumn.ColumnName + "='" + item_claim.VisitId + "'");


                DSHCFA dsTemp_ = new DSHCFA();
                dsTemp_._837Claim.ImportRow(item_claim);
                foreach (var item_name in name_rows)
                    dsTemp_._837Name.ImportRow(item_name);
                foreach (var item_service in service_line_rows)
                    dsTemp_._837ServiceLine.ImportRow(item_service);
                foreach (var item_svd in svd_service_line_rows)
                    dsTemp_._837SVDServiceLine.ImportRow(item_svd);
                foreach (var item_icds in icds_service_line_rows)
                    dsTemp_.VisitICDs.ImportRow(item_icds);

                ds_list.Add(dsTemp_);
            }

            return ds_list;
        }

        private static string GetHcfaDay(string in_date)
        {
            try
            {
                return in_date.Substring(6, 2);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        private static string GetHcfaMonth(string in_date)
        {
            try
            {
                return in_date.Substring(4, 2);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        private static string GetHcfaYear(string in_date, bool isFull = false)
        {
            try
            {
                if (!isFull)
                    return in_date.Substring(2, 2);
                else
                    return in_date.Substring(0, 4);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private bool ToBool(string val)
        {
            try
            {
                return Convert.ToBoolean(val);
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region " Patient Statement "

        public BLObject<bool> UploadPatientStatement(SharedVariable SharedVariable, string WinscpPatch, long ClearingHouseId, string PatientStatementXML)
        {
            try
            {
                string fileName = "";
                DSEDI dsEDI = new DSEDI();

                BLLBillingClaim objBillingClaim = new BLLBillingClaim();
                dsEDI = new DALClearingHouse(SharedVariable).LoadClearingHouse(SharedVariable, ClearingHouseId, null, null, null, null);
                if (dsEDI.ClearingHouse.Rows.Count > 0)
                {
                    string ftp = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTPColumn]);
                    string userName = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserNameColumn]);
                    string userPassword = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserPasswordColumn]);
                    Int32 ftpPortNo = MDVUtility.ToInt32(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_PORTNOColumn]);
                    string ftpHostKey = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_HOSTKEYColumn]);
                    string FTPUploadFolder = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.IN_STATEMENTSColumn]);
                    string PatientStatementExtension = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.PatientStatementExtensionColumn]);

                    Guid strGuid = Guid.NewGuid();
                    //string WinscpPatch = string.Empty;
                    //if (HttpContext.Current != null)
                    //    WinscpPatch = MDVUtility.GetDLLPath("~/DLL", "winscp", "exe");
                    //else
                    //    WinscpPatch = System.Configuration.ConfigurationManager.AppSettings["WinSCP_Path"];

                    //SharedVariable SharedVariable = new SharedVariable();
                    //SharedVariable.EntityId = MDVSession.Current.EntityId;
                    //SharedVariable.AppUserId = MDVSession.Current.AppUserId;
                    //SharedVariable.UserName = MDVSession.Current.AppUserName;
                    //SharedVariable.ClientId = MDVSession.Current.ClientId;
                    //SharedVariable.AppPassWord = MDVSession.Current.AppPassWord;
                    //SharedVariable.WebEntityURL = MDVSession.Current.WebEntityURL;

                    fileName = new ClaimSubmission(SharedVariable, WinscpPatch).UploadFileToFTP(PatientStatementXML, strGuid.ToString(), ftp, userName, userPassword, ftpPortNo, FTPUploadFolder, PatientStatementExtension, ftpHostKey, true);
                    return new BLObject<bool>(true);
                }
                else
                {
                    return new BLObject<bool>(false, "Failed to submit");
                }

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLBilling::UploadPatientStatement", ex);
                return new BLObject<bool>(false, ex.Message);
            }
        }

        #endregion

        #endregion



        #region "837 Batch"
        public BLObject<DS837Batch> Load837BatchClaim(long Batch837ClaimId)
        {
            try
            {
                DS837Batch ds = new DS837Batch();
                ds = new DAL837().Load837BatchClaim(Batch837ClaimId);

                return new BLObject<DS837Batch>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Load837BatchClaim", ex);
                return new BLObject<DS837Batch>(null, ex.Message);
            }
        }
        public BLObject<DS837Batch> Load837Batch(long Batch837Id, string BatchNumber, string ClearingHouseId, DateTime? SubmitDate, long SubmittedBy, string SubmitType, string IsCompleted, string BatchStatus, int PageNumber = 1, int RowspPage = 15)
        {
            try
            {
                DS837Batch ds = new DS837Batch();
                ds = new DAL837().Load837Batch(Batch837Id, BatchNumber, ClearingHouseId, SubmitDate, SubmittedBy, SubmitType, IsCompleted, BatchStatus, PageNumber, RowspPage);

                return new BLObject<DS837Batch>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Load837Batch", ex);
                return new BLObject<DS837Batch>(null, ex.Message);
            }
        }

        public BLObject<DS837Batch> Load837BatchSearch(long Batch837Id, string BatchNumber, string ClearingHouseId, DateTime? SubmitDate, long SubmittedBy, string SubmitType, string IsCompleted, string BatchStatus, int PageNumber = 1, int RowspPage = 15)
        {
            try
            {
                DS837Batch ds = new DS837Batch();
                ds = new DAL837().Load837BatchSearch(Batch837Id, BatchNumber, ClearingHouseId, SubmitDate, SubmittedBy, SubmitType, IsCompleted, BatchStatus, PageNumber, RowspPage);

                return new BLObject<DS837Batch>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Load837Batch", ex);
                return new BLObject<DS837Batch>(null, ex.Message);
            }
        }

        public BLObject<DS837Batch> Update837Batch(DS837Batch ds)
        {
            try
            {
                ds = new DAL837().Update837Batch(ds);
                return new BLObject<DS837Batch>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Update837Batch", ex);
                return new BLObject<DS837Batch>(null, ex.Message);
            }
        }

        //public BLObject<DS837Batch> Insert837BatchClaim(DS837Batch ds)
        //{
        //    try
        //    {
        //        ds = new DAL837().Insert837BatchClaim(ds);
        //        return new BLObject<DS837Batch>(ds);

        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLBillingClaim::Insert837BatchClaim", ex);
        //        return new BLObject<DS837Batch>(null, ex.Message);
        //    }
        //}
        //public BLObject<DS837Batch> Update837BatchClaim(DS837Batch ds)
        //{
        //    try
        //    {
        //        ds = new DAL837().Update837BatchClaim(ds);
        //        return new BLObject<DS837Batch>(ds);

        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLBillingClaim::Update837BatchClaim", ex);
        //        return new BLObject<DS837Batch>(null, ex.Message);
        //    }
        //}

        public BLObject<string> Delete837BatchClaim(long Batch837ClaimId)
        {
            try
            {
                return new BLObject<string>(new DAL837().Delete837BatchClaim(Batch837ClaimId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Delete837BatchClaim", ex);
                return new BLObject<string>("", ex.Message);
            }
        }


        public BLObject<string> Update837BatchStatusResubmit(int BatchId, int VisitId)
        {

            try
            {
                return new BLObject<string>(new DAL837().Update837BatchStatusResubmit(BatchId, VisitId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Update837BatchStatusResubmit", ex);
                return new BLObject<string>("", ex.Message);
            }

        }

        #endregion

        #endregion

        #region "835 Reports"


        public BLObject<DSEDIReports> GetLatestReports(long clearingHouseId, bool isFromService = false)
        {
            //var dsEdiReports = new DALEDIReports().LoadEDIReports(0, 0, clearingHouseId, null, null, null, null);
            object obj = new object();
            // For locking functionality
            lock (obj)
            {
                SharedVariable SharedVariable = new SharedVariable();
                SharedVariable.EntityId = MDVSession.Current.EntityId;
                SharedVariable.AppUserId = MDVSession.Current.AppUserId;
                SharedVariable.UserName = MDVSession.Current.AppUserName;
                SharedVariable.ClientId = MDVSession.Current.ClientId;
                SharedVariable.AppPassWord = MDVSession.Current.AppPassWord;
                SharedVariable.WebEntityURL = MDVSession.Current.WebEntityURL;

                try
                {
                    DSEDI dsEDI = new DSEDI();
                    dsEDI = new DALClearingHouse().LoadClearingHouse(clearingHouseId, null, null, null, null);

                    DSEDIReports dsEdiReport = new DSEDIReports();
                    DSEDIReports dsMainEdiReport = new DSEDIReports();
                    string ftp = Convert.ToString(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTPColumn.ColumnName]);
                    string userName = Convert.ToString(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserNameColumn.ColumnName]);
                    string userPassword = Convert.ToString(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserPasswordColumn.ColumnName]);

                    string ftpPN = dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_PORTNOColumn.ColumnName].ToString();
                    if (ftpPN != "")
                    {
                        Int32 ftpPortNo = Convert.ToInt32(ftpPN);
                        string ftpHostKey = Convert.ToString(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_HOSTKEYColumn.ColumnName]);
                        string WinscpPatch = string.Empty;
                        if (HttpContext.Current != null)
                            WinscpPatch = MDVUtility.GetDLLPath("~/DLL", "winscp", "exe");
                        else
                            WinscpPatch = System.Configuration.ConfigurationManager.AppSettings["WinSCP_Path"];
                        //dsEDI.ClearingHouse.Columns.Contains(dsEDI.ClearingHouse.OUT_835Column.ColumnName);

                        foreach (DSEDI.ClearingHouseRow drClearingHouse in dsEDI.ClearingHouse.Rows)
                        {
                            //if (string.IsNullOrEmpty(drClearingHouse[dsEDI.ClearingHouse.OUT_835Column.ColumnName.ToString()].ToString()))
                            //throw new Exception("FTP is not setup for the selected clearing house.");

                            if (dsEDI.ClearingHouse.Columns.Contains(dsEDI.ClearingHouse.OUT_835Column.ColumnName))
                            {
                                if (!drClearingHouse.IsNull(dsEDI.ClearingHouse.OUT_835Column))
                                {
                                    dsEdiReport = new ClaimSubmission(SharedVariable, WinscpPatch).DownloadsEDIReport(clearingHouseId, userName, userPassword, ftp, ftpPortNo, ftpHostKey, drClearingHouse.OUT_835.ToString(), "835", "ERA (ELECTRONIC REMITTANCE ADVICE)", isFromService);
                                    dsMainEdiReport.Merge(dsEdiReport);
                                }
                            }
                            if (dsEDI.ClearingHouse.Columns.Contains(dsEDI.ClearingHouse.OUT_271Column.ColumnName))
                            {
                                if (!drClearingHouse.IsNull(dsEDI.ClearingHouse.OUT_271Column.ToString()))
                                {
                                    dsEdiReport = new ClaimSubmission(SharedVariable, WinscpPatch).DownloadsEDIReport(clearingHouseId, userName, userPassword, ftp, ftpPortNo, ftpHostKey, drClearingHouse.OUT_271.ToString(), "271", "ELIGIBILITY RESPONSE", isFromService);
                                    dsMainEdiReport.Merge(dsEdiReport);
                                }
                            }
                            if (dsEDI.ClearingHouse.Columns.Contains(dsEDI.ClearingHouse.OUT_277Column.ColumnName))
                            {
                                if (!drClearingHouse.IsNull(dsEDI.ClearingHouse.OUT_277Column.ToString()))
                                {
                                    dsEdiReport = new ClaimSubmission(SharedVariable, WinscpPatch).DownloadsEDIReport(clearingHouseId, userName, userPassword, ftp, ftpPortNo, ftpHostKey, drClearingHouse.OUT_277.ToString(), "277", "CLAIM STATUS RESPONSE", isFromService);
                                    dsMainEdiReport.Merge(dsEdiReport);
                                }
                            }
                            if (dsEDI.ClearingHouse.Columns.Contains(dsEDI.ClearingHouse.OUT_997Column.ColumnName))
                            {
                                if (!drClearingHouse.IsNull(dsEDI.ClearingHouse.OUT_997Column.ToString()))
                                {
                                    dsEdiReport = new ClaimSubmission(SharedVariable, WinscpPatch).DownloadsEDIReport(clearingHouseId, userName, userPassword, ftp, ftpPortNo, ftpHostKey, drClearingHouse.OUT_997.ToString(), "997", "ACKNOWLEDGEMENT", isFromService);
                                    dsMainEdiReport.Merge(dsEdiReport);
                                }
                            }
                            if (dsEDI.ClearingHouse.Columns.Contains(dsEDI.ClearingHouse.OUT_REPORTSColumn.ColumnName))
                            {
                                if (!drClearingHouse.IsNull(dsEDI.ClearingHouse.OUT_REPORTSColumn.ToString()))
                                {
                                    // Title against Each Report
                                    dsEdiReport = new ClaimSubmission(SharedVariable, WinscpPatch).DownloadsEDIReport(clearingHouseId, userName, userPassword, ftp, ftpPortNo, ftpHostKey, drClearingHouse.OUT_REPORTS.ToString(), "REPORTS", "REPORTS", isFromService);
                                    dsMainEdiReport.Merge(dsEdiReport);
                                }
                            }

                        }
                    }
                    return new BLObject<DSEDIReports>(dsMainEdiReport);
                }
                catch (Exception ex)
                {
                    MDVLogger.BLLErrorLog(SharedVariable, "BLLBillingClaim::GetLatestReports", ex);
                    return new BLObject<DSEDIReports>(null, ex.Message);
                }
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable">Variables that are no in MDVSession Context </param>
        /// <param name="clearingHouseId"></param>
        /// <param name="isFromService"></param>
        /// <returns></returns>
        public BLObject<DSEDIReports> GetLatestReports(SharedVariable SharedVariable, long clearingHouseId, bool isFromService = false)
        {
            //var dsEdiReports = new DALEDIReports().LoadEDIReports(0, 0, clearingHouseId, null, null, null, null);
            object obj = new object();
            // For locking functionality
            lock (obj)
            {
                try
                {
                    DSEDI dsEDI = new DSEDI();
                    dsEDI = new DALClearingHouse(SharedVariable).LoadClearingHouse(SharedVariable, clearingHouseId, null, null, null, null);

                    DSEDIReports dsEdiReport = new DSEDIReports();
                    DSEDIReports dsMainEdiReport = new DSEDIReports();
                    string ftp = Convert.ToString(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTPColumn.ColumnName]);
                    string userName = Convert.ToString(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserNameColumn.ColumnName]);
                    string userPassword = Convert.ToString(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserPasswordColumn.ColumnName]);

                    string ftpPN = dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_PORTNOColumn.ColumnName].ToString();
                    if (ftpPN != "")
                    {

                        Int32 ftpPortNo = Convert.ToInt32(ftpPN);
                        string ftpHostKey = Convert.ToString(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_HOSTKEYColumn.ColumnName]);
                        string WinscpPatch = string.Empty;
                        if (HttpContext.Current != null)
                            WinscpPatch = MDVUtility.GetDLLPath("~/DLL", "winscp", "exe");
                        else
                            WinscpPatch = System.Configuration.ConfigurationManager.AppSettings["WinSCP_Path"];
                        //dsEDI.ClearingHouse.Columns.Contains(dsEDI.ClearingHouse.OUT_835Column.ColumnName);

                        foreach (DSEDI.ClearingHouseRow drClearingHouse in dsEDI.ClearingHouse.Rows)
                        {
                            //if (string.IsNullOrEmpty(drClearingHouse[dsEDI.ClearingHouse.OUT_835Column.ColumnName.ToString()].ToString()))
                            //throw new Exception("FTP is not setup for the selected clearing house.");

                            if (dsEDI.ClearingHouse.Columns.Contains(dsEDI.ClearingHouse.OUT_835Column.ColumnName))
                            {
                                if (!drClearingHouse.IsNull(dsEDI.ClearingHouse.OUT_835Column))
                                {
                                    dsEdiReport = new ClaimSubmission(SharedVariable, WinscpPatch).DownloadsEDIReport(clearingHouseId, userName, userPassword, ftp, ftpPortNo, ftpHostKey, drClearingHouse.OUT_835.ToString(), "835", "ERA (ELECTRONIC REMITTANCE ADVICE)", isFromService);
                                    dsMainEdiReport.Merge(dsEdiReport);
                                }
                            }
                            if (dsEDI.ClearingHouse.Columns.Contains(dsEDI.ClearingHouse.OUT_271Column.ColumnName))
                            {
                                if (!drClearingHouse.IsNull(dsEDI.ClearingHouse.OUT_271Column.ToString()))
                                {
                                    dsEdiReport = new ClaimSubmission(SharedVariable, WinscpPatch).DownloadsEDIReport(clearingHouseId, userName, userPassword, ftp, ftpPortNo, ftpHostKey, drClearingHouse.OUT_271.ToString(), "271", "ELIGIBILITY RESPONSE", isFromService);
                                    dsMainEdiReport.Merge(dsEdiReport);
                                }
                            }
                            if (dsEDI.ClearingHouse.Columns.Contains(dsEDI.ClearingHouse.OUT_277Column.ColumnName))
                            {
                                if (!drClearingHouse.IsNull(dsEDI.ClearingHouse.OUT_277Column.ToString()))
                                {
                                    dsEdiReport = new ClaimSubmission(SharedVariable, WinscpPatch).DownloadsEDIReport(clearingHouseId, userName, userPassword, ftp, ftpPortNo, ftpHostKey, drClearingHouse.OUT_277.ToString(), "277", "CLAIM STATUS RESPONSE", isFromService);
                                    dsMainEdiReport.Merge(dsEdiReport);
                                }
                            }
                            if (dsEDI.ClearingHouse.Columns.Contains(dsEDI.ClearingHouse.OUT_997Column.ColumnName))
                            {
                                if (!drClearingHouse.IsNull(dsEDI.ClearingHouse.OUT_997Column.ToString()))
                                {
                                    dsEdiReport = new ClaimSubmission(SharedVariable, WinscpPatch).DownloadsEDIReport(clearingHouseId, userName, userPassword, ftp, ftpPortNo, ftpHostKey, drClearingHouse.OUT_997.ToString(), "997", "ACKNOWLEDGEMENT", isFromService);
                                    dsMainEdiReport.Merge(dsEdiReport);
                                }
                            }
                            if (dsEDI.ClearingHouse.Columns.Contains(dsEDI.ClearingHouse.OUT_REPORTSColumn.ColumnName))
                            {
                                if (!drClearingHouse.IsNull(dsEDI.ClearingHouse.OUT_REPORTSColumn.ToString()))
                                {
                                    // Title against Each Report
                                    dsEdiReport = new ClaimSubmission(SharedVariable, WinscpPatch).DownloadsEDIReport(clearingHouseId, userName, userPassword, ftp, ftpPortNo, ftpHostKey, drClearingHouse.OUT_REPORTS.ToString(), "REPORTS", "REPORTS", isFromService);
                                    dsMainEdiReport.Merge(dsEdiReport);
                                }
                            }

                        }
                    }
                    return new BLObject<DSEDIReports>(dsMainEdiReport);
                }
                catch (Exception ex)
                {
                    MDVLogger.BLLErrorLog(SharedVariable, "BLLBillingClaim::GetLatestReports", ex);
                    return new BLObject<DSEDIReports>(null, ex.Message);
                }
            }
        }


        public void ParseOldEdiReports(SharedVariable sharedVariable)
        {
            //var dsEdiReports = new DALEDIReports().LoadEDIReports(0, 0, clearingHouseId, null, null, null, null);
            object obj = new object();
            // For locking functionality
            lock (obj)
            {
                try
                {
                    List<EDIReport> PreEDIReportList = new List<EDIReport>();
                    PreEDIReportList = new DALEDIReports(sharedVariable).LoadPreEdiReports();
                    List<STC> STCList = new List<STC>();
                    List<EDIReport> EDIReportList = new List<EDIReport>();

                    foreach (EDIReport rEDIReport in PreEDIReportList)
                    {
                        DS277 ds277 = new DS277();

                        BLObject<DS277> obj277 = new BLLBillingClaim().Report277CA(rEDIReport.EDIText, sharedVariable);
                        ds277 = obj277.Data;
                        getEdidetail(ds277, ref STCList, ref EDIReportList, rEDIReport.EDIReportId);
                    }


                    string result = new DALEDIReports(sharedVariable).SaveEdiReportDetail(MDVUtility.GetXmlOfObject(typeof(List<STC>), STCList), MDVUtility.GetXmlOfObject(typeof(List<EDIReport>), EDIReportList));
                    if (result != "")
                    {
                        throw new Exception(result);
                    }

                }
                catch (Exception ex)
                {
                    MDVLogger.BLLErrorLog(sharedVariable, "BLLBillingClaim::GetLatestReports", ex);
                }
            }
        }

        #endregion

        #region 835

        #region EDI Reports
        public BLObject<DSEDIReports> LoadEDIReports(long DSEDIReportsId, long clearingHouseId = 0, bool IsFormat = false, string FileName = null, DateTime? DOS = null, string ReviewStatus = null, string ReportType = null, string EDIText = null, string IsERADeleted = null, string IsParse = null, string CheckNo = "", DateTime? CreatedOn = null, int PageNumber = 1, int RowspPage = 1000)
        {
            try
            {
                DSEDIReports ds = new DSEDIReports();
                ds = new DALEDIReports().LoadEDIReports(DSEDIReportsId, clearingHouseId, FileName, ReviewStatus, ReportType, EDIText, IsERADeleted, DOS, IsParse, CreatedOn, PageNumber, RowspPage);

                if (IsFormat == true)
                {
                    foreach (DSEDIReports.EDIReportsRow row in ds.EDIReports.Rows)
                    {

                        if (row.ReportType.ToString() == "835")
                        {
                            EDIParser.dsCache ds835Parser = new EDIParser.dsCache();
                            row.EDIFormat = EDICommon.ParseEDI(row.EDIText, row.EDIReportId, row.FileName, ref ds835Parser, MDVSession.Current.AppUserName, "", "", false, false, CheckNo);
                        }

                    }
                    ds.AcceptChanges();
                }
                return new BLObject<DSEDIReports>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::LoadDSEDIReportss", ex);
                return new BLObject<DSEDIReports>(null, ex.Message);
            }
        }

        public BLObject<DSEDIReports> InsertEDIReportBatch(DSEDIReports ds, bool IsBatch)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            DSEDIReports dsEDIReports = new DSEDIReports();
            try
            {
                // Transaction Begin
                dbManager.BeginTransaction();
                dsEDIReports = new DALEDIReports().InsertEDIReports(ds, dbManager);

                if (IsBatch)
                {
                    long EDIReportId = Convert.ToInt64(dsEDIReports.EDIReports.Rows[0][dsEDIReports.EDIReports.EDIReportIdColumn]);
                    ds._837BatchReport.Rows[0][ds._837BatchReport.EDIReportIdColumn] = EDIReportId;
                    dsEDIReports = new DALEDIReports().Insert837BatchReports(ds, dbManager);
                }

                dbManager.CommitTransaction();
                return new BLObject<DSEDIReports>(dsEDIReports);
            }
            catch (Exception ex)
            {
                ds.RejectChanges();
                dbManager.RollBackTransaction();
                return new BLObject<DSEDIReports>(null, ex.Message);

            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public BLObject<DSEDIReports> InsertEDIReports(DSEDIReports ds)
        {
            try
            {
                ds = new DALEDIReports().InsertEDIReports(ds);
                return new BLObject<DSEDIReports>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::InsertEDIReports", ex);
                return new BLObject<DSEDIReports>(null, ex.Message);
            }

        }

        public BLObject<DSEDIReports> UpdateEDIReports(DSEDIReports ds)
        {
            try
            {
                ds = new DALEDIReports().UpdateEDIReports(ds);
                return new BLObject<DSEDIReports>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::UpdateEDIReports", ex);
                return new BLObject<DSEDIReports>(null, ex.Message);
            }

        }

        public BLObject<string> DeleteEDIReports(string EDIReportId)
        {
            try
            {
                EDIReportId = new DALEDIReports().DeleteEDIReports(EDIReportId);
                return new BLObject<string>(EDIReportId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::DeleteEDIReports", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        #endregion

        #region 837BatchReports
        public BLObject<DSEDIReports> Load837BatchReports(string BatchNumber, long EDIReportId, bool IsFormat = false)
        {
            try
            {
                DSEDIReports ds = new DSEDIReports();
                ds = new DALEDIReports().Load837BatchReports(BatchNumber, EDIReportId);

                if (IsFormat == true)
                {
                    foreach (DSEDIReports._837BatchReportRow row in ds._837BatchReport.Rows)
                    {

                        if (row.ReportType.ToString() == "835")
                        {
                            EDIParser.dsCache ds835Parser = new EDIParser.dsCache();
                            row.EDIFormat = EDICommon.ParseEDI(row.EDIText, row.EDIReportId, row.FileName, ref ds835Parser, MDVSession.Current.AppUserName);
                        }

                    }
                    ds.AcceptChanges();
                }

                return new BLObject<DSEDIReports>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Load837BatchReports", ex);
                return new BLObject<DSEDIReports>(null, ex.Message);
            }
        }

        public BLObject<DSEDIReports> Insert837BatchReports(DSEDIReports ds)
        {
            try
            {
                ds = new DALEDIReports().Insert837BatchReports(ds);
                return new BLObject<DSEDIReports>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Insert837BatchReports", ex);
                return new BLObject<DSEDIReports>(null, ex.Message);
            }

        }

        public BLObject<DSEDIReports> Update837BatchReports(DSEDIReports ds)
        {
            try
            {
                ds = new DALEDIReports().Update837BatchReports(ds);
                return new BLObject<DSEDIReports>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Update837BatchReports", ex);
                return new BLObject<DSEDIReports>(null, ex.Message);
            }

        }

        public BLObject<string> Delete837BatchReports(long BatchReportId)
        {
            try
            {
                string ReportId = new DALEDIReports().Delete837BatchReports(BatchReportId);
                return new BLObject<string>(ReportId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Delete837BatchReports", ex);
                return new BLObject<string>("", ex.Message);
            }
        }
        #endregion

        #region 837ClaimReports
        public BLObject<DSEDIReports> Load837ClaimReports(long ClaimReportId)
        {
            try
            {
                DSEDIReports ds = new DSEDIReports();
                ds = new DALEDIReports().Load837ClaimReports(ClaimReportId);
                return new BLObject<DSEDIReports>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Load837ClaimReports", ex);
                return new BLObject<DSEDIReports>(null, ex.Message);
            }
        }

        public BLObject<DSEDIReports> Insert837ClaimReports(DSEDIReports ds)
        {
            try
            {
                ds = new DALEDIReports().Insert837ClaimReports(ds);
                return new BLObject<DSEDIReports>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Insert837ClaimReports", ex);
                return new BLObject<DSEDIReports>(null, ex.Message);
            }

        }

        public BLObject<DSEDIReports> Update837ClaimReports(DSEDIReports ds)
        {
            try
            {
                ds = new DALEDIReports().Update837ClaimReports(ds);
                return new BLObject<DSEDIReports>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Update837ClaimReports", ex);
                return new BLObject<DSEDIReports>(null, ex.Message);
            }

        }

        public BLObject<string> Delete837ClaimReports(long ClaimReportId)
        {
            try
            {
                string ReportId = new DALEDIReports().Delete837ClaimReports(ClaimReportId);
                return new BLObject<string>(ReportId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Delete837ClaimReports", ex);
                return new BLObject<string>("", ex.Message);
            }
        }
        #endregion

        #endregion

        #region " Electronic EOB "
        public BLObject<string> ElectronicEOB(long chargeId, long visitId)
        {
            try
            {
                //1- Load EDI Text and ERAClaimNumber.
                DSERA dsEra = new DSERA();
                dsEra = new DALERA().LoadElectronicEOB(chargeId, visitId);
                if (dsEra.ElectronicEOB.Rows.Count > 0)
                {
                    string StrEOB = string.Empty;
                    int count = 0;
                    foreach (var item in dsEra.ElectronicEOB.Rows)
                    {
                        DSERA.ElectronicEOBRow row = (DSERA.ElectronicEOBRow)item;
                        EDIParser.dsCache ds835Parser = new EDIParser.dsCache();
                        string strtemp = "";
                        //if (visitId != 0)
                        //    strtemp = EDICommon.ParseEDI(row.EDIText, row.EDIReportId, row.FileName, ref ds835Parser, SharedObj.UserName);
                        //else
                        bool IsFilterCharges = false;
                        if (chargeId != 0)
                            IsFilterCharges = true;

                        strtemp = EDICommon.ParseEDI(row.EDIText, row.EDIReportId, row.FileName, ref ds835Parser, MDVSession.Current.AppUserName, row.ERAClaimNumber, row.ERAChargeNumber, IsFilterCharges, true);

                        count++;
                        strtemp += "\r\n<h3 id='CheckCount' class='blue' style='text-align: center !important;;'>" + count + "</h3>";
                        if (count < dsEra.ElectronicEOB.Rows.Count && strtemp != "")
                            strtemp += "<hr id='CheckBreak' class='line-dashed'>\r\n";

                        StrEOB += strtemp;
                    }

                    return new BLObject<string>(StrEOB);
                }
                else
                {
                    return new BLObject<string>(null, "No record Found.");
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::ElectronicEOB", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region " 277CA "

        public BLObject<DS277> Report277CA(string Str277, SharedVariable sharedVariable = null)
        {
            try
            {
                DS277 ds277 = new DS277();
                EDI277Parser obj277 = new EDI277Parser();

                //Load ClaimStatusCode
                //Load ClaimStatusCategoryCode
                DSFollowUp dsCodes = new DSFollowUp();
                if (sharedVariable != null)
                {
                    dsCodes.Merge(new DALFollowUpClaimStatusCategoryCode(sharedVariable).LoadClaimStatusCategoryCode(0, null, null, "1", 1, 50000));
                    dsCodes.Merge(new DALFollowUpClaimStatusCode(sharedVariable).LoadClaimStatusCode(0, null, null, "1", 1, 50000));
                }
                else
                {
                    dsCodes.Merge(new DALFollowUpClaimStatusCategoryCode().LoadClaimStatusCategoryCode(0, null, null, "1", 1, 50000));
                    dsCodes.Merge(new DALFollowUpClaimStatusCode().LoadClaimStatusCode(0, null, null, "1", 1, 50000));
                }
                obj277.GetHumanReadable277(Str277, ref ds277, dsCodes, dsCodes.ClaimStatusCategoryCode.TableName, dsCodes.ClaimStatusCode.TableName, dsCodes.ClaimStatusCode.CodeColumn.ColumnName, dsCodes.ClaimStatusCode.DescriptionColumn.ColumnName);
                return new BLObject<DS277>(ds277);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBillingClaim::Report277CA", ex);
                return new BLObject<DS277>(null, ex.Message);
            }
        }


        public void getEdidetail(DS277 ds, ref List<STC> STCList, ref List<EDIReport> EDIReportList, Int64 EDIReportId)
        {
            var TotalRejected = 0;
            var TotalAccepted = 0;
            foreach (DS277.EDI277NamesRow EDI277Names in ds.EDI277Names)
            {
                if (EDI277Names.NM101_QUL == "85")
                {
                    var ProviderName = EDI277Names.NM103;
                    var NPI = EDI277Names.NM109;

                    foreach (DS277.EDI277NamesRow Patient_item in ds.EDI277Names)
                    {

                        if (Patient_item.ParentNameId == EDI277Names.EDI277NameId)
                        {
                            //Select Patient STC rows
                            foreach (DS277.EDI277StatusRow Status_item in ds.EDI277Status)
                            {

                                if (Status_item.EDI277NameId == Patient_item.EDI277NameId)
                                {
                                    STC stc = new STC();
                                    stc.ClaimNumber = Patient_item.TRN02;
                                    stc.ChargeAmount = Status_item.STC04;
                                    stc.PaidAmount = Status_item.STC05;
                                    stc.ClaimCategoryCode = Status_item.STC01_1_QUL + ": " + Status_item.STC01_1;
                                    stc.ClaimStatusCode = Status_item.STC01_2_QUL + ": " + Status_item.STC01_2;
                                    stc.EDIReportId = MDVUtility.ToStr(EDIReportId);
                                    if (Status_item.STC03.ToLower() == "accept")
                                    {
                                        stc.isAccepted = true;
                                        TotalAccepted++;
                                    }
                                    else
                                    {
                                        if (Status_item.STC01_1_QUL == "A6")
                                        {
                                            stc.rejectionReason = Status_item.STC12;
                                        }
                                        stc.isAccepted = false;
                                        TotalRejected++;
                                    }

                                    STCList.Add(stc);

                                }

                            }



                        }

                    }
                }
            }
            EDIReport obj = new EDIReport();
            obj.EDIReportId = EDIReportId;
            obj.TotalAccepted = TotalAccepted;
            obj.TotalRejected = TotalRejected;
            EDIReportList.Add(obj);
        }
        #endregion

        #region " Claim Submission Errors "


        public BLObject<string> DeleteClaimSubmissionError(string ErrorIds, string VisitIds)
        {
            try
            {
                ErrorIds = new DALClaim().DeleteClaimSubmissionError(ErrorIds, VisitIds);
                return new BLObject<string>(ErrorIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBilling::DeleteClaimSubmissionError", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        #endregion
    }
}
