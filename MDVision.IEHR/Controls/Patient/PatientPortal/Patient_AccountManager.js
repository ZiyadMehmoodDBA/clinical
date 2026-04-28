Patient_AccountManager = {
    params: null,
    EditableGrid: null,
    isFirstEditableGridLoad: false,
    deletedrowID: 0,
    PatientPortalURL: "",
    Load: function (params) {
        Patient_AccountManager.params = params;
        if (Patient_AccountManager.params.PanelID != 'pnlPatientAccountManager') {
            Patient_AccountManager.params.PanelID = Patient_AccountManager.params.PanelID + ' #pnlPatientAccountManager';
        } else {
            Patient_AccountManager.params.PanelID = 'pnlPatientAccountManager';
        }
        var PatientId = Patient_AccountManager.params.PatientId;

        $('#pnlPatientAccountManager #hfPatientId').val(PatientId);
        if (params.RefCtrl)
            params.FromAdmin = '0';
        else
            params.FromAdmin = '1';


        var firstInitial = Patient_AccountManager.params.PatientFirstName.charAt(0);
        var lastName = Patient_AccountManager.params.PatientLastName;
        var dateOfBirth = Patient_AccountManager.params.PatientDOB;
        var yearOfBirth = dateOfBirth.split('/')[2] ? dateOfBirth.split('/')[2] : dateOfBirth.split('-')[2];

        $('#' + Patient_AccountManager.params.PanelID + ' #txtUserName').val(firstInitial + lastName + yearOfBirth);
        $('#' + Patient_AccountManager.params.PanelID + ' #txtpassword').val('sovereignhealth#1');
        $('#' + Patient_AccountManager.params.PanelID + ' #txtAccountEmail').val(Patient_AccountManager.params.PatientEmail);

        if (Patient_AccountManager.params.PatientPortalStatus == '0') {
            $('#' + Patient_AccountManager.params.PanelID + ' #btnAddPatientRepresentative').attr('disabled', 'disabled');
            $('#' + Patient_AccountManager.params.PanelID + ' #lblNotEnableStatus').show();
        }

        else if (Patient_AccountManager.params.PatientPortalStatus == '1') {
            $('#' + Patient_AccountManager.params.PanelID + ' #lblEnableStatus').show();
            $('#' + Patient_AccountManager.params.PanelID + ' #btnAddPatientRepresentative').removeAttr('disabled', 'disabled');
        }


        if (Patient_AccountManager.params.mode == "Add") {

            $('#pnlPatientAccountManager #btnPrint').attr('disabled', 'disabled');
            if (Patient_AccountManager.params.ZipCode == "") {
                $('#pnlPatientAccountManager #txtAnswer').removeAttr('disabled');
            } else {
                $('#pnlPatientAccountManager #txtAnswer').val(Patient_AccountManager.params.ZipCode);
            }

        } else {
            $('#pnlPatientAccountManager #btnPrint').removeAttr('disabled');
            $('#pnlPatientAccountManager #btnResetPassword').removeAttr('disabled');
            $('#pnlPatientAccountManager #btnAddPatientRepresentative').removeAttr('disabled');

        }
        Patient_AccountManager.PatientRepresentativeSearch();


        var self = "";
        if (Patient_AccountManager.params["PanelID"].indexOf("pnlPatientAccountManager") == 0) {
            self = $("#" + Patient_AccountManager.params["PanelID"] + " #pnlPatientAccountManager");
        }
        else {
            self = $('#pnlPatientAccountManager');
        }
        self.loadDropDowns(true).done(function () {
            $('#' + Patient_AccountManager.params.PanelID + ' #frmPatAccountManager').data('serialize', $('#' + Patient_AccountManager.params.PanelID + ' #frmPatAccountManager').serialize());
            $('#pnlPatientAccountManager #ddlSecurityQuestion option:eq(1)').prop('selected', true);
            Patient_AccountManager.LoadPatientAccountManager(PatientId);

        });

        Patient_AccountManager.ValidateAccountManager();
        Patient_AccountManager.BindFamilyFirstName();
        Patient_AccountManager.BindFamilyLastName();
        Patient_AccountManager.BindFamilyAccount();
    },

    LoadPatientAccountManager: function (PatientId) {

        if (Patient_AccountManager.params.mode == "Add") {


        } else if (Patient_AccountManager.params.mode == "Edit") {

            Patient_AccountManager.FillPatientAccountManager(PatientId).done(function (response) {

                if (response.status != false) {

                    var PatientPortal_detail = JSON.parse(response.PatientPortalFill_JSON);
                    Patient_AccountManager.PatientPortalURL = response.PatientPortalURL;

                    var self = $('#' + Patient_AccountManager.params.PanelID + " #frmPatAccountManager");
                    utility.bindMyJSON(true, PatientPortal_detail, false, self).done(function () {
                        if (PatientPortal_detail.chkunlockAccount == 'True')
                            $("#frmPatAccountManager #unlockAccount").attr("checked", false);
                        else
                            $("#frmPatAccountManager #unlockAccount").attr("checked", true);
                        $('#' + Patient_AccountManager.params.PanelID + ' #frmPatAccountManager').data('serialize', $('#' + Patient_AccountManager.params.PanelID + ' #frmPatAccountManager').serialize());
                    });


                } else {

                }

            });
        }

    },

    FillPatientAccountManager: function (PatientId) {

        var data = "PatientId=" + PatientId;
        return MDVisionService.defaultService(data, "PATIENT_ACCOUNT_MANAGER", "FILL_PATIENT_ACCOUNT_MANAGER");

    },

    ValidateAccountManager: function () {
        $('#frmPatAccountManager')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   SecurityQuestion: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Answer: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   AccountEmail: {
                       group: '.col-lg-3',
                       validators: {
                           //emailAddress: {
                           //    message: 'Email not Valid'
                           //},
                           regexp: {
                               regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                               message: 'Email not Valid'
                           },
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
               //}).on('keyup', 'input#txtAccountEmail', function (e) {
               //    var formValidation = $("#pnlPatientAccountManager #frmPatAccountManager").data("bootstrapValidator");
               //    switch ($(this).attr("name")) {
               //        case 'AccountEmail':
               //            var OccurenceCod1Val = $("#pnlPatientAccountManager #frmPatAccountManager input#txtAccountEmail").val();
               //            if (OccurenceCod1Val != "") {
               //                //formValidation.enableFieldValidators('OccurrenceDate1', true);//.revalidateField('OccurrenceDate1');
               //                formValidation.enableFieldValidators('AccountEmail', true);
               //            }
               //            else
               //                formValidation.enableFieldValidators('AccountEmail', false);
               //            break;
               //        default:
               //            break;
               //    }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Patient_AccountManager.AccountManagerSave();
        });
    },

    AccountManagerSave: function () {

        var dfd = $.Deferred();

        $('#' + Patient_AccountManager.params.PanelID + " #hfPatientId").val(Patient_AccountManager.params["PatientId"]);
        // $('#pnlPatientAccountManager #hfPatientId').val(Patient_AccountManager.params["patientID"]);

        var strMessage = "";
        var self = $("#frmPatAccountManager");
        var myJSON = self.getMyJSON();
        if (Patient_AccountManager.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Patient Portal Account", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Patient_AccountManager.SaveAccountManager(myJSON).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Patient_AccountManager.params.mode = "Edit";
                            Patient_AccountManager.params.PatLoginId = response.PatientLoginID;
                            $('#pnlPatientAccountManager #btnPrint').removeAttr('disabled');
                            $('#pnlPatientAccountManager #btnResetPassword').removeAttr('disabled');
                            $('#pnlPatientAccountManager #btnAddPatientRepresentative').removeAttr('disabled');
                            Patient_Demographic.params.PatientPortalStatus = "1";
                            $('#' + Patient_AccountManager.params.PanelID + ' #lblNotEnableStatus').hide();
                            $('#' + Patient_AccountManager.params.PanelID + ' #lblEnableStatus').show();
                            $('#' + Patient_AccountManager.params.PanelID + ' #btnAddPatientRepresentative').removeAttr('disabled', 'disabled');
                            if (Patient_AccountManager.params.ParentCtrl == "patTabDemographic") {
                                Patient_Demographic.LoadPatientDemogrphic();
                            } else {
                                $('#pnldemographicDetail #txtEmail').val($('#' + Patient_AccountManager.params.PanelID + ' #frmPatAccountManager #txtAccountEmail').val());
                            }

                            Patient_AccountManager.PatientPortalURL = response.PatientPortalURL;
                            $('#pnlPatientAccountManager #txtpassword').val(response.password);

                            $('#' + Patient_AccountManager.params.PanelID + ' #frmPatAccountManager').data('serialize', $('#' + Patient_AccountManager.params.PanelID + ' #frmPatAccountManager').serialize());
                            dfd.resolve(true);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                            dfd.resolve(false);
                        }
                    });
                }
                else {
                    utility.DisplayMessages(strMessage, 2);
                    dfd.resolve(false);
                }
                   
            });
        }
        else if (Patient_AccountManager.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Patient Portal Account", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Patient_AccountManager.UpdateAccountManager(myJSON, Patient_AccountManager.params.PatLoginId).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            if (Patient_AccountManager.params.ParentCtrl == "patTabDemographic") {
                                Patient_Demographic.LoadPatientDemogrphic();
                            } else {
                                $('#pnldemographicDetail #txtEmail').val($('#' + Patient_AccountManager.params.PanelID + ' #frmPatAccountManager #txtAccountEmail').val());
                            }
                            $('#' + Patient_AccountManager.params.PanelID + ' #frmPatAccountManager').data('serialize', $('#' + Patient_AccountManager.params.PanelID + ' #frmPatAccountManager').serialize());
                            dfd.resolve(true);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                            dfd.resolve(false);
                        }
                    });
                }
                else {
                    utility.DisplayMessages(strMessage, 2);
                    dfd.resolve(true);
                }  
            });
        }
        else
            dfd.resolve(false);

        // to remove MU eAccess Alert.
        dfd.then(function (res) {

            if (res && $("#pnlPatientAccountManager #lblNotEnableStatus").html() == "Not Enable") {
                MU_Alerts.UpdateMUAlertProfile("eAccess", 0, Patient_AccountManager.params.PatientId);
            }
        });
    },

    SaveAccountManager: function (AccountManagerData) {
        var data = "AccountManagerData=" + AccountManagerData;
        return MDVisionService.defaultService(data, "PATIENT_ACCOUNT_MANAGER", "SAVE_ACCOUNT_MANAGER");
    },

    UpdateAccountManager: function (AccountManagerData, PatLoginId) {
        var data = "AccountManagerData=" + AccountManagerData + "&PatLoginId=" + PatLoginId;
        return MDVisionService.defaultService(data, "PATIENT_ACCOUNT_MANAGER", "UPDATE_ACCOUNT_MANAGER");
    },

    UnLoad: function () {
        utility.UnLoadDialog('frmPatAccountManager', function () {

            UnloadActionPan(Patient_AccountManager.params.ParentCtrl, 'Patient_AccountManager');

        }, function () {
            UnloadActionPan(Patient_AccountManager.params.ParentCtrl, 'Patient_AccountManager');
        });

    },

    OpenPatientSearch: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_AccountManager';
        LoadActionPan('Patient_Search', params);
    },

    OpenFamilySearch: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["FamilyId"] = "-1";
        params["ParentCtrl"] = 'Patient_AccountManager';
        params["patientID"] = Patient_AccountManager.params.PatientId;
        LoadActionPan('Patient_Family', params);
    },

    BindFamilyFirstName: function () {
        var Ctrl = $("#pnlPatientAccountManager #frmPatAccountManager #txtFirstName");
        var func = function () { return Patient_AccountManager.GetFamilyArray("", Ctrl.val(), "") };
        var hfCtrl = $("#pnlPatientAccountManager #frmPatAccountManager #hfPatientRepresentativeId");
        var onSelect = function (e) {
            utility.SetKendoAutoCompleteSourceforValidate($("#pnlPatientAccountManager #frmPatAccountManager #txtAccountNo"), e.AccountNumber, hfCtrl, e.id);
            utility.SetKendoAutoCompleteSourceforValidate($("#pnlPatientAccountManager #frmPatAccountManager #txtLastName"), e.FullName.split(',')[0], hfCtrl, e.id);
        }
        var onChange = function (valid) {
            if (!valid) {
                $("#pnlPatientAccountManager #frmPatAccountManager #txtAccountNo").val('');
                $("#pnlPatientAccountManager #frmPatAccountManager #txtLastName").val('');
            }
        }
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },

    BindFamilyLastName: function () {
        var Ctrl = $("#pnlPatientAccountManager #frmPatAccountManager #txtLastName");
        var func = function () { return Patient_AccountManager.GetFamilyArray("", "", Ctrl.val()) };
        var hfCtrl = $("#pnlPatientAccountManager #frmPatAccountManager #hfPatientRepresentativeId");
        var onSelect = function (e) {
            utility.SetKendoAutoCompleteSourceforValidate($("#pnlPatientAccountManager #frmPatAccountManager #txtAccountNo"), e.AccountNumber, hfCtrl, e.id);
            utility.SetKendoAutoCompleteSourceforValidate($("#pnlPatientAccountManager #frmPatAccountManager #txtFirstName"), e.FullName.split(', ')[1], hfCtrl, e.id);
        }
        var onChange = function (valid) {
            if (!valid) {
                $("#pnlPatientAccountManager #frmPatAccountManager #txtAccountNo").val('');
                $("#pnlPatientAccountManager #frmPatAccountManager #txtFirstName").val('');
            }
        }
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },

    BindFamilyAccount: function () {
        var Ctrl = $("#pnlPatientAccountManager #frmPatAccountManager #txtAccountNo");
        var func = function () { return Patient_AccountManager.GetFamilyArray(Ctrl.val(), "", "") };
        var hfCtrl = $("#pnlPatientAccountManager #frmPatAccountManager #hfPatientRepresentativeId");
        var onSelect = function (e) {
            utility.SetKendoAutoCompleteSourceforValidate($("#pnlPatientAccountManager #frmPatAccountManager #txtFirstName"), e.FullName.split(', ')[1], hfCtrl, e.id);
            utility.SetKendoAutoCompleteSourceforValidate($("#pnlPatientAccountManager #frmPatAccountManager #txtLastName"), e.FullName.split(',')[0], hfCtrl, e.id);
        }
        var onChange = function (valid) {
            if (!valid) {
                $("#pnlPatientAccountManager #frmPatAccountManager #txtFirstName").val('');
                $("#pnlPatientAccountManager #frmPatAccountManager #txtLastName").val('');
            }
        }
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },

    GetFamilyArray: function (AccountNo, FirstName, LastName) {
        var isValid = false;
        var familyList = [];
        if ((AccountNo && AccountNo.length > 2) || (FirstName && FirstName.length > 2) || (LastName && LastName.length > 2))
            isValid = true;

        var dfd = new $.Deferred();
        if (isValid) {
            Patient_AccountManager.LoadFamilyArray_DBCall(AccountNo, FirstName, LastName).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.FamilyCount > 0) {
                        var PatientLoadJSONData = JSON.parse(responseData.FamilyLoad_JSON);
                        $.each(PatientLoadJSONData, function (i, item) {
                            if (AccountNo)
                                familyList.push({ id: item.RepresentativeId, value: item.AccountNumber, AccountNumber: item.AccountNumber, FullName: item.FullName });
                            if (FirstName)
                                familyList.push({ id: item.RepresentativeId, value: item.FullName.split(', ')[1], AccountNumber: item.AccountNumber, FullName: item.FullName });
                            if (LastName)
                                familyList.push({ id: item.RepresentativeId, value: item.FullName.split(',')[0], AccountNumber: item.AccountNumber, FullName: item.FullName });
                        });
                    }
                }
                dfd.resolve(familyList);
            });
        }
        else {
            dfd.resolve(familyList);
        }
        return dfd.promise();
    },

    LoadFamilyArray_DBCall: function (AccountNo, FirstName, LastName) {
        var data = "PatientID=" + Patient_AccountManager.params.PatientId + "&AccountNo=" + AccountNo + "&FirstName=" + FirstName + "&LastName=" + LastName;
        return MDVisionService.defaultService(data, "PATIENT_FAMILY", "LOOKUP_PATIENT_FAMILY");
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNumber, FirstName, LastName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#" + Patient_AccountManager.params.PanelID + " #hfPatientRepresentativeId").val(PatientId);
        $("#" + Patient_AccountManager.params.PanelID + " #txtAccountNo").val(AccountNumber);
        $("#" + Patient_AccountManager.params.PanelID + " #txtFirstName").val(FirstName);
        $("#" + Patient_AccountManager.params.PanelID + " #txtLastName").val(LastName);
        UnloadActionPan("Patient_AccountManager");
    },

    FillFamilyInfoFromSearch: function (PatientId, AccountNumber, FirstName, LastName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var hfCtrl = $("#" + Patient_AccountManager.params.PanelID + " #hfPatientRepresentativeId");
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Patient_AccountManager.params.PanelID + " #frmPatAccountManager #txtAccountNo"), AccountNumber, hfCtrl, PatientId);
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Patient_AccountManager.params.PanelID + " #frmPatAccountManager #txtFirstName"), FirstName, hfCtrl, PatientId);
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Patient_AccountManager.params.PanelID + " #frmPatAccountManager #txtLastName"), LastName, hfCtrl, PatientId);
        UnloadActionPan("Patient_AccountManager");
    },

    PatientRepresentativeSave: function () {
        if ($("#pnlPatientAccountManager #pnlPatientRepresentative_Result").css("display") == "none") {
            $("#pnlPatientAccountManager #pnlPatientRepresentative_Result").show();
            //$("#pnlPatientAccountManager #divPatRepresentative").show();
        }
        else {
            var self = $("#pnlPatientAccountManager");
            var patientRepresentative = self.find('#pnlPatientRepresentative_Result #hfPatientRepresentativeId').val();
            if (!patientRepresentative) {
                utility.DisplayMessages("Please select the Representative first.", 3);
            }
            else {
                var myJSON = self.getMyJSON();
                Patient_AccountManager.SavePatientRepresentative(myJSON).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        Patient_AccountManager.PatientRepresentativeSearch();
                        $("#" + Patient_AccountManager.params.PanelID + " #hfPatientRepresentativeId").val("");
                        $("#" + Patient_AccountManager.params.PanelID + " #txtAccountNo").val("");
                        $("#" + Patient_AccountManager.params.PanelID + " #txtFirstName").val("");
                        $("#" + Patient_AccountManager.params.PanelID + " #txtLastName").val("");
                        $("#" + Patient_AccountManager.params.PanelID + " #ddlRelation").val("");
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
    },

    SavePatientRepresentative: function (RepresentativeData) {
        var data = "RepresentativeData=" + RepresentativeData;
        return MDVisionService.defaultService(data, "PATIENT_ACCOUNT_MANAGER", "SAVE_PATIENT_REPRESENTATIVE");
    },

    PatientRepresentativeSearch: function () {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment Status", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        //if ($("#pnlPatientAccountManager #pnlPatientRepresentative_Result").css("display") == "none") {
        //    $("#pnlPatientAccountManager #pnlPatientRepresentative_Result").show();
        //}

        Patient_AccountManager.SearchPatientRepresentative().done(function (response) {
            if (response.status != false) {

                if (response.PatientRepresentativeCount == 0) {
                    $("#pnlPatientAccountManager #pnlPatientRepresentative_Result").hide();
                    //$("#pnlPatientAccountManager #divPatRepresentative").hide();
                }
                Patient_AccountManager.PatientRepresentativeGridLoad(response);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
                $("#pnlPatientAccountManager #pnlPatientRepresentative_Result").hide();
                //$("#pnlPatientAccountManager #divPatRepresentative").hide();
            }
        });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    PatientRepresentativeGridLoad: function (response) {
        $("#dgvPatRepresentative").dataTable().fnDestroy();
        $("#pnlPatientRepresentative_Result #dgvPatRepresentative tbody").find("tr").remove();

        $("#dgvPatRepresentative").dataTable().fnClearTable();
        $("#dgvPatRepresentative").dataTable().fnDestroy();


        if (response.PatientRepresentativeCount > 0) {
            var PatientRepresentativeLoadJSONData = JSON.parse(response.PatientRepresentativeLoad_JSON);
            $.each(PatientRepresentativeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.PatRepresentativeId);

                var actions = "";
                $("#pnlAccountManager_result #dgvPatRepresentative tr th").each(function () {
                    if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                        var arrActionType = [];
                        if ($(this).attr("ActionType") != null) {
                            arrActionType = $(this).attr("ActionType").split(',');
                            //arrActionType = jQuery.grep(arrActionType, function (value) {
                            //    return value != "Delete";
                            //});
                            actions = EditableGrid.GetActions(arrActionType);
                        }
                    }
                });

                $row.append('<td style="display:none;">' + item.PatRepresentativeId + '</td><td class="action" id="' + item.PatRepresentativeId + '" >' + actions + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.DOB.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.Address1 + '</td><td class="ddl">' + item.RelationName + '</td><td>' + (item.ZipCode ? item.ZipCode : "N/A") + '</td><td>' + (item.IsHealthRecordPrivilege ? item.IsHealthRecordPrivilege : false) + '</td>');


                $("#pnlPatientRepresentative_Result #dgvPatRepresentative tbody").last().append($row);
            });
        }
        //if (!Patient_AccountManager.isFirstEditableGridLoad) {
        var PanelGrid = "#pnlPatientRepresentative_Result";
        var GridId = "#dgvPatRepresentative";
        utility.MakeEditableGrid(PanelGrid, GridId, Patient_AccountManager, "0");
        Patient_AccountManager.isFirstEditableGridLoad = true;
        //}
    },

    SearchPatientRepresentative: function () {
        var patId = Patient_AccountManager.params.PatientId;
        var data = "PatientId=" + patId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ACCOUNT_MANAGER", "SEARCH_PATIENT_REPRESENTATIVE");
    },

    UnLoadTab: function (Tab) {
        utility.UnLoadDialog('frmPatAccountManager', function () {


            if (Patient_AccountManager.params["FromAdmin"] == "0") {
                if (Patient_AccountManager.params != null && Patient_AccountManager.params.ParentCtrl != null) {
                    UnloadActionPan(Patient_AccountManager.params.ParentCtrl, 'Patient_AccountManager');
                }
                else
                    UnloadActionPan(null, 'Patient_AccountManager');

            }
            else {
                RemoveAdminTab();
            }

        }, function () {

        });
        Patient_AccountManager.isFirstEditableGridLoad = false;
    },


    //PrintLetter: function () {
    //    var firstName = Patient_AccountManager.params.PatientFirstName;
    //    var lastName = Patient_AccountManager.params.PatientLastName;
    //    var dob = Patient_AccountManager.params.PatientDOB;
    //    dob = dob.replace(/\//g, "");
    //    var params = [];
    //    params["FromAdmin"] = "0";
    //    params["UserName"] = firstName + '.' + lastName + dob;
    //    params["Password"] = 'sovereignhealth#1';
    //    params["ParentCtrl"] = 'Patient_AccountManager';
    //    params["FirstName"] = firstName;
    //    params["LastName"] = lastName;
    //    LoadActionPan('Patient_AccountManager_Detail', params);

    //},

    //----------------- Start Editable Grid Functions

    rowSave: function ($row, obj) {

        if (obj.rowValidate($row)) {

            var _self = obj,
            $actions,
            values = [];

            if ($row.hasClass('adding')) {
                $row.removeClass('adding');
            }

            values = $row.find('td').map(function () {

                var $this = $(this);

                if ($this.hasClass('expand')) {
                    return '<a href="#" class="hidden on-editing expand-row" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>';
                }
                else if ($this.hasClass('actions')) {

                    return _self.datatable.cell(this).data();
                }
                else if ($this.hasClass("ddl")) {
                    //if ($this.find("select").attr("id") == "ddlRelation" + $($row).attr("id"))
                    return $.trim($this.find('select option:selected').text());

                } else {
                    $obj_ = $this.find('input');

                    if ($obj_.attr('type') == "checkbox") {
                        if ($obj_.prop('checked'))
                            return $.trim("True");
                        else
                            return $.trim("False");
                    }
                    else
                        return $.trim($obj_.val());
                }
            });


            var relationShipID = $row.find('td:eq(6) select option:selected').val();
            var IsHealthRecordAccess = $row.find('td:eq(8) input:checkbox').prop('checked');
            var id = $row.attr("id");
            var myJSON = $row.getMyJSON();

            if (id && id > 0) {
                //Edit Record
                var strMessage = "";
                //AppPrivileges.GetFormPrivileges("Type Of Service", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                //    if (strMessage == "") {

                Patient_AccountManager.UpdatePatientRepresentative(id, relationShipID, IsHealthRecordAccess).done(function (response) {
                    if (response.status != false) {

                        utility.DisplayMessages(response.message, 1);
                        Patient_AccountManager.rowDraw($row, _self, values);
                        //Patient_AccountManager.PatientRepresentativeSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
                //    }
                //});
            }
        }
    },

    rowAdd: function () {

        AppPrivileges.GetFormPrivileges("Type Of Service", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd("-" + $('#' + typeOfServiceDetail.params.PanelID + ' #dgvTOSPlanInfo tr').length);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowRemove: function ($row, obj) {

        var id = $row.attr("id");
        if (Patient_AccountManager.deletedrowID != id) {
            AppPrivileges.GetFormPrivileges("Patient Portal Account", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    utility.myConfirm('1', function () {
                        Patient_AccountManager.deletedrowID = 0;
                        var selectedValue = id;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Patient_AccountManager.DeletePatientRepresentative(selectedValue).done(function (response) {
                                if (response.status != false) {

                                    if ($row.hasClass('adding')) {
                                    }
                                    var _self = obj;
                                    _self.datatable.row($row.get(0)).remove().draw();
                                    //Patient_AccountManager.PatientRepresentativeSearch();
                                    //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                    utility.removePaginationFromGrid($('#' + Patient_AccountManager.params.PanelID + ' #pnlAccountManager_result'));
                                    //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537

                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () {
                        Patient_AccountManager.deletedrowID = 0;
                    },
                        '1'
                    );

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        Patient_AccountManager.deletedrowID = id;
    },

    rowCancel: function ($row, obj) {


        var _self = obj,
            $actions,
            i,
            data;

        if ($row.hasClass('adding')) {
            _self.datatable.row($row.get(0)).remove().draw();

        } else {

            data = _self.datatable.row($row.get(0)).data();
            _self.datatable.row($row.get(0)).data(data);

            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                _self.rowSetActionsDefault($row);
            }

            _self.datatable.draw();
        }
    },

    rowDraw: function ($row, _self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },

    UpdatePatientRepresentative: function (id, relationShipID, IsHealthRecordAccess) {
        var data = "RepresentativeId=" + id + "&RelationShipId=" + relationShipID + "&HealthPrivilegeBit=" + IsHealthRecordAccess;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ACCOUNT_MANAGER", "UPDATE_PATIENT_REPRESENTATIVE");

    },

    DeletePatientRepresentative: function (representativeId) {

        var data = "RepresentativeId=" + representativeId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ACCOUNT_MANAGER", "DELETE_PATIENT_REPRESENTATIVE");

    },
    //-------------------Editable Grid Methods Ends---

    ResetPassword: function () {
        Patient_AccountManager.PasswordReset().done(function (response) {

            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
            } else {
                utility.DisplayMessages(response.message, 3);
            }

        });
    },

    PasswordReset: function () {
        var patientId = $('#pnlPatientAccountManager #hfPatientId').val();
        var patientEmail = $('#pnlPatientAccountManager #txtAccountEmail').val();

        var data = "PatientId=" + patientId + "&PatientEmail=" + patientEmail;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ACCOUNT_MANAGER", "RESET_PATIENT_PASSWORD");
    },
    loadHeaderTemplate: function () {
        var patientId = $('#pnlPatientAccountManager #hfPatientId').val();
        var providerId = $("#pnlDemographic #hfProvider").val();
        var data = "PatientId=" + patientId + "&ProviderId=" + providerId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ACCOUNT_MANAGER", "LOAD_TEMPLATEHEADER_DATA");
    },
    PrintLetter: function () {

        Patient_AccountManager.loadHeaderTemplate().done(function (response) {

            if (response.status != false) {
                var formHeaderHtml = '';
                var formFooterHtml = '';

                var firstName = Patient_AccountManager.params.PatientFirstName;
                var lastName = Patient_AccountManager.params.PatientLastName;
                var dob = Patient_AccountManager.params.PatientDOB;
                var yearOfBirth = dob.split('/')[2] ? dob.split('/')[2] : dob.split('-')[2];
                dob = dob.replace(/\//g, "");
                var firstInitial = Patient_AccountManager.params.PatientFirstName.charAt(0);

                $('#pnlPatientAccountManager #spanUserName').html(firstInitial + lastName + yearOfBirth);
                $('#pnlPatientAccountManager #spanUserPassword').html($("#txtpassword").val());
                $('#pnlPatientAccountManager #userName').html("Welcome " + lastName + ", " + firstName + "!");
                $('#pnlPatientAccountManager #spanPPLink').html(Patient_AccountManager.PatientPortalURL);

                //-------------------------
                $('#pnlPatientAccountManager #spnFacilityAddress').html(Patient_AccountManager.params.FacilityAddress);
                $('#pnlPatientAccountManager #spnFacilityCityState').html(Patient_AccountManager.params.FacilityCity + ", " + Patient_AccountManager.params.FacilityState + " " + Patient_AccountManager.params.FacilityZip + "-" + Patient_AccountManager.params.FacilityZipExt);
                $('#pnlPatientAccountManager #spnFacilityTel').html(Patient_AccountManager.params.FacilityPhone);
                $('#pnlPatientAccountManager #spnProviderName').html(Patient_AccountManager.params.ProviderFName + " " + Patient_AccountManager.params.ProviderLName);

                //-------------------------

                var docType = '<!doctype html>';
                var docCnt = $("#pnlPatientAccountManager #printLetter").html();
                if (response.ReportHeaderInfo != "" && response.ReportFooterInfo != "") {

                    formHeaderHtml = response.ReportHeaderInfo;
                    formFooterHtml = response.ReportFooterInfo;
                    $("#pnlPatientAccountManager #printLetter header").remove();
                    $("#pnlPatientAccountManager #printLetter footer").remove();
                    var docCnt = $("#pnlPatientAccountManager #printLetter").html();
                }

                var docHead = '<head> <script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                              + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
                              + '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;utility.myConfirm("Do you want to submit statement ?", function () {window.opener.Bill_PatientStatement.saveStatements("false");}, function () {},"<b>Confirm submission</b>");window.print(); } '
                              + '</script>'
                              + '</head>';
                var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=865, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                var newWin = window.open("", "_blank", winAttr);
                writeDoc = newWin.document;
                writeDoc.open();
                writeDoc.write(docType + '<html>' + docHead + '<body>' + formHeaderHtml + docCnt + formFooterHtml + '</body></html>');
                //writeDoc.close();
                var checkForContent = function () {
                    setTimeout(function () {
                        var content = newWin.document.querySelector('body');

                        if (content && content.innerHTML.length) {
                            writeDoc.close();
                            newWin.focus();
                            newWin.print();
                        } else {
                            checkForContent();
                        }
                    }, 200);
                };
                checkForContent();
            }

        });

    },
}