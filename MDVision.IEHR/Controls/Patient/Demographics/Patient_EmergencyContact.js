Patient_EmergencyContact = {
    params: [],
    bIsFirstLoad: true,

    Load: function (params) {
        if (Patient_EmergencyContact.bIsFirstLoad) {
            Patient_EmergencyContact.bIsFirstLoad = false;
        }
        Patient_EmergencyContact.params = params
        Patient_EmergencyContact.LoadEmergencyContacts();
    },

    EmergencyContractAdd: function (event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Emergency Contact", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

            if (strMessage == "") {
                var params = [];
                params["ContactId"] = null;
                params["PatientID"] = Patient_EmergencyContact.params.patientID;
                params["mode"] = "Add";
                params["ParentCtrl"] = 'Patient_EmergencyContact';
                LoadActionPan('emergencyContactDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EmergencyContactEdit: function (PatientId, ContactId, IsPrimary, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvEmergencyContacts_row' + ContactId));
        AppPrivileges.GetFormPrivileges("Emergency Contact", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ContactId"] = ContactId;
                params["PatientID"] = PatientId;
                params["mode"] = "Edit";
                params["ParentCtrl"] = 'Patient_EmergencyContact';
                params["IsPrimary"] = IsPrimary;
                LoadActionPan('emergencyContactDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LoadEmergencyContacts: function (PrimaryID, PageNumber, ResultPerPage) {
        AppPrivileges.GetFormPrivileges("Emergency Contact", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlEmergencyContact #pnlEmergencyContacts_Result").css("display") == "none") {
                    $("#pnlEmergencyContact #pnlEmergencyContacts_Result").show();
                }
                Patient_EmergencyContact.EmergencyContactsLoad(PrimaryID, PageNumber, ResultPerPage).done(function (response) {
                    if (response.status != false) {
                        Patient_EmergencyContact.params["EmergencyContactsCount"] = response.EmergencyContactsCount;
                        Patient_EmergencyContact.EmergencyContactGridLoad(response);
                        var TableControl = $("#pnlEmergencyContacts_Result #dgvEmergencyContacts");
                        var PagingPanelControlID = Patient_EmergencyContact.params.PanelID + " #dgvEmergencyContacts_Paging";
                        var ClassControlName = "Patient_EmergencyContact";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.EmergencyContactsCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Patient_EmergencyContact.LoadEmergencyContacts(PrimaryID, PageNumber, ResultPerPage);
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

    EmergencyContactGridLoad: function (response) {
        $("#pnlEmergencyContacts_Result #dgvEmergencyContacts").dataTable().fnDestroy();
        $("#pnlEmergencyContacts_Result #dgvEmergencyContacts tbody").find("tr").remove();
        var primaryRecordId;
        if (response.EmergencyContactsCount > 0) {
            var EmergencyContactsLoadJSONData = JSON.parse(response.EmergencyContactsLoad_JSON);
            $.each(EmergencyContactsLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Patient_EmergencyContact.EmergencyContactEdit('" + item.PatientId + "','" + item.ContactId + "','" + item.IsPrimary + "',event);");
                $row.attr("id", "gvEmergencyContacts_row" + item.ContactId);
                $row.attr("ContactId", item.ContactId);

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

                if (item.IsPrimary == "True") {
                    isprimary = 0;
                    primarytglclass = "fa fa-dot-circle-o green";
                    primaryRecordId = item.ContactId;
                    primaryContactTitle = "Primary Contact";
                }
                else {
                    isprimary = 1;
                    primarytglclass = "fa fa-dot-circle-o";
                    primaryContactTitle = "";
                }

                $row.append('<td style="display:none;">' + item.ContactId + '</td><td><a class="btn  btn-xs" href="#" onclick="Patient_EmergencyContact.DeleteEmergencyContact(\'' + item.ContactId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_EmergencyContact.EmergencyContactEdit(\'' + item.PatientId + '\',\'' + item.ContactId + '\',\'' + item.IsPrimary + '\',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_EmergencyContact.ActiveInactiveEmergencyContact(\'' + item.ContactId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.LastName + ", " + item.FirstName + " " + item.MI + '</td><td>' + item.Address1 + '</td><td>' + item.HomePhone + '</td><td>' + item.CellNo + '</td><td><a class="btn  btn-xs" href="#" onclick="Patient_EmergencyContact.IsPrimaryEmergencyContact(\'' + item.ContactId + '\', ' + isprimary + ',event, \'' + item.HomePhone + '\', \'' + item.CellNo + '\');" title="' + primaryContactTitle + '"><i class="' + primarytglclass + '"></i></a></td>');


                $("#pnlEmergencyContacts_Result #dgvEmergencyContacts tbody").last().append($row);
            });
        }
        else {
            utility.ClearFormValidation('#frmEmergencyContact', true);
            Patient_EmergencyContact.params["mode"] = "Add";
            //Patient_EmergencyContact.ValidateEmergencyContact();
            $('#pnlEmergencyContacts_Result #dgvEmergencyContacts').DataTable({
               
                "language": {
                    "emptyTable": "No Emergency Contact Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlEmergencyContacts_Result #dgvEmergencyContacts'))
            ;
        else
            $("#pnlEmergencyContacts_Result #dgvEmergencyContacts").DataTable({"bInfo": false, "bLengthChange": false, "autoWidth": false, "bPaginate": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //if (Patient_EmergencyContact.params.EmergencyContactId == "-1") {
        //    Patient_EmergencyContact.FillEmergencyContact(primaryRecordId);
        //    Patient_EmergencyContact.ValidateEmergencyContact();
        //}

    },

    ValidateEmergencyContact: function () {
        $('#frmEmergencyContact')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  LastName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  FirstName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Patient_EmergencyContact.SaveEmergencyContact();
       });
    },

    DeleteEmergencyContact: function (ContactId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvEmergencyContacts_row' + ContactId));
        AppPrivileges.GetFormPrivileges("Emergency Contact", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ContactId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_EmergencyContact.EmergencyContactDelete(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#pnlEmergencyContacts_Result #dgvEmergencyContacts').DataTable();
                                table1.row('.active').remove().draw(false);
                                Patient_EmergencyContact.UpdateCareGiversDropDown();
                                Patient_EmergencyContact.LoadEmergencyContacts();
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

    ActiveInactiveEmergencyContact: function (ContactId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Emergency Contact", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ContactId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_EmergencyContact.EmergencyContactUpdateActiveInactive(ContactId, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Patient_EmergencyContact.UpdateCareGiversDropDown();
                                Patient_EmergencyContact.LoadEmergencyContacts();
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

    IsPrimaryEmergencyContact: function (ContactId, IsPrimary,event, HomePhone, CellNo) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Emergency Contact", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (HomePhone != "" || CellNo != "") {
                    var diaLogTextType = "4";
                    if (IsPrimary == 0) {
                        diaLogTextType = "11";
                    }
                    utility.myConfirm(diaLogTextType, function () {
                        var selectedValue = ContactId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Patient_EmergencyContact.EmergencyContactUpdateIsPrimary(ContactId, IsPrimary).done(function (response) {
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                    Patient_EmergencyContact.LoadEmergencyContacts();
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () { },
                            diaLogTextType
                        );
                }
                else {
                    utility.DisplayMessages("No Contact number found. Contact could not be set as Primary Emergency Conatct.", 3);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EmergencyContactsLoad: function (PrimaryId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "PatientID=" + Patient_EmergencyContact.params.patientID + "&pageNumber=" + PageNumber + "&rowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_EMERGENCYCONTACT", "LOAD_EMERGENCYCONTACTS");
    },

    EmergencyContactUpdateActiveInactive: function (EmergencyContactID, IsActive) {
        var data = "PatientID=" + Patient_EmergencyContact.params.patientID + "&EmergencyContactID=" + EmergencyContactID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_EMERGENCYCONTACT", "UPDATE_EMERGENCYCONTACT_ACTIVE_INACTIVE");
    },

    EmergencyContactUpdateIsPrimary: function (EmergencyContactID, IsPrimary) {
        var data = "PatientID=" + Patient_EmergencyContact.params.patientID + "&EmergencyContactID=" + EmergencyContactID + "&IsPrimary=" + IsPrimary;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_EMERGENCYCONTACT", "UPDATE_EMERGENCYCONTACT_IS_PRIMARY");
    },

    EmergencyContactDelete: function (EmergencyContactID) {
        var data = "EmergencyContactID=" + EmergencyContactID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_EMERGENCYCONTACT", "DELETE_EMERGENCYCONTACT");
    },

    UnLoad: function (Tab) {

        if (Patient_EmergencyContact.params != null && Patient_EmergencyContact.params.ParentCtrl != null) {
            UnloadActionPan(Patient_EmergencyContact.params.ParentCtrl, 'Patient_EmergencyContact');
        }
        else
            UnloadActionPan(null, 'Patient_EmergencyContact');


    },

    UpdateCareGiversDropDown: function () {
        $.when(Patient_Demographic.LoadCareGiverDropDowns('pnlDemographic').then(function () {
            if (Patient_Demographic.careGiverIds) {
                $('#' + Patient_Demographic.params.PanelID + " #ddlCareGiver").val(Patient_Demographic.careGiverIds.split(','));
            }
            Patient_Demographic.IntializeMultiSelectDropDownCareGiver();
        }));
    },
}