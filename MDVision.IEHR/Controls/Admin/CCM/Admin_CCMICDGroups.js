Admin_CCMICDGroups = {
    params: [],

    Load: function (params) {
        Admin_CCMICDGroups.params = params;
        EMRUtility.SwicthWidgetInializatoin();

        Admin_CCMICDGroups.loadICDGroups();
    },
    loadICDGroups: function () {
        var switcher = $('#CCMICDGroups #switchActive');
        if ($(switcher).attr('IsActive') == '0') {
            Admin_CCMICDGroups.loadCCMICDGroups(null, false, "").done(
           function (resp) {
               if (resp.status != false) {
                   txtGroupId = resp.ICDGroupId;
                   Admin_CCMICDGroups.CCMICDGroupsGridLoad(resp);
               }
               else {
                   Admin_CCMICDGroups.CCMICDGroupsGridLoad(resp);
               }
           });
        }
        else if ($(switcher).attr('IsActive') == '1') {
            Admin_CCMICDGroups.loadCCMICDGroups(null, true, "").done(
       function (resp) {
           if (resp.status != false) {
               txtGroupId = resp.ICDGroupId;
               Admin_CCMICDGroups.CCMICDGroupsGridLoad(resp);
           }
           else {
               Admin_CCMICDGroups.CCMICDGroupsGridLoad(resp);
           }
       });
        }
    },
    loadCCMICDGroups: function (tempid, IsActive, ShortName) {
        var obj = new Object();
        obj["IsActive"] = IsActive;
        obj["ShortName"] = ShortName;
        return Admin_CCMICDGroups.CCMLoadICDGroups(obj, tempid, 1, 1000);
    },

    CCMLoadICDGroups: function (dataObj, templateId, PageNumber, RowsPerPage) {
        dataObj = JSON.stringify(dataObj);
        var data = "CCMICDGroupsData=" + dataObj + "&ICDGroupID=" + templateId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        return MDVisionService.defaultService(data, "ADMIN_CCMICDGROUPS", "LOAD_CCMICDGROUPS");
    },

    CCMICDGroupsGridLoad: function (object) {
        if (object.status == false) {
            object = [];
        }
        else {
            object = JSON.parse(object.CCMICDGroupFill_JSON);
        }
        var data = new kendo.data.DataSource({
            data: object,
            pageSize: 15
        });
        $("#dgvCCMICDGroups").kendoGrid({
            dataSource: data,
            resizable: true,
            scrollable: false,
            noRecords: true,
            messages: {
                noRecords: "There is no data on current page"
            },
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 20, 50, 100],
                buttonCount: 5
            },
            columns: [
            { title: "Action", width: "100px", template: '#=Admin_CCMICDGroups.ActionCCMICDGroup(data)#' },
            { title: "Name", field: "ShortName", title: "Name", width: "100px" },
            { title: "Description", field: "Description", title: "Description", width: "100px" },
            { title: "No of ICD", field: "NoOfICD", width: "100px" },
            { title: "Last Updated", field: "ModifiedOn", title: "Last updated", width: "100px" }
            ],
        });
        //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        utility.removePaginationFromGrid($('#' + Admin_CCMICDGroups.params.PanelID + ' #dgvCCMICDGroups'));
        //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
    },

    ActionCCMICDGroup: function (data) {

        var deleteCCMICDGroupsMethod = "Admin_CCMICDGroups.CCMICDGroupsDelete('" + data.ICDGroupId + "')";
        var editCCMICDGroupsMethod = "Admin_CCMICDGroups.CCMICDGroupsEdit('" + data.ICDGroupId + "')";

        if (data.IsActive == "True") {
            isactive = 0;
            activeTitle = "Active Record";
            tglclass = "fa fa-toggle-on green";
        }
        else {
            isactive = 1;
            activeTitle = "Inactive Record";
            tglclass = "fa fa-toggle-on red fa-rotate-180";
        }
        var activeInactiveCCMICDGroupsMethod = "Admin_CCMICDGroups.CCMICDGroupsActiveInactive('" + data.ICDGroupId + "', '" + isactive + "')";


        //return '<a class="btn  btn-xs" href="javascript:;" onclick="' + editCCMICDGroupsMethod + '" title="Edit ICD Group"><i class="fa fa-edit black"></i></a>';
        return '<a class="btn  btn-xs" href="#" onclick="' + deleteCCMICDGroupsMethod + '" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="javascript:;" onclick="' + editCCMICDGroupsMethod + '" title="Edit ICD Group"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="' + activeInactiveCCMICDGroupsMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
    },

    CCMICDGroupsDelete: function (ICDGroupId) {
        utility.myConfirm("1", function () {
            var data = "ICDGroupId=" + ICDGroupId;
            MDVisionService.defaultService(data, "ADMIN_CCMICDGROUPS", "DELETE_CCMICDGROUPS").done(
                function (resp) {
                    if (resp.status == true) {
                        Admin_CCMICDGroups.loadICDGroups();
                    }
                    else if (resp.status == false) {
                        utility.DisplayMessages(resp.Message, 2);
                    }
                });
        }, function () { });

    },

    CCMICDGroupsActiveInactive: function (ICDGroupId, isactive) {
        utility.myConfirm("3", function () {
            var data = "ICDGroupId=" + ICDGroupId + "&isactive=" + isactive;;
            MDVisionService.defaultService(data, "ADMIN_CCMICDGROUPS", "ACTIVEINACTIVE_CCMICDGROUPS").done(function () {
                Admin_CCMICDGroups.loadICDGroups();
            });
        }, function () { },
                    "3", null, null, null, isactive
                );
    },

    changeSwitch: function (objThis) {
        var IsActive = $(objThis).attr('IsActive');
        if (IsActive == '1') {
            Admin_CCMICDGroups.loadCCMICDGroups(null, false, "").done(
               function (resp) {
                   Admin_CCMICDGroups.CCMICDGroupsGridLoad(resp);
               });
            $(objThis).attr('IsActive', '0');
        }
        else if (IsActive == '0') {
            Admin_CCMICDGroups.loadCCMICDGroups(null, true, "").done(
           function (resp) {
               Admin_CCMICDGroups.CCMICDGroupsGridLoad(resp);
           });
            $(objThis).attr('IsActive', '1');
        }
    },

    CCMICDGroupsAdd: function () {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Facility", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            var params = [];
            params["CCMICDGroupID"] = null;
            params["mode"] = "Add";
            params["FromAdmin"] = Admin_CCMICDGroups.params["FromAdmin"];
            if (Admin_CCMICDGroups.params["FromAdmin"] == "0") {
                params["ParentCtrl"] = 'Admin_CCMICDGroups';
            }
            LoadActionPan('Admin_CCMICDGroups_Detail', params);
        }
    },

    CCMICDGroupsEdit: function (ICDGroupId) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Facility", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            var params = [];
            params["CCMICDGroupID"] = ICDGroupId;
            params["mode"] = "Edit";
            params["FromAdmin"] = Admin_CCMICDGroups.params["FromAdmin"];
            if (Admin_CCMICDGroups.params["FromAdmin"] == "0") {
                params["ParentCtrl"] = 'Admin_CCMICDGroups';
            }
            LoadActionPan('Admin_CCMICDGroups_Detail', params);
        }
    },

    searchCCMICDGroup: function () {
        var name = $('#CCMICDGroups #txtName').val();
        var switcher = $('#CCMICDGroups #switchActive');
        if ($(switcher).attr('IsActive') == '0') {
            Admin_CCMICDGroups.loadCCMICDGroups(null, false, name).done(
              function (resp) {
                  Admin_CCMICDGroups.CCMICDGroupsGridLoad(resp);
              });
        }
        else {
            Admin_CCMICDGroups.loadCCMICDGroups(null, true, name).done(
             function (resp) {
                 Admin_CCMICDGroups.CCMICDGroupsGridLoad(resp);
             });
        }
    }
}
