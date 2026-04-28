using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Text.RegularExpressions;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MDVision.IEHR.Controls.Patient.Demographics;
using MDVision.IEHR.Common;
using System.Threading;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.Business.BLL;
using System.Threading.Tasks;
using MDVision.Model.Clinical.DrFirst;
using System.Web.Configuration;
using MDVision.Model.Clinical.Orderset;
using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.EMR.Helpers.Clinical.Templates.OrderSet;
using MDVision.Model.Clinical.Medical;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Medical
{
    public class RcopiaHelper
    {

        private BLLRcopia BLLRcopiaObj = null;
        private BLLPatient BLLPatientObj = null;
        public RcopiaHelper()
        {
            BLLRcopiaObj = new BLLRcopia();
            BLLPatientObj = new BLLPatient();
        }
        private static RcopiaHelper _instance = null;
        private static bool isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
        public static RcopiaHelper Instance()
        {
            if (_instance == null)
                _instance = new RcopiaHelper();
            return _instance;
        }

        public string DownloadAllClinicals(RcopiaModel model)
        {
            bool AllergyDownloadSuccessfully = false;
            bool MedicationDownloadSuccessfully = false;
            bool PrescriptionDownloadSuccessfully = false;
            bool IsAllergyDownload = false;
            bool IsMedicationDownload = false;
            bool IsPrescriptionDownload = false;
            string SavedAllergyIds = "";
            string SavedMedicationIds = "";
            string SavedPrescriptionIds = "";
            try
            {

                model.IsPatientLastUpdateInfo = false;
                DSRcopia dsRcopia = new DSRcopia();
                BLObject<DSRcopia> obj = BLLRcopiaObj.SelectGetUrls();
                dsRcopia = obj.Data;
                if (obj.Data != null)
                {

                    model.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);
                    dsRcopia = new DSRcopia();
                    DSRcopia.Rcopia_PatientLastUpdateInfoRow dr = dsRcopia.Rcopia_PatientLastUpdateInfo.NewRcopia_PatientLastUpdateInfoRow();
                    dr.PatientId = MDVUtility.ToLong(model.PatientId);
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dsRcopia.Rcopia_PatientLastUpdateInfo.AddRcopia_PatientLastUpdateInfoRow(dr);
                    BLObject<DSRcopia> obj1 = BLLRcopiaObj.SelectPatientLastUpdateInfo(dsRcopia);
                    dsRcopia = obj1.Data;
                    if (obj1.Data != null)
                    {



                        model.AllergyLastUpdateDate = MDVUtility.ToStr((dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn.ColumnName])) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn.ColumnName]).ToString("MM/dd/yyyy HH:mm:ss")) : "";
                        model.MedicationLastUpdateDate = MDVUtility.ToStr((dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName])) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName]).ToString("MM/dd/yyyy HH:mm:ss")) : "";
                        model.PrescriptionLastUpdateDate = MDVUtility.ToStr((dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn.ColumnName])) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn.ColumnName]).ToString("MM/dd/yyyy  HH:mm:ss")) : "";
                        #region allergies download
                        //model.AllergyLastUpdateDate = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn.ColumnName]);
                        dynamic DownloadAllergiesResponse = JObject.Parse(DownloadAllergiesAndSave(model));

                        if (DownloadAllergiesResponse.DownloadAllergiesCount != 0)
                        {
                            IsAllergyDownload = true;
                            model.IsPatientLastUpdateInfo = true;
                            if (DownloadAllergiesResponse.status == true)
                            {
                                SavedAllergyIds = DownloadAllergiesResponse.SavedAllergyIds;
                                model.AllergyLastUpdateDate = DownloadAllergiesResponse.lastUpdateDate;
                                var responseAllergy = UpdatePatientLastUpdateInfo(model);
                                dynamic Allergyresponse = JObject.Parse(responseAllergy);
                                if (Allergyresponse.status != true)
                                {
                                    var response = new
                                    {
                                        status = false,
                                        AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                        MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                        PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                        IsAllergyDownload = IsAllergyDownload,
                                        IsMedicationDownload = IsMedicationDownload,
                                        IsPrescriptionDownload = IsPrescriptionDownload,
                                        Message = Allergyresponse.Message,
                                        SavedAllergyIds = SavedAllergyIds,
                                        SavedPrescriptionIds = SavedPrescriptionIds,
                                        SavedMedicationIds = SavedMedicationIds,

                                    };
                                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                                }
                                else
                                {
                                    AllergyDownloadSuccessfully = true;

                                }

                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                    IsAllergyDownload = IsAllergyDownload,
                                    IsMedicationDownload = IsMedicationDownload,
                                    IsPrescriptionDownload = IsPrescriptionDownload,
                                    SavedAllergyIds = SavedAllergyIds,
                                    SavedPrescriptionIds = SavedPrescriptionIds,
                                    SavedMedicationIds = SavedMedicationIds,
                                    Message = DownloadAllergiesResponse.MessageFromSave
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {

                            model.AllergyLastUpdateDate = DownloadAllergiesResponse.lastUpdateDate;
                            var responseAllergy = UpdatePatientLastUpdateInfo(model);
                            dynamic Allergyresponse = JObject.Parse(responseAllergy);
                            if (Allergyresponse.status != true)
                            {
                                var response = new
                                {
                                    status = false,
                                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                    IsAllergyDownload = IsAllergyDownload,
                                    IsMedicationDownload = IsMedicationDownload,
                                    IsPrescriptionDownload = IsPrescriptionDownload,
                                    SavedAllergyIds = SavedAllergyIds,
                                    SavedPrescriptionIds = SavedPrescriptionIds,
                                    SavedMedicationIds = SavedMedicationIds,
                                    Message = Allergyresponse.Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                            else
                            {
                                AllergyDownloadSuccessfully = true;
                            }

                        }
                        #endregion
                        #region medication download

                        //model.MedicationLastUpdateDate = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName]);
                        dynamic DownloadMedicationResponse = JObject.Parse(DownloadMadicationsAndSave(model));

                        if (DownloadMedicationResponse.DownloadMedicationCount != 0)
                        {
                            IsMedicationDownload = true;
                            model.IsPatientLastUpdateInfo = true;
                            if (DownloadMedicationResponse.status == true)
                            {
                                SavedMedicationIds = DownloadMedicationResponse.SavedMedicationIds;
                                model.MedicationLastUpdateDate = DownloadMedicationResponse.lastUpdateDate;

                                var responseMedication = UpdatePatientLastUpdateInfo(model);
                                dynamic Medicationresponse = JObject.Parse(responseMedication);
                                if (Medicationresponse.status != true)
                                {
                                    var response = new
                                    {
                                        status = false,
                                        AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                        MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                        PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                        IsAllergyDownload = IsAllergyDownload,
                                        IsMedicationDownload = IsMedicationDownload,
                                        IsPrescriptionDownload = IsPrescriptionDownload,
                                        SavedAllergyIds = SavedAllergyIds,
                                        SavedPrescriptionIds = SavedPrescriptionIds,
                                        SavedMedicationIds = SavedMedicationIds,
                                        Message = Medicationresponse.Message
                                    };
                                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                                }
                                else
                                {
                                    MedicationDownloadSuccessfully = true;
                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                    IsAllergyDownload = IsAllergyDownload,
                                    IsMedicationDownload = IsMedicationDownload,
                                    IsPrescriptionDownload = IsPrescriptionDownload,
                                    SavedAllergyIds = SavedAllergyIds,
                                    SavedPrescriptionIds = SavedPrescriptionIds,
                                    SavedMedicationIds = SavedMedicationIds,
                                    Message = DownloadMedicationResponse.MessageFromSave
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {

                            model.MedicationLastUpdateDate = DownloadMedicationResponse.lastUpdateDate;
                            var responseMedication = UpdatePatientLastUpdateInfo(model);
                            dynamic Medicationresponse = JObject.Parse(responseMedication);
                            if (Medicationresponse.status != true)
                            {
                                var response = new
                                {
                                    status = false,
                                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                    IsAllergyDownload = IsAllergyDownload,
                                    IsMedicationDownload = IsMedicationDownload,
                                    IsPrescriptionDownload = IsPrescriptionDownload,
                                    SavedAllergyIds = SavedAllergyIds,
                                    SavedPrescriptionIds = SavedPrescriptionIds,
                                    SavedMedicationIds = SavedMedicationIds,
                                    Message = Medicationresponse.Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                            else
                            {
                                MedicationDownloadSuccessfully = true;
                            }
                        }

                        #endregion

                        #region prescription download

                        //model.PrescriptionLastUpdateDate = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn.ColumnName]);
                        dynamic DownloadPrescriptionResponse = JObject.Parse(DownloadPriscriptionsAndSave(model));

                        if (DownloadPrescriptionResponse.DownloadPrescriptionCount != 0)
                        {
                            IsPrescriptionDownload = true;
                            model.IsPatientLastUpdateInfo = true;
                            if (DownloadPrescriptionResponse.status == true)
                            {

                                SavedPrescriptionIds = DownloadPrescriptionResponse.SavedPrescriptionIds;
                                model.PrescriptionLastUpdateDate = DownloadPrescriptionResponse.lastUpdateDate;
                                var responsePrescription = UpdatePatientLastUpdateInfo(model);
                                dynamic Prescriptionresponse = JObject.Parse(responsePrescription);
                                if (Prescriptionresponse.status != true)
                                {
                                    var response = new
                                    {
                                        status = false,
                                        AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                        MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                        PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                        IsAllergyDownload = IsAllergyDownload,
                                        IsMedicationDownload = IsMedicationDownload,
                                        IsPrescriptionDownload = IsPrescriptionDownload,
                                        SavedAllergyIds = SavedAllergyIds,
                                        SavedPrescriptionIds = SavedPrescriptionIds,
                                        SavedMedicationIds = SavedMedicationIds,
                                        Message = Prescriptionresponse.Message
                                    };
                                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                                }
                                else
                                {
                                    PrescriptionDownloadSuccessfully = true;
                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                    IsAllergyDownload = IsAllergyDownload,
                                    IsMedicationDownload = IsMedicationDownload,
                                    IsPrescriptionDownload = IsPrescriptionDownload,
                                    SavedAllergyIds = SavedAllergyIds,
                                    SavedPrescriptionIds = SavedPrescriptionIds,
                                    SavedMedicationIds = SavedMedicationIds,
                                    Message = DownloadPrescriptionResponse.MessageFromSave
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {

                            model.PrescriptionLastUpdateDate = DownloadPrescriptionResponse.lastUpdateDate;
                            var responsePrescription = UpdatePatientLastUpdateInfo(model);
                            dynamic Prescriptionresponse = JObject.Parse(responsePrescription);
                            if (Prescriptionresponse.status != true)
                            {
                                var response = new
                                {
                                    status = false,
                                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                    IsAllergyDownload = IsAllergyDownload,
                                    IsMedicationDownload = IsMedicationDownload,
                                    IsPrescriptionDownload = IsPrescriptionDownload,
                                    SavedAllergyIds = SavedAllergyIds,
                                    SavedPrescriptionIds = SavedPrescriptionIds,
                                    SavedMedicationIds = SavedMedicationIds,
                                    Message = Prescriptionresponse.Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                            else
                            {
                                PrescriptionDownloadSuccessfully = true;
                            }

                        }

                        #endregion

                        #region Reviewed download



                        dynamic DownloadReviewsResponse = JObject.Parse(DownloadReviewsAndSave(model));

                        if (DownloadReviewsResponse.DownloadReviewCount != 0)
                        {
                            model.IsPatientLastUpdateInfo = true;
                            if (DownloadReviewsResponse.status != true)
                            {
                                var response = new
                                {
                                    status = false,
                                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                    IsAllergyDownload = IsAllergyDownload,
                                    IsMedicationDownload = IsMedicationDownload,
                                    IsPrescriptionDownload = IsPrescriptionDownload,
                                    SavedAllergyIds = SavedAllergyIds,
                                    SavedPrescriptionIds = SavedPrescriptionIds,
                                    SavedMedicationIds = SavedMedicationIds,
                                    Message = DownloadReviewsResponse.Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        #endregion

                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                            MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                            PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                            IsAllergyDownload = IsAllergyDownload,
                            IsMedicationDownload = IsMedicationDownload,
                            IsPrescriptionDownload = IsPrescriptionDownload,
                            SavedAllergyIds = SavedAllergyIds,
                            SavedPrescriptionIds = SavedPrescriptionIds,
                            SavedMedicationIds = SavedMedicationIds,
                            Message = obj1.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                        MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                        PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                        IsAllergyDownload = IsAllergyDownload,
                        IsMedicationDownload = IsMedicationDownload,
                        IsPrescriptionDownload = IsPrescriptionDownload,
                        SavedAllergyIds = SavedAllergyIds,
                        SavedPrescriptionIds = SavedPrescriptionIds,
                        SavedMedicationIds = SavedMedicationIds,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }

                var response2 = new
                {
                    status = true,
                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                    IsAllergyDownload = IsAllergyDownload,
                    IsMedicationDownload = IsMedicationDownload,
                    IsPrescriptionDownload = IsPrescriptionDownload,
                    SavedAllergyIds = SavedAllergyIds,
                    SavedPrescriptionIds = SavedPrescriptionIds,
                    SavedMedicationIds = SavedMedicationIds,
                    Message = "All Clinicals Are Download SuccessFully"
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response2);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                    IsAllergyDownload = IsAllergyDownload,
                    IsMedicationDownload = IsMedicationDownload,
                    IsPrescriptionDownload = IsPrescriptionDownload,
                    SavedAllergyIds = SavedAllergyIds,
                    SavedPrescriptionIds = SavedPrescriptionIds,
                    SavedMedicationIds = SavedMedicationIds,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string DownloadAllClinicalsOp(RcopiaModel model)
        {
            bool AllergyDownloadSuccessfully = false;
            bool MedicationDownloadSuccessfully = false;
            bool PrescriptionDownloadSuccessfully = false;
            bool IsAllergyDownload = false;
            bool IsMedicationDownload = false;
            bool IsPrescriptionDownload = false;
            bool IsPrescriptionDeleted = false;
            string SavedAllergyIds = "";
            string SavedPrescriptionIds = "";
            string SavedMedicationIds = "";
            Int64 AllergyReviewID = 0;
            Int64 MedicationReviewID = 0;
            try
            {



                model.IsPatientLastUpdateInfo = false;
                DSRcopia dsRcopia = new DSRcopia();

                List<RcopiaModel> ListRcopia = GetRcopiaInfo();
                model.RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                model.RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                model.RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                model.RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                model.RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                model.RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;

                BLObject<DSRcopia> obj = BLLRcopiaObj.SelectGetUrls();
                dsRcopia = obj.Data;
                model.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);



                BLObject<DSRcopia> obj1 = BLLRcopiaObj.SelectPatientLastUpdateInfoOp(MDVUtility.ToLong(model.PatientId));
                dsRcopia = obj1.Data;
                if (obj1.Data != null)
                {
                    int LastUpdateRowsCount = (dsRcopia != null && dsRcopia.Tables.Count > 0 && dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName] != null) ? dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows.Count : 0;

                    if (LastUpdateRowsCount > 0)
                    {
                        model.AllergyLastUpdateDate = MDVUtility.ToStr((dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn.ColumnName])) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn.ColumnName]).ToString("MM/dd/yyyy HH:mm:ss")) : "";
                        model.MedicationLastUpdateDate = MDVUtility.ToStr((dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName])) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName]).ToString("MM/dd/yyyy HH:mm:ss")) : "";
                        model.PrescriptionLastUpdateDate = MDVUtility.ToStr((dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn.ColumnName])) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn.ColumnName]).ToString("MM/dd/yyyy  HH:mm:ss")) : "";
                    }

                    #region allergies download
                    //model.AllergyLastUpdateDate = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn.ColumnName]);
                    dynamic DownloadAllergiesResponse = JObject.Parse(DownloadAllergiesAndSave(model));

                    if (DownloadAllergiesResponse.DownloadAllergiesCount != 0)
                    {
                        IsAllergyDownload = true;
                        model.IsPatientLastUpdateInfo = true;
                        if (DownloadAllergiesResponse.status == true)
                        {
                            SavedAllergyIds = DownloadAllergiesResponse.SavedAllergyIds;
                            model.AllergyLastUpdateDate = DownloadAllergiesResponse.lastUpdateDate;
                            //var responseAllergy = UpdatePatientLastUpdateInfo(model);
                            //dynamic Allergyresponse = JObject.Parse(responseAllergy);
                            //if (Allergyresponse.status != true)
                            //{
                            //    var response = new
                            //    {
                            //        status = false,
                            //        AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                            //        MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                            //        PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                            //        IsAllergyDownload = IsAllergyDownload,
                            //        IsMedicationDownload = IsMedicationDownload,
                            //        IsPrescriptionDownload = IsPrescriptionDownload,
                            //        Message = Allergyresponse.Message,
                            //        SavedAllergyIds = SavedAllergyIds,

                            //    };
                            //    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            //}
                            //else
                            //{
                            AllergyDownloadSuccessfully = true;

                            //}

                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                IsAllergyDownload = IsAllergyDownload,
                                IsMedicationDownload = IsMedicationDownload,
                                IsPrescriptionDownload = IsPrescriptionDownload,
                                SavedAllergyIds = SavedAllergyIds,
                                SavedPrescriptionIds = SavedPrescriptionIds,
                                SavedMedicationIds = SavedMedicationIds,
                                Message = DownloadAllergiesResponse.MessageFromSave,
                                AllergyReviewID = AllergyReviewID,
                                MedicationReviewID = MedicationReviewID,
                                IsPrescriptionDeleted = IsPrescriptionDeleted

                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {

                        model.AllergyLastUpdateDate = DownloadAllergiesResponse.lastUpdateDate;
                        //var responseAllergy = UpdatePatientLastUpdateInfo(model);
                        //dynamic Allergyresponse = JObject.Parse(responseAllergy);
                        //if (Allergyresponse.status != true)
                        //{
                        //    var response = new
                        //    {
                        //        status = false,
                        //        AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                        //        MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                        //        PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                        //        IsAllergyDownload = IsAllergyDownload,
                        //        IsMedicationDownload = IsMedicationDownload,
                        //        IsPrescriptionDownload = IsPrescriptionDownload,
                        //        SavedAllergyIds = SavedAllergyIds,
                        //        Message = Allergyresponse.Message
                        //    };
                        //    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        //}
                        //else
                        //{
                        AllergyDownloadSuccessfully = true;
                        //}

                    }
                    #endregion

                    #region medication download

                    //model.MedicationLastUpdateDate = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName]);
                    dynamic DownloadMedicationResponse = JObject.Parse(DownloadMadicationsAndSave(model));

                    if (DownloadMedicationResponse.DownloadMedicationCount != 0)
                    {
                        IsMedicationDownload = true;
                        model.IsPatientLastUpdateInfo = true;
                        if (DownloadMedicationResponse.status == true)
                        {
                            SavedMedicationIds = DownloadMedicationResponse.SavedMedicationIds;
                            model.MedicationLastUpdateDate = DownloadMedicationResponse.lastUpdateDate;

                            //var responseMedication = UpdatePatientLastUpdateInfo(model);
                            //dynamic Medicationresponse = JObject.Parse(responseMedication);
                            //if (Medicationresponse.status != true)
                            //{
                            //    var response = new
                            //    {
                            //        status = false,
                            //        AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                            //        MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                            //        PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                            //        IsAllergyDownload = IsAllergyDownload,
                            //        IsMedicationDownload = IsMedicationDownload,
                            //        IsPrescriptionDownload = IsPrescriptionDownload,
                            //        SavedAllergyIds = SavedAllergyIds,
                            //        Message = Medicationresponse.Message
                            //    };
                            //    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            //}
                            //else
                            //{
                            MedicationDownloadSuccessfully = true;
                            //}
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                IsAllergyDownload = IsAllergyDownload,
                                IsMedicationDownload = IsMedicationDownload,
                                IsPrescriptionDownload = IsPrescriptionDownload,
                                SavedAllergyIds = SavedAllergyIds,
                                SavedPrescriptionIds = SavedPrescriptionIds,
                                SavedMedicationIds = SavedMedicationIds,
                                Message = DownloadMedicationResponse.MessageFromSave,
                                AllergyReviewID = AllergyReviewID,
                                MedicationReviewID = MedicationReviewID,
                                IsPrescriptionDeleted = IsPrescriptionDeleted
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {

                        model.MedicationLastUpdateDate = DownloadMedicationResponse.lastUpdateDate;
                        //var responseMedication = UpdatePatientLastUpdateInfo(model);
                        //dynamic Medicationresponse = JObject.Parse(responseMedication);
                        //if (Medicationresponse.status != true)
                        //{
                        //    var response = new
                        //    {
                        //        status = false,
                        //        AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                        //        MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                        //        PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                        //        IsAllergyDownload = IsAllergyDownload,
                        //        IsMedicationDownload = IsMedicationDownload,
                        //        IsPrescriptionDownload = IsPrescriptionDownload,
                        //        SavedAllergyIds = SavedAllergyIds,
                        //        Message = Medicationresponse.Message
                        //    };
                        //    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        //}
                        //else
                        //{
                        MedicationDownloadSuccessfully = true;
                        //}
                    }

                    #endregion

                    #region prescription download

                    //model.PrescriptionLastUpdateDate = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn.ColumnName]);
                    dynamic DownloadPrescriptionResponse = JObject.Parse(DownloadPriscriptionsAndSave(model));

                    if (DownloadPrescriptionResponse.DownloadPrescriptionCount != 0)
                    {
                        IsPrescriptionDownload = true;
                        model.IsPatientLastUpdateInfo = true;
                        if (DownloadPrescriptionResponse.status == true)
                        {
                            SavedPrescriptionIds = DownloadPrescriptionResponse.SavedPrescriptionIds;
                            model.PrescriptionLastUpdateDate = DownloadPrescriptionResponse.lastUpdateDate;
                            var responsePrescription = UpdatePatientLastUpdateInfo(model);
                            dynamic Prescriptionresponse = JObject.Parse(responsePrescription);
                            if (Prescriptionresponse.status != true)
                            {
                                var response = new
                                {
                                    status = false,
                                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                    IsAllergyDownload = IsAllergyDownload,
                                    IsMedicationDownload = IsMedicationDownload,
                                    IsPrescriptionDownload = IsPrescriptionDownload,
                                    SavedAllergyIds = SavedAllergyIds,
                                    SavedPrescriptionIds = SavedPrescriptionIds,
                                    SavedMedicationIds = SavedMedicationIds,
                                    Message = Prescriptionresponse.Message,
                                    AllergyReviewID = AllergyReviewID,
                                    MedicationReviewID = MedicationReviewID,
                                    IsPrescriptionDeleted = IsPrescriptionDeleted
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                            else
                            {
                                if (DownloadPrescriptionResponse.IsPrescriptionDeleted == "true")
                                {
                                    IsPrescriptionDeleted = true;
                                }

                                PrescriptionDownloadSuccessfully = true;
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                IsAllergyDownload = IsAllergyDownload,
                                IsMedicationDownload = IsMedicationDownload,
                                IsPrescriptionDownload = IsPrescriptionDownload,
                                SavedAllergyIds = SavedAllergyIds,
                                SavedPrescriptionIds = SavedPrescriptionIds,
                                SavedMedicationIds = SavedMedicationIds,
                                Message = DownloadPrescriptionResponse.MessageFromSave,
                                AllergyReviewID = AllergyReviewID,
                                MedicationReviewID = MedicationReviewID,
                                IsPrescriptionDeleted = IsPrescriptionDeleted
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {

                        model.PrescriptionLastUpdateDate = DownloadPrescriptionResponse.lastUpdateDate;
                        var responsePrescription = UpdatePatientLastUpdateInfo(model);
                        dynamic Prescriptionresponse = JObject.Parse(responsePrescription);
                        if (Prescriptionresponse.status != true)
                        {
                            var response = new
                            {
                                status = false,
                                AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                IsAllergyDownload = IsAllergyDownload,
                                IsMedicationDownload = IsMedicationDownload,
                                IsPrescriptionDownload = IsPrescriptionDownload,
                                SavedAllergyIds = SavedAllergyIds,
                                SavedPrescriptionIds = SavedPrescriptionIds,
                                SavedMedicationIds = SavedMedicationIds,
                                Message = Prescriptionresponse.Message,
                                AllergyReviewID = AllergyReviewID,
                                MedicationReviewID = MedicationReviewID,
                                IsPrescriptionDeleted = IsPrescriptionDeleted
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        else
                        {
                            PrescriptionDownloadSuccessfully = true;
                        }

                    }

                    #endregion

                    #region Reviewed download



                    dynamic DownloadReviewsResponse = JObject.Parse(DownloadReviewsAndSave(model));

                    if (DownloadReviewsResponse.DownloadReviewCount != 0)
                    {
                        model.IsPatientLastUpdateInfo = true;
                        if (DownloadReviewsResponse.status != true)
                        {
                            var response = new
                            {
                                status = false,
                                AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                                MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                                IsAllergyDownload = IsAllergyDownload,
                                IsMedicationDownload = IsMedicationDownload,
                                IsPrescriptionDownload = IsPrescriptionDownload,
                                SavedAllergyIds = SavedAllergyIds,
                                SavedPrescriptionIds = SavedPrescriptionIds,
                                SavedMedicationIds = SavedMedicationIds,
                                Message = DownloadReviewsResponse.Message,
                                AllergyReviewID = DownloadReviewsResponse.AllergyReviewID,
                                MedicationReviewID = DownloadReviewsResponse.MedicationReviewID,
                                IsPrescriptionDeleted = IsPrescriptionDeleted
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        else
                        {
                            AllergyReviewID = DownloadReviewsResponse.AllergyReviewID;
                            MedicationReviewID = DownloadReviewsResponse.MedicationReviewID;
                        }
                    }
                    #endregion

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                        MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                        PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                        IsAllergyDownload = IsAllergyDownload,
                        IsMedicationDownload = IsMedicationDownload,
                        IsPrescriptionDownload = IsPrescriptionDownload,
                        SavedAllergyIds = SavedAllergyIds,
                        SavedPrescriptionIds = SavedPrescriptionIds,
                        SavedMedicationIds = SavedMedicationIds,
                        Message = obj1.Message,
                        AllergyReviewID = AllergyReviewID,
                        MedicationReviewID = MedicationReviewID,
                        IsPrescriptionDeleted = IsPrescriptionDeleted
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }



                var response2 = new
                {
                    status = true,
                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                    IsAllergyDownload = IsAllergyDownload,
                    IsMedicationDownload = IsMedicationDownload,
                    IsPrescriptionDownload = IsPrescriptionDownload,
                    SavedAllergyIds = SavedAllergyIds,
                    SavedPrescriptionIds = SavedPrescriptionIds,
                    SavedMedicationIds = SavedMedicationIds,
                    Message = "All Clinicals Are Download SuccessFully",
                    AllergyReviewID = AllergyReviewID,
                    MedicationReviewID = MedicationReviewID,
                    IsPrescriptionDeleted = IsPrescriptionDeleted
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response2);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                    IsAllergyDownload = IsAllergyDownload,
                    IsMedicationDownload = IsMedicationDownload,
                    IsPrescriptionDownload = IsPrescriptionDownload,
                    SavedAllergyIds = SavedAllergyIds,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                    AllergyReviewID = AllergyReviewID,
                    MedicationReviewID = MedicationReviewID,
                    IsPrescriptionDeleted = IsPrescriptionDeleted
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string DownloadMedications(RcopiaModel model, string OrderSetId, bool AddInLastUpdateTime = false)
        {
            bool MedicationDownloadSuccessfully = false;
            bool IsMedicationDownload = false;
            string SavedMedicationIds = "";
            try
            {



                model.IsPatientLastUpdateInfo = false;
                DSRcopia dsRcopia = new DSRcopia();

                List<RcopiaModel> ListRcopia = GetRcopiaInfo();
                model.RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                model.RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                model.RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                model.RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                model.RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                model.RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;

                BLObject<DSRcopia> obj = BLLRcopiaObj.SelectGetUrls();
                dsRcopia = obj.Data;
                model.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);



                BLObject<DSRcopia> obj1 = BLLRcopiaObj.SelectPatientLastUpdateInfoOp(MDVUtility.ToLong(model.PatientId));
                dsRcopia = obj1.Data;
                if (obj1.Data != null)
                {
                    int LastUpdateRowsCount = (dsRcopia != null && dsRcopia.Tables.Count > 0 && dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName] != null) ? dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows.Count : 0;

                    if (LastUpdateRowsCount > 0)
                    {
                        model.AllergyLastUpdateDate = MDVUtility.ToStr((dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn.ColumnName])) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn.ColumnName]).ToString("MM/dd/yyyy HH:mm:ss")) : "";
                        model.MedicationLastUpdateDate = MDVUtility.ToStr((dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName])) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName]).ToString("MM/dd/yyyy HH:mm:ss")) : "";
                        model.PrescriptionLastUpdateDate = MDVUtility.ToStr((dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn.ColumnName])) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn.ColumnName]).ToString("MM/dd/yyyy  HH:mm:ss")) : "";
                    }


                    #region medication download

                    //model.MedicationLastUpdateDate = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName]);
                    dynamic DownloadMedicationResponse = JObject.Parse(DownloadMadicationsAndSave(model, "", null, null, OrderSetId));

                    if (DownloadMedicationResponse.DownloadMedicationCount != 0)
                    {
                        //IsMedicationDownload = true;
                        model.IsPatientLastUpdateInfo = true;
                        if (DownloadMedicationResponse.status == true)
                        {
                            SavedMedicationIds = DownloadMedicationResponse.SavedMedicationIds;
                            if (AddInLastUpdateTime)
                            {
                                var dateTime = MDVUtility.ToDateTime(DownloadMedicationResponse.lastUpdateDate);
                                dateTime = dateTime.AddSeconds(1);
                                model.MedicationLastUpdateDate = MDVUtility.ToStr(dateTime);
                                //MDVLogger.RcopiaLogMessage("Request: Download_Medication_ChangeTime", "", "", MDVUtility.ToStr(DownloadMedicationResponse.lastUpdateDate) + "" + MDVUtility.ToStr(dateTime), "", 0, true);
                            }
                            else
                            {
                                model.MedicationLastUpdateDate = DownloadMedicationResponse.lastUpdateDate;
                            }

                            var responseUpdatePatientLastUpdateInfo = UpdatePatientLastUpdateInfoOp(model, "Medication");
                            dynamic ResponseUpdatePatientLastUpdateInfo = JObject.Parse(responseUpdatePatientLastUpdateInfo);
                            if (ResponseUpdatePatientLastUpdateInfo.status != "True")
                            {
                                var response = new
                                {
                                    status = false,
                                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                    IsMedicationDownload = IsMedicationDownload,
                                    SavedMedicationIds = SavedMedicationIds,
                                    Message = DownloadMedicationResponse.MessageFromSave,
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                            else
                            {
                                IsMedicationDownload = true;
                                MedicationDownloadSuccessfully = true;
                            }

                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                                IsMedicationDownload = IsMedicationDownload,
                                SavedMedicationIds = SavedMedicationIds,
                                Message = DownloadMedicationResponse.MessageFromSave,
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        model.MedicationLastUpdateDate = DownloadMedicationResponse.lastUpdateDate;
                        MedicationDownloadSuccessfully = true;
                    }

                    #endregion


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                        IsMedicationDownload = IsMedicationDownload,
                        SavedMedicationIds = SavedMedicationIds,
                        Message = obj1.Message,
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }



                var response2 = new
                {
                    status = true,
                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                    IsMedicationDownload = IsMedicationDownload,
                    SavedMedicationIds = SavedMedicationIds,
                    Message = "Medication Are Download SuccessFully",
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response2);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                    IsMedicationDownload = IsMedicationDownload,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string UpdatePatientLastUpdateInfo(RcopiaModel model, bool OnlyInsert = false)
        {
            try
            {
                DSRcopia dsRcopia = new DSRcopia();
                DSRcopia.Rcopia_PatientLastUpdateInfoRow dr = dsRcopia.Rcopia_PatientLastUpdateInfo.NewRcopia_PatientLastUpdateInfoRow();
                dr.PatientId = MDVUtility.ToLong(model.PatientId);
                if (model.AllergyLastUpdateDate != "" && model.AllergyLastUpdateDate != null)
                {
                    dr.AllergyLastUpdateDate = MDVUtility.ToDateTime(model.AllergyLastUpdateDate);
                }
                else
                {
                    dr[dsRcopia.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn] = DBNull.Value;
                }
                if (model.MedicationLastUpdateDate != "" && model.MedicationLastUpdateDate != null)
                {
                    dr.MedicationLastUpdateDate = MDVUtility.ToDateTime(model.MedicationLastUpdateDate);
                }
                else
                {
                    dr[dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn] = DBNull.Value;
                }
                if (model.PrescriptionLastUpdateDate != "" && model.PrescriptionLastUpdateDate != null)
                {
                    dr.PrescriptionLastUpdateDate = MDVUtility.ToDateTime(model.PrescriptionLastUpdateDate);
                }
                else
                {
                    dr[dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn] = DBNull.Value;
                }

                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.OnlyInsert = OnlyInsert;
                dsRcopia.Rcopia_PatientLastUpdateInfo.AddRcopia_PatientLastUpdateInfoRow(dr);
                BLObject<DSRcopia> obj = BLLRcopiaObj.UpdatePtientLastUpdateDate(dsRcopia);
                dsRcopia = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = obj.Message
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


        public string UpdatePatientLastUpdateInfoOp(RcopiaModel model, string WhichAttrUpdate, SharedVariable sharedVariable = null)
        {
            try
            {
                DSRcopia dsRcopia = new DSRcopia();
                DSRcopia.Rcopia_PatientLastUpdateInfoRow dr = dsRcopia.Rcopia_PatientLastUpdateInfo.NewRcopia_PatientLastUpdateInfoRow();
                dr.PatientId = MDVUtility.ToLong(model.PatientId);
                if (model.AllergyLastUpdateDate != "" && WhichAttrUpdate == "Allergy")
                {
                    dr.AllergyLastUpdateDate = MDVUtility.ToDateTime(model.AllergyLastUpdateDate);
                }
                else
                {
                    dr[dsRcopia.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn] = DBNull.Value;
                }
                if (model.MedicationLastUpdateDate != "" && WhichAttrUpdate == "Medication")
                {
                    dr.MedicationLastUpdateDate = MDVUtility.ToDateTime(model.MedicationLastUpdateDate);
                }
                else
                {
                    dr[dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn] = DBNull.Value;
                }
                if (model.PrescriptionLastUpdateDate != "" && WhichAttrUpdate == "Prescription")
                {
                    dr.PrescriptionLastUpdateDate = MDVUtility.ToDateTime(model.PrescriptionLastUpdateDate);
                }
                else
                {
                    dr[dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn] = DBNull.Value;
                }
                dr.WhichAttrUpdate = WhichAttrUpdate;
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(sharedVariable == null ? MDVSession.Current.AppUserName : sharedVariable.UserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(sharedVariable == null ? MDVSession.Current.AppUserName : sharedVariable.UserName);
                dr.ModifiedOn = DateTime.Now;
                dsRcopia.Rcopia_PatientLastUpdateInfo.AddRcopia_PatientLastUpdateInfoRow(dr);
                BLObject<DSRcopia> obj = BLLRcopiaObj.UpdatePtientLastUpdateDate(dsRcopia, sharedVariable);
                dsRcopia = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = obj.Message
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
        public string DownloadAllergiesAndSave(RcopiaModel model, SharedVariable sharedVariable = null, string UserName = null, Int64 UserId=0)
        {
            try
            {
                Patient_Demographic objpatient = new Patient_Demographic();

                List<AllergyModel> DownloadAllergy = new List<AllergyModel>();
                DownloadAllergy = GetDownloadRcopiaResponseUrl(model, sharedVariable, UserId);
                bool successfullySave = true;
                string messageFromSave = "";
                string SavedAllergyIds = "";
                int DownloadAllergiesCount = 0;//localy use
                if (DownloadAllergy.Count == 1 && DownloadAllergy[0].RcopiaID == "0")
                {
                    DownloadAllergiesCount = 0;
                }
                else
                {
                    DownloadAllergiesCount = DownloadAllergy.Count;
                    AllergyHelper helperAllergy = new AllergyHelper();

                    dynamic response = JObject.Parse(helperAllergy.saveAllergy(DownloadAllergy, sharedVariable, UserName));


                    if (response.status == true)
                    {
                        //if (MDVUtility.ToInt64(model.NotesId) > 0)
                        //{
                        //    BLObject<DSAllergies> obj = new BLLClinical().attachAllergiesWithNotes(MDVUtility.ToStr(response.SavedAllergyIds), MDVUtility.ToInt64(model.NotesId), sharedVariable);
                        //    if (obj.Data != null)
                        //    {
                        //        messageFromSave = response.Message;
                        //        successfullySave = true;
                        //        SavedAllergyIds = response.SavedAllergyIds;
                        //    }
                        //    else
                        //    {
                        //        messageFromSave = "Problem in Saving allergy from DrFirst";
                        //        successfullySave = false;
                        //    }
                        //}
                        //else
                        //{
                        messageFromSave = response.Message;
                        successfullySave = true;
                        SavedAllergyIds = response.SavedAllergyIds;
                        //}

                    }
                    else
                    {
                        messageFromSave = "Problem in Saving allergy from DrFirst";
                        //messageFromSave = response.Message;
                        successfullySave = false;
                    }

                }
                var respons1 = new
                {
                    MessageFromSave = messageFromSave,
                    status = successfullySave,
                    lastUpdateDate = DownloadAllergy[0].LastUpdateDate,
                    DownloadAllergiesCount = DownloadAllergiesCount,
                    SavedAllergyIds = SavedAllergyIds

                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(respons1));
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
        public string DownloadMadicationsAndSave(RcopiaModel model, string MedicationLastUpdateDateForLIMP = "", SharedVariable sharedVariable = null, string UserName = null, string OrderSetId = "", Int64 UserId=0)
        {
            try
            {
                List<MedicationModel> sl = new List<MedicationModel>();
                List<object> ol = sl.Cast<object>().ToList();
                List<MedicationModel> DownloadMedication = new List<MedicationModel>();
                DownloadMedication = GetDownloadMedicationRcopiaResponseUrl(model, MedicationLastUpdateDateForLIMP, sharedVariable, UserId);//DownloadMedicationsList(model, MedicationLastUpdateDateForLIMP);
                bool successfullySave = true;
                string messageFromSave = "";
                var SavedMedicationIds = "";
                int DownloadMedicationCount = 0;//localy use
                if (DownloadMedication.Count == 1 && DownloadMedication[0].RcopiaID == "0")
                {
                    DownloadMedicationCount = 0;
                }
                else
                {
                    DownloadMedicationCount = DownloadMedication.Count;
                    MedicationsHelper helperMedication = new MedicationsHelper();

                    dynamic response = JObject.Parse(helperMedication.SaveMedication(DownloadMedication, false, sharedVariable, UserName, OrderSetId));
                    if (response.status == true)
                    {
                        //if (MDVUtility.ToInt64(model.NotesId) > 0)
                        //{
                        //    BLObject<DSClinicalMedication> obj = new BLLClinical().attachMedicationsWithNotes(MDVUtility.ToStr(response.SavedMedicationIds), MDVUtility.ToInt64(model.NotesId), sharedVariable);
                        //    if (obj.Data != null)
                        //    {
                        //        messageFromSave = response.Message;
                        //        successfullySave = true;
                        //        SavedMedicationIds = response.SavedMedicationIds;
                        //    }
                        //    else
                        //    {
                        //        messageFromSave = "Problem in Saving medication from DrFirst";
                        //        successfullySave = false;
                        //    }
                        //}
                        //else
                        //{
                        messageFromSave = response.Message;
                        SavedMedicationIds = response.SavedMedicationIds;
                        successfullySave = true;
                        //}
                    }
                    else
                    {
                        messageFromSave = "Problem in Saving medication from DrFirst";
                        successfullySave = false;
                    }

                }
                var respons1 = new
                {
                    MessageFromSave = messageFromSave,
                    status = successfullySave,
                    SavedMedicationIds = SavedMedicationIds,
                    lastUpdateDate = DownloadMedication[0].LastUpdateDate,
                    DownloadMedicationCount = DownloadMedicationCount

                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(respons1));
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
        public List<PrescriptionsModel> GetDownloadPrescriptionRcopiaResponseUrl(RcopiaModel model, string MedicationLastUpdateDateForLIMP, SharedVariable sharedVariable = null, Int64 UserId = 0)
        {
            List<PrescriptionsModel> Prescription = new List<PrescriptionsModel>();
            int count = 0;
            int ANS1count = 0;
            int ANS2count = 0;

            string Errorresponse = string.Empty;

            for (int i = 0; i < 3; i++)
            {

                count++;
                Prescription = DownloadPrescriptionsList(model, count, MedicationLastUpdateDateForLIMP, sharedVariable,UserId);
                string status = Prescription[0].Status;
                if (Prescription.Any() && status != "One or more errors occurred.")
                {
                    break;
                }
                else
                {
                    MDVLogger.RcopiaLogMessage("Error: Download_Prescription", "", "", "", status, count, true,UserId);
                }
                int milliseconds = 30000;
                Thread.Sleep(milliseconds);
            }
            if (count > 2)
            {
                for (int j = 0; j < 3; j++)
                {
                    ANS1count++;
                    Prescription = DownloadPrescriptionsListANS1(model, ANS1count, MedicationLastUpdateDateForLIMP, sharedVariable,UserId);
                    string status = Prescription[0].Status;
                    if (Errorresponse == "DrFirst ANS cannot be called within 10 minutes,Please retry later" || status == "10 minutes")
                    {
                        // MDVLogger.RcopiaLogMessage("Error", "", "", "Using ANS1", Prescription.ToString(), ANS1count);
                        break;
                    }
                    else
                    {
                        MDVLogger.RcopiaLogMessage("Exception on getting latest URL", "", "", "Using ANS1", status, ANS1count, true,UserId);

                    }
                    int milliseconds = 30000;
                    Thread.Sleep(milliseconds);
                }
            }
            if (ANS1count > 2)
            {

                Prescription = DownloadPrescriptionsListANS2(model, ANS1count, MedicationLastUpdateDateForLIMP, sharedVariable,UserId);
                string status = Prescription[0].Status;
                if (!Prescription.Any())
                {
                    //Do nothing;
                }
                else
                {
                    MDVLogger.RcopiaLogMessage("Exception on getting latest URL", "", "", "Using ANS2", status, 0, true,UserId);
                }


            }
            return Prescription;
        }
        public List<AllergyModel> GetDownloadRcopiaResponseUrl(RcopiaModel model, SharedVariable sharedVariable = null, Int64 UserId=0)
        {
            List<AllergyModel> allergys = new List<AllergyModel>();
            int count = 0;
            int ANS1count = 0;
            int ANS2count = 0;

            string Errorresponse = string.Empty;

            for (int i = 0; i < 3; i++)
            {

                count++;
                allergys = DownloadAllergiesList(model, count, sharedVariable, UserId);
                string status = allergys[0].IsActive;
                if (allergys.Any() && status != "One or more errors occurred.")
                {
                    break;
                }
                else
                {
                    MDVLogger.RcopiaLogMessage("Error: Download_Allergy", "", "", "", status, count, true,UserId);
                }
                int milliseconds = 30000;
                Thread.Sleep(milliseconds);
            }
            if (count > 2)
            {
                for (int j = 0; j < 3; j++)
                {
                    ANS1count++;
                    allergys = DownloadAllergiesListANS1(model, ANS1count, sharedVariable, UserId);
                    string status = allergys[0].IsActive;
                    if (Errorresponse == "DrFirst ANS cannot be called within 10 minutes,Please retry later" || status == "10 minutes")
                    {
                        //MDVLogger.RcopiaLogMessage("Error", "", "", "Using ANS1", Errorresponse, ANS1count);
                        break;
                    }
                    else
                    {
                        MDVLogger.RcopiaLogMessage("Exception on getting latest URL", "", "", "Using ANS1", status, ANS1count, true,UserId);

                    }
                    int milliseconds = 30000;
                    Thread.Sleep(milliseconds);
                }
            }
            if (ANS1count > 2)
            {

                allergys = DownloadAllergiesListANS2(model, ANS1count, sharedVariable, UserId);
                string status = allergys[0].IsActive;
                if (allergys.Any())
                {
                    //Do nothing;
                }
                else
                {
                    MDVLogger.RcopiaLogMessage("Exception on getting latest URL", "", "", "Using ANS2", status, 0, true,UserId);
                }

            }
            return allergys;
        }
        public List<AllergyModel> DownloadAllergiesListANS2(RcopiaModel model, int count, SharedVariable sharedVariable = null, Int64 UserId=0)
        {
            List<string> Errorresponse = new System.Collections.Generic.List<string>();
            List<AllergyModel> allergys = new List<AllergyModel>();
            try
            {

                DSRcopia dsRcopia = new DSRcopia();
                RcopiaModel modelRcopia = new RcopiaModel();
                RcopiaHelper helperRcopia = new RcopiaHelper();
                List<RcopiaModel> ListRcopia = helperRcopia.GetRcopiaInfo(sharedVariable);
                string RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;
                string error = string.Empty;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue("application/xml"));
                var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

                var ANS1url = RcopiaANSbackup + "?xml=" + inputdata;
                MDVLogger.RcopiaLogMessage("Request: GET URL from ANS2", "", "", ANS1url,null,0,true,UserId);
                HttpResponseMessage ResponseDownloadAllergy = client.GetAsync(ANS1url).Result;
                MDVLogger.RcopiaLogMessage("Response: GET URL  from ANS2", "", "", ResponseDownloadAllergy.ToString(),null,0,true,UserId);
                if (ResponseDownloadAllergy != null)
                {
                    var GetdataANS1 = ResponseDownloadAllergy.Content.ReadAsStringAsync().Result;
                    XmlDocument DocANS1 = new XmlDocument();
                    DocANS1.LoadXml(GetdataANS1);
                    XmlNodeList nodeListuploadurlANS1 = DocANS1.GetElementsByTagName("EngineUploadURL");
                    XmlNodeList nodelistDownloadurlANS1 = DocANS1.GetElementsByTagName("EngineDownloadURL");
                    XmlNodeList nodelistWebBrowserURLANS1 = DocANS1.GetElementsByTagName("WebBrowserURL");
                    string UploadUrlANS2 = string.Empty;
                    string downloadUrlANS2 = string.Empty;
                    string WebBrowserURLANS2 = string.Empty;
                    foreach (XmlNode node in nodelistWebBrowserURLANS1)
                    {
                        WebBrowserURLANS2 = node.InnerText;
                    }
                    foreach (XmlNode node in nodeListuploadurlANS1)
                    {
                        UploadUrlANS2 = node.InnerText;
                    }
                    foreach (XmlNode node in nodelistDownloadurlANS1)
                    {
                        downloadUrlANS2 = node.InnerText;
                    }




                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/xml"));
                    var DownloadAllergyXml = MDVUtility.GetXmlForDownloadAllergy(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, model.PatientId, model.AllergyLastUpdateDate);
                    var DownloadUrl = downloadUrlANS2 + "?xml=" + DownloadAllergyXml;

                    HttpResponseMessage ResponseData = client.GetAsync(DownloadUrl).Result;
                    var downloadAllergyData = ResponseData.Content.ReadAsStringAsync().Result;
                    MDVLogger.RcopiaLogMessage("Request: update_allergy", "", "", downloadAllergyData, "", count,true,UserId);
                    if (downloadAllergyData != string.Empty)
                    {

                        modelRcopia.URLID = 1;
                        modelRcopia.EngineDownloadURL = downloadUrlANS2;
                        modelRcopia.EngineUploadURL = UploadUrlANS2;
                        modelRcopia.WebBrowserURL = WebBrowserURLANS2;
                        modelRcopia.CreatedOn = DateTime.Now;
                        modelRcopia.ModifiedOn = DateTime.Now;
                        modelRcopia.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        modelRcopia.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dsRcopia = new DSRcopia();
                        DSRcopia.Rcopia_GetUrlRow dr = dsRcopia.Rcopia_GetUrl.NewRcopia_GetUrlRow();
                        dr.UrlID = modelRcopia.URLID;
                        dr.EngineDownloadURL = modelRcopia.EngineDownloadURL;
                        dr.EngineUploadURL = modelRcopia.EngineUploadURL;
                        dr.WebBrowserURL = modelRcopia.WebBrowserURL;
                        dr.IsActive = true;
                        dr.CreatedOn = modelRcopia.CreatedOn;
                        dr.ModifiedOn = modelRcopia.ModifiedOn;
                        dr.CreatedBy = modelRcopia.CreatedBy;
                        dr.ModifiedBy = modelRcopia.ModifiedBy;
                        dsRcopia.Rcopia_GetUrl.AddRcopia_GetUrlRow(dr);

                        BLObject<DSRcopia> objMofieddate = BLLRcopiaObj.UpDateGetUrl(dsRcopia, sharedVariable);

                        XmlDocument doc1 = new XmlDocument();
                        doc1.LoadXml(downloadAllergyData);

                        XmlNodeList nodeListError = doc1.GetElementsByTagName("Error");
                        string ErrorText = "";
                        foreach (XmlNode node in nodeListError)
                        {
                            ErrorText = node.SelectSingleNode("Text").InnerText;
                            MDVLogger.RcopiaLogMessage("Response: Download_Allergies", model.PatientId, "error", downloadAllergyData, ErrorText,0,true,UserId);
                            return allergys;
                        }

                        XmlNodeList nodes = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response/AllergyList/Allergy");
                        string LastUpdateDate = "";
                        XmlNodeList nodesLastUpdateDate = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response");
                        foreach (XmlNode nodeUpdate in nodesLastUpdateDate)
                        {
                            LastUpdateDate = nodeUpdate.SelectSingleNode("LastUpdateDate").InnerText;
                        }
                        foreach (XmlNode node in nodes)
                        {
                            AllergyModel allergy = new AllergyModel();
                            allergy.RcopiaID = node.SelectSingleNode("RcopiaID") != null ? node.SelectSingleNode("RcopiaID").InnerText : "";
                            allergy.AllergyId = node.SelectSingleNode("ExternalID") != null ? node.SelectSingleNode("ExternalID").InnerText : "";
                            allergy.IsDeleted = node.SelectSingleNode("Deleted") != null ? node.SelectSingleNode("Deleted").InnerText : "";
                            allergy.IsActive = node.SelectSingleNode("Status").InnerXml;
                            XmlNode pat = node.SelectSingleNode("Patient");

                            if (pat != null)
                            {
                                allergy.PatientId = pat.SelectSingleNode("ExternalID").InnerText;
                            }
                            XmlNode alergen = node.SelectSingleNode("Allergen");

                            if (alergen != null)
                            {
                                allergy.Allergen = alergen.SelectSingleNode("Name").InnerText;
                                XmlNode Drug = alergen.SelectSingleNode("Drug");

                                if (Drug != null)
                                {
                                    allergy.RxnormID = Drug.SelectSingleNode("RxnormID") != null ? Drug.SelectSingleNode("RxnormID").InnerText : "";
                                    allergy.RxnormIDType = Drug.SelectSingleNode("RxnormIDType") != null ? Drug.SelectSingleNode("RxnormIDType").InnerText : "";
                                }
                            }
                            allergy.Reaction = node.SelectSingleNode("Reaction") != null ? node.SelectSingleNode("Reaction").InnerText : "";
                            allergy.OnSetDate = node.SelectSingleNode("OnsetDate") != null ? MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("OnsetDate").InnerText) : "";
                            allergy.LastModifiedDate = node.SelectSingleNode("LastModifiedDate").InnerText;
                            allergy.LastModifiedBy = node.SelectSingleNode("LastModifiedBy").InnerText;
                            allergy.LastUpdateDate = LastUpdateDate;
                            allergys.Add(allergy);
                            MDVLogger.RcopiaLogMessage("Response: Download_Allergy", "", allergy.IsActive, DownloadUrl, "", count,true,UserId);

                        }


                        if (allergys.Count == 0)
                        {
                            AllergyModel allergy = new AllergyModel();
                            allergy.LastUpdateDate = LastUpdateDate;
                            allergy.RcopiaID = "0";
                            allergys.Add(allergy);
                        }
                    }
                }

                return allergys;
            }
            catch (Exception ex)
            {
                AllergyModel objallergy = new AllergyModel();
                objallergy.IsActive = ex.Message;
                allergys.Add(objallergy);
                return allergys;
            }
        }
        public List<AllergyModel> DownloadAllergiesListANS1(RcopiaModel model, int count, SharedVariable sharedVariable = null, Int64 UserId = 0)
        {
            List<AllergyModel> allergys = new List<AllergyModel>();
            DateTime Modified;
            string Errorresponse = "DrFirst ANS cannot be called within 10 minutes,Please retry later";
            try
            {


                DSRcopia dsRcopia = new DSRcopia();
                RcopiaModel modelRcopia = new RcopiaModel();
                RcopiaHelper helperRcopia = new RcopiaHelper();
                List<RcopiaModel> ListRcopia = helperRcopia.GetRcopiaInfo(sharedVariable);
                string RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;

                BLObject<DSRcopia> obj = BLLRcopiaObj.SelectGetUrls(sharedVariable);
                dsRcopia = obj.Data;
                Modified = MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.ModifiedOnColumn.ColumnName]);
                int minutes = Convert.ToInt32(DateTime.Now.Subtract(Modified).TotalMinutes);
                if (minutes > 10)
                {
                    BLObject<DSRcopia> objRcopiaAns = BLLRcopiaObj.SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode, sharedVariable);
                    HttpClient client = new HttpClient();
                    dsRcopia = objRcopiaAns.Data;
                    modelRcopia.RcopiaANS = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaANSColumn.ColumnName]);
                    string RcopiaANS = modelRcopia.RcopiaANS;
                    string error = string.Empty;
                    client.DefaultRequestHeaders.Accept.Add(
                           new MediaTypeWithQualityHeaderValue("application/xml"));
                    var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

                    var ANS1url = RcopiaANS + "?xml=" + inputdata;
                    HttpResponseMessage ResponseDownloadAllergy = client.GetAsync(ANS1url).Result;

                    if (ResponseDownloadAllergy != null)
                    {
                        var GetdataANS1 = ResponseDownloadAllergy.Content.ReadAsStringAsync().Result;
                        XmlDocument DocANS1 = new XmlDocument();
                        DocANS1.LoadXml(GetdataANS1);
                        XmlNodeList nodeListuploadurlANS1 = DocANS1.GetElementsByTagName("EngineUploadURL");
                        XmlNodeList nodelistDownloadurlANS1 = DocANS1.GetElementsByTagName("EngineDownloadURL");
                        XmlNodeList nodelistWebBrowserURLANS1 = DocANS1.GetElementsByTagName("WebBrowserURL");
                        string UploadUrlANS1 = string.Empty;
                        string downloadUrlANS1 = string.Empty;
                        string WebBrowserURLANS1 = string.Empty;
                        foreach (XmlNode node in nodelistWebBrowserURLANS1)
                        {
                            WebBrowserURLANS1 = node.InnerText;
                        }
                        foreach (XmlNode node in nodeListuploadurlANS1)
                        {
                            UploadUrlANS1 = node.InnerText;
                        }
                        foreach (XmlNode node in nodelistDownloadurlANS1)
                        {
                            downloadUrlANS1 = node.InnerText;
                        }

                        //model.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);

                        client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/xml"));
                        var DownloadAllergyXml = MDVUtility.GetXmlForDownloadAllergy(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, model.PatientId, model.AllergyLastUpdateDate);
                        var DownloadUrl = downloadUrlANS1 + "?xml=" + DownloadAllergyXml;

                        MDVLogger.RcopiaLogMessage("Request: Download_Allergy", "", "", downloadUrlANS1, "", count,true,UserId);
                        HttpResponseMessage ResponseData = client.GetAsync(DownloadUrl).Result;
                        var downloadAllergyData = ResponseData.Content.ReadAsStringAsync().Result;
                        if (downloadAllergyData != string.Empty)
                        {
                            model.URLID = 1;
                            model.EngineDownloadURL = downloadUrlANS1;
                            model.EngineUploadURL = UploadUrlANS1;
                            model.WebBrowserURL = WebBrowserURLANS1;
                            model.CreatedOn = DateTime.Now;
                            model.ModifiedOn = DateTime.Now;
                            model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dsRcopia = new DSRcopia();
                            DSRcopia.Rcopia_GetUrlRow dr = dsRcopia.Rcopia_GetUrl.NewRcopia_GetUrlRow();
                            dr.UrlID = model.URLID;
                            dr.EngineDownloadURL = model.EngineDownloadURL;
                            dr.EngineUploadURL = model.EngineUploadURL;
                            dr.WebBrowserURL = model.WebBrowserURL;
                            dr.IsActive = true;
                            dr.CreatedOn = model.CreatedOn;
                            dr.ModifiedOn = model.ModifiedOn;
                            dr.CreatedBy = model.CreatedBy;
                            dr.ModifiedBy = model.ModifiedBy;
                            dsRcopia.Rcopia_GetUrl.AddRcopia_GetUrlRow(dr);

                            BLObject<DSRcopia> objMofieddate = BLLRcopiaObj.UpDateGetUrl(dsRcopia, sharedVariable);



                            XmlDocument doc1 = new XmlDocument();
                            doc1.LoadXml(downloadAllergyData);

                            XmlNodeList nodeListError = doc1.GetElementsByTagName("Error");
                            string ErrorText = "";
                            Errorresponse = "";
                            foreach (XmlNode node in nodeListError)
                            {
                                ErrorText = node.SelectSingleNode("Text").InnerText;
                                MDVLogger.RcopiaLogMessage("Response: Download_Allergies", model.PatientId, "error", downloadAllergyData, ErrorText,0,true,UserId);
                                return allergys;
                            }

                            XmlNodeList nodes = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response/AllergyList/Allergy");
                            string LastUpdateDate = "";
                            XmlNodeList nodesLastUpdateDate = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response");
                            foreach (XmlNode nodeUpdate in nodesLastUpdateDate)
                            {
                                LastUpdateDate = nodeUpdate.SelectSingleNode("LastUpdateDate").InnerText;
                            }
                            foreach (XmlNode node in nodes)
                            {
                                AllergyModel allergy = new AllergyModel();
                                allergy.RcopiaID = node.SelectSingleNode("RcopiaID") != null ? node.SelectSingleNode("RcopiaID").InnerText : "";
                                allergy.AllergyId = node.SelectSingleNode("ExternalID") != null ? node.SelectSingleNode("ExternalID").InnerText : "";
                                allergy.IsDeleted = node.SelectSingleNode("Deleted") != null ? node.SelectSingleNode("Deleted").InnerText : "";
                                allergy.IsActive = node.SelectSingleNode("Status").InnerXml;
                                XmlNode pat = node.SelectSingleNode("Patient");

                                if (pat != null)
                                {
                                    allergy.PatientId = pat.SelectSingleNode("ExternalID").InnerText;
                                }
                                XmlNode alergen = node.SelectSingleNode("Allergen");

                                if (alergen != null)
                                {
                                    allergy.Allergen = alergen.SelectSingleNode("Name").InnerText;
                                    XmlNode Drug = alergen.SelectSingleNode("Drug");

                                    if (Drug != null)
                                    {
                                        allergy.RxnormID = Drug.SelectSingleNode("RxnormID") != null ? Drug.SelectSingleNode("RxnormID").InnerText : "";
                                        allergy.RxnormIDType = Drug.SelectSingleNode("RxnormIDType") != null ? Drug.SelectSingleNode("RxnormIDType").InnerText : "";
                                    }
                                }
                                allergy.Reaction = node.SelectSingleNode("Reaction") != null ? node.SelectSingleNode("Reaction").InnerText : "";
                                allergy.OnSetDate = node.SelectSingleNode("OnsetDate") != null ? MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("OnsetDate").InnerText) : "";
                                allergy.LastModifiedDate = node.SelectSingleNode("LastModifiedDate").InnerText;
                                allergy.LastModifiedBy = node.SelectSingleNode("LastModifiedBy").InnerText;
                                allergy.LastUpdateDate = LastUpdateDate;
                                allergys.Add(allergy);
                                MDVLogger.RcopiaLogMessage("Response: Download_Allergy", "", allergy.IsActive, DownloadUrl, "", count,true,UserId);

                            }



                            if (allergys.Count == 0)
                            {
                                AllergyModel allergy = new AllergyModel();
                                allergy.LastUpdateDate = LastUpdateDate;
                                allergy.RcopiaID = "0";
                                allergys.Add(allergy);
                            }
                        }
                    }
                }
                if (Errorresponse != string.Empty)
                {
                    MDVLogger.RcopiaLogMessage("Response: Download_Allergy", "", "", Errorresponse, "", count,true,UserId);
                    AllergyModel allergies = new AllergyModel();
                    allergies.IsActive = "10 minutes";
                    allergys.Add(allergies);
                }
                return allergys;
            }
            catch (Exception ex)
            {
                AllergyModel allergies = new AllergyModel();
                allergies.IsActive = ex.Message;
                allergys.Add(allergies);
                return allergys;
            }
        }
        public List<AllergyModel> DownloadAllergiesList(RcopiaModel model, int count, SharedVariable sharedVariable = null, Int64 UserId = 0)
        {
            List<AllergyModel> allergys = new List<AllergyModel>();
            string errormessage = string.Empty;
            try
            {

                DSRcopia dsRcopia = new DSRcopia();
                RcopiaModel modelRcopia = new RcopiaModel();
                RcopiaHelper helperRcopia = new RcopiaHelper();

                string RcopiaANSbackup = model.RcopiaANSbackup;
                string RcopiaScretkey = model.RcopiaScretkey;
                string RcopiaVendorUsername = model.RcopiaVendorUsername;
                string RcopiaVendorPassword = model.RcopiaVendorPassword;
                string RcopiaPortalSystemName = model.RcopiaPortalSystemName;
                string RcopiaPracticeUserName = model.RcopiaPracticeUserName;


                modelRcopia.EngineDownloadURL = model.EngineDownloadURL;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));
                var DownloadAllergyXml = MDVUtility.GetXmlForDownloadAllergy(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, model.PatientId, model.AllergyLastUpdateDate);
                var DownloadUrl = modelRcopia.EngineDownloadURL + "?xml=" + DownloadAllergyXml;
                MDVLogger.RcopiaLogMessage("Request: Download_Allergy", "", "", DownloadUrl, "", count,true,UserId);
                HttpResponseMessage ResponseData = client.GetAsync(DownloadUrl).Result;
                var downloadAllergyData = ResponseData.Content.ReadAsStringAsync().Result;
                if (downloadAllergyData != string.Empty)
                {
                    XmlDocument doc1 = new XmlDocument();
                    doc1.LoadXml(downloadAllergyData);

                    XmlNodeList nodeListError = doc1.GetElementsByTagName("Error");
                    string ErrorText = "";
                    foreach (XmlNode node in nodeListError)
                    {
                        ErrorText = node.SelectSingleNode("Text").InnerText;
                        MDVLogger.RcopiaLogMessage("Response: Download_Allergies", model.PatientId, "error", downloadAllergyData, ErrorText,0,true,UserId);
                        return allergys;
                    }

                    XmlNodeList nodes = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response/AllergyList/Allergy");
                    string LastUpdateDate = "";
                    XmlNodeList nodesLastUpdateDate = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response");
                    foreach (XmlNode nodeUpdate in nodesLastUpdateDate)
                    {
                        LastUpdateDate = nodeUpdate.SelectSingleNode("LastUpdateDate").InnerText;
                    }
                    foreach (XmlNode node in nodes)
                    {
                        AllergyModel allergy = new AllergyModel();
                        allergy.RcopiaID = node.SelectSingleNode("RcopiaID") != null ? node.SelectSingleNode("RcopiaID").InnerText : "";
                        allergy.AllergyId = node.SelectSingleNode("ExternalID") != null ? node.SelectSingleNode("ExternalID").InnerText : "";
                        allergy.IsDeleted = node.SelectSingleNode("Deleted") != null ? node.SelectSingleNode("Deleted").InnerText : "";
                        allergy.IsActive = node.SelectSingleNode("Status").InnerXml;
                        XmlNode pat = node.SelectSingleNode("Patient");

                        if (pat != null)
                        {
                            allergy.PatientId = pat.SelectSingleNode("ExternalID").InnerText;
                        }
                        XmlNode alergen = node.SelectSingleNode("Allergen");

                        if (alergen != null)
                        {
                            allergy.Allergen = alergen.SelectSingleNode("Name").InnerText;
                            XmlNode Drug = alergen.SelectSingleNode("Drug");

                            if (Drug != null)
                            {
                                allergy.RxnormID = Drug.SelectSingleNode("RxnormID") != null ? Drug.SelectSingleNode("RxnormID").InnerText : "";
                                allergy.RxnormIDType = Drug.SelectSingleNode("RxnormIDType") != null ? Drug.SelectSingleNode("RxnormIDType").InnerText : "";
                            }
                        }
                        allergy.Reaction = node.SelectSingleNode("Reaction") != null ? node.SelectSingleNode("Reaction").InnerText : "";
                        allergy.OnSetDate = node.SelectSingleNode("OnsetDate") != null ? MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("OnsetDate").InnerText) : "";
                        allergy.LastModifiedDate = node.SelectSingleNode("LastModifiedDate").InnerText;
                        allergy.LastModifiedBy = node.SelectSingleNode("LastModifiedBy").InnerText;
                        allergy.LastUpdateDate = LastUpdateDate;
                        allergys.Add(allergy);
                        MDVLogger.RcopiaLogMessage("Response: Download_Allergy", "", allergy.IsActive, DownloadUrl, "", count, true,UserId);

                    }


                    if (allergys.Count == 0)
                    {
                        AllergyModel allergy = new AllergyModel();
                        allergy.LastUpdateDate = LastUpdateDate;
                        allergy.RcopiaID = "0";
                        allergys.Add(allergy);
                    }
                }
                MDVLogger.RcopiaLogMessage("Response: Download_Allergy", "", "", downloadAllergyData, "", count, true,UserId);
                return allergys;
            }
            catch (Exception ex)
            {
                AllergyModel objAllergy = new AllergyModel();
                objAllergy.IsActive = ex.Message;
                allergys.Add(objAllergy);
                return allergys;
            }


        }
        public List<MedicationModel> GetDownloadMedicationRcopiaResponseUrl(RcopiaModel model, string MedicationLastUpdateDateForLIMP, SharedVariable sharedVariable = null, Int64 UserId=0)
        {
            List<MedicationModel> Medication = new List<MedicationModel>();
            int count = 0;
            int ANS1count = 0;
            int ANS2count = 0;

            string Errorresponse = string.Empty;

            for (int i = 0; i < 3; i++)
            {

                count++;
                Medication = DownloadMedicationsList(model, count, MedicationLastUpdateDateForLIMP, sharedVariable, UserId);
                string status = Medication[0].Status;
                if (Medication.Any() && status != "One or more errors occurred.")
                {
                    break;
                }
                else
                {
                    MDVLogger.RcopiaLogMessage("Error: Download_Medication", "", "", "", status, count, true,UserId);
                }
                int milliseconds = 30000;
                Thread.Sleep(milliseconds);
            }
            if (count > 2)
            {
                for (int j = 0; j < 3; j++)
                {
                    ANS1count++;
                    Medication = DownloadMedicationsListANS1(model, ANS1count, MedicationLastUpdateDateForLIMP, sharedVariable, UserId);
                    string status = Medication[0].Status;
                    if (Errorresponse == "DrFirst ANS cannot be called within 10 minutes,Please retry later" || status == "10 minutes")
                    {
                        // MDVLogger.RcopiaLogMessage("Error", "", "", "Using ANS1",  Medication.ToString(), ANS1count);
                        break;
                    }
                    else
                    {
                        MDVLogger.RcopiaLogMessage("Exception on getting latest URL", "", "", "Using ANS1", status, ANS1count, true,UserId);

                    }
                    int milliseconds = 30000;
                    Thread.Sleep(milliseconds);
                }
            }
            if (ANS1count > 2)
            {

                Medication = DownloadMedicationsListANS2(model, ANS1count, MedicationLastUpdateDateForLIMP, sharedVariable, UserId);
                string status = Medication[0].Status;
                if (!Medication.Any())
                {
                    //Do nothing;
                }
                else
                {
                    MDVLogger.RcopiaLogMessage("Exception on getting latest URL", "", "", "Using ANS2", status, 0, true,UserId);
                }

            }
            return Medication;
        }

        public List<MedicationModel> DownloadMedicationsList(RcopiaModel model, int count, string MedicationLastUpdateDateForLIMP = "", SharedVariable sharedVariable = null, Int64 UserId=0)
        {
            List<MedicationModel> Medications = new List<MedicationModel>();
            try
            {
                Patient_Demographic objpatient = new Patient_Demographic();
                DSRcopia dsRcopia = new DSRcopia();
                RcopiaModel modelRcopia = new RcopiaModel();
                RcopiaHelper helperRcopia = new RcopiaHelper();

                string RcopiaANSbackup = model.RcopiaANSbackup;
                string RcopiaScretkey = model.RcopiaScretkey;
                string RcopiaVendorUsername = model.RcopiaVendorUsername;
                string RcopiaVendorPassword = model.RcopiaVendorPassword;
                string RcopiaPortalSystemName = model.RcopiaPortalSystemName;
                string RcopiaPracticeUserName = model.RcopiaPracticeUserName;


                modelRcopia.EngineDownloadURL = model.EngineDownloadURL;

                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(4);
                //client.BaseAddress = new Uri("http://localhost:8080/");
                string error = string.Empty;
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));
                //BLObject<DSRcopia> obj =BLLRcopiaObj.SelectGetUrls();
                //dsRcopia = obj.Data;
                //modelRcopia.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);
                //modelRcopia.EngineUploadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineUploadURLColumn.ColumnName]);
                string UploadUrl = modelRcopia.EngineUploadURL;
                string DownloadURL = modelRcopia.EngineDownloadURL;
                var upload = string.Empty;

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));


                var DownloadMedicationXml = MDVUtility.GetXmlForDownloadMedication(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, model.PatientId, model.MedicationLastUpdateDate, MedicationLastUpdateDateForLIMP);
                var DownloadUrl = modelRcopia.EngineDownloadURL + "?xml=" + DownloadMedicationXml;
                MDVLogger.RcopiaLogMessage("Request: update_Medication", "", "", DownloadMedicationXml, "", count, true,UserId);
                HttpResponseMessage ResponseData = client.GetAsync(DownloadUrl).Result;
                var downloadMedicationData = ResponseData.Content.ReadAsStringAsync().Result;
                if (downloadMedicationData != string.Empty)
                {
                    MDVLogger.RcopiaLogMessage("Response: update_Medication", "", "", downloadMedicationData, "", count, true,UserId);
                    XmlDocument doc1 = new XmlDocument();
                    doc1.LoadXml(downloadMedicationData);

                    XmlNodeList nodeListError = doc1.GetElementsByTagName("Error");
                    string ErrorText = "";
                    foreach (XmlNode node in nodeListError)
                    {
                        ErrorText = node.SelectSingleNode("Text").InnerText;
                        MDVLogger.RcopiaLogMessage("Response: Download_Allergies", model.PatientId, "error", downloadMedicationData, ErrorText, 0, true,UserId);
                        return Medications;
                    }

                    XmlNodeList nodes = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response/MedicationList/Medication");
                    string LastUpdateDate = "";
                    XmlNodeList nodesLastUpdateDate = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response");
                    foreach (XmlNode nodeUpdate in nodesLastUpdateDate)
                    {
                        LastUpdateDate = nodeUpdate.SelectSingleNode("LastUpdateDate").InnerText;
                    }
                    foreach (XmlNode node in nodes)
                    {
                        XmlNode Drug1 = node.SelectSingleNode("Sig/Drug");
                        var DrugIsFreeText = false;
                        if (Drug1 != null)
                        {

                            var DrugNDCID = Drug1.SelectSingleNode("NDCID") != null ? Drug1.SelectSingleNode("NDCID").InnerText : "";
                            var DrugRcopiaID = Drug1.SelectSingleNode("RcopiaID").InnerText;
                            if (DrugNDCID == "" && DrugRcopiaID == "")
                            {
                                DrugIsFreeText = true;
                            }
                        }
                        if (!DrugIsFreeText)
                        {
                            MedicationModel medication = new MedicationModel();
                            medication.IsDeleted = node.SelectSingleNode("Deleted").InnerText;

                            medication.RcopiaID = node.SelectSingleNode("RcopiaID").InnerText;

                            XmlNode prescription = node.SelectSingleNode("Prescription");

                            if (prescription != null)
                            {
                                medication.PrescriptionRcopiaID = prescription.SelectSingleNode("RcopiaID").InnerText;
                            }
                            XmlNode patient = node.SelectSingleNode("Patient");

                            if (patient != null)
                            {
                                medication.PatientID = MDVUtility.ToLong(patient.SelectSingleNode("ExternalID").InnerText);
                            }

                            XmlNode provider = node.SelectSingleNode("Provider");

                            if (provider != null)
                            {
                                //medication.ProviderID = MDVUtility.ToLong(provider.SelectSingleNode("ExternalID").InnerText);//use in future
                                medication.ProviderRcopiaID = provider.SelectSingleNode("RcopiaID").InnerText;
                                medication.RcopiaUserName = provider.SelectSingleNode("Username").InnerText;
                                medication.ProviderNPI = provider.SelectSingleNode("NPI").InnerText;
                            }

                            XmlNode preparer = node.SelectSingleNode("Preparer");

                            if (preparer != null)
                            {
                                //medication.Preparer_UserID = MDVUtility.ToLong(preparer.SelectSingleNode("ExternalID").InnerText);//use in future
                                medication.PreparerRcopiaID = provider.SelectSingleNode("RcopiaID").InnerText;
                            }

                            medication.DrugCodingLevel = node.SelectSingleNode("DrugCodingLevel") != null ? node.SelectSingleNode("DrugCodingLevel").InnerText : "";

                            XmlNode Drug = node.SelectSingleNode("Sig/Drug");

                            if (Drug != null)
                            {
                                DrugModel drugModel = new DrugModel();
                                drugModel.NDCID = Drug.SelectSingleNode("NDCID") != null ? Drug.SelectSingleNode("NDCID").InnerText : "";
                                drugModel.FirstDataBankMedID = Drug.SelectSingleNode("FirstDataBankMedID") != null ? Drug.SelectSingleNode("FirstDataBankMedID").InnerText : "";
                                drugModel.RcopiaID = Drug.SelectSingleNode("RcopiaID").InnerText;
                                XmlNode rxNorm = Drug.SelectSingleNode("RxnormID");
                                if (rxNorm != null)
                                {
                                    drugModel.RxnormID = Drug.SelectSingleNode("RxnormID") != null ? Drug.SelectSingleNode("RxnormID").InnerText : "";
                                    drugModel.RxnormIDType = Drug.SelectSingleNode("RxnormIDType") != null ? Drug.SelectSingleNode("RxnormIDType").InnerText : "";
                                }


                                drugModel.BrandName = Drug.SelectSingleNode("BrandName").InnerText;
                                drugModel.GenericName = Drug.SelectSingleNode("GenericName").InnerText;
                                drugModel.Schedule = Drug.SelectSingleNode("Schedule") != null ? Drug.SelectSingleNode("Schedule").InnerText : "";

                                drugModel.BrandType = Drug.SelectSingleNode("BrandType") != null ? Drug.SelectSingleNode("BrandType").InnerText : "";
                                drugModel.LegendStatus = Drug.SelectSingleNode("LegendStatus") != null ? Drug.SelectSingleNode("LegendStatus").InnerText : "";
                                drugModel.Route = Drug.SelectSingleNode("Route").InnerText;
                                drugModel.Form = Drug.SelectSingleNode("Form") != null ? Drug.SelectSingleNode("Form").InnerText : "";
                                drugModel.Strength = Drug.SelectSingleNode("Strength") != null ? Drug.SelectSingleNode("Strength").InnerText : "";
                                medication.drugModel = drugModel;

                            }
                            medication.Action = node.SelectSingleNode("Sig/Action") != null ? node.SelectSingleNode("Sig/Action").InnerText : "";
                            medication.Dose = node.SelectSingleNode("Sig/Dose") != null ? node.SelectSingleNode("Sig/Dose").InnerText : "";
                            medication.DoseUnit = node.SelectSingleNode("Sig/DoseUnit") != null ? node.SelectSingleNode("Sig/DoseUnit").InnerText : "";
                            medication.Routeby = node.SelectSingleNode("Sig/Route") != null ? node.SelectSingleNode("Sig/Route").InnerText : "";
                            medication.DoseTiming = node.SelectSingleNode("Sig/DoseTiming") != null ? node.SelectSingleNode("Sig/DoseTiming").InnerText : "";
                            medication.DoseOther = node.SelectSingleNode("Sig/DoseOther") != null ? node.SelectSingleNode("Sig/DoseOther").InnerText : "";
                            medication.Duration = node.SelectSingleNode("Sig/Duration") != null ? MDVUtility.ToInt(node.SelectSingleNode("Sig/Duration").InnerText) : 0;
                            medication.Quantity = node.SelectSingleNode("Sig/Quantity").InnerText;
                            medication.QuantityUnit = node.SelectSingleNode("Sig/QuantityUnit").InnerText;
                            medication.Refill = node.SelectSingleNode("Sig/Refills") != null ? node.SelectSingleNode("Sig/Refills").InnerText : "";
                            medication.Substitution = node.SelectSingleNode("Sig/SubstitutionPermitted").InnerText;
                            medication.PatientNotes = node.SelectSingleNode("Sig/PatientNotes").InnerText;
                            if (node.SelectSingleNode("StartDate").InnerText != "")
                            {
                                medication.StartDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("StartDate").InnerText));
                            }
                            if (node.SelectSingleNode("StopDate").InnerText != "")
                            {
                                medication.StopDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("StopDate").InnerText));
                            }
                            if (node.SelectSingleNode("FillDate").InnerText != "")
                            {
                                medication.FillDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("FillDate").InnerText));
                            }


                            medication.StopReason = node.SelectSingleNode("StopReason").InnerText;
                            if (node.SelectSingleNode("SigChangedDate").InnerText != "")
                            {
                                medication.SigChangedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("SigChangedDate").InnerText));
                            }
                            if (node.SelectSingleNode("LastModifiedDate").InnerText != "")
                            {
                                medication.LastModifiedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("LastModifiedDate").InnerText));
                            }
                            medication.LastModifiedBy = node.SelectSingleNode("LastModifiedBy").InnerText;


                            medication.LastUpdateDate = LastUpdateDate;
                            Medications.Add(medication);
                        }


                    }


                    if (Medications.Count == 0)
                    {
                        MedicationModel medication = new MedicationModel();
                        medication.LastUpdateDate = LastUpdateDate;
                        medication.RcopiaID = "0";
                        Medications.Add(medication);
                    }
                }

                return Medications;
            }
            catch (Exception ex)
            {
                MedicationModel objmedication = new MedicationModel();
                objmedication.Status = ex.Message;
                Medications.Add(objmedication);
                return Medications;
            }


        }
        public List<MedicationModel> DownloadMedicationsListANS1(RcopiaModel model, int count, string MedicationLastUpdateDateForLIMP = "", SharedVariable sharedVariable = null, Int64 UserId = 0)
        {
            List<MedicationModel> Medications = new List<MedicationModel>();
            DateTime Modified;
            string Errorresponse = "DrFirst ANS cannot be called within 10 minutes,Please retry later";
            try
            {
                Patient_Demographic objpatient = new Patient_Demographic();
                DSRcopia dsRcopia = new DSRcopia();
                RcopiaModel modelRcopia = new RcopiaModel();
                RcopiaHelper helperRcopia = new RcopiaHelper();
                List<RcopiaModel> ListRcopia = helperRcopia.GetRcopiaInfo(sharedVariable);
                string RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;

                BLObject<DSRcopia> objGetUrl = BLLRcopiaObj.SelectGetUrls(sharedVariable);
                dsRcopia = objGetUrl.Data;
                Modified = MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.ModifiedOnColumn.ColumnName]);
                int minutes = Convert.ToInt32(DateTime.Now.Subtract(Modified).TotalMinutes);
                if (minutes > 10)
                {
                    BLObject<DSRcopia> obj = BLLRcopiaObj.SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode, sharedVariable);
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(4);
                    dsRcopia = obj.Data;
                    modelRcopia.RcopiaANS = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaANSColumn.ColumnName]);
                    string RcopiaANS = modelRcopia.RcopiaANS;

                    //client.BaseAddress = new Uri("http://localhost:8080/");
                    string error = string.Empty;
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/xml"));
                    var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";
                    MDVLogger.RcopiaLogMessage("ANS: update_Medication", "", "", RcopiaANS, "", count, true,UserId);
                    var ANS1url = RcopiaANS + "?xml=" + inputdata;
                    HttpResponseMessage ResponseDownloadAllergy = client.GetAsync(ANS1url).Result;


                    if (ResponseDownloadAllergy != null)
                    {




                        XmlDocument DocANS1 = new XmlDocument();
                        var GetdataANS1 = ResponseDownloadAllergy.Content.ReadAsStringAsync().Result;
                        DocANS1.LoadXml(GetdataANS1);
                        XmlNodeList nodeListuploadurlANS1 = DocANS1.GetElementsByTagName("EngineUploadURL");
                        XmlNodeList nodelistDownloadurlANS1 = DocANS1.GetElementsByTagName("EngineDownloadURL");
                        XmlNodeList nodelistWebBrowserURLANS1 = DocANS1.GetElementsByTagName("WebBrowserURL");
                        string UploadUrlANS1 = string.Empty;
                        string downloadUrlANS1 = string.Empty;
                        string WebBrowserURLANS1 = string.Empty;
                        foreach (XmlNode node in nodelistWebBrowserURLANS1)
                        {
                            WebBrowserURLANS1 = node.InnerText;
                        }
                        foreach (XmlNode node in nodeListuploadurlANS1)
                        {
                            UploadUrlANS1 = node.InnerText;
                        }
                        foreach (XmlNode node in nodelistDownloadurlANS1)
                        {
                            downloadUrlANS1 = node.InnerText;
                        }

                        var DownloadMedicationXml = MDVUtility.GetXmlForDownloadMedication(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, model.PatientId, model.MedicationLastUpdateDate, MedicationLastUpdateDateForLIMP);
                        var DownloadUrl = downloadUrlANS1 + "?xml=" + DownloadMedicationXml;
                        MDVLogger.RcopiaLogMessage("Request: update_allergy", "", "", DownloadUrl, "", count, true,UserId);

                        HttpResponseMessage ResponseData = client.GetAsync(DownloadUrl).Result;
                        var downloadMedicationData = ResponseData.Content.ReadAsStringAsync().Result;
                        MDVLogger.RcopiaLogMessage("Response: update_Medication", "", "", downloadMedicationData, "", count, true,UserId);
                        if (downloadMedicationData != string.Empty)
                        {

                            modelRcopia.URLID = 1;
                            modelRcopia.EngineDownloadURL = downloadUrlANS1;
                            modelRcopia.EngineUploadURL = UploadUrlANS1;
                            modelRcopia.WebBrowserURL = WebBrowserURLANS1;
                            modelRcopia.CreatedOn = DateTime.Now;
                            modelRcopia.ModifiedOn = DateTime.Now;
                            modelRcopia.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            modelRcopia.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dsRcopia = new DSRcopia();
                            DSRcopia.Rcopia_GetUrlRow dr = dsRcopia.Rcopia_GetUrl.NewRcopia_GetUrlRow();
                            dr.UrlID = modelRcopia.URLID;
                            dr.EngineDownloadURL = modelRcopia.EngineDownloadURL;
                            dr.EngineUploadURL = modelRcopia.EngineUploadURL;
                            dr.WebBrowserURL = modelRcopia.WebBrowserURL;
                            dr.IsActive = true;
                            dr.CreatedOn = modelRcopia.CreatedOn;
                            dr.ModifiedOn = modelRcopia.ModifiedOn;
                            dr.CreatedBy = modelRcopia.CreatedBy;
                            dr.ModifiedBy = modelRcopia.ModifiedBy;
                            dsRcopia.Rcopia_GetUrl.AddRcopia_GetUrlRow(dr);

                            BLObject<DSRcopia> objMofieddate = BLLRcopiaObj.UpDateGetUrl(dsRcopia, sharedVariable);




                            XmlDocument doc1 = new XmlDocument();
                            doc1.LoadXml(downloadMedicationData);

                            XmlNodeList nodeListError = doc1.GetElementsByTagName("Error");
                            string ErrorText = "";
                            Errorresponse = "";
                            foreach (XmlNode node in nodeListError)
                            {
                                ErrorText = node.SelectSingleNode("Text").InnerText;
                                MDVLogger.RcopiaLogMessage("Response: Download_Allergies", model.PatientId, "error", downloadMedicationData, ErrorText, 0, true,UserId);
                                return Medications;
                            }

                            XmlNodeList nodes = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response/MedicationList/Medication");
                            string LastUpdateDate = "";
                            XmlNodeList nodesLastUpdateDate = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response");
                            foreach (XmlNode nodeUpdate in nodesLastUpdateDate)
                            {
                                LastUpdateDate = nodeUpdate.SelectSingleNode("LastUpdateDate").InnerText;
                            }
                            foreach (XmlNode node in nodes)
                            {

                                XmlNode Drug1 = node.SelectSingleNode("Sig/Drug");
                                var DrugIsFreeText = false;
                                if (Drug1 != null)
                                {

                                    var DrugNDCID = Drug1.SelectSingleNode("NDCID") != null ? Drug1.SelectSingleNode("NDCID").InnerText : "";
                                    var DrugRcopiaID = Drug1.SelectSingleNode("RcopiaID").InnerText;
                                    if (DrugNDCID == "" && DrugRcopiaID == "")
                                    {
                                        DrugIsFreeText = true;
                                    }
                                }
                                if (!DrugIsFreeText)
                                {
                                    MedicationModel medication = new MedicationModel();
                                    medication.IsDeleted = node.SelectSingleNode("Deleted").InnerText;

                                    medication.RcopiaID = node.SelectSingleNode("RcopiaID").InnerText;

                                    XmlNode prescription = node.SelectSingleNode("Prescription");

                                    if (prescription != null)
                                    {
                                        medication.PrescriptionRcopiaID = prescription.SelectSingleNode("RcopiaID").InnerText;
                                    }
                                    XmlNode patient = node.SelectSingleNode("Patient");

                                    if (patient != null)
                                    {
                                        medication.PatientID = MDVUtility.ToLong(patient.SelectSingleNode("ExternalID").InnerText);
                                    }

                                    XmlNode provider = node.SelectSingleNode("Provider");

                                    if (provider != null)
                                    {
                                        //medication.ProviderID = MDVUtility.ToLong(provider.SelectSingleNode("ExternalID").InnerText);//use in future
                                        medication.ProviderRcopiaID = provider.SelectSingleNode("RcopiaID").InnerText;
                                        medication.RcopiaUserName = provider.SelectSingleNode("Username").InnerText;
                                    }

                                    XmlNode preparer = node.SelectSingleNode("Preparer");

                                    if (preparer != null)
                                    {
                                        //medication.Preparer_UserID = MDVUtility.ToLong(preparer.SelectSingleNode("ExternalID").InnerText);//use in future
                                        medication.PreparerRcopiaID = provider.SelectSingleNode("RcopiaID").InnerText;
                                    }

                                    medication.DrugCodingLevel = node.SelectSingleNode("DrugCodingLevel") != null ? node.SelectSingleNode("DrugCodingLevel").InnerText : "";

                                    XmlNode Drug = node.SelectSingleNode("Sig/Drug");

                                    if (Drug != null)
                                    {
                                        DrugModel drugModel = new DrugModel();
                                        drugModel.NDCID = Drug.SelectSingleNode("NDCID") != null ? Drug.SelectSingleNode("NDCID").InnerText : "";
                                        drugModel.FirstDataBankMedID = Drug.SelectSingleNode("FirstDataBankMedID") != null ? Drug.SelectSingleNode("FirstDataBankMedID").InnerText : "";
                                        drugModel.RcopiaID = Drug.SelectSingleNode("RcopiaID").InnerText;
                                        XmlNode rxNorm = Drug.SelectSingleNode("RxnormID");
                                        if (rxNorm != null)
                                        {
                                            drugModel.RxnormID = Drug.SelectSingleNode("RxnormID") != null ? Drug.SelectSingleNode("RxnormID").InnerText : "";
                                            drugModel.RxnormIDType = Drug.SelectSingleNode("RxnormIDType") != null ? Drug.SelectSingleNode("RxnormIDType").InnerText : "";
                                        }


                                        drugModel.BrandName = Drug.SelectSingleNode("BrandName").InnerText;
                                        drugModel.GenericName = Drug.SelectSingleNode("GenericName").InnerText;
                                        drugModel.Schedule = Drug.SelectSingleNode("Schedule") != null ? Drug.SelectSingleNode("Schedule").InnerText : "";

                                        drugModel.BrandType = Drug.SelectSingleNode("BrandType") != null ? Drug.SelectSingleNode("BrandType").InnerText : "";
                                        drugModel.LegendStatus = Drug.SelectSingleNode("LegendStatus") != null ? Drug.SelectSingleNode("LegendStatus").InnerText : "";
                                        drugModel.Route = Drug.SelectSingleNode("Route").InnerText;
                                        drugModel.Form = Drug.SelectSingleNode("Form") != null ? Drug.SelectSingleNode("Form").InnerText : "";
                                        drugModel.Strength = Drug.SelectSingleNode("Strength") != null ? Drug.SelectSingleNode("Strength").InnerText : "";
                                        medication.drugModel = drugModel;

                                    }
                                    medication.Action = node.SelectSingleNode("Sig/Action") != null ? node.SelectSingleNode("Sig/Action").InnerText : "";
                                    medication.Dose = node.SelectSingleNode("Sig/Dose") != null ? node.SelectSingleNode("Sig/Dose").InnerText : "";
                                    medication.DoseUnit = node.SelectSingleNode("Sig/DoseUnit") != null ? node.SelectSingleNode("Sig/DoseUnit").InnerText : "";
                                    medication.Routeby = node.SelectSingleNode("Sig/Route") != null ? node.SelectSingleNode("Sig/Route").InnerText : "";
                                    medication.DoseTiming = node.SelectSingleNode("Sig/DoseTiming") != null ? node.SelectSingleNode("Sig/DoseTiming").InnerText : "";
                                    medication.DoseOther = node.SelectSingleNode("Sig/DoseOther") != null ? node.SelectSingleNode("Sig/DoseOther").InnerText : "";
                                    medication.Duration = node.SelectSingleNode("Sig/Duration") != null ? MDVUtility.ToInt(node.SelectSingleNode("Sig/Duration").InnerText) : 0;
                                    medication.Quantity = node.SelectSingleNode("Sig/Quantity").InnerText;
                                    medication.QuantityUnit = node.SelectSingleNode("Sig/QuantityUnit").InnerText;
                                    medication.Refill = node.SelectSingleNode("Sig/Refills") != null ? node.SelectSingleNode("Sig/Refills").InnerText : "";
                                    medication.Substitution = node.SelectSingleNode("Sig/SubstitutionPermitted").InnerText;
                                    medication.PatientNotes = node.SelectSingleNode("Sig/PatientNotes").InnerText;
                                    if (node.SelectSingleNode("StartDate").InnerText != "")
                                    {
                                        medication.StartDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("StartDate").InnerText));
                                    }
                                    if (node.SelectSingleNode("StopDate").InnerText != "")
                                    {
                                        medication.StopDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("StopDate").InnerText));
                                    }
                                    if (node.SelectSingleNode("FillDate").InnerText != "")
                                    {
                                        medication.FillDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("FillDate").InnerText));
                                    }


                                    medication.StopReason = node.SelectSingleNode("StopReason").InnerText;
                                    if (node.SelectSingleNode("SigChangedDate").InnerText != "")
                                    {
                                        medication.SigChangedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("SigChangedDate").InnerText));
                                    }
                                    if (node.SelectSingleNode("LastModifiedDate").InnerText != "")
                                    {
                                        medication.LastModifiedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("LastModifiedDate").InnerText));
                                    }
                                    medication.LastModifiedBy = node.SelectSingleNode("LastModifiedBy").InnerText;


                                    medication.LastUpdateDate = LastUpdateDate;
                                    Medications.Add(medication);
                                }

                            }


                            if (Medications.Count == 0)
                            {
                                MedicationModel medication = new MedicationModel();
                                medication.LastUpdateDate = LastUpdateDate;
                                medication.RcopiaID = "0";
                                Medications.Add(medication);
                            }

                        }
                    }
                }
                MDVLogger.RcopiaLogMessage("Response: update_Medication", "", "", Medications.ToString(), "", count, true,UserId);
                if (Errorresponse != string.Empty)
                {
                    MedicationModel objmodel = new MedicationModel();
                    objmodel.Status = "10 minutes";
                    Medications.Add(objmodel);
                    MDVLogger.RcopiaLogMessage("Response: update_Medication", "", "", Errorresponse, "", count, true,UserId);
                }
                return Medications;
            }
            catch (Exception ex)
            {
                MedicationModel objmedication = new MedicationModel();
                objmedication.Status = ex.Message;
                Medications.Add(objmedication);
                return Medications;
            }


        }
        public List<MedicationModel> DownloadMedicationsListANS2(RcopiaModel model, int count, string MedicationLastUpdateDateForLIMP = "", SharedVariable sharedVariable = null, Int64 UserId = 0)
        {
            List<MedicationModel> Medications = new List<MedicationModel>();
            try
            {
                Patient_Demographic objpatient = new Patient_Demographic();
                DSRcopia dsRcopia = new DSRcopia();
                RcopiaModel modelRcopia = new RcopiaModel();
                RcopiaHelper helperRcopia = new RcopiaHelper();
                List<RcopiaModel> ListRcopia = helperRcopia.GetRcopiaInfo(sharedVariable);
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(4);
                string RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;
                string error = string.Empty;
                client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue("application/xml"));
                var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

                var ANS1url = RcopiaANSbackup + "?xml=" + inputdata;
                MDVLogger.RcopiaLogMessage("Request: GET URL from ANS2", "", "", ANS1url,null,0,true,UserId);
                HttpResponseMessage ResponseDownloadMedication = client.GetAsync(ANS1url).Result;
                MDVLogger.RcopiaLogMessage("Response: GET URL  from ANS2", "", "", ResponseDownloadMedication.ToString(),null,0,true,UserId);
                if (ResponseDownloadMedication != null)
                {
                    var GetdataANS2 = ResponseDownloadMedication.Content.ReadAsStringAsync().Result;
                    XmlDocument DocANS2 = new XmlDocument();
                    DocANS2.LoadXml(GetdataANS2);
                    XmlNodeList nodeListuploadurlANS2 = DocANS2.GetElementsByTagName("EngineUploadURL");
                    XmlNodeList nodelistDownloadurlANS2 = DocANS2.GetElementsByTagName("EngineDownloadURL");
                    XmlNodeList nodelistWebBrowserURLANS2 = DocANS2.GetElementsByTagName("WebBrowserURL");
                    string UploadUrlANS2 = string.Empty;
                    string downloadUrlANS2 = string.Empty;
                    string WebBrowserURLANS2 = string.Empty;
                    foreach (XmlNode node in nodelistWebBrowserURLANS2)
                    {
                        WebBrowserURLANS2 = node.InnerText;
                    }
                    foreach (XmlNode node in nodeListuploadurlANS2)
                    {
                        UploadUrlANS2 = node.InnerText;
                    }
                    foreach (XmlNode node in nodelistDownloadurlANS2)
                    {
                        downloadUrlANS2 = node.InnerText;
                    }





                    var DownloadMedicationXml = MDVUtility.GetXmlForDownloadMedication(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, model.PatientId, model.MedicationLastUpdateDate, MedicationLastUpdateDateForLIMP);
                    var DownloadUrl = model.EngineDownloadURL + "?xml=" + DownloadMedicationXml;
                    MDVLogger.RcopiaLogMessage("Request: update_Medication", "", "", DownloadUrl, "", count,true,UserId);
                    HttpResponseMessage ResponseData = client.GetAsync(DownloadUrl).Result;
                    var downloadMedicationData = ResponseData.Content.ReadAsStringAsync().Result;
                    MDVLogger.RcopiaLogMessage("Response: update_Medication", "", "", downloadMedicationData, "", count,true,UserId);
                    if (downloadMedicationData != string.Empty)
                    {
                        modelRcopia.URLID = 1;
                        modelRcopia.EngineDownloadURL = downloadUrlANS2;
                        modelRcopia.EngineUploadURL = UploadUrlANS2;
                        modelRcopia.WebBrowserURL = WebBrowserURLANS2;
                        modelRcopia.CreatedOn = DateTime.Now;
                        modelRcopia.ModifiedOn = DateTime.Now;
                        modelRcopia.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        modelRcopia.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dsRcopia = new DSRcopia();
                        DSRcopia.Rcopia_GetUrlRow dr = dsRcopia.Rcopia_GetUrl.NewRcopia_GetUrlRow();
                        dr.UrlID = modelRcopia.URLID;
                        dr.EngineDownloadURL = modelRcopia.EngineDownloadURL;
                        dr.EngineUploadURL = modelRcopia.EngineUploadURL;
                        dr.WebBrowserURL = modelRcopia.WebBrowserURL;
                        dr.IsActive = true;
                        dr.CreatedOn = modelRcopia.CreatedOn;
                        dr.ModifiedOn = modelRcopia.ModifiedOn;
                        dr.CreatedBy = modelRcopia.CreatedBy;
                        dr.ModifiedBy = modelRcopia.ModifiedBy;
                        dsRcopia.Rcopia_GetUrl.AddRcopia_GetUrlRow(dr);
                    }
                    XmlDocument doc1 = new XmlDocument();
                    doc1.LoadXml(downloadMedicationData);

                    XmlNodeList nodeListError = doc1.GetElementsByTagName("Error");
                    string ErrorText = "";
                    foreach (XmlNode node in nodeListError)
                    {
                        ErrorText = node.SelectSingleNode("Text").InnerText;
                        MDVLogger.RcopiaLogMessage("Response: Download_Allergies", model.PatientId, "error", downloadMedicationData, ErrorText, 0, true,UserId);
                        return Medications;
                    }

                    XmlNodeList nodes = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response/MedicationList/Medication");
                    string LastUpdateDate = "";
                    XmlNodeList nodesLastUpdateDate = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response");
                    foreach (XmlNode nodeUpdate in nodesLastUpdateDate)
                    {
                        LastUpdateDate = nodeUpdate.SelectSingleNode("LastUpdateDate").InnerText;
                    }
                    foreach (XmlNode node in nodes)
                    {
                        XmlNode Drug1 = node.SelectSingleNode("Sig/Drug");
                        var DrugIsFreeText = false;
                        if (Drug1 != null)
                        {

                            var DrugNDCID = Drug1.SelectSingleNode("NDCID") != null ? Drug1.SelectSingleNode("NDCID").InnerText : "";
                            var DrugRcopiaID = Drug1.SelectSingleNode("RcopiaID").InnerText;
                            if (DrugNDCID == "" && DrugRcopiaID == "")
                            {
                                DrugIsFreeText = true;
                            }
                        }
                        if (!DrugIsFreeText)
                        {
                            MedicationModel medication = new MedicationModel();
                            medication.IsDeleted = node.SelectSingleNode("Deleted").InnerText;

                            medication.RcopiaID = node.SelectSingleNode("RcopiaID").InnerText;

                            XmlNode prescription = node.SelectSingleNode("Prescription");

                            if (prescription != null)
                            {
                                medication.PrescriptionRcopiaID = prescription.SelectSingleNode("RcopiaID").InnerText;
                            }
                            XmlNode patient = node.SelectSingleNode("Patient");

                            if (patient != null)
                            {
                                medication.PatientID = MDVUtility.ToLong(patient.SelectSingleNode("ExternalID").InnerText);
                            }

                            XmlNode provider = node.SelectSingleNode("Provider");

                            if (provider != null)
                            {
                                //medication.ProviderID = MDVUtility.ToLong(provider.SelectSingleNode("ExternalID").InnerText);//use in future
                                medication.ProviderRcopiaID = provider.SelectSingleNode("RcopiaID").InnerText;
                                medication.RcopiaUserName = provider.SelectSingleNode("Username").InnerText;
                            }

                            XmlNode preparer = node.SelectSingleNode("Preparer");

                            if (preparer != null)
                            {
                                //medication.Preparer_UserID = MDVUtility.ToLong(preparer.SelectSingleNode("ExternalID").InnerText);//use in future
                                medication.PreparerRcopiaID = provider.SelectSingleNode("RcopiaID").InnerText;
                            }

                            medication.DrugCodingLevel = node.SelectSingleNode("DrugCodingLevel") != null ? node.SelectSingleNode("DrugCodingLevel").InnerText : "";

                            XmlNode Drug = node.SelectSingleNode("Sig/Drug");

                            if (Drug != null)
                            {
                                DrugModel drugModel = new DrugModel();
                                drugModel.NDCID = Drug.SelectSingleNode("NDCID") != null ? Drug.SelectSingleNode("NDCID").InnerText : "";
                                drugModel.FirstDataBankMedID = Drug.SelectSingleNode("FirstDataBankMedID") != null ? Drug.SelectSingleNode("FirstDataBankMedID").InnerText : "";
                                drugModel.RcopiaID = Drug.SelectSingleNode("RcopiaID").InnerText;
                                XmlNode rxNorm = Drug.SelectSingleNode("RxnormID");
                                if (rxNorm != null)
                                {
                                    drugModel.RxnormID = Drug.SelectSingleNode("RxnormID") != null ? Drug.SelectSingleNode("RxnormID").InnerText : "";
                                    drugModel.RxnormIDType = Drug.SelectSingleNode("RxnormIDType") != null ? Drug.SelectSingleNode("RxnormIDType").InnerText : "";
                                }


                                drugModel.BrandName = Drug.SelectSingleNode("BrandName").InnerText;
                                drugModel.GenericName = Drug.SelectSingleNode("GenericName").InnerText;
                                drugModel.Schedule = Drug.SelectSingleNode("Schedule") != null ? Drug.SelectSingleNode("Schedule").InnerText : "";

                                drugModel.BrandType = Drug.SelectSingleNode("BrandType") != null ? Drug.SelectSingleNode("BrandType").InnerText : "";
                                drugModel.LegendStatus = Drug.SelectSingleNode("LegendStatus") != null ? Drug.SelectSingleNode("LegendStatus").InnerText : "";
                                drugModel.Route = Drug.SelectSingleNode("Route").InnerText;
                                drugModel.Form = Drug.SelectSingleNode("Form") != null ? Drug.SelectSingleNode("Form").InnerText : "";
                                drugModel.Strength = Drug.SelectSingleNode("Strength") != null ? Drug.SelectSingleNode("Strength").InnerText : "";
                                medication.drugModel = drugModel;

                            }
                            medication.Action = node.SelectSingleNode("Sig/Action") != null ? node.SelectSingleNode("Sig/Action").InnerText : "";
                            medication.Dose = node.SelectSingleNode("Sig/Dose") != null ? node.SelectSingleNode("Sig/Dose").InnerText : "";
                            medication.DoseUnit = node.SelectSingleNode("Sig/DoseUnit") != null ? node.SelectSingleNode("Sig/DoseUnit").InnerText : "";
                            medication.Routeby = node.SelectSingleNode("Sig/Route") != null ? node.SelectSingleNode("Sig/Route").InnerText : "";
                            medication.DoseTiming = node.SelectSingleNode("Sig/DoseTiming") != null ? node.SelectSingleNode("Sig/DoseTiming").InnerText : "";
                            medication.DoseOther = node.SelectSingleNode("Sig/DoseOther") != null ? node.SelectSingleNode("Sig/DoseOther").InnerText : "";
                            medication.Duration = node.SelectSingleNode("Sig/Duration") != null ? MDVUtility.ToInt(node.SelectSingleNode("Sig/Duration").InnerText) : 0;
                            medication.Quantity = node.SelectSingleNode("Sig/Quantity").InnerText;
                            medication.QuantityUnit = node.SelectSingleNode("Sig/QuantityUnit").InnerText;
                            medication.Refill = node.SelectSingleNode("Sig/Refills") != null ? node.SelectSingleNode("Sig/Refills").InnerText : "";
                            medication.Substitution = node.SelectSingleNode("Sig/SubstitutionPermitted").InnerText;
                            medication.PatientNotes = node.SelectSingleNode("Sig/PatientNotes").InnerText;
                            if (node.SelectSingleNode("StartDate").InnerText != "")
                            {
                                medication.StartDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("StartDate").InnerText));
                            }
                            if (node.SelectSingleNode("StopDate").InnerText != "")
                            {
                                medication.StopDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("StopDate").InnerText));
                            }
                            if (node.SelectSingleNode("FillDate").InnerText != "")
                            {
                                medication.FillDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("FillDate").InnerText));
                            }


                            medication.StopReason = node.SelectSingleNode("StopReason").InnerText;
                            if (node.SelectSingleNode("SigChangedDate").InnerText != "")
                            {
                                medication.SigChangedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("SigChangedDate").InnerText));
                            }
                            if (node.SelectSingleNode("LastModifiedDate").InnerText != "")
                            {
                                medication.LastModifiedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("LastModifiedDate").InnerText));
                            }
                            medication.LastModifiedBy = node.SelectSingleNode("LastModifiedBy").InnerText;


                            medication.LastUpdateDate = LastUpdateDate;
                            Medications.Add(medication);
                        }
                    }


                    if (Medications.Count == 0)
                    {
                        MedicationModel medication = new MedicationModel();
                        medication.LastUpdateDate = LastUpdateDate;
                        medication.RcopiaID = "0";
                        Medications.Add(medication);
                    }
                }
                //MDVLogger.RcopiaLogMessage("Request: update_Medication", "", "", Medications.ToString(), "", count);
                return Medications;
            }
            catch (Exception ex)
            {
                MedicationModel objmedication = new MedicationModel();
                objmedication.Status = ex.Message;
                Medications.Add(objmedication);
                return Medications;
            }


        }
        public NotificationCountModel DownloadNotificationCount(RcopiaModel model)
        {
            NotificationCountModel NotificationCount = new NotificationCountModel();
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));
                DSRcopia dsRcopia1 = new DSRcopia();
                BLObject<DSRcopia> obj1 = new BLLRcopia().SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode);
                dsRcopia1 = obj1.Data;
                if (obj1.Data != null)
                {
                    if (dsRcopia1.SoftwareCustomersInfo.Rows.Count > 0)
                    {
                        string RcopiaANSbackup = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaANSbackupColumn.ColumnName]);
                        string RcopiaScretkey = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaScretkeyColumn.ColumnName]);
                        string RcopiaVendorUsername = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaVendorUsernameColumn.ColumnName]);
                        string RcopiaVendorPassword = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaVendorPasswordColumn.ColumnName]);
                        string RcopiaPortalSystemName = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaPortalSystemNameColumn.ColumnName]);
                        string RcopiaPracticeUserName = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaPracticeUserNameColumn.ColumnName]);

                        var GetNotificationCountXml = MDVUtility.GetXmlForNotificationCount(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName);
                        var DownloadUrl = model.EngineDownloadURL + "?xml=" + GetNotificationCountXml;

                        HttpResponseMessage ResponseData = client.GetAsync(DownloadUrl).Result;
                        var downloadNotificationData = ResponseData.Content.ReadAsStringAsync().Result;
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(downloadNotificationData);
                        XmlNodeList nodes = doc.DocumentElement.SelectNodes("/RCExtResponse/Response/NotificationCountList/NotificationCount");

                        foreach (XmlNode node in nodes)
                        {

                            string gettype = node.SelectSingleNode("Type").InnerText;
                            if (gettype == "refill")
                            {
                                NotificationCount.RefillPrescriptionCount += Convert.ToInt32(node.SelectSingleNode("Number").InnerText);
                            }
                            else if (gettype == "rx_pending")
                            {
                                NotificationCount.PendingPrescriptionCount += Convert.ToInt32(node.SelectSingleNode("Number").InnerText);
                            }

                        }

                        return NotificationCount;
                    }
                    else
                    {
                        throw new Exception("No Record Found");
                    }
                }
                else
                {
                    throw new Exception("No Record Found");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DownloadPriscriptionsAndSave(RcopiaModel model, string PrescriptionLastUpdateDateForLIMP = "", SharedVariable sharedVariable = null, string UserName = null, Int64 UserId = 0)
        {
            try
            {
                List<PrescriptionsModel> DownloadPrescription = new List<PrescriptionsModel>();
                DownloadPrescription = GetDownloadPrescriptionRcopiaResponseUrl(model, PrescriptionLastUpdateDateForLIMP, sharedVariable,UserId);
                bool successfullySave = true;
                string messageFromSave = "";
                string IsPrescriptionDeleted = "";
                string SavedPrescriptionIds = "";
                int DownloadPrescriptionCount = 0;//localy use
                if (DownloadPrescription.Count == 1 && DownloadPrescription[0].RcopiaID == "0")
                {
                    DownloadPrescriptionCount = 0;
                }
                else
                {
                    DownloadPrescriptionCount = DownloadPrescription.Count;
                    MedicationsHelper helperPrescription = new MedicationsHelper();

                    dynamic response = JObject.Parse(helperPrescription.SavePrescription(DownloadPrescription, false, sharedVariable, UserName));
                    if (response.status == true)
                    {
                        //if (MDVUtility.ToInt64(model.NotesId) > 0 && response.InsertPrescription == "true")
                        //{
                        //    BLObject<DSClinicalMedication> obj = new BLLClinical().attachPrescriptionsWithNotes(MDVUtility.ToStr(response.SavedPrescriptionIds), MDVUtility.ToInt64(model.NotesId), sharedVariable);
                        //    if (obj.Data != null)
                        //    {
                        //        messageFromSave = response.Message;
                        //        SavedPrescriptionIds = response.SavedPrescriptionIds;
                        //        successfullySave = true;
                        //    }
                        //    else
                        //    {
                        //        messageFromSave = "Problem in Saving prescription from DrFirst";
                        //        successfullySave = false;
                        //    }
                        //}
                        //else
                        //{
                        messageFromSave = response.Message;
                        SavedPrescriptionIds = response.SavedPrescriptionIds;
                        successfullySave = true;
                        //}
                        IsPrescriptionDeleted = response.IsPrescriptionDeleted;


                    }
                    else
                    {
                        //messageFromSave = response.Message;
                        messageFromSave = "Problem in Saving prescription from DrFirst";
                        successfullySave = false;
                    }

                }
                var respons1 = new
                {
                    MessageFromSave = messageFromSave,
                    status = successfullySave,
                    SavedPrescriptionIds = SavedPrescriptionIds,
                    lastUpdateDate = DownloadPrescription[0].LastUpdateDate,
                    IsPrescriptionDeleted = IsPrescriptionDeleted,
                    DownloadPrescriptionCount = DownloadPrescriptionCount

                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(respons1));
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
        public List<PrescriptionsModel> DownloadPrescriptionsList(RcopiaModel model, int count, string PrescriptionLastUpdateDateForLIMP = "", SharedVariable sharedVariable = null, Int64 UserId = 0)
        {
            List<PrescriptionsModel> Prescriptions = new List<PrescriptionsModel>();
            string response = string.Empty;
            try
            {
                DSRcopia dsRcopia = new DSRcopia();
                RcopiaModel modelRcopia = new RcopiaModel();
                RcopiaHelper helperRcopia = new RcopiaHelper();

                string RcopiaANSbackup = model.RcopiaANSbackup;
                string RcopiaScretkey = model.RcopiaScretkey;
                string RcopiaVendorUsername = model.RcopiaVendorUsername;
                string RcopiaVendorPassword = model.RcopiaVendorPassword;
                string RcopiaPortalSystemName = model.RcopiaPortalSystemName;
                string RcopiaPracticeUserName = model.RcopiaPracticeUserName;


                modelRcopia.EngineDownloadURL = model.EngineDownloadURL;

                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(4);
                //client.BaseAddress = new Uri("http://localhost:8080/");
                string error = string.Empty;
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));
                //BLObject<DSRcopia> obj =BLLRcopiaObj.SelectGetUrls();
                //dsRcopia = obj.Data;
                //modelRcopia.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);


                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));


                var DownloadPrescriptionXml = MDVUtility.GetXmlForDownloadPrescription(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, model.PatientId, model.PrescriptionLastUpdateDate, PrescriptionLastUpdateDateForLIMP);
                var DownloadUrl = modelRcopia.EngineDownloadURL + "?xml=" + DownloadPrescriptionXml;
                MDVLogger.RcopiaLogMessage("Request: update_Prescription", "", "", DownloadUrl, "", count, true,UserId);
                HttpResponseMessage ResponseData = client.GetAsync(DownloadUrl).Result;
                var downloadPrescription = ResponseData.Content.ReadAsStringAsync().Result;
                MDVLogger.RcopiaLogMessage("Response: update_Prescription", "", "", downloadPrescription, "", count, true,UserId);
                if (downloadPrescription != string.Empty)
                {
                    XmlDocument doc1 = new XmlDocument();
                    doc1.LoadXml(downloadPrescription);

                    XmlNodeList nodeListError = doc1.GetElementsByTagName("Error");
                    string ErrorText = "";
                    foreach (XmlNode node in nodeListError)
                    {
                        ErrorText = node.SelectSingleNode("Text").InnerText;
                        MDVLogger.RcopiaLogMessage("Response: Download_Prescription", model.PatientId, "error", downloadPrescription, ErrorText, 0, true,UserId);
                        return Prescriptions;
                    }

                    XmlNodeList nodes = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response/PrescriptionList/Prescription");
                    string LastUpdateDate = "";
                    XmlNodeList nodesLastUpdateDate = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response");
                    foreach (XmlNode nodeUpdate in nodesLastUpdateDate)
                    {
                        LastUpdateDate = nodeUpdate.SelectSingleNode("LastUpdateDate").InnerText;
                    }
                    foreach (XmlNode node in nodes)
                    {
                        XmlNode Drug1 = node.SelectSingleNode("Sig/Drug");
                        var DrugIsFreeText = false;
                        if (Drug1 != null)
                        {
                            var DrugNDCID = Drug1.SelectSingleNode("NDCID") != null ? Drug1.SelectSingleNode("NDCID").InnerText : "";
                            var DrugRcopiaID = Drug1.SelectSingleNode("RcopiaID").InnerText;
                            if (DrugNDCID == "" && DrugRcopiaID == "")
                            {
                                DrugIsFreeText = true;
                            }
                        }
                        if (!DrugIsFreeText)
                        {


                            PrescriptionsModel Prescription = new PrescriptionsModel();
                            Prescription.IsDeleted = node.SelectSingleNode("Deleted") != null ? node.SelectSingleNode("Deleted").InnerText : "";

                            Prescription.Voided = node.SelectSingleNode("Voided") != null ? node.SelectSingleNode("Voided").InnerText : "";
                            Prescription.Denied = node.SelectSingleNode("Denied") != null ? node.SelectSingleNode("Denied").InnerText : "";
                            Prescription.RcopiaID = node.SelectSingleNode("RcopiaID").InnerText;




                            XmlNode patient = node.SelectSingleNode("Patient");

                            if (patient != null)
                            {
                                Prescription.PatientID = MDVUtility.ToLong(patient.SelectSingleNode("ExternalID").InnerText);

                            }

                            XmlNode provider = node.SelectSingleNode("Provider");

                            if (provider != null)
                            {

                                Prescription.ProviderID = provider.SelectSingleNode("RcopiaID").InnerText;
                            }

                            XmlNode preparer = node.SelectSingleNode("Preparer");

                            if (preparer != null)
                            {

                                Prescription.Preparer_UserID = provider.SelectSingleNode("RcopiaID").InnerText;
                            }


                            PharamacyModel PharamacyModels = new PharamacyModel();

                            XmlNode Pharamacy = node.SelectSingleNode("Pharmacy");

                            if (Pharamacy != null)
                            {
                                PharamacyModels.RcopiaID = Pharamacy.SelectSingleNode("RcopiaID").InnerText;
                                PharamacyModels.RcopiaMasterID = Pharamacy.SelectSingleNode("RcopiaMasterID").InnerText;
                                PharamacyModels.NCPDPID = Pharamacy.SelectSingleNode("NCPDPID") != null ? Pharamacy.SelectSingleNode("NCPDPID").InnerText : "";

                                XmlNode NPI = Pharamacy.SelectSingleNode("NPI");
                                if (NPI != null)
                                {
                                    PharamacyModels.NPI = Pharamacy.SelectSingleNode("NPI").InnerText;
                                }
                                else
                                {
                                    PharamacyModels.NPI = "";
                                }
                                PharamacyModels.PharmacyName = Pharamacy.SelectSingleNode("Name").InnerText;
                                PharamacyModels.Address = Pharamacy.SelectSingleNode("Address1").InnerText;
                                PharamacyModels.City = Pharamacy.SelectSingleNode("City") != null ? Pharamacy.SelectSingleNode("City").InnerText : "";
                                PharamacyModels.State = Pharamacy.SelectSingleNode("State").InnerText;
                                PharamacyModels.Zip = Pharamacy.SelectSingleNode("Zip") != null ? Pharamacy.SelectSingleNode("Zip").InnerText : "";
                                PharamacyModels.Phone = Pharamacy.SelectSingleNode("Phone").InnerText;
                                PharamacyModels.Fax = Pharamacy.SelectSingleNode("Fax").InnerText;
                                PharamacyModels.Is24Hour = Pharamacy.SelectSingleNode("Is24Hour").InnerText;
                                PharamacyModels.Level3 = Pharamacy.SelectSingleNode("Level3").InnerText;
                                PharamacyModels.Electronic = Pharamacy.SelectSingleNode("Electronic").InnerText;
                                PharamacyModels.MailOrder = Pharamacy.SelectSingleNode("MailOrder").InnerText;
                                PharamacyModels.Retail = Pharamacy.SelectSingleNode("Retail").InnerText;
                                PharamacyModels.LongTermCare = Pharamacy.SelectSingleNode("LongTermCare").InnerText;
                                PharamacyModels.Specialty = Pharamacy.SelectSingleNode("Specialty").InnerText;
                                PharamacyModels.CanReceiveControlledSubstance = Pharamacy.SelectSingleNode("CanReceiveControlledSubstance").InnerText;
                                Prescription.PharamacyModel = PharamacyModels;
                            }


                            XmlNode Drug = node.SelectSingleNode("Sig/Drug");

                            if (Drug != null)
                            {
                                // after changes for unique Drug, NDCID is unique for each drug thats why RcopiaID is replaced by NDCID
                                Prescription.DrugRcopiaID = Drug.SelectSingleNode("NDCID") != null ? Drug.SelectSingleNode("NDCID").InnerText : "";//Drug.SelectSingleNode("RcopiaID").InnerText;
                                DrugModel drugModel = new DrugModel();

                                drugModel.NDCID = Drug.SelectSingleNode("NDCID") != null ? Drug.SelectSingleNode("NDCID").InnerText : "";
                                drugModel.FirstDataBankMedID = Drug.SelectSingleNode("FirstDataBankMedID") != null ? Drug.SelectSingleNode("FirstDataBankMedID").InnerText : "";
                                drugModel.RcopiaID = Drug.SelectSingleNode("RcopiaID").InnerText;

                                XmlNode rxNorm = Drug.SelectSingleNode("RxnormID");
                                if (rxNorm != null)
                                {
                                    drugModel.RxnormID = Drug.SelectSingleNode("RxnormID") != null ? Drug.SelectSingleNode("RxnormID").InnerText : "";
                                    drugModel.RxnormIDType = Drug.SelectSingleNode("RxnormIDType") != null ? Drug.SelectSingleNode("RxnormIDType").InnerText : "";
                                }

                                drugModel.DrugDescription = Drug.SelectSingleNode("DrugDescription").InnerText;
                                drugModel.BrandName = Drug.SelectSingleNode("BrandName").InnerText;
                                drugModel.GenericName = Drug.SelectSingleNode("GenericName").InnerText;
                                drugModel.Schedule = Drug.SelectSingleNode("Schedule") != null ? Drug.SelectSingleNode("Schedule").InnerText : "";

                                drugModel.BrandType = Drug.SelectSingleNode("BrandType") != null ? Drug.SelectSingleNode("BrandType").InnerText : "";
                                drugModel.LegendStatus = Drug.SelectSingleNode("LegendStatus") != null ? Drug.SelectSingleNode("LegendStatus").InnerText : "";
                                drugModel.Route = Drug.SelectSingleNode("Route").InnerText;
                                drugModel.Form = Drug.SelectSingleNode("Form") != null ? Drug.SelectSingleNode("Form").InnerText : "";
                                drugModel.Strength = Drug.SelectSingleNode("Strength") != null ? Drug.SelectSingleNode("Strength").InnerText : "";
                                Prescription.drugModel = drugModel;

                            }
                            MedicationModel Medicationmodel = new MedicationModel();

                            Medicationmodel.Action = node.SelectSingleNode("Sig/Action") != null ? node.SelectSingleNode("Sig/Action").InnerText : "";
                            Medicationmodel.Dose = node.SelectSingleNode("Sig/Dose") != null ? node.SelectSingleNode("Sig/Dose").InnerText : "";
                            Medicationmodel.DoseUnit = node.SelectSingleNode("Sig/DoseUnit") != null ? node.SelectSingleNode("Sig/DoseUnit").InnerText : "";
                            Medicationmodel.Routeby = node.SelectSingleNode("Sig/Route") != null ? node.SelectSingleNode("Sig/Route").InnerText : "";
                            Medicationmodel.DoseTiming = node.SelectSingleNode("Sig/DoseTiming") != null ? node.SelectSingleNode("Sig/DoseTiming").InnerText : "";
                            Medicationmodel.DoseOther = node.SelectSingleNode("Sig/DoseOther") != null ? node.SelectSingleNode("Sig/DoseOther").InnerText : "";
                            Medicationmodel.Duration = node.SelectSingleNode("Sig/Duration") != null ? MDVUtility.ToInt(node.SelectSingleNode("Sig/Duration").InnerText) : 0;
                            Medicationmodel.Quantity = node.SelectSingleNode("Sig/Quantity").InnerText;
                            Medicationmodel.QuantityUnit = node.SelectSingleNode("Sig/QuantityUnit").InnerText;
                            Medicationmodel.Refill = node.SelectSingleNode("Sig/Refills").InnerText;
                            Medicationmodel.Substitution = node.SelectSingleNode("Sig/SubstitutionPermitted").InnerText;
                            Medicationmodel.PatientNotes = node.SelectSingleNode("Sig/PatientNotes").InnerText;
                            Prescription.MedicationModel = Medicationmodel;



                            Prescription.Refill = node.SelectSingleNode("Sig/Refills").InnerText;
                            Prescription.SubstitutionPermitted = node.SelectSingleNode("Sig/SubstitutionPermitted").InnerText;
                            Prescription.OtherNotes = node.SelectSingleNode("Sig/OtherNotes").InnerText;
                            Prescription.PatientNotes = node.SelectSingleNode("Sig/PatientNotes").InnerText;
                            Prescription.Comments = node.SelectSingleNode("Sig/Comments").InnerText;

                            if (node.SelectSingleNode("CreatedDate").InnerText != "")
                            {
                                Prescription.CreatedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("CreatedDate").InnerText));
                            }

                            if (node.SelectSingleNode("CompletedDate").InnerText != "")
                            {
                                Prescription.CompletedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("CompletedDate").InnerText));
                            }

                            if (node.SelectSingleNode("StopDate").InnerText != "")
                            {
                                Prescription.StopDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("StopDate").InnerText));
                            }
                            //StopDate
                            Prescription.LastModifiedBy = node.SelectSingleNode("LastModifiedBy") != null ? node.SelectSingleNode("LastModifiedBy").InnerText : "";

                            if (node.SelectSingleNode("SignedDate").InnerText != "")
                            {
                                Prescription.SignedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("SignedDate").InnerText));
                            }
                            if (node.SelectSingleNode("LastModifiedDate").InnerText != "")
                            {
                                Prescription.LastModifiedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("LastModifiedDate").InnerText));
                            }
                            Prescription.IntendedUse = node.SelectSingleNode("IntendedUse") != null ? node.SelectSingleNode("IntendedUse").InnerText : "";
                            XmlNode CompletionAction = node.SelectSingleNode("CompletionAction");

                            if (CompletionAction != null)
                            {
                                Prescription.CompletionAction = node.SelectSingleNode("CompletionAction").InnerText;
                                Prescription.SendMethod = node.SelectSingleNode("SendMethod") != null ? node.SelectSingleNode("SendMethod").InnerText : "";
                            }
                            else
                            {
                                Prescription.CompletionAction = "Pending";
                                Prescription.SendMethod = "";
                            }


                            Prescription.LastUpdateDate = LastUpdateDate;
                            Prescriptions.Add(Prescription);
                        }

                    }


                    if (Prescriptions.Count == 0)
                    {
                        PrescriptionsModel medication = new PrescriptionsModel();
                        medication.LastUpdateDate = LastUpdateDate;
                        medication.RcopiaID = "0";
                        Prescriptions.Add(medication);
                    }
                }
                //MDVLogger.RcopiaLogMessage("Response: update_Prescription", "", "", Prescriptions.ToString(), "", count);
                return Prescriptions;
            }
            catch (Exception ex)
            {
                PrescriptionsModel objPrescription = new PrescriptionsModel();
                objPrescription.Status = ex.Message;
                Prescriptions.Add(objPrescription);
                return Prescriptions;
            }


        }
        public List<PrescriptionsModel> DownloadPrescriptionsListANS1(RcopiaModel model, int count, string PrescriptionLastUpdateDateForLIMP = "", SharedVariable sharedVariable = null, Int64 UserId = 0)
        {
            List<PrescriptionsModel> Prescriptions = new List<PrescriptionsModel>();
            DateTime Modified;
            string exception = "DrFirst ANS cannot be called within 10 minutes,Please retry later";
            try
            {
                DSRcopia dsRcopia = new DSRcopia();
                Patient_Demographic objpatient = new Patient_Demographic();
                RcopiaModel modelRcopia = new RcopiaModel();
                RcopiaHelper helperRcopia = new RcopiaHelper();
                List<RcopiaModel> ListRcopia = helperRcopia.GetRcopiaInfo(sharedVariable);
                string RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;

                BLObject<DSRcopia> objGetUrl = BLLRcopiaObj.SelectGetUrls(sharedVariable);
                dsRcopia = objGetUrl.Data;
                Modified = MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.ModifiedOnColumn.ColumnName]);
                int minutes = Convert.ToInt32(DateTime.Now.Subtract(Modified).TotalMinutes);
                if (minutes > 10)
                {


                    BLObject<DSRcopia> obj = BLLRcopiaObj.SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode, sharedVariable);
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(4);
                    dsRcopia = obj.Data;
                    modelRcopia.RcopiaANS = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaANSColumn.ColumnName]);
                    string RcopiaANS = modelRcopia.RcopiaANS;
                    string error = string.Empty;
                    client.DefaultRequestHeaders.Accept.Add(
                           new MediaTypeWithQualityHeaderValue("application/xml"));
                    var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

                    var ANS1url = RcopiaANS + "?xml=" + inputdata;
                    MDVLogger.RcopiaLogMessage("Request: update_Prescription", "", "", ANS1url, "", 0, true,UserId);
                    HttpResponseMessage ResponseDownloadPrescription = client.GetAsync(ANS1url).Result;

                    if (ResponseDownloadPrescription != null)
                    {
                        var GetdataANS1 = ResponseDownloadPrescription.Content.ReadAsStringAsync().Result;
                        XmlDocument DocANS1 = new XmlDocument();
                        DocANS1.LoadXml(GetdataANS1);
                        XmlNodeList nodeListuploadurlANS1 = DocANS1.GetElementsByTagName("EngineUploadURL");
                        XmlNodeList nodelistDownloadurlANS1 = DocANS1.GetElementsByTagName("EngineDownloadURL");
                        XmlNodeList nodelistWebBrowserURLANS1 = DocANS1.GetElementsByTagName("WebBrowserURL");
                        string UploadUrlANS1 = string.Empty;
                        string downloadUrlANS1 = string.Empty;
                        string WebBrowserURLANS1 = string.Empty;
                        foreach (XmlNode node in nodelistWebBrowserURLANS1)
                        {
                            WebBrowserURLANS1 = node.InnerText;
                        }
                        foreach (XmlNode node in nodeListuploadurlANS1)
                        {
                            UploadUrlANS1 = node.InnerText;
                        }
                        foreach (XmlNode node in nodelistDownloadurlANS1)
                        {
                            downloadUrlANS1 = node.InnerText;
                        }






                        var DownloadPrescriptionXml = MDVUtility.GetXmlForDownloadPrescription(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, model.PatientId, model.PrescriptionLastUpdateDate, PrescriptionLastUpdateDateForLIMP);
                        var DownloadUrl = downloadUrlANS1 + "?xml=" + DownloadPrescriptionXml;
                        MDVLogger.RcopiaLogMessage("Request: update_Prescription", "", "", DownloadUrl, "", count, true,UserId);
                        HttpResponseMessage ResponseData = client.GetAsync(DownloadUrl).Result;
                        var downloadPrescription = ResponseData.Content.ReadAsStringAsync().Result;
                        MDVLogger.RcopiaLogMessage("Request: update_Prescription", "", "", downloadPrescription, "", count, true,UserId);
                        if (downloadPrescription != string.Empty)
                        {
                            modelRcopia.URLID = 1;
                            modelRcopia.EngineDownloadURL = downloadUrlANS1;
                            modelRcopia.EngineUploadURL = UploadUrlANS1;
                            modelRcopia.WebBrowserURL = WebBrowserURLANS1;
                            modelRcopia.CreatedOn = DateTime.Now;
                            modelRcopia.ModifiedOn = DateTime.Now;
                            modelRcopia.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            modelRcopia.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dsRcopia = new DSRcopia();
                            DSRcopia.Rcopia_GetUrlRow dr = dsRcopia.Rcopia_GetUrl.NewRcopia_GetUrlRow();
                            dr.UrlID = modelRcopia.URLID;
                            dr.EngineDownloadURL = modelRcopia.EngineDownloadURL;
                            dr.EngineUploadURL = modelRcopia.EngineUploadURL;
                            dr.WebBrowserURL = modelRcopia.WebBrowserURL;
                            dr.IsActive = true;
                            dr.CreatedOn = modelRcopia.CreatedOn;
                            dr.ModifiedOn = modelRcopia.ModifiedOn;
                            dr.CreatedBy = modelRcopia.CreatedBy;
                            dr.ModifiedBy = modelRcopia.ModifiedBy;
                            dsRcopia.Rcopia_GetUrl.AddRcopia_GetUrlRow(dr);

                            BLObject<DSRcopia> objMofieddate = BLLRcopiaObj.UpDateGetUrl(dsRcopia, sharedVariable);
                        }
                        XmlDocument doc1 = new XmlDocument();
                        doc1.LoadXml(downloadPrescription);

                        XmlNodeList nodeListError = doc1.GetElementsByTagName("Error");
                        string ErrorText = "";
                        exception = "";
                        foreach (XmlNode node in nodeListError)
                        {
                            ErrorText = node.SelectSingleNode("Text").InnerText;
                            MDVLogger.RcopiaLogMessage("Response: Update_Prescription", model.PatientId, "error", downloadPrescription, ErrorText, 0, true,UserId);
                            return Prescriptions;
                        }

                        XmlNodeList nodes = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response/PrescriptionList/Prescription");
                        string LastUpdateDate = "";
                        XmlNodeList nodesLastUpdateDate = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response");
                        foreach (XmlNode nodeUpdate in nodesLastUpdateDate)
                        {
                            LastUpdateDate = nodeUpdate.SelectSingleNode("LastUpdateDate").InnerText;
                        }
                        foreach (XmlNode node in nodes)
                        {
                            XmlNode Drug1 = node.SelectSingleNode("Sig/Drug");
                            var DrugIsFreeText = false;
                            if (Drug1 != null)
                            {
                                var DrugNDCID = Drug1.SelectSingleNode("NDCID") != null ? Drug1.SelectSingleNode("NDCID").InnerText : "";
                                var DrugRcopiaID = Drug1.SelectSingleNode("RcopiaID").InnerText;
                                if (DrugNDCID == "" && DrugRcopiaID == "")
                                {
                                    DrugIsFreeText = true;
                                }
                            }
                            if (!DrugIsFreeText)
                            {
                                PrescriptionsModel Prescription = new PrescriptionsModel();
                                Prescription.IsDeleted = node.SelectSingleNode("Deleted") != null ? node.SelectSingleNode("Deleted").InnerText : "";

                                Prescription.Voided = node.SelectSingleNode("Voided") != null ? node.SelectSingleNode("Voided").InnerText : "";
                                Prescription.Denied = node.SelectSingleNode("Denied") != null ? node.SelectSingleNode("Denied").InnerText : "";
                                Prescription.RcopiaID = node.SelectSingleNode("RcopiaID").InnerText;




                                XmlNode patient = node.SelectSingleNode("Patient");

                                if (patient != null)
                                {
                                    Prescription.PatientID = MDVUtility.ToLong(patient.SelectSingleNode("ExternalID").InnerText);

                                }

                                XmlNode provider = node.SelectSingleNode("Provider");

                                if (provider != null)
                                {

                                    Prescription.ProviderID = provider.SelectSingleNode("RcopiaID").InnerText;
                                }

                                XmlNode preparer = node.SelectSingleNode("Preparer");

                                if (preparer != null)
                                {

                                    Prescription.Preparer_UserID = provider.SelectSingleNode("RcopiaID").InnerText;
                                }


                                PharamacyModel PharamacyModels = new PharamacyModel();

                                XmlNode Pharamacy = node.SelectSingleNode("Pharmacy");

                                if (Pharamacy != null)
                                {
                                    PharamacyModels.RcopiaID = Pharamacy.SelectSingleNode("RcopiaID").InnerText;
                                    PharamacyModels.RcopiaMasterID = Pharamacy.SelectSingleNode("RcopiaMasterID").InnerText;
                                    PharamacyModels.NCPDPID = Pharamacy.SelectSingleNode("NCPDPID") != null ? Pharamacy.SelectSingleNode("NCPDPID").InnerText : "";

                                    XmlNode NPI = Pharamacy.SelectSingleNode("NPI");
                                    if (NPI != null)
                                    {
                                        PharamacyModels.NPI = Pharamacy.SelectSingleNode("NPI").InnerText;
                                    }
                                    else
                                    {
                                        PharamacyModels.NPI = "";
                                    }
                                    PharamacyModels.PharmacyName = Pharamacy.SelectSingleNode("Name").InnerText;
                                    PharamacyModels.Address = Pharamacy.SelectSingleNode("Address1").InnerText;
                                    PharamacyModels.City = Pharamacy.SelectSingleNode("City") != null ? Pharamacy.SelectSingleNode("City").InnerText : "";
                                    PharamacyModels.State = Pharamacy.SelectSingleNode("State").InnerText;
                                    PharamacyModels.Zip = Pharamacy.SelectSingleNode("Zip") != null ? Pharamacy.SelectSingleNode("Zip").InnerText : "";
                                    PharamacyModels.Phone = Pharamacy.SelectSingleNode("Phone").InnerText;
                                    PharamacyModels.Fax = Pharamacy.SelectSingleNode("Fax").InnerText;
                                    PharamacyModels.Is24Hour = Pharamacy.SelectSingleNode("Is24Hour").InnerText;
                                    PharamacyModels.Level3 = Pharamacy.SelectSingleNode("Level3").InnerText;
                                    PharamacyModels.Electronic = Pharamacy.SelectSingleNode("Electronic").InnerText;
                                    PharamacyModels.MailOrder = Pharamacy.SelectSingleNode("MailOrder").InnerText;
                                    PharamacyModels.Retail = Pharamacy.SelectSingleNode("Retail").InnerText;
                                    PharamacyModels.LongTermCare = Pharamacy.SelectSingleNode("LongTermCare").InnerText;
                                    PharamacyModels.Specialty = Pharamacy.SelectSingleNode("Specialty").InnerText;
                                    PharamacyModels.CanReceiveControlledSubstance = Pharamacy.SelectSingleNode("CanReceiveControlledSubstance").InnerText;
                                    Prescription.PharamacyModel = PharamacyModels;
                                }


                                XmlNode Drug = node.SelectSingleNode("Sig/Drug");

                                if (Drug != null)
                                {
                                    // after changes for unique Drug, NDCID is unique for each drug thats why RcopiaID is replaced by NDCID
                                    Prescription.DrugRcopiaID = Drug.SelectSingleNode("NDCID") != null ? Drug.SelectSingleNode("NDCID").InnerText : "";//Drug.SelectSingleNode("RcopiaID").InnerText;
                                    DrugModel drugModel = new DrugModel();

                                    drugModel.NDCID = Drug.SelectSingleNode("NDCID") != null ? Drug.SelectSingleNode("NDCID").InnerText : "";
                                    drugModel.FirstDataBankMedID = Drug.SelectSingleNode("FirstDataBankMedID") != null ? Drug.SelectSingleNode("FirstDataBankMedID").InnerText : "";
                                    drugModel.RcopiaID = Drug.SelectSingleNode("RcopiaID").InnerText;

                                    XmlNode rxNorm = Drug.SelectSingleNode("RxnormID");
                                    if (rxNorm != null)
                                    {
                                        drugModel.RxnormID = Drug.SelectSingleNode("RxnormID") != null ? Drug.SelectSingleNode("RxnormID").InnerText : "";
                                        drugModel.RxnormIDType = Drug.SelectSingleNode("RxnormIDType") != null ? Drug.SelectSingleNode("RxnormIDType").InnerText : "";
                                    }

                                    drugModel.DrugDescription = Drug.SelectSingleNode("DrugDescription").InnerText;
                                    drugModel.BrandName = Drug.SelectSingleNode("BrandName").InnerText;
                                    drugModel.GenericName = Drug.SelectSingleNode("GenericName").InnerText;
                                    drugModel.Schedule = Drug.SelectSingleNode("Schedule") != null ? Drug.SelectSingleNode("Schedule").InnerText : "";

                                    drugModel.BrandType = Drug.SelectSingleNode("BrandType") != null ? Drug.SelectSingleNode("BrandType").InnerText : "";
                                    drugModel.LegendStatus = Drug.SelectSingleNode("LegendStatus") != null ? Drug.SelectSingleNode("LegendStatus").InnerText : "";
                                    drugModel.Route = Drug.SelectSingleNode("Route").InnerText;
                                    drugModel.Form = Drug.SelectSingleNode("Form") != null ? Drug.SelectSingleNode("Form").InnerText : "";
                                    drugModel.Strength = Drug.SelectSingleNode("Strength") != null ? Drug.SelectSingleNode("Strength").InnerText : "";
                                    Prescription.drugModel = drugModel;

                                }
                                MedicationModel Medicationmodel = new MedicationModel();

                                Medicationmodel.Action = node.SelectSingleNode("Sig/Action") != null ? node.SelectSingleNode("Sig/Action").InnerText : "";
                                Medicationmodel.Dose = node.SelectSingleNode("Sig/Dose") != null ? node.SelectSingleNode("Sig/Dose").InnerText : "";
                                Medicationmodel.DoseUnit = node.SelectSingleNode("Sig/DoseUnit") != null ? node.SelectSingleNode("Sig/DoseUnit").InnerText : "";
                                Medicationmodel.Routeby = node.SelectSingleNode("Sig/Route") != null ? node.SelectSingleNode("Sig/Route").InnerText : "";
                                Medicationmodel.DoseTiming = node.SelectSingleNode("Sig/DoseTiming") != null ? node.SelectSingleNode("Sig/DoseTiming").InnerText : "";
                                Medicationmodel.DoseOther = node.SelectSingleNode("Sig/DoseOther") != null ? node.SelectSingleNode("Sig/DoseOther").InnerText : "";
                                Medicationmodel.Duration = node.SelectSingleNode("Sig/Duration") != null ? MDVUtility.ToInt(node.SelectSingleNode("Sig/Duration").InnerText) : 0;
                                Medicationmodel.Quantity = node.SelectSingleNode("Sig/Quantity").InnerText;
                                Medicationmodel.QuantityUnit = node.SelectSingleNode("Sig/QuantityUnit").InnerText;
                                Medicationmodel.Refill = node.SelectSingleNode("Sig/Refills").InnerText;
                                Medicationmodel.Substitution = node.SelectSingleNode("Sig/SubstitutionPermitted").InnerText;
                                Medicationmodel.PatientNotes = node.SelectSingleNode("Sig/PatientNotes").InnerText;
                                Prescription.MedicationModel = Medicationmodel;



                                Prescription.Refill = node.SelectSingleNode("Sig/Refills").InnerText;
                                Prescription.SubstitutionPermitted = node.SelectSingleNode("Sig/SubstitutionPermitted").InnerText;
                                Prescription.OtherNotes = node.SelectSingleNode("Sig/OtherNotes").InnerText;
                                Prescription.PatientNotes = node.SelectSingleNode("Sig/PatientNotes").InnerText;
                                Prescription.Comments = node.SelectSingleNode("Sig/Comments").InnerText;

                                if (node.SelectSingleNode("CreatedDate").InnerText != "")
                                {
                                    Prescription.CreatedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("CreatedDate").InnerText));
                                }

                                if (node.SelectSingleNode("CompletedDate").InnerText != "")
                                {
                                    Prescription.CompletedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("CompletedDate").InnerText));
                                }

                                if (node.SelectSingleNode("StopDate").InnerText != "")
                                {
                                    Prescription.StopDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("StopDate").InnerText));
                                }
                                //StopDate
                                Prescription.LastModifiedBy = node.SelectSingleNode("LastModifiedBy") != null ? node.SelectSingleNode("LastModifiedBy").InnerText : "";

                                if (node.SelectSingleNode("SignedDate").InnerText != "")
                                {
                                    Prescription.SignedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("SignedDate").InnerText));
                                }
                                if (node.SelectSingleNode("LastModifiedDate").InnerText != "")
                                {
                                    Prescription.LastModifiedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("LastModifiedDate").InnerText));
                                }
                                Prescription.IntendedUse = node.SelectSingleNode("IntendedUse") != null ? node.SelectSingleNode("IntendedUse").InnerText : "";
                                XmlNode CompletionAction = node.SelectSingleNode("CompletionAction");


                                if (CompletionAction != null)
                                {
                                    Prescription.CompletionAction = node.SelectSingleNode("CompletionAction").InnerText;
                                    Prescription.SendMethod = node.SelectSingleNode("SendMethod") != null ? node.SelectSingleNode("SendMethod").InnerText : "";
                                }
                                else
                                {
                                    Prescription.CompletionAction = "Pending";
                                    Prescription.SendMethod = "";
                                }


                                Prescription.LastUpdateDate = LastUpdateDate;
                                Prescriptions.Add(Prescription);
                            }

                        }


                        if (Prescriptions.Count == 0)
                        {
                            PrescriptionsModel medication = new PrescriptionsModel();
                            medication.LastUpdateDate = LastUpdateDate;
                            medication.RcopiaID = "0";
                            Prescriptions.Add(medication);
                        }
                    }
                }
                // MDVLogger.RcopiaLogMessage("Response: update_Prescription", "", "", Prescriptions.ToString(), "", count);

                if (exception != string.Empty)
                {
                    PrescriptionsModel objprescription = new PrescriptionsModel();
                    objprescription.Status = "10 minutes";
                    Prescriptions.Add(objprescription);
                    MDVLogger.RcopiaLogMessage("Response: update_Prescription", "", "", exception, "", count, true,UserId);
                }
                return Prescriptions;
            }
            catch (Exception ex)
            {
                PrescriptionsModel objPrescription = new PrescriptionsModel();
                objPrescription.Status = ex.Message;
                Prescriptions.Add(objPrescription);
                return Prescriptions;
            }


        }
        public List<PrescriptionsModel> DownloadPrescriptionsListANS2(RcopiaModel model, int count, string PrescriptionLastUpdateDateForLIMP = "", SharedVariable sharedVariable = null, Int64 UserId = 0)
        {
            List<PrescriptionsModel> Prescriptions = new List<PrescriptionsModel>();
            try
            {
                DSRcopia dsRcopia = new DSRcopia();
                BLObject<DSRcopia> obj = BLLRcopiaObj.SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode, sharedVariable);
                dsRcopia = obj.Data;
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(4);
                RcopiaModel modelRcopia = new RcopiaModel();
                RcopiaHelper helperRcopia = new RcopiaHelper();
                List<RcopiaModel> ListRcopia = helperRcopia.GetRcopiaInfo(sharedVariable);
                string RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                string RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                string RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                string RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                string RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                string RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;
                string error = string.Empty;
                client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue("application/xml"));
                var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>" + RcopiaVendorUsername + "</VendorName>       <VendorPassword>" + RcopiaVendorPassword + "</VendorPassword>       </Caller>    <RcopiaPracticeUsername>" + RcopiaPracticeUserName + "</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

                var ANS1url = RcopiaANSbackup + "?xml=" + inputdata;
                MDVLogger.RcopiaLogMessage("Request: GET URL from ANS2", "", "", ANS1url,null,0,true,UserId);
                HttpResponseMessage ResponsePrescription = client.GetAsync(ANS1url).Result;
                MDVLogger.RcopiaLogMessage("Response: GET URL  from ANS2", "", "", ResponsePrescription.ToString(),null,0,true,UserId);
                if (ResponsePrescription != null)
                {
                    var GetdataANS2 = ResponsePrescription.Content.ReadAsStringAsync().Result;
                    XmlDocument DocANS2 = new XmlDocument();
                    DocANS2.LoadXml(GetdataANS2);
                    XmlNodeList nodeListuploadurlANS1 = DocANS2.GetElementsByTagName("EngineUploadURL");
                    XmlNodeList nodelistDownloadurlANS1 = DocANS2.GetElementsByTagName("EngineDownloadURL");
                    XmlNodeList nodelistWebBrowserURLANS1 = DocANS2.GetElementsByTagName("WebBrowserURL");
                    string UploadUrlANS2 = string.Empty;
                    string downloadUrlANS2 = string.Empty;
                    string WebBrowserURLANS2 = string.Empty;
                    foreach (XmlNode node in nodelistWebBrowserURLANS1)
                    {
                        WebBrowserURLANS2 = node.InnerText;
                    }
                    foreach (XmlNode node in nodeListuploadurlANS1)
                    {
                        UploadUrlANS2 = node.InnerText;
                    }
                    foreach (XmlNode node in nodelistDownloadurlANS1)
                    {
                        downloadUrlANS2 = node.InnerText;
                    }





                    var DownloadPrescriptionXml = MDVUtility.GetXmlForDownloadPrescription(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, model.PatientId, model.PrescriptionLastUpdateDate, PrescriptionLastUpdateDateForLIMP);
                    var DownloadUrl = downloadUrlANS2 + "?xml=" + DownloadPrescriptionXml;
                    MDVLogger.RcopiaLogMessage("Request: update_Prescription", "", "", DownloadUrl, "", count,true,UserId);
                    HttpResponseMessage ResponseData = client.GetAsync(DownloadUrl).Result;
                    var downloadPrescription = ResponseData.Content.ReadAsStringAsync().Result;
                    MDVLogger.RcopiaLogMessage("Response: update_Prescription", "", "", downloadPrescription, "", count,true,UserId);
                    if (downloadPrescription != string.Empty)
                    {
                        modelRcopia.URLID = 1;
                        modelRcopia.EngineDownloadURL = downloadUrlANS2;
                        modelRcopia.EngineUploadURL = UploadUrlANS2;
                        modelRcopia.WebBrowserURL = WebBrowserURLANS2;
                        modelRcopia.CreatedOn = DateTime.Now;
                        modelRcopia.ModifiedOn = DateTime.Now;
                        modelRcopia.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        modelRcopia.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dsRcopia = new DSRcopia();
                        DSRcopia.Rcopia_GetUrlRow dr = dsRcopia.Rcopia_GetUrl.NewRcopia_GetUrlRow();
                        dr.UrlID = modelRcopia.URLID;
                        dr.EngineDownloadURL = modelRcopia.EngineDownloadURL;
                        dr.EngineUploadURL = modelRcopia.EngineUploadURL;
                        dr.WebBrowserURL = modelRcopia.WebBrowserURL;
                        dr.IsActive = true;
                        dr.CreatedOn = modelRcopia.CreatedOn;
                        dr.ModifiedOn = modelRcopia.ModifiedOn;
                        dr.CreatedBy = modelRcopia.CreatedBy;
                        dr.ModifiedBy = modelRcopia.ModifiedBy;
                        dsRcopia.Rcopia_GetUrl.AddRcopia_GetUrlRow(dr);

                        BLObject<DSRcopia> objMofieddate = BLLRcopiaObj.UpDateGetUrl(dsRcopia, sharedVariable);
                        XmlDocument doc1 = new XmlDocument();
                        doc1.LoadXml(downloadPrescription);

                        XmlNodeList nodeListError = doc1.GetElementsByTagName("Error");
                        string ErrorText = "";
                        foreach (XmlNode node in nodeListError)
                        {
                            ErrorText = node.SelectSingleNode("Text").InnerText;
                            MDVLogger.RcopiaLogMessage("Response: Download_Prescription", model.PatientId, "error", downloadPrescription, ErrorText,0,true,UserId);
                            return Prescriptions;
                        }

                        XmlNodeList nodes = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response/PrescriptionList/Prescription");
                        string LastUpdateDate = "";
                        XmlNodeList nodesLastUpdateDate = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response");
                        foreach (XmlNode nodeUpdate in nodesLastUpdateDate)
                        {
                            LastUpdateDate = nodeUpdate.SelectSingleNode("LastUpdateDate").InnerText;
                        }
                        foreach (XmlNode node in nodes)
                        {
                            XmlNode Drug1 = node.SelectSingleNode("Sig/Drug");
                            var DrugIsFreeText = false;
                            if (Drug1 != null)
                            {
                                var DrugNDCID = Drug1.SelectSingleNode("NDCID") != null ? Drug1.SelectSingleNode("NDCID").InnerText : "";
                                var DrugRcopiaID = Drug1.SelectSingleNode("RcopiaID").InnerText;
                                if (DrugNDCID == "" && DrugRcopiaID == "")
                                {
                                    DrugIsFreeText = true;
                                }
                            }
                            if (!DrugIsFreeText)
                            {
                                PrescriptionsModel Prescription = new PrescriptionsModel();
                                Prescription.IsDeleted = node.SelectSingleNode("Deleted") != null ? node.SelectSingleNode("Deleted").InnerText : "";

                                Prescription.Voided = node.SelectSingleNode("Voided") != null ? node.SelectSingleNode("Voided").InnerText : "";
                                Prescription.Denied = node.SelectSingleNode("Denied") != null ? node.SelectSingleNode("Denied").InnerText : "";
                                Prescription.RcopiaID = node.SelectSingleNode("RcopiaID").InnerText;




                                XmlNode patient = node.SelectSingleNode("Patient");

                                if (patient != null)
                                {
                                    Prescription.PatientID = MDVUtility.ToLong(patient.SelectSingleNode("ExternalID").InnerText);

                                }

                                XmlNode provider = node.SelectSingleNode("Provider");

                                if (provider != null)
                                {

                                    Prescription.ProviderID = provider.SelectSingleNode("RcopiaID").InnerText;
                                }

                                XmlNode preparer = node.SelectSingleNode("Preparer");

                                if (preparer != null)
                                {

                                    Prescription.Preparer_UserID = provider.SelectSingleNode("RcopiaID").InnerText;
                                }


                                PharamacyModel PharamacyModels = new PharamacyModel();

                                XmlNode Pharamacy = node.SelectSingleNode("Pharmacy");

                                if (Pharamacy != null)
                                {
                                    PharamacyModels.RcopiaID = Pharamacy.SelectSingleNode("RcopiaID").InnerText;
                                    PharamacyModels.RcopiaMasterID = Pharamacy.SelectSingleNode("RcopiaMasterID").InnerText;
                                    PharamacyModels.NCPDPID = Pharamacy.SelectSingleNode("NCPDPID") != null ? Pharamacy.SelectSingleNode("NCPDPID").InnerText : "";

                                    XmlNode NPI = Pharamacy.SelectSingleNode("NPI");
                                    if (NPI != null)
                                    {
                                        PharamacyModels.NPI = Pharamacy.SelectSingleNode("NPI").InnerText;
                                    }
                                    else
                                    {
                                        PharamacyModels.NPI = "";
                                    }
                                    PharamacyModels.PharmacyName = Pharamacy.SelectSingleNode("Name").InnerText;
                                    PharamacyModels.Address = Pharamacy.SelectSingleNode("Address1").InnerText;
                                    PharamacyModels.City = Pharamacy.SelectSingleNode("City") != null ? Pharamacy.SelectSingleNode("City").InnerText : "";
                                    PharamacyModels.State = Pharamacy.SelectSingleNode("State").InnerText;
                                    PharamacyModels.Zip = Pharamacy.SelectSingleNode("Zip") != null ? Pharamacy.SelectSingleNode("Zip").InnerText : "";
                                    PharamacyModels.Phone = Pharamacy.SelectSingleNode("Phone").InnerText;
                                    PharamacyModels.Fax = Pharamacy.SelectSingleNode("Fax").InnerText;
                                    PharamacyModels.Is24Hour = Pharamacy.SelectSingleNode("Is24Hour").InnerText;
                                    PharamacyModels.Level3 = Pharamacy.SelectSingleNode("Level3").InnerText;
                                    PharamacyModels.Electronic = Pharamacy.SelectSingleNode("Electronic").InnerText;
                                    PharamacyModels.MailOrder = Pharamacy.SelectSingleNode("MailOrder").InnerText;
                                    PharamacyModels.Retail = Pharamacy.SelectSingleNode("Retail").InnerText;
                                    PharamacyModels.LongTermCare = Pharamacy.SelectSingleNode("LongTermCare").InnerText;
                                    PharamacyModels.Specialty = Pharamacy.SelectSingleNode("Specialty").InnerText;
                                    PharamacyModels.CanReceiveControlledSubstance = Pharamacy.SelectSingleNode("CanReceiveControlledSubstance").InnerText;
                                    Prescription.PharamacyModel = PharamacyModels;
                                }


                                XmlNode Drug = node.SelectSingleNode("Sig/Drug");

                                if (Drug != null)
                                {
                                    // after changes for unique Drug, NDCID is unique for each drug thats why RcopiaID is replaced by NDCID
                                    Prescription.DrugRcopiaID = Drug.SelectSingleNode("NDCID") != null ? Drug.SelectSingleNode("NDCID").InnerText : "";//Drug.SelectSingleNode("RcopiaID").InnerText;
                                    DrugModel drugModel = new DrugModel();

                                    drugModel.NDCID = Drug.SelectSingleNode("NDCID") != null ? Drug.SelectSingleNode("NDCID").InnerText : "";
                                    drugModel.FirstDataBankMedID = Drug.SelectSingleNode("FirstDataBankMedID") != null ? Drug.SelectSingleNode("FirstDataBankMedID").InnerText : "";
                                    drugModel.RcopiaID = Drug.SelectSingleNode("RcopiaID").InnerText;

                                    XmlNode rxNorm = Drug.SelectSingleNode("RxnormID");
                                    if (rxNorm != null)
                                    {
                                        drugModel.RxnormID = Drug.SelectSingleNode("RxnormID") != null ? Drug.SelectSingleNode("RxnormID").InnerText : "";
                                        drugModel.RxnormIDType = Drug.SelectSingleNode("RxnormIDType") != null ? Drug.SelectSingleNode("RxnormIDType").InnerText : "";
                                    }

                                    drugModel.DrugDescription = Drug.SelectSingleNode("DrugDescription").InnerText;
                                    drugModel.BrandName = Drug.SelectSingleNode("BrandName").InnerText;
                                    drugModel.GenericName = Drug.SelectSingleNode("GenericName").InnerText;
                                    drugModel.Schedule = Drug.SelectSingleNode("Schedule") != null ? Drug.SelectSingleNode("Schedule").InnerText : "";

                                    drugModel.BrandType = Drug.SelectSingleNode("BrandType") != null ? Drug.SelectSingleNode("BrandType").InnerText : "";
                                    drugModel.LegendStatus = Drug.SelectSingleNode("LegendStatus") != null ? Drug.SelectSingleNode("LegendStatus").InnerText : "";
                                    drugModel.Route = Drug.SelectSingleNode("Route").InnerText;
                                    drugModel.Form = Drug.SelectSingleNode("Form") != null ? Drug.SelectSingleNode("Form").InnerText : "";
                                    drugModel.Strength = Drug.SelectSingleNode("Strength") != null ? Drug.SelectSingleNode("Strength").InnerText : "";
                                    Prescription.drugModel = drugModel;

                                }
                                MedicationModel Medicationmodel = new MedicationModel();

                                Medicationmodel.Action = node.SelectSingleNode("Sig/Action") != null ? node.SelectSingleNode("Sig/Action").InnerText : "";
                                Medicationmodel.Dose = node.SelectSingleNode("Sig/Dose") != null ? node.SelectSingleNode("Sig/Dose").InnerText : "";
                                Medicationmodel.DoseUnit = node.SelectSingleNode("Sig/DoseUnit") != null ? node.SelectSingleNode("Sig/DoseUnit").InnerText : "";
                                Medicationmodel.Routeby = node.SelectSingleNode("Sig/Route") != null ? node.SelectSingleNode("Sig/Route").InnerText : "";
                                Medicationmodel.DoseTiming = node.SelectSingleNode("Sig/DoseTiming") != null ? node.SelectSingleNode("Sig/DoseTiming").InnerText : "";
                                Medicationmodel.DoseOther = node.SelectSingleNode("Sig/DoseOther") != null ? node.SelectSingleNode("Sig/DoseOther").InnerText : "";
                                Medicationmodel.Duration = node.SelectSingleNode("Sig/Duration") != null ? MDVUtility.ToInt(node.SelectSingleNode("Sig/Duration").InnerText) : 0;
                                Medicationmodel.Quantity = node.SelectSingleNode("Sig/Quantity").InnerText;
                                Medicationmodel.QuantityUnit = node.SelectSingleNode("Sig/QuantityUnit").InnerText;
                                Medicationmodel.Refill = node.SelectSingleNode("Sig/Refills").InnerText;
                                Medicationmodel.Substitution = node.SelectSingleNode("Sig/SubstitutionPermitted").InnerText;
                                Medicationmodel.PatientNotes = node.SelectSingleNode("Sig/PatientNotes").InnerText;
                                Prescription.MedicationModel = Medicationmodel;



                                Prescription.Refill = node.SelectSingleNode("Sig/Refills").InnerText;
                                Prescription.SubstitutionPermitted = node.SelectSingleNode("Sig/SubstitutionPermitted").InnerText;
                                Prescription.OtherNotes = node.SelectSingleNode("Sig/OtherNotes").InnerText;
                                Prescription.PatientNotes = node.SelectSingleNode("Sig/PatientNotes").InnerText;
                                Prescription.Comments = node.SelectSingleNode("Sig/Comments").InnerText;

                                if (node.SelectSingleNode("CreatedDate").InnerText != "")
                                {
                                    Prescription.CreatedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("CreatedDate").InnerText));
                                }

                                if (node.SelectSingleNode("CompletedDate").InnerText != "")
                                {
                                    Prescription.CompletedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("CompletedDate").InnerText));
                                }

                                if (node.SelectSingleNode("StopDate").InnerText != "")
                                {
                                    Prescription.StopDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("StopDate").InnerText));
                                }
                                //StopDate
                                Prescription.LastModifiedBy = node.SelectSingleNode("LastModifiedBy") != null ? node.SelectSingleNode("LastModifiedBy").InnerText : "";

                                if (node.SelectSingleNode("SignedDate").InnerText != "")
                                {
                                    Prescription.SignedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("SignedDate").InnerText));
                                }
                                if (node.SelectSingleNode("LastModifiedDate").InnerText != "")
                                {
                                    Prescription.LastModifiedDate = Convert.ToDateTime(MDVUtility.GetDateMMDDYYY(node.SelectSingleNode("LastModifiedDate").InnerText));
                                }
                                Prescription.IntendedUse = node.SelectSingleNode("IntendedUse") != null ? node.SelectSingleNode("IntendedUse").InnerText : "";
                                XmlNode CompletionAction = node.SelectSingleNode("CompletionAction");


                                if (CompletionAction != null)
                                {
                                    Prescription.CompletionAction = node.SelectSingleNode("CompletionAction").InnerText;
                                    Prescription.SendMethod = node.SelectSingleNode("SendMethod") != null ? node.SelectSingleNode("SendMethod").InnerText : "";
                                }
                                else
                                {
                                    Prescription.CompletionAction = "Pending";
                                    Prescription.SendMethod = "";
                                }


                                Prescription.LastUpdateDate = LastUpdateDate;
                                Prescriptions.Add(Prescription);
                            }
                        }


                        if (Prescriptions.Count == 0)
                        {
                            PrescriptionsModel medication = new PrescriptionsModel();
                            medication.LastUpdateDate = LastUpdateDate;
                            medication.RcopiaID = "0";
                            Prescriptions.Add(medication);
                        }
                    }
                }
                //MDVLogger.RcopiaLogMessage("Response: update_Prescription", "", "", Prescriptions.ToString(), "", count);
                return Prescriptions;
            }
            catch (Exception ex)
            {
                PrescriptionsModel objPrescription = new PrescriptionsModel();
                objPrescription.Status = ex.Message;
                Prescriptions.Add(objPrescription);
                return Prescriptions;
            }


        }
        public string DownloadClinicalsForLIMPMode(RcopiaModel model)
        {
            try
            {
                model.IsPatientLastUpdateInfo = false;
                DSRcopia dsRcopia = new DSRcopia();
                BLObject<DSRcopia> obj = BLLRcopiaObj.SelectGetUrls();
                dsRcopia = obj.Data;
                if (obj.Data != null)
                {

                    model.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);
                    string MedicationLastUpdateDateForLIMP = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.MedicationLastUpdateDateForLIMPColumn.ColumnName]) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.MedicationLastUpdateDateForLIMPColumn.ColumnName]).ToString("MM/dd/yyyy HH:mm:ss")) : "";
                    string PrescriptionLastUpdateDateForLIMP = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.PrescriptionLastUpdateDateForLIMPColumn.ColumnName]) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.PrescriptionLastUpdateDateForLIMPColumn.ColumnName]).ToString("MM/dd/yyyy HH:mm:ss")) : "";
                    model.MedicationLastUpdateDateForLIMP = MedicationLastUpdateDateForLIMP;
                    model.PrescriptionLastUpdateDateForLIMP = PrescriptionLastUpdateDateForLIMP;

                    #region medication download

                    //model.MedicationLastUpdateDate = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName]);
                    dynamic DownloadMedicationResponse = JObject.Parse(DownloadMadicationsAndSave(model, MedicationLastUpdateDateForLIMP));

                    if (DownloadMedicationResponse.DownloadMedicationCount != 0)
                    {
                        model.IsPatientLastUpdateInfo = true;
                        if (DownloadMedicationResponse.status == true)
                        {
                            model.MedicationLastUpdateDateForLIMP = DownloadMedicationResponse.lastUpdateDate;

                            var responseMedication = UpdateMedicationAndPrescriptionLastUpdateDateForLIMP(model);
                            dynamic Medicationresponse = JObject.Parse(responseMedication);
                            if (Medicationresponse.status != true)
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = Medicationresponse.Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = DownloadMedicationResponse.MessageFromSave
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {

                        model.MedicationLastUpdateDateForLIMP = DownloadMedicationResponse.lastUpdateDate;
                        var responseMedication = UpdateMedicationAndPrescriptionLastUpdateDateForLIMP(model);
                        dynamic Medicationresponse = JObject.Parse(responseMedication);
                        if (Medicationresponse.status != true)
                        {
                            var response = new
                            {
                                status = false,
                                Message = Medicationresponse.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }

                    #endregion

                    #region prescription download

                    //model.PrescriptionLastUpdateDate = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn.ColumnName]);
                    dynamic DownloadPrescriptionResponse = JObject.Parse(DownloadPriscriptionsAndSave(model, PrescriptionLastUpdateDateForLIMP));

                    if (DownloadPrescriptionResponse.DownloadPrescriptionCount != 0)
                    {
                        model.IsPatientLastUpdateInfo = true;
                        if (DownloadPrescriptionResponse.status == true)
                        {
                            model.PrescriptionLastUpdateDateForLIMP = DownloadPrescriptionResponse.lastUpdateDate;

                            var responsePrescription = UpdateMedicationAndPrescriptionLastUpdateDateForLIMP(model);
                            dynamic Prescriptionresponse = JObject.Parse(responsePrescription);
                            if (Prescriptionresponse.status != true)
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = Prescriptionresponse.Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = DownloadPrescriptionResponse.MessageFromSave
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {

                        model.PrescriptionLastUpdateDateForLIMP = DownloadPrescriptionResponse.lastUpdateDate;
                        return UpdateMedicationAndPrescriptionLastUpdateDateForLIMP(model);

                    }

                    #endregion



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

                var response2 = new
                {
                    status = true,
                    Message = "Medication And Prescription For LIMP Are Downloaded SuccessFully"
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response2);
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
        public string UpdateMedicationAndPrescriptionLastUpdateDateForLIMP(RcopiaModel model)
        {
            try
            {
                DSRcopia dsRcopia = new DSRcopia();
                DSRcopia.Rcopia_GetUrlRow dr = dsRcopia.Rcopia_GetUrl.NewRcopia_GetUrlRow();

                if (model.MedicationLastUpdateDateForLIMP != "")
                {
                    dr.MedicationLastUpdateDateForLIMP = MDVUtility.ToDateTime(model.MedicationLastUpdateDateForLIMP);
                }
                if (model.PrescriptionLastUpdateDateForLIMP != "")
                {
                    dr.PrescriptionLastUpdateDateForLIMP = MDVUtility.ToDateTime(model.PrescriptionLastUpdateDateForLIMP);
                }

                dsRcopia.Rcopia_GetUrl.AddRcopia_GetUrlRow(dr);
                BLObject<DSRcopia> obj = BLLRcopiaObj.UpdateMedicationAndPrescriptionLastUpdateDateForLIMP(dsRcopia);
                dsRcopia = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = obj.Message
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
        public string DownloadReviewsAndSave(RcopiaModel model, SharedVariable sharedVariable = null)
        {
            try
            {
                List<ReviewedModel> DownloadReviews = new List<ReviewedModel>();
                DownloadReviews = DownloadReviewsList(model, sharedVariable);
                bool successfullySave = true;
                string messageFromSave = "";
                int DownloadReviewsCount = 0;//localy use
                var MedicationReviewID = 0;
                var AllergyReviewID = 0;
                if (DownloadReviews.Count == 0)
                {
                    DownloadReviewsCount = 0;
                }
                else
                {
                    DownloadReviewsCount = DownloadReviews.Count;

                    MedicationsHelper helperMedication = new MedicationsHelper();

                    dynamic response = JObject.Parse(helperMedication.SaveReviews(DownloadReviews, sharedVariable));
                    if (response.status == true)
                    {
                        messageFromSave = response.message;
                        successfullySave = true;
                        AllergyReviewID = response.AllergyReviewID;
                        MedicationReviewID = response.MedicationReviewID;
                    }
                    else
                    {
                        messageFromSave = response.message;
                        successfullySave = false;
                    }

                }
                var respons1 = new
                {
                    MessageFromSave = messageFromSave,
                    status = successfullySave,
                    DownloadReviewCount = DownloadReviewsCount,
                    AllergyReviewID = AllergyReviewID,
                    MedicationReviewID = MedicationReviewID,

                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(respons1));
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
        public List<ReviewedModel> DownloadReviewsList(RcopiaModel model, SharedVariable sharedVariable = null)
        {
            List<ReviewedModel> Revieweds = new List<ReviewedModel>();
            try
            {


                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));
                DSRcopia dsRcopia1 = new DSRcopia();
                BLObject<DSRcopia> obj1 = new BLLRcopia().SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode, sharedVariable);
                dsRcopia1 = obj1.Data;
                if (obj1.Data != null)
                {
                    if (dsRcopia1.SoftwareCustomersInfo.Rows.Count > 0)
                    {
                        string RcopiaANSbackup = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaANSbackupColumn.ColumnName]);
                        string RcopiaScretkey = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaScretkeyColumn.ColumnName]);
                        string RcopiaVendorUsername = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaVendorUsernameColumn.ColumnName]);
                        string RcopiaVendorPassword = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaVendorPasswordColumn.ColumnName]);
                        string RcopiaPortalSystemName = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaPortalSystemNameColumn.ColumnName]);
                        string RcopiaPracticeUserName = MDVUtility.ToStr(dsRcopia1.Tables[dsRcopia1.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia1.SoftwareCustomersInfo.RcopiaPracticeUserNameColumn.ColumnName]);

                        var DownloadReviewdXml = MDVUtility.GetXMLForReviwedStatus(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, model.PatientId);
                        var DownloadUrl = model.EngineDownloadURL + "?xml=" + DownloadReviewdXml;

                        HttpResponseMessage ResponseData = client.GetAsync(DownloadUrl).Result;
                        var downloadReviewedData = ResponseData.Content.ReadAsStringAsync().Result;
                        MDVLogger.RcopiaLogMessage("Response: Download_Reviewed_Data", model.PatientId, "Response", downloadReviewedData);
                        XmlDocument doc1 = new XmlDocument();
                        doc1.LoadXml(downloadReviewedData);

                        XmlNodeList nodeListError = doc1.GetElementsByTagName("Error");
                        string ErrorText = "";
                        foreach (XmlNode node in nodeListError)
                        {
                            ErrorText = node.SelectSingleNode("Text").InnerText;
                            MDVLogger.RcopiaLogMessage("Response: Download_Reviewed_Data", model.PatientId, "error", downloadReviewedData, ErrorText);
                            return Revieweds;
                        }

                        XmlNodeList nodes = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response/PatientReviewStatusList/PatientReviewStatus");


                        foreach (XmlNode node in nodes)
                        {
                            ReviewedModel Review = new ReviewedModel();

                            #region Medication Review
                            XmlNode MedicationReviews = node.SelectSingleNode("MedicationReviewStatus");
                            if (MedicationReviews != null)
                            {
                                //if (MedicationReviews.SelectSingleNode("HasRecords").InnerText == "y")
                                //{
                                XmlNode MedicationReviewsList = MedicationReviews.SelectSingleNode("ReviewList");
                                if (MedicationReviewsList != null)
                                {
                                    Review.PatientId = MDVUtility.ToLong(model.PatientId);
                                    XmlNodeList review = MedicationReviewsList.SelectNodes("Review");
                                    foreach (XmlNode ReviewData in review)
                                    {

                                        if (ReviewData.SelectSingleNode("ReviewDate").InnerText != "")
                                        {
                                            Review.ReviewedOn = ReviewData.SelectSingleNode("ReviewDate").InnerText;
                                            Review.ReviewedOn = Review.ReviewedOn.Replace(" EST", "");
                                        }

                                        XmlNode Reviewer = ReviewData.SelectSingleNode("Reviewer");
                                        if (Reviewer != null)
                                        {
                                            Review.ReviewedBy = Reviewer.SelectSingleNode("Username") != null ? Reviewer.SelectSingleNode("Username").InnerText : "";
                                        }
                                        break;
                                    }
                                    Review.WhichReviewed = "Medication";
                                    Revieweds.Add(Review);
                                }
                                //}
                                //else if (MedicationReviews.SelectSingleNode("HasRecords").InnerText == "n")
                                //{
                                //    if (MedicationReviews.SelectSingleNode("Complete").InnerText == "y")
                                //    {
                                //        XmlNode MedicationReviewsList = MedicationReviews.SelectSingleNode("ReviewList");
                                //        if (MedicationReviewsList != null)
                                //        {
                                //            Review.PatientId = MDVUtility.ToLong(model.PatientId);
                                //            XmlNodeList review = MedicationReviewsList.SelectNodes("Review");
                                //            foreach (XmlNode ReviewData in review)
                                //            {

                                //                if (ReviewData.SelectSingleNode("ReviewDate").InnerText != "")
                                //                {
                                //                    Review.ReviewedOn = ReviewData.SelectSingleNode("ReviewDate").InnerText;
                                //                    Review.ReviewedOn = Review.ReviewedOn.Replace(" EST", "");
                                //                }

                                //                XmlNode Reviewer = ReviewData.SelectSingleNode("Reviewer");
                                //                if (Reviewer != null)
                                //                {
                                //                    Review.ReviewedBy = Reviewer.SelectSingleNode("Username") != null ? Reviewer.SelectSingleNode("Username").InnerText : "";
                                //                }
                                //                break;
                                //            }
                                //            Review.WhichReviewed = "Medication";
                                //            Revieweds.Add(Review);
                                //        }
                                //    }
                                //}
                            }

                            #endregion
                            ReviewedModel Review1 = new ReviewedModel();
                            #region Allergy Review
                            XmlNode AllergyReviews = node.SelectSingleNode("AllergyReviewStatus");
                            if (AllergyReviews != null)
                            {
                                //if (AllergyReviews.SelectSingleNode("HasRecords").InnerText == "y")
                                //{
                                XmlNode AllergyReviewsList = AllergyReviews.SelectSingleNode("ReviewList");
                                if (AllergyReviewsList != null)
                                {
                                    Review1.PatientId = MDVUtility.ToLong(model.PatientId);
                                    XmlNodeList review = AllergyReviewsList.SelectNodes("Review");
                                    foreach (XmlNode ReviewData in review)
                                    {

                                        if (ReviewData.SelectSingleNode("ReviewDate").InnerText != "")
                                        {
                                            Review1.ReviewedOn = ReviewData.SelectSingleNode("ReviewDate").InnerText;
                                            Review1.ReviewedOn = Review1.ReviewedOn.Replace(" EST", "");
                                        }

                                        XmlNode Reviewer = ReviewData.SelectSingleNode("Reviewer");
                                        if (Reviewer != null)
                                        {
                                            Review1.ReviewedBy = Reviewer.SelectSingleNode("Username") != null ? Reviewer.SelectSingleNode("Username").InnerText : "";
                                        }
                                        break;
                                    }
                                    Review1.WhichReviewed = "Allergy";
                                    Revieweds.Add(Review1);
                                }
                                //}
                                //else if (AllergyReviews.SelectSingleNode("HasRecords").InnerText == "n")
                                //{
                                //    if (AllergyReviews.SelectSingleNode("Known").InnerText == "y")
                                //    {
                                //        XmlNode AllergyReviewsList = AllergyReviews.SelectSingleNode("ReviewList");
                                //        if (AllergyReviewsList != null)
                                //        {
                                //            Review1.PatientId = MDVUtility.ToLong(model.PatientId);
                                //            XmlNodeList review = AllergyReviewsList.SelectNodes("Review");
                                //            foreach (XmlNode ReviewData in review)
                                //            {

                                //                if (ReviewData.SelectSingleNode("ReviewDate").InnerText != "")
                                //                {
                                //                    Review1.ReviewedOn = ReviewData.SelectSingleNode("ReviewDate").InnerText;
                                //                    Review1.ReviewedOn = Review1.ReviewedOn.Replace(" EST", "");
                                //                }

                                //                XmlNode Reviewer = ReviewData.SelectSingleNode("Reviewer");
                                //                if (Reviewer != null)
                                //                {
                                //                    Review1.ReviewedBy = Reviewer.SelectSingleNode("Username") != null ? Reviewer.SelectSingleNode("Username").InnerText : "";
                                //                }
                                //                break;
                                //            }
                                //            Review1.WhichReviewed = "Allergy";
                                //            Revieweds.Add(Review1);
                                //        }
                                //    }
                                //}
                            }

                            #endregion

                        }

                        return Revieweds;
                    }
                    else
                    {
                        throw new Exception("No Record found in SoftwareCustomerInfo");
                    }
                }
                else
                {
                    throw new Exception("No Record found in SoftwareCustomerInfo");
                }


            }
            catch (Exception ex)
            {
                return Revieweds;
            }


        }
        public List<RcopiaModel> GetRcopiaInfo(SharedVariable sharedVariable = null)
        {
            List<RcopiaModel> RcopiaInfo = new List<RcopiaModel>();
            try
            {
                RcopiaModel model = new RcopiaModel();
                DSRcopia dsRcopia = new DSRcopia();
                BLObject<DSRcopia> obj = BLLRcopiaObj.SelectSoftwareCustomerInfo(MDVApplication.CustomerRegCode, sharedVariable);
                dsRcopia = obj.Data;
                HttpClient client = new HttpClient();
                model.RcopiaANSbackup = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaANSbackupColumn.ColumnName]);
                model.RcopiaScretkey = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaScretkeyColumn.ColumnName]);
                model.RcopiaVendorUsername = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaVendorUsernameColumn.ColumnName]);
                model.RcopiaVendorPassword = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaVendorPasswordColumn.ColumnName]);
                model.RcopiaPortalSystemName = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaPortalSystemNameColumn.ColumnName]);
                model.RcopiaPracticeUserName = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaPracticeUserNameColumn.ColumnName]);
                RcopiaInfo.Add(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RcopiaInfo;
        }
        public string CheckPatientIsRegisteredOnDrFirs(RcopiaModel model)
        {
            try
            {
                BLObject<string> obj = BLLRcopiaObj.IsPatientRegisteredOnDrFirs(MDVUtility.ToLong(model.PatientId));
                if (obj.Data.ToLower() == "no")
                {
                    DSPatient dsPatient = new DSPatient();
                    DSPatient dsDrFirstPatient = new DSPatient();
                    BLObject<DSPatient> objLoad = BLLPatientObj.FillPatient_PictureById(MDVUtility.ToInt64(model.PatientId), "Demographics");
                    dsPatient = objLoad.Data;

                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        dynamic ResponseOfDrFirst = null;

                        #region Update Patient In DrFirst

                        DSPatient.PatientsRow dr = (DSPatient.PatientsRow)dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];

                        MDVision.IEHR.Controls.Clinical.ProblemListHelper objProblemlisthelper = new MDVision.IEHR.Controls.Clinical.ProblemListHelper();
                        ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("Patient", dr, model.PatientId));

                        dsPatient.Patients.Rows[0][dsPatient.Patients.RcopiaIDColumn.ColumnName] = ResponseOfDrFirst.Rcopia;
                        if (ResponseOfDrFirst.Rcopia != "")
                        {
                            DSPatient DatasetPatient = new DSPatient();
                            DSPatient.PatientsRow PatientRow = DatasetPatient.Patients.NewPatientsRow();
                            PatientRow.PatientId = MDVUtility.ToLong(model.PatientId);
                            DatasetPatient.Patients.AddPatientsRow(PatientRow);
                            PatientRow.RcopiaID = ResponseOfDrFirst.Rcopia;
                            BLObject<DSPatient> obj1 = BLLPatientObj.InsertPatientsRcopialID(DatasetPatient);
                            dsDrFirstPatient = obj1.Data;
                            if (obj1.Data != null && dsDrFirstPatient.Tables[dsDrFirstPatient.Patients.TableName].Rows.Count > 0)
                            {
                                obj.Data = "Yes";
                            }
                            RcopiaModel Rcopiamodel = new RcopiaModel();
                            Rcopiamodel.PatientId = model.PatientId;
                            Rcopiamodel.AllergyLastUpdateDate = "";
                            Rcopiamodel.MedicationLastUpdateDate = "";
                            Rcopiamodel.PrescriptionLastUpdateDate = "";
                            RcopiaHelper helperRcopia = new RcopiaHelper();
                            helperRcopia.UpdatePatientLastUpdateInfo(Rcopiamodel);
                        }
                        #endregion




                    }

                }

                if (obj.Data.ToLower() != "yes" || obj.Data.ToLower() != "no")
                {
                    var response = new
                    {
                        status = true,
                        Message = obj.Data.ToLower(),

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


        public string CheckUserHaveRcopiaRights(RcopiaModel model)
        {
            try
            {
                BLObject<string> obj = BLLRcopiaObj.CheckUserHaveRcopiaRights(MDVUtility.ToLong(model.UserId));

                if (obj.Data.ToLower() == "yes" || obj.Data.ToLower() == "no")
                {
                    var response = new
                    {
                        status = true,
                        Message = obj.Data.ToLower(),
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

        public void setRcopiaNotifucationCount(ref BLObject<DSUsers> dsUsers)
        {
            try
            {
                RcopiaModel rcopiaModel = new RcopiaModel();
                rcopiaModel.IsPatientLastUpdateInfo = false;
                DSRcopia dsRcopia = new DSRcopia();
                BLObject<DSRcopia> objrcopia = BLLRcopiaObj.SelectGetUrls();
                dsRcopia = objrcopia.Data;
                if (objrcopia.Data != null)
                {

                    rcopiaModel.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);

                }
                //getting Count of Pending Prescriptions and Prescriptions Refill
                NotificationCountModel NotificationCountModelObj = DownloadNotificationCount(rcopiaModel);
                foreach (DSUsers.EntityUserOptionRow dr in dsUsers.Data.EntityUserOption.Rows)
                {
                    dr.PrescriptionsRefillCount = NotificationCountModelObj.RefillPrescriptionCount.ToString();
                    dr.PendingPrescriptionsCount = NotificationCountModelObj.PendingPrescriptionCount.ToString();
                }
            }
            catch (Exception)
            {
                if (dsUsers != null && dsUsers.Data.EntityUserOption.Rows.Count > 0)
                {
                    foreach (DSUsers.EntityUserOptionRow dr in dsUsers.Data.EntityUserOption.Rows)
                    {
                        dr.PrescriptionsRefillCount = "?";
                        dr.PendingPrescriptionsCount = "?";
                    }
                }
            }
        }

        public void setRcopiaNotificationCount(List<MDVision.Model.User.UserModel> listUsersModels)
        {
            List<MDVision.Model.User.EntityUserOptions> listEntityUserOptions = listUsersModels[0].EntityUserOptions;
            try
            {
                var rcopiaModel = new RcopiaModel { IsPatientLastUpdateInfo = false };
                var objrcopia = BLLRcopiaObj.SelectGetUrls();
                var dsRcopia = objrcopia.Data;
                if (objrcopia.Data != null)
                {
                    rcopiaModel.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);
                }
                var notificationCountModelObj = DownloadNotificationCount(rcopiaModel);
                foreach (var entityUserOptions in listEntityUserOptions)
                {
                    entityUserOptions.PrescriptionsRefillCount = notificationCountModelObj.RefillPrescriptionCount.ToString();
                    entityUserOptions.PendingPrescriptionsCount = notificationCountModelObj.PendingPrescriptionCount.ToString();
                }
            }
            catch (Exception)
            {
                if (listEntityUserOptions != null && listEntityUserOptions.Count > 0)
                {
                    foreach (var entityUserOptions in listEntityUserOptions)
                    {
                        entityUserOptions.PrescriptionsRefillCount = "?";
                        entityUserOptions.PendingPrescriptionsCount = "?";
                    }
                }
            }
        }

        public string GetRcopiaUserName()
        {
            try
            {
                BLObject<string> obj = BLLRcopiaObj.GetRcopiaUserName();
                if (obj.Data != "")
                {
                    var response = new
                    {
                        status = true,
                        UserName = obj.Data,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        UserName = "-1",
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

        public string GetProviderRcopiaUserName(long NotesId)
        {
            try
            {
                BLObject<string> obj = BLLRcopiaObj.GetProviderRcopiaUserName(NotesId);
                if (obj.Data != "")
                {
                    var response = new
                    {
                        status = true,
                        UserName = obj.Data,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        UserName = "-1",
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

        #region New Optimization
        public string DownloadAllClinicalDataFromDrFirst(RcopiaModel model)//Download allergies,medication,prescription,reviewdBy from DrFirst/Recopia
        {
            bool AllergyDownloadSuccessfully = false;
            bool MedicationDownloadSuccessfully = false;
            bool PrescriptionDownloadSuccessfully = false;
            bool IsAllergyDownload = false;
            bool IsMedicationDownload = false;
            bool IsPrescriptionDownload = false;
            bool IsPrescriptionDeleted = false;
            string SavedAllergyIds = "";
            string SavedPrescriptionIds = "";
            string SavedMedicationIds = "";
            Int64 AllergyReviewID = 0;
            Int64 MedicationReviewID = 0;
            bool status = true;
            string message = "All Clinicals Are Download SuccessFully";
            try
            {



                model.IsPatientLastUpdateInfo = false;
                DSRcopia dsRcopia = new DSRcopia();

                List<RcopiaModel> ListRcopia = GetRcopiaInfo();
                model.RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                model.RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                model.RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                model.RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                model.RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                model.RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;

                BLObject<DSRcopia> obj = BLLRcopiaObj.SelectGetUrls();
                dsRcopia = obj.Data;
                model.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);


                RcopiaModel Rcopiamodel = new RcopiaModel();
                Rcopiamodel.PatientId = model.PatientId;
                Rcopiamodel.AllergyLastUpdateDate = "";
                Rcopiamodel.MedicationLastUpdateDate = "";
                Rcopiamodel.PrescriptionLastUpdateDate = "";
                RcopiaHelper helperRcopia = new RcopiaHelper();

                dynamic UpdatePatientLastUpdateInfo = JObject.Parse(helperRcopia.UpdatePatientLastUpdateInfo(Rcopiamodel, true));
                if (UpdatePatientLastUpdateInfo.status == "True")
                {
                    DownloadResponseModel FinalResponse = null;
                    BLObject<DSRcopia> obj1 = BLLRcopiaObj.SelectPatientLastUpdateInfoOp(MDVUtility.ToLong(model.PatientId));
                    dsRcopia = obj1.Data;
                    if (obj1.Data != null)
                    {
                        int LastUpdateRowsCount = (dsRcopia != null && dsRcopia.Tables.Count > 0 && dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName] != null) ? dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows.Count : 0;

                        if (LastUpdateRowsCount > 0)
                        {
                            model.AllergyLastUpdateDate = MDVUtility.ToStr((dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn.ColumnName])) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.AllergyLastUpdateDateColumn.ColumnName]).ToString("MM/dd/yyyy HH:mm:ss")) : "";
                            model.MedicationLastUpdateDate = MDVUtility.ToStr((dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName])) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.MedicationLastUpdateDateColumn.ColumnName]).ToString("MM/dd/yyyy HH:mm:ss")) : "";
                            model.PrescriptionLastUpdateDate = MDVUtility.ToStr((dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn.ColumnName])) != "" ? MDVUtility.ToStr(MDVUtility.ToDateTime(dsRcopia.Tables[dsRcopia.Rcopia_PatientLastUpdateInfo.TableName].Rows[0][dsRcopia.Rcopia_PatientLastUpdateInfo.PrescriptionLastUpdateDateColumn.ColumnName]).ToString("MM/dd/yyyy  HH:mm:ss")) : "";
                        }
                        Dictionary<int, Task<DownloadResponseModel>> tasks = new Dictionary<int, Task<DownloadResponseModel>>();
                        SharedVariable sharedVariableAllergies = SharedVariable.GetSharedVariable();
                        SharedVariable sharedVariableMedication = SharedVariable.GetSharedVariable();
                        SharedVariable sharedVariablePrescription = SharedVariable.GetSharedVariable();
                        SharedVariable sharedVariableReviewed = SharedVariable.GetSharedVariable();
                        string UserNameAllergies = MDVSession.Current.AppUserName;
                        string UserNameMedication = MDVSession.Current.AppUserName;
                        string UserNamePrescription = MDVSession.Current.AppUserName;
                        Int64 UserIdAllergies = MDVSession.Current.AppUserId;
                        Int64 UserIdMedication = MDVSession.Current.AppUserId;
                        Int64 UserIdPrescription = MDVSession.Current.AppUserId;
                        RcopiaModel modelAllergies = new RcopiaModel();
                        RcopiaModel modelMedication = new RcopiaModel();
                        RcopiaModel modelPrescription = new RcopiaModel();
                        RcopiaModel modelReviewed = new RcopiaModel();
                        modelAllergies = model;
                        modelMedication = model;
                        modelPrescription = model;
                        modelReviewed = model;



                        Task<DownloadResponseModel> taskAllegies1 = new Task<DownloadResponseModel>(() => DownloadAllergies(modelAllergies, sharedVariableAllergies, UserNameAllergies, UserIdAllergies));
                        Task<DownloadResponseModel> taskMedication2 = new Task<DownloadResponseModel>(() => DownloadMedications(modelMedication, sharedVariableMedication, UserNameAllergies, UserIdMedication));
                        Task<DownloadResponseModel> taskPrescription3 = new Task<DownloadResponseModel>(() => DownloadPrescription(modelPrescription, sharedVariablePrescription, UserNameAllergies, UserIdPrescription));
                        Task<DownloadResponseModel> taskReviewed4 = new Task<DownloadResponseModel>(() => DownloadReviewed(modelPrescription, sharedVariablePrescription));
                        tasks.Add(1, taskAllegies1);
                        tasks.Add(2, taskMedication2);
                        tasks.Add(3, taskPrescription3);
                        tasks.Add(4, taskReviewed4);

                        foreach (var item in tasks)
                        {
                            if (item.Key == 3)
                                System.Threading.Thread.Sleep(1000);

                            item.Value.Start();
                        }

                        Task.WaitAll(tasks.Values.ToArray());

                        List<DownloadResponseModel> list = tasks.Values.ToList<Task<DownloadResponseModel>>().Select(p => p.Result).ToList<DownloadResponseModel>();
                        foreach (DownloadResponseModel item in list)
                        {
                            if (item.status == false)
                            {
                                status = false;
                                message = item.Message;
                            }
                            if (item.Component == "Allergy")
                            {
                                AllergyDownloadSuccessfully = item.AllergyDownloadSuccessfully;
                                IsAllergyDownload = item.IsAllergyDownload;
                                SavedAllergyIds = item.SavedAllergyIds;
                            }
                            else if (item.Component == "Medication")
                            {
                                MedicationDownloadSuccessfully = item.MedicationDownloadSuccessfully;
                                IsMedicationDownload = item.IsMedicationDownload;
                                SavedMedicationIds = item.SavedMedicationIds;
                            }
                            else if (item.Component == "Prescription")
                            {
                                PrescriptionDownloadSuccessfully = item.PrescriptionDownloadSuccessfully;
                                IsPrescriptionDownload = item.IsPrescriptionDownload;
                                SavedPrescriptionIds = item.SavedPrescriptionIds;
                                IsPrescriptionDeleted = item.IsPrescriptionDeleted;
                            }
                            else if (item.Component == "Reviewed")
                            {
                                AllergyReviewID = item.AllergyReviewID;
                                MedicationReviewID = item.MedicationReviewID;
                            }
                        }


                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                            MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                            PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                            IsAllergyDownload = IsAllergyDownload,
                            IsMedicationDownload = IsMedicationDownload,
                            IsPrescriptionDownload = IsPrescriptionDownload,
                            SavedAllergyIds = SavedAllergyIds,
                            SavedPrescriptionIds = SavedPrescriptionIds,
                            SavedMedicationIds = SavedMedicationIds,
                            Message = obj1.Message,
                            AllergyReviewID = AllergyReviewID,
                            MedicationReviewID = MedicationReviewID,
                            IsPrescriptionDeleted = IsPrescriptionDeleted
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    var response2 = new
                    {
                        status = status,
                        AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                        MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                        PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                        IsAllergyDownload = IsAllergyDownload,
                        IsMedicationDownload = IsMedicationDownload,
                        IsPrescriptionDownload = IsPrescriptionDownload,
                        SavedAllergyIds = SavedAllergyIds,
                        SavedPrescriptionIds = SavedPrescriptionIds,
                        SavedMedicationIds = SavedMedicationIds,
                        Message = message,
                        AllergyReviewID = AllergyReviewID,
                        MedicationReviewID = MedicationReviewID,
                        IsPrescriptionDeleted = IsPrescriptionDeleted
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response2);
                }
                else
                {
                    var response2 = new
                    {
                        status = false,
                        AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                        MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                        PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                        IsAllergyDownload = IsAllergyDownload,
                        IsMedicationDownload = IsMedicationDownload,
                        IsPrescriptionDownload = IsPrescriptionDownload,
                        SavedAllergyIds = SavedAllergyIds,
                        SavedPrescriptionIds = SavedPrescriptionIds,
                        SavedMedicationIds = SavedMedicationIds,
                        Message = "Problem In Downloading",
                        AllergyReviewID = AllergyReviewID,
                        MedicationReviewID = MedicationReviewID,
                        IsPrescriptionDeleted = IsPrescriptionDeleted
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response2);
                }



            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    AllergyDownloadSuccessfully = AllergyDownloadSuccessfully,
                    MedicationDownloadSuccessfully = MedicationDownloadSuccessfully,
                    PrescriptionDownloadSuccessfully = PrescriptionDownloadSuccessfully,
                    IsAllergyDownload = IsAllergyDownload,
                    IsMedicationDownload = IsMedicationDownload,
                    IsPrescriptionDownload = IsPrescriptionDownload,
                    SavedAllergyIds = SavedAllergyIds,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                    AllergyReviewID = AllergyReviewID,
                    MedicationReviewID = MedicationReviewID,
                    IsPrescriptionDeleted = IsPrescriptionDeleted
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public DownloadResponseModel DownloadAllergies(RcopiaModel model, SharedVariable sharedVariable = null, string UserName = null, Int64 UserId=0)
        {
            DownloadResponseModel DownloadResponse = new DownloadResponseModel();
            DownloadResponse.AllergyDownloadSuccessfully = false;
            DownloadResponse.IsAllergyDownload = false;
            DownloadResponse.SavedAllergyIds = "";
            DownloadResponse.Component = "Allergy";
            try
            {
                dynamic DownloadAllergiesResponse = JObject.Parse(DownloadAllergiesAndSave(model, sharedVariable, UserName, UserId));
                if (DownloadAllergiesResponse.DownloadAllergiesCount != 0)
                {
                    DownloadResponse.IsAllergyDownload = true;
                    model.IsPatientLastUpdateInfo = true;
                    if (DownloadAllergiesResponse.status == true)
                    {
                        DownloadResponse.SavedAllergyIds = DownloadAllergiesResponse.SavedAllergyIds;
                        model.AllergyLastUpdateDate = DownloadAllergiesResponse.lastUpdateDate;

                        var responseAllergy = UpdatePatientLastUpdateInfoOp(model, "Allergy", sharedVariable);
                        dynamic Allergyresponse = JObject.Parse(responseAllergy);
                        if (Allergyresponse.status != true)
                        {
                            var response = new DownloadResponseModel
                            {
                                status = false,
                                AllergyDownloadSuccessfully = DownloadResponse.AllergyDownloadSuccessfully,
                                IsAllergyDownload = DownloadResponse.IsAllergyDownload,
                                SavedAllergyIds = DownloadResponse.SavedAllergyIds,
                                Message = Allergyresponse.Message,
                            };
                            return response;
                        }
                        else
                        {
                            DownloadResponse.AllergyDownloadSuccessfully = true;
                        }
                    }
                    else
                    {
                        var response = new DownloadResponseModel
                        {
                            status = false,
                            AllergyDownloadSuccessfully = DownloadResponse.AllergyDownloadSuccessfully,
                            IsAllergyDownload = DownloadResponse.IsAllergyDownload,
                            SavedAllergyIds = DownloadResponse.SavedAllergyIds,
                            Message = DownloadAllergiesResponse.MessageFromSave,
                        };
                        return response;
                    }
                }
                else
                {
                    model.AllergyLastUpdateDate = DownloadAllergiesResponse.lastUpdateDate;
                    var responseAllergy = UpdatePatientLastUpdateInfoOp(model, "Allergy", sharedVariable);
                    dynamic Allergyresponse = JObject.Parse(responseAllergy);
                    if (Allergyresponse.status != true)
                    {
                        var response = new DownloadResponseModel
                        {
                            status = false,
                            AllergyDownloadSuccessfully = DownloadResponse.AllergyDownloadSuccessfully,
                            IsAllergyDownload = DownloadResponse.IsAllergyDownload,
                            SavedAllergyIds = DownloadResponse.SavedAllergyIds,
                            Message = Allergyresponse.Message,
                        };
                        return response;
                    }
                    else
                    {
                        DownloadResponse.AllergyDownloadSuccessfully = true;
                    }
                }

                var Finalresponse = new DownloadResponseModel
                {
                    status = true,
                    AllergyDownloadSuccessfully = DownloadResponse.AllergyDownloadSuccessfully,
                    IsAllergyDownload = DownloadResponse.IsAllergyDownload,
                    SavedAllergyIds = DownloadResponse.SavedAllergyIds,
                    Component = DownloadResponse.Component,
                };
                return Finalresponse;
            }
            catch (Exception ex)
            {
                var Finalresponse = new DownloadResponseModel
                {
                    status = false,
                    AllergyDownloadSuccessfully = DownloadResponse.AllergyDownloadSuccessfully,
                    IsAllergyDownload = DownloadResponse.IsAllergyDownload,
                    SavedAllergyIds = DownloadResponse.SavedAllergyIds,
                    Message = ex.Message,
                };
                return Finalresponse;
            }
        }
        public DownloadResponseModel DownloadMedications(RcopiaModel model, SharedVariable sharedVariable = null, string UserName = null, Int64 UserId=0)
        {
            DownloadResponseModel DownloadResponse = new DownloadResponseModel();
            DownloadResponse.MedicationDownloadSuccessfully = false;
            DownloadResponse.IsMedicationDownload = false;
            DownloadResponse.SavedMedicationIds = "";
            DownloadResponse.Component = "Medication";

            try
            {

                dynamic DownloadMedicationResponse = JObject.Parse(DownloadMadicationsAndSave(model, "", sharedVariable, UserName, "", UserId));

                if (DownloadMedicationResponse.DownloadMedicationCount != 0)
                {
                    DownloadResponse.IsMedicationDownload = true;
                    model.IsPatientLastUpdateInfo = true;
                    if (DownloadMedicationResponse.status == true)
                    {
                        DownloadResponse.SavedMedicationIds = DownloadMedicationResponse.SavedMedicationIds;
                        model.MedicationLastUpdateDate = DownloadMedicationResponse.lastUpdateDate;

                        var responseMedication = UpdatePatientLastUpdateInfoOp(model, "Medication", sharedVariable);
                        dynamic Medicationresponse = JObject.Parse(responseMedication);
                        if (Medicationresponse.status != true)
                        {
                            var response = new DownloadResponseModel
                            {
                                status = false,
                                MedicationDownloadSuccessfully = DownloadResponse.MedicationDownloadSuccessfully,
                                IsMedicationDownload = DownloadResponse.IsMedicationDownload,
                                SavedMedicationIds = DownloadResponse.SavedMedicationIds,
                                Message = Medicationresponse.Message,
                            };
                            return response;
                        }
                        else
                        {
                            DownloadResponse.MedicationDownloadSuccessfully = true;
                        }
                    }
                    else
                    {
                        var response = new DownloadResponseModel
                        {
                            status = false,
                            MedicationDownloadSuccessfully = DownloadResponse.MedicationDownloadSuccessfully,
                            IsMedicationDownload = DownloadResponse.IsMedicationDownload,
                            SavedMedicationIds = DownloadResponse.SavedMedicationIds,
                            Message = DownloadMedicationResponse.MessageFromSave,
                        };
                        return response;
                    }
                }
                else
                {

                    model.MedicationLastUpdateDate = DownloadMedicationResponse.lastUpdateDate;
                    var responseMedication = UpdatePatientLastUpdateInfoOp(model, "Medication", sharedVariable);
                    dynamic Medicationresponse = JObject.Parse(responseMedication);
                    if (Medicationresponse.status != true)
                    {
                        var response = new DownloadResponseModel
                        {
                            status = false,
                            MedicationDownloadSuccessfully = DownloadResponse.MedicationDownloadSuccessfully,
                            IsMedicationDownload = DownloadResponse.IsMedicationDownload,
                            SavedMedicationIds = DownloadResponse.SavedMedicationIds,
                            Message = Medicationresponse.Message,
                        };
                        return response;
                    }
                    else
                    {
                        DownloadResponse.MedicationDownloadSuccessfully = true;
                    }
                }
                var Finalresponse = new DownloadResponseModel
                {
                    status = true,
                    MedicationDownloadSuccessfully = DownloadResponse.MedicationDownloadSuccessfully,
                    IsMedicationDownload = DownloadResponse.IsMedicationDownload,
                    SavedMedicationIds = DownloadResponse.SavedMedicationIds,
                    Component = DownloadResponse.Component,
                };
                return Finalresponse;
            }
            catch (Exception ex)
            {
                var Finalresponse = new DownloadResponseModel
                {
                    status = false,
                    MedicationDownloadSuccessfully = DownloadResponse.MedicationDownloadSuccessfully,
                    IsMedicationDownload = DownloadResponse.IsMedicationDownload,
                    SavedMedicationIds = DownloadResponse.SavedMedicationIds,
                    Message = ex.Message,
                };
                return Finalresponse;
            }
        }
        public DownloadResponseModel DownloadPrescription(RcopiaModel model, SharedVariable sharedVariable = null, string UserName = null, Int64 UserId=0)
        {
            DownloadResponseModel DownloadResponse = new DownloadResponseModel();
            DownloadResponse.PrescriptionDownloadSuccessfully = false;
            DownloadResponse.IsPrescriptionDownload = false;
            DownloadResponse.SavedPrescriptionIds = "";
            DownloadResponse.IsPrescriptionDeleted = false;
            DownloadResponse.Component = "Prescription";
            try
            {
                dynamic DownloadPrescriptionResponse = JObject.Parse(DownloadPriscriptionsAndSave(model, "", sharedVariable, UserName,UserId));

                if (DownloadPrescriptionResponse.DownloadPrescriptionCount != 0)
                {
                    DownloadResponse.IsPrescriptionDownload = true;
                    model.IsPatientLastUpdateInfo = true;
                    if (DownloadPrescriptionResponse.status == true)
                    {
                        DownloadResponse.SavedPrescriptionIds = DownloadPrescriptionResponse.SavedPrescriptionIds;
                        model.PrescriptionLastUpdateDate = DownloadPrescriptionResponse.lastUpdateDate;
                        var responsePrescription = UpdatePatientLastUpdateInfoOp(model, "Prescription", sharedVariable);
                        dynamic Prescriptionresponse = JObject.Parse(responsePrescription);
                        if (Prescriptionresponse.status != true)
                        {
                            var response = new DownloadResponseModel
                            {
                                status = false,
                                PrescriptionDownloadSuccessfully = DownloadResponse.PrescriptionDownloadSuccessfully,
                                IsPrescriptionDownload = DownloadResponse.IsPrescriptionDownload,
                                SavedPrescriptionIds = DownloadResponse.SavedPrescriptionIds,
                                Message = Prescriptionresponse.Message,
                                IsPrescriptionDeleted = DownloadResponse.IsPrescriptionDeleted
                            };
                            return response;
                        }
                        else
                        {
                            if (DownloadPrescriptionResponse.IsPrescriptionDeleted == "true")
                            {
                                DownloadResponse.IsPrescriptionDeleted = true;
                            }

                            DownloadResponse.PrescriptionDownloadSuccessfully = true;
                        }
                    }
                    else
                    {
                        var response = new DownloadResponseModel
                        {
                            status = false,
                            PrescriptionDownloadSuccessfully = DownloadResponse.PrescriptionDownloadSuccessfully,
                            IsPrescriptionDownload = DownloadResponse.IsPrescriptionDownload,
                            SavedPrescriptionIds = DownloadResponse.SavedPrescriptionIds,
                            Message = DownloadPrescriptionResponse.MessageFromSave,
                            IsPrescriptionDeleted = DownloadResponse.IsPrescriptionDeleted
                        };
                        return response;
                    }
                }
                else
                {
                    model.PrescriptionLastUpdateDate = DownloadPrescriptionResponse.lastUpdateDate;
                    var responsePrescription = UpdatePatientLastUpdateInfoOp(model, "Prescription", sharedVariable);
                    dynamic Prescriptionresponse = JObject.Parse(responsePrescription);
                    if (Prescriptionresponse.status != true)
                    {
                        var response = new DownloadResponseModel
                        {
                            status = false,
                            PrescriptionDownloadSuccessfully = DownloadResponse.PrescriptionDownloadSuccessfully,
                            IsPrescriptionDownload = DownloadResponse.IsPrescriptionDownload,
                            SavedPrescriptionIds = DownloadResponse.SavedPrescriptionIds,
                            Message = Prescriptionresponse.Message,
                            IsPrescriptionDeleted = DownloadResponse.IsPrescriptionDeleted
                        };
                        return response;
                    }
                    else
                    {
                        DownloadResponse.PrescriptionDownloadSuccessfully = true;
                    }

                }
                var Finalresponse = new DownloadResponseModel
                {
                    status = true,
                    PrescriptionDownloadSuccessfully = DownloadResponse.PrescriptionDownloadSuccessfully,
                    IsPrescriptionDownload = DownloadResponse.IsPrescriptionDownload,
                    SavedPrescriptionIds = DownloadResponse.SavedPrescriptionIds,
                    IsPrescriptionDeleted = DownloadResponse.IsPrescriptionDeleted,
                    Component = DownloadResponse.Component,
                };
                return Finalresponse;
            }
            catch (Exception ex)
            {
                var response = new DownloadResponseModel
                {
                    status = false,
                    PrescriptionDownloadSuccessfully = DownloadResponse.PrescriptionDownloadSuccessfully,
                    IsPrescriptionDownload = DownloadResponse.IsPrescriptionDownload,
                    SavedPrescriptionIds = DownloadResponse.SavedPrescriptionIds,
                    Message = ex.Message,
                    IsPrescriptionDeleted = DownloadResponse.IsPrescriptionDeleted
                };
                return response;
            }
        }


        public DownloadResponseModel DownloadReviewed(RcopiaModel model, SharedVariable sharedVariable = null)
        {
            DownloadResponseModel DownloadResponse = new DownloadResponseModel();
            DownloadResponse.AllergyReviewID = 0;
            DownloadResponse.MedicationReviewID = 0;
            DownloadResponse.Component = "Reviewed";
            try
            {
                dynamic DownloadReviewsResponse = JObject.Parse(DownloadReviewsAndSave(model, sharedVariable));

                if (DownloadReviewsResponse.DownloadReviewCount != 0)
                {
                    model.IsPatientLastUpdateInfo = true;
                    if (DownloadReviewsResponse.status != true)
                    {
                        var response = new DownloadResponseModel
                        {
                            status = false,
                            Message = DownloadReviewsResponse.Message,
                            AllergyReviewID = DownloadReviewsResponse.AllergyReviewID,
                            MedicationReviewID = DownloadReviewsResponse.MedicationReviewID,
                        };
                        return response;
                    }
                    else
                    {
                        DownloadResponse.AllergyReviewID = DownloadReviewsResponse.AllergyReviewID;
                        DownloadResponse.MedicationReviewID = DownloadReviewsResponse.MedicationReviewID;
                    }
                }
                var Finalresponse = new DownloadResponseModel
                {
                    status = true,
                    AllergyReviewID = DownloadResponse.AllergyReviewID,
                    MedicationReviewID = DownloadResponse.MedicationReviewID,
                    Component = DownloadResponse.Component,
                };
                return Finalresponse;
            }
            catch (Exception ex)
            {
                var response = new DownloadResponseModel
                {
                    status = false,
                    Message = ex.Message,
                    AllergyReviewID = DownloadResponse.AllergyReviewID,
                    MedicationReviewID = DownloadResponse.MedicationReviewID,
                };
                return response;
            }
        }
        #endregion



        public string GetDrugNamesFDB(string DrugName)
        {
            List<DrugModel> Drugs = new List<DrugModel>();
            string errormessage = string.Empty;
            try
            {
                DSRcopia dsRcopia = new DSRcopia();
                RcopiaModel model = new RcopiaModel();
                List<RcopiaModel> ListRcopia = GetRcopiaInfo();
                model.RcopiaANSbackup = ListRcopia[0].RcopiaANSbackup;
                model.RcopiaScretkey = ListRcopia[0].RcopiaScretkey;
                model.RcopiaVendorUsername = ListRcopia[0].RcopiaVendorUsername;
                model.RcopiaVendorPassword = ListRcopia[0].RcopiaVendorPassword;
                model.RcopiaPortalSystemName = ListRcopia[0].RcopiaPortalSystemName;
                model.RcopiaPracticeUserName = ListRcopia[0].RcopiaPracticeUserName;

                BLObject<DSRcopia> obj = BLLRcopiaObj.SelectGetUrls();
                dsRcopia = obj.Data;
                model.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);

                string RcopiaANSbackup = model.RcopiaANSbackup;
                string RcopiaScretkey = model.RcopiaScretkey;
                string RcopiaVendorUsername = model.RcopiaVendorUsername;
                string RcopiaVendorPassword = model.RcopiaVendorPassword;
                string RcopiaPortalSystemName = model.RcopiaPortalSystemName;
                string RcopiaPracticeUserName = model.RcopiaPracticeUserName;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                //modelRcopia.EngineDownloadURL = model.EngineDownloadURL;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));
                var DownloadAllergyXml = MDVUtility.GetXmlForDownloadDrug(RcopiaVendorUsername, RcopiaVendorPassword, RcopiaPortalSystemName, RcopiaPracticeUserName, DrugName);
                var DownloadUrl = model.EngineDownloadURL + "?xml=" + DownloadAllergyXml;
                MDVLogger.RcopiaLogMessage("Request:Download_Medication_From_FDB", model.PatientId, "Successfull", DownloadUrl);
                HttpResponseMessage ResponseData = client.GetAsync(DownloadUrl).Result;
                var downloadMedicationData = ResponseData.Content.ReadAsStringAsync().Result;
                MDVLogger.RcopiaLogMessage("Response1:Download_Medication_From_FDB", model.PatientId, "Successfull", downloadMedicationData);
                if (downloadMedicationData != string.Empty)
                {
                    XmlDocument doc1 = new XmlDocument();
                    doc1.LoadXml(downloadMedicationData);

                    XmlNodeList nodeListError = doc1.GetElementsByTagName("Error");
                    string ErrorText = "";
                    foreach (XmlNode node in nodeListError)
                    {
                        ErrorText = node.SelectSingleNode("Text").InnerText;
                        MDVLogger.RcopiaLogMessage("Response:Download_Medication_From_FDB", model.PatientId, "error", downloadMedicationData, ErrorText);
                        DrugModel objDrug = new DrugModel();
                        objDrug.Message = ErrorText;
                        Drugs.Add(objDrug);
                        var response = new
                        {
                            status = false,
                            DrugsData = js.Serialize(Drugs)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    XmlNodeList nodes = doc1.DocumentElement.SelectNodes("/RCExtResponse/Response/DrugList/Drug");
                    foreach (XmlNode node in nodes)
                    {
                        DrugModel Drug = new DrugModel();
                        Drug.NDCID = node.SelectSingleNode("NDCID") != null ? node.SelectSingleNode("NDCID").InnerText : "";
                        Drug.BrandName = node.SelectSingleNode("BrandName") != null ? node.SelectSingleNode("BrandName").InnerText : "";
                        Drug.GenericName = node.SelectSingleNode("GenericName") != null ? node.SelectSingleNode("GenericName").InnerText : "";
                        Drug.Form = node.SelectSingleNode("Form") != null ? node.SelectSingleNode("Form").InnerText : "";
                        Drug.Strength = node.SelectSingleNode("Strength") != null ? node.SelectSingleNode("Strength").InnerText : "";
                        Drugs.Add(Drug);
                    }
                }
                var response1 = new
                {
                    status = true,
                    DrugsData = js.Serialize(Drugs)
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response1));
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

        public string UploadMedicationOnDrFirst(string AppUserName, long PatientId, string OrderSetId, long notesId, string UploadMedicationIds = "", string MedicationDeleteIds = "", SharedVariable sharedVariable = null, HttpContext hCtrl = null)
        {
            try
            {

                if (isDrFirstRequired == true)
                {
                    //temp
                    #region Add Medication In DrFirst

                    ProblemListHelper problemListHelper = new ProblemListHelper();
                    OS_MedicationHelper MedicationHelper = new OS_MedicationHelper();
                    OS_MedicationModel MedicationModel = new OS_MedicationModel();
                    MedicationModel.OrdersetId = OrderSetId;
                    MedicationModel.OS_MedicationId = "";
                    MedicationModel.pageNumber = "1";
                    MedicationModel.rowsPerPage = "1000";
                    BLObject<List<OS_MedicationModel>> obj = null;
                    if (MedicationDeleteIds != "")
                    {
                        obj = new BLLOrderSet().LoadMedicationForDeleteFromDrFirst(MedicationDeleteIds);
                    }
                    else
                    {
                        obj = new BLLOrderSet().LoadMedication(OrderSetId, UploadMedicationIds, 1, 1000);
                    }

                    List<OS_MedicationModel> model = new List<OS_MedicationModel>();
                    if (obj.Data != null)
                    {
                        model = obj.Data;
                        if (model.Count > 0)
                        {
                            RcopiaModel rcopiaModel = new RcopiaModel();
                            rcopiaModel.PatientId = MDVUtility.ToStr(PatientId);
                            dynamic CheckPatientIsRegisteredOnDrFirst = JObject.Parse(CheckPatientIsRegisteredOnDrFirs(rcopiaModel));//register Patient On DrFirst.
                            if (CheckPatientIsRegisteredOnDrFirst.status == "True")
                            {
                                if (CheckPatientIsRegisteredOnDrFirst.Message == "yes")
                                {
                                    var ProviderUsername = "sprovider47";
                                    RcopiaHelper helperRcopia = new RcopiaHelper();
                                    DSRcopia dsRcopia = new DSRcopia();
                                    BLObject<DSRcopia> objGetUrl = new BLLRcopia().SelectGetUrls();
                                    dsRcopia = objGetUrl.Data;
                                    if (objGetUrl.Data != null)
                                    {
                                        if (dsRcopia.Rcopia_GetUrl != null && dsRcopia.Rcopia_GetUrl.Rows.Count > 0)
                                        {
                                            if (MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineUploadURLColumn.ColumnName]) != "https://engine201.staging.drfirst.com/servlet/rcopia.servlet.EngineServlet")
                                            {
                                                dynamic GetProviderRcopiaUserNameResponse = JObject.Parse(helperRcopia.GetProviderRcopiaUserName(notesId));

                                                if (GetProviderRcopiaUserNameResponse.status == "True")
                                                {
                                                    if (GetProviderRcopiaUserNameResponse.UserName != "-1")
                                                    {
                                                        ProviderUsername = GetProviderRcopiaUserNameResponse.UserName.ToString();
                                                    }
                                                    else
                                                    {
                                                        ProviderUsername = "";
                                                    }
                                                }
                                                else
                                                {
                                                    ProviderUsername = "";
                                                }
                                            }
                                            else
                                            {
                                                ProviderUsername = "sprovider47";
                                            }

                                        }
                                        else
                                        {
                                            ProviderUsername = "";
                                        }

                                    }
                                    else
                                    {
                                        ProviderUsername = "";
                                    }

                                    if (ProviderUsername != "")
                                    {
                                        dynamic ResponseOfDrFirst = new System.Dynamic.ExpandoObject();
                                        //Start Upload Medication On drFirst
                                        if (MedicationDeleteIds != "")
                                        {
                                            ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("MedicationDelete", null, "1", sharedVariable, hCtrl, model, ProviderUsername, PatientId));
                                        }
                                        else
                                        {
                                            ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("Medication", null, "1", sharedVariable, hCtrl, model, ProviderUsername, PatientId));
                                        }
                                        //End Upload Medication On drFirst

                                        // SaveProblemInDrFirst(dr, problemID);
                                        if (ResponseOfDrFirst.Rcopia != "")
                                        {

                                            //Download Medications
                                            dynamic ResponseOfDownloadMedications = JObject.Parse(DownloadMedications(rcopiaModel, OrderSetId, true));
                                            if (ResponseOfDownloadMedications.status == "true")
                                            {
                                                if (ResponseOfDownloadMedications.MedicationDownloadSuccessfully == "true" && ResponseOfDownloadMedications.IsMedicationDownload == "true")
                                                {

                                                    var responseRcopiaerror = new
                                                    {
                                                        status = true,
                                                        SavedMedicationIds = ResponseOfDownloadMedications.SavedMedicationIds,
                                                    };
                                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                                                }
                                                else
                                                {
                                                    var responseRcopiaerror = new
                                                    {
                                                        status = false,
                                                        Message = "Problem in Download Medication from DrFirst"
                                                    };
                                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                                                }

                                            }
                                            else
                                            {
                                                var responseRcopiaerror = new
                                                {
                                                    status = false,
                                                    Message = "Problem in Download Medication from DrFirst"
                                                };
                                                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                                            }



                                        }
                                        else
                                        {
                                            var responseRcopiaerror = new
                                            {
                                                status = false,
                                                Message = "Problem in Add Problem on DrFirst"
                                            };
                                            return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));

                                        }
                                    }
                                    else
                                    {
                                        var responseRcopiaerror = new
                                        {
                                            status = false,
                                            Message = "Provider UserName Is Not Valid"
                                        };
                                        return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                                    }
                                }
                                else
                                {
                                    var responseRcopiaerror = new
                                    {
                                        status = false,
                                        SavedMedicationIds = "Problem Patient Registeration"
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                                }
                            }
                            else
                            {
                                var responseRcopiaerror = new
                                {
                                    status = false,
                                    SavedMedicationIds = "Problem Patient Registeration"
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                            }
                        }
                        else
                        {
                            var responseRcopiaerror = new
                            {
                                status = true,
                                SavedMedicationIds = ""
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                        }
                    }
                    else
                    {
                        var responseRcopiaerror = new
                        {
                            status = false,
                            Message = "Problem In Load Medication"
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                    }
                    #endregion
                }
                else
                {
                    var responseRcopiaerror = new
                    {
                        status = true,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                }




            }
            catch (Exception ex)
            {
                throw ex;
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string DeleteMedicationFromDrFirst(string AppUserName, long PatientId, string OrderSetId, long notesId, string MedicationDeleteIds = "", SharedVariable sharedVariable = null, HttpContext hCtrl = null)
        {
            try
            {

                if (isDrFirstRequired == true)
                {
                    //temp
                    #region Add Medication In DrFirst

                    ProblemListHelper problemListHelper = new ProblemListHelper();
                    OS_MedicationHelper MedicationHelper = new OS_MedicationHelper();
                    OS_MedicationModel MedicationModel = new OS_MedicationModel();
                    MedicationModel.OrdersetId = OrderSetId;
                    MedicationModel.OS_MedicationId = "";
                    MedicationModel.pageNumber = "1";
                    MedicationModel.rowsPerPage = "1000";
                    BLObject<List<OS_MedicationModel>> obj = new BLLOrderSet().LoadMedicationForDeleteFromDrFirst(MedicationDeleteIds);


                    List<OS_MedicationModel> model = new List<OS_MedicationModel>();
                    if (obj.Data != null)
                    {
                        model = obj.Data;
                        if (model.Count > 0)
                        {
                            RcopiaModel rcopiaModel = new RcopiaModel();
                            rcopiaModel.PatientId = MDVUtility.ToStr(PatientId);
                            var ProviderUsername = "sprovider47";
                            RcopiaHelper helperRcopia = new RcopiaHelper();
                            DSRcopia dsRcopia = new DSRcopia();
                            BLObject<DSRcopia> objGetUrl = new BLLRcopia().SelectGetUrls();
                            dsRcopia = objGetUrl.Data;
                            if (objGetUrl.Data != null)
                            {
                                if (dsRcopia.Rcopia_GetUrl != null && dsRcopia.Rcopia_GetUrl.Rows.Count > 0)
                                {
                                    if (MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineUploadURLColumn.ColumnName]) != "https://engine201.staging.drfirst.com/servlet/rcopia.servlet.EngineServlet")
                                    {
                                        dynamic GetProviderRcopiaUserNameResponse = JObject.Parse(helperRcopia.GetProviderRcopiaUserName(notesId));

                                        if (GetProviderRcopiaUserNameResponse.status == "True")
                                        {
                                            if (GetProviderRcopiaUserNameResponse.UserName != "-1")
                                            {
                                                ProviderUsername = GetProviderRcopiaUserNameResponse.UserName.ToString();
                                            }
                                            else
                                            {
                                                ProviderUsername = "";
                                            }
                                        }
                                        else
                                        {
                                            ProviderUsername = "";
                                        }
                                    }
                                    else
                                    {
                                        ProviderUsername = "sprovider47";
                                    }

                                }
                                else
                                {
                                    ProviderUsername = "";
                                }

                            }
                            else
                            {
                                ProviderUsername = "";
                            }

                            if (ProviderUsername != "")
                            {
                                //Start Upload Medication On drFirst
                                dynamic ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("MedicationDelete", null, "1", sharedVariable, hCtrl, model, ProviderUsername, PatientId));
                                //End Upload Medication On drFirst

                                // SaveProblemInDrFirst(dr, problemID);
                                if (ResponseOfDrFirst.Rcopia != "")
                                {

                                    //Download Medications
                                    dynamic ResponseOfDownloadMedications = JObject.Parse(DownloadMedications(rcopiaModel, OrderSetId, true));
                                    if (ResponseOfDownloadMedications.status == "true")
                                    {
                                        if (ResponseOfDownloadMedications.MedicationDownloadSuccessfully == "true" && ResponseOfDownloadMedications.IsMedicationDownload == "true")
                                        {

                                            var responseRcopiaerror = new
                                            {
                                                status = true,
                                                SavedMedicationIds = ResponseOfDownloadMedications.SavedMedicationIds,
                                            };
                                            return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                                        }
                                        else
                                        {
                                            var responseRcopiaerror = new
                                            {
                                                status = false,
                                                Message = "Problem in Download Medication from DrFirst"
                                            };
                                            return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                                        }

                                    }
                                    else
                                    {
                                        var responseRcopiaerror = new
                                        {
                                            status = false,
                                            Message = "Problem in Download Medication from DrFirst"
                                        };
                                        return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                                    }



                                }
                                else
                                {
                                    var responseRcopiaerror = new
                                    {
                                        status = false,
                                        Message = "Problem in Add Problem on DrFirst"
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));

                                }
                            }
                            else
                            {
                                var responseRcopiaerror = new
                                {
                                    status = false,
                                    Message = "Provider UserName Is Not Valid"
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                            }

                        }
                        else
                        {
                            var responseRcopiaerror = new
                            {
                                status = true,
                                SavedMedicationIds = ""
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                        }
                    }
                    else
                    {
                        var responseRcopiaerror = new
                        {
                            status = false,
                            Message = "Problem In Load Medication"
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                    }
                    #endregion
                }
                else
                {
                    var responseRcopiaerror = new
                    {
                        status = true,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseRcopiaerror));
                }




            }
            catch (Exception ex)
            {
                throw ex;
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }




    }
}