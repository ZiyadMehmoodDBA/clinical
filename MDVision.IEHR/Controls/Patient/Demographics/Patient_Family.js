Patient_Family = {
    params: [],
    bIsFirstLoad: true,

    Load: function (params) {
        Patient_Family.params = params;
        if (Patient_Family.bIsFirstLoad) {
            Patient_Family.bIsFirstLoad = false;
            var self = $('#pnlFamily');
            self.loadDropDowns(true);
        }
        Patient_Family.LoadFamilies();
    },

    FamilyDetailAdd: function (event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Patient Family", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FamilyId"] = null;
                params["PatientID"] = Patient_Family.params.patientID;
                params["mode"] = "Add";
                params["ParentCtrl"] = 'Patient_Family';
                LoadActionPan('Patient_Family_Detail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    FamilyDetailEdit: function (FamilyId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvFamily_row' + FamilyId));
        AppPrivileges.GetFormPrivileges("Patient Family", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FamilyId"] = FamilyId;
                params["PatientID"] = Patient_Family.params.patientID;
                params["mode"] = "Edit";
                params["ParentCtrl"] = 'Patient_Family';
                LoadActionPan('Patient_Family_Detail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LoadFamilies: function (PrimaryID, PageNumber, ResultPerPage) {
        AppPrivileges.GetFormPrivileges("Patient Family", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlFamily #pnlFamily_Result").css("display") == "none") {
                    $("#pnlFamily #pnlFamily_Result").show();
                }

                var self = $("#pnlFamily #pnl_Search");
                var myJSON = self.getMyJSON();
                Patient_Family.FamilyLoad(myJSON, PrimaryID, PageNumber, ResultPerPage).done(function (response) {
                    if (response.status != false) {
                        Patient_Family.params["PatientFamilyCount"] = response.PatientFamilyCount;
                        Patient_Family.FamilyGridLoad(response);
                        var TableControl = $("#pnlFamily_Result #dgvFamily");

                        var PagingPanelControlID = Patient_Family.params.PanelID + " #dgvPatientFamily_Paging";
                        var ClassControlName = "Patient_Family";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.PatientFamilyCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Patient_Family.LoadFamilies(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);
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

    FamilyGridLoad: function (response) {
        $("#pnlFamily_Result #dgvFamily").dataTable().fnDestroy();
        $("#pnlFamily_Result #dgvFamily tbody").find("tr").remove();
        var firstFamilyId = "";
        if (response.PatientFamilyCount > 0) {
            var FamilyLoadJSONData = JSON.parse(response.PatientFamilyLoad_JSON);
            $.each(FamilyLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Patient_Family.FamilyDetailEdit('" + item.FamilyId + "',event);");
                $row.attr("id", "gvFamily_row" + item.FamilyId);
                $row.attr("FamilyId", item.FamilyId);
                //To Load First Family in Edit Mode
                if (firstFamilyId == "") {
                    firstFamilyId = item.FamilyId;
                }
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
                if (Patient_Family.params.ParentCtrl == 'Patient_AccountManager') {
                    var selectMethod = "Patient_AccountManager.FillFamilyInfoFromSearch(" + item.FamilyId + ",'" + item.AccountNumber + "',\"" + item.FirstName + "\",\"" + item.LastName + "\",event);"
                    selectFacility = '&nbsp;<a class="btn btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", selectMethod);
                    $row.append('<td style="display:none;">' + item.FamilyId + '</td><td>' + selectFacility + '</td><td>' + item.LastName + ', ' + item.FirstName + " " + item.MI + '</td><td>' + item.RelationShipName + '</td><td>' + item.Address1 + '</td><td>' + item.DOB.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.HomePhoneNo + '</td><td>' + item.AccountNumber + '</td>');
                } else {
                    //Begin Jan 6th, 2015, Author: Abdur Rehman Latif, PMS-3252 Change: last name and First name
                    var editMethod = "Patient_Family.FamilyDetailEdit(\'" + item.FamilyId + "'\,event)";;
                    $row.attr('onclick', editMethod);
                    $row.append('<td style="display:none;">' + item.FamilyId + '</td><td><a class="btn  btn-xs" href="#" onclick="Patient_Family.DeleteFamily(\'' + item.FamilyId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_Family.ActiveInactiveFamily(\'' + item.FamilyId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.LastName + ', ' + item.FirstName + " " + item.MI + '</td><td>' + item.RelationShipName + '</td><td>' + item.Address1 + '</td><td>' + item.DOB.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.HomePhoneNo + '</td><td>' + item.AccountNumber + '</td>');
                    //End Jan 6th, 2015, Author: Abdur Rehman Latif, PMS-3252 Change: last name and First name
                }
                $("#pnlFamily_Result #dgvFamily tbody").last().append($row);
            });
        }
        else {
            $('#pnlFamily_Result #dgvFamily').DataTable({
                "language": {
                    "emptyTable": "No Family Found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }]
            });
            utility.ClearFormValidation('#frmFamily', true);
            Patient_Family.params["mode"] = "Add";
        }
        if ($.fn.dataTable.isDataTable('#pnlFamily_Result #dgvFamily'))
            ;
        else
            $("#pnlFamily_Result #dgvFamily").DataTable({ "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "bSort": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    DeleteFamily: function (FamilyId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvFamily_row' + FamilyId));
        AppPrivileges.GetFormPrivileges("Patient Family", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = FamilyId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Family.FamilyDelete(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#pnlFamily_Result #dgvFamily').DataTable();
                                table1.row('.active').remove().draw(false);
                                Patient_Family.LoadFamilies();
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

    ActiveInactiveFamily: function (FamilyId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Patient Family", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = FamilyId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Family.FamilyUpdateActiveInactive(FamilyId, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Patient_Family.LoadFamilies();
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

    FamilyUpdateActiveInactive: function (FamilyID, IsActive) {
        var data = "PatientID=" + Patient_Family.params.patientID + "&FamilyID=" + FamilyID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_FAMILY", "UPDATE_FAMILY_ACTIVE_INACTIVE");
    },

    FamilyUpdateIsPrimary: function (FamilyID, IsPrimary) {
        var data = "PatientID=" + Patient_Family.params.patientID + "&FamilyID=" + FamilyID + "&IsPrimary=" + IsPrimary;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_FAMILY", "UPDATE_Family_IS_PRIMARY");
    },

    FamilyDelete: function (FamilyID) {
        var data = "FamilyID=" + FamilyID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_FAMILY", "DELETE_FAMILY");
    },

    FamilyLoad: function (FamilyData, PrimaryId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "";
        if ($("#pnlFamily #hfPatientRepresentativeId").val())
            data = "FamilyData=" + FamilyData + "&PatientRepresentativeId=" + $("#pnlFamily #hfPatientRepresentativeId").val() + "&pageNumber=" + PageNumber + "&rowsPerPage=" + RowsPerPage;
        else
            data = "FamilyData=" + FamilyData + "&PatientID=" + Patient_Family.params.patientID + "&pageNumber=" + PageNumber + "&rowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_FAMILY", "LOAD_PATIENTFAMILY");
    },

    UnLoad: function (Tab) {
        if (Patient_Family.params.FromAdmin != "1") {
            if (Patient_Family.params != null && Patient_Family.params.ParentCtrl != null) {
                UnloadActionPan(Patient_Family.params.ParentCtrl, 'Patient_Family');
            }
            else
                UnloadActionPan(null, 'Patient_Family');
        }
        else
            RemoveAdminTab(Tab);
    },
    BindFamilyLastName: function () {
        var LastName = $("#pnlFamily #txtLastName").val();
        utility.Keyupdelay(function () {
            Patient_Family.GetFamilyArray("", LastName).done(function (response) {
                $("#pnlFamily #txtLastName").autocomplete({
                    autoFocus: true,
                    source: response,
                    select: function (event, ui) {
                        setTimeout(function () {
                            $("#pnlFamily #hfPatientRepresentativeId").val(ui.item.id);
                            //$("#pnlFamily #txtFirstName").val(ui.item.FullName.split(', ')[1]);
                            $("#pnlFamily #txtLastName").val(ui.item.FullName);
                        }, 100);
                    }
                }).blur(function () {
                    setTimeout(function () {
                        utility.ValidateAutoComplete($("#pnlFamily #txtLastName"), "pnlFamily #hfPatientRepresentativeId", false, 1, null, null);
                    }, 200);
                });

                $("#pnlFamily #txtLastName").autocomplete("search");
            });
        });
    },
    BindFamilyFirstName: function () {
        var FirstName = $("#pnlFamily #txtFirstName").val();
        utility.Keyupdelay(function () {
            Patient_Family.GetFamilyArray(FirstName, "").done(function (response) {
                $("#pnlFamily #txtFirstName").autocomplete({
                    autoFocus: true,
                    source: response,
                    select: function (event, ui) {
                        setTimeout(function () {
                            $("#pnlFamily #hfPatientRepresentativeId").val(ui.item.id);
                            $("#pnlFamily #txtFirstName").val(ui.item.FullName);
                            //$("#pnlFamily #txtLastName").val(ui.item.FullName.split(',')[0]);
                        }, 100);
                    }
                }).blur(function () {
                    setTimeout(function () {
                        utility.ValidateAutoComplete($("#pnlFamily #txtFirstName"), "pnlFamily #hfPatientRepresentativeId", false, 1, null, null);
                    }, 200);
                });
                $("#pnlFamily #txtFirstName").autocomplete("search");
            });
        });
    },
    GetFamilyArray: function (FirstName, LastName) {
        var familyList = [];
        var dfd = new $.Deferred();
        Patient_Family.LoadFamilyArray_DBCall(FirstName, LastName).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.FamilyCount > 0) {
                    var PatientLoadJSONData = JSON.parse(responseData.FamilyLoad_JSON);
                    $.each(PatientLoadJSONData, function (i, item) {
                        familyList.push({ id: item.RepresentativeId, value: item.FullName, AccountNumber: item.AccountNumber, FullName: item.FullName });
                    });
                }
            }
            dfd.resolve(familyList);
        });

        return dfd.promise();
    },
    LoadFamilyArray_DBCall: function (FirstName, LastName) {
        var data =  "FirstName=" + FirstName + "&LastName=" + LastName;
        return MDVisionService.defaultService(data, "PATIENT_FAMILY", "LOOKUP_PATIENT_FAMILY");
    },
}