Patient_Lawyer = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Patient_Lawyer.params = params;
        if (Patient_Lawyer.bIsFirstLoad) {
            Patient_Lawyer.bIsFirstLoad = false;
            Patient_Lawyer.LawyerSearch();
        }
    },

    LawyerAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Lawyer", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["LawyerId"] = null;
                params["mode"] = "Add";
                    params["ParentCtrl"] = 'Patient_Lawyer';
                LoadActionPan('lawyerDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LawyerEdit: function (LawyerId, ParentCtrl,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
	}
	utility.SelectGridRow($("#gvLawyer_row" + LawyerId));
        AppPrivileges.GetFormPrivileges("Lawyer", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = LawyerId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["LawyerId"] = selectedValue;
                    params["mode"] = "Edit";
                    if (ParentCtrl != null) {
                        params["ParentCtrl"] = ParentCtrl;
                    }
                    else
                        params["ParentCtrl"] = 'Patient_Lawyer';
                    LoadActionPan('lawyerDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LawyerDelete: function (LawyerId,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
	}
	utility.SelectGridRow($("#gvLawyer_row" + LawyerId));
        AppPrivileges.GetFormPrivileges("Lawyer", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LawyerId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Lawyer.DeleteLawyer(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvLawyer').DataTable();
                                table1.row('.active').remove().draw(false);
                                CacheManager.BindCodes('GetLawyer', true);
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

    LawyerActiveInactive: function (LawyerId, IsActive,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Lawyer", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = LawyerId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        lawyerDetail.UpdateLawyerActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Patient_Lawyer.LawyerSearch('0');
                                CacheManager.BindCodes('GetLawyer', true);
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

    LawyerSearch: function (LawyerId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Lawyer", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlLawyer #pnlLawyer_Result").css("display") == "none") {
                    $("#pnlLawyer #pnlLawyer_Result").show();
                }

                var self = $("#pnlLawyer_Search");
                var myJSON = self.getMyJSON();

                Patient_Lawyer.SearchLawyer(myJSON, LawyerId).done(function (response) {
                    if (response.status != false) {
                        Patient_Lawyer.LawyerGridLoad(response);
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

    LawyerGridLoad: function (response) {
        $("#dgvLawyer").dataTable().fnDestroy();
        $("#pnlLawyer_Result #dgvLawyer tbody").find("tr").remove();
        if (response.LawyerCount > 0) {
            var LawyerLoadJSONData = JSON.parse(response.LawyerLoad_JSON);
            $.each(LawyerLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvLawyer_row" + item.LawyerId);
                $row.attr("LawyerId", item.LawyerId);

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
                var selectLawyer = "";
                var selectMethod = "";
                if (Patient_Lawyer.params["FromAdmin"] == "0" && item.IsActive == "True") {
                    selectMethod = "Patient_Insurance.FillLawyerName('" + item.LawyerId + "','" + item.LawyerName + "');"
                    selectLawyer = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Patient_Lawyer.LawyerEdit('" + item.LawyerId + "',null,event);");
                }

                $row.append('<td style="display:none;">' + item.LawyerId + '</td><td><a class="btn btn-xs" href="#" onclick="Patient_Lawyer.LawyerDelete(\'' + item.LawyerId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Lawyer.LawyerEdit(\'' + item.LawyerId + '\',null,event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Lawyer.LawyerActiveInactive(\'' + item.LawyerId + '\', ' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a>' + selectLawyer + '</td><td>' + item.LawyerName + '</td><td>' + item.FirmName + '</td><td>' + item.LicenseNo + '</td><td>' + item.ContactNo + '</td><td>' + item.City + '</td><td>' + item.State + '</td><td>' + item.ZipCode + '</td>');

                $("#pnlLawyer_Result #dgvLawyer tbody").last().append($row);
            });
        }
        else {
            $('#pnlLawyer_Result #dgvLawyer').DataTable({
                "language": {
                    "emptyTable": "No Lawyer Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlLawyer_Result #dgvLawyer'))
            ;
        else {
            $("#pnlLawyer_Result #dgvLawyer").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });

        }
    },

    SearchLawyer: function (LawyerData, LawyerId) {
        var data = "LawyerData=" + LawyerData + "&LawyerID=" + LawyerId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_LAWYER", "SEARCH_LAWYER");
    },

    DeleteLawyer: function (LawyerId) {
        var data = "LawyerID=" + LawyerId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_LAWYER_DETAIL", "DELETE_LAWYER");
    },

    //UnLoad: function (Tab) {
    //    UnloadActionPan(null, 'Patient_Lawyer');
    //},

    UnLoad: function () {

        if (Patient_Lawyer.params != null && Patient_Lawyer.params.ParentCtrl != null) {
            UnloadActionPan(Patient_Lawyer.params.ParentCtrl, 'Patient_Lawyer');
            if (Patient_Lawyer.params.RefCtrl != null)
                $("#" + Patient_Lawyer.params.RefCtrl).removeClass('disableAll');
        }
        else {
            UnloadActionPan(null, 'Patient_Lawyer');
            if (Patient_Lawyer.params.RefCtrl != null)
                $("#" + Patient_Lawyer.params.RefCtrl).removeClass('disableAll');
        }            
    },

}