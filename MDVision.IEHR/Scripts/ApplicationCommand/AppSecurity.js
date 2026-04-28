
//var No_Add_Privilege_Message = "You don't have sufficient privileges for adding.\nPlease contact your Administrator";
//var No_Edit_Privilege_Message = "You don't have sufficient privileges for editing.\nPlease contact your Administrator";
//var No_Delete_Privilege_Message = "You don't have sufficient privileges for deletion.\nPlease contact your Administrator";
//var No_View_Privilege_Message = "You don't have sufficient privileges for view.\nPlease contact your Administrator";
//var No_Print_Privilege_Message = "You don't have sufficient privileges for print.\nPlease contact your Administrator";
//var No_Save_Privilege_Message = "You don't have sufficient privileges for save.\nPlease contact your Administrator";
//var No_Search_Privilege_Message = "You don't have sufficient privileges for search.\nPlease contact your Administrator";
//var No_OverBooking_Privilege_Message = "You don't have sufficient privileges for over booking.\nPlease contact your Administrator";
//var No_Email_Privilege_Message = "You don't have sufficient privileges for Email.\nPlease contact your Administrator";

//var selectPermission = "SELECT";
//var insertPermission = "INSERT";
//var deletePermission = "DELETE";
//var updatePermission = "UPDATE";

AppPrivileges = {

    GetFormPrivileges: function (FormName, Permission, controlName, cammandAction, callback) {
       // var str = "";
        AppPrivileges.appSecurity(FormName,Permission, controlName, cammandAction).done(function (response) {
            //var permissions = response.split(',');

            //$.each(permissions, function (index, value) {
            //    if (value == Permission) {
            //        str = No_View_Privilege_Message;
            //        return false;
            //    }
            //    else if (value == Permission) {
            //        str = No_Add_Privilege_Message;
            //        return false;
            //    }
            //        //$("#btnSpecialityAdd").show();
            //    else if (value == Permission) {
            //        str = No_Delete_Privilege_Message;
            //        // gvDeleteRow = value;
            //        return false;
            //    }
            //    else if (value == Permission) {
            //        str = No_Edit_Privilege_Message;
            //        // gvUpdateRow = value;
            //        return false;
            //    }

            //});
            callback(response);
        });

    },

    appSecurity: function (formname,Permission, controlName, cammandAction) {
        
        var data = "FormName=" + formname + "&Permission=" + Permission;
        return MDVisionService.defaultService(data, controlName, cammandAction);
    },

    GetMultipleFormPrivileges: function (data_, controlName, cammandAction, callback) {
        AppPrivileges.GetPrivileges_DbCall(data_, controlName, cammandAction).done(function (response) {
            callback(response);
        });

    },
    GetPrivileges_DbCall: function (data_, controlName, cammandAction) {

        return MDVisionService.defaultService(data_, controlName, cammandAction);
    },
    GetFormPrivilegesByModule: function (FormName, Permission,ModuleName, controlName, cammandAction, callback) {
        // var str = "";
        AppPrivileges.GetFormPrivilegesByModule_DbCall(FormName, Permission, ModuleName, controlName, cammandAction).done(function (response) {
            //var permissions = response.split(',');

            //$.each(permissions, function (index, value) {
            //    if (value == Permission) {
            //        str = No_View_Privilege_Message;
            //        return false;
            //    }
            //    else if (value == Permission) {
            //        str = No_Add_Privilege_Message;
            //        return false;
            //    }
            //        //$("#btnSpecialityAdd").show();
            //    else if (value == Permission) {
            //        str = No_Delete_Privilege_Message;
            //        // gvDeleteRow = value;
            //        return false;
            //    }
            //    else if (value == Permission) {
            //        str = No_Edit_Privilege_Message;
            //        // gvUpdateRow = value;
            //        return false;
            //    }

            //});
            callback(response);
        });

    },

    GetFormPrivilegesByModule_DbCall: function (formname, Permission,ModuleName, controlName, cammandAction) {

        var data = "FormName=" + formname + "&ModuleName=" + ModuleName + "&Permission=" + Permission;
        return MDVisionService.defaultService(data, controlName, cammandAction);
    },
}