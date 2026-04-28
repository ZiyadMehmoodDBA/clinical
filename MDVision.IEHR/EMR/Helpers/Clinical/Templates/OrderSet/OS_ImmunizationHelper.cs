using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.IEHR.EMR.Model.Clinical.Immunization;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.Model.Clinical.Templates.OrderSets;
using MDVision.Model.Lookups;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates.OrderSet
{
    public class OS_ImmunizationHelper
    {
        //private BLLClinical BLLOrderSetObj = null;
        //private BLLRcopia BLLRcopiaObj = null;
        private BLLOrderSet BLLOrderSetObj = null;
        public OS_ImmunizationHelper()
        {
            BLLOrderSetObj = new BLLOrderSet();
            // BLLRcopiaObj = new BLLRcopia();
        }
        private static OS_ImmunizationHelper _instance = null;
        //private static bool isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
        public static OS_ImmunizationHelper Instance()
        {
            if (_instance == null)
            {
                _instance = new OS_ImmunizationHelper();
                //   isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
            }
            return _instance;
        }

        public string SaveAministerVaccine(OS_ImmunizationModel model)
        {
            try
            {
                DSOS_Immunization dsImmunization = new DSOS_Immunization();
                DSOS_Immunization.OS_VaccineHxRow dr = dsImmunization.OS_VaccineHx.NewOS_VaccineHxRow();

                dr.MainAgeGroup = MDVUtility.ToInt32(model.MainAgeGroup);
                dr.MainCategory = MDVUtility.ToInt64(model.MainCategory);
                dr.MainSchedule = MDVUtility.ToInt64(model.MainSchedule);
                dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);


                dr.OS_VaccineGroupCategory = MDVUtility.ToInt64(model.CategoryID);

                dr[dsImmunization.OS_VaccineHx.ProviderIdColumn] = MDVUtility.ToInt64(model.ProviderId);




                dr.OS_Vaccine = MDVUtility.ToInt64(model.VaccineID);



                if (!string.IsNullOrEmpty(model.Dose))
                {
                    dr.Dose = MDVUtility.ToDecimal(model.Dose);
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.DoseColumn] = DBNull.Value;
                }

                dr.Amount = model.Amount;

                if (!string.IsNullOrEmpty(model.LotNo))
                {
                    dr.OS_LotNumberId = MDVUtility.ToLong(model.LotNo);
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.OS_LotNumberIdColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.ManufacturerId))
                {
                    dr.Manufacturer = MDVUtility.ToInt64(model.ManufacturerId);
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.ManufacturerColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.RouteId))
                {
                    dr.OS_Route = MDVUtility.ToInt64(model.RouteId);
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.OS_RouteColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.SourceOfHxId))
                {
                    dr.SourceOfHx = MDVUtility.ToInt64(model.SourceOfHxId);
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.SourceOfHxColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.SiteId))
                {
                    dr.Site = MDVUtility.ToInt64(model.SiteId);
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.SiteColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.ExpiryDate))
                    dr.ExpiryDate = MDVUtility.ToDateTime(model.ExpiryDate);
                else
                    dr[dsImmunization.OS_VaccineHx.ExpiryDateColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.VfcId))
                {
                    dr.OS_VFC = MDVUtility.ToInt64(model.VfcId);
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.OS_VFCColumn] = DBNull.Value;
                }


                if (MDVUtility.ToInt64(model.VisDateId) != 0)
                {
                    dr.VISDate = MDVUtility.ToInt64(model.VisDateId);
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.VISDateColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.Reaction))
                {
                    dr.Reaction = MDVUtility.ToInt64(model.Reaction);
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.ReactionColumn] = DBNull.Value;
                }
                dr.VoidDose = model.VoidDose;

                if (!string.IsNullOrEmpty(model.Comments))
                {
                    dr.OS_Comments = model.Comments;
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.OS_CommentsColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.PublicityCode))
                {
                    dr.PublicityCode = model.PublicityCode;
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.PublicityCodeColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.PublicityCodeExpiryDate))
                    dr.PublicityCodeExpiryDate = MDVUtility.ToDateTime(model.PublicityCodeExpiryDate);
                else
                    dr[dsImmunization.OS_VaccineHx.PublicityCodeExpiryDateColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.ImmunizationRegistryStatusCode))
                {
                    dr.ImmunizationRegistryStatusCode = model.ImmunizationRegistryStatusCode;
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.ImmunizationRegistryStatusCodeColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.IRSEffectiveDate))
                    dr.IRSEffectiveDate = MDVUtility.ToDateTime(model.IRSEffectiveDate);
                else
                    dr[dsImmunization.OS_VaccineHx.IRSEffectiveDateColumn] = DBNull.Value;


                dr.ProtectionIndicator = model.ProtectionIndicator;
                if (!string.IsNullOrEmpty(model.PIEffectiveDate))
                    dr.PIEffectiveDate = MDVUtility.ToDateTime(model.PIEffectiveDate);
                else
                    dr[dsImmunization.OS_VaccineHx.PIEffectiveDateColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.RefusalReasonID))
                    dr.RefusalReasonId = MDVUtility.ToLong(model.RefusalReasonID);
                else
                    dr[dsImmunization.OS_VaccineHx.RefusalReasonIdColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.Type) && model.Type == "ADMINISTER")
                {
                    dr.OverrideRule = model.OverrideRule;
                }
                else
                {
                    dr[dsImmunization.OS_VaccineHx.OverrideRuleColumn] = DBNull.Value;
                }
                dr.GivenBy = MDVSession.Current.AppUserId;
                dr.Type = (model.Type);
                dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                dr.ISActive = MDVUtility.ToByte(model.IsActive);

                //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields
                dr.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

                #region Database Insertion
                dsImmunization.OS_VaccineHx.AddOS_VaccineHxRow(dr);
                BLObject<DSOS_Immunization> obj = BLLOrderSetObj.InsertVaccineHx(dsImmunization);
                dsImmunization = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        VaccineHxIdColumn = dsImmunization.Tables[dsImmunization.OS_VaccineHx.TableName].Rows[0][dsImmunization.OS_VaccineHx.OS_VaccineHxIdColumn.ColumnName]
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


        public string updateAdministerVaccine(OS_ImmunizationModel model)
        {
            try
            {
                DSOS_Immunization dsImmunization = new DSOS_Immunization();
                DSOS_Immunization.OS_VaccineHxRow dr = null;

                var obj = BLLOrderSetObj.SearchVacinehxForEdit(MDVUtility.ToInt64(model.VaccineHxId));

                dsImmunization = obj.Data;
                if (obj.Data != null)
                {

                    if (dsImmunization.Tables[dsImmunization.OS_VaccineHx.TableName].Rows.Count > 0)
                    {
                        dr = (DSOS_Immunization.OS_VaccineHxRow)dsImmunization.Tables[dsImmunization.OS_VaccineHx.TableName].Rows[0];

                        dr.OS_VaccineGroupCategory = MDVUtility.ToInt64(model.CategoryID);

                        dr[dsImmunization.OS_VaccineHx.ProviderIdColumn] = MDVUtility.ToInt64(model.ProviderId);




                        dr.OS_Vaccine = MDVUtility.ToInt64(model.VaccineID);



                        if (!string.IsNullOrEmpty(model.Dose))
                        {
                            dr.Dose = MDVUtility.ToDecimal(model.Dose);
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.DoseColumn] = DBNull.Value;
                        }

                        dr.Amount = model.Amount;

                        if (!string.IsNullOrEmpty(model.LotNo))
                        {
                            dr.OS_LotNumberId = MDVUtility.ToLong(model.LotNo);
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.OS_LotNumberIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.ManufacturerId))
                        {
                            dr.Manufacturer = MDVUtility.ToInt64(model.ManufacturerId);
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.ManufacturerColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.RouteId))
                        {
                            dr.OS_Route = MDVUtility.ToInt64(model.RouteId);
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.OS_RouteColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.SourceOfHxId))
                        {
                            dr.SourceOfHx = MDVUtility.ToInt64(model.SourceOfHxId);
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.SourceOfHxColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.SiteId))
                        {
                            dr.Site = MDVUtility.ToInt64(model.SiteId);
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.SiteColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.ExpiryDate))
                            dr.ExpiryDate = MDVUtility.ToDateTime(model.ExpiryDate);
                        else
                            dr[dsImmunization.OS_VaccineHx.ExpiryDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.VfcId))
                        {
                            dr.OS_VFC = MDVUtility.ToInt64(model.VfcId);
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.OS_VFCColumn] = DBNull.Value;
                        }


                        if (MDVUtility.ToInt64(model.VisDateId) != 0)
                        {
                            dr.VISDate = MDVUtility.ToInt64(model.VisDateId);
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.VISDateColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.Reaction))
                        {
                            dr.Reaction = MDVUtility.ToInt64(model.Reaction);
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.ReactionColumn] = DBNull.Value;
                        }
                        dr.VoidDose = model.VoidDose;

                        if (!string.IsNullOrEmpty(model.Comments))
                        {
                            dr.OS_Comments = model.Comments;
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.OS_CommentsColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.PublicityCode))
                        {
                            dr.PublicityCode = model.PublicityCode;
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.PublicityCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.PublicityCodeExpiryDate))
                            dr.PublicityCodeExpiryDate = MDVUtility.ToDateTime(model.PublicityCodeExpiryDate);
                        else
                            dr[dsImmunization.OS_VaccineHx.PublicityCodeExpiryDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.ImmunizationRegistryStatusCode))
                        {
                            dr.ImmunizationRegistryStatusCode = model.ImmunizationRegistryStatusCode;
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.ImmunizationRegistryStatusCodeColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.IRSEffectiveDate))
                            dr.IRSEffectiveDate = MDVUtility.ToDateTime(model.IRSEffectiveDate);
                        else
                            dr[dsImmunization.OS_VaccineHx.IRSEffectiveDateColumn] = DBNull.Value;


                        dr.ProtectionIndicator = model.ProtectionIndicator;
                        if (!string.IsNullOrEmpty(model.PIEffectiveDate))
                            dr.PIEffectiveDate = MDVUtility.ToDateTime(model.PIEffectiveDate);
                        else
                            dr[dsImmunization.OS_VaccineHx.PIEffectiveDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(model.RefusalReasonID))
                            dr.RefusalReasonId = MDVUtility.ToLong(model.RefusalReasonID);
                        else
                            dr[dsImmunization.OS_VaccineHx.RefusalReasonIdColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.Type) && model.Type == "ADMINISTER")
                        {
                            dr.OverrideRule = model.OverrideRule;
                        }
                        else
                        {
                            dr[dsImmunization.OS_VaccineHx.OverrideRuleColumn] = DBNull.Value;
                        }
                        dr.GivenBy = MDVSession.Current.AppUserId;
                        dr.Type = (model.Type);
                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                        dr.ISActive = MDVUtility.ToByte(model.IsActive);

                        //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields

                        dr.ModifiedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

                    }

                    obj = BLLOrderSetObj.UpdateVaccineHx(dsImmunization);
                    dsImmunization = obj.Data;

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message,
                            VaccineHxIdColumn = dsImmunization.Tables[dsImmunization.OS_VaccineHx.TableName].Rows[0][dsImmunization.OS_VaccineHx.OS_VaccineHxIdColumn.ColumnName]
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = obj.Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = obj.Message,
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

        public string DeleteOsVaccinehx(OS_ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.VaccineHxId)))
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

                    BLObject<string> obj = BLLOrderSetObj.DeleteOsVaccinehx(MDVUtility.ToStr(model.VaccineHxId));
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

        public string loadParentChildImmunization(OS_ImmunizationModel model)
        {
            try
            {
                DSOS_Immunization dsImmunization = null;
                BLObject<DSOS_Immunization> obj;
                obj = BLLOrderSetObj.loadParentChildImmunization(MDVUtility.ToInt64(model.OrderSetId),(string.IsNullOrEmpty(model.NotesId)?0:MDVUtility.ToInt64(model.NotesId)), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsImmunization = obj.Data;

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, string>> listChilImunization = new List<Dictionary<string, string>>();

                if (obj.Data != null)
                {
                    // int ImmunizationTotalCount = 0;
                    List<OS_ParentChildImunizationModel> immunizationList = new List<OS_ParentChildImunizationModel>();
                    if (dsImmunization.Tables[dsImmunization.ParentOS_VaccineHx.TableName].Rows.Count > 0)
                    {

                        foreach (DSOS_Immunization.ParentOS_VaccineHxRow row in dsImmunization.Tables[dsImmunization.ParentOS_VaccineHx.TableName].Rows)
                        {
                            OS_ParentChildImunizationModel immunizationViewModel = new OS_ParentChildImunizationModel();
                            //Add Parent Records
                            immunizationViewModel.ParentImmunization.VaccineHxId = MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.VaccineHxIdColumn]);
                            immunizationViewModel.ParentImmunization.VaccineId = MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.vaccineColumn]);
                            immunizationViewModel.ParentImmunization.VaccineName = MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.CVXShortDescriptionColumn]);
                            immunizationViewModel.ParentImmunization.Dose = MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.DoseColumn]).Trim() == "ml"
                                                                           ? "" : MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.DoseColumn]);
                            immunizationViewModel.ParentImmunization.AdministrationDate = MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.AdministrationDateColumn]);
                            immunizationViewModel.ParentImmunization.GivenBy = MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.GivenByColumn]);
                            immunizationViewModel.ParentImmunization.Location = MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.LocationColumn]);
                            immunizationViewModel.ParentImmunization.LotNumber = MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.LotNumberColumn]);
                            immunizationViewModel.ParentImmunization.Type = MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.TypeColumn]);
                            immunizationViewModel.ParentImmunization.IsNoteLinked = MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.IsNoteLinkedColumn]);
                            immunizationViewModel.ParentImmunization.Category = MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.CategoryColumn]);
                            immunizationViewModel.ParentImmunization.VaccineScheduleId = MDVUtility.ToStr(row[dsImmunization.ParentOS_VaccineHx.VaccineScheduleIdColumn]);

                            if (dsImmunization.ChildOS_VaccineHx.Rows.Count > 0)
                            {
                                DSOS_Immunization.ChildOS_VaccineHxRow[] childImunizationRows = (DSOS_Immunization.ChildOS_VaccineHxRow[])dsImmunization.ChildOS_VaccineHx.Select(dsImmunization.ChildOS_VaccineHx.vaccineColumn.ColumnName + " = " + row.vaccine);
                                // Add child Records
                                foreach (var dr in childImunizationRows)
                                {
                                    var childImunizationRow = new OS_Immunization
                                    {
                                        VaccineId = MDVUtility.ToStr(dr[dsImmunization.ChildOS_VaccineHx.vaccineColumn]),
                                        VaccineName = MDVUtility.ToStr(dr[dsImmunization.ChildOS_VaccineHx.CVXShortDescriptionColumn]),
                                        Dose = MDVUtility.ToStr(dr[dsImmunization.ChildOS_VaccineHx.DoseColumn]),
                                        AdministrationDate = MDVUtility.ToStr(dr[dsImmunization.ChildOS_VaccineHx.AdministrationDateColumn]),
                                        GivenBy = MDVUtility.ToStr(dr[dsImmunization.ChildOS_VaccineHx.GivenByColumn]),
                                        Location = MDVUtility.ToStr(dr[dsImmunization.ChildOS_VaccineHx.LocationColumn]),
                                        LotNumber = MDVUtility.ToStr(dr[dsImmunization.ChildOS_VaccineHx.LotNumberColumn]),
                                        Type = MDVUtility.ToStr(dr[dsImmunization.ChildOS_VaccineHx.TypeColumn]),
                                        IsNoteLinked = MDVUtility.ToStr(dr[dsImmunization.ChildOS_VaccineHx.IsNoteLinkedColumn]),
                                        VaccineHxId = MDVUtility.ToStr(dr[dsImmunization.ChildOS_VaccineHx.VaccineHxIdColumn]),
                                        Category = MDVUtility.ToStr(dr[dsImmunization.ChildOS_VaccineHx.CategoryColumn]),
                                        VaccineScheduleId = MDVUtility.ToStr(dr[dsImmunization.ChildOS_VaccineHx.VaccineScheduleIdColumn]),
                                    };
                                    immunizationViewModel.ChildImmunizationList.Add(childImunizationRow);
                                }
                            }
                            immunizationList.Add(immunizationViewModel);
                        }
                        var response = new
                        {
                            status = true,
                            ImmunizationParentChildLoad_JSON = js.Serialize(immunizationList),
                            iTotalDisplayRecords = MDVUtility.ToStr(dsImmunization.ParentOS_VaccineHx.Rows[0][dsImmunization.ParentOS_VaccineHx.RecordCountColumn.ColumnName]),
                            ParentImmunizationCount = dsImmunization.Tables[dsImmunization.ParentOS_VaccineHx.TableName].Rows.Count,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ImmunizationParentChildLoad_JSON = js.Serialize(immunizationList),
                            ParentImmunizationCount = 0
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ImmunizationCount = 0,
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



        public string GetAgeLimScheCategAgainstVaccShedId(OS_ImmunizationModel model)
        {
            try
            {
                DSOS_Immunization dsImmunization = null;
                BLObject<DSOS_Immunization> obj;
                obj = BLLOrderSetObj.GetAgeLimScheCategAgainstVaccShedId(MDVUtility.ToInt64(model.VaccineScheduleId));
                dsImmunization = obj.Data;
                if (obj.Data != null)
                {
                    if (dsImmunization.Tables[dsImmunization.ScheduleDetail.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ScheduleDetailLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.ScheduleDetail.TableName]),
                            ScheduleDetailCount = MDVUtility.ToStr(dsImmunization.Tables[dsImmunization.ScheduleDetail.TableName].Rows.Count)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ScheduleDetailLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.ScheduleDetail.TableName]),
                            ScheduleDetailCount = 0
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        ScheduleDetailCount = 0,
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

        public string SearchVacinehxForEdit(OS_ImmunizationModel model)
        {

            try
            {
                DSOS_Immunization dsImmunization = null;
                BLObject<DSOS_Immunization> obj;

                //Start || 22 April, 2016 || ZeeshanAK || Changes for audit
                obj = BLLOrderSetObj.SearchVacinehxForEdit(MDVUtility.ToInt64(model.VaccineHxId));
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                dsImmunization = obj.Data;
                if (obj.Data != null)
                {
                    List<OS_ImmunizationAdministerVaccineHx> viewModelAdmin = new List<OS_ImmunizationAdministerVaccineHx>();
                    List<OS_ImmunizationDocumentHxDoseHx> viewModelDoc = new List<OS_ImmunizationDocumentHxDoseHx>();
                    List<OS_ImmunizationRefusalRecord> viewModelRef = new List<OS_ImmunizationRefusalRecord>();

                    if (dsImmunization.Tables[dsImmunization.OS_VaccineHx.TableName].Rows.Count > 0)
                    {
                        foreach (DSOS_Immunization.OS_VaccineHxRow row in dsImmunization.Tables[dsImmunization.OS_VaccineHx.TableName].Rows)
                        {
                            if (row[dsImmunization.OS_VaccineHx.TypeColumn].ToString().Trim().ToLower() == "administer")
                            {
                                OS_ImmunizationAdministerVaccineHx immunizationViccineHxModel = new OS_ImmunizationAdministerVaccineHx();

                                immunizationViccineHxModel.VaccineHxId = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_VaccineHxIdColumn]);
                                immunizationViccineHxModel.AdministerVaccine_Category = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_VaccineGroupCategoryColumn]);

                                immunizationViccineHxModel.AdministerVaccine_Vaccine = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_VaccineColumn]);
                                immunizationViccineHxModel.AdministerVaccine_Provider = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.ProviderIdColumn]);
                                immunizationViccineHxModel.AdministerVaccine_Dose = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.DoseColumn]);
                                immunizationViccineHxModel.AdministerVaccine_Amount = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.AmountColumn]);
                                immunizationViccineHxModel.AdministerVaccine_LotNumber = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_LotNumberIdColumn]);
                                immunizationViccineHxModel.AdministerVaccine_Manufacturer = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.ManufacturerColumn]);
                                immunizationViccineHxModel.AdministerVaccine_Route = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_RouteColumn]);
                                immunizationViccineHxModel.AdministerVaccine_Site = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.SiteColumn]);
                                immunizationViccineHxModel.AdministerVaccine_ExpiryDate = string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.ExpiryDateColumn])) ? "" : MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.OS_VaccineHx.ExpiryDateColumn]).ToShortDateString());
                                immunizationViccineHxModel.AdministerVaccine_VFC = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_VFCColumn]);
                                immunizationViccineHxModel.AdministerVaccine_VISDate = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.OS_VaccineHx.VIS_EditionDateColumn]).ToShortDateString());
                                immunizationViccineHxModel.AdministerReaction = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.ReactionColumn]);
                                if (row[dsImmunization.OS_VaccineHx.VoidDoseColumn] != DBNull.Value)
                                {
                                    immunizationViccineHxModel.AdministerVoidDose = Convert.ToBoolean(row[dsImmunization.OS_VaccineHx.VoidDoseColumn]) == true ? "1" : "0";
                                }
                                else
                                {
                                    immunizationViccineHxModel.AdministerVoidDose = "0";
                                }
                                immunizationViccineHxModel.AdministerVaccine_Comments = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_CommentsColumn]);
                                immunizationViccineHxModel.AdministerVaccine_PublicityCode = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.PublicityCodeColumn]);
                                immunizationViccineHxModel.AdministerVaccine_PublicityExpiryDate = MDVUtility.ToStr(MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.PublicityCodeExpiryDateColumn]) != "" ? MDVUtility.ToDateTime(row[dsImmunization.OS_VaccineHx.PublicityCodeExpiryDateColumn]).ToShortDateString() : row[dsImmunization.OS_VaccineHx.PublicityCodeExpiryDateColumn]);
                                immunizationViccineHxModel.AdministerVaccine_IRS = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.ImmunizationRegistryStatusCodeColumn]);
                                immunizationViccineHxModel.AdministerVaccine_IRSEffectiveDate = MDVUtility.ToStr(MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.IRSEffectiveDateColumn]) != "" ? (MDVUtility.ToDateTime(row[dsImmunization.OS_VaccineHx.IRSEffectiveDateColumn]).ToShortDateString()) : row[dsImmunization.OS_VaccineHx.IRSEffectiveDateColumn]);
                                immunizationViccineHxModel.AdministerVaccine_ProtectionIndicator = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.ProtectionIndicatorColumn]);
                                immunizationViccineHxModel.AdministerVaccine_PIEffectiveDate = MDVUtility.ToStr(MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.PIEffectiveDateColumn]) != "" ? MDVUtility.ToDateTime(row[dsImmunization.OS_VaccineHx.PIEffectiveDateColumn]).ToShortDateString() : row[dsImmunization.OS_VaccineHx.PIEffectiveDateColumn]);
                                immunizationViccineHxModel.AdministerVaccine_VisitDate = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.VisitDateIdColumn]);
                                immunizationViccineHxModel.AdministerVaccine_IsActive = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.ISActiveColumn]);
                                immunizationViccineHxModel.LotText = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.LotTextColumn]);
                                immunizationViccineHxModel.AdministerOverrideRule = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OverrideRuleColumn]);
                                viewModelAdmin.Add(immunizationViccineHxModel);
                            }
                            else if (row[dsImmunization.OS_VaccineHx.TypeColumn].ToString().Trim().ToLower() == "documenthx")
                            {
                                OS_ImmunizationDocumentHxDoseHx immunizationViccineHxModel = new OS_ImmunizationDocumentHxDoseHx();
                                immunizationViccineHxModel.VaccineHxId = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_VaccineHxIdColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Provider = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.ProviderIdColumn]);
                                immunizationViccineHxModel.DocumentHxDose_SourceOfHx = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.SourceOfHxColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Category = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_VaccineGroupCategoryColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Vaccine = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_VaccineColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Dose = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.DoseColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Amount = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.AmountColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Site = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.SiteColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Route = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_RouteColumn]);
                                immunizationViccineHxModel.DocumentHxDose_Comments = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_CommentsColumn]);
                                if (row[dsImmunization.OS_VaccineHx.VoidDoseColumn] != DBNull.Value)
                                {
                                    immunizationViccineHxModel.DocumentHxVoidDose = Convert.ToBoolean(row[dsImmunization.OS_VaccineHx.VoidDoseColumn]) == true ? "1" : "0";
                                }
                                else
                                {
                                    immunizationViccineHxModel.DocumentHxVoidDose = "0";
                                }
                                immunizationViccineHxModel.DocumentHxDose_IsActive = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.ISActiveColumn]);
                                viewModelDoc.Add(immunizationViccineHxModel);
                            }
                            else if (row[dsImmunization.OS_VaccineHx.TypeColumn].ToString().Trim().ToLower() == "refusal")
                            {
                                OS_ImmunizationRefusalRecord immunizationViccineHxModel = new OS_ImmunizationRefusalRecord();
                                immunizationViccineHxModel.VaccineHxId = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_VaccineHxIdColumn]);
                                immunizationViccineHxModel.RecordRefusal_Provider = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.ProviderIdColumn]);
                                immunizationViccineHxModel.RecordRefusal_Category = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_VaccineGroupCategoryColumn]);
                                immunizationViccineHxModel.RecordRefusal_Vaccine = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_VaccineColumn]);
                                immunizationViccineHxModel.RecordRefusalReason = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.RefusalReasonIdColumn]);
                                immunizationViccineHxModel.RecordRefusalVaccine_ExpiryDate = string.IsNullOrEmpty(MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.ExpiryDateColumn])) ? "" : MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.OS_VaccineHx.ExpiryDateColumn]).ToShortDateString());
                                immunizationViccineHxModel.RecordRefusal_Comments = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.OS_CommentsColumn]);
                                if (row[dsImmunization.OS_VaccineHx.VoidDoseColumn] != DBNull.Value)
                                {
                                    immunizationViccineHxModel.RecordRefusalVoidDose = Convert.ToBoolean(row[dsImmunization.OS_VaccineHx.VoidDoseColumn]) == true ? "1" : "0";
                                }
                                else
                                {
                                    immunizationViccineHxModel.RecordRefusalVoidDose = "0";
                                }
                                immunizationViccineHxModel.RecordRefusal_IsActive = MDVUtility.ToStr(row[dsImmunization.OS_VaccineHx.ISActiveColumn]);
                                viewModelRef.Add(immunizationViccineHxModel);
                            }
                        }
                    }

                    var response = new
                    {
                        status = true,
                        AdminVaccineHxLoad_JSON = js.Serialize(viewModelAdmin),
                        DocVaccineHxLoad_JSON = js.Serialize(viewModelDoc),
                        RefusalVaccineLoad_JSON = js.Serialize(viewModelRef)

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ImmunizationCount = 0,
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

        public string IsVaccineHxInValidAge(OS_ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.OS_VaccineHxId)))
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

                    BLObject<string> obj = BLLOrderSetObj.IsVaccineHxInValidAge(model.OS_VaccineHxId, MDVUtility.ToInt64(model.PatientId));
                    if (obj.Data == "1" || obj.Data == "0")
                    {
                        var response = new
                        {
                            status = true,
                            IsVaccineHxInValidAge = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Exception in VaccineHxIsInValidAge"
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

        public string IsVaccineHxLotIssue(OS_ImmunizationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.OS_VaccineHxId)))
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

                    BLObject<string> obj = BLLOrderSetObj.IsVaccineHxLotIssue(MDVUtility.ToInt64(model.OS_VaccineHxId), model.Type,model.ImmunizationIds);
                    if (obj.Data == "1" || obj.Data == "0" || obj.Data == "2")
                    {
                        var response = new
                        {
                            status = true,
                            IsVaccineHxLotIssue = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Exception in IsVaccineHxLotIssue"
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

        public string Save_Therapeutic_Injection(TherapeuticInjectionModel model)
        {
            try
            {
                DSImmunization dsImmunization = new DSImmunization();
                DSImmunization.TherapeuticInjectionRow dr = dsImmunization.TherapeuticInjection.NewTherapeuticInjectionRow();

                dr.OSImmTherInjectionId = MDVUtility.ToInt64(model.OSImmTherInjectionId);
                dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
                dr.TherapeuticInjectionId = MDVUtility.ToInt64(model.TherapeuticInjectionId);

                dr[dsImmunization.TherapeuticInjection.VisitDateColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.ProviderId))
                    dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                else
                    dr[dsImmunization.TherapeuticInjection.ProviderIdColumn] = DBNull.Value;


                dr[dsImmunization.TherapeuticInjection.AdministrationDateColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.Dose))
                {
                    dr.Dose = MDVUtility.ToDouble(model.Dose);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.DoseColumn] = DBNull.Value;
                }
                dr.Amount = model.Amount;

                if (!string.IsNullOrEmpty(model.ManufacturerId))
                {
                    dr.ManufacturerId = MDVUtility.ToInt64(model.ManufacturerId);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.ManufacturerIdColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.RouteId))
                {
                    dr.RouteId = MDVUtility.ToInt64(model.RouteId);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.RouteIdColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.SiteId))
                {
                    dr.SiteId = MDVUtility.ToInt64(model.SiteId);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.SiteIdColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.ExpiryDate))
                    dr.ExpiryDate = MDVUtility.ToDateTime(model.ExpiryDate);
                else
                    dr[dsImmunization.TherapeuticInjection.ExpiryDateColumn] = DBNull.Value;

                if (!string.IsNullOrEmpty(model.VFC))
                {
                    dr.VFC = MDVUtility.ToInt64(model.VFC);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.VFCColumn] = DBNull.Value;
                }



                if (!string.IsNullOrEmpty(model.ReactionId))
                {
                    dr.ReactionId = MDVUtility.ToInt16(model.ReactionId);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.ReactionIdColumn] = DBNull.Value;
                }


                if (!string.IsNullOrEmpty(model.Comments))
                {
                    dr.Comments = model.Comments;
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.CommentsColumn] = DBNull.Value;
                }


                if (!string.IsNullOrEmpty(model.SourceOfHx))
                {
                    dr.SourceOfHx = MDVUtility.ToInt64(model.SourceOfHx);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.SourceOfHxColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.LotNumber))
                {
                    dr.LotNumber = MDVUtility.ToInt64(model.LotNumber);
                }
                else
                {
                    dr[dsImmunization.TherapeuticInjection.LotNumberColumn] = DBNull.Value;
                }

                dr.Type = model.Type;

                dr[dsImmunization.TherapeuticInjection.PatientIdColumn] = DBNull.Value;

                //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields

                dr.ModifiedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                dr.CreatedOn = DateTime.Now;
                dr.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);

                #region Database Insertion
                dsImmunization.TherapeuticInjection.AddTherapeuticInjectionRow(dr);
                BLObject<DSImmunization> obj = BLLOrderSetObj.OS_InsertTherapeuticInjection(dsImmunization);
                dsImmunization = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        TherapeuticInjectionIdColumn = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows[0][dsImmunization.TherapeuticInjection.ImmTherInjectionIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = obj.Message
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

        public string SearchImmunizationTherapeuticInjection(TherapeuticInjectionModel model)
        {
            try
            {

                DSImmunization dsImmunization = null;
                BLObject<DSImmunization> obj;

                obj = BLLOrderSetObj.OS_LoadImmunizationTherapeuticInjection(MDVUtility.ToInt64(model.OSImmTherInjectionId), MDVUtility.ToInt64(model.OrderSetId),(string.IsNullOrEmpty(model.NotesId)?0:MDVUtility.ToInt64(model.NotesId)), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                dsImmunization = obj.Data;
                if (obj.Data != null)
                {

                    if (!string.IsNullOrEmpty(model.OSImmTherInjectionId))
                    {
                        if (model.Type == "DocumentHx")
                        {
                            List<TherapeuticInjectionHistoryModel> TherapeuticInjectionList = new List<TherapeuticInjectionHistoryModel>();

                            if (dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count > 0)
                            {

                                foreach (DSImmunization.TherapeuticInjectionRow row in dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows)
                                {
                                    TherapeuticInjectionHistoryModel therapeuticInjectionModel = new TherapeuticInjectionHistoryModel();
                                    therapeuticInjectionModel.OSImmTherInjectionId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.OSImmTherInjectionIdColumn]);
                                    therapeuticInjectionModel.TherapeuticInjectionIdHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.TherapeuticInjectionIdColumn]);
                                    therapeuticInjectionModel.ProviderIdHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.ProviderIdColumn]);
                                    if (row[dsImmunization.TherapeuticInjection.AdministrationDateColumn] != DBNull.Value)
                                    {
                                        therapeuticInjectionModel.AdministrationDateHistory = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.TherapeuticInjection.AdministrationDateColumn]).ToShortDateString());
                                    }
                                    else
                                    {
                                        therapeuticInjectionModel.AdministrationDateHistory = "";
                                    }
                                    if (row[dsImmunization.TherapeuticInjection.AdministrationDateColumn] != DBNull.Value)
                                    {
                                        therapeuticInjectionModel.AdministrationTimeHistory = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.TherapeuticInjection.AdministrationDateColumn]).ToShortTimeString());
                                    }
                                    else
                                    {
                                        therapeuticInjectionModel.AdministrationDateHistory = "";
                                    }

                                    therapeuticInjectionModel.DoseHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.DoseColumn]);
                                    therapeuticInjectionModel.AmountHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.AmountColumn]);
                                    therapeuticInjectionModel.SiteIdHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.SiteIdColumn]);
                                    therapeuticInjectionModel.RouteIdHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.RouteIdColumn]);
                                    therapeuticInjectionModel.CommentsHistory = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.CommentsColumn]);
                                    therapeuticInjectionModel.SourceOfHx = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.SourceOfHxColumn]);
                                    therapeuticInjectionModel.CPTCode = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.CPTCodeColumn]);
                                    therapeuticInjectionModel.TherapeuticInjection = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.TherapeuticInjectionColumn]);
                                    therapeuticInjectionModel.LinkedWithAnyNote = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.LinkedWithAnyNoteColumn]);
                                    therapeuticInjectionModel.GivenBy = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.GivenByColumn]);

                                    TherapeuticInjectionList.Add(therapeuticInjectionModel);
                                }
                                var response = new
                                {
                                    status = true,
                                    TherapeuticInjectionCount = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count,
                                    TherapeuticInjectionLoad_JSON = js.Serialize(TherapeuticInjectionList),
                                    iTotalDisplayRecords = (dsImmunization.TherapeuticInjection.Rows.Count > 0) ? dsImmunization.TherapeuticInjection.Rows[0][dsImmunization.TherapeuticInjection.RecordCountColumn.ColumnName] : 0,
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    TherapeuticInjectionCount = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count,
                                    TherapeuticInjectionLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName]),
                                    iTotalDisplayRecords = (dsImmunization.TherapeuticInjection.Rows.Count > 0) ? dsImmunization.TherapeuticInjection.Rows[0][dsImmunization.TherapeuticInjection.RecordCountColumn.ColumnName] : 0,
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }
                        else if (model.Type == "Administered")
                        {
                            List<TherapeuticInjectionModel> TherapeuticInjectionList = new List<TherapeuticInjectionModel>();

                            if (dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count > 0)
                            {

                                foreach (DSImmunization.TherapeuticInjectionRow row in dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows)
                                {
                                    TherapeuticInjectionModel therapeuticInjectionModel = new TherapeuticInjectionModel();
                                    therapeuticInjectionModel.OSImmTherInjectionId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.OSImmTherInjectionIdColumn]);
                                    therapeuticInjectionModel.TherapeuticInjectionId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.TherapeuticInjectionIdColumn]);
                                    therapeuticInjectionModel.VisitDate = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.TherapeuticInjection.VisitDateColumn]).ToShortDateString());
                                    therapeuticInjectionModel.ProviderId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.ProviderIdColumn]);
                                    therapeuticInjectionModel.AdministrationDate = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.TherapeuticInjection.AdministrationDateColumn]).ToShortDateString());
                                    therapeuticInjectionModel.AdministrationTime = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.TherapeuticInjection.AdministrationDateColumn]).ToShortTimeString());
                                    therapeuticInjectionModel.Dose = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.DoseColumn]);
                                    therapeuticInjectionModel.Amount = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.AmountColumn]);
                                    therapeuticInjectionModel.ManufacturerId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.ManufacturerIdColumn]);
                                    therapeuticInjectionModel.ManufacturerName = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.ManufacturerNameColumn]);
                                    therapeuticInjectionModel.SiteId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.SiteIdColumn]);
                                    therapeuticInjectionModel.RouteId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.RouteIdColumn]);
                                    therapeuticInjectionModel.Comments = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.CommentsColumn]);
                                    if (row[dsImmunization.TherapeuticInjection.ExpiryDateColumn] != DBNull.Value)
                                    {
                                        therapeuticInjectionModel.ExpiryDate = MDVUtility.ToStr(MDVUtility.ToDateTime(row[dsImmunization.TherapeuticInjection.ExpiryDateColumn]).ToShortDateString());
                                    }
                                    else
                                    {
                                        therapeuticInjectionModel.ExpiryDate = "";
                                    }

                                    therapeuticInjectionModel.VFC = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.VFCColumn]);
                                    therapeuticInjectionModel.ReactionId = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.ReactionIdColumn]);
                                    therapeuticInjectionModel.Type = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.TypeColumn]);
                                    therapeuticInjectionModel.CPTCode = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.CPTCodeColumn]);
                                    therapeuticInjectionModel.TherapeuticInjection = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.TherapeuticInjectionColumn]);
                                    therapeuticInjectionModel.LotNumber = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.LotNumberColumn]);
                                    therapeuticInjectionModel.LotText = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.LotTextColumn]);
                                    therapeuticInjectionModel.LinkedWithAnyNote = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.LinkedWithAnyNoteColumn]);
                                    therapeuticInjectionModel.GivenBy = MDVUtility.ToStr(row[dsImmunization.TherapeuticInjection.GivenByColumn]);
                                    TherapeuticInjectionList.Add(therapeuticInjectionModel);
                                }
                                var response = new
                                {
                                    status = true,
                                    TherapeuticInjectionCount = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count,
                                    TherapeuticInjectionLoad_JSON = js.Serialize(TherapeuticInjectionList),
                                    iTotalDisplayRecords = (dsImmunization.TherapeuticInjection.Rows.Count > 0) ? dsImmunization.TherapeuticInjection.Rows[0][dsImmunization.TherapeuticInjection.RecordCountColumn.ColumnName] : 0,
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    TherapeuticInjectionCount = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count,
                                    TherapeuticInjectionLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName]),
                                    iTotalDisplayRecords = (dsImmunization.TherapeuticInjection.Rows.Count > 0) ? dsImmunization.TherapeuticInjection.Rows[0][dsImmunization.TherapeuticInjection.RecordCountColumn.ColumnName] : 0,
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                TherapeuticInjectionCount = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count,
                                TherapeuticInjectionLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName]),
                                iTotalDisplayRecords = (dsImmunization.TherapeuticInjection.Rows.Count > 0) ? dsImmunization.TherapeuticInjection.Rows[0][dsImmunization.TherapeuticInjection.RecordCountColumn.ColumnName] : 0,
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            TherapeuticInjectionCount = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count,
                            TherapeuticInjectionLoad_JSON = MDVUtility.JSON_DataTable(dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName]),
                            iTotalDisplayRecords = (dsImmunization.TherapeuticInjection.Rows.Count > 0) ? dsImmunization.TherapeuticInjection.Rows[0][dsImmunization.TherapeuticInjection.RecordCountColumn.ColumnName] : 0,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }

                else
                {
                    var response = new
                    {
                        status = true,
                        TherapeuticInjectionCount = 0,
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

        public string Update_Therapeutic_Injection(TherapeuticInjectionModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.OSImmTherInjectionId) > 0)
                {

                    DSImmunization dsImmunization = new DSImmunization();
                    BLObject<DSImmunization> obj = BLLOrderSetObj.OS_LoadImmunizationTherapeuticInjection(MDVUtility.ToInt64(model.OSImmTherInjectionId), MDVUtility.ToInt64(model.OrderSetId),0, 1, 1);
                    dsImmunization = obj.Data;
                    DSImmunization.TherapeuticInjectionRow dr1 = null;
                    foreach (DSImmunization.TherapeuticInjectionRow dr in dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows)
                    {
                        dr.TherapeuticInjectionId = MDVUtility.ToInt64(model.TherapeuticInjectionId);
                        dr[dsImmunization.TherapeuticInjection.VisitDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.ProviderId))
                            dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                        else
                            dr[dsImmunization.TherapeuticInjection.ProviderIdColumn] = DBNull.Value;
                        dr[dsImmunization.TherapeuticInjection.AdministrationDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.Dose))
                        {
                            dr.Dose = MDVUtility.ToDouble(model.Dose);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.DoseColumn] = DBNull.Value;
                        }
                        dr.Amount = model.Amount;

                        if (!string.IsNullOrEmpty(model.ManufacturerId))
                        {
                            dr.ManufacturerId = MDVUtility.ToInt64(model.ManufacturerId);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.ManufacturerIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.RouteId))
                        {
                            dr.RouteId = MDVUtility.ToInt64(model.RouteId);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.RouteIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.SiteId))
                        {
                            dr.SiteId = MDVUtility.ToInt64(model.SiteId);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.SiteIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.ExpiryDate))
                            dr.ExpiryDate = MDVUtility.ToDateTime(model.ExpiryDate);
                        else
                            dr[dsImmunization.TherapeuticInjection.ExpiryDateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.VFC))
                        {
                            dr.VFC = MDVUtility.ToInt64(model.VFC);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.VFCColumn] = DBNull.Value;
                        }



                        if (!string.IsNullOrEmpty(model.ReactionId))
                        {
                            dr.ReactionId = MDVUtility.ToInt16(model.ReactionId);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.ReactionIdColumn] = DBNull.Value;
                        }


                        if (!string.IsNullOrEmpty(model.Comments))
                        {
                            dr.Comments = model.Comments;
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.CommentsColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.SourceOfHx))
                        {
                            dr.SourceOfHx = MDVUtility.ToInt64(model.SourceOfHx);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.SourceOfHxColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.LotNumber))
                        {
                            dr.LotNumber = MDVUtility.ToInt64(model.LotNumber);
                        }
                        else
                        {
                            dr[dsImmunization.TherapeuticInjection.LotNumberColumn] = DBNull.Value;
                        }

                        dr.Type = model.Type;
                        //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields
                        dr.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserLastName) + ", " + MDVUtility.ToStr(MDVSession.Current.AppUserFirstName);
                        dr1 = dr;
                        //end newly added
                    }



                    #region Database Updation
                    if (dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows.Count > 0)
                    {
                        BLObject<DSImmunization> objUpdate = BLLOrderSetObj.Os_UpdateTherapeuticInjection(dsImmunization);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                TherapeuticInjectionIdColumn = dsImmunization.Tables[dsImmunization.TherapeuticInjection.TableName].Rows[0][dsImmunization.TherapeuticInjection.ImmTherInjectionIdColumn.ColumnName],
                                message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Therapeutic Injection not found."
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
        public string DeleteOsTherapeutichx(TherapeuticInjectionModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.OSImmTherInjectionId)))
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

                    BLObject<string> obj = BLLOrderSetObj.DeleteOsTherapeutichx(MDVUtility.ToStr(model.OSImmTherInjectionId));
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
    }
}