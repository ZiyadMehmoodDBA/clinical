Patient_School = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Patient_School.params = params;
        if (Patient_School.bIsFirstLoad) {
            Patient_School.bIsFirstLoad = false;
        }
        Patient_School.SchoolSearch();
    },

    SchoolAdd: function (event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("School", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["SchoolId"] = null;
                params["mode"] = "Add";
                params["ParentCtrl"] = 'Patient_School';
                LoadActionPan('schoolDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SchoolEdit: function (SchoolId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvSchool_row' + SchoolId));
        AppPrivileges.GetFormPrivileges("School", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = SchoolId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["SchoolId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["ParentCtrl"] = 'Patient_School';
                    LoadActionPan('schoolDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SchoolDelete: function (SchoolId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvSchool_row' + SchoolId));
        AppPrivileges.GetFormPrivileges("School", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = SchoolId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_School.DeleteSchool(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvSchool').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SchoolActiveInactive: function (SchoolId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("School", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = SchoolId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        schoolDetail.UpdateSchoolActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Patient_School.SchoolSearch('0');
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '3', null, null, null, IsActive
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SchoolSearch: function (SchoolId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("School", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlSchool #pnlSchool_Result").css("display") == "none") {
                    $("#pnlSchool #pnlSchool_Result").show();
                }

                var self = $("#pnlSchool_Search");
                var myJSON = self.getMyJSON();

                Patient_School.SearchSchool(myJSON, SchoolId).done(function (response) {
                    if (response.status != false) {
                        Patient_School.SchoolGridLoad(response);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SchoolGridLoad: function (response) {
        $("#dgvSchool").dataTable().fnDestroy();
        $("#pnlSchool_Result #dgvSchool tbody").find("tr").remove();
        if (response.SchoolCount > 0) {
            var SchoolLoadJSONData = JSON.parse(response.SchoolLoad_JSON);
            $.each(SchoolLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvSchool_row" + item.SchoolId);
                $row.attr("SchoolId", item.SchoolId);

                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var selectSchool = "";
                var selectMethod = "";
                if (Patient_School.params["FromAdmin"] == "0") {
                    selectMethod = "Patient_Preferences.FillSchoolName('" + item.SchoolId + "','" + item.SchoolName + "');"
                    selectSchool = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Patient_School.SchoolEdit('" + item.SchoolId + "',event);");
                }

                $row.append('<td style="display:none;">' + item.SchoolId + '</td><td><a class="btn btn-xs" href="#" onclick="Patient_School.SchoolDelete(\'' + item.SchoolId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_School.SchoolEdit(\'' + item.SchoolId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_School.SchoolActiveInactive(\'' + item.SchoolId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectSchool + '</td><td>' + item.SchoolName + '</td><td>' + item.Address1 + '</td><td>' + item.City + '</td><td>' + item.State + '</td><td>' + item.ZipCode + '</td><td>' + item.PhoneNo + '</td>');

                $("#pnlSchool_Result #dgvSchool tbody").last().append($row);
            });
        }
        else {
            $('#pnlSchool_Result #dgvSchool').DataTable({
                "language": {
                    "emptyTable": "No School Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlSchool_Result #dgvSchool'))
            ;
        else {
            $("#pnlSchool_Result #dgvSchool").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
        }
    },

    SearchSchool: function (SchoolData, SchoolId) {
        var data = "SchoolData=" + SchoolData + "&SchoolID=" + SchoolId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_SCHOOL", "SEARCH_SCHOOL");
    },

    DeleteSchool: function (SchoolId) {
        var data = "SchoolID=" + SchoolId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_SCHOOL_DETAIL", "DELETE_SCHOOL");
    },

    UnLoad: function () {
        UnloadActionPan(Patient_School.params["ParentCtrl"], 'Patient_School');

    },
    //UnLoad: function () {

    //        UnloadActionPan(Patient_Preferences.params["ParentCtrl"]);

    //},

}