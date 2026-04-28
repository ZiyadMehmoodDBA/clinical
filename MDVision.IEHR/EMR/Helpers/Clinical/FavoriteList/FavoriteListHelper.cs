using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Model.FavoriteList;
using MDVision.Model.Clinical.Favorites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.FavoriteList
{
    public class FavoriteListHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public FavoriteListHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static FavoriteListHelper _instance = null;
        public static FavoriteListHelper Instance()
        {
            if (_instance == null)
                _instance = new FavoriteListHelper();
            return _instance;
        }

        /// Author: ZeeshanAK
        /// Purpose: To load Systems for Review Of Systems section
        /// Date : January 27, 2016
        #region Load FavoriteList
        public string loadClinical_FavoriteList(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = null;
                BLObject<DSFavoriteList> obj;
                /*if (string.IsNullOrEmpty(model.EntityId))
                {
                    model.EntityId = MDVSession.Current.EntityId;
                }*/

                byte isActive = Convert.ToByte(model.IsActive);
                if (MDVUtility.ToLong(model.EntityId) == -1)
                {
                    model.EntityId = null;
                }

                if (model.FavoriteListId > 0)
                {
                    obj = BLLClinicalObj.loadClinical_FavoriteList(model.FavoriteListId, model.ListType, MDVUtility.ToInt64(model.EntityId), isActive, model.PageNumber, model.RowsPerPage, "1", "", MDVUtility.ToInt64(model.LabId));
                }
                else
                {
                    obj = BLLClinicalObj.loadClinical_FavoriteList(model.FavoriteListId, model.ListType, MDVUtility.ToInt64(model.EntityId), isActive, model.PageNumber, model.RowsPerPage, "", "", MDVUtility.ToInt64(model.LabId), MDVUtility.ToInt64(model.ProviderId), IsSelectForLookUp: model.IsSelectForLookUp ?? false);
                }
                dsFavoriteList = obj.Data;
                if (obj.Data != null)
                {
                    if (dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName].Rows.Count > 0)
                    {
                        DSFavoriteList dsFavoriteListProvider = null;
                        BLObject<DSFavoriteList> objProvider;
                        objProvider = BLLClinicalObj.loadClinical_FavoriteListProviders(model.FavoriteListId);
                        dsFavoriteListProvider = objProvider.Data;
                        List<string> providers = new List<string>();
                        if (objProvider.Data != null)
                        {
                            providers = dsFavoriteListProvider.FavoriteListProvider.AsEnumerable().AsQueryable().Select(p => p.ProviderId).ToList();
                        }


                        //DSFavoriteList dsFavoriteListCustomForms = null;
                        //BLObject<DSFavoriteList> objCustomForms;
                        //objCustomForms = BLLClinicalObj.loadClinical_FavoriteListCustomForms(model.FavoriteListId);
                        //dsFavoriteListCustomForms = objCustomForms.Data;
                        //List<string> forms = new List<string>();
                        //if (objCustomForms.Data != null)
                        //{
                        //    forms = dsFavoriteListProvider.FavoriteListCustomForm.AsEnumerable().AsQueryable().Select(p => p.FavoriteListCustomFormId.ToString()).ToList();
                        //}
                        // providers
                        var response = new
                        {
                            status = true,
                            FavoriteListCount = dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName].Rows.Count,
                            iTotalDisplayRecords = dsFavoriteList.FavoriteList.Rows[0][dsFavoriteList.FavoriteList.RecordCountColumn.ColumnName],
                            FavoriteListJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName]),
                            FavoriteListProviders_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(providers)
                            //FavoriteListCustomForms_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(forms)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FavoriteListCount = 0,
                            FavoriteListJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        SystemsCount = 0,
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

        public string loadClinical_FavoriteListICD(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = null;
                BLObject<DSFavoriteList> obj;
                byte isActive = Convert.ToByte(model.IsActive);
                obj = BLLClinicalObj.loadClinical_FavoriteListICD(model.FavoriteListICDId, model.FavoriteListId, isActive, model.PageNumber, model.RowsPerPage, model.SearchData);

                dsFavoriteList = obj.Data;
                if (obj.Data != null)
                {
                    if (dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FavoriteListICDCount = dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName].Rows.Count,
                            iTotalDisplayRecords = dsFavoriteList.FavoriteListICD.Rows[0][dsFavoriteList.FavoriteListICD.RecordCountColumn.ColumnName],
                            FavoriteListICDJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FavoriteListICDCount = 0,
                            FavoriteListICDJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        SystemsCount = 0,
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

        public string fillClinical_FavoriteListICD(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = null;
                BLObject<DSFavoriteList> obj;
                byte isActive = Convert.ToByte(model.IsActive);
                obj = BLLClinicalObj.loadClinical_FavoriteListICD(model.FavoriteListICDId, model.FavoriteListId, 3, model.PageNumber, model.RowsPerPage);

                dsFavoriteList = obj.Data;
                if (obj.Data != null)
                {
                    if (dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName].Rows.Count > 0)
                    {
                        DSFavoriteList dsFavoriteListProvider = null;
                        BLObject<DSFavoriteList> objProvider;
                        objProvider = BLLClinicalObj.loadClinical_FavoriteListProviders(model.FavoriteListId);
                        dsFavoriteListProvider = objProvider.Data;
                        List<string> providers = new List<string>();
                        if (objProvider.Data != null)
                        {
                            providers = dsFavoriteListProvider.FavoriteListProvider.AsEnumerable().AsQueryable().Select(p => p.ProviderId).ToList();
                        }
                        var response = new
                        {
                            status = true,
                            FavoriteListICDCount = dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName].Rows.Count,
                            iTotalDisplayRecords = dsFavoriteList.FavoriteListICD.Rows[0][dsFavoriteList.FavoriteListICD.RecordCountColumn.ColumnName],
                            FavoriteListICDJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName]),
                            FavoriteListProviders_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(providers)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FavoriteListICDCount = 0,
                            FavoriteListICDJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        SystemsCount = 0,
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
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 22/03/2016
        //OverView: Methods "SaveFavComplaints" for Call BLL for Save Fav Complaint Data
        public string SaveFavComplaints(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = new DSFavoriteList();
                if (model.FavoriteListId <= 0)
                {
                    DSFavoriteList.FavoriteListRow dr = dsFavoriteList.FavoriteList.NewFavoriteListRow();
                    dr.FavoriteListId = -1;
                    dr.Name = model.FavoriteListName;

                    if (MDVUtility.ToInt32(model.EntityId) == -1)
                    {
                        dr.EntityId = MDVUtility.ToInt32(MDVSession.Current.EntityId);
                    }
                    else
                    {
                        dr.EntityId = MDVUtility.ToInt32(model.EntityId);
                    }
                    dr.ListType = model.ListType;
                    if (!string.IsNullOrEmpty(model.BodyPartId))
                        dr.BodyPartId = MDVUtility.ToInt64(model.BodyPartId);
                    else
                        dr[dsFavoriteList.FavoriteList.BodyPartIdColumn] = DBNull.Value;
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dsFavoriteList.FavoriteList.AddFavoriteListRow(dr);
                }
                for (int i = 0; i < model.FavoriteListIcd.Count; i++)
                {
                    DSFavoriteList.FavoriteListICDRow dr1 = dsFavoriteList.FavoriteListICD.NewFavoriteListICDRow();
                    dr1.FavoriteListICDId = model.FavoriteListId <= 0 ? -i : model.FavoriteListId;
                    dr1.ICD9Code = model.FavoriteListIcd[i].ICD9;
                    dr1.ICD10Code = model.FavoriteListIcd[i].ICD10;
                    dr1.ICD10CodeDescription = model.FavoriteListIcd[i].ICD10Description;
                    dr1.SNOMEDID = model.FavoriteListIcd[i].SNOMED;
                    if (!string.IsNullOrEmpty(model.FavoriteListIcd[i].ICD9Description))
                    {
                        dr1.ICD9CodeDescription = model.FavoriteListIcd[i].ICD9Description;
                    }
                    else
                    {
                        dr1[dsFavoriteList.FavoriteListICD.ICD9CodeDescriptionColumn] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(model.FavoriteListIcd[i].SNOMEDDescription))
                    {
                        dr1.SNOMEDDescription = model.FavoriteListIcd[i].SNOMEDDescription;
                    }
                    else
                    {
                        dr1[dsFavoriteList.FavoriteListICD.SNOMEDDescriptionColumn] = DBNull.Value;
                    }
                    dr1.IsActive = true;
                    dr1.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr1.CreatedOn = DateTime.Now;
                    dr1.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr1.ModifiedOn = DateTime.Now;
                    dsFavoriteList.FavoriteListICD.AddFavoriteListICDRow(dr1);
                }

                #region Database Insertion
                BLObject<DSFavoriteList> obj = BLLClinicalObj.insertClinical_FavoriteList(dsFavoriteList, model.ProviderIds);
                dsFavoriteList = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        FavComplaintId = dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName].Rows[0][dsFavoriteList.FavoriteList.FavoriteListIdColumn.ColumnName],
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
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


        public string SaveFavComplaintsICD(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = new DSFavoriteList();

                for (int i = 0; i < model.FavoriteListIcd.Count; i++)
                {
                    DSFavoriteList.FavoriteListICDRow dr1 = dsFavoriteList.FavoriteListICD.NewFavoriteListICDRow();
                    dr1.FavoriteListId = model.FavoriteListId <= 0 ? -i : model.FavoriteListId;
                    dr1.FavoriteListICDId = -i;
                    dr1.ICD9Code = model.FavoriteListIcd[i].ICD9;
                    dr1.ICD10Code = model.FavoriteListIcd[i].ICD10;
                    dr1.ICD10CodeDescription = model.FavoriteListIcd[i].ICD10Description;

                    if (!string.IsNullOrEmpty(model.FavoriteListIcd[i].ICD9Description))
                    {
                        dr1.ICD9CodeDescription = model.FavoriteListIcd[i].ICD9Description;
                    }
                    else
                    {
                        dr1[dsFavoriteList.FavoriteListICD.ICD9CodeDescriptionColumn] = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(model.FavoriteListIcd[i].SNOMEDDescription))
                    {
                        dr1.SNOMEDDescription = model.FavoriteListIcd[i].SNOMEDDescription;
                    }
                    else
                    {
                        dr1[dsFavoriteList.FavoriteListICD.SNOMEDDescriptionColumn] = DBNull.Value;
                    }

                    dr1.SNOMEDID = model.FavoriteListIcd[i].SNOMED;
                    dr1.IsActive = model.IsActive;
                    dr1.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr1.CreatedOn = DateTime.Now;
                    dr1.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr1.ModifiedOn = DateTime.Now;
                    dsFavoriteList.FavoriteListICD.AddFavoriteListICDRow(dr1);
                }

                #region Database Insertion
                BLObject<DSFavoriteList> obj = BLLClinicalObj.insertClinical_FavoriteListICD(dsFavoriteList);
                dsFavoriteList = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        FavComplaintId = dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName].Rows[0][dsFavoriteList.FavoriteList.FavoriteListIdColumn.ColumnName],
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
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

        public string updateFavComplaints(FavoriteListModel model)
        {
            try
            {

                List<FavoriteListICDModel> FavoriteListInsert = model.FavoriteListIcd.Where(n => n.FavoriteListICDId == -1).ToList<FavoriteListICDModel>();
                List<FavoriteListICDModel> FavoriteListUpdated = model.FavoriteListIcd.Where(n => n.FavoriteListICDId != -1).ToList<FavoriteListICDModel>();


                DSFavoriteList dsFavoriteList = null;
                BLObject<DSFavoriteList> obj;
                byte isActive = Convert.ToByte(model.IsActive);
                byte isActivePrevious = Convert.ToByte(model.IsActivePrevious);
                obj = BLLClinicalObj.loadClinical_FavoriteListICD(model.FavoriteListICDId, model.FavoriteListId, 3, model.PageNumber, model.RowsPerPage);
                List<string> RemoveListICD = new List<string>();
                if (obj.Data != null)
                {
                    dsFavoriteList = obj.Data;
                    if (dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName].Rows.Count > 0)
                    {
                        foreach (DSFavoriteList.FavoriteListICDRow dr in dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName].Rows)
                        {
                            string FavoriteListICDId = dr[dsFavoriteList.FavoriteListICD.FavoriteListICDIdColumn].ToString();
                            FavoriteListICDModel modelObj = FavoriteListUpdated.FirstOrDefault(n => n.FavoriteListICDId == MDVUtility.ToLong(FavoriteListICDId));
                            if (modelObj != null)
                            {
                                FavoriteListUpdated.Remove(modelObj);
                                dr.IsActive = model.IsActive;
                                if (MDVUtility.ToInt32(model.EntityId) == -1)
                                {
                                    dr.EntityId = MDVUtility.ToInt32(MDVSession.Current.EntityId);
                                }
                                else
                                {
                                    dr.EntityId = MDVUtility.ToInt32(model.EntityId);
                                }
                            }
                            else
                            {
                                RemoveListICD.Add(FavoriteListICDId);
                            }
                        }
                        obj = BLLClinicalObj.updateClinical_FavoriteListICD(dsFavoriteList);
                    }
                }


                string FavoriteListICDIdList = string.Join(", ", Array.ConvertAll(RemoveListICD.ToArray(), i => i.ToString()));
                #region Database Delete
                if (!string.IsNullOrEmpty(FavoriteListICDIdList))
                {
                    BLObject<string> objd = BLLClinicalObj.deleteClinical_FavoriteListICD(FavoriteListICDIdList);
                }

                if (FavoriteListInsert.Count > 0)
                {
                    FavoriteListModel modelInsert = model;
                    modelInsert.FavoriteListIcd = FavoriteListInsert;
                    SaveFavComplaintsICD(model);
                }

                //obj = BLLClinicalObj.loadClinical_FavoriteList(model.FavoriteListId, model.ListType, 0, 3, model.PageNumber, model.RowsPerPage);
                obj = BLLClinicalObj.loadClinical_FavoriteList(model.FavoriteListId, model.ListType, 0, isActivePrevious, model.PageNumber, model.RowsPerPage);
                dsFavoriteList = obj.Data;
                if (dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName].Rows.Count > 0)
                {
                    foreach (DSFavoriteList.FavoriteListRow dr in dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName].Rows)
                    {
                        dr.Name = model.FavoriteListName;

                        if (!string.IsNullOrEmpty(model.EntityId))
                        {
                            if (MDVUtility.ToInt32(model.EntityId) == -1)
                            {
                                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                            }
                            else
                            {
                                dr.EntityId = MDVUtility.ToInt64(model.EntityId);
                            }
                        }
                        else
                        {
                            dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                        }
                        dr.ListType = model.ListType;
                        dr.IsActive = model.IsActive;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(model.BodyPartId))
                            dr.BodyPartId = MDVUtility.ToInt64(model.BodyPartId);
                        else
                            dr[dsFavoriteList.FavoriteList.BodyPartIdColumn] = DBNull.Value;
                    }
                    obj = BLLClinicalObj.updateClinical_FavoriteList(dsFavoriteList, "", model.ProviderIds, model.ListType);
                    dsFavoriteList = obj.Data;
                }
                if (obj.Data != null)
                {

                    var response = new
                    {
                        FavComplaintId = dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName].Rows[0][dsFavoriteList.FavoriteList.FavoriteListIdColumn.ColumnName],
                        status = true,
                        Message = Common.AppPrivileges.Update_Message,
                        //Message = Common.AppPrivileges.Save_Message,
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

        /// <summary>
        /// Method Name: loadClinicalFavoriteListCPT
        /// Author Name: Ahmad Raza
        /// Description: This function will load CPT of Favorite List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string loadClinicalFavoriteListCPT(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = null;
                BLObject<DSFavoriteList> obj;
                obj = BLLClinicalObj.loadClinicalFavoriteListCPT(model.FavoriteListCPTId, model.FavoriteListId, model.PageNumber, model.RowsPerPage, model.SearchData);

                dsFavoriteList = obj.Data;
                if (obj.Data != null)
                {
                    if (dsFavoriteList.Tables[dsFavoriteList.FavoriteListCPT.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FavoriteListCPTCount = dsFavoriteList.Tables[dsFavoriteList.FavoriteListCPT.TableName].Rows.Count,
                            iTotalDisplayRecords = dsFavoriteList.FavoriteListCPT.Rows[0][dsFavoriteList.FavoriteListCPT.RecordCountColumn.ColumnName],
                            FavoriteListCPTJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteListCPT.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FavoriteListCPTCount = 0,
                            FavoriteListCPTJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteListCPT.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        SystemsCount = 0,
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

        public string loadClinicalFavoriteListCustomForm(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = null;
                BLObject<DSFavoriteList> obj;
                obj = BLLClinicalObj.loadClinicalFavoriteListCustomForm(model.FavoriteListCustomFormId, model.FavoriteListId, model.PageNumber, model.RowsPerPage);

                dsFavoriteList = obj.Data;
                if (obj.Data != null)
                {
                    if (dsFavoriteList.Tables[dsFavoriteList.FavoriteListCustomForm.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FavoriteListCustomFormCount = dsFavoriteList.Tables[dsFavoriteList.FavoriteListCustomForm.TableName].Rows.Count,
                            iTotalDisplayRecords = dsFavoriteList.FavoriteListCustomForm.Rows[0][dsFavoriteList.FavoriteListCustomForm.RecordCountColumn.ColumnName],
                            FavoriteListCustomFormJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteListCustomForm.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FavoriteListCPTCount = 0,
                            FavoriteListCPTJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteListCPT.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        SystemsCount = 0,
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

        public string loadClinicalFavoriteListICD(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = null;
                BLObject<DSFavoriteList> obj;
                obj = BLLClinicalObj.loadClinical_FavoriteListICD(model.FavoriteListICDId, model.FavoriteListId, 1, model.PageNumber, model.RowsPerPage);

                dsFavoriteList = obj.Data;
                if (obj.Data != null)
                {
                    if (dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FavoriteListICDCount = dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName].Rows.Count,
                            iTotalDisplayRecords = dsFavoriteList.FavoriteListICD.Rows[0][dsFavoriteList.FavoriteListICD.RecordCountColumn.ColumnName],
                            FavoriteListICDJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FavoriteListICDCount = 0,
                            FavoriteListICDJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        SystemsCount = 0,
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
        /// Method Name: insertUpdateFavouriteListCPT
        /// Author Name: Ahmad Raza
        /// Description: This function will insert CPT of Favorite List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string insertUpdateFavouriteListCPT(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = new DSFavoriteList();
                DSFavoriteList.FavoriteListRow dr = null;


                BLObject<DSFavoriteList> objFavList = BLLClinicalObj.getAllFavoriteList(model.FavoriteListId, model.ListType, 0, 2, 0, 0, "", "", Convert.ToInt64(model.LabId), 0, true);

                dsFavoriteList = objFavList.Data;
                bool isNewRecord = false;
                DSFavoriteList.FavoriteListRow[] arrFavListRows = null;
                DSFavoriteList.FavoriteListCPTRow[] arrFavListCPTRows = null;
                string message = string.Empty;
                if (objFavList.Data != null)
                {
                    arrFavListRows = (DSFavoriteList.FavoriteListRow[])dsFavoriteList.FavoriteList.Select(dsFavoriteList.FavoriteList.FavoriteListIdColumn.ColumnName + "=" + model.FavoriteListId);
                    if (arrFavListRows.Length > 0)
                    {
                        dr = arrFavListRows[0];
                        message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        dr = dsFavoriteList.FavoriteList.NewFavoriteListRow();
                        dr.FavoriteListId = -1;
                        message = Common.AppPrivileges.Save_Message;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        isNewRecord = true;
                    }
                }
                dr.Name = model.FavoriteListName;
                if (!string.IsNullOrEmpty(model.EntityId))
                {
                    if (MDVUtility.ToInt32(model.EntityId) == -1)
                    {
                        dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    }
                    else
                    {
                        dr.EntityId = MDVUtility.ToInt64(model.EntityId);
                    }
                }
                else
                {
                    dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                }
                if (model.LabId != "" && model.LabId != null)
                {
                    dr.LabId = model.LabId;
                }
                else
                {
                    dr.LabId = null;
                }
                if (!string.IsNullOrEmpty(model.BodyPartId))
                    dr.BodyPartId = MDVUtility.ToInt64(model.BodyPartId);
                else
                    dr[dsFavoriteList.FavoriteList.BodyPartIdColumn] = DBNull.Value;
                dr.IsActive = model.IsActive;
                dr.ListType = model.ListType;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                if (isNewRecord)
                {
                    dsFavoriteList.FavoriteList.AddFavoriteListRow(dr);
                }

                for (int i = 0; i < model.FavoriteListCPT.Count; i++)
                {
                    if (((model.FavoriteListCPT[i].FavoriteListCPTId == null || model.FavoriteListCPT[i].FavoriteListCPTId == "0") || model.FavoriteListCPT[i].FavoriteListCPTId == ""))
                    {
                        DSFavoriteList.FavoriteListCPTRow dr1 = null;
                        dr1 = dsFavoriteList.FavoriteListCPT.NewFavoriteListCPTRow();
                        dr1.FavoriteListCPTId = -i;

                        dr1.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr1.CreatedOn = DateTime.Now;

                        dr1.CPTCode = model.FavoriteListCPT[i].CPTCode;
                        dr1.CPTCodeDescription = model.FavoriteListCPT[i].CPTCodeDescription;
                        dr1.SNOMEDID = model.FavoriteListCPT[i].SNOMEDID;
                        dr1.SNOMED_DESCRIPTION = model.FavoriteListCPT[i].SNOMED_DESCRIPTION;
                        if (!string.IsNullOrEmpty(model.FavoriteListCPT[i].LabId))
                            dr1.LabId = MDVUtility.ToInt64(model.FavoriteListCPT[i].LabId);
                        else
                            dr1[dsFavoriteList.FavoriteListCPT.LabIdColumn] = DBNull.Value;

                        dr1.IsActive = true;

                        dr1.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr1.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(model.FavoriteListCPT[i].Unit))
                            dr1.Unit = MDVUtility.ToInt32(model.FavoriteListCPT[i].Unit);
                        else
                            dr1[dsFavoriteList.FavoriteListCPT.UnitColumn] = DBNull.Value;
                        dr1.Modifier = model.FavoriteListCPT[i].Modifier;

                        dsFavoriteList.FavoriteListCPT.AddFavoriteListCPTRow(dr1);
                    }
                    else
                    {
                        if (model.FavoriteListCPT[i].FavoriteListCPTId != null)
                            if (objFavList.Data.FavoriteListCPT != null)
                            {

                                arrFavListCPTRows
                                   = (DSFavoriteList.FavoriteListCPTRow[])dsFavoriteList.FavoriteListCPT.Select(dsFavoriteList.FavoriteListCPT.FavoriteListCPTIdColumn.ColumnName + "=" + model.FavoriteListCPT[i].FavoriteListCPTId);

                                if (arrFavListCPTRows.Length > 0)
                                {
                                    DataRow cptDr = arrFavListCPTRows[0];

                                    cptDr[dsFavoriteList.FavoriteListCPT.ModifierColumn] = model.FavoriteListCPT[i].Modifier;
                                    if (!string.IsNullOrEmpty(model.FavoriteListCPT[i].Unit))
                                        cptDr[dsFavoriteList.FavoriteListCPT.UnitColumn] = MDVUtility.ToInt32(model.FavoriteListCPT[i].Unit);
                                    else
                                        cptDr[dsFavoriteList.FavoriteListCPT.UnitColumn] = DBNull.Value;

                                    cptDr[dsFavoriteList.FavoriteListCPT.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                    cptDr[dsFavoriteList.FavoriteListCPT.ModifiedOnColumn] = DateTime.Now;

                                    cptDr[dsFavoriteList.FavoriteListCPT.CPTCodeColumn] = model.FavoriteListCPT[i].CPTCode;
                                    cptDr[dsFavoriteList.FavoriteListCPT.CPTCodeDescriptionColumn] = model.FavoriteListCPT[i].CPTCodeDescription;

                                    cptDr[dsFavoriteList.FavoriteListCPT.SNOMEDIDColumn] = model.FavoriteListCPT[i].SNOMEDID;
                                    cptDr[dsFavoriteList.FavoriteListCPT.SNOMED_DESCRIPTIONColumn] = model.FavoriteListCPT[i].SNOMED_DESCRIPTION;
                                    if (!string.IsNullOrEmpty(model.FavoriteListCPT[i].LabId))
                                        cptDr[dsFavoriteList.FavoriteListCPT.LabIdColumn] = MDVUtility.ToInt64(model.FavoriteListCPT[i].LabId);
                                    else
                                        cptDr[dsFavoriteList.FavoriteListCPT.LabIdColumn] = DBNull.Value;

                                    cptDr[dsFavoriteList.FavoriteListCPT.IsActiveColumn] = true;
                                }


                            }
                    }
                }

                #region Database Insertion
                BLObject<DSFavoriteList> obj = null;
                if (isNewRecord)
                {
                    //Insert new fav list record
                    obj = BLLClinicalObj.insertClinicalFavoriteList(dsFavoriteList, "CPT", model.ProviderIds, model.ListType);
                    dsFavoriteList = obj.Data;
                }
                else
                {
                    //update fav list record
                    obj = BLLClinicalObj.updateClinical_FavoriteList(dsFavoriteList, "CPT", model.ProviderIds, model.ListType, true);
                    dsFavoriteList = obj.Data;
                }
                if (obj.Data != null)
                {
                    var response = new
                    {
                        FavCPTId = dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName].Rows[0][dsFavoriteList.FavoriteList.FavoriteListIdColumn.ColumnName],
                        status = true,
                        Message = message
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
        public string loadClinicalFavoriteListMedication(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = null;
                BLObject<DSFavoriteList> obj;
                obj = BLLClinicalObj.loadClinicalFavoriteListMedication(model.FavoriteListMedicationId, model.FavoriteListId, model.PageNumber, model.RowsPerPage);
                dsFavoriteList = obj.Data;
                if (obj.Data != null)
                {
                    if (dsFavoriteList.Tables[dsFavoriteList.FavoriteListMedication.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FavoriteListMedCount = dsFavoriteList.Tables[dsFavoriteList.FavoriteListMedication.TableName].Rows.Count,
                            iTotalDisplayRecords = dsFavoriteList.FavoriteListMedication.Rows[0][dsFavoriteList.FavoriteListMedication.RecordCountColumn.ColumnName],
                            FavoriteListMedJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteListMedication.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FavoriteListMedCount = 0,
                            FavoriteListMedSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.FavoriteListMedication.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
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
        public string insertUpdateFavouriteListMedication(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = new DSFavoriteList();
                DSFavoriteList.FavoriteListRow dr = null;
                BLObject<DSFavoriteList> objFavList = BLLClinicalObj.getAllFavoriteListMedication(model.FavoriteListId, model.ListType, 0, 2, 0, 0, "", "", 0, 0, true);
                #region FavList Insert/Update
                dsFavoriteList = objFavList.Data;
                bool isNewRecord = false;
                DSFavoriteList.FavoriteListRow[] arrFavListRows = null;
                string message = string.Empty;
                if (objFavList.Data != null)
                {
                    arrFavListRows = (DSFavoriteList.FavoriteListRow[])dsFavoriteList.FavoriteList.Select(dsFavoriteList.FavoriteList.FavoriteListIdColumn.ColumnName + "=" + model.FavoriteListId);
                    if (arrFavListRows.Length > 0)
                    {
                        dr = arrFavListRows[0];
                        message = Common.AppPrivileges.Update_Message;
                    }
                    else
                    {
                        dr = dsFavoriteList.FavoriteList.NewFavoriteListRow();
                        dr.FavoriteListId = -1;
                        message = Common.AppPrivileges.Save_Message;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        isNewRecord = true;
                    }
                }
                dr.Name = model.FavoriteListName;
                if (!string.IsNullOrEmpty(model.EntityId))
                {
                    if (MDVUtility.ToInt32(model.EntityId) == -1)
                        dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    else
                        dr.EntityId = MDVUtility.ToInt64(model.EntityId);
                }
                else
                    dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                if (model.LabId != "" && model.LabId != null)
                    dr.LabId = model.LabId;
                else
                    dr[dsFavoriteList.FavoriteList.LabIdColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.BodyPartId))
                    dr.BodyPartId = MDVUtility.ToInt64(model.BodyPartId);
                else
                    dr[dsFavoriteList.FavoriteList.BodyPartIdColumn] = DBNull.Value;
                dr.IsActive = model.IsActive;
                dr.ListType = model.ListType;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                if (isNewRecord)
                    dsFavoriteList.FavoriteList.AddFavoriteListRow(dr);

                #region Database Insertion
                BLObject<DSFavoriteList> obj = null;
                if (isNewRecord)
                {
                    //Insert new fav list record
                    obj = BLLClinicalObj.insertClinicalFavoriteList(dsFavoriteList, "Medication", model.ProviderIds, model.ListType);
                    dsFavoriteList = obj.Data;
                }
                else
                {
                    //update fav list record
                    obj = BLLClinicalObj.updateClinical_FavoriteList(dsFavoriteList, "Medication", model.ProviderIds, model.ListType, true);
                    dsFavoriteList = obj.Data;
                }
                #endregion

                if (obj.Data != null)
                {
                    var response = new
                    {
                        FavMedicationId = dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName].Rows[0][dsFavoriteList.FavoriteList.FavoriteListIdColumn.ColumnName],
                        status = true,
                        Message = message
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
        public string SaveFavMedicationDetail(FavoriteListMedicationModel model)
        {
            try
            {
                string MedicationId = BLLClinicalObj.SaveFavMedicationDetail(model);
                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
                };
                return (JsonConvert.SerializeObject(response));
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
        public string UpdateFavMedicationDetail(FavoriteListMedicationModel model)
        {
            try
            {
                string MedicationId = BLLClinicalObj.UpdateFavMedicationDetail(model);
                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Update_Message,
                };
                return (JsonConvert.SerializeObject(response));
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
        public string LoadMedicationDetail(FavoriteListMedicationModel model)
        {
            try
            {
                FavoriteListMedicationModel MedicationObj = null;
                BLObject<FavoriteListMedicationModel> obj;
                obj = BLLClinicalObj.LoadMedicationDetail(MDVUtility.ToInt64(model.FavoriteListId), MDVUtility.ToInt64(model.Id));
                MedicationObj = obj.Data;
                if (obj.Data != null)
                {
                    if (MedicationObj != null)
                    {
                        var response = new
                        {
                            status = true,
                            Medication_JSON = MedicationObj,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MedicationCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string DeleteMedicationDetail(FavoriteListMedicationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.Id)))
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
                    BLObject<string> obj = BLLClinicalObj.DeleteMedicationDetail(MDVUtility.ToInt64(model.Id));
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
        public string insertUpdateFavouriteListICD(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = new DSFavoriteList();
                DSFavoriteList.FavoriteListRow dr = null;
                BLObject<DSFavoriteList> objFavList = BLLClinicalObj.loadClinical_FavoriteList(model.FavoriteListId, model.ListType, 0, 2);
                dsFavoriteList = objFavList.Data;
                bool isNewRecord = false;
                DSFavoriteList.FavoriteListRow[] arrFavListRows = null;
                string message = string.Empty;
                if (objFavList.Data != null)
                {
                    arrFavListRows = (DSFavoriteList.FavoriteListRow[])dsFavoriteList.FavoriteList.Select(dsFavoriteList.FavoriteList.FavoriteListIdColumn.ColumnName + "=" + model.FavoriteListId);
                    if (arrFavListRows.Length > 0)
                    {
                        dr = arrFavListRows[0];
                        message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        dr = dsFavoriteList.FavoriteList.NewFavoriteListRow();
                        dr.FavoriteListId = -1;
                        message = Common.AppPrivileges.Save_Message;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        isNewRecord = true;
                    }
                }
                dr.Name = model.FavoriteListName;
                if (MDVUtility.ToInt32(model.EntityId) == -1)
                {

                }
                else
                {
                    dr.EntityId = MDVUtility.ToInt32(model.EntityId);
                }
                dr.IsActive = model.IsActive;
                dr.ListType = model.ListType;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                if (isNewRecord)
                {
                    dsFavoriteList.FavoriteList.AddFavoriteListRow(dr);
                }

                #region Database Insertion
                BLObject<DSFavoriteList> obj = null;
                if (isNewRecord)
                {
                    //Insert new fav list record
                    obj = BLLClinicalObj.insertClinicalFavoriteHistoryList(dsFavoriteList, model.ProviderIds);
                    dsFavoriteList = obj.Data;
                }
                else
                {
                    //update fav list record
                    obj = BLLClinicalObj.updateClinicalFavoriteHistoryList(dsFavoriteList, model.ProviderIds);
                    dsFavoriteList = obj.Data;
                }
                if (obj.Data != null)
                {
                    long listId = MDVUtility.ToInt64(dsFavoriteList.FavoriteList.Rows[0]["FavoriteListId"]);

                    if (model.FavoriteListIcd != null && model.FavoriteListIcd.Count > 0)
                    {
                        model.FavoriteListIcd.ForEach(cc => cc.FavoriteListICDId = listId);

                        var id = saveFavoriteICDs(model, listId);

                    }
                    var response = new
                    {
                        FavICDId = dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName].Rows[0][dsFavoriteList.FavoriteList.FavoriteListIdColumn.ColumnName],
                        status = true,
                        Message = message
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

        public string insertUpdateFavouriteListCustomForms(FavoriteListModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = new DSFavoriteList();
                DSFavoriteList.FavoriteListRow dr = null;
                BLObject<DSFavoriteList> objFavList = BLLClinicalObj.loadClinical_FavoriteList(model.FavoriteListId, model.ListType, 0, 2);
                dsFavoriteList = objFavList.Data;
                bool isNewRecord = false;
                DSFavoriteList.FavoriteListRow[] arrFavListRows = null;
                string message = string.Empty;
                if (objFavList.Data != null)
                {
                    arrFavListRows = (DSFavoriteList.FavoriteListRow[])dsFavoriteList.FavoriteList.Select(dsFavoriteList.FavoriteList.FavoriteListIdColumn.ColumnName + "=" + model.FavoriteListId);
                    if (arrFavListRows.Length > 0)
                    {
                        dr = arrFavListRows[0];
                        message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        dr = dsFavoriteList.FavoriteList.NewFavoriteListRow();
                        dr.FavoriteListId = -1;
                        message = Common.AppPrivileges.Save_Message;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        isNewRecord = true;
                    }
                }
                dr.Name = model.FavoriteListName;
                if (MDVUtility.ToInt32(model.EntityId) == -1)
                {

                }
                else
                {
                    dr.EntityId = MDVUtility.ToInt32(model.EntityId);
                }
                if (!string.IsNullOrEmpty(model.BodyPartId))
                    dr.BodyPartId = MDVUtility.ToInt64(model.BodyPartId);
                else
                    dr[dsFavoriteList.FavoriteList.BodyPartIdColumn] = DBNull.Value;
                dr.IsActive = model.IsActive;
                dr.ListType = model.ListType;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                if (isNewRecord)
                {
                    dsFavoriteList.FavoriteList.AddFavoriteListRow(dr);
                }

                #region Database Insertion
                BLObject<DSFavoriteList> obj = null;
                if (isNewRecord)
                {
                    //Insert new fav list record
                    obj = BLLClinicalObj.insertClinicalFavoriteHistoryList(dsFavoriteList, model.ProviderIds, model.CustomFormsIds);
                    dsFavoriteList = obj.Data;
                }
                else
                {
                    //update fav list record
                    obj = BLLClinicalObj.updateClinicalFavoriteHistoryList(dsFavoriteList, model.ProviderIds, model.CustomFormsIds);
                    dsFavoriteList = obj.Data;
                }
                if (obj.Data != null)
                {
                    long listId = MDVUtility.ToInt64(dsFavoriteList.FavoriteList.Rows[0]["FavoriteListId"]);

                    if (model.FavoriteListCustomForms != null && model.FavoriteListCustomForms.Count > 0)
                    {
                        //  model.FavoriteListCustomForms.ForEach(cc => cc.FavoriteListCustomFormsId = listId);

                        //  var id = saveFavoriteCustomForms(model, listId);

                    }
                    var response = new
                    {
                        FavId = dsFavoriteList.Tables[dsFavoriteList.FavoriteList.TableName].Rows[0][dsFavoriteList.FavoriteList.FavoriteListIdColumn.ColumnName],
                        status = true,
                        Message = message
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

        /// <summary>
        /// Method Name: deleteFavouriteListCPT
        /// Author Name: Ahmad Raza
        /// Description: This function will delete CPT of Favorite List
        /// </summary>
        /// <param name="favouriteListCPT"></param>
        /// <returns></returns>
        public string deleteFavouriteListCPT(string favouriteListCPT, string favouriteListId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(favouriteListCPT)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteClinicalFavoriteListCPT(favouriteListCPT, favouriteListId);
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

        public string deleteFavouriteListICD(string favouriteListICD)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(favouriteListICD)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteClinical_FavoriteListICD(favouriteListICD);
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


        public string GetFavListValue(string favouriteListName)
        {
            try
            {
                if (string.IsNullOrEmpty(favouriteListName))
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
                    BLObject<string> obj = BLLClinicalObj.GetFavListValue(favouriteListName);
                    var response = new
                    {
                        status = true,
                        favListVal = obj.Data
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


        public string InsertUpdateFavlistValue(string favListName, string favListVal)
        {
            try
            {

                DSUsers dsDefaultsetting = new DSUsers();

                DSUsers dsUser = new DSUsers();
                BLObject<DSUsers> objUser = null;
                BLObject<DSUsers> obj = null;

                objUser = new BLLAdminSecurity().LoadEntityUserOption(ref dsUser, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), MDVSession.Current.EntityId);
                dsDefaultsetting = objUser.Data;

                if (dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows.Count == 0)
                {
                    DSUsers.EntityUserOptionRow dr = dsDefaultsetting.EntityUserOption.NewEntityUserOptionRow();
                    dr.UserId = MDVSession.Current.AppUserId;
                    dr.EntityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
                    dr.EntityRegCode = MDVSession.Current.EntityRegCode;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.IsDefault = false;
                    dr.IsActive = true;
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    if (favListName == "ClinicalProcedureDetail")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavProceduresVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavProceduresValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {

                        dr[dsDefaultsetting.EntityUserOption.FavProceduresValColumn] = DBNull.Value;

                    }

                    if (favListName == "ClinicalProblemList")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavProblemsVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavProblemsValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {

                        dr[dsDefaultsetting.EntityUserOption.FavProblemsValColumn] = DBNull.Value;

                    }
                    if (favListName == "ClinicalMedicalHx")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavMedicalHxVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavMedicalHxValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {

                        dr[dsDefaultsetting.EntityUserOption.FavMedicalHxValColumn] = DBNull.Value;

                    }
                    if (favListName == "ClinicalHospitalizationHx")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavHospitalizationHxVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavHospitalizationHxValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {

                        dr[dsDefaultsetting.EntityUserOption.FavHospitalizationHxValColumn] = DBNull.Value;

                    }
                    if (favListName == "ClinicalFamilyHx")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavFamilyHxVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavFamilyHxValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {

                        dr[dsDefaultsetting.EntityUserOption.FavFamilyHxValColumn] = DBNull.Value;

                    }
                    if (favListName == "ClinicalSurgicalHx")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavSurgicalHxVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavSurgicalHxValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {

                        dr[dsDefaultsetting.EntityUserOption.FavSurgicalHxValColumn] = DBNull.Value;

                    }
                    if (favListName == "LabOrderDetail")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavLabOrderVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavLabOrderValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {

                        dr[dsDefaultsetting.EntityUserOption.FavLabOrderValColumn] = DBNull.Value;

                    }
                    if (favListName == "ProcedureOrderDetail")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavProcedureOrderVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavProcedureOrderValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {

                        dr[dsDefaultsetting.EntityUserOption.FavProcedureOrderValColumn] = DBNull.Value;

                    }
                    if (favListName == "RadiologyOrderDetail")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavRadiologyOrderVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavRadiologyOrderValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {

                        dr[dsDefaultsetting.EntityUserOption.FavRadiologyOrderValColumn] = DBNull.Value;

                    }
                    if (favListName == "ClinicalConsultationOrder")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavConsultationVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavConsultationValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {

                        dr[dsDefaultsetting.EntityUserOption.FavConsultationValColumn] = DBNull.Value;

                    }
                    if (favListName == "Complaint")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavComplaintVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavComplaintValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {

                        dr[dsDefaultsetting.EntityUserOption.FavComplaintValColumn] = DBNull.Value;

                    }
                    if (favListName == "ClinicalImmunizationDetail")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavImmunizationVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavImmunizationValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {
                        dr[dsDefaultsetting.EntityUserOption.FavImmunizationValColumn] = DBNull.Value;

                    }
                    if (favListName == "ClinicalTherapeuticInjection")
                    {
                        if (!string.IsNullOrEmpty(favListVal))
                        {
                            dr.FavTherapeuticVal = MDVUtility.ToInt64(favListVal);
                        }
                        else
                        {
                            dr[dsDefaultsetting.EntityUserOption.FavTherapeuticValColumn] = DBNull.Value;
                        }
                    }
                    else
                    {
                        dr[dsDefaultsetting.EntityUserOption.FavTherapeuticValColumn] = DBNull.Value;

                    }



                    dsDefaultsetting.EntityUserOption.AddEntityUserOptionRow(dr);
                    obj = new BLLAdminSecurity().InsertDefaultSettings(dsDefaultsetting);

                }
                else
                {
                    foreach (DSUsers.EntityUserOptionRow dr in dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows)
                    {
                        dr.UserId = MDVSession.Current.AppUserId;
                        dr.EntityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
                        dr.EntityRegCode = MDVSession.Current.EntityRegCode;
                        // dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.IsDefault = false;
                        dr.IsActive = true;
                        // dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (favListName == "ClinicalProcedureDetail")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavProceduresVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavProceduresValColumn] = DBNull.Value;
                            }
                        }


                        if (favListName == "ClinicalProblemList")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavProblemsVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavProblemsValColumn] = DBNull.Value;
                            }
                        }
                        if (favListName == "ClinicalMedicalHx")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavMedicalHxVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavMedicalHxValColumn] = DBNull.Value;
                            }
                        }
                        if (favListName == "ClinicalHospitalizationHx")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavHospitalizationHxVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavHospitalizationHxValColumn] = DBNull.Value;
                            }
                        }
                        if (favListName == "ClinicalFamilyHx")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavFamilyHxVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavFamilyHxValColumn] = DBNull.Value;
                            }
                        }
                        if (favListName == "ClinicalSurgicalHx")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavSurgicalHxVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavSurgicalHxValColumn] = DBNull.Value;
                            }
                        }
                        if (favListName == "LabOrderDetail")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavLabOrderVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavLabOrderValColumn] = DBNull.Value;
                            }
                        }
                        if (favListName == "ProcedureOrderDetail")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavProcedureOrderVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavProcedureOrderValColumn] = DBNull.Value;
                            }
                        }
                        if (favListName == "RadiologyOrderDetail")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavRadiologyOrderVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavRadiologyOrderValColumn] = DBNull.Value;
                            }
                        }
                        if (favListName == "ClinicalConsultationOrder")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavConsultationVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavConsultationValColumn] = DBNull.Value;
                            }
                        }
                        if (favListName == "Complaint")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavComplaintVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavComplaintValColumn] = DBNull.Value;
                            }
                        }
                        if (favListName == "ClinicalImmunizationDetail")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavImmunizationVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavImmunizationValColumn] = DBNull.Value;
                            }
                        }
                        if (favListName == "ClinicalTherapeuticInjection")
                        {
                            if (!string.IsNullOrEmpty(favListVal))
                            {
                                dr.FavTherapeuticVal = MDVUtility.ToInt64(favListVal);
                            }
                            else
                            {
                                dr[dsDefaultsetting.EntityUserOption.FavTherapeuticValColumn] = DBNull.Value;
                            }
                        }





                    }
                    obj = new BLLAdminSecurity().UpdatDefaultSettings(dsDefaultsetting);
                }
                if (obj != null)
                {

                    var response = new
                    {
                        status = true,
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



        //Start//03/23/2016//Babur Rizwan//Delete Favorite Complaint
        /// <summary>
        /// This Function will delete Favorite Complaint
        /// </summary>
        /// <param name="FavoriteListModel"></param>
        /// <returns></returns>
        public string DeleteFavComplaints(FavoriteListModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.FavoriteListId)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteClinical_FavoriteList(MDVUtility.ToStr(model.FavoriteListId));
                    //   BLObject<DSFavoriteList> obj = BLLClinicalObj.insertClinical_FavoriteList(dsFavoriteList);
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
        //End/03/23/2016//Babur Rizwan//Delete Favorite Complaint

        /// <summary>
        /// Method Name: deleteFavoriteListProcedure
        /// Author Name: Ahmad Raza
        /// Created Date: 24-03-2016
        /// Description: this function will delete favorite list procedure order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string deleteFavoriteListProcedure(FavoriteListModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.FavoriteListId)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteClinical_FavoriteList(MDVUtility.ToStr(model.FavoriteListId));
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

        public string deleteFavoriteListCustomForms(FavoriteListModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.FavoriteListId)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteClinical_FavoriteListCustomForms(MDVUtility.ToStr(model.FavoriteListId));
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

        public string saveFavoriteICDs(FavoriteListModel model, long favoriteListId)
        {
            try
            {
                DSFavoriteList dsFavoriteList = new DSFavoriteList();

                BLObject<DSFavoriteList> objFavListICDs = BLLClinicalObj.loadClinical_FavoriteListICD(0, favoriteListId, 1, 1, 2000);
                dsFavoriteList.Merge(objFavListICDs.Data);

                for (int i = 0; i < model.FavoriteListIcd.Count; i++)
                {

                    DSFavoriteList.FavoriteListICDRow dr1 = null;
                    var output = dsFavoriteList.FavoriteListICD.Where(p => p.ICD9Code == model.FavoriteListIcd[i].ICD9);
                    if (output != null && output.ToList().Count > 0)
                    {
                        dr1 = dsFavoriteList.FavoriteListICD.Where(p => p.ICD9Code == model.FavoriteListIcd[i].ICD9).FirstOrDefault();
                    }
                    else
                    {
                        dr1 = dsFavoriteList.FavoriteListICD.NewFavoriteListICDRow();
                        dr1.FavoriteListICDId = -i;
                        dr1.FavoriteListId = favoriteListId;
                        dr1.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr1.CreatedOn = DateTime.Now;
                        if (MDVUtility.ToInt32(model.EntityId) == -1)
                        {

                        }
                        else
                        {
                            dr1.EntityId = MDVUtility.ToInt32(model.EntityId);
                        }
                    }

                    dr1.ICD9Code = model.FavoriteListIcd[i].ICD9;
                    dr1.ICD9CodeDescription = model.FavoriteListIcd[i].ICD9Description;
                    dr1.ICD10Code = model.FavoriteListIcd[i].ICD10;
                    dr1.ICD10CodeDescription = model.FavoriteListIcd[i].ICD10Description;
                    dr1.SNOMEDID = model.FavoriteListIcd[i].SNOMED;
                    dr1.SNOMEDDescription = model.FavoriteListIcd[i].SNOMEDDescription;
                    dr1.IsActive = true;
                    dr1.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr1.ModifiedOn = DateTime.Now;
                    if (!(output != null && output.ToList().Count > 0))
                        dsFavoriteList.FavoriteListICD.AddFavoriteListICDRow(dr1);
                }

                #region Database Insertion
                BLObject<DSFavoriteList> obj = BLLClinicalObj.insertClinicalFavoriteHistoryListICD(dsFavoriteList);
                dsFavoriteList = obj.Data;

                if (obj.Data != null)
                {

                    Int64 favId = MDVUtility.ToInt64(dsFavoriteList.Tables[dsFavoriteList.FavoriteListICD.TableName].Rows[0][dsFavoriteList.FavoriteListICD.FavoriteListICDIdColumn.ColumnName]);

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        LabOrderId = favId,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }


        public string saveFavoriteCustomForms(FavoriteListModel model, long favoriteListId)
        {
            try
            {
                DSFavoriteList dsFavoriteList = new DSFavoriteList();

                BLObject<DSFavoriteList> objFavListCustomForms = BLLClinicalObj.loadClinical_FavoriteListCustomForms(0, favoriteListId, 1, 1, 2000);
                dsFavoriteList.Merge(objFavListCustomForms.Data);

                for (int i = 0; i < model.FavoriteListCustomForms.Count; i++)
                {

                    DSFavoriteList.FavoriteListCustomFormRow dr1 = null;
                    var output = dsFavoriteList.FavoriteListCustomForm.Where(p => p.FavoriteListCustomFormId == MDVUtility.ToInt64(model.FavoriteListCustomForms[i].FavoriteListCustomFormId));
                    if (output != null && output.ToList().Count > 0)
                    {
                        dr1 = dsFavoriteList.FavoriteListCustomForm.Where(p => p.FavoriteListCustomFormId == MDVUtility.ToInt64(model.FavoriteListCustomForms[i].CustomFormId)).FirstOrDefault();
                    }
                    else
                    {
                        dr1 = dsFavoriteList.FavoriteListCustomForm.NewFavoriteListCustomFormRow();
                        dr1.FavoriteListCustomFormId = -i;
                        dr1.FavoriteListId = favoriteListId;
                        dr1.CustomFormName = model.FavoriteListCustomForms[i].FormName;
                        dr1.CustomFormId = MDVUtility.ToInt64(model.FavoriteListCustomForms[i].CustomFormId);
                        dr1.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr1.CreatedOn = DateTime.Now;
                        if (MDVUtility.ToInt32(model.EntityId) == -1)
                        {

                        }
                        else
                        {
                            dr1.EntityId = MDVUtility.ToInt32(model.EntityId);
                        }
                    }


                    dr1.IsActive = true;
                    dr1.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr1.ModifiedOn = DateTime.Now;
                    if (!(output != null && output.ToList().Count > 0))
                        dsFavoriteList.FavoriteListCustomForm.AddFavoriteListCustomFormRow(dr1);
                }

                #region Database Insertion
                BLObject<DSFavoriteList> obj = BLLClinicalObj.insertClinicalFavoriteListCustomForms(dsFavoriteList);
                dsFavoriteList = obj.Data;

                if (obj.Data != null)
                {

                    Int64 favId = MDVUtility.ToInt64(dsFavoriteList.Tables[dsFavoriteList.FavoriteListCustomForm.TableName].Rows[0][dsFavoriteList.FavoriteListCustomForm.FavoriteListCustomFormIdColumn.ColumnName]);

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        LabOrderId = favId,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        #endregion

        #region Immunization
        public string GetCVXAndAdministeredCode(FavoriteListImmunizationModel model)
        {
            try
            {
                DSFavoriteList dsFavoriteList = null;
                BLObject<DSFavoriteList> obj;

                obj = BLLClinicalObj.GetCVXAndAdministeredCode(MDVUtility.ToInt64(model.VaccineIds), MDVUtility.ToInt64(model.TherapueticIds), model.Type);

                dsFavoriteList = obj.Data;
                if (obj.Data != null)
                {
                    if (dsFavoriteList.Tables[dsFavoriteList.VaccineData.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VaccineDataCount = dsFavoriteList.Tables[dsFavoriteList.VaccineData.TableName].Rows.Count,
                            VaccineDataJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.VaccineData.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VaccineDataCount = 0,
                            VaccineDataJSON = MDVUtility.JSON_DataTable(dsFavoriteList.Tables[dsFavoriteList.VaccineData.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                }

                else
                {
                    var response = new
                    {
                        status = false,
                        VaccineDataCount = 0,
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


        public string SaveFavVaccine(FavoriteListImmunizationModel model)
        {
            try
            {
                string FavVaccineId = BLLClinicalObj.SaveFavVaccine(model);
                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
                };
                return (JsonConvert.SerializeObject(response));
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


        public string LoadFavImmunization(FavoriteListImmunizationModel model)
        {
            try
            {
                List<FavoriteListImmunizationModel> FavoriteListImmunizationList = null;
                BLObject<List<FavoriteListImmunizationModel>> obj;

                obj = BLLClinicalObj.LoadFavImmunization(model.Type, model.IsActive, MDVUtility.ToInt64(model.ProviderIds));
                FavoriteListImmunizationList = obj.Data;
                if (obj.Data != null)
                {
                    if (FavoriteListImmunizationList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FavImmunizationCount = FavoriteListImmunizationList.Count,
                            FavImmunization_JSON = FavoriteListImmunizationList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FavImmunizationCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }


        public string LoadFavImmunizationDetail(FavoriteListImmunizationModel model)
        {
            try
            {
                List<FavoriteListImmunizationModel> FavoriteListImmunizationList = null;
                BLObject<List<FavoriteListImmunizationModel>> obj;

                obj = BLLClinicalObj.LoadFavImmunizationDetail(MDVUtility.ToInt64(model.FavoritiesListId));
                FavoriteListImmunizationList = obj.Data;
                if (obj.Data != null)
                {
                    if (FavoriteListImmunizationList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FavImmunizationCount = FavoriteListImmunizationList.Count,
                            FavImmunization_JSON = FavoriteListImmunizationList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            FavImmunizationCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string LoadFavImmunizationVaccineDetail(FavoriteListImmunizationModel model)
        {
            try
            {
                List<FavoriteListImmunizationModel> FavoriteListImmunizationList = null;
                BLObject<List<FavoriteListImmunizationModel>> obj;

                obj = BLLClinicalObj.LoadFavImmunizationVaccineDetail(MDVUtility.ToInt64(model.FavoritiesListId), model.Tab, model.SearchData);
                FavoriteListImmunizationList = obj.Data;
                if (obj.Data != null)
                {
                    if (FavoriteListImmunizationList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FavImmunizationCount = FavoriteListImmunizationList.Count,
                            FavImmunization_JSON = FavoriteListImmunizationList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            FavImmunizationCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string DeleteFavoriteImmunization(FavoriteListImmunizationModel model)
        {
            try
            {
                long FavoritiesListId = MDVUtility.ToInt64(model.FavoritiesListId);
                if (FavoritiesListId > 0)
                {

                    string ResponseOfDeleteCall = BLLClinicalObj.DeleteFavoriteImmunization(FavoritiesListId);
                    if (ResponseOfDeleteCall.Contains("Delete issue."))
                    {
                        var response = new
                        {
                            status = false,
                            ResponseOfDeleteCall = ResponseOfDeleteCall,
                            Message = ResponseOfDeleteCall,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ResponseOfDeleteCall = ResponseOfDeleteCall,
                            Message = AppPrivileges.Delete_Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = AppPrivileges.Delete_Error_Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string UpdateFavVaccine(FavoriteListImmunizationModel model)
        {
            try
            {
                string favVaccineId = BLLClinicalObj.UpdateFavVaccine(model);

                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Update_Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }
        #endregion
    }
}