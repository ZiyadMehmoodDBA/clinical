using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.EMR.Model.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using MDVision.Model.Lookups;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Model.Clinical.Medication;
using Newtonsoft.Json;
using MDVision.IEHR.EMR.Helpers.Clinical.ClinicalNotes;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Medical
{
    public class MedicationsHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public MedicationsHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static MedicationsHelper _instance = null;
        public static MedicationsHelper Instance()
        {
            if (_instance == null)
                _instance = new MedicationsHelper();
            return _instance;
        }

        public string LoadPrescriptionsForPrint(PrescriptionsModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientID)) && MDVUtility.ToInt64(model.PatientID) <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSClinicalMedication dsMedication = null;
                    BLObject<DSClinicalMedication> obj;
                    obj = BLLClinicalObj.LoadPrescriptionsForPrint(MDVUtility.ToInt64(model.PatientID), "1");
                    dsMedication = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            PrescriptionCount = dsMedication.Tables[dsMedication.Prescription.TableName].Rows.Count,
                            PrescriptionLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.Prescription.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PrescriptionCount = 0,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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
        public string loadPrescriptions(PrescriptionsModel model, bool isViewed = false)
        {
            try
            {
                DSClinicalMedication dsMedication = null;
                BLObject<DSClinicalMedication> obj;

                //obj = BLLClinicalObj.loadPrescriptions(MDVUtility.ToInt64(model.PrescriptionID), MDVUtility.ToInt64(model.PatientID), MDVUtility.ToInt64(model.NotesId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "1", "0");
                //Start 07-11-2016 Humaira Yousaf for db audit
                string isView = "";
                if (isViewed == true)
                {
                    isView = "1";
                }

                obj = BLLClinicalObj.loadPrescriptionsOp(MDVUtility.ToInt64(model.PatientID), MDVUtility.ToInt64(model.NotesId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), isView, "0");
                //End 07-11-2016 Humaira Yousaf for db audit
                dsMedication = obj.Data;
                if (obj.Data != null)
                {

                    var response = new
                    {
                        status = true,
                        PrescriptionCount = dsMedication.Tables[dsMedication.Prescription.TableName].Rows.Count,
                        PrescriptionLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.Prescription.TableName]),
                        iTotalDisplayRecords = (dsMedication.Prescription.Rows.Count > 0) ? dsMedication.Prescription.Rows[0][dsMedication.Prescription.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        PrescriptionCount = 0,
                        Message = obj.Message
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
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : function to load Medications.
        /// Date : 14 january 2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string loadMedications_Obsolete(MedicationModel model, bool isViewed = false)
        {
            try
            {
                DSClinicalMedication dsMedication = null;
                BLObject<DSClinicalMedication> obj;
                //Start 26-10-2016 Humaira Yousaf for logging of view action
                string isView = "";
                if (isViewed == true)
                {
                    isView = "1";
                }
                obj = BLLClinicalObj.loadMedicationsOp_Obsolete(MDVUtility.ToInt64(model.PatientID), MDVUtility.ToInt64(model.NoteId), Convert.ToBoolean(model.isCurrent), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "1", isView, "0", model.isFromCDS);
                //End 26-10-2016 Humaira Yousaf for logging of view action
                dsMedication = obj.Data;
                if (obj.Data != null)
                {
                    int medicationTotalCount = 0;

                    medicationTotalCount = dsMedication.Tables[dsMedication.Medication.TableName].Rows.Count;
                    if (medicationTotalCount > 0)
                    {
                        var response = new
                        {
                            status = true,
                            medicationTotalCount = medicationTotalCount,
                            MedicationCount = dsMedication.Tables[dsMedication.Medication.TableName].Rows.Count,
                            MedicationLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.Medication.TableName]),
                            MedicationHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationHistory.TableName]),
                            iTotalDisplayRecords = (dsMedication.Medication.Rows.Count > 0) ? dsMedication.Medication.Rows[0][dsMedication.Medication.RecordCountColumn.ColumnName] : 0,
                            MedicationReviewSoap_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationReview.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            medicationTotalCount = medicationTotalCount,
                            MedicationCount = 0,
                            MedicationLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.Medication.TableName]),
                            MedicationHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationHistory.TableName]),
                            iTotalDisplayRecords = 0,
                            MedicationReviewSoap_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationReview.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        MedicationCount = 0,
                        Message = obj.Message
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

        public string loadMedications(MedicationModel model, bool isViewed = false)
        {
            try
            {
                Tuple<List<ClinicalMedicationsModel>, List<ClinicalMedicationHistoryModel>, List<ClinicalMedicationReviewModel>> tupleMedication = null;
                BLObject<Tuple<List<ClinicalMedicationsModel>, List<ClinicalMedicationHistoryModel>, List<ClinicalMedicationReviewModel>>> obj;
                List<ClinicalMedicationsModel> clinicalMedicationsModelList;
                List<ClinicalMedicationHistoryModel> clinicalMedicationHistoryModelList;
                List<ClinicalMedicationReviewModel> clinicalMedicationReviewModelList;
                List<CustomModel> obRoutesLookUpModelList = new List<CustomModel>();
                List<CustomModel> obNegationReasonLookUpModelList = new List<CustomModel>();
                BLObject<List<CustomModel>> obRoutes;
                BLObject<List<CustomModel>> obNegationReason;

                string isView = "";
                if (isViewed == true)
                {
                    isView = "1";
                }
                obj = BLLClinicalObj.loadMedicationsOp(MDVUtility.ToInt64(model.PatientID), MDVUtility.ToInt64(model.NoteId), Convert.ToBoolean(model.isCurrent), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "1", isView, "0", model.isFromCDS);
                if (obj.Data != null)
                {
                    tupleMedication = obj.Data;
                    clinicalMedicationsModelList = tupleMedication.Item1;
                    clinicalMedicationHistoryModelList = tupleMedication.Item2;
                    clinicalMedicationReviewModelList = tupleMedication.Item3;
                    int medicationTotalCount = 0;

                    medicationTotalCount = clinicalMedicationsModelList.Count;
                    if (medicationTotalCount > 0)
                    {
                        obRoutes = BLLClinicalObj.LoadMedicationRoutesLookUp();
                        obRoutesLookUpModelList = obRoutes.Data;
                        obNegationReason = BLLClinicalObj.LookupNegationReason();
                        obNegationReasonLookUpModelList = obNegationReason.Data;

                        var response = new
                        {
                            status = true,
                            medicationTotalCount = medicationTotalCount,
                            MedicationCount = medicationTotalCount,
                            MedicationAntimicrobialRoute = (obRoutesLookUpModelList != null) ? JsonConvert.SerializeObject(obRoutesLookUpModelList) : JsonConvert.SerializeObject(obRoutesLookUpModelList),
                            MedicationLoad_JSON = JsonConvert.SerializeObject(clinicalMedicationsModelList),
                            MedicationHistoryLoad_JSON = JsonConvert.SerializeObject(clinicalMedicationHistoryModelList),
                            iTotalDisplayRecords = (clinicalMedicationsModelList.Count > 0) ? MDVUtility.ToInt(clinicalMedicationsModelList[0].RecordCount) : 0,
                            MedicationReviewSoap_JSON = JsonConvert.SerializeObject(clinicalMedicationReviewModelList),
                            MedicationNegationReason = (obNegationReasonLookUpModelList != null) ? JsonConvert.SerializeObject(obNegationReasonLookUpModelList) : JsonConvert.SerializeObject(obNegationReasonLookUpModelList)
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            medicationTotalCount = medicationTotalCount,
                            MedicationCount = 0,
                            MedicationLoad_JSON = clinicalMedicationsModelList,// MDVUtility.JSON_DataTable(tupleMedication.Tables[tupleMedication.Medication.TableName]),
                            MedicationAntimicrobialRoute = (obRoutesLookUpModelList != null) ? JsonConvert.SerializeObject(obRoutesLookUpModelList) : JsonConvert.SerializeObject(obRoutesLookUpModelList),
                            MedicationHistoryLoad_JSON = clinicalMedicationHistoryModelList,// MDVUtility.JSON_DataTable(tupleMedication.Tables[tupleMedication.MedicationHistory.TableName]),
                            iTotalDisplayRecords = 0,
                            MedicationReviewSoap_JSON = JsonConvert.SerializeObject(clinicalMedicationReviewModelList),// MDVUtility.JSON_DataTable(tupleMedication.Tables[tupleMedication.MedicationReview.TableName]),
                            MedicationNegationReason = (obNegationReasonLookUpModelList != null) ? JsonConvert.SerializeObject(obNegationReasonLookUpModelList) : JsonConvert.SerializeObject(obNegationReasonLookUpModelList)
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        MedicationCount = 0,
                        Message = obj.Message
                    };
                    return JsonConvert.SerializeObject(response);
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string UpdateNegationReasonIdByMedicationId(MedicationModel model)
        {
            try
            {
                string dsMedication = null;
                BLObject<string> obj;

                obj = BLLClinicalObj.UpdateNegationReasonIdByMedicationId(MDVUtility.ToStr(model.MedicationID), MDVUtility.ToStr(model.NegationReason));
                dsMedication = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Update_Error_Message
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
        public string UpdateRouteIdByMedicationId(MedicationModel model)
        {
            try
            {
                string dsMedication = null;
                BLObject<string> obj;

                obj = BLLClinicalObj.UpdateRouteIdByMedicationId(MDVUtility.ToStr(model.MedicationID), MDVUtility.ToStr(model.RouteId));
                dsMedication = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Update_Error_Message
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
        public string loadMedicationsReviewd(MedicationModel model)
        {
            try
            {
                DSClinicalMedication dsMedication = null;
                BLObject<DSClinicalMedication> obj;
                obj = BLLClinicalObj.loadMedicationsReviewd(MDVUtility.ToInt64(model.PatientID));
                dsMedication = obj.Data;
                if (obj.Data != null)
                {
                    int ReviewedTotalCount = 0;
                    ReviewedTotalCount = dsMedication.Tables[dsMedication.MedicationReview.TableName].Rows.Count;
                    if (ReviewedTotalCount > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ReviewedTotalCount = ReviewedTotalCount,
                            MedicationReviewSoap_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationReview.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ReviewedTotalCount = ReviewedTotalCount,
                            MedicationReviewSoap_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationReview.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ReviewedTotalCount = 0,
                        Message = obj.Message
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

        public string loadAllMedications(MedicationModel model, bool isViewed = false)
        {
            try
            {
                DSClinicalMedication dsMedication = null;
                BLObject<DSClinicalMedication> obj;
                string isView = "";
                if (isViewed == true)
                {
                    isView = "1";
                }
                obj = BLLClinicalObj.loadAllMedicationsOp(MDVUtility.ToInt64(model.PatientID), MDVUtility.ToInt64(model.NoteId), Convert.ToBoolean(model.isCurrent), "1", isView, "0", model.isFromCDS);
                dsMedication = obj.Data;
                if (obj.Data != null)
                {
                    int medicationTotalCount = 0;
                    medicationTotalCount = dsMedication.Tables[dsMedication.Medication.TableName].Rows.Count;
                    if (medicationTotalCount > 0)
                    {
                        var response = new
                        {
                            status = true,
                            medicationTotalCount = medicationTotalCount,
                            MedicationCount = dsMedication.Tables[dsMedication.Medication.TableName].Rows.Count,
                            MedicationLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.Medication.TableName]),
                            MedicationHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationHistory.TableName]),
                            iTotalDisplayRecords = (dsMedication.Medication.Rows.Count > 0) ? dsMedication.Medication.Rows[0][dsMedication.Medication.RecordCountColumn.ColumnName] : 0,
                            MedicationReviewSoap_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationReview.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            medicationTotalCount = medicationTotalCount,
                            MedicationCount = 0,
                            MedicationLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.Medication.TableName]),
                            MedicationHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationHistory.TableName]),
                            iTotalDisplayRecords = 0,
                            MedicationReviewSoap_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationReview.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        MedicationCount = 0,
                        Message = obj.Message
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

        public string loadMedicationsForCDS(string MedLookupName)
        {
            try
            {
                DSClinicalMedication dsMedication = null;
                BLObject<DSClinicalMedication> obj;
                obj = BLLClinicalObj.loadMedicationsForCDS(MedLookupName);

                dsMedication = obj.Data;
                if (obj.Data != null)
                {
                    int medicationTotalCount = 0;
                    medicationTotalCount = dsMedication.Tables[dsMedication.MedicationCDS.TableName].Rows.Count;
                    if (medicationTotalCount > 0)
                    {
                        var response = new
                        {
                            status = true,
                            // medicationTotalCount = medicationTotalCount,
                            // MedicationCount = dsMedication.Tables[dsMedication.Medication.TableName].Rows.Count,
                            MedicationLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationCDS.TableName]),
                            // MedicationHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationHistory.TableName]),
                            // iTotalDisplayRecords = (dsMedication.Medication.Rows.Count > 0) ? dsMedication.Medication.Rows[0][dsMedication.Medication.RecordCountColumn.ColumnName] : 0,
                            // MedicationReviewSoap_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationReview.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            // medicationTotalCount = medicationTotalCount,
                            // MedicationCount = 0,
                            MedicationLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationCDS.TableName]),
                            //  MedicationHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationHistory.TableName]),
                            //  iTotalDisplayRecords = 0,
                            //  MedicationReviewSoap_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.MedicationReview.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        // MedicationCount = 0,
                        Message = obj.Message
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

        public string SaveReviews(List<ReviewedModel> model, SharedVariable sharedVariable = null)
        {
            try
            {




                DSClinicalMedication dsmedication = new DSClinicalMedication();
                DSAllergies dsAllergy = new DSAllergies();
                for (int i = 0; i < model.Count; i++)
                {

                    if (model[i].WhichReviewed == "Medication")
                    {
                        #region Medication DataSet Intialization
                        DSClinicalMedication.MedicationReviewRow dr = dsmedication.MedicationReview.NewMedicationReviewRow();

                        dr.MedicationReviewID = -i;
                        dr.PatientId = model[i].PatientId;
                        dr.ReviewedBy = model[i].ReviewedBy;
                        if (model[i].ReviewedOn != "")
                        {
                            //Begin 4/26/16  Edit By M Ahmad Imran Bug # EMR-281
                            model[i].ReviewedOn = model[i].ReviewedOn.Replace(" EDT", "");
                            //End 4/26/16  Edit By M Ahmad Imran Bug # EMR-281
                            dr.ReviewedOn = MDVUtility.ToDateTime(model[i].ReviewedOn);
                        }
                        dsmedication.MedicationReview.AddMedicationReviewRow(dr);
                        #endregion
                    }


                    else
                    {
                        #region Allergy DataSet Intialization
                        DSAllergies.AllergyReviewRow dr = dsAllergy.AllergyReview.NewAllergyReviewRow();

                        dr.AllergyReviewID = -i;
                        dr.PatientId = model[i].PatientId;
                        dr.ReviewedBy = model[i].ReviewedBy;
                        if (model[i].ReviewedOn != "")
                        {
                            //Begin 4/26/16  Edit By M Ahmad Imran Bug # EMR-281
                            model[i].ReviewedOn = model[i].ReviewedOn.Replace(" EDT", "");
                            //End 4/26/16  Edit By M Ahmad Imran Bug # EMR-281
                            dr.ReviewedOn = MDVUtility.ToDateTime(model[i].ReviewedOn);
                        }
                        dsAllergy.AllergyReview.AddAllergyReviewRow(dr);
                        #endregion
                    }


                }



                #region Database Insertion
                BLObject<DSClinicalMedication> obj = new BLObject<DSClinicalMedication>();
                BLObject<DSAllergies> obj1 = new BLObject<DSAllergies>();
                if (dsmedication.MedicationReview.Count > 0)
                {
                    obj = BLLClinicalObj.InsertMedicationReviews(dsmedication, sharedVariable);
                }

                if (dsAllergy.AllergyReview.Count > 0)
                {
                    obj1 = BLLClinicalObj.InsertAllergyReviews(dsAllergy, sharedVariable);
                }

                long MedicationReviewID = 0;
                long AllergyReviewID = 0;
                if (dsmedication.MedicationReview.Count > 0)
                {
                    if (obj.Data != null)
                    {
                        dsmedication = obj.Data;

                        MedicationReviewID = MDVUtility.ToLong(dsmedication.Tables[dsmedication.MedicationReview.TableName].Rows[0][dsmedication.MedicationReview.MedicationReviewIDColumn.ColumnName]);

                    }

                    else
                    {

                        var responseRcopiaerror = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                    }
                }
                if (dsAllergy.AllergyReview.Count > 0)
                {
                    if (obj1.Data != null)
                    {

                        dsAllergy = obj1.Data;
                        //Begin 28/4/1016  Edit By M Ahmad Imran Bug # EMR-274
                        AllergyReviewID = MDVUtility.ToLong(dsAllergy.Tables[dsAllergy.AllergyReview.TableName].Rows[0][dsAllergy.AllergyReview.AllergyReviewIDColumn.ColumnName]);
                        //End 28/4/1016  Edit By M Ahmad Imran Bug # EMR-274
                    }

                    else
                    {

                        var responseRcopiaerror = new
                        {
                            status = false,
                            Message = obj1.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                    }
                }
                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    MedicationReviewID = MedicationReviewID,
                    AllergyReviewID = AllergyReviewID
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                #endregion



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
        /// Author : M Ahmad Imran
        /// Purpose : function to Save Complaints.
        /// Date : 11 Feb 2016
        public string SaveComplaint(ComplaintModel model)
        {
            try
            {
                DSClinicalComplaint dsComplaint = new DSClinicalComplaint();
                DSClinicalComplaint.ComplaintRow dr = dsComplaint.Complaint.NewComplaintRow();
                dr.ComplaintId = MDVUtility.ToLong(model.ComplaintId);
                if (!string.IsNullOrEmpty(model.DateCaptured))
                {
                    dr.DateCaptured = MDVUtility.ToDateTime(model.DateCaptured);
                }
                else
                {
                    dr[dsComplaint.Complaint.DateCapturedColumn] = DBNull.Value;
                }
                dr.OverallComments = model.OverallComments;
                dr.PatientId = MDVUtility.ToLong(model.PatientId);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.NotesId = MDVUtility.ToLong(model.NotesId);
                dsComplaint.Complaint.AddComplaintRow(dr);

                DSClinicalComplaint.NotesComplaintRow NotesComplaint = dsComplaint.NotesComplaint.NewNotesComplaintRow();
                NotesComplaint.NotesComplaintId = -1;
                NotesComplaint.NotesId = MDVUtility.ToLong(model.NotesId);
                dsComplaint.NotesComplaint.AddNotesComplaintRow(NotesComplaint);
                for (int i = 0; i < model.ComplaintDetails.Count; i++)
                {
                    DSClinicalComplaint.ComplaintDetailRow complaintDetail = dsComplaint.ComplaintDetail.NewComplaintDetailRow();
                    complaintDetail.AssociatedWith = model.ComplaintDetails[i].AssociatedWith;
                    complaintDetail.Comments = model.ComplaintDetails[i].Comments;
                    complaintDetail.Complaint_AggravatedById = MDVUtility.ToInt(model.ComplaintDetails[i].Complaint_AggravatedById);
                    complaintDetail.Complaint_CaseId = MDVUtility.ToInt(model.ComplaintDetails[i].Complaint_CaseId);
                    complaintDetail.Complaint_CharacterIds = model.ComplaintDetails[i].Complaint_CharacterIds;
                    complaintDetail.Complaint_ContextId = MDVUtility.ToInt(model.ComplaintDetails[i].Complaint_ContextId);
                    complaintDetail.Complaint_DurationId = MDVUtility.ToInt(model.ComplaintDetails[i].Complaint_DurationId);
                    complaintDetail.Complaint_FrequencyId = MDVUtility.ToInt(model.ComplaintDetails[i].Complaint_FrequencyId);
                    complaintDetail.Complaint_LocationIds = model.ComplaintDetails[i].Complaint_LocationIds;
                    complaintDetail.Complaint_QualityId = MDVUtility.ToInt(model.ComplaintDetails[i].Complaint_QualityId);
                    complaintDetail.Complaint_RadiationId = MDVUtility.ToInt(model.ComplaintDetails[i].Complaint_RadiationId);
                    complaintDetail.Complaint_RelievedById = MDVUtility.ToInt(model.ComplaintDetails[i].Complaint_RelievedById);
                    complaintDetail.Complaint_SeverityId = MDVUtility.ToInt(model.ComplaintDetails[i].Complaint_SeverityId);
                    complaintDetail.ComplaintDescription = model.ComplaintDetails[i].ComplaintDescription;
                    complaintDetail.ComplaintDetailId = MDVUtility.ToLong(model.ComplaintDetails[i].ComplaintDetailId) < 0 ? 0 : MDVUtility.ToLong(model.ComplaintDetails[i].ComplaintDetailId);
                    complaintDetail.Duration = model.ComplaintDetails[i].Duration;
                    complaintDetail.Onset = model.ComplaintDetails[i].Onset;
                    complaintDetail.PrecipitatedBy = model.ComplaintDetails[i].PrecipitatedBy;
                    complaintDetail.PreviousHistory = model.ComplaintDetails[i].PreviousHistory;
                    complaintDetail.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    complaintDetail.CreatedOn = DateTime.Now;
                    complaintDetail.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    complaintDetail.ModifiedOn = DateTime.Now;
                    complaintDetail.IsChiefComplaint = model.ComplaintDetails[i].IsChiefComplaint != "False" ? true : false;
                    complaintDetail.ICD9 = model.ComplaintDetails[i].ICD9;
                    complaintDetail.ICD9Description = model.ComplaintDetails[i].ICD9Description;
                    complaintDetail.ICD10 = model.ComplaintDetails[i].ICD10;
                    complaintDetail.ICD10Description = model.ComplaintDetails[i].ICD10Description;
                    complaintDetail.SNOMED = model.ComplaintDetails[i].SNOMED;
                    complaintDetail.SNOMEDDescription = model.ComplaintDetails[i].SNOMEDDescription;
                    complaintDetail.IsReported = model.ComplaintDetails[i].IsReported != "False" ? true : false;
                    dsComplaint.ComplaintDetail.AddComplaintDetailRow(complaintDetail);
                }

                #region Database Insertion
                BLObject<DSClinicalComplaint> obj = BLLClinicalObj.InsertComplaint(dsComplaint);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ComplaintId = ((DSClinicalComplaint.ComplaintRow)obj.Data.Complaint.Rows[0]).ComplaintId,
                        ComplaintDetail_JSON = MDVUtility.JSON_DataTable(obj.Data.ComplaintDetail),
                        Message = Common.AppPrivileges.Save_Message,
                        //MedicationId = dsmedication.Tables[dsmedication.Medication.TableName].Rows[0][dsmedication.Medication.MedicationIDColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var responseRcopiaerror = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                }



                #endregion



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



        public string UpdateComplaintFromNotes(ComplaintModel model)
        {
            try
            {
                DSClinicalComplaint dsComplaint = new DSClinicalComplaint();
                DSClinicalComplaint.ComplaintRow dr = dsComplaint.Complaint.NewComplaintRow();
                dr.ComplaintId = MDVUtility.ToLong(model.ComplaintId);
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dsComplaint.Complaint.AddComplaintRow(dr);
                DSClinicalComplaint.NotesComplaintRow NotesComplaint = dsComplaint.NotesComplaint.NewNotesComplaintRow();
                dsComplaint.NotesComplaint.AddNotesComplaintRow(NotesComplaint);
                for (int i = 0; i < model.ComplaintDetails.Count; i++)
                {
                    DSClinicalComplaint.ComplaintDetailRow complaintDetail = dsComplaint.ComplaintDetail.NewComplaintDetailRow();
                    complaintDetail.ComplaintDescription = model.ComplaintDetails[i].ComplaintDescription;
                    complaintDetail.ComplaintDetailId = MDVUtility.ToLong(model.ComplaintDetails[i].ComplaintDetailId) < 0 ? 0 : MDVUtility.ToLong(model.ComplaintDetails[i].ComplaintDetailId);
                    complaintDetail.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    complaintDetail.ModifiedOn = DateTime.Now;
                    dsComplaint.ComplaintDetail.AddComplaintDetailRow(complaintDetail);
                }

                #region Database Insertion
                BLObject<DSClinicalComplaint> obj = BLLClinicalObj.UpdateComplaintFromNotes(dsComplaint);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        //MedicationId = dsmedication.Tables[dsmedication.Medication.TableName].Rows[0][dsmedication.Medication.MedicationIDColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var responseRcopiaerror = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                }



                #endregion



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


        /// Author : M Ahmad Imran
        /// Purpose : function to Load Complaints.
        /// Date : 11 Feb 2016
        public string DeleteComplaint(ComplaintModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.ComplaintDetails[0].ComplaintDetailId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.DeleteCompliantDetail(MDVUtility.ToLong(model.ComplaintDetails[0].ComplaintDetailId), MDVUtility.ToLong(model.NotesId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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
        /// Author : M Ahmad Imran
        /// Purpose : function to Reset Complaints.
        /// Date : 16 Feb 2016
        public string ResetComplaint(ComplaintModel model)
        {
            try
            {

                BLObject<string> obj = BLLClinicalObj.ResetComplaint(MDVUtility.ToLong(model.ComplaintId), MDVUtility.ToLong(model.NotesId));


                if (obj.Data == "")
                {

                    var response = new
                    {
                        status = true,
                        Message = "Reset Successfully"

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Data
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


        public string load_SearchAllComplaints(ComplaintModel model)
        {
            try
            {

                DSClinicalComplaint dsComplaintList = null;
                BLObject<DSClinicalComplaint> obj;

                obj = BLLClinicalObj.LoadAllComplaintsForFaceSheet(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "1", "0");
                dsComplaintList = obj.Data;
                if (obj.Data != null)
                {
                    int ComplaintTotalCount = 0;
                    if (dsComplaintList.Tables[dsComplaintList.FaceSheetComplaints.TableName].Rows.Count > 0)
                    {

                        var response = new
                        {
                            status = true,
                            ComplaintListTotalCount = ComplaintTotalCount,
                            ComplaintListCount = dsComplaintList.Tables[dsComplaintList.FaceSheetComplaints.TableName].Rows.Count,
                            ComplaintListLoad_JSON = MDVUtility.JSON_DataTable(dsComplaintList.Tables[dsComplaintList.FaceSheetComplaints.TableName]),
                            iTotalDisplayRecords = (dsComplaintList.FaceSheetComplaints.Rows.Count > 0) ? dsComplaintList.FaceSheetComplaints.Rows[0][dsComplaintList.FaceSheetComplaints.RecordCountColumn.ColumnName] : 0,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ComplaintListCount = 0,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ComplaintListCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// Author : M Ahmad Imran
        /// Purpose : function to Load Complaints.
        /// Date : 11 Feb 2016
        public string LoadComplaint(ComplaintModel model)
        {
            try
            {
                DSClinicalComplaint dsComplaint = null;
                BLObject<DSClinicalComplaint> obj;

                obj = BLLClinicalObj.GetComplaintInfo(MDVUtility.ToLong(model.NotesId));

                dsComplaint = obj.Data;
                if (obj.Data != null)
                {
                    int ComplaintTotalCount = 0;
                    int ComplaintDetailTotalCount = 0;
                    string CapturedDateDate = "";
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    ComplaintTotalCount = dsComplaint.Tables[dsComplaint.Complaint.TableName].Rows.Count;
                    ComplaintDetailTotalCount = dsComplaint.Tables[dsComplaint.ComplaintDetail.TableName].Rows.Count;

                    List<ComplaintDetailModel> ComplainteDetailLoad_J = new List<ComplaintDetailModel>();
                    List<ComplaintModel> ComplainteLoad_J = new List<ComplaintModel>();

                    if (ComplaintTotalCount > 0)
                    {
                        CapturedDateDate = MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dsComplaint.Complaint.Rows[0]["DateCaptured"]));
                        foreach (DSClinicalComplaint.ComplaintRow row in dsComplaint.Tables[dsComplaint.Complaint.TableName].Rows)
                        {
                            ComplaintModel ComplaintModel = new ComplaintModel();
                            ComplaintModel.ComplaintId = MDVUtility.ToStr(row[dsComplaint.Complaint.ComplaintIdColumn]);
                            ComplaintModel.PatientId = MDVUtility.ToStr(row[dsComplaint.Complaint.PatientIdColumn]);
                            ComplaintModel.DateCaptured = MDVUtility.ToStr(row[dsComplaint.Complaint.DateCapturedColumn]);
                            ComplaintModel.OverallComments = MDVUtility.ToStr(row[dsComplaint.Complaint.OverallCommentsColumn]);
                            ComplainteLoad_J.Add(ComplaintModel);
                        }
                    }
                    if (ComplaintDetailTotalCount > 0)
                    {

                        foreach (DSClinicalComplaint.ComplaintDetailRow row in dsComplaint.Tables[dsComplaint.ComplaintDetail.TableName].Rows)
                        {
                            ComplaintDetailModel ComplaintModel = new ComplaintDetailModel();
                            //Add Parent Records
                            ComplaintModel.ComplaintDetailId = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.ComplaintDetailIdColumn]);
                            ComplaintModel.ComplaintId = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.ComplaintIdColumn]);
                            ComplaintModel.ComplaintDescription = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.ComplaintDescriptionColumn]);

                            ComplaintModel.PreviousHistory = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.PreviousHistoryColumn]);
                            ComplaintModel.IsChiefComplaint = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.IsChiefComplaintColumn]);
                            ComplaintModel.Complaint_CaseId = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.Complaint_CaseIdColumn]);
                            ComplaintModel.Complaint_LocationIds = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.Complaint_LocationIdsColumn]);
                            ComplaintModel.Complaint_RadiationId = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.Complaint_RadiationIdColumn]);

                            ComplaintModel.Complaint_QualityId = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.Complaint_QualityIdColumn]);
                            ComplaintModel.Complaint_SeverityId = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.Complaint_SeverityIdColumn]);
                            ComplaintModel.Onset = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.OnsetColumn]);

                            ComplaintModel.Complaint_DurationId = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.Complaint_DurationIdColumn]);
                            ComplaintModel.Duration = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.DurationColumn]);
                            ComplaintModel.Complaint_FrequencyId = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.Complaint_FrequencyIdColumn]);
                            ComplaintModel.Complaint_ContextId = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.Complaint_ContextIdColumn]);

                            ComplaintModel.Complaint_CharacterIds = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.Complaint_CharacterIdsColumn]);
                            ComplaintModel.AssociatedWith = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.AssociatedWithColumn]);
                            ComplaintModel.PrecipitatedBy = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.PrecipitatedByColumn]);

                            ComplaintModel.Complaint_AggravatedById = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.Complaint_AggravatedByIdColumn]);
                            ComplaintModel.Complaint_RelievedById = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.Complaint_RelievedByIdColumn]);

                            ComplaintModel.Comments = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.CommentsColumn]);

                            ComplaintModel.ICD9 = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.ICD9Column]);
                            ComplaintModel.ICD10 = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.ICD10Column]);
                            ComplaintModel.SNOMED = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.SNOMEDColumn]);
                            ComplaintModel.ICD9Description = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.ICD9DescriptionColumn]);
                            ComplaintModel.ICD10Description = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.ICD10DescriptionColumn]);
                            ComplaintModel.SNOMEDDescription = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.SNOMEDDescriptionColumn]);

                            ComplaintModel.ProblemListId = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.ProblemListIdColumn]);
                            ComplaintModel.IsReported = MDVUtility.ToStr(row[dsComplaint.ComplaintDetail.IsReportedColumn]);
                            ComplainteDetailLoad_J.Add(ComplaintModel);
                        }



                    }
                    var response = new
                    {
                        status = true,
                        CapturedDateDate,
                        ComplaintTotalCount = ComplaintTotalCount,
                        ComplaintDetailTotalCount = ComplaintDetailTotalCount,
                        ComplainteLoad_JSON = js.Serialize(ComplainteLoad_J),
                        ComplainteDetailLoad_JSON = js.Serialize(ComplainteDetailLoad_J),

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ComplaintTotalCount = 0,
                        ComplaintDetailTotalCount = 0,
                        Message = obj.Message
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
        public string SaveMedication(List<MedicationModel> model, bool IsRefill = false, SharedVariable sharedVariable = null, string UserName = null, string OrderSetId = "")
        {
            try
            {
                if (UserName == null)
                {
                    UserName = MDVSession.Current.AppUserName;
                }
                DSClinicalMedication dsmedication = new DSClinicalMedication();
                for (int i = 0; i < model.Count; i++)
                {
                    DSClinicalMedication.MedicationRow dr = dsmedication.Medication.NewMedicationRow();

                    dr.MedicationID = -i;
                    dr.IsDeleted = false;
                    if (model[i].IsDeleted.Equals("y"))
                    {
                        dr.IsDeleted = true;
                    }

                    dr.RcopiaID = model[i].RcopiaID;
                    dr.PatientID = model[i].PatientID;
                    if (model[i].PrescriptionRcopiaID != "")
                    {
                        dr.PrescriptionRcopiaID = model[i].PrescriptionRcopiaID;
                    }

                    dr.ProviderID = model[i].ProviderID;
                    dr.NPI = model[i].ProviderNPI;
                    dr.Preparer_UserID = model[i].Preparer_UserID;
                    dr.DrugID = model[i].DrugID;
                    dr.Action = model[i].Action;
                    dr.Dose = model[i].Dose;
                    dr.DoseUnit = model[i].DoseUnit;
                    dr.Routeby = model[i].Routeby;
                    dr.DoseTiming = model[i].DoseTiming;
                    dr.DoseOther = model[i].DoseOther;
                    dr.Duration = model[i].Duration;
                    dr.Quantity = model[i].Quantity;
                    dr.QuantityUnit = model[i].QuantityUnit;
                    dr.Refill = model[i].Refill;
                    dr.Substitution = model[i].Substitution;
                    dr.OtherNote = model[i].OtherNote;
                    dr.PatientNotes = model[i].PatientNotes;
                    dr.Comments = model[i].Comments;
                    if (OrderSetId != "")
                    {
                        dr.OrderSetId = MDVUtility.ToInt64(OrderSetId);
                    }
                    else
                    {
                        dr[dsmedication.Medication.OrderSetIdColumn] = DBNull.Value;
                    }
                    if (model[i].StartDate.Year > 0001)
                    {
                        dr.StartDate = model[i].StartDate;
                    }


                    if (model[i].StopDate.Year > 0001)
                    {
                        dr.StopDate = model[i].StopDate;
                    }
                    dr.StopReason = model[i].StopReason;

                    if (model[i].SigChangedDate.Year > 0001)
                    {
                        dr.SigChangedDate = model[i].SigChangedDate;
                    }
                    if (model[i].FillDate.Year > 0001)
                    {
                        dr.FillDate = model[i].FillDate;
                    }

                    dr.LastModifiedBy = MDVUtility.ToStr(model[i].LastModifiedBy);


                    if (model[i].LastModifiedDate.Year > 0001)
                    {
                        dr.LastModifiedDate = MDVUtility.ToDateTime(model[i].LastModifiedDate);
                    }
                    dr.IntendedUse = model[i].IntendedUse;
                    dr.Number = model[i].Number;
                    dr.Status = model[i].Status;
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(UserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(UserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.DrugDescription = model[i].drugModel.BrandName + '(' + model[i].drugModel.GenericName + ')';
                    dr.RcopiaUserName = model[i].RcopiaUserName;
                    DSClinicalMedication.DrugRow drug = dsmedication.Drug.NewDrugRow();
                    drug.BrandName = model[i].drugModel.BrandName;
                    drug.BrandType = model[i].drugModel.BrandType;
                    drug.FirstDataBankMedID = model[i].drugModel.FirstDataBankMedID;
                    drug.Form = model[i].drugModel.Form;
                    drug.GenericName = model[i].drugModel.GenericName;
                    drug.NDCID = model[i].drugModel.NDCID;
                    drug.RcopiaID = model[i].drugModel.RcopiaID;
                    drug.Route = model[i].drugModel.Route;
                    drug.RxnormID = model[i].drugModel.RxnormID;

                    drug.RxnormIDType = model[i].drugModel.RxnormIDType;
                    drug.Schedule = model[i].drugModel.Schedule;
                    drug.Strength = model[i].drugModel.Strength;
                    drug.IsActive = true;
                    drug.CreatedBy = MDVUtility.DecryptFrom64(UserName);
                    drug.CreatedOn = DateTime.Now;
                    drug.ModifiedBy = MDVUtility.DecryptFrom64(UserName);
                    drug.ModifiedOn = DateTime.Now;
                    drug.MedicationRcopiaId = model[i].RcopiaID;
                    dsmedication.Medication.AddMedicationRow(dr);
                    dsmedication.Drug.AddDrugRow(drug);
                }
                #region Database Insertion
                BLObject<DSClinicalMedication> obj = BLLClinicalObj.insertMedication(dsmedication, sharedVariable);
                dsmedication = obj.Data;
                if (obj.Data != null)
                {


                    string SavedMedicationIds = "";
                    int i = 1;
                    foreach (DSClinicalMedication.MedicationRow row in dsmedication.Tables[dsmedication.Medication.TableName].Rows)
                    {
                        if (i == 1)
                        {
                            SavedMedicationIds = MDVUtility.ToStr(row[dsmedication.Medication.MedicationIDColumn]);
                        }
                        else
                        {
                            SavedMedicationIds = SavedMedicationIds + "," + MDVUtility.ToStr(row[dsmedication.Medication.MedicationIDColumn]);
                        }
                        i++;
                    }

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        SavedMedicationIds = SavedMedicationIds,
                        MedicationId = dsmedication.Tables[dsmedication.Medication.TableName].Rows[0][dsmedication.Medication.MedicationIDColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var responseRcopiaerror = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                }

                #endregion


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

        public string SavePrescription(List<PrescriptionsModel> model, bool IsRefill = false, SharedVariable sharedVariable = null, string UserName = null)
        {
            try
            {
                if (UserName == null)
                {
                    UserName = MDVSession.Current.AppUserName;
                }
                DSClinicalMedication dsmedication = new DSClinicalMedication();
                int a = 1;
                for (int i = 0; i < model.Count; i++)
                {
                    if (model[i].PharamacyModel != null)
                    {

                        DSClinicalMedication.PharmacyRow Pharmacy = dsmedication.Pharmacy.NewPharmacyRow();

                        Pharmacy.PharmacyId = -a;
                        a++;
                        Pharmacy.RcopiaID = model[i].PharamacyModel.RcopiaID;
                        Pharmacy.RcopiaMasterID = model[i].PharamacyModel.RcopiaMasterID;
                        Pharmacy.NCPDPID = model[i].PharamacyModel.NCPDPID;
                        //Pharmacy.NPI = model[i].PharamacyModel.NPI;//implement in future
                        Pharmacy.PharmacyName = model[i].PharamacyModel.PharmacyName;
                        Pharmacy.Address = model[i].PharamacyModel.Address;
                        Pharmacy.City = model[i].PharamacyModel.City;
                        Pharmacy.State = model[i].PharamacyModel.State;
                        Pharmacy.Zip = model[i].PharamacyModel.Zip;
                        Pharmacy.Phone = model[i].PharamacyModel.Phone;
                        Pharmacy.Fax = model[i].PharamacyModel.Fax;
                        Pharmacy.Is24Hour = model[i].PharamacyModel.Is24Hour;
                        Pharmacy.Level3 = model[i].PharamacyModel.Level3;
                        Pharmacy.Electronic = model[i].PharamacyModel.Electronic;
                        Pharmacy.MailOrder = model[i].PharamacyModel.MailOrder;


                        Pharmacy.Retail = model[i].PharamacyModel.Retail;
                        Pharmacy.LongTermCare = model[i].PharamacyModel.LongTermCare;
                        Pharmacy.Specialty = model[i].PharamacyModel.Specialty;
                        Pharmacy.CanReceiveControlledSubstance = model[i].PharamacyModel.CanReceiveControlledSubstance;
                        Pharmacy.IsActive = true;
                        Pharmacy.CreatedBy = MDVUtility.DecryptFrom64(UserName);
                        Pharmacy.CreatedOn = DateTime.Now;
                        Pharmacy.ModifiedBy = MDVUtility.DecryptFrom64(UserName);
                        Pharmacy.ModifiedOn = DateTime.Now;

                        dsmedication.Pharmacy.AddPharmacyRow(Pharmacy);
                    }
                    DSClinicalMedication.MedicationRow dr = dsmedication.Medication.NewMedicationRow();

                    dr.MedicationID = -i;
                    dr.PrescriptionRcopiaID = model[i].RcopiaID;
                    //dr.RcopiaID = model[i].RcopiaID;
                    dr.PatientID = model[i].PatientID;
                    //dr.PrescriptionID = model[i].PrescriptionID;
                    dr.ProviderID = model[i].MedicationModel.ProviderID;
                    dr.Preparer_UserID = model[i].MedicationModel.Preparer_UserID;
                    dr.DrugID = model[i].MedicationModel.DrugID;
                    dr.Action = model[i].MedicationModel.Action;
                    dr.Dose = model[i].MedicationModel.Dose;
                    dr.DoseUnit = model[i].MedicationModel.DoseUnit;
                    dr.Routeby = model[i].MedicationModel.Routeby;
                    dr.DoseTiming = model[i].MedicationModel.DoseTiming;
                    dr.DoseOther = model[i].MedicationModel.DoseOther;
                    dr.Duration = model[i].MedicationModel.Duration;
                    dr.Quantity = model[i].MedicationModel.Quantity;
                    dr.QuantityUnit = model[i].MedicationModel.QuantityUnit;
                    dr.Refill = model[i].MedicationModel.Refill;
                    dr.Substitution = model[i].MedicationModel.Substitution;
                    dr.OtherNote = model[i].MedicationModel.OtherNote;
                    dr.PatientNotes = model[i].PatientNotes;
                    dr.Comments = model[i].Comments;
                    if (model[i].MedicationModel.StartDate.Year > 0001)
                    {
                        dr.StartDate = model[i].MedicationModel.StartDate;
                    }


                    if (model[i].StopDate.Year > 0001)
                    {
                        dr.StopDate = model[i].StopDate;
                    }
                    dr.StopReason = model[i].MedicationModel.StopReason;

                    if (model[i].MedicationModel.SigChangedDate.Year > 0001)
                    {
                        dr.SigChangedDate = model[i].MedicationModel.SigChangedDate;
                    }


                    dr.LastModifiedBy = MDVUtility.ToStr(model[i].LastModifiedBy);


                    if (model[i].LastModifiedDate.Year > 0001)
                    {
                        dr.LastModifiedDate = MDVUtility.ToDateTime(model[i].LastModifiedDate);
                    }
                    dr.IntendedUse = model[i].MedicationModel.IntendedUse;
                    dr.Number = model[i].MedicationModel.Number;
                    dr.Status = model[i].Status;
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(UserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(UserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.DrugDescription = model[i].drugModel.BrandName + '(' + model[i].drugModel.GenericName + ')';//kr

                    DSClinicalMedication.DrugRow drug = dsmedication.Drug.NewDrugRow();
                    drug.BrandName = model[i].drugModel.BrandName;
                    drug.BrandType = model[i].drugModel.BrandType;
                    drug.DrugDescription = model[i].drugModel.DrugDescription;
                    drug.FirstDataBankMedID = model[i].drugModel.FirstDataBankMedID;
                    drug.Form = model[i].drugModel.Form;
                    drug.GenericName = model[i].drugModel.GenericName;
                    drug.NDCID = model[i].drugModel.NDCID;
                    drug.RcopiaID = model[i].drugModel.RcopiaID;
                    drug.Route = model[i].drugModel.Route;
                    drug.RxnormID = model[i].drugModel.RxnormID;
                    drug.RxnormIDType = model[i].drugModel.RxnormIDType;
                    drug.Schedule = model[i].drugModel.Schedule;
                    drug.Strength = model[i].drugModel.Strength;
                    drug.IsActive = true;
                    drug.CreatedBy = MDVUtility.DecryptFrom64(UserName);
                    drug.CreatedOn = DateTime.Now;
                    drug.ModifiedBy = MDVUtility.DecryptFrom64(UserName);
                    drug.ModifiedOn = DateTime.Now;
                    drug.PrescriptionRcopiaId = model[i].RcopiaID;


                    DSClinicalMedication.PrescriptionRow prescription = dsmedication.Prescription.NewPrescriptionRow();
                    prescription.DrugRcopiaID = model[i].DrugRcopiaID;
                    prescription.PatientID = model[i].PatientID;
                    prescription.ProviderID = MDVUtility.ToLong(model[i].ProviderID);
                    prescription.Preparer_UserID = MDVUtility.ToLong(model[i].Preparer_UserID);

                    prescription.PrescriptionID = -i;
                    if (model[i].IsDeleted.ToLower() == "y")
                    {
                        prescription.IsDeleted = true;
                    }
                    else
                    {
                        prescription.IsDeleted = false;
                    }

                    prescription.RcopiaID = model[i].RcopiaID;
                    prescription.Voided = model[i].Voided;
                    prescription.Denied = model[i].Denied;

                    prescription.Action = model[i].MedicationModel.Action;
                    prescription.Dose = model[i].MedicationModel.Dose;
                    prescription.DoseUnit = model[i].MedicationModel.DoseUnit;
                    prescription.Routeby = model[i].MedicationModel.Routeby;
                    prescription.DoseTiming = model[i].MedicationModel.DoseTiming;
                    prescription.DoseOther = model[i].MedicationModel.DoseOther;
                    prescription.Duration = model[i].MedicationModel.Duration;
                    prescription.Quantity = model[i].MedicationModel.Quantity;
                    prescription.QuantityUnit = model[i].MedicationModel.QuantityUnit;
                    prescription.Refill = model[i].MedicationModel.Refill;
                    prescription.SubstitutionPermitted = model[i].SubstitutionPermitted;
                    if (model[i].PharamacyModel != null)
                    {
                        prescription.PharmacyRcopiaId = model[i].PharamacyModel.RcopiaID;
                    }
                    prescription.OtherNotes = model[i].OtherNotes;
                    prescription.PatientNotes = model[i].PatientNotes;
                    prescription.Comments = model[i].Comments;
                    if (model[i].CreatedDate.Year > 0001)
                    {
                        prescription.CreatedDate = model[i].CreatedDate;
                    }



                    if (model[i].CompletedDate.Year > 0001)
                    {
                        prescription.CompletedDate = model[i].CompletedDate;
                    }

                    if (model[i].StopDate.Year > 0001)
                    {
                        prescription.StopDate = model[i].StopDate;
                    }
                    prescription.LastModifiedBy = model[i].LastModifiedBy;

                    if (model[i].SignedDate.Year > 0001)
                    {
                        prescription.SignedDate = model[i].SignedDate;
                    }

                    if (model[i].LastModifiedDate.Year > 0001)
                    {
                        prescription.LastModifiedDate = model[i].LastModifiedDate;
                    }
                    prescription.IsActive = true;
                    prescription.CreatedBy = MDVUtility.DecryptFrom64(UserName);
                    prescription.CreatedOn = DateTime.Now;
                    prescription.ModifiedBy = MDVUtility.DecryptFrom64(UserName);
                    prescription.ModifiedOn = DateTime.Now;
                    prescription.CompletionAction = model[i].CompletionAction;
                    prescription.IntendedUse = model[i].IntendedUse;
                    prescription.SendMethod = model[i].SendMethod;
                    prescription.DrugDescription = model[i].drugModel.BrandName + '(' + model[i].drugModel.GenericName + ')';//kr
                    dsmedication.Prescription.AddPrescriptionRow(prescription);


                    dsmedication.Medication.AddMedicationRow(dr);
                    dsmedication.Drug.AddDrugRow(drug);
                }
                #region Database Insertion
                BLObject<DSClinicalMedication> obj = BLLClinicalObj.InsertPrescription(dsmedication, sharedVariable);
                dsmedication = obj.Data;
                if (obj.Data != null)
                {
                    var message = obj.Message.Split(',');
                    string SavedPrescriptionIds = "";
                    if (message[0] == "true")
                    {
                        int i = 1;
                        foreach (DSClinicalMedication.PrescriptionRow row in dsmedication.Tables[dsmedication.Prescription.TableName].Rows)
                        {
                            if (MDVUtility.ToInt64(row[dsmedication.Prescription.PrescriptionIDColumn]) > 0)
                            {
                                if (i == 1)
                                {
                                    SavedPrescriptionIds = MDVUtility.ToStr(row[dsmedication.Prescription.PrescriptionIDColumn]);
                                }
                                else
                                {
                                    SavedPrescriptionIds = SavedPrescriptionIds + "," + MDVUtility.ToStr(row[dsmedication.Prescription.PrescriptionIDColumn]);
                                }
                            }
                            i++;
                        }
                    }
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        SavedPrescriptionIds = SavedPrescriptionIds,
                        InsertPrescription = message[0],
                        IsPrescriptionDeleted = message[1],
                        PrescriptionId = dsmedication.Tables[dsmedication.Prescription.TableName].Rows[0][dsmedication.Prescription.PrescriptionIDColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var responseRcopiaerror = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                }



                #endregion



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

        internal string loadPrescriptionsForSoap(string prescriptionId, long noteId)
        {
            try
            {

                DSClinicalMedication dsPrescriptionSoap = null;
                BLObject<DSClinicalMedication> obj = BLLClinicalObj.loadPrescriptionsForSoap(prescriptionId, noteId);
                dsPrescriptionSoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPrescriptionSoap.Tables[dsPrescriptionSoap.Prescription.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PrescriptionsSoapCount = dsPrescriptionSoap.Tables[dsPrescriptionSoap.Prescription.TableName].Rows.Count,
                            PrescriptionsSoap_JSON = MDVUtility.JSON_DataTable(dsPrescriptionSoap.Tables[dsPrescriptionSoap.Prescription.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PrescriptionsSoapCount = 0,
                            PrescriptionsSoap_JSON = MDVUtility.JSON_DataTable(dsPrescriptionSoap.Tables[dsPrescriptionSoap.Prescription.TableName]),
                            Message = Common.AppPrivileges.No_Record_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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

        public string getLatestPrescriptionByPatientId(long patientId, long notesId)
        {
            try
            {

                DSClinicalMedication dsPrescription = null;
                BLObject<DSClinicalMedication> obj;

                obj = BLLClinicalObj.getLatestPrescriptionByPatientId(patientId, notesId);

                dsPrescription = obj.Data;
                var response = new
                {
                    status = true,
                    PrescriptionsSoapCount = dsPrescription.Tables[dsPrescription.Prescription.TableName].Rows.Count,
                    PrescriptionsSoap_JSON = MDVUtility.JSON_DataTable(dsPrescription.Tables[dsPrescription.Prescription.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        internal string detachPrescriptionsFromNotes(string prescriptionId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(prescriptionId) || notesId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.detachPrescriptionsFromNotes(prescriptionId, notesId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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

        internal string attachPrescriptionsWithNotes(string prescriptionId, long notesId)
        {
            try
            {
                DSClinicalMedication dsPrescription = null;
                if (string.IsNullOrEmpty(prescriptionId) || notesId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<DSClinicalMedication> obj = BLLClinicalObj.attachPrescriptionsWithNotes(prescriptionId, notesId);
                    if (obj.Data != null)
                    {
                        dsPrescription = obj.Data;
                        var response = new
                        {
                            status = true,
                            PrescriptionsTotalCount = dsPrescription.Tables[dsPrescription.Prescription.TableName].Rows.Count,
                            PrescriptionsCount = dsPrescription.Tables[dsPrescription.Prescription.TableName].Rows.Count,
                            PrescriptionsLoad_JSON = MDVUtility.JSON_DataTable(dsPrescription.Tables[dsPrescription.Prescription.TableName]),
                            PrescriptionsHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsPrescription.Tables[dsPrescription.Prescription.TableName]),
                            Message = Common.AppPrivileges.Update_Message
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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

        /// Author: ZeeshanAK
        /// Purpose: Function to load SOAP for Medications.
        /// Date : January 15, 2016
        internal string getMedicationsForSoap(string medicationID, long patientId)
        {
            try
            {

                DSClinicalMedication dsMedicationSoap = null;
                BLObject<DSClinicalMedication> obj = BLLClinicalObj.loadMedicationsForSoap(medicationID, patientId);


                dsMedicationSoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsMedicationSoap.Tables[dsMedicationSoap.Medication.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MedicationSoapCount = dsMedicationSoap.Tables[dsMedicationSoap.Medication.TableName].Rows.Count,
                            MedicationSoap_JSON = MDVUtility.JSON_DataTable(dsMedicationSoap.Tables[dsMedicationSoap.Medication.TableName]),
                            MedicationReviewSoap_JSON = MDVUtility.JSON_DataTable(dsMedicationSoap.Tables[dsMedicationSoap.MedicationReview.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MedicationSoapCount = 0,
                            MedicationSoap_JSON = MDVUtility.JSON_DataTable(dsMedicationSoap.Tables[dsMedicationSoap.Medication.TableName]),
                            Message = Common.AppPrivileges.No_Record_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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

        /// Author: ZeeshanAK
        /// Purpose: Function to load SOAP for Medications.
        /// Date : January 15, 2016
        internal string getMedicationsForNoteReconciledView(long patientId, long NoteId)
        {
            try
            {

                List<string> ReconciledIds = BLLClinicalObj.loadReconciledMedicationIds(patientId, NoteId);

                if (ReconciledIds.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        LastVisitReconciledMedIds = ReconciledIds.Count > 0 ? ReconciledIds[0] : "",
                        ReconcileMedsIds = ReconciledIds.Count > 1 ? ReconciledIds[1] : "",

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Error Occurred while getting Reconciled Medications"
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

        /// Author: ZeeshanAK
        /// Purpose: This Function will attach Medications to notes.
        /// Date : January 18, 2016
        internal string attachMedicationsWithNotes(string medicationID, long notesId, string bMedReconciled = "0", string MedReconciledId = "")
        {
            try
            {
                DSClinicalMedication dsMedication = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(medicationID)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<DSClinicalMedication> obj = BLLClinicalObj.attachMedicationsWithNotes(medicationID, notesId);
                    if (obj.Data != null)
                    {
                        dsMedication = obj.Data;
                        var response = new
                        {
                            status = true,
                            MedicationTotalCount = dsMedication.Tables[dsMedication.Medication.TableName].Rows.Count,
                            MedicationCount = dsMedication.Tables[dsMedication.Medication.TableName].Rows.Count,
                            MedicationLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.Medication.TableName]),
                            MedicationHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.Medication.TableName]),
                            Message = Common.AppPrivileges.Update_Message
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
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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

        /// Author: ZeeshanAK
        /// Purpose: This Function will detach Medications from notes.
        /// Date : January 19, 2016
        internal string detachMedicationsFromNotes(string medicationID, long notesID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(medicationID)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.detachMedicationsFromNotes(medicationID, notesID);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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

        /// Author: ZeeshanAK
        /// Purpose: This function will retrive Medication information for Notes attachment
        /// Date : January 18, 2016
        public string getLatestMedicationsByPatientId(Int64 patientId, Int64 userId, Int64 entityId)
        {
            try
            {

                DSClinicalMedication dsMedication = null;
                BLObject<DSClinicalMedication> obj;

                obj = BLLClinicalObj.getLatestMedicationsByPatientId(patientId, userId, entityId);
                dsMedication = obj.Data;



                var response = new
                {
                    status = true,
                    MedicationSoapCount = dsMedication.Tables[dsMedication.Medication.TableName].Rows.Count,
                    MedicationSoap_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.Medication.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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


        /// Author: ZeeshanAK
        /// Purpose:  to load Medications for Batch Patient List
        /// Date : April 06, 2016
        public string getAllMedications(MedicationModel model)
        {
            try
            {

                DSClinicalMedication dsMedications = null;
                BLObject<DSClinicalMedication> obj;

                obj = BLLClinicalObj.LookupMedications(MDVUtility.ToInt32(model.PatientID), MDVUtility.ToInt32(model.MedicationID), MDVUtility.ToStr(model.MedicationName));

                dsMedications = obj.Data;
                var response = new
                {
                    status = true,
                    MedicationsCount = dsMedications.Tables[dsMedications.Medication.TableName].Rows.Count,
                    MedicationsLoad_JSON = MDVUtility.JSON_DataTable(dsMedications.Tables[dsMedications.Medication.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        internal string LookupMedicationsReprot()
        {
            try
            {
                BLObject<List<MedicationLookupModel>> obj = BLLClinicalObj.LookupMedicationsReprot();
                List<MedicationLookupModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        medicationCount = modelList.Count,
                        medicationList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        medicationCount = 0,
                        Message = obj.Message
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

        internal string getReportHeaderFooter(MedicationModel model)
        {
            try
            {
                DSReportHeader dsreportHeader = null;
                BLObject<DSReportHeader> obj = new BLLAdminClinical().getReportHeaderTagsValue(MDVUtility.ToInt64(model.PatientID), 0, -1, "Medications/ Prescriptions");
                dsreportHeader = obj.Data;
                if (dsreportHeader.ReportHeaderTags.Rows.Count > 0)
                {
                    DSReportHeader.ReportHeaderTagsRow dr = (DSReportHeader.ReportHeaderTagsRow)dsreportHeader.Tables[dsreportHeader.ReportHeaderTags.TableName].Rows[0];

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        HeaderLogo = dr["HeaderLogo"],
                        FooterText = dr["FooterText"],
                        PatientText = dr["PatientText"],
                        ProviderText = dr["ProviderText"],
                        PracticeText = dr["PracticeText"],
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        HeaderLogo = "",
                        FooterText = "",
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
        #region "GetComplaintSoap text"
        internal string getComplaintForSoap(long ComplaintID, long NotesID)
        {
            try
            {

                DSClinicalComplaint dsComplaintSoap = null;
                BLObject<DSClinicalComplaint> obj = BLLClinicalObj.GetComplaint_ComplaintDetailsforSoap(ComplaintID, NotesID);
                dsComplaintSoap = obj.Data;
                if (obj.Data != null)
                {
                    if (dsComplaintSoap.Tables[dsComplaintSoap.ComplaintDetail.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ComplaintSoapCount = dsComplaintSoap.Tables[dsComplaintSoap.ComplaintDetail.TableName].Rows.Count,
                            ComplaintSoap_JSON = MDVUtility.JSON_DataTable(dsComplaintSoap.Tables[dsComplaintSoap.ComplaintDetail.TableName]),
                            //,
                            //ComplaintReviewSoap_JSON = MDVUtility.JSON_DataTable(dsComplaintSoap.Tables[dsComplaintSoap.NotesComplaint.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ComplaintSoapCount = 0,
                            ComplaintReviewSoap_JSON = MDVUtility.JSON_DataTable(dsComplaintSoap.Tables[dsComplaintSoap.ComplaintDetail.TableName]),
                            Message = Common.AppPrivileges.No_Record_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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

        #region 'Attachment/Detachment of Complaint with Progress note'
        /// <summary>
        /// This Function will detach Complaint from notes
        /// </summary>
        /// <param name="birthHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string detach_Complaint_From_Notes(long ComplaintID, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ComplaintID)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.detachComplaintFromNotes(ComplaintID, notesId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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

        /// <summary>
        /// This Function will attach Complaint to notes
        /// </summary>
        /// <param name="ComplaintID"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string attach_Complaint_With_Notes(long ComplaintID, long notesId)
        {
            try
            {
                DSClinicalComplaint dsComplaint = null;
                if (ComplaintID <= 0 || notesId <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<DSClinicalComplaint> obj = BLLClinicalObj.attachComplaintFromNotes(ComplaintID, notesId);
                    if (obj.Data != null)
                    {
                        dsComplaint = obj.Data;
                        var response = new
                        {
                            status = true,
                            //ComplaintTotalCount = dsComplaint.Tables[dsComplaint.NotesComplaint.TableName].Rows.Count,
                            ComplaintCount = dsComplaint.Tables[dsComplaint.NotesComplaint.TableName].Rows.Count,
                            Complaint_JSON = MDVUtility.JSON_DataTable(dsComplaint.Tables[dsComplaint.NotesComplaint.TableName]),
                            Message = Common.AppPrivileges.Update_Message
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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
    }
}