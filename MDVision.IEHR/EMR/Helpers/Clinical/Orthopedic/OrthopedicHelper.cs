using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Model.FavoriteList;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Model.Clinical.Orthopedic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MDVision.IEHR.Controls.DashBoard.DashBoard;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Orthopedic
{
    public class OrthopedicHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public OrthopedicHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }

        private static OrthopedicHelper _instance = null;
        public static OrthopedicHelper Instance()
        {
            if (_instance == null)
                _instance = new OrthopedicHelper();
            return _instance;
        }
        public string LoadOrthopedicFavoriteList(OrthoFavListModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.ListType) && model.ProviderId > 0)
                {
                    BLObject<List<OrthoFavListModel>> obj = BLLClinicalObj.LoadOrthopedicFavoriteList(model.ListType, model.ProviderId, model.FavListBodyPartIds);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            OrthopedicFavList_JSON = obj.Data,
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = AppPrivileges.Load_Favorities_Error_Message
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
        public string loadBodyPartsByNotes(OrthopedicModel model)
        {
            try
            {
                BLObject<List<BodyPartModel>> obj = BLLClinicalObj.LoadBodyPartsLookUp(model.NotesId);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        OrthopedicChat_JSON = obj.Data,
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

        public string loadBodyParts()
        {
            try
            {
                BLObject<List<BodyPartModel>> obj = BLLClinicalObj.LoadBodyPartsLookUp();
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        OrthopedicChat_JSON = obj.Data,
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

        public string loadOrthopedicComplaints()
        {
            try
            {
                List<OrthopedicComplaintsModel> ModelList = BLLClinicalObj.LoadOrthopedicComplaints();
                if (ModelList.Count >= 0)
                {
                    var response = new
                    {
                        status = true,
                        OrthopedicComplaints_JSON = ModelList,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        OrthopedicComplaints_JSON = "[]",
                        Message = Common.AppPrivileges.No_Record_Message
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

        public string saveOrthopedicComplaints(string Complaint)
        {
            try
            {
                BLObject<string> obj = BLLClinicalObj.SaveOrthopedicComplaints(Complaint);
                if (obj.Data != null && obj.Data != "-1")
                {
                    var response = new
                    {
                        status = true,
                        OrthopedicComplainId = obj.Data.ToString(),
                        Message = Common.AppPrivileges.Save_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message == "" ? "Complaint already exist" : obj.Message
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

        public string deleteOrthopedicComplaints(string OrthopedicComplainId)
        {
            try
            {
                BLObject<string> obj = BLLClinicalObj.DeleteOrthopedicComplaints(OrthopedicComplainId);
                if (obj.Data != null && obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,
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

        public string saveBodyPartAndComplaint(OrthopedicModel model)
        {
            try
            {
                DSClinicalComplaint dsComplaint = new DSClinicalComplaint();

                // Complaint
                DSClinicalComplaint.ComplaintRow dr = dsComplaint.Complaint.NewComplaintRow();
                dr.ComplaintId = model.ComplaintId;
                dr.DateCaptured = DateTime.Now;
                dr.PatientId = MDVUtility.ToLong(model.PatientId);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.NotesId = MDVUtility.ToLong(model.NotesId);
                dsComplaint.Complaint.AddComplaintRow(dr);

                // Notes Complaints
                DSClinicalComplaint.NotesComplaintRow NotesComplaint = dsComplaint.NotesComplaint.NewNotesComplaintRow();
                NotesComplaint.NotesComplaintId = -1;
                NotesComplaint.NotesId = MDVUtility.ToLong(model.NotesId);
                dsComplaint.NotesComplaint.AddNotesComplaintRow(NotesComplaint);

                // Complaint Detail
                foreach (var item in model.Complaints)
                {
                    DSClinicalComplaint.ComplaintDetailRow complaintDetail = dsComplaint.ComplaintDetail.NewComplaintDetailRow();
                    complaintDetail.ComplaintDescription = item.ComplaintDescription;
                    complaintDetail.ComplaintDetailId = item.ComplaintDetailId;
                    complaintDetail.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    complaintDetail.CreatedOn = DateTime.Now;
                    complaintDetail.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    complaintDetail.ModifiedOn = DateTime.Now;
                    complaintDetail.IsChiefComplaint = false;
                    complaintDetail.IsReported = true;
                    complaintDetail.IsBodyPart = true;
                    dsComplaint.ComplaintDetail.AddComplaintDetailRow(complaintDetail);
                }
                BLObject<DSClinicalComplaint> obj = BLLClinicalObj.InsertComplaintForOrthopedic(dsComplaint,model.NotesId);
                if (obj.Data != null)
                {
                    BLObject<string> res = BLLClinicalObj.SaveNotesBodyPart(model.BodyPart, model.NotesId);
                    if (res.Data != null && res.Data != "-1")
                    {
                        var data = obj.Data;
                        BLObject<List<Complaints>> obj_ = BLLClinicalObj.NotesComplaintsSelect(model.NotesId, model.PatientId);
                        if (obj_.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                NotesBodyPartId = obj.Data.ToString(),
                                ComplaintId = obj_.Data.FirstOrDefault().ComplaintId,
                                ComplaintDetail_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(obj_.Data),
                                Message = Common.AppPrivileges.Save_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                NotesBodyPartId = obj.Data.ToString(),
                                ComplaintId = ((DSClinicalComplaint.ComplaintRow)obj.Data.Complaint.Rows[0]).ComplaintId,
                                ComplaintDetail_JSON = MDVUtility.JSON_DataTable(obj.Data.ComplaintDetail),
                                Message = Common.AppPrivileges.Save_Message
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

        public string deleteBodyPartAndComplaint(OrthopedicModel model)
        {
            try
            {
                if (model.ComplaintDetailId <= 0 || model.NotesId <= 0 || model.NotesBodyPartId <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.No_Record_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                BLObject<string> res = BLLClinicalObj.DeleteNotesBodyPart(model.ComplaintDetailId, model.NotesBodyPartId, model.NotesId, model.IsDeleteBodyPartAssociation);

                if (res.Data != null && res.Data != "-1")
                {
                    BLObject<List<Complaints>> obj_ = BLLClinicalObj.NotesComplaintsSelect(model.NotesId, model.PatientId);
                    List<Complaints> list = new List<Complaints>();
                    if (obj_.Data != null)
                    {
                        list = obj_.Data;
                    }


                    var response = new
                    {
                        status = true,
                        ComplaintId = list.FirstOrDefault() != null ? list.FirstOrDefault().ComplaintId : 0,
                        ComplaintDetail_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(list),
                        Message = Common.AppPrivileges.Delete_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = res.Message
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

        public string loadNotesBodyPartsComplaints(long NotesId)
        {
            try
            {
                List<OrthopedicComplaintsModel> ModelList = BLLClinicalObj.LoadNotesBodyPartsComplaints(NotesId);
                if (ModelList.Count >= 0)
                {
                    var response = new
                    {
                        status = true,
                        NotesBodyPartsComplaints_JSON = ModelList,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        NotesBodyPartsComplaints_JSON = "[]",
                        Message = Common.AppPrivileges.No_Record_Message
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
    }
}