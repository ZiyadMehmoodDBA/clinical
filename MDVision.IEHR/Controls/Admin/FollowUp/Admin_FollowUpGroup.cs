using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin.FollowUp
{
    public class Admin_FollowUpGroup
    {
         private BLLAdminFollowUp BLLAdminFollowUpObj = null;
         public Admin_FollowUpGroup()
        {
            BLLAdminFollowUpObj = new BLLAdminFollowUp();
        }
        #region Singleton
        private static Admin_FollowUpGroup _obj = null;
        public static Admin_FollowUpGroup Instance()
        {
            if (_obj == null)
                _obj = new Admin_FollowUpGroup();
            return _obj;
        }
        #endregion

        #region Private Functions

        #region AR Group
        private string SaveFollowUpGroup(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSFollowUp dsFollowUpGroup = new DSFollowUp();
                DSFollowUp.FollowupARGroupRow dr = dsFollowUpGroup.FollowupARGroup.NewFollowupARGroupRow();

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtShortName"])))
                    dr.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["txtShortName"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"])))
                    dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);

                dr.IsActive = Convert.ToBoolean(SearchedfieldsJSON["chkIsActive"]);

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsFollowUpGroup.FollowupARGroup.AddFollowupARGroupRow(dr);
                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.InsertARGroup(dsFollowUpGroup);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        GroupId = dsFollowUpGroup.Tables[dsFollowUpGroup.FollowupARGroup.TableName].Rows[0][dsFollowUpGroup.FollowupARGroup.ARGroupIdColumn.ColumnName],
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
        private string UpdateFollowUpGroup(string fieldsJSON, Int64 GroupId)
        {

            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSFollowUp dsFollowUpGroup = new DSFollowUp();
                BLObject<DSFollowUp> objLoad = BLLAdminFollowUpObj.LoadARGroup(GroupId, "", "", "", 1, 15);
                dsFollowUpGroup = objLoad.Data;
                foreach (DSFollowUp.FollowupARGroupRow dr in dsFollowUpGroup.Tables[dsFollowUpGroup.FollowupARGroup.TableName].Rows)
                {
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtShortName"])))
                        dr.ShortName = MDVUtility.ToStr(SearchedfieldsJSON["txtShortName"]);

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"])))
                        dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);

                    dr.IsActive = Convert.ToBoolean(SearchedfieldsJSON["chkIsActive"]);

                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Updation



                if (dsFollowUpGroup.Tables[dsFollowUpGroup.FollowupARGroup.TableName].Rows.Count > 0)
                {
                    BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.UpdateARGroup(dsFollowUpGroup);
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
        private string LoadFollowUpGroup(string fieldsJSON, Int64 GroupId, int PageNumber, int RowsPerPage)
        {

            string description = null, shortName = null;
          //  string isActive = null;

            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);


                if (SearchedfieldsJSON["txtShortName"] != "")
                    shortName = SearchedfieldsJSON["txtShortName"];

                if (SearchedfieldsJSON["txtDiscription"] != "")
                    description = SearchedfieldsJSON["txtDiscription"];

               // isActive = MDVUtility.ToStr(SearchedfieldsJSON["chkIsActive"]);


                DSFollowUp dsFollowUp = null;
                BLObject<DSFollowUp> obj;
                obj = BLLAdminFollowUpObj.LoadARGroup(GroupId, shortName, description, SearchedfieldsJSON["ddlActive"], PageNumber, RowsPerPage);
                dsFollowUp = obj.Data;

                if (obj.Data != null)
                {
                    if (dsFollowUp.Tables[dsFollowUp.FollowupARGroup.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            GroupCount = dsFollowUp.Tables[dsFollowUp.FollowupARGroup.TableName].Rows.Count,
                            iTotalDisplayRecords = dsFollowUp.FollowupARGroup.Rows[0][dsFollowUp.FollowupARGroup.RecordCountColumn.ColumnName],
                            FollowUpGroupLoad_JSON = MDVUtility.JSON_DataTable(dsFollowUp.Tables[dsFollowUp.FollowupARGroup.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            GroupCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        GroupCount = 0,
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
        private string DeleteFollowUpGroup(long groupId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(groupId)))
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
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteARGroup(groupId);
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
        private string FillFollowUpGroup(Int64 groupId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(groupId)))
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
                    DSFollowUp dsFollowUpGroup = new DSFollowUp();
                    
                    BLObject<DSFollowUp> objLoad = BLLAdminFollowUpObj.LoadARGroup(groupId,"","","",1,15);
                    dsFollowUpGroup.Merge(objLoad.Data.Tables[objLoad.Data.FollowupARGroup.TableName]);
                    
                    BLObject<DSFollowUp> objFacilityGroup =  BLLAdminFollowUpObj.LoadARFacilityGroup(0, groupId, 1, 1000);
                    dsFollowUpGroup.Merge(objFacilityGroup.Data.Tables[objFacilityGroup.Data.FollowupARFacilityGroup.TableName]);

                    BLObject<DSFollowUp> objPlanCategoryGroup = BLLAdminFollowUpObj.LoadARPlanCategoryGroup(0, groupId, 1, 1000);
                    dsFollowUpGroup.Merge(objPlanCategoryGroup.Data.Tables[objPlanCategoryGroup.Data.FollowupARPlanCategoryGroup.TableName]);

                    BLObject<DSFollowUp> objPlanTypeGroup = BLLAdminFollowUpObj.LoadARPTGroup(0, groupId, 1, 1000);
                    dsFollowUpGroup.Merge(objPlanTypeGroup.Data.Tables[objPlanTypeGroup.Data.FollowupARPlanTypeGroup.TableName]);

                    BLObject<DSFollowUp> objPOSGroup = BLLAdminFollowUpObj.LoadARPOSGroup(0, groupId, 1, 1000);
                    dsFollowUpGroup.Merge(objPOSGroup.Data.Tables[objPOSGroup.Data.FollowupARPOSGroup.TableName]);

                    BLObject<DSFollowUp> objProviderGroup = BLLAdminFollowUpObj.LoadARProviderGroup(0, groupId, 1, 1000);
                    dsFollowUpGroup.Merge(objProviderGroup.Data.Tables[objProviderGroup.Data.FollowupARProviderGroup.TableName]);
                    
                    if (dsFollowUpGroup.Tables[dsFollowUpGroup.FollowupARGroup.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsFollowUpGroup.Tables[dsFollowUpGroup.FollowupARGroup.TableName].Rows[0];

                        var keyValues = new Dictionary<string, string>
                        {
                            { "txtShortName", MDVUtility.ToStr(dr[dsFollowUpGroup.FollowupARGroup.ShortNameColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsFollowUpGroup.FollowupARGroup.DescriptionColumn.ColumnName])},
                            { "chkIsActive",  Convert.ToBoolean(dr[dsFollowUpGroup.FollowupARGroup.IsActiveColumn.ColumnName]).ToString()}
                          
                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            GroupLoad_JSON = js.Serialize(keyValues),
                            FollowUpFacilityGroupLoad_JSON = MDVUtility.JSON_DataTable(dsFollowUpGroup.Tables[dsFollowUpGroup.FollowupARFacilityGroup.TableName]),
                            FollowUpPlanCategoryGroupLoad_JSON = MDVUtility.JSON_DataTable(dsFollowUpGroup.Tables[dsFollowUpGroup.FollowupARPlanCategoryGroup.TableName]),
                            FollowUpPlanTypeGroupLoad_JSON = MDVUtility.JSON_DataTable(dsFollowUpGroup.Tables[dsFollowUpGroup.FollowupARPlanTypeGroup.TableName]),
                            FollowUpPOSGroupLoad_JSON = MDVUtility.JSON_DataTable(dsFollowUpGroup.Tables[dsFollowUpGroup.FollowupARPOSGroup.TableName]),
                            FollowUpProviderGroupLoad_JSON = MDVUtility.JSON_DataTable(dsFollowUpGroup.Tables[dsFollowUpGroup.FollowupARProviderGroup.TableName]),
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

        private string GroupUpdateActiveInactive(Int64 ARGroupId, Int64 IsActive)
        {
            try
            {
                DSFollowUp dsARGroup = new DSFollowUp();

                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.LoadARGroup(ARGroupId);
                if (obj.Data != null)
                {
                    dsARGroup = obj.Data;
                    if (dsARGroup.Tables[dsARGroup.FollowupARGroup.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsARGroup.Tables[dsARGroup.FollowupARGroup.TableName].Rows[0];
                        dr[dsARGroup.FollowupARGroup.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSFollowUp> objARGroup = BLLAdminFollowUpObj.UpdateARGroup(dsARGroup);
                        string successMsg;
                        if (objARGroup.Data != null)
                        {
                            if (IsActive == 0)
                                successMsg = Common.AppPrivileges.Inactive_Message;
                            else
                                successMsg = Common.AppPrivileges.Active_Message;
                            var response = new
                            {
                                status = true,
                                Message = successMsg
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objARGroup.Message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region  AR Provider Group
        private string SaveARProviderGroup(Int64 providerId , Int64 ARGroupId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSFollowUp dsFollowUp = new DSFollowUp();
                DSFollowUp.FollowupARProviderGroupRow dr = dsFollowUp.FollowupARProviderGroup.NewFollowupARProviderGroupRow();
                dr.ProviderId = providerId;
                dr.ARGroupId = ARGroupId;

                #region Database Insertion
                dsFollowUp.FollowupARProviderGroup.AddFollowupARProviderGroupRow(dr);
                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.InsertARProviderGroup(dsFollowUp);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        ProviderGroupId = dsFollowUp.Tables[dsFollowUp.FollowupARProviderGroup.TableName].Rows[0][dsFollowUp.FollowupARProviderGroup.ProviderGrpIdColumn.ColumnName],
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
        private string DeleteARProviderGroup(long providerGroupId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(providerGroupId)))
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
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteARProviderGroup(providerGroupId);
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
        private string SaveARGroupProviderCheckAll(Int64 ARGroupId, string ProviderJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var ProviderJson = ser.Deserialize<dynamic>(ProviderJSON);
                DSFollowUp dsFollowUp = new DSFollowUp();

                foreach (dynamic Ids in ProviderJson)
                {
                    string ProviderId = Ids["ProviderId"];
                    DSFollowUp.FollowupARProviderGroupRow dr = dsFollowUp.FollowupARProviderGroup.NewFollowupARProviderGroupRow();
                    dr.ProviderId = MDVUtility.ToInt64(ProviderId);
                    dr.ARGroupId = ARGroupId;
                    dsFollowUp.FollowupARProviderGroup.AddFollowupARProviderGroupRow(dr);

                }

                #region Database Insertion
                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.InsertARProviderGroup(dsFollowUp);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        Provider_JSON = MDVUtility.JSON_DataTable(dsFollowUp.Tables[dsFollowUp.FollowupARProviderGroup.TableName]),
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
        private string DeleteARGroupProviderCheckAll(string ProviderJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var ProviderJSon = ser.Deserialize<dynamic>(ProviderJSON);

                if (string.IsNullOrEmpty(MDVUtility.ToStr(ProviderJSon)))
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
                    foreach (dynamic Ids in ProviderJSon)
                    {

                        string ProviderGroupId = Ids["ProviderId"];
                        BLObject<string> obj = BLLAdminFollowUpObj.DeleteARProviderGroup(MDVUtility.ToInt64(ProviderGroupId));
                    }

                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message
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

        #region  AR Facility Group
        private string SaveARFacilityGroup(Int64 FacilityId, Int64 ARGroupId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSFollowUp dsFollowUp = new DSFollowUp();
                DSFollowUp.FollowupARFacilityGroupRow dr = dsFollowUp.FollowupARFacilityGroup.NewFollowupARFacilityGroupRow();
                dr.FacilityId = FacilityId;
                dr.ARGroupId = ARGroupId;

                #region Database Insertion
                dsFollowUp.FollowupARFacilityGroup.AddFollowupARFacilityGroupRow(dr);
                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.InsertARFacilityGroup(dsFollowUp);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        FacilityGroupId = dsFollowUp.Tables[dsFollowUp.FollowupARFacilityGroup.TableName].Rows[0][dsFollowUp.FollowupARFacilityGroup.FacilityGrpIdColumn.ColumnName],
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
        private string DeleteARFacilityGroup(long FacilityGroupId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(FacilityGroupId)))
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
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteARFacilityGroup(FacilityGroupId);
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
        private string SaveARGroupFacilityCheckAll(Int64 ARGroupId, string FacilityJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var FacilityJson = ser.Deserialize<dynamic>(FacilityJSON);

                DSFollowUp dsFollowUp = new DSFollowUp();

                foreach (dynamic Ids in FacilityJson)
                {
                    string facilityId = Ids["FacilityId"];
                    DSFollowUp.FollowupARFacilityGroupRow dr = dsFollowUp.FollowupARFacilityGroup.NewFollowupARFacilityGroupRow();
                    dr.FacilityId = MDVUtility.ToInt64(facilityId);
                    dr.ARGroupId = ARGroupId;
                    dsFollowUp.FollowupARFacilityGroup.AddFollowupARFacilityGroupRow(dr);
                }

                #region Database Insertion
                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.InsertARFacilityGroup(dsFollowUp);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        Facility_JSON = MDVUtility.JSON_DataTable(dsFollowUp.Tables[dsFollowUp.FollowupARFacilityGroup.TableName]),
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
        private string DeleteARGroupFacilityCheckAll(string FacilityJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var FacilityJSon = ser.Deserialize<dynamic>(FacilityJSON);

                if (string.IsNullOrEmpty(MDVUtility.ToStr(FacilityJSon)))
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
                    foreach (dynamic Ids in FacilityJSon)
                    {
                        string FacilityGroupId = Ids["FacilityId"];
                        BLObject<string> obj = BLLAdminFollowUpObj.DeleteARFacilityGroup(MDVUtility.ToInt64(FacilityGroupId));
                    }

                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message
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

        #region  AR Plan Category Group
        private string SaveARPlanCategoryGroup(Int64 PlanCategoryId, Int64 ARGroupId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSFollowUp dsFollowUp = new DSFollowUp();
                DSFollowUp.FollowupARPlanCategoryGroupRow dr = dsFollowUp.FollowupARPlanCategoryGroup.NewFollowupARPlanCategoryGroupRow();
                dr.PlanCategoryId = PlanCategoryId;
                dr.ARGroupId = ARGroupId;

                #region Database Insertion
                dsFollowUp.FollowupARPlanCategoryGroup.AddFollowupARPlanCategoryGroupRow(dr);
                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.InsertARPlanCategoryGroup(dsFollowUp);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        PlanCategoryGroupId = dsFollowUp.Tables[dsFollowUp.FollowupARPlanCategoryGroup.TableName].Rows[0][dsFollowUp.FollowupARPlanCategoryGroup.PCGrpIdColumn.ColumnName],
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
        private string DeleteARPlanCategoryGroup(long PlanCategoryGroupId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PlanCategoryGroupId)))
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
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteARPlanCategoryGroup(PlanCategoryGroupId);
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
        private string SaveARGroupPCCheckAll(Int64 ARGroupId, string PCJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var PCJson = ser.Deserialize<dynamic>(PCJSON);


                DSFollowUp dsFollowUp = new DSFollowUp();

                foreach (dynamic Ids in PCJson)
                {

                    string PlanCategoryId = Ids["PlanCategoryId"];

                    DSFollowUp.FollowupARPlanCategoryGroupRow dr = dsFollowUp.FollowupARPlanCategoryGroup.NewFollowupARPlanCategoryGroupRow();

                    dr.PlanCategoryId = MDVUtility.ToInt64(PlanCategoryId);
                    dr.ARGroupId = ARGroupId;

                    dsFollowUp.FollowupARPlanCategoryGroup.AddFollowupARPlanCategoryGroupRow(dr);

                }

                #region Database Insertion
                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.InsertARPlanCategoryGroup(dsFollowUp);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        PC_JSON = MDVUtility.JSON_DataTable(dsFollowUp.Tables[dsFollowUp.FollowupARPlanCategoryGroup.TableName]),
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
        private string DeleteARGroupPCCheckAll(string PCJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var PCJSon = ser.Deserialize<dynamic>(PCJSON);

                if (string.IsNullOrEmpty(MDVUtility.ToStr(PCJSon)))
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
                    foreach (dynamic Ids in PCJSon)
                    {

                        string PlanCategoryId = Ids["PlanCategoryId"];


                        BLObject<string> obj = BLLAdminFollowUpObj.DeleteARPlanCategoryGroup(MDVUtility.ToInt64(PlanCategoryId)); ;
                    }

                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message
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

        #region  AR POS Group
        private string SaveARPOSGroup(Int64 POSId, Int64 ARGroupId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSFollowUp dsFollowUp = new DSFollowUp();
                DSFollowUp.FollowupARPOSGroupRow dr = dsFollowUp.FollowupARPOSGroup.NewFollowupARPOSGroupRow();
                dr.PlaceOfServiceId = POSId;
                dr.ARGroupId = ARGroupId;

                #region Database Insertion
                dsFollowUp.FollowupARPOSGroup.AddFollowupARPOSGroupRow(dr);
                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.InsertARPOSGroup(dsFollowUp);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        POSGroupId = dsFollowUp.Tables[dsFollowUp.FollowupARPOSGroup.TableName].Rows[0][dsFollowUp.FollowupARPOSGroup.POSGrpIdColumn.ColumnName],
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
        private string DeleteARPOSGroup(long POSGroupId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(POSGroupId)))
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
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteARPOSGroup(POSGroupId);
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
        private string SaveARGroupPOSCheckAll(Int64 ARGroupId, string POSJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var POSJson = ser.Deserialize<dynamic>(POSJSON);

                DSFollowUp dsFollowUp = new DSFollowUp();

                foreach (dynamic Ids in POSJson)
                {

                    string POSId = Ids["POSId"];

                    DSFollowUp.FollowupARPOSGroupRow dr = dsFollowUp.FollowupARPOSGroup.NewFollowupARPOSGroupRow();

                    dr.PlaceOfServiceId = MDVUtility.ToInt64(POSId);
                    dr.ARGroupId = ARGroupId;

                    dsFollowUp.FollowupARPOSGroup.AddFollowupARPOSGroupRow(dr);

                }

                #region Database Insertion
                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.InsertARPOSGroup(dsFollowUp);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        POS_JSON = MDVUtility.JSON_DataTable(dsFollowUp.Tables[dsFollowUp.FollowupARPOSGroup.TableName]),
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
        private string DeleteARGroupPOSCheckAll(string POSJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var POSJSon = ser.Deserialize<dynamic>(POSJSON);

                if (string.IsNullOrEmpty(MDVUtility.ToStr(POSJSon)))
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
                    foreach (dynamic Ids in POSJSon)
                    {

                        string POSGroupId = Ids["POSId"];
                        BLObject<string> obj = BLLAdminFollowUpObj.DeleteARPOSGroup(MDVUtility.ToInt64(POSGroupId));
                    }

                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message
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

        #region  AR Plan Type Group
        private string SaveARPlanTypeGroup(Int64 PlanTypeId, Int64 ARGroupId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSFollowUp dsFollowUp = new DSFollowUp();
                DSFollowUp.FollowupARPlanTypeGroupRow dr = dsFollowUp.FollowupARPlanTypeGroup.NewFollowupARPlanTypeGroupRow();
                dr.PlanTypeId = PlanTypeId;
                dr.ARGroupId = ARGroupId;

                #region Database Insertion
                dsFollowUp.FollowupARPlanTypeGroup.AddFollowupARPlanTypeGroupRow(dr);
                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.InsertARPTGroup(dsFollowUp);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        PlanTypeGroupId = dsFollowUp.Tables[dsFollowUp.FollowupARPlanTypeGroup.TableName].Rows[0][dsFollowUp.FollowupARPlanTypeGroup.PTGrpIdColumn.ColumnName],
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
        private string DeleteARPlanTypeGroup(long PlanTypeGroupId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PlanTypeGroupId)))
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
                    BLObject<string> obj = BLLAdminFollowUpObj.DeleteARPTGroup(PlanTypeGroupId);
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
        private string SaveARGroupPTCheckAll(Int64 ARGroupId, string PTJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var PTJson = ser.Deserialize<dynamic>(PTJSON);

                DSFollowUp dsFollowUp = new DSFollowUp();

                foreach (dynamic Ids in PTJson)
                {

                    string PlanTypeId = Ids["PlanTypeId"];

                    DSFollowUp.FollowupARPlanTypeGroupRow dr = dsFollowUp.FollowupARPlanTypeGroup.NewFollowupARPlanTypeGroupRow();

                    dr.PlanTypeId = MDVUtility.ToInt64(PlanTypeId);
                    dr.ARGroupId = ARGroupId;

                    dsFollowUp.FollowupARPlanTypeGroup.AddFollowupARPlanTypeGroupRow(dr);

                }

                #region Database Insertion
                BLObject<DSFollowUp> obj = BLLAdminFollowUpObj.InsertARPTGroup(dsFollowUp);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        PT_JSON = MDVUtility.JSON_DataTable(dsFollowUp.Tables[dsFollowUp.FollowupARPlanTypeGroup.TableName]),
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
        private string DeleteARGroupPTCheckAll(string PTJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var PTJSon = ser.Deserialize<dynamic>(PTJSON);

                if (string.IsNullOrEmpty(MDVUtility.ToStr(PTJSon)))
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
                    foreach (dynamic Ids in PTJSon)
                    {
                        string PlanTypeId = Ids["PlanTypeId"];
                        BLObject<string> obj = BLLAdminFollowUpObj.DeleteARPTGroup(MDVUtility.ToInt64(PlanTypeId));
                    }

                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message
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

        #endregion

        #region Service Command Handler

        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_GROUP":
                    {
                        string fieldsJSON = context.Request["groupData"];
                        Int64 groupId = MDVUtility.ToInt64(context.Request["groupId"]);
                        int pageNo = MDVUtility.ToInt(context.Request["pageNo"]);
                        int recordPerPage = MDVUtility.ToInt(context.Request["recordPerPage"]);
                        string strJSONData = LoadFollowUpGroup(fieldsJSON, groupId, pageNo, recordPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "DELETE_GROUP":
                    {
                        Int64 groupId = MDVUtility.ToInt64(context.Request["groupId"]);
                        string strJSONData = DeleteFollowUpGroup(groupId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                
                case "UPDATE_GROUP_ACTIVE_INACTIVE":
                    {
                        Int64 ARGroupId = MDVUtility.ToInt64(context.Request["ARGroupId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = GroupUpdateActiveInactive(ARGroupId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SAVE_GROUP":
                    {
                        string fieldsJson = context.Request["groupData"];
                        string strJsonData = SaveFollowUpGroup(fieldsJson);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "UPDATE_GROUP":
                    {
                        string fieldsJson = context.Request["groupData"];
                        Int64 groupId = MDVUtility.ToInt64(context.Request["groupId"]);
                        string strJsonData = UpdateFollowUpGroup(fieldsJson, groupId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "FILL_GROUP":
                    {
                        string groupId = context.Request["groupId"];
                        string strJsonData = FillFollowUpGroup(MDVUtility.ToInt64(groupId));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "SAVE_AR_PROVIDER_GROUP":
                    {
                        Int64 providerId = MDVUtility.ToInt64(context.Request["providerId"]);
                        Int64 ARGroupId = MDVUtility.ToInt64(context.Request["ARGroupId"]);
                        string strJsonData = SaveARProviderGroup(providerId,ARGroupId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "DELETE_AR_PROVIDER_GROUP":
                    {
                        Int64 providerGroupId = MDVUtility.ToInt64(context.Request["providerGroupId"]);
                        string strJsonData = DeleteARProviderGroup(providerGroupId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "SAVE_AR_FACILITY_GROUP":
                    {
                        Int64 FacilityId = MDVUtility.ToInt64(context.Request["FacilityId"]);
                        Int64 ARGroupId = MDVUtility.ToInt64(context.Request["ARGroupId"]);
                        string strJsonData = SaveARFacilityGroup(FacilityId, ARGroupId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "DELETE_AR_FACILITY_GROUP":
                    {
                        Int64 FacilityGroupId = MDVUtility.ToInt64(context.Request["FacilityGroupId"]);
                        string strJsonData = DeleteARFacilityGroup(FacilityGroupId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "SAVE_AR_PLAN_CATEGORY_GROUP":
                    {
                        Int64 PlanCategoryId = MDVUtility.ToInt64(context.Request["PlanCategoryId"]);
                        Int64 ARGroupId = MDVUtility.ToInt64(context.Request["ARGroupId"]);
                        string strJsonData = SaveARPlanCategoryGroup(PlanCategoryId, ARGroupId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "DELETE_AR_PLAN_CATEGORY_GROUP":
                    {
                        Int64 PlanCategoryGroupId = MDVUtility.ToInt64(context.Request["PlanCategoryGroupId"]);
                        string strJsonData = DeleteARPlanCategoryGroup(PlanCategoryGroupId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "SAVE_AR_POS_GROUP":
                    {
                        Int64 POSId = MDVUtility.ToInt64(context.Request["POSId"]);
                        Int64 ARGroupId = MDVUtility.ToInt64(context.Request["ARGroupId"]);
                        string strJsonData = SaveARPOSGroup(POSId, ARGroupId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "DELETE_AR_POS_GROUP":
                    {
                        Int64 POSGroupId = MDVUtility.ToInt64(context.Request["POSGroupId"]);
                        string strJsonData = DeleteARPOSGroup(POSGroupId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "SAVE_AR_PLAN_TYPE_GROUP":
                    {
                        Int64 PlanTypeId = MDVUtility.ToInt64(context.Request["PlanTypeId"]);
                        Int64 ARGroupId = MDVUtility.ToInt64(context.Request["ARGroupId"]);
                        string strJsonData = SaveARPlanTypeGroup(PlanTypeId, ARGroupId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "DELETE_AR_PLAN_TYPE_GROUP":
                    {
                        Int64 PlanTypeGroupId = MDVUtility.ToInt64(context.Request["PlanTypeGroupId"]);
                        string strJsonData = DeleteARPlanTypeGroup(PlanTypeGroupId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "SAVE_AR_GROUP_FACILITY_CHECK_ALL":
                    {
                        string ARGroupId = context.Request["ARGroupId"];
                        string FacilityJSON = context.Request["FacilityJSon"];
                        string strJSONData = SaveARGroupFacilityCheckAll(MDVUtility.ToInt64(ARGroupId), FacilityJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_AR_GROUP_FACILITY_CHECK_ALL":
                    {
                        string FacilityJSON = context.Request["FacilityJSon"];
                        string strJSONData = DeleteARGroupFacilityCheckAll(FacilityJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_AR_GROUP_PROVIDER_CHECK_ALL":
                    {
                        string ARGroupId = context.Request["ARGroupId"];
                        string ProviderJSON = context.Request["ProviderJSON"];
                        string strJSONData = SaveARGroupProviderCheckAll(MDVUtility.ToInt64(ARGroupId), ProviderJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_AR_GROUP_PROVIDER_CHECK_ALL":
                    {
                        string ProviderJSON = context.Request["ProviderJSON"];
                        string strJSONData = DeleteARGroupProviderCheckAll(ProviderJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_AR_GROUP_POS_CHECK_ALL":
                    {
                        string ARGroupId = context.Request["ARGroupId"];
                        string POSJSON = context.Request["POSJSON"];
                        string strJSONData = SaveARGroupPOSCheckAll(MDVUtility.ToInt64(ARGroupId), POSJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_AR_GROUP_POS_CHECK_ALL":
                    {
                        string POSJSON = context.Request["POSJSON"];
                        string strJSONData = DeleteARGroupPOSCheckAll(POSJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_AR_GROUP_PLANTYPE_CHECK_ALL":
                    {
                        string ARGroupId = context.Request["ARGroupId"];
                        string PTJSON = context.Request["PTJSON"];
                        string strJSONData = SaveARGroupPTCheckAll(MDVUtility.ToInt64(ARGroupId), PTJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_AR_GROUP_PLANTYPE_CHECK_ALL":
                    {
                        string PTJSON = context.Request["PTJSON"];
                        string strJSONData = DeleteARGroupPTCheckAll(PTJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_AR_GROUP_PLANCATEGORY_CHECK_ALL":
                    {
                        string ARGroupId = context.Request["ARGroupId"];
                        string PCJSON = context.Request["PCJSON"];
                        string strJSONData = SaveARGroupPCCheckAll(MDVUtility.ToInt64(ARGroupId), PCJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_AR_GROUP_PLANCATEGORY_CHECK_ALL":
                    {
                        string PCJSON = context.Request["PCJSON"];
                        string strJSONData = DeleteARGroupPCCheckAll(PCJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}