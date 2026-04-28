using System.Linq;
using System.Web;
using System.Web.UI;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MDVision.IEHR.Common
{
    public static class AppPrivileges
    {
        #region Variables

        private const string No_Add_Privilege_Message =
            "You don't have sufficient privileges for adding.\nPlease contact your Administrator.";

        private const string No_Edit_Privilege_Message =
            "You don't have sufficient privileges for modifying.\nPlease contact your Administrator.";

        private const string No_Delete_Privilege_Message =
            "You don't have sufficient privileges for deletion.\nPlease contact your Administrator.";

        private const string No_View_Privilege_Message =
            "You don't have sufficient privileges for view.\nPlease contact your Administrator.";

        private const string No_Print_Privilege_Message =
            "You don't have sufficient privileges for print.\nPlease contact your Administrator.";

        private const string No_Save_Privilege_Message =
            "You don't have sufficient privileges for save.\nPlease contact your Administrator.";

        private const string No_Search_Privilege_Message =
            "You don't have sufficient privileges for search.\nPlease contact your Administrator.";

        private const string No_OverBooking_Privilege_Message =
            "You don't have sufficient privileges for over booking.\nPlease contact your Administrator.";

        private const string No_Email_Privilege_Message =
            "You don't have sufficient privileges for Email.\nPlease contact your Administrator.";

        private const string No_Scan_Privilege_Message =
            "You don't have sufficient privileges for Scan.\nPlease contact your Administrator.";

        private const string No_Link_Privilege_Message =
          "You don't have sufficient privileges for Link Document.\nPlease contact your Administrator.";

        private const string No_ReviewAndSign_Privilege_Message =
      "You don't have sufficient privileges for Review and Sign.\nPlease contact your Administrator.";

        private const string No_PrivacyCheck_Privilege_Message =
      "You don't have sufficient privileges for Privacy Check.\nPlease contact your Administrator.";

        public const string Delete_Message = "Successfully Deleted";
        public const string Suspend_Message = "Successfully Suspended";
        public const string Update_Message = "Successfully Updated";
        public const string Save_Message = "Successfully Saved";

        public const string Save_Message_Contact_Admin =
            "Successfully created please contact administrator to get access.";

        public const string Inactive_Message = "Successfully Inactive";
        public const string Active_Message = "Successfully Active";
        public const string IsPrimary_Contact_Message = "Successfully Marked as Primary Contact";
        public const string IsNonPrimary_Contact_Message = "Successfully Marked as Non-Primary Contact";
        public const string Scan_Message = "Successfully Scanned.";
        public const string Select_Message = "Please select a record first";
        public const string CheckBox_Message = "Please click the cross sign to delete record";
        public const string Mark_As_Submitted_Message = "Selected claims are marked as submitted successfully.";
        public const string Submitted_Success_Message = "Claim submitted successfully.";
        public const string Statement_Submitted_Success_Message = "Statement submitted successfully.";
        public const string Download_Success_Message = "Successfully Downloaded.";
        public const string Payment_Posted_Success = "Payment Posted Successfully.";
        public const string Payment_Posted_Partially_Success = "Payment Posted Partially Successfully.";
        public const string Password_Change_Success = "Password Changed Successfully.";
        public const string No_Charge_Message = "Couldn't found any charge for payment posting.";
        public const string Quick_Reminder_Success_Message = "Quick Reminder sent Successfully.";

        public const string No_Record_Message = "No record Found.";
        public const string No_New_Report_Message = "Could not find any new report to download.";

        public const string Delete_Error_Message = "Failed to delete the record.";
        public const string Update_Error_Message = "Failed to Update the record.";
        public const string Save_Error_Message = "Failed to save the record.";
        public const string Inactive_Error_Message = "Failed to inactive the record.";
        public const string Active_Error_Message = "Failed to active the record.";
        public const string Select_Error_Message = "Failed to select the record.";
        public const string Download_Error_Message = "Failed to Download.";
        public const string Payment_Posted_Error = "Failed to Post Payment.";
        public const string NO_AMOUNT_ADDED = "Please Add a valid Amount.";

        public const string Load_Favorities_Error_Message = "Failed to load favorite list.";
        public const string Upload_Image = "Successfully upload image.";
        public const string Resubmit_Visit_Charge_Message = "Successfully Resubmitted";
        public const string Resubmit_Error_Message = "Failed to Resubmit";
        public const string Void_And_ReCreate_Success_Message = "Claim Voided and recreated Successfully";
        public const string Split_Claim_Success_Message = "Claim Splitted Successfully";

        public const string Refund_Message = "Successfully Refunded";
        public const string Refund_Error_Message = "Failed to Refund";

        public const string Split_Claim_Message = "Claim SPlitted Successfully.";
        public const string Split_Claim_Error_Message = "Failed to Split Claim";

        public const string DefaultUser = "MDVISION";

        public const string SESSION_CANCELLED_MESSAGE = "Your access is revoked by another user. Please reset your password or contact support for further assistance.";
        public const string DEPLOYMENT_MESSAGE = "You have been logged out due to deployment.";
        public const string DEPLOYMENT_OR_SESSION_TIMEOUT_MESSAGE = "Your session has expired due to timed out or deployment, please re-login to continue.";
        public const string SESSION_TIMEOUT_MESSAGE = "Your session has timed out Please re-login to continue.";


        public enum Permissions : long
        {
            VIEW = 0,
            ADD = 1,
            EDIT = 2,
            DELETE = 3,
            SAVE = 4,
            PRINT = 5,
            SEARCH = 6,
            OVERBOOKING = 7,
            EMAIL = 8,
            SCAN = 9,
            LINK = 10,
            REVIEWANDSIGN = 11,
            PRIVACYCHECK = 12,
            UNDEFINED
        }

        #endregion

        #region Support Functions

        public static void UserPrivilages(Page p)
        {
            if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == DefaultUser)
                EnableDisableControls(p.FindControl("mainForm"), true, "1");
            else
            {
                EnableDisableControls(p.FindControl("mainForm"), true, "0");
                if (MDVSession.Current.ListUserPrivileges == null) return;

                #region Modules

                var modules =
                    MDVSession.Current.ListUserPrivileges.AsEnumerable()
                        .Select(s => s.ModuleHTMLId)
                        .Distinct()
                        .ToArray();

                foreach (var moduleHtmlId in modules)
                {
                    if (string.IsNullOrEmpty(moduleHtmlId)) continue;

                    var ctrl = p.FindControl(moduleHtmlId);
                    if (ctrl != null)
                    {
                        ctrl.Visible = true;
                        if (moduleHtmlId.ToLower().Contains("admin") && MDVSession.Current.IsAdmin == false)
                            ctrl.Visible = false;
                    }

                    var moduleHtmlTabId = moduleHtmlId.Replace("Menu", "Tab");
                    var ctrlTab = p.FindControl(moduleHtmlTabId);
                    if (ctrlTab == null) continue;
                    ctrlTab.Visible = true;
                    if (moduleHtmlTabId.ToLower().Contains("admin") && MDVSession.Current.IsAdmin == false)
                        ctrlTab.Visible = false;
                }

                #endregion

                #region Forms

                var formsid =
                    MDVSession.Current.ListUserPrivileges.AsEnumerable().Select(s => s.FormHTMLId).Distinct().ToArray();
                foreach (
                    var ctrl in
                        formsid.Where(j => !string.IsNullOrEmpty(j)).Select(p.FindControl).Where(ctrl => ctrl != null))
                {
                    if (ctrl.ClientID == "batchMenuExportCCDA" && MDVSession.Current != null && !string.IsNullOrEmpty(MDVSession.Current.isDataExport) && MDVSession.Current.isDataExport.ToLower() == "false")
                    {
                        ctrl.Visible = false;
                        continue;
                    }
                    else if (ctrl.ClientID == "clinicalMenu_History_SocPsyandBehaviorHx" && MDVSession.Current != null && !string.IsNullOrEmpty(MDVSession.Current.isMU3SocPsycBehaviourHx) && MDVSession.Current.isMU3SocPsycBehaviourHx.ToLower() == "false")
                    {
                        ctrl.Visible = false;
                        continue;
                    }
                    else if (ctrl.ClientID == "clinicalMenu_Medical_Implantable" && MDVSession.Current != null && !string.IsNullOrEmpty(MDVSession.Current.isImplantableDevices) && MDVSession.Current.isImplantableDevices.ToLower() == "false")
                    {
                        ctrl.Visible = false;
                        continue;
                    }
                    else if (ctrl.ClientID == "clinicalMenu_Medical_Cognitive" && MDVSession.Current != null && !string.IsNullOrEmpty(MDVSession.Current.isConsolidatedCDACreationPreformance) && MDVSession.Current.isConsolidatedCDACreationPreformance.ToLower() == "false")
                    {
                        ctrl.Visible = false;
                        continue;
                    }
                    else if (ctrl.ClientID == "clinicalMenu_Medical_CarePlan" && MDVSession.Current != null && !string.IsNullOrEmpty(MDVSession.Current.isCarePlan) && MDVSession.Current.isCarePlan.ToLower() == "false")
                    {
                        ctrl.Visible = false;
                        continue;
                    }
                    ctrl.Visible = true;
                }

                #endregion

                #region SubModules

                var subModuleid =
                    MDVSession.Current.ListUserPrivileges.AsEnumerable()
                        .Select(s => s.FormParentHTMLId)
                        .Distinct()
                        .ToArray();
                foreach (var k in subModuleid)
                {
                    if (string.IsNullOrEmpty(k)) continue;
                    var arrParentMenu = k.Split(',');

                    if (arrParentMenu.Length > 1)
                    {
                        foreach (var ctrl in arrParentMenu.Select(p.FindControl).Where(ctrl => ctrl != null))
                        {
                            ctrl.Visible = true;
                        }
                    }
                    else
                    {
                        var ctrl = p.FindControl(k);
                        if (ctrl != null)
                            ctrl.Visible = true;
                    }
                }

                #endregion
            }
        }
        public static void EnableDisableControls(Control parent, bool state, string isadmin)
        {
            if (isadmin == "1")
            {
                foreach (Control c in parent.Controls)
                {
                    if (c.ClientID == "mstrTabAdmin" || c.ClientID == "mstrMenuAdmin" ||
                        c.ClientID.Contains("adminMenu"))
                    {
                        c.Visible = state;
                        EnableDisableControls1(c, state, "1");
                    }
                    if (c.ClientID == "Alerts" || c.ClientID == "AddPatient" || c.ClientID == "PatientSearch" ||
                        c.ClientID == "dassett")
                        c.Visible = false;
                }
            }
            else
            {
                foreach (
                    var c in
                        from Control c in parent.Controls
                        where c.ClientID == "mstrTabDashBoard" || c.ClientID == "mstrMenuDashBoard"
                        select c)
                {
                    c.Visible = state;
                    EnableDisableControls1(c, state, "0");
                }
            }
        }
        public static void EnableDisableControls1(Control parent, bool state, string isAdmin)
        {
            foreach (Control c in parent.Controls)
            {
                c.Visible = state;
                EnableDisableControls(c, state, isAdmin);
            }
        }
        public static string GetMultipleFormSecurity(string JSON_Data)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<AppPerivilegesModel> objModel = ser.Deserialize<List<AppPerivilegesModel>>(JSON_Data);

                var listformPrivileges = MDVSession.Current.ListUserPrivileges;
                if (listformPrivileges == null) return (JsonConvert.SerializeObject(""));

                foreach (AppPerivilegesModel model in objModel)
                {
                    foreach (var permission in model.Permissions)
                    {
                        if (!string.IsNullOrEmpty(permission))
                        {
                            PermissionsResponseModel ResModel = new PermissionsResponseModel();
                            ResModel.PermissionName = permission;

                            if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == DefaultUser)
                            {
                                ResModel.IsAccessible = true;
                            }
                            else
                            {

                                var list =
                                    listformPrivileges.FirstOrDefault(
                                    a =>
                                    (a.FormName != null && a.FormName.ToLower() == MDVUtility.ToStr(model.FormName).ToLower()) &&
                                    (a.PrivilegeName != null && a.PrivilegeName.ToLower() == MDVUtility.ToStr(permission).ToLower()));

                                if (list != null)
                                    ResModel.IsAccessible = true;
                                else
                                {
                                    if (permission == Permissions.SAVE.ToString())
                                        ResModel.Message = No_Save_Privilege_Message;
                                    else if (permission == Permissions.DELETE.ToString())
                                        ResModel.Message = No_Delete_Privilege_Message;
                                    else if (permission == Permissions.SEARCH.ToString())
                                        ResModel.Message = No_Search_Privilege_Message;
                                    else if (permission == Permissions.EDIT.ToString())
                                        ResModel.Message = No_Edit_Privilege_Message;
                                    else if (permission == Permissions.PRINT.ToString())
                                        ResModel.Message = No_Print_Privilege_Message;
                                    else if (permission == Permissions.ADD.ToString())
                                        ResModel.Message = No_Add_Privilege_Message;
                                    else if (permission == Permissions.VIEW.ToString())
                                        ResModel.Message = No_View_Privilege_Message;
                                    else if (permission == Permissions.SCAN.ToString())
                                        ResModel.Message = No_Scan_Privilege_Message;
                                }
                            }

                            model.PermissionsResponseModel.Add(ResModel);
                        }
                    }
                }

                var response = new
                {
                    status = true,
                    Privilege_JSON = objModel,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (System.Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }
        public static string GetFormSecurity(string formName, string permission)
        {
            if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == DefaultUser)
                return (JsonConvert.SerializeObject(""));

            var listformPrivileges = MDVSession.Current.ListUserPrivileges;
            if (listformPrivileges == null) return (JsonConvert.SerializeObject(""));
            var list =
                listformPrivileges.Where(a => MDVUtility.ToStr(a.FormName).ToUpper().Trim() == formName.ToUpper().Trim() && MDVUtility.ToStr(a.PrivilegeName).ToUpper().Trim() == permission.ToUpper().Trim()).ToList();

            if (list.Count > 0) return (JsonConvert.SerializeObject(""));

            if (permission == Permissions.SAVE.ToString())
                return (JsonConvert.SerializeObject(No_Save_Privilege_Message));
            else if (permission == Permissions.DELETE.ToString())
                return (JsonConvert.SerializeObject(No_Delete_Privilege_Message));
            else if (permission == Permissions.SEARCH.ToString())
                return (JsonConvert.SerializeObject(No_Search_Privilege_Message));
            else if (permission == Permissions.EDIT.ToString())
                return (JsonConvert.SerializeObject(No_Edit_Privilege_Message));
            else if (permission == Permissions.PRINT.ToString())
                return (JsonConvert.SerializeObject(No_Print_Privilege_Message));
            else if (permission == Permissions.ADD.ToString())
                return (JsonConvert.SerializeObject(No_Add_Privilege_Message));
            else if (permission == Permissions.VIEW.ToString())
                return (JsonConvert.SerializeObject(No_View_Privilege_Message));
            else if (permission == Permissions.SCAN.ToString())
                return (JsonConvert.SerializeObject(No_Scan_Privilege_Message));

            else if (permission == Permissions.LINK.ToString())
                return (JsonConvert.SerializeObject(No_Link_Privilege_Message));

            else if (permission.Replace(" ", "").Trim().ToUpper() == Permissions.REVIEWANDSIGN.ToString())
                return (JsonConvert.SerializeObject(No_ReviewAndSign_Privilege_Message));

            else if (permission.Replace(" ", "").Trim().ToUpper() == Permissions.PRIVACYCHECK.ToString())
                return (JsonConvert.SerializeObject(No_PrivacyCheck_Privilege_Message));
            return (JsonConvert.SerializeObject(""));



        }
        public static string GetFormSecurityByModuleName(string formName, string permission, string moduleName)
        {
            if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == DefaultUser)
                return (JsonConvert.SerializeObject(""));

            var listformPrivileges = MDVSession.Current.ListUserPrivileges;
            if (listformPrivileges == null) return (JsonConvert.SerializeObject(""));
            var list =
                listformPrivileges.Where(a => MDVUtility.ToStr(a.FormName).ToUpper().Trim() == formName.ToUpper().Trim() && MDVUtility.ToStr(a.PrivilegeName).ToUpper().Trim() == permission.ToUpper().Trim() && MDVUtility.ToStr(a.ModuleName).ToUpper().Trim() == moduleName.ToUpper().Trim()).ToList();

            if (list.Count > 0) return (JsonConvert.SerializeObject(""));

            if (permission == Permissions.SAVE.ToString())
                return (JsonConvert.SerializeObject(No_Save_Privilege_Message));
            else if (permission == Permissions.DELETE.ToString())
                return (JsonConvert.SerializeObject(No_Delete_Privilege_Message));
            else if (permission == Permissions.SEARCH.ToString())
                return (JsonConvert.SerializeObject(No_Search_Privilege_Message));
            else if (permission == Permissions.EDIT.ToString())
                return (JsonConvert.SerializeObject(No_Edit_Privilege_Message));
            else if (permission == Permissions.PRINT.ToString())
                return (JsonConvert.SerializeObject(No_Print_Privilege_Message));
            else if (permission == Permissions.ADD.ToString())
                return (JsonConvert.SerializeObject(No_Add_Privilege_Message));
            else if (permission == Permissions.VIEW.ToString())
                return (JsonConvert.SerializeObject(No_View_Privilege_Message));
            else if (permission == Permissions.SCAN.ToString())
                return (JsonConvert.SerializeObject(No_Scan_Privilege_Message));

            return (JsonConvert.SerializeObject(""));

        }
        public static string GetFormSecurity(string formName, string permission, string moduleName)
        {
            if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == DefaultUser)
                return (JsonConvert.SerializeObject(""));

            var listformPrivileges = MDVSession.Current.ListUserPrivileges;
            if (listformPrivileges == null) return (JsonConvert.SerializeObject(""));

            var list =
                listformPrivileges.Select(
                    a =>
                        a.FormName == MDVUtility.ToLINQFormatString(formName) &&
                        a.PrivilegeName == MDVUtility.ToLINQFormatString(permission) &&
                        a.ModuleName == MDVUtility.ToLINQFormatString(moduleName));
            if (list.Any()) return (JsonConvert.SerializeObject(""));
            if (permission == Permissions.SAVE.ToString())
                return (JsonConvert.SerializeObject(No_Save_Privilege_Message));
            else if (permission == Permissions.DELETE.ToString())
                return (JsonConvert.SerializeObject(No_Delete_Privilege_Message));
            else if (permission == Permissions.SEARCH.ToString())
                return (JsonConvert.SerializeObject(No_Search_Privilege_Message));
            else if (permission == Permissions.EDIT.ToString())
                return (JsonConvert.SerializeObject(No_Edit_Privilege_Message));
            else if (permission == Permissions.PRINT.ToString())
                return (JsonConvert.SerializeObject(No_Print_Privilege_Message));
            else if (permission == Permissions.ADD.ToString())
                return (JsonConvert.SerializeObject(No_Add_Privilege_Message));
            else if (permission == Permissions.VIEW.ToString())
                return (JsonConvert.SerializeObject(No_View_Privilege_Message));
            else if (permission == Permissions.SCAN.ToString())
                return (JsonConvert.SerializeObject(No_Scan_Privilege_Message));
            return (JsonConvert.SerializeObject(""));

        }

        #endregion

        #region Service Command Handler

        public static void CommandHandler(HttpContext context)
        {
            var cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            switch (cammandAction)
            {
                case "GET_FORM_PRIVILEGE":
                    {
                        var formname = context.Request["formname"];
                        var permission = context.Request["Permission"];
                        var strJsonData = GetFormSecurity(formname, permission);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "GET_MULTIPLE_FORM_PRIVILEGE":
                    {
                        string JSON_Data = context.Request["PrivilegeDate"];
                        var strJsonData = GetMultipleFormSecurity(JSON_Data);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
                case "GET_FORM_PRIVILEGE_BY_MODULE":
                    {
                        var formname = context.Request["formname"];
                        var permission = context.Request["Permission"];
                        var ModuleName = context.Request["ModuleName"];
                        var strJsonData = GetFormSecurityByModuleName(formname, permission, ModuleName);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }

        #endregion

        public class AppPerivilegesModel
        {
            public string FormName { get; set; }
            public List<string> Permissions { get; set; }
            public List<PermissionsResponseModel> PermissionsResponseModel { get; set; }
            public AppPerivilegesModel()
            {
                this.Permissions = new List<string>();
                this.PermissionsResponseModel = new List<PermissionsResponseModel>();
            }


        }
        public class PermissionsResponseModel
        {
            public string PermissionName { get; set; }
            public bool IsAccessible { get; set; }
            public string Message { get; set; }
            public PermissionsResponseModel()
            {
                this.IsAccessible = false;
            }

        }
    }
}
