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
using System.Text;
using System.Web;

namespace MDVision.IEHR.Controls.Admin.Statement
{
    public class Admin_StatementGroup
    {
        private BLLBilling BLLBillingObj = null;
        public Admin_StatementGroup()
        {
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton

        private static Admin_StatementGroup _instance = null;
        public static Admin_StatementGroup Instance()
        {
            if (_instance == null)
                _instance = new Admin_StatementGroup();
            return _instance;
        }

        #endregion

        #region Private Functions
        private string SaveStatementGroup(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Statement Group", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSPatientStatement dsPatientStatement = new DSPatientStatement();
                    DSPatientStatement.PatientStatementGroupRow dr = dsPatientStatement.PatientStatementGroup.NewPatientStatementGroupRow();



                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtName"]))
                        dr.Name = SearchedfieldsJSON["txtName"];

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtDescription"]))
                        dr.Description = SearchedfieldsJSON["txtDescription"];

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCycleDays"]))
                        dr.CycleDays = MDVUtility.ToInt32(SearchedfieldsJSON["txtCycleDays"]);

                    //  if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlLetters"]))
                    //  dr.LetterId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlLetters"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMessage1"]))
                    {
                        dr.FirstMsgId = MDVUtility.ToStr(SearchedfieldsJSON["ddlMessage1"]);
                    }
                    else
                    {
                        dr.FirstMsgId = null;
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMessage2"]))
                    {
                        dr.SecondMsgId = MDVUtility.ToStr(SearchedfieldsJSON["ddlMessage2"]);
                    }
                    else
                    {
                        dr.SecondMsgId = null;
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMessage3"]))
                    {
                        dr.ThirdMsgId = MDVUtility.ToStr(SearchedfieldsJSON["ddlMessage3"]);
                    }
                    else
                    {
                        dr.ThirdMsgId = null;
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMessage4"]))
                    {
                        dr.FourthMsgId = MDVUtility.ToStr(SearchedfieldsJSON["ddlMessage4"]);
                    }
                    else
                    {
                        dr.FourthMsgId = null;
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMessage5"]))
                    {
                        dr.FifthMsgId = MDVUtility.ToStr(SearchedfieldsJSON["ddlMessage5"]);
                    }
                    else
                    {
                        dr.FifthMsgId = null;
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOutstandingDays"]))
                        dr.OutStandingDays = MDVUtility.ToInt32(SearchedfieldsJSON["txtOutstandingDays"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtNumberOfStatements"]))
                        dr.NoOfStatements = MDVUtility.ToInt32(SearchedfieldsJSON["txtNumberOfStatements"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtNumberOfStatements"]))
                        dr.NoOfStatements = MDVUtility.ToInt32(SearchedfieldsJSON["txtNumberOfStatements"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["imgGroupImage"]))
                    {
                        string markup = "<table style='height: 382px;' width='845'><tbody><tr><td><img src='" + SearchedfieldsJSON["imgGroupImage"].Split(',')[0].Split(';')[0] + ";base64," + SearchedfieldsJSON["imgGroupImage"].Split(',')[1] + "' alt='' width='71' height='139' />&nbsp;</td></tr><tr><td><input id='6' class='FieldInserted_PK' style='min-width: 10px; margin: 0 5px; margin-right: 5px; border: none; padding: 0 0px; width: 140px;' readonly='readonly' type='text' value='{{ PatientStatement }}' /></td></tr></tbody></table> ";
                        //  string markup = "<img src='" + SearchedfieldsJSON["imgGroupImage"].Split(',')[0].Split(';')[0] + ";base64," + SearchedfieldsJSON["imgGroupImage"].Split(',')[1] + "' alt='' width='71' height='139' />";
                        //string strBase64 = SearchedfieldsJSON["imgGroupImage"].Split(',')[1];
                        //strBase64 = strBase64.Replace(' ', '+');
                        //byte[] currentFileStream = Convert.FromBase64String(strBase64);
                        dr.StatementImage = markup;
                        //dr.StatementImageType = SearchedfieldsJSON["imgPatient"].Split(',')[0].Split(';')[0];
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["uploadFilePH"]))
                    {
                        dr.ImageName = SearchedfieldsJSON["uploadFilePH"];
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtMessage"]))
                    {
                        dr.Message = SearchedfieldsJSON["txtMessage"];
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFromName"]))
                    {
                        dr.FromName = SearchedfieldsJSON["txtFromName"];
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFromAddress"]))
                    {
                        dr.FromAddress = SearchedfieldsJSON["txtFromAddress"];
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFromCity"]))
                    {
                        dr.FromCity = SearchedfieldsJSON["txtFromCity"];
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFromState"]))
                    {
                        dr.FromState = SearchedfieldsJSON["txtFromState"];
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFromZip"]))
                    {
                        dr.FromZip = MDVUtility.ToInt32(SearchedfieldsJSON["txtFromZip"]);
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFromZipExt"]))
                    {
                        dr.FromZipExt = MDVUtility.ToInt32(SearchedfieldsJSON["txtFromZipExt"]);
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRemitToName"]))
                    {
                        dr.RemitToName = SearchedfieldsJSON["txtRemitToName"];
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRemitToAddress"]))
                    {
                        dr.RemitToAddress = SearchedfieldsJSON["txtRemitToAddress"];
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRemitToCity"]))
                    {
                        dr.RemitToCity = SearchedfieldsJSON["txtRemitToCity"];
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRemitToState"]))
                    {
                        dr.RemitToState = SearchedfieldsJSON["txtRemitToState"];
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRemitZip"]))
                    {
                        dr.RemitToZip = MDVUtility.ToInt32(SearchedfieldsJSON["txtRemitZip"]);
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRemitToZipExt"]))
                    {
                        dr.RemitToZipExt = MDVUtility.ToInt32(SearchedfieldsJSON["txtRemitToZipExt"]);
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOfficeHoursFrom"]))
                    {
                        dr.OfcHoursFrom = SearchedfieldsJSON["txtOfficeHoursFrom"];
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOfficeHoursTo"]))
                    {
                        dr.OfcHoursTo = SearchedfieldsJSON["txtOfficeHoursTo"];
                    }

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtTelePhone"]))
                    {
                        dr.PhoneNo = SearchedfieldsJSON["txtTelePhone"];
                    }
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPhoneExt"]))
                    {
                        dr.PhoneExt = SearchedfieldsJSON["txtPhoneExt"];
                    }

                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsPatientStatement.PatientStatementGroup.AddPatientStatementGroupRow(dr);
                    BLObject<DSPatientStatement> obj = BLLBillingObj.InsertStatementGroup(dsPatientStatement);
                    dsPatientStatement = obj.Data;

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            StatementGroupId = dsPatientStatement.Tables[dsPatientStatement.PatientStatementGroup.TableName].Rows[0][dsPatientStatement.PatientStatementGroup.PtStmtGrpIdColumn.ColumnName]
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


        private string SearchStatementGroup(string fieldsJSON, Int64 statementGroupID, int PageNumber, int RowsPerPage)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Statement Group", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    DSPatientStatement dsPatientStatement = null;
                    BLObject<DSPatientStatement> obj;

                    string name = "";
                    Int32 cycleDays = 0;
                    string isActive = "";



                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtName"]))
                        name = MDVUtility.ToStr(SearchedfieldsJSON["txtName"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtCycleDays"]))
                        cycleDays = MDVUtility.ToInt32(SearchedfieldsJSON["txtCycleDays"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlActive"]))
                        isActive = MDVUtility.ToStr(SearchedfieldsJSON["ddlActive"]);

                    obj = BLLBillingObj.LoadStatementGroup(statementGroupID, name, cycleDays, isActive, PageNumber, RowsPerPage);


                    dsPatientStatement = obj.Data;
                    if (obj.Data != null)
                    {
                        if (dsPatientStatement.Tables[dsPatientStatement.PatientStatementGroup.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                StatementGroupCount = dsPatientStatement.Tables[dsPatientStatement.PatientStatementGroup.TableName].Rows.Count,
                                iTotalDisplayRecords = dsPatientStatement.PatientStatementGroup.Rows[0][dsPatientStatement.PatientStatementGroup.RecordCountColumn.ColumnName],
                                StatementGroupLoad_JSON = MDVUtility.JSON_DataTable(dsPatientStatement.Tables[dsPatientStatement.PatientStatementGroup.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                MessageCount = 0,
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

        private string UpdateStatementGroupActive(Int64 StatementGroupId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Statement Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSPatientStatement dsPatientStatement = null;
                    BLObject<DSPatientStatement> obj = BLLBillingObj.LoadStatementGroup(StatementGroupId, "", 0, "", 1, 15);
                    dsPatientStatement = obj.Data;
                    if (dsPatientStatement.Tables[dsPatientStatement.PatientStatementGroup.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPatientStatement.Tables[dsPatientStatement.PatientStatementGroup.TableName].Rows[0];
                        dr[dsPatientStatement.PatientStatementGroup.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSPatientStatement> objStatementGroup = BLLBillingObj.UpdateStatementGroup(dsPatientStatement);
                        string successMsg;
                        if (objStatementGroup.Data != null)
                        {
                            if (IsActive == 0)
                                successMsg = Common.AppPrivileges.Inactive_Message;
                            else
                                successMsg = Common.AppPrivileges.Active_Message;
                            var response = new
                            {
                                status = true,
                                message = successMsg
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objStatementGroup.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
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

        private string UpdateStatementGroup(string fieldsJSON)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Statement Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    ser.MaxJsonLength = Int32.MaxValue;
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    BLObject<DSPatientStatement> objStatementGroup;
                    DSPatientStatement dsPatientStatement = new DSPatientStatement();
                    objStatementGroup = BLLBillingObj.LoadStatementGroup(MDVUtility.ToInt64(SearchedfieldsJSON["hfStatementGroupId"]), "", 0, "", 1, 15);

                    if (objStatementGroup.Data != null)
                    {
                        foreach (DSPatientStatement.PatientStatementGroupRow dr in objStatementGroup.Data.Tables[dsPatientStatement.PatientStatementGroup.TableName].Rows)
                        {
                            dr.Name = SearchedfieldsJSON["txtName"];

                            dr.Description = SearchedfieldsJSON["txtDescription"];

                            dr.CycleDays = MDVUtility.ToInt32(SearchedfieldsJSON["txtCycleDays"]);


                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMessage1"]))
                            {
                                dr.FirstMsgId = MDVUtility.ToStr(SearchedfieldsJSON["ddlMessage1"]);
                            }
                            else
                            {
                                dr.FirstMsgId = null;
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMessage2"]))
                            {
                                dr.SecondMsgId = MDVUtility.ToStr(SearchedfieldsJSON["ddlMessage2"]);
                            }
                            else
                            {
                                dr.SecondMsgId = null;

                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMessage3"]))
                            {
                                dr.ThirdMsgId = MDVUtility.ToStr(SearchedfieldsJSON["ddlMessage3"]);
                            }
                            else
                            {
                                dr.ThirdMsgId = null;
                            }


                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMessage4"]))
                            {
                                dr.FourthMsgId = MDVUtility.ToStr(SearchedfieldsJSON["ddlMessage4"]);
                            }
                            else
                            {
                                dr.FourthMsgId = null;
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMessage5"]))
                            {
                                dr.FifthMsgId = MDVUtility.ToStr(SearchedfieldsJSON["ddlMessage5"]);

                            }
                            else
                            {
                                dr.FifthMsgId = null;
                            }

                            dr.OutStandingDays = MDVUtility.ToInt32(SearchedfieldsJSON["txtOutstandingDays"]);

                            dr.NoOfStatements = MDVUtility.ToInt32(SearchedfieldsJSON["txtNumberOfStatements"]);

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["imgGroupImage"]))
                            {
                                string markup = "<table style='height: 382px;' width='845'><tbody><tr><td><img src='" + SearchedfieldsJSON["imgGroupImage"].Split(',')[0].Split(';')[0] + ";base64," + SearchedfieldsJSON["imgGroupImage"].Split(',')[1] + "' alt='' height='65' />&nbsp;</td></tr><tr><td><input id='6' class='FieldInserted_PK' style='min-width: 10px; margin: 0 5px; margin-right: 5px; border: none; padding: 0 0px; width: 140px;' readonly='readonly' type='text' value='{{ PatientStatement }}' /></td></tr></tbody></table> ";
                                //string markup = "<img src='" + SearchedfieldsJSON["imgGroupImage"].Split(',')[0].Split(';')[0] + ";base64," + SearchedfieldsJSON["imgGroupImage"].Split(',')[1] + "' alt='' width='71' height='139' />";

                                //string strBase64 = SearchedfieldsJSON["imgGroupImage"].Split(',')[1];
                                //strBase64 = strBase64.Replace(' ', '+');
                                //byte[] currentFileStream = Convert.FromBase64String(strBase64);
                                dr.StatementImage = markup;
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["uploadFilePH"]))
                            {
                                dr.ImageName = SearchedfieldsJSON["uploadFilePH"];
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtMessage"]))
                            {
                                dr.Message = SearchedfieldsJSON["txtMessage"];
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFromName"]))
                            {
                                dr.FromName = SearchedfieldsJSON["txtFromName"];
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFromAddress"]))
                            {
                                dr.FromAddress = SearchedfieldsJSON["txtFromAddress"];
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFromCity"]))
                            {
                                dr.FromCity = SearchedfieldsJSON["txtFromCity"];
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFromState"]))
                            {
                                dr.FromState = SearchedfieldsJSON["txtFromState"];
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFromZip"]))
                            {
                                dr.FromZip = MDVUtility.ToInt32(SearchedfieldsJSON["txtFromZip"]);
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtFromZipExt"]))
                            {
                                dr.FromZipExt = MDVUtility.ToInt32(SearchedfieldsJSON["txtFromZipExt"]);
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRemitToName"]))
                            {
                                dr.RemitToName = SearchedfieldsJSON["txtRemitToName"];
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRemitToAddress"]))
                            {
                                dr.RemitToAddress = SearchedfieldsJSON["txtRemitToAddress"];
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRemitToCity"]))
                            {
                                dr.RemitToCity = SearchedfieldsJSON["txtRemitToCity"];
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRemitToState"]))
                            {
                                dr.RemitToState = SearchedfieldsJSON["txtRemitToState"];
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRemitZip"]))
                            {
                                dr.RemitToZip = MDVUtility.ToInt32(SearchedfieldsJSON["txtRemitZip"]);
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRemitToZipExt"]))
                            {
                                dr.RemitToZipExt = MDVUtility.ToInt32(SearchedfieldsJSON["txtRemitToZipExt"]);
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOfficeHoursFrom"]))
                            {
                                dr.OfcHoursFrom = SearchedfieldsJSON["txtOfficeHoursFrom"];
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtOfficeHoursTo"]))
                            {
                                dr.OfcHoursTo = SearchedfieldsJSON["txtOfficeHoursTo"];
                            }

                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtTelePhone"]))
                            {
                                dr.PhoneNo = SearchedfieldsJSON["txtTelePhone"];
                            }

                            /*Zia Mehmood*/
                            if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPhoneExt"]) && SearchedfieldsJSON["txtPhoneExt"] != "_____")
                            {
                                dr.PhoneExt = SearchedfieldsJSON["txtPhoneExt"];
                            }
                            else
                            {
                                dr.PhoneExt = null;
                            }


                            //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlLetters"]))
                            dr.LetterId = 0;

                            dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }

                        dsPatientStatement = objStatementGroup.Data;

                        #region Database Update

                        BLObject<DSPatientStatement> obj = BLLBillingObj.UpdateStatementGroup(dsPatientStatement);

                        if (dsPatientStatement.Tables[dsPatientStatement.PatientStatementGroup.TableName].Rows.Count > 0)
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

        private string DeleteStatementGroup(Int64 StatementGroupID)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Statement Group", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(StatementGroupID)))
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
                        BLObject<string> obj = BLLBillingObj.DeleteStatementGroup(MDVUtility.ToLong(StatementGroupID));
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


        private string FillStatementGroup(Int64 StatementGroupId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Statement Group", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(StatementGroupId)))
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        DSPatientStatement dsPatientStatement = null;
                        BLObject<DSPatientStatement> obj = BLLBillingObj.LoadStatementGroup(StatementGroupId, "", 0, "", 1, 15);
                        if (obj.Data != null)
                        {
                            dsPatientStatement = obj.Data;
                            if (dsPatientStatement.Tables[dsPatientStatement.PatientStatementGroup.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsPatientStatement.Tables[dsPatientStatement.PatientStatementGroup.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                          { "hfStatementGroupId", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.PtStmtGrpIdColumn.ColumnName])},
                          { "txtName", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.NameColumn.ColumnName])},
                          { "txtDescription", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.DescriptionColumn.ColumnName])},
                          { "txtCycleDays", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.CycleDaysColumn.ColumnName])},
                          { "ddlMessage1", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.FirstMsgIdColumn.ColumnName])},
                          { "ddlMessage2", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.SecondMsgIdColumn.ColumnName])},
                          { "ddlMessage3", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.ThirdMsgIdColumn.ColumnName])},
                          { "ddlMessage4", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.FourthMsgIdColumn.ColumnName])},
                          { "ddlMessage5", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.FifthMsgIdColumn.ColumnName])},
                          { "txtOutstandingDays", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.OutStandingDaysColumn.ColumnName])},
                          { "txtNumberOfStatements", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.NoOfStatementsColumn.ColumnName])},
                          { "chkActive", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.IsActiveColumn.ColumnName])},
                          { "uploadFilePH", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.ImageNameColumn.ColumnName])},
                         //  ,psg.RemitToZip,psg.RemitToZipExt,psg.OfcHoursFrom ,psg.OfcHoursTo ,psg.PhoneNo
                          { "txtMessage", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.MessageColumn.ColumnName])},
                          { "txtFromName", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.FromNameColumn.ColumnName])},
                          { "txtFromAddress", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.FromAddressColumn.ColumnName])},
                          { "txtFromCity", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.FromCityColumn.ColumnName])},
                          { "txtFromState", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.FromStateColumn.ColumnName])},
                          { "txtFromZip", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.FromZipColumn.ColumnName])},
                          { "txtFromZipExt", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.FromZipExtColumn.ColumnName])},
                          { "txtRemitToName", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.RemitToNameColumn.ColumnName])},
                          { "txtRemitToAddress", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.RemitToAddressColumn.ColumnName])},
                          { "txtRemitToCity", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.RemitToCityColumn.ColumnName])},
                          { "txtRemitToState", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.RemitToStateColumn.ColumnName])},
                          { "txtRemitZip", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.RemitToZipColumn.ColumnName])},
                          { "txtRemitToZipExt", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.RemitToZipExtColumn.ColumnName])},
                          { "txtOfficeHoursFrom", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.OfcHoursFromColumn.ColumnName])},
                          { "txtOfficeHoursTo", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.OfcHoursToColumn.ColumnName])},
                          { "txtTelePhone", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.PhoneNoColumn.ColumnName])},
                          { "txtPhoneExt", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.PhoneExtColumn.ColumnName])},
                         
                         // { "ddlLetters", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.LetterIdColumn.ColumnName])},

                         
                        };

                                if (MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.StatementImageColumn.ColumnName]).Contains("<table"))
                                {
                                    int ImgaeBinaryStartIndex = MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.StatementImageColumn.ColumnName]).IndexOf("src=");
                                    int ImgaeBinaryendIndex = MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.StatementImageColumn.ColumnName]).IndexOf("alt");
                                    int length = ImgaeBinaryendIndex - ImgaeBinaryStartIndex;
                                    var imagebinary = MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.StatementImageColumn.ColumnName]).Substring(ImgaeBinaryStartIndex + 5, length - 7);
                                    keyValues.Add("imgGroupImage", imagebinary);
                                }
                                else
                                {
                                    keyValues.Add("imgGroupImage", MDVUtility.ToStr(dr[dsPatientStatement.PatientStatementGroup.StatementImageColumn.ColumnName]));
                                }

                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    StatementGroupFill_JSON = js.Serialize(keyValues)
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = Common.AppPrivileges.No_Record_Message,
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

                case "SAVE_STATEMENT_GROUP":
                    {
                        string fieldsJSON = context.Request["StatementGroupData"];
                        string strJSONData = SaveStatementGroup(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


                case "SEARCH_STATEMENT_GROUP":
                    {
                        string fieldsJSON = context.Request["StatementGroupData"];
                        string page = context.Request["page"];
                        int pageNo = MDVUtility.ToInt(context.Request["pageNo"]);
                        int recordPerPage = MDVUtility.ToInt(context.Request["recordPerPage"]);
                        Int64 StatementGroupID = MDVUtility.ToInt64(context.Request["StatementGroupID"]);
                        string strJSONData = SearchStatementGroup(fieldsJSON, StatementGroupID, pageNo, recordPerPage);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_STATEMENT_GROUP_ACTIVE_INACTIVE":
                    {
                        Int64 StatementGroupID = MDVUtility.ToInt64(context.Request["StatementGroupID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateStatementGroupActive(StatementGroupID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_STATEMENT_GROUP":
                    {
                        string fieldsJSON = context.Request["StatementGroupData"];
                        string strJSONData = UpdateStatementGroup(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_STATEMENT_GROUP":
                    {
                        string StatementGroupId = context.Request["StatementGroupId"];
                        string strJSONData = DeleteStatementGroup(MDVUtility.ToInt64(StatementGroupId));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


                case "FILL_STATEMENT_GROUP":
                    {

                        string StatementGroupID = context.Request["StatementGroupID"];
                        string strJSONData = FillStatementGroup(MDVUtility.ToInt64(StatementGroupID));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;




            }
        }





        #endregion
    }
}