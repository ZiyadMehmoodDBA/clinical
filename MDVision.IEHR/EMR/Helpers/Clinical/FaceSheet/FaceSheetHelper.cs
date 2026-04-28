// Author:  Muhammad Arshad
// Created Date: 12/10/2015
//OverView: Helper class for FaceSheet
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.EMR.Model.FaceSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Text;
using MDVision.Model.FaceSheet;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Model.Common;

namespace MDVision.IEHR.EMR.Helpers.Clinical.FaceSheet
{
    public class FaceSheetHelper
    {
        private BLLClinical BLLClinicalObj = null;
        private BLLSchedule BLLScheduleObj = null;
        public FaceSheetHelper()
        {
            BLLClinicalObj = new BLLClinical();
            BLLScheduleObj = new BLLSchedule();
        }
        private static FaceSheetHelper _instance = null;
        public static FaceSheetHelper Instance()
        {
            if (_instance == null)
                _instance = new FaceSheetHelper();
            return _instance;
        }

        #region saveFaceSheet

        // Author:  Muhammad Arshad
        // Created Date: 12/15/2015
        //OverView: This function will handle save of FaceSheet for current logged in user
        public string saveFaceSheet(FaceSheetModel model)
        {
            try
            {
                DSFaceSheet dsFaceSheet = new DSFaceSheet();

                DSFaceSheet.FaceSheetRow dr = dsFaceSheet.FaceSheet.NewFaceSheetRow();

                dr.UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);

                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsFaceSheet.FaceSheet.AddFaceSheetRow(dr);
                BLObject<DSFaceSheet> obj = BLLClinicalObj.insertFaceSheet(dsFaceSheet);
                dsFaceSheet = obj.Data;
                //if (obj.Data != null)
                //{
                //    Int64 FaceSheetId = MDVUtility.ToInt64(dsFaceSheet.Tables[dsFaceSheet.FaceSheet.TableName].Rows[0][dsFaceSheet.FaceSheet.FaceSheetIdColumn.ColumnName]);
                //    if (FaceSheetId > 0)
                //    {
                //        obj = BLLClinicalObj.loadFaceSheet(0, MDVUtility.ToInt64(Common.AppConfig.AppUserId), 0);
                //    }

                //}
                if (obj.Data != null)
                {
                    //dsFaceSheet = obj.Data;
                    var response = new
                    {
                        //status = true,
                        //message = Common.AppPrivileges.Save_Message,
                        status = true,
                        FaceSheetCount = dsFaceSheet.Tables[dsFaceSheet.FaceSheet.TableName].Rows.Count,
                        FaceSheetLoad_JSON = MDVUtility.JSON_DataTable(dsFaceSheet.Tables[dsFaceSheet.FaceSheet.TableName]),
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
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region fillFaceSheet

        public string loadFaceSheetAllergies(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSAllergyModel> listAllergies = new List<FSAllergyModel>();
                BLObject<List<FSAllergyModel>> obj = BLLClinicalObj.loadFaceSheetAllergies(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listAllergies = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listAllergies));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string loadFaceSheetProblemList(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSProblemListModel> listProblemList = new List<FSProblemListModel>();
                BLObject<List<FSProblemListModel>> obj = BLLClinicalObj.loadFaceSheetProblemList(shared_model, MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listProblemList = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listProblemList));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string loadFaceSheetVitals(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSVitalsModel> listVitals = new List<FSVitalsModel>();
                BLObject<List<FSVitalsModel>> obj = BLLClinicalObj.loadFaceSheetVitals(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listVitals = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listVitals));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string loadFaceSheetNotes(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSNotesModel> listNotes = new List<FSNotesModel>();
                BLObject<List<FSNotesModel>> obj = BLLClinicalObj.loadFaceSheetNotes(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listNotes = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listNotes));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string loadFaceSheetAppointmentsNew(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSAppointmentModel> listAppointments = new List<FSAppointmentModel>();
                BLObject<List<FSAppointmentModel>> obj = BLLClinicalObj.loadFaceSheetAppointments(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listAppointments = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listAppointments));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string loadFaceSheetHistory(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSHistoryModel> listHistory = new List<FSHistoryModel>();
                BLObject<List<FSHistoryModel>> obj = BLLClinicalObj.loadFaceSheetHistory(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listHistory = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listHistory));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string loadFaceSheetLabResult(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSLabResultsModel> listLabResult = new List<FSLabResultsModel>();
                BLObject<List<FSLabResultsModel>> obj = BLLClinicalObj.loadFaceSheetLabResult(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listLabResult = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listLabResult));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string loadFaceSheetLabOrder(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSLabOrdersModel> listLabOrder = new List<FSLabOrdersModel>();
                BLObject<List<FSLabOrdersModel>> obj = BLLClinicalObj.loadFaceSheetLabOrder(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listLabOrder = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listLabOrder));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string loadFaceSheetProcedureOrder(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSProcedureOrderModel> listProcedureOrder = new List<FSProcedureOrderModel>();
                BLObject<List<FSProcedureOrderModel>> obj = BLLClinicalObj.loadFaceSheetProcedureOrder(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listProcedureOrder = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listProcedureOrder));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string loadFaceSheetRadiologyOrder(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSRadiologyOrdersModel> listRadiologyOrder = new List<FSRadiologyOrdersModel>();
                BLObject<List<FSRadiologyOrdersModel>> obj = BLLClinicalObj.loadFaceSheetRadiologyOrder(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listRadiologyOrder = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listRadiologyOrder));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string loadFaceSheetMedications(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSMedicationsModel> listMedications = new List<FSMedicationsModel>();
                BLObject<List<FSMedicationsModel>> obj = BLLClinicalObj.loadFaceSheetMedications(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listMedications = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listMedications));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string loadFaceSheetReferrals(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSReferralsModel> listReferrals = new List<FSReferralsModel>();
                BLObject<List<FSReferralsModel>> obj = BLLClinicalObj.loadFaceSheetReferrals(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listReferrals = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listReferrals));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string loadFaceSheetImmunization(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSImmunizationsModel> listImmunization = new List<FSImmunizationsModel>();
                BLObject<List<FSImmunizationsModel>> obj = BLLClinicalObj.loadFaceSheetImmunization(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listImmunization = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listImmunization));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string loadFaceSheetComplaints(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSComplaintsModel> listComplaints = new List<FSComplaintsModel>();
                BLObject<List<FSComplaintsModel>> obj = BLLClinicalObj.loadFaceSheetComplaints(shared_model,MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listComplaints = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listComplaints));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string loadFaceSheetPatientDocument(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSPatientDocumentModel> listPatientDocument = new List<FSPatientDocumentModel>();
                BLObject<List<FSPatientDocumentModel>> obj = BLLClinicalObj.loadFaceSheetPatientDocument(shared_model, MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listPatientDocument = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listPatientDocument));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string loadFaceSheetImplantableDevices(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FSImplantableDevicesModel> listImplantableDevices = new List<FSImplantableDevicesModel>();
                BLObject<List<FSImplantableDevicesModel>> obj = BLLClinicalObj.loadFaceSheetImplantableDevices(shared_model, MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    listImplantableDevices = obj.Data;
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(listImplantableDevices));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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

        #region UpdateComponentOrder

        public string LoadFaceSheet(SharedModel shared_model, FaceSheetModel model)
        {
            try
            {
                List<FaceSheetOrderModel> listFSOrder = new List<FaceSheetOrderModel>();

                BLObject<List<FaceSheetOrderModel>> obj = new BLLClinical().loadFaceSheetNew(shared_model, MDVUtility.ToInt64(0), MDVUtility.ToInt64(model.PatientId));
                if (obj.Data != null)
                {
                    var response = new
                    {
                        listFSOrder = obj.Data,
                        FaceSheetCount = obj.Data.Count
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string UpdateComponentOrderSorting(FaceSheetModel model)
        {
            try
            {
                BLObject<string> result;
                result = BLLClinicalObj.updateFaceSheet(model.FaceSheetComponentSorted);
                var response = new
                {
                    status = true,
                    message = result
                };

                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /*
        Author: Muhammad Irfan
        Date: 21/12/2015
        Overview: This function is created to call server side 
        */
        public string loadFaceSheetAppointments(FaceSheetModel model)
        {
            try
            {
                DSAppointment dsFaceSheetApp = null;
                BLObject<DSAppointment> obj;

                obj = BLLScheduleObj.LoadAppointmentsVisits(0, 0, 0, "", "", "", "", "", "0", MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "", MDVUtility.ToInt64(model.PatientId), "");

                dsFaceSheetApp = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        AppointmentsCount = dsFaceSheetApp.Tables[dsFaceSheetApp.AppointmentsVisits.TableName].Rows.Count,
                        AppointmentsLoad_JSON = MDVUtility.JSON_DataTable(dsFaceSheetApp.Tables[dsFaceSheetApp.AppointmentsVisits.TableName]),
                        iTotalDisplayRecords = (dsFaceSheetApp.AppointmentsVisits.Rows.Count > 0) ? dsFaceSheetApp.AppointmentsVisits.Rows[0][dsFaceSheetApp.AppointmentsVisits.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }



        #endregion

        #region "PDF View of FaceSheet"
        /**
         * Author: Muhammad Irfan
         * Date: 06/01/2016
         * Overview: This function will load pdf for face sheet
         * **/
        public string previewFaceSheet(FaceSheetModel model)
        {
            try
            {
                DSFaceSheet dsFaceSheet = null;
                BLObject<byte[]> objLoad = BLLClinicalObj.previewFaceSheet(MDVUtility.ToInt64(MDVSession.Current.AppUserId), MDVUtility.ToInt64(model.PatientId));


                if (objLoad.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetHTML = Convert.ToBase64String(objLoad.Data),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
                        Message = objLoad.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string LoadClinicalSummaryPrintComponents(string PatientId, List<string> PrintComponents)
        {
            try
            {               
                BLObject<byte[]> objLoad = BLLClinicalObj.ClinicalSummaryCustomizePreview(MDVUtility.ToInt64(PatientId), PrintComponents);


                if (objLoad.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetHTML = Convert.ToBase64String(objLoad.Data),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
                        Message = objLoad.Message
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
    }
}