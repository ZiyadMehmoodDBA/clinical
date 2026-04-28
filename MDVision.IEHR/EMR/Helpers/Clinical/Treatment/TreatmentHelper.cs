using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.Model.Clinical.Treatment;
using MDVision.Model.Lookups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MDVision.IEHR.Common.MDVisionLookups;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Treatment
{

    public class TreatmentHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public TreatmentHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }

        private static TreatmentHelper _instance = null;
        public static TreatmentHelper Instance()
        {
            if (_instance == null)
                _instance = new TreatmentHelper();
            return _instance;
        }

        public string SaveTreatment(TreatmentPlanModel model)
        {
            try
            {
                #region Make Delete treatment data
                DataTable dtDeleteTreatmentsIds = new DataTable();
                DataColumn COLUMN = new DataColumn();
                COLUMN.ColumnName = "Id";
                COLUMN.DataType = typeof(int);
                dtDeleteTreatmentsIds.Columns.Add(COLUMN);



                if (model.DeleteTreatments.Count>0)
                {
                    foreach (TreatmentComponentModel a in model.DeleteTreatments)
                    {
                        DataRow Dr = dtDeleteTreatmentsIds.NewRow();
                        Dr[0] = a.TreatmentId;
                        dtDeleteTreatmentsIds.Rows.Add(Dr);
                    }
                }
                else
                {
                    DataRow Dr = dtDeleteTreatmentsIds.NewRow();
                    Dr[0] = -1;
                    dtDeleteTreatmentsIds.Rows.Add(Dr);
                }

                #endregion

                List<SaveTreatmentModel> TreatmentList = new List<SaveTreatmentModel>();
                BLObject<List<NoteComponentsLookupModel>> Componentmodel = new BLLClinical().LookupNoteComponents();
                List<NoteComponentsLookupModel> NoteComponents = Componentmodel.Data;
                BLObject<TreatementSoapTextDataModel> ObjTreatmentMapping = BLLClinicalObj.GetTreatmentMapping(MDVUtility.ToInt64(model.NoteId));
                TreatementSoapTextDataModel TreatMapping = null;
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                if (ObjTreatmentMapping.Data != null)
                {
                    TreatMapping = new TreatementSoapTextDataModel();
                    TreatMapping = ObjTreatmentMapping.Data;
                }
                else
                {

                }
                foreach (TreatmentComponentModel tre in model.Treatments)
                {
                    if (!string.IsNullOrEmpty(tre.PrescriptionIds))
                    {
                        AddTreatment(tre.PrescriptionIds, "Prescription", tre, TreatMapping, NoteComponents, TreatmentList);
                    }
                    if (!string.IsNullOrEmpty(tre.LabOrderIds))
                    {
                        AddTreatment(tre.LabOrderIds, "Lab Orders", tre, TreatMapping, NoteComponents, TreatmentList);
                    }
                    if (!string.IsNullOrEmpty(tre.DiagnosticImagingIds))
                    {
                        AddTreatment(tre.DiagnosticImagingIds, "Diagnostic Imaging Order", tre, TreatMapping, NoteComponents, TreatmentList);
                    }

                    if (!string.IsNullOrEmpty(tre.ProcedureIds))
                    {
                        AddTreatment(tre.ProcedureIds, "Procedures", tre, TreatMapping, NoteComponents, TreatmentList);
                    }
                    if (!string.IsNullOrEmpty(tre.ImmunizationIds))
                    {
                        AddTreatment(tre.ImmunizationIds, "Immunization", tre, TreatMapping, NoteComponents, TreatmentList);
                    }
                    if (!string.IsNullOrEmpty(tre.TherapeuticIds))
                    {
                        AddTreatment(tre.TherapeuticIds, "Therapeutic", tre, TreatMapping, NoteComponents, TreatmentList);
                    }
                    if (!string.IsNullOrEmpty(tre.ReferralIds))
                    {
                        AddTreatment(tre.ReferralIds, "Referrals", tre, TreatMapping, NoteComponents, TreatmentList);
                    }
                }
                var XML = MDVUtility.GetXmlOfObject(typeof(List<SaveTreatmentModel>), TreatmentList);

                string UserName = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                DateTime Date = DateTime.Now;
                #region DBInsertion
                BLObject<TreatementSoapTextDataModel> resultSet = BLLClinicalObj.SaveTreatment(MDVUtility.ToInt64(model.NoteId), XML, model.Comment, dtDeleteTreatmentsIds, UserName, Date, UserName, Date);
                if (resultSet.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        TreatmentData = resultSet.Data
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Some Error Occured."
                    };
                    return (JsonConvert.SerializeObject(response));
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
        private void AddTreatment(string ComponentIds,string ComponentName,TreatmentComponentModel obj, TreatementSoapTextDataModel TreatMapping, List<NoteComponentsLookupModel> NoteComponents, List<SaveTreatmentModel> TreatmentList)
        {
            foreach (string PreId in ComponentIds.Split(',').ToList<string>())
            {
                SaveTreatmentModel treatment = new SaveTreatmentModel();
                treatment.ProblemId = obj.ProblemId;
                treatment.TreatmentId = obj.TreatmentId;
                treatment.NoteComponentsLookupId = GetTreatmentComponentId(NoteComponents, ComponentName);
                treatment.TreatmentDataId = GetTreatmentDataId(TreatMapping, obj.TreatmentId, treatment.NoteComponentsLookupId, PreId);
                treatment.DataId = PreId;
                treatment.Comment = obj.Comments;
                TreatmentList.Add(treatment);
            }
        }
        private string GetTreatmentDataId(TreatementSoapTextDataModel obj, string TreatmentId, string ComponentId, string DataId)
        {
            if (obj.TreatmentDataList.Count > 0)
            {
                List<TreatmentData> filteredList = obj.TreatmentDataList.Where(x => x.TreatmentId == TreatmentId && x.ComponentId == ComponentId && x.DataId == DataId).ToList();
                if (filteredList.Count > 0)
                {
                    return filteredList[0].TreatmentDataId;
                }
                else
                {
                    return "-1";
                }
            }
            else
            {
                return "-1";
            }
        }
        private string GetTreatmentComponentId(List<NoteComponentsLookupModel> Data, string ComponentName)
        {
            string ComponentId = "-1";
            foreach (NoteComponentsLookupModel item in Data)
            {
                if (item.NoteComponentName.ToLower() == ComponentName.ToLower())
                {
                    ComponentId = item.NoteComponentId;
                }
            }
            return ComponentId;
        }
        public string detachTreatment(long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachTreatment(notesId);
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
        public string LoadTreatment(long NoteId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(NoteId)))
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
                    BLObject<TreatementSoapTextDataModel> ObjTreatmentMapping = BLLClinicalObj.GetTreatmentMapping(MDVUtility.ToInt64(NoteId));
                    if (ObjTreatmentMapping.Data != null)
                    {
                        var response = new
                        {
                            TreatmentData= ObjTreatmentMapping.Data,
                            status = true
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ObjTreatmentMapping.Data
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
    }
}