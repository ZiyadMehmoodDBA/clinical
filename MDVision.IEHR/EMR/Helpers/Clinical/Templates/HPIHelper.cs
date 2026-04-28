using MDVision.Business.BCommon;
using System;
using System.Collections.Generic;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Common.Utilities;
using MDVision.Model.Clinical.Templates;
using System.IO;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using MDVision.Model.Clinical.HPI;
using Newtonsoft.Json;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.ClinicalNotes;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates
{
    public class HPIHelper
    {
        private BLLHPI BLLHPIObj = null;
        public HPIHelper()
        {
            BLLHPIObj = new BLLHPI();
        }

        private static HPIHelper _instance = null;
        public static HPIHelper Instance()
        {
            if (_instance == null)
                _instance = new HPIHelper();
            return _instance;
        }

        #region " HPI Finding Fill, Save and Update Methods "
        public string LoadHPIFindings(HPIFindingsModel model)
        {
            try
            {
                List<HPIFindingsModel> listHPIFindings = new List<HPIFindingsModel>();
                BLObject<List<HPIFindingsModel>> obj = null;
                obj = BLLHPIObj.LoadHPIFindings(MDVUtility.ToLong(model.HPIFindingsId), model.IsActive, model.Name, MDVUtility.ToLong(model.PageNumber), MDVUtility.ToLong(model.RowsPerPage));
                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        FindingsCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount,
                        listFindings = obj.Data,
                    };
                    return (JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FindingsCount = 0,
                        iTotalDisplayRecords = 0,
                        Message = "Record not found."
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
        public string DeleteHPIFindings(string HPIFindingsId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(HPIFindingsId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLHPIObj.DeleteHPIFindings(HPIFindingsId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }

        }
        public string SaveHPIFindings(HPIFindingsModel model)
        {
            try
            {
                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();

                #region Database Insertion

                BLLHPIObj.SaveHPIFindings(model);

                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
                    HPIFindingsId = model.HPIFindingsId
                };
                return (JsonConvert.SerializeObject(response));

                #endregion

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }

        }
        public string UpdateHPIFindings(HPIFindingsModel model)
        {
            try
            {
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                BLLHPIObj.UpdateHPIFindings(model);

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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return JsonConvert.SerializeObject(response);
            }
        }
        public string LookupHPIFindings(HPIFindingsModel model)
        {
            try
            {
                List<HPIFindingsModel> listHPIFindings = new List<HPIFindingsModel>();
                BLObject<List<HPIFindingsModel>> obj = null;
                obj = BLLHPIObj.LookupHPIFindings(model.IsActive);
                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        FindingsCount = obj.Data.Count,
                        listFindings = obj.Data,
                    };
                    return (JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FindingsCount = 0,
                        Message = "Record not found."
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
        public string FillHPIFindings(HPIFindingsModel model)
        {
            try
            {
                List<HPIFindingsModel> listHPIFindings = new List<HPIFindingsModel>();
                BLObject<List<HPIFindingsModel>> obj = null;
                obj = BLLHPIObj.LoadHPIFindings(MDVUtility.ToLong(model.HPIFindingsId), "", "", 1, 25);
                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        FindingsCount = obj.Data.Count,
                        listFindings = obj.Data,
                    };
                    return (JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        FindingsCount = 0,
                        Message = "Record not found."
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

        #endregion

        #region " HPI Symptoms Fill, Save and Update Methods "
        public string LoadHPISymptoms(HPISymptomsModel model)
        {
            try
            {
                List<HPISymptomsModel> listHPISymptoms = new List<HPISymptomsModel>();
                BLObject<List<HPISymptomsModel>> obj = null;
                obj = BLLHPIObj.LoadHPISymptoms(MDVUtility.ToLong(model.HPISymptomsId), model.IsActive, model.Name, MDVUtility.ToLong(model.PageNumber), MDVUtility.ToLong(model.RowsPerPage));
                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        SymptomsCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount,
                        listSymptoms = obj.Data,
                    };
                    return (JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        SymptomsCount = 0,
                        iTotalDisplayRecords = 0,
                        Message = "Record not found."
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
        public string DeleteHPISymptoms(string HPISymptomsId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(HPISymptomsId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLHPIObj.DeleteHPISymptoms(HPISymptomsId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }

        }
        public string FillHPISymptoms(HPISymptomsModel model)
        {
            try
            {
                BLObject<List<HPISymptomsModel>> objSymptom;
                BLObject<List<HPIFindingsModel>> objFinding;
                objSymptom = BLLHPIObj.LoadHPISymptoms(MDVUtility.ToLong(model.HPISymptomsId), "", "", 1, 25);
                if (objSymptom.Data != null)
                {
                    if (objSymptom.Data.Count > 0)
                    {
                        objFinding = BLLHPIObj.LoadHPISymptomFinding(MDVUtility.ToLong(model.HPISymptomsId));

                        var response = new
                        {
                            status = true,
                            SymptomsCount = objSymptom.Data.Count,
                            listSymptoms = objSymptom.Data,
                            FindingsCount = objFinding.Data.Count,
                            listFindings = objFinding.Data
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SymptomsCount = 0,
                            FindingsCount = 0,
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
                        SymptomsCount = 0,
                        Message = objSymptom.Message
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
        public string UpdateHPISymptoms(HPISymptomsModel model)
        {
            try
            {
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                BLLHPIObj.UpdateHPISymptoms(model);

                if (!string.IsNullOrEmpty(model.HPIFindingsIds))
                {
                    HPISymptomsModelResponse<List<HPISymptomFindingModel>> response_ = associateHPIFindingforSymptom(model);
                    if (response_.Status)
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Update_Message,
                            HPISymptomsId = model.HPISymptomsId,
                            listFindings = response_.Data,

                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            HPISymptomsId = model.HPISymptomsId,
                            Message = AppPrivileges.Update_Message + " " + response_.Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Update_Message,
                        HPISymptomsId = model.HPISymptomsId,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public HPISymptomsModelResponse<List<HPISymptomFindingModel>> associateHPIFindingforSymptom(HPISymptomsModel model)
        {
            HPISymptomsModelResponse<List<HPISymptomFindingModel>> response = new HPISymptomsModelResponse<List<HPISymptomFindingModel>>();
            HPISymptomFindingModel HPISymptomFindingModel = new HPISymptomFindingModel();
            response.Status = false;

            HPISymptomFindingModel.HPIFindingsIds = model.HPIFindingsIds;
            HPISymptomFindingModel.HPISymptomsId = model.HPISymptomsId;
            HPISymptomFindingModel.HPITemplateSymptomId = model.HPITemplateSymptomId == null ? "0" : model.HPITemplateSymptomId;
            HPISymptomFindingModel.FindingOrder = model.FindingOrder == "" ? null : model.FindingOrder;
            try
            {
                BLObject<List<HPISymptomFindingModel>> objObservation = BLLHPIObj.SaveHPISymptomFinding(HPISymptomFindingModel);
                if (objObservation.Data != null)
                {
                    response.Status = true;
                    response.Message = AppPrivileges.Save_Message;
                    response.Data = objObservation.Data;
                }
                else
                {
                    response.Status = false;
                    response.Message = objObservation.Message;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                return response;
            }
        }
        public string SaveHPISymptoms(HPISymptomsModel model)
        {
            try
            {
                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = DateTime.Now.ToString();
                model.IsGlobal = MDVUtility.ToStr(true);
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = DateTime.Now.ToString();
                model.HPITemplateId = model.HPITemplateId;

                List<HPISymptomsModel> hpiSymtoms = new List<HPISymptomsModel>();

                BLObject<List<HPISymptomsModel>> obj = BLLHPIObj.SaveHPISymptoms(model);
                if (obj.Data != null)
                {
                    hpiSymtoms = obj.Data;
                }
                model.HPISymptomsId = hpiSymtoms[0].HPISymptomsId;

                if (!string.IsNullOrEmpty(model.HPIFindingsIds))
                {
                    HPISymptomsModelResponse<List<HPISymptomFindingModel>> response_ = associateHPIFindingforSymptom(model);
                    if (response_.Status)
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Save_Message,
                            HPISymptomsId = model.HPISymptomsId,

                            listFindings = response_.Data,

                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            HPISymptomsId = model.HPISymptomsId,
                            Message = AppPrivileges.Save_Message + " " + response_.Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Message,
                        HPISymptomsId = model.HPISymptomsId,
                        TemplateSymptoms = obj.Data
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message
                };
                return (JsonConvert.SerializeObject(response));
            }


        }

        #endregion

        #region " HPI Symptoms Finding Fill, Save and Update Methods "
        public string DeleteHPISymptomFindings(string HPISymptomFindingsId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(HPISymptomFindingsId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.CheckBox_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLHPIObj.DeleteHPISymptomFindings(HPISymptomFindingsId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }

        }

        #endregion

        public string loadHPITemplates(HPITemplateModel model)
        {
            try
            {
                List<HPITemplateModel> listHPITemplate = new List<HPITemplateModel>();
                BLObject<List<HPITemplateModel>> obj = BLLHPIObj.loadHPITemplates(MDVUtility.ToLong(model.HPITemplateId), MDVUtility.ToInt32(model.EntityId), MDVUtility.ToInt32(model.IsActive));

                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listHPITemplate = obj.Data,
                        HPITemplateCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        listCustomForm = "[]",
                        customFormCount = 0,
                        iTotalDisplayRecords = 0
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

        public string insertUpdateHPITemplate(HPITemplateModel model)
        {
            HPIResponse responseModel = new HPIResponse();

            try
            {

                using (TextWriter writer = new StringWriter())
                {
                    GetProviderXMLTable(model.ProviderIds).WriteXml(writer);
                    model.ProviderXML = writer.ToString();
                }

                using (TextWriter writer = new StringWriter())
                {
                    GetSpecialtyXMLTable(model.SpecialtyIds).WriteXml(writer);
                    model.SpecialtyXML = writer.ToString();
                }

                using (TextWriter writer = new StringWriter())
                {
                    GetSymptomFindingsXMLTable(model).WriteXml(writer);
                    model.SymptomFindingXML = writer.ToString();
                }

                BLObject<string> obj = BLLHPIObj.insertupdateHPITemplate(model);

                if (obj.Data != null && obj.Data != "-1")
                {
                    responseModel.HPITemplateId = obj.Data.ToString();
                    responseModel.status = true;
                    responseModel.Message = Common.AppPrivileges.Save_Message;
                }
                else
                {
                    responseModel.status = false;
                    responseModel.HPITemplateId = model.HPITemplateId;
                    responseModel.Message = obj.Message;
                }
            }
            catch (Exception ex)
            {
                responseModel.status = false;
                responseModel.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.HPITemplateId = model.HPITemplateId;
            }

            if (responseModel.status)
            {
                var response = new
                {
                    status = true,
                    HPITemplateId = responseModel.HPITemplateId,
                    Message = Common.AppPrivileges.Save_Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = responseModel.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        DataTable GetProviderXMLTable(string providers)
        {
            DataTable ProviderTable = new DataTable() { TableName = "Providers" };
            ProviderTable.Columns.Add("Providerid", typeof(int));
            ProviderTable.Columns.Add("ShortName", typeof(string));

            string ProviderIds = providers;
            IList<string> providers_ = ProviderIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < providers_.Count; i++)
                ProviderTable.Rows.Add(providers_[i], "");

            return ProviderTable;
        }

        DataTable GetSpecialtyXMLTable(string Specialties)
        {
            DataTable SpecialtyTable = new DataTable() { TableName = "Specialties" };
            SpecialtyTable.Columns.Add("Specialtyid", typeof(int));
            SpecialtyTable.Columns.Add("ShortName", typeof(string));

            string SpecialtyIds = Specialties;
            IList<string> Specialties_ = SpecialtyIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < Specialties_.Count; i++)
                SpecialtyTable.Rows.Add(Specialties_[i], "");

            return SpecialtyTable;
        }

        DataTable GetSymptomFindingsXMLTable(HPITemplateModel model)
        {
            DataTable SymptomFindingsTable = new DataTable() { TableName = "HPITempPart" };

            SymptomFindingsTable.Columns.Add("HPITemplateId", typeof(string));
            SymptomFindingsTable.Columns.Add("HPISymptomsId", typeof(int));
            SymptomFindingsTable.Columns.Add("HPIFindingId", typeof(int));
            SymptomFindingsTable.Columns.Add("HPISymptomIsSelected", typeof(int));
            SymptomFindingsTable.Columns.Add("HPIFindingIsSelected", typeof(int));
            SymptomFindingsTable.Columns.Add("IsSymptomDeleted", typeof(int));
            SymptomFindingsTable.Columns.Add("IsFindingDeleted", typeof(int));
            SymptomFindingsTable.Columns.Add("HPISymptomsAnswersId", typeof(int));
            SymptomFindingsTable.Columns.Add("HPISymptoms_SeverityId", typeof(int));
            SymptomFindingsTable.Columns.Add("HPISymptoms_LocationIds", typeof(string));
            SymptomFindingsTable.Columns.Add("HPISymptoms_RadiationId", typeof(int));
            SymptomFindingsTable.Columns.Add("HPISymptoms_QualityId", typeof(int));
            SymptomFindingsTable.Columns.Add("Onset", typeof(string));
            SymptomFindingsTable.Columns.Add("AssociatedWith", typeof(string));
            SymptomFindingsTable.Columns.Add("HPISymptoms_AggravatedById", typeof(int));
            SymptomFindingsTable.Columns.Add("HPISymptoms_RelievedById", typeof(int));
            SymptomFindingsTable.Columns.Add("IsSymptomsDetail", typeof(string));
            SymptomFindingsTable.Columns.Add("SymptomOrder", typeof(int));
            SymptomFindingsTable.Columns.Add("FindingOrder", typeof(int));

            if (MDVUtility.ToInt64(model.HPITemplateId) != -1)
            {
                foreach (var tSymptomFindingData in model.SymptomFindingData)
                {
                    if (string.IsNullOrEmpty(tSymptomFindingData.FindingId)) continue;
                    if (MDVUtility.ToInt64(tSymptomFindingData.HPISymptomId) > 0)
                        SymptomFindingsTable.Rows.Add
                            (
                                string.IsNullOrEmpty(model.HPITemplateId) ? "-1" : model.HPITemplateId,
                                tSymptomFindingData.HPISymptomId,
                                tSymptomFindingData.FindingId,
                                MDVUtility.ToBool(tSymptomFindingData.IsSymptomChecked) ? 1 : 0,
                                MDVUtility.ToBool(tSymptomFindingData.IsChecked) ? 1 : 0,
                                tSymptomFindingData.IsSymptomDeleted,
                                tSymptomFindingData.IsFindingDeleted,
                                tSymptomFindingData.HPISymptomsAnswersId,
                                MDVUtility.ToStr(tSymptomFindingData.HPISymptoms_SeverityId) == "" ? "0" : tSymptomFindingData.HPISymptoms_SeverityId,
                                MDVUtility.ToStr(tSymptomFindingData.HPISymptoms_LocationIds) == "" ? "0" : tSymptomFindingData.HPISymptoms_LocationIds,
                                MDVUtility.ToStr(tSymptomFindingData.HPISymptoms_RadiationId) == "" ? "0" : tSymptomFindingData.HPISymptoms_RadiationId,
                                MDVUtility.ToStr(tSymptomFindingData.HPISymptoms_QualityId) == "" ? "0" : tSymptomFindingData.HPISymptoms_QualityId,
                                MDVUtility.ToStr(tSymptomFindingData.Onset) == "" ? "" : tSymptomFindingData.Onset,
                                MDVUtility.ToStr(tSymptomFindingData.AssociatedWith) == "" ? "" : tSymptomFindingData.AssociatedWith,
                                MDVUtility.ToStr(tSymptomFindingData.HPISymptoms_AggravatedById) == "" ? "0" : tSymptomFindingData.HPISymptoms_AggravatedById,
                                MDVUtility.ToStr(tSymptomFindingData.HPISymptoms_RelievedById) == "" ? "0" : tSymptomFindingData.HPISymptoms_RelievedById,
                                tSymptomFindingData.IsSymptomsDetail, 
                                tSymptomFindingData.SymptomOrder,
                                tSymptomFindingData.FindingOrder == "" ? "-1" : tSymptomFindingData.FindingOrder
                            );
                }
            }
            else
            {
                foreach (HPITemplateSymptomsModel tSymptomFindingData in model.SymptomFindingData)
                {
                    if (!string.IsNullOrEmpty(tSymptomFindingData.FindingId))
                    {
                        SymptomFindingsTable.Rows.Add
                            (
                                string.IsNullOrEmpty(tSymptomFindingData.HPITemplateId) ? "-1" : tSymptomFindingData.HPITemplateId,
                                tSymptomFindingData.HPISymptomId,
                                tSymptomFindingData.FindingId,
                                MDVUtility.ToBool(tSymptomFindingData.IsSymptomChecked) ? 1 : 0,
                                MDVUtility.ToBool(tSymptomFindingData.IsChecked) ? 1 : 0,
                                tSymptomFindingData.IsSymptomDeleted,
                                tSymptomFindingData.IsFindingDeleted,
                                tSymptomFindingData.HPISymptomsAnswersId,
                                MDVUtility.ToStr(tSymptomFindingData.HPISymptoms_SeverityId) == "" ? "0" : tSymptomFindingData.HPISymptoms_SeverityId,
                                MDVUtility.ToStr(tSymptomFindingData.HPISymptoms_LocationIds) == "" ? "0" : tSymptomFindingData.HPISymptoms_LocationIds,
                                MDVUtility.ToStr(tSymptomFindingData.HPISymptoms_RadiationId) == "" ? "0" : tSymptomFindingData.HPISymptoms_RadiationId,
                                MDVUtility.ToStr(tSymptomFindingData.HPISymptoms_QualityId) == "" ? "0" : tSymptomFindingData.HPISymptoms_QualityId,
                                MDVUtility.ToStr(tSymptomFindingData.Onset) == "" ? "" : tSymptomFindingData.Onset,
                                MDVUtility.ToStr(tSymptomFindingData.AssociatedWith) == "" ? "" : tSymptomFindingData.AssociatedWith,
                                MDVUtility.ToStr(tSymptomFindingData.HPISymptoms_AggravatedById) == "" ? "0" : tSymptomFindingData.HPISymptoms_AggravatedById,
                                MDVUtility.ToStr(tSymptomFindingData.HPISymptoms_RelievedById) == "" ? "0" : tSymptomFindingData.HPISymptoms_RelievedById,
                                tSymptomFindingData.IsSymptomsDetail,
                                tSymptomFindingData.SymptomOrder,
                                tSymptomFindingData.FindingOrder == "" ? "-1" : tSymptomFindingData.FindingOrder
                            );
                    }
                }
            }
            return SymptomFindingsTable;
        }

        public string LookupSymptoms(int? isactive = 1)
        {
            try
            {
                List<HPISymptomsLookupModel> listHPITemplate = new List<HPISymptomsLookupModel>();
                BLObject<List<HPISymptomsLookupModel>> obj = BLLHPIObj.lookupHPISymptoms(isactive);

                //List<HPISymptomsLookupModel> symptoms = new List<HPISymptomsLookupModel>();
                //HPISymptomsLookupModel sym = new HPISymptomsLookupModel();
                //sym.Name = "Symptom 1";
                //sym.IsGlobal = "1";
                //sym.IsActive = "1";
                //sym.HPISymptomId = "1";
                //symptoms.Add(sym);

                //sym = new HPISymptomsLookupModel();
                //sym.Name = "Symptom 2";
                //sym.IsGlobal = "1";
                //sym.IsActive = "1";
                //sym.HPISymptomId = "2";
                //symptoms.Add(sym);

                //sym = new HPISymptomsLookupModel();
                //sym.Name = "Symptom 3";
                //sym.IsGlobal = "1";
                //sym.IsActive = "1";
                //sym.HPISymptomId = "3";
                //symptoms.Add(sym);

                //System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                //if (symptoms.Count != 0)
                //{
                //    var response = new
                //    {
                //        status = true,
                //        HPISymptoms_JSON = js.Serialize(symptoms),
                //        HPISymptomsCount = symptoms.Count,
                //    };
                //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                //}


                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        HPISymptoms_JSON = obj.Data,
                        HPISymptomsCount = obj.Data.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        HPISymptoms_JSON = "[]",
                        HPISymptomsCount = 0,
                        iTotalDisplayRecords = 0
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


        public string FillHPITemplate(HPITemplateModel model)
        {
            try
            {
                List<HPITemplateModel> listHPISymFindingsModel = new List<HPITemplateModel>();

                BLObject<List<HPITemplateModel>> obj = BLLHPIObj.fillHPITemplate(MDVUtility.ToLong(model.HPITemplateId));

                if (obj.Data != null)
                {
                    listHPISymFindingsModel = obj.Data;
                }

                if (listHPISymFindingsModel != null && listHPISymFindingsModel.Count > 0)
                {
                    List<HPITemplateSymptomsModel> listHPISymModel = new List<HPITemplateSymptomsModel>(listHPISymFindingsModel[0].SymptomData);
                    List<HPITemplateSymptomFindingsModel> listHPIFindingmModel = new List<HPITemplateSymptomFindingsModel>(listHPISymFindingsModel[0].FindingsData);

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        HPITemplateCount = listHPISymFindingsModel.Count,
                        HPITemplate_JSON = js.Serialize(listHPISymFindingsModel),
                        HPITemplateSymptomsCount = listHPISymModel.GroupBy(s => s.HPISymptomId).Count(),
                        HPITemplateSymptoms_JSON = js.Serialize(listHPISymModel.GroupBy(s => s.HPISymptomId).Select(x => x.First())),
                        HPISymFindingsCount = listHPIFindingmModel.Count,
                        HPISymFindings_JSON = js.Serialize(listHPIFindingmModel),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        HPITemplate_JSON = "[]",
                        HPITemplateCount = 0,
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
        public string LoadHPISymptomsFindings(long hpiTemplateId, long symptomsId)
        {
            try
            {
                List<HPITemplateSymptomFindingsModel> listHPISymFindingsModel = new List<HPITemplateSymptomFindingsModel>();
                BLObject<List<HPITemplateSymptomFindingsModel>> obj = BLLHPIObj.LoadHPISymptomsFindings(hpiTemplateId, symptomsId);

                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        HPIFinding_JSON = obj.Data,
                        HPIFindingCount = obj.Data.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        HPIFinding_JSON = "[]",
                        HPIFindingCount = 0,
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
        public string LoadHPISymptomsFindingsDetail(long hpiSymptomDetailId, long hpiTemplateSymptomsId)
        {
            try
            {
                List<HPITemplateSymptomFindingsModel> listHPISymFindingsModel = new List<HPITemplateSymptomFindingsModel>();
                BLObject<List<HPITemplateSymptomFindingsModel>> obj = BLLHPIObj.LoadHPISymptomsFindingsDetail(hpiSymptomDetailId, hpiTemplateSymptomsId);

                if (obj.Data != null && obj.Data.Count != 0)
                {
                    var response = new
                    {
                        status = true,
                        HPISymptomsDetail_JSON = obj.Data,
                        HPISymptomsDetailCount = obj.Data.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        HPISymptomsDetail_JSON = "[]",
                        HPISymptomsDetailCount = 0,
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
        public string loadHPIForProvider(long providerId)
        {
            try
            {
                List<HPITemplateModel> listHPITemplateModel = new List<HPITemplateModel>();
                BLObject<List<HPITemplateModel>> obj = BLLHPIObj.loadHPIForProvider(providerId);

                if (obj.Data != null && obj.Data.Count != 0)
                {
                    var response = new
                    {
                        status = true,
                        HPITemplate_JSON = obj.Data,
                        HPITemplateCount = obj.Data.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        HPITemplate_JSON = "[]",
                        HPITemplateCount = 0,
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
        public string loadHPITempSymptomFindingsForNotes(long notesId)
        {
            try
            {
                List<HPINotesFindings> listHPIFindingNotesModel = new List<HPINotesFindings>();
                BLObject<List<HPINotesFindings>> obj = BLLHPIObj.loadHPITempSymptomFindingsForNotes(notesId);

                if (obj.Data != null && obj.Data.Count != 0)
                {
                    var response = new
                    {
                        status = true,
                        HPITemplate_JSON = obj.Data,
                        HPITemplateCount = obj.Data.Count,
                        HPITemplateSymptoms_JSON = obj.Data,
                        HPITemplateSymptomsCount = obj.Data.Count,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        HPITemplate_JSON = "[]",
                        HPITemplateCount = 0,
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
        public string deleteHPITemplateSymptom(string HPITemplateSymptomId)
        {
            try
            {
                if (string.IsNullOrEmpty(HPITemplateSymptomId))
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
                    BLObject<string> obj = BLLHPIObj.deleteHPITemplateSymptom(MDVUtility.ToLong(HPITemplateSymptomId));
                    if (obj.Data != null && obj.Data == "")
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
        public string deleteHPISymptomFinding(string symptomFindingId)
        {
            try
            {
                if (string.IsNullOrEmpty(symptomFindingId))
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
                    BLObject<string> obj = BLLHPIObj.deleteHPISymptomFinding(MDVUtility.ToLong(symptomFindingId));
                    if (obj.Data != null && obj.Data == "")
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

        public string loadHPITempSymptomFindingNote(long templateId, long systemId, long notesId)
        {
            try
            {
                List<HPITemplateModel> listHPIFindingNotesModel = new List<HPITemplateModel>();
                BLObject<List<HPITemplateModel>> obj = BLLHPIObj.loadHPITempSymptomFindingNote(templateId, systemId, notesId);

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();


                if (obj.Data != null && obj.Data.Count != 0)
                {
                    listHPIFindingNotesModel = obj.Data;

                    var response = new
                    {
                        status = true,
                        HPIFinding_JSON = js.Serialize(listHPIFindingNotesModel[0].FindingsData),
                        HPIFindingCount = listHPIFindingNotesModel[0].FindingsData.Count,
                        HPINotesFinding_JSON = js.Serialize(listHPIFindingNotesModel[0].NotesFindingsData)
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        HPIFinding_JSON = "[]",
                        HPIFindingCount = 0,
                        HPINotesFinding_JSON = "[]"
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

        public string loadHPITemplateSymptoms(long templateId, long entityId, int? IsActive = 1, int? isSelected = 1)
        {

            try
            {
                List<HPITemplateSymptomsModel> listHPISymptoms = new List<HPITemplateSymptomsModel>();
                BLObject<List<HPITemplateSymptomsModel>> obj = BLLHPIObj.loadHPITemplateSymptoms(templateId, entityId, IsActive, isSelected);

                if (obj.Data != null && obj.Data.Count != 0)
                {
                    var response = new
                    {
                        status = true,
                        HPITemplateSymptoms_JSON = obj.Data,
                        HPITemplateSymptomsCount = obj.Data.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        HPITemplateSymptoms_JSON = "[]",
                        HPITemplateSymptomsCount = 0,
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
        public string saveHPIComplaintNotesFinding(List<HPINotesFindings> model, string NotesId)
        {
            try
            {
                List<HPINotesFindings> listHPISymFindingsModel = new List<HPINotesFindings>();

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<HPINotesFindings>));
                StringWriter textWriter = new StringWriter();
                xmlSerializer.Serialize(textWriter, model);
                string xml = textWriter.ToString();
                BLObject<List<HPINotesFindings>> obj = null;
                if (model.Count > 0)
                {
                    obj = BLLHPIObj.saveHPIComplaintNotesFinding(xml);

                    if (obj.Data != null)
                    {
                        listHPISymFindingsModel = obj.Data;
                        listHPISymFindingsModel = listHPISymFindingsModel.Where(a => a.NotesId == NotesId).ToList();

                        if (listHPISymFindingsModel != null && listHPISymFindingsModel.Count > 0)
                        {
                            var templateList = (from a in listHPISymFindingsModel
                                                select new
                                                {
                                                    HPITemplateId = a.HPITemplateId,
                                                    TemplateName = a.TemplateName,
                                                    Comments = a.Comments
                                                }).Distinct().ToList();
                            var symptomsList = (from a in listHPISymFindingsModel
                                                select new
                                                {
                                                    HPITemplateId = a.HPITemplateId,
                                                    HPISymptomId = a.HPISymptomId,
                                                    HPITemplateSymptomId = a.HPITemplateSymptomId,
                                                    SymptomName = a.SymptomName,
                                                    Desc = a.Desc,
                                                    SymptomDescription = a.SymptomDescription,
                                                    HPINotesFindingsId = a.HPINotesFindingsId
                                                }).ToList();
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                HPITemplate_JSON = js.Serialize(templateList),
                                HPITemplateCount = templateList.Count,
                                HPITemplateSymptomCount = listHPISymFindingsModel.Count,
                                HPISymptoms_JSON = js.Serialize(symptomsList),
                                HPISymptomsCount = symptomsList.Count,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                HPITemplate_JSON = "[]",
                                HPITemplateCount = "0",
                                HPITemplateSymtoms_JSON = "[]",
                                HPITemplateSymptomCount = "0",
                                HPISymptoms_JSON = "[]",
                                HPISymptomsCount = 0,
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
                        status = true,
                        HPITemplate_JSON = "[]",
                        HPITemplateCount = "0",
                        HPITemplateSymtoms_JSON = "[]",
                        HPITemplateSymptomCount = "0",
                        HPISymptoms_JSON = "[]",
                        HPISymptomsCount = 0,
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
        public string isHPIComplaint(long notesId)
        {
            try
            {
                List<HPINotesFindings> listHPISymFindingsModel = new List<HPINotesFindings>();

                BLObject<string> obj = BLLHPIObj.isHPIComplaint(notesId);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        isHPIComplaint = obj.Data,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        isHPIComplaint = false
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

        public string deleteHPITemplate(long templateId)
        {
            string currenthpiTempId = string.Empty;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            BLObject<string> obj = BLLHPIObj.deleteHPITemplate(templateId);
            currenthpiTempId = obj.Data;
            if (string.IsNullOrEmpty(currenthpiTempId) || currenthpiTempId == "Successfully Deleted")
            {
                var response = new { status = true, message = "HPI template deleted successfully." };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new { status = false, message = currenthpiTempId };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        public string HPITemplateIsActive(long templateID, long IsActive)
        {
            try
            {
                BLObject<string> objInsertedNormalSystem = BLLHPIObj.HPITemplateIsActive(templateID, IsActive);
                if (objInsertedNormalSystem.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objInsertedNormalSystem.Data
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

        public string associateHPISymptomAndTemplate(HPITemplateSymptomsModel model)
        {
            try
            {
                BLObject<string> obj = BLLHPIObj.associateHPISymptomAndTemplate(model);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        HPITemplateSymptomId = obj.Data.ToString(),
                        Message = Common.AppPrivileges.Save_Message

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        HPITemplateId = model.HPITemplateSymptomId,
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


        public string associateHPIFindingAndSymptom(HPISymptomFindingModel model)
        {
            try
            {
                BLObject<List<HPISymptomFindingModel>> obj = BLLHPIObj.SaveHPISymptomFinding(model);


                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        Findings_JSON = obj.Data,
                        Message = Common.AppPrivileges.Save_Message

                    };
                    return (JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Findings_JSON = "[]",
                        Message = "Record not found."
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
        public string UpdateHPINotesSymptomFindingDesc(long HPINotesFindingId, string Desc)
        {
            try
            {
                BLObject<string> obj = BLLHPIObj.UpdateHPINotesSymptomFindingDesc(HPINotesFindingId, Desc);
                if (obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Update_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Data
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

    }
}