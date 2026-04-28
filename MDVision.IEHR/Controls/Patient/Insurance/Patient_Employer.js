Patient_Employer = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Patient_Employer.params = params;
        if (Patient_Employer.bIsFirstLoad) {
            Patient_Employer.bIsFirstLoad = false;
            Patient_Employer.EmployerSearch();
        }
    },

    EmployerAdd: function (event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Employer", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["EmployerId"] = null;
                params["mode"] = "Add";
                    params["ParentCtrl"] = 'Patient_Employer';
                LoadActionPan('employerDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EmployerEdit: function (EmployerId, ParentCtrl,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
	}
	utility.SelectGridRow($("#gvEmployer_row" + EmployerId));
        AppPrivileges.GetFormPrivileges("Employer", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = EmployerId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["EmployerId"] = selectedValue;
                    params["mode"] = "Edit";
                    if (ParentCtrl != null) {
                        params["ParentCtrl"] = ParentCtrl;
                    }
                    else
                        params["ParentCtrl"] = 'Patient_Employer';
                    LoadActionPan('employerDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EmployerDelete: function (EmployerId,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
	}
	utility.SelectGridRow($("#gvEmployer_row" + EmployerId));
        AppPrivileges.GetFormPrivileges("Employer", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = EmployerId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Employer.DeleteEmployer(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvEmployer').DataTable();
                                table1.row('.active').remove().draw(false);
                                CacheManager.BindCodes('GetEmployer', true);
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

    EmployerActiveInactive: function (EmployerId, IsActive,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Employer", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = EmployerId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        employerDetail.UpdateEmployerActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Patient_Employer.EmployerSearch('0');
                                CacheManager.BindCodes('GetEmployer', true);
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

    EmployerSearch: function (EmployerId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Employer", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlEmployer #pnlEmployer_Result").css("display") == "none") {
                    $("#pnlEmployer #pnlEmployer_Result").show();
                }

                var self = $("#pnlEmployer_Search");
                var myJSON = self.getMyJSON();

                Patient_Employer.SearchEmployer(myJSON, EmployerId).done(function (response) {
                    if (response.status != false) {
                        Patient_Employer.EmployerGridLoad(response);
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

    EmployerGridLoad: function (response) {
        $("#dgvEmployer").dataTable().fnDestroy();
        $("#pnlEmployer_Result #dgvEmployer tbody").find("tr").remove();
        if (response.EmployerCount > 0) {
            var EmployerLoadJSONData = JSON.parse(response.EmployerLoad_JSON);
            $.each(EmployerLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                
                $row.attr("id", "gvEmployer_row" + item.EmployerId);
                $row.attr("EmployerId", item.EmployerId);

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
                var selectEmployer = "";
                var selectMethod = "";
                if (Patient_Employer.params["FromAdmin"] == "0" && item.IsActive == "True") {
                    selectMethod = "Patient_Insurance.FillEmployerName('" + item.EmployerId + "','" + item.EmployerName + "',event);"
                    selectEmployer = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Patient_Employer.EmployerEdit('" + item.EmployerId + "',null,event);");
                }

                $row.append('<td style="display:none;">' + item.EmployerId + '</td><td><a class="btn btn-xs" href="#" onclick="Patient_Employer.EmployerDelete(\'' + item.EmployerId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Employer.EmployerEdit(\'' + item.EmployerId + '\',null,event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Employer.EmployerActiveInactive(\'' + item.EmployerId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectEmployer + '</td><td>' + item.EmployerName + '</td><td>' + item.Address1 + '</td><td>' + item.City + '</td><td>' + item.State + '</td><td>' + item.ZipCode + '</td><td>'  + item.ZipExt + '</td><td>' + item.PhoneNo + '</td>');

                $("#pnlEmployer_Result #dgvEmployer tbody").last().append($row);
            });
        }
        else {
            $('#pnlEmployer_Result #dgvEmployer').DataTable({
                "language": {
                    "emptyTable": "No Employer Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlEmployer_Result #dgvEmployer'))
            ;
        else {
            $("#pnlEmployer_Result #dgvEmployer").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
        }
    },

    SearchEmployer: function (EmployerData, EmployerId) {
        var data = "EmployerData=" + EmployerData + "&EmployerID=" + EmployerId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_EMPLOYER", "SEARCH_EMPLOYER");
    },

    DeleteEmployer: function (EmployerId) {
        var data = "EmployerID=" + EmployerId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_EMPLOYER_DETAIL", "DELETE_EMPLOYER");
    },

    //UnLoad: function (Tab) {
    //    UnloadActionPan(null, 'Patient_Employer');
    //},

    UnLoad: function () {

        if (Patient_Employer.params != null && Patient_Employer.params.ParentCtrl != null) {
            UnloadActionPan(Patient_Employer.params.ParentCtrl, 'Patient_Employer');
            if (Patient_Employer.params.RefCtrl != null)
                $("#" + Patient_Employer.params.RefCtrl).removeClass('disableAll');
        }
        else {
            UnloadActionPan(null, 'Patient_Employer');
            if (Patient_Employer.params.RefCtrl != null)
                $("#" + Patient_Employer.params.RefCtrl).removeClass('disableAll');
        }            
    },
}