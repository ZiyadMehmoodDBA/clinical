using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_SupperBillDetail
    {
        private BLLBilling BLLBillingObj = null;
        public Admin_SupperBillDetail()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton

        private static Admin_SupperBillDetail _instance = null;
        public static Admin_SupperBillDetail Instance()
        {
            if (_instance == null)
                _instance = new Admin_SupperBillDetail();
            return _instance;
        }

        #endregion

        #region Data Members
        #endregion

        #region Private Functions

        private string GetSupperBill(Int64 BillID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    BLObject<DSSupperBill> objBill;
                    DSSupperBill dsBill = null;

                    objBill = BLLBillingObj.LoadSupperBillGrid(BillID);
                    dsBill = objBill.Data;
                    if (objBill.Data != null)
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string Bill = string.Empty;
                        if (dsBill.SuperBills.Rows.Count > 0)
                        {
                            DSSupperBill.SuperBillsRow dr = (DSSupperBill.SuperBillsRow)dsBill.SuperBills.Rows[0];
                            var keyValues = new Dictionary<string, string>
                            {
                                { "txtShortName", MDVUtility.ToStr(dr.ShortName)},
                                { "txtDescription", MDVUtility.ToStr(dr.Description)},
                                { "ddlPractice", MDVUtility.ToStr(dr.PracticeId)},
                                { "txtddlPractice", MDVUtility.ToStr(dr.PracticeName)},
                                { "chkActive", MDVUtility.ToStr(dr.IsActive)},
                                { "hfbillId", MDVUtility.ToStr(dr.SuperBillId)},
                            };
                            Bill = js.Serialize(keyValues);
                        }



                        var response = new
                        {
                            status = true,
                            BillCount = dsBill.Tables[dsBill.SuperBills.TableName].Rows.Count,
                            BillFill_JSON = Bill,
                            Title_JSON = MDVUtility.JSON_DataTable(dsBill.Tables[dsBill.SupBillTitle.TableName]),
                            ICD_JSON = MDVUtility.JSON_DataTable(dsBill.Tables[dsBill.SupBillICD.TableName]),
                            CPT_JSON = MDVUtility.JSON_DataTable(dsBill.Tables[dsBill.SupBillCPT.TableName]),
                            Modifier_JSON = MDVUtility.JSON_DataTable(dsBill.Tables[dsBill.SupBillModifier.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objBill.Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string UpdateSupperBill(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    BLObject<DSSupperBill> objBill;
                    DSSupperBill dsBill = new DSSupperBill();
                    objBill = BLLBillingObj.LoadSupperBill(MDVUtility.ToLong(SearchedfieldsJSON["hfbillId"]), 0, 0);

                    if (objBill.Data != null)
                    {
                        foreach (DSSupperBill.SuperBillsRow dr in objBill.Data.Tables[dsBill.SuperBills.TableName].Rows)
                        {
                            dr.ShortName = SearchedfieldsJSON["txtShortName"];
                            dr.Description = SearchedfieldsJSON["txtDescription"];
                            dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                            dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPractice"]);
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }

                        dsBill = objBill.Data;

                        #region Database Update

                        BLObject<DSSupperBill> obj = BLLBillingObj.UpdateSupperBill(ref dsBill);

                        if (dsBill.Tables[dsBill.SuperBills.TableName].Rows.Count > 0)
                        {
                            if (obj.Data != null)
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
                                    Message = obj.Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = ""
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
                            Message = privilegesMessage
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = ""
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

        private string SaveSupperBill(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSSupperBill dsBill = new DSSupperBill();
                    DSSupperBill.SuperBillsRow dr = dsBill.SuperBills.NewSuperBillsRow();

                    dr.ShortName = SearchedfieldsJSON["txtShortName"];
                    dr.Description = SearchedfieldsJSON["txtDescription"];
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPractice"]);
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsBill.SuperBills.AddSuperBillsRow(dr);
                    BLObject<DSSupperBill> obj = BLLBillingObj.InsertSupperBill(ref dsBill);
                    dsBill = obj.Data;

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            BillId = dsBill.Tables[dsBill.SuperBills.TableName].Rows[0][dsBill.SuperBills.SuperBillIdColumn.ColumnName]
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string SaveSupperBillTitle(string BillId, string Title, string TitleType)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSSupperBill dsBill = new DSSupperBill();
                    DSSupperBill.SupBillTitleRow dr = dsBill.SupBillTitle.NewSupBillTitleRow();

                    dr.SuperBillId = MDVUtility.ToLong(BillId);
                    dr.Description = Title;
                    dr.IsActive = true;
                    dr.TitleType = TitleType;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsBill.SupBillTitle.AddSupBillTitleRow(dr);
                    BLObject<DSSupperBill> obj = BLLBillingObj.InsertSupperBillTitle(ref dsBill);
                    dsBill = obj.Data;

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            TitleId = dsBill.Tables[dsBill.SupBillTitle.TableName].Rows[0][dsBill.SupBillTitle.SBTitleIdColumn.ColumnName]
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string UpdateSupperBillTitle(string BillId, string Title, string TitleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    BLObject<DSSupperBill> objBill;
                    DSSupperBill dsBill = new DSSupperBill();
                    objBill = BLLBillingObj.LoadSupperBillTitle(MDVUtility.ToLong(TitleId), 0);

                    if (objBill.Data != null)
                    {
                        if (objBill.Data.Tables[dsBill.SupBillTitle.TableName].Rows.Count > 0)
                        {
                            DSSupperBill.SupBillTitleRow dr = (DSSupperBill.SupBillTitleRow)objBill.Data.Tables[dsBill.SupBillTitle.TableName].Rows[0];
                            dr.Description = Title;
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;

                            dsBill = objBill.Data;

                            #region Database Updation

                            if (dsBill.Tables[dsBill.SupBillTitle.TableName].Rows.Count > 0)
                            {
                                BLObject<DSSupperBill> obj = BLLBillingObj.UpdateSupperBillTitle(ref dsBill);
                                if (obj.Data != null)
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
                                        Message = obj.Message
                                    };
                                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = ""
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
                                Message = "No record found."
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {

                        var response = new
                        {
                            status = false,
                            Message = objBill.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string SaveSupperBillICD(string TitleId, string Code, string Description, string SortId, string ICD10, string ICD10Description, string SNOMEDId, string SnomedDescription, string LexiCode)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSSupperBill dsBill = new DSSupperBill();
                    DSSupperBill.SupBillICDRow dr = dsBill.SupBillICD.NewSupBillICDRow();

                    dr.SBTitleId = MDVUtility.ToLong(TitleId);
                    dr.Description = Description;
                    dr.IsActive = true;
                    dr.SortId = Convert.ToInt32(SortId) + 1;
                    dr.ICDCode = Code;
                    dr.ICD10 = ICD10;
                    dr.ICD10Description = ICD10Description;
                    dr.SNOMEDId = SNOMEDId;
                    dr.SNOMEDDescription = SnomedDescription;
                    dr.LexiCode = LexiCode;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsBill.SupBillICD.AddSupBillICDRow(dr);
                    BLObject<DSSupperBill> obj = BLLBillingObj.InsertSupperBillICD(ref dsBill);
                    dsBill = obj.Data;

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            CodeId = dsBill.Tables[dsBill.SupBillICD.TableName].Rows[0][dsBill.SupBillICD.SBICDIdColumn.ColumnName],
                            SortId = dsBill.Tables[dsBill.SupBillICD.TableName].Rows[0][dsBill.SupBillICD.SortIdColumn.ColumnName],
                            TitleId = dsBill.Tables[dsBill.SupBillICD.TableName].Rows[0][dsBill.SupBillICD.SBTitleIdColumn.ColumnName],
                        };

                        #region Updation sort id of onwards records
                        //update sort id of onwards records.
                        BLObject<DSSupperBill> objBill;
                        long Title_Id = MDVUtility.ToLong(dsBill.Tables[dsBill.SupBillICD.TableName].Rows[0][dsBill.SupBillICD.SBTitleIdColumn.ColumnName]);
                        long Sort_Id = MDVUtility.ToLong(dsBill.Tables[dsBill.SupBillICD.TableName].Rows[0][dsBill.SupBillICD.SortIdColumn.ColumnName]);
                        long Code_Id = MDVUtility.ToLong(dsBill.Tables[dsBill.SupBillICD.TableName].Rows[0][dsBill.SupBillICD.SBICDIdColumn.ColumnName]);
                        objBill = BLLBillingObj.LoadSupperBillICD(0, 0, MDVUtility.ToLong(Title_Id));
                        DSSupperBill dsBill_ = new DSSupperBill();

                        if (objBill.Data != null)
                        {
                            foreach (DSSupperBill.SupBillICDRow drr in objBill.Data.Tables[dsBill_.SupBillICD.TableName].Rows)
                            {
                                if (drr.SortId >= Sort_Id && Code_Id != drr.SBICDId)
                                {
                                    drr.SortId = ++drr.SortId;
                                }
                            }

                            dsBill_ = objBill.Data;

                            #region Database Updation

                            if (dsBill_.Tables[dsBill_.SupBillICD.TableName].Rows.Count > 0)
                            {
                                BLLBillingObj.UpdateSupperBillICD(ref dsBill_);
                            }
                            #endregion
                        }
                        #endregion

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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string UpdateSupperBillICD(string Code, string Description, string CodeID, string ICD10, string ICD10Description, string SNOMEDId, string SnomedDescription, string LexiCode)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    BLObject<DSSupperBill> objBill;
                    DSSupperBill dsBill = new DSSupperBill();
                    objBill = BLLBillingObj.LoadSupperBillICD(MDVUtility.ToLong(CodeID), 0);

                    if (objBill.Data != null)
                    {
                        if (objBill.Data.Tables[dsBill.SupBillICD.TableName].Rows.Count > 0)
                        {
                            DSSupperBill.SupBillICDRow dr = (DSSupperBill.SupBillICDRow)objBill.Data.Tables[dsBill.SupBillICD.TableName].Rows[0];
                            dr.Description = Description;
                            dr.ICDCode = Code;
                            dr.ICD10 = ICD10;
                            dr.ICD10Description = ICD10Description;
                            dr.SNOMEDId = SNOMEDId;
                            dr.SNOMEDDescription = SnomedDescription;
                            dr.LexiCode = LexiCode;

                            dsBill = objBill.Data;

                            #region Database Updation

                            if (dsBill.Tables[dsBill.SupBillICD.TableName].Rows.Count > 0)
                            {
                                BLObject<DSSupperBill> obj = BLLBillingObj.UpdateSupperBillICD(ref dsBill);
                                if (obj.Data != null)
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
                                        Message = obj.Message
                                    };
                                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = ""
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
                                Message = "no data found."
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {

                        var response = new
                        {
                            status = false,
                            Message = objBill.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string DeleteCode(string Id, string Action)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(Id)))
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
                    BLObject<string> obj = null;
                    switch (Action)
                    {
                        case "ICD":
                            {
                                obj = BLLBillingObj.DeleteSupperBillICD(MDVUtility.ToLong(Id));
                            }
                            break;
                        case "CPT":
                            {
                                obj = BLLBillingObj.DeleteSupperBillCPT(MDVUtility.ToLong(Id));
                            }
                            break;
                        case "Modifier":
                            {
                                obj = BLLBillingObj.DeleteSupperBillModifier(MDVUtility.ToLong(Id));
                            }
                            break;
                    }

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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string DeleteTitle(string Id)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(Id)))
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
                        BLObject<string> obj = BLLBillingObj.DeleteSupperBillTitle(MDVUtility.ToLong(Id));
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string SortICD(string fromId, string toId)
        {
            string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "EDIT")).ToString();
            if (string.IsNullOrEmpty(privilegesMessage))
            {
                BLObject<DSSupperBill> objBillFrom;
                BLObject<DSSupperBill> objBillTo;

                DSSupperBill dsBillFrom = new DSSupperBill();
                DSSupperBill dsBillTo = new DSSupperBill();

                objBillFrom = BLLBillingObj.LoadSupperBillICD(MDVUtility.ToLong(fromId), 0);
                objBillTo = BLLBillingObj.LoadSupperBillICD(MDVUtility.ToLong(toId), 0);

                if (objBillTo.Data != null && objBillFrom.Data != null)
                {
                    int from_sortId = 0;
                    int to_sortId = 0;
                    foreach (DSSupperBill.SupBillICDRow dr in objBillFrom.Data.Tables[dsBillFrom.SupBillICD.TableName].Rows)
                    {
                        from_sortId = dr.SortId;
                    }
                    foreach (DSSupperBill.SupBillICDRow dr in objBillTo.Data.Tables[dsBillTo.SupBillICD.TableName].Rows)
                    {
                        to_sortId = dr.SortId;
                        dr.SortId = from_sortId;
                    }
                    foreach (DSSupperBill.SupBillICDRow dr in objBillFrom.Data.Tables[dsBillFrom.SupBillICD.TableName].Rows)
                    {
                        dr.SortId = to_sortId;
                    }

                    dsBillTo = objBillTo.Data;
                    dsBillFrom = objBillFrom.Data;

                    #region Database Updation

                    BLObject<DSSupperBill> objto = BLLBillingObj.UpdateSupperBillICD(ref dsBillTo);
                    BLObject<DSSupperBill> objfrom = BLLBillingObj.UpdateSupperBillICD(ref dsBillFrom);

                    if (objto.Data != null && objfrom != null)
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
                            Message = ""
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
                        Message = ""
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = privilegesMessage
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        private string SortCPT(string fromId, string toId)
        {
            string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "EDIT")).ToString();
            if (string.IsNullOrEmpty(privilegesMessage))
            {
                BLObject<DSSupperBill> objBillFrom;
                BLObject<DSSupperBill> objBillTo;

                DSSupperBill dsBillFrom = new DSSupperBill();
                DSSupperBill dsBillTo = new DSSupperBill();

                objBillFrom = BLLBillingObj.LoadSupperBillCPT(MDVUtility.ToLong(fromId), 0);
                objBillTo = BLLBillingObj.LoadSupperBillCPT(MDVUtility.ToLong(toId), 0);

                if (objBillTo.Data != null && objBillFrom.Data != null)
                {
                    int from_sortId = 0;
                    int to_sortId = 0;
                    foreach (DSSupperBill.SupBillCPTRow dr in objBillFrom.Data.Tables[dsBillFrom.SupBillCPT.TableName].Rows)
                    {
                        from_sortId = dr.SortId;
                    }
                    foreach (DSSupperBill.SupBillCPTRow dr in objBillTo.Data.Tables[dsBillTo.SupBillCPT.TableName].Rows)
                    {
                        to_sortId = dr.SortId;
                        dr.SortId = from_sortId;
                    }
                    foreach (DSSupperBill.SupBillCPTRow dr in objBillFrom.Data.Tables[dsBillFrom.SupBillCPT.TableName].Rows)
                    {
                        dr.SortId = to_sortId;
                    }

                    dsBillTo = objBillTo.Data;
                    dsBillFrom = objBillFrom.Data;

                    #region Database Updation

                    BLObject<DSSupperBill> objto = BLLBillingObj.UpdateSupperBillCPT(ref dsBillTo);
                    BLObject<DSSupperBill> objfrom = BLLBillingObj.UpdateSupperBillCPT(ref dsBillFrom);

                    if (objto.Data != null && objfrom != null)
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
                            Message = ""
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
                        Message = ""
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = privilegesMessage
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        private string SortModifier(string fromId, string toId)
        {
            string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "EDIT")).ToString();
            if (string.IsNullOrEmpty(privilegesMessage))
            {
                BLObject<DSSupperBill> objBillFrom;
                BLObject<DSSupperBill> objBillTo;

                DSSupperBill dsBillFrom = new DSSupperBill();
                DSSupperBill dsBillTo = new DSSupperBill();

                objBillFrom = BLLBillingObj.LoadSupperBillModifier(MDVUtility.ToLong(fromId), 0);
                objBillTo = BLLBillingObj.LoadSupperBillModifier(MDVUtility.ToLong(toId), 0);

                if (objBillTo.Data != null && objBillFrom.Data != null)
                {
                    int from_sortId = 0;
                    int to_sortId = 0;
                    foreach (DSSupperBill.SupBillModifierRow dr in objBillFrom.Data.Tables[dsBillFrom.SupBillModifier.TableName].Rows)
                    {
                        from_sortId = dr.SortId;
                    }
                    foreach (DSSupperBill.SupBillModifierRow dr in objBillTo.Data.Tables[dsBillTo.SupBillModifier.TableName].Rows)
                    {
                        to_sortId = dr.SortId;
                        dr.SortId = from_sortId;
                    }
                    foreach (DSSupperBill.SupBillModifierRow dr in objBillFrom.Data.Tables[dsBillFrom.SupBillModifier.TableName].Rows)
                    {
                        dr.SortId = to_sortId;
                    }

                    dsBillTo = objBillTo.Data;
                    dsBillFrom = objBillFrom.Data;

                    #region Database Updation

                    BLObject<DSSupperBill> objto = BLLBillingObj.UpdateSupperBillModifier(ref dsBillTo);
                    BLObject<DSSupperBill> objfrom = BLLBillingObj.UpdateSupperBillModifier(ref dsBillFrom);

                    if (objto.Data != null && objfrom != null)
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
                            Message = ""
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
                        Message = ""
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = privilegesMessage
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        private string SaveSupperBillCPT(string TitleId, string Code, string Description, string SortId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSSupperBill dsBill = new DSSupperBill();
                    DSSupperBill.SupBillCPTRow dr = dsBill.SupBillCPT.NewSupBillCPTRow();

                    dr.SBTitleId = MDVUtility.ToLong(TitleId);
                    dr.Description = Description;
                    dr.IsActive = true;
                    dr.SortId = Convert.ToInt32(SortId) + 1;
                    dr.CPTCode = Code;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsBill.SupBillCPT.AddSupBillCPTRow(dr);
                    BLObject<DSSupperBill> obj = BLLBillingObj.InsertSupperBillCPT(ref dsBill);
                    dsBill = obj.Data;

                    if (obj.Data != null)
                    {


                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            CodeId = dsBill.Tables[dsBill.SupBillCPT.TableName].Rows[0][dsBill.SupBillCPT.SBCPTIdColumn.ColumnName],
                            SortId = dsBill.Tables[dsBill.SupBillCPT.TableName].Rows[0][dsBill.SupBillCPT.SortIdColumn.ColumnName],
                            TitleId = dsBill.Tables[dsBill.SupBillCPT.TableName].Rows[0][dsBill.SupBillCPT.SBTitleIdColumn.ColumnName],
                        };

                        #region Updation sort id of onwards records
                        //update sort id of onwards records.
                        BLObject<DSSupperBill> objBill;
                        long Title_Id = MDVUtility.ToLong(dsBill.Tables[dsBill.SupBillCPT.TableName].Rows[0][dsBill.SupBillCPT.SBTitleIdColumn.ColumnName]);
                        long Sort_Id = MDVUtility.ToLong(dsBill.Tables[dsBill.SupBillCPT.TableName].Rows[0][dsBill.SupBillCPT.SortIdColumn.ColumnName]);
                        long Code_Id = MDVUtility.ToLong(dsBill.Tables[dsBill.SupBillCPT.TableName].Rows[0][dsBill.SupBillCPT.SBCPTIdColumn.ColumnName]);
                        objBill = BLLBillingObj.LoadSupperBillCPT(0, 0, MDVUtility.ToLong(Title_Id));
                        DSSupperBill dsBill_ = new DSSupperBill();

                        if (objBill.Data != null)
                        {
                            foreach (DSSupperBill.SupBillCPTRow drr in objBill.Data.Tables[dsBill_.SupBillCPT.TableName].Rows)
                            {
                                if (drr.SortId >= Sort_Id && Code_Id != drr.SBCPTId)
                                {
                                    drr.SortId = ++drr.SortId;
                                }
                            }

                            dsBill_ = objBill.Data;

                            #region Database Updation

                            if (dsBill_.Tables[dsBill_.SupBillCPT.TableName].Rows.Count > 0)
                            {
                                BLLBillingObj.UpdateSupperBillCPT(ref dsBill_);
                            }
                            #endregion
                        }
                        #endregion

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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string SaveSupperBillModifier(string TitleId, string Code, string Description, string SortId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSSupperBill dsBill = new DSSupperBill();
                    DSSupperBill.SupBillModifierRow dr = dsBill.SupBillModifier.NewSupBillModifierRow();

                    dr.SBTitleId = MDVUtility.ToLong(TitleId);
                    dr.Description = Description;
                    dr.IsActive = true;
                    dr.SortId = Convert.ToInt32(SortId) + 1;
                    dr.ModifierCode = Code;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsBill.SupBillModifier.AddSupBillModifierRow(dr);
                    BLObject<DSSupperBill> obj = BLLBillingObj.InsertSupperBillModifier(ref dsBill);
                    dsBill = obj.Data;

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            CodeId = dsBill.Tables[dsBill.SupBillModifier.TableName].Rows[0][dsBill.SupBillModifier.SBModifierIdColumn.ColumnName],
                            SortId = dsBill.Tables[dsBill.SupBillModifier.TableName].Rows[0][dsBill.SupBillModifier.SortIdColumn.ColumnName],
                            TitleId = dsBill.Tables[dsBill.SupBillModifier.TableName].Rows[0][dsBill.SupBillModifier.SBTitleIdColumn.ColumnName],
                        };

                        #region Updation sort id of onwards records
                        //update sort id of onwards records.
                        BLObject<DSSupperBill> objBill;
                        long Title_Id = MDVUtility.ToLong(dsBill.Tables[dsBill.SupBillModifier.TableName].Rows[0][dsBill.SupBillModifier.SBTitleIdColumn.ColumnName]);
                        long Sort_Id = MDVUtility.ToLong(dsBill.Tables[dsBill.SupBillModifier.TableName].Rows[0][dsBill.SupBillModifier.SortIdColumn.ColumnName]);
                        long Code_Id = MDVUtility.ToLong(dsBill.Tables[dsBill.SupBillModifier.TableName].Rows[0][dsBill.SupBillModifier.SBModifierIdColumn.ColumnName]);
                        objBill = BLLBillingObj.LoadSupperBillModifier(0, 0, MDVUtility.ToLong(Title_Id));
                        DSSupperBill dsBill_ = new DSSupperBill();

                        if (objBill.Data != null)
                        {
                            foreach (DSSupperBill.SupBillModifierRow drr in objBill.Data.Tables[dsBill_.SupBillModifier.TableName].Rows)
                            {
                                if (drr.SortId >= Sort_Id && Code_Id != drr.SBModifierId)
                                {
                                    drr.SortId = ++drr.SortId;
                                }
                            }

                            dsBill_ = objBill.Data;

                            #region Database Updation

                            if (dsBill_.Tables[dsBill_.SupBillModifier.TableName].Rows.Count > 0)
                            {
                                BLLBillingObj.UpdateSupperBillModifier(ref dsBill_);
                            }
                            #endregion
                        }
                        #endregion

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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string UpdateSupperBillModifier(string Code, string Description, string CodeID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    BLObject<DSSupperBill> objBill;
                    DSSupperBill dsBill = new DSSupperBill();
                    objBill = BLLBillingObj.LoadSupperBillModifier(MDVUtility.ToLong(CodeID), 0);

                    if (objBill.Data != null)
                    {
                        foreach (DSSupperBill.SupBillModifierRow dr in objBill.Data.Tables[dsBill.SupBillModifier.TableName].Rows)
                        {
                            dr.Description = Description;
                            dr.ModifierCode = Code;
                        }

                        dsBill = objBill.Data;

                        #region Database Updation

                        if (dsBill.Tables[dsBill.SupBillModifier.TableName].Rows.Count > 0)
                        {
                            BLObject<DSSupperBill> obj = BLLBillingObj.UpdateSupperBillModifier(ref dsBill);
                            if (obj.Data != null)
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
                                    Message = obj.Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = ""
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
                            Message = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string UpdateSupperBillCPT(string Code, string Description, string CodeID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Super Bill", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    BLObject<DSSupperBill> objBill;
                    DSSupperBill dsBill = new DSSupperBill();
                    objBill = BLLBillingObj.LoadSupperBillCPT(MDVUtility.ToLong(CodeID), 0);

                    if (objBill.Data != null)
                    {
                        foreach (DSSupperBill.SupBillCPTRow dr in objBill.Data.Tables[dsBill.SupBillCPT.TableName].Rows)
                        {
                            dr.Description = Description;
                            dr.CPTCode = Code;
                        }

                        dsBill = objBill.Data;

                        #region Database Updation

                        if (dsBill.Tables[dsBill.SupBillCPT.TableName].Rows.Count > 0)
                        {
                            BLObject<DSSupperBill> obj = BLLBillingObj.UpdateSupperBillCPT(ref dsBill);
                            if (obj.Data != null)
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
                                    Message = obj.Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = ""
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
                            Message = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Public Functions

        #endregion

        #region Control Events
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the User Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "FILL_SUPPERBILL":
                    {

                        string strBillID = context.Request["BillID"];
                        string strJSONData = GetSupperBill(MDVUtility.ToInt64(strBillID));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_SUPPERBILL":
                    {
                        string fieldsJSON = context.Request["SupperBillData"];
                        string strJSONData = SaveSupperBill(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SUPPERBILL":
                    {
                        string fieldsJSON = context.Request["SupperBillData"];
                        string strJSONData = UpdateSupperBill(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_SUPPERBILL_TITLE":
                    {
                        string BillId = context.Request["BillID"];
                        string Title = context.Request["Title"];
                        string TitleType = context.Request["action"];
                        string strJSONData = SaveSupperBillTitle(BillId, Title, TitleType);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SUPPERBILL_TITLE":
                    {
                        string BillId = context.Request["BillID"];
                        string Title = context.Request["Title"];
                        string TitleId = context.Request["TitleId"];
                        string strJSONData = UpdateSupperBillTitle(BillId, Title, TitleId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_SUPPERBILL_CODE":
                    {
                        string Code = context.Request["Code"];
                        string TitleId = context.Request["TitleId"];
                        string Description = context.Request["Description"];
                        string SortId = context.Request["SortId"];
                        string Action = context.Request["action"];
                        switch (Action)
                        {
                            case "ICD":
                                {
                                    string ICD10 = context.Request["ICD10"];
                                    string ICD10Description = context.Request["ICD10Description"];
                                    string SNOMEDId = context.Request["SNOMEDId"];
                                    string SnomedDescription = context.Request["SnomedDescription"];
                                    string LexiCode = context.Request["LexiCode"];

                                    string strJSONData = SaveSupperBillICD(TitleId, Code, Description, SortId, ICD10, ICD10Description, SNOMEDId, SnomedDescription, LexiCode);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write(strJSONData);
                                }
                                break;
                            case "CPT":
                                {
                                    string strJSONData = SaveSupperBillCPT(TitleId, Code, Description, SortId);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write(strJSONData);
                                }
                                break;
                            case "Modifier":
                                {
                                    string strJSONData = SaveSupperBillModifier(TitleId, Code, Description, SortId);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write(strJSONData);
                                }
                                break;

                        }
                    }
                    break;
                case "UPDATE_SUPPERBILL_CODE":
                    {
                        string Code = context.Request["Code"];
                        string Description = context.Request["Description"];
                        string CodeID = context.Request["CodeID"];
                        string Action = context.Request["action"];

                        switch (Action)
                        {
                            case "ICD":
                                {
                                    string ICD10 = context.Request["ICD10"];
                                    string ICD10Description = context.Request["ICD10Description"];
                                    string SNOMEDId = context.Request["SNOMEDId"];
                                    string SnomedDescription = context.Request["SnomedDescription"];
                                    string LexiCode = context.Request["LexiCode"];

                                    string strJSONData = UpdateSupperBillICD(Code, Description, CodeID, ICD10, ICD10Description, SNOMEDId, SnomedDescription, LexiCode);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write(strJSONData);
                                }
                                break;
                            case "CPT":
                                {
                                    string strJSONData = UpdateSupperBillCPT(Code, Description, CodeID);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write(strJSONData);
                                }
                                break;
                            case "Modifier":
                                {
                                    string strJSONData = UpdateSupperBillModifier(Code, Description, CodeID);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write(strJSONData);
                                }
                                break;

                        }
                    }
                    break;
                case "DELETE_SUPPERBILL_TITLE":
                    {
                        string Id = context.Request["TitleId"];
                        string strJSONData = DeleteTitle(Id);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_SUPPERBILL_CODE":
                    {
                        string Id = context.Request["CodeId"];
                        string Action = context.Request["action"];

                        string strJSONData = DeleteCode(Id, Action);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SORT_SUPPERBILL_CODE":
                    {

                        string fromId = context.Request["FromElementId"];
                        string toId = context.Request["ToElementId"];
                        string Action = context.Request["action"];

                        switch (Action)
                        {
                            case "ICD":
                                {
                                    string strJSONData = SortICD(fromId, toId);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write(strJSONData);
                                }
                                break;
                            case "CPT":
                                {
                                    string strJSONData = SortCPT(fromId, toId);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write(strJSONData);
                                }
                                break;
                            case "Modifier":
                                {
                                    string strJSONData = SortModifier(fromId, toId);
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write(strJSONData);
                                }
                                break;

                        }
                    }
                    break;

            }
        }


        #endregion


    }
}