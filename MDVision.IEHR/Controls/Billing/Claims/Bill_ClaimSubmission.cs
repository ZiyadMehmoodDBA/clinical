using MDVision.Business.BCommon;
using MDVision.Datasets;

using MDVision.IEHR.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using EDIParser;
using MDVision.IEHR.Model.Billing;
using MDVision.Business.ClaimScrubber;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.Billing.Claims
{
    public class Bill_ClaimSubmission
    {
        private BLLBillingClaim BLLBillingClaimObj = null;
        public Bill_ClaimSubmission()
        {
            BLLBillingClaimObj = new BLLBillingClaim();
        }

        #region Private Functions

        public string SearchPatientClaim(ClaimSubmissionModel model)
        {
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();


                DSVisits dsVisits = null;
                BLObject<DSVisits> obj;

                DateTime? dtfrom = String.IsNullOrEmpty(model.DOSFrom) ? (DateTime?)null : DateTime.Parse(model.DOSFrom);
                DateTime? dtto = String.IsNullOrEmpty(model.DOSTo) ? (DateTime?)null : DateTime.Parse(model.DOSTo);
                string ClaimNumber = !string.IsNullOrEmpty(model.ClaimNumber) ? MDVUtility.ToStr(model.ClaimNumber) : "";

                obj = BLLBillingClaimObj.LoadPatientClaim(model.PatientId, model.FirstName, model.LastName, MDVUtility.ToLong(model.ProviderId), MDVUtility.ToLong(model.ChargeBatchId), MDVUtility.ToLong(model.FacilityId), MDVUtility.ToLong(model.PracticeId), MDVUtility.ToLong(model.InsurancePlanId), dtfrom, dtto, MDVUtility.ToLong(model.ClearingHouse), MDVUtility.ToLong(model.BillerId), model.ClaimTypeId, model.BillerTypeId, MDVUtility.ToInt32(model.StatusId), model.ClaimErroredId, model.SubmissionMode, ClaimNumber, MDVUtility.ToInt32(model.PageNo), MDVUtility.ToInt32(model.rpp),MDVUtility.ToStr(model.SubmissionErrorId), MDVUtility.ToInt32(model.submitUserId));

                if (obj.Data != null)
                {
                    dsVisits = obj.Data;
                    if (dsVisits.Tables[dsVisits.PatientClaims.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ClaimCount = dsVisits.Tables[dsVisits.PatientClaims.TableName].Rows.Count,
                            iTotalDisplayRecords = dsVisits.PatientClaims.Rows[0][dsVisits.PatientClaims.RecordCountColumn.ColumnName],
                            ClaimLoad_JSON = MDVUtility.JSON_DataTable(dsVisits.Tables[dsVisits.PatientClaims.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClaimCount = 0,
                            Message = "Record not found.",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }



            }
            catch (Exception ex)
            {

                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string UpdatePrintClaim(string Visits, long ClearingHouseId, bool IsSubmit, bool MarkSubmitted, string UserBrowser)
        {

            try
            {

                BLObject<List<DSHCFA>> hcfa_ds = null;
                BLObject<byte[]> blank_obj = null;

                if (!string.IsNullOrEmpty(Visits))
                {

                    List<long> Visits_list = Visits.Split(',').ToList().Select(s => long.Parse(s)).ToList();

                    hcfa_ds = BLLBillingClaimObj.PrintClaim(Visits_list, ClearingHouseId, IsSubmit, MarkSubmitted);
                    if (hcfa_ds.Data != null)
                    {
                        List<DSHCFA> hcfa_ds_list = hcfa_ds.Data;
                        blank_obj = BLLBillingClaimObj.CreateClaimForm(hcfa_ds_list, UserBrowser, false);

                        if (blank_obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                PrintClaimForm = Convert.ToBase64String(blank_obj.Data),
                                Message = Common.AppPrivileges.Submitted_Success_Message,
                            };

                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                PrintClaimForm = "",
                                Message = Common.AppPrivileges.Submitted_Success_Message
                            };

                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else if (hcfa_ds.Message == "Success")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Mark_As_Submitted_Message,
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = hcfa_ds.Message,
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Please select patient visits.",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        public string PrintClaim(string Visits, long ClearingHouseId, bool IsSubmit, bool MarkSubmitted, string UserBrowser, bool ViewOnly)
        {

            try
            {

                BLObject<List<DSHCFA>> hcfa_ds = null;
                BLObject<byte[]> blank_obj = null;
                BLObject<byte[]> filled_obj = null;

                if (!string.IsNullOrEmpty(Visits))
                {

                    List<long> Visits_list = Visits.Split(',').ToList().Select(s => long.Parse(s)).ToList();

                    hcfa_ds = BLLBillingClaimObj.PrintClaim(Visits_list, ClearingHouseId, IsSubmit, MarkSubmitted, ViewOnly);
                    if (hcfa_ds.Data != null)
                    {
                        List<DSHCFA> hcfa_ds_list = hcfa_ds.Data;
                        blank_obj = BLLBillingClaimObj.CreateClaimForm(hcfa_ds_list, UserBrowser, false);
                        filled_obj = BLLBillingClaimObj.CreateClaimForm(hcfa_ds_list, UserBrowser, true);

                        string Message = "";
                        if (IsSubmit)
                            Message = AppPrivileges.Submitted_Success_Message;

                        if (blank_obj.Data != null && filled_obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                PrintClaimForm = Convert.ToBase64String(blank_obj.Data),
                                PreviewClaimForm = Convert.ToBase64String(filled_obj.Data),
                                Message = Message,
                            };

                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = blank_obj.Message + "" + filled_obj.Message,
                            };

                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else if (hcfa_ds.Message == "Success")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Mark_As_Submitted_Message,
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = hcfa_ds.Message,
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Please select patient visits.",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string TransmitClaim(string Visits, string ClearingHouseId, bool MarkSubmitted)
        {
            try
            {

                BLObject<BLLClaimSubmissionModel> obj;
                if (!string.IsNullOrEmpty(Visits))
                {
                    List<long> Visits_list = Visits.Split(',').ToList().Select(s => long.Parse(s)).ToList();
                    //execute in case of Db exception occure
                    obj = BLLBillingClaimObj.UploadClaim(Visits_list, MDVUtility.ToInt64(ClearingHouseId), MarkSubmitted);
                    if (obj.Data != null && obj.Data.isDBException == false)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Newtonsoft.Json.JsonConvert.SerializeObject(obj.Message),
                            ClaimsSubmissionJSON = Newtonsoft.Json.JsonConvert.SerializeObject(obj.Data),
                            ErroredClaims_Count = obj.Data.EroredClaims.Count,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    //execute in case of Db exception occure
                    else if (obj.Data.isDBException == true)
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data.Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "no visits selected.",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string SearchSubmittedBatch(ClaimSubmissionModel model)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DS837Batch dsBatches = null;
                BLObject<DS837Batch> obj;

                DateTime? dtSubmit = String.IsNullOrEmpty(model.SubmittedDate) ? (DateTime?)null : DateTime.Parse(model.SubmittedDate);

                obj = BLLBillingClaimObj.Load837BatchSearch(0, model.batchNumber, model.ClearingHouse, dtSubmit, MDVUtility.ToLong(model.SubmittedBy), (model.SubmissionMode), model.Completed, model.BatchStatus, MDVUtility.ToInt32(model.PageNo), MDVUtility.ToInt32(model.rpp));

                if (obj.Data != null)
                {
                    dsBatches = obj.Data;
                    if (dsBatches.Tables[dsBatches._837Batch.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SubmittedBatchCount = dsBatches.Tables[dsBatches._837Batch.TableName].Rows.Count,
                            iTotalDisplayRecords = dsBatches._837Batch.Rows[0][dsBatches._837Batch.RecordCountColumn.ColumnName],
                            SubmittedBatchLoad_JSON = MDVUtility.JSON_DataTable(dsBatches.Tables[dsBatches._837Batch.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClaimCount = 0,
                            Message = "Record not found.",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {

                    var response = new
                    {
                        status = false,
                        Message = obj.Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }


            }
            catch (Exception ex)
            {

                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string SearchClaimSubmitHistory(ClaimSubmissionModel model)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSVisits dsClaimHistory = null;
                BLObject<DSVisits> obj;

                obj = BLLBillingClaimObj.LoadClaimSubmitHistory(model.AccountNumber, MDVUtility.ToLong(model.ClearingHouse), model.batchNumber, model.ClaimNumber, model.SubmissionMode, model.SubmittedDate, MDVUtility.ToInt32(model.PageNo), MDVUtility.ToInt32(model.rpp));

                if (obj.Data != null)
                {
                    dsClaimHistory = obj.Data;
                    if (dsClaimHistory.Tables[dsClaimHistory.ClaimSubmitHistory.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ClaimSubmitHistoryCount = dsClaimHistory.Tables[dsClaimHistory.ClaimSubmitHistory.TableName].Rows.Count,
                            iTotalDisplayRecords = dsClaimHistory.ClaimSubmitHistory.Rows[0][dsClaimHistory.ClaimSubmitHistory.RecordCountColumn.ColumnName],
                            ClaimSubmitHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsClaimHistory.Tables[dsClaimHistory.ClaimSubmitHistory.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClaimCount = 0,
                            Message = "Record not found.",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {

                    var response = new
                    {
                        status = false,
                        Message = obj.Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }



            }
            catch (Exception ex)
            {

                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string ViewEDIFile(Int64 _837BatchId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DS837Batch dsClaim = null;
                BLObject<DS837Batch> obj;
                obj = BLLBillingClaimObj.Load837Batch(_837BatchId, "", "", null, 0, "", "", "");
                dsClaim = obj.Data;

                if (dsClaim.Tables[dsClaim._837Batch.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsClaim.Tables[dsClaim._837Batch.TableName].Rows[0];
                    var keyValues = new Dictionary<string, string>
                        {
                            { "txtTextView", MDVUtility.ToStr(dr[dsClaim._837Batch.EDI837StringColumn.ColumnName])}

                        };
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        EDI_FileDataJSON = js.Serialize(keyValues)
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string CreateEDIFile(string Visits, string ClearingHouseId)
        {
            try
            {
                BLObject<string> EDI_String = null;
                if (!string.IsNullOrEmpty(Visits))
                {

                    List<long> Visits_list = Visits.Split(',').ToList().Select(s => long.Parse(s)).ToList();

                    EDI_String = BLLBillingClaimObj.Genrate837EDIString(Visits_list, MDVUtility.ToInt64(ClearingHouseId));

                    if (EDI_String.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            EDI_JSON = EDI_String.Data,
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = EDI_String.Message,
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string ScrubClaims(string Visits)
        {
            try
            {
                if (!string.IsNullOrEmpty(Visits))
                {
                    List<long> Visits_list = Visits.Split(',').ToList().Select(s => long.Parse(s)).ToList();

                    BLObject<ClaimScrub> obj = null;
                    obj = BLLBillingClaimObj.ScrubClaims(Visits_list);

                    if (obj.Data != null)
                    {
                        string claim_word = "Claim";
                        if (Visits_list.Count > 1)
                            claim_word = "Claim(s)";

                        int totalerrors = MDVUtility.ToInt(obj.Data.ErrorReport.summary.totalerrors);

                        if (totalerrors > 0)
                        {
                            string error_word = "Error";
                            if (totalerrors > 1)
                                error_word = "Errors";

                            var response = new
                            {
                                status = true,
                                Message = totalerrors + " " + error_word + " found in " + obj.Data.Claims.Where(p => p.HasErrors == true).Count() + " " + claim_word + ".",
                                ErrorsCount = totalerrors,
                                ScrubClaim_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(obj.Data),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                Message = "No error found in " + obj.Data.ErrorReport.summary.totalclaimcount + " " + claim_word + ".",
                                ErrorsCount = totalerrors,
                                ScrubClaim_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(obj.Data),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string PrintClaimHistory(string VisitId, string PatientId)
        {

            try
            {
                if (!string.IsNullOrEmpty(VisitId))
                {
                    BLObject<string> ds = null;
                    ds = BLLBillingClaimObj.PrintClaimHistory(VisitId, PatientId);
                    if (ds.Data == "")
                    {

                        var response = new
                        {
                            status = true,
                            Message = "",
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ds.Data,
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Please select patient visits.",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string DeleteClaimSubmissionErrors(string Visits)
        {
            try
            {

                BLObject<string> obj;
                if (!string.IsNullOrEmpty(Visits))
                {
                    obj = BLLBillingClaimObj.DeleteClaimSubmissionError("", Visits);
                    if (obj.Data == "" )
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "no visits selected.",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Service Command Handler
        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
            }
        }



        #endregion
    }
}